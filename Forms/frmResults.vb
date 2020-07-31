Option Explicit On
Option Strict Off

Imports System
Imports System.IO


Public Class frmResults
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+

    Public WiliIndexShow As Integer

    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private constants
    Private Const _maxRowCount = 150
    Private Graphe_Type As Integer

    ' Private variables
    Private _WS02Results As cWS02Results
    Private _WS03Results As cWS03Results
    Private _WS04Results As cWS04Results
    Private _WS05Results As cWS05Results

    ' References to station WS02 result values
    Private _WS02GeneralInformation(0 To _maxRowCount - 1) As Object
    Private _WS02PowerSypply(0 To _maxRowCount - 1) As Object
    Private _WS02CurrentMode(0 To _maxRowCount - 1) As Object
    Private _WS02SerialNumber(0 To _maxRowCount - 1) As Object
    Private _WS02Shape(0 To _maxRowCount - 1) As Object
    Private _WS02Shape2(0 To _maxRowCount - 1) As Object
    Private _WS02OtherTest(0 To _maxRowCount - 1) As Object
    Private _WS02BacklightMS(0 To _maxRowCount - 1) As Object
    Private _WS02BacklightMA(0 To _maxRowCount - 1) As Object
    Private _WS02BacklightFL(0 To _maxRowCount - 1) As Object
    Private _WS02BacklightWLF(0 To _maxRowCount - 1) As Object
    Private _WS02BacklightWLR(0 To _maxRowCount - 1) As Object
    Private _WS02BacklightCL(0 To _maxRowCount - 1) As Object
    Private _WS02TellTale(0 To _maxRowCount - 1) As Object
    Private _WS02EMSTraceability(0 To _maxRowCount - 1) As Object
    Private _WS02MMSTraceability(0 To _maxRowCount - 1) As Object
    Private _WS02AnalogInput(0 To _maxRowCount - 1) As Object
    Private _WS02PWMOutput_1(0 To _maxRowCount - 1) As Object
    Private _WS02PWMOutput_2(0 To _maxRowCount - 1) As Object
    Private _WS02DO_UP_FrontPassenger(0 To _maxRowCount - 1) As Object
    Private _WS02DO_DN_FrontPassenger(0 To _maxRowCount - 1) As Object
    Private _WS02DO_Switch_Inhibition(0 To _maxRowCount - 1) As Object
    Private _WS02DO_UP_RearRight(0 To _maxRowCount - 1) As Object
    Private _WS02DO_DN_RearRight(0 To _maxRowCount - 1) As Object
    Private _WS02DO_UP_RearLeft(0 To _maxRowCount - 1) As Object
    Private _WS02DO_DN_RearLeft(0 To _maxRowCount - 1) As Object
    Private _WS02DO_CDE_RBT_RTRV_G_Fold(0 To _maxRowCount - 1) As Object
    Private _WS02DO_CDE_RBT_RTRV_G_UnFold(0 To _maxRowCount - 1) As Object
    Private _WS02DO_CDE_RBT_RTRV_D_Fold(0 To _maxRowCount - 1) As Object
    Private _WS02DO_CDE_RBT_RTRV_D_UnFold(0 To _maxRowCount - 1) As Object
    Private _WS02DO_CDE_RTRV_HB_G(0 To _maxRowCount - 1) As Object
    Private _WS02DO_CDE_RTRV_DG_G(0 To _maxRowCount - 1) As Object
    Private _WS02DO_CDE_RTRV_HB_D(0 To _maxRowCount - 1) As Object
    Private _WS02DO_CDE_RTRV_DG_D(0 To _maxRowCount - 1) As Object

    ' References to station WS03 result values
    Private _WS03GeneralInformation(0 To _maxRowCount - 1) As Object
    Private _WS03PowerSypply(0 To _maxRowCount - 1) As Object
    Private _WS03ValeoSerialNumber(0 To _maxRowCount - 1) As Object
    Private _WS03FrontLeftPull(0 To _maxRowCount - 1) As Object
    Private _WS03FrontLeftPush(0 To _maxRowCount - 1) As Object
    Private _WS03FrontRightPull(0 To _maxRowCount - 1) As Object
    Private _WS03FrontRightPush(0 To _maxRowCount - 1) As Object
    Private _WS03PushChildrenLock(0 To _maxRowCount - 1) As Object
    Private _WS03FinalState(0 To _maxRowCount - 1) As Object

    ' References to station WS04 result values
    Private _WS04GeneralInformation(0 To _maxRowCount - 1) As Object
    Private _WS04PowerSypply(0 To _maxRowCount - 1) As Object
    Private _WS04ValeoSerialNumber(0 To _maxRowCount - 1) As Object
    Private _WS04RearLeftPull(0 To _maxRowCount - 1) As Object
    Private _WS04RearLeftPush(0 To _maxRowCount - 1) As Object
    Private _WS04RearRightPull(0 To _maxRowCount - 1) As Object
    Private _WS04RearRightPush(0 To _maxRowCount - 1) As Object
    Private _WS04PushMirrorFolding(0 To _maxRowCount - 1) As Object
    Private _WS04FinalState(0 To _maxRowCount - 1) As Object

    ' References to station WS05 result values
    Private _WS05GeneralInformation(0 To _maxRowCount - 1) As Object
    Private _WS05PowerSypply(0 To _maxRowCount - 1) As Object
    Private _WS05ValeoSerialNumber(0 To _maxRowCount - 1) As Object
    Private _WS05MirrorUP(0 To _maxRowCount - 1) As Object
    Private _WS05MirrorDN(0 To _maxRowCount - 1) As Object
    Private _WS05MirrorMR(0 To _maxRowCount - 1) As Object
    Private _WS05MirrorML(0 To _maxRowCount - 1) As Object
    Private _WS05MirrorSR(0 To _maxRowCount - 1) As Object
    Private _WS05MirrorSL(0 To _maxRowCount - 1) As Object
    Private _WS05FinalState(0 To _maxRowCount - 1) As Object

    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+

    Private Sub frmResults_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Disable the timer monitor
        tmrMonitor.Enabled = False
    End Sub



    Private Sub frmResults_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Initialize the reference to the station results
        _WS02Results = mWS02Main.Results
        _WS03Results = mWS03Main.Results
        _WS04Results = mWS04Main.Results
        _WS05Results = mWS05Main.Results

        ' Initialize the station WS02 tabs
        InitStationWS02Tabs()
        ' Initialize the station WS03 tabs
        InitStationWS03Tabs()
        ' Initialize the station WS04 tabs
        InitStationWS04Tabs()
        ' Initialize the station WS05 tabs
        InitStationWS05Tabs()
        '
        TabSW090_100.SendToBack()
        ' Show the results
        ShowResults()
        ' Initialize the status bar
        mStatusBar.Initialize(ssStatusBar)
        ' Configurate and enable the timer monitor
        tmrMonitor.Interval = 500
        tmrMonitor.Enabled = True
    End Sub




    Private Sub InitStationWS02Tabs()
        InitStationWS02TabsGeneralInformation()
        InitStationWS02TabsPowerOn()
        InitStationWS02TabsCurrentMode()
        InitStationWS02TabsSerialNumber()
        InitStationWS02TabsShape()
        InitStationWS02TabsBACKLIGHT()
        InitStationWS02TabsTELLTALE()
        InitStationWS02TabsMMSTraceability()
        InitStationWS02TabsEMSTraceability()
        InitStationWS02TabsAnalogInput()

        InitStationWS02DO_UP_FrontPassenger()
        InitStationWS02DO_DN_FrontPassenger()
        InitStationWS02DO_Switch_Inhibition()
        InitStationWS02DO_UP_RearRight()
        InitStationWS02DO_DN_RearRight()
        InitStationWS02DO_UP_RearLeft()
        InitStationWS02DO_DN_RearLeft()
        InitStationWS02DO_CDE_RBT_RTRV_G()
        InitStationWS02DO_CDE_RBT_RTRV_D()
        InitStationWS02DO_CDE_HB_RTRV_G()
        InitStationWS02DO_CDE_DG_RTRV_G()
        InitStationWS02DO_CDE_HB_RTRV_D()
        InitStationWS02DO_CDE_DG_RTRV_D()

        InitStationWS02TabsPWMOutput()
    End Sub

    'Part A
    Private Sub InitStationWS02TabsGeneralInformation()
        ' General information
        _WS02GeneralInformation(0) = New String("General information".ToCharArray)
        _WS02GeneralInformation(1) = _WS02Results.TestDate
        _WS02GeneralInformation(2) = _WS02Results.TestTime
        _WS02GeneralInformation(3) = _WS02Results.RecipeName
        _WS02GeneralInformation(4) = _WS02Results.RecipeModifyDate
        _WS02GeneralInformation(5) = _WS02Results.RecipeModifyTime
        _WS02GeneralInformation(6) = _WS02Results.PartUniqueNumber
        _WS02GeneralInformation(7) = _WS02Results.PartTypeNumber

        InitResultsTable(dgvWS02GeneralInformation, 8, _WS02GeneralInformation, 730)

    End Sub


    Private Sub InitStationWS02TabsPowerOn()
        _WS02PowerSypply(0) = New String("Power On ".ToCharArray)
        _WS02PowerSypply(1) = _WS02Results.PowerUp_VBAT

        InitResultsTable(dgvWS02PowerUp, 2, _WS02PowerSypply, 730)
    End Sub

    Private Sub InitStationWS02TabsCurrentMode()
        _WS02CurrentMode(0) = New String("Mormal Mode Current ".ToCharArray)
        _WS02CurrentMode(1) = _WS02Results.PowerUp_Ibat

        InitResultsTable(dgvWS02CurrentMode, 2, _WS02CurrentMode, 730)
    End Sub



    Private Sub InitStationWS02TabsSerialNumber()
        _WS02SerialNumber(0) = New String("Check Serial Number ".ToCharArray)
        _WS02SerialNumber(1) = _WS02Results.Valeo_Serial_Number

        InitResultsTable(dgvWS02ValeoSerialNumber, 2, _WS02SerialNumber, 730)

    End Sub

    Private Sub InitStationWS02TabsShape()
        _WS02Shape(0) = New String("Push Shape ".ToCharArray)
        _WS02Shape(1) = _WS02Results.MINIMUM_CONFORMITY_PUSH_SELECT_LEFT_MIRROR
        _WS02Shape(2) = _WS02Results.MINIMUM_CONFORMITY_PUSH_SELECT_RIGHT_MIRROR
        _WS02Shape(3) = _WS02Results.MINIMUM_CONFORMITY_PUSH_FOLDING_MIRROR
        _WS02Shape(4) = _WS02Results.MINIMUM_CONFORMITY_PUSH_ADJUST_UP
        _WS02Shape(5) = _WS02Results.MINIMUM_CONFORMITY_PUSH_ADJUST_DOWN
        _WS02Shape(6) = _WS02Results.MINIMUM_CONFORMITY_PUSH_ADJUST_LEFT
        _WS02Shape(7) = _WS02Results.MINIMUM_CONFORMITY_PUSH_ADJUST_RIGHT
        _WS02Shape(8) = _WS02Results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_LEFT
        _WS02Shape(9) = _WS02Results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_FRONT_RIGHT
        _WS02Shape(10) = _WS02Results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_LEFT
        _WS02Shape(11) = _WS02Results.MINIMUM_CONFORMITY_WINDOWS_LIFTER_REAR_RIGHT
        _WS02Shape(12) = _WS02Results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK
        _WS02Shape(13) = _WS02Results.MINIMUM_CONFORMITY_PUSH_CHILDREN_LOCK2
        _WS02Shape(14) = _WS02Results.CONFORMITY_Adatper_Front
        _WS02Shape(15) = _WS02Results.CONFORMITY_Adatper_Rear
        _WS02Shape(16) = New String("Push defect area ".ToCharArray)
        _WS02Shape(17) = _WS02Results.DEFECT_AREA_PUSH_SELECT_LEFT_MIRROR
        _WS02Shape(18) = _WS02Results.DEFECT_AREA_PUSH_SELECT_RIGHT_MIRROR
        _WS02Shape(19) = _WS02Results.DEFECT_AREA_PUSH_FOLDING_MIRROR
        _WS02Shape(20) = _WS02Results.DEFECT_AREA_PUSH_ADJUST_UP
        _WS02Shape(21) = _WS02Results.DEFECT_AREA_PUSH_ADJUST_DOWN
        _WS02Shape(22) = _WS02Results.DEFECT_AREA_PUSH_ADJUST_LEFT
        _WS02Shape(23) = _WS02Results.DEFECT_AREA_PUSH_ADJUST_RIGHT
        _WS02Shape(24) = _WS02Results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_LEFT
        _WS02Shape(25) = _WS02Results.DEFECT_AREA_WINDOWS_LIFTER_FRONT_RIGHT
        _WS02Shape(26) = _WS02Results.DEFECT_AREA_WINDOWS_LIFTER_REAR_LEFT
        _WS02Shape(27) = _WS02Results.DEFECT_AREA_WINDOWS_LIFTER_REAR_RIGHT
        _WS02Shape(28) = _WS02Results.DEFECT_AREA_PUSH_CHILDREN_LOCK
        _WS02Shape(29) = _WS02Results.DEFECT_AREA_PUSH_CHILDREN_LOCK2

        InitResultsTable(dgvWS02Shape, 30, _WS02Shape, 800)

        Dim i As Integer = 0
        _WS02Shape2(i) = New String("Mirror Ring RGB ".ToCharArray) ： i += 1
        _WS02Shape2(i) = _WS02Results.Ring_Red ： i += 1
        _WS02Shape2(i) = _WS02Results.Ring_Green ： i += 1
        _WS02Shape2(i) = _WS02Results.Ring_Blue ： i += 1

        _WS02Shape2(i) = New String("Points and Customer interfaces ".ToCharArray) ： i += 1
        _WS02Shape2(i) = _WS02Results.Points_On_Front_Left ： i += 1
        _WS02Shape2(i) = _WS02Results.Points_On_Front_Right ： i += 1
        _WS02Shape2(i) = _WS02Results.Points_On_Rear_Left ： i += 1
        _WS02Shape2(i) = _WS02Results.Points_On_Rear_Right ： i += 1
        For index = 0 To 19
            _WS02Shape2(i) = _WS02Results.Customer_Interface(index) ： i += 1
        Next

        InitResultsTable(dgvWS02Shape2, i, _WS02Shape2, 800)
    End Sub

    Private Sub InitStationWS02TabsBacklight()
        Dim i As Integer
        i = 0
        _WS02BacklightMS(i) = New String("Checked Value Push Selection Left Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_Backlight_RSQ : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_backlight_intensity : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_backlight_homogeneity : i = i + 1
        _WS02BacklightMS(i) = New String("Information Value Push Selection Left Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_backlight_red : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_backlight_green : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_backlight_blue : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_backlight_intensity_Camera : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_backlight_Saturation : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_Backlight_x : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_Backlight_y : i = i + 1
        _WS02BacklightMS(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_LEFT_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightMS(i) = New String("backlight defect area ".ToCharArray) : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_LEFT_MIRROR : i = i + 1

        _WS02BacklightMS(i) = New String("Checked Value Push Selection Right Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_Backlight_RSQ : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_backlight_intensity : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_backlight_homogeneity : i = i + 1
        _WS02BacklightMS(i) = New String("Information Value Push Selection Right Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_backlight_red : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_backlight_green : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_backlight_blue : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_backlight_intensity_Camera : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_backlight_Saturation : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_Backlight_x : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_Backlight_y : i = i + 1
        _WS02BacklightMS(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.Push_SELECT_RIGHT_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightMS(i) = New String("backlight defect area ".ToCharArray) : i = i + 1
        _WS02BacklightMS(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_PUSH_SELECT_RIGHT_MIRROR : i = i + 1

        InitResultsTable(dgvWS02Backlight_MS, i, _WS02BacklightMS, 700)

        i = 0
        _WS02BacklightFL(i) = New String("Checked Value Push Folding Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_Backlight_RSQ : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_backlight_intensity : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_backlight_homogeneity : i = i + 1
        _WS02BacklightFL(i) = New String("Information Value Push Folding Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_backlight_red : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_backlight_green : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_backlight_blue : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_backlight_intensity_Camera : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_backlight_Saturation : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_Backlight_x : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_Backlight_y : i = i + 1
        _WS02BacklightFL(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.Push_FOLDING_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightFL(i) = New String("backlight defect area ".ToCharArray) : i = i + 1
        _WS02BacklightFL(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_PUSH_FOLDING_MIRROR : i = i + 1

        InitResultsTable(dgvWS02Backlight_FL, i, _WS02BacklightFL, 700)

        i = 0
        _WS02BacklightMA(i) = New String("Checked Value Push Adjustement UP Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_Backlight_RSQ : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_backlight_intensity : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_backlight_homogeneity : i = i + 1
        _WS02BacklightMA(i) = New String("Information Value Push Adjustement UP Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_backlight_red : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_backlight_green : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_backlight_blue : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_backlight_intensity_Camera : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_backlight_Saturation : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_Backlight_x : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_Backlight_y : i = i + 1
        _WS02BacklightMA(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_UP_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightMA(i) = New String("backlight defect area ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_UP : i = i + 1

        _WS02BacklightMA(i) = New String("Checked Value Push Adjustement RIGHT Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_Backlight_RSQ : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_backlight_intensity : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_backlight_homogeneity : i = i + 1
        _WS02BacklightMA(i) = New String("Information Value Push Adjustement RIGHT Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_backlight_red : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_backlight_green : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_backlight_blue : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_backlight_intensity_Camera : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_backlight_Saturation : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_Backlight_x : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_Backlight_y : i = i + 1
        _WS02BacklightMA(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_RIGHT_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightMA(i) = New String("backlight defect area ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_RIGHT : i = i + 1

        _WS02BacklightMA(i) = New String("Checked Value Push Adjustement LEFT Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_Backlight_RSQ : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_backlight_intensity : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_backlight_homogeneity : i = i + 1
        _WS02BacklightMA(i) = New String("Information Value Push Adjustement LEFT Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_backlight_red : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_backlight_green : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_backlight_blue : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_backlight_intensity_Camera : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_backlight_Saturation : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_Backlight_x : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_Backlight_y : i = i + 1
        _WS02BacklightMA(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_LEFT_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightMA(i) = New String("backlight defect area ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_LEFT : i = i + 1

        _WS02BacklightMA(i) = New String("Checked Value Push Adjustement DOWN Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_Backlight_RSQ : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_backlight_intensity : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_backlight_homogeneity : i = i + 1
        _WS02BacklightMA(i) = New String("Information Value Push Adjustement DOWN Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_backlight_red : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_backlight_green : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_backlight_blue : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_backlight_intensity_Camera : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_backlight_Saturation : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_Backlight_x : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_Backlight_y : i = i + 1
        _WS02BacklightMA(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.Push_ADJUST_DOWN_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightMA(i) = New String("backlight defect area ".ToCharArray) : i = i + 1
        _WS02BacklightMA(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_PUSH_ADJUST_DOWN : i = i + 1

        InitResultsTable(dgvWS02Backlight_MA, i, _WS02BacklightMA, 700)

        i = 0
        _WS02BacklightWLF(i) = New String("Checked Value Front LEFT Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_RSQ : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_backlight_homogeneity : i = i + 1
        _WS02BacklightWLF(i) = New String("Information Value Front LEFT Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_backlight_red : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_backlight_green : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_backlight_blue : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_backlight_intensity_Camera : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_backlight_Saturation : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_x : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_y : i = i + 1
        _WS02BacklightWLF(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightWLF(i) = New String("backlight defect area ".ToCharArray) : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_LEFT : i = i + 1

        _WS02BacklightWLF(i) = New String("Checked Value Front RIGHT Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_RSQ : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_homogeneity : i = i + 1
        _WS02BacklightWLF(i) = New String("Information Value Front RIGHT Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_red : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_green : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_blue : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_intensity_Camera : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_backlight_Saturation : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_x : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_y : i = i + 1
        _WS02BacklightWLF(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightWLF(i) = New String("backlight defect area ".ToCharArray) : i = i + 1
        _WS02BacklightWLF(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_FRONT_RIGHT : i = i + 1

        InitResultsTable(dgvWS02Backlight_WLF, i, _WS02BacklightWLF, 700)

        i = 0
        _WS02BacklightWLR(i) = New String("Checked Value REAR LEFT Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_RSQ : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_backlight_homogeneity : i = i + 1
        _WS02BacklightWLR(i) = New String("Information Value REAR LEFT Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_backlight_red : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_backlight_green : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_backlight_blue : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_backlight_intensity_Camera : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_backlight_Saturation : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_x : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_y : i = i + 1
        _WS02BacklightWLR(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightWLR(i) = New String("backlight defect area ".ToCharArray) : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_LEFT : i = i + 1

        _WS02BacklightWLR(i) = New String("Checked Value REAR RIGHT Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_RSQ : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_backlight_homogeneity : i = i + 1
        _WS02BacklightWLR(i) = New String("Information Value REAR RIGHT Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_backlight_red : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_backlight_green : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_backlight_blue : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_backlight_intensity_Camera : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_backlight_Saturation : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_x : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_y : i = i + 1
        _WS02BacklightWLR(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightWLR(i) = New String("backlight defect area ".ToCharArray) : i = i + 1
        _WS02BacklightWLR(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_WINDOWS_LIFTER_REAR_RIGHT : i = i + 1

        InitResultsTable(dgvWS02Backlight_WLR, i, _WS02BacklightWLR, 700)

        i = 0
        _WS02BacklightCL(i) = New String("Checked Value CHILDREN LOCK Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_RSQ : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_backlight_intensity : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_backlight_homogeneity : i = i + 1
        _WS02BacklightCL(i) = New String("Information Value CHILDREN LOCK Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_backlight_red : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_backlight_green : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_backlight_blue : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_backlight_intensity_Camera : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_backlight_Saturation : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_x : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_y : i = i + 1
        _WS02BacklightCL(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightCL(i) = New String("backlight defect area ".ToCharArray) : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK : i = i + 1

        _WS02BacklightCL(i) = New String("Checked Value CHILDREN LOCK Backlight2 ".ToCharArray) : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_Backlight_RSQ : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_backlight_intensity : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_backlight_homogeneity : i = i + 1
        _WS02BacklightCL(i) = New String("Information Value CHILDREN LOCK2 Backlight ".ToCharArray) : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_backlight_red : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_backlight_green : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_backlight_blue : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_backlight_intensity_Camera : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_backlight_DominantWavelenght : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_backlight_Saturation : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_Backlight_x : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_Backlight_y : i = i + 1
        _WS02BacklightCL(i) = New String("Setting Polygon ".ToCharArray) : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_Backlight_Polygon_Axy : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_Backlight_Polygon_Bxy : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_Backlight_Polygon_Cxy : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_Backlight_Polygon_Dxy : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_Backlight_Polygon_Exy : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.Push_CHILDREN_LOCK2_Backlight_Polygon_Fxy : i = i + 1
        _WS02BacklightCL(i) = New String("backlight defect area2 ".ToCharArray) : i = i + 1
        _WS02BacklightCL(i) = _WS02Results.DEFECT_AREA_BACKLIGHT_PUSH_CHILDREN_LOCK2 : i = i + 1

        InitResultsTable(dgvWS02Backlight_CL, i, _WS02BacklightCL, 700)

    End Sub


    Private Sub InitStationWS02TabsTellTale()
        Dim i As Integer
        i = 0
        _WS02TellTale(i) = New String("Checked Value Selection Mirror Left TellTale ".ToCharArray) : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_LEFT_LED_saturation : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_LEFT_LED_intensity : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_LEFT_LED_WaveLenght : i = i + 1
        _WS02TellTale(i) = New String("Information Value Selection Mirror Left TellTale ".ToCharArray) : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_LEFT_LED_red : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_LEFT_LED_green : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_LEFT_LED_blue : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_LEFT_LED_x : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_LEFT_LED_y : i = i + 1
        _WS02TellTale(i) = New String("Settings Polygon".ToCharArray) : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_LEFT_LED_Polygon_Axy : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_LEFT_LED_Polygon_Bxy : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_LEFT_LED_Polygon_Cxy : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_LEFT_LED_Polygon_Dxy : i = i + 1
        _WS02TellTale(i) = New String("telltale defect area ".ToCharArray) : i = i + 1
        _WS02TellTale(i) = _WS02Results.DEFECT_AREA_TELLTALE_SELECT_LEFT_MIRROR : i = i + 1

        _WS02TellTale(i) = New String("Checked Value Selection Mirror Right TellTale ".ToCharArray) : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_RIGHT_LED_saturation : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_RIGHT_LED_intensity : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_RIGHT_LED_WaveLenght : i = i + 1
        _WS02TellTale(i) = New String("Information Value Selection Mirror Right TellTale ".ToCharArray) : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_RIGHT_LED_red : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_RIGHT_LED_green : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_RIGHT_LED_blue : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_RIGHT_LED_x : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_RIGHT_LED_y : i = i + 1
        _WS02TellTale(i) = New String("Settings Polygon".ToCharArray) : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_RIGHT_LED_Polygon_Axy : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_RIGHT_LED_Polygon_Bxy : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_RIGHT_LED_Polygon_Cxy : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_SELECT_RIGHT_LED_Polygon_Dxy : i = i + 1
        _WS02TellTale(i) = New String("telltale defect area ".ToCharArray) : i = i + 1
        _WS02TellTale(i) = _WS02Results.DEFECT_AREA_TELLTALE_SELECT_RIGHT_MIRROR : i = i + 1

        _WS02TellTale(i) = New String("Checked Value Children Lock TellTale ".ToCharArray) : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_CHILDREN_LOCK_LED_saturation : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_CHILDREN_LOCK_LED_intensity : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_CHILDREN_LOCK_LED_WaveLenght : i = i + 1
        _WS02TellTale(i) = New String("Information Value Children Lock TellTale ".ToCharArray) : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_CHILDREN_LOCK_LED_red : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_CHILDREN_LOCK_LED_green : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_CHILDREN_LOCK_LED_blue : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_CHILDREN_LOCK_LED_x : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_CHILDREN_LOCK_LED_y : i = i + 1
        _WS02TellTale(i) = New String("Settings Polygon".ToCharArray) : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Axy : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Bxy : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Cxy : i = i + 1
        _WS02TellTale(i) = _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Dxy : i = i + 1
        _WS02TellTale(i) = New String("telltale defect area ".ToCharArray) : i = i + 1
        _WS02TellTale(i) = _WS02Results.DEFECT_AREA_TELLTALE_CHILDREN_LOCK : i = i + 1

        InitResultsTable(dgvWS02TellTale, i, _WS02TellTale, 730)

    End Sub

    Private Sub InitStationWS02TabsAnalogInput()
        _WS02AnalogInput(0) = New String("Analog Input Variant Coding".ToCharArray)
        _WS02AnalogInput(1) = _WS02Results.ADC_UCAD_VARIANT_1
        _WS02AnalogInput(2) = _WS02Results.ADC_UCAD_VARIANT_2
        _WS02AnalogInput(3) = New String("Analog Input Curseur Position".ToCharArray)
        _WS02AnalogInput(4) = _WS02Results.ADC_CURSEUR_UP_DN
        _WS02AnalogInput(5) = _WS02Results.ADC_CURSEUR_LEFT_RIGHT
        _WS02AnalogInput(6) = New String("Analog Temperature".ToCharArray)
        _WS02AnalogInput(7) = _WS02Results.ADC_Temp
        _WS02AnalogInput(8) = _WS02Results.External_Temp

        InitResultsTable(dgvWS02Analog, 9, _WS02AnalogInput, 730)

    End Sub
    Private Sub InitStationWS02TabsPWMOutput()
        Dim i As Integer
        i = 0
        _WS02PWMOutput_1(i) = New String("UCDA SINGLE SW DIAG".ToCharArray) : i += 1
        _WS02PWMOutput_1(i) = _WS02Results.UCDA_SINGLE_SW_DIAG_VoltageLOW_X1_12 : i += 1
        _WS02PWMOutput_1(i) = _WS02Results.UCDA_SINGLE_SW_DIAG_VoltageHIGH_X1_12 : i += 1
        _WS02PWMOutput_1(i) = _WS02Results.UCDA_SINGLE_SW_DIAG_Frequency_X1_12 : i += 1
        _WS02PWMOutput_1(i) = _WS02Results.UCDA_SINGLE_SW_DIAG_DutyCycle_X1_12 : i += 1
        _WS02PWMOutput_1(i) = _WS02Results.UCDA_SINGLE_SW_DIAG_RiseTime_X1_12 : i += 1
        _WS02PWMOutput_1(i) = _WS02Results.UCDA_SINGLE_SW_DIAG_FallTime_X1_12 : i += 1
        _WS02PWMOutput_1(i) = New String("UCAD SINGLE SW DIAG".ToCharArray) : i += 1
        _WS02PWMOutput_1(i) = _WS02Results.UCDA_SINGLE_SW_DIAG : i += 1
        InitResultsTable(dgvWS02PWM_WLAP, i, _WS02PWMOutput_1, 730)

        i = 0
        _WS02PWMOutput_2(i) = New String("UCDA DOOR LOCK DIAG".ToCharArray) : i += 1
        _WS02PWMOutput_2(i) = _WS02Results.UCDA_DOOR_LOCK_DIAG_VoltageLOW_X1_24 : i += 1
        _WS02PWMOutput_2(i) = _WS02Results.UCDA_DOOR_LOCK_DIAG_VoltageHIGH_X1_24 : i += 1
        _WS02PWMOutput_2(i) = _WS02Results.UCDA_DOOR_LOCK_DIAG_Frequency_X1_24 : i += 1
        _WS02PWMOutput_2(i) = _WS02Results.UCDA_DOOR_LOCK_DIAG_DutyCycle_X1_24 : i += 1
        _WS02PWMOutput_2(i) = _WS02Results.UCDA_DOOR_LOCK_DIAG_RiseTime_X1_24 : i += 1
        _WS02PWMOutput_2(i) = _WS02Results.UCDA_DOOR_LOCK_DIAG_FallTime_X1_24 : i += 1
        _WS02PWMOutput_2(i) = New String("UCAD DOOR LOCK DIAG".ToCharArray) : i += 1
        _WS02PWMOutput_2(i) = _WS02Results.UCDA_DOOR_LOCK_DIAG : i += 1
        InitResultsTable(dgvWS02PWM_DOORLock, i, _WS02PWMOutput_2, 730)


    End Sub
    Private Sub InitStationWS02DO_UP_FrontPassenger()
        Dim i As Integer
        Dim r As Integer
        i = 0
        _WS02DO_UP_FrontPassenger(i) = New String("DO_UP_FrontPassenger".ToCharArray) : i = i + 1
        _WS02DO_UP_FrontPassenger(i) = New String("Enable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_UP_FrontPassenger(i) = _WS02Results.STsgn_Front_Passenger_Commande_UP(r, 0) : i = i + 1
        Next
        _WS02DO_UP_FrontPassenger(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_UP_FrontPassenger(i) = _WS02Results.STsgn_Front_Passenger_Commande_UP(r, 1) : i = i + 1
        Next

        InitResultsTable(dgvWS02FrontPassengerUP, i, _WS02DO_UP_FrontPassenger, 700)

    End Sub
    Private Sub InitStationWS02DO_DN_FrontPassenger()
        Dim i As Integer
        Dim r As Integer
        i = 0
        _WS02DO_DN_FrontPassenger(i) = New String("DO_DN_FrontPassenger".ToCharArray) : i = i + 1
        _WS02DO_DN_FrontPassenger(i) = New String("Enable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_DN_FrontPassenger(i) = _WS02Results.STsgn_Front_Passenger_Commande_Down(r, 0) : i = i + 1
        Next
        _WS02DO_DN_FrontPassenger(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_DN_FrontPassenger(i) = _WS02Results.STsgn_Front_Passenger_Commande_Down(r, 1) : i = i + 1
        Next

        InitResultsTable(dgvWS02FrontPassengerDN, i, _WS02DO_DN_FrontPassenger, 700)

    End Sub

    Private Sub InitStationWS02DO_Switch_Inhibition()
        Dim i As Integer
        Dim r As Integer
        i = 0
        _WS02DO_Switch_Inhibition(i) = New String("DO_Switch_Inhibition".ToCharArray) : i = i + 1
        _WS02DO_Switch_Inhibition(i) = New String("Enable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_Switch_Inhibition(i) = _WS02Results.STsgn_Switches_Inhibition(r, 0) : i = i + 1
        Next
        _WS02DO_Switch_Inhibition(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_Switch_Inhibition(i) = _WS02Results.STsgn_Switches_Inhibition(r, 1) : i = i + 1
        Next

        InitResultsTable(dgvWS02SwitchInhibition, i, _WS02DO_Switch_Inhibition, 700)

    End Sub

    Private Sub InitStationWS02DO_UP_RearRight()
        Dim i As Integer
        Dim r As Integer
        i = 0
        _WS02DO_UP_RearRight(i) = New String("DO_UP_RearRight".ToCharArray) : i = i + 1
        _WS02DO_UP_RearRight(i) = New String("Enable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_UP_RearRight(i) = _WS02Results.STsgn_UP_Rear_Right(r, 0) : i = i + 1
        Next
        _WS02DO_UP_RearRight(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_UP_RearRight(i) = _WS02Results.STsgn_UP_Rear_Right(r, 1) : i = i + 1
        Next

        InitResultsTable(dgvWS02RearRightUP, i, _WS02DO_UP_RearRight, 700)

    End Sub
    Private Sub InitStationWS02DO_DN_RearRight()
        Dim i As Integer
        Dim r As Integer
        i = 0
        _WS02DO_DN_RearRight(i) = New String("DO_DN_RearRight".ToCharArray) : i = i + 1
        _WS02DO_DN_RearRight(i) = New String("Enable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_DN_RearRight(i) = _WS02Results.STsgn_Down_Rear_Right(r, 0) : i = i + 1
        Next
        _WS02DO_DN_RearRight(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_DN_RearRight(i) = _WS02Results.STsgn_Down_Rear_Right(r, 1) : i = i + 1
        Next

        InitResultsTable(dgvWS02RearRightDN, i, _WS02DO_DN_RearRight, 700)

    End Sub

    Private Sub InitStationWS02DO_UP_RearLeft()
        Dim i As Integer
        Dim r As Integer
        i = 0
        _WS02DO_UP_RearLeft(i) = New String("DO_UP_RearLeft".ToCharArray) : i = i + 1
        _WS02DO_UP_RearLeft(i) = New String("Enable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_UP_RearLeft(i) = _WS02Results.STsgn_UP_Rear_Left(r, 0) : i = i + 1
        Next
        _WS02DO_UP_RearLeft(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_UP_RearLeft(i) = _WS02Results.STsgn_UP_Rear_Left(r, 1) : i = i + 1
        Next

        InitResultsTable(dgvWS02RearLeftUP, i, _WS02DO_UP_RearLeft, 700)

    End Sub

    Private Sub InitStationWS02DO_DN_RearLeft()
        Dim i As Integer
        Dim r As Integer
        i = 0
        _WS02DO_DN_RearLeft(i) = New String("DO_DN_RearLeft".ToCharArray) : i = i + 1
        _WS02DO_DN_RearLeft(i) = New String("Enable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_DN_RearLeft(i) = _WS02Results.STsgn_Down_Rear_Left(r, 0) : i = i + 1
        Next
        _WS02DO_DN_RearLeft(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_DN_RearLeft(i) = _WS02Results.STsgn_Down_Rear_Left(r, 1) : i = i + 1
        Next

        InitResultsTable(dgvWS02RearLeftDN, i, _WS02DO_DN_RearLeft, 700)

    End Sub

    Private Sub InitStationWS02DO_CDE_RBT_RTRV_G()
        Dim i As Integer
        Dim r As Integer
        i = 0
        _WS02DO_CDE_RBT_RTRV_G_Fold(i) = New String("CDE_RBT_RTRV_G".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RBT_RTRV_G_Fold(i) = _WS02Results.STsgn_CDE_RBT_RTRV_G(r, 0) : i = i + 1
        Next
        _WS02DO_CDE_RBT_RTRV_G_Fold(i) = _WS02Results.ADC_CDE_RBT_RTRV_G(0) : i = i + 1

        _WS02DO_CDE_RBT_RTRV_G_Fold(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RBT_RTRV_G_Fold(i) = _WS02Results.STsgn_CDE_RBT_RTRV_G(r, 1) : i = i + 1
        Next
        _WS02DO_CDE_RBT_RTRV_G_Fold(i) = _WS02Results.ADC_CDE_RBT_RTRV_G(1) : i = i + 1

        InitResultsTable(dgvWS02RBT_RTRV_G_Fold, i, _WS02DO_CDE_RBT_RTRV_G_Fold, 700)

    End Sub

    Private Sub InitStationWS02DO_CDE_RBT_RTRV_D()
        Dim i As Integer
        Dim r As Integer
        i = 0
        _WS02DO_CDE_RBT_RTRV_D_Fold(i) = New String("CDE_RBT_RTRV_D".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RBT_RTRV_D_Fold(i) = _WS02Results.STsgn_CDE_RBT_RTRV_D(r, 0) : i = i + 1
        Next
        _WS02DO_CDE_RBT_RTRV_D_Fold(i) = _WS02Results.ADC_CDE_RBT_RTRV_D(0) : i = i + 1

        _WS02DO_CDE_RBT_RTRV_D_Fold(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RBT_RTRV_D_Fold(i) = _WS02Results.STsgn_CDE_RBT_RTRV_D(r, 1) : i = i + 1
        Next
        _WS02DO_CDE_RBT_RTRV_D_Fold(i) = _WS02Results.ADC_CDE_RBT_RTRV_D(1) : i = i + 1


        InitResultsTable(dgvWS02RBT_RTRV_D_Fold, i, _WS02DO_CDE_RBT_RTRV_D_Fold, 700)

    End Sub

    Private Sub InitStationWS02DO_CDE_HB_RTRV_G()
        Dim i As Integer
        Dim r As Integer
        i = 0
        _WS02DO_CDE_RTRV_HB_G(i) = New String("DO_CDE_H/B_RTRV_G".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RTRV_HB_G(i) = _WS02Results.STsgn_CDE_HB_RTRV_G(r, 0) : i = i + 1
        Next
        
        _WS02DO_CDE_RTRV_HB_G(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RTRV_HB_G(i) = _WS02Results.STsgn_CDE_HB_RTRV_G(r, 1) : i = i + 1
        Next
        
        _WS02DO_CDE_RTRV_HB_G(i) = New String("SGN_COMMUN_MOT_RTRV_G".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RTRV_HB_G(i) = _WS02Results.STsgn_SGN_COMMUN_MOT_RTRV_G(r, 0) : i = i + 1
        Next
        _WS02DO_CDE_RTRV_HB_G(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RTRV_HB_G(i) = _WS02Results.STsgn_SGN_COMMUN_MOT_RTRV_G(r, 1) : i = i + 1
        Next



        InitResultsTable(dgvWS02RTRV_HB_G, i, _WS02DO_CDE_RTRV_HB_G, 700)

    End Sub

    Private Sub InitStationWS02DO_CDE_DG_RTRV_G()
        Dim i As Integer
        Dim r As Integer
        i = 0
        _WS02DO_CDE_RTRV_DG_G(i) = New String("DO_CDE_D/G_RTRV_G".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RTRV_DG_G(i) = _WS02Results.STsgn_CDE_DG_RTRV_G(r, 0) : i = i + 1
        Next
        _WS02DO_CDE_RTRV_DG_G(i) = _WS02Results.ADC_CDE_DG_RTRV_G(0) : i = i + 1

        _WS02DO_CDE_RTRV_DG_G(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RTRV_DG_G(i) = _WS02Results.STsgn_CDE_DG_RTRV_G(r, 1) : i = i + 1
        Next
        _WS02DO_CDE_RTRV_DG_G(i) = _WS02Results.ADC_CDE_DG_RTRV_G(1) : i = i + 1

        InitResultsTable(dgvWS02RTRV_DG_G, i, _WS02DO_CDE_RTRV_DG_G, 700)

    End Sub

    Private Sub InitStationWS02DO_CDE_HB_RTRV_D()
        Dim i As Integer
        Dim r As Integer
        i = 0

        _WS02DO_CDE_RTRV_HB_D(i) = New String("SGN_COMMUN_MOT_RTRV_D".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RTRV_HB_D(i) = _WS02Results.STsgn_SGN_COMMUN_MOT_RTRV_D(r, 0) : i = i + 1
        Next
        _WS02DO_CDE_RTRV_HB_D(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RTRV_HB_D(i) = _WS02Results.STsgn_SGN_COMMUN_MOT_RTRV_D(r, 1) : i = i + 1
        Next

        InitResultsTable(dgvWS02RTRV_HB_D, i, _WS02DO_CDE_RTRV_HB_D, 700)

    End Sub

    Private Sub InitStationWS02DO_CDE_DG_RTRV_D()
        Dim i As Integer
        Dim r As Integer
        i = 0
        _WS02DO_CDE_RTRV_DG_D(i) = New String("DO_CDE_D/G_RTRV_D".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RTRV_DG_D(i) = _WS02Results.STsgn_CDE_DG_RTRV_D(r, 0) : i = i + 1
        Next
        _WS02DO_CDE_RTRV_DG_D(i) = _WS02Results.ADC_CDE_DG_RTRV_D(0) : i = i + 1

        _WS02DO_CDE_RTRV_DG_D(i) = New String("Disable".ToCharArray) : i = i + 1
        For r = 0 To 20
            _WS02DO_CDE_RTRV_DG_D(i) = _WS02Results.STsgn_CDE_DG_RTRV_D(r, 1) : i = i + 1
        Next
        _WS02DO_CDE_RTRV_DG_D(i) = _WS02Results.ADC_CDE_DG_RTRV_D(1) : i = i + 1

        InitResultsTable(dgvWS02RTRV_DG_D, i, _WS02DO_CDE_RTRV_DG_D, 700)

    End Sub

    Private Sub InitStationWS02TabsMMSTraceability()
        Dim i As Integer
        i = 0
        _WS02MMSTraceability(i) = New String("MMS Traceability".ToCharArray) : i = i + 1
        'Read Valeo Traceability MMS
        _WS02MMSTraceability(i) = _WS02Results.MMS_Read_Final_Product_Reference : i = i + 1
        _WS02MMSTraceability(i) = _WS02Results.MMS_Read_Final_Product_Index : i = i + 1
        _WS02MMSTraceability(i) = _WS02Results.MMS_Read_Valeo_Final_Product_Plant : i = i + 1
        _WS02MMSTraceability(i) = _WS02Results.MMS_Read_Valeo_Final_Product_Line : i = i + 1
        _WS02MMSTraceability(i) = _WS02Results.MMS_Read_Valeo_Final_Product_Manufacturing_Date : i = i + 1
        _WS02MMSTraceability(i) = _WS02Results.MMS_Read_Valeo_Serial_Number : i = i + 1
        _WS02MMSTraceability(i) = _WS02Results.MMS_Read_Deviation_Number : i = i + 1
        _WS02MMSTraceability(i) = _WS02Results.MMS_Read_Test_Byte : i = i + 1

        InitResultsTable(dgvWS02MMS, i, _WS02MMSTraceability, 730)

    End Sub

    Private Sub InitStationWS02TabsEMSTraceability()
        Dim i As Integer
        i = 0
        _WS02EMSTraceability(i) = New String("EMS Traceability".ToCharArray) : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.PCBA_Number_Reference : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.PCBA_Number_Index : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.PCBA_Plant_Line : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.PCBA_ManufacturingDate : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.PCBA_SerialNumber : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.PCBA_DeviationNumber : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.LED_BIN_PT_White_RSA : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.LED_BIN_PT_RED : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.LED_BIN_PT_YELLOW : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.LED_BIN_PT_WHITE_NISSAN : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.EMS_Test_Byte : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.Major_SoftwareVersion : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.Minor_SoftwareVersion : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.Major_NVMversion : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.Minor_NVMversion : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.SW_checksum : i = i + 1
        _WS02EMSTraceability(i) = New String("MMS Write Config".ToCharArray) : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.SW_Coding : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.HW_Coding : i = i + 1
        _WS02EMSTraceability(i) = _WS02Results.Backlight_Coding : i = i + 1

        InitResultsTable(dgvWS02EMS, i, _WS02EMSTraceability, 800)


    End Sub

    Private Sub ShowGraphPWM(ByRef chart As DataVisualization.Charting.Chart,
                          ByVal PWMIndex As Integer)
        Dim i As Integer
        ' Reference to the chart
        With chart
            ' Suspend the layout
            .SuspendLayout()
            ' Clear the series
            .Series.Clear()
            .ChartAreas(0).AxisX.Minimum = 0
            .ChartAreas(0).AxisX.Maximum = 10
            .ChartAreas(0).AxisX.Interval = 0.1
            .ChartAreas(0).AxisY.Minimum = 0
            .ChartAreas(0).AxisY.Maximum = 15
            .ChartAreas(0).AxisY.Interval = 1

            For i = 0 To 1
                ' Add the series
                .Series.Add(CStr(i))
                .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
                .Series(i).BorderWidth = 3
                If i <> 0 Then
                    For j = 0 To mWS02Main.chUCDA_Sample(PWMIndex) - 1
                        .Series(i).Points.AddXY(j * 0.02, mWS02Main.chUCDA_PWM(PWMIndex, j))
                    Next
                End If
            Next i
            ' Resume the layout
            .ResumeLayout()
        End With
    End Sub

    '*********************************************************
    ' WS03
    '*********************************************************
    Private Sub InitStationWS03Tabs()
        InitStationWS03TabsGeneralInformation()
        InitStationWS03TabsPowerSypply()
        InitStationWS03TabsReadValeoSerialNumber()
        InitStationWS03TabsFrontLeftUP()
        InitStationWS03TabsFrontLeftDN()
        InitStationWS03TabsFrontRightUP()
        InitStationWS03TabsFrontRightDN()
        InitStationWS03TabsPushChildrenLock()
        InitStationWS03TabsFinalState()

    End Sub

    Private Sub InitStationWS03TabsGeneralInformation()
        ' General information
        _WS03GeneralInformation(0) = New String("General information".ToCharArray)
        _WS03GeneralInformation(1) = _WS03Results.TestDate
        _WS03GeneralInformation(2) = _WS03Results.TestTime
        _WS03GeneralInformation(3) = _WS03Results.RecipeName
        _WS03GeneralInformation(4) = _WS03Results.RecipeModifyDate
        _WS03GeneralInformation(5) = _WS03Results.RecipeModifyTime
        _WS03GeneralInformation(6) = _WS03Results.PartUniqueNumber
        _WS03GeneralInformation(7) = _WS03Results.PartTypeNumber

        InitResultsTable(dgvWS03GeneralInformation, 8, _WS03GeneralInformation, 730)
    End Sub

    Private Sub InitStationWS03TabsPowerSypply()

        ' Power Supply
        _WS03PowerSypply(0) = New String("Signals Values".ToCharArray)
        _WS03PowerSypply(1) = _WS03Results.Power_supply_voltage
        _WS03PowerSypply(2) = _WS03Results.Power_supply_Normal_Current

        InitResultsTable(dgvWS03PowerUp, 3, _WS03PowerSypply, 730)

    End Sub

    Private Sub InitStationWS03TabsReadValeoSerialNumber()
        _WS03ValeoSerialNumber(0) = New String("Traceability Valeo Serial Number ".ToCharArray)
        _WS03ValeoSerialNumber(1) = _WS03Results.Valeo_Serial_Number
        _WS03ValeoSerialNumber(2) = _WS03Results.MMS_Test_Byte_Before
        _WS03ValeoSerialNumber(3) = New String("MMS Test Byte".ToCharArray)
        _WS03ValeoSerialNumber(4) = _WS03Results.MMS_Test_Byte_After

        InitResultsTable(dgvWS03SerialNumber, 5, _WS03ValeoSerialNumber, 730)
    End Sub

    Private Sub InitStationWS03TabsFrontLeftUP()
        Dim i As Integer
        Dim tempindex As Integer = 0
        Dim pushpull As ePush_Pull = ePush_Pull.Pull
        Dim position As mWS03Main.eWindows = mWS03Main.eWindows.FrontLeft

        'init tab
        _WS03FrontLeftPull(tempindex) = New String("Early Communtation values".ToCharArray) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_XCe1(pushpull, position) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_XCe2(pushpull, position) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_Xe(pushpull, position) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_DiffS2Ce1(pushpull, position) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_DiffS5Ce2(pushpull, position) : tempindex += 1

        _WS03FrontLeftPull(tempindex) = New String("Strenght values".ToCharArray) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_Fs1_F1(pushpull, position) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_Xs1(pushpull, position) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_dFs1_Haptic_1(pushpull, position) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_dXs1(pushpull, position) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_Fs2_F2(pushpull, position) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_Xs2(pushpull, position) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_dFs2_Haptic_2(pushpull, position) : tempindex += 1
        _WS03FrontLeftPull(tempindex) = _WS03Results.WL_dXs2(pushpull, position) : tempindex += 1

        _WS03FrontLeftPull(tempindex) = New String("Analog values before commutation".ToCharArray) : tempindex += 1
        tempindex = 21
        _WS03FrontLeftPull(tempindex) = New String("Analog values after commutation Ce1".ToCharArray) : tempindex += 1
        tempindex = 27
        _WS03FrontLeftPull(tempindex) = New String("Analog values after commutation Ce2".ToCharArray) : tempindex += 1
        For i = 0 To 2
            tempindex = 16
            _WS03FrontLeftPull(tempindex + (6 * i)) = _WS03Results.WL_Front_Left_Pull_Manual(i, pushpull, position) : tempindex += 1
            _WS03FrontLeftPull(tempindex + (6 * i)) = _WS03Results.WL_Front_Left_Pull_Automatic(i, pushpull, position) : tempindex += 1
            _WS03FrontLeftPull(tempindex + (6 * i)) = _WS03Results.WL_Front_Left_Push_Manual(i, pushpull, position) : tempindex += 1
            _WS03FrontLeftPull(tempindex + (6 * i)) = _WS03Results.WL_Front_Left_Push_Automatic(i, pushpull, position) : tempindex += 1
            _WS03FrontLeftPull(tempindex + (6 * i)) = _WS03Results.WL_Front_Left_Jama_Down(i, pushpull, position) : tempindex += 1
        Next i

        InitResultsTable(dgvWS03FrontLeftUP, 33, _WS03FrontLeftPull, 730)

    End Sub

    Private Sub InitStationWS03TabsFrontLeftDN()
        Dim i As Integer
        Dim tempindex As Integer = 0
        Dim pushpull As ePush_Pull = ePush_Pull.Push
        Dim position As mWS03Main.eWindows = mWS03Main.eWindows.FrontLeft

        'init tab
        _WS03FrontLeftPush(tempindex) = New String("Early Communtation values".ToCharArray) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_XCe1(pushpull, position) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_XCe2(pushpull, position) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_Xe(pushpull, position) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_DiffS2Ce1(pushpull, position) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_DiffS5Ce2(pushpull, position) : tempindex += 1

        _WS03FrontLeftPush(tempindex) = New String("Strenght values".ToCharArray) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_Fs1_F1(pushpull, position) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_Xs1(pushpull, position) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_dFs1_Haptic_1(pushpull, position) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_dXs1(pushpull, position) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_Fs2_F2(pushpull, position) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_Xs2(pushpull, position) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_dFs2_Haptic_2(pushpull, position) : tempindex += 1
        _WS03FrontLeftPush(tempindex) = _WS03Results.WL_dXs2(pushpull, position) : tempindex += 1

        _WS03FrontLeftPush(tempindex) = New String("Analog values before commutation".ToCharArray) : tempindex += 1
        tempindex = 21
        _WS03FrontLeftPush(tempindex) = New String("Analog values after commutation Ce1".ToCharArray)
        tempindex = 27
        _WS03FrontLeftPush(tempindex) = New String("Analog values after commutation Ce2".ToCharArray)
        For i = 0 To 2
            tempindex = 16
            _WS03FrontLeftPush(tempindex + (6 * i)) = _WS03Results.WL_Front_Left_Pull_Manual(i, pushpull, position) : tempindex += 1
            _WS03FrontLeftPush(tempindex + (6 * i)) = _WS03Results.WL_Front_Left_Pull_Automatic(i, pushpull, position) : tempindex += 1
            _WS03FrontLeftPush(tempindex + (6 * i)) = _WS03Results.WL_Front_Left_Push_Manual(i, pushpull, position) : tempindex += 1
            _WS03FrontLeftPush(tempindex + (6 * i)) = _WS03Results.WL_Front_Left_Push_Automatic(i, pushpull, position) : tempindex += 1
            _WS03FrontLeftPush(tempindex + (6 * i)) = _WS03Results.WL_Front_Left_Jama_Down(i, pushpull, position) : tempindex += 1
        Next i

        InitResultsTable(dgvWS03FrontLeftDN, 33, _WS03FrontLeftPush, 730)

    End Sub

    Private Sub InitStationWS03TabsFrontRightUP()
        Dim i As Integer
        Dim tempindex As Integer = 0
        Dim pushpull As ePush_Pull = ePush_Pull.Pull
        Dim position As mWS03Main.eWindows = mWS03Main.eWindows.FrontRight

        'init tab
        _WS03FrontRightPull(tempindex) = New String("Early Communtation values".ToCharArray) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_XCe1(pushpull, position) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_XCe2(pushpull, position) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_Xe(pushpull, position) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_DiffS2Ce1(pushpull, position) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_DiffS5Ce2(pushpull, position) : tempindex += 1

        _WS03FrontRightPull(tempindex) = New String("Strenght values".ToCharArray) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_Fs1_F1(pushpull, position) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_Xs1(pushpull, position) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_dFs1_Haptic_1(pushpull, position) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_dXs1(pushpull, position) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_Fs2_F2(pushpull, position) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_Xs2(pushpull, position) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_dFs2_Haptic_2(pushpull, position) : tempindex += 1
        _WS03FrontRightPull(tempindex) = _WS03Results.WL_dXs2(pushpull, position) : tempindex += 1

        _WS03FrontRightPull(tempindex) = New String("Analog values before commutation".ToCharArray) : tempindex += 1
        tempindex = 21
        _WS03FrontRightPull(tempindex) = New String("Analog values after commutation Ce1".ToCharArray)
        tempindex = 27
        _WS03FrontRightPull(tempindex) = New String("Analog values after commutation Ce2".ToCharArray)
        For i = 0 To 2
            tempindex = 16
            _WS03FrontRightPull(tempindex + (6 * i)) = _WS03Results.WL_Front_Right_Pull_Manual(i, pushpull, position) : tempindex += 1
            _WS03FrontRightPull(tempindex + (6 * i)) = _WS03Results.WL_Front_Right_Pull_Automatic(i, pushpull, position) : tempindex += 1
            _WS03FrontRightPull(tempindex + (6 * i)) = _WS03Results.WL_Front_Right_Push_Manual(i, pushpull, position) : tempindex += 1
            _WS03FrontRightPull(tempindex + (6 * i)) = _WS03Results.WL_Front_Right_Push_Automatic(i, pushpull, position) : tempindex += 1
            _WS03FrontRightPull(tempindex + (6 * i)) = _WS03Results.WL_Front_Right_Jama_Down(i, pushpull, position) : tempindex += 1
        Next i

        InitResultsTable(dgvWS03FrontRightUP, 33, _WS03FrontRightPull, 730)

    End Sub

    Private Sub InitStationWS03TabsFrontRightDN()
        Dim i As Integer
        Dim tempindex As Integer = 0
        Dim pushpull As ePush_Pull = ePush_Pull.Push
        Dim position As mWS03Main.eWindows = mWS03Main.eWindows.FrontRight

        'init tab
        _WS03FrontRightPush(tempindex) = New String("Early Communtation values".ToCharArray) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_XCe1(pushpull, position) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_XCe2(pushpull, position) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_Xe(pushpull, position) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_DiffS2Ce1(pushpull, position) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_DiffS5Ce2(pushpull, position) : tempindex += 1

        _WS03FrontRightPush(tempindex) = New String("Strenght values".ToCharArray) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_Fs1_F1(pushpull, position) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_Xs1(pushpull, position) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_dFs1_Haptic_1(pushpull, position) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_dXs1(pushpull, position) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_Fs2_F2(pushpull, position) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_Xs2(pushpull, position) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_dFs2_Haptic_2(pushpull, position) : tempindex += 1
        _WS03FrontRightPush(tempindex) = _WS03Results.WL_dXs2(pushpull, position) : tempindex += 1

        _WS03FrontRightPush(tempindex) = New String("Analog values before commutation".ToCharArray) : tempindex += 1
        tempindex = 21
        _WS03FrontRightPush(tempindex) = New String("Analog values after commutation Ce1".ToCharArray)
        tempindex = 27
        _WS03FrontRightPush(tempindex) = New String("Analog values after commutation Ce2".ToCharArray)
        For i = 0 To 2
            tempindex = 16
            _WS03FrontRightPush(tempindex + (6 * i)) = _WS03Results.WL_Front_Right_Pull_Manual(i, pushpull, position) : tempindex += 1
            _WS03FrontRightPush(tempindex + (6 * i)) = _WS03Results.WL_Front_Right_Pull_Automatic(i, pushpull, position) : tempindex += 1
            _WS03FrontRightPush(tempindex + (6 * i)) = _WS03Results.WL_Front_Right_Push_Manual(i, pushpull, position) : tempindex += 1
            _WS03FrontRightPush(tempindex + (6 * i)) = _WS03Results.WL_Front_Right_Push_Automatic(i, pushpull, position) : tempindex += 1
            _WS03FrontRightPush(tempindex + (6 * i)) = _WS03Results.WL_Front_Right_Jama_Down(i, pushpull, position) : tempindex += 1
        Next i

        InitResultsTable(dgvWS03FrontRightDN, 33, _WS03FrontRightPush, 730)

    End Sub

    Private Sub InitStationWS03TabsPushChildrenLock()
        Dim i As Integer

        'init tab
        _WS03PushChildrenLock(0) = New String("Early Communtation values".ToCharArray)
        _WS03PushChildrenLock(1) = _WS03Results.CL_XCe1
        _WS03PushChildrenLock(2) = _WS03Results.CL_Xe
        _WS03PushChildrenLock(3) = _WS03Results.CL_DiffS2Ce1

        _WS03PushChildrenLock(4) = New String("Strenght values".ToCharArray)
        _WS03PushChildrenLock(5) = _WS03Results.CL_Fs1_F1
        _WS03PushChildrenLock(6) = _WS03Results.CL_Xs1
        _WS03PushChildrenLock(7) = _WS03Results.CL_dFs1_Haptic_1
        _WS03PushChildrenLock(8) = _WS03Results.CL_dXs1

        _WS03PushChildrenLock(9) = New String("Analog values before commutation".ToCharArray)
        _WS03PushChildrenLock(10) = _WS03Results.Push_CL_Electric(0)
        _WS03PushChildrenLock(11) = New String("Analog values after commutation Ce1".ToCharArray)
        _WS03PushChildrenLock(12) = _WS03Results.Push_CL_Electric(1)
        _WS03PushChildrenLock(13) = New String("Analog values Back to Initial Position".ToCharArray)
        _WS03PushChildrenLock(14) = _WS03Results.BakcInitialSate

        InitResultsTable(dgvWS03ChildrenLock, 15, _WS03PushChildrenLock, 730)

    End Sub

    Private Sub InitStationWS03TabsFinalState()
        Dim i As Integer
        'init tab
        _WS03FinalState(0) = New String("Electric values".ToCharArray)
        For i = 0 To 27
            _WS03FinalState(1 + i) = _WS03Results.FinalState(i)
        Next

        InitResultsTable(dgvWS03FinalState, 29, _WS03FinalState, 730)

    End Sub

    '*********************************************************
    ' WS04
    '*********************************************************
    Private Sub InitStationWS04Tabs()
        InitStationWS04TabsGeneralInformation()
        InitStationWS04TabsPowerSypply()
        InitStationWS04TabsReadValeoSerialNumber()
        InitStationWS04TabsRearLeftUP()
        InitStationWS04TabsRearLeftDN()
        InitStationWS04TabsRearRightUP()
        InitStationWS04TabsRearRightDN()
        InitStationWS04TabsPushMirrorFolding()
        InitStationWS04TabsFinalState()

    End Sub

    Private Sub InitStationWS04TabsGeneralInformation()
        ' General information
        _WS04GeneralInformation(0) = New String("General information".ToCharArray)
        _WS04GeneralInformation(1) = _WS04Results.TestDate
        _WS04GeneralInformation(2) = _WS04Results.TestTime
        _WS04GeneralInformation(3) = _WS04Results.RecipeName
        _WS04GeneralInformation(4) = _WS04Results.RecipeModifyDate
        _WS04GeneralInformation(5) = _WS04Results.RecipeModifyTime
        _WS04GeneralInformation(6) = _WS04Results.PartUniqueNumber
        _WS04GeneralInformation(7) = _WS04Results.PartTypeNumber

        InitResultsTable(dgvWS04GeneralInformation, 8, _WS04GeneralInformation, 730)
    End Sub

    Private Sub InitStationWS04TabsPowerSypply()

        ' Power Supply
        _WS04PowerSypply(0) = New String("Signals Values".ToCharArray)
        _WS04PowerSypply(1) = _WS04Results.Power_supply_voltage
        _WS04PowerSypply(2) = _WS04Results.Power_supply_Normal_Current

        InitResultsTable(dgvWS04PowerUp, 3, _WS04PowerSypply, 730)

    End Sub

    Private Sub InitStationWS04TabsReadValeoSerialNumber()
        _WS04ValeoSerialNumber(0) = New String("Traceability Valeo Serial Number ".ToCharArray)
        _WS04ValeoSerialNumber(1) = _WS04Results.Valeo_Serial_Number
        _WS04ValeoSerialNumber(2) = _WS04Results.MMS_Test_Byte_Before
        _WS04ValeoSerialNumber(3) = New String("MMS Test Byte".ToCharArray)
        _WS04ValeoSerialNumber(4) = _WS04Results.MMS_Test_Byte_After

        InitResultsTable(dgvWS04SerialNumber, 5, _WS04ValeoSerialNumber, 730)
    End Sub

    Private Sub InitStationws04TabsRearLeftUP()
        Dim i As Integer
        'init tab
        _WS04RearLeftPull(0) = New String("Early Communtation values".ToCharArray)
        _WS04RearLeftPull(1) = _WS04Results.WL_XCe1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPull(2) = _WS04Results.WL_XCe2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPull(3) = _WS04Results.WL_Xe(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPull(4) = _WS04Results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPull(5) = _WS04Results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)

        _WS04RearLeftPull(6) = New String("Strenght values".ToCharArray)
        _WS04RearLeftPull(7) = _WS04Results.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPull(8) = _WS04Results.WL_Xs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPull(9) = _WS04Results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPull(10) = _WS04Results.WL_dXs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPull(11) = _WS04Results.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPull(12) = _WS04Results.WL_Xs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPull(13) = _WS04Results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPull(14) = _WS04Results.WL_dXs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)

        _WS04RearLeftPull(15) = New String("Analog values before commutation".ToCharArray)
        _WS04RearLeftPull(20) = New String("Analog values after commutation Ce1".ToCharArray)
        _WS04RearLeftPull(25) = New String("Analog values after commutation Ce2".ToCharArray)
        For i = 0 To 2
            _WS04RearLeftPull(16 + (5 * i)) = _WS04Results.WL_Rear_Left_Pull_Manual(i, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
            _WS04RearLeftPull(17 + (5 * i)) = _WS04Results.WL_Rear_Left_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
            _WS04RearLeftPull(18 + (5 * i)) = _WS04Results.WL_Rear_Left_Push_Manual(i, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
            _WS04RearLeftPull(19 + (5 * i)) = _WS04Results.WL_Rear_Left_Push_Automatic(i, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearLeft)
        Next i

        InitResultsTable(dgvWS04RearLeftUP, 30, _WS04RearLeftPull, 730)

    End Sub

    Private Sub InitStationws04TabsRearLeftDN()
        Dim i As Integer
        'init tab
        _WS04RearLeftPush(0) = New String("Early Communtation values".ToCharArray)
        _WS04RearLeftPush(1) = _WS04Results.WL_XCe1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPush(2) = _WS04Results.WL_XCe2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPush(3) = _WS04Results.WL_Xe(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPush(4) = _WS04Results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPush(5) = _WS04Results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)

        _WS04RearLeftPush(6) = New String("Strenght values".ToCharArray)
        _WS04RearLeftPush(7) = _WS04Results.WL_Fs1_F1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPush(8) = _WS04Results.WL_Xs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPush(9) = _WS04Results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPush(10) = _WS04Results.WL_dXs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPush(11) = _WS04Results.WL_Fs2_F2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPush(12) = _WS04Results.WL_Xs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPush(13) = _WS04Results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
        _WS04RearLeftPush(14) = _WS04Results.WL_dXs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)

        _WS04RearLeftPush(15) = New String("Analog values before commutation".ToCharArray)
        _WS04RearLeftPush(20) = New String("Analog values after commutation Ce1".ToCharArray)
        _WS04RearLeftPush(25) = New String("Analog values after commutation Ce2".ToCharArray)
        For i = 0 To 2
            _WS04RearLeftPush(16 + (5 * i)) = _WS04Results.WL_Rear_Left_Pull_Manual(i, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
            _WS04RearLeftPush(17 + (5 * i)) = _WS04Results.WL_Rear_Left_Pull_Automatic(i, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
            _WS04RearLeftPush(18 + (5 * i)) = _WS04Results.WL_Rear_Left_Push_Manual(i, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
            _WS04RearLeftPush(19 + (5 * i)) = _WS04Results.WL_Rear_Left_Push_Automatic(i, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearLeft)
        Next i

        InitResultsTable(dgvWS04RearLeftDN, 30, _WS04RearLeftPush, 730)

    End Sub

    Private Sub InitStationws04TabsRearRightUP()
        Dim i As Integer
        'init tab
        _WS04RearRightPull(0) = New String("Early Communtation values".ToCharArray)
        _WS04RearRightPull(1) = _WS04Results.WL_XCe1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
        _WS04RearRightPull(2) = _WS04Results.WL_XCe2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
        _WS04RearRightPull(3) = _WS04Results.WL_Xe(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
        _WS04RearRightPull(4) = _WS04Results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
        _WS04RearRightPull(5) = _WS04Results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)

        _WS04RearRightPull(6) = New String("Strenght values".ToCharArray)
        _WS04RearRightPull(7) = _WS04Results.WL_Fs1_F1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
        _WS04RearRightPull(8) = _WS04Results.WL_Xs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
        _WS04RearRightPull(9) = _WS04Results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
        _WS04RearRightPull(10) = _WS04Results.WL_dXs1(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
        _WS04RearRightPull(11) = _WS04Results.WL_Fs2_F2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
        _WS04RearRightPull(12) = _WS04Results.WL_Xs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
        _WS04RearRightPull(13) = _WS04Results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
        _WS04RearRightPull(14) = _WS04Results.WL_dXs2(mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)

        _WS04RearRightPull(15) = New String("Analog values before commutation".ToCharArray)
        _WS04RearRightPull(20) = New String("Analog values after commutation Ce1".ToCharArray)
        _WS04RearRightPull(25) = New String("Analog values after commutation Ce2".ToCharArray)
        For i = 0 To 2
            _WS04RearRightPull(16 + (5 * i)) = _WS04Results.WL_Rear_Right_Pull_Manual(i, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
            _WS04RearRightPull(17 + (5 * i)) = _WS04Results.WL_Rear_Right_Pull_Automatic(i, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
            _WS04RearRightPull(18 + (5 * i)) = _WS04Results.WL_Rear_Right_Push_Manual(i, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
            _WS04RearRightPull(19 + (5 * i)) = _WS04Results.WL_Rear_Right_Push_Automatic(i, mGlobal.ePush_Pull.Pull, mWS04Main.eWindows.RearRight)
        Next i

        InitResultsTable(dgvWS04RearRightUP, 30, _WS04RearRightPull, 730)

    End Sub

    Private Sub InitStationws04TabsRearRightDN()
        Dim i As Integer
        'init tab
        _WS04RearRightPush(0) = New String("Early Communtation values".ToCharArray)
        _WS04RearRightPush(1) = _WS04Results.WL_XCe1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
        _WS04RearRightPush(2) = _WS04Results.WL_XCe2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
        _WS04RearRightPush(3) = _WS04Results.WL_Xe(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
        _WS04RearRightPush(4) = _WS04Results.WL_DiffS2Ce1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
        _WS04RearRightPush(5) = _WS04Results.WL_DiffS5Ce2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)

        _WS04RearRightPush(6) = New String("Strenght values".ToCharArray)
        _WS04RearRightPush(7) = _WS04Results.WL_Fs1_F1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
        _WS04RearRightPush(8) = _WS04Results.WL_Xs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
        _WS04RearRightPush(9) = _WS04Results.WL_dFs1_Haptic_1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
        _WS04RearRightPush(10) = _WS04Results.WL_dXs1(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
        _WS04RearRightPush(11) = _WS04Results.WL_Fs2_F2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
        _WS04RearRightPush(12) = _WS04Results.WL_Xs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
        _WS04RearRightPush(13) = _WS04Results.WL_dFs2_Haptic_2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
        _WS04RearRightPush(14) = _WS04Results.WL_dXs2(mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)

        _WS04RearRightPush(15) = New String("Analog values before commutation".ToCharArray)
        _WS04RearRightPush(20) = New String("Analog values after commutation Ce1".ToCharArray)
        _WS04RearRightPush(25) = New String("Analog values after commutation Ce2".ToCharArray)
        For i = 0 To 2
            _WS04RearRightPush(16 + (5 * i)) = _WS04Results.WL_Rear_Right_Pull_Manual(i, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
            _WS04RearRightPush(17 + (5 * i)) = _WS04Results.WL_Rear_Right_Pull_Automatic(i, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
            _WS04RearRightPush(18 + (5 * i)) = _WS04Results.WL_Rear_Right_Push_Manual(i, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
            _WS04RearRightPush(19 + (5 * i)) = _WS04Results.WL_Rear_Right_Push_Automatic(i, mGlobal.ePush_Pull.Push, mWS04Main.eWindows.RearRight)
        Next i

        InitResultsTable(dgvWS04RearRightDN, 30, _WS04RearRightPush, 730)

    End Sub


    Private Sub InitStationWS04TabsPushMirrorFolding()
        Dim i As Integer
        'init tab
        _WS04PushMirrorFolding(0) = New String("Early Communtation values".ToCharArray)
        _WS04PushMirrorFolding(1) = _WS04Results.MF_XCe1
        _WS04PushMirrorFolding(2) = _WS04Results.MF_Xe
        _WS04PushMirrorFolding(3) = _WS04Results.MF_DiffS2Ce1

        _WS04PushMirrorFolding(4) = New String("Strenght values".ToCharArray)
        _WS04PushMirrorFolding(5) = _WS04Results.MF_Fs1_F1
        _WS04PushMirrorFolding(6) = _WS04Results.MF_Xs1
        _WS04PushMirrorFolding(7) = _WS04Results.MF_dFs1_Haptic_1
        _WS04PushMirrorFolding(8) = _WS04Results.MF_dXs1

        _WS04PushMirrorFolding(9) = New String("Analog values before commutation".ToCharArray)
        _WS04PushMirrorFolding(10) = _WS04Results.Push_MF_Electric(0)
        _WS04PushMirrorFolding(11) = New String("Analog values after commutation Ce1".ToCharArray)
        _WS04PushMirrorFolding(12) = _WS04Results.Push_MF_Electric(1)
        _WS04PushMirrorFolding(13) = New String("Analog values Back to Initial Position".ToCharArray)
        _WS04PushMirrorFolding(14) = _WS04Results.BakcInitialSate


        InitResultsTable(dgvWS04MirrorFolding, 15, _WS04PushMirrorFolding, 730)

    End Sub

    Private Sub InitStationWS04TabsFinalState()
        Dim i As Integer
        'init tab
        _WS04FinalState(0) = New String("Electric values".ToCharArray)
        For i = 0 To 27
            _WS04FinalState(1 + i) = _WS04Results.FinalState(i)
        Next

        InitResultsTable(dgvWS04FinalState, 29, _WS04FinalState, 730)

    End Sub

    '*********************************************************
    ' WS05
    '*********************************************************
    Private Sub InitStationWS05Tabs()
        InitStationWS05TabsGeneralInformation()
        InitStationWS05TabsPowerSypply()
        InitStationWS05TabsReadValeoSerialNumber()
        InitStationWS05TabsMirrorUP()
        InitStationWS05TabsMirrorDN()
        InitStationWS05TabsMirrorMR()
        InitStationWS05TabsMirrorML()
        InitStationWS05TabsMirrorSR()
        InitStationWS05TabsMirrorSL()
        InitStationWS05TabsFinalState()

    End Sub

    Private Sub InitStationWS05TabsGeneralInformation()
        ' General information
        _WS05GeneralInformation(0) = New String("General information".ToCharArray)
        _WS05GeneralInformation(1) = _WS05Results.TestDate
        _WS05GeneralInformation(2) = _WS05Results.TestTime
        _WS05GeneralInformation(3) = _WS05Results.RecipeName
        _WS05GeneralInformation(4) = _WS05Results.RecipeModifyDate
        _WS05GeneralInformation(5) = _WS05Results.RecipeModifyTime
        _WS05GeneralInformation(6) = _WS05Results.PartUniqueNumber
        _WS05GeneralInformation(7) = _WS05Results.PartTypeNumber

        InitResultsTable(dgvWS05GeneralInformation, 8, _WS05GeneralInformation, 730)
    End Sub

    Private Sub InitStationWS05TabsPowerSypply()

        ' Power Supply
        _WS05PowerSypply(0) = New String("Signals Values".ToCharArray)
        _WS05PowerSypply(1) = _WS05Results.Power_supply_voltage
        _WS05PowerSypply(2) = _WS05Results.Power_supply_Normal_Current

        InitResultsTable(dgvWS05PowerUp, 3, _WS05PowerSypply, 730)

    End Sub

    Private Sub InitStationWS05TabsReadValeoSerialNumber()
        _WS05ValeoSerialNumber(0) = New String("Traceability Valeo Serial Number ".ToCharArray)
        _WS05ValeoSerialNumber(1) = _WS05Results.Valeo_Serial_Number
        _WS05ValeoSerialNumber(2) = _WS05Results.MMS_Test_Byte_Before
        _WS05ValeoSerialNumber(3) = New String("MMS Test Byte".ToCharArray)
        _WS05ValeoSerialNumber(4) = _WS05Results.MMS_Test_Byte_After

        InitResultsTable(dgvWS05SerialNumber, 5, _WS05ValeoSerialNumber, 730)
    End Sub

    Private Sub InitStationWS05TabsMirrorUP()
        Dim i As Integer
        'init tab
        _WS05MirrorUP(0) = New String("Early Communtation values".ToCharArray)
        _WS05MirrorUP(1) = _WS05Results.Mirror_XCe1(mWS05Main.eMirrorPush.MirrorUP)
        _WS05MirrorUP(2) = _WS05Results.Mirror_Xe(mWS05Main.eMirrorPush.MirrorUP)
        _WS05MirrorUP(3) = _WS05Results.Mirror_DiffS2Ce1(mWS05Main.eMirrorPush.MirrorUP)

        _WS05MirrorUP(4) = New String("Strenght values".ToCharArray)
        _WS05MirrorUP(5) = _WS05Results.Mirror_Fs1_F1(mWS05Main.eMirrorPush.MirrorUP)
        _WS05MirrorUP(6) = _WS05Results.Mirror_Xs1(mWS05Main.eMirrorPush.MirrorUP)
        _WS05MirrorUP(7) = _WS05Results.Mirror_dFs1_Haptic_1(mWS05Main.eMirrorPush.MirrorUP)
        _WS05MirrorUP(8) = _WS05Results.Mirror_dXs1(mWS05Main.eMirrorPush.MirrorUP)

        _WS05MirrorUP(9) = New String("Analog values before commutation".ToCharArray)
        _WS05MirrorUP(10) = _WS05Results.Mirror_Push_Electric(0, mWS05Main.eMirrorPush.MirrorUP)
        _WS05MirrorUP(11) = New String("Analog values after commutation Ce1".ToCharArray)
        _WS05MirrorUP(12) = _WS05Results.Mirror_Push_Electric(1, mWS05Main.eMirrorPush.MirrorUP)
        _WS05MirrorUP(13) = New String("Analog values Initial Position".ToCharArray)
        _WS05MirrorUP(14) = _WS05Results.InitialState(mWS05Main.eMirrorPush.MirrorUP)


        InitResultsTable(dgvWS05MirrorUP, 15, _WS05MirrorUP, 790)

    End Sub

    Private Sub InitStationWS05TabsMirrorDN()
        Dim i As Integer
        'init tab
        _WS05MirrorDN(0) = New String("Early Communtation values".ToCharArray)
        _WS05MirrorDN(1) = _WS05Results.Mirror_XCe1(mWS05Main.eMirrorPush.MirrorDN)
        _WS05MirrorDN(2) = _WS05Results.Mirror_Xe(mWS05Main.eMirrorPush.MirrorDN)
        _WS05MirrorDN(3) = _WS05Results.Mirror_DiffS2Ce1(mWS05Main.eMirrorPush.MirrorDN)

        _WS05MirrorDN(4) = New String("Strenght values".ToCharArray)
        _WS05MirrorDN(5) = _WS05Results.Mirror_Fs1_F1(mWS05Main.eMirrorPush.MirrorDN)
        _WS05MirrorDN(6) = _WS05Results.Mirror_Xs1(mWS05Main.eMirrorPush.MirrorDN)
        _WS05MirrorDN(7) = _WS05Results.Mirror_dFs1_Haptic_1(mWS05Main.eMirrorPush.MirrorDN)
        _WS05MirrorDN(8) = _WS05Results.Mirror_dXs1(mWS05Main.eMirrorPush.MirrorDN)

        _WS05MirrorDN(9) = New String("Analog values before commutation".ToCharArray)
        _WS05MirrorDN(10) = _WS05Results.Mirror_Push_Electric(0, mWS05Main.eMirrorPush.MirrorDN)
        _WS05MirrorDN(11) = New String("Analog values after commutation Ce1".ToCharArray)
        _WS05MirrorDN(12) = _WS05Results.Mirror_Push_Electric(1, mWS05Main.eMirrorPush.MirrorDN)

        _WS05MirrorDN(13) = New String("Analog values Initial Position".ToCharArray)
        _WS05MirrorDN(14) = _WS05Results.InitialState(mWS05Main.eMirrorPush.MirrorDN)

        InitResultsTable(dgvWS05MirrorDN, 15, _WS05MirrorDN, 790)

    End Sub

    Private Sub InitStationWS05TabsMirrorMR()
        Dim i As Integer
        'init tab
        _WS05MirrorMR(0) = New String("Early Communtation values".ToCharArray)
        _WS05MirrorMR(1) = _WS05Results.Mirror_XCe1(mWS05Main.eMirrorPush.MirrorMR)
        _WS05MirrorMR(2) = _WS05Results.Mirror_Xe(mWS05Main.eMirrorPush.MirrorMR)
        _WS05MirrorMR(3) = _WS05Results.Mirror_DiffS2Ce1(mWS05Main.eMirrorPush.MirrorMR)

        _WS05MirrorMR(4) = New String("Strenght values".ToCharArray)
        _WS05MirrorMR(5) = _WS05Results.Mirror_Fs1_F1(mWS05Main.eMirrorPush.MirrorMR)
        _WS05MirrorMR(6) = _WS05Results.Mirror_Xs1(mWS05Main.eMirrorPush.MirrorMR)
        _WS05MirrorMR(7) = _WS05Results.Mirror_dFs1_Haptic_1(mWS05Main.eMirrorPush.MirrorMR)
        _WS05MirrorMR(8) = _WS05Results.Mirror_dXs1(mWS05Main.eMirrorPush.MirrorMR)

        _WS05MirrorMR(9) = New String("Analog values before commutation".ToCharArray)
        _WS05MirrorMR(10) = _WS05Results.Mirror_Push_Electric(0, mWS05Main.eMirrorPush.MirrorMR)
        _WS05MirrorMR(11) = New String("Analog values after commutation Ce1".ToCharArray)
        _WS05MirrorMR(12) = _WS05Results.Mirror_Push_Electric(1, mWS05Main.eMirrorPush.MirrorMR)

        _WS05MirrorMR(13) = New String("Analog values Initial Position".ToCharArray)
        _WS05MirrorMR(14) = _WS05Results.InitialState(mWS05Main.eMirrorPush.MirrorMR)

        InitResultsTable(dgvWS05MirrorRight, 15, _WS05MirrorMR, 790)

    End Sub

    Private Sub InitStationWS05TabsMirrorML()
        Dim i As Integer
        'init tab
        _WS05MirrorML(0) = New String("Early Communtation values".ToCharArray)
        _WS05MirrorML(1) = _WS05Results.Mirror_XCe1(mWS05Main.eMirrorPush.MirrorML)
        _WS05MirrorML(2) = _WS05Results.Mirror_Xe(mWS05Main.eMirrorPush.MirrorML)
        _WS05MirrorML(3) = _WS05Results.Mirror_DiffS2Ce1(mWS05Main.eMirrorPush.MirrorML)

        _WS05MirrorML(4) = New String("Strenght values".ToCharArray)
        _WS05MirrorML(5) = _WS05Results.Mirror_Fs1_F1(mWS05Main.eMirrorPush.MirrorML)
        _WS05MirrorML(6) = _WS05Results.Mirror_Xs1(mWS05Main.eMirrorPush.MirrorML)
        _WS05MirrorML(7) = _WS05Results.Mirror_dFs1_Haptic_1(mWS05Main.eMirrorPush.MirrorML)
        _WS05MirrorML(8) = _WS05Results.Mirror_dXs1(mWS05Main.eMirrorPush.MirrorML)

        _WS05MirrorML(9) = New String("Analog values before commutation".ToCharArray)
        _WS05MirrorML(10) = _WS05Results.Mirror_Push_Electric(0, mWS05Main.eMirrorPush.MirrorML)
        _WS05MirrorML(11) = New String("Analog values after commutation Ce1".ToCharArray)
        _WS05MirrorML(12) = _WS05Results.Mirror_Push_Electric(1, mWS05Main.eMirrorPush.MirrorML)

        _WS05MirrorML(13) = New String("Analog values Initial Position".ToCharArray)
        _WS05MirrorML(14) = _WS05Results.InitialState(mWS05Main.eMirrorPush.MirrorML)

        InitResultsTable(dgvWS05MirrorLeft, 15, _WS05MirrorML, 790)

    End Sub

    Private Sub InitStationWS05TabsMirrorSR()
        Dim i As Integer
        'init tab
        _WS05MirrorSR(0) = New String("Early Communtation values".ToCharArray)
        _WS05MirrorSR(1) = _WS05Results.Mirror_XCe1(mWS05Main.eMirrorPush.MirrorSR)
        _WS05MirrorSR(2) = _WS05Results.Mirror_Xe(mWS05Main.eMirrorPush.MirrorSR)
        _WS05MirrorSR(3) = _WS05Results.Mirror_DiffS2Ce1(mWS05Main.eMirrorPush.MirrorSR)

        _WS05MirrorSR(4) = New String("Strenght values".ToCharArray)
        _WS05MirrorSR(5) = _WS05Results.Mirror_Fs1_F1(mWS05Main.eMirrorPush.MirrorSR)
        _WS05MirrorSR(6) = _WS05Results.Mirror_Xs1(mWS05Main.eMirrorPush.MirrorSR)
        _WS05MirrorSR(7) = _WS05Results.Mirror_dFs1_Haptic_1(mWS05Main.eMirrorPush.MirrorSR)
        _WS05MirrorSR(8) = _WS05Results.Mirror_dXs1(mWS05Main.eMirrorPush.MirrorSR)

        _WS05MirrorSR(9) = New String("Analog values before commutation".ToCharArray)
        _WS05MirrorSR(10) = _WS05Results.Mirror_Push_Electric(0, mWS05Main.eMirrorPush.MirrorSR)
        _WS05MirrorSR(11) = New String("Analog values after commutation Ce1".ToCharArray)
        _WS05MirrorSR(12) = _WS05Results.Mirror_Push_Electric(1, mWS05Main.eMirrorPush.MirrorSR)

        _WS05MirrorSR(13) = New String("Analog values Initial Position".ToCharArray)
        _WS05MirrorSR(14) = _WS05Results.InitialState(mWS05Main.eMirrorPush.MirrorSR)

        InitResultsTable(dgvWS05MirrorSR, 15, _WS05MirrorSR, 790)

    End Sub

    Private Sub InitStationWS05TabsMirrorSL()
        Dim i As Integer
        'init tab
        _WS05MirrorSL(0) = New String("Early Communtation values".ToCharArray)
        _WS05MirrorSL(1) = _WS05Results.Mirror_XCe1(mWS05Main.eMirrorPush.MirrorSL)
        _WS05MirrorSL(2) = _WS05Results.Mirror_Xe(mWS05Main.eMirrorPush.MirrorSL)
        _WS05MirrorSL(3) = _WS05Results.Mirror_DiffS2Ce1(mWS05Main.eMirrorPush.MirrorSL)

        _WS05MirrorSL(4) = New String("Strenght values".ToCharArray)
        _WS05MirrorSL(5) = _WS05Results.Mirror_Fs1_F1(mWS05Main.eMirrorPush.MirrorSL)
        _WS05MirrorSL(6) = _WS05Results.Mirror_Xs1(mWS05Main.eMirrorPush.MirrorSL)
        _WS05MirrorSL(7) = _WS05Results.Mirror_dFs1_Haptic_1(mWS05Main.eMirrorPush.MirrorSL)
        _WS05MirrorSL(8) = _WS05Results.Mirror_dXs1(mWS05Main.eMirrorPush.MirrorSL)

        _WS05MirrorSL(9) = New String("Analog values before commutation".ToCharArray)
        _WS05MirrorSL(10) = _WS05Results.Mirror_Push_Electric(0, mWS05Main.eMirrorPush.MirrorSL)
        _WS05MirrorSL(11) = New String("Analog values after commutation Ce1".ToCharArray)
        _WS05MirrorSL(12) = _WS05Results.Mirror_Push_Electric(1, mWS05Main.eMirrorPush.MirrorSL)

        _WS05MirrorSL(13) = New String("Analog values Initial Position".ToCharArray)
        _WS05MirrorSL(14) = _WS05Results.InitialState(mWS05Main.eMirrorPush.MirrorSL)

        InitResultsTable(dgvWS05MirrorSL, 15, _WS05MirrorSL, 790)

    End Sub

    Private Sub InitStationWS05TabsFinalState()
        Dim i As Integer
        'init tab
        _WS05FinalState(0) = New String("Electric values".ToCharArray)
        For i = 0 To 27
            _WS05FinalState(1 + i) = _WS05Results.FinalState(i)
        Next

        InitResultsTable(dgvWS05FinalState, 29, _WS05FinalState, 730)

    End Sub

    Public Sub ShowResults()
        ' Show the station WS02 values
        ShowStationWS02Values()
        ' Show the station WS03 values
        ShowStationWS03Values()
        ' Show the station WS04 values
        ShowStationWS04Values()
        ' Show the station WS05 values
        ShowStationWS05Values()
    End Sub

    Private Sub ShowStationWS02Values()
        ' General Information
        ShowValues(dgvWS02GeneralInformation, _WS02GeneralInformation)
        ' Power UP
        ShowValues(dgvWS02PowerUp, _WS02PowerSypply)
        ' Current Mode
        ShowValues(dgvWS02CurrentMode, _WS02CurrentMode)
        ' Serial number
        ShowValues(dgvWS02ValeoSerialNumber, _WS02SerialNumber)
        '
        ShowValues(dgvWS02Shape, _WS02Shape)

        ShowValues(dgvWS02Shape2, _WS02Shape2)
        '
        ShowValues(dgvWS02Backlight_MS, _WS02BacklightMS)
        ShowValues(dgvWS02Backlight_MA, _WS02BacklightMA)
        ShowValues(dgvWS02Backlight_WLF, _WS02BacklightWLF)
        ShowValues(dgvWS02Backlight_WLR, _WS02BacklightWLR)
        ShowValues(dgvWS02Backlight_FL, _WS02BacklightFL)
        ShowValues(dgvWS02Backlight_CL, _WS02BacklightCL)
        '
        ShowValues(dgvWS02TellTale, _WS02TellTale)
        ' 
        ShowValues(dgvWS02Analog, _WS02AnalogInput)
        '
        ShowValues(dgvWS02PWM_WLAP, _WS02PWMOutput_1)
        ShowGraphPWM(chWLAP, 0)
        ShowValues(dgvWS02PWM_DOORLock, _WS02PWMOutput_2)
        ShowGraphPWM(chDoorLock, 1)

        ShowValues(dgvWS02MMS, _WS02MMSTraceability)
        '
        ShowValues(dgvWS02EMS, _WS02EMSTraceability)

        ShowValues(dgvWS02FrontPassengerUP, _WS02DO_UP_FrontPassenger)
        ShowValues(dgvWS02FrontPassengerDN, _WS02DO_DN_FrontPassenger)
        ShowValues(dgvWS02SwitchInhibition, _WS02DO_Switch_Inhibition)
        ShowValues(dgvWS02RearRightUP, _WS02DO_UP_RearRight)
        ShowValues(dgvWS02RearRightDN, _WS02DO_DN_RearRight)
        ShowValues(dgvWS02RearLeftUP, _WS02DO_UP_RearLeft)
        ShowValues(dgvWS02RearLeftDN, _WS02DO_DN_RearLeft)
        ShowValues(dgvWS02RBT_RTRV_G_Fold, _WS02DO_CDE_RBT_RTRV_G_Fold)
        ShowValues(dgvWS02RBT_RTRV_D_Fold, _WS02DO_CDE_RBT_RTRV_D_Fold)
        ShowValues(dgvWS02RTRV_HB_G, _WS02DO_CDE_RTRV_HB_G)
        ShowValues(dgvWS02RTRV_DG_G, _WS02DO_CDE_RTRV_DG_G)
        ShowValues(dgvWS02RTRV_HB_D, _WS02DO_CDE_RTRV_HB_D)
        ShowValues(dgvWS02RTRV_DG_D, _WS02DO_CDE_RTRV_DG_D)

    End Sub


    Private Sub ShowStationWS03Values()
        '
        ' General Information
        ShowValues(dgvWS03GeneralInformation, _WS03GeneralInformation)
        ' Power UP
        ShowValues(dgvWS03PowerUp, _WS03PowerSypply)
        '
        ShowValues(dgvWS03SerialNumber, _WS03ValeoSerialNumber)
        '
        ShowValues(dgvWS03FrontLeftUP, _WS03FrontLeftPull)
        ShowGraph_WS03(chWS03FLUPElectricalTest, cWS03Results.eWindowsLifterTest.FrontLeft_Pull, ePush_Pull.Pull)
        '
        ShowValues(dgvWS03FrontLeftDN, _WS03FrontLeftPush)
        ShowGraph_WS03(chWS03FLDNElectricalTest, cWS03Results.eWindowsLifterTest.FrontLeft_Push, ePush_Pull.Push)
        '
        ShowValues(dgvWS03FrontRightUP, _WS03FrontRightPull)
        ShowGraph_WS03(chWS03FRUPElectricalTest, cWS03Results.eWindowsLifterTest.FrontRight_Pull, ePush_Pull.Pull)
        '
        ShowValues(dgvWS03FrontRightDN, _WS03FrontRightPush)
        ShowGraph_WS03(chWS03RRDNElectricalTest, cWS03Results.eWindowsLifterTest.FrontRight_Push, ePush_Pull.Push)

        ShowValues(dgvWS03ChildrenLock, _WS03PushChildrenLock)
        ShowGraphPush(chWS03ChildrenElectricalTest)
        '
        ShowValues(dgvWS03FinalState, _WS03FinalState)
        '


    End Sub

    Private Sub ShowStationWS04Values()
        '
        ' General Information
        ShowValues(dgvWS04GeneralInformation, _WS04GeneralInformation)
        ' Power UP
        ShowValues(dgvWS04PowerUp, _WS04PowerSypply)
        '
        ShowValues(dgvWS04SerialNumber, _WS04ValeoSerialNumber)
        '
        ShowValues(dgvWS04RearLeftUP, _WS04RearLeftPull)
        ShowGraph_ws04(chWS04RLUPElectricalTest, cWS04Results.eWindowsLifterTest.RearLeft_Pull, ePush_Pull.Pull)
        '
        ShowValues(dgvWS04RearLeftDN, _WS04RearLeftPush)
        ShowGraph_ws04(chWS04RLDNElectricalTest, cWS04Results.eWindowsLifterTest.RearLeft_Push, ePush_Pull.Push)
        '
        ShowValues(dgvWS04RearRightUP, _WS04RearRightPull)
        ShowGraph_ws04(chWS04RRUPElectricalTest, cWS04Results.eWindowsLifterTest.RearRight_Pull, ePush_Pull.Pull)
        '
        ShowValues(dgvWS04RearRightDN, _WS04RearRightPush)
        ShowGraph_ws04(chWS04RRDNElectricalTest, cWS04Results.eWindowsLifterTest.RearRight_Push, ePush_Pull.Push)
        '
        ShowValues(dgvWS04MirrorFolding, _WS04PushMirrorFolding)
        ShowGraphPushMF(chWS04MFElectricalTest)
        '
        ShowValues(dgvWS04FinalState, _WS04FinalState)
        '


    End Sub


    Private Sub ShowStationWS05Values()
        '
        ' General Information
        ShowValues(dgvWS05GeneralInformation, _WS05GeneralInformation)
        ' Power UP
        ShowValues(dgvWS05PowerUp, _WS05PowerSypply)
        '
        ShowValues(dgvWS05SerialNumber, _WS05ValeoSerialNumber)
        '
        ShowValues(dgvWS05MirrorUP, _WS05MirrorUP)
        ShowGraphPushMirror(chWS05MUPElectricalTest, mWS05Main.eMirrorPush.MirrorUP)
        '
        ShowValues(dgvWS05MirrorDN, _WS05MirrorDN)
        ShowGraphPushMirror(chWS05MDNElectricalTest, mWS05Main.eMirrorPush.MirrorDN)
        '
        ShowValues(dgvWS05MirrorRight, _WS05MirrorMR)
        ShowGraphPushMirror(chWS05MRElectricalTest, mWS05Main.eMirrorPush.MirrorMR)
        '
        ShowValues(dgvWS05MirrorLeft, _WS05MirrorML)
        ShowGraphPushMirror(chWS05MLElectricalTest, mWS05Main.eMirrorPush.MirrorML)
        '
        ShowValues(dgvWS05MirrorSR, _WS05MirrorSR)
        ShowGraphPushMirror(chWS05MSRElectricalTest, mWS05Main.eMirrorPush.MirrorSR)
        '
        ShowValues(dgvWS05MirrorSL, _WS05MirrorSL)
        ShowGraphPushMirror(chWS05MSLElectricalTest, mWS05Main.eMirrorPush.MirrorSL)
        '
        ShowValues(dgvWS05FinalState, _WS05FinalState)
        '


    End Sub


    Private Sub pbLogoValeo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbLogoValeo.Click
        ' Make the form invisible
        'Me.Visible = False
        Me.Close()
    End Sub

    Private Sub InitResultsTable(ByRef table As DataGridView, _
                                 ByVal rowCount As Integer, _
                                 ByRef objectReference() As Object, _
                                 ByVal iHeight As Integer)
        Dim resultValue As cResultValue

        ' Reference to the table
        With table
            .SendToBack()
            ' Disable the refresh
            .SuspendLayout()
            ' Configurate the rows
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .RowHeadersVisible = False
            .RowCount = rowCount
            ' Configurate the columns
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .ColumnHeadersVisible = False
            .ColumnCount = 5
            ' Configurate the cells
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .DefaultCellStyle.Font = New Font("Arial", 12)
            For i = 0 To .RowCount - 1
                .Rows(i).Height = 27
                .Item(0, i).Style.Alignment = DataGridViewContentAlignment.MiddleLeft
                .Item(1, i).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
                .Item(2, i).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
            Next
            .Height = .Rows(0).Height * .RowCount
            If .Height > iHeight Then
                .Height = iHeight
            End If
            ' Configurate the scrollbars and the column width
            If (.Rows(0).Height * .RowCount <= .Height) Then
                .ScrollBars = ScrollBars.None
                .Columns(0).Width = CInt(0.45 * .Width)
                .Columns(1).Width = CInt(0.15 * .Width)
                .Columns(2).Width = CInt(0.15 * .Width)
                .Columns(3).Width = CInt(0.15 * .Width)
                .Columns(4).Width = CInt(0.1 * .Width)
            Else
                .ScrollBars = ScrollBars.Vertical
                .Columns(0).Width = CInt(0.45 * (.Width - SystemInformation.VerticalScrollBarWidth))
                .Columns(1).Width = CInt(0.15 * (.Width - SystemInformation.VerticalScrollBarWidth))
                .Columns(2).Width = CInt(0.15 * (.Width - SystemInformation.VerticalScrollBarWidth))
                .Columns(3).Width = CInt(0.15 * (.Width - SystemInformation.VerticalScrollBarWidth))
                .Columns(4).Width = CInt(0.1 * (.Width - SystemInformation.VerticalScrollBarWidth))
            End If
            ' Configurate the edit mode
            .EditMode = DataGridViewEditMode.EditOnKeystroke
            ' Disable multiple selection
            .MultiSelect = False

            On Error GoTo errhandling

            ' Print the recipe value descriptions
            For i = 0 To .RowCount - 1
                If (TypeOf (objectReference(i)) Is String) Then
                    .SendToBack()
                    .Item(2, i).Value = objectReference(i).ToString
                    .Item(2, i).Style.Font = New Font("Arial", 12, FontStyle.Bold)
                Else
                    resultValue = CType(objectReference(i), cResultValue)
                    .Item(0, i).Value = resultValue.Description
                    .Item(4, i).Value = resultValue.Units
                End If
            Next
            .ReadOnly = True
            .SelectionMode = DataGridViewSelectionMode.FullRowSelect
            ' Enable the refresh
            .ResumeLayout(True)
        End With

        Exit Sub

errhandling:
        resultValue = resultValue

    End Sub


    Private Sub ShowValues(ByRef table As DataGridView,
                           ByRef objectReference() As Object)
        Dim resultValue As cResultValue

        table.SuspendLayout()
        For i = 0 To table.RowCount - 1
            If (TypeOf (objectReference(i)) Is cResultValue) Then
                resultValue = CType(objectReference(i), cResultValue)
                If (resultValue.TestResult <> cResultValue.eValueTestResult.NotTested) And
                    (resultValue.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                    table.Item(1, i).Value = resultValue.StringMinimumLimit
                End If
                If (resultValue.TestResult = cResultValue.eValueTestResult.Disabled Or resultValue.TestResult = cResultValue.eValueTestResult.NotValueCheck) Then
                    table.Item(2, i).Value = "---"
                    table.Item(2, i).Style.BackColor = Color.DarkGray
                ElseIf (resultValue.TestResult = cResultValue.eValueTestResult.Unknown) Then
                    table.Item(2, i).Value = "???"
                    table.Item(2, i).Style.BackColor = Color.Yellow
                Else
                    table.Item(2, i).Value = resultValue.StringValue
                    If (resultValue.TestResult = cResultValue.eValueTestResult.NotTested) Then
                        table.Item(2, i).Style.BackColor = Color.White
                    ElseIf (resultValue.TestResult = cResultValue.eValueTestResult.Passed) Then
                        table.Item(2, i).Style.BackColor = Color.LawnGreen
                    ElseIf (resultValue.TestResult = cResultValue.eValueTestResult.Failed) Then
                        table.Item(2, i).Style.BackColor = Color.Red
                    ElseIf (resultValue.TestResult = cResultValue.eValueTestResult.NotCoherent) Then
                        table.Item(2, i).Style.BackColor = Color.Magenta
                    End If
                End If

                If resultValue.ValueType = cResultValue.eValueType.SingleValue And
                    (table.Name.Contains("UP") Or table.Name.Contains("DN")) And
                    (resultValue.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                    If CSng(resultValue.MaximumLimit) >= 9999 Then
                        table.Item(3, i).Value = "---"
                        table.Item(3, i).Style.BackColor = Color.DarkGray
                    Else
                        table.Item(3, i).Value = resultValue.StringMaximumLimit
                        table.Item(3, i).Style.BackColor = Color.White
                    End If
                Else
                    If (resultValue.TestResult = cResultValue.eValueTestResult.NotTested) Then
                        table.Item(3, i).Value = ""
                    ElseIf (resultValue.TestResult <> cResultValue.eValueTestResult.Disabled) Then
                        table.Item(3, i).Value = resultValue.StringMaximumLimit
                    End If
                    table.Item(3, i).Style.BackColor = Color.White
                End If

            End If
        Next
        'table.RefreshEdit()
        table.ResumeLayout()
    End Sub

    Private Sub tmrMonitor_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMonitor.Tick
        Dim g As Graphics
        If tcWS02A.SelectedTab.Name = "tpBacklight" Then
            g = Me.pbBacklight.CreateGraphics
            If tcBacklight.SelectedIndex = 0 Then
                DrawLineforPolygon(g, _WS02Results.Push_SELECT_RIGHT_Backlight_x.Value, _WS02Results.Push_SELECT_RIGHT_Backlight_y.Value,
                                   _WS02Results.Push_SELECT_RIGHT_Backlight_Polygon_Axy,
                                    _WS02Results.Push_SELECT_RIGHT_Backlight_Polygon_Bxy,
                                    _WS02Results.Push_SELECT_RIGHT_Backlight_Polygon_Cxy,
                                    _WS02Results.Push_SELECT_RIGHT_Backlight_Polygon_Dxy,
                                    _WS02Results.Push_SELECT_RIGHT_Backlight_Polygon_Exy,
                                    _WS02Results.Push_SELECT_RIGHT_Backlight_Polygon_Fxy)
                DrawLineforPolygon(g, _WS02Results.Push_SELECT_LEFT_Backlight_x.Value, _WS02Results.Push_SELECT_LEFT_Backlight_y.Value,
                                   _WS02Results.Push_SELECT_LEFT_Backlight_Polygon_Axy,
                                    _WS02Results.Push_SELECT_LEFT_Backlight_Polygon_Bxy,
                                    _WS02Results.Push_SELECT_LEFT_Backlight_Polygon_Cxy,
                                    _WS02Results.Push_SELECT_LEFT_Backlight_Polygon_Dxy,
                                    _WS02Results.Push_SELECT_LEFT_Backlight_Polygon_Exy,
                                    _WS02Results.Push_SELECT_LEFT_Backlight_Polygon_Fxy)
            ElseIf tcBacklight.SelectedIndex = 1 Then
                DrawLineforPolygon(g, _WS02Results.Push_ADJUST_UP_Backlight_x.Value, _WS02Results.Push_ADJUST_UP_Backlight_y.Value,
                                   _WS02Results.Push_ADJUST_UP_Backlight_Polygon_Axy,
                                    _WS02Results.Push_ADJUST_UP_Backlight_Polygon_Bxy,
                                    _WS02Results.Push_ADJUST_UP_Backlight_Polygon_Cxy,
                                    _WS02Results.Push_ADJUST_UP_Backlight_Polygon_Dxy,
                                    _WS02Results.Push_ADJUST_UP_Backlight_Polygon_Exy,
                                    _WS02Results.Push_ADJUST_UP_Backlight_Polygon_Fxy)
                DrawLineforPolygon(g, _WS02Results.Push_ADJUST_DOWN_Backlight_x.Value, _WS02Results.Push_ADJUST_DOWN_Backlight_y.Value,
                                   _WS02Results.Push_ADJUST_DOWN_Backlight_Polygon_Axy,
                                    _WS02Results.Push_ADJUST_DOWN_Backlight_Polygon_Bxy,
                                    _WS02Results.Push_ADJUST_DOWN_Backlight_Polygon_Cxy,
                                    _WS02Results.Push_ADJUST_DOWN_Backlight_Polygon_Dxy,
                                    _WS02Results.Push_ADJUST_DOWN_Backlight_Polygon_Exy,
                                    _WS02Results.Push_ADJUST_DOWN_Backlight_Polygon_Fxy)
                DrawLineforPolygon(g, _WS02Results.Push_ADJUST_LEFT_Backlight_x.Value, _WS02Results.Push_ADJUST_LEFT_Backlight_y.Value,
                                   _WS02Results.Push_ADJUST_LEFT_Backlight_Polygon_Axy,
                                    _WS02Results.Push_ADJUST_LEFT_Backlight_Polygon_Bxy,
                                    _WS02Results.Push_ADJUST_LEFT_Backlight_Polygon_Cxy,
                                    _WS02Results.Push_ADJUST_LEFT_Backlight_Polygon_Dxy,
                                    _WS02Results.Push_ADJUST_LEFT_Backlight_Polygon_Exy,
                                    _WS02Results.Push_ADJUST_LEFT_Backlight_Polygon_Fxy)
                DrawLineforPolygon(g, _WS02Results.Push_ADJUST_RIGHT_Backlight_x.Value, _WS02Results.Push_ADJUST_RIGHT_Backlight_y.Value,
                                   _WS02Results.Push_ADJUST_RIGHT_Backlight_Polygon_Axy,
                                    _WS02Results.Push_ADJUST_RIGHT_Backlight_Polygon_Bxy,
                                    _WS02Results.Push_ADJUST_RIGHT_Backlight_Polygon_Cxy,
                                    _WS02Results.Push_ADJUST_RIGHT_Backlight_Polygon_Dxy,
                                    _WS02Results.Push_ADJUST_RIGHT_Backlight_Polygon_Exy,
                                    _WS02Results.Push_ADJUST_RIGHT_Backlight_Polygon_Fxy)
            ElseIf tcBacklight.SelectedIndex = 2 Then
                DrawLineforPolygon(g, _WS02Results.Push_FOLDING_Backlight_x.Value, _WS02Results.Push_FOLDING_Backlight_y.Value,
                                   _WS02Results.Push_FOLDING_Backlight_Polygon_Axy,
                                    _WS02Results.Push_FOLDING_Backlight_Polygon_Bxy,
                                    _WS02Results.Push_FOLDING_Backlight_Polygon_Cxy,
                                    _WS02Results.Push_FOLDING_Backlight_Polygon_Dxy,
                                    _WS02Results.Push_FOLDING_Backlight_Polygon_Exy,
                                    _WS02Results.Push_FOLDING_Backlight_Polygon_Fxy)
            ElseIf tcBacklight.SelectedIndex = 3 Then
                DrawLineforPolygon(g, _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_x.Value, _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_y.Value,
                                   _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Axy,
                                    _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Bxy,
                                    _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Cxy,
                                    _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Dxy,
                                    _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Exy,
                                    _WS02Results.WINDOWS_LIFTER_FRONT_LEFT_Backlight_Polygon_Fxy)
                DrawLineforPolygon(g, _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_x.Value, _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_y.Value,
                                   _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Axy,
                                    _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Bxy,
                                    _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Cxy,
                                    _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Dxy,
                                    _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Exy,
                                    _WS02Results.WINDOWS_LIFTER_FRONT_RIGHT_Backlight_Polygon_Fxy)
            ElseIf tcBacklight.SelectedIndex = 4 Then
                DrawLineforPolygon(g, _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_x.Value, _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_y.Value,
                                   _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Axy,
                                    _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Bxy,
                                    _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Cxy,
                                    _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Dxy,
                                    _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Exy,
                                    _WS02Results.WINDOWS_LIFTER_REAR_LEFT_Backlight_Polygon_Fxy)
                DrawLineforPolygon(g, _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_x.Value, _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_y.Value,
                                   _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Axy,
                                    _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Bxy,
                                    _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Cxy,
                                    _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Dxy,
                                    _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Exy,
                                    _WS02Results.WINDOWS_LIFTER_REAR_RIGHT_Backlight_Polygon_Fxy)
            ElseIf tcBacklight.SelectedIndex = 5 Then
                DrawLineforPolygon(g, _WS02Results.Push_CHILDREN_LOCK_Backlight_x.Value, _WS02Results.Push_CHILDREN_LOCK_Backlight_y.Value,
                                   _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Axy,
                                    _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Bxy,
                                    _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Cxy,
                                    _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Dxy,
                                    _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Exy,
                                    _WS02Results.Push_CHILDREN_LOCK_Backlight_Polygon_Fxy)
            End If
        ElseIf tcWS02A.SelectedTab.Name = "tpTellTale" Then
            g = Me.pbTallTele.CreateGraphics
            DrawLineforPolygon(g, _WS02Results.Push_CHILDREN_LOCK_LED_x.Value, _WS02Results.Push_CHILDREN_LOCK_LED_y.Value,
                                   _WS02Results.Push_CHILDREN_LOCK_LED_Polygon_Axy,
                                    _WS02Results.Push_CHILDREN_LOCK_LED_Polygon_Bxy,
                                    _WS02Results.Push_CHILDREN_LOCK_LED_Polygon_Cxy,
                                    _WS02Results.Push_CHILDREN_LOCK_LED_Polygon_Dxy)
            DrawLineforPolygon(g, _WS02Results.Push_SELECT_LEFT_LED_x.Value, _WS02Results.Push_SELECT_LEFT_LED_y.Value,
                                   _WS02Results.Push_SELECT_LEFT_LED_Polygon_Axy,
                                    _WS02Results.Push_SELECT_LEFT_LED_Polygon_Bxy,
                                    _WS02Results.Push_SELECT_LEFT_LED_Polygon_Cxy,
                                    _WS02Results.Push_SELECT_LEFT_LED_Polygon_Dxy)
            DrawLineforPolygon(g, _WS02Results.Push_SELECT_RIGHT_LED_x.Value, _WS02Results.Push_SELECT_RIGHT_LED_y.Value,
                                   _WS02Results.Push_SELECT_RIGHT_LED_Polygon_Axy,
                                    _WS02Results.Push_SELECT_RIGHT_LED_Polygon_Bxy,
                                    _WS02Results.Push_SELECT_RIGHT_LED_Polygon_Cxy,
                                    _WS02Results.Push_SELECT_RIGHT_LED_Polygon_Dxy)
        End If

        ' Update the status bar
        mStatusBar.Update(ssStatusBar)
    End Sub

    Private Sub DrawLineforPolygon(ByRef g As Graphics, ByVal xvalue As Single, ByVal yvalue As Single, ParamArray ByVal xylimit() As cResultValue)
        Dim polygonnumber As Integer = xylimit.Count
        Dim Decla_x As Integer = 30 '35
        Dim Decla_y As Integer = 696
        Dim x(polygonnumber - 1) As Single
        Dim y(polygonnumber - 1) As Single

        For index = 0 To polygonnumber - 1
            x(index) = xylimit(index).MinimumLimit
            y(index) = xylimit(index).MaximumLimit
        Next

        For index = 0 To polygonnumber - 2
            g.DrawLine(Pens.Green, CInt(Decla_x + (((615 - Decla_x) * x(index)) / 0.8)),
                                CInt(Decla_y - (((696 - 12) * y(index)) / 0.9)),
                                CInt(Decla_x + (((615 - Decla_x) * x(index + 1)) / 0.8)),
                                CInt(Decla_y - (((696 - 12) * y(index + 1)) / 0.9)))
        Next
        g.DrawLine(Pens.Green, CInt(Decla_x + (((615 - Decla_x) * x(polygonnumber - 1)) / 0.8)),
                                CInt(Decla_y - (((696 - 12) * y(polygonnumber - 1)) / 0.9)),
                                CInt(Decla_x + (((615 - Decla_x) * x(0)) / 0.8)),
                                CInt(Decla_y - (((696 - 12) * y(0)) / 0.9)))

        Dim x1 As Single ' = 0.335
        Dim y1 As Single ' = 0.35
        Dim x2 As Single ' = 0.35
        Dim y2 As Single ' = 0.372
        Dim x3 As Single ' = 0.363
        Dim y3 As Single ' = 0.363
        Dim x4 As Single ' = 0.347
        Dim y4 As Single ' = 0.341
        x1 = CSng(xvalue - 0.005) : y1 = CSng(yvalue - 0.005)
        x2 = CSng(xvalue + 0.005) : y2 = CSng(yvalue + 0.005)
        x3 = CSng(xvalue - 0.005) : y3 = CSng(yvalue + 0.005)
        x4 = CSng(xvalue + 0.005) : y4 = CSng(yvalue - 0.005)

        g.DrawLine(Pens.Black, CInt(Decla_x + (((615 - Decla_x) * x1) / 0.8)),
                                        CInt(Decla_y - (((696 - 12) * y1) / 0.9)),
                                        CInt(Decla_x + (((615 - Decla_x) * x2) / 0.8)),
                                        CInt(Decla_y - (((696 - 12) * y2) / 0.9)))
        g.DrawLine(Pens.Black, CInt(Decla_x + (((615 - Decla_x) * x3) / 0.8)),
                                CInt(Decla_y - (((696 - 12) * y3) / 0.9)),
                                CInt(Decla_x + (((615 - Decla_x) * x4) / 0.8)),
                                CInt(Decla_y - (((696 - 12) * y4) / 0.9)))
    End Sub

    Private Sub ShowGraph_WS03(ByRef chart As DataVisualization.Charting.Chart, _
                                ByVal WiliIndex As cWS03Results.eWindowsLifterTest, _
                                ByVal PushPull As ePush_Pull)
        Dim NbCurve As Integer = 3
        Dim Recalage As Single
        Dim X As Single
        Dim WindowsLifterTest As mWS03Main.eWindows
        Try
            ' Reference to the chart
            With chart
                ' Suspend the layout
                .SuspendLayout()
                ' Clear the series
                .Series.Clear()
                .ChartAreas(0).AxisX.Minimum = 0
                .ChartAreas(0).AxisX.Maximum = 10

                If WiliIndex = cWS03Results.eWindowsLifterTest.FrontLeft_Pull Or _
                    WiliIndex = cWS03Results.eWindowsLifterTest.FrontLeft_Push Then
                    WindowsLifterTest = mWS03Main.eWindows.FrontLeft
                Else
                    WindowsLifterTest = mWS03Main.eWindows.FrontRight
                End If
                If _WS03Results.WL_Xe(PushPull, WindowsLifterTest).Value > 0 Then .ChartAreas(0).AxisX.Maximum = _WS03Results.WL_Xe(PushPull, WindowsLifterTest).Value

                .ChartAreas(0).AxisX.Interval = 0.5
                .ChartAreas(0).AxisY.Minimum = 0
                .ChartAreas(0).AxisY.Maximum = 15
                .ChartAreas(0).AxisY.Interval = 0.5

                ' Add the series
                For i = 0 To NbCurve - 1
                    .Series.Add(CStr(i))
                    .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .Series(i).BorderWidth = 3
                    If i = 0 Then
                        .Series(i).Name = "Force (N)"
                    ElseIf i = 1 Then
                        .Series(i).Name = "Electric State 1 (V)"
                    ElseIf i = 2 Then
                        .Series(i).Name = "Electric State 2 (V)"
                    End If
                    '
                    If _WS03Results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, _WS03Results.WL_X_Indexes(WiliIndex).Xs) > 0 Then
                        Recalage = _WS03Results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, _WS03Results.WL_X_Indexes(WiliIndex).Xs)
                    End If
                    '
                    For j = _WS03Results.WL_X_Indexes(WiliIndex).Xs To _WS03Results.WL_X_Indexes(WiliIndex).Xe
                        ' Strenght curve
                        X = _WS03Results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.EarlySensor, j) - Recalage
                        If (WiliIndex = cWS03Results.eWindowsLifterTest.FrontLeft_Pull) Or _
                            (WiliIndex = cWS03Results.eWindowsLifterTest.FrontRight_Pull) Then
                            X = X * -1
                        End If
                        If i = 0 Then
                            .Series(i).Points.AddXY(X, _WS03Results.Samples_Push_Pull(WiliIndex, mWS03Main.cSample_Signal.StrenghtSensor, j))
                            ShowPoints_WS03(WiliIndex, j, .Series(i))
                        End If
                        ' Signal Curve
                        If i = 1 Then
                            .Series(i).Points.AddXY(X, _WS03Results.Samples_Push_Pull(WiliIndex, 2 * CInt(PushPull) + i + 1, j))
                        End If
                        ' Signal Curve
                        If i = 2 Then
                            .Series(i).Points.AddXY(X, _WS03Results.Samples_Push_Pull(WiliIndex, 2 * CInt(PushPull) + i + 1, j))
                        End If
                    Next
                Next
                ' Resume the layout
                .ResumeLayout()
            End With
        Catch ex As Exception

        End Try
    End Sub



    Private Sub ShowGraph_WS04(ByRef chart As DataVisualization.Charting.Chart,
                                ByVal WiliIndex As cWS04Results.eWindowsLifterTest,
                                ByVal PushPull As ePush_Pull)
        Dim NbCurve As Integer = 3
        Dim Recalage As Single
        Dim X As Single
        Dim WindowsLifterTest As mWS04Main.eWindows
        Try
            ' Reference to the chart
            With chart
                ' Suspend the layout
                .SuspendLayout()
                ' Clear the series
                .Series.Clear()
                .ChartAreas(0).AxisX.Minimum = 0
                .ChartAreas(0).AxisX.Maximum = 10

                If WiliIndex = cWS04Results.eWindowsLifterTest.RearLeft_Pull Or
                    WiliIndex = cWS04Results.eWindowsLifterTest.RearLeft_Push Then
                    WindowsLifterTest = mWS04Main.eWindows.RearLeft
                Else
                    WindowsLifterTest = mWS04Main.eWindows.RearRight
                End If
                If _WS04Results.WL_Xe(PushPull, WindowsLifterTest).Value > 0 Then .ChartAreas(0).AxisX.Maximum = _WS04Results.WL_Xe(PushPull, WindowsLifterTest).Value

                .ChartAreas(0).AxisX.Interval = 0.5
                .ChartAreas(0).AxisY.Minimum = 0
                .ChartAreas(0).AxisY.Maximum = 15
                .ChartAreas(0).AxisY.Interval = 0.5

                ' Add the series
                For i = 0 To NbCurve - 1
                    .Series.Add(CStr(i))
                    .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .Series(i).BorderWidth = 3
                    If i = 0 Then
                        .Series(i).Name = "Force (N)"
                    ElseIf i = 1 Then
                        .Series(i).Name = "Electric State 1 (V)"
                    ElseIf i = 2 Then
                        .Series(i).Name = "Electric State 2 (V)"
                    End If
                    '
                    If _WS04Results.Samples_Push_Pull(WiliIndex, mWS04Main.cSample_Signal.EarlySensor, _WS04Results.WL_X_Indexes(WiliIndex).Xs) > 0 Then
                        Recalage = _WS04Results.Samples_Push_Pull(WiliIndex, mWS04Main.cSample_Signal.EarlySensor, _WS04Results.WL_X_Indexes(WiliIndex).Xs)
                    End If
                    '
                    For j = _WS04Results.WL_X_Indexes(WiliIndex).Xs To _WS04Results.WL_X_Indexes(WiliIndex).Xe
                        ' Strenght curve
                        X = _WS04Results.Samples_Push_Pull(WiliIndex, mWS04Main.cSample_Signal.EarlySensor, j) - Recalage
                        If (WiliIndex = cWS04Results.eWindowsLifterTest.RearLeft_Pull) Or
                            (WiliIndex = cWS04Results.eWindowsLifterTest.RearRight_Pull) Then
                            X = X * -1
                        End If
                        If i = 0 Then
                            .Series(i).Points.AddXY(X, _WS04Results.Samples_Push_Pull(WiliIndex, mWS04Main.cSample_Signal.StrenghtSensor, j))
                            ShowPoints_WS04(WiliIndex, j, .Series(i))
                        End If
                        ' Signal Curve
                        If i = 1 Then
                            .Series(i).Points.AddXY(X, _WS04Results.Samples_Push_Pull(WiliIndex, 2 * CInt(PushPull) + i + 1, j))
                        End If
                        ' Signal Curve
                        If i = 2 Then
                            .Series(i).Points.AddXY(X, _WS04Results.Samples_Push_Pull(WiliIndex, 2 * CInt(PushPull) + i + 1, j))
                        End If
                    Next
                Next
                ' Resume the layout
                .ResumeLayout()
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ShowGraphPush(ByRef chart As DataVisualization.Charting.Chart)
        Dim NbCurve As Integer
        Dim Recalage As Single

        Try
            NbCurve = 2
            ' Reference to the chart
            With chart
                ' Suspend the layout
                .SuspendLayout()
                ' Clear the series
                .Series.Clear()
                .ChartAreas(0).AxisX.Minimum = 0
                .ChartAreas(0).AxisX.Maximum = 10
                If _WS03Results.CL_Xe.Value > 0 Then .ChartAreas(0).AxisX.Maximum = _WS03Results.CL_Xe.Value 
                .ChartAreas(0).AxisX.Interval = 0.5
                .ChartAreas(0).AxisY.Minimum = 0
                .ChartAreas(0).AxisY.Maximum = 15
                .ChartAreas(0).AxisY.Interval = 0.5

                ' Add the series
                For i = 0 To NbCurve - 1
                    .Series.Add(CStr(i))
                    .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .Series(i).BorderWidth = 3
                    If i = 0 Then
                        .Series(i).Name = "Force (N)"
                    ElseIf i = 1 Then
                        .Series(i).Name = "Electric State 1 (V)"
                    End If
                    '
                    If _WS03Results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, _WS03Results.CL_X_Indexes.Xs) > 0 Then
                        Recalage = _WS03Results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, _WS03Results.CL_X_Indexes.Xs)
                    End If
                    '
                    For j = _WS03Results.CL_X_Indexes.Xs To _WS03Results.CL_X_Indexes.Xe
                        ' Strenght curve
                        If i = 0 Then
                            .Series(i).Points.AddXY(_WS03Results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, j) - Recalage, _
                                                    _WS03Results.WS03_SamplePush(i + 1, j))
                            ShowPointsPush_WS03(j, .Series(i))
                        End If
                        ' Signal Curve
                        If i = 1 Then
                            .Series(i).Points.AddXY(_WS03Results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, j) - Recalage, _
                                                    _WS03Results.WS03_SamplePush(2, j))
                        End If
                    Next
                Next
                ' Resume the layout
                .ResumeLayout()
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ShowGraphPushMF(ByRef chart As DataVisualization.Charting.Chart)
        Dim NbCurve As Integer
        Dim Recalage As Single

        Try
            NbCurve = 2
            ' Reference to the chart
            With chart
                ' Suspend the layout
                .SuspendLayout()
                ' MFear the series
                .Series.Clear()
                .ChartAreas(0).AxisX.Minimum = 0
                .ChartAreas(0).AxisX.Maximum = 10
                If _WS04Results.MF_Xe.Value > 0 Then .ChartAreas(0).AxisX.Maximum = _WS04Results.MF_Xe.Value
                .ChartAreas(0).AxisX.Interval = 0.5
                .ChartAreas(0).AxisY.Minimum = 0
                .ChartAreas(0).AxisY.Maximum = 15
                .ChartAreas(0).AxisY.Interval = 0.5

                ' Add the series
                For i = 0 To NbCurve - 1
                    .Series.Add(CStr(i))
                    .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .Series(i).BorderWidth = 3
                    If i = 0 Then
                        .Series(i).Name = "Force (N)"
                    ElseIf i = 1 Then
                        .Series(i).Name = "Electric State 1 (V)"
                    End If
                    '
                    If _WS04Results.WS04_SamplePush(mWS04Main.cSample_Signal.EarlySensor, _WS04Results.MF_X_Indexes.Xs) > 0 Then
                        Recalage = _WS04Results.WS04_SamplePush(mWS04Main.cSample_Signal.EarlySensor, _WS04Results.MF_X_Indexes.Xs)
                    End If
                    '
                    For j = _WS04Results.MF_X_Indexes.Xs To _WS04Results.MF_X_Indexes.Xe
                        ' Strenght curve
                        If i = 0 Then
                            .Series(i).Points.AddXY(_WS04Results.WS04_SamplePush(mWS04Main.cSample_Signal.EarlySensor, j) - Recalage,
                                                    _WS04Results.WS04_SamplePush(i + 1, j))
                            ShowPointsPush_WS04(j, .Series(i))
                        End If
                        ' Signal Curve
                        If i = 1 Then
                            .Series(i).Points.AddXY(_WS04Results.WS04_SamplePush(mWS04Main.cSample_Signal.EarlySensor, j) - Recalage,
                                                    _WS04Results.WS04_SamplePush(2, j))
                        End If
                    Next
                Next
                ' Resume the layout
                .ResumeLayout()
            End With
        Catch ex As Exception

        End Try
    End Sub

    Private Sub ShowGraphPushMirror(ByRef chart As DataVisualization.Charting.Chart,
                                    ByVal MirrorPush As mWS05Main.eMirrorPush)
        Dim NbCurve As Integer
        Dim Recalage As Single

        Try

            NbCurve = 2
            ' Reference to the chart
            With chart
                ' Suspend the layout
                .SuspendLayout()
                ' Clear the series
                .Series.Clear()
                .ChartAreas(0).AxisX.Minimum = 0
                .ChartAreas(0).AxisX.Maximum = 10
                If _WS05Results.Mirror_Xe(MirrorPush).Value > 0 Then .ChartAreas(0).AxisX.Maximum = _WS05Results.Mirror_Xe(MirrorPush).Value
                .ChartAreas(0).AxisX.Interval = 0.2
                .ChartAreas(0).AxisY.Minimum = 0
                .ChartAreas(0).AxisY.Maximum = 15
                .ChartAreas(0).AxisY.Interval = 0.5


                ' Add the series
                For i = 0 To NbCurve - 1
                    .Series.Add(CStr(i))
                    .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
                    .Series(i).BorderWidth = 3
                    If i = 0 Then
                        .Series(i).Name = "Force (N)"
                    ElseIf i = 1 Then
                        .Series(i).Name = "Electric State 1 (V)"
                    End If
                    '
                    If _WS05Results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, _WS05Results.Mirror_X_Indexes(MirrorPush).Xs) > 0 Then
                        Recalage = _WS05Results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, _WS05Results.Mirror_X_Indexes(MirrorPush).Xs)
                    End If
                    '
                    For j = _WS05Results.Mirror_X_Indexes(MirrorPush).Xs To _WS05Results.Mirror_X_Indexes(MirrorPush).Xe
                        ' Strenght curve
                        If i = 0 Then
                            .Series(i).Points.AddXY(_WS05Results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, j) - Recalage,
                                                    _WS05Results.Sample(i + 1, MirrorPush, j))
                            ShowPointsMirror(j, .Series(i), MirrorPush)
                        End If
                        ' Signal Curve
                        If i = 1 Then
                            .Series(i).Points.AddXY(_WS05Results.Sample(mWS05Main.cSample_Signal.EarlySensor, MirrorPush, j) - Recalage,
                                                    _WS05Results.Sample(2 + MirrorPush, MirrorPush, j))
                        End If
                    Next
                Next
                ' Resume the layout
                .ResumeLayout()
            End With
        Catch ex As Exception

        End Try

    End Sub



    Private Sub lblFormTitle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblFormTitle.Click
        If cWS03Global.Visible = False Then
            frmHapticCurve.ShowDialog()
            TabSW090_100.Visible = False
            cWS03Global.Visible = True
            With cWS03Global
                ' Suspend the layout
                .SuspendLayout()
                ' Clear the series
                .Series.Clear()
                ' Add the series
                Select Case frmHapticCurve.station
                    Case 3
                        For i = 0 To 6
                            .Series.Add(CStr(i))
                            .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
                            .Series(i).BorderWidth = 3
                            If i <> 0 Then
                                For j = 0 To _WS03Results.Sample_TMP_AllKnobCount(WiliIndexShow) - 1
                                    ' Strenght curve
                                    If i > 0 Then
                                        .Series(i).Points.AddXY(j, _WS03Results.Sample_TMP_AllKnob(WiliIndexShow)(i - 1, j))
                                    End If
                                Next
                            End If
                        Next
                    Case 4
                        For i = 0 To 6
                            .Series.Add(CStr(i))
                            .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
                            .Series(i).BorderWidth = 3
                            If i <> 0 Then
                                For j = 0 To _WS04Results.Sample_TMP_AllKnobCount(WiliIndexShow) - 1
                                    ' Strenght curve
                                    If i > 0 Then
                                        .Series(i).Points.AddXY(j, _WS04Results.Sample_TMP_AllKnob(WiliIndexShow)(i - 1, j))
                                    End If
                                Next
                            End If
                        Next
                    Case 5
                        For i = 0 To 8
                            .Series.Add(CStr(i))
                            .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
                            .Series(i).BorderWidth = 3
                            If i <> 0 Then
                                For j = 0 To _WS05Results.Sample_TMP_AllKnobCount(WiliIndexShow) - 1
                                    ' Strenght curve
                                    If i > 0 Then
                                        .Series(i).Points.AddXY(j, _WS05Results.Sample_TMP_AllKnob(WiliIndexShow)(i - 1, j))
                                    End If
                                Next
                            End If
                        Next
                    Case Else
                End Select
                .ResumeLayout()
            End With
        Else
            TabSW090_100.Visible = True
            cWS03Global.Visible = False
        End If
        'If cWS03Global.Visible = False Then
        '    frmWili.ShowDialog()
        '    TabSW090_100.Visible = False
        '    cWS03Global.Visible = True
        '    ' Reference to the chart
        '    With cWS03Global
        '        ' Suspend the layout
        '        .SuspendLayout()
        '        ' Clear the series
        '        .Series.Clear()
        '        '.ChartAreas(0).AxisX.Minimum = 0
        '        '.ChartAreas(0).AxisX.Maximum = mWS03Main.FH_SampleIndex_ZF(WiliIndex) - mWS03Main.FH_SampleIndex_Z0(WiliIndex)
        '        '.ChartAreas(0).AxisX.Interval = 40
        '        '.ChartAreas(0).AxisY.Minimum = 0
        '        '.ChartAreas(0).AxisY.Maximum = 15
        '        '.ChartAreas(0).AxisY.Interval = 0.5

        '        ' Add the series
        '        For i = 0 To 6
        '            .Series.Add(CStr(i))
        '            .Series(i).ChartType = DataVisualization.Charting.SeriesChartType.Line
        '            .Series(i).BorderWidth = 3
        '            If i <> 0 Then
        '                For j = 0 To _WS03Results.SampleCount_TMP_WILI(WiliIndexShow)
        '                    ' Strenght curve
        '                    If i > 0 Then
        '                        .Series(i).Points.AddXY(j, _WS03Results.Sample_TMP_WILI(WiliIndexShow, i - 1, j))
        '                    End If
        '                Next
        '            End If
        '        Next
        '        ' Resume the layout
        '        .ResumeLayout()
        '    End With
        'Else
        '    TabSW090_100.Visible = True
        '    cWS03Global.Visible = False
        'End If
    End Sub

    Private Sub ShowPoints_WS03(ByVal WiliIndex As cWS03Results.eWindowsLifterTest, ByVal j As Integer, ByVal my_serie As DataVisualization.Charting.Series)

        If WiliIndex = cWS03Results.eWindowsLifterTest.FrontLeft_Push AndAlso
                            (j = _WS03Results.WL_X_Indexes(WiliIndex).X_F1 OrElse j = _WS03Results.WL_X_Indexes(WiliIndex).X_F2 OrElse j = _WS03Results.WL_X_Indexes(WiliIndex).X_F4 OrElse j = _WS03Results.WL_X_Indexes(WiliIndex).X_F5) Then
            Dim text As String = Nothing
            If j = _WS03Results.WL_X_Indexes(WiliIndex).X_F1 Then
                text = "F1"
            ElseIf j = _WS03Results.WL_X_Indexes(WiliIndex).X_F2 Then
                text = "F2"
            ElseIf j = _WS03Results.WL_X_Indexes(WiliIndex).X_F4 Then
                text = "F4"
            ElseIf j = _WS03Results.WL_X_Indexes(WiliIndex).X_F5 Then
                text = "F5"
            End If
            With my_serie.Points(my_serie.Points.Count - 1)
                .Label = text
                .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                .MarkerSize = 10
                .MarkerColor = Color.Red
            End With
        ElseIf WiliIndex = cWS03Results.eWindowsLifterTest.FrontLeft_Pull AndAlso
            (j = _WS03Results.WL_X_Indexes(WiliIndex).X_F1 OrElse j = _WS03Results.WL_X_Indexes(WiliIndex).X_F2 OrElse j = _WS03Results.WL_X_Indexes(WiliIndex).X_F4 OrElse j = _WS03Results.WL_X_Indexes(WiliIndex).X_F5) Then
            Dim text As String = Nothing
            If j = _WS03Results.WL_X_Indexes(WiliIndex).X_F1 Then
                text = "F1"
            ElseIf j = _WS03Results.WL_X_Indexes(WiliIndex).X_F2 Then
                text = "F2"
            ElseIf j = _WS03Results.WL_X_Indexes(WiliIndex).X_F4 Then
                text = "F4"
            ElseIf j = _WS03Results.WL_X_Indexes(WiliIndex).X_F5 Then
                text = "F5"
            End If
            With my_serie.Points(my_serie.Points.Count - 1)
                .Label = text
                .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                .MarkerSize = 10
                .MarkerColor = Color.Red
            End With
        ElseIf WiliIndex = cWS03Results.eWindowsLifterTest.FrontRight_Push AndAlso
            (j = _WS03Results.WL_X_Indexes(WiliIndex).X_F1 OrElse j = _WS03Results.WL_X_Indexes(WiliIndex).X_F2 OrElse j = _WS03Results.WL_X_Indexes(WiliIndex).X_F4 OrElse j = _WS03Results.WL_X_Indexes(WiliIndex).X_F5) Then
            Dim text As String = Nothing
            If j = _WS03Results.WL_X_Indexes(WiliIndex).X_F1 Then
                text = "F1"
            ElseIf j = _WS03Results.WL_X_Indexes(WiliIndex).X_F2 Then
                text = "F2"
            ElseIf j = _WS03Results.WL_X_Indexes(WiliIndex).X_F4 Then
                text = "F4"
            ElseIf j = _WS03Results.WL_X_Indexes(WiliIndex).X_F5 Then
                text = "F5"
            End If
            With my_serie.Points(my_serie.Points.Count - 1)
                .Label = text
                .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                .MarkerSize = 10
                .MarkerColor = Color.Red
            End With

        ElseIf WiliIndex = cWS03Results.eWindowsLifterTest.FrontRight_Pull AndAlso
            (j = _WS03Results.WL_X_Indexes(WiliIndex).X_F1 OrElse j = _WS03Results.WL_X_Indexes(WiliIndex).X_F2 OrElse j = _WS03Results.WL_X_Indexes(WiliIndex).X_F4 OrElse j = _WS03Results.WL_X_Indexes(WiliIndex).X_F5) Then
            Dim text As String = Nothing
            If j = _WS03Results.WL_X_Indexes(WiliIndex).X_F1 Then
                text = "F1"
            ElseIf j = _WS03Results.WL_X_Indexes(WiliIndex).X_F2 Then
                text = "F2"
            ElseIf j = _WS03Results.WL_X_Indexes(WiliIndex).X_F4 Then
                text = "F4"
            ElseIf j = _WS03Results.WL_X_Indexes(WiliIndex).X_F5 Then
                text = "F5"
            End If
            With my_serie.Points(my_serie.Points.Count - 1)
                .Label = text
                .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                .MarkerSize = 10
                .MarkerColor = Color.Red
            End With
        End If
    End Sub

    Private Sub ShowPoints_WS04(ByVal WiliIndex As cWS04Results.eWindowsLifterTest, ByVal j As Integer, ByVal my_serie As DataVisualization.Charting.Series)

        If WiliIndex = cWS04Results.eWindowsLifterTest.RearLeft_Push AndAlso
                            (j = _WS04Results.WL_X_Indexes(WiliIndex).X_F1 OrElse j = _WS04Results.WL_X_Indexes(WiliIndex).X_F2 OrElse j = _WS04Results.WL_X_Indexes(WiliIndex).X_F4 OrElse j = _WS04Results.WL_X_Indexes(WiliIndex).X_F5) Then
            Dim text As String = Nothing
            If j = _WS04Results.WL_X_Indexes(WiliIndex).X_F1 Then
                text = "F1"
            ElseIf j = _WS04Results.WL_X_Indexes(WiliIndex).X_F2 Then
                text = "F2"
            ElseIf j = _WS04Results.WL_X_Indexes(WiliIndex).X_F4 Then
                text = "F4"
            ElseIf j = _WS04Results.WL_X_Indexes(WiliIndex).X_F5 Then
                text = "F5"
            End If
            With my_serie.Points(my_serie.Points.Count - 1)
                .Label = text
                .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                .MarkerSize = 10
                .MarkerColor = Color.Red
            End With
        ElseIf WiliIndex = cWS04Results.eWindowsLifterTest.RearLeft_Pull AndAlso
            (j = _WS04Results.WL_X_Indexes(WiliIndex).X_F1 OrElse j = _WS04Results.WL_X_Indexes(WiliIndex).X_F2 OrElse j = _WS04Results.WL_X_Indexes(WiliIndex).X_F4 OrElse j = _WS04Results.WL_X_Indexes(WiliIndex).X_F5) Then
            Dim text As String = Nothing
            If j = _WS04Results.WL_X_Indexes(WiliIndex).X_F1 Then
                text = "F1"
            ElseIf j = _WS04Results.WL_X_Indexes(WiliIndex).X_F2 Then
                text = "F2"
            ElseIf j = _WS04Results.WL_X_Indexes(WiliIndex).X_F4 Then
                text = "F4"
            ElseIf j = _WS04Results.WL_X_Indexes(WiliIndex).X_F5 Then
                text = "F5"
            End If
            With my_serie.Points(my_serie.Points.Count - 1)
                .Label = text
                .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                .MarkerSize = 10
                .MarkerColor = Color.Red
            End With
        ElseIf WiliIndex = cWS04Results.eWindowsLifterTest.RearRight_Push AndAlso
            (j = _WS04Results.WL_X_Indexes(WiliIndex).X_F1 OrElse j = _WS04Results.WL_X_Indexes(WiliIndex).X_F2 OrElse j = _WS04Results.WL_X_Indexes(WiliIndex).X_F4 OrElse j = _WS04Results.WL_X_Indexes(WiliIndex).X_F5) Then
            Dim text As String = Nothing
            If j = _WS04Results.WL_X_Indexes(WiliIndex).X_F1 Then
                text = "F1"
            ElseIf j = _WS04Results.WL_X_Indexes(WiliIndex).X_F2 Then
                text = "F2"
            ElseIf j = _WS04Results.WL_X_Indexes(WiliIndex).X_F4 Then
                text = "F4"
            ElseIf j = _WS04Results.WL_X_Indexes(WiliIndex).X_F5 Then
                text = "F5"
            End If
            With my_serie.Points(my_serie.Points.Count - 1)
                .Label = text
                .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                .MarkerSize = 10
                .MarkerColor = Color.Red
            End With

        ElseIf WiliIndex = cWS04Results.eWindowsLifterTest.RearRight_Pull AndAlso
            (j = _WS04Results.WL_X_Indexes(WiliIndex).X_F1 OrElse j = _WS04Results.WL_X_Indexes(WiliIndex).X_F2 OrElse j = _WS04Results.WL_X_Indexes(WiliIndex).X_F4 OrElse j = _WS04Results.WL_X_Indexes(WiliIndex).X_F5) Then
            Dim text As String = Nothing
            If j = _WS04Results.WL_X_Indexes(WiliIndex).X_F1 Then
                text = "F1"
            ElseIf j = _WS04Results.WL_X_Indexes(WiliIndex).X_F2 Then
                text = "F2"
            ElseIf j = _WS04Results.WL_X_Indexes(WiliIndex).X_F4 Then
                text = "F4"
            ElseIf j = _WS04Results.WL_X_Indexes(WiliIndex).X_F5 Then
                text = "F5"
            End If
            With my_serie.Points(my_serie.Points.Count - 1)
                .Label = text
                .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                .MarkerSize = 10
                .MarkerColor = Color.Red
            End With
        End If
    End Sub

    Private Sub ShowPointsPush_WS03(ByVal j As Integer, ByVal my_serie As DataVisualization.Charting.Series)

        Dim text As String = Nothing
        If (j = _WS03Results.CL_X_Indexes.X_F1 OrElse j = _WS03Results.CL_X_Indexes.X_F2) Then
            If j = _WS03Results.CL_X_Indexes.X_F1 Then
                text = "F1"
            ElseIf j = _WS03Results.CL_X_Indexes.X_F2 Then
                text = "F2"
            End If
            With my_serie.Points(my_serie.Points.Count - 1)
                .Label = text
                .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                .MarkerSize = 10
                .MarkerColor = Color.Red
            End With
        End If
    End Sub

    Private Sub ShowPointsPush_WS04(ByVal j As Integer, ByVal my_serie As DataVisualization.Charting.Series)

        Dim text As String = Nothing
        If (j = _WS04Results.MF_X_Indexes.X_F1 OrElse j = _WS04Results.MF_X_Indexes.X_F2) Then
            If j = _WS04Results.MF_X_Indexes.X_F1 Then
                text = "F1"
            ElseIf j = _WS04Results.MF_X_Indexes.X_F2 Then
                text = "F2"
            End If
            With my_serie.Points(my_serie.Points.Count - 1)
                .Label = text
                .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                .MarkerSize = 10
                .MarkerColor = Color.Red
            End With
        End If
    End Sub

    Private Sub ShowPointsMirror(ByVal j As Integer, ByVal my_serie As DataVisualization.Charting.Series, ByVal MirrorPush As eMirrorPush)

        Dim text As String = Nothing
        If (j = _WS05Results.Mirror_X_Indexes(MirrorPush).X_F1 OrElse j = _WS05Results.Mirror_X_Indexes(MirrorPush).X_F2) Then
            If j = _WS05Results.Mirror_X_Indexes(MirrorPush).X_F1 Then
                text = "F1"
            ElseIf j = _WS05Results.Mirror_X_Indexes(MirrorPush).X_F2 Then
                text = "F2"
            End If
            With my_serie.Points(my_serie.Points.Count - 1)
                .Label = text
                .MarkerStyle = DataVisualization.Charting.MarkerStyle.Cross
                .MarkerSize = 10
                .MarkerColor = Color.Red
            End With
        End If
    End Sub

    Private Sub dgv_CellPainting(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellPaintingEventArgs) Handles dgvWS02GeneralInformation.CellPainting, dgvWS02PowerUp.CellPainting,
                                                                                                                                    dgvWS02ValeoSerialNumber.CellPainting, dgvWS02EMS.CellPainting,
                                                                                                                                     dgvWS02Backlight_CL.CellPainting,
                                                                                                                                    dgvWS02Backlight_FL.CellPainting, dgvWS02Backlight_MA.CellPainting,
                                                                                                                                    dgvWS02Backlight_MS.CellPainting, dgvWS02Backlight_WLF.CellPainting,
                                                                                                                                    dgvWS02Backlight_WLR.CellPainting, dgvWS02TellTale.CellPainting,
                                                                                                                                    dgvWS02MMS.CellPainting, dgvWS02Shape.CellPainting,
                                                                                                                                    dgvWS02ValeoSerialNumber.CellPainting,
                                                                                                                                    dgvWS03FrontLeftDN.CellPainting, dgvWS03FrontLeftUP.CellPainting, dgvWS03FrontRightDN.CellPainting,
                                                                                                                                    dgvWS03FrontRightUP.CellPainting, dgvWS03GeneralInformation.CellPainting, dgvWS03PowerUp.CellPainting,
                                                                                                                                    dgvWS03SerialNumber.CellPainting, dgvWS03ChildrenLock.CellPainting,
                                                                                                                                    dgvWS04RearLeftDN.CellPainting, dgvWS04RearLeftUP.CellPainting, dgvWS04RearRightDN.CellPainting,
                                                                                                                                    dgvWS04RearRightUP.CellPainting, dgvWS04GeneralInformation.CellPainting, dgvWS04PowerUp.CellPainting,
                                                                                                                                    dgvWS04SerialNumber.CellPainting, dgvWS04MirrorFolding.CellPainting,
                                                                                                                                    dgvWS05MirrorUP.CellPainting, dgvWS05MirrorDN.CellPainting, dgvWS05MirrorRight.CellPainting,
                                                                                                                                    dgvWS05MirrorLeft.CellPainting, dgvWS05MirrorSR.CellPainting, dgvWS05MirrorSL.CellPainting,
                                                                                                                                    dgvWS05GeneralInformation.CellPainting, dgvWS05PowerUp.CellPainting, dgvWS05SerialNumber.CellPainting,
                                                                                                                                    dgvWS02FrontPassengerDN.CellPainting, dgvWS02FrontPassengerUP.CellPainting,
                                                                                                                                    dgvWS02RBT_RTRV_D_Fold.CellPainting,
                                                                                                                                    dgvWS02RBT_RTRV_G_Fold.CellPainting,
                                                                                                                                    dgvWS02RearLeftDN.CellPainting, dgvWS02RearLeftUP.CellPainting,
                                                                                                                                    dgvWS02RearRightDN.CellPainting, dgvWS02RearRightUP.CellPainting,
                                                                                                                                    dgvWS02RTRV_DG_D.CellPainting, dgvWS02RTRV_HB_D.CellPainting,
                                                                                                                                    dgvWS02RTRV_HB_G.CellPainting, dgvWS02RTRV_DG_G.CellPainting,
                                                                                                                                    dgvWS02SwitchInhibition.CellPainting, dgvWS02Analog.CellPainting,
                                                                                                                                    dgvWS02SwitchInhibition.CellPainting, dgvWS02PWM_WLAP.CellPainting,
                                                                                                                                    dgvWS02PWM_DOORLock.CellPainting, dgvWS02Shape2.CellPainting




        Try
            If (sender.item(0, e.RowIndex).value Is Nothing) AndAlso (e.ColumnIndex >= 0 And e.ColumnIndex <= sender.columncount - 1) Then
                Using gridBrush As Brush = New SolidBrush(Color.Blue), backColorBrush As Brush = New SolidBrush(Color.Blue)
                    Using gridLinePen As Pen = New Pen(gridBrush)
                        ' Clear cell 
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds)
                        'Bottom line drawing
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1)
                        'top line drawing
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Top)
                        'Drawing Right line
                        If e.ColumnIndex = 2 Then
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom)
                        End If
                        'Inserting text
                        If e.ColumnIndex = 2 Then
                            e.Graphics.DrawString(CType(e.Value, String), e.CellStyle.Font, Brushes.White, (sender.width / 2) - (e.CellBounds.X / 4), e.CellBounds.Y + 5)
                        End If
                        e.Handled = True
                    End Using
                End Using
            End If
        Catch ex As Exception
            ex = ex
        End Try

    End Sub


    Private Sub Screen_Shot_WS03_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_Shot_WS03.Click
        Dim basepath As String = Nothing
        Dim filename As String = Nothing
        Dim fullpath As String = Nothing
        Dim my_graph As DataVisualization.Charting.Chart = Nothing
        Dim WindowsLifterTest As cWS03Results.eWindowsLifterTest
        Dim i As Integer
        Try
            For i = 0 To 4
                If i = 0 Then
                    filename = "FRONT_" + "LEFT_" + "Pull_"
                    my_graph = chWS03FLUPElectricalTest
                    WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Pull
                ElseIf i = 1 Then
                    filename = "FRONT_" + "LEFT_" + "Push_"
                    my_graph = chWS03FLDNElectricalTest
                    WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontLeft_Push
                ElseIf i = 2 Then
                    filename = "FRONT_" + "RIGHT_" + "Pull_"
                    my_graph = chWS03FRUPElectricalTest
                    WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Pull
                ElseIf i = 3 Then
                    filename = "FRONT_" + "RIGHT_" + "Push_"
                    my_graph = chWS03RRDNElectricalTest
                    WindowsLifterTest = cWS03Results.eWindowsLifterTest.FrontRight_Push
                Else
                    filename = "Children_Lock"
                    my_graph = chWS03ChildrenElectricalTest
                End If
                'construct directory name
                basepath = Path.Combine(Path.GetFullPath(mConstants.BasePath & mWS03Main.Settings.ResultsPath), "ScreenShot", Format(Date.Now, "yyyyMMdd"))
                ' Create the record directory if required
                If Not (Directory.Exists(basepath)) Then
                    Directory.CreateDirectory(basepath)
                End If

                filename += Mid(mWS03Main.Results.TestDate.Value.ToString, 7, 4) + Mid(mWS03Main.Results.TestDate.Value.ToString, 4, 2) + Mid(mWS03Main.Results.TestDate.Value.ToString, 1, 2)
                filename += "_" + mWS03Main.Results.PartUniqueNumber.Value.ToString
                filename = Path.ChangeExtension(filename, "png")
                fullpath = Path.Combine(basepath, filename)

                my_graph.SaveImage(fullpath, System.Drawing.Imaging.ImageFormat.Png)
                If (Directory.Exists(Path.Combine(mResults.FTPLocalFilePath + "ManualSave\", Format(Date.Now, "HHmmss") + "_" + filename)) = False) Then
                    Try
                        Directory.CreateDirectory(Path.Combine(mResults.FTPLocalFilePath + "ManualSave\", Format(Date.Now, "HHmmss") + "_" + filename))
                    Catch ex As Exception

                    End Try
                End If
                Try
                    my_graph.SaveImage(Path.Combine(mResults.FTPLocalFilePath + "ManualSave\", Format(Date.Now, "HHmmss") + "_" + filename), System.Drawing.Imaging.ImageFormat.Png) 'for CDA Guangzhou.
                Catch ex As Exception

                End Try
                'save points
                Dim fileWriter As StreamWriter = Nothing
                filename = Path.ChangeExtension(filename, "txt")
                fullpath = Path.Combine(basepath, filename)
                fileWriter = New StreamWriter(fullpath)
                ' Store the samples
                Dim WriteLine As String
                ' Store the samples
                If i <= 3 Then
                    For sampleIndex = 0 To mWS03Main.Results.WS03_SampleCount(WindowsLifterTest) - 1
                        WriteLine = (mWS03Main.Results.Samples_Push_Pull(WindowsLifterTest, mWS03Main.cSample_Signal.EarlySensor, sampleIndex) & vbTab &
                                    mWS03Main.Results.Samples_Push_Pull(WindowsLifterTest, mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) & vbTab &
                                    mWS03Main.Results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex) & vbTab &
                                    mWS03Main.Results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex) & vbTab &
                                    mWS03Main.Results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex) & vbTab &
                                    mWS03Main.Results.Samples_Push_Pull(WindowsLifterTest, cWS03Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex))
                        fileWriter.WriteLine(WriteLine)
                    Next
                Else
                    For sampleIndex = 0 To mWS03Main.Results.WS03_SampleCountPush - 1
                        WriteLine = (mWS03Main.Results.WS03_SamplePush(mWS03Main.cSample_Signal.EarlySensor, sampleIndex) & vbTab & _
                                    mWS03Main.Results.WS03_SamplePush(mWS03Main.cSample_Signal.StrenghtSensor, sampleIndex) & vbTab & _
                                    mWS03Main.Results.WS03_SamplePush(2, sampleIndex))
                        fileWriter.WriteLine(WriteLine)
                    Next
                End If


                fileWriter.Close()
            Next i

            frmMessage.MessageType = frmMessage.eType.Information
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Save all Screen Shot Done"
            frmMessage.ShowDialog()


        Catch ex As Exception
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error during Saving , " & ex.ToString
            frmMessage.ShowDialog()
        End Try

    End Sub

    Private Sub Screen_Shot_WS04_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_Shot_WS04.Click
        Dim basepath As String = Nothing
        Dim filename As String = Nothing
        Dim fullpath As String = Nothing
        Dim my_graph As DataVisualization.Charting.Chart = Nothing
        Dim WindowsLifterTest As cWS04Results.eWindowsLifterTest
        Dim i As Integer
        Try
            For i = 0 To 4
                If i = 0 Then
                    filename = "REAR_" + "LEFT_" + "Pull_"
                    my_graph = chWS04RLUPElectricalTest
                    WindowsLifterTest = cWS04Results.eWindowsLifterTest.RearLeft_Pull
                ElseIf i = 1 Then
                    filename = "REAR_" + "LEFT_" + "Push_"
                    my_graph = chWS04RLDNElectricalTest
                    WindowsLifterTest = cWS04Results.eWindowsLifterTest.RearLeft_Push
                ElseIf i = 2 Then
                    filename = "REAR_" + "RIGHT_" + "Pull_"
                    my_graph = chWS04RRUPElectricalTest
                    WindowsLifterTest = cWS04Results.eWindowsLifterTest.RearRight_Pull
                ElseIf i = 3 Then
                    filename = "REAR_" + "RIGHT_" + "Push_"
                    my_graph = chWS04RRDNElectricalTest
                    WindowsLifterTest = cWS04Results.eWindowsLifterTest.RearRight_Push
                Else
                    filename = "Mirror Folding"
                    my_graph = chWS04MFElectricalTest
                End If
                'construct directory name
                basepath = Path.Combine(Path.GetFullPath(mConstants.BasePath & mWS04Main.Settings.ResultsPath), "ScreenShot", Format(Date.Now, "yyyyMMdd"))
                ' Create the record directory if required
                If Not (Directory.Exists(basepath)) Then
                    Directory.CreateDirectory(basepath)
                End If

                filename += Mid(mWS04Main.Results.TestDate.Value.ToString, 7, 4) + Mid(mWS04Main.Results.TestDate.Value.ToString, 4, 2) + Mid(mWS04Main.Results.TestDate.Value.ToString, 1, 2)
                filename += "_" + mWS04Main.Results.PartUniqueNumber.Value.ToString
                filename = Path.ChangeExtension(filename, "png")
                fullpath = Path.Combine(basepath, filename)

                my_graph.SaveImage(fullpath, System.Drawing.Imaging.ImageFormat.Png)
                If (Directory.Exists(mResults.FTPLocalFilePath + "ManualSave\") = False) Then
                    Try
                        Directory.CreateDirectory(mResults.FTPLocalFilePath + "ManualSave\")
                    Catch ex As Exception

                    End Try
                End If
                Try
                    my_graph.SaveImage(Path.Combine(mResults.FTPLocalFilePath + "ManualSave\", Format(Date.Now, "HHmmss") + "_" + filename), System.Drawing.Imaging.ImageFormat.Png) 'for CDA Guangzhou.
                Catch ex As Exception

                End Try
                'save points
                Dim fileWriter As StreamWriter = Nothing
                filename = Path.ChangeExtension(filename, "txt")
                fullpath = Path.Combine(basepath, filename)
                fileWriter = New StreamWriter(fullpath)
                ' Store the samples
                Dim WriteLine As String
                ' Store the samples
                If i <= 3 Then
                    For sampleIndex = 0 To mWS04Main.Results.WS04_SampleCount(WindowsLifterTest) - 1
                        WriteLine = (mWS04Main.Results.Samples_Push_Pull(WindowsLifterTest, mWS04Main.cSample_Signal.EarlySensor, sampleIndex) & vbTab &
                                    mWS04Main.Results.Samples_Push_Pull(WindowsLifterTest, mWS04Main.cSample_Signal.StrenghtSensor, sampleIndex) & vbTab &
                                    mWS04Main.Results.Samples_Push_Pull(WindowsLifterTest, cWS04Results.eWindowsLifterSignal.Push_Manual + 2, sampleIndex) & vbTab &
                                    mWS04Main.Results.Samples_Push_Pull(WindowsLifterTest, cWS04Results.eWindowsLifterSignal.Push_Automtic + 2, sampleIndex) & vbTab &
                                    mWS04Main.Results.Samples_Push_Pull(WindowsLifterTest, cWS04Results.eWindowsLifterSignal.Pull_Manual + 2, sampleIndex) & vbTab &
                                    mWS04Main.Results.Samples_Push_Pull(WindowsLifterTest, cWS04Results.eWindowsLifterSignal.Pull_Automtic + 2, sampleIndex))
                        fileWriter.WriteLine(WriteLine)
                    Next
                Else
                    For sampleIndex = 0 To mWS04Main.Results.WS04_SampleCountPush - 1
                        WriteLine = (mWS04Main.Results.WS04_SamplePush(mWS04Main.cSample_Signal.EarlySensor, sampleIndex) & vbTab & _
                                    mWS04Main.Results.WS04_SamplePush(mWS04Main.cSample_Signal.StrenghtSensor, sampleIndex) & vbTab & _
                                    mWS04Main.Results.WS04_SamplePush(2, sampleIndex))
                        fileWriter.WriteLine(WriteLine)
                    Next
                End If


                fileWriter.Close()
            Next i

            frmMessage.MessageType = frmMessage.eType.Information
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Save all Screen Shot Done"
            frmMessage.ShowDialog()


        Catch ex As Exception
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error during Saving , " & ex.ToString
            frmMessage.ShowDialog()
        End Try


    End Sub

    Private Sub Screen_Shot_WS05_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Screen_Shot_WS05.Click
        Dim basepath As String = Nothing
        Dim filename As String = Nothing
        Dim fullpath As String = Nothing
        Dim my_graph As DataVisualization.Charting.Chart = Nothing
        Dim PushTest As cWS05Results.eMirrorPushTest
        Dim i As Integer
        Try
            For i = 0 To 4
                If i = 0 Then
                    filename = "Mirror Adj Down"
                    my_graph = chWS05MDNElectricalTest
                    PushTest = cWS05Results.eMirrorPushTest.Mirror_DN
                ElseIf i = 1 Then
                    filename = "Mirror Adj Left"
                    my_graph = chWS05MLElectricalTest
                    PushTest = cWS05Results.eMirrorPushTest.Mirror_ML
                ElseIf i = 2 Then
                    filename = "Mirror Adj Right"
                    my_graph = chWS05MRElectricalTest
                    PushTest = cWS05Results.eMirrorPushTest.Mirror_MR
                ElseIf i = 3 Then
                    filename = "Mirror UP"
                    my_graph = chWS05MUPElectricalTest
                    PushTest = cWS05Results.eMirrorPushTest.Mirror_UP
                ElseIf i = 4 Then
                    filename = "Mirror Select Left"
                    my_graph = chWS05MSLElectricalTest
                    PushTest = cWS05Results.eMirrorPushTest.Mirror_SL
                Else
                    filename = "Mirror Select Right"
                    my_graph = chWS05MSRElectricalTest
                    PushTest = cWS05Results.eMirrorPushTest.Mirror_SR
                End If
                'construct directory name
                basepath = Path.Combine(Path.GetFullPath(mConstants.BasePath & mWS05Main.Settings.ResultsPath), "ScreenShot", Format(Date.Now, "yyyyMMdd"))
                ' Create the record directory if required
                If Not (Directory.Exists(basepath)) Then
                    Directory.CreateDirectory(basepath)
                End If

                filename += Mid(mWS05Main.Results.TestDate.Value.ToString, 7, 4) + Mid(mWS05Main.Results.TestDate.Value.ToString, 4, 2) + Mid(mWS05Main.Results.TestDate.Value.ToString, 1, 2)
                filename += "_" + mWS05Main.Results.PartUniqueNumber.Value.ToString
                filename = Path.ChangeExtension(filename, "png")
                fullpath = Path.Combine(basepath, filename)

                my_graph.SaveImage(fullpath, System.Drawing.Imaging.ImageFormat.Png)
                If (Directory.Exists(Path.Combine(mResults.FTPLocalFilePath + "ManualSave\", Format(Date.Now, "HHmmss") + "_" + filename)) = False) Then
                    Try
                        Directory.CreateDirectory(Path.Combine(mResults.FTPLocalFilePath + "ManualSave\", Format(Date.Now, "HHmmss") + "_" + filename))
                    Catch ex As Exception

                    End Try
                End If
                Try
                    my_graph.SaveImage(Path.Combine(mResults.FTPLocalFilePath + "ManualSave\", Format(Date.Now, "HHmmss") + "_" + filename), System.Drawing.Imaging.ImageFormat.Png) 'for CDA Guangzhou.
                Catch ex As Exception

                End Try
                'save points
                Dim fileWriter As StreamWriter = Nothing
                filename = Path.ChangeExtension(filename, "txt")
                fullpath = Path.Combine(basepath, filename)
                fileWriter = New StreamWriter(fullpath)
                ' Store the samples
                Dim WriteLine As String
                ' Store the samples
                For sampleIndex = 0 To mWS05Main.Results.SampleCount(PushTest) - 1
                    WriteLine = (mWS05Main.Results.Sample(mWS05Main.cSample_Signal.EarlySensor, PushTest, sampleIndex) & vbTab & _
                                mWS05Main.Results.Sample(mWS05Main.cSample_Signal.StrenghtSensor, PushTest, sampleIndex) & vbTab & _
                                mWS05Main.Results.Sample(PushTest + 2, PushTest, sampleIndex))
                    fileWriter.WriteLine(WriteLine)
                Next
                

                fileWriter.Close()
            Next i

            frmMessage.MessageType = frmMessage.eType.Information
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Save all Screen Shot Done"
            frmMessage.ShowDialog()


        Catch ex As Exception
            frmMessage.MessageType = frmMessage.eType.Critical
            frmMessage.MessageButtons = frmMessage.eButtons.OkOnly
            frmMessage.Message = "Error during Saving , " & ex.ToString
            frmMessage.ShowDialog()
        End Try
    End Sub
End Class