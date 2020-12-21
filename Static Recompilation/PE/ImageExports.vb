
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Text

Public Class ImageExport
#Region "Fields"

    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_AddressRVA As UInteger
    Private m_Directory As ImageExportDirectoryEntry = Nothing
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_ForwardedTo As String
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_Name As String
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_Ordinal As UInteger

#End Region

#Region "Constructors"

    Public Sub New(ByVal directory As ImageExportDirectoryEntry, ByVal addressRVA As UInteger, ByVal ordinal As UInteger, ByVal name As String, ByVal forwarder As String)
        m_Directory = directory

        m_AddressRVA = addressRVA
        m_Ordinal = ordinal
        m_Name = name
        m_ForwardedTo = forwarder
    End Sub

#End Region

#Region "Properties"

    <TypeConverter(GetType(DWordConverter))> _
    Public Property AddressRVA() As UInteger
        Get
            Return m_AddressRVA
        End Get
        Set(ByVal value As UInteger)
            m_AddressRVA = value
        End Set
    End Property

    <Browsable(False)> _
    Public Property Directory() As ImageExportDirectoryEntry
        Get
            Return m_Directory
        End Get
        Set(ByVal value As ImageExportDirectoryEntry)
            m_Directory = value
        End Set
    End Property

    Public Property Forwarder() As String
        Get
            Return m_ForwardedTo
        End Get
        Set(ByVal value As String)
            m_ForwardedTo = value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property IsForwarded() As Boolean
        Get
            Return m_ForwardedTo <> "-"
        End Get
    End Property

    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(ByVal value As String)
            m_Name = value
        End Set
    End Property

    <TypeConverter(GetType(DWordConverter))> _
    Public Property Ordinal() As UInteger
        Get
            Return m_Ordinal
        End Get
        Set(ByVal value As UInteger)
            m_Ordinal = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Overrides Function ToString() As String
        If m_Directory Is Nothing Then
            If m_Name <> "-" Then
                ' Not by ordinal
                Return String.Format("{0}", m_Name)
            End If

            Return String.Format("#{0}", m_Ordinal.ToString())
        End If
        If m_Name <> "-" Then
            Return String.Format("{0}.{1} ", m_Directory.DllName, m_Name)
        End If
        Return String.Format("{0}.#{1}", m_Directory.DllName, m_Ordinal.ToString())
    End Function

#End Region
End Class

Public Class ImageExportDirectoryEntry
#Region "Fields"

    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_AddressTableEntries As UInteger
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_DllName As String
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_ExportAddressTableRVA As UInteger
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_ExportFlags As UInteger
    'RVA of Export lookup table
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_Exports As New List(Of ImageExport)()
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_MajorVersion As UShort
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_MinorVersion As UShort
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_NamePointerRVA As UInteger
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_NameRVA As UInteger
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_NumberOfNamePointers As UInteger
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_OrdinalBase As UInteger
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_OrdinalTableRVA As UInteger
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_TimeDateStamp As UInteger

#End Region

#Region "Constructors"

    Friend Sub New()
    End Sub

    Friend Sub New(ByVal image As PEImage, ByVal reader As BinaryReader)
        m_ExportFlags = reader.ReadUInt32()
        ' Reserved, must be 0
        m_TimeDateStamp = reader.ReadUInt32()
        m_MajorVersion = reader.ReadUInt16()
        m_MinorVersion = reader.ReadUInt16()
        m_NameRVA = reader.ReadUInt32()
        m_OrdinalBase = reader.ReadUInt32()
        m_AddressTableEntries = reader.ReadUInt32()
        m_NumberOfNamePointers = reader.ReadUInt32()
        m_ExportAddressTableRVA = reader.ReadUInt32()
        m_NamePointerRVA = reader.ReadUInt32()
        m_OrdinalTableRVA = reader.ReadUInt32()

        If m_OrdinalBase <> 1 Then
            Debug.WriteLine("OrdinalBase, check this")
        End If
        If m_NumberOfNamePointers <> m_AddressTableEntries Then
            Debug.WriteLine("Number of entries, check this")
        End If

        reader.BaseStream.Position = image.RvaToOffset(m_NameRVA)
        m_DllName = StreamHelper.ReadString(reader)
        'if (m_DllName == "MSDbg2.dll")
        '    Debugger.Break();

        ' First read all the tables
        ' Export Address Table
        Dim exportAddressTable As UInteger() = New UInteger(m_AddressTableEntries - 1) {}
        reader.BaseStream.Position = image.RvaToOffset(m_ExportAddressTableRVA)
        For i As Integer = 0 To m_AddressTableEntries - 1
            exportAddressTable(i) = reader.ReadUInt32()
        Next

        ' Export ordinal table
        Dim exportOrdinalTable As UShort() = New UShort(m_NumberOfNamePointers - 1) {}
        reader.BaseStream.Position = image.RvaToOffset(m_OrdinalTableRVA)
        For i As Integer = 0 To m_NumberOfNamePointers - 1
            exportOrdinalTable(i) = reader.ReadUInt16()
        Next

        ' Export NamePointer table
        Dim exportNamePointerTable As UInteger() = New UInteger(m_NumberOfNamePointers - 1) {}
        reader.BaseStream.Position = image.RvaToOffset(m_NamePointerRVA)
        For i As Integer = 0 To m_NumberOfNamePointers - 1
            exportNamePointerTable(i) = reader.ReadUInt32()
        Next

        ' Export names
        Dim exportName As String() = New String(m_NumberOfNamePointers - 1) {}
        For i As Integer = 0 To m_NumberOfNamePointers - 1

            reader.BaseStream.Position = image.RvaToOffset(exportNamePointerTable(i))
            exportName(i) = StreamHelper.ReadString(reader)
        Next

        Dim ex As ImageExport() = New ImageExport(m_AddressTableEntries - 1) {}
        Dim sectionStart As UInteger = image.PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_EXPORT)).VirtualAddress
        Dim sectionEnd As UInteger = sectionStart + image.PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_EXPORT)).Size
        For i As UInteger = 0 To m_AddressTableEntries - 1
            Dim addressRVA As UInteger = exportAddressTable(i)
            Dim ordinal As UInteger = i + m_OrdinalBase
            Dim forwarder As String = "-"
            Dim name As String = "-"

            If addressRVA >= sectionStart AndAlso addressRVA <= sectionEnd Then
                reader.BaseStream.Position = image.RvaToOffset(addressRVA)
                Dim f As String = StreamHelper.ReadString(reader)
                If f.IndexOf("."c) <> -1 Then
                    forwarder = f
                Else
                    Trace.WriteLine("Invalid forwarder string " + image.FileName)
                End If
            End If
            ' Find the export name by ordinal
            For j As Integer = 0 To exportOrdinalTable.Length - 1
                If i = exportOrdinalTable(j) Then
                    name = exportName(j)
                    Exit For
                End If
            Next
            ex(i) = New ImageExport(Me, addressRVA, ordinal, name, forwarder)
        Next
        m_Exports = New List(Of ImageExport)(ex)
    End Sub

