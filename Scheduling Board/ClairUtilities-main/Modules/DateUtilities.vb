Public Module DateUtilities
    Private Const ERR_INVALID_DATE = 20000
    Private Const ERR_INVALID_DATE_MSG = "Date Required"

    Public Function DaysBetween(ByVal fromdate As Date, ByVal todate As Date) As Integer
        Return (1 + (todate - fromdate).Days)
    End Function

    Public Function Max(d1 As Date, d2 As Date) As Date
        Return If(d1 > d2, d1, d2)
    End Function

    Public Function Min(d1 As Date, d2 As Date) As Date
        Return If(d1 < d2, d1, d2)
    End Function

    Public Function AddBusinessDays(ByVal StartDate As DateTime, numDays As Integer) As DateTime
        Dim newDate As DateTime = StartDate
        While (numDays > 0)
            newDate = newDate.AddDays(1)
            If (newDate.DayOfWeek <> DayOfWeek.Saturday And newDate.DayOfWeek <> DayOfWeek.Sunday) Then
                numDays -= 1
            End If
        End While
        Return newDate
    End Function

    Public Function IsWeekend(ByVal DateValue As Object) As Boolean

        Dim dDateValue As Date

        If Not IsDate(DateValue) Then
            Err.Raise(ERR_INVALID_DATE, ERR_INVALID_DATE_MSG)
            IsWeekend = False
            Exit Function
        End If

        dDateValue = CDate(DateValue)
        IsWeekend = (Weekday(dDateValue) Mod 6 = 1)

    End Function

End Module
