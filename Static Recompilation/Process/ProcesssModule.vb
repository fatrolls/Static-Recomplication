
Imports System.Collections.Generic
Imports System.Text
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Threading


Public Class ProcesssModule
    Private m_Process As Process
    Private m_ModuleName As String
    Private m_BaseAddress As IntPtr
    Private m_EntryPointAddress As IntPtr
    Private m_FileName As String
    Private m_ModuleMemorySize As UInteger

#Region "Constructor"

    Friend Sub New()
    End Sub
#End Region

#Region "Properties"
    Public ReadOnly Property ModuleName() As String
        Get
            Return m_ModuleName
        End Get
    End Property

    Public ReadOnly Property BaseAddress() As IntPtr
        Get
            Return m_BaseAddress
        End Get
    End Property

    Public ReadOnly Property EntryPointAddress() As IntPtr
        Get
            Return m_EntryPointAddress
        End Get
    End Property

    Public ReadOnly Property FileName() As String
        Get
            Return m_FileName
        End Get
    End Property


    Public ReadOnly Property ModuleMemorySize() As UInteger
        Get
            Return m_ModuleMemorySize
        End Get
    End Property
#End Region

#Region "Methods"
    Friend Shared Function GetModule(ByVal process As Process, ByVal lpAddress As IntPtr, ByVal wowProcess As Boolean, ByVal realSize As UInteger) As ProcesssModule
        Dim bsm As New ProcesssModule()
        bsm.m_Process = process

        Dim info As New MODULEINFO()
        If Not PSAPI.GetModuleInformation(process.Handle, lpAddress, info, CUInt(Marshal.SizeOf(info))) Then
            Return Nothing
        End If

        Dim sbFileName As New StringBuilder(1024)
        If PSAPI.GetModuleFileNameEx(process.Handle, lpAddress, sbFileName, 1024) = 0 Then
            Return Nothing
        End If

        Dim fileName As String = sbFileName.ToString()
        Dim moduleName As String = Path.GetFileName(fileName)
        If wowProcess Then
            Dim upperName As String = moduleName.ToUpper()
            If upperName <> "NTDLL.DLL" AndAlso upperName <> "WOW64.DLL" AndAlso upperName <> "WOW64CPU.DLL" AndAlso upperName <> "WOW64WIN.DLL" AndAlso fileName.ToUpper().StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.System).ToUpper()) Then
                Dim newFileName As String = fileName.ToLower().Replace("system32", "syswow64")
                If File.Exists(newFileName) Then
                    fileName = newFileName
                    moduleName = Path.GetFileName(fileName)
                End If
            End If
        End If

        bsm.m_BaseAddress = lpAddress
        bsm.m_EntryPointAddress = info.EntryPoint
        bsm.m_FileName = fileName
        bsm.m_ModuleMemorySize = realSize
        bsm.m_ModuleName = moduleName
        Return bsm
    End Function

    Public Function GetBytes() As Byte()
        Dim bytes As Byte() = New Byte(m_ModuleMemorySize - 1) {}
        Using memStream As New ProcessMemoryStream(m_Process.Id)
            memStream.Seek(m_BaseAddress.ToInt64(), SeekOrigin.Begin)
            memStream.Read(bytes, 0, bytes.Length)
        End Using
        Return bytes
    End Function
#End Region
End Class
