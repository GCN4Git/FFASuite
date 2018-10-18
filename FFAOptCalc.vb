Imports System.ServiceModel
Imports System.Net
Imports System.Net.Security
Imports System.Net.Sockets
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.IO
Imports System.Xml.Serialization
Imports agsXMPP
Imports agsXMPP.Net
Imports agsXMPP.Xml
Imports agsXMPP.protocol
Imports agsXMPP.protocol.client
Imports Telerik.WinControls.UI
Imports FM
Imports FM.WebSync
Imports ArtB_Class_Library

Public Class FFAOptCalc

    Private trade_class_FirstTime As Boolean = True
    Private vessel_class_FirstTime As Boolean = True
    Private route_FirstTime As Boolean = True

    Private FirstTimeUserResult As Integer
    Private m_jid As agsXMPP.Jid

    Public GVC As New ArtB_Class_Library.GlobalViewClass
    Public UserInfo As ArtB_Class_Library.TraderInfoClass = Nothing

    Public WithEvents myXMPP As XmppClientConnection
    Public WithEvents myWS As Client

    Public Event ReceivedFFALiveTrade(sender As Object, LiveTrade As FFAOptCalcService.VolDataClass)
    Public Event SpotRatesUpdated()
    Public Event SwapRatesUpdated()
    Public Event RefreshMarketView()
    Public Event AmmededTradeReceived(sender As Object, AmmededTrade As FFAOptCalcService.VolDataClass)
    Public Event BidAskReceived(sender As Object, BAList As List(Of FFASuitePL.WP8FFAData))

    Private m_XMPPCLose As Boolean = False
    Private m_FirstTimeServiceConnect As Boolean = True
    Private m_CurrentDock As Telerik.WinControls.UI.Docking.DockWindow

    Private WithEvents DockService As Telerik.WinControls.UI.Docking.DragDropService

    'Private voice As New Speech.Synthesis.SpeechSynthesizer

    Private Sub FFAOptCalc_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        Application.Exit()
    End Sub

    Private Sub FFAOptCalc_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        My.Settings.MainFormDimensions = Me.Size
        My.Settings.Save()

        Try
            WEB.SDB.Close()
        Catch ex As Exception

        End Try
        Try
            If g_msgServiceType = MsgConnectivityServiceEnum.BOSH Or g_msgServiceType = MsgConnectivityServiceEnum.TCP Then
                m_XMPPCLose = True
                If IsNothing(myXMPP) = False Then
                    myXMPP.Close()
                    System.Threading.Thread.Sleep(4000)
                    Dim cntr As Integer
                    Do While myXMPP.XmppConnectionState <> XmppConnectionState.Disconnected And cntr <= 20
                        cntr += 1
                        System.Threading.Thread.Sleep(500)
                        Debug.Print("trying to disconnect: " & cntr)
                        If cntr = 20 Then
                            myXMPP = Nothing
                        End If
                    Loop
                End If
            ElseIf g_msgServiceType = MsgConnectivityServiceEnum.WS Then
                myWS.Disconnect()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub FFAOptCalc_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Me.Size = My.Settings.MainFormDimensions
        Catch ex As Exception
            Me.Size = New System.Drawing.Size(775, 637)
        End Try

        Try
            DockService = Me.rd_MAIN.GetService(Of Telerik.WinControls.UI.Docking.DragDropService)()
        Catch ex As Exception
            Stop
        End Try

        'voice.SelectVoiceByHints(Speech.Synthesis.VoiceGender.Neutral)

        CommandBarStripElement1.OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Collapsed

        If UD.MYFINGERPRINT.UPDATER = True Then
            rms_Broker.Visibility = Telerik.WinControls.ElementVisibility.Visible
            rmi_BrokerActions.Visibility = Telerik.WinControls.ElementVisibility.Visible
        Else
            rms_Broker.Visibility = Telerik.WinControls.ElementVisibility.Collapsed
            rmi_BrokerActions.Visibility = Telerik.WinControls.ElementVisibility.Collapsed
        End If

        FirstTime()
    End Sub
    Private Sub rd_MAIN_FormRenaming(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.Docking.ContextMenuDisplayingEventArgs)
        'On the Form Load event add following to be able to change programmaticaly the text of the tab
        'Dim menuService As Telerik.WinControls.UI.Docking.ContextMenuService = Me.rd_MAIN.GetService(Of Telerik.WinControls.UI.Docking.ContextMenuService)()
        'AddHandler menuService.ContextMenuDisplaying, AddressOf rd_MAIN_FormRenaming

        If e.MenuType = Telerik.WinControls.UI.Docking.ContextMenuType.DockWindow AndAlso TypeOf e.DockWindow.DockTabStrip Is Telerik.WinControls.UI.Docking.DocumentTabStrip Then
            Dim newMenuItem As New RadMenuItem("Rename Form")
            m_CurrentDock = e.DockWindow
            AddHandler newMenuItem.Click, AddressOf rmi_FormRenameClick
            e.MenuItems.Add(newMenuItem)
        End If
    End Sub
    Private Sub rmi_FormRenameClick(ByVal sender As Object, ByVal e As EventArgs)
        m_CurrentDock.Text = "New Name"
    End Sub

    Private Sub FirstTime()
        Dim cuda = New FFAOptionSolveClass
        If cuda.CUDAEnabled Then
            CudaEnabled = True
        Else
            CudaEnabled = False
        End If
        cuda = Nothing

        If g_msgServiceType = MsgConnectivityServiceEnum.Indifferent Then
            g_msgServiceType = CheckMsgConnectivityService()
        End If

        'g_msgServiceType = MsgConnectivityServiceEnum.WS
        Select Case g_msgServiceType
            Case MsgConnectivityServiceEnum.TCP, MsgConnectivityServiceEnum.WS
                BOSH_reconnect.Enabled = True
                BOSH_reconnect.Start()
            Case MsgConnectivityServiceEnum.BOSH
                ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertficate)
                BOSH_reconnect.Enabled = True
                BOSH_reconnect.Start()
            Case Else
                MsgError(Me, "Unable to implement a suitable live server connection", "Unable to connect to Live Server", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
        End Select

        Dim routes As New List(Of Integer)
        For Each r In ROUTES_DETAIL
            routes.Add(r.ROUTE_ID)
        Next
        Dim freshintradaydata As New List(Of FFAOptCalcService.VolDataClass)
        Try
            freshintradaydata.AddRange(WEB.SDB.RefreshIntradayData(routes))
            FIXINGS_RefreshLiveData(routes, freshintradaydata)
        Catch ex As Exception
            MsgError(Me, "Failed to refresh Live Data", "Communication Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
        End Try
        'System.Threading.Thread.Sleep(1500)

        DisplayMarketViews()

    End Sub

    Private Function CheckMsgConnectivityService() As MsgConnectivityServiceEnum

        'check if TCP for XMPP port 5222 is enabled
        Dim TCPOpen As Boolean = TCPOpenPort(g_XMPPServerString, 5222)
        If TCPOpen = True Then
            My.Settings.msgServiceType = MsgConnectivityServiceEnum.TCP
            My.Settings.Save()
            Return MsgConnectivityServiceEnum.TCP
        End If

        'if TCP unavailable, check if xmpp BOSH connection is available
        Dim boshrequest As WebRequest = WebRequest.Create("https://" & g_XMPPServerString & ":7443/")
        boshrequest.Timeout = 20000
        Dim boshresponse As HttpWebResponse
        Try
            boshresponse = CType(boshrequest.GetResponse(), HttpWebResponse)
            If boshresponse.StatusCode = HttpStatusCode.OK Then
                boshresponse.Close()
                My.Settings.msgServiceType = MsgConnectivityServiceEnum.BOSH
                My.Settings.Save()
                Return MsgConnectivityServiceEnum.BOSH
            End If
        Catch ex As Exception
        End Try

        'Finally check if WS is available
        Dim wsrequest As WebRequest = WebRequest.Create(g_WSServerString)
        wsrequest.Timeout = 20000
        Dim wsresponse As HttpWebResponse
        Try
            wsresponse = CType(wsrequest.GetResponse(), HttpWebResponse)
            If wsresponse.StatusCode = HttpStatusCode.OK Then
                wsresponse.Close()
                My.Settings.msgServiceType = MsgConnectivityServiceEnum.WS
                My.Settings.Save()
                Return MsgConnectivityServiceEnum.WS
            End If
        Catch ex As Exception
        End Try

        'finally
        Return 999
    End Function

#Region "XMPPMessages"
    Private Sub myXMPP_OnAuthError(sender As Object, e As Dom.Element) Handles myXMPP.OnAuthError
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf myXMPP_OnAuthError, agsXMPP.XmppElementHandler), New Object() {sender, e})
            Return
        End If
        rle_FormSatus.Text = "Authentication Error: " & e.Value
    End Sub
    Private Sub myXMPP_OnError(sender As Object, ex As Exception) Handles myXMPP.OnError
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf myXMPP_OnError, agsXMPP.ErrorHandler), New Object() {sender, ex})
            Return
        End If
        rle_FormSatus.Text = "Error: " & ex.Message
    End Sub
    Private Sub myXMPP_OnIq(sender As Object, iq As IQ) Handles myXMPP.OnIq
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf myXMPP_OnIq, IqHandler), New Object() {sender, iq})
            Return
        End If

        If IsNothing(iq) Then Exit Sub
        If IsNothing(iq.Error) Then Exit Sub
        If IsNothing(iq.Error.Code) Then Exit Sub
        If iq.Error.Code = 409 Then
            rle_FormSatus.Text = "You appear as logged in to the server from another connection."
        End If
    End Sub
    Private Sub myXMPP_OnLogin(sender As Object) Handles myXMPP.OnLogin
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf myXMPP_OnLogin, agsXMPP.ObjectHandler), New Object() {sender})
            Return
        End If

        If m_XMPPCLose = True Then Exit Sub

        If g_msgServiceType = MsgConnectivityServiceEnum.BOSH Then
            BOSH_KeepAlive.Enabled = True
            BOSH_KeepAlive.Start()
            rle_FormSatus.Text = "Connected to Data Server BOSH"
        ElseIf g_msgServiceType = MsgConnectivityServiceEnum.TCP Then
            rle_FormSatus.Text = "Connected to Data Server TCP"
        End If
        BOSH_reconnect.Stop()
        BOSH_reconnect.Enabled = False
        BOSH_reconnect.Interval = 15000
        rib_XMPP.BackColor = Color.DarkGreen

        If m_FirstTimeServiceConnect = False Then
            'insert code to retrieve fresh trade list
            Dim routes As New List(Of Integer)
            For Each r In ROUTES_DETAIL
                routes.Add(r.ROUTE_ID)
            Next
            Dim freshintradaydata As List(Of FFAOptCalcService.VolDataClass) = WEB.SDB.RefreshIntradayData(routes)
            FIXINGS_RefreshLiveData(routes, freshintradaydata)
            RaiseEvent RefreshMarketView()
        End If
        m_FirstTimeServiceConnect = False
    End Sub
    Private Sub myXMPP_OnXmppConnectionStateChanged(sender As Object, state As XmppConnectionState) Handles myXMPP.OnXmppConnectionStateChanged
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf myXMPP_OnXmppConnectionStateChanged, agsXMPP.XmppConnectionStateHandler), New Object() {sender, state})
            Return
        End If

        If m_XMPPCLose = True Then Exit Sub

        If state = XmppConnectionState.Disconnected Then
            BOSH_reconnect.Enabled = True
            BOSH_reconnect.Start()
            Debug.Print("Disconnected")
            rib_XMPP.BackColor = Color.Red
            rle_FormSatus.Text = "Disconnected from Data Server"
        Else
            rle_FormSatus.Text = state.ToString
        End If
    End Sub

    Private Sub myXMPP_OnMessage(sender As Object, msg As agsXMPP.protocol.client.Message) Handles myXMPP.OnMessage
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf myXMPP_OnMessage, MessageHandler), New Object() {sender, msg})
            Return
        End If

        Dim msgtype = CInt(msg.Subject)
        Select Case msgtype
            Case ArtBMessages.DisconnectFFAOptionsUsers
                Me.Close()
            Case ArtBMessages.OrderFFATrade
                GVC.UpdateFromMesage(msg.Body, msgtype, UserInfo)
                Dim c As Collection = GVC.ParseString(msg.Body, ArtBMessages.OrderFFATrade)
                Dim ListTrades As New List(Of TRADES_FFA_CLASS)
                For Each Trade As TRADES_FFA_CLASS In c
                    Dim alreadyinserted = (From q In ListTrades Where q.TRADE_ID = Trade.TRADE_ID Select q).FirstOrDefault
                    If IsNothing(alreadyinserted) = True Then
                        ListTrades.Add(Trade)
                    End If
                Next
                For Each trade In ListTrades
                    If trade.TRADE_TYPE = ArtB_Class_Library.OrderTypes.FFA Then
                        Dim nc As New FFAOptCalcService.VolDataClass
                        nc.TRADE_ID = trade.TRADE_ID
                        nc.DESK_TRADER_ID = trade.DESK_TRADER_ID1
                        nc.TRADE_TYPE = trade.TRADE_TYPE
                        nc.ROUTE_ID = trade.ROUTE_ID
                        If trade.PNC = False Then
                            nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live
                        Else
                            nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.level
                        End If
                        nc.PERIOD = trade.SHORTDES
                        nc.FIXING_DATE = trade.ORDER_DATETIME
                        nc.FFA_PRICE = trade.PRICE_TRADED
                        nc.YY1 = trade.YY1
                        nc.YY2 = trade.YY2
                        nc.MM1 = trade.MM1
                        nc.MM2 = trade.MM2
                        FIXINGS_Add(nc)
                        Dim ShouldDisplay = (From q In ROUTES_DETAIL Where q.ROUTE_ID = trade.ROUTE_ID Select q).FirstOrDefault
                        If IsNothing(ShouldDisplay) = False Then
                            RaiseEvent ReceivedFFALiveTrade(Me, nc)
                        End If
                    Else
                        Dim nc As New FFAOptCalcService.VolDataClass
                        nc.TRADE_ID = trade.TRADE_ID
                        nc.DESK_TRADER_ID = trade.DESK_TRADER_ID1
                        nc.TRADE_TYPE = trade.TRADE_TYPE
                        nc.ROUTE_ID = trade.ROUTE_ID
                        nc.ROUTE_ID2 = trade.ROUTE_ID2
                        If trade.PNC = False Then
                            nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live
                        Else
                            nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.level
                        End If
                        nc.PERIOD = trade.SHORTDES
                        nc.FIXING_DATE = trade.ORDER_DATETIME
                        nc.FFA_PRICE = trade.PRICE_TRADED
                        nc.YY1 = trade.YY1
                        nc.YY2 = trade.YY2
                        nc.MM1 = trade.MM1
                        nc.MM2 = trade.MM2
                        nc.YY21 = trade.YY21
                        nc.YY22 = trade.YY22
                        nc.MM21 = trade.MM21
                        nc.MM22 = trade.MM22
                        'FIXINGS_Add(nc)
                        Dim ShouldDisplay1 = (From q In ROUTES_DETAIL Where q.ROUTE_ID = trade.ROUTE_ID Select q).FirstOrDefault
                        Dim ShouldDisplay2 = (From q In ROUTES_DETAIL Where q.ROUTE_ID = trade.ROUTE_ID2 Select q).FirstOrDefault
                        If IsNothing(ShouldDisplay1) = False And IsNothing(ShouldDisplay2) = False Then
                            'RaiseEvent ReceivedFFALiveTrade(Me, nc)
                        End If
                    End If
                    If My.Settings.Messages = True And trade.PNC = False Then
                        If trade.TRADE_TYPE = FFAOptCalcService.OrderTypes.FFA Then
                            If IsNothing((From q In ROUTES_DETAIL Where q.ROUTE_ID = trade.ROUTE_ID Select q).FirstOrDefault) = False Then
                                Dim route = (From q In ROUTES Where q.ROUTE_ID = trade.ROUTE_ID Select q).FirstOrDefault
                                Dim sMsg As String = route.ROUTE_SHORT
                                sMsg += " " & trade.SHORTDES & " trades " & trade.PRICE_TRADED
                                Me.Rad_Alert.ContentText = sMsg
                                Me.Rad_Alert.Show()
                                'voice.SpeakAsync(sMsg)
                            End If
                        ElseIf trade.TRADE_TYPE = FFAOptCalcService.OrderTypes.PriceSpread Then
                            Dim sMsg As String = (From q In ROUTES Where q.ROUTE_ID = trade.ROUTE_ID Select q.ROUTE_SHORT).FirstOrDefault
                            sMsg += "-" & (From q In ROUTES Where q.ROUTE_ID = trade.ROUTE_ID2 Select q.ROUTE_SHORT).FirstOrDefault
                            sMsg += " " & trade.SHORTDES & " trades " & trade.PRICE_TRADED
                            Me.Rad_Alert.ContentText = sMsg
                            Me.Rad_Alert.Show()
                        ElseIf trade.TRADE_TYPE = FFAOptCalcService.OrderTypes.RatioSpread Then
                            Dim sMsg As String = (From q In ROUTES Where q.ROUTE_ID = trade.ROUTE_ID Select q.ROUTE_SHORT).FirstOrDefault
                            sMsg += "/" & (From q In ROUTES Where q.ROUTE_ID = trade.ROUTE_ID2 Select q.ROUTE_SHORT).FirstOrDefault
                            sMsg += " " & trade.SHORTDES & " trades " & trade.PRICE_TRADED
                            Me.Rad_Alert.ContentText = sMsg
                            Me.Rad_Alert.Show()
                        End If
                    End If
                Next
            Case ArtBMessages.SpotRatesUpdated
                If msg.Body = "" Then
                    Exit Sub
                End If
                Try
                    Dim newspots As List(Of FFAOptCalcService.SpotFixingsClass) = Json.Deserialize(Of List(Of FFAOptCalcService.SpotFixingsClass))(msg.Body)
                    Dim applroutes = (From q In ROUTES_DETAIL Select q).ToList
                    For Each r In applroutes
                        Dim spot = (From q In newspots Where q.ROUTE_ID = r.ROUTE_ID Select q).FirstOrDefault
                        If IsNothing(spot) = False Then
                            r.SPOT_FIXING_DATE = spot.FIXING_DATE
                            r.AVERAGE_INCLUDES_TODAY = True
                            r.FIXING_AVG = spot.AVG_FIXING
                            r.SPOT_PRICE = spot.FIXING

                            Dim nc As New FFAOptCalcService.VolDataClass
                            nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot
                            nc.ROUTE_ID = spot.ROUTE_ID
                            nc.FIXING_DATE = spot.FIXING_DATE
                            nc.SPOT_PRICE = spot.FIXING
                            nc.FFA_PRICE = spot.FIXING
                            nc.PERIOD = "SPOT"
                            nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot
                            FIXINGS_UpdateSpotPrice(nc)
                        End If
                    Next
                    RaiseEvent SpotRatesUpdated()
                Catch ex As Exception
