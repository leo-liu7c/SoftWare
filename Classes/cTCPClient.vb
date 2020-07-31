Option Explicit On
Option Strict On

Imports System.IO
Imports System.Net
Imports System.Net.Sockets

Public Class cTCPClient
    '+-----------------------------------------------------------------------------------+
    '|                                Public declarations                                |
    '+-----------------------------------------------------------------------------------+



    '+-----------------------------------------------------------------------------------+
    '|                               Private declarations                                |
    '+-----------------------------------------------------------------------------------+
    ' Private constants
    Private Const _rxBufferSize = 1024

    ' Private variabled
    Private _client As New TcpClient
    Private _lastReceiveDate As Date
    Private _lastTransmitDate As Date
    Private _lock As New Object
    Private _remoteIPAddress As String
    Private _remotePortNumber As Integer
    Private _stream As NetworkStream



    '+-----------------------------------------------------------------------------------+
    '|                                    Properties                                     |
    '+-----------------------------------------------------------------------------------+
    Public ReadOnly Property Connected As Boolean
        Get
            ' Return True if the client is connected, False otherwise
            Connected = (_client IsNot Nothing AndAlso _client.Connected)
        End Get
    End Property



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



    '+-----------------------------------------------------------------------------------+
    '|                                  Public methods                                   |
    '+-----------------------------------------------------------------------------------+
    Public Function Connect(ByVal remoteIPAddress As String, _
                            ByVal remotePortNumber As Integer) As Boolean
        ' Enable the error handling
        Try
            ' Close and destroy the stream
            If (_stream IsNot Nothing) Then
                _stream.Close()
                _stream = Nothing
            End If
            ' Close and destroy the client
            If (_client IsNot Nothing) Then
                _client.Close()
                _client = Nothing
            End If
            ' Store the remote IP address and port number
            _remoteIPAddress = remoteIPAddress
            _remotePortNumber = remotePortNumber
            ' Create the client
            _client = New TcpClient
            ' Connect to the server
            _client.Connect(_remoteIPAddress, _remotePortNumber)
            ' Get the network stream
            _stream = _client.GetStream
            ' Return False
            Connect = False

        Catch ex As Exception
            ' Return True
            Connect = True
        End Try
    End Function
    Public Function ReConnect() As Boolean
        ' Enable the error handling
        Try
            ' Close and destroy the stream
            If (_stream IsNot Nothing) Then
                _stream.Close()
                _stream = Nothing
            End If
            ' Close and destroy the client
            If (_client IsNot Nothing) Then
                _client.Close()
                _client = Nothing
            End If
            ' Create the client
            _client = New TcpClient
            ' Connect to the server
            _client.Connect(_remoteIPAddress, _remotePortNumber)
            ' Get the network stream
            _stream = _client.GetStream
            ' Return False
            ReConnect = False

        Catch ex As Exception
            ' Return True
            ReConnect = True
        End Try
    End Function



    Public Sub Disconnect()
        ' Close and destroy the stream
        If (_stream IsNot Nothing) Then
            _stream.Close()
            _stream = Nothing
        End If
        ' Close and destroy the client
        If (_client IsNot Nothing) Then
            _client.Close()
            _client = Nothing
        End If
    End Sub



    Public Function Receive(ByRef rxString As String) As Boolean
        Dim byteCount As Integer
        Dim rxBuffer(0 To _rxBufferSize - 1) As Byte

        Try
            ' If there are data available
            If (_stream.DataAvailable) Then
                ' Read the incoming data
                byteCount = _stream.Read(rxBuffer, 0, _rxBufferSize)
                ' Convert the incoming data to string
                rxString = System.Text.Encoding.ASCII.GetString(rxBuffer, 0, byteCount)
                ' Update the last receive date
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
            ' Convert the string to the outgoing data
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



    '+-----------------------------------------------------------------------------------+
    '|                                  Private methods                                  |
    '+-----------------------------------------------------------------------------------+
End Class
