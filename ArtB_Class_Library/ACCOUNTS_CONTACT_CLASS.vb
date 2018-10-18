Public Class ACCOUNTS_CONTACT_CLASS
    Inherits ACCOUNTS_CONTACT

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            ACCOUNTS_CONTACTS_ID = q.ACCOUNTS_CONTACTS_ID
            ACCOUNT_ID = q.ACCOUNT_ID
            CONTACT_ID = q.CONTACT_ID
            ACTIVE = q.ACTIVE
            MAIN_CONTACT = q.MAIN_CONTACT
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
            q.ACCOUNTS_CONTACTS_ID = ACCOUNTS_CONTACTS_ID
            q.ACCOUNT_ID = ACCOUNT_ID
            q.CONTACT_ID = CONTACT_ID
            q.ACTIVE = ACTIVE
            q.MAIN_CONTACT = MAIN_CONTACT
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.ACCOUNTS_CONTACTs _
        Where q.ACCOUNT_ID = ACCOUNT_ID And q.CONTACT_ID = CONTACT_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_ACCOUNT_ID As Integer, ByVal a_CONTACT_ID As Integer) As Integer
        ACCOUNT_ID = a_ACCOUNT_ID
        CONTACT_ID = a_CONTACT_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New ACCOUNTS_CONTACT
        SetToObject(q)

        gdb.ACCOUNTS_CONTACTs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                ACCOUNTS_CONTACTS_ID = q.ACCOUNTS_CONTACTS_ID
                ACCOUNT_ID = q.ACCOUNT_ID
                CONTACT_ID = q.CONTACT_ID
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

        Dim l = From q In gdb.ACCOUNTS_CONTACTs _
          Where q.ACCOUNTS_CONTACTS_ID = ACCOUNTS_CONTACTS_ID And q.ACCOUNT_ID = ACCOUNT_ID And q.CONTACT_ID = CONTACT_ID _
          Select q

        For Each q As ACCOUNTS_CONTACT In l
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

        Dim l = From q In gdb.ACCOUNTS_CONTACTs _
          Where q.ACCOUNT_ID = ACCOUNT_ID And q.CONTACT_ID = CONTACT_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As ACCOUNTS_CONTACT In l
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

        Dim l = From q In gdb.ACCOUNTS_CONTACTs _
         Where q.ACCOUNT_ID = ACCOUNT_ID And q.CONTACT_ID = CONTACT_ID _
          Select q

        For Each q As ACCOUNTS_CONTACT In l
            gdb.ACCOUNTS_CONTACTs.DeleteOnSubmit(q)
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
            s = s & Int2Str(ACCOUNTS_CONTACTS_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ACCOUNT_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(CONTACT_ID) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(ACTIVE) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(MAIN_CONTACT) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 5 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            ACCOUNTS_CONTACTS_ID = Str2Int(ls(0))
            ACCOUNT_ID = Str2Int(ls(1))
            CONTACT_ID = Str2Int(ls(2))
            ACTIVE = Str2Bool(ls(3))
            MAIN_CONTACT = Str2Bool(ls(4))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As ACCOUNTS_CONTACT_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If ACCOUNTS_CONTACTS_ID <> q.ACCOUNTS_CONTACTS_ID Then Exit Function
        If ACCOUNT_ID <> q.ACCOUNT_ID Then Exit Function
        If CONTACT_ID <> q.CONTACT_ID Then Exit Function
        If ACTIVE <> q.ACTIVE Then Exit Function
        If MAIN_CONTACT <> q.MAIN_CONTACT Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As ACCOUNTS_CONTACT_CLASS
        GetNewCopy = New ACCOUNTS_CONTACT_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.ACCOUNTS_CONTACTS_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------

End Class