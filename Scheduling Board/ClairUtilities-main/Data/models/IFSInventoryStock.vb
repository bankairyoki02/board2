Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports Newtonsoft.Json

Public Class IFSInventoryStock
    <Column("ORDER_NO")>
    <DisplayName("Order No")>
    <JsonProperty("ORDER_NO")>
    Public Property OrderNo As String

    <Column("PART_NO")>
    <DisplayName("Part No")>
    <JsonProperty("PART_NO")>
    Public Property PartNo As String

    <Column("RELEASE_NO")>
    <DisplayName("Release No")>
    <JsonProperty("RELEASE_NO")>
    Public Property ReleaseNo As String

    <Column("LINE_NO")>
    <DisplayName("Line No")>
    <JsonProperty("LINE_NO")>
    Public Property LineNo As String

    <Column("DESCRIPTION")>
    <DisplayName("Part No")>
    <JsonProperty("DESCRIPTION")>
    Public Property Description As String

    <Column("SERIAL_NO")>
    <DisplayName("Serial No")>
    <JsonProperty("SERIAL_NO")>
    Public Property SerialNo As String

    <Column("FINESSE_QTY")>
    <DisplayName("Qty In Finesse")>
    <JsonProperty("FINESSE_QTY")>
    Public Property FinesseQty As Integer?

    <Column("QTY_ONHAND")>
    <DisplayName("Available Qty")>
    <JsonProperty("QTY_ONHAND")>
    Public Property AvailableQty As Integer

    <Column("INV_QTY_ARRIVED")>
    <DisplayName("Qty Arrived")>
    <JsonProperty("INV_QTY_ARRIVED")>
    Public Property InvQtyArrived As Integer?

    Public ReadOnly Property UnrecievedQty() As Integer
        Get
            Return InvQtyArrived - FinesseQty
        End Get
    End Property

    <JsonProperty("LOT_BATCH_NO")>
    Public Property LotBatchNo As String
End Class
