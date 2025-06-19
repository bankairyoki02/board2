Imports System.ComponentModel.DataAnnotations.Schema

Public Class FinessePart
    <Column("partno")>
    Public Property PartNo As String
    <Column("partdesc")>
    Public Property Description As String
    <Column("commmodity")>
    Public Property Category As String
    <Column("GUID")>
    Public Property GUID As Guid

    <Column("Owner")>
    Public Property Owner As String
End Class
