Imports System.Environment
Imports System.IO
Imports System.Runtime.InteropServices.Marshal
Imports Microsoft.Office.Interop

Public Class ExcelExport

    Dim oExcel As New Excel.Application()
    Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
    Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
    Dim oCells As Excel.Range

    Public Shared Sub DataTableToExcel(ByVal table As System.Data.DataTable, Optional TableName As String = "")
        Cursor.Current = Cursors.WaitCursor
        Try
            Dim columnCount = table.Columns.Count
            Dim rowsCount = table.Rows.Count

            Dim dataForExcel(0 To rowsCount, 0 To columnCount - 1) As Object

            For ixColumn = 0 To columnCount - 1
                dataForExcel(0, ixColumn) = table.Columns(ixColumn).ColumnName
            Next

            For i As Integer = 1 To rowsCount - 1
                For j As Integer = 0 To columnCount - 1
                    dataForExcel(i, j) = table.Rows(i)(j)
                Next
            Next

            Dim oExcel As New Excel.Application()
            Dim oBooks As Excel.Workbooks = oExcel.Workbooks
            Dim oBook As Excel.Workbook
            Dim oSheet As Excel.Worksheet
            Dim oCells As Excel.Range

            oExcel.Visible = True

            Dim ci As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
            Try
                oBook = oExcel.Workbooks.Add
            Catch ex As Exception
                oBook = oBooks.GetType.InvokeMember("Add", Reflection.BindingFlags.InvokeMethod, Nothing, oBooks, Nothing, ci)
            End Try

            oSheet = oBook.Worksheets(1)

            oCells = oSheet.Range("A1:" & xlRC(rowsCount + 1, columnCount))
            oCells.Value2 = dataForExcel

            oCells.EntireColumn.ColumnWidth = 10
            oCells.EntireColumn.AutoFit()

            ReleaseComObject(oCells) : ReleaseComObject(oSheet)
            ReleaseComObject(oBook)
            ReleaseComObject(oExcel)

            oExcel = Nothing : oBook = Nothing
            oSheet = Nothing : oCells = Nothing

        Catch ex As Exception
            MsgBox("Unexpected error (" & TypeName(ex) & "): " & vbNewLine & ex.Message, MsgBoxStyle.Exclamation)
            Dim trace As New System.Diagnostics.StackTrace(ex, True)
            MessageBox.Show(trace.GetFrame(0).GetMethod().Name)
            MessageBox.Show("Line: " + trace.GetFrame(0).GetFileLineNumber())
            MessageBox.Show("Column: " + trace.GetFrame(0).GetFileColumnNumber())
        Finally
            GC.Collect()
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Public Shared Sub ListViewToExcel(ByVal lv As ListView, Optional TableName As String = "")

        Cursor.Current = Cursors.WaitCursor

        Try
            Dim columnCount = lv.Columns.Count

            Dim dataForExcel(0 To lv.Items.Count, 0 To columnCount - 1) As Object

            For ixColumn = 0 To columnCount - 1
                dataForExcel(0, ixColumn) = lv.Columns(ixColumn).Text
            Next

            For ixRow = 1 To lv.Items.Count
                Dim lvi = lv.Items(ixRow - 1)

                For ixColumn = 0 To columnCount - 1
                    dataForExcel(ixRow, ixColumn) = lvi.SubItems(ixColumn).Text
                Next
            Next

            Dim oExcel As New Excel.Application()
            Dim oBooks As Excel.Workbooks = oExcel.Workbooks
            Dim oBook As Excel.Workbook
            Dim oSheet As Excel.Worksheet
            Dim oCells As Excel.Range

            oExcel.Visible = True

            Dim ci As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
            Try
                oBook = oExcel.Workbooks.Add
            Catch ex As Exception
                oBook = oBooks.GetType.InvokeMember("Add", Reflection.BindingFlags.InvokeMethod, Nothing, oBooks, Nothing, ci)
            End Try

            oSheet = oBook.Worksheets(1)

            oCells = oSheet.Range("A1:" & xlRC(lv.Items.Count + 1, columnCount))
            oCells.Value2 = dataForExcel

            oCells.EntireColumn.ColumnWidth = 10
            oCells.EntireColumn.AutoFit()

            ReleaseComObject(oCells) : ReleaseComObject(oSheet)
            ReleaseComObject(oBook)
            ReleaseComObject(oExcel)

            oExcel = Nothing : oBook = Nothing
            oSheet = Nothing : oCells = Nothing

            System.GC.Collect()
        Catch ex As Exception
            MsgBox("Unexpected error (" & TypeName(ex) & "): " & vbNewLine & ex.Message, MsgBoxStyle.Exclamation)
            Dim trace As New System.Diagnostics.StackTrace(ex, True)
            MessageBox.Show(trace.GetFrame(0).GetMethod().Name)
            MessageBox.Show("Line: " + trace.GetFrame(0).GetFileLineNumber())
            MessageBox.Show("Column: " + trace.GetFrame(0).GetFileColumnNumber())
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Public Shared Sub DataGridViewToExcel(ByVal dgv As DataGridView, Optional TableName As String = "")

        Cursor.Current = Cursors.WaitCursor

        Try
            Dim visibleColumns = (From dgc As DataGridViewColumn In dgv.Columns
                                  Where dgc.Visible = True
                                  Select dgc)

            Dim columnCount = visibleColumns.Count

            Dim t As Data.DataTable = TryCast(dgv.DataSource, Data.DataTable)
            If t Is Nothing Then t = TryCast(TryCast(dgv.DataSource, BindingSource).DataSource, Data.DataTable)
            'If t Is Nothing Then t = TryCast(TryCast(dgv.DataSource, BindingSource).DataSource, DataSet).Tables(If(TableName = String.Empty, 0, TableName))
            If t Is Nothing Then t = TryCast(TryCast(dgv.DataSource, BindingSource).List, DataView).ToTable

            If t Is Nothing Then
                MsgBox("There is no data to export.", vbOKOnly)
                Return
            End If

            Dim dataForExcel(t.Rows.Count, columnCount) As Object

            Dim r As Integer = 0

            For Each dr As DataRow In t.Rows
                For c As Integer = 0 To columnCount - 1
                    Dim gridColumn As DataGridViewColumn = visibleColumns(c)
                    Dim itemForExcel As Object = dr.Item(gridColumn.DataPropertyName)
                    If TypeOf (itemForExcel) Is Double Then
                        If Double.IsNaN(itemForExcel) OrElse Double.IsInfinity(itemForExcel) Then
                            itemForExcel = DBNull.Value
                        End If
                    End If

                    dataForExcel(r, c) = itemForExcel
                Next
                r += 1
            Next

            Dim oExcel As New Excel.Application()
            Dim oBooks As Excel.Workbooks = oExcel.Workbooks
            Dim oBook As Excel.Workbook
            Dim oSheet As Excel.Worksheet
            Dim oCells As Excel.Range

            oExcel.Visible = True

            Dim ci As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
            Try
                oBook = oExcel.Workbooks.Add
            Catch ex As Exception
                oBook = oBooks.GetType.InvokeMember("Add", Reflection.BindingFlags.InvokeMethod, Nothing, oBooks, Nothing, ci)
            End Try

            oSheet = oBook.Worksheets(1)

            oCells = oSheet.Range("A1:" & xlRC(t.Rows.Count, columnCount))
            oCells.Value2 = dataForExcel


            Dim captions(columnCount - 1)

            For c As Integer = 0 To columnCount - 1
                Dim gridColumn As DataGridViewColumn = visibleColumns(c)
                If gridColumn Is Nothing Then
                    captions(c) = t.Columns(gridColumn.DataPropertyName).ColumnName
                Else
                    captions(c) = gridColumn.HeaderText
                End If
            Next

            oCells = oSheet.Rows(1)
            oCells.Insert()

            oCells = oSheet.Range("A1:" & xlRC(1, columnCount))
            oCells.Value2 = captions
            oCells.WrapText = True


            For c As Integer = 0 To columnCount - 1
                Dim gridColumn As DataGridViewColumn = visibleColumns(c)
                Dim columnFormat As String = gridColumn.DefaultCellStyle.Format

                If columnFormat = "" AndAlso t.Columns(gridColumn.DataPropertyName).DataType Is GetType(DateTime) Then
                    columnFormat = "dd-mmm-yyyy"
                End If

                If columnFormat <> "" Then
                    oCells.Range(xlWholeColumn(c + 1)).NumberFormat = columnFormat
                End If
            Next
            oCells.EntireColumn.ColumnWidth = 10
            oCells.EntireColumn.AutoFit()

            ReleaseComObject(oCells) : ReleaseComObject(oSheet)
            ReleaseComObject(oBook)
            ReleaseComObject(oExcel)

            oExcel = Nothing : oBook = Nothing
            oSheet = Nothing : oCells = Nothing

            System.GC.Collect()
        Catch ex As Exception
            MsgBox("Unexpected error (" & TypeName(ex) & "): " & vbNewLine & ex.Message, MsgBoxStyle.Exclamation)
            Dim trace As New System.Diagnostics.StackTrace(ex, True)
            MessageBox.Show(trace.GetFrame(0).GetMethod().Name)
            MessageBox.Show("Line: " + trace.GetFrame(0).GetFileLineNumber())
            MessageBox.Show("Column: " + trace.GetFrame(0).GetFileColumnNumber())
        Finally
            Cursor.Current = Cursors.Default
        End Try
    End Sub

    Public Sub DataSetToExcel(ByRef ds As DataSet, ByVal FilePath As String, ByVal SheetName As String, Optional ByVal invisibleColumns As HashSet(Of String) = Nothing)

        Dim usCI = New Globalization.CultureInfo("en-US")
        Dim localCI = System.Threading.Thread.CurrentThread.CurrentCulture

        oExcel.Visible = False
        Try
            oExcel.DisplayAlerts = False
        Catch ex As Exception
            System.Threading.Thread.CurrentThread.CurrentCulture = usCI
            oExcel.DisplayAlerts = False
        End Try

        'Start a new workbook
        oBooks = oExcel.Workbooks
        oBook = oBooks.Add()

        oSheets = oBook.Worksheets
        oSheet = oSheets.Item(1)
        oSheet.Name = SheetName
        oCells = oSheet.Cells

        'ProjectDataDump(ds.Tables(0))
        FastDataDump(ds, invisibleColumns) 'Fill in the data
        CType(oExcel.Columns("A:AY"), Excel.Range).EntireColumn.AutoFit()

        oExcel.Visible = True
        oBook.Activate()
        oBook.SaveAs(FilePath)

        'oBook.Close()

        'Quit Excel and thoroughly deallocate everything
        'oExcel.Quit()

        'Try to pull in Macros for people that use them.
        Try
            Dim appdatapath As String = GetFolderPath(SpecialFolder.ApplicationData)
            'MsgBox("This pop-up is only temporary for troubleshooting purposes." & vbCrLf & vbCrLf & Path.Combine(appdatapath, "Microsoft\Excel\XLSTART\personal.xlsb"), vbOKOnly)
            oExcel.Workbooks.Open(Path.Combine(appdatapath, "Microsoft\Excel\XLSTART\personal.xlsb"))
        Catch ex As Exception
            '6/9/2015 - This error started firing.  Maybe a Microsoft update caused it?  At this point we are not that worried about it.
            'At this point Jason and Kurt agreed that if a computer does not have the personal.xlsb macro file just ignore it.
            'MsgBox(ex.Message & vbCrLf & vbCrLf & "Opening Macros file failed.", vbOKOnly)
        End Try

        ReleaseComObject(oCells) : ReleaseComObject(oSheet)
        ReleaseComObject(oSheets) : ReleaseComObject(oBook)
        ReleaseComObject(oBooks) : ReleaseComObject(oExcel)

        oExcel = Nothing : oBooks = Nothing : oBook = Nothing
        oSheets = Nothing : oSheet = Nothing : oCells = Nothing

        If System.Threading.Thread.CurrentThread.CurrentCulture IsNot localCI Then
            System.Threading.Thread.CurrentThread.CurrentCulture = localCI
        End If

        System.GC.Collect()

    End Sub

    Private Sub FastDataDump(ByRef ds As DataSet, Optional ByVal invisibleColumns As HashSet(Of String) = Nothing)

        'the number of columns to indent the barcode rows
        Dim barcodeRecordIndent = 2

        Dim t = ds.Tables(0)

        Dim totalRows As Integer
        Dim sheetColumnCount As Integer

        totalRows = t.Rows.Count
        sheetColumnCount = t.Columns.Count

        Dim ColumnCount = t.Columns.Count
        Dim RowsToBold As New HashSet(Of Integer)

        'These integer variables will handle the column offset.
        'For example: col1, col2, col3, col4
        'If col2 is hidden then col3 and col4 must be offset by 1 column
        Dim partColumnOffset = 0

        ' Copy the DataTable to an object array
        Dim rawData(totalRows, sheetColumnCount - 1) As Object

        With t
            ' Copy the column names to the first row of the object array
            For col = 0 To ColumnCount - 1
                'bold the part header row
                RowsToBold.Add(1)
                If invisibleColumns IsNot Nothing AndAlso Not invisibleColumns.Contains(.Columns(col).ColumnName) Then
                    rawData(0, col - partColumnOffset) = .Columns(col).ColumnName
                Else
                    If invisibleColumns IsNot Nothing Then
                        partColumnOffset += 1
                    End If
                End If
            Next
            'reset the part column offset for the next row insert
            partColumnOffset = 0


            ' Copy the values to the object array
            Dim partRowIndex = 0
            For row = 0 To totalRows - 1

                For col = 0 To ColumnCount - 1
                    If invisibleColumns IsNot Nothing AndAlso Not invisibleColumns.Contains(.Columns(col).ColumnName) Then
                        'export the Phase and Part columns as strings
                        If .Columns(col).ColumnName = "Phase" OrElse .Columns(col).ColumnName = "Part" Then
                            rawData(row + 1, col - partColumnOffset) = "'" & .Rows(partRowIndex).ItemArray(col)
                        Else
                            rawData(row + 1, col - partColumnOffset) = .Rows(partRowIndex).ItemArray(col)
                        End If
                    Else
                        If invisibleColumns IsNot Nothing Then
                            partColumnOffset += 1
                        End If
                    End If
                    If InStr(ReplaceNull(rawData(row + 1, col - partColumnOffset), String.Empty), "TOTAL:", vbTextCompare) Then RowsToBold.Add(row + 1 + 1)
                Next

                'reset the part column offset for the next row insert
                partColumnOffset = 0

                partRowIndex += 1
            Next

        End With

        ' Calculate the final column letter
        Dim finalColLetter As String = String.Empty
        Dim colCharset As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim colCharsetLen As Integer = colCharset.Length

        If sheetColumnCount > colCharsetLen Then
            finalColLetter = colCharset.Substring( _
             sheetColumnCount - 1 \ colCharsetLen - 1, 1)
        End If

        finalColLetter += colCharset.Substring( _
          sheetColumnCount - 1 Mod colCharsetLen, 1)

        Dim excelRange As String
        ' Fast data export to Excel
        excelRange = String.Format("A1:{0}{1}", finalColLetter, t.Rows.Count + 1)
        oSheet.Range(excelRange, Type.Missing).Value2 = rawData
        oSheet.Range(excelRange).HorizontalAlignment = 2

        'Mark the header rows as BOLD
        For Each rowNum In RowsToBold
            CType(oSheet.Rows(rowNum, Type.Missing), Excel.Range).Font.Bold = True
        Next

    End Sub

    Public Function CreateNewWorkbook(ByVal FilePath As String) As Excel.Workbook

        Dim wb As Excel.Workbook
        Dim ex As New Excel.Application
        wb = ex.Workbooks.Add

        If (File.Exists(FilePath) = False) Then
            If Not Directory.Exists(FilePath.Substring(0, FilePath.LastIndexOf("\"))) Then

                BuildFolder(FilePath.Substring(0, FilePath.LastIndexOf("\")))

            End If


            wb.SaveAs(IIf(FilePath.EndsWith(".xls"), FilePath, FilePath + ".xls"))
            'ex.Visible = True
            'wb.Activate()

            CreateNewWorkbook = wb
        Else
            CreateNewWorkbook = Nothing
        End If

    End Function

    Protected Sub BuildFolder(ByVal DirectoryPath As String)

        Dim ParentFolder As String = DirectoryPath.Substring(0, DirectoryPath.LastIndexOf("\"))
        If Not Directory.Exists(ParentFolder) Then
            BuildFolder(ParentFolder)
        End If
        Directory.CreateDirectory(DirectoryPath)

    End Sub


End Class

