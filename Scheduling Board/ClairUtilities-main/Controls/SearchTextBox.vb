Imports System.Text
Imports System.Data.SqlClient
Imports System.Windows.Forms
Imports System.Drawing

<System.ComponentModel.DefaultBindingProperty("SelectedValue")> _
Public Class SearchTextBox
    Inherits HighlightTextBox
    'Implements System.ComponentModel.INotifyPropertyChanged

    Private _selectedValue As Object
    Private _selectedItem As DataRowView

    Private _connection As SqlConnection
    Private _dataTable As New DataTable

    Private _displayMember As String
    Private _valueMember As String
    Private _tableName As String
    Private _filterClause As String
    Private _orderBy As String
    Private _resultsRowLimit As Integer

    Private _valueSearchRegion As SearchRegion
    Private _displaySearchRegion As SearchRegion = SearchRegion.Contains

    Private _fontSearching As Font = MyBase.Font
    Private _fontNothingFound As Font = MyBase.Font
    Private _fontHasValue As Font = MyBase.Font

    Private _ctrl As New MyListBoxHost
    Public WithEvents _list As ListBox = _ctrl.ListBox
    Private _controlHost As New ToolStripControlHost(_ctrl) With {.AutoSize = False, .Padding = New Padding(0), .Margin = New Padding(0)}
    Private WithEvents _popupControl As MyPopup

    Private WithEvents _parentForm As Form

    Private WithEvents tmrDelayedSearch As New Timer With {.Interval = 250}

    Public Event SelectedValueChanged As EventHandler
    'Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Public Enum SearchRegion
        None
        StartsWith
        EndsWith
        Contains
    End Enum

    Public Sub HideSearchResultsListBox()
        _popupControl.Hide()
    End Sub

    Public Sub New()
        _popupControl = New MyPopup(Me) With {.AutoSize = False, .Padding = New Padding(0), .Margin = New Padding(0), .DropShadowEnabled = False}
        _popupControl.Items.Add(_controlHost)

        InitInternalFonts(MyBase.Font)
    End Sub

    Private Function IsNullValue(ByVal value As Object) As Boolean
        Return value Is Nothing OrElse value.ToString = "" OrElse IsDBNull(value)
    End Function

    Private Function DatabaseFieldsOK() As Boolean
        ' bail if 
        If String.IsNullOrEmpty(_tableName) OrElse
            String.IsNullOrEmpty(_valueMember) OrElse
            _connection Is Nothing Then
            'Throw New Exception("The connection, table name, or value member are not specified.")
            Return False
        End If

        If (String.IsNullOrEmpty(_displayMember) OrElse _displaySearchRegion = SearchRegion.None) Then
            If (String.IsNullOrEmpty(_valueMember) OrElse _valueSearchRegion = SearchRegion.None) Then
                'Throw New Exception("The connection, table name, or value member are not specified.")
                Return False
            End If
        End If

        Return True
    End Function


    Private Sub EnsureSelectedValueIsInDataTable()
        _selectedItem = Nothing

        ' bail if there cannot possibly be a value in the datatable
        If IsNullValue(_selectedValue) Then
            If Not Me.Focused Then
                MyBase.Text = ""
            End If
            Return
        End If

        If SelectDataItemForSelectedValue() Then
            Return
        End If

        If Not DatabaseFieldsOK Then
            Return
        End If

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("select " & GetSelectFields())
        sSQL.AppendLine("from " & _tableName)
        sSQL.AppendLine("where " & _valueMember & " = " & _selectedValue.ToString.SQLQuote)
        If Not String.IsNullOrEmpty(_filterClause) Then
            sSQL.AppendLine(_filterClause)
        End If

        Using a As New SqlDataAdapter(sSQL.ToString, _connection)
            a.Fill(_dataTable)
            If Not SelectDataItemForSelectedValue() Then
                Throw New Exception(String.Format("[{0}] not found in [{1}].[{2}]",
                                                  _selectedValue, _tableName, _valueMember))
            End If
        End Using
    End Sub

