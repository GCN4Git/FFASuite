Public Class TRADES_FFA_CLASS
    Inherits TRADES_FFA

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            TRADE_ID = q.TRADE_ID
            ORDER_DATETIME = q.ORDER_DATETIME
            TRADE_BOOKED_DATETIME = q.TRADE_BOOKED_DATETIME
            TRADE_TYPE = q.TRADE_TYPE
            ORDER_QUALIFIER = q.ORDER_QUALIFIER
            ORDER_ID1 = q.ORDER_ID1
            ORDER_ID2 = q.ORDER_ID2
            TRADE_BS1 = q.TRADE_BS1
            TRADE_BS2 = q.TRADE_BS2
            DESK_TRADER_ID1 = q.DESK_TRADER_ID1
            DESK_TRADER_ID2 = q.DESK_TRADER_ID2
            ROUTE_ID = q.ROUTE_ID
            MM1 = q.MM1
            YY1 = q.YY1
            MM2 = q.MM2
            YY2 = q.YY2
            SHORTDES = q.SHORTDES
            PRICE_TRADED = q.PRICE_TRADED
            AMMENDED_PRICE = q.AMMENDED_PRICE
            UPDATE_STATUS = q.UPDATE_STATUS
            DAY_QUALIFIER = q.DAY_QUALIFIER
            QUANTITY = q.QUANTITY
            ROUTE_ID2 = q.ROUTE_ID2
            MM21 = q.MM21
            YY21 = q.YY21
            MM22 = q.MM22
            YY22 = q.YY22
            PRICE_TRADED2 = q.PRICE_TRADED2
            DAY_QUALIFIER2 = q.DAY_QUALIFIER2
            QUANTITY2 = q.QUANTITY2
            EXCHANGE_ID = q.EXCHANGE_ID
            DEAL_CONFIRMATION_SENT = q.DEAL_CONFIRMATION_SENT
            SENT_TO_CLEARING = q.SENT_TO_CLEARING
            CLEARING_ACCEPTED = q.CLEARING_ACCEPTED
            DEAL_CONFIRMATION_SENT2 = q.DEAL_CONFIRMATION_SENT2
            SENT_TO_CLEARING2 = q.SENT_TO_CLEARING2
            CLEARING_ACCEPTED2 = q.CLEARING_ACCEPTED2
            CLEARING_ID1 = q.CLEARING_ID1
            CLEARING_ID2 = q.CLEARING_ID2
            PNC = q.PNC
            INFORM_DESK_ID1 = q.INFORM_DESK_ID1
            INFORM_DESK_ID2 = q.INFORM_DESK_ID2
            IS_SYNTHETIC = q.IS_SYNTHETIC
            SPREAD_TRADE_ID1 = q.SPREAD_TRADE_ID1
            BROKER_ID1 = q.BROKER_ID1
            BROKER_ID2 = q.BROKER_ID2
            SPREAD_TRADE_ID2 = q.SPREAD_TRADE_ID2
        Catch e As Exception
            GetFromObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function SetToObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            SetToObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        SetToObject = ArtBErrors.Success
        Try
            q.TRADE_ID = TRADE_ID
            q.ORDER_DATETIME = ORDER_DATETIME
            q.TRADE_BOOKED_DATETIME = TRADE_BOOKED_DATETIME
            q.TRADE_TYPE = TRADE_TYPE
            q.ORDER_QUALIFIER = ORDER_QUALIFIER
            q.ORDER_ID1 = ORDER_ID1
            q.ORDER_ID2 = ORDER_ID2
            q.TRADE_BS1 = TRADE_BS1
            q.TRADE_BS2 = TRADE_BS2
            q.DESK_TRADER_ID1 = DESK_TRADER_ID1
            q.DESK_TRADER_ID2 = DESK_TRADER_ID2
            q.ROUTE_ID = ROUTE_ID
            q.MM1 = MM1
            q.YY1 = YY1
            q.MM2 = MM2
            q.YY2 = YY2
            q.SHORTDES = SHORTDES
            q.PRICE_TRADED = PRICE_TRADED
            q.AMMENDED_PRICE = AMMENDED_PRICE
            q.UPDATE_STATUS = UPDATE_STATUS
            q.DAY_QUALIFIER = DAY_QUALIFIER
            q.QUANTITY = QUANTITY
            q.ROUTE_ID2 = ROUTE_ID2
            q.MM21 = MM21
            q.YY21 = YY21
            q.MM22 = MM22
            q.YY22 = YY22
            q.PRICE_TRADED2 = PRICE_TRADED2
            q.DAY_QUALIFIER2 = DAY_QUALIFIER2
            q.QUANTITY2 = QUANTITY2
            q.EXCHANGE_ID = EXCHANGE_ID
            q.DEAL_CONFIRMATION_SENT = DEAL_CONFIRMATION_SENT
            q.SENT_TO_CLEARING = SENT_TO_CLEARING
            q.CLEARING_ACCEPTED = CLEARING_ACCEPTED
            q.DEAL_CONFIRMATION_SENT2 = DEAL_CONFIRMATION_SENT2
            q.SENT_TO_CLEARING2 = SENT_TO_CLEARING2
            q.CLEARING_ACCEPTED2 = CLEARING_ACCEPTED2
            q.CLEARING_ID1 = CLEARING_ID1
            q.CLEARING_ID2 = CLEARING_ID2
            q.PNC = PNC
            q.INFORM_DESK_ID1 = INFORM_DESK_ID1
            q.INFORM_DESK_ID2 = INFORM_DESK_ID2
            q.IS_SYNTHETIC = IS_SYNTHETIC
            q.SPREAD_TRADE_ID1 = SPREAD_TRADE_ID1
            q.BROKER_ID1 = BROKER_ID1
            q.BROKER_ID2 = BROKER_ID2
            q.SPREAD_TRADE_ID2 = SPREAD_TRADE_ID2
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Try
            Dim l = From q In gdb.TRADES_FFAs _
            Where q.TRADE_ID = TRADE_ID _
            Select q

            For Each q In l
                GetData = GetFromObject(q)
                Exit Function
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_TRADE_ID As Integer) As Integer
        TRADE_ID = a_TRADE_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Try
            Dim q As New TRADES_FFA
            SetToObject(q)

            gdb.TRADES_FFAs.InsertOnSubmit(q)

            If submit = True Then
                gdb.SubmitChanges()
                TRADE_ID = q.TRADE_ID
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

            Dim l = From q In gdb.TRADES_FFAs _
              Where q.TRADE_ID = TRADE_ID _
              Select q

            For Each q As TRADES_FFA In l
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
            Dim l = From q In gdb.TRADES_FFAs _
              Where q.TRADE_ID = TRADE_ID _
              Select q

            For Each q As TRADES_FFA In l
                InsertUpdate = SetToObject(q)
                If InsertUpdate <> ArtBErrors.Success Then
                    Exit Function
                End If
                If submit = True Then
                    gdb.SubmitChanges()
                End If
                Exit Function
            Next

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

            Dim l = From q In gdb.TRADES_FFAs _
             Where q.TRADE_ID = TRADE_ID _
              Select q

            For Each q As TRADES_FFA In l
                gdb.TRADES_FFAs.DeleteOnSubmit(q)
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
            s = s & Int2Str(TRADE_ID) & FIELD_SEPARATOR_STR
            s = s & DateTime2Str(ORDER_DATETIME) & FIELD_SEPARATOR_STR
            s = s & NullDateTime2Str(TRADE_BOOKED_DATETIME) & FIELD_SEPARATOR_STR
            s = s & Short2Str(TRADE_TYPE) & FIELD_SEPARATOR_STR
            s = s & ORDER_QUALIFIER & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(ORDER_ID1) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(ORDER_ID2) & FIELD_SEPARATOR_STR
            s = s & TRADE_BS1 & FIELD_SEPARATOR_STR
            s = s & TRADE_BS2 & FIELD_SEPARATOR_STR
            s = s & Int2Str(DESK_TRADER_ID1) & FIELD_SEPARATOR_STR
            s = s & Int2Str(DESK_TRADER_ID2) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ROUTE_ID) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(MM1) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(YY1) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(MM2) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(YY2) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(SHORTDES) & FIELD_SEPARATOR_STR
            s = s & Double2Str(PRICE_TRADED) & FIELD_SEPARATOR_STR
            s = s & Double2Str(AMMENDED_PRICE) & FIELD_SEPARATOR_STR
            s = s & Byte2Str(UPDATE_STATUS) & FIELD_SEPARATOR_STR
            s = s & Byte2Str(DAY_QUALIFIER) & FIELD_SEPARATOR_STR
            s = s & Int2Str(QUANTITY) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(ROUTE_ID2) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(MM21) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(YY21) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(MM22) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(YY22) & FIELD_SEPARATOR_STR
            s = s & NullDouble2Str(PRICE_TRADED2) & FIELD_SEPARATOR_STR
            s = s & NullByte2Str(DAY_QUALIFIER2) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(QUANTITY2) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(EXCHANGE_ID) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(DEAL_CONFIRMATION_SENT) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(SENT_TO_CLEARING) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(CLEARING_ACCEPTED) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(DEAL_CONFIRMATION_SENT2) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(SENT_TO_CLEARING2) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(CLEARING_ACCEPTED2) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(CLEARING_ID1) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(CLEARING_ID2) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(PNC) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(INFORM_DESK_ID1) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(INFORM_DESK_ID2) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(IS_SYNTHETIC) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(SPREAD_TRADE_ID1) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(BROKER_ID1) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(BROKER_ID2) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(SPREAD_TRADE_ID2) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 47 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            TRADE_ID = Str2Int(ls(0))
            ORDER_DATETIME = Str2DateTime(ls(1))
            TRADE_BOOKED_DATETIME = Str2NullDateTime(ls(2))
            TRADE_TYPE = Str2Short(ls(3))
            ORDER_QUALIFIER = Str2NullChar(ls(4))
            ORDER_ID1 = Str2NullInt(ls(5))
            ORDER_ID2 = Str2NullInt(ls(6))
            TRADE_BS1 = Str2Char(ls(7))
            TRADE_BS2 = Str2Char(ls(8))
            DESK_TRADER_ID1 = Str2Int(ls(9))
            DESK_TRADER_ID2 = Str2Int(ls(10))
            ROUTE_ID = Str2Int(ls(11))
            MM1 = Str2NullShort(ls(12))
            YY1 = Str2NullShort(ls(13))
            MM2 = Str2NullShort(ls(14))
            YY2 = Str2NullShort(ls(15))
            SHORTDES = ls(16)
            PRICE_TRADED = Str2Double(ls(17))
            AMMENDED_PRICE = Str2Double(ls(18))
            UPDATE_STATUS = Str2Byte(ls(19))
            DAY_QUALIFIER = Str2Byte(ls(20))
            QUANTITY = Str2Int(ls(21))
            ROUTE_ID2 = Str2NullInt(ls(22))
            MM21 = Str2NullShort(ls(23))
            YY21 = Str2NullShort(ls(24))
            MM22 = Str2NullShort(ls(25))
            YY22 = Str2NullShort(ls(26))
            PRICE_TRADED2 = Str2NullDouble(ls(27))
            DAY_QUALIFIER2 = Str2NullByte(ls(28))
            QUANTITY2 = Str2NullInt(ls(29))
            EXCHANGE_ID = Str2NullInt(ls(30))
            DEAL_CONFIRMATION_SENT = Str2Bool(ls(31))
            SENT_TO_CLEARING = Str2Bool(ls(32))
            CLEARING_ACCEPTED = Str2Bool(ls(33))
            DEAL_CONFIRMATION_SENT2 = Str2Bool(ls(34))
            SENT_TO_CLEARING2 = Str2Bool(ls(35))
            CLEARING_ACCEPTED2 = Str2Bool(ls(36))
            CLEARING_ID1 = Str2NullInt(ls(37))
            CLEARING_ID2 = Str2NullInt(ls(38))
            PNC = Str2Bool(ls(39))
            INFORM_DESK_ID1 = Str2NullInt(ls(40))
            INFORM_DESK_ID2 = Str2NullInt(ls(41))
            IS_SYNTHETIC = Str2Bool(ls(42))
            SPREAD_TRADE_ID1 = Str2NullInt(ls(43))
            BROKER_ID1 = Str2NullInt(ls(44))
            BROKER_ID2 = Str2NullInt(ls(45))
            SPREAD_TRADE_ID2 = Str2NullInt(ls(46))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As TRADES_FFA_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If TRADE_ID <> q.TRADE_ID Then Exit Function
        If ORDER_DATETIME <> q.ORDER_DATETIME Then Exit Function
        If TRADE_BOOKED_DATETIME <> q.TRADE_BOOKED_DATETIME Then Exit Function
        If TRADE_TYPE <> q.TRADE_TYPE Then Exit Function
        If ORDER_QUALIFIER <> q.ORDER_QUALIFIER Then Exit Function
        If ORDER_ID1 <> q.ORDER_ID1 Then Exit Function
        If ORDER_ID2 <> q.ORDER_ID2 Then Exit Function
        If TRADE_BS1 <> q.TRADE_BS1 Then Exit Function
        If TRADE_BS2 <> q.TRADE_BS2 Then Exit Function
        If DESK_TRADER_ID1 <> q.DESK_TRADER_ID1 Then Exit Function
        If DESK_TRADER_ID2 <> q.DESK_TRADER_ID2 Then Exit Function
        If ROUTE_ID <> q.ROUTE_ID Then Exit Function
        If MM1 <> q.MM1 Then Exit Function
        If YY1 <> q.YY1 Then Exit Function
        If MM2 <> q.MM2 Then Exit Function
        If YY2 <> q.YY2 Then Exit Function
        If SHORTDES <> q.SHORTDES Then Exit Function
        If PRICE_TRADED <> q.PRICE_TRADED Then Exit Function
        If AMMENDED_PRICE <> q.AMMENDED_PRICE Then Exit Function
        If UPDATE_STATUS <> q.UPDATE_STATUS Then Exit Function
        If DAY_QUALIFIER <> q.DAY_QUALIFIER Then Exit Function
        If QUANTITY <> q.QUANTITY Then Exit Function
        If ROUTE_ID2 <> q.ROUTE_ID2 Then Exit Function
        If MM21 <> q.MM21 Then Exit Function
        If YY21 <> q.YY21 Then Exit Function
        If MM22 <> q.MM22 Then Exit Function
        If YY22 <> q.YY22 Then Exit Function
        If PRICE_TRADED2 <> q.PRICE_TRADED2 Then Exit Function
        If DAY_QUALIFIER2 <> q.DAY_QUALIFIER2 Then Exit Function
        If QUANTITY2 <> q.QUANTITY2 Then Exit Function
        If EXCHANGE_ID <> q.EXCHANGE_ID Then Exit Function
        If DEAL_CONFIRMATION_SENT <> q.DEAL_CONFIRMATION_SENT Then Exit Function
        If SENT_TO_CLEARING <> q.SENT_TO_CLEARING Then Exit Function
        If CLEARING_ACCEPTED <> q.CLEARING_ACCEPTED Then Exit Function
        If DEAL_CONFIRMATION_SENT2 <> q.DEAL_CONFIRMATION_SENT2 Then Exit Function
        If SENT_TO_CLEARING2 <> q.SENT_TO_CLEARING2 Then Exit Function
        If CLEARING_ACCEPTED2 <> q.CLEARING_ACCEPTED2 Then Exit Function
        If CLEARING_ID1 <> q.CLEARING_ID1 Then Exit Function
        If CLEARING_ID2 <> q.CLEARING_ID2 Then Exit Function
        If PNC <> q.PNC Then Exit Function
        If INFORM_DESK_ID1 <> q.INFORM_DESK_ID1 Then Exit Function
        If INFORM_DESK_ID2 <> q.INFORM_DESK_ID2 Then Exit Function
        If IS_SYNTHETIC <> q.IS_SYNTHETIC Then Exit Function
        If SPREAD_TRADE_ID1 <> q.SPREAD_TRADE_ID1 Then Exit Function
        If BROKER_ID1 <> q.BROKER_ID1 Then Exit Function
        If BROKER_ID2 <> q.BROKER_ID2 Then Exit Function
        If SPREAD_TRADE_ID2 <> q.SPREAD_TRADE_ID2 Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As TRADES_FFA_CLASS
        GetNewCopy = New TRADES_FFA_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.TRADE_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------
    Public Function UpdateConfSent(ByVal gdb As DB_ARTB_NETDataContext, ByVal a_side As Integer) As Integer
        UpdateConfSent = GetData(gdb)
        If UpdateConfSent <> ArtBErrors.Success Then
            Return UpdateConfSent
        End If

        If a_side = 1 Then
            DEAL_CONFIRMATION_SENT = True
        Else
            DEAL_CONFIRMATION_SENT2 = True
        End If

        Return Update(gdb, True)
    End Function
End Class