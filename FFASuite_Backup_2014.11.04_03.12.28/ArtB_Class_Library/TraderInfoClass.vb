Public Class TraderInfoClass
    Public TraderID As Integer = 0
    Public DeskID As Integer = 0
    Public MainDeskID As Integer = 0
    Public AccountID As Integer = 0
    Public AffBrokerID As Integer = 0
    Public BrokerID As Integer = 0
    Public AccountType As Integer = 0
    Public TraderName As String = ""
    Public DeskName As String = ""
    Public AccountName As String = ""

    Public TraderClass As DESK_TRADER_CLASS = Nothing
    Public DeskClass As ACCOUNT_DESK_CLASS = Nothing
    Public AccountClass As ACCOUNT_CLASS = Nothing

    Public IsTrader As Boolean = False
    Public IsDeskAdmin As Boolean = False
    Public IsSystemAdmin As Boolean = False
    Public IsBroker As Boolean = False
    Public IsAffiliateBroker As Boolean = False

    Public CanTrade(,) As Boolean

    Public UserName As String = ""
    Public OF_ID As String = ""

    Public TradeAuthority As Integer = 0
    Public DefaultNuke As Short
    Public ToolbarShow As Boolean
    Public DefaultMarket As Char
    Public DefaultShowNames As Boolean
    Public GridMarketDepth As Integer

    Public DefaultSSE As Boolean
    Public DefaultBI As Boolean = False

    Public BidColor As Integer = 0
    Public OfferColor As Integer = 0
    Public FontType As Short = 1

    Public IndicativesVisible As Boolean = False
    Public OneClickHit As Boolean = False

    Public Brokers As New Collection
    Public AffiliateBrokers As New Collection
    Public BrokerDefaultClient As Integer
    Public BrokerDefaultClientDeskID As Integer
    Public BrokerDefaultClientTraderID As Integer

    Public Sub AddTCBroker(ByVal TC As String, ByVal BrokerID As Integer)
        If Brokers.Contains(TC) Then
            Brokers.Remove(TC)
        End If
        Brokers.Add(BrokerID, TC)
    End Sub

    Public Sub AddTCAffiliateBroker(ByVal TC As String, ByVal BrokerID As Integer)
        If AffiliateBrokers.Contains(TC) Then
            AffiliateBrokers.Remove(TC)
        End If
        AffiliateBrokers.Add(BrokerID, TC)
    End Sub

    Public Function TCBrokerID(ByVal TC As String) As Integer
        TCBrokerID = NullInt2Int(GetViewClass(Brokers, TC))
    End Function

    Public Function TCAffiliateBrokerID(ByVal TC As String) As Integer
        TCAffiliateBrokerID = NullInt2Int(GetViewClass(Brokers, TC))
    End Function

End Class
