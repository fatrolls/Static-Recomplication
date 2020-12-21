
Imports System.Collections
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Text

#Region "Enumerations"

<Flags()> _
Public Enum DllCharacteristics As UShort
    Reserved0 = &H1
    Reserved1 = &H2
    Reserved2 = &H4
    Reserved3 = &H8
    IMAGE_DLL_CHARACTERISTICS_DYNAMIC_BASE = &H40
    IMAGE_DLL_CHARACTERISTICS_FORCE_INTEGRITY = &H80
    IMAGE_DLL_CHARACTERISTICS_NX_COMPAT = &H100
    IMAGE_DLLCHARACTERISTICS_NO_ISOLATION = &H200
    IMAGE_DLLCHARACTERISTICS_NO_SEH = &H400
    IMAGE_DLLCHARACTERISTICS_NO_BIND = &H800
    Reserved4 = &H1000
    IMAGE_DLLCHARACTERISTICS_WDM_DRIVER = &H2000
    IMAGE_DLLCHARACTERISTICS_TERMINAL_SERVER_AWARE = &H8000
End Enum

Public Enum IMAGE_DIRECTORY_ENTRY_TYPES As Integer
    IMAGE_DIRECTORY_ENTRY_EXPORT = 0
    IMAGE_DIRECTORY_ENTRY_IMPORT = 1
    IMAGE_DIRECTORY_ENTRY_RESOURCE = 2
    IMAGE_DIRECTORY_ENTRY_EXCEPTION = 3
    IMAGE_DIRECTORY_ENTRY_SECURITY = 4
    IMAGE_DIRECTORY_ENTRY_BASERELOC = 5
    IMAGE_DIRECTORY_ENTRY_DEBUG = 6
    IMAGE_DIRECTORY_ENTRY_ARCHITECTURE = 7
    IMAGE_DIRECTORY_ENTRY_GLOBALPTR = 8
    IMAGE_DIRECTORY_ENTRY_TLS = 9
    IMAGE_DIRECTORY_ENTRY_LOAD_CONFIG = 10
    IMAGE_DIRECTORY_ENTRY_BOUND_IMPORT = 11
    IMAGE_DIRECTORY_ENTRY_IAT = 12
    IMAGE_DIRECTORY_ENTRY_DELAY_IMPORT = 13
    IMAGE_DIRECTORY_ENTRY_COM_DESCRIPTOR = 14
    Reserved = 15
End Enum

Public Enum WindowsSubSystem As UShort
    IMAGE_SUBSYSTEM_UNKNOWN = 0
    IMAGE_SUBSYSTEM_NATIVE = 1
    IMAGE_SUBSYSTEM_WINDOWS_GUI = 2
    IMAGE_SUBSYSTEM_WINDOWS_CUI = 3
    IMAGE_SUBSYSTEM_POSIX_CUI = 7
    IMAGE_SUBSYSTEM_WINDOWS_CE_GUI = 9
    IMAGE_SUBSYSTEM_EFI_APPLICATION = 10
    IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER = 11
    IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER = 12
    IMAGE_SUBSYSTEM_EFI_ROM = 13
    IMAGE_SUBSYSTEM_XBOX = 14
End Enum

#End Region

