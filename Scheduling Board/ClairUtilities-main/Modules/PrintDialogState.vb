Imports System.Data.SqlClient
Imports System.IO
Public Class PrintDialogState

    Private _zPrinters As New List(Of ZPrinter)
    Private _Templates As New List(Of BarcodeTemplate)


    Public Property Printers As List(Of ZPrinter)
        Get
            Return _zPrinters
        End Get
        Set(value As List(Of ZPrinter))
            _zPrinters = value
        End Set
    End Property

    Public Property BarcodeTemplates As List(Of BarcodeTemplate)
        Get
            Return _Templates
        End Get
        Set(value As List(Of BarcodeTemplate))
            _Templates = value
        End Set
    End Property

    Public Sub New()
        GetPrinters()

        Dim csvDir As String = "C:\Finesse\BarcodePrinting\Zebra Templates" 'This is dumb, this should be a table
        Dim templateDir As String = "C:\Finesse\BarcodePrinting\EDS Apps\LABELS"
        Dim dtTemplateConfig = GetTemplateConfig(csvDir)
        GetBarcodeTemplates(templateDir, dtTemplateConfig)
    End Sub

    Private Function GetTemplateConfig(ByVal TemplateDir As String) As DataTable
        'This is gross, this most be modified to come from the template or the db
        Dim templateConfigData = File.ReadAllText(TemplateDir + "\TemplateConfig.csv").Split(vbCrLf)
        Dim templateConfig As New DataTable

        'first row is header
        For Each csvHeader In templateConfigData(0).Split(",")
            templateConfig.Columns.Add(csvHeader)
        Next

        'add data rows
        For i = 1 To templateConfigData.Count - 1
            Dim row = templateConfigData(i).Replace(vbLf, "")
            Dim values = row.Split(",")
            Dim newRow = templateConfig.NewRow()
            For j = 0 To values.Count - 1
                newRow.Item(j) = values(j)
            Next
            If Not String.IsNullOrEmpty(row) Then
                templateConfig.Rows.Add(newRow)
            End If
        Next

        Return templateConfig

    End Function

    Private Sub GetPrinters()
        Try
            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()

                Dim dtPrinters = newConn.GetDataTable("select zp.PrinterName, zp.IPAddress, zp.Port from dbo.ZebraPrinters zp")

                For Each row In dtPrinters.Rows
                    Dim printer = New ZPrinter With {
                    .Name = row.Item("PrinterName") + " " + row.Item("IPAddress"),
                    .Address = row.Item("IPAddress"),
                    .Port = row.Item("Port")
                        }
                    _zPrinters.Add(printer)
                Next
            End Using

        Catch ex As Exception
            Throw New Exception("Something bad happened when retrieving the printers: " + vbCrLf + ex.Message)
        End Try
    End Sub

    Private Sub GetBarcodeTemplates(ByVal TemplateDir As String, ByVal dtTemplateConfig As DataTable)
        ' Try
        Dim templatesPaths = Directory.GetFiles(TemplateDir, "*.zpl")

        For Each filePath As String In templatesPaths
            Dim name = Path.GetFileNameWithoutExtension(filePath)
            Dim row = dtTemplateConfig.Select($"Name='{name}'")

            Dim config As New Dictionary(Of String, String)
            If row.Length <> 0 Then
                For Each col As DataColumn In dtTemplateConfig.Columns
                    config.Add(col.ColumnName, row(0).Item(col.Ordinal).ToString)
                Next
            End If

            Dim barcodeType = New BarcodeTemplate With {
                        .Name = name,
                        .Path = filePath,
                        .XOffset = 0,
                        .YOffset = 0,
                        .CurrentTemplate = False,
                        .Config = config
                            }
            _Templates.Add(barcodeType)
        Next
        'Catch ex As Exception
        'Throw New Exception("Something bad happened when retrieving the barcode templates: " + vbCrLf + ex.Message)
        'End Try
    End Sub

End Class
