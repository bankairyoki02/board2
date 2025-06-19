Imports System.Data

Public Class frmGenericPicker
    Private _picker As GenericPicker

    Private _keyIsdown As Boolean = True
    Private _searchStringFromKeyDown As String = ""
    Private _initialRowFilter As String = ""
    Private _view As DataView
    Public _groupTitle As String = ""

    Private _displayColumns As Dictionary(Of DataColumn, String)
    Private _searchColumns As New Dictionary(Of String, GenericPicker.SearchPosition)

    Private _searchDelayTimer As Timer

    Private Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
    End Sub

    Public Sub New(ByVal picker As GenericPicker)
        Me.New()

        _picker = picker
        _picker.SelectedItem = Nothing
        _searchDelayTimer = New Timer With {
            .Interval = 100
        }
        AddHandler _searchDelayTimer.Tick, AddressOf SearchDelayTimer_Tick
    End Sub

    Public ReadOnly Property Picker() As GenericPicker
        Get
            Return _picker
        End Get
    End Property

    Private Sub frmGenericPicker_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub


    Private Sub frmGenericPicker_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.Text = "Choose " & _picker.ItemDescription
        Me.Size = _picker.Size
        Me.MinimumSize = New System.Drawing.Size(Me.Width, Me.Height)

        _displayColumns = _picker.DisplayColumns
        If _displayColumns Is Nothing OrElse _displayColumns.Count = 0 Then
            _displayColumns = New Dictionary(Of DataColumn, String)
            For Each c As DataColumn In _picker.ItemList.Table.Columns
                _displayColumns.Add(c, If(c.Caption <> "", c.Caption, c.ColumnName))
            Next
        End If

        For Each c In _displayColumns.Keys
            lvItems.Columns.Add(_displayColumns(c))
        Next

        If _picker.AllowSearching Then
            If _picker.SearchColumns.Count = 0 Then
                For Each c As DataColumn In _displayColumns.Keys
                    If c.DataType Is GetType(String) Then
                        _searchColumns.Add(c.ColumnName, GenericPicker.SearchPosition.Within)
                    End If
                Next
            Else
                For Each c As DataColumn In _picker.SearchColumns.Keys
                    If c.DataType Is GetType(String) Then
                        _searchColumns.Add(c.ColumnName, _picker.SearchColumns(c))
                    End If
                Next
            End If
        End If


        With _picker.ItemList
            _view = New DataView(.Table, .RowFilter, .Sort, .RowStateFilter)
        End With

        _groupTitle = Picker.GroupTitle
        lvItems.ShowGroups = Not String.IsNullOrEmpty(_groupTitle)

        _initialRowFilter = _view.RowFilter
        LoadList(_view, _groupTitle)
        lvItems.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)

        gbSearch.Visible = _picker.AllowSearching
        txtSearch.Text = _picker.SearchString

        ApplyControlVisibility()

        MaybePositionFormOverControl(Me, _picker.PositionOverControl)
        MaybePositionFormOverToolStripItem(Me, _picker.PositionOverToolStripItem)

    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        ItemSelected()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        CloseForm()
    End Sub

    Private Sub lvItems_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvItems.DoubleClick
        ItemSelected()
    End Sub

    Private Sub lvItems_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvItems.Click
        ItemSelected()
    End Sub

    Private Sub lvItems_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvItems.SelectedIndexChanged
        EnableOKButton()
    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        Dim delta As Integer = 0
        Select Case e.KeyCode
            Case Keys.Up
                delta = -1
            Case Keys.Down
                delta = 1
        End Select

        If delta <> 0 Then
            Dim targetIndex As Integer = 0
            If lvItems.SelectedIndices.Count = 0 Then
                targetIndex = If(delta = 1, 0, lvItems.Items.Count - 1)
            Else
                targetIndex = lvItems.SelectedIndices(0) + delta
            End If

            targetIndex = Math.Min(lvItems.Items.Count - 1, Math.Max(0, targetIndex))

            If lvItems.Items.Count > 0 Then
                lvItems.SelectedIndices.Clear()
                lvItems.SelectedIndices.Add(targetIndex)
                lvItems.FocusedItem = lvItems.Items(targetIndex)
            End If

            e.Handled = True
            Return
        End If

        If Not _keyIsdown Then
            _searchStringFromKeyDown = txtSearch.Text
            _keyIsdown = True
        End If
    End Sub

    Private Sub txtSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyUp
        _keyIsdown = False
        If _searchStringFromKeyDown <> txtSearch.Text Then
            'ApplySearch()
        End If
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        Select Case txtSearch.Text.Length
            Case 1
                _searchDelayTimer.Interval = 1500
            Case 2
                _searchDelayTimer.Interval = 1000
            Case 3
                _searchDelayTimer.Interval = 500
            Case 4
                _searchDelayTimer.Interval = 300
            Case 5
                _searchDelayTimer.Interval = 200
            Case Else
                _searchDelayTimer.Interval = 150
        End Select

        _searchDelayTimer.Stop()
        _searchDelayTimer.Start()
    End Sub

    Private Sub SearchDelayTimer_Tick(ByVal sender As Object, ByVal e As EventArgs)
        _searchDelayTimer.Stop()
        ApplySearch()
    End Sub

    Private Sub ApplySearch()
        Dim outerAndClauses As New List(Of String)

        If _initialRowFilter <> "" Then
            outerAndClauses.Add(_initialRowFilter)
        End If

        Dim pieces = txtSearch.Text.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        For Each piece In pieces
            If piece <> "" Then
                Dim innerOrClauses As New List(Of String)
                Static likeFormats As New Dictionary(Of GenericPicker.SearchPosition, String) From {
                    {GenericPicker.SearchPosition.Start, "{0}%"},
                    {GenericPicker.SearchPosition.Within, "%{0}%"}
                }

                For Each column In _searchColumns.Keys
                    innerOrClauses.Add(column & " like " & String.Format(likeFormats(_searchColumns(column)), piece).SQLQuote)
                Next

                outerAndClauses.Add("(" & String.Join(" or ", innerOrClauses.ToArray) & ")")
            End If
        Next

        Dim newFilter As String = ""
        If outerAndClauses.Count > 0 Then
            newFilter = String.Join(" and ", outerAndClauses.ToArray)
        End If

        Static lastFilter As String = ""

        If lastFilter = newFilter Then
            Exit Sub
        End If

        _view.RowFilter = newFilter
        lastFilter = newFilter

        LoadList(_view, _groupTitle)

        EnableOKButton()

        ApplyControlVisibility()
    End Sub

    Private Sub ApplyControlVisibility()
        If (_view.Count > 0) Then
            lvItems.Visible = True
            lblNoData.Visible = False
        Else
            lblNoData.Visible = True
            lvItems.Visible = False
        End If
    End Sub

    Private Sub EnableOKButton()
        btnOK.Enabled = ChosenItem() IsNot Nothing
    End Sub

    Private Sub ItemSelected()
        _picker.SelectedItem = ChosenItem()
        If _picker.SelectedItem IsNot Nothing Then
            CloseForm()
        Else
            MsgBox("Please choose an entry in the list.", vbExclamation)
        End If
    End Sub

    Private Function ChosenItem() As DataRowView
        If lvItems.SelectedItems.Count = 1 Then
            Return lvItems.SelectedItems(0).Tag
        ElseIf lvItems.Items.Count = 1 Then
            Return lvItems.Items(0).Tag
        Else
            Return Nothing
        End If
    End Function

    Private Sub CloseForm()
        Me.Close()
    End Sub

    Private Sub LoadList(ByRef v As DataView, Optional ByVal groupTitle As String = Nothing)
        Dim doingGroups As Boolean = Not String.IsNullOrEmpty(groupTitle)
        Dim groups = New Dictionary(Of String, ListViewGroup)

        Dim stringsFromDisplayFields = Function(r As DataRowView) _displayColumns.Keys.Select(
                                           Function(c)
                                               Dim columnValue = r(c.Ordinal)
                                               If TypeOf (columnValue) Is Date Then
                                                   Return String.Format("{0:dd-MMM-yyyy}", columnValue)
                                               Else
                                                   Return columnValue.ToString
                                               End If
                                           End Function)

        Dim LVItemFromDataRow = Function(r As DataRowView)
                                    Dim lvi As New ListViewItem(stringsFromDisplayFields(r).ToArray)
                                    lvi.Tag = r
                                    If _searchColumns.Keys.Any(Function(s) r(s).ToString.ToLower.Trim = txtSearch.Text.ToLower.Trim) Then
                                        lvi.Selected = True
                                    End If
                                    If doingGroups Then
                                        lvi.Group = groups.Item(r(groupTitle))
                                    End If
                                    Return lvi
                                End Function

        lvItems.SuspendLayout()

        lvItems.Groups.Clear()

        If doingGroups Then
            For Each r As DataRow In v.ToTable.Rows
                Dim headerText As String = r.Item(groupTitle)

                If Not groups.ContainsKey(headerText) Then
                    groups.Add(headerText, New ListViewGroup(headerText))
                End If
            Next

            lvItems.Groups.AddRange(groups.Values.ToArray)
        End If

        lvItems.Items.Clear()
        lvItems.Items.AddRange(v.OfType(Of DataRowView).Select(LVItemFromDataRow).ToArray)

        If lvItems.SelectedIndices.Count = 1 Then
            lvItems.FocusedItem = lvItems.SelectedItems(0)
        Else
            lvItems.SelectedIndices.Clear()
        End If

        lvItems.ResumeLayout()
    End Sub

    Private Sub frmGenericPicker_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        _picker.Size = Me.Size
        _picker.SearchString = txtSearch.Text
    End Sub
End Class