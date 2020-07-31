'Module Temp
'    Private Sub PhasePlatine_Test(ByVal UP_DN As mGlobal.eUP_DN, _
'                                  ByVal WiLi_Test As mGlobal.eWindows)
'        Dim e As Boolean
'        Dim sp As Integer
'        Dim i As Integer

'        Dim PeakStrenghtF1 As Single
'        Dim TactileRatioF1F2 As Single
'        Dim TactileRatioF4F5 As Single
'        Dim TactileRatioF1F4 As Single

'        Dim EarlyContactCe1 As Single
'        Dim EarlyContactCe2Ce1 As Single
'        Dim OverEarly As Single

'        Dim failedOff As Boolean
'        Dim failedOnCe1 As Boolean
'        Dim failedOnCe2 As Boolean
'        Dim WindowsLifterTest As cWS03Results_Platine.eWindowsLifterTest
'        ' Offset for Early Sensor and Strenght Sensor to adapte Data measurement
'        Dim OffsetEarlySensor As Integer = 1
'        Dim OffsetStrenghtSensor As Integer = 1
'        Dim f As CLINFrame
'        Dim tpSample As Single
'        Static CommutCe1 As Single
'        Static CommutCe2 As Single
'        Static frameIndex As Integer
'        Static t0 As Date
'        Static t0Phase As Date
'        Static t0Push As Date
'        Static s As String
'        Static StatusPushLin(0 To 20) As String
'        ' Clear the error flag
'        e = False
'        ' Store the entry subphase
'        sp = _subPhase(_phase)
'        ' Setting de Windows Lifter Test
'        If WiLi_Test = eWindows.FrontLeft And UP_DN = eUP_DN.UP Then
'            WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_UP
'        ElseIf WiLi_Test = eWindows.FrontLeft And UP_DN = eUP_DN.DN Then
'            WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_DN
'        ElseIf WiLi_Test = eWindows.FrontRight And UP_DN = eUP_DN.UP Then
'            WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_UP
'        ElseIf WiLi_Test = eWindows.FrontRight And UP_DN = eUP_DN.DN Then
'            WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_DN
'        ElseIf WiLi_Test = eWindows.RearLeft And UP_DN = eUP_DN.UP Then
'            WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_UP
'        ElseIf WiLi_Test = eWindows.RearLeft And UP_DN = eUP_DN.DN Then
'            WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_DN
'        ElseIf WiLi_Test = eWindows.RearRight And UP_DN = eUP_DN.UP Then
'            WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_UP
'        ElseIf WiLi_Test = eWindows.RearRight And UP_DN = eUP_DN.DN Then
'            WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_DN
'        End If
'        ' Manage the subphases
'        Select Case sp
'            Case 0
'                If (CBool(_recipe_Platine.TestEnable_Front_Left_UP_Electrical.Value) And WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_UP) Or _
'                    (CBool(_recipe_Platine.TestEnable_Front_Left_DN_Electrical.Value) And WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_DN) Or _
'                    (CBool(_recipe_Platine.TestEnable_Front_Right_UP_Electrical.Value) And WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_UP) Or _
'                    (CBool(_recipe_Platine.TestEnable_Front_Right_DN_Electrical.Value) And WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_DN) Or _
'                    (CBool(_recipe_Platine.TestEnable_Rear_Left_UP_Electrical.Value) And WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_UP) Or _
'                    (CBool(_recipe_Platine.TestEnable_Rear_Left_DN_Electrical.Value) And WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_DN) Or _
'                    (CBool(_recipe_Platine.TestEnable_Rear_Right_UP_Electrical.Value) And WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_UP) Or _
'                    (CBool(_recipe_Platine.TestEnable_Rear_Right_DN_Electrical.Value) And WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_DN) Then
'                    ' Store the phase entry time
'                    t0Phase = Date.Now
'                    ' Add a log entry
'                    If WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_UP Then
'                        AddLogEntry("Begin Windows Lifter Front Left UP ")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_DN Then
'                        AddLogEntry("Begin Windows Lifter Front Left DN ")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_UP Then
'                        AddLogEntry("Begin Windows Lifter Front Right UP ")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_DN Then
'                        AddLogEntry("Begin Windows Lifter Front Right DN ")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_UP Then
'                        AddLogEntry("Begin Windows Lifter Rear Left UP ")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_DN Then
'                        AddLogEntry("Begin Windows Lifter Rear Left DN ")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_UP Then
'                        AddLogEntry("Begin Windows Lifter Rear Right UP ")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_DN Then
'                        AddLogEntry("Begin Windows Lifter Rear Right DN ")
'                    End If
'                    ' clear data lin
'                    For i = 0 To 20
'                        StatusPushLin(i) = "00"
'                    Next
'                    CommutCe1 = 0
'                    CommutCe2 = 0
'                    ' Store the time
'                    t0 = Date.Now
'                    ' Go to next subphase
'                    _subPhase(_phase) = 1
'                Else
'                    ' Add a log entry
'                    If WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_UP Then
'                        AddLogEntry("Windows Lifter Front Left UP is disabled")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_DN Then
'                        AddLogEntry("Windows Lifter Front Left DN is disabled")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_UP Then
'                        AddLogEntry("Windows Lifter Front Right UP is disabled")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_DN Then
'                        AddLogEntry("Windows Lifter Front Right DN is disabled")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_UP Then
'                        AddLogEntry("Windows Lifter Rear Left UP is disabled")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_DN Then
'                        AddLogEntry("Windows Lifter Rear Left DN is disabled")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_UP Then
'                        AddLogEntry("Windows Lifter Rear Right UP is disabled")
'                    ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_DN Then
'                        AddLogEntry("Windows Lifter Rear Right DN is disabled")
'                    End If
'                    ' Go to next phase
'                    If _phase = ePhase.FrontLeft_UP Then
'                        _phase = ePhase.FrontLeft_DN
'                    ElseIf _phase = ePhase.FrontLeft_DN Then
'                        _phase = ePhase.FrontRight_UP
'                    ElseIf _phase = ePhase.FrontRight_UP Then
'                        _phase = ePhase.FrontRight_DN
'                    ElseIf _phase = ePhase.FrontRight_DN Then
'                        _phase = ePhase.RearLeft_UP
'                    ElseIf _phase = ePhase.RearLeft_UP Then
'                        _phase = ePhase.RearLeft_DN
'                    ElseIf _phase = ePhase.RearLeft_DN Then
'                        _phase = ePhase.RearRight_UP
'                    ElseIf _phase = ePhase.RearLeft_UP Then
'                        _phase = ePhase.RearRight_DN
'                    ElseIf _phase = ePhase.RearRight_DN Then
'                        _phase = ePhase.FinalState
'                    End If

