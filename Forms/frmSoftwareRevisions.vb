Option Explicit On
Option Strict On

Public Class frmSoftwareRevisions
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



    '+------------------------------------------------------------------------------+
    '|                               Private methods                                |
    '+------------------------------------------------------------------------------+
    Private Sub frmSoftwareRevisions_Activated(sender As Object, e As System.EventArgs) Handles Me.Activated
        ' Clear the selection
        tbSoftwareRevisions.SelectionLength = 0
    End Sub



    Private Sub frmSoftwareRevisions_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        ' Show the software revisions

        tbSoftwareRevisions.Text = tbSoftwareRevisions.Text & vbCrLf &
                            "----------------------------------------------------------------------------------------------------" & vbCrLf &
                            "Revision number  : " & mConstants.SoftwareVersion & "" & vbCrLf &
                            "Date (dd/mm/yyyy): " & mConstants.SoftwareDate & vbCrLf &
                            "Author           : Ing. David Graveline Valeo" & vbCrLf &
                            "Description      : first release."

    End Sub

    Private Sub tbSoftwareRevisions_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbSoftwareRevisions.TextChanged

    End Sub
End Class