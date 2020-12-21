
Imports System.Collections.Generic
Imports System.IO
Imports System.Text

Public NotInheritable Class StreamHelper
    Private Sub New()
    End Sub
#Region "Methods"

    Public Shared Function ReadString(ByVal reader As BinaryReader) As String
        Dim bytes As New List(Of Byte)()
        Dim b As Byte = reader.ReadByte()
        While b <> 0
            bytes.Add(b)
            b = reader.ReadByte()
        End While
        Return New String(Encoding.ASCII.GetChars(bytes.ToArray()))
    End Function

#End Region

#Region "Other"

    'public static string ReadUnicodeString(BinaryReader reader)
    '{
    '    List<byte> bytes = new List<byte>();
    '    byte b = reader.ReadByte();
    '    while (b != 0)
    '    {
    '        bytes.Add(b);
    '        b = reader.ReadByte();
    '    }
    '    return new string(Encoding.ASCII.GetChars(bytes.ToArray()));
    '}

#End Region
End Class
