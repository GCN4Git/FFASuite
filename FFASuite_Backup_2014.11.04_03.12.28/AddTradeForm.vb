Imports ArtB_Class_Library
Imports agsXMPP
Imports agsXMPP.Xml
Imports agsXMPP.protocol
Imports agsXMPP.protocol.client
Imports FM
Imports FM.WebSync
Imports FM.WebSync.Chat
Imports FM.WebSync.Subscribers

Public Class AddTradeForm
    Private m_OldTrade As FFAOptCalcService.VolDataClass
    Private iThread As Integer = CInt(Int(1000000000 * Rnd() + 1000000000))
    Private UpdateString As String = String.Empty
    Private m_xmpp As XmppClientConnection
    Private m_myWS As Client
    Private m_FormMode As FormModeEnum = FormModeEnum.ReportTrade

    Public Enum FormModeEnum
        ReportTrade
        EditTrade
    End Enum
    Public Property myXMPP As XmppClientConnection
        Get
            Return m_xmpp
        End Get
        Set(value As XmppClientConnection)
            m_xmpp = value
        End Set
    End Property
    Public Property myWS As Client
        Get
            Return m_myWS
        End Get
        Set(value As Client)
            m_myWS = value
        End Set
    End Property
    Public Property OldTrade As FFAOptCalcService.VolDataClass
        Get
            Return m_OldTrade
        End Get
        Set(value As FFAOptCalcService.VolDataClass)
            m_OldTrade = value
        End Set
    End Property
    Public Property FormMode As FormModeEnum
        Get
            Return m_FormMode
        End Get
        Set(value As FormModeEnum)
            m_FormMode = value
        End Set
    End Property

    Private Sub AddTradeForm_Load(sender As Object, e As EventArgs) Handles Me.Load

    End Sub

    Private Sub rbtn_SUBMIT_Click(sender As Object, e As EventArgs) Handles rbtn_SUBMIT.Click
        If m_FormMode = FormModeEnum.ReportTrade Then
            ReportTrade()
        ElseIf m_FormMode = FormModeEnum.EditTrade Then
            Try
                m_OldTrade.FFA_PRICE = se_PRICE.Value
                If rrb_LIVE.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then
                    m_OldTrade.PNC = False
                    m_OldTrade.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.live
                Else
                    m_OldTrade.PNC = True
                    m_OldTrade.VolRecordType = FFAOptCalcService.VolRecordTypeEnum.level
                End If
                If WEB.SDB.AmmendReportedTrade(OldTrade) = False Then
                    MsgError(Me, "Trade failed processing in the server.", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
                    Me.Close()
                Else
                    Select g_msgServiceType
                        Case MsgConnectivityServiceEnum.WS
                            Dim p As New PublishArgs(My.Settings.ws_TradesChannel, Json.Serialize(m_OldTrade), CStr(FFAOptCalcService.MessageEnum.AmmendReportedTrade)) With _
                                        {.OnFailure = Sub(er As PublishFailureArgs)
                                                          MsgError(Me, "Trade Report failed to be sent to server." & Environment.NewLine & "Error: " & er.ErrorMessage, "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                                                      End Sub}
                            Try
                                myWS.Publish(p)
                            Catch ex As Exception
                                MsgError(Me, "Trade Report failed to be sent to server.", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
                            Finally
                                Me.Close()
                            End Try
                        Case MsgConnectivityServiceEnum.TCP, MsgConnectivityServiceEnum.BOSH
                            Dim msg As New agsXMPP.protocol.client.Message
                            Dim m_Jid As New Jid("traders", "broadcast." & g_XMPPServerString, "FFA Opt Calc")
                            msg.Subject = ArtB_Class_Library.ArtBMessages.AmmendReportedTrade
                            msg.Body = Json.Serialize(m_OldTrade)
                            msg.To = m_Jid
                            Try
                                m_xmpp.Send(msg)
                                Me.Close()
                            Catch ex As Exception
                                MsgError(Me, "Trade Report failed to be sent to server.", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
                            Finally
                                Me.Close()
                            End Try
                    End Select
                End If
            Catch ex As Exception
                MsgError(Me, "Trade Report failed to be sent to server.", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
            Finally
                Me.Close()
            End Try
        End If
    End Sub

    Private Sub ReportTrade()
        Dim t As New ORDERS_FFA_CLASS
        t.THREAD = iThread
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

        t.PRICE_TYPE = "F"
        t.PRICE_TRY_BETTER = False
        t.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Fixed
        t.QUANTITY_STEP = 1
        t.ORDER_DATETIME = DateTime.UtcNow
        t.PRICE_INDICATED = se_PRICE.Value
        t.ROUTE_ID = OldTrade.ROUTE_ID
        t.MM1 = OldTrade.MM1
        t.MM2 = OldTrade.MM2
        t.YY1 = OldTrade.YY1
        t.YY2 = OldTrade.YY2
        t.ORDER_BS = "B"
        t.ORDER_QUANTITY = 1
        t.DAY_QUALIFIER = 0
        t.SHORTDES = New ArtBTimePeriod(OldTrade.YY1, OldTrade.MM1, OldTrade.YY2, OldTrade.MM2).Descr
        t.DESK_TRADER_ID = 1 'SystemDeskTraderId
        t.FOR_DESK_TRADER_ID = 1 'SystemDeskTraderId
        t.PNC_ORDER = True
        If rrb_LIVE.ToggleState = Telerik.WinControls.Enumerations.ToggleState.On Then t.PNC_ORDER = False
        t.AppendToStr(UpdateString)

        t.ORDER_BS = "S"
        t.AppendToStr(UpdateString)

        Select Case g_msgServiceType
            Case MsgConnectivityServiceEnum.WS
                Dim msg As New FFAOptCalcService.FFAMessage
                msg.TimeStamp = Now
                msg.Payload = UpdateString
                msg.Username = t.THREAD
                Dim p As New PublishArgs(My.Settings.ws_TradesChannel, Json.Serialize(msg), CStr(FFAOptCalcService.MessageEnum.TradeAnnouncement)) With _
                                        {.OnFailure = Sub(e As PublishFailureArgs)
                                                          MsgError(Me, "Trade Report failed to be sent to server." & Environment.NewLine & "Error: " & e.ErrorMessage, "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                                                      End Sub}
                'p.OnFailure = AddressOf ws_MessageFailed
                Try
                    myWS.Publish(p)
                Catch ex As Exception
                    MsgError(Me, "Trade Report failed to be sent to server.", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
                Finally
                    Me.Close()
                End Try
            Case MsgConnectivityServiceEnum.TCP, MsgConnectivityServiceEnum.BOSH
                Dim msg As New agsXMPP.protocol.client.Message
                Dim m_Jid As New agsXMPP.Jid(My.Settings.OrdersJid & "@" & g_XMPPServerString)
                msg.Subject = ArtB_Class_Library.ArtBMessages.FicticiousTrade
                msg.Body = UpdateString
                msg.Thread = iThread
                msg.To = m_Jid
                Try
                    m_xmpp.Send(msg)
                    Me.Close()
                Catch ex As Exception
                    MsgError(Me, "Trade Report failed to be sent to server.", "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Info)
                Finally
                    Me.Close()
                End Try
        End Select
    End Sub
    Private Sub ws_MessageFailed(ByVal e As PublishFailureArgs)
        If InvokeRequired Then
            BeginInvoke(CType(AddressOf ws_MessageFailed, agsXMPP.ObjectHandler), New Object() {e})
            Return
        End If
        Dim nf As New FFAOptCalc
        nf.ThemeClassName = "Office2010SilverTheme"
        Dim s As String = "Trade Report failed to be sent to server."
        s += Environment.NewLine & "Error: " & e.ErrorMessage
        MsgError(nf, s, "Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
        nf.Dispose()
        Me.Close()
    End Sub
End Class
