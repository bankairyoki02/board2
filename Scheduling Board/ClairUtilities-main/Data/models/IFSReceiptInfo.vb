Imports System.ComponentModel.DataAnnotations.Schema

<Table("RECEIPT_INFO", Schema:="IFS")>
Public Class IFSReceiptInfo

    <Column("AIRWAY_BILL_NO")>
    Public Property AirwayBillNo As String

    <Column("APPROVED_DATE")>
    Public Property ApprovedDate As Date?

    <Column("ARRIVAL_DATE")>
    Public Property ArrivalDate As Date?

    <Column("CATCH_QTY_ARRIVED")>
    Public Property CatchQtyArrived As Double?

    <Column("CONTRACT")>
    Public Property Contract As String

    <Column("CONV_FACTOR")>
    Public Property ConvFactor As Double?

    <Column("COUNTRY_OF_ORIGIN")>
    Public Property CountryOfOrigin As String

    <Column("CUSTOMS_DECLARATION_DATE")>
    Public Property CustomsDeclarationDate As Date?

    <Column("CUSTOMS_DECLARATION_NO")>
    Public Property CustomsDeclarationNo As String

    <Column("DELIVERY_DATE")>
    Public Property DeliveryDate As Date?

    <Column("DESCRIPTION")>
    Public Property Description As String

    <Column("DUAL_RECEIPT_SEQUENCE")>
    Public Property DualReceiptSequence As Double?

    <Column("INVENTORY_PART")>
    Public Property InventoryPart As String

    <Column("INVENTORY_PART_DB")>
    Public Property InventoryPartDb As String

    <Column("INV_PART_NO")>
    Public Property InvPartNo As String

    <Column("INV_QTY_ARRIVED")>
    Public Property InvQtyArrived As Double?

    <Column("INV_UNIT_MEAS")>
    Public Property InvUnitMeas As String

    <Column("NOTE_ID")>
    Public Property NoteId As Integer?

    <Column("NOTE_TEXT")>
    Public Property NoteText As String

    <Column("NO_OF_INSPECTIONS")>
    Public Property NoOfInspections As Double?

    <Column("OBJEVENTS")>
    Public Property ObjEvents As String

    <Column("OBJID")>
    Public Property ObjId As String

    <Column("OBJKEY")>
    Public Property ObjKey As String

    <Column("OBJSTATE")>
    Public Property ObjState As String

    <Column("OBJVERSION")>
    Public Property ObjVersion As String

    <Column("PRINTED_ARRIVAL_FLAG")>
    Public Property PrintedArrivalFlag As String

    <Column("PRINTED_ARRIVAL_FLAG_DB")>
    Public Property PrintedArrivalFlagDb As String

    <Column("PRINTED_RETURN_FLAG")>
    Public Property PrintedReturnFlag As String

    <Column("PRINTED_RETURN_FLAG_DB")>
    Public Property PrintedReturnFlagDb As String

    <Column("QC_CODE")>
    Public Property QcCode As String

    <Column("RECEIPT_NO")>
    Public Property ReceiptNo As Integer?

    <Column("RECEIPT_REFERENCE")>
    Public Property ReceiptReference As String

    <Column("RECEIPT_SEQUENCE")>
    Public Property ReceiptSequence As Double?

    <Column("RECEIVED_BY")>
    Public Property ReceivedBy As String

    <Column("RECEIVE_CASE")>
    Public Property ReceiveCase As String

    <Column("RECEIVE_CASE_DB")>
    Public Property ReceiveCaseDb As String

    <Column("SENDER_ID")>
    Public Property SenderId As String

    <Column("SENDER_TYPE")>
    Public Property SenderType As String

    <Column("SENDER_TYPE_DB")>
    Public Property SenderTypeDb As String

    <Column("SOURCE_PART_NO")>
    Public Property SourcePartNo As String

    <Column("SOURCE_QTY_ARRIVED")>
    Public Property SourceQtyArrived As Double?

    <Column("SOURCE_QTY_INSPECTED")>
    Public Property SourceQtyInspected As Double?

    <Column("SOURCE_QTY_TO_INSPECT")>
    Public Property SourceQtyToInspect As Double?

    <Column("SOURCE_REF1")>
    Public Property SourceRef1 As String

    <Column("SOURCE_REF2")>
    Public Property SourceRef2 As String

    <Column("SOURCE_REF3")>
    Public Property SourceRef3 As String

    <Column("SOURCE_REF4")>
    Public Property SourceRef4 As String

    <Column("SOURCE_REF_TYPE")>
    Public Property SourceRefType As String

    <Column("SOURCE_REF_TYPE_DB")>
    Public Property SourceRefTypeDb As String

    <Column("SOURCE_UNIT_MEAS")>
    Public Property SourceUnitMeas As String

    <Column("STATE")>
    Public Property State As String

    <Column("UPDATE_FROM_CLIENT")>
    Public Property UpdateFromClient As String

    <Column("UPDATE_FROM_CLIENT_DB")>
    Public Property UpdateFromClientDb As String

End Class
