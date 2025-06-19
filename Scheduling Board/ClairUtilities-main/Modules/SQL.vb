Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Threading.Tasks

Public Module SQL
    <Extension()>
    Public Function SQLQuote(ByVal s As String) As String
        If Not String.IsNullOrEmpty(s) Then
            SQLQuote = "'" & s.Replace("'", "''") & "'"
        Else
            Return "''"
        End If
    End Function

    <Extension()>
    Public Function SQLEscape(ByVal s As String) As String
        Return s.Replace("[", "[[]").Replace("'", "''").Replace("_", "[_]").Replace("%", "[%]")
    End Function

    <Extension()>
    Public Function SQLDate(ByVal d As Date) As String
        Return "convert(datetime, '" & d.ToString("yyyy-MM-dd") & "', 121)"
    End Function

    <Extension()>
    Public Function SQLDateWithTime(ByVal d As Date) As String
        Return "convert(datetime, '" & d.ToString("yyyy-MM-dd hh:mm:ss") & "', 121)"
    End Function

    Public Function ReplaceNull(Of ReturnT)(possiblyNull As Object, NullReplacement As ReturnT) As ReturnT
        Return If(IsDBNull(possiblyNull), NullReplacement, possiblyNull)
    End Function

    Public Function NullIf(ByVal valueToCheck As Object, ByVal nullEquivalent As Object) As Object
        Return If(IsDBNull(valueToCheck) OrElse valueToCheck = nullEquivalent, DBNull.Value, valueToCheck)
    End Function

    <Extension()>
    Public Function ReplaceNull(Of ReturnT)(ByVal r As SqlDataReader, ByVal columnName As String, ByVal nullReplacement As ReturnT) As ReturnT
        Return If(r.IsDBNull(r.GetOrdinal(columnName)), nullReplacement, r(columnName))
    End Function

    <Extension()>
    Public Function ReplaceNull(Of ReturnT)(ByVal r As DataRow, ByVal columnName As String, ByVal nullReplacement As ReturnT) As ReturnT
        Return If(r.IsNull(columnName), nullReplacement, r(columnName))
    End Function

    <Extension()>
    Public Function AssignExistingParameterValue(Of T)(ByVal cmd As SqlCommand, ByRef value As T, ByVal parameterName As String)
        If cmd.Parameters.Contains(parameterName) Then
            Dim parameterValue = cmd.Parameters.Item(parameterName).Value
            If Not IsDBNull(parameterValue) Then
                value = CType(parameterValue, T)
                Return True
            End If
        End If

        Return False
    End Function


    <Extension()>
    Public Function isNull(ByVal o As Object, ByVal nullReplacement As Object) As Object
        Return If(o Is Nothing, nullReplacement, o)
    End Function

    <Extension()>
    Public Function ExecuteScalar(ByVal Conn As SqlConnection, ByVal sqlStr As String) As Object

        Using cmd As New SqlCommand(sqlStr, Conn)
            ExecuteScalar = cmd.ExecuteScalar()
        End Using

    End Function

    <Extension()>
    Public Function ExecuteScalar(ByVal Conn As SqlConnection, ByVal sqlStr As StringBuilder) As Object
        Return ExecuteScalar(Conn, sqlStr.ToString)
    End Function

    <Extension()>
    Public Sub ExecuteNonQuery(ByVal Conn As SqlConnection, ByVal sqlStr As String, Optional commandTimeoutSeconds As Integer = 30)
        Using cmd As New SqlCommand(sqlStr, Conn) With {.CommandTimeout = commandTimeoutSeconds}
            cmd.DebugCommand()
            cmd.ExecuteNonQuery()
        End Using

    End Sub

    <Extension()>
    Public Sub ExecuteNonQuery(ByVal Conn As SqlConnection, ByVal sqlStr As StringBuilder)
        ExecuteNonQuery(Conn, sqlStr.ToString)
    End Sub

    <Extension()>
    Public Sub ExecuteStoredProcedure(ByVal Conn As SqlConnection, ByVal procedureName As String)
        Using cmd As New SqlCommand(procedureName, Conn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.DebugCommand()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    <Extension()>
    Public Sub ExecuteStoredProcedure(ByVal Conn As SqlConnection, ByVal procedureName As String, ByVal parameters As String())
        If (Conn.State <> ConnectionState.Open) Then
            Conn.Open()
        End If

        Dim cmd As SqlCommand
        Dim result As DataTable = New DataTable()

        cmd = Conn.CreateCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = procedureName

        If (parameters IsNot Nothing) AndAlso (parameters.Length >= 2) Then
            For i As Integer = 0 To parameters.Length - 1 Step 2
                Console.WriteLine($"@{parameters(i)} = '{parameters(i + 1)}'")
                cmd.Parameters.AddWithValue(parameters(i), parameters(i + 1))
            Next
        End If

        cmd.DebugCommand()
        cmd.ExecuteNonQuery()

        Conn.Close()
    End Sub

    <Extension()>
    Public Async Function ExecuteStoredProcedureAsync(ByVal Conn As SqlConnection, ByVal procedureName As String, ByVal parameters As String(), Optional autoClose As Boolean = True) As Task
        If (Conn.State <> ConnectionState.Open) Then
            Await Conn.OpenAsync()
        End If

        Dim cmd As SqlCommand
        Dim result As DataTable = New DataTable()

        cmd = Conn.CreateCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = procedureName

        If (parameters IsNot Nothing) AndAlso (parameters.Length >= 2 And parameters.Length Mod 2 = 0) Then
            For i As Integer = 0 To parameters.Length - 1 Step 2
                cmd.Parameters.AddWithValue(parameters(i), parameters(i + 1))
            Next
        Else
            Throw New ArgumentException("The number of parameters must be even.")
        End If

        cmd.DebugCommand()
        Await cmd.ExecuteNonQueryAsync()

        If (autoClose) Then
            Conn.Close()
        End If

    End Function

    <Extension()>
    Public Function DebugCommand(ByVal cmd As SqlCommand) As String
        Dim debugMessage As New StringBuilder()
        ' Print BEGIN QUERY
        debugMessage.AppendLine("/*-----------------------------------------------------------------")
        debugMessage.AppendLine("                     BEGIN QUERY                                   ")
        debugMessage.AppendLine("-----------------------------------------------------------------*/")
        debugMessage.AppendLine()


        ' Print parameters (if CommandType is StoredProcedure)
        If cmd.CommandType = CommandType.StoredProcedure Then
            debugMessage.AppendLine($"EXEC {cmd.CommandText}")

            For Each param As SqlParameter In cmd.Parameters
                debugMessage.AppendLine($"{param.ParameterName} = '{param.Value}',")
            Next
            debugMessage.AppendLine()
            Dim debugger = debugMessage.ToString().Trim().TrimEnd(","c, " "c)

            debugMessage = New StringBuilder()
            debugMessage.Append(debugger)
        Else
            debugMessage.AppendLine(cmd.CommandText)
        End If

        debugMessage.AppendLine()
        debugMessage.AppendLine("/*---------------------------------------------------------------")
        debugMessage.AppendLine("                     END QUERY                                   ")
        debugMessage.AppendLine("---------------------------------------------------------------*/")


        Console.WriteLine(debugMessage.ToString())
        Return debugMessage.ToString()
    End Function


    <Extension()>
    Public Function GetDataSet(ByVal Conn As SqlConnection, ByVal sqlStr As String) As DataSet

        Dim ds As New DataSet()
        Using adapter As New SqlDataAdapter(sqlStr, Conn)
            adapter.Fill(ds)
        End Using
        Return ds

    End Function

    <Extension()>
    Public Function GetDataSet(ByVal Conn As SqlConnection, ByVal sqlStr As StringBuilder) As DataSet
        Return GetDataSet(Conn, sqlStr.ToString)
    End Function

    <Extension()>
    Public Function GetDataTable(ByVal Conn As SqlConnection, ByVal sqlStr As String) As DataTable

        Dim dt As New DataTable()
        Using adapter As New SqlDataAdapter(sqlStr, Conn)
            adapter.Fill(dt)
        End Using

        Return dt

    End Function


    <Extension()>
    Public Function GetDataTable(ByVal cmd As SqlCommand) As DataTable

        Dim dt As New DataTable()
        Using adapter As New SqlDataAdapter(cmd)
            adapter.Fill(dt)
        End Using

        Return dt

    End Function

    Public Delegate Sub GotDataTableAsync(ByVal t As DataTable)
    <Extension()>
    Public Sub GetDataTableAsync(ByVal conn As SqlConnection, ByVal sqlStr As String, ByVal gotDataTableAsync As GotDataTableAsync)

        Dim cmd = New SqlCommand(sqlStr, conn)
        cmd.BeginExecuteReader(
            Sub(result As IAsyncResult)
                Dim dt As New DataTable
                Dim reader As SqlDataReader = cmd.EndExecuteReader(result)
                dt.Load(reader)

                reader.Close()
                cmd.Dispose()

                gotDataTableAsync(dt)
            End Sub, Nothing)
    End Sub

    <Extension()>
    Public Function GetDataTable(ByVal Conn As SqlConnection, ByVal sqlStr As StringBuilder) As DataTable
        Return GetDataTable(Conn, sqlStr.ToString)
    End Function

    <Extension()>
    Public Function SQLCaseInsensitiveLike(ByVal value As String) As String
        Dim sb As StringBuilder = New StringBuilder()

        For Each c As Char In value

            If Char.IsLetter(c) Then
                sb.AppendFormat("[{0}{1}]", Char.ToLower(c), Char.ToUpper(c))
            Else
                sb.Append(c)
            End If
        Next

        Return sb.ToString()
    End Function

    Public Enum SearchPosition
        Start
        Within
    End Enum

    Public Function CreateSQLMultiColumnSearchClause(ByVal searchText As String, ByVal searchPositionForColumn As Dictionary(Of String, SearchPosition)) As String
        Dim likeFormatForSearchPosition As Dictionary(Of SearchPosition, String) = New Dictionary(Of SearchPosition, String)() From {
            {SearchPosition.Start, "{0}%"},
            {SearchPosition.Within, "%{0}%"}
        }
        Dim topLevelAndClauses As List(Of String) = New List(Of String)()
        Dim pieces As String() = searchText.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)

        For Each piece As String In pieces
            Dim innerOrClauses As List(Of String) = New List(Of String)()

            For Each column As String In searchPositionForColumn.Keys
                Dim searchPosition As SearchPosition = searchPositionForColumn(column)
                Dim format As String = likeFormatForSearchPosition(searchPosition)
                Dim quotedSearchTerm As String = String.Format(format, piece.SQLCaseInsensitiveLike()).SQLQuote()
                innerOrClauses.Add($"{column} like {quotedSearchTerm}")
            Next

            topLevelAndClauses.Add($"({String.Join(" or ", innerOrClauses.ToArray())})")
        Next

        Dim searchClause As String = ""

        If topLevelAndClauses.Count > 0 Then
            searchClause = String.Join(" and ", topLevelAndClauses.ToArray())
        End If

        Return searchClause
    End Function

    Public Sub BulkSave(ByVal tSource As DataTable, ByVal destinationTable As String, ByVal conn As SqlConnection)
        Dim tTemp As New DataTable

        tTemp.Columns.Add("is_insert", GetType(Boolean)).DefaultValue = False
        tTemp.Columns.Add("is_update", GetType(Boolean)).DefaultValue = False
        tTemp.Columns.Add("is_delete", GetType(Boolean)).DefaultValue = False

        For Each c As DataColumn In tSource.Columns
            If Not c.ReadOnly Or (tSource.PrimaryKey.Contains(c)) Then ' assuming that read-only columns will by calculated automatically by the DB
                tTemp.Columns.Add(c.ColumnName, c.DataType)
            End If
        Next

        For Each r As DataRow In tSource.Rows

            Select Case r.RowState
                Case DataRowState.Added, DataRowState.Deleted, DataRowState.Modified
                    tTemp.ImportRow(r)
                Case Else
                    Debug.Assert(r.RowState = DataRowState.Unchanged)
            End Select
        Next

        For Each r As DataRow In tTemp.Rows
            Select Case r.RowState
                Case DataRowState.Added
                    r("is_insert") = True
                Case DataRowState.Modified
                    r("is_update") = True
                Case DataRowState.Deleted
                    r.RejectChanges() ' necessary so that we can look at and modify fields.
                    r("is_delete") = True
            End Select
        Next

        Using bulkCopy As New SqlBulkCopy(conn)
            bulkCopy.DestinationTableName = destinationTable
            bulkCopy.BatchSize = 0

            bulkCopy.ColumnMappings.Clear()
            For Each c As DataColumn In tTemp.Columns
                Debug.Print(c.ColumnName)
                bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName)
            Next

            'tTemp.Columns("displayname").ColumnName = "state"
            'Dim foo = tTemp.Columns("state")

            bulkCopy.WriteToServer(tTemp)
        End Using
    End Sub

    <Extension()>
    Public Function ExecuteStoredProcedureAndGetData(ByVal Conn As SqlConnection, ByVal procedureName As String, ByVal paramnmeters As String()) As DataTable
        Dim cmd As SqlCommand
        Dim result As DataTable = New DataTable()

        cmd = Conn.CreateCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = procedureName

        If (paramnmeters IsNot Nothing) AndAlso (paramnmeters.Length >= 2) Then
            For i As Integer = 0 To paramnmeters.Length - 1 Step 2
                cmd.Parameters.AddWithValue(paramnmeters(i), paramnmeters(i + 1))
            Next
        End If

        'Here we can add automatically the version if need it

        Using adapter As New SqlDataAdapter(cmd)
            adapter.Fill(result)
        End Using

        Conn.Close()

        Return result
    End Function

    <Extension()>
    Public Function ExecuteStoredProcedureAndGetObject(Of T)(ByVal Conn As SqlConnection, ByVal procedureName As String, ByVal paramnmeters As String()) As T
        Using Conn
            If (Conn.State <> ConnectionState.Open) Then
                Conn.Open()
            End If

            Dim Data = Conn.ExecuteStoredProcedureAndGetData(procedureName, paramnmeters)
            Dim itemInstance = Activator.CreateInstance(GetType(T))
            If (Data.Rows.Count = 0) Then
                Return itemInstance
            End If
            Dim row = Data.Rows(0)
            Dim properties = GetType(T).GetProperties()
            For Each prop In properties
                If row.Table.Columns.Contains(prop.Name) Then
                    Dim valueFromDb = row(prop.Name)
                    If valueFromDb IsNot DBNull.Value Then
                        prop.SetValue(itemInstance, valueFromDb)
                    Else
                        prop.SetValue(itemInstance, Nothing)
                    End If
                End If
            Next
            Conn.Close()
            Return itemInstance
        End Using
    End Function

    <Extension()>
    Public Function ExecuteStoredProcedureAndGetACollection(Of T)(ByVal Conn As SqlConnection, ByVal procedureName As String, ByVal paramnmeters As String()) As IEnumerable(Of T)

        Using Conn
            If (Conn.State <> ConnectionState.Open) Then
                Conn.Open()
            End If

            Dim items As New List(Of T)()
            Dim Data = Conn.ExecuteStoredProcedureAndGetData(procedureName, paramnmeters)
            Dim properties = GetType(T).GetProperties()
            Dim result As T = Nothing
            If Data IsNot Nothing And Data.Rows.Count > 0 Then
                For Each row As DataRow In Data.Rows
                    Try
                        Dim item As T = Activator.CreateInstance(Of T)()
                        For Each prop In properties
                            Dim valueFromDb = row(prop.Name)
                            If valueFromDb IsNot DBNull.Value Then
                                prop.SetValue(item, valueFromDb)
                            Else
                                'This can be change to set the value to a default like empty string or 0
                                prop.SetValue(item, Nothing)
                            End If
                        Next
                        items.Add(item)
                    Catch ex As System.Exception
                        Throw New System.Exception(String.Format("Error parsing row {0}, the field does not exist in the object {1}", row, GetType(T).Name), ex.InnerException)
                    End Try
                Next
            End If

            Conn.Close()
            Return result
        End Using
    End Function

    <Extension()>
    Public Async Function ExecuteStoredProcedureAndGetDataAsync(ByVal Conn As SqlConnection, ByVal procedureName As String, ByVal Parameters As String()) As Task(Of Data.DataTable)
        Using cmd As New SqlCommand(procedureName, Conn)
            cmd.CommandType = CommandType.StoredProcedure

            If (Parameters IsNot Nothing) AndAlso (Parameters.Length >= 2 And Parameters.Length Mod 2 = 0) Then
                For i As Integer = 0 To Parameters.Length - 1 Step 2
                    cmd.Parameters.AddWithValue(Parameters(i), Parameters(i + 1))
                Next
            Else
                Throw New ArgumentException("The number of parameters must be even.")
            End If

            Dim Data As New Data.DataTable()
            cmd.DebugCommand()
            Using reader As SqlDataReader = Await cmd.ExecuteReaderAsync()
                Data.Load(reader)
            End Using

            Return Data
        End Using
    End Function




    <Extension()>
    Public Async Function ExecuteStoredProcedureAndGetACollectionAsync(Of T)(ByVal Conn As SqlConnection, ByVal procedureName As String, ByVal paramnmeters As String()) As Task(Of IEnumerable(Of T))
        Using Conn
            If (Conn.State <> ConnectionState.Open) Then
                Await Conn.OpenAsync()
            End If

            Dim items As New List(Of T)()
            Dim Data = Await Conn.ExecuteStoredProcedureAndGetDataAsync(procedureName, paramnmeters)
            Dim properties = GetType(T).GetProperties()

            If Data IsNot Nothing AndAlso Data.Rows.Count > 0 Then
                For Each row As DataRow In Data.Rows
                    Try
                        Dim item As T = Activator.CreateInstance(Of T)()
                        For Each prop In properties
                            Dim valueFromDb = row(prop.Name)
                            If valueFromDb IsNot DBNull.Value Then
                                Dim convertedValue = Convert.ChangeType(valueFromDb, prop.PropertyType)
                                prop.SetValue(item, convertedValue)
                            Else
                                ' This can be changed to set the value to a default like an empty string or 0
                                prop.SetValue(item, Nothing)
                            End If
                        Next
                        items.Add(item)
                    Catch ex As Exception
                        Throw New Exception(String.Format("Error parsing row {0}, the field does not exist in the object {1}", row, GetType(T).Name), ex.InnerException)
                    End Try
                Next
            End If

            Conn.Close()
            Conn.Dispose()
            Return items
        End Using
    End Function

End Module