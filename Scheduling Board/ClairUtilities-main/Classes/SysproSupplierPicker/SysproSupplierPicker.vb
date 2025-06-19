Imports System.Data.SqlClient
Imports System.Text
Imports System.Threading

Public Class SysproSupplierPicker

    Private _searchDelay As Integer = 1000
    Private _searchTimer As Threading.Timer

    Public selectionMade As Boolean = False

    Private connectionString As String

    Public _selectedRow As DataRow

    Private _searchResults As New DataTable

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Sub New(SqlConnectionString As String)

        InitializeComponent()

        connectionString = SqlConnectionString

        tstbSearchTerms.TextBox.TabIndex = 1
    End Sub


    Private Sub Form_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        tstbSearchTerms.Select()
    End Sub


    Private Sub invokeSearchCatalog(ByVal x As Object)
        Dim mi = New MethodInvoker(AddressOf populateSearchResults)
        Invoke(mi)
    End Sub

    Private Sub tsbtnCancel_Click(sender As Object, e As EventArgs) Handles tsbCancel.Click
        selectionMade = False
        Me.Close()
    End Sub

    Private Sub populateSearchResults()


        Dim WhereClause As StringBuilder = New StringBuilder

        WhereClause.Append($"WHERE (asu.Supplier LIKE '%{tstbSearchTerms.Text}%' COLLATE SQL_Latin1_General_CP1_CI_AS OR asu.SupplierName LIKE '%{tstbSearchTerms.Text}%' COLLATE SQL_Latin1_General_CP1_CI_AS)")

        For Each searchString As String In tstbSearchTerms.Text.Split(" ")
            WhereClause.Append($" AND (asu.Supplier LIKE '%{searchString}%' COLLATE SQL_Latin1_General_CP1_CI_AS OR asu.SupplierName LIKE '%{searchString}%' COLLATE SQL_Latin1_General_CP1_CI_AS)")
        Next

        Dim search As StringBuilder = New StringBuilder
        search.Append($"select asu.Supplier, asu.SupplierName
                        from dbo.ApSupplier AS asu
                        {WhereClause.ToString}")

        Dim foo = search.ToString

        Dim newConn As New SqlConnection(connectionString)
        newConn.Open()
        _searchResults = newConn.GetDataTable(search.ToString)
        newConn.Close()
        newConn.Dispose()

        lvSupplierList.Items.Clear()

        For Each result As DataRow In _searchResults.Rows
            Dim arr As String() = New String(2) {}
            With result
                arr(0) = .Item("Supplier")
                arr(1) = .Item("SupplierName")

                Dim item As ListViewItem = New ListViewItem(arr)

                item.Tag = .Item("Supplier")

                lvSupplierList.Items.Add(item)
            End With
        Next

        lvSupplierList.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
    End Sub

    Private Sub tstbSearch_textChanged(sender As Object, e As EventArgs) Handles tstbSearchTerms.TextChanged
        If tstbSearchTerms.Text.Length <= 3 Then
            Exit Sub
        End If

        If IsNothing(_searchTimer) Then
            _searchTimer = New Threading.Timer(AddressOf invokeSearchCatalog, Nothing, _searchDelay, Timeout.Infinite)
        Else
            _searchTimer.Change(_searchDelay, Timeout.Infinite)
        End If
    End Sub

    Private Sub lvSearchResults_DoubleClick(sender As Object, e As EventArgs) Handles lvSupplierList.DoubleClick
        If lvSupplierList.SelectedItems.Count = 0 Then
            Exit Sub
        End If

        Dim row As DataRow() = _searchResults.Select($"Supplier = '{lvSupplierList.SelectedItems(0).Tag}'")

        If row.Any Then
            _selectedRow = row(0)
            selectionMade = True
            Me.Close()
        End If
    End Sub

End Class