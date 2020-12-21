
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.Diagnostics
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

Public Class PEImage
#Region "Fields"

    Private Shared m_Cache As New Dictionary(Of String, PEImage)()

    Private m_BaseRelocationBlocks As BaseRelocationBlockCollection
    Private m_Data As Byte()
    Private m_DosHeader As ImageDosHeader
    Private m_ExportDirectory As ImageExportDirectoryEntry
    Private m_FileName As String
    Private m_ImportDirectoryTable As List(Of ImageImportDirectoryEntry)

    'private byte[] m_PeHeaderThunk;
    Private m_NumberOfAvailableSections As Integer = 0
    Private m_PeHeader As ImageNtHeaders32
    Private m_ResourceDirectory As ResourceDirectoryTable
    Private m_Sections As SectionCollection

#End Region

#Region "Constructors"

    Friend Sub New(ByVal fileName As String)
        m_FileName = fileName
        If File.Exists(fileName) Then
            Me.Refresh()
        Else
            Throw New FileNotFoundException(String.Format("PE File '{0}' not found.", fileName))
        End If
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property DosHeader() As ImageDosHeader
        Get
            Return m_DosHeader
        End Get
    End Property

    Public ReadOnly Property ExportDirectory() As ImageExportDirectoryEntry
        Get
            If m_ExportDirectory Is Nothing Then
                Me.ReadExports()
            End If
            Return m_ExportDirectory
        End Get
    End Property

    Public ReadOnly Property FileName() As String
        Get
            Return m_FileName
        End Get
    End Property

    'public ImportDirectoriesCollection ImportDirectoryTable
    Public ReadOnly Property ImportDirectoryTable() As List(Of ImageImportDirectoryEntry)
        Get
            If m_ImportDirectoryTable Is Nothing Then
                If Not Me.ReadImports() Then
                    'm_ImportDirectoryTable = new ImportDirectoriesCollection(new ImageImportDirectoryEntry[0]);
                    m_ImportDirectoryTable = New List(Of ImageImportDirectoryEntry)()
                End If
            End If
            Return m_ImportDirectoryTable
        End Get
    End Property

    Public ReadOnly Property PeHeader() As ImageNtHeaders32
        Get
            Return m_PeHeader
        End Get
    End Property

    Public ReadOnly Property RelocationBlocks() As BaseRelocationBlockCollection
        Get
            If m_BaseRelocationBlocks Is Nothing Then
                ReadRelocations()
            End If
            Return m_BaseRelocationBlocks
        End Get
    End Property

    Public ReadOnly Property ResourceDirectory() As ResourceDirectoryTable
        Get
            If m_ResourceDirectory Is Nothing Then
                ReadResources()
            End If
            Return m_ResourceDirectory
        End Get
    End Property

    Public ReadOnly Property Sections() As SectionCollection
        Get
            Return m_Sections
        End Get
    End Property

#End Region

