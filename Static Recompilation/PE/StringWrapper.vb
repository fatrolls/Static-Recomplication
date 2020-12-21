Public Module StringWrapper
    Public Function StringWrapper(ByVal str As String) As String
        Dim newstr As String = ""
        For Each c As Char In Str
            If c = "\\" Then
                newstr += "\\\\"
            ElseIf c = """" Then
                newstr += "\\"""
            ElseIf c = "\r" Then
                newstr += "\\r"
            ElseIf c = "\n" Then
                newstr += "\\n"
            ElseIf AscW(c) > 128 Then
                newstr += "\\" + "&O" & AscW(c)
            Else
                newstr += c
            End If
        Next
        Return newstr
    End Function

    Public Function to_c(ByVal format_spec As String, ByVal args() As Object)
        Dim new_args As New List(Of String)
        For Each arg As Object In args
            If TypeOf arg Is String Then
                arg = StringWrapper(arg)
            ElseIf TypeOf arg Is Boolean Then
                If DirectCast(arg, Boolean) = True Then
                    arg = "true"
                Else
                    arg = "false"
                End If
            End If
            new_args.Add(DirectCast(arg, String))
        Next
        Return String.Format(format_spec, new_args.ToArray())
    End Function
End Module
