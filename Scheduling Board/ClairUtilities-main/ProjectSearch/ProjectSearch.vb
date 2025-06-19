Imports System.Data.SqlClient
Imports System.Environment
Imports System.Linq
Imports System.Windows.Forms


Friend Class frmProjectPicker
    Private _ProjectPicker As ProjectPicker

    Private _keyIsdown As Boolean = True
    Private _searchStringFromKeyDown As String = ""
    Private _expectedCommand As SqlCommand = Nothing

    Private Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Friend Sub New(ByVal picker As ProjectPicker)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _ProjectPicker = picker

    End Sub

    Public ReadOnly Property Project_Picker() As ProjectPicker
        Get
            Return _ProjectPicker
        End Get
    End Property


#Region "Events"

    Private Sub frmProjectPicker_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.MinimumSize = New System.Drawing.Size(Me.Width, Me.Height)

        MaybePositionFormOverControl(Me, _ProjectPicker.PositionOverControl)
        MaybePositionFormOverToolStripItem(Me, _ProjectPicker.PositionOverToolStripItem)

        txtSearch.Text = _ProjectPicker.DefaultSearch

        txtSearch.Select()
        txtSearch.SelectAll()

        GetProjects()
    End Sub

    Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
        ItemSelected()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub lvProjects_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvProjects.DoubleClick
        ItemSelected()
    End Sub

    Private Sub lvProjects_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvProjects.Click
        ItemSelected()
    End Sub

    Private Sub lvProjects_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvProjects.Enter
        If (lvProjects.SelectedItems.Count = 0) Then
            lvProjects.Items(0).Selected = True
        End If
    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        If Not _keyIsdown Then
            _searchStringFromKeyDown = txtSearch.Text
            _keyIsdown = True
        End If
    End Sub

    Private Sub txtSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress
    End Sub

    Private Sub txtSearch_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyUp
        _keyIsdown = False
        If _searchStringFromKeyDown <> txtSearch.Text Then
            GetProjects()
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
            GetProjects()
        End If
    End Sub

    Private Sub lvProjects_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvProjects.SelectedIndexChanged
        btnOk.Enabled = (lvProjects.Items.Count = 1 OrElse lvProjects.SelectedIndices.Count = 1)
    End Sub

    Private Sub chkExcludeInactive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        GetProjects()
    End Sub

    Private Sub chkOnlyShowCurrentAndFuture_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        GetProjects()
    End Sub


    Private Sub tmrDelayedSearch_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrDelayedSearch.Tick
        If Trim(txtSearch.Text) = "" Then
            lvProjects.Visible = False
            lblEnterSearch.Visible = True
            lblNoData.Visible = False
            _expectedCommand = Nothing
            Exit Sub
        End If

        lblEnterSearch.Visible = False

        Dim sqlStr As New System.Text.StringBuilder()

        sqlStr.AppendLine("select top 300 p.entityno, p.entitydesc, s.StatusGroup")
        sqlStr.AppendLine("from dbo.glentities p")
        sqlStr.AppendLine("cross apply dbo.ProjectStatusGroup(p.engactivecd, p.enddate) s")
        sqlStr.AppendLine("where p.entitytype = 'PJ'")

        Dim pieces = txtSearch.Text.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        For Each piece In pieces
            If piece <> "" Then
                sqlStr.AppendLine("and (")
                sqlStr.AppendLine(" p.entitydesc like " & ("%" & piece & "%").SQLQuote)
                sqlStr.AppendLine(" or")
                sqlStr.AppendLine(" p.entityno like " & ("%" & piece & "%").SQLQuote)
                sqlStr.AppendLine(")")
            End If
        Next

        If Not String.IsNullOrEmpty(_ProjectPicker.ExtraSQLFilter) Then
            sqlStr.AppendLine("and " & _ProjectPicker.ExtraSQLFilter)
        End If

        sqlStr.AppendLine("order by")
        sqlStr.AppendLine("  s.SortOrder,")
        sqlStr.AppendLine("  min(entitydesc) over (partition by left(entityno, charindex('-', entityno+'-')-1)), entityno")

        Static lastSearch As String = ""

        If lastSearch = sqlStr.ToString Then
            Exit Sub
        End If

        Dim newConn = New SqlConnection(_ProjectPicker.ConnectionString)
        newConn.Open()

        lastSearch = sqlStr.ToString
        _expectedCommand = New SqlCommand(sqlStr.ToString, newConn)
        _expectedCommand.BeginExecuteReader(AddressOf GotProjectsAsync, _expectedCommand)
    End Sub
#End Region

#Region "Methods"

    Public Sub GetProjects()
        tmrDelayedSearch.Stop()
        tmrDelayedSearch.Start()
    End Sub

    Private Sub GotProjectsAsync(ByVal result As IAsyncResult)
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

                        '"Select entityno,entitydesc agency from glentities where entityno like " & SQLQuote(txtProject.Text & "%") & " order by entityno"

                        If (dt.Rows.Count > 0) Then
                            LoadList(dt, lvProjects, "StatusGroup")
                            lvProjects.Visible = True
                            lblNoData.Visible = False
                        Else
                            lblNoData.Visible = True
                            lvProjects.Visible = False
                        End If

                        btnOk.Enabled = (lvProjects.Items.Count = 1 OrElse lvProjects.SelectedIndices.Count = 1)
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
            End Sub)
    End Sub

    Private Sub LoadList(ByRef dt As DataTable, ByVal lv As System.Windows.Forms.ListView, Optional ByVal groupTitle As String = Nothing)
        Dim doingGroups As Boolean = Not String.IsNullOrEmpty(groupTitle)
        Dim groups = New Dictionary(Of String, ListViewGroup)

        Dim LVItemFromDataRow =
            Function(r As DataRow)
                Dim lvi = New ListViewItem(New String() {r("entityno"), r("entitydesc")})
                If doingGroups Then
                    lvi.Group = groups.Item(r(groupTitle))
                End If
                Return lvi
            End Function

        lv.SuspendLayout()

        lv.Groups.Clear()

        If doingGroups Then
            For Each r As DataRow In dt.Rows
                Dim headerText As String = r.Item(groupTitle)

                If Not groups.ContainsKey(headerText) Then
                    groups.Add(headerText, New ListViewGroup(headerText))
                End If
            Next

            lv.Groups.AddRange(groups.Values.ToArray)
        End If

        lv.Items.Clear()
        lv.Items.AddRange(dt.Rows.OfType(Of DataRow).Select(LVItemFromDataRow).ToArray)
        lv.ResumeLayout()
    End Sub

    Private Sub ItemSelected()
        Dim selectedItems = lvProjects.SelectedItems
        Dim selectedItem As ListViewItem = Nothing

        If selectedItems.Count = 1 Then
            selectedItem = selectedItems(0)
        ElseIf lvProjects.Items.Count = 1 Then
            selectedItem = lvProjects.Items(0)
        End If

        If selectedItem IsNot Nothing Then
            _ProjectPicker.ProjectNum = selectedItem.SubItems(0).Text
            _ProjectPicker.ProjectNumDesc = selectedItem.SubItems(1).Text
            _ProjectPicker.DefaultSearch = txtSearch.Text
            _ProjectPicker.ProjectSelected = True
            Me.Close()
            Me.Dispose()
        Else
            MsgBox("Please choose a project.")
        End If
    End Sub

#End Region
End Class