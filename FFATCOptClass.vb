Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Environment
Imports System.Collections.Specialized
Imports System.Xml.Serialization

Public Class FFATCOptClass

#Region "FormPrivateAssignments"
    Private m_ROUTE_ID As Integer
    Private m_GRIDDATA As New List(Of GRIDPeriodsClass)
    Private m_FIXINGS As New List(Of FFAOptCalcService.VolDataClass)
    Private m_INTEREST_RATES As New List(Of FFAOptCalcService.InterestRatesClass)
    Private m_PUBLIC_HOLIDAYS As New List(Of Date)
    Private m_ROUTE_DETAIL As FFAOptCalcService.SwapDataClass
    Private m_SERVER_DATE As Date

    Private cb_TC_START As TCMonthsClass
    Private cb_TC_END As TCMonthsClass
    Private cb_OPTION_END As TCMonthsClass
    Private rtb_TC_DES As String
    Private se_VEP As Double
    Private se_SKEW As Double

    Private rrb_MAIN_CHARTER_TYPE As Telerik.WinControls.Enumerations.ToggleState
    Private se_MAIN_CHARTER_RATE As Double
    Private se_MAIN_PROTECTION As Double
    Private se_MAIN_FFA_PRICE As Double
    Private rcb_MAIN_HAS_PROFIT_SHARE As Telerik.WinControls.Enumerations.ToggleState
    Private se_MAIN_PROFIT_SHARE As Double
    Private se_MAIN_PROFIT_SHARE_STRIKE As Double
    Private se_MAIN_PROFIT_SHARE_CAP As Double
    Private rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP As Telerik.WinControls.Enumerations.ToggleState
    Private se_MAIN_ADJ_TC_RATE As Double
    Private se_OPTIONS_PRICE As Double

    Private rrb_OPTION_CHARTER_TYPE As Telerik.WinControls.Enumerations.ToggleState
    Private se_OPTION_CHARTER_RATE As Double
    Private se_OPTION_FFA_PRICE As Double
    Private rcb_OPTION_HAS_PROFIT_SHARE As Telerik.WinControls.Enumerations.ToggleState
    Private rcb_SAME_AS_MAIN As Telerik.WinControls.Enumerations.ToggleState
    Private se_OPTION_PROFIT_SHARE As Double
    Private se_OPTION_PROFIT_SHARE_STRIKE As Double
    Private rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP As Telerik.WinControls.Enumerations.ToggleState
    Private se_OPTION_PROFIT_SHARE_CAP As Double
    Private se_OPTION_ADJ_TC_RATE As Double
#End Region

    Public Sub New()
    End Sub
    Public Sub New(ByVal Form As FFATimeCharterOptionForm)
        m_ROUTE_ID = Form.ROUTE_ID
        m_FIXINGS = Form.FIXINGS
        m_INTEREST_RATES = Form.INTEREST_RATES
        m_PUBLIC_HOLIDAYS = Form.PUBLIC_HOLIDAYS
        m_SERVER_DATE = Form.SERVER_DATE
        m_GRIDDATA = Form.GRIDDATA
        m_ROUTE_DETAIL = Form.ROUTE_DETAIL

        cb_TC_START = Form.cb_TC_START.SelectedItem.DataBoundItem
        cb_TC_END = Form.cb_TC_END.SelectedItem.DataBoundItem
        cb_OPTION_END = Form.cb_OPTION_END.SelectedItem.DataBoundItem
        rtb_TC_DES = Form.rtb_TC_DES.Text
        se_VEP = Form.se_VEP.Value
        se_SKEW = Form.se_SKEW.Value

        rrb_MAIN_CHARTER_TYPE = Form.rrb_MAIN_CHARTER_TYPE_FIXED.ToggleState
        se_MAIN_CHARTER_RATE = Form.se_MAIN_CHARTER_RATE.Value
        se_MAIN_PROTECTION = Form.se_MAIN_PROTECTION.Value
        se_MAIN_FFA_PRICE = Form.se_MAIN_FFA_PRICE.Value
        se_OPTION_ADJ_TC_RATE = Form.se_MAIN_ADJ_TC_RATE.Value
        se_OPTIONS_PRICE = Form.se_OPTIONS_PRICE.Value
        rcb_MAIN_HAS_PROFIT_SHARE = Form.rcb_MAIN_HAS_PROFIT_SHARE.ToggleState
        se_MAIN_PROFIT_SHARE = Form.se_MAIN_PROFIT_SHARE.Value
        rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP = Form.rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState

        se_MAIN_PROFIT_SHARE_STRIKE = Form.se_MAIN_PROFIT_SHARE_STRIKE.Value        
        se_MAIN_PROFIT_SHARE_CAP = Form.se_MAIN_PROFIT_SHARE_CAP.Value
        se_MAIN_ADJ_TC_RATE = Form.se_MAIN_ADJ_TC_RATE.Value

        rcb_SAME_AS_MAIN = Form.rcb_SAME_AS_MAIN.ToggleState
        rrb_OPTION_CHARTER_TYPE = Form.rrb_OPTION_CHARTER_TYPE_FIXED.ToggleState
        se_OPTION_CHARTER_RATE = Form.se_OPTION_CHARTER_RATE.Value
        se_OPTION_FFA_PRICE = Form.se_OPTION_FFA_PRICE.Value
        se_OPTION_ADJ_TC_RATE = Form.se_OPTION_ADJ_TC_RATE.Value
        rcb_OPTION_HAS_PROFIT_SHARE = Form.rcb_OPTION_HAS_PROFIT_SHARE.ToggleState
        se_OPTION_PROFIT_SHARE = Form.se_OPTION_PROFIT_SHARE.Value
        rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP = Form.rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP.ToggleState
        se_OPTION_PROFIT_SHARE_STRIKE = Form.se_OPTION_PROFIT_SHARE_STRIKE.Value
        se_OPTION_PROFIT_SHARE_CAP = Form.se_OPTION_PROFIT_SHARE_CAP.Value

    End Sub

