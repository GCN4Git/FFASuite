Public Class DESK_TRADE_CLASS_CLASS
    Inherits DESK_TRADE_CLASS

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            ACCOUNT_DESK_ID = q.ACCOUNT_DESK_ID
            TRADE_CLASS_SHORT = q.TRADE_CLASS_SHORT
            ACTIVE = q.ACTIVE
            BROKER_ID = q.BROKER_ID
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
            q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT
            q.ACTIVE = ACTIVE
            q.BROKER_ID = BROKER_ID
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Try
            Dim l = From q In gdb.DESK_TRADE_CLASSES _
            Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID And q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT _
            Select q

            For Each q In l
                GetData = GetFromObject(q)
                Exit Function
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_ACCOUNT_DESK_ID As Integer, ByVal a_TRADE_CLASS_SHORT As String) As Integer
        ACCOUNT_DESK_ID = a_ACCOUNT_DESK_ID
        TRADE_CLASS_SHORT = a_TRADE_CLASS_SHORT
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Try
            Dim q As New DESK_TRADE_CLASS
            SetToObject(q)

            gdb.DESK_TRADE_CLASSes.InsertOnSubmit(q)

            If submit = True Then
                gdb.SubmitChanges()
                ACCOUNT_DESK_ID = q.ACCOUNT_DESK_ID
                TRADE_CLASS_SHORT = q.TRADE_CLASS_SHORT
            End If
            Insert = ArtBErrors.Success
        Catch e As Exception
            Insert = ArtBErrors.InsertFailed
            Debug.Print(e.ToString())
            Exit Function
        End Try
    End Function

    Public Function Update(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Update = ArtBErrors.Success
        Try

            Dim l = From q In gdb.DESK_TRADE_CLASSES _
              Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID And q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT _
              Select q

            For Each q As DESK_TRADE_CLASS In l
                Update = SetToObject(q)
                If Update <> ArtBErrors.Success Then
                    Exit Function
                End If
            Next

            If submit = True Then
                gdb.SubmitChanges()
            End If
        Catch e As Exception
            Update = ArtBErrors.UpdateFailed
            Debug.Print(e.ToString())
            Exit Function
        End Try
    End Function

    Public Function InsertUpdate(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        InsertUpdate = ArtBErrors.Success

        Try
            Dim l = From q In gdb.DESK_TRADE_CLASSES _
              Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID And q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT _
              Select q

            For Each q As DESK_TRADE_CLASS In l
                InsertUpdate = SetToObject(q)
                If InsertUpdate <> ArtBErrors.Success Then
                    Exit Function
                End If
                If submit = True Then
                    gdb.SubmitChanges()
                End If
                Exit Function
            Next

            InsertUpdate = Insert(gdb, submit)

            If submit = True Then
                gdb.SubmitChanges()
            End If
        Catch e As Exception
            Debug.Print(e.ToString())
            InsertUpdate = ArtBErrors.UpdateFailed
            Exit Function
        End Try
    End Function

    Public Function Delete(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Delete = ArtBErrors.Success
        Try

            Dim l = From q In gdb.DESK_TRADE_CLASSES _
             Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID And q.TRADE_CLASS_SHORT = TRADE_CLASS_SHORT _
              Select q

            For Each q As DESK_TRADE_CLASS In l
                gdb.DESK_TRADE_CLASSes.DeleteOnSubmit(q)
            Next

            If submit = True Then
                gdb.SubmitChanges()
            End If
        Catch e As Exception
            Debug.Print(e.ToString())
            Delete = ArtBErrors.DeleteFailed
            Exit Function
        End Try
    End Function

    Public Function AppendToStr(ByRef DestinationStr As String) As Integer
        Dim s As String = ""
        Try
            s = s & Int2Str(ACCOUNT_DESK_ID) & FIELD_SEPARATOR_STR
            s = s & TRADE_CLASS_SHORT & FIELD_SEPARATOR_STR
            s = s & Bool2Str(ACTIVE) & FIELD_SEPARATOR_STR
            s = s & Int2Str(BROKER_ID) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 4 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            ACCOUNT_DESK_ID = Str2Int(ls(0))
            TRADE_CLASS_SHORT = Str2Char(ls(1))
            ACTIVE = Str2Bool(ls(2))
            BROKER_ID = Str2Int(ls(3))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As DESK_TRADE_CLASS_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If ACCOUNT_DESK_ID <> q.ACCOUNT_DESK_ID Then Exit Function
        If TRADE_CLASS_SHORT <> q.TRADE_CLASS_SHORT Then Exit Function
        If ACTIVE <> q.ACTIVE Then Exit Function
        If BROKER_ID <> q.BROKER_ID Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As DESK_TRADE_CLASS_CLASS
        GetNewCopy = New DESK_TRADE_CLASS_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.ACCOUNT_DESK_ID = 0
        GetNewCopy.TRADE_CLASS_SHORT = ""
    End Function

    '-----------------------------------------------------------------------------------------------
    Public EXCHANGES As New Collection
End Class