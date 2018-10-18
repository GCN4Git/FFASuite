Imports Telerik.WinControls.UI

Public Class UpdaterSettings
    Private store As New RadPropertyStore

    Private ClickATell_UserName As PropertyStoreItem
    Private ClickATell_Password As PropertyStoreItem
    Private ClickATell_API_ID As PropertyStoreItem
    Private ClickATell_SMSFrom As PropertyStoreItem

    Private TxtLocal_UserName As PropertyStoreItem
    Private TxtLocal_Password As PropertyStoreItem
    Private TxtLocalSender As PropertyStoreItem

    Private SMS_SafetyBalance As PropertyStoreItem

    Private Sub UpdaterSettings_Load(sender As Object, e As EventArgs) Handles Me.Load
        Me.rad_PROPERTIES.PropertySort = PropertySort.Categorized

        SMS_SafetyBalance = New PropertyStoreItem(GetType(Integer), "SMS Safety Credits Balance", My.Settings.SMS_SafetyBalance, "Define alert thresold", "SMS Credits", False)

        ClickATell_API_ID = New PropertyStoreItem(GetType(String), "ClickATell API_ID", My.Settings.ClickATell_API_ID, "ClickATel API ID supplied to you", "Provider ClickATel", False)
        ClickATell_UserName = New PropertyStoreItem(GetType(String), "ClickATell User Name", My.Settings.ClickATell_UserName, "ClickATel User Name", "Provider ClickATel", False)
        ClickATell_Password = New PropertyStoreItem(GetType(String), "ClickATell Password", My.Settings.ClickATell_Password, "ClickATel Password", "Provider ClickATel", False)
        ClickATell_SMSFrom = New PropertyStoreItem(GetType(String), "ClickATell SMS From", My.Settings.ClickATell_SMSFrom, "ClickATel SMS From", "Provider ClickATel", False)

        TxtLocal_UserName = New PropertyStoreItem(GetType(String), "TxtLocal User Name", My.Settings.TxtLocal_UserName, "TxtLocal User Name", "Provider TxtLocal", False)
        TxtLocal_Password = New PropertyStoreItem(GetType(String), "TxtLocal Password", My.Settings.TxtLocal_Password, "TxtLocal Password", "Provider TxtLocal", False)
        TxtLocalSender = New PropertyStoreItem(GetType(String), "TxtLocal SMS From", My.Settings.TxtLocalSender, "TxtLocal SMS From", "Provider TxtLocal", False)

        store.Add(SMS_SafetyBalance)

        store.Add(ClickATell_API_ID)
        store.Add(ClickATell_UserName)
        store.Add(ClickATell_Password)
        store.Add(ClickATell_SMSFrom)

        store.Add(TxtLocal_UserName)
        store.Add(TxtLocal_Password)
        store.Add(TxtLocalSender)

        Me.rad_PROPERTIES.SelectedObject = store
    End Sub

    Private Sub rbtn_SAVE_Click(sender As Object, e As EventArgs) Handles rbtn_SAVE.Click
        My.Settings.ClickATell_UserName = ClickATell_UserName.Value
        My.Settings.ClickATell_Password = ClickATell_Password.Value
        My.Settings.ClickATell_API_ID = ClickATell_API_ID.Value
        My.Settings.ClickATell_SMSFrom = ClickATell_SMSFrom.Value

        My.Settings.TxtLocal_UserName = TxtLocal_UserName.Value
        My.Settings.TxtLocal_Password = TxtLocal_Password.Value
        My.Settings.TxtLocalSender = TxtLocalSender.Value

        My.Settings.SMS_SafetyBalance = SMS_SafetyBalance.Value

        My.Settings.Save()
        Me.Close()
    End Sub

    Private Sub rbt_Cancel_Click(sender As Object, e As EventArgs) Handles rbt_Cancel.Click
        Me.Close()
    End Sub
End Class
