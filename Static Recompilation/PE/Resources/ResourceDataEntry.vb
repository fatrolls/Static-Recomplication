
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.IO
Imports System.Text

Public Class ResourceDataEntry
#Region "Fields"

    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Codepage As UInteger
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_DataRVA As UInteger
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Image As PEImage = Nothing
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Reserved As UInteger
    <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
    Private m_Size As UInteger

#End Region

#Region "Constructors"

    Public Sub New(ByVal image As PEImage, ByVal reader As BinaryReader)
        m_Image = image

        m_DataRVA = reader.ReadUInt32()
        m_Size = reader.ReadUInt32()
        m_Codepage = reader.ReadUInt32()
        m_Reserved = reader.ReadUInt32()
    End Sub

#End Region

#Region "Properties"

    Public Property Codepage() As UInteger
        Get
            Return m_Codepage
        End Get
        Set(ByVal value As UInteger)
            m_Codepage = value
        End Set
    End Property

    Public Property DataRVA() As UInteger
        Get
            Return m_DataRVA
        End Get
        Set(ByVal value As UInteger)
            m_DataRVA = value
        End Set
    End Property

    Public Property Reserved() As UInteger
        Get
            Return m_Reserved
        End Get
        Set(ByVal value As UInteger)
            m_Reserved = value
        End Set
    End Property

    Public Property Size() As UInteger
        Get
            Return m_Size
        End Get
        Set(ByVal value As UInteger)
            m_Size = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Overrides Function ToString() As String
        Return String.Format("ResourceDataEntry")
    End Function

#End Region
End Class
