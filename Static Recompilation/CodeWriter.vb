Imports System.IO
Imports System.Reflection

Public Class CodeWriter
    Public indentation As Long = 0
    Public comment As String = ""
    Public line_count As Long = 0
    Dim filename As String = ""

    Dim fp As StreamWriter
    Dim LICENSE_TEXT As String = "/*" & vbNewLine & _
    "Static Recompilation" & vbNewLine & _
    "www.CamSpark.com" & vbNewLine & _
    "www.HighGamer.com" & vbNewLine & _
    "By Fatrolls"

    Public Sub New(ByVal filename As String, Optional ByVal license As Boolean = True)
        Me.filename = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), filename)
        fp = New StreamWriter(Me.filename)
        If license Then
            putln(LICENSE_TEXT)
        End If
    End Sub

    Public Function format_line(ByVal line As String) As String
        Return get_spaces() + line
    End Function

    Public Sub putln(ByVal ParamArray lines() As String)
        For Each line As String In lines
            line = format_line(line)
            If comment Then
                line = String.Format("{0} // {1}", line, comment)
            End If
            fp.Write(line + vbNewLine)
            line_count += 1
        Next
    End Sub

    Public Sub putlnc(ByVal line As String, ByVal ParamArray arg() As String)
        line = to_c(line, arg)
        putln(line)
    End Sub

    Public Function get_data() As String
        Dim data As String = ""
        Using textReader As New System.IO.StreamReader(filename)
            data = textReader.ReadToEnd
        End Using
        Return data
    End Function

    Public Sub start_brace()
        fp.Write("{")
        indent()
    End Sub

    Public Sub end_brace(Optional ByVal semicolon = False)
        dedent()
        Dim text As String = "}"
        If semicolon Then
            text += ";"
        End If
        putln(text)
    End Sub

    Public Sub put_method(ByVal name As String, ByVal ParamArray arg() As String)
        'creates without args like   void blah()
        'creates with args like      void blah(foo, bar)
        putln(String.Format("{0}({1})", name, String.Join(", ", arg)))
        start_brace()
    End Sub

    Public Sub put_label(ByVal name As String)
        putln(String.Format("{0}: ;", name))
    End Sub

    Public Sub start_guard(ByVal name As String)
        putln(String.Format("#ifndef {0}", name))
        putln(String.Format("#define {0}", name))
        putln("")
    End Sub

    Public Sub close_guard(ByVal name As String)
        putln("")
        putln(String.Format("#endif // {0}", name))
        putln("")
    End Sub

    Public Sub indent()
        indentation += 1
    End Sub

    Public Sub dedent()
        indentation -= 1
        If indentation < 0 Then
            Throw New Exception("indentation cannot be lower than 0")
        End If
    End Sub

    Public Function get_spaces(Optional ByVal extra = 0) As String
        Return Left("    ", indentation + extra)
    End Function

    Public Sub close()
        fp.Flush()
        fp.Close()
    End Sub
End Class
