Imports System.Data.SqlClient

Friend Class frmPartPicker
    Private _LVWColumnSorter As ListViewColumnSorter
    Private _PartPicker As PartPicker

    Private _keyIsdown As Boolean = True
    Private _searchStringFromKeyDown As String = ""
    Private _expectedCommand As SqlCommand = Nothing

    Private _dtSubCategories As DataTable

    Private Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Friend Sub New(ByVal picker As PartPicker)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _PartPicker = picker

    End Sub

    Public ReadOnly Property Part_Picker() As PartPicker
        Get
            Return _PartPicker
        End Get
    End Property


#Region "Events"

    Private Sub frmPartPicker_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        _PartPicker.PartSelected = False
        Me.MinimumSize = New System.Drawing.Size(Me.Width, Me.Height)

        MaybePositionFormOverControl(Me, _PartPicker.PositionOverControl)
        MaybePositionFormOverToolStripItem(Me, _PartPicker.PositionOverToolStripItem)

        If _PartPicker.SearchForBarcode Then
            lblEnterSearch.Text = "&Search for part by barcode, number, or description:"
        Else
            lblEnterSearch.Text = "&Search for part by number or description:"
        End If

        ' Create an instance of a ListView column sorter and assign it 
        ' to the ListView control.
        _LVWColumnSorter = New ListViewColumnSorter() With {.SortColumn = 1, .Order = System.Windows.Forms.SortOrder.Ascending}
        'lvParts.ListViewItemSorter = _LVWColumnSorter

        txtSearch.Text = _PartPicker.DefaultSearch

        'Fill Category and SubCategory
        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))

        jammer.Add("
                   select '(All)' as commodity, '(All)' as commoditydesc
                   union
                   select i.commodity, i.commoditydesc from dbo.incommodity i order by commoditydesc",
                   Sub(t)

                       cmbCategory.DisplayMember = "commoditydesc"
                       cmbCategory.ValueMember = "commodity"

                       cmbCategory.DataSource = t

                   End Sub)

        jammer.Add("select '(All)' as [SecondaryCategoryCode], '(All)' as [SecondaryCategoryDesc], '' as [commodity] 
                    union
                    select SecondaryCategoryCode, SecondaryCategoryDesc, commodity 
                    FROM dbo.PartSecondaryCategories",
                   Sub(t)
                       _dtSubCategories = t

                       Dim dtSub As New DataView(t)

                       dtSub.RowFilter = "SecondaryCategoryCode = '(All)'"

                       cmbSubCategory.DataSource = dtSub
                       cmbSubCategory.DisplayMember = "SecondaryCategoryDesc"
                       cmbSubCategory.ValueMember = "SecondaryCategoryCode"
                   End Sub)

        jammer.Execute()

        txtSearch.Select()
        txtSearch.SelectAll()

        GetParts()
    End Sub



    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        ItemSelected()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub lvParts_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvParts.DoubleClick
        ItemSelected()
    End Sub

    Private Sub lvParts_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvParts.Click
        ItemSelected()
    End Sub

    Private Sub lvParts_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvParts.ColumnClick

        If lvParts.ListViewItemSorter Is Nothing Then lvParts.ListViewItemSorter = _LVWColumnSorter

        ' Determine if the clicked column is already the column that is 
        ' being sorted.
        If (e.Column = _LVWColumnSorter.SortColumn) Then
            ' Reverse the current sort direction for this column.
            If (_LVWColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending) Then
                _LVWColumnSorter.Order = System.Windows.Forms.SortOrder.Descending
            Else
                _LVWColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending
            End If
        Else
            ' Set the column number that is to be sorted; default to ascending.
            _LVWColumnSorter.SortColumn = e.Column
            _LVWColumnSorter.Order = System.Windows.Forms.SortOrder.Ascending
        End If

        ' Perform the sort with these new sort options.
        lvParts.Sort()

    End Sub

    Private Sub lvParts_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvParts.Enter
        If (lvParts.SelectedItems.Count = 0 AndAlso lvParts.Items.Count > 0) Then
            lvParts.Items(0).Selected = True
        End If
    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        If Not _keyIsdown Then
            _searchStringFromKeyDown = txtSearch.Text
            _keyIsdown = True
        End If
    End Sub

    Private Sub txtSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyUp
        _keyIsdown = False
        If _searchStringFromKeyDown <> txtSearch.Text Then
            GetParts()
        End If
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        Select Case txtSearch.Text.Length
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

        If Not _keyIsdown Then
            GetParts()
        End If
    End Sub

    Private Sub lvParts_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvParts.SelectedIndexChanged
        btnOk.Enabled = (lvParts.Items.Count = 1 OrElse lvParts.SelectedIndices.Count = 1)
    End Sub

    Private Sub tmrDelayedSearch_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrDelayedSearch.Tick

        Try
            tmrDelayedSearch.Stop()
            If Trim(txtSearch.Text) = "" OrElse Trim(txtSearch.Text).Length < 3 Then
                lvParts.Visible = False
                lblEnterSearch.Visible = True
                lblNoData.Visible = False
                _expectedCommand = Nothing
                Exit Sub
            End If

            lblEnterSearch.Visible = False

            Dim sqlStr As New System.Text.StringBuilder()
            Dim pieces = txtSearch.Text.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries).Distinct.ToArray

            If _PartPicker.SearchForBarcode Then
                sqlStr.AppendLine($"select distinct p.partno, ip.partdesc, ip.commmodity{If(_PartPicker.UseGroups, ", partgroup = '!Barcode', partseq = 0", "")}")
                sqlStr.AppendLine("from dbo.inpartsub p")
                sqlStr.AppendLine("join dbo.inpart ip on ip.partno = p.partno")
                sqlStr.AppendLine($"where p.unique_no in ({String.Join(", ", pieces.Select(AddressOf SQLQuote))})")
                sqlStr.AppendLine("and (p.onhand > 0 Or p.parentunique_no <> '')")

                sqlStr.AppendLine("union all")
            End If

            sqlStr.AppendLine($"select distinct ip.partno, ip.partdesc, ip.commmodity{If(_PartPicker.UseGroups, ", amg.partgroup, amg.partseq", "")}")
            sqlStr.AppendLine("from dbo.inpart ip")
            If _PartPicker.UseGroups Then
                sqlStr.AppendLine("left outer join (select amg.partno, partgroup = min(amg.partgroup), partseq = min(amg.partseq) from dbo.Avail_Multipart_Groups amg group by amg.partno) amg on ip.partno = amg.partno")
            End If

            'Added for searching tags
            sqlStr.AppendLine("LEFT OUTER JOIN dbo.PartTagIdeas pt ON pt.partno = ip.partno")
            'End addition

            'Added for My Warehouses only
            If chkOnlyMyWarehouses.Checked Then
                'Dumbass - this didn't work --------------------------------------------------
                'sqlStr.AppendLine("JOIN inpartsub ips ON ips.partno = ip.partno AND ips.bld IN (SELECT WarehouseCode FROM dbo.WarehouseVisible)")

                sqlStr.AppendLine("JOIN inpartsub ips ON ips.partno = ip.partno AND ips.bld IN (SELECT warehouse_entity FROM dbo.pjtfrusr WHERE USER_NAME = SUSER_NAME())")

            End If

            sqlStr.AppendLine("where (1=1)")

            For Each piece In pieces
                sqlStr.AppendLine("and (")
                sqlStr.AppendLine(" ip.partdesc like " & ("%" & piece & "%").SQLQuote)
                sqlStr.AppendLine(" or")
                sqlStr.AppendLine(" ip.partno like " & ("%" & piece & "%").SQLQuote)
                sqlStr.AppendLine(" or")
                sqlStr.AppendLine(" pt.tag like " & ("%" & piece & "%").SQLQuote)
                sqlStr.AppendLine(")")
            Next

            'Category
            If cmbCategory.SelectedValue <> "(All)" And cmbCategory.SelectedValue.ToString.SQLEscape <> "" Then
                sqlStr.AppendLine($"and ip.commmodity = '{cmbCategory.SelectedValue.ToString.SQLEscape}'")
            End If

            'Sub-Category
            If cmbSubCategory.SelectedValue <> "(All)" And cmbSubCategory.SelectedValue.ToString.SQLEscape <> "" Then
                sqlStr.AppendLine($"and ip.SecondaryCategoryCode = '{cmbSubCategory.SelectedValue.ToString.SQLEscape}'")
            End If

            If chkHideUnusedParts.Checked Then
                sqlStr.AppendLine("and ip.commmodity <> 'UNUSED'")
            End If

            If chkMyPartsOnly.Checked AndAlso _PartPicker.UseGroups Then
                sqlStr.AppendLine("and amg.partgroup is not null")
            End If

            If _PartPicker.ExtraSQLFilter <> "" Then
                sqlStr.AppendLine("and " & _PartPicker.ExtraSQLFilter)
            End If

            sqlStr.AppendLine($"order by {If(_PartPicker.UseGroups, "partgroup desc, partseq, ", "")}partdesc")

            Static lastSearch As String = ""

            If lastSearch = sqlStr.ToString Then
                Exit Sub
            End If

            lastSearch = sqlStr.ToString
            Dim newConn As New SqlConnection(_PartPicker.ConnectionString)
            _expectedCommand = New SqlCommand(sqlStr.ToString, newConn)

            Threading.ThreadPool.QueueUserWorkItem(
                Sub()
                    Try
                        newConn.Open()
                        _expectedCommand.BeginExecuteReader(AddressOf GotPartsAsync, _expectedCommand)
                    Catch ex As Exception
                        MsgBox("SQL Connection Error:" & vbCrLf & vbCrLf & ex.Message)
                    End Try
                End Sub
                )
        Catch ex As Exception
            MsgBox("Part Search Error:" & vbCrLf & vbCrLf & ex.Message)
        End Try
    End Sub