#Region "properties"
    <System.ComponentModel.Bindable(True)> _
    Public Property SelectedValue() As Object
        Get
            Return _selectedValue
        End Get
        Set(ByVal value)
            Dim valuesAreEquivalent =
                IsNullValue(_selectedValue) AndAlso IsNullValue(value) OrElse
                Not IsNullValue(value) AndAlso value.Equals(_selectedValue)

            _selectedValue = value

            If Not valuesAreEquivalent Then
                EnsureSelectedValueIsInDataTable()
                RefreshTextBoxFont()
            End If

            RaiseEvent SelectedValueChanged(Me, New EventArgs())
        End Set
    End Property

    Public ReadOnly Property DataTable() As DataTable
        Get
            Return _dataTable
        End Get
    End Property

    <System.ComponentModel.Browsable(True)> _
    Public Property Connection() As SqlConnection
        Get
            Return _connection
        End Get
        Set(ByVal value As SqlConnection)
            _connection = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True)>
    Public Property TableName() As String
        Get
            Return _tableName
        End Get
        Set(ByVal value As String)
            _tableName = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True)>
    Public Property FilterClause() As String
        Get
            Return _filterClause
        End Get
        Set(ByVal value As String)
            _filterClause = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True)>
    Public Property OrderBy() As String
        Get
            Return _orderBy
        End Get
        Set(ByVal value As String)
            _orderBy = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True)> _
    Public Property ValueMember() As String
        Get
            Return _valueMember
        End Get
        Set(ByVal value As String)
            RemoveDataTableColumn(_valueMember)
            AddDataTableColumn(value)
            _valueMember = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True)> _
    <System.ComponentModel.DefaultValue(GetType(SearchRegion), "None")> _
    Public Property ValueSearchRegion() As SearchRegion
        Get
            Return _valueSearchRegion
        End Get
        Set(ByVal value As SearchRegion)
            _valueSearchRegion = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True)> _
    Public Property DisplayMember() As String
        Get
            Return _displayMember
        End Get
        Set(ByVal value As String)
            RemoveDataTableColumn(_displayMember)
            AddDataTableColumn(value)
            _displayMember = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True)> _
    <System.ComponentModel.DefaultValue(GetType(SearchRegion), "Contains")> _
    Public Property DisplaySearchRegion() As SearchRegion
        Get
            Return _displaySearchRegion
        End Get
        Set(ByVal value As SearchRegion)

            _displaySearchRegion = value
        End Set
    End Property

    <System.ComponentModel.Browsable(True)> _
    <System.ComponentModel.DefaultValue(100)> _
    Public Property ResultsRowLimit() As Integer
        Get
            Return _resultsRowLimit
        End Get
        Set(ByVal value As Integer)
            _resultsRowLimit = value
        End Set
    End Property

    <System.ComponentModel.Bindable(True)> _
    Public Overrides Property Font() As System.Drawing.Font
        Get
            Return If(_fontSearching IsNot Nothing, _fontSearching, MyBase.Font)
        End Get
        Set(ByVal value As System.Drawing.Font)
            InitInternalFonts(value)
            RefreshTextBoxFont()
        End Set
    End Property
#End Region

