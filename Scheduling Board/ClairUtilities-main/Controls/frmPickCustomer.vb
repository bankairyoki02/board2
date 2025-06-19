Imports System.Data.SqlClient
Imports System.Text

Public Class FrmPickCustomer

    Public _custno As String = Nothing
    Public _subno As String = Nothing
    Public _customerName As String = Nothing

    Private dtCustomerAddresses As New DataTable
    Private dtKnownCustomers As New DataTable

    Private _partno As String
    Private _KnownCustomerListVisible As String

    Private Function CustomerSelected() As Boolean
        If CustNo IsNot Nothing AndAlso SubNo IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetCustomer() As Boolean
        Return CustomerSelected()
    End Function

    Public ReadOnly Property CustNo As String
        Get
            Return _custno
        End Get
    End Property

    Public ReadOnly Property SubNo As String
        Get
            Return _subno
        End Get
    End Property

    Public ReadOnly Property CustomerName As String
        Get
            Return _customerName
        End Get
    End Property

    Public Sub New(ByVal isContactMaintenanceVisible As Boolean, ByVal partno As String, Optional KnownCustomerListVisible As Boolean = True)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        btnContactMaintenance.Visible = isContactMaintenanceVisible

        _partno = partno
        _KnownCustomerListVisible = KnownCustomerListVisible

    End Sub

    Private Sub btnContactMaintenance_Click(sender As System.Object, e As System.EventArgs) Handles btnContactMaintenance.Click
        Dim commandArgs As String = ""
        If Not txtCustomer.SelectedValue Is Nothing AndAlso Not String.IsNullOrEmpty(txtCustomer.SelectedValue.ToString) Then commandArgs = ReplaceNull(txtCustomer.SelectedValue, String.Empty)
        StartFinesseProcess("Contact Maintenance.exe", commandArgs)
    End Sub

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click

        ' Known customer list is not yet used. Convert this for use with customer when needed

        'If _KnownCustomerListVisible AndAlso dtKnownCustomers.Select("custno=" & _custno.SQLQuote & " and subno=" & _subno.SQLQuote).Count = 0 Then
        '    Dim result = MsgBox("Add " & _customerName & " to the Known Vendors list?", vbYesNoCancel)
        '    If result = MsgBoxResult.Yes Then

        '        Dim sSQL As New StringBuilder
        '        sSQL.AppendLine("	insert dbo.PartSubhireVendors	")
        '        sSQL.AppendLine("	        ( partno ,	")
        '        sSQL.AppendLine("	          custno ,	")
        '        sSQL.AppendLine("	          SiteNo ,	")
        '        sSQL.AppendLine("	          notes ,	")
        '        sSQL.AppendLine("	          currency ,	")
        '        sSQL.AppendLine("	          Rate ,	")
        '        sSQL.AppendLine("	          RateType ,	")
        '        sSQL.AppendLine("	          DeliveryRate ,	")
        '        sSQL.AppendLine("	          ReturnRate	")
        '        sSQL.AppendLine("	        )	")
        '        sSQL.AppendLine("	values  ( " & _partno.SQLQuote & " , -- partno - varchar(50)	")
        '        sSQL.AppendLine("	          " & _custno.SQLQuote & " , -- custno - varchar(10)	")
        '        sSQL.AppendLine("	          " & _subno.SQLQuote & " , -- SiteNo - varchar(10)	")
        '        sSQL.AppendLine("	          null , -- notes - varchar(250)	")
        '        sSQL.AppendLine("	          'USD' , -- currency - varchar(10)	")
        '        sSQL.AppendLine("	          0.00 , -- Rate - money	")
        '        sSQL.AppendLine("	          'D' , -- RateType - varchar(10)	")
        '        sSQL.AppendLine("	          0.00 , -- DeliveryRate - money	")
        '        sSQL.AppendLine("	          0.00  -- ReturnRate - money	")
        '        sSQL.AppendLine("	        )	")

        '        Dim newConn As New SqlConnection(FinesseConnectionString)
        '        newConn.Open()

        '        newConn.ExecuteNonQuery(sSQL)

        '        newConn.Close()
        '        newConn.Dispose()

        '    End If
        'End If

        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub frmPickCustomer_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        txtCustomer.Connection = gConn

        LoadKnownCustomers()

        Me.Width = Me.Width - gbKnownCustomers.Width

        txtCustomer.Select()

    End Sub

    Public Sub LoadKnownCustomers()
        If _KnownCustomerListVisible = Nothing Then Return

        ' TODO - change this query to be for customers

        'If _KnownCustomerListVisible = True Then
        '    Dim sSQL As New StringBuilder
        '    sSQL.AppendLine("select v.custno, s.subno, v.vendor_name, p.currency, p.Rate, p.RateType")
        '    sSQL.AppendLine(", TwoDayWeek = case when p.RateType = 'D' then p.Rate * 2.00 else 0.00 end")
        '    sSQL.AppendLine("	, ThreeDayWeek = case when p.RateType = 'D' then p.rate * 3.00 else 0.00 end")
        '    sSQL.AppendLine(", p.DeliveryRate, p.ReturnRate")
        '    sSQL.AppendLine("	, s.city, s.state, s.country, s.phone, s.contact, email = s.usenet, mobile = s.voicemail")
        '    sSQL.AppendLine("from dbo.PartSubhireVendors p")
        '    sSQL.AppendLine("Join dbo.povendor v on v.custno = p.custno")
        '    sSQL.AppendLine("join dbo.povendsite s on p.custno = s.custno And p.SiteNo = s.subno")
        '    sSQL.AppendLine("where p.partno = " & SQLQuote(_partno))



        '    Dim newconn As New SqlConnection(FinesseConnectionString)
        '    newconn.Open()

        '    dtKnownCustomers = newconn.GetDataTable(sSQL)

        '    newconn.Close()
        '    newconn.Dispose()

        '    dgvKnownVendors.DataSource = dtKnownCustomers
        'Else
        '    gbKnownVendors.Visible = False
        '    gbVendorSearch.Location = gbKnownVendors.Location
        'End If

    End Sub


    Private Sub txt_SelectedValueChanged(sender As Object, e As EventArgs) Handles txtCustomer.SelectedValueChanged
        Dim trycustno As String = txtCustomer.SelectedValue

        If String.IsNullOrEmpty(trycustno) Then
            cboSubAddress.DataSource = Nothing
        Else
            Dim newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()

            dtCustomerAddresses = newConn.GetDataTable("exec dbo.pm2_get_customer_addresses @custno=" & trycustno.SQLQuote & ", @hideInactive=" & If(chkHideInactive.Checked, 1, 0))

            newConn.Close()
            newConn.Dispose()

            cboSubAddress.DisplayMember = "contact"
            cboSubAddress.ValueMember = "subno"
            cboSubAddress.DataSource = dtCustomerAddresses
        End If
    End Sub

    Private Sub cboSubAddress_DrawItem(sender As Object, e As System.Windows.Forms.DrawItemEventArgs) Handles cboSubAddress.DrawItem
        e.DrawBackground()

        Dim subnoToDraw = String.Empty
        Dim stringToDraw = "[error: this shouldn't happen!]"

        If e.Index = -1 Then
            If cboSubAddress.SelectedValue Is Nothing OrElse cboSubAddress.SelectedValue = "" Then
                stringToDraw = "(choose a customer before picking an address)"
            ElseIf dtCustomerAddresses.Rows.Count = 0 Then
                stringToDraw = "(no customer addresses created)"
            Else
                stringToDraw = "(choose a customer address)"
            End If
            GoTo BAIL
        End If

        Dim rv As DataRowView = cboSubAddress.Items(e.Index)
        If rv Is Nothing Then
            stringToDraw = "[error: no address?]"
            GoTo BAIL
        End If

        Dim addressRow As DataRow = rv.Row

        Dim address As New System.Text.StringBuilder
        Dim maybeAppend = Sub(addrLine As String)
                              If addrLine Is Nothing Then
                                  Return
                              End If

                              ' replace multiple spaces with a single space
                              Dim re As New System.Text.RegularExpressions.Regex(" +")
                              addrLine = re.Replace(addrLine, " ")

                              If Not String.IsNullOrEmpty(addrLine.Trim) Then
                                  address.AppendLine(addrLine.Trim)
                              End If
                          End Sub

        With addressRow
            maybeAppend(.Item("customer_name"))
            If Not String.IsNullOrEmpty(.Item("contact").Trim) Then
                maybeAppend("Attn: " & .Item("contact").Trim)
            End If
            maybeAppend(.Item("addr1"))
            maybeAppend(.Item("addr2"))
            maybeAppend(.Item("addr3"))
            maybeAppend((.Item("city") & " " & .Item("state") & " " & .Item("zipcode")))
        End With

        subnoToDraw = addressRow.Item("subno")
        stringToDraw = address.ToString

