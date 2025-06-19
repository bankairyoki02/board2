Imports System.Collections
Imports System.Windows.Forms

Public Class ListViewColumnSorter
    Implements System.Collections.IComparer

    Private myColumnToSort As Integer = 0
    Private myOrderOfSort As SortOrder = SortOrder.None
    Private myObjectCompare As New CaseInsensitiveComparer
    Private myColumnTypeDict As New Dictionary(Of Integer, ListViewColumnType)

    Public Enum ListViewColumnType
        ctInt
        ctString
    End Enum

    Public Property ColumnType(ByVal Col As Integer) As ListViewColumnType
        Get
            If (myColumnTypeDict.ContainsKey(Col)) Then
                ColumnType = myColumnTypeDict(Col)
            Else
                ColumnType = ListViewColumnType.ctString
            End If
        End Get
        Set(ByVal value As ListViewColumnType)
            myColumnTypeDict(Col) = value
        End Set
    End Property


    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim compareResult As Integer
        Dim listviewX As ListViewItem
        Dim listviewY As ListViewItem

        ' Cast the objects to be compared to ListViewItem objects.
        listviewX = CType(x, ListViewItem)
        listviewY = CType(y, ListViewItem)

        Select ColumnType(myColumnToSort)
            Case ListViewColumnType.ctInt
                Dim xLVItem As Integer, yLVItem As Integer
                xLVItem = listviewX.SubItems(myColumnToSort).Text
                yLVItem = Integer.Parse(listviewY.SubItems(myColumnToSort).Text)
                compareResult = myObjectCompare.Compare(xLVItem, yLVItem)
            Case ListViewColumnType.ctString
                compareResult = myObjectCompare.Compare(listviewX.SubItems(myColumnToSort).Text, listviewY.SubItems(myColumnToSort).Text)
            Case Else
                Debug.Assert(False)
        End Select

        ' Calculate the correct return value based on the object 
        ' comparison.
        If (myOrderOfSort = SortOrder.Ascending) Then
            ' Ascending sort is selected, return typical result of 
            ' compare operation.
            Return compareResult
        ElseIf (myOrderOfSort = SortOrder.Descending) Then
            ' Descending sort is selected, return negative result of 
            ' compare operation.
            Return (-compareResult)
        Else
            ' Return '0' to indicate that they are equal.
            Return 0
        End If
    End Function

    Public Property SortColumn() As Integer
        Set(ByVal Value As Integer)
            myColumnToSort = Value
        End Set

        Get
            Return myColumnToSort
        End Get
    End Property

    Public Property Order() As SortOrder
        Set(ByVal Value As SortOrder)
            myOrderOfSort = Value
        End Set

        Get
            Return myOrderOfSort
        End Get
    End Property
End Class

