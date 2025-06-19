Public Class SortByColumns
    Private _columns As New List(Of String)
    Private _sortDirectionFromColumn As New Dictionary(Of String, SortDirection)

    Public Enum SortDirection
        Ascending = 1
        Descending = -1
    End Enum

    Public Sub New()

    End Sub

    Public Sub SetPrimarySort(columnName As String)
        Select Case _columns.IndexOf(columnName)
            Case -1
                _columns.Insert(0, columnName)
                _sortDirectionFromColumn(columnName) = SortDirection.Ascending
            Case 0
                _sortDirectionFromColumn(columnName) *= -1
            Case Else
                _columns.Remove(columnName)
                _columns.Insert(0, columnName)
                _sortDirectionFromColumn(columnName) = SortDirection.Ascending
        End Select
    End Sub

    Public Function ColumnsAndDirection() As IEnumerable(Of KeyValuePair(Of String, SortDirection))
        Return From column In _columns
               Select New KeyValuePair(Of String, SortDirection)(column, _sortDirectionFromColumn(column))
    End Function

    Public Sub SetColumnSortDirection(columnName As String, sortDirection As SortDirection)
        If _columns.IndexOf(columnName) = -1 Then
            _columns.Insert(0, columnName)
        End If

        _sortDirectionFromColumn(columnName) = sortDirection
    End Sub
End Class
