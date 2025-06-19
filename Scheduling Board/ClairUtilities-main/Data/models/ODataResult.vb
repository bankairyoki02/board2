Imports Newtonsoft.Json

Public Class ODataResult(Of T)
    <JsonProperty("@odata.contex")>
    Public Property Context As String

    <JsonProperty("@odata.type")>
    Public Property Type As String

    <JsonProperty("value")>
    Public Property Value As IEnumerable(Of T) = New List(Of T)()
End Class
