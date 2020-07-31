Option Explicit On
Option Strict On

Imports System.Math

Public Class cRecipeValue
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Public enumerations
    Public Enum eValueType
        BCDRange = 0
        BCDValue = 1
        BooleanValue = 2
        HexRange = 3
        HexValue = 4
        IntegerRange = 5
        IntegerValue = 6
        SingleRange = 7
        SingleValue = 8
        StringValue = 9
    End Enum



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _stringValueFalse = "False"
    Private Const _stringValueTrue = "True"

    ' Private variables
    Private _description As String
    Private _units As String
    Private _valueType As eValueType

    Private _booleanDefaultValue As Boolean
    Private _booleanValue As Boolean

    Private _integerMinimumValue As Integer
    Private _integerMaximumValue As Integer
    Private _integerDefaultMinimumLimit As Integer
    Private _integerDefaultMaximumLimit As Integer
    Private _integerDefaultValue As Integer
    Private _integerMinimumLimit As Integer
    Private _integerMaximumLimit As Integer
    Private _integerValue As Integer

    Private _decimals As Integer
    Private _singleMinimumValue As Single
    Private _singleMaximumValue As Single
    Private _singleDefaultMinimumLimit As Single
    Private _singleDefaultMaximumLimit As Single
    Private _singleDefaultValue As Single
    Private _singleMinimumLimit As Single
    Private _singleMaximumLimit As Single
    Private _singleValue As Single

    Private _byteSize As Integer
    Private _maximumLength As Integer
    Private _stringMinimumValue As String
    Private _stringMaximumValue As String
    Private _stringDefaultMinimumLimit As String
    Private _stringDefaultMaximumLimit As String
    Private _stringDefaultValue As String
    Private _stringMinimumLimit As String
    Private _stringMaximumLimit As String
    Private _stringValue As String



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public Property ByteSize As Integer
        Get
            If (_valueType = eValueType.BCDRange Or _
                _valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexRange Or _
                _valueType = eValueType.HexValue) Then
                ByteSize = _byteSize
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Get
        Set(value As Integer)
            If (_valueType = eValueType.BCDRange Or _
                _valueType = eValueType.HexRange) Then
                _byteSize = value
                _stringMinimumValue = Right(StrDup(_byteSize * 2, "0") & _stringMinimumValue, _byteSize * 2)
                _stringMaximumValue = Right(StrDup(_byteSize * 2, "0") & _stringMaximumValue, _byteSize * 2)
                _stringDefaultMinimumLimit = Right(StrDup(_byteSize * 2, "0") & _stringDefaultMinimumLimit, _byteSize * 2)
                _stringDefaultMaximumLimit = Right(StrDup(_byteSize * 2, "0") & _stringDefaultMaximumLimit, _byteSize * 2)
                _stringMinimumLimit = Right(StrDup(_byteSize * 2, "0") & _stringMinimumLimit, _byteSize * 2)
                _stringMaximumLimit = Right(StrDup(_byteSize * 2, "0") & _stringMaximumLimit, _byteSize * 2)
            ElseIf (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue) Then
                _byteSize = value
                _stringMinimumValue = Right(StrDup(_byteSize * 2, "0") & _stringMinimumValue, _byteSize * 2)
                _stringMaximumValue = Right(StrDup(_byteSize * 2, "0") & _stringMaximumValue, _byteSize * 2)
                _stringDefaultValue = Right(StrDup(_byteSize * 2, "0") & _stringDefaultValue, _byteSize * 2)
                _stringValue = Right(StrDup(_byteSize * 2, "0") & _stringValue, _byteSize * 2)
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Set
    End Property



    Public Property Decimals As Integer
        Get
            If (_valueType = eValueType.SingleRange Or _
                _valueType = eValueType.SingleValue) Then
                Decimals = _decimals
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Get
        Set(value As Integer)
            If (_valueType = eValueType.SingleRange) Then
                _decimals = value
                _singleMinimumValue = CSng(Round(_singleMinimumValue, _decimals))
                _singleMaximumValue = CSng(Round(_singleMaximumValue, _decimals))
                _singleDefaultMinimumLimit = CSng(Round(_singleDefaultMinimumLimit, _decimals))
                _singleDefaultMaximumLimit = CSng(Round(_singleDefaultMaximumLimit, _decimals))
                _singleMinimumLimit = CSng(Round(_singleMinimumLimit, _decimals))
                _singleMaximumLimit = CSng(Round(_singleMaximumLimit, _decimals))

            ElseIf (_valueType = eValueType.SingleValue) Then
                _decimals = value
                _singleMinimumValue = CSng(Round(_singleMinimumValue, _decimals))
                _singleMaximumValue = CSng(Round(_singleMaximumValue, _decimals))
                _singleDefaultValue = CSng(Round(_singleDefaultValue, _decimals))
                _singleValue = CSng(Round(_singleValue, _decimals))
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Set
    End Property



    Public Property DefaultMaximumLimit As Object
        Get
            If (_valueType = eValueType.BCDRange Or _
                _valueType = eValueType.HexRange) Then
                DefaultMaximumLimit = _stringDefaultMaximumLimit
            ElseIf (_valueType = eValueType.IntegerRange) Then
                DefaultMaximumLimit = _integerDefaultMaximumLimit
            ElseIf (_valueType = eValueType.SingleRange) Then
                DefaultMaximumLimit = _singleDefaultMaximumLimit
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Get
        Set(value As Object)
            If (_valueType = eValueType.BCDRange Or _
                _valueType = eValueType.HexRange) Then
                _stringDefaultMaximumLimit = Right(StrDup(_byteSize * 2, "0") & CStr(value), _byteSize * 2)
                If (_stringDefaultMaximumLimit < _stringMinimumValue) Then
                    _stringDefaultMaximumLimit = _stringMinimumValue
                ElseIf (_stringDefaultMaximumLimit > _stringMaximumValue) Then
                    _stringDefaultMaximumLimit = _stringMaximumValue
                End If
            ElseIf (_valueType = eValueType.IntegerRange) Then
                _integerDefaultMaximumLimit = CInt(value)
                If (_integerDefaultMaximumLimit < _integerMinimumValue) Then
                    _integerDefaultMaximumLimit = _integerMinimumValue
                ElseIf (_integerDefaultMaximumLimit > _integerMaximumValue) Then
                    _integerDefaultMaximumLimit = _integerMaximumValue
                End If
            ElseIf (_valueType = eValueType.SingleRange) Then
                _singleDefaultMaximumLimit = CSng(value)
                If (_singleDefaultMaximumLimit < _singleMinimumValue) Then
                    _singleDefaultMaximumLimit = _singleMinimumValue
                ElseIf (_singleDefaultMaximumLimit > _singleMaximumValue) Then
                    _singleDefaultMaximumLimit = _singleMaximumValue
                End If
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Set
    End Property



    Public Property DefaultMinimumLimit As Object
        Get
            If (_valueType = eValueType.BCDRange Or _
                _valueType = eValueType.HexRange) Then
                DefaultMinimumLimit = _stringDefaultMinimumLimit
            ElseIf (_valueType = eValueType.IntegerRange) Then
                DefaultMinimumLimit = _integerDefaultMinimumLimit
            ElseIf (_valueType = eValueType.SingleRange) Then
                DefaultMinimumLimit = _singleDefaultMinimumLimit
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Get
        Set(value As Object)
            If (_valueType = eValueType.BCDRange Or _
                _valueType = eValueType.HexRange) Then
                _stringDefaultMinimumLimit = Right(StrDup(_byteSize * 2, "0") & CStr(value), _byteSize * 2)
                If (_stringDefaultMinimumLimit < _stringMinimumValue) Then
                    _stringDefaultMinimumLimit = _stringMinimumValue
                ElseIf (_stringDefaultMinimumLimit > _stringMaximumValue) Then
                    _stringDefaultMinimumLimit = _stringMaximumValue
                End If
            ElseIf (_valueType = eValueType.IntegerRange) Then
                _integerDefaultMinimumLimit = CInt(value)
                If (_integerDefaultMinimumLimit < _integerMinimumValue) Then
                    _integerDefaultMinimumLimit = _integerMinimumValue
                ElseIf (_integerDefaultMinimumLimit > _integerMaximumValue) Then
                    _integerDefaultMinimumLimit = _integerMaximumValue
                End If
            ElseIf (_valueType = eValueType.SingleRange) Then
                _singleDefaultMinimumLimit = CSng(value)
                If (_singleDefaultMinimumLimit < _singleMinimumValue) Then
                    _singleDefaultMinimumLimit = _singleMinimumValue
                ElseIf (_singleDefaultMinimumLimit > _singleMaximumValue) Then
                    _singleDefaultMinimumLimit = _singleMaximumValue
                End If
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Set
    End Property



    Public Property DefaultValue As Object
        Get
            If (_valueType = eValueType.BCDValue or _
                _valueType = eValueType.HexValue) Then
                DefaultValue = _stringDefaultValue
            ElseIf (_valueType = eValueType.BooleanValue) Then
                DefaultValue = _booleanDefaultValue
            ElseIf (_valueType = eValueType.IntegerValue) Then
                DefaultValue = _integerDefaultValue
            ElseIf (_valueType = eValueType.SingleValue) Then
                DefaultValue = _singleDefaultValue
            ElseIf (_valueType = eValueType.StringValue) Then
                DefaultValue = _stringDefaultValue
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Get
        Set(value As Object)
            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue) Then
                _stringDefaultValue = Right(StrDup(_byteSize * 2, "0") & CStr(value), _byteSize * 2)
                If (_stringDefaultValue < _stringMinimumValue) Then
                    _stringDefaultValue = _stringMinimumValue
                ElseIf (_stringDefaultValue > _stringMaximumValue) Then
                    _stringDefaultValue = _stringMaximumValue
                End If
            ElseIf (_valueType = eValueType.BooleanValue) Then
                _booleanDefaultValue = CBool(value)
            ElseIf (_valueType = eValueType.IntegerValue) Then
                _integerDefaultValue = CInt(value)
                If (_integerDefaultValue < _integerMinimumValue) Then
                    _integerDefaultValue = _integerMinimumValue
                ElseIf (_integerDefaultValue > _integerMaximumValue) Then
                    _integerDefaultValue = _integerMaximumValue
                End If
            ElseIf (_valueType = eValueType.SingleValue) Then
                _singleDefaultValue = CSng(value)
                If (_singleDefaultValue < _singleMinimumValue) Then
                    _singleDefaultValue = _singleMinimumValue
                ElseIf (_singleDefaultValue > _singleMaximumValue) Then
                    _singleDefaultValue = _singleMaximumValue
                End If
            ElseIf (_valueType = eValueType.StringValue) Then
                _stringDefaultValue = CStr(value)
                If (_stringDefaultValue.Length > _maximumLength) Then
                    _stringDefaultValue = Left(_stringDefaultValue, _maximumLength)
                End If
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
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
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Get
        Set(value As Integer)
            If (_valueType = eValueType.StringValue) Then
                _maximumLength = value
                If (_stringValue.Length > _maximumLength) Then
                    _stringValue = Left(_stringValue, _maximumLength)
                End If
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Set
    End Property



    Public Property MaximumLimit As Object
        Get
            Try
                If (_valueType = eValueType.BCDRange Or
                _valueType = eValueType.HexRange) Then
                    MaximumLimit = _stringMaximumLimit
                ElseIf (_valueType = eValueType.IntegerRange) Then
                    MaximumLimit = _integerMaximumLimit
                ElseIf (_valueType = eValueType.SingleRange) Then
                    MaximumLimit = _singleMaximumLimit
                Else
                    Throw New Exception("Property not supported by this type of recipe value.")
                End If
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try
        End Get
        Set(value As Object)
            Try
                If (_valueType = eValueType.BCDRange Or
                    _valueType = eValueType.HexRange) Then
                    _stringMaximumLimit = Right(StrDup(_byteSize * 2, "0") & CStr(value), _byteSize * 2)
                    If (_stringMaximumLimit < _stringMinimumValue) Then
                        _stringMaximumLimit = _stringMinimumValue
                    ElseIf (_stringMaximumLimit > _stringMaximumValue) Then
                        _stringMaximumLimit = _stringMaximumValue
                    End If
                ElseIf (_valueType = eValueType.IntegerRange) Then
                    _integerMaximumLimit = CInt(value)
                    If (_integerMaximumLimit < _integerMinimumValue) Then
                        _integerMaximumLimit = _integerMinimumValue
                    ElseIf (_integerMaximumLimit > _integerMaximumValue) Then
                        _integerMaximumLimit = _integerMaximumValue
                    End If
                ElseIf (_valueType = eValueType.SingleRange) Then
                    _singleMaximumLimit = CSng(value)
                    If (_singleMaximumLimit < _singleMinimumValue) Then
                        _singleMaximumLimit = _singleMinimumValue
                    ElseIf (_singleMaximumLimit > _singleMaximumValue) Then
                        _singleMaximumLimit = _singleMaximumValue
                    End If
                Else
                    Throw New Exception("Property not supported by this type of recipe value.")
                End If
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try
        End Set
    End Property



    Public Property MaximumValue As Object
        Get
            If (_valueType = eValueType.BCDRange Or _
                _valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexRange Or _
                _valueType = eValueType.HexValue) Then
                MaximumValue = _stringMaximumValue
            ElseIf (_valueType = eValueType.IntegerRange Or _
                    _valueType = eValueType.IntegerValue) Then
                MaximumValue = _integerMaximumValue
            ElseIf (_valueType = eValueType.SingleRange Or _
                    _valueType = eValueType.SingleValue) Then
                MaximumValue = _singleMaximumValue
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Get
        Set(value As Object)
            If (_valueType = eValueType.BCDRange Or _
                _valueType = eValueType.HexRange) Then                      ' BCD or hex range
                _stringMaximumValue = Right(StrDup(_byteSize * 2, "0") & CStr(value), _byteSize * 2)
                If (_stringDefaultMinimumLimit > _stringMaximumValue) Then
                    _stringDefaultMinimumLimit = _stringMaximumValue
                End If
                If (_stringDefaultMaximumLimit > _stringMaximumValue) Then
                    _stringDefaultMaximumLimit = _stringMaximumValue
                End If
                If (_stringMinimumLimit > _stringMaximumValue) Then
                    _stringMinimumLimit = _stringMaximumValue
                End If
                If (_stringMaximumLimit > _stringMaximumValue) Then
                    _stringMaximumLimit = _stringMaximumValue
                End If

            ElseIf (_valueType = eValueType.BCDValue Or _
                    _valueType = eValueType.HexValue) Then                  ' BCD or hex value
                _stringMaximumValue = Right(StrDup(_byteSize * 2, "0") & CStr(value), _byteSize * 2)
                If (_stringDefaultValue > _stringMaximumValue) Then
                    _stringDefaultValue = _stringMaximumValue
                End If
                If (_stringValue > _stringMaximumValue) Then
                    _stringValue = _stringMaximumValue
                End If

            ElseIf (_valueType = eValueType.IntegerRange) Then              ' Integer range
                _integerMaximumValue = CInt(value)
                If (_integerDefaultMinimumLimit > _integerMaximumValue) Then
                    _integerDefaultMinimumLimit = _integerMaximumValue
                End If
                If (_integerDefaultMaximumLimit > _integerMaximumValue) Then
                    _integerDefaultMaximumLimit = _integerMaximumValue
                End If
                If (_integerMinimumLimit > _integerMaximumValue) Then
                    _integerMinimumLimit = _integerMaximumValue
                End If
                If (_integerMaximumLimit > _integerMaximumValue) Then
                    _integerMaximumLimit = _integerMaximumValue
                End If

            ElseIf (_valueType = eValueType.IntegerValue) Then              ' Integer value
                _integerMaximumValue = CInt(value)
                If (_integerDefaultValue > _integerMaximumValue) Then
                    _integerDefaultValue = _integerMaximumValue
                End If
                If (_integerValue > _integerMaximumValue) Then
                    _integerValue = _integerMaximumValue
                End If

            ElseIf (_valueType = eValueType.SingleRange) Then               ' Single range
                _singleMaximumValue = CSng(Round(CDec(value), _decimals))
                If (_singleDefaultMinimumLimit > _singleMaximumValue) Then
                    _singleDefaultMinimumLimit = _singleMaximumValue
                End If
                If (_singleDefaultMaximumLimit > _singleMaximumValue) Then
                    _singleDefaultMaximumLimit = _singleMaximumValue
                End If
                If (_singleMinimumLimit > _singleMaximumValue) Then
                    _singleMinimumLimit = _singleMaximumValue
                End If
                If (_singleMaximumLimit > _singleMaximumValue) Then
                    _singleMaximumLimit = _singleMaximumValue
                End If

            ElseIf (_valueType = eValueType.SingleValue) Then               ' Single value
                _singleMaximumValue = CSng(Round(CDec(value), _decimals))
                If (_singleDefaultValue > _singleMaximumValue) Then
                    _singleDefaultValue = _singleMaximumValue
                End If
                If (_singleValue > _singleMaximumValue) Then
                    _singleValue = _singleMaximumValue
                End If
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Set
    End Property



    Public Property MinimumLimit As Object
        Get
            Try
                If (_valueType = eValueType.BCDRange Or
                    _valueType = eValueType.HexRange) Then
                    MinimumLimit = _stringMinimumLimit
                ElseIf (_valueType = eValueType.IntegerRange) Then
                    MinimumLimit = _integerMinimumLimit
                ElseIf (_valueType = eValueType.SingleRange) Then
                    MinimumLimit = _singleMinimumLimit
                Else
                    Throw New Exception("Property not supported by this type of recipe value.")
                End If
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try
        End Get
        Set(value As Object)
            Try
                If (_valueType = eValueType.BCDRange Or
                    _valueType = eValueType.HexRange) Then
                    _stringMinimumLimit = Right(StrDup(_byteSize * 2, "0") & CStr(value), _byteSize * 2)
                    If (_stringMinimumLimit < _stringMinimumValue) Then
                        _stringMinimumLimit = _stringMinimumValue
                    ElseIf (_stringMinimumLimit > _stringMaximumValue) Then
                        _stringMinimumLimit = _stringMaximumValue
                    End If
                ElseIf (_valueType = eValueType.IntegerRange) Then
                    _integerMinimumLimit = CInt(value)
                    If (_integerMinimumLimit < _integerMinimumValue) Then
                        _integerMinimumLimit = _integerMinimumValue
                    ElseIf (_integerMinimumLimit > _integerMaximumValue) Then
                        _integerMinimumLimit = _integerMaximumValue
                    End If
                ElseIf (_valueType = eValueType.SingleRange) Then
                    _singleMinimumLimit = CSng(value)
                    If (_singleMinimumLimit < _singleMinimumValue) Then
                        _singleMinimumLimit = _singleMinimumValue
                    ElseIf (_singleMinimumLimit > _singleMaximumValue) Then
                        _singleMinimumLimit = _singleMaximumValue
                    End If
                Else
                    Throw New Exception("Property not supported by this type of recipe value.")
                End If
            Catch ex As Exception
                Console.WriteLine(ex.ToString)
            End Try
        End Set
    End Property



    Public Property MinimumValue As Object
        Get
            If (_valueType = eValueType.BCDRange Or _
                _valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexRange Or _
                _valueType = eValueType.HexValue) Then
                MinimumValue = _stringMinimumValue
            ElseIf (_valueType = eValueType.IntegerRange Or _
                    _valueType = eValueType.IntegerValue) Then
                MinimumValue = _integerMinimumValue
            ElseIf (_valueType = eValueType.SingleRange Or _
                    _valueType = eValueType.SingleValue) Then
                MinimumValue = _singleMinimumValue
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Get
        Set(value As Object)
            If (_valueType = eValueType.BCDRange Or _
                _valueType = eValueType.HexRange) Then                      ' BCD or hex range
                _stringMinimumValue = Right(StrDup(_byteSize * 2, "0") & CStr(value), _byteSize * 2)
                If (_stringDefaultMinimumLimit < _stringMinimumValue) Then
                    _stringDefaultMinimumLimit = _stringMinimumValue
                End If
                If (_stringDefaultMaximumLimit < _stringMinimumValue) Then
                    _stringDefaultMaximumLimit = _stringMinimumValue
                End If
                If (_stringMinimumLimit < _stringMinimumValue) Then
                    _stringMinimumLimit = _stringMinimumValue
                End If
                If (_stringMaximumLimit < _stringMinimumValue) Then
                    _stringMaximumLimit = _stringMinimumValue
                End If

            ElseIf (_valueType = eValueType.BCDValue Or _
                    _valueType = eValueType.HexValue) Then                  ' BCD or hex value
                _stringMinimumValue = Right(StrDup(_byteSize * 2, "0") & CStr(value), _byteSize * 2)
                If (_stringDefaultValue < _stringMinimumValue) Then
                    _stringDefaultValue = _stringMinimumValue
                End If
                If (_stringValue < _stringMinimumValue) Then
                    _stringValue = _stringMinimumValue
                End If

            ElseIf (_valueType = eValueType.IntegerRange) Then              ' Integer range
                _integerMinimumValue = CInt(value)
                If (_integerDefaultMinimumLimit < _integerMinimumValue) Then
                    _integerDefaultMinimumLimit = _integerMinimumValue
                End If
                If (_integerDefaultMaximumLimit < _integerMinimumValue) Then
                    _integerDefaultMaximumLimit = _integerMinimumValue
                End If
                If (_integerMinimumLimit < _integerMinimumValue) Then
                    _integerMinimumLimit = _integerMinimumValue
                End If
                If (_integerMaximumLimit < _integerMinimumValue) Then
                    _integerMaximumLimit = _integerMinimumValue
                End If

            ElseIf (_valueType = eValueType.IntegerValue) Then              ' Integer value
                _integerMinimumValue = CInt(value)
                If (_integerDefaultValue < _integerMinimumValue) Then
                    _integerDefaultValue = _integerMinimumValue
                End If
                If (_integerValue < _integerMinimumValue) Then
                    _integerValue = _integerMinimumValue
                End If

            ElseIf (_valueType = eValueType.SingleRange) Then               ' Single range
                _singleMinimumValue = CSng(Round(CDec(value), _decimals))
                If (_singleDefaultMinimumLimit < _singleMinimumValue) Then
                    _singleDefaultMinimumLimit = _singleMinimumValue
                End If
                If (_singleDefaultMaximumLimit < _singleMinimumValue) Then
                    _singleDefaultMaximumLimit = _singleMinimumValue
                End If
                If (_singleMinimumLimit < _singleMinimumValue) Then
                    _singleMinimumLimit = _singleMinimumValue
                End If
                If (_singleMaximumLimit < _singleMinimumValue) Then
                    _singleMaximumLimit = _singleMinimumValue
                End If

            ElseIf (_valueType = eValueType.SingleValue) Then               ' Single value
                _singleMinimumValue = CSng(Round(CDec(value), _decimals))
                If (_singleDefaultValue < _singleMinimumValue) Then
                    _singleDefaultValue = _singleMinimumValue
                End If
                If (_singleValue < _singleMinimumValue) Then
                    _singleValue = _singleMinimumValue
                End If
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Set
    End Property



    Public ReadOnly Property StringMaximumLimit As String
        Get
            Dim formatString As String

            If (_valueType = eValueType.BCDRange Or _
                _valueType = eValueType.HexRange) Then
                StringMaximumLimit = _stringMaximumLimit
            ElseIf (_valueType = eValueType.IntegerRange) Then
                StringMaximumLimit = _integerMaximumLimit.ToString
            ElseIf (_valueType = eValueType.SingleRange) Then
                formatString = "0"
                If (_decimals > 0) Then
                    formatString = formatString & "." & StrDup(_decimals, "0")
                End If
                StringMaximumLimit = _singleMaximumLimit.ToString(formatString)
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Get
    End Property



    Public ReadOnly Property StringMinimumLimit As String
        Get
            Dim formatString As String

            If (_valueType = eValueType.BCDRange Or _
                _valueType = eValueType.HexRange) Then
                StringMinimumLimit = _stringMinimumLimit
            ElseIf (_valueType = eValueType.IntegerRange) Then
                StringMinimumLimit = _integerMinimumLimit.ToString
            ElseIf (_valueType = eValueType.SingleRange) Then
                formatString = "0"
                If (_decimals > 0) Then
                    formatString = formatString & "." & StrDup(_decimals, "0")
                End If
                StringMinimumLimit = _singleMinimumLimit.ToString(formatString)
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
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
            ElseIf (_valueType = eValueType.BooleanValue) Then
                StringValue = CStr(IIf(_booleanValue, _stringValueTrue, _stringValueFalse))
            ElseIf (_valueType = eValueType.IntegerValue) Then
                StringValue = _integerValue.ToString
            ElseIf (_valueType = eValueType.SingleValue) Then
                formatString = "0"
                If (_decimals > 0) Then
                    formatString = formatString & "." & StrDup(_decimals, "0")
                End If
                StringValue = _singleValue.ToString(formatString)
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Get
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
                _valueType = eValueType.HexValue) Then
                Value = _stringValue
            ElseIf (_valueType = eValueType.BooleanValue) Then
                Value = _booleanValue
            ElseIf (_valueType = eValueType.IntegerValue) Then
                Value = _integerValue
            ElseIf (_valueType = eValueType.SingleValue) Then
                Value = _singleValue
            ElseIf (_valueType = eValueType.StringValue) Then
                Value = _stringValue
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
            End If
        End Get
        Set(value As Object)
            If (_valueType = eValueType.BCDValue Or _
                _valueType = eValueType.HexValue) Then
                _stringValue = Right(StrDup(_byteSize * 2, "0") & CStr(value), _byteSize * 2)
                If (_stringValue < _stringMinimumValue) Then
                    _stringValue = _stringMinimumValue
                ElseIf (_stringValue > _stringMaximumValue) Then
                    _stringValue = _stringMaximumValue
                End If
            ElseIf (_valueType = eValueType.BooleanValue) Then
                _booleanValue = CBool(value)
            ElseIf (_valueType = eValueType.IntegerValue) Then
                _integerValue = CInt(value)
                If (_integerValue < _integerMinimumValue) Then
                    _integerValue = _integerMinimumValue
                ElseIf (_integerValue > _integerMaximumValue) Then
                    _integerValue = _integerMaximumValue
                End If
            ElseIf (_valueType = eValueType.SingleValue) Then
                _singleValue = CSng(value)
                If (_singleValue < _singleMinimumValue) Then
                    _singleValue = _singleMinimumValue
                ElseIf (_singleValue > _singleMaximumValue) Then
                    _singleValue = _singleMaximumValue
                End If
            ElseIf (_valueType = eValueType.StringValue) Then
                _stringValue = CStr(value)
                If (_stringValue.Length > _maximumLength) Then
                    _stringValue = Left(_stringValue, _maximumLength)
                End If
            Else
                Throw New Exception("Property not supported by this type of recipe value.")
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
        _units = ""

        _booleanDefaultValue = False
        _booleanValue = False

        _integerMinimumValue = 0
        _integerMaximumValue = 0
        _integerDefaultMinimumLimit = 0
        _integerDefaultMaximumLimit = 0
        _integerDefaultValue = 0
        _integerMinimumLimit = 0
        _integerMaximumLimit = 0
        _integerValue = 0

        _decimals = 0
        _singleMinimumValue = 0
        _singleMaximumValue = 0
        _singleDefaultMinimumLimit = 0
        _singleDefaultMaximumLimit = 0
        _singleDefaultValue = 0
        _singleMinimumLimit = 0
        _singleMaximumLimit = 0
        _singleValue = 0

        _byteSize = 0
        _maximumLength = 0
        _stringMinimumValue = ""
        _stringMaximumValue = ""
        _stringDefaultMinimumLimit = ""
        _stringDefaultMaximumLimit = ""
        _stringDefaultValue = ""
        _stringMinimumLimit = ""
        _stringMaximumLimit = ""
        _stringValue = ""

        _valueType = valueType
    End Sub



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Class