#End Region

#Region "Properties"

    <Category("ImageExportDirectory")> _
    <Description("The number of entries in the export address table.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property AddressTableEntries() As UInteger
        Get
            Return m_AddressTableEntries
        End Get
        Set(ByVal value As UInteger)
            m_AddressTableEntries = value
        End Set
    End Property

    <Category("ImageExportDirectory")> _
    <Description("The name of the DLL.")> _
    Public Property DllName() As String
        Get
            Return m_DllName
        End Get
        Set(ByVal value As String)
            m_DllName = value
        End Set
    End Property

    <Category("ImageExportDirectory")> _
    <Description("The address of the export address table, relative to the image base.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property ExportAddressTableRVA() As UInteger
        Get
            Return m_ExportAddressTableRVA
        End Get
        Set(ByVal value As UInteger)
            m_ExportAddressTableRVA = value
        End Set
    End Property

    <Category("ImageExportDirectory")> _
    <Description("Reserved, must be 0.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property ExportFlags() As UInteger
        Get
            Return m_ExportFlags
        End Get
        Set(ByVal value As UInteger)
            m_ExportFlags = value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Exports() As List(Of ImageExport)
        Get
            Return m_Exports
        End Get
    End Property

    <Category("ImageExportDirectory")> _
    <Description("The major version number. The major and minor version numbers can be set by the user.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property MajorVersion() As WORD
        Get
            Return m_MajorVersion
        End Get
        Set(ByVal value As WORD)
            m_MajorVersion = value
        End Set
    End Property

    <Category("ImageExportDirectory")> _
    <Description("The minor version number.")> _
    <TypeConverter(GetType(WORDConverter))> _
    Public Property MinorVersion() As WORD
        Get
            Return m_MinorVersion
        End Get
        Set(ByVal value As WORD)
            m_MinorVersion = value
        End Set
    End Property

    <Category("ImageExportDirectory")> _
    <Description("The address of the export name pointer table, relative to the image base. The table size is given by the Number of Name Pointers field.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property NamePointerRVA() As UInteger
        Get
            Return m_NamePointerRVA
        End Get
        Set(ByVal value As UInteger)
            m_NamePointerRVA = value
        End Set
    End Property

    <Category("ImageExportDirectory")> _
    <Description("The address of the ASCII string that contains the name of the DLL. This address is relative to the image base.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property NameRVA() As UInteger
        Get
            Return m_NameRVA
        End Get
        Set(ByVal value As UInteger)
            m_NameRVA = value
        End Set
    End Property

    <Category("ImageExportDirectory")> _
    <Description("The number of entries in the name pointer table. This is also the number of entries in the ordinal table.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property NumberOfNamePointers() As UInteger
        Get
            Return m_NumberOfNamePointers
        End Get
        Set(ByVal value As UInteger)
            m_NumberOfNamePointers = value
        End Set
    End Property

    <Category("ImageExportDirectory")> _
    <Description("The starting ordinal number for exports in this image. This field specifies the starting ordinal number for the export address table. It is usually set to 1.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property OrdinalBase() As UInteger
        Get
            Return m_OrdinalBase
        End Get
        Set(ByVal value As UInteger)
            m_OrdinalBase = value
        End Set
    End Property

    <Category("ImageExportDirectory")> _
    <Description("The address of the ordinal table, relative to the image base.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property OrdinalTableRVA() As UInteger
        Get
            Return m_OrdinalTableRVA
        End Get
        Set(ByVal value As UInteger)
            m_OrdinalTableRVA = value
        End Set
    End Property

    <Category("ImageExportDirectory")> _
    <Description("The time and date that the export data was created.")> _
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
End Class
