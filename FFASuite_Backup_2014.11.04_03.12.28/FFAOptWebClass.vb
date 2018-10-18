Imports System.Net
Imports System.Net.Sockets
Imports System.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.ComponentModel
Imports System.Net.Security
Imports System.ServiceModel

Public Class ArtBOptDataClass

    Private WithEvents m_SDB As FFAOptCalcService.FFAOptMainClient
    Private binding As WSHttpBinding
    Private endpoint As EndpointAddress
    Private m_ServiceIsOK As Boolean

    Public Sub New()
#If DEBUG Then
        'ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertficate)
#End If
        m_SDB = New FFAOptCalcService.FFAOptMainClient
    End Sub

    Public Function VerifyServiceStatus() As WCFServiceStatusEnum
        Dim npc As WCFServiceStatusClass = Me.WCFServiceStatus
        Try
            Dim d As Date = m_SDB.ServerDate
            Dim host As String
#If DEBUG Then
            host = My.Settings.xmppserverdemo
#Else
            host = My.Settings.xmppserver
#End If
            If TCPOpenPort(host, 5222) Then
                My.Settings.xmppUseTCP = True
                My.Settings.Save()
            Else
                If Me.WCFServiceStatus.ProxyDetected = True Then
                    Dim cred As NetworkCredential = CredentialCache.DefaultNetworkCredentials
                    If IsNothing(cred) = False Then
                        If IsNothing(cred.UserName) = False And IsNothing(cred.Password) = False Then
                            If My.Settings.UsesProxy = False Then
                                BehindProxy = True
                                My.Settings.proxyaddress = Me.WCFServiceStatus.ProxyAddress
                                My.Settings.UsesProxy = cred.UserName
                                My.Settings.proxypswd = cred.Password
                            End If
                        End If
                    End If
                Else
                    My.Settings.UsesProxy = False
                    My.Settings.Save()
                End If
            End If
            Return WCFServiceStatusEnum.ServiceIsOK
        Catch ex As Exception
            If My.Settings.UsesProxy = True Then
                Dim wproxy As New WebProxy(npc.ProxyAddress, True)
                wproxy.Credentials = New NetworkCredential(My.Settings.proxyuser, My.Settings.proxypswd)
                WebRequest.DefaultWebProxy = wproxy
                m_SDB = New FFAOptCalcService.FFAOptMainClient
            End If
        End Try

        Try
            Dim d As Date = m_SDB.ServerDate
            BehindProxy = True
            Return WCFServiceStatusEnum.ServiceIsOK
        Catch ex As Exception
            'we need to define fresh proxy credentials
            Dim msg As String
            msg = "It seems your PC is sitting behind a corporate web proxy."
            msg += vbCrLf & "You need to define your porxy setup string and credentials"
            msg += vbCrLf & vbCrLf & "If you are not sure what value to enter then"
            msg += vbCrLf & "your IT stuff could assist you in this regard."
            msg += vbCrLf & vbCrLf & "Press OK to continue, or Cancel to exit application."
            Dim answ = MsgError(FirstTimeUserForm, msg, "WEB Proxy Configuration needed", MessageBoxButtons.OKCancel, Telerik.WinControls.RadMessageIcon.Error)
            If answ = DialogResult.Cancel Then
                Return WCFServiceStatusEnum.UserWantsToExitApplication
            End If
        End Try

        Dim f As New ArtBConfigureForm
        f.SetUpProxyOnly = True
        f.ExternalProxyAddress = npc.ProxyAddress
        Dim dr As DialogResult = f.ShowDialog()
        If dr = DialogResult.OK Then
            Dim wproxy As New WebProxy(npc.ProxyAddress, True)
            wproxy.Credentials = New NetworkCredential(My.Settings.proxyuser, My.Settings.proxypswd)
            WebRequest.DefaultWebProxy = wproxy
            m_SDB = New FFAOptCalcService.FFAOptMainClient
            Try
                Dim d As Date = m_SDB.ServerDate
                BehindProxy = True
                Return WCFServiceStatusEnum.ServiceIsOK
            Catch ex As Exception
                Dim msg As String
                msg = "Unable to access web service, pleaase consult your IT"
                msg += vbCrLf & "department for more help. Application will now exit."
                MsgError(FirstTimeUserForm, msg, "WEB Proxy Configuration Error", MessageBoxButtons.OK, Telerik.WinControls.RadMessageIcon.Error)
                Return WCFServiceStatusEnum.UserWantsToExitApplication
            End Try
        End If

        Return WCFServiceStatusEnum.UserWantsToExitApplication
    End Function

    Public Property SDB As FFAOptCalcService.FFAOptMainClient
        Get
            Return m_SDB
        End Get
        Set(value As FFAOptCalcService.FFAOptMainClient)
            m_SDB = value
        End Set
    End Property
    Public ReadOnly Property WCFServiceStatus As WCFServiceStatusClass
        Get
            Dim npc As New WCFServiceStatusClass
            Dim mysite As String = "http://brs.artbtrading.com/"
            Dim userproxy As IWebProxy = WebRequest.GetSystemWebProxy()
            Dim objUriProxy As System.Uri = userproxy.GetProxy(New System.Uri(mysite))
            If objUriProxy.AbsoluteUri.ToString <> mysite Then
                npc.ProxyDetected = True
                npc.ProxyAddress = objUriProxy.AbsoluteUri.ToString
            Else
                npc.ProxyDetected = False
            End If
            npc.ServiceStatus = m_ServiceIsOK
            Return npc
        End Get
    End Property
End Class
Public Class WCFServiceStatusClass
    Private m_ServiceStatus As WCFServiceStatusEnum
    Private m_ProxyDetected As Boolean
    Private m_ProxyAddress As String = String.Empty
    Public Property ServiceStatus As WCFServiceStatusEnum
        Get
            Return m_ServiceStatus
        End Get
        Set(value As WCFServiceStatusEnum)
            m_ServiceStatus = value
        End Set
    End Property
    Public Property ProxyDetected As Boolean
        Get
            Return m_ProxyDetected
        End Get
        Set(value As Boolean)
            m_ProxyDetected = value
        End Set
    End Property
    Public Property ProxyAddress As String
        Get
            Return m_ProxyAddress
        End Get
        Set(value As String)
            m_ProxyAddress = value
        End Set
    End Property
End Class
Public Enum WCFServiceStatusEnum
    ServiceIsOK
    UserWantsToExitApplication
    ServiceStillDoesNotWork
End Enum