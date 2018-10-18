Imports Telerik.WinControls.UI

Public Class BRSClientsSMS

    Private SMS As New SMS_COMAPILib.SMS
    Private SessionID As String

    Private Sub SMSCLIENTSBindingNavigatorSaveItem_Click(sender As Object, e As EventArgs) Handles SMSCLIENTSBindingNavigatorSaveItem.Click
        Me.Validate()
        rgv_SMS.EndEdit()
        Me.SMSCLIENTSBindingSource.EndEdit()
        Me.TableAdapterManager.UpdateAll(Me.BRSDataSet)
    End Sub

    Private Sub BRSClientsSMS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'BRSDataSet.SMSPROVIDERS' table. You can move, or remove it, as needed.
        Me.SMSPROVIDERSTableAdapter.Fill(Me.BRSDataSet.SMSPROVIDERS)
        'TODO: This line of code loads data into the 'BRSDataSet.SMSCLIENTS' table. You can move, or remove it, as needed.
        Me.SMSCLIENTSTableAdapter.Fill(Me.BRSDataSet.SMSCLIENTS)

        Try
            SessionID = SMS.Authenticate(My.Settings.ClickATell_API_ID, My.Settings.ClickATell_UserName, My.Settings.ClickATell_Password)
            SMS.smsFROM = My.Settings.ClickATell_SMSFrom
        Catch ex As Exception
        End Try
    End Sub

    Private Sub rgv_SMS_CellValueChanged(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles rgv_SMS.CellValueChanged
        If rgv_SMS.Columns(e.ColumnIndex).FieldName = "MOBILE" Then
            Dim mbl As String = e.Value
            Dim response As String
            Try
                response = SMS.QueryCoverage(mbl)
                If response.Substring(0, 2) <> "OK" Then
                    MsgError(Me, "Please check this number, it is either wrong or for a network where coverage is not provided for.", "Wrong Number?", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
                End If
            Catch ex As Exception
                MsgError(Me, "Failed to check number validity", "Connection Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
            End Try
        End If
    End Sub

    Private Sub rgv_SMS_DataError(sender As Object, e As Telerik.WinControls.UI.GridViewDataErrorEventArgs) Handles rgv_SMS.DataError
        If e.Context = Telerik.WinControls.UI.GridViewDataErrorContexts.Commit Then
            MsgError(Me, "This mobile number already exists, duplication is not allowed", "Duplicate Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            e.Cancel = True
        End If
    End Sub

    Private Sub rgv_SMS_ViewCellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles rgv_SMS.ViewCellFormatting
        If IsNothing(e.CellElement.ColumnInfo) Then Exit Sub
        If e.Column.IsVisible = False Then Exit Sub

        If TypeOf e.CellElement Is GridHeaderCellElement Then
            e.CellElement.Font = New Font(rgv_SMS.TableElement.Font.Name, rgv_SMS.TableElement.Font.Size, FontStyle.Bold)
        End If
    End Sub
End Class
