Imports System.IO
Imports System.Net
Imports System.Text

Public Class frmPrintZPLDialog

    Private _printDialogState As PrintDialogState
    Private _dtItemsToPrint As List(Of ItemsToPrintRow)
    Private _validOwners As List(Of DeviceOwner)
    Private cmndsel As String = Nothing

    Private AppConfig As New List(Of AppConfigState)

    Public Property dtItemsToPrint As List(Of ItemsToPrintRow)
        Get
            Return _dtItemsToPrint
        End Get
        Set(value As List(Of ItemsToPrintRow))
            _dtItemsToPrint = value
        End Set
    End Property

    Public Property PrintDialogState As PrintDialogState
        Get
            Return _printDialogState
        End Get
        Set(value As PrintDialogState)
            _printDialogState = value
        End Set
    End Property

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

    End Sub

    Public Sub New(cmdsel As String)
        ' This call is required by the designer.
        InitializeComponent()
        cmndsel = cmdsel
    End Sub

    Private Sub frmPrintZPLDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeDialogState()
        LoadPrinters()
        LoadBarcodeTemplates()
        LoadPrintData()

        'simplify ui if only a single item being printed
        If _dtItemsToPrint.Count = 1 Then
            btnPrintBarcode.Text = "Print"
            btnPreviewNextBarcode.Visible = False
            btnPreviewPrevBarcode.Visible = False
            cbBarcode.Enabled = False
        End If

        GetPreview()
    End Sub

    Private Sub InitializeDialogState()
        AppConfig = New AppConfigState() With {.Cmndsel = cmndsel}.GetAppConfigState()
        _validOwners = New DeviceOwner().Getall().Where(Function(owner) owner.EnableZebraPrinting = True).ToList()

        If (PrintDialogState Is Nothing) Then
            Dim printDialog = New PrintDialogState
            Dim printers = printDialog.Printers
            'set last printer
            For Each printer In printers
                Dim lastPrinter = IIf(Get_AppConfigSetting(AppConfig, "LastZebraPrinter") IsNot Nothing, Get_AppConfigSetting(AppConfig, "LastZebraPrinter"), "")
                printer.CurrentPrinter = (printer.Name = lastPrinter)
            Next

            'set last template and offsets
            For Each template In printDialog.BarcodeTemplates
                Dim lastBarcodeTemplateName = IIf(Get_AppConfigSetting(AppConfig, "LastZebraPrintTemplateName") IsNot Nothing, Get_AppConfigSetting(AppConfig, "LastZebraPrintTemplateName"), "")
                Dim lastBarcodeTemplateXOffset = IIf(Get_AppConfigSetting(AppConfig, "LastZebraPrintTemplateXOffset") IsNot Nothing, CInt(Get_AppConfigSetting(AppConfig, "LastZebraPrintTemplateXOffset")), 0)
                Dim lastBarcodeTemplateYOffset = IIf(Get_AppConfigSetting(AppConfig, "LastZebraPrintTemplateYOffset") IsNot Nothing, CInt(Get_AppConfigSetting(AppConfig, "LastZebraPrintTemplateYOffset")), 0)
                If template.Path = lastBarcodeTemplateName Then
                    template.CurrentTemplate = True
                    template.XOffset = lastBarcodeTemplateXOffset
                    template.YOffset = lastBarcodeTemplateYOffset
                Else
                    template.CurrentTemplate = False
                End If
            Next

            PrintDialogState = printDialog
        End If
    End Sub


    Private Sub frmPrintZPLDialog_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Escape Then Me.Close()
    End Sub

    Private Sub LoadPrinters()
        Dim bs = New BindingSource
        bs.DataSource = _printDialogState.Printers

        cbPrinters.DataSource = bs

        cbPrinters.DisplayMember = "Name"
        cbPrinters.ValueMember = "Name"

        For Each printer In _printDialogState.Printers
            If printer.CurrentPrinter Then
                cbPrinters.SelectedItem = printer
                printer.CurrentPrinter = False
            End If
        Next
    End Sub

    Private Sub LoadBarcodeTemplates()
        Dim bs As New BindingSource
        bs.DataSource = _printDialogState.BarcodeTemplates

        cbBarcodeType.DataSource = bs

        nupXCalibration.DataBindings.Add("Value", bs, "XOffset", False, DataSourceUpdateMode.OnPropertyChanged)
        nupYCalibration.DataBindings.Add("Value", bs, "YOffset", False, DataSourceUpdateMode.OnPropertyChanged)

        cbBarcodeType.DisplayMember = "Name"
        cbBarcodeType.ValueMember = "Path"

        For Each template In _printDialogState.BarcodeTemplates
            If template.CurrentTemplate Then
                cbBarcodeType.SelectedItem = template
                template.CurrentTemplate = False
            End If
        Next

    End Sub

    Private Sub LoadPrintData()
        If _dtItemsToPrint.Count = 0 Then
            MsgBox("No barcode data to print.", MsgBoxStyle.OkOnly)
            Me.Close()
        Else
            Dim bs As New BindingSource

            Dim owners = _validOwners.Select(Function(owner) owner.OwnerCode).ToArray()
            Dim listtoPrint = _dtItemsToPrint.Where(Function(item) String.IsNullOrEmpty(item.Owner) Or owners.Contains(item.Owner))

            If (listtoPrint.Count = 0) Then
                MsgBox("No barcode data to print with Zebra Printer. Please check the owner of the selected items", MsgBoxStyle.OkOnly)
                Me.Close()
                Exit Sub
            End If

            bs.DataSource = listtoPrint

            cbBarcode.DataSource = bs
            cbBarcode.DisplayMember = "Barcode"
            cbBarcode.ValueMember = "Barcode"
            lbSerialNumberValue.DataBindings.Add("Text", bs, "SerialNumber", False, DataSourceUpdateMode.OnPropertyChanged)

            'cbBarcode.SelectedIndex = 0
        End If
    End Sub

    Private Sub GetPreview()

        Dim selectedItemToPrint As ItemsToPrintRow = cbBarcode.SelectedItem
        Dim selectedBarcodeTemplate As BarcodeTemplate = cbBarcodeType.SelectedItem
        If (selectedItemToPrint Is Nothing) Then
            Exit Sub
        End If
        Dim ZPLData = selectedBarcodeTemplate.GetZPLData(selectedItemToPrint.Barcode, selectedItemToPrint.SerialNumber, "255.252.0.0", selectedItemToPrint.IPAddress, "\14", selectedItemToPrint.MacAddress)

        If ZPLData = "" Then
            Throw New ArgumentNullException("ZPLData", "This barcode type has no template file or data. Select another barcode type and contact IT for help." + vbCrLf + "Expected template file: " + selectedBarcodeTemplate.Path)
        End If

        Dim zpl() As Byte = Encoding.UTF8.GetBytes(ZPLData)

        ' adjust print density (8dpmm), label width (4 inches), label height (6 inches), and label index (0) as necessary
        Dim request As HttpWebRequest = WebRequest.Create("http://api.labelary.com/v1/printers/12dpmm/labels/1x.5/0/")
        request.Method = "POST"
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = zpl.Length

        Dim requestStream As Stream = request.GetRequestStream()
        requestStream.Write(zpl, 0, zpl.Length)
        requestStream.Close()

        Try
            Dim response As HttpWebResponse = request.GetResponse()
            Dim responseStream As Stream = response.GetResponseStream()
            Dim bm As New Bitmap(responseStream)
            picBarcodePreview.Image = bm
            responseStream.Close()
        Catch ex As WebException
            MsgBox("Failed to get preview." + vbCrLf + "Error: " + ex.Message, MsgBoxStyle.Critical)
        End Try

    End Sub

    Private Function Print() As Boolean
        Dim selectedBarcodeTemplate As BarcodeTemplate = cbBarcodeType.SelectedItem
        Dim selectedPrinter As ZPrinter = cbPrinters.SelectedItem
        Dim ZPLData As String
        Dim blPrintSuccess = False
        Cursor = Cursors.WaitCursor

        Try
            'print each item passed from parent form
            For Each row As ItemsToPrintRow In _dtItemsToPrint
                ZPLData = ""

                ZPLData = selectedBarcodeTemplate.GetZPLData(row.Barcode, row.SerialNumber)

                If ZPLData = "" Then
                    MsgBox("This barcode type has no template file or data. Select another barcode type and contact IT for help." + vbCrLf + "Expected template file: " + selectedBarcodeTemplate.Path, MsgBoxStyle.OkOnly)
                    Return False
                End If

                'repeat print * # Copies
                Dim copies = nupPrintCopies.Value
                For n As Integer = 1 To copies