'                End If

'            Case 1
'                AddLogEntry("Reset HBM " & vbTab & "Case : " & _subPhase(_phase))
'                t0Push = Date.Now
'                ' Set the force sensors reset signals
'                mWS03DIOManager.SetDigitalOutput(mWS03DIOManager.eDigitalOutput.DO_Reset_Force)
'                ' Transmit Frame Diag (Frame, entry Log, Data Value, Session Diag, Send Frame or change Data, Diag Request)
'                AddLogEntry("Trasmit Frame Start Counter Transition " & vbTab & "Case : " & _subPhase(_phase))
'                e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_TCounterStart), _
'                                                    True, _
'                                                    txData_MasterReq, _
'                                                    cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
'                                                    True, _
'                                                    True)
'                ' Store the time
'                t0 = Date.Now
'                ' Go to next subphase
'                _subPhase(_phase) = 2

'            Case 2
'                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_TCounterStart))
'                If (i <> -1) Then
'                    AddLogEntry("Ack LIN Frame " & vbTab & "Case : " & _subPhase(_phase))
'                    ' Delete the frame
'                    _LinInterface.DeleteRxFrame(i)
'                    ' Start sampling the analog inputs
'                    e = mWS03AIOManager.StartSampling(_samplingFrequency)
'                    AddLogEntry("Start Analog Sample " & vbTab & "Case : " & _subPhase(_phase))
'                    ' Set the start step output (push activation)
'                    AddLogEntry("Start Step = 1 " & vbTab & "Case : " & _subPhase(_phase))
'                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03StartStep)
'                    ' Go to subphase 8
'                    _subPhase(_phase) = 3
'                ElseIf ((Date.Now - t0).TotalMilliseconds >= _LINTimeout_ms) Then
'                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03StartStep)
'                    ' Store the time
'                    t0 = Date.Now
'                    _subPhase(_phase) = 3
'                End If

'            Case 3
'                ' If the step in progress input is set (push activation)
'                If ((mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03StepInProgress) = True And _
'                        (StepByStep = False Or (StepByStep = True And _step = True))) Or _testMode = eTestMode.Debug) Then
'                    _step = False
'                    AddLogEntry("Start Step = 0 / Step in Progress = 1 " & vbTab & "Case : " & _subPhase(_phase))
'                    ' Reset the force sensors reset signals
'                    mWS03DIOManager.ResetDigitalOutput(mWS03DIOManager.eDigitalOutput.DO_Reset_Force)
'                    ' Reset the start step output (push activation)
'                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03StartStep)
'                    ' Go to subphase 9
'                    _subPhase(_phase) = 4
'                End If

'            Case 4
'                ' If the step in progress input is cleared (push activation)
'                If (mDIOManager.DigitalInputStatus(eDigitalInput.WS03StepInProgress) = False) Then
'                    AddLogEntry("Step in Progress = 0 / ReadDataBlock  transition " & vbTab & "Case : " & _subPhase(_phase))
'                    ' Stop sampling the analog inputs
'                    e = e Or mWS03AIOManager.StopSampling

'                    ' Read Timing Lin
'                    e = e Or _LinInterface.Transmit(_LINFrame(mGlobal.eLINFrame.DIAG_Req_Raw_Push_Trans), _
'                                        True, _
'                                        txData_MasterReq, _
'                                        cLINInterface.eNxSessionOut.eSession_DIAG_MasterReq, _
'                                        True, _
'                                        True)
'                    ' Store the time
'                    t0 = Date.Now
'                    frameIndex = 21
'                    AddLogEntry((Date.Now - t0Push).TotalMilliseconds.ToString("0.00"))
'                    ' Go to subphase 10
'                    _subPhase(_phase) = 5
'                End If

'            Case 5
'                'Check Receive Frame
'                i = _LinInterface.RxFrameIndex(_LINFrame(mGlobal.eLINFrame.DIAG_Rep_Raw_Push_Trans))
'                If (i <> -1) Then
'                    AddLogEntry("Stop Analog Sample " & vbTab & "Case : " & _subPhase(_phase))
'                    s = ""
'                    ' Delete the frame
'                    _LinInterface.DeleteRxFrame(i)
'                    frameIndex = 21
'                    ' Store the time
'                    t0 = Date.Now
'                ElseIf ((Date.Now - t0).TotalMilliseconds >= _LINTimeout_ms) Then
'                    _subPhase(_phase) = 6
'                End If

'                Do
'                    ' If a CAN frame was received
'                    f = _LINFrame(mGlobal.eLINFrame.Diag_Rep_FlowCtrl).DeepClone
'                    For i = 0 To 7
'                        f.Data(i) = "XX"
'                    Next i
'                    f.Data(0) = "0A"
'                    f.Data(1) = CStr(frameIndex)
'                    ' Check Response
'                    i = _LinInterface.RxFrameIndex(f)
'                    If (i <> -1) Then
'                        ' Store the values
'                        s = s & _LinInterface.RxFrame(i).Data(2) & _
'                            _LinInterface.RxFrame(i).Data(3) & _
'                            _LinInterface.RxFrame(i).Data(4) & _
'                            _LinInterface.RxFrame(i).Data(5) & _
'                            _LinInterface.RxFrame(i).Data(6) & _
'                            _LinInterface.RxFrame(i).Data(7)
'                        ' Delete the frame
'                        _LinInterface.DeleteRxFrame(i)
'                        ' Increase the frame index or go to subphase 100
'                        If (frameIndex < 22) Then
'                            frameIndex = frameIndex + 1
'                            t0 = Date.Now
'                        Else
'                            AddLogEntry((Date.Now - t0Push).TotalMilliseconds.ToString("0.00"))
'                            CommutCe1 = CSng("&h" & Mid(s, 13, 4)) / 2
'                            StatusPushLin(0) = (Mid(s, 5, 2))
'                            StatusPushLin(1) = (Mid(s, 11, 2))
'                            CommutCe2 = CSng("&h" & Mid(s, 13, 4)) / 2
'                            StatusPushLin(2) = (Mid(s, 5, 2))
'                            StatusPushLin(3) = (Mid(s, 11, 2))
'                            ' Lin Schedulle
'                            _LinInterface.StopScheduleDiag()
'                            _subPhase(_phase) = 6
'                        End If
'                    ElseIf ((Date.Now - t0).TotalMilliseconds >= _LINTimeout_ms) Then
'                        ' Set the flag of CAN timeout
'                        _subPhase(_phase) = 6
'                    End If
'                Loop Until (i = -1)

