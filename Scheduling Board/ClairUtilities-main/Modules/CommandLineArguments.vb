Module CommandLineArguments
    Public Function GetCommandLineArgument(ByVal argumentName As String) As String
        For Each arg In My.Application.CommandLineArgs
            Dim splitArg = arg.Split({"="c, ":"c}, 2)
            If String.Compare(splitArg(0), argumentName, True) = 0 AndAlso splitArg.Length > 1 Then
                Return splitArg(1)
            End If
        Next

        Return String.Empty
    End Function
End Module
