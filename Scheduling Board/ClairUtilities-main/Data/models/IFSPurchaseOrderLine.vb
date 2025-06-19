Imports System.ComponentModel.DataAnnotations.Schema

<Table("PURCHASE_ORDER_LINE", Schema:="IFS")>
Public Class IFSPurchaseOrderLine
    <Column("ACQUISITION_ORIGIN")>
    Public Property AcquisitionOrigin As Integer?

    <Column("ACQUISITION_REASON_ID")>
    Public Property AcquisitionReasonId As String

    <Column("ACTIVITY_SEQ")>
    Public Property ActivitySeq As Integer?

    <Column("ADDITIONAL_COST_AMOUNT")>
    Public Property AdditionalCostAmount As Decimal?

    <Column("ADDITIONAL_COST_INCL_TAX")>
    Public Property AdditionalCostInclTax As Decimal?

    <Column("ADDRESS_ID")>
    Public Property AddressId As String

    <Column("ADDR_FLAG")>
    Public Property AddrFlag As String

    <Column("ADDR_FLAG_DB")>
    Public Property AddrFlagDb As String

    <Column("ASSORTMENT")>
    Public Property Assortment As String

    <Column("AUTOMATIC_INVOICE")>
    Public Property AutomaticInvoice As String

    <Column("AUTOMATIC_INVOICE_DB")>
    Public Property AutomaticInvoiceDb As String

    <Column("BLANKET_LINE")>
    Public Property BlanketLine As Integer?

    <Column("BLANKET_ORDER")>
    Public Property BlanketOrder As String

    <Column("BUSINESS_OPERATION")>
    Public Property BusinessOperation As String

    <Column("BUY_QTY_DUE")>
    Public Property BuyQtyDue As Decimal?

    <Column("BUY_UNIT_MEAS")>
    Public Property BuyUnitMeas As String

    <Column("BUY_UNIT_PRICE")>
    Public Property BuyUnitPrice As Decimal?

    <Column("BUY_UNIT_PRICE_INCL_TAX")>
    Public Property BuyUnitPriceInclTax As Decimal?

    <Column("CAMPAIGN_ID")>
    Public Property CampaignId As Integer?

    <Column("CANCEL_REASON")>
    Public Property CancelReason As String

    <Column("CATEGORY_ASSORTMENT")>
    Public Property CategoryAssortment As String

    <Column("CATEGORY_ASSORTMENT_NODE")>
    Public Property CategoryAssortmentNode As String

    <Column("CHG_ORDER_NO")>
    Public Property ChgOrderNo As String

    <Column("CLOSE_CODE")>
    Public Property CloseCode As String

    <Column("CLOSE_CODE_DB")>
    Public Property CloseCodeDb As String

    <Column("CLOSE_TOLERANCE")>
    Public Property CloseTolerance As Decimal?

    <Column("COMPANY")>
    Public Property Company As String

    <Column("CONFIRM_WITH_DIFFERENCES")>
    Public Property ConfirmWithDifferences As String

    <Column("CONTRACT")>
    Public Property Contract As String

    <Column("CONV_FACTOR")>
    Public Property ConvFactor As Decimal?

    <Column("CREATE_FA_OBJ")>
    Public Property CreateFaObj As String

    <Column("CREATE_FA_OBJ_DB")>
    Public Property CreateFaObjDb As String

    <Column("CURRENCY_CODE")>
    Public Property CurrencyCode As String

    <Column("CURRENCY_RATE")>
    Public Property CurrencyRate As Decimal?

    <Column("CURRENCY_TYPE")>
    Public Property CurrencyType As String

    <Column("CUSTOMS_STAT_NO")>
    Public Property CustomsStatNo As String

    <Column("DATE_ENTERED")>
    Public Property DateEntered As Date?

    <Column("DEFAULT_ADDR_FLAG")>
    Public Property DefaultAddrFlag As String

    <Column("DEFAULT_ADDR_FLAG_DB")>
    Public Property DefaultAddrFlagDb As String

    <Column("DEFECT_PART_KEY_REF")>
    Public Property DefectPartKeyRef As String

    <Column("DELIVERY_CONTROL_CODE")>
    Public Property DeliveryControlCode As String

    <Column("DELIVERY_REMINDER")>
    Public Property DeliveryReminder As String

    <Column("DELIVERY_REMINDER_DB")>
    Public Property DeliveryReminderDb As String

    <Column("DELIVERY_REM_NUM")>
    Public Property DeliveryRemNum As Integer?

    <Column("DELIVERY_TERMS")>
    Public Property DeliveryTerms As String

    <Column("DEL_TERMS_LOCATION")>
    Public Property DelTermsLocation As String

    <Column("DEMAND_CODE")>
    Public Property DemandCode As String

    <Column("DEMAND_CODE_DB")>
    Public Property DemandCodeDb As String

    <Column("DEMAND_OPERATION_NO")>
    Public Property DemandOperationNo As String

    <Column("DEMAND_ORDER_CODE")>
    Public Property DemandOrderCode As String

    <Column("DEMAND_ORDER_NO")>
    Public Property DemandOrderNo As String

    <Column("DEMAND_ORDER_TYPE")>
    Public Property DemandOrderType As String

    <Column("DEMAND_RELEASE")>
    Public Property DemandRelease As String

    <Column("DEMAND_SEQUENCE_NO")>
    Public Property DemandSequenceNo As String

    <Column("DESCRIPTION")>
    Public Property Description As String

    <Column("DESPATCH_QTY")>
    Public Property DespatchQty As Decimal?

    <Column("DESTINATION_ID")>
    Public Property DestinationId As String

    <Column("DESTINATION_WAREHOUSE_ID")>
    Public Property DestinationWarehouseId As String

    <Column("DISCOUNT")>
    Public Property Discount As Decimal?

    <Column("EXTERNAL_PROJECT_RESOURCE")>
    Public Property ExternalProjectResource As String

    <Column("EXTERNAL_PROJECT_RESOURCE_DB")>
    Public Property ExternalProjectResourceDb As String

    <Column("EXT_TRANSPORT_CALENDAR_ID")>
    Public Property ExtTransportCalendarId As String

    <Column("FA_OBJ_PER_UNIT")>
    Public Property FaObjPerUnit As String

    <Column("FA_OBJ_PER_UNIT_DB")>
    Public Property FaObjPerUnitDb As String

    <Column("FBUY_UNIT_PRICE")>
    Public Property FBuyUnitPrice As Decimal?

    <Column("FBUY_UNIT_PRICE_INCL_TAX")>
    Public Property FBuyUnitPriceInclTax As Decimal?

    <Column("FEE_CODE")>
    Public Property FeeCode As String

    <Column("FORWARDER_ID")>
    Public Property ForwarderId As String

    <Column("FREEZE_FLAG")>
    Public Property FreezeFlag As String

    <Column("FREEZE_FLAG_DB")>
    Public Property FreezeFlagDb As String

    <Column("HEADER_UPDATE")>
    Public Property HeaderUpdate As String

    <Column("HSN_SAC_CODE")>
    Public Property HsnSacCode As String

    <Column("IGNORE_DEFAULT_TAXES")>
    Public Property IgnoreDefaultTaxes As String

    <Column("IGNORE_DEFAULT_TAXES_DB")>
    Public Property IgnoreDefaultTaxesDb As String

    <Column("INPUT_CONV_FACTOR")>
    Public Property InputConvFactor As Decimal?

    <Column("INPUT_GTIN_NO")>
    Public Property InputGtinNo As String

    <Column("INPUT_QTY")>
    Public Property InputQty As Decimal?

    <Column("INPUT_UNIT_MEAS")>
    Public Property InputUnitMeas As String

    <Column("INSPECTION_CODE")>
    Public Property InspectionCode As String

    <Column("INTERNAL_DEST_DESC")>
    Public Property InternalDestDesc As String

    <Column("INTERNAL_INCOME_TYPE")>
    Public Property InternalIncomeType As Integer?

    <Column("INTRASTAT_AFFECTED")>
    Public Property IntrastatAffected As String

    <Column("INTRASTAT_AFFECTED_DB")>
    Public Property IntrastatAffectedDb As String

    <Column("INTRASTAT_CONV_FACTOR")>
    Public Property IntrastatConvFactor As Decimal?

    <Column("INTRASTAT_EXEMPT")>
    Public Property IntrastatExempt As String

    <Column("INTRASTAT_EXEMPT_DB")>
    Public Property IntrastatExemptDb As String

    <Column("INVENTORY_PART")>
    Public Property InventoryPart As String

    <Column("INVENTORY_PART_DB")>
    Public Property InventoryPartDb As String

    <Column("INVOICING_SUPPLIER")>
    Public Property InvoicingSupplier As String


    <Column("SUPPLIER_NAME")>
    Public Property SupplierName As String

    <Column("LAST_ACTIVITY_DATE")>
    Public Property LastActivityDate As Date?

    <Column("LAST_DELIVERY_REMINDER")>
    Public Property LastDeliveryReminder As Date?

    <Column("LAST_ORD_CONF_REMINDER")>
    Public Property LastOrdConfReminder As Date?

    <Column("LINE_NO")>
    Public Property LineNo As String

    <Column("MANUAL_TAX_BASE_CURR_AMT")>
    Public Property ManualTaxBaseCurrAmt As Decimal?

    <Column("NON_DEDUCT_TAX_AMOUNT")>
    Public Property NonDeductTaxAmount As Decimal?

    <Column("NOTE_ID")>
    Public Property NoteId As Integer?

    <Column("NOTE_TEXT")>
    Public Property NoteText As String

    <Column("OBJEVENTS")>
    Public Property ObjEvents As String

    <Column("OBJID")>
    Public Property ObjId As String

    <Column("OBJKEY")>
    Public Property ObjKey As String

    <Column("OBJSTATE")>
    Public Property ObjState As String

    <Column("OBJTYPE")>
    Public Property ObjType As String

    <Column("OBJVERSION")>
    Public Property ObjVersion As String

    <Column("ORDER_NO")>
    Public Property OrderNo As String

    <Column("ORD_CONF_REMINDER")>
    Public Property OrdConfReminder As String

    <Column("ORD_CONF_REMINDER_DB")>
    Public Property OrdConfReminderDb As String

    <Column("ORD_CONF_REM_NUM")>
    Public Property OrdConfRemNum As Integer?

    <Column("ORIGINAL_QTY")>
    Public Property OriginalQty As Decimal?

    <Column("OVER_DELIVERY_TOLERANCE")>
    Public Property OverDeliveryTolerance As Decimal?

    <Column("PART_NO")>
    Public Property PartNo As String

    <Column("PLANNED_ARRIVAL_DATE")>
    Public Property PlannedArrivalDate As Date?

    <Column("PLANNED_DELIVERY_DATE")>
    Public Property PlannedDeliveryDate As Date?

    <Column("PLANNED_RECEIPT_DATE")>
    Public Property PlannedReceiptDate As Date?

    <Column("POST_ON_PURCHASING_COMP")>
    Public Property PostOnPurchasingComp As String

    <Column("POST_ON_PURCHASING_COMP_DB")>
    Public Property PostOnPurchasingCompDb As String

    <Column("PRE_ACCOUNTING_ID")>
    Public Property PreAccountingId As Integer?

    <Column("PRICE_CONV_FACTOR")>
    Public Property PriceConvFactor As Decimal?

    <Column("PROCESS_TYPE")>
    Public Property ProcessType As String

    <Column("PROJECT_ADDRESS")>
    Public Property ProjectAddress As String

    <Column("PROJECT_ADDRESS_DB")>
    Public Property ProjectAddressDb As String

    <Column("PROJECT_ID")>
    Public Property ProjectId As String

    <Column("PROMISED_DELIVERY_DATE")>
    Public Property PromisedDeliveryDate As Date?

    <Column("PURCHASE_PAYMENT_TYPE")>
    Public Property PurchasePaymentType As String

    <Column("PURCHASE_PAYMENT_TYPE_DB")>
    Public Property PurchasePaymentTypeDb As String

    <Column("PURCHASE_SITE")>
    Public Property PurchaseSite As String

    <Column("QTY_ON_ORDER")>
    Public Property QtyOnOrder As Decimal?

    <Column("RECEIVE_CASE")>
    Public Property ReceiveCase As String

    <Column("RECEIVE_CASE_DB")>
    Public Property ReceiveCaseDb As String

    <Column("RELEASE_NO")>
    Public Property ReleaseNo As String

    <Column("RENTAL")>
    Public Property Rental As String

    <Column("RENTAL_DB")>
    Public Property RentalDb As String

    <Column("REPLACES_LINE_NO")>
    Public Property ReplacesLineNo As String

    <Column("REPLACES_ORDER_NO")>
    Public Property ReplacesOrderNo As String

    <Column("REPLACES_RECEIPT_NO")>
    Public Property ReplacesReceiptNo As Integer?

    <Column("REPLACES_RELEASE_NO")>
    Public Property ReplacesReleaseNo As String

    <Column("REQUISITION_NO")>
    Public Property RequisitionNo As String

    <Column("REQ_LINE")>
    Public Property ReqLine As String

    <Column("REQ_RELEASE")>
    Public Property ReqRelease As String

    <Column("ROUTE_ID")>
    Public Property RouteId As String

    <Column("SAMPLE_PERCENT")>
    Public Property SamplePercent As Decimal?

    <Column("SAMPLE_QTY")>
    Public Property SampleQty As Decimal?

    <Column("SERVER_DATA_CHANGE")>
    Public Property ServerDataChange As Integer?

    <Column("SHIP_VIA_CODE")>
    Public Property ShipViaCode As String

    <Column("SKIP_AUTH_VALIDATION")>
    Public Property SkipAuthValidation As String

    <Column("STATE")>
    Public Property State As String

    <Column("STATISTICAL_CODE")>
    Public Property StatisticalCode As String

    <Column("TAXABLE")>
    Public Property Taxable As String

    <Column("TAXABLE_DB")>
    Public Property TaxableDb As String

    <Column("TAX_AMOUNT")>
    Public Property TaxAmount As Decimal?

    <Column("TAX_AMOUNT_BASE")>
    Public Property TaxAmountBase As Decimal?

    <Column("TAX_CALC_STRUCTURE_ID")>
    Public Property TaxCalcStructureId As String

    <Column("TAX_LIABILITY")>
    Public Property TaxLiability As String

    <Column("TAX_LIABILITY_TYPE")>
    Public Property TaxLiabilityType As String

    <Column("TAX_LIABILITY_TYPE_DB")>
    Public Property TaxLiabilityTypeDb As String

    <Column("TECHNICAL_COORDINATOR_ID")>
    Public Property TechnicalCoordinatorId As String

    <Column("VENDOR_PART_DESCRIPTION")>
    Public Property VendorPartDescription As String

    <Column("VENDOR_PART_NO")>
    Public Property VendorPartNo As String

    <Column("WANTED_DELIVERY_DATE")>
    Public Property WantedDeliveryRate As String

    <Column("WEIGHT_NET")>
    Public Property WeightNet As String

    <Column("WORK_TASK_SEQ")>
    Public Property WorkTaskSeq As String

    Private _Currentcurrency As Decimal
    Public Property Currentcurrency() As Decimal
        Get
            Return Math.Round(_Currentcurrency, 2)
        End Get
        Set(ByVal value As Decimal)
            _Currentcurrency = value
        End Set
    End Property

    Public ReadOnly Property BuyUnitPriceInclTaxRounded() As Decimal
        Get
            If (BuyUnitPriceInclTax Is Nothing) Then
                Return 0
            End If
            Dim amount = Math.Round(Decimal.Round(BuyUnitPriceInclTax), 2)
            Return amount
        End Get
    End Property

    Public ReadOnly Property BuyUnitPriceRounded() As Decimal
        Get
            If (BuyUnitPrice Is Nothing) Then
                Return 0
            End If
            Dim amount = Math.Round(Decimal.Round(BuyUnitPrice), 2)
            Return amount
        End Get
    End Property
End Class


