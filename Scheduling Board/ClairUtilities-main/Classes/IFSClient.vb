Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Reflection.Emit
Imports System.Security.Policy
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class IFSClient

#Region "Properties"

    Private _token As String
    Public Property Token() As String
        Get
            Return _token
        End Get
        Set(ByVal value As String)
            _token = value
        End Set
    End Property

    Private _environmentConfig As EnvironmentConfig
    Private Property EnvironmentCredentials() As EnvironmentConfig
        Get
            Return _environmentConfig
        End Get
        Set(ByVal value As EnvironmentConfig)
            _environmentConfig = value
        End Set
    End Property

    Private _requestTimeOut As Integer = 10
    Public Property RequestTimeout() As Integer
        Get
            Return _requestTimeOut
        End Get
        Set(ByVal value As Integer)
            _requestTimeOut = value
        End Set
    End Property

    Public ReadOnly Property Environment() As String
        Get
            Dim database = System.Environment.GetEnvironmentVariable("ESSDB")
            Return If(database.ToLower.Contains("test"), "uat", "prd")
        End Get
    End Property

    Public BaseUrl As String

    Public Sub Initialize(Optional ifsEnvironment As String = Nothing)
        If (EnvironmentCredentials IsNot Nothing) Then
            Exit Sub
        End If

        Dim credentials = New IFSCredentialManager().GetCredentials()
        Dim env = If(String.IsNullOrEmpty(ifsEnvironment), Environment, ifsEnvironment)
        Dim cred = credentials.environments(env)
        BaseUrl = credentials.baseUrl
        EnvironmentCredentials = cred
    End Sub

    Public ReadOnly Property AuthenticationBody() As Dictionary(Of String, String)
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

#Region "Constructor"


    Public Sub New()
        Initialize()
    End Sub


    Public Sub New(ifsEnvironment As String)
        Initialize(ifsEnvironment)
    End Sub

