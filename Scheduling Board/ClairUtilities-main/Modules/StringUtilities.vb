Imports System.Runtime.CompilerServices
Imports System.Text

Module StringUtilities

    Function strClean(ByVal strToClean)
        Dim inStringArray()
        ReDim inStringArray(Len(strToClean) - 1)

        Dim currentChar As String = Nothing

        For iterator = 1 To Len(strToClean)
            currentChar = Mid(strToClean, iterator, 1)

            Select Case currentChar
                Case "?", "/", "\", ":", "*", """", "<", ">", "", "#", "~", "%", "{", "}", "+", "_", "."
                    inStringArray(iterator - 1) = "-"
                Case "&"
                    inStringArray(iterator - 1) = " and "
                Case Else
                    inStringArray(iterator - 1) = currentChar
            End Select

        Next

        strClean = Join(inStringArray, "")

    End Function

    Function HTMLtoPlainText(ByVal StringToClean As String)

        Dim lStringReplacements As New List(Of String())
        lStringReplacements.Add({"<p>", String.Empty})
        lStringReplacements.Add({"</p>", vbCrLf & vbCrLf})
        lStringReplacements.Add({"<ol><li>", "- "})
        lStringReplacements.Add({"</li><li>", "- "})
        lStringReplacements.Add({"</li></ol>", String.Empty})
        lStringReplacements.Add({"<br>", vbCrLf})
        lStringReplacements.Add({"<br />", vbCrLf})
        lStringReplacements.Add({"&#39;", "'"})

        For Each s In lStringReplacements
            StringToClean = Replace(StringToClean, s(0), s(1))
        Next

        Return StringToClean
    End Function

    <Extension()>
    Public Function Format(ByVal s As StringBuilder, ParamArray args() As Object) As String
        Return String.Format(s.ToString, args)
    End Function

    Public Function Get_SizeFromString(ByVal s As String)
        Dim size As New Global.System.Drawing.Size

        Try
            Dim ary As String() = s.Split(",")

            size.Width = Replace(ary(0), "{Width=", String.Empty)
            size.Height = Replace(Replace(ary(1), "Height=", String.Empty), "}", String.Empty)
        Catch ex As Exception
            size.Width = 1024
            size.Height = 768
        End Try

        Return size
    End Function

    Public Function Get_LocationFromString(ByVal s As String)
        Dim l As New Global.System.Drawing.Point

        Try
            Dim ary As String() = s.Split(",")

            l.X = Replace(ary(0), "{X=", String.Empty)
            l.Y = Replace(Replace(ary(1), "Y=", String.Empty), "}", String.Empty)
        Catch ex As Exception
            l.X = 0
            l.Y = 0
        End Try

        Return l
    End Function

End Module