Public Class ImageOptionalHeader32
#Region "Fields"

    Protected m_AddressOfEntryPoint As UInteger
    Protected m_BaseOfCode As UInteger
    Protected m_BaseOfData As UInteger
    Protected m_CheckSum As UInteger
    Protected m_DataDirectories As OptionalHeaderDataDirectory()
    Protected m_DllCharacteristics As DllCharacteristics
    Protected m_FileAlignment As UInteger
    Protected m_Image As PEImage
    Protected m_ImageBase As UInteger
    Protected m_LoaderFlags As UInteger
    Protected m_Magic As MagicType
    Protected m_MajorImageVersion As UShort
    Protected m_MajorLinkerVersion As Byte
    Protected m_MajorOperatingSystemVersion As UShort
    Protected m_MajorSubsystemVersion As UShort
    Protected m_MinorImageVersion As UShort
    Protected m_MinorLinkerVersion As Byte
    Protected m_MinorOperatingSystemVersion As UShort
    Protected m_MinorSubsystemVersion As UShort
    Protected m_NumberOfRvaAndSizes As UInteger
    Protected m_SectionAlignment As UInteger
    Protected m_SizeOfCode As UInteger
    Protected m_SizeOfHeaders As UInteger
    Protected m_SizeOfHeapCommit As UInteger
    Protected m_SizeOfHeapReserve As UInteger
    Protected m_SizeOfImage As UInteger
    Protected m_SizeOfInitializedData As UInteger
    Protected m_SizeOfStackCommit As UInteger
    Protected m_SizeOfStackReserve As UInteger
    Protected m_SizeOfUninitializedData As UInteger
    Protected m_Subsystem As WindowsSubSystem
    Protected m_Win32VersionValue As UInteger

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal image As PEImage, ByVal reader As BinaryReader)
        m_Image = image

        m_MajorLinkerVersion = reader.ReadByte()
        m_MinorLinkerVersion = reader.ReadByte()
        m_SizeOfCode = reader.ReadUInt32()
        m_SizeOfInitializedData = reader.ReadUInt32()
        m_SizeOfUninitializedData = reader.ReadUInt32()
        m_AddressOfEntryPoint = reader.ReadUInt32()
        m_BaseOfCode = reader.ReadUInt32()
        m_BaseOfData = reader.ReadUInt32()
        m_ImageBase = reader.ReadUInt32()
        m_SectionAlignment = reader.ReadUInt32()
        m_FileAlignment = reader.ReadUInt32()
        m_MajorOperatingSystemVersion = reader.ReadUInt16()
        m_MinorOperatingSystemVersion = reader.ReadUInt16()
        m_MajorImageVersion = reader.ReadUInt16()
        m_MinorImageVersion = reader.ReadUInt16()
        m_MajorSubsystemVersion = reader.ReadUInt16()
        m_MinorSubsystemVersion = reader.ReadUInt16()
        m_Win32VersionValue = reader.ReadUInt32()
        m_SizeOfImage = reader.ReadUInt32()
        m_SizeOfHeaders = reader.ReadUInt32()
        m_CheckSum = reader.ReadUInt32()
        m_Subsystem = CType(reader.ReadUInt16(), WindowsSubSystem)
        m_DllCharacteristics = CType(reader.ReadUInt16(), DllCharacteristics)
        m_SizeOfStackReserve = reader.ReadUInt32()
        m_SizeOfStackCommit = reader.ReadUInt32()
        m_SizeOfHeapReserve = reader.ReadUInt32()
        m_SizeOfHeapCommit = reader.ReadUInt32()
        m_LoaderFlags = reader.ReadUInt32()
        m_NumberOfRvaAndSizes = reader.ReadUInt32()
        If m_NumberOfRvaAndSizes > 16 Then
            m_NumberOfRvaAndSizes = 16
        End If
        m_DataDirectories = New OptionalHeaderDataDirectory(m_NumberOfRvaAndSizes - 1) {}
        For i As Integer = 0 To m_NumberOfRvaAndSizes - 1
            m_DataDirectories(i) = New OptionalHeaderDataDirectory(image, reader)
        Next
    End Sub

#End Region

