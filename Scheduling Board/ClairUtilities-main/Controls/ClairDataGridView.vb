Imports System.Windows.Forms

Public Class ClairDataGridView
    Inherits DataGridView

    Private Enum ColumnStep
        Forward = 1
        Backward = -1
    End Enum

    Private Function SelectFirstNonReadOnlyCellInRow(ByVal row As Integer, ByVal columnStart As Integer, ByVal columnEnd As Integer, ByVal direction As ColumnStep) As Boolean
        For col = columnStart To columnEnd Step direction
            If Not Me.Columns(col).ReadOnly AndAlso Me.Columns(col).Visible Then
                Try
                    Dim gridRow = Me.Rows(row)
                    Dim boundItem = gridRow.DataBoundItem
                    Me.CurrentCell = gridRow.Cells(col)

                    If Me.CurrentRow IsNot Nothing AndAlso Me.CurrentRow.DataBoundItem IsNot boundItem Then
                        For Each r As DataGridViewRow In Me.Rows
                            If r.DataBoundItem Is boundItem Then
                                Me.CurrentCell = r.Cells(col)
                            End If
                        Next
                    End If
                Catch
                End Try
                Return True
            End If
        Next
        Return False
    End Function

    Private Function HandleTab(ByVal keyData As Keys) As Boolean
        If (keyData And Keys.KeyCode) = Keys.Tab AndAlso Me.CurrentCell IsNot Nothing Then
            With Me.CurrentCell
                If (keyData And Keys.Shift) = 0 Then
                    If Not SelectFirstNonReadOnlyCellInRow(.RowIndex, .ColumnIndex + 1, Me.Columns.Count - 1, ColumnStep.Forward) Then
                        If .RowIndex < Me.RowCount - 1 Then
                            SelectFirstNonReadOnlyCellInRow(.RowIndex + 1, 0, .ColumnIndex, ColumnStep.Forward)
                        End If
                    End If
                Else
                    If Not SelectFirstNonReadOnlyCellInRow(.RowIndex, .ColumnIndex - 1, 0, ColumnStep.Backward) Then
                        If .RowIndex > 0 Then
                            SelectFirstNonReadOnlyCellInRow(.RowIndex - 1, Me.Columns.Count - 1, .ColumnIndex, ColumnStep.Backward)
                        End If
                    End If
                End If
            End With

            Return True
        End If

        Return False
    End Function

    Protected Overloads Overrides Function ProcessDialogKey(ByVal keyData As Keys) As Boolean
        If HandleTab(keyData) Then
            Return True
        End If

        Return MyBase.ProcessDialogKey(keyData)
    End Function

    Protected Overloads Overrides Function ProcessDataGridViewKey(ByVal e As KeyEventArgs) As Boolean
        If HandleTab(e.KeyData) Then
            Return True
        End If

        Return MyBase.ProcessDataGridViewKey(e)
    End Function
End Class
