Imports System.ComponentModel
Imports System.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Threading
Imports FM
Imports FM.WebSync
Imports Telerik.WinControls.UI
Imports FFASuiteUpdater.DataContracts

Public Class SMSMainForm

    Private SpotDiffs As New List(Of BALTIC_SPOT_RATES_DIFF)
    Private SMSPROV As New List(Of SMSProviderClass)
    Private SMSThread As Thread = Nothing

    Private Sub SMSMainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertficate)

        rdtp_DATE.Value = Today.Date
        RefreshLocalDB()
        preparecapessms()
        preparepmxsms()
        GetSMSBalances()
    End Sub

    Private Sub Activity(ByVal txt As String)
        Dim nr As New ListViewDataItem
        nr.Text = txt
        rlv_SMSActivity.Items.Add(nr)
        rlv_SMSActivity.Refresh()
    End Sub
#Region "STARTUP"
    Private Sub RefreshLocalDB()
        Dim DBL As New BRSDataContext(My.Settings.ConnectionString_BRS2)
        Dim tDateF As Date = rdtp_DATE.Value
        Dim tDateL As Date = tDateF.AddDays(1)
        Try
            SpotDiffs = (From q In DBL.BALTIC_SPOT_RATES_DIFF Where q.FIXING_DATE >= tDateF And q.FIXING_DATE < tDateL Select q).ToList
        Catch ex As Exception
        End Try
        DBL.Dispose()
    End Sub
