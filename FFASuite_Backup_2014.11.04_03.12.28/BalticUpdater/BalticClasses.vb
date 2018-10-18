Imports System.Runtime.Serialization
Imports System.Xml.Serialization

Namespace DataContracts
    <DataContract> _
    Public Class RoutesUpdateClass
        Private m_RoutesUpdate As New List(Of RouteUpdate)

        <DataMember> _
        Public Property RoutesUpdate As List(Of RouteUpdate)
            Get
                Return Me.m_RoutesUpdate
            End Get
            Set(value As List(Of RouteUpdate))
                Me.m_RoutesUpdate = value
            End Set
        End Property
    End Class

    <DataContract> _
    Partial Public Class RouteUpdate
        Private m_CMSRouteArchiveId As String
        Private m_CMSRouteId As String
        Private m_RouteAverage As Double
        Private m_ArchiveDate As Date
        Private m_NextRolloverDate As Date
        Private m_ReportDesc As String

        <DataMember> _
        Public Property CMSRouteArchiveId As String
            Get
                Return Me.m_CMSRouteArchiveId
            End Get
            Set(value As String)
                Me.m_CMSRouteArchiveId = value
            End Set
        End Property

        <DataMember> _
        Public Property CMSRouteId As String
            Get
                Return Me.m_CMSRouteId
            End Get
            Set(value As String)
                Me.m_CMSRouteId = value
            End Set
        End Property

        <DataMember> _
        Public Property RouteAverage As Double
            Get
                Return Me.m_RouteAverage
            End Get
            Set(value As Double)
                Me.m_RouteAverage = value
            End Set
        End Property

        <DataMember> _
        Public Property ArchiveDate As Date
            Get
                Return Me.m_ArchiveDate
            End Get
            Set(value As Date)
                Me.m_ArchiveDate = value
            End Set
        End Property

        <DataMember> _
        Public Property NextRolloverDate As Date
            Get
                Return Me.m_NextRolloverDate
            End Get
            Set(value As Date)
                Me.m_NextRolloverDate = value
            End Set
        End Property

        <DataMember> _
        Public Property ReportDesc As String
            Get
                Return Me.m_ReportDesc
            End Get
            Set(value As String)
                Me.m_ReportDesc = value
            End Set
        End Property
    End Class


End Namespace