BAIL:
        Dim subnoFont As New Font(e.Font.FontFamily, e.Font.Size * 1.2!, e.Font.Style Or FontStyle.Bold)

        Dim maxSubnoWidth = (
            From va As DataRow In dtCustomerAddresses.Rows
            Select e.Graphics.MeasureString(va.Item("subno") & ":", subnoFont).Width
            ).DefaultIfEmpty(0.0!).Max

        Using textBrush As New SolidBrush(e.ForeColor)
            If Not String.IsNullOrEmpty(subnoToDraw) Then
                e.Graphics.DrawString(subnoToDraw & ":", subnoFont, textBrush, e.Bounds.Location)
            End If

            Dim addressLocation = e.Bounds.Location
            addressLocation.Offset(maxSubnoWidth, 0)
            e.Graphics.DrawString(stringToDraw, e.Font, textBrush, addressLocation)
        End Using

        e.DrawFocusRectangle()

    End Sub

    Private Sub DgvKnownCustomers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvKnownCustomers.CellClick, dgvKnownCustomers.CellDoubleClick
        If dgvKnownCustomers.SelectedRows.Count = 1 Then
            _custno = dgvKnownCustomers.SelectedRows(0).Cells("custno").Value
            _subno = dgvKnownCustomers.SelectedRows(0).Cells("subno").Value
            _customerName = dgvKnownCustomers.SelectedRows(0).Cells("customer_name").Value

            Me.Close()
            Me.Dispose()
        End If
    End Sub

    Private Sub CboSubAddress_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboSubAddress.SelectedValueChanged
        _custno = txtCustomer.SelectedValue
        _subno = cboSubAddress.SelectedValue
        _customerName = txtCustomer.Text

        cmdOK.Enabled = cboSubAddress.SelectedIndex >= 0
    End Sub

    Private Sub DgvKnownCustomers_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvKnownCustomers.ColumnAdded
        Select Case e.Column.Name
            Case "custno" : e.Column.HeaderText = "Customer No"
            Case "subno" : e.Column.HeaderText = "Sub No"
            Case "customer_name" : e.Column.HeaderText = "Customer Name"
            Case "currency" : e.Column.HeaderText = "Currency"
            Case "Rate"
                e.Column.HeaderText = "Rate"
                e.Column.DefaultCellStyle.Format = "N2"
                e.Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Case "RateType" : e.Column.HeaderText = "Rate Type"
            Case "TwoDayWeek" : e.Column.HeaderText = "2-Day Week"
                e.Column.DefaultCellStyle.Format = "N2"
                e.Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Case "ThreeDayWeek" : e.Column.HeaderText = "3-Day Week"
                e.Column.DefaultCellStyle.Format = "N2"
                e.Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Case "DeliveryRate" : e.Column.HeaderText = "Delivery Rate"
                e.Column.DefaultCellStyle.Format = "N2"
                e.Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Case "ReturnRate" : e.Column.HeaderText = "Return Rate"
                e.Column.DefaultCellStyle.Format = "N2"
                e.Column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Case "city" : e.Column.HeaderText = "City"
            Case "state" : e.Column.HeaderText = "State"
            Case "country" : e.Column.HeaderText = "Country"
            Case "phone" : e.Column.HeaderText = "Phone"
            Case "contact" : e.Column.HeaderText = "Contact"
            Case "email" : e.Column.HeaderText = "E-mail"
            Case "mobile" : e.Column.HeaderText = "Mobile"
        End Select
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        _custno = Nothing
        _subno = Nothing
    End Sub

    Private Sub chkHideInactive_CheckedChanged(sender As Object, e As EventArgs) Handles chkHideInactive.CheckedChanged
        txtCustomer.FilterClause = If(chkHideInactive.Checked, "and activeind = 'Y'", "")
        LoadKnownCustomers()
    End Sub
End Class