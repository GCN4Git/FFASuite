Imports System
Imports agsXMPP
Imports agsXMPP.Net
Imports agsXMPP.Xml
Imports agsXMPP.protocol
Imports agsXMPP.protocol.client
Imports FM
Imports FM.TheRest
Imports FM.WebSync
Imports FM.WebSync.Chat
Imports ArtB_Class_Library
Imports System.Net.Sockets
Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security

Public Class FFSuiteRepeaterService

    Private WithEvents xmpp As XmppClientConnection
    Private WithEvents ws As FM.WebSync.Client
    Private WithEvents RESTClient As FM.TheRest.Client
    Private WithEvents xmpptimer As New System.Timers.Timer(10000)
    Private WithEvents wstimer As New System.Timers.Timer(5000)
    'Private WithEvents msgtimer As New System.Timers.Timer(30000)
    Private GVC As New GlobalViewClass
    Private xmpp_server As String
    Private ws_Server As String
    Private xmppmsglock As New Object
    Private wsquelock As New Object
    Private wsMessageQue As New Queue(Of PublishArgs)
    Private wsTimerFunction As wsTimerFunctionEnum = wsTimerFunctionEnum.Connect

    Private Enum wsTimerFunctionEnum
        Connect
        KeepAlive
    End Enum

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        'ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertficate)
#If DEBUG Then
        xmpp_server = My.Settings.xmpp_serverdemo
        ws_Server = My.Settings.ws_serverdemo
#Else
        xmpp_server = My.Settings.xmpp_server
        ws_Server = My.Settings.ws_server
#End If

        wstimer.Enabled = True
        wstimer.Start()

        xmpptimer.Enabled = True
        xmpptimer.Start()

#If DEBUG Then
		RESTClient = new FM.TheRest.Client("http://localhost:1959/therest.ashx")
#Else
        RESTClient = New FM.TheRest.Client("https://www.artbsystems.com/FFASuiteRestServer/therest.ashx")