#End Region

#Region "Methods"

    Public Sub GetParts()
        tmrDelayedSearch.Stop()
        tmrDelayedSearch.Start()
    End Sub

    Private Sub GotPartsAsync(ByVal result As IAsyncResult)
        Dim cmd As SqlCommand = CType(result.AsyncState, SqlCommand)
        Dim reader As SqlDataReader = cmd.EndExecuteReader(result)

        If Not Me.IsHandleCreated Then
            Return
        End If

        Me.Invoke(
            Sub()
                Try
                    If cmd Is _expectedCommand Then
                        Dim dt As New DataTable
                        dt.Load(reader)

                        '"Select partno,partdesc agency from glentities where partno like " & SQLQuote(txtPart.Text & "%") & " order by partno"

                        If (dt.Rows.Count > 0) Then
                            LoadList(dt)
                            lvParts.Visible = True
                            lblNoData.Visible = False
                        Else
                            lblNoData.Visible = True
                            lvParts.Visible = False
                        End If

                        btnOk.Enabled = (lvParts.Items.Count = 1 OrElse lvParts.SelectedIndices.Count = 1)
                    End If

                Catch ex As Exception
                    MsgBox("WTF?", MsgBoxStyle.Critical)
                Finally
                    If Not reader Is Nothing Then
                        reader.Close()
                    End If
                    cmd.Connection.Close()
                    cmd.Connection.Dispose()
                    cmd.Dispose()
                End Try
            End Sub
            )
    End Sub

    Private Sub LoadList(ByRef dt As DataTable)
        lvParts.SuspendLayout()

        Dim LVItemFromDataRow = Function(r) New ListViewItem(New String() {r("partno"), r("partdesc"), r("commmodity")})

        lvParts.Items.Clear()
        lvParts.Items.AddRange(dt.Rows.OfType(Of DataRow).Select(LVItemFromDataRow).ToArray)
        lvParts.ResumeLayout()
    End Sub

    Private Sub ItemSelected()
        Dim selectedItems = lvParts.SelectedItems
        Dim selectedItem As ListViewItem = Nothing

        If selectedItems.Count = 1 Then
            selectedItem = selectedItems(0)
        ElseIf lvParts.Items.Count = 1 Then
            selectedItem = lvParts.Items(0)
        End If

        If selectedItem IsNot Nothing Then
            _PartPicker.PartNumber = selectedItem.SubItems(0).Text
            _PartPicker.PartDesc = selectedItem.SubItems(1).Text
            _PartPicker.PartCategory = selectedItem.SubItems(2).Text
            _PartPicker.DefaultSearch = txtSearch.Text
            _PartPicker.PartSelected = True
            Me.Close()
            Me.Dispose()
        Else
            MsgBox("Please choose a part.")
        End If
    End Sub

    Private Sub chkHideUnusedParts_CheckedChanged(sender As Object, e As EventArgs) Handles chkHideUnusedParts.CheckedChanged, chkMyPartsOnly.CheckedChanged
        GetParts()
    End Sub

    Private Sub cmbCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCategory.SelectedIndexChanged

        If cmbCategory.SelectedValue <> "(All)" And cmbCategory.SelectedValue <> "" Then
            'set the SubCategory list

            Dim dvSubCat As New DataView(_dtSubCategories)
            dvSubCat.RowFilter = $"commodity = '{cmbCategory.SelectedValue}' or SecondaryCategoryDesc = '(All)'"
            dvSubCat.Sort = "SecondaryCategoryDesc"

            cmbSubCategory.DisplayMember = "SecondaryCategoryDesc"
            cmbSubCategory.ValueMember = "SecondaryCategoryCode"
            cmbSubCategory.DataSource = dvSubCat
        ElseIf cmbCategory.SelectedValue = "(All)" Then

            If Not _dtSubCategories Is Nothing Then
                Dim dvSubCat As New DataView(_dtSubCategories)
                dvSubCat.RowFilter = $"SecondaryCategoryDesc = '(All)'"

                cmbSubCategory.DisplayMember = "SecondaryCategoryDesc"
                cmbSubCategory.ValueMember = "SecondaryCategoryCode"
                cmbSubCategory.DataSource = dvSubCat
            End If
        Else
            Exit Sub
        End If

        GetParts()
    End Sub

    Private Sub cmbSubCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbSubCategory.SelectedIndexChanged
        GetParts()
    End Sub

    Private Sub chkOnlyMyWarehouses_Click(sender As Object, e As EventArgs) Handles chkOnlyMyWarehouses.Click
        GetParts()
    End Sub

#End Region

End Class