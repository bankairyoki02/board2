Imports System.Data
Imports System.Data.SqlClient
Imports System.Environment
Imports System.Runtime.InteropServices

Module SchedulingBoardGeneral
    <DllImport("user32.dll", SetLastError:=True)> Private Function SetForegroundWindow(ByVal hWnd As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)> Private Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function

    Public Function EmptyStringToNull(ByVal arg1 As String) As Object
        Dim varOut As Object
        varOut = Nothing
        If arg1 <> vbNullString Then
            varOut = arg1
        End If
        EmptyStringToNull = varOut
    End Function
End Module
