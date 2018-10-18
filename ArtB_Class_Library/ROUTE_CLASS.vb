Public Class ROUTE_CLASS
    Inherits ROUTE

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            ROUTE_ID = q.ROUTE_ID
            VESSEL_CLASS_ID = q.VESSEL_CLASS_ID
            QUOTE_TYPE = q.QUOTE_TYPE
            QUANTITY_TYPE = q.QUANTITY_TYPE
            CCY_ID = q.CCY_ID
            ROUTE_SHORT = q.ROUTE_SHORT
            ROUTE_DESCR = q.ROUTE_DESCR
            BALTIC_ABRV = q.BALTIC_ABRV
            BALTIC_ABRV_FFA = q.BALTIC_ABRV_FFA
            SETTLEMENT_TYPE = q.SETTLEMENT_TYPE
            LOT_SIZE = q.LOT_SIZE
            SETTLEMENT_TICK = q.SETTLEMENT_TICK
            PRICING_TICK = q.PRICING_TICK
            FULL_LOT_SIZE = q.FULL_LOT_SIZE
            FFA_TRADED = q.FFA_TRADED
            LAST_DAY_RULE_ID = q.LAST_DAY_RULE_ID
            DEFAULT_QUANTITY = q.DEFAULT_QUANTITY
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
            q.ROUTE_ID = ROUTE_ID
            q.VESSEL_CLASS_ID = VESSEL_CLASS_ID
            q.QUOTE_TYPE = QUOTE_TYPE
            q.QUANTITY_TYPE = QUANTITY_TYPE
            q.CCY_ID = CCY_ID
            q.ROUTE_SHORT = ROUTE_SHORT
            q.ROUTE_DESCR = ROUTE_DESCR
            q.BALTIC_ABRV = BALTIC_ABRV
            q.BALTIC_ABRV_FFA = BALTIC_ABRV_FFA
            q.SETTLEMENT_TYPE = SETTLEMENT_TYPE
            q.LOT_SIZE = LOT_SIZE
            q.SETTLEMENT_TICK = SETTLEMENT_TICK
            q.PRICING_TICK = PRICING_TICK
            q.FULL_LOT_SIZE = FULL_LOT_SIZE
            q.FFA_TRADED = FFA_TRADED
            q.LAST_DAY_RULE_ID = LAST_DAY_RULE_ID
            q.DEFAULT_QUANTITY = DEFAULT_QUANTITY
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.ROUTEs _
        Where q.ROUTE_ID = ROUTE_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_ROUTE_ID As Integer) As Integer
        ROUTE_ID = a_ROUTE_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New ROUTE
        SetToObject(q)

        gdb.ROUTEs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                ROUTE_ID = q.ROUTE_ID
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

        Dim l = From q In gdb.ROUTEs _
          Where q.ROUTE_ID = ROUTE_ID _
          Select q

        For Each q As ROUTE In l
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

        Dim l = From q In gdb.ROUTEs _
          Where q.ROUTE_ID = ROUTE_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As ROUTE In l
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

        Dim l = From q In gdb.ROUTEs _
         Where q.ROUTE_ID = ROUTE_ID _
          Select q

        For Each q As ROUTE In l
            gdb.ROUTEs.DeleteOnSubmit(q)
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
            s = s & Int2Str(ROUTE_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(VESSEL_CLASS_ID) & FIELD_SEPARATOR_STR
            s = s & Short2Str(QUOTE_TYPE) & FIELD_SEPARATOR_STR
            s = s & Short2Str(QUANTITY_TYPE) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(CCY_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(ROUTE_SHORT) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(ROUTE_DESCR) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(BALTIC_ABRV) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(BALTIC_ABRV_FFA) & FIELD_SEPARATOR_STR
            s = s & Short2Str(SETTLEMENT_TYPE) & FIELD_SEPARATOR_STR
            s = s & Int2Str(LOT_SIZE) & FIELD_SEPARATOR_STR
            s = s & Double2Str(SETTLEMENT_TICK) & FIELD_SEPARATOR_STR
            s = s & Double2Str(PRICING_TICK) & FIELD_SEPARATOR_STR
            s = s & Int2Str(FULL_LOT_SIZE) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(FFA_TRADED) & FIELD_SEPARATOR_STR
            s = s & Int2Str(LAST_DAY_RULE_ID) & FIELD_SEPARATOR_STR
            s = s & Double2Str(DEFAULT_QUANTITY) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 17 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            ROUTE_ID = Str2Int(ls(0))
            VESSEL_CLASS_ID = Str2Int(ls(1))
            QUOTE_TYPE = Str2Short(ls(2))
            QUANTITY_TYPE = Str2Short(ls(3))
            CCY_ID = Str2NullInt(ls(4))
            ROUTE_SHORT = ls(5)
            ROUTE_DESCR = ls(6)
            BALTIC_ABRV = ls(7)
            BALTIC_ABRV_FFA = ls(8)
            SETTLEMENT_TYPE = Str2Short(ls(9))
            LOT_SIZE = Str2Int(ls(10))
            SETTLEMENT_TICK = Str2Double(ls(11))
            PRICING_TICK = Str2Double(ls(12))
            FULL_LOT_SIZE = Str2Int(ls(13))
            FFA_TRADED = Str2Bool(ls(14))
            LAST_DAY_RULE_ID = Str2Int(ls(15))
            DEFAULT_QUANTITY = Str2Double(ls(16))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As ROUTE_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If ROUTE_ID <> q.ROUTE_ID Then Exit Function
        If VESSEL_CLASS_ID <> q.VESSEL_CLASS_ID Then Exit Function
        If QUOTE_TYPE <> q.QUOTE_TYPE Then Exit Function
        If QUANTITY_TYPE <> q.QUANTITY_TYPE Then Exit Function
        If CCY_ID <> q.CCY_ID Then Exit Function
        If ROUTE_SHORT <> q.ROUTE_SHORT Then Exit Function
        If ROUTE_DESCR <> q.ROUTE_DESCR Then Exit Function
        If BALTIC_ABRV <> q.BALTIC_ABRV Then Exit Function
        If BALTIC_ABRV_FFA <> q.BALTIC_ABRV_FFA Then Exit Function
        If SETTLEMENT_TYPE <> q.SETTLEMENT_TYPE Then Exit Function
        If LOT_SIZE <> q.LOT_SIZE Then Exit Function
        If SETTLEMENT_TICK <> q.SETTLEMENT_TICK Then Exit Function
        If PRICING_TICK <> q.PRICING_TICK Then Exit Function
        If FULL_LOT_SIZE <> q.FULL_LOT_SIZE Then Exit Function
        If FFA_TRADED <> q.FFA_TRADED Then Exit Function
        If LAST_DAY_RULE_ID <> q.LAST_DAY_RULE_ID Then Exit Function
        If DEFAULT_QUANTITY <> q.DEFAULT_QUANTITY Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As ROUTE_CLASS
        GetNewCopy = New ROUTE_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.ROUTE_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------
    Public EXCHANGES As New Collection

End Class
