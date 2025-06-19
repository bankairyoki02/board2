''' <summary>
''' Use this class when you need to manipulate multiple rows in a child table where the
''' parent table has calculated columns.
''' 
''' HACK: Deleting rows is apparently an N^2 operation if the rows are referenced by an aggregate
''' expression in a parent table.
'''       
''' Temporarily deleting those columns makes deleting lots of rows much faster, i.e.
''' deleting ~1700 rows from 123123-BM takes ~5 minutes with dtProjects expressions
''' and less than 1 second with expressions removed.
''' 
''' Note: BeginLoadData and EndLoadData do not have the same effect.
''' </summary>
'''
''' <example>
''' Using New TemporarilyRemoveExpressionColumns(parentTable)
'''     For Each r as DataRow in childTable.Rows
'''         r.Delete()
'''     Next
''' End Using
''' </example>
Public Class TemporarilyRemoveExpressionColumns
    Implements IDisposable

    Private _disposedValue As Boolean ' To detect redundant calls

    Private Class TableExpressionColumnRemoveAndReapply
        Private _table As DataTable
        Private _expressionColumns As DataColumn()
        Private _exceptions As New List(Of DataColumn)

        Public Sub New(ByVal table As DataTable)
            _table = table
            _expressionColumns = (From c As DataColumn In _table.Columns Where c.Expression <> "").ToArray()
            For Each ec In _expressionColumns
                _table.Columns.Remove(ec)
            Next
        End Sub

        Public Sub New(ByVal table As DataTable, ByVal exceptions As List(Of String))
            _table = table
            _expressionColumns = (From c As DataColumn In _table.Columns Where c.Expression <> "").ToArray()


            For Each ec In _expressionColumns
                If exceptions.Contains(ec.ColumnName) Then
                    _exceptions.Add(ec)
                Else
                    _table.Columns.Remove(ec)
                End If
            Next


        End Sub

        Public Sub ReapplySettings()
            For Each ec In _expressionColumns
                If _exceptions.Contains(ec) Then
                    Continue For
                End If
                _table.Columns.Add(ec)
            Next
        End Sub
    End Class

    Private _tableInfo As List(Of TableExpressionColumnRemoveAndReapply)

    Public Sub New(ByVal ParamArray tables() As DataTable)
        _tableInfo = tables.ToList.ConvertAll(Function(t) New TableExpressionColumnRemoveAndReapply(t))
    End Sub

    Public Sub New(ByVal exceptions As List(Of String), ByVal ParamArray tables() As DataTable)
        _tableInfo = tables.ToList.ConvertAll(Function(t) New TableExpressionColumnRemoveAndReapply(t, exceptions))
    End Sub

    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me._disposedValue Then
            If disposing Then
                For Each gi In _tableInfo.AsEnumerable().Reverse()
                    gi.ReapplySettings()
                Next
            End If

            _tableInfo = Nothing
        End If
        Me._disposedValue = True
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
End Class
