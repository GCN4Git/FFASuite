Public Class ArtBOptionsClasses

    Public Class ArtBOptionFromToClass
        Public ACCOUNT As New OptionsAccountClass
        Public FOR_ACCOUNT As New OptionsAccountClass

        Public Sub New(ByVal _ACCOUNT_DESK_ID As Integer, ByVal _FOR_ACCOUNT_DESK_ID As Integer, ByVal TRADE_CLASS_SHORT As Char, ByVal TRADED_EXCHANGES As Collection)

            Dim DB As New DB_ARTB_NETDataContext

            'ACCOUNT = New OptionsAccountClass
            Dim qr1 = (From q In DB.ACCOUNT_DESKs _
                       Join e In DB.ACCOUNTs On e.ACCOUNT_ID Equals q.ACCOUNT_ID _
                       Where q.ACCOUNT_DESK_ID = _ACCOUNT_DESK_ID _
                       Select q, e).FirstOrDefault
            ACCOUNT.ID = qr1.q.ACCOUNT_ID
            ACCOUNT.DESK_ID = qr1.q.ACCOUNT_DESK_ID
            ACCOUNT.BROKER_ID = qr1.e.BROKER_ID
            ACCOUNT.ACCOUNT_TYPE_ID = qr1.e.ACCOUNT_TYPE_ID
            If qr1.e.ACCOUNT_TYPE_ID = 2 Or qr1.e.ACCOUNT_TYPE_ID = 5 Then
                ACCOUNT.IsBroker = True
            Else
                ACCOUNT.IsBroker = False
            End If

            Dim qr2 = (From q In DB.DESK_TRADERs _
                        Where q.ACCOUNT_DESK_ID = _ACCOUNT_DESK_ID _
                        And q.IS_DESK_ADMIN = True _
                        Select q).First
            ACCOUNT.DESK_TRADER_ID = qr2.DESK_TRADER_ID


            'FOR_ACCOUNT = New OptionsAccountClass
            Dim qr3 = (From q In DB.ACCOUNT_DESKs _
                        Join e In DB.ACCOUNTs On e.ACCOUNT_ID Equals q.ACCOUNT_ID _
                        Where q.ACCOUNT_DESK_ID = _FOR_ACCOUNT_DESK_ID _
                        Select q, e).First
            FOR_ACCOUNT.ID = qr3.q.ACCOUNT_ID
            FOR_ACCOUNT.ACCOUNT_TYPE_ID = qr3.e.ACCOUNT_TYPE_ID
            FOR_ACCOUNT.DESK_ID = qr3.q.ACCOUNT_DESK_ID
            FOR_ACCOUNT.BROKER_ID = qr3.e.BROKER_ID
            If qr3.e.ACCOUNT_TYPE_ID = 2 Or qr3.e.ACCOUNT_TYPE_ID = 5 Then
                FOR_ACCOUNT.IsBroker = True
            Else
                FOR_ACCOUNT.IsBroker = False
            End If

            Dim qr4 = (From q In DB.DESK_TRADERs _
                            Where q.ACCOUNT_DESK_ID = _FOR_ACCOUNT_DESK_ID _
                            And q.IS_DESK_ADMIN = True _
                            Select q).First
            FOR_ACCOUNT.DESK_TRADER_ID = qr4.DESK_TRADER_ID

            FOR_ACCOUNT.DESK_ALLOWED_EXCHANGES.Clear()
            If FOR_ACCOUNT.IsBroker = False Then
                Dim qr5 = From q In DB.DESK_EXCHANGEs _
                          Where q.ACCOUNT_DESK_ID = _FOR_ACCOUNT_DESK_ID _
                          And q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT _
                          And q.ACTIVE = True _
                          Order By q.RANKING _
                          Select q
                For Each rec In qr5
                    For Each te As ExchangeClass In TRADED_EXCHANGES
                        If te.EXCHANGE_ID = rec.EXCHANGE_ID Then
                            Dim tec As ExchangeClass = te
                            tec.EXCHANGE_ORDER_INCLUDE = rec.ORDER_INCLUDE
                            tec.EXCHANGE_RANKING = rec.RANKING
                            Dim dec = From q In DB.DESK_EXCHANGES_CLEARERs _
                                      Join e In DB.ACCOUNTs On e.ACCOUNT_ID Equals q.ACCOUNT_ID _
                                      Where q.ACCOUNT_DESK_ID = _FOR_ACCOUNT_DESK_ID _
                                      And q.EXCHANGE_ID = te.EXCHANGE_ID _
                                      And q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT _
                                      And q.ACTIVE = True _
                                      Order By q.MAIN Descending, e.SHORT_NAME _
                                      Select q, e.SHORT_NAME
                            tec.CLEARERS.Clear()
                            For Each eclr In dec
                                Dim nclrc As New ExchangeClearerClass
                                nclrc.CLEARER_ID = eclr.q.ACCOUNT_ID
                                nclrc.MAIN = eclr.q.MAIN
                                nclrc.SHORT_NAME = eclr.SHORT_NAME
                                tec.CLEARERS.Add(nclrc, nclrc.CLEARER_ID)
                            Next
                            FOR_ACCOUNT.DESK_ALLOWED_EXCHANGES.Add(te, te.EXCHANGE_ID)
                            Exit For
                        End If
                    Next
                Next
            End If
        End Sub

    End Class

    Public Class ArtBOptionControlClass
        Inherits RouteOptionClass

        Public OPTION_TYPE As Integer
        Public YY1 As Integer
        Public MM1 As Integer
        Public YY2 As Integer
        Public MM2 As Integer
        Public PERIOD_DESCRIPTION As String
        Public OPTION_TYPE_DESCRIPTION As String
        Public OPTION_STRIKE_DESCRIPTION As String
        Public FULL_DESCRIPTION As String
        Public TAB_DESCRIPTION As String
        Public STRIKE_PRICE1 As Double
        Public STRIKE_PRICE2 As Double
        Public OptionTimeMonths As Integer

        Public Sub New(ByVal _ROUTE_ID As Integer, ByVal _OPTION_TYPE As OptionTypes, ByVal _YY1 As Integer, ByVal _MM1 As Integer, ByVal _YY2 As Integer, ByVal _MM2 As Integer)
            MyBase.new(_ROUTE_ID)
            OPTION_TYPE = _OPTION_TYPE
            YY1 = _YY1
            MM1 = _MM1
            YY2 = _YY2
            MM2 = _MM2
            GetOptionDescription()
        End Sub


        Public ReadOnly Property TABDescription() As String
            Get
                Return TAB_DESCRIPTION
            End Get
        End Property

        Public Sub GetOptionDescription()
            Dim dc As New ArtBTimePeriod
            Dim fs As String

            dc.FillMY(MM1, YY1, MM2, YY2)
            dc.CreateDescr()
            PERIOD_DESCRIPTION = dc.Descr

            fs = FormatStringPrice(PRICING_TICK)

            Select Case OPTION_TYPE
                Case OptionTypes.CallOption
                    OPTION_TYPE_DESCRIPTION = "Calls"
                    OPTION_STRIKE_DESCRIPTION = Format(STRIKE_PRICE1, fs)
                Case OptionTypes.PutOption
                    OPTION_TYPE_DESCRIPTION = "Puts"
                    OPTION_STRIKE_DESCRIPTION = Format(STRIKE_PRICE1, fs)
                Case OptionTypes.CollarOption
                    OPTION_TYPE_DESCRIPTION = "Collars"
                    OPTION_STRIKE_DESCRIPTION = Format(STRIKE_PRICE1, fs) & "/" & Format(STRIKE_PRICE2, fs)
                Case OptionTypes.StraddleOption
                    OPTION_TYPE_DESCRIPTION = "Straddles"
                    OPTION_STRIKE_DESCRIPTION = Format(STRIKE_PRICE1, fs)
                Case OptionTypes.StrangleOption
                    OPTION_TYPE_DESCRIPTION = "Strangles"
                    OPTION_STRIKE_DESCRIPTION = Format(STRIKE_PRICE1, fs) & "/" & Format(STRIKE_PRICE2, fs)
            End Select
            TAB_DESCRIPTION = ROUTE_SHORT & " " & PERIOD_DESCRIPTION & " " & OPTION_TYPE_DESCRIPTION
            FULL_DESCRIPTION = TAB_DESCRIPTION & " Strike(s): " & OPTION_STRIKE_DESCRIPTION

        End Sub

        Public Function GetOptionTimeMonths() As Integer
            GetOptionTimeMonths = DateAndTime.DateDiff(DateInterval.Month, DateAndTime.DateSerial(YY2, MM2, 28), DateAndTime.Today) + 1
        End Function
    End Class

    Public Class RouteOptionClass
        Public ROUTE_ID As Integer
        Public ROUTE_SHORT As String
        Public TRADE_CLASS_SHORT As Char
        Public VESSEL_CLASS_ID As Integer
        Public QUOTE_TYPE_ID As Integer
        Public QUOTE_TYPE_DES As String
        Public QUANTITY_TYPE_ID As String
        Public QUANTITY_TYPE_DES As String
        Public PRICING_TICK As Double
        Public LOT_SIZE As Integer
        Public CCY_SYMBOL As String
        Public TRADED_EXCHANGES As Collection

        Public Sub New(ByVal _ROUTE_ID As Integer)
            Dim DB As New DB_ARTB_NETDataContext

            ROUTE_ID = _ROUTE_ID

            TRADED_EXCHANGES = New Collection

            Dim qr = From q In DB.EXCHANGE_ROUTEs _
                     Join e In DB.EXCHANGEs On e.EXCHANGE_ID Equals q.EXCHANGE_ID _
                     Join f In DB.ROUTEs On f.ROUTE_ID Equals q.ROUTE_ID _
                     Join g In DB.CCies On g.CCY_ID Equals f.CCY_ID _
                     Join h In DB.QUOTE_TYPEs On h.QUOTE_TYPE_ID Equals f.QUOTE_TYPE _
                     Join i In DB.QUANTITY_TYPEs On i.QUANTITY_TYPE_ID Equals f.QUANTITY_TYPE _
                     Join j In DB.VESSEL_CLASSes On j.VESSEL_CLASS_ID Equals f.VESSEL_CLASS_ID _
                     Where q.ROUTE_ID = ROUTE_ID _
                     And q.ACTIVE = True _
                     And q.OPTIONS_AVAILABLE = True _
                     Order By q.EXCHANGE_ID _
                     Select q, e, f, g, h, i, j
            For Each r In qr
                ROUTE_SHORT = r.f.ROUTE_SHORT
                TRADE_CLASS_SHORT = r.j.DRYWET
                VESSEL_CLASS_ID = r.f.VESSEL_CLASS_ID
                QUOTE_TYPE_ID = r.f.QUOTE_TYPE
                QUOTE_TYPE_DES = r.h.QUOTE_TYPE_DES
                QUANTITY_TYPE_ID = r.i.QUANTITY_TYPE_ID
                QUANTITY_TYPE_DES = r.i.QUANTITY_TYPE_DES
                PRICING_TICK = r.f.PRICING_TICK
                LOT_SIZE = r.f.LOT_SIZE
                CCY_SYMBOL = r.g.CCY_SYMBOL
                Dim nc As New ExchangeClass
                nc.EXCHANGE_ID = r.q.EXCHANGE_ID
                nc.EXCHANGE_NAME_SHORT = r.e.EXCHANGE_NAME_SHORT
                nc.EXCHANGE_SHORCUT = r.e.EXCHANGE_SHORTCUT
                TRADED_EXCHANGES.Add(nc, nc.EXCHANGE_ID.ToString)
            Next
        End Sub
    End Class
    Public Class ExchangeClass
        Public EXCHANGE_ID As Integer
        Public EXCHANGE_NAME_SHORT As String
        Public EXCHANGE_SHORCUT As String
        Public EXCHANGE_RANKING As Integer
        Public EXCHANGE_ORDER_INCLUDE As Boolean
        Public CLEARERS As New Collection
    End Class
    Public Class ExchangeClearerClass       
        Public CLEARER_ID As Integer
        Public MAIN As Boolean
        Public SHORT_NAME As String
    End Class
    Public Class OptionsAccountClass
        Public ID As Integer
        Public DESK_ID As Integer
        Public DESK_TRADER_ID As Integer
        Public ACCOUNT_TYPE_ID As Integer
        Public BROKER_ID As Integer
        Public DESK_ALLOWED_EXCHANGES As New Collection
        Public IsBroker As Boolean
        Public DefaultTraderID As Integer
        Public TradingAuthority As Integer
    End Class

    Public Enum OptionContextMenu
        AddNewOrder = 1
        AddNewOrderBuy = 2
        AddNewOrderSell = 3
        AmmendOrder = 4
        DeleteOrder = 5
        SleepOrder = 6
        WakeUpOrder = 7
        SendToBroker = 9
        RetreiveFromBroker = 9
        SendToUser = 10
        RetreiveFromUser = 11
        DirectHit = 12
        DirectHitOnExchange = 13
        NegotiateHisTerms = 14
        NegotiateMyTerms = 15
        NegotiateHisTermsSusspendMine = 16
        NegotiateMyTermsSuspendMine = 17
        RequestFirmUp = 18
        RequestFirmUpSuspendMine = 19
    End Enum
    Public Enum OptionTypes
        CallOption = 1
        PutOption = 2
        CollarOption = 3
        StrangleOption = 4
        StraddleOption = 5
    End Enum
    Public Enum OptionOrderOrigin
        StandAlone = 1
        StrategyLinked = 2
    End Enum
    Public Enum OptionOrderQualifier
        Normal = 1
        Negotiated = 2
        Ammended = 3
        SendToUser = 4
        SendToBroker = 5
        RetreivedFromUser = 6
        RetreivedFromBroker = 7
        DeletedPermantly = 8
    End Enum
    Public Enum OptionOrderStatus
        Deleted = 0
        Alive = 1
        Sleeping = 2
        Executed = 3
        Negotiated = 4
        Ammended = 5
    End Enum
    Public Enum OptionPriceQualifier
        Firm = 1
        FirmCanDoBetter = 2
        Indicative = 3
    End Enum
    Public Enum OptionQantityQualifier
        Full = 1
        Half = 2
        Days = 3
    End Enum
    Public Enum OptionPriceFlag
        Firm = 1
        FirmTryBetter = 2
        Indicative = 3
        FirmTimer = 4
    End Enum
    Public Enum OptionQuantityFlag
        Fixed = 1
        Basket = 2
        CanDoLess = 3
        CanDoMore = 4
        Flexible = 5
    End Enum
    Public Enum OptionsGridImages
        BrokerOrderGreen
        BrokerOrderRed
        CountDownGreen
        CountDownRed
        LinkedOrderGray
        LinkedOrderGreen
        LinkedOrderRed
        OrderBuyOnly
        OrderBuySell
        OrderChatBlinkGreen
        OrderChatBlinkRed
        OrderLockedGray
        OrderLockedGreen
        OrderLockedRed
        OrderNegotiatedBlinkGreen
        OrderNegotiatedBlinkRed
        OrderNegotiatedGray
        OrderNegotiatedGreen
        OrderNegotiatedRed
        OrderSellOnly
        PlainGray
        PlainGreen
        PlainRed
        PriceCanDoLessGray
        PriceCanDoLessRed
        PriceCanDoMoreGray
        PriceCanDoMoreGreen
        PriceCanDoMoreRed
        PriceFixedGray
        PriceFixedGreen
        PriceFixedRed
        PriceIndicativeGray
        PriceIndicativeGreen
        PriceIndicativeRed
        QuantityBasketCyan
        QuantityBasketGray
        QuantityCanDoLessCyan
        QuantityCanDoLessGray
        QuantityCanDoMoreCyan
        QuantityCanDoMoreGray
        QuantityFixedCyan
        QuantityFixedGray
        QuantityFlexibleCyan
        QuantityFlexibleGray
    End Enum
    Public Enum OptionsShowNames
        Never = 1
        Always = 2
        Mutual = 3
    End Enum
    Public Enum OptionsOrderGT
        DayOrder = 1
        GTC = 2
        DayOrderTimer = 3
    End Enum

    Public Enum OptionsTrafficMsg
        AmmendOrder = 1
        DeleteOrder = 2
        ConfirmReceive = 3
        UpdateOrdersDB = 4
    End Enum
    Public NotInheritable Class OptionGridCRM
        Inherits StringEnumeration(Of OptionGridCRM)

        Public Shared ReadOnly ORDER_AMMEND As New OptionGridCRM("ORDER_AMMEND")
        Public Shared ReadOnly ORDER_SLEEP As New OptionGridCRM("ORDER_SLEEP")
        Public Shared ReadOnly ORDER_SUBMIT As New OptionGridCRM("ORDER_SUBMIT")
        Public Shared ReadOnly ORDER_DELETE As New OptionGridCRM("ORDER_DELETE")
        Public Shared ReadOnly DIRECT_HIT As New OptionGridCRM("DIRECT_HIT")
        Public Shared ReadOnly DIRECT_HIT_ON As New OptionGridCRM("DIRECT_HIT_ON")
        Public Shared ReadOnly NEGOTIATE_HIS As New OptionGridCRM("NEGOTIATE_HIS")
        Public Shared ReadOnly NEGOTIATE_MINE As New OptionGridCRM("NEGOTIATE_MINE")
        Public Shared ReadOnly ASK_INDICATIVE_FIRM As New OptionGridCRM("ASK_INDICATIVE_FIRM")
        Public Shared ReadOnly NEW_BUY_ORDER_SPECIFIC As New OptionGridCRM("NEW_BUY_ORDER_SPECIFIC")
        Public Shared ReadOnly NEW_SELL_ORDER_SPECIFIC As New OptionGridCRM("NEW_SELL_ORDER_SPECIFIC")
        Public Shared ReadOnly BROKER_SEND As New OptionGridCRM("BROKER_SEND")
        Public Shared ReadOnly BROKER_RETREIVE As New OptionGridCRM("BROKER_RETREIVE")
        Public Shared ReadOnly USER_SEND As New OptionGridCRM("USER_SEND")
        Public Shared ReadOnly USER_RETREIVE As New OptionGridCRM("USER_RETREIVE")
        
        Private Sub New(ByVal StringConstant As String)
            MyBase.New(StringConstant)
        End Sub
    End Class
End Class


