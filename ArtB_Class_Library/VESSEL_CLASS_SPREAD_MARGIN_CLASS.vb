Public Class VESSEL_CLASS_SPREAD_MARGIN_CLASS
    Inherits VESSEL_CLASS_SPREAD_MARGIN

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            VESSEL_CLASS_ID = q.VESSEL_CLASS_ID
            ROUTE_ID = q.ROUTE_ID
            CMSROUTE_ID = q.CMSROUTE_ID
            MM1 = q.MM1
            YY1 = q.YY1
            MM2 = q.MM2
            YY2 = q.YY2
            PERIOD = q.PERIOD
            YY = q.YY
            MARGIN = q.MARGIN
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
            q.ROUTE_ID = ROUTE_ID
            q.CMSROUTE_ID = CMSROUTE_ID
            q.MM1 = MM1
            q.YY1 = YY1
            q.MM2 = MM2
            q.YY2 = YY2
            q.PERIOD = PERIOD
            q.YY = YY
            q.MARGIN = MARGIN
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.VESSEL_CLASS_SPREAD_MARGINs _
        Where q.VESSEL_CLASS_ID = VESSEL_CLASS_ID And q.ROUTE_ID = ROUTE_ID And q.CMSROUTE_ID = CMSROUTE_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_VESSEL_CLASS_ID As Integer, ByVal a_ROUTE_ID As Integer, ByVal a_CMSROUTE_ID As String) As Integer
        VESSEL_CLASS_ID = a_VESSEL_CLASS_ID
        ROUTE_ID = a_ROUTE_ID
        CMSROUTE_ID = a_CMSROUTE_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New VESSEL_CLASS_SPREAD_MARGIN
        SetToObject(q)

        gdb.VESSEL_CLASS_SPREAD_MARGINs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                VESSEL_CLASS_ID = q.VESSEL_CLASS_ID
                ROUTE_ID = q.ROUTE_ID
                CMSROUTE_ID = q.CMSROUTE_ID
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

        Dim l = From q In gdb.VESSEL_CLASS_SPREAD_MARGINs _
          Where q.VESSEL_CLASS_ID = VESSEL_CLASS_ID And q.ROUTE_ID = ROUTE_ID And q.CMSROUTE_ID = CMSROUTE_ID _
          Select q

        For Each q As VESSEL_CLASS_SPREAD_MARGIN In l
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

        Dim l = From q In gdb.VESSEL_CLASS_SPREAD_MARGINs _
          Where q.VESSEL_CLASS_ID = VESSEL_CLASS_ID And q.ROUTE_ID = ROUTE_ID And q.CMSROUTE_ID = CMSROUTE_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As VESSEL_CLASS_SPREAD_MARGIN In l
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

        Dim l = From q In gdb.VESSEL_CLASS_SPREAD_MARGINs _
         Where q.VESSEL_CLASS_ID = VESSEL_CLASS_ID And q.ROUTE_ID = ROUTE_ID And q.CMSROUTE_ID = CMSROUTE_ID _
          Select q

        For Each q As VESSEL_CLASS_SPREAD_MARGIN In l
            gdb.VESSEL_CLASS_SPREAD_MARGINs.DeleteOnSubmit(q)
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
            s = s & Int2Str(ROUTE_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(CMSROUTE_ID) & FIELD_SEPARATOR_STR
            s = s & Short2Str(MM1) & FIELD_SEPARATOR_STR
            s = s & Short2Str(YY1) & FIELD_SEPARATOR_STR
            s = s & Short2Str(MM2) & FIELD_SEPARATOR_STR
            s = s & Short2Str(YY2) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(PERIOD) & FIELD_SEPARATOR_STR
            s = s & Int2Str(YY) & FIELD_SEPARATOR_STR
            s = s & Double2Str(MARGIN) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 10 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            VESSEL_CLASS_ID = Str2Int(ls(0))
            ROUTE_ID = Str2Int(ls(1))
            CMSROUTE_ID = ls(2)
            MM1 = Str2Short(ls(3))
            YY1 = Str2Short(ls(4))
            MM2 = Str2Short(ls(5))
            YY2 = Str2Short(ls(6))
            PERIOD = ls(7)
            YY = Str2Int(ls(8))
            MARGIN = Str2Double(ls(9))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As VESSEL_CLASS_SPREAD_MARGIN_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If VESSEL_CLASS_ID <> q.VESSEL_CLASS_ID Then Exit Function
        If ROUTE_ID <> q.ROUTE_ID Then Exit Function
        If CMSROUTE_ID <> q.CMSROUTE_ID Then Exit Function
        If MM1 <> q.MM1 Then Exit Function
        If YY1 <> q.YY1 Then Exit Function
        If MM2 <> q.MM2 Then Exit Function
        If YY2 <> q.YY2 Then Exit Function
        If PERIOD <> q.PERIOD Then Exit Function
        If YY <> q.YY Then Exit Function
        If MARGIN <> q.MARGIN Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As VESSEL_CLASS_SPREAD_MARGIN_CLASS
        GetNewCopy = New VESSEL_CLASS_SPREAD_MARGIN_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.VESSEL_CLASS_ID = 0
        GetNewCopy.ROUTE_ID = 0
        GetNewCopy.CMSROUTE_ID = ""
    End Function

    '-----------------------------------------------------------------------------------------------

End Class
