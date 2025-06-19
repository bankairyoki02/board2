Public Class GroupedListBox
    Inherits ListBox


    Private Interface IGroupedListBoxItem
        Sub Paint(e As System.Windows.Forms.DrawItemEventArgs)
    End Interface

    Public Class GroupedListBoxItem
        Implements IGroupedListBoxItem

        Private _Group As String
        Public Property Group() As String
            Get
                Return _Group
            End Get
            Private Set(value As String)
                _Group = value
            End Set
        End Property

        Private _Item As String
        Public Property Item() As String
            Get
                Return _Item
            End Get
            Private Set(ByVal value As String)
                _Item = value
            End Set
        End Property

        Private _ItemData As Object
        Public Property ItemData() As Object
            Get
                Return _ItemData
            End Get
            Private Set(ByVal value As Object)
                _ItemData = value
            End Set
        End Property

        Sub New(Group As String, Item As String, Optional ItemData As Object = Nothing)
            _Group = Group
            _Item = Item
            _ItemData = ItemData
        End Sub


        Public Sub Paint(e As System.Windows.Forms.DrawItemEventArgs) Implements IGroupedListBoxItem.Paint
            e.DrawBackground()
            Using b As New System.Drawing.SolidBrush(e.ForeColor)
                Dim r As Rectangle = e.Bounds
                r.Offset(5, 0)
                e.Graphics.DrawString(Item, e.Font, b, r)
            End Using
            e.DrawFocusRectangle()
        End Sub
    End Class

    Private Class GroupedListBoxGroup
        Implements IGroupedListBoxItem

        Private _Group As String
        Public Property Group() As String
            Get
                Return _Group
            End Get
            Private Set(value As String)
                _Group = value
            End Set
        End Property

        Sub New(ByVal Group)
            _Group = Group
        End Sub

        Public Sub Paint(e As System.Windows.Forms.DrawItemEventArgs) Implements IGroupedListBoxItem.Paint
            e.DrawBackground()
            Using b As New System.Drawing.SolidBrush(e.ForeColor), f As New Font(e.Font, FontStyle.Bold)
                e.Graphics.DrawString(Group, f, b, e.Bounds)
            End Using
            e.DrawFocusRectangle()
        End Sub
    End Class

    Private Class GroupedListBoxAllItem
        Implements IGroupedListBoxItem

        Public Sub Paint(e As System.Windows.Forms.DrawItemEventArgs) Implements IGroupedListBoxItem.Paint
            e.DrawBackground()
            Using b As New System.Drawing.SolidBrush(e.ForeColor), f As New Font(e.Font, FontStyle.Bold)
                Dim r As Rectangle = e.Bounds
                e.Graphics.DrawString("All", f, b, r)
            End Using
            e.DrawFocusRectangle()
        End Sub
    End Class

    Private _SelectingMultipleItems As Boolean
    Private _focusedIndex As Integer = -1

    Public Property IncludeAllItem As Boolean

    Public ReadOnly Property SelectingMultipleItems As Boolean
        Get
            Return _SelectingMultipleItems
        End Get
    End Property

    Public Sub New()
        Me.DrawMode = Windows.Forms.DrawMode.OwnerDrawFixed
    End Sub


    Public Sub Initialize(items As IEnumerable(Of GroupedListBoxItem))
        Dim groups =
            From c In items
            Select c.Group
            Distinct
            Select New GroupedListBoxGroup(Group)

        Dim listBoxItems = New List(Of IGroupedListBoxItem)

        For Each g In groups
            Dim group = g
            listBoxItems.Add(group)
            listBoxItems.AddRange(From c In items Where c.Group = group.Group)
        Next

        If IncludeAllItem Then
            MyBase.Items.Add(New GroupedListBoxAllItem)
        End If

        MyBase.Items.AddRange(listBoxItems.ToArray)
    End Sub

    Protected Overrides Sub OnDrawItem(e As System.Windows.Forms.DrawItemEventArgs)
        If (e.State And DrawItemState.Focus) <> 0 Then
            _focusedIndex = e.Index
        End If

        ' the designer puts strings into these listboxes, so we need to handle that case.
        Dim itemObject = If(e.Index >= 0 AndAlso e.Index < MyBase.Items.Count,
                            MyBase.Items(e.Index),
                            MyBase.Name)

        Dim item = TryCast(itemObject, IGroupedListBoxItem)
        If item IsNot Nothing Then
            item.Paint(e)
        Else
            e.DrawBackground()
            Using b As New System.Drawing.SolidBrush(e.ForeColor)
                e.Graphics.DrawString(itemObject.ToString, e.Font, b, e.Bounds)
            End Using
            e.DrawFocusRectangle()
        End If
    End Sub

    Protected Overrides Sub OnSelectedIndexChanged(e As System.EventArgs)
        If _SelectingMultipleItems Then
            Return
        End If

        If _focusedIndex >= 0 Then
            Dim group = TryCast(MyBase.Items(_focusedIndex), GroupedListBoxGroup)

            If group IsNot Nothing Then
                MyBase.BeginUpdate()
                _SelectingMultipleItems = True

                Dim itemsInGroup =
                    From c In MyBase.Items.OfType(Of GroupedListBoxItem)()
                    Where c.Group = group.Group

                If MyBase.SelectionMode = Windows.Forms.SelectionMode.One Then
                    itemsInGroup = {itemsInGroup.First}
                End If

                For Each item In itemsInGroup.ToList
                    MyBase.SelectedItems.Add(item)
                Next

                _SelectingMultipleItems = False
                MyBase.EndUpdate()
            End If
        End If

        MyBase.OnSelectedIndexChanged(e)
    End Sub

    Public Shadows Property SelectedItems() As IEnumerable(Of GroupedListBoxItem)
        Get
            Return MyBase.SelectedItems.OfType(Of GroupedListBoxItem)()
        End Get

        Set(ByVal items As IEnumerable(Of GroupedListBoxItem))
            MyBase.SelectedItems.Clear()
            SelectItems(items)
        End Set
    End Property

    Public Shadows ReadOnly Property Items() As IEnumerable(Of GroupedListBoxItem)
        Get
            Return MyBase.Items.OfType(Of GroupedListBoxItem)()
        End Get
    End Property

    Public Sub SelectItems(ByVal items As IEnumerable(Of GroupedListBoxItem))
        _SelectingMultipleItems = True

        items.ToList.ForEach(Sub(item)
                                 MyBase.SelectedItems.Add(item)
                             End Sub)

        _SelectingMultipleItems = False
    End Sub

    Public Sub SelectItemsFromApplicationSetting(ByVal AppSetting As System.Collections.Specialized.StringCollection)
        If AppSetting IsNot Nothing Then
            SelectedItems = From c In Items
                                      Where AppSetting.Contains(c.Item)
        End If
    End Sub

    Public Sub SaveSelectedItemsToApplicationSetting(ByRef AppSetting As System.Collections.Specialized.StringCollection)
        Dim selectedItems = From cci In Me.SelectedItems Select cci.Item

        AppSetting = New System.Collections.Specialized.StringCollection
        AppSetting.AddRange(selectedItems.ToArray)
    End Sub

End Class
