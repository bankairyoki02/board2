Imports System.Data.SqlClient
Imports System.Text

Public Class frmPickVendor

    Public _vendno As String = Nothing
    Public _siteno As String = Nothing
    Public _vendorname As String = Nothing

    Private dtVendorAddresses As New DataTable
    Private dtKnownVendors As New DataTable

    Private _partno As String
    Private _KnownVendorsListVisible As String

    Private Function VendorSelected() As Boolean
        If vendno IsNot Nothing AndAlso siteno IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetVendor() As Boolean
        Return VendorSelected()
    End Function

    Public ReadOnly Property vendno As String
        Get
            Return _vendno
        End Get
    End Property

    Public ReadOnly Property siteno As String
        Get
            Return _siteno
        End Get
    End Property

    Public ReadOnly Property vendorname As String
        Get
            Return _vendorname
        End Get
    End Property

    Public Sub New(ByVal isContactMaintenanceVisible As Boolean, ByVal partno As String, Optional KnownVendorsListVisible As Boolean = True)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        btnContactMaintenance.Visible = isContactMaintenanceVisible

        _partno = partno
        _KnownVendorsListVisible = KnownVendorsListVisible

    End Sub

    Private Sub btnContactMaintenance_Click(sender As System.Object, e As System.EventArgs) Handles btnContactMaintenance.Click
        Dim commandArgs As String = ""
        If Not txtVendor.SelectedValue Is Nothing AndAlso Not String.IsNullOrEmpty(txtVendor.SelectedValue.ToString) Then commandArgs = ReplaceNull(txtVendor.SelectedValue, String.Empty)
        StartFinesseProcess("Contact Maintenance.exe", commandArgs)
    End Sub

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click

        If _KnownVendorsListVisible AndAlso dtKnownVendors.Select("vendno=" & _vendno.SQLQuote & " and site_no=" & _siteno.SQLQuote).Count = 0 Then
            Dim result = MsgBox("Add " & _vendorname & " to the Known Vendors list?", vbYesNoCancel)
            If result = MsgBoxResult.Yes Then

                Dim sSQL As New StringBuilder
                sSQL.AppendLine("	insert dbo.PartSubhireVendors	")
                sSQL.AppendLine("	        ( partno ,	")
                sSQL.AppendLine("	          vendno ,	")
                sSQL.AppendLine("	          siteno ,	")
                sSQL.AppendLine("	          notes ,	")
                sSQL.AppendLine("	          currency ,	")
                sSQL.AppendLine("	          Rate ,	")
                sSQL.AppendLine("	          RateType ,	")
                sSQL.AppendLine("	          DeliveryRate ,	")
                sSQL.AppendLine("	          ReturnRate	")
                sSQL.AppendLine("	        )	")
                sSQL.AppendLine("	values  ( " & _partno.SQLQuote & " , -- partno - varchar(50)	")
                sSQL.AppendLine("	          " & _vendno.SQLQuote & " , -- vendno - varchar(10)	")
                sSQL.AppendLine("	          " & _siteno.SQLQuote & " , -- siteno - varchar(10)	")
                sSQL.AppendLine("	          null , -- notes - varchar(250)	")
                sSQL.AppendLine("	          'USD' , -- currency - varchar(10)	")
                sSQL.AppendLine("	          0.00 , -- Rate - money	")
                sSQL.AppendLine("	          'D' , -- RateType - varchar(10)	")
                sSQL.AppendLine("	          0.00 , -- DeliveryRate - money	")
                sSQL.AppendLine("	          0.00  -- ReturnRate - money	")
                sSQL.AppendLine("	        )	")

                Dim newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()

                newConn.ExecuteNonQuery(sSQL)

                newConn.Close()
                newConn.Dispose()

            End If
        End If

        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub frmPickVendor_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Dim newConn As New SqlConnection(FinesseConnectionString)
        newConn.Open()

        txtVendor.Connection = newConn

        LoadKnownVendors()

        Me.Width = Me.Width - gbKnownVendors.Width

        txtVendor.Select()

    End Sub

    Public Sub LoadKnownVendors()
        If _KnownVendorsListVisible = Nothing Then Return

        If _KnownVendorsListVisible = True Then
            Dim sSQL As New StringBuilder
            sSQL.AppendLine("select v.vendno, s.site_no, v.vendor_name, p.currency, p.Rate, p.RateType")
            sSQL.AppendLine(", TwoDayWeek = case when p.RateType = 'D' then p.Rate * 2.00 else 0.00 end")
            sSQL.AppendLine("	, ThreeDayWeek = case when p.RateType = 'D' then p.rate * 3.00 else 0.00 end")
            sSQL.AppendLine(", p.DeliveryRate, p.ReturnRate")
            sSQL.AppendLine("	, s.city, s.state, s.country, s.phone, s.contact, email = s.usenet, mobile = s.voicemail")
            sSQL.AppendLine("from dbo.PartSubhireVendors p")
            sSQL.AppendLine("Join dbo.povendor v on v.vendno = p.vendno")
            sSQL.AppendLine("join dbo.povendsite s on p.vendno = s.vendno And p.siteno = s.site_no")
            sSQL.AppendLine("where p.partno = " & SQLQuote(_partno))

            Dim newconn As New SqlConnection(FinesseConnectionString)
            newconn.Open()

            dtKnownVendors = newconn.GetDataTable(sSQL)

            newconn.Close()
            newconn.Dispose()

            dgvKnownVendors.DataSource = dtKnownVendors
        Else
            gbKnownVendors.Visible = False
            gbVendorSearch.Location = gbKnownVendors.Location
        End If

    End Sub


    Private Sub txtVendor_SelectedValueChanged(sender As Object, e As EventArgs) Handles txtVendor.SelectedValueChanged
        Dim tryVendno As String = txtVendor.SelectedValue

        If String.IsNullOrEmpty(tryVendno) Then
            cboSiteAddress.DataSource = Nothing
        Else
            Dim newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()

            dtVendorAddresses = newConn.GetDataTable("exec dbo.pm2_get_vendor_addresses @vendno=" & tryVendno.SQLQuote & ", @hideInactive=" & If(chkHideInactive.Checked, 1, 0))

            newConn.Close()
            newConn.Dispose()

            cboSiteAddress.DisplayMember = "contact"
            cboSiteAddress.ValueMember = "site_no"
            cboSiteAddress.DataSource = dtVendorAddresses
        End If
    End Sub

    Private Sub cboSiteAddress_DrawItem(sender As Object, e As System.Windows.Forms.DrawItemEventArgs) Handles cboSiteAddress.DrawItem
        e.DrawBackground()

        Dim site_noToDraw = String.Empty
        Dim stringToDraw = "[error: this shouldn't happen!]"

        If e.Index = -1 Then
            If cboSiteAddress.SelectedValue Is Nothing OrElse cboSiteAddress.SelectedValue = "" Then
                stringToDraw = "(choose a vendor before picking an address)"
            ElseIf dtVendorAddresses.Rows.Count = 0 Then
                stringToDraw = "(no vendor addresses created)"
            Else
                stringToDraw = "(choose a vendor address)"
            End If
            GoTo BAIL
        End If

        Dim rv As DataRowView = cboSiteAddress.Items(e.Index)
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
            maybeAppend(.Item("vendor_name"))
            If Not String.IsNullOrEmpty(.Item("contact").Trim) Then
                maybeAppend("Attn: " & .Item("contact").Trim)
            End If
            maybeAppend(.Item("address_1"))
            maybeAppend(.Item("address_2"))
            maybeAppend(.Item("address_3"))
            maybeAppend((.Item("city") & " " & .Item("state") & " " & .Item("zipcode")))
        End With

        site_noToDraw = addressRow.Item("site_no")
        stringToDraw = address.ToString

