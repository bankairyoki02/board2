Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations.Schema
Imports Newtonsoft.Json

Public Class IFSPurchaseOrderSchema
    <Column("ORDER_NO")>
    <DisplayName("Order No")>
    <JsonProperty("ORDER_NO")>
    Public Property OrderNo As String

    <Column("PART_NO")>
    <DisplayName("Part No")>
    <JsonProperty("PART_NO")>
    Public Property PartNo As String

    <Column("DESCRIPTION")>
    <DisplayName("Description")>
    <JsonProperty("DESCRIPTION")>
    Public Property Description As String

    <Column("INVOICING_SUPPLIER")>
    <DisplayName("Supplier ID")>
    <JsonProperty("INVOICING_SUPPLIER")>
    Public Property SupplierID As String

    <Column("Warehouse")>
    <DisplayName("Warehouse")>
    <JsonProperty("WAREHOUSE")>
    Public Property Warehouse As String

    <Column("Owner")>
    <DisplayName("Owner")>
    Public Property Owner As String

    <Column("IsRecieved")>
    <DisplayName("IsRecieved")>
    Public Property IsRecieved As Boolean

    <Column("SUPPLIER_NAME")>
    <DisplayName("Supplier Name")>
    <JsonProperty("SUPPLIER_NAME")>
    Public Property SupplierName As String

    <Column("ORIGINAL_QTY")>
    <DisplayName("Qty")>
    <JsonProperty("ORIGINAL_QTY")>
    Public Property OriginalQty As Integer?

    Public ReadOnly Property UnrecievedQty() As Integer
        Get
            Return InvQtyArrived - FinesseQty
        End Get
    End Property

    <Column("BUY_QTY_DUE")>
    <DisplayName("Qty Due")>
    <JsonProperty("BUY_QTY_DUE")>
    Public Property BuyQtyDue As Double?

    <Column("INV_QTY_ARRIVED")>
    <DisplayName("Qty Arrived")>
    <JsonProperty("INV_QTY_ARRIVED")>
    Public Property InvQtyArrived As Integer?

    <Column("QTY_ON_ORDER")>
    <DisplayName("Qty Ordered")>
    <JsonProperty("QTY_ON_ORDER")>
    Public Property QtyOnOrder As Integer?

    <Column("DESPATCH_QTY")>
    <DisplayName("Qty Despatch")>
    Public Property DespatchQty As Integer?

    <Column("FINESSE_QTY")>
    <DisplayName("Qty In Finesse")>
    <JsonProperty("FINESSE_QTY")>
    Public Property FinesseQty As Integer?

    <Column("DATE_ENTERED")>
    <DisplayName("Date")>
    <JsonProperty("DATE_ENTERED")>
    Public Property DateEntered As DateTime?

    '<Display(AutoGenerateField:=False)>
    <Column("APPROVED_DATE")>
    <DisplayName("Approved Date")>
    <JsonProperty("APPROVED_DATE")>
    Public Property ApprovedDate As DateTime?

    <Column("ARRIVAL_DATE")>
    <DisplayName("Due Date")>
    <JsonProperty("ARRIVAL_DATE")>
    Public Property ArrivalDate As DateTime?

    <Column("DELIVERY_DATE")>
    <DisplayName("Delivery Date")>
    <JsonProperty("DELIVERY_DATE")>
    Public Property DeliveryDate As DateTime?


    <Column("QTY_ONHAND")>
    <DisplayName("Available Qty")>
    <JsonProperty("QTY_ONHAND")>
    Public Property QtyOnHand As Integer?

    <Column("IsSerializedOrder")>
    <DisplayName("Is Serialized")>
    Public Property IsSerializedOrder As Boolean?

    Private _status As String
    <Column("STATE")>
    <DisplayName("State")>
    Public Property State() As String
        Get
            If (String.IsNullOrEmpty(_status) AndAlso IsRecieved) Then
                Return "Recieved"
            End If
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property

    <Column("PO_STATE")>
    <DisplayName("PO State")>
    <JsonProperty("PO_STATE")>
    Public Property PoState As String

    <Column("RECEIPT_STATE")>
    <DisplayName("Receipt State")>
    <JsonProperty("RECEIPT_STATE")>
    Public Property ReceiptState As String


    <Column("LOCATION_TYPE")>
    <DisplayName("Location Type")>
    <JsonProperty("LOCATION_TYPE")>
    Public Property LocationType As String
End Class