#End Region


    Private Async Function GetIFSToken() As Task(Of String)
        Try
            Using client As New HttpClient()
                client.BaseAddress = New Uri(EnvironmentCredentials.domain)
                client.Timeout = TimeSpan.FromSeconds(RequestTimeout)
                client.DefaultRequestHeaders.Add("Accept", "application/json")
                Dim content As New FormUrlEncodedContent(AuthenticationBody)
                Dim response As HttpResponseMessage = Await client.PostAsync(EnvironmentCredentials.token_url, content)

                If response.IsSuccessStatusCode Then
                    Dim responseBody As String = Await response.Content.ReadAsStringAsync()
                    Dim jsonResponse As JObject = JObject.Parse(responseBody)
                    Return jsonResponse("access_token").ToString()
                Else
                    Throw New Exception($"Unable to authorize the IFS user. {response.ReasonPhrase}")
                End If
            End Using
        Catch ex As TaskCanceledException
            Throw New Exception($"Operation Timed out getting Token from API")
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw ex
        End Try
    End Function

    Private _refreshRetries = 0

    Public Async Function GetAsync(Of T)(request As IFSRequest) As Task(Of ODataResult(Of T))
        Using client As New HttpClient()
            _refreshRetries = _refreshRetries + 1
            Dim url = $"{BaseUrl}/{request.RequestURL}"
            client.BaseAddress = New Uri(EnvironmentCredentials.domain)
            client.Timeout = TimeSpan.FromSeconds(RequestTimeout)
            client.DefaultRequestHeaders.Add("Accept", "application/json")
            Dim _token = request.Token
            If (String.IsNullOrEmpty(_token)) Then
                _token = Await GetIFSToken()
            End If
            client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", _token)
            Dim requesResult = Await client.GetAsync(url)
            If requesResult.IsSuccessStatusCode Then
                Dim responseBody As String = Await requesResult.Content.ReadAsStringAsync()
                Dim cleanedJson As String = SanitizeIfsJson(responseBody)
                Dim jsonResponse = JsonConvert.DeserializeObject(Of ODataResult(Of T))(cleanedJson)
                _refreshRetries = 0
                Return jsonResponse
            Else
                If (requesResult.StatusCode = HttpStatusCode.Unauthorized) Then
                    If (String.IsNullOrEmpty(request.Token) AndAlso _refreshRetries < 2) Then
                        request.Token = Await GetIFSToken()
                        Return Await GetAsync(Of T)(request)
                    Else
                        _refreshRetries = 0
                        Throw New Exception("The user does not have permission to access the resource.")
                    End If
                End If
                client.Dispose()
                _refreshRetries = 0
                Throw New Exception($"Unable to fetch IFS Data from the API. Reason: {vbNewLine} {requesResult.ReasonPhrase}")
            End If
        End Using
    End Function

    Public Async Function GetToken() As Task(Of String)
        Dim token = Await GetIFSToken()
        Return token
    End Function

    Public Async Function GetRequestHeder() As Task(Of HttpClient)
        Dim token = Await GetIFSToken()
        Dim client As New HttpClient With {
            .BaseAddress = New Uri(BaseUrl),
            .Timeout = TimeSpan.FromSeconds(RequestTimeout)
        }
        client.DefaultRequestHeaders.Add("Accept", "application/json")
        client.DefaultRequestHeaders.Authorization = New AuthenticationHeaderValue("Bearer", token)
        Return client
    End Function

    Private Function SanitizeIfsJson(json As String) As String
        Dim cleanedJson As String = Regex.Replace(json, """[^""]*@odata\.type""\s*:\s*""[^""]*"",?", "")
        cleanedJson = Regex.Replace(json, """C\d+_(\w+)""", """$1""")
        Return cleanedJson
    End Function

End Class

Public Class IFSRequest
    ''' <summary>
    ''' If not supplied, a request to get the token will be made using the user: FINESSE_PO_READER
    ''' </summary>
    ''' <returns></returns>
    Public Property Token As String
    ''' <summary>
    ''' URL to get the data from e.i: SupplierForPurchasePartsHandling.svc/PurchasePartSupplierSet
    ''' </summary>
    ''' <returns></returns>
    Public Property Uri As String
    ''' <summary>
    ''' *Optional* Select statement for the Request (comma sepparated column values) e.i: PartNo,Contract,VendorNo,SupplierName,Company
    ''' </summary>
    ''' <returns></returns>
    Public Property SelectStatement As String
    ''' <summary>
    ''' *Optional* Filter Statement for the request. e.i: PartNo eq '123456'
    ''' </summary>
    ''' <returns></returns>
    Public Property FilterStatement As String

    Private _requestUrl As String
    Public ReadOnly Property RequestURL() As String
        Get
            Dim url = $"{Uri}"
            Dim uriElement = "?"
            If (Not String.IsNullOrEmpty(SelectStatement)) Then
                url += $"{uriElement}$select={SelectStatement}"
            End If

            If (url.Contains("?$")) Then
                uriElement = "&"
            End If

            If (Not String.IsNullOrEmpty(FilterStatement)) Then
                url += $"{uriElement}$filter={FilterStatement}"
            End If


            Return url
        End Get
    End Property

    Public Sub New()
    End Sub

    ''' <summary>
    ''' Execute a HTTP Get request to IFS for the specify url, the domain and rest of the uri will be autocompleted
    ''' </summary>
    ''' <param name="_uri">URL to get the data from e.i: SupplierForPurchasePartsHandling.svc/PurchasePartSupplierSet</param>
    ''' <param name="_token">If not supplied, a request to get the token will be made using the user: FINESSE_PO_READER</param>
    ''' <param name="_selectStatement">*Optional* Select statement for the Request (comma sepparated column values) e.i: PartNo,Contract,VendorNo,SupplierName,Company</param>
    ''' <param name="_filterStatement">*Optional* Filter Statement for the request. e.i: PartNo eq '123456'</param>
    Public Sub New(_uri As String, Optional _token As String = Nothing, Optional _selectStatement As String = Nothing, Optional _filterStatement As String = Nothing)
        Uri = _uri
        Token = _token
        SelectStatement = _selectStatement
        FilterStatement = _filterStatement
    End Sub

End Class