#Region "methods"
    Private Function GetSelectFields() As String
        Dim hs As New HashSet(Of String)
        hs.Add(_displayMember)
        hs.Add(_valueMember)
        hs.RemoveWhere(Function(s) s = "")

        'Dim ary() As String = Nothing
        'hs.CopyTo(ary)

        Return String.Join(", ", hs.ToArray)
    End Function

    Private Function GetFieldLikeClause(ByVal searchText As String, ByVal fieldName As String, ByVal fieldSearchRegion As SearchRegion) As String
        If String.IsNullOrEmpty(fieldName) OrElse String.IsNullOrEmpty(searchText) OrElse fieldSearchRegion = SearchRegion.None Then
            Return ""
        End If

        Dim piecePattern As String = ""

        Select Case fieldSearchRegion
            Case SearchRegion.Contains
                piecePattern = "%" & searchText & "%"
            Case SearchRegion.EndsWith
                piecePattern = "%" & searchText
            Case SearchRegion.StartsWith
                piecePattern = searchText & "%"
            Case Else
                Debug.Assert(False)
                Return ""
        End Select

        Return fieldName & " like " & piecePattern.SQLQuote
    End Function

    Private Function TextMatchesSelectedItem() As Boolean
        TextMatchesSelectedItem = False

        If _selectedItem Is Nothing OrElse _selectedItem.Row.RowState = DataRowState.Detached Then
            Exit Function
        End If

        If String.IsNullOrEmpty(_displayMember) Then
            Exit Function
        End If

        Try
            If _selectedItem.Item(_displayMember) <> MyBase.Text Then
                Exit Function
            End If
        Catch e As RowNotInTableException
            Exit Function
        End Try

        TextMatchesSelectedItem = True
    End Function

    Private Sub ShowPopup()
        If TextMatchesSelectedItem() Then
            Return
        End If

        _list.Width = MyBase.Width
        _list.Height = _list.ItemHeight * Math.Min(15, Math.Max(5, _list.Items.Count)) + (_list.Bounds.Height - _list.DisplayRectangle.Height)
        _ctrl.Size = _list.Size
        _popupControl.Size = _list.Size

        _popupControl.Show(Me, New Point(-2, MyBase.Height - 2), ToolStripDropDownDirection.BelowRight)
    End Sub

    Private Sub AddDataTableColumn(ByVal columnName As String)
        If String.IsNullOrEmpty(columnName) Then
            Return
        End If

        If Not _dataTable.Columns.Contains(columnName) Then
            _dataTable.Columns.Add(columnName, GetType(String))
        End If
    End Sub

    Private Sub RemoveDataTableColumn(ByVal columnName As String)
        If String.IsNullOrEmpty(columnName) Then
            Return
        End If

        If _dataTable.Columns.Contains(columnName) Then
            _dataTable.Columns.Remove(columnName)
        End If
    End Sub

    Private Sub InitInternalFonts(ByVal value As System.Drawing.Font)
        _fontSearching = value
        _fontHasValue = New Font(value, FontStyle.Underline)
        _fontNothingFound = New Font(value, FontStyle.Italic)
    End Sub

    Private Sub RefreshTextBoxFont()
        RefreshTextBoxFont(Me.Focused)
    End Sub

    Private Sub RefreshTextBoxFont(ByVal newIsFocused As Boolean)
        If TextMatchesSelectedItem() Then
            MyBase.Font = _fontHasValue
        ElseIf newIsFocused Then
            MyBase.Font = _fontSearching
        Else
            MyBase.Font = _fontNothingFound
        End If
    End Sub

    Private Sub ApplySelectedItem()
        Dim selectedRow = TryCast(_list.SelectedItem, DataRowView)

        If selectedRow Is Nothing Then
            Me.Select()
            Return
        End If

        _popupControl.Close()

        SelectedValue = selectedRow(_valueMember)
    End Sub

    Private Function SelectDataItemForSelectedValue() As Boolean
        Static dataRowsForSelectedValue As New DataView(_dataTable)
        dataRowsForSelectedValue.RowFilter = _valueMember & " = " & _selectedValue.ToString.SQLQuote

        If dataRowsForSelectedValue.Count = 0 Then
            Return False
        End If

        _selectedItem = dataRowsForSelectedValue(0)
        MyBase.Text = _selectedItem(_displayMember)

        RefreshTextBoxFont()

        Return True
    End Function
#End Region

