Option Explicit On
Option Strict On

Public Class frmAlarms
    '+------------------------------------------------------------------------------+
    '|                             Public declarations                              |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                             Private declarations                             |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                  Properties                                  |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                          Constructor and destructor                          |
    '+------------------------------------------------------------------------------+



    '+------------------------------------------------------------------------------+
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Sub ShowAlarms()
        ' Reference to the table
        With dgvAlarms
            ' Configurate the rows
            .RowHeadersVisible = False
            .RowCount = 0
            ' Configurate the columns
            .ColumnHeadersVisible = False
            .ColumnCount = 1
            .Columns(0).Width = .Width
            ' Configurate the scrollbars
            .ScrollBars = ScrollBars.Vertical
            ' Configurate the default cell style
            .DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft
            .DefaultCellStyle.BackColor = Color.Red
            .DefaultCellStyle.ForeColor = Color.White
            .DefaultCellStyle.Font = New Font("Arial", 12, FontStyle.Bold)
            ' Show the station WS02 alarms
            For i = 0 To mWS02Main.eAlarm.Count - 1
                If (mWS02Main.Alarm(CType(i, mWS02Main.eAlarm))) Then
                    .RowCount = .RowCount + 1
                    .Rows(.RowCount - 1).Height = 30
                    .Item(0, .RowCount - 1).Value = "Work Station WS02 - " & mWS02Main.AlarmDescription(CType(i, mWS02Main.eAlarm))
                End If
            Next i
            .RowCount = .RowCount + 1
            .Rows(.RowCount - 1).Height = 30
            .Item(0, .RowCount - 1).Value = ""
            ' Station WS03
            For i = 0 To mWS03Main.eAlarm.Count - 1
                If (mWS03Main.Alarm(CType(i, mWS03Main.eAlarm))) Then
                    .RowCount = .RowCount + 1
                    .Rows(.RowCount - 1).Height = 30
                    .Item(0, .RowCount - 1).Value = "Work Station WS03 - " & mWS03Main.AlarmDescription(CType(i, mWS03Main.eAlarm))
                End If
            Next i
            .RowCount = .RowCount + 1
            .Rows(.RowCount - 1).Height = 30
            .Item(0, .RowCount - 1).Value = ""
            ' Show the station WS04 alarms
            For i = 0 To mWS04Main.eAlarm.Count - 1
                If (mWS04Main.Alarm(CType(i, mWS04Main.eAlarm))) Then
                    .RowCount = .RowCount + 1
                    .Rows(.RowCount - 1).Height = 30
                    .Item(0, .RowCount - 1).Value = "Work Station WS04 - " & mWS04Main.AlarmDescription(CType(i, mWS04Main.eAlarm))
                End If
            Next i
            .RowCount = .RowCount + 1
            .Rows(.RowCount - 1).Height = 30
            .Item(0, .RowCount - 1).Value = ""
            ' Show the station WS05 alarms
            For i = 0 To mWS05Main.eAlarm.Count - 1
                If (mWS05Main.Alarm(CType(i, mWS05Main.eAlarm))) Then
                    .RowCount = .RowCount + 1
                    .Rows(.RowCount - 1).Height = 30
                    .Item(0, .RowCount - 1).Value = "Work Station WS05 - " & mWS05Main.AlarmDescription(CType(i, mWS05Main.eAlarm))
                End If
            Next i
        End With
    End Sub



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
    Private Sub btnClearAlarms_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAlarms.Click
        ' Clear the Station WS02 alarms
        mWS02Main.ClearAlarms()
        ' Clear the Station W03 alarms
        mWS03Main.ClearAlarms()
        ' Clear the Station WS04 alarms
        mWS04Main.ClearAlarms()
        ' Clear the Station WS05 alarms
        mWS05Main.ClearAlarms()
        '
        Me.Close()
    End Sub



    Private Sub dgvAlarms_SelectionChanged(sender As Object, e As System.EventArgs) Handles dgvAlarms.SelectionChanged
        ' Clear the selection
        dgvAlarms.ClearSelection()
    End Sub



    Private Sub frmAlarms_FormClosing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        ' If the user tries to manually close the form
        If (e.CloseReason = CloseReason.UserClosing) Then
            ' Cancel the closing request
            e.Cancel = True
        End If
    End Sub

    Private Sub frmAlarms_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class