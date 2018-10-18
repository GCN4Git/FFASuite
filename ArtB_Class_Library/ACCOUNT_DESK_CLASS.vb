Public Class ACCOUNT_DESK_CLASS
    Inherits ACCOUNT_DESK

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            ACCOUNT_DESK_ID = q.ACCOUNT_DESK_ID
            ACCOUNT_ID = q.ACCOUNT_ID
            DESK_DESCR = q.DESK_DESCR
            DESK_QUALIFIER = q.DESK_QUALIFIER
            DESK_ACTIVE = q.DESK_ACTIVE
            SEE_OTHER_DESKS = q.SEE_OTHER_DESKS
            AMMEND_OTHER_DESKS = q.AMMEND_OTHER_DESKS
            CLEARED_SHOW_NAME = q.CLEARED_SHOW_NAME
            OTC_SHOW_NAME = q.OTC_SHOW_NAME
            SUSPENDED = q.SUSPENDED
            LOST_CONNECTION = q.LOST_CONNECTION
            WAIT_CONNECTION = q.WAIT_CONNECTION
            DEFAULT_NUKE = q.DEFAULT_NUKE
            DEFAULT_SEE = q.DEFAULT_SEE
            DEFAULT_BI = q.DEFAULT_BI
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
            q.ACCOUNT_ID = ACCOUNT_ID
            q.DESK_DESCR = DESK_DESCR
            q.DESK_QUALIFIER = DESK_QUALIFIER
            q.DESK_ACTIVE = DESK_ACTIVE
            q.SEE_OTHER_DESKS = SEE_OTHER_DESKS
            q.AMMEND_OTHER_DESKS = AMMEND_OTHER_DESKS
            q.CLEARED_SHOW_NAME = CLEARED_SHOW_NAME
            q.OTC_SHOW_NAME = OTC_SHOW_NAME
            q.SUSPENDED = SUSPENDED
            q.LOST_CONNECTION = LOST_CONNECTION
            q.WAIT_CONNECTION = WAIT_CONNECTION
            q.DEFAULT_NUKE = DEFAULT_NUKE
            q.DEFAULT_SEE = DEFAULT_SEE
            q.DEFAULT_BI = DEFAULT_BI
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Try
            Dim l = From q In gdb.ACCOUNT_DESKs _
            Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID _
            Select q

            For Each q In l
                GetData = GetFromObject(q)
                Exit Function
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_ACCOUNT_DESK_ID As Integer) As Integer
        ACCOUNT_DESK_ID = a_ACCOUNT_DESK_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Try
            Dim q As New ACCOUNT_DESK
            SetToObject(q)

            gdb.ACCOUNT_DESKs.InsertOnSubmit(q)

            If submit = True Then
                gdb.SubmitChanges()
                ACCOUNT_DESK_ID = q.ACCOUNT_DESK_ID
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

            Dim l = From q In gdb.ACCOUNT_DESKs _
              Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID _
              Select q

            For Each q As ACCOUNT_DESK In l
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
            Dim l = From q In gdb.ACCOUNT_DESKs _
              Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID _
              Select q

            For Each q As ACCOUNT_DESK In l
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

            Dim l = From q In gdb.ACCOUNT_DESKs _
             Where q.ACCOUNT_DESK_ID = ACCOUNT_DESK_ID _
              Select q

            For Each q As ACCOUNT_DESK In l
                gdb.ACCOUNT_DESKs.DeleteOnSubmit(q)
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
            s = s & Int2Str(ACCOUNT_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(DESK_DESCR) & FIELD_SEPARATOR_STR
            s = s & Short2Str(DESK_QUALIFIER) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(DESK_ACTIVE) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(SEE_OTHER_DESKS) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(AMMEND_OTHER_DESKS) & FIELD_SEPARATOR_STR
            s = s & Short2Str(CLEARED_SHOW_NAME) & FIELD_SEPARATOR_STR
            s = s & Short2Str(OTC_SHOW_NAME) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(SUSPENDED) & FIELD_SEPARATOR_STR
            s = s & Short2Str(LOST_CONNECTION) & FIELD_SEPARATOR_STR
            s = s & Short2Str(WAIT_CONNECTION) & FIELD_SEPARATOR_STR
            s = s & Short2Str(DEFAULT_NUKE) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(DEFAULT_SEE) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(DEFAULT_BI) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 15 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            ACCOUNT_DESK_ID = Str2Int(ls(0))
            ACCOUNT_ID = Str2Int(ls(1))
            DESK_DESCR = ls(2)
            DESK_QUALIFIER = Str2Short(ls(3))
            DESK_ACTIVE = Str2Bool(ls(4))
            SEE_OTHER_DESKS = Str2Bool(ls(5))
            AMMEND_OTHER_DESKS = Str2Bool(ls(6))
            CLEARED_SHOW_NAME = Str2Short(ls(7))
            OTC_SHOW_NAME = Str2Short(ls(8))
            SUSPENDED = Str2Bool(ls(9))
            LOST_CONNECTION = Str2Short(ls(10))
            WAIT_CONNECTION = Str2Short(ls(11))
            DEFAULT_NUKE = Str2Short(ls(12))
            DEFAULT_SEE = Str2Bool(ls(13))
            DEFAULT_BI = Str2Bool(ls(14))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As ACCOUNT_DESK_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If ACCOUNT_DESK_ID <> q.ACCOUNT_DESK_ID Then Exit Function
        If ACCOUNT_ID <> q.ACCOUNT_ID Then Exit Function
        If DESK_DESCR <> q.DESK_DESCR Then Exit Function
        If DESK_QUALIFIER <> q.DESK_QUALIFIER Then Exit Function
        If DESK_ACTIVE <> q.DESK_ACTIVE Then Exit Function
        If SEE_OTHER_DESKS <> q.SEE_OTHER_DESKS Then Exit Function
        If AMMEND_OTHER_DESKS <> q.AMMEND_OTHER_DESKS Then Exit Function
        If CLEARED_SHOW_NAME <> q.CLEARED_SHOW_NAME Then Exit Function
        If OTC_SHOW_NAME <> q.OTC_SHOW_NAME Then Exit Function
        If SUSPENDED <> q.SUSPENDED Then Exit Function
        If LOST_CONNECTION <> q.LOST_CONNECTION Then Exit Function
        If WAIT_CONNECTION <> q.WAIT_CONNECTION Then Exit Function
        If DEFAULT_NUKE <> q.DEFAULT_NUKE Then Exit Function
        If DEFAULT_SEE <> q.DEFAULT_SEE Then Exit Function
        If DEFAULT_BI <> q.DEFAULT_BI Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As ACCOUNT_DESK_CLASS
        GetNewCopy = New ACCOUNT_DESK_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.ACCOUNT_DESK_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------
    Public TRADERS As New Collection
    Public TRADE_CLASSES As New Collection
    Public COUNTER_PARTY_LIMITS As New Collection

    Public Sub AddDeskTrader(ByVal TRADER As DESK_TRADER_CLASS)
        TRADERS.Add(TRADER, TRADER.OF_ID.ToString())
    End Sub

    Public LoggedIn As Boolean = False
End Class


