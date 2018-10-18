Public Class CONTACT_CLASS
    Inherits CONTACT

    Public Function GetFromObject(ByRef q As Object) As Integer
        If q Is Nothing Then
            GetFromObject = ArtBErrors.EmptyObject
            Exit Function
        End If

        GetFromObject = ArtBErrors.Success
        Try
            CONTACT_ID = q.CONTACT_ID
            FIRSTNAME = q.FIRSTNAME
            LASTNAME = q.LASTNAME
            TITLE = q.TITLE
            EMAIL1 = q.EMAIL1
            EMAIL2 = q.EMAIL2
            EMAIL3 = q.EMAIL3
            MSN = q.MSN
            YAHOO = q.YAHOO
            TEL_B1 = q.TEL_B1
            TEL_B2 = q.TEL_B2
            TEL_MBL1 = q.TEL_MBL1
            TEL_MBL2 = q.TEL_MBL2
            TEL_H1 = q.TEL_H1
            FAX_B = q.FAX_B
            FAX_H = q.FAX_H
            CONTACT_TYPE_ID = q.CONTACT_TYPE_ID
            STREET = q.STREET
            CITY = q.CITY
            REGION = q.REGION
            ZIPCODE = q.ZIPCODE
            COUNTRY_ID = q.COUNTRY_ID
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
            q.CONTACT_ID = CONTACT_ID
            q.FIRSTNAME = FIRSTNAME
            q.LASTNAME = LASTNAME
            q.TITLE = TITLE
            q.EMAIL1 = EMAIL1
            q.EMAIL2 = EMAIL2
            q.EMAIL3 = EMAIL3
            q.MSN = MSN
            q.YAHOO = YAHOO
            q.TEL_B1 = TEL_B1
            q.TEL_B2 = TEL_B2
            q.TEL_MBL1 = TEL_MBL1
            q.TEL_MBL2 = TEL_MBL2
            q.TEL_H1 = TEL_H1
            q.FAX_B = FAX_B
            q.FAX_H = FAX_H
            q.CONTACT_TYPE_ID = CONTACT_TYPE_ID
            q.STREET = STREET
            q.CITY = CITY
            q.REGION = REGION
            q.ZIPCODE = ZIPCODE
            q.COUNTRY_ID = COUNTRY_ID
        Catch e As Exception
            SetToObject = ArtBErrors.FieldConversionFailed
            Debug.Print(e.ToString())
        End Try
    End Function

    Public Function GetData(ByRef gdb As DB_ARTB_NETDataContext) As Integer
        GetData = ArtBErrors.RecordNotFound
        Dim l = From q In gdb.CONTACTs _
        Where q.CONTACT_ID = CONTACT_ID _
        Select q

        For Each q In l
            GetData = GetFromObject(q)
            Exit Function
        Next
    End Function

    Public Function GetFromID(ByRef gdb As DB_ARTB_NETDataContext, ByVal a_CONTACT_ID As Integer) As Integer
        CONTACT_ID = a_CONTACT_ID
        GetFromID = GetData(gdb)
    End Function

    Public Function Insert(ByRef gdb As DB_ARTB_NETDataContext, Optional ByVal submit As Boolean = False) As Integer
        Dim q As New CONTACT
        SetToObject(q)

        gdb.CONTACTs.InsertOnSubmit(q)

        If submit = True Then
            Try
                gdb.SubmitChanges()
                CONTACT_ID = q.CONTACT_ID
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

        Dim l = From q In gdb.CONTACTs _
          Where q.CONTACT_ID = CONTACT_ID _
          Select q

        For Each q As CONTACT In l
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

        Dim l = From q In gdb.CONTACTs _
          Where q.CONTACT_ID = CONTACT_ID _
          Select q

        If l.Count < 1 Then
            InsertUpdate = Insert(gdb, submit)
            Exit Function
        End If
        For Each q As CONTACT In l
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

        Dim l = From q In gdb.CONTACTs _
         Where q.CONTACT_ID = CONTACT_ID _
          Select q

        For Each q As CONTACT In l
            gdb.CONTACTs.DeleteOnSubmit(q)
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
            s = s & Int2Str(CONTACT_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(FIRSTNAME) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(LASTNAME) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(TITLE) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(EMAIL1) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(EMAIL2) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(EMAIL3) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(MSN) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(YAHOO) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(TEL_B1) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(TEL_B2) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(TEL_MBL1) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(TEL_MBL2) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(TEL_H1) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(FAX_B) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(FAX_H) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(CONTACT_TYPE_ID) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(STREET) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(CITY) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(REGION) & FIELD_SEPARATOR_STR
            s = s & Str2QuotedStr(ZIPCODE) & FIELD_SEPARATOR_STR
            s = s & NullInt2Str(COUNTRY_ID) & RECORD_SEPARATOR_STR
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
        If ls.Count() <> 22 Then
            GetFromStr = ArtBErrors.InvalidnumberOfFields
            Exit Function
        End If

        Try
            CONTACT_ID = Str2Int(ls(0))
            FIRSTNAME = ls(1)
            LASTNAME = ls(2)
            TITLE = ls(3)
            EMAIL1 = ls(4)
            EMAIL2 = ls(5)
            EMAIL3 = ls(6)
            MSN = ls(7)
            YAHOO = ls(8)
            TEL_B1 = ls(9)
            TEL_B2 = ls(10)
            TEL_MBL1 = ls(11)
            TEL_MBL2 = ls(12)
            TEL_H1 = ls(13)
            FAX_B = ls(14)
            FAX_H = ls(15)
            CONTACT_TYPE_ID = Str2NullInt(ls(16))
            STREET = ls(17)
            CITY = ls(18)
            REGION = ls(19)
            ZIPCODE = ls(20)
            COUNTRY_ID = Str2NullInt(ls(21))
        Catch e As Exception
            Debug.Print(e.ToString())
            Return ArtBErrors.ConversionFromStrError
        End Try
        GetFromStr = ArtBErrors.Success
    End Function

    Public Function Equal(ByRef q As CONTACT_CLASS) As Boolean
        Equal = False
        If q Is Nothing Then Exit Function
        If CONTACT_ID <> q.CONTACT_ID Then Exit Function
        If FIRSTNAME <> q.FIRSTNAME Then Exit Function
        If LASTNAME <> q.LASTNAME Then Exit Function
        If TITLE <> q.TITLE Then Exit Function
        If EMAIL1 <> q.EMAIL1 Then Exit Function
        If EMAIL2 <> q.EMAIL2 Then Exit Function
        If EMAIL3 <> q.EMAIL3 Then Exit Function
        If MSN <> q.MSN Then Exit Function
        If YAHOO <> q.YAHOO Then Exit Function
        If TEL_B1 <> q.TEL_B1 Then Exit Function
        If TEL_B2 <> q.TEL_B2 Then Exit Function
        If TEL_MBL1 <> q.TEL_MBL1 Then Exit Function
        If TEL_MBL2 <> q.TEL_MBL2 Then Exit Function
        If TEL_H1 <> q.TEL_H1 Then Exit Function
        If FAX_B <> q.FAX_B Then Exit Function
        If FAX_H <> q.FAX_H Then Exit Function
        If CONTACT_TYPE_ID <> q.CONTACT_TYPE_ID Then Exit Function
        If STREET <> q.STREET Then Exit Function
        If CITY <> q.CITY Then Exit Function
        If REGION <> q.REGION Then Exit Function
        If ZIPCODE <> q.ZIPCODE Then Exit Function
        If COUNTRY_ID <> q.COUNTRY_ID Then Exit Function
        Equal = True
    End Function

    Public Function GetNewCopy() As CONTACT_CLASS
        GetNewCopy = New CONTACT_CLASS
        GetNewCopy.GetFromObject(Me)
        GetNewCopy.CONTACT_ID = 0
    End Function

    '-----------------------------------------------------------------------------------------------

End Class