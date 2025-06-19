Imports System.Data.SqlClient

Public Enum SearchRegion
    None
    StartsWith
    EndsWith
    Contains
End Enum

Public Enum WhenToStartSearch
    Immediately
    AfterKeystrokeDelay
End Enum

Public Class AsYouTypeTableSearch
    Private _connectionString As String
    Private _tableName As String
    Private _columnSearchPositions As Dictionary(Of String, SearchRegion)
    Private _resultsRowLimit As Integer

    Private WithEvents searchHelper As AsYouTypeSearch

    Public Event GotResults(sender As AsYouTypeTableSearch, search As String, results As DataView)

    Public Sub New(connectionString As String, tableName As String, columnSearchPositions As Dictionary(Of String, SearchRegion), Optional resultsRowLimit As Integer = 100)
        _connectionString = connectionString
        _tableName = tableName
        _columnSearchPositions = New Dictionary(Of String, SearchRegion)(columnSearchPositions)
        _resultsRowLimit = resultsRowLimit

        searchHelper = New AsYouTypeSearch(resultsRowLimit, columnSearchPositions)
    End Sub

    Private Sub New()
        Debug.Assert(False)
    End Sub

    Sub StartSearch(searchText As String, Optional ByVal startWhen As WhenToStartSearch = WhenToStartSearch.AfterKeystrokeDelay)
        searchHelper.StartSearch(searchText, startWhen)
    End Sub

    Private Sub searchHelper_GetTableForSearch(sender As AsYouTypeSearch, e As AsYouTypeSearch.GetTableForSearchArgs) Handles searchHelper.GetTableForSearch
        Using conn As New SqlConnection(_connectionString)
            conn.Open()


        End Using
    End Sub

    Private Sub searchHelper_GotResults(sender As AsYouTypeSearch, search As String, results As System.Data.DataView) Handles searchHelper.GotResults
        RaiseEvent GotResults(Me, search, results)
    End Sub
End Class




