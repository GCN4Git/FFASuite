Public Class VESSEL_CLASS_CLASS
    Inherits VESSEL_CLASS

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            VESSEL_CLASS_ID = q.VESSEL_CLASS_ID
            VESSEL_CLASS = q.VESSEL_CLASS
            DRYWET = q.DRYWET
            WETSECTOR = q.WETSECTOR
            DESCRIPTION = q.DESCRIPTION
            DEFAULT_ROUTE_ID = q.DEFAULT_ROUTE_ID
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
            q.VESSEL_CLASS_ID = VESSEL_CLASS_ID
            q.VESSEL_CLASS = VESSEL_CLASS
            q.DRYWET = DRYWET
            q.WETSECTOR = WETSECTOR
            q.DESCRIPTION = DESCRIPTION
            q.DEFAULT_ROUTE_ID = DEFAULT_ROUTE_ID
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.VESSEL_CLASSes _
        Where q.VESSEL_CLASS_ID = VESSEL_CLASS_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_VESSEL_CLASS_ID As Integer) As Integer
        VESSEL_CLASS_ID = a_VESSEL_CLASS_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New VESSEL_CLASS
        SetToObject(q)

        gdb.VESSEL_CLASSes.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                VESSEL_CLASS_ID = q.VESSEL_CLASS_ID
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

        Dim l = From q In gdb.VESSEL_CLASSes _
          Where q.VESSEL_CLASS_ID = VESSEL_CLASS_ID _
          Select q

        For Each q As VESSEL_CLASS In l
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

        Dim l = From q In gdb.VESSEL_CLASSes _
          Where q.VESSEL_CLASS_ID = VESSEL_CLASS_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As VESSEL_CLASS In l
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

        Dim l = From q In gdb.VESSEL_CLASSes _
         Where q.VESSEL_CLASS_ID = VESSEL_CLASS_ID _
          Select q

        For Each q As VESSEL_CLASS In l
            gdb.VESSEL_CLASSes.DeleteOnSubmit(q)
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
            s = s & Int2Str(VESSEL_CLASS_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(VESSEL_CLASS) & FIELD_SEPARATOR_STR
            s = s & DRYWET & FIELD_SEPARATOR_STR
            s = s & WETSECTOR & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(DESCRIPTION) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(DEFAULT_ROUTE_ID) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 6 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            VESSEL_CLASS_ID = Str2Int(ls(0))
            VESSEL_CLASS = ls(1)
            DRYWET = Str2Char(ls(2))
            WETSECTOR = Str2NullChar(ls(3))
            DESCRIPTION = ls(4)
            DEFAULT_ROUTE_ID = Str2NullInt(ls(5))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As VESSEL_CLASS_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If VESSEL_CLASS_ID <> q.VESSEL_CLASS_ID Then Exit Function
        If VESSEL_CLASS <> q.VESSEL_CLASS Then Exit Function
        If DRYWET <> q.DRYWET Then Exit Function
        If WETSECTOR <> q.WETSECTOR Then Exit Function
        If DESCRIPTION <> q.DESCRIPTION Then Exit Function
        If DEFAULT_ROUTE_ID <> q.DEFAULT_ROUTE_ID Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As VESSEL_CLASS_CLASS
        GetNewCopy = New VESSEL_CLASS_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.VESSEL_CLASS_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------

End Class