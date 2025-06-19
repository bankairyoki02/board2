Imports System.Text

Public Class CheckedComboBox
    Inherits ComboBox

    ''' <summary>
    ''' Internal class to represent the dropdown list of the CheckedComboBox
    ''' </summary>
    Friend Class Dropdown
        Inherits Form

        ' ---------------------------------- internal class CCBoxEventArgs --------------------------------------------
        ''' <summary>
        ''' Custom EventArgs encapsulating value as to whether the combo box value(s) should be assignd to or not.
        ''' </summary>
        Friend Class CCBoxEventArgs
            Inherits EventArgs

            Private assignValuesInternal As Boolean
            Public Property AssignValues() As Boolean
                Get
                    Return assignValuesInternal
                End Get
                Set(ByVal value As Boolean)
                    assignValuesInternal = value
                End Set
            End Property

            Private eventArgsInternal As EventArgs
            Public Property EventArgs() As EventArgs
                Get
                    Return eventArgsInternal
                End Get
                Set(ByVal value As EventArgs)
                    eventArgsInternal = value
                End Set
            End Property

            Public Sub New(ByVal e As EventArgs, ByVal assignValues As Boolean)
                MyBase.New()
                Me.EventArgs = e
                Me.AssignValues = assignValues
            End Sub
        End Class

        ' ---------------------------------- internal class CustomCheckedListBox --------------------------------------------

        ''' <summary>
        ''' A custom CheckedListBox being shown within the dropdown form representing the dropdown list of the CheckedComboBox.
        ''' </summary>
        Friend Class CustomCheckedListBox
            Inherits CheckedListBox

            Private curSelIndexInternal As Integer

            Public Sub New()
                MyBase.New()
                Me.SelectionMode = SelectionMode.One
                Me.HorizontalScrollbar = True
            End Sub

            ''' <summary>
            ''' Intercepts the keyboard input, [Enter] confirms a selection and [Esc] cancels it.
            ''' </summary>
            ''' <param name="e">The Key event arguments</param>
            Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
                If e.KeyCode = Keys.Enter Then
                    ' Enact selection.
                    DirectCast(Parent, CheckedComboBox.Dropdown).OnDeactivate(New CCBoxEventArgs(Nothing, True))
                    e.Handled = True
                ElseIf e.KeyCode = Keys.Escape Then
                    ' Cancel selection.
                    DirectCast(Parent, CheckedComboBox.Dropdown).OnDeactivate(New CCBoxEventArgs(Nothing, False))
                    e.Handled = True
                ElseIf e.KeyCode = Keys.Delete Then
                    ' Delete unchecks all, [Shift + Delete] checks all.
                    For i As Integer = 0 To Items.Count - 1
                        SetItemChecked(i, e.Shift)
                    Next
                    e.Handled = True
                End If
                ' If no Enter or Esc keys presses, let the base class handle it.
                MyBase.OnKeyDown(e)
            End Sub

            Protected Overrides Sub OnMouseMove(ByVal e As MouseEventArgs)
                MyBase.OnMouseMove(e)
                Dim index As Integer = IndexFromPoint(e.Location)
                Debug.WriteLine("Mouse over item: " & If(index >= 0, GetItemText(Items(index)), "None"))
                If index >= 0 AndAlso index <> curSelIndexInternal Then
                    curSelIndexInternal = index
                    SetSelected(index, True)
                End If
            End Sub

        End Class ' end internal class CustomCheckedListBox

        ' --------------------------------------------------------------------------------------------------------

        ' ********************************************* Data *********************************************

        Private ccbParentInternal As CheckedComboBox

        ' Keeps track of whether checked item(s) changed, hence the value of the CheckedComboBox as a whole changed.
        ' This is simply done via maintaining the old string-representation of the value(s) and the new one and comparing them!
        Private oldStrValueInternal As String = ""
        Public ReadOnly Property ValueChanged() As Boolean
            Get
                Dim newStrValue As String = ccbParentInternal.Text
                If oldStrValueInternal.Length > 0 AndAlso newStrValue.Length > 0 Then
                    Return oldStrValueInternal.CompareTo(newStrValue) <> 0
                Else
                    Return oldStrValueInternal.Length <> newStrValue.Length
                End If
            End Get
        End Property

        ' Array holding the checked states of the items. This will be used to reverse any changes if user cancels selection.
        Private checkedStateArrInternal() As Boolean

        ' Whether the dropdown is closed.
        Private dropdownClosedInternal As Boolean = True

        Private cclbInternal As CustomCheckedListBox
        Public Property List() As CustomCheckedListBox
            Get
                Return cclbInternal
            End Get
            Set(ByVal value As CustomCheckedListBox)
                cclbInternal = value
            End Set
        End Property

        ' ********************************************* Construction *********************************************

        Public Sub New(ByVal ccbParent As CheckedComboBox)
            MyBase.New()
            Me.ccbParentInternal = ccbParent
            InitializeComponent()
            Me.ShowInTaskbar = False
            ' Add a handler to notify our parent of ItemCheck events.
            AddHandler Me.cclbInternal.ItemCheck, AddressOf Me.cclb_ItemCheck
        End Sub

        ' ********************************************* Methods *********************************************

        ' Create a CustomCheckedListBox which fills up the entire form area.
        Private Sub InitializeComponent()
            Me.cclbInternal = New CustomCheckedListBox()
            Me.SuspendLayout()
            '
            ' cclb
            '
            Me.cclbInternal.BorderStyle = BorderStyle.None
            Me.cclbInternal.Dock = DockStyle.Fill
            Me.cclbInternal.FormattingEnabled = True
            Me.cclbInternal.Location = New Point(0, 0)
            Me.cclbInternal.Name = "cclb"
            Me.cclbInternal.Size = New Size(47, 15)
            Me.cclbInternal.TabIndex = 0
            '
            ' Dropdown
            '
            Me.AutoScaleDimensions = New SizeF(6.0F, 13.0F)
            Me.AutoScaleMode = AutoScaleMode.Font
            Me.BackColor = SystemColors.Menu
            Me.ClientSize = New Size(47, 16)
            Me.ControlBox = False
            Me.Controls.Add(Me.cclbInternal)
            Me.ForeColor = SystemColors.ControlText
            Me.FormBorderStyle = FormBorderStyle.FixedToolWindow
            Me.MinimizeBox = False
            Me.Name = "ccbParent"
            Me.StartPosition = FormStartPosition.Manual
            Me.ResumeLayout(False)
        End Sub

        Public Function GetCheckedItemsStringValue() As String
            Dim sb As New StringBuilder("")
            For i As Integer = 0 To cclbInternal.CheckedItems.Count - 1
                sb.Append(cclbInternal.GetItemText(cclbInternal.CheckedItems(i))).Append(ccbParentInternal.ValueSeparator)
            Next
            If sb.Length > 0 Then
                sb.Remove(sb.Length - ccbParentInternal.ValueSeparator.Length, ccbParentInternal.ValueSeparator.Length)
            End If
            Return sb.ToString()
        End Function

        ''' <summary>
        ''' Closes the dropdown portion and enacts any changes according to the specified boolean parameter.
        ''' NOTE: even though the caller might ask for changes to be enacted, this doesn't necessarily mean
        ''' that any changes have occurred as such. Caller should check the ValueChanged property of the
        ''' CheckedComboBox (after the dropdown has closed) to determine any actual value changes.
        ''' </summary>
        ''' <param name="enactChanges"></param>
        Public Sub CloseDropdown(ByVal enactChanges As Boolean)
            If dropdownClosedInternal Then
                Return
            End If
            Debug.WriteLine("CloseDropdown")
            ' Perform the actual selection and display of checked items.
            If enactChanges Then
                ccbParentInternal.SelectedIndex = -1
                ' Set the text portion equal to the string comprising all checked items (if any, otherwise empty!).
                ccbParentInternal.Text = GetCheckedItemsStringValue()

            Else
                ' Caller cancelled selection - need to restore the checked items to their original state.
                For i As Integer = 0 To cclbInternal.Items.Count - 1
                    cclbInternal.SetItemChecked(i, checkedStateArrInternal(i))
                Next
            End If
            ' From now on the dropdown is considered closed. We set the flag here to prevent OnDeactivate() calling
            ' this method once again after hiding this window.
            dropdownClosedInternal = True
            ' Set the focus to our parent CheckedComboBox and hide the dropdown check list.
            ccbParentInternal.Focus()
            Me.Hide()
            ' Notify CheckedComboBox that its dropdown is closed. (NOTE: it does not matter which parameters we pass to
            ' OnDropDownClosed() as long as the argument is CCBoxEventArgs so that the method knows the notification has
            ' come from our code and not from the framework).
            ccbParentInternal.OnDropDownClosed(New CCBoxEventArgs(Nothing, False))
        End Sub

        Protected Overrides Sub OnActivated(ByVal e As EventArgs)
            Debug.WriteLine("OnActivated")
            MyBase.OnActivated(e)
            dropdownClosedInternal = False
            ' Assign the old string value to compare with the new value for any changes.
            oldStrValueInternal = ccbParentInternal.Text
            ' Make a copy of the checked state of each item, in cace caller cancels selection.
            checkedStateArrInternal = New Boolean(cclbInternal.Items.Count - 1) {}
            For i As Integer = 0 To cclbInternal.Items.Count - 1
                checkedStateArrInternal(i) = cclbInternal.GetItemChecked(i)
            Next
        End Sub

        Protected Overrides Sub OnDeactivate(ByVal e As EventArgs)
            Debug.WriteLine("OnDeactivate")
            MyBase.OnDeactivate(e)
            Dim ce As CCBoxEventArgs = TryCast(e, CCBoxEventArgs)
            If ce IsNot Nothing Then
                CloseDropdown(ce.AssignValues)
            Else
                ' If not custom event arguments passed, means that this method was called from the
                ' framework. We assume that the checked values should be registered regardless.
                CloseDropdown(True)
            End If
        End Sub

        Private Sub cclb_ItemCheck(ByVal sender As Object, ByVal e As ItemCheckEventArgs)
            'If ccbParentInternal.ItemCheck IsNot Nothing Then
            '    ccbParentInternal.ItemCheck(sender, e)
            'End If
        End Sub

    End Class ' end internal class Dropdown

    ' ******************************** Data ********************************
    ''' <summary>
    ''' Required designer variable.
    ''' </summary>
    Private componentsInternal As System.ComponentModel.IContainer = Nothing
    ' A form-derived object representing the drop-down list of the checked combo box.
    Private dropdownInternal As Dropdown

    ' The valueSeparator character(s) between the ticked elements as they appear in the 
    ' text portion of the CheckedComboBox.
    Private valueSeparatorInternal As String
    Public Property ValueSeparator() As String
        Get
            Return valueSeparatorInternal
        End Get
        Set(ByVal value As String)
            valueSeparatorInternal = value
        End Set
    End Property

    Public Property CheckOnClick() As Boolean
        Get
            Return dropdownInternal.List.CheckOnClick
        End Get
        Set(ByVal value As Boolean)
            dropdownInternal.List.CheckOnClick = value
        End Set
    End Property

    Public Shadows Property DisplayMember() As String
        Get
            Return dropdownInternal.List.DisplayMember
        End Get
        Set(ByVal value As String)
            dropdownInternal.List.DisplayMember = value
        End Set
    End Property

    Public Shadows ReadOnly Property Items() As CheckedListBox.ObjectCollection
        Get
            Return dropdownInternal.List.Items
        End Get
    End Property

    Public ReadOnly Property CheckedItems() As CheckedListBox.CheckedItemCollection
        Get
            Return dropdownInternal.List.CheckedItems
        End Get
    End Property

    Public ReadOnly Property CheckedIndices() As CheckedListBox.CheckedIndexCollection
        Get
            Return dropdownInternal.List.CheckedIndices
        End Get
    End Property

    Public ReadOnly Property ValueChanged() As Boolean
        Get
            Return dropdownInternal.ValueChanged
        End Get
    End Property

    ' Event handler for when an item check state changes.
    Public Event ItemCheck As ItemCheckEventHandler

    ' ******************************** Construction ********************************

    Public Sub New()
        MyBase.New()
        ' We want to do the drawing of the dropdown.
        Me.DrawMode = DrawMode.OwnerDrawVariable
        ' Default value separator.
        Me.ValueSeparator = ", "
        ' This prevents the actual ComboBox dropdown to show, although it's not strickly-speaking necessary.
        ' But including this remove a slight flickering just before our dropdown appears (which is caused by
        ' the empty-dropdown list of the ComboBox which is displayed for fractions of a second).
        Me.DropDownHeight = 1
        ' This is the default setting - text portion is editable and user must click the arrow button
        ' to see the list portion. Although we don't want to allow the user to edit the text portion
        ' the DropDownList style is not being used because for some reason it wouldn't allow the text
        ' portion to be programmatically set. Hence we set it as editable but disable keyboard input (see below).
        Me.DropDownStyle = ComboBoxStyle.DropDown
        Me.dropdownInternal = New Dropdown(Me)
        ' CheckOnClick style for the dropdown (NOTE: must be set after dropdown is created).
        Me.CheckOnClick = True
    End Sub

    ' ******************************** Operations ********************************

    ''' <summary>
    ''' Clean up any resources being used.
    ''' </summary>
    ''' <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso (componentsInternal IsNot Nothing) Then
            componentsInternal.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Protected Overrides Sub OnDropDown(ByVal e As EventArgs)
        MyBase.OnDropDown(e)
        DoDropDown()
    End Sub

    Private Sub DoDropDown()
        If Not dropdownInternal.Visible Then
            Dim rect As Rectangle = RectangleToScreen(Me.ClientRectangle)
            dropdownInternal.Location = New Point(rect.X, rect.Y + Me.Size.Height)
            Dim count As Integer = dropdownInternal.List.Items.Count
            If count > Me.MaxDropDownItems Then
                count = Me.MaxDropDownItems
            ElseIf count = 0 Then
                count = 1
            End If
            dropdownInternal.Size = New Size(Me.Size.Width, (dropdownInternal.List.ItemHeight) * count + 2)
            dropdownInternal.Show(Me)
        End If
    End Sub

    Protected Overrides Sub OnDropDownClosed(ByVal e As EventArgs)
        ' Call the handlers for this event only if the call comes from our code - NOT the framework's!
        ' NOTE: that is because the events were being fired in a wrong order, due to the actual dropdown list
        '       of the ComboBox which lies underneath our dropdown and gets involved every time.
        If TypeOf e Is Dropdown.CCBoxEventArgs Then
            MyBase.OnDropDownClosed(e)
        End If
    End Sub

    Protected Overrides Sub OnKeyDown(ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Down Then
            ' Signal that the dropdown is "down". This is required so that the behaviour of the dropdown is the same
            ' when it is a result of user pressing the Down_Arrow (which we handle and the framework wouldn't know that
            ' the list portion is down unless we tell it so).
            ' NOTE: all that so the DropDownClosed event fires correctly!
            OnDropDown(Nothing)
        End If
        ' Make sure that certain keys or combinations are not blocked.
        e.Handled = Not e.Alt AndAlso Not (e.KeyCode = Keys.Tab) AndAlso
            Not ((e.KeyCode = Keys.Left) OrElse (e.KeyCode = Keys.Right) OrElse (e.KeyCode = Keys.Home) OrElse (e.KeyCode = Keys.End))

        MyBase.OnKeyDown(e)
    End Sub

    Protected Overrides Sub OnKeyPress(ByVal e As KeyPressEventArgs)
        e.Handled = True
        MyBase.OnKeyPress(e)
    End Sub

    Public Function GetItemChecked(ByVal index As Integer) As Boolean
        If index < 0 OrElse index > Items.Count Then
            Throw New ArgumentOutOfRangeException("index", "value out of range")
        Else
            Return dropdownInternal.List.GetItemChecked(index)
        End If
    End Function

    Public Sub SetItemChecked(ByVal index As Integer, ByVal isChecked As Boolean)
        If index < 0 OrElse index > Items.Count Then
            Throw New ArgumentOutOfRangeException("index", "value out of range")
        Else
            dropdownInternal.List.SetItemChecked(index, isChecked)
            ' Need to update the Text.
            Me.Text = dropdownInternal.GetCheckedItemsStringValue()
        End If
    End Sub

    Public Function GetItemCheckState(ByVal index As Integer) As CheckState
        If index < 0 OrElse index > Items.Count Then
            Throw New ArgumentOutOfRangeException("index", "value out of range")
        Else
            Return dropdownInternal.List.GetItemCheckState(index)
        End If
    End Function

    Public Sub SetItemCheckState(ByVal index As Integer, ByVal state As CheckState)
        If index < 0 OrElse index > Items.Count Then
            Throw New ArgumentOutOfRangeException("index", "value out of range")
        Else
            dropdownInternal.List.SetItemCheckState(index, state)
            ' Need to update the Text.
            Me.Text = dropdownInternal.GetCheckedItemsStringValue()
        End If
    End Sub

End Class ' end public class CheckedComboBox

