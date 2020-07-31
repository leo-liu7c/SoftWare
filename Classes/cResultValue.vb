Option Explicit On
Option Strict On

Imports System.Math

Public Class cResultValue
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Public enumerations
    Public Enum eValueTestResult
        NotTested = 0
        Disabled = 1
        Unknown = 2
        Passed = 3
        Failed = 4
        NotCoherent = 5
        NotValueCheck = 6
    End Enum

    Public Enum eValueType
        BCDValue = 0
        HexValue = 1
        IntegerValue = 2
        SingleValue = 3
        StringValue = 4
    End Enum



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Private _description As String
    Private _testResult As eValueTestResult
    Private _units As String
    Private _valueType As eValueType

    Private _integerMinimumLimit As Integer
    Private _integerMaximumLimit As Integer
    Private _integerValue As Integer

    Private _decimals As Integer
    Private _singleMinimumLimit As Single
    Private _singleMaximumLimit As Single
    Private _singleValue As Single

    Private _byteSize As Integer
    Private _maximumLength As Integer
    Private _stringMinimumLimit As String
    Private _stringMaximumLimit As String
    Private _stringValue As String



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public Property ByteSize As Integer
        Get
            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue) Then
                ByteSize = _byteSize
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Get
        Set(value As Integer)
            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue) Then
                _byteSize = value
                _stringMinimumLimit = Right(StrDup(_byteSize * 2, "0") & _stringMinimumLimit, _byteSize * 2)
                _stringMaximumLimit = Right(StrDup(_byteSize * 2, "0") & _stringMaximumLimit, _byteSize * 2)
                _stringValue = Right(StrDup(_byteSize * 2, "0") & _stringValue, _byteSize * 2)
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Set
    End Property



    Public Property Decimals As Integer
        Get
            If (_valueType = eValueType.SingleValue) Then
                Decimals = _decimals
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Get
        Set(value As Integer)
            If (_valueType = eValueType.SingleValue) Then
                _decimals = value
                _singleMinimumLimit = CSng(Round(_singleMinimumLimit, _decimals))
                _singleMaximumLimit = CSng(Round(_singleMaximumLimit, _decimals))
                _singleValue = CSng(Round(_singleValue, _decimals))
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Set
    End Property



    Public Property Description As String
        Get
            Description = _description
        End Get
        Set(value As String)
            _description = value
        End Set
    End Property



    Public Property MaximumLength As Integer
        Get
            If (_valueType = eValueType.StringValue) Then
                MaximumLength = _maximumLength
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Get
        Set(value As Integer)
            If (_valueType = eValueType.StringValue) Then
                _maximumLength = value
                If (_stringMinimumLimit.Length > _maximumLength) Then
                    _stringMinimumLimit = Left(_stringMinimumLimit, _maximumLength)
                End If
                If (_stringMaximumLimit.Length > _maximumLength) Then
                    _stringMaximumLimit = Left(_stringMaximumLimit, _maximumLength)
                End If
                If (_stringValue.Length > _maximumLength) Then
                    _stringValue = Left(_stringValue, _maximumLength)
                End If
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Set
    End Property



    Public Property MaximumLimit As Object
        Get
            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue Or _
                _valueType = eValueType.StringValue) Then
                MaximumLimit = _stringMaximumLimit
            ElseIf (_valueType = eValueType.IntegerValue) Then
                MaximumLimit = _integerMaximumLimit
            ElseIf (_valueType = eValueType.SingleValue) Then
                MaximumLimit = _singleMaximumLimit
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Get
        Set(value As Object)
            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue Or _
                _valueType = eValueType.StringValue) Then
                _stringMaximumLimit = CStr(value)
            ElseIf (_valueType = eValueType.IntegerValue) Then
                _integerMaximumLimit = CInt(value)
            ElseIf (_valueType = eValueType.SingleValue) Then
                _singleMaximumLimit = CSng(value)
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Set
    End Property



    Public Property MinimumLimit As Object
        Get
            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue Or _
                _valueType = eValueType.StringValue) Then
                MinimumLimit = _stringMinimumLimit
            ElseIf (_valueType = eValueType.IntegerValue) Then
                MinimumLimit = _integerMinimumLimit
            ElseIf (_valueType = eValueType.SingleValue) Then
                MinimumLimit = _singleMinimumLimit
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Get
        Set(value As Object)
            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue Or _
                _valueType = eValueType.StringValue) Then
                _stringMinimumLimit = CStr(value)
            ElseIf (_valueType = eValueType.IntegerValue) Then
                _integerMinimumLimit = CInt(value)
            ElseIf (_valueType = eValueType.SingleValue) Then
                _singleMinimumLimit = CSng(value)
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Set
    End Property



    Public ReadOnly Property StringMaximumLimit As String
        Get
            Dim formatString As String

            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue Or _
                _valueType = eValueType.StringValue) Then
                StringMaximumLimit = _stringMaximumLimit
            ElseIf (_valueType = eValueType.IntegerValue) Then
                StringMaximumLimit = _integerMaximumLimit.ToString
            ElseIf (_valueType = eValueType.SingleValue) Then
                formatString = "0"
                If (_decimals > 0) Then
                    formatString = formatString & "." & StrDup(_decimals, "0")
                End If
                StringMaximumLimit = _singleMaximumLimit.ToString(formatString)
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Get
    End Property



    Public ReadOnly Property StringMinimumLimit As String
        Get
            Dim formatString As String

            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue Or _
                _valueType = eValueType.StringValue) Then
                StringMinimumLimit = _stringMinimumLimit
            ElseIf (_valueType = eValueType.IntegerValue) Then
                StringMinimumLimit = _integerMinimumLimit.ToString
            ElseIf (_valueType = eValueType.SingleValue) Then
                formatString = "0"
                If (_decimals > 0) Then
                    formatString = formatString & "." & StrDup(_decimals, "0")
                End If
                StringMinimumLimit = _singleMinimumLimit.ToString(formatString)
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Get
    End Property



    Public ReadOnly Property StringValue As String
        Get
            Dim formatString As String

            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue Or _
                _valueType = eValueType.StringValue) Then
                StringValue = _stringValue
            ElseIf (_valueType = eValueType.IntegerValue) Then
                StringValue = _integerValue.ToString
            ElseIf (_valueType = eValueType.SingleValue) Then
                formatString = "0"
                If (_decimals > 0) Then
                    formatString = formatString & "." & StrDup(_decimals, "0")
                End If
                StringValue = _singleValue.ToString(formatString)
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Get
    End Property



    Public Property TestResult As eValueTestResult
        Get
            TestResult = _testResult
        End Get
        Set(value As eValueTestResult)
            _testResult = value
        End Set
    End Property



    Public Property Units As String
        Get
            Units = _units
        End Get
        Set(value As String)
            _units = value
        End Set
    End Property



    Public Property Value As Object
        Get
            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue Or _
                _valueType = eValueType.StringValue) Then
                Value = _stringValue
            ElseIf (_valueType = eValueType.IntegerValue) Then
                Value = _integerValue
            ElseIf (_valueType = eValueType.SingleValue) Then
                Value = _singleValue
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Get
        Set(value As Object)
            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue Or _
                _valueType = eValueType.StringValue) Then
                _stringValue = CStr(value)
            ElseIf (_valueType = eValueType.IntegerValue) Then
                _integerValue = CInt(value)
            ElseIf (_valueType = eValueType.SingleValue) Then
                _singleValue = CSng(value)
            Else
                Throw New Exception("Property not supported by this type of result value.")
            End If
        End Set
    End Property



    Public ReadOnly Property ValueType As eValueType
        Get
            ValueType = _valueType
        End Get
    End Property



    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+
    Public Sub New(ByVal valueType As eValueType)
        ' Initialize private variables
        _description = ""
        _testResult = eValueTestResult.Disabled
        _units = ""

        _integerMinimumLimit = 0
        _integerMaximumLimit = 0
        _integerValue = 0

        _decimals = 0
        _singleMinimumLimit = 0
        _singleMaximumLimit = 0
        _singleValue = 0

        _byteSize = 0
        _maximumLength = 0
        _stringMinimumLimit = ""
        _stringMaximumLimit = ""
        _stringValue = ""

        _valueType = valueType
    End Sub



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function Test() As eValueTestResult
        If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue Or _
                _valueType = eValueType.StringValue) And _testResult <> eValueTestResult.Disabled Then
            If (_stringValue >= _stringMinimumLimit And _stringValue <= _stringMaximumLimit) Then
                _testResult = eValueTestResult.Passed
            Else
                _testResult = eValueTestResult.Failed
            End If
        ElseIf (_valueType = eValueType.IntegerValue) And _testResult <> eValueTestResult.Disabled Then
            If (_integerValue >= _integerMinimumLimit And _integerValue <= _integerMaximumLimit) Then
                _testResult = eValueTestResult.Passed
            Else
                _testResult = eValueTestResult.Failed
            End If
        ElseIf (_valueType = eValueType.SingleValue) And _testResult <> eValueTestResult.Disabled Then
            If (_singleValue >= _singleMinimumLimit And _singleValue <= _singleMaximumLimit) Then
                _testResult = eValueTestResult.Passed
            Else
                _testResult = eValueTestResult.Failed
            End If
        ElseIf _testResult = eValueTestResult.Disabled Then
            '
        Else
            Throw New Exception("Method not supported by this type of result value.")
        End If
        Test = _testResult
    End Function



    Public Shared Function ValueTestResultDescription(ByVal value As eValueTestResult) As String
        Select Case value
            Case eValueTestResult.NotTested
                ValueTestResultDescription = "Not tested"
            Case eValueTestResult.Disabled
                ValueTestResultDescription = "Disabled"
            Case eValueTestResult.Unknown
                ValueTestResultDescription = "Unknown"
            Case eValueTestResult.Passed
                ValueTestResultDescription = "Passed"
            Case eValueTestResult.Failed
                ValueTestResultDescription = "Failed"
            Case eValueTestResult.NotCoherent
                ValueTestResultDescription = "Not coherent"
            Case Else
                ValueTestResultDescription = String.Format("Value {0} unknown", value)
        End Select
    End Function



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Class