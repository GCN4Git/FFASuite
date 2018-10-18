Public Class SERVER_INFO_CLASS
    Inherits SERVER_INFO

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            SERVER_ID = q.SERVER_ID
            SERVER_ACTIVE = q.SERVER_ACTIVE
            LAST_CONNECTION_TIME = q.LAST_CONNECTION_TIME
            SERVICE_ON = q.SERVICE_ON
            SERVICE_OF_CONNECTED = q.SERVICE_OF_CONNECTED
            SERVICE_START_HOUR = q.SERVICE_START_HOUR
            SERVICE_END_HOUR = q.SERVICE_END_HOUR
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
            q.SERVER_ID = SERVER_ID
            q.SERVER_ACTIVE = SERVER_ACTIVE
            q.LAST_CONNECTION_TIME = LAST_CONNECTION_TIME
            q.SERVICE_ON = SERVICE_ON
            q.SERVICE_OF_CONNECTED = SERVICE_OF_CONNECTED
            q.SERVICE_START_HOUR = SERVICE_START_HOUR
            q.SERVICE_END_HOUR = SERVICE_END_HOUR
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.SERVER_INFOs _
        Where q.SERVER_ID = SERVER_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_SERVER_ID As Integer) As Integer
        SERVER_ID = a_SERVER_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New SERVER_INFO
        SetToObject(q)

        gdb.SERVER_INFOs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                SERVER_ID = q.SERVER_ID
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

        Dim l = From q In gdb.SERVER_INFOs _
          Where q.SERVER_ID = SERVER_ID _
          Select q

        For Each q As SERVER_INFO In l
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

        Dim l = From q In gdb.SERVER_INFOs _
          Where q.SERVER_ID = SERVER_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As SERVER_INFO In l
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

        Dim l = From q In gdb.SERVER_INFOs _
         Where q.SERVER_ID = SERVER_ID _
          Select q

        For Each q As SERVER_INFO In l
            gdb.SERVER_INFOs.DeleteOnSubmit(q)
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
            s = s & Int2Str(SERVER_ID) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(SERVER_ACTIVE) & FIELD_SEPARATOR_STR
            s = s & NullDateTime2Str(LAST_CONNECTION_TIME) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(SERVICE_ON) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(SERVICE_OF_CONNECTED) & FIELD_SEPARATOR_STR
            s = s & Int2Str(SERVICE_START_HOUR) & FIELD_SEPARATOR_STR
            s = s & Int2Str(SERVICE_END_HOUR) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 5 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            SERVER_ID = Str2Int(ls(0))
            SERVER_ACTIVE = Str2Bool(ls(1))
            LAST_CONNECTION_TIME = Str2NullDateTime(ls(2))
            SERVICE_ON = Str2Bool(ls(3))
            SERVICE_OF_CONNECTED = Str2Bool(ls(4))
            SERVICE_START_HOUR = Str2Int(ls(5))
            SERVICE_END_HOUR = Str2Int(ls(6))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As SERVER_INFO_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If SERVER_ID <> q.SERVER_ID Then Exit Function
        If SERVER_ACTIVE <> q.SERVER_ACTIVE Then Exit Function
        If LAST_CONNECTION_TIME <> q.LAST_CONNECTION_TIME Then Exit Function
        If SERVICE_ON <> q.SERVICE_ON Then Exit Function
        If SERVICE_OF_CONNECTED <> q.SERVICE_OF_CONNECTED Then Exit Function
        If SERVICE_START_HOUR <> q.SERVICE_START_HOUR Then Exit Function
        If SERVICE_END_HOUR <> q.SERVICE_END_HOUR Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As SERVER_INFO_CLASS
        GetNewCopy = New SERVER_INFO_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.SERVER_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------

End Class