'            Case 6
'                If ((mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03StepInProgress) = False And _
'                        (StepByStep = False Or (StepByStep = True And _step = True))) Or _testMode = eTestMode.Debug) Then
'                    _step = False
'                    AddLogEntry("Start Step = 1 / Step in Progress = 0 " & vbTab & "Case : " & _subPhase(_phase))
'                    ' Start Robot for realese
'                    e = e Or mDIOManager.SetDigitalOutput(mDIOManager.eDigitalOutput.WS03StartStep)
'                    '
'                    _subPhase(_phase) = 7
'                End If

'            Case 7
'                ' If the step in progress input is set (push activation)
'                If ((mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03StepInProgress) = True And _
'                        (StepByStep = False Or (StepByStep = True And _step = True))) Or _testMode = eTestMode.Debug) Then
'                    _step = False
'                    AddLogEntry("Start Step = 0 / Step in Progress = 1 " & vbTab & "Case : " & _subPhase(_phase))
'                    ' Reset the start step output (push activation)
'                    e = e Or mDIOManager.ResetDigitalOutput(mDIOManager.eDigitalOutput.WS03StartStep)
'                    ' Go to subphase 9
'                    _subPhase(_phase) = 8
'                End If

'            Case 8
'                ' Store the sample count
'                _results_Platine.SampleCount(WindowsLifterTest) = mWS03AIOManager.SampleCount

'                If (_results_Platine.SampleCount(WindowsLifterTest) > cWS03Results_Platine.MaxSampleCount) Then

'                    _results_Platine.SampleCount(WindowsLifterTest) = cWS03Results_Platine.MaxSampleCount
'                End If
'                ' Store the samples
'                For sampleIndex = 0 To _results_Platine.SampleCount(WindowsLifterTest) - 1

'                    ' Early Sensor
'                    ' Attnetion le capteur Keyence peut fournir des datas erronées au debut du test, voir si il faut faire un filtre
'                    _results_Platine.Sample(WindowsLifterTest, 0, sampleIndex) = _
'                        mWS03AIOManager.SampleBuffer(mWS03AIOManager.eAnalogInput.WS03_Stroke, sampleIndex) * _
'                        OffsetEarlySensor
'                    ' Strenght Sensor
'                    _results_Platine.Sample(WindowsLifterTest, 1, sampleIndex) = _
'                        mWS03AIOManager.SampleBuffer(mWS03AIOManager.eAnalogInput.WS03_Force, sampleIndex) * _
'                        OffsetStrenghtSensor
'                    tpSample = sampleIndex * ((1 / _samplingFrequency) * 1000)
'                    ' 50 offset de timing
'                    If tpSample >= CommutCe2 - 20 Then
'                        ' Windows Lifter Front Left UP Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_UP_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 0) And &H1)))) * 13.5
'                        ' Windows Lifter Front Left UP Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_UP_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 1) And &H1)))) * 13.5
'                        ' Windows Lifter Front Left DN Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_DN_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 2) And &H1)))) * 13.5
'                        ' Windows Lifter Front Left DN Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_DN_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 3) And &H1)))) * 13.5
'                        ' Windows Lifter Front Right UP Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_UP_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 4) And &H1)))) * 13.5
'                        ' Windows Lifter Front Right UP Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_UP_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 5) And &H1)))) * 13.5
'                        ' Windows Lifter Front Right DN Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_DN_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 6) And &H1)))) * 13.5
'                        ' Windows Lifter Front Right DN Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_DN_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 7) And &H1)))) * 13.5

'                        ' Windows Lifter Rear Left UP Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_UP_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 0) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Left UP Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_UP_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 1) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Left DN Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_DN_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 2) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Left DN Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_DN_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 3) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Right UP Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_UP_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 4) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Right UP Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_UP_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 5) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Right DN Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_DN_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 6) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Right DN Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_DN_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(2)) \ CByte(2 ^ 7) And &H1)))) * 13.5

'                    ElseIf tpSample >= CommutCe1 - 20 And tpSample < CommutCe2 - 20 Then
'                        ' Windows Lifter Front Left UP Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_UP_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 0) And &H1)))) * 13.5
'                        ' Windows Lifter Front Left UP Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_UP_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 1) And &H1)))) * 13.5
'                        ' Windows Lifter Front Left DN Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_DN_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 2) And &H1)))) * 13.5
'                        ' Windows Lifter Front Left DN Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_DN_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 3) And &H1)))) * 13.5
'                        ' Windows Lifter Front Right UP Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_UP_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 4) And &H1)))) * 13.5
'                        ' Windows Lifter Front Right UP Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_UP_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 5) And &H1)))) * 13.5
'                        ' Windows Lifter Front Right DN Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_DN_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 6) And &H1)))) * 13.5
'                        ' Windows Lifter Front Right DN Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_DN_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 7) And &H1)))) * 13.5

