Imports System.Windows
Imports System.Windows.Forms
Imports System.Runtime.CompilerServices


Public Module ControlLocation

    Public Sub MaybePositionFormOverControl(ByRef frm As Form, ByRef ctrl As Control)
        If ctrl Is Nothing Or frm Is Nothing Then
            Exit Sub
        End If

        Dim newLocation = ctrl.PointToScreen(Drawing.Point.Empty)
        newLocation.Offset(-frm.Width / 4, ctrl.Height)

        EnsureOnScreen(newLocation, frm.Size)

        frm.Location = newLocation
    End Sub

    Public Sub MaybePositionFormOverToolStripItem(ByRef frm As Form, ByRef tsItem As ToolStripItem)
        If tsItem Is Nothing Or frm Is Nothing Then
            Exit Sub
        End If

        Dim newLocation = tsItem.GetCurrentParent.PointToScreen(tsItem.Bounds.Location)
        newLocation.Offset(-frm.Width / 4, tsItem.Height)

        EnsureOnScreen(newLocation, frm.Size)

        frm.Location = newLocation
    End Sub

    Public Sub GetControlAbsoluteScreenPosition(ByVal ctrl As Control, ByRef x As Single, ByRef y As Single)
        Dim Location = ctrl.PointToScreen(Drawing.Point.Empty)
        x = Location.X
        y = Location.Y
    End Sub

    Public Sub EnsureOnScreen(ByRef location As System.Drawing.Point, ByRef Size As System.Drawing.Size)
        Dim x As Single, y As Single

        x = location.X
        y = location.Y

        Dim rc = System.Windows.Forms.Screen.GetWorkingArea(location)

        Dim screenTop = rc.Top
        Dim screenBottom = rc.Bottom
        Dim screenLeft = rc.Left
        Dim screenRight = rc.Right

        If y + Size.Height > screenBottom Then
            y = screenBottom - Size.Height
        End If

        If x + Size.Width > screenRight Then
            x = screenRight - Size.Width
        End If

        If x < screenLeft Then
            x = screenLeft
        End If

        If y < screenTop Then
            y = screenTop
        End If

        If x + Size.Width > screenRight Then
            Size.Width = screenRight - screenLeft
        End If

        If y + Size.Height > screenBottom Then
            Size.Height = screenBottom - screenTop
        End If

        location.X = x
        location.Y = y
    End Sub

    Public Sub EnsureOnScreen(ByVal frm As Form)
        EnsureOnScreen(frm.Location, frm.Size)
    End Sub

    <Extension>
    Public Sub ApplyWindowSizeAndState(frm As Form, location As Drawing.Point, size As Drawing.Size, windowState As FormWindowState)
        If frm.InvokeRequired Then
            frm.Invoke(Sub() ApplyWindowSizeAndState(frm, location, size, windowState))
            Exit Sub
        End If

        Dim formBounds As New Rectangle(location, size)
        Dim titlebarHeight As Integer = frm.Height - frm.ClientSize.Height - Windows.SystemParameters.BorderWidth ' references PresentationFramework
        Dim titlebarBounds As New Rectangle(location, New Drawing.Size(size.Width, titlebarHeight))

        Dim screenOverlaps =
            From s In System.Windows.Forms.Screen.AllScreens
            Where s.Bounds.IntersectsWith(formBounds)
            Select Bounds = Rectangle.Intersect(s.Bounds, formBounds),
                ContainsTitlebar = s.Bounds.IntersectsWith(titlebarBounds),
                Screen = s
            Select Bounds, ContainsTitlebar, Screen, Area = Bounds.Width * Bounds.Height
            Order By Area Descending

        If Not screenOverlaps.Any Then
            Exit Sub
        End If

        Dim primaryScreenBounds = screenOverlaps.First.Screen.Bounds

        If windowState = FormWindowState.Maximized Then
            frm.Location = primaryScreenBounds.Location
            frm.WindowState = FormWindowState.Maximized
        Else
            If Not screenOverlaps.Any(Function(o) o.ContainsTitlebar) Then
                Exit Sub
            End If

            ' make sure that a reasonable amount of the form is on the screen.
            If formBounds.Width * formBounds.Height * 0.3 > screenOverlaps.Sum(Function(s) s.Area) Then
                Exit Sub
            End If

            frm.SetBounds(location.X, location.Y, size.Width, size.Height)
            frm.WindowState = FormWindowState.Normal ' ignore requests for minimized windows
        End If

    End Sub

End Module
