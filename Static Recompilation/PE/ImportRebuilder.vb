
Imports System.Collections.Generic
Imports System.Text
Imports System.Diagnostics
Imports System.IO
Imports System.Collections

Public Class IatRebuilder
#Region "Helper classes"
    Public Class ApiThunk
#Region "Fields"
        Private m_Rebuilder As IatRebuilder
        Private m_Table As ApiTableThunk
        Private m_Rva As UInteger
        Private m_Address As Long
        Private m_Export As ImageExport
        Private m_PossibleExports As ImageExport()
#End Region

#Region "Constructor"
        Friend Sub New(ByVal rebuilder As IatRebuilder, ByVal table As ApiTableThunk, ByVal rva As UInteger, ByVal address As Long)
            m_Rebuilder = rebuilder
            m_Table = table

            m_Rva = rva
            m_Address = address
            m_PossibleExports = m_Rebuilder.GetExports(address)
            For Each exp As ImageExport In m_PossibleExports
                If exp.Directory.DllName = Me.m_Table.DllName Then
                    m_Export = exp
                    Exit For
                End If
            Next
        End Sub
#End Region

#Region "Properties"
        Public ReadOnly Property Export() As ImageExport
            Get
                Return m_Export
            End Get
        End Property
        Public ReadOnly Property PossibleExports() As ImageExport()
            Get
                Return m_PossibleExports
            End Get
        End Property
        Public ReadOnly Property Rva() As UInteger
            Get
                Return m_Rva
            End Get
        End Property
        Public Property Address() As Long
            Get
                Return m_Address
            End Get
            Set(ByVal value As Long)
                m_Address = value
                m_PossibleExports = m_Rebuilder.GetExports(value)
                ' Set Possible exports
                m_Table.CalcModule()
                ' ReCalculate complete table
                For Each exp As ImageExport In m_PossibleExports
                    If exp.Directory.DllName = Me.m_Table.DllName Then
                        m_Export = exp
                        Exit For
                    End If

                Next
            End Set
        End Property
        Public ReadOnly Property ThunkTable() As ApiTableThunk
            Get
                Return m_Table
            End Get
        End Property
        Public ReadOnly Property IsValid() As Boolean
            Get
                If m_Export Is Nothing Then
                    Return False
                End If
                If m_Export.Directory.DllName <> Me.ThunkTable.DllName Then
                    Return False
                End If
                Return m_PossibleExports IsNot Nothing AndAlso m_PossibleExports.Length > 0
            End Get
        End Property
#End Region
        Public Sub Reset()
            m_Export = Nothing
        End Sub
        Friend Sub SetExport(ByVal exp As ImageExport)
            m_Export = exp
        End Sub
    End Class

    Public Class ApiTableThunk
        Private m_DllName As String = ""
        Public Rva As UInteger
        Public DllModules As New List(Of String)()
        Public APIs As New List(Of ApiThunk)()

        Public Property DllName() As String
            Get
                Return m_DllName
            End Get
            Set(ByVal value As String)
                SetModule(value)
            End Set
        End Property
        Public ReadOnly Property IsValid() As Boolean
            Get
                If DllName = "" Then
                    Return False
                End If

                For Each api As ApiThunk In APIs
                    If Not api.IsValid Then
                        Return False
                    End If
                Next
                Return True
            End Get
        End Property

        Public Sub CalcModule()
            SetModule(GetDllName())
        End Sub

        Friend Function GetDllName() As String
            Dim iMax As Integer = 0
            Dim dllName As String = ""
            Dim stat As New Dictionary(Of String, Integer)()
            Me.DllModules.Clear()

            For Each api As ApiThunk In Me.APIs
                For Each exp As ImageExport In api.PossibleExports
                    If Not stat.ContainsKey(exp.Directory.DllName) Then
                        stat.Add(exp.Directory.DllName, 1)
                    Else
                        ' Inital
                        stat(exp.Directory.DllName) += 1
                    End If
                    ' Increment
                    If stat(exp.Directory.DllName) > iMax Then
                        iMax = stat(exp.Directory.DllName)
                        dllName = exp.Directory.DllName
                    End If

                    ' Add possible module
                    If Not Me.DllModules.Contains(exp.Directory.DllName) Then
                        Me.DllModules.Add(exp.Directory.DllName)

                    End If
                Next
            Next
            Return dllName
        End Function

        Public Sub SetModule(ByVal dllName As String)
            Me.m_DllName = dllName

            ' now set the apis
            For Each api As ApiThunk In Me.APIs
                'api.SetExport(null);
                For Each exp As ImageExport In api.PossibleExports
                    If exp.Directory.DllName = dllName Then
                        api.SetExport(exp)
                        Exit For
                    End If
                Next
            Next
        End Sub
    End Class

    Public Class ImageExportCollection
        Inherits List(Of ImageExport)

    End Class
