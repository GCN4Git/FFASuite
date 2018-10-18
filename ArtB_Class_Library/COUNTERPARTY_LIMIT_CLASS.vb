Public Class COUNTERPARTY_LIMIT_CLASS
    Inherits COUNTERPARTY_LIMIT

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            PRI_ACCOUNT_DESK_ID = q.PRI_ACCOUNT_DESK_ID
            SEC_ACCOUNT_DESK_ID = q.SEC_ACCOUNT_DESK_ID
            CLEARED = q.CLEARED
            CLEARED_SHOW_NAME = q.CLEARED_SHOW_NAME
            OTC = q.OTC
            OTC_SHOW_NAME = q.OTC_SHOW_NAME
            PERIOD_LIMIT = q.PERIOD_LIMIT
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
            q.PRI_ACCOUNT_DESK_ID = PRI_ACCOUNT_DESK_ID
            q.SEC_ACCOUNT_DESK_ID = SEC_ACCOUNT_DESK_ID
            q.CLEARED = CLEARED
            q.CLEARED_SHOW_NAME = CLEARED_SHOW_NAME
            q.OTC = OTC
            q.OTC_SHOW_NAME = OTC_SHOW_NAME
            q.PERIOD_LIMIT = PERIOD_LIMIT
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.COUNTERPARTY_LIMITs _
        Where q.PRI_ACCOUNT_DESK_ID = PRI_ACCOUNT_DESK_ID And q.SEC_ACCOUNT_DESK_ID = SEC_ACCOUNT_DESK_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_PRI_ACCOUNT_DESK_ID As Integer, ByVal a_SEC_ACCOUNT_DESK_ID As Integer) As Integer
        PRI_ACCOUNT_DESK_ID = a_PRI_ACCOUNT_DESK_ID
        SEC_ACCOUNT_DESK_ID = a_SEC_ACCOUNT_DESK_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New COUNTERPARTY_LIMIT
        SetToObject(q)

        gdb.COUNTERPARTY_LIMITs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                PRI_ACCOUNT_DESK_ID = q.PRI_ACCOUNT_DESK_ID
                SEC_ACCOUNT_DESK_ID = q.SEC_ACCOUNT_DESK_ID
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

        Dim l = From q In gdb.COUNTERPARTY_LIMITs _
          Where q.PRI_ACCOUNT_DESK_ID = PRI_ACCOUNT_DESK_ID And q.SEC_ACCOUNT_DESK_ID = SEC_ACCOUNT_DESK_ID _
          Select q

        For Each q As COUNTERPARTY_LIMIT In l
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

        Dim l = From q In gdb.COUNTERPARTY_LIMITs _
          Where q.PRI_ACCOUNT_DESK_ID = PRI_ACCOUNT_DESK_ID And q.SEC_ACCOUNT_DESK_ID = SEC_ACCOUNT_DESK_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As COUNTERPARTY_LIMIT In l
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

        Dim l = From q In gdb.COUNTERPARTY_LIMITs _
         Where q.PRI_ACCOUNT_DESK_ID = PRI_ACCOUNT_DESK_ID And q.SEC_ACCOUNT_DESK_ID = SEC_ACCOUNT_DESK_ID _
          Select q

        For Each q As COUNTERPARTY_LIMIT In l
            gdb.COUNTERPARTY_LIMITs.DeleteOnSubmit(q)
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
            s = s & Int2Str(PRI_ACCOUNT_DESK_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(SEC_ACCOUNT_DESK_ID) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(CLEARED) & FIELD_SEPARATOR_STR
            s = s & Short2Str(CLEARED_SHOW_NAME) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(OTC) & FIELD_SEPARATOR_STR
            s = s & Short2Str(OTC_SHOW_NAME) & FIELD_SEPARATOR_STR
            s = s & Int2Str(PERIOD_LIMIT) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 7 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            PRI_ACCOUNT_DESK_ID = Str2Int(ls(0))
            SEC_ACCOUNT_DESK_ID = Str2Int(ls(1))
            CLEARED = Str2Bool(ls(2))
            CLEARED_SHOW_NAME = Str2Short(ls(3))
            OTC = Str2Bool(ls(4))
            OTC_SHOW_NAME = Str2Short(ls(5))
            PERIOD_LIMIT = Str2Int(ls(6))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As COUNTERPARTY_LIMIT_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If PRI_ACCOUNT_DESK_ID <> q.PRI_ACCOUNT_DESK_ID Then Exit Function
        If SEC_ACCOUNT_DESK_ID <> q.SEC_ACCOUNT_DESK_ID Then Exit Function
        If CLEARED <> q.CLEARED Then Exit Function
        If CLEARED_SHOW_NAME <> q.CLEARED_SHOW_NAME Then Exit Function
        If OTC <> q.OTC Then Exit Function
        If OTC_SHOW_NAME <> q.OTC_SHOW_NAME Then Exit Function
        If PERIOD_LIMIT <> q.PERIOD_LIMIT Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As COUNTERPARTY_LIMIT_CLASS
        GetNewCopy = New COUNTERPARTY_LIMIT_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.PRI_ACCOUNT_DESK_ID = 0
        GetNewCopy.SEC_ACCOUNT_DESK_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------

End Class