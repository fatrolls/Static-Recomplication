Public Class Section
    Public VirtualAddress As UInteger
    Public SizeOfRawData As UInteger
    Public Misc_VirtualSize As UInteger
    Public PointerToRawData As UInteger
    Public Name As String
    Public data_adj As UInteger
    Public virtual_adj As UInteger
    Public pe As PEImage

    Public Sub New(ByVal pe As PEImage, ByVal pe_section As SectionHeader)
        Me.pe = pe
        Me.VirtualAddress = pe_section.VirtualAddress
        Me.SizeOfRawData = pe_section.SizeOfRawData
        Me.Misc_VirtualSize = pe_section.VirtualSize
        Me.PointerToRawData = pe_section.PointerToRawData
        Me.Name = pe_section.Name.Trim()
        Me.data_adj = pe_section.adjust_FileAlignment(pe_section.PointerToRawData, pe.PeHeader.OptionalHeader.FileAlignment)
        Me.virtual_adj = pe_section.adjust_SectionAlignment( _
            pe_section.VirtualAddress,
            pe.PeHeader.OptionalHeader.SectionAlignment,
            pe.PeHeader.OptionalHeader.FileAlignment)
    End Sub

    Public Function get_data(Optional ByVal offset As UInteger = 0, Optional ByVal length As UInteger = 0) As Byte()
        Dim offset_start As UInteger
        Dim offset_end As UInteger

        If offset = 0 Then
            offset_start = Me.data_adj
        Else
            offset_start = (offset - Me.virtual_adj) + Me.data_adj
        End If

        If length <> 0 Then
            offset_end = offset_start + length
        Else
            offset_end = offset_start + Me.SizeOfRawData
        End If

        ' PointerToRawData is not adjusted here as we might want to read any
        ' possible extra bytes that might get cut off by aligning the start
        ' (and hence cutting something off the end)

        If offset_end > Me.PointerToRawData + Me.SizeOfRawData Then
            offset_end = Me.PointerToRawData + Me.SizeOfRawData
        End If
        Return pe.GetDataAtOffset(offset_start, offset_end)
    End Function
End Class
