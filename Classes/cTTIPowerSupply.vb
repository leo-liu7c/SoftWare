Option Explicit On
Option Strict On

Public Class cTTIPowerSupply
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Public constants
    Public Const DefaultPortNumber = 9221



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _answerTimeout = 2
    Private Const _maxCurrent = 3
    Private Const _maxVoltage = 30
    Private Const _HardwareEnabled = mConstants.HardwareEnabled_TTI
    ' Private variables
    Private _connected As Boolean
    Private _manufacturer As String
    Private _model As String
    Private _serialNumber As String
    Private _softwareRevision As String
    Private _TCPClient As New cTCPClient



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public ReadOnly Property Connected() As Boolean
        Get
            ' Return the flag of instrument connected
            Connected = _connected
        End Get
    End Property



    Public ReadOnly Property Manufacturer() As String
        Get
            ' Return the manufacturer of the instrument
            Manufacturer = _manufacturer
        End Get
    End Property



    Public ReadOnly Property Model() As String
        Get
            ' Return the model of the instrument
            Model = _model
        End Get
    End Property



    Public ReadOnly Property SerialNumber() As String
        Get
            ' Return the serial number of the instrument
            SerialNumber = _serialNumber
        End Get
    End Property



    Public ReadOnly Property SoftwareRevision() As String
        Get
            ' Return the software revision of the instrument
            SoftwareRevision = _softwareRevision
        End Get
    End Property



    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+
    Protected Overrides Sub Finalize()
        ' If the instrument is connected
        If (_connected = True) Then
            ' Disconnect the instrument
            Me.Disconnect()
        End If
        ' Finalize the object
        MyBase.Finalize()
    End Sub



    Public Sub New()
        ' Initialize the private variables
        _connected = False
        _manufacturer = ""
        _model = ""
        _serialNumber = ""
        _softwareRevision = ""
    End Sub



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function Connect(ByVal IPAddress As String, _
                            ByVal portNumber As Long) As Boolean
        Dim answer As String
        Dim errFlag As Boolean
        Dim token(0 To 3) As String

        If (_HardwareEnabled) Then
            ' If the instrument is not connected
            If Not (_connected) Then
                ' If the TCP client is already connected
                If (_TCPClient.Connected) Then
                    ' Disconnect the TCP client
                    _TCPClient.Disconnect()
                End If
                ' Connect to the instrument
                errFlag = _TCPClient.Connect(IPAddress, CInt(portNumber))
                ' Reset the instrument
                errFlag = errFlag OrElse RunCommand("*RST", False, CStr(vbNull))
                ' Read the instrument identifier
                answer = ""
                errFlag = errFlag OrElse RunCommand("*IDN?", True, answer)
                ' If no errors happened
                If Not (errFlag) Then
                    ' Split the instrument's information
                    token = Split(answer, ",")
                    _manufacturer = token(0)
                    _model = token(1)
                    _serialNumber = token(2)
                    _softwareRevision = token(3)
                    ' Set the flag of instrument connected
                    _connected = True
                    ' Return False
                    Connect = False
                Else    ' Otherwise, if some error happened
                    ' Return True
                    Connect = True
                End If
            Else
                ' Return False
                Connect = False
                Console.WriteLine("1Connect = False")
            End If
        Else
            ' Return False
            Connect = False
            Console.WriteLine("2Connect = False")
        End If
    End Function



    Public Function Disconnect() As Boolean
        ' If the instrument is connected
        If (_connected) Then
            Threading.Thread.Sleep(10)
            ' Reset the instrument
            RunCommand("*RST", False, CStr(vbNull))
            Disconnect = False
            Console.WriteLine("Disconnect = RunCommand("" * RST"", False, CStr(vbNull))," + Disconnect.ToString())
            ' Clear the instrument's information
            _manufacturer = ""
            _model = ""
            _serialNumber = ""
            _softwareRevision = ""
            ' Clear the flag of instrument connected
            _connected = False
            ' Disconnect the instrument
            _TCPClient.Disconnect()
        Else    ' Otherwise, if the instrument is not connected
            ' Return False
            Disconnect = False
            Console.WriteLine("Disconnect = False")
        End If
    End Function



    Public Function GetCurrent(ByVal outputIndex As Integer, _
                               ByRef value As Single) As Boolean
        Dim answer As String

        ' If the instrument is connected and the parameters are valid
        If (_connected = True And _
            outputIndex >= 1 And outputIndex <= 3) Then
            ' Read the current measured
            answer = ""
            GetCurrent = RunCommand("I" & CStr(outputIndex) & "O?", True, answer)
            ' Store the current measured
            If (GetCurrent = False) Then
                value = CSng(Val(answer))
            Else
                value = 0
            End If
        Else    ' Otherwise, if the instrument is not connected or the parameters are not valid
            ' Return True
            GetCurrent = True
            Console.WriteLine("GetCurrent = True")
        End If
    End Function



    Public Function GetMaxCurrentSet(ByVal outputIndex As Integer, _
                                     ByRef value As Single) As Boolean
        Dim answer As String

        ' If the instrument is connected and the parameters are valid
        If (_connected = True And _
            outputIndex >= 1 And outputIndex <= 3) Then
            ' Read the maximum current set
            answer = ""
            GetMaxCurrentSet = RunCommand("I" & CStr(outputIndex) & "?", True, answer)
            ' Store the current set
            If (GetMaxCurrentSet = False) Then
                answer = Mid(answer, 4)
                value = CSng(Val(answer))
            Else
                value = 0
            End If
        Else    ' Otherwise, if the instrument is not connected or the parameters are not valid
            ' Return True
            GetMaxCurrentSet = True
            Console.WriteLine("GetMaxCurrentSet = True")
        End If
    End Function



    Public Function GetOutputStatus(ByVal outputIndex As Integer, _
                                    ByRef status As Boolean) As Boolean
        Dim answer As String

        ' If the instrument is connected and the parameters are valid
        If (_connected = True And _
            outputIndex >= 1 And outputIndex <= 3) Then
            ' Read the output status
            answer = ""
            GetOutputStatus = RunCommand("OP" & CStr(outputIndex) & "?", True, answer)
            ' Store the output status
            status = (answer = "1")
        Else    ' Otherwise, if the instrument is not connected or the parameters are not valid
            ' Return True
            GetOutputStatus = True
            Console.WriteLine("GetOutputStatus = True")
        End If
    End Function



    Public Function GetVoltage(ByVal outputIndex As Integer, _
                               ByRef value As Single) As Boolean
        Dim answer As String

        ' If the instrument is connected and the parameters are valid
        If (_connected = True And _
            outputIndex >= 1 And outputIndex <= 3) Then
            ' Read the voltage measured
            answer = ""
            GetVoltage = RunCommand("V" & CStr(outputIndex) & "O?", True, answer)
            ' Store the voltage measured
            If (GetVoltage = False) Then
                value = CSng(Val(answer))
            Else
                value = 0
            End If
        Else    ' Otherwise, if the instrument is not connected or the parameters are not valid
            ' Return True
            GetVoltage = True
            Console.WriteLine("GetVoltage = True")
        End If
    End Function



    Public Function GetVoltageSet(ByVal outputIndex As Integer, _
                                  ByRef value As Single) As Boolean
        Dim answer As String

        ' If the instrument is connected and the parameters are valid
        If (_connected = True And _
            outputIndex >= 1 And outputIndex <= 3) Then
            ' Read the voltage set
            answer = ""
            GetVoltageSet = RunCommand("V" & CStr(outputIndex) & "?", True, answer)
            ' Store the voltage set
            If (GetVoltageSet = False) Then
                answer = Mid(answer, 4)
                value = CSng(Val(answer))
            Else
                value = 0
            End If
        Else    ' Otherwise, if the instrument is not connected or the parameters are not valid
            ' Return True
            GetVoltageSet = True
            Console.WriteLine("GetVoltageSet = True")
        End If
    End Function



    Public Function OutputOff(ByVal outputIndex As Integer) As Boolean
        Dim answer As String

        ' If the instrument is connected and the parameters are valid
        If (_connected = True And _
            outputIndex >= 1 And outputIndex <= 3) Then
            ' Switch the output off
            answer = ""
            OutputOff = RunCommand("OP" & CStr(outputIndex) & " 0", False, answer)
        Else    ' Otherwise, if the instrument is not connected or the parameters are not valid
            ' Return True
            OutputOff = True
            Console.WriteLine("OutputOff = True")
        End If
    End Function



    Public Function OutputOn(ByVal outputIndex As Integer) As Boolean
        Dim answer As String

        ' If the instrument is connected and the parameters are valid
        If (_connected = True And _
            outputIndex >= 1 And outputIndex <= 3) Then
            ' Switch the output on
            answer = ""
            OutputOn = RunCommand("OP" & CStr(outputIndex) & " 1", False, answer)
        Else    ' Otherwise, if the instrument is not connected or the parameters are not valid
            ' Return True
            OutputOn = True
            Console.WriteLine("OutputOn = True")
        End If
    End Function



    Public Function SetMaxCurrent(ByVal outputIndex As Integer, _
                                  ByVal value As Single) As Boolean
        ' If the instrument is connected and the parameters are valid
        If (_connected = True And _
            outputIndex >= 1 And outputIndex <= 3 And _
            value >= 0 And value <= _maxCurrent) Then
            ' Set the maximum current
            SetMaxCurrent = RunCommand("I" & CStr(outputIndex) & " " & Str(value), False, CStr(vbNull))
        Else    ' Otherwise, if the instrument is not connected or the parameters are not valid
            ' Return True
            SetMaxCurrent = True
            Console.WriteLine("SetMaxCurrent = True")
        End If
    End Function



    Public Function SetVoltage(ByVal outputIndex As Integer, _
                               ByVal value As Single) As Boolean
        ' If the instrument is connected and the parameters are valid
        If (_connected = True And _
            outputIndex >= 1 And outputIndex <= 3 And _
            value >= 0 And value <= _maxVoltage) Then
            ' Set the voltage
            SetVoltage = RunCommand("V" & CStr(outputIndex) & " " & Str(value), False, CStr(vbNull))
        Else    ' Otherwise, if the instrument is not connected or the parameters are not valid
            ' Return True
            SetVoltage = True
            Console.WriteLine("SetVoltage = True")
        End If
    End Function



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
    Private Function RunCommand(ByVal command As String, _
                                ByVal withAnswer As Boolean, _
                                ByRef answer As String) As Boolean
        Dim errFlag As Boolean
        Dim rxBuffer As String
        Dim rxString As String
        Dim t0 As Date

        ' If the TCP client is connected
        If (_TCPClient.Connected) Then
            ' Empty the input buffer
            rxString = ""
            errFlag = _TCPClient.Receive(rxString)
            ' Send the command
            errFlag = errFlag OrElse _TCPClient.Transmit(command)
            If (Not _TCPClient.Connected) Then
                If (Not _TCPClient.ReConnect()) Then
                    _connected = True
                End If
                Console.WriteLine("_TCPClient.ReConnect(),command:" & command & ",RunCommand = " & RunCommand.ToString())
                errFlag = _TCPClient.Transmit(command)
            End If
            ' If the command has an answer
            If (withAnswer) Then
                ' Wait for the answer
                rxBuffer = ""
                t0 = Date.Now
                Do
                    rxString = ""
                    errFlag = errFlag OrElse _TCPClient.Receive(rxString)
                    rxBuffer = rxBuffer & rxString
                Loop Until (errFlag Or rxBuffer.Contains(vbCrLf) Or (Date.Now - t0).TotalSeconds > _answerTimeout)
                ' If an answer was received
                If (rxBuffer.Contains(vbCrLf)) Then
                    ' Remove the terminator characters and return the answer
                    answer = Left(rxBuffer, InStr(rxBuffer, vbCrLf) - 1)
                Else    ' Otherwise, if an answer was not received
                    ' Set the error flag
                    errFlag = True
                End If
            End If
            ' Wait 5 ms
            t0 = Date.Now
            Do
            Loop Until (Date.Now - t0).TotalMilliseconds > 5
            ' Send the EER? command
            errFlag = errFlag OrElse _TCPClient.Transmit("EER?")
            ' Wait for the answer
            rxBuffer = ""
            t0 = Date.Now
            Do
                rxString = ""
                errFlag = errFlag OrElse _TCPClient.Receive(rxString)
                rxBuffer = rxBuffer & rxString
            Loop Until (errFlag Or rxBuffer.Contains(vbCrLf) Or (Date.Now - t0).TotalSeconds > _answerTimeout)
            ' If an answer was received
            If (rxBuffer.Contains(vbCrLf)) Then
                ' If the value is different from 0 sets the error flag
                rxBuffer = Left(rxBuffer, InStr(rxBuffer, vbCrLf) - 1)
                errFlag = errFlag OrElse (rxBuffer <> "0")
            Else    ' Otherwise, if an answer was not received
                ' Set the error flag
                errFlag = True
            End If
            ' Return the error flag
            RunCommand = errFlag
            Console.WriteLine("command:" & command & ",RunCommand = " & RunCommand.ToString())
        Else    ' Otherwise, if the TCP client is not connected
            ' Return True
            RunCommand = True
            Console.WriteLine("command:" & command & ",RunCommand = True!")
        End If
    End Function
End Class