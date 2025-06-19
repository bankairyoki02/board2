Imports System.Text
Imports System.Data.SqlClient


Public Class frmScheduleSubstitution

    Private myEntityno As String = Nothing
    Private myPartno_original As String = Nothing
    Private myPartdesc_original As String = Nothing
    Private myQtyOrdered_original As Integer = Nothing

    Private IsDataLoaded As Boolean = False

    Public Event NotifyParentToRefresh()

    Public Sub New(ByVal entityno As String, ByVal entitydesc As String, ByVal partno_current As String, ByVal partdesc As String, ByVal qtyOrdered_current As Integer)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        myEntityno = entityno
        myPartno_original = partno_current
        myPartdesc_original = partdesc
        myQtyOrdered_original = qtyOrdered_current

        Me.Text += " (" & myPartno_original & " - " & partdesc & ")"

        lblProjectNo.Text = entityno
        lblProjectDesc.Text = entitydesc
        txtPartNo.Text = partno_current
        txtOrderedQty.Text = qtyOrdered_current

        Init_cboMultiPartList()

    End Sub

    Private Sub frmScheduleSubstitution_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        btnCommit.Focus()

    End Sub

    Private Sub Init_cboMultiPartList()

        Dim newConn As New SqlConnection(FinesseConnectionString)
        newConn.Open()

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("select amg.partno, p.partdesc, min(partseq) from dbo.Avail_Multipart_Groups amg join inpart p on amg.partno = p.partno where partgroup in ( select partgroup from dbo.Avail_Multipart_Groups amg where partno = " & SQLQuote(myPartno_original) & ") group by amg.partno, p.partdesc order by min(partseq), p.partdesc")
        Dim t = newConn.GetDataTable(sSQL)

        If t.Rows.Count = 0 Then t = newConn.GetDataTable("select p.partno, p.partdesc, 0 from inpart p where p.partno=" & SQLQuote(myPartno_original))

        newConn.Close()
        newConn.Dispose()

        cboMultiPartList.DisplayMember = "partdesc"
        cboMultiPartList.ValueMember = "partno"
        cboMultiPartList.DataSource = New DataView(t, "", "", DataViewRowState.CurrentRows)

        cboSplitMultipartList.DisplayMember = "partdesc"
        cboSplitMultipartList.ValueMember = "partno"
        cboSplitMultipartList.DataSource = New DataView(t, "", "", DataViewRowState.CurrentRows)

        Select Case cboSplitMultipartList.Items.Count
            Case > 1 : cboSplitMultipartList.SelectedIndex = 1
            Case 1 : cboSplitMultipartList.SelectedIndex = 0
            Case 0 : cboSplitMultipartList.SelectedIndex = -1
        End Select

        'If cboSplitMultipartList.Items.Count > 1 Then cboSplitMultipartList.SelectedIndex = 1 Else cboSplitMultipartList.SelectedIndex = 0

        IsDataLoaded = True

        cboMultiPartList.SelectedValue = myPartno_original
        If cboSplitMultipartList.Items.Count > 0 Then cboSplitMultipartList.SelectedIndex = 0

    End Sub

    Private Function _qtyLoaded_current(ByVal entityno As String, ByVal partno As String)
        Dim newConn As New SqlConnection(FinesseConnectionString)
        newConn.Open()
        _qtyLoaded_current = CDbl(NullIf(newConn.ExecuteScalar("select onhand = isnull(onhand,0) from inpartsub where batchno = " & SQLQuote(entityno) & " and partno = " & SQLQuote(partno)), CStr(0)))
        newConn.Close()
        newConn.Dispose()
        Return _qtyLoaded_current
    End Function

    Private Sub cboMultiPartList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMultiPartList.SelectedIndexChanged
        If Not IsDataLoaded Then Exit Sub
        txtPartNo.Text = cboMultiPartList.SelectedValue
    End Sub

    Private Sub cboSplitMultipartList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSplitMultipartList.SelectedIndexChanged
        If Not IsDataLoaded Then Exit Sub
        txtSplitPartNo.Text = cboSplitMultipartList.SelectedValue
    End Sub

    Private Sub chkSplit_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSplit.CheckedChanged
        pnlSplit.Visible = chkSplit.Checked
        btnDelete.Enabled = Not chkSplit.Checked
    End Sub

    Private Sub btnPartSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPartSearch.Click
        Static picker As New PartPicker(Me, btnPartSearch, FinesseConnectionString)

        If Not picker.GetPart() Then
            Exit Sub
        End If

        txtPartNo.Text = picker.PartNumber
        cboMultiPartList.Text = picker.PartDesc
    End Sub

    Private Sub btnSplitPartSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSplitPartSearch.Click
        Static picker As New PartPicker(Me, btnSplitPartSearch, FinesseConnectionString)

        If Not picker.GetPart() Then
            Exit Sub
        End If

        txtSplitPartNo.Text = picker.PartNumber
        cboSplitMultipartList.Text = picker.PartDesc
    End Sub

    Private Sub txtPartNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPartNo.TextChanged
        txtLoadedQty.Text = _qtyLoaded_current(myEntityno, txtPartNo.Text)
        btnDelete.Enabled = (myPartno_original = txtPartNo.Text)
    End Sub

    Private Sub txtSplitPartNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtSplitPartNo.TextChanged
        txtSplitLoadedQty.Text = _qtyLoaded_current(myEntityno, txtSplitPartNo.Text)
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If CDbl(txtLoadedQty.Text) > 0 Then
            Dim _is_are As String = IIf(CDbl(txtLoadedQty.Text) = 1, " is ", " are ")
            MsgBox("Cannot delete this part because " & txtLoadedQty.Text & _is_are & " checked out.", vbOKOnly)
            Exit Sub
        Else

            Try
                Dim newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()

                Dim sSQL As New StringBuilder
                sSQL.AppendLine("delete pjjobbudexp")
                sSQL.AppendLine("where entityno = " & SQLQuote(myEntityno))
                sSQL.AppendLine("and partno = " & SQLQuote(myPartno_original))
                sSQL.AppendLine("and est_qty = " & myQtyOrdered_original)
                sSQL.AppendLine("and line_no = 0")
                newConn.ExecuteNonQuery(sSQL)
                newConn.Close()
                newConn.Dispose()
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                Me.Close()
                RaiseEvent NotifyParentToRefresh()
            End Try

        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Sub QtyBoxesValidated(ByVal qty As String, ByVal qty_loaded As String, ByRef shouldReturn As Boolean)
        shouldReturn = False
        Dim iNumber As Integer
        If qty = String.Empty Then                          'Nothing was entered
            MsgBox("Action cancelled...")
            shouldReturn = True : Exit Sub
        ElseIf Not Integer.TryParse(qty, iNumber) Then      'Checking to see if input is a valid number >= 0
            MsgBox("Please enter a valid quantity.", vbOKOnly)
            shouldReturn = True : Exit Sub
        ElseIf CInt(qty) < 0 Then
            MsgBox("Please enter a valid quantity.", vbOKOnly)
            shouldReturn = True : Exit Sub
        ElseIf Len(qty) > 2 Then                            'Number with greater than 2 digits entered
            MsgBox("Really? Quantities with 3 digits are questionable.  Cancelling...")
            shouldReturn = True : Exit Sub
        ElseIf qty < CDbl(txtLoadedQty.Text) Then           'Loaded quantity is great than the updated quantity
            MsgBox("The Ordered Quantity cannot be less than the Loaded Quantity.", vbOKOnly)
            shouldReturn = True : Exit Sub
        End If
    End Sub

    Private Sub btnCommit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCommit.Click
        Dim qty_new As String = CDbl(txtOrderedQty.Text)
        Dim qty_loaded As String = CDbl(txtLoadedQty.Text)
        qty_new = qty_new.ToString.Replace("$"c, "").Replace(","c, "")

        Dim lShouldReturn_qty_new As Boolean
        QtyBoxesValidated(qty_new, qty_loaded, lShouldReturn_qty_new)
        If lShouldReturn_qty_new Then
            Return
        End If

        Dim qty_Split As String = CDbl(txtSplitOrderedQty.Text)
        Dim qty_SplitLoaded As String = CDbl(txtSplitLoadedQty.Text)
        qty_Split = qty_Split.ToString.Replace("$"c, "").Replace(","c, "")

        Dim lShouldReturn_qty_Split As Boolean
        QtyBoxesValidated(qty_Split, qty_SplitLoaded, lShouldReturn_qty_Split)
        If lShouldReturn_qty_Split Then
            Return
        End If

        If String.IsNullOrEmpty(txtPartNo.Text) Then
            MsgBox("No part number entered for substitution.", vbOKOnly)
            Return
        End If

        'If Not chkSplit.Checked And myPartno_original = txtPartNo.Text Then  'Ops uses Schedule Substitution to change Qty, so this check blows that up
        '    MsgBox("Substitute part is the same as the orignal part.")
        '    Exit Sub
        'End If

        If chkSplit.Checked And txtSplitPartNo.Text = myPartno_original Then
            MsgBox("Split part is the same as the orignal part.  If this is what you intended, put the original part as the first part in the list and a substitute as the second.")
            Exit Sub
        End If

        If chkSplit.Checked And String.IsNullOrEmpty(txtSplitPartNo.Text) Then
            MsgBox("No part number entered for split.", vbOKOnly)
            Return
        End If

        Debug.Print("set @entityno = " & SQLQuote(myEntityno))
        'Debug.Print("set @entitystartdate = " & SQLQuote(myStartDate))
        Debug.Print("set @original_partno = " & SQLQuote(myPartno_original))
        Debug.Print("set @original_qty = " & myQtyOrdered_original)
        Debug.Print("set @new_partno = " & SQLQuote(txtPartNo.Text))
        Debug.Print("set @new_qty = " & qty_new)
        Debug.Print("set @split_partno = " & IIf(chkSplit.Checked, SQLQuote(txtSplitPartNo.Text), DBNull.Value))
        Debug.Print("set @split_qty = " & IIf(chkSplit.Checked, qty_Split, DBNull.Value))

        Try
            Dim newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()

            Dim cmd As New SqlClient.SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Connection = newConn
            cmd.CommandText = "commit_part_substitution"
            cmd.Parameters.AddWithValue("@entityno", myEntityno)
            cmd.Parameters.AddWithValue("@original_partno", myPartno_original)
            cmd.Parameters.AddWithValue("@original_qty", myQtyOrdered_original)
            cmd.Parameters.AddWithValue("@new_partno", txtPartNo.Text)
            cmd.Parameters.AddWithValue("@new_qty", qty_new)
            cmd.Parameters.AddWithValue("@split_partno", IIf(chkSplit.Checked, txtSplitPartNo.Text, DBNull.Value))
            cmd.Parameters.AddWithValue("@split_qty", IIf(chkSplit.Checked, qty_Split, DBNull.Value))
            cmd.ExecuteNonQuery()

            If Not chkSplit.Checked Then
                Dim doesSubtituteExistInPartAssociations = newConn.ExecuteScalar("SELECT COUNT(*) FROM dbo.inpart_Reference ir WHERE ir.id_ReferenceType = 5 AND ir.parentpartno = " & myPartno_original.SQLQuote & " AND ir.partno = " & txtPartNo.Text.SQLQuote)

                If doesSubtituteExistInPartAssociations = 0 Then
                    Dim result = MsgBox("Add " & cboMultiPartList.Text & " as a known substitute for " & myPartdesc_original & "?", vbYesNo)
                    If result = MsgBoxResult.Yes Then
                        newConn.ExecuteNonQuery("INSERT dbo.inpart_Reference ( partno, parentpartno, Factor, id_ReferenceType ) VALUES ( " & txtPartNo.Text.SQLQuote & ", " & myPartno_original.SQLQuote & ", 1.0, 5) 

IF (SELECT COUNT(*) FROM dbo.inpart_Reference ir WHERE ir.id_ReferenceType = 5 AND ir.parentpartno = " & txtPartNo.Text.SQLQuote & " AND ir.partno = " & myPartno_original.SQLQuote & ") = 0
begin

INSERT dbo.inpart_Reference ( partno, parentpartno, Factor, id_ReferenceType ) VALUES ( " & myPartno_original.SQLQuote & ", " & txtPartNo.Text.SQLQuote & ", 1.0, 5)

end

")
                    End If
                End If
            End If



            newConn.Close()
            newConn.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Me.Close()
            RaiseEvent NotifyParentToRefresh()
        End Try

    End Sub
End Class