Public Class EXCHANGE_CLASS
    Inherits EXCHANGE

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            EXCHANGE_ID = q.EXCHANGE_ID
            EXCHANGE_NAME_FULL = q.EXCHANGE_NAME_FULL
            EXCHANGE_NAME_SHORT = q.EXCHANGE_NAME_SHORT
            EXCHANGE_OPEN_TIME = q.EXCHANGE_OPEN_TIME
            EXCHANGE_CLOSING_TIME = q.EXCHANGE_CLOSING_TIME
            COUNTRY_ID = q.COUNTRY_ID
            EXCHANGE_WEBSITE = q.EXCHANGE_WEBSITE
            EXCHANGE_GMT = q.EXCHANGE_GMT
            ACCOUNT_ID = q.ACCOUNT_ID
            HALF_DAYS = q.HALF_DAYS
            DEFAULT_CLEARER_ID = q.DEFAULT_CLEARER_ID
            FORCE_TO_OTC = q.FORCE_TO_OTC
            ISDA_ID = q.ISDA_ID
            EXCHANGE_SHORTCUT = q.EXCHANGE_SHORTCUT
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
            q.EXCHANGE_ID = EXCHANGE_ID
            q.EXCHANGE_NAME_FULL = EXCHANGE_NAME_FULL
            q.EXCHANGE_NAME_SHORT = EXCHANGE_NAME_SHORT
            q.EXCHANGE_OPEN_TIME = EXCHANGE_OPEN_TIME
            q.EXCHANGE_CLOSING_TIME = EXCHANGE_CLOSING_TIME
            q.COUNTRY_ID = COUNTRY_ID
            q.EXCHANGE_WEBSITE = EXCHANGE_WEBSITE
            q.EXCHANGE_GMT = EXCHANGE_GMT
            q.ACCOUNT_ID = ACCOUNT_ID
            q.HALF_DAYS = HALF_DAYS
            q.DEFAULT_CLEARER_ID = DEFAULT_CLEARER_ID
            q.FORCE_TO_OTC = FORCE_TO_OTC
            q.ISDA_ID = ISDA_ID
            q.EXCHANGE_SHORTCUT = EXCHANGE_SHORTCUT
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.EXCHANGEs _
        Where q.EXCHANGE_ID = EXCHANGE_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_EXCHANGE_ID As Integer) As Integer
        EXCHANGE_ID = a_EXCHANGE_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New EXCHANGE
        SetToObject(q)

        gdb.EXCHANGEs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                EXCHANGE_ID = q.EXCHANGE_ID
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

        Dim l = From q In gdb.EXCHANGEs _
          Where q.EXCHANGE_ID = EXCHANGE_ID _
          Select q

        For Each q As EXCHANGE In l
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

        Dim l = From q In gdb.EXCHANGEs _
          Where q.EXCHANGE_ID = EXCHANGE_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As EXCHANGE In l
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

        Dim l = From q In gdb.EXCHANGEs _
         Where q.EXCHANGE_ID = EXCHANGE_ID _
          Select q

        For Each q As EXCHANGE In l
            gdb.EXCHANGEs.DeleteOnSubmit(q)
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
            s = s & Int2Str(EXCHANGE_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(EXCHANGE_NAME_FULL) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(EXCHANGE_NAME_SHORT) & FIELD_SEPARATOR_STR
            s = s & Time2Str(EXCHANGE_OPEN_TIME) & FIELD_SEPARATOR_STR
            s = s & Time2Str(EXCHANGE_CLOSING_TIME) & FIELD_SEPARATOR_STR
            s = s & Int2Str(COUNTRY_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(EXCHANGE_WEBSITE) & FIELD_SEPARATOR_STR
            s = s & Double2Str(EXCHANGE_GMT) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(ACCOUNT_ID) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(HALF_DAYS) & FIELD_SEPARATOR_STR
            s = s & Int2Str(DEFAULT_CLEARER_ID) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(FORCE_TO_OTC) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ISDA_ID) & FIELD_SEPARATOR_STR
            s = s & EXCHANGE_SHORTCUT & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 14 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            EXCHANGE_ID = Str2Int(ls(0))
            EXCHANGE_NAME_FULL = ls(1)
            EXCHANGE_NAME_SHORT = ls(2)
            EXCHANGE_OPEN_TIME = Str2Time(ls(3))
            EXCHANGE_CLOSING_TIME = Str2Time(ls(4))
            COUNTRY_ID = Str2Int(ls(5))
            EXCHANGE_WEBSITE = ls(6)
            EXCHANGE_GMT = Str2Double(ls(7))
            ACCOUNT_ID = Str2NullInt(ls(8))
            HALF_DAYS = Str2Bool(ls(9))
            DEFAULT_CLEARER_ID = Str2Int(ls(10))
            FORCE_TO_OTC = Str2Bool(ls(11))
            ISDA_ID = Str2Int(ls(12))
            EXCHANGE_SHORTCUT = Str2Char(ls(13))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As EXCHANGE_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If EXCHANGE_ID <> q.EXCHANGE_ID Then Exit Function
        If EXCHANGE_NAME_FULL <> q.EXCHANGE_NAME_FULL Then Exit Function
        If EXCHANGE_NAME_SHORT <> q.EXCHANGE_NAME_SHORT Then Exit Function
        If EXCHANGE_OPEN_TIME <> q.EXCHANGE_OPEN_TIME Then Exit Function
        If EXCHANGE_CLOSING_TIME <> q.EXCHANGE_CLOSING_TIME Then Exit Function
        If COUNTRY_ID <> q.COUNTRY_ID Then Exit Function
        If EXCHANGE_WEBSITE <> q.EXCHANGE_WEBSITE Then Exit Function
        If EXCHANGE_GMT <> q.EXCHANGE_GMT Then Exit Function
        If ACCOUNT_ID <> q.ACCOUNT_ID Then Exit Function
        If HALF_DAYS <> q.HALF_DAYS Then Exit Function
        If DEFAULT_CLEARER_ID <> q.DEFAULT_CLEARER_ID Then Exit Function
        If FORCE_TO_OTC <> q.FORCE_TO_OTC Then Exit Function
        If ISDA_ID <> q.ISDA_ID Then Exit Function
        If EXCHANGE_SHORTCUT <> q.EXCHANGE_SHORTCUT Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As EXCHANGE_CLASS
        GetNewCopy = New EXCHANGE_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.EXCHANGE_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------

End Class