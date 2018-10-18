Imports System.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports FM
Imports FM.WebSync

Public Class UpdatesForm

    Private Sub UpdatesForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        My.MySettings.Default.Upgrade()
        'ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertficate)

        Try
#If DEBUG Then
            DBW = New FFASuiteDataService.ARTBEntities(New Uri(My.Settings.ServicePathDebug))
#Else
            DBW = New FFASuiteDataService.ARTBEntities(New Uri(My.Settings.ServicePathLive))
#End If

        Catch ex As Exception
            MsgError(Me, "Could not initialize web database", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Me.Close()
        End Try

        DBW.IgnoreResourceNotFoundException = True

        If My.Settings.CheckBoxConnection = 1 Then 'external access then
            Me.radio_EXTERNAL.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
            radio_EXTERNAL_ToggleStateChanged(Me, New Telerik.WinControls.UI.StateChangedEventArgs(Telerik.WinControls.Enumerations.ToggleState.On))
        ElseIf My.Settings.CheckBoxConnection = 0 Then 'internal access
            Me.radio_BRS.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On
            radio_BRS_ToggleStateChanged(Me, New Telerik.WinControls.UI.StateChangedEventArgs(Telerik.WinControls.Enumerations.ToggleState.On))
        End If

        If My.Settings.RememberMe = True Then
            Exit Sub
        End If

        Dim GoAhead As Boolean = True
        'Dim myFP As String = FP.FINGER_PRINT
        'Dim authority = (From q In DBW.ARTBOPTCALC_FINGERPRINTS _
        '                 Where q.PRODUCT_ID = My.Settings.PRODUCT_ID _
        '                 And q.FINGER_PRINT = myFP _
        '                 Select q).SingleOrDefault
        'If IsNothing(authority) Then
        '    GoAhead = False
        'Else
        '    If authority.UPDATER = False Then
        '        GoAhead = False
        '    End If
        'End If

        'If GoAhead = False Then
        '    Dim nf As New LoginForm
        '    nf.ShowDialog(Me)
        '    If nf.AuthenticationResult = False Then
        '        MsgError(Me, "You are not authorised to use this application", "Invalid Credentials", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
        '        Me.Close()
        '        Application.Exit()
        '    End If
        'End If

    End Sub

    Private Sub rbtn_SMS_Click(sender As Object, e As EventArgs) Handles rbtn_SMS.Click
        For Each af As Form In Application.OpenForms
            If af.Name = "SMSMainForm" Then
                Beep()
                af.Activate()
                Exit Sub
            End If
        Next
        Dim nf As New SMSMainForm
        nf.ShowDialog(Me)
    End Sub

    Private Sub rbtn_SGX_Click(sender As Object, e As EventArgs) Handles rbtn_SGX.Click
        For Each af As Form In Application.OpenForms
            If af.Name = "ArtBNSGXServiceUpdate" Then
                Beep()
                af.Activate()
                Exit Sub
            End If
        Next
        Dim nf As New ArtBNSGXServiceUpdate
        nf.ShowDialog(Me)
    End Sub

    Private Sub rbtn_Baltic_Click(sender As Object, e As EventArgs) Handles rbtn_Baltic.Click
        For Each af As Form In Application.OpenForms
            If af.Name = "BalticFTP" Then
                Beep()
                af.Activate()
                Exit Sub
            End If
        Next
        Dim nf As New BalticFTP
        If rcb_UPDATE_BRS.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
            nf.UpdateLocalDB = True
        End If
        nf.ShowDialog(Me)
    End Sub

    Private Sub radio_BRS_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles radio_BRS.ToggleStateChanged
        If radio_BRS.IsChecked = True Then
            My.Settings.CheckBoxConnection = 0
            My.Settings.Save()
            My.Settings.SetUserOverride("ConnectionString_BRS2", My.Settings.ConnectionString_BRS)
        End If
    End Sub

    Private Sub radio_EXTERNAL_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles radio_EXTERNAL.ToggleStateChanged
        If radio_EXTERNAL.IsChecked = True Then
            My.Settings.CheckBoxConnection = 1
            My.Settings.Save()
            My.Settings.SetUserOverride("ConnectionString_BRS2", My.Settings.ConnectionString_EXTERNAL)
        End If
    End Sub

    Private Sub rbtn_TestWS_Click(sender As Object, e As EventArgs) Handles rbtn_TestWS.Click
        Dim newspots As New List(Of FFAOptCalcService.SpotFixingsClass)
        Try
            newspots.AddRange(SDB.SpotFixings())
            If newspots.Count > 0 Then
                Dim p As Publication = New Publisher(My.Settings.ws_ServerLive).Publish(My.Settings.ws_TradesChannel, Json.Serialize(newspots), DataContracts.MessageEnum.SpotRatesUpdate)
                If p.Successful = False Then
                    MsgError(Me, "Failed to publish alert for spot rates update." & Environment.NewLine & "No action necessary, press OK to continue.", "Alert: No action necessary", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
                End If
            End If
        Catch ex As Exception
            MsgError(Me, "Failed to publish alert for spot rates update." & Environment.NewLine & "No action necessary, press OK to continue.", "Alert: No action necessary", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
        End Try
    End Sub
End Class
