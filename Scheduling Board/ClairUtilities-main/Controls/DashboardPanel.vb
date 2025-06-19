Imports System.Data.SqlClient

Public Class DashboardPanel

    Private _dtPrimary As New DataTable
    Private _dtSecondary As New DataTable

    Private _SQLQueryPrimary As String = ""
    Private _CaptionFieldPrimary As String
    Private _ResultFieldPrimary As String

    Private _SQLQuerySecondary As String = ""
    Private _CaptionFieldSecondary As String
    Private _ResultFieldSecondary As String

    <System.ComponentModel.Browsable(True)>
    Public Property SQLQueryPrimary() As String
        Get
            Return _SQLQueryPrimary
        End Get
        Set(value As String)
            _SQLQueryPrimary = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True)>
    Public Property CaptionFieldPrimary() As String
        Get
            Return _CaptionFieldPrimary
        End Get
        Set(value As String)
            _CaptionFieldPrimary = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True)>
    Public Property ResultFieldPrimary() As String
        Get
            Return _ResultFieldPrimary
        End Get
        Set(value As String)
            _ResultFieldPrimary = value
        End Set
    End Property


    <System.ComponentModel.Browsable(True)>
    Public Property SQLQuerySecondary() As String
        Get
            Return _SQLQuerySecondary
        End Get
        Set(value As String)
            _SQLQuerySecondary = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True)>
    Public Property CaptionFieldSecondary() As String
        Get
            Return _CaptionFieldSecondary
        End Get
        Set(value As String)
            _CaptionFieldSecondary = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True)>
    Public Property ResultFieldSecondary() As String
        Get
            Return _ResultFieldSecondary
        End Get
        Set(value As String)
            _ResultFieldSecondary = value
        End Set
    End Property


    <System.ComponentModel.Browsable(True)>
    Public Property _StartDate As Nullable(Of DateTime)
    <System.ComponentModel.Browsable(True)>
    Public Property _EndDate As Nullable(Of DateTime)

    Public Sub New(ByRef SQLQueryPrimary As String, ByRef CaptionFieldPrimary As String, ByRef ResultFieldPrimary As String,
                   Optional ByRef SQLQuerySecondary As String = "", Optional ByRef CaptionFieldSecondary As String = "", Optional ByRef ResultFieldSecondary As String = "",
                   Optional ByRef StartDate As DateTime = Nothing, Optional ByRef EndDate As DateTime = Nothing)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _SQLQueryPrimary = SQLQueryPrimary
        _CaptionFieldPrimary = CaptionFieldPrimary
        _ResultFieldPrimary = ResultFieldPrimary

        _SQLQuerySecondary = SQLQuerySecondary
        _CaptionFieldSecondary = CaptionFieldSecondary
        _ResultFieldSecondary = ResultFieldSecondary

        If Not StartDate = Nothing Then _StartDate = StartDate
        If Not EndDate = Nothing Then _EndDate = EndDate

    End Sub

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub RefreshData()

        lblResultPrimary.Text = "-"
        lblResultSecondary.Text = "-"

        _dtPrimary = Nothing
        _dtSecondary = Nothing

        Threading.Tasks.Task.Factory.StartNew(
            Sub()
                Try
                    Using newConn As New SqlConnection(FinesseConnectionString)
                        newConn.Open()
                        _dtPrimary = newConn.GetDataTable(_SQLQueryPrimary)
                        If _SQLQuerySecondary = _SQLQueryPrimary Then
                            _dtSecondary = _dtPrimary
                        Else
                            If Not String.IsNullOrEmpty(_SQLQuerySecondary) Then _dtSecondary = newConn.GetDataTable(_SQLQuerySecondary)
                        End If
                    End Using

                    Me.Invoke(Sub()
                                  If _CaptionFieldPrimary IsNot Nothing Then lblCaptionPrimary.Text = _dtPrimary.Rows(0).Item(_CaptionFieldPrimary)

                                  If _dtPrimary.Rows.Count > 0 Then
                                      lblResultPrimary.Text = _dtPrimary.Rows(0).Item(_ResultFieldPrimary)
                                      If IsNumeric(lblResultPrimary.Text) Then lblResultPrimary.Text = CDec(lblResultPrimary.Text).ToString("N2")
                                  End If

                                  If _dtSecondary IsNot Nothing AndAlso _dtSecondary.Rows.Count > 0 Then
                                      If _CaptionFieldSecondary IsNot Nothing Then lblCaptionSecondary.Text = _dtPrimary.Rows(0).Item(_CaptionFieldSecondary)
                                      lblResultSecondary.Text = _dtSecondary.Rows(0).Item(_ResultFieldSecondary)
                                      If IsNumeric(lblResultSecondary.Text) Then lblResultSecondary.Text = CDec(lblResultSecondary.Text).ToString("N2")
                                  End If
                              End Sub)
                Catch ex As Exception
                    'MsgBox(ex.Message)
                    Me.Invoke(Sub()
                                  lblResultPrimary.Text = "-"
                                  lblResultSecondary.Text = "-"
                              End Sub)
                End Try
            End Sub)

    End Sub

    Private Sub LblResultPrimary_Click(sender As Object, e As EventArgs) Handles lblResultPrimary.Click, lblResultSecondary.Click

        Dim lbl = TryCast(sender, Label)
        My.Computer.Clipboard.SetText(lbl.Text)

    End Sub

End Class
