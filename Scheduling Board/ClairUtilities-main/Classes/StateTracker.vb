Imports SchedulingBoard

Public Class UndoRedoState(Of T)
    Private UndoStack As Stack(Of T)
    Private RedoStack As Stack(Of T)

    Public CurrentState As T
    Protected WithEvents _btnBack As Button
    Protected WithEvents _btnForward As Button

    Public is_changing As Boolean
    Public is_refreshing As Boolean

    Public Sub New(backBtn As Button, forwardBtn As Button)
        UndoStack = New Stack(Of T)
        RedoStack = New Stack(Of T)

        _btnBack = backBtn
        _btnForward = forwardBtn
    End Sub

    Public Sub Clear()
        UndoStack.Clear()
        RedoStack.Clear()
        CurrentState = Nothing
    End Sub

    Public Sub AddState(ByVal state As T)
        If CurrentState IsNot Nothing Then UndoStack.Push(CurrentState)
        CurrentState = state
        RedoStack.Clear()
        CheckButtonStates()
    End Sub

    Public Sub Undo()
        _btnBack.Enabled = False
        _btnForward.Enabled = False
        RedoStack.Push(CurrentState)
        CurrentState = UndoStack.Pop()
    End Sub

    Public Sub Redo()
        _btnBack.Enabled = False
        _btnForward.Enabled = False
        UndoStack.Push(CurrentState)
        CurrentState = RedoStack.Pop
    End Sub

    Public Function CanUndo() As Boolean
        Return UndoStack.Count > 0
    End Function

    Public Function CanRedo() As Boolean
        Return RedoStack.Count > 0
    End Function

    Public Sub CheckButtonStates()
        _btnBack.Enabled = CanUndo()
        _btnForward.Enabled = CanRedo()
    End Sub

    Public Function UndoStates() As List(Of T)
        Return UndoStack.ToList
    End Function

    Public Function RedoStates() As List(Of T)
        Return RedoStack.ToList
    End Function
End Class

Public Class StateTracker
    Inherits UndoRedoState(Of Dictionary(Of String, Object))

    Protected _form As Form
    Protected _stateList As List(Of String)

    Public Delegate Sub StateHandler(sender As Button)
    Public Event OnRefresh As StateHandler

    Sub New(form As Form, back As Button, forward As Button, stateList As List(Of String))
        MyBase.New(back, forward)
        _form = form
        _stateList = stateList
        SnapShot()
    End Sub

    Public Sub SnapShot()
        Dim props As New Dictionary(Of String, Object)

        For Each ctlKey As String In _stateList
            Dim ctlValue As New Object()
            ctlValue = GetPropValue(ctlKey)
            props.Add(ctlKey, ctlValue)
        Next

        AddState(props)
    End Sub

    Function GetPropValue(ctlKey As String) As Object
        Dim ctlinfo = Split(ctlKey, ".")
        Dim ctl = _form.Controls.Find(ctlinfo(0), True).FirstOrDefault()
        Dim ctlProp = ctl.GetType().GetProperties.Where(Function(f) f.Name = ctlinfo(1)).First()
        Dim propValue = ctlProp.GetValue(ctl, Nothing)

        Select Case ctlProp.PropertyType().Name
            Case "ObjectCollection", "CheckedIndexCollection"
                Return TryCast(propValue, IEnumerable).Cast(Of Object).ToList()
            Case Else
                Return ctlProp.GetValue(ctl, Nothing)
        End Select
    End Function

    Sub SetList(ctl As ListBox, ByVal values As List(Of Object))
        If ctl Is Nothing OrElse values Is Nothing Then Return
        Try
            ctl.Items.Clear()
        Catch ex As Exception

        End Try
        For Each item In values
            ctl.Items.Add(item)
        Next
    End Sub

    Sub SetProp(prop As KeyValuePair(Of String, Object))
        Dim ctlinfo = Split(prop.Key, ".")
        Dim ctlName = ctlinfo(0)
        Dim propName = ctlinfo(1)
        Dim ctl = _form.Controls.Find(ctlName, True).FirstOrDefault()
        Dim ctlProp = ctl.GetType().GetProperties.Where(Function(f) f.Name = propName).FirstOrDefault()

        'set changed controls

        Select Case ctlProp.PropertyType().Name
            Case "ObjectCollection"
                If propName = "Items" Then TryCast(ctl, ListBox).Items.Clear()
                SetList(ctl, prop.Value)
            Case "CheckedIndexCollection"
                CheckList(ctl, prop.Value)
            Case Else
                ctlProp.SetValue(ctl, prop.Value, Nothing)
        End Select

        ctl.Update()
    End Sub

    Sub SetState(newProps As Dictionary(Of String, Object), oldProps As Dictionary(Of String, Object))
        is_changing = True
        Dim diffs = newProps.Except(oldProps)
        For Each prop As KeyValuePair(Of String, Object) In diffs
            SetProp(prop)
        Next
        is_changing = False
    End Sub

    Sub CheckList(ctl As CheckedListBox, ByVal values As List(Of Object))
        ' check all items in a list
        For Each index In values
            ctl.SetItemChecked(index, True)
        Next
    End Sub

    Sub Change()
        If IsNothing(CurrentState) Or is_changing Or is_refreshing Then Exit Sub
        is_changing = True
        SnapShot()
        is_changing = False
    End Sub

    Function GetProp(ctlname As String, propName As String) As Object
        Try
            Return CurrentState(ctlname & "." & propName)
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Sub Undo_click(sender As Object, e As EventArgs) Handles _btnBack.Click
        Undo()
        SetState(CurrentState, RedoStates.Last())
        is_refreshing = True
        RaiseEvent OnRefresh(sender)
        is_refreshing = False
        CheckButtonStates()
    End Sub

    Sub Redo_Click(sender As Object, args As EventArgs) Handles _btnForward.Click
        Redo()
        SetState(CurrentState, UndoStates.Last())
        is_refreshing = True
        RaiseEvent OnRefresh(sender)
        is_refreshing = False
        CheckButtonStates()
    End Sub

End Class