' This example was contributed by Sailsoft. Thanks!

' The example below shows a VB.Net aspx page for responding to ShareIt with a generated
' license key using Microsofts .Net Framework 2.0.
'
' It is compliant with the CGI keygenerator method ShareIt offers
'
' We use a dataset and a webservice in this example, but of course this is not necessary
' Yet it shows how to use the Request method to read individual fields from the form
' that ShareIt posts, and how to respond to ShareIt with the proper parameters

Option Strict Off


Public Class FFAOptCalcKeyLicense
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim LicenseDataFromLicenseProvider As String

        '<Insert code here if you like to check if the posted data originates from ShareIt>
        If Request.HttpMethod = "GET" Then
            Exit Sub
        End If
        Select Case Request.UserHostAddress
            Case "85.255.19.0" To "85.255.19.255"
                'its ok
            Case "127.0.0.1"
                'its ok
            Case "195.97.80.49"
                'its ok
            Case Else
                'Throw New HttpException(400, "Not Authorised")
        End Select

        'fetch and store the data received in a dataset 
        Dim ds As New System.Data.DataSet
        ds = FetchPostedData() 'see further on...

        'Call your webservice to create a new license and receive the result
        LicenseDataFromLicenseProvider = GenerateLicense(ds)
        Response.ContentType = "text/plain"
        Response.StatusCode = 200
        Response.Status = "200 OK"
        Response.Write(LicenseDataFromLicenseProvider)

    End Sub

    Friend Function CreateLicenseKey() As String
        Dim DB As New ArtBDataContext
        Dim rand As New Random
        Dim L As Integer = 65
        Dim U As Integer = 90
        Dim retval As String = String.Empty

        Dim ItsSafe As Boolean = False
        Do While ItsSafe = False
            retval = String.Empty
            For I = 1 To 15
                retval += Chr(rand.Next(L, U))
            Next
            Dim exists = (From q In DB.ARTBOPTCALC_LICENSES Where q.LICENSE_KEY = retval Select q).FirstOrDefault
            If IsNothing(exists) = True Then
                ItsSafe = True
            End If
        Loop
        DB.Dispose()
        Return retval
    End Function

    Friend Function GenerateLicense(ByVal ds As System.Data.DataSet) As String
        Dim DB As New ArtBDataContext
        Dim rnd As New Random

        'Absolute necessary Fields
        Dim PURCHASE_ID As String = String.Empty
        Dim PRODUCT_ID As String = String.Empty
        Dim REG_NAME As String = String.Empty
        Dim PURCHASE_DATE As Date
        Dim QUANTITY As Integer
        Dim FINGERPRINT As String = String.Empty

        'Optional Fields
        Dim COMPUTER_NAME As String = String.Empty
        Dim FIRSTNAME As String = String.Empty
        Dim LASTNAME As String = String.Empty
        Dim EMAIL As String = String.Empty
        Dim COMPANY As String = String.Empty
        Dim COUNTRY As String = String.Empty
        Dim retval As String = String.Empty

        '=======================================================================================
        'absolute necessary fields
        '=======================================================================================
        Try
            PURCHASE_ID = ds.Tables("SHAREIT").Rows(0).Item("PURCHASE_ID")
        Catch ex As Exception
            DB.Dispose()
            Throw New HttpException(400, "Field PURCHASE_ID empty or missing")
        End Try
        Try
            PRODUCT_ID = ds.Tables("SHAREIT").Rows(0).Item("PRODUCT_ID")
        Catch ex As Exception
            DB.Dispose()
            Throw New HttpException(400, "Field PRODUCT_ID empty or missing")
        End Try
        Try
            REG_NAME = ds.Tables("SHAREIT").Rows(0).Item("REG_NAME")
        Catch ex As Exception
            DB.Dispose()
            Throw New HttpException(400, "Field REG_NAME empty or missing")
        End Try
        Try
            PURCHASE_DATE = DateValue(ds.Tables("SHAREIT").Rows(0).Item("PURCHASE_DATE"))
        Catch ex As Exception
            DB.Dispose()
            Throw New HttpException(400, "Field PURCHASE_DATE empty or missing")
        End Try
        Try
            QUANTITY = ds.Tables("SHAREIT").Rows(0).Item("QUANTITY")
        Catch ex As Exception
            DB.Dispose()
            Throw New HttpException(400, "Field QUANTITY empty or missing")
        End Try
        Try
            'B8D8-F887-221B-C31D-3EC1-FC9B-E8C4-1C49
            FINGERPRINT = ds.Tables("SHAREIT").Rows(0).Item("ADDITIONAL1")
            If FINGERPRINT = "" Then
                DB.Dispose()
                Throw New HttpException(400, "Field ADDITIONAL1 empty")
            End If
        Catch ex As Exception
            Throw New HttpException(400, "Field ADDITIONAL1 empty or missing")
        End Try

        '=======================================================================================
        'optional fields
        '=======================================================================================
        Try
            COMPUTER_NAME = ds.Tables("SHAREIT").Rows(0).Item("ADDITIONAL2")
        Catch ex As Exception
        End Try
        Try
            FIRSTNAME = ds.Tables("SHAREIT").Rows(0).Item("FIRSTNAME")
        Catch ex As Exception
        End Try
        Try
            LASTNAME = ds.Tables("SHAREIT").Rows(0).Item("LASTNAME")
        Catch ex As Exception
        End Try
        Try
            EMAIL = ds.Tables("SHAREIT").Rows(0).Item("EMAIL")
        Catch ex As Exception
        End Try
        Try
            COMPANY = ds.Tables("SHAREIT").Rows(0).Item("COMPANY")
        Catch ex As Exception
        End Try
        Try
            COUNTRY = ds.Tables("SHAREIT").Rows(0).Item("COUNTRY")
        Catch ex As Exception
        End Try
        '=======================================================================================

        Dim trans As System.Data.Common.DbTransaction = Nothing
        Try
            DB.Connection.Open()
            trans = DB.Connection.BeginTransaction(IsolationLevel.Serializable)
            DB.Transaction = trans

            Dim License As ARTBOPTCALC_LICENSES = (From q In DB.ARTBOPTCALC_LICENSES
                                                   Join f In DB.ARTBOPTCALC_FINGERPRINTS On f.LICENSE_KEY Equals q.LICENSE_KEY _
                                                   Where f.FINGER_PRINT = FINGERPRINT And f.PRODUCT_ID = PRODUCT_ID _
                                                   Select q).FirstOrDefault
            If IsNothing(License) Then
                Throw New HttpException(400, "Invalid FP Key, License not found")
            End If

            License.PURCHASE_ID = PURCHASE_ID
            License.PRODUCT_ID = PRODUCT_ID
            License.DEMO = False
            License.LICENSE_PURCH_DATE = PURCHASE_DATE
            If License.DEMO = False And PURCHASE_DATE < License.LICENSE_EXP_DATE Then
                License.LICENSE_EXP_DATE = License.LICENSE_EXP_DATE.Value.AddMonths(12)
            Else
                License.LICENSE_EXP_DATE = PURCHASE_DATE.AddMonths(12)
            End If
            License.REG_NAME = REG_NAME
            License.FIRSTNAME = FIRSTNAME
            License.LASTNAME = LASTNAME
            License.COMPANY = COMPANY
            License.EMAIL = EMAIL
            License.COUNTRY = COUNTRY
            License.MAX_LICENSES = QUANTITY
            Dim cntr As Integer = License.USED_LICENSES
            If License.USED_LICENSES > QUANTITY Then
                License.USED_LICENSES = QUANTITY
                For Each r In License.ARTBOPTCALC_FINGERPRINTS
                    If cntr <= QUANTITY Then Exit For
                    r.ACTIVE = False
                    cntr -= 1
                Next
            Else
                License.USED_LICENSES = License.USED_LICENSES
            End If

            DB.SubmitChanges()
            trans.Commit()

            'retval = "LICENSE KEY: "
            retval += License.LICENSE_KEY(0)
            For I = 1 To License.LICENSE_KEY.Length - 1
                Dim m = I Mod 3
                If (m) = 0 Then
                    retval += "-" & License.LICENSE_KEY(I)
                Else
                    retval += License.LICENSE_KEY(I)
                End If
            Next
            'retval += " Valid for " & QUANTITY & " Users," & " Expires on: " & FormatDateTime(License.LICENSE_EXP_DATE, DateFormat.ShortDate)
        Catch ex As Exception
            trans.Rollback()
            DB.Dispose()
            Throw New HttpException(400, "Internal Server DB Error")
        End Try

        DB.Dispose()
        Return retval
    End Function

    '----------Function FetchPostedData------------------------------------------------------
    Friend Function CreatePurchaseDataset() As System.Data.DataSet
        Dim ds As New System.Data.DataSet
        Dim dt As New System.Data.DataTable("SHAREIT")
        dt.Columns.Add("PURCHASE_ID")
        dt.Columns.Add("RUNNING_NO")
        dt.Columns.Add("PURCHASE_DATE")
        dt.Columns.Add("PRODUCT_ID")
        dt.Columns.Add("LANGUAGE_ID")
        dt.Columns.Add("QUANTITY")
        dt.Columns.Add("REG_NAME")
        dt.Columns.Add("ADDITIONAL1")
        dt.Columns.Add("ADDITIONAL2")
        dt.Columns.Add("RESELLER")
        dt.Columns.Add("LASTNAME")
        dt.Columns.Add("FIRSTNAME")
        dt.Columns.Add("COMPANY")
        dt.Columns.Add("EMAIL")
        dt.Columns.Add("PHONE")
        dt.Columns.Add("FAX")
        dt.Columns.Add("STREET")
        dt.Columns.Add("ZIP")
        dt.Columns.Add("CITY")
        dt.Columns.Add("STATE")
        dt.Columns.Add("COUNTRY")
        ds.Tables.Add(dt)
        Return ds
    End Function
    'We save the data received from ShareIt in a dataset using the Request.Form(item) method
    Friend Function FetchPostedData() As System.Data.DataSet

        'make a dataset with one (purchase) table
        Dim ds As System.Data.DataSet = CreatePurchaseDataset()
        'create a new row for our table
        Dim r As System.Data.DataRow = ds.Tables("SHAREIT").NewRow()
        On Error Resume Next
        r.Item("PURCHASE_ID") = Request.Form("PURCHASE_ID")
        r.Item("RUNNING_NO") = Request.Form("RUNNING_NO")
        r.Item("PURCHASE_DATE") = Request.Form("PURCHASE_DATE")
        r.Item("PRODUCT_ID") = Request.Form("PRODUCT_ID")
        r.Item("LANGUAGE_ID") = Request.Form("LANGUAGE_ID")
        r.Item("QUANTITY") = Request.Form("QUANTITY")
        r.Item("REG_NAME") = Request.Form("REG_NAME")
        r.Item("ADDITIONAL1") = Request.Form("ADDITIONAL1")
        r.Item("ADDITIONAL2") = Request.Form("ADDITIONAL2")
        r.Item("RESELLER") = Request.Form("RESELLER")
        r.Item("LASTNAME") = Request.Form("LASTNAME")
        r.Item("FIRSTNAME") = Request.Form("FIRSTNAME")
        r.Item("COMPANY") = Request.Form("COMPANY")
        r.Item("EMAIL") = Request.Form("EMAIL")
        r.Item("PHONE") = Request.Form("PHONE")
        r.Item("FAX") = Request.Form("FAX")
        r.Item("STREET") = Request.Form("STREET")
        r.Item("ZIP") = Request.Form("ZIP")
        r.Item("CITY") = Request.Form("CITY")
        r.Item("STATE") = Request.Form("STATE")
        r.Item("COUNTRY") = Request.Form("COUNTRY")
        On Error GoTo 0
        'add the row to the table
        ds.Tables("SHAREIT").Rows.Add(r)
        Return ds
    End Function

    ' key generator exception class
    Public Class KeyGenException
        Inherits HttpException
        Public ERC As KeyGenReturnCode

        Public Sub New(ByVal message As String, ByVal e As KeyGenReturnCode)
            Me.HResult = 33
            ERC = e
        End Sub
    End Class
    Public Enum KeyGenReturnCode As Integer
        ' success
        ERC_SUCCESS = 0
        ERC_SUCCESS_BIN = 1
        ' failure
        ERC_ERROR = 10
        ERC_MEMORY = 11
        ERC_FILE_IO = 12
        ERC_BAD_ARGS = 13
        ERC_BAD_INPUT = 14
        ERC_EXPIRED = 15
        ERC_INTERNAL = 16
    End Enum
End Class