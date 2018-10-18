Public Class EXCHANGE_ROUTE_PERIOD_CLASS
    Inherits EXCHANGE_ROUTE_PERIOD

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            EXCHANGE_ROUTE_PERIOD_ID = q.EXCHANGE_ROUTE_PERIOD_ID
            EXCHANGE_ROUTE_PERIOD_DESCR = q.EXCHANGE_ROUTE_PERIOD_DESCR
            FRONT_MONTHS = q.FRONT_MONTHS
            FRONT_QUARTERS = q.FRONT_QUARTERS
            FRONT_HALF_YEARS = q.FRONT_HALF_YEARS
            FRONT_YEARS = q.FRONT_YEARS
            FRONT_MAX_MONTHS = q.FRONT_MAX_MONTHS
            MC_0_1 = q.MC_0_1
            MC_1_2 = q.MC_1_2
            MC_0_1_2 = q.MC_0_1_2
            QC_0_1 = q.QC_0_1
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
            q.EXCHANGE_ROUTE_PERIOD_ID = EXCHANGE_ROUTE_PERIOD_ID
            q.EXCHANGE_ROUTE_PERIOD_DESCR = EXCHANGE_ROUTE_PERIOD_DESCR
            q.FRONT_MONTHS = FRONT_MONTHS
            q.FRONT_QUARTERS = FRONT_QUARTERS
            q.FRONT_HALF_YEARS = FRONT_HALF_YEARS
            q.FRONT_YEARS = FRONT_YEARS
            q.FRONT_MAX_MONTHS = FRONT_MAX_MONTHS
            q.MC_0_1 = MC_0_1
            q.MC_1_2 = MC_1_2
            q.MC_0_1_2 = MC_0_1_2
            q.QC_0_1 = QC_0_1
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.EXCHANGE_ROUTE_PERIODs _
        Where q.EXCHANGE_ROUTE_PERIOD_ID = EXCHANGE_ROUTE_PERIOD_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_EXCHANGE_ROUTE_PERIOD_ID As Integer) As Integer
        EXCHANGE_ROUTE_PERIOD_ID = a_EXCHANGE_ROUTE_PERIOD_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New EXCHANGE_ROUTE_PERIOD
        SetToObject(q)

        gdb.EXCHANGE_ROUTE_PERIODs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                EXCHANGE_ROUTE_PERIOD_ID = q.EXCHANGE_ROUTE_PERIOD_ID
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

        Dim l = From q In gdb.EXCHANGE_ROUTE_PERIODs _
          Where q.EXCHANGE_ROUTE_PERIOD_ID = EXCHANGE_ROUTE_PERIOD_ID _
          Select q

        For Each q As EXCHANGE_ROUTE_PERIOD In l
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

        Dim l = From q In gdb.EXCHANGE_ROUTE_PERIODs _
          Where q.EXCHANGE_ROUTE_PERIOD_ID = EXCHANGE_ROUTE_PERIOD_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As EXCHANGE_ROUTE_PERIOD In l
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

        Dim l = From q In gdb.EXCHANGE_ROUTE_PERIODs _
         Where q.EXCHANGE_ROUTE_PERIOD_ID = EXCHANGE_ROUTE_PERIOD_ID _
          Select q

        For Each q As EXCHANGE_ROUTE_PERIOD In l
            gdb.EXCHANGE_ROUTE_PERIODs.DeleteOnSubmit(q)
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
            s = s & Int2Str(EXCHANGE_ROUTE_PERIOD_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(EXCHANGE_ROUTE_PERIOD_DESCR) & FIELD_SEPARATOR_STR
            s = s & Int2Str(FRONT_MONTHS) & FIELD_SEPARATOR_STR
            s = s & Int2Str(FRONT_QUARTERS) & FIELD_SEPARATOR_STR
            s = s & Int2Str(FRONT_HALF_YEARS) & FIELD_SEPARATOR_STR
            s = s & Int2Str(FRONT_YEARS) & FIELD_SEPARATOR_STR
            s = s & Int2Str(FRONT_MAX_MONTHS) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(MC_0_1) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(MC_1_2) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(MC_0_1_2) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(QC_0_1) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 11 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            EXCHANGE_ROUTE_PERIOD_ID = Str2Int(ls(0))
            EXCHANGE_ROUTE_PERIOD_DESCR = ls(1)
            FRONT_MONTHS = Str2Int(ls(2))
            FRONT_QUARTERS = Str2Int(ls(3))
            FRONT_HALF_YEARS = Str2Int(ls(4))
            FRONT_YEARS = Str2Int(ls(5))
            FRONT_MAX_MONTHS = Str2Int(ls(6))
            MC_0_1 = Str2Bool(ls(7))
            MC_1_2 = Str2Bool(ls(8))
            MC_0_1_2 = Str2Bool(ls(9))
            QC_0_1 = Str2Bool(ls(10))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As EXCHANGE_ROUTE_PERIOD_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If EXCHANGE_ROUTE_PERIOD_ID <> q.EXCHANGE_ROUTE_PERIOD_ID Then Exit Function
        If EXCHANGE_ROUTE_PERIOD_DESCR <> q.EXCHANGE_ROUTE_PERIOD_DESCR Then Exit Function
        If FRONT_MONTHS <> q.FRONT_MONTHS Then Exit Function
        If FRONT_QUARTERS <> q.FRONT_QUARTERS Then Exit Function
        If FRONT_HALF_YEARS <> q.FRONT_HALF_YEARS Then Exit Function
        If FRONT_YEARS <> q.FRONT_YEARS Then Exit Function
        If FRONT_MAX_MONTHS <> q.FRONT_MAX_MONTHS Then Exit Function
        If MC_0_1 <> q.MC_0_1 Then Exit Function
        If MC_1_2 <> q.MC_1_2 Then Exit Function
        If MC_0_1_2 <> q.MC_0_1_2 Then Exit Function
        If QC_0_1 <> q.QC_0_1 Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As EXCHANGE_ROUTE_PERIOD_CLASS
        GetNewCopy = New EXCHANGE_ROUTE_PERIOD_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.EXCHANGE_ROUTE_PERIOD_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------

End Class