Public Class MultiLevelTaskHelper
    ''' <summary>
    ''' Raised when there are no more tasks in progress
    ''' </summary>
    ''' <param name="helper">the MultiLevelTaskHelper which has no more tasks</param>
    ''' <remarks></remarks>
    Public Event AllDone(ByVal helper As MultiLevelTaskHelper)

    Private myCount As Integer = 0

    ''' <summary>
    ''' Begins a new task
    ''' </summary>
    ''' <returns>an opaque IDisposable object; when all objects returned by this class have 
    ''' been Dispose()d of, the AllDone event will be raised.</returns>
    ''' <remarks>This class should be used with the pattern:
    ''' Using myTaskHelper.StartTask
    ''' ' ...
    ''' End Using</remarks>
    Public Function StartTask() As IDisposable
        myCount += 1
        Return New TaskInProgress(Me)
    End Function

    ''' <summary>
    ''' the number of task levels currently in progress
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Level() As Integer
        Get
            Level = myCount
        End Get
    End Property

    ''' <summary>
    ''' whether there are any tasks in progress
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property TasksRunning() As Boolean
        Get
            TasksRunning = (myCount >= 1)
        End Get
    End Property



    Protected Sub NotifyDestroyed()
        myCount -= 1
        Debug.Assert(myCount >= 0)

        If myCount = 0 Then
            RaiseEvent AllDone(Me)
        End If
    End Sub


    Private Class TaskInProgress
        Implements IDisposable

        Private _TaskOwner As MultiLevelTaskHelper

        Sub New(ByRef helper As MultiLevelTaskHelper)
            _TaskOwner = helper
        End Sub

        Sub Dispose() Implements IDisposable.Dispose
            _TaskOwner.NotifyDestroyed()
            _TaskOwner = Nothing
        End Sub

        Protected Overrides Sub Finalize()
            If _TaskOwner IsNot Nothing Then
                Me.Dispose()
            End If

            MyBase.Finalize()
        End Sub
    End Class

End Class
