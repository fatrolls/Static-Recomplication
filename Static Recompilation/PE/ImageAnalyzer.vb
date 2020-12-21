
Imports System.Collections.Generic
Imports System.Text
Imports System.Diagnostics

Public Module ImageAnalyzer
    Public Function AnalyzeAPI(ByVal processModule As ProcesssModule) As Boolean
        Dim dmp As Byte() = processModule.GetBytes()
        Dim img As PEImage = PEImage.ReadImage(processModule.FileName)
        Return True
    End Function
End Module
