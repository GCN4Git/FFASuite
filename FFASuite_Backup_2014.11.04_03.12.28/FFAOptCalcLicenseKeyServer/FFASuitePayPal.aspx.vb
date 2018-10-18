Option Strict Off
Imports System
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Net.Mime
Imports System.Threading
Imports System.ComponentModel


Public Class FFASuitePayPal
    Inherits System.Web.UI.Page

    Private Shared LockWeb As New Object

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'example taken from: http://www.codeproject.com/Tips/84538/Setting-up-PayPal-Instant-Payment-Notification-IPN
        '++++ NOTE: to send email asynchronously you ned to set the Async="true" on the page aspx
        '<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PayPalVerify.aspx.vb" Inherits="FFASuitePayPal.PayPalVerify" Async="true" %>
        '++++

        If Request.HttpMethod = "GET" Then
            'SendTestEmail()
            Exit Sub
        End If

        'Post back to either sandbox or live
        Dim strPayPalServer As String
#If DEBUG Then
        strPayPalServer = "https://www.sandbox.paypal.com/cgi-bin/webscr"
#Else
        strPayPalServer = "https://www.paypal.com/cgi-bin/webscr"
#End If

        Dim IPN As PayPalIPNClass
        Dim req As HttpWebRequest = CType(WebRequest.Create(strPayPalServer), HttpWebRequest)

        'Set values for the request back
        req.Method = "POST"
        req.ContentType = "application/x-www-form-urlencoded"
        Dim param As Byte() = Request.BinaryRead(HttpContext.Current.Request.ContentLength)
        Dim strRequest As String = Encoding.ASCII.GetString(param)
        Dim strRequestCopy As String = strRequest
        Debug.Print(strRequest)
        strRequest += "&cmd=_notify-validate"
        req.ContentLength = strRequest.Length

        'Send the request to PayPal and get the response
        Dim streamOut As New StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII)
        streamOut.Write(strRequest)
        streamOut.Close()
        Dim streamIn As New StreamReader(req.GetResponse().GetResponseStream())
        Dim strResponse As String = streamIn.ReadToEnd()
        streamIn.Close()

        Dim DB As New ArtBDataContext
        If strResponse = "VERIFIED" Then
            IPN = New PayPalIPNClass(strRequestCopy)

            'process payment
            Dim trans As System.Data.Common.DbTransaction = Nothing
            Try
                DB.Connection.Open()

                'check that txn_id has not been previously processed
                SyncLock LockWeb
                    Dim OldStuff = (From q In DB.ARTTBOPTCALC_PAYPAL_IPN _
                                    Where q.ipn_track_id = IPN.ipn_track_id _
                                    Select q).FirstOrDefault
                    If IsNothing(OldStuff) = False Then
                        DB.Dispose()
                        Exit Sub
                    Else
                        Dim tipn As New ARTTBOPTCALC_PAYPAL_IPN
                        tipn.ipn_track_id = IPN.ipn_track_id
                        tipn.STATUS = PayPalEnum.DBError
                        tipn.LICENSE_ID = 0
                        tipn.strRequest = IPN.strRequest
                        DB.ARTTBOPTCALC_PAYPAL_IPN.InsertOnSubmit(tipn)
                        DB.SubmitChanges()
                    End If
                End SyncLock

                If IPN.PAYMENT_STATUS = PayPalPaymentStatusEnum.Completed Or IPN.PAYMENT_STATUS = PayPalPaymentStatusEnum.Processed Then
                    'All OK
                Else
                    DB.Dispose()
                    SendDebugEmail(IPN)
                    Exit Sub
                End If

                trans = DB.Connection.BeginTransaction(IsolationLevel.Serializable)
                DB.Transaction = trans

                Dim License As ARTBOPTCALC_LICENSES = (From q In DB.ARTBOPTCALC_LICENSES
                                                       Join f In DB.ARTBOPTCALC_FINGERPRINTS On f.LICENSE_KEY Equals q.LICENSE_KEY _
                                                       Where f.FINGER_PRINT = IPN.FINGERPRINT And f.PRODUCT_ID = IPN.PRODUCT_ID _
                                                       Select q).FirstOrDefault
                If IsNothing(License) Then
                    Dim ripn = (From q In DB.ARTTBOPTCALC_PAYPAL_IPN Where _
                                q.ipn_track_id = IPN.ipn_track_id _
                                Select q).FirstOrDefault
                    ripn.STATUS = PayPalEnum.InvalidFingerPrint
                    DB.SubmitChanges()
                    trans.Commit()
                    DB.Dispose()
                    Exit Sub
                End If

                License.PURCHASE_ID = IPN.PURCHASE_ID
                License.PRODUCT_ID = IPN.PRODUCT_ID
                License.LICENSE_PURCH_DATE = IPN.LICENSE_PURCH_DATE
                If License.DEMO = False And IPN.LICENSE_PURCH_DATE < License.LICENSE_EXP_DATE Then
                    License.LICENSE_EXP_DATE = License.LICENSE_EXP_DATE.Value.AddMonths(12)
                Else
                    License.LICENSE_EXP_DATE = IPN.LICENSE_PURCH_DATE.AddMonths(12)
                End If
                License.DEMO = False
                If IPN.Company = "" Then
                    License.REG_NAME = IPN.FIRSTNAME & " " & IPN.LASTNAME
                Else
                    License.REG_NAME = IPN.Company
                End If
                License.FIRSTNAME = IPN.FIRSTNAME
                License.LASTNAME = IPN.LASTNAME
                License.COMPANY = IPN.Company
                License.EMAIL = IPN.EMAIL
                License.COUNTRY = IPN.COUNTRY
                License.MAX_LICENSES = IPN.QUANTITY
                Dim cntr As Integer = License.USED_LICENSES
                If License.USED_LICENSES > IPN.QUANTITY Then
                    License.USED_LICENSES = IPN.QUANTITY
                    For Each r In License.ARTBOPTCALC_FINGERPRINTS
                        If cntr <= IPN.QUANTITY Then Exit For
                        If r.FINGER_PRINT <> IPN.FINGERPRINT Then
                            r.ACTIVE = False
                            cntr -= 1
                        End If
                    Next
                Else
                    License.USED_LICENSES = License.USED_LICENSES
                End If

                Dim uipn = (From q In DB.ARTTBOPTCALC_PAYPAL_IPN _
                            Where q.ipn_track_id = IPN.ipn_track_id _
                            Select q).FirstOrDefault
                uipn.LICENSE_ID = License.ID
                uipn.STATUS = PayPalEnum.OK
                DB.SubmitChanges()
                trans.Commit()
                SendClientEmail(IPN, License)
            Catch ex As Exception
                trans.Rollback()
                SendDebugEmail(IPN)
            End Try
        ElseIf strResponse = "INVALID" Then
            'log response/ipn data for manual investigation
        Else

        End If
        DB.Dispose()
    End Sub

    Private Class PayPalIPNClass
        Private _StringRequest As String

        Private _ipn_track_id As String
        Private _txn_id As String
        Private _payment_status As String
        Private _pending_reason As String
        Private _payment_date As String
        Private _payment_gross As String
        Private _payment_fee As String
        Private _mc_currency As String

        Private _custom As String
        Private _option_selection1 As String
        Private _first_name As String
        Private _last_name As String
        Private _residence_country As String
        Private _payer_email As String
        Private _receipt_id As String

        Private _item_name As String
        Private _item_number As String
        Private _quantity As String

        Public Sub New(ByVal f_strRequest As String)
            _StringRequest = f_strRequest

            Dim args As NameValueCollection = HttpUtility.ParseQueryString(f_strRequest)
            On Error Resume Next
            _ipn_track_id = args("ipn_track_id")
            _txn_id = args("_txn_id")
            _payment_status = args("payment_status")
            _pending_reason = args("pending_reason")
            _payment_date = args("payment_date")
            Debug.Print(_payment_date)
            _payment_gross = args("payment_gross")
            _payment_fee = args("mc_fee")
            _mc_currency = args("mc_currency")

            _custom = args("custom")
            _option_selection1 = args("option_selection1")
            _first_name = args("first_name")
            _last_name = args("last_name")
            _residence_country = args("residence_country")
            _payer_email = args("payer_email")
            _receipt_id = args("receipt_id")

            _item_name = args("item_name")
            _item_number = args("item_number")
            _quantity = args("quantity")
            On Error GoTo 0
        End Sub

        Public ReadOnly Property PAYMENT_STATUS As PayPalPaymentStatusEnum
            Get
                Select Case _payment_status
                    Case "Completed" 'The payment has been completed, and the funds have been added successfully to your account balance.
                        Return PayPalPaymentStatusEnum.Completed
                    Case "Processed" 'A payment has been accepted.
                        Return PayPalPaymentStatusEnum.Processed
                    Case "Pending" 'The payment is pending. See pending_reason for more information.
                        Return PayPalPaymentStatusEnum.Pending
                    Case "Canceled_Reversal" 'A reversal has been canceled. For example, you won a dispute with the customer, and the funds for the transaction that was reversed have been returned to you.
                        Return PayPalPaymentStatusEnum.Canceled_Reversal
                    Case "Created" 'A German ELV payment is made using Express Checkout.
                        Return PayPalPaymentStatusEnum.Created
                    Case "Denied" 'The payment was denied. This happens only if the payment was previously pending because of one of the reasons listed for the pending_reason variable or the Fraud_Management_Filters_x variable.
                        Return PayPalPaymentStatusEnum.Denied
                    Case "Expired" ' This authorization has expired and cannot be captured.
                        Return PayPalPaymentStatusEnum.Expired
                    Case "Failed" 'The payment has failed. This happens only if the payment was made from your customer’s bank account.
                        Return PayPalPaymentStatusEnum.Failed
                    Case "Refunded" 'You refunded the payment.
                        Return PayPalPaymentStatusEnum.Refunded
                    Case "Reversed" 'A payment was reversed due to a chargeback or other type of reversal. The funds have been removed from your account balance and returned to the buyer. The reason for the reversal is specified in the ReasonCode element.
                        Return PayPalPaymentStatusEnum.Reversed
                    Case "Voided" 'This authorization has been voided.
                        Return PayPalPaymentStatusEnum.Voided
                End Select
            End Get
        End Property

        Public ReadOnly Property strRequest As String
            Get
                Return _StringRequest
            End Get
        End Property
        Public ReadOnly Property LICENSE_PURCH_DATE As Date
            Get
                Dim d As DateTime = ConvertFromPayPalDate(_payment_date, 0)
                Return d
            End Get
        End Property
        Private Shared Function ConvertFromPayPalDate(rawPayPalDate As String, localUtcOffset As Integer) As DateTime
            ' Converts a PayPal datestring into a valid .net datetime value
            'Example Taken From:
            'http://www.ifinity.com.au/Blog/EntryId/77/Converting-PayPal-Dates-to-Net-DateTime-using-Regex
            ' <param name="dateValue">a string containing a PayPal date</param>
            ' <param name="localUtcOffset">the number of hours from UTC/GMT the local 
            ' time is (ie, the timezone where the computer is)</param>
            ' <returns>Valid DateTime value if successful, DateTime.MinDate if not</returns>

            ' regex pattern splits paypal date into
            '     * time : hh:mm:ss
            '     * date : Mmm dd yyyy
            '     * timezone : PST/PDT
            '     

            Const payPalDateRegex As String = "(?<time>\d{1,2}:\d{2}:\d{2})\s(?<date>(?<Mmm>[A-Za-z\.]{3,5})\s(?<dd>\d{1,2}),?\s(?<yyyy>\d{4}))\s(?<tz>[A-Z]{0,3})"
            '!important : above line broken over two lines for formatting - rejoin in code editor
            'example 05:49:56 Oct. 18, 2009 PDT
            '        20:48:22 Dec 25, 2009 PST
            Dim dateMatch As System.Text.RegularExpressions.Match = Regex.Match(rawPayPalDate, payPalDateRegex, RegexOptions.IgnoreCase)
            Dim time As DateTime, [date] As DateTime = DateTime.MinValue
            'check to see if the regex pattern matched the supplied string
            If dateMatch.Success Then
                'extract the relevant parts of the date from regex match groups
                Dim rawDate As String = dateMatch.Groups("date").Value
                Dim rawTime As String = dateMatch.Groups("time").Value
                Dim tz As String = dateMatch.Groups("tz").Value

                'create date and time values
                If DateTime.TryParse(rawTime, time) AndAlso DateTime.TryParse(rawDate, [date]) Then
                    'add the time to the date value to get the datetime value
                    [date] = [date].Add(New TimeSpan(time.Hour, time.Minute, time.Second))
                    'adjust for the pdt timezone.  Pass 0 to localUtcOffset to get UTC/GMT
                    Dim offset As Integer = localUtcOffset + 7
                    'pdt = utc-7, pst = utc-8
                    If tz = "PDT" Then
                        'pacific daylight time
                        [date] = [date].AddHours(offset)
                    Else
                        'pacific standard time
                        [date] = [date].AddHours(offset + 1)
                    End If
                End If
            End If
            Return [date]
        End Function
        Public ReadOnly Property FINGERPRINT As String
            Get
                Return _custom
            End Get
        End Property
        Public ReadOnly Property PRODUCT_ID As String
            Get
                Return _item_number
            End Get
        End Property
        Public ReadOnly Property PURCHASE_ID As String
            Get
                Return _receipt_id
            End Get
        End Property
        Public ReadOnly Property QUANTITY As Integer
            Get
                Return CInt(_quantity)
            End Get
        End Property
        Public ReadOnly Property COUNTRY As String
            Get
                Return _residence_country
            End Get
        End Property
        Public ReadOnly Property FIRSTNAME As String
            Get
                Return _first_name
            End Get
        End Property
        Public ReadOnly Property LASTNAME As String
            Get
                Return _last_name
            End Get
        End Property
        Public ReadOnly Property EMAIL As String
            Get
                Return _payer_email
            End Get
        End Property
        Public ReadOnly Property ipn_track_id As String
            Get
                Return _ipn_track_id
            End Get
        End Property
        Public ReadOnly Property Company As String
            Get
                Return _option_selection1
            End Get
        End Property
    End Class

    Private Enum PayPalEnum
        OK
        InvalidFingerPrint
        DBError
        OtherError
    End Enum

    Private Enum PayPalPaymentStatusEnum
        Canceled_Reversal 'A reversal has been canceled. For example, you won a dispute with the customer, and the funds for the transaction that was reversed have been returned to you.
        Completed 'The payment has been completed, and the funds have been added successfully to your account balance.
        Created 'A German ELV payment is made using Express Checkout.
        Denied 'The payment was denied. This happens only if the payment was previously pending because of one of the reasons listed for the pending_reason variable or the Fraud_Management_Filters_x variable.
        Expired ' This authorization has expired and cannot be captured.
        Failed 'The payment has failed. This happens only if the payment was made from your customer’s bank account.
        Pending 'The payment is pending. See pending_reason for more information.
        Refunded 'You refunded the payment.
        Reversed 'A payment was reversed due to a chargeback or other type of reversal. The funds have been removed from your account balance and returned to the buyer. The reason for the reversal is specified in the ReasonCode element.
        Processed 'A payment has been accepted.
        Voided 'This authorization has been voided.
    End Enum
    Private Enum ApyPalPendingStatusEnum
        address 'The payment is pending because your customer did not include a confirmed shipping address and your Payment Receiving Preferences is set yo allow you to manually accept or deny each of these payments. To change your preference, go to the Preferences section of your Profile.
        authorization 'You set the payment action to Authorization and have not yet captured funds.
        echeck 'The payment is pending because it was made by an eCheck that has not yet cleared.
        intl 'The payment is pending because you hold a non-U.S. account and do not have a withdrawal mechanism. You must manually accept or deny this payment from your Account Overview.
        multi_currency 'You do not have a balance in the currency sent, and you do not have your profiles's Payment Receiving Preferences option set to automatically convert and accept this payment. As a result, you must manually accept or deny this payment.
        'order 'You set the payment action to Order and have not yet captured funds.
        paymentreview 'The payment is pending while it is reviewed by PayPal for risk.
        regulatory_review 'The payment is pending because PayPal is reviewing it for compliance with government regulations. PayPal will complete this review within 72 hours. When the review is complete, you will receive a second IPN message whose payment_status/reason code variables indicate the result.
        unilateral 'The payment is pending because it was made to an email address that is not yet registered or confirmed.
        upgrade 'The payment is pending because it was made via credit card and you must upgrade your account to Business or Premier status before you can receive the funds. upgrade can also mean that you have reached the monthly limit for transactions on your account.
        verify 'The payment is pending because you are not yet verified. You must verify your account before you can accept this payment.
        other 'The payment is pending for a reason other than those listed above. For more information, contact PayPal Customer Service.
    End Enum

    Private Shared Sub SendDebugEmail(ByVal ipn As PayPalIPNClass)
        ' Command line argument must the the SMTP host. 
        Dim client As New SmtpClient("localhost", 25)

        ' Specify the e-mail sender. 
        ' Create a mailing address that includes a UTF8 character 
        ' in the display name. 
        Dim [from] As New MailAddress("debug@artbtrading.com", "PayPal " & ChrW(&HD8) & " Service", System.Text.Encoding.UTF8)

        ' Set destinations for the e-mail message. 
        Dim [to] As New MailAddress("sales@artbtrading.com")

        ' Specify the message content. 
        Dim message As New MailMessage([from], [to])
        message.Subject = "PayPal Service Error"
        message.Priority = MailPriority.High
        message.Body = "PayPal Service Error for following received message:"
        message.Body += Environment.NewLine
        message.Body += Environment.NewLine
        message.Body += ipn.strRequest
        message.SubjectEncoding = System.Text.Encoding.UTF8

        ' Set the method that is called back when the send operation ends. 
        AddHandler client.SendCompleted, AddressOf SendCompletedCallback

        ' The userState can be any object that allows your callback  
        ' method to identify this send operation. 
        ' For this example, the userToken is a string constant. 
        Dim userState As String = "Debug: " & ipn.ipn_track_id
        Try
            client.SendAsync(message, userState)
        Catch ex As Exception