#Region "SMS Prepare"
    Private Sub rbtn_PREPARE_CAPES_Click(sender As Object, e As EventArgs) Handles rbtn_PREPARE_CAPES.Click
        preparecapessms()
    End Sub
    Private Sub rbtn_PREPARE_PMX_Click(sender As Object, e As EventArgs) Handles rbtn_PREPARE_PMX.Click
        preparepmxsms()
    End Sub

    Private Sub preparecapessms()
        Dim str As String = ""

        Try
            Dim BDI = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.BDI Select q
            str = "BDI " & Format(BDI.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(BDI.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim BCI = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.BCI14 Select q
            str = str & "BCI " & Format(BCI.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(BCI.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C4TC = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C4TC Select q
            str = str & "C4TC " & Format(C4TC.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(C4TC.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C5TC = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C5TC Select q
            str = str & "C5TC " & Format(C5TC.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(C5TC.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C2 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C2 Select q
            str = str & "C2 " & Format(C2.FirstOrDefault.FIXING, DryRoutesFormat.Dec) & " " & Format(C2.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.DecSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C3 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C3 Select q
            str = str & "C3 " & Format(C3.FirstOrDefault.FIXING, DryRoutesFormat.Dec) & " " & Format(C3.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.DecSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C4 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C4 Select q
            str = str & "C4 " & Format(C4.FirstOrDefault.FIXING, DryRoutesFormat.Dec) & " " & Format(C4.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.DecSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C5 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C5 Select q
            str = str & "C5 " & Format(C5.FirstOrDefault.FIXING, DryRoutesFormat.Dec) & " " & Format(C5.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.DecSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C7 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C7 Select q
            str = str & "C7 " & Format(C7.FirstOrDefault.FIXING, DryRoutesFormat.Dec) & " " & Format(C7.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.DecSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C8_14 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C8_14 Select q
            str = str & "C8_14 " & Format(C8_14.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(C8_14.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C9_14 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C9_14 Select q
            str = str & "C9_14 " & Format(C9_14.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(C9_14.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C10_14 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C10_14 Select q
            str = str & "C10_14 " & Format(C10_14.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(C10_14.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C14 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C14 Select q
            str = str & "C14 " & Format(C14.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(C14.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C15 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C15 Select q
            str = str & "C15 " & Format(C15.FirstOrDefault.FIXING, DryRoutesFormat.Dec) & " " & Format(C15.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.DecSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C16 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C16 Select q
            str = str & "C16 " & Format(C16.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(C16.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim C17 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.C17 Select q
            str = str & "C17 " & Format(C17.FirstOrDefault.FIXING, DryRoutesFormat.Dec) & " " & Format(C17.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.DecSign) & vbCrLf
        Catch ex As Exception
        End Try

        'Try
        '    Dim P4TC = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.P4TC Select q
        '    str = str & "Pmx TC " & Format(P4TC.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(P4TC.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        'Catch ex As Exception
        'End Try

        'Try
        '    Dim S5TC = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S5TC Select q
        '    str = str & "Smx TC " & Format(S5TC.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S5TC.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        'Catch ex As Exception
        'End Try

        'Try
        '    Dim H6TC = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.H6TC Select q
        '    str = str & "Hsz TC " & Format(H6TC.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(H6TC.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign)
        'Catch ex As Exception
        'End Try

        rtb_SMS_CAPES.Text = str

    End Sub
    Private Sub preparepmxsms()
        Dim str As String = ""
        Try
            Dim BDI = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.BDI Select q
            str = "BDI " & Format(BDI.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(BDI.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim P4TC = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.P4TC Select q
            str = str & "P4TC " & Format(P4TC.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(P4TC.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try
        Try
            Dim K5TC = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.K5TC Select q
            str = str & "K5TC " & Format(K5TC.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(K5TC.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim P1A = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.P1A Select q
            str = str & "P1A " & Format(P1A.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(P1A.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim P2A = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.P2A Select q
            str = str & "P2A " & Format(P2A.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(P2A.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim P3A = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.P3A Select q
            str = str & "P3A " & Format(P3A.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(P3A.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim P4 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.P4 Select q
            str = str & "P4 " & Format(P4.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(P4.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim P5 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.P5 Select q
            str = str & "P5 " & Format(P5.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(P5.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim S6TC = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S6TC Select q
            str = str & "S6TC " & Format(S6TC.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S6TC.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim S10TC58 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S10TC58 Select q
            str = str & "S10TC58 " & Format(S10TC58.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S10TC58.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim S1B_58 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S1B_58 Select q
            str = str & "S1B_58 " & Format(S1B_58.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S1B_58.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim S1C_58 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S1C_58 Select q
            str = str & "S1C_58 " & Format(S1C_58.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S1C_58.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim S2_58 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S2_58 Select q
            str = str & "S2_58 " & Format(S2_58.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S2_58.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim S3_58 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S3_58 Select q
            str = str & "S3_58 " & Format(S3_58.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S3_58.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim S4A_58 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S4A_58 Select q
            str = str & "S4A_58 " & Format(S4A_58.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S4A_58.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim S4B_58 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S4B_58 Select q
            str = str & "S4B_58 " & Format(S4B_58.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S4B_58.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim S5_58 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S5_58 Select q
            str = str & "S5_58 " & Format(S5_58.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S5_58.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim S858 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S8_58 Select q
            str = str & "S8_58 " & Format(S858.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S858.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim S9_58 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S9_58 Select q
            str = str & "S9_58 " & Format(S9_58.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S9_58.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim S10_58 = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.S10_58 Select q
            str = str & "S10_58 " & Format(S10_58.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(S10_58.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try

        Try
            Dim H6TC = From q In SpotDiffs Where q.ROUTE_ID = DryRoutes.H6TC Select q
            str = str & "H6TC " & Format(H6TC.FirstOrDefault.FIXING, DryRoutesFormat.Fixed) & " " & Format(H6TC.FirstOrDefault.FIXING_DIFF, DryRoutesFormat.FixedSign) & vbCrLf
        Catch ex As Exception
        End Try
        rtb_SMS_PMX.Text = str
    End Sub

    Private Enum DryRoutes
        C2 = 3
        C3 = 4
        C4 = 5
        C5 = 6
        C7 = 7
        C8 = 8
        C8_14 = 160
        C9 = 9
        C9_14 = 161
        C10 = 10
        C10_14 = 162
        C11 = 11
        C12 = 12
        C14 = 159
        C15 = 154
        C16 = 156
        C17 = 1171

        BCI = 32
        BCI14 = 149
        C4TC = 36
        C5TC = 148
        P4TC = 37
        K5TC = 1176
        S6TC = 38
        S10TC58 = 1173
        H6TC = 39
        BDI = 40

        P1A = 13
        P2A = 14
        P3A = 15
        P4 = 16
        P5 = 153
        P1A_82 = 1188
        P2A_82 = 1189
        P3A_82 = 1190
        P4_82 = 1191
        P6_82 = 1192

        S1B_58 = 1234
        S1C_58 = 1235
        S2_58 = 1236
        S3_58 = 1237
        S4A_58 = 1238
        S4B_58 = 1239
        S5_58 = 1240
        S8_58 = 1185
        S9_58 = 1241
        S10_58 = 1186
        S11_58 = 1187
    End Enum
#End Region
    Private Sub GetSMSBalances()
        Dim DBL As New BRSDataContext(My.Settings.ConnectionString_BRS2)
        Dim query = (From q In DBL.SMSPROVIDERS Order By q.ID Select q).ToList
        DBL.Dispose()

        Dim SMS As New SMS_COMAPILib.SMS
        Dim SessionID As String
        Try
            SessionID = SMS.Authenticate(My.Settings.ClickATell_API_ID, My.Settings.ClickATell_UserName, My.Settings.ClickATell_Password)
            SMS.smsFROM = My.Settings.ClickATell_SMSFrom
        Catch ex As Exception
            MsgError(Me, "Falied to connect to ClickaTell", "Connection Failure", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
        End Try

        For Each r In query
            Dim nc As New SMSProviderClass
            nc.ID = r.ID
            nc.SMSPROVIDER = r.SMSPROVIDER
            If r.ID = SMSProvidersEnum.ClickATell Then
                Try
                    nc.SMSBALANCE = CInt(SMS.QueryBalance)
                Catch ex As Exception
                    nc.SMSBALANCE = -1
                End Try
            ElseIf r.ID = SMSProvidersEnum.TxtLocal Then
                Try
                    nc.SMSBALANCE = CInt(TxtLocal_Balance())
                Catch ex As Exception
                    nc.SMSBALANCE = -1
                End Try
            End If
            If nc.SMSBALANCE <= My.Settings.SMS_SafetyBalance Then
                rtb_SMSBALANCE.Text = "Running low on credits"
            End If
            SMSPROV.Add(nc)
        Next

        rgv_SMSPROVIDERS.DataSource = SMSPROV
        rgv_SMSPROVIDERS.Refresh()
    End Sub

#End Region

#Region "SENDSMS"
    Private Sub rbtn_SEND_CAPES_Click(sender As Object, e As EventArgs) Handles rbtn_SEND_CAPES.Click

        If rtb_SMS_CAPES.Text = "" Then
            Beep()
            MsgError(Me, "SMS for Capes contains no text! Do you want to continue?", "Alert", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
            Exit Sub
        End If

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

        Dim DBL As New BRSDataContext(My.Settings.ConnectionString_BRS2)
        Dim SMS As New SMS_COMAPILib.SMS
        Dim SessionID As String
        Dim response As String = String.Empty
        Try
            SessionID = SMS.Authenticate(My.Settings.ClickATell_API_ID, My.Settings.ClickATell_UserName, My.Settings.ClickATell_Password)
            SMS.smsFROM = My.Settings.ClickATell_SMSFrom
        Catch ex As Exception
            Dim answ = MsgError(Me, "Falied to connect to ClickaTell", "Connection Failure", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
        End Try

        Me.Cursor = Cursors.WaitCursor

        DBL.ExecuteCommand("update SMSCLIENTS set RESPONSE='none' where CAPES=1")
        Dim smslist = (From q In DBL.SMSCLIENTS Where q.CAPES = True Order By q.MOBILE Select q)
        pb_SMS.Value = 0
        pb_SMS.Minimum = 0
        pb_SMS.Maximum = smslist.Count
        pb_SMS.Step = 1
        Activity("... started message sending")
        For Each s In smslist
            Select Case s.SMSPROVIDERID
                Case SMSProvidersEnum.ClickATell
                    SMS.smsMSG_TYPE = SMS_COMAPILib.eSMSMsgType.SMS_TYPE_TEXT
                    If s.SENTSIPMPLESMS = True Then
                        'split msg to two for clients with no suport for concantenated SMS messages
                        Dim iM2 As Integer = rtb_SMS_CAPES.Text.IndexOf("C5 ")
                        Dim sM1 As String = rtb_SMS_CAPES.Text.Substring(0, iM2 - 1)
                        Dim sM2 As String = rtb_SMS_CAPES.Text.Substring(iM2, rtb_SMS_CAPES.Text.Length - iM2)
                        SMS.smsCONCAT = SMS_COMAPILib.eSMSConcatType.SMS_CONCAT_1_MSG
                        response = SMS.SendMsg(True, sM1, s.MOBILE)
                        response = SMS.SendMsg(True, sM2, s.MOBILE)                        
                    Else
                        If rtb_SMS_CAPES.Text.Length > 160 Then
                            SMS.smsCONCAT = SMS_COMAPILib.eSMSConcatType.SMS_CONCAT_2_MSG
                        Else
                            SMS.smsCONCAT = SMS_COMAPILib.eSMSConcatType.SMS_CONCAT_1_MSG
                        End If
                        response = SMS.SendMsg(True, rtb_SMS_CAPES.Text, s.MOBILE)
                    End If
                    If response.Substring(0, 2) <> "ID" Then
                        Activity("ClickATell: send to " & s.MOBILE & " failed")
                    End If
                Case SMSProvidersEnum.TxtLocal
                    If s.SENTSIPMPLESMS = True Then
                        'split msg to two for clients with no suport for concantenated SMS messages
                        Dim iM2 As Integer = rtb_SMS_CAPES.Text.IndexOf("C5 ")
                        Dim sM1 As String = rtb_SMS_CAPES.Text.Substring(0, iM2 - 1)
                        Dim sM2 As String = rtb_SMS_CAPES.Text.Substring(iM2, rtb_SMS_CAPES.Text.Length - iM2)
                        response = TxtLocal_SendSMS(False, My.Settings.TxtLocalSender, sM1, s.MOBILE, "")
                        response = TxtLocal_SendSMS(False, My.Settings.TxtLocalSender, sM2, s.MOBILE, "")
                    Else
                        response = TxtLocal_SendSMS(False, My.Settings.TxtLocalSender, rtb_SMS_CAPES.Text, s.MOBILE, "")
                    End If

                    If response.Contains("MessageReceived") = False Then
                        Activity("TxtLocal: send to " & s.MOBILE & " failed")
                    End If
            End Select
            s.RESPONSE = response
            pb_SMS.PerformStep()
        Next
        DBL.SubmitChanges()
        DBL.Dispose()

        Me.Cursor = Cursors.Default
        Beep()
        Activity("... completed message sending")
        MsgError(Me, "Capes SMS's have been sent, please read error box for possible misses.", "Alert", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
    End Sub
    Private Sub rbtn_SEND_PMX_Click(sender As Object, e As EventArgs) Handles rbtn_SEND_PMX.Click

        If rtb_SMS_PMX.Text = "" Then
            Beep()
            MsgError(Me, "SMS for PMX contains no text! Do you want to continue?", "Alert", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
            Exit Sub
        End If

        Dim newspots As New List(Of FFAOptCalcService.SpotFixingsClass)
        Try
            newspots.AddRange(SDB.SpotFixings())
            If newspots.Count > 0 Then
                Dim p As Publication = New Publisher(My.Settings.ws_ServerLive).Publish(My.Settings.ws_TradesChannel, Json.Serialize(newspots), DataContracts.MessageEnum.SpotRatesUpdate)
            End If
        Catch ex As Exception
            MsgError(Me, "Failed to publish alert for spot rates update.", "Alert: No action necessary", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
        End Try

        Dim DBL As New BRSDataContext(My.Settings.ConnectionString_BRS2)

        Dim SMS As New SMS_COMAPILib.SMS
        Dim SessionID As String
        Dim response As String = String.Empty
        Try
            SessionID = SMS.Authenticate(My.Settings.ClickATell_API_ID, My.Settings.ClickATell_UserName, My.Settings.ClickATell_Password)
            SMS.smsFROM = My.Settings.ClickATell_SMSFrom
        Catch ex As Exception
            Dim answ = MsgError(Me, "Falied to connect to ClickaTell", "Connection Failure", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
        End Try

        Me.Cursor = Cursors.WaitCursor
        DBL.ExecuteCommand("update SMSCLIENTS set RESPONSE='none' where PMX=1")
        Dim smslist = (From q In DBL.SMSCLIENTS Where q.PMX = True Order By q.MOBILE Select q)
        pb_SMS.Value = 0
        pb_SMS.Minimum = 0
        pb_SMS.Maximum = smslist.Count
        pb_SMS.Step = 1
        Activity("starting sending messages.....")
        For Each s In smslist
            Select Case s.SMSPROVIDERID
                Case SMSProvidersEnum.ClickATell
                    SMS.smsMSG_TYPE = SMS_COMAPILib.eSMSMsgType.SMS_TYPE_TEXT
                    If s.SENTSIPMPLESMS = True Then
                        'split msg to two for clients with no suport for concantenated SMS messages
                        Dim iM2 As Integer = rtb_SMS_PMX.Text.IndexOf("S2 ")
                        Dim sM1 As String = rtb_SMS_PMX.Text.Substring(0, iM2 - 1)
                        Dim sM2 As String = rtb_SMS_PMX.Text.Substring(iM2, rtb_SMS_PMX.Text.Length - iM2)
                        SMS.smsCONCAT = SMS_COMAPILib.eSMSConcatType.SMS_CONCAT_1_MSG
                        response = SMS.SendMsg(True, sM1, s.MOBILE)
                        response = SMS.SendMsg(True, sM2, s.MOBILE)
                    Else
                        If rtb_SMS_PMX.Text.Length > 160 Then
                            SMS.smsCONCAT = SMS_COMAPILib.eSMSConcatType.SMS_CONCAT_2_MSG
                        Else
                            SMS.smsCONCAT = SMS_COMAPILib.eSMSConcatType.SMS_CONCAT_1_MSG
                        End If
                        response = SMS.SendMsg(True, rtb_SMS_PMX.Text, s.MOBILE)
                    End If                    
                    If response.Substring(0, 2) <> "ID" Then
                        Activity("ClickATell: send to " & s.MOBILE & " failed")
                    End If
                Case SMSProvidersEnum.TxtLocal
                    If s.SENTSIPMPLESMS = True Then
                        'split msg to two for clients with no suport for concantenated SMS messages
                        Dim iM2 As Integer = rtb_SMS_PMX.Text.IndexOf("S2 ")
                        Dim sM1 As String = rtb_SMS_PMX.Text.Substring(0, iM2 - 1)
                        Dim sM2 As String = rtb_SMS_PMX.Text.Substring(iM2, rtb_SMS_PMX.Text.Length - iM2)
                        response = TxtLocal_SendSMS(False, My.Settings.TxtLocalSender, sM1, s.MOBILE, "")
                        response = TxtLocal_SendSMS(False, My.Settings.TxtLocalSender, sM2, s.MOBILE, "")
                    Else
                        response = TxtLocal_SendSMS(False, My.Settings.TxtLocalSender, rtb_SMS_PMX.Text, s.MOBILE, "")
                    End If
                    If response.Contains("MessageReceived") = False Then
                        Activity("TxtLocal: send to " & s.MOBILE & " failed")
                    End If
            End Select
            s.RESPONSE = response
            pb_SMS.PerformStep()
        Next
        DBL.SubmitChanges()
        DBL.Dispose()

        Me.Cursor = Cursors.Default
        Beep()
        Activity("... completed PMX message sending")
        MsgError(Me, "PMX SMS's have been sent, please read error box for possible misses.", "Alert", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
    End Sub

#End Region

    Private Sub rbtn_SMSCLIENTS_Click(sender As Object, e As EventArgs) Handles rbtn_SMSCLIENTS.Click
        For Each af As Form In Application.OpenForms
            If af.Name = "BRSClientsSMS" Then
                Beep()
                af.Activate()
                Exit Sub
            End If
        Next
        Dim nf As New BRSClientsSMS
        nf.ShowDialog(Me)
    End Sub
    Private Sub rgv_SMSPROVIDERS_ViewCellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles rgv_SMSPROVIDERS.ViewCellFormatting
        If IsNothing(e.CellElement.ColumnInfo) Then Exit Sub
        If e.Column.IsVisible = False Then Exit Sub

        If TypeOf e.CellElement Is GridHeaderCellElement Then
            e.CellElement.Font = New Font(rgv_SMSPROVIDERS.TableElement.Font.Name, rgv_SMSPROVIDERS.TableElement.Font.Size, FontStyle.Bold)
        ElseIf TypeOf e.CellElement Is Telerik.WinControls.UI.GridDataCellElement And e.Column.IsVisible = True Then
            If e.CellElement.ColumnInfo.Name = "SMSBALANCE" Then
                If e.CellElement.Value <= My.Settings.SMS_SafetyBalance Then
                    e.CellElement.Font = New Font(rgv_SMSPROVIDERS.TableElement.Font.Name, rgv_SMSPROVIDERS.TableElement.Font.Size, FontStyle.Bold)
                    e.CellElement.ForeColor = Color.Red
                End If
            End If
        End If
    End Sub

    Private Sub rbtn_SMS_SETTINGS_Click(sender As Object, e As EventArgs) Handles rbtn_SMS_SETTINGS.Click
        Dim f As New UpdaterSettings
        f.ShowDialog(Me)
    End Sub

    Private Sub rbtn_TEST_Click(sender As Object, e As EventArgs) Handles rbtn_TEST.Click
        Dim SMS As New SMS_COMAPILib.SMS
        Dim SessionID As String
        Dim response As String = String.Empty
        Try
            SessionID = SMS.Authenticate(My.Settings.ClickATell_API_ID, My.Settings.ClickATell_UserName, My.Settings.ClickATell_Password)
            SMS.smsFROM = My.Settings.ClickATell_SMSFrom
        Catch ex As Exception
            Dim answ = MsgError(Me, "Falied to connect to ClickaTell", "Connection Failure", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
        End Try

        SMS.smsMSG_TYPE = SMS_COMAPILib.eSMSMsgType.SMS_TYPE_TEXT
        If rtb_SMS_CAPES.Text.Length > 160 Then
            SMS.smsCONCAT = SMS_COMAPILib.eSMSConcatType.SMS_CONCAT_2_MSG
        Else
            SMS.smsCONCAT = SMS_COMAPILib.eSMSConcatType.SMS_CONCAT_1_MSG
        End If
        response = SMS.SendMsg(True, rtb_SMS_CAPES.Text, rtb_TEST.Text)
        If response.Substring(0, 2) <> "ID" Then
            MsgError(Me, "Falied to send SMS", "SMS FAilure", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
        End If
    End Sub
End Class
