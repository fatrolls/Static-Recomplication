
Imports System.Collections
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text

#Region "Enumerations"

Public Enum BaseRelocationType As UInteger
    IMAGE_REL_BASED_ABSOLUTE = 0
    IMAGE_REL_BASED_HIGH = 1
    IMAGE_REL_BASED_LOW = 2
    IMAGE_REL_BASED_HIGHLOW = 3
    IMAGE_REL_BASED_HIGHADJ = 4
    IMAGE_REL_BASED_MIPS_JMPADDR = 5
    IMAGE_REL_BASED_MIPS_JMPADDR16 = 9
    IMAGE_REL_BASED_DIR64
End Enum

#End Region

Public Class BaseRelocationBlock
#Region "Fields"

    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_BlockSize As UInteger
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_Entries As List(Of RelocationPageEntry)
    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_PageRVA As UInteger

#End Region

#Region "Constructors"

    Public Sub New(ByVal reader As BinaryReader)
        m_PageRVA = reader.ReadUInt32()
        m_BlockSize = reader.ReadUInt32()
        Dim numberOfEntries As UInteger = (m_BlockSize - 8) / 2
        m_Entries = New List(Of RelocationPageEntry)(CInt(numberOfEntries))
        For i As Integer = 0 To numberOfEntries - 1
            m_Entries.Add(New RelocationPageEntry(reader))
        Next
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property BlockSize() As UInteger
        Get
            Return m_BlockSize
        End Get
    End Property

    Public ReadOnly Property Entries() As List(Of RelocationPageEntry)
        Get
            Return m_Entries
        End Get
    End Property

    Public ReadOnly Property PageRVA() As UInteger
        Get
            Return m_PageRVA
        End Get
    End Property

#End Region
End Class

Public Class BaseRelocationBlockCollection
    Inherits CollectionBase
#Region "Constructors"

    Friend Sub New()
    End Sub

    Friend Sub New(ByVal entries As BaseRelocationBlock())
        MyBase.InnerList.AddRange(entries)
    End Sub

#End Region

#Region "Indexers"

    ' Properties
    Default Public ReadOnly Property Item(ByVal index As Integer) As BaseRelocationBlock
        Get
            Return DirectCast(MyBase.InnerList(index), BaseRelocationBlock)
        End Get
    End Property

#End Region

#Region "Methods"

    Public Sub CopyTo(ByVal array As BaseRelocationBlock(), ByVal index As Integer)
        MyBase.InnerList.CopyTo(array, index)
    End Sub

    Friend Sub Add(ByVal block As BaseRelocationBlock)
        MyBase.InnerList.Add(block)
    End Sub

#End Region
End Class

Public Class RelocationPageEntry
#Region "Fields"

    <DebuggerBrowsableAttribute(DebuggerBrowsableState.Never)> _
    Private m_Data As UShort

#End Region

#Region "Constructors"

    Friend Sub New(ByVal reader As BinaryReader)
        m_Data = reader.ReadUInt16()
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property EntryData() As WORD
        Get
            Return m_Data
        End Get
    End Property

    Public ReadOnly Property Offset() As UShort
        Get
            Return CUShort(m_Data And &HFFF)
        End Get
    End Property

    Public ReadOnly Property Type() As BaseRelocationType
        Get
            Return CType((m_Data And &HF000) >> 12, BaseRelocationType)
        End Get
    End Property

#End Region
End Class
