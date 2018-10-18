Imports Microsoft.Win32
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.IO
Imports Telerik.WinControls.UI
Imports ArtB_Class_Library

Public Class FirstTimeUserForm

    Private m_InstComp As New List(Of InstalledComputersClass)
    Public Property InstComp As List(Of InstalledComputersClass)
        Get
            Return m_InstComp
        End Get
        Set(value As List(Of InstalledComputersClass))
            m_InstComp = value
        End Set
    End Property
    Private m_EshopList As New List(Of EshopListClass)
    Private m_FreezeEshop As Boolean = True

    Private Sub FirstTimeUserForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        My.MySettings.Default.Upgrade()
        Me.Icon = My.Resources.ArtB_Robots__48X48

        Step2.Close()
        Step4.Close()
        Step5.Close()

        rbtn_step1_Left.Text = "Check Connection"
        rbtn_step1_Right.Visible = False

        rl_step1_2.Text = "Welcome to the FFAs Suite Application. In order to proceed with first time installation"
        rl_step1_2.Text += " your computer needs to be connected to the internet. Please wait while the application performs some tests"

        rl_step2_1.Text = "It appears that your computer is sitting behing a proxy server to connect to the internet."
        rl_step2_1.Text = rl_step2_1.Text & " Do you want to set up the proxy parameters in order for the application to be able to connect to the intenet?"
        rl_step2_1.Text = rl_step2_1.Text & vbCrLf & vbCrLf & "If you do not know the details of the proxy configuaration please consult your EDP Help Desk to assist you. If you do not wish to continue further at this stage press the cancel button to exit the application."

        eshop_lblDigitalRiver.Text += vbCrLf & "After payment is finalized press exit button, and restart the application."
        eshop_lblDigitalRiver.Text += vbCrLf & "An email will follow with your purchase details"

        Version.Text = System.String.Format(Version.Text, My.Application.Info.Version.Major, My.Application.Info.Version.Minor)
        CopyRight.Text = My.Application.Info.Copyright
        Company.Text = "HiPath Systems"

        g_msgServiceType = My.Settings.msgServiceType
        UD = New FFAOptCalcService.FingerPrintClass
        UD.License = New FFAOptCalcService.ARTBOPTCALC_LICENSES
        UD.FingerPrints = New List(Of FFAOptCalcService.ARTBOPTCALC_FINGERPRINTS)
        UD.PRODUCT = New FFAOptCalcService.ARTBOPTCALC_PRODUCTS
        UD.MYFINGERPRINT = New FFAOptCalcService.ARTBOPTCALC_FINGERPRINTS

        UD.MYFINGERPRINT.FINGER_PRINT = FP.FINGER_PRINT
        UD.MYFINGERPRINT.COMPUTER_NAME = System.Environment.MachineName
        rtb_FINGERPRINT.Text = UD.MYFINGERPRINT.FINGER_PRINT
        UD.PRODUCT_ID = My.Settings.PRODUCT_ID

        bs_rgv.DataSource = m_InstComp
        rgv_InstalledComputers.DataSource = bs_rgv

        'My.Settings.MarketViews = ""
        g_MarketViews.Load()
        FirstTimeRun()

    End Sub

#Region "FirstTimeRun"
    Private Sub FirstTimeRun()
        rsb_WEBWait.StartWaiting()
        bw_InternetConnection.RunWorkerAsync()
    End Sub
    Private Sub bw_InternetConnection_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bw_InternetConnection.DoWork
        Dim answ As New BWTypeClass

        'TEST 1: Chech if interenet connection is available in general
        bw_InternetConnection.ReportProgress(1)
        answ.Assignment = BWEnum.CheckInternetConnection
        answ.Result = CheckInternetConnection()
        If answ.Result = FFAOptCalcService.FPStatusEnum.NoInternetConnection Then
            e.Result = answ
            Exit Sub
        End If

        'TEST2: Check if my service is available
        bw_InternetConnection.ReportProgress(2)
        answ.Assignment = BWEnum.CheckServiceStatus
        WEB = New ArtBOptDataClass
        Dim swfstatus = WEB.VerifyServiceStatus
        If swfstatus = WCFServiceStatusEnum.UserWantsToExitApplication Then
            answ.Result = FFAOptCalcService.FPStatusEnum.ServiceError
            e.Result = answ
            Exit Sub
        End If

        'TEST3: Check Credentials
        bw_InternetConnection.ReportProgress(3)
        answ.Assignment = BWEnum.CheckCredentials
        SERVER_DATE = WEB.SDB.ServerDate
        Try
            UD = WEB.SDB.CheckCredentials(UD)
        Catch ex As Exception
            Stop
        End Try

        answ.Result = UD.FPStatus
        If answ.Result <> FFAOptCalcService.FPStatusEnum.ExistingClientAllOK Then
            e.Result = answ
            Exit Sub
        End If
        Dim zip As New XMPPZipClass
        UD.OFPswd = zip.Decompress(UD.OFPswd)
        e.Result = answ

        bw_InternetConnection.ReportProgress(4)
        TRADE_CLASS.AddRange(WEB.SDB.GetTradeClases)
        bw_InternetConnection.ReportProgress(5)
        VESSEL_CLASSES.AddRange(WEB.SDB.GetVesselClases)
        bw_InternetConnection.ReportProgress(6)
        ROUTES.AddRange(WEB.SDB.GetRoutes)
        bw_InternetConnection.ReportProgress(7)
        INTEREST_RATES.AddRange(WEB.SDB.InterestRates)
        bw_InternetConnection.ReportProgress(8)
        PUBLIC_HOLIDAYS.AddRange(WEB.SDB.GetHolidays)

        bw_InternetConnection.ReportProgress(9)
        Dim routelist As New List(Of Integer)
        For Each r In g_MarketViews.DistinctRoutes
            routelist.Add(r)
        Next
        ROUTES_DETAIL_Add(WEB.SDB.ROUTE_DETAIL(routelist))

        bw_InternetConnection.ReportProgress(10)
        g_MVPeriods = WEB.SDB.GetMVPeriods(36, False)

        For Each r In routelist
            bw_InternetConnection.ReportProgress(r + 100000)
            Try
                FIXINGS_Add(WEB.SDB.SwapVolatility(r, My.Settings.DataPeriodDefault, My.Settings.DataVolDefault))
            Catch ex As Exception
