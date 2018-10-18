Public Class LAST_DAY_RULE_CLASS
    Inherits LAST_DAY_RULE

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            LAST_DAY_RULE_ID = q.LAST_DAY_RULE_ID
            LAST_DAY_RULE_DESCR = q.LAST_DAY_RULE_DESCR
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
            q.LAST_DAY_RULE_ID = LAST_DAY_RULE_ID
            q.LAST_DAY_RULE_DESCR = LAST_DAY_RULE_DESCR
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.LAST_DAY_RULEs _
        Where q.LAST_DAY_RULE_ID = LAST_DAY_RULE_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_LAST_DAY_RULE_ID As Integer) As Integer
        LAST_DAY_RULE_ID = a_LAST_DAY_RULE_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New LAST_DAY_RULE
        SetToObject(q)

        gdb.LAST_DAY_RULEs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                LAST_DAY_RULE_ID = q.LAST_DAY_RULE_ID
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

        Dim l = From q In gdb.LAST_DAY_RULEs _
          Where q.LAST_DAY_RULE_ID = LAST_DAY_RULE_ID _
          Select q

        For Each q As LAST_DAY_RULE In l
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

        Dim l = From q In gdb.LAST_DAY_RULEs _
          Where q.LAST_DAY_RULE_ID = LAST_DAY_RULE_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As LAST_DAY_RULE In l
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

        Dim l = From q In gdb.LAST_DAY_RULEs _
         Where q.LAST_DAY_RULE_ID = LAST_DAY_RULE_ID _
          Select q

        For Each q As LAST_DAY_RULE In l
            gdb.LAST_DAY_RULEs.DeleteOnSubmit(q)
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
            s = s & Int2Str(LAST_DAY_RULE_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(LAST_DAY_RULE_DESCR) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 2 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            LAST_DAY_RULE_ID = Str2Int(ls(0))
            LAST_DAY_RULE_DESCR = ls(1)
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As LAST_DAY_RULE_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If LAST_DAY_RULE_ID <> q.LAST_DAY_RULE_ID Then Exit Function
        If LAST_DAY_RULE_DESCR <> q.LAST_DAY_RULE_DESCR Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As LAST_DAY_RULE_CLASS
        GetNewCopy = New LAST_DAY_RULE_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.LAST_DAY_RULE_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------

    Public MONTHS As New Collection
End Class
