Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports System.Threading.Tasks

Module SQLObject

    <Extension()>
    Public Async Function ExecuteSTPDAndGetACollectionAsync(Of T As New)(ByVal Conn As SqlConnection, ByVal procedureName As String, Optional ByVal parameters As String() = Nothing) As Task(Of IEnumerable(Of T))
        Dim items As New List(Of T)()

        If Conn.State <> ConnectionState.Open Then
            Await Conn.OpenAsync()
        End If

        Using cmd As New SqlCommand(procedureName, Conn)
            cmd.CommandType = CommandType.StoredProcedure

            If parameters IsNot Nothing AndAlso parameters.Length >= 2 AndAlso parameters.Length Mod 2 = 0 Then
                For i As Integer = 0 To parameters.Length - 1 Step 2
                    cmd.Parameters.AddWithValue(parameters(i), parameters(i + 1))
                Next
            Else
                If (parameters IsNot Nothing) Then
                    Throw New ArgumentException("The number of parameters must be even.")
                End If

            End If

            cmd.DebugCommand()

            Using reader As SqlDataReader = Await cmd.ExecuteReaderAsync()
                Dim properties = GetType(T).GetProperties()
                Dim columnNames As New HashSet(Of String)(Enumerable.Range(0, reader.FieldCount).Select(Function(i) reader.GetName(i)))

                While Await reader.ReadAsync()
                    Try
                        Dim item As New T()
                        For Each prop In properties
                            If prop.CanWrite Then
                                Dim columnName = prop.Name

                                Try
                                    Dim columnAttr = prop.GetCustomAttributes(GetType(ColumnAttribute), False).FirstOrDefault()
                                    If columnAttr IsNot Nothing Then
                                        columnName = CType(columnAttr, ColumnAttribute).Name
                                    End If
                                    If columnNames.Contains(columnName) AndAlso Not reader.IsDBNull(reader.GetOrdinal(columnName)) Then
                                        Dim valueFromDb = reader(columnName)
                                        prop.SetValue(item, valueFromDb)
                                    Else
                                        prop.SetValue(item, Nothing)
                                    End If
                                Catch ex As Exception
                                    Dim msg = $"Error parsing column {columnName}. {ex.Message}"
                                    Console.WriteLine(msg)
                                    Throw New Exception(msg, ex)
                                End Try
                            End If
                        Next
                        items.Add(item)
                    Catch ex As Exception
                        Dim msg = $"Error parsing row, the field does not exist in the object {GetType(T).Name}"
                        Console.WriteLine(msg)
                        Throw New Exception(msg, ex)
                    End Try
                End While
            End Using
        End Using

        Return items
    End Function

End Module
