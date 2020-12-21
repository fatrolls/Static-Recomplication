
Imports System.Collections
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Text

#Region "Enumerations"

<Flags()> _
Public Enum Characteristics As UShort
    IMAGE_FILE_RELOCS_STRIPPED = &H1
    IMAGE_FILE_EXECUTABLE_IMAGE = &H2
    IMAGE_FILE_LINE_NUMS_STRIPPED = &H4
    IMAGE_FILE_LOCAL_SYMS_STRIPPED = &H8
    IMAGE_FILE_AGGRESSIVE_WS_TRIM = &H10
    IMAGE_FILE_LARGE_ADDRESS_AWARE = &H20
    RESERVED = &H40
    IMAGE_FILE_BYTES_REVERSED_LO = &H80
    IMAGE_FILE_32BIT_MACHINE = &H100
    IMAGE_FILE_DEBUG_STRIPPED = &H200
    IMAGE_FILE_REMOVABLE_RUN_FROM_SWAP = &H400
    IMAGE_FILE_NET_RUN_FROM_SWAP = &H800
    IMAGE_FILE_SYSTEM = &H1000
    IMAGE_FILE_DLL = &H2000
    IMAGE_FILE_UP_SYSTEM_ONLY = &H4000
    IMAGE_FILE_BYTES_REVERSED_HI = &H8000
End Enum

Public Enum MachineType As UShort
    IMAGE_FILE_MACHINE_UNKNOWN = &H0
    IMAGE_FILE_MACHINE_AM33 = &H1D3
    IMAGE_FILE_MACHINE_AMD64 = &H8664
    IMAGE_FILE_MACHINE_ARM = &H1C0
    IMAGE_FILE_MACHINE_EBC = &HEBC
    IMAGE_FILE_MACHINE_I386 = &H14C
    IMAGE_FILE_MACHINE_IA64 = &H200
    IMAGE_FILE_MACHINE_M32R = &H9041
    IMAGE_FILE_MACHINE_MIPS16 = &H266
    IMAGE_FILE_MACHINE_MIPSFPU = &H366
    IMAGE_FILE_MACHINE_MIPSFPU16 = &H466
    IMAGE_FILE_MACHINE_POWERPC = &H1F0
    IMAGE_FILE_MACHINE_POWERPCFP = &H1F1
    IMAGE_FILE_MACHINE_R4000 = &H166
    IMAGE_FILE_MACHINE_SH3 = &H1A2
    IMAGE_FILE_MACHINE_SH3DSP = &H1A3
    IMAGE_FILE_MACHINE_SH4 = &H1A6
    IMAGE_FILE_MACHINE_SH5 = &H1A8
    IMAGE_FILE_MACHINE_THUMB = &H1C2
    IMAGE_FILE_MACHINE_WCEMIPSV2 = &H169
End Enum

#End Region

Public Class ImageFileHeader
#Region "Fields"

    Private m_Characteristics As Characteristics
    Private m_Machine As MachineType
    Private m_NumberOfSections As UShort
    Private m_NumberOfSymbols As UInteger
    Private m_PointerToSymbolStore As UInteger
    Private m_SizeOfOptionalHeader As UShort
    Private m_TimeDateStamp As UInteger

#End Region

#Region "Constructors"

    Public Sub New(ByVal reader As BinaryReader)
        m_Machine = CType(reader.ReadUInt16(), MachineType)
        m_NumberOfSections = reader.ReadUInt16()
        m_TimeDateStamp = reader.ReadUInt32()
        m_PointerToSymbolStore = reader.ReadUInt32()
        m_NumberOfSymbols = reader.ReadUInt32()
        m_SizeOfOptionalHeader = reader.ReadUInt16()
        m_Characteristics = CType(reader.ReadUInt16(), Characteristics)
    End Sub

#End Region

#Region "Properties"

    <Category("FileHeader")> _
    <Description("The flags that indicate the attributes of the file.")> _
    Public Property Characteristics() As Characteristics
        Get
            Return m_Characteristics
        End Get
        Set(ByVal value As Characteristics)
            m_Characteristics = value
        End Set
    End Property

    <Category("FileHeader")> _
    <Description("The number that identifies the type of target machine. ")> _
    Public Property Machine() As MachineType
        Get
            Return m_Machine
        End Get
        Set(ByVal value As MachineType)
            m_Machine = value
        End Set
    End Property

    <Category("FileHeader")> _
    <Description("The number of sections. This indicates the size of the section table, which immediately follows the headers.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property NumberOfSections() As WORD
        Get
            Return m_NumberOfSections
        End Get
        Set(ByVal value As WORD)
            m_NumberOfSections = value
        End Set
    End Property

    <Category("FileHeader")> _
    <Description("The number of entries in the symbol table. This data can be used to locate the string table, which immediately follows the symbol table. This value should be zero for an image because COFF debugging information is deprecated.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property NumberOfSymbols() As UInteger
        Get
            Return m_NumberOfSymbols
        End Get
        Set(ByVal value As UInteger)
            m_NumberOfSymbols = value
        End Set
    End Property

    <Category("FileHeader")> _
    <Description("The file offset of the COFF symbol table, or zero if no COFF symbol table is present. This value should be zero for an image because COFF debugging information is deprecated.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property PointerToSymbolStore() As UInteger
        Get
            Return m_PointerToSymbolStore
        End Get
        Set(ByVal value As UInteger)
            m_PointerToSymbolStore = value
        End Set
    End Property

    <Category("FileHeader")> _
    <Description("The size of the optional header, which is required for executable files but not for object files. This value should be zero for an object file.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property SizeOfOptionalHeader() As WORD
        Get
            Return m_SizeOfOptionalHeader
        End Get
        Set(ByVal value As WORD)
            m_SizeOfOptionalHeader = value
        End Set
    End Property

    <Category("FileHeader")> _
    <Description("The low 32 bits of the number of seconds since 00:00 January 1, 1970 (a C run-time time_t value), that indicates when the file was created.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property TimeDateStamp() As UInteger
        Get
            Return m_TimeDateStamp
        End Get
        Set(ByVal value As UInteger)
            m_TimeDateStamp = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Sub Save(ByVal writer As BinaryWriter)
        writer.Write(DirectCast(m_Machine, UInt16))
        writer.Write(m_NumberOfSections)
        writer.Write(m_TimeDateStamp)
        writer.Write(m_PointerToSymbolStore)
        writer.Write(m_NumberOfSymbols)
        writer.Write(m_SizeOfOptionalHeader)
        writer.Write(DirectCast(m_Characteristics, UInt16))
    End Sub

#End Region
End Class
