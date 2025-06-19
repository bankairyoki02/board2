Imports System.Text
Imports System.Data.SqlClient
Public Class ContactPicker

    Private _flpInitialHeight As Integer

    Private _SqlConnection As SqlConnection
    Private _AddressType As AddressType
    Private _AddressValue As String = Nothing
    Private _AddressName As String = Nothing
    Private _SourceData As String = Nothing

    Private _id_ContactCategory As Integer
    Private _entityno As String

    Private Function AddressSelected() As Boolean
        If AddressValue IsNot Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetAddress() As Boolean
        Return AddressSelected()
    End Function

    Public ReadOnly Property AddressValue As String
        Get
            Return _AddressValue
        End Get
    End Property

    Public ReadOnly Property AddressName As String
        Get
            Return _AddressName
        End Get
    End Property

    Public ReadOnly Property SourceData As String
        Get
            Return _SourceData
        End Get
    End Property

    Public Enum AddressType
        Contact
        Location
    End Enum

    Public Sub New(ByVal SqlConnection As SqlConnection, ByVal AddressType As AddressType, ByVal isContactMaintenanceVisible As Boolean, ByVal id_ContactCategory As Integer, ByVal entityno As String)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _flpInitialHeight = flpAddNewDetails.Height
        Me.Height = 155
        btnContactMaintenance.Visible = isContactMaintenanceVisible
        _AddressType = AddressType

        txtSearch.Connection = SqlConnection

        Select Case _AddressType
            Case AddressType.Contact
                Me.Text = Replace(Me.Text, "_AddressType", "Contact")
                cmdAddNew.Text = Replace(cmdAddNew.Text, "_AddressType", "Contact")
                lblSearch.Text = Replace(lblSearch.Text, "_AddressType", "Contact")
                lblName.Text = Replace(lblName.Text, "_AddressType", "Contact")

                txtSearch.TableName = "contacts"
                txtSearch.ValueMember = "contactno"
                txtSearch.DisplayMember = "contactname"

                _id_ContactCategory = id_ContactCategory

            Case AddressType.Location
                Me.Text = Replace(Me.Text, "_AddressType", "Address")
                cmdAddNew.Text = Replace(cmdAddNew.Text, "_AddressType", "Address")
                lblSearch.Text = Replace(lblSearch.Text, "_AddressType", "Address")
                lblName.Text = Replace(lblName.Text, "_AddressType", "Address")
                lblPhone.Text = Replace(lblPhone.Text, "Cell ", String.Empty)

                txtSearch.TableName = "Venues_And_Shipping_Destinations"
                txtSearch.ValueMember = "ValueMember"
                txtSearch.DisplayMember = "DisplayMember"

                _entityno = entityno
                btnContactMaintenance.Visible = False

        End Select

    End Sub

    Private Sub btnContactMaintenance_Click(sender As System.Object, e As System.EventArgs) Handles btnContactMaintenance.Click
        Dim commandArgs As String = ""
        If Not txtSearch.SelectedValue Is Nothing AndAlso Not String.IsNullOrEmpty(txtSearch.SelectedValue.ToString) Then commandArgs = ReplaceNull(txtSearch.SelectedValue, String.Empty)
        StartFinesseProcess("Contact Maintenance.exe", commandArgs)
    End Sub

    Private Sub cmdOK_Click(sender As System.Object, e As System.EventArgs) Handles cmdOK.Click

        Select Case flpAddNewDetails.Visible
            Case True

                Dim lName = IIf(_AddressType = AddressType.Contact, 30, 100)

                If txtName.Text.Length > lName Then
                    MsgBox("Name field can only be " & lName.ToString & " characters long.  Data will be truncated.", vbOKOnly)
                End If

                txtName.Text = Mid(txtName.Text, 1, lName)

                Using newConn As New SqlConnection(FinesseConnectionString)
                    newConn.Open()

                    Try

                        Select Case _AddressType
                            Case AddressType.Contact
                                Dim cmd As New SqlCommand("Customer_QuickAdd", New SqlConnection(FinesseConnectionString))
                                cmd.Connection.Open()
                                cmd.CommandType = CommandType.StoredProcedure
                                cmd.Parameters.AddWithValue("@id_ContactCategory", _id_ContactCategory)
                                cmd.Parameters.AddWithValue("@name", txtName.Text)
                                cmd.Parameters.AddWithValue("@phone", txtPhone.Text)
                                cmd.Parameters.AddWithValue("@email", txtEmail.Text)
                                cmd.Parameters.AddWithValue("@Addr1", txtAddr1.Text)
                                cmd.Parameters.AddWithValue("@Addr2", txtAddr2.Text)
                                cmd.Parameters.AddWithValue("@City", txtCity.Text)
                                cmd.Parameters.AddWithValue("@State", txtState.Text)
                                cmd.Parameters.AddWithValue("@Zip", txtZip.Text)
                                cmd.Parameters.AddWithValue("@CountryCode", txtCountry.Text)
                                cmd.Parameters.AddWithValue("@Province", txtProvince.Text)

                                cmd.Parameters.Add("@custno", SqlDbType.VarChar, 10)
                                cmd.Parameters("@custno").Direction = ParameterDirection.Output

                                cmd.ExecuteNonQuery()

                                _AddressValue = cmd.Parameters("@custno").Value.ToString
                                _AddressName = txtName.Text

                                txtSearch.SelectedValue = _AddressValue
                            Case AddressType.Location
                                Dim cmd As New SqlCommand("ShippingLocation_QuickAdd", New SqlConnection(FinesseConnectionString))
                                cmd.Connection.Open()
                                cmd.CommandType = CommandType.StoredProcedure
                                cmd.Parameters.AddWithValue("@entityno", _entityno)
                                cmd.Parameters.AddWithValue("@name", txtName.Text)
                                cmd.Parameters.AddWithValue("@phone", txtPhone.Text)
                                cmd.Parameters.AddWithValue("@email", txtEmail.Text)
                                cmd.Parameters.AddWithValue("@Addr1", txtAddr1.Text)
                                cmd.Parameters.AddWithValue("@Addr2", txtAddr2.Text)
                                cmd.Parameters.AddWithValue("@City", txtCity.Text)
                                cmd.Parameters.AddWithValue("@State", txtState.Text)
                                cmd.Parameters.AddWithValue("@Zip", txtZip.Text)
                                cmd.Parameters.AddWithValue("@CountryCode", txtCountry.Text)
                                cmd.Parameters.AddWithValue("@Province", txtProvince.Text)

                                cmd.Parameters.Add("@DestinationDetail", SqlDbType.VarChar, 150)
                                cmd.Parameters("@DestinationDetail").Direction = ParameterDirection.Output

                                cmd.ExecuteNonQuery()

                                _AddressValue = cmd.Parameters("@DestinationDetail").Value.ToString
                                _AddressName = txtName.Text
                                _SourceData = "S"

                                txtSearch.SelectedValue = _AddressValue

                        End Select

                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try

                End Using

            Case False
                'Nothing... value has been set by txtSearch.SelectedValue.  Just need to get _SourceData to send back to frmProjectMaintenance

                If _AddressType = AddressType.Location Then
                    Using newConn As New SqlConnection(FinesseConnectionString)
                        newConn.Open()

                        Dim sSQL As New Text.StringBuilder
                        sSQL.AppendLine("select v.SourceData from Venues_And_Shipping_Destinations v where v.ValueMember = " & _AddressValue.SQLQuote)

                        _SourceData = newConn.GetDataTable(sSQL).Rows(0).Item("SourceData")
                    End Using
                End If
        End Select

        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub frmPickContact_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        flpAddNewDetails.Visible = False

        'Using newConn As New SqlConnection(FinesseConnectionString)
        '    txtSearch.Connection = newConn
        '    newConn.Open()
        'End Using

        txtSearch.Select()
    End Sub

    Private Sub txtSearch_SelectedValueChanged(sender As Object, e As EventArgs) Handles txtSearch.SelectedValueChanged
        _AddressValue = txtSearch.SelectedValue
        _AddressName = txtSearch.Text

        cmdOK.Enabled = txtSearch.SelectedValue IsNot Nothing
    End Sub

    Private Sub txtName_TextChanged(sender As Object, e As EventArgs) Handles txtName.TextChanged
        cmdOK.Enabled = Not String.IsNullOrEmpty(txtName.Text)
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        _AddressValue = Nothing
        _AddressName = String.Empty
    End Sub

    Private Sub cmdAddNewContact_Click(sender As Object, e As EventArgs) Handles cmdAddNew.Click
        If Not flpAddNewDetails.Visible = True Then
            Me.Height = Me.Height + _flpInitialHeight
        End If
        flpAddNewDetails.Visible = True
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        cmdAddNew.Enabled = String.IsNullOrEmpty(txtSearch.SelectedValue)
    End Sub

End Class