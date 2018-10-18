Public Class DESK_EXCHANGES_CLEARER_CLASS
    Inherits DESK_EXCHANGES_CLEARER

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            ACCOUNT_DESK_ID = q.ACCOUNT_DESK_ID
            EXCHANGE_ID = q.EXCHANGE_ID
            ACCOUNT_ID = q.ACCOUNT_ID
            TRADE_CLASS_SHORT = q.TRADE_CLASS_SHORT
            CLEARER_ACCOUNT = q.CLEARER_ACCOUNT
            CLEARER_EXTRA1 = q.CLEARER_EXTRA1
            CLEARER_EXTRA2 = q.CLEARER_EXTRA2
            CLEARER_EXTRA3 = q.CLEARER_EXTRA3
            ACTIVE = q.ACTIVE
            MAIN = q.MAIN
            FORCE_TO_OTC = q.FORCE_TO_OTC
            ISDA_ID = q.ISDA_ID
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
            q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID
            q.EXCHANGE_ID = EXCHANGE_ID
            q.ACCOUNT_ID = ACCOUNT_ID
            q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT
            q.CLEARER_ACCOUNT = CLEARER_ACCOUNT
            q.CLEARER_EXTRA1 = CLEARER_EXTRA1
            q.CLEARER_EXTRA2 = CLEARER_EXTRA2
            q.CLEARER_EXTRA3 = CLEARER_EXTRA3
            q.ACTIVE = ACTIVE
            q.MAIN = MAIN
            q.FORCE_TO_OTC = FORCE_TO_OTC
            q.ISDA_ID = ISDA_ID
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.DESK_EXCHANGES_CLEARERs _
        Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID And q.EXCHANGE_ID = EXCHANGE_ID And q.ACCOUNT_ID = ACCOUNT_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_ACCOUNT_DESK_ID As Integer, ByVal a_EXCHANGE_ID As Integer, ByVal a_ACCOUNT_ID As Integer) As Integer
        ACCOUNT_DESK_ID = a_ACCOUNT_DESK_ID
        EXCHANGE_ID = a_EXCHANGE_ID
        ACCOUNT_ID = a_ACCOUNT_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New DESK_EXCHANGES_CLEARER
        SetToObject(q)

        gdb.DESK_EXCHANGES_CLEARERs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                ACCOUNT_DESK_ID = q.ACCOUNT_DESK_ID
                EXCHANGE_ID = q.EXCHANGE_ID
                ACCOUNT_ID = q.ACCOUNT_ID
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

        Dim l = From q In gdb.DESK_EXCHANGES_CLEARERs _
          Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID And q.EXCHANGE_ID = EXCHANGE_ID And q.ACCOUNT_ID = ACCOUNT_ID _
          Select q

        For Each q As DESK_EXCHANGES_CLEARER In l
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

        Dim l = From q In gdb.DESK_EXCHANGES_CLEARERs _
          Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID And q.EXCHANGE_ID = EXCHANGE_ID And q.ACCOUNT_ID = ACCOUNT_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As DESK_EXCHANGES_CLEARER In l
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

        Dim l = From q In gdb.DESK_EXCHANGES_CLEARERs _
         Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID And q.EXCHANGE_ID = EXCHANGE_ID And q.ACCOUNT_ID = ACCOUNT_ID _
          Select q

        For Each q As DESK_EXCHANGES_CLEARER In l
            gdb.DESK_EXCHANGES_CLEARERs.DeleteOnSubmit(q)
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
            s = s & Int2Str(ACCOUNT_DESK_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(EXCHANGE_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ACCOUNT_ID) & FIELD_SEPARATOR_STR
            s = s & TRADE_CLASS_SHORT & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(CLEARER_ACCOUNT) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(CLEARER_EXTRA1) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(CLEARER_EXTRA2) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(CLEARER_EXTRA3) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(ACTIVE) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(MAIN) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(FORCE_TO_OTC) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ISDA_ID) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 12 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            ACCOUNT_DESK_ID = Str2Int(ls(0))
            EXCHANGE_ID = Str2Int(ls(1))
            ACCOUNT_ID = Str2Int(ls(2))
            TRADE_CLASS_SHORT = Str2Char(ls(3))
            CLEARER_ACCOUNT = ls(4)
            CLEARER_EXTRA1 = ls(5)
            CLEARER_EXTRA2 = ls(6)
            CLEARER_EXTRA3 = ls(7)
            ACTIVE = Str2Bool(ls(8))
            MAIN = Str2Bool(ls(9))
            FORCE_TO_OTC = Str2Bool(ls(10))
            ISDA_ID = Str2Int(ls(11))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As DESK_EXCHANGES_CLEARER_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If ACCOUNT_DESK_ID <> q.ACCOUNT_DESK_ID Then Exit Function
        If EXCHANGE_ID <> q.EXCHANGE_ID Then Exit Function
        If ACCOUNT_ID <> q.ACCOUNT_ID Then Exit Function
        If TRADE_CLASS_SHORT <> q.TRADE_CLASS_SHORT Then Exit Function
        If CLEARER_ACCOUNT <> q.CLEARER_ACCOUNT Then Exit Function
        If CLEARER_EXTRA1 <> q.CLEARER_EXTRA1 Then Exit Function
        If CLEARER_EXTRA2 <> q.CLEARER_EXTRA2 Then Exit Function
        If CLEARER_EXTRA3 <> q.CLEARER_EXTRA3 Then Exit Function
        If ACTIVE <> q.ACTIVE Then Exit Function
        If MAIN <> q.MAIN Then Exit Function
        If FORCE_TO_OTC <> q.FORCE_TO_OTC Then Exit Function
        If ISDA_ID <> q.ISDA_ID Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As DESK_EXCHANGES_CLEARER_CLASS
        GetNewCopy = New DESK_EXCHANGES_CLEARER_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.ACCOUNT_DESK_ID = 0
        GetNewCopy.EXCHANGE_ID = 0
        GetNewCopy.ACCOUNT_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------

End Class