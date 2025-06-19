Imports System.IO
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class EnvironmentConfig
    Public Property domain As String
    Public Property token_url As String
    Public Property secrets As Dictionary(Of String, String)
    Public Property grant_type As String

End Class


Public Class IFSCredentialManager
    <JsonProperty("baseUrl")>
    Public Property baseUrl As String
    <JsonExtensionData>
    Public Property environments As Dictionary(Of String, EnvironmentConfig)

    Public Function GetCredentials() As IFSCredentialManager
        Dim mainPath As String = System.Environment.GetEnvironmentVariable("ESSVBDir")
        Dim credentialsPath = System.IO.Path.Combine(mainPath, "ifs-credential-manager.json")
        If File.Exists(credentialsPath) Then
            Dim jsonContent As String = File.ReadAllText(credentialsPath)
            Dim rawData As JObject = JObject.Parse(jsonContent)

            Dim baseUrl As String = rawData("baseUrl").ToString()

            Dim environments As New Dictionary(Of String, EnvironmentConfig)

            For Each envKey In rawData.Properties().Where(Function(p) p.Name <> "baseUrl")
                environments.Add(envKey.Name, envKey.Value.ToObject(Of EnvironmentConfig)())
            Next

            Dim credentials As New IFSCredentialManager() With {
        .baseUrl = baseUrl,
        .environments = environments
    }

            Return credentials
        Else
            Throw New FileNotFoundException($"The file '{credentialsPath}' was not found.")
        End If

    End Function
End Class

