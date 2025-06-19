Imports System.Data.SqlClient
Imports System.Diagnostics.Contracts
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Reflection.Emit
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel
Imports System.Windows.Markup
Imports System.Windows.Media
Imports System.Windows.Shapes
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Module IFSDAL

#Region "Properties"

    Private ReadOnly ApiRequestTimeOut As Integer = 10

    Private _environmentConfig As EnvironmentConfig
    Private Property EnvironmentCredentials() As EnvironmentConfig
        Get
            If (_environmentConfig Is Nothing) Then
                Initialize()
                'Throw New NullReferenceException("The environment credentials have not been initialized. Call Initialize().")
            End If

            Return _environmentConfig
        End Get
        Set(ByVal value As EnvironmentConfig)
            _environmentConfig = value
        End Set
    End Property

    Public ReadOnly Property Environment() As String
        Get
            Dim database = System.Environment.GetEnvironmentVariable("ESSDB")
            Return If(database.ToLower.Contains("test"), "uat", "prd")
        End Get
    End Property

    Public BaseUrl As String

    Public Sub Initialize()
        Dim credentials = New IFSCredentialManager().GetCredentials()
        Dim cred = credentials.environments(Environment)
        BaseUrl = credentials.baseUrl
        EnvironmentCredentials = cred
    End Sub

    Private ReadOnly Property AuthenticationBody() As Dictionary(Of String, String)
        Get
            Dim userid = "FINESSE_PO_READER"

            Return New Dictionary(Of String, String) From {
                                             {"client_id", userid},
                                             {"client_secret", EnvironmentCredentials.secrets(userid)},
                                             {"grant_type", "client_credentials"}
                                         }
        End Get
    End Property

#End Region