'                        ' Windows Lifter Rear Left UP Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_UP_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 0) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Left UP Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_UP_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 1) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Left DN Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_DN_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 2) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Left DN Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_DN_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 3) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Right UP Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_UP_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 4) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Right UP Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_UP_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 5) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Right DN Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_DN_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 6) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Right DN Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_DN_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(1)) \ CByte(2 ^ 7) And &H1)))) * 13.5
'                    Else
'                        ' Windows Lifter Front Left UP Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_UP_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 0) And &H1)))) * 13.5
'                        ' Windows Lifter Front Left UP Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_UP_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 1) And &H1)))) * 13.5
'                        ' Windows Lifter Front Left DN Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_DN_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 2) And &H1)))) * 13.5
'                        ' Windows Lifter Front Left DN Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_DN_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 3) And &H1)))) * 13.5
'                        ' Windows Lifter Front Right UP Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_UP_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 4) And &H1)))) * 13.5
'                        ' Windows Lifter Front Right UP Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_UP_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 5) And &H1)))) * 13.5
'                        ' Windows Lifter Front Right DN Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_DN_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 6) And &H1)))) * 13.5
'                        ' Windows Lifter Front Right DN Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_DN_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 7) And &H1)))) * 13.5

'                        ' Windows Lifter Rear Left UP Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_UP_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 0) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Left UP Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_UP_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 1) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Left DN Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_DN_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 2) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Left DN Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_DN_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 3) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Right UP Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_UP_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 4) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Right UP Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_UP_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 5) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Right DN Manual
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_DN_Manual + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 6) And &H1)))) * 13.5
'                        ' Windows Lifter Rear Right DN Automatic
'                        _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_DN_Automtic + 2, sampleIndex) = _
'                            CInt((Hex((CByte("&H" & StatusPushLin(0)) \ CByte(2 ^ 7) And &H1)))) * 13.5
'                    End If
'                Next
'                'Write_AnalogPoint(WindowsLifterTest)
'                ' Store the time
'                t0 = Date.Now
'                ' Go to subphase 11
'                _subPhase(_phase) = 9

'            Case 9
'                ' Process the electrical test samples
'                ProcessSamples_BaseTps(WindowsLifterTest, _
'                                       PeakStrenghtF1,
'                                       TactileRatioF1F2, _
'                                       TactileRatioF4F5, _
'                                       TactileRatioF1F4, _
'                                       EarlyContactCe1, _
'                                       EarlyContactCe2Ce1, _
'                                       OverEarly)
'                '
'                'Write_AnalogPoint(WindowsLifterTest)

'                ' Store the commutation Early, the overstroke, the Strenght peak F1, the Early Strenght S1, the Activation Strenght F2 and the Tactile Ratio
'                If WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_UP Then
'                    ' Stroke
'                    _results_Platine.WL_Early_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe1
'                    _results_Platine.WL_Early_Ce2_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe2Ce1
'                    _results_Platine.WL_Over_stroke(UP_DN, WiLi_Test).Value = OverEarly
'                    ' Force
'                    _results_Platine.WL_Peak_Strenght_F1(UP_DN, WiLi_Test).Value = PeakStrenghtF1
'                    _results_Platine.WL_Strenght_Ratio_F1_F2(UP_DN, WiLi_Test).Value = TactileRatioF1F2
'                    _results_Platine.WL_Strenght_Ratio_F4_F5(UP_DN, WiLi_Test).Value = TactileRatioF4F5
'                    _results_Platine.WL_Strenght_Ratio_F4_F1(UP_DN, WiLi_Test).Value = TactileRatioF1F4
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_DN Then
'                    ' Stroke
'                    _results_Platine.WL_Early_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe1
'                    _results_Platine.WL_Early_Ce2_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe2Ce1
'                    _results_Platine.WL_Over_stroke(UP_DN, WiLi_Test).Value = OverEarly
'                    ' Force
'                    _results_Platine.WL_Peak_Strenght_F1(UP_DN, WiLi_Test).Value = PeakStrenghtF1
'                    _results_Platine.WL_Strenght_Ratio_F1_F2(UP_DN, WiLi_Test).Value = TactileRatioF1F2
'                    _results_Platine.WL_Strenght_Ratio_F4_F5(UP_DN, WiLi_Test).Value = TactileRatioF4F5
'                    _results_Platine.WL_Strenght_Ratio_F4_F1(UP_DN, WiLi_Test).Value = TactileRatioF1F4
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_UP Then
'                    ' Stroke
'                    _results_Platine.WL_Early_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe1
'                    _results_Platine.WL_Early_Ce2_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe2Ce1
'                    _results_Platine.WL_Over_stroke(UP_DN, WiLi_Test).Value = OverEarly
'                    ' Force
'                    _results_Platine.WL_Peak_Strenght_F1(UP_DN, WiLi_Test).Value = PeakStrenghtF1
'                    _results_Platine.WL_Strenght_Ratio_F1_F2(UP_DN, WiLi_Test).Value = TactileRatioF1F2
'                    _results_Platine.WL_Strenght_Ratio_F4_F5(UP_DN, WiLi_Test).Value = TactileRatioF4F5
'                    _results_Platine.WL_Strenght_Ratio_F4_F1(UP_DN, WiLi_Test).Value = TactileRatioF1F4
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_DN Then
'                    ' Stroke
'                    _results_Platine.WL_Early_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe1
'                    _results_Platine.WL_Early_Ce2_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe2Ce1
'                    _results_Platine.WL_Over_stroke(UP_DN, WiLi_Test).Value = OverEarly
'                    ' Force
'                    _results_Platine.WL_Peak_Strenght_F1(UP_DN, WiLi_Test).Value = PeakStrenghtF1
'                    _results_Platine.WL_Strenght_Ratio_F1_F2(UP_DN, WiLi_Test).Value = TactileRatioF1F2
'                    _results_Platine.WL_Strenght_Ratio_F4_F5(UP_DN, WiLi_Test).Value = TactileRatioF4F5
'                    _results_Platine.WL_Strenght_Ratio_F4_F1(UP_DN, WiLi_Test).Value = TactileRatioF1F4
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_UP Then
'                    ' Stroke
'                    _results_Platine.WL_Early_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe1
'                    _results_Platine.WL_Early_Ce2_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe2Ce1
'                    _results_Platine.WL_Over_stroke(UP_DN, WiLi_Test).Value = OverEarly
'                    ' Force
'                    _results_Platine.WL_Peak_Strenght_F1(UP_DN, WiLi_Test).Value = PeakStrenghtF1
'                    _results_Platine.WL_Strenght_Ratio_F1_F2(UP_DN, WiLi_Test).Value = TactileRatioF1F2
'                    _results_Platine.WL_Strenght_Ratio_F4_F5(UP_DN, WiLi_Test).Value = TactileRatioF4F5
'                    _results_Platine.WL_Strenght_Ratio_F4_F1(UP_DN, WiLi_Test).Value = TactileRatioF1F4
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_DN Then
'                    ' Stroke
'                    _results_Platine.WL_Early_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe1
'                    _results_Platine.WL_Early_Ce2_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe2Ce1
'                    _results_Platine.WL_Over_stroke(UP_DN, WiLi_Test).Value = OverEarly
'                    ' Force
'                    _results_Platine.WL_Peak_Strenght_F1(UP_DN, WiLi_Test).Value = PeakStrenghtF1
'                    _results_Platine.WL_Strenght_Ratio_F1_F2(UP_DN, WiLi_Test).Value = TactileRatioF1F2
'                    _results_Platine.WL_Strenght_Ratio_F4_F5(UP_DN, WiLi_Test).Value = TactileRatioF4F5
'                    _results_Platine.WL_Strenght_Ratio_F4_F1(UP_DN, WiLi_Test).Value = TactileRatioF1F4
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_UP Then
'                    ' Stroke
'                    _results_Platine.WL_Early_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe1
'                    _results_Platine.WL_Early_Ce2_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe2Ce1
'                    _results_Platine.WL_Over_stroke(UP_DN, WiLi_Test).Value = OverEarly
'                    ' Force
'                    _results_Platine.WL_Peak_Strenght_F1(UP_DN, WiLi_Test).Value = PeakStrenghtF1
'                    _results_Platine.WL_Strenght_Ratio_F1_F2(UP_DN, WiLi_Test).Value = TactileRatioF1F2
'                    _results_Platine.WL_Strenght_Ratio_F4_F5(UP_DN, WiLi_Test).Value = TactileRatioF4F5
'                    _results_Platine.WL_Strenght_Ratio_F4_F1(UP_DN, WiLi_Test).Value = TactileRatioF1F4
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_DN Then
'                    ' Stroke
'                    _results_Platine.WL_Early_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe1
'                    _results_Platine.WL_Early_Ce2_Ce1(UP_DN, WiLi_Test).Value = EarlyContactCe2Ce1
'                    _results_Platine.WL_Over_stroke(UP_DN, WiLi_Test).Value = OverEarly
'                    ' Force
'                    _results_Platine.WL_Peak_Strenght_F1(UP_DN, WiLi_Test).Value = PeakStrenghtF1
'                    _results_Platine.WL_Strenght_Ratio_F1_F2(UP_DN, WiLi_Test).Value = TactileRatioF1F2
'                    _results_Platine.WL_Strenght_Ratio_F4_F5(UP_DN, WiLi_Test).Value = TactileRatioF4F5
'                    _results_Platine.WL_Strenght_Ratio_F4_F1(UP_DN, WiLi_Test).Value = TactileRatioF1F4
'                End If

