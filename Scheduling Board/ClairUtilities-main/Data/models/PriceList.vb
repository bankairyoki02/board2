Public Class PriceList
    Public Property Luname As String
    Public Property Contract As String
    Public Property PartNo As String
    Public Property VendorNo As String
    Public Property LineNo As Integer
    Public Property MinQty As Integer
    Public Property ValidFrom As Date?

    Public Property CurrencyCode As String
    Public Property ValidUntil As Date?
    Public Property QuotePrice As Decimal
    Public Property Discount As Decimal
    Public Property AdditionalCostAmount As Decimal
    Public Property DateCreated As DateTime?
    Public Property QuotePriceInclTax As Decimal
    Public Property AdditionalCostInclTax As Decimal
    Public Property RentalDb As String

    Public ReadOnly Property QuotePriceInclTaxRounded() As Decimal
        Get
            Dim amount = Math.Round(QuotePriceInclTax, 2)
            Return amount
        End Get
    End Property

    Public ReadOnly Property QuotePriceRounded() As Decimal
        Get
            Dim amount = Math.Round(QuotePrice, 2)
            Return amount
        End Get
    End Property

    Private _Currentcurrency As Decimal
    Public Property Currentcurrency() As Decimal
        Get
            Return Math.Round(_Currentcurrency, 2)
        End Get
        Set(ByVal value As Decimal)
            _Currentcurrency = value
        End Set
    End Property
End Class
