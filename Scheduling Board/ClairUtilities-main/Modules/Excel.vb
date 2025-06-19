Module ExcelUtilities
    Public Function xlWholeColumn(ByVal column As Integer) As String
        Dim xl = xlColumn(column)
        Return xl & ":" & xl
    End Function

    Public Function xlColumn(ByVal column As Integer) As String
        Const xlColChars As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"

        Debug.Assert(column > 0)

        column -= 1

        Dim ret As String = ""

        Do While column >= 0
            ret = xlColChars.Substring(column Mod 26, 1) & ret
            If column = 0 Then
                Exit Do
            End If
            column = (column \ 26) - 1
        Loop

        Return ret
    End Function

    Public Function xlRC(ByVal row As Integer, ByVal column As Integer) As String
        Debug.Assert(row > 0)
        Return xlColumn(column) & row
    End Function
End Module