#If DEBUG Then
                    MsgBox(ZPLData)
#Else
                    'do the print stuff
                    Try
                        Dim buffer = Encoding.ASCII.GetBytes(ZPLData)

                        Dim client = New Sockets.TcpClient(selectedPrinter.Address, selectedPrinter.Port)

                        Dim clientStream = client.GetStream()
                        clientStream.Write(buffer, 0, buffer.Length)
                        clientStream.Flush()
                        clientStream.Close()
                    Catch ex As Exception
                        MsgBox(ex.Message, MsgBoxStyle.OkOnly)
                    End Try
#End If


                    'this is a total guess on sleep time and if its even needed.
                    Threading.Thread.Sleep(1000)
                Next
            Next
        Catch ex As Exception
            MsgBox("Failed to print." + vbCrLf + "Error: " + ex.Message, MsgBoxStyle.Critical)
            Return False
        End Try

        Cursor = Cursors.Default

        selectedBarcodeTemplate.CurrentTemplate = True
        selectedPrinter.CurrentPrinter = True

        Return True

    End Function

    Private Sub btnPreviewBarcode_Click(sender As Object, e As EventArgs) Handles btnPreviewBarcode.Click
        GetPreview()
    End Sub



    Private Sub btnPrintBarcode_Click(sender As Object, e As EventArgs) Handles btnPrintBarcode.Click
        'set _dtItemsToPrint

        Dim blPrintSuccess = Print()

        'users requested the dialog stay open 
        'If Not blPrintSuccess Then
        'Exit Sub
        'Else
        'Me.Close()
        'End If

    End Sub

    Private Sub form_close(sender As Object, e As EventArgs) Handles Me.Closed
        If _printDialogState IsNot Nothing And cmndsel IsNot Nothing Then
            Dim SettingsToSave = New List(Of AppConfigState) From {
            New AppConfigState() With {.Cmndsel = cmndsel, .Tag = "LastZebraPrinter", .Rvalue = cbPrinters.SelectedValue},
            New AppConfigState() With {.Cmndsel = cmndsel, .Tag = "LastZebraPrintTemplateName", .Rvalue = cbBarcodeType.SelectedValue},
            New AppConfigState() With {.Cmndsel = cmndsel, .Tag = "LastZebraPrintTemplateXOffset", .Rvalue = nupXCalibration.Value.ToString},
            New AppConfigState() With {.Cmndsel = cmndsel, .Tag = "LastZebraPrintTemplateYOffset", .Rvalue = nupYCalibration.Value.ToString}
        }
            SettingsToSave.SaveConfig()
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim selectedBarcodeTemplate As BarcodeTemplate = cbBarcodeType.SelectedItem
        Dim selectedPrinter As ZPrinter = cbPrinters.SelectedItem

        selectedBarcodeTemplate.CurrentTemplate = True
        selectedPrinter.CurrentPrinter = True
        Me.Close()
    End Sub

    Private Sub btnXCalibrationUp_Click(sender As Object, e As EventArgs) Handles btnXCalibrationUp.Click
        nupXCalibration.Value += 1
        'Dim selectedBarcodeType As BarcodeType = cbBarcodeType.SelectedItem
        'selectedBarcodeType.XOffset += 1
    End Sub

    Private Sub btnXCalibrationDown_Click(sender As Object, e As EventArgs) Handles btnXCalibrationDown.Click
        nupXCalibration.Value += -1
        'Dim selectedBarcodeType As BarcodeType = cbBarcodeType.SelectedItem
        'selectedBarcodeType.XOffset += -1
    End Sub

    Private Sub btnPreviewNextBarcode_Click(sender As Object, e As EventArgs) Handles btnPreviewNextBarcode.Click
        Dim curInx = cbBarcode.SelectedIndex

        If curInx = cbBarcode.Items.Count - 1 Then
            cbBarcode.SelectedIndex = 0
        Else
            cbBarcode.SelectedIndex = curInx + 1
        End If

        GetPreview()
    End Sub

    Private Sub btnPreviewPrevBarcode_Click(sender As Object, e As EventArgs) Handles btnPreviewPrevBarcode.Click
        Dim curIndex = cbBarcode.SelectedIndex

        If curIndex = 0 Then
            cbBarcode.SelectedIndex = cbBarcode.Items.Count - 1
        Else
            cbBarcode.SelectedIndex = curIndex - 1
        End If

        GetPreview()
    End Sub

    Private Sub cbBarcodeType_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cbBarcodeType.SelectionChangeCommitted
        GetPreview()
    End Sub

    Private Sub cbBarcode_SelectionChangeCommitted(sender As Object, e As EventArgs) Handles cbBarcode.SelectionChangeCommitted
        GetPreview()
    End Sub

    Private Sub nupXCalibration_ValueChanged(sender As Object, e As EventArgs) Handles nupXCalibration.ValueChanged
        GetPreview()
    End Sub

    Private Sub nupYCalibration_ValueChanged(sender As Object, e As EventArgs) Handles nupYCalibration.ValueChanged
        GetPreview()
    End Sub

