Public Class MarketRename

    Private m_OldFormName As String

    Public Property FormName As String
        Get
            Return rtb_FORM_NAME.Text.Trim(" ")
        End Get
        Set(value As String)
            m_OldFormName = value
            rtb_FORM_NAME.Text = value
        End Set
    End Property

    Private Sub rbtn_OK_Click(sender As Object, e As EventArgs) Handles rbtn_OK.Click
        If FormName = "" Then
            MsgError(Me, "Form name cannot be blank.", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If
        If m_OldFormName = FormName Then
            Me.DialogResult = Windows.Forms.DialogResult.Ignore
        Else
            Dim found = (From q In g_MarketViews.VIEWS Where q.NAME <> m_OldFormName _
                         And q.NAME = FormName Select q).FirstOrDefault
            If IsNothing(found) = True Then
                Me.DialogResult = Windows.Forms.DialogResult.OK
            Else
                MsgError(Me, "Form name already defined, use a unique name.", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Exit Sub
            End If
        End If
        Me.Close()
    End Sub

    Private Sub rbtn_Cancel_Click(sender As Object, e As EventArgs) Handles rbtn_Cancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Timer_CLose_Tick(sender As Object, e As EventArgs) Handles Timer_CLose.Tick
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub MarketRename_Load(sender As Object, e As EventArgs) Handles Me.Load
        Timer_CLose.Start()
    End Sub
End Class
