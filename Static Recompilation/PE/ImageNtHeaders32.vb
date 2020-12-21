
Imports System.Collections
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.IO
Imports System.Text

#Region "Enumerations"

Public Enum MagicType As UShort
    PE32 = &H10B
    PE32Plus = &H20B
End Enum

#End Region

Public Class ImageNtHeaders32
#Region "Fields"

    Private m_FileHeader As ImageFileHeader
    Private m_Image As PEImage
    Private m_OptionalHeader As ImageOptionalHeader32
    Private m_Signature As String

#End Region

#Region "Constructors"

    Public Sub New(ByVal image As PEImage, ByVal reader As BinaryReader)
        m_Image = image

        m_Signature = Encoding.ASCII.GetString(reader.ReadBytes(4))
        m_FileHeader = New ImageFileHeader(reader)

        Dim magic As MagicType = CType(reader.ReadUInt16(), MagicType)
        If magic = MagicType.PE32 Then
            OptionalHeader = New ImageOptionalHeader32(image, reader)
        Else
            OptionalHeader = New ImageOptionalHeader32Plus(image, reader)
        End If
        m_OptionalHeader.Magic = magic
    End Sub

#End Region

#Region "Properties"

    <Browsable(False)> _
    Public Property FileHeader() As ImageFileHeader
        Get
            Return m_FileHeader
        End Get
        Set(ByVal value As ImageFileHeader)
            m_FileHeader = value
        End Set
    End Property

    <Browsable(False)> _
    Public Property OptionalHeader() As ImageOptionalHeader32
        Get
            Return m_OptionalHeader
        End Get
        Set(ByVal value As ImageOptionalHeader32)
            m_OptionalHeader = value
        End Set
    End Property

    <Description("After the MS DOS stub, at the file offset specified at offset 0x3c, is a 4-byte signature that identifies the file as a PE format image file. This signature is " & ChrW(147) & "PE" & ChrW(148) & vbNullChar & vbNullChar & " (the letters " & ChrW(147) & "P" & ChrW(148) & " and " & ChrW(147) & "E" & ChrW(148) & " followed by two null bytes).")> _
    Public Property Signature() As String
        Get
            Return m_Signature
        End Get
        Set(ByVal value As String)
            m_Signature = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Sub Save(ByVal writer As BinaryWriter)
        writer.Write(m_Signature.ToCharArray())
        m_FileHeader.Save(writer)
        m_OptionalHeader.Save(writer)
    End Sub

#End Region
End Class
