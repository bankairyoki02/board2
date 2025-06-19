Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Public Class IFSInventoryReceiptMessages
    <Column("FinesseRef")>
    <DisplayName("REF")>
    Public Property FinesseRef As Integer

    <Column("OrderNo")>
    <DisplayName("PO No")>
    Public Property OrderNumber As String

    <Column("PartNo")>
    <DisplayName("Part No")>
    Public Property PartNumber As String

    <Column("SerialNo")>
    <DisplayName("Serial No")>
    Public Property SerialNumber As String

    <Column("TransactionDate")>
    Public Property TransactionDate As DateTime

    <Column("SITE")>
    Public Property Site As String

    <Column("INTERNAL_CUSTOMER")>
    Public Property InternalCustomer As String

    <Column("INTERNAL_DESTINATION")>
    Public Property InternalDestination As String

    <Column("INTERNAL_DESTINATION_DESCRIPTION")>
    Public Property InternalDestinationDescription As String

    <Column("Line_No")>
    <DisplayName("Line No")>
    Public Property LineNumber As Integer

    <Column("Qty")>
    <DisplayName("Qty")>
    Public Property Quantity As Integer

    <Column("INVENTORY_LOCATION")>
    Public Property InventoryLocation As String

    <Column("PO_LINE")>
    <DisplayName("PO Line")>
    <Display(AutoGenerateField:=False)>
    Public Property PurchaseOrderLine As Integer

    <Column("PO_RELEASE")>
    <DisplayName("Release No")>
    Public Property PurchaseOrderRelease As Integer

    <Column("LOT_BATCH_NO")>
    Public Property LotBatchNumber As String

    <Column("CONDITION_CODE")>
    <Display(AutoGenerateField:=False)>
    Public Property ConditionCode As String

    <Column("LINE_NOTE")>
    Public Property LineNote As String

    <Column("HEADER_NOTE")>
    <Display(AutoGenerateField:=False)>
    Public Property HeaderNote As String

    <Column("IsSent")>
    Public Property IsSent As Boolean

    <Column("ERROR")>
    Public Property ErrorMessage As String
End Class

