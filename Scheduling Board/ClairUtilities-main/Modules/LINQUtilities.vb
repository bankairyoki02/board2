Public Module LINQUtilities

    <System.Runtime.CompilerServices.Extension()>
    Public Function MinOrDefault(Of t)(source As IEnumerable(Of t), ByVal defaultValue As t) As t
        If source.Any Then
            Return source.Min()
        End If

        Return defaultValue
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function MaxOrDefault(Of t)(source As IEnumerable(Of t), ByVal defaultValue As t) As t
        If source.Any Then
            Return source.Max()
        End If

        Return defaultValue
    End Function

    <System.Runtime.CompilerServices.Extension()> _
    Public Function AsDataTable(Of T)(ByVal varlist As IEnumerable(Of T)) As DataTable

        Dim dtReturn As New DataTable()

        If varlist.Count = 0 Then
            Return Nothing
        End If
        ' Could add a check to verify that there is an element 0
        Dim TopRec As T = varlist.ElementAt(0)


        ' Use reflection to get property names, to create table
        ' column names

        Dim oProps As System.Reflection.PropertyInfo() = DirectCast(TopRec.[GetType](), Type).GetProperties()

        For Each pi As System.Reflection.PropertyInfo In oProps
            dtReturn.Columns.Add(pi.Name, pi.PropertyType)
        Next

        For Each rec As T In varlist
            Dim dr As DataRow = dtReturn.NewRow()
            For Each pi As System.Reflection.PropertyInfo In oProps
                dr(pi.Name) = pi.GetValue(rec, Nothing)
            Next
            dtReturn.Rows.Add(dr)
        Next

        Return dtReturn

    End Function

    ''' <summary>
    ''' Convert a single column LINQ query to a hashset
    ''' </summary>
    ''' <typeparam name="T"></typeparam>
    ''' <param name="source"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <System.Runtime.CompilerServices.Extension()> _
    Public Function ToHashSet(Of T)(source As IEnumerable(Of T)) As HashSet(Of T)
        Return New HashSet(Of T)(source)
    End Function
End Module
