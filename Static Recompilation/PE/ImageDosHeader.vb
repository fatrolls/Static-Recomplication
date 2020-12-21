
Imports System.Collections
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Text

''' <summary>
''' Dos header
''' </summary>
Public Class ImageDosHeader
#Region "Fields"

    Public Const IMAGE_DOS_SIGNATURE As UShort = &H5A4D
    ' MZ
    Public Const IMAGE_OS2_SIGNATURE As UShort = &H454E
    ' NE
    Public Const IMAGE_OS2_SIGNATURE_LE As UShort = &H454C
    ' LE
    Public Const IMAGE_VXD_SIGNATURE As UShort = &H454C
    ' LE
    'private int m_AddressOfNewExeHeader;           // File address of new exe header
    Private m_AddressOfNewExeHeader As UInteger
    ' File address of new exe header
    Private m_AddressOfRelocationTable As UShort
    ' File address of relocation table
    Private m_BytesOnLastPage As UShort
    ' Bytes on last page of file
    Private m_Checksum As UShort
    ' Checksum
    Private m_InitialCS As UShort
    ' Initial (relative) CS value
    Private m_InitialIP As UShort
    ' Initial IP value
    Private m_InitialSP As UShort
    ' Initial SP value
    Private m_InitialSS As UShort
    ' Initial (relative) SS value
    ' DOS .EXE header
    Private m_Magic As String
    ' Magic number
    Private m_MaxExtraParagraphs As UShort
    ' Maximum extra paragraphs needed
    Private m_MinExtraParagraphs As UShort
    ' Minimum extra paragraphs needed
    Private m_OemId As UShort
    ' OEM identifier (for e_oeminfo)
    Private m_OemInfo As UShort
    ' OEM information; e_oemid specific
    Private m_OverlayNumber As UShort
    ' Overlay number
    Private m_Pages As UShort
    ' Pages in file
    Private m_ParagraphsInDosHeader As UShort
    ' Size of header in paragraphs
    Private m_Relocations As UShort
    ' Relocations
    Private m_ReserveUIntegers As UShort()
    ' Reserved words
    Private m_ReserveUIntegers2 As UShort()
    ' Reserved words
#End Region

#Region "Constructors"

    Public Sub New(ByVal reader As BinaryReader)
        m_Magic = Encoding.ASCII.GetString(reader.ReadBytes(2))
        m_BytesOnLastPage = reader.ReadUInt16()
        m_Pages = reader.ReadUInt16()
        m_Relocations = reader.ReadUInt16()
        m_ParagraphsInDosHeader = reader.ReadUInt16()
        m_MinExtraParagraphs = reader.ReadUInt16()
        m_MaxExtraParagraphs = reader.ReadUInt16()
        m_InitialSS = reader.ReadUInt16()
        m_InitialSP = reader.ReadUInt16()
        m_Checksum = reader.ReadUInt16()
        m_InitialIP = reader.ReadUInt16()
        m_InitialCS = reader.ReadUInt16()
        m_AddressOfRelocationTable = reader.ReadUInt16()
        m_OverlayNumber = reader.ReadUInt16()
        m_ReserveUIntegers = New UShort(3) {}
        m_ReserveUIntegers(0) = reader.ReadUInt16()
        m_ReserveUIntegers(1) = reader.ReadUInt16()
        m_ReserveUIntegers(2) = reader.ReadUInt16()
        m_ReserveUIntegers(3) = reader.ReadUInt16()

        m_OemId = reader.ReadUInt16()
        m_OemInfo = reader.ReadUInt16()
        m_ReserveUIntegers2 = New UShort(9) {}
        m_ReserveUIntegers2(0) = reader.ReadUInt16()
        m_ReserveUIntegers2(1) = reader.ReadUInt16()
        m_ReserveUIntegers2(2) = reader.ReadUInt16()
        m_ReserveUIntegers2(3) = reader.ReadUInt16()
        m_ReserveUIntegers2(4) = reader.ReadUInt16()
        m_ReserveUIntegers2(5) = reader.ReadUInt16()
        m_ReserveUIntegers2(6) = reader.ReadUInt16()
        m_ReserveUIntegers2(7) = reader.ReadUInt16()
        m_ReserveUIntegers2(8) = reader.ReadUInt16()
        m_ReserveUIntegers2(9) = reader.ReadUInt16()
        'm_AddressOfNewExeHeader = reader.ReadInt32();

        'int sizeOfDosStub = m_AddressOfNewExeHeader - (int)reader.BaseStream.Position;
        'm_DosStub = new byte[sizeOfDosStub];
        'reader.Read(m_DosStub, 0, sizeOfDosStub);
        m_AddressOfNewExeHeader = DWORD.Read(reader)
    End Sub

#End Region

#Region "Properties"

    <Category("DosHeader")> _
    <Description("Start of PE structure.")> _
    <TypeConverter(GetType(DWORDConverter))> _
    Public Property AddressOfNewExeHeader() As UInteger
        Get
            Return m_AddressOfNewExeHeader
        End Get
        Set(ByVal value As UInteger)
            m_AddressOfNewExeHeader = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("Offset of first relocation item. (Usually set to 1Eh for MS, 1Ch for Borland)")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property AddressOfRelocationTable() As WORD
        Get
            Return m_AddressOfRelocationTable
        End Get
        Set(ByVal value As WORD)
            m_AddressOfRelocationTable = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("Image size mod 512 bytes on last page.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property BytesOnLastPage() As WORD
        Get
            Return m_BytesOnLastPage
        End Get
        Set(ByVal value As WORD)
            m_BytesOnLastPage = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("File checksum. One's complement of the sum of all words in .exe file. ([12h] is assumed 00h when reading file for calculation). If [12h] is zero, no checksum is used.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property Checksum() As WORD
        Get
            Return m_Checksum
        End Get
        Set(ByVal value As WORD)
            m_Checksum = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("CS offset in load module. CS:IP = [16h]:[14h].")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property InitialCS() As WORD
        Get
            Return m_InitialCS
        End Get
        Set(ByVal value As WORD)
            m_InitialCS = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("Initial value of IP.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property InitialIP() As WORD
        Get
            Return m_InitialIP
        End Get
        Set(ByVal value As WORD)
            m_InitialIP = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("Initial value of sp. SS:SP = [OEh]:[10h]")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property InitialSP() As WORD
        Get
            Return m_InitialSP
        End Get
        Set(ByVal value As WORD)
            m_InitialSP = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("Stack segment offset in load module.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property InitialSS() As WORD
        Get
            Return m_InitialSS
        End Get
        Set(ByVal value As WORD)
            m_InitialSS = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("ID signature ('MZ' or 'ZM')")> _
    Public Property Magic() As String
        Get
            Return m_Magic
        End Get
        Set(ByVal value As String)
            m_Magic = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("Maximum required memory. Usually set to 0FFFFh.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property MaxExtraParagraphs() As WORD
        Get
            Return m_MaxExtraParagraphs
        End Get
        Set(ByVal value As WORD)
            m_MaxExtraParagraphs = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("Minimum required memory. DOS (should) use this to find out how much memory is required for the program. Size of EXE + [0Ah] = minimum amount.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property MinExtraParagraphs() As WORD
        Get
            Return m_MinExtraParagraphs
        End Get
        Set(ByVal value As WORD)
            m_MinExtraParagraphs = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property OemId() As WORD
        Get
            Return m_OemId
        End Get
        Set(ByVal value As WORD)
            m_OemId = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property OemInfo() As WORD
        Get
            Return m_OemInfo
        End Get
        Set(ByVal value As WORD)
            m_OemInfo = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("Overlay number. (most of the time is zero)")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property OverlayNumber() As WORD
        Get
            Return m_OverlayNumber
        End Get
        Set(ByVal value As WORD)
            m_OverlayNumber = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("Number of 512-byte pages in image. Size of total file = [04h] * 512 + [02h]")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property Pages() As WORD
        Get
            Return m_Pages
        End Get
        Set(ByVal value As WORD)
            m_Pages = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("Size of header, in paragraphs. Usually set to 20h for 512 bytes.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property ParagraphsInDosHeader() As WORD
        Get
            Return m_ParagraphsInDosHeader
        End Get
        Set(ByVal value As WORD)
            m_ParagraphsInDosHeader = value
        End Set
    End Property

    <Category("DosHeader")> _
    <Description("Count of relocation table entries.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property Relocations() As WORD
        Get
            Return m_Relocations
        End Get
        Set(ByVal value As WORD)
            m_Relocations = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Sub Save(ByVal writer As BinaryWriter)
        writer.Write(m_Magic.ToCharArray())
        writer.Write(m_BytesOnLastPage)
        writer.Write(m_Pages)
        writer.Write(m_Relocations)
        writer.Write(m_ParagraphsInDosHeader)
        writer.Write(m_MinExtraParagraphs)
        writer.Write(m_MaxExtraParagraphs)
        writer.Write(m_InitialSS)
        writer.Write(m_InitialSP)
        writer.Write(m_Checksum)
        writer.Write(m_InitialIP)
        writer.Write(m_InitialCS)
        writer.Write(m_AddressOfRelocationTable)
        writer.Write(m_OverlayNumber)
        writer.Write(m_ReserveUIntegers(0))
        writer.Write(m_ReserveUIntegers(1))
        writer.Write(m_ReserveUIntegers(2))
        writer.Write(m_ReserveUIntegers(3))
        writer.Write(m_OemId)
        writer.Write(m_OemInfo)
        writer.Write(m_ReserveUIntegers2(0))
        writer.Write(m_ReserveUIntegers2(1))
        writer.Write(m_ReserveUIntegers2(2))
        writer.Write(m_ReserveUIntegers2(3))
        writer.Write(m_ReserveUIntegers2(4))
        writer.Write(m_ReserveUIntegers2(5))
        writer.Write(m_ReserveUIntegers2(6))
        writer.Write(m_ReserveUIntegers2(7))
        writer.Write(m_ReserveUIntegers2(8))
        writer.Write(m_ReserveUIntegers2(9))
        writer.Write(m_AddressOfNewExeHeader)
        'writer.Write(m_DosStub);
    End Sub

#End Region

#Region "Other"

    'private byte[] m_DosStub;

#End Region
End Class
