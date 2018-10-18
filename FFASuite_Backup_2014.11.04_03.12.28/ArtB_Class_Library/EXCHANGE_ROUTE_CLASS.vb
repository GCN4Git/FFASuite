Public Class EXCHANGE_ROUTE_CLASS
    Inherits EXCHANGE_ROUTE

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            EXCHANGE_ROUTES_ID = q.EXCHANGE_ROUTES_ID
            EXCHANGE_ID = q.EXCHANGE_ID
            VESSEL_CLASS_ID = q.VESSEL_CLASS_ID
            ROUTE_ID = q.ROUTE_ID
            ACTIVE = q.ACTIVE
            EXCHANGE_ABRV = q.EXCHANGE_ABRV
            TRADING_START = q.TRADING_START
            TRADING_END = q.TRADING_END
            TRADE_REGISTRATION_START = q.TRADE_REGISTRATION_START
            TRADE_REGISTRATION_END = q.TRADE_REGISTRATION_END
            TRADE_REGISTRATION_LAST = q.TRADE_REGISTRATION_LAST
            OPTIONS_AVAILABLE = q.OPTIONS_AVAILABLE
            EXCHANGE_ROUTE_PERIOD_ID = q.EXCHANGE_ROUTE_PERIOD_ID
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
            q.EXCHANGE_ROUTES_ID = EXCHANGE_ROUTES_ID
            q.EXCHANGE_ID = EXCHANGE_ID
            q.VESSEL_CLASS_ID = VESSEL_CLASS_ID
            q.ROUTE_ID = ROUTE_ID
            q.ACTIVE = ACTIVE
            q.EXCHANGE_ABRV = EXCHANGE_ABRV
            q.TRADING_START = TRADING_START
            q.TRADING_END = TRADING_END
            q.TRADE_REGISTRATION_START = TRADE_REGISTRATION_START
            q.TRADE_REGISTRATION_END = TRADE_REGISTRATION_END
            q.TRADE_REGISTRATION_LAST = TRADE_REGISTRATION_LAST
            q.OPTIONS_AVAILABLE = OPTIONS_AVAILABLE
            q.EXCHANGE_ROUTE_PERIOD_ID = EXCHANGE_ROUTE_PERIOD_ID
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.EXCHANGE_ROUTEs _
        Where q.EXCHANGE_ROUTES_ID = EXCHANGE_ROUTES_ID And q.EXCHANGE_ID = EXCHANGE_ID And q.VESSEL_CLASS_ID = VESSEL_CLASS_ID And q.ROUTE_ID = ROUTE_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_EXCHANGE_ID As Integer, ByVal a_VESSEL_CLASS_ID As Integer, ByVal a_ROUTE_ID As Integer) As Integer
        EXCHANGE_ID = a_EXCHANGE_ID
        VESSEL_CLASS_ID = a_VESSEL_CLASS_ID
        ROUTE_ID = a_ROUTE_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New EXCHANGE_ROUTE
        SetToObject(q)

        gdb.EXCHANGE_ROUTEs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                EXCHANGE_ROUTES_ID = q.EXCHANGE_ROUTES_ID
            Catch e As Exception
                Insert = ArtBErrors.InsertFailed
                Debug.Print(e.ToString())
                Exit Function
            End Try
        End If
        Insert = ArtBErrors.Success
    End Function

    Public Function Update(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Update = ArtBErrors.Success

        Dim l = From q In gdb.EXCHANGE_ROUTEs _
          Where q.EXCHANGE_ID = EXCHANGE_ID And q.VESSEL_CLASS_ID = VESSEL_CLASS_ID And q.ROUTE_ID = ROUTE_ID _
          Select q

        For Each q As EXCHANGE_ROUTE In l
            Update = SetToObject(q)
            If Update <> ArtBErrors.Success Then
                Exit Function
            End If
        Next

        If submit = True Then
            Try
                gdb.SubmitChanges()
            Catch e As Exception
                Update = ArtBErrors.UpdateFailed
                Debug.Print(e.ToString())
                Exit Function
            End Try
        End If
    End Function

    Public Function InsertUpdate(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        InsertUpdate = ArtBErrors.Success

        Dim l = From q In gdb.EXCHANGE_ROUTEs _
          Where q.EXCHANGE_ID = EXCHANGE_ID And q.VESSEL_CLASS_ID = VESSEL_CLASS_ID And q.ROUTE_ID = ROUTE_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As EXCHANGE_ROUTE In l
            InsertUpdate = SetToObject(q)
            If InsertUpdate <> ArtBErrors.Success Then
                Exit Function
            End If
        Next

        If submit = True Then
            Try
                gdb.SubmitChanges()
            Catch e As Exception
                Debug.Print(e.ToString())
                InsertUpdate = ArtBErrors.UpdateFailed
                Exit Function
            End Try
        End If
    End Function

    Public Function Delete(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Delete = ArtBErrors.Success

        Dim l = From q In gdb.EXCHANGE_ROUTEs _
         Where q.EXCHANGE_ID = EXCHANGE_ID And q.VESSEL_CLASS_ID = VESSEL_CLASS_ID And q.ROUTE_ID = ROUTE_ID _
          Select q

        For Each q As EXCHANGE_ROUTE In l
            gdb.EXCHANGE_ROUTEs.DeleteOnSubmit(q)
        Next

        If submit = True Then
            Try
                gdb.SubmitChanges()
            Catch e As Exception
                Debug.Print(e.ToString())
                Delete = ArtBErrors.DeleteFailed
                Exit Function
            End Try
        End If
    End Function

    Public Function AppendToStr(ByRef DestinationStr As String) As Integer
        Dim s As String = ""
        Try
            s = s & Int2Str(EXCHANGE_ROUTES_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(EXCHANGE_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(VESSEL_CLASS_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ROUTE_ID) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(ACTIVE) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(EXCHANGE_ABRV) & FIELD_SEPARATOR_STR
            s = s & NullTime2Str(TRADING_START) & FIELD_SEPARATOR_STR
            s = s & NullTime2Str(TRADING_END) & FIELD_SEPARATOR_STR
            s = s & NullTime2Str(TRADE_REGISTRATION_START) & FIELD_SEPARATOR_STR
            s = s & NullTime2Str(TRADE_REGISTRATION_END) & FIELD_SEPARATOR_STR
            s = s & NullTime2Str(TRADE_REGISTRATION_LAST) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(OPTIONS_AVAILABLE) & FIELD_SEPARATOR_STR
            s = s & Int2Str(EXCHANGE_ROUTE_PERIOD_ID) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 13 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            EXCHANGE_ROUTES_ID = Str2Int(ls(0))
            EXCHANGE_ID = Str2Int(ls(1))
            VESSEL_CLASS_ID = Str2Int(ls(2))
            ROUTE_ID = Str2Int(ls(3))
            ACTIVE = Str2Bool(ls(4))
            EXCHANGE_ABRV = ls(5)
            TRADING_START = Str2NullTime(ls(6))
            TRADING_END = Str2NullTime(ls(7))
            TRADE_REGISTRATION_START = Str2NullTime(ls(8))
            TRADE_REGISTRATION_END = Str2NullTime(ls(9))
            TRADE_REGISTRATION_LAST = Str2NullTime(ls(10))
            OPTIONS_AVAILABLE = Str2Bool(ls(11))
            EXCHANGE_ROUTE_PERIOD_ID = Str2Int(ls(12))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As EXCHANGE_ROUTE_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If EXCHANGE_ROUTES_ID <> q.EXCHANGE_ROUTES_ID Then Exit Function
        If EXCHANGE_ID <> q.EXCHANGE_ID Then Exit Function
        If VESSEL_CLASS_ID <> q.VESSEL_CLASS_ID Then Exit Function
        If ROUTE_ID <> q.ROUTE_ID Then Exit Function
        If ACTIVE <> q.ACTIVE Then Exit Function
        If EXCHANGE_ABRV <> q.EXCHANGE_ABRV Then Exit Function
        If TRADING_START <> q.TRADING_START Then Exit Function
        If TRADING_END <> q.TRADING_END Then Exit Function
        If TRADE_REGISTRATION_START <> q.TRADE_REGISTRATION_START Then Exit Function
        If TRADE_REGISTRATION_END <> q.TRADE_REGISTRATION_END Then Exit Function
        If TRADE_REGISTRATION_LAST <> q.TRADE_REGISTRATION_LAST Then Exit Function
        If OPTIONS_AVAILABLE <> q.OPTIONS_AVAILABLE Then Exit Function
        If EXCHANGE_ROUTE_PERIOD_ID <> q.EXCHANGE_ROUTE_PERIOD_ID Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As EXCHANGE_ROUTE_CLASS
        GetNewCopy = New EXCHANGE_ROUTE_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.EXCHANGE_ROUTES_ID = 0
        GetNewCopy.EXCHANGE_ID = 0
        GetNewCopy.VESSEL_CLASS_ID = 0
        GetNewCopy.ROUTE_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------

End Class