'                ' Go to subphase 100
'                _subPhase(_phase) = 10

'            Case 10
'                ' If the step in progress input is set (push activation)
'                If ((mDIOManager.DigitalInputStatus(mDIOManager.eDigitalInput.WS03StepInProgress) = False And _
'                        (StepByStep = False Or (StepByStep = True And _step = True))) Or _testMode = eTestMode.Debug) Then
'                    _step = False
'                    AddLogEntry("Start Step = 0 / Step in Progress = 0 " & vbTab & "Case : " & _subPhase(_phase))
'                    ' Go to subphase 9
'                    _subPhase(_phase) = 100
'                End If


'            Case 100
'                ' If the electrical test is enabled
'                '--------------------------------------------
'                '|   Windows Lift UP/DN and Position        |
'                '--------------------------------------------
'                If (_recipe_Platine.TestEnable_Rear_Left_DN_Electrical.Value And WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_UP) Then
'                    ' Test the values before and after commutation
'                    failedOff = False
'                    failedOnCe1 = False
'                    failedOnCe2 = False
'                    For sampleIndex = Wili_Z0(WindowsLifterTest) To _results_Platine.SampleCount(WindowsLifterTest) - 1
'                        If (Not (failedOff) And _
'                            sampleIndex < Wili_EarlyCe1(WindowsLifterTest) Or _
'                             _results_Platine.WL_Early_Ce1(UP_DN, WiLi_Test).Value = 0) Then
'                            _results_Platine.WL_Front_Left_UP_Manual(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_UP_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Front_Left_UP_Automatic(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Front_Left_DN_Manual(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_DN_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Front_Left_DN_Automatic(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_DN_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Front_Right_UP_Manual(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_UP_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Front_Right_UP_Automatic(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Front_Right_DN_Manual(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_DN_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Front_Right_DN_Automatic(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_DN_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Left_UP_Manual(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_UP_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Left_UP_Automatic(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Left_DN_Manual(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_DN_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Left_DN_Automatic(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_DN_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Right_UP_Manual(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Right_UP_Automatic(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Right_DN_Manual(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_DN_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Right_DN_Automatic(0, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(WindowsLifterTest, cWS03Results_Platine.eWindowsLifterSignal.RearRight_DN_Automtic + 2, sampleIndex)