End Class

Public Class ItemsToPrintRow
    Private _serialNumber As String
    Public Property SerialNumber() As String
        Get
            Return _serialNumber
        End Get
        Set(ByVal value As String)
            _serialNumber = value
        End Set
    End Property

    Private _barcode As String
    Public Property Barcode() As String
        Get
            Return _barcode
        End Get
        Set(ByVal value As String)
            _barcode = value
        End Set
    End Property

    Private _owner As String
    Public Property Owner() As String
        Get
            Return _owner
        End Get
        Set(ByVal value As String)
            _owner = value
        End Set
    End Property

    Private _macAddress As String
    Public Property MacAddress() As String
        Get
            Return _macAddress
        End Get
        Set(ByVal value As String)
            _macAddress = value
        End Set
    End Property


    Private _ipAddress As String
    Public Property IPAddress() As String
        Get
            Return _ipAddress
        End Get
        Set(ByVal value As String)
            _ipAddress = value
        End Set
    End Property
    Public Sub New()

    End Sub
    Public Sub New(serialNo As String, barcode As String)
        Me.SerialNumber = serialNo
        Me.Barcode = barcode
    End Sub

    Public Sub New(serialNo As String, barcode As String, owner As String)
        Me.SerialNumber = serialNo
        Me.Barcode = barcode
        Me.Owner = owner
    End Sub
    Public Sub New(serialNo As String, barcode As String, owner As String, ipaddress As String, macAddress As String)
        Me.SerialNumber = serialNo
        Me.Barcode = barcode
        Me.Owner = owner
        Me.IPAddress = ipaddress
        Me.MacAddress = macAddress
    End Sub
End Class