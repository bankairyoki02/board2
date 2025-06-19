Imports System.Data.SqlClient

Public Class ProjectPicker
    Public ProjectNum As String
    Public ProjectNumDesc As String
    Public ProjectSelected As Boolean = False

    Public DefaultSearch As String = ""
    Public ExtraSQLFilter As String = ""

    <Obsolete("This property has no effect anymore.")>
    Public ByDefaultExcludeInactiveProjects As Boolean = True

    <Obsolete("This property has no effect anymore.")>
    Public ByDefaultIncludeOnlyCurrentAndFutureProjects As Boolean = True

    Private _PositionOverControl As System.Windows.Forms.Control
    Private _PositionOverToolStripItem As System.Windows.Forms.ToolStripItem
    Private _ParentForm As System.Windows.Forms.Form
    Private _ConnectionString As String = String.Empty

    Public ReadOnly Property PositionOverControl() As System.Windows.Forms.Control
        Get
            Return _PositionOverControl
        End Get
    End Property

    Public ReadOnly Property PositionOverToolStripItem() As System.Windows.Forms.ToolStripItem
        Get
            Return _PositionOverToolStripItem
        End Get
    End Property

    Public ReadOnly Property ConnectionString() As String
        Get
            Return _ConnectionString
        End Get
    End Property

    Private Sub New()
        Debug.Assert(False)
    End Sub

    Private Sub New(sendingForm As System.Windows.Forms.Form, connectionString As String)
        _ParentForm = sendingForm

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

    Public Sub New(ByVal sendingForm As System.Windows.Forms.Form, ByVal callingControl As System.Windows.Forms.Control, Optional ByVal connectionString As String = "")
        Me.New(sendingForm, connectionString)
        _PositionOverControl = callingControl
    End Sub

    Public Sub New(ByVal sendingForm As System.Windows.Forms.Form, ByVal callingToolStripItem As System.Windows.Forms.ToolStripItem, Optional ByVal connectionString As String = "")
        Me.New(sendingForm, connectionString)
        _PositionOverToolStripItem = callingToolStripItem
    End Sub

    <Obsolete("Use New(Form, Control, Connection String) instead")>
    Public Sub New(ByVal sendingForm As System.Windows.Forms.Form, ByVal callingControl As System.Windows.Forms.Control, ByVal connection As SqlConnection)
        Me.New(sendingForm, callingControl, connection.ConnectionString)
    End Sub

    Public Function GetProject() As Boolean
        Dim tmpForm As New frmProjectPicker(Me)
        tmpForm.ShowDialog(_ParentForm)

        GetProject = ProjectSelected
    End Function

    <Obsolete("Use SQLJammer when possible to minimize the round trips to the database.")>
    Public Shared Function GetProjectDescription(ByVal ProjectNumber As String, ByVal connection As SqlConnection) As String

        Dim sqlStr As String = "select entitydesc from glentities where entityno = " & ProjectNumber.SQLQuote

        Try
            GetProjectDescription = ReplaceNull(connection.ExecuteScalar(sqlStr), "")

        Catch ex As Exception
            MsgBox(ex.ToString(), , "Unexpected error in Project Picker")
            GetProjectDescription = ""
        End Try

    End Function
End Class
