Public Class TRADE_CLASS_RATIO_SPREAD_CLASS
    Inherits TRADE_CLASS_RATIO_SPREAD

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            TRADE_CLASS_SHORT = q.TRADE_CLASS_SHORT
            ROUTE_ID1 = q.ROUTE_ID1
            ROUTE_ID2 = q.ROUTE_ID2
            PRICING_TICK = q.PRICING_TICK
            PRECISION_TICK = q.PRECISION_TICK
            DEFAULT_MULTIPLIER = q.DEFAULT_MULTIPLIER
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
            q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT
            q.ROUTE_ID1 = ROUTE_ID1
            q.ROUTE_ID2 = ROUTE_ID2
            q.PRICING_TICK = PRICING_TICK
            q.PRECISION_TICK = PRECISION_TICK
            q.DEFAULT_MULTIPLIER = DEFAULT_MULTIPLIER
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.TRADE_CLASS_RATIO_SPREADs _
        Where q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT And q.ROUTE_ID1 = ROUTE_ID1 And q.ROUTE_ID2 = ROUTE_ID2 _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_TRADE_CLASS_SHORT As String, ByVal a_ROUTE_ID1 As Integer, ByVal a_ROUTE_ID2 As Integer) As Integer
        TRADE_CLASS_SHORT = a_TRADE_CLASS_SHORT
        ROUTE_ID1 = a_ROUTE_ID1
        ROUTE_ID2 = a_ROUTE_ID2
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New TRADE_CLASS_RATIO_SPREAD
        SetToObject(q)

        gdb.TRADE_CLASS_RATIO_SPREADs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                TRADE_CLASS_SHORT = q.TRADE_CLASS_SHORT
                ROUTE_ID1 = q.ROUTE_ID1
                ROUTE_ID2 = q.ROUTE_ID2
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

        Dim l = From q In gdb.TRADE_CLASS_RATIO_SPREADs _
          Where q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT And q.ROUTE_ID1 = ROUTE_ID1 And q.ROUTE_ID2 = ROUTE_ID2 _
          Select q

        For Each q As TRADE_CLASS_RATIO_SPREAD In l
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

        Dim l = From q In gdb.TRADE_CLASS_RATIO_SPREADs _
          Where q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT And q.ROUTE_ID1 = ROUTE_ID1 And q.ROUTE_ID2 = ROUTE_ID2 _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As TRADE_CLASS_RATIO_SPREAD In l
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

        Dim l = From q In gdb.TRADE_CLASS_RATIO_SPREADs _
         Where q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT And q.ROUTE_ID1 = ROUTE_ID1 And q.ROUTE_ID2 = ROUTE_ID2 _
          Select q

        For Each q As TRADE_CLASS_RATIO_SPREAD In l
            gdb.TRADE_CLASS_RATIO_SPREADs.DeleteOnSubmit(q)
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
            s = s & TRADE_CLASS_SHORT & FIELD_SEPARATOR_STR
            s = s & Int2Str(ROUTE_ID1) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ROUTE_ID2) & FIELD_SEPARATOR_STR
            s = s & Double2Str(PRICING_TICK) & FIELD_SEPARATOR_STR
            s = s & Double2Str(PRECISION_TICK) & FIELD_SEPARATOR_STR
            s = s & Int2Str(DEFAULT_MULTIPLIER) & RECORD_SEPARATOR_STR
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
            TRADE_CLASS_SHORT = Str2Char(ls(0))
            ROUTE_ID1 = Str2Int(ls(1))
            ROUTE_ID2 = Str2Int(ls(2))
            PRICING_TICK = Str2Double(ls(3))
            PRECISION_TICK = Str2Double(ls(4))
            DEFAULT_MULTIPLIER = Str2Int(ls(5))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As TRADE_CLASS_RATIO_SPREAD_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If TRADE_CLASS_SHORT <> q.TRADE_CLASS_SHORT Then Exit Function
        If ROUTE_ID1 <> q.ROUTE_ID1 Then Exit Function
        If ROUTE_ID2 <> q.ROUTE_ID2 Then Exit Function
        If PRICING_TICK <> q.PRICING_TICK Then Exit Function
        If PRECISION_TICK <> q.PRECISION_TICK Then Exit Function
        If DEFAULT_MULTIPLIER <> q.DEFAULT_MULTIPLIER Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As TRADE_CLASS_RATIO_SPREAD_CLASS
        GetNewCopy = New TRADE_CLASS_RATIO_SPREAD_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.TRADE_CLASS_SHORT = ""
        GetNewCopy.ROUTE_ID1 = 0
        GetNewCopy.ROUTE_ID2 = 0
    End Function

    '-----------------------------------------------------------------------------------------------

End Class