BAIL:
        Dim subnoFont As New Font(e.Font.FontFamily, e.Font.Size * 1.2!, e.Font.Style Or FontStyle.Bold)

        Dim maxSubnoWidth = (
            From va As DataRow In dtVendorAddresses.Rows
            Select e.Graphics.MeasureString(va.Item("site_no") & ":", subnoFont).Width
            ).DefaultIfEmpty(0.0!).Max

        Using textBrush As New SolidBrush(e.ForeColor)
            If Not String.IsNullOrEmpty(site_noToDraw) Then
                e.Graphics.DrawString(site_noToDraw & ":", subnoFont, textBrush, e.Bounds.Location)
            End If

            Dim addressLocation = e.Bounds.Location
            addressLocation.Offset(maxSubnoWidth, 0)
            e.Graphics.DrawString(stringToDraw, e.Font, textBrush, addressLocation)
        End Using

        e.DrawFocusRectangle()

    End Sub

    Private Sub dgvKnownVendors_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvKnownVendors.CellClick, dgvKnownVendors.CellDoubleClick
        If dgvKnownVendors.SelectedRows.Count = 1 Then
            _vendno = dgvKnownVendors.SelectedRows(0).Cells("vendno").Value
            _siteno = dgvKnownVendors.SelectedRows(0).Cells("site_no").Value
            _vendorname = dgvKnownVendors.SelectedRows(0).Cells("vendor_name").Value

            Me.Close()
            Me.Dispose()
        End If
    End Sub

    Private Sub cboSiteAddress_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboSiteAddress.SelectedValueChanged
        _vendno = txtVendor.SelectedValue
        _siteno = cboSiteAddress.SelectedValue
        _vendorname = txtVendor.Text

        cmdOK.Enabled = cboSiteAddress.SelectedIndex >= 0
    End Sub

    Private Sub dgvKnownVendors_ColumnAdded(sender As Object, e As DataGridViewColumnEventArgs) Handles dgvKnownVendors.ColumnAdded
        Select Case e.Column.Name
            Case "vendno" : e.Column.HeaderText = "Vendor No"
            Case "site_no" : e.Column.HeaderText = "Site No"
            Case "vendor_name" : e.Column.HeaderText = "Vendor Name"
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
        _vendno = Nothing
        _siteno = Nothing
    End Sub

    Private Sub chkHideInactive_CheckedChanged(sender As Object, e As EventArgs) Handles chkHideInactive.CheckedChanged
        txtVendor.FilterClause = If(chkHideInactive.Checked, "and activeind = 'Y'", "")
        LoadKnownVendors()
    End Sub
End Class