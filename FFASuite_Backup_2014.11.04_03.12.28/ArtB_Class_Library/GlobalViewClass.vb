Imports System.Data.SqlClient

Public Class GlobalViewClass
    Public IsDemo As Integer = 0
    Public Event OrderFFAUpdated(ByVal a_RouteIdList As List(Of Integer), ByVal a_OrderIdList As List(Of Integer))
    Public Event OrdersUpdated(ByVal a_bRefreshGrid As Boolean)

    Public Event TradesUpdated(ByVal TradeList As List(Of Integer), ByVal a_MessageType As Integer)
    Public Event TradeInfo(ByVal a_OrderId As Integer, ByVal dir As Integer)

    Public Event TimeLimitOrder(ByVal OrderId As Integer, ByVal Seconds As Integer)
    Public Event OrderFFARequest(ByVal a_OrderId As Integer)
    Public Event OrderFFARequestRespond(ByVal a_OrderId As Integer, ByVal a_CounterOrderId As Integer, ByVal a_Response As Boolean)
    Public Event OrderFFANegotiation(ByVal a_OrderId As Integer)
    Public Event OrderFFAChangeOwner(ByVal a_OrderId As Integer)
    Public Event OrderFFADirectHitFailed(ByVal a_OrderId As Integer)
    Public Event UpdateSpreadLegs(ByVal AffectedRoutePeriodsStr As String)

    Public OperationMode As Integer = GVCOpMode.Service
    Public GlobalDateTimeDiff As TimeSpan

    Public ACCOUNTS As New Collection
    Public EXCHANGES As New Collection
    Public SHOW_NAME_TYPES As New Collection
    Public PERIOD_LIMIT_DESCRS As New Collection
    Public ACCOUNT_DESKS As New Collection
    Public CONTACTS As New Collection
    Public DESK_TRADERS_BY_OF As New Collection
    Public DESK_TRADERS As New Collection
    Public ROUTES As New Collection
    Public VESSEL_CLASSES As New Collection
    Public Mode As Integer
    Public AccountID As Integer
    Public DeskID As Integer
    Public TraderID As Integer
    Public MainDeskID As Integer
    Public LAST_DAY_RULES As New Collection
    Public EXCHANGE_ROUTE_PERIODS As New Collection
    Public HOLIDAYS As New Collection
    Public ORDERS_FFAS As New Collection
    Public QUOTE_TYPES As New Collection
    Public TRADES_FFAS As New Collection
    Public TRADE_CLASSES As New Collection
    Public TRADE_CLASS_EXCHANGES As New Collection
    Public BALTIC_FORWARD_RATES As New Collection
    Public QUANTITY_TYPES As New Collection
    Public VESSEL_CLASS_SPREAD_MARGINS As New Collection
    Public TotalExchanges As Integer = 7

    Public bUpdateDBFromNewOrder As Boolean = False
    Public NextUpdateDBFromNewOrder As Integer = 0
    Public ExecUpdateDBFromNewOrder As Integer = 0

    Public bGetAll As Boolean = False
    Public MaxBalticDate As Date = "1-1-2000"
    Public RefCol As New Collection

    Public Sub New()
        ArtBNumberInfoInit()
        Mode = GlobalViewModes.Admin
        AccountID = 14
        DeskID = 1039
        TraderID = 28
        MainDeskID = 0
        GlobalDateTimeDiff = DateTime.UtcNow - DateTime.UtcNow
    End Sub

    Public Sub SetConnectionString(Optional ByVal ServerStr As String = "", Optional ByVal UserStr As String = "", Optional ByVal PswStr As String = "")
        If ServerStr = "" Then
            ArtBConnectionStr = "Data Source=artbtrading.com;Initial Catalog=ARTB;Persist Security Info=True;User ID=gmf;Password=@egean@ndr0s"
        Else
            ArtBConnectionStr = ServerStr
        End If
    End Sub

    Public Function GetConnectionString() As String
        Return ArtBConnectionStr
    End Function

    Public Function GetNewConnection() As DB_ARTB_NETDataContext
        GetNewConnection = New DB_ARTB_NETDataContext(ArtBConnectionStr)
    End Function

    Public Function GetServiceCache(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetServiceCache = ArtBErrors.Success
        Try
            GetRatioSpreadPrecisionFromDB(gdb)
            GetTradeClasses(gdb)
            GetContacts(gdb)
            GetAccounts(gdb)
            GetAccountDesks(gdb)
            GetDeskTraders(gdb)
        Catch ex As Exception
            Debug.Print(ex.ToString)
            GetServiceCache = ArtBErrors.ConnectionDead
        End Try

    End Function

    Public Function GetAll(Optional ByRef gdb As DB_ARTB_NETDataContext = Nothing) As Integer
        Dim bHasCreatedConnection As Boolean = False
        If IsNothing(gdb) Then
            gdb = New DB_ARTB_NETDataContext(ArtBConnectionStr)
            bHasCreatedConnection = True
        End If
        GetAll = ArtBErrors.Success
        If bGetAll Then Exit Function
        bGetAll = True
        Try
            'GetRatioSpreadPrecisionFromDB(gdb)
            GetTradeClasses(gdb)
            'GetQuoteTypes(gdb)
            'GetQuantityTypes(gdb)
            GetPeriodLimitDescrs(gdb)
            'GetShowNameTypes(gdb)
            'GetContacts(gdb)
            GetExchanges(gdb)
            GetHolidays(gdb)
            GetLastDayRules(gdb)
            GetLastDayRuleMonths(gdb)
            GetExchangeRoutePeriods(gdb)
            'GetAccounts(gdb)
            'GetAccountDesks(gdb)
            'GetDeskTraders(gdb)
            'GetDeskTradeClasses(gdb)
            'GetDeskExchanges(gdb)
            GetVesselClasses(gdb)
            GetRoutes(gdb)
            GetExchangeRoutes(gdb)
            'GetDeskExchangesClearers(gdb)
            'GetBalticForwardRates(gdb)
            'GetVesselClassSpreadMargins(gdb)
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
        Return True
    End Function

    Public Sub GetTradeClasses(ByRef gdb As DB_ARTB_NETDataContext)
        Dim tCLASS As TRADE_CLASS_CLASS
        Dim bCLASS As BROKER_DESK_TRADE_CLASS_CLASS
        Dim rsCLASS As TRADE_CLASS_RATIO_SPREAD_CLASS

        Dim TRADE_CLASS_List = (From q In gdb.TRADE_CLASSes _
                                Select q).ToList()

        TRADE_CLASSES.Clear()
        For Each q In TRADE_CLASS_List
            tCLASS = New TRADE_CLASS_CLASS
            tCLASS.TRADE_CLASS_SHORT = q.TRADE_CLASS_SHORT
            tCLASS.TRADE_CLASS_DES = q.TRADE_CLASS_DES
            tCLASS.TRADE_CLASS = q.TRADE_CLASS
            tCLASS.EXCHANGES.Clear()
            tCLASS.VESSEL_CLASSes.Clear()
            Dim c As Char = tCLASS.TRADE_CLASS_SHORT
            Dim teCLASS As TRADE_CLASS_EXCHANGE_CLASS
            Dim TRADE_CLASS_EXCHANGE_List = (From r In gdb.TRADE_CLASS_EXCHANGEs _
                      Where r.TRADE_CLASS_SHORT = c Order By r.RANKING_ORDER Select r).ToList

            TRADE_CLASS_EXCHANGES.Clear()
            For Each r In TRADE_CLASS_EXCHANGE_List
                teCLASS = New TRADE_CLASS_EXCHANGE_CLASS
                teCLASS.TRADE_CLASS_SHORT = r.TRADE_CLASS_SHORT
                teCLASS.EXCHANGE_ID = r.EXCHANGE_ID
                tCLASS.EXCHANGES.Add(teCLASS, teCLASS.EXCHANGE_ID.ToString())
            Next

            Dim brokerList = (From qr In gdb.BROKER_DESK_TRADE_CLASSes _
                                Where qr.TRADE_CLASS_SHORT = c Select qr).ToList()

            tCLASS.BROKER_DESKS.Clear()
            For Each qr In brokerList
                bCLASS = New BROKER_DESK_TRADE_CLASS_CLASS
                bCLASS.TRADE_CLASS_SHORT = qr.TRADE_CLASS_SHORT
                bCLASS.ACCOUNT_DESK_ID = qr.ACCOUNT_DESK_ID
                tCLASS.BROKER_DESKS.Add(bCLASS, bCLASS.ACCOUNT_DESK_ID.ToString())
            Next

            Dim RatioSpreadList = (From qrs In gdb.TRADE_CLASS_RATIO_SPREADs _
                                    Where qrs.TRADE_CLASS_SHORT = c Select qrs).ToList
            tCLASS.RATIO_SPREADS.Clear()
            Dim rsi As Integer = 0
            For Each qrs In RatioSpreadList
                rsCLASS = New TRADE_CLASS_RATIO_SPREAD_CLASS
                rsCLASS.GetFromObject(qrs)
                tCLASS.RATIO_SPREADS.Add(rsCLASS, rsi.ToString)
                rsi = rsi + 1
            Next

            TRADE_CLASSES.Add(tCLASS, tCLASS.TRADE_CLASS_SHORT)
        Next
    End Sub

    Public Sub GetQuoteTypes(ByRef gdb As DB_ARTB_NETDataContext)
        Dim QUOTE_TYPE As QUOTE_TYPE_CLASS
        Dim QUOTE_TYPE_List = (From q In gdb.QUOTE_TYPEs _
                  Order By q.QUOTE_TYPE_ID _
                  Select q).ToList

        QUOTE_TYPES.Clear()
        For Each q In QUOTE_TYPE_List
            QUOTE_TYPE = New QUOTE_TYPE_CLASS
            QUOTE_TYPE.QUOTE_TYPE_ID = q.QUOTE_TYPE_ID
            QUOTE_TYPE.QUOTE_TYPE_DES = q.QUOTE_TYPE_DES
            QUOTE_TYPES.Add(QUOTE_TYPE, QUOTE_TYPE.QUOTE_TYPE_ID.ToString())
        Next
    End Sub

    Public Sub GetQuantityTypes(ByRef gdb As DB_ARTB_NETDataContext)

        Dim Quantity_TYPE As QUANTITY_TYPE_CLASS
        Dim Quantity_TYPE_List = (From q In gdb.QUANTITY_TYPEs _
                  Order By q.QUANTITY_TYPE_ID _
                  Select q).ToList

        QUANTITY_TYPES.Clear()
        For Each q In Quantity_TYPE_List
            Quantity_TYPE = New QUANTITY_TYPE_CLASS
            Quantity_TYPE.QUANTITY_TYPE_ID = q.QUANTITY_TYPE_ID
            Quantity_TYPE.QUANTITY_TYPE_DES = q.QUANTITY_TYPE_DES
            QUANTITY_TYPES.Add(Quantity_TYPE, Quantity_TYPE.QUANTITY_TYPE_ID.ToString())
        Next
    End Sub

 
    Public Sub GetShowNameTypes(ByRef gdb As DB_ARTB_NETDataContext)
        Dim SHOW_NAME_TYPE As SHOW_NAME_TYPE_CLASS
        Dim SHOW_NAME_TYPE_List = (From q In gdb.SHOW_NAME_TYPEs _
                  Order By q.SHOW_NAME_ID _
                  Select q).ToList()

        SHOW_NAME_TYPES.Clear()
        For Each q In SHOW_NAME_TYPE_List
            SHOW_NAME_TYPE = New SHOW_NAME_TYPE_CLASS
            SHOW_NAME_TYPE.SHOW_NAME_ID = q.SHOW_NAME_ID
            SHOW_NAME_TYPE.SHOW_NAME = q.SHOW_NAME
            SHOW_NAME_TYPES.Add(SHOW_NAME_TYPE, SHOW_NAME_TYPE.SHOW_NAME_ID.ToString())
        Next
    End Sub

    Public Sub GetPeriodLimitDescrs(ByRef gdb As DB_ARTB_NETDataContext)
        Dim PERIOD_LIMIT_C As PERIOD_LIMIT_DESCR_CLASS
        Dim PERIOD_LIMIT_DESCR_List = (From q In gdb.PERIOD_LIMIT_DESCRs _
                  Order By q.PERIOD_LIMIT _
                  Select q).ToList()

        PERIOD_LIMIT_DESCRS.Clear()
        For Each q In PERIOD_LIMIT_DESCR_List
            PERIOD_LIMIT_C = New PERIOD_LIMIT_DESCR_CLASS
            PERIOD_LIMIT_C.PERIOD_LIMIT = q.PERIOD_LIMIT
            PERIOD_LIMIT_C.PERIOD_LIMIT_DESCR = q.PERIOD_LIMIT_DESCR
            PERIOD_LIMIT_DESCRS.Add(PERIOD_LIMIT_C, PERIOD_LIMIT_C.PERIOD_LIMIT.ToString())
        Next
    End Sub

    Public Sub GetRoutes(ByRef gdb As DB_ARTB_NETDataContext)
        Dim r As ROUTE_CLASS
        Dim l = (From q In gdb.ROUTEs _
                  Order By q.ROUTE_ID _
                  Select q).ToList()

        ROUTES.Clear()
        For Each q In l
            r = New ROUTE_CLASS
            r.GetFromObject(q)
            r.EXCHANGES.Clear()
            ROUTES.Add(r, r.ROUTE_ID.ToString())
        Next
    End Sub

    Public Sub GetExchangeRoutePeriods(ByRef gdb As DB_ARTB_NETDataContext)

        Dim r As EXCHANGE_ROUTE_PERIOD_CLASS
        Dim l = (From q In gdb.EXCHANGE_ROUTE_PERIODs _
                  Order By q.EXCHANGE_ROUTE_PERIOD_ID _
                  Select q).ToList()

        EXCHANGE_ROUTE_PERIODS.Clear()
        For Each q In l
            r = New EXCHANGE_ROUTE_PERIOD_CLASS
            r.GetFromObject(q)
            EXCHANGE_ROUTE_PERIODS.Add(r, r.EXCHANGE_ROUTE_PERIOD_ID.ToString())
        Next
    End Sub

    Public Sub GetHolidays(ByRef gdb As DB_ARTB_NETDataContext)

        Dim r As HOLIDAY_CLASS
        Dim l = (From q In gdb.HOLIDAYs _
                  Order By q.HOLIDAY _
                  Select q).ToList()
        HOLIDAYS.Clear()
        For Each q In l
            r = New HOLIDAY_CLASS
            r.GetFromObject(q)
            HOLIDAYS.Add(r, r.HOLIDAY.ToString(ARTB_DATE_FORMATSTR))
        Next
    End Sub

    Public Sub GetLastDayRules(ByRef gdb As DB_ARTB_NETDataContext)
        Dim r As LAST_DAY_RULE_CLASS
        Dim l = (From q In gdb.LAST_DAY_RULEs _
                  Order By q.LAST_DAY_RULE_ID _
                  Select q).ToList()

        LAST_DAY_RULES.Clear()
        For Each q In l
            r = New LAST_DAY_RULE_CLASS
            r.GetFromObject(q)
            r.MONTHS.Clear()
            LAST_DAY_RULES.Add(r, r.LAST_DAY_RULE_ID.ToString())
        Next
    End Sub

    Public Sub GetLastDayRuleMonths(ByRef gdb As DB_ARTB_NETDataContext)
        Dim r As LAST_DAY_RULE_MONTH_CLASS
        Dim l = (From q In gdb.LAST_DAY_RULE_MONTHs _
                  Order By q.LAST_DAY_RULE_ID, q.MONTH _
                  Select q).ToList()

        For Each q In l
            r = New LAST_DAY_RULE_MONTH_CLASS
            r.GetFromObject(q)
            Dim LastDayRule As LAST_DAY_RULE_CLASS = GetViewClass(LAST_DAY_RULES, r.LAST_DAY_RULE_ID.ToString())
            If Not LastDayRule Is Nothing Then
                LastDayRule.MONTHS.Add(r, r.MONTH.ToString())
            End If
        Next
    End Sub

    Public Function GetRouteClass(ByVal a_ROUTE_ID As Integer) As ROUTE_CLASS
        Try
            GetRouteClass = ROUTES(a_ROUTE_ID.ToString())
        Catch e As Exception
            GetRouteClass = Nothing
        End Try
    End Function

    Public Sub GetExchangeRoutes(ByRef gdb As DB_ARTB_NETDataContext)
        Dim r As EXCHANGE_ROUTE_CLASS
        Dim l = (From q In gdb.EXCHANGE_ROUTEs _
                  Order By q.ROUTE_ID _
                  Select q).ToList
        Dim rc As ROUTE_CLASS

        For Each q In l
            r = New EXCHANGE_ROUTE_CLASS
            r.GetFromObject(q)
            rc = GetRouteClass(q.ROUTE_ID)
            If Not rc Is Nothing Then
                rc.EXCHANGES.Add(r, r.EXCHANGE_ID.ToString())
            End If
        Next
    End Sub

    Public Sub GetVesselClasses(ByRef gdb As DB_ARTB_NETDataContext)
        Dim r As VESSEL_CLASS_CLASS
        Dim l = (From q In gdb.VESSEL_CLASSes _
                  Order By q.VESSEL_CLASS_ID _
                  Select q).ToList

        VESSEL_CLASSES.Clear()
        For Each q In l
            r = New VESSEL_CLASS_CLASS
            r.GetFromObject(q)
            VESSEL_CLASSES.Add(r, r.VESSEL_CLASS_ID.ToString())
            Dim tc As TRADE_CLASS_CLASS = GetViewClass(TRADE_CLASSES, r.DRYWET)
            If Not IsNothing(tc) Then
                tc.VESSELCLASSES.Add(r, r.VESSEL_CLASS_ID.ToString())
            End If
        Next
    End Sub

    Public Sub GetVesselClassSpreadMargins(ByRef gdb As DB_ARTB_NETDataContext)
        Dim r As VESSEL_CLASS_SPREAD_MARGIN_CLASS
        Dim l = (From q In gdb.VESSEL_CLASS_SPREAD_MARGINs _
                Join Route In gdb.ROUTEs On q.VESSEL_CLASS_ID Equals Route.VESSEL_CLASS_ID _
                  Order By q.VESSEL_CLASS_ID _
                  Select q, Route).ToList

        VESSEL_CLASS_SPREAD_MARGINS.Clear()
        For Each v In l
            Dim RPStr As String = RoutePeriodStringFromObj(v.q, 1, v.Route)
            If VESSEL_CLASS_SPREAD_MARGINS.Contains(RPStr) Then Continue For
            r = New VESSEL_CLASS_SPREAD_MARGIN_CLASS
            r.GetFromObject(v.q)
            VESSEL_CLASS_SPREAD_MARGINS.Add(r, RPStr)
        Next
    End Sub

    Public Sub GetBalticForwardRates(ByRef gdb As DB_ARTB_NETDataContext)
        MaxBalticDate = "1-1-2000"
        Dim l = (From q In gdb.BALTIC_FORWARD_RATES_VIEWs Select q).ToList
        Dim RPStr As String
        BALTIC_FORWARD_RATES.Clear()
        For Each q In l
            Dim bfr As New BALTIC_FORWARD_RATE_CLASS
            If bfr.GetFromObject(q) <> ArtBErrors.Success Then
                bfr = Nothing
                Continue For
            End If
            RPStr = RoutePeriodStringFromObj(q)
            Try
                If BALTIC_FORWARD_RATES.Contains(RPStr) Then
                    Dim bfr2 As BALTIC_FORWARD_RATE_CLASS = GetViewClass(BALTIC_FORWARD_RATES, RPStr)
                    If bfr2.FIXING_DATE > bfr.FIXING_DATE Then
                        bfr = Nothing
                        Continue For
                    Else
                        BALTIC_FORWARD_RATES.Remove(RPStr)
                    End If
                End If
                BALTIC_FORWARD_RATES.Add(bfr, RPStr)
                If MaxBalticDate < bfr.FIXING_DATE Then MaxBalticDate = bfr.FIXING_DATE
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try
            bfr = Nothing
        Next
    End Sub

    Public Function GetAccountClass(ByVal a_ACCOUNT_ID As Integer) As ACCOUNT_CLASS
        Try
            GetAccountClass = ACCOUNTS(a_ACCOUNT_ID.ToString)
        Catch e As Exception
            GetAccountClass = Nothing
        End Try
    End Function

    Public Function GetAccountDeskClass(ByVal a_ACCOUNT_DESK_ID As Integer) As ACCOUNT_DESK_CLASS
        Try
            GetAccountDeskClass = ACCOUNT_DESKS(a_ACCOUNT_DESK_ID.ToString)
        Catch e As Exception
            GetAccountDeskClass = Nothing
        End Try
    End Function

    Public Function GetDeskTradeClassClass(ByVal a_ACCOUNT_DESK_ID As Integer, _
                                           ByVal a_TRADE_CLASS_SHORT As String) _
                                           As DESK_TRADE_CLASS_CLASS
        GetDeskTradeClassClass = Nothing
        Dim GetAccountDeskClass As ACCOUNT_DESK_CLASS = Nothing
        Try
            GetAccountDeskClass = ACCOUNT_DESKS(a_ACCOUNT_DESK_ID.ToString)
        Catch e As Exception
        End Try
        If IsNothing(GetAccountDeskClass) Then Exit Function
        Try
            GetDeskTradeClassClass = GetAccountDeskClass.TRADE_CLASSES(a_TRADE_CLASS_SHORT)
        Catch e As Exception
        End Try
    End Function

    Public Sub GetAccounts(ByRef gdb As DB_ARTB_NETDataContext)
        Dim ACCOUNT As ACCOUNT_CLASS
        Dim l = (From q In gdb.ACCOUNTs _
                  Order By q.ACCOUNT_ID _
                  Select q).ToList

        ACCOUNTS.Clear()
        For Each q In l
            ACCOUNT = New ACCOUNT_CLASS
            ACCOUNT.GetFromObject(q)
            ACCOUNT.DESKS.Clear()
            ACCOUNTS.Add(ACCOUNT, ACCOUNT.ACCOUNT_ID.ToString())
        Next

    End Sub

    Public Sub GetAccountDesks(ByRef gdb As DB_ARTB_NETDataContext)
        Dim DESK As ACCOUNT_DESK_CLASS
        Dim ACCOUNT As ACCOUNT_CLASS
        Dim AccountDeskList = (From q In gdb.ACCOUNT_DESKs _
                  Select q).ToList
        ACCOUNT_DESKS.Clear()
        For Each q In AccountDeskList
            DESK = New ACCOUNT_DESK_CLASS
            DESK.GetFromObject(q)
            ACCOUNT_DESKS.Add(DESK, DESK.ACCOUNT_DESK_ID.ToString())
            If OperationMode = GVCOpMode.Client Then
                ACCOUNT = GetAccountClass(q.ACCOUNT_ID)
                If Not (ACCOUNT Is Nothing) Then
                    ACCOUNT.DESKS.Add(DESK, DESK.ACCOUNT_DESK_ID.ToString())
                End If
            End If
            DESK.TRADERS.Clear()
            DESK.COUNTER_PARTY_LIMITS.Clear()
            DESK.TRADE_CLASSES.Clear()
        Next
    End Sub

    Public Sub GetCounterParties(ByRef UserInfo As TraderInfoClass, _
                                 ByRef gdb As DB_ARTB_NETDataContext)
        Dim PRI_DESK As ACCOUNT_DESK_CLASS
        Dim COUNTER_PARTY_CLASS As COUNTERPARTY_LIMIT_CLASS
        Dim l As Object

        If UserInfo.IsTrader Then
            l = (From q In gdb.COUNTERPARTY_LIMITs _
                Where q.PRI_ACCOUNT_DESK_ID = DeskID Or q.PRI_ACCOUNT_DESK_ID = MainDeskID _
                Or q.SEC_ACCOUNT_DESK_ID = DeskID Or q.SEC_ACCOUNT_DESK_ID = MainDeskID _
                Select q).ToList
        Else
            l = (From q In gdb.COUNTERPARTY_LIMITs _
                Select q).ToList
        End If
        For Each q In l
            COUNTER_PARTY_CLASS = New COUNTERPARTY_LIMIT_CLASS
            COUNTER_PARTY_CLASS.GetFromObject(q)

            PRI_DESK = GetAccountDeskClass(q.PRI_ACCOUNT_DESK_ID)
            If Not (PRI_DESK Is Nothing) Then
                PRI_DESK.COUNTER_PARTY_LIMITS.Add(COUNTER_PARTY_CLASS, COUNTER_PARTY_CLASS.SEC_ACCOUNT_DESK_ID.ToString())
            End If
        Next
    End Sub

    Public Sub GetAccountInfo(ByVal a_DeskId As Integer, _
                              ByRef a_AccountID As Integer, _
                              ByRef AccountFullName As String)
        a_AccountID = 0
        AccountFullName = ""
        If Not ACCOUNT_DESKS.Contains(a_DeskId.ToString()) Then Exit Sub
        Dim Desk As ACCOUNT_DESK_CLASS = ACCOUNT_DESKS(a_DeskId.ToString())
        If Desk Is Nothing Then Exit Sub
        a_AccountID = Desk.ACCOUNT_ID
        If Not ACCOUNTS.Contains(a_AccountID.ToString()) Then Exit Sub
        Dim Account As ACCOUNT_CLASS = ACCOUNTS(a_AccountID.ToString())
        If Account Is Nothing Then Exit Sub
        AccountFullName = Account.SHORT_NAME
    End Sub

    Public Sub GetDeskTraders(ByRef gdb As DB_ARTB_NETDataContext)
        Dim TRADER As DESK_TRADER_CLASS
        Dim DESK As ACCOUNT_DESK_CLASS
        Dim l = (From q In gdb.DESK_TRADERs _
                  Select q).ToList
        DESK_TRADERS.Clear()
        DESK_TRADERS_BY_OF.Clear()
        For Each q In l
            TRADER = New DESK_TRADER_CLASS
            TRADER.GetFromObject(q)
            If OperationMode = GVCOpMode.Client Then
                TRADER.CONTACT_DETAILS = GetContactClass(TRADER.CONTACT_ID)
            End If
            DESK = GetAccountDeskClass(TRADER.ACCOUNT_DESK_ID)
            If Not (DESK Is Nothing) Then
                DESK.TRADERS.Add(TRADER, TRADER.DESK_TRADER_ID.ToString())
            End If
            DESK_TRADERS_BY_OF.Add(TRADER, TRADER.OF_ID)
            DESK_TRADERS.Add(TRADER, TRADER.DESK_TRADER_ID.ToString())
        Next

    End Sub

    Public Sub GetDeskTradeClasses(ByRef gdb As DB_ARTB_NETDataContext)
        Dim de As DESK_TRADE_CLASS_CLASS
        Dim DESK As ACCOUNT_DESK_CLASS
        Dim l = (From q In gdb.DESK_TRADE_CLASSes _
                  Select q).ToList

        For Each q In l
            de = New DESK_TRADE_CLASS_CLASS
            de.GetFromObject(q)
            DESK = GetAccountDeskClass(de.ACCOUNT_DESK_ID)
            de.EXCHANGES.Clear()
            If Not IsNothing(DESK) Then
                DESK.TRADE_CLASSES.Add(de, de.TRADE_CLASS_SHORT)
            End If
        Next

    End Sub
    Public Sub GetDeskExchanges(ByRef gdb As DB_ARTB_NETDataContext)
        Dim de As DESK_EXCHANGE_CLASS
        Dim DTC As DESK_TRADE_CLASS_CLASS
        Dim l = (From q In gdb.DESK_EXCHANGEs _
                  Select q).ToList

        For Each q In l
            de = New DESK_EXCHANGE_CLASS
            de.GetFromObject(q)
            DTC = GetDeskTradeClassClass(de.ACCOUNT_DESK_ID, de.TRADE_CLASS_SHORT)
            de.CLEARERS.Clear()
            If Not (DTC Is Nothing) Then
                DTC.EXCHANGES.Add(de, de.EXCHANGE_ID.ToString())
            End If
        Next

    End Sub

    Public Sub GetDeskExchangesClearers(ByRef gdb As DB_ARTB_NETDataContext)
        Dim de As DESK_EXCHANGES_CLEARER_CLASS
        Dim DTC As DESK_TRADE_CLASS_CLASS
        Dim DESK_EXCHANGE As DESK_EXCHANGE_CLASS
        Dim l = (From q In gdb.DESK_EXCHANGES_CLEARERs _
                  Select q).ToList

        For Each q In l
            de = New DESK_EXCHANGES_CLEARER_CLASS
            de.GetFromObject(q)
            DTC = GetDeskTradeClassClass(de.ACCOUNT_DESK_ID, de.TRADE_CLASS_SHORT)
            If Not (DTC Is Nothing) Then
                DESK_EXCHANGE = GetViewClass(DTC.EXCHANGES, de.EXCHANGE_ID.ToString())
                If Not IsNothing(DESK_EXCHANGE) Then
                    DESK_EXCHANGE.CLEARERS.Add(de, de.ACCOUNT_ID.ToString())
                End If
            End If
        Next

    End Sub

    Public Function GetContactClass(ByVal a_CONTACT_ID As Integer) As CONTACT_CLASS
        Try
            GetContactClass = CONTACTS(a_CONTACT_ID.ToString())
        Catch ex As Exception
            GetContactClass = Nothing
        End Try
    End Function

    Public Sub GetContacts(ByRef gdb As DB_ARTB_NETDataContext)
        Dim CONTACT As CONTACT_CLASS
        Dim l = (From q In gdb.CONTACTs _
                  Select q).ToList
        CONTACTS.Clear()
        For Each q In l
            CONTACT = New CONTACT_CLASS
            CONTACT.GetFromObject(q)
            CONTACTS.Add(CONTACT, CONTACT.CONTACT_ID.ToString)
        Next
    End Sub

    Public Sub GetExchanges(ByRef gdb As DB_ARTB_NETDataContext)
        Dim EXCHANGE As EXCHANGE_CLASS
        Dim l = (From q In gdb.EXCHANGEs _
                  Order By q.EXCHANGE_ID _
                  Select q).ToList()

        EXCHANGES.Clear()
        For Each q In l
            EXCHANGE = New EXCHANGE_CLASS
            EXCHANGE.GetFromObject(q)
            EXCHANGES.Add(EXCHANGE, EXCHANGE.EXCHANGE_ID.ToString())
        Next

    End Sub

    Public Sub GetTradesFFAs(ByRef gdb As DB_ARTB_NETDataContext, ByRef UserInfo As TraderInfoClass)
        Dim d As Integer = DateTime.UtcNow.Day()
        Dim m As Integer = DateTime.UtcNow.Month()
        Dim y As Integer = DateTime.UtcNow.Year()
        Dim DeskID As Integer = UserInfo.DeskID
        Dim r As TRADES_FFA_CLASS
        Dim l = (From q In gdb.TRADES_FFAs _
                  Where (q.ORDER_DATETIME.Day = d _
                    And q.ORDER_DATETIME.Month = m _
                    And q.ORDER_DATETIME.Year = y) _
                    Or (q.INFORM_DESK_ID1 = DeskID) _
                    Or (q.INFORM_DESK_ID2 = DeskID) _
                  Order By q.TRADE_ID _
                  Select q).ToList

        TRADES_FFAS.Clear()
        Dim TradeList As New List(Of Integer)
        Dim InfoList As New List(Of Integer)

        For Each q In l
            r = New TRADES_FFA_CLASS
            r.GetFromObject(q)
            If InsertOrReplaceTrade(r, InfoList) = True Then
                If TradeList.Contains(r.TRADE_ID) = False Then TradeList.Add(r.TRADE_ID)
            End If
        Next
        For Each TradeId As Integer In InfoList
            If TradeId > 0 Then RaiseEvent TradeInfo(TradeId, 1)
            If TradeId < 0 Then RaiseEvent TradeInfo(-TradeId, 2)
        Next
        RaiseEvent TradesUpdated(TradeList, 0)
    End Sub

    Public Sub GetOrdersFFAs(ByRef gdb As DB_ARTB_NETDataContext, ByRef UserInfo As TraderInfoClass, ByRef AffectedRoutePeriods As List(Of String))
        Dim r As ORDERS_FFA_CLASS
        Dim d As Integer = DateTime.UtcNow.Day()
        Dim m As Integer = DateTime.UtcNow.Month()
        Dim y As Integer = DateTime.UtcNow.Year()

        Dim l = (From q In gdb.ORDERS_FFAs _
                  Where ((q.ORDER_DATETIME.Day = d _
                    And q.ORDER_DATETIME.Month = m _
                    And q.ORDER_DATETIME.Year = y) Or q.ORDER_GOOD_TILL = OrderGoodTill.GTC) _
                    Or (q.ORDER_GOOD_TILL = OrderGoodTill.GTC And _
                        q.LIVE_STATUS <> "D" And q.LIVE_STATUS <> "E") _
                    Order By q.ORDER_ID _
                Select q).ToList

        ORDERS_FFAS.Clear()
        Dim RouteList As New List(Of Integer)
        Dim FirmUpList As New List(Of Integer)
        Dim NegotiationList As New List(Of Integer)
        Dim DirectHitFailedList As New List(Of Integer)
        For Each q In l
            r = New ORDERS_FFA_CLASS
            r.GetFromObject(q)
            InsertOrReplaceOrder(r, RouteList, FirmUpList, NegotiationList, DirectHitFailedList)
        Next

        If RouteList.Count > 0 Then
            RaiseEvent OrdersUpdated(False)
            Dim OrderIdList As New List(Of Integer)
            For Each q In l
                If OrderIdList.Contains(q.ORDER_ID) = False Then
                    OrderIdList.Add(q.ORDER_ID)
                    If q.ORDER_QUALIFIER <> "R" And q.ORDER_QUALIFIER <> "N" Then
                        Dim RPStr As String = RoutePeriodStringFromObj(q)
                        If AffectedRoutePeriods.Contains(RPStr) = False Then AffectedRoutePeriods.Add(RPStr)
                        If q.ORDER_TYPE <> OrderTypes.FFA Then
                            RPStr = RoutePeriodStringFromObj(q, 2)
                            If AffectedRoutePeriods.Contains(RPStr) = False Then AffectedRoutePeriods.Add(RPStr)
                        End If
                    End If
                End If
            Next

            RaiseEvent OrderFFAUpdated(RouteList, OrderIdList)
            OrderIdList.Clear()
            OrderIdList = Nothing
            RouteList.Clear()
            RouteList = Nothing
        End If

        HandleOrders(FirmUpList, NegotiationList, DirectHitFailedList)


    End Sub


    Public Sub GetOrdersFFAExchanges(ByRef gdb As DB_ARTB_NETDataContext)
        Dim r As ORDERS_FFA_EXCHANGE_CLASS
        Dim l = From q In gdb.ORDERS_FFA_EXCHANGEs Join s In gdb.ORDERS_FFAs On q.ORDER_ID Equals s.ORDER_ID _
                Order By q.ORDER_ID, q.EXCHANGE_ID _
                  Select q

        For Each q In l
            r = New ORDERS_FFA_EXCHANGE_CLASS
            r.GetFromObject(q)
            Dim o As ORDERS_FFA_CLASS = GetViewClass(ORDERS_FFAS, r.ORDER_ID.ToString())
            If Not (o Is Nothing) Then
                o.EXCHANGES.Add(r, r.EXCHANGE_ID.ToString())
            End If
        Next
    End Sub

    Public Function ParseString(ByRef SourceStr As String, ByVal ArtBMessage As Integer) As Collection
        Dim ReturnClass As Object = Nothing
        Dim I As Integer = 0

        ParseString = New Collection

        While SourceStr.Length > 0
            I = I + 1
            Select Case ArtBMessage
                Case ArtBMessages.ChangeCounterPartyLimits
                    ReturnClass = New COUNTERPARTY_LIMIT_CLASS
                Case ArtBMessages.ChangeTraderAuthorities
                    ReturnClass = New DESK_TRADER_CLASS
                Case ArtBMessages.OrderFFANew, _
                     ArtBMessages.OrderFFAAmmend, _
                     ArtBMessages.OrderFFAFirmUp, _
                     ArtBMessages.OrderFFAInfo, _
                     ArtBMessages.OrderFFAChangeOwner, _
                     ArtBMessages.FicticiousTrade
                    ReturnClass = New ORDERS_FFA_CLASS
                Case ArtBMessages.OrderFFATrade, _
                     ArtBMessages.ChangeFFATrade, _
                     ArtBMessages.TradeFFAInfo
                    ReturnClass = New TRADES_FFA_CLASS
            End Select
            Try
                If ReturnClass.GetFromStr(SourceStr) = ArtBErrors.Success Then
                    ParseString.Add(ReturnClass, I)
                End If
            Catch e As Exception

            End Try
        End While
    End Function

    Public Function GetServerDateTimeDiff() As TimeSpan
        Dim cmd As SqlCommand = Nothing
        Dim tmpConnection As SqlConnection = Nothing
        Dim ServerDateTime As DateTime = DateTime.UtcNow
#If DEBUG Then
        Dim t0 As DateTime = DateTime.UtcNow
#End If
        Try
            tmpConnection = New SqlConnection(ArtBConnectionStr)
            tmpConnection.Open()
            cmd = New SqlCommand("Select GetUTCDate()", tmpConnection)
            ServerDateTime = cmd.ExecuteScalar
        Catch ex As Exception
            'MsgBox(ex.Message)
        Finally
            If tmpConnection.State = ConnectionState.Open Then
                cmd = Nothing
                tmpConnection.Close()
            End If
        End Try
        GetServerDateTimeDiff = DateTime.UtcNow - ServerDateTime
        GlobalDateTimeDiff = ServerDateTime - DateTime.UtcNow
#If DEBUG Then
        Dim t As TimeSpan = DateTime.UtcNow - t0
        Debug.Print("GetServerDateTimeDiff Exec Time:" & t.TotalMilliseconds.ToString())
#End If
    End Function

    Public Function GetServerDateTime() As Nullable(Of DateTime)
        Dim cmd As SqlCommand = Nothing
        Dim tmpConnection As SqlConnection = Nothing
        GetServerDateTime = Nothing
        Try
            tmpConnection = New SqlConnection(ArtBConnectionStr)
            tmpConnection.Open()
            cmd = New SqlCommand("Select GetUTCDate()", tmpConnection)
            GetServerDateTime = cmd.ExecuteScalar
        Catch ex As Exception
            Debug.Print(ex.ToString())
        Finally
            If tmpConnection.State = ConnectionState.Open Then
                cmd = Nothing
                tmpConnection.Close()
            End If
        End Try
    End Function

    Public Function ChangeTraderPasword(ByVal User_Trader_ID As Integer, _
                                        ByVal a_Password As String, _
                                        Optional ByRef gdb As DB_ARTB_NETDataContext = Nothing) As Integer

        Dim bHasCreatedConnection As Boolean = False
        If IsNothing(gdb) Then
            gdb = New DB_ARTB_NETDataContext(ArtBConnectionStr)
            bHasCreatedConnection = True
        End If
        ChangeTraderPasword = ArtBErrors.UpdateFailed
        Try
            Dim l = From q In gdb.DESK_TRADERs _
                    Where q.DESK_TRADER_ID = User_Trader_ID _
                    Select q

            For Each q In l
                Dim t As New DESK_TRADER_CLASS
                t.GetFromObject(q)
                t.PASSWORD = a_Password
                t.CHANGE_PSW = False
                ChangeTraderPasword = t.Update(gdb, True)
                t = Nothing
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
            ChangeTraderPasword = ArtBErrors.UpdateFailed
        End Try

        If bHasCreatedConnection Then gdb = Nothing
    End Function

    Public Function ValidateUser(ByVal a_LoginDesk As Integer, _
                                 ByVal a_Username As String, _
                                 ByVal a_Password As String, _
                                 ByRef User As Integer, _
                                 ByRef User_OF_ID As String, _
                                 ByRef User_OF_Password As String, _
                                 ByRef User_Account_ID As Integer, _
                                 ByRef User_Desk_ID As Integer, _
                                 ByRef User_Trader_ID As Integer, _
                                 ByRef User_Main_Desk_ID As Integer, _
                                 ByRef IsDeskAdmin As Boolean, _
                                 ByRef IsBroker As Boolean, _
                                 ByRef ServerDateTimeDiff As TimeSpan, _
                                 ByRef ChangePSW As Boolean, _
                                 ByRef AllwaysAgree As Boolean, _
                                 Optional ByRef gdb As DB_ARTB_NETDataContext = Nothing) As Integer
        Dim LoginSuccesfull As Boolean = False

        Dim bHasCreatedConnection As Boolean = False
        If IsNothing(gdb) Then
            gdb = New DB_ARTB_NETDataContext(ArtBConnectionStr)
            bHasCreatedConnection = True
        End If
        ServerDateTimeDiff = GetServerDateTimeDiff()

        Dim l = From q In gdb.DESK_TRADERs _
                Join a In gdb.ACCOUNTs On a.ACCOUNT_ID Equals q.ACCOUNT_ID _
                Where q.USERNAME = a_Username And q.ACCOUNT_DESK_ID = a_LoginDesk _
                Select q, a

        If l.Count = 0 Then
            ValidateUser = ArtBErrors.InvalidLoginName
            gdb = Nothing
            Exit Function
        End If

        AllwaysAgree = False
        ChangePSW = False
        For Each t In l
            If t.q.PASSWORD = a_Password Then LoginSuccesfull = True
            User_OF_ID = t.q.OF_ID
            User_Trader_ID = t.q.DESK_TRADER_ID
            TraderID = t.q.DESK_TRADER_ID
            User_Desk_ID = t.q.ACCOUNT_DESK_ID
            IsDeskAdmin = t.q.IS_DESK_ADMIN
            DeskID = t.q.ACCOUNT_DESK_ID
            User_OF_Password = t.q.OF_PASSWORD
            If t.q.CHANGE_PSW <> 0 Then ChangePSW = True
            If t.q.AGREES_TO_STATEMENT <> 0 Then AllwaysAgree = True
        Next

        User = Str2Int(User_OF_ID)


        Dim al = From qa In gdb.ACCOUNT_DESKs _
                 Where qa.ACCOUNT_DESK_ID = DeskID _
                 Select qa

        For Each qa In al
            User_Account_ID = qa.ACCOUNT_ID
            AccountID = qa.ACCOUNT_ID
        Next

        Dim acl = From qal In gdb.ACCOUNTs _
                 Where qal.ACCOUNT_ID = AccountID _
                 Select qal
        IsBroker = False
        For Each qal In acl
            IsBroker = True
            If qal.ACCOUNT_TYPE_ID = 1 Then
                IsBroker = False
            End If
        Next

        Dim ml = From qm In gdb.ACCOUNT_DESKs _
                 Where qm.ACCOUNT_ID = AccountID And qm.DESK_QUALIFIER = 0 _
                 Select qm

        For Each qm In ml
            User_Main_Desk_ID = qm.ACCOUNT_DESK_ID
            MainDeskID = User_Main_Desk_ID
        Next

        ValidateUser = ArtBErrors.Success

        If LoginSuccesfull = False Then
            ValidateUser = ArtBErrors.InvalidLoginPassword
            Exit Function
        End If

        If bHasCreatedConnection Then gdb = Nothing

    End Function

    Public Function LockUnlockOrder(ByRef counterPartyOrder As ORDERS_FFA_CLASS, _
                                    ByRef ByOrder As ORDERS_FFA_CLASS) As Integer
        LockUnlockOrder = ArtBErrors.CounterOrderDoesNotExist
        If IsNothing(counterPartyOrder) Then Exit Function

        Select Case ByOrder.LIVE_STATUS
            Case "A", "R"
                Select Case counterPartyOrder.LIVE_STATUS
                    Case "A"
                        LockUnlockOrder = ArtBErrors.Success
                        counterPartyOrder.LIVE_STATUS = "N"
                        counterPartyOrder.LOCKED_BY_ORDER_ID = ByOrder.ORDER_ID
                        counterPartyOrder.LOCK_DESK_TRADER_ID = ByOrder.FOR_DESK_TRADER_ID
                    Case "D"
                        LockUnlockOrder = ArtBErrors.CounterOrderIsDead
                    Case "S"
                        LockUnlockOrder = ArtBErrors.CounterOrderIsSleeping
                    Case "N"
                        If counterPartyOrder.LOCK_ORDER_ID <> ByOrder.ORDER_ID Then
                            LockUnlockOrder = ArtBErrors.CounterOrderIsNegotiated
                        Else
                            LockUnlockOrder = ArtBErrors.Success
                        End If
                    Case "R"
                        LockUnlockOrder = ArtBErrors.CounterOrderIsNegotiated
                    Case "E"
                        LockUnlockOrder = ArtBErrors.CounterOrderIsExecuted
                End Select
            Case "N"
                LockUnlockOrder = ArtBErrors.Success
            Case Else
                counterPartyOrder.LIVE_STATUS = "A"
                counterPartyOrder.LOCKED_BY_ORDER_ID = Nothing
                counterPartyOrder.LOCK_DESK_TRADER_ID = Nothing

                LockUnlockOrder = ArtBErrors.Success
                'Will there be multiple order lockings?

        End Select
    End Function

    Public Function InsertNewOrder(ByRef cpl As Object, _
                                   ByRef gdb As DB_ARTB_NETDataContext) As Integer

        InsertNewOrder = cpl.Insert(True, gdb)
        If InsertNewOrder <> ArtBErrors.Success Then Exit Function
        Dim s As String = cpl.ORDER_EXCHANGES
        Dim c As String, j As Integer, exchangeId As Integer = 1, clearerId As Integer = 0
        While Len(s) > 1
            j = s.IndexOf("_")
            If j < 0 Or j >= s.Length() Then
                c = s
                s = ""
            Else
                c = s.Substring(0, j)
                s = s.Substring(j + 1)
            End If
            clearerId = Str2Int(c)
            If Len(c) < 1 Then clearerId = -1
            If clearerId >= 0 Then
                Dim ofe As New ORDERS_FFA_EXCHANGE_CLASS
                ofe.EXCHANGE_ID = exchangeId
                ofe.ORDER_ID = cpl.ORDER_ID
                ofe.ACCOUNT_ID = clearerId
                InsertNewOrder = ofe.Insert(gdb, True)
                If InsertNewOrder <> ArtBErrors.Success Then Exit Function
            End If
            exchangeId = exchangeId + 1
        End While
    End Function

    Public Function AmmendOrder(ByRef gdb As DB_ARTB_NETDataContext, _
                                ByRef cpl As Object) As Integer
        AmmendOrder = cpl.DeleteExchanges(True)
        If AmmendOrder <> ArtBErrors.Success Then Exit Function
        AmmendOrder = cpl.Update(True)
        If AmmendOrder <> ArtBErrors.Success Then Exit Function
        Dim s As String = cpl.ORDER_EXCHANGES
        Dim c As String, j As Integer, exchangeId As Integer = 1, clearerId As Integer = 0
        While Len(s) > 1
            j = s.IndexOf("_")
            If j < 0 Or j >= s.Length() Then
                c = s
                s = ""
            Else
                c = s.Substring(0, j)
                s = s.Substring(j + 1)
            End If
            clearerId = Str2Int(c)
            If Len(c) < 1 Then clearerId = -1
            If clearerId >= 0 Then
                Dim ofe As New ORDERS_FFA_EXCHANGE_CLASS
                ofe.EXCHANGE_ID = exchangeId
                ofe.ORDER_ID = cpl.ORDER_ID
                ofe.ACCOUNT_ID = clearerId
                AmmendOrder = ofe.Insert(gdb, True)
                If AmmendOrder <> ArtBErrors.Success Then Exit Function
            End If
            exchangeId = exchangeId + 1
        End While
    End Function

    Public Function InsertUpdateOrder(ByRef gdb As DB_ARTB_NETDataContext, _
                                      ByRef cpl As ORDERS_FFA_CLASS, _
                                      Optional ByVal bFixTime As Boolean = False) As Integer
        InsertUpdateOrder = cpl.DeleteExchanges(gdb, True)
        If InsertUpdateOrder <> ArtBErrors.Success Then Exit Function
        If bFixTime Then cpl.ORDER_DATETIME = DateTime.UtcNow.Add(GlobalDateTimeDiff)
        InsertUpdateOrder = cpl.InsertUpdate(gdb, True)
        If InsertUpdateOrder <> ArtBErrors.Success Then Exit Function
        Dim s As String = cpl.ORDER_EXCHANGES
        Dim c As String, j As Integer, exchangeId As Integer = 1, clearerId As Integer = 0
        While Len(s) > 1
            j = s.IndexOf("_")
            If j < 0 Or j >= s.Length() Then
                c = s
                s = ""
            Else
                c = s.Substring(0, j)
                s = s.Substring(j + 1)
            End If
            clearerId = Str2Int(c)
            If Len(c) < 1 Then clearerId = -1
            If clearerId >= 0 Then
                Dim ofe As New ORDERS_FFA_EXCHANGE_CLASS
                ofe.EXCHANGE_ID = exchangeId
                ofe.ORDER_ID = cpl.ORDER_ID
                ofe.ACCOUNT_ID = clearerId
                InsertUpdateOrder = ofe.Insert(gdb, True)
                If InsertUpdateOrder <> ArtBErrors.Success Then Exit Function
            End If
            exchangeId = exchangeId + 1
        End While

    End Function

    Public Sub GetOrderExchangesFromStr(ByRef o As Object, ByRef ox() As Boolean)
        Dim s As String = o.ORDER_EXCHANGES
        If s = GlobalExchangeStr Then
            For i As Integer = 1 To TotalExchanges
                ox(i) = True
            Next
            Exit Sub
        End If
        Dim c As String, j As Integer, exchangeId As Integer = 1, clearerId As Integer = 0
        While Len(s) > 1
            ox(exchangeId) = False
            j = s.IndexOf("_")
            If j < 0 Or j >= s.Length() Then
                c = s
                s = ""
            Else
                c = s.Substring(0, j)
                s = s.Substring(j + 1)
            End If
            clearerId = Str2Int(c)
            If Len(c) < 1 Then clearerId = -1
            If clearerId >= 0 Then
                ox(exchangeId) = True
            End If
            exchangeId = exchangeId + 1
        End While
    End Sub

    Public Sub LimitExchangeStr(ByRef o As Object, ByVal a_ExchangeID As Integer)
        Dim s As String = o.ORDER_EXCHANGES
        Dim c As String, j As Integer, exchangeId As Integer = 1, clearerId As Integer = 0
        Dim ns As String = ""
        While Len(s) > 1
            j = s.IndexOf("_")
            If j < 0 Or j >= s.Length() Then
                c = s
                s = ""
            Else
                c = s.Substring(0, j)
                s = s.Substring(j + 1)
            End If
            clearerId = Str2Int(c)
            If Len(c) < 1 Then clearerId = -1
            If clearerId >= 0 And exchangeId = a_ExchangeID Then
                ns = ns & clearerId.ToString()
            End If
            ns = ns & "_"
            exchangeId = exchangeId + 1
        End While
        o.ORDER_EXCHANGES = ns
    End Sub

    Public Function UpdateDBFromNewOrders(ByRef gdb As DB_ARTB_NETDataContext, _
                                          ByRef SourceStr As String, _
                                          ByVal ArtBMessage As Integer, _
                                          ByRef OrderStr As String, _
                                          ByRef TradeStr As String) As Integer
        'First guarantee single order execution
        If bUpdateDBFromNewOrder Then Exit Function
        bUpdateDBFromNewOrder = True

        'Empty String
        If (SourceStr.Length() < 1) Then
            bUpdateDBFromNewOrder = False
            Return ArtBErrors.Success
        End If
        Dim ss As String = SourceStr
        Dim xs As String = ""
        Dim cl As Collection = ParseString(ss, ArtBMessage)
        Dim AffectedRoutePeriods As New List(Of String)


        If IsNothing(cl) Then
            bUpdateDBFromNewOrder = False
            Return ArtBErrors.Success
        End If
        If cl.Count < 1 Then
            bUpdateDBFromNewOrder = False
            Return ArtBErrors.Success
        End If

        Try
            If gdb.Connection.State = ConnectionState.Closed Or gdb.Connection.State = ConnectionState.Broken Then
                gdb.Connection.Open()
            End If
            If gdb.Connection.State = ConnectionState.Closed Or gdb.Connection.State = ConnectionState.Broken Then
                bUpdateDBFromNewOrder = False
                Return ArtBErrors.ConnectionDead
            End If

            gdb.Transaction = gdb.Connection.BeginTransaction(IsolationLevel.ReadUncommitted)
        Catch
            bUpdateDBFromNewOrder = False
            Return ArtBErrors.ConnectionDead
        End Try
        UpdateDBFromNewOrders = ArtBErrors.Success
        For Each cpl As ORDERS_FFA_CLASS In cl
            Try
                UpdateDBFromNewOrders = UpdateDBFromNewOrder(gdb, cpl, OrderStr, TradeStr, AffectedRoutePeriods) ' Extend with routes/periods affected list 
                If UpdateDBFromNewOrders <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrdersEnd
            Catch ex As Exception
                Debug.Print(ex.ToString)
                UpdateDBFromNewOrders = ArtBErrors.UpdateOrderInDBCrash
                GoTo UpdateDBFromNewOrdersEnd
            End Try
            If UpdateDBFromNewOrders <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrdersEnd
        Next

        'Adjust Spread Prices
        Try
            UpdateDBFromNewOrders = IssueAdditionalSpreadTrades(gdb, AffectedRoutePeriods, OrderStr, TradeStr)
            If UpdateDBFromNewOrders <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrdersEnd
        Catch ex As Exception
            Debug.Print(ex.ToString)
            UpdateDBFromNewOrders = ArtBErrors.IssueAdditionalSpreadTradesCrash
            GoTo UpdateDBFromNewOrdersEnd
        End Try

        'UpdateDBFromNewOrders = AdjustSpreads2(gdb, AffectedRoutePeriods, OrderStr, TradeStr)

UpdateDBFromNewOrdersEnd:
        If UpdateDBFromNewOrders = ArtBErrors.Success Then
            Try
                gdb.SubmitChanges()
                gdb.Transaction.Commit()
            Catch ex As Exception
                Debug.Print("UpdateDBFromNewOrdersEnd:")
                Debug.Print(ex.ToString)
                RollBack(gdb)
            End Try
        Else
            RollBack(gdb)
        End If

UpdateDBFromNewOrderGC:
        Try
            gdb.Transaction.Dispose()
            For Each o In cl
                o = Nothing
            Next
            cl = Nothing
        Catch ex3 As Exception
            Debug.Print(ex3.ToString)
        End Try
        bUpdateDBFromNewOrder = False

    End Function

    Public Sub RollBack(ByRef gdb As DB_ARTB_NETDataContext)
        Dim bSuccess As Boolean = False
        Dim i As Integer = 1

        While bSuccess = False and i<20
            Try
                gdb.Transaction.Rollback()
                bSuccess = True
                Exit Sub
            Catch ex2 As Exception
                Debug.Print("gdb.Transaction.Rollback Attemp #" & i.ToString)
                Debug.Print(ex2.ToString)
            End Try
            Debug.Print("Sleeping for 50 miliseconds....")
            System.Threading.Thread.Sleep(50)
            i = i + 1
        End While
    End Sub

    Public Function UpdateDBFromNewOrder(ByRef gdb As DB_ARTB_NETDataContext, _
                                         ByRef cpl As ORDERS_FFA_CLASS, _
                                         ByRef OrderStr As String, _
                                         ByRef TradeStr As String, _
                                         ByRef AffectedRoutePeriods As List(Of String), _
                                         Optional ByVal bCheckExec As Boolean = True) As Integer
        'Should be executed when any order is inserted or changes LIVE_STATUS

        If IsNothing(cpl) Then Return ArtBErrors.Success

        Dim CounterOrder As ORDERS_FFA_CLASS = Nothing
        Dim PreviousOrder As ORDERS_FFA_CLASS = Nothing
        Dim CommitOrder As ORDERS_FFA_CLASS = Nothing
        Dim LockedByOrder As ORDERS_FFA_CLASS = Nothing
        Dim LockOrder As ORDERS_FFA_CLASS = Nothing
        Dim NegotiationOrder As ORDERS_FFA_CLASS = Nothing

        Dim Ob As ORDERS_FFA_CLASS = Nothing
        Dim CounterOrderB As ORDERS_FFA_CLASS = Nothing
        Dim PreviousOrderB As ORDERS_FFA_CLASS = Nothing
        Dim CommitOrderB As ORDERS_FFA_CLASS = Nothing
        Dim LockedByOrderB As ORDERS_FFA_CLASS = Nothing
        Dim LockOrderB As ORDERS_FFA_CLASS = Nothing
        Dim NegotiationOrderB As ORDERS_FFA_CLASS = Nothing

        Dim CounterPartyOrderId As Integer = NullInt2Int(cpl.COUNTER_PARTY_ORDER_ID)
        Dim PreviousOrderId As Integer = NullInt2Int(cpl.PREVIOUS_ORDER_ID)
        Dim CommitOrderId As Integer = NullInt2Int(cpl.COMMIT_ORDER_ID)
        Dim LockedByOrderId As Integer = NullInt2Int(cpl.LOCKED_BY_ORDER_ID)
        Dim LockOrderId As Integer = NullInt2Int(cpl.LOCK_ORDER_ID)
        Dim NegotiationOrderId As Integer = NullInt2Int(cpl.NEGOTIATION_ORDER_ID)

        If CounterPartyOrderId <> 0 Then
            CounterOrder = New ORDERS_FFA_CLASS
            UpdateDBFromNewOrder = CounterOrder.GetFromID(gdb, CounterPartyOrderId)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            CounterOrderB = New ORDERS_FFA_CLASS
            CounterOrderB.GetFromObject(CounterOrder)
        End If
        If PreviousOrderId <> 0 Then
            PreviousOrder = New ORDERS_FFA_CLASS
            UpdateDBFromNewOrder = PreviousOrder.GetFromID(gdb, PreviousOrderId)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            PreviousOrderB = New ORDERS_FFA_CLASS
            PreviousOrderB.GetFromObject(PreviousOrder)
        End If
        If CommitOrderId <> 0 Then
            CommitOrder = New ORDERS_FFA_CLASS
            UpdateDBFromNewOrder = CommitOrder.GetFromID(gdb, CommitOrderId)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            CommitOrderB = New ORDERS_FFA_CLASS
            CommitOrderB.GetFromObject(CommitOrder)
        End If
        If LockOrderId <> 0 Then
            LockOrder = New ORDERS_FFA_CLASS
            UpdateDBFromNewOrder = LockOrder.GetFromID(gdb, LockOrderId)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            LockOrderB = New ORDERS_FFA_CLASS
            LockOrderB.GetFromObject(LockOrder)
        End If
        If LockedByOrderId <> 0 Then
            LockedByOrder = New ORDERS_FFA_CLASS
            UpdateDBFromNewOrder = LockedByOrder.GetFromID(gdb, LockedByOrderId)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            LockedByOrderB = New ORDERS_FFA_CLASS
            LockedByOrderB.GetFromObject(LockedByOrder)

        End If
        If NegotiationOrderId <> 0 Then
            NegotiationOrder = New ORDERS_FFA_CLASS
            UpdateDBFromNewOrder = NegotiationOrder.GetFromID(gdb, NegotiationOrderId)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            NegotiationOrderB = New ORDERS_FFA_CLASS
            NegotiationOrderB.GetFromObject(NegotiationOrder)
        End If

        Dim CrossTraderId As Integer = 0
        Dim PrevLiveStatus As String = ""
        'Get ID for new orders

        If cpl.PRICE_TYPE = "I" Then
            'If ChechForRejectedIndicatives(gdb, cpl) Then Return ArtBErrors.IndicativeOrderProhibited
        End If

        If cpl.ORDER_QUALIFIER = "A" And cpl.LIVE_STATUS = "A" Then
            If IsNothing(PreviousOrder) Then Return ArtBErrors.TryToAmmendAllreadyModifiedOrder
            If PreviousOrder.ORDER_QUALIFIER <> "A" Or PreviousOrder.LIVE_STATUS <> "D" Then Return ArtBErrors.TryToAmmendAllreadyModifiedOrder
            If NullInt2Int(PreviousOrder.COUNTER_PARTY_ORDER_ID) <> 0 Then Return ArtBErrors.TryToAmmendAllreadyModifiedOrder
        End If

        If cpl.ORDER_ID = 0 Then
            If OrderThreadExists(gdb, cpl) Then Return ArtBErrors.Success

            UpdateDBFromNewOrder = InsertUpdateOrder(gdb, cpl, True)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
        Else
            Ob = New ORDERS_FFA_CLASS
            UpdateDBFromNewOrder = Ob.GetFromID(gdb, cpl.ORDER_ID)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then
                Ob = Nothing
            Else
                PrevLiveStatus = Ob.LIVE_STATUS
            End If
        End If

        If cpl.PRICE_TYPE = "I" And cpl.LIVE_STATUS = "A" And PrevLiveStatus = "" And Not IsNothing(PreviousOrder) Then
            cpl.ORDER_DATETIME = PreviousOrder.ORDER_DATETIME
        End If

        If cpl.ORDER_QUALIFIER = "A" And cpl.LIVE_STATUS = "A" Then
            If cpl.PRICE_INDICATED = PreviousOrder.PRICE_INDICATED And cpl.ORDER_GOOD_TILL <> OrderGoodTill.Limit Then cpl.ORDER_DATETIME = PreviousOrder.ORDER_DATETIME
            PreviousOrder.COUNTER_PARTY_ORDER_ID = cpl.ORDER_ID
            UpdateDBFromNewOrder = InsertUpdateOrder(gdb, PreviousOrder, True)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            PreviousOrderB.GetFromObject(PreviousOrder)
            cpl.ORDER_QUALIFIER = " "
        End If

        If cpl.ORDER_QUALIFIER = "A" And cpl.LIVE_STATUS = "D" And PrevLiveStatus = "D" Then
            If NullInt2Int(cpl.COUNTER_PARTY_ORDER_ID) <> 0 Then Return ArtBErrors.TryToAmmendAllreadyModifiedOrder
        End If

        'Validity checks

        'Check for Non Consistency
        Select Case PrevLiveStatus
            Case "D"
                If cpl.ORDER_QUALIFIER <> "D" Then 'To handle Direct Hit Ack Info
                    If cpl.ORDER_QUALIFIER <> "R" Then Return ArtBErrors.Success
                End If
            Case "E"
                Return ArtBErrors.Success
                ''D' , 'E' are final status so further received orders for this ORDER_ID will be ignored
        End Select

        If cpl.ORDER_QUALIFIER = "R" Then
            If IsNothing(CounterOrder) Then
                UpdateDBFromNewOrder = ArtBErrors.CounterOrderDoesNotExist
                GoTo UpdateDBFromNewOrderEnd
            End If
        ElseIf cpl.ORDER_QUALIFIER = "N" Then
            If IsNothing(CounterOrder) Then
                UpdateDBFromNewOrder = ArtBErrors.CounterOrderDoesNotExist
                GoTo UpdateDBFromNewOrderEnd
            End If
            If NullInt2Int(cpl.SPREAD_ORDER_ID) = 0 Then
                If IsNothing(NegotiationOrder) Then
                    UpdateDBFromNewOrder = ArtBErrors.CounterOrderDoesNotExist
                    GoTo UpdateDBFromNewOrderEnd
                End If

                If cpl.LIVE_STATUS = "A" Then
                    If Not (NegotiationOrder.LIVE_STATUS = "A" Or NegotiationOrder.LIVE_STATUS = "N") Then
                        UpdateDBFromNewOrder = ArtBErrors.NegotiationOnNonActiveOrder
                        GoTo UpdateDBFromNewOrderEnd
                    End If
                End If
            End If
        End If

        'Lock/Unlock Orders
        If (Not IsNothing(LockOrder)) And PrevLiveStatus <> cpl.LIVE_STATUS Then
            UpdateDBFromNewOrder = LockUnlockOrder(LockOrder, cpl)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
        End If

        'Spread leg orders handling
        Dim LegOrderId1 As Integer = NullInt2Int(cpl.CROSS_ORDER_ID1)
        Dim LegOrder1 As ORDERS_FFA_CLASS = Nothing
        Dim LegOrderId2 As Integer = NullInt2Int(cpl.CROSS_ORDER_ID2)
        Dim LegOrder2 As ORDERS_FFA_CLASS = Nothing
        Dim Leg1Price As Double = -1.0E+20
        Dim Leg2Price As Double = -1.0E+20

        If cpl.LIVE_STATUS <> "R" And _
           (cpl.ORDER_TYPE <> OrderTypes.FFA) And _
           cpl.PRICE_TYPE = "F" Then

            'Spread legs
            If LegOrderId1 <> 0 Then
                LegOrder1 = New ORDERS_FFA_CLASS
                UpdateDBFromNewOrder = LegOrder1.GetFromID(gdb, LegOrderId1)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
                LegOrder1.LIVE_STATUS = cpl.LIVE_STATUS
                LegOrder1.SetQuantity(cpl.ORDER_QUANTITY)

                If cpl.LIVE_STATUS = "E" Then
                    Leg1Price = LegOrder1.PRICE_INDICATED
                End If
            Else
                LegOrder1 = cpl.GetSpreadLeg(1)
            End If
            If Not IsNothing(LegOrder1) Then
                LegOrder1.ORDER_ID = LegOrderId1
                UpdateDBFromNewOrder = UpdateDBFromNewOrder(gdb, _
                                                            LegOrder1, _
                                                            OrderStr, TradeStr, AffectedRoutePeriods)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
                LegOrderId1 = LegOrder1.ORDER_ID
            End If
            cpl.CROSS_ORDER_ID1 = LegOrder1.ORDER_ID
            If LegOrderId2 <> 0 Then
                LegOrder2 = New ORDERS_FFA_CLASS
                UpdateDBFromNewOrder = LegOrder2.GetFromID(gdb, LegOrderId2)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
                LegOrder2.LIVE_STATUS = cpl.LIVE_STATUS
                LegOrder2.SetQuantity(cpl.ORDER_QUANTITY2)
                If cpl.LIVE_STATUS = "E" Then
                    Leg2Price = LegOrder2.PRICE_INDICATED
                End If
            Else
                LegOrder2 = cpl.GetSpreadLeg(2)
            End If
            If Not IsNothing(LegOrder2) Then
                LegOrder2.ORDER_ID = LegOrderId2
                UpdateDBFromNewOrder = UpdateDBFromNewOrder(gdb, _
                                                            LegOrder2, _
                                                            OrderStr, TradeStr, AffectedRoutePeriods)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
                LegOrderId2 = LegOrder2.ORDER_ID
            End If
            cpl.CROSS_ORDER_ID2 = LegOrder2.ORDER_ID

            If NullInt2Int(LegOrder1.COMMIT_ORDER_ID) <> LegOrder2.ORDER_ID Then
                LegOrder1.COMMIT_ORDER_ID = LegOrder2.ORDER_ID
                UpdateDBFromNewOrder = UpdateDBFromNewOrder(gdb, LegOrder1, OrderStr, TradeStr, AffectedRoutePeriods)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            End If
            If NullInt2Int(LegOrder2.COMMIT_ORDER_ID) <> LegOrder1.ORDER_ID Then
                LegOrder2.COMMIT_ORDER_ID = LegOrder1.ORDER_ID
                UpdateDBFromNewOrder = UpdateDBFromNewOrder(gdb, LegOrder2, OrderStr, TradeStr, AffectedRoutePeriods)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            End If
        End If

        ' Fill Inform Desk 
        'Debug.Print(cpl.ORDER_ID.ToString & ": " & cpl.LIVE_STATUS & "-" & PrevLiveStatus & " : " & cpl.INFORM_DESK_ID.ToString)
        Select Case cpl.LIVE_STATUS
            Case "R"
                'Request has been received
                CounterOrder.COUNTER_PARTY_ORDER_ID = cpl.ORDER_ID
            Case "D"
                ' Handle Request Rejected
                ' Indicative order will be removed
                If PrevLiveStatus = "R" Then
                    cpl.INFORM_DESK_ID = GetTraderDeskIdFromDB(gdb, cpl.DESK_TRADER_ID)
                    CounterOrder.LIVE_STATUS = "D"
                End If
            Case "A"
                If PrevLiveStatus = "" And (cpl.ORDER_QUALIFIER = "N") Then
                    'Fill inform desk as first insertion of order
                    cpl.INFORM_DESK_ID = GetTraderDeskIdFromDB(gdb, CounterOrder.DESK_TRADER_ID)
                End If
            Case "E"
                cpl.INFORM_DESK_ID = Nothing
        End Select
        ' Debug.Print(cpl.ORDER_ID.ToString & ": " & cpl.LIVE_STATUS & "-" & PrevLiveStatus & " : " & cpl.INFORM_DESK_ID.ToString)

        'Reset timer for sleep awake
        If PrevLiveStatus = "S" And cpl.LIVE_STATUS = "A" Then
            cpl.ORDER_DATETIME = DateTime.UtcNow.Add(GlobalDateTimeDiff)
        End If
        If cpl.LIVE_STATUS = "S" And PrevLiveStatus = "A" And cpl.ORDER_GOOD_TILL = OrderGoodTill.Limit Then
            cpl.LIVE_STATUS = "D"
        End If
        'First Level Updates
        UpdateDBFromNewOrder = InsertUpdateOrder(gdb, cpl)
        If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
        UpdateDBFromNewOrder = cpl.AppendToStr(OrderStr)
        If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd

        If Not IsNothing(CounterOrder) Then
            If Not CounterOrderB.Equal(CounterOrder) Then
                UpdateDBFromNewOrder = UpdateDBFromNewOrder(gdb, CounterOrder, OrderStr, TradeStr, AffectedRoutePeriods)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            End If
        End If

        If Not IsNothing(PreviousOrder) Then
            If Not PreviousOrderB.Equal(PreviousOrder) Then
                UpdateDBFromNewOrder = UpdateDBFromNewOrder(gdb, PreviousOrder, OrderStr, TradeStr, AffectedRoutePeriods)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            End If
        End If

        If Not IsNothing(NegotiationOrder) Then
            If Not NegotiationOrderB.Equal(NegotiationOrder) Then
                UpdateDBFromNewOrder = UpdateDBFromNewOrder(gdb, NegotiationOrder, OrderStr, TradeStr, AffectedRoutePeriods)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            End If
        End If

        If Not IsNothing(CommitOrder) Then
            If Not CommitOrderB.Equal(CommitOrder) Then
                UpdateDBFromNewOrder = UpdateDBFromNewOrder(gdb, CommitOrder, OrderStr, TradeStr, AffectedRoutePeriods)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            End If
        End If

        If Not IsNothing(LockOrder) Then
            If Not LockOrderB.Equal(LockOrder) Then
                UpdateDBFromNewOrder = UpdateDBFromNewOrder(gdb, LockOrder, OrderStr, TradeStr, AffectedRoutePeriods)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            End If
        End If

        If Not IsNothing(LockedByOrder) Then
            If Not LockedByOrderB.Equal(LockedByOrder) Then
                UpdateDBFromNewOrder = UpdateDBFromNewOrder(gdb, LockedByOrder, OrderStr, TradeStr, AffectedRoutePeriods)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            End If
        End If

        If cpl.LIVE_STATUS = "D" Or cpl.LIVE_STATUS = "E" Or cpl.LIVE_STATUS = "S" Then
            'Kill Negotiations on executed deleted order
            UpdateDBFromNewOrder = DeletePreviousNegotiations(gdb, _
                                                              cpl.ORDER_ID, _
                                                              cpl.ORDER_ID, _
                                                              0, _
                                                              OrderStr, _
                                                              TradeStr, _
                                                              AffectedRoutePeriods)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd

        End If

        'Order Execution
        'It will not run for individual spreaqda legs

        Dim bIsShadow As Boolean = False

        If cpl.LIVE_STATUS = "A" And NullInt2Int(cpl.SPREAD_LEG_TYPE) = 0 And bCheckExec Then
            Dim currentOrder As ORDERS_FFA_CLASS = cpl
            Dim NEWORDER As ORDERS_FFA_CLASS = Nothing
            Dim OtherNewOrder As ORDERS_FFA_CLASS = Nothing

            Dim MarketMatchingOrdres As Collection = MarketMatching(gdb, currentOrder)
            Dim BuyOrderId As Integer, SellOrderId As Integer
            If Not IsNothing(MarketMatchingOrdres) Then
                For Each oc As MARKET_MATCHING_CLASS In MarketMatchingOrdres
                    UpdateDBFromNewOrder = TradeOrders(gdb, oc.BuyOrder, oc.SellOrder, _
                                                           NEWORDER, OtherNewOrder, _
                                                           OrderStr, TradeStr, AffectedRoutePeriods, _
                                                           oc.ActualQuantity, oc.ExchangeID, oc.Price, oc.FreePrice)
                    If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo updateDBFromNewOrderEnd
                    If Not IsNothing(NEWORDER) Then
                        BuyOrderId = oc.BuyOrder.ORDER_ID
                        For Each ocb As MARKET_MATCHING_CLASS In MarketMatchingOrdres
                            If ocb.BuyOrder.ORDER_ID = BuyOrderId Then
                                ocb.BuyOrder = Nothing
                                ocb.BuyOrder = NEWORDER
                            End If
                        Next
                    End If
                    If Not IsNothing(OtherNewOrder) Then
                        SellOrderId = oc.SellOrder.ORDER_ID
                        For Each ocs As MARKET_MATCHING_CLASS In MarketMatchingOrdres
                            If ocs.SellOrder.ORDER_ID = SellOrderId Then
                                ocs.SellOrder = Nothing
                                ocs.SellOrder = OtherNewOrder
                            End If
                        Next
                    End If
                    If oc.BuyOrder.DESK_TRADER_ID = SystemDeskTraderId Or oc.SellOrder.DESK_TRADER_ID = SystemDeskTraderId Then bIsShadow = True
                Next
            End If

        End If

        If cpl.ORDER_QUALIFIER = "N" And cpl.LIVE_STATUS = "A" And NullInt2Int(cpl.SPREAD_ORDER_ID) = 0 Then
            CrossTraderId = CounterOrder.FOR_DESK_TRADER_ID
            If CrossTraderId = NegotiationOrder.FOR_DESK_TRADER_ID Then
                CrossTraderId = cpl.FOR_DESK_TRADER_ID
            End If


            UpdateDBFromNewOrder = DeletePreviousNegotiationsForTrader(gdb, _
                                                                       cpl.ORDER_ID, _
                                                                       NegotiationOrder.ORDER_ID, _
                                                                       CrossTraderId, _
                                                                       OrderStr, _
                                                                       TradeStr, _
                                                                       AffectedRoutePeriods)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo updateDBFromNewOrderEnd
            'If cpl.FOR_DESK_TRADER_ID = NegotiationOrder.FOR_DESK_TRADER_ID Then
            '    UpdateDBFromNewOrder = SetLiveStatusForOtherNegotiations(cpl, NegotiationOrder, CrossTraderId, "N", OrderStr, TradeStr)
            '    If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            'Else
            '    UpdateDBFromNewOrder = SetLiveStatusForOtherNegotiations(CounterOrder, NegotiationOrder, CrossTraderId, "A", OrderStr, TradeStr)
            '    If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo UpdateDBFromNewOrderEnd
            'End If
        End If

        'Second Level Updates
        If cpl.ORDER_QUALIFIER = "D" And _
           cpl.LIVE_STATUS = "A" _
           And NullInt2Int(cpl.SPREAD_LEG_TYPE) = 0 _
           And bCheckExec Then

            UpdateDBFromNewOrder = cpl.GetFromID(gdb, cpl.ORDER_ID)
            If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo updateDBFromNewOrderEnd
            If cpl.LIVE_STATUS = "A" Then
                cpl.LIVE_STATUS = "D"
                cpl.INFORM_DESK_ID = GetTraderDeskIdFromDB(gdb, cpl.DESK_TRADER_ID)
                UpdateDBFromNewOrder = cpl.Update(gdb, True)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo updateDBFromNewOrderEnd
                UpdateDBFromNewOrder = cpl.AppendToStr(OrderStr)
                If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo updateDBFromNewOrderEnd


                If Not IsNothing(LegOrder1) And LegOrderId1 <> 0 Then
                    LegOrder1.LIVE_STATUS = "D"
                    UpdateDBFromNewOrder = LegOrder1.Update(gdb, True)
                    If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo updateDBFromNewOrderEnd
                    UpdateDBFromNewOrder = LegOrder1.AppendToStr(OrderStr)
                    If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo updateDBFromNewOrderEnd
                End If

                If Not IsNothing(LegOrder2) And LegOrderId2 <> 0 Then
                    LegOrder1.LIVE_STATUS = "D"
                    UpdateDBFromNewOrder = LegOrder2.Update(gdb, True)
                    If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo updateDBFromNewOrderEnd
                    UpdateDBFromNewOrder = LegOrder2.AppendToStr(OrderStr)
                    If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo updateDBFromNewOrderEnd
                End If
                If Not CounterOrder Is Nothing Then
                    If CounterOrder.ORDER_QUALIFIER = "R" And CounterOrder.LIVE_STATUS = "A" Then
                        UpdateDBFromNewOrder = SetOrderLiveStatus(gdb, CounterOrder.ORDER_ID, "D", OrderStr, TradeStr, AffectedRoutePeriods)
                    End If
                End If
            End If
        End If

        'Time limit orders
        If cpl.ORDER_GOOD_TILL = OrderGoodTill.Limit And _
            (cpl.LIVE_STATUS = "A" Or cpl.LIVE_STATUS = "R") And IsNothing(cpl.SPREAD_ORDER_ID) Then
            RaiseEvent TimeLimitOrder(cpl.ORDER_ID, cpl.ORDER_TIME_LIMIT * 60)
        End If

        If (Not (cpl.PRICE_TYPE = "I" And PrevLiveStatus = "")) _
            And IsNothing(cpl.SPREAD_ORDER_ID) _
            And cpl.LIVE_STATUS <> "R" Then
            'Indicative orders and spread legs do not participate in adjust prices
            Dim RPStr As String
            RPStr = Format(cpl.ROUTE_ID, "00000")
            Dim TP As New ArtBTimePeriod
            TP.FillMY(cpl.MM1, cpl.YY1, cpl.MM2, cpl.YY2)
            RPStr = RPStr & Format(TP.GetID, "00000000")

            If AffectedRoutePeriods.Contains(RPStr) = False Then
                AffectedRoutePeriods.Add(RPStr)
            End If

            If cpl.ORDER_TYPE <> OrderTypes.FFA Then

                RPStr = Format(cpl.ROUTE_ID2, "00000")
                TP.FillMY(cpl.MM21, cpl.YY21, cpl.MM22, cpl.YY22)
                RPStr = RPStr & Format(TP.GetID, "00000000")

                If AffectedRoutePeriods.Contains(RPStr) = False Then
                    AffectedRoutePeriods.Add(RPStr)
                End If
            End If
            TP = Nothing

        End If

        If bIsShadow Then
            Dim currentOrder As ORDERS_FFA_CLASS = cpl
            Dim NEWORDER As ORDERS_FFA_CLASS = Nothing
            Dim OtherNewOrder As ORDERS_FFA_CLASS = Nothing

            Dim MarketMatchingOrdres As Collection = MarketMatching(gdb, currentOrder)
            Dim BuyOrderId As Integer, SellOrderId As Integer
            If Not IsNothing(MarketMatchingOrdres) Then
                For Each oc As MARKET_MATCHING_CLASS In MarketMatchingOrdres
                    UpdateDBFromNewOrder = TradeOrders(gdb, oc.BuyOrder, oc.SellOrder, _
                                                           NEWORDER, OtherNewOrder, _
                                                           OrderStr, TradeStr, AffectedRoutePeriods, _
                                                           oc.ActualQuantity, oc.ExchangeID, oc.Price, oc.FreePrice)
                    If UpdateDBFromNewOrder <> ArtBErrors.Success Then GoTo updateDBFromNewOrderEnd
                    If Not IsNothing(NEWORDER) Then
                        BuyOrderId = oc.BuyOrder.ORDER_ID
                        For Each ocb As MARKET_MATCHING_CLASS In MarketMatchingOrdres
                            If ocb.BuyOrder.ORDER_ID = BuyOrderId Then
                                ocb.BuyOrder = Nothing
                                ocb.BuyOrder = NEWORDER
                            End If
                        Next
                    End If
                    If Not IsNothing(OtherNewOrder) Then
                        SellOrderId = oc.SellOrder.ORDER_ID
                        For Each ocs As MARKET_MATCHING_CLASS In MarketMatchingOrdres
                            If ocs.SellOrder.ORDER_ID = SellOrderId Then
                                ocs.SellOrder = Nothing
                                ocs.SellOrder = OtherNewOrder
                            End If
                        Next
                    End If
                    If oc.BuyOrder.DESK_TRADER_ID = SystemDeskTraderId Or oc.SellOrder.DESK_TRADER_ID = SystemDeskTraderId Then bIsShadow = True
                Next
            End If


        End If


        'Error reporting
        'Dim s As String = ""
        'OrderDescr(cpl, 1, s)

        'Debug.Print("Successfull handling of order:" & cpl.ORDER_ID.ToString() & _
        '                    ", " & cpl.ORDER_DATETIME.ToString() & ", " & _
        '                    PrevLiveStatus & "->" & cpl.LIVE_STATUS & ":" & vbNewLine & s)

        UpdateDBFromNewOrder = ArtBErrors.Success
updateDBFromNewOrderEnd:

    End Function

    Public Function TradeOrders(ByRef gdb As DB_ARTB_NETDataContext, _
                                    ByRef order1 As ORDERS_FFA_CLASS, _
                                    ByRef order2 As ORDERS_FFA_CLASS, _
                                    ByRef NewOrder As ORDERS_FFA_CLASS, _
                                    ByRef NewSecOrder As ORDERS_FFA_CLASS, _
                                    ByRef OrderStr As String, _
                                    ByRef TradeStr As String, _
                                    ByRef AffectedRoutePeriods As List(Of String), _
                                    ByVal ActualQuantity As Integer, _
                                    Optional ByVal OnExchangeId As Integer = 0, _
                                    Optional ByVal TradePrice As Double = -1.0E+20, _
                                    Optional ByVal FreePrice As Boolean = False) As Integer
        Dim MatchingExchanges As New List(Of Integer)
        Dim minOrderSide As Integer = 0
        NewOrder = Nothing
        Dim bCheckPrices As Boolean = True

        If TradePrice > -1.0E+20 Then bCheckPrices = False

        If MatchingOrders(gdb, order1, order2, MatchingExchanges, True, bCheckPrices) = 0 Then
            TradeOrders = ArtBErrors.OrdersNotMatch
            Exit Function
        End If

        If OnExchangeId > 0 Then
            If MatchingExchanges.Contains(OnExchangeId) = False Then
                TradeOrders = ArtBErrors.OrdersNotMatch
                Exit Function
            End If
            MatchingExchanges.Clear()
            MatchingExchanges.Add(OnExchangeId)
        End If

        Dim SpreadOrder1 As ORDERS_FFA_CLASS = Nothing
        Dim SpreadOrder2 As ORDERS_FFA_CLASS = Nothing

        If NullInt2Int(order1.SPREAD_ORDER_ID) <> 0 Then
            SpreadOrder1 = New ORDERS_FFA_CLASS
            TradeOrders = SpreadOrder1.GetFromID(gdb, order1.SPREAD_ORDER_ID)
            If TradeOrders <> ArtBErrors.Success Then Exit Function
        End If

        If NullInt2Int(order2.SPREAD_ORDER_ID) <> 0 Then
            SpreadOrder2 = New ORDERS_FFA_CLASS
            TradeOrders = SpreadOrder2.GetFromID(gdb, order2.SPREAD_ORDER_ID)
            If TradeOrders <> ArtBErrors.Success Then Exit Function
        End If

        Dim trade = New TRADES_FFA_CLASS
        If MatchingExchanges.Count = 1 Then
            order1.ORDER_TRADED_ON_EXCHANGE = MatchingExchanges(0)
            order2.ORDER_TRADED_ON_EXCHANGE = MatchingExchanges(0)
            trade.EXCHANGE_ID = order2.ORDER_TRADED_ON_EXCHANGE
        Else
            order1.ORDER_TRADED_ON_EXCHANGE = Nothing
            order2.ORDER_TRADED_ON_EXCHANGE = Nothing
            trade.EXCHANGE_ID = Nothing
        End If

        trade.ORDER_ID1 = order1.ORDER_ID
        trade.ORDER_ID2 = order2.ORDER_ID
        trade.TRADE_BS1 = order1.ORDER_BS
        trade.TRADE_BS2 = order2.ORDER_BS
        trade.DESK_TRADER_ID1 = order1.FOR_DESK_TRADER_ID
        trade.DESK_TRADER_ID2 = order2.FOR_DESK_TRADER_ID
        trade.INFORM_DESK_ID1 = GetTraderDeskIdFromDB(gdb, order1.DESK_TRADER_ID)
        trade.INFORM_DESK_ID2 = GetTraderDeskIdFromDB(gdb, order2.DESK_TRADER_ID)
        trade.QUANTITY = ActualQuantity
        If TradePrice > -1.0E+20 Then
            trade.PRICE_TRADED = TradePrice
        ElseIf order2.ORDER_DATETIME > order1.ORDER_DATETIME Then
            trade.PRICE_TRADED = order1.PRICE_INDICATED
        Else
            trade.PRICE_TRADED = order2.PRICE_INDICATED
        End If

        trade.CLEARING_ID1 = order1.CLEARER_ID
        trade.CLEARING_ID2 = order2.CLEARER_ID
        trade.ROUTE_ID = order2.ROUTE_ID
        trade.MM1 = order2.MM1
        trade.YY1 = order2.YY1
        trade.MM2 = order2.MM2
        trade.YY2 = order2.YY2
        trade.PNC = order1.PNC_ORDER Or order2.PNC_ORDER
        Dim sdt As Object = GetServerDateTime()
        If Not IsNothing(sdt) Then
            Try
                trade.ORDER_DATETIME = sdt
            Catch ex As Exception
                Debug.Print(ex.ToString)
                trade.ORDER_DATETIME = DateTime.UtcNow.Add(GlobalDateTimeDiff)
            End Try
        Else
            trade.ORDER_DATETIME = DateTime.UtcNow.Add(GlobalDateTimeDiff)
        End If

        trade.SHORTDES = order2.SHORTDES
        trade.TRADE_TYPE = order1.ORDER_TYPE


        Dim q1 As Double = GetActualQuantity(order1)
        Dim q2 As Double = GetActualQuantity(order2)
        Dim olq1 As Double = 0
        Dim olq2 As Double = 0

        If trade.TRADE_TYPE <> OrderTypes.FFA Then
            olq1 = GetActualQuantity(order1, 2)
            olq2 = GetActualQuantity(order2, 2)
            trade.ROUTE_ID2 = order2.ROUTE_ID2
            trade.MM21 = order2.MM21
            trade.YY21 = order2.YY21
            trade.MM22 = order2.MM22
            trade.YY22 = order2.YY22
            If order2.SHORTDES2 <> order2.SHORTDES Then
                trade.SHORTDES = trade.SHORTDES & "/" & order2.SHORTDES
            End If
        End If

        trade.ORDER_QUALIFIER = order1.ORDER_QUALIFIER
        If Int(q1) = ActualQuantity Then
            trade.DAY_QUALIFIER = order1.DAY_QUALIFIER
            trade.QUANTITY = order1.ORDER_QUANTITY
            If olq1 <> 0 Then
                trade.DAY_QUALIFIER2 = order1.DAY_QUALIFIER2
                trade.QUANTITY2 = order1.ORDER_QUANTITY2
            End If
        ElseIf Int(q2) = ActualQuantity Then
            trade.DAY_QUALIFIER = order2.DAY_QUALIFIER
            trade.QUANTITY = order2.ORDER_QUANTITY
            If olq2 <> 0 Then
                trade.DAY_QUALIFIER2 = order2.DAY_QUALIFIER2
                trade.QUANTITY2 = order2.ORDER_QUANTITY2
            End If
        Else
            trade.QUANTITY = ActualQuantity
            trade.DAY_QUALIFIER = order1.DAY_QUALIFIER
            If trade.DAY_QUALIFIER <> OrderDayQualifier.NotInDays Then trade.DAY_QUALIFIER = OrderDayQualifier.DPM
            If olq1 <> 0 And q1 <> 0 Then
                trade.DAY_QUALIFIER2 = ActualQuantity * olq1 / q1
                trade.QUANTITY2 = order1.ORDER_QUANTITY2
                If trade.DAY_QUALIFIER2 <> OrderDayQualifier.NotInDays Then trade.DAY_QUALIFIER2 = OrderDayQualifier.DPM
            End If
        End If

        TradeFillBrokers(gdb, trade)
        RoundTradePrice(gdb, trade)

        Dim MoveNegotiationOrdersFrom As ORDERS_FFA_CLASS = Nothing
        Dim MoveNegotiationOrdersTo As ORDERS_FFA_CLASS = Nothing
        Dim NewOrderId1 As Integer = 0
        Dim NewOrderId2 As Integer = 0
        Dim NegotiationOrderID1 As Integer = NullInt2Int(order1.NEGOTIATION_ORDER_ID)
        Dim NegotiationOrderID2 As Integer = NullInt2Int(order2.NEGOTIATION_ORDER_ID)
        Dim LockOrderID1 As Integer = NullInt2Int(order1.LOCK_ORDER_ID)
        Dim LockOrderID2 As Integer = NullInt2Int(order2.LOCK_ORDER_ID)
        Dim CLockOrderID1 As Integer = 0
        Dim CLockOrderID2 As Integer = 0

        If Int(q1) > ActualQuantity Then
            NewOrder = order1.GetNewCopy()
            NewOrder.SetQuantity(Int(q1) - ActualQuantity)
            NewOrder.PREVIOUS_ORDER_ID = order1.ORDER_ID
            If MatchingExchanges.Count = 1 And _
                order1.SINGLE_EXCHANGE_EXECUTION And _
                order1.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
                LimitExchangeStr(NewOrder, MatchingExchanges(0))
            End If
            'If order1.ORDER_QUALIFIER = "R" And order1.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
            '    NewOrder.LIVE_STATUS = "A"
            '    NewOrder.ORDER_QUALIFIER = "R"
            '    NewOrder.PRICE_TYPE = "I"
            'End If
            TradeOrders = InsertUpdateOrder(gdb, NewOrder, False)
            If TradeOrders <> ArtBErrors.Success Then Exit Function
            If order1.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
                MoveNegotiationOrdersFrom = order1
                MoveNegotiationOrdersTo = NewOrder
            End If
            TradeOrders = NewOrder.AppendToStr(OrderStr)
            If TradeOrders <> ArtBErrors.Success Then Exit Function
            NewOrderId1 = NewOrder.ORDER_ID
        End If

        If Not IsNothing(SpreadOrder1) Then
            Dim SpreadActualQ As Double
            If order1.ORDER_BS = SpreadOrder1.ORDER_BS Then
                'order1 is first leg
                If NewOrderId1 <> 0 Then SpreadOrder1.CROSS_ORDER_ID1 = NewOrderId1
                SpreadActualQ = Int(GetActualQuantity(SpreadOrder1, 1))
                If MatchingExchanges.Count = 1 And _
                   SpreadOrder1.SINGLE_EXCHANGE_EXECUTION And _
                   SpreadOrder1.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
                    LimitExchangeStr(SpreadOrder1, MatchingExchanges(0))
                End If
                If SpreadActualQ > ActualQuantity Then
                    'If order2.ORDER_QUALIFIER <> "N" Then SpreadOrder1.SetQuantity(SpreadActualQ - ActualQuantity)
                    SpreadOrder1.SetQuantity(SpreadActualQ - ActualQuantity)
                    If NewOrderId1 = 0 Then
                        SpreadOrder1.CROSS_ORDER_ID1 = Nothing
                        SpreadOrder1.CROSS_ORDER_ID2 = Nothing
                    End If
                Else
                    SpreadOrder1.LIVE_STATUS = "E"
                End If
            Else
                'order1 is second leg
                If NewOrderId1 <> 0 Then SpreadOrder1.CROSS_ORDER_ID2 = NewOrderId1
                'If NewOrderId1 = 0 Then
                '    SpreadOrder1.CROSS_ORDER_ID2 = Nothing
                'End If
            End If
        End If

        If Int(q2) > ActualQuantity Then
            NewSecOrder = order2.GetNewCopy()
            NewSecOrder.SetQuantity(Int(q2) - ActualQuantity)

            NewSecOrder.PREVIOUS_ORDER_ID = order2.ORDER_ID
            If MatchingExchanges.Count = 1 And _
               order2.SINGLE_EXCHANGE_EXECUTION And _
               order2.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
                LimitExchangeStr(NewSecOrder, MatchingExchanges(0))
            End If

            'If order2.ORDER_QUALIFIER = "R" And order2.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
            '    NewSecOrder.LIVE_STATUS = "A"
            '    NewSecOrder.ORDER_QUALIFIER = "R"
            '    NewSecOrder.PRICE_TYPE = "I"
            'End If
            TradeOrders = InsertUpdateOrder(gdb, NewSecOrder, False)
            If TradeOrders <> ArtBErrors.Success Then Exit Function
            If order2.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
                MoveNegotiationOrdersFrom = order2
                MoveNegotiationOrdersTo = NewSecOrder
            End If
            TradeOrders = NewSecOrder.AppendToStr(OrderStr)
            If TradeOrders <> ArtBErrors.Success Then Exit Function
            NewOrderId2 = NewSecOrder.ORDER_ID
        End If

        If Not IsNothing(SpreadOrder2) Then
            Dim SpreadActualQ As Double
            If order2.ORDER_BS = SpreadOrder2.ORDER_BS Then
                'order2 is first leg
                If NewOrderId2 <> 0 Then SpreadOrder2.CROSS_ORDER_ID1 = NewOrderId2
                SpreadActualQ = Int(GetActualQuantity(SpreadOrder2, 1))
                If MatchingExchanges.Count = 1 And _
                   SpreadOrder2.SINGLE_EXCHANGE_EXECUTION And _
                   SpreadOrder2.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
                    LimitExchangeStr(SpreadOrder2, MatchingExchanges(0))
                End If
                If SpreadActualQ > ActualQuantity Then
                    'If order1.ORDER_QUALIFIER <> "N" Then SpreadOrder2.SetQuantity(SpreadActualQ - ActualQuantity)
                    SpreadOrder2.SetQuantity(SpreadActualQ - ActualQuantity)
                    If NewOrderId2 = 0 Then
                        SpreadOrder2.CROSS_ORDER_ID2 = Nothing
                        SpreadOrder2.CROSS_ORDER_ID1 = Nothing
                    End If
                Else
                    SpreadOrder2.LIVE_STATUS = "E"
                End If
            Else
                'order2 is second leg
                If NewOrderId2 <> 0 Then SpreadOrder2.CROSS_ORDER_ID2 = NewOrderId2
                'If NewOrderId2 = 0 Then
                '    SpreadOrder2.CROSS_ORDER_ID2 = Nothing
                'End If
            End If
        End If

        If Not IsNothing(MoveNegotiationOrdersFrom) And Not IsNothing(MoveNegotiationOrdersTo) Then
            TradeOrders = MoveNegotiationOrders(gdb, _
                                                    MoveNegotiationOrdersFrom, _
                                                    MoveNegotiationOrdersTo, _
                                                    OrderStr, TradeStr, AffectedRoutePeriods)
            If TradeOrders <> ArtBErrors.Success Then Exit Function
        End If

        If order1.ORDER_QUALIFIER = "R" Then
            'TradeOrders = SetOrderLiveStatus(gdb, order1.PREVIOUS_ORDER_ID, "E", OrderStr, TradeStr, AffectedRoutePeriods)
            'If TradeOrders <> ArtBErrors.Success Then Exit Function
        ElseIf order1.ORDER_QUALIFIER = "N" Then
            Dim FilterQ As Integer = 1000000000
            Dim DummyFilterQ As Integer = 1000000000
            Dim PExchangeID As Integer = 0
            If order1.SINGLE_EXCHANGE_EXECUTION And MatchingExchanges.Count = 1 Then
                PExchangeID = MatchingExchanges(0)
            End If
            If NegotiationOrderID1 = 0 And Not IsNothing(SpreadOrder1) Then
                NegotiationOrderID1 = SpreadOrder1.NEGOTIATION_ORDER_ID
            End If
            Dim SpreadLegType As Integer = NullInt2Int(order1.SPREAD_LEG_TYPE)
            TradeOrders = SetNegotiationOrderStatus(gdb, _
                                                        NegotiationOrderID1, _
                                                        ActualQuantity, _
                                                        FilterQ, _
                                                        OrderStr, TradeStr, AffectedRoutePeriods, _
                                                        PExchangeID, , SpreadLegType)
            If TradeOrders <> ArtBErrors.Success Then Exit Function

            If LockOrderID1 <> 0 And LockOrderID1 <> NegotiationOrderID1 Then
                TradeOrders = SetNegotiationOrderStatus(gdb, _
                                                            LockOrderID1, _
                                                            ActualQuantity, _
                                                            DummyFilterQ, _
                                                            OrderStr, TradeStr, AffectedRoutePeriods, _
                                                            PExchangeID, _
                                                            "D", SpreadLegType)
                If TradeOrders <> ArtBErrors.Success Then Exit Function
            End If

            Dim CounterOrderID As Integer = NullInt2Int(order1.COUNTER_PARTY_ORDER_ID)
            If CounterOrderID <> 0 Then
                Dim CounterOrderClass As New ORDERS_FFA_CLASS
                TradeOrders = CounterOrderClass.GetFromID(gdb, CounterOrderID)
                If TradeOrders <> ArtBErrors.Success Then Exit Function
                CLockOrderID1 = NullInt2Int(CounterOrderClass.LOCK_ORDER_ID)
                If CLockOrderID1 <> 0 And _
                   CLockOrderID1 <> NegotiationOrderID1 And _
                   CLockOrderID1 <> LockOrderID1 Then
                    TradeOrders = SetNegotiationOrderStatus(gdb, _
                                                                CLockOrderID1, _
                                                                ActualQuantity, _
                                                                DummyFilterQ, _
                                                                OrderStr, TradeStr, AffectedRoutePeriods, _
                                                                PExchangeID, _
                                                                "D", SpreadLegType)
                    If TradeOrders <> ArtBErrors.Success Then Exit Function
                End If
            End If

            TradeOrders = DeletePreviousNegotiations(gdb, _
                                                         order1.ORDER_ID, _
                                                         NegotiationOrderID1, _
                                                         FilterQ, _
                                                         OrderStr, _
                                                         TradeStr, _
                                                         AffectedRoutePeriods)
            If TradeOrders <> ArtBErrors.Success Then Exit Function
        End If

        If order2.ORDER_QUALIFIER = "R" Then
            'TradeOrders = SetOrderLiveStatus(gdb, order2.PREVIOUS_ORDER_ID, "E", OrderStr, TradeStr, AffectedRoutePeriods)
            'If TradeOrders <> ArtBErrors.Success Then Exit Function
        ElseIf order2.ORDER_QUALIFIER = "N" Then
            Dim FilterQ As Integer = 1000000000
            Dim DummyFilterQ As Integer = 1000000000
            Dim PExchangeID As Integer = 0
            If order2.SINGLE_EXCHANGE_EXECUTION And MatchingExchanges.Count = 1 Then
                PExchangeID = MatchingExchanges(0)
            End If
            If NegotiationOrderID2 = 0 And Not IsNothing(SpreadOrder2) Then
                If order2.ORDER_BS = SpreadOrder2.ORDER_BS Then NegotiationOrderID2 = SpreadOrder2.NEGOTIATION_ORDER_ID
            End If
            Dim SpreadLegType As Integer = NullInt2Int(order2.SPREAD_LEG_TYPE)

            If NegotiationOrderID2 <> NegotiationOrderID1 Then
                TradeOrders = SetNegotiationOrderStatus(gdb, _
                                                            NegotiationOrderID2, _
                                                            ActualQuantity, _
                                                            FilterQ, _
                                                            OrderStr, TradeStr, AffectedRoutePeriods, _
                                                            PExchangeID, , SpreadLegType)
                If TradeOrders <> ArtBErrors.Success Then Exit Function
            End If

            If LockOrderID2 <> 0 And _
               LockOrderID2 <> NegotiationOrderID2 And _
               LockOrderID2 <> NegotiationOrderID1 And _
               LockOrderID2 <> LockOrderID1 And _
               LockOrderID2 <> CLockOrderID1 Then
                TradeOrders = SetNegotiationOrderStatus(gdb, _
                                                            LockOrderID2, _
                                                            ActualQuantity, _
                                                            DummyFilterQ, _
                                                            OrderStr, TradeStr, AffectedRoutePeriods, _
                                                            PExchangeID, _
                                                            "D", SpreadLegType)
                If TradeOrders <> ArtBErrors.Success Then Exit Function
            End If

            Dim CounterOrderID As Integer = NullInt2Int(order2.COUNTER_PARTY_ORDER_ID)
            If CounterOrderID <> 0 Then
                Dim CounterOrderClass As New ORDERS_FFA_CLASS
                TradeOrders = CounterOrderClass.GetFromID(gdb, CounterOrderID)
                If TradeOrders <> ArtBErrors.Success Then Exit Function
                CLockOrderID2 = NullInt2Int(CounterOrderClass.LOCK_ORDER_ID)
                If CLockOrderID2 <> 0 And _
                   CLockOrderID2 <> NegotiationOrderID1 And _
                   CLockOrderID2 <> NegotiationOrderID2 And _
                   CLockOrderID2 <> LockOrderID1 And _
                   CLockOrderID2 <> LockOrderID2 And _
                   CLockOrderID2 <> CLockOrderID1 Then
                    TradeOrders = SetNegotiationOrderStatus(gdb, _
                                                                CLockOrderID2, _
                                                                ActualQuantity, _
                                                                DummyFilterQ, _
                                                                OrderStr, TradeStr, AffectedRoutePeriods, _
                                                                PExchangeID, _
                                                                "D", SpreadLegType)
                    If TradeOrders <> ArtBErrors.Success Then Exit Function
                End If
            End If

            TradeOrders = DeletePreviousNegotiations(gdb, _
                                                         order2.ORDER_ID, _
                                                         NegotiationOrderID2, _
                                                         FilterQ, _
                                                         OrderStr, _
                                                         TradeStr, _
                                                         AffectedRoutePeriods)
            If TradeOrders <> ArtBErrors.Success Then Exit Function
        End If

        order1.COUNTER_PARTY_ORDER_ID = order2.ORDER_ID
        order2.COUNTER_PARTY_ORDER_ID = order1.ORDER_ID

        order1.LIVE_STATUS = "E"
        order2.LIVE_STATUS = "E"

        TradeOrders = UpdateDBFromNewOrder(gdb, order1, OrderStr, TradeStr, AffectedRoutePeriods, False)
        If TradeOrders <> ArtBErrors.Success Then Exit Function

        TradeOrders = UpdateDBFromNewOrder(gdb, order2, OrderStr, TradeStr, AffectedRoutePeriods, False)
        If TradeOrders <> ArtBErrors.Success Then Exit Function

        If Not IsNothing(SpreadOrder1) Then
            TradeOrders = UpdateDBFromNewOrder(gdb, SpreadOrder1, OrderStr, TradeStr, AffectedRoutePeriods, False)
            If TradeOrders <> ArtBErrors.Success Then Exit Function
            SpreadOrder1 = Nothing
        End If
        If Not IsNothing(SpreadOrder2) Then
            TradeOrders = UpdateDBFromNewOrder(gdb, SpreadOrder2, OrderStr, TradeStr, AffectedRoutePeriods, False)
            If TradeOrders <> ArtBErrors.Success Then Exit Function
            SpreadOrder2 = Nothing
        End If

        TradeOrders = trade.Insert(gdb, True)
        If TradeOrders <> ArtBErrors.Success Then Exit Function

        If trade.TRADE_TYPE <> OrderTypes.FFA Then
            Dim LegTrade1 As TRADES_FFA_CLASS = trade.GetNewCopy()
            Dim CrossOrderID11 As Integer = NullInt2Int(order1.CROSS_ORDER_ID1)
            Dim CrossOrderID12 As Integer = NullInt2Int(order1.CROSS_ORDER_ID2)
            Dim CrossOrderID21 As Integer = NullInt2Int(order2.CROSS_ORDER_ID1)
            Dim CrossOrderID22 As Integer = NullInt2Int(order2.CROSS_ORDER_ID2)
            Dim LegOrder11 As ORDERS_FFA_CLASS = Nothing
            Dim LegOrder12 As ORDERS_FFA_CLASS = Nothing
            Dim LegOrder21 As ORDERS_FFA_CLASS = Nothing
            Dim LegOrder22 As ORDERS_FFA_CLASS = Nothing

            If CrossOrderID11 = 0 Then
                TradeOrders = CreateExecutedSpreadLeg(gdb, order1, 1, LegOrder11, _
                                                          AffectedRoutePeriods, OrderStr, TradeStr)
                If TradeOrders <> ArtBErrors.Success Then Exit Function
                CrossOrderID11 = NullInt2Int(LegOrder11.ORDER_ID)
                LegOrder11 = Nothing
            End If
            If CrossOrderID12 = 0 Then
                TradeOrders = CreateExecutedSpreadLeg(gdb, order1, 2, LegOrder12, _
                                                          AffectedRoutePeriods, OrderStr, TradeStr)
                If TradeOrders <> ArtBErrors.Success Then Exit Function
                CrossOrderID12 = NullInt2Int(LegOrder12.ORDER_ID)
                LegOrder12 = Nothing
            End If
            If CrossOrderID21 = 0 Then
                TradeOrders = CreateExecutedSpreadLeg(gdb, order2, 1, LegOrder21, _
                                                          AffectedRoutePeriods, OrderStr, TradeStr)
                If TradeOrders <> ArtBErrors.Success Then Exit Function
                CrossOrderID21 = NullInt2Int(LegOrder21.ORDER_ID)
                LegOrder21 = Nothing
            End If
            If CrossOrderID22 = 0 Then
                TradeOrders = CreateExecutedSpreadLeg(gdb, order2, 2, LegOrder22, _
                                                          AffectedRoutePeriods, OrderStr, TradeStr)
                If TradeOrders <> ArtBErrors.Success Then Exit Function
                CrossOrderID22 = NullInt2Int(LegOrder22.ORDER_ID)
                LegOrder22 = Nothing
            End If

            LegTrade1.ORDER_ID1 = CrossOrderID11
            LegTrade1.ORDER_ID2 = CrossOrderID21

            If trade.TRADE_TYPE = OrderTypes.RatioSpread Or trade.TRADE_TYPE = OrderTypes.CalendarSpread Or trade.TRADE_TYPE = OrderTypes.PriceSpread Then
                LegTrade1.PRICE_TRADED = GetAvgReferencePrice(gdb, CrossOrderID11)
            Else
                If order2.ORDER_DATETIME > order1.ORDER_DATETIME Then
                    LegTrade1.PRICE_TRADED = order1.PRICE_INDICATED
                Else
                    LegTrade1.PRICE_TRADED = order2.PRICE_INDICATED
                End If
            End If

            LegTrade1.TRADE_TYPE = OrderTypes.FFA
            LegTrade1.IS_SYNTHETIC = True
            LegTrade1.ROUTE_ID = order1.ROUTE_ID
            LegTrade1.MM1 = order1.MM1
            LegTrade1.MM2 = order1.MM2
            LegTrade1.YY1 = order1.YY1
            LegTrade1.YY2 = order1.YY2
            LegTrade1.SPREAD_TRADE_ID1 = trade.TRADE_ID
            LegTrade1.SPREAD_TRADE_ID2 = trade.TRADE_ID
            LegTrade1.SHORTDES = order1.SHORTDES
            RoundTradePrice(gdb, LegTrade1)

            TradeOrders = LegTrade1.Insert(gdb, True)
            If TradeOrders <> ArtBErrors.Success Then Exit Function

            TradeOrders = LegTrade1.AppendToStr(TradeStr)
            If TradeOrders <> ArtBErrors.Success Then Exit Function


            Dim LegTrade2 As TRADES_FFA_CLASS = trade.GetNewCopy()
            LegTrade2.ORDER_ID1 = CrossOrderID12
            LegTrade2.ORDER_ID2 = CrossOrderID22
            LegTrade2.TRADE_TYPE = OrderTypes.FFA
            LegTrade2.IS_SYNTHETIC = True
            LegTrade2.ROUTE_ID = order1.ROUTE_ID2
            LegTrade2.MM1 = order1.MM21
            LegTrade2.MM2 = order1.MM22
            LegTrade2.YY1 = order1.YY21
            LegTrade2.YY2 = order1.YY22
            LegTrade2.TRADE_BS1 = order2.ORDER_BS
            LegTrade2.TRADE_BS2 = order1.ORDER_BS
            LegTrade2.SPREAD_TRADE_ID1 = trade.TRADE_ID
            LegTrade2.SPREAD_TRADE_ID2 = trade.TRADE_ID

            LegTrade2.SHORTDES = order1.SHORTDES2


            Dim lq1 As Integer = Int(GetActualQuantity(order1, 2))
            Dim aq As Integer = ActualQuantity * lq1 / q1
            LegTrade2.QUANTITY = aq
            If LegTrade2.DAY_QUALIFIER <> OrderDayQualifier.NotInDays Then LegTrade2.DAY_QUALIFIER = OrderDayQualifier.DPM
            If trade.TRADE_TYPE = OrderTypes.RatioSpread Then
                LegTrade2.PRICE_TRADED = LegTrade1.PRICE_TRADED / trade.PRICE_TRADED
            ElseIf trade.TRADE_TYPE = OrderTypes.CalendarSpread Or trade.TRADE_TYPE = OrderTypes.PriceSpread Then
                LegTrade2.PRICE_TRADED = LegTrade1.PRICE_TRADED - trade.PRICE_TRADED
            Else
                If order2.ORDER_DATETIME > order1.ORDER_DATETIME Then
                    LegTrade2.PRICE_TRADED = order1.PRICE_INDICATED2
                Else
                    LegTrade2.PRICE_TRADED = order2.PRICE_INDICATED2
                End If
            End If

            RoundTradePrice(gdb, LegTrade2)

            TradeOrders = LegTrade2.Insert(gdb, True)
            If TradeOrders <> ArtBErrors.Success Then Exit Function

            TradeOrders = LegTrade2.AppendToStr(TradeStr)
            If TradeOrders <> ArtBErrors.Success Then Exit Function


            'If CrossOrderID11 <> 0 Then
            '    TradeOrders = SetFastOrderLiveStatus(CrossOrderID11, "E", OrderStr, TradeStr, AffectedRoutePeriods)
            '    If TradeOrders <> ArtBErrors.Success Then Exit Function
            'End If

            'If CrossOrderID12 <> 0 Then
            '    TradeOrders = SetFastOrderLiveStatus(CrossOrderID12, "E", OrderStr, TradeStr, AffectedRoutePeriods)
            '    If TradeOrders <> ArtBErrors.Success Then Exit Function
            'End If

            'If CrossOrderID21 <> 0 Then
            '    TradeOrders = SetFastOrderLiveStatus(CrossOrderID21, "E", OrderStr, TradeStr, AffectedRoutePeriods)
            '    If TradeOrders <> ArtBErrors.Success Then Exit Function
            'End If

            'If CrossOrderID22 <> 0 Then
            '    TradeOrders = SetFastOrderLiveStatus(CrossOrderID22, "E", OrderStr, TradeStr, AffectedRoutePeriods)
            '    If TradeOrders <> ArtBErrors.Success Then Exit Function
            'End If

            LegOrder11 = Nothing
            LegOrder12 = Nothing
            LegOrder21 = Nothing
            LegOrder22 = Nothing
        End If

        TradeOrders = trade.AppendToStr(TradeStr)
        If TradeOrders <> ArtBErrors.Success Then Exit Function

        TradeOrders = ArtBErrors.Success
    End Function

    Public Function GetOrderTimePeriod(ByRef o As Object, Optional ByVal a_Leg As Integer = 1) As ArtBTimePeriod
        GetOrderTimePeriod = New ArtBTimePeriod

        If a_Leg = 1 Then
            GetOrderTimePeriod.FillMY(o.MM1, o.YY1, o.MM2, o.YY2)
        Else
            GetOrderTimePeriod.FillMY(o.MM21, o.YY21, o.MM22, o.YY22)
        End If
        GetOrderTimePeriod.FillDates()
        GetOrderTimePeriod.FillDPM()
    End Function

    Public Function GetActualQuantity(ByRef o As Object, Optional ByVal a_Leg As Integer = 1) As Double
        If Not (TypeOf o Is ORDERS_FFA Or TypeOf o Is ORDERS_FFA_CLASS Or _
                TypeOf o Is TRADES_FFA Or TypeOf o Is TRADES_FFA_CLASS) Then Return 0
        Dim tp As ArtBTimePeriod
        If a_Leg = 1 Then
            tp = GetOrderTimePeriod(o, 1)

            If TypeOf o Is ORDERS_FFA Or TypeOf o Is ORDERS_FFA_CLASS Then
                GetActualQuantity = o.ORDER_QUANTITY
            Else
                GetActualQuantity = o.QUANTITY
            End If

            Select Case o.DAY_QUALIFIER
                Case OrderDayQualifier.Full
                    GetActualQuantity = tp.DPM
                Case OrderDayQualifier.Half
                    GetActualQuantity = tp.DPM * 0.5
                Case OrderDayQualifier.ContractDays
                    GetActualQuantity = o.ORDER_QUANTITY / (tp.EndMonth - tp.StartMonth + 1)
            End Select
        Else
            tp = GetOrderTimePeriod(o, 2)
            If TypeOf o Is ORDERS_FFA Or TypeOf o Is ORDERS_FFA_CLASS Then
                GetActualQuantity = o.ORDER_QUANTITY2
            Else
                GetActualQuantity = o.QUANTITY2
            End If
            Select Case o.DAY_QUALIFIER2
                Case OrderDayQualifier.Full
                    GetActualQuantity = tp.DPM
                Case OrderDayQualifier.Half
                    GetActualQuantity = tp.DPM * 0.5
                Case OrderDayQualifier.ContractDays
                    GetActualQuantity = o.ORDER_QUANTITY2 / (tp.EndMonth - tp.StartMonth + 1)
            End Select
        End If

        tp = Nothing
    End Function

    Public Function GetRouteTickFromDB(ByRef gdb As DB_ARTB_NETDataContext, _
                                       ByVal a_RouteId As Integer) As Double
        GetRouteTickFromDB = 0
        Try
            Dim ol = From q In gdb.ROUTEs _
                     Where q.ROUTE_ID = a_RouteId _
                     Select q.PRICING_TICK
            For Each q In ol
                Return q
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Function

    Public Function GetDeskExchangeRankingFromDB(ByRef gdb As DB_ARTB_NETDataContext, _
                                                 ByVal a_DeskId As Integer, _
                                                 ByVal a_ExchangeId As Integer, _
                                                 ByVal a_RouteId As Integer) As Integer
        GetDeskExchangeRankingFromDB = 0
        Try
            Dim ol = From q In gdb.DESK_EXCHANGEs _
                     Join t In gdb.TRADE_CLASSes On q.TRADE_CLASS_SHORT Equals t.TRADE_CLASS_SHORT _
                     Join v In gdb.VESSEL_CLASSes On v.DRYWET Equals t.TRADE_CLASS_SHORT _
                     Join r In gdb.ROUTEs On r.VESSEL_CLASS_ID Equals v.VESSEL_CLASS_ID _
                        Where q.ACCOUNT_DESK_ID = a_DeskId _
                        And q.EXCHANGE_ID = a_ExchangeId _
                        And r.ROUTE_ID = a_RouteId _
                     Select q
            For Each q In ol
                GetDeskExchangeRankingFromDB = q.RANKING
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Function

    Public Function GetDeskExchangeRanking(ByVal a_DeskId As Integer, ByVal a_ExchangeId As Integer, ByVal a_RouteId As Integer) As Integer
        GetDeskExchangeRanking = 0
        Dim TC As String = "", VC As String = "", VCID As Integer = 0
        GetRouteInfo(a_RouteId, TC, VC, VCID)
        Dim AD As ACCOUNT_DESK_CLASS = GetAccountDeskClass(a_DeskId)
        Dim DT As DESK_TRADE_CLASS_CLASS = GetViewClass(AD.TRADE_CLASSES, TC)
        If IsNothing(DT) Then Exit Function
        Dim DX As DESK_EXCHANGE_CLASS = GetViewClass(DT.EXCHANGES, a_ExchangeId.ToString())
        If IsNothing(DX) Then Exit Function
        GetDeskExchangeRanking = DX.RANKING
    End Function

    Public Function GetTraderDeskIdFromDB(ByRef gdb As DB_ARTB_NETDataContext, _
                                          ByVal a_TraderId As Integer) As Integer
        GetTraderDeskIdFromDB = 0
        Try
            Dim ol = From q In gdb.DESK_TRADERs _
                     Where q.DESK_TRADER_ID = a_TraderId Select q
            For Each q In ol
                GetTraderDeskIdFromDB = q.ACCOUNT_DESK_ID
            Next
        Catch
        End Try
    End Function

    Public Function GetTraderDeskId(ByVal a_TraderId As Integer) As Integer
        GetTraderDeskId = 0
        Dim TradeClass = GetViewClass(DESK_TRADERS, a_TraderId.ToString())
        If IsNothing(TradeClass) Then Exit Function
        GetTraderDeskId = TradeClass.ACCOUNT_DESK_ID
    End Function

    Public Function GetDeskId(ByVal a_TraderId As Integer) As Integer
        GetDeskId = 0
        Dim TraderClass As DESK_TRADER_CLASS = GetViewClass(DESK_TRADERS, a_TraderId.ToString())
        If IsNothing(TraderClass) Then Exit Function
        GetDeskId = TraderClass.ACCOUNT_DESK_ID
    End Function

    Public Function GetDeskExchangeRankingsFromDB(ByRef gdb As DB_ARTB_NETDataContext, _
                                                  ByVal DeskID As Integer, _
                                                  ByRef o As ORDERS_FFA_CLASS) As Collection
        Dim xl = From x In gdb.EXCHANGEs Select x
        GetDeskExchangeRankingsFromDB = New Collection
        For Each x In xl
            Dim ranking As Integer = GetDeskExchangeRankingFromDB(gdb, DeskID, x.EXCHANGE_ID, o.ROUTE_ID)
            GetDeskExchangeRankingsFromDB.Add(ranking, x.EXCHANGE_ID.ToString())
        Next
    End Function

    Public Function GetDeskExchangeRankings(ByVal DeskID As Integer, ByRef o As ORDERS_FFA_CLASS) As Collection
        GetDeskExchangeRankings = New Collection
        For Each x In EXCHANGES
            Dim ranking As Integer = GetDeskExchangeRanking(DeskID, x.EXCHANGE_ID, o.ROUTE_ID)
            GetDeskExchangeRankings.Add(ranking, x.EXCHANGE_ID.ToString())
        Next
    End Function

    Public Function BestMatchingSwapOrder(ByRef gdb As DB_ARTB_NETDataContext, _
                                          ByRef o As ORDERS_FFA_CLASS) As Collection
        BestMatchingSwapOrder = Nothing

        Dim BuyOrder As Boolean = False
        Dim oRouteID As Integer = o.ROUTE_ID
        Dim oMM1 As Integer = o.MM1
        Dim oMM2 As Integer = o.MM2
        Dim oYY1 As Integer = o.YY1
        Dim oYY2 As Integer = o.YY2
        Dim oDeskId As Integer = GetTraderDeskIdFromDB(gdb, o.FOR_DESK_TRADER_ID)

        Dim ol = From q In gdb.ORDERS_FFAs Join s In gdb.DESK_TRADERs _
                    On q.FOR_DESK_TRADER_ID Equals s.DESK_TRADER_ID _
                 Where q.LIVE_STATUS = "A" _
                     And q.MM1 = oMM1 _
                     And q.YY1 = oYY1 _
                     And q.MM2 = oMM2 _
                     And q.YY2 = oYY2 _
                     And q.ROUTE_ID = oRouteID _
                     And q.SPREAD_ORDER_ID Is Nothing _
                     And (q.SPREAD_LEG_TYPE = 0 Or q.SPREAD_LEG_TYPE Is Nothing) _
                     And s.ACCOUNT_DESK_ID <> oDeskId _
                 Order By q.ORDER_DATETIME _
                 Select q

        Dim nc As New Collection
        Dim nmoc As New MATCHING_ORDERS_CLASS
        nmoc.Exchanges = New List(Of Integer)
        For Each co In ol
            If MatchingOrders(gdb, o, co, nmoc.Exchanges) Then
                nmoc.OrderClass = co
                nmoc.ActualQuantity = 0
                nc.Add(nmoc)
                nmoc = New MATCHING_ORDERS_CLASS
                nmoc.Exchanges = New List(Of Integer)
            End If
        Next
        BestMatchingSwapOrder = MIPOrders(o, nc, GetDeskExchangeRankingsFromDB(gdb, oDeskId, o))

        For Each noc As MATCHING_ORDERS_CLASS In nc
            noc.Exchanges = Nothing
            noc = Nothing
        Next
        nc = Nothing
        nmoc = Nothing
    End Function

    Public Function PrivateOrderMatching(ByRef gdb As DB_ARTB_NETDataContext, _
                                          ByRef order1 As Object, _
                                          ByRef order2 As Object) As Boolean
        PrivateOrderMatching = False
        Dim OId As Integer = NullInt2Int(order2.COUNTER_PARTY_ORDER_ID)
        Dim o As New ORDERS_FFA_CLASS
        Dim ret As Integer = ArtBErrors.Success
        Dim Desk1 As Integer = 0
        Dim Desk2 As Integer = 0
        If OperationMode = GVCOpMode.Client Then
            o = GetViewClass(ORDERS_FFAS, OId.ToString())
            If IsNothing(o) Then
                ret = ArtBErrors.RecordNotFound
            Else
                Desk2 = GetTraderDeskId(o.FOR_DESK_TRADER_ID)
                Desk1 = GetTraderDeskId(order1.FOR_DESK_TRADER_ID)
            End If
        Else
            ret = o.GetFromID(gdb, OId)
            If ret = ArtBErrors.Success Then
                Desk2 = GetTraderDeskIdFromDB(gdb, o.FOR_DESK_TRADER_ID)
                Desk1 = GetTraderDeskIdFromDB(gdb, order1.FOR_DESK_TRADER_ID)
            End If
        End If

        If ret = ArtBErrors.Success Then
            If Desk1 = Desk2 Then PrivateOrderMatching = True
        End If
        o = Nothing
    End Function

    Public Function MatchingOrders(ByRef gdb As DB_ARTB_NETDataContext, _
                                   ByRef order1 As Object, _
                                   ByRef order2 As Object, _
                                   ByRef MatchingExchanges As List(Of Integer), _
                                   Optional ByVal CheckLiveStatus As Boolean = True, _
                                   Optional ByVal bMatchPrices As Boolean = True, _
                                   Optional ByVal bMatchSides As Boolean = True, _
                                   Optional ByVal bMatchLimits As Boolean = True) As Integer
        MatchingOrders = 0
        If order1.FOR_DESK_TRADER_ID = order2.FOR_DESK_TRADER_ID Then
            If order1.FOR_DESK_TRADER_ID <> SystemDeskTraderId Then Exit Function
        Else
            If CheckLiveStatus = True Then
                'If Not (order1.LIVE_STATUS = "A" Or _
                '        (order1.LIVE_STATUS = "E" And IsNothing(order1.SPREAD_ORDER_ID) = False)) Then Exit Function
                'If Not (order2.LIVE_STATUS = "A" Or _
                '        (order2.LIVE_STATUS = "E" And IsNothing(order2.SPREAD_ORDER_ID) = False)) Then Exit Function
                If order1.LIVE_STATUS <> "A" Then Exit Function
                If order2.LIVE_STATUS <> "A" Then Exit Function
                If order2.ORDER_QUALIFIER = "N" Then
                    If PrivateOrderMatching(gdb, order1, order2) = False Then Exit Function
                End If
                If order1.ORDER_QUALIFIER = "N" Then
                    If PrivateOrderMatching(gdb, order2, order1) = False Then Exit Function
                End If
            End If
        End If

        If order1.ORDER_GOOD_TILL <> OrderGoodTill.GTC _
            And order2.ORDER_GOOD_TILL <> OrderGoodTill.GTC Then
            Dim d1 As DateTime = order1.ORDER_DATETIME
            Dim d2 As DateTime = order2.ORDER_DATETIME
            If d1.Day <> d2.Day Then Exit Function
            If d1.Month <> d2.Month Then Exit Function
            If d1.Year <> d2.Year Then Exit Function

            'Add Time Limit Handling here

        End If
        If order1.ORDER_TYPE = order2.order_type Then
            If bMatchSides AndAlso order1.ORDER_BS = order2.ORDER_BS Then Exit Function

            If order1.ROUTE_ID <> order2.ROUTE_ID Then Exit Function

            If order1.MM1 <> order2.MM1 Then Exit Function
            If order1.MM2 <> order2.MM2 Then Exit Function
            If order1.YY1 <> order2.YY1 Then Exit Function
            If order1.YY2 <> order2.YY2 Then Exit Function

            If order1.PRICE_TYPE = "I" Then Exit Function
            If order2.PRICE_TYPE = "I" Then Exit Function

            If order1.order_type <> OrderTypes.FFA Then
                If order1.ROUTE_ID2 <> order2.ROUTE_ID2 Then Exit Function

                If order1.MM21 <> order2.MM21 Then Exit Function
                If order1.MM22 <> order2.MM22 Then Exit Function
                If order1.YY21 <> order2.YY21 Then Exit Function
                If order1.YY22 <> order2.YY22 Then Exit Function

                If order1.PRICE_TYPE2 = "I" Then Exit Function
                If order2.PRICE_TYPE2 = "I" Then Exit Function
            End If

            If order1.FLEXIBLE_QUANTITY = OrderFlexQuantinty.StrictFull Or _
                order2.FLEXIBLE_QUANTITY = OrderFlexQuantinty.StrictFull Then
                If order1.DAY_QUALIFIER = OrderDayQualifier.Full And order2.DAY_QUALIFIER <> OrderDayQualifier.Full Then
                    Exit Function
                End If
                If order2.DAY_QUALIFIER = OrderDayQualifier.Full And order1.DAY_QUALIFIER <> OrderDayQualifier.Full Then
                    Exit Function
                End If
                If Not IsNothing(order1.DAY_QUALIFIER2) And Not IsNothing(order2.DAY_QUALIFIER2) Then
                    If order1.DAY_QUALIFIER2 = OrderDayQualifier.Full And order2.DAY_QUALIFIER2 <> OrderDayQualifier.Full Then
                        Exit Function
                    End If
                    If order2.DAY_QUALIFIER2 = OrderDayQualifier.Full And order1.DAY_QUALIFIER2 <> OrderDayQualifier.Full Then
                        Exit Function
                    End If
                End If
            End If

        Else
            Exit Function
        End If

        Dim bExchangeFound = False
        Dim ox1() As Boolean
        Dim ox2() As Boolean
        ReDim ox1(TotalExchanges + 1)
        ReDim ox2(TotalExchanges + 1)
        Dim i As Integer

        GetOrderExchangesFromStr(order1, ox1)
        GetOrderExchangesFromStr(order2, ox2)

        MatchingExchanges.Clear()
        For i = 1 To TotalExchanges
            If ox1(i) = True And ox2(i) = True Then
                MatchingExchanges.Add(i)
                bExchangeFound = True
            End If
        Next

        If bExchangeFound = False Then Exit Function

        Dim bid As Double, offer As Double, side As Integer
        If order1.ORDER_BS = "B" Then
            side = 1
            bid = order1.PRICE_INDICATED
            offer = order2.PRICE_INDICATED
        Else
            side = 2
            bid = order2.PRICE_INDICATED
            offer = order1.PRICE_INDICATED
        End If
        If IsNothing(order1.SPREAD_ORDER_ID) And IsNothing(order2.SPREAD_ORDER_ID) Then
            If bid < offer And bMatchPrices Then Exit Function
        End If
        'Check Limits
        If bMatchLimits And order1.FOR_DESK_TRADER_ID <> SystemDeskTraderId Then
            If order1.FOR_DESK_TRADER_ID <> GlobalMatchingTrader And order2.FOR_DESK_TRADER_ID <> GlobalMatchingTrader Then
                If CheckCounterPartyLimits(gdb, _
                                           order1, _
                                           order1.FOR_DESK_TRADER_ID, _
                                           order2.FOR_DESK_TRADER_ID, _
                                           MatchingExchanges) = False Then Exit Function
            End If

        End If

        MatchingOrders = 1
        Exit Function
    End Function

    Public Function UpdateDBFromMesage(ByRef gdb As DB_ARTB_NETDataContext, _
                                       ByRef SourceStr As String, _
                                       ByVal ArtBMessage As Integer, _
                                       ByRef TradeStr As String) As Integer
        If (SourceStr.Length() < 1) Then
            UpdateDBFromMesage = ArtBErrors.Success
            Exit Function
        End If
        Dim s As String = SourceStr
        Dim xs As String = SourceStr

        Dim c As Collection = ParseString(s, ArtBMessage)
        Dim result As Integer
        If ArtBMessage = ArtBMessages.OrderFFANew Then
            If c.Count <> 1 Then
                UpdateDBFromMesage = ArtBErrors.InvalidNumberOfRecords
                Exit Function
            End If
        End If

        Try
            If gdb.Connection.State = ConnectionState.Closed Or gdb.Connection.State = ConnectionState.Broken Then
                gdb.Connection.Open()
            End If
            If gdb.Connection.State = ConnectionState.Closed Or gdb.Connection.State = ConnectionState.Broken Then
                bUpdateDBFromNewOrder = False
                Return ArtBErrors.ConnectionDead
            End If

            gdb.Transaction = gdb.Connection.BeginTransaction(IsolationLevel.ReadUncommitted)
        Catch
            bUpdateDBFromNewOrder = False
            Return ArtBErrors.ConnectionDead
        End Try
        UpdateDBFromMesage = ArtBErrors.Success

        For Each cpl In c
            Select Case ArtBMessage
                Case ArtBMessages.ChangeCounterPartyLimits, _
                     ArtBMessages.ChangeTraderAuthorities, _
                     ArtBMessages.OrderFFAInfo
                    result = cpl.Update(gdb)
                Case ArtBMessages.OrderFFATrade, _
                     ArtBMessages.TradeFFAInfo, _
                    ArtBMessages.ChangeFFATrade
                    result = InsertUpdateTradeInDB(gdb, cpl, ArtBMessage, xs, TradeStr)
            End Select
            If result <> ArtBErrors.Success Then Exit For
        Next

        UpdateDBFromMesage = result
        If UpdateDBFromMesage = ArtBErrors.Success Then
            Try
                gdb.SubmitChanges()
                gdb.Transaction.Commit()
                SourceStr = xs
            Catch
                gdb.Transaction.Rollback()
            End Try
        Else
            Try
                gdb.Transaction.Rollback()
            Catch
            End Try
        End If

        Try
            gdb.Transaction.Dispose()
            For Each cpl In c
                cpl = Nothing
            Next
            c = Nothing
        Catch
        End Try
    End Function

    Public Function InsertUpdateTradeInDB(ByRef gdb As DB_ARTB_NETDataContext, _
                                          ByRef Trade As TRADES_FFA_CLASS, _
                                          ByVal MessageType As Integer, _
                                          ByRef TradeStr As String, _
                                          ByRef ExportTradeStr As String) As Integer
        Dim t As New TRADES_FFA_CLASS

        InsertUpdateTradeInDB = t.GetFromID(gdb, Trade.TRADE_ID)
        If InsertUpdateTradeInDB = ArtBErrors.Success Then
            Dim OldTraderID1 As Integer = NullInt2Int(t.DESK_TRADER_ID1)
            Dim OldTraderID2 As Integer = NullInt2Int(t.DESK_TRADER_ID2)
            Dim NewTraderID1 As Integer = NullInt2Int(Trade.DESK_TRADER_ID1)
            Dim NewTraderID2 As Integer = NullInt2Int(Trade.DESK_TRADER_ID2)

            If OldTraderID1 <> 0 And NewTraderID1 <> 0 And OldTraderID1 <> NewTraderID1 Then
                InsertUpdateTradeInDB = ChangeTradeTrader(gdb, Trade, 1, OldTraderID1, NewTraderID1, TradeStr)
                If InsertUpdateTradeInDB <> ArtBErrors.Success Then
                    t = Nothing
                    Exit Function
                End If
            End If
            If OldTraderID2 <> 0 And NewTraderID2 <> 0 And OldTraderID2 <> NewTraderID2 Then
                InsertUpdateTradeInDB = ChangeTradeTrader(gdb, Trade, 2, OldTraderID2, NewTraderID2, TradeStr)
                If InsertUpdateTradeInDB <> ArtBErrors.Success Then
                    t = Nothing
                    Exit Function
                End If
            End If

            If NullInt2Int(Trade.INFORM_DESK_ID1) <> 0 And _
               NullInt2Int(t.INFORM_DESK_ID1) = 0 Then
                Trade.INFORM_DESK_ID1 = Nothing
            End If
            If NullInt2Int(Trade.INFORM_DESK_ID2) <> 0 And _
               NullInt2Int(t.INFORM_DESK_ID2) = 0 Then
                Trade.INFORM_DESK_ID2 = Nothing
            End If

            If MessageType = ArtBMessages.TradeFFAInfo Then
                t.INFORM_DESK_ID1 = Trade.INFORM_DESK_ID1
                t.INFORM_DESK_ID2 = Trade.INFORM_DESK_ID2
                InsertUpdateTradeInDB = t.Update(gdb, True)
                t = Nothing
            End If
        End If

        InsertUpdateTradeInDB = Trade.InsertUpdate(gdb, True)
        If InsertUpdateTradeInDB <> ArtBErrors.Success Then
            t = Nothing
            Exit Function
        End If
        If MessageType = ArtBMessages.ChangeFFATrade And Not IsNothing(t) Then
            Dim SpreadTradeID1 As Integer = NullInt2Int(t.SPREAD_TRADE_ID1)
            If SpreadTradeID1 <> 0 Then
                InsertUpdateTradeInDB = UpdateSpreadTradeConfos(gdb, SpreadTradeID1, TradeStr)
            End If
            If InsertUpdateTradeInDB <> ArtBErrors.Success Then
                t = Nothing
                Exit Function
            End If
            Dim SpreadTradeID2 As Integer = NullInt2Int(t.SPREAD_TRADE_ID2)
            If SpreadTradeID2 <> 0 And SpreadTradeID2 <> SpreadTradeID1 Then
                InsertUpdateTradeInDB = UpdateSpreadTradeConfos(gdb, SpreadTradeID2, TradeStr)
            End If

        End If

        'If MessageType = ArtBMessages.FicticiousTrade Then
        '    Dim s As String = ""
        '    InsertUpdateTradeInDB = Trade.AppendToStr(s)
        '    If InsertUpdateTradeInDB <> ArtBErrors.Success Then
        '        t = Nothing
        '        Exit Function
        '    End If
        '    ExportTradeStr &= s
        'End If
        t = Nothing
    End Function

    Public Function ChangeTradeTrader(ByRef gdb As DB_ARTB_NETDataContext, _
                                      ByRef Trade As TRADES_FFA_CLASS, _
                                      ByVal Side As Integer, _
                                      ByVal OldTraderID As Integer, _
                                      ByVal NewTraderID As Integer, _
                                      ByRef TradeStr As String) As Integer
        ChangeTradeTrader = ArtBErrors.Success
        If Trade Is Nothing Then Exit Function
        Try
            Dim SpreadTradeID As Integer
            Select Case Side
                Case 1
                    SpreadTradeID = NullInt2Int(Trade.SPREAD_TRADE_ID1)
                Case 2
                    SpreadTradeID = NullInt2Int(Trade.SPREAD_TRADE_ID2)
            End Select

            If SpreadTradeID = 0 Then
                SpreadTradeID = Trade.TRADE_ID
            End If
            Dim tl = From q In gdb.TRADES_FFAs _
                       Where q.SPREAD_TRADE_ID1 = SpreadTradeID Or q.SPREAD_TRADE_ID2 = SpreadTradeID Or q.TRADE_ID = SpreadTradeID _
                       Select q
            For Each q In tl
                Dim bHasChanges As Boolean = False

                If NullInt2Int(q.DESK_TRADER_ID1) = OldTraderID Then
                    q.DESK_TRADER_ID1 = NewTraderID
                    bHasChanges = True
                End If

                If NullInt2Int(q.DESK_TRADER_ID2) = OldTraderID Then
                    q.DESK_TRADER_ID2 = NewTraderID
                    bHasChanges = True
                End If

                If bHasChanges = True And q.TRADE_ID <> Trade.TRADE_ID Then
                    Dim t As New TRADES_FFA_CLASS
                    ChangeTradeTrader = t.GetFromObject(q)
                    If ChangeTradeTrader <> ArtBErrors.Success Then
                        t = Nothing
                        Exit Function
                    End If
                    ChangeTradeTrader = t.AppendToStr(TradeStr)
                    If ChangeTradeTrader <> ArtBErrors.Success Then
                        t = Nothing
                        Exit Function
                    End If
                End If
            Next

            gdb.SubmitChanges()
        Catch e As Exception
            ChangeTradeTrader = ArtBErrors.UpdateFailed
            Debug.Print(e.ToString())
            Exit Function
        End Try
    End Function

    Public Function UpdateSpreadTradeConfos(ByRef gdb As DB_ARTB_NETDataContext, _
                                            ByVal SpreadTradeID As Integer, _
                                            ByRef TradeStr As String) As Integer
        UpdateSpreadTradeConfos = ArtBErrors.Success
        Dim t As New TRADES_FFA_CLASS
        Dim ct As New TRADES_FFA_CLASS
        UpdateSpreadTradeConfos = t.GetFromID(gdb, SpreadTradeID)
        If UpdateSpreadTradeConfos = ArtBErrors.Success Then
            ct.GetFromObject(t)
            Dim tl = From q In gdb.TRADES_FFAs _
                     Where q.SPREAD_TRADE_ID1 = SpreadTradeID Or q.SPREAD_TRADE_ID2 = SpreadTradeID _
                     Select q

            ct.SENT_TO_CLEARING = True
            ct.CLEARING_ACCEPTED = True
            ct.DEAL_CONFIRMATION_SENT = True
            ct.SENT_TO_CLEARING2 = True
            ct.CLEARING_ACCEPTED2 = True
            ct.DEAL_CONFIRMATION_SENT2 = True

            For Each q In tl
                If q.SENT_TO_CLEARING = False Then
                    ct.SENT_TO_CLEARING = False
                    ct.SENT_TO_CLEARING2 = False
                End If
                If q.CLEARING_ACCEPTED = False Then
                    ct.CLEARING_ACCEPTED = False
                    ct.CLEARING_ACCEPTED2 = False
                End If
                If q.DEAL_CONFIRMATION_SENT = False Then
                    ct.DEAL_CONFIRMATION_SENT = False
                    ct.DEAL_CONFIRMATION_SENT2 = False
                End If
                If q.SENT_TO_CLEARING2 = False Then
                    ct.SENT_TO_CLEARING = False
                    ct.SENT_TO_CLEARING2 = False
                End If
                If q.CLEARING_ACCEPTED2 = False Then
                    ct.CLEARING_ACCEPTED = False
                    ct.CLEARING_ACCEPTED2 = False
                End If
                If q.DEAL_CONFIRMATION_SENT2 = False Then
                    ct.DEAL_CONFIRMATION_SENT = False
                    ct.DEAL_CONFIRMATION_SENT2 = False
                End If
            Next

            If t.Equal(ct) = False Then
                UpdateSpreadTradeConfos = ct.InsertUpdate(gdb, True)
                If UpdateSpreadTradeConfos <> ArtBErrors.Success Then
                    t = Nothing
                    ct = Nothing
                    Exit Function
                End If

                UpdateSpreadTradeConfos = ct.AppendToStr(TradeStr)
            End If
        End If

        t = Nothing
        ct = Nothing
    End Function

    Public Function UpdateFromMesage(ByVal SourceStr As String, ByVal ArtBMessage As Integer, ByRef UserInfo As TraderInfoClass) As Integer
        Dim result As Integer = ArtBErrors.Success
        Dim bs As String = SourceStr
        Dim c As Collection = ParseString(bs, ArtBMessage)
        Dim bExisted As Boolean = False
        Dim bTradesUpdated = False
        Dim RouteList As New List(Of Integer)
        Dim FirmUpList As New List(Of Integer)
        Dim NegotiationList As New List(Of Integer)
        Dim DirectHitFailedList As New List(Of Integer)
        Dim OrderUpdatedStatus As Integer = 0
        Dim TradeList As New List(Of Integer)
        Dim InfoList As New List(Of Integer)
        For Each cpl In c
            Select Case ArtBMessage
                Case ArtBMessages.ChangeCounterPartyLimits
                    Dim desk As ACCOUNT_DESK_CLASS = GetAccountDeskClass(cpl.PRI_ACCOUNT_DESK_ID.ToString)
                    If Not (desk Is Nothing) Then
                        Call InsertOrReplace(desk.COUNTER_PARTY_LIMITS, cpl, cpl.SEC_ACCOUNT_DESK_ID.ToString)
                    End If
                    OrderUpdatedStatus = 1
                    bTradesUpdated = True
                Case ArtBMessages.ChangeTraderAuthorities
                    Dim desk As ACCOUNT_DESK_CLASS = GetAccountDeskClass(cpl.PRI_ACCOUNT_DESK_ID.ToString)
                    If Not (desk Is Nothing) Then
                        Call InsertOrReplace(desk.TRADERS, cpl, cpl.DESK_TRADER_ID.ToString())
                    End If
                    Call InsertOrReplace(DESK_TRADERS, cpl, cpl.DESK_TRADER_ID.ToString())
                    Call InsertOrReplace(DESK_TRADERS_BY_OF, cpl, cpl.OF_ID)
                Case ArtBMessages.OrderFFANew, _
                     ArtBMessages.OrderFFAFirmUp, _
                     ArtBMessages.OrderFFAAmmend
                    Call InsertOrReplaceOrder(cpl, RouteList, FirmUpList, NegotiationList, DirectHitFailedList)
                Case ArtBMessages.OrderFFAChangeOwner
                    Call InsertOrReplaceOrder(cpl, RouteList, FirmUpList, NegotiationList, DirectHitFailedList)
                    RaiseEvent OrderFFAChangeOwner(cpl.ORDER_ID)
                Case ArtBMessages.OrderFFATrade, ArtBMessages.ChangeFFATrade
                    If InsertOrReplaceTrade(cpl, InfoList) = True Then
                        If TradeList.Contains(cpl.TRADE_ID) = False Then
                            TradeList.Add(cpl.TRADE_ID)
                        End If
                    End If
                    bTradesUpdated = True
                Case ArtBMessages.OrderFFAInfo
                    Call InsertOrReplaceOrder(cpl, RouteList, FirmUpList, NegotiationList, DirectHitFailedList)
                Case ArtBMessages.TradeFFAInfo
                    InsertOrReplace(TRADES_FFAS, cpl, cpl.trade_ID.ToString())
            End Select
        Next
        If bTradesUpdated Then
            For Each TradeId As Integer In InfoList
                If TradeId > 0 Then RaiseEvent TradeInfo(TradeId, 1)
                If TradeId < 0 Then RaiseEvent TradeInfo(-TradeId, 2)
            Next
            RaiseEvent TradesUpdated(TradeList, ArtBMessage)
        End If
        If OrderUpdatedStatus = 1 Then
            RaiseEvent OrdersUpdated(True)
        End If

        Dim AffectedRoutePeriods As New List(Of String)

        If RouteList.Count > 0 And ArtBMessage <> ArtBMessages.OrderFFAInfo Then
            If OrderUpdatedStatus = 0 Then RaiseEvent OrdersUpdated(False)
            Dim OrderIdList As New List(Of Integer)
            For Each q In c
                If OrderIdList.Contains(q.ORDER_ID) = False Then
                    OrderIdList.Add(q.ORDER_ID)
                    If q.ORDER_QUALIFIER <> "R" And q.ORDER_QUALIFIER <> "N" Then
                        Dim RPStr As String = RoutePeriodStringFromObj(q)
                        If AffectedRoutePeriods.Contains(RPStr) = False Then AffectedRoutePeriods.Add(RPStr)
                    End If
                End If
            Next

            RaiseEvent OrderFFAUpdated(RouteList, OrderIdList)
            OrderIdList.Clear()
            OrderIdList = Nothing
        End If

        RouteList.Clear()
        RouteList = Nothing

        HandleOrders(FirmUpList, NegotiationList, DirectHitFailedList)

        If ArtBMessage = ArtBMessages.OrderFFATrade Or ArtBMessage = ArtBMessages.OrderFFANew Or ArtBMessage = ArtBMessages.OrderFFAAmmend Then
            Dim AffectedRoutePeriodsStr As String = ""
            For Each RPStr As String In AffectedRoutePeriods
                AffectedRoutePeriodsStr &= RPStr & RECORD_SEPARATOR_STR
            Next
            RaiseEvent UpdateSpreadLegs(AffectedRoutePeriodsStr)
        End If

        UpdateFromMesage = ArtBErrors.Success

    End Function

    Public Function InsertOrReplaceTrade(ByRef cpl As TRADES_FFA_CLASS, ByRef InfoList As List(Of Integer)) As Boolean
        Dim key As String = cpl.TRADE_ID.ToString()
        If TRADES_FFAS.Contains(key) Then
            Dim xTrade As TRADES_FFA_CLASS = TRADES_FFAS(key)
            If xTrade.Equals(cpl) Then Return False
            TRADES_FFAS.Remove(key)
        End If
        TRADES_FFAS.Add(cpl, key)
        Call InsertOrReplace(TRADES_FFAS, cpl, cpl.TRADE_ID)
        If NullInt2Int(cpl.INFORM_DESK_ID1) <> 0 Then
            If InfoList.Contains(cpl.TRADE_ID) = False Then InfoList.Add(cpl.TRADE_ID)
        End If
        If NullInt2Int(cpl.INFORM_DESK_ID2) <> 0 Then
            If InfoList.Contains(-cpl.TRADE_ID) = False Then InfoList.Add(-cpl.TRADE_ID)
        End If
        Return True
    End Function

    Public Sub InsertOrReplaceOrder(ByRef cpl As ORDERS_FFA_CLASS, _
                                    ByRef RouteList As List(Of Integer), _
                                    ByRef FirmUPList As List(Of Integer), _
                                    ByRef NegotiationList As List(Of Integer), _
                                    ByRef DirectHitFailedList As List(Of Integer))
        Dim existingOrder As ORDERS_FFA_CLASS = Nothing
        For Each exc In cpl.EXCHANGES
            exc = Nothing
        Next
        Dim PrevLiveStatus As String = ""
        Dim PrevPriceType As String = ""
        cpl.EXCHANGES.Clear()

        If ORDERS_FFAS.Contains(cpl.ORDER_ID.ToString()) = True Then
            existingOrder = GetViewClass(ORDERS_FFAS, cpl.ORDER_ID.ToString())
            PrevLiveStatus = existingOrder.LIVE_STATUS
            PrevPriceType = existingOrder.PRICE_TYPE
            If Not IsNothing(existingOrder) Then
                If cpl.ORDER_DATETIME < existingOrder.ORDER_DATETIME Then
                    'Exit Sub
                End If
                If (existingOrder.LIVE_STATUS = "E" Or existingOrder.LIVE_STATUS = "D") And cpl.LIVE_STATUS <> "P" Then
                    If existingOrder.LIVE_STATUS = "D" Then
                        If cpl.LIVE_STATUS <> "D" Then Exit Sub
                    Else
                        Exit Sub
                    End If
                End If
            End If
        End If
        Dim s As String = cpl.ORDER_EXCHANGES
        Dim c As String, j As Integer, exchangeId As Integer = 1, clearerId As Integer = 0
        While Len(s) > 1
            j = s.IndexOf("_")
            If j < 0 Or j >= s.Length() Then
                c = s
                s = ""
            Else
                c = s.Substring(0, j)
                s = s.Substring(j + 1)
            End If
            clearerId = Str2Int(c)
            If Len(c) < 1 Then clearerId = -1
            If clearerId >= 0 Then
                Dim ofe As New ORDERS_FFA_EXCHANGE_CLASS
                ofe.EXCHANGE_ID = exchangeId
                ofe.ORDER_ID = cpl.ORDER_ID
                ofe.ACCOUNT_ID = clearerId
                Call InsertOrReplace(cpl.EXCHANGES, ofe, ofe.EXCHANGE_ID.ToString())
            End If
            exchangeId = exchangeId + 1
        End While
        Call InsertOrReplace(ORDERS_FFAS, cpl, cpl.ORDER_ID.ToString())
        If cpl.LIVE_STATUS = "R" Then
            If FirmUPList.Contains(cpl.ORDER_ID) = False Then FirmUPList.Add(cpl.ORDER_ID)
            Exit Sub
        End If
        If Not RouteList.Contains(cpl.ROUTE_ID) Then RouteList.Add(cpl.ROUTE_ID)
        If Not IsNothing(cpl.ROUTE_ID2) Then
            If Not RouteList.Contains(cpl.ROUTE_ID2) Then RouteList.Add(cpl.ROUTE_ID2)
        End If
        If cpl.ORDER_QUALIFIER = "R" Then
            'If cpl.LIVE_STATUS = "A" Then
            '    RaiseEvent OrderFFARequestRespond(cpl.ORDER_ID, NullInt2Int(cpl.ORDER_ID), True)
            'Else
            If cpl.LIVE_STATUS = "D" Then
                RaiseEvent OrderFFARequestRespond(cpl.ORDER_ID, NullInt2Int(cpl.COUNTER_PARTY_ORDER_ID), False)
            Else
                'RaiseEvent OrderFFARequestRespond(cpl.ORDER_ID, NullInt2Int(cpl.COUNTER_PARTY_ORDER_ID), True)
            End If
        End If

        If cpl.ORDER_QUALIFIER = "D" And cpl.LIVE_STATUS = "D" Then
            If DirectHitFailedList.Contains(cpl.ORDER_ID) = False Then DirectHitFailedList.Add(cpl.ORDER_ID)
        End If

        If cpl.ORDER_QUALIFIER = "N" And _
               cpl.LIVE_STATUS = "A" And PrevLiveStatus <> "N" And _
               NullInt2Int(cpl.NEGOTIATION_ORDER_ID) > 0 Then
            If NegotiationList.Contains(cpl.ORDER_ID) = False Then NegotiationList.Add(cpl.ORDER_ID)
        ElseIf cpl.ORDER_QUALIFIER = "N" And (cpl.LIVE_STATUS = "E" Or cpl.LIVE_STATUS = "D") Then
            If NegotiationList.Contains(cpl.ORDER_ID) Then NegotiationList.Remove(cpl.ORDER_ID)
        End If
    End Sub

    Public Function PopulatePeriods(ByVal RouteId As Integer, _
                                    ByRef ExchangeIds As List(Of Integer), _
                                    ByVal d As DateTime, _
                                    ByRef FrontMaxMonths As Integer, _
                                    ByRef HalfDays As Boolean) As List(Of ArtBTimePeriod)

        Dim DB As New DB_ARTB_NETDataContext(ArtBConnectionStr)
        PopulatePeriods = New List(Of ArtBTimePeriod)
        PopulatePeriods.Clear()
        Dim StartMonth As Integer = d.Month
        Dim StartYear As Integer = d.Year Mod 2000
        Dim StartDay As Integer = d.Day
        Dim CurrentMonth = StartMonth
        Dim CurrentYear = StartYear
        Dim i As Integer
        HalfDays = True

        Dim FrontYears As Integer = 1000
        Dim FrontMonths As Integer = 1000
        Dim FrontHalfYears As Integer = 1000
        Dim FrontQuarters As Integer = 1000
        Dim bMC01 As Boolean = True
        Dim bMC12 As Boolean = True
        Dim bMC012 As Boolean = True
        Dim bQC01 As Boolean = True
        FrontMaxMonths = 99
        Dim rt As ROUTE_CLASS = GetViewClass(ROUTES, RouteId.ToString())
        If rt Is Nothing Then Exit Function
        Dim GSetDay As Integer = DateTime.DaysInMonth(StartYear, StartMonth)

        Dim ldr As LAST_DAY_RULE_CLASS = GetViewClass(LAST_DAY_RULES, rt.LAST_DAY_RULE_ID.ToString())
        If ldr Is Nothing Then Exit Function

        Dim ldrm As LAST_DAY_RULE_MONTH_CLASS = GetViewClass(ldr.MONTHS, StartMonth.ToString())
        If ldrm Is Nothing Then Exit Function

        If ldrm.SETTLEMENT_DAY > 0 Then
            If GSetDay > ldrm.SETTLEMENT_DAY Then GSetDay = ldrm.SETTLEMENT_DAY
        End If

        Dim SD As New Date(StartYear + 2000, StartMonth, GSetDay, 0, 0, 0)

        Dim exh As HOLIDAY_CLASS
        Dim bCheck As Boolean = True

        While (bCheck = True)
            If SD.DayOfWeek() = DayOfWeek.Saturday Or SD.DayOfWeek() = DayOfWeek.Sunday Then
                SD = SD.AddDays(-1)
            Else
                exh = GetViewClass(HOLIDAYS, SD.ToString(ARTB_DATE_FORMATSTR))
                If exh Is Nothing Then
                    bCheck = False
                Else
                    SD = SD.AddDays(-1)
                End If
            End If
        End While

        If GSetDay = 0 Then
            GSetDay = SD.Day
        Else
            If GSetDay > SD.Day Then GSetDay = SD.Day
        End If
        Dim pc As EXCHANGE_ROUTE_PERIOD_CLASS
        For Each ExchangeId As Integer In ExchangeIds
            Dim ex As EXCHANGE_CLASS = GetViewClass(EXCHANGES, ExchangeId.ToString())
            If ex Is Nothing Then Exit Function

            If ex.HALF_DAYS = False Then HalfDays = False
            Dim ert As EXCHANGE_ROUTE_CLASS = GetViewClass(rt.EXCHANGES, ExchangeId.ToString())
            If ert Is Nothing Then Continue For

            pc = GetViewClass(EXCHANGE_ROUTE_PERIODS, ert.EXCHANGE_ROUTE_PERIOD_ID.ToString)
            If pc Is Nothing Then Continue For

            If pc.FRONT_YEARS < FrontYears Then FrontYears = pc.FRONT_YEARS
            If pc.FRONT_MONTHS < FrontMonths Then FrontMonths = pc.FRONT_MONTHS
            If pc.FRONT_HALF_YEARS < FrontHalfYears Then FrontHalfYears = pc.FRONT_HALF_YEARS
            If pc.FRONT_QUARTERS < FrontQuarters Then FrontQuarters = pc.FRONT_QUARTERS
            If pc.FRONT_MAX_MONTHS < FrontMaxMonths Then FrontMaxMonths = pc.FRONT_MAX_MONTHS
            bMC01 = bMC01 And pc.MC_0_1
            bMC12 = bMC12 And pc.MC_1_2
            bMC012 = bMC012 And pc.MC_0_1_2
            bQC01 = bQC01 And pc.QC_0_1
        Next

        Dim bStartFromNextMonth As Boolean = False
        If StartDay >= GSetDay Then bStartFromNextMonth = True

        If bStartFromNextMonth = True Then
            CurrentMonth = StartMonth + 1
            If CurrentMonth > 12 Then
                CurrentMonth = 1
                CurrentYear = StartYear + 1
            End If
        End If

        Dim SMonth As Integer = CurrentYear * 12 + CurrentMonth - 1
        Dim nextQ As Integer = Int((SMonth - 1) / 3) + 1
        Dim nMonths As Integer = FrontMonths

        ' Months
        For i = 0 To nMonths - 1
            Dim tp As New ArtBTimePeriod
            tp.StartMonth = SMonth + i
            tp.EndMonth = SMonth + i
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        Next

        If bMC01 Then
            Dim tp As New ArtBTimePeriod
            tp.StartMonth = SMonth
            tp.EndMonth = SMonth + 1
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        End If

        If bMC12 Then
            Dim tp As New ArtBTimePeriod
            tp.StartMonth = SMonth + 1
            tp.EndMonth = SMonth + 2
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        End If

        If bMC012 And SMonth Mod 3 <> 0 Then
            Dim tp As New ArtBTimePeriod
            tp.StartMonth = SMonth
            tp.EndMonth = SMonth + 2
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        End If

        Dim nextCal As Integer = CurrentYear + 1
        'Dim nQs As Integer = nextCal * 4 - nextQ
        'If nQs < FrontQuarters Then nQs = FrontQuarters and
        'If nQs > FrontQuarters + 1 Then nQs = FrontQuarters + 1
        Dim nQs As Integer = FrontQuarters
        For i = 0 To nQs - 1
            Dim tp As New ArtBTimePeriod
            tp.StartMonth = (nextQ + i) * 3
            tp.EndMonth = tp.StartMonth + 2
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        Next

        Dim nextHY As Integer = Int((SMonth - 1) / 6) + 1
        Dim nHYs As Integer = FrontHalfYears
        For i = 0 To nHYs - 1
            Dim tp As New ArtBTimePeriod
            tp.StartMonth = (nextHY + i) * 6
            tp.EndMonth = tp.StartMonth + 5
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        Next

        If bQC01 Then
            If nextQ Mod 2 = 0 Then
                Dim tp As New ArtBTimePeriod
                tp.StartMonth = (nextQ + 1) * 3
                tp.EndMonth = tp.StartMonth + 5
                tp.CreateDescr()
                If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
            Else
                Dim tp As New ArtBTimePeriod
                tp.StartMonth = (nextQ) * 3
                tp.EndMonth = tp.StartMonth + 5
                tp.CreateDescr()
                If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
            End If
        End If
        Dim nCals As Integer = FrontYears
        'If nCals > 5 Then nCals = 5
        If CurrentMonth <= 1 Then
            nextCal = CurrentYear
            nCals = nCals + 1
        End If
        For i = 0 To nCals - 1
            Dim tp As New ArtBTimePeriod
            tp.StartMonth = (nextCal + i) * 12
            tp.EndMonth = tp.StartMonth + 11
            tp.CreateDescr()
            If Not tp.AddToList(PopulatePeriods) Then tp = Nothing
        Next

        For Each r In PopulatePeriods
            r.ROUTE_ID = RouteId
            r.SERVER_DATE = d
            r.YY1 += 2000
            r.YY2 += 2000
        Next
    End Function

    Public Sub ValidatePeriods(ByRef l As Collection)
        For Each tp As ArtBTimePeriod In l
            For Each jtp As ArtBTimePeriod In l
                If tp.StartMonth <> jtp.StartMonth Or tp.EndMonth <> jtp.EndMonth Then
                    If Not ((tp.StartMonth > jtp.EndMonth + 1) Or (tp.EndMonth + 1 < jtp.StartMonth)) Then
                        If jtp.StartMonth < tp.StartMonth Then tp.StartMonth = jtp.StartMonth
                        If jtp.EndMonth > tp.EndMonth Then tp.EndMonth = jtp.StartMonth
                        l.Remove(jtp.Descr)
                    End If
                End If
            Next
        Next
    End Sub

    Public Function OrderDescr(ByRef q As Object, ByVal ObjectType As Integer, ByRef s As String) As Integer
        OrderDescr = ArtBErrors.EmptyObject
        s = ""
        If ObjectType = 5 Then
            Try
                Dim dt As DateTime = q.ORDER_DATETIME
                s &= "[" & Format(dt, ARTB_DATETIME_REPORT_FORMATSTR) & "]: "
            Catch ex As Exception
                Debug.Print(ex.ToString())
            End Try
        End If
        Try
            If ObjectType = 4 Or ObjectType = 5 Then
                Dim DeskId As Integer = GetDeskId(q.FOR_DESK_TRADER_ID)
                Dim AccounId As Integer = 0
                Dim AccountName As String = ""
                GetAccountInfo(DeskId, AccounId, AccountName)
                If q.BROKER_INVISIBLE = False Then s &= AccountName & " "
            End If
            If ObjectType <> 5 Then
                If q.ORDER_BS = "B" Then
                    s = s & "Buy "
                ElseIf q.ORDER_BS = "S" Then
                    s = s & "Sell "
                End If
                s = s & GetOrderTypeDescr(q.ORDER_TYPE) & " "
            End If
            Dim Route As ROUTE_CLASS = Nothing
            Dim Route2 As ROUTE_CLASS = Nothing
            Dim VesselClass As VESSEL_CLASS_CLASS = Nothing
            Dim VesselClass2 As VESSEL_CLASS_CLASS = Nothing
            Route = GetViewClass(ROUTES, q.ROUTE_ID.ToString())
            If IsNothing(Route) Then Exit Function
            VesselClass = GetViewClass(VESSEL_CLASSES, Route.VESSEL_CLASS_ID.ToString())
            If IsNothing(VesselClass) Then Exit Function
            If q.ORDER_TYPE = OrderTypes.RatioSpread _
               Or q.ORDER_TYPE = OrderTypes.PriceSpread _
               Or q.ORDER_TYPE = OrderTypes.MarketSpread Then
                Route2 = GetViewClass(ROUTES, q.ROUTE_ID2.ToString())
                If IsNothing(Route2) Then Exit Function
                VesselClass2 = GetViewClass(VESSEL_CLASSES, Route2.VESSEL_CLASS_ID.ToString())
                If IsNothing(VesselClass2) Then Exit Function
            End If

            If ObjectType > 0 And ObjectType <> 5 Then
                s = s & VesselClass.VESSEL_CLASS & " - " & Route.ROUTE_SHORT & " "
                If q.ORDER_TYPE = OrderTypes.RatioSpread _
                   Or q.ORDER_TYPE = OrderTypes.PriceSpread _
                   Or q.ORDER_TYPE = OrderTypes.MarketSpread Then
                    s = s & " / " & VesselClass2.VESSEL_CLASS & " - " & Route2.ROUTE_SHORT & " "
                End If
            ElseIf ObjectType = 0 Then
                s = s & q.ROUTE_ID & "-" & q.ROUTEID2 & ", "
            End If

            If ObjectType <> 5 Then
                s = s & q.SHORTDES

                If q.ORDER_TYPE = OrderTypes.CalendarSpread Or q.ORDER_TYPE = OrderTypes.MarketSpread Then
                    s &= "/" & q.SHORTDES2
                End If
                s &= " "
            End If

            Dim QuoteStr As String = ""
            Dim QuoteStr2 As String = ""
            Dim QTypeStr As String = ""
            Dim QType2Str As String = ""

            If Not IsNothing(Route) Then
                If q.ORDER_TYPE <> OrderTypes.RatioSpread Then
                    Dim QuoteTypeClass As QUOTE_TYPE_CLASS = GetViewClass(QUOTE_TYPES, Route.QUOTE_TYPE.ToString())
                    Dim QuoteTypeClass2 As QUOTE_TYPE_CLASS = Nothing
                    If Not IsNothing(Route2) Then QuoteTypeClass2 = GetViewClass(QUOTE_TYPES, Route2.QUOTE_TYPE.ToString())
                    If Not IsNothing(QuoteTypeClass) Then
                        QuoteStr = QuoteTypeClass.QUOTE_TYPE_DES
                        QuoteStr2 = QuoteStr
                    End If
                    If Not IsNothing(QuoteTypeClass2) Then
                        QuoteStr2 = QuoteTypeClass2.QUOTE_TYPE_DES
                    End If
                End If

                Dim QTypeClass As QUANTITY_TYPE_CLASS = GetViewClass(QUANTITY_TYPES, Route.QUANTITY_TYPE.ToString())
                Dim QTypeClass2 As QUANTITY_TYPE_CLASS = Nothing
                If Not IsNothing(Route2) Then QTypeClass2 = GetViewClass(QUANTITY_TYPES, Route2.QUANTITY_TYPE.ToString())
                If Not IsNothing(QTypeClass) Then
                    QTypeStr = QTypeClass.QUANTITY_TYPE_DES
                    QType2Str = QTypeStr
                End If
                If Not IsNothing(QTypeClass2) Then
                    QType2Str = QTypeClass2.QUANTITY_TYPE_DES
                End If

            End If

            Dim d As Double = q.PRICE_INDICATED
            Dim PriceStr As String = ""
            If Math.Abs(d) > 1.0E+19 Then
                PriceStr = "Spread Basis "
            ElseIf q.ORDER_TYPE = OrderTypes.RatioSpread Then
                PriceStr = "@ " & d.ToString(ARTB_RATIOPRICE_FORMATSTR, ArtBRatioPriceInfo) & " "
            ElseIf Route.SETTLEMENT_TICK >= 1 Then
                PriceStr = "@ " & d.ToString(ARTB_INT_FORMATSTR, ArtBIntNumberInfo) & " " & QuoteStr & " "
            Else
                PriceStr = "@ " & d.ToString(FormatPriceString(Route.SETTLEMENT_TICK)) & " " & QuoteStr & " "
            End If
            Dim PriceType As String = "Indicative"

            If q.PRICE_TYPE = "F" Then PriceType = "Fixed Price"

            If q.PRICE_TRY_BETTER = True Then PriceType = "Can Do Better "

            If ObjectType = 5 Then
                PriceType = ""
                s &= PriceStr
            End If

            If PriceType <> "" Then PriceStr &= " " & PriceType & " "
            's = s & vbNewLine

            Dim bFull As Boolean = False
            Select Case q.DAY_QUALIFIER
                Case OrderDayQualifier.NotInDays
                    s = s & Format(q.ORDER_QUANTITY, "#,##0") & " " & QTypeStr & " "
                Case OrderDayQualifier.Full
                    s = s & "Full Contract "
                    bFull = True
                Case OrderDayQualifier.Half
                    s = s & "Half Contract "
                Case OrderDayQualifier.DPM
                    s = s & Format(q.ORDER_QUANTITY, "0") & " dpm "
                Case OrderDayQualifier.ContractDays
                    s = s & Format(q.ORDER_QUANTITY, "0") & " Contract Days "
            End Select

            If q.ORDER_TYPE = OrderTypes.RatioSpread Or q.ORDER_TYPE = OrderTypes.PriceSpread Or q.ORDER_TYPE = OrderTypes.CalendarSpread Or q.ORDER_TYPE = OrderTypes.MarketSpread Then
                bFull = False
                s = s & "/ "
                Select Case q.DAY_QUALIFIER2
                    Case OrderDayQualifier.NotInDays
                        s = s & Format(q.ORDER_QUANTITY2, "#,##0") & " " & QType2Str & " "
                    Case OrderDayQualifier.Full
                        s = s & "Full Contract "
                    Case OrderDayQualifier.Half
                        s = s & "Half Contract "
                    Case OrderDayQualifier.DPM
                        s = s & Format(q.ORDER_QUANTITY2, "0") & " dpm "
                    Case OrderDayQualifier.ContractDays
                        s = s & Format(q.ORDER_QUANTITY2, "0") & " Contract Days "
                End Select
            End If

            If ObjectType <> 5 Then
                Select Case q.FLEXIBLE_QUANTITY
                    Case OrderFlexQuantinty.Fixed
                        If bFull Then
                            s = s & "or 30 dpm "
                        Else
                            s = s & "Fixed Qty "
                        End If
                    Case OrderFlexQuantinty.CanDoLess
                        s = s & "Can Do Less "
                    Case OrderFlexQuantinty.CanDoMore
                        s = s & "Can Do More "
                    Case OrderFlexQuantinty.CanDoMoreOrLess
                        s = s & "Can Do More or Less "
                    Case OrderFlexQuantinty.StrictFull
                        s = s & "Strict "
                    Case OrderFlexQuantinty.Bucket
                        s = s & "Bucket Order "
                End Select
            End If

            If ObjectType <> 5 Then
                s &= PriceStr
            Else
                Return ArtBErrors.Success
            End If
            s = s & " in "
            Dim soe As String = q.ORDER_EXCHANGES
            Dim XID = 1
            For Each e As EXCHANGE_CLASS In EXCHANGES
                Try
                    Dim j As Integer = InStr(soe, "_")
                    If (j > 0) Then
                        soe = soe.Substring(j)
                        If (j > 1) Then s = s & e.EXCHANGE_NAME_SHORT & ", "
                    End If
                Catch ex As Exception
                End Try
                XID = XID + 1
            Next
            Select Case q.ORDER_GOOD_TILL
                Case OrderGoodTill.Day
                    s = s & "Day Order"
                Case OrderGoodTill.GTC
                    s = s & "GTC"
                Case OrderGoodTill.Limit
                    s = s & "Good for " & q.ORDER_TIME_LIMIT & " mins"
            End Select

            If q.PNC_ORDER Then s = s & ", PNC"

            OrderDescr = ArtBErrors.Success
        Catch ex As Exception
            Debug.Print(ex.ToString)
            OrderDescr = ArtBErrors.DescriptionError
        End Try
    End Function

    Public Function TradeDescr(ByRef q As TRADES_FFA_CLASS, _
                               ByVal a_Mode As Integer, _
                               ByVal a_Side As Integer, _
                               ByVal ObjectType As Integer, _
                               ByRef s As String) As Integer
        TradeDescr = ArtBErrors.EmptyObject
        s = ""
        Dim TradeBS As String
        Dim TradeID As Integer
        Dim o1 As ORDERS_FFA_CLASS
        If a_Side = 1 Then
            TradeBS = q.TRADE_BS1
        Else
            TradeBS = q.TRADE_BS2
        End If
        If a_Mode = 1 Then
            If TradeBS = "B" Then
                s = s & "Buy "
            ElseIf TradeBS = "S" Then
                s = s & "Sell "
            End If
        ElseIf a_Mode = 2 Then
            s = s & "CONTRACT:" & vbTab
        End If

        Dim Route As ROUTE_CLASS = GetViewClass(ROUTES, q.ROUTE_ID.ToString())
        Dim Route2 As ROUTE_CLASS = Nothing
        If Route Is Nothing Then Exit Function
        Dim VesselClass As VESSEL_CLASS_CLASS = GetViewClass(VESSEL_CLASSES, Route.VESSEL_CLASS_ID.ToString())
        Dim VesselClass2 As VESSEL_CLASS_CLASS = Nothing
        If VesselClass Is Nothing Then Exit Function

        If a_Mode = 3 Then
            s &= Route.ROUTE_SHORT & " "
        Else
            s = s & VesselClass.VESSEL_CLASS & " - " & Route.ROUTE_SHORT & " "
        End If
        o1 = GetViewClass(ORDERS_FFAS, q.ORDER_ID1.ToString())
        TradeID = q.TRADE_ID
        If IsNothing(o1) Then Exit Function
        If o1.ORDER_TYPE = OrderTypes.RatioSpread _
           Or o1.ORDER_TYPE = OrderTypes.PriceSpread _
           Or o1.ORDER_TYPE = OrderTypes.MarketSpread Then
            Route2 = GetViewClass(ROUTES, o1.ROUTE_ID2.ToString())
            If IsNothing(Route2) Then Exit Function
            VesselClass2 = GetViewClass(VESSEL_CLASSES, Route2.VESSEL_CLASS_ID.ToString())
            If IsNothing(VesselClass2) Then Exit Function
            If a_Mode = 3 Then
                s &= "/" & Route2.ROUTE_SHORT & " "
            Else
                s = s & "/ " & VesselClass2.VESSEL_CLASS & " - " & Route2.ROUTE_SHORT & " "
            End If
        End If

        If q.TRADE_TYPE = OrderTypes.CalendarSpread Or q.TRADE_TYPE = OrderTypes.MarketSpread Then
            s = s & o1.SHORTDES & "/" & o1.SHORTDES2 & " "
        Else
            s = s & q.SHORTDES & " "
        End If

        Dim QuoteStr As String = ""
        Dim QuoteStr2 As String = ""
        Dim QTypeStr As String = ""
        Dim QType2Str As String = ""

        If Not IsNothing(Route) Then
            If q.TRADE_TYPE <> OrderTypes.RatioSpread Then
                Dim QuoteTypeClass As QUOTE_TYPE_CLASS = GetViewClass(QUOTE_TYPES, Route.QUOTE_TYPE.ToString())
                Dim QuoteTypeClass2 As QUOTE_TYPE_CLASS = Nothing
                If Not IsNothing(Route2) Then QuoteTypeClass2 = GetViewClass(QUOTE_TYPES, Route2.QUOTE_TYPE.ToString())
                If Not IsNothing(QuoteTypeClass) Then
                    QuoteStr = QuoteTypeClass.QUOTE_TYPE_DES & " "
                    QuoteStr2 = QuoteStr
                End If
                If Not IsNothing(QuoteTypeClass2) Then
                    QuoteStr2 = QuoteTypeClass2.QUOTE_TYPE_DES & " "
                End If
            End If

            Dim QTypeClass As QUANTITY_TYPE_CLASS = GetViewClass(QUANTITY_TYPES, Route.QUANTITY_TYPE.ToString())
            Dim QTypeClass2 As QUANTITY_TYPE_CLASS = Nothing
            If Not IsNothing(Route2) Then QTypeClass2 = GetViewClass(QUANTITY_TYPES, Route2.QUANTITY_TYPE.ToString())
            If Not IsNothing(QTypeClass) Then
                QTypeStr = QTypeClass.QUANTITY_TYPE_DES
                QType2Str = QTypeStr
            End If
            If Not IsNothing(QTypeClass2) Then
                QType2Str = QTypeClass2.QUANTITY_TYPE_DES
            End If

        End If
        If a_Mode = 2 Then s = s & vbNewLine & "QUANTITY:" & vbTab
        Dim d As Double = q.PRICE_TRADED
        Dim PriceStr As String = ""
        If q.TRADE_TYPE = OrderTypes.RatioSpread Then
            PriceStr = d.ToString(ARTB_RATIOPRICE_FORMATSTR, ArtBRatioPriceInfo)
        ElseIf Route.SETTLEMENT_TICK >= 1 Then
            PriceStr = d.ToString(ARTB_INT_FORMATSTR, ArtBIntNumberInfo)
        Else
            PriceStr = d.ToString(FormatPriceString(Route.SETTLEMENT_TICK))
        End If

        Dim q1 As Double = q.QUANTITY

        If a_Mode = 3 Then
            If q.DESK_TRADER_ID1 = SystemDeskTraderId Or q.DESK_TRADER_ID2 = SystemDeskTraderId Then
                s = s & " trades " & PriceStr & " away"
                TradeDescr = ArtBErrors.Success
                Exit Function
            Else
                s = s & " trades " & PriceStr & " for "
                q1 = ReportedSpreadTradeQ(q)
                If q1 <= 0 Then
                    s = ""
                    Return ArtBErrors.TradeNotToBeReported
                End If
            End If
        End If

        Select Case q.DAY_QUALIFIER
            Case OrderDayQualifier.NotInDays
                s = s & Format(q1, "#,##0") & " "
                If a_Mode <> 3 And Len(QTypeStr) > 0 Then s &= QTypeStr & " "
            Case OrderDayQualifier.Full
                s = s & "Full "
            Case OrderDayQualifier.Half
                s = s & "Half "
            Case OrderDayQualifier.DPM
                s = s & Format(q1, "0") & " dpm "
            Case OrderDayQualifier.ContractDays
                s = s & Format(q1, "0") & " Contract Days "
        End Select
        If o1.ORDER_TYPE <> OrderTypes.FFA Then
            s = s & "/ "
            Select Case o1.DAY_QUALIFIER2
                Case OrderDayQualifier.NotInDays
                    s = s & Format(q1 * o1.ORDER_QUANTITY2 / o1.ORDER_QUANTITY, "#,##0") & " "
                    If a_Mode <> 3 And Len(QType2Str) > 0 Then s &= QType2Str & " "
                Case OrderDayQualifier.Full
                    s = s & "Full "
                Case OrderDayQualifier.Half
                    s = s & "Half "
                Case OrderDayQualifier.DPM
                    s = s & Format(q1 * o1.ORDER_QUANTITY2 / o1.ORDER_QUANTITY, "0") & " dpm "
            End Select
        End If

        Select Case a_Mode
            Case 0, 1
                s = s & "@ " & PriceStr & " " & QuoteStr & "in "
            Case 2
                s = s & vbNewLine & "PRICE:" & vbTab & vbTab & PriceStr & " " & QuoteStr & vbNewLine & "EXCHANGE:" & vbTab
        End Select

        If q.IS_SYNTHETIC = False Or q.TRADE_TYPE = OrderTypes.FFA Then
            s = s & GetExchangeName(q.EXCHANGE_ID)
        Else
            Dim LegTradeList = From t In TRADES_FFAS Where t.SPREAD_TRADE_ID1 = TradeID Or t.SPREAD_TRADE_ID2 = TradeID Select t
            Dim exch1 As String = ""

            For Each t As TRADES_FFA_CLASS In LegTradeList
                Dim exn As String = GetExchangeName(t.EXCHANGE_ID)
                If InStr(exch1, exn) <= 0 Then
                    If Len(exch1) > 0 Then exch1 = exch1 & ","
                    exch1 = exch1 & exn
                End If
            Next
            s = s & exch1
        End If

        If a_Mode = 1 Then
            If q.PNC Then s = s & ", PNC"
        End If

        If a_Mode = 3 Then
            If q.TRADE_TYPE = OrderTypes.FFA And _
               (NullInt2Int(q.SPREAD_TRADE_ID1) <> 0 Or NullInt2Int(q.SPREAD_TRADE_ID2) <> 0) Then
                s &= " on a Spread Basis"
            End If
        End If
        TradeDescr = ArtBErrors.Success
    End Function

    Public Function GetTraderName(ByVal a_TraderId As Integer, _
                                  Optional ByVal bContactName As Boolean = False, _
                                  Optional ByRef UserInfo As TraderInfoClass = Nothing) As String
        GetTraderName = ""
        Dim TraderClass As DESK_TRADER_CLASS = GetViewClass(DESK_TRADERS, a_TraderId.ToString())
        If TraderClass Is Nothing Then Exit Function
        If bContactName = True Then
            Dim ContactClass As CONTACT_CLASS = GetViewClass(CONTACTS, TraderClass.CONTACT_ID.ToString())
            If ContactClass Is Nothing Then Exit Function
            GetTraderName = ContactClass.FIRSTNAME & " " & ContactClass.LASTNAME
            Exit Function
        End If
        Dim DeskClass As ACCOUNT_DESK_CLASS = GetViewClass(ACCOUNT_DESKS, TraderClass.ACCOUNT_DESK_ID.ToString())
        If DeskClass Is Nothing Then Exit Function
        Dim AccountClass As ACCOUNT_CLASS = GetViewClass(ACCOUNTS, DeskClass.ACCOUNT_ID.ToString())
        If AccountClass Is Nothing Then Exit Function

        If Not IsNothing(UserInfo) Then
            If UserInfo.AccountID = DeskClass.ACCOUNT_ID Then
                GetTraderName = AccountClass.SHORT_NAME & ", " & DeskClass.DESK_DESCR
            Else
                GetTraderName = AccountClass.SHORT_NAME
            End If
        Else
            GetTraderName = AccountClass.SHORT_NAME
        End If

    End Function

    Public Function GetTraderNameWithRules(ByRef TraderInfo As TraderInfoClass, _
                                           ByRef UserInfo As TraderInfoClass, _
                                           ByRef OrderClass As Object, _
                                           Optional ByVal bTradeMode As Boolean = False) As String
        GetTraderNameWithRules = ""
        If IsNothing(OrderClass) Then Exit Function
        If UserInfo.IsBroker And bTradeMode = False Then
            If Not IsNothing(OrderClass.BROKER_INVISIBLE) Then
                If OrderClass.BROKER_INVISIBLE Then Return ""
            End If
        End If

        Dim ForTraderID As Integer = OrderClass.FOR_DESK_TRADER_ID
        Dim TraderClass As DESK_TRADER_CLASS = GetViewClass(DESK_TRADERS, ForTraderID.ToString())
        If TraderClass Is Nothing Then Exit Function
        Dim ContactClass As CONTACT_CLASS = GetViewClass(CONTACTS, TraderClass.CONTACT_ID.ToString())
        If ContactClass Is Nothing Then Exit Function
        'GetTraderNameWithRules = ContactClass.LASTNAME & " " & ContactClass.FIRSTNAME
        Dim DeskClass As ACCOUNT_DESK_CLASS = GetViewClass(ACCOUNT_DESKS, TraderClass.ACCOUNT_DESK_ID.ToString())
        If DeskClass Is Nothing Then Exit Function
        Dim AccountClass As ACCOUNT_CLASS = GetViewClass(ACCOUNTS, DeskClass.ACCOUNT_ID.ToString())
        If AccountClass Is Nothing Then Exit Function

        Dim ViewerAccount As Integer
        Dim VCStr As String, TCStr As String, RouteStr As String, VCId As Integer
        GetRouteInfo(OrderClass.ROUTE_ID, TCStr, VCStr, VCId)
        If IsNothing(TraderInfo) Then
            GetTraderNameWithRules = AccountClass.SHORT_NAME
            Exit Function
        End If

        If TraderInfo.AccountID = DeskClass.ACCOUNT_ID Then
            GetTraderNameWithRules = AccountClass.SHORT_NAME & ", " & DeskClass.DESK_DESCR
        Else
            GetTraderNameWithRules = AccountClass.SHORT_NAME
        End If
        'If OrderClass.FOR_DESK_TRADER_ID <> OrderClass.DESK_TRADER_ID Then
        '    Dim s1 As String = "", s2 As String = "", s3 As String = ""
        '    Dim bacid As Integer = 0, bdid As Integer = 0
        '    GetAllTraderNames(OrderClass.DESK_TRADER_ID, s1, s2, s3, bacid, bdid)

        '    If TraderInfo.IsBroker Then
        '        GetTraderNameWithRules = s1 & "->" & AccountClass.SHORT_NAME & ", " & DeskClass.DESK_DESCR
        '    Else
        '        If TraderInfo.AccountID = DeskClass.ACCOUNT_ID Then
        '            If UserInfo.IsBroker Then
        '                GetTraderNameWithRules = s1 & "->" & GetTraderNameWithRules
        '            Else
        '                GetTraderNameWithRules = GetTraderNameWithRules & "->" & s1
        '            End If
        '        End If
        '    End If
        'End If
        'Check limits only for traders
        If TraderInfo.IsSystemAdmin Then Exit Function
        If OrderClass.PRICE_TYPE = "I" Then
            Dim UserTraderID As Integer = OrderClass.DESK_TRADER_ID
            Dim UserTraderClass As DESK_TRADER_CLASS = GetViewClass(DESK_TRADERS, UserTraderID.ToString())
            If UserTraderClass Is Nothing Then Exit Function
            Dim UserAccountClass As ACCOUNT_CLASS = GetViewClass(ACCOUNTS, UserTraderClass.ACCOUNT_ID.ToString())
            If UserAccountClass Is Nothing Then Exit Function
            If TraderInfo.IsTrader Or UserAccountClass.ACCOUNT_ID <> UserInfo.AccountID Then
                GetTraderNameWithRules = UserAccountClass.SHORT_NAME & " CLIENT"
            End If
            Exit Function
        End If
        If TraderInfo.DeskID = DeskClass.ACCOUNT_DESK_ID Then Exit Function

        If TraderInfo.IsBroker Then
            For Each tc As DESK_TRADE_CLASS_CLASS In DeskClass.TRADE_CLASSES
                If tc.TRADE_CLASS_SHORT <> TCStr Then Continue For
                Dim EndBrokerID As Integer = GetMainBrokerForAffiliate(TCStr, tc.BROKER_ID)
                Dim EBAc As ACCOUNT_CLASS = GetViewClass(ACCOUNTS, EndBrokerID.ToString())
                If TraderInfo.IsAffiliateBroker Then
                    If AccountClass.BROKER_ID <> TraderInfo.AccountID Then
                        If bTradeMode And Not IsNothing(EBAc) Then
                            GetTraderNameWithRules = EBAc.SHORT_NAME
                        Else
                            GetTraderNameWithRules = ""
                        End If

                        Exit For
                    End If
                Else
                    If tc.BROKER_ID <> TraderInfo.AccountID And _
                       EndBrokerID <> TraderInfo.AccountID Then
                        If bTradeMode And Not IsNothing(EBAc) Then
                            GetTraderNameWithRules = EBAc.SHORT_NAME
                        Else
                            GetTraderNameWithRules = ""
                        End If
                        Exit For
                    End If
                End If
            Next
            Exit Function
        End If
        Dim DeskID1 As Integer
        Dim DeskID2 As Integer
        Dim MainDeskID1 As Integer
        Dim MainDeskID2 As Integer

        If TraderInfo.DeskID = DeskClass.ACCOUNT_DESK_ID Then Exit Function
        If TraderInfo.AccountID = DeskClass.ACCOUNT_ID Then
            'Intra account Transaction
            DeskID1 = TraderInfo.DeskID
            DeskID2 = DeskClass.ACCOUNT_DESK_ID
            MainDeskID1 = DeskID1
            MainDeskID2 = DeskID1
        Else
            DeskID1 = TraderInfo.DeskID
            DeskID2 = DeskClass.ACCOUNT_DESK_ID
            MainDeskID1 = GetMainDesk(TraderInfo.AccountID)
            MainDeskID2 = GetMainDesk(DeskClass.ACCOUNT_ID)
        End If
        Dim ViewCPL As COUNTERPARTY_LIMIT_CLASS = GetCPL(Nothing, DeskID1, MainDeskID2)
        If IsNothing(ViewCPL) Then Exit Function
        Dim CPL As COUNTERPARTY_LIMIT_CLASS = GetCPL(Nothing, DeskID2, MainDeskID1)
        If IsNothing(CPL) Then Exit Function

        Dim bOTC = False
        Dim bCleared = False
        For Each x As ORDERS_FFA_EXCHANGE_CLASS In OrderClass.EXCHANGES
            If x.EXCHANGE_ID = Exch.OTC Then
                bOTC = True
            Else
                bCleared = True
            End If
        Next

        'If IsNothing(ViewCPL) Then
        '    If bOTC = True And CPL.OTC_SHOW_NAME <> ShowName.Always Then
        '        GetTraderNameWithRules = ""
        '    End If
        '    If bCleared = True And CPL.CLEARED_SHOW_NAME <> ShowName.Always Then
        '        GetTraderNameWithRules = ""
        '    End If
        '    Exit Function
        'End If
        If bOTC = True And _
            (CPL.OTC_SHOW_NAME = ShowName.Never Or _
             (CPL.OTC_SHOW_NAME = ShowName.Mutual And ViewCPL.OTC_SHOW_NAME = ShowName.Never)) Then
            GetTraderNameWithRules = ""
        End If
        If bCleared = True And _
            (CPL.CLEARED_SHOW_NAME = ShowName.Never Or _
             (CPL.CLEARED_SHOW_NAME = ShowName.Mutual And ViewCPL.CLEARED_SHOW_NAME = ShowName.Never)) Then
            GetTraderNameWithRules = ""
        End If

    End Function

    Public Function GetMainBrokerForAffiliate(ByVal TCStr As String, ByVal BrokerID As Integer) As Integer
        GetMainBrokerForAffiliate = BrokerID
        Dim BrokerAccount As ACCOUNT_CLASS = GetViewClass(ACCOUNTS, BrokerID.ToString)
        If BrokerAccount Is Nothing Then Return 0
        If BrokerAccount.ACCOUNT_TYPE_ID = 2 Then Return BrokerID
        Dim MainDesk As ACCOUNT_DESK = Nothing

        For Each d As ACCOUNT_DESK_CLASS In BrokerAccount.DESKS
            If d.DESK_QUALIFIER = 0 Then
                MainDesk = d
                Exit For
            End If
        Next
        If MainDesk Is Nothing Then Exit Function

        For Each TC As DESK_TRADE_CLASS_CLASS In MainDesk.DESK_TRADE_CLASSes
            If TC.TRADE_CLASS_SHORT <> TCStr Then Continue For
            Return TC.BROKER_ID
        Next
    End Function

    Public Function GetMainBrokerForAffiliate(ByRef gdb As DB_ARTB_NETDataContext, ByVal TCStr As String, ByVal BrokerID As Integer) As Integer
        GetMainBrokerForAffiliate = BrokerID

        Try
            Dim q1 = (From d In gdb.ACCOUNT_DESKs _
                       Join t In gdb.DESK_TRADE_CLASSes On d.ACCOUNT_DESK_ID Equals t.ACCOUNT_DESK_ID _
                       Where t.TRADE_CLASS_SHORT = TCStr And d.ACCOUNT_ID = BrokerID And d.DESK_QUALIFIER = 0 _
                       Select t.BROKER_ID).ToList.First()
            GetMainBrokerForAffiliate = BrokerID
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Function

    Public Function GetMainDesk(ByVal a_AccountId As Integer) As Integer
        GetMainDesk = 0
        Try
            Dim AC As ACCOUNT_CLASS = GetAccountClass(a_AccountId)

            If IsNothing(AC) Then Exit Function
            If IsNothing(AC.DESKS) Then Exit Function
            For Each d As ACCOUNT_DESK_CLASS In AC.DESKS
                If d.DESK_QUALIFIER = 0 Then
                    GetMainDesk = d.ACCOUNT_DESK_ID
                    Exit Function
                End If
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Function

    Public Function GetMainDeskFromDB(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_AccountId As Integer) As Integer
        GetMainDeskFromDB = 0
        Dim l = From q In gdb.ACCOUNT_DESKs Where q.ACCOUNT_ID = a_AccountId And q.DESK_QUALIFIER = 0
        If IsNothing(l) Then Exit Function
        For Each q In l
            GetMainDeskFromDB = q.ACCOUNT_DESK_ID
            Exit Function
        Next
    End Function

    Public Sub GetAllTraderNames(ByVal a_TraderId As Integer, _
                                 ByRef AccountShort As String, _
                                 ByRef DeskName As String, _
                                 ByRef TraderName As String, _
                                 ByRef a_AccountId As Integer, _
                                 ByRef a_Desk_Id As Integer)
        AccountShort = ""
        DeskName = ""
        TraderName = ""
        Dim TraderClass As DESK_TRADER_CLASS = GetViewClass(DESK_TRADERS, a_TraderId.ToString())
        If TraderClass Is Nothing Then Exit Sub
        Dim DeskClass As ACCOUNT_DESK_CLASS = GetViewClass(ACCOUNT_DESKS, TraderClass.ACCOUNT_DESK_ID.ToString())
        If DeskClass Is Nothing Then Exit Sub
        Dim AccountClass As ACCOUNT_CLASS = GetViewClass(ACCOUNTS, DeskClass.ACCOUNT_ID.ToString())
        If AccountClass Is Nothing Then Exit Sub
        AccountShort = AccountClass.SHORT_NAME
        DeskName = DeskClass.DESK_DESCR
        Dim ContactClass As CONTACT_CLASS = GetViewClass(CONTACTS, TraderClass.CONTACT_ID.ToString())
        If AccountClass Is Nothing Then Exit Sub
        TraderName = ContactClass.FIRSTNAME & " " & ContactClass.LASTNAME
        a_Desk_Id = DeskClass.ACCOUNT_DESK_ID
        a_AccountId = AccountClass.ACCOUNT_ID
    End Sub

    Public Function GetExchangeName(ByVal a_ExchangeID As Nullable(Of Integer)) As String
        GetExchangeName = ""
        If IsNothing(a_ExchangeID) Then Exit Function
        Dim e As EXCHANGE_CLASS = GetViewClass(EXCHANGES, a_ExchangeID.ToString())
        If Not IsNothing(e) Then
            GetExchangeName = e.EXCHANGE_NAME_SHORT
        End If
    End Function

    Public Sub GetVesselClassRouteName(ByVal a_RouteID As Integer, _
                                       ByRef VCStr As String, ByRef RouteStr As String, _
                                       ByRef VCID As Integer, _
                                       Optional ByRef TradeClassShort As String = "")
        Dim e As ROUTE_CLASS = GetViewClass(ROUTES, a_RouteID.ToString())
        VCStr = ""
        RouteStr = ""
        VCID = 0
        If IsNothing(e) Then Exit Sub
        RouteStr = e.ROUTE_SHORT
        VCID = e.VESSEL_CLASS_ID
        Dim q As VESSEL_CLASS_CLASS = GetViewClass(VESSEL_CLASSES, e.VESSEL_CLASS_ID.ToString())
        If IsNothing(q) Then Exit Sub
        VCStr = q.VESSEL_CLASS
        TradeClassShort = q.DRYWET
    End Sub

    Public Function OrderExpireInDB(ByRef gdb As DB_ARTB_NETDataContext, _
                                    ByVal a_OrderId As Integer, _
                                    ByRef OrderStr As String, _
                                    ByRef TradeStr As String, _
                                    ByRef AffectedRoutePeriods As List(Of String)) As Integer
        OrderStr = ""
        OrderExpireInDB = ArtBErrors.Success
        Dim oc As ORDERS_FFA
        Dim OrderClass As New ORDERS_FFA_CLASS
        Try
            oc = (From q In gdb.ORDERS_FFAs _
                            Where q.ORDER_ID = a_OrderId Select q).First()
            If OrderClass.GetFromObject(oc) <> ArtBErrors.Success Then Return ArtBErrors.Success

        Catch ex As Exception
            Exit Function
        End Try
        If IsNothing(OrderClass) Then Exit Function

        Select Case OrderClass.LIVE_STATUS
            Case "R"
                OrderClass.LIVE_STATUS = "D"
                'Dim counterPartyID As Integer = NullInt2Int(OrderClass.COUNTER_PARTY_ORDER_ID)
                'If counterPartyID > 0 Then
                '    OrderExpireInDB = SetOrderLiveStatus(counterPartyID, "D", OrderStr, tradestr)
                '    If OrderExpireInDB <> ArtBErrors.Success Then
                '        Exit Function
                '    End If
                'End If
            Case "A"
                OrderClass.LIVE_STATUS = "D"
                'If OrderClass.ORDER_QUALIFIER = "R" Then
                '    Dim previousOrderID As Integer = NullInt2Int(OrderClass.PREVIOUS_ORDER_ID)
                '    If previousOrderID > 0 Then
                '        OrderExpireInDB = SetOrderLiveStatus(previousOrderID, "A", OrderStr, TradeStr)
                '        If OrderExpireInDB <> ArtBErrors.Success Then
                '            Exit Function
                '        End If
                '    End If
                'End If
            Case "N"
                OrderClass.LIVE_STATUS = "D"
            Case Else
                Exit Function
        End Select
        Dim s As String
        Return UpdateDBFromNewOrder(gdb, OrderClass, OrderStr, TradeStr, AffectedRoutePeriods)
    End Function


    Public Function OrdersExpireInDB(ByRef gdb As DB_ARTB_NETDataContext, _
                                     ByRef OrderStr As String, _
                                     ByRef TradeStr As String, _
                                     Optional ByVal bRaiseEvents As Boolean = False) As Integer
        OrdersExpireInDB = ArtBErrors.Success
        OrderStr = ""
        TradeStr = ""
        Try
            Dim DN As DateTime = DateTime.UtcNow.Add(GlobalDateTimeDiff)
            Dim D As DateTime
            Dim OrderClass As New ORDERS_FFA_CLASS
            D = DateAdd(DateInterval.Day, -1, DN)
            Dim l = (From q In gdb.ORDERS_FFAs _
                                Where q.ORDER_GOOD_TILL = OrderGoodTill.Limit And _
                                      (q.LIVE_STATUS = "A" Or q.LIVE_STATUS = "N" Or q.LIVE_STATUS = "R") And _
                                      q.ORDER_DATETIME > D).ToList
            If IsNothing(l) Then Exit Function
            If l.Count <= 0 Then Exit Function
            Dim s As String = ""
            For Each q As ORDERS_FFA In l
                Dim xt As DateTime = DateAdd(DateInterval.Minute, q.ORDER_TIME_LIMIT, q.ORDER_DATETIME)
                If xt < DN Then
                    If OrderClass.GetFromObject(q) <> ArtBErrors.Success Then Return ArtBErrors.Success
                    Select Case OrderClass.LIVE_STATUS
                        Case "R"
                            OrderClass.LIVE_STATUS = "D"
                            'Dim counterPartyID As Integer = NullInt2Int(OrderClass.COUNTER_PARTY_ORDER_ID)
                            'If counterPartyID > 0 Then
                            '    OrderExpireInDB = SetOrderLiveStatus(counterPartyID, "D", OrderStr, tradestr)
                            '    If OrderExpireInDB <> ArtBErrors.Success Then
                            '        Exit Function
                            '    End If
                            'End If
                        Case "A"
                            OrderClass.LIVE_STATUS = "D"
                            'If OrderClass.ORDER_QUALIFIER = "R" Then
                            '    Dim previousOrderID As Integer = NullInt2Int(OrderClass.PREVIOUS_ORDER_ID)
                            '    If previousOrderID > 0 Then
                            '        OrderExpireInDB = SetOrderLiveStatus(previousOrderID, "A", OrderStr, TradeStr)
                            '        If OrderExpireInDB <> ArtBErrors.Success Then
                            '            Exit Function
                            '        End If
                            '    End If
                            'End If
                        Case "N"
                            OrderClass.LIVE_STATUS = "D"
                    End Select
                    OrdersExpireInDB = OrderClass.AppendToStr(s)
                    If OrdersExpireInDB <> ArtBErrors.Success Then Exit Function
                ElseIf bRaiseEvents Then
                    Dim ts As TimeSpan = xt - DN
                    RaiseEvent TimeLimitOrder(OrderClass.ORDER_ID, ts.TotalSeconds)
                End If
            Next
            If s.Length >= 1 Then
                OrdersExpireInDB = UpdateDBFromNewOrders(gdb, s, ArtBMessages.OrderFFAAmmend, OrderStr, TradeStr)
            End If
        Catch ex As Exception
            Debug.Print(ex.ToString)
            OrdersExpireInDB = ArtBErrors.DeleteFailed
        End Try

    End Function

    Public Function SleepAllInDB(ByRef gdb As DB_ARTB_NETDataContext, _
                                 ByRef OrderStr As String, _
                                 ByRef TradeStr As String) As Integer
        SleepAllInDB = ArtBErrors.Success
        OrderStr = ""
        TradeStr = ""
        Dim DN As Nullable(Of DateTime) = DateTime.UtcNow.Add(GlobalDateTimeDiff)
        Dim D As DateTime
        Dim OrderClass As New ORDERS_FFA_CLASS
        DN = GetServerDateTime()
        If IsNothing(DN) Then
            DN = DateTime.UtcNow.Add(GlobalDateTimeDiff)
        End If
        D = DN
        D = DateAndTime.DateValue(DN.Value.ToString)
        Dim l = (From q In gdb.ORDERS_FFAs _
                Join t In gdb.DESK_TRADERs On t.DESK_TRADER_ID Equals q.DESK_TRADER_ID _
                Join de In gdb.ACCOUNT_DESKs On de.ACCOUNT_DESK_ID Equals t.ACCOUNT_DESK_ID _
                            Where (q.ORDER_GOOD_TILL = OrderGoodTill.Limit Or q.ORDER_GOOD_TILL = OrderGoodTill.Day) And _
                                  (q.LIVE_STATUS = "A" Or q.LIVE_STATUS = "N" Or q.LIVE_STATUS = "R") And _
                                  q.ORDER_DATETIME > D
                Select q, de).ToList
        If IsNothing(l) Then Return ArtBErrors.Success
        If l.Count <= 0 Then Return ArtBErrors.Success

        Dim s As String = ""
        For Each x In l
            If OrderClass.GetFromObject(x.q) <> ArtBErrors.Success Then Return ArtBErrors.Success
            Select Case OrderClass.LIVE_STATUS
                Case "R"
                    OrderClass.LIVE_STATUS = "D"
                Case "A", "N"
                    If (OrderClass.ORDER_GOOD_TILL = OrderGoodTill.Limit) Or _
                        x.de.LOST_CONNECTION = 2 Then
                        OrderClass.LIVE_STATUS = "D"
                    Else
                        OrderClass.LIVE_STATUS = "S"
                    End If
            End Select

            SleepAllInDB = OrderClass.AppendToStr(s)
            If SleepAllInDB <> ArtBErrors.Success Then Exit Function
        Next
        If s.Length >= 1 Then
            SleepAllInDB = UpdateDBFromNewOrders(gdb, s, ArtBMessages.OrderFFAAmmend, OrderStr, TradeStr)
        End If
    End Function

    Public Function SetNewNegotiationOrderID(ByRef gdb As DB_ARTB_NETDataContext, _
                                             ByVal a_OrderId As Integer, _
                                             ByVal a_NegotiationOrderID As Integer, _
                                             ByRef s As String) As Integer
        SetNewNegotiationOrderID = ArtBErrors.Success
        Dim OrderClass = (From q In gdb.ORDERS_FFAs _
                            Where q.ORDER_ID = a_OrderId Select q).First()
        If IsNothing(OrderClass) Then Exit Function
        If OrderClass.COUNTER_PARTY_ORDER_ID = OrderClass.NEGOTIATION_ORDER_ID Then
            OrderClass.COUNTER_PARTY_ORDER_ID = a_NegotiationOrderID
        End If
        OrderClass.NEGOTIATION_ORDER_ID = a_NegotiationOrderID
        Try
            gdb.SubmitChanges()
        Catch e As Exception
            SetNewNegotiationOrderID = ArtBErrors.UpdateFailed
            Exit Function
        End Try
        Dim OC As New ORDERS_FFA_CLASS
        SetNewNegotiationOrderID = OC.GetFromObject(OrderClass)
        If SetNewNegotiationOrderID <> ArtBErrors.Success Then Exit Function
        SetNewNegotiationOrderID = OC.AppendToStr(s)
    End Function


    Public Function SetOrderLiveStatus(ByRef gdb As DB_ARTB_NETDataContext, _
                                       ByVal a_OrderId As Integer, _
                                       ByVal c As Char, _
                                       ByRef OrderStr As String, _
                                       ByRef TradeStr As String, _
                                       ByRef AffectedRoutePeriods As List(Of String), _
                                       Optional ByVal a_LockOrderID As Integer = 0, _
                                       Optional ByVal a_LockTraderId As Integer = 0) As Integer
        SetOrderLiveStatus = ArtBErrors.Success
        Dim OrderClass As ORDERS_FFA_CLASS
        Dim OC As ORDERS_FFA
        Try
            OC = (From q In gdb.ORDERS_FFAs _
                                Where q.ORDER_ID = a_OrderId Select q).First()
        Catch
            SetOrderLiveStatus = ArtBErrors.RecordNotFound
            Exit Function
        End Try
        OrderClass = New ORDERS_FFA_CLASS
        If OrderClass.GetFromObject(OC) <> ArtBErrors.Success Then Return ArtBErrors.RecordNotFound
        If IsNothing(OrderClass) Then
            SetOrderLiveStatus = ArtBErrors.RecordNotFound
            Exit Function
        End If

        Dim LockOrderID As Integer = NullInt2Int(OrderClass.LOCK_ORDER_ID)
        Dim LockTraderId As Integer = NullInt2Int(OrderClass.LOCK_DESK_TRADER_ID)

        If a_LockOrderID > 0 Then
            If c = "N" Then
                OrderClass.LOCKED_BY_ORDER_ID = a_LockOrderID
                OrderClass.LOCK_DESK_TRADER_ID = a_LockTraderId
            End If
        End If
        If a_LockTraderId > 0 Then
            If c = "N" Then
                OrderClass.LOCKED_BY_ORDER_ID = a_LockOrderID
                OrderClass.LOCK_DESK_TRADER_ID = a_LockTraderId
            End If
        End If
        If c <> "N" Then
            If OrderClass.LIVE_STATUS = c And LockOrderID <= 0 And LockTraderId <= 0 Then Exit Function
            OrderClass.LOCKED_BY_ORDER_ID = Nothing
            OrderClass.LOCK_DESK_TRADER_ID = Nothing
        End If

        OrderClass.LIVE_STATUS = c
        Return UpdateDBFromNewOrder(gdb, OrderClass, OrderStr, TradeStr, AffectedRoutePeriods)
    End Function

    Public Function SetFastOrderLiveStatus(ByRef gdb As DB_ARTB_NETDataContext, _
                                           ByVal a_OrderId As Integer, _
                                           ByVal c As Char, _
                                           ByRef OrderStr As String, _
                                           ByRef TradeStr As String, _
                                           ByRef AffectedRoutePeriods As List(Of String)) As Integer
        SetFastOrderLiveStatus = ArtBErrors.Success
        Dim OrderClass As ORDERS_FFA_CLASS
        Dim OC As ORDERS_FFA
        Try
            OC = (From q In gdb.ORDERS_FFAs _
                                Where q.ORDER_ID = a_OrderId Select q).First()
        Catch
            SetFastOrderLiveStatus = ArtBErrors.RecordNotFound
            Exit Function
        End Try
        OrderClass = New ORDERS_FFA_CLASS
        If OrderClass.GetFromObject(OC) <> ArtBErrors.Success Then Return ArtBErrors.RecordNotFound
        If IsNothing(OrderClass) Then
            SetFastOrderLiveStatus = ArtBErrors.RecordNotFound
            Exit Function
        End If

        OrderClass.LIVE_STATUS = c

        SetFastOrderLiveStatus = InsertUpdateOrder(gdb, OrderClass, False)
        If SetFastOrderLiveStatus <> ArtBErrors.Success Then Exit Function
        Return OrderClass.AppendToStr(OrderStr)
    End Function

    Public Function SetNegotiationOrderStatus(ByRef gdb As DB_ARTB_NETDataContext, _
                                              ByVal a_OrderId As Integer, _
                                              ByVal ActualQuantity As Integer, _
                                              ByRef FilterQ As Integer, _
                                              ByRef OrderStr As String, _
                                              ByRef TradeStr As String, _
                                              ByRef AffectedRoutePeriods As List(Of String), _
                                              Optional ByVal PExchangeID As Integer = 0, _
                                              Optional ByVal ToLiveStatus As String = "E", _
                                              Optional ByVal SpreadLegType As Integer = 0) As Integer
        SetNegotiationOrderStatus = ArtBErrors.Success
        Dim OrderClass As New ORDERS_FFA_CLASS
        If OrderClass.GetFromID(gdb, a_OrderId) <> ArtBErrors.Success Then
            OrderClass = Nothing
            Exit Function
        End If
        Dim PreviousLiveStatus As String = OrderClass.LIVE_STATUS
        If OrderClass.LIVE_STATUS = ToLiveStatus Then
            OrderClass = Nothing
            Exit Function
        End If
        If OrderClass.LIVE_STATUS <> "A" And OrderClass.LIVE_STATUS <> "N" Then
            SetNegotiationOrderStatus = ArtBErrors.NegotiationOnNonActiveOrder
            OrderClass = Nothing
            Exit Function
        End If
        Dim OQ As Integer = Int(GetActualQuantity(OrderClass))
        Dim OQ2 As Integer = Int(GetActualQuantity(OrderClass, 2))
        If OrderClass.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket And _
            ((ActualQuantity < OQ And SpreadLegType <> SpreadLegTypes.Order2) Or (ActualQuantity < OQ2 And SpreadLegType = SpreadLegTypes.Order2)) Then
            FilterQ = OQ - ActualQuantity
            If OrderClass.ORDER_TYPE = OrderTypes.FFA Or SpreadLegType <> SpreadLegTypes.Order2 Then
                OrderClass.SetQuantity(FilterQ)
            End If

            OrderClass.LIVE_STATUS = PreviousLiveStatus
            If OrderClass.SINGLE_EXCHANGE_EXECUTION And PExchangeID > 0 Then
                LimitExchangeStr(OrderClass, PExchangeID)
            End If
        Else
            OrderClass.LIVE_STATUS = ToLiveStatus
        End If
        Return UpdateDBFromNewOrder(gdb, OrderClass, OrderStr, TradeStr, AffectedRoutePeriods, False)
    End Function

    Public Function MarketDepth(ByRef order As Object, ByRef c As Collection, ByRef s As String) As Collection
        s = ""
        Return Nothing
        Dim nc As New Collection
        Dim nmoc As New MATCHING_ORDERS_CLASS
        nmoc.Exchanges = New List(Of Integer)
        Dim uDeskID As Integer = GetDeskId(order.FOR_DESK_TRADER_ID)
        For Each o In c
            Dim oDeskId As Integer = GetDeskId(o.FOR_DESK_TRADER_ID)
            If MatchingOrders(Nothing, order, o, nmoc.Exchanges) And uDeskID <> oDeskId And NullInt2Int(o.SPREAD_ORDER_ID) = 0 Then
                nmoc.OrderClass = o
                nmoc.ActualQuantity = 0
                nc.Add(nmoc)
                nmoc = New MATCHING_ORDERS_CLASS
                nmoc.Exchanges = New List(Of Integer)


            End If
        Next
        MarketDepth = MIPOrders(order, nc, GetDeskExchangeRankings(uDeskID, order))
        s = ""
        If IsNothing(MarketDepth) Then Exit Function
        For Each oc As MATCHING_ORDERS_CLASS In MarketDepth
            Dim o As Object = oc.OrderClass
            s = s & Format(o.PRICE_INDICATED, "0.00") & "x" & oc.ActualQuantity.ToString() & " {Order Id = " & o.ORDER_ID.ToString() & "} In "
            For Each i As Integer In oc.Exchanges
                s = s & GetExchangeName(i) & ","
            Next
            s = s & vbNewLine
            oc = Nothing
        Next
        For Each noc In nc
            noc = Nothing
        Next
        MarketDepth = Nothing
        nc = Nothing
        nmoc = Nothing
    End Function

    Public Function TargetPrice(ByRef order As Object) As Double
        Select Case order.ORDER_BS
            Case "B"
                Select Case order.ORDER_TYPE
                    Case OrderTypes.RatioSpread
                        Return MIPTopRatioPrice
                    Case OrderTypes.FFA, OrderTypes.CalendarSpread, OrderTypes.PriceSpread
                        Return MIPTopPrice
                    Case OrderTypes.MarketSpread
                        Return order.PRICE_INDICATED
                End Select
            Case Else
                Select Case order.ORDER_TYPE
                    Case OrderTypes.RatioSpread
                        Return MIPMinRatioPrice
                    Case OrderTypes.FFA
                        Return 0.001
                    Case OrderTypes.CalendarSpread, OrderTypes.PriceSpread
                        Return -MIPTopPrice
                    Case OrderTypes.MarketSpread
                        Return order.PRICE_INDICATED
                End Select
        End Select
        Return 0
    End Function

    Public Function MarketDepth2(ByRef order As Object, _
                                 ByRef rs As String, _
                                 Optional ByVal bAnyPrice As Boolean = False) As Collection
        If IsNothing(order) Then Return Nothing
        Dim TCS As String = ""
        Dim ic As Integer = 0
        Dim XPrice As Double = -1.0E+20

        While ic <= 0
            Dim nc As New Collection
            Dim BidId As Integer = 2000000000
            MarketDepth2 = Nothing
            Dim c As New Collection
            Dim initialOrder As New ORDERS_FFA_CLASS
            initialOrder.GetFromObject(order)
            'If initialOrder.ORDER_ID = 0 Then
            If bAnyPrice Then
                initialOrder.PRICE_INDICATED = TargetPrice(order)
            Else
                ic = ic + 1
            End If

            If Math.Abs(XPrice) < 1.0E+20 Then
                bAnyPrice = False
                initialOrder.PRICE_INDICATED = XPrice
            End If
            ic = ic + 1
            initialOrder.PRICE_TYPE = "F"
            If initialOrder.ORDER_TYPE <> OrderTypes.FFA Then ic = ic + 1
            If initialOrder.ORDER_ID = 0 Then
                initialOrder.ORDER_ID = BidId
            Else
                BidId = initialOrder.ORDER_ID
            End If
            Dim BIDE As Integer = BidId
            'End If
            Dim LegOrderID1 As Integer = 0
            Dim LegOrderID2 As Integer = 0
            Dim LegOrder1 As ORDERS_FFA_CLASS = Nothing
            Dim LegOrder2 As ORDERS_FFA_CLASS = Nothing
            Dim VCStr As String = "", RStr As String = ""
            Dim VCID As Integer = 0
            GetVesselClassRouteName(initialOrder.ROUTE_ID, VCStr, RStr, VCID, TCS)

            c.Add(initialOrder, initialOrder.ORDER_ID.ToString())
            If order.ORDER_TYPE <> OrderTypes.FFA Then
                LegOrderID1 = NullInt2Int(initialOrder.CROSS_ORDER_ID1)
                LegOrderID2 = NullInt2Int(initialOrder.CROSS_ORDER_ID2)
                If LegOrderID1 = 0 Then
                    LegOrderID1 = BidId + 1
                    BIDE += 1
                End If
                If LegOrderID2 = 0 Then
                    LegOrderID2 = BidId + 2
                    BIDE += 1
                End If

                LegOrder1 = initialOrder.GetSpreadLeg(1)
                LegOrder1.ORDER_ID = LegOrderID1
                LegOrder1.SPREAD_ORDER_ID = initialOrder.ORDER_ID
                initialOrder.CROSS_ORDER_ID1 = LegOrder1.ORDER_ID
                c.Add(LegOrder1, LegOrder1.ORDER_ID.ToString())

                LegOrder2 = initialOrder.GetSpreadLeg(2)
                LegOrder2.ORDER_ID = LegOrderID2
                LegOrder2.SPREAD_ORDER_ID = initialOrder.ORDER_ID
                initialOrder.CROSS_ORDER_ID2 = LegOrder2.ORDER_ID
                c.Add(LegOrder2, LegOrder2.ORDER_ID.ToString())
            End If

            For Each o As ORDERS_FFA_CLASS In ORDERS_FFAS
                If o.ORDER_ID <> initialOrder.ORDER_ID And _
                   o.ORDER_ID <> LegOrderID1 And _
                   o.ORDER_ID <> LegOrderID2 And _
                   o.LIVE_STATUS = "A" And o.PRICE_TYPE = "F" Then
                    c.Add(o, o.ORDER_ID.ToString())
                End If
            Next

            Dim ol = From q In c Join s In DESK_TRADERS _
                On q.FOR_DESK_TRADER_ID Equals s.DESK_TRADER_ID _
                Join r In ROUTES On q.ROUTE_ID Equals r.ROUTE_ID _
             Where q.LIVE_STATUS = "A" And q.ORDER_TYPE = OrderTypes.FFA _
                 And q.PRICE_TYPE = "F" _
             Order By q.ORDER_DATETIME _
             Select q, s.ACCOUNT_DESK_ID, r.SETTLEMENT_TICK, r, r.PRICING_TICK

            Dim gdb As DB_ARTB_NETDataContext = Nothing
            RefCol.Clear()
            Dim ExchangeList = New List(Of Integer)
            For Each co In ol
                Dim rps As String = RoutePeriodStringFromObj(co.q)
                Dim RefC As REF_CLASS = GetViewClass(RefCol, rps)
                If IsNothing(RefC) Then
                    RefC = New REF_CLASS(rps)

                    RefCol.Add(RefC, rps)
                End If
                If NullInt2Int(co.q.SPREAD_ORDER_ID) = 0 Then RefC.AssignOrderPrice(co.q, co.PRICING_TICK)

                If co.q.ORDER_BS <> "B" Then Continue For

                For Each co2 In ol
                    If co.ACCOUNT_DESK_ID = co2.ACCOUNT_DESK_ID Then Continue For
                    If co2.q.ORDER_BS <> "S" Then Continue For
                    If MatchingOrders(Nothing, co.q, co2.q, ExchangeList, , False) Then
                        For Each ExchangeId As Integer In ExchangeList
                            Dim nmoc As MARKET_MATCHING_CLASS = New MARKET_MATCHING_CLASS
                            nmoc.BuyOrder = co.q
                            nmoc.BuyRefPrice = co.q.PRICE_INDICATED
                            If Not IsNothing(nmoc.BuyOrder.SPREAD_ORDER_ID) Then
                                If nmoc.BuyOrder.SPREAD_ORDER_ID = initialOrder.ORDER_ID Then
                                    nmoc.BuyOrderSpread = initialOrder
                                Else
                                    nmoc.BuyOrderSpread = GetViewClass(ORDERS_FFAS, nmoc.BuyOrder.SPREAD_ORDER_ID.ToString())
                                End If
                            End If

                            nmoc.SellOrder = co2.q
                            nmoc.SellRefPrice = co2.q.PRICE_INDICATED

                            If Not IsNothing(nmoc.SellOrder.SPREAD_ORDER_ID) Then
                                If nmoc.SellOrder.SPREAD_ORDER_ID = initialOrder.ORDER_ID Then
                                    nmoc.SellOrderSpread = initialOrder
                                Else
                                    nmoc.SellOrderSpread = GetViewClass(ORDERS_FFAS, nmoc.SellOrder.SPREAD_ORDER_ID.ToString())
                                End If
                            End If

                            nmoc.Tick = co.SETTLEMENT_TICK
                            If co.q.ORDER_TYPE = OrderTypes.RatioSpread Then nmoc.Tick = 0.001
                            nmoc.LotSize = co.r.LOT_SIZE
                            Dim level As Integer
                            'If Math.Abs(nmoc.BuyRefPrice) > 1000000000.0 Then
                            '    nmoc.BuyRefPrice = GetReferencePrice(gdb, nmoc.BuyOrder, False, True, level)
                            '    If level < 2 Then
                            '        nmoc.BuyRefPrice *= 1 + RatioVolatility
                            '    Else
                            '        nmoc.BuyRefPrice *= 1 + HistoricRatioVolatility
                            '    End If
                            'End If
                            'If Math.Abs(nmoc.SellRefPrice) > 1000000000.0 Then
                            '    nmoc.SellRefPrice = GetReferencePrice(gdb, nmoc.SellOrder, False, True)
                            '    If level < 2 Then
                            '        nmoc.SellRefPrice *= 1 - RatioVolatility
                            '    Else
                            '        nmoc.SellRefPrice *= 1 - HistoricRatioVolatility
                            '    End If
                            'End If
                            nmoc.ActualQuantity = 0
                            nmoc.ExchangeID = ExchangeId
                            nmoc.BuyExchangeRank = GetDeskExchangeRanking(co.ACCOUNT_DESK_ID, _
                                                                      ExchangeId, nmoc.BuyOrder.ROUTE_ID)
                            nmoc.SellExchangeRank = GetDeskExchangeRanking(co2.ACCOUNT_DESK_ID, _
                                                                  ExchangeId, nmoc.SellOrder.ROUTE_ID)
                            nc.Add(nmoc)
                        Next
                    End If
                Next

            Next

            UpdateRefCol(Nothing, TCS)

            If nc.Count = 0 Then
                gdb = Nothing
                Exit Function
            End If
            Dim AdjOrderID As Integer = BidId
            If bAnyPrice = False Then AdjOrderID = -1
            MarketDepth2 = MarketMIPOrders(gdb, nc, BidId, c, MIPPrefBonus, BIDE, , , AdjOrderID)
            gdb = Nothing
            RefCol.Clear()
            rs = ""
            If IsNothing(MarketDepth2) Then Exit Function
            Dim Cap(2) As Double
            Dim AQ(2) As Double
            Dim FQ(2) As String

            For i As Integer = 0 To 1
                Cap(i) = 0
                AQ(i) = 0
                FQ(2) = ""
            Next

            For Each oc As MARKET_MATCHING_CLASS In MarketDepth2
                Dim Pr As Double = 0
                Dim fs As String = "0"
                Dim dg As Integer = -Math.Log10(oc.Tick)
                Dim RouteName As String

                If dg >= 1 Then
                    fs = "0."
                    For j As Integer = 1 To dg
                        fs &= "0"
                    Next
                End If
                Dim QS As String = oc.ActualQuantity.ToString()
                If oc.BuyOrder.ORDER_ID >= BidId And oc.BuyOrder.ORDER_ID <= BIDE Then
                    Dim Leg As Integer = 0
                    If initialOrder.ORDER_TYPE <> OrderTypes.FFA And initialOrder.ORDER_BS = "S" Then Leg = 1
                    Cap(Leg) += oc.Price * oc.ActualQuantity
                    AQ(Leg) += oc.ActualQuantity
                    Pr = Round(oc.Price, oc.Tick)
                    RouteName = GetRouteName(oc.BuyOrder.ROUTE_ID)
                    If Int(oc.ActualQuantity) = 30 And oc.BuyOrder.FLEXIBLE_QUANTITY = OrderFlexQuantinty.StrictFull Then
                        QS = "Full"
                        FQ(Leg) = QS
                    End If
                    rs &= RouteName & " " & oc.BuyOrder.SHORTDES & " " & Format(Pr, fs) & " x " & QS & " In "
                    rs &= GetExchangeName(oc.ExchangeID) & "," & vbNewLine
                ElseIf oc.SellOrder.ORDER_ID >= BidId And oc.SellOrder.ORDER_ID <= BIDE Then
                    Dim Leg As Integer = 0
                    If initialOrder.ORDER_TYPE <> OrderTypes.FFA And initialOrder.ORDER_BS = "B" Then Leg = 1
                    Cap(Leg) += oc.Price * oc.ActualQuantity
                    AQ(Leg) += oc.ActualQuantity

                    Pr = Round(oc.Price, oc.Tick)
                    RouteName = GetRouteName(oc.SellOrder.ROUTE_ID)
                    If Int(oc.ActualQuantity) = 30 And oc.SellOrder.FLEXIBLE_QUANTITY = OrderFlexQuantinty.StrictFull Then
                        QS = "Full"
                        FQ(Leg) = QS
                    End If
                    rs &= RouteName & " " & oc.SellOrder.SHORTDES & " " & Format(Pr, fs) & " x " & QS & " In "
                    rs &= GetExchangeName(oc.ExchangeID) & "," & vbNewLine
                End If
            Next
            If AQ(0) > 0 Then
                Dim Pr As Double = -1.0E+20
                Dim Tick As Double = 0, PriceTick As Double = 0
                Call GetRouteTick(Nothing, initialOrder, PriceTick, Tick)

                rs &= vbNewLine
                If initialOrder.ORDER_TYPE <> OrderTypes.FFA And AQ(1) > 0 Then
                    Select Case initialOrder.ORDER_TYPE
                        Case OrderTypes.RatioSpread
                            Pr = (Cap(0) / AQ(0)) / (Cap(1) / AQ(1))
                        Case OrderTypes.PriceSpread, OrderTypes.CalendarSpread
                            Pr = (Cap(0) / AQ(0)) - (Cap(1) / AQ(1))
                    End Select
                    Pr = Round(Pr, Tick)
                    If Int(AQ(0)) <> 30 Or FQ(0) = "" Then FQ(0) = AQ(0).ToString()
                    If Int(AQ(1)) <> 30 Or FQ(1) = "" Then FQ(1) = AQ(1).ToString()
                    Dim qs As String = ""
                    qs = Format(Pr, FormatPriceString(Tick)) & " x " & FQ(0)
                    qs &= "/" & FQ(1)
                    rs = vbNewLine & qs & vbNewLine & vbNewLine & rs
                Else
                    Pr = (Cap(0) / AQ(0))
                    Pr = Round(Pr, Tick)
                    If Int(AQ(0)) <> 30 Or FQ(0) = "" Then FQ(0) = AQ(0).ToString()

                    Dim qs As String = Format(Pr, FormatPriceString(Tick)) & " x " & FQ(0)
                    rs = vbNewLine & qs & vbNewLine & vbNewLine & rs
                End If
                XPrice = Pr
            End If
            rs = rs & vbNewLine
            For Each noc In nc
                noc = Nothing
            Next
            MarketDepth2 = Nothing
            nc = Nothing
            initialOrder = Nothing
            LegOrder1 = Nothing
            LegOrder2 = Nothing
        End While

    End Function

    Public Function GetSettlementTick(ByVal RouteID As Integer, ByVal Ordertype As Integer) As Double
        Dim RouteClass As ROUTE_CLASS = GetRouteClass(RouteID)
        If IsNothing(RouteClass) Then Exit Function
        Select Case Ordertype
            Case OrderTypes.FFA, OrderTypes.CalendarSpread, OrderTypes.PriceSpread
                GetSettlementTick = RouteClass.SETTLEMENT_TICK
                Exit Function
            Case OrderTypes.RatioSpread
                GetSettlementTick = MIPMinRatioPrice
                Exit Function
        End Select
    End Function

    Public Function GetPriceTick(ByVal RouteID As Integer, ByVal Ordertype As Integer) As Double
        Dim RouteClass As ROUTE_CLASS = GetRouteClass(RouteID)
        If IsNothing(RouteClass) Then Exit Function
        Select Case Ordertype
            Case OrderTypes.FFA, OrderTypes.CalendarSpread, OrderTypes.PriceSpread
                GetPriceTick = RouteClass.PRICING_TICK
                Exit Function
            Case OrderTypes.RatioSpread
                GetPriceTick = MIPMinRatioPrice
                Exit Function
        End Select
    End Function


    Public Sub GetRouteTick(ByRef gdb As DB_ARTB_NETDataContext, _
                            ByRef o As Object, _
                            ByRef PriceTick As Double, _
                            ByRef SettlementTick As Double)
        PriceTick = 0
        SettlementTick = 0
        Dim RouteID As Integer
        Dim RouteID2 As Integer
        Dim OrderType As Integer = OrderTypes.FFA
        Try
            RouteID = NullInt2Int(o.ROUTE_ID)
            RouteID2 = NullInt2Int(o.ROUTE_ID2)
            If TypeOf o Is ORDERS_FFA Or TypeOf o Is ORDERS_FFA_CLASS Then OrderType = o.ORDER_TYPE
            If TypeOf o Is TRADES_FFA Or TypeOf o Is TRADES_FFA_CLASS Then OrderType = o.TRADE_TYPE
        Catch ex As Exception
            Debug.Print(ex.ToString)
            Exit Sub
        End Try
        If Not gdb Is Nothing And OperationMode = GVCOpMode.Service Then
            Select Case OrderType
                Case OrderTypes.FFA, OrderTypes.CalendarSpread, OrderTypes.PriceSpread
                    Try
                        Dim ol = From q In gdb.ROUTEs _
                                 Where q.ROUTE_ID = RouteID _
                                 Select q.PRICING_TICK, q.SETTLEMENT_TICK
                        For Each q In ol
                            PriceTick = q.PRICING_TICK
                            SettlementTick = q.SETTLEMENT_TICK
                            Exit Sub
                        Next
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try
                Case OrderTypes.RatioSpread
                    Try
                        Dim ol = From q In gdb.TRADE_CLASS_RATIO_SPREADs _
                                  Where q.ROUTE_ID1 = RouteID And q.ROUTE_ID2 = RouteID2 _
                                  Select q.PRICING_TICK, q.PRECISION_TICK
                        For Each q In ol
                            PriceTick = q.PRICING_TICK
                            SettlementTick = q.PRICING_TICK
                            Exit Sub
                        Next
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try
            End Select
        Else
            Dim RouteClass As ROUTE_CLASS = GetRouteClass(RouteID)
            If IsNothing(RouteClass) Then Exit Sub
            Select Case OrderType
                Case OrderTypes.FFA, OrderTypes.CalendarSpread, OrderTypes.PriceSpread
                    PriceTick = RouteClass.PRICING_TICK
                    SettlementTick = RouteClass.SETTLEMENT_TICK
                    Exit Sub
                Case OrderTypes.RatioSpread
                    PriceTick = MIPMinRatioPrice
                    SettlementTick = MIPMinRatioPrice
                    Exit Sub
            End Select
        End If

    End Sub


    Public Function GetTradeFFAExchangeID(ByVal a_TradeID As Integer) As Integer
        GetTradeFFAExchangeID = 0
        Dim TradeClass As TRADES_FFA_CLASS = GetViewClass(TRADES_FFAS, a_TradeID.ToString())
        If IsNothing(TradeClass) Then Exit Function
        If TradeClass.IS_SYNTHETIC Then Return 1
        Try
            GetTradeFFAExchangeID = TradeClass.EXCHANGE_ID
        Catch ex As Exception

        End Try
    End Function

    Public Function GetDefaultRouteID(ByVal a_VesselClassID As Integer) As Integer
        GetDefaultRouteID = 0
        Dim VesselClass As VESSEL_CLASS_CLASS = GetViewClass(VESSEL_CLASSES, a_VesselClassID.ToString())
        If IsNothing(VesselClass) Then Exit Function
        GetDefaultRouteID = NullInt2Int(VesselClass.DEFAULT_ROUTE_ID)
    End Function

    Public Sub GetRouteInfo(ByVal a_RouteID As Integer, _
                            ByRef a_TradeClassShort As String, _
                            ByRef a_VesselClassName As String, _
                            ByRef a_VesselClassID As Integer)
        a_TradeClassShort = ""
        a_VesselClassName = ""
        a_VesselClassID = 0
        Dim RouteClass As ROUTE_CLASS = GetViewClass(ROUTES, a_RouteID.ToString())
        If IsNothing(RouteClass) Then Exit Sub
        a_VesselClassID = RouteClass.VESSEL_CLASS_ID
        Dim VesselClass As VESSEL_CLASS_CLASS = GetViewClass(VESSEL_CLASSES, a_VesselClassID.ToString())
        If IsNothing(VesselClass) Then Exit Sub
        a_TradeClassShort = VesselClass.DRYWET
        a_VesselClassName = VesselClass.VESSEL_CLASS
    End Sub

    Public Function GetRouteName(ByVal a_RouteID As Integer) As String
        GetRouteName = ""
        Dim RouteClass As ROUTE_CLASS = GetViewClass(ROUTES, a_RouteID.ToString())
        If IsNothing(RouteClass) Then Exit Function
        Dim VesselClassID As Integer = RouteClass.VESSEL_CLASS_ID
        Dim VesselClass As VESSEL_CLASS_CLASS = GetViewClass(VESSEL_CLASSES, VesselClassID.ToString())
        If IsNothing(VesselClass) Then Exit Function
        GetRouteName = VesselClass.VESSEL_CLASS & "-" & RouteClass.ROUTE_SHORT
    End Function

    Public Function FirmUpOrder(ByRef o As ORDERS_FFA_CLASS, _
                                Optional ByVal a_TraderId As Integer = 0, _
                                Optional ByVal a_ForTraderId As Integer = 0, _
                                Optional ByVal a_SingleExchangeID As Integer = 0) As ORDERS_FFA_CLASS
        FirmUpOrder = Nothing
        If IsNothing(o) Then Exit Function
        FirmUpOrder = New ORDERS_FFA_CLASS
        FirmUpOrder.GetFromObject(o)
        If FirmUpOrder.ORDER_BS = "B" Then
            FirmUpOrder.ORDER_BS = "S"
        Else
            FirmUpOrder.ORDER_BS = "B"
        End If
        FirmUpOrder.PRICE_TYPE = "F"
        If Not IsNothing(FirmUpOrder.PRICE_TYPE2) Then
            FirmUpOrder.PRICE_TYPE2 = "F"
        End If
        FirmUpOrder.COUNTER_PARTY_ORDER_ID = o.ORDER_ID
        FirmUpOrder.LIVE_STATUS = "R"
        FirmUpOrder.ORDER_ID = 0
        FirmUpOrder.ORDER_GOOD_TILL = OrderGoodTill.Limit
        FirmUpOrder.ORDER_TIME_LIMIT = Int(REQUEST_REPLY_TIMEOUT / 60)
        If a_TraderId = 0 Then a_TraderId = TraderID
        If a_ForTraderId = 0 Then a_ForTraderId = TraderID
        FirmUpOrder.DESK_TRADER_ID = a_TraderId
        FirmUpOrder.FOR_DESK_TRADER_ID = a_ForTraderId
        FirmUpOrder.ORDER_QUALIFIER = "R"
    End Function

    Public Function GetOrderActivity(ByRef q As ORDERS_FFA_CLASS) As String
        GetOrderActivity = ""
        If q.LIVE_STATUS = "R" Then Return "N"
        If q.LIVE_STATUS = "N" Then
            GetOrderActivity = "R"
            Exit Function
        End If
        If q.ORDER_QUALIFIER = "N" Or q.ORDER_QUALIFIER = "R" Then
            GetOrderActivity = q.ORDER_QUALIFIER
            Exit Function
        End If
        Dim OrderId As Integer = q.ORDER_ID
        Dim ol = From o In ORDERS_FFAS _
                  Where o.COUNTER_PARTY_ORDER_ID = OrderId And o.LIVE_STATUS = "A" Select o
        For Each o In ol
            If o.ORDER_QUALIFIER = "N" Then
                GetOrderActivity = "I"
                Exit Function
            ElseIf o.ORDER_QUALIFIER = "R" Then
                GetOrderActivity = "R"
                Exit Function
            End If
        Next
    End Function

    Public Function DeletePreviousNegotiations(ByRef gdb As DB_ARTB_NETDataContext, _
                                                ByVal OrderId As Integer, _
                                                ByVal NegotiationOrderId As Integer, _
                                                ByVal FilterQ As Integer, _
                                                ByRef OrderStr As String, _
                                                ByRef TradeStr As String, _
                                                ByRef AffectedRoutePeriods As List(Of String)) As Integer
        DeletePreviousNegotiations = ArtBErrors.Success
        Dim OrderList = From q In gdb.ORDERS_FFAs _
                         Where q.ORDER_ID <> OrderId _
                           And q.NEGOTIATION_ORDER_ID = NegotiationOrderId And q.ORDER_QUALIFIER = "N" _
                           And (q.LIVE_STATUS = "A" Or q.LIVE_STATUS = "N") _
                         Select q
        For Each o In OrderList
            Dim q As Integer = Int(GetActualQuantity(o))
            If q > FilterQ Then
                DeletePreviousNegotiations = SetOrderLiveStatus(gdb, o.ORDER_ID, "D", OrderStr, TradeStr, AffectedRoutePeriods)
                If DeletePreviousNegotiations <> ArtBErrors.Success Then
                    Exit Function
                End If
            End If
        Next
    End Function

    'Public Function SetLiveStatusForOtherNegotiations(ByRef NOrder As ORDERS_FFA_CLASS, _
    '                                        ByVal IOrder As ORDERS_FFA_CLASS, _
    '                                        ByVal CounterTraderId As Integer, _
    '                                        ByVal c As String, _
    '                                        ByRef s As String) As Integer
    '    Dim gdb As New DB_ARTB_NETDataContext(ArtBConnectionStr)
    '    Dim oc As String = "A"
    '    If c = "A" Then oc = "N"
    '    Dim NOrderId As Integer = NOrder.ORDER_ID
    '    Dim IOrderId As Integer = IOrder.ORDER_ID
    '    SetLiveStatusForOtherNegotiations = ArtBErrors.Success
    '    Dim OrderList = From q In gdb.ORDERS_FFAs _
    '                     Where q.ORDER_ID <> NOrderId _
    '                       And (q.ORDER_ID = IOrderId Or _
    '                            (q.NEGOTIATION_ORDER_ID = IOrderId And q.ORDER_QUALIFIER = "N")) _
    '                        And q.LIVE_STATUS = oc _
    '                     Select q
    '    For Each o In OrderList
    '        SetLiveStatusForOtherNegotiations = SetOrderLiveStatus(o.ORDER_ID, c, s, NOrderId, CounterTraderId)
    '        If SetLiveStatusForOtherNegotiations <> ArtBErrors.Success Then
    '            Exit Function
    '        End If
    '    Next
    'End Function

    Public Function UnlockOrders(ByRef gdb As DB_ARTB_NETDataContext, _
                                 ByVal NOrderID As Integer, _
                                 ByRef OrderStr As String, _
                                 ByRef TradeStr As String, _
                                 ByRef AffectedRoutePeriods As List(Of String)) As Integer
        UnlockOrders = ArtBErrors.Success
        Dim OrderList = From q In gdb.ORDERS_FFAs _
                         Where q.ORDER_ID <> NOrderID _
                           And q.LOCKED_BY_ORDER_ID = NOrderID And q.LIVE_STATUS = "N" _
                         Select q
        For Each o In OrderList
            UnlockOrders = SetOrderLiveStatus(gdb, o.ORDER_ID, "A", OrderStr, TradeStr, AffectedRoutePeriods, 0, 0)
            If UnlockOrders <> ArtBErrors.Success Then
                Exit Function
            End If
        Next
    End Function

    Public Function MoveNegotiationOrders(ByRef gdb As DB_ARTB_NETDataContext, _
                                          ByRef FromOrder As ORDERS_FFA_CLASS, _
                                          ByRef ToOrder As ORDERS_FFA_CLASS, _
                                          ByRef OrderStr As String, _
                                          ByRef TradeStr As String, _
                                          ByRef AffectedRoutePeriods As List(Of String)) As Integer
        MoveNegotiationOrders = ArtBErrors.Success
        Dim NewQ As Integer = Int(GetActualQuantity(ToOrder))
        Dim FromID As Integer = FromOrder.ORDER_ID
        Dim ToID As Integer = ToOrder.ORDER_ID

        Dim OrderList = From q In gdb.ORDERS_FFAs _
                         Where q.ORDER_ID <> ToID _
                           And (q.NEGOTIATION_ORDER_ID = FromID And q.ORDER_QUALIFIER = "N" And q.LIVE_STATUS = "A") _
                         Select q

        For Each o In OrderList
            Dim q As Integer = Int(GetActualQuantity(o))
            If q <= NewQ Then
                MoveNegotiationOrders = SetNewNegotiationOrderID(gdb, o.ORDER_ID, ToOrder.ORDER_ID, OrderStr)
                If MoveNegotiationOrders <> ArtBErrors.Success Then
                    Exit Function
                End If
            Else
                MoveNegotiationOrders = SetOrderLiveStatus(gdb, o.ORDER_ID, "D", OrderStr, TradeStr, AffectedRoutePeriods)
                If MoveNegotiationOrders <> ArtBErrors.Success Then
                    Exit Function
                End If
            End If
        Next
    End Function

    Public Function DeletePreviousNegotiationsForTrader(ByRef gdb As DB_ARTB_NETDataContext, _
                                                        ByVal NOrderId As Integer, _
                                                        ByVal IOrderId As Integer, _
                                                        ByVal TraderId As Integer, _
                                                        ByRef OrderStr As String, _
                                                        ByRef TradeStr As String, _
                                                        ByRef AffectedRoutePeriods As List(Of String)) As Integer
        DeletePreviousNegotiationsForTrader = ArtBErrors.Success
        Dim OrderList = From q In gdb.ORDERS_FFAs _
                         Where q.ORDER_ID <> NOrderId _
                           And q.ORDER_ID <> IOrderId _
                           And q.NEGOTIATION_ORDER_ID = IOrderId _
                           And q.ORDER_QUALIFIER = "N" _
                           And (q.LIVE_STATUS = "A" Or q.LIVE_STATUS = "N") _
                           And q.FOR_DESK_TRADER_ID = TraderId _
                         Select q
        Dim cpl As New ORDERS_FFA_CLASS
        For Each o In OrderList
            DeletePreviousNegotiationsForTrader = cpl.GetFromObject(o)
            cpl.LIVE_STATUS = "D"
            DeletePreviousNegotiationsForTrader = UpdateDBFromNewOrder(gdb, cpl, OrderStr, TradeStr, AffectedRoutePeriods)
            If DeletePreviousNegotiationsForTrader <> ArtBErrors.Success Then
                cpl = Nothing
                Exit Function
            End If
        Next

        Dim OrderList2 = From q In gdb.ORDERS_FFAs Join r In gdb.ORDERS_FFAs On q.COUNTER_PARTY_ORDER_ID Equals r.ORDER_ID _
                 Where q.ORDER_ID <> NOrderId _
                   And q.ORDER_ID <> IOrderId _
                   And q.NEGOTIATION_ORDER_ID = IOrderId _
                   And q.ORDER_QUALIFIER = "N" _
                   And (q.LIVE_STATUS = "A" Or q.LIVE_STATUS = "N") _
                   And r.FOR_DESK_TRADER_ID = TraderId _
                 Select q
        For Each o In OrderList2
            DeletePreviousNegotiationsForTrader = cpl.GetFromObject(o)
            cpl.LIVE_STATUS = "D"
            DeletePreviousNegotiationsForTrader = UpdateDBFromNewOrder(gdb, cpl, OrderStr, TradeStr, AffectedRoutePeriods)
            If DeletePreviousNegotiationsForTrader <> ArtBErrors.Success Then
                cpl = Nothing
                Exit Function
            End If
        Next
        cpl = Nothing
    End Function

    Public Function GetTradeInfo(ByVal a_TradeId As Integer, _
                                 ByVal a_dir As Integer, _
                                 ByRef UserInfo As TraderInfoClass, _
                                 ByRef c As String) As String
        GetTradeInfo = ""
        c = ""
        Dim TradeClass As TRADES_FFA_CLASS = GetViewClass(TRADES_FFAS, a_TradeId.ToString())
        If IsNothing(TradeClass) Then Exit Function
        If a_dir = 1 And NullInt2Int(TradeClass.INFORM_DESK_ID1) <> UserInfo.DeskID Then Exit Function
        If a_dir = 2 And NullInt2Int(TradeClass.INFORM_DESK_ID2) <> UserInfo.DeskID Then Exit Function
        Dim TradeID As Integer = TradeClass.TRADE_ID
        Dim ac1 As String = "", desk1 As String = "", trader1 As String = ""
        Dim acID1 As Integer, deskID1 As Integer
        GetAllTraderNames(TradeClass.DESK_TRADER_ID1, ac1, desk1, trader1, acID1, deskID1)
        Dim ac2 As String = "", desk2 As String = "", trader2 As String = ""
        Dim acID2 As Integer, deskID2 As Integer
        Dim CounterPartyStr As String
        GetAllTraderNames(TradeClass.DESK_TRADER_ID2, ac2, desk2, trader2, acID2, deskID2)
        Dim s As String
        Dim OrderClass As ORDERS_FFA_CLASS

        If a_dir = 1 Then
            GetTradeInfo = "ORDER ID:" & vbTab & TradeClass.ORDER_ID1.ToString() & vbNewLine & _
                            "ACCOUNT:" & vbTab & ac1 & vbNewLine & _
                            "DESK:" & vbTab & vbTab & desk1 & vbNewLine & _
                            "TRADER:" & vbTab & trader1 & vbNewLine & vbNewLine
            TradeDescr(TradeClass, 2, 1, 1, s)
            If TradeClass.TRADE_BS1 = "B" Then
                c = "Buy Order Execution for  " & ac1 & ", " & desk1
            Else
                c = "Sell Order Execution for  " & ac1 & ", " & desk1
            End If
            If TradeClass.IS_SYNTHETIC = False Or TradeClass.TRADE_TYPE = OrderTypes.FFA Then
                Dim o2 As ORDERS_FFA_CLASS = GetViewClass(ORDERS_FFAS, TradeClass.ORDER_ID2.ToString())
                CounterPartyStr = GetTraderNameWithRules(UserInfo, UserInfo, o2, True)
            Else
                Dim LegTradeList = From t In TRADES_FFAS Where t.SPREAD_TRADE_ID1 = TradeID Or t.SPREAD_TRADE_ID2 = TradeID Select t
                Dim exch1 As String = ""

                For Each t As TRADES_FFA In LegTradeList
                    Dim TraderName As String = ""
                    If TradeClass.DESK_TRADER_ID1 <> t.DESK_TRADER_ID1 Then
                        If TradeClass.EXCHANGE_ID = Exch.OTC Then
                            TraderName = GetTraderName(t.DESK_TRADER_ID1, , UserInfo)
                        Else
                            Dim o1 As ORDERS_FFA_CLASS = GetViewClass(ORDERS_FFAS, t.ORDER_ID1.ToString())
                            TraderName = GetTraderNameWithRules(UserInfo, UserInfo, o1, True)
                        End If
                    Else
                        If TradeClass.EXCHANGE_ID = Exch.OTC Then
                            TraderName = GetTraderName(t.DESK_TRADER_ID2, , UserInfo)
                        Else
                            Dim o2 As ORDERS_FFA_CLASS = GetViewClass(ORDERS_FFAS, t.ORDER_ID2.ToString())
                            TraderName = GetTraderNameWithRules(UserInfo, UserInfo, o2, True)
                        End If
                    End If
                    If Len(TraderName) >= 1 And InStr(CounterPartyStr, TraderName) <= 0 Then
                        If Len(CounterPartyStr) >= 1 Then CounterPartyStr = CounterPartyStr & ","
                        CounterPartyStr = CounterPartyStr & TraderName
                    End If
                Next
            End If
        Else
            GetTradeInfo = "ORDER ID:" & vbTab & TradeClass.ORDER_ID2.ToString() & vbNewLine & _
                            "ACCOUNT:" & vbTab & ac2 & vbNewLine & _
                            "DESK:" & vbTab & vbTab & desk2 & vbNewLine & _
                            "TRADER:" & vbTab & trader2 & vbNewLine & vbNewLine
            TradeDescr(TradeClass, 2, 2, 1, s)
            If TradeClass.TRADE_BS2 = "B" Then
                c = "Buy Order Execution for  " & ac2 & ", " & desk2
            Else
                c = "Sell Order Execution for  " & ac2 & ", " & desk2
            End If
            Dim o1 As ORDERS_FFA_CLASS = GetViewClass(ORDERS_FFAS, TradeClass.ORDER_ID1.ToString())
            CounterPartyStr = GetTraderNameWithRules(UserInfo, UserInfo, o1, True)
        End If

        GetTradeInfo = GetTradeInfo & s
        If (UserInfo.IsAffiliateBroker Or UserInfo.IsBroker) Then
            GetTradeInfo = GetTradeInfo & vbNewLine & vbNewLine & "C/PARTY: " & vbTab & CounterPartyStr
        End If
        If GetTradeFFAExchangeID(a_TradeId) > 0 Then Exit Function
        GetTradeInfo = GetTradeInfo & vbNewLine & "Please select exchange from Trades window."
    End Function

    Public Function GetTraderInfo(ByVal a_TraderId As Integer, ByVal a_SWAuth As Boolean) As TraderInfoClass
        GetTraderInfo = Nothing
        Dim TraderClass As DESK_TRADER_CLASS = GetViewClass(DESK_TRADERS, a_TraderId.ToString())
        If TraderClass Is Nothing Then Exit Function
        Dim DeskClass As ACCOUNT_DESK_CLASS = GetViewClass(ACCOUNT_DESKS, TraderClass.ACCOUNT_DESK_ID.ToString())
        If DeskClass Is Nothing Then Exit Function
        Dim AccountClass As ACCOUNT_CLASS = GetViewClass(ACCOUNTS, DeskClass.ACCOUNT_ID.ToString())
        If AccountClass Is Nothing Then Exit Function
        Dim ContactClass As CONTACT_CLASS = GetViewClass(CONTACTS, TraderClass.CONTACT_ID.ToString())
        If AccountClass Is Nothing Then Exit Function
        GetTraderInfo = New TraderInfoClass
        With GetTraderInfo
            .AccountName = AccountClass.SHORT_NAME
            .DeskName = DeskClass.DESK_DESCR
            .TraderName = ContactClass.FIRSTNAME & " " & ContactClass.LASTNAME
            .DeskID = DeskClass.ACCOUNT_DESK_ID
            .AccountID = AccountClass.ACCOUNT_ID
            .AccountType = AccountClass.ACCOUNT_TYPE_ID
            .TraderID = a_TraderId
            .IsBroker = True
            .IsSystemAdmin = False
            .IsAffiliateBroker = False
            .AffBrokerID = AccountClass.BROKER_ID
            If .AccountType = 1 Then
                .IsBroker = False
                .IsTrader = True
            End If
            If .AccountType = 5 Then
                .IsAffiliateBroker = True
            End If

            .BrokerDefaultClient = NullInt2Int(AccountClass.DEFAULT_BROKER_CLIENT)
            If .BrokerDefaultClient <> 0 Then
                Dim ClientClass As ACCOUNT_CLASS = GetViewClass(ACCOUNTS, .BrokerDefaultClient.ToString())
                If Not ClientClass Is Nothing Then
                    For Each ClientDesk As ACCOUNT_DESK_CLASS In ClientClass.DESKS
                        If ClientDesk.DESK_ACTIVE = True And ClientDesk.DESK_QUALIFIER = 1 Then
                            For Each ClientTraderClass As DESK_TRADER_CLASS In ClientDesk.TRADERS
                                If ClientTraderClass.IS_DESK_ADMIN = True And ClientTraderClass.SUSPENDED = False Then
                                    .BrokerDefaultClientDeskID = ClientTraderClass.ACCOUNT_DESK_ID
                                    .BrokerDefaultClientTraderID = ClientTraderClass.DESK_TRADER_ID
                                End If
                            Next
                        End If
                    Next
                End If
            End If

            If .AccountType = 6 Then .IsSystemAdmin = True

            .TraderClass = TraderClass
            .AccountClass = AccountClass
            .DeskClass = DeskClass

            .OF_ID = TraderClass.OF_ID
            .IsDeskAdmin = TraderClass.IS_DESK_ADMIN
            .UserName = TraderClass.OF_PASSWORD
            .BidColor = TraderClass.BID_COLOR
            .OfferColor = TraderClass.OFFER_COLOR
            .FontType = TraderClass.FONT_TYPE
            .IndicativesVisible = TraderClass.INDICATIVES_VISIBLE
            .OneClickHit = TraderClass.ONE_CLICK_HIT
            .TradeAuthority = TraderClass.TRADE_AUTHORITY
            If a_SWAuth = False And .AccountType = 1 Then .TradeAuthority = TradeAuthorities.ViewOnly

            Dim ml = From qm In ACCOUNT_DESKS _
                     Where qm.ACCOUNT_ID = AccountID And qm.DESK_QUALIFIER = 0 _
                     Select qm

            For Each qm In ml
                .MainDeskID = qm.ACCOUNT_DESK_ID
            Next

            ReDim .CanTrade(TRADE_CLASSES.Count, EXCHANGES.Count + 1)
            Dim i As Integer
            Dim j As Integer = 0
            For i = 0 To TRADE_CLASSES.Count - 1
                For j = 0 To EXCHANGES.Count
                    .CanTrade(i, j) = False
                Next
            Next
            j = 0

            For Each TC As TRADE_CLASS_CLASS In TRADE_CLASSES
                Dim DTC As DESK_TRADE_CLASS_CLASS
                DTC = GetViewClass(DeskClass.TRADE_CLASSES, TC.TRADE_CLASS_SHORT)
                If Not IsNothing(DTC) Then
                    .AddTCAffiliateBroker(TC.TRADE_CLASS_SHORT, DTC.BROKER_ID)
                    Dim EndBrokerID As Integer = GetMainBrokerForAffiliate(TC.TRADE_CLASS_SHORT, DTC.BROKER_ID)
                    .AddTCBroker(TC.TRADE_CLASS_SHORT, EndBrokerID)

                    For Each q As ArtB_Class_Library.DESK_EXCHANGE_CLASS In DTC.EXCHANGES
                        If q.ACTIVE = True Then
                            .CanTrade(j, q.EXCHANGE_ID) = True
                        End If
                    Next
                End If
                j = j + 1
            Next

            .DefaultMarket = TraderClass.DEFAULT_MARKET
            .ToolbarShow = TraderClass.TOOLBAR_SHOW
            .DefaultShowNames = TraderClass.DEFAULT_SHOW_NAMES
            .DefaultNuke = DeskClass.DEFAULT_NUKE
            .GridMarketDepth = TraderClass.GRID_MARKET_DEPTH
            .DefaultSSE = DeskClass.DEFAULT_SEE
            .DefaultBI = DeskClass.DEFAULT_BI

            Dim BrokerClass As ACCOUNT_CLASS = GetViewClass(ACCOUNTS, .AffBrokerID.ToString())
            If BrokerClass Is Nothing Then Exit Function
            .BrokerID = BrokerClass.BROKER_ID
        End With
    End Function

    Public Function GetTraderInfoFromDesk(ByVal a_DeskId As Integer, ByVal a_SWAuth As Boolean) As TraderInfoClass
        GetTraderInfoFromDesk = Nothing
        Dim ml = From qm In DESK_TRADERS _
                 Where qm.ACCOUNT_DESK_ID = a_DeskId And qm.IS_DESK_ADMIN <> 0 And qm.AUTHORISED <> 0 _
                 Select qm

        For Each q In ml
            GetTraderInfoFromDesk = GetTraderInfo(q.DESK_TRADER_ID, a_SWAuth)
            If Not IsNothing(GetTraderInfoFromDesk) Then Exit Function
        Next

        Dim ml2 = From qm In DESK_TRADERS _
                    Where qm.ACCOUNT_DESK_ID = a_DeskId And qm.AUTHORISED <> 0 _
                    Select qm

        For Each q In ml2
            GetTraderInfoFromDesk = GetTraderInfo(q.DESK_TRADER_ID, a_SWAuth)
            If Not IsNothing(GetTraderInfoFromDesk) Then Exit Function
        Next

    End Function

    Public Function GetDeskAdmin(ByRef DeskClass As ACCOUNT_DESK_CLASS) As Integer
        GetDeskAdmin = 0
        For Each t As DESK_TRADER_CLASS In DeskClass.TRADERS
            If t.AUTHORISED Then
                GetDeskAdmin = t.DESK_TRADER_ID
                If t.IS_DESK_ADMIN Then
                    Exit Function
                End If
            End If
        Next
    End Function

    Public Function GetBrokerTraderIDForOrder(ByRef gdb As DB_ARTB_NETDataContext, _
                                              ByRef OrderClass As ORDERS_FFA_CLASS, _
                                              ByRef UserInfo As TraderInfoClass) As Integer
        If IsNothing(OrderClass) Then Return 0
        If IsNothing(UserInfo) Then Return 0
        Dim TC As String
        Dim s1 As String = "", s2 = ""
        Dim VCID As Integer = 0
        Call GetVesselClassRouteName(OrderClass.ROUTE_ID, s1, s2, VCID, TC)
        GetBrokerTraderIDForOrder = GetBrokerTradeIDForTradeClass(gdb, UserInfo.AccountClass.BROKER_ID, TC)
    End Function

    Public Function GetBrokerTradeIDForTradeClass(ByRef gdb As DB_ARTB_NETDataContext, _
                                                  ByVal a_BrokerAccountId As Integer, _
                                                  ByVal a_TradeClassShort As String) As Integer
        GetBrokerTradeIDForTradeClass = 0
        If OperationMode = GVCOpMode.Service Then
            Dim bNewConnection As Boolean = False
            Try
                If IsNothing(gdb) Then
                    gdb = GetNewConnection()
                    bNewConnection = True
                End If
                Dim q = (From tc In gdb.TRADE_CLASSes _
                         Join tcbd In gdb.BROKER_DESK_TRADE_CLASSes On tc.TRADE_CLASS_SHORT Equals tcbd.TRADE_CLASS_SHORT _
                         Join de In gdb.ACCOUNT_DESKs On tcbd.ACCOUNT_DESK_ID Equals de.ACCOUNT_DESK_ID _
                         Where de.ACCOUNT_ID = a_BrokerAccountId Select de).First
                If bNewConnection Then gdb = Nothing
                If IsNothing(q) Then Exit Function
                Dim DeskId As Integer = q.ACCOUNT_DESK_ID
                Dim l = From t In gdb.DESK_TRADERs Where t.ACCOUNT_DESK_ID = DeskId Select t
                For Each t As DESK_TRADER In l
                    If t.AUTHORISED Then
                        GetBrokerTradeIDForTradeClass = t.DESK_TRADER_ID
                        If t.IS_DESK_ADMIN Then
                            Exit Function
                        End If
                    End If
                Next
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try
            Exit Function
        End If
        Dim TCClass As TRADE_CLASS_CLASS = GetViewClass(TRADE_CLASSES, a_TradeClassShort)
        If IsNothing(TCClass) Then Exit Function
        Dim D As ACCOUNT_DESK_CLASS
        For Each bd In TCClass.BROKER_DESKS
            D = GetViewClass(ACCOUNT_DESKS, bd.ACCOUNT_DESK_ID.ToString())
            If Not IsNothing(D) Then
                If D.ACCOUNT_ID = a_BrokerAccountId Then
                    GetBrokerTradeIDForTradeClass = GetDeskAdmin(D)
                    Exit Function
                End If
            End If
        Next
    End Function

    Public Function GetDeskFullName(ByRef DeskClass As ACCOUNT_DESK_CLASS) As String
        GetDeskFullName = DeskClass.DESK_DESCR
        Dim ACClass As ACCOUNT_CLASS = GetAccountClass(DeskClass.ACCOUNT_ID)
        If IsNothing(ACClass) Then Exit Function
        GetDeskFullName = ACClass.SHORT_NAME & ", " & GetDeskFullName
    End Function

    Public Function MarketMatching(ByRef gdb As DB_ARTB_NETDataContext, _
                                   ByRef o As ORDERS_FFA_CLASS) As Collection
        MarketMatching = Nothing

        Dim BuyOrder As Boolean = False
        Dim oRouteID As Integer = o.ROUTE_ID
        Dim oMM1 As Integer = o.MM1
        Dim oMM2 As Integer = o.MM2
        Dim oYY1 As Integer = o.YY1
        Dim oYY2 As Integer = o.YY2

        Dim TCS As String = ""

        Dim rc = From r In gdb.ROUTEs Join v In gdb.VESSEL_CLASSes On r.VESSEL_CLASS_ID Equals v.VESSEL_CLASS_ID _
                 Where r.ROUTE_ID = oRouteID _
                 Select v

        For Each v In rc
            TCS = v.DRYWET
        Next

        Dim d As Integer = DateTime.UtcNow.Day()
        Dim m As Integer = DateTime.UtcNow.Month()
        Dim y As Integer = DateTime.UtcNow.Year()

        Dim ol = From q In gdb.ORDERS_FFAs Join s In gdb.DESK_TRADERs _
            On q.FOR_DESK_TRADER_ID Equals s.DESK_TRADER_ID _
            Join r In gdb.ROUTEs On q.ROUTE_ID Equals r.ROUTE_ID _
            Join v In gdb.VESSEL_CLASSes On r.VESSEL_CLASS_ID Equals v.VESSEL_CLASS_ID _
             Where (q.LIVE_STATUS = "A" _
                     And q.PRICE_TYPE = "F") _
                     And v.DRYWET = TCS _
                     And ((q.ORDER_DATETIME.Day = d _
                     And q.ORDER_DATETIME.Month = m _
                     And q.ORDER_DATETIME.Year = y) Or q.ORDER_GOOD_TILL = OrderGoodTill.GTC) _
             Order By q.ORDER_DATETIME _
             Select q, s.ACCOUNT_DESK_ID, r.SETTLEMENT_TICK, r.LOT_SIZE, r.PRICING_TICK

        Dim OrderCollection As New Collection

        RefCol.Clear()

        For Each co In ol
            Dim oc As New ORDERS_FFA_CLASS
            oc.GetFromObject(co.q)
            OrderCollection.Add(oc, oc.ORDER_ID.ToString())
            If oc.ORDER_TYPE <> OrderTypes.FFA Then Continue For
            Dim RPStr As String = RoutePeriodStringFromObj(oc)
            Dim RefC As REF_CLASS = GetViewClass(RefCol, RPStr)
            If IsNothing(RefC) Then
                RefC = New REF_CLASS(RPStr)
                RefCol.Add(RefC, RPStr)
            End If
            If NullInt2Int(oc.SPREAD_ORDER_ID) = 0 Then RefC.AssignOrderPrice(oc, co.PRICING_TICK)
        Next

        UpdateRefCol(gdb, TCS)

        Dim nc As New Collection
        Dim ExchangeList = New List(Of Integer)
        For Each co In ol
            If co.q.ORDER_BS <> "B" Then Continue For
            If co.q.ORDER_TYPE <> OrderTypes.FFA Then Continue For
            For Each co2 In ol
                If co.ACCOUNT_DESK_ID = co2.ACCOUNT_DESK_ID And co.ACCOUNT_DESK_ID <> 1113 Then Continue For
                If co2.q.ORDER_BS <> "S" Then Continue For
                If co2.q.ORDER_TYPE <> OrderTypes.FFA Then Continue For
                If MatchingOrders(gdb, co.q, co2.q, ExchangeList, , False) Then
                    For Each ExchangeId As Integer In ExchangeList
                        Dim nmoc As MARKET_MATCHING_CLASS = New MARKET_MATCHING_CLASS
                        nmoc.BuyOrder = GetViewClass(OrderCollection, co.q.ORDER_ID.ToString())
                        If Not IsNothing(nmoc.BuyOrder.SPREAD_ORDER_ID) Then
                            nmoc.BuyOrderSpread = GetViewClass(OrderCollection, nmoc.BuyOrder.SPREAD_ORDER_ID.ToString())
                            If Not IsNothing(nmoc.BuyOrderSpread) Then
                                If nmoc.BuyOrderSpread.PRICE_TYPE <> "F" Then
                                    nmoc = Nothing
                                    Continue For
                                End If
                            End If
                        End If
                        nmoc.SellOrder = GetViewClass(OrderCollection, co2.q.ORDER_ID.ToString())
                        If Not IsNothing(nmoc.SellOrder.SPREAD_ORDER_ID) Then
                            nmoc.SellOrderSpread = GetViewClass(OrderCollection, nmoc.SellOrder.SPREAD_ORDER_ID.ToString())
                            If Not IsNothing(nmoc.SellOrderSpread) Then
                                If nmoc.SellOrderSpread.PRICE_TYPE <> "F" Then
                                    nmoc = Nothing
                                    Continue For
                                End If
                            End If
                        End If

                        nmoc.ActualQuantity = 0
                        nmoc.ExchangeID = ExchangeId
                        nmoc.BuyExchangeRank = GetDeskExchangeRankingFromDB(gdb, co.ACCOUNT_DESK_ID, ExchangeId, nmoc.BuyOrder.ROUTE_ID)
                        nmoc.SellExchangeRank = GetDeskExchangeRankingFromDB(gdb, co2.ACCOUNT_DESK_ID, ExchangeId, nmoc.SellOrder.ROUTE_ID)
                        nmoc.Tick = co.SETTLEMENT_TICK
                        nmoc.LotSize = co.LOT_SIZE
                        If co.q.ORDER_TYPE = OrderTypes.RatioSpread Then nmoc.Tick = 0.001
                        nmoc.BuyRefPrice = nmoc.BuyOrder.PRICE_INDICATED
                        nmoc.SellRefPrice = nmoc.SellOrder.PRICE_INDICATED
                        Dim level As Integer = 0

                        nc.Add(nmoc)
                    Next
                End If
            Next

        Next
        If nc.Count = 0 Then
            Exit Function
        End If
        MarketMatching = MarketMIPOrders(gdb, nc, o.ORDER_ID, OrderCollection, MIPPrefBonus)
        RefCol.Clear()
        ' For Each noc As MARKET_MATCHING_CLASS In nc
        ' noc.BuyOrder = Nothing
        ' noc.SellOrder = Nothing
        ' noc = Nothing
        'Next
        nc = Nothing
    End Function

    Public Sub SetMarketDepthPrice(ByRef o As ORDERS_FFA_CLASS, _
                                    ByRef RPPLCol As Collection, _
                                    ByVal a_MArketDepthOrderID As Integer, _
                                    ByVal a_Tick As Double, _
                                    ByRef a_bDone As Boolean, _
                                    Optional ByVal a_MArketDepthOrderIDEnd As Integer = -1)
        If IsNothing(o) Then Exit Sub
        If a_MArketDepthOrderIDEnd <> -1 Then
            If o.ORDER_ID < a_MArketDepthOrderID Then Exit Sub
            If o.ORDER_ID > a_MArketDepthOrderIDEnd Then Exit Sub
        ElseIf o.ORDER_ID <> a_MArketDepthOrderID Then
            Exit Sub
        End If
        If IsNothing(RPPLCol) Then Exit Sub

        Dim RPStr1 As String = RoutePeriodStringFromObj(o, 1)
        Dim RPPL1 As RoutePeriodPriceLimits = GetViewClass(RPPLCol, RPStr1)
        If IsNothing(RPPL1) Then Exit Sub

        If o.ORDER_TYPE = OrderTypes.FFA Then
            If o.ORDER_BS = "B" Then
                o.PRICE_INDICATED = Round(RPPL1.High, a_Tick, , 1)
            Else
                o.PRICE_INDICATED = Round(RPPL1.Low, a_Tick, , -1)
            End If
        Else
            Dim RPStr2 As String = RoutePeriodStringFromObj(o, 2)
            Dim RPPL2 As RoutePeriodPriceLimits = GetViewClass(RPPLCol, RPStr2)
            Dim PriceLeg1 As Double = 0
            Dim PriceLeg2 As Double = 0
            If IsNothing(RPPL2) Then Exit Sub
            If o.ORDER_BS = "B" Then
                PriceLeg1 = Round(RPPL1.High, a_Tick, , 1)
                PriceLeg2 = Round(RPPL2.Low, a_Tick, , -1)
            Else
                PriceLeg1 = Round(RPPL1.Low, a_Tick, , -1)
                PriceLeg2 = Round(RPPL2.High, a_Tick, , 1)
            End If

            Select Case o.ORDER_TYPE
                Case OrderTypes.RatioSpread
                    If PriceLeg2 <> 0 Then
                        o.PRICE_INDICATED = PriceLeg1 / PriceLeg2
                    End If
                Case OrderTypes.PriceSpread, OrderTypes.CalendarSpread
                    o.PRICE_INDICATED = PriceLeg1 - PriceLeg2
            End Select
        End If

        If a_MArketDepthOrderIDEnd = -1 Then a_bDone = True

    End Sub

    Public Function CheckRoutePeriodBS(ByRef o1 As Object, ByRef o2 As Object, Optional ByVal bSameBs As Boolean = True) As Boolean
        If o1.ORDER_ID = o2.ORDER_ID Then Return False
        If o1.ORDER_TYPE <> o2.ORDER_TYPE Then Return False
        If bSameBs Then
            If o1.ORDER_BS <> o2.ORDER_BS Then Return False
        Else
            If o1.ORDER_BS = o2.ORDER_BS Then Return False
        End If
        If o1.ROUTE_ID <> o2.ROUTE_ID Then Return False
        If o1.MM1 <> o2.MM1 Then Return False
        If o1.MM2 <> o2.MM2 Then Return False
        If o1.YY1 <> o2.YY1 Then Return False
        If o1.YY2 <> o2.YY2 Then Return False
        If o1.ORDER_TYPE = OrderTypes.FFA Then Return True
        If o1.ROUTE_ID2 <> o2.ROUTE_ID2 Then Return False
        If o1.MM21 <> o2.MM21 Then Return False
        If o1.MM22 <> o2.MM22 Then Return False
        If o1.YY21 <> o2.YY21 Then Return False
        If o1.YY22 <> o2.YY22 Then Return False
        Return True
    End Function

    Public Function MarketMIPOrders(ByRef gdb As DB_ARTB_NETDataContext, _
                                    ByRef c As Collection, _
                                    ByVal PrefOrderId As Integer, _
                                    Optional ByRef OrderCollection As Collection = Nothing, _
                                    Optional ByVal PrefBonus As Double = 1, _
                                    Optional ByVal PrefOrderIdEnd As Integer = 0, _
                                    Optional ByVal bAdjustSpreads As Boolean = False, _
                                    Optional ByRef RPPLCol As Collection = Nothing, _
                                    Optional ByVal MarketDepthOrderID As Integer = -1, _
                                    Optional ByVal MarketDepthOrderIDEnd As Integer = -1) As Collection
        MarketMIPOrders = Nothing
        Dim n As Integer = c.Count
        If n <= 0 Then Exit Function
        If PrefOrderIdEnd = 0 Then PrefOrderIdEnd = PrefOrderId
        Dim lp As New LPProblem
        Dim StartTime As DateTime = DateTime.UtcNow

        Dim q As Integer, j As Double, k As Double, t As Double
        Dim dt As DateTime, ts As TimeSpan

        Dim o As Object, order As Object
        Dim MOC As New Collection
        Dim qTick As Double = 1
        'Spread constrains handling
        Dim SpreadCollection As New Collection
        Dim CurrConstr As Integer = -1
        Dim BuySpreadLeg As Integer = -1
        Dim MinPrice As Double = 0.0001
        Dim VarName As String = "", VarDescr = ""
        Dim QCount As Integer = 1
        Dim CurrDateTime As Date = Date.UtcNow
        Dim CurrDT As DateTime = CurrDateTime.Date
        Dim TotalMaxCap As Double = 0
        CurrDT = DateAdd(DateInterval.Hour, 2, CurrDT)
        MOC.Clear()

        If IsNothing(RPPLCol) Then
            RPPLCol = New Collection
            If bAdjustSpreads Then
                If SP(gdb, OrderCollection, RPPLCol, PrefOrderId, PrefOrderIdEnd) = False Then Exit Function
            Else
                Call SP(gdb, OrderCollection, RPPLCol, PrefOrderId, PrefOrderIdEnd, False)
            End If
        End If

        Dim ExchangeFactor As Double = 0.001
        Dim nPrefOrders As Integer = 0
        MIPSchema(gdb, c)
        Dim bMarkerDepthPriceAdjusted As Boolean = True
        If MarketDepthOrderID <> -1 Then bMarkerDepthPriceAdjusted = False
        Dim bNoPrefs As Boolean = False
        For Each oc As MARKET_MATCHING_CLASS In c
            If bMarkerDepthPriceAdjusted = False Then
                SetMarketDepthPrice(oc.BuyOrder, RPPLCol, MarketDepthOrderID, oc.Tick, bMarkerDepthPriceAdjusted, MarketDepthOrderIDEnd)
                SetMarketDepthPrice(oc.SellOrder, RPPLCol, MarketDepthOrderID, oc.Tick, bMarkerDepthPriceAdjusted, MarketDepthOrderIDEnd)
                SetMarketDepthPrice(oc.BuyOrderSpread, RPPLCol, MarketDepthOrderID, oc.Tick, bMarkerDepthPriceAdjusted, MarketDepthOrderIDEnd)
                SetMarketDepthPrice(oc.SellOrderSpread, RPPLCol, MarketDepthOrderID, oc.Tick, bMarkerDepthPriceAdjusted, MarketDepthOrderIDEnd)
            End If

            If Not IsNothing(oc.BuyOrderSpread) Then
                If oc.BuyOrderSpread.ORDER_BS = "B" Then
                    oc.BQ = Int(oc.BuyOrderSpread.GetActualQuantity(1))
                Else
                    oc.BQ = Int(oc.BuyOrderSpread.GetActualQuantity(2))
                End If
            Else
                oc.BQ = Int(GetActualQuantity(oc.BuyOrder))
            End If
            If 0 = oc.BQ Then Continue For

            If Not IsNothing(oc.SellOrderSpread) Then
                If oc.SellOrderSpread.ORDER_BS = "S" Then
                    oc.SQ = Int(oc.SellOrderSpread.GetActualQuantity(1))
                Else
                    oc.SQ = Int(oc.SellOrderSpread.GetActualQuantity(2))
                End If
            Else
                oc.SQ = Int(GetActualQuantity(oc.SellOrder))
            End If
            If 0 = oc.SQ Then Continue For

            oc.MOB = GetViewClass(MOC, oc.BuyOrder.ORDER_ID.ToString())
            If IsNothing(oc.MOB) Then
                oc.MOB = New MARKET_ORDER_CLASS(oc.BuyOrder, oc.BQ, lp, oc.ExchangeID)
                MOC.Add(oc.MOB, oc.BuyOrder.ORDER_ID.ToString())
            End If
            oc.MOS = GetViewClass(MOC, oc.SellOrder.ORDER_ID.ToString())
            If IsNothing(oc.MOS) Then
                oc.MOS = New MARKET_ORDER_CLASS(oc.SellOrder, oc.SQ, lp, oc.ExchangeID)
                MOC.Add(oc.MOS, oc.SellOrder.ORDER_ID.ToString())
            End If
            oc.MOBX = GetViewClass(oc.MOB.Exchanges, oc.ExchangeID.ToString())
            If IsNothing(oc.MOBX) Then
                oc.MOBX = New MARKET_ORDER_EXCHANGE_CLASS(oc.MOB, lp, oc.ExchangeID)
                oc.MOB.Exchanges.Add(oc.MOBX, oc.ExchangeID.ToString())
            End If
            oc.MOSX = GetViewClass(oc.MOS.Exchanges, oc.ExchangeID.ToString())
            If IsNothing(oc.MOSX) Then
                oc.MOSX = New MARKET_ORDER_EXCHANGE_CLASS(oc.MOS, lp, oc.ExchangeID)
                oc.MOS.Exchanges.Add(oc.MOSX, oc.ExchangeID.ToString())
            End If

            If Not IsNothing(oc.BuyOrderSpread) Then
                If oc.BuyOrderSpread.ORDER_BS <> "B" Then oc.MOB.bValidForQStep = False
                oc.BuySpreadID = oc.BuyOrderSpread.ORDER_ID
                If Not SpreadCollection.Contains(oc.BuySpreadID.ToString) Then
                    oc.BuySpread = New MARKET_SPREAD_CLASS(oc.BuyOrderSpread, lp, MIPTopPrice)
                    Dim SQ2 As Integer = Int(GetActualQuantity(oc.BuyOrderSpread, 2))
                    Dim SQ1 As Integer = Int(GetActualQuantity(oc.BuyOrderSpread, 1))
                    oc.BuySpread.QuantityRatio = SQ2 / SQ1
                    oc.BuySpread.CalcActualQuantityRatio(SQ1, SQ2)
                    oc.BuySpread.CalendarQModifier *= oc.BuySpread.QuantityRatio
                    lp.Rows(oc.BuySpread.TotalExecRow).BoundsHigh *= SQ1 * oc.BuySpread.QuantityRatio
                    lp.Rows(oc.BuySpread.TotalExecRow).BoundsLow *= SQ1 * oc.BuySpread.QuantityRatio
                    SpreadCollection.Add(oc.BuySpread, oc.BuySpreadID.ToString)
                Else
                    oc.BuySpread = SpreadCollection(oc.BuySpreadID.ToString)
                End If
            End If
            If Not IsNothing(oc.SellOrderSpread) Then
                oc.SellSpreadID = oc.SellOrderSpread.ORDER_ID
                If oc.SellOrderSpread.ORDER_BS <> "S" Then oc.MOS.bValidForQStep = False
                If Not SpreadCollection.Contains(oc.SellSpreadID.ToString) Then
                    oc.SellSpread = New MARKET_SPREAD_CLASS(oc.SellOrderSpread, lp, MIPTopPrice)
                    Dim SQ2 As Integer = Int(GetActualQuantity(oc.SellOrderSpread, 2))
                    Dim SQ1 As Integer = Int(GetActualQuantity(oc.SellOrderSpread, 1))
                    oc.SellSpread.QuantityRatio = SQ2 / SQ1
                    oc.SellSpread.CalcActualQuantityRatio(SQ1, SQ2)
                    oc.SellSpread.CalendarQModifier *= oc.SellSpread.QuantityRatio
                    lp.Rows(oc.SellSpread.TotalExecRow).BoundsHigh *= SQ1 * oc.SellSpread.QuantityRatio
                    lp.Rows(oc.SellSpread.TotalExecRow).BoundsLow *= SQ1 * oc.SellSpread.QuantityRatio
                    SpreadCollection.Add(oc.SellSpread, oc.SellSpreadID.ToString)
                Else
                    oc.SellSpread = SpreadCollection(oc.SellSpreadID.ToString)
                End If
            End If

            oc.FixPreferd(PrefOrderId, PrefOrderIdEnd)
            If oc.Prefered <> 0 Then
                nPrefOrders += 1
            End If

            Dim bdt As DateTime = oc.BuyOrder.ORDER_DATETIME
            Dim sdt As DateTime = oc.SellOrder.ORDER_DATETIME
            Dim BuyXMultip As Double = TotalExchanges
            Dim SellXMultip As Double = 1
            If sdt < bdt Or oc.SellPrefered Then Swap(BuyXMultip, SellXMultip)
            If oc.BuyPrefered Then
                If Not bAdjustSpreads Then bdt = CurrDT
                If Not oc.SellPrefered And Not bAdjustSpreads Then oc.SellExchangeRank = oc.BuyExchangeRank
                'If Not oc.SellPrefered Then oc.SellExchangeRank = oc.BuyExchangeRank
            End If
            If oc.SellPrefered Then
                If Not bAdjustSpreads Then sdt = CurrDT
                If Not oc.BuyPrefered And Not bAdjustSpreads Then oc.BuyExchangeRank = oc.SellExchangeRank
                'If Not oc.BuyPrefered Then oc.BuyExchangeRank = oc.SellExchangeRank
            End If

            ts = CurrDateTime - sdt
            t = ts.TotalMilliseconds
            If Not IsNothing(oc.SellSpread) Then oc.SellSpread.OrderTime = t
            oc.SellTime = t - ExchangeFactor * BuyXMultip * oc.SellExchangeRank
            ts = CurrDateTime - bdt
            t = ts.TotalMilliseconds
            If Not IsNothing(oc.BuySpread) Then oc.BuySpread.OrderTime = t
            oc.BuyTime = t - ExchangeFactor * SellXMultip * oc.BuyExchangeRank
            oc.TotalTime = oc.BuyTime + oc.SellTime
        Next

        If nPrefOrders = 0 Then
            bNoPrefs = True
        End If
        Dim i As Integer

        i = -1
        Dim cb = From ocb In c Order By ocb.BuyTime Select ocb
        Dim PrevRank As Double = 0
        For Each ocb As MARKET_MATCHING_CLASS In cb
            If ocb.BuyTime <> PrevRank Then
                i = i + 1
            Else
                i = i
            End If
            ocb.BuyRank = i
            PrevRank = ocb.BuyTime
        Next
        Dim nTotalBuyRanks As Integer = i + 1

        'For Each oc1 As MARKET_MATCHING_CLASS In c
        '    For Each oc2 As MARKET_MATCHING_CLASS In c
        '        If oc2.BuyOrder.ORDER_ID = oc1.BuyOrder.ORDER_ID Then Continue For
        '        If CheckRoutePeriodBS(oc1.BuyOrder, oc2.BuyOrder) = False Then Continue For
        '        If IsNothing(oc1.BuySpread) And IsNothing(oc2.BuySpread) Then
        '            If (oc1.BuyOrder.PRICE_INDICATED - oc2.BuyOrder.PRICE_INDICATED) * (oc1.BuyRank - oc2.BuyRank) < 0 Then
        '                Swap(oc1.BuyRank, oc2.BuyRank)
        '            End If
        '        ElseIf Not IsNothing(oc1.BuySpread) And Not IsNothing(oc2.BuySpread) Then
        '            If CheckRoutePeriodBS(oc1.BuySpread.SpreadOrder, oc2.BuySpread.SpreadOrder) = False Then Continue For
        '            If (oc1.BuySpread.SpreadOrder.PRICE_INDICATED - oc2.BuySpread.SpreadOrder.PRICE_INDICATED) * (oc1.BuyRank - oc2.BuyRank) < 0 Then
        '                Swap(oc1.BuyRank, oc2.BuyRank)
        '            End If
        '        End If
        '    Next
        'Next


        i = -1
        PrevRank = 0
        Dim cs = From ocs In c Order By ocs.SellTime Select ocs
        For Each ocs As MARKET_MATCHING_CLASS In cs
            If ocs.SellTime <> PrevRank Then
                i = i + 1
            Else
                i = i
            End If
            ocs.SellRank = i
            PrevRank = ocs.SellTime
        Next
        Dim nTotalSellRanks As Integer = i + 1

        'For Each oc1 As MARKET_MATCHING_CLASS In c
        '    For Each oc2 As MARKET_MATCHING_CLASS In c
        '        If oc2.SellOrder.ORDER_ID = oc1.SellOrder.ORDER_ID Then Continue For
        '        If CheckRoutePeriodBS(oc1.SellOrder, oc2.SellOrder) = False Then Continue For
        '        If IsNothing(oc1.SellSpread) And IsNothing(oc2.SellSpread) Then
        '            If (oc2.SellOrder.PRICE_INDICATED - oc1.SellOrder.PRICE_INDICATED) * (oc1.SellRank - oc2.SellRank) < 0 Then
        '                Swap(oc1.SellRank, oc2.SellRank)
        '            End If
        '        ElseIf Not IsNothing(oc1.SellSpread) And Not IsNothing(oc2.SellSpread) Then
        '            If CheckRoutePeriodBS(oc1.SellSpread.SpreadOrder, oc2.SellSpread.SpreadOrder) = False Then Continue For
        '            If (oc2.SellSpread.SpreadOrder.PRICE_INDICATED - oc1.SellSpread.SpreadOrder.PRICE_INDICATED) * (oc1.SellRank - oc2.SellRank) < 0 Then
        '                Swap(oc1.SellRank, oc2.SellRank)
        '            End If
        '        End If
        '    Next
        'Next

        'For Each oc1 As MARKET_MATCHING_CLASS In c
        '    oc1.TotalTime = oc1.TotalTime * 0.000000001 + oc1.SellRank + oc1.BuyRank
        'Next

        i = -1
        PrevRank = 0
        Dim ct = From oct In c Order By oct.TotalTime Select oct
        For Each oct As MARKET_MATCHING_CLASS In ct
            If oct.TotalTime <> PrevRank Then
                i = i + 1
            Else
                i = i
            End If
            oct.TotalRank = i
            PrevRank = oct.TotalTime

        Next
        Dim nTotalRanks As Integer = i + 1

        i = -1
        PrevRank = 0
        Dim nTotalSpreads As Integer = SpreadCollection.Count
        Dim st = From s In SpreadCollection Order By s.OrderTime Select s
        For Each s As MARKET_SPREAD_CLASS In st
            If s.OrderTime <> PrevRank Then
                i = i + 1
            Else
                i = i
            End If
            s.SpreadRank = i
            PrevRank = s.OrderTime
        Next
        Dim nTotalSpreadRanks As Integer = i + 1

        If bAdjustSpreads Then
            For Each s As MARKET_SPREAD_CLASS In st
                If s.SpreadOrder.order_bs = "B" Then
                    s.OrderRank = s.SpreadOrder.price_indicated + 0.00001 * s.SpreadRank / nTotalSpreadRanks
                Else
                    s.OrderRank = -s.SpreadOrder.price_indicated + 0.00001 * s.SpreadRank / nTotalSpreadRanks
                End If
            Next

            i = -1
            PrevRank = 0
            Dim st2 = From s In SpreadCollection Order By s.OrderRank Select s
            For Each s As MARKET_SPREAD_CLASS In st2
                If s.OrderRank <> PrevRank Then
                    i = i + 1
                Else
                    i = i
                End If
                s.SpreadRank2 = i
                PrevRank = s.OrderRank
            Next
        End If

        Dim nTotalMathces As Integer = c.Count
        For Each oc As MARKET_MATCHING_CLASS In c
            If oc.SQ = 0 Then Continue For
            If oc.BQ = 0 Then Continue For
            q = Min(oc.BQ, oc.SQ) / oc.LotSize
            'j = 1
            'k = q
            'If oc.SellOrder.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Or _
            '   oc.BuyOrder.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
            j = q
            k = 1
            oc.Bucket = True
            'End If
            oc.MaxQuantinty = q

            oc.MaxVariableValue = j
            Dim qcoef As Double = (1 + oc.TotalRank / nTotalRanks) * 0.1 * oc.Tick
            'If oc.BuyPrefered Then
            '    qcoef += (TotalExchanges - oc.BuyExchangeRank) * ExchangeFactor
            'Else
            '    qcoef += (TotalExchanges - oc.BuyExchangeRank) * ExchangeFactor / 10
            'End If

            'If oc.SellPrefered Then
            '    qcoef += (TotalExchanges - oc.SellExchangeRank) * ExchangeFactor
            'Else
            '    qcoef += (TotalExchanges - oc.SellExchangeRank) * ExchangeFactor / 10
            'End If

            VarDescr = oc.BuyOrder.ORDER_ID.ToString & "(" & oc.BQ.ToString & ")<->" _
                      & oc.SellOrder.ORDER_ID.ToString & "(" & oc.SQ.ToString & ") @" & oc.ExchangeID
            oc.ID = QCount
            VarName = "Q" & QCount.ToString
            'If (oc.BuyOrder.ORDER_ID = PrefOrderId Or oc.BuySpreadID = PrefOrderId) Or oc.SellOrder.ORDER_ID = PrefOrderId Or oc.SellSpreadID = PrefOrderId Then qcoef *= PrefBonus
            If oc.Prefered Then
                qcoef += MIPTopPrice * oc.Tick
                'If bAdjustSpreads Then qcoef = MIPTopPrice * oc.Tick
            ElseIf bNoPrefs Then
                qcoef += MIPTopPrice * oc.Tick
            End If
            'Dim GainBonusB As Double = 1 + (oc.BuyRank * nTotalSellRanks + oc.SellRank) / (nTotalBuyRanks * nTotalSellRanks * oc.Tick * MIPTopPrice)
            'Dim GainBonusS As Double = 1 + (oc.SellRank * nTotalBuyRanks + oc.BuyRank) / (nTotalBuyRanks * nTotalSellRanks * oc.Tick * MIPTopPrice)
            Dim GainBonusB As Double = 1 + (oc.BuyRank) / (nTotalBuyRanks * oc.Tick * MIPTopPrice)
            Dim GainBonusS As Double = 1 + (oc.SellRank) / (nTotalSellRanks * oc.Tick * MIPTopPrice)
            Dim KCoef As Double = 0
            If oc.BuyPrefered And bAdjustSpreads Then
                KCoef = GainBonusS
                If Not IsNothing(oc.SellSpread) Then
                    If oc.SellSpread.SpreadOrder.ORDER_BS = "B" Then KCoef = -KCoef
                End If
            End If
            If oc.SellPrefered And bAdjustSpreads Then
                KCoef = -GainBonusB
                If Not IsNothing(oc.BuySpread) Then
                    If oc.BuySpread.SpreadOrder.ORDER_BS = "S" Then KCoef = -KCoef
                End If
            End If
            KCoef = 0
            oc.VariableEQ = lp.AddVariable(GLP_IV, _
                                           qcoef * k * oc.LotSize, _
                                           GLP_DB, 0, j, _
                                           VarName, "Q-" & VarDescr)
            oc.VariableBinEQ = lp.AddVariable(GLP_BV, -0.000001, GLP_DB, 0, 1, _
                                            "B" & VarName, "oc.BQ-" & VarDescr)

            oc.VariableECap = lp.AddVariable(GLP_CV, KCoef, GLP_DB, 0, MIPTopPrice * oc.MaxQuantinty * oc.LotSize, _
                                             "K" & QCount.ToString, "Cap-" & VarDescr)

            CurrConstr = lp.AddConstraint(GLP_UP, 0, 0, VarName & " GE")
            lp.AddElement(oc.VariableBinEQ, CurrConstr, 1)
            lp.AddElement(oc.VariableEQ, CurrConstr, -1)
            CurrConstr = lp.AddConstraint(GLP_LO, 0, 0, VarName & " LE")
            lp.AddElement(oc.VariableBinEQ, CurrConstr, 1)
            lp.AddElement(oc.VariableEQ, CurrConstr, -1 / j)

            oc.VariablePenalty = lp.AddVariable(GLP_BV, -(1 / nTotalRanks) * 0.01 * oc.Tick, GLP_DB, 0, 1, "PQ" & QCount.ToString, "Incomplete Penalty Variable " & VarDescr)
            CurrConstr = lp.AddConstraint(GLP_LO, 0, 0, "Do minimum or nothing")
            lp.AddElement(oc.VariablePenalty, CurrConstr, 1)
            lp.AddElement(oc.VariableEQ, CurrConstr, 1 / oc.MaxQuantinty)
            lp.AddElement(oc.VariableBinEQ, CurrConstr, -1)
            If oc.BuyOrder.FLEXIBLE_QUANTITY <> OrderFlexQuantinty.Bucket And _
                oc.SellOrder.FLEXIBLE_QUANTITY <> OrderFlexQuantinty.Bucket And _
                oc.BQ = oc.SQ Then
            End If

            Dim AdjPrice As Double = 0
            Dim BAdjPrice As Double = 0
            Dim SAdjPrice As Double = 0
            'GainBonusB = PrefBonus
            If oc.BuyPrefered Then
                'If bAdjustSpreads Then
                '    If Not IsNothing(oc.SellSpread) Then
                '        GainBonusB -= PrefBonus * (oc.SellSpread.SpreadRank2 / (nTotalSpreadRanks * oc.Tick * MIPTopPrice))
                '    Else
                '        GainBonusB -= PrefBonus * (GainBonusS - 1)
                '    End If
                'End If
                GainBonusB += PrefBonus
            End If
            If oc.SellPrefered Then
                'If bAdjustSpreads Then
                '    If Not IsNothing(oc.BuySpread) Then
                '        GainBonusS -= PrefBonus * (oc.BuySpread.SpreadRank2 / (nTotalSpreadRanks * oc.Tick * MIPTopPrice))
                '    Else
                '        GainBonusS -= PrefBonus * (GainBonusB - 1)
                '    End If
                'End If
                GainBonusS += PrefBonus
            End If

            TotalMaxCap += MIPTopPrice * oc.MaxQuantinty * oc.LotSize
            'GainBonusS = PrefBonus


            lp.AddElement(oc.VariableEQ, oc.MOB.TotQRow, k * oc.LotSize)
            lp.AddElement(oc.VariableEQ, oc.MOS.TotQRow, k * oc.LotSize)

            lp.AddElement(oc.VariableEQ, oc.MOBX.GERow, 1 * oc.LotSize)
            lp.AddElement(oc.VariableEQ, oc.MOSX.GERow, 1 * oc.LotSize)
            lp.AddElement(oc.VariableEQ, oc.MOBX.LERow, k / oc.BQ * oc.LotSize)
            lp.AddElement(oc.VariableEQ, oc.MOSX.LERow, k / oc.SQ * oc.LotSize)

            oc.FreePrice = True
            If Math.Abs(oc.MOB.Order.PRICE_INDICATED) >= MIPTopPrice Then
                'lp.AddElement(oc.VariableEQ, oc.MOB.CapRow, -k * MIPTopPrice * oc.LotSize)
            Else
                If IsNothing(oc.BuyOrderSpread) Then
                    lp.AddElement(oc.VariableEQ, oc.MOB.CapRow, -k * oc.MOB.Order.PRICE_INDICATED * oc.LotSize)
                    lp.AddElement(oc.VariableECap, oc.MOB.CapRow, oc.Tick)
                    oc.FreePrice = False
                End If
            End If

            If Math.Abs(oc.MOS.Order.PRICE_INDICATED) >= MIPTopPrice Then
                'lp.AddElement(oc.VariableEQ, oc.MOS.CapRow, -k * MinPrice * oc.LotSize)
            Else
                If IsNothing(oc.SellOrderSpread) Then
                    lp.AddElement(oc.VariableEQ, oc.MOS.CapRow, -k * oc.MOS.Order.PRICE_INDICATED * oc.LotSize)
                    lp.AddElement(oc.VariableECap, oc.MOS.CapRow, oc.Tick)
                    oc.FreePrice = False
                End If
            End If

            'If Math.Abs(oc.Price) < MIPTopPrice Then
            '    If Dir() = 1 Then
            '        CurrConstr = lp.AddConstraint(GLP_LO, 0, 0, VarName & " Price Buy Side ")
            '        lp.AddElement(oc.VariableEQ, CurrConstr, -k * oc.Price * oc.LotSize)
            '        lp.AddElement(oc.VariableECap, CurrConstr, oc.Tick)
            '    Else
            '        CurrConstr = lp.AddConstraint(GLP_UP, 0, 0, VarName & " Price Sell Side ")
            '        lp.AddElement(oc.VariableEQ, CurrConstr, -k * oc.Price * oc.LotSize)
            '        lp.AddElement(oc.VariableECap, CurrConstr, oc.Tick)
            '    End If
            'End If

            Dim SpreadLowP As Double = -1.0E+20, SpreadHighP As Double = 1.0E+20
            Dim RPStr As String = RoutePeriodStringFromObj(oc.MOB.Order)
            Dim RPPL As RoutePeriodPriceLimits = GetViewClass(RPPLCol, RPStr)
            Dim bFixAdjPrice As Boolean = False
            If AdjPrice = 0 Then bFixAdjPrice = True
            If Not IsNothing(RPPL) Then
                oc.LowPrice = Round(RPPL.Low, oc.Tick, , -1) ' - 0.000001
                oc.HighPrice = Round(RPPL.High, oc.Tick, , 1) ' + 0.000001

                If Not IsNothing(oc.BuySpread) Then
                    Dim iLeg As Integer = 1
                    If oc.BuySpread.SpreadOrder.ORDER_BS = "B" Then iLeg = 0

                    If oc.BuySpread.PriceLeg(iLeg) = -1 Then
                        oc.BuySpread.CreateAllPriceConstraints(lp, RPPLCol, Me)
                    End If
                    If oc.BuySpread.PriceLeg(iLeg) <> -1 Then oc.BuySpread.AddPriceConstraints(lp, oc, RPPL, iLeg)
                End If

                If Not IsNothing(oc.SellSpread) Then
                    Dim iLeg As Integer = 1
                    If oc.SellSpread.SpreadOrder.ORDER_BS = "S" Then iLeg = 0

                    If oc.SellSpread.PriceLeg(iLeg) = -1 Then
                        oc.SellSpread.CreateAllPriceConstraints(lp, RPPLCol, Me)
                    End If

                    If oc.SellSpread.PriceLeg(iLeg) <> -1 Then oc.SellSpread.AddPriceConstraints(lp, oc, RPPL, iLeg)
                End If


                If oc.LowPrice > oc.Tick And oc.LowPrice < oc.Tick * MIPTopPrice Then
                    CurrConstr = lp.AddConstraint(GLP_LO, 0, 0, VarName & " Price Low ")
                    lp.AddElement(oc.VariableEQ, CurrConstr, -k * oc.LowPrice * oc.LotSize)
                    lp.AddElement(oc.VariableECap, CurrConstr, oc.Tick)
                    oc.FreePrice = False
                    If bFixAdjPrice Then AdjPrice = oc.LowPrice
                End If
                If oc.HighPrice > oc.Tick And oc.HighPrice < oc.Tick * MIPTopPrice Then
                    CurrConstr = lp.AddConstraint(GLP_UP, 0, 0, VarName & " Price High")
                    lp.AddElement(oc.VariableEQ, CurrConstr, -k * oc.HighPrice * oc.LotSize)
                    lp.AddElement(oc.VariableECap, CurrConstr, oc.Tick)
                    oc.FreePrice = False
                    If bFixAdjPrice Then
                        If AdjPrice = oc.LowPrice Then
                            AdjPrice = (oc.LowPrice + oc.HighPrice) * 0.5
                        Else
                            AdjPrice = oc.HighPrice
                        End If
                    End If
                End If
            Else
                CurrConstr = lp.AddConstraint(GLP_UP, 0, 0, VarName & " Cap vs Q LE")
                lp.AddElement(oc.VariableECap, CurrConstr, oc.Tick)
                lp.AddElement(oc.VariableEQ, CurrConstr, -MIPTopPrice * oc.MaxQuantinty * oc.LotSize)
                CurrConstr = lp.AddConstraint(GLP_LO, 0, 0, VarName & " Cap vs Q GE")
                lp.AddElement(oc.VariableECap, CurrConstr, oc.Tick)
                lp.AddElement(oc.VariableEQ, CurrConstr, -oc.MaxQuantinty * oc.LotSize)
            End If

            Dim BuySpreadBonus As Double = 0
            Dim SellSpreadBonus As Double = 0
            If Math.Abs(oc.MOB.Order.PRICE_INDICATED) > oc.Tick And _
               Math.Abs(oc.MOB.Order.PRICE_INDICATED) < MIPTopPrice * oc.Tick And _
               IsNothing(oc.BuyOrderSpread) Then
                'AdjPrice = oc.MOB.Order.PRICE_INDICATED
                'BAdjPrice = oc.MOB.Order.PRICE_INDICATED
                oc.VariableEGain = lp.AddVariable(GLP_CV, GainBonusB, GLP_FR, _
                                                       -MIPTopPrice * oc.MaxQuantinty * oc.LotSize, _
                                                       MIPTopPrice * oc.MaxQuantinty * oc.LotSize, _
                                                     "BG" & QCount.ToString, "Buyer's Profit-" & VarDescr)
                CurrConstr = lp.AddConstraint(GLP_FX, 0, 0, VarName & " Gain Buy Constr")
                lp.AddElement(oc.VariableEQ, CurrConstr, -k * oc.MOB.Order.PRICE_INDICATED * oc.LotSize)
                lp.AddElement(oc.VariableECap, CurrConstr, oc.Tick)
                lp.AddElement(oc.VariableEGain, CurrConstr, 1)
            ElseIf Not IsNothing(oc.BuySpread) Then
                If oc.BuySpread.VariableGain = -1 Then
                    BuySpreadBonus = 1 + oc.BuySpread.SpreadRank / (nTotalSpreadRanks * oc.Tick * MIPTopPrice)
                    If oc.BuyPrefered Then BuySpreadBonus += PrefBonus
                    oc.BuySpread.CreateGain(lp, oc, BuySpreadBonus)
                End If
            End If

            If Math.Abs(oc.MOS.Order.PRICE_INDICATED) > oc.Tick And _
               Math.Abs(oc.MOS.Order.PRICE_INDICATED) < MIPTopPrice * oc.Tick And _
               IsNothing(oc.SellOrderSpread) Then
                'AdjPrice = oc.MOS.Order.PRICE_INDICATED
                'SAdjPrice = oc.MOS.Order.PRICE_INDICATED
                oc.VariableSGain = lp.AddVariable(GLP_CV, GainBonusS, GLP_FR, _
                                                  -MIPTopPrice * oc.MaxQuantinty * oc.LotSize, _
                                                  MIPTopPrice * oc.MaxQuantinty * oc.LotSize, _
                                                "SG" & QCount.ToString, "Seller's Profit-" & VarDescr)
                CurrConstr = lp.AddConstraint(GLP_FX, 0, 0, VarName & " Gain Sell Constr")
                lp.AddElement(oc.VariableEQ, CurrConstr, +k * oc.MOS.Order.PRICE_INDICATED * oc.LotSize)
                lp.AddElement(oc.VariableECap, CurrConstr, -oc.Tick)
                lp.AddElement(oc.VariableSGain, CurrConstr, 1)
                'If BAdjPrice <> SAdjPrice And BAdjPrice > 0 Then
                '    If GainBonusS > GainBonusB Then
                '        AdjPrice = SAdjPrice
                '    Else
                '        AdjPrice = BAdjPrice
                '    End If
                'End If
            ElseIf Not IsNothing(oc.SellSpread) Then
                If oc.SellSpread.VariableGain = -1 Then
                    SellSpreadBonus = 1 + oc.SellSpread.SpreadRank / (nTotalSpreadRanks * oc.Tick * MIPTopPrice)
                    If oc.SellPrefered Then SellSpreadBonus += PrefBonus
                    oc.SellSpread.CreateGain(lp, oc, SellSpreadBonus)
                End If
            End If

            Dim GainCoef As Double = -1 '-(MaxCoef - Min(GainBonusB, GainBonusS))

            If oc.FreePrice = True Then
                If IsNothing(oc.BuyOrderSpread) = False Then
                    If oc.BuyOrderSpread.ORDER_BS = oc.BuyOrder.ORDER_BS Then
                        If AdjPrice = 0 And oc.BuySpread.Leg1Price = 0 Then
                            AdjPrice = GetAvgReferencePrice(gdb, oc.BuyOrder, OrderCollection, , , False)
                        ElseIf AdjPrice = 0 Then
                            AdjPrice = oc.BuySpread.Leg1Price
                        End If
                        Dim bBuyAdj As Boolean = False

                        If AdjPrice > 0 And AdjPrice < MIPTopPrice Then bBuyAdj = True
                        If IsNothing(oc.SellOrderSpread) = False Then
                            If oc.SellOrderSpread.ORDER_BS <> oc.SellOrder.ORDER_BS Then bBuyAdj = False
                        End If
                        If False Then ' bBuyAdj Then
                            CurrConstr = lp.AddConstraint(GLP_UP, 0, 0, VarName & " Price Buy AvgRef ")
                            lp.AddElement(oc.VariableEQ, CurrConstr, -k * AdjPrice * oc.LotSize)
                            lp.AddElement(oc.VariableECap, CurrConstr, oc.Tick)
                            oc.FreePrice = False
                        End If
                    End If
                End If

                If IsNothing(oc.SellOrderSpread) = False Then
                    If oc.SellOrderSpread.ORDER_BS = oc.SellOrder.ORDER_BS Then
                        If AdjPrice = 0 And oc.SellSpread.Leg1Price = 0 Then
                            AdjPrice = GetAvgReferencePrice(gdb, oc.SellOrder, OrderCollection, , False)
                        ElseIf AdjPrice = 0 Then
                            AdjPrice = oc.SellSpread.Leg1Price
                        End If
                        Dim bSellAdj As Boolean = False

                        If AdjPrice > 0 And AdjPrice < MIPTopPrice Then bSellAdj = True
                        If IsNothing(oc.BuyOrderSpread) = False Then
                            If oc.BuyOrderSpread.ORDER_BS <> oc.BuyOrder.ORDER_BS Then bSellAdj = False
                        End If
                        If False Then ' bSellAdj Then
                            CurrConstr = lp.AddConstraint(GLP_LO, 0, 0, VarName & " Price Sell AvgRef ")
                            lp.AddElement(oc.VariableEQ, CurrConstr, -k * AdjPrice * oc.LotSize)
                            lp.AddElement(oc.VariableECap, CurrConstr, oc.Tick)
                            oc.FreePrice = False
                        End If
                    End If
                End If
            End If

            If AdjPrice > oc.Tick And AdjPrice < oc.Tick * MIPTopPrice Then
                If oc.VariableEGain = -1 Then
                    If IsNothing(oc.BuyOrderSpread) Then
                        oc.VariableEGain = lp.AddVariable(GLP_CV, GainBonusB, GLP_DB, _
                                              -MIPTopPrice * oc.MaxQuantinty, _
                                              MIPTopPrice * oc.MaxQuantinty, _
                                            "BG" & QCount.ToString, "2nd Round Buyer's Profit-" & VarDescr)
                        CurrConstr = lp.AddConstraint(GLP_FX, 0, 0, VarName & " 2nd Round Gain Buy Constr")
                        lp.AddElement(oc.VariableEQ, CurrConstr, -k * AdjPrice * oc.LotSize)
                        lp.AddElement(oc.VariableECap, CurrConstr, oc.Tick)
                        lp.AddElement(oc.VariableEGain, CurrConstr, 1)
                        BAdjPrice = AdjPrice
                        'ElseIf bAdjustSpreads Then
                        '    'If oc.BuySpread.SpreadOrder.ORDER_BS = "S" Then
                        '    'BuySpreadBonus = 1 + oc.BuySpread.SpreadRank2 / (nTotalSpreadRanks * MIPTopPrice * oc.Tick)
                        '    'Else
                        '    BuySpreadBonus = 1 + (nTotalSpreadRanks - oc.BuySpread.SpreadRank2 - 1) / (nTotalSpreadRanks * MIPTopPrice * oc.Tick)
                        '    'End If
                        '    oc.VariableEGain = lp.AddVariable(GLP_CV, BuySpreadBonus, GLP_DB, _
                        '                          -MIPTopPrice * oc.MaxQuantinty, _
                        '                          MIPTopPrice * oc.MaxQuantinty, _
                        '                        "BSG" & QCount.ToString, "2nd Round Buyer's Spread Profit-" & VarDescr)
                        '    CurrConstr = lp.AddConstraint(GLP_FX, 0, 0, VarName & "2nd Round Spread Gain Buy Constr")
                        '    lp.AddElement(oc.VariableEQ, CurrConstr, -k * AdjPrice * oc.LotSize)
                        '    lp.AddElement(oc.VariableECap, CurrConstr, oc.Tick)
                        '    lp.AddElement(oc.VariableEGain, CurrConstr, -1)
                        '    BAdjPrice = AdjPrice
                    End If
                End If

                If oc.VariableSGain = -1 Then
                    If IsNothing(oc.SellOrderSpread) Then
                        oc.VariableSGain = lp.AddVariable(GLP_CV, GainBonusS, GLP_DB, _
                              -MIPTopPrice * oc.MaxQuantinty, _
                              MIPTopPrice * oc.MaxQuantinty, _
                            "SG" & QCount.ToString, "Seller's Profit-" & VarDescr)
                        CurrConstr = lp.AddConstraint(GLP_FX, 0, 0, VarName & "2nd Round  Gain Sell Constr")
                        lp.AddElement(oc.VariableEQ, CurrConstr, +k * AdjPrice * oc.LotSize)
                        lp.AddElement(oc.VariableECap, CurrConstr, -oc.Tick)
                        lp.AddElement(oc.VariableSGain, CurrConstr, 1)
                        SAdjPrice = AdjPrice
                        'ElseIf bAdjustSpreads Then
                        '    'If oc.SellSpread.SpreadOrder.ORDER_BS = "S" Then
                        '    'SellSpreadBonus = 1 + oc.SellSpread.SpreadRank2 / (nTotalSpreadRanks * MIPTopPrice * oc.Tick)
                        '    'Else
                        '    SellSpreadBonus = 1 + (nTotalSpreadRanks - oc.SellSpread.SpreadRank2 - 1) / (nTotalSpreadRanks * MIPTopPrice * oc.Tick)
                        '    'End If
                        '    oc.VariableSGain = lp.AddVariable(GLP_CV, SellSpreadBonus, GLP_DB, _
                        '                                          -MIPTopPrice * oc.MaxQuantinty, _
                        '                                          MIPTopPrice * oc.MaxQuantinty, _
                        '                                        "SSG" & QCount.ToString, "Seller's Profit-" & VarDescr)
                        '    CurrConstr = lp.AddConstraint(GLP_FX, 0, 0, VarName & "2nd Round Spread Sell Constr")
                        '    lp.AddElement(oc.VariableEQ, CurrConstr, +k * AdjPrice * oc.LotSize)
                        '    lp.AddElement(oc.VariableECap, CurrConstr, -oc.Tick)
                        '    lp.AddElement(oc.VariableSGain, CurrConstr, -1)
                        '    SAdjPrice = AdjPrice
                    End If
                End If
            End If

            If oc.MOB.VariableX <> -1 Then
                lp.AddElement(oc.VariableEQ, oc.MOB.NonBucketFixQRow, k / oc.BQ * oc.LotSize)
            End If
            If oc.BuyOrder.QUANTITY_STEP > 1 And oc.MOB.VariableBucketX = -1 And oc.MOB.bValidForQStep Then
                Dim OrderName As String = oc.BuyOrder.ORDER_ID.ToString
                Dim MQ As Integer = oc.BQ / oc.BuyOrder.QUANTITY_STEP
                oc.MOB.VariableBucketX = lp.AddVariable(GLP_IV, 0, GLP_DB, 0, MQ, "QS" & OrderName, OrderName & " Bucket Step")
                oc.MOB.BucketStepRow = lp.AddConstraint(GLP_FX, 0, 0, OrderName & " Bucket Step")
                lp.AddElement(oc.MOB.VariableBucketX, oc.MOB.BucketStepRow, -oc.BuyOrder.QUANTITY_STEP / oc.BQ)
            ElseIf oc.BuyOrder.FLEXIBLE_QUANTITY <> OrderFlexQuantinty.Bucket And oc.MOB.VariableBucketX = -1 And oc.MOB.bValidForQStep Then
                Dim OrderName As String = oc.BuyOrder.ORDER_ID.ToString
                oc.MOB.VariableBucketX = lp.AddVariable(GLP_IV, 0, GLP_DB, 0, 1, "QS" & OrderName, OrderName & " Bucket Step")
                oc.MOB.BucketStepRow = lp.AddConstraint(GLP_FX, 0, 0, OrderName & " Bucket Step")
                lp.AddElement(oc.MOB.VariableBucketX, oc.MOB.BucketStepRow, -1)
            End If
            If oc.SellOrder.QUANTITY_STEP > 1 And oc.MOS.VariableBucketX = -1 And oc.MOS.bValidForQStep Then
                Dim OrderName As String = oc.SellOrder.ORDER_ID.ToString
                Dim MQ As Integer = oc.SQ / oc.SellOrder.QUANTITY_STEP
                oc.MOS.VariableBucketX = lp.AddVariable(GLP_IV, 0, GLP_DB, 0, MQ, "QS" & OrderName, OrderName & " Bucket Step")
                oc.MOS.BucketStepRow = lp.AddConstraint(GLP_FX, 0, 0, OrderName & " Bucket Step")
                lp.AddElement(oc.MOS.VariableBucketX, oc.MOS.BucketStepRow, -oc.SellOrder.QUANTITY_STEP / oc.SQ)
            ElseIf oc.SellOrder.FLEXIBLE_QUANTITY <> OrderFlexQuantinty.Bucket And oc.MOS.VariableBucketX = -1 And oc.MOS.bValidForQStep Then
                Dim OrderName As String = oc.SellOrder.ORDER_ID.ToString
                oc.MOS.VariableBucketX = lp.AddVariable(GLP_IV, 0, GLP_DB, 0, 1, "QS" & OrderName, OrderName & " Bucket Step")
                oc.MOS.BucketStepRow = lp.AddConstraint(GLP_FX, 0, 0, OrderName & " Bucket Step")
                lp.AddElement(oc.MOS.VariableBucketX, oc.MOS.BucketStepRow, -1)
            End If

            If oc.MOB.VariableBucketX <> -1 Then
                lp.AddElement(oc.VariableEQ, oc.MOB.BucketStepRow, k / oc.BQ * oc.LotSize)
            End If
            If oc.MOS.VariableX <> -1 Then
                lp.AddElement(oc.VariableEQ, oc.MOS.NonBucketFixQRow, k / oc.SQ * oc.LotSize)
            End If
            If oc.MOS.VariableBucketX <> -1 Then
                lp.AddElement(oc.VariableEQ, oc.MOS.BucketStepRow, k / oc.SQ * oc.LotSize)
            End If

            QCount = QCount + 1

            BuySpreadLeg = -1

            If Not IsNothing(oc.BuyOrderSpread) And Not IsNothing(oc.BuySpread) Then
                If oc.BuyOrder.ORDER_BS = oc.BuyOrderSpread.ORDER_BS Then
                    lp.AddElement(oc.VariableEQ, _
                                  oc.BuySpread.LegQuantinyBalanceRow, _
                                  k * oc.BuySpread.QuantityRatio * oc.LotSize)
                    lp.AddElement(oc.VariableEQ, _
                                  oc.BuySpread.TotalExecRow, _
                                   k * oc.BuySpread.QuantityRatio * oc.LotSize)
                    If oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.RatioSpread Then
                        lp.AddElement(oc.VariableECap, _
                                      oc.BuySpread.PriceRow, _
                                      oc.Tick)
                        lp.AddElement(oc.VariableECap, _
                                      oc.BuySpread.GainRow, _
                                      oc.Tick)
                        'ElseIf oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.CalendarSpread Then
                        '    lp.AddElement(oc.VariableECap, _
                        '                  oc.BuySpread.PriceRow, _
                        '                  oc.Tick)
                        '    lp.AddElement(oc.VariableEQ, _
                        '                  oc.BuySpread.PriceRow, _
                        '                  -k * oc.BuySpread.CalendarQModifier * oc.LotSize)
                    ElseIf oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.PriceSpread Or oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.CalendarSpread Then
                        lp.AddElement(oc.VariableECap, _
                                      oc.BuySpread.PriceRow, _
                                      oc.Tick * oc.BuySpread.QuantityRatio)
                        lp.AddElement(oc.VariableEQ, _
                                      oc.BuySpread.PriceRow, _
                                      -k * oc.BuySpread.PriceQModifier * oc.LotSize * oc.BuySpread.QuantityRatio)
                        lp.AddElement(oc.VariableECap, _
                                      oc.BuySpread.GainRow, _
                                      oc.Tick * oc.BuySpread.QuantityRatio)
                        lp.AddElement(oc.VariableEQ, _
                                      oc.BuySpread.GainRow, _
                                      -k * oc.BuySpread.PriceQModifier * oc.LotSize * oc.BuySpread.QuantityRatio)
                    End If
                    'If IsNothing(oc.sellOrderSpread) Then
                    '    lp.AddElement(oc.VariableECap, _
                    '                  oc.BuyOrderSpread.SwapSpreadPriceBalanceRow, _
                    '                  k)
                    'Else
                    '    lp.AddElement(oc.VariableECap, _
                    '                  oc.BuyOrderSpread.SwapSpreadPriceBalanceRow, _
                    '                  -k)
                    'End If
                Else
                    lp.AddElement(oc.VariableEQ, _
                                  oc.BuySpread.LegQuantinyBalanceRow, _
                                  -k * oc.LotSize)
                    If oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.RatioSpread Then
                        lp.AddElement(oc.VariableECap, _
                                      oc.BuySpread.PriceRow, _
                                      -oc.Tick * oc.BuyOrderSpread.PRICE_INDICATED / oc.BuySpread.QuantityRatio)
                        lp.AddElement(oc.VariableECap, _
                                      oc.BuySpread.GainRow, _
                                      -oc.Tick * oc.BuyOrderSpread.PRICE_INDICATED / oc.BuySpread.QuantityRatio)
                    Else
                        lp.AddElement(oc.VariableECap, _
                                      oc.BuySpread.PriceRow, _
                                      -oc.Tick)
                        lp.AddElement(oc.VariableECap, _
                                      oc.BuySpread.GainRow, _
                                      -oc.Tick)
                    End If
                End If

            End If
            If Not IsNothing(oc.SellOrderSpread) And Not IsNothing(oc.SellSpread) Then
                If oc.SellOrder.ORDER_BS = oc.SellOrderSpread.ORDER_BS Then
                    lp.AddElement(oc.VariableEQ, _
                                  oc.SellSpread.LegQuantinyBalanceRow, _
                                  k * oc.SellSpread.QuantityRatio * oc.LotSize)
                    lp.AddElement(oc.VariableEQ, _
                                  oc.SellSpread.TotalExecRow, _
                                   k * oc.SellSpread.QuantityRatio * oc.LotSize)
                    If oc.SellOrderSpread.ORDER_TYPE = OrderTypes.RatioSpread Then
                        lp.AddElement(oc.VariableECap, _
                                      oc.SellSpread.PriceRow, _
                                      oc.Tick)
                        lp.AddElement(oc.VariableECap, _
                                      oc.SellSpread.GainRow, _
                                      oc.Tick)
                        'ElseIf oc.SellOrderSpread.ORDER_TYPE = OrderTypes.CalendarSpread Then
                        '    lp.AddElement(oc.VariableECap, _
                        '                  oc.SellSpread.PriceRow, _
                        '                  oc.Tick)
                        '    lp.AddElement(oc.VariableEQ, _
                        '                  oc.SellSpread.PriceRow, _
                        '                  -k * oc.SellSpread.CalendarQModifier * oc.LotSize)
                    ElseIf oc.SellOrderSpread.ORDER_TYPE = OrderTypes.PriceSpread Or oc.SellOrderSpread.ORDER_TYPE = OrderTypes.CalendarSpread Then
                        lp.AddElement(oc.VariableECap, _
                                      oc.SellSpread.PriceRow, _
                                      oc.Tick * oc.SellSpread.QuantityRatio)
                        lp.AddElement(oc.VariableEQ, _
                                      oc.SellSpread.PriceRow, _
                                      -k * oc.SellSpread.PriceQModifier * oc.LotSize * oc.SellSpread.QuantityRatio)
                        lp.AddElement(oc.VariableECap, _
                                      oc.SellSpread.GainRow, _
                                      oc.Tick * oc.SellSpread.QuantityRatio)
                        lp.AddElement(oc.VariableEQ, _
                                      oc.SellSpread.GainRow, _
                                      -k * oc.SellSpread.PriceQModifier * oc.LotSize * oc.SellSpread.QuantityRatio)
                    End If
                    'If IsNothing(oc.BuyOrderSpread) Then
                    '    lp.AddElement(oc.VariableECap, _
                    '                  oc.sellOrderSpread.SwapSpreadPriceBalanceRow, _
                    '                  k)
                    'Else
                    '    lp.AddElement(oc.VariableECap, _
                    '                  oc.sellOrderSpread.SwapSpreadPriceBalanceRow, _
                    '                  -k)
                    'End If

                Else
                    lp.AddElement(oc.VariableEQ, oc.SellSpread.LegQuantinyBalanceRow, -k * oc.LotSize)
                    If oc.SellOrderSpread.ORDER_TYPE = OrderTypes.RatioSpread Then
                        lp.AddElement(oc.VariableECap, _
                                      oc.SellSpread.PriceRow, _
                                      -oc.Tick * oc.SellOrderSpread.PRICE_INDICATED / oc.SellSpread.QuantityRatio)
                        lp.AddElement(oc.VariableECap, _
                                      oc.SellSpread.GainRow, _
                                      -oc.Tick * oc.SellOrderSpread.PRICE_INDICATED / oc.SellSpread.QuantityRatio)
                    Else
                        lp.AddElement(oc.VariableECap, _
                                      oc.SellSpread.PriceRow, _
                                      -oc.Tick)
                        lp.AddElement(oc.VariableECap, _
                                      oc.SellSpread.GainRow, _
                                      -oc.Tick)
                    End If
                End If
            End If

        Next

        ' 2ND PASS
        'Used to add constraint between leg & spread execution
        Dim TotalExecVar As Integer
        TotalExecVar = lp.AddVariable(GLP_BV, TotalMaxCap, GLP_DB, 0, 1, "T", "Trades Executed")
        Dim TotalExecConstrLE As Integer = lp.AddConstraint(GLP_UP, 0, 0, "Trades Executed LE")
        Dim TotalExecConstrGE As Integer = lp.AddConstraint(GLP_LO, 0, 0, "Trades Executed GE")

        lp.AddElement(TotalExecVar, TotalExecConstrLE, 1)
        lp.AddElement(TotalExecVar, TotalExecConstrGE, 1)

        For Each oc As MARKET_MATCHING_CLASS In c
            If oc.VariableBinEQ = -1 Then Continue For
            If oc.Prefered <> 0 Then
                lp.AddElement(oc.VariableBinEQ, TotalExecConstrGE, -1 / nPrefOrders)
                lp.AddElement(oc.VariableBinEQ, TotalExecConstrLE, -1)
            End If

            If bAdjustSpreads Then
            End If
            'If bAdjustSpreads Then
            '    If oc.SellPrefered And Not IsNothing(oc.BuySpread) Then
            '        For Each oc2 As MARKET_MATCHING_CLASS In c
            '            If oc2.SellPrefered = False Then Continue For
            '            If CheckRoutePeriodBS(oc.BuyOrder, oc2.BuyOrder) = False Then Continue For
            '            If IsNothing(oc2.BuySpread) Then Continue For
            '            If CheckRoutePeriodBS(oc.BuySpread.SpreadOrder, oc2.BuySpread.SpreadOrder) = False Then Continue For
            '            If (oc2.BuySpread.SpreadOrder.PRICE_INDICATED < oc.BuySpread.SpreadOrder.PRICE_INDICATED) Then
            '                If oc2.BuySpread.SpreadOrder.ORDER_BS = "B" Then
            '                    CurrConstr = lp.AddConstraint(GLP_LO, 0, 0, oc.BuySpread.SpreadOrder.ORDER_ID.ToString() & "-" & oc2.BuySpread.SpreadOrder.ORDER_ID.ToString() & " Cap Reg")
            '                Else
            '                    CurrConstr = lp.AddConstraint(GLP_UP, 0, 0, oc.BuySpread.SpreadOrder.ORDER_ID.ToString() & "-" & oc2.BuySpread.SpreadOrder.ORDER_ID.ToString() & " Cap Reg")
            '                End If
            '                lp.AddElement(oc.VariableECap, CurrConstr, 1 / oc.MaxQuantinty)
            '                lp.AddElement(oc2.VariableECap, CurrConstr, -1 / oc.MaxQuantinty)
            '            End If
            '        Next
            '    End If

            '    If oc.BuyPrefered And Not IsNothing(oc.SellSpread) Then
            '        For Each oc2 As MARKET_MATCHING_CLASS In c
            '            If oc2.BuyPrefered = False Then Continue For
            '            If CheckRoutePeriodBS(oc.SellOrder, oc2.SellOrder) = False Then Continue For
            '            If IsNothing(oc2.SellSpread) Then Continue For
            '            If CheckRoutePeriodBS(oc.SellSpread.SpreadOrder, oc2.SellSpread.SpreadOrder) = False Then Continue For
            '            If (oc2.SellSpread.SpreadOrder.PRICE_INDICATED > oc.SellSpread.SpreadOrder.PRICE_INDICATED) Then
            '                If oc2.SellSpread.SpreadOrder.ORDER_BS = "S" Then
            '                    CurrConstr = lp.AddConstraint(GLP_UP, 0, 0, oc.SellSpread.SpreadOrder.ORDER_ID.ToString() & "-" & oc2.SellSpread.SpreadOrder.ORDER_ID.ToString() & " Cap Reg")
            '                Else
            '                    CurrConstr = lp.AddConstraint(GLP_LO, 0, 0, oc.SellSpread.SpreadOrder.ORDER_ID.ToString() & "-" & oc2.SellSpread.SpreadOrder.ORDER_ID.ToString() & " Cap Reg")
            '                End If
            '                lp.AddElement(oc.VariableECap, CurrConstr, 1 / oc.MaxQuantinty)
            '                lp.AddElement(oc2.VariableECap, CurrConstr, -1 / oc.MaxQuantinty)
            '            End If
            '        Next
            '    End If
            'End If

            'oc.BuySpreadID = 0
            'oc.SellSpreadID = 0
            'k = oc.MaxQuantinty
            'If oc.Bucket Then k = 1
            'If Not IsNothing(oc.BuyOrder) Then
            '    oc.BuySpreadID = oc.BuyOrder.ORDER_ID
            '    If SpreadCollection.Contains(oc.BuySpreadID.ToString) Then
            '        oc.BuyOrderSpread = SpreadCollection(oc.BuySpreadID.ToString)
            '        lp.AddElement(oc.VariableEQ, oc.BuySpread.TotalExecRow, k * oc.BuySpread.QuantityRatio * oc.LotSize)
            '    End If
            'End If
            'If Not IsNothing(oc.SellOrder) Then
            '    oc.SellSpreadID = oc.SellOrder.ORDER_ID
            '    If SpreadCollection.Contains(oc.SellSpreadID.ToString) Then
            '        oc.SellOrderSpread = SpreadCollection(oc.SellSpreadID.ToString)
            '        lp.AddElement(oc.VariableEQ, oc.SellSpread.TotalExecRow, k * oc.SellSpread.QuantityRatio * oc.LotSize)
            '    End If
            'End If


            'Dim SPXRow As Integer = -1
            'For Each oc2 As MARKET_MATCHING_CLASS In c
            '    If oc2.VariableBinEQ = oc.VariableBinEQ Then Continue For
            '    If oc2.ExchangeID = oc.ExchangeID Then Continue For
            '    If oc2.BuyOrder.ORDER_ID <> oc.BuyOrder.ORDER_ID Then Continue For
            '    If oc2.SellOrder.ORDER_ID <> oc.SellOrder.ORDER_ID Then Continue For
            '    If -1 = SPXRow Then
            '        SPXRow = lp.AddConstraint(GLP_DB, 0, 1, "Single Pair Exchange " & _
            '                                  oc.BuyOrder.ORDER_ID.ToString & " <-> " & oc.SellOrder.ORDER_ID.ToString)
            '        lp.AddElement(oc.VariableBinEQ, SPXRow, 1)
            '    End If
            '    lp.AddElement(oc2.VariableBinEQ, SPXRow, 1)
            'Next

            If Not IsNothing(oc.BuySpread) Then
                If oc.BuySpread.bSXXRow = False Then
                    For Each oc2 As MARKET_MATCHING_CLASS In c
                        If IsNothing(oc2.SellSpread) Then Continue For
                        If Not oc.BuySpread.Equals(oc2.SellSpread) Then Continue For
                        lp.AddElement(oc.MOB.VariableExchange, oc.BuySpread.SXXRow, 1)
                        lp.AddElement(oc2.MOS.VariableExchange, oc.BuySpread.SXXRow, -1)
                        oc.BuySpread.bSXXRow = True
                        Exit For
                    Next
                End If
            End If
            If Not IsNothing(oc.SellSpread) Then
                If oc.SellSpread.bSXXRow = False Then
                    For Each oc2 As MARKET_MATCHING_CLASS In c
                        If IsNothing(oc2.BuySpread) Then Continue For
                        If Not oc.SellSpread.Equals(oc2.BuySpread) Then Continue For
                        lp.AddElement(oc.MOS.VariableExchange, oc.SellSpread.SXXRow, 1)
                        lp.AddElement(oc2.MOB.VariableExchange, oc.SellSpread.SXXRow, -1)
                        oc.SellSpread.bSXXRow = True
                        Exit For
                    Next
                End If
            End If

        Next

        If lp.ColumnsNum <= 1 Then
            lp.Destroy()
            Exit Function
        End If
        For Each mo As MARKET_ORDER_CLASS In MOC
            mo.FinalizeConstaints(lp)
        Next

        Dim pass As Integer = 0
HandleSolution:
        pass += 1
        lp.Report()
        If lp.Solve > -1.0E+20 Then

            For Each mo As MARKET_ORDER_CLASS In MOC
                'Debug.Print("OrderId:" & mo.Order.ORDER_ID.ToString())
                If mo.VariableX <> -1 Then Debug.Print(lp.GetVariableValue(mo.VariableX).ToString())
                For Each ex As MARKET_ORDER_EXCHANGE_CLASS In mo.Exchanges
                    'Debug.Print("Exchange:" & ex.ExchangeID.ToString() & " ," & lp.GetVariableValue(ex.VariableEX).ToString())
                Next
            Next

            For i = 1 To lp.ColumnsNum - 1
                Dim vx As Double = lp.GetVariableValue(i)
                If Len(lp.Columns(i).Name) > 0 Then
                    Debug.Print(lp.Columns(i).Name & ": " & vx & " x " & lp.Columns(i).Coef & " = " & Format(vx * lp.Columns(i).Coef))
                Else
                    Debug.Print("V" & i & ": " & vx & " x " & lp.Columns(i).Coef & " = " & Format(vx * lp.Columns(i).Coef))
                End If
            Next

            If bAdjustSpreads And pass = 1 And False Then
                Dim TotalPrefCap As Double = 0
                Dim TotalPrefQ As Double = 0
                For Each oc As MARKET_MATCHING_CLASS In c
                    If oc.Prefered Then
                        TotalPrefQ += lp.GetVariableValue(oc.VariableEQ)
                        TotalPrefCap += lp.GetVariableValue(oc.VariableECap)
                    End If
                Next

                Dim QConstr As Integer = lp.AddConstraint(GLP_FX, TotalPrefQ, TotalPrefQ, "Adj Spreads 2nd Pass Q")
                Dim CapConstr As Integer = lp.AddConstraint(GLP_FX, TotalPrefCap, TotalPrefCap, "Adj Spreads 2nd Pass Q")
                QCount = 0
                Dim BonusMultiplier As Double = 10
                For Each oc As MARKET_MATCHING_CLASS In c
                    QCount += 1
                    VarDescr = oc.BuyOrder.ORDER_ID.ToString & "(" & oc.BQ.ToString & ")<->" _
                                          & oc.SellOrder.ORDER_ID.ToString & "(" & oc.SQ.ToString & ") @" & oc.ExchangeID
                    oc.ID = QCount
                    VarName = "Q" & QCount.ToString
                    lp.Columns(oc.VariableEQ).Coef = 0
                    Dim PQCoef As Double = MIPTopPrice
                    PQCoef = -10000000000.0

                    If bAdjustSpreads And oc.Prefered Then
                        If oc.BuyPrefered And Not IsNothing(oc.SellSpread) Then
                            oc.SellSpread.PreferedSpread = True

                        End If
                        If oc.SellPrefered And Not IsNothing(oc.BuySpread) Then
                            oc.BuySpread.PreferedSpread = True
                        End If
                    End If

                    lp.Columns(oc.VariablePenalty).Coef = PQCoef
                    If oc.Prefered = False Then

                        If oc.VariableEGain <> -1 Then lp.Columns(oc.VariableEGain).Coef = 1
                        If oc.VariableSGain <> -1 Then lp.Columns(oc.VariableSGain).Coef = 1
                        Continue For
                    End If
                    lp.AddElement(oc.VariableEQ, QConstr, 1)
                    lp.AddElement(oc.VariableECap, CapConstr, 1)
                    Dim cc As Integer
                    cc = lp.AddConstraint(GLP_FX, _
                                     lp.GetVariableValue(oc.VariableEQ), _
                                     lp.GetVariableValue(oc.VariableEQ), "Adj Spreads Fix Non Spreads")
                    lp.AddElement(oc.VariableEQ, cc, 1)

                    Dim cq As Double = lp.GetVariableValue(oc.VariableEQ)
                    If cq = 0 Then Continue For
                    'Dim AvgPrice As Double = lp.GetVariableValue(oc.VariableECap) / cq
                    Dim AvgPrice As Double = 0.5 * (oc.LowPrice + oc.HighPrice)
                    If AvgPrice < oc.Tick Or AvgPrice > MIPTopPrice * oc.Tick Then Continue For

                    If oc.BuyPrefered Then
                        lp.Columns(oc.VariableEGain).Coef = PrefBonus
                        If Not IsNothing(oc.SellSpread) And oc.VariableSGain = -1 And oc.BuyPrefered Then
                            Dim SellSpreadBonus As Double = 1
                            If oc.SellSpread.SpreadOrder.ORDER_BS = "2S" Then
                                'SellSpreadBonus = 1 + oc.SellSpread.SpreadRank2 / (nTotalSpreadRanks * MIPTopPrice * oc.Tick)
                                SellSpreadBonus = BonusMultiplier + oc.SellSpread.SpreadRank2 * BonusMultiplier
                            Else
                                'SellSpreadBonus = 1 + (nTotalSpreadRanks - oc.SellSpread.SpreadRank2 - 1) / (nTotalSpreadRanks * MIPTopPrice * oc.Tick)
                                SellSpreadBonus = BonusMultiplier + (nTotalSpreadRanks - oc.SellSpread.SpreadRank2 - 1) * BonusMultiplier
                            End If
                            SellSpreadBonus *= -PrefBonus
                            oc.VariableSGain = lp.AddVariable(GLP_CV, SellSpreadBonus, GLP_DB, _
                                  -MIPTopPrice * oc.MaxQuantinty, _
                                  MIPTopPrice * oc.MaxQuantinty, _
                                "SSG" & QCount.ToString, "Seller's Profit-" & VarDescr)
                            CurrConstr = lp.AddConstraint(GLP_FX, 0, 0, VarName & " 2nd Round Spread Sell Constr")

                            lp.AddElement(oc.VariableEQ, CurrConstr, +k * AvgPrice * oc.LotSize)

                            lp.AddElement(oc.VariableECap, CurrConstr, -oc.Tick)
                            lp.AddElement(oc.VariableSGain, CurrConstr, -1)
                        ElseIf oc.VariableSGain <> -1 Then
                            lp.Columns(oc.VariableSGain).Coef = 1
                        End If
                    End If

                    If oc.SellPrefered Then
                        lp.Columns(oc.VariableSGain).Coef = PrefBonus
                        If Not IsNothing(oc.BuySpread) And oc.VariableEGain = -1 Then
                            Dim BuySpreadBonus As Double = 1
                            If oc.BuySpread.SpreadOrder.ORDER_BS = "1B" Then
                                'BuySpreadBonus = 1 + oc.BuySpread.SpreadRank2 / (nTotalSpreadRanks * MIPTopPrice * oc.Tick)
                                BuySpreadBonus = BonusMultiplier + oc.BuySpread.SpreadRank2 * oc.BuySpread.SpreadRank2
                            Else
                                'BuySpreadBonus = 1 + (nTotalSpreadRanks - oc.BuySpread.SpreadRank2 - 1) / (nTotalSpreadRanks * MIPTopPrice * oc.Tick)
                                BuySpreadBonus = BonusMultiplier + (nTotalSpreadRanks - oc.BuySpread.SpreadRank2 - 1) * BonusMultiplier
                            End If
                            BuySpreadBonus *= -PrefBonus
                            oc.VariableEGain = lp.AddVariable(GLP_CV, BuySpreadBonus, GLP_DB, _
                                                  -MIPTopPrice * oc.MaxQuantinty, _
                                                  MIPTopPrice * oc.MaxQuantinty, _
                                                "BSG" & QCount.ToString, "2nd Round Buyer's Spread Profit-" & VarDescr)
                            CurrConstr = lp.AddConstraint(GLP_FX, 0, 0, VarName & "2nd Round Spread Gain Buy Constr")
                            lp.AddElement(oc.VariableEQ, CurrConstr, -k * AvgPrice * oc.LotSize)
                            lp.AddElement(oc.VariableECap, CurrConstr, oc.Tick)
                            lp.AddElement(oc.VariableEGain, CurrConstr, -1)
                        ElseIf oc.VariableEGain <> -1 Then
                            lp.Columns(oc.VariableEGain).Coef = 1
                        End If
                    End If

                    If (oc.BuyPrefered And IsNothing(oc.SellSpread)) Or (oc.SellPrefered And IsNothing(oc.BuySpread)) Then
                        cc = lp.AddConstraint(GLP_FX, _
                                                             lp.GetVariableValue(oc.VariableECap), _
                                                             lp.GetVariableValue(oc.VariableECap), "Adj Spreads Fix Non Spreads")
                        lp.AddElement(oc.VariableECap, cc, 1)
                    End If

                Next

                For Each SP As MARKET_SPREAD_CLASS In SpreadCollection
                    If SP.VariableGain <> -1 Then
                        lp.Columns(SP.VariableGain).Coef = 1
                        Dim cc As Integer = lp.AddConstraint(GLP_FX, lp.GetVariableValue(SP.VariableGain), lp.GetVariableValue(SP.VariableGain), "Adj Spreads Fix Spreads")
                        lp.AddElement(SP.VariableGain, cc, 1)
                    End If
                Next

                'For Each oc As MARKET_MATCHING_CLASS In c
                '    If Not IsNothing(oc.BuySpread) Then
                '        If oc.BuySpread.PreferedSpread = True Then
                '            CurrConstr = lp.AddConstraint(GLP_FX, 0, 0, "Prefered Spell")
                '            lp.AddElement(oc.VariableEQ, CurrConstr, 1)
                '            lp.AddElement(oc.VariableBinEQ, CurrConstr, -oc.MaxQuantinty)
                '        End If
                '    End If
                '    If Not IsNothing(oc.SellSpread) Then
                '        If oc.SellSpread.PreferedSpread = True Then
                '            CurrConstr = lp.AddConstraint(GLP_FX, 0, 0, "Prefered Spell")
                '            lp.AddElement(oc.VariableEQ, CurrConstr, 1)
                '            lp.AddElement(oc.VariableBinEQ, CurrConstr, -oc.MaxQuantinty)
                '        End If
                '    End If
                'Next
                GoTo HandleSolution
            End If

            Dim nc As New Collection
            Dim bTotalFreePrices As Boolean = False
            nc = New Collection
            For Each oc As MARKET_MATCHING_CLASS In c
                If oc.VariableEQ < 0 Then Continue For
                k = lp.GetVariableValue(oc.VariableEQ)
                If k > 0 Then
                    If oc.MaxVariableValue > 1 Then
                        oc.ActualQuantity = k * oc.LotSize
                    Else
                        oc.ActualQuantity = oc.MaxQuantinty * oc.LotSize
                    End If
                    If oc.VariableECap >= 0 Then
                        Dim cap As Double = lp.GetVariableValue(oc.VariableECap) * oc.Tick
                        oc.Price = cap / oc.ActualQuantity
                        If Not IsNothing(oc.BuySpread) AndAlso _
                           Not IsNothing(oc.SellSpread) AndAlso _
                           Not IsNothing(oc.BuyOrderSpread) AndAlso _
                           Not IsNothing(oc.SellOrderSpread) AndAlso _
                           oc.BuySpread.SpreadOrder.ORDER_TYPE = oc.SellSpread.SpreadOrder.ORDER_TYPE Then
                            'bTotalFreePrices = True
                            'oc.FreePrice = True
                        Else
                            If Not IsNothing(oc.BuySpread) Then
                                If oc.BuyOrder.ORDER_BS = oc.BuySpread.SpreadOrder.ORDER_BS Then
                                    oc.BuySpread.Leg1Q += oc.ActualQuantity
                                    oc.BuySpread.Leg1Price += cap
                                Else
                                    oc.BuySpread.Leg2Q += oc.ActualQuantity
                                    oc.BuySpread.Leg2Price += cap
                                End If
                            End If
                            If Not IsNothing(oc.SellSpread) Then
                                If oc.SellOrder.ORDER_BS = oc.SellSpread.SpreadOrder.ORDER_BS Then
                                    oc.SellSpread.Leg1Q += oc.ActualQuantity
                                    oc.SellSpread.Leg1Price += cap
                                Else
                                    oc.SellSpread.Leg2Q += oc.ActualQuantity
                                    oc.SellSpread.Leg2Price += cap
                                End If
                            End If
                        End If
                    End If
                    nc.Add(oc)
                End If
            Next

            MIPExecuted(gdb, nc)

            If bTotalFreePrices Then
                For Each sc As MARKET_SPREAD_CLASS In SpreadCollection
                    If sc.Leg1Q > 0 Then
                        sc.Leg1Price /= sc.Leg1Q
                    End If
                    If sc.Leg2Q > 0 Then
                        sc.Leg2Price /= sc.Leg2Q
                    End If
                Next

                For i = 0 To 1
                    For Each oc As MARKET_MATCHING_CLASS In nc
                        If oc.FreePrice AndAlso Not IsNothing(oc.BuySpread) AndAlso _
                           Not IsNothing(oc.SellSpread) AndAlso _
                           Not IsNothing(oc.BuyOrderSpread) AndAlso _
                           Not IsNothing(oc.SellOrderSpread) Then
                            If oc.BuyOrderSpread.ORDER_BS = oc.BuyOrder.ORDER_BS _
                            And oc.BuySpread.Leg1Price <> 0 Then
                                If oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.RatioSpread Then
                                    oc.BuySpread.SpreadPriceAdjust = oc.Price / oc.BuySpread.Leg1Price
                                ElseIf oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.CalendarSpread Or oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.PriceSpread Then
                                    oc.BuySpread.SpreadPriceAdjust = oc.Price - oc.BuySpread.Leg1Price
                                End If
                                oc.Price = Round(oc.BuySpread.Leg1Price, oc.Tick)
                                oc.FreePrice = False
                            ElseIf oc.SellOrderSpread.ORDER_BS = oc.SellOrder.ORDER_BS _
                            And oc.SellSpread.Leg1Price <> 0 Then
                                If oc.SellOrderSpread.ORDER_TYPE = OrderTypes.RatioSpread Then
                                    oc.SellSpread.SpreadPriceAdjust = oc.Price / oc.SellSpread.Leg1Price
                                ElseIf oc.SellOrderSpread.ORDER_TYPE = OrderTypes.CalendarSpread Or oc.SellOrderSpread.ORDER_TYPE = OrderTypes.PriceSpread Then
                                    oc.SellSpread.SpreadPriceAdjust = oc.Price - oc.SellSpread.Leg1Price
                                End If
                                oc.Price = Round(oc.SellSpread.Leg1Price, oc.Tick)
                                oc.FreePrice = False
                            ElseIf oc.BuyOrderSpread.ORDER_BS <> oc.BuyOrder.ORDER_BS _
                            And oc.BuySpread.SpreadPriceAdjust <> 0 Then
                                If oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.RatioSpread Then
                                    oc.Price /= oc.BuySpread.SpreadPriceAdjust
                                ElseIf oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.CalendarSpread Or oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.PriceSpread Then
                                    oc.Price -= oc.BuySpread.SpreadPriceAdjust
                                End If
                                oc.Price = Round(oc.Price, oc.Tick)
                                oc.FreePrice = False
                            ElseIf oc.SellOrderSpread.ORDER_BS <> oc.SellOrder.ORDER_BS _
                            And oc.SellSpread.SpreadPriceAdjust <> 0 Then
                                If oc.SellOrderSpread.ORDER_TYPE = OrderTypes.RatioSpread Then
                                    oc.Price /= oc.SellSpread.SpreadPriceAdjust
                                ElseIf oc.SellOrderSpread.ORDER_TYPE = OrderTypes.CalendarSpread Or oc.SellOrderSpread.ORDER_TYPE = OrderTypes.PriceSpread Then
                                    oc.Price -= oc.SellSpread.SpreadPriceAdjust
                                End If
                                oc.Price = Round(oc.Price, oc.Tick)
                                oc.FreePrice = False
                            Else
                                If oc.SellOrderSpread.ORDER_BS = oc.SellOrder.ORDER_BS _
                                And oc.SellSpread.Leg1Price = 0 Then
                                    oc.SellSpread.Leg1Price = GetAvgReferencePrice(gdb, oc.SellOrder, OrderCollection, oc.Price, , False)
                                    If oc.SellSpread.Leg1Price <> 0 Then
                                        If oc.SellOrderSpread.ORDER_TYPE = OrderTypes.RatioSpread Then
                                            oc.SellSpread.SpreadPriceAdjust = oc.Price / oc.SellSpread.Leg1Price
                                        ElseIf oc.SellOrderSpread.ORDER_TYPE = OrderTypes.CalendarSpread Or oc.SellOrderSpread.ORDER_TYPE = OrderTypes.PriceSpread Then
                                            oc.SellSpread.SpreadPriceAdjust = oc.Price - oc.SellSpread.Leg1Price
                                        End If
                                        oc.Price = oc.SellSpread.Leg1Price
                                        oc.Price = Round(oc.Price, oc.Tick)
                                        oc.FreePrice = False
                                    End If
                                End If

                                If oc.FreePrice = True _
                                And oc.BuyOrderSpread.ORDER_BS = oc.BuyOrder.ORDER_BS _
                                And oc.BuySpread.Leg1Price = 0 Then
                                    oc.BuySpread.Leg1Price = GetAvgReferencePrice(gdb, oc.BuyOrder, OrderCollection, oc.Price, , False)
                                    If oc.BuySpread.Leg1Price <> 0 Then
                                        If oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.RatioSpread Then
                                            oc.BuySpread.SpreadPriceAdjust = oc.Price / oc.BuySpread.Leg1Price
                                        ElseIf oc.BuyOrderSpread.ORDER_TYPE = OrderTypes.CalendarSpread Or oc.SellOrderSpread.ORDER_TYPE = OrderTypes.PriceSpread Then
                                            oc.BuySpread.SpreadPriceAdjust = oc.Price - oc.BuySpread.Leg1Price
                                        End If
                                        oc.Price = oc.BuySpread.Leg1Price
                                        oc.Price = Round(oc.Price, oc.Tick)
                                        oc.FreePrice = False
                                    End If
                                End If
                            End If
                        End If
                    Next
                Next
            End If
            MarketMIPOrders = nc '  MIPFixPrices(nc, PrefOrderId)

        End If

#If DEBUG Then
        Dim EndTime As DateTime = DateTime.UtcNow
        Dim Ts1 As TimeSpan = EndTime - StartTime
        Debug.Print("MIP Start Time: " & StartTime.ToString())
        Debug.Print("MIP End   Time: " & EndTime.ToString())
        Debug.Print("MIP Handl Time: " & Ts1.TotalMilliseconds.ToString())
#End If

        lp.Destroy()
    End Function

    Public Function MIPOrders(ByRef order As Object, _
                              ByRef c As Collection, _
                              ByRef Rankings As Collection, _
                              Optional ByVal bReadFromDB As Boolean = True) As Collection
        MIPOrders = Nothing
        Dim n As Integer = c.Count
        If n <= 0 Then Exit Function
        Dim lp As New LPProblem
        Dim sign As Double = 1
        Dim i As Integer = 1, ii As Integer = 1
        Dim v1 As Integer, vStart As Integer = -1
        Dim q As Integer, j As Double, k As Double, t As Double
        Dim dt As DateTime, ts As TimeSpan
        Dim coef As Double
        Dim o As Object
        Dim TotQRow As Integer, SXXRow As Integer = -1
        Dim ox1() As Boolean
        'Dim ox2() As Boolean
        Dim XArray() As ORDER_EXCHANGE_MATCHING_CLASS
        Dim orderQ As Integer = Int(GetActualQuantity(order))
        Dim dOrderQ As Double = orderQ
        Dim kQ As Double
        Dim SelectedExchangeId As Integer = -1
        If order.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
            TotQRow = lp.AddConstraint(GLP_DB, 0.0, orderQ)
        Else
            TotQRow = lp.AddConstraint(GLP_FX, orderQ, orderQ)
        End If
        If order.SINGLE_EXCHANGE_EXECUTION Then
            SXXRow = lp.AddConstraint(GLP_FX, 1.0, 1.0)
        Else
            SXXRow = lp.AddConstraint(GLP_UP, 0)
        End If
        ReDim ox1(TotalExchanges + 1)

        GetOrderExchangesFromStr(order, ox1)
        ReDim XArray(TotalExchanges + 1)
        For i = 1 To TotalExchanges
            If ox1(i) Then
                XArray(i) = New ORDER_EXCHANGE_MATCHING_CLASS(SXXRow, lp, i)
            Else
                XArray(i) = Nothing
            End If
        Next

        For Each oc As MATCHING_ORDERS_CLASS In c
            o = oc.OrderClass
            q = Int(GetActualQuantity(o))
            dt = o.ORDER_DATETIME
            ts = DateTime.UtcNow - dt
            t = ts.TotalMilliseconds / 1000000000.0
            j = 1
            k = q
            If o.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
                j = q
                k = 1
            End If
            kQ = k / dOrderQ
            If order.ORDER_BS = "B" Then
                coef = (order.PRICE_INDICATED - o.PRICE_INDICATED) * k + t * k
            Else
                coef = (o.PRICE_INDICATED - order.PRICE_INDICATED) * k + t * k
            End If

            For Each i In oc.Exchanges
                Dim rank As Integer
                rank = NullInt2Int(GetViewClass(Rankings, i.ToString()))
                v1 = lp.AddVariable(GLP_IV, coef + (TotalExchanges - rank) * 0.001, GLP_DB, 0.0, j)

                If vStart = -1 Then vStart = v1
                lp.AddElement(v1, TotQRow, k)

                lp.AddElement(v1, XArray(i).GERow, 1)
                lp.AddElement(v1, XArray(i).LERow, kQ)
            Next
        Next
        If lp.Solve > -1.0E+20 Then
            ii = vStart

            MIPOrders = New Collection
            For Each oc As MATCHING_ORDERS_CLASS In c
                o = oc.OrderClass
                For Each i In oc.Exchanges
                    k = lp.GetVariableValue(ii)
                    If k > 0 Then
                        If TypeOf (oc.OrderClass) Is ORDERS_FFA Then
                            oc.OrderClass = New ORDERS_FFA_CLASS
                            oc.OrderClass.GetFromObject(o)
                        End If
                        q = Int(GetActualQuantity(oc.OrderClass))
                        If oc.OrderClass.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
                            oc.ActualQuantity = k
                            q = k
                        Else
                            oc.ActualQuantity = q
                        End If
                        Dim noc As New MATCHING_ORDERS_CLASS
                        noc.OrderClass = oc.OrderClass
                        noc.ActualQuantity = oc.ActualQuantity
                        noc.Exchanges = New List(Of Integer)
                        noc.Exchanges.Add(i)
                        MIPOrders.Add(noc)
                    End If
                    ii = ii + 1
                Next
            Next

            For i = 1 To TotalExchanges
                If Not IsNothing(XArray(i)) Then XArray(i) = Nothing
            Next
        End If
        lp.Destroy()
    End Function

    Public Function DayOrdersExpireInDB(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        DayOrdersExpireInDB = ArtBErrors.Success

        Dim l = From q In gdb.ORDERS_FFAs _
                  Where q.ORDER_GOOD_TILL <> OrderGoodTill.GTC And _
                        q.LIVE_STATUS <> "D" And q.LIVE_STATUS <> "E" _
                    Order By q.ORDER_ID _
                Select q

        For Each q As ORDERS_FFA In l
            q.LIVE_STATUS = "D"
        Next

        Try
            gdb.SubmitChanges()
        Catch ex As Exception
            DayOrdersExpireInDB = ArtBErrors.SubmitChangesFailed
        End Try
    End Function

    Public Function GetMainDeskClass(ByVal a_ACCOUNT_ID As Integer) As ACCOUNT_DESK_CLASS
        GetMainDeskClass = Nothing
        Dim AccountDeskList = From q In ACCOUNT_DESKS _
                              Where q.ACCOUNT_ID = a_ACCOUNT_ID _
                              Select q

        For Each q As ACCOUNT_DESK_CLASS In AccountDeskList
            If q.DESK_QUALIFIER <> 0 Then
                GetMainDeskClass = q
                Exit Function
            End If
        Next
    End Function

    Public Function GetAccount(ByVal a_ACCOUNT_ID As Integer, _
                               Optional ByVal bIsClient As Boolean = True) As Integer
        Dim gdb As DB_ARTB_NETDataContext = GetNewConnection()

        GetAccount = ArtBErrors.Success
        Dim CONTACT As CONTACT_CLASS
        Dim ContactList = (From d In gdb.ACCOUNT_DESKs _
                              Join t In gdb.DESK_TRADERs On d.ACCOUNT_DESK_ID Equals t.ACCOUNT_DESK_ID _
                              Join q In gdb.CONTACTs On q.CONTACT_ID Equals t.CONTACT_ID _
                              Where d.ACCOUNT_ID = a_ACCOUNT_ID _
                              Select q).ToList

        For Each qc In ContactList
            CONTACT = New CONTACT_CLASS
            CONTACT.GetFromObject(qc)
            InsertOrReplace(CONTACTS, CONTACT, CONTACT.CONTACT_ID.ToString)
        Next

        Dim ACCOUNT As ACCOUNT_CLASS
        Dim l = (From q In gdb.ACCOUNTs _
                Where (q.ACCOUNT_ID = a_ACCOUNT_ID) _
                Select q).ToList

        For Each qa In l
            ACCOUNT = New ACCOUNT_CLASS
            ACCOUNT.GetFromObject(qa)
            ACCOUNT.DESKS.Clear()
            InsertOrReplace(ACCOUNTS, ACCOUNT, qa.ACCOUNT_ID.ToString())
        Next

        Dim DESK As ACCOUNT_DESK_CLASS
        Dim AccountDeskList = (From q In gdb.ACCOUNT_DESKs Where q.ACCOUNT_ID = a_ACCOUNT_ID _
                  Select q).ToList
        For Each qd In AccountDeskList
            DESK = New ACCOUNT_DESK_CLASS
            DESK.GetFromObject(qd)
            InsertOrReplace(ACCOUNT_DESKS, DESK, DESK.ACCOUNT_DESK_ID.ToString())
            ACCOUNT = GetAccountClass(qd.ACCOUNT_ID)
            If Not (ACCOUNT Is Nothing) Then
                InsertOrReplace(ACCOUNT.DESKS, DESK, DESK.ACCOUNT_DESK_ID.ToString())
            End If
            DESK.TRADERS.Clear()
            DESK.COUNTER_PARTY_LIMITS.Clear()
        Next

        Dim TRADER As DESK_TRADER_CLASS
        Dim tl = (From q In gdb.DESK_TRADERs _
                 Join d In gdb.ACCOUNT_DESKs On q.ACCOUNT_DESK_ID Equals d.ACCOUNT_DESK_ID _
                 Where d.ACCOUNT_ID = a_ACCOUNT_ID Select q).ToList
        For Each qt In tl
            TRADER = New DESK_TRADER_CLASS
            TRADER.GetFromObject(qt)
            TRADER.CONTACT_DETAILS = GetContactClass(TRADER.CONTACT_ID)
            DESK = GetAccountDeskClass(TRADER.ACCOUNT_DESK_ID)
            If Not (DESK Is Nothing) Then
                InsertOrReplace(DESK.TRADERS, TRADER, TRADER.DESK_TRADER_ID.ToString())
            End If
            InsertOrReplace(DESK_TRADERS_BY_OF, TRADER, TRADER.OF_ID)
            InsertOrReplace(DESK_TRADERS, TRADER, TRADER.DESK_TRADER_ID.ToString())
        Next

        If bIsClient = False Then
            gdb = Nothing
            Exit Function
        End If

        Dim de As DESK_TRADE_CLASS_CLASS
        Dim tcl = (From tc In gdb.DESK_TRADE_CLASSes _
                  Join d In gdb.ACCOUNT_DESKs On tc.ACCOUNT_DESK_ID Equals d.ACCOUNT_DESK_ID _
                  Where d.ACCOUNT_ID = a_ACCOUNT_ID Select tc).ToList

        For Each qtc In tcl
            de = New DESK_TRADE_CLASS_CLASS
            de.GetFromObject(qtc)
            DESK = GetAccountDeskClass(de.ACCOUNT_DESK_ID)
            de.EXCHANGES.Clear()
            If Not IsNothing(DESK) Then
                InsertOrReplace(DESK.TRADE_CLASSES, de, de.TRADE_CLASS_SHORT)
            End If
        Next

        Dim xl = (From x In gdb.DESK_EXCHANGEs _
           Join tc In gdb.DESK_TRADE_CLASSes On tc.TRADE_CLASS_SHORT Equals x.TRADE_CLASS_SHORT And _
                                           tc.ACCOUNT_DESK_ID Equals x.ACCOUNT_DESK_ID _
           Join d In gdb.ACCOUNT_DESKs On tc.ACCOUNT_DESK_ID Equals d.ACCOUNT_DESK_ID _
           Where d.ACCOUNT_ID = a_ACCOUNT_ID Select x).ToList

        Dim DX As DESK_EXCHANGE_CLASS
        For Each qx In xl
            DX = New DESK_EXCHANGE_CLASS
            DX.GetFromObject(qx)
            de = GetDeskTradeClassClass(DX.ACCOUNT_DESK_ID, DX.TRADE_CLASS_SHORT)
            DX.CLEARERS.Clear()
            If Not (de Is Nothing) Then
                InsertOrReplace(de.EXCHANGES, DX, DX.EXCHANGE_ID.ToString())
            End If
        Next

        Dim dec As DESK_EXCHANGES_CLEARER_CLASS
        Dim cl = (From c In gdb.DESK_EXCHANGES_CLEARERs _
                   Join x In gdb.DESK_EXCHANGEs On c.EXCHANGE_ID Equals x.EXCHANGE_ID And _
                                                   c.TRADE_CLASS_SHORT Equals x.TRADE_CLASS_SHORT And _
                                                   c.ACCOUNT_DESK_ID Equals x.ACCOUNT_DESK_ID _
                   Join tc In gdb.DESK_TRADE_CLASSes On tc.TRADE_CLASS_SHORT Equals x.TRADE_CLASS_SHORT And _
                                                   tc.ACCOUNT_DESK_ID Equals x.ACCOUNT_DESK_ID _
                   Join d In gdb.ACCOUNT_DESKs On tc.ACCOUNT_DESK_ID Equals d.ACCOUNT_DESK_ID _
                   Where d.ACCOUNT_ID = a_ACCOUNT_ID Select c).ToList

        For Each qcl In cl
            dec = New DESK_EXCHANGES_CLEARER_CLASS
            dec.GetFromObject(qcl)
            de = GetDeskTradeClassClass(dec.ACCOUNT_DESK_ID, dec.TRADE_CLASS_SHORT)
            If Not (de Is Nothing) Then
                DX = GetViewClass(de.EXCHANGES, dec.EXCHANGE_ID.ToString())
                If Not IsNothing(DX) Then
                    InsertOrReplace(DX.CLEARERS, dec, dec.ACCOUNT_ID.ToString())
                End If
            End If
        Next

        Dim PRI_DESK As ACCOUNT_DESK_CLASS
        Dim COUNTER_PARTY_CLASS As COUNTERPARTY_LIMIT_CLASS
        Dim llim = (From q In gdb.COUNTERPARTY_LIMITs _
                 Join pri In gdb.ACCOUNT_DESKs On q.PRI_ACCOUNT_DESK_ID Equals pri.ACCOUNT_DESK_ID _
                 Join sec In gdb.ACCOUNT_DESKs On q.SEC_ACCOUNT_DESK_ID Equals sec.ACCOUNT_DESK_ID _
                    Where pri.ACCOUNT_ID = a_ACCOUNT_ID Or sec.ACCOUNT_ID = a_ACCOUNT_ID _
                Select q).ToList

        For Each qlim In llim
            COUNTER_PARTY_CLASS = New COUNTERPARTY_LIMIT_CLASS
            COUNTER_PARTY_CLASS.GetFromObject(qlim)

            PRI_DESK = GetAccountDeskClass(COUNTER_PARTY_CLASS.PRI_ACCOUNT_DESK_ID)
            If Not (PRI_DESK Is Nothing) Then
                PRI_DESK.COUNTER_PARTY_LIMITS.Add(COUNTER_PARTY_CLASS, COUNTER_PARTY_CLASS.SEC_ACCOUNT_DESK_ID.ToString())
            End If
        Next
        gdb = Nothing
    End Function

    Public Function ValidAccountName(ByRef gdb As DB_ARTB_NETDataContext, _
                                     ByVal s As String) As Boolean
        ValidAccountName = False

        Dim l = From q In gdb.ACCOUNTs _
                Where (q.SHORT_NAME = s) _
                Select q
        If l.Count >= 1 Then Exit Function
        ValidAccountName = True
    End Function

    Public Function DeskHasPresenceOrders(ByRef gdb As DB_ARTB_NETDataContext, _
                                          ByVal a_DeskId As Integer) As Boolean
        DeskHasPresenceOrders = False

        Dim l = From q In gdb.ORDERS_FFAs Join _
                     t In gdb.DESK_TRADERs On q.FOR_DESK_TRADER_ID Equals t.DESK_TRADER_ID _
                Where q.ORDER_GOOD_TILL <> OrderGoodTill.GTC And _
                      (q.LIVE_STATUS = "A" Or q.LIVE_STATUS = "N") And _
                      (q.ORDER_QUALIFIER <> "N" And q.ORDER_QUALIFIER <> "R") _
                      And t.ACCOUNT_DESK_ID = a_DeskId _
                Select q
        If l.Count > 0 Then DeskHasPresenceOrders = True
    End Function

    Public Sub GetDeskBrokerFromDB(ByRef gdb As DB_ARTB_NETDataContext, _
                                   ByVal a_DeskIID As Integer, _
                                   ByVal a_RouteID As Integer, _
                                   ByRef BrokerTraderID As Integer)
        BrokerTraderID = 0
        Try
            Dim q = (From vc In gdb.VESSEL_CLASSes Join _
                          r In gdb.ROUTEs On vc.VESSEL_CLASS_ID Equals r.VESSEL_CLASS_ID Select vc).First()
            If IsNothing(q) Then Exit Sub

            Dim a = (From ac In gdb.ACCOUNTs _
                     Join d In gdb.ACCOUNT_DESKs On ac.ACCOUNT_ID Equals d.ACCOUNT_ID _
                     Where d.ACCOUNT_DESK_ID = a_DeskIID Select ac).First()
            If IsNothing(a) Then Exit Sub

            BrokerTraderID = GetBrokerTradeIDForTradeClass(gdb, a.BROKER_ID, q.DRYWET)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Sub

    Public Function HandleClientLogout(ByRef gdb As DB_ARTB_NETDataContext, _
                                       ByVal a_DeskId As Integer, _
                                       ByVal a_TraderID As Integer, _
                                       ByVal a_Action As Integer, _
                                       ByRef OrderStr As String, _
                                       ByRef TradeStr As String) As Integer
        OrderStr = ""
        TradeStr = ""
        HandleClientLogout = ArtBErrors.Success
        Dim s As String = ""
        Dim l = From q In gdb.ORDERS_FFAs _
                Where q.ORDER_GOOD_TILL <> OrderGoodTill.GTC And _
                      (q.LIVE_STATUS = "A" Or q.LIVE_STATUS = "N") And _
                      (q.ORDER_QUALIFIER <> "N" And q.ORDER_QUALIFIER <> "R") And q.DESK_TRADER_ID = a_TraderID And _
                      (q.SPREAD_ORDER_ID Is Nothing) _
                Select q
        If l.Count = 0 Then Exit Function
        Dim BrokerId As Integer = 0
        For Each q As ORDERS_FFA In l
            Dim o As New ORDERS_FFA_CLASS

            HandleClientLogout = o.GetFromObject(q)
            If HandleClientLogout <> ArtBErrors.Success Then Exit Function
            Select Case a_Action
                Case 1
                    o.LIVE_STATUS = "S"
                Case 2
                    o.LIVE_STATUS = "D"
                Case 3
                    GetDeskBrokerFromDB(gdb, a_DeskId, o.ROUTE_ID, BrokerId)
                    If BrokerId <> 0 Then
                        o.DESK_TRADER_ID = BrokerId
                        o.BROKER_INVISIBLE = False
                    End If
                    o.INFORM_DESK_ID = GetTraderDeskIdFromDB(gdb, BrokerId)
            End Select

            HandleClientLogout = o.AppendToStr(s)
            If HandleClientLogout <> ArtBErrors.Success Then Exit Function
        Next
        If s.Length >= 1 Then
            HandleClientLogout = UpdateDBFromNewOrders(gdb, s, ArtBMessages.OrderFFAAmmend, OrderStr, TradeStr)
        End If
    End Function

    Public Function CheckCounterPartyLimits(ByRef gdb As DB_ARTB_NETDataContext, _
                                            ByRef order1 As Object, _
                                            ByVal TraderID1 As Integer, _
                                            ByVal TraderID2 As Integer, _
                                            ByRef MatchingExchanges As List(Of Integer)) As Boolean
        CheckCounterPartyLimits = False
        Dim OrderPeriod As New ArtBTimePeriod
        OrderPeriod.FillMY(order1.MM1, order1.YY1, order1.MM2, order1.YY2)
        Dim OrderMonths As Integer = OrderPeriod.TotalMonths

        Dim DeskID1 As Integer
        Dim DeskID2 As Integer
        Dim MainDeskID1 As Integer
        Dim MainDeskID2 As Integer

        Dim TC1 As DESK_TRADER_CLASS = Nothing
        Dim TC2 As DESK_TRADER_CLASS = Nothing

        If OperationMode = GVCOpMode.Service Then
            TC1 = New DESK_TRADER_CLASS
            If TC1.GetFromID(gdb, TraderID1) <> ArtBErrors.Success Then
                TC1 = Nothing
                Exit Function
            End If
        Else
            TC1 = GetViewClass(DESK_TRADERS, TraderID1.ToString())
        End If
        If IsNothing(TC1) Then Exit Function
        If OperationMode = GVCOpMode.Service Then
            TC2 = New DESK_TRADER_CLASS
            If TC2.GetFromID(gdb, TraderID2) <> ArtBErrors.Success Then
                TC1 = Nothing
                Exit Function
            End If
        Else
            TC2 = GetViewClass(DESK_TRADERS, TraderID2.ToString())
        End If
        If IsNothing(TC2) Then Exit Function

        If TC1.ACCOUNT_DESK_ID = TC2.ACCOUNT_DESK_ID Then Exit Function
        If TC1.ACCOUNT_ID = TC2.ACCOUNT_ID Then
            'Intra account Transaction
            DeskID1 = TC1.ACCOUNT_DESK_ID
            DeskID2 = TC2.ACCOUNT_DESK_ID
            MainDeskID1 = DeskID1
            MainDeskID2 = DeskID1
        Else
            DeskID1 = TC1.ACCOUNT_DESK_ID
            DeskID2 = TC2.ACCOUNT_DESK_ID
            If OperationMode = GVCOpMode.Service Then
                MainDeskID1 = GetMainDeskFromDB(gdb, TC1.ACCOUNT_ID)
                MainDeskID2 = GetMainDeskFromDB(gdb, TC2.ACCOUNT_ID)
            Else
                MainDeskID1 = GetMainDesk(TC1.ACCOUNT_ID)
                MainDeskID2 = GetMainDesk(TC2.ACCOUNT_ID)
            End If
        End If
        Dim CPL1 As COUNTERPARTY_LIMIT_CLASS = GetCPL(gdb, DeskID1, MainDeskID2)
        If IsNothing(CPL1) Then Exit Function
        Dim CPL2 As COUNTERPARTY_LIMIT_CLASS = GetCPL(gdb, DeskID2, MainDeskID1)
        If IsNothing(CPL2) Then Exit Function

        If MatchingExchanges.Contains(Exch.OTC) Then
            If CPL1.OTC = False Or CPL2.OTC = False Or CPL1.PERIOD_LIMIT < OrderMonths Or CPL2.PERIOD_LIMIT < OrderMonths Then
                MatchingExchanges.Remove(Exch.OTC)
            Else
                CheckCounterPartyLimits = True
            End If
        End If
        Dim CXL As New List(Of Integer)
        For Each x As Integer In MatchingExchanges
            If x <> Exch.OTC Then CXL.Add(x)
        Next
        If CXL.Count = 0 Then
            CXL = Nothing
            Exit Function
        End If
        If CPL1.CLEARED = True And CPL2.CLEARED = True Then
            CheckCounterPartyLimits = True
        Else
            For Each x In CXL
                MatchingExchanges.Remove(x)
            Next
            CXL = Nothing
        End If
    End Function

    Public Function GetCPL(ByRef gdb As DB_ARTB_NETDataContext, _
                           ByVal Desk1 As Integer, _
                           ByVal Desk2 As Integer) As COUNTERPARTY_LIMIT_CLASS
        GetCPL = Nothing
        Dim bHasCreatedConnection As Boolean = False

        If OperationMode = GVCOpMode.Service Then
            If IsNothing(gdb) Then
                gdb = New DB_ARTB_NETDataContext(ArtBConnectionStr)
                bHasCreatedConnection = True
            End If
            GetCPL = New COUNTERPARTY_LIMIT_CLASS
            If GetCPL.GetFromID(gdb, Desk1, Desk2) <> ArtBErrors.Success Then
                GetCPL = Nothing
                GoTo GetCPLEnd
            End If
        Else
            Dim PDC As ACCOUNT_DESK_CLASS = GetAccountDeskClass(Desk1)
            If IsNothing(PDC) Then Exit Function
            GetCPL = GetViewClass(PDC.COUNTER_PARTY_LIMITS, Desk2.ToString())
            If IsNothing(GetCPL) Then
                GetCPL = New COUNTERPARTY_LIMIT_CLASS
                If IsNothing(gdb) Then
                    gdb = New DB_ARTB_NETDataContext(ArtBConnectionStr)
                    bHasCreatedConnection = True
                End If

                If GetCPL.GetFromID(gdb, Desk1, Desk2) <> ArtBErrors.Success Then
                    GetCPL = Nothing
                    GoTo GetCPLEnd
                End If
                PDC.COUNTER_PARTY_LIMITS.Add(GetCPL, Desk2.ToString())
            End If
        End If

GetCPLEnd:
        If bHasCreatedConnection = True Then gdb = Nothing
    End Function

    Public Function GetTraderNameFromOFID(ByVal a_OFID As String) As String
        GetTraderNameFromOFID = ""
        Dim TraderClass As DESK_TRADER_CLASS = GetViewClass(DESK_TRADERS_BY_OF, a_OFID)
        If IsNothing(TraderClass) Then Exit Function
        GetTraderNameFromOFID = GetTraderName(TraderClass.DESK_TRADER_ID, True)
    End Function

    Public Function HasReverseOrders(ByVal a_OrderId As Integer, ByVal a_UserTraderId As Integer) As Boolean
        Dim OrderClass As ORDERS_FFA_CLASS = GetViewClass(ORDERS_FFAS, a_OrderId.ToString())
        If IsNothing(OrderClass) Then Return False
        If OrderClass.ORDER_QUALIFIER = "N" Then Return False
        Dim TBS As String = "B"
        If OrderClass.ORDER_BS = "B" Then TBS = "S"
        If OrderClass.ORDER_TYPE = OrderTypes.FFA Then
            Dim l = From q In ORDERS_FFAS Where _
                        q.ORDER_TYPE = OrderClass.ORDER_TYPE And _
                        q.ROUTE_ID = OrderClass.ROUTE_ID And _
                        q.YY1 = OrderClass.YY1 And _
                        q.MM1 = OrderClass.MM1 And _
                        q.YY2 = OrderClass.YY2 And _
                        q.MM2 = OrderClass.MM2 And _
                        q.LIVE_STATUS = "A" And _
                        q.ORDER_BS = TBS And _
                        q.FOR_DESK_TRADER_ID = a_UserTraderId And _
                        q.PRICE_TYPE = "F" And _
                        q.order_id <> OrderClass.NEGOTIATION_ORDER_ID _
                    Select q.ORDER_ID
            If (l.Count > 0) Then Return True
            Return False
        Else
            Dim l = From q In ORDERS_FFAS Where _
                q.ORDER_TYPE = OrderClass.ORDER_TYPE And _
                q.ROUTE_ID = OrderClass.ROUTE_ID And _
                q.YY1 = OrderClass.YY1 And _
                q.MM1 = OrderClass.MM1 And _
                q.YY2 = OrderClass.YY2 And _
                q.MM2 = OrderClass.MM2 And _
                q.ROUTE_ID2 = OrderClass.ROUTE_ID2 And _
                q.YY21 = OrderClass.YY21 And _
                q.MM21 = OrderClass.MM21 And _
                q.YY22 = OrderClass.YY22 And _
                q.MM22 = OrderClass.MM22 And _
                q.LIVE_STATUS = "A" And _
                q.ORDER_BS = TBS And _
                q.FOR_DESK_TRADER_ID = a_UserTraderId And _
                q.PRICE_TYPE = "F" And _
                q.order_id <> OrderClass.NEGOTIATION_ORDER_ID _
            Select q.ORDER_ID
            If (l.Count > 0) Then Return True
            Return False
        End If
    End Function

    Public Function PrjSpreadLegMatch(ByRef PriSpread As ORDERS_FFA, _
                                      ByRef Pri As ORDERS_FFA_CLASS, _
                                      ByRef SecSpread As ORDERS_FFA, _
                                      ByRef Sec As ORDERS_FFA_CLASS) As Integer
        'Returns:
        ' 0 - orders do not match
        ' 1 - price of order1 must be higher or equal than order2
        '-1 - price of order1 must be lower or equal than order2
        Dim Price1 As Double
        Dim Price2 As Double
        If Pri.ROUTE_ID <> Sec.ROUTE_ID Then Return 0
        If Pri.MM1 <> Sec.MM1 Then Return 0
        If Pri.MM2 <> Sec.MM2 Then Return 0
        If Pri.YY1 <> Sec.YY1 Then Return 0
        If Pri.YY2 <> Sec.YY2 Then Return 0
        If PriSpread.ORDER_TYPE = OrderTypes.RatioSpread Then
            Price1 = PriSpread.PRICE_INDICATED
            If Pri.ORDER_BS <> PriSpread.ORDER_BS Then Price1 = 1 / PriSpread.PRICE_INDICATED
            Price2 = SecSpread.PRICE_INDICATED
            If Sec.ORDER_BS <> SecSpread.ORDER_BS Then Price1 = 1 / SecSpread.PRICE_INDICATED
        End If
        If Pri.ORDER_BS = Sec.ORDER_BS Then
            'Hierarchy of prices
            If Pri.ORDER_BS = "B" Then
                If Price1 > Price2 Then
                    Return 1
                ElseIf Price1 < Price2 Then
                    Return -1
                ElseIf PriSpread.ORDER_DATETIME < SecSpread.ORDER_DATETIME Then
                    Return 1
                Else
                    Return -1
                End If
            Else
                If Price1 < Price2 Then
                    Return -1
                ElseIf Price1 > Price2 Then
                    Return 1
                ElseIf Pri.ORDER_DATETIME < Sec.ORDER_DATETIME Then
                    Return -1
                Else
                    Return 1
                End If
            End If
        Else
            'Bid lower than offer 
            If Pri.ORDER_BS = "B" Then
                Return -1
            Else
                Return 1
            End If
        End If
    End Function

    Public Function GetAvgReferencePrice(ByRef gdb As DB_ARTB_NETDataContext, _
                                         ByRef a_Order As Object, _
                                         Optional ByRef OrderCollection As Object = Nothing, _
                                         Optional ByVal SpreadPrice As Double = -1.0E+20, _
                                         Optional ByVal bSplineRefPrices As Boolean = True, _
                                         Optional ByVal bUseSpreadLegs As Boolean = True, _
                                         Optional ByRef Level As Integer = 0, _
                                         Optional ByRef PriceDateTime As DateTime = Nothing) As Double
        GetAvgReferencePrice = 0
        Dim StartTime As DateTime = DateTime.UtcNow
        Dim Order As Object = Nothing
        Dim Route As Object = Nothing
        Try
            If TypeOf a_Order Is Integer Or TypeOf a_Order Is Nullable(Of Integer) Then
                Dim OrderId As Integer = NullInt2Int(a_Order)
                If OperationMode = GVCOpMode.Client Or IsNothing(gdb) Then
                    Order = GetViewClass(ORDERS_FFAS, OrderId.ToString())
                    If IsNothing(Order) Then Return 0
                    Route = GetViewClass(ROUTES, Order.ROUTE_ID.ToString())
                Else
                    Dim OrderRoute = From q In gdb.ORDERS_FFAs _
                                    Join r In gdb.ROUTEs On q.ROUTE_ID Equals r.ROUTE_ID _
                                    Where q.ORDER_ID = OrderId _
                                    Select q, r
                    For Each x In OrderRoute
                        Order = x.q
                        Route = x.r
                        Exit For
                    Next
                End If
            Else
                If IsNothing(a_Order) Then Return 0
                Order = a_Order
                If RefCol.Count > 0 And Order.ORDER_TYPE = OrderTypes.FFA Then
                    Dim RPStr As String = RoutePeriodStringFromObj(Order)
                    Dim RefC As REF_CLASS = GetViewClass(RefCol, RPStr)
                    If Not IsNothing(RefCol) And Math.Abs(RefC.AvgPrice) < 1.0E+19 Then
                        If RefC.LastIsClose Then Level = 20
                        PriceDateTime = RefC.TradeDateTime
                        Return RefC.AvgPrice
                    End If
                End If

                Dim RouteId As Integer = Order.ROUTE_ID
                If OperationMode = GVCOpMode.Client Or IsNothing(gdb) Then
                    Route = GetViewClass(ROUTES, RouteId.ToString())
                Else
                    Dim OrderRoute = From r In gdb.ROUTEs _
                                    Where r.ROUTE_ID = RouteId _
                                    Select r
                    For Each r In OrderRoute
                        Route = r
                        Exit For
                    Next
                End If
            End If
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try

        If IsNothing(Order) Then Return 0
        If IsNothing(Route) Then Return 0

        If RefCol.Count > 0 And Order.ORDER_TYPE = OrderTypes.FFA Then
            Dim RPStr As String = RoutePeriodStringFromObj(Order)
            Dim RefC As REF_CLASS = GetViewClass(RefCol, RPStr)
            If Not IsNothing(RefCol) And Math.Abs(RefC.AvgPrice) < 1.0E+19 Then
                If RefC.LastIsClose Then
                    Level = 20
                End If
                PriceDateTime = RefC.TradeDateTime
                Return RefC.AvgPrice
            End If
        End If

        Try
            Dim PriceTick As Double = Route.PRICING_TICK
            GetAvgReferencePrice = ReferencePrice(gdb, Order, PriceTick, Level, OrderCollection, SpreadPrice, bSplineRefPrices, bUseSpreadLegs, PriceDateTime)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try

#If DEBUG Then
        Dim EndTime As DateTime = DateTime.UtcNow
        Dim Ts As TimeSpan = EndTime - StartTime
        If Ts.TotalMilliseconds > 1000 Then
            Dim k As Integer = 0
        End If
        Debug.Print("GetAvgReferencePrice Start Time: " & StartTime.ToString())
        Debug.Print("GetAvgReferencePrice End   Time: " & EndTime.ToString())
        Debug.Print("GetAvgReferencePrice Handl Time: " & Ts.TotalMilliseconds.ToString())
#End If

    End Function

    Public Function GetSpreadOrder(ByRef gdb As DB_ARTB_NETDataContext, _
                                   ByRef Order As Object, _
                                   Optional ByRef C As Collection = Nothing) As Object
        GetSpreadOrder = Nothing
        If IsNothing(Order) Then Exit Function
        If NullInt2Int(Order.SPREAD_ORDER_ID) = 0 Then Exit Function
        Dim SpreadOrderID As Integer = Order.SPREAD_ORDER_ID
        If OperationMode = GVCOpMode.Client Then
            If IsNothing(C) Then C = ORDERS_FFAS
            GetSpreadOrder = GetViewClass(C, SpreadOrderID.ToString())
        Else
            Dim l = From q In gdb.ORDERS_FFAs _
                    Where q.ORDER_ID = SpreadOrderID Select q

            For Each q In l
                GetSpreadOrder = q
                Exit Function
            Next
        End If
    End Function

    Public Function AdjustLeg1Price(ByVal Price As Double, _
                                    ByVal ic As Integer, _
                                    ByRef Level As Integer, _
                                    ByRef SpreadOrder As Object, _
                                    ByVal SpreadPrice As Double) As Double
        AdjustLeg1Price = Price
        If Not IsNothing(SpreadOrder) And ic >= 2 Then
            Level += 1
            If SpreadOrder.ORDER_TYPE = OrderTypes.RatioSpread Then
                AdjustLeg1Price *= SpreadOrder.PRICE_INDICATED
            ElseIf SpreadOrder.ORDER_TYPE = OrderTypes.CalendarSpread Or SpreadOrder.ORDER_TYPE = OrderTypes.PriceSpread Then
                AdjustLeg1Price += SpreadOrder.PRICE_INDICATED
            End If
        End If
    End Function

    Public Function IssueAdditionalSpreadTrades(ByRef gdb As DB_ARTB_NETDataContext, _
                               ByRef AffetedRoutePeriods As List(Of String), _
                               ByRef OrderStr As String, _
                               ByRef TradeStr As String) As Integer
        IssueAdditionalSpreadTrades = ArtBErrors.Success

        If IsNothing(AffetedRoutePeriods) Then Exit Function
        If 0 = AffetedRoutePeriods.Count Then Exit Function
        Dim MatchingExchanges As New List(Of Integer)
        Dim Spreads As New List(Of ORDERS_FFA)
        For Each RPStr In AffetedRoutePeriods
            If Len(RPStr) < 13 Then Return ArtBErrors.InvalidPrimaryKey
            Dim mm1 As Integer = Str2Int(RPStr.Substring(7, 2))
            Dim mm2 As Integer = Str2Int(RPStr.Substring(11, 2))
            Dim yy1 As Integer = Str2Int(RPStr.Substring(5, 2)) + 2000
            Dim yy2 As Integer = Str2Int(RPStr.Substring(9, 2)) + 2000
            Dim RouteID As Integer = Str2Int(RPStr.Substring(0, 5))
            Dim d As Date = DateTime.UtcNow.Date

            Dim ExecutedSpreads = (From Leg In gdb.ORDERS_FFAs _
                                   Join Spread In gdb.ORDERS_FFAs On Leg.SPREAD_ORDER_ID Equals Spread.ORDER_ID _
                                   Where _
                                   ((Spread.ROUTE_ID = RouteID And _
                                       Spread.YY1 = yy1 And _
                                       Spread.MM1 = mm1 And _
                                       Spread.YY2 = yy2 And _
                                       Spread.MM2 = mm2) Or _
                                   (Spread.ROUTE_ID2 = RouteID And _
                                       Spread.YY21 = yy1 And _
                                       Spread.MM21 = mm1 And _
                                       Spread.YY22 = yy2 And _
                                       Spread.MM22 = mm2)) And _
                                   (Spread.LIVE_STATUS = "E" Or Spread.LIVE_STATUS = "A" Or (Spread.LIVE_STATUS = "D" Or Spread.ORDER_QUALIFIER = "N")) And _
                                   (Spread.ORDER_DATETIME >= d Or Spread.ORDER_GOOD_TILL = OrderGoodTill.GTC) And _
                                   (Leg.ORDER_DATETIME >= d) And _
                                   (Spread.ORDER_TYPE = OrderTypes.RatioSpread _
                                    Or Spread.ORDER_TYPE = OrderTypes.CalendarSpread _
                                    Or Spread.ORDER_TYPE = OrderTypes.PriceSpread) And _
                                    Spread.PRICE_TYPE = "F" _
                               Select Spread Distinct).ToList

            Dim TradesList As New List(Of TRADES_FFA_CLASS)

            For Each Spread In ExecutedSpreads
                Dim SpreadOrderID As Integer = Spread.ORDER_ID
                Dim Cap1 As Double = 0
                Dim Cap2 As Double = 0
                Dim q1 As Double = 0
                Dim q2 As Double = 0

                Dim TradeList1 = (From T In gdb.TRADES_FFAs _
                                  Join O1 In gdb.ORDERS_FFAs On T.ORDER_ID1 Equals O1.ORDER_ID _
                                  Join O2 In gdb.ORDERS_FFAs On T.ORDER_ID2 Equals O2.ORDER_ID _
                                  Where (O1.SPREAD_ORDER_ID = SpreadOrderID Or O2.SPREAD_ORDER_ID = SpreadOrderID) _
                                  And T.TRADE_TYPE = OrderTypes.FFA _
                                  And O1.ORDER_TYPE = OrderTypes.FFA _
                                  And O2.ORDER_TYPE = OrderTypes.FFA _
                                  And T.IS_SYNTHETIC = False _
                                  Select T, O1, O2).ToList
                Dim OtherSpreadList As New List(Of Integer)
                Dim TIDList1 As New List(Of Integer)
                Dim TIDList2 As New List(Of Integer)

                For Each tupple In TradeList1
                    Dim q As Double = GetActualQuantity(tupple.T)
                    If (tupple.T.UPDATE_STATUS And 1) <> 1 And NullInt2Int(tupple.O1.SPREAD_ORDER_ID) = SpreadOrderID Then
                        If tupple.O1.ORDER_BS = Spread.ORDER_BS Then
                            Cap1 = Cap1 + tupple.T.PRICE_TRADED * q
                            q1 = q1 + q
                        Else
                            Cap2 = Cap2 + tupple.T.PRICE_TRADED * q
                            q2 = q2 + q
                        End If
                        Dim OSID As Integer = NullInt2Int(tupple.O2.SPREAD_ORDER_ID)
                        If OtherSpreadList.Contains(OSID) = False Then OtherSpreadList.Add(OSID)
                        If TIDList1.Contains(tupple.T.TRADE_ID) = False Then TIDList1.Add(tupple.T.TRADE_ID)
                    End If
                    If (tupple.T.UPDATE_STATUS And 2) <> 2 And NullInt2Int(tupple.O2.SPREAD_ORDER_ID) = SpreadOrderID Then
                        If tupple.O2.ORDER_BS = Spread.ORDER_BS Then
                            Cap1 = Cap1 + tupple.T.PRICE_TRADED * q
                            q1 = q1 + q
                        Else
                            Cap2 = Cap2 + tupple.T.PRICE_TRADED * q
                            q2 = q2 + q
                        End If
                        Dim OSID As Integer = NullInt2Int(tupple.O1.SPREAD_ORDER_ID)
                        If OtherSpreadList.Contains(OSID) = False Then OtherSpreadList.Add(OSID)
                        If TIDList2.Contains(tupple.T.TRADE_ID) = False Then TIDList2.Add(tupple.T.TRADE_ID)
                    End If
                Next

                If q1 > 0 And q2 > 0 Then
                    Dim Trade As New TRADES_FFA_CLASS
                    Trade.ORDER_ID1 = Spread.ORDER_ID
                    Trade.PNC = Spread.PNC_ORDER
                    Trade.ORDER_ID2 = Nothing
                    Trade.TRADE_TYPE = Spread.ORDER_TYPE
                    Trade.TRADE_BS1 = Spread.ORDER_BS
                    Trade.TRADE_BS2 = Spread.ORDER_BS
                    Trade.DESK_TRADER_ID1 = Spread.FOR_DESK_TRADER_ID
                    Trade.DESK_TRADER_ID2 = Spread.FOR_DESK_TRADER_ID
                    Trade.INFORM_DESK_ID1 = GetTraderDeskIdFromDB(gdb, Spread.DESK_TRADER_ID)
                    Trade.INFORM_DESK_ID2 = Nothing
                    Trade.QUANTITY = Int(q1)
                    Trade.DAY_QUALIFIER = OrderDayQualifier.NotInDays
                    If Spread.DAY_QUALIFIER <> OrderDayQualifier.NotInDays Then
                        Trade.DAY_QUALIFIER = OrderDayQualifier.DPM
                    End If
                    Trade.QUANTITY2 = Int(q2)
                    Trade.DAY_QUALIFIER2 = OrderDayQualifier.NotInDays
                    If Spread.DAY_QUALIFIER2 <> OrderDayQualifier.NotInDays Then
                        Trade.DAY_QUALIFIER2 = OrderDayQualifier.DPM
                    End If

                    Trade.CLEARING_ID1 = Spread.CLEARER_ID
                    Trade.CLEARING_ID2 = Spread.CLEARER_ID
                    Trade.ROUTE_ID = Spread.ROUTE_ID
                    Trade.MM1 = Spread.MM1
                    Trade.YY1 = Spread.YY1
                    Trade.MM2 = Spread.MM2
                    Trade.YY2 = Spread.YY2
                    Trade.ROUTE_ID2 = Spread.ROUTE_ID2
                    Trade.MM21 = Spread.MM21
                    Trade.YY21 = Spread.YY21
                    Trade.MM22 = Spread.MM22
                    Trade.YY22 = Spread.YY22
                    Trade.ORDER_DATETIME = DateTime.UtcNow.Add(GlobalDateTimeDiff)
                    Trade.SHORTDES = Spread.SHORTDES
                    Trade.IS_SYNTHETIC = True
                    If Spread.ORDER_TYPE = OrderTypes.RatioSpread Then
                        Trade.PRICE_TRADED = (Cap1 / q1) / (Cap2 / q2)
                    Else
                        Trade.PRICE_TRADED = (Cap1 / q1) - (Cap2 / q2)
                    End If
                    RoundTradePrice(gdb, Trade)
                    TradeFillBrokers(gdb, Trade)

                    Dim bSetNothing As Boolean = True
                    Dim bAlreadyDone As Boolean = False
                    If OtherSpreadList.Count = 1 Then
                        Dim OSID As Integer = OtherSpreadList.First
                        If OSID <> 0 Then
                            For Each ot As TRADES_FFA_CLASS In TradesList
                                If ot.ORDER_ID1 = OSID And ot.PRICE_TRADED = Trade.PRICE_TRADED Then
                                    bAlreadyDone = True
                                    Exit For
                                End If
                            Next
                            If bAlreadyDone = False Then
                                TradesList.Add(Trade)
                                bSetNothing = False
                                Trade.UPDATE_STATUS = Trade.UPDATE_STATUS Or 4
                            End If
                        End If
                    End If

                    IssueAdditionalSpreadTrades = Trade.Insert(gdb, True)
                    If IssueAdditionalSpreadTrades <> ArtBErrors.Success Then Exit Function

                    IssueAdditionalSpreadTrades = Trade.AppendToStr(TradeStr)
                    If IssueAdditionalSpreadTrades <> ArtBErrors.Success Then Exit Function

                    For Each tupple In TradeList1
                        Dim b1 As Boolean = TIDList1.Contains(tupple.T.TRADE_ID)
                        Dim b2 As Boolean = TIDList2.Contains(tupple.T.TRADE_ID)
                        If b1 = False And b2 = False Then Continue For
                        Dim TC As New TRADES_FFA_CLASS
                        IssueAdditionalSpreadTrades = TC.GetFromObject(tupple.T)
                        If IssueAdditionalSpreadTrades <> ArtBErrors.Success Then Exit Function

                        If b1 Then TC.SPREAD_TRADE_ID1 = Trade.TRADE_ID
                        If b2 Then TC.SPREAD_TRADE_ID2 = Trade.TRADE_ID
                        If b1 Then TC.UPDATE_STATUS = TC.UPDATE_STATUS Or 1
                        If b2 Then TC.UPDATE_STATUS = TC.UPDATE_STATUS Or 2
                        IssueAdditionalSpreadTrades = TC.Update(gdb, True)
                        If IssueAdditionalSpreadTrades <> ArtBErrors.Success Then Exit Function

                        IssueAdditionalSpreadTrades = TC.AppendToStr(TradeStr)
                        If IssueAdditionalSpreadTrades <> ArtBErrors.Success Then Exit Function

                        TC = Nothing
                    Next

                    OtherSpreadList.Clear()
                    OtherSpreadList = Nothing
                    TIDList1.Clear()
                    TIDList1 = Nothing
                    TIDList2.Clear()
                    TIDList2 = Nothing
                    If bSetNothing Then Trade = Nothing
                End If
            Next
            For Each trade In TradesList
                trade = Nothing
            Next
            TradesList.Clear()
            TradesList = Nothing
        Next

    End Function

    Public Function GetOrderTypeDescr(ByVal a_OrderType As Integer, Optional ByVal a_bUpperCase As Boolean = False) As String
        Dim s As String = ""
        Select Case a_OrderType
            Case OrderTypes.FFA
                s = "Swap"
            Case OrderTypes.RatioSpread
                s = "Ratio Spread"
            Case OrderTypes.CalendarSpread
                s = "Calendar Spread"
            Case OrderTypes.MarketSpread
                s = "Market Spread"
            Case OrderTypes.PriceSpread
                s = "Price Spread"
        End Select
        If a_bUpperCase Then s = s.ToUpper
        Return s
    End Function

    Public Function CreateExecutedSpreadLeg(ByRef gdb As DB_ARTB_NETDataContext, _
                            ByRef Order As ORDERS_FFA_CLASS, _
                            ByVal a_Leg As Integer, _
                            ByRef LegOrder As ORDERS_FFA_CLASS, _
                            ByRef AffetedRoutePeriods As List(Of String), _
                            ByRef OrderStr As String, _
                            ByRef TradeStr As String) As Integer
        CreateExecutedSpreadLeg = ArtBErrors.Success
        If IsNothing(Order) Then Exit Function
        If a_Leg <> 2 Then a_Leg = 1
        LegOrder = Order.GetSpreadLeg(a_Leg)
        LegOrder.LIVE_STATUS = "E"
        CreateExecutedSpreadLeg = UpdateDBFromNewOrder(gdb, LegOrder, OrderStr, TradeStr, AffetedRoutePeriods, False)
    End Function

    Public Function OrderThreadExists(ByRef gdb As DB_ARTB_NETDataContext, _
                                      ByRef Order As ORDERS_FFA_CLASS) As Boolean
        If IsNothing(Order) Then Return False
        If 0 = Order.THREAD Then Return False
        Dim iThread As Integer = Order.THREAD
        Dim DN As DateTime = DateTime.UtcNow
        Dim D As DateTime
        D = DateAdd(DateInterval.Day, -1, DN)
        Dim i As Integer = 1
        While i < 20
            Try
                Dim l = From q In gdb.ORDERS_FFAs Where q.THREAD = iThread And q.ORDER_DATETIME >= D Select q
                If IsNothing(l) Then Exit Function
                If l.Count = 0 Then Return False
                For Each q In l
                    If Order.Equal(q) Then Return True
                Next
                Return False
            Catch ex As Exception
                Debug.Print(ex.ToString)
                'Probably here will enter if a locking situation exists
                'and always rerun in order to conclude
                Debug.Print("Sleeping for 50 miliseconds....")
                System.Threading.Thread.Sleep(50)
            End Try
            i = i + 1
        End While
    End Function

    Public Function ProposePrice(ByRef OrderClass As Object) As Double
        Dim BuyP As Double
        Dim SellP As Double
        Dim LastDone As Double = GetBestPrice(OrderClass, , , True)
        If IsNothing(OrderClass) Then Exit Function
        If OrderClass.ORDER_BS = "B" Then
            BuyP = GetBestPrice(OrderClass, True, , False)
            SellP = GetBestPrice(OrderClass, False, , False)
        Else
            BuyP = GetBestPrice(OrderClass, False, , False)
            SellP = GetBestPrice(OrderClass, True, , False)
        End If

        If BuyP < LastDone And LastDone < SellP Then
            Return LastDone
        End If
        If BuyP > SellP Then Return BuyP
        If OrderClass.ORDER_BS = "B" Then Return BuyP
        Return SellP
    End Function

    Public Function GetBestPrice(ByRef OrderClass As Object, _
                                 Optional ByVal bSameSide As Boolean = True, _
                                 Optional ByVal bMatchQuantity As Boolean = False, _
                                 Optional ByVal bLastDoneOnly As Boolean = False) As Double
        GetBestPrice = -1.0E+20
        If IsNothing(OrderClass) Then Exit Function

        Dim TBS As String = "B"
        If OrderClass.ORDER_BS = "B" Then TBS = "S"
        If bSameSide Then TBS = OrderClass.ORDER_BS

        Dim mm1 As Integer = OrderClass.MM1
        Dim mm2 As Integer = OrderClass.MM2
        Dim yy1 As Integer = OrderClass.YY1
        Dim yy2 As Integer = OrderClass.YY2
        Dim RouteID As Integer = OrderClass.ROUTE_ID
        Dim OrderType As Integer = OrderClass.ORDER_TYPE
        Dim d1 As DateTime = OrderClass.ORDER_DATETIME
        Dim d As Date = d1.Date
        Dim PotentialMatchingOrders As System.Collections.Generic.IEnumerable(Of Object)
        Dim mm21 As Nullable(Of Short) = OrderClass.MM21
        Dim mm22 As Nullable(Of Short) = OrderClass.MM22
        Dim yy21 As Nullable(Of Short) = OrderClass.yy21
        Dim yy22 As Nullable(Of Short) = OrderClass.yy22
        Dim RouteID2 As Nullable(Of Integer) = OrderClass.ROUTE_ID2

        If OrderType = OrderTypes.FFA Then
            If TBS = "S" Then
                PotentialMatchingOrders = From q In ORDERS_FFAS _
                                Where _
                                q.ROUTE_ID = RouteID And _
                                q.YY1 = yy1 And _
                                q.MM1 = mm1 And _
                                q.YY2 = yy2 And _
                                q.MM2 = mm2 And _
                                (q.LIVE_STATUS = "A" Or q.LIVE_STATUS = "N") And _
                                q.ORDER_BS = TBS And _
                                q.ORDER_TYPE = OrderType And _
                                (q.ORDER_DATETIME >= d) And _
                                q.PRICE_INDICATED > 0 And _
                                q.PRICE_INDICATED < GlobalTopPrice _
                            Order By q.PRICE_INDICATED Ascending _
                            Select q
            Else
                PotentialMatchingOrders = From q In ORDERS_FFAS _
                                Where _
                                q.ROUTE_ID = RouteID And _
                                q.YY1 = yy1 And _
                                q.MM1 = mm1 And _
                                q.YY2 = yy2 And _
                                q.MM2 = mm2 And _
                                (q.LIVE_STATUS = "A" Or q.LIVE_STATUS = "N") And _
                                q.ORDER_BS = TBS And _
                                q.ORDER_TYPE = OrderType And _
                                (q.ORDER_DATETIME >= d) And _
                                q.PRICE_INDICATED > 0 And _
                                q.PRICE_INDICATED < GlobalTopPrice _
                            Order By q.PRICE_INDICATED Descending _
                            Select q
            End If
        Else
            If TBS = "S" Then
                PotentialMatchingOrders = From q In ORDERS_FFAS _
                                Where _
                                q.ROUTE_ID = RouteID And _
                                q.YY1 = yy1 And _
                                q.MM1 = mm1 And _
                                q.YY2 = yy2 And _
                                q.MM2 = mm2 And _
                                q.ROUTE_ID2 = RouteID2 And _
                                q.YY21 = yy21 And _
                                q.MM21 = mm21 And _
                                q.YY22 = yy22 And _
                                q.MM22 = mm22 And _
                                (q.LIVE_STATUS = "A" Or q.LIVE_STATUS = "N") And _
                                q.ORDER_BS = TBS And _
                                q.ORDER_TYPE = OrderType And _
                                (q.ORDER_DATETIME >= d) And _
                                q.PRICE_INDICATED > -GlobalTopPrice And _
                                q.PRICE_INDICATED < GlobalTopPrice _
                            Order By q.PRICE_INDICATED Ascending _
                            Select q
            Else
                PotentialMatchingOrders = From q In ORDERS_FFAS _
                                Where _
                                q.ROUTE_ID = RouteID And _
                                q.YY1 = yy1 And _
                                q.MM1 = mm1 And _
                                q.YY2 = yy2 And _
                                q.MM2 = mm2 And _
                                q.ROUTE_ID2 = RouteID2 And _
                                q.YY21 = yy21 And _
                                q.MM21 = mm21 And _
                                q.YY22 = yy22 And _
                                q.MM22 = mm22 And _
                                (q.LIVE_STATUS = "A" Or q.LIVE_STATUS = "N") And _
                                q.ORDER_BS = TBS And _
                                q.ORDER_TYPE = OrderType And _
                                (q.ORDER_DATETIME >= d) And _
                                q.PRICE_INDICATED > -GlobalTopPrice And _
                                q.PRICE_INDICATED < GlobalTopPrice _
                            Order By q.PRICE_INDICATED Descending _
                            Select q
            End If
        End If

        If Not IsNothing(PotentialMatchingOrders) And bLastDoneOnly = False Then
            If bMatchQuantity = False Then
                For Each Order As ORDERS_FFA_CLASS In PotentialMatchingOrders
                    Return Order.PRICE_INDICATED
                Next
            Else
                Dim MatchingExchanges As New List(Of Integer)
                For Each Order As ORDERS_FFA_CLASS In PotentialMatchingOrders
                    If MatchingOrders(Nothing, Order, OrderClass, MatchingExchanges, False, False) = 1 Then
                        If Order.MatchQuantity(OrderClass) Then
                            GetBestPrice = Order.PRICE_INDICATED
                            Exit For
                        End If
                    End If
                Next
                MatchingExchanges.Clear()
                MatchingExchanges = Nothing
                Exit Function
            End If
        End If
        If IsNothing(RouteID2) Then RouteID2 = 0
        If IsNothing(mm21) Then mm21 = 0
        If IsNothing(mm22) Then mm22 = 0
        If IsNothing(yy21) Then yy21 = 0
        If IsNothing(yy22) Then yy22 = 0

        Dim PotentialMatchingTrades As List(Of PRICE_CLASS)
        PotentialMatchingTrades = RoutePeriodTradePrices(Nothing, _
                                                            False, _
                                                            OrderType, _
                                                            RouteID, _
                                                            mm1, yy1, mm2, yy2, _
                                                            RouteID2, _
                                                            mm21, yy21, mm22, yy22, , , True)

        If IsNothing(PotentialMatchingTrades) Then Exit Function
        If PotentialMatchingTrades.Count = 0 Then Exit Function
        Return PotentialMatchingTrades.First.Price

    End Function

    Public Function ChechForRejectedIndicatives(ByRef gdb As DB_ARTB_NETDataContext, _
                                                ByRef OrderClass As ORDERS_FFA_CLASS) As Boolean
        If IsNothing(OrderClass) Then Return False
        Dim TBS As String = "B"
        If OrderClass.ORDER_BS = "B" Then TBS = "S"

        Dim mm1 As Integer = OrderClass.MM1
        Dim mm2 As Integer = OrderClass.MM2
        Dim yy1 As Integer = OrderClass.YY1
        Dim yy2 As Integer = OrderClass.YY2
        Dim RouteID As Integer = OrderClass.ROUTE_ID
        Dim OrderType As Integer = OrderClass.ORDER_TYPE
        Dim d1 As DateTime = DateTime.UtcNow.Add(GlobalDateTimeDiff)
        Dim d As DateTime = d1.AddMinutes(-IndicativeOrdersBanMinutes)
        Dim DeskId As Integer = GetTraderDeskIdFromDB(gdb, OrderClass.FOR_DESK_TRADER_ID)

        Dim PotentialMatchingOrders = From q In gdb.ORDERS_FFAs _
                                      Join cq In gdb.ORDERS_FFAs On cq.ORDER_ID Equals q.COUNTER_PARTY_ORDER_ID _
                                      Join T In gdb.DESK_TRADERs On cq.FOR_DESK_TRADER_ID Equals T.DESK_TRADER_ID _
                                        Where _
                                        q.ROUTE_ID = RouteID And _
                                        q.YY1 = yy1 And _
                                        q.MM1 = mm1 And _
                                        q.YY2 = yy2 And _
                                        q.MM2 = mm2 And _
                                        q.LIVE_STATUS = "D" And _
                                        cq.PRICE_TYPE = "I" And _
                                        q.ORDER_QUALIFIER = "R" And _
                                        q.ORDER_BS = TBS And _
                                        T.ACCOUNT_DESK_ID = DeskId And _
                                        q.ORDER_TYPE = OrderType And _
                                        q.ORDER_DATETIME >= d And _
                                        q.ORDER_GOOD_TILL = OrderGoodTill.Limit And _
                                        q.PRICE_INDICATED > -1000000000.0 And _
                                        q.PRICE_INDICATED < 1000000000.0 _
                                        Select q

        If Not IsNothing(PotentialMatchingOrders) Then
            For Each Order As ORDERS_FFA In PotentialMatchingOrders
                If Order.THREAD <> OrderClass.THREAD Then
                    If TBS = "S" And OrderClass.PRICE_INDICATED >= Order.PRICE_INDICATED Then Return True
                    If TBS = "B" And OrderClass.PRICE_INDICATED <= Order.PRICE_INDICATED Then Return True
                End If

            Next
        End If
        Return False

    End Function

    Public Function ModifySpreadLegPrice( _
                         ByRef LegTrade1 As TRADES_FFA_CLASS, _
                         ByRef TradeStr As String) As Integer
        If IsNothing(LegTrade1) Then Return ArtBErrors.Success
        Dim LegTradeID1 As Integer = LegTrade1.TRADE_ID
        Dim SpreadTradeID As Integer = 0, LegTradeID2 As Integer = 0
        Dim TradeList = From t In TRADES_FFAS Join l In TRADES_FFAS On l.SPREAD_TRADE_ID1 Equals t.TRADE_ID _
                        Where (l.trade_ID = LegTradeID1) And _
                        (t.TRADE_TYPE = OrderTypes.RatioSpread Or t.TRADE_TYPE = OrderTypes.CalendarSpread Or t.TRADE_TYPE = OrderTypes.PriceSpread) And _
                        t.IS_SYNTHETIC = False And l.trade_type = OrderTypes.FFA _
                        Select t
        'Join is only in SPREAD_TRADE_ID1 since it only applies to spread/spread trades where both SPREAD_TRADE_IDs are the same
        If IsNothing(TradeList) Then Return ArtBErrors.Success
        If TradeList.Count = 0 Then Return ArtBErrors.Success
        For Each t As TRADES_FFA_CLASS In TradeList
            Dim StartPrice As Double = t.PRICE_TRADED
            Dim OtherLegPrice As Double = 0
            SpreadTradeID = t.TRADE_ID
            If t.TRADE_TYPE = OrderTypes.RatioSpread Then
                If (StartPrice = 0) Then Continue For
                OtherLegPrice = LegTrade1.PRICE_TRADED / t.PRICE_TRADED
            Else
                OtherLegPrice = LegTrade1.PRICE_TRADED - t.PRICE_TRADED
            End If
            If OtherLegPrice > 1000 And OtherLegPrice < 1000000 Then OtherLegPrice = CInt(OtherLegPrice)

            Dim OtherTradeList = From t2 In TRADES_FFAS _
                                 Join l2 In TRADES_FFAS On l2.SPREAD_TRADE_ID1 Equals t2.TRADE_ID _
                                 Where (t2.TRADE_ID = SpreadTradeID) _
                                 And (l2.TRADE_ID <> LegTradeID1) _
                                 And (t2.TRADE_TYPE = OrderTypes.RatioSpread Or t2.TRADE_TYPE = OrderTypes.CalendarSpread Or t2.TRADE_TYPE = OrderTypes.CalendarSpread) _
                                 And l2.TRADE_TYPE = OrderTypes.FFA _
                                 Select l2
            For Each l2 As TRADES_FFA_CLASS In OtherTradeList
                l2.PRICE_TRADED = OtherLegPrice
                ModifySpreadLegPrice = l2.AppendToStr(TradeStr)
                If ModifySpreadLegPrice <> ArtBErrors.Success Then
                    Exit Function
                End If
            Next
        Next
        Return ArtBErrors.Success
    End Function

    Public Function RoutePeriodStringFromObj(ByRef q As Object, Optional ByVal a_Leg As Integer = 1, Optional ByRef r As Object = Nothing) As String
        RoutePeriodStringFromObj = ""
        If IsNothing(r) Then r = q
        Try
            If a_Leg = 0 Then
                Return q.ORDER_TYPE.ToString() & RoutePeriodString(q.ROUTE_ID, q.MM1, q.YY1, q.MM2, q.YY2) & RoutePeriodString(q.ROUTE_ID2, q.MM21, q.YY21, q.MM22, q.YY22)
            ElseIf a_Leg = 1 Then
                Return RoutePeriodString(r.ROUTE_ID, q.MM1, q.YY1, q.MM2, q.YY2)
            End If
            Return RoutePeriodString(q.ROUTE_ID2, q.MM21, q.YY21, q.MM22, q.YY22)
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try

    End Function

    Public Function RoutePeriodString(ByVal RouteId As Integer, _
                                      ByVal MM1 As Integer, _
                                      ByVal YY1 As Integer, _
                                      ByVal MM2 As Integer, _
                                      ByVal YY2 As Integer) As String
        Dim TP As New ArtBTimePeriod
        RoutePeriodString = Format(RouteId, "00000")
        TP.FillMY(MM1, YY1, MM2, YY2)
        RoutePeriodString = RoutePeriodString & Format(TP.GetID, "00000000")
        TP = Nothing
    End Function

    Public Function RatioSpreadName(ByVal a_RouteID1 As Integer, ByVal a_RouteID2 As Integer) As String
        RatioSpreadName = ""
        Dim RouteName As String = "", VCName As String = "", VCID As Integer = 0, TCName As String = ""
        Dim RouteClass As ROUTE_CLASS = GetViewClass(ROUTES, a_RouteID1.ToString())
        If IsNothing(RouteClass) Then Exit Function
        GetRouteInfo(a_RouteID1, TCName, VCName, VCID)
        RatioSpreadName = VCName & "-" & RouteClass.ROUTE_SHORT
        RouteClass = GetViewClass(ROUTES, a_RouteID2.ToString())
        If IsNothing(RouteClass) Then Exit Function
        GetRouteInfo(a_RouteID2, TCName, VCName, VCID)
        RatioSpreadName = RatioSpreadName & "/" & VCName & "-" & RouteClass.ROUTE_SHORT
    End Function

    Public Function GetRatioSpreadPrecisionFromDB(ByRef gdb As DB_ARTB_NETDataContext) As Double
        GetRatioSpreadPrecisionFromDB = 0
        Try
            Dim l = (From q In gdb.TRADE_CLASS_RATIO_SPREADs _
                        Select q).ToList()
            For Each q In l
                GetRatioSpreadPrecisionFromDB = q.PRECISION_TICK
                Exit Function
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try

    End Function

    Public Function ReferencePrice(ByRef gdb As DB_ARTB_NETDataContext, _
                                      ByRef OrderClass As Object, _
                                      Optional ByVal PriceTick As Double = 1, _
                                      Optional ByRef Level As Integer = 0, _
                                      Optional ByRef OrderCollection As Collection = Nothing, _
                                      Optional ByVal SpreadPrice As Double = -1.0E+20, _
                                      Optional ByVal bSplineRefPrices As Boolean = False, _
                                      Optional ByVal bUseSpreadLegs As Boolean = True, _
                                      Optional ByRef PriceDateTime As DateTime = Nothing) As Double
        ReferencePrice = 0

        Dim TBS As String = "B"
        Dim Ret As Double = 0

        If OrderClass.ORDER_BS = "B" Then TBS = "S"
        Dim OBS As String = TBS
        Dim mm1 As Integer = OrderClass.MM1
        Dim mm2 As Integer = OrderClass.MM2
        Dim yy1 As Integer = OrderClass.YY1
        Dim yy2 As Integer = OrderClass.YY2
        Dim RouteID As Integer = OrderClass.ROUTE_ID
        Dim mm21 As Integer = 0
        Dim mm22 As Integer = 0
        Dim yy21 As Integer = 0
        Dim yy22 As Integer = 0
        Dim RouteID2 As Integer = 0
        Dim omm1 As Integer = OrderClass.MM1
        Dim omm2 As Integer = OrderClass.MM2
        Dim oyy1 As Integer = OrderClass.YY1
        Dim oyy2 As Integer = OrderClass.YY2
        Dim oRouteID As Integer = OrderClass.ROUTE_ID
        Dim OrderType As Integer = OrderClass.ORDER_TYPE
        Dim d1 As DateTime = OrderClass.ORDER_DATETIME
        Dim d As Date = d1.Date
        Dim ic As Integer = 0
        Dim SpreadOrder As Object = GetSpreadOrder(gdb, OrderClass, OrderCollection)
        Dim icLimit As Integer = 2
        Dim PotentialMatchingOrders As Object

        Dim CurrPrices(6) As Double
        Dim CurrPriceTimes(6) As DateTime
        For i As Integer = 0 To 5
            CurrPrices(i) = -1.0E+20
        Next
        If OrderClass.ORDER_TYPE <> OrderTypes.FFA Then
            mm21 = OrderClass.MM21
            mm22 = OrderClass.MM22
            yy21 = OrderClass.YY21
            yy22 = OrderClass.YY22
            RouteID2 = OrderClass.ROUTE_ID2
            icLimit = 4
            If SpreadPrice < -GlobalTopPrice Then SpreadPrice = SpreadOrder.PRICE_INDICATED
        ElseIf Not IsNothing(SpreadOrder) Then
            If OrderClass.ORDER_BS = SpreadOrder.ORDER_BS Then
                mm21 = SpreadOrder.MM21
                mm22 = SpreadOrder.MM22
                yy21 = SpreadOrder.YY21
                yy22 = SpreadOrder.YY22
                RouteID2 = SpreadOrder.ROUTE_ID2
            Else
                mm21 = SpreadOrder.MM1
                mm22 = SpreadOrder.MM2
                yy21 = SpreadOrder.YY1
                yy22 = SpreadOrder.YY2
                RouteID2 = SpreadOrder.ROUTE_ID
            End If
            icLimit = 4
            If SpreadPrice < -GlobalTopPrice Then SpreadPrice = SpreadOrder.PRICE_INDICATED
        End If

        Level = 0
        While ic < icLimit
            Try
                If TBS = "S" Then
                    If IsNothing(gdb) Or OperationMode = GVCOpMode.Client Then
                        PotentialMatchingOrders = (From q In ORDERS_FFAS _
                                        Where _
                                        q.ROUTE_ID = oRouteID And _
                                        q.YY1 = oyy1 And _
                                        q.MM1 = omm1 And _
                                        q.YY2 = oyy2 And _
                                        q.MM2 = omm2 And _
                                        q.LIVE_STATUS = "A" And _
                                        q.ORDER_BS = TBS And _
                                        (bUseSpreadLegs Or q.SPREAD_ORDER_ID Is Nothing) And _
                                         q.ORDER_TYPE = OrderType And _
                                        (q.ORDER_DATETIME >= d Or q.ORDER_GOOD_TILL = OrderGoodTill.GTC) And _
                                        q.PRICE_INDICATED > -GlobalTopPrice And q.PRICE_INDICATED < GlobalTopPrice _
                                    Order By q.PRICE_INDICATED Ascending _
                                    Select q).FirstOrDefault
                    Else
                        PotentialMatchingOrders = (From q In gdb.ORDERS_FFAs _
                                        Where _
                                        q.ROUTE_ID = oRouteID And _
                                        q.YY1 = oyy1 And _
                                        q.MM1 = omm1 And _
                                        q.YY2 = oyy2 And _
                                        q.MM2 = omm2 And _
                                        q.LIVE_STATUS = "A" And _
                                        q.ORDER_BS = TBS And _
                                        (bUseSpreadLegs Or q.SPREAD_ORDER_ID Is Nothing) And _
                                        q.ORDER_TYPE = OrderType And _
                                        (q.ORDER_DATETIME >= d Or q.ORDER_GOOD_TILL = OrderGoodTill.GTC) And _
                                        q.PRICE_INDICATED > -GlobalTopPrice And q.PRICE_INDICATED < GlobalTopPrice _
                                    Order By q.PRICE_INDICATED Ascending _
                                    Select q).FirstOrDefault
                    End If
                Else
                    If IsNothing(gdb) Or OperationMode = GVCOpMode.Client Then
                        PotentialMatchingOrders = (From q In ORDERS_FFAS _
                                        Where _
                                        q.ROUTE_ID = oRouteID And _
                                        q.YY1 = oyy1 And _
                                        q.MM1 = omm1 And _
                                        q.YY2 = oyy2 And _
                                        q.MM2 = omm2 And _
                                        q.LIVE_STATUS = "A" And _
                                        q.ORDER_BS = TBS And _
                                        (bUseSpreadLegs Or q.SPREAD_ORDER_ID Is Nothing) And _
                                        q.ORDER_TYPE = OrderType And _
                                        (q.ORDER_DATETIME >= d Or q.ORDER_GOOD_TILL = OrderGoodTill.GTC) And _
                                        q.PRICE_INDICATED > -GlobalTopPrice And q.PRICE_INDICATED < GlobalTopPrice _
                                    Order By q.PRICE_INDICATED Descending _
                                    Select q).FirstOrDefault
                    Else
                        PotentialMatchingOrders = (From q In gdb.ORDERS_FFAs _
                                        Where _
                                        q.ROUTE_ID = oRouteID And _
                                        q.YY1 = oyy1 And _
                                        q.MM1 = omm1 And _
                                        q.YY2 = oyy2 And _
                                        q.MM2 = omm2 And _
                                        q.LIVE_STATUS = "A" And _
                                        q.ORDER_BS = TBS And _
                                        (bUseSpreadLegs Or q.SPREAD_ORDER_ID Is Nothing) And _
                                        q.ORDER_TYPE = OrderType And _
                                        (q.ORDER_DATETIME >= d Or q.ORDER_GOOD_TILL = OrderGoodTill.GTC) And _
                                        q.PRICE_INDICATED > -GlobalTopPrice And q.PRICE_INDICATED < GlobalTopPrice _
                                    Order By q.PRICE_INDICATED Descending _
                                    Select q).FirstOrDefault
                    End If
                End If
            Catch ex As Exception
            End Try

            If Not IsNothing(PotentialMatchingOrders) Then
                If TypeOf PotentialMatchingOrders Is ORDERS_FFA Or _
                       TypeOf PotentialMatchingOrders Is ORDERS_FFA_CLASS Then
                    CurrPrices(ic) = AdjustLeg1Price(PotentialMatchingOrders.PRICE_INDICATED, ic, Level, SpreadOrder, SpreadPrice)
                    CurrPriceTimes(ic) = PotentialMatchingOrders.ORDER_DATETIME
                End If
            End If


            ic = ic + 1

            If TBS = "S" Then
                TBS = "B"
            Else
                TBS = "S"
            End If
            If ic = 2 Then
                If CurrPrices(0) > -GlobalTopPrice And CurrPrices(1) > -GlobalTopPrice Then
                    PriceDateTime = Max(CurrPriceTimes(0), CurrPriceTimes(1))
                    Return CInt(((CurrPrices(0) + CurrPrices(1)) * 0.5) / PriceTick) * PriceTick
                End If

                If TBS = "S" Then
                    TBS = "B"
                Else
                    TBS = "S"
                End If
                omm1 = mm21
                omm2 = mm22
                oyy1 = yy21
                oyy2 = yy22
                oRouteID = RouteID2
            End If
        End While

        If CurrPrices(2) > -GlobalTopPrice And CurrPrices(3) > -GlobalTopPrice Then
            PriceDateTime = Max(CurrPriceTimes(2), CurrPriceTimes(3))
            Return CInt(((CurrPrices(2) + CurrPrices(3)) * 0.5) / PriceTick) * PriceTick
        End If

        ic = 0
        Level = 10
        Dim TradePriceLists(2) As List(Of PRICE_CLASS)
        omm1 = mm1
        omm2 = mm2
        oyy1 = yy1
        oyy2 = yy2
        oRouteID = RouteID
        Dim xOrderClass As Object = Nothing
        Dim xOrderCollection As Object = Nothing
        If bSplineRefPrices Then
            xOrderClass = OrderClass
            xOrderCollection = OrderCollection
        End If
        Dim cpi As Integer = 4

        While ic < icLimit
            TradePriceLists(ic) = RoutePeriodTradePrices(gdb, False, _
                                                            OrderType, _
                                                            oRouteID, _
                                                            omm1, oyy1, omm2, oyy2, _
                                                            RouteID2, _
                                                            mm21, yy21, mm22, yy22, _
                                                            xOrderClass, xOrderCollection)
            If Not IsNothing(TradePriceLists(ic)) Then
                If TradePriceLists(ic).Count > 0 Then
                    Dim pc As PRICE_CLASS = TradePriceLists(ic).First
                    If pc.bTrade = True Then
                        CurrPrices(cpi) = AdjustLeg1Price(pc.Price, ic, Level, SpreadOrder, SpreadPrice)
                        CurrPriceTimes(cpi) = pc.Time

                        For pix As Integer = (cpi - 4) * 2 To (cpi - 4) * 2 + 1
                            If CurrPrices(pix) > -GlobalTopPrice Then
                                If OBS = "B" Then
                                    If CurrPrices(pix) > CurrPrices(cpi) Then
                                        Level = pix
                                        Return CInt(CurrPrices(pix) / PriceTick) * PriceTick
                                    End If
                                Else
                                    If CurrPrices(pix) < CurrPrices(cpi) Then
                                        Level = pix
                                        Return CInt(CurrPrices(pix) / PriceTick) * PriceTick
                                    End If
                                End If
                            End If
                        Next
                        PriceDateTime = CurrPriceTimes(cpi)
                        Return CInt(CurrPrices(cpi) / PriceTick) * PriceTick
                    End If
                End If
            End If
            cpi += 1
            ic += 2
            If ic = 2 Then
                omm1 = mm21
                omm2 = mm22
                oyy1 = yy21
                oyy2 = yy22
                oRouteID = RouteID2
            End If
        End While
        Level = 20
        ic = 0
        If Not IsNothing(TradePriceLists(ic)) Then
            If TradePriceLists(ic).Count > 0 Then
                Dim pc As PRICE_CLASS = TradePriceLists(ic).First
                PriceDateTime = DateTime.UtcNow.Date
                Return CInt(AdjustLeg1Price(pc.Price, ic, Level, SpreadOrder, SpreadPrice) / PriceTick) * PriceTick
            End If
        End If
        Level = 100
    End Function


    Public Function ParseRPString(ByVal RPStr As String, _
                                    ByRef OrderType As Integer, _
                                    ByRef RouteID As Integer, _
                                    ByRef MM1 As Integer, _
                                    ByRef YY1 As Integer, _
                                    ByRef MM2 As Integer, _
                                    ByRef YY2 As Integer, _
                                    Optional ByRef RouteID2 As Integer = 0, _
                                    Optional ByRef MM21 As Integer = 0, _
                                    Optional ByRef YY21 As Integer = 0, _
                                    Optional ByRef MM22 As Integer = 0, _
                                    Optional ByRef YY22 As Integer = 0) As Integer

        If Len(RPStr) < 13 Then Return ArtBErrors.InvalidPrimaryKey
        OrderType = OrderTypes.FFA
        If Len(RPStr) = 27 Then
            OrderType = Str2Int(RPStr.Substring(0, 1))
            RPStr = RPStr.Substring(1)
        End If

        MM1 = Str2Int(RPStr.Substring(7, 2))
        MM2 = Str2Int(RPStr.Substring(11, 2))
        YY1 = Str2Int(RPStr.Substring(5, 2)) + 2000
        YY2 = Str2Int(RPStr.Substring(9, 2)) + 2000
        RouteID = Str2Int(RPStr.Substring(0, 5))

        If Len(RPStr) = 26 Then
            RPStr = RPStr.Substring(13)
            MM21 = Str2Int(RPStr.Substring(7, 2))
            MM22 = Str2Int(RPStr.Substring(11, 2))
            YY21 = Str2Int(RPStr.Substring(5, 2)) + 2000
            YY22 = Str2Int(RPStr.Substring(9, 2)) + 2000
            RouteID2 = Str2Int(RPStr.Substring(0, 5))
        End If
        Return ArtBErrors.Success
    End Function

    Public Function RoutePeriodTradePrices(ByRef gdb As DB_ARTB_NETDataContext, _
                                               ByVal bGetAll As Boolean, _
                                               ByVal RPStr As String, _
                                               Optional ByRef OrderClass As Object = Nothing, _
                                               Optional ByRef OrderCollection As Object = Nothing, _
                                               Optional ByVal BrokerMode As Boolean = False) As List(Of PRICE_CLASS)
        Dim OrderType As Integer
        Dim RouteID As Integer
        Dim MM1 As Integer
        Dim YY1 As Integer
        Dim MM2 As Integer
        Dim YY2 As Integer
        Dim RouteID2 As Integer = 0
        Dim MM21 As Integer = 0
        Dim YY21 As Integer = 0
        Dim MM22 As Integer = 0
        Dim YY22 As Integer = 0
        If ParseRPString(RPStr, OrderType, RouteID, MM1, YY1, MM2, YY2, RouteID2, MM21, YY21, MM22, YY22) <> ArtBErrors.Success Then Return Nothing
        Return RoutePeriodTradePrices(gdb, bGetAll, OrderType, RouteID, MM1, YY1, MM2, YY2, RouteID2, MM21, YY21, MM22, YY22, OrderClass, OrderCollection, BrokerMode)
    End Function

    Public Function RoutePeriodTradePrices(ByRef gdb As DB_ARTB_NETDataContext, _
                                              ByVal bGetAll As Boolean, _
                                              ByVal a_OrderType As Integer, _
                                              ByVal a_RouteID As Integer, _
                                              ByVal MM1 As Integer, _
                                              ByVal YY1 As Integer, _
                                              ByVal MM2 As Integer, _
                                              ByVal YY2 As Integer, _
                                              Optional ByVal a_RouteID2 As Integer = 0, _
                                              Optional ByVal MM21 As Integer = 0, _
                                              Optional ByVal YY21 As Integer = 0, _
                                              Optional ByVal MM22 As Integer = 0, _
                                              Optional ByVal YY22 As Integer = 0, _
                                              Optional ByRef OrderClass As Object = Nothing, _
                                              Optional ByRef OrderCollection As Object = Nothing, _
                                              Optional ByVal BrokerMode As Boolean = False) As List(Of PRICE_CLASS)
        If 0 = a_RouteID Then Return Nothing
        If 0 = a_RouteID2 Then a_RouteID2 = a_RouteID
        If MM21 = 0 Then
            MM21 = MM1
            MM22 = MM2
            YY21 = YY1
            YY22 = YY2
        End If

        Dim d1 As DateTime = DateTime.UtcNow
        Dim d As Date = d1.Date
        Dim PL = New List(Of PRICE_CLASS)

        If a_OrderType = OrderTypes.FFA Then
            Try
                If IsNothing(gdb) Or OperationMode = GVCOpMode.Client Then
                    Dim LastTradeList = From q In TRADES_FFAS _
                                             Where q.ROUTE_ID = a_RouteID _
                                             And q.MM1 = MM1 _
                                             And q.yy1 = YY1 _
                                             And q.mm2 = MM2 _
                                             And q.yy2 = YY2 _
                                             And q.TRADE_TYPE = a_OrderType _
                                             And (q.PNC = False Or (q.PNC = True And BrokerMode = True And q.DESK_TRADER_ID1 = SystemDeskTraderId)) _
                                             And q.ORDER_DATETIME >= d _
                                             And q.PRICE_TRADED < GlobalTopPrice _
                                             And q.PRICE_TRADED > 0 _
                                             And (q.UPDATE_STATUS And 4) <> 4 _
                                             Order By q.ORDER_DATETIME Descending _
                                             Select q

                    For Each q As TRADES_FFA_CLASS In LastTradeList
                        Dim PC As New PRICE_CLASS
                        PC.TradeId = q.TRADE_ID
                        PC.Price = q.PRICE_TRADED
                        PC.Time = q.ORDER_DATETIME
                        If q.DESK_TRADER_ID1 = SystemDeskTraderId Then PC.Ficticious = True
                        PC.PNC = q.PNC
                        PL.Add(PC)
                        If bGetAll = False Then Return PL
                    Next
                Else
                    Dim LastTradeList = From q In gdb.TRADES_FFAs _
                                             Where q.ROUTE_ID = a_RouteID _
                                             And q.MM1 = MM1 _
                                             And q.YY1 = YY1 _
                                             And q.MM2 = MM2 _
                                             And q.YY2 = YY2 _
                                             And q.TRADE_TYPE = a_OrderType _
                                             And (q.PNC = False Or (q.PNC = True And BrokerMode = True And q.DESK_TRADER_ID1 = SystemDeskTraderId)) _
                                             And q.ORDER_DATETIME >= d _
                                             And q.PRICE_TRADED < GlobalTopPrice _
                                             And q.PRICE_TRADED > 0 _
                                             And (q.UPDATE_STATUS And 4) <> 4 _
                                             Order By q.ORDER_DATETIME Descending _
                                             Select q

                    For Each q As TRADES_FFA In LastTradeList
                        Dim PC As New PRICE_CLASS
                        PC.TradeId = q.TRADE_ID
                        PC.Price = q.PRICE_TRADED
                        PC.Time = q.ORDER_DATETIME
                        If q.DESK_TRADER_ID1 = SystemDeskTraderId Then PC.Ficticious = True
                        PC.PNC = q.PNC
                        PL.Add(PC)
                        If bGetAll = False Then Return PL
                    Next
                End If

                Dim BPrice As Double = 0
                Dim BD As Date
                If RoutePeriodBalticPrice(gdb, _
                                          BPrice, _
                                          a_RouteID, _
                                          MM1, YY1, MM2, YY2, _
                                          BD, OrderClass, OrderCollection) Then
                    Dim PC As New PRICE_CLASS
                    PC.Price = BPrice
                    PC.Time = BD
                    PC.bTrade = False
                    PL.Add(PC)
                End If

            Catch e As Exception
                Debug.Print(e.ToString)
            End Try
        Else
            Try
                If IsNothing(gdb) Or OperationMode = GVCOpMode.Client Then
                    Dim LastTradeList = From q In TRADES_FFAS _
                                        Where q.ROUTE_ID = a_RouteID _
                                        And q.MM1 = MM1 _
                                        And q.yy1 = YY1 _
                                        And q.mm2 = MM2 _
                                        And q.yy2 = YY2 _
                                        And q.ROUTE_ID2 = a_RouteID2 _
                                        And q.MM21 = MM21 _
                                        And q.yy21 = YY21 _
                                        And q.mm22 = MM22 _
                                        And q.yy22 = YY22 _
                                        And q.TRADE_TYPE = a_OrderType _
                                        And (q.PNC = False Or (q.PNC = True And BrokerMode = True And q.DESK_TRADER_ID1 = SystemDeskTraderId)) _
                                        And q.ORDER_DATETIME >= d _
                                        And q.PRICE_TRADED < GlobalTopPrice _
                                        And q.PRICE_TRADED > -GlobalTopPrice _
                                        And (q.UPDATE_STATUS And 4) <> 4 _
                                        Order By q.ORDER_DATETIME Descending _
                                        Select q

                    For Each q As TRADES_FFA_CLASS In LastTradeList
                        Dim PC As New PRICE_CLASS
                        PC.TradeId = q.TRADE_ID
                        PC.Price = q.PRICE_TRADED
                        PC.Price2 = NullDouble2Double(q.PRICE_TRADED2)
                        PC.Time = q.ORDER_DATETIME
                        If q.DESK_TRADER_ID1 = SystemDeskTraderId Then PC.Ficticious = True
                        PC.PNC = q.PNC

                        PL.Add(PC)
                        If bGetAll = False Then Return PL
                    Next
                Else
                    Dim LastTradeList = From q In gdb.TRADES_FFAs _
                                        Where q.ROUTE_ID = a_RouteID _
                                        And q.MM1 = MM1 _
                                        And q.YY1 = YY1 _
                                        And q.MM2 = MM2 _
                                        And q.YY2 = YY2 _
                                        And q.ROUTE_ID2 = a_RouteID2 _
                                        And q.MM21 = MM21 _
                                        And q.YY21 = YY21 _
                                        And q.MM22 = MM22 _
                                        And q.YY22 = YY22 _
                                        And q.TRADE_TYPE = a_OrderType _
                                        And (q.PNC = False Or (q.PNC = True And BrokerMode = True And q.DESK_TRADER_ID1 = SystemDeskTraderId)) _
                                        And q.ORDER_DATETIME >= d _
                                        And q.PRICE_TRADED < GlobalTopPrice _
                                        And q.PRICE_TRADED > -GlobalTopPrice _
                                        And (q.UPDATE_STATUS And 4) <> 4 _
                                        Order By q.ORDER_DATETIME Descending _
                                        Select q

                    For Each q As TRADES_FFA In LastTradeList
                        Dim PC As New PRICE_CLASS
                        PC.TradeId = q.TRADE_ID
                        PC.Price = q.PRICE_TRADED
                        PC.Price2 = NullDouble2Double(q.PRICE_TRADED2)
                        PC.Time = q.ORDER_DATETIME
                        If q.DESK_TRADER_ID1 = SystemDeskTraderId Then PC.Ficticious = True
                        PC.PNC = q.PNC
                        PL.Add(PC)
                        If bGetAll = False Then Return PL
                    Next
                End If

                Dim BPrice1 As Double = 0
                Dim BD1 As Date
                Dim BPrice2 As Double = 0
                Dim BD2 As Date
                If RoutePeriodBalticPrice(gdb, _
                                          BPrice1, _
                                          a_RouteID, _
                                          MM1, YY1, MM2, YY2, _
                                          BD1, OrderClass, OrderCollection) And _
                    RoutePeriodBalticPrice(gdb, _
                                          BPrice2, _
                                          a_RouteID2, _
                                          MM21, YY21, MM22, YY22, _
                                          BD2, OrderClass, OrderCollection) Then
                    Select Case a_OrderType
                        Case OrderTypes.RatioSpread
                            If BPrice2 <> 0 Then
                                Dim PC As New PRICE_CLASS
                                PC.Price = BPrice1 / BPrice2
                                If BD1 < BD2 Then
                                    PC.Time = BD1
                                Else
                                    PC.Time = BD2
                                End If
                                PC.bTrade = False
                                PL.Add(PC)
                            End If
                        Case OrderTypes.CalendarSpread, OrderTypes.PriceSpread
                            Dim PC As New PRICE_CLASS
                            PC.Price = BPrice1 - BPrice2
                            If BD1 < BD2 Then
                                PC.Time = BD1
                            Else
                                PC.Time = BD2
                            End If
                            PC.bTrade = False
                            PL.Add(PC)

                        Case OrderTypes.MarketSpread
                            Dim PC As New PRICE_CLASS
                            PC.Price = BPrice1
                            PC.Price2 = BPrice2
                            If BD1 < BD2 Then
                                PC.Time = BD1
                            Else
                                PC.Time = BD2
                            End If
                            PC.bTrade = False
                            PL.Add(PC)
                    End Select
                End If

            Catch e As Exception
                Debug.Print(e.ToString)
            End Try
        End If
        If PL.Count = 0 Then
            PL = Nothing
            Return Nothing
        End If
        Dim TickPL = From p In PL Where p.Ficticious = False Or p.PNC = False
        Dim i As Integer = TickPL.Count - 2
        If i < 0 Then Return PL
        Dim LastTick As Integer = TickPL(i + 1).Tick
        Dim LastPrice As Double = TickPL(i + 1).Price
        While (i >= 0)
            If TickPL(i).Price > LastPrice Then
                TickPL(i).Tick = Ticks.Up
            ElseIf TickPL(i).Price < LastPrice Then
                TickPL(i).Tick = Ticks.Down
            Else
                Select Case LastTick
                    Case Ticks.None, Ticks.Up, Ticks.Up_Equal
                        TickPL(i).Tick = Ticks.Up_Equal
                    Case Else
                        TickPL(i).Tick = Ticks.Down_Equal
                End Select
            End If
            LastTick = TickPL(i).Tick
            LastPrice = TickPL(i).Price
            i = i - 1
        End While
        Return PL
    End Function

    Public Function RoutePeriodBalticPrice(ByRef gdb As DB_ARTB_NETDataContext, _
                                            ByRef Price As Double, _
                                            ByVal RouteId As Integer, _
                                            ByVal MM1 As Integer, _
                                            ByVal YY1 As Integer, _
                                            ByVal MM2 As Integer, _
                                            ByVal YY2 As Integer, _
                                            ByRef BD As Date, _
                                            Optional ByRef OrderClass As Object = Nothing, _
                                            Optional ByRef OrderCollection As Object = Nothing) As Boolean
        Price = 0
        If IsNothing(gdb) Or OperationMode = GVCOpMode.Client Then
            Try
                Dim RPStr As String = RoutePeriodString(RouteId, MM1, YY1, MM2, YY2)
                Dim BFTC As BALTIC_FORWARD_RATE_CLASS
                BFTC = GetViewClass(BALTIC_FORWARD_RATES, RPStr)
                If Not IsNothing(BFTC) Then
                    BD = BFTC.FIXING_DATE
                    Price = BFTC.FIXING
                    Return True
                End If

                Dim sc As New PeriodCubicSplineClass(Me, gdb, RouteId, OrderClass, OrderCollection)

                Price = sc.PerformSpline(YY1, MM1, YY2, MM2)
                Return True

                Return False
            Catch ex As Exception
                Debug.Print(ex.ToString())
            End Try
        Else
            Try
                Dim BalticForwardTrades = From q In gdb.BALTIC_FORWARD_RATES_VIEWs _
                                            Where _
                                            q.ROUTE_ID = RouteId And _
                                            q.YY1 = YY1 And _
                                            q.MM1 = MM1 And _
                                            q.YY2 = YY2 And _
                                            q.MM2 = MM2 _
                                             Select q
                If BalticForwardTrades.Count > 0 Then
                    Price = BalticForwardTrades.First.FIXING
                    BD = BalticForwardTrades.First.FIXING_DATE
                    Return True
                End If

                Dim sc As New PeriodCubicSplineClass(Me, gdb, RouteId, OrderClass, OrderCollection)

                Price = sc.PerformSpline(YY1, MM1, YY2, MM2)
                BD = DateAdd(DateInterval.Day, -1, Date.UtcNow)

                Return True
            Catch ex As Exception
                Debug.Print(ex.ToString())
            End Try
        End If
    End Function

    Public Function CreateConfos(ByRef gdb As DB_ARTB_NETDataContext, _
                                 ByRef TradeStr As String, _
                                 ByRef EmailC As Collection, _
                                 ByRef SMSC As Collection) As Integer
        CreateConfos = ArtBErrors.Success
        Dim TradeC As Collection

        TradeC = ParseString(TradeStr, ArtBMessages.OrderFFATrade)

        If IsNothing(TradeC) Then Exit Function
        If TradeC.Count <= 0 Then Exit Function

        Dim CI As EMAIL_CLASS
        Dim SI As SMS_CLASS

        For Each t As TRADES_FFA_CLASS In TradeC
            Dim TradeId As Int32 = t.TRADE_ID
            Dim L = gdb.TradeConfos(TradeId)

            For Each Q In L
                If Q.CONFIRMATION_SENT = True Then Continue For
                Dim s As String
                If Q.TRADE_BS = "B" Then
                    s = "You bought "
                Else
                    s = "You sold "
                End If

                s = s & Q.VESSEL_CLASS1 & "-" & Q.ROUTE_SHORT1
                If Q.TRADE_TYPE <> OrderTypes.FFA Then
                    s &= "/" & Q.VESSEL_CLASS2 & "-" & Q.ROUTE_SHORT2
                End If
                s &= " " & Q.SHORTDES & " "

                Dim d As Double = Q.PRICE_TRADED
                Dim PriceStr As String = ""
                If Q.TRADE_TYPE = OrderTypes.RatioSpread Then
                    PriceStr = d.ToString(ARTB_RATIOPRICE_FORMATSTR, ArtBRatioPriceInfo)
                ElseIf Q.TICK1 >= 1 Then
                    PriceStr = d.ToString(ARTB_INT_FORMATSTR, ArtBIntNumberInfo) & " " & Q.QUOTEDES1
                Else
                    PriceStr = d.ToString(ARTB_CURRENCY_FORMATSTR, ArtBNumberInfo) & " " & Q.QUOTEDES1
                End If

                Select Case Q.DAY_QUALIFIER
                    Case OrderDayQualifier.NotInDays
                        s = s & Format(Q.QUANTITY, "#,##0") & " " & Q.QDES1 & " "
                    Case OrderDayQualifier.Full
                        s = s & "Full "
                    Case OrderDayQualifier.Half
                        s = s & "Half "
                    Case OrderDayQualifier.DPM
                        s = s & Format(Q.QUANTITY, "0") & " dpm "
                End Select
                If Q.TRADE_TYPE <> OrderTypes.FFA Then
                    s = s & "/ "
                    Select Case Q.DAY_QUALIFIER2
                        Case OrderDayQualifier.NotInDays
                            s = s & Format(Q.QUANTITY2, "#,##0") & " " & Q.QDES2 & " "
                        Case OrderDayQualifier.Full
                            s = s & "Full "
                        Case OrderDayQualifier.Half
                            s = s & "Half "
                        Case OrderDayQualifier.DPM
                            s = s & Format(Q.QUANTITY2, "0") & " dpm "
                    End Select
                End If
                s = s & "@ " & PriceStr
                If Q.IS_SYNTHETIC = 0 Or Q.TRADE_TYPE = OrderTypes.FFA Then
                    If Not IsNothing(Q.EXCHANGE_NAME) Then
                        s &= " " & Q.EXCHANGE_NAME
                    End If
                    If Q.CPT.Length() >= 1 Then
                        s &= " with " & Q.CPT
                    End If
                Else
                    Dim xl = From qt In gdb.TRADES_FFAs Join qx In gdb.EXCHANGEs On qt.EXCHANGE_ID Equals qx.EXCHANGE_ID _
                             Where qt.IS_SYNTHETIC = False And _
                                 (qt.SPREAD_TRADE_ID1 = TradeId Or qt.SPREAD_TRADE_ID2 = TradeId) And _
                                qt.TRADE_TYPE = OrderTypes.FFA _
                                Select qt, qx
                    Dim xs As String = ""
                    Dim tcpts As String = ""
                    Dim cpts As String
                    For Each qq In xl
                        If InStr(xs, qq.qx.EXCHANGE_NAME_SHORT) <= 0 Then
                            If Len(xs) >= 1 Then xs &= ", "
                            xs &= qq.qx.EXCHANGE_NAME_SHORT
                        End If
                        Dim QTID As Integer = Q.TRADER_ID1
                        Dim TR1 As Integer = qq.qt.DESK_TRADER_ID1
                        Dim TR2 As Integer = qq.qt.DESK_TRADER_ID2
                        Dim XID As Integer = qq.qx.EXCHANGE_ID

                        If qq.qt.DESK_TRADER_ID1 = Q.TRADER_ID1 Then
                            Try

                                Dim qqs = gdb.GetTraderNameWithRules(TR2, QTID, XID)
                                cpts = qqs
                            Catch ex As Exception
                                Debug.Print(ex.ToString())
                            End Try
                        Else
                            Try
                                Dim qqs = gdb.GetTraderNameWithRules(TR1, QTID, XID)
                                cpts = qqs
                            Catch ex As Exception
                                Debug.Print(ex.ToString())
                            End Try
                        End If
                        If Len(cpts) > 1 Then
                            If InStr(tcpts, cpts) <= 0 Then
                                If Len(tcpts) >= 1 Then tcpts &= ", "
                                tcpts &= cpts
                            End If
                        End If
                    Next

                    If Len(xs) >= 1 Then s &= " " & xs
                    If Len(tcpts) > 1 Then s &= " with " & tcpts
                End If
                If Q.SEND_TRADE_EMAIL = True Then
                    Dim EmailAddrStr As String = Q.EMAIL1
                    Dim EmailStr As String = s & vbNewLine
                    If EmailC.Contains(EmailAddrStr) Then
                        CI = CType(EmailC(EmailAddrStr), EMAIL_CLASS)
                        CI.Body = CI.Body & vbNewLine & EmailStr
                        EmailC.Remove(EmailAddrStr)
                    Else
                        CI = New EMAIL_CLASS
                        CI.Addr = EmailAddrStr
                        CI.Body = EmailStr
                    End If
                    CI.BrokerStr = "brs"
                    CI.LongBrokerStr = "BRS Futures"
                    If NullInt2Int(t.BROKER_ID1) <> 0 And Q.SIDE = 1 Then
                        Dim AC As New ACCOUNT_CLASS
                        If AC.GetFromID(gdb, t.BROKER_ID1) = ArtBErrors.Success Then
                            CI.BrokerStr = AC.SHORT_NAME
                            CI.LongBrokerStr = AC.FULL_NAME
                        End If
                        AC = Nothing
                    ElseIf NullInt2Int(t.BROKER_ID2) <> 0 And Q.SIDE = 2 Then
                        Dim AC As New ACCOUNT_CLASS
                        If AC.GetFromID(gdb, t.BROKER_ID2) = ArtBErrors.Success Then
                            CI.BrokerStr = AC.SHORT_NAME
                            CI.LongBrokerStr = AC.FULL_NAME
                        End If
                        AC = Nothing
                    End If
                    EmailC.Add(CI, EmailAddrStr)
                End If
                If Q.SEND_TRADE_SMS = True Then
                    Dim MobileNumStr As String = Q.TEL_MBL1
                    Dim SMSStr As String = s
                    If SMSC.Contains(MobileNumStr) Then
                        SI = CType(SMSC(MobileNumStr), SMS_CLASS)
                        If NullInt2Int(t.BROKER_ID1) <> 0 And Q.SIDE = 1 Then
                            Dim AC As New ACCOUNT_CLASS
                            If AC.GetFromID(gdb, t.BROKER_ID1) = ArtBErrors.Success Then
                                SI.BrokerStr = AC.SHORT_NAME
                                SI.LongBrokerStr = AC.FULL_NAME
                            End If
                            AC = Nothing
                        ElseIf NullInt2Int(t.BROKER_ID2) <> 0 And Q.SIDE = 2 Then
                            Dim AC As New ACCOUNT_CLASS
                            If AC.GetFromID(gdb, t.BROKER_ID2) = ArtBErrors.Success Then
                                SI.BrokerStr = AC.SHORT_NAME
                                SI.LongBrokerStr = AC.FULL_NAME
                            End If
                            AC = Nothing
                        End If
                        SI.SMSList.Add(SMSStr)
                    Else
                        SI = New SMS_CLASS
                        SI.MobileNum = MobileNumStr
                        If NullInt2Int(t.BROKER_ID1) <> 0 And Q.SIDE = 1 Then
                            Dim AC As New ACCOUNT_CLASS
                            If AC.GetFromID(gdb, t.BROKER_ID1) = ArtBErrors.Success Then
                                SI.BrokerStr = AC.SHORT_NAME
                                SI.LongBrokerStr = AC.FULL_NAME
                            End If
                            AC = Nothing
                        ElseIf NullInt2Int(t.BROKER_ID2) <> 0 And Q.SIDE = 2 Then
                            Dim AC As New ACCOUNT_CLASS
                            If AC.GetFromID(gdb, t.BROKER_ID2) = ArtBErrors.Success Then
                                SI.BrokerStr = AC.SHORT_NAME
                                SI.LongBrokerStr = AC.FULL_NAME
                            End If
                            AC = Nothing
                        End If
                        SI.SMSList = New List(Of String)
                        SI.SMSList.Add(SMSStr)
                        SMSC.Add(SI, MobileNumStr)
                    End If
                End If

                If t.TRADE_TYPE <> OrderTypes.FFA And t.IS_SYNTHETIC Then
                    Call t.UpdateConfSent(gdb, 1)
                    Call t.UpdateConfSent(gdb, 2)
                Else
                    Call t.UpdateConfSent(gdb, Q.SIDE)
                End If
            Next
        Next
    End Function

    Public Sub RoundTradePrice(ByVal gdb As DB_ARTB_NETDataContext, ByRef Trade As Object)
        Dim PriceLimit As Double = 10000000
        If IsNothing(Trade) Then Exit Sub
        If Trade.TRADE_TYPE <> OrderTypes.RatioSpread Then
            If Not IsNothing(Trade.PRICE_TRADED) And Not IsNothing(Trade.ROUTE_ID) Then
                Dim Price As Double = Trade.PRICE_TRADED
                If Price > -PriceLimit And Price < PriceLimit Then
                    Try
                        Dim RouteID1 As Integer = NullInt2Int(Trade.ROUTE_ID)
                        Dim l = From q In gdb.ROUTEs _
                                Where q.ROUTE_ID = RouteID1 _
                                Select q

                        For Each q As ROUTE In l
                            Dim st As Double = NullDouble2Double(q.SETTLEMENT_TICK)
                            If 0 = st Then Continue For
                            Trade.PRICE_TRADED = Round(Price, st)
                            Exit For
                        Next
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try
                End If
            End If
            If Trade.TRADE_TYPE = OrderTypes.MarketSpread Then
                If Not IsNothing(Trade.PRICE_TRADED2) And Not IsNothing(Trade.ROUTE_ID2) Then
                    Dim Price As Double = CType(Trade.PRICE_TRADED2, Double)
                    If Price > -PriceLimit And Price < PriceLimit Then
                        Try
                            Dim RouteID2 As Integer = NullInt2Int(Trade.ROUTE_ID2)
                            Dim l = From q In gdb.ROUTEs _
                                    Where q.ROUTE_ID = RouteID2 _
                                    Select q

                            For Each q As ROUTE In l
                                Dim st As Double = NullDouble2Double(q.SETTLEMENT_TICK)
                                If 0 = st Then Continue For
                                Trade.PRICE_TRADED2 = Round(Price, st)
                                Exit For
                            Next
                        Catch ex As Exception
                            Debug.Print(ex.ToString)
                        End Try
                    End If
                End If
            End If
        Else
            If Not IsNothing(Trade.PRICE_TRADED) _
               And Not IsNothing(Trade.ROUTE_ID) _
               And Not IsNothing(Trade.ROUTE_ID2) Then
                Dim Price As Double = Trade.PRICE_TRADED
                If Price > -PriceLimit And Price < PriceLimit Then
                    Try
                        Dim RouteID1 As Integer = NullInt2Int(Trade.ROUTE_ID)
                        Dim RouteID2 As Integer = NullInt2Int(Trade.ROUTE_ID2)
                        Dim l = From q In gdb.TRADE_CLASS_RATIO_SPREADs _
                                Where q.ROUTE_ID1 = RouteID1 And q.ROUTE_ID2 = RouteID2 _
                                Select q

                        For Each q As TRADE_CLASS_RATIO_SPREAD In l
                            Dim st As Double = NullDouble2Double(q.PRECISION_TICK)
                            If 0 = st Then st = NullDouble2Double(q.PRICING_TICK)
                            If 0 = st Then Continue For
                            Trade.PRICE_TRADED = Round(Price, st)
                            Exit For
                        Next
                    Catch ex As Exception
                        Debug.Print(ex.ToString)
                    End Try
                End If
            End If
        End If

    End Sub

    Public Sub SaveLayoutFile(ByVal TraderID As Integer, ByVal FN As String)
        Dim gdb As DB_ARTB_NETDataContext = Nothing
        Dim newFile As New LAYOUT
        If System.IO.File.Exists(FN) = True Then
            Dim fs As String = ""
            Try
                gdb = GetNewConnection()

                If IsNothing(gdb) Then
                    gdb = Nothing
                    newFile = Nothing
                    Exit Sub
                End If

                Dim inputBuffer As Byte() = System.IO.File.ReadAllBytes(FN)


                newFile.DESK_TRADER_ID = TraderID
                newFile.FILENAME = FN

                newFile.CONTENTS = New System.Data.Linq.Binary(inputBuffer)

                Dim l = From q In gdb.LAYOUTs _
                  Where q.DESK_TRADER_ID = TraderID And q.FILENAME = FN _
                  Select q

                If l.Count < 1 Then
                    gdb.LAYOUTs.InsertOnSubmit(newFile)
                Else
                    For Each q As LAYOUT In l
                        q.DESK_TRADER_ID = TraderID
                        q.FILENAME = FN
                        q.CONTENTS = New System.Data.Linq.Binary(inputBuffer)
                    Next
                End If
                gdb.SubmitChanges()
            Catch ex As Exception
                Debug.Print(ex.ToString)
            Finally
                gdb = Nothing
                newFile = Nothing
            End Try
        End If
    End Sub

    Public Sub LoadLayoutFile(ByVal TraderID As Integer, ByVal FN As String)
        Dim gdb As DB_ARTB_NETDataContext = Nothing
        Try
            gdb = GetNewConnection()

            If IsNothing(gdb) Then
                Exit Sub
            End If

            Dim l = From q In gdb.LAYOUTs _
              Where q.DESK_TRADER_ID = TraderID And q.FILENAME = FN _
              Select q

            For Each q As LAYOUT In l
                Dim buffer As Byte() = q.CONTENTS.ToArray()
                System.IO.File.WriteAllBytes(FN, buffer)
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString)
        Finally
            gdb = Nothing
        End Try
    End Sub

    Public Function ReportedSpreadTradeQ(ByRef Trade As TRADES_FFA_CLASS) As Double
        If IsNothing(Trade) Then Return 0
        Dim TradeID As Integer = Trade.TRADE_ID
        Dim TradeBS As String = Trade.TRADE_BS1
        Dim AQ As Integer = Int(GetActualQuantity(Trade, 1))
        If Trade.IS_SYNTHETIC = False Then Return AQ
        If Trade.TRADE_TYPE = OrderTypes.FFA Then Return AQ
        If TradeBS = "B" Then Return AQ
        Dim tl1 = From q In TRADES_FFAS _
                 Where _
                 Not (q.SPREAD_TRADE_ID1 Is Nothing) And _
                 Not (q.SPREAD_TRADE_ID2 Is Nothing) And _
                 q.SPREAD_TRADE_ID1 = TradeID And _
                 q.TRADE_BS1 = TradeBS

        For Each t As TRADES_FFA_CLASS In tl1
            AQ -= Int(GetActualQuantity(t, 1))
        Next

        Dim tl2 = From q In TRADES_FFAS _
                 Where _
                 Not (q.SPREAD_TRADE_ID1 Is Nothing) And _
                 Not (q.SPREAD_TRADE_ID2 Is Nothing) And _
                 q.SPREAD_TRADE_ID2 = TradeID And _
                 q.TRADE_BS2 = TradeBS

        For Each t As TRADES_FFA_CLASS In tl2
            AQ -= Int(GetActualQuantity(t, 1))
        Next
        Return AQ
    End Function

    Public Function GetDefaultTraderID(ByVal AccountName As String) As Integer
        GetDefaultTraderID = 0
        For Each ac As ACCOUNT_CLASS In ACCOUNTS
            If ac.SUSPENDED Then Continue For
            If ac.ACCOUNT_TYPE_ID <> 1 Then Continue For
            If InStr(ac.SHORT_NAME, AccountName) <= 0 Then Continue For
            For Each D As ACCOUNT_DESK_CLASS In ac.DESKS
                If D.DESK_ACTIVE = False Then Continue For
                If D.DESK_QUALIFIER <> 1 Then Continue For
                For Each t As DESK_TRADER_CLASS In D.TRADERS
                    If t.IS_DESK_ADMIN Then Return t.DESK_TRADER_ID
                Next
            Next
        Next

    End Function

    Public Sub HandleOrderEvents(ByRef RouteList As List(Of Integer), _
                                      ByRef FirmUPList As List(Of Integer), _
                                      ByRef NegotiationList As List(Of Integer), _
                                      ByRef DirectHitFailedList As List(Of Integer), _
                                      ByRef UserInfo As TraderInfoClass, _
                                      ByRef NewOrderCollection As Collection)

        If NewOrderCollection.Count > 0 Then
            RaiseEvent OrdersUpdated(False)
            Dim OrderIdList As New List(Of Integer)
            For Each q In NewOrderCollection
                If OrderIdList.Contains(q.ORDER_ID) = False Then
                    OrderIdList.Add(q.ORDER_ID)
                End If
            Next
            RaiseEvent OrderFFAUpdated(RouteList, OrderIdList)
            OrderIdList.Clear()
            OrderIdList = Nothing
        ElseIf RouteList.Count > 0 Then
            Dim OrderIdList As New List(Of Integer)
            RaiseEvent OrderFFAUpdated(RouteList, OrderIdList)
            OrderIdList.Clear()
            OrderIdList = Nothing

        End If

        NewOrderCollection.Clear()
        NewOrderCollection = Nothing

        RouteList.Clear()
        RouteList = Nothing

        HandleOrders(FirmUPList, NegotiationList, DirectHitFailedList)

    End Sub

    Public Function AdjustSpreads2(ByRef gdb As DB_ARTB_NETDataContext, _
                              ByRef AffetedRoutePeriods As List(Of String), _
                              ByRef OrderStr As String, _
                              ByRef TradeStr As String, _
                              Optional ByRef RouteList As List(Of Integer) = Nothing, _
                              Optional ByRef FirmUPList As List(Of Integer) = Nothing, _
                              Optional ByRef NegotiationList As List(Of Integer) = Nothing, _
                              Optional ByRef DirectHitFailedList As List(Of Integer) = Nothing, _
                              Optional ByRef UserInfo As TraderInfoClass = Nothing, _
                              Optional ByRef NewOrderCollection As Collection = Nothing, _
                              Optional ByRef e As System.ComponentModel.DoWorkEventArgs = Nothing) As Integer

        AdjustSpreads2 = ArtBErrors.Success

        If IsNothing(AffetedRoutePeriods) Then Exit Function
        If 0 = AffetedRoutePeriods.Count Then Exit Function
AdjustSpreads2Restart:
        AdjustSpreads2 = ArtBErrors.Success
        Dim MatchingExchanges As New List(Of Integer)
        Dim CurrDateTime As Date = Date.UtcNow
        Dim CurrDT As DateTime = CurrDateTime.Date
        Dim TotalMaxCap As Double = 0
        CurrDT = DateAdd(DateInterval.Hour, 2, CurrDT)

        Dim TradeClasses As New List(Of String)
        Dim RoutesL As New List(Of Integer)
        For Each RPStr In AffetedRoutePeriods
            Dim RouteID As Integer = Str2Int(RPStr.Substring(0, 5))
            If RoutesL.Contains(RouteID) = False Then RoutesL.Add(RouteID)
        Next

        For Each RouteId As Integer In RoutesL
            Dim tempRouteID = RouteId
            Dim vcl As Object
            If IsNothing(gdb) Then
                vcl = From t In VESSEL_CLASSES Join r In ROUTES On r.VESSEL_CLASS_ID Equals t.VESSEL_CLASS_ID _
                  Where r.ROUTE_ID = tempRouteID
            Else
                vcl = From t In gdb.VESSEL_CLASSes Join r In gdb.ROUTEs On r.VESSEL_CLASS_ID Equals t.VESSEL_CLASS_ID _
                  Where r.ROUTE_ID = tempRouteID
            End If

            For Each vc In vcl
                Dim DW As String = vc.t.DRYWET
                If TradeClasses.Contains(DW) = False Then TradeClasses.Add(DW)
            Next
        Next

        Dim d As Integer = DateTime.UtcNow.Day()
        Dim m As Integer = DateTime.UtcNow.Month()
        Dim y As Integer = DateTime.UtcNow.Year()


        For Each RPStr As String In AffetedRoutePeriods
            If Len(RPStr) < 13 Then Return ArtBErrors.InvalidPrimaryKey
            Dim mm1 As Integer = Str2Int(RPStr.Substring(7, 2))
            Dim mm2 As Integer = Str2Int(RPStr.Substring(11, 2))
            Dim yy1 As Integer = Str2Int(RPStr.Substring(5, 2)) + 2000
            Dim yy2 As Integer = Str2Int(RPStr.Substring(9, 2)) + 2000
            Dim RouteID As Integer = Str2Int(RPStr.Substring(0, 5))

            Dim SynthOrders = From q In ORDERS_FFAS _
                             Where q.ORDER_ID >= SynthOrderOffset And _
                                    q.ORDER_TYPE <> OrderTypes.FFA And _
                                   ((q.ROUTE_ID = RouteID And _
                                   q.YY1 = yy1 And _
                                   q.MM1 = mm1 And _
                                   q.YY2 = yy2 And _
                                   q.MM2 = mm2) Or _
                                    (q.ROUTE_ID2 = RouteID And _
                                   q.YY21 = yy1 And _
                                   q.MM21 = mm1 And _
                                   q.YY22 = yy2 And _
                                   q.MM22 = mm2))
            For Each q In SynthOrders
                q.LIVE_STATUS = "D"
                If RouteList.Contains(q.ROUTE_ID) = False Then RouteList.Add(q.ROUTE_ID)
                If RouteList.Contains(q.ROUTE_ID2) = False Then RouteList.Add(q.ROUTE_ID2)
            Next
        Next

        For Each TradeClassShort As String In TradeClasses
            Dim TCS As String = TradeClassShort
            Dim ol As Object

            If IsNothing(gdb) Then
                ol = From q In ORDERS_FFAS Join s In DESK_TRADERS _
                            On q.FOR_DESK_TRADER_ID Equals s.DESK_TRADER_ID _
                            Join r In ROUTES On q.ROUTE_ID Equals r.ROUTE_ID _
                            Join v In VESSEL_CLASSES On r.VESSEL_CLASS_ID Equals v.VESSEL_CLASS_ID _
                             Where (q.LIVE_STATUS = "A" _
                                     And q.PRICE_TYPE = "F") _
                                     And v.DRYWET = TCS _
                                     And ((q.ORDER_DATETIME.Day = d _
                                     And q.ORDER_DATETIME.Month = m _
                                     And q.ORDER_DATETIME.Year = y) Or q.ORDER_GOOD_TILL = OrderGoodTill.GTC) _
                             Order By q.ORDER_DATETIME _
                             Select q, s.ACCOUNT_DESK_ID, r.SETTLEMENT_TICK, r.LOT_SIZE, r.PRICING_TICK
            Else
                ol = From q In gdb.ORDERS_FFAs Join s In gdb.DESK_TRADERs _
                            On q.FOR_DESK_TRADER_ID Equals s.DESK_TRADER_ID _
                            Join r In gdb.ROUTEs On q.ROUTE_ID Equals r.ROUTE_ID _
                            Join v In gdb.VESSEL_CLASSes On r.VESSEL_CLASS_ID Equals v.VESSEL_CLASS_ID _
                             Where (q.LIVE_STATUS = "A" _
                                     And q.PRICE_TYPE = "F") _
                                     And v.DRYWET = TCS _
                                     And ((q.ORDER_DATETIME.Day = d _
                                     And q.ORDER_DATETIME.Month = m _
                                     And q.ORDER_DATETIME.Year = y) Or q.ORDER_GOOD_TILL = OrderGoodTill.GTC) _
                             Order By q.ORDER_DATETIME _
                             Select q, s.ACCOUNT_DESK_ID, r.SETTLEMENT_TICK, r.LOT_SIZE, r.PRICING_TICK

            End If

            Dim originalOLC As New Collection
            Dim OrderCollection As New Collection
            Dim ARPs As New List(Of String)
            Dim Spreads As New Collection

            RefCol.Clear()

            For Each co In ol
                Dim nq As New ORDERS_FFA_CLASS
                nq.GetFromObject(co.q)
                OrderCollection.Add(nq, nq.ORDER_ID.ToString)
                If nq.ORDER_TYPE <> OrderTypes.FFA Then Continue For
                Dim nas As New AdjustSpreadsClass
                nas.q = nq
                nas.SETTLEMENT_TICK = co.SETTLEMENT_TICK
                nas.ACCOUNT_DESK_ID = co.ACCOUNT_DESK_ID
                nas.LOT_SIZE = co.LOT_SIZE
                If (NullInt2Int(co.q.SPREAD_ORDER_ID) <> 0) Then
                    Dim sq As New ORDERS_FFA_CLASS
                    If IsNothing(gdb) Then
                        sq.GetFromObject(GetViewClass(ORDERS_FFAS, co.q.SPREAD_ORDER_ID.ToString()))
                    Else
                        sq.GetFromID(gdb, co.q.SPREAD_ORDER_ID)
                    End If
                    nas.s = sq
                End If
                originalOLC.Add(nas, co.q.ORDER_ID.ToString())
                Dim rps As String = RoutePeriodStringFromObj(co.q)
                If ARPs.Contains(rps) = False Then ARPs.Add(rps)

                Dim RefC As REF_CLASS = GetViewClass(RefCol, rps)
                If IsNothing(RefC) Then
                    RefC = New REF_CLASS(rps)
                    RefCol.Add(RefC, rps)
                End If
                If NullInt2Int(co.q.SPREAD_ORDER_ID) = 0 Then RefC.AssignOrderPrice(co.q, co.PRICING_TICK)
            Next

            UpdateRefCol(Nothing, TCS)

            Dim nc0 As New Collection
            MIPPrepare(gdb, originalOLC, nc0, 2100000000, 2100000000, UserInfo)

            Dim RPPLCol As New Collection
            Call SP(gdb, OrderCollection, RPPLCol, 2100000000, 2100000000)
            If Not IsNothing(e) Then
                If e.Cancel = True Then
                    e.Cancel = False
                    Debug.Print("Adj Spreads restarted at:" & DateTime.UtcNow.ToString())
                    GoTo AdjustSpreads2Restart
                End If
            End If


            For Each RPStr In ARPs
                If Len(RPStr) < 13 Then Return ArtBErrors.InvalidPrimaryKey
                Dim mm1 As Integer = Str2Int(RPStr.Substring(7, 2))
                Dim mm2 As Integer = Str2Int(RPStr.Substring(11, 2))
                Dim yy1 As Integer = Str2Int(RPStr.Substring(5, 2)) + 2000
                Dim yy2 As Integer = Str2Int(RPStr.Substring(9, 2)) + 2000
                Dim RouteID As Integer = Str2Int(RPStr.Substring(0, 5))

                Dim SynthOrders = From q In ORDERS_FFAS _
                                 Where q.ORDER_ID >= SynthOrderOffset And _
                                        q.ORDER_TYPE <> OrderTypes.FFA And _
                                       ((q.ROUTE_ID = RouteID And _
                                       q.YY1 = yy1 And _
                                       q.MM1 = mm1 And _
                                       q.YY2 = yy2 And _
                                       q.MM2 = mm2) Or _
                                        (q.ROUTE_ID2 = RouteID And _
                                       q.YY21 = yy1 And _
                                       q.MM21 = mm1 And _
                                       q.YY22 = yy2 And _
                                       q.MM22 = mm2))
                For Each q In SynthOrders
                    q.LIVE_STATUS = "D"
                    If RouteList.Contains(q.ROUTE_ID) = False Then RouteList.Add(q.ROUTE_ID)
                    If RouteList.Contains(q.ROUTE_ID2) = False Then RouteList.Add(q.ROUTE_ID2)

                Next
            Next

            For Each RPStr In ARPs
                If Len(RPStr) < 13 Then Return ArtBErrors.InvalidPrimaryKey
                Dim mm1 As Integer = Str2Int(RPStr.Substring(7, 2))
                Dim mm2 As Integer = Str2Int(RPStr.Substring(11, 2))
                Dim yy1 As Integer = Str2Int(RPStr.Substring(5, 2)) + 2000
                Dim yy2 As Integer = Str2Int(RPStr.Substring(9, 2)) + 2000
                Dim RouteID As Integer = Str2Int(RPStr.Substring(0, 5))


                Dim i As Integer
                Dim BS(2) As String

                BS(0) = "B"
                BS(1) = "S"

                For i = 0 To 1
                    Debug.Print("------------------------------------------------------------------------------")
                    Debug.Print(BS(i) & "-" & RPStr)

                    If Not IsNothing(e) Then
                        If e.Cancel = True Then
                            e.Cancel = False
                            Debug.Print("Adj Spreads restarted at:" & DateTime.UtcNow.ToString())
                            GoTo AdjustSpreads2Restart
                        End If
                    End If
                    Dim AffectedSpreads As Object
                    Dim BidId As Integer = 2000000000
                    Dim BidE As Integer = BidId

                    Dim nc As New Collection
                    Dim olc As New Collection
                    Dim TotalQ As Integer = 0
                    For Each co In originalOLC
                        olc.Add(co, co.q.ORDER_ID.ToString())
                        If co.q.FLEXIBLE_QUANTITY = OrderFlexQuantinty.StrictFull Then Continue For
                        If co.q.MM1 <> mm1 Then Continue For
                        If co.q.MM2 <> mm2 Then Continue For
                        If co.q.YY1 <> yy1 Then Continue For
                        If co.q.YY2 <> yy2 Then Continue For
                        If co.q.ORDER_TYPE <> OrderTypes.FFA Then Continue For
                        If co.q.ROUTE_ID <> RouteID Then Continue For
                        If co.q.ORDER_BS = BS(i) Then Continue For
                        TotalQ += GetActualQuantity(co.q)
                    Next

                    If IsNothing(gdb) Then
                        AffectedSpreads = From MatchingLeg In ORDERS_FFAS _
                                                Join s In DESK_TRADERS On MatchingLeg.FOR_DESK_TRADER_ID Equals s.DESK_TRADER_ID _
                                                   Join r In ROUTES On MatchingLeg.ROUTE_ID Equals r.ROUTE_ID _
                                                   Join Spread In ORDERS_FFAS On MatchingLeg.SPREAD_ORDER_ID Equals Spread.ORDER_ID _
                                                   Join AffectedLeg In ORDERS_FFAS On AffectedLeg.SPREAD_ORDER_ID Equals Spread.ORDER_ID _
                                                   Where _
                                                   Spread.ORDER_ID < SynthOrderOffset And _
                                                   AffectedLeg.ROUTE_ID = RouteID And _
                                                   AffectedLeg.YY1 = yy1 And _
                                                   AffectedLeg.MM1 = mm1 And _
                                                   AffectedLeg.YY2 = yy2 And _
                                                   AffectedLeg.MM2 = mm2 And _
                                                   AffectedLeg.LIVE_STATUS = "A" And _
                                                   Spread.LIVE_STATUS = "A" And _
                                                   AffectedLeg.LIVE_STATUS = "A" And _
                                                   MatchingLeg.ORDER_BS <> AffectedLeg.ORDER_BS And _
                                                   AffectedLeg.ORDER_BS = BS(i) And _
                                                   (Spread.ORDER_TYPE = OrderTypes.RatioSpread _
                                                    Or Spread.ORDER_TYPE = OrderTypes.CalendarSpread _
                                                    Or Spread.ORDER_TYPE = OrderTypes.PriceSpread) _
                                               Select MatchingLeg, Spread, AffectedLeg, s.ACCOUNT_DESK_ID, r.SETTLEMENT_TICK, r.LOT_SIZE
                    Else
                        AffectedSpreads = From MatchingLeg In gdb.ORDERS_FFAs _
                                Join s In gdb.DESK_TRADERs _
                                        On MatchingLeg.FOR_DESK_TRADER_ID Equals s.DESK_TRADER_ID _
                                        Join r In gdb.ROUTEs On MatchingLeg.ROUTE_ID Equals r.ROUTE_ID _
                                   Join Spread In gdb.ORDERS_FFAs On MatchingLeg.SPREAD_ORDER_ID Equals Spread.ORDER_ID _
                                   Join AffectedLeg In gdb.ORDERS_FFAs On AffectedLeg.SPREAD_ORDER_ID Equals Spread.ORDER_ID _
                                   Where _
                                   Spread.ORDER_ID < SynthOrderOffset And _
                                   AffectedLeg.ROUTE_ID = RouteID And _
                                   AffectedLeg.YY1 = yy1 And _
                                   AffectedLeg.MM1 = mm1 And _
                                   AffectedLeg.YY2 = yy2 And _
                                   AffectedLeg.MM2 = mm2 And _
                                   AffectedLeg.LIVE_STATUS = "A" And _
                                   Spread.LIVE_STATUS = "A" And _
                                   AffectedLeg.LIVE_STATUS = "A" And _
                                   MatchingLeg.ORDER_BS <> AffectedLeg.ORDER_BS And _
                                   AffectedLeg.ORDER_BS = BS(i) And _
                                   (Spread.ORDER_TYPE = OrderTypes.RatioSpread _
                                    Or Spread.ORDER_TYPE = OrderTypes.CalendarSpread _
                                    Or Spread.ORDER_TYPE = OrderTypes.PriceSpread) _
                                   Select MatchingLeg, Spread, AffectedLeg, s.ACCOUNT_DESK_ID, r.SETTLEMENT_TICK, r.LOT_SIZE
                    End If
                    Dim bFirst As Boolean = True
                    Dim SecondOffset As Integer = 0
                    Dim SpreadCount As Integer = 0

                    For Each SpreadTupple In AffectedSpreads
                        SpreadCount += 1
                    Next

                    If SpreadCount = 0 Then
                        Debug.Print("Skipped...")
                        Continue For
                    End If

                    For Each SpreadTupple In AffectedSpreads
                        Dim nas As New AdjustSpreadsClass
                        nas.LegBS = SpreadTupple.AffectedLeg.ORDER_BS
                        If nas.LegBS = "B" Then
                            nas.LegPrice = 1.0E+20
                            nas.LegDir = 1
                        Else
                            nas.LegPrice = -1.0E+20
                            nas.LegDir = -1
                        End If
                        If SpreadTupple.Spread.ORDER_BS <> SpreadTupple.AffectedLeg.ORDER_BS Then
                            nas.AffectedSide = 2
                            nas.MatchingSide = 1
                        End If

                        nas.SpreadLegQuantity = Int(GetActualQuantity(SpreadTupple.Spread, nas.MatchingSide))
                        nas.AffectedLegQuantinty = Int(GetActualQuantity(SpreadTupple.Spread, nas.AffectedSide))
                        nas.AffectedLeg = SpreadTupple.AffectedLeg
                        nas.CurrPrice = SpreadTupple.AffectedLeg.PRICE_INDICATED
                        nas.CurrQ = Int(GetActualQuantity(SpreadTupple.AffectedLeg, 1))
                        nas.LegQ = nas.AffectedLegQuantinty
                        nas.q = Nothing
                        Dim Spread As New ORDERS_FFA_CLASS
                        Spread.GetFromObject(SpreadTupple.Spread)
                        nas.Spread = Spread
                        nas.s = Spread
                        nas.LOT_SIZE = SpreadTupple.LOT_SIZE
                        nas.ACCOUNT_DESK_ID = SpreadTupple.ACCOUNT_DESK_ID
                        nas.SETTLEMENT_TICK = SpreadTupple.SETTLEMENT_TICK
                        olc.Add(nas, BidE.ToString())
                        BidE += 1
                        If Not Spreads.Contains(Spread.ORDER_ID.ToString()) Then Spreads.Add(nas, Spread.ORDER_ID.ToString)

                        If (bFirst = True And SpreadTupple.AffectedLeg.FLEXIBLE_QUANTITY <> OrderFlexQuantinty.StrictFull) _
                           Or (SpreadTupple.AffectedLeg.FLEXIBLE_QUANTITY = OrderFlexQuantinty.StrictFull) Then
                            Dim IssuedOrders As Integer = 1
                            If SpreadTupple.AffectedLeg.FLEXIBLE_QUANTITY <> OrderFlexQuantinty.StrictFull Then
                                IssuedOrders = 1 'totalq
                            End If
                            For k As Integer = 1 To IssuedOrders
                                Dim nas0 As New AdjustSpreadsClass
                                Dim nq0 As New ORDERS_FFA_CLASS
                                Dim nd As DateTime = CurrDT
                                nd = DateAdd(DateInterval.Second, SecondOffset, CurrDT)
                                SecondOffset += 0
                                nq0.GetFromObject(SpreadTupple.Affectedleg)
                                nq0.ORDER_ID = BidE
                                BidE += 1
                                nq0.ORDER_DATETIME = nd

                                nas0.MinPrice = -1.0E+20
                                nas0.MaxPrice = 1.0E+20
                                If i = 0 Then
                                    nq0.ORDER_BS = "S"
                                Else
                                    nq0.ORDER_BS = "B"
                                End If
                                If nq0.ORDER_BS = "B" Then
                                    nq0.PRICE_INDICATED = nas0.MaxPrice
                                Else
                                    nq0.PRICE_INDICATED = nas0.MinPrice
                                End If
                                If SpreadTupple.AffectedLeg.FLEXIBLE_QUANTITY <> OrderFlexQuantinty.StrictFull Then
                                    bFirst = False

                                    nq0.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket
                                    nq0.QUANTITY_STEP = 1
                                    nq0.SetQuantity(1000)
                                End If
                                nq0.SINGLE_EXCHANGE_EXECUTION = False
                                nq0.SPREAD_ORDER_ID = Nothing
                                nq0.COMMIT_ORDER_ID = Nothing
                                nq0.FOR_DESK_TRADER_ID = GlobalMatchingTrader
                                nq0.DESK_TRADER_ID = GlobalMatchingTrader
                                nq0.ORDER_EXCHANGES = GlobalExchangeStr
                                nas0.q = nq0
                                nas0.LOT_SIZE = SpreadTupple.LOT_SIZE
                                nas0.ACCOUNT_DESK_ID = GlobalMatchingDesk

                                nas0.SETTLEMENT_TICK = SpreadTupple.SETTLEMENT_TICK
                                nas0.s = Nothing
                                nas0.Spread = Nothing
                                olc.Add(nas0, nq0.ORDER_ID.ToString())
                            Next k
                        End If
                    Next

                    MIPPrepare(gdb, olc, nc, BidId, BidE, UserInfo)

                    Dim MarketDepth2 As Collection
                    MarketDepth2 = MarketMIPOrders(gdb, nc, BidId, OrderCollection, MIPPrefBonus, BidE, True, RPPLCol, BidId, BidE)


                    For Each nas As AdjustSpreadsClass In olc
                        If IsNothing(nas.q) = False Then Continue For
                        If IsNothing(nas.s) Then nas.s = nas.Spread
                        If IsNothing(nas.s) Then Continue For
                        If IsNothing(nas.AffectedLeg) Then Continue For

                        Dim CurrId As Integer = nas.AffectedLeg.ORDER_ID
                        Dim Pr As Double = 0
                        Dim Cap As Double = 0
                        Dim OrderQuantity As Integer = 0

                        If Not IsNothing(MarketDepth2) Then
                            For Each oc As MARKET_MATCHING_CLASS In MarketDepth2
                                If oc.BuyOrder.ORDER_ID = CurrId And oc.SellOrder.ORDER_ID >= BidId Then
                                    'Pr = Round(oc.Price, oc.Tick, , 1)
                                    Pr = oc.Price
                                    If nas.MinPrice < Pr And Pr < nas.MaxPrice Then
                                        Cap = Cap + Pr * Int(oc.ActualQuantity)
                                        OrderQuantity += Int(oc.ActualQuantity)
                                    End If
                                ElseIf oc.SellOrder.ORDER_ID = CurrId And oc.BuyOrder.ORDER_ID >= BidId Then
                                    'Pr = Round(oc.Price, oc.Tick, , -1)
                                    Pr = oc.Price
                                    If nas.MinPrice < Pr And Pr < nas.MaxPrice Then
                                        Cap = Cap + Pr * Int(oc.ActualQuantity)
                                        OrderQuantity += Int(oc.ActualQuantity)
                                    End If
                                Else
                                    Continue For
                                End If
                            Next
                        End If

                        'If (nas.s.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket And OrderQuantity > 0) Or _
                        '   (OrderQuantity = nas.AffectedLegQuantinty) Then
                        If (OrderQuantity > 0) Then
                            Pr = Cap / OrderQuantity
                            Dim Price As Double = 0
                            Dim Q As Integer = nas.CurrQ
                            'If nas.s.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then
                            Q = OrderQuantity
                            'If Q > nas.AffectedLegQuantinty Then Q = nas.AffectedLegQuantinty
                            'End If
                            Price = Round(Pr, nas.SETTLEMENT_TICK, , 0)
                            If nas.LegBS = "B" Then
                                '    If Price < nas.LegPrice Then
                                nas.LegPrice = Price
                                nas.LegQ = Q
                                'End If
                            Else
                                'If Price > nas.LegPrice Then
                                nas.LegPrice = Price
                                nas.LegQ = Q
                                'End If
                            End If
                        End If

                        If nas.CurrPrice <> nas.LegPrice Or nas.CurrQ <> nas.LegQ Then
                            Dim UpdateOrder As New ORDERS_FFA_CLASS
                            AdjustSpreads2 = UpdateOrder.GetFromObject(nas.AffectedLeg)
                            If AdjustSpreads2 <> ArtBErrors.Success Then
                                UpdateOrder = Nothing
                                Exit Function
                            End If
                            UpdateOrder.PRICE_INDICATED = nas.LegPrice
                            If nas.CurrQ <> nas.LegQ Then
                                UpdateOrder.ORDER_QUANTITY = nas.LegQ
                                If UpdateOrder.DAY_QUALIFIER > OrderDayQualifier.NotInDays Then
                                    UpdateOrder.DAY_QUALIFIER = OrderDayQualifier.DPM
                                End If
                            End If
                            If IsNothing(gdb) Then
                                InsertOrReplaceOrder(UpdateOrder, RouteList, FirmUPList, NegotiationList, DirectHitFailedList)
                            Else
                                AdjustSpreads2 = InsertUpdateOrder(gdb, UpdateOrder)
                                If AdjustSpreads2 <> ArtBErrors.Success Then
                                    UpdateOrder = Nothing
                                    Exit Function
                                End If
                                AdjustSpreads2 = UpdateOrder.AppendToStr(OrderStr)
                                If AdjustSpreads2 <> ArtBErrors.Success Then
                                    UpdateOrder = Nothing
                                    Exit Function
                                End If

                            End If

                            If Not IsNothing(NewOrderCollection) Then
                                InsertOrReplace(NewOrderCollection, UpdateOrder, UpdateOrder.ORDER_ID.ToString())
                            ElseIf Not IsNothing(gdb) Then
                                UpdateOrder = Nothing
                            End If
                        End If
                    Next
                Next
            Next

            If IsNothing(UserInfo) = False Then
                For Each nas As AdjustSpreadsClass In Spreads
                    If IsNothing(nas.Spread) Then Continue For
                    Dim nc2 As New Collection
                    Dim olc2 As New Collection
                    If UserInfo.IsTrader And nas.Spread.FOR_DESK_TRADER_ID <> UserInfo.TraderID Then Continue For
                    Dim Spread As New ORDERS_FFA_CLASS
                    Spread.GetFromObject(nas.Spread)

                    For Each co In originalOLC
                        olc2.Add(co, co.q.ORDER_ID.ToString())
                    Next
                    If Spread.ORDER_TYPE = OrderTypes.RatioSpread Then
                        If Spread.ORDER_BS = "B" Then
                            Spread.PRICE_INDICATED = 1000
                        Else
                            Spread.PRICE_INDICATED = 0.001
                        End If
                    Else
                        If Spread.ORDER_BS = "B" Then
                            Spread.PRICE_INDICATED = MIPTopPrice
                        Else
                            Spread.PRICE_INDICATED = -MIPTopPrice
                        End If
                    End If
                    Spread.ORDER_ID += SynthOrderOffset
                    Dim nas1 As New AdjustSpreadsClass
                    Dim nq1 As ORDERS_FFA_CLASS = Spread.GetSpreadLeg(1)
                    Dim LegID1 As Integer = 2000000000
                    nq1.ORDER_ID = LegID1
                    nq1.SPREAD_ORDER_ID = Spread.ORDER_ID
                    Spread.CROSS_ORDER_ID1 = nq1.ORDER_ID
                    nas1.MinPrice = -1.0E+20
                    nas1.MaxPrice = 1.0E+20
                    nas1.q = nq1
                    nas1.LOT_SIZE = nas.LOT_SIZE
                    nas1.ACCOUNT_DESK_ID = nas.ACCOUNT_DESK_ID
                    nas1.SETTLEMENT_TICK = nas.SETTLEMENT_TICK
                    nas1.Spread = Spread
                    nas1.s = Spread
                    olc2.Add(nas1, nq1.ORDER_ID.ToString())

                    Dim nas2 As New AdjustSpreadsClass
                    Dim nq2 As ORDERS_FFA_CLASS = Spread.GetSpreadLeg(2)
                    Dim LegID2 As Integer = 2000000001
                    nq2.ORDER_ID = LegID2
                    nq2.SPREAD_ORDER_ID = Spread.ORDER_ID
                    Spread.CROSS_ORDER_ID1 = nq2.ORDER_ID
                    nas2.MinPrice = -1.0E+20
                    nas2.MaxPrice = 1.0E+20
                    nas2.q = nq2
                    nas2.LOT_SIZE = nas.LOT_SIZE
                    nas2.ACCOUNT_DESK_ID = nas.ACCOUNT_DESK_ID
                    nas2.SETTLEMENT_TICK = nas.SETTLEMENT_TICK
                    nas2.Spread = Spread
                    nas2.s = Spread
                    olc2.Add(nas2, nq2.ORDER_ID.ToString())

                    Debug.Print("------------------------------------------------------------------------------")
#If DEBUG Then
                    Dim rs As String = ""
                    OrderDescr(Spread, 4, rs)
                    Debug.Print(rs)

#End If
                    If Not IsNothing(e) Then
                        If e.Cancel = True Then
                            e.Cancel = False
                            Debug.Print("Adj Spreads restarted at:" & DateTime.UtcNow.ToString())
                            GoTo AdjustSpreads2Restart
                        End If
                    End If

                    MIPPrepare(gdb, olc2, nc2, LegID1, LegID2, , Spread.ORDER_ID)

                    If nc2.Count > 0 Then
                        Dim MIPRet As Collection
                        MIPRet = MarketMIPOrders(gdb, nc2, LegID1, OrderCollection, MIPPrefBonus, LegID2, False, RPPLCol)
                        If IsNothing(MIPRet) = False AndAlso MIPRet.Count > 0 Then
                            Dim Cap1 As Double = 0
                            Dim OQ1 As Double = 0
                            Dim Cap2 As Double = 0
                            Dim OQ2 As Double = 0
                            Dim Pr As Double
                            Dim Tick As Double = 0

                            Dim bSameTRP As Boolean = True
                            Dim OrderList As New List(Of Integer)
                            For Each oc As MARKET_MATCHING_CLASS In MIPRet
                                If oc.BuyOrder.ORDER_ID = LegID1 Then
                                    If Not IsNothing(oc.SellOrderSpread) Then
                                        If OrderList.Contains(oc.SellOrderSpread.ORDER_ID) = False Then
                                            OrderList.Add(oc.SellOrderSpread.ORDER_ID)
                                        End If
                                        bSameTRP = bSameTRP And oc.BuyOrderSpread.SameTypeRoutePeriod(oc.SellOrderSpread)
                                    End If
                                    Pr = oc.Price
                                    If Tick = 0 Then Tick = oc.Tick
                                    If oc.Tick < Pr And Pr < MIPTopPrice * oc.Tick Then
                                        Cap1 += Pr * Int(oc.ActualQuantity)
                                        OQ1 += Int(oc.ActualQuantity)
                                    End If
                                ElseIf oc.SellOrder.ORDER_ID = LegID1 Then
                                    If Not IsNothing(oc.BuyOrderSpread) Then
                                        If OrderList.Contains(oc.BuyOrderSpread.ORDER_ID) = False Then
                                            OrderList.Add(oc.BuyOrderSpread.ORDER_ID)
                                        End If
                                        bSameTRP = bSameTRP And oc.SellOrderSpread.SameTypeRoutePeriod(oc.BuyOrderSpread)
                                    End If

                                    Pr = oc.Price
                                    If Tick = 0 Then Tick = oc.Tick
                                    If oc.Tick < Pr And Pr < MIPTopPrice * oc.Tick Then
                                        Cap1 += Pr * Int(oc.ActualQuantity)
                                        OQ1 += Int(oc.ActualQuantity)
                                    End If
                                ElseIf oc.BuyOrder.ORDER_ID = LegID2 Then
                                    If Not IsNothing(oc.SellOrderSpread) Then
                                        If OrderList.Contains(oc.SellOrderSpread.ORDER_ID) = False Then
                                            OrderList.Add(oc.SellOrderSpread.ORDER_ID)
                                        End If
                                        bSameTRP = bSameTRP And oc.BuyOrderSpread.SameTypeRoutePeriod(oc.SellOrderSpread)
                                    End If
                                    Pr = oc.Price
                                    If Tick = 0 Then Tick = oc.Tick
                                    If oc.Tick < Pr And Pr < MIPTopPrice * oc.Tick Then
                                        Cap2 += Pr * Int(oc.ActualQuantity)
                                        OQ2 += Int(oc.ActualQuantity)
                                    End If
                                ElseIf oc.SellOrder.ORDER_ID = LegID2 Then
                                    If Not IsNothing(oc.BuyOrderSpread) Then
                                        If OrderList.Contains(oc.BuyOrderSpread.ORDER_ID) = False Then
                                            OrderList.Add(oc.BuyOrderSpread.ORDER_ID)
                                        End If
                                        bSameTRP = bSameTRP And oc.SellOrderSpread.SameTypeRoutePeriod(oc.BuyOrderSpread)
                                    End If
                                    Pr = oc.Price
                                    If Tick = 0 Then Tick = oc.Tick
                                    If oc.Tick < Pr And Pr < MIPTopPrice * oc.Tick Then
                                        Cap2 += Pr * Int(oc.ActualQuantity)
                                        OQ2 += Int(oc.ActualQuantity)
                                    End If
                                End If
                            Next

                            If OQ1 > 0 And OQ2 > 0 And (bSameTRP = False Or OrderList.Count <> 1) Then
                                Dim nso As New ORDERS_FFA_CLASS
                                nso.GetFromObject(Spread)

                                If nso.ORDER_TYPE = OrderTypes.RatioSpread Then
                                    nso.PRICE_INDICATED = (Cap1 / OQ1) / (Cap2 / OQ2)
                                    Tick = 0.001
                                Else
                                    nso.PRICE_INDICATED = (Cap1 / OQ1) - (Cap2 / OQ2)
                                End If
                                nso.LIVE_STATUS = "P"
                                If nso.ORDER_BS = "B" Then
                                    nso.ORDER_BS = "S"
                                    nso.PRICE_INDICATED = Round(nso.PRICE_INDICATED, Tick, , 1)
                                Else
                                    nso.ORDER_BS = "B"
                                    nso.PRICE_INDICATED = Round(nso.PRICE_INDICATED, Tick, , -1)
                                End If
                                nso.CROSS_ORDER_ID1 = Nothing
                                nso.CROSS_ORDER_ID2 = Nothing
                                nso.SetQuantity(OQ1)
                                If IsNothing(gdb) Then
                                    InsertOrReplaceOrder(nso, RouteList, FirmUPList, NegotiationList, DirectHitFailedList)
                                End If

                                If Not IsNothing(NewOrderCollection) Then
                                    InsertOrReplace(NewOrderCollection, nso, nso.ORDER_ID.ToString())
                                End If

                            End If
                            OrderList.Clear()
                            OrderList = Nothing
                        End If
                    End If
                Next ' Spread
            End If
        Next ' Tradeclass
        RefCol.Clear()
        If Not IsNothing(e) Then
            If e.Cancel = True Then
                e.Cancel = False
                Debug.Print("Adj Spreads restarted at:" & DateTime.UtcNow.ToString())
                GoTo AdjustSpreads2Restart
            End If
        End If

    End Function

    Public Function ConstrainedPrice(ByRef gdb As DB_ARTB_NETDataContext, _
                                     ByRef bcp As Double, _
                                     ByRef scp As Double, _
                                     ByRef OrderClass As Object, _
                                     ByRef RoutePeriodList As List(Of String), _
                                     Optional ByVal PriceTick As Double = 1, _
                                     Optional ByRef OrderCollection As Collection = Nothing) As Integer
        ConstrainedPrice = 0
        bcp = -1.0E+20
        scp = 1.0E+20
        If IsNothing(OrderClass) Then Exit Function
        Dim mm1 As Integer = OrderClass.MM1
        Dim mm2 As Integer = OrderClass.MM2
        Dim yy1 As Integer = OrderClass.YY1
        Dim yy2 As Integer = OrderClass.YY2
        Dim RouteID As Integer = OrderClass.ROUTE_ID
        Dim PotentialMatchingOrders As Object = Nothing
        Dim d1 As DateTime = OrderClass.ORDER_DATETIME
        Dim d As Date = d1.Date

        Try
            If IsNothing(gdb) Then
                PotentialMatchingOrders = From q In OrderCollection _
                                Where _
                                q.ROUTE_ID = RouteID And _
                                q.YY1 = yy1 And _
                                q.MM1 = mm1 And _
                                q.YY2 = yy2 And _
                                q.MM2 = mm2 And _
                                q.LIVE_STATUS = "A" And _
                                 q.ORDER_TYPE = OrderTypes.FFA And _
                                (q.ORDER_DATETIME >= d Or q.ORDER_GOOD_TILL = OrderGoodTill.GTC) _
                            Select q
            Else
                PotentialMatchingOrders = From q In gdb.ORDERS_FFAs _
                                Where _
                                q.ROUTE_ID = RouteID And _
                                q.YY1 = yy1 And _
                                q.MM1 = mm1 And _
                                q.YY2 = yy2 And _
                                q.MM2 = mm2 And _
                                q.LIVE_STATUS = "A" And _
                                 q.ORDER_TYPE = OrderTypes.FFA And _
                                (q.ORDER_DATETIME >= d Or q.ORDER_GOOD_TILL = OrderGoodTill.GTC) _
                            Select q
            End If
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try

        If IsNothing(PotentialMatchingOrders) Then Exit Function
        For Each q In PotentialMatchingOrders
            Dim price As Double = -1.0E+20
            If NullInt2Int(q.SPREAD_ORDER_ID) = 0 Then
                price = q.PRICE_INDICATED
            End If
            If price > -1.0E+20 And price < 1.0E+20 Then
                If bcp < price Then bcp = price
                If scp > price Then scp = price
            End If
        Next
        ConstrainedPrice = 1
    End Function

    Public Function SP(ByRef gdb As DB_ARTB_NETDataContext, _
                  ByRef OrderCollection As Collection, _
                  ByRef RPPLCol As Collection, _
                  Optional ByVal PrefOrderId As Integer = 2000000000, _
                  Optional ByVal PrefOrderIdEnd As Integer = 2000000000, _
                  Optional ByVal bAffectBestBuySell As Boolean = True) As Boolean
        If IsNothing(OrderCollection) Then Return False
        Dim StartTime As DateTime = DateTime.UtcNow
        Dim lp As New LPProblem
        Dim bOneFound As Boolean = False
        For Each ord As ORDERS_FFA_CLASS In OrderCollection
            If ord.ORDER_TYPE <> OrderTypes.FFA Then Continue For
            If ord.PRICE_INDICATED = TargetPrice(ord) Then Continue For

            Dim RPPL As RoutePeriodPriceLimits
            Dim RPStr As String = RoutePeriodStringFromObj(ord)
            RPPL = GetViewClass(RPPLCol, RPStr)

            If IsNothing(RPPL) Then
                RPPL = New RoutePeriodPriceLimits(lp, RPStr)
                RPPL.RPStr = RPStr
                RPPLCol.Add(RPPL, RPStr)
            End If

            If NullInt2Int(ord.SPREAD_ORDER_ID) = 0 Then
                If (ord.ORDER_ID < PrefOrderId Or ord.ORDER_ID > PrefOrderIdEnd) Then
                    RPPL.ApplySwapPrice(lp, ord.PRICE_INDICATED, "", ord.ORDER_BS, ord.ORDER_DATETIME)
                    bOneFound = True
                Else
                    If bAffectBestBuySell = False Then
                        RPPL.ApplySwapPrice(lp, ord.PRICE_INDICATED, "", "", ord.ORDER_DATETIME)
                        bOneFound = True
                    End If
                End If
            End If
        Next

        Dim SpreadCollection As New Collection
        For Each OrderClass As ORDERS_FFA_CLASS In OrderCollection
            If OrderClass.ORDER_TYPE <> OrderTypes.RatioSpread And _
               OrderClass.ORDER_TYPE <> OrderTypes.CalendarSpread And _
               OrderClass.ORDER_TYPE <> OrderTypes.PriceSpread Then Continue For
            If OrderClass.PRICE_INDICATED = TargetPrice(OrderClass) Then Continue For
            Dim RPPL1 As RoutePeriodPriceLimits
            Dim RPPL2 As RoutePeriodPriceLimits
            Dim RPStr1 As String = RoutePeriodStringFromObj(OrderClass, 1)
            RPPL1 = GetViewClass(RPPLCol, RPStr1)
            Dim RPStr2 As String = RoutePeriodStringFromObj(OrderClass, 2)
            RPPL2 = GetViewClass(RPPLCol, RPStr2)
            If IsNothing(RPPL1) Then
                RPPL1 = New RoutePeriodPriceLimits(lp, RPStr1)
                RPPL1.RPStr = RPStr1
                RPPLCol.Add(RPPL1, RPStr1)
            End If
            If IsNothing(RPPL2) Then
                RPPL2 = New RoutePeriodPriceLimits(lp, RPStr2)
                RPPL2.RPStr = RPStr2
                RPPLCol.Add(RPPL2, RPStr2)
            End If

            Dim SRPStr As String = RoutePeriodStringFromObj(OrderClass, 0)
            Dim LowP As Double = OrderClass.PRICE_INDICATED
            Dim HighP As Double = OrderClass.PRICE_INDICATED
            Dim CurrConstr As Integer

            Dim SL As RoutePeriodSpreadPriceLimits
            If SpreadCollection.Contains(SRPStr) Then
                SL = SpreadCollection(SRPStr)
                SL.Low = Min(SL.Low, LowP)
                SL.High = Max(SL.High, HighP)
            Else
                SL = New RoutePeriodSpreadPriceLimits
                SL.SRPStr = SRPStr
                SL.RPPL1 = RPPL1
                SL.RPPL2 = RPPL2
                SL.Low = LowP
                SL.High = HighP
                SL.OrderType = OrderClass.ORDER_TYPE
                SpreadCollection.Add(SL, SRPStr)
                SL.SpreadOrder = OrderClass
            End If
        Next

        For Each SL As RoutePeriodSpreadPriceLimits In SpreadCollection
            Dim CurrConstr As Integer
            Dim RPPL1 As RoutePeriodPriceLimits = SL.RPPL1
            Dim RPPL2 As RoutePeriodPriceLimits = SL.RPPL2
            RPPL1.Bounded = RPPL1.Bounded Or RPPL2.Bounded
            RPPL2.Bounded = RPPL1.Bounded Or RPPL2.Bounded
        Next

#If DEBUG Then
        Dim Ts0 As TimeSpan = DateTime.UtcNow - StartTime
        Debug.Print("SP0 Handl Time: " & Ts0.TotalMilliseconds.ToString())
#End If

        bOneFound = True
        While bOneFound
            bOneFound = False
            For Each SL As RoutePeriodSpreadPriceLimits In SpreadCollection
                Dim RPPL1 As RoutePeriodPriceLimits = SL.RPPL1
                Dim RPPL2 As RoutePeriodPriceLimits = SL.RPPL2
                If (RPPL1.Bounded = True And RPPL2.Bounded = False) Or _
                   (RPPL1.Bounded = False And RPPL2.Bounded = True) Then bOneFound = True
                RPPL1.Bounded = RPPL1.Bounded Or RPPL2.Bounded
                RPPL2.Bounded = RPPL1.Bounded Or RPPL2.Bounded
            Next
        End While

#If DEBUG Then
        Dim Ts1 As TimeSpan = DateTime.UtcNow - StartTime
        Debug.Print("SP1 Handl Time: " & Ts1.TotalMilliseconds.ToString())
#End If

        For Each SL As RoutePeriodSpreadPriceLimits In SpreadCollection
            If IsNothing(SL.SpreadOrder) Then Continue For
            Dim CurrConstr As Integer
            Dim RPPL1 As RoutePeriodPriceLimits = SL.RPPL1
            Dim AvgRefPrice As Double = 0
            Dim Leg1Order As ORDERS_FFA_CLASS = Nothing
            Dim Leg2Order As ORDERS_FFA_CLASS = Nothing
            Dim Level As Integer = 0
            Dim Command As String = "T"
            Dim ODT As New DateTime
            If RPPL1.Bounded = False Then
                Leg1Order = GetViewClass(OrderCollection, SL.SpreadOrder.CROSS_ORDER_ID1.ToString())
                If Not IsNothing(Leg1Order) Then
                    AvgRefPrice = GetAvgReferencePrice(gdb, Leg1Order, OrderCollection, , True, False, Level, ODT)
                End If
                If Level = 20 Then Command = "C"
                If AvgRefPrice > 0 And AvgRefPrice < MIPTopPrice Then
                    RPPL1.ApplySwapPrice(lp, AvgRefPrice, "", Command, ODT)
                End If
            ElseIf Math.Abs(RPPL1.LastTrade) > 1.0E+19 Then
                Dim RefC As REF_CLASS = GetViewClass(RefCol, RPPL1.RPStr)
                If Not IsNothing(RefC) Then
                    If Not IsNothing(RefCol) And Math.Abs(RefC.LastTrade) < 1.0E+19 Then
                        RPPL1.LastTrade = RefC.LastTrade
                        If RPPL1.LastIsClose Then Command = "C"
                        RPPL1.ApplySwapPrice(lp, RefC.LastTrade, "", Command, RefC.TradeDateTime)
                    End If
                Else
                    Leg1Order = GetViewClass(OrderCollection, SL.SpreadOrder.CROSS_ORDER_ID1.ToString())
                    If Not IsNothing(Leg1Order) Then
                        AvgRefPrice = GetAvgReferencePrice(gdb, Leg1Order, OrderCollection, , True, False, Level, ODT)
                    End If
                    If Level = 20 Then Command = "C"
                    If AvgRefPrice > 0 And AvgRefPrice < MIPTopPrice Then
                        RPPL1.ApplySwapPrice(lp, AvgRefPrice, "", Command, ODT)
                    End If
                End If
            End If
            AvgRefPrice = 0
            Command = "T"
            Level = 0
            Dim RPPL2 As RoutePeriodPriceLimits = SL.RPPL2
            If RPPL2.Bounded = False Then
                Leg2Order = GetViewClass(OrderCollection, SL.SpreadOrder.CROSS_ORDER_ID2.ToString())

                If Not IsNothing(Leg2Order) Then
                    AvgRefPrice = GetAvgReferencePrice(gdb, Leg2Order, OrderCollection, , True, False, Level, ODT)
                End If
                If Level = 20 Then Command = "C"

                If AvgRefPrice > 0 And AvgRefPrice < MIPTopPrice Then
                    RPPL2.ApplySwapPrice(lp, AvgRefPrice, "", Command, ODT)
                End If
            ElseIf Math.Abs(RPPL2.LastTrade) > 1.0E+19 Then
                Dim RefC As REF_CLASS = GetViewClass(RefCol, RPPL2.RPStr)
                If Not IsNothing(RefC) Then
                    If Not IsNothing(RefCol) And Math.Abs(RefC.LastTrade) < 1.0E+19 Then
                        RPPL2.LastTrade = RefC.LastTrade
                        If RPPL2.LastIsClose Then Command = "C"
                        RPPL2.ApplySwapPrice(lp, RefC.LastTrade, "", Command, RefC.TradeDateTime)
                    End If
                    'Else
                    Leg2Order = GetViewClass(OrderCollection, SL.SpreadOrder.CROSS_ORDER_ID2.ToString())

                    If Not IsNothing(Leg2Order) Then
                        AvgRefPrice = GetAvgReferencePrice(gdb, Leg2Order, OrderCollection, , True, False, Level, ODT)
                    End If
                    If Level = 20 Then Command = "C"

                    If AvgRefPrice > 0 And AvgRefPrice < MIPTopPrice Then
                        RPPL2.ApplySwapPrice(lp, AvgRefPrice, "", Command, ODT)
                    End If
                End If
            End If
        Next

#If DEBUG Then
        Dim Ts2 As TimeSpan = DateTime.UtcNow - StartTime
        Debug.Print("SP2 Handl Time: " & Ts2.TotalMilliseconds.ToString())
#End If
        bOneFound = True
        While bOneFound
            bOneFound = False
            For Each SL As RoutePeriodSpreadPriceLimits In SpreadCollection
                Dim RPPL1 As RoutePeriodPriceLimits = SL.RPPL1
                Dim RPPL2 As RoutePeriodPriceLimits = SL.RPPL2

                For Each rpp In RPPL1.PriceList
                    If InStr(rpp.PathKeyStr, RPPL2.RPStr) <= 0 Then
                        Dim pLow As Double = rpp.Price
                        Dim pHigh As Double = rpp.Price
                        Dim DHL As Double
                        If SL.OrderType = OrderTypes.RatioSpread Then
                            DHL = SL.High / SL.Low
                            pLow /= SL.Low
                            pLow /= DHL
                            pHigh /= SL.Low
                            pHigh *= DHL
                        Else
                            DHL = SL.High - SL.Low
                            pLow -= (SL.Low + DHL)
                            pHigh -= (SL.Low - DHL)
                        End If
                        bOneFound = bOneFound Or RPPL2.ApplyPriceFromSpread(lp, pLow, pHigh, RPPL1.RPStr, SL.SRPStr, rpp.PathKeyStr)
                        pLow = rpp.Price
                        pHigh = rpp.Price
                        If SL.OrderType = OrderTypes.RatioSpread Then
                            DHL = SL.High / SL.Low
                            pLow /= SL.High
                            pLow /= DHL
                            pHigh /= SL.High
                            pHigh *= DHL
                        Else
                            DHL = SL.High - SL.Low
                            pLow -= (SL.High + DHL)
                            pHigh -= (SL.High - DHL)
                        End If
                        bOneFound = bOneFound Or RPPL2.ApplyPriceFromSpread(lp, pLow, pHigh, RPPL1.RPStr, SL.SRPStr, rpp.PathKeyStr)
                    End If
                Next

                For Each rpp In RPPL2.PriceList
                    If InStr(rpp.PathKeyStr, RPPL1.RPStr) <= 0 Then
                        Dim pLow As Double = rpp.Price
                        Dim pHigh As Double = rpp.Price
                        Dim DHL As Double
                        If SL.OrderType = OrderTypes.RatioSpread Then
                            DHL = SL.High / SL.Low
                            pLow *= SL.Low
                            pLow /= DHL
                            pHigh *= SL.Low
                            pHigh *= DHL
                        Else
                            DHL = SL.High - SL.Low
                            pLow += (SL.Low - DHL)
                            pHigh += (SL.Low + DHL)
                        End If
                        bOneFound = bOneFound Or RPPL1.ApplyPriceFromSpread(lp, pLow, pHigh, RPPL2.RPStr, SL.SRPStr, rpp.PathKeyStr)
                        pLow = rpp.Price
                        pHigh = rpp.Price
                        If SL.OrderType = OrderTypes.RatioSpread Then
                            DHL = SL.High / SL.Low
                            pLow *= SL.High
                            pLow /= DHL
                            pHigh *= SL.High
                            pHigh *= DHL
                        Else
                            DHL = SL.High - SL.Low
                            pLow += (SL.High - DHL)
                            pHigh += (SL.High + DHL)
                        End If
                        bOneFound = bOneFound Or RPPL1.ApplyPriceFromSpread(lp, pLow, pHigh, RPPL2.RPStr, SL.SRPStr, rpp.PathKeyStr)
                    End If
                Next
            Next
        End While

        For Each RPPL As RoutePeriodPriceLimits In RPPLCol
            RPPL.Volatility = Volatility(RPPL.RPStr)
            RPPL.CalcSpreadLimits(RefCol)
        Next

        If lp.ColumnsNum <= 1 Then
            lp.Destroy()
            Return False
        End If
        If lp.Solve > -1.0E+20 Then

            For i = 1 To lp.ColumnsNum - 1
                If Len(lp.Columns(i).Name) > 0 Then
                    Debug.Print(lp.Columns(i).Name & ": " & lp.GetVariableValue(i))
                Else
                    Debug.Print("V" & i & ": " & lp.GetVariableValue(i))
                End If
            Next
            Dim k As Double
            For Each RPPL As RoutePeriodPriceLimits In RPPLCol
                If RPPL.LowV <> -1 Then
                    k = lp.GetVariableValue(RPPL.LowV)
                    RPPL.Low = k
                End If
                If RPPL.HighV <> -1 Then
                    k = lp.GetVariableValue(RPPL.HighV)
                    RPPL.High = k
                End If
            Next
        End If

#If DEBUG Then
        Dim EndTime As DateTime = DateTime.UtcNow
        Dim Ts As TimeSpan = EndTime - StartTime
        Debug.Print("SP Start Time: " & StartTime.ToString())
        Debug.Print("SP End   Time: " & EndTime.ToString())
        Debug.Print("SP Handl Time: " & Ts.TotalMilliseconds.ToString())
#End If

        Return True
    End Function

    Public Sub HandleOrders(ByRef FirmUPList As List(Of Integer), _
                            ByRef NegotiationList As List(Of Integer), _
                            ByRef DirectHitFailedList As List(Of Integer))
        For Each OrderId As Integer In FirmUPList
            RaiseEvent OrderFFARequest(OrderId)
        Next

        FirmUPList.Clear()
        FirmUPList = Nothing

        For Each OrderId As Integer In NegotiationList
            RaiseEvent OrderFFANegotiation(OrderId)
        Next

        NegotiationList.Clear()
        NegotiationList = Nothing

        For Each OrderId As Integer In DirectHitFailedList
            RaiseEvent OrderFFADirectHitFailed(OrderId)
        Next

        DirectHitFailedList.Clear()
        DirectHitFailedList = Nothing

    End Sub

    Public Sub MIPSchema(ByRef gdb As DB_ARTB_NETDataContext, _
                          ByRef nc As Collection)
#If DEBUG Then
        If Not IsNothing(gdb) Then Exit Sub
        Dim i As Integer = 1
        For Each oc As MARKET_MATCHING_CLASS In nc
            Dim s1 As String = ""
            Dim s2 As String = ""
            OrderDescr(oc.BuyOrder, 4, s1)
            OrderDescr(oc.SellOrder, 4, s2)
            s1 = oc.BuyOrder.ORDER_ID.ToString() & "[" & s1 & "]"
            s2 = oc.SellOrder.ORDER_ID.ToString() & "[" & s2 & "]"

            Debug.Print("Q" & i.ToString() & ": " & s1 & " <-> " & s2)
            i += 1
        Next
#End If

    End Sub

    Public Sub MIPExecuted(ByRef gdb As DB_ARTB_NETDataContext, _
                      ByRef nc As Collection)
#If DEBUG Then
        If Not IsNothing(gdb) Then Exit Sub
        Dim i As Integer = 1
        For Each oc As MARKET_MATCHING_CLASS In nc
            If oc.VariableEQ < 0 Then Continue For
            Dim s1 As String = ""
            Dim s2 As String = ""
            OrderDescr(oc.BuyOrder, 4, s1)
            OrderDescr(oc.SellOrder, 4, s2)
            s1 = oc.BuyOrder.ORDER_ID.ToString() & "[" & s1 & "]"
            s2 = oc.SellOrder.ORDER_ID.ToString() & "[" & s2 & "]"

            Dim s As String = "", k As Double = 0
            If oc.Price > 0 And oc.ActualQuantity > 0 Then
                s = "#" & i.ToString() & ", Q" & oc.ID.ToString & ": Exchange:" & oc.ExchangeID.ToString() & " : " & oc.ActualQuantity & " x " & oc.Price & vbNewLine & s1 & vbNewLine & s2
                Debug.Print(s)
            End If
            i += 1
        Next
#End If

    End Sub

    Public Sub MIPPrepare(ByRef gdb As DB_ARTB_NETDataContext, _
                          ByRef olc As Collection, _
                          ByRef nc As Collection, _
                          ByVal BidId As Integer, _
                          ByVal BidE As Integer, _
                          Optional ByRef UserInfo As TraderInfoClass = Nothing, _
                          Optional ByVal ExcludeSpreadOrderId As Integer = 0)
        Dim ExchangeList = New List(Of Integer)
        For Each co As AdjustSpreadsClass In olc
            If IsNothing(co.q) Then Continue For
            If co.q.ORDER_BS <> "B" Then Continue For
            If co.q.ORDER_TYPE <> OrderTypes.FFA Then Continue For
            For Each co2 As AdjustSpreadsClass In olc
                If IsNothing(co2.q) Then Continue For
                If co.ACCOUNT_DESK_ID = co2.ACCOUNT_DESK_ID Then Continue For
                If co2.q.ORDER_BS <> "S" Then Continue For
                If co2.q.ORDER_TYPE <> OrderTypes.FFA Then Continue For
                If MatchingOrders(gdb, co.q, co2.q, ExchangeList, , False) Then
                    For Each ExchangeId As Integer In ExchangeList
                        Dim nmoc As MARKET_MATCHING_CLASS = New MARKET_MATCHING_CLASS
                        nmoc.BuyOrder = co.q
                        If Not IsNothing(nmoc.BuyOrder.SPREAD_ORDER_ID) Then
                            nmoc.BuyOrderSpread = co.s
                            If co.s.ORDER_ID = ExcludeSpreadOrderId And nmoc.BuyOrder.ORDER_ID < BidId Then
                                nmoc = Nothing
                                Continue For
                            End If
                        End If
                        nmoc.SellOrder = co2.q
                        If Not IsNothing(nmoc.SellOrder.SPREAD_ORDER_ID) Then
                            nmoc.SellOrderSpread = co2.s
                            If co2.s.ORDER_ID = ExcludeSpreadOrderId And nmoc.SellOrder.ORDER_ID < BidId Then
                                nmoc = Nothing
                                Continue For
                            End If
                        End If
                        If nmoc.BuyOrder.ORDER_ID >= BidId And nmoc.SellOrder.ORDER_ID >= BidId Then
                            nmoc = Nothing
                            Continue For
                        End If

                        nmoc.ActualQuantity = 0
                        nmoc.ExchangeID = ExchangeId
                        If IsNothing(gdb) Then
                            If co.ACCOUNT_DESK_ID <> GlobalMatchingDesk Then
                                nmoc.BuyExchangeRank = GetDeskExchangeRanking(co.ACCOUNT_DESK_ID, ExchangeId, nmoc.BuyOrder.ROUTE_ID)
                            ElseIf Not IsNothing(UserInfo) Then
                                nmoc.BuyExchangeRank = GetDeskExchangeRanking(UserInfo.DeskID, ExchangeId, nmoc.BuyOrder.ROUTE_ID)
                            End If
                            If co2.ACCOUNT_DESK_ID <> GlobalMatchingDesk Then
                                nmoc.SellExchangeRank = GetDeskExchangeRanking(co2.ACCOUNT_DESK_ID, ExchangeId, nmoc.SellOrder.ROUTE_ID)
                            ElseIf Not IsNothing(UserInfo) Then
                                nmoc.SellExchangeRank = GetDeskExchangeRanking(UserInfo.DeskID, ExchangeId, nmoc.SellOrder.ROUTE_ID)
                            End If

                        Else
                            If co.ACCOUNT_DESK_ID <> GlobalMatchingDesk Then
                                nmoc.BuyExchangeRank = GetDeskExchangeRankingFromDB(gdb, co.ACCOUNT_DESK_ID, ExchangeId, nmoc.BuyOrder.ROUTE_ID)
                            ElseIf Not IsNothing(UserInfo) Then
                                nmoc.BuyExchangeRank = GetDeskExchangeRankingFromDB(gdb, UserInfo.DeskID, ExchangeId, nmoc.BuyOrder.ROUTE_ID)
                            End If
                            If co2.ACCOUNT_DESK_ID <> GlobalMatchingDesk Then
                                nmoc.SellExchangeRank = GetDeskExchangeRankingFromDB(gdb, co2.ACCOUNT_DESK_ID, ExchangeId, nmoc.SellOrder.ROUTE_ID)
                            ElseIf Not IsNothing(UserInfo) Then
                                nmoc.SellExchangeRank = GetDeskExchangeRankingFromDB(gdb, UserInfo.DeskID, ExchangeId, nmoc.SellOrder.ROUTE_ID)
                            End If
                        End If

                        If nmoc.BuyExchangeRank = 0 And Not IsNothing(UserInfo) Then

                        End If
                        If nmoc.SellExchangeRank = 0 And Not IsNothing(UserInfo) Then

                        End If

                        nmoc.Tick = co.SETTLEMENT_TICK
                        nmoc.LotSize = co.LOT_SIZE
                        If co.q.ORDER_TYPE = OrderTypes.RatioSpread Then nmoc.Tick = 0.001
                        nmoc.BuyRefPrice = nmoc.BuyOrder.PRICE_INDICATED
                        nmoc.SellRefPrice = nmoc.SellOrder.PRICE_INDICATED
                        nc.Add(nmoc)
                    Next
                End If
            Next
        Next

    End Sub

    Public Sub OrderHistory(ByRef s As String, _
                            ByVal BrokerId As Integer, _
                            ByVal BS As String, _
                            ByVal OrderType As Integer, _
                            ByVal RouteID As Integer, _
                            ByRef Period As ArtBTimePeriod, _
                            ByVal RouteID2 As Integer, _
                            ByRef Period2 As ArtBTimePeriod)
        s = ""
        Dim DN As DateTime = DateTime.UtcNow
        Dim D As DateTime
        D = DateAdd(DateInterval.Day, -4, DN)
        D.AddSeconds(-D.Second)
        D.AddMinutes(-D.Minute)
        D.AddHours(-D.Hour)

        Dim gdb As DB_ARTB_NETDataContext = GetNewConnection()

        Dim ol As Object
        Dim mm1 As Integer = 0
        Dim mm2 As Integer = 0
        Dim yy1 As Integer = 0
        Dim yy2 As Integer = 0
        Dim mm21 As Integer = 0
        Dim mm22 As Integer = 0
        Dim yy21 As Integer = 0
        Dim yy22 As Integer = 0

        If Not IsNothing(Period) Then
            mm1 = Period.MM1
            mm2 = Period.MM2
            yy1 = Period.YY1 + 2000
            yy2 = Period.YY2 + 2000
        End If

        If Not IsNothing(Period2) Then
            mm21 = Period2.MM1
            mm22 = Period2.MM2
            yy21 = Period2.YY1 + 2000
            yy22 = Period2.YY2 + 2000
        End If

        If OrderType = OrderTypes.FFA Then
            ol = (From q In gdb.ORDERS_FFAs _
                 Join dt In gdb.DESK_TRADERs On q.FOR_DESK_TRADER_ID Equals dt.DESK_TRADER_ID _
                 Join ac In gdb.ACCOUNTs On dt.ACCOUNT_ID Equals ac.ACCOUNT_ID _
                        Where ac.BROKER_ID = BrokerId And _
                              q.ORDER_BS = BS And _
                              q.ORDER_DATETIME >= D And _
                              q.ORDER_TYPE = OrderType And _
                              q.ROUTE_ID = RouteID And _
                              q.MM1 = mm1 And _
                              q.MM2 = mm2 And _
                              q.YY1 = yy1 And _
                              q.YY2 = yy2 And _
                              q.SPREAD_ORDER_ID Is Nothing And _
                              q.FOR_DESK_TRADER_ID <> SystemDeskTraderId And _
                              q.DESK_TRADER_ID <> SystemDeskTraderId _
                      Order By q.ORDER_DATETIME Descending _
                      Select q).ToList
        Else
            ol = (From q In gdb.ORDERS_FFAs _
                 Join dt In gdb.DESK_TRADERs On q.FOR_DESK_TRADER_ID Equals dt.DESK_TRADER_ID _
                 Join ac In gdb.ACCOUNTs On dt.ACCOUNT_ID Equals ac.ACCOUNT_ID _
                        Where ac.BROKER_ID = BrokerId And _
                              q.ORDER_BS = BS And _
                              q.ORDER_DATETIME >= D And _
                              q.ORDER_TYPE = OrderType And _
                              q.ROUTE_ID = RouteID And _
                              q.MM1 = mm1 And _
                              q.MM2 = mm2 And _
                              q.YY1 = yy1 And _
                              q.YY2 = yy2 And _
                              q.ROUTE_ID2 = RouteID2 And _
                              q.MM21 = mm21 And _
                              q.MM22 = mm22 And _
                              q.YY21 = yy21 And _
                              q.YY22 = yy22 And _
                              q.FOR_DESK_TRADER_ID <> SystemDeskTraderId And _
                              q.DESK_TRADER_ID <> SystemDeskTraderId _
                      Order By q.ORDER_DATETIME Descending _
                      Select q).ToList
        End If

        Dim qs As String = ""

        For Each q In ol
            If OrderDescr(q, 5, qs) <> ArtBErrors.Success Then Continue For
            s &= qs & vbNewLine
        Next

    End Sub


    Public Sub UpdateRefCol(ByRef gdb As DB_ARTB_NETDataContext, _
                            ByVal TCS As String)
        Dim tl As Object
        Dim bl As Object
        Dim vl As Object
        Dim d As Integer = DateTime.UtcNow.Day()
        Dim m As Integer = DateTime.UtcNow.Month()
        Dim y As Integer = DateTime.UtcNow.Year()

        If Not IsNothing(gdb) Then
            tl = From q In gdb.TRADES_FFAs _
                     Join r In gdb.ROUTEs On q.ROUTE_ID Equals r.ROUTE_ID _
                     Join v In gdb.VESSEL_CLASSes On r.VESSEL_CLASS_ID Equals v.VESSEL_CLASS_ID _
                     Where q.TRADE_TYPE = OrderTypes.FFA _
                         And q.IS_SYNTHETIC = False _
                         And v.DRYWET = TCS _
                         And q.ORDER_DATETIME.Day = d _
                         And q.ORDER_DATETIME.Month = m _
                         And q.ORDER_DATETIME.Year = y _
                 Order By q.ORDER_DATETIME _
                 Select q, r.PRICING_TICK

            bl = From q In gdb.BALTIC_FORWARD_RATES_VIEWs _
                     Join r In gdb.ROUTEs On q.ROUTE_ID Equals r.ROUTE_ID _
                     Join v In gdb.VESSEL_CLASSes On r.VESSEL_CLASS_ID Equals v.VESSEL_CLASS_ID _
                       Select q, r.PRICING_TICK

            vl = From q In gdb.VESSEL_CLASS_SPREAD_MARGINs _
                 Join r In gdb.ROUTEs On q.VESSEL_CLASS_ID Equals r.VESSEL_CLASS_ID _
                 Select q, r
        Else
            tl = From q In TRADES_FFAS _
                     Join r In ROUTES On q.ROUTE_ID Equals r.ROUTE_ID _
                     Join v In VESSEL_CLASSES On r.VESSEL_CLASS_ID Equals v.VESSEL_CLASS_ID _
                     Where q.TRADE_TYPE = OrderTypes.FFA _
                         And q.IS_SYNTHETIC = False _
                         And v.DRYWET = TCS _
                         And q.ORDER_DATETIME.Day = d _
                         And q.ORDER_DATETIME.Month = m _
                         And q.ORDER_DATETIME.Year = y _
                 Order By q.ORDER_DATETIME _
                 Select q, r.PRICING_TICK

            bl = From q In BALTIC_FORWARD_RATES _
                     Join r In ROUTES On q.ROUTE_ID Equals r.ROUTE_ID _
                     Join v In VESSEL_CLASSES On r.VESSEL_CLASS_ID Equals v.VESSEL_CLASS_ID _
                       Select q, r.PRICING_TICK

            vl = From q In VESSEL_CLASS_SPREAD_MARGINS _
                 Join r In ROUTES On q.VESSEL_CLASS_ID Equals r.VESSEL_CLASS_ID _
                 Select q, r

        End If

        For Each t In tl
            Dim RPStr As String = RoutePeriodStringFromObj(t.q)
            Dim RefC As REF_CLASS = GetViewClass(RefCol, RPStr)
            If IsNothing(RefC) Then
                RefC = New REF_CLASS(RPStr)
                RefCol.Add(RefC, RPStr)
                RefC.AssignTradePrice(t.q, t.PRICING_TICK)
            Else
                RefC.AssignTradePrice(t.q, t.PRICING_TICK)
            End If
        Next

        For Each b In bl
            Dim RPStr As String = RoutePeriodStringFromObj(b.q)
            Dim RefC As REF_CLASS = GetViewClass(RefCol, RPStr)
            If IsNothing(RefC) Then
                RefC = New REF_CLASS(RPStr)
                RefCol.Add(RefC, RPStr)
                RefC.AssignBalticPrice(b.q, b.PRICING_TICK)
            Else
                RefC.AssignBalticPrice(b.q, b.PRICING_TICK)
            End If

        Next

        For Each v In vl
            Dim RPStr As String = RoutePeriodStringFromObj(v.q, 1, v.r)
            Dim RefC As REF_CLASS = GetViewClass(RefCol, RPStr)
            If IsNothing(RefC) Then
                RefC = New REF_CLASS(RPStr)
                RefCol.Add(RefC, RPStr)
                RefC.Volatility = v.q.MARGIN
                RefC.SynthVolatility = False
            Else
                RefC.Volatility = v.q.MARGIN
                RefC.SynthVolatility = False
            End If
        Next
    End Sub

    Public Function Volatility(ByVal RPStr As String) As Double
        If IsNothing(RefCol) Then Return 0.02
        Dim RefC1 As REF_CLASS = Nothing
        If RefCol.Contains(RPStr) Then
            RefC1 = GetViewClass(RefCol, RPStr)
            If Not IsNothing(RefC1) Then
                If Math.Abs(RefC1.Volatility) < 1.0E+19 Then Return RefC1.Volatility
            End If
        End If
        Dim p As New ArtBTimePeriod
        p.GetFromRPStr(RPStr)
        Dim TotalMonths As Double = 0
        Dim TotalVolatility As Double = 0
        For Each RefC As REF_CLASS In RefCol
            If Math.Abs(RefC.Volatility) > 1.0E+19 Then Continue For
            If RefC.SynthVolatility Then Continue For
            If RefC.RPSTR = "" Then Continue For
            If RPStr.Substring(0, 5) <> RefC.RPSTR.Substring(0, 5) Then Continue For
            Dim t As New ArtBTimePeriod
            t.GetFromRPStr(RefC.RPSTR)
            Dim tm As Integer = p.OverlapMonths(t)
            If tm = 0 Then Continue For
            TotalMonths += tm
            TotalVolatility += RefC.Volatility
        Next
        Volatility = 0.02
        If TotalMonths <> 0 Then Volatility = TotalVolatility / TotalMonths
        If Not IsNothing(RefC1) Then
            RefC1.Volatility = Volatility
            Exit Function
        End If
        Dim RefC2 As New REF_CLASS(RPStr)
        RefCol.Add(RefC2, RPStr)
        RefC2.Volatility = Volatility
    End Function

    Public Function TradeFillBrokers(ByRef gdb As DB_ARTB_NETDataContext, ByRef Trade As TRADES_FFA_CLASS) As Integer
        TradeFillBrokers = ArtBErrors.Success
        Dim TCStr As String = RouteTradeClass(gdb, Trade.ROUTE_ID)

        Dim TraderID1 As Integer = NullInt2Int(Trade.DESK_TRADER_ID1)
        Dim TraderID2 As Integer = NullInt2Int(Trade.DESK_TRADER_ID2)

        If TraderID1 <> 0 And TCStr <> "" Then
            Try
                Dim q1 = (From t In gdb.DESK_TRADERs _
                          Join tc In gdb.DESK_TRADE_CLASSes On t.ACCOUNT_DESK_ID Equals tc.ACCOUNT_DESK_ID _
                          Where t.DESK_TRADER_ID = TraderID1 And tc.TRADE_CLASS_SHORT = TCStr _
                          Select tc.BROKER_ID).ToList.First()
                Trade.BROKER_ID1 = GetMainBrokerForAffiliate(gdb, TCStr, q1)
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try
        End If
        If TraderID2 <> 0 And TCStr <> "" Then
            Try
                Dim q2 = (From t In gdb.DESK_TRADERs _
                          Join tc In gdb.DESK_TRADE_CLASSes On t.ACCOUNT_DESK_ID Equals tc.ACCOUNT_DESK_ID _
                          Where t.DESK_TRADER_ID = TraderID2 And tc.TRADE_CLASS_SHORT = TCStr _
                          Select tc.BROKER_ID).ToList.First()
                Trade.BROKER_ID2 = GetMainBrokerForAffiliate(gdb, TCStr, q2)
            Catch ex As Exception
                Debug.Print(ex.ToString)
            End Try
        End If
    End Function

    Public Function RouteTradeClass(ByRef gdb As DB_ARTB_NETDataContext, ByRef a_RouteID As Object) As String
        RouteTradeClass = ""
        Try
            Dim RouteID As Integer = NullInt2Int(a_RouteID)
            If gdb Is Nothing Then
                Dim q = From r In ROUTES _
                         Join v In VESSEL_CLASSES On r.VESSEL_CLASS_ID Equals v.VESSEL_CLASS_ID _
                         Where r.ROUTE_ID = RouteID _
                         Select v.DRYWET
                For Each TCStr As String In q
                    Return TCStr
                Next
            Else
                Dim q = (From r In gdb.ROUTEs _
                         Join v In gdb.VESSEL_CLASSes On r.VESSEL_CLASS_ID Equals v.VESSEL_CLASS_ID _
                         Where r.ROUTE_ID = RouteID _
                         Select v.DRYWET).ToList
                For Each TCStr As String In q
                    Return TCStr
                Next
            End If
        Catch ex As Exception
            Debug.Print(ex.ToString)
        End Try
    End Function
End Class
