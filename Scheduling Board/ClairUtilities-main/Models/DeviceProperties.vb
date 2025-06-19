Imports System.Runtime.CompilerServices
Imports System.Threading.Tasks

Public Class DeviceProperty
    Private _partNumber As String
    Private _serialNumber As String
    Private _propertyType As Integer
    Private _sequenceNumber As Integer = -1
    Private _value As String
    Private _note As String = ""
    Private _uniqueNumber As String

    Public Property PartNumber As String
        Get
            Return _partNumber
        End Get
        Set(value As String)
            _partNumber = If(value IsNot DBNull.Value AndAlso value IsNot Nothing, value.ToString, "")
        End Set
    End Property

    Public Property SerialNumber As String
        Get
            Return _serialNumber
        End Get
        Set(value As String)
            _serialNumber = If(value IsNot DBNull.Value AndAlso value IsNot Nothing, value.ToString, "")
        End Set
    End Property

    Public Property PropertyType As Integer?
        Get
            Return _propertyType
        End Get
        Set(value As Integer?)
            If (value Is Nothing) Then
                Throw New ArgumentNullException("PropertyType can't be DBNULL")
            End If
            _propertyType = value
        End Set
    End Property

    Public Property SequenceNumber As Integer?
        Get
            Return _sequenceNumber
        End Get
        Set(value As Integer?)
            _sequenceNumber = value
        End Set
    End Property

    Public Property Value As String
        Get
            Return _value
        End Get
        Set(value As String)
            _value = If(value IsNot DBNull.Value AndAlso value IsNot Nothing, value.ToString, "")
        End Set
    End Property

    Public Property Note As String
        Get
            Return _note
        End Get
        Set(value As String)
            _note = If(value IsNot DBNull.Value AndAlso value IsNot Nothing, value.ToString, "")
        End Set
    End Property

    Public Property UniqueNumber As String
        Get
            Return _uniqueNumber
        End Get
        Set(value As String)
            _uniqueNumber = If(value IsNot DBNull.Value AndAlso value IsNot Nothing, value.ToString, "")
        End Set
    End Property

    Public Async Sub Save()
        Try
            Using conn = New SqlConnection(FinesseConnectionString)
                Await conn.ExecuteStoredProcedureAsync("dbo.Add_Device_Property", {
                "@PartNumber", PartNumber,
                "@SerialNumber", SerialNumber,
                "@PropertyType", PropertyType,
                "@Value", Value,
                "@Note", Note,
                "@SequenceNumber", SequenceNumber,
                "@UniqueNumber", UniqueNumber
                })
            End Using
        Catch ex As Exception
            Throw New Exception($"Error saving Device Property. {Environment.NewLine} {ex.Message}")
        End Try
    End Sub
End Class

Module BulkSaveExtension
    <Extension()>
    Public Async Function BulkSave(ByVal devices As IEnumerable(Of DeviceProperty)) As Task
        Try
            Using conn = New SqlConnection(FinesseConnectionString)
                Await conn.OpenAsync()
                For Each item In devices
                    With item
                        Await conn.ExecuteStoredProcedureAsync("dbo.Add_Device_Property", {
                                "@PartNumber", .PartNumber,
                                "@SerialNumber", .SerialNumber,
                                "@PropertyType", .PropertyType,
                                "@Value", .Value,
                                "@Note", .Note,
                                "@SequenceNumber", .SequenceNumber,
                                "@UniqueNumber", .UniqueNumber
                            }, autoClose:=False)
                    End With
                Next
            End Using
        Catch ex As Exception
            Console.WriteLine(ex.Message)
            Throw New Exception($"Error saving Device Property. {Environment.NewLine} {ex.Message}")
        End Try
    End Function
End Module
