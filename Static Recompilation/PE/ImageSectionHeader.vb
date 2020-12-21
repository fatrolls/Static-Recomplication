
Imports System.Collections
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Text

#Region "Enumerations"

<Flags()> _
Public Enum SectionCharacteristics As UInteger
    Reserved0 = &H0
    Reserved1 = &H1
    Reserved2 = &H2
    Reserved3 = &H4
    IMAGE_SCN_TYPE_NO_PAD = &H8
    Reserved5 = &H10
    IMAGE_SCN_CNT_CODE = &H20
    IMAGE_SCN_CNT_INITIALIZED_DATA = &H40
    IMAGE_SCN_CNT_UNINITIALIZED_DATA = &H80
    IMAGE_SCN_LNK_OTHER = &H100
    IMAGE_SCN_LNK_INFO = &H200
    Reserved6 = &H400
    IMAGE_SCN_LNK_REMOVE = &H800
    IMAGE_SCN_LNK_COMDAT = &H1000
    IMAGE_SCN_GPREL = &H8000
    IMAGE_SCN_MEM_PURGEABLE = &H20000
    IMAGE_SCN_MEM_16BIT = &H20000
    IMAGE_SCN_MEM_LOCKED = &H40000
    IMAGE_SCN_MEM_PRELOAD = &H80000
    IMAGE_SCN_ALIGN_1BYTES = &H100000
    IMAGE_SCN_ALIGN_2BYTES = &H200000
    IMAGE_SCN_ALIGN_4BYTES = &H300000
    IMAGE_SCN_ALIGN_8BYTES = &H400000
    IMAGE_SCN_ALIGN_16BYTES = &H500000
    IMAGE_SCN_ALIGN_32BYTES = &H600000
    IMAGE_SCN_ALIGN_64BYTES = &H700000
    IMAGE_SCN_ALIGN_128BYTES = &H800000
    IMAGE_SCN_ALIGN_256BYTES = &H900000
    IMAGE_SCN_ALIGN_512BYTES = &HA00000
    IMAGE_SCN_ALIGN_1024BYTES = &HB00000
    IMAGE_SCN_ALIGN_2048BYTES = &HC00000
    IMAGE_SCN_ALIGN_4096BYTES = &HD00000
    IMAGE_SCN_ALIGN_8192BYTES = &HE00000
    IMAGE_SCN_LNK_NRELOC_OVFL = &H1000000
    IMAGE_SCN_MEM_DISCARDABLE = &H2000000
    IMAGE_SCN_MEM_NOT_CACHED = &H4000000
    IMAGE_SCN_MEM_NOT_PAGED = &H8000000
    IMAGE_SCN_MEM_SHARED = &H10000000
    IMAGE_SCN_MEM_EXECUTE = &H20000000
    IMAGE_SCN_MEM_READ = &H40000000
    IMAGE_SCN_MEM_WRITE = &H80000000UI
End Enum

#End Region

Public Class SectionCollection
    Inherits CollectionBase
#Region "Constructors"

    Friend Sub New(ByVal sections As SectionHeader())
        MyBase.InnerList.AddRange(sections)
    End Sub

#End Region

#Region "Indexers"

    ' Properties
    Default Public ReadOnly Property Item(ByVal index As Integer) As SectionHeader
        Get
            Return DirectCast(MyBase.InnerList(index), SectionHeader)
        End Get
    End Property

#End Region

#Region "Methods"

    Public Sub CopyTo(ByVal array As SectionHeader(), ByVal index As Integer)
        MyBase.InnerList.CopyTo(array, index)
    End Sub

    Public Sub Remove(ByVal section As SectionHeader)
        MyBase.InnerList.Remove(section)
    End Sub

    Friend Function Add(ByVal section As SectionHeader) As Integer
        Return MyBase.InnerList.Add(section)
    End Function

    Friend Sub Insert(ByVal index As Integer, ByVal section As SectionHeader)
        MyBase.InnerList.Insert(index, section)
    End Sub

#End Region
End Class

Public Class SectionHeader
#Region "Fields"

    Private m_Characteristics As SectionCharacteristics
    ' TODO Change to enum flags field.
    Private m_Image As PEImage
    Private m_Name As String
    Private m_NumberOfLineNumbers As UShort
    Private m_NumberOfRelocations As UShort
    Private m_PointerToLineNumbers As UInteger
    Private m_PointerToRawData As UInteger
    Private m_PointerToRelocations As UInteger
    Private m_SizeOfRawData As UInteger
    Private m_VirtualAddress As UInteger
    Private m_VirtualSize As UInteger

#End Region