#If DEBUG Then
                    Stop
#End If
                End Try
            Case ArtBMessages.ForwardRatesUpdated
                If msg.Body = "" Then
                    Exit Sub
                End If
                Try
                    Dim newswaps As List(Of FFAOptCalcService.SwapFixingsClass) = Json.Deserialize(Of List(Of FFAOptCalcService.SwapFixingsClass))(msg.Body)
                    Dim applroutes = (From q In ROUTES_DETAIL Select q.ROUTE_ID).ToList
                    For Each r In applroutes
                        Dim swapfixings = (From q In newswaps Where q.ROUTE_ID = r Select q).ToList
                        For Each s In swapfixings
                            Dim nr As New FFAOptCalcService.VolDataClass
                            nr.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap
                            nr.PERIOD = "LIVE"
                            nr.ROUTE_ID = s.ROUTE_ID
                            nr.FIXING_DATE = Now
                            nr.FFA_PRICE = s.FIXING
                            nr.YY1 = s.YY1
                            nr.YY2 = s.YY2
                            nr.MM1 = s.MM1
                            nr.MM2 = s.MM2
                            FIXINGS_Add(nr)
                        Next
                    Next
                    RaiseEvent SwapRatesUpdated()
                Catch ex As Exception
#If DEBUG Then
                    Stop
