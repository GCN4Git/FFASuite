Public Class ORDERS_FFA_EXCHANGE_CLASS
    Inherits ORDERS_FFA_EXCHANGE

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            ORDER_ID = q.ORDER_ID
            EXCHANGE_ID = q.EXCHANGE_ID
            ACCOUNT_ID = q.ACCOUNT_ID
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
            q.ORDER_ID = ORDER_ID
            q.EXCHANGE_ID = EXCHANGE_ID
            q.ACCOUNT_ID = ACCOUNT_ID
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.ORDERS_FFA_EXCHANGEs _
        Where q.ORDER_ID = ORDER_ID And q.EXCHANGE_ID = EXCHANGE_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_ORDER_ID As Integer, ByVal a_EXCHANGE_ID As Integer) As Integer
        ORDER_ID = a_ORDER_ID
        EXCHANGE_ID = a_EXCHANGE_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New ORDERS_FFA_EXCHANGE
        SetToObject(q)

        gdb.ORDERS_FFA_EXCHANGEs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                ORDER_ID = q.ORDER_ID
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

        Dim l = From q In gdb.ORDERS_FFA_EXCHANGEs _
          Where q.ORDER_ID = ORDER_ID And q.EXCHANGE_ID = EXCHANGE_ID _
          Select q

        For Each q As ORDERS_FFA_EXCHANGE In l
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

        Dim l = From q In gdb.ORDERS_FFA_EXCHANGEs _
          Where q.ORDER_ID = ORDER_ID And q.EXCHANGE_ID = EXCHANGE_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As ORDERS_FFA_EXCHANGE In l
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

        Dim l = From q In gdb.ORDERS_FFA_EXCHANGEs _
         Where q.ORDER_ID = ORDER_ID And q.EXCHANGE_ID = EXCHANGE_ID _
          Select q

        For Each q As ORDERS_FFA_EXCHANGE In l
            gdb.ORDERS_FFA_EXCHANGEs.DeleteOnSubmit(q)
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
            s = s & Int2Str(ORDER_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(EXCHANGE_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ACCOUNT_ID) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 3 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            ORDER_ID = Str2Int(ls(0))
            EXCHANGE_ID = Str2Int(ls(1))
            ACCOUNT_ID = Str2Int(ls(2))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As ORDERS_FFA_EXCHANGE_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If ORDER_ID <> q.ORDER_ID Then Exit Function
        If EXCHANGE_ID <> q.EXCHANGE_ID Then Exit Function
        If ACCOUNT_ID <> q.ACCOUNT_ID Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As ORDERS_FFA_EXCHANGE_CLASS
        GetNewCopy = New ORDERS_FFA_EXCHANGE_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.ORDER_ID = 0
        GetNewCopy.EXCHANGE_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------

End Class