#Region "PUBLIC_PROPERTIES"
    Public Property OPTIONS_PRICE As Double
        Get
            Return se_OPTIONS_PRICE
        End Get
        Set(value As Double)
            se_OPTIONS_PRICE = value
        End Set
    End Property
    Public Property MAIN_ADJ_TC_RATE As Double
        Get
            Return se_MAIN_ADJ_TC_RATE
        End Get
        Set(value As Double)
            se_MAIN_ADJ_TC_RATE = value
        End Set
    End Property
    Public Property OPTION_ADJ_TC_RATE As Double
        Get
            Return se_OPTION_ADJ_TC_RATE
        End Get
        Set(value As Double)
            se_OPTION_ADJ_TC_RATE = value
        End Set
    End Property

    Public Property ROUTE_ID As Integer
        Get
            Return m_ROUTE_ID
        End Get
        Set(value As Integer)
            m_ROUTE_ID = value
        End Set
    End Property
    Public Property SERVER_DATE As Date
        Get
            Return m_SERVER_DATE
        End Get
        Set(value As Date)
            m_SERVER_DATE = value
        End Set
    End Property
    Public Property PUBLIC_HOLIDAYS As List(Of Date)
        Get
            Return m_PUBLIC_HOLIDAYS
        End Get
        Set(value As List(Of Date))
            m_PUBLIC_HOLIDAYS = value
        End Set
    End Property
    Public Property INTEREST_RATES As List(Of FFAOptCalcService.InterestRatesClass)
        Get
            Return m_INTEREST_RATES
        End Get
        Set(value As List(Of FFAOptCalcService.InterestRatesClass))
            m_INTEREST_RATES = value
        End Set
    End Property
    Public Property FIXINGS As List(Of FFAOptCalcService.VolDataClass)
        Get
            Return m_FIXINGS
        End Get
        Set(value As List(Of FFAOptCalcService.VolDataClass))
            m_FIXINGS = value
        End Set
    End Property
    Public Property GRIDDATA As List(Of GRIDPeriodsClass)
        Get
            Return m_GRIDDATA
        End Get
        Set(value As List(Of GRIDPeriodsClass))
            m_GRIDDATA = value
        End Set
    End Property
    Public Property ROUTE_DETAIL As FFAOptCalcService.SwapDataClass
        Get
            Return m_ROUTE_DETAIL
        End Get
        Set(value As FFAOptCalcService.SwapDataClass)
            m_ROUTE_DETAIL = value
        End Set
    End Property
    Public Property TC_START As TCMonthsClass
        Get
            Return cb_TC_START
        End Get
        Set(value As TCMonthsClass)
            cb_TC_START = value
        End Set
    End Property
    Public Property TC_END As TCMonthsClass
        Get
            Return cb_TC_END
        End Get
        Set(value As TCMonthsClass)
            cb_TC_END = value
        End Set
    End Property
    Public Property OPTION_END As TCMonthsClass
        Get
            Return cb_OPTION_END
        End Get
        Set(value As TCMonthsClass)
            cb_OPTION_END = value
        End Set
    End Property
    Public Property TC_DES As String
        Get
            Return rtb_TC_DES
        End Get
        Set(value As String)
            rtb_TC_DES = value
        End Set
    End Property
    Public Property VEP As Double
        Get
            Return se_VEP
        End Get
        Set(value As Double)
            se_VEP = value
        End Set
    End Property
    Public Property SKEW As Double
        Get
            Return se_SKEW
        End Get
        Set(value As Double)
            se_SKEW = value
        End Set
    End Property
    Public Property MAIN_CHARTER_TYPE As Telerik.WinControls.Enumerations.ToggleState
        Get
            Return rrb_MAIN_CHARTER_TYPE
        End Get
        Set(value As Telerik.WinControls.Enumerations.ToggleState)
            rrb_MAIN_CHARTER_TYPE = value
        End Set
    End Property
    Public Property MAIN_CHARTER_RATE As Double
        Get
            Return se_MAIN_CHARTER_RATE
        End Get
        Set(value As Double)
            se_MAIN_CHARTER_RATE = value
        End Set
    End Property
    Public Property MAIN_PROTECTION As Double
        Get
            Return se_MAIN_PROTECTION
        End Get
        Set(value As Double)
            se_MAIN_PROTECTION = value
        End Set
    End Property
    Public Property MAIN_FFA_PRICE As Double
        Get
            Return se_MAIN_FFA_PRICE
        End Get
        Set(value As Double)
            se_MAIN_FFA_PRICE = value
        End Set
    End Property
    Public Property MAIN_HAS_PROFIT_SHARE As Telerik.WinControls.Enumerations.ToggleState
        Get
            Return rcb_MAIN_HAS_PROFIT_SHARE
        End Get
        Set(value As Telerik.WinControls.Enumerations.ToggleState)
            rcb_MAIN_HAS_PROFIT_SHARE = value
        End Set
    End Property
    Public Property MAIN_PROFIT_SHARE As Double
        Get
            Return se_MAIN_PROFIT_SHARE
        End Get
        Set(value As Double)
            se_MAIN_PROFIT_SHARE = value
        End Set
    End Property
    Public Property MAIN_PROFIT_SHARE_STRIKE As Double
        Get
            Return se_MAIN_PROFIT_SHARE_STRIKE
        End Get
        Set(value As Double)
            se_MAIN_PROFIT_SHARE_STRIKE = value
        End Set
    End Property
    Public Property MAIN_PROFIT_SHARE_CAP As Double
        Get
            Return se_MAIN_PROFIT_SHARE_CAP
        End Get
        Set(value As Double)
            se_MAIN_PROFIT_SHARE_CAP = value
        End Set
    End Property
    Public Property OPTION_CHARTER_TYPE As Telerik.WinControls.Enumerations.ToggleState
        Get
            Return rrb_OPTION_CHARTER_TYPE
        End Get
        Set(value As Telerik.WinControls.Enumerations.ToggleState)
            rrb_OPTION_CHARTER_TYPE = value
        End Set
    End Property
    Public Property OPTION_CHARTER_RATE As Double
        Get
            Return se_OPTION_CHARTER_RATE
        End Get
        Set(value As Double)
            se_OPTION_CHARTER_RATE = value
        End Set
    End Property
    Public Property OPTION_FFA_PRICE As Double
        Get
            Return se_OPTION_FFA_PRICE
        End Get
        Set(value As Double)
            se_OPTION_FFA_PRICE = value
        End Set
    End Property
    Public Property OPTION_HAS_PROFIT_SHARE As Telerik.WinControls.Enumerations.ToggleState
        Get
            Return rcb_OPTION_HAS_PROFIT_SHARE
        End Get
        Set(value As Telerik.WinControls.Enumerations.ToggleState)
            rcb_OPTION_HAS_PROFIT_SHARE = value
        End Set
    End Property
    Public Property SAME_AS_MAIN As Telerik.WinControls.Enumerations.ToggleState
        Get
            Return rcb_SAME_AS_MAIN
        End Get
        Set(value As Telerik.WinControls.Enumerations.ToggleState)
            rcb_SAME_AS_MAIN = value
        End Set
    End Property
    Public Property OPTION_PROFIT_SHARE As Double
        Get
            Return se_OPTION_PROFIT_SHARE
        End Get
        Set(value As Double)
            se_OPTION_PROFIT_SHARE = value
        End Set
    End Property
    Public Property OPTION_PROFIT_SHARE_STRIKE As Double
        Get
            Return se_OPTION_PROFIT_SHARE_STRIKE
        End Get
        Set(value As Double)
            se_OPTION_PROFIT_SHARE_STRIKE = value
        End Set
    End Property
    Public Property MAIN_PROFIT_SHARE_ADJ_FOR_VEP As Telerik.WinControls.Enumerations.ToggleState
        Get
            Return rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP
        End Get
        Set(value As Telerik.WinControls.Enumerations.ToggleState)
            rcb_MAIN_PROFIT_SHARE_ADJ_FOR_VEP = value
        End Set
    End Property
    Public Property OPTION_PROFIT_SHARE_ADJ_FOR_VEP As Telerik.WinControls.Enumerations.ToggleState
        Get
            Return rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP
        End Get
        Set(value As Telerik.WinControls.Enumerations.ToggleState)
            rcb_OPTION_PROFIT_SHARE_ADJ_FOR_VEP = value
        End Set
    End Property
    Public Property OPTION_PROFIT_SHARE_CAP As Double
        Get
            Return se_OPTION_PROFIT_SHARE_CAP
        End Get
        Set(value As Double)
            se_OPTION_PROFIT_SHARE_CAP = value
        End Set
    End Property