#End Region

#Region "Fields"
    Public Delegate Sub LogEventHandler(ByVal sender As Object, ByVal message As String)
    Public Event LogMessage As LogEventHandler

    Private m_Process As Process
    Private m_x64 As Boolean = False
    Private m_IatEntrySize As Integer = 4

    ' List of all know addresses in the address space
    Private m_KnownAddresses As New Dictionary(Of Long, ImageExportCollection)()
    Private m_ProcessAPIs As New SortedList(Of UInteger, Long)()
    Private m_ApiTables As List(Of ApiTableThunk) = Nothing
#End Region

#Region "Constructor"
    Public Sub New(ByVal process As Process, ByVal x64 As Boolean)
        m_Process = process
        m_x64 = x64
        If m_x64 Then
            m_IatEntrySize = 8
        Else
            m_IatEntrySize = 4
        End If
    End Sub
#End Region

#Region "Properies"
    Public ReadOnly Property FThunks() As ApiTableThunk()
        Get
            If m_ApiTables Is Nothing Then
                BuildThunkTables()
            End If
            Return m_ApiTables.ToArray()
        End Get
    End Property
    Public ReadOnly Property IsValid() As Boolean
        Get
            If m_ApiTables Is Nothing Then
                Return False
            End If
            For Each th As ApiTableThunk In m_ApiTables
                If Not th.IsValid Then
                    Return False
                End If
            Next
            Return True
        End Get
    End Property
#End Region

#Region "Methods"

