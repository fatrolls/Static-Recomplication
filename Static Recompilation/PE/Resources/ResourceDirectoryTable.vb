
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Text

Public Class ResourceDirectoryTable
#Region "Fields"

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Characteristics As UInteger
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_DateTimeStamp As UInteger
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Entries As List(Of ResourceDirectoryEntry) = Nothing
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Image As PEImage = Nothing
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_MajorVersion As UShort
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_MinorVersion As UShort
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_NumberOfIdEntries As UShort
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_NumberOfNameEntries As UShort

#End Region

#Region "Constructors"

    Public Sub New(ByVal image As PEImage, ByVal reader As BinaryReader)
        m_Image = image

        m_Characteristics = reader.ReadUInt32()
        m_DateTimeStamp = reader.ReadUInt32()
        m_MajorVersion = reader.ReadUInt16()
        m_MinorVersion = reader.ReadUInt16()
        m_NumberOfNameEntries = reader.ReadUInt16()
        m_NumberOfIdEntries = reader.ReadUInt16()
        m_Entries = New List(Of ResourceDirectoryEntry)(m_NumberOfNameEntries + m_NumberOfIdEntries)
        For i As Integer = 0 To m_NumberOfNameEntries - 1
            m_Entries.Add(New ResourceDirectoryEntry(image, reader, ResourceDirectoryEntryType.Name))
        Next

        For i As Integer = 0 To m_NumberOfIdEntries - 1
            m_Entries.Add(New ResourceDirectoryEntry(image, reader, ResourceDirectoryEntryType.Id))
        Next
    End Sub

#End Region

#Region "Properties"

    <Description("Resource flags. This field is reserved for future use. It is currently set to zero.")> _
    Public Property Characteristics() As UInteger
        Get
            Return m_Characteristics
        End Get
        Set(ByVal value As UInteger)
            m_Characteristics = value
        End Set
    End Property

    <Description("The time that the resource data was created by the resource compiler.")> _
    Public Property DateTimeStamp() As UInteger
        Get
            Return m_DateTimeStamp
        End Get
        Set(ByVal value As UInteger)
            m_DateTimeStamp = value
        End Set
    End Property

    <Browsable(False)> _
    Public ReadOnly Property Entries() As ResourceDirectoryEntry()
        Get
            Return m_Entries.ToArray()
        End Get
    End Property

    <Description("The major version number, set by the user.")> _
    Public Property MajorVersion() As WORD
        Get
            Return m_MajorVersion
        End Get
        Set(ByVal value As WORD)
            m_MajorVersion = value
        End Set
    End Property

    <Description("The minor version number, set by the user.")> _
    Public Property MinorVersion() As WORD
        Get
            Return m_MinorVersion
        End Get
        Set(ByVal value As WORD)
            m_MinorVersion = value
        End Set
    End Property

    <Description("The number of directory entries immediately following the table that use strings to identify Type, Name, or Language entries (depending on the level of the table).")> _
    Public Property NumberOfIdEntries() As WORD
        Get
            Return m_NumberOfIdEntries
        End Get
        Set(ByVal value As WORD)
            m_NumberOfIdEntries = value
        End Set
    End Property

    <Description("The number of directory entries immediately following the Name entries that use numeric IDs for Type, Name, or Language entries.")> _
    Public Property NumberOfNameEntries() As WORD
        Get
            Return m_NumberOfNameEntries
        End Get
        Set(ByVal value As WORD)
            m_NumberOfNameEntries = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Overrides Function ToString() As String
        Return "Resource Directory Table"
    End Function

#End Region
End Class
