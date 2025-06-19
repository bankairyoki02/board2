Imports System.Data.SqlClient
Imports System.Text

Module Lame
    Public ReadOnly Property LAMEURL As String
        Get
            Dim sqlStr As New StringBuilder

            sqlStr.AppendLine("select value")
            sqlStr.AppendLine("from dbo.SysConfig sc")
            sqlStr.AppendLine("where tag = 'PODIO_PUBLIC_ISSUE_TRACKING_URL'")

            Using conn As New SqlConnection(FinesseConnectionString)
                conn.Open()
                LAMEURL = conn.ExecuteScalar(sqlStr)
            End Using
        End Get
    End Property

End Module