'                            'Front Left
'                            failedOff = failedOff Or (_results_Platine.WL_Front_Left_UP_Manual(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Left_UP_Electrical.Value))
'                            failedOff = failedOff Or (_results_Platine.WL_Front_Left_UP_Automatic(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Left_UP_Electrical.Value))
'                            failedOff = failedOff Or (_results_Platine.WL_Front_Left_DN_Manual(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Left_DN_Electrical.Value))
'                            failedOff = failedOff Or (_results_Platine.WL_Front_Left_DN_Automatic(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Left_DN_Electrical.Value))
'                            'Front Right
'                            failedOff = failedOff Or (_results_Platine.WL_Front_Right_UP_Manual(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Right_UP_Electrical.Value))
'                            failedOff = failedOff Or (_results_Platine.WL_Front_Right_UP_Automatic(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Right_UP_Electrical.Value))
'                            failedOff = failedOff Or (_results_Platine.WL_Front_Right_DN_Manual(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Right_DN_Electrical.Value))
'                            failedOff = failedOff Or (_results_Platine.WL_Front_Right_DN_Automatic(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Right_DN_Electrical.Value))
'                            'Rear Left
'                            failedOff = failedOff Or (_results_Platine.WL_Rear_Left_UP_Manual(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Left_UP_Electrical.Value))
'                            failedOff = failedOff Or (_results_Platine.WL_Rear_Left_UP_Automatic(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Left_UP_Electrical.Value))
'                            failedOff = failedOff Or (_results_Platine.WL_Rear_Left_DN_Manual(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Left_DN_Electrical.Value))
'                            failedOff = failedOff Or (_results_Platine.WL_Rear_Left_DN_Automatic(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Left_DN_Electrical.Value))
'                            ' Rear Right
'                            failedOff = failedOff Or (_results_Platine.WL_Rear_Right_UP_Manual(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Right_UP_Electrical.Value))
'                            failedOff = failedOff Or (_results_Platine.WL_Rear_Right_UP_Automatic(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Right_UP_Electrical.Value))
'                            failedOff = failedOff Or (_results_Platine.WL_Rear_Right_DN_Manual(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Right_DN_Electrical.Value))
'                            failedOff = failedOff Or (_results_Platine.WL_Rear_Right_DN_Automatic(0, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Right_DN_Electrical.Value))

'                        End If
'                        If (Not (failedOnCe1) And _
'                            sampleIndex > Wili_EarlyCe1(1) Or _
'                             _results_Platine.WL_Early_Ce1(UP_DN, WiLi_Test).Value = 0) Then
'                            _results_Platine.WL_Front_Left_UP_Manual(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_UP_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Front_Left_UP_Automatic(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Front_Left_DN_Manual(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_DN_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Front_Left_DN_Automatic(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_DN_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Front_Right_UP_Manual(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_UP_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Front_Right_UP_Automatic(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Front_Right_DN_Manual(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_DN_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Front_Right_DN_Automatic(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_DN_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Left_UP_Manual(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_UP_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Left_UP_Automatic(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Left_DN_Manual(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_DN_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Left_DN_Automatic(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_DN_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Right_UP_Manual(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.RearRight_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Right_UP_Automatic(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.RearRight_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Right_DN_Manual(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.RearRight_DN_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Right_DN_Automatic(1, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(1, cWS03Results_Platine.eWindowsLifterSignal.RearRight_DN_Automtic + 2, sampleIndex)
'                            '
'                            'Front Left
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Front_Left_UP_Manual(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Left_UP_Electrical.Value))
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Front_Left_UP_Automatic(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Left_UP_Electrical.Value))
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Front_Left_DN_Manual(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Left_DN_Electrical.Value))
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Front_Left_DN_Automatic(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Left_DN_Electrical.Value))
'                            'Front Right
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Front_Right_UP_Manual(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Right_UP_Electrical.Value))
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Front_Right_UP_Automatic(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Right_UP_Electrical.Value))
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Front_Right_DN_Manual(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Right_DN_Electrical.Value))
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Front_Right_DN_Automatic(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Right_DN_Electrical.Value))
'                            'Rear Left
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Rear_Left_UP_Manual(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Left_UP_Electrical.Value))
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Rear_Left_UP_Automatic(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Left_UP_Electrical.Value))
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Rear_Left_DN_Manual(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Left_DN_Electrical.Value))
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Rear_Left_DN_Automatic(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Left_DN_Electrical.Value))
'                            ' Rear Right
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Rear_Right_UP_Manual(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Right_UP_Electrical.Value))
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Rear_Right_UP_Automatic(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Right_UP_Electrical.Value))
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Rear_Right_DN_Manual(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Right_DN_Electrical.Value))
'                            failedOnCe1 = failedOnCe1 Or (_results_Platine.WL_Rear_Right_DN_Automatic(1, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Right_DN_Electrical.Value))

'                        End If
'                        If (Not (failedOnCe2) And _
'                                              sampleIndex > Wili_EarlyCe2(2) Or _
'                                               _results_Platine.WL_Early_Ce2_Ce1(UP_DN, WiLi_Test).Value = 0) Then
'                            _results_Platine.WL_Front_Left_UP_Manual(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_UP_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Front_Left_UP_Automatic(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Front_Left_DN_Manual(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_DN_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Front_Left_DN_Automatic(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.FrontLeft_DN_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Front_Right_UP_Manual(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_UP_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Front_Right_UP_Automatic(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Front_Right_DN_Manual(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_DN_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Front_Right_DN_Automatic(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.FrontRight_DN_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Left_UP_Manual(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_UP_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Left_UP_Automatic(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Left_DN_Manual(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_DN_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Left_DN_Automatic(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.RearLeft_DN_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Right_UP_Manual(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.RearRight_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Right_UP_Automatic(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.RearRight_UP_Automtic + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Right_DN_Manual(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.RearRight_DN_Manual + 2, sampleIndex)
'                            _results_Platine.WL_Rear_Right_DN_Automatic(2, UP_DN, WiLi_Test).Value = _
'                                _results_Platine.Sample(2, cWS03Results_Platine.eWindowsLifterSignal.RearRight_DN_Automtic + 2, sampleIndex)
'                            '
'                            'Front Left
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Front_Left_UP_Manual(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Left_UP_Electrical.Value))
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Front_Left_UP_Automatic(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Left_UP_Electrical.Value))
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Front_Left_DN_Manual(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Left_DN_Electrical.Value))
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Front_Left_DN_Automatic(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Left_DN_Electrical.Value))
'                            'Front Right
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Front_Right_UP_Manual(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Right_UP_Electrical.Value))
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Front_Right_UP_Automatic(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Right_UP_Electrical.Value))
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Front_Right_DN_Manual(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Right_DN_Electrical.Value))
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Front_Right_DN_Automatic(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Front_Right_DN_Electrical.Value))
'                            'Rear Left
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Rear_Left_UP_Manual(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Left_UP_Electrical.Value))
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Rear_Left_UP_Automatic(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Left_UP_Electrical.Value))
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Rear_Left_DN_Manual(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Left_DN_Electrical.Value))
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Rear_Left_DN_Automatic(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Left_DN_Electrical.Value))
'                            ' Rear Right
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Rear_Right_UP_Manual(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Right_UP_Electrical.Value))
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Rear_Right_UP_Automatic(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Right_UP_Electrical.Value))
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Rear_Right_DN_Manual(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Right_DN_Electrical.Value))
'                            failedOnCe2 = failedOnCe2 Or (_results_Platine.WL_Rear_Right_DN_Automatic(2, UP_DN, WiLi_Test).Test <> _
'                                                      cResultValue.eValueTestResult.Passed And CBool(_recipe_Platine.TestEnable_Rear_Right_DN_Electrical.Value))

