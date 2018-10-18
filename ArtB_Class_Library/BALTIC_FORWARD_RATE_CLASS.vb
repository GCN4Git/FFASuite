Public Class BALTIC_FORWARD_RATE_CLASS
    Inherits BALTIC_FORWARD_RATE

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            ROUTE_ID = q.ROUTE_ID
            CMSROUTE_ID = q.CMSROUTE_ID
            FIXING_DATE = q.FIXING_DATE
            NEXT_ROLLOVER_DATE = q.NEXT_ROLLOVER_DATE
            FIXING = q.FIXING
            REPORTDESC = q.REPORTDESC
            MM1 = q.MM1
            YY1 = q.YY1
            MM2 = q.MM2
            YY2 = q.YY2
            PERIOD = q.PERIOD
            YY = q.YY
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
            q.CMSROUTE_ID = CMSROUTE_ID
            q.FIXING_DATE = FIXING_DATE
            q.NEXT_ROLLOVER_DATE = NEXT_ROLLOVER_DATE
            q.FIXING = FIXING
            q.REPORTDESC = REPORTDESC
            q.MM1 = MM1
            q.YY1 = YY1
            q.MM2 = MM2
            q.YY2 = YY2
            q.PERIOD = PERIOD
            q.YY = YY
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.BALTIC_FORWARD_RATEs _
        Where q.ROUTE_ID = ROUTE_ID And q.CMSROUTE_ID = CMSROUTE_ID And q.FIXING_DATE = FIXING_DATE _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_ROUTE_ID As Integer, ByVal a_CMSROUTE_ID As String, ByVal a_FIXING_DATE As DateTime) As Integer
        ROUTE_ID = a_ROUTE_ID
        CMSROUTE_ID = a_CMSROUTE_ID
        FIXING_DATE = a_FIXING_DATE
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New BALTIC_FORWARD_RATE
        SetToObject(q)

        gdb.BALTIC_FORWARD_RATEs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                ROUTE_ID = q.ROUTE_ID
                CMSROUTE_ID = q.CMSROUTE_ID
                FIXING_DATE = q.FIXING_DATE
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

        Dim l = From q In gdb.BALTIC_FORWARD_RATEs _
          Where q.ROUTE_ID = ROUTE_ID And q.CMSROUTE_ID = CMSROUTE_ID And q.FIXING_DATE = FIXING_DATE _
          Select q

        For Each q As BALTIC_FORWARD_RATE In l
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

        Dim l = From q In gdb.BALTIC_FORWARD_RATEs _
          Where q.ROUTE_ID = ROUTE_ID And q.CMSROUTE_ID = CMSROUTE_ID And q.FIXING_DATE = FIXING_DATE _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As BALTIC_FORWARD_RATE In l
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

        Dim l = From q In gdb.BALTIC_FORWARD_RATEs _
         Where q.ROUTE_ID = ROUTE_ID And q.CMSROUTE_ID = CMSROUTE_ID And q.FIXING_DATE = FIXING_DATE _
          Select q

        For Each q As BALTIC_FORWARD_RATE In l
            gdb.BALTIC_FORWARD_RATEs.DeleteOnSubmit(q)
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
            s = s & Str2QuotedStr(CMSROUTE_ID) & FIELD_SEPARATOR_STR
            s = s & DateTime2Str(FIXING_DATE) & FIELD_SEPARATOR_STR
            s = s & DateTime2Str(NEXT_ROLLOVER_DATE) & FIELD_SEPARATOR_STR
            s = s & Double2Str(FIXING) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(REPORTDESC) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(MM1) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(YY1) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(MM2) & FIELD_SEPARATOR_STR
            s = s & NullShort2Str(YY2) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(PERIOD) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(YY) & RECORD_SEPARATOR_STR
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
            ROUTE_ID = Str2Int(ls(0))
            CMSROUTE_ID = ls(1)
            FIXING_DATE = Str2DateTime(ls(2))
            NEXT_ROLLOVER_DATE = Str2DateTime(ls(3))
            FIXING = Str2Double(ls(4))
            REPORTDESC = ls(5)
            MM1 = Str2NullShort(ls(6))
            YY1 = Str2NullShort(ls(7))
            MM2 = Str2NullShort(ls(8))
            YY2 = Str2NullShort(ls(9))
            PERIOD = ls(10)
            YY = Str2NullInt(ls(11))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As BALTIC_FORWARD_RATE_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If ROUTE_ID <> q.ROUTE_ID Then Exit Function
        If CMSROUTE_ID <> q.CMSROUTE_ID Then Exit Function
        If FIXING_DATE <> q.FIXING_DATE Then Exit Function
        If NEXT_ROLLOVER_DATE <> q.NEXT_ROLLOVER_DATE Then Exit Function
        If FIXING <> q.FIXING Then Exit Function
        If REPORTDESC <> q.REPORTDESC Then Exit Function
        If MM1 <> q.MM1 Then Exit Function
        If YY1 <> q.YY1 Then Exit Function
        If MM2 <> q.MM2 Then Exit Function
        If YY2 <> q.YY2 Then Exit Function
        If PERIOD <> q.PERIOD Then Exit Function
        If YY <> q.YY Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As BALTIC_FORWARD_RATE_CLASS
        GetNewCopy = New BALTIC_FORWARD_RATE_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.ROUTE_ID = 0
        GetNewCopy.CMSROUTE_ID = ""
        GetNewCopy.FIXING_DATE = "2000-01-01"
    End Function

    '-----------------------------------------------------------------------------------------------

End Class