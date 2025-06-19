Imports System.Data.SqlClient
Module FinesseProjectUtilities
    <System.Runtime.CompilerServices.Extension()>
    Public Function GetParentProjectNumber(ByVal entityno As String) As String
        Dim dashIndex As Integer = entityno.LastIndexOf("-"c)
        If dashIndex >= 0 Then
            Return entityno.Substring(0, dashIndex)
        Else
            Return String.Empty
        End If
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetRootProjectNumber(ByVal entityno As String) As String
        Dim dashIndex = entityno.IndexOf("-"c)
        If dashIndex = -1 Then
            Return entityno
        Else
            Return entityno.Substring(0, dashIndex)
        End If
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function EntitynoSuffix(ByVal entityno As String) As String
        Return entityno.Substring(("-" & entityno).LastIndexOf("-"c))
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetProjectDescription(ByVal ProjectNumber As String, ByVal connection As SQLConnection) As String

        Dim sqlStr As String = "select entitydesc from glentities where entityno = " & ProjectNumber.SQLQuote

        Try
            GetProjectDescription = ReplaceNull(connection.ExecuteScalar(sqlStr), "")

        Catch ex As Exception
            MsgBox(ex.ToString(), , "Unexpected error retrieving Project Description.")
            GetProjectDescription = ""
        End Try

    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function Get_EmpnameFromEmpno(ByVal empno As String) As String
        Dim empname = String.Empty
        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))
        jammer.Add("select empname = e.firstname + ' ' + e.lastname from dbo.peemployee e where e.empno = " & empno.SQLQuote,
                   Sub(t)

                       If t.Rows.Count = 1 Then
                           empname = t.Rows(0).Item("empname")
                       End If

                   End Sub)
        jammer.Execute()
        Return empname
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function Get_ProjectDescFromProjectNo(ByVal entityno As String) As String
        Dim entitydesc = String.Empty
        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))
        jammer.Add("select g.entitydesc from dbo.glentities g where g.entityno = " & entityno.SQLQuote,
                   Sub(t)

                       If t.Rows.Count = 1 Then
                           entitydesc = t.Rows(0).Item("entitydesc")
                       End If

                   End Sub)
        jammer.Execute()
        Return entitydesc
    End Function
End Module
