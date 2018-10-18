Public Class ORDERS_FFA_CLASS
    Inherits ORDERS_FFA

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            ORDER_ID = q.ORDER_ID
            ORDER_TYPE = q.ORDER_TYPE
            ORDER_DATETIME = q.ORDER_DATETIME
            DESK_TRADER_ID = q.DESK_TRADER_ID
            FOR_DESK_TRADER_ID = q.FOR_DESK_TRADER_ID
            ORDERED_BY_WHO = q.ORDERED_BY_WHO
            PREVIOUS_ORDER_ID = q.PREVIOUS_ORDER_ID
            LIVE_STATUS = q.LIVE_STATUS
            ORDER_QUALIFIER = q.ORDER_QUALIFIER
            ORDER_BS = q.ORDER_BS
            ROUTE_ID = q.ROUTE_ID
            MM1 = q.MM1
            YY1 = q.YY1
            MM2 = q.MM2
            YY2 = q.YY2
            SHORTDES = q.SHORTDES
            PRICE_INDICATED = q.PRICE_INDICATED
            PRICE_TYPE = q.PRICE_TYPE
            PRICE_TRY_BETTER = q.PRICE_TRY_BETTER
            ORDER_QUANTITY = q.ORDER_QUANTITY
            DAY_QUALIFIER = q.DAY_QUALIFIER
            FLEXIBLE_QUANTITY = q.FLEXIBLE_QUANTITY
            QUANTITY_STEP = q.QUANTITY_STEP
            ORDER_GOOD_TILL = q.ORDER_GOOD_TILL
            ORDER_TIME_LIMIT = q.ORDER_TIME_LIMIT
            SHOW_MY_NAME = q.SHOW_MY_NAME
            PNC_ORDER = q.PNC_ORDER
            SINGLE_EXCHANGE_EXECUTION = q.SINGLE_EXCHANGE_EXECUTION
            ORDER_TRADED_ON_EXCHANGE = q.ORDER_TRADED_ON_EXCHANGE
            CLEARER_ID = q.CLEARER_ID
            ORDER_EXCHANGES = q.ORDER_EXCHANGES
            THREAD = q.THREAD
            COUNTER_PARTY_ORDER_ID = q.COUNTER_PARTY_ORDER_ID
            LOCK_DESK_TRADER_ID = q.LOCK_DESK_TRADER_ID
            LOCK_ORDER_ID = q.LOCK_ORDER_ID
            COMMIT_ORDER_ID = q.COMMIT_ORDER_ID
            INFORM_DESK_ID = q.INFORM_DESK_ID
            NEGOTIATION_ORDER_ID = q.NEGOTIATION_ORDER_ID
            LOCKED_BY_ORDER_ID = q.LOCKED_BY_ORDER_ID
            SPREAD_LEG_TYPE = q.SPREAD_LEG_TYPE
            SPREAD_ORDER_ID = q.SPREAD_ORDER_ID
            CROSS_ORDER_ID1 = q.CROSS_ORDER_ID1
            CROSS_ORDER_ID2 = q.CROSS_ORDER_ID2
            ROUTE_ID2 = q.ROUTE_ID2
            MM21 = q.MM21
            YY21 = q.YY21
            MM22 = q.MM22
            YY22 = q.YY22
            SHORTDES2 = q.SHORTDES2
            PRICE_INDICATED2 = q.PRICE_INDICATED2
            PRICE_TYPE2 = q.PRICE_TYPE2
            PRICE_TRY_BETTER2 = q.PRICE_TRY_BETTER2
            ORDER_QUANTITY2 = q.ORDER_QUANTITY2
            DAY_QUALIFIER2 = q.DAY_QUALIFIER2
            FLEXIBLE_QUANTITY2 = q.FLEXIBLE_QUANTITY2
            ORDER_EXCHANGES2 = q.ORDER_EXCHANGES2
            STRIKE_PRICE = q.STRIKE_PRICE
            ICEBERG = q.ICEBERG
            BROKER_INVISIBLE = q.BROKER_INVISIBLE
        Catch e As Exception
            GetFromObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function SetToObject(ByRef q As Object, Optional ByVal bCopyTime As Boolean = True) As Integer
        If q Is Nothing Then
            SetToObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        SetToObject = ArtBErrors.Success
        Try
            q.ORDER_ID = ORDER_ID
            q.ORDER_TYPE = ORDER_TYPE
            If bCopyTime Then q.ORDER_DATETIME = ORDER_DATETIME
            q.DESK_TRADER_ID = DESK_TRADER_ID
            q.FOR_DESK_TRADER_ID = FOR_DESK_TRADER_ID
            q.ORDERED_BY_WHO = ORDERED_BY_WHO
            q.PREVIOUS_ORDER_ID = PREVIOUS_ORDER_ID
            q.LIVE_STATUS = LIVE_STATUS
            q.ORDER_QUALIFIER = ORDER_QUALIFIER
            q.ORDER_BS = ORDER_BS
            q.ROUTE_ID = ROUTE_ID
            q.MM1 = MM1
            q.YY1 = YY1
            q.MM2 = MM2
            q.YY2 = YY2
            q.SHORTDES = SHORTDES
            q.PRICE_INDICATED = PRICE_INDICATED
            q.PRICE_TYPE = PRICE_TYPE
            q.PRICE_TRY_BETTER = PRICE_TRY_BETTER
            q.ORDER_QUANTITY = ORDER_QUANTITY
            q.DAY_QUALIFIER = DAY_QUALIFIER
            q.FLEXIBLE_QUANTITY = FLEXIBLE_QUANTITY
            q.QUANTITY_STEP = QUANTITY_STEP
            q.ORDER_GOOD_TILL = ORDER_GOOD_TILL
            q.ORDER_TIME_LIMIT = ORDER_TIME_LIMIT
            q.SHOW_MY_NAME = SHOW_MY_NAME
            q.PNC_ORDER = PNC_ORDER
            q.SINGLE_EXCHANGE_EXECUTION = SINGLE_EXCHANGE_EXECUTION
            q.ORDER_TRADED_ON_EXCHANGE = ORDER_TRADED_ON_EXCHANGE
            q.CLEARER_ID = CLEARER_ID
            q.ORDER_EXCHANGES = ORDER_EXCHANGES
            q.THREAD = THREAD
            q.COUNTER_PARTY_ORDER_ID = COUNTER_PARTY_ORDER_ID
            q.LOCK_DESK_TRADER_ID = LOCK_DESK_TRADER_ID
            q.LOCK_ORDER_ID = LOCK_ORDER_ID
            q.COMMIT_ORDER_ID = COMMIT_ORDER_ID
            q.INFORM_DESK_ID = INFORM_DESK_ID
            q.NEGOTIATION_ORDER_ID = NEGOTIATION_ORDER_ID
            q.LOCKED_BY_ORDER_ID = LOCKED_BY_ORDER_ID
            q.SPREAD_LEG_TYPE = SPREAD_LEG_TYPE
            q.SPREAD_ORDER_ID = SPREAD_ORDER_ID
            q.CROSS_ORDER_ID1 = CROSS_ORDER_ID1
            q.CROSS_ORDER_ID2 = CROSS_ORDER_ID2
            q.ROUTE_ID2 = ROUTE_ID2
            q.MM21 = MM21
            q.YY21 = YY21
            q.MM22 = MM22
            q.YY22 = YY22
            q.SHORTDES2 = SHORTDES2
            q.PRICE_INDICATED2 = PRICE_INDICATED2
            q.PRICE_TYPE2 = PRICE_TYPE2
            q.PRICE_TRY_BETTER2 = PRICE_TRY_BETTER2
            q.ORDER_QUANTITY2 = ORDER_QUANTITY2
            q.DAY_QUALIFIER2 = DAY_QUALIFIER2
            q.FLEXIBLE_QUANTITY2 = FLEXIBLE_QUANTITY2
            q.ORDER_EXCHANGES2 = ORDER_EXCHANGES2
            q.STRIKE_PRICE = STRIKE_PRICE
            q.ICEBERG = ICEBERG
            q.BROKER_INVISIBLE = BROKER_INVISIBLE
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Try
            Dim l = From q In gdb.ORDERS_FFAs _
            Where q.ORDER_ID = ORDER_ID _
            Select q

            For Each q In l
                GetData = GetFromObject(q)
                Exit Function
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_ORDER_ID As Integer) As Integer
        ORDER_ID = a_ORDER_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Try
            Dim q As New ORDERS_FFA
            SetToObject(q)

            gdb.ORDERS_FFAs.InsertOnSubmit(q)

            If submit = True Then
                gdb.SubmitChanges()
                ORDER_ID = q.ORDER_ID
            End If
            Insert = ArtBErrors.Success
        Catch e As Exception
            Insert = ArtBErrors.InsertFailed
            Debug.Print(e.ToString())
            Exit Function
        End Try
    End Function

    Public Function Update(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Update = ArtBErrors.Success
        Try

            Dim l = From q In gdb.ORDERS_FFAs _
              Where q.ORDER_ID = ORDER_ID _
              Select q

            For Each q As ORDERS_FFA In l
                Update = SetToObject(q)
                If Update <> ArtBErrors.Success Then
                    Exit Function
                End If
            Next

            If submit = True Then
                gdb.SubmitChanges()
            End If
        Catch e As Exception
            Update = ArtBErrors.UpdateFailed
            Debug.Print(e.ToString())
            Exit Function
        End Try
    End Function

    Public Function InsertUpdate(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        InsertUpdate = ArtBErrors.Success

        Try
            If ORDER_ID <> 0 Then
                Dim l = From q In gdb.ORDERS_FFAs _
                  Where q.ORDER_ID = ORDER_ID _
                  Select q

                For Each q As ORDERS_FFA In l
                    InsertUpdate = SetToObject(q, False)
                    If InsertUpdate <> ArtBErrors.Success Then
                        Exit Function
                    End If
                    If submit = True Then
                        gdb.SubmitChanges()
                    End If
                    Exit Function
                Next
            End If

            ORDER_ID = 0
            InsertUpdate = Insert(gdb, submit)

            If submit = True Then
                gdb.SubmitChanges()
            End If
        Catch e As Exception
            Debug.Print(e.ToString())
            InsertUpdate = ArtBErrors.UpdateFailed
            Exit Function
        End Try
    End Function

    Public Function Delete(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Delete = ArtBErrors.Success
        Try

            Dim l = From q In gdb.ORDERS_FFAs _
             Where q.ORDER_ID = ORDER_ID _
              Select q

            For Each q As ORDERS_FFA In l
                gdb.ORDERS_FFAs.DeleteOnSubmit(q)
            Next

            If submit = True Then
                gdb.SubmitChanges()
            End If
        Catch e As Exception
            Debug.Print(e.ToString())
            Delete = ArtBErrors.DeleteFailed
            Exit Function
        End Try
    End Function

    Public Function AppendToStr(ByRef DestinationStr As String) As Integer
        Dim s As String = ""
        Try
            s = s & Int2Str(ORDER_ID) & FIELD_SEPARATOR_STR
            s = s & Short2Str(ORDER_TYPE) & FIELD_SEPARATOR_STR
            s = s & DateTime2Str(ORDER_DATETIME) & FIELD_SEPARATOR_STR
            s = s & Int2Str(DESK_TRADER_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(FOR_DESK_TRADER_ID) & FIELD_SEPARATOR_STR
            s = s & ORDERED_BY_WHO & FIELD_SEPARATOR_STR
            s = s & Int2Str(PREVIOUS_ORDER_ID) & FIELD_SEPARATOR_STR
            s = s & LIVE_STATUS & FIELD_SEPARATOR_STR
            s = s & ORDER_QUALIFIER & FIELD_SEPARATOR_STR
            s = s & ORDER_BS & FIELD_SEPARATOR_STR
            s = s & Int2Str(ROUTE_ID) & FIELD_SEPARATOR_STR
            s = s & Short2Str(MM1) & FIELD_SEPARATOR_STR
            s = s & Short2Str(YY1) & FIELD_SEPARATOR_STR
            s = s & Short2Str(MM2) & FIELD_SEPARATOR_STR
            s = s & Short2Str(YY2) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(SHORTDES) & FIELD_SEPARATOR_STR
            s = s & Double2Str(PRICE_INDICATED) & FIELD_SEPARATOR_STR
            s = s & PRICE_TYPE & FIELD_SEPARATOR_STR
            s = s & Bool2Str(PRICE_TRY_BETTER) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ORDER_QUANTITY) & FIELD_SEPARATOR_STR
            s = s & Byte2Str(DAY_QUALIFIER) & FIELD_SEPARATOR_STR
            s = s & Short2Str(FLEXIBLE_QUANTITY) & FIELD_SEPARATOR_STR
            s = s & Int2Str(QUANTITY_STEP) & FIELD_SEPARATOR_STR
            s = s & Short2Str(ORDER_GOOD_TILL) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ORDER_TIME_LIMIT) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(SHOW_MY_NAME) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(PNC_ORDER) & FIELD_SEPARATOR_STR
            s = s & Short2Str(SINGLE_EXCHANGE_EXECUTION) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(ORDER_TRADED_ON_EXCHANGE) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(CLEARER_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(ORDER_EXCHANGES) & FIELD_SEPARATOR_STR
            s = s & Int2Str(THREAD) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(COUNTER_PARTY_ORDER_ID) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(LOCK_DESK_TRADER_ID) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(LOCK_ORDER_ID) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(COMMIT_ORDER_ID) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(INFORM_DESK_ID) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(NEGOTIATION_ORDER_ID) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(LOCKED_BY_ORDER_ID) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(SPREAD_LEG_TYPE) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(SPREAD_ORDER_ID) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(CROSS_ORDER_ID1) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(CROSS_ORDER_ID2) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(ROUTE_ID2) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(MM21) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(YY21) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(MM22) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(YY22) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(SHORTDES2) & FIELD_SEPARATOR_STR
            s = s & NullDouble2Str(PRICE_INDICATED2) & FIELD_SEPARATOR_STR
            s = s & PRICE_TYPE2 & FIELD_SEPARATOR_STR
            s = s & NullBool2Str(PRICE_TRY_BETTER2) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(ORDER_QUANTITY2) & FIELD_SEPARATOR_STR
            s = s & NullByte2Str(DAY_QUALIFIER2) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(FLEXIBLE_QUANTITY2) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(ORDER_EXCHANGES2) & FIELD_SEPARATOR_STR
            s = s & NullDouble2Str(STRIKE_PRICE) & FIELD_SEPARATOR_STR
            s = s & NullBool2Str(ICEBERG) & FIELD_SEPARATOR_STR
            s = s & NullBool2Str(BROKER_INVISIBLE) & RECORD_SEPARATOR_STR
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionToStrError
        End Try
        DestinationStr = DestinationStr & s
        AppendToStr = ArtBErrors.Success
    End Function

    Public Function GetFromStr(ByRef SourceStr As String) As Integer
        Dim ls As New List(Of String)
        GetFromStr = ParseRecordString(SourceStr, ls)
        If GetFromStr <> ArtBErrors.Success Then Exit Function
        If ls.Count() <> 59 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            ORDER_ID = Str2Int(ls(0))
            ORDER_TYPE = Str2Short(ls(1))
            ORDER_DATETIME = Str2DateTime(ls(2))
            DESK_TRADER_ID = Str2Int(ls(3))
            FOR_DESK_TRADER_ID = Str2Int(ls(4))
            ORDERED_BY_WHO = Str2Char(ls(5))
            PREVIOUS_ORDER_ID = Str2Int(ls(6))
            LIVE_STATUS = Str2Char(ls(7))
            ORDER_QUALIFIER = Str2Char(ls(8))
            ORDER_BS = Str2Char(ls(9))
            ROUTE_ID = Str2Int(ls(10))
            MM1 = Str2Short(ls(11))
            YY1 = Str2Short(ls(12))
            MM2 = Str2Short(ls(13))
            YY2 = Str2Short(ls(14))
            SHORTDES = ls(15)
            PRICE_INDICATED = Str2Double(ls(16))
            PRICE_TYPE = Str2Char(ls(17))
            PRICE_TRY_BETTER = Str2Bool(ls(18))
            ORDER_QUANTITY = Str2Int(ls(19))
            DAY_QUALIFIER = Str2Byte(ls(20))
            FLEXIBLE_QUANTITY = Str2Short(ls(21))
            QUANTITY_STEP = Str2Int(ls(22))
            ORDER_GOOD_TILL = Str2Short(ls(23))
            ORDER_TIME_LIMIT = Str2Int(ls(24))
            SHOW_MY_NAME = Str2Bool(ls(25))
            PNC_ORDER = Str2Bool(ls(26))
            SINGLE_EXCHANGE_EXECUTION = Str2Short(ls(27))
            ORDER_TRADED_ON_EXCHANGE = Str2NullInt(ls(28))
            CLEARER_ID = Str2NullInt(ls(29))
            ORDER_EXCHANGES = ls(30)
            THREAD = Str2Int(ls(31))
            COUNTER_PARTY_ORDER_ID = Str2NullInt(ls(32))
            LOCK_DESK_TRADER_ID = Str2NullInt(ls(33))
            LOCK_ORDER_ID = Str2NullInt(ls(34))
            COMMIT_ORDER_ID = Str2NullInt(ls(35))
            INFORM_DESK_ID = Str2NullInt(ls(36))
            NEGOTIATION_ORDER_ID = Str2NullInt(ls(37))
            LOCKED_BY_ORDER_ID = Str2NullInt(ls(38))
            SPREAD_LEG_TYPE = Str2NullShort(ls(39))
            SPREAD_ORDER_ID = Str2NullInt(ls(40))
            CROSS_ORDER_ID1 = Str2NullInt(ls(41))
            CROSS_ORDER_ID2 = Str2NullInt(ls(42))
            ROUTE_ID2 = Str2NullInt(ls(43))
            MM21 = Str2NullShort(ls(44))
            YY21 = Str2NullShort(ls(45))
            MM22 = Str2NullShort(ls(46))
            YY22 = Str2NullShort(ls(47))
            SHORTDES2 = ls(48)
            PRICE_INDICATED2 = Str2NullDouble(ls(49))
            PRICE_TYPE2 = Str2NullChar(ls(50))
            PRICE_TRY_BETTER2 = Str2NullBool(ls(51))
            ORDER_QUANTITY2 = Str2NullInt(ls(52))
            DAY_QUALIFIER2 = Str2NullByte(ls(53))
            FLEXIBLE_QUANTITY2 = Str2NullShort(ls(54))
            ORDER_EXCHANGES2 = ls(55)
            STRIKE_PRICE = Str2NullDouble(ls(56))
            ICEBERG = Str2NullBool(ls(57))
            BROKER_INVISIBLE = Str2NullBool(ls(58))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As Object, Optional ByVal bCheckIdDate As Boolean = True) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If bCheckIdDate And ORDER_ID <> q.ORDER_ID Then Exit Function
        If ORDER_TYPE <> q.ORDER_TYPE Then Exit Function
        If bCheckIdDate And ORDER_DATETIME <> q.ORDER_DATETIME Then Exit Function
        If DESK_TRADER_ID <> q.DESK_TRADER_ID Then Exit Function
        If FOR_DESK_TRADER_ID <> q.FOR_DESK_TRADER_ID Then Exit Function
        If ORDERED_BY_WHO <> q.ORDERED_BY_WHO Then Exit Function
        If PREVIOUS_ORDER_ID <> q.PREVIOUS_ORDER_ID Then Exit Function
        If LIVE_STATUS <> q.LIVE_STATUS Then Exit Function
        If ORDER_QUALIFIER <> q.ORDER_QUALIFIER Then Exit Function
        If ORDER_BS <> q.ORDER_BS Then Exit Function
        If ROUTE_ID <> q.ROUTE_ID Then Exit Function
        If MM1 <> q.MM1 Then Exit Function
        If YY1 <> q.YY1 Then Exit Function
        If MM2 <> q.MM2 Then Exit Function
        If YY2 <> q.YY2 Then Exit Function
        If SHORTDES <> q.SHORTDES Then Exit Function
        If PRICE_INDICATED <> q.PRICE_INDICATED Then Exit Function
        If PRICE_TYPE <> q.PRICE_TYPE Then Exit Function
        If PRICE_TRY_BETTER <> q.PRICE_TRY_BETTER Then Exit Function
        If ORDER_QUANTITY <> q.ORDER_QUANTITY Then Exit Function
        If DAY_QUALIFIER <> q.DAY_QUALIFIER Then Exit Function
        If FLEXIBLE_QUANTITY <> q.FLEXIBLE_QUANTITY Then Exit Function
        If QUANTITY_STEP <> q.QUANTITY_STEP Then Exit Function
        If ORDER_GOOD_TILL <> q.ORDER_GOOD_TILL Then Exit Function
        If ORDER_TIME_LIMIT <> q.ORDER_TIME_LIMIT Then Exit Function
        If SHOW_MY_NAME <> q.SHOW_MY_NAME Then Exit Function
        If PNC_ORDER <> q.PNC_ORDER Then Exit Function
        If SINGLE_EXCHANGE_EXECUTION <> q.SINGLE_EXCHANGE_EXECUTION Then Exit Function
        If ORDER_TRADED_ON_EXCHANGE <> q.ORDER_TRADED_ON_EXCHANGE Then Exit Function
        If CLEARER_ID <> q.CLEARER_ID Then Exit Function
        If ORDER_EXCHANGES <> q.ORDER_EXCHANGES Then Exit Function
        If THREAD <> q.THREAD Then Exit Function
        If COUNTER_PARTY_ORDER_ID <> q.COUNTER_PARTY_ORDER_ID Then Exit Function
        If LOCK_DESK_TRADER_ID <> q.LOCK_DESK_TRADER_ID Then Exit Function
        If LOCK_ORDER_ID <> q.LOCK_ORDER_ID Then Exit Function
        If COMMIT_ORDER_ID <> q.COMMIT_ORDER_ID Then Exit Function
        If INFORM_DESK_ID <> q.INFORM_DESK_ID Then Exit Function
        If NEGOTIATION_ORDER_ID <> q.NEGOTIATION_ORDER_ID Then Exit Function
        If LOCKED_BY_ORDER_ID <> q.LOCKED_BY_ORDER_ID Then Exit Function
        If SPREAD_LEG_TYPE <> q.SPREAD_LEG_TYPE Then Exit Function
        If SPREAD_ORDER_ID <> q.SPREAD_ORDER_ID Then Exit Function
        If CROSS_ORDER_ID1 <> q.CROSS_ORDER_ID1 Then Exit Function
        If CROSS_ORDER_ID2 <> q.CROSS_ORDER_ID2 Then Exit Function
        If ROUTE_ID2 <> q.ROUTE_ID2 Then Exit Function
        If MM21 <> q.MM21 Then Exit Function
        If YY21 <> q.YY21 Then Exit Function
        If MM22 <> q.MM22 Then Exit Function
        If YY22 <> q.YY22 Then Exit Function
        If SHORTDES2 <> q.SHORTDES2 Then Exit Function
        If PRICE_INDICATED2 <> q.PRICE_INDICATED2 Then Exit Function
        If PRICE_TYPE2 <> q.PRICE_TYPE2 Then Exit Function
        If PRICE_TRY_BETTER2 <> q.PRICE_TRY_BETTER2 Then Exit Function
        If ORDER_QUANTITY2 <> q.ORDER_QUANTITY2 Then Exit Function
        If DAY_QUALIFIER2 <> q.DAY_QUALIFIER2 Then Exit Function
        If FLEXIBLE_QUANTITY2 <> q.FLEXIBLE_QUANTITY2 Then Exit Function
        If ORDER_EXCHANGES2 <> q.ORDER_EXCHANGES2 Then Exit Function
        If STRIKE_PRICE <> q.STRIKE_PRICE Then Exit Function
        If ICEBERG <> q.ICEBERG Then Exit Function
        If BROKER_INVISIBLE <> q.BROKER_INVISIBLE Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As ORDERS_FFA_CLASS
        GetNewCopy = New ORDERS_FFA_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.ORDER_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------
    Public EXCHANGES As New Collection

    Public Function SetLiveStatus(ByRef gdb As DB_ARTB_NETDataContext, _
                                  ByVal c As Char) As Integer
        SetLiveStatus = ArtBErrors.Success
        If LIVE_STATUS <> "A" Then Exit Function
        LIVE_STATUS = c
        SetLiveStatus = Update(gdb, True)
    End Function

    Public Function DeleteExchanges(ByRef gdb As DB_ARTB_NETDataContext, _
                                    Optional ByVal submit As Boolean = False) As Integer
        DeleteExchanges = ArtBErrors.Success
        If 0 = ORDER_ID Then Exit Function
        Try
            Dim l = From q In gdb.ORDERS_FFA_EXCHANGEs _
             Where q.ORDER_ID = ORDER_ID _
              Select q

            For Each q As ORDERS_FFA_EXCHANGE In l
                gdb.ORDERS_FFA_EXCHANGEs.DeleteOnSubmit(q)
            Next

            If submit = True Then
                gdb.SubmitChanges()
            End If
        Catch e As Exception
            DeleteExchanges = ArtBErrors.DeleteFailed
            Exit Function
        End Try

    End Function

    Public Function GetSpreadLeg(Optional ByVal a_Leg As Integer = 1, Optional ByVal a_bProjecredLeg As Boolean = False) As ORDERS_FFA_CLASS
        GetSpreadLeg = Nothing
        If ORDER_TYPE = OrderTypes.FFA Then Return Nothing
        GetSpreadLeg = GetNewCopy()
        GetSpreadLeg.ORDER_TYPE = OrderTypes.FFA
        GetSpreadLeg.SPREAD_ORDER_ID = ORDER_ID
        GetSpreadLeg.COMMIT_ORDER_ID = Nothing
        GetSpreadLeg.CROSS_ORDER_ID1 = Nothing
        GetSpreadLeg.CROSS_ORDER_ID2 = Nothing
        GetSpreadLeg.ROUTE_ID2 = Nothing
        GetSpreadLeg.MM21 = Nothing
        GetSpreadLeg.YY21 = Nothing
        GetSpreadLeg.MM22 = Nothing
        GetSpreadLeg.YY22 = Nothing
        GetSpreadLeg.SHORTDES2 = Nothing
        GetSpreadLeg.PRICE_INDICATED2 = Nothing
        GetSpreadLeg.PRICE_TYPE2 = Nothing
        GetSpreadLeg.PRICE_TRY_BETTER2 = Nothing
        GetSpreadLeg.ORDER_QUANTITY2 = Nothing
        GetSpreadLeg.DAY_QUALIFIER2 = Nothing
        GetSpreadLeg.FLEXIBLE_QUANTITY2 = Nothing
        GetSpreadLeg.ORDER_EXCHANGES2 = Nothing

        If a_bProjecredLeg And (LIVE_STATUS = "A" Or LIVE_STATUS = "N") Then GetSpreadLeg.LIVE_STATUS = "P"
        If a_Leg = 1 Then
            GetSpreadLeg.SPREAD_LEG_TYPE = SpreadLegTypes.Order1
            If a_bProjecredLeg Then GetSpreadLeg.SPREAD_LEG_TYPE = SpreadLegTypes.ProjectedOrder1
            If ORDER_QUALIFIER = "N" Then
                GetSpreadLeg.COUNTER_PARTY_ORDER_ID = COUNTER_PARTY_ORDER_ID + 1
                GetSpreadLeg.NEGOTIATION_ORDER_ID = Nothing
            End If
        Else
            If ORDER_QUALIFIER = "N" Then
                GetSpreadLeg.COUNTER_PARTY_ORDER_ID = COUNTER_PARTY_ORDER_ID + 2
                GetSpreadLeg.NEGOTIATION_ORDER_ID = Nothing
            End If
            GetSpreadLeg.SPREAD_LEG_TYPE = SpreadLegTypes.Order2
            If a_bProjecredLeg Then GetSpreadLeg.SPREAD_LEG_TYPE = SpreadLegTypes.ProjectedOrder2
            If ORDER_BS = "B" Then
                GetSpreadLeg.ORDER_BS = "S"
            Else
                GetSpreadLeg.ORDER_BS = "B"
            End If
            GetSpreadLeg.ROUTE_ID = ROUTE_ID2
            GetSpreadLeg.MM1 = MM21
            GetSpreadLeg.YY1 = YY21
            GetSpreadLeg.MM2 = MM22
            GetSpreadLeg.YY2 = YY22
            GetSpreadLeg.SHORTDES = SHORTDES2
            GetSpreadLeg.PRICE_INDICATED = NullDouble2Double(PRICE_INDICATED2)
            GetSpreadLeg.PRICE_TYPE = "F"
            GetSpreadLeg.PRICE_TRY_BETTER = False
            Try
                GetSpreadLeg.ORDER_QUANTITY = ORDER_QUANTITY2
                GetSpreadLeg.DAY_QUALIFIER = DAY_QUALIFIER2
                GetSpreadLeg.ORDER_EXCHANGES = ORDER_EXCHANGES2
            Catch ex As Exception
                Debug.Print(ex.ToString())
            End Try
            Dim q1 As Double = GetActualQuantity(1)
            If FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket And q1 <> 0 Then
                Dim QRatio As Double = GetActualQuantity(2) / q1
                GetSpreadLeg.QUANTITY_STEP = QUANTITY_STEP * QRatio
            End If
            'If ORDER_GOOD_TILL = OrderGoodTill.Limit Then
            '    GetSpreadLeg.ORDER_GOOD_TILL = OrderGoodTill.Day
            'End If
        End If
        If ORDER_TYPE = OrderTypes.RatioSpread Or ORDER_TYPE = OrderTypes.PriceSpread Or ORDER_TYPE = OrderTypes.CalendarSpread Then
            If GetSpreadLeg.ORDER_BS = "B" Then
                GetSpreadLeg.PRICE_INDICATED = 1.0E+20
            Else
                GetSpreadLeg.PRICE_INDICATED = -1.0E+20
            End If
        End If
    End Function

    Public Function GetActualQuantity(Optional ByVal a_Leg As Integer = 1) As Integer
        Dim AQ As Double
        Dim tp As ArtBTimePeriod
        If a_Leg = 1 Then
            tp = GetOrderTimePeriod(1)
            AQ = ORDER_QUANTITY
            Select Case DAY_QUALIFIER
                Case OrderDayQualifier.Full
                    AQ = tp.DPM
                Case OrderDayQualifier.Half
                    AQ = tp.DPM * 0.5
                Case OrderDayQualifier.ContractDays
                    AQ = ORDER_QUANTITY / (tp.EndMonth - tp.StartMonth + 1)
            End Select
        Else
            tp = GetOrderTimePeriod(2)
            AQ = ORDER_QUANTITY2
            Select Case DAY_QUALIFIER2
                Case OrderDayQualifier.Full
                    AQ = tp.DPM
                Case OrderDayQualifier.Half
                    AQ = tp.DPM * 0.5
                Case OrderDayQualifier.ContractDays
                    AQ = ORDER_QUANTITY2 / (tp.EndMonth - tp.StartMonth + 1)
            End Select
        End If
        GetActualQuantity = Int(AQ)
        tp = Nothing
    End Function

    Public Function GetOrderTimePeriod(Optional ByVal a_Leg As Integer = 1) As ArtBTimePeriod
        GetOrderTimePeriod = New ArtBTimePeriod

        If a_Leg = 1 Then
            GetOrderTimePeriod.FillMY(MM1, YY1, MM2, YY2)
        Else
            GetOrderTimePeriod.FillMY(MM21, YY21, MM22, YY22)
        End If
        GetOrderTimePeriod.FillDates()
        GetOrderTimePeriod.FillDPM()
    End Function

    Public Sub SetQuantity(ByVal a_NewQuantinty As Double)
        Dim q1 As Integer = GetActualQuantity(1)
        ORDER_QUANTITY = a_NewQuantinty
        If q1 <> 0 And _
         (ORDER_TYPE <> OrderTypes.FFA) Then
            Dim q2 As Integer = GetActualQuantity(2)
            ORDER_QUANTITY2 = a_NewQuantinty * q2 / q1
        End If
        If DAY_QUALIFIER = OrderDayQualifier.NotInDays Then Exit Sub

        ORDER_QUANTITY = Int(ORDER_QUANTITY)

        Select Case ORDER_QUANTITY
            Case 15
                DAY_QUALIFIER = OrderDayQualifier.Half
                ORDER_QUANTITY = 0
            Case 30
                DAY_QUALIFIER = OrderDayQualifier.Full
                ORDER_QUANTITY = 0
            Case Else
                DAY_QUALIFIER = OrderDayQualifier.DPM
        End Select

        If (ORDER_TYPE <> OrderTypes.FFA) Then
            ORDER_QUANTITY2 = Int(ORDER_QUANTITY2)
            Select Case ORDER_QUANTITY2
                Case 15
                    DAY_QUALIFIER2 = OrderDayQualifier.Half
                    ORDER_QUANTITY2 = 0
                Case 30
                    DAY_QUALIFIER2 = OrderDayQualifier.Full
                    ORDER_QUANTITY2 = 0
                Case Else
                    DAY_QUALIFIER2 = OrderDayQualifier.DPM
            End Select
        End If

    End Sub

    Public Sub SetAsCancelOrder(ByRef CounterOrder As ORDERS_FFA_CLASS)
        Dim q11 As Integer = GetActualQuantity(1)
        Dim q21 As Integer = CounterOrder.GetActualQuantity(1)
        If FLEXIBLE_QUANTITY <> OrderFlexQuantinty.Bucket Or q21 >= q11 Or THREAD = CounterOrder.THREAD Then
            LIVE_STATUS = "D"
            If CounterOrder.PREVIOUS_ORDER_ID = 0 Then CounterOrder.PREVIOUS_ORDER_ID = ORDER_ID
            Exit Sub
        End If
        Dim fq As Integer = q11 - q21
        SetQuantity(fq)
        ORDER_QUALIFIER = " "
    End Sub

    Public Function MatchQuantity(ByRef Order As ORDERS_FFA_CLASS) As Boolean
        If IsNothing(Order) Then Return False
        If ORDER_TYPE <> Order.ORDER_TYPE Then Return False
        If FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then Return True
        If Order.FLEXIBLE_QUANTITY = OrderFlexQuantinty.Bucket Then Return True
        Dim aq11 As Integer = GetActualQuantity(1)
        Dim aq21 As Integer = Order.GetActualQuantity(1)
        If aq11 = aq21 Then
            If ORDER_TYPE = OrderTypes.FFA Then
                Return True
            End If
            Dim aq12 As Integer = GetActualQuantity(2)
            Dim aq22 As Integer = Order.GetActualQuantity(2)
            If aq12 = aq22 Then Return True
        End If
        Return False
    End Function

    Public Function GetRatioSpreadPrecisionFromDB(ByRef gdb As DB_ARTB_NETDataContext) As Double
        GetRatioSpreadPrecisionFromDB = 0
        If ORDER_TYPE <> OrderTypes.RatioSpread Then Return 0
        Dim l = From q In gdb.TRADE_CLASS_RATIO_SPREADs _
                Where q.ROUTE_ID1 = ROUTE_ID And q.ROUTE_ID2 = ROUTE_ID2 _
                Select q
        For Each q In l
            GetRatioSpreadPrecisionFromDB = q.PRECISION_TICK
        Next
    End Function

    Public Sub TestOrder(ByRef GVC As GlobalViewClass, _
                         Optional ByVal a_OrderType As Integer = OrderTypes.FFA, _
                           Optional ByVal TraderName As String = "ATLA", _
                           Optional ByVal a_BS As String = "B", _
                           Optional ByVal a_Route As Integer = 36, _
                           Optional ByVal a_Cal As Integer = 0, _
                           Optional ByVal a_Qty As Double = 0, _
                           Optional ByVal a_QtyType As Integer = OrderFlexQuantinty.Fixed, _
                           Optional ByVal a_Price As Double = 35000, _
                           Optional ByVal a_Route2 As Integer = 0, _
                           Optional ByVal a_Cal2 As Integer = 0, _
                           Optional ByVal a_Qty2 As Double = 0, _
                           Optional ByVal a_SXX As Boolean = False, _
                           Optional ByVal a_XGS As Integer = 2)
        ORDER_ID = 0
        ORDER_TYPE = a_OrderType
        THREAD = CInt(Int(1000000000 * Rnd() + 1000000000))
        ORDER_DATETIME = DateTime.UtcNow
        DESK_TRADER_ID = 28
        Dim ForTraderId As Integer = 86

        FOR_DESK_TRADER_ID = GVC.GetDefaultTraderID(TraderName)
        ORDERED_BY_WHO = "U"
        PREVIOUS_ORDER_ID = 0
        LIVE_STATUS = "A"

        ORDER_QUALIFIER = " "
        ORDER_BS = a_BS
        ROUTE_ID = a_Route
        Dim OrderTimePeriod As New ArtBTimePeriod
        Dim d As DateTime = DateTime.UtcNow
        OrderTimePeriod.StartMonth = (d.Year Mod 2000) * 12 + (d.Month - d.Month Mod 3) + a_Cal * 3
        OrderTimePeriod.EndMonth = OrderTimePeriod.StartMonth + 2
        OrderTimePeriod.FillDates()
        OrderTimePeriod.CreateDescr()

        Dim m As Integer = d.Month
        m = m - (m Mod 3) + 1
        Dim y As Integer = d.Year Mod 2000
        MM1 = OrderTimePeriod.MM1
        YY1 = OrderTimePeriod.YY1 + 2000
        MM2 = OrderTimePeriod.MM2
        YY2 = OrderTimePeriod.YY2 + 2000
        SHORTDES = OrderTimePeriod.Descr
        PRICE_INDICATED = a_Price
        PRICE_TYPE = "F"
        PRICE_TRY_BETTER = False
        ORDER_QUANTITY = a_Qty
        DAY_QUALIFIER = 3
        FLEXIBLE_QUANTITY = a_QtyType
        ORDER_GOOD_TILL = OrderGoodTill.Day
        ORDER_TIME_LIMIT = 0
        PNC_ORDER = False
        ORDER_TRADED_ON_EXCHANGE = 0
        SINGLE_EXCHANGE_EXECUTION = a_SXX
        Dim s As String = "_4_____"
        Select Case a_XGS
            Case 4
                s = "__5____"
            Case 6
                s = "_4_5____"
        End Select
        ORDER_EXCHANGES = s

        If ORDER_TYPE <> OrderTypes.FFA Then
            If a_Route2 = 0 Then a_Route2 = a_Route
            ROUTE_ID2 = a_Route2
            OrderTimePeriod.StartMonth = (d.Year Mod 2000) * 12 + (d.Month - d.Month Mod 3) + a_Cal2 * 3
            OrderTimePeriod.EndMonth = OrderTimePeriod.StartMonth + 2
            OrderTimePeriod.FillDates()
            OrderTimePeriod.CreateDescr()
            OrderTimePeriod.CreateDescr()
            MM21 = OrderTimePeriod.MM1
            YY21 = OrderTimePeriod.YY1 + 2000
            MM22 = OrderTimePeriod.MM2
            YY22 = OrderTimePeriod.YY2 + 2000
            SHORTDES2 = OrderTimePeriod.Descr
            DAY_QUALIFIER2 = 3
            If a_Qty2 = 0 Then a_Qty2 = a_Qty
            ORDER_QUANTITY2 = a_Qty2
        End If
        ORDER_EXCHANGES2 = s

        OrderTimePeriod = Nothing
    End Sub

    Public Function SameTypeRoutePeriod(ByRef o As ORDERS_FFA_CLASS) As Boolean
        If IsNothing(o) Then Return False
        If ORDER_TYPE <> o.ORDER_TYPE Then Return False
        If ROUTE_ID <> o.ROUTE_ID Then Return False
        If MM1 <> o.MM1 Then Return False
        If YY1 <> o.YY1 Then Return False
        If MM2 <> o.MM2 Then Return False
        If YY2 <> o.YY2 Then Return False
        If ORDER_TYPE = OrderTypes.FFA Then Return True

        If ROUTE_ID2 <> o.ROUTE_ID2 Then Return False
        If MM21 <> o.MM21 Then Return False
        If YY21 <> o.YY21 Then Return False
        If MM22 <> o.MM22 Then Return False
        If YY22 <> o.YY22 Then Return False

        Dim q1 As Integer = GetActualQuantity(1)
        Dim q2 As Integer = GetActualQuantity(2)
        Dim q21 As Integer = o.GetActualQuantity(1)
        Dim q22 As Integer = o.GetActualQuantity(2)
        If q2 <> 0 And q22 <> 0 Then
            If q1 / q2 <> q21 / q22 Then Return False
        End If
        Return True
    End Function

    Public Sub FicticiouOrder(ByRef GVC As GlobalViewClass, _
                              ByVal RPStr As String, _
                              ByVal Price As Double, _
                              ByVal bPNC As Boolean)
        ORDER_ID = 0
        THREAD = CInt(Int(1000000000 * Rnd() + 1000000000))
        Dim OrderType As Integer
        Dim RouteID As Integer
        Dim fMM1 As Integer
        Dim fYY1 As Integer
        Dim fMM2 As Integer
        Dim fYY2 As Integer
        Dim RouteID2 As Integer = 0
        Dim fMM21 As Integer = 0
        Dim fYY21 As Integer = 0
        Dim fMM22 As Integer = 0
        Dim fYY22 As Integer = 0
        If GVC.ParseRPString(RPStr, OrderType, RouteID, fMM1, fYY1, fMM2, fYY2, RouteID2, fMM21, fYY21, fMM22, fYY22) <> ArtBErrors.Success Then Exit Sub

        Dim OrderTimePeriod As New ArtBTimePeriod
        OrderTimePeriod.FillMY(fMM1, fYY1, fMM2, fYY2)
        OrderTimePeriod.CreateDescr()

        ORDERED_BY_WHO = "S"
        LIVE_STATUS = "A"
        SINGLE_EXCHANGE_EXECUTION = False
        ORDER_TYPE = OrderType
        ORDER_GOOD_TILL = OrderGoodTill.Day
        ORDER_QUALIFIER = " "
        ORDER_EXCHANGES = "10"
        For i As Integer = 1 To GVC.EXCHANGES.Count
            ORDER_EXCHANGES &= "_"
        Next

        PRICE_TYPE = "F"
        PRICE_TRY_BETTER = False
        FLEXIBLE_QUANTITY = OrderFlexQuantinty.Fixed
        QUANTITY_STEP = 1
        ORDER_DATETIME = DateTime.UtcNow
        PRICE_INDICATED = Price
        ROUTE_ID = RouteID
        MM1 = OrderTimePeriod.MM1
        MM2 = OrderTimePeriod.MM2
        YY1 = OrderTimePeriod.YY1 + 2000
        YY2 = OrderTimePeriod.YY2 + 2000
        ORDER_BS = "B"
        ORDER_QUANTITY = 1
        DAY_QUALIFIER = 0
        SHORTDES = OrderTimePeriod.Descr
        DESK_TRADER_ID = SystemDeskTraderId
        FOR_DESK_TRADER_ID = SystemDeskTraderId
        PNC_ORDER = bPNC
        If OrderType <> OrderTypes.FFA Then
            ROUTE_ID2 = RouteID2
            Dim OrderTimePeriod2 As New ArtBTimePeriod
            OrderTimePeriod2.FillMY(fMM21, fYY21, fMM22, fYY22)
            OrderTimePeriod2.CreateDescr()
            MM21 = OrderTimePeriod2.MM1
            MM22 = OrderTimePeriod2.MM2
            YY21 = OrderTimePeriod2.YY1 + 2000
            YY22 = OrderTimePeriod2.YY2 + 2000
            SHORTDES2 = OrderTimePeriod2.Descr
            ORDER_QUANTITY2 = 1
            DAY_QUALIFIER2 = 0
            FLEXIBLE_QUANTITY2 = OrderFlexQuantinty.Fixed
            OrderTimePeriod2 = Nothing
            ORDER_EXCHANGES2 = ORDER_EXCHANGES
        End If
        OrderTimePeriod = Nothing
    End Sub

End Class