#If DEBUG Then
                Stop
#End If
            End Try
        Next

    End Sub
    Private Sub bw_InternetConnection_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bw_InternetConnection.ProgressChanged
        Select Case e.ProgressPercentage
            Case 1
                rlv_WEBProgress.Items.Insert(0, New ListViewDataItem("Checking Web Connection ...."))
            Case 2
                rlv_WEBProgress.Items.Insert(0, New ListViewDataItem("Checking Service ...."))
            Case 3
                rlv_WEBProgress.Items.Insert(0, New ListViewDataItem("Checking Credentials ...."))
            Case 4
                rlv_WEBProgress.Items.Insert(0, New ListViewDataItem("Initializing Trade Classes ...."))
            Case 5
                rlv_WEBProgress.Items.Insert(0, New ListViewDataItem("Initializing Vessel Classes ...."))
            Case 6
                rlv_WEBProgress.Items.Insert(0, New ListViewDataItem("Initializing Routes ...."))
            Case 7
                rlv_WEBProgress.Items.Insert(0, New ListViewDataItem("Initializing Interest Rates ...."))
            Case 8
                rlv_WEBProgress.Items.Insert(0, New ListViewDataItem("Initializing Exchange Holidays ...."))
            Case 9
                rlv_WEBProgress.Items.Insert(0, New ListViewDataItem("Fetching Route Details ...."))
            Case 10
                rlv_WEBProgress.Items.Insert(0, New ListViewDataItem("Fetching Market View Format ...."))
            Case Is > 100000
                Dim r As Integer = e.ProgressPercentage - 100000
                Dim s As String = (From q In ROUTES Where q.ROUTE_ID = r Select q.ROUTE_SHORT).FirstOrDefault
                rlv_WEBProgress.Items.Insert(0, New ListViewDataItem("Fetching " & s & " data ...."))
        End Select
    End Sub
    Private Sub bw_InternetConnection_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bw_InternetConnection.RunWorkerCompleted
        Dim answ As BWTypeClass = TryCast(e.Result, BWTypeClass)
        rsb_WEBWait.StopWaiting()

        If answ.Assignment = BWEnum.CheckInternetConnection Then
            If answ.Result = FFAOptCalcService.FPStatusEnum.NoInternetConnection Then
                Dim msg As String = "Your PC is not connected to the internet." & vbCrLf & "Please resolve the problem and try again." & vbCrLf & "Application will now exit"
                MsgError(Me, msg, "WEB Connection Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Me.DialogResult = Windows.Forms.DialogResult.Abort
                Me.Close()
            ElseIf answ.Result = FFAOptCalcService.FPStatusEnum.ServiceError Then
                'put code to handle
                Me.DialogResult = Windows.Forms.DialogResult.Abort
                Me.Close()
            End If
        End If

        If answ.Assignment = BWEnum.CheckCredentials Then
            Select Case answ.Result
                Case FFAOptCalcService.FPStatusEnum.DemoFingerPrintHasExpired
                    Dim msg As String = "Your trial period has expired application will now exit. If you have a"
                    msg = msg & vbCrLf & "valid license key, or you wish to purchase a subscription please press OK."
                    Dim rslt = MsgError(Me, msg, "Trial period has expired", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info)
                    If rslt = Windows.Forms.DialogResult.Cancel Then
                        Me.DialogResult = Windows.Forms.DialogResult.Abort
                        Me.Close()
                        Exit Sub
                    Else
                        Step1.Close()
                        Step4.Show()
                        ConstructPCList()
                        Step5.Show()
                        ConstructEshop()
                        Step5.Close()
                        Step4.Show()
                        Exit Sub
                    End If
                Case FFAOptCalcService.FPStatusEnum.LiveLicenseHasExpiredNoFingerPrintAdded
                    Dim msg As String = "Your registration license period has expired application will now exit. If you"
                    msg = msg & vbCrLf & "have a valid license key, or you wish to renew you subscription please press OK."
                    Dim rslt = MsgError(Me, msg, "Registration has expired", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info)
                    If rslt = Windows.Forms.DialogResult.Cancel Then
                        Me.DialogResult = Windows.Forms.DialogResult.Abort
                        Me.Close()
                        Exit Sub
                    Else
                        Step1.Close()
                        Step4.Show()
                        ConstructPCList()
                        Step5.Show()
                        ConstructEshop()
                        Step5.Close()
                        Step4.Show()
                        Exit Sub
                    End If
                Case FFAOptCalcService.FPStatusEnum.UserWantsToRegisterPC
                    Dim msg As String = "Your PC appears to belong to a valid license registration but it is inactive."
                    msg = msg & vbCrLf & "Presse OK to proceed to re-register your PC."
                    Dim rslt = MsgError(Me, msg, "You need to activate your PC", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info)
                    If rslt = Windows.Forms.DialogResult.Cancel Then
                        Me.DialogResult = Windows.Forms.DialogResult.Abort
                        Me.Close()
                        Exit Sub
                    Else
                        Step1.Close()
                        Step4.Show()
                        ConstructPCList()
                        Step5.Show()
                        ConstructEshop()
                        Step5.Close()
                        Step4.Show()
                    End If
                Case FFAOptCalcService.FPStatusEnum.NewClient
                    Dim msg As String = "Your need to register your PC for first time use."
                    msg = msg & vbCrLf & "Presse OK to proceed, or cancel to quit application"
                    Dim rslt = MsgError(Me, msg, "You need to activate your PC", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info)
                    If rslt = Windows.Forms.DialogResult.Cancel Then
                        Me.DialogResult = Windows.Forms.DialogResult.Abort
                        Me.Close()
                        Exit Sub
                    Else
                        Step1.Close()
                        Step4.Show()
                        ConstructPCList()
                        Step5.Show()
                        ConstructEshop()
                        Step5.Close()
                        Step4.Show()
                    End If
                Case FFAOptCalcService.FPStatusEnum.DBError
                    Dim msg As String = "An unexpected application error occured. Please exit application"
                    msg = msg & vbCrLf & "and attempt to start over again."
                    Dim rslt = MsgError(Me, msg, "YUnexpected application error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
                    Me.DialogResult = Windows.Forms.DialogResult.Abort
                    Me.Close()
                    Exit Sub
                Case FFAOptCalcService.FPStatusEnum.ExistingClientAllOK
                    My.Settings.UserFingerPrint = UD.MYFINGERPRINT.FINGER_PRINT
                    My.Settings.Save()
                    Me.DialogResult = Windows.Forms.DialogResult.OK
                    'Connect to OF
                    FFAOptCalc.Show()
                    System.Threading.Thread.Sleep(1500)
                    Me.Close()
            End Select
        End If
    End Sub
#End Region

#Region "ConstructTabs"
    Private Sub ConstructPCList()
        m_InstComp.Clear()

        Dim MyPC = (From q In UD.FingerPrints Where q.FINGER_PRINT = UD.MYFINGERPRINT.FINGER_PRINT Select q).FirstOrDefault
        If IsNothing(MyPC) = True Then 'it means freshly registered PC
            UD.FingerPrints = New List(Of FFAOptCalcService.ARTBOPTCALC_FINGERPRINTS)
            Dim nfp As New FFAOptCalcService.ARTBOPTCALC_FINGERPRINTS
            nfp.FINGER_PRINT = UD.MYFINGERPRINT.FINGER_PRINT
            nfp.COMPUTER_NAME = UD.MYFINGERPRINT.COMPUTER_NAME
            nfp.PRODUCT_ID = My.Settings.PRODUCT_ID
            nfp.ACTIVE = False
            UD.FingerPrints.Add(nfp)
            UD.MYFINGERPRINT = nfp

            Dim npc As New InstalledComputersClass
            npc.NO = 1
            npc.COMPUTER = UD.MYFINGERPRINT.COMPUTER_NAME
            npc.FINGERPRINT = UD.MYFINGERPRINT.FINGER_PRINT
            npc.MYPC = True
            npc.ACTIVE = False
            npc.DELETE = False
            m_InstComp.Add(npc)
        Else
            Dim npc As New InstalledComputersClass
            npc.NO = 1
            npc.COMPUTER = UD.MYFINGERPRINT.COMPUTER_NAME
            npc.FINGERPRINT = UD.MYFINGERPRINT.FINGER_PRINT
            npc.MYPC = True
            npc.ACTIVE = MyPC.ACTIVE
            npc.DELETE = False
            m_InstComp.Add(npc)
        End If

        Dim PCs = (From q In UD.FingerPrints Where q.FINGER_PRINT <> UD.MYFINGERPRINT.FINGER_PRINT Order By q.COMPUTER_NAME Select q)
        Dim I As Integer = 2
        For Each r In PCs
            Dim npc As New InstalledComputersClass
            npc.NO = I
            npc.COMPUTER = r.COMPUTER_NAME
            npc.FINGERPRINT = r.FINGER_PRINT
            npc.MYPC = False
            npc.ACTIVE = r.ACTIVE
            npc.DELETE = False
            m_InstComp.Add(npc)
            I += 1
        Next
        bs_rgv.ResetBindings(False)
        rgv_InstalledComputers.Refresh()
        rgv_InstalledComputers.Rows(0).IsCurrent = True
        rgv_InstalledComputers.Rows(0).IsSelected = True
        If m_InstComp.Count > 1 Then
            rgv_InstalledComputers.Columns("ACTIVE").ReadOnly = False
        Else
            rgv_InstalledComputers.Columns("ACTIVE").ReadOnly = True
        End If

        If UD.License.LICENSE_KEY <> "" Then
            rmeb_License.Value = UD.License.LICENSE_KEY
            rmeb_Email.Value = UD.License.EMAIL
            se_NoLicenses.Value = UD.License.MAX_LICENSES
            se_LicensesUsed.Value = UD.License.USED_LICENSES
            rtb_LicenseExpiration.Text = FormatDateTime(UD.License.LICENSE_EXP_DATE, DateFormat.ShortDate)
            rtb_Organization.Text = UD.License.REG_NAME
            If UD.License.DEMO = True Then
                lbl_VERSION.Text = "Trial"
                lbl_VERSION.ForeColor = Color.Red
            Else
                lbl_VERSION.Text = "Registered"
                lbl_VERSION.ForeColor = Color.DarkGreen
            End If
        Else
            rmeb_Email.Value = Nothing
            rtb_Organization.Text = ""
            se_NoLicenses.Value = 0
            se_LicensesUsed.Value = 0
            rtb_LicenseExpiration.Text = ""
            lbl_VERSION.Text = "Trial"
            lbl_VERSION.ForeColor = Color.Red
        End If
    End Sub
    Private Sub ConstructEshop()
        m_FreezeEshop = True

        rgv_EshopPrices.DataSource = Nothing
        bs_eshop.DataSource = Nothing

        Select Case UD.PRODUCT.CCY_ID
            Case 978 'euro
                rgv_EshopPrices.Columns("PRICE").HeaderText = "Price (EUR)"
                eshop_lblTotalPrice.Text = "Total (EUR)"
            Case 840 'usd
                rgv_EshopPrices.Columns("PRICE").HeaderText = "Price (USD)"
                eshop_lblTotalPrice.Text = "Total (USD)"
        End Select

        Dim nc1 As New EshopListClass
        nc1.NOLICENSES = "Single"
        nc1.PRICE = UD.PRODUCT.BASE_PRICE
        m_EshopList.Add(nc1)

        If UD.PRODUCT.DISCOUNTS = True Then
            Dim nc2 As New EshopListClass
            nc2.NOLICENSES = "2 - " & UD.PRODUCT.DISC_TIER1
            nc2.PRICE = UD.PRODUCT.DISC_PRC1
            m_EshopList.Add(nc2)

            Dim nc3 As New EshopListClass
            nc3.NOLICENSES = UD.PRODUCT.DISC_TIER1 + 1 & " - " & UD.PRODUCT.DISC_TIER2
            nc3.PRICE = UD.PRODUCT.DISC_PRC2
            m_EshopList.Add(nc3)

            Dim nc4 As New EshopListClass
            nc4.NOLICENSES = UD.PRODUCT.DISC_TIER2 + 1 & " - " & UD.PRODUCT.DISC_TIER3
            nc4.PRICE = UD.PRODUCT.DISC_PRC3
            m_EshopList.Add(nc4)

            Dim nc5 As New EshopListClass
            nc5.NOLICENSES = "Over " & UD.PRODUCT.DISC_TIER3
            nc5.PRICE = UD.PRODUCT.DISC_PRC4
            m_EshopList.Add(nc5)
        End If

        bs_eshop.DataSource = m_EshopList
        rgv_EshopPrices.DataSource = bs_eshop
        rgv_EshopPrices.Refresh()

        eshop_LicenseKey.Value = rmeb_License.Value
        eshop_LicenseExpDate.Text = rtb_LicenseExpiration.Text
        eshop_NoLicenses.Value = se_NoLicenses.Value

        If UD.License.DEMO = True Then
            eshop_Registered.Text = "Trial"
            eshop_Registered.ForeColor = Color.Red
        Else
            eshop_Registered.Text = "Registered"
            eshop_Registered.ForeColor = Color.DarkGreen
        End If

        m_FreezeEshop = False
        eshop_NoLicenses_ValueChanged(Me, New System.EventArgs)
    End Sub
    Private Sub eshop_NoLicenses_ValueChanged(sender As Object, e As EventArgs) Handles eshop_NoLicenses.ValueChanged
        If m_FreezeEshop = True Then Exit Sub

        If IsNothing(UD.PRODUCT) Then Exit Sub

        If UD.PRODUCT.DISCOUNTS = False Then
            eshop_TotalPrice.Value = UD.PRODUCT.BASE_PRICE * eshop_NoLicenses.Value
            Exit Sub
        ElseIf UD.PRODUCT.DISCOUNTS = True Then
            Select Case eshop_NoLicenses.Value
                Case 1
                    eshop_TotalPrice.Value = UD.PRODUCT.BASE_PRICE * eshop_NoLicenses.Value
                    rgv_EshopPrices.Rows(0).IsCurrent = True
                    rgv_EshopPrices.Rows(0).IsSelected = True
                Case 2 To UD.PRODUCT.DISC_TIER1
                    eshop_TotalPrice.Value = UD.PRODUCT.DISC_PRC1 * eshop_NoLicenses.Value
                    rgv_EshopPrices.Rows(1).IsCurrent = True
                    rgv_EshopPrices.Rows(1).IsSelected = True
                Case UD.PRODUCT.DISC_TIER1 + 1 To UD.PRODUCT.DISC_TIER2
                    eshop_TotalPrice.Value = UD.PRODUCT.DISC_PRC2 * eshop_NoLicenses.Value
                    rgv_EshopPrices.Rows(2).IsCurrent = True
                    rgv_EshopPrices.Rows(2).IsSelected = True
                Case UD.PRODUCT.DISC_TIER2 + 1 To UD.PRODUCT.DISC_TIER3
                    eshop_TotalPrice.Value = UD.PRODUCT.DISC_PRC3 * eshop_NoLicenses.Value
                    rgv_EshopPrices.Rows(3).IsCurrent = True
                    rgv_EshopPrices.Rows(3).IsSelected = True
                Case Is > UD.PRODUCT.DISC_TIER3
                    eshop_TotalPrice.Value = UD.PRODUCT.DISC_PRC4 * eshop_NoLicenses.Value
                    rgv_EshopPrices.Rows(4).IsCurrent = True
                    rgv_EshopPrices.Rows(4).IsSelected = True
            End Select
        End If
    End Sub

    Private Sub eshop_ProceedToCheckOut_Click(sender As Object, e As EventArgs) Handles eshop_ProceedToCheckOut.Click
        Dim msg As String = "Press OK to close the application and be diverted from your system web browser to secure"
        msg += vbCrLf & "web site for payment processing. Press cancel to remain on this form if you beed to review your order"
        Dim answ = MsgError(Me, msg, "Web Payment Process", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info)
        If answ = Windows.Forms.DialogResult.OK Then
            OpenEshopSite("https://secure.shareit.com/shareit/cart.html?PRODUCT[300361434]=1&backlink=http%3A%2F%2Fwww.ag-software.de%2Fmatrix-xmpp-sdk%2Fpurchase%2F&cookies=1&showcart=1&js=-1")
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Me.Close()
        End If
        Exit Sub
    End Sub
    Private Sub eshop_btn_exit_Click(sender As Object, e As EventArgs) Handles eshop_btn_exit.Click
        Me.DialogResult = Windows.Forms.DialogResult.Abort
        Me.Close()
    End Sub
#End Region

    Private Sub rbtn_step1_Left_Click(sender As Object, e As EventArgs) Handles rbtn_step1_Left.Click
        Dim ConnectionState As ConnStateEnum = ConnStateEnum.CannotConnectAtAll

        'check if we can connect to the internet
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim d As Date = WEB.SDB.ServerDate
            ConnectionState = ConnStateEnum.DirectConnectionEstablished
            My.Settings.UsesProxy = False
        Catch ex As Exception
            ConnectionState = ConnStateEnum.DirectConnectionFailed
        End Try
        Me.Cursor = Cursors.Default
        rbtn_step1_Left.Enabled = True

        If ConnectionState = ConnStateEnum.DirectConnectionEstablished Then
            Dim answ = MsgError(Me, "Internet Connection Succeded. Press OK to move to next screen" & vbCrLf & "or press Cancel to abort application setup.", "Internet Connection Success", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info)
            If answ = Windows.Forms.DialogResult.Cancel Then
                Me.DialogResult = Windows.Forms.DialogResult.Abort
                Me.Close()
            ElseIf answ = Windows.Forms.DialogResult.OK Then
                Step1.Close()
                Step4.Show()
                SERVER_DATE = WEB.SDB.ServerDate
                UD = WEB.SDB.CheckCredentials(UD)
                ConstructPCList()
                ConstructEshop()
            Else
                Me.Cursor = Cursors.WaitCursor
                Step1.Close()
                Step2.Show()
                Try

                Catch ex As Exception
                    Me.Cursor = Cursors.Default
                    Dim msg As String = "Cannot connect to the internet. Make sure your computer is connected to the web."
                    msg += vbCrLf & vbCrLf & "Error: " & ex.Message
                    Dim answr = MsgError(Me, msg, "Internet Connection Error", MessageBoxButtons.RetryCancel, Telerik.WinControls.RadMessageIcon.Error)
                    If answr = Windows.Forms.DialogResult.Cancel Then
                        Me.DialogResult = Windows.Forms.DialogResult.Abort
                        Me.Close()
                    End If
                End Try
                Me.Cursor = Cursors.Default
            End If
        End If
    End Sub

#Region "TAB4_Events"
    Private Sub rbtn_RegisterPC_Click(sender As Object, e As EventArgs) Handles rbtn_RegisterPC.Click
        'check if email address is valid
        Dim valid As Boolean = TryCast(rmeb_Email.MaskedEditBoxElement.Provider, Telerik.WinControls.UI.EMailMaskTextBoxProvider).Validate(rmeb_Email.Text)
        If valid = False Or rmeb_Email.Text = "" Then
            'MsgError(Me, "Please enter a valid email address.", "Email Required", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            'Exit Sub
        End If

        'check case current pc not selected for initilization
        If rgv_InstalledComputers.Rows.Count > 1 Then
            For Each r In rgv_InstalledComputers.Rows
                If r.Cells("FINGERPRINT").Value = UD.MYFINGERPRINT.FINGER_PRINT Then
                    If r.Cells("ACTIVE").Value = False Then
                        MsgError(Me, "Your current PC is not selected for activation.", "Please check the active box to proceed.", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                        Exit Sub
                    End If
                End If
            Next
        End If
        'check if number of activated PCs exceeds number of available license
        Dim RequestedLicenses As Integer = 0
        For Each r In rgv_InstalledComputers.Rows
            If r.Cells("ACTIVE").Value = True Then
                RequestedLicenses += 1
            End If
        Next
        If RequestedLicenses > se_NoLicenses.Value Then
            Dim msg As String = "Number of activated PCs cannot exceed maximum number of available liceses."
            msg += vbCrLf & "Please deselect appropriate number of active PCs to comply with requirement."
            MsgError(Me, msg, "Error is requested PCs activation", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If

        'Copy User Personal Data
        'UD.License.EMAIL = rmeb_Email.Value

        Dim Abort As Boolean = False
        While Abort = False
            Me.Cursor = Cursors.WaitCursor
            Try
                UD = WEB.SDB.RegisterUser(UD)
                Abort = True
                Me.Cursor = Cursors.Default
            Catch ex As Exception
                Me.Cursor = Cursors.Default
                Dim answ = MsgError(Me, "Unable to connect to the internet to register Computer.", "Connection Error", MessageBoxButtons.RetryCancel, Telerik.WinControls.RadMessageIcon.Error)
                If answ = Windows.Forms.DialogResult.Cancel Then
                    Abort = True
                    Exit Sub
                End If
            End Try
        End While

        My.Settings.UserFingerPrint = UD.MYFINGERPRINT.FINGER_PRINT
        My.Settings.Save()

        Select Case UD.FPStatus
            Case FFAOptCalcService.FPStatusEnum.DBError, FFAOptCalcService.FPStatusEnum.OFError
                MsgError(Me, "Server Error Processing your request", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Case FFAOptCalcService.FPStatusEnum.DemoFingerPrintHasExpired, FFAOptCalcService.FPStatusEnum.DemoLicenseHasExpiredNoFingerPrintAdded
                Dim msg As String = "Your trial period has expired application will now exit. If you have an alternative"
                msg = msg & vbCrLf & "valid license key, or you wish to purchase a subscription please press OK."
                Dim answ = MsgError(Me, msg, "Trial period has expired", MessageBoxButtons.YesNo, Telerik.WinControls.RadMessageIcon.Info)
                If answ = Windows.Forms.DialogResult.No Then
                    Me.DialogResult = Windows.Forms.DialogResult.Abort
                    Me.Close()
                Else
                    ConstructPCList()
                    Step4.Close()
                    ConstructEshop()
                    Step5.Show()
                    Exit Sub
                End If
            Case FFAOptCalcService.FPStatusEnum.LiveLicenseHasExpiredNoFingerPrintAdded
                Dim msg As String = "Your subscription period has expired application will now exit. If you have an alternative"
                msg = msg & vbCrLf & "valid license key, or you wish to purchase a subscription please press OK."
                Dim answ = MsgError(Me, msg, "Registrarion period has expired", MessageBoxButtons.YesNo, Telerik.WinControls.RadMessageIcon.Info)
                If answ = Windows.Forms.DialogResult.No Then
                    Me.DialogResult = Windows.Forms.DialogResult.Abort
                    Me.Close()
                Else
                    ConstructPCList()
                    Step4.Close()
                    ConstructEshop()
                    Step5.Show()
                End If
            Case FFAOptCalcService.FPStatusEnum.ExistingFPLiveMaxLicensesError
                Dim msg As String = "Number of activated PCs cannot exceed maximum number of available liceses. Please"
                msg += vbCrLf & " deselect appropriate number of active PCs to comply with requirement and try again"
                MsgError(Me, msg, "Error is requested PCs activation", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Exit Sub
            Case Else
                ConstructPCList()
                MsgError(Me, "User Data Registered Succesfully. Continuing with data fetch.", "Success", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
                Me.Step4.Close()
                Me.Step1.Show()
                FirstTimeRun()
        End Select
    End Sub
    Private Sub rmeb_License_Validated(sender As Object, e As EventArgs) Handles rmeb_License.Validated

        Dim license As String = rmeb_License.Value.Replace("-", "")
        Try
            If license = "" Then
                Exit Sub
            End If
        Catch ex As Exception
            Exit Sub
        End Try

        If license.Length <> 15 Then
            Dim answ = MsgError(Me, "You have only entered partial part of the license key. Press OK to retry or cancel to leave this field blank", "License key incomplete", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info)
            If answ = Windows.Forms.DialogResult.Cancel Then
                rmeb_License.Value = Nothing
                rbtn_RegisterPC.Focus()
                Exit Sub
            Else
                rmeb_License.Focus()
                Exit Sub
            End If
        End If

        Dim Abort As Boolean = False
        While Abort = False
            Me.Cursor = Cursors.WaitCursor
            Try
                UD.MYFINGERPRINT.LICENSE_KEY = license
                UD = WEB.SDB.CheckLicense(UD)
                Abort = True
                Me.Cursor = Cursors.Default
            Catch ex As Exception
                Me.Cursor = Cursors.Default
                Beep()
                Dim answ = MsgError(Me, "Unable to connect to the internet to validate license key.", "Connection Error", MessageBoxButtons.RetryCancel, Telerik.WinControls.RadMessageIcon.Error)
                If answ = Windows.Forms.DialogResult.Cancel Then
                    Abort = True
                End If
            End Try
        End While

        If UD.FPStatus = FFAOptCalcService.FPStatusEnum.InvalidLicenseKey Then
            UD.MYFINGERPRINT.LICENSE_KEY = UD.License.LICENSE_KEY
            MsgError(Me, "This is not a valid license key, please enter a valid key or leave empty to proceed", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            rmeb_License.Value = Nothing
            rmeb_License.Focus()
            Exit Sub
        ElseIf UD.FPStatus = FFAOptCalcService.FPStatusEnum.ValidLicenseKey Then
            rtb_LicenseExpiration.Text = FormatDateTime(UD.License.LICENSE_EXP_DATE, DateFormat.ShortDate)
            se_NoLicenses.Value = UD.License.MAX_LICENSES
            se_LicensesUsed.Value = UD.License.USED_LICENSES
            ConstructPCList()
        End If
    End Sub
    Private Sub rbtn_ESHOP_Click(sender As Object, e As EventArgs) Handles rbtn_ESHOP.Click
        'Me.Step4.Close()
        'Me.Step5.Show()

        Dim msg As String = String.Empty
        msg += "The annual subscription cost to the FFA Suite Service is "
        Select Case UD.PRODUCT.CCY_ID
            Case 978 'euro
                msg += "EUR " & Format(UD.PRODUCT.BASE_PRICE, "#,##.00")
            Case 840 'usd
                msg += "USD " & Format(UD.PRODUCT.BASE_PRICE, "#,##.00")
        End Select
        msg += vbCrLf
        msg += " The purchase of a subscription allows you to install"
        msg += vbCrLf
        msg += " the application in two PC's of your choice."
        Dim answ = MsgError(Me, msg, "Proceed to payment website?", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Question)
        If answ = Windows.Forms.DialogResult.Cancel Then
            Exit Sub
        End If

        Dim appPath As String = Path.GetDirectoryName(Application.ExecutablePath)
        Dim htmlpage As String
        Dim PayPalSrvc As String
        Dim notifyurl As String
        Dim merchantid As String
        htmlpage = "\PayPalGetOneFree.html"

#If DEBUG Then
        notifyurl = My.Settings.PayPalNotifyURL_Demo
        merchantid = My.Settings.PayPalMerchantID_Demo
        PayPalSrvc = My.Settings.PayPAlSrvc_Demo
#Else
        notifyurl = My.Settings.PayPalNotifyURL
        merchantid=My.Settings.PayPalMerchantID
        PayPalSrvc = My.Settings.PayPalSrvc
#End If

        Dim sr As StreamReader = New StreamReader(appPath & htmlpage)
        Dim srt As String = sr.ReadToEnd
        srt = srt.Replace("PayPalSrvc", PayPalSrvc)
        srt = srt.Replace("notifyurl", notifyurl)
        srt = srt.Replace("merchantid", merchantid)
        srt = srt.Replace("FINGERPRINT", UD.MYFINGERPRINT.FINGER_PRINT)
        Select Case UD.PRODUCT.CCY_ID
            Case 978 'euro
                srt = srt.Replace("CCY", "EUR")
            Case 840 'usd
                srt = srt.Replace("CCY", "USD")
        End Select
        srt = srt.Replace("PRICE", Format(UD.PRODUCT.BASE_PRICE, "######.00"))

        Dim NewFile As MemoryStream = New MemoryStream(System.Text.Encoding.UTF8.GetBytes(srt))
        Dim fs As FileStream = New FileStream(appPath & "\PayPalGetOneFreeCopy.html", FileMode.Create)
        NewFile.WriteTo(fs)
        fs.Close()

        Dim browser As String = getDefaultBrowser()

        If Not browser.Contains("ERROR") Then
            Dim process As New Process()
            process.StartInfo.FileName = browser
            process.StartInfo.Arguments = appPath & "\PayPalGetOneFreeCopy.html"
            process.Start()
        Else
            MsgError(Me, "Unable to open your default broweser.", "Default browser not found", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If

        Me.Close()
        Application.Exit()
    End Sub
    Private Sub rgv_InstalledComputers_CommandCellClick(sender As Object, e As EventArgs) Handles rgv_InstalledComputers.CommandCellClick
        Dim c As Telerik.WinControls.UI.GridCommandCellElement = TryCast(sender, Telerik.WinControls.UI.GridCommandCellElement)
        If IsNothing(c) Then Exit Sub

        If c.ColumnInfo.Name = "REMOVE" Then
            Dim answ = MsgError(Me, "Do you want to delete this PC from the registered users list?", "Remove PC", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Question)
            If answ = Windows.Forms.DialogResult.OK Then
                Dim fp = (From q In UD.FingerPrints Where q.FINGER_PRINT = rgv_InstalledComputers.CurrentRow.Cells("FINGERPRINT").Value Select q).FirstOrDefault
                If IsNothing(fp) Then
                    fp.HIDE = True
                End If
                If rgv_InstalledComputers.CurrentRow.Cells("ACTIVE").Value = True Then
                    se_LicensesUsed.Value -= 1
                End If
                rgv_InstalledComputers.CurrentRow.Cells("ACTIVE").Value = False
                rgv_InstalledComputers.CurrentRow.Cells("DELETE").Value = True
                rgv_InstalledComputers.EndEdit()
                rgv_InstalledComputers.CurrentRow.IsVisible = False
            End If
        End If
    End Sub
    Private Sub rgv_InstalledComputers_ViewCellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles rgv_InstalledComputers.ViewCellFormatting
        If IsNothing(e.CellElement.ColumnInfo) Then Exit Sub

        If TypeOf e.CellElement Is Telerik.WinControls.UI.GridDataCellElement Then
            If e.Column.IsVisible Then
                If e.Row.Cells("MYPC").Value = True Then
                    e.CellElement.ForeColor = Color.Red
                Else
                    e.CellElement.ForeColor = Color.Black
                End If

                If e.CellElement.ColumnInfo.Name = "REMOVE" And e.Row.Cells("MYPC").Value = True Then
                    e.CellElement.Enabled = False
                ElseIf e.CellElement.ColumnInfo.Name = "REMOVE" And e.Row.Cells("MYPC").Value = False Then
                    e.CellElement.Enabled = True
                End If
            End If
        End If
    End Sub
    Private Sub rgv_InstalledComputers_ValueChanging(sender As Object, e As Telerik.WinControls.UI.ValueChangingEventArgs) Handles rgv_InstalledComputers.ValueChanging
        If IsNothing(rgv_InstalledComputers.CurrentCell) Then Exit Sub

        Dim cr As Telerik.WinControls.UI.GridDataCellElement = rgv_InstalledComputers.CurrentCell

        If cr.ColumnIndex = 3 Then
            If cr.RowInfo.Cells("MYPC").Value = True And e.OldValue = True Then
                Beep()
                e.Cancel = True
                Exit Sub
            End If

            Dim LicenseCntr As Integer
            For Each r In m_InstComp
                If r.ACTIVE = True Then
                    LicenseCntr += 1
                End If
            Next
            If e.NewValue = True And LicenseCntr + 1 > se_NoLicenses.Value Then
                e.Cancel = True
                Dim msg As String = "You have excedded the maximum allowable licenses." & vbCrLf & "Deselect another PC from active status first."
                MsgError(Me, msg, "Maximum licenses Exceeded", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
                Exit Sub
            End If

            If e.OldValue = True And e.NewValue = False Then
                Me.se_LicensesUsed.Value -= 1
            ElseIf e.OldValue = False And e.NewValue = True Then
                Me.se_LicensesUsed.Value += 1
            End If
        End If
    End Sub
    Private Sub rgv_InstalledComputers_ValueChanged(sender As Object, e As EventArgs) Handles rgv_InstalledComputers.ValueChanged
        Me.rgv_InstalledComputers.EndEdit()
    End Sub
    Private Sub rbtn_Step4_Cancel_Click(sender As Object, e As EventArgs) Handles rbtn_Step4_Cancel.Click
        Dim answ = MsgError(Me, "Application will exit, all Changes will be lost", "Cancel Wizard", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info)
        If answ = Windows.Forms.DialogResult.OK Then
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Me.Close()
        End If
    End Sub
#End Region
    

    Private Sub rbtn_step1_Right_Click(sender As Object, e As EventArgs) Handles rbtn_step1_Right.Click
        Dim answ = MsgError(Me, "Application will exit, all Changes will be lost", "Cancel Wizard", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Info)
        If answ = Windows.Forms.DialogResult.OK Then
            Me.DialogResult = Windows.Forms.DialogResult.Abort
            Me.Close()
            Application.Exit()
        End If
    End Sub

    Private Sub rbtn_step2_Right_Click(sender As Object, e As EventArgs)
        Me.DialogResult = Windows.Forms.DialogResult.Abort
        Me.Close()
    End Sub


    Private Sub rbtn_step2_Right_Click_1(sender As Object, e As EventArgs) Handles rbtn_step2_Right.Click
        Me.DialogResult = Windows.Forms.DialogResult.Abort
        Me.Close()
        Application.Exit()
    End Sub

    
End Class
