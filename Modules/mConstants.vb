Option Explicit On
Option Strict On

Module mConstants
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+
    Public Const SettingsPath = "..\..\..\Settings\Global Settings.txt"

    Public Const _bconnected As Boolean = False

    Public Const HardwareEnabled_PLC = _bconnected
    Public Const HardwareEnabled_VIPA = _bconnected
    Public Const HardwareEnabled_TTI = _bconnected
    Public Const HardwareEnabled_NI = _bconnected
    Public Const HardwareEnabled_Keyence = _bconnected

    Public Const BasePath = "..\..\.."
    ' Xnet LDF file
    Public Const dbBasePath = "..\..\..\Settings\Ldf\"

    Public Const WriteFilePoint As Boolean = True
    Public Const SaveFSScreenshot As Boolean = True


    Public Simulation_Test As Boolean = True

    Public SoftwareVersion As String = "V1.02"
    Public SoftwareDate As String = "20200717"

    'Software update list
    'Version| Contents                             | Author      |Date
    'V1.01  | New Update                           | YAN.Qian    |20200630
    'V1.02  | Update VIPA Read DI                  | YAN.Qian    |20200717

    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+


    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Module