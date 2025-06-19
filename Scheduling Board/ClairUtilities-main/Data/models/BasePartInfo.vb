Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema

Public Class BasePartInfo
    <Column("PART_NO")>
    <DisplayName("Part No")>
    Public Property PartNo As String

    <Column("DESCRIPTION")>
    <DisplayName("Part Description")>
    Public Property Description As String
End Class
