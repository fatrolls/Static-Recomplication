
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Text

#Region "Enumerations"

Public Enum ResourceDirectoryEntryType
    Name
    Id
End Enum

#End Region

Public Class ResourceDirectoryEntry
#Region "Fields"

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_DataEntry As ResourceDataEntry = Nothing
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_DataEntryRVA As UInteger
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Image As PEImage = Nothing
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_IntegerID As UInteger
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Name As String
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_NameRVA As UInteger
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_SubDirectoryTable As ResourceDirectoryTable = Nothing
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_SubdirectoryRVA As UInteger
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Type As ResourceDirectoryEntryType

#End Region

#Region "Constructors"

    Public Sub New(ByVal image As PEImage, ByVal reader As BinaryReader, ByVal type As ResourceDirectoryEntryType)
        m_Image = image
        m_Type = type

        If m_Type = ResourceDirectoryEntryType.Name Then
            m_NameRVA = reader.ReadUInt32()
        Else
            m_IntegerID = reader.ReadUInt32()
        End If

        Dim temp As UInteger = reader.ReadUInt32()
        If (temp And &H80000000UI) = &H80000000UI Then
            ' Is a subdirectory?
            m_SubdirectoryRVA = temp And Not &H80000000UI
            Dim CurrPos As Long = reader.BaseStream.Position
            reader.BaseStream.Seek(m_Image.RvaToOffset(m_SubdirectoryRVA + image.PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_RESOURCE)).VirtualAddress), SeekOrigin.Begin)
            m_SubDirectoryTable = New ResourceDirectoryTable(m_Image, reader)
            reader.BaseStream.Position = CurrPos
        Else
            ' Data Entry
            m_DataEntryRVA = temp
            Dim CurrPos As Long = reader.BaseStream.Position
            reader.BaseStream.Seek(m_Image.RvaToOffset(m_DataEntryRVA + image.PeHeader.OptionalHeader.DataDirectories(CInt(IMAGE_DIRECTORY_ENTRY_TYPES.IMAGE_DIRECTORY_ENTRY_RESOURCE)).VirtualAddress), SeekOrigin.Begin)
            m_DataEntry = New ResourceDataEntry(image, reader)
            reader.BaseStream.Position = CurrPos
        End If
    End Sub

#End Region

#Region "Properties"

    Public Property DataEntry() As ResourceDataEntry
        Get
            Return m_DataEntry
        End Get
        Set(ByVal value As ResourceDataEntry)
            m_DataEntry = value
        End Set
    End Property

    Public Property DataEntryRVA() As UInteger
        Get
            Return m_DataEntryRVA
        End Get
        Set(ByVal value As UInteger)
            m_DataEntryRVA = value
        End Set
    End Property

    Public Property IntegerID() As UInteger
        Get
            Return m_IntegerID
        End Get
        Set(ByVal value As UInteger)
            m_IntegerID = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(ByVal value As String)
            m_Name = value
        End Set
    End Property

    Public Property NameRVA() As UInteger
        Get
            Return m_NameRVA
        End Get
        Set(ByVal value As UInteger)
            m_NameRVA = value
        End Set
    End Property

    Public Property SubDirectoryTable() As ResourceDirectoryTable
        Get
            Return m_SubDirectoryTable
        End Get
        Set(ByVal value As ResourceDirectoryTable)
            m_SubDirectoryTable = value
        End Set
    End Property

    Public Property SubdirectoryRVA() As UInteger
        Get
            Return m_SubdirectoryRVA
        End Get
        Set(ByVal value As UInteger)
            m_SubdirectoryRVA = value
        End Set
    End Property

    Public ReadOnly Property Type() As ResourceDirectoryEntryType
        Get
            Return m_Type
        End Get
    End Property

#End Region

#Region "Methods"

    Public Overrides Function ToString() As String
        If m_Type = ResourceDirectoryEntryType.Id Then
            Return String.Format("Resource Directory Entry {0}", m_IntegerID)
        Else
            Return String.Format("Resource Directory Entry {0}", m_Name)
        End If
    End Function

#End Region
End Class
