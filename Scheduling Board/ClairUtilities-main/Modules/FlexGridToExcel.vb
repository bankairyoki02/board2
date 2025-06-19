Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel

Imports System.Runtime.InteropServices.Marshal

Module FlexGridToExcel

    Public Sub ExportFlexGridToExcel(TheFlexgrid As AxMSFlexGridLib.AxMSFlexGrid, Optional RowsToExclude() As Integer = Nothing, Optional ColsToExclude() As Integer = Nothing,
    Optional TheRows As Integer = 0, Optional TheCols As Integer = 0,
    Optional GridStyle As Integer = 1, Optional WorkSheetName As String = "Sheet1", Optional MergeRequired As Boolean = False)

        Dim objXL As New Excel.Application

        Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        Dim wbXL As Excel.Workbook = Nothing
        Dim wsXL As Excel.Worksheet = Nothing
        Dim intRow As Integer ' counter
        Dim intCol As Integer ' counter

        Dim MrgColStart As Integer
        Dim MrgColEnd As Integer
        Dim MrgRow As Integer
        Dim MrgData As String
        Dim i As Integer
        Dim MrgNow As String

        If objXL Is Nothing Then
            MsgBox("You need Microsoft Excel to use this function",
            vbExclamation, "Print to Excel")
            Exit Sub
        End If

        TheFlexgrid.Visible = False

        On Error GoTo BailOut

        TheRows = TheFlexgrid.Rows
        TheCols = TheFlexgrid.Cols

        wbXL = objXL.Workbooks.Add
        wsXL = objXL.ActiveSheet

        ' name the worksheet
        With wsXL
            If Not WorkSheetName = "" Then
                .Name = WorkSheetName
            End If
        End With

        objXL.Visible = True

        Dim xlRow As Integer = 1
        Dim xlCol As Integer = 1

        Dim flexgridcellcolor As String
        Dim flexgridforecolor As String

        If RowsToExclude Is Nothing Then RowsToExclude = {-1}
        If ColsToExclude Is Nothing Then ColsToExclude = {-1}

        ' fill worksheet
        For intRow = 0 To TheFlexgrid.Rows - 1
            If Array.IndexOf(RowsToExclude, intRow) = -1 Then
                For intCol = 0 To TheFlexgrid.Cols - 1
                    If Array.IndexOf(ColsToExclude, intCol) = -1 Then
                        Dim cell = wsXL.Cells(xlRow, xlCol)
                        Dim range = wsXL.Range(cell, cell)

                        With TheFlexgrid
                            .Row = intRow
                            .Col = intCol
                            wsXL.Cells(xlRow, xlCol).Value = .get_TextMatrix(intRow, intCol)
                            flexgridcellcolor = .CellBackColor.Name.ToString
                            flexgridforecolor = .CellForeColor.Name.ToString
                            Select Case flexgridcellcolor
                                Case Color.LightGoldenrodYellow.Name
                                    range.Interior.ColorIndex = 19
                                Case Color.WhiteSmoke.Name
                                    range.Interior.ColorIndex = 15
                                Case Color.LightSkyBlue.Name
                                    range.Interior.ColorIndex = 34
                                Case "ffffc0c0"
                                    range.Interior.ColorIndex = 22
                                Case Color.PeachPuff.Name
                                    range.Interior.ColorIndex = 40
                                Case Color.Khaki.Name
                                    range.Interior.ColorIndex = 36
                                Case Color.LightGreen.Name
                                    range.Interior.ColorIndex = 35
                                Case Color.DarkBlue.Name
                                    range.Interior.ColorIndex = 33
                                Case Color.AliceBlue.Name
                                    range.Interior.ColorIndex = 34
                                Case Color.Red.Name
                                    range.Interior.ColorIndex = 22
                                Case Color.Yellow.Name
                                    range.Interior.ColorIndex = 36
                                Case Color.Orange.Name
                                    range.Interior.ColorIndex = 45
                                Case Color.Tomato.Name
                                    range.Interior.ColorIndex = 22
                                Case Color.Black.Name
                                    'nothing
                                Case Else
                                    Dim othercolor = 0
                            End Select

                            Select Case flexgridforecolor
                                Case Color.Red.Name
                                    range.Cells.Font.ColorIndex = 3
                                Case Color.Tomato.Name
                                    range.Cells.Font.ColorIndex = 3
                            End Select

                        End With
                        xlCol += 1
                    End If
                Next
                xlRow += 1
                xlCol = 1
            End If
        Next

        TheFlexgrid.Visible = True

        If MergeRequired Then
            Dim mrgValue As String = String.Empty

            xlRow = 1
            xlCol = 1

            For intRow = 0 To TheFlexgrid.Rows - 1
                If Array.IndexOf(RowsToExclude, intRow) = -1 Then
                    For intCol = 0 To TheFlexgrid.Cols - 1
                        If Array.IndexOf(ColsToExclude, intCol) = -1 Then
                            If xlCol > 1 And xlRow > 1 Then
                                Dim currCell = wsXL.Cells(xlRow, xlCol)
                                Dim prevCell = wsXL.Cells(xlRow, xlCol - 1)
                                Dim currCellValue = currCell.value & String.Empty
                                If xlCol = 2 Then mrgValue = currCellValue
                                If currCellValue = mrgValue And mrgValue <> String.Empty And Not IsNumeric(currCellValue) Then
                                    currCell.value = String.Empty
                                    wsXL.Range(currCell, prevCell).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium)
                                    wsXL.Range(currCell, prevCell).Merge()
                                End If
                                mrgValue = currCellValue
                            End If
                            xlCol += 1
                        End If
                    Next
                    xlRow += 1
                    xlCol = 1
                End If
            Next
        End If

        ' format the look
        wsXL.Columns.AutoFit()
        wsXL.Rows.AutoFit()

        'wsXL.Range("a1", Right(wsXL.Columns(TheCols).AddressLocal, 1) & TheRows).AutoFormat(XlRangeAutoFormat.xlRangeAutoFormatTable1)  'GridStyle

        wsXL.PageSetup.LeftMargin = objXL.InchesToPoints(0.25)
        wsXL.PageSetup.TopMargin = objXL.InchesToPoints(0.25)

BailOut:

        ReleaseComObject(wsXL) : ReleaseComObject(wbXL)
        ReleaseComObject(wbXL) : ReleaseComObject(objXL)

        objXL = Nothing : wbXL = Nothing : wsXL = Nothing

        System.GC.Collect()

        TheFlexgrid.Visible = True

        System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

    End Sub


End Module