#If DEBUG Then
            Stop
#End If
        End Try
        Debug.Print("Sending Debug Message... " & ipn.ipn_track_id)
    End Sub
    Private Shared Sub SendTestEmail()
        ' Command line argument must the the SMTP host. 
        Dim client As New SmtpClient("localhost", 25)
        'client.UseDefaultCredentials = False
        'client.Credentials = New System.Net.NetworkCredential("sale@artbtrading.com", "@ndr0s")
        'client.DeliveryMethod = SmtpDeliveryMethod.Network

        ' Specify the e-mail sender. 
        ' Create a mailing address that includes a UTF8 character 
        ' in the display name. 
        Dim [from] As New MailAddress("sales@artbtrading.com", "FFASuite Sales", System.Text.Encoding.UTF8)
        ' Set destinations for the e-mail message. 
        Dim [to] As New MailAddress("hipath@on.gr")

        ' Specify the message content. 
        Dim message As New MailMessage([from], [to])
        message.Subject = "FFASuite Service Renewal Aknowledgement"
        message.Priority = MailPriority.High
        message.IsBodyHtml = False
        message.Body = "Dear Client,"
        message.Body += Environment.NewLine
        message.Body += Environment.NewLine
        message.Body = "Thank you very much for your order. Your FFA Suite service has been renewed till " & FormatDateTime(Today, DateFormat.LongDate)
        message.Body += Environment.NewLine
        message.Body += Environment.NewLine
        message.Body += "Your License Key is: " & "ASDASDASDASDSAD"
        message.Body += Environment.NewLine
        message.Body += Environment.NewLine
        message.Body += "Please keep this Key in a safe place, you will need it to activete the second free PC subscription."
        message.Body += Environment.NewLine
        message.Body += Environment.NewLine
        message.Body += "The FFA Suite Sales Team,"

        ' Set the method that is called back when the send operation ends. 
        AddHandler client.SendCompleted, AddressOf SendCompletedCallback

        ' The userState can be any object that allows your callback  
        ' method to identify this send operation. 
        ' For this example, the userToken is a string constant. 
        Dim userState As String = "Client Message: "
        Try
            client.SendAsync(message, userState)
        Catch ex As Exception
            Stop
        End Try
        Debug.Print("Sending TEST TEST TEST Client Message... ")
    End Sub
    Private Shared Sub SendClientEmail(ByVal IPN As PayPalIPNClass, ByVal lic As ARTBOPTCALC_LICENSES)
        ' Command line argument must the the SMTP host. 
        Dim client As New SmtpClient("localhost", 25)

        ' Specify the e-mail sender. 
        ' Create a mailing address that includes a UTF8 character 
        ' in the display name. 
        Dim [from] As New MailAddress("sales@artbtrading.com", "FFASuite Sales", System.Text.Encoding.UTF8)
        ' Set destinations for the e-mail message. 
        Dim [to] As New MailAddress(IPN.EMAIL)

        ' Specify the message content. 
        Dim message As New MailMessage([from], [to])
        message.Subject = "FFASuite Service Renewal Acknowledgement"
        message.Priority = MailPriority.High
        message.IsBodyHtml = False
        message.Body = "Dear Client,"
        message.Body += Environment.NewLine
        message.Body += Environment.NewLine
        message.Body = "Thank you very much for your order. Your FFA Suite service has been renewed till " & FormatDateTime(lic.LICENSE_EXP_DATE, DateFormat.LongDate)
        message.Body += Environment.NewLine
        message.Body += Environment.NewLine
        Dim retval As String = lic.LICENSE_KEY(0)
        For I = 1 To lic.LICENSE_KEY.Length - 1
            Dim m = I Mod 3
            If (m) = 0 Then
                retval += "-" & lic.LICENSE_KEY(I)
            Else
                retval += lic.LICENSE_KEY(I)
            End If
        Next
        message.Body += "Your License Key is: " & retval
        message.Body += Environment.NewLine
        message.Body += Environment.NewLine
        message.Body += "Please keep this Key in a safe place, you will need it to activete the second free PC subscription."
        message.Body += Environment.NewLine
        message.Body += Environment.NewLine
        message.Body += "The FFA Suite Sales Team,"

        ' Set the method that is called back when the send operation ends. 
        AddHandler client.SendCompleted, AddressOf SendCompletedCallback

        ' The userState can be any object that allows your callback  
        ' method to identify this send operation. 
        ' For this example, the userToken is a string constant. 
        Dim userState As String = "Client Message: " & IPN.ipn_track_id
        Try
            client.SendAsync(message, userState)
        Catch ex As Exception
#If DEBUG Then
            Stop
#End If
        End Try
        Debug.Print("Sending Client Message... " & IPN.ipn_track_id)
    End Sub
    Private Shared Sub SendCompletedCallback(ByVal sender As Object, ByVal e As AsyncCompletedEventArgs)
        ' Get the unique identifier for this asynchronous operation. 
        Dim smtpc As SmtpClient = TryCast(sender, SmtpClient)
        Dim token As String = CStr(e.UserState)

        If e.Cancelled Then
            Debug.Print("[{0}] Send canceled.", token)
        End If
        If e.Error IsNot Nothing Then
            Debug.Print("[{0}] {1}", token, e.Error.ToString())
        Else
            Debug.Print("Message sent.")
        End If
        smtpc.Dispose()
        smtpc = Nothing
    End Sub
End Class