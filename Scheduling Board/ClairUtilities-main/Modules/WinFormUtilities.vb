Imports System.Windows.Forms

Public Module WinFormUtilities
    ''' <summary>
    ''' Find the innermost active edit control within parent container.
    ''' </summary>
    ''' <param name="parent">The container of the active control</param>
    ''' <returns>the innermost edit control</returns>
    ''' <remarks></remarks>
    <System.Runtime.CompilerServices.Extension()> _
    Public Function ActiveEditControl(ByRef parent As ContainerControl) As Control
        Dim c As Control = parent.ActiveControl
        Return If(TypeOf c Is ContainerControl, ActiveEditControl(c), c)
    End Function
End Module