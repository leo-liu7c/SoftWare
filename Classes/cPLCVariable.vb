Option Explicit On

Public Class cPLCVariable
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Variable enumeration
    Public Enum eType
        BoolVariable = 0
        ByteVariable = 1
        WordVariable = 2
        DoubleWordVariable = 3
        IntegerVariable = 4
        DoubleIntegerVariable = 5
        RealVariable = 6
        StringVariable = 7
    End Enum



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    Private _address As String
    Private _description As String
    Private _length As Integer
    Private _type As eType
    Private _value As Object



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public Property Address As String
        Get
            Address = _address
        End Get
        Set(propertyValue As String)
            _address = propertyValue
        End Set
    End Property



    Public Property Description As String
        Get
            Description = _description
        End Get
        Set(propertyValue As String)
            _description = propertyValue
        End Set
    End Property



    Public Property Length As Integer
        Get
            Length = _length
        End Get
        Set(propertyValue As Integer)
            _length = propertyValue
        End Set
    End Property



    Public ReadOnly Property Type As eType
        Get
            Type = _type
        End Get
    End Property



    Public Property Value As Object
        Get
            Value = _value
        End Get
        Set(propertyValue As Object)
            If (_type = eType.BoolVariable) Then
                _value = CBool(propertyValue)
            ElseIf (_type = eType.ByteVariable) Then
                _value = CByte(propertyValue)
            ElseIf (_type = eType.WordVariable) Then
                _value = CType(propertyValue, UInt16)
            ElseIf (_type = eType.DoubleWordVariable) Then
                _value = CType(propertyValue, UInt32)
            ElseIf (_type = eType.IntegerVariable) Then
                _value = CType(propertyValue, Int16)
            ElseIf (_type = eType.DoubleIntegerVariable) Then
                _value = CType(propertyValue, Int32)
            ElseIf (_type = eType.RealVariable) Then
                _value = CSng(propertyValue)
            Else
                _value = CStr(propertyValue)
            End If
        End Set
    End Property



    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+
    Protected Overrides Sub Finalize()
        _value = Nothing
        MyBase.Finalize()
    End Sub



    Public Sub New(ByVal type As eType)
        _type = type
        If (_type = eType.BoolVariable) Then
            _value = New Boolean = False
        ElseIf (_type = eType.ByteVariable) Then
            _value = New Byte = 0
        ElseIf (_type = eType.WordVariable) Then
            _value = New UInt16 = 0
        ElseIf (_type = eType.DoubleWordVariable) Then
            _value = New UInt32 = 0
        ElseIf (_type = eType.IntegerVariable) Then
            _value = New Int16 = 0
        ElseIf (_type = eType.DoubleIntegerVariable) Then
            _value = New Int32 = 0
        ElseIf (_type = eType.RealVariable) Then
            _value = New Single = 0
        Else
            _value = New String("")
        End If

    End Sub



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Class