#End If
                End Try
            Case ArtBMessages.AmmendReportedTrade
                Try
                    Dim AmmededTrade As FFAOptCalcService.VolDataClass = Json.Deserialize(Of FFAOptCalcService.VolDataClass)(msg.Body)
                    If IsNothing(AmmededTrade) = False Then
                        Dim oldtrade = (From q In FIXINGS Where q.TRADE_ID = AmmededTrade.TRADE_ID Select q).FirstOrDefault
                        If IsNothing(oldtrade) = False Then
                            AmmededTrade.SPOT_PRICE = oldtrade.SPOT_PRICE
                            oldtrade.FFA_PRICE = AmmededTrade.FFA_PRICE
                            oldtrade.VolRecordType = AmmededTrade.VolRecordType
                            oldtrade.PNC = AmmededTrade.PNC
                            RaiseEvent AmmededTradeReceived(Me, AmmededTrade)
                        End If
                    End If
                Catch ex As Exception
#If DEBUG Then
                    Stop
#End If
                End Try
            Case ArtBMessages.BidAskUpdate
                Dim BAList As List(Of FFASuitePL.WP8FFAData) = Json.Deserialize(Of List(Of FFASuitePL.WP8FFAData))(msg.Body)
                RaiseEvent BidAskReceived(Me, BAList)
        End Select
    End Sub

    Private Sub BOSH_KeepAlive_Tick(sender As Object, e As EventArgs) Handles BOSH_KeepAlive.Tick
        If g_msgServiceType <> MsgConnectivityServiceEnum.BOSH Then Exit Sub

        If IsNothing(myXMPP) = False Then
            If m_XMPPCLose = False Then
                Try
                    Dim p As New agsXMPP.protocol.extensions.ping.PingIq
                    p.To = g_XMPPServerString
                    p.Type = IqType.get
                    myXMPP.Send(p)
                Catch ex As Exception
                End Try
            End If
        End If
    End Sub
    Private Sub BOSH_reconnect_Tick(sender As Object, e As EventArgs) Handles BOSH_reconnect.Tick
        Static cntr As Integer = 1

        If InvokeRequired Then
            BeginInvoke(CType(AddressOf BOSH_reconnect_Tick, EventHandler), New Object() {sender, e})
            Return
        End If

        BOSH_reconnect.Interval = 20000
        Select Case g_msgServiceType
            Case MsgConnectivityServiceEnum.TCP, MsgConnectivityServiceEnum.BOSH
                If IsNothing(myXMPP) = False Then
                    rle_FormSatus.Text = "Trying to reconnect... (" & cntr & "), ConState: " & myXMPP.XmppConnectionState
                Else
                    If cntr = 1 Then
                        rle_FormSatus.Text = "Trying to connect..."
                    Else
                        rle_FormSatus.Text = "Trying to reconnect... (" & cntr & "), ConState: nothing"
                    End If
                End If
                If m_XMPPCLose = False Then
                    xmppOpenBosh()
                End If
                cntr += 1
            Case MsgConnectivityServiceEnum.WS
                If IsNothing(myWS) = False Then
                    If myWS.IsConnected Then Exit Sub
                End If
                If cntr = 1 Then
                    rle_FormSatus.Text = "Trying to connect ..."
                Else
                    rle_FormSatus.Text = "Trying to reconnect... (" & cntr & ")"
                End If
                cntr += 1
                WS_Connect()
        End Select
    End Sub
    Private Sub xmppOpenBosh()
        myXMPP = New XmppClientConnection
        m_jid = New agsXMPP.Jid(UD.MYFINGERPRINT.OFID & "@" & g_XMPPServerString)
        myXMPP.Server = m_jid.Server
        myXMPP.Username = m_jid.User
        myXMPP.Password = UD.OFPswd
        myXMPP.Resource = "FFAs Suite"
        myXMPP.Priority = 0
        If g_msgServiceType = MsgConnectivityServiceEnum.TCP Then
            myXMPP.Port = 5222
            'myXMPP.UseSSL = True
            myXMPP.KeepAlive = True
            myXMPP.KeepAliveInterval = 30
            myXMPP.UseCompression = True
        ElseIf g_msgServiceType = MsgConnectivityServiceEnum.BOSH Then
            myXMPP.AutoResolveConnectServer = False
            myXMPP.UseCompression = False
            myXMPP.SocketConnectionType = Net.SocketConnectionType.Bosh
            myXMPP.ConnectServer = "https://" & m_jid.Server & ":7443/http-bind/"
            If BehindProxy = True Or My.Settings.UsesProxy = True Then
                Dim BoshClientSocket As BoshClientSocket = DirectCast(myXMPP.ClientSocket, BoshClientSocket)
                BoshClientSocket.Proxy = New System.Net.WebProxy(New System.Uri(My.Settings.proxyaddress), True)
                BoshClientSocket.Proxy.Credentials = New System.Net.NetworkCredential(My.Settings.proxyuser, My.Settings.proxypswd)
                BoshClientSocket.Proxy.BypassProxyOnLocal = True
            End If
        End If
        myXMPP.Open()
    End Sub
