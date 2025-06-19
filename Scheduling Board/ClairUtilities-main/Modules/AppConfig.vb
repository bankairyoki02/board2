Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports System.Text

Public Module AppConfig
    Public Function Get_AppConfigSetting(ByVal dtAppConfig As DataTable, ByVal Tag As String)
        If dtAppConfig Is Nothing Then
            Return Nothing
        End If

        Dim result = dtAppConfig.Select("Tag=" & SQLQuote(Tag))

        If result.Length = 1 Then
            Return result(0)("Value")
        Else
            Return Nothing
        End If

    End Function

    Public Function Get_AppConfigSetting(ByVal dtAppConfig As IEnumerable(Of AppConfigState), ByVal Tag As String)
        If dtAppConfig Is Nothing Then
            Return Nothing
        End If

        Dim result = From item In dtAppConfig
                     Where item.Tag = Tag
                     Select item

        If result.Count = 1 Then
            Return result(0).Rvalue
        Else
            Return Nothing
        End If

    End Function
End Module

Public Class AppConfigState
    Private _cmdsel As String
    Public Property Cmndsel() As String
        Get
            Return _cmdsel
        End Get
        Set(ByVal value As String)
            _cmdsel = value
        End Set
    End Property

    Private _tag As String
    Public Property Tag() As String
        Get
            Return _tag
        End Get
        Set(ByVal value As String)
            _tag = value
        End Set
    End Property

    Private _rValue As String
    Public Property Rvalue() As String
        Get
            Return _rValue
        End Get
        Set(ByVal value As String)
            _rValue = value
        End Set
    End Property

    Public Function GetAppConfigState(Optional command As String = "") As List(Of AppConfigState)
        Dim result As List(Of AppConfigState)
        Dim dataTable As New DataTable()
        Using conn = New SqlConnection(FinesseConnectionString)
            conn.Open()
            Dim cmd = New SqlCommand("select c.cmndsel As Cmndsel, Tag, Value As Rvalue from App_Config c where c.cmndsel = " & If(String.IsNullOrEmpty(command), Cmndsel, command).SQLQuote, conn)
            Using reader As SqlDataReader = cmd.ExecuteReader()
                dataTable.Load(reader)
            End Using
        End Using
        result = dataTable.ToListOfObject(Of AppConfigState)

        Return result
    End Function


    Public Sub SaveConfig(list As List(Of AppConfigState))
        Dim dataTable As New DataTable()
        Try
            Using conn = New SqlConnection(FinesseConnectionString)
                conn.Open()

                Dim sSQL As New StringBuilder
                For Each item In list
                    With item
                        sSQL.AppendLine($"delete s from app_config s where cmndsel = {SQLQuote(.Cmndsel)} And tag = {SQLQuote(.Tag)};")
                        sSQL.AppendLine($"insert dbo.AppConfig(cmndsel, user_name, tag, value) values ({SQLQuote(.Cmndsel)}, SUSER_NAME(), {SQLQuote(.Tag)}, {SQLQuote(.Rvalue)});")
                    End With
                Next

                Dim cmd = New SqlCommand(sSQL.ToString(), conn)
                cmd.DebugCommand()
                cmd.ExecuteNonQuery()
                conn.Close()
                conn.Dispose()
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

    End Sub
End Class

Module AppConfigModule
    <Extension>
    Public Sub SaveConfig(ByVal list As List(Of AppConfigState))
        Dim config = New AppConfigState()
        config.SaveConfig(list)
    End Sub
End Module
