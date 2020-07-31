Imports System.ComponentModel

Public Class frmDUTStats

    Private _datasource As DataTable
    Private _syncLock As New Object
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _datasource = New DataTable
        _datasource.Columns.Add("SerialNumber")
        _datasource.Columns.Add("Testing_Time")
        _datasource.Columns.Add("Reference")
        _datasource.Columns.Add("ResultCode")
        _datasource.Columns.Add("NGNameEN")
        _datasource.Columns.Add("NGNameCN")
        '_datasource.Columns.Add("LSL")
        '_datasource.Columns.Add("Value")
        '_datasource.Columns.Add("USL")
        DGV1.DataSource = _datasource
        DGV1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells

    End Sub

    Private Sub frmDUTStats_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.Hide()
        e.Cancel = True
    End Sub

    Private Sub BTReset_Click(sender As Object, e As EventArgs) Handles BTReset.Click

        _datasource.Clear()
        'DGV1.Rows.Clear()
        TBNOK.Text = 0
        TBOK.Text = 0
        TBTotal.Text = 0
        TBYield.Text = "100%"

    End Sub

    Public Sub Addnewitem(ByVal SerialNumber As String, ByVal Testing_Time As DateTime, ByVal Station As Int16, ByVal ResultCode As Integer, ByVal Reference As String)

        Try
            SyncLock _syncLock
                If ResultCode <> 0 Then
                    TBNOK.Text = CInt(TBNOK.Text) + 1
                    Dim tempnewRow As DataRow = _datasource.NewRow
                    tempnewRow("SerialNumber") = SerialNumber
                    tempnewRow("Testing_Time") = Testing_Time.ToLongTimeString
                    tempnewRow("Reference") = Reference
                    tempnewRow("ResultCode") = ResultCode
                    Select Case Station
                        Case 2
                            tempnewRow("NGNameEN") = [Enum].Parse(GetType(cWS02Results.eTestResult), ResultCode)
                            tempnewRow("NGNameCN") = [Enum].Parse(GetType(cWS02Results.eTestResultCN), ResultCode)
                        Case 3
                            tempnewRow("NGNameEN") = [Enum].Parse(GetType(cWS03Results.eTestResult), ResultCode)
                            tempnewRow("NGNameCN") = [Enum].Parse(GetType(cWS03Results.eTestResultCN), ResultCode)
                        Case 4
                            tempnewRow("NGNameEN") = [Enum].Parse(GetType(cWS04Results.eTestResult), ResultCode)
                            tempnewRow("NGNameCN") = [Enum].Parse(GetType(cWS04Results.eTestResultCN), ResultCode)
                        Case 5
                            tempnewRow("NGNameEN") = [Enum].Parse(GetType(cWS05Results.eTestResult), ResultCode)
                            tempnewRow("NGNameCN") = [Enum].Parse(GetType(cWS05Results.eTestResultCN), ResultCode)
                    End Select
                    _datasource.Rows.Add(tempnewRow)
                    DGV1.FirstDisplayedScrollingRowIndex = DGV1.Rows(DGV1.Rows.Count - 1).Index
                Else
                    TBOK.Text = CInt(TBOK.Text) + 1
                End If
                TBTotal.Text = CInt(TBTotal.Text) + 1
                TBYield.Text = (Math.Round(CInt(TBOK.Text) / CInt(TBTotal.Text), 3) * 100).ToString() + "%"
            End SyncLock
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

    End Sub
    'Public Sub Addnewitem(ByVal SerialNumber As String, ByVal Testing_Time As DateTime, ByVal Station As Int16,
    '                      ByVal ResultCode As Integer, ByVal NGNameEN As String, ByVal NGNameCN As String,
    '                      ByVal LSL As String, ByVal Value As String, ByVal USL As String)

    '    Try
    '        SyncLock _syncLock
    '            If ResultCode <> 0 Then
    '                TBNOK.Text = CInt(TBNOK.Text) + 1
    '                Dim tempnewRow As DataRow = _datasource.NewRow
    '                tempnewRow("SerialNumber") = SerialNumber
    '                tempnewRow("Testing_Time") = Testing_Time.ToLongTimeString
    '                tempnewRow("Station") = Station
    '                tempnewRow("ResultCode") = ResultCode
    '                Select Case Station
    '                    Case 2
    '                        tempnewRow("NGNameEN") = [Enum].Parse(GetType(cWS02Results.eTestResult), ResultCode)
    '                        tempnewRow("NGNameCN") = [Enum].Parse(GetType(cWS02Results.eTestResultCN), ResultCode)
    '                    Case 3
    '                        tempnewRow("NGNameEN") = [Enum].Parse(GetType(cWS03Results.eTestResult), ResultCode)
    '                        tempnewRow("NGNameCN") = [Enum].Parse(GetType(cWS03Results.eTestResultCN), ResultCode)
    '                    Case 4
    '                        tempnewRow("NGNameEN") = [Enum].Parse(GetType(cWS04Results.eTestResult), ResultCode)
    '                        tempnewRow("NGNameCN") = [Enum].Parse(GetType(cWS04Results.eTestResultCN), ResultCode)
    '                    Case 5
    '                        tempnewRow("NGNameEN") = [Enum].Parse(GetType(cWS05Results.eTestResult), ResultCode)
    '                        tempnewRow("NGNameCN") = [Enum].Parse(GetType(cWS05Results.eTestResultCN), ResultCode)
    '                End Select
    '                'tempnewRow("NGNameEN") = NGNameEN
    '                'tempnewRow("NGNameCN") = NGNameCN
    '                'tempnewRow("LSL") = LSL
    '                'tempnewRow("Value") = Value
    '                'tempnewRow("USL") = USL
    '                _datasource.Rows.Add(tempnewRow)
    '            Else
    '                TBOK.Text = CInt(TBOK.Text) + 1
    '            End If
    '            TBTotal.Text = CInt(TBTotal.Text) + 1
    '            TBYield.Text = (Math.Round(CInt(TBOK.Text) / CInt(TBTotal.Text), 1) * 100).ToString() + "%"
    '        End SyncLock
    '    Catch ex As Exception
    '        Console.WriteLine(ex.ToString())
    '    End Try

    'End Sub

End Class