#End Region
#Region "FrozenMountain"
    Private Sub WS_Connect()
        myWS = New Client(g_WSServerString)
        Dim conargs As New ConnectArgs
        Dim cred As New FFAOptCalcService.FFASuiteCredentials
        cred.FINGERPRINT = UD.MYFINGERPRINT.FINGER_PRINT
        cred.PRODUCT_ID = My.Settings.PRODUCT_ID
        cred.TimeStamp = Now
        cred.SubscribedRoutes = New List(Of Integer)({0})
        conargs.MetaJson = Json.Serialize(cred)
        conargs.OnStreamFailure = AddressOf myWS_Disconnected
        myWS.Connect(conargs)
    End Sub
    Private Sub myWS_Disconnected(d As StreamFailureArgs)
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf myWS_Disconnected, agsXMPP.ObjectHandler), New Object() {d})
            Return
        End If
        rle_FormSatus.Text = "Disconnected from Data Server WS"
        rib_XMPP.BackColor = Color.Red
    End Sub
    Private Sub myWS_OnConnectFailure(p As ConnectFailureArgs) Handles myWS.OnConnectFailure
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf myWS_OnConnectFailure, agsXMPP.ObjectHandler), New Object() {p})
            Return
        End If
        rle_FormSatus.Text = "WS Connection Failure: " & p.ErrorMessage
        rib_XMPP.BackColor = Color.Red
    End Sub
    Private Sub myWS_OnConnectSuccess(p As ConnectSuccessArgs) Handles myWS.OnConnectSuccess
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf myWS_OnConnectSuccess, agsXMPP.ObjectHandler), New Object() {p})
            Return
        End If

        BOSH_reconnect.Stop()
        BOSH_reconnect.Enabled = False

        rle_FormSatus.Text = "Connected to Data Server WS"
        rib_XMPP.BackColor = Color.DarkGreen

        If p.IsReconnect = False Then
            Dim wsmessages As New SubscribeArgs(My.Settings.ws_SubscriptionChannel)
            wsmessages.OnSuccess = AddressOf WSMessages_Subscribed
            wsmessages.OnReceive = AddressOf WSMessages_OnReceive
            myWS.Subscribe(wsmessages)
        End If

        If UD.MYFINGERPRINT.PRICER = True Then
            If p.IsReconnect = False Then
                Dim tradeschannel As New SubscribeArgs(My.Settings.ws_TradesChannel)
                tradeschannel.OnSuccess = AddressOf TradesChannel_Subscribed
                tradeschannel.OnReceive = AddressOf TradesChannel_OnReceive
                myWS.Subscribe(tradeschannel)
            End If
        End If

        If p.IsReconnect = True Then
            'insert code to retrieve fresh trade list
            Dim routes As New List(Of Integer)
            For Each r In ROUTES_DETAIL
                routes.Add(r.ROUTE_ID)
            Next
            Dim freshintradaydata As New List(Of FFAOptCalcService.VolDataClass)
            freshintradaydata.AddRange(WEB.SDB.RefreshIntradayData(routes))
            FIXINGS_RefreshLiveData(routes, freshintradaydata)
            RaiseEvent RefreshMarketView()
        End If
        m_FirstTimeServiceConnect = False
    End Sub
    Private Sub myWS_OnNotify(p As NotifyReceiveArgs) Handles myWS.OnNotify

    End Sub
    Public Sub WSMessages_Subscribed(ByVal t As SubscribeSuccessArgs)

    End Sub
    Public Sub TradesChannel_Subscribed(ByVal trade As SubscribeSuccessArgs)

    End Sub

    Public Sub WSMessages_OnReceive(ByVal trade As SubscribeReceiveArgs)
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf WSMessages_OnReceive, agsXMPP.ObjectHandler), New Object() {trade})
            Return
        End If

        Select Case trade.Tag
            Case FFAOptCalcService.MessageEnum.TradeAnnouncement
                Dim ntrades = Json.Deserialize(Of List(Of FFAOptCalcService.VolDataClass))(trade.DataJson)
                For Each nr In ntrades
                    If nr.TRADE_TYPE = FFAOptCalcService.OrderTypes.FFA Then
                        FIXINGS_Add(nr)
                        Dim ShouldDisplay = (From q In ROUTES_DETAIL Where q.ROUTE_ID = nr.ROUTE_ID Select q).FirstOrDefault
                        If IsNothing(ShouldDisplay) = False Then
                            RaiseEvent ReceivedFFALiveTrade(Me, nr)
                        End If
                    Else
                        'FIXINGS_Add(nr)
                        Dim ShouldDisplay1 = (From q In ROUTES_DETAIL Where q.ROUTE_ID = nr.ROUTE_ID Select q).FirstOrDefault
                        Dim ShouldDisplay2 = (From q In ROUTES_DETAIL Where q.ROUTE_ID = nr.ROUTE_ID2 Select q).FirstOrDefault
                        If IsNothing(ShouldDisplay1) = False And IsNothing(ShouldDisplay2) = False Then
                            'RaiseEvent ReceivedFFALiveTrade(Me, nr)
                        End If
                    End If
                    If My.Settings.Messages = True And nr.PNC = False Then
                        If nr.TRADE_TYPE = FFAOptCalcService.OrderTypes.FFA Then
                            If IsNothing((From q In ROUTES_DETAIL Where q.ROUTE_ID = nr.ROUTE_ID Select q).FirstOrDefault) = False Then
                                Dim sMsg As String = (From q In ROUTES Where q.ROUTE_ID = nr.ROUTE_ID Select q.ROUTE_SHORT).FirstOrDefault
                                sMsg += " " & nr.PERIOD & " trades: " & nr.FFA_PRICE
                                Me.Rad_Alert.ContentText = sMsg
                                Me.Rad_Alert.Show()
                            End If
                        ElseIf nr.TRADE_TYPE = FFAOptCalcService.OrderTypes.PriceSpread Then
                            Dim sMsg As String = (From q In ROUTES Where q.ROUTE_ID = nr.ROUTE_ID Select q.ROUTE_SHORT).FirstOrDefault
                            sMsg += "-" & (From q In ROUTES Where q.ROUTE_ID = nr.ROUTE_ID2 Select q.ROUTE_SHORT).FirstOrDefault
                            sMsg += " " & nr.PERIOD & " trades: " & nr.FFA_PRICE
                            Me.Rad_Alert.ContentText = sMsg
                            Me.Rad_Alert.Show()
                        ElseIf nr.TRADE_TYPE = FFAOptCalcService.OrderTypes.RatioSpread Then
                            Dim sMsg As String = (From q In ROUTES Where q.ROUTE_ID = nr.ROUTE_ID Select q.ROUTE_SHORT).FirstOrDefault
                            sMsg += "/" & (From q In ROUTES Where q.ROUTE_ID = nr.ROUTE_ID2 Select q.ROUTE_SHORT).FirstOrDefault
                            sMsg += " " & nr.PERIOD & " trades: " & nr.FFA_PRICE
                            Me.Rad_Alert.ContentText = sMsg
                            Me.Rad_Alert.Show()
                        End If
                    End If
                Next
            Case FFAOptCalcService.MessageEnum.SpotRatesUpdate
                Try
                    Dim newspots As List(Of FFAOptCalcService.SpotFixingsClass) = Json.Deserialize(Of List(Of FFAOptCalcService.SpotFixingsClass))(trade.DataJson)
                    Dim applroutes = (From q In ROUTES_DETAIL Select q).ToList
                    For Each r In applroutes
                        Dim spot = (From q In newspots Where q.ROUTE_ID = r.ROUTE_ID Select q).FirstOrDefault
                        If IsNothing(spot) = False Then
                            r.SPOT_FIXING_DATE = spot.FIXING_DATE
                            r.AVERAGE_INCLUDES_TODAY = True
                            r.FIXING_AVG = spot.AVG_FIXING
                            r.SPOT_PRICE = spot.FIXING

                            Dim nc As New FFAOptCalcService.VolDataClass
                            nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot
                            nc.ROUTE_ID = spot.ROUTE_ID
                            nc.FIXING_DATE = spot.FIXING_DATE
                            nc.SPOT_PRICE = spot.FIXING
                            nc.FFA_PRICE = spot.FIXING
                            nc.PERIOD = "SPOT"
                            nc.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.nspot
                            FIXINGS_UpdateSpotPrice(nc)
                        End If
                    Next
                    RaiseEvent SpotRatesUpdated()
                Catch ex As Exception
