Imports System.Runtime.CompilerServices

Public Module UltraGridUtilities
    <Extension()> _
    Public Sub FormatInfo(ByVal g As Infragistics.Win.UltraWinGrid.UltraGrid, ByVal c As System.Globalization.CultureInfo)

        If g.DataSource IsNot Nothing Then
            For Each b As Infragistics.Win.UltraWinGrid.UltraGridBand In g.DisplayLayout.Bands
                For Each col As Infragistics.Win.UltraWinGrid.UltraGridColumn In b.Columns
                    col.FormatInfo = c
                Next
            Next
        End If

    End Sub

    <Extension()> _
    Public Function GetDataRowView(ByVal ugr As Infragistics.Win.UltraWinGrid.UltraGridRow) As DataRowView
        Return CType(ugr.ListObject, DataRowView)
    End Function

    <Extension()> _
    Public Function GetDataRow(ByVal ugr As Infragistics.Win.UltraWinGrid.UltraGridRow) As DataRow
        Return CType(ugr.ListObject, DataRowView).Row
    End Function

    <Extension()>
    Public Function DataRow(Of T As DataRow)(ByVal gr As Infragistics.Win.UltraWinGrid.UltraGridRow) As T
        Dim rv = CType(gr.ListObject, DataRowView)

        If rv IsNot Nothing Then
            Return TryCast(rv.Row, T)
        End If

        Return Nothing
    End Function

    <System.Runtime.CompilerServices.Extension()>
    Public Function GetSelectedDataRows(Of T As DataRow)(ByVal dgv As Infragistics.Win.UltraWinGrid.UltraGrid) As IEnumerable(Of T)
        Return (
            From r As Infragistics.Win.UltraWinGrid.UltraGridRow In dgv.Selected.Rows
            Where r.ListObject IsNot Nothing
            Select rv = CType(r.ListObject, DataRowView)
            Select CType(rv.Row, T)
        )
    End Function
End Module
