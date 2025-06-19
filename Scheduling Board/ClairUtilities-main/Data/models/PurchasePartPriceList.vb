Public Class PurchasePartPriceList


    Public Property Contract As String
    Public Property PartNo As String
    Public Property VendorNo As String
    Public Property CurrencyCode As String
    Public Property Discount As Decimal
    Public Property ListPrice As Decimal
    Public Property Company As String
    Public Property ListPriceInclTax As Decimal
    Public Property UsePriceInclTax As Boolean
    Public Property SupplierName As String
    Public Property FinesseCompany As String
    Public Property FinesseCompanyDescription As String
    Public Property PrimaryVendor As String

    Public ReadOnly Property ListPriceInclTaxRounded() As Decimal
        Get
            Dim amount = Math.Round(ListPriceInclTax, 2)
            Return amount
        End Get
    End Property

    Public ReadOnly Property ListPriceRounded() As Decimal
        Get
            Dim amount = Math.Round(ListPrice, 2)
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

    Public ReadOnly Property IsPrimarySupplier() As Boolean
        Get
            Return If(PrimaryVendor = "Yes", True, False)
        End Get
    End Property
End Class
