Imports System.Reflection

Public Module DataTableUtilities

    <System.Runtime.CompilerServices.Extension()>
    Public Function ToDataTable(Of T)(collection As IEnumerable(Of T)) As DataTable
        Dim dt As New DataTable()
        Dim ty As Type = GetType(T)
        Dim pia As PropertyInfo() = ty.GetProperties()
        'Create the columns in the DataTable
        For Each pi As PropertyInfo In pia
            Dim exists = dt.Columns.OfType(Of DataColumn).FirstOrDefault(Function(col) col.ColumnName = pi.Name) IsNot Nothing
            If (Not exists) Then
                dt.Columns.Add(pi.Name, If(System.Nullable.GetUnderlyingType(pi.PropertyType), pi.PropertyType))
            End If

        Next
        'Populate the table
        For Each item As T In collection
            Dim dr As DataRow = dt.NewRow()
            dr.BeginEdit()
            For Each pi As PropertyInfo In pia
                'dr(pi.Name) = pi.GetValue(item, Nothing)
                dr(pi.Name) = If(pi.GetValue(item, Nothing), DBNull.Value)
            Next
            dr.EndEdit()
            dt.Rows.Add(dr)
        Next
        Return dt
    End Function

    ''' <summary>
    ''' Converts The Datatable into a List of specific object
    ''' </summary>
    ''' <typeparam name="T">The object to return</typeparam>
    ''' <param name="dataTable">DataTable to convert</param>
    ''' <returns>IEnumerable of your YourObject</returns>
    <System.Runtime.CompilerServices.Extension>
    Public Function ToListOfObject(Of T As New)(ByVal dataTable As DataTable) As IEnumerable(Of T)
        Dim objects As New List(Of T)()

        For Each row As DataRow In dataTable.Rows
            Dim obj As New T()

            For Each column As DataColumn In dataTable.Columns
                Dim propertyInfo = GetType(T).GetProperty(column.ColumnName)
                If propertyInfo IsNot Nothing AndAlso row(column) IsNot DBNull.Value Then
                    Dim value = row(column)
                    If propertyInfo.PropertyType.IsGenericType AndAlso propertyInfo.PropertyType.GetGenericTypeDefinition() = GetType(Nullable(Of )) Then
                        propertyInfo.SetValue(obj, Convert.ChangeType(value, Nullable.GetUnderlyingType(propertyInfo.PropertyType)))
                    Else
                        propertyInfo.SetValue(obj, Convert.ChangeType(value, propertyInfo.PropertyType))
                    End If
                End If
            Next

            objects.Add(obj)
        Next

        Return objects
    End Function
End Module
