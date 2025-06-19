Public Module Converters
    Public Function TwipsFromPixelsX(ByVal pixels As Integer) As Integer
        Return Microsoft.VisualBasic.Compatibility.VB6.PixelsToTwipsX(pixels)
    End Function

    Public Function TwipsFromPixelsY(ByVal pixels As Integer) As Integer
        Return Microsoft.VisualBasic.Compatibility.VB6.PixelsToTwipsY(pixels)
    End Function

    Public Function PixelsFromTwipsX(ByVal twips As Integer) As Integer
        Return Microsoft.VisualBasic.Compatibility.VB6.TwipsToPixelsX(twips)
    End Function

    Public Function PixelsFromTwipsY(ByVal twips As Integer) As Integer
        Return Microsoft.VisualBasic.Compatibility.VB6.TwipsToPixelsY(twips)
    End Function

    ''' <summary>
    ''' Convert from a user-entered value in "display units" to "standard units", i.e. the units stored in the database, given a conversion factor.
    ''' Blanks will be converted to zero.
    ''' </summary>
    ''' <param name="userEnteredDisplayValue">Text value entered by the user</param>
    ''' <param name="multiplierDisplayUnitToStandardUnit">Conversion factor from display units to standard units</param>
    ''' <returns></returns>
    Public Function StandardUnitsFromDisplayUnits(userEnteredDisplayValue As String, multiplierDisplayUnitToStandardUnit As Double) As Double
        Dim parsedDisplayValue As Double
        Dim parsedValueInStandardUnit As Double
        If String.IsNullOrEmpty(userEnteredDisplayValue) OrElse Not Double.TryParse(userEnteredDisplayValue, parsedDisplayValue) Then
            parsedValueInStandardUnit = 0
        Else
            parsedValueInStandardUnit = parsedDisplayValue * multiplierDisplayUnitToStandardUnit
        End If

        Return parsedValueInStandardUnit
    End Function

    ''' <summary>
    ''' Format a value stored in the database in a "standard unit" (e.g. pounds or feet) in a "display unit" (e.g. grams or meters), with 
    ''' the specified number of significant digits. 0 and negative values will be converted to blanks.
    ''' </summary>
    ''' <param name="valueToFormat">Value to format (nullable Double)</param>
    ''' <param name="multiplierDisplayUnitToStandardUnit">Conversion factor from display units to standard units</param>
    ''' <param name="significantDigits">The number of digits to display.</param>
    ''' <returns></returns>
    Public Function FormattedDisplayUnitsFromStandardUnits(valueToFormat As Object, multiplierDisplayUnitToStandardUnit As Double, Optional significantDigits As Integer = 5) As String
        Dim formattedValue As String

        If IsDBNull(valueToFormat) OrElse valueToFormat <= 0 Then
            formattedValue = String.Empty
        Else
            Dim fmt As String = "#,0."

            Dim multiplierPoundsToDisplayUnit = 1 / multiplierDisplayUnitToStandardUnit
            Dim valueInDisplayUnits As Double = CDbl(valueToFormat) * multiplierPoundsToDisplayUnit
            Dim digitsToLeftOfDecimalPoint As Integer = 1 + Math.Floor(Math.Log10(valueInDisplayUnits))
            If digitsToLeftOfDecimalPoint < significantDigits Then
                Dim decimalsToDisplay As Integer = significantDigits - digitsToLeftOfDecimalPoint
                fmt += New String("#", decimalsToDisplay)
            End If

            formattedValue = valueInDisplayUnits.ToString(fmt)
        End If

        Return formattedValue
    End Function
End Module
