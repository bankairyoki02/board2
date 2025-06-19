Imports System.ComponentModel.DataAnnotations.Schema

<Table("PURCHASE_ORDER", Schema:="IFS")>
Public Class IFSPurchaseOrder
    <Column("ADDRESS1")>
    Public Property Address1 As String

    <Column("ADDRESS2")>
    Public Property Address2 As String

    <Column("ADDRESS3")>
    Public Property Address3 As String

    <Column("ADDRESS4")>
    Public Property Address4 As String

    <Column("ADDRESS5")>
    Public Property Address5 As String

    <Column("ADDRESS6")>
    Public Property Address6 As String

    <Column("ADDR_FLAG")>
    Public Property AddressFlag As String

    <Column("ADDR_FLAG_DB")>
    Public Property AddressFlagDb As String

    <Column("ADDR_NO")>
    Public Property AddressNo As String

    <Column("ADDR_STATE")>
    Public Property AddressState As String

    <Column("ADHOC_PURCHASED")>
    Public Property AdhocPurchased As String

    <Column("ADHOC_PURCHASED_DB")>
    Public Property AdhocPurchasedDb As String

    <Column("APPROVAL_RULE")>
    Public Property ApprovalRule As String

    <Column("AUTHORIZATION_REJECTED")>
    Public Property AuthorizationRejected As String

    <Column("AUTHORIZATION_REJECTED_DB")>
    Public Property AuthorizationRejectedDb As String

    <Column("AUTHORIZE_CODE")>
    Public Property AuthorizeCode As String

    <Column("AUTHORIZE_ID")>
    Public Property AuthorizeId As String

    <Column("BLANKET_DATE")>
    Public Property BlanketDate As String

    <Column("BLANKET_DATE_DB")>
    Public Property BlanketDateDb As String

    <Column("BUSINESS_TRANSACTION_ID")>
    Public Property BusinessTransactionId As String

    <Column("BUYER_CODE")>
    Public Property BuyerCode As String

    <Column("CANCEL_REASON")>
    Public Property CancelReason As String

    <Column("CASE_ID")>
    Public Property CaseId As Integer?

    <Column("CENTRALIZED_ORDER_SITE")>
    Public Property CentralizedOrderSite As String

    <Column("CENTRALIZED_ORDER_SITE_DB")>
    Public Property CentralizedOrderSiteDb As String

    <Column("CENTRAL_ORDER_FLAG")>
    Public Property CentralOrderFlag As String

    <Column("CENTRAL_ORDER_FLAG_DB")>
    Public Property CentralOrderFlagDb As String

    <Column("CHANGES_COMMUNICATED_VIA")>
    Public Property ChangesCommunicatedVia As String

    <Column("CHANGES_COMMUNICATED_VIA_DB")>
    Public Property ChangesCommunicatedViaDb As String

    <Column("CHANGE_DATE")>
    Public Property ChangeDate As Date?

    <Column("CITY")>
    Public Property City As String

    <Column("COMMUNICATED")>
    Public Property Communicated As String

    <Column("COMMUNICATED_DB")>
    Public Property CommunicatedDb As String

    <Column("COMMUNICATED_VIA")>
    Public Property CommunicatedVia As String

    <Column("COMMUNICATED_VIA_DB")>
    Public Property CommunicatedViaDb As String

    <Column("COMPANY")>
    Public Property Company As String

    <Column("CONFIRM_WITH_DIFFERENCES")>
    Public Property ConfirmWithDifferences As String

    <Column("CONSOLIDATED_FLAG")>
    Public Property ConsolidatedFlag As String

    <Column("CONSOLIDATED_FLAG_DB")>
    Public Property ConsolidatedFlagDb As String

    <Column("CONTACT")>
    Public Property Contact As String

    <Column("CONTRACT")>
    Public Property Contract As String

    <Column("COPY_ORDER")>
    Public Property CopyOrder As String

    <Column("COUNTRY_CODE")>
    Public Property CountryCode As String

    <Column("COUNTY")>
    Public Property County As String

    <Column("CURRENCY_CODE")>
    Public Property CurrencyCode As String

    <Column("CUSTOMER_PO_NO")>
    Public Property CustomerPoNo As String

    <Column("DATE_ENTERED")>
    Public Property DateEntered As Date?

    <Column("DELIVERY_ADDRESS")>
    Public Property DeliveryAddress As String

    <Column("DELIVERY_TERMS")>
    Public Property DeliveryTerms As String

    <Column("DEL_TERMS_LOCATION")>
    Public Property DeliveryTermsLocation As String

    <Column("DESTINATION_ID")>
    Public Property DestinationId As String

    <Column("DOCUMENT_ADDRESS_ID")>
    Public Property DocumentAddressId As String

    <Column("DOC_ADDR_NO")>
    Public Property DocumentAddressNo As String

    <Column("EXT_TRANSPORT_CALENDAR_ID")>
    Public Property ExtTransportCalendarId As String

    <Column("FORWARDER_ID")>
    Public Property ForwarderId As String

    <Column("INTERNAL_DESTINATION")>
    Public Property InternalDestination As String

    <Column("INTRASTAT_EXEMPT")>
    Public Property IntrastatExempt As String

    <Column("INTRASTAT_EXEMPT_DB")>
    Public Property IntrastatExemptDb As String

    <Column("INVOICING_SUPPLIER")>
    Public Property InvoicingSupplier As String

    <Column("LABEL_NOTE")>
    Public Property LabelNote As String

    <Column("LANGUAGE_CODE")>
    Public Property LanguageCode As String

    <Column("NOTE_ID")>
    Public Property NoteId As Integer?

    <Column("NOTE_TEXT")>
    Public Property NoteText As String

    <Column("OBJEVENTS")>
    Public Property ObjEvents As String

    <Column("OBJID")>
    Public Property ObjId As Guid?

    <Column("OBJKEY")>
    Public Property ObjKey As String

    <Column("OBJSTATE")>
    Public Property ObjState As String

    <Column("OBJVERSION")>
    Public Property ObjVersion As Integer?

    <Column("ORDCHG_SEQUENCE_NO")>
    Public Property OrdchgSequenceNo As Integer?

    <Column("ORDCHG_VERSION_NO")>
    Public Property OrdchgVersionNo As Integer?

    <Column("ORDERS_SEQUENCE_NO")>
    Public Property OrdersSequenceNo As Integer?

    <Column("ORDERS_VERSION_NO")>
    Public Property OrdersVersionNo As Integer?

    <Column("ORDER_CODE")>
    Public Property OrderCode As String

    <Column("ORDER_DATE")>
    Public Property OrderDate As Date?

    <Column("ORDER_NO")>
    Public Property OrderNo As String

    <Column("PARTY_TYPE_SUPPLIER")>
    Public Property PartyTypeSupplier As String

    <Column("PAY_TERM_ID")>
    Public Property PayTermId As String

    <Column("PENDING_CHANGES")>
    Public Property PendingChanges As String

    <Column("PENDING_CHANGES_DB")>
    Public Property PendingChangesDb As String

    <Column("PICK_LIST_FLAG")>
    Public Property PickListFlag As String

    <Column("PICK_LIST_FLAG_DB")>
    Public Property PickListFlagDb As String

    <Column("PRE_ACCOUNTING_ID")>
    Public Property PreAccountingId As Integer?

    <Column("PROJECT_ADDRESS_FLAG")>
    Public Property ProjectAddressFlag As String

    <Column("PROJECT_ADDRESS_FLAG_DB")>
    Public Property ProjectAddressFlagDb As String

    <Column("PROJECT_ADDRESS_ID")>
    Public Property ProjectAddressId As String

    <Column("PROJECT_ID")>
    Public Property ProjectId As String

    <Column("PURCHASE_CODE")>
    Public Property PurchaseCode As String

    <Column("RECALC_PRICES_TO_NEW_CURR")>
    Public Property RecalcPricesToNewCurr As String

    <Column("RECIPIENT_NAME")>
    Public Property RecipientName As String

    <Column("REFERENCE")>
    Public Property Reference As String

    <Column("REJECTED_BY")>
    Public Property RejectedBy As String

    <Column("REJECTED_DATE")>
    Public Property RejectedDate As Date?

    <Column("REJECT_REASON_ID")>
    Public Property RejectReasonId As String

    <Column("REQUISITION_NO")>
    Public Property RequisitionNo As String

    <Column("REVISION")>
    Public Property Revision As Integer?

    <Column("ROUTE_ID")>
    Public Property RouteId As String

    <Column("SCHEDULE_AGREEMENT_ORDER")>
    Public Property ScheduleAgreementOrder As String

    <Column("SCHEDULE_AGREEMENT_ORDER_DB")>
    Public Property ScheduleAgreementOrderDb As String

    <Column("SERVER_DATA_CHANGE")>
    Public Property ServerDataChange As Integer?

    <Column("SHIP_VIA_CODE")>
    Public Property ShipViaCode As String

    <Column("SOURCE_ORDER")>
    Public Property SourceOrder As String

    <Column("STATE")>
    Public Property State As String

    <Column("TASK_ID")>
    Public Property TaskId As Integer?

    <Column("TAX_LIABILITY")>
    Public Property TaxLiability As String

    <Column("TEMPLATE_ID")>
    Public Property TemplateId As String

    <Column("TOTAL_AMT_AT_AUTH")>
    Public Property TotalAmtAtAuth As Integer?

    <Column("USE_DELIVERY_DOC_ADDRESS")>
    Public Property UseDeliveryDocAddress As Boolean?

    <Column("USE_PRICE_INCL_TAX")>
    Public Property UsePriceInclTax As String

    <Column("USE_PRICE_INCL_TAX_DB")>
    Public Property UsePriceInclTaxDb As String

    <Column("VENDOR_CO_NO")>
    Public Property VendorCoNo As String

    <Column("VENDOR_NO")>
    Public Property VendorNo As String

    <Column("WANTED_RECEIPT_DATE")>
    Public Property WantedReceiptDate As Date?

    <Column("ZIP_CODE")>
    Public Property ZipCode As String
End Class
