Public Class LoginForm

    Public AuthenticationResult As Boolean = False

    Private Sub OK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK.Click
        If PasswordTextBox.Text = "SMSBRS" Then
            If cb_REMEMBER.Checked Then
                My.Settings.RememberMe = True
            End If
            My.Settings.Save()
            AuthenticationResult = True
            Me.Close()
        Else
            MsgError(Me, "Invalid Password", "Authentication Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
        End If
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        AuthenticationResult = False
        Me.Close()
    End Sub

End Class
