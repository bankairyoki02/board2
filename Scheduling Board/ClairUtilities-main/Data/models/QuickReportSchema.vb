Imports Newtonsoft.Json

Public Class QuickReportSchema
    <JsonProperty("ORDER_NO")>
    Public Property OrderNo As String

    <JsonProperty("PART_NO")>
    Public Property PartNo As String

    <JsonProperty("DESCRIPTION")>
    Public Property Description As String

    <JsonProperty("CONTRACT")>
    Public Property Contract As String

    <JsonProperty("DESTINATION_ID")>
    Public Property DestinationId As String


    <JsonProperty("INTERNAL_DESTINATION")>
    Public Property InternalDestination As String

    <JsonProperty("LINE_NO")>
    Public Property LineNo As String

    <JsonProperty("RELEASE_NO")>
    Public Property ReleaseNo As String

    <JsonProperty("LOCATION_NO")>
    Public Property LocationNo As String

    <JsonProperty("LINE_NOTE_TEXT")>
    Public Property NoteText As String

    <JsonProperty("INVOICING_SUPPLIER")>
    Public Property SupplierID As String

    <JsonProperty("WAREHOUSE")>
    Public Property Warehouse As String

    <JsonProperty("SUPPLIER_NAME")>
    Public Property SupplierName As String

    <JsonProperty("ORIGINAL_QTY")>
    Public Property OriginalQty As String

    Public ReadOnly Property UnrecievedQty() As Integer
        Get
            If (InvQtyArrived Is Nothing OrElse FinesseQty Is Nothing) Then
                Return 0
            End If
            Return Integer.Parse(InvQtyArrived) - Integer.Parse(FinesseQty)
        End Get
    End Property

    <JsonProperty("BUY_QTY_DUE")>
    Public Property BuyQtyDue As String

    <JsonProperty("INV_QTY_ARRIVED")>
    Public Property InvQtyArrived As String

    <JsonProperty("QTY_ON_ORDER")>
    Public Property QtyOnOrder As String

    <JsonProperty("FINESSE_QTY")>
    Public Property FinesseQty As String

    <JsonProperty("DATE_ENTERED")>
    Public Property DateEntered As Date?

    <JsonProperty("APPROVED_DATE")>
    Public Property ApprovedDate As Date?

    <JsonProperty("ARRIVAL_DATE")>
    Public Property ArrivalDate As Date?

    <JsonProperty("DELIVERY_DATE")>
    Public Property DeliveryDate As Date?

    <JsonProperty("QTY_ONHAND")>
    Public Property QtyOnHand As String

    <JsonProperty("PO_STATE")>
    Public Property PoState As String

    <JsonProperty("RECEIPT_STATE")>
    Public Property ReceiptState As String

    <JsonProperty("LOCATION_TYPE")>
    Public Property LocationType As String

    <JsonProperty("BUY_UNIT_PRICE")>
    Public Property BuyUnitPrice As String

    Public ReadOnly Property BuyUnitPriceDecimal() As Decimal
        Get
            Dim amount = Math.Round(Decimal.Parse(BuyUnitPrice), 2)
            Return amount
        End Get
    End Property

    <JsonProperty("CURRENCY_RATE")>
    Public Property CurrencyRate As String


    Public ReadOnly Property IsRecieved() As String
        Get
            Dim state = If(ReceiptState, "").ToLower()
            Return state.ToLower().Contains("received")
        End Get
    End Property

    Public ReadOnly Property Owner() As String
        Get
            If (FinessePart IsNot Nothing) Then
                Return FinessePart.Owner
            End If
            Return ""
        End Get
    End Property

    Public Property FinessePart As FinessePart

    Public Property IFSInventory As IEnumerable(Of IFSInventoryStock) = New List(Of IFSInventoryStock)()


    Public ReadOnly Property InternalCustomer() As String
        Get
            If (String.IsNullOrEmpty(BuyUnitPrice) OrElse String.IsNullOrEmpty(CurrencyRate)) Then
                Return Nothing
            End If

            Dim price = Decimal.Parse(BuyUnitPrice)
            Dim ConversionRate = Decimal.Parse(CurrencyRate)
            Dim MaxAmount = 500 / ConversionRate
            Dim ActualCost = price * ConversionRate

            If (ActualCost >= MaxAmount) Then
                Return "RENT-CAPEX"
                'ElseIf (ActualCost < MaxAmount AndAlso Contract = "260") Then
                '    Return "CGP->CGCINT"
            Else
                Return "RENT-PURCH"
            End If
        End Get
    End Property

    Public ReadOnly Property FinesseDescription() As String
        Get
            If (FinessePart IsNot Nothing) Then
                Return FinessePart.Description
            End If
            Return Description
        End Get
    End Property

    Public ReadOnly Property IsValidSchema() As Boolean
        Get
            Dim result = ValidateSchema()
            Return result.Item1
        End Get
    End Property

    Public ReadOnly Property IsSerializedPo() As Boolean
        Get
            Dim result = IFSInventory.Count > 0 AndAlso IFSInventory.Where(Function(item) item.SerialNo <> "*").Any
            Return result
        End Get
    End Property



    Public ReadOnly Property SchemaError() As String
        Get
            Dim result = ValidateSchema()
            Return result.Item2
        End Get
    End Property

    Private Function ValidateSchema() As (Boolean, String)
        Dim IsValid = True
        Dim errorMsg = String.Empty

        If (String.IsNullOrEmpty(Me.DateEntered)) Then
            IsValid = False
            errorMsg += "There is an error getting the Base data of the PO."
        End If
        If (String.IsNullOrEmpty(Me.Contract)) Then
            IsValid = False
            errorMsg += "There is an error getting the Contract (Site) of the PO."
        End If
        If (String.IsNullOrEmpty(Me.LineNo)) Then
            IsValid = False
            errorMsg += "There is an error getting the Line No of the PO."
        End If
        If (String.IsNullOrEmpty(Me.ReleaseNo)) Then
            IsValid = False
            errorMsg += "There is an error getting the Release No of the PO."
        End If
        If (String.IsNullOrEmpty(Me.InternalCustomer)) Then
            IsValid = False
            errorMsg += "There is no price Or Conversion Rate set to the PO Line to calculate the Internal Customer."
        End If
        If (IFSInventory Is Nothing OrElse IFSInventory.Count = 0) Then
            IsValid = False
            errorMsg += "The current PO does not contain inventory instock"
        End If

        Return (IsValid, errorMsg)
    End Function

    Public Function GetBatchNo(SerialNo As String) As String
        SerialNo = If(String.IsNullOrEmpty(SerialNo), "*", SerialNo)
        Dim inv = IFSInventory.FirstOrDefault(Function(item) item.SerialNo = SerialNo)
        If (inv IsNot Nothing) Then
            Dim Result = If(String.IsNullOrEmpty(inv.LotBatchNo), "*", inv.LotBatchNo)
            Return Result
        End If
        Return Nothing
    End Function

End Class


Public Class QuickReportSchemaComparer
    Implements IEqualityComparer(Of QuickReportSchema)

    Public Function Equals(x As QuickReportSchema, y As QuickReportSchema) As Boolean Implements IEqualityComparer(Of QuickReportSchema).Equals
        If x Is Nothing OrElse y Is Nothing Then Return False
        Return x.OrderNo = y.OrderNo AndAlso x.PartNo = y.PartNo AndAlso x.ReleaseNo = y.ReleaseNo AndAlso x.LineNo = y.LineNo
    End Function

    Public Function GetHashCode(obj As QuickReportSchema) As Integer Implements IEqualityComparer(Of QuickReportSchema).GetHashCode
        If obj Is Nothing Then Return 0
        Return obj.OrderNo.GetHashCode() Xor obj.PartNo.GetHashCode() Xor obj.ReleaseNo.GetHashCode() Xor obj.LineNo.GetHashCode()
    End Function
End Class