#If DEBUG Then
                    Stop
#End If
                End Try
            Case FFAOptCalcService.MessageEnum.SwapRatesUpdate
                Try
                    Dim newswaps As List(Of FFAOptCalcService.SwapFixingsClass) = Json.Deserialize(Of List(Of FFAOptCalcService.SwapFixingsClass))(trade.DataJson)
                    Dim applroutes = (From q In ROUTES_DETAIL Select q.ROUTE_ID).ToList
                    For Each r In applroutes
                        Dim swapfixings = (From q In newswaps Where q.ROUTE_ID = r Select q).ToList
                        For Each s In swapfixings
                            Dim nr As New FFAOptCalcService.VolDataClass
                            nr.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.swap
                            nr.PERIOD = "LIVE"
                            nr.ROUTE_ID = s.ROUTE_ID
                            nr.FIXING_DATE = Now
                            nr.FFA_PRICE = s.FIXING
                            nr.YY1 = s.YY1
                            nr.YY2 = s.YY2
                            nr.MM1 = s.MM1
                            nr.MM2 = s.MM2
                            FIXINGS_Add(nr)
                        Next
                    Next
                    RaiseEvent SwapRatesUpdated()
                Catch ex As Exception
#If DEBUG Then
                    Stop
#End If
                End Try
            Case FFAOptCalcService.MessageEnum.CloseClient
                Me.Close()
            Case FFAOptCalcService.MessageEnum.AmmendReportedTrade
                Dim AmmededTrade As FFAOptCalcService.VolDataClass = Json.Deserialize(Of FFAOptCalcService.VolDataClass)(trade.DataJson)
                If IsNothing(AmmededTrade) = False Then
                    Dim oldtrade = (From q In FIXINGS Where q.TRADE_ID = AmmededTrade.TRADE_ID Select q).FirstOrDefault
                    If IsNothing(oldtrade) = False Then
                        AmmededTrade.SPOT_PRICE = oldtrade.SPOT_PRICE
                        oldtrade.FFA_PRICE = AmmededTrade.FFA_PRICE
                        oldtrade.VolRecordType = AmmededTrade.VolRecordType
                        oldtrade.PNC = AmmededTrade.PNC
                        RaiseEvent AmmededTradeReceived(Me, AmmededTrade)
                    End If
                End If
            Case FFAOptCalcService.MessageEnum.MarketViewUpdate
                Dim BAList As List(Of FFASuitePL.WP8FFAData) = Json.Deserialize(Of List(Of FFASuitePL.WP8FFAData))(trade.DataJson)
                RaiseEvent BidAskReceived(Me, BAList)
        End Select
    End Sub
    Public Sub TradesChannel_OnReceive(ByVal trade As SubscribeReceiveArgs)
        If trade.WasSentByMe Then
            Exit Sub
        End If

        If InvokeRequired Then
            BeginInvoke(CType(AddressOf TradesChannel_OnReceive, agsXMPP.ObjectHandler), New Object() {trade})
            Return
        End If
    End Sub
