Imports System.Data
Imports System.Windows.Forms

Public Class GenericPicker
    Public SelectedItem As DataRowView = Nothing

    Public ItemDescription As String = "Item"
    Public ItemList As DataView = Nothing
    Public GroupTitle As String = Nothing
    Public AllowSearching As Boolean = True
    Public Property SearchString() As String = ""

    Public Enum SearchPosition
        Start
        Within
    End Enum

    Private _DisplayColumns As New Dictionary(Of DataColumn, String)
    Private _SearchColumns As New Dictionary(Of DataColumn, SearchPosition)

    Private _PositionOverControl As Control
    Private _PositionOverToolStripItem As System.Windows.Forms.ToolStripItem
    Private _ParentForm As Form
    Private _Size As New Size(600, 600)

    Public ReadOnly Property ItemSelected() As Boolean
        Get
            Return SelectedItem IsNot Nothing
        End Get
    End Property

    Public Property PositionOverControl() As Control
        Get
            Return _PositionOverControl
        End Get
        Set(ByVal value As Control)
            _PositionOverControl = value
        End Set
    End Property

    Public ReadOnly Property PositionOverToolStripItem() As System.Windows.Forms.ToolStripItem
        Get
            Return _PositionOverToolStripItem
        End Get
    End Property

    Public Property Size() As Size
        Get
            Return _Size
        End Get
        Set(ByVal value As Size)
            _Size = value
        End Set
    End Property

    Public ReadOnly Property DisplayColumns() As Dictionary(Of DataColumn, String)
        Get
            Return _DisplayColumns
        End Get
    End Property

    Public ReadOnly Property SearchColumns() As Dictionary(Of DataColumn, SearchPosition)
        Get
            Return _SearchColumns
        End Get
    End Property

    Private Sub New()
        Debug.Assert(False)
    End Sub

    Public Sub New(ByVal sendingForm As Form, ByVal callingControl As Control, Optional ByVal itemList As DataView = Nothing, Optional ByVal groupTitle As String = Nothing)
        _PositionOverControl = callingControl
        _ParentForm = sendingForm
        Me.GroupTitle = groupTitle
        Me.ItemList = itemList
    End Sub

    Public Function GetItem() As Boolean
        If Me.ItemList Is Nothing Then
            Debug.Assert(False)
            Return False
        End If

        Cursor.Current = Cursors.WaitCursor
        Dim tmpForm As New frmGenericPicker(Me)
        tmpForm.ShowDialog(_ParentForm)
        GetItem = Me.ItemSelected
        Cursor.Current = Cursors.Default
    End Function

End Class
