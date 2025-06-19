Imports System.Runtime.InteropServices
Imports System.Threading

Public Class AppUtilities
    Private Const ATTACH_PARENT_PROCESS As Integer = -1

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function AllocConsole() As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function AttachConsole(dwProcessId As Integer) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function FreeConsole() As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function SetConsoleCtrlHandler(ByVal handlerRoutine As ConsoleCtrlDelegate, ByVal add As Boolean) As Boolean
    End Function

    Private Delegate Function ConsoleCtrlDelegate(ByVal ctrlType As CtrlTypes) As Boolean

    Private Enum CtrlTypes
        CTRL_C_EVENT = 0
        CTRL_BREAK_EVENT = 1
        CTRL_CLOSE_EVENT = 2
        CTRL_LOGOFF_EVENT = 5
        CTRL_SHUTDOWN_EVENT = 6
    End Enum

    Private Shared Function ConsoleCtrlHandler(ByVal ctrlType As CtrlTypes) As Boolean
        If ctrlType = CtrlTypes.CTRL_CLOSE_EVENT Then
            FreeConsole()
            While True
                Thread.Sleep(1000)
                'The app is killed either way when closing the cmd
            End While
            Return True
        End If

        Return False
    End Function


    ''' <summary>
    ''' Show an output console, showing all the Console.Write/WriteLine being printed in real time
    ''' </summary>
    Public Shared Sub ShowCommandOutput()
        Try
            AllocConsole()

            Dim consoleCtrlHandlerdim As New ConsoleCtrlDelegate(AddressOf ConsoleCtrlHandler)
            SetConsoleCtrlHandler(consoleCtrlHandlerdim, True)

            Console.WriteLine("Initializing the console...")
            Console.WriteLine("Events happening after opening the console will be shown below...")
            AttachConsole(ATTACH_PARENT_PROCESS)
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Assuming the command arguments are supplied as follow: "partno=123456 barcode=123456" Or "partno:123456 barcode:123456"
    ''' It will Trim by empty space and take the key values pair
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function GetCommandLineArgs() As Dictionary(Of String, String)
        Dim keyValuePairs As New Dictionary(Of String, String)()

        If My.Application.CommandLineArgs.Count > 0 Then
            For Each arg As String In My.Application.CommandLineArgs
                Dim multiplekeys = arg.Split(" "c)
                For Each keys In multiplekeys
                    Dim parts() As String = keys.Split("="c)
                    If (parts.Length = 0) Then
                        parts = keys.Split(":"c)
                    End If

                    If parts.Length = 2 Then
                        Dim key As String = parts(0).Trim()
                        Dim value As String = parts(1).Trim()
                        keyValuePairs(key) = value
                    Else
                        Console.WriteLine($"Invalid argument format: {arg}")
                    End If
                Next
            Next
        Else
            Console.WriteLine($"No command-line arguments provided.")
        End If
        Return keyValuePairs
    End Function

    ''' <summary>
    ''' Will Search the command line Args by the supplied key if exists will return the value
    ''' <seealso cref="GetCommandLineArgs()"/>
    ''' </summary>
    ''' <param name="key"></param>
    ''' <returns>The value if found else Nothing</returns>
    Public Shared Function GetCommandArgValue(key As String) As String
        Dim args = GetCommandLineArgs()
        Dim value As String = Nothing
        If args.ContainsKey(key) Then
            value = args(key)
        End If
        Return value
    End Function
End Class
