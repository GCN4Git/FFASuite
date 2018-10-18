Imports System
Imports System.IO
Imports System.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Net
Imports System.Net.Security
Imports System.Text
Imports System.Xml
Imports System.Runtime.InteropServices
Imports System.Security.Permissions

Module MainModule
    Public WithEvents DBW As FFASuiteDataService.ARTBEntities
    Public SDB As FFAOptCalcService.FFAOptMainClient = New FFAOptCalcService.FFAOptMainClient

    Public FP As New Security.FingerPrintClass

    Public Function ValidateServerCertficate(ByVal sender As Object, ByVal cert As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        If cert.Subject.Contains("artbtrading") Then
            Return True
        ElseIf My.Settings.ValidCertificates.Contains(cert.GetSerialNumberString) Then
            Return True
        Else
#If DEBUG Then
            Return True
#Else
            Return False
#End If
        End If
    End Function

    Private Sub DBW_SendingRequest(sender As Object, e As Services.Client.SendingRequestEventArgs) Handles DBW.SendingRequest
        Dim p As String = String.Empty
        For I = My.Settings.DogHouse.Length - 1 To 0 Step -1
            p = p & My.Settings.DogHouse.Substring(I, 1)
        Next
        e.RequestHeaders.Add("Authorization", p)
    End Sub

    Public Function MsgError(ByVal sender As Telerik.WinControls.UI.RadForm, msg As String, ByVal msgTitle As String, ByVal msgButton As MessageBoxButtons, ByVal msgIcon As Telerik.WinControls.RadMessageIcon) As DialogResult
        Dim sform As Telerik.WinControls.UI.RadForm = DirectCast(sender, Telerik.WinControls.UI.RadForm)
        Telerik.WinControls.RadMessageBox.SetThemeName(sender.ThemeName)
        Telerik.WinControls.RadMessageBox.Instance.FormElement.TitleBar.TitlePrimitive.Font = New Font(sform.FormElement.TitleBar.Font.Name, sform.FormElement.TitleBar.Font.Size + 3, FontStyle.Bold)
        Telerik.WinControls.RadMessageBox.Instance.Controls("radLabel1").Font = New Font(sform.FormElement.Font.Name, sform.FormElement.Font.Size, FontStyle.Bold)
        Telerik.WinControls.RadMessageBox.Instance.BackColor = Color.White
        MsgError = Telerik.WinControls.RadMessageBox.Show(sender, msg, msgTitle, msgButton, msgIcon)
    End Function
    Public Function MsgError(ByVal sender As System.Windows.Forms.Form, msg As String, ByVal msgTitle As String, ByVal msgButton As MessageBoxButtons, ByVal msgIcon As Telerik.WinControls.RadMessageIcon) As DialogResult
        Dim sform As System.Windows.Forms.Form = DirectCast(sender, System.Windows.Forms.Form)
        Telerik.WinControls.RadMessageBox.SetThemeName("Office2010Silver")
        Telerik.WinControls.RadMessageBox.Instance.FormElement.TitleBar.TitlePrimitive.Font = New Font(sform.Font.Name, sform.Font.Size + 3, FontStyle.Bold)
        Telerik.WinControls.RadMessageBox.Instance.BackColor = Color.White
        MsgError = Telerik.WinControls.RadMessageBox.Show(sender, msg, msgTitle, msgButton, msgIcon)
    End Function

#Region "String Enumeration Class"
    Public NotInheritable Class DryRoutesFormat
        Inherits StringEnumeration(Of DryRoutesFormat)

        Public Shared ReadOnly Fixed As New DryRoutesFormat("#,##")
        Public Shared ReadOnly Dec As New DryRoutesFormat("0.000")

        Public Shared ReadOnly FixedSign As New DryRoutesFormat("+#,##0;-#,##0")
        Public Shared ReadOnly DecSign As New DryRoutesFormat("+0.000;-0.000")

        Private Sub New(ByVal StringConstant As String)
            MyBase.New(StringConstant)
        End Sub
    End Class
    Public MustInherit Class StringEnumeration(Of TStringEnumeration As StringEnumeration(Of TStringEnumeration))
        Implements IStringEnumeration

        Private myString As String
        Sub New(ByVal StringConstant As String)
            myString = StringConstant
        End Sub

#Region "Properties"
        Public Class [Enum]
            Public Shared Function GetValues() As String()
                Dim myValues As New List(Of String)
                For Each myFieldInfo As System.Reflection.FieldInfo In GetSharedFieldsInfo()
                    Dim myValue As StringEnumeration(Of TStringEnumeration) = CType(myFieldInfo.GetValue(Nothing), StringEnumeration(Of TStringEnumeration))  'Shared Fields use a Null object
                    myValues.Add(myValue)
                Next
                Return myValues.ToArray
            End Function

            Public Shared Function GetNames() As String()
                Dim myNames As New List(Of String)
                For Each myFieldInfo As System.Reflection.FieldInfo In GetSharedFieldsInfo()
                    myNames.Add(myFieldInfo.Name)
                Next
                Return myNames.ToArray
            End Function

            Public Shared Function GetName(ByVal myName As StringEnumeration(Of TStringEnumeration)) As String
                Return myName
            End Function

            Public Shared Function isDefined(ByVal myName As String) As Boolean
                If GetName(myName) Is Nothing Then Return False
                Return True
            End Function

            Public Shared Function GetUnderlyingType() As Type
                Return GetType(String)
            End Function

            Friend Shared Function GetSharedFieldsInfo() As System.Reflection.FieldInfo()
                Return GetType(TStringEnumeration).GetFields
            End Function

            Friend Shared Function GetSharedFields() As StringEnumeration(Of TStringEnumeration)()
                Dim myFields As New List(Of StringEnumeration(Of TStringEnumeration))
                For Each myFieldInfo As System.Reflection.FieldInfo In GetSharedFieldsInfo()
                    Dim myField As StringEnumeration(Of TStringEnumeration) = CType(myFieldInfo.GetValue(Nothing), StringEnumeration(Of TStringEnumeration))  'Shared Fields use a Null object
                    myFields.Add(myField)
                Next
                Return myFields.ToArray
            End Function
        End Class
#End Region

#Region "Cast Operators"
        'Downcast to String
        Public Shared Widening Operator CType(ByVal myStringEnumeration As StringEnumeration(Of TStringEnumeration)) As String
            If myStringEnumeration Is Nothing Then Return Nothing
            Return myStringEnumeration.ToString
        End Operator

        'Upcast to StringEnumeration
        Public Shared Widening Operator CType(ByVal myString As String) As StringEnumeration(Of TStringEnumeration)
            For Each myElement As StringEnumeration(Of TStringEnumeration) In StringEnumeration(Of TStringEnumeration).Enum.GetSharedFields
                'Found a Matching StringEnumeration - Return it
                If myElement.ToString = myString Then Return myElement
            Next
            'Did not find a Match - return NOTHING
            Return Nothing
        End Operator

        Overrides Function ToString() As String Implements IStringEnumeration.ToString
            Return myString
        End Function
#End Region

#Region "Concatenation Operators"
        Public Shared Operator &(ByVal left As StringEnumeration(Of TStringEnumeration), ByVal right As StringEnumeration(Of TStringEnumeration)) As String
            If left Is Nothing And right Is Nothing Then Return Nothing
            If left Is Nothing Then Return right.ToString
            If right Is Nothing Then Return left.ToString
            Return left.ToString & right.ToString
        End Operator

        Public Shared Operator &(ByVal left As StringEnumeration(Of TStringEnumeration), ByVal right As IStringEnumeration) As String
            If left Is Nothing And right Is Nothing Then Return Nothing
            If left Is Nothing Then Return right.ToString
            If right Is Nothing Then Return left.ToString
            Return left.ToString & right.ToString
        End Operator
#End Region

#Region "Operator Equals"

        Public Shared Operator =(ByVal left As StringEnumeration(Of TStringEnumeration), ByVal right As StringEnumeration(Of TStringEnumeration)) As Boolean
            If left Is Nothing Or right Is Nothing Then Return False
            Return left.ToString.Equals(right.ToString)
        End Operator

        Public Overrides Function Equals(ByVal obj As Object) As Boolean
            If TypeOf (obj) Is StringEnumeration(Of TStringEnumeration) Then
                Return CType(obj, StringEnumeration(Of TStringEnumeration)).ToString = myString
            ElseIf TypeOf (obj) Is String Then
                Return CType(obj, String) = myString
            End If
            Return False
        End Function
#End Region

#Region "Operator Not Equals"
        Public Shared Operator <>(ByVal left As StringEnumeration(Of TStringEnumeration), ByVal right As StringEnumeration(Of TStringEnumeration)) As Boolean
            Return Not left = right
        End Operator

#End Region

    End Class
    Public Interface IStringEnumeration
        Function ToString() As String
    End Interface
#End Region

    Public Enum DryRoutes
        C2 = 3
        C3 = 4
        C4 = 5
        C5 = 6
        C7 = 7
        C8 = 8
        C9 = 9
        C10 = 10
        C11 = 11
        C12 = 12
        BCI = 32
        C4TC = 36
        P4TC = 37
        S5TC = 38
        H6TC = 39
        BDI = 40

        P1A = 13
        P2A = 14
        P3A = 15
        P4 = 16
        S1A = 17
        S1B = 18
        S2 = 19
        S3 = 20
        S4A = 21
        S4B = 22
        S5 = 23
        S6 = 24
        S7 = 25
        S8 = 74
    End Enum

#Region "SMSPROVIDERS"
    Public Class SMSActivityClass
        Private m_Text As String
        Public Property Text As String
            Get
                Return m_Text
            End Get
            Set(value As String)
                m_Text = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal txt As String)
            m_Text = txt
        End Sub
    End Class
    Public Class SMSProviderClass
        Private m_ID As Integer
        Private m_SMSPROVIDER As String
        Private m_SMSBALANCE As Double

        Public Property ID As Integer
            Get
                Return m_ID
            End Get
            Set(value As Integer)
                m_ID = value
            End Set
        End Property
        Public Property SMSPROVIDER As String
            Get
                Return m_SMSPROVIDER
            End Get
            Set(value As String)
                m_SMSPROVIDER = value
            End Set
        End Property
        Public Property SMSBALANCE As Double
            Get
                Return m_SMSBALANCE
            End Get
            Set(value As Double)
                m_SMSBALANCE = value
            End Set
        End Property
    End Class
    Public Function TxtLocal_Balance() As String
        ' Send a message using the txtLocal transport
        Const TransportURL As String = "http://www.txtlocal.com/getcredits.php"
        Dim TransportUserName As String = My.Settings.TxtLocal_UserName
        Dim TransportPassword As String = My.Settings.TxtLocal_Password

        Dim strPost As String
        ' Build POST String
        strPost = "uname=" & TransportUserName & "&pword=" & TransportPassword

        ' Create POST
        Dim request As WebRequest = WebRequest.Create(TransportURL)
        request.Method = "POST"
        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(strPost)
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = byteArray.Length
        Dim dataStream As Stream = request.GetRequestStream()
        dataStream.Write(byteArray, 0, byteArray.Length)
        dataStream.Close()
        ' Get the response.
        Dim response As WebResponse = request.GetResponse()
        dataStream = response.GetResponseStream()
        Dim reader As New StreamReader(dataStream)
        Dim responseFromServer As String = reader.ReadToEnd()
        ' Clean up the streams.
        reader.Close()
        dataStream.Close()
        response.Close()
        ' Return result to calling function
        If responseFromServer.Length > 0 Then
            Return responseFromServer
        Else
            Return CType(response, HttpWebResponse).StatusDescription
        End If
    End Function
    Public Function TxtLocal_SendSMS(ByVal Test As Boolean, ByVal From As String, _
                                      ByVal Message As String, _
                                      ByVal SendTo As String, _
                                      ByVal URL As String) As String

        ' Send a message using the txtLocal transport
        Const TransportURL As String = "http://www.txtlocal.com/sendsmspost.php"
        Dim TransportUserName As String = My.Settings.TxtLocal_UserName
        Dim TransportPassword As String = My.Settings.TxtLocal_Password
        Const TransportVerbose As Boolean = True
        Dim strPost As String

        ' Build POST String
        strPost = "uname=" + TransportUserName _
        + "&pword=" + TransportPassword _
        + "&message=" + Message _
        + "&from=" + From _
        + "&selectednums=" + SendTo
        If URL <> "" Then
            strPost += "&url=" + URL
        End If
        If Test = True Then
            strPost += "&test=1"
        End If
        If TransportVerbose = True Then
            strPost += "&info=1"
        End If
        ' Create POST
        Dim request As WebRequest = WebRequest.Create(TransportURL)
        request.Method = "POST"
        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(strPost)
        request.ContentType = "application/x-www-form-urlencoded"
        request.ContentLength = byteArray.Length
        Dim dataStream As Stream = request.GetRequestStream()
        dataStream.Write(byteArray, 0, byteArray.Length)
        dataStream.Close()
        ' Get the response.
        Dim response As WebResponse = request.GetResponse()
        dataStream = response.GetResponseStream()
        Dim reader As New StreamReader(dataStream)
        Dim responseFromServer As String = reader.ReadToEnd()
        ' Clean upthe streams.
        reader.Close()
        dataStream.Close()
        response.Close()
        ' Return result to calling function
        If responseFromServer.Length > 0 Then
            Return responseFromServer
        Else
            Return CType(response, HttpWebResponse).StatusDescription
        End If
    End Function
    Public Enum SMSProvidersEnum
        ClickATell = 1
        TxtLocal = 2
    End Enum
#End Region


End Module