#End If


    End Sub
    Private Function ValidateServerCertficate(ByVal sender As Object, ByVal cert As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function
    Protected Overrides Sub OnStop()
        Try
            xmpp.Close()
        Catch ex As Exception
        End Try
        Try
            ws.Disconnect()
        Catch ex As Exception
        End Try
    End Sub

#Region "wsMessageEvents"
    Public Sub General_OnReceive(ByVal general As SubscribeReceiveArgs)
        If general.WasSentByMe Then
            Exit Sub
        End If
    End Sub
    Public Sub Trades_OnReceive(ByVal trade As SubscribeReceiveArgs)
        If trade.WasSentByMe Then
            Exit Sub
        End If

        Select Case trade.Tag
            Case DataContracts.MessageEnum.AmmendReportedTrade
                Dim msg As New agsXMPP.protocol.client.Message
                Dim m_Jid As New Jid("traders", "broadcast." & xmpp.Server, "FFA Opt Calc")
                msg.Subject = ArtB_Class_Library.ArtBMessages.AmmendReportedTrade
                msg.Body = trade.DataJson
                msg.To = m_Jid
                xmpp.Send(msg)
            Case DataContracts.MessageEnum.TradeAnnouncement
                Try
                    Dim ffamsg = Json.Deserialize(Of DataContracts.FFAMessage)(trade.DataJson)
                    If ffamsg.Payload = "" Then
                        Exit Sub
                    End If

                    Dim msg As New agsXMPP.protocol.client.Message
                    Dim m_Jid As New agsXMPP.Jid(My.Settings.OrdersJid & "@" & xmpp.Server)
                    msg.Subject = ArtB_Class_Library.ArtBMessages.FicticiousTrade
                    msg.Body = ffamsg.Payload
                    msg.Thread = CInt(ffamsg.Username)
                    msg.To = m_Jid
                    xmpp.Send(msg)
                Catch ex As Exception
#If DEBUG Then
                    Stop
#End If
                End Try
            Case DataContracts.MessageEnum.SpotRatesUpdate
                Try
                    Dim msg As New agsXMPP.protocol.client.Message
                    Dim m_Jid As New Jid("traders", "broadcast." & xmpp.Server, "XMPP Server Repeater")
                    msg.Subject = ArtB_Class_Library.ArtBMessages.SpotRatesUpdated
                    msg.Body = trade.DataJson
                    msg.To = m_Jid
                    xmpp.Send(msg)
                Catch ex As Exception
#If DEBUG Then
                    Stop
#End If
                End Try
            Case DataContracts.MessageEnum.SwapRatesUpdate
                Try
                    Dim msg As New agsXMPP.protocol.client.Message
                    Dim m_Jid As New Jid("traders", "broadcast." & xmpp.Server, "XMPP Server Repeater")
                    msg.Subject = ArtB_Class_Library.ArtBMessages.ForwardRatesUpdated
                    msg.Body = trade.DataJson
                    msg.To = m_Jid
                    xmpp.Send(msg)
                Catch ex As Exception
#If DEBUG Then
                    Stop
#End If
                End Try
            Case DataContracts.MessageEnum.CloseClient
                Try
                    Dim msg As New agsXMPP.protocol.client.Message
                    Dim m_Jid As New Jid("traders", "broadcast." & xmpp.Server, "XMPP Server Repeater")
                    msg.Subject = ArtB_Class_Library.ArtBMessages.DisconnectFFAOptionsUsers
                    msg.Body = "XMPP Server Repeater"
                    msg.To = m_Jid
                    xmpp.Send(msg)
                Catch ex As Exception
#If DEBUG Then
                    Stop
#End If
                End Try
            Case DataContracts.MessageEnum.MarketViewUpdate
                Try
                    Dim msg As New agsXMPP.protocol.client.Message
                    Dim m_Jid As New Jid("traders", "broadcast." & xmpp.Server, "XMPP Server Repeater")
                    msg.Subject = ArtB_Class_Library.ArtBMessages.BidAskUpdate
                    msg.Body = trade.DataJson
                    msg.To = m_Jid
                    xmpp.Send(msg)
                Catch ex As Exception
#If DEBUG Then
                    Stop
#End If
                End Try
        End Select
    End Sub

    Private Sub wsSendSubcriptions()
        If IsNothing(ws) Then Exit Sub
        If ws.IsConnected = False Then Exit Sub

        SyncLock wsquelock
            Try
                For Each r In wsMessageQue
                    ws.Publish(wsMessageQue.Dequeue)
                Next
            Catch
            End Try
        End SyncLock
    End Sub

#End Region

#Region "xmppMessageEvents"
    Private Sub xmpp_OnMessage(sender As Object, xmppmsg As protocol.client.Message) Handles xmpp.OnMessage
        If IsNothing(xmppmsg) Then Exit Sub
        If xmppmsg.Body = "" Then Exit Sub

        Dim msgtype = CInt(xmppmsg.Subject)

        Select Case msgtype
            Case ArtBMessages.AmmendReportedTrade
                Dim p As New PublishArgs(My.Settings.ws_generalchannel, xmppmsg.Body, CStr(DataContracts.MessageEnum.AmmendReportedTrade))
                SyncLock wsquelock
                    wsMessageQue.Enqueue(p)
                End SyncLock
            Case ArtBMessages.OrderFFATrade
                GVC.UpdateFromMesage(xmppmsg.Body, msgtype, New TraderInfoClass)
                Dim c As Collection = GVC.ParseString(xmppmsg.Body, ArtBMessages.OrderFFATrade)
                Dim ListTrades As New List(Of TRADES_FFA_CLASS)
                For Each Trade As TRADES_FFA_CLASS In c
                    Dim alreadyinserted = (From q In ListTrades Where q.TRADE_ID = Trade.TRADE_ID Select q).FirstOrDefault
                    If IsNothing(alreadyinserted) = True Then
                        ListTrades.Add(Trade)
                    End If
                Next
                Dim tradelist As New List(Of DataContracts.VolDataClass)
                Dim mobilelist As New List(Of DataContracts.VolDataClass)
                For Each Trade In ListTrades
                    If Trade.TRADE_TYPE = ArtB_Class_Library.OrderTypes.FFA Then
                        Dim ntrade As New DataContracts.VolDataClass
                        ntrade.DESK_TRADER_ID = Trade.DESK_TRADER_ID1
                        ntrade.TRADE_ID = Trade.TRADE_ID
                        ntrade.TRADE_TYPE = Trade.TRADE_TYPE
                        ntrade.ROUTE_ID = CInt(Trade.ROUTE_ID)
                        If Trade.PNC = False Then
                            ntrade.VolRecordType = DataContracts.VolRecordTypeEnum.live
                        Else
                            ntrade.VolRecordType = DataContracts.VolRecordTypeEnum.level
                        End If
                        ntrade.PERIOD = Trade.SHORTDES
                        ntrade.FIXING_DATE = Trade.ORDER_DATETIME
                        ntrade.FFA_PRICE = CDbl(Trade.PRICE_TRADED)
                        ntrade.YY1 = CInt(Trade.YY1)
                        ntrade.YY2 = CInt(Trade.YY2)
                        ntrade.MM1 = CInt(Trade.MM1)
                        ntrade.MM2 = CInt(Trade.MM2)
                        ntrade.TRADE_TYPE = Trade.TRADE_TYPE
                        tradelist.Add(ntrade)
                        mobilelist.Add(ntrade)
                    End If

                    If Trade.TRADE_TYPE <> ArtB_Class_Library.OrderTypes.FFA Then
                        If Trade.UPDATE_STATUS <> 0 Then Continue For

                        Dim ntrade As New DataContracts.VolDataClass
                        ntrade.DESK_TRADER_ID = Trade.DESK_TRADER_ID1
                        ntrade.TRADE_ID = Trade.TRADE_ID
                        ntrade.TRADE_TYPE = Trade.TRADE_TYPE
                        ntrade.ROUTE_ID = CInt(Trade.ROUTE_ID)
                        ntrade.ROUTE_ID2 = CInt(Trade.ROUTE_ID2)
                        If Trade.PNC = False Then
                            ntrade.VolRecordType = DataContracts.VolRecordTypeEnum.live
                        Else
                            ntrade.VolRecordType = DataContracts.VolRecordTypeEnum.level
                        End If
                        ntrade.PERIOD = Trade.SHORTDES
                        ntrade.FIXING_DATE = Trade.ORDER_DATETIME
                        ntrade.FFA_PRICE = CDbl(Trade.PRICE_TRADED)
                        ntrade.YY1 = CInt(Trade.YY1)
                        ntrade.YY2 = CInt(Trade.YY2)
                        ntrade.MM1 = CInt(Trade.MM1)
                        ntrade.MM2 = CInt(Trade.MM2)
                        ntrade.YY21 = CInt(Trade.YY21)
                        ntrade.YY22 = CInt(Trade.YY22)
                        ntrade.MM21 = CInt(Trade.MM21)
                        ntrade.MM22 = CInt(Trade.MM22)
                        ntrade.TRADE_TYPE = Trade.TRADE_TYPE
                        tradelist.Add(ntrade)
                    End If
                Next                
                SyncLock wsquelock
                    Dim p As New PublishArgs(My.Settings.ws_generalchannel, Json.Serialize(tradelist), CStr(DataContracts.MessageEnum.TradeAnnouncement))
                    wsMessageQue.Enqueue(p)

                    Dim PostTrades As SendArgs = New SendArgs(HttpMethod.Post, "/PostArtBTrades")
                    PostTrades.DataJson = Json.Serialize(Of List(Of DataContracts.VolDataClass))(mobilelist)
                    Try
                        RESTClient.Send(PostTrades)
                    Catch ex As Exception
                        Debug.Print(ex.Message)
                    End Try
                End SyncLock
            Case ArtBMessages.SpotRatesUpdated
                Dim p As New PublishArgs(My.Settings.ws_generalchannel, xmppmsg.Body, CStr(DataContracts.MessageEnum.SpotRatesUpdate))
                SyncLock wsquelock
                    wsMessageQue.Enqueue(p)
                End SyncLock
            Case ArtBMessages.ForwardRatesUpdated
                Dim p As New PublishArgs(My.Settings.ws_generalchannel, xmppmsg.Body, CStr(DataContracts.MessageEnum.SwapRatesUpdate))
                SyncLock wsquelock
                    wsMessageQue.Enqueue(p)
                End SyncLock
            Case ArtBMessages.DisconnectFFAOptionsUsers
                Dim nmsg As New DataContracts.FFAMessage
                nmsg.TimeStamp = Now
                nmsg.Payload = "Disconnect Users"
                nmsg.Username = "XMPP Repeater Service"
                Dim p As New PublishArgs(My.Settings.ws_generalchannel, Json.Serialize(nmsg), CStr(DataContracts.MessageEnum.CloseClient))
                SyncLock wsquelock
                    wsMessageQue.Enqueue(p)
                End SyncLock
            Case ArtBMessages.BidAskUpdate
                Dim DB As New ArtBDataContext
                Dim UpdateDB As Boolean = False
                Dim BAlist As List(Of FFASuitePL.WP8FFAData) = Json.Deserialize(Of List(Of FFASuitePL.WP8FFAData))(xmppmsg.Body)
                Dim UT As Date = Date.Now
                For Each r In BAlist
                    If r.PRICE_STATUS = FFASuitePL.PriceStatusEnum.Level Then
                        Dim nr As New ARTBOPTCALC_FFA_QUOTES
                        nr.FIXING_DATE = UT
                        nr.ROUTE_ID = r.ROUTE_ID
                        nr.YY1 = r.YY1
                        nr.YY2 = r.YY2
                        nr.MM1 = r.MM1
                        nr.MM2 = r.MM2
                        nr.BID = r.BID
                        nr.ASK = r.ASK
                        DB.ARTBOPTCALC_FFA_QUOTES.InsertOnSubmit(nr)
                        UpdateDB = True
                    End If
                Next
                If UpdateDB = True Then
                    DB.SubmitChanges()
                End If
                DB.Dispose()

                Try
                    Dim p As New PublishArgs(My.Settings.ws_generalchannel, xmppmsg.Body, CStr(DataContracts.MessageEnum.MarketViewUpdate))
                    SyncLock wsquelock
                        wsMessageQue.Enqueue(p)
                    End SyncLock
                Catch ex As Exception
#If DEBUG Then
                    Stop
#End If
                End Try
        End Select
        wsSendSubcriptions()
    End Sub

#End Region

    Private Sub NewOrder(ByVal BS As Char, ByVal ROUTE_ID As Integer, ByVal YY1 As Integer, ByVal YY2 As Integer, ByVal MM1 As Integer, ByVal MM2 As Integer, ByVal PRC As Double)
        Dim UpdateString As String = String.Empty
        Dim t As New ORDERS_FFA_CLASS
        t.THREAD = CInt(Int(1000000000 * Rnd() + 1000000000))
        t.ORDER_ID = 0
        t.ORDERED_BY_WHO = "S"
        t.LIVE_STATUS = "A"
        t.SINGLE_EXCHANGE_EXECUTION = False
        t.ORDER_TYPE = OrderTypes.FFA
        t.ORDER_GOOD_TILL = OrderGoodTill.Day
        t.ORDER_QUALIFIER = " "
        t.ORDER_EXCHANGES = "10"
        For i As Integer = 1 To 3
            t.ORDER_EXCHANGES &= "_"
        Next
        t.ORDER_BS = BS
        t.PRICE_TYPE = "F"
        t.PRICE_TRY_BETTER = False
        t.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Fixed
        t.QUANTITY_STEP = 1
        t.ORDER_DATETIME = DateTime.UtcNow
        t.PRICE_INDICATED = PRC
        t.ROUTE_ID = ROUTE_ID
        t.MM1 = MM1
        t.MM2 = MM2
        t.YY1 = YY1
        t.YY2 = YY2
        t.ORDER_BS = BS
        t.ORDER_QUANTITY = 1
        t.DAY_QUALIFIER = 0
        t.SHORTDES = New FFASuiteRepeater.ArtBTimePeriod(YY1, MM1, YY2, MM2).Descr
        t.DESK_TRADER_ID = 1 'SystemDeskTraderId
        t.FOR_DESK_TRADER_ID = 1 'SystemDeskTraderId
        t.PNC_ORDER = False                
        t.AppendToStr(UpdateString)
    End Sub
#Region "wsConnectEvents"
    Private Sub ws_connect()
        ws = New FM.WebSync.Client(ws_Server)
        Dim conargs As New ConnectArgs
        Dim cred As New DataContracts.FFASuiteCredentials
        cred.FINGERPRINT = "FFASUITEREPEATERSERVER"
        cred.PRODUCT_ID = "2809"
        cred.TimeStamp = Today
        cred.SubscribedRoutes = New List(Of Integer)({36, 38})
        conargs.OnStreamFailure = AddressOf ws_Disconnected
        conargs.MetaJson = Json.Serialize(cred)
        ws.Connect(conargs)
    End Sub
    Private Sub ws_Disconnected(d As StreamFailureArgs)
        wstimer.Stop()
        wstimer.Enabled = False
    End Sub
    Private Sub ws_OnConnectFailure(p As ConnectFailureArgs) Handles ws.OnConnectFailure
        Debug.Print("Connection failure to WS Server")
        Debug.Print(p.ErrorMessage)
    End Sub
    Private Sub ws_OnConnectSuccess(p As ConnectSuccessArgs) Handles ws.OnConnectSuccess

        wstimer.Interval = 15000
        wsTimerFunction = wsTimerFunctionEnum.KeepAlive
        wstimer.Enabled = True
        wstimer.Start()

        'If p.IsReconnect = True Then Exit Sub

        If ws.GetSubscribedChannels.Contains(My.Settings.ws_generalchannel) = False Then
            Dim general As New SubscribeArgs(My.Settings.ws_generalchannel)
            general.OnSuccess = AddressOf General_Subscribed
            general.OnReceive = AddressOf General_OnReceive
            ws.Subscribe(general)
        End If

        If ws.GetSubscribedChannels.Contains(My.Settings.ws_tradeschannel) = False Then
            Dim trades As New SubscribeArgs(My.Settings.ws_tradeschannel)
            trades.OnSuccess = AddressOf Trades_Subscribed
            trades.OnReceive = AddressOf Trades_OnReceive
            ws.Subscribe(trades)
        End If

        Debug.Print("connected to WS Server")
    End Sub
    Public Sub General_Subscribed(ByVal result As SubscribeSuccessArgs)
        Debug.Print("Subscribed to my/general")
    End Sub
    Public Sub Trades_Subscribed(ByVal Trades As SubscribeSuccessArgs)
        Debug.Print("Subscribed to my/trades")
    End Sub
    Private Sub wstimer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles wstimer.Elapsed
        If wsTimerFunction = wsTimerFunctionEnum.Connect Then
            ws_connect()
        ElseIf wsTimerFunction = wsTimerFunctionEnum.KeepAlive Then
            If ws.IsConnected = True Then
                Dim p As New PublishArgs(My.Settings.ws_tradeschannel, Json.Serialize("HeartBeat"), CStr(99999999999))
                ws.Publish(p)
            End If
        End If
    End Sub
#End Region
#Region "xmppConnectEvents"
    Private Sub xmpp_connect()
        xmpp = New XmppClientConnection
        Dim myjid As agsXMPP.Jid = New agsXMPP.Jid(My.Settings.xmpp_user & "@" & xmpp_server)
        xmpp.Server = myjid.Server
        xmpp.Username = myjid.User
        xmpp.Password = My.Settings.xmpp_password
        xmpp.Resource = "XMPP Server Repeater"
        xmpp.Priority = 0
        xmpp.Port = 5222
        xmpp.KeepAlive = True
        xmpp.KeepAliveInterval = 30
        xmpp.UseCompression = True
        xmpp.Open()
    End Sub
    Private Sub xmpp_OnXmppConnectionStateChanged(sender As Object, state As XmppConnectionState) Handles xmpp.OnXmppConnectionStateChanged
        If state = XmppConnectionState.Disconnected Then
            xmpptimer.Enabled = True
            xmpptimer.Start()
        End If
    End Sub
    Private Sub xmpp_OnLogin(sender As Object) Handles xmpp.OnLogin
        xmpptimer.Stop()
        xmpptimer.Enabled = False
        xmpptimer.Interval = 15000
        Debug.Print("Connected to XMPP server")
    End Sub
    Private Sub xmpptimer_Elapsed(sender As Object, e As Timers.ElapsedEventArgs) Handles xmpptimer.Elapsed
        xmpp_connect()
    End Sub
#End Region

End Class
