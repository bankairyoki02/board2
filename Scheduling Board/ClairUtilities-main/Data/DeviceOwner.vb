Imports System.Data.SqlClient

Public Class DeviceOwner

#Region "Properties"
    Public Property OwnerCode As String
    Public Property OwnerDesc As String
    Public Property CompanyCode As String
    Public Property AllowAddNewEquipment As Boolean
    Public Property IncludeInJobCosting As Boolean
    Public Property DoIntercompanyBilling As Boolean
    Public Property IsVisible As Boolean
    Public Property EnableZebraPrinting As Boolean
#End Region

    Public Function Getall() As List(Of DeviceOwner)
        Dim result As List(Of DeviceOwner)
        Dim dataTable As New DataTable()
        Using conn = New SqlConnection(FinesseConnectionString)
            conn.Open()
            Dim cmd = New SqlCommand("
                                        SELECT [OwnerCode]
                                              ,[OwnerDesc]
                                              ,[CompanyCode]
                                              ,[AllowAddNewEquipment]
                                              ,[IncludeInJobCosting]
                                              ,[DoIntercompanyBilling]
                                              ,[IsVisible]
                                              ,[EnableZebraPrinting]
                                          FROM [dbo].[EquipmentOwner]
                                        ", conn)
            Using reader As SqlDataReader = cmd.ExecuteReader()
                dataTable.Load(reader)
            End Using
        End Using
        result = dataTable.ToListOfObject(Of DeviceOwner)

        Return result
    End Function
End Class
