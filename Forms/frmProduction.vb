Option Explicit On
Option Strict On

Public Class frmProduction
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+
    ' Private variables

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


    Private Sub dgvSingleTestResults_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dgvWS02SingleTestResults.SelectionChanged,
                                                                                                                   dgvWS03SingleTestResults.SelectionChanged,
                                                                                                                   dgvWS04SingleTestResults.SelectionChanged,
                                                                                                                   dgvWS05SingleTestResults.SelectionChanged
        Dim table As DataGridView

        ' Clear the selection
        table = CType(sender, DataGridView)
        table.ClearSelection()
    End Sub



    Private Sub frmProduction_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' Disable the timer monitor
        tmrMonitor.Enabled = False
        '
        tmrLoop.Enabled = False
        ' Close the barrette results form
        frmResults.Close()
        ' Close the alarms form
        frmAlarms.Close()
    End Sub



    Private Sub frmProduction_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Initialize the controls related to station FW
        InitializeStationWS02()
        InitializeStationWS03()
        InitializeStationWS04()
        InitializeStationWS05()

        ' Set the reload recipe bits


        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RecipeLoaded)
        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS02_Alarm)
        mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS02_RelaodRecipe)

        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_RecipeLoaded)
        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03_Alarm)
        mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03_RelaodRecipe)

        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS04_RecipeLoaded)
        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS04_Alarm)
        mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS04_RelaodRecipe)

        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_RecipeLoaded)
        mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS05_Alarm)
        mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS05_RelaodRecipe)


        ' Load and hide the results form
        frmResults.Show()
        frmRecipes.Visible = False
        ' Load and hide the alarms form
        frmAlarms.Show()
        frmAlarms.Visible = False
        ' Initialize the status bar
        mStatusBar.Initialize(ssStatusBar)
        ' Configurate and enable the timer monitor
        tmrMonitor.Interval = 100
        tmrMonitor.Enabled = True
        '
        tmrLoop.Enabled = True
    End Sub




    Private Sub InitializeStationWS02()
        ' Initialize the table of single test results
        With dgvWS02SingleTestResults
            ' Disable the refresh
            .SuspendLayout()
            ' Configurate the rows
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .RowHeadersVisible = False
            ' If the reference type is barrette
            ' Set the row count
            .RowCount = cWS02Results.SingleTestCount + 1
            ' Set the row height
            For i = 0 To .RowCount - 1
                .Rows(i).Height = 25
            Next
            ' Set the table height
            .Height = 25 * .RowCount
            ' Configurate the columns
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .ColumnHeadersVisible = False
            .ColumnCount = 2
            ' Configurate the scrollbars and the column width
            .ScrollBars = ScrollBars.None
            .Columns(0).Width = CInt(0.8 * .Width)
            .Columns(1).Width = CInt(0.2 * .Width)
            ' Configurate the cell styles
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .DefaultCellStyle.Font = New Font("Arial", 12)
            .Item(0, 0).Style.BackColor = Color.Blue
            .Item(0, 0).Style.ForeColor = Color.White
            .Item(1, 0).Style.BackColor = Color.Blue
            .Item(1, 0).Style.ForeColor = Color.White
            ' Print the column headers
            .Item(0, 0).Value = "Test description"
            .Item(1, 0).Value = "Result"
            ' Print the single test descriptions
            For i = 1 To .RowCount - 1
                .Item(0, i).Value = mWS02Main.Results.Value(cWS02Results.SingleTestBaseIndex + i - 1).Description
            Next
            ' Configurate the edit mode
            .EditMode = DataGridViewEditMode.EditProgrammatically
            ' Disable multiple selection
            .MultiSelect = False
            ' Enable the refresh
            .RefreshEdit()
            .ResumeLayout(True)
        End With

    End Sub

    Private Sub InitializeStationWS03()
        ' Initialize the table of single test results
        With dgvWS03SingleTestResults
            ' Disable the refresh
            .SuspendLayout()
            ' Configurate the rows
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .RowHeadersVisible = False
            ' If the reference type is barrette
            ' Set the row count
            .RowCount = cWS03Results.SingleTestCount + 1
            ' Set the row height
            For i = 0 To .RowCount - 1
                .Rows(i).Height = 25
            Next
            ' Set the table height
            .Height = 25 * .RowCount
            ' Configurate the columns
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .ColumnHeadersVisible = False
            .ColumnCount = 2
            ' Configurate the scrollbars and the column width
            .ScrollBars = ScrollBars.None
            .Columns(0).Width = CInt(0.8 * .Width)
            .Columns(1).Width = CInt(0.2 * .Width)
            ' Configurate the cell styles
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .DefaultCellStyle.Font = New Font("Arial", 12)
            .Item(0, 0).Style.BackColor = Color.Blue
            .Item(0, 0).Style.ForeColor = Color.White
            .Item(1, 0).Style.BackColor = Color.Blue
            .Item(1, 0).Style.ForeColor = Color.White
            ' Print the column headers
            .Item(0, 0).Value = "Test description"
            .Item(1, 0).Value = "Result"
            ' Print the single test descriptions
            For i = 1 To .RowCount - 1
                .Item(0, i).Value = mWS03Main.Results.Value(cWS03Results.SingleTestBaseIndex + i - 1).Description
            Next
            ' Configurate the edit mode
            .EditMode = DataGridViewEditMode.EditProgrammatically
            ' Disable multiple selection
            .MultiSelect = False
            ' Enable the refresh
            .RefreshEdit()
            .ResumeLayout(True)
        End With

    End Sub

    Private Sub InitializeStationWS04()
        ' Initialize the table of single test results
        With dgvWS04SingleTestResults
            ' Disable the refresh
            .SuspendLayout()
            ' Configurate the rows
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .RowHeadersVisible = False
            ' If the reference type is barrette
            ' Set the row count
            .RowCount = cWS04Results.SingleTestCount + 1
            ' Set the row height
            For i = 0 To .RowCount - 1
                .Rows(i).Height = 25
            Next
            ' Set the table height
            .Height = 25 * .RowCount
            ' Configurate the columns
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .ColumnHeadersVisible = False
            .ColumnCount = 2
            ' Configurate the scrollbars and the column width
            .ScrollBars = ScrollBars.None
            .Columns(0).Width = CInt(0.8 * .Width)
            .Columns(1).Width = CInt(0.2 * .Width)
            ' Configurate the cell styles
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .DefaultCellStyle.Font = New Font("Arial", 12)
            .Item(0, 0).Style.BackColor = Color.Blue
            .Item(0, 0).Style.ForeColor = Color.White
            .Item(1, 0).Style.BackColor = Color.Blue
            .Item(1, 0).Style.ForeColor = Color.White
            ' Print the column headers
            .Item(0, 0).Value = "Test description"
            .Item(1, 0).Value = "Result"
            ' Print the single test descriptions
            For i = 1 To .RowCount - 1
                .Item(0, i).Value = mWS04Main.Results.Value(cWS04Results.SingleTestBaseIndex + i - 1).Description
            Next
            ' Configurate the edit mode
            .EditMode = DataGridViewEditMode.EditProgrammatically
            ' Disable multiple selection
            .MultiSelect = False
            ' Enable the refresh
            .RefreshEdit()
            .ResumeLayout(True)
        End With

    End Sub

    Private Sub InitializeStationWS05()
        ' Initialize the table of single test results
        With dgvWS05SingleTestResults
            ' Disable the refresh
            .SuspendLayout()
            ' Configurate the rows
            .AllowUserToAddRows = False
            .AllowUserToDeleteRows = False
            .AllowUserToResizeRows = False
            .RowHeadersVisible = False
            ' If the reference type is barrette
            ' Set the row count
            .RowCount = cWS05Results.SingleTestCount + 1
            ' Set the row height
            For i = 0 To .RowCount - 1
                .Rows(i).Height = 25
            Next
            ' Set the table height
            .Height = 25 * .RowCount
            ' Configurate the columns
            .AllowUserToOrderColumns = False
            .AllowUserToResizeColumns = False
            .ColumnHeadersVisible = False
            .ColumnCount = 2
            ' Configurate the scrollbars and the column width
            .ScrollBars = ScrollBars.None
            .Columns(0).Width = CInt(0.8 * .Width)
            .Columns(1).Width = CInt(0.2 * .Width)
            ' Configurate the cell styles
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter
            .DefaultCellStyle.Font = New Font("Arial", 12)
            .Item(0, 0).Style.BackColor = Color.Blue
            .Item(0, 0).Style.ForeColor = Color.White
            .Item(1, 0).Style.BackColor = Color.Blue
            .Item(1, 0).Style.ForeColor = Color.White
            ' Print the column headers
            .Item(0, 0).Value = "Test description"
            .Item(1, 0).Value = "Result"
            ' Print the single test descriptions
            For i = 1 To .RowCount - 1
                .Item(0, i).Value = mWS05Main.Results.Value(cWS05Results.SingleTestBaseIndex + i - 1).Description
            Next
            ' Configurate the edit mode
            .EditMode = DataGridViewEditMode.EditProgrammatically
            ' Disable multiple selection
            .MultiSelect = False
            ' Enable the refresh
            .RefreshEdit()
            .ResumeLayout(True)
        End With

    End Sub


    Private Sub pbLogoValeo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles pbLogoValeo.Click
        '
        mWS04_05AIOManager.StopSampling()
        '
        mUserManager.Logout()
        '
        fmrTestLoop.Close()
        ' Close the form
        Me.Close()
    End Sub



    Private Sub tmrMonitor_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrMonitor.Tick
        Dim alarm As Boolean
        Dim j As Integer
        Dim i As Integer
        Static blink As Boolean
        'Static WS02OldPhase As mWS02Main.ePhase
        'Static WS03OldPhase As mWS03Main.ePhase
        'Static WS04OldPhase As mWS04Main.ePhase
        'Static WS05OldPhase As mWS05Main.ePhase
        Static t0Blink As Date


        ' Disable the timer
        tmrMonitor.Enabled = False

        btnWS03AbortTest.Visible = Simulation_Test
        btnWS03StartTest.Visible = Simulation_Test
        btnWS04AbortTest.Visible = Simulation_Test
        btnWS04StartTest.Visible = Simulation_Test
        btnWS05AbortTest.Visible = Simulation_Test
        btnWS05StartTest.Visible = Simulation_Test

        'After delay Logout user
        If ((Date.Now - mGlobal.tUser).TotalMilliseconds >= 30000) And
            mUserManager.CurrentAccessLevel <> 0 Then
            mUserManager.Logout()
        End If

        ' Generate the blink flag
        If ((Date.Now - t0Blink).TotalSeconds > 0.5) Then
            blink = Not (blink)
            t0Blink = Date.Now
        End If

        If ((mWS02Main.ForcePartOk Or mWS02Main.ForcePartTypeOk Or
             mWS03Main.ForcePartOk Or mWS03Main.ForcePartTypeOk Or
             mWS04Main.ForcePartOk Or mWS04Main.ForcePartTypeOk Or
             mWS05Main.ForcePartOk Or mWS05Main.ForcePartTypeOk) And
            blink) Then
            lblSoftwareTitle.BackColor = Color.Red
            lblFormTitle.BackColor = Color.Red
        Else
            lblSoftwareTitle.BackColor = Color.Cyan
            lblFormTitle.BackColor = Color.Cyan
        End If

        '************************************************************************
        ' Work Staiton 02 Camera and Electric
        '************************************************************************
        ' Show the station WS02 test mode
        lblWS02TestMode.Text = mWS02Main.TestModeDescription(mWS02Main.TestMode)
        ' Show the station WS02 test phase
        lblWS02Phase.Text = CInt(mWS02Main.Phase).ToString & "." & mWS02Main.SubPhase(mWS02Main.Phase).ToString & " - " & mWS02Main.PhaseDescription(mWS02Main.Phase)
        ' Show the station WS02 reference
        lblWS02Reference.Text = mWS02Main.Reference
        If (CType(mWS02Ethernet.InputValue(mWS02Ethernet.eInput.Test_Mode), mWS02Main.eTestMode) = mWS02Main.eTestMode.Analyse) Then
            BtnWS02StartPLC.Visible = True
        Else
            BtnWS02StartPLC.Visible = False
        End If

        ' Show btn Step
        If mWS02Main.StepByStep = False Then
            btnWS02StepMode.BackColor = Color.Yellow
        Else
            btnWS02StepMode.BackColor = Color.LawnGreen
        End If
        btnWS02Step.Visible = mWS02Main.StepByStep

        ' Show the station WS02 test results
        lblWS02TestResult.Text = CInt(mWS02Main.Results.TestResult).ToString & " = " & cWS02Results.TestResultDescription(mWS02Main.Results.TestResult)
        If (mWS02Main.Results.TestResult = cWS02Results.eTestResult.Unknown) Then
            lblWS02TestResult.BackColor = Color.White
        ElseIf (mWS02Main.Results.TestResult = cWS02Results.eTestResult.Passed) Then
            lblWS02TestResult.BackColor = Color.LawnGreen
        Else
            lblWS02TestResult.BackColor = Color.Red
        End If

        'Show last test
        lblWS02TPCycle.Text = Format(mWS02Main.TestLast, "0.00") & " s"

        ' Show the station WS02 single test results
        With dgvWS02SingleTestResults
            For i = 1 To .RowCount - 1
                If (mWS02Main.Results.Value(cWS02Results.SingleTestBaseIndex + i - 1).TestResult = cWS02Results.eSingleTestResult.Disabled) Then
                    .Item(1, i).Value = "---"
                    .Item(1, i).Style.BackColor = Color.DarkGray
                ElseIf (mWS02Main.Results.Value(cWS02Results.SingleTestBaseIndex + i - 1).TestResult = cWS02Results.eSingleTestResult.Unknown) Then
                    .Item(1, i).Value = "???"
                    .Item(1, i).Style.BackColor = Color.Yellow
                ElseIf (mWS02Main.Results.Value(cWS02Results.SingleTestBaseIndex + i - 1).TestResult = cWS02Results.eSingleTestResult.Passed) Then
                    .Item(1, i).Value = "OK"
                    .Item(1, i).Style.BackColor = Color.LawnGreen
                ElseIf (mWS02Main.Results.Value(cWS02Results.SingleTestBaseIndex + i - 1).TestResult = cWS02Results.eSingleTestResult.Failed) Then
                    .Item(1, i).Value = "KO"
                    .Item(1, i).Style.BackColor = Color.Red
                Else
                    .Item(1, i).Value = "NC"
                    .Item(1, i).Style.BackColor = Color.Magenta
                End If
            Next i
        End With

        ' Show the Station WS02 I/O status
        lblWS02TestEnable.BackColor = CType(IIf(mWS02Main.TestEnableInputStatus, Color.Yellow, Color.White), Color)
        lblWS02StepInProgress.BackColor = CType(IIf(mWS02Main.StepInProgressInputStatus, Color.Yellow, Color.White), Color)
        lblWS02TestOkA.BackColor = CType(IIf(mWS02Main.TestOkOutputStatus, Color.LawnGreen, Color.White), Color)
        lblWS02StartStep.BackColor = CType(IIf(mWS02Main.StartStepOutputStatus, Color.Yellow, Color.White), Color)


        '************************************************************************
        ' Work Staiton 03 Haptic Rear Windows Lifter
        '************************************************************************
        ' Show the station WS03 test mode
        lblWS03TestMode.Text = mWS03Main.TestModeDescription(mWS03Main.TestMode)
        ' Show the station WS03 test phase
        lblWS03Phase.Text = CInt(mWS03Main.Phase).ToString & "." & mWS03Main.SubPhase(mWS03Main.Phase).ToString & " - " & mWS03Main.PhaseDescription(mWS03Main.Phase)
        ' Show the station WS03 reference
        lblWS03Reference.Text = mWS03Main.Reference
        If (CType(mWS03Ethernet.InputValue(mWS03Ethernet.eInput.Test_Mode), mWS03Main.eTestMode) = mWS03Main.eTestMode.Analyse) Then
            BtnWS03StartPLC.Visible = True
        Else
            BtnWS03StartPLC.Visible = False
        End If

        ' Show btn Step
        If mWS03Main.StepByStep = False Then
            btnWS03StepMode.BackColor = Color.Yellow
        Else
            btnWS03StepMode.BackColor = Color.LawnGreen
        End If
        btnWS03Step.Visible = mWS03Main.StepByStep

        ' Show the station WS03 test results
        lblWS03TestResult.Text = CInt(mWS03Main.Results.TestResult).ToString & " = " & cWS03Results.TestResultDescription(mWS03Main.Results.TestResult)
        If (mWS03Main.Results.TestResult = cWS03Results.eTestResult.Unknown) Then
            lblWS03TestResult.BackColor = Color.White
        ElseIf (mWS03Main.Results.TestResult = cWS03Results.eTestResult.Passed) Then
            lblWS03TestResult.BackColor = Color.LawnGreen
        Else
            lblWS03TestResult.BackColor = Color.Red
        End If

        'Show last test
        lblWS03TPCycle.Text = Format(mWS03Main.TestLast, "0.00") & " s"

        ' Show the station WS03 single test results
        With dgvWS03SingleTestResults
            For i = 1 To .RowCount - 1
                If (mWS03Main.Results.Value(cWS03Results.SingleTestBaseIndex + i - 1).TestResult = cWS03Results.eSingleTestResult.Disabled) Then
                    .Item(1, i).Value = "---"
                    .Item(1, i).Style.BackColor = Color.DarkGray
                ElseIf (mWS03Main.Results.Value(cWS03Results.SingleTestBaseIndex + i - 1).TestResult = cWS03Results.eSingleTestResult.Unknown) Then
                    .Item(1, i).Value = "???"
                    .Item(1, i).Style.BackColor = Color.Yellow
                ElseIf (mWS03Main.Results.Value(cWS03Results.SingleTestBaseIndex + i - 1).TestResult = cWS03Results.eSingleTestResult.Passed) Then
                    .Item(1, i).Value = "OK"
                    .Item(1, i).Style.BackColor = Color.LawnGreen
                ElseIf (mWS03Main.Results.Value(cWS03Results.SingleTestBaseIndex + i - 1).TestResult = cWS03Results.eSingleTestResult.Failed) Then
                    .Item(1, i).Value = "KO"
                    .Item(1, i).Style.BackColor = Color.Red
                Else
                    .Item(1, i).Value = "NC"
                    .Item(1, i).Style.BackColor = Color.Magenta
                End If
            Next i
        End With

        ' Show the Station WS03 I/O status
        lblWS03TestEnable.BackColor = CType(IIf(mWS03Main.TestEnableInputStatus, Color.Yellow, Color.White), Color)
        lblWS03StepInProgress.BackColor = CType(IIf(mWS03Main.StepInProgressInputStatus, Color.Yellow, Color.White), Color)
        lblWS03TestOk.BackColor = CType(IIf(mWS03Main.TestOkOutputStatus, Color.LawnGreen, Color.White), Color)
        lblWS03StartStep.BackColor = CType(IIf(mWS03Main.StartStepOutputStatus, Color.Yellow, Color.White), Color)

        '************************************************************************
        ' Work Staiton 04 Haptic Front Windows Lifter
        '************************************************************************
        ' Show the station WS04 test mode
        lblWS04TestMode.Text = mWS04Main.TestModeDescription(mWS04Main.TestMode)
        ' Show the station WS04 test phase
        lblWS04Phase.Text = CInt(mWS04Main.Phase).ToString & "." & mWS04Main.SubPhase(mWS04Main.Phase).ToString & " - " & mWS04Main.PhaseDescription(mWS04Main.Phase)
        ' Show the station WS04 reference
        lblWS04Reference.Text = mWS04Main.Reference
        If (CType(mWS04Ethernet.InputValue(mWS04Ethernet.eInput.Test_Mode), mWS04Main.eTestMode) = mWS04Main.eTestMode.Analyse) Then
            BtnWS04StartPLC.Visible = True
        Else
            BtnWS04StartPLC.Visible = False
        End If

        ' Show btn Step
        If mWS04Main.StepByStep = False Then
            btnWS04StepMode.BackColor = Color.Yellow
        Else
            btnWS04StepMode.BackColor = Color.LawnGreen
        End If
        btnWS04Step.Visible = mWS04Main.StepByStep

        ' Show the station WS04 test results
        lblWS04TestResult.Text = CInt(mWS04Main.Results.TestResult).ToString & " = " & cWS04Results.TestResultDescription(mWS04Main.Results.TestResult)
        If (mWS04Main.Results.TestResult = cWS04Results.eTestResult.Unknown) Then
            lblWS04TestResult.BackColor = Color.White
        ElseIf (mWS04Main.Results.TestResult = cWS04Results.eTestResult.Passed) Then
            lblWS04TestResult.BackColor = Color.LawnGreen
        Else
            lblWS04TestResult.BackColor = Color.Red
        End If

        'Show last test
        lblWS04TPCycle.Text = Format(mWS04Main.TestLast, "0.00") & " s"

        ' Show the station WS04 single test results
        With dgvWS04SingleTestResults
            For i = 1 To .RowCount - 1
                If (mWS04Main.Results.Value(cWS04Results.SingleTestBaseIndex + i - 1).TestResult = cWS04Results.eSingleTestResult.Disabled) Then
                    .Item(1, i).Value = "---"
                    .Item(1, i).Style.BackColor = Color.DarkGray
                ElseIf (mWS04Main.Results.Value(cWS04Results.SingleTestBaseIndex + i - 1).TestResult = cWS04Results.eSingleTestResult.Unknown) Then
                    .Item(1, i).Value = "???"
                    .Item(1, i).Style.BackColor = Color.Yellow
                ElseIf (mWS04Main.Results.Value(cWS04Results.SingleTestBaseIndex + i - 1).TestResult = cWS04Results.eSingleTestResult.Passed) Then
                    .Item(1, i).Value = "OK"
                    .Item(1, i).Style.BackColor = Color.LawnGreen
                ElseIf (mWS04Main.Results.Value(cWS04Results.SingleTestBaseIndex + i - 1).TestResult = cWS04Results.eSingleTestResult.Failed) Then
                    .Item(1, i).Value = "KO"
                    .Item(1, i).Style.BackColor = Color.Red
                Else
                    .Item(1, i).Value = "NC"
                    .Item(1, i).Style.BackColor = Color.Magenta
                End If
            Next i
        End With

        ' Show the Station WS04 I/O status
        lblWS04TestEnable.BackColor = CType(IIf(mWS04Main.TestEnableInputStatus, Color.Yellow, Color.White), Color)
        lblWS04StepInProgress.BackColor = CType(IIf(mWS04Main.StepInProgressInputStatus, Color.Yellow, Color.White), Color)
        lblWS04TestOk.BackColor = CType(IIf(mWS04Main.TestOkOutputStatus, Color.LawnGreen, Color.White), Color)
        lblWS04StartStep.BackColor = CType(IIf(mWS04Main.StartStepOutputStatus, Color.Yellow, Color.White), Color)

        '************************************************************************
        ' Work Staiton 05 Haptic Push Mirror
        '************************************************************************
        ' Show the station WS05 test mode
        lblWS05TestMode.Text = mWS05Main.TestModeDescription(mWS05Main.TestMode)
        ' Show the station WS05 test phase
        lblWS05Phase.Text = CInt(mWS05Main.Phase).ToString & "." & mWS05Main.SubPhase(mWS05Main.Phase).ToString & " - " & mWS05Main.PhaseDescription(mWS05Main.Phase)
        ' Show the station WS05 reference
        lblWS05Reference.Text = mWS05Main.Reference
        If (CType(mWS05Ethernet.InputValue(mWS05Ethernet.eInput.Test_Mode), mWS05Main.eTestMode) = mWS05Main.eTestMode.Analyse) Then
            BtnWS05StartPLC.Visible = True
        Else
            BtnWS05StartPLC.Visible = False
        End If

        ' Show btn Step
        If mWS05Main.StepByStep = False Then
            btnWS05StepMode.BackColor = Color.Yellow
        Else
            btnWS05StepMode.BackColor = Color.LawnGreen
        End If
        btnWS05Step.Visible = mWS05Main.StepByStep

        ' Show the station WS05 test results
        lblWS05TestResult.Text = CInt(mWS05Main.Results.TestResult).ToString & " = " & cWS05Results.TestResultDescription(mWS05Main.Results.TestResult)
        If (mWS05Main.Results.TestResult = cWS05Results.eTestResult.Unknown) Then
            lblWS05TestResult.BackColor = Color.White
        ElseIf (mWS05Main.Results.TestResult = cWS05Results.eTestResult.Passed) Then
            lblWS05TestResult.BackColor = Color.LawnGreen
        Else
            lblWS05TestResult.BackColor = Color.Red
        End If

        'Show last test
        lblWS05TPCycle.Text = Format(mWS05Main.TestLast, "0.00") & " s"

        ' Show the station WS05 single test results
        With dgvWS05SingleTestResults
            For i = 1 To .RowCount - 1
                If (mWS05Main.Results.Value(cWS05Results.SingleTestBaseIndex + i - 1).TestResult = cWS05Results.eSingleTestResult.Disabled) Then
                    .Item(1, i).Value = "---"
                    .Item(1, i).Style.BackColor = Color.DarkGray
                ElseIf (mWS05Main.Results.Value(cWS05Results.SingleTestBaseIndex + i - 1).TestResult = cWS05Results.eSingleTestResult.Unknown) Then
                    .Item(1, i).Value = "???"
                    .Item(1, i).Style.BackColor = Color.Yellow
                ElseIf (mWS05Main.Results.Value(cWS05Results.SingleTestBaseIndex + i - 1).TestResult = cWS05Results.eSingleTestResult.Passed) Then
                    .Item(1, i).Value = "OK"
                    .Item(1, i).Style.BackColor = Color.LawnGreen
                ElseIf (mWS05Main.Results.Value(cWS05Results.SingleTestBaseIndex + i - 1).TestResult = cWS05Results.eSingleTestResult.Failed) Then
                    .Item(1, i).Value = "KO"
                    .Item(1, i).Style.BackColor = Color.Red
                Else
                    .Item(1, i).Value = "NC"
                    .Item(1, i).Style.BackColor = Color.Magenta
                End If
            Next i
        End With

        ' Show the Station 05 I/O status
        lblWS05TestEnable.BackColor = CType(IIf(mWS05Main.TestEnableInputStatus, Color.Yellow, Color.White), Color)
        lblWS05StepInProgress.BackColor = CType(IIf(mWS05Main.StepInProgressInputStatus, Color.Yellow, Color.White), Color)
        lblWS05TestOk.BackColor = CType(IIf(mWS05Main.TestOkOutputStatus, Color.LawnGreen, Color.White), Color)
        lblWS05StartStep.BackColor = CType(IIf(mWS05Main.StartStepOutputStatus, Color.Yellow, Color.White), Color)

        ' Show the alarms
        alarm = False
        For i = 0 To mWS02Main.eAlarm.Count - 1
            alarm = alarm Or mWS02Main.Alarm(CType(i, mWS02Main.eAlarm))
        Next
        For i = 0 To mWS03Main.eAlarm.Count - 1
            alarm = alarm Or mWS03Main.Alarm(CType(i, mWS03Main.eAlarm))
        Next
        For i = 0 To mWS04Main.eAlarm.Count - 1
            alarm = alarm Or mWS04Main.Alarm(CType(i, mWS04Main.eAlarm))
        Next
        For i = 0 To mWS05Main.eAlarm.Count - 1
            alarm = alarm Or mWS05Main.Alarm(CType(i, mWS05Main.eAlarm))
        Next

        If (alarm) Then
            If Not (frmAlarms.Visible) Then
                frmAlarms.Visible = True
                'Me.Enabled = False
            End If
            frmAlarms.ShowAlarms()
            frmAlarms.BringToFront()
        Else
            If (frmAlarms.Visible = True) Then
                Me.Enabled = True
                frmAlarms.Visible = False
            End If
        End If


        'If (mWS02Main.Phase <> WS02OldPhase And mWS02Main.Phase = mWS02Main.ePhase.WaitStartTest) And
        '    (mWS03Main.Phase <> WS03OldPhase And mWS03Main.Phase = mWS03Main.ePhase.WaitStartTest) And
        '    (mWS04Main.Phase <> WS04OldPhase And mWS04Main.Phase = mWS04Main.ePhase.WaitStartTest) And
        '    (mWS05Main.Phase <> WS05OldPhase And mWS05Main.Phase = mWS05Main.ePhase.WaitStartTest) Then
        '    frmResults.ShowResults()
        'End If

        'WS02OldPhase = mWS02Main.Phase
        'WS03OldPhase = mWS03Main.Phase
        'WS04OldPhase = mWS04Main.Phase
        'WS05OldPhase = mWS05Main.Phase
        ' Update the status bar
        mStatusBar.Update(ssStatusBar)
        ' Re-enable the timer
        tmrMonitor.Enabled = True
    End Sub


    Private Sub lblWS02Reference_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblWS02Reference.Click
        If mWS02Main.TestMode = mWS02Main.eTestMode.Debug Or
            mWS02Main.TestMode = mWS02Main.eTestMode.Local Then
            ' Show the recipe type selection form
            frmRecipeSelection.ShowDialog()
            ' Set the reference
            mWS02Main.Reference = frmRecipeSelection.RecipeName
            mWS02Main.ReloadRecipe()
        End If
    End Sub

    Private Sub btnWS02StartTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS02StartTest.Click
        mWS02Main.StartTest()
    End Sub

    Private Sub btnWS02AbortTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS02AbortTest.Click
        mWS02Main.AbortTest()
    End Sub


    Private Sub dgvWS02SingleTestResult_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvWS02SingleTestResults.CellClick
        ' Make the  results form visible
        frmResults.Visible = True
        frmResults.TabSW090_100.SelectedTab = frmResults.TabPage2
        ' Show the results
        frmResults.ShowResults()
        ' Focus the  results form visible
        frmResults.Focus()

    End Sub

    Private Sub dgvWS03SingleTestResult_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvWS03SingleTestResults.CellClick
        ' Make the  results form visible
        frmResults.Visible = True
        frmResults.TabSW090_100.SelectedTab = frmResults.TabPage1
        ' Show the results
        frmResults.ShowResults()
        ' Focus the  results form visible
        frmResults.Focus()

    End Sub

    Private Sub dgvWS04SingleTestResult_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvWS04SingleTestResults.CellClick
        ' Make the  results form visible
        frmResults.Visible = True
        frmResults.TabSW090_100.SelectedTab = frmResults.TabPage11
        ' Show the results
        frmResults.ShowResults()
        ' Focus the  results form visible
        frmResults.Focus()

    End Sub

    Private Sub dgvWS05SingleTestResult_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvWS05SingleTestResults.CellClick
        ' Make the  results form visible
        frmResults.Visible = True
        frmResults.TabSW090_100.SelectedTab = frmResults.TabPage21
        frmResults.tbWS05.SelectedTab = frmResults.tpWS05General
        ' Show the results
        frmResults.ShowResults()
        ' Focus the  results form visible
        frmResults.Focus()

    End Sub


    Private Sub btnWS03AbortTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03AbortTest.Click
        mWS03Main.AbortTest()
    End Sub

    Private Sub btnWS04AbortTest_Click(sender As Object, e As EventArgs) Handles btnWS04AbortTest.Click
        mWS04Main.AbortTest()
    End Sub
    Private Sub lblWS03Reference_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblWS03Reference.Click
        If mWS03Main.TestMode = mWS03Main.eTestMode.Debug Or
            mWS03Main.TestMode = mWS03Main.eTestMode.Local Then
            ' Show the recipe type selection form
            frmRecipeSelection.ShowDialog()
            ' Set the reference
            mWS03Main.Reference = frmRecipeSelection.RecipeName
            mWS03Main.ReloadRecipe()
        End If
    End Sub

    Private Sub lblWS04Reference_Click(sender As Object, e As EventArgs) Handles lblWS04Reference.Click
        If mWS04Main.TestMode = mWS04Main.eTestMode.Debug Or
            mWS04Main.TestMode = mWS04Main.eTestMode.Local Then
            ' Show the recipe type selection form
            frmRecipeSelection.ShowDialog()
            ' Set the reference
            mWS04Main.Reference = frmRecipeSelection.RecipeName
            mWS04Main.ReloadRecipe()
        End If
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If (mUserManager.Login >= 99) Then
            'frmTricks.Show()
            frmTricks.ShowDialog()
        End If

    End Sub

    Private Sub btnWS03StepMode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03StepMode.Click
        mWS03Main.StepByStep = Not mWS03Main.StepByStep
    End Sub

    Private Sub btnWS03Step_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03Step.Click
        mWS03Main.StepTest()
    End Sub

    Private Sub btnWS03StartTest_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS03StartTest.Click
        mWS03Main.StartTest()
    End Sub

    Private Sub btnWS04StartTest_Click(sender As Object, e As EventArgs) Handles btnWS04StartTest.Click
        mWS04Main.StartTest()
    End Sub
    Private Sub btnWS02StepMode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS02StepMode.Click
        mWS02Main.StepByStep = Not mWS02Main.StepByStep
        btnWS02Step.Visible = mWS02Main.StepByStep
    End Sub

    Private Sub btnWS02Step_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS02Step.Click
        mWS02Main.StepTest()
    End Sub

    Private Sub tmrLoop_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrLoop.Tick
        '
        tmrLoop.Enabled = False
        '
        fmrTestLoop.Show()

    End Sub

    Private Sub CustomerLogo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CustomerLogo.Click
        If (mUserManager.Login >= 1) Then
            frmTricks.ShowDialog()
            mUserManager.Logout()
        End If
    End Sub


    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnWS03StartPLC.Click
        mWS03Main.StartPLC()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnWS04StartPLC.Click
        mWS04Main.StartPLC()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnWS05StartPLC.Click
        mWS05Main.StartPLC()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnWS02StartPLC.Click
        mWS02Main.StartPLC()
    End Sub

    Private Sub lblWS05Reference_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblWS05Reference.Click
        If mWS03Main.TestMode = mWS05Main.eTestMode.Debug Or
            mWS05Main.TestMode = mWS05Main.eTestMode.Local Then
            ' Show the recipe type selection form
            frmRecipeSelection.ShowDialog()
            ' Set the reference
            mWS05Main.Reference = frmRecipeSelection.RecipeName
            mWS05Main.ReloadRecipe()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS05AbortTest.Click
        mWS05Main.AbortTest()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnWS05StartTest.Click
        mWS05Main.StartTest()
    End Sub

    Private Sub BTShowDUTStats_Click(sender As Object, e As EventArgs) Handles BTShowDUTStats.Click
        Try
            mGlobal.FormDUTStats.Visible = False
            mGlobal.FormDUTStats.Show()
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

End Class