#End Region

#Region "RMI"

    Private Sub rd_MAIN_DockStateChanged(sender As Object, e As Docking.DockWindowEventArgs) Handles rd_MAIN.DockStateChanged
        If e.DockWindow.DockState = Docking.DockState.Floating Then
            e.DockWindow.FloatingParent.Icon = My.Resources.Table
            e.DockWindow.FloatingParent.ShowIcon = True
            e.DockWindow.FloatingParent.Font = New Font(e.DockWindow.FloatingParent.Font, FontStyle.Bold)
            e.DockWindow.FloatingParent.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
            e.DockWindow.FloatingParent.FormElement.TitleBar.IconPrimitive.Visibility = Telerik.WinControls.ElementVisibility.Visible
            Dim tooltabstrip As Telerik.WinControls.UI.Docking.ToolTabStrip = TryCast(e.DockWindow.Parent, Telerik.WinControls.UI.Docking.ToolTabStrip)
            tooltabstrip.TabStripElement.ContentArea.BackColor = rd_MAIN.BackColor
        ElseIf e.DockWindow.DockState = Docking.DockState.Docked Then
            Dim tooltabstrip As Telerik.WinControls.UI.Docking.ToolTabStrip = TryCast(e.DockWindow.Parent, Telerik.WinControls.UI.Docking.ToolTabStrip)
            Dim iconImage As Telerik.WinControls.Primitives.ImagePrimitive = New Telerik.WinControls.Primitives.ImagePrimitive
            iconImage.Image = My.Resources.Table_B16R
            Dim found As Boolean = False
            For Each r In tooltabstrip.CaptionElement.Children(2).Children
                If r.ToString = "ImagePrimitive" Then
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                tooltabstrip.CaptionElement.Children(2).Children.Insert(0, iconImage)
            End If
            tooltabstrip.TabStripElement.ContentArea.BackColor = rd_MAIN.BackColor
        ElseIf e.DockWindow.DockState = Docking.DockState.TabbedDocument Then
            Dim tab As Telerik.WinControls.UI.TabStripItem = e.DockWindow.TabStripItem
            tab.Image = My.Resources.Table_B16R
            Dim tabstrip As Telerik.WinControls.UI.Docking.DocumentTabStrip = TryCast(e.DockWindow.Parent, Telerik.WinControls.UI.Docking.DocumentTabStrip)
            tabstrip.TabStripElement.ContentArea.BackColor = rd_MAIN.BackColor
        End If
    End Sub

    Private Sub rd_MAIN_DockTabStripNeeded(sender As Object, e As Docking.DockTabStripNeededEventArgs) Handles rd_MAIN.DockTabStripNeeded
        If IsNothing(rd_MAIN.DocumentManager.ActiveDocument) Then Exit Sub
        If e.DockType = Docking.DockType.Document Then
            'If IsNothing(rd_MAIN.DocumentManager.ActiveDocument) Then Exit Sub
            'e.Strip = New Telerik.WinControls.UI.Docking.DocumentTabStrip
            'e.Strip.TabStripElement.ContentArea.BackColor = rd_MAIN.BackColor
        End If

        Dim strip As Docking.DockTabStrip
        If e.DockType = Docking.DockType.Document Then
            strip = New Docking.DocumentTabStrip()
        Else
            strip = New Docking.ToolTabStrip()
        End If
        strip.TabStripElement.ContentArea.Padding = New Padding(0)
        e.Strip = strip
        rd_MAIN.SplitPanelElement.Fill.Visibility = Telerik.WinControls.ElementVisibility.Collapsed
    End Sub

    Private Sub DockService_Stopping(sender As Object, e As Docking.StateServiceStoppingEventArgs) Handles DockService.Stopping
        Dim service As Telerik.WinControls.UI.Docking.StateService = DirectCast(sender, Telerik.WinControls.UI.Docking.StateService)
        If IsNothing(service) Then Exit Sub
        Dim DockWindow As Telerik.WinControls.UI.Docking.DockWindow = TryCast(service.Context, Telerik.WinControls.UI.Docking.DockWindow)
        If IsNothing(DockWindow) Then Exit Sub
        If DockWindow.DockState = Docking.DockState.TabbedDocument Then
            Try
                DockWindow.Image = My.Resources.Table_B16R
            Catch ex As Exception
                Stop
            End Try
        End If
    End Sub
    Private Sub rd_MAIN_DockWindowAdded(sender As Object, e As Docking.DockWindowEventArgs) Handles rd_MAIN.DockWindowAdded
        e.DockWindow.AllowedDockState = Docking.AllowedDockState.TabbedDocument Or Docking.AllowedDockState.Floating Or Docking.AllowedDockState.Docked Or Docking.AllowedDockState.Hidden
    End Sub
    Private Sub rd_MAIN_FloatingWindowCreated(sender As Object, e As Docking.FloatingWindowEventArgs) Handles rd_MAIN.FloatingWindowCreated
        e.Window.MaximizeBox = True
        e.Window.MinimizeBox = True
        e.Window.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
    End Sub
    Private Sub rmi_MarketWatch_Click(sender As Object, e As EventArgs) Handles rmi_MarketWatch.Click
        DisplayMarketViews()
    End Sub
    Private Sub DisplayMarketViews()
        Me.rd_MAIN.DocumentManager.DocumentInsertOrder = Telerik.WinControls.UI.Docking.DockWindowInsertOrder.ToBack
        Me.rd_MAIN.MdiChildrenDockType = Docking.DockType.ToolWindow

        For Each v In g_MarketViews.VIEWS
            Dim found As Boolean = False
            For Each r In Me.rd_MAIN.DockWindows
                Dim dw As Telerik.WinControls.UI.Docking.DockWindow = TryCast(r, Telerik.WinControls.UI.Docking.DockWindow)
                If dw.Text = v.NAME Then
                    found = True
                    dw.Select()
                    Exit For
                End If
            Next
            If found = False Then
                Dim f As New MarketWatch
                f.TabView = v
                f.Text = v.NAME
                f.EventForm = Me
                f.MdiParent = Me
                f.Show()
            End If
        Next
        rd_MAIN.DockWindows(0).Select()
    End Sub

    Private Sub rmi_settings_Click(sender As Object, e As EventArgs) Handles rmi_settings.Click
        ArtBConfigureForm.Show(Me)
    End Sub
    Private Sub rmi_SpotRates_Click(sender As Object, e As EventArgs) Handles rmi_SpotRates.Click
        rwb_WAIT.StartWaiting()
        Me.Cursor = Cursors.WaitCursor

        Dim bw As New System.ComponentModel.BackgroundWorker
        AddHandler bw.DoWork, AddressOf bw_Updates_DoWork
        AddHandler bw.RunWorkerCompleted, AddressOf bw_Updates_RunWorkerCompleted
        bw.RunWorkerAsync(FFAOptCalcService.VolRecordTypeEnum.spot)
    End Sub
    Private Sub rmi_SwapRates_Click(sender As Object, e As EventArgs) Handles rmi_SwapRates.Click
        rwb_WAIT.StartWaiting()
        Me.Cursor = Cursors.WaitCursor

        Dim bw As New System.ComponentModel.BackgroundWorker
        AddHandler bw.DoWork, AddressOf bw_Updates_DoWork
        AddHandler bw.RunWorkerCompleted, AddressOf bw_Updates_RunWorkerCompleted
        bw.RunWorkerAsync(FFAOptCalcService.VolRecordTypeEnum.swap)
    End Sub
    Private Sub rmi_NukeUsers_Click(sender As Object, e As EventArgs) Handles rmi_NukeUsers.Click
        Try
            If g_msgServiceType = MsgConnectivityServiceEnum.WS Then
                Dim msg As New FFAOptCalcService.FFAMessage
                msg.TimeStamp = Now
                msg.Username = "FFA Suite Client"
                msg.Payload = ""
                Dim tradechannel As New PublishArgs(My.Settings.ws_TradesChannel, Json.Serialize(msg), CStr(FFAOptCalcService.MessageEnum.CloseClient)) With _
                                                    {.OnFailure = Sub(p As PublishFailureArgs) MsgError(Me, "Failed to send command error: " & p.ErrorMessage, "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)}
                myWS.Publish(tradechannel)
            ElseIf g_msgServiceType = MsgConnectivityServiceEnum.BOSH Or g_msgServiceType = MsgConnectivityServiceEnum.TCP Then
                Dim msg As New agsXMPP.protocol.client.Message
                Dim m_Jid As New Jid("traders", "broadcast." & g_XMPPServerString, "FFA Opt Calc")
                msg.Subject = ArtB_Class_Library.ArtBMessages.DisconnectFFAOptionsUsers
                msg.Body = ""
                msg.To = m_Jid
                myXMPP.Send(msg)
            End If
        Catch ex As Exception
            MsgError(Me, "Failed to send command error: " & ex.Message, "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
        End Try
    End Sub
#End Region

#Region "UpdateSwapsSpots"
    Private Sub bw_Updates_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Dim worker As System.ComponentModel.BackgroundWorker = TryCast(sender, System.ComponentModel.BackgroundWorker)

        If worker.CancellationPending = True Then
            e.Result = 0
        End If

        e.Result = CInt(e.Argument)
        Select Case CInt(e.Argument)
            Case FFAOptCalcService.VolRecordTypeEnum.spot
                Dim newspots As New List(Of FFAOptCalcService.SpotFixingsClass)
                Try
                    newspots.AddRange(WEB.SDB.SpotFixings())
                    If newspots.Count > 0 Then
                        If g_msgServiceType = MsgConnectivityServiceEnum.WS Then
                            Dim tradechannel As New PublishArgs(My.Settings.ws_TradesChannel, Json.Serialize(newspots), CStr(FFAOptCalcService.MessageEnum.SpotRatesUpdate)) With _
                                                    {.OnFailure = Sub(p As PublishFailureArgs) MsgError(Me, "Failed to send SpotRates Update, error: " & p.ErrorMessage, "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)}
                            myWS.Publish(tradechannel)
                        ElseIf g_msgServiceType = MsgConnectivityServiceEnum.TCP Or g_msgServiceType = MsgConnectivityServiceEnum.BOSH Then
                            Dim msg As New agsXMPP.protocol.client.Message
                            Dim m_Jid As New Jid("traders", "broadcast." & g_XMPPServerString, "FFA Opt Calc")
                            msg.Subject = ArtB_Class_Library.ArtBMessages.SpotRatesUpdated
                            msg.Body = Json.Serialize(newspots)
                            msg.To = m_Jid
                            myXMPP.Send(msg)
                        End If
                    End If
                Catch ex As Exception
                    e.Cancel = True
                    MsgError(Me, "Failed to send command error: " & ex.Message, "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                End Try
            Case FFAOptCalcService.VolRecordTypeEnum.swap
                Dim newswaps As New List(Of FFAOptCalcService.SwapFixingsClass)
                newswaps.AddRange(WEB.SDB.SwapFixings())
                If newswaps.Count > 0 Then
                    Try
                        If g_msgServiceType = MsgConnectivityServiceEnum.WS Then
                            Dim tradechannel As New PublishArgs(My.Settings.ws_TradesChannel, Json.Serialize(newswaps), CStr(FFAOptCalcService.MessageEnum.SwapRatesUpdate)) With _
                                                    {.OnFailure = Sub(p As PublishFailureArgs) MsgError(Me, "Failed to send SwapRates Update, error: " & p.ErrorMessage, "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)}
                            myWS.Publish(tradechannel)
                        ElseIf g_msgServiceType = MsgConnectivityServiceEnum.TCP Or g_msgServiceType = MsgConnectivityServiceEnum.BOSH Then
                            Dim msg As New agsXMPP.protocol.client.Message
                            Dim m_Jid As New Jid("traders", "broadcast." & g_XMPPServerString, "FFA Opt Calc")
                            msg.Subject = ArtB_Class_Library.ArtBMessages.ForwardRatesUpdated
                            msg.Body = Json.Serialize(newswaps)
                            msg.To = m_Jid
                            myXMPP.Send(msg)
                        End If
                        'following will run idependently on the server to recreate all records in memory
                        WEB.SDB.BuildGDPostFixing(UD.MYFINGERPRINT.FINGER_PRINT)
                    Catch ex As Exception
                        e.Result = ex.Message
                        e.Cancel = True
                    End Try
                End If
        End Select
    End Sub
    Private Sub bw_Updates_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)

        rwb_WAIT.StopWaiting()
        rwb_WAIT.ResetWaiting()
        Me.Cursor = Cursors.Default

        If (e.Cancelled = True) Then
            Dim msg As String = "Failed to retrieve and send XMPP message." & vbCrLf & vbCrLf
            MsgError(Me, msg, "XMPP Update Failed", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        ElseIf Not (e.Error Is Nothing) Then
            Dim msg As String = "Failed to retrieve and send XMPP message." & vbCrLf & vbCrLf
            msg += "Error: " & e.Error.Message
            MsgError(Me, msg, "XMPP Update Failed", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
            Exit Sub
        End If
    End Sub
#End Region

    Private Sub FFAOptCalc_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
#If DEBUG Then
        rle_FormSatus.Text = "Width: " & Me.Size.Width & " Height: " & Me.Size.Height
#End If
    End Sub

    Private Sub rmi_HELP_MAIN_Click(sender As Object, e As EventArgs) Handles rmi_HELP_MAIN.Click
        Dim f As New WebHelpForm
        f.url = "HelpForm0.pdf"
        f.Show()
    End Sub

    Private Function TCPOpenPort(ByVal host As String, ByVal port As Integer) As Boolean
        Dim addr As IPAddress = CType(System.Net.Dns.GetHostAddresses(host)(0), IPAddress)
        Dim client As TcpClient = New TcpClient()
        client.SendTimeout = 20000
        client.ReceiveTimeout = 20000
        Try
            client.Connect(addr, port)
            TCPOpenPort = True
        Catch ex As SocketException
            TCPOpenPort = False
        Finally
            client.Close()
        End Try
        Return TCPOpenPort
    End Function
    Private Sub rmi_DayTrades_Click(sender As Object, e As EventArgs) Handles rmi_DayTrades.Click
        Dim f As New DayTrades
        f.Show()
    End Sub

End Class