'                        End If
'                    Next

'                    ' Update the single test result
'                    If (failedOff Or failedOnCe1 Or failedOnCe2) And _
'                        _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Failed
'                        If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                            _results_Platine.eFront_Left_Up_Electrical.TestResult <> _
'                                cResultValue.eValueTestResult.Passed) Then
'                            _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftUP_ElectricalTest
'                        End If
'                    End If
'                    ' Check all data is checked
'                    For i = 0 To 2
'                        If (_results_Platine.WL_Front_Left_UP_Manual(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Front_Left_UP_Automatic(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Front_Left_DN_Manual(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Front_Left_DN_Automatic(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Front_Right_UP_Manual(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Front_Right_UP_Automatic(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Front_Right_DN_Manual(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Front_Right_DN_Automatic(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Rear_Left_UP_Manual(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Rear_Left_UP_Automatic(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Rear_Left_DN_Manual(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Rear_Left_DN_Automatic(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Rear_Right_UP_Manual(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Rear_Right_UP_Automatic(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Rear_Right_DN_Manual(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown Or _
'                            _results_Platine.WL_Rear_Right_DN_Automatic(i, UP_DN, WiLi_Test).TestResult = cResultValue.eValueTestResult.Unknown) And _
'                            _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                            _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Failed
'                            If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                                _results_Platine.eFront_Left_Up_Electrical.TestResult <> _
'                                    cResultValue.eValueTestResult.Passed) Then
'                                _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftUP_ElectricalTest
'                            End If
'                        End If
'                    Next i
'                    ' If the force test is enabled
'                    If (_recipe_Platine.TestEnable_Front_Left_UP_Strenght.Value) Then
'                        ' Test the Peak Strenght F1
'                        If (_results_Platine.WL_Peak_Strenght_F1(UP_DN, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And _
'                            _results_Platine.eFront_Left_Up_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                            _results_Platine.eFront_Left_Up_Strenght.TestResult = cResultValue.eValueTestResult.Failed
'                            If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                                _results_Platine.eFront_Left_Up_Strenght.TestResult <> _
'                                    cResultValue.eValueTestResult.Passed) Then
'                                _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftUP_PeakF1Test
'                            End If
'                        End If
'                        ' Test the Tactile Ratio
'                        If (_results_Platine.WL_Strenght_Ratio_F1_F2(UP_DN, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And _
'                            _results_Platine.eFront_Left_Up_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                            _results_Platine.eFront_Left_Up_Strenght.TestResult = cResultValue.eValueTestResult.Failed
'                            If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                                _results_Platine.eFront_Left_Up_Strenght.TestResult <> _
'                                    cResultValue.eValueTestResult.Passed) Then
'                                _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftUP_F1F2Test
'                            End If
'                        End If
'                        ' Test the Tactile Ratio
'                        If (_results_Platine.WL_Strenght_Ratio_F4_F5(UP_DN, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And _
'                            _results_Platine.eFront_Left_Up_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                            _results_Platine.eFront_Left_Up_Strenght.TestResult = cResultValue.eValueTestResult.Failed
'                            If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                                _results_Platine.eFront_Left_Up_Strenght.TestResult <> _
'                                    cResultValue.eValueTestResult.Passed) Then
'                                _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftUP_F4F5Test
'                            End If
'                        End If
'                        ' Test the Tactile Ratio
'                        If (_results_Platine.WL_Strenght_Ratio_F4_F1(UP_DN, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And _
'                            _results_Platine.eFront_Left_Up_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                            _results_Platine.eFront_Left_Up_Strenght.TestResult = cResultValue.eValueTestResult.Failed
'                            If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                                _results_Platine.eFront_Left_Up_Strenght.TestResult <> _
'                                    cResultValue.eValueTestResult.Passed) Then
'                                _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftUP_F4F1Test
'                            End If
'                        End If
'                    End If
'                    ' Defect Early ce
'                    If (_results_Platine.WL_Early_Ce1(UP_DN, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And _
'                        _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Failed
'                        If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                            _results_Platine.eFront_Left_Up_Electrical.TestResult <> _
'                                cResultValue.eValueTestResult.Passed) Then
'                            _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftUP_Ce1Test
'                        End If
'                    End If
'                    If (_results_Platine.WL_Early_Ce2_Ce1(UP_DN, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And _
'                        _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Failed
'                        If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                            _results_Platine.eFront_Left_Up_Electrical.TestResult <> _
'                                cResultValue.eValueTestResult.Passed) Then
'                            _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftUP_Ce2Ce1Test
'                        End If
'                    End If
'                    ' Defect Over Early
'                    If (_results_Platine.WL_Over_stroke(UP_DN, WiLi_Test).Test <> cResultValue.eValueTestResult.Passed) And _
'                        _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Failed
'                        If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                            _results_Platine.eFront_Left_Up_Electrical.TestResult <> _
'                                cResultValue.eValueTestResult.Passed) Then
'                            _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftUP_OverStrokeTest
'                        End If
'                    End If

'                    If _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Passed
'                    End If
'                    If _results_Platine.eFront_Left_Up_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Left_Up_Strenght.TestResult = cResultValue.eValueTestResult.Passed
'                    End If
'                End If

