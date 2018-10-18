Public Class DESK_EXCHANGE_CLASS
    Inherits DESK_EXCHANGE

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            ACCOUNT_DESK_ID = q.ACCOUNT_DESK_ID
            EXCHANGE_ID = q.EXCHANGE_ID
            TRADE_CLASS_SHORT = q.TRADE_CLASS_SHORT
            ACTIVE = q.ACTIVE
            ORDER_INCLUDE = q.ORDER_INCLUDE
            RANKING = q.RANKING
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
            q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID
            q.EXCHANGE_ID = EXCHANGE_ID
            q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT
            q.ACTIVE = ACTIVE
            q.ORDER_INCLUDE = ORDER_INCLUDE
            q.RANKING = RANKING
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.DESK_EXCHANGEs _
        Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID And q.EXCHANGE_ID = EXCHANGE_ID And q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_ACCOUNT_DESK_ID As Integer, ByVal a_EXCHANGE_ID As Integer, ByVal a_TRADE_CLASS_SHORT As String) As Integer
        ACCOUNT_DESK_ID = a_ACCOUNT_DESK_ID
        EXCHANGE_ID = a_EXCHANGE_ID
        TRADE_CLASS_SHORT = a_TRADE_CLASS_SHORT
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New DESK_EXCHANGE
        SetToObject(q)

        gdb.DESK_EXCHANGEs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                ACCOUNT_DESK_ID = q.ACCOUNT_DESK_ID
                EXCHANGE_ID = q.EXCHANGE_ID
                TRADE_CLASS_SHORT = q.TRADE_CLASS_SHORT
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

        Dim l = From q In gdb.DESK_EXCHANGEs _
          Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID And q.EXCHANGE_ID = EXCHANGE_ID And q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT _
          Select q

        For Each q As DESK_EXCHANGE In l
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

        Dim l = From q In gdb.DESK_EXCHANGEs _
          Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID And q.EXCHANGE_ID = EXCHANGE_ID And q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As DESK_EXCHANGE In l
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

        Dim l = From q In gdb.DESK_EXCHANGEs _
         Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID And q.EXCHANGE_ID = EXCHANGE_ID And q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT _
          Select q

        For Each q As DESK_EXCHANGE In l
            gdb.DESK_EXCHANGEs.DeleteOnSubmit(q)
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
            s = s & Int2Str(ACCOUNT_DESK_ID) & FIELD_SEPARATOR_STR
            s = s & Int2Str(EXCHANGE_ID) & FIELD_SEPARATOR_STR
            s = s & TRADE_CLASS_SHORT & FIELD_SEPARATOR_STR
            s = s & Bool2Str(ACTIVE) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(ORDER_INCLUDE) & FIELD_SEPARATOR_STR
            s = s & Int2Str(RANKING) & RECORD_SEPARATOR_STR
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
            ACCOUNT_DESK_ID = Str2Int(ls(0))
            EXCHANGE_ID = Str2Int(ls(1))
            TRADE_CLASS_SHORT = Str2Char(ls(2))
            ACTIVE = Str2Bool(ls(3))
            ORDER_INCLUDE = Str2Bool(ls(4))
            RANKING = Str2Int(ls(5))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As DESK_EXCHANGE_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If ACCOUNT_DESK_ID <> q.ACCOUNT_DESK_ID Then Exit Function
        If EXCHANGE_ID <> q.EXCHANGE_ID Then Exit Function
        If TRADE_CLASS_SHORT <> q.TRADE_CLASS_SHORT Then Exit Function
        If ACTIVE <> q.ACTIVE Then Exit Function
        If ORDER_INCLUDE <> q.ORDER_INCLUDE Then Exit Function
        If RANKING <> q.RANKING Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As DESK_EXCHANGE_CLASS
        GetNewCopy = New DESK_EXCHANGE_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.ACCOUNT_DESK_ID = 0
        GetNewCopy.EXCHANGE_ID = 0
        GetNewCopy.TRADE_CLASS_SHORT = ""
    End Function

    '-----------------------------------------------------------------------------------------------
    Public CLEARERS As New Collection
End Class