#Region "Methods"

    Public Shared Function ReadImage(ByVal fileName As String, ByVal fromCache As Boolean) As PEImage
        If fromCache Then
            Return ReadImage(fileName)
        End If

        Dim image As New PEImage(fileName)
        If Not m_Cache.ContainsKey(fileName) Then
            m_Cache.Add(fileName, image)
        Else
            m_Cache(fileName) = image
        End If
        Return image
    End Function

    Public Shared Function ReadImage(ByVal fileName As String) As PEImage
        If m_Cache.ContainsKey(fileName) Then
            Return m_Cache(fileName)
        Else
            Dim image As New PEImage(fileName)
            m_Cache.Add(fileName, image)
            Return image
        End If
    End Function

    Public Sub AppendSection(ByVal header As SectionHeader)
        If header.Name.Length > 8 Then
            Throw New ArgumentException("SectionName can not be greater than 8", "Name")
        End If

        If header.VirtualAddress = 0 Then
            Throw New ArgumentException("Invalid virtual address", "VirtualAddress")
        End If

        Me.Sections.Add(header)
        ValidatePE()
    End Sub

    Public Sub AppendSection(ByVal header As SectionHeader, ByVal data As Byte())
        Using mem As New MemoryStream(data)
            AppendSection(header, mem)
        End Using
    End Sub

    Public Sub AppendSection(ByVal header As SectionHeader, ByVal stream As MemoryStream)
        Dim alignment As Integer = Me.PeHeader.OptionalHeader.FileAlignment
        Dim Size As UInteger = CUInt(stream.Length)
        Dim [rem] As Integer = 0
        Dim num As Integer = Math.DivRem(CInt(Size), alignment, [rem])
        If [rem] <> 0 Then
            stream.SetLength(System.Threading.Interlocked.Increment(num) * alignment)
        End If
        header.SizeOfRawData = CUInt(stream.Length)
        header.VirtualSize = header.SizeOfRawData
        header.VirtualSize = header.AlignedVirtualSize
        AppendSection(header)

        Using f As New FileStream(m_FileName, FileMode.Append, FileAccess.Write, FileShare.Read)
            f.Write(stream.ToArray(), 0, CInt(stream.Length))
        End Using
    End Sub

    ''' <summary>
    ''' Creates a backup of the PE file
    ''' </summary>
    Public Sub Backup()
        Dim index As Integer = 0
        While True
            Dim backup__1 As String = Path.ChangeExtension(m_FileName, String.Format("${0}", index.ToString().PadLeft(2, "0"c)))
            If Not File.Exists(backup__1) Then
                File.Copy(m_FileName, backup__1)
                Exit While
            End If
            index += 1
        End While
    End Sub

    ''' <summary>
    ''' Call this member to retrieve an initialised new sectionheader for the PE-file
    ''' </summary>
    ''' <returns></returns>
    Public Function GetNextSection() As SectionHeader
        'if (m_NumberOfAvailableSections == 0)
        '    throw new InvalidDataException("No more room to append a new section");

        Dim sec As New SectionHeader()
        sec.Image = Me
        ' Some defaults
        Dim prev As SectionHeader = m_Sections(m_Sections.Count - 1)
        sec.PointerToRawData = prev.PointerToRawData + prev.SizeOfRawData
        sec.VirtualAddress = prev.VirtualAddress + prev.AlignedVirtualSize
        sec.VirtualSize = Me.PeHeader.OptionalHeader.SectionAlignment
        sec.Characteristics = SectionCharacteristics.IMAGE_SCN_MEM_EXECUTE Or SectionCharacteristics.IMAGE_SCN_MEM_READ Or SectionCharacteristics.IMAGE_SCN_MEM_WRITE
        sec.Name = "FATROLLS"

        Return sec
    End Function

    Public Function GetSectionFromRva(ByVal rva As UInteger) As SectionHeader
        For i As Integer = 0 To Sections.Count - 1
            If rva >= Sections(i).VirtualAddress AndAlso rva < (Sections(i).VirtualAddress + Sections(i).VirtualSize) Then
                Return Sections(i)
            End If
        Next
        Throw New ApplicationException("unknown RVA! " + rva.ToString("X8"))
    End Function

    Public Function HasDirectoryEntry(ByVal type As IMAGE_DIRECTORY_ENTRY_TYPES) As Boolean
        Return (PeHeader.OptionalHeader.DataDirectories(CInt(type)).VirtualAddress <> 0)
    End Function

    Public Function OffsetToRva(ByVal offset As UInteger) As UInteger
        If offset < Sections(0).PointerToRawData Then
            ' In PE header
            Return offset
        End If

        For i As Integer = 0 To Sections.Count - 1
            If offset >= Sections(i).PointerToRawData AndAlso offset < (Sections(i).PointerToRawData + Sections(i).SizeOfRawData) Then
                Return offset - Sections(i).PointerToRawData + Sections(i).VirtualAddress
            End If
        Next
        Throw New ApplicationException("unknown OffSet! " + offset.ToString("X8"))
    End Function

    Public Function Refresh() As Boolean
        m_Data = File.ReadAllBytes(m_FileName)
        Using memStream As New MemoryStream(m_Data)
            Using reader As New BinaryReader(New FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                reader.BaseStream.Seek(0L, SeekOrigin.Begin)
                m_DosHeader = New ImageDosHeader(reader)

                reader.BaseStream.Position = DosHeader.AddressOfNewExeHeader
                ' Position ready to read NtHeaders
                m_PeHeader = New ImageNtHeaders32(Me, reader)

                ' Read sections
                Dim sections__1 As SectionHeader() = New SectionHeader(PeHeader.FileHeader.NumberOfSections.Value - 1) {}
                For i As Integer = 0 To PeHeader.FileHeader.NumberOfSections.Value - 1
                    sections__1(i) = New SectionHeader(Me, reader)
                Next
                m_Sections = New SectionCollection(sections__1)

                Dim sectionSize As Integer = 40
                Dim sec As Byte() = New Byte(sectionSize - 1) {}
                While reader.BaseStream.Position < Sections(0).PointerToRawData
                    reader.Read(sec, 0, sec.Length)
                    Dim empty As Boolean = True
                    For i As Integer = 0 To sec.Length - 1
                        If sec(i) <> 0 Then
                            empty = False
                            Exit For
                        End If
                    Next
                    If empty AndAlso reader.BaseStream.Position < Sections(0).PointerToRawData Then
                        m_NumberOfAvailableSections += 1
                    Else
                        Exit While
                    End If
                End While

                ' Read remainder of the PeHeader
                'int remainingPeThunk = m_Sections[0].PointerToRawData - (int)reader.BaseStream.Position;
                'm_PeHeaderThunk = new byte[remainingPeThunk];
                'reader.Read(m_PeHeaderThunk, 0, remainingPeThunk);

                If m_ImportDirectoryTable IsNot Nothing Then
                    m_ImportDirectoryTable.Clear()
                    m_ImportDirectoryTable = Nothing
                End If
                If m_ExportDirectory IsNot Nothing Then
                    m_ExportDirectory.Exports.Clear()
                    m_ExportDirectory = Nothing
                End If
                If m_BaseRelocationBlocks IsNot Nothing Then
                    m_BaseRelocationBlocks.Clear()
                    m_BaseRelocationBlocks = Nothing
                    'ReadImports(reader);
                    'ReadExports(reader);
                    'ReadRelocations(reader);
                End If
            End Using
        End Using
        Return True
    End Function

    Public Function RvaToOffset(ByVal rva As UInteger) As UInteger
        If rva < Sections(0).VirtualAddress Then
            ' In PE header
            Return rva
        End If

        For i As Integer = 0 To Sections.Count - 1
            If rva >= Sections(i).VirtualAddress AndAlso rva < (Sections(i).VirtualAddress + Sections(i).VirtualSize) Then
                Return rva - Sections(i).VirtualAddress + Sections(i).PointerToRawData
            End If
        Next
        Throw New ApplicationException("unknown RVA! " + rva.ToString("X8"))
    End Function

    Public Sub SaveAsFile(ByVal file As String)
        Using outFile As New FileStream(file, FileMode.OpenOrCreate)
            Dim writer As New BinaryWriter(outFile)
            SaveToStream(writer)
            writer.Close()
        End Using
    End Sub

    Public Sub SaveToStream(ByVal str As Stream)
        Using writer As New BinaryWriter(str)
            SaveToStream(writer)
        End Using
    End Sub

    Public Function GetDataAtOffset(ByVal offset_start As UInteger, ByVal offset_end As UInteger) As Byte()
        Dim size As UInteger = offset_end - offset_start
        If size < 0 Then Return Nothing
        Dim output() As Byte
        ReDim output(size)
        Buffer.BlockCopy(m_Data, offset_start, output, 0, size)
        Return output
    End Function

    Public Sub SaveToStream(ByVal writer As BinaryWriter)
        m_DosHeader.Save(writer)
        writer.BaseStream.Position = DosHeader.AddressOfNewExeHeader
        ' Position ready to write NtHeaders
        m_PeHeader.Save(writer)
        For Each section As SectionHeader In m_Sections
            section.Save(writer)
        Next
    End Sub

    Public Sub ValidatePE()
        Me.PeHeader.FileHeader.NumberOfSections = CUShort(Me.Sections.Count)
        'uint sizeOfCode = 0;
        'uint sizeOfInitData = 0;
        'uint sizeOfUnInitData = 0;
        Dim sizeOfImage As UInteger = m_Sections(0).VirtualAddress
        Dim sizeOfHeaders As UInteger = 0
        For Each h As SectionHeader In Me.Sections
            h.VirtualSize = h.AlignedVirtualSize
            h.SizeOfRawData = h.AlignedSizeOfRawData

            If sizeOfHeaders = 0 AndAlso h.PointerToRawData <> 0 Then
                sizeOfHeaders = h.PointerToRawData
            End If

            'if ((h.Characteristics & SectionCharacteristics.IMAGE_SCN_CNT_CODE) == SectionCharacteristics.IMAGE_SCN_CNT_CODE)
            '    sizeOfCode += h.SizeOfRawData;

            'if ((h.Characteristics & SectionCharacteristics.IMAGE_SCN_CNT_INITIALIZED_DATA) == SectionCharacteristics.IMAGE_SCN_CNT_INITIALIZED_DATA)
            '    sizeOfInitData += h.SizeOfRawData;

            'if ((h.Characteristics & SectionCharacteristics.IMAGE_SCN_CNT_UNINITIALIZED_DATA) == SectionCharacteristics.IMAGE_SCN_CNT_UNINITIALIZED_DATA)
            '    sizeOfUnInitData += h.SizeOfRawData;

            sizeOfImage += h.AlignedVirtualSize
        Next

        'this.PeHeader.OptionalHeader.SizeOfCode = sizeOfCode;
        'this.PeHeader.OptionalHeader.SizeOfInitializedData = sizeOfInitData;
        'this.PeHeader.OptionalHeader.SizeOfUninitializedData = sizeOfUnInitData;
        'this.PeHeader.OptionalHeader.NumberOfRvaAndSizes = 16;
        Me.PeHeader.OptionalHeader.SizeOfHeaders = sizeOfHeaders
        Me.PeHeader.OptionalHeader.SizeOfImage = sizeOfImage
        ' BaseOfCode + BaseOfData
    End Sub

    Friend Function RvaIsValid(ByVal rva As UInteger) As Boolean
        If rva < Sections(0).VirtualAddress Then
            ' In PE header
            Return True
        End If

        If rva < (Sections(Sections.Count - 1).VirtualAddress + Sections(Sections.Count - 1).VirtualSize) Then
            Return True
        End If

        Return False
    End Function

    Private Function ReadExports() As Boolean
        Dim bRead As Boolean = False
        If PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_EXPORT)).VirtualAddress = 0 Then
            Return False
        End If

        Using memStream As New MemoryStream(m_Data)
            Using reader As New BinaryReader(memStream)
                bRead = ReadExports(reader)
            End Using
        End Using
        Return bRead
    End Function

    Private Function ReadExports(ByVal reader As BinaryReader) As Boolean
        If PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_EXPORT)).VirtualAddress = 0 Then
            Return False
        End If

        Try
            reader.BaseStream.Seek(RvaToOffset(PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_EXPORT)).VirtualAddress), SeekOrigin.Begin)
            m_ExportDirectory = New ImageExportDirectoryEntry(Me, reader)
        Catch err As Exception
            ' Log
            Trace.WriteLine(err)
            Return False
        End Try
        Return True
    End Function

    Private Function ReadImports() As Boolean
        Dim bRead As Boolean = False
        If PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_IMPORT)).VirtualAddress = 0 Then
            Return False
        End If

        Using memStream As New MemoryStream(m_Data)
            Using reader As New BinaryReader(memStream)
                bRead = ReadImports(reader)
            End Using
        End Using
        Return bRead
    End Function

    Private Function ReadImports(ByVal reader As BinaryReader) As Boolean
        Try
            If PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_IMPORT)).VirtualAddress = 0 Then
                Return False
            End If

            m_ImportDirectoryTable = New List(Of ImageImportDirectoryEntry)()
            reader.BaseStream.Seek(RvaToOffset(PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_IMPORT)).VirtualAddress), SeekOrigin.Begin)

            While True
                Dim import As New ImageImportDirectoryEntry(reader)
                If Not import.IsNull Then
                    m_ImportDirectoryTable.Add(import)
                Else
                    Exit While
                End If
            End While
            For Each entry As ImageImportDirectoryEntry In m_ImportDirectoryTable
                entry.ReadFunctions(Me, reader)
            Next
            Return True
        Catch err As Exception
            ' Log error
            Trace.WriteLine(err)
            Return False
        End Try
    End Function

    Private Function ReadRelocations() As Boolean
        Dim dataDirectory As OptionalHeaderDataDirectory = PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_BASERELOC))
        If dataDirectory.VirtualAddress = 0 Then
            Return False
        End If

        Dim bRead As Boolean = False
        Using memStream As New MemoryStream(m_Data)
            ' Hmm.
            Using reader As New BinaryReader(memStream)
                bRead = ReadRelocations(reader)
            End Using
        End Using
        Return bRead
    End Function

    Private Function ReadRelocations(ByVal reader As BinaryReader) As Boolean
        Dim dataDirectory As OptionalHeaderDataDirectory = PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_BASERELOC))
        If dataDirectory.VirtualAddress = 0 Then
            Return False
        End If

        Try
            m_BaseRelocationBlocks = New BaseRelocationBlockCollection()
            reader.BaseStream.Seek(RvaToOffset(dataDirectory.VirtualAddress), SeekOrigin.Begin)
            Dim bytesRead As UInteger = 0
            While bytesRead < dataDirectory.Size
                Dim block As New BaseRelocationBlock(reader)
                m_BaseRelocationBlocks.Add(block)
                bytesRead += block.BlockSize
            End While
        Catch err As Exception
            ' Log Error
            Trace.WriteLine(err)
            Return False
        End Try
        Return True
    End Function

    Private Function ReadResources() As Boolean
        Dim dataDirectory As OptionalHeaderDataDirectory = PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_RESOURCE))
        If dataDirectory.VirtualAddress = 0 Then
            Return False
        End If

        Dim bRead As Boolean = False
        Using memStream As New MemoryStream(m_Data)
            Using reader As New BinaryReader(memStream)
                bRead = ReadResources(reader)
            End Using
        End Using
        Return bRead
    End Function

    Private Function ReadResources(ByVal reader As BinaryReader) As Boolean
        Dim dataDirectory As OptionalHeaderDataDirectory = PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_RESOURCE))
        If dataDirectory.VirtualAddress = 0 Then
            Return False
        End If

        Try
            reader.BaseStream.Seek(RvaToOffset(dataDirectory.VirtualAddress), SeekOrigin.Begin)
            m_ResourceDirectory = New ResourceDirectoryTable(Me, reader)
        Catch err As Exception
            ' Log Error
            Trace.WriteLine(err)
            Return False
        End Try
        Return True
    End Function

#End Region
End Class