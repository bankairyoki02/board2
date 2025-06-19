Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("PART_CATALOG", Schema:="IFS")>
Public Class PartCatalog
    <Key>
    <Column("PART_NO")>
    Public Property PartNo As String

    <Column("DESCRIPTION")>
    Public Property Description As String

    <Column("INFO_TEXT")>
    Public Property InfoText As String

    <Column("STD_NAME_ID")>
    Public Property StdNameId As Decimal?

    <Column("UNIT_CODE")>
    Public Property UnitCode As String

    <Column("LOT_TRACKING_CODE")>
    Public Property LotTrackingCode As String

    <Column("SERIAL_RULE")>
    Public Property SerialRule As String

    <Column("SERIAL_TRACKING_CODE")>
    Public Property SerialTrackingCode As String

    <Column("ENG_SERIAL_TRACKING_CODE")>
    Public Property EngSerialTrackingCode As String

    <Column("PART_MAIN_GROUP")>
    Public Property PartMainGroup As String

    <Column("CONFIGURABLE")>
    Public Property Configurable As String

    <Column("CUST_WARRANTY_ID")>
    Public Property CustWarrantyId As Integer?

    <Column("SUP_WARRANTY_ID")>
    Public Property SupWarrantyId As Integer?

    <Column("CONDITION_CODE_USAGE")>
    Public Property ConditionCodeUsage As String

    <Column("SUB_LOT_RULE")>
    Public Property SubLotRule As String

    <Column("LOT_QUANTITY_RULE")>
    Public Property LotQuantityRule As String

    <Column("POSITION_PART")>
    Public Property PositionPart As String

    <Column("INPUT_UNIT_MEAS_GROUP_ID")>
    Public Property InputUnitMeasGroupId As String

    <Column("CATCH_UNIT_ENABLED")>
    Public Property CatchUnitEnabled As String

    <Column("MULTILEVEL_TRACKING")>
    Public Property MultilevelTracking As String

    <Column("COMPONENT_LOT_RULE")>
    Public Property ComponentLotRule As String

    <Column("STOP_ARRIVAL_ISSUED_SERIAL")>
    Public Property StopArrivalIssuedSerial As String

    <Column("WEIGHT_NET")>
    Public Property WeightNet As Integer?

    <Column("UOM_FOR_WEIGHT_NET")>
    Public Property UomForWeightNet As String

    <Column("VOLUME_NET")>
    Public Property VolumeNet As Integer?

    <Column("UOM_FOR_VOLUME_NET")>
    Public Property UomForVolumeNet As String

    <Column("FREIGHT_FACTOR")>
    Public Property FreightFactor As Double?

    <Column("ALLOW_AS_NOT_CONSUMED")>
    Public Property AllowAsNotConsumed As String

    <Column("RECEIPT_ISSUE_SERIAL_TRACK")>
    Public Property ReceiptIssueSerialTrack As String

    <Column("STOP_NEW_SERIAL_IN_RMA")>
    Public Property StopNewSerialInRma As String

    <Column("TECHNICAL_DRAWING_NO")>
    Public Property TechnicalDrawingNo As String

    <Column("PRODUCT_TYPE_CLASSIF")>
    Public Property ProductTypeClassif As String

    <Column("CEST_CODE")>
    Public Property CestCode As String

    <Column("FCI_CODE")>
    Public Property FciCode As String

    <Column("ROWVERSION")>
    Public Property RowVersion As String

    <Column("ROWKEY")>
    Public Property RowKey As String

End Class
