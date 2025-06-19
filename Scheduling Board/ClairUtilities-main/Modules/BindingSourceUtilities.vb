Imports System.Runtime.CompilerServices

Public Module BindingSourceUtilities

    <Extension()>
    Public Function GetDataRows(Of T As DataRow)(ByVal bs As BindingSource) As IEnumerable(Of T)
        Dim rows As New List(Of T)

        For Each rv As DataRowView In bs.List
            If rv IsNot Nothing AndAlso rv.Row IsNot Nothing Then
                rows.Add(rv.Row)
            End If
        Next

        Return rows
    End Function

    <Extension()>
    Public Function SetCurrentDataRow(ByVal bs As BindingSource, ByVal row As DataRow) As Boolean
        For i = 0 To bs.List.Count - 1
            Dim rv As DataRowView = bs.List(i)
            If rv IsNot Nothing AndAlso rv.Row Is row Then
                bs.Position = i
                Return True
            End If
        Next

        Return False
    End Function

    <Extension()>
    Public Function CurrentDataRow(Of T As DataRow)(ByVal bs As BindingSource) As T
        If bs.Position < 0 OrElse bs.Position >= bs.Count Then
            Return Nothing
        End If

        Dim rv = CType(bs.Current, DataRowView)

        If rv IsNot Nothing Then
            Return CType(rv.Row, T)
        End If

        Return Nothing
    End Function

    <Extension()>
    Public Function Find(ByVal bs As BindingSource, ByVal row As DataRow) As Integer

        For i = 0 To bs.List.Count - 1
            Dim current = CType(bs.List(i), DataRowView)
            If current.Row Is row Then Return i
        Next

        Return -1
    End Function

    ''' <summary>
    ''' Convert a column's name to a canonical format usable by DataView's RowFilter property, 
    ''' which is usually passed-through from BindingSource Filter property. This handles column
    ''' names that include spaces or weird characters like ]
    ''' </summary>
    ''' <param name="col">DataColumn whose name should be quoted</param>
    ''' <returns>column name quoted for filtering, e.g. "hello ]world" -> "[hello ]]world]"</returns>
    <Extension()>
    Public Function NameQuotedForFilter(col As DataColumn) As String
        Return $"[{col.ColumnName.Replace("]", "\]")}]"
    End Function


End Module