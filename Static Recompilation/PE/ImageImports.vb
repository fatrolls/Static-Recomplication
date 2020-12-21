
Imports System.Collections
Imports System.Collections.Generic
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Text

Public Class ImageImport
#Region "Fields"

    Private m_Hint As UShort
    Private m_IATRVA As UInteger
    Private m_ImportName As String
    Private m_Ordinal As UShort

#End Region

#Region "Constructors"

    Public Sub New(ByVal image As PEImage, ByVal reader As BinaryReader, ByVal importLookupTableEntry As UInteger)
        If importLookupTableEntry >= &H80000000UI Then
            ' Import by ordinal
            m_Ordinal = Convert.ToUInt16(importLookupTableEntry)
            m_ImportName = ""
        Else
            reader.BaseStream.Seek(image.RvaToOffset(importLookupTableEntry), SeekOrigin.Begin)
            m_Hint = reader.ReadUInt16()
            m_ImportName = StreamHelper.ReadString(reader)
        End If
    End Sub

    Public Sub New(ByVal image As PEImage, ByVal reader As BinaryReader, ByVal importLookupTableEntry As ULong)
        If importLookupTableEntry >= &H8000000000000000UL Then
            ' Import by ordinal
            m_Ordinal = Convert.ToUInt16(importLookupTableEntry)
            m_ImportName = ""
        Else
            reader.BaseStream.Seek(image.RvaToOffset(CUInt(importLookupTableEntry)), SeekOrigin.Begin)
            m_Hint = reader.ReadUInt16()
            m_ImportName = StreamHelper.ReadString(reader)
        End If
    End Sub

#End Region

#Region "Properties"

    Public Property Hint() As WORD
        Get
            Return m_Hint
        End Get
        Set(ByVal value As WORD)
            m_Hint = value
        End Set
    End Property

    Public Property IATRVA() As UInteger
        Get
            Return m_IATRVA
        End Get
        Set(ByVal value As UInteger)
            m_IATRVA = value
        End Set
    End Property

    Public Property ImportName() As String
        Get
            Return m_ImportName
        End Get
        Set(ByVal value As String)
            m_ImportName = value
        End Set
    End Property

    Public Property Ordinal() As WORD
        Get
            Return m_Ordinal
        End Get
        Set(ByVal value As WORD)
            m_Ordinal = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Overrides Function ToString() As String
        If ImportName IsNot Nothing Then
            Return String.Format("{0} @{1}", ImportName, Ordinal.ToString())
        End If
        Return String.Format("@{0}", Ordinal.ToString())
    End Function

#End Region
End Class

<[ReadOnly](True)> _
Public Class ImageImportDirectoryEntry
#Region "Fields"

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_ForwarderChain As UInteger
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_ImportLookupRVA As UInteger
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Imports As List(Of ImageImport)
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Name As String = "?"
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_NameRVA As UInteger
    'RVA of name
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_ThunkTableRVA As UInteger
    'RVA of import thunk table
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_TimeDateStamp As UInteger

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal reader As BinaryReader)
        m_ImportLookupRVA = reader.ReadUInt32()
        'RVA of import lookup table
        m_TimeDateStamp = reader.ReadUInt32()
        m_ForwarderChain = reader.ReadUInt32()
        m_NameRVA = reader.ReadUInt32()
        'RVA of name
        'RVA of import thunk table
        m_ThunkTableRVA = reader.ReadUInt32()
    End Sub

#End Region

