Option Strict Off
Option Explicit On
Imports System.Xml.Serialization

Namespace BalticXML
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929"), _
 System.SerializableAttribute(), _
 System.Diagnostics.DebuggerStepThroughAttribute(), _
 System.ComponentModel.DesignerCategoryAttribute("code"), _
 System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True), _
 System.Xml.Serialization.XmlRootAttribute([Namespace]:="", IsNullable:=False)> _
 Partial Public Class RoutesUpdate

        Private routeUpdateField() As RoutesUpdateRouteUpdate

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("RouteUpdate")> _
        Public Property RouteUpdate As RoutesUpdateRouteUpdate()
            Get
                Return Me.routeUpdateField
            End Get
            Set(value As RoutesUpdateRouteUpdate())
                Me.routeUpdateField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.17929"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True)> _
    Partial Public Class RoutesUpdateRouteUpdate

        Private cMSRouteArchiveIdField As ULong

        Private cMSRouteIdField As String

        Private routeAverageField As Double

        Private archiveDateField As Date

        Private nextRolloverDateField As Date

        Private reportDescField As String

        '''<remarks/>
        Public Property CMSRouteArchiveId() As ULong
            Get
                Return Me.cMSRouteArchiveIdField
            End Get
            Set(value As ULong)
                Me.cMSRouteArchiveIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CMSRouteId() As String
            Get
                Return Me.cMSRouteIdField
            End Get
            Set(value As String)
                Me.cMSRouteIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RouteAverage() As Double
            Get
                Return Me.routeAverageField
            End Get
            Set(value As Double)
                Me.routeAverageField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property ArchiveDate() As Date
            Get
                Return Me.archiveDateField
            End Get
            Set(value As Date)
                Me.archiveDateField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
        Public Property NextRolloverDate() As Date
            Get
                Return Me.nextRolloverDateField
            End Get
            Set(value As Date)
                Me.nextRolloverDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReportDesc() As String
            Get
                Return Me.reportDescField
            End Get
            Set(value As String)
                Me.reportDescField = value
            End Set
        End Property
    End Class
End Namespace
