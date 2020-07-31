Option Explicit On
Option Strict On

Imports System
Imports System.IO
Imports System.Math
Imports System.Net
Imports System.Net.Sockets



Public Class cVIPA
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Public constants
    Public Const DefaultPortNumber = 502



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _answerTimeout_s = 1
    Private Const _bufferSize = 262
    Private Const _dataSize = 254
    Private Const _protocolIdentifier = &H0
    Private Const _transactionIdentifier = &H0

    ' Function codes
    Private Enum FunctionCode
        ReadOutputBits = &H1
        ReadInputBits = &H2
        ReadOutputWords = &H3
        ReadInputWords = &H4
        WriteOutputBits = &HF
        WriteOutputWords = &H10
    End Enum

    ' Private variables
    Private _client As TcpClient
    Private _connected As Boolean
    Private _IPAddress As String
    Private _portNumber As Integer
    Private _slaveAddress As Byte
    Private _stream As NetworkStream



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+
    Protected Overrides Sub Finalize()
        ' Disconnect the device
        Me.Disconnect()
        ' Finalize the object
        MyBase.Finalize()
    End Sub



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function Connect(ByVal IPAddress As String, _
                            ByVal portNumber As Integer, _
                            Optional ByVal slaveAddress As Byte = 0) As Boolean
        ' Store the IP address, the port number and the slave address
        _IPAddress = IPAddress
        _portNumber = portNumber
        _slaveAddress = slaveAddress
        Try
            ' Create the client and try to connect
            _client = New TcpClient
            _client.Connect(_IPAddress, _portNumber)
            _stream = _client.GetStream
            ' Set the flag on slave connected
            _connected = True
            ' Return False
            Connect = False

        Catch ex As Exception
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
            ' Clear the flag on slave connected
            _connected = False
            ' Return True
            Connect = True
        End Try
    End Function



    Public Function Disconnect() As Boolean
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
            ' Clear the flag on slave connected
            _connected = False
            ' Return False
            Disconnect = False

        Catch ex As Exception
            ' Return True
            Disconnect = True
        End Try
    End Function



    Public Function ReadInputBits(ByVal startBit As Integer, _
                                  ByVal bitCount As Integer, _
                                  ByRef bitValue() As Boolean) As Boolean
        Dim i As Integer
        Dim rxData(0 To _dataSize - 1) As Byte
        Dim rxDataCount As Integer
        Dim txData(0 To _dataSize - 1) As Byte

        ' If the device is connected
        If (_connected) Then
            ' Build the tx data
            txData(0) = CByte((startBit \ 256) And &HFF)
            txData(1) = CByte(startBit And &HFF)
            txData(2) = CByte((bitCount \ 256) And &HFF)
            txData(3) = CByte(bitCount And &HFF)
            ' Run the command
            ReadInputBits = RunCommand(FunctionCode.ReadInputBits, txData, 4, rxData, rxDataCount)
            ' Verify the rx data
            ReadInputBits = ReadInputBits Or (rxData(0) <> Ceiling(bitCount / 8))
            ' Process the rx data
            If Not (ReadInputBits) Then
                For i = 0 To bitCount - 1
                    bitValue(i) = ((rxData(1 + i \ 8) And CByte(2 ^ (i Mod 8))) <> 0)
                Next
            End If
        Else    ' Otherwise, if the device is not connected
            ' Return True
            ReadInputBits = True
        End If
    End Function



    Public Function ReadOutputBits(ByVal startBit As Integer, _
                                   ByVal bitCount As Integer, _
                                   ByRef bitValue() As Boolean) As Boolean
        Dim i As Integer
        Dim rxData(0 To _dataSize - 1) As Byte
        Dim rxDataCount As Integer
        Dim txData(0 To _dataSize - 1) As Byte

        ' If the device is connected
        If (_connected) Then
            ' Build the tx data
            txData(0) = CByte((startBit \ 256) And &HFF)
            txData(1) = CByte(startBit And &HFF)
            txData(2) = CByte((bitCount \ 256) And &HFF)
            txData(3) = CByte(bitCount And &HFF)
            ' Run the command
            ReadOutputBits = RunCommand(FunctionCode.ReadOutputBits, txData, 4, rxData, rxDataCount)
            ' Verify the rx data
            ReadOutputBits = ReadOutputBits Or (rxData(0) <> Ceiling(bitCount / 8))
            ' Process the rx data
            If Not (ReadOutputBits) Then
                For i = 0 To bitCount - 1
                    bitValue(i) = ((rxData(1 + i \ 8) And CByte(2 ^ (i Mod 8))) <> 0)
                Next
            End If
        Else    ' Otherwise, if the device is not connected
            ' Return True
            ReadOutputBits = True
        End If
    End Function



    Public Function ReadInputWords(ByVal startWord As Integer, _
                                   ByVal wordCount As Integer, _
                                   ByVal wordValue() As Integer) As Boolean
        Dim i As Integer
        Dim rxData(0 To _dataSize - 1) As Byte
        Dim rxDataCount As Integer
        Dim txData(0 To _dataSize - 1) As Byte

        ' If the device is connected
        If (_connected) Then
            ' Build the tx data
            txData(0) = CByte((startWord \ 256) And &HFF)
            txData(1) = CByte(startWord And &HFF)
            txData(2) = CByte((wordCount \ 256) And &HFF)
            txData(3) = CByte(wordCount And &HFF)
            ' Run the command
            ReadInputWords = RunCommand(FunctionCode.ReadInputWords, txData, 4, rxData, rxDataCount)
            ' Verify the rx data
            ReadInputWords = ReadInputWords Or (rxData(0) <> wordCount * 2)
            ' Process the rx data
            If Not (ReadInputWords) Then
                For i = 0 To wordCount - 1
                    wordValue(i) = rxData(1 + 2 * i) * 256 + rxData(2 + 2 * i)
                    If (wordValue(i) > 32767) Then
                        wordValue(i) = wordValue(i) - 65536
                    End If
                Next
            End If
        Else    ' Otherwise, if the device is not connected
            ' Return True
            ReadInputWords = True
        End If
    End Function



    Public Function ReadOutputWords(ByVal startWord As Integer, _
                                    ByVal wordCount As Integer, _
                                    ByVal wordValue() As Integer) As Boolean
        Dim i As Integer
        Dim rxData(0 To _dataSize - 1) As Byte
        Dim rxDataCount As Integer
        Dim txData(0 To _dataSize - 1) As Byte

        ' If the device is connected
        If (_connected) Then
            ' Build the tx data
            txData(0) = CByte((startWord \ 256) And &HFF)
            txData(1) = CByte(startWord And &HFF)
            txData(2) = CByte((wordCount \ 256) And &HFF)
            txData(3) = CByte(wordCount And &HFF)
            ' Run the command
            ReadOutputWords = RunCommand(FunctionCode.ReadOutputWords, txData, 4, rxData, rxDataCount)
            ' Verify the rx data
            ReadOutputWords = ReadOutputWords Or (rxData(0) <> wordCount * 2)
            ' Process the rx data
            If Not (ReadOutputWords) Then
                For i = 0 To wordCount - 1
                    wordValue(i) = rxData(1 + 2 * i) * 256 + rxData(2 + 2 * i)
                Next
            End If
        Else    ' Otherwise, if the device is not connected
            ' Return True
            ReadOutputWords = True
        End If
    End Function



    Public Function WriteOutputBits(ByVal startBit As Integer, _
                                    ByVal bitCount As Integer, _
                                    ByVal bitValue() As Boolean) As Boolean
        Dim i As Integer
        Dim rxData(0 To _dataSize - 1) As Byte
        Dim rxDataCount As Integer
        Dim txData(0 To _dataSize - 1) As Byte

        ' If the device is connected
        If (_connected) Then
            ' Build the tx data
            txData(0) = CByte((startBit \ 256) And &HFF)
            txData(1) = CByte(startBit And &HFF)
            txData(2) = CByte((bitCount \ 256) And &HFF)
            txData(3) = CByte(bitCount And &HFF)
            txData(4) = CByte(Ceiling(bitCount / 8))
            For i = 0 To bitCount - 1
                If (bitValue(i)) Then
                    txData(5 + i \ 8) = txData(5 + i \ 8) Or CByte(2 ^ (i Mod 8))
                End If
            Next
            ' Run the command
            WriteOutputBits = RunCommand(FunctionCode.WriteOutputBits, txData, 5 + CInt(Ceiling(bitCount / 8)), rxData, rxDataCount)
            ' Verify the rx data
            WriteOutputBits = WriteOutputBits Or _
                              (rxData(0) <> txData(0) Or rxData(1) <> txData(1)) Or _
                              (rxData(2) <> txData(2) Or rxData(3) <> txData(3))
        Else    ' Otherwise, if the device is not connected
            ' Return True
            WriteOutputBits = True
        End If
    End Function



    Public Function WriteOutputWords(ByVal startWord As Integer, _
                                     ByVal wordCount As Integer, _
                                     ByVal wordValue() As Integer) As Boolean
        Dim i As Integer
        Dim rxData(0 To _dataSize - 1) As Byte
        Dim rxDataCount As Integer
        Dim txData(0 To _dataSize - 1) As Byte

        ' If the device is connected
        If (_connected) Then
            ' Build the tx data
            txData(0) = CByte((startWord \ 256) And &HFF)
            txData(1) = CByte(startWord And &HFF)
            txData(2) = CByte((wordCount \ 256) And &HFF)
            txData(3) = CByte(wordCount And &HFF)
            txData(4) = CByte(wordCount * 2)
            For i = 0 To wordCount - 1
                txData(5 + i * 2) = CByte((wordValue(i) \ 256) And &HFF)
                txData(6 + i * 2) = CByte(wordValue(i) And &HFF)
            Next
            ' Run the command
            WriteOutputWords = RunCommand(FunctionCode.WriteOutputWords, txData, 5 + wordCount * 2, rxData, rxDataCount)
            ' Verify the rx data
            WriteOutputWords = WriteOutputWords Or _
                               (rxData(0) <> txData(0) Or rxData(1) <> txData(1)) Or _
                               (rxData(2) <> txData(2) Or rxData(3) <> txData(3))
        Else    ' Otherwise, if the device is not connected
            ' Return True
            WriteOutputWords = True
        End If
    End Function



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
    Private Function RunCommand(ByVal functionCode As Integer, _
                                ByVal txData() As Byte, _
                                ByVal txDataCount As Integer, _
                                ByRef rxData() As Byte, _
                                ByRef rxDataCount As Integer) As Boolean
        Dim answerReceived As Boolean
        Dim rxBuffer(0 To _bufferSize - 1) As Byte
        Dim rxBufferCount As Integer
        Dim txBuffer(0 To _bufferSize - 1) As Byte
        Dim txBufferCount As Integer
        Dim t0 As Date

        ' If the device is connected
        If (_connected) Then
            Try
                ' Empty the rx buffer
                If (_stream.DataAvailable) Then
                    _stream.Read(rxBuffer, 0, _bufferSize)
                End If
                ' Build the command
                txBuffer(0) = CByte((_transactionIdentifier \ 256) And &HFF)
                txBuffer(1) = CByte(_transactionIdentifier And &HFF)
                txBuffer(2) = CByte((_protocolIdentifier \ 256) And &HFF)
                txBuffer(3) = CByte((_protocolIdentifier \ 256) And &HFF)
                txBuffer(4) = 0
                txBuffer(5) = CByte(txDataCount + 2)
                txBuffer(6) = _slaveAddress
                txBuffer(7) = CByte(functionCode)
                txBufferCount = 0
                Do While (txBufferCount < txDataCount)
                    txBuffer(txBufferCount + 8) = txData(txBufferCount)
                    txBufferCount = txBufferCount + 1
                Loop
                txBufferCount = txBufferCount + 8
                ' Send the command
                _stream.Write(txBuffer, 0, txBufferCount)
                ' Wait for the answer
                rxBufferCount = 0
                t0 = Date.Now
                Do
                    If (_stream.DataAvailable) Then
                        rxBufferCount = rxBufferCount + _stream.Read(rxBuffer, rxBufferCount, _bufferSize - rxBufferCount)
                    End If
                    answerReceived = (rxBufferCount >= 6 AndAlso rxBufferCount = rxBuffer(4) * 256 + rxBuffer(5) + 6)
                Loop Until (answerReceived Or (Date.Now - t0).TotalSeconds > _answerTimeout_s)
                ' If the answer is correct and positive
                If (answerReceived And _
                    rxBuffer(0) = txBuffer(0) And rxBuffer(1) = txBuffer(1) And _
                    rxBuffer(2) = txBuffer(2) And rxBuffer(3) = txBuffer(3) And _
                    rxBuffer(6) = _slaveAddress And _
                    rxBuffer(7) = functionCode) Then
                    ' Return the data bytes
                    rxDataCount = rxBufferCount - 8
                    For i = 0 To rxDataCount - 1
                        rxData(i) = rxBuffer(8 + i)
                    Next
                    ' Return False
                    RunCommand = False
                Else
                    ' Return True
                    RunCommand = True
                End If

            Catch ex As Exception   ' If some unexpected errors happened
                ' Return True
                RunCommand = True
            End Try
        Else    ' Otherwise, if the device is not connected
            ' Return True
            RunCommand = True
        End If
    End Function
End Class