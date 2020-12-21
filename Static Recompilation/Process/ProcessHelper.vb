
Imports System.Collections.Generic
Imports System.Text
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Threading

Public NotInheritable Class ProcessHelper
    Private Sub New()
    End Sub
    Private Const PROCESS_ALL_ACCESS As Integer = (STANDARD_RIGHTS_REQUIRED Or SYNCHRONIZE Or &HFFF)
    Private Const STANDARD_RIGHTS_REQUIRED As Integer = &HF0000
    Private Const SYNCHRONIZE As Integer = &H100000
    Private Const MEM_COMMIT As Integer = &H1000
    Private Const PAGE_READWRITE As Integer = &H4
    Private Const STILL_ACTIVE As Integer = STATUS_PENDING
    Private Const STATUS_PENDING As Integer = &H103
    Private Const MEM_DECOMMIT As Integer = &H4000
    Private Const MEM_RELEASE As Integer = &H8000
    Private Const CREATE_SUSPENDED As Integer = &H4

    Public Shared Function GetModules(ByVal process As Process) As List(Of ProcesssModule)
        Dim list As New List(Of ProcesssModule)()
        Dim wowProcess As Boolean = False
        wowProcess = IsWow64Process(process)
        Dim snapshot As New MemorySnapshot(process.Handle)
        For Each reg As MemoryRegion In snapshot.Regions
            If reg.Type = MemoryType.MEM_IMAGE Then
                Dim bsm As ProcesssModule = ProcesssModule.GetModule(process, reg.RegionAddress, wowProcess, CUInt(reg.RegionSize.ToInt32()))
                If bsm IsNot Nothing Then
                    list.Add(bsm)
                End If
            End If
        Next
        Return list
    End Function
    Public Shared Function GetProcessBasicInfo(ByVal process As Process) As NtProcessBasicInfo
        Dim info As New NtProcessBasicInfo()
        Try
            If NtDll.NtQueryInformationProcess(process.Handle, 0, info, Marshal.SizeOf(info), Nothing) = 0 Then
                Return info
            End If
        Catch err As Exception
            Trace.WriteLine(err)
        End Try
        Return Nothing
    End Function
    Public Shared Function IsWow64Process(ByVal process As Process) As Boolean
        Dim bRunningInWow As Boolean = False
        Try
            If Kernel32.IsWow64Process(process.Handle, bRunningInWow) Then
                Return bRunningInWow
            End If
        Catch
        End Try
        Return False
    End Function
    Public Shared Function GetPEFiles(ByVal process As Process) As Dictionary(Of IntPtr, PEImage)
        Dim files As New Dictionary(Of IntPtr, PEImage)()
        For Each [module] As ProcesssModule In GetModules(process)
            Try
                Dim image As New PEImage([module].FileName)
                files.Add([module].BaseAddress, image)
            Catch err As Exception
                Trace.WriteLine(err)
            End Try
        Next
        Return files
    End Function
    Public Shared Function GetRemoteExport(ByVal process As Process, ByVal DllName As String, ByVal ApiName__1 As String, ByVal ordinal As UInteger) As ImageExport
        Dim img As PEImage = Nothing
        Dim remoteModule As ProcesssModule = GetRemoteLibrary(process, DllName)
        If remoteModule Is Nothing Then
            Throw New Exception(String.Format("Could not locate DLL {0} in process {1}", DllName, process.ProcessName))
        End If
        Dim export As ImageExport = Nothing
        Dim apiName__2 As String = ApiName__1.ToLower()
        If img.HasDirectoryEntry(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_EXPORT) Then
            For Each exp As ImageExport In img.ExportDirectory.Exports
                If exp.Name.ToLower() = apiName__2 OrElse exp.Ordinal = ordinal Then
                    export = exp
                    Exit For
                End If
            Next
        End If
        Return export
    End Function
    Public Shared Function GetRemoteLibrary(ByVal process As Process, ByVal ModuleName As String) As ProcesssModule
        Dim myModule As ProcesssModule = Nothing
        Dim dllName As String = ModuleName.ToLower()
        Dim modules As List(Of ProcesssModule) = GetModules(process)
        For Each [module] As ProcesssModule In modules
            If [module].ModuleName.ToLower() = dllName Then
                myModule = [module]
                Exit For
            End If
        Next
        Return myModule
    End Function
    Public Shared Function GetRemoteLibraryByFileName(ByVal process As Process, ByVal FileName As String) As ProcesssModule
        Dim myModule As ProcesssModule = Nothing
        Dim dllName As String = FileName.ToLower()
        Dim modules As List(Of ProcesssModule) = GetModules(process)
        For Each [module] As ProcesssModule In modules
            If [module].FileName.ToLower() = dllName Then
                myModule = [module]
                Exit For
            End If
        Next
        Return myModule
    End Function
    Public Shared Function GetRemoteAddress(ByVal process As Process, ByVal ModuleName As String, ByVal ApiName__1 As String, ByVal ordinal As UInteger) As IntPtr
        Dim img As PEImage = Nothing
        Dim remoteModule As ProcesssModule = GetRemoteLibrary(process, ModuleName)
        If remoteModule Is Nothing Then
            Throw New Exception(String.Format("Could not locate DLL {0} in process {1}", ModuleName, process.ProcessName))
        End If
        img = PEImage.ReadImage(remoteModule.FileName)

        Dim export As ImageExport = Nothing
        Dim apiName__2 As String = ApiName__1.ToLower()
        If img.HasDirectoryEntry(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_EXPORT) Then
            For Each exp As ImageExport In img.ExportDirectory.Exports
                If exp.Name.ToLower() = apiName__2 OrElse exp.Ordinal = ordinal Then
                    export = exp
                    Exit For
                End If
            Next
        End If
        Return New IntPtr(remoteModule.BaseAddress.ToInt64() + export.AddressRVA)
    End Function
    Public Shared Function LoadRemoteLibrary(ByVal process As Process, ByVal Dll2Inject As String) As Boolean
        Dim lpModule As IntPtr = IntPtr.Zero
        Return LoadRemoteLibrary(process, Dll2Inject, lpModule)
    End Function
    Public Shared Function LoadRemoteLibrary(ByVal process As Process, ByVal Dll2Inject As String, ByRef lpModule As IntPtr) As Boolean
        lpModule = IntPtr.Zero

        If Not File.Exists(Dll2Inject) Then
            Throw New FileNotFoundException(String.Format("PE File '{0}' not found.", Dll2Inject))
        End If

        Dim enc As New UnicodeEncoding()
        Dim rawdllStr As Byte() = enc.GetBytes(Dll2Inject)
        Dim aDllBytes As Byte() = New Byte(rawdllStr.Length + 1) {}
        rawdllStr.CopyTo(aDllBytes, 0)
        aDllBytes(aDllBytes.Length - 2) = 0
        aDllBytes(aDllBytes.Length - 1) = 0

        Dim lpLoadAddress As IntPtr = ProcessHelper.GetRemoteAddress(process, "Kernel32.dll", "LoadLibraryW", 0)
        If lpLoadAddress = IntPtr.Zero Then
            Throw New Exception("Could not resolve remote address")
        End If

        ' Allocate memory for the string in the target process
        Dim lpDllString As IntPtr = Kernel32.VirtualAllocEx(process.Handle, IntPtr.Zero, CUInt(aDllBytes.Length), MEM_COMMIT, PAGE_READWRITE)

        If lpDllString = IntPtr.Zero Then
            Throw New Exception("VirtualAllocEx failed with error code " + Convert.ToString(Marshal.GetLastWin32Error()) + ".")
        End If

        Try
            Dim bytesWritten As IntPtr
            If Not Kernel32.WriteProcessMemory(process.Handle, lpDllString, aDllBytes, New IntPtr(aDllBytes.Length), bytesWritten) Then
                Throw New Exception("WriteProcessMemory failed with error code " + Convert.ToString(Marshal.GetLastWin32Error()) + ".")
            End If

            Dim ThreadID As UInteger = 0
            Dim hRemoteThread As IntPtr = Kernel32.CreateRemoteThread(process.Handle, IntPtr.Zero, IntPtr.Zero, lpLoadAddress, lpDllString, CREATE_SUSPENDED, _
             ThreadID)

            If hRemoteThread = IntPtr.Zero Then
                Throw New Exception("CreateRemoteThread failed with error code " + Convert.ToString(Marshal.GetLastWin32Error()) + ".")
            End If
            ' Hide from debugger
            Dim exitCode As UInteger
            While True
                Dim result As Integer = NtDll.NtSetInformationThread(hRemoteThread, 17, IntPtr.Zero, 0, IntPtr.Zero)
                Kernel32.ResumeThread(hRemoteThread)

                If Not Kernel32.GetExitCodeThread(hRemoteThread, exitCode) Then
                    Throw New Exception("GetExitCodeThread failed with error code " + Convert.ToString(Marshal.GetLastWin32Error()) + ".")
                End If

                If exitCode <> STILL_ACTIVE Then
                    Exit While
                Else
                    Thread.Sleep(10)
                End If
            End While
            If exitCode = 0 Then
                Throw New Exception("LoadLibraryW in remote process failed.")
            End If

            lpModule = ProcessHelper.GetRemoteLibraryByFileName(process, Dll2Inject).BaseAddress
            Kernel32.CloseHandle(hRemoteThread)
        Finally
            Dim btest As Boolean = Kernel32.VirtualFreeEx(process.Handle, lpDllString, IntPtr.Zero, MEM_RELEASE)
        End Try
        Return True
    End Function
    Public Shared Function FreeRemoteLibrary(ByVal process As Process, ByVal hModule As IntPtr) As Boolean
        Dim lpLoadAddress As IntPtr = ProcessHelper.GetRemoteAddress(process, "Kernel32.dll", "FreeLibraryAndExitThread", 0)
        If lpLoadAddress = IntPtr.Zero Then
            Throw New Exception("Could not resolve remote address!")
        End If

        Dim ThreadID As UInteger = 0
        Dim hRemoteThread As IntPtr = Kernel32.CreateRemoteThread(process.Handle, IntPtr.Zero, IntPtr.Zero, lpLoadAddress, hModule, 0, _
         ThreadID)

        If hRemoteThread = IntPtr.Zero Then
            Throw New Exception("CreateRemoteThread failed with error code " + Convert.ToString(Marshal.GetLastWin32Error()) + ".")
        End If

        NtDll.NtSetInformationThread(hRemoteThread, 17, IntPtr.Zero, 0, IntPtr.Zero)
        Kernel32.ResumeThread(hRemoteThread)


        Kernel32.CloseHandle(hRemoteThread)
        Return True
    End Function

End Class