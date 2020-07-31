Option Explicit On
Option Strict On

Module mStatusBar
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
    '|                                Public methods                                |
    '+------------------------------------------------------------------------------+
    Public Sub Initialize(ByRef statusBar As StatusStrip)
        Dim label As ToolStripStatusLabel

        ' Initialize the status bar
        With statusBar
            ' Create the items
            .Items.Add(New ToolStripStatusLabel)
            .Items.Add(New ToolStripStatusLabel)
            .Items.Add(New ToolStripStatusLabel)
            .Items.Add(New ToolStripStatusLabel)
            .Items.Add(New ToolStripStatusLabel)
            .Items.Add(New ToolStripStatusLabel)
            .Items.Add(New ToolStripStatusLabel)
            ' Initialize the panel 0 (username)
            label = CType(.Items.Item(0), ToolStripStatusLabel)
            label.AutoSize = False
            label.BorderSides = ToolStripStatusLabelBorderSides.All
            label.BorderStyle = Border3DStyle.SunkenOuter
            label.Width = CInt(0.13 * .Width)
            label.TextAlign = ContentAlignment.MiddleCenter

            ' Initializes the panel 1 (access level)
            label = CType(.Items.Item(1), ToolStripStatusLabel)
            label.AutoSize = False
            label.BorderSides = ToolStripStatusLabelBorderSides.All
            label.BorderStyle = Border3DStyle.SunkenOuter
            label.Width = CInt(0.15 * .Width)
            label.TextAlign = ContentAlignment.MiddleCenter

            ' Initializes the panel 2 (ws02 scan times)
            label = CType(.Items.Item(2), ToolStripStatusLabel)
            label.AutoSize = False
            label.BorderSides = ToolStripStatusLabelBorderSides.All
            label.BorderStyle = Border3DStyle.SunkenOuter
            label.Width = CInt(0.13 * .Width)
            label.TextAlign = ContentAlignment.MiddleCenter

            ' Initializes the panel 3 (ws03 scan times)
            label = CType(.Items.Item(3), ToolStripStatusLabel)
            label.AutoSize = False
            label.BorderSides = ToolStripStatusLabelBorderSides.All
            label.BorderStyle = Border3DStyle.SunkenOuter
            label.Width = CInt(0.13 * .Width)
            label.TextAlign = ContentAlignment.MiddleCenter

            ' Initializes the panel 4 (ws04 scan times)
            label = CType(.Items.Item(4), ToolStripStatusLabel)
            label.AutoSize = False
            label.BorderSides = ToolStripStatusLabelBorderSides.All
            label.BorderStyle = Border3DStyle.SunkenOuter
            label.Width = CInt(0.13 * .Width)
            label.TextAlign = ContentAlignment.MiddleCenter

            ' Initializes the panel 5 (ws05 scan times)
            label = CType(.Items.Item(5), ToolStripStatusLabel)
            label.AutoSize = False
            label.BorderSides = ToolStripStatusLabelBorderSides.All
            label.BorderStyle = Border3DStyle.SunkenOuter
            label.Width = CInt(0.13 * .Width)
            label.TextAlign = ContentAlignment.MiddleCenter

            ' Initializes the panel 6 (date and time)
            label = CType(.Items.Item(6), ToolStripStatusLabel)
            label.AutoSize = False
            label.BorderSides = ToolStripStatusLabelBorderSides.All
            label.BorderStyle = Border3DStyle.SunkenOuter
            label.Width = CInt(0.13 * .Width)
            label.TextAlign = ContentAlignment.MiddleCenter
        End With
        ' Update the status bar
        mStatusBar.Update(statusBar)
    End Sub



    Public Sub Update(ByRef statusBar As StatusStrip)
        With statusBar
            ' Update the panel 0 (username)
            If (mUserManager.CurrentUsername() <> "") Then
                .Items.Item(0).Text = "Username: " & mUserManager.CurrentUsername()
            Else
                .Items.Item(0).Text = "Username: unknown"
            End If
            ' Update the panel 1 (access level)
            .Items.Item(1).Text = "Access level: " & mUserManager.CurrentAccessLevel.ToString
            ' Update the panel 2 (scan times)
            .Items.Item(2).Text = String.Format("WS02 scan time: {0} / {1} ms", mWS02Main.ScanTime.ToString("0.0"), mWS02Main.MaxScanTime.ToString("0.0"))
            ' Update the panel 2 (scan times)
            .Items.Item(3).Text = String.Format("WS03 scan time: {0} / {1} ms", mWS03Main.ScanTime.ToString("0.0"), mWS03Main.MaxScanTime.ToString("0.0"))
            ' Update the panel 2 (scan times)
            .Items.Item(4).Text = String.Format("WS04 scan time: {0} / {1} ms", mWS04Main.ScanTime.ToString("0.0"), mWS04Main.MaxScanTime.ToString("0.0"))
            ' Update the panel 3 (scan times)
            .Items.Item(5).Text = String.Format("WS05 scan time: {0} / {1} ms", mWS05Main.ScanTime.ToString("0.0"), mWS05Main.MaxScanTime.ToString("0.0"))
            ' Updates  the panel 3 (date and time)
            .Items.Item(6).Text = Format(Date.Now, "dd/MM/yyyy, HH:mm:ss")
        End With
    End Sub



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
End Module
