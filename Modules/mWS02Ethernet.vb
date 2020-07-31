Option Explicit On
Option Strict On

Module mWS02Ethernet
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    ' Input enumeration
    Public Enum eInput
        Reserve = 0
        PartTypeNumber = 1
        Test_Mode = 2
        PartModel = 3
        Reference = 4
        UniqueNumber = 5
        HW_Version = 6
        SW_Version = 7
        NVM_Version = 8
        SemiFGSN = 9
        Side_Barcode = 10
        FixtureID = 11

        Count = 12
    End Enum

    ' Outputs enumeration
    Public Enum eOutput
        Reserve = 0
        ResultCode = 1
        Reference = 2
        UniqueNumber = 3
        HW_Version = 4
        SW_Version = 5
        NVM_Version = 6
        CustomerPartNumber = 7
        LineNo = 8
        IDCode = 9
        Test_Enable_Camera = 10
        Test_Enable_PLC = 11

        Count = 12
    End Enum



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables
    Private _inputDB As cPLCDataBlock
    Private _outputDB As cPLCDataBlock
    Private Const _HardwareEnabled = mConstants.HardwareEnabled_PLC


    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+
    Public ReadOnly Property InputValue(ByVal index As eInput) As Object
        Get
            InputValue = _inputDB.Variable(index).Value
        End Get
    End Property



    Public Property OutputValue(ByVal index As eOutput) As Object
        Get
            OutputValue = _outputDB.Variable(index).Value
        End Get
        Set(ByVal value As Object)
            _outputDB.Variable(index).Value = value
        End Set
    End Property



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Function PowerDown() As Boolean
        ' Return False
        PowerDown = False
    End Function



    Public Function PowerUp(ByVal inputDBConfigurationPath As String, _
                            ByVal outputDBConfigurationPath As String) As Boolean
        ' Load the input data block configuration
        PowerUp = cPLCDataBlock.LoadConfiguration(_inputDB, inputDBConfigurationPath)
        ' Load the output data block configuration
        PowerUp = PowerUp Or cPLCDataBlock.LoadConfiguration(_outputDB, outputDBConfigurationPath)
    End Function



    Public Function ReadInputDataBlock() As Boolean
        If (_HardwareEnabled) Then
            ' Read the input data block
            ReadInputDataBlock = mPLC.ReadDataBlock(_inputDB, 0, _inputDB.VariableCount - 1)
        Else
            ReadInputDataBlock = False
        End If
    End Function



    Public Function WriteOutputDataBlock() As Boolean
        If (_HardwareEnabled) Then
            ' Write the output data block
            WriteOutputDataBlock = mPLC.WriteDataBlock(_outputDB, 0, _outputDB.VariableCount - 1)
        Else
            WriteOutputDataBlock = False
        End If
    End Function

    Public Function DataDescription(ByVal _Data As Integer) As String
        ' Return the phase description
        Select Case _Data
            Case 0
                DataDescription = "WS02-Reserve"
            Case 1
                DataDescription = "PartTypeNumber "
            Case 2
                DataDescription = "Test_Mode "
            Case 3
                DataDescription = "PartModel "
            Case 4
                DataDescription = "Reference "
            Case 5
                DataDescription = "WS02-UniqueNumber "
            Case 6
                DataDescription = "HW_Version "
            Case 7
                DataDescription = "SW_Version "
            Case 8
                DataDescription = "NVM_Version "
            Case Else
                DataDescription = String.Format("Value {0} unknown", _Data)
        End Select
    End Function

End Module