#Region "Constructors"

    Friend Sub New(ByVal image As PEImage, ByVal reader As BinaryReader)
        m_Image = image

        'byte[] buffer = new byte[8];
        Dim b As New StringBuilder(8)
        For i As Integer = 0 To 7
            Dim c As Char = reader.ReadChar()
            If AscW(c) <> &H0 Then
                b.Append(c)
            Else
                b.Append(" "c)
            End If
        Next
        m_Name = b.ToString()

        m_VirtualSize = reader.ReadUInt32()
        m_VirtualAddress = reader.ReadUInt32()
        m_SizeOfRawData = reader.ReadUInt32()

        m_PointerToRawData = reader.ReadUInt32()
        m_PointerToRelocations = reader.ReadUInt32()
        m_PointerToLineNumbers = reader.ReadUInt32()

        m_NumberOfRelocations = reader.ReadUInt16()
        m_NumberOfLineNumbers = reader.ReadUInt16()
        m_Characteristics = CType(reader.ReadUInt32(), SectionCharacteristics)
    End Sub

    Friend Sub New()
    End Sub

#End Region

#Region "Properties"
    'TODO: TEMP FUNCTIONS REMOVE THEM IF AlignedSizeOfRawData() and AlignedVirtualSize() can be used below
    Public Function power_of_two(ByVal val As UInteger) As Boolean
        Return val <> 0 AndAlso (val And (val - 1)) = 0
    End Function

    Public Function adjust_FileAlignment(ByVal val As UInteger, ByVal file_alignment As UInteger) As UInteger
        If file_alignment > &H200 Then
            'if it's not a power of two, report it
            If Not power_of_two(file_alignment) Then
                Throw New Exception("Not Power of two file alignment")
            End If
        End If
        If file_alignment < &H200 Then
            Return val
        End If
        Return (val / &H200) * &H200
    End Function

    Public Function adjust_SectionAlignment(ByVal val As UInteger, ByVal section_alignment As UInteger, ByVal file_alignment As UInteger) As UInteger
        If file_alignment < &H200 Then
            If file_alignment <> section_alignment Then
                Throw New Exception(String.Format("If FileAlignment({0}) < 0x200 it should equal SectionAlignment({1})", _
                                                   file_alignment, section_alignment))
            End If
        End If

        If section_alignment < &H1000 Then 'page size
            section_alignment = file_alignment
        End If

        If section_alignment AndAlso (val Mod section_alignment) Then
            Return section_alignment * (val / section_alignment)
        End If
        Return val
    End Function

    <Browsable(False)> _
    Public ReadOnly Property AlignedSizeOfRawData() As UInteger
        Get
            Dim alignment As Integer = m_Image.PeHeader.OptionalHeader.FileAlignment
            Dim [rem] As Integer = 0
            Dim num As Integer = Math.DivRem(CInt(SizeOfRawData), alignment, [rem])
            If [rem] <> 0 Then
                Return CUInt(System.Threading.Interlocked.Increment(num) * alignment)
            End If

            Return SizeOfRawData
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property AlignedVirtualSize() As UInteger
        Get
            Dim alignment As Integer = m_Image.PeHeader.OptionalHeader.SectionAlignment
            Dim [rem] As Integer = 0
            Dim num As Integer = Math.DivRem(CInt(VirtualSize), alignment, [rem])
            If [rem] <> 0 Then
                Return CUInt(System.Threading.Interlocked.Increment(num) * alignment)
            End If

            Return VirtualSize
        End Get
    End Property

    <Category("SectionHeader")> _
    <Description("The flags that describe the characteristics of the section.")> _
    Public Property Characteristics() As SectionCharacteristics
        Get
            Return m_Characteristics
        End Get
        Set(ByVal value As SectionCharacteristics)
            m_Characteristics = value
        End Set
    End Property

    <Category("SectionHeader")> _
    <Description("An 8-byte, null-padded UTF-8 encoded string. If the string is exactly 8 characters long, there is no terminating null. For longer names, this field contains a slash (/) that is followed by an ASCII representation of a decimal number that is an offset into the string table. Executable images do not use a string table and do not support section names longer than 8 characters. Long names in object files are truncated if they are emitted to an executable file.")> _
    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(ByVal value As String)
            If value.Length > 8 Then
                value = value.Substring(0, 8)
            End If
            m_Name = value
        End Set
    End Property

    <Category("SectionHeader")> _
    <Description("The number of line-number entries for the section. This value should be zero for an image because COFF debugging information is deprecated.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property NumberOfLineNumbers() As WORD
        Get
            Return m_NumberOfLineNumbers
        End Get
        Set(ByVal value As WORD)
            m_NumberOfLineNumbers = value
        End Set
    End Property

    <Category("SectionHeader")> _
    <Description("The number of relocation entries for the section. This is set to zero for executable images.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property NumberOfRelocations() As WORD
        Get
            Return m_NumberOfRelocations
        End Get
        Set(ByVal value As WORD)
            m_NumberOfRelocations = value
        End Set
    End Property

    <Category("SectionHeader")> _
    <Description("The file pointer to the beginning of line-number entries for the section. This is set to zero if there are no COFF line numbers. This value should be zero for an image because COFF debugging information is deprecated.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property PointerToLineNumbers() As UInteger
        Get
            Return m_PointerToLineNumbers
        End Get
        Set(ByVal value As UInteger)
            m_PointerToLineNumbers = value
        End Set
    End Property

    <Category("SectionHeader")> _
    <Description("The file pointer to the first page of the section within the COFF file. For executable images, this must be a multiple of FileAlignment from the optional header. For object files, the value should be aligned on a 4 byte boundary for best performance. When a section contains only uninitialized data, this field should be zero.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property PointerToRawData() As UInteger
        Get
            Return m_PointerToRawData
        End Get
        Set(ByVal value As UInteger)
            m_PointerToRawData = value
        End Set
    End Property

    <Category("SectionHeader")> _
    <Description("The file pointer to the beginning of relocation entries for the section. This is set to zero for executable images or if there are no relocations.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property PointerToRelocations() As UInteger
        Get
            Return m_PointerToRelocations
        End Get
        Set(ByVal value As UInteger)
            m_PointerToRelocations = value
        End Set
    End Property

    <Category("SectionHeader")> _
    <Description("The size of the section (for object files) or the size of the initialized data on disk (for image files). For executable images, this must be a multiple of FileAlignment from the optional header. If this is less than VirtualSize, the remainder of the section is zero-filled. Because the SizeOfRawData field is rounded but the VirtualSize field is not, it is possible for SizeOfRawData to be greater than VirtualSize as well. When a section contains only uninitialized data, this field should be zero.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property SizeOfRawData() As UInteger
        Get
            Return m_SizeOfRawData
        End Get
        Set(ByVal value As UInteger)
            m_SizeOfRawData = value
        End Set
    End Property

    <Category("SectionHeader")> _
    <Description("For executable images, the address of the first byte of the section relative to the image base when the section is loaded into memory. For object files, this field is the address of the first byte before relocation is applied; for simplicity, compilers should set this to zero. Otherwise, it is an arbitrary value that is subtracted from offsets during relocation.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property VirtualAddress() As UInteger
        Get
            Return m_VirtualAddress
        End Get
        Set(ByVal value As UInteger)
            m_VirtualAddress = value
        End Set
    End Property

    <Category("SectionHeader")> _
    <Description("The total size of the section when loaded into memory. If this value is greater than SizeOfRawData, the section is zero-padded. This field is valid only for executable images and should be set to zero for object files.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property VirtualSize() As UInteger
        Get
            Return m_VirtualSize
        End Get
        Set(ByVal value As UInteger)
            m_VirtualSize = value
        End Set
    End Property

    Friend Property Image() As PEImage
        Get
            Return m_Image
        End Get
        Set(ByVal value As PEImage)
            m_Image = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Function GetBytes() As Byte()
        Dim bytes As Byte() = Nothing
        Using s As New FileStream(m_Image.FileName, FileMode.Open, FileAccess.Read, FileShare.Read)
            s.Seek(Me.m_PointerToRawData, SeekOrigin.Begin)
            bytes = New Byte(Me.m_SizeOfRawData - 1) {}
            s.Read(bytes, 0, CInt(m_SizeOfRawData))
        End Using
        Return bytes
    End Function

    Public Sub GetBytes(ByVal bytes As Byte())
        Using s As New FileStream(m_Image.FileName, FileMode.Open)
            s.Seek(Me.m_PointerToRawData, SeekOrigin.Begin)
            s.Read(bytes, 0, CInt(m_SizeOfRawData))
        End Using
    End Sub

    Public Sub Save(ByVal writer As BinaryWriter)
        Dim sectionName As String = m_Name
        If sectionName.Length <> 8 Then
            sectionName = sectionName.PadRight(8, " "c).Substring(0, 8)
        End If

        writer.Write(sectionName.ToCharArray())
        writer.Write(m_VirtualSize)
        writer.Write(m_VirtualAddress)
        writer.Write(m_SizeOfRawData)

        writer.Write(m_PointerToRawData)
        writer.Write(m_PointerToRelocations)
        writer.Write(m_PointerToLineNumbers)

        writer.Write(m_NumberOfRelocations)
        writer.Write(m_NumberOfLineNumbers)
        writer.Write(DirectCast(m_Characteristics, UInt32))
    End Sub

#End Region
End Class