Public Class AsYouTypeSearch
    Private _columnSearchPositions As Dictionary(Of String, SearchRegion)
    Private _rowCountForTruncatedResults As Integer

    Private _searchLocations As HashSet(Of SearchRegion)
    Private _columnsSearchClauseFormat As String

    Private _resultTableFromCanonicalizedSearch As New Dictionary(Of String, DataTable)

    Private _searches As New List(Of Search)

    Private _delayedSearchTimer As New System.Threading.Timer(AddressOf DelayedSearch)

    Public Event GotResults(sender As AsYouTypeSearch, search As String, results As DataView)


    Private Class Search
        Public Search As String
        Public SearchCanonicalized As String
        Public TimeToExecute As DateTime
    End Class

    Public Class GetTableForSearchArgs
        Private Sub New()

        End Sub

        Protected Friend Sub New(canonicalizedSearch As String)
            Me.CanonicalizedSearch = canonicalizedSearch
        End Sub

        Public ReadOnly CanonicalizedSearch As String
        Public Results As DataTable
    End Class



    Public Event GetTableForSearch(sender As AsYouTypeSearch, e As GetTableForSearchArgs)

    Private Sub RaiseGetTableForSearch(canonicalizedSearch As String)
        Debug.Assert(canonicalizedSearch = GetCanonicalizedSearch(canonicalizedSearch))

        Dim e As GetTableForSearchArgs = New GetTableForSearchArgs(canonicalizedSearch)
        RaiseEvent GetTableForSearch(Me, e)
        If e.Results IsNot Nothing Then
            SyncLock _resultTableFromCanonicalizedSearch
                _resultTableFromCanonicalizedSearch(canonicalizedSearch) = e.Results
            End SyncLock

            SyncLock _searches
                If Not _searchQueue.Peek = canonicalizedSearch Then
                    Return
                End If
            End SyncLock

            RaiseEvent GotResults(Me, canonicalizedSearch, GetCachedSearch(canonicalizedSearch))
        End If
    End Sub


    Public Sub New(ByVal rowCountForTruncatedResults As Integer, columnSearchPositions As Dictionary(Of String, SearchRegion))
        _columnSearchPositions = New Dictionary(Of String, SearchRegion)(columnSearchPositions)
        _searchLocations = New HashSet(Of SearchRegion)(_columnSearchPositions.Values)
        _searchLocations.Remove(SearchRegion.None)

        Dim columnSearchFormats =
            From kv In _columnSearchPositions
            Select columnName = kv.Key, fieldSearchRegion = kv.Value
            Where fieldSearchRegion <> SearchRegion.None
            Select "{1}" & columnName &
                "like '" &
                If(fieldSearchRegion = SearchRegion.StartsWith, "", "%") &
                "{0}" &
                If(fieldSearchRegion = SearchRegion.EndsWith, "", "%") &
                "'"

        _columnsSearchClauseFormat = String.Join(" and ", columnSearchFormats.ToArray)

        _rowCountForTruncatedResults = rowCountForTruncatedResults
    End Sub


    Private Sub New()
        Throw New NotImplementedException()
    End Sub



    Private Function GetFilterString(search As String, Optional tableAlias As String = "") As String
        Dim searchPieces = search.Split(New Char() {" "c}, StringSplitOptions.RemoveEmptyEntries)
        Dim sSQL As New System.Text.StringBuilder

        If Not tableAlias.EndsWith(".") Then
            tableAlias &= "."
        End If

        Dim GetFilterClauseForSearchPiece =
            Function(searchPiece As String) As String
                Return String.Format(_columnsSearchClauseFormat, searchPiece.SQLEscape(), tableAlias)
            End Function

        Dim pieceClauses = Array.ConvertAll(searchPieces, GetFilterClauseForSearchPiece)

        Return String.Join(" or ", pieceClauses)
    End Function


    Public Sub StartSearch(searchText As String, Optional startWhen As WhenToStartSearch = WhenToStartSearch.AfterKeystrokeDelay)
        Dim searchCanonicalized As String = GetCanonicalizedSearch(searchText)
        Dim dv = GetCachedSearch(searchCanonicalized)

        If dv IsNot Nothing Then
            RaiseEvent GotResults(Me, searchText, dv)
            Return
        End If

        '_delayedSearchTimer.
    End Sub


    Private Sub DelayedSearch(state As Object)

    End Sub


    Private Function GetCachedSearch(ByVal searchCanonicalized As String) As DataView
        Debug.Assert(searchCanonicalized = GetCanonicalizedSearch(searchCanonicalized))

        Dim searchFromCache As String = String.Empty
        Dim resultsTable As DataTable = Nothing

        SyncLock _resultTableFromCanonicalizedSearch
            _resultTableFromCanonicalizedSearch.TryGetValue(searchCanonicalized, resultsTable)

            If resultsTable Is Nothing Then
                Dim possibleSupersetSearches =
                    From skv In _resultTableFromCanonicalizedSearch
                    Select searchText = skv.Key, table = skv.Value
                    Where searchText.Length < searchCanonicalized.Length
                    Where _rowCountForTruncatedResults <= 0 OrElse table.Rows.Count < _rowCountForTruncatedResults
                    Select searchText
                    Order By searchText.Length Descending

                For Each cachedSearchText In possibleSupersetSearches
                    If GetCanonicalizedSearch(cachedSearchText & " " & searchCanonicalized) = searchCanonicalized Then
                        resultsTable = _resultTableFromCanonicalizedSearch(cachedSearchText)
                        searchFromCache = cachedSearchText
                        Exit For
                    End If
                Next
            Else
                searchFromCache = searchCanonicalized
            End If
        End SyncLock

        If resultsTable Is Nothing Then
            Return Nothing
        End If

        Dim dv = resultsTable.DefaultView

        If searchCanonicalized <> searchFromCache Then
            dv.RowFilter = GetFilterString(searchCanonicalized)
        Else
            dv.RowFilter = ""
        End If

        Return dv
    End Function

    Public Sub AddSearchResult(search As String, results As DataTable)
        SyncLock _resultTableFromCanonicalizedSearch
            Dim searchCanonicalized = GetCanonicalizedSearch(search)
            _resultTableFromCanonicalizedSearch(searchCanonicalized) = results
        End SyncLock
    End Sub


    Private Function GetCanonicalizedSearch(search As String) As String
        Dim searchingStarts = _searchLocations.Contains(SearchRegion.StartsWith)
        Dim searchingEnds = _searchLocations.Contains(SearchRegion.EndsWith)
        Dim searchingMiddles = _searchLocations.Contains(SearchRegion.Contains)

        Dim isRedundantSearchTerm As Predicate(Of String)
        Dim searchTerms = search.ToLower.Split({" "c}, StringSplitOptions.RemoveEmptyEntries).Distinct.ToArray
        Dim searchTermsSorted() As String
        Dim ixCurrentWord As Integer

        If searchingMiddles And _searchLocations.Count = 1 Then
            searchTermsSorted = (From s In searchTerms Order By s.Length).ToArray
            isRedundantSearchTerm =
                Function(s)
                    For i = ixCurrentWord + 1 To UBound(searchTermsSorted)
                        If searchTermsSorted(i).Contains(s) Then
                            Return True
                        End If
                    Next
                    Return False
                End Function
        ElseIf searchingStarts And Not searchingEnds Then
            searchTermsSorted = (From s In searchTerms Order By s).ToArray
            isRedundantSearchTerm =
                Function(s)
                    For i = ixCurrentWord + 1 To UBound(searchTermsSorted)
                        If searchTermsSorted(i).StartsWith(s) Then
                            Return True
                        ElseIf searchTermsSorted(i) > s Then
                            Return False
                        End If
                    Next
                    Return False
                End Function
        ElseIf searchingEnds And Not searchingStarts Then

            searchTermsSorted = (From s In searchTerms Order By StrReverse(s)).ToArray
            isRedundantSearchTerm =
                Function(s)
                    For i = ixCurrentWord + 1 To UBound(searchTermsSorted)
                        If searchTermsSorted(i).EndsWith(s) Then
                            Return True
                        ElseIf StrReverse(searchTermsSorted(i)) > StrReverse(s) Then
                            Return False
                        End If
                    Next
                    Return False
                End Function
        Else
            Return String.Join(" ", searchTerms)
        End If

        Dim nonRedundantSearchTerms As New List(Of String)
        For ixCurrentWord = LBound(searchTermsSorted) To UBound(searchTermsSorted)
            Dim term As String = searchTermsSorted(ixCurrentWord)
            If Not isRedundantSearchTerm(term) Then
                nonRedundantSearchTerms.Add(term)
            End If
        Next

        Return String.Join(" ", nonRedundantSearchTerms.ToArray)
    End Function

End Class
