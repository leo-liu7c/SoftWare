Option Explicit On
Option Strict On

Imports System.IO
Imports System.Net
Imports System.Net.Sockets



Public Class cTCPServer
    '+-----------------------------------------------------------------------------+
    '|                             Public declarations                             |
    '+-----------------------------------------------------------------------------+
    ' Public enumerations
    Public Enum EStatus
        Disconnected = 0
        Listening = 1
        Connected = 2
    End Enum



    '+-----------------------------------------------------------------------------+
    '|                            Private declarations                             |
    '+-----------------------------------------------------------------------------+
    ' Public constants
    Private Const _rxBufferSize = 1024

    ' Private variables
    Private _client As TcpClient = Nothing
    Private _lastReceiveDate As Date
    Private _lastTransmitDate As Date
    Private _listener As TcpListener
    Private _lock As New Object
    Private _localPortNumber As Integer = 0
    Private _status As EStatus = EStatus.Disconnected
    Private _stream As NetworkStream = Nothing



    '+-----------------------------------------------------------------------------+
    '|                                 Properties                                  |
    '+-----------------------------------------------------------------------------+
    Public ReadOnly Property LastReceiveDate As Date
        Get
            ' Return the last receive date
            LastReceiveDate = _lastReceiveDate
        End Get
    End Property



    Public ReadOnly Property LastTransmitDate As Date
        Get
            ' Return the last transmit date
            LastTransmitDate = _lastTransmitDate
        End Get
    End Property



    Public ReadOnly Property Status As EStatus
        Get
            ' Return the current status
            Status = _status
        End Get
    End Property



    '+-----------------------------------------------------------------------------+
    '|                               Public methods                                |
    '+-----------------------------------------------------------------------------+
    Public Sub Close()
        SyncLock (_lock)
            If (_status = EStatus.Listening) Then
                ' Stop and destroy the listener
                _listener.Stop()
                _listener = Nothing
                ' Status = disconnected
                _status = EStatus.Disconnected
            ElseIf (_status = EStatus.Connected) Then
                ' Close and destroy the stream
                _stream.Close()
                _stream = Nothing
                ' Close and destroy the client
                _client.Close()
                _client = Nothing
                ' Status = disconnected
                _status = EStatus.Disconnected
            End If
        End SyncLock
    End Sub



    Public Function Listen(ByVal localIPAddress As String, _
                           ByVal localPortNumber As Integer) As Boolean
        SyncLock (_lock)
            If (_status = EStatus.Disconnected) Then
                Try
                    ' Store the local port number
                    _localPortNumber = localPortNumber
                    ' Create, start and begin accepting incoming connections
                    _listener = New TcpListener(IPAddress.Parse(localIPAddress), localPortNumber)
                    _listener.Start(_localPortNumber)
                    _listener.BeginAcceptTcpClient(New AsyncCallback(AddressOf AcceptCallback), _listener)
                    ' Status = listening
                    _status = EStatus.Listening
                    ' Return False
                    Listen = False
                Catch ex As Exception
                    ' Return True
                    Listen = True
                End Try
            Else
                ' Return True
                Listen = True
            End If
        End SyncLock
    End Function



    Public Function Receive(ByRef rxString As String) As Boolean
        Dim n As Integer
        Dim rxBuffer(0 To _rxBufferSize - 1) As Byte

        Try
            ' If there are data available
            If (_stream.DataAvailable) Then
                ' Read the incoming data
                n = _stream.Read(rxBuffer, 0, _rxBufferSize)
                ' Convert the incoming data from byte to string
                rxString = System.Text.Encoding.ASCII.GetString(rxBuffer, 0, n)
                ' Store the last receive date
                _lastReceiveDate = Date.Now
            Else
                rxString = ""
            End If
            ' Return False
            Receive = False
        Catch ex As Exception
            ' Return True
            Receive = True
        End Try
    End Function



    Public Function Transmit(ByVal txString As String) As Boolean
        Dim txBuffer() As Byte

        Try
            ' Convert the outgoing data from string to byte
            txBuffer = System.Text.Encoding.ASCII.GetBytes(txString)
            ' Write the outgoing data
            _stream.Write(txBuffer, 0, txBuffer.Length)
            ' Store the last transmit date 
            _lastTransmitDate = Date.Now
            ' Return False
            Transmit = False

        Catch ex As Exception
            ' Return True
            Transmit = True
        End Try
    End Function



    '+-----------------------------------------------------------------------------+
    '|                               Private methods                               |
    '+-----------------------------------------------------------------------------+
    Private Sub AcceptCallback(ByVal ar As IAsyncResult)
        Try
            SyncLock (_lock)
                ' End accepting incoming connections
                _client = _listener.EndAcceptTcpClient(ar)
                ' Get the communication stream
                _stream = _client.GetStream
                ' Stop and destroy the listener
                _listener.Stop()
                _listener = Nothing
                ' Store the last receive date
                _lastReceiveDate = Date.Now
                ' Status = connected
                _status = EStatus.Connected
            End SyncLock
        Catch ex As Exception

        End Try
    End Sub
End Class