#Region "Properties"

    <Category("ImageImportDirectoryEntry")> _
    <Description("The index of the first forwarder reference.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property ForwarderChain() As UInteger
        Get
            Return m_ForwarderChain
        End Get
        Set(ByVal value As UInteger)
            m_ForwarderChain = value
        End Set
    End Property

    <Category("ImageImportDirectoryEntry")> _
    <Description("The RVA of the import lookup table. This table contains a name or ordinal for each import. ")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property ImportLookupRVA() As UInteger
        Get
            Return m_ImportLookupRVA
        End Get
        Set(ByVal value As UInteger)
            m_ImportLookupRVA = value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property ImageImports() As List(Of ImageImport)
        Get
            Return m_Imports
        End Get
    End Property

    <Browsable(False)> _
    Public ReadOnly Property IsNull() As Boolean
        Get
            Return m_ImportLookupRVA = 0 AndAlso m_TimeDateStamp = 0 AndAlso m_ForwarderChain = 0 AndAlso m_NameRVA = 0 AndAlso m_ThunkTableRVA = 0
        End Get
    End Property

    <Category("ImageImportDirectoryEntry")> _
    <Description("Name of DLL.")> _
    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(ByVal value As String)
            m_Name = value
        End Set
    End Property

    <Category("ImageImportDirectoryEntry")> _
    <Description("The address of an ASCII string that contains the name of the DLL. This address is relative to the image base.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property NameRVA() As UInteger
        Get
            Return m_NameRVA
        End Get
        Set(ByVal value As UInteger)
            m_NameRVA = value
        End Set
    End Property

    <Category("ImageImportDirectoryEntry")> _
    <Description("The RVA of the import address table. The contents of this table are identical to the contents of the import lookup table until the image is bound.")> _
    <TypeConverter(GetType(DWordConverter))> _
    Public Property ThunkTableRVA() As UInteger
        Get
            Return m_ThunkTableRVA
        End Get
        Set(ByVal value As UInteger)
            m_ThunkTableRVA = value
        End Set
    End Property

    <Category("ImageImportDirectoryEntry")> _
    <Description("The stamp that is set to zero until the image is bound. After the image is bound, this field is set to the time/data stamp of the DLL.")> _
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
        writer.Write(m_ImportLookupRVA)
        writer.Write(m_TimeDateStamp)
        writer.Write(m_ForwarderChain)
        writer.Write(m_NameRVA)
        writer.Write(m_ThunkTableRVA)
    End Sub

    Public Overrides Function ToString() As String
        Return String.Format("{0} Thunk RVA {1}", m_Name, m_ThunkTableRVA)
    End Function

    Friend Sub ReadFunctions(ByVal image As PEImage, ByVal reader As BinaryReader)
        If Not Me.IsNull Then
            ' Read the DllName
            reader.BaseStream.Seek(image.RvaToOffset(m_NameRVA), SeekOrigin.Begin)
            m_Name = StreamHelper.ReadString(reader)

            Dim thunkTable As New ArrayList(100)
            Dim importLookupTable As New ArrayList(100)

            Dim ImageImports As ImageImport() = New ImageImport(-1) {}
            Debug.Assert(m_ThunkTableRVA <> 0, "Invalid thunktable")
            If image.PeHeader.OptionalHeader.Magic = MagicType.PE32 Then
                '#Region "PE32"
                If m_ImportLookupRVA <> 0 Then
                    reader.BaseStream.Seek(image.RvaToOffset(m_ImportLookupRVA), SeekOrigin.Begin)
                    Dim entry As UInteger = 0
                    Do
                        If (InlineAssignHelper(entry, reader.ReadUInt32())) <> 0 Then
                            importLookupTable.Add(entry)
                        End If
                    Loop While entry <> 0
                End If
                If m_ThunkTableRVA <> 0 Then
                    reader.BaseStream.Seek(image.RvaToOffset(m_ThunkTableRVA), SeekOrigin.Begin)
                    Dim entry As UInteger = 0
                    Do
                        If (InlineAssignHelper(entry, reader.ReadUInt32())) <> 0 Then
                            thunkTable.Add(entry)
                        End If
                    Loop While entry <> 0
                End If

                Dim importSource As ArrayList
                ' Check ImportLookupTable
                Dim validImportLookupTable As Boolean = importLookupTable.Count <> 0
                Dim i As Integer = 0
                While i < importLookupTable.Count AndAlso validImportLookupTable
                    If CUInt(importLookupTable(0)) < &H80000000UI Then
                        validImportLookupTable = validImportLookupTable And image.RvaIsValid(CUInt(importLookupTable(0)))
                    End If
                    i += 1
                End While

                If validImportLookupTable Then
                    importSource = importLookupTable
                Else
                    importSource = thunkTable
                End If

                ImageImports = New ImageImport(importSource.Count - 1) {}
                For i = 0 To importSource.Count - 1
                    If CUInt(importSource(i)) <> 0 Then
                        ImageImports(i) = New ImageImport(image, reader, CUInt(importSource(i)))
                        ImageImports(i).IATRVA = CUInt(Me.m_ThunkTableRVA + (i * 4))
                    End If
                    '#End Region
                Next
            Else
                '#Region "PE32Plus"
                If m_ImportLookupRVA <> 0 Then
                    reader.BaseStream.Seek(image.RvaToOffset(m_ImportLookupRVA), SeekOrigin.Begin)
                    Dim entry As ULong = 0
                    Do
                        If (InlineAssignHelper(entry, reader.ReadUInt64())) <> 0 Then
                            importLookupTable.Add(entry)
                        End If
                    Loop While entry <> 0
                End If
                If m_ThunkTableRVA <> 0 Then
                    reader.BaseStream.Seek(image.RvaToOffset(m_ThunkTableRVA), SeekOrigin.Begin)
                    Dim entry As ULong = 0
                    Do
                        If (InlineAssignHelper(entry, reader.ReadUInt64())) <> 0 Then
                            thunkTable.Add(entry)
                        End If
                    Loop While entry <> 0
                End If

                ' Check the entries of the importlookuptable
                Dim importSource As ArrayList
                Dim validImportLookupTable As Boolean = importLookupTable.Count <> 0
                Dim i As Integer = 0
                While i < importLookupTable.Count AndAlso validImportLookupTable
                    If CULng(importLookupTable(0)) < &H8000000000000000UL Then
                        ' Ordinal
                        validImportLookupTable = validImportLookupTable And image.RvaIsValid(Convert.ToUInt32(importLookupTable(0)))
                    End If
                    i += 1
                End While

                If validImportLookupTable Then
                    importSource = importLookupTable
                Else
                    importSource = thunkTable
                End If

                ImageImports = New ImageImport(importSource.Count - 1) {}
                For i = 0 To importSource.Count - 1
                    If CULng(importSource(i)) <> 0 Then
                        ImageImports(i) = New ImageImport(image, reader, CULng(importSource(i)))
                        ImageImports(i).IATRVA = CUInt(Me.m_ThunkTableRVA + (i * 8))
                    End If
                    '#End Region
                Next
            End If
            m_Imports = New List(Of ImageImport)(ImageImports)
        End If
    End Sub
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function

#End Region
End Class
