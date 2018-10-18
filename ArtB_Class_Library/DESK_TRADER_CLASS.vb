Public Class DESK_TRADER_CLASS
    Inherits DESK_TRADER

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            DESK_TRADER_ID = q.DESK_TRADER_ID
            ACCOUNT_DESK_ID = q.ACCOUNT_DESK_ID
            ACCOUNT_ID = q.ACCOUNT_ID
            CONTACT_ID = q.CONTACT_ID
            AUTHORISED = q.AUTHORISED
            AUTHORISATION_RECEIVED = q.AUTHORISATION_RECEIVED
            RECEIVED_WHEN = q.RECEIVED_WHEN
            EXPIRES_WHEN = q.EXPIRES_WHEN
            TRADE_AUTHORITY = q.TRADE_AUTHORITY
            USERNAME = q.USERNAME
            PASSWORD = q.PASSWORD
            EXPIRED = q.EXPIRED
            OF_ID = q.OF_ID
            OF_PASSWORD = q.OF_PASSWORD
            IS_DESK_ADMIN = q.IS_DESK_ADMIN
            SUSPENDED = q.SUSPENDED
            TOOLBAR_SHOW = q.TOOLBAR_SHOW
            DEFAULT_MARKET = q.DEFAULT_MARKET
            DEFAULT_SHOW_NAMES = q.DEFAULT_SHOW_NAMES
            GRID_MARKET_DEPTH = q.GRID_MARKET_DEPTH
            BID_COLOR = q.BID_COLOR
            OFFER_COLOR = q.OFFER_COLOR
            FONT_TYPE = q.FONT_TYPE
            CHANGE_PSW = q.CHANGE_PSW
            AGREES_TO_STATEMENT = q.AGREES_TO_STATEMENT
            INDICATIVES_VISIBLE = q.INDICATIVES_VISIBLE
            ONE_CLICK_HIT = q.ONE_CLICK_HIT
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
            q.DESK_TRADER_ID = DESK_TRADER_ID
            q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID
            q.ACCOUNT_ID = ACCOUNT_ID
            q.CONTACT_ID = CONTACT_ID
            q.AUTHORISED = AUTHORISED
            q.AUTHORISATION_RECEIVED = AUTHORISATION_RECEIVED
            q.RECEIVED_WHEN = RECEIVED_WHEN
            q.EXPIRES_WHEN = EXPIRES_WHEN
            q.TRADE_AUTHORITY = TRADE_AUTHORITY
            q.USERNAME = USERNAME
            q.PASSWORD = PASSWORD
            q.EXPIRED = EXPIRED
            q.OF_ID = OF_ID
            q.OF_PASSWORD = OF_PASSWORD
            q.IS_DESK_ADMIN = IS_DESK_ADMIN
            q.SUSPENDED = SUSPENDED
            q.TOOLBAR_SHOW = TOOLBAR_SHOW
            q.DEFAULT_MARKET = DEFAULT_MARKET
            q.DEFAULT_SHOW_NAMES = DEFAULT_SHOW_NAMES
            q.GRID_MARKET_DEPTH = GRID_MARKET_DEPTH
            q.BID_COLOR = BID_COLOR
            q.OFFER_COLOR = OFFER_COLOR
            q.FONT_TYPE = FONT_TYPE
            q.CHANGE_PSW = CHANGE_PSW
            q.AGREES_TO_STATEMENT = AGREES_TO_STATEMENT
            q.INDICATIVES_VISIBLE = INDICATIVES_VISIBLE
            q.ONE_CLICK_HIT = ONE_CLICK_HIT
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Try
            Dim l = From q In gdb.DESK_TRADERs _
            Where q.DESK_TRADER_ID = DESK_TRADER_ID _
            Select q

            For Each q In l
                GetData = GetFromObject(q)
                Exit Function
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_DESK_TRADER_ID As Integer) As Integer
        DESK_TRADER_ID = a_DESK_TRADER_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Try
            Dim q As New DESK_TRADER
            SetToObject(q)

            gdb.DESK_TRADERs.InsertOnSubmit(q)

            If submit = True Then
                gdb.SubmitChanges()
                DESK_TRADER_ID = q.DESK_TRADER_ID
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

            Dim l = From q In gdb.DESK_TRADERs _
              Where q.DESK_TRADER_ID = DESK_TRADER_ID _
              Select q

            For Each q As DESK_TRADER In l
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
            Dim l = From q In gdb.DESK_TRADERs _
              Where q.DESK_TRADER_ID = DESK_TRADER_ID _
              Select q

            For Each q As DESK_TRADER In l
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

            Dim l = From q In gdb.DESK_TRADERs _
             Where q.DESK_TRADER_ID = DESK_TRADER_ID _
              Select q

            For Each q As DESK_TRADER In l
                gdb.DESK_TRADERs.DeleteOnSubmit(q)
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
            s = s & Int2Str(DESK_TRADER_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ACCOUNT_DESK_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ACCOUNT_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(CONTACT_ID) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(AUTHORISED) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(AUTHORISATION_RECEIVED) & FIELD_SEPARATOR_STR
            s = s & NullDateTime2Str(RECEIVED_WHEN) & FIELD_SEPARATOR_STR
            s = s & NullDateTime2Str(EXPIRES_WHEN) & FIELD_SEPARATOR_STR
            s = s & Short2Str(TRADE_AUTHORITY) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(USERNAME) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(PASSWORD) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(EXPIRED) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(OF_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(OF_PASSWORD) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(IS_DESK_ADMIN) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(SUSPENDED) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(TOOLBAR_SHOW) & FIELD_SEPARATOR_STR
            s = s & DEFAULT_MARKET & FIELD_SEPARATOR_STR
            s = s & Bool2Str(DEFAULT_SHOW_NAMES) & FIELD_SEPARATOR_STR
            s = s & Int2Str(GRID_MARKET_DEPTH) & FIELD_SEPARATOR_STR
            s = s & Int2Str(BID_COLOR) & FIELD_SEPARATOR_STR
            s = s & Int2Str(OFFER_COLOR) & FIELD_SEPARATOR_STR
            s = s & Short2Str(FONT_TYPE) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(CHANGE_PSW) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(AGREES_TO_STATEMENT) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(INDICATIVES_VISIBLE) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(ONE_CLICK_HIT) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 27 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            DESK_TRADER_ID = Str2Int(ls(0))
            ACCOUNT_DESK_ID = Str2Int(ls(1))
            ACCOUNT_ID = Str2Int(ls(2))
            CONTACT_ID = Str2Int(ls(3))
            AUTHORISED = Str2Bool(ls(4))
            AUTHORISATION_RECEIVED = Str2Bool(ls(5))
            RECEIVED_WHEN = Str2NullDateTime(ls(6))
            EXPIRES_WHEN = Str2NullDateTime(ls(7))
            TRADE_AUTHORITY = Str2Short(ls(8))
            USERNAME = ls(9)
            PASSWORD = ls(10)
            EXPIRED = Str2Bool(ls(11))
            OF_ID = ls(12)
            OF_PASSWORD = ls(13)
            IS_DESK_ADMIN = Str2Bool(ls(14))
            SUSPENDED = Str2Bool(ls(15))
            TOOLBAR_SHOW = Str2Bool(ls(16))
            DEFAULT_MARKET = Str2Char(ls(17))
            DEFAULT_SHOW_NAMES = Str2Bool(ls(18))
            GRID_MARKET_DEPTH = Str2Int(ls(19))
            BID_COLOR = Str2Int(ls(20))
            OFFER_COLOR = Str2Int(ls(21))
            FONT_TYPE = Str2Short(ls(22))
            CHANGE_PSW = Str2Bool(ls(23))
            AGREES_TO_STATEMENT = Str2Bool(ls(24))
            INDICATIVES_VISIBLE = Str2Bool(ls(25))
            ONE_CLICK_HIT = Str2Bool(ls(26))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As DESK_TRADER_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If DESK_TRADER_ID <> q.DESK_TRADER_ID Then Exit Function
        If ACCOUNT_DESK_ID <> q.ACCOUNT_DESK_ID Then Exit Function
        If ACCOUNT_ID <> q.ACCOUNT_ID Then Exit Function
        If CONTACT_ID <> q.CONTACT_ID Then Exit Function
        If AUTHORISED <> q.AUTHORISED Then Exit Function
        If AUTHORISATION_RECEIVED <> q.AUTHORISATION_RECEIVED Then Exit Function
        If RECEIVED_WHEN <> q.RECEIVED_WHEN Then Exit Function
        If EXPIRES_WHEN <> q.EXPIRES_WHEN Then Exit Function
        If TRADE_AUTHORITY <> q.TRADE_AUTHORITY Then Exit Function
        If USERNAME <> q.USERNAME Then Exit Function
        If PASSWORD <> q.PASSWORD Then Exit Function
        If EXPIRED <> q.EXPIRED Then Exit Function
        If OF_ID <> q.OF_ID Then Exit Function
        If OF_PASSWORD <> q.OF_PASSWORD Then Exit Function
        If IS_DESK_ADMIN <> q.IS_DESK_ADMIN Then Exit Function
        If SUSPENDED <> q.SUSPENDED Then Exit Function
        If TOOLBAR_SHOW <> q.TOOLBAR_SHOW Then Exit Function
        If DEFAULT_MARKET <> q.DEFAULT_MARKET Then Exit Function
        If DEFAULT_SHOW_NAMES <> q.DEFAULT_SHOW_NAMES Then Exit Function
        If GRID_MARKET_DEPTH <> q.GRID_MARKET_DEPTH Then Exit Function
        If BID_COLOR <> q.BID_COLOR Then Exit Function
        If OFFER_COLOR <> q.OFFER_COLOR Then Exit Function
        If FONT_TYPE <> q.FONT_TYPE Then Exit Function
        If CHANGE_PSW <> q.CHANGE_PSW Then Exit Function
        If AGREES_TO_STATEMENT <> q.AGREES_TO_STATEMENT Then Exit Function
        If INDICATIVES_VISIBLE <> q.INDICATIVES_VISIBLE Then Exit Function
        If ONE_CLICK_HIT <> q.ONE_CLICK_HIT Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As DESK_TRADER_CLASS
        GetNewCopy = New DESK_TRADER_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.DESK_TRADER_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------
    Public CONTACT_DETAILS As New CONTACT_CLASS
    Public LoggedIn As Boolean = False

    Public Function GetContactName() As String
        If CONTACT_DETAILS Is Nothing Then
            GetContactName = USERNAME
            Exit Function
        End If
        GetContactName = CONTACT_DETAILS.LASTNAME & " " & CONTACT_DETAILS.FIRSTNAME
    End Function

End Class