#Region "textbox events"
    Protected Overrides Sub OnEnter(ByVal e As System.EventArgs)
        RefreshTextBoxFont(True)

        If Me.Text.Trim <> "" And _selectedItem Is Nothing Then
            ShowPopup()
        End If

        MyBase.OnEnter(e)
    End Sub

    Protected Overrides Sub OnLeave(ByVal e As System.EventArgs)
        RefreshTextBoxFont(False)
        _popupControl.Hide()
        MyBase.OnLeave(e)
    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As System.Windows.Forms.Message, ByVal keyData As System.Windows.Forms.Keys) As Boolean
        If keyData = Keys.Tab AndAlso _popupControl.Visible Then
            ApplySelectedItem()
        End If

        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    Protected Overrides Sub OnKeyDown(ByVal e As System.Windows.Forms.KeyEventArgs)
        Dim delta As Integer = 0
        Dim visibleItems As Integer = Math.Max(1, (_list.DisplayRectangle.Height \ _list.ItemHeight) - 1)

        Select Case e.KeyCode
            Case Keys.Up
                delta = -1
            Case Keys.Down
                delta = 1
            Case Keys.PageUp
                delta = -visibleItems
            Case Keys.PageDown
                delta = visibleItems
        End Select

        If delta <> 0 Then
            If _list.Items.Count > 0 AndAlso _list.SelectionMode <> SelectionMode.None Then
                Dim targetIndex = _list.SelectedIndex + delta
                _list.SelectedIndex = Math.Max(0, Math.Min(_list.Items.Count - 1, targetIndex))
            End If
            e.Handled = True
            Return
        End If

        If e.KeyCode = Keys.Enter AndAlso _popupControl.Visible Then
            ApplySelectedItem()
            e.Handled = True
            Return
        End If

        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If e.KeyChar = vbTab Then
            e.Handled = True
            Return
        End If

        MyBase.OnKeyPress(e)
    End Sub

    Protected Overrides Sub OnMouseUp(ByVal mevent As System.Windows.Forms.MouseEventArgs)
        If MyBase.Focused AndAlso MyBase.Text.Trim <> "" Then
            ShowPopup()
        End If

        MyBase.OnMouseUp(mevent)
    End Sub

    Protected Overrides Sub OnParentChanged(ByVal e As System.EventArgs)
        _parentForm = TryCast(MyBase.Parent, Form)

        MyBase.OnParentChanged(e)
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Select Case Me.Text.Length
            Case 1
                tmrDelayedSearch.Interval = 3000
            Case 2
                tmrDelayedSearch.Interval = 2000
            Case 3
                tmrDelayedSearch.Interval = 1000
            Case 4
                tmrDelayedSearch.Interval = 750
            Case 5
                tmrDelayedSearch.Interval = 500
            Case Else
                tmrDelayedSearch.Interval = 350
        End Select

        tmrDelayedSearch.Start()
    End Sub

    Private Sub GetSearchResults(e As EventArgs)

        tmrDelayedSearch.Stop()

        If MyBase.Text.Trim = "" Then
            _popupControl.Visible = False
        End If

        If TextMatchesSelectedItem() Then
            GoTo bail
        End If

        If Not DatabaseFieldsOK() Then
            GoTo bail
        End If

        SelectedValue = Nothing
        RefreshTextBoxFont()

        If MyBase.Text.Trim = "" Then
            GoTo bail
        End If

        If MyBase.Text.Trim.Length < 2 Then
            GoTo bail
        End If

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("select distinct * from (")
        sSQL.AppendLine("select " & If(_resultsRowLimit > 0, "top " & _resultsRowLimit & " ", "") & GetSelectFields() & ", orderby = " & If(String.IsNullOrEmpty(OrderBy), _displayMember, OrderBy))
        sSQL.AppendLine("from " & _tableName)
        sSQL.AppendLine("where (1=1)")
        If Not String.IsNullOrEmpty(_filterClause) Then
            sSQL.AppendLine(_filterClause)
        End If

        Dim pieces = MyBase.Text.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        For Each piece In pieces
            If piece <> "" Then
                Dim clauses As New List(Of String)
                clauses.Add(GetFieldLikeClause(piece, _displayMember, _displaySearchRegion))
                clauses.Add(GetFieldLikeClause(piece, _valueMember, _valueSearchRegion))
                clauses.RemoveAll(Function(s) s = "")

                If clauses.Count > 0 Then
                    sSQL.AppendLine("and (")
                    sSQL.AppendLine(String.Join(" or ", clauses.ToArray))
                    sSQL.AppendLine(")")
                End If
            End If
        Next

        sSQL.AppendLine(") x")
        sSQL.AppendLine("order by orderby," & _displayMember)

        _dataTable.Rows.Clear()
        Using a As New SqlDataAdapter(sSQL.ToString, _connection)
            a.Fill(_dataTable)
        End Using

        _list.BeginUpdate()

        If _dataTable.Rows.Count = 0 Then
            _list.SelectionMode = SelectionMode.None
            _list.DataSource = Nothing
            If _list.Items.Count = 0 Then
                _list.Items.Add("<nothing found>")
            End If
        Else
            _list.SelectionMode = SelectionMode.One

            _list.DisplayMember = _displayMember
            _list.ValueMember = _valueMember
            _list.DataSource = _dataTable
        End If

        _list.EndUpdate()

        ShowPopup()

