Imports System.Data.Services
Imports System.Data.Services.Common
Imports System.Linq
Imports System.ServiceModel.Web

Public Class ArtBwcfDataService
    ' TODO: replace [[class name]] with your data class name
    Inherits DataService(Of ARTBEntities)

    Public Sub New()
        AddHandler Me.ProcessingPipeline.ProcessingRequest, AddressOf Me.AuthenticateRequest

        ''''''''''''  Message From ME ''''''''''''''''''''''''''''''''''''''''''''''''
        ' ARGHHHHHHHHHHHH the wcf service wont allow special characters like "+' to pass through
        ' and CMSROUTE_ID could be somenting like "C4TC_+1MON"
        ' so we need to add special lines into the web.config file of the service to allow passing through of special charactes
        '<system.web>
        '   <httpRuntime targetFramework="4.5" requestPathInvalidCharacters="" />
        ' </system.web>
        '<system.webServer> 
        '   <security> 
        '       <requestFiltering allowDoubleEscaping="true" /> 
        '   </security> 
        '</system.webServer> 
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

    End Sub
    Public Function AuthenticateRequest(ByVal source As Object, ByVal e As DataServiceProcessingPipelineEventArgs) As Boolean

#If DEBUG Then
        Return True
#End If
        If e.OperationContext.RequestHeaders.AllKeys.Contains("Authorization") = False Then
            Throw New DataServiceException(401, "401 Unauthorized")
            Return False
        End If

        Dim s() As String = e.OperationContext.RequestHeaders.GetValues("Authorization")
        Dim PassPhrase As String = s(0)

        If PassPhrase <> "dexter" Then
            Throw New DataServiceException(401, "401 Unauthorized")
            Return False
        End If
        Return True
    End Function

    ' This method is called only once to initialize service-wide policies.
    Public Shared Sub InitializeService(ByVal config As DataServiceConfiguration)
        ' TODO: set rules to indicate which entity sets and service operations are visible, updatable, etc.
        ' Examples:
        config.SetEntitySetAccessRule("*", EntitySetRights.All)
        config.SetServiceOperationAccessRule("*", ServiceOperationRights.All)
        config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V3
    End Sub

End Class