#Region "API Calls"
    '''' <summary>
    '''' Get the IFS Token for accessing the quick Report data
    '''' </summary>
    '''' <returns></returns>
    'Private Async Function GetIFSQuickReportToken() As Task(Of String)

    '    Try

    '        Using client As New HttpClient()
    '            client.BaseAddress = New Uri(EnvironmentCredentials.domain)
    '            client.Timeout = TimeSpan.FromSeconds(ApiRequestTimeOut)
    '            client.DefaultRequestHeaders.Add("Accept", "application/json")
    '            Dim content As New FormUrlEncodedContent(AuthenticationBody)
    '            Dim response As HttpResponseMessage = Await client.PostAsync(EnvironmentCredentials.token_url, content)

    '            If response.IsSuccessStatusCode Then
    '                Dim responseBody As String = Await response.Content.ReadAsStringAsync()
    '                Dim jsonResponse As JObject = JObject.Parse(responseBody)
    '                Return jsonResponse("access_token").ToString()
    '            Else
    '                Throw New Exception($"Unable to authorize the IFS user. {response.ReasonPhrase}")
    '            End If


    '        End Using
    '    Catch ex As TaskCanceledException
    '        Throw New Exception($"Operation Timed out getting Token from API")
    '    Catch ex As Exception
    '        Console.WriteLine(ex.Message)
    '        Throw ex
    '    End Try
    'End Function


    ''' <summary>
    ''' Generate the data from the quick reports to get Purchase Order Lines and Inventory part in Stock form IFS
    ''' Refrence to quick report: https://clair-uat.ifs.cloud/main/ifsapplications/web/page/QuickReport/Form;$filter=QuickReportId%20eq%20527167;path=0.1656053651.1494445447.169839528.786554908;record=KFF1aWNrUmVwb3J0SWQ9NTI3MTY3KQ%3D%3D
    ''' </summary>
    ''' <returns></returns>
    Public Async Function GetPOLinesQuickReport() As Task(Of IEnumerable(Of QuickReportSchema))
        Try
            Dim ifsClient = New IFSClient()
            Dim token = Await ifsClient.GetToken()
            Dim poLinesRequest = New IFSRequest() With {
            .Uri = "QuickReports.svc/QuickReport_527167()",
            .Token = token
            }

            Dim LineTask = ifsClient.GetAsync(Of QuickReportSchema)(poLinesRequest)

            Dim inventoryRequest = New IFSRequest() With {
            .Uri = "QuickReports.svc/QuickReport_586864()",
            .Token = token
            }
            Dim InventoryTask = ifsClient.GetAsync(Of IFSInventoryStock)(inventoryRequest)
            Await Task.WhenAll(LineTask, InventoryTask)
            Dim poLineResponse = LineTask.Result
            Dim inventoryResponse = InventoryTask.Result
            If poLineResponse IsNot Nothing AndAlso poLineResponse.Value.Count > 0 Then
                Dim tasks As New List(Of Task)
                Using conn = GetOpenedFinesseConnection()
                    For index = 0 To poLineResponse.Value.Count - 1
                        Dim po = poLineResponse.Value(index)
                        Dim currentIndex = index
                        tasks.Add(Task.Run(Async Function()
                                               If (Not String.IsNullOrEmpty(po.PartNo)) Then
                                                   Dim finessePart = Await GetPartInfo(po.PartNo, po.Contract, conn)
                                                   If finessePart IsNot Nothing Then
                                                       Dim orderInventory = inventoryResponse.Value.Where(Function(inv) inv.OrderNo = po.OrderNo AndAlso inv.PartNo = po.PartNo)
                                                       poLineResponse.Value(currentIndex).FinessePart = finessePart
                                                       If (orderInventory IsNot Nothing) Then
                                                           poLineResponse.Value(currentIndex).IFSInventory = orderInventory.ToList()
                                                       End If
                                                   End If
                                               End If
                                           End Function))
                    Next
                    Await Task.WhenAll(tasks)
                End Using
            End If
            Return poLineResponse.Value

            'Dim client As HttpClient = Await GetRequestHeder()
            'Using client
            '    Dim LineTask = client.GetAsync($"{BaseUrl}QuickReports.svc/QuickReport_527167()")
            '    Dim InventoryTask = client.GetAsync($"{BaseUrl}QuickReports.svc/QuickReport_586864()")

            '    Await Task.WhenAll(LineTask, InventoryTask)

            '    Dim LineResponse As HttpResponseMessage = Await LineTask
            '    Dim InventoryResponse As HttpResponseMessage = Await InventoryTask

            '    If LineResponse.IsSuccessStatusCode Then
            '        Dim responseBody As String = Await LineResponse.Content.ReadAsStringAsync()
            '        Dim cleanedJson As String = SanitizeIfsJson(responseBody)
            '        Dim jsonResponse = JsonConvert.DeserializeObject(Of ODataResult(Of QuickReportSchema))(cleanedJson)

            '        Dim InventoryresponseBody As String = Await InventoryResponse.Content.ReadAsStringAsync()
            '        Dim InventorycleanedJson As String = SanitizeIfsJson(InventoryresponseBody)
            '        Dim InventoryjsonResponse = JsonConvert.DeserializeObject(Of ODataResult(Of IFSInventoryStock))(InventorycleanedJson)


            '        If jsonResponse IsNot Nothing AndAlso jsonResponse.Value.Count > 0 Then
            '            Dim tasks As New List(Of Task)
            '            Using conn = GetOpenedFinesseConnection()
            '                For index = 0 To jsonResponse.Value.Count - 1
            '                    Dim po = jsonResponse.Value(index)
            '                    Dim currentIndex = index
            '                    tasks.Add(Task.Run(Async Function()
            '                                           If (Not String.IsNullOrEmpty(po.PartNo)) Then
            '                                               Dim finessePart = Await GetPartInfo(po.PartNo, po.Contract, conn)
            '                                               If finessePart IsNot Nothing Then
            '                                                   Dim orderInventory = InventoryjsonResponse.Value.Where(Function(inv) inv.OrderNo = po.OrderNo AndAlso inv.PartNo = po.PartNo)
            '                                                   jsonResponse.Value(currentIndex).FinessePart = finessePart
            '                                                   If (orderInventory IsNot Nothing) Then
            '                                                       jsonResponse.Value(currentIndex).IFSInventory = orderInventory.ToList()
            '                                                   End If
            '                                               End If
            '                                           End If
            '                                       End Function))
            '                Next
            '                Await Task.WhenAll(tasks)
            '            End Using
            '        End If
            '        Return jsonResponse.Value
            '    Else
            '        Throw New Exception($"Unable to fetch IFS Data from the API. Reason: {vbNewLine} {LineResponse.ReasonPhrase}")
            '    End If

            'End Using
        Catch ex As TaskCanceledException
            Throw New Exception($"Operation Timed out getting PO Lines from API")
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw ex
        End Try
    End Function


    Private Function GetExchangeRateFromBook(dt As DataTable, currency As String)
        Dim ExchangeRateBook = From r In dt
                               Where r.Item("currency") = currency
                               Select r

        Return ExchangeRateBook(0).Item("ratetodollars")
    End Function

    Public Async Function GetSupplierForPurchasePartPartPriceList(PartNumber As String, Optional Companies As DataTable = Nothing, Optional ExchangeRateToCurrency As CurrencyRate = Nothing, Optional token As String = Nothing) As Task(Of IEnumerable(Of PurchasePartPriceList))
        Try
            Dim ifsClient = New IFSClient()
            Dim requestContent = New IFSRequest() With {
            .Uri = "SupplierForPurchasePartsHandling.svc/PurchasePartSupplierSet",
            .FilterStatement = $"(((PartNo eq '{PartNumber}')) and ListPrice ne 0)",
            .SelectStatement = "PartNo,Contract,VendorNo,SupplierName,Company,UsePriceInclTax,ListPrice,ListPriceInclTax,CurrencyCode,Discount,PrimaryVendor",
            .Token = token
            }
            Dim result = Await ifsClient.GetAsync(Of PurchasePartPriceList)(requestContent)
            Dim priceList = result.Value

            If (Companies IsNot Nothing) Then
                For Each item In PriceList
                    Dim Company = Companies.AsEnumerable().FirstOrDefault(Function(comp) comp.Field(Of String)("IFSCompanyCd") = item.Company)
                    If (Company IsNot Nothing) Then
                        item.FinesseCompany = Company("CompanyCode")
                        item.FinesseCompanyDescription = Company("CompanyDesc")
                        If (ExchangeRateToCurrency IsNot Nothing) Then

                            Dim price = item.ListPrice
                            If (ExchangeRateToCurrency.CurrencyCode <> item.CurrencyCode AndAlso ExchangeRateToCurrency.ExchangeRateBook IsNot Nothing) Then
                                price = CalculatePriceToCurrency(ExchangeRateToCurrency, item.CurrencyCode, price)
                            End If
                            item.Currentcurrency = price
                        End If
                    End If
                Next
            End If
            Return priceList
        Catch ex As TaskCanceledException
            Throw New Exception($"Operation Timed out getting Price List from API")
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw ex
        End Try

    End Function

    Public Async Function GetPartPriceList(PartNumber As String, Optional Companies As DataTable = Nothing, Optional ExchangeRateToCurrency As CurrencyRate = Nothing, Optional token As String = Nothing) As Task(Of IEnumerable(Of PriceList))
        Try
            Dim ifsClient = New IFSClient()
            Dim requestContent = New IFSRequest() With {
            .Uri = "SupplierForPurchasePartPriceListLinesHandling.svc/PurchaseQuantityPriceList",
            .FilterStatement = $"PartNo eq '{PartNumber}'",
            .SelectStatement = "VendorNo,PartNo,Contract,LineNo,MinQty,ValidFrom,ValidUntil,QuotePrice,QuotePriceInclTax,CurrencyCode,Discount,AdditionalCostAmount,AdditionalCostInclTax,PriceCatalogNo,PriceCatalogRef,DateCreated,Objstate,Objgrants,luname,keyref",
            .Token = token
            }

            Dim result = Await ifsClient.GetAsync(Of PriceList)(requestContent)
            Dim priceList = result.Value
            If (ExchangeRateToCurrency IsNot Nothing AndAlso priceList IsNot Nothing) Then
                For Each item In priceList
                    Dim price = item.QuotePriceInclTax
                    If (ExchangeRateToCurrency.CurrencyCode <> item.CurrencyCode AndAlso ExchangeRateToCurrency.ExchangeRateBook IsNot Nothing) Then
                        price = CalculatePriceToCurrency(ExchangeRateToCurrency, item.CurrencyCode, price)
                    End If
                    item.CurrentCurrency = price
                Next
            End If
            Return priceList
        Catch ex As TaskCanceledException
            Throw New Exception($"Operation Timed out getting Price List from API")
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw ex
        End Try

    End Function

    Public Async Function GetPartPOLines(PartNumber As String, Optional Companies As DataTable = Nothing, Optional ExchangeRateToCurrency As CurrencyRate = Nothing, Optional token As String = Nothing) As Task(Of IEnumerable(Of IFSPurchaseOrderLine))
        Try
            Dim ifsClient = New IFSClient()
            Dim requestContent = New IFSRequest() With {
            .Uri = "PurchaseOrderLinesHandling.svc/PurchaseOrderLineSet",
            .FilterStatement = $"((PartNo eq '{PartNumber}') and FbuyUnitPrice ne 0)",
            .Token = token
            }

            Dim result = Await ifsClient.GetAsync(Of IFSPurchaseOrderLine)(requestContent)
            Dim PoLines = result.Value
            If (ExchangeRateToCurrency IsNot Nothing) Then
                For Each item In PoLines
                    Dim price = Decimal.Parse(item.BuyUnitPrice)
                    If (ExchangeRateToCurrency.CurrencyCode <> item.CurrencyCode AndAlso ExchangeRateToCurrency.ExchangeRateBook IsNot Nothing) Then
                        price = CalculatePriceToCurrency(ExchangeRateToCurrency, item.CurrencyCode, price)
                    End If
                    item.Currentcurrency = price
                Next
            End If
            Return PoLines
        Catch ex As TaskCanceledException
            Throw New Exception($"Operation Timed out getting Price List from API")
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw ex
        End Try
    End Function

    Private Function CalculatePriceToCurrency(rateBook As CurrencyRate, currency As String, itemPriceInActualCurrency As Decimal)
        Dim dollarRate = GetExchangeRateFromBook(rateBook.ExchangeRateBook, currency)
        Dim currencyRate = GetExchangeRateFromBook(rateBook.ExchangeRateBook, rateBook.CurrencyCode)

        Dim usDollarprice = Math.Round(CDbl(itemPriceInActualCurrency * dollarRate), 2)
        Dim price = Math.Round(CDbl(usDollarprice / currencyRate), 2)
        Return price
    End Function

    'Private Async Function GetRequestHeder() As Task(Of HttpClient)

    '    Dim token = Await (New IFSClient().GetToken())
    '    Dim client As New HttpClient()
    '    client.BaseAddress = New Uri(BaseUrl)
    '    client.Timeout = TimeSpan.FromSeconds(ApiRequestTimeOut)
    '    client.DefaultRequestHeaders.Add("Accept", "application/json")
    '    client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", token)
    '    Return client
    'End Function

    'Private Function SanitizeIfsJson(json As String) As String
    '    Dim cleanedJson As String = Regex.Replace(json, """[^""]*@odata\.type""\s*:\s*""[^""]*"",?", "")
    '    cleanedJson = Regex.Replace(json, """C\d+_(\w+)""", """$1""")
    '    Return cleanedJson
    'End Function

#End Region
#Region "Database interactions"
    Public Async Function GetAllPOs(Optional connection As SqlConnection = Nothing) As Task(Of IEnumerable(Of IFSPurchaseOrderSchema))
        Dim conn = If(connection IsNot Nothing, connection, GetOpenedFinesseConnection())
        Dim Result = Await conn.ExecuteSTPDAndGetACollectionAsync(Of IFSPurchaseOrderSchema)("IFS.get_all_purchase_orders")
        If (connection Is Nothing) Then
            conn.Dispose()
        End If
        Return Result
    End Function
    Public Async Function GetAllInventoryInStockDetails(Optional connection As SqlConnection = Nothing) As Task(Of IEnumerable(Of IFSInventoryStock))
        Dim conn = If(connection IsNot Nothing, connection, GetOpenedFinesseConnection())
        Dim Result = Await conn.ExecuteSTPDAndGetACollectionAsync(Of IFSInventoryStock)("IFS.get_all_purchase_orders_inventory_in_stock")
        If (connection Is Nothing) Then
            conn.Dispose()
        End If
        Return Result
    End Function
    Public Async Function GetPartInfo(partno As String, Optional IFSCompany As String = Nothing, Optional connection As SqlConnection = Nothing) As Task(Of FinessePart)
        Dim conn = If(connection IsNot Nothing, connection, GetOpenedFinesseConnection())
        Dim part = Await conn.ExecuteSTPDAndGetACollectionAsync(Of FinessePart)("IFS.get_part_detail", {"@partno", partno,
                                                                                                        "@ifsCompany", If(String.IsNullOrEmpty(IFSCompany), "", IFSCompany)
                                                                                                        })
        If (connection Is Nothing) Then
            conn.Dispose()
        End If
        Return part.FirstOrDefault()

    End Function

    Public Async Function GetIFSInventoryReceiptMessages(PoNumber As String, Optional IFSCompany As String = Nothing, Optional connection As SqlConnection = Nothing) As Task(Of IEnumerable(Of IFSInventoryReceiptMessages))
        Dim conn = If(connection IsNot Nothing, connection, GetOpenedFinesseConnection())

        Dim Result = Await conn.ExecuteSTPDAndGetACollectionAsync(Of IFSInventoryReceiptMessages)("IFS.get_po_messages_by_po", {"@orderNo", PoNumber})
        If (connection Is Nothing) Then
            conn.Dispose()
        End If
        Return Result
    End Function


    Public Async Function GetPartFromCatalog(PartNumber As String, Optional IFSCompany As String = Nothing, Optional connection As SqlConnection = Nothing) As Task(Of PartCatalog)
        Dim conn = If(connection IsNot Nothing, connection, GetOpenedFinesseConnection())

        Dim Result = Await conn.ExecuteSTPDAndGetACollectionAsync(Of PartCatalog)("IFS.get_part_from_catalog", {"@partno", PartNumber
                                                                                                  })
        If (connection Is Nothing) Then
            conn.Dispose()
        End If
        Return Result.FirstOrDefault()
    End Function

    Public Async Function GetPartsFromCatalogBySearchTerm(searchTerm As String, Optional connection As SqlConnection = Nothing) As Task(Of IEnumerable(Of BasePartInfo))
        Dim conn = If(connection IsNot Nothing, connection, GetOpenedFinesseConnection())
        Dim Result = Await conn.ExecuteSTPDAndGetACollectionAsync(Of BasePartInfo)("IFS.get_part_catalog_by_number_or_description", {"@searchTerms", searchTerm
                                                                                                  })
        If (connection Is Nothing) Then
            conn.Dispose()
        End If
        Return Result
    End Function


#End Region

End Module
