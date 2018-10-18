Public Class ACCOUNT_CLASS
    Inherits ACCOUNT

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            ACCOUNT_ID = q.ACCOUNT_ID
            FULL_NAME = q.FULL_NAME
            SHORT_NAME = q.SHORT_NAME
            STREET = q.STREET
            CITY = q.CITY
            REGION = q.REGION
            ZIPCODE = q.ZIPCODE
            COUNTRY_ID = q.COUNTRY_ID
            TEL_B1 = q.TEL_B1
            TEL_B2 = q.TEL_B2
            FAX_B = q.FAX_B
            EMAIL = q.EMAIL
            WEBSITE = q.WEBSITE
            ACCOUNT_TYPE_ID = q.ACCOUNT_TYPE_ID
            TRADE_AUTHORISED = q.TRADE_AUTHORISED
            DEFAULT_CCY = q.DEFAULT_CCY
            CAN_DELETE = q.CAN_DELETE
            BROKER_ID = q.BROKER_ID
            SUSPENDED = q.SUSPENDED
            DEFAULT_BROKER_CLIENT = q.DEFAULT_BROKER_CLIENT
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
            q.ACCOUNT_ID = ACCOUNT_ID
            q.FULL_NAME = FULL_NAME
            q.SHORT_NAME = SHORT_NAME
            q.STREET = STREET
            q.CITY = CITY
            q.REGION = REGION
            q.ZIPCODE = ZIPCODE
            q.COUNTRY_ID = COUNTRY_ID
            q.TEL_B1 = TEL_B1
            q.TEL_B2 = TEL_B2
            q.FAX_B = FAX_B
            q.EMAIL = EMAIL
            q.WEBSITE = WEBSITE
            q.ACCOUNT_TYPE_ID = ACCOUNT_TYPE_ID
            q.TRADE_AUTHORISED = TRADE_AUTHORISED
            q.DEFAULT_CCY = DEFAULT_CCY
            q.CAN_DELETE = CAN_DELETE
            q.BROKER_ID = BROKER_ID
            q.SUSPENDED = SUSPENDED
            q.DEFAULT_BROKER_CLIENT = DEFAULT_BROKER_CLIENT
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Try
            Dim l = From q In gdb.ACCOUNTs _
            Where q.ACCOUNT_ID = ACCOUNT_ID _
            Select q

            For Each q In l
                GetData = GetFromObject(q)
                Exit Function
            Next
        Catch ex As Exception
            Debug.Print(ex.ToString())
        End Try
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_ACCOUNT_ID As Integer) As Integer
        ACCOUNT_ID = a_ACCOUNT_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Try
            Dim q As New ACCOUNT
            SetToObject(q)

            gdb.ACCOUNTs.InsertOnSubmit(q)

            If submit = True Then
                gdb.SubmitChanges()
                ACCOUNT_ID = q.ACCOUNT_ID
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

            Dim l = From q In gdb.ACCOUNTs _
              Where q.ACCOUNT_ID = ACCOUNT_ID _
              Select q

            For Each q As ACCOUNT In l
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
            Dim l = From q In gdb.ACCOUNTs _
              Where q.ACCOUNT_ID = ACCOUNT_ID _
              Select q

            For Each q As ACCOUNT In l
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

            Dim l = From q In gdb.ACCOUNTs _
             Where q.ACCOUNT_ID = ACCOUNT_ID _
              Select q

            For Each q As ACCOUNT In l
                gdb.ACCOUNTs.DeleteOnSubmit(q)
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
            s = s & Int2Str(ACCOUNT_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(FULL_NAME) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(SHORT_NAME) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(STREET) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(CITY) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(REGION) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(ZIPCODE) & FIELD_SEPARATOR_STR
            s = s & Int2Str(COUNTRY_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(TEL_B1) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(TEL_B2) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(FAX_B) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(EMAIL) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(WEBSITE) & FIELD_SEPARATOR_STR
            s = s & Int2Str(ACCOUNT_TYPE_ID) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(TRADE_AUTHORISED) & FIELD_SEPARATOR_STR
            s = s & Int2Str(DEFAULT_CCY) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(CAN_DELETE) & FIELD_SEPARATOR_STR
            s = s & Int2Str(BROKER_ID) & FIELD_SEPARATOR_STR
            s = s & Bool2Str(SUSPENDED) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(DEFAULT_BROKER_CLIENT) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 20 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            ACCOUNT_ID = Str2Int(ls(0))
            FULL_NAME = ls(1)
            SHORT_NAME = ls(2)
            STREET = ls(3)
            CITY = ls(4)
            REGION = ls(5)
            ZIPCODE = ls(6)
            COUNTRY_ID = Str2Int(ls(7))
            TEL_B1 = ls(8)
            TEL_B2 = ls(9)
            FAX_B = ls(10)
            EMAIL = ls(11)
            WEBSITE = ls(12)
            ACCOUNT_TYPE_ID = Str2Int(ls(13))
            TRADE_AUTHORISED = Str2Bool(ls(14))
            DEFAULT_CCY = Str2Int(ls(15))
            CAN_DELETE = Str2Bool(ls(16))
            BROKER_ID = Str2Int(ls(17))
            SUSPENDED = Str2Bool(ls(18))
            DEFAULT_BROKER_CLIENT = Str2NullInt(ls(19))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As ACCOUNT_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If ACCOUNT_ID <> q.ACCOUNT_ID Then Exit Function
        If FULL_NAME <> q.FULL_NAME Then Exit Function
        If SHORT_NAME <> q.SHORT_NAME Then Exit Function
        If STREET <> q.STREET Then Exit Function
        If CITY <> q.CITY Then Exit Function
        If REGION <> q.REGION Then Exit Function
        If ZIPCODE <> q.ZIPCODE Then Exit Function
        If COUNTRY_ID <> q.COUNTRY_ID Then Exit Function
        If TEL_B1 <> q.TEL_B1 Then Exit Function
        If TEL_B2 <> q.TEL_B2 Then Exit Function
        If FAX_B <> q.FAX_B Then Exit Function
        If EMAIL <> q.EMAIL Then Exit Function
        If WEBSITE <> q.WEBSITE Then Exit Function
        If ACCOUNT_TYPE_ID <> q.ACCOUNT_TYPE_ID Then Exit Function
        If TRADE_AUTHORISED <> q.TRADE_AUTHORISED Then Exit Function
        If DEFAULT_CCY <> q.DEFAULT_CCY Then Exit Function
        If CAN_DELETE <> q.CAN_DELETE Then Exit Function
        If BROKER_ID <> q.BROKER_ID Then Exit Function
        If SUSPENDED <> q.SUSPENDED Then Exit Function
        If DEFAULT_BROKER_CLIENT <> q.DEFAULT_BROKER_CLIENT Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As ACCOUNT_CLASS
        GetNewCopy = New ACCOUNT_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.ACCOUNT_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------

    Public DESKS As New Collection

    Public Sub AddDesk(ByVal DESK As ACCOUNT_DESK_CLASS)
        DESKS.Add(DESK, DESK.ACCOUNT_DESK_ID.ToString())
    End Sub

End Class




