
Public Class BROKER_DESK_TRADE_CLASS_CLASS
    Inherits BROKER_DESK_TRADE_CLASS
End Class

Public Class TRADE_CLASS_EXCHANGE_CLASS
    Inherits TRADE_CLASS_EXCHANGE
End Class

Public Class TRADE_CLASS_CLASS
    Inherits TRADE_CLASS

    Public EXCHANGES As New Collection
    Public VESSELCLASSES As New Collection
    Public BROKER_DESKS As New Collection
    Public RATIO_SPREADS As New Collection
End Class

Public Class PERIOD_LIMIT_DESCR_CLASS
    Inherits PERIOD_LIMIT_DESCR
End Class

Public Class SHOW_NAME_TYPE_CLASS
    Inherits SHOW_NAME_TYPE
End Class

Public Class QUOTE_TYPE_CLASS
    Inherits QUOTE_TYPE
End Class

Public Class QUANTITY_TYPE_CLASS
    Inherits QUANTITY_TYPE
End Class

Public Class CCY_CLASS
    Public CCY_ID As Integer
    Public CCY As String
    Public CCY_DESCR As String
    Public CCY_SYMBOL As String
    Public CCY_UNICODE As Integer
End Class

Public Class COUNTRIES_CLASS
    Public COUNTRY_ID As Integer
    Public COUNTRY_ISO As String
    Public COUNTRY_UN As String
    Public COUNTRY_DESCR As String
End Class

Public Class MATCHING_ORDERS_CLASS
    Public OrderClass As Object
    Public ActualQuantity As Double
    Public Exchanges As List(Of Integer) = Nothing
End Class

Public Class CONTACT_TYPE_CLASS
    Public CONTACT_TYPE_ID As Integer
    Public CONTACT_TYPEDES As String
End Class

Public Class ACCOUNT_TYPES_CLASS
    Public ACCOUNT_TYPE_ID As Integer
    Public ACCOUNT_TYPE_DESCR As String
End Class

Public Class PRICE_CLASS
    Public Price As Double
    Public Price2 As Double = 0
    Public Time As DateTime
    Public bTrade As Boolean = True
    Public Tick As Integer = Ticks.None
    Public Ficticious As Boolean = False
    Public PNC As Boolean = False
    Public TradeId As Integer = 0
End Class

Public Class EMAIL_CLASS
    Public Addr As String
    Public Body As String
    Public BrokerStr As String
    Public LongBrokerStr As String
End Class

Public Class SMS_CLASS
    Public MobileNum As String
    Public SMSList As List(Of String)
    Public BrokerStr As String
    Public LongBrokerStr As String
End Class

Public Class REF_CLASS
    Public RPSTR As String = ""
    Public BuyPrice As Double = -1.0E+20
    Public SellPrice As Double = 1.0E+20
    Public AvgPrice As Double = 1.0E+20
    Public LastTrade As Double = 1.0E+20
    Public TradeDateTime As DateTime = "1/1/2000"
    Public Volatility As Double = -1.0E+20
    Public SynthVolatility As Boolean = True
    Public LastIsClose As Boolean = False

    Public Sub New(ByVal a_RPStr As String)
        RPSTR = a_RPStr
    End Sub

    Public Sub AssignOrderPrice(ByRef o As ORDERS_FFA_CLASS, ByVal PriceTick As Double)
        If Math.Abs(o.PRICE_INDICATED) > 1.0E+19 Then Exit Sub
        If Math.Abs(o.ORDER_TYPE) <> OrderTypes.FFA Then Exit Sub
        If o.ORDER_BS = "B" Then
            If o.PRICE_INDICATED > BuyPrice Then BuyPrice = o.PRICE_INDICATED
        Else
            If o.PRICE_INDICATED < SellPrice Then SellPrice = o.PRICE_INDICATED
        End If
        If Math.Abs(BuyPrice) < 1.0E+19 And Math.Abs(SellPrice) < 1.0E+19 Then
            AvgPrice = CInt(((BuyPrice + SellPrice) * 0.5) / PriceTick) * PriceTick
        ElseIf Math.Abs(BuyPrice) < 1.0E+19 Then
            AvgPrice = BuyPrice
        ElseIf Math.Abs(SellPrice) < 1.0E+19 Then
            AvgPrice = SellPrice
        End If
    End Sub

    Public Sub AssignTradePrice(ByRef t As Object, ByVal PriceTick As Double)
        If Math.Abs(AvgPrice) < 1.0E+19 Then
            If t.ORDER_DATETIME < TradeDateTime Then Exit Sub
            LastTrade = t.PRICE_TRADED
            AvgPrice = t.PRICE_TRADED
            AvgPrice = CInt(AvgPrice / PriceTick) * PriceTick
            TradeDateTime = t.ORDER_DATETIME
            Exit Sub
        End If
        If t.ORDER_DATETIME < TradeDateTime Then Exit Sub
        LastTrade = t.PRICE_TRADED
        AvgPrice = t.PRICE_TRADED
        AvgPrice = CInt(AvgPrice / PriceTick) * PriceTick
        TradeDateTime = t.ORDER_DATETIME
    End Sub

    Public Sub AssignBalticPrice(ByRef t As Object, ByVal PriceTick As Double)
        If Math.Abs(LastTrade) > 1.0E+19 Then LastTrade = CInt(t.FIXING / PriceTick) * PriceTick
        If Math.Abs(AvgPrice) < 1.0E+19 Then Exit Sub
        LastIsClose = True
        AvgPrice = t.FIXING
        AvgPrice = CInt(AvgPrice / PriceTick) * PriceTick
    End Sub


End Class

Public Class AdjustSpreadsClass
    Public q As ORDERS_FFA_CLASS = Nothing
    Public ACCOUNT_DESK_ID As Integer = 0
    Public SETTLEMENT_TICK As Double = 0
    Public LOT_SIZE As Double = 0
    Public s As ORDERS_FFA_CLASS = Nothing

    Public LegBS As String = ""
    Public LegPrice As Double = 1.0E+20
    Public LegDir As Integer = -1
    Public AffectedSide As Integer = 1
    Public MatchingSide As Integer = 2

    Public SpreadLegQuantity As Integer = 0
    Public AffectedLegQuantinty As Integer = 0

    Public CurrPrice As Double = 0
    Public CurrQ As Integer = 0
    Public LegQ As Integer = 0

    Public MinPrice As Double = 1
    Public MaxPrice As Double = 200000
    Public AffectedLeg As Object = Nothing
    Public Spread As Object = Nothing
End Class