#Region "Properties"

    <Category("Standard Fields")> _
    <Description("The address of the entry point relative to the image base when the executable file is loaded into memory. For program images, this is the starting address. For device drivers, this is the address of the initialization function. An entry point is optional for DLLs. When no entry point is present, this field must be zero.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property AddressOfEntryPoint() As UInteger
        Get
            Return m_AddressOfEntryPoint
        End Get
        Set(ByVal value As UInteger)
            m_AddressOfEntryPoint = value
        End Set
    End Property

    <Category("Standard Fields")> _
    <Description("The address that is relative to the image base of the beginning-of-code section when it is loaded into memory.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property BaseOfCode() As UInteger
        Get
            Return m_BaseOfCode
        End Get
        Set(ByVal value As UInteger)
            m_BaseOfCode = value
        End Set
    End Property

    <Category("Standard Fields")> _
    <Description("The address that is relative to the image base of the beginning-of-data section when it is loaded into memory.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property BaseOfData() As UInteger
        Get
            Return m_BaseOfData
        End Get
        Set(ByVal value As UInteger)
            m_BaseOfData = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The image file checksum. The algorithm for computing the checksum is incorporated into IMAGHELP.DLL. The following are checked for validation at load time: all drivers, any DLL loaded at boot time, and any DLL that is loaded into a critical Windows process.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property CheckSum() As UInteger
        Get
            Return m_CheckSum
        End Get
        Set(ByVal value As UInteger)
            m_CheckSum = value
        End Set
    End Property

    <Browsable(False)> _
    Public Property DataDirectories() As OptionalHeaderDataDirectory()
        Get
            Return m_DataDirectories
        End Get
        Set(ByVal value As OptionalHeaderDataDirectory())
            m_DataDirectories = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("")> _
    Public Property DllCharacteristics() As DllCharacteristics
        Get
            Return m_DllCharacteristics
        End Get
        Set(ByVal value As DllCharacteristics)
            m_DllCharacteristics = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The alignment factor (in bytes) that is used to align the raw data of sections in the image file. The value should be a power of 2 between 512 and 64 K, inclusive. The default is 512. If the SectionAlignment is less than the architecture’s page size, then FileAlignment must match SectionAlignment.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property FileAlignment() As UInteger
        Get
            Return m_FileAlignment
        End Get
        Set(ByVal value As UInteger)
            m_FileAlignment = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The preferred address of the first byte of image when loaded into memory; must be a multiple of 64 K. The default for DLLs is 0x10000000. The default for Windows CE EXEs is 0x00010000. The default for Windows NT, Windows 2000, Windows XP, Windows 95, Windows 98, and Windows Me is 0x00400000.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property ImageBase() As UInteger
        Get
            Return m_ImageBase
        End Get
        Set(ByVal value As UInteger)
            m_ImageBase = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("Reserved, must be zero.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property LoaderFlags() As UInteger
        Get
            Return m_LoaderFlags
        End Get
        Set(ByVal value As UInteger)
            m_LoaderFlags = value
        End Set
    End Property

    <Category("Standard Fields")> _
    <Description("The unsigned integer that identifies the state of the image file. The most common number is 0x10B, which identifies it as a normal executable file. 0x107 identifies it as a ROM image, and 0x20B identifies it as a PE32+ executable.")> _
    Public Property Magic() As MagicType
        Get
            Return m_Magic
        End Get
        Set(ByVal value As MagicType)
            m_Magic = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The major version number of the image.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property MajorImageVersion() As WORD
        Get
            Return m_MajorImageVersion
        End Get
        Set(ByVal value As WORD)
            m_MajorImageVersion = value
        End Set
    End Property

    <Category("Standard Fields")> _
    <Description("The linker major version number.")> _
    <TypeConverter(GetType(BYTEConverter))> _
    Public Property MajorLinkerVersion() As [BYTE]
        Get
            Return m_MajorLinkerVersion
        End Get
        Set(ByVal value As [BYTE])
            m_MajorLinkerVersion = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The major version number of the required operating system.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property MajorOperatingSystemVersion() As WORD
        Get
            Return m_MajorOperatingSystemVersion
        End Get
        Set(ByVal value As WORD)
            m_MajorOperatingSystemVersion = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The major version number of the subsystem.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property MajorSubsystemVersion() As WORD
        Get
            Return m_MajorSubsystemVersion
        End Get
        Set(ByVal value As WORD)
            m_MajorSubsystemVersion = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The minor version number of the image.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property MinorImageVersion() As WORD
        Get
            Return m_MinorImageVersion
        End Get
        Set(ByVal value As WORD)
            m_MinorImageVersion = value
        End Set
    End Property

    <Category("Standard Fields")> _
    <Description("The linker minor version number.")> _
    <TypeConverter(GetType(BYTEConverter))> _
    Public Property MinorLinkerVersion() As [BYTE]
        Get
            Return m_MinorLinkerVersion
        End Get
        Set(ByVal value As [BYTE])
            m_MinorLinkerVersion = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The minor version number of the required operating system.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property MinorOperatingSystemVersion() As WORD
        Get
            Return m_MinorOperatingSystemVersion
        End Get
        Set(ByVal value As WORD)
            m_MinorOperatingSystemVersion = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The minor version number of the subsystem.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property MinorSubsystemVersion() As WORD
        Get
            Return m_MinorSubsystemVersion
        End Get
        Set(ByVal value As WORD)
            m_MinorSubsystemVersion = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The number of data-directory entries in the remainder of the optional header. Each describes a location and size.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property NumberOfRvaAndSizes() As UInteger
        Get
            Return m_NumberOfRvaAndSizes
        End Get
        Set(ByVal value As UInteger)
            m_NumberOfRvaAndSizes = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The alignment (in bytes) of sections when they are loaded into memory. It must be greater than or equal to FileAlignment. The default is the page size for the architecture.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property SectionAlignment() As UInteger
        Get
            Return m_SectionAlignment
        End Get
        Set(ByVal value As UInteger)
            m_SectionAlignment = value
        End Set
    End Property

    <Category("Standard Fields")> _
    <Description("The size of the code (text) section, or the sum of all code sections if there are multiple sections.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property SizeOfCode() As UInteger
        Get
            Return m_SizeOfCode
        End Get
        Set(ByVal value As UInteger)
            m_SizeOfCode = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The combined size of an MS DOS stub, PE header, and section headers rounded up to a multiple of FileAlignment.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property SizeOfHeaders() As UInteger
        Get
            Return m_SizeOfHeaders
        End Get
        Set(ByVal value As UInteger)
            m_SizeOfHeaders = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The size of the local heap space to commit.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property SizeOfHeapCommit() As UInteger
        Get
            Return m_SizeOfHeapCommit
        End Get
        Set(ByVal value As UInteger)
            m_SizeOfHeapCommit = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The size of the local heap space to reserve. Only SizeOfHeapCommit is committed; the rest is made available one page at a time until the reserve size is reached.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property SizeOfHeapReserve() As UInteger
        Get
            Return m_SizeOfHeapReserve
        End Get
        Set(ByVal value As UInteger)
            m_SizeOfHeapReserve = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The size (in bytes) of the image, including all headers, as the image is loaded in memory. It must be a multiple of SectionAlignment.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property SizeOfImage() As UInteger
        Get
            Return m_SizeOfImage
        End Get
        Set(ByVal value As UInteger)
            m_SizeOfImage = value
        End Set
    End Property

    <Category("Standard Fields")> _
    <Description("The size of the initialized data section, or the sum of all such sections if there are multiple data sections.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property SizeOfInitializedData() As UInteger
        Get
            Return m_SizeOfInitializedData
        End Get
        Set(ByVal value As UInteger)
            m_SizeOfInitializedData = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The size of the stack to commit.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property SizeOfStackCommit() As UInteger
        Get
            Return m_SizeOfStackCommit
        End Get
        Set(ByVal value As UInteger)
            m_SizeOfStackCommit = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The size of the stack to reserve. Only SizeOfStackCommit is committed; the rest is made available one page at a time until the reserve size is reached.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property SizeOfStackReserve() As UInteger
        Get
            Return m_SizeOfStackReserve
        End Get
        Set(ByVal value As UInteger)
            m_SizeOfStackReserve = value
        End Set
    End Property

    <Category("Standard Fields")> _
    <Description("The size of the uninitialized data section (BSS), or the sum of all such sections if there are multiple BSS sections.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property SizeOfUninitializedData() As UInteger
        Get
            Return m_SizeOfUninitializedData
        End Get
        Set(ByVal value As UInteger)
            m_SizeOfUninitializedData = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The subsystem that is required to run this image. ")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property Subsystem() As WindowsSubSystem
        Get
            Return m_Subsystem
        End Get
        Set(ByVal value As WindowsSubSystem)
            m_Subsystem = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("Reserved, must be zero.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property Win32VersionValue() As UInteger
        Get
            Return m_Win32VersionValue
        End Get
        Set(ByVal value As UInteger)
            m_Win32VersionValue = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Overridable Sub Save(ByVal writer As BinaryWriter)
        writer.Write(DirectCast(m_Magic, UInt16))
        writer.Write(m_MajorLinkerVersion)
        writer.Write(m_MinorLinkerVersion)
        writer.Write(m_SizeOfCode)
        writer.Write(m_SizeOfInitializedData)
        writer.Write(m_SizeOfUninitializedData)
        writer.Write(m_AddressOfEntryPoint)
        writer.Write(m_BaseOfCode)
        writer.Write(m_BaseOfData)
        writer.Write(m_ImageBase)
        writer.Write(m_SectionAlignment)
        writer.Write(m_FileAlignment)
        writer.Write(m_MajorOperatingSystemVersion)
        writer.Write(m_MinorOperatingSystemVersion)
        writer.Write(m_MajorImageVersion)
        writer.Write(m_MinorImageVersion)
        writer.Write(m_MajorSubsystemVersion)
        writer.Write(m_MinorSubsystemVersion)
        writer.Write(m_Win32VersionValue)
        writer.Write(m_SizeOfImage)
        writer.Write(m_SizeOfHeaders)
        writer.Write(m_CheckSum)
        writer.Write(DirectCast(m_Subsystem, UInt16))
        writer.Write(DirectCast(m_DllCharacteristics, UInt16))
        writer.Write(m_SizeOfStackReserve)
        writer.Write(m_SizeOfStackCommit)
        writer.Write(m_SizeOfHeapReserve)
        writer.Write(m_SizeOfHeapCommit)
        writer.Write(m_LoaderFlags)
        writer.Write(m_NumberOfRvaAndSizes)
        For i As Integer = 0 To m_NumberOfRvaAndSizes - 1
            m_DataDirectories(i).Save(writer)
        Next
    End Sub

#End Region
End Class

Public Class ImageOptionalHeader32Plus
    Inherits ImageOptionalHeader32
#Region "Fields"

    Private Shadows m_BaseOfData As UInteger = 0
    ' Hide it
    ' Pe32+ long memory address fields
    Private Shadows m_ImageBase As ULong
    Private Shadows m_SizeOfHeapCommit As ULong
    Private Shadows m_SizeOfHeapReserve As ULong
    Private Shadows m_SizeOfStackCommit As ULong
    Private Shadows m_SizeOfStackReserve As ULong

#End Region

#Region "Constructors"

    Public Sub New(ByVal image As PEImage, ByVal reader As BinaryReader)
        m_BaseOfData = 0
        m_Image = image

        m_MajorLinkerVersion = reader.ReadByte()
        m_MinorLinkerVersion = reader.ReadByte()
        m_SizeOfCode = reader.ReadUInt32()
        m_SizeOfInitializedData = reader.ReadUInt32()
        m_SizeOfUninitializedData = reader.ReadUInt32()
        m_AddressOfEntryPoint = reader.ReadUInt32()
        m_BaseOfCode = reader.ReadUInt32()

        m_BaseOfData = 0
        ' This field is not used in PE32+
        m_ImageBase = reader.ReadUInt64()

        m_SectionAlignment = reader.ReadUInt32()
        m_FileAlignment = reader.ReadUInt32()
        m_MajorOperatingSystemVersion = reader.ReadUInt16()
        m_MinorOperatingSystemVersion = reader.ReadUInt16()
        m_MajorImageVersion = reader.ReadUInt16()
        m_MinorImageVersion = reader.ReadUInt16()
        m_MajorSubsystemVersion = reader.ReadUInt16()
        m_MinorSubsystemVersion = reader.ReadUInt16()
        m_Win32VersionValue = reader.ReadUInt32()
        m_SizeOfImage = reader.ReadUInt32()
        m_SizeOfHeaders = reader.ReadUInt32()
        m_CheckSum = reader.ReadUInt32()
        m_Subsystem = CType(reader.ReadUInt16(), WindowsSubSystem)
        m_DllCharacteristics = CType(reader.ReadUInt16(), DllCharacteristics)

        m_SizeOfStackReserve = reader.ReadUInt64()
        m_SizeOfStackCommit = reader.ReadUInt64()
        m_SizeOfHeapReserve = reader.ReadUInt64()
        m_SizeOfHeapCommit = reader.ReadUInt64()

        m_LoaderFlags = reader.ReadUInt32()
        m_NumberOfRvaAndSizes = reader.ReadUInt32()

        m_DataDirectories = New OptionalHeaderDataDirectory(m_NumberOfRvaAndSizes - 1) {}
        For i As Integer = 0 To m_NumberOfRvaAndSizes - 1
            m_DataDirectories(i) = New OptionalHeaderDataDirectory(image, reader)
        Next
    End Sub

#End Region

#Region "Properties"

    <Category("Windows-Specific Fields")> _
    <Description("The preferred address of the first byte of image when loaded into memory; must be a multiple of 64 K. The default for DLLs is 0x10000000. The default for Windows CE EXEs is 0x00010000. The default for Windows NT, Windows 2000, Windows XP, Windows 95, Windows 98, and Windows Me is 0x00400000.")> _
    <TypeConverter(GetType(QWORDConverter))> _
    Public Shadows Property ImageBase() As QWORD
        Get
            Return m_ImageBase
        End Get
        Set(ByVal value As QWORD)
            m_ImageBase = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The size of the local heap space to commit.")> _
    <TypeConverter(GetType(QWORDConverter))> _
    Public Shadows Property SizeOfHeapCommit() As QWORD
        Get
            Return m_SizeOfHeapCommit
        End Get
        Set(ByVal value As QWORD)
            m_SizeOfHeapCommit = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The size of the local heap space to reserve. Only SizeOfHeapCommit is committed; the rest is made available one page at a time until the reserve size is reached.")> _
    <TypeConverter(GetType(QWORDConverter))> _
    Public Shadows Property SizeOfHeapReserve() As QWORD
        Get
            Return m_SizeOfHeapReserve
        End Get
        Set(ByVal value As QWORD)
            m_SizeOfHeapReserve = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The size of the stack to commit.")> _
    <TypeConverter(GetType(QWORDConverter))> _
    Public Shadows Property SizeOfStackCommit() As QWORD
        Get
            Return m_SizeOfStackCommit
        End Get
        Set(ByVal value As QWORD)
            m_SizeOfStackCommit = value
        End Set
    End Property

    <Category("Windows-Specific Fields")> _
    <Description("The size of the stack to reserve. Only SizeOfStackCommit is committed; the rest is made available one page at a time until the reserve size is reached.")> _
    <TypeConverter(GetType(QWORDConverter))> _
    Public Shadows Property SizeOfStackReserve() As QWORD
        Get
            Return m_SizeOfStackReserve
        End Get
        Set(ByVal value As QWORD)
            m_SizeOfStackReserve = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Overrides Sub Save(ByVal writer As BinaryWriter)
        writer.Write(DirectCast(m_Magic, UInt16))
        writer.Write(m_MajorLinkerVersion)
        writer.Write(m_MinorLinkerVersion)
        writer.Write(m_SizeOfCode)
        writer.Write(m_SizeOfInitializedData)
        writer.Write(m_SizeOfUninitializedData)
        writer.Write(m_AddressOfEntryPoint)
        writer.Write(m_BaseOfCode)
        ' No BaseOfData
        writer.Write(m_ImageBase)
        writer.Write(m_SectionAlignment)
        writer.Write(m_FileAlignment)
        writer.Write(m_MajorOperatingSystemVersion)
        writer.Write(m_MinorOperatingSystemVersion)
        writer.Write(m_MajorImageVersion)
        writer.Write(m_MinorImageVersion)
        writer.Write(m_MajorSubsystemVersion)
        writer.Write(m_MinorSubsystemVersion)
        writer.Write(m_Win32VersionValue)
        writer.Write(m_SizeOfImage)
        writer.Write(m_SizeOfHeaders)
        writer.Write(m_CheckSum)
        writer.Write(DirectCast(m_Subsystem, UInt16))
        writer.Write(DirectCast(m_DllCharacteristics, UInt16))
        writer.Write(m_SizeOfStackReserve)
        writer.Write(m_SizeOfStackCommit)
        writer.Write(m_SizeOfHeapReserve)
        writer.Write(m_SizeOfHeapCommit)
        writer.Write(m_LoaderFlags)
        writer.Write(m_NumberOfRvaAndSizes)
        For i As Integer = 0 To m_NumberOfRvaAndSizes - 1
            m_DataDirectories(i).Save(writer)
        Next
    End Sub

#End Region
End Class

Public Class OptionalHeaderDataDirectory
#Region "Fields"

    Private m_Image As PEImage
    Private m_Size As UInteger
    Private m_VirtualAddress As UInteger

#End Region

#Region "Constructors"

    Public Sub New(ByVal image As PEImage, ByVal reader As BinaryReader)
        m_Image = image

        m_VirtualAddress = reader.ReadUInt32()
        m_Size = reader.ReadUInt32()
    End Sub

#End Region

#Region "Properties"

    <Category("DataDirectory")> _
    <Description("The size in bytes")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property Size() As UInteger
        Get
            Return m_Size
        End Get
        Set(ByVal value As UInteger)
            m_Size = value
        End Set
    End Property

    <Category("DataDirectory")> _
    <Description("VirtualAddress is actually the RVA of the table. The RVA is the address of the table relative to the base address of the image when the table is loaded.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property VirtualAddress() As UInteger
        Get
            Return m_VirtualAddress
        End Get
        Set(ByVal value As UInteger)
            m_VirtualAddress = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Function GetBytes() As Byte()
        Dim data As Byte() = New Byte(m_Size - 1) {}
        Using fileStream As New FileStream(m_Image.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
            Dim offset As UInteger = m_Image.RvaToOffset(m_VirtualAddress)
            fileStream.Seek(CLng(offset), SeekOrigin.Begin)
            fileStream.Read(data, 0, CInt(m_Size))
        End Using
        Return data
    End Function

    Public Sub Save(ByVal writer As BinaryWriter)
        writer.Write(m_VirtualAddress)
        writer.Write(m_Size)
    End Sub

#End Region
End Class