Imports System.Web.SessionState
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports FM.WebSync.Server
Imports FM.Json

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        'Config.Current.Server.AllowPublishers = True
        WebSyncServer.DefaultResponseHandler = Function(context)
                                                   Return String.Format("<p>WSFFASuiteService is <b>{0}</b>.</p>" & vbCr & vbLf & "<p>{1}</p>" & vbCr & vbLf & "<p>{2} client(s) are connected.</p>", (If(WebSyncServer.IsActive, "active", "not active")), (If(WebSyncServer.IsActive, Nothing, WebSyncServer.LastFatalMessage)), (If(WebSyncServer.IsActive, WebSyncServer.GetClientCount(), 0)))
                                               End Function
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class