bail:
        MyBase.OnTextChanged(e)
    End Sub
#End Region

#Region "listbox events"
    Private Sub _list_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles _list.Click
        Debug.Print("_list_Click")
        ApplySelectedItem()
    End Sub

    Private Sub _list_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _list.MouseMove
        'Debug.Print("mousemove {0} {1}", e.X, e.Y)
        If _list.SelectionMode <> SelectionMode.None Then
            _list.SelectedIndex = _list.IndexFromPoint(e.X, e.Y)
        End If
    End Sub
#End Region

#Region "parent form events"
    Private Sub _parentForm_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _parentForm.LocationChanged
        If _parentForm.ActiveControl Is Me Then
            ShowPopup()
        End If
    End Sub

    Private Sub _parentForm_MouseCaptureChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _parentForm.MouseCaptureChanged
        If _parentForm.ActiveControl Is Me Then
            ShowPopup()
        End If
    End Sub

    Private Sub _parentForm_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles _parentForm.MouseWheel
        Const WHEEL_DELTA = 120

        If _parentForm.ActiveControl Is Me Then
            If _list.ClientRectangle.Contains(_list.PointToClient(MousePosition)) OrElse MyBase.ClientRectangle.Contains(MyBase.PointToClient(MousePosition)) Then
                _list.TopIndex = Math.Max(Math.Min(_list.TopIndex + 3 * -e.Delta / WHEEL_DELTA, _list.Items.Count - 1), 0)
            End If
        End If
    End Sub

    Private Sub _tmrDelayedSearch_Tick(sender As Object, e As EventArgs) Handles tmrDelayedSearch.Tick
        GetSearchResults(e)
    End Sub
#End Region

#Region "Popup helper class"
    Private Class MyPopup
        Inherits ToolStripDropDown

        Private Declare Function ShowCaret Lib "user32" (ByVal hWnd As IntPtr) As Boolean
        Private Declare Auto Function SendMessage Lib "user32" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr

        Private _owner As TextBox

        Private Const WM_KEYDOWN As Int32 = &H100
        Private Const WM_KEYUP As Int32 = &H101
        Private Const WM_CHAR As Int32 = &H102


        Public Sub New(ByRef owner As TextBox)
            _owner = owner
        End Sub

        Public Overrides Function PreProcessMessage(ByRef msg As System.Windows.Forms.Message) As Boolean
            If _owner.IsHandleCreated Then
                Select Case msg.Msg
                    Case WM_KEYDOWN, WM_KEYUP, WM_CHAR
                        msg.HWnd = _owner.Handle
                        Return _owner.PreProcessMessage(msg)
                End Select
            End If

            Return MyBase.PreProcessMessage(msg)
        End Function

        Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
            If _owner.IsHandleCreated Then
                Select Case m.Msg
                    Case WM_KEYDOWN, WM_KEYUP, WM_CHAR
                        m.HWnd = _owner.Handle
                        SendMessage(_owner.Handle, m.Msg, m.WParam, m.LParam)
                        Return
                End Select
            End If

            MyBase.WndProc(m)
        End Sub

        Protected Overrides Sub OnVisibleChanged(ByVal e As System.EventArgs)
            If Me.Visible Then
                ShowCaret(_owner.Handle)
            End If
            MyBase.OnVisibleChanged(e)
        End Sub
    End Class

#End Region

#Region "Popup ListBox helper class (hack)"
    Private Class MyListBoxHost
        Inherits UserControl

        Private WithEvents _lb As ListBox

        Public ReadOnly Property ListBox() As ListBox
            Get
                Return _lb
            End Get
        End Property

        Public Sub New()
            MyBase.New()

            Me._lb = New ListBox()
            With _lb
                .IntegralHeight = True
                .BorderStyle = BorderStyle.FixedSingle
                .Margin = New Padding(0)
                .SelectionMode = SelectionMode.One
            End With
            Me.Controls.Add(_lb)
        End Sub

        Private Sub _lb_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles _lb.Resize
            Me.Size = _lb.Size
        End Sub
    End Class
#End Region
End Class