#Region "Public"
    Public Sub Init()
        m_KnownAddresses.Clear()
        Dim forwardedFunctions As New List(Of ImageExport)()
        Dim modules As List(Of ProcesssModule) = ProcessHelper.GetModules(m_Process)
        Dim loadedModules As New List(Of ProcesssModule)()

        Dim imgMain As PEImage = PEImage.ReadImage(m_Process.MainModule.FileName)
        For Each oModule As ProcesssModule In modules
            Try
                Dim [module] As PEImage = PEImage.ReadImage(oModule.FileName)
                ' Don't mix x64 with x86
                If imgMain.PeHeader.OptionalHeader.Magic <> [module].PeHeader.OptionalHeader.Magic Then
                    OnLog(String.Format("Skipping: {0}", [module].FileName))
                    Continue For
                End If
                loadedModules.Add(oModule)

                If [module].HasDirectoryEntry(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_EXPORT) Then
                    For Each exp As ImageExport In [module].ExportDirectory.Exports
                        If exp.AddressRVA = 0 Then
                            Continue For
                        End If

                        ' Not sure anymore
                        'SectionHeader header = module.GetSectionFromRva(exp.AddressRVA);
                        'if ((header.Characteristics & SectionCharacteristics.IMAGE_SCN_MEM_EXECUTE) != SectionCharacteristics.IMAGE_SCN_MEM_EXECUTE)
                        '    continue;
                        ' When functions are exported the RVA lies int the .rdata section.

                        If exp.IsForwarded Then
                            If exp.Forwarder.ToUpper().IndexOf("SHUNIMPL") = -1 Then
                                forwardedFunctions.Add(exp)
                            End If
                        Else
                            Dim VA As Long = oModule.BaseAddress.ToInt64() + exp.AddressRVA
                            If Not m_KnownAddresses.ContainsKey(VA) Then
                                Dim col As New ImageExportCollection()
                                col.Add(exp)
                                m_KnownAddresses.Add(VA, col)
                            End If
                        End If
                    Next
                End If
                OnLog(String.Format("Module loaded: {0}", oModule.FileName))
            Catch err As Exception
                OnLog(String.Format("ERROR: Cannot load module: {0}", oModule.FileName))
                OnLog(String.Format("       => {0}", err.Message))
                Trace.WriteLine(err)
            End Try
        Next


        Dim resolvedFunction As Integer = 0
        Try
            ' Locate the forwarded function
            For Each forwarded As ImageExport In forwardedFunctions
                Dim resolved As Boolean = False

                Dim f As String() = forwarded.Forwarder.Split("."c)

                Dim dll As String = f(0).ToLower() + ".dll"
                Dim [function] As String = f(1).ToLower()
                ' find address
                For Each [mod] As ProcesssModule In loadedModules
                    If [mod].ModuleName.ToLower() = dll Then
                        Dim img As PEImage = PEImage.ReadImage([mod].FileName)
                        If img.HasDirectoryEntry(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_EXPORT) Then
                            For Each exp As ImageExport In img.ExportDirectory.Exports
                                If [function] = exp.Name.ToLower() Then
                                    If exp.IsForwarded = False Then
                                        Dim VA As Long = [mod].BaseAddress.ToInt64() + exp.AddressRVA
                                        Dim col As ImageExportCollection = m_KnownAddresses(VA)
                                        col.Add(forwarded)
                                        resolvedFunction += 1
                                        'OnLog(String.Format("Resolved forwarded API {0} -> {1}", forwarded, forwarded.Forwarder));
                                        resolved = True
                                    End If
                                    Exit For
                                End If
                            Next
                        End If
                        Exit For
                    End If
                Next
                If Not resolved Then
                    OnLog(String.Format("Could not resolve {0} -> {1}", forwarded, forwarded.Forwarder))
                End If
            Next
        Catch err As Exception
            Trace.WriteLine(err)
        End Try
        If forwardedFunctions.Count - resolvedFunction <> 0 Then
            OnLog(String.Format("Could not resolve all forwarded function!" & vbLf & vbTab & "{0} remaining.", (forwardedFunctions.Count - resolvedFunction)))
        End If
    End Sub
    Public Function IsKnownAddress(ByVal address As Long) As Boolean
        Return m_KnownAddresses.ContainsKey(address)
    End Function
    Public Function GetApi(ByVal address As Long) As ImageExportCollection
        If m_KnownAddresses.ContainsKey(address) Then
            Return m_KnownAddresses(address)
        End If
        Return Nothing
    End Function
    Public Sub AddAPI(ByVal rva As UInteger, ByVal address As Long)
        If Not m_ProcessAPIs.ContainsKey(rva) Then
            If address >= MemorySnapshot.SystemInformation.lpMinimumApplicationAddress.ToInt64() AndAlso address < MemorySnapshot.SystemInformation.lpMaximumApplicationAddress.ToInt64() Then
                m_ProcessAPIs.Add(rva, address)
            End If
        End If
    End Sub
    Public Sub DeleteAPI(ByVal rva As UInteger)
        If m_ProcessAPIs.ContainsKey(rva) Then
            m_ProcessAPIs.Remove(rva)
        End If

        If m_ApiTables IsNot Nothing Then
            For Each th As ApiTableThunk In m_ApiTables
                For Each api As ApiThunk In th.APIs
                    If api.Rva = rva Then
                        th.APIs.Remove(api)
                        If th.APIs.Count = 0 Then
                            m_ApiTables.Remove(th)
                        End If
                        Return
                    End If
                Next
            Next
        End If
    End Sub
    Public Function GetApiRva(ByVal address As Long) As UInteger
        Dim index As Integer = m_ProcessAPIs.IndexOfValue(address)
        If index <> -1 Then
            Return m_ProcessAPIs.Keys(index)
        End If
        Return 0
    End Function

    Public Sub SetAPI(ByVal rva As UInteger, ByVal address As Long)
        If Not m_ProcessAPIs.ContainsKey(rva) Then
            Return
        End If

        m_ProcessAPIs(rva) = address
        If m_ApiTables IsNot Nothing Then
            For Each th As ApiTableThunk In m_ApiTables
                For Each api As ApiThunk In th.APIs
                    If api.Rva = rva Then
                        api.Address = address
                        Return
                    End If
                Next
            Next
        End If
    End Sub
    Public Sub ClearAPIs()
        m_ProcessAPIs.Clear()
        If m_ApiTables IsNot Nothing Then
            m_ApiTables.Clear()
            m_ApiTables = Nothing
        End If
    End Sub
    Public Function SearchAPIs(ByVal [module] As ProcesssModule, ByRef IAT_start As Long, ByRef IAT_end As Long) As Boolean
        Try
            Dim img As PEImage = PEImage.ReadImage([module].FileName)
            Dim baseAddres As Long = [module].BaseAddress.ToInt64()
            Dim size As Integer = CInt([module].ModuleMemorySize)
            If size Mod img.PeHeader.OptionalHeader.SectionAlignment <> 0 Then
                size -= size Mod img.PeHeader.OptionalHeader.SectionAlignment
                size += img.PeHeader.OptionalHeader.SectionAlignment
            End If
            Dim max As Long = [module].BaseAddress.ToInt64() + size

            Dim rva0 As New List(Of Long)()
            Dim rvas As New List(Of Long)()

            Dim dmp As Byte() = New Byte(size - 1) {}
            Using pmStream As New ProcessMemoryStream(m_Process.Id)
                pmStream.Seek([module].BaseAddress.ToInt64(), SeekOrigin.Begin)
                pmStream.Read(dmp, 0, dmp.Length)
            End Using
            '
            '0046DCC4                              A1 00004000             mov     eax,UInteger ptr [400000]
            '0046DCC9                              8B1D 00004000           mov     ebx,UInteger ptr [400000]
            '0046DCCF                              8B0D 00004000           mov     ecx,UInteger ptr [400000]
            '0046DCD5                              8B15 00004000           mov     edx,UInteger ptr [400000]
            '0046DCDB                              8B35 00004000           mov     esi,UInteger ptr [400000]
            '0046DCE1                              8B3D 00004000           mov     edi,UInteger ptr [400000]
            '0046DCE7                              8B2D 00004000           mov     ebp,UInteger ptr [400000]
            '0046DCED                              8B25 00004000           mov     esp,UInteger ptr [400000]
            '


            Dim mDmpStream As New MemoryStream(dmp)
            Using dmpReader As New BinaryReader(mDmpStream)
                For Each h As SectionHeader In img.Sections
                    If (h.Characteristics And SectionCharacteristics.IMAGE_SCN_MEM_EXECUTE) = SectionCharacteristics.IMAGE_SCN_MEM_EXECUTE Then
                        Dim mCodeStream As New MemoryStream(dmp, h.VirtualAddress, CInt(h.AlignedVirtualSize))
                        Using codeReader As New BinaryReader(mCodeStream)
                            Dim test As Short = 0
                            While codeReader.BaseStream.Position <= codeReader.BaseStream.Length - 6
                                test = codeReader.ReadInt16()
                                If Not m_x64 Then
                                    ' JMP UInteger PTR[XXXXXXXX] or CALL UInteger PTR[XXXXXXXX]
                                    ' MOV R32, UInteger PTR[XXXXXXXX]
                                    If test = &H15FF OrElse test = &H25FF OrElse test = &H1D8B OrElse test = &HD8B OrElse test = &H158B OrElse test = &H3D8B OrElse test = &H358B OrElse test = &H2D8B Then
                                        Dim myVa As Long = codeReader.ReadInt32()

                                        If myVa >= baseAddres AndAlso myVa <= max Then
                                            dmpReader.BaseStream.Seek(myVa - baseAddres, SeekOrigin.Begin)
                                            Dim address As Long = dmpReader.ReadInt32()
                                            If IsKnownAddress(address) Then
                                                rva0.Add(myVa - baseAddres)
                                            End If
                                        End If
                                    End If
                                Else
                                    If test = &H15FF Then
                                        ' CALL QWORD PTR[XXXXXXXX]
                                        Dim offset As UInteger = codeReader.ReadUInt32()
                                        ' Plus of Min
                                        Dim myVa As Long = baseAddres + codeReader.BaseStream.Position + offset + h.VirtualAddress
                                        If myVa >= baseAddres AndAlso myVa <= max Then
                                            dmpReader.BaseStream.Seek(myVa - baseAddres, SeekOrigin.Begin)
                                            Dim address As Long = dmpReader.ReadInt64()
                                            If IsKnownAddress(address) Then
                                                rva0.Add(myVa - baseAddres)
                                            End If
                                        End If
                                    End If
                                End If

                                codeReader.BaseStream.Position -= 1
                            End While
                        End Using

                        ' Stop after first section
                        Exit For
                    End If
                Next

                ' This is not a 100% but it beats having huge tables.
                If rva0.Count > 0 Then
                    rva0.Sort()
                    Dim last As Long = rva0(0)
                    For Each rv As Long In rva0
                        If rv - last > &H1000 Then
                            Exit For
                        End If
                        rvas.Add(rv)
                        last = rv
                    Next
                End If
            End Using

            If rvas.Count > 0 Then
                '&& (addresses[addresses.Count - 1] - addresses[0]) < 0x1000)
                rvas.Sort()
                IAT_start = rvas(0)
                IAT_end = rvas(rvas.Count - 1)
                Return True
            End If
            Return False
        Catch
            Return False
        End Try
    End Function
    Public Sub FixDump(ByVal dumpFile As String, ByVal bByOrdinal As Boolean)
        If Me.m_ProcessAPIs.Count = 0 Then
            OnLog("Nothing to do...;)")
            Return
        End If

        Dim dmp As PEImage = PEImage.ReadImage(dumpFile, False)
        dmp.Backup()


        Dim size As UInteger = 20
        ' For the empty directory
        For Each th As IatRebuilder.ApiTableThunk In Me.FThunks
            size += 20
            size += CUInt(th.DllName.Length)
            size += 1
            If th.DllName.Length Mod 2 <> 0 Then
                size += 1
            End If

            If Not bByOrdinal Then
                For Each api As IatRebuilder.ApiThunk In th.APIs
                    If api.Export.Name <> "-" Then
                        Dim nameSize As UInteger = CUInt(api.Export.Name.Length)
                        If nameSize Mod 2 <> 0 Then
                            nameSize += 1
                        End If
                        size += 2
                        ' Hint
                        size += nameSize
                        ' Aligned
                        size += 1
                    End If
                Next
            End If
        Next

        Dim alignment As Integer = dmp.PeHeader.OptionalHeader.FileAlignment
        Dim [rem] As Integer = 0
        Dim num As Integer = Math.DivRem(CInt(size), alignment, [rem])
        If [rem] <> 0 Then
            size = CUInt(System.Threading.Interlocked.Increment(num) * alignment)
        End If
        Dim data As Byte() = New Byte(size - 1) {}
        ' Some empty bytes 
        Dim h As SectionHeader = dmp.GetNextSection()
        ' This will align the bytes
        dmp.AppendSection(h, data)
        dmp.PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_IMPORT)).VirtualAddress = dmp.OffsetToRva(h.PointerToRawData)
        dmp.PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_IMPORT)).Size = size

        Dim dllNameFixups As New Queue(Of Long)()
        Using file As New FileStream(dmp.FileName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)
            Using writer As New BinaryWriter(file)
                writer.Seek(h.PointerToRawData, SeekOrigin.Begin)

                ' Write the directory entries
                For Each th As IatRebuilder.ApiTableThunk In Me.FThunks
                    Dim zero As UInteger = 0

                    writer.Write(zero)
                    ' ImportLookupRVA
                    writer.Write(zero)
                    ' TimeDateStamp
                    writer.Write(zero)
                    ' ForwarderChain
                    ' We do not know the location of the Name yet so fix up later
                    dllNameFixups.Enqueue(writer.BaseStream.Position)
                    writer.Write(zero)
                    writer.Write(th.Rva)
                Next
                ' Closing directory
                Dim emptyDir As Byte() = New Byte(19) {}
                Dim padding As Byte = 0
                writer.Write(emptyDir)

                For Each th As IatRebuilder.ApiTableThunk In Me.FThunks
                    ' The name of the DLL will be written here. So now we know what the RVA will be and we can do the fixup
                    FixupName(writer, dllNameFixups.Dequeue(), dmp.OffsetToRva(CUInt(writer.BaseStream.Position)))

                    ' No write the DLL name. (Aligned)
                    writer.Write(th.DllName.ToCharArray())
                    writer.Write(padding)
                    If th.DllName.Length Mod 2 <> 0 Then
                        writer.Write(padding)
                    End If

                    ' now write the function names
                    For Each api As IatRebuilder.ApiThunk In th.APIs
                        Dim ThunkOffset As Integer = CInt(dmp.RvaToOffset(api.Rva))
                        If api.Export.Name <> "-" AndAlso Not bByOrdinal Then
                            ' Only by name
                            Dim thisRva As UInteger = dmp.OffsetToRva(CUInt(writer.BaseStream.Position))
                            If Not m_x64 Then
                                FixupIAT_Thunk(ThunkOffset, thisRva, writer)
                            Else
                                FixupIAT_Thunk(ThunkOffset, CULng(thisRva), writer)
                            End If

                            writer.Write(Convert.ToUInt16(api.Export.Ordinal))
                            ' Hint, check this....
                            writer.Write(api.Export.Name.ToCharArray())
                            writer.Write(padding)
                            If api.Export.Name.Length Mod 2 <> 0 Then
                                writer.Write(padding)
                            End If
                        Else
                            ' Ordinal numbers only
                            If Not m_x64 Then
                                Dim ordinal As UInteger = api.Export.Ordinal
                                ordinal = ordinal Or &H80000000UI
                                FixupIAT_Thunk(ThunkOffset, ordinal, writer)
                            Else
                                Dim ordinal As ULong = api.Export.Ordinal
                                ordinal = ordinal Or &H8000000000000000UL
                                FixupIAT_Thunk(ThunkOffset, ordinal, writer)
                            End If
                        End If
                    Next
                    ' Mark the end with zero's
                    Dim zeroThunkOffset As Integer = CInt(dmp.RvaToOffset(th.APIs(th.APIs.Count - 1).Rva)) + m_IatEntrySize
                    If Not m_x64 Then
                        FixupIAT_Thunk(zeroThunkOffset, UInteger.MinValue, writer)
                    Else
                        FixupIAT_Thunk(zeroThunkOffset, ULong.MinValue, writer)
                    End If
                Next

                writer.Seek(0, SeekOrigin.Begin)
                dmp.SaveToStream(writer)
            End Using
        End Using
        OnLog(String.Format("Dump fixed, added {0} functions in new section. Section size {1}.", m_ProcessAPIs.Count.ToString("X4"), size.ToString("X4")))
    End Sub
    Public Sub BuildThunkTables()
        Dim last As Long = 0
        Dim address As Long = 0
        Dim thunk As New ApiTableThunk()

        ' Build thunks
        Dim list As New List(Of ApiTableThunk)()
        For Each rva As UInteger In m_ProcessAPIs.Keys
            If rva - m_IatEntrySize <> last Then
                thunk = New ApiTableThunk()
                thunk.Rva = rva
                list.Add(thunk)
            End If
            last = rva
            address = m_ProcessAPIs(rva)
            Dim exports As ImageExport() = GetExports(address)
            thunk.APIs.Add(New ApiThunk(Me, thunk, rva, address))
        Next

        ' Validate thunks and apis
        For Each table As ApiTableThunk In list
            table.CalcModule()
        Next
        m_ApiTables = list
    End Sub
