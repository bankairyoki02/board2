Imports System.Collections.Concurrent

Module Comparers

    Public Class MultiComparer(Of T)
        Implements IComparer(Of T)

        Private _comparersInOrder As New List(Of IComparer(Of T))

        Public Sub New()

        End Sub

        Public Sub New(comparers As List(Of IComparer(Of T)))
            _comparersInOrder.AddRange(comparers)
        End Sub

        Public Sub Add(comparer As IComparer(Of T))
            _comparersInOrder.Add(comparer)
        End Sub

        Public Function Compare(x As T, y As T) As Integer Implements IComparer(Of T).Compare
            For Each c In _comparersInOrder
                Dim result = c.Compare(x, y)
                If result <> 0 Then
                    Return result
                End If
            Next

            Return 0
        End Function
    End Class

    Public Enum SortDirection
        Ascending = 1
        Descending = -1
    End Enum

    Public Class MultiComparerDirectional(Of T)
        Implements IComparer(Of T)

        Private _comparersInOrder As New List(Of IComparer(Of T))
        Private _directionFromComparer As New Dictionary(Of IComparer(Of T), SortDirection)

        Public Sub New()

        End Sub

        Public Sub New(comparers As List(Of KeyValuePair(Of IComparer(Of T), SortDirection)))
            _comparersInOrder.AddRange(From c In comparers Select c.Key)
            For Each cd In comparers
                _directionFromComparer.Add(cd.Key, cd.Value)
            Next
        End Sub

        Public Sub Add(comparer As IComparer(Of T), direction As SortDirection)
            _comparersInOrder.Add(comparer)
            _directionFromComparer.Add(comparer, direction)
        End Sub

        Public Function Compare(x As T, y As T) As Integer Implements IComparer(Of T).Compare
            For Each c In _comparersInOrder
                Dim result = c.Compare(x, y)
                If result <> 0 Then
                    Return result * _directionFromComparer(c)
                End If
            Next

            Return 0
        End Function
    End Class

    Public Class DataRowFieldComparer(Of T As IComparable)
        Implements IComparer(Of DataRow)

        Private _fieldName As String

        Public Sub New(fieldName As String)
            _fieldName = fieldName
        End Sub

        Public Function Compare(x As DataRow, y As DataRow) As Integer Implements IComparer(Of DataRow).Compare
            Dim xField = CType(x(_fieldName), T)
            Dim yField = CType(y(_fieldName), T)

            Return xField.CompareTo(yField)
        End Function
    End Class

    Public Class DataRowFieldComparer
        Implements IComparer(Of DataRow)

        Private _fieldName As String

        Public Sub New(fieldName As String)
            _fieldName = fieldName
        End Sub

        Public Function Compare(x As DataRow, y As DataRow) As Integer Implements IComparer(Of DataRow).Compare
            Dim xField = CType(x(_fieldName), IComparable)
            Dim yField = CType(y(_fieldName), IComparable)

            Return xField.CompareTo(yField)
        End Function
    End Class

    Public Class DataRowFieldLookupComparer
        Implements IComparer(Of DataRow)

        Private _lookupField As String
        Private _nameFromFieldValue As ConcurrentDictionary(Of String, String)


        Private Sub New()
        End Sub

        Public Sub New(ByVal lookupField As String, ByVal nameFromFieldValue As ConcurrentDictionary(Of String, String))
            _lookupField = lookupField
            _nameFromFieldValue = nameFromFieldValue
        End Sub

        Public Function Compare(x As DataRow, y As DataRow) As Integer Implements IComparer(Of DataRow).Compare
            Dim fieldValue1 = String.Empty
            Dim fieldValue2 = String.Empty
            _nameFromFieldValue.TryGetValue(x(_lookupField), fieldValue1)
            _nameFromFieldValue.TryGetValue(y(_lookupField), fieldValue2)

            Return String.Compare(fieldValue1, fieldValue2)
        End Function
    End Class
End Module


