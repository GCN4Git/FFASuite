Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Runtime.Serialization
Imports FM
Imports FM.WebSync.Server
Imports WSFFASuiteServer.DataContracts

Namespace WSFFASuiteServer

    Public Class WebSyncChat
        Private Shared GD As New List(Of VolDataClass)
        Private Shared ServiceIsBusy As Boolean = False
        Private Shared GDLock As New Object
        Private Shared SwapVolatilityLock As New Object

        Private Shared Messages As New List(Of ChatMessage)
        Private Shared MessagesLock As New Object
        Private Shared OnConnectLock As New Object
        Private Shared AfterConnectLock As New Object

        Private Shared MVPeriodsLock As New Object
        Private Shared MVPeriods As New List(Of DataContracts.VolDataClass)
        Private Shared MarketViewLock As New Object
        Private Shared MarketView As New List(Of VolDataClass)

        <WebSyncEvent(EventType.BeforeConnect)> _
        Public Shared Sub ValidateUser(sender As Object, e As WebSyncEventArgs)
            Dim FINGERPRINT As String = String.Empty
            Dim PRODUCTID As String = String.Empty
            If IsNothing(e.MetaJson) Then
                e.Cancel("Empty Credentials")
                Exit Sub
            ElseIf e.MetaJson = "" Then
                e.Cancel("Empty Credentials")
                Exit Sub
            End If

            Dim answ = Json.Deserialize(Of FFASuiteCredentials)(e.MetaJson)
            Try
                FINGERPRINT = answ.FINGERPRINT
                PRODUCTID = answ.PRODUCT_ID
            Catch ex As Exception
                e.Cancel("Invalid Fingerprint or ProductID")
                Exit Sub
            End Try

            Dim AllOK As Boolean = False
            Dim cr As ClientReference() = WebSyncServer.GetClients
            Dim qr = (From q In cr.AsEnumerable Where _
                      Json.Deserialize(Of String)(q.Bindings.GetRecord("fingerprint").ValueJson) = FINGERPRINT _
                      And Json.Deserialize(Of String)(q.Bindings.GetRecord("productid").ValueJson) = PRODUCTID _
                      Select q).FirstOrDefault
            If IsNothing(qr) = True Then 'client is not connected
                If FINGERPRINT = "FFASUITEREPEATERSERVER" And PRODUCTID = "2809" Then
                    AllOK = True
                Else
                    Dim DB As New ArtBDataContext
                    Dim usid = (From q In DB.ARTBOPTCALC_FINGERPRINTS _
                                Where q.PRODUCT_ID = PRODUCTID And q.FINGER_PRINT = FINGERPRINT _
                                Select q).FirstOrDefault
                    If IsNothing(usid) Then 'non existing user
                        AllOK = False
                    Else
                        AllOK = True
                    End If
                    DB.Dispose()
                End If
            Else
                AllOK = False
            End If
            If AllOK Then
                answ.ANSWER = FPStatusEnum.ExistingClientAllOK
                e.MetaJson = Json.Serialize(answ)
            Else
                answ.ANSWER = FPStatusEnum.OFError
                e.MetaJson = Json.Serialize(answ)
                e.Cancel("Invalid Logon Attempt")
            End If

        End Sub

        <WebSyncEvent(EventType.AfterConnect)> _
        Public Shared Sub TrackUsersArriving(sender As Object, e As WebSyncEventArgs)
            Dim answ = Json.Deserialize(Of FFASuiteCredentials)(e.MetaJson)
            SyncLock AfterConnectLock
                Try
                    e.Client.Bind("fingerprint", Json.Serialize(answ.FINGERPRINT))
                    e.Client.Bind("productid", Json.Serialize(answ.PRODUCT_ID))
                Catch ex As Exception
                    e.Cancel("Client Already Binded")
                    Exit Sub
                End Try
            End SyncLock
        End Sub

        <WebSyncEvent(EventType.AfterSubscribe, "/chat", FilterType.Template)> Public Shared Sub SendMessages(sender As Object, e As WebSyncEventArgs)
            ' include past messages in the response
            Dim pastMessages As ChatMessage()
            SyncLock MessagesLock
                pastMessages = Messages.ToArray()
            End SyncLock
            e.SetExtensionValueJson("pastMessages", Json.Serialize(pastMessages))
        End Sub

        <WebSyncEvent(EventType.BeforePublish, "/chat", FilterType.Template)> _
        Public Shared Sub StoreMessage(sender As Object, e As WebSyncEventArgs)
            ' get the message, timestamp it, and store message for future chatters
            Dim message = Json.Deserialize(Of ChatMessage)(e.PublishInfo.DataJson)
            SyncLock MessagesLock
                message.Timestamp = DateTime.UtcNow
                Messages.Add(message)
            End SyncLock
            e.PublishInfo.DataJson = Json.Serialize(message)
        End Sub

        <WebSyncEvent(EventType.BeforePublish, "/my/trades", FilterType.Template)> _
        Public Shared Sub ProcessService(sender As Object, e As WebSyncEventArgs)
            ' e.PublishInfo has details about the publish request,
            ' so here is where you would modify the request
            ' (before it is processed) or even cancel the
            ' request using e.Cancel(...)
        End Sub

        <WebSyncEvent(EventType.AfterPublish, "/my/trades", FilterType.Template)> _
        Public Shared Sub PublishTradesService(sender As Object, e As WebSyncEventArgs)
            ' e.PublishInfo has details about the publish response,
            ' so here is where you would modify the response
        End Sub
    End Class

End Namespace
