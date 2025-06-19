Public Module Grammar

    Public Function WasWere(ByVal qty As Integer) As String
        Select Case qty
            Case 1
                Return "was"
            Case Else
                Return "were"
        End Select
    End Function

    Public Function IsAre(ByVal qty As Integer) As String
        Select Case qty
            Case 1
                Return "is"
            Case Else
                Return "are"
        End Select
    End Function

    Public Function NounPlural(qty As Integer, singular As String, plural As String) As String
        Select Case qty
            Case 1
                Return singular
            Case Else
                Return plural
        End Select
    End Function

End Module
