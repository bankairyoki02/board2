Imports System.Windows.Forms
Imports System.Linq
Imports System.Runtime.CompilerServices

Public Module DataGridViewUtilities

    <System.Runtime.CompilerServices.Extension()>
    Public Sub SetCurrentDataRowAndColumn(ByVal dgv As DataGridView, ByVal currentRow As DataRow, ByVal Column As System.Windows.Forms.DataGridViewColumn)
        Debug.Assert(dgv.Columns.Contains(Column))

        Dim bs = TryCast(dgv.DataSource, BindingSource)
        If bs IsNot Nothing AndAlso bs.SetCurrentDataRow(currentRow) Then
            dgv.CurrentCell = dgv.CurrentRow.Cells(Column.Index)
        End If
    End Sub

    <System.Runtime.CompilerServices.Extension()>
    Public Function DataRow(Of T As DataRow)(ByVal gridRow As DataGridViewRow) As T
        Try
            Dim rv = CType(gridRow.DataBoundItem, DataRowView)

            If rv IsNot Nothing Then
                Return CType(rv.Row, T)
            End If
        Catch ex As IndexOutOfRangeException
            ' the fact that this happens is bogus
            Debug.Print("DataRow from DataGridViewRow IndexOutOfRangeException. r.Index = {0}, grid.rowscount= {1}", gridRow.Index, gridRow.DataGridView.RowCount)
        End Try

        Return Nothing
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetSelectedDataRows(Of T As DataRow)(ByVal dgv As DataGridView) As IEnumerable(Of T)
        Return (
            From r In dgv.GetSelectionRows
            Where r.DataBoundItem IsNot Nothing
            Select rv = CType(r.DataBoundItem, DataRowView)
            Select CType(rv.Row, T)
        )
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetSelectionRows(ByVal dgv As DataGridView) As IEnumerable(Of DataGridViewRow)
        Dim selectionRows =
            From r As DataGridViewRow In dgv.SelectedRows

        If Not selectionRows.Any Then
            selectionRows =
                From c As DataGridViewCell In dgv.SelectedCells
                Where c.RowIndex >= 0
                Select r = dgv.Rows(c.RowIndex)
        End If

        If Not selectionRows.Any Then
            If dgv.CurrentCell IsNot Nothing Then
                selectionRows = New DataGridViewRow() {dgv.CurrentCell.OwningRow}
            End If
        End If

        If Not selectionRows.Any Then
            selectionRows = Enumerable.Empty(Of DataGridViewRow)()
        End If

        ' for some reason, DataGridView.Selected* methods return items in the reverse order in which they
        ' were selected, so this puts them back into the (correct) order in which they were selected.
        Return selectionRows.Reverse
    End Function

    Public Sub EnableDoubleBuffering(ByVal control As System.Windows.Forms.Control)
        GetType(DataGridView).InvokeMember( _
                    "DoubleBuffered", _
                    Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.SetProperty, _
                    Nothing, _
                    control, _
                    New Object() {True} _
                    )
    End Sub

    Public Class TemporarilyUnbindDataGrid
        Implements IDisposable

        Private _disposedValue As Boolean ' To detect redundant calls

        Private Class GridInfo
            Private _grid As DataGridView
            Private _BindingSource As BindingSource
            Private _SelectedDataRows As HashSet(Of Object)

            Private _CurrentCellDataRow As Object
            Private _CurrentCellColumnIndex As Integer

            Private _FirstDisplayedScrollingColumnIndex As Integer
            Private _FirstDisplayedScrollingRowIndex As Integer

            Public Sub New(ByVal grid As DataGridView)
                _grid = grid
                _BindingSource = grid.DataSource

                _SelectedDataRows = New HashSet(Of Object)(
                    From r As DataGridViewRow In grid.SelectedRows
                    Select If(r.DataBoundItem Is Nothing, _BindingSource.Current, r.DataBoundItem)
                    )

                Dim currentCell = grid.CurrentCellAddress
                Dim currentRow = grid.CurrentRow
                If currentCell.X >= 0 AndAlso currentCell.Y >= 0 Then
                    _CurrentCellDataRow = currentRow.DataBoundItem
                    ' newly-added rows may not yet be assigned to currentRow.DataBoundItem
                    If _CurrentCellDataRow Is Nothing Then
                        _CurrentCellDataRow = _BindingSource.Current
                    End If
                    _CurrentCellColumnIndex = currentCell.X
                End If

                _FirstDisplayedScrollingColumnIndex = _grid.FirstDisplayedScrollingColumnIndex
                _FirstDisplayedScrollingRowIndex = _grid.FirstDisplayedScrollingRowIndex

                _BindingSource.RaiseListChangedEvents = False
            End Sub

            Public Sub ReapplySettings()
                _BindingSource.RaiseListChangedEvents = True
                _BindingSource.ResetBindings(False)

                Try
                    If _FirstDisplayedScrollingColumnIndex >= 0 AndAlso _FirstDisplayedScrollingColumnIndex < _grid.ColumnCount Then
                        _grid.FirstDisplayedScrollingColumnIndex = _FirstDisplayedScrollingColumnIndex
                    End If
                    If _FirstDisplayedScrollingRowIndex >= 0 AndAlso _FirstDisplayedScrollingRowIndex < _grid.RowCount Then
                        _grid.FirstDisplayedScrollingRowIndex = _FirstDisplayedScrollingRowIndex
                    End If
                Catch ex As Exception
                    Debug.Print(ex.Message)
                End Try

                If _CurrentCellDataRow IsNot Nothing Then
                    For Each r As DataGridViewRow In _grid.Rows
                        If r.DataBoundItem Is _CurrentCellDataRow Then
                            _grid.CurrentCell = r.Cells(_CurrentCellColumnIndex)
                            Exit For
                        End If
                    Next
                End If

                For Each r As DataGridViewRow In _grid.Rows
                    r.Selected = _SelectedDataRows.Contains(r.DataBoundItem)
                Next

            End Sub
        End Class

        Private _gridInfo As List(Of GridInfo)

        Public Sub New(ByVal ParamArray grids() As DataGridView)
            _gridInfo = grids.ToList.ConvertAll(Function(grid) New GridInfo(grid))
        End Sub

        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me._disposedValue Then
                If disposing Then
                    _gridInfo.ForEach(Sub(gi) gi.ReapplySettings())
                End If

                _gridInfo = Nothing
            End If
            Me._disposedValue = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Public Sub EnableDoubleBuffering(ByVal control As System.Windows.Forms.Control)
            GetType(DataGridView).InvokeMember(
                        "DoubleBuffered",
                        Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.SetProperty,
                        Nothing,
                        control,
                        New Object() {True}
                        )
        End Sub
    End Class

    <Extension()>
    Public Function ToListOfObject(Of T)(rows As DataGridViewRowCollection, converter As Func(Of DataGridViewRow, T), Optional condition As Func(Of DataGridViewRow, Boolean) = Nothing) As List(Of T)
        Dim list As New List(Of T)()
        For Each row As DataGridViewRow In rows
            If condition Is Nothing OrElse (condition IsNot Nothing AndAlso condition(row)) Then
                list.Add(converter(row))
            End If
        Next
        Return list
    End Function

    <Extension()>
    Public Function ToListOfObject(Of T)(rows As DataGridViewSelectedRowCollection, converter As Func(Of DataGridViewRow, T), Optional condition As Func(Of DataGridViewRow, Boolean) = Nothing) As List(Of T)

        Dim list As New List(Of T)()
        For Each row As DataGridViewRow In rows
            If condition Is Nothing OrElse (condition IsNot Nothing AndAlso condition(row)) Then
                list.Add(converter(row))
            End If
        Next
        Return list
    End Function
End Module
