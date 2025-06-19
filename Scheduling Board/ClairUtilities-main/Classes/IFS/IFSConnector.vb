Public Class IFSConnector
    Public Property Environment As String = "CFG"
    Public Property Username As String
    Public Property Password As String
    Public Property Force As Boolean
    Public Property IFSEnvProps As Object

    Private Shared _scriptIFSEnvProps As Object

    Public Sub ConnectIFS()
        If IFSEnvProps IsNot Nothing Then
            _scriptIFSEnvProps = IFSEnvProps
            Console.WriteLine("Hello")
            Return
        End If

        If Force OrElse (_scriptIFSEnvProps IsNot Nothing AndAlso Not _scriptIFSEnvProps.Environment.Equals(Environment, StringComparison.OrdinalIgnoreCase)) Then
            Console.WriteLine("Clear IFSEnvProps")
            _scriptIFSEnvProps = Nothing
        End If

        Dim ifsEnvironment As String = String.Empty
        Dim apiConfigUrl As String = String.Empty
        Dim onePassID As String = String.Empty

        Select Case Environment.ToUpper()
            Case "PRD"
                onePassID = "y6os6xbvkz5djs2drzfunp3eqa"
                ifsEnvironment = "https://clair.ifs.cloud"
                apiConfigUrl = $"{ifsEnvironment}/auth/realms/claiprd1/.well-known/openid-configuration"
            Case "STG"
                onePassID = "g6zzfsugjyiha65ns4k4xzqfbe"
                ifsEnvironment = "https://clair-stg.ifs.cloud"
                apiConfigUrl = $"{ifsEnvironment}/auth/realms/claistg1/.well-known/openid-configuration"
            Case "CFG"
                onePassID = "b7zogdwcp35pkjecgducuxoqsy"
                ifsEnvironment = "https://clair-cfg.ifs.cloud"
                apiConfigUrl = $"{ifsEnvironment}/auth/realms/claicfg1/.well-known/openid-configuration"
            Case "DEV"
                onePassID = "y6vyzkfz4vtwif7gmr76bye44m"
                ifsEnvironment = "https://pa2mahu-dev1.build.ifs.cloud"
                apiConfigUrl = $"{ifsEnvironment}/auth/realms/pa2mahudev1/.well-known/openid-configuration"
            Case "UAT"
                onePassID = "656fdpbnz6slzhyd3dxke7qvd4"
                ifsEnvironment = "https://clair-uat.ifs.cloud"
                apiConfigUrl = $"{ifsEnvironment}/auth/realms/claiuat1/.well-known/openid-configuration"
            Case "TST"
                onePassID = "qbxi4c2iio4abxer2a3oo4qxuq"
                ifsEnvironment = "https://clair-tst.ifs.cloud"
                apiConfigUrl = $"{ifsEnvironment}/auth/realms/claitst1/.well-known/openid-configuration"
            Case Else
                Throw New ArgumentException($"Environment '{Environment}' is not supported.")
        End Select

        Dim apiBaseUrl As String = $"{ifsEnvironment}/main/ifsapplications/projection/v1"

        Dim body As New Dictionary(Of String, String)

        If Not String.IsNullOrWhiteSpace(Username) AndAlso Not String.IsNullOrWhiteSpace(Password) Then
            body.Add("client_id", Username)
            body.Add("client_secret", Password)
            body.Add("redirect_uri", "https://localhost")
            body.Add("grant_type", "client_credentials")
        Else
            ' Code for retrieving credentials from an external tool like 1Password would go here.
            Throw New NotImplementedException("Retrieving credentials from 1Password is not implemented.")
        End If

        Try
            Dim openid = InvokeRestMethod(apiConfigUrl)
            Dim response = InvokeRestMethod(openid("token_endpoint").ToString(), body)
            Dim jwt = ParseJWT(response("access_token").ToString())
            Dim jwtRefresh = ParseJWT(response("refresh_token").ToString())

            Dim properties As New Dictionary(Of String, Object) From {
                {"TokenEndpoint", openid("token_endpoint")},
                {"Token", response("access_token")},
                {"RefreshToken", response("refresh_token")},
                {"RequestTime", UnixTimeToDateTime(jwt("iat"))},
                {"AccessExpires", UnixTimeToDateTime(jwt("exp"))},
                {"RefreshExpires", UnixTimeToDateTime(jwtRefresh("exp"))},
                {"Domain", ifsEnvironment},
                {"ApiBaseUrl", apiBaseUrl},
                {"Environment", Environment}
            }

            _scriptIFSEnvProps = properties
        Catch ex As Exception
            Throw New Exception("Error retrieving token from IFS.", ex)
        End Try
    End Sub

    Private Function InvokeRestMethod(ByVal url As String, Optional body As Dictionary(Of String, String) = Nothing) As Object
        ' Implementation for making HTTP calls using HttpClient or similar.
        Throw New NotImplementedException("HTTP call implementation is required.")
    End Function

    Private Function ParseJWT(ByVal token As String) As Dictionary(Of String, Object)
        ' Implementation for parsing JWT tokens.
        Throw New NotImplementedException("JWT parsing implementation is required.")
    End Function

    Private Function UnixTimeToDateTime(ByVal unixTime As Long) As DateTime
        Return DateTimeOffset.FromUnixTimeSeconds(unixTime).LocalDateTime
    End Function
End Class