#End Region

End Class

Public Class FFAOptTCComplexClass
    Private m_VEP As Double
    Private m_SKEW As Double
    Private m_PERIOD As Integer
    Private m_DESCR As String
    Private m_TC_TYPE As TCTypesEnum
    Private m_TC_START As TCMonthsClass
    Private m_TC_END As TCMonthsClass
    Private m_CHARTER_RATE As Double
    Private m_FFA_PRICE As Double
    Private m_HAS_PROFIT_SHARE As Telerik.WinControls.Enumerations.ToggleState
    Private m_PROFIT_SHARE As Double
    Private m_PROFIT_SHARE_STRIKE As Double
    Private m_PROFIT_SHARE_CAP As Double
    Private m_PROFIT_SHARE_ADJ_FOR_VEP As Telerik.WinControls.Enumerations.ToggleState
    Private m_ADJ_TC_RATE As Double
    Private m_OPTIONS_PRICE As Double

    Public Property VEP As Double
        Get
            Return m_VEP
        End Get
        Set(value As Double)
            m_VEP = value
        End Set
    End Property
    Public Property SKEW As Double
        Get
            Return m_SKEW
        End Get
        Set(value As Double)
            m_SKEW = value
        End Set
    End Property
    Public Property PERIOD As Integer
        Get
            Return m_PERIOD
        End Get
        Set(value As Integer)
            m_PERIOD = value
        End Set
    End Property
    Public Property DESCR As String
        Get
            Return m_DESCR
        End Get
        Set(value As String)
            m_DESCR = value
        End Set
    End Property
    Public Property TC_TYPE As TCTypesEnum
        Get
            Return m_TC_TYPE
        End Get
        Set(value As TCTypesEnum)
            m_TC_TYPE = value
        End Set
    End Property
    Public Property TC_START As TCMonthsClass
        Get
            Return m_TC_START
        End Get
        Set(value As TCMonthsClass)
            m_TC_START = value
        End Set
    End Property
    Public Property TC_END As TCMonthsClass
        Get
            Return m_TC_END
        End Get
        Set(value As TCMonthsClass)
            m_TC_END = value
        End Set
    End Property
    Public Property CHARTER_RATE As Double
        Get
            Return m_CHARTER_RATE
        End Get
        Set(value As Double)
            m_CHARTER_RATE = value
        End Set
    End Property
    Public Property FFA_PRICE As Double
        Get
            Return m_FFA_PRICE
        End Get
        Set(value As Double)
            m_FFA_PRICE = value
        End Set
    End Property
    Public Property HAS_PROFIT_SHARE As Telerik.WinControls.Enumerations.ToggleState
        Get
            Return m_HAS_PROFIT_SHARE
        End Get
        Set(value As Telerik.WinControls.Enumerations.ToggleState)
            m_HAS_PROFIT_SHARE = value
        End Set
    End Property
    Public Property PROFIT_SHARE As Double
        Get
            Return m_PROFIT_SHARE
        End Get
        Set(value As Double)
            m_PROFIT_SHARE = value
        End Set
    End Property
    Public Property PROFIT_SHARE_STRIKE As Double
        Get
            Return m_PROFIT_SHARE_STRIKE
        End Get
        Set(value As Double)
            m_PROFIT_SHARE_STRIKE = value
        End Set
    End Property
    Public Property PROFIT_SHARE_CAP As Double
        Get
            Return m_PROFIT_SHARE_CAP
        End Get
        Set(value As Double)
            m_PROFIT_SHARE_CAP = value
        End Set
    End Property
    Public Property PROFIT_SHARE_ADJ_FOR_VEP As Telerik.WinControls.Enumerations.ToggleState
        Get
            Return m_PROFIT_SHARE_ADJ_FOR_VEP
        End Get
        Set(value As Telerik.WinControls.Enumerations.ToggleState)
            m_PROFIT_SHARE_ADJ_FOR_VEP = value
        End Set
    End Property
    Public Property ADJ_TC_RATE As Double
        Get
            Return m_ADJ_TC_RATE
        End Get
        Set(value As Double)
            m_ADJ_TC_RATE = value
        End Set
    End Property
    Public Property OPTIONS_PRICE As Double
        Get
            Return m_OPTIONS_PRICE
        End Get
        Set(value As Double)
            m_OPTIONS_PRICE = value
        End Set
    End Property
End Class

Public Enum TCTypesEnum
    Fixed
    IndexLinked
End Enum