'                ' Go to next subphase
'                _subPhase(_phase) = 101


'            Case 101
'                AddLogEntry(String.Format("End " & PhaseDescription(_phase) & " - Phase last {0} s" & vbCrLf, (Date.Now - t0Phase).TotalSeconds.ToString("0.00")))
'                ' Updates the global test result
'                If WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_UP Then
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eFront_Left_Up_Electrical.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftUP_ElectricalTest
'                    End If
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eFront_Left_Up_Strenght.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftUP_StrenghtTest
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_DN Then
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eFront_Left_Down_Electrical.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftDN_ElectricalTest
'                    End If
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eFront_Left_Down_Strenght.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontLeftDN_StrenghtTest
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_UP Then
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eFront_Right_Up_Electrical.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontRightUP_ElectricalTest
'                    End If
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eFront_Right_Up_Strenght.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontRightUP_StrenghtTest
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_DN Then
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eFront_Right_Down_Electrical.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontRightDN_ElectricalTest
'                    End If
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eFront_Right_Down_Electrical.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedFrontRightDN_StrenghtTest
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_UP Then
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eRear_Left_Up_Electrical.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedRearLeftUP_ElectricalTest
'                    End If
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eRear_Left_Up_Strenght.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedRearLeftUP_StrenghtTest
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_DN Then
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eRear_Left_Down_Electrical.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedRearLeftDN_ElectricalTest
'                    End If
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eRear_Left_Down_Strenght.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedRearLeftDN_StrenghtTest
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_UP Then
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eRear_Right_Up_Electrical.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedRearRightUP_ElectricalTest
'                    End If
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eRear_Right_Up_Strenght.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedRearRightUP_StrenghtTest
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_DN Then
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eRear_Right_Down_Electrical.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedRearRightDN_ElectricalTest
'                    End If
'                    If (_results_Platine.TestResult = cWS03Results_Platine.eTestResult.Unknown And _
'                        _results_Platine.eRear_Right_Down_Electrical.TestResult <> _
'                            cResultValue.eValueTestResult.Passed) Then
'                        _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedRearRightDN_StrenghtTest
'                    End If
'                End If
'                'Clear Subphase
'                _subPhase(_phase) = 0
'                ' Go to next phase
'                If _phase = ePhase.FrontLeft_UP Then
'                    _phase = ePhase.FrontLeft_DN
'                ElseIf _phase = ePhase.FrontLeft_DN Then
'                    _phase = ePhase.FrontRight_UP
'                ElseIf _phase = ePhase.FrontRight_UP Then
'                    _phase = ePhase.FrontRight_DN
'                ElseIf _phase = ePhase.FrontRight_DN Then
'                    _phase = ePhase.RearLeft_UP
'                ElseIf _phase = ePhase.RearLeft_UP Then
'                    _phase = ePhase.RearLeft_DN
'                ElseIf _phase = ePhase.RearLeft_DN Then
'                    _phase = ePhase.RearRight_UP
'                ElseIf _phase = ePhase.RearLeft_UP Then
'                    _phase = ePhase.RearRight_DN
'                ElseIf _phase = ePhase.RearRight_DN Then
'                    _phase = ePhase.FinalState
'                End If

'            Case 199
'                ' Adds a log entry
'                AddLogEntry("Timeout on LIN")
'                ' Update the test result
'                ' Go to next phase
'                If WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_UP Then
'                    If _results_Platine.eFront_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Left_Up_Electrical.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                    If _results_Platine.eFront_Left_Up_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Left_Up_Strenght.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontLeft_DN Then
'                    If _results_Platine.eFront_Left_Down_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Left_Down_Electrical.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                    If _results_Platine.eFront_Left_Down_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Left_Down_Strenght.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_UP Then
'                    If _results_Platine.eFront_Right_Up_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Right_Up_Electrical.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                    If _results_Platine.eFront_Right_Up_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Right_Up_Strenght.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.FrontRight_DN Then
'                    If _results_Platine.eFront_Right_Down_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Right_Down_Electrical.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                    If _results_Platine.eFront_Right_Down_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eFront_Right_Down_Strenght.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_UP Then
'                    If _results_Platine.eRear_Left_Up_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eRear_Left_Up_Electrical.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                    If _results_Platine.eRear_Left_Up_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eRear_Left_Up_Strenght.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearLeft_DN Then
'                    If _results_Platine.eRear_Left_Down_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eRear_Left_Down_Electrical.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                    If _results_Platine.eRear_Left_Down_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eRear_Left_Down_Strenght.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_UP Then
'                    If _results_Platine.eRear_Right_Up_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eRear_Right_Up_Electrical.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                    If _results_Platine.eRear_Right_Up_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eRear_Right_Up_Strenght.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                ElseIf WindowsLifterTest = cWS03Results_Platine.eWindowsLifterTest.RearRight_DN Then
'                    If _results_Platine.eRear_Right_Down_Electrical.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eRear_Right_Down_Electrical.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                    If _results_Platine.eRear_Right_Down_Strenght.TestResult = cResultValue.eValueTestResult.Unknown Then
'                        _results_Platine.eRear_Right_Down_Strenght.TestResult = _
'                            cResultValue.eValueTestResult.Failed
'                    End If
'                End If
'                ' Go to next subphase
'                _subPhase(_phase) = 101

'        End Select

'        ' TMP DBE
'        e = False
'        If TestMode = eTestMode.Debug and  testmode=etestmode.remote  Then
'            e = False ' Tmp David DBE
'        End If
'        ' If a runtime error occured
'        If (e = True) Then
'            ' Add a log entry
'            AddLogEntry("Runtime error in phase " & PhaseDescription(_phase) & " , subphase " & sp)
'            ' Update the global test result
'            _results_Platine.TestResult = cWS03Results_Platine.eTestResult.FailedRuntimeError
'            ' Raise an alarm for runtime error
'            _alarm(eAlarm.RuntimeError) = True
'            ' Go to Phase Abort test
'            _phase = ePhase.AbortTest
'        End If


'End Module
