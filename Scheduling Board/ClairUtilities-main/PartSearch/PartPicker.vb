Imports System.Data.SqlClient

Public Class PartPicker

    Public PartNumber As String = ""
    Public PartDesc As String = ""
    Public PartCategory As String = ""
    Public PartSelected As Boolean = False
    Public SearchForBarcode As Boolean = True
    Public UseGroups As Boolean = True

    Public DefaultSearch As String = ""
    Public ExtraSQLFilter As String = ""

    Private _PositionOverControl As System.Windows.Forms.Control
    Private _PositionOverToolStripItem As ToolStripItem
    Private myParentForm As System.Windows.Forms.Form
    Private _ConnectionString As String = String.Empty

    Public ReadOnly Property ConnectionString() As String
        Get
            Return _ConnectionString
        End Get
    End Property

    Public Property PositionOverControl() As System.Windows.Forms.Control
        Get
            Return _PositionOverControl
        End Get
        Set(ByVal value As System.Windows.Forms.Control)
            _PositionOverControl = value
            If _PositionOverControl IsNot Nothing Then
                _PositionOverToolStripItem = Nothing
            End If
        End Set
    End Property

    Public Property PositionOverToolStripItem() As System.Windows.Forms.ToolStripItem
        Get
            Return _PositionOverToolStripItem
        End Get
        Set(ByVal value As System.Windows.Forms.ToolStripItem)
            _PositionOverToolStripItem = value
            If value IsNot Nothing Then
                _PositionOverControl = Nothing
            End If
        End Set
    End Property


    Private Sub New()
        Debug.Assert(False)
    End Sub

    Private Sub New(sendingForm As System.Windows.Forms.Form, Optional connectionString As String = "")
        myParentForm = sendingForm

        If Not String.IsNullOrEmpty(connectionString) Then
            Dim sb As New SqlConnectionStringBuilder(connectionString)
            If sb.AsynchronousProcessing AndAlso sb.PersistSecurityInfo Then
                _ConnectionString = connectionString
            Else
                Debug.Assert(False)
            End If
        End If

        If String.IsNullOrEmpty(_ConnectionString) Then
            _ConnectionString = FinesseConnectionString
        End If
    End Sub

    Public Sub New(sendingForm As System.Windows.Forms.Form, callingControl As System.Windows.Forms.Control, Optional connectionString As String = "")
        Me.New(sendingForm, connectionString)
        _PositionOverControl = callingControl
    End Sub

    Sub New(sendingForm As System.Windows.Forms.Form, callingToolStripItem As ToolStripItem, Optional connectionString As String = "")
        Me.New(sendingForm, connectionString)
        _PositionOverToolStripItem = callingToolStripItem
    End Sub

    <Obsolete("Use New(Form, Control, Connection String) instead")>
    Public Sub New(ByVal sendingForm As System.Windows.Forms.Form, ByVal callingControl As System.Windows.Forms.Control, ByVal connection As SqlConnection)
        Me.New(sendingForm, callingControl, connection.ConnectionString)
    End Sub

    Public Function GetPart() As Boolean
        Dim tmpForm As New frmPartPicker(Me)
        tmpForm.ShowDialog(myParentForm)
        GetPart = PartSelected
    End Function

    Public Shared Function GetPartCategory(ByVal PartNumber As String, ByVal connection As SqlConnection) As String

        Dim sqlStr As String = "select commmodity from inpart where partno = " + PartNumber.SQLQuote

        Try
            GetPartCategory = ReplaceNull(connection.ExecuteScalar(sqlStr), "")

        Catch ex As Exception
            MsgBox(ex.ToString(), , "Unexpected error in Part Picker")
            GetPartCategory = ""
        End Try

    End Function

    <Obsolete()>
    Public Shared Function GetPartDescription(ByVal PartNumber As String, ByVal connection As SqlConnection) As String

        Dim sqlStr As String = "select partdesc from inpart where partno = " + PartNumber.SQLQuote

        Try
            GetPartDescription = ReplaceNull(connection.ExecuteScalar(sqlStr), "")

        Catch ex As Exception
            MsgBox(ex.ToString(), , "Unexpected error in Part Picker")
            GetPartDescription = ""
        End Try

    End Function


    <Obsolete()>
    Public Shared Function PartExists(ByVal PartNumber As String, ByVal connection As SqlConnection) As Boolean

        Dim sqlStr As String = "select count(*) from inpart where partno = " + PartNumber.SQLQuote

        Try
            PartExists = CInt(connection.ExecuteScalar(sqlStr)) > 0

        Catch ex As Exception
            MsgBox(ex.ToString(), , "Unexpected error in Part Picker")
            PartExists = False
        End Try

    End Function

    <Obsolete()>
    Public Shared Sub GetPartFromPartPicker(ByVal sendingForm As System.Windows.Forms.Form, ByVal callingControl As System.Windows.Forms.Control, ByRef updateTextBox As System.Windows.Forms.TextBox, ByVal connection As SqlConnection)
        Dim picker As New PartPicker(sendingForm, callingControl)

        If Not picker.GetPart() Then
            Exit Sub
        End If

        updateTextBox.Text = picker.PartNumber
    End Sub
End Class