#End Region

#Region "Private"
    Protected Overridable Sub OnLog(ByVal message As String)
        RaiseEvent LogMessage(Me, message)
    End Sub

    Private Function GetExports(ByVal address As Long) As ImageExport()
        If m_KnownAddresses.ContainsKey(address) Then
            Return m_KnownAddresses(address).ToArray()
        End If
        Return New ImageExport(-1) {}
    End Function
    Private Sub FixupName(ByVal writer As BinaryWriter, ByVal position As Long, ByVal nameRVA As UInteger)
        ' remember current possition
        Dim currentPos As Long = writer.BaseStream.Position

        ' Go to fixup position and overwrite
        writer.BaseStream.Position = position
        writer.Write(nameRVA)

        'restore current possition
        writer.BaseStream.Position = currentPos
    End Sub
    Private Sub FixupIAT_Thunk(ByVal thunkPosition As Integer, ByVal rvaValue As UInteger, ByVal writer As BinaryWriter)
        ' remember current possition
        Dim currentPos As Long = writer.BaseStream.Position

        ' Go to fixup and overwrite
        writer.Seek(thunkPosition, SeekOrigin.Begin)

        writer.Write(rvaValue)
        ' UInteger
        'restore current possition
        writer.BaseStream.Position = currentPos
    End Sub
    Private Sub FixupIAT_Thunk(ByVal thunkPosition As Integer, ByVal rvaValue As ULong, ByVal writer As BinaryWriter)
        ' remember current possition
        Dim currentPos As Long = writer.BaseStream.Position

        ' Go to fixup and overwrite
        writer.Seek(thunkPosition, SeekOrigin.Begin)

        writer.Write(rvaValue)
        ' QWORD
        'restore current possition
        writer.BaseStream.Position = currentPos
    End Sub
#End Region
#End Region
End Class
