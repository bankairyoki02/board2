Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.Environment
Imports System.Runtime.InteropServices
Imports System.Text
Imports AxMSFlexGridLib
Imports SchedulingBoard
Imports SchedulingBoard.SchedulingBoardDataset



Public Class frmSchedulingBoard

#Region "Public Definitions"
    Public AppConfig As DataTable
    Dim cmndsel As String = "AVAILSB"

    Private Declare Function FindWindow Lib "user32" Alias "FindWindowA" _
    (
    ByVal lpClassName As String,
    ByVal lpWindowName As String
    ) As Integer

    '\\ Declaration to register custom messages
    Private Declare Function RegisterWindowMessage Lib "user32" Alias _
      "RegisterWindowMessageA" (ByVal lpString As String) As Long

    Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" _
        (ByVal hpvDest As Object, ByVal hpvSource As Object, ByVal cbCopy As Long)

    Private Structure COPYDATASTRUCT
        Public dwData As Integer
        Public cbData As Integer
        Public lpData As IntPtr
    End Structure

    Private Const GWL_WNDPROC = (-4)
    Private Const WM_COPYDATA = &H4A

    'Private Const WM_GETMINMAXINFO As Long = &H24

    'Private Const WH_GETMESSAGE As Integer = 3

    'Private Const WINDOWTITLE_SERVER_SCHEDULINGBOARD = "Scheduling Board"

    Class SchedulingBoardCellInfo
        Public is_date As Boolean
        Public is_project As Boolean
        Public is_late As Boolean
        Public is_warehouse As Boolean
        Public is_blank As Boolean
        Public is_part As Boolean
        Public is_qty As Boolean
        Public the_date As Date
        Public ProjStartDate As Date
        Public ProjEndDate As Date
        Public entityno As String
        Public entitydesc As String
        Public warehouse As String
        Public partno As String
        Public qty As Integer
    End Class

    Class CalendarCellInfo
        Public is_date As Boolean
        Public is_warehouse As Boolean
        Public is_blank As Boolean
        Public is_part As Boolean
        Public is_qty As Boolean
        Public the_date As Date
        Public entityno As String
        Public entitydesc As String
        Public warehouse As String
        Public partno As String
        Public qty As Integer
    End Class

    Class TimelineCellInfo
        Public is_date As Boolean
        Public is_warehouse As Boolean
        Public is_blank As Boolean
        Public is_part As Boolean
        Public is_qty As Boolean
        Public the_date As Date
        Public entityno As String
        Public entitydesc As String
        Public warehouse As String
        Public partno As String
        Public qty As Integer
    End Class

    Private _toolTip As ToolTip
    Private _stateList As New List(Of String)
    Private WithEvents _stateTracker As StateTracker
    Private fSchedulingBoardCellInfo(0, maxrowcount) As SchedulingBoardCellInfo

    Private fSchedulingBoardCellInfoList As New List(Of List(Of SchedulingBoardCellInfo))
    Private fCalendarCellInfoList As New List(Of List(Of CalendarCellInfo))
    Private fTimelineCellInfoList As New List(Of List(Of TimelineCellInfo))

    Private _fieldsAvailableForCopying As New Dictionary(Of String, String)

    Private Const maxrowcount = 9999

    Private Const ERR_INVALID_DATE = 20000
    Private Const ERR_INVALID_DATE_MSG = "Date Required"

    Private _dtWarehouses As DataTable
    Private _dtDivisions As DataTable
    Private _dtPermissions As DataTable = Nothing
    Private _can_edit_projects As Boolean = False
    Private _can_see_part_prices As Boolean = False

    Private lUnavailabilityDeletedParts As New List(Of String)

    Private myUserName As String
    Private myCurrency As String
    Private myDoubleClickProgramWithProject As String
    Private myDoubleClickProgramWithoutProject As String

    Private dtFoundPartGroup As DataTable
    Private dtMultiPartList As DataTable
    Private dtCurrentPart As DataTable
    Private dsSchedulingBoardData As New DataSet

    Const FLEXGRID_COLINDEX_ROWTYPE = 0
    Const FLEXGRID_COLINDEX_ROWCODE = 1
    Const FLEXGRID_COLINDEX_ROWDESC = 2

    Private Const FLEXGRID_ROWTYPE_PROJECT As String = "proj"
    Private Const FLEXGRID_ROWTYPE_PART As String = "part"
    Private Const FLEXGRID_ROWTYPE_WAREHOUSE As String = "bld"
    Private Const FLEXGRID_ROWTYPE_SPACER As String = "spacer"

    Private user_colWidths As Integer = 1000
    Private scrollPosition As Integer
    Private strFirstVisiblePartNo As String = Nothing
    Private intFirstVisibleCol As Integer = 3
    Private strLastVisiblePartNo As String = Nothing
    Private intLastVisiblePartRow As Integer = 0

    Public IsDataLoaded As Boolean = False
    Public IsRefreshingMultiPartList As Boolean = False
    Private DoNotSwitchSelectedTabPage As Boolean = False
    Private AllowRepairCopyOptions As Boolean = False

    Public days As Integer = 7

    Private WithEvents fSubstitution As frmScheduleSubstitution

    Public argPartList As String = Nothing
    Public argPartno As String = String.Empty
    Public argStartDate As DateTime = Nothing
    Public argEndDate As DateTime = Nothing
    Public argStartupMode As String = Nothing
    Public argStartupView As String = Nothing
    Private LastPartNo As String = String.Empty

    Private PreviousHighlightColumns_Start As Integer = 0
    Private PreviousHighlightColumns_End As Integer = 0
    Private HighlightColumns_Start As Integer = 0
    Private HighlightColumns_End As Integer = 0

    Private _dsSchedulingBoardDataSet As New SchedulingBoardDataset
    Private _dtUserWarehouseGroups As New UserWarehouseGroupsDataTable
    Private _dtWarehouseGroupWarehouses As New userWarehouseGroupsWarehousesDataTable

    Const PartStringSpaces = 23

    Private _suspendRefreshData As Boolean
    Private _WarehouseListDisplayMode As WarehouseListDisplayMode
    Public Property _MultiPartGroupsHidden As Boolean
    Public Property _WarehousesPaneMinimized As Boolean
    Public Property _PopoutFormLocation As Point
    Public Property _PopoutFormSize As Size
    Public Property _PopoutFormWindowState As FormWindowState

#End Region

#Region "Functions"


    Private Function GetStrIncludedWarehouses(ByRef Codes As Boolean, ByRef Descriptions As Boolean) As String
        Dim warehouseCodes As New Dictionary(Of String, String)
        Dim warehouseDescriptions As New Dictionary(Of String, String)
        Dim WarehousesSelected As Boolean = False

        Dim t = If(argStartupMode = "Crew", _dtDivisions, _dtWarehouses)
        Dim valuemember = If(argStartupMode = "Crew", "division", "warehouse_code")
        Dim displaymember = If(argStartupMode = "Crew", "divdesc", "warehouse_description")

        For Each pnl As Panel In pnlWarehouses.Controls.OfType(Of Panel)
            If pnl.Visible = True Then

                For Each chkWH As CheckBox In pnl.Controls.OfType(Of CheckBox)
                    If chkWH.Checked Then
                        warehouseCodes.Item(pnl.Tag) = True
                        warehouseDescriptions.Item(t.Select(valuemember & "=" & SQLQuote(pnl.Tag))(0)(displaymember)) = True
                        WarehousesSelected = True
                    End If
                Next
            End If
        Next

        Dim strIncludedWarehouseCodes = "'" & Join(warehouseCodes.Keys.ToArray, ",") & "'"
        Dim strIncludedWarehouseDescriptions = "'" & Join(warehouseDescriptions.Keys.ToArray, ",") & "'"

        If Codes Then
            Return strIncludedWarehouseCodes
        ElseIf Descriptions Then
            Return strIncludedWarehouseDescriptions
        Else
            Return Nothing
        End If
    End Function

    Private Function GetStrIncludedParts()

        Dim p As Integer
        Dim partNos As New Dictionary(Of String, String)
        Dim PartNosSelected As Boolean = False

        'If pnlUnvailability.Visible = True Then
        '    For Each dgr As DataGridViewRow In dgvUnavailability.SelectedRows
        '        partNos.Item(dgr.DataBoundItem("partno").ToString) = True
        '    Next
        'Else
        For p = 0 To lstMultiPartList.Items.Count - 1
            If lstMultiPartList.GetItemCheckState(p) Then
                Dim arypartNos() = Split(lstMultiPartList.Items(p).ToString)
                partNos.Item(arypartNos(0).ToString) = True
                PartNosSelected = True
            End If
        Next
        'End If

        Dim strIncludedPartNos = "'" & Join(partNos.Keys.ToArray, ",") & "'"

        Return strIncludedPartNos
    End Function

    Private Function GetStrIncludedJobTypes()

        Dim jobtypes As New Dictionary(Of String, String)
        Dim PartNosSelected As Boolean = False

        For p = 0 To lstMultiPartList.Items.Count - 1
            If lstMultiPartList.GetItemCheckState(p) Then
                Dim arypartNos() = Split(lstMultiPartList.Items(p).ToString, "  ")
                jobtypes.Item(arypartNos(0).ToString) = True
                PartNosSelected = True
            End If
        Next

        Dim strIncludedJobTypes = "'" & Join(jobtypes.Keys.ToArray, ",") & "'"

        Return strIncludedJobTypes
    End Function

    Private Function GetSelectedMultiPartListPartNo()
        Dim selectedPartno

        Try
            selectedPartno = lstMultiPartList.SelectedItem.ToString.Substring(0, InStr(1, lstMultiPartList.SelectedItem.ToString, "  ", vbTextCompare) - 1)
        Catch ex As Exception
            selectedPartno = _stateTracker.GetProp("txtPartNo", "Text")
        End Try

        RefreshPartData(selectedPartno)
        'timer_RefreshData.Start()

        Return selectedPartno
    End Function

    Private Function GetStr_chkFilters() As String
        Dim chkMainFilters As New Dictionary(Of String, String)
        Dim str_chkMainFilters As String = ""

        For Each chk As CheckBox In pnlControls.Controls.OfType(Of CheckBox)()
            If chk.Checked Then chkMainFilters.Item(chk.Name) = True
        Next

        For Each chk As CheckBox In pnlSchedulingBoardControls.Controls.OfType(Of CheckBox)()
            If chk.Checked Then chkMainFilters.Item(chk.Name) = True
        Next

        str_chkMainFilters = "'" & Join(chkMainFilters.Keys.ToArray, ",") & "'"
        Return str_chkMainFilters
    End Function

    Public Function IsWeekend(ByVal DateValue As Object) As Boolean

        Dim dDateValue As Date

        If Not IsDate(DateValue) Then
            Err.Raise(ERR_INVALID_DATE, ERR_INVALID_DATE_MSG)
            IsWeekend = False
            Exit Function
        End If

        dDateValue = CDate(DateValue)
        IsWeekend = (Weekday(dDateValue) Mod 6 = 1)
    End Function

    Private Function GetSchedulingBoardCellInfo(ByVal column As Integer, ByVal row As Integer) As SchedulingBoardCellInfo
        For r = fSchedulingBoardCellInfoList.Count To row
            fSchedulingBoardCellInfoList.Add(New List(Of SchedulingBoardCellInfo))
        Next

        Dim columns = fSchedulingBoardCellInfoList(row)

        For c = columns.Count To column
            columns.Add(New SchedulingBoardCellInfo)
        Next

        Return columns(column)
    End Function

    Private Function GetCalendarCellInfo(ByVal column As Integer, ByVal row As Integer) As CalendarCellInfo
        For r = fCalendarCellInfoList.Count To row
            fCalendarCellInfoList.Add(New List(Of CalendarCellInfo))
        Next

        Dim columns = fCalendarCellInfoList(row)

        For c = columns.Count To column
            columns.Add(New CalendarCellInfo)
        Next

        Return columns(column)
    End Function

    Private Function GetTimelineCellInfo(ByVal column As Integer, ByVal row As Integer) As TimelineCellInfo
        For r = fTimelineCellInfoList.Count To row
            fTimelineCellInfoList.Add(New List(Of TimelineCellInfo))
        Next

        Dim columns = fTimelineCellInfoList(row)

        For c = columns.Count To column
            columns.Add(New TimelineCellInfo)
        Next

        Return columns(column)
    End Function

    Public Function MatchComboText(ByVal cmb As ComboBox, ByVal textToMatch As String, Optional ByVal resetIfNotFound As Boolean = False) As Boolean
        Dim i As Integer

        MatchComboText = False

        For i = 0 To cmb.Items.Count - 1
            If cmb.Items(i) = textToMatch Then
                cmb.SelectedItem = i
                cmb.Text = textToMatch
                MatchComboText = True
                Exit Function
            End If
        Next

        If resetIfNotFound Then
            cmb.SelectedIndex = -1
        End If
    End Function

#End Region

#Region "Form Subs"

    Private Sub frmSchedulingBoard_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F5 And IsDataLoaded Then
            RefreshAvailabilityData()
            e.Handled = True
        End If
    End Sub

    Private Sub Reset_SchedulingBoard_Params()
        argPartList = argPartno
        Dim aryArgPartno = Split(argPartno, ",")

        If argStartupMode = "Crew" Then

            RefreshMultipartListWithJobTypes()

        Else
            Select Case aryArgPartno.Length
                Case 0 'Nothing
                Case 1 : If Not String.IsNullOrEmpty(argPartno) Then
                        Dim partno = argPartno
                        Dim partdesc = GetPartDescFromPartNo(partno)

                        txtPartNo.Text = partno
                        txtPartDesc.Text = partdesc

                        'IsRefreshingMultiPartList = True
                        'SetMultiPartGroupComboByPartNo(argPartno)
                        'IsRefreshingMultiPartList = False

                        _suspendRefreshData = True
                        IsRefreshingMultiPartList = True
                        cboMultiPartGroups.SelectedIndex = 0

                        lstMultiPartList.Items.Clear()
                        lstMultiPartList.Items.Add(partNo & Space(PartStringSpaces - Len(partNo)) & PartDesc, True)
                        lstMultiPartList.Update()

                        dtMultiPartList.Clear()
                        Dim r = dtMultiPartList.NewRow
                        r.Item("partno") = partno
                        r.Item("partdesc") = partdesc
                        dtMultiPartList.Rows.Add(r)

                        txtPartDesc.Text = PartDesc

                        lstMultiPartList.SelectedIndex = 0

                        IsRefreshingMultiPartList = False
                        _suspendRefreshData = False

                        IsDataLoaded = True

                        'RefreshPartScheduleDetails(CurrentFlexGrid)
                        RefreshPartData(partno)
                        RefreshAvailabilityData()

                        'If lstMultiPartList.Items.Count > 0 Then lstMultiPartList.SelectedItem = lstMultiPartList.Items(0)
                    End If
                Case Is > 1
                    If Not String.IsNullOrEmpty(argPartList) Then RefreshMultiPartListByPartList(argPartList)
                    If lstMultiPartList.Items.Count > 0 Then lstMultiPartList.SelectedItem = lstMultiPartList.Items(0)
                    'RefreshPartData(GetCurrentPartNo) 'This has already happened by the time we get to this point
                    RefreshAvailabilityData()
            End Select
        End If

    End Sub

    Private Sub frmSchedulingBoard_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        _dtUserWarehouseGroups = _dsSchedulingBoardDataSet.UserWarehouseGroups
        _dtWarehouseGroupWarehouses = _dsSchedulingBoardDataSet.userWarehouseGroupsWarehouses

        'default start and end dates
        dtpStartDate.Value = Today
        dtpEndDate.Value = DateAdd(Microsoft.VisualBasic.DateInterval.WeekOfYear, 6, Today)


        Dim args As System.Collections.ObjectModel.ReadOnlyCollection(Of String) = My.Application.CommandLineArgs
        'partno:442743,442740,442739 startdate=05/22/2011 enddate=06/22/2011

        If args.Count > 0 Then
            For Each arg In args
                If InStr(arg.ToString(), "mode=") Then argStartupMode = Trim(Replace(arg.ToString(), "mode=", ""))
                If InStr(arg.ToString(), "view=") Then argStartupView = Trim(Replace(arg.ToString(), "view=", ""))

                If InStr(arg.ToString(), "startdate=") Then argStartDate = Trim(Replace(arg.ToString(), "startdate=", ""))
                If InStr(arg.ToString(), "enddate=") Then argEndDate = Trim(Replace(arg.ToString(), "enddate=", ""))

                If InStr(arg.ToString(), "partno:") Then argPartno = Trim(Replace(arg.ToString(), "partno:", ""))
                If InStr(arg.ToString(), "partno=") Then argPartno = Trim(Replace(arg.ToString(), "partno=", ""))


                Select Case argStartupMode
                    Case "Crew"
                        tcMain.TabPages.Remove(tpSchedulingBoard)

                        cmdPartSearch.Visible = False
                        txtPartNo.ReadOnly = True
                        cmdSearchByBarcode.Visible = False

                        lblExclSubWexler.Visible = False

                        tpTimeline.Text = "Timeline (Multi-Job Type)"
                        tpCalendar.Text = "Calendar (Single-Job Type)"

                        ToolStripDropDownButton1.Visible = False
                        For Each b As Button In gbMultiPartGroups.Controls.OfType(Of Button)
                            b.Enabled = False
                        Next
                        cboMultiPartGroups.Enabled = False

                        lblPartSchedulingDetail.Text = "Scheduling Detail"

                        pnlAddPartToOrder.Visible = False

                        scWarehouses.Panel2Collapsed = True

                        StatusStrip1.Visible = False

                        EquipmentModeToolStripMenuItem.Visible = True
                        CrewModeToolStripMenuItem.Visible = False

                    Case Else

                        EquipmentModeToolStripMenuItem.Visible = False
                        CrewModeToolStripMenuItem.Visible = True

                End Select

                Select Case argStartupView
                    Case "Scheduling Board"
                        tcMain.SelectedTab = If(argStartupMode = "Crew", tpTimeline, tpSchedulingBoard)
                    Case "Timeline"
                        tcMain.SelectedTab = tpTimeline
                    Case "Calendar"
                        tcMain.SelectedTab = tpCalendar
                    Case Else
                        Dim LastView = Get_AppConfigSetting(AppConfig, "LastView")
                        If Not String.IsNullOrEmpty(LastView) Then
                            Try
                                tcMain.SelectTab(LastView)
                            Catch

                            End Try
                        Else
                            tcMain.SelectedTab = If(argStartupMode = "Crew", tpTimeline, tpSchedulingBoard)
                        End If
                End Select

                If InStr(arg.ToString(), "startdate=") Then dtpStartDate.Value = CDate(Trim(Replace(arg.ToString(), "startdate=", "")))
                If InStr(arg.ToString(), "enddate=") Then dtpEndDate.Value = CDate(Trim(Replace(arg.ToString(), "enddate=", "")))
            Next
        End If

        Initializations()

        argStartDate = dtpStartDate.Value
        argEndDate = dtpEndDate.Value

        days = DateDiff(DateInterval.DayOfYear, dtpStartDate.Value, dtpEndDate.Value)

        If Get_AppConfigSetting(AppConfig, "str_chkViewOptionsFilters") IsNot Nothing Then Set_chkFilters()

        user_colWidths = Get_AppConfigSetting(AppConfig, "user_colWidths")

        If user_colWidths < 100 Then user_colWidths = 350

        Select Case Get_AppConfigSetting(AppConfig, "MainFormWindowState")
            Case FormWindowState.Maximized
                Dim MainFormLocation = Get_AppConfigSetting(AppConfig, "MainFormLocation")
                If Not String.IsNullOrEmpty(MainFormLocation) Then Me.DesktopLocation = Get_LocationFromString(MainFormLocation)
                Me.WindowState = FormWindowState.Maximized
            Case FormWindowState.Normal
                Me.WindowState = FormWindowState.Normal
                Dim MainFormLocation = Get_AppConfigSetting(AppConfig, "MainFormLocation")
                If Not String.IsNullOrEmpty(MainFormLocation) Then Me.DesktopLocation = Get_LocationFromString(MainFormLocation)
                Dim MainFormSize = Get_AppConfigSetting(AppConfig, "MainFormSize")
                If Not String.IsNullOrEmpty(MainFormSize) Then Me.Size = Get_SizeFromString(MainFormSize)
        End Select

        If Get_AppConfigSetting(AppConfig, "WarehousesPaneMinimized") = True Then
            Dim newText = lblCollapseExpandWarehouses.Tag

            lblCollapseExpandWarehouses.Tag = lblCollapseExpandWarehouses.Text
            lblCollapseExpandWarehouses.Text = newText

            pnlTotalsAndWarehouses.Height = 42
        End If

        If Get_AppConfigSetting(AppConfig, "scPartsSplitterDistance") IsNot Nothing Then
            scParts.SplitterDistance = Get_AppConfigSetting(AppConfig, "scPartsSplitterDistance")
            scParts.Panel1Collapsed = Get_AppConfigSetting(AppConfig, "MultiPartGroupsHidden")
            scPartsAndDetail.SplitterDistance = Get_AppConfigSetting(AppConfig, "scPartsAndDetailSplitterDistance")
        End If

        _PopoutFormLocation = Get_LocationFromString(Get_AppConfigSetting(AppConfig, "PopoutFormLocation"))
        _PopoutFormSize = Get_SizeFromString(Get_AppConfigSetting(AppConfig, "PopoutFormSize"))
        _PopoutFormWindowState = Get_AppConfigSetting(AppConfig, "PopoutFormWindowState")

        Dim displayMode As Object = Get_AppConfigSetting(AppConfig, "WarehouseDisplayMode")
        If argStartupMode = "Crew" Then displayMode = 0

        Dim warehouses As Object = Get_AppConfigSetting(AppConfig, "ChosenDisplayWarehouses")?.ToString().Split({","c}).Where(Function(wh) wh <> "")

        'If argStartupMode = "Crew" Then warehouses = Get_AppConfigSetting(AppConfig, "ChosenDisplayDivisions")?.ToString().Split({","c}).Where(Function(wh) wh <> "")

        If warehouses IsNot Nothing Then
            ChosenDisplayWarehouses.AddRange(warehouses)
        End If
        [Enum].TryParse(displayMode, WarehousesDisplayMode)


        AddHandler lvWarehouseGroups.SelectedIndexChanged, AddressOf lvWarehouseGroups_AfterSelect


        initWarehouseGroups()

        Application.DoEvents()
        Reset_SchedulingBoard_Params()

        If lstMultiPartList.Items.Count > 0 And lstMultiPartList.SelectedIndex = -1 Then
            lstMultiPartList.SelectedItem = lstMultiPartList.Items(0)
            GetSelectedMultiPartListPartNo()
        End If

        IsDataLoaded = True
    End Sub

    Private Sub Initializations()

        timer_DoubleClickInterval.Interval = SystemInformation.DoubleClickTime

        EnableDoubleBuffering(dgvDetail)

        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))

        jammer.Add(
            "select * from App_Config ac where ac.cmndsel = " & SQLQuote(cmndsel),
            Sub(t)
                AppConfig = t
            End Sub)

        If argStartupMode = "Crew" Then
            jammer.Add("SELECT pv.division, pv.divdesc, c.CompanyCode, c.CompanyDesc, c.CountryCode, c.locationcd, c.Abbreviation, c.DefaultCrewOpsEmpno, defaultcrewOps = p.firstname + ' ' + p.lastname, defaultcrewOpsEmail = p.email, c.TouringRevenueGroup
FROM dbo.pedivision pv
JOIN dbo.Company c ON c.CompanyCode = pv.internal_org AND c.IncludeInProjectMaintenance = 1 AND c.isRentalCompany = 1 AND c.IsClairTouring = 1
LEFT OUTER JOIN dbo.peemployee p ON p.empno = c.DefaultCrewOpsEmpno
ORDER BY c.TouringRevenueGroup
",
                   Sub(t)
                       _dtDivisions = t

                       InitWarehouses(_dtDivisions, "division", "divdesc")
                   End Sub)
        Else
            jammer.Add("
With warehouses as (
    select warehouse_code = w.WarehouseCode, warehouse_description = w.WarehouseDesc, w.SalesForecastGroup, w.TimezoneHoursDelta, IsCheckoutInWarehouse
	    , is_default = convert(bit, case when uw.warehouse_entity = w.WarehouseCode then 1 else 0 end), w.Latitude, w.Longitude, w.coordinate
    from dbo.Warehouse w
    join dbo.WarehouseVisible wv on w.WarehouseCode = wv.WarehouseCode and wv.user_name = suser_sname()
    cross apply (
	    select mui.warehouse_entity from dbo.my_user_info mui
    ) uw
    where 
    (
	    w.WarehouseCode <> 'NEW'
	    or
	    uw.warehouse_entity = 'LITZ'
	    or
	    is_member('NewWarehouseViewers') = 1
	    or
	    is_srvrolemember('sysadmin') = 1
    )
    and w.isVisible = 1
)
select warehouse_code, warehouse_description, is_default
from warehouses
order by case when warehouses.coordinate is null then 1 else 0 end
   , warehouses.coordinate.STDistance((select top(1) warehouses.coordinate from warehouses order by warehouses.is_default desc))
   , Longitude, is_default desc, warehouse_code asc
",
                   Sub(t)
                       _dtWarehouses = t

                       InitWarehouses(_dtWarehouses, "warehouse_code", "warehouse_description")
                   End Sub)
        End If





        jammer.Add("select distinct partgroup From Avail_Multipart_Groups order by partgroup",
                   Sub(t)
                       InitMultipartGroupCombo(t)
                   End Sub)

        jammer.Add("select partgroup from Avail_Multipart_Groups where partno = '" & argPartno & "'",
                   Sub(t)
                       dtFoundPartGroup = t
                   End Sub)

        jammer.Add(
            "select appexe = replace(m.appexec,'%ESSVBDir%\','') + m.appframe
             from dbo.user_menu_inclusions umi
             join dbo.mumenus m on umi.packagecd = m.packagecd and umi.cmndsel = m.cmndsel
             where m.appframe <> ''
             union all
             select appframe = 'ManufacturingCalendar'
             where (
                     select db_id(convert(sysname, sc.Value))
                     from dbo.SysConfig sc
                     where sc.Tag = 'PARALLEL_DATABASE'
             ) is not null
             ", Sub(t)

                    For Each Button As ToolStripButton In tsMainVert.Items.OfType(Of ToolStripButton)()
                        Button.Visible = False
                        If t.Select("appexe=" & SQLQuote(Button.Tag)).Count > 0 Or Button.Tag Is Nothing Then
                            Button.Visible = True
                        End If
                    Next

                    For Each Button As ToolStripDropDownButton In tsMainVert.Items.OfType(Of ToolStripDropDownButton)()
                        Button.Visible = False
                        If t.Select("appexe=" & SQLQuote(Button.Tag)).Count > 0 Or Button.Tag Is Nothing Then
                            Button.Visible = True
                        End If
                    Next

                    For Each Button As ToolStripSplitButton In tsMainVert.Items.OfType(Of ToolStripSplitButton)()
                        Button.Visible = False
                        If t.Select("appexe=" & SQLQuote(Button.Tag)).Count > 0 Or Button.Tag Is Nothing Then
                            Button.Visible = True
                        End If
                    Next

                    ProjectMaintenanceToolStripMenuItem.Visible = tsbProjectMaintenance.Visible
                    TransferToolsToolStripMenuItem.Visible = tsbTransferTools.Visible
                    AvailabilityToolStripMenuItem.Visible = tsbAvailability.Visible
                    PartMaintenanceToolStripMenuItem.Visible = tsbPartMaintenance.Visible

                End Sub)

        jammer.Add(
            "select user_name, currency, unitofweight, unitoflength, doubleclick_program_Project, doubleclick_program_NoProject, can_edit_projects = dbo.can_edit_projects(), can_see_part_prices = dbo.can_see_part_prices() from pjtfrusr where user_name = suser_sname()",
            Sub(t)
                myUserName = t.Rows(0).Item("user_name")
                _can_edit_projects = CBool(t.Rows(0).Item("can_edit_projects"))
                _can_see_part_prices = CBool(t.Rows(0).Item("can_see_part_prices"))
                myDoubleClickProgramWithProject = t.Rows(0).Item("doubleclick_program_Project")
                myDoubleClickProgramWithoutProject = t.Rows(0).Item("doubleclick_program_NoProject")
                myCurrency = t.Rows(0).Item("currency")

                sslblRentalValue.Visible = _can_see_part_prices
                sslblRentalValueValue.Visible = _can_see_part_prices
                sslblWeeklyRate.Visible = _can_see_part_prices
                sslblWeeklyRateValue.Visible = _can_see_part_prices
                sslbl2DayWeekRate.Visible = _can_see_part_prices
                sslbl2DayWeekRateValue.Visible = _can_see_part_prices
                sslblDailyRate.Visible = _can_see_part_prices
                sslblDailyRateValue.Visible = _can_see_part_prices
            End Sub)

        jammer.Add("SELECT g.WarehouseGroup FROM dbo.UserWarehouseGroups g
                    WHERE g.user_name = SUSER_SNAME()", _dtUserWarehouseGroups)


        jammer.Add("SELECT w.WarehouseGroup, w.WarehouseCode FROM dbo.userWarehouseGroupsWarehouses w
                    WHERE w.user_name = SUSER_SNAME()", _dtWarehouseGroupWarehouses)

        jammer.Execute()
    End Sub

    Private Sub frmSchedulingBoard_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Try

            Dim SettingsToSave As New List(Of Array)
            SettingsToSave.Add({"MainFormSize", Me.Size.ToString})
            SettingsToSave.Add({"MainFormLocation", Me.Location.ToString})
            SettingsToSave.Add({"MainFormWindowState", Me.WindowState})
            SettingsToSave.Add({"WarehousesPaneMinimized", _WarehousesPaneMinimized})
            SettingsToSave.Add({"str_chkViewOptionsFilters", GetStr_chkFilters()})
            SettingsToSave.Add({"user_colWidths", user_colWidths})
            SettingsToSave.Add({"scPartsSplitterDistance", scParts.SplitterDistance})
            SettingsToSave.Add({"MultiPartGroupsHidden", _MultiPartGroupsHidden})
            SettingsToSave.Add({"scPartsAndDetailSplitterDistance", scPartsAndDetail.SplitterDistance})
            SettingsToSave.Add({"PopoutFormLocation", _PopoutFormLocation.ToString})
            SettingsToSave.Add({"PopoutFormSize", _PopoutFormSize.ToString})
            SettingsToSave.Add({"PopoutFormWindowState", _PopoutFormWindowState})
            SettingsToSave.Add({"LastView", tcMain.SelectedTab.Name})
            If argStartupMode = "Crew" Then
                SettingsToSave.Add({"IncludedDivisions", GetStrIncludedWarehouses(True, False)})
                'SettingsToSave.Add({"ChosenDisplayDivisions", String.Join(",", ChosenDisplayWarehouses)})
                SettingsToSave.Add({"JobTypes", String.Join(",", GetStrIncludedJobTypes)})
            Else
                SettingsToSave.Add({"WarehouseDisplayMode", WarehousesDisplayMode})
                SettingsToSave.Add({"IncludedWarehouses", GetStrIncludedWarehouses(True, False)})
                SettingsToSave.Add({"ChosenDisplayWarehouses", String.Join(",", ChosenDisplayWarehouses)})
            End If


            Dim sSQL As New StringBuilder
            For Each s In SettingsToSave
                sSQL.AppendLine("delete s from app_config s where cmndsel = " & SQLQuote(cmndsel) & " and tag = " & SQLQuote(s(0)))
                sSQL.AppendLine("insert dbo.AppConfig(cmndsel, tag, value) values (" & SQLQuote(cmndsel) & ", " & SQLQuote(s(0)) & ", " & SQLQuote(s(1)) & ")")
            Next

            Dim t As New System.Threading.Tasks.Task(Sub()
                                                         Dim newConn = GetOpenedFinesseConnection()
                                                         newConn.ExecuteNonQuery(sSQL)
                                                     End Sub)
            t.Start()


        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

#End Region

#Region "Initializations"


    Private Sub Form_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        _stateList.AddRange({
            txtPartNo.Name & ".Text",
            cboMultiPartGroups.Name & ".SelectedIndex",
            lstMultiPartList.Name & ".Items",
            lstMultiPartList.Name & ".CheckedIndices",
            lstMultiPartList.Name & ".SelectedIndex",
            tcMain.Name & ".SelectedIndex"
        })
        'TODO: Uncomment this if need it, it makes the project break
        'chkShowDetail.Name & ".Checked",
        '    chkShowFuture.Name & ".Checked",
        '    chkIncludeProposals.Name & ".Checked",
        '    chkSummarize.Name & ".Checked",
        '    chkGroupByParentProject.Name & ".Checked",
        '    dtpStartDate.Name & ".Value",
        '    dtpEndDate.Name & ".Value"

        _stateTracker = New StateTracker(Me, BtnBack, BtnForward, _stateList)

        _toolTip = New ToolTip() With {
            .IsBalloon = True,
            .AutoPopDelay = 5000,
            .InitialDelay = 1000,
            .ReshowDelay = 500
        }

        _toolTip.SetToolTip(fgSchedulingBoard, "Hover over a row")
        _toolTip.Active = False

        If lstMultiPartList.Items.Count > 0 Then
            RefreshAvailabilityData()
            RefreshPartData(GetCurrentPartNo)
        End If
    End Sub

    Private Enum WarehouseListDisplayMode
        AllWarehouses
        OnlyChosenWarehouses
    End Enum

    Private ChosenDisplayWarehouses As New List(Of String)

    Private Property WarehousesDisplayMode As WarehouseListDisplayMode
        Get
            Return _WarehouseListDisplayMode
        End Get

        Set(value As WarehouseListDisplayMode)
            _WarehouseListDisplayMode = value
            Dim iWarehouses As Integer = 0

            For Each iPanel In pnlWarehouses.Controls.OfType(Of Panel)
                Dim shouldBeVisible = iPanel.Tag <> "" AndAlso (
                    _WarehouseListDisplayMode = WarehouseListDisplayMode.AllWarehouses OrElse
                    ChosenDisplayWarehouses.Contains(iPanel.Tag)
                    )

                If shouldBeVisible Then
                    iPanel.Left = (iPanel.Size.Width + 2) * iWarehouses
                    iWarehouses += 1
                End If

                iPanel.Visible = shouldBeVisible
            Next
        End Set
    End Property


    Private Sub InitWarehouses(t As DataTable, valuemember As String, displaymember As String)
        Dim iWarehouses As Integer = 0

        For Each r As DataRow In t.Rows

            Dim new_pnlWarehouse As New Panel()
            With new_pnlWarehouse
                .Name = "pnlWarehouse_" & r.Item(valuemember)
                .Tag = r.Item(valuemember)
                .Size = pnlWarehouse.Size
                .Left = (.Size.Width + 2) * iWarehouses
                .BorderStyle = pnlWarehouse.BorderStyle
                .Visible = True
            End With

            Dim new_chkToggleWarehouse As New CheckBox()
            With new_chkToggleWarehouse
                .Name = "chkToggleWH_" & r.Item(valuemember)
                If argStartupMode = "Crew" Then
                    .Text = r.Item(valuemember)
                Else
                    .Text = GetWarehouseCheckboxLabel(warehouseData:=r, quantityText:="...")
                End If
                .Tag = "qty"
                .AutoEllipsis = chkToggleWarehouse.AutoEllipsis
                .Appearance = chkToggleWarehouse.Appearance
                .Dock = chkToggleWarehouse.Dock
                .AutoSize = chkToggleWarehouse.AutoSize
                .Size = chkToggleWarehouse.Size
                .MaximumSize = chkToggleWarehouse.MaximumSize
                .TextAlign = chkToggleWarehouse.TextAlign
                AddHandler new_chkToggleWarehouse.Click, AddressOf chkToggleWarehouse_CheckedChanged
                AddHandler new_chkToggleWarehouse.MouseMove, AddressOf availablityControl_NavigationEventHandler
                AddHandler new_chkToggleWarehouse.Enter, AddressOf availablityControl_NavigationEventHandler
            End With

            Dim new_lblWarehouseDesc As New Label
            With new_lblWarehouseDesc
                .Name = "lblWarehouseDescription_" & r.Item(valuemember)
                .Text = r.Item(displaymember)
                .Tag = "desc"
                .Dock = lblWarehouseDesc.Dock
                .Font = lblWarehouseDesc.Font
                .Size = lblWarehouseDesc.Size
                .TextAlign = lblWarehouseDesc.TextAlign
                AddHandler new_lblWarehouseDesc.Click,
                    Sub()
                        new_chkToggleWarehouse.Checked = Not new_chkToggleWarehouse.Checked
                        RefreshPartScheduleDetails(new_lblWarehouseDesc, Nothing)
                    End Sub
                AddHandler new_lblWarehouseDesc.MouseMove, AddressOf availablityControl_NavigationEventHandler
            End With

            If argStartupMode <> "Crew" Then

                Dim new_lblRepairQty As New Label
                With new_lblRepairQty
                    .Name = "lblRepairQty_" & r.Item(valuemember)
                    .Text = "(...)"
                    .Tag = "repair"
                    .Dock = lblRepairQty.Dock
                    .Font = New Font(lblRepairQty.Font, FontStyle.Bold)
                    .ForeColor = lblRepairQty.ForeColor
                    .Image = lblRepairQty.Image
                    .ImageAlign = lblRepairQty.ImageAlign
                    .Padding = lblRepairQty.Padding
                    .Size = lblRepairQty.Size
                    .TextAlign = lblRepairQty.TextAlign
                    AddHandler new_lblRepairQty.Click, AddressOf RefreshPartScheduleDetails
                End With

                Dim new_lblLateQty As New Label
                With new_lblLateQty
                    .Name = "lblLateQty_" & r.Item(valuemember)
                    .Text = "(...)"
                    .Tag = "late"
                    .Dock = lblLateQty.Dock
                    .Font = New Font(lblLateQty.Font, FontStyle.Bold)
                    .ForeColor = lblLateQty.ForeColor
                    .Image = lblLateQty.Image
                    .ImageAlign = lblLateQty.ImageAlign
                    .Padding = lblLateQty.Padding
                    .Size = lblLateQty.Size
                    .TextAlign = lblLateQty.TextAlign
                    AddHandler new_lblLateQty.Click, AddressOf RefreshPartScheduleDetails
                End With

                Dim new_lblPlannedOrder As New Label
                With new_lblPlannedOrder
                    .Name = "lblPlannedOrder" & r.Item(valuemember)
                    .Text = "(...)"
                    .Tag = "planned"
                    .Dock = lblPlannedOrder.Dock
                    .Font = New Font(lblPlannedOrder.Font, FontStyle.Bold)
                    .ForeColor = lblPlannedOrder.ForeColor
                    .Image = lblPlannedOrder.Image
                    .ImageAlign = lblPlannedOrder.ImageAlign
                    .Padding = lblPlannedOrder.Padding
                    .Size = lblPlannedOrder.Size
                    .TextAlign = lblPlannedOrder.TextAlign
                    AddHandler new_lblPlannedOrder.Click, AddressOf RefreshPartScheduleDetails
                End With

                new_pnlWarehouse.Controls.Add(new_lblPlannedOrder)
                new_pnlWarehouse.Controls.Add(new_lblLateQty)
                new_pnlWarehouse.Controls.Add(new_lblRepairQty)

            End If

            new_pnlWarehouse.Controls.Add(new_lblWarehouseDesc)
            new_pnlWarehouse.Controls.Add(new_chkToggleWarehouse)

            _stateList.Add(new_chkToggleWarehouse.Name & ".Checked")

            pnlWarehouses.Controls.Add(new_pnlWarehouse)

            iWarehouses += 1

        Next r


        Dim sIncludedWarehouses = Get_AppConfigSetting(AppConfig, "IncludedWarehouses")

        If argStartupMode = "Crew" Then sIncludedWarehouses = Get_AppConfigSetting(AppConfig, "IncludedDivisions")

        If Not String.IsNullOrEmpty(sIncludedWarehouses) Then
            Dim warehouses As String() = Replace(sIncludedWarehouses, "'", "").Split(New Char() {","c}, StringSplitOptions.RemoveEmptyEntries)

            For Each w In warehouses
                Try
                    Dim pnlWH = From pnl As Control In pnlWarehouses.Controls.OfType(Of Panel)
                                Where pnl.Visible = True
                                Where pnl.Tag = w
                                Select pnl

                    Dim chkWh As CheckBox = pnlWH(0).Controls.OfType(Of CheckBox).First
                    chkWh.Checked = True

                Catch ex As Exception
                End Try
            Next
        Else
            Try

                If argStartupMode <> "Crew" Then
                    Dim myWarehouse = _dtWarehouses.Select("is_default=1")(0).Item(valuemember)

                    Dim pnlMyWarehouse = From pnl As Control In pnlWarehouses.Controls.OfType(Of Panel)
                                         Where pnl.Visible = True
                                         Where pnl.Tag = myWarehouse
                                         Select pnl

                    If pnlMyWarehouse.Any Then pnlMyWarehouse(0).Controls.OfType(Of CheckBox).First.Checked = True
                End If

            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Shared Function GetWarehouseCheckboxLabel(warehouseData As DataRow, quantityText As String) As String
        Dim maybeQty = If(quantityText <> "", $" ({quantityText})", "")
        Return $"{warehouseData!warehouse_code}{maybeQty}"
    End Function

    Private Shared Function GetDivisionCheckboxLabel(warehouseData As DataRow, quantityText As String) As String
        Dim maybeQty = If(quantityText <> "", $" ({quantityText})", "")
        Return $"{warehouseData!division}{maybeQty}"
    End Function

    Private Sub InitMultipartGroupCombo(ByVal t As DataTable)

        cboMultiPartGroups.Items.Clear()
        cboMultiPartGroups.Items.Add("(Select a Multi-Part Group...)")
        cboMultiPartGroups.SelectedIndex = 0

        For i = 0 To t.Rows.Count - 1
            cboMultiPartGroups.Items.Add(t.Rows(i).Item("partgroup"))
        Next
    End Sub

    Private Sub Set_chkFilters()
        Dim str_chkFilters = Get_AppConfigSetting(AppConfig, "str_chkViewOptionsFilters")

        For Each chk As CheckBox In pnlControls.Controls.OfType(Of CheckBox)()
            If InStr(str_chkFilters, chk.Name) > 0 Then chk.Checked = True Else chk.Checked = False
        Next

        For Each chk As CheckBox In pnlSchedulingBoardControls.Controls.OfType(Of CheckBox)()
            If InStr(str_chkFilters, chk.Name) > 0 Then chk.Checked = True Else chk.Checked = False
        Next
    End Sub

    Private Sub SetMultiPartGroupComboByPartNo(ByVal partno As String)
        cboMultiPartGroups.SelectedIndex = 0
        If Not dtFoundPartGroup.Rows.Count > 0 Then
            RefreshMultiPartListByPartList(partno)
            SetSelectedPartInMultiPartList(partno)
            IsDataLoaded = True
            RefreshPartData(partno)
            RefreshAvailabilityData()
        Else
            Dim foundPartGroup = dtFoundPartGroup.Rows(0).Item(0)
            For i = 0 To cboMultiPartGroups.Items.Count - 1
                If InStr(cboMultiPartGroups.Items(i), foundPartGroup, vbTextCompare) Then
                    'cboMultiPartGroups.Text = foundPartGroup 'Change text only, get rid of any code in TextChanged event, let SelectedIndexChanged do all the work
                    cboMultiPartGroups.SelectedIndex = cboMultiPartGroups.FindStringExact(foundPartGroup) 'Selected the items index, hopefully firing SelectedIndexChanged rather than TextChanged
                    _suspendRefreshData = True
                    SetSelectedPartInMultiPartList(partno)
                    _suspendRefreshData = False
                    IsDataLoaded = True
                    RefreshAvailabilityData()
                End If
            Next
        End If
    End Sub

#End Region

#Region "Multi-part Groups"

    Private Sub btnMultiPartGroup_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMultiPartGroup_Add.Click
        Dim NewGroupName As String
        Dim i As Long

        NewGroupName = InputBox("Group Name:", "Add New Multi-Part Group")
        If NewGroupName = String.Empty Then Exit Sub

        For i = 0 To cboMultiPartGroups.Items.Count - 1
            If NewGroupName = cboMultiPartGroups.Items(i) Then
                MsgBox("Multi-Part Group already exists.", vbOKOnly)
                Exit Sub
            End If
        Next i

        NewGroupName = NewGroupName.Substring(0, IIf(NewGroupName.Length >= 50, 50, NewGroupName.Length))

        cboMultiPartGroups.Items.Add(NewGroupName)
        MatchComboText(cboMultiPartGroups, NewGroupName)

        With fgSchedulingBoard
            .Clear()
            .Rows = 0
            .Cols = 0
        End With
    End Sub

    Private Sub btnMultiPartGroup_Rename_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMultiPartGroup_Rename.Click
        If cboMultiPartGroups.Items.Count > 0 And cboMultiPartGroups.SelectedIndex <> 0 Then

            Dim OldGroupName As String
            Dim NewGroupName As String
            Dim i As Long

            OldGroupName = cboMultiPartGroups.Items(cboMultiPartGroups.SelectedIndex)

EnterGroupName:

            NewGroupName = InputBox("Rename group '" & OldGroupName & "' to:", "Rename Multi-Part Group")

            If NewGroupName = String.Empty Then Exit Sub 'Cancel renaming the group

            NewGroupName = NewGroupName.Substring(0, IIf(NewGroupName.Length >= 50, 50, NewGroupName.Length))

            For i = 0 To cboMultiPartGroups.Items.Count - 1
                If UCase(NewGroupName) = UCase(cboMultiPartGroups.Items(i)) Then
                    MsgBox("Group name '" & NewGroupName & "' already exists.  Please enter another group name.", vbOKOnly, "Multi-Part Group Name Already Exists")
                    GoTo EnterGroupName
                    Exit For
                End If
            Next i

            Dim newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()
            newConn.ExecuteNonQuery("update Avail_MultiPart_Groups set partgroup = " & SQLQuote(NewGroupName) & " where PartGroup = " & SQLQuote(OldGroupName))
            newConn.Close()
            newConn.Dispose()

            cboMultiPartGroups.Items(cboMultiPartGroups.SelectedIndex) = NewGroupName
            cboMultiPartGroups.Sorted = True
            MatchComboText(cboMultiPartGroups, NewGroupName)

        End If
    End Sub

    Private Sub btnMultiPartGroup_Copy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMultiPartGroup_Copy.Click
        Dim NewGroupName As String

        If cboMultiPartGroups.Items.Count > 0 And cboMultiPartGroups.SelectedIndex <> 0 Then
            Dim OldGroupName As String
            Dim i As Long

            OldGroupName = cboMultiPartGroups.Items(cboMultiPartGroups.SelectedIndex)

EnterGroupName:

            NewGroupName = InputBox("Copy group '" & OldGroupName & "' to:", "Rename Multi-Part Group")
            If NewGroupName = "" Then Exit Sub 'Cancel renaming the group

            NewGroupName = NewGroupName.Substring(0, IIf(NewGroupName.Length >= 50, 50, NewGroupName.Length))

            For i = 0 To cboMultiPartGroups.Items.Count - 1
                If UCase(NewGroupName) = UCase(cboMultiPartGroups.Items(i)) Then
                    MsgBox("Group name '" & NewGroupName & "' already exists.  Please enter another group name.", vbOKOnly, "Multi-Part Group Name Already Exists")
                    GoTo EnterGroupName
                    Exit For
                End If
            Next i

            Dim newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()
            newConn.ExecuteNonQuery("Insert Avail_MultiPart_Groups (partgroup, partno, partseq, partactive) select " & SQLQuote(NewGroupName) & ", partno, partseq, partactive from Avail_MultiPart_Groups where partgroup = " & SQLQuote(OldGroupName))
            newConn.Close()
            newConn.Dispose()


        Else

EnterGroupName2:

            NewGroupName = InputBox("New Multi-Part Group name for current parts list:", "Rename Multi-Part Group")
            If NewGroupName = "" Then Exit Sub 'Cancel renaming the group

            NewGroupName = NewGroupName.Substring(0, IIf(NewGroupName.Length >= 50, 50, NewGroupName.Length))

            For i = 0 To cboMultiPartGroups.Items.Count - 1
                If UCase(NewGroupName) = UCase(cboMultiPartGroups.Items(i)) Then
                    MsgBox("Group name '" & NewGroupName & "' already exists.  Please enter another group name.", vbOKOnly, "Multi-Part Group Name Already Exists")
                    GoTo EnterGroupName2
                    Exit For
                End If
            Next i

            Dim sSQL As New StringBuilder
            Dim seqno = 0

            For Each r In dtMultiPartList.Rows
                sSQL.AppendLine("Insert Avail_MultiPart_Groups (partgroup, partno, partseq, partactive) select " & SQLQuote(NewGroupName) & ", " & SQLQuote(r.item("partno")) & ", " & seqno & ", 1")
                seqno += 1
            Next

            Dim newConn = GetOpenedFinesseConnection()
            newConn.ExecuteNonQuery(sSQL)

            MsgBox("New Multi-Part Group has been Created: " & NewGroupName, vbOKOnly)

        End If

        cboMultiPartGroups.Items.Add(NewGroupName)
        cboMultiPartGroups.Sorted = True

        MatchComboText(cboMultiPartGroups, NewGroupName)


    End Sub

    Private Sub btnMultiPartGroup_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMultiPartGroup_Delete.Click
        If cboMultiPartGroups.Items.Count > 0 And (cboMultiPartGroups.SelectedIndex <> 0 Or cboMultiPartGroups.Text <> "(Select a Multi-Part Group...)") Then
            If MsgBox("Are you sure you want to delete Part Group '" & cboMultiPartGroups.Items(cboMultiPartGroups.SelectedIndex) & "'", vbYesNo) = vbNo Then
                Return
            Else

                Dim newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()
                newConn.ExecuteNonQuery("delete from Avail_MultiPart_Groups where partgroup = " & SQLQuote(cboMultiPartGroups.Text))
                newConn.Close()
                newConn.Dispose()

                cboMultiPartGroups.Items.RemoveAt(cboMultiPartGroups.SelectedIndex)
                lstMultiPartList.Items.Clear()
                cboMultiPartGroups.SelectedIndex = 0
            End If

        End If

        With fgSchedulingBoard
            .Clear()
            .Rows = 0
            .Cols = 0
        End With

        dgvDetail.DataSource = Nothing
    End Sub

#End Region

#Region "Query Criteria"


    Private Sub dtpEndDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles dtpEndDate.Validating
        On Error GoTo Message
        Exit Sub
Message:
        MsgBox(dtpEndDate.Value & " is not a valid date.")
    End Sub

    Private Sub dtpEndDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpEndDate.ValueChanged
        If IsNothing(_stateTracker) OrElse _stateTracker.is_changing Then Exit Sub
        'If IsDataLoaded Then RefreshData()
        If dtpStartDate.Value >= dtpEndDate.Value Then dtpStartDate.Value = DateAdd(DateInterval.DayOfYear, -days, dtpEndDate.Value)
        days = DateDiff(DateInterval.DayOfYear, dtpStartDate.Value, dtpEndDate.Value)

        _stateTracker.Change()
    End Sub

    Private Sub dtpStartDate_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles dtpStartDate.Validating
        On Error GoTo Message
        Exit Sub

Message:
        MsgBox(dtpStartDate.Value & " is not a valid date.")
    End Sub

    Private Sub dtpStartDate_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtpStartDate.ValueChanged
        If IsNothing(_stateTracker) OrElse _stateTracker.is_changing Then Exit Sub
        'If IsDataLoaded Then RefreshData()
        If dtpStartDate.Value >= dtpEndDate.Value Then dtpEndDate.Value = DateAdd(DateInterval.DayOfYear, days, dtpStartDate.Value)
        days = DateDiff(DateInterval.DayOfYear, dtpStartDate.Value, dtpEndDate.Value)

        _stateTracker.Change()
    End Sub

    Private Sub chkIncludeProposals_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkIncludeProposals.CheckedChanged
        If IsNothing(_stateTracker) OrElse _stateTracker.is_changing Then Exit Sub
        If Not IsDataLoaded Then Exit Sub
        RefreshAvailabilityData()

        _stateTracker.Change()
    End Sub

    Private Sub chkShowDetail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowDetail.CheckedChanged
        If IsNothing(_stateTracker) OrElse _stateTracker.is_changing Then Exit Sub
        If Not IsDataLoaded Then Exit Sub
        Me.Cursor = Cursors.WaitCursor
        FillSchedulingBoardFlexGrid(dsSchedulingBoardData)
        Me.Cursor = Cursors.Default

        _stateTracker.Change()
    End Sub

    Private Sub chkShowFuture_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowFuture.CheckedChanged
        If IsNothing(_stateTracker) OrElse _stateTracker.is_changing Then Exit Sub
        If Not IsDataLoaded Then Exit Sub
        Me.Cursor = Cursors.WaitCursor
        FillSchedulingBoardFlexGrid(dsSchedulingBoardData)
        Me.Cursor = Cursors.Default

        _stateTracker.Change()
    End Sub

#Region "....Multi-Part Groups"

    Private Sub cboMultiPartGroups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMultiPartGroups.SelectedIndexChanged
        If cboMultiPartGroups.SelectedIndex = 0 Then Return

        'If IsDataLoaded Then RefreshAvailabilityData()
        If IsDataLoaded Then
            RefreshMultiPartListByPartGroup(True)
            'RefreshAvailabilityData()
            RefreshAvailabilityDataWhenIdleFromDirectAction()
        End If
    End Sub

    Private Sub cboMultiPartGroups_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMultiPartGroups.TextChanged
        RefreshMultiPartListByPartGroup(True)
    End Sub

    Private Sub UpdateAvailMultipartGroupsPartSeq(ByVal newConn As SqlConnection)
        Dim cmd As New SqlCommand("Avail_Multipart_Groups_Update_partseq", newConn)
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.Add("@partgroup", SqlDbType.VarChar, 50).Value = cboMultiPartGroups.Text
        cmd.ExecuteNonQuery()
    End Sub

    Private Sub RefreshMultiPartListByPartGroup(ByVal UpdatePartSeq As Boolean)
        If cboMultiPartGroups.SelectedIndex = 0 Then Return
        If IsRefreshingMultiPartList Then Return

        Dim newConn = GetOpenedFinesseConnection() ' As New SqlConnection(FinesseConnectionString)
        'newConn.Open()

        If UpdatePartSeq Then UpdateAvailMultipartGroupsPartSeq(newConn)

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("select * from Avail_Multipart_Groups_Desc where partgroup = " & SQLQuote(cboMultiPartGroups.Text) & " order by partseq asc")
        dtMultiPartList = newConn.GetDataTable(sSQL)
        'newConn.Close()
        'newConn.Dispose()

        Dim strPartNo As String

        lstMultiPartList.Items.Clear()
        IsRefreshingMultiPartList = True

        For Each r As DataRow In dtMultiPartList.Rows
            strPartNo = r.Item("partno") & Space(PartStringSpaces - Len(r.Item("partno"))) & r.Item("partdesc")
            lstMultiPartList.Items.Add(strPartNo, r.Item("partactive"))
        Next

        If lstMultiPartList.CheckedItems.Count > 0 Then
            lstMultiPartList.SelectedItem = lstMultiPartList.CheckedItems(0)
        ElseIf lstMultiPartList.Items.Count > 0 Then
            lstMultiPartList.SelectedItem = lstMultiPartList.Items(0)
        End If

        If lstMultiPartList.SelectedItems.Count >= 1 Then GetSelectedMultiPartListPartNo()

        IsRefreshingMultiPartList = False

        SetActiveInactiveMPListButtons()
    End Sub

    Private Sub RefreshMultipartListWithJobTypes()

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("SELECT p.jobtype, p.jobdesc FROM dbo.pejob p WHERE p.isActive = 1 AND p.is_qualification = 1")

        dtMultiPartList = GetOpenedFinesseConnection().GetDataTable(sSQL)

        Dim strPartNo As String
        Dim PartStringSpaces = 2

        If dtMultiPartList.Rows.Count > 0 Then
            PartStringSpaces = (From row As DataRow In dtMultiPartList.Rows
                                Select partnoLength = row.Item("jobtype").ToString.Length).Max + 2
        End If

        lstMultiPartList.Items.Clear()

        IsRefreshingMultiPartList = True

        Dim appConfigSelectedJobTypes = Split(Replace(Get_AppConfigSetting(AppConfig, "JobTypes"), "'", ""), ",")
        Dim iFirstFoundCheckedItem = -1

        For Each r As DataRow In dtMultiPartList.Rows
            strPartNo = r.Item("jobtype") & Space(PartStringSpaces - Len(r.Item("jobtype"))) & r.Item("jobdesc")
            lstMultiPartList.Items.Add(strPartNo, If(appConfigSelectedJobTypes.Contains(r.Item("jobtype")), True, False))
            If appConfigSelectedJobTypes.Contains(r.Item("jobtype")) And iFirstFoundCheckedItem = -1 Then iFirstFoundCheckedItem = lstMultiPartList.Items.Count - 1
        Next

        txtPartNo.Text = dtMultiPartList.Rows(0).Item("jobtype")
        txtPartDesc.Text = dtMultiPartList.Rows(0).Item("jobdesc")

        If iFirstFoundCheckedItem <> -1 Then
            lstMultiPartList.SelectedIndex = iFirstFoundCheckedItem
            GetSelectedMultiPartListPartNo()
            RefreshPartWarehouseQtys()
        End If

        IsRefreshingMultiPartList = False

        SetActiveInactiveMPListButtons()

    End Sub

    Private Sub RefreshMultiPartListByPartList(ByVal strPartNos As String)
        Dim newConn = GetOpenedFinesseConnection()
        'newConn.Open()

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("select partno, partdesc from inpart where partno in (select string from fn_split(" & SQLQuote(strPartNos) & ",',')) order by partdesc asc")
        dtMultiPartList = newConn.GetDataTable(sSQL)
        'newConn.Close()
        'newConn.Dispose()

        Dim strPartNo As String
        Dim PartStringSpaces = 2

        If dtMultiPartList.Rows.Count > 0 Then
            PartStringSpaces = (From row As DataRow In dtMultiPartList.Rows
                                Select partnoLength = row.Item("partno").ToString.Length).Max + 2
        End If

        lstMultiPartList.Items.Clear()
        IsRefreshingMultiPartList = True

        For Each r As DataRow In dtMultiPartList.Rows
            strPartNo = r.Item("partno") & Space(PartStringSpaces - Len(r.Item("partno"))) & r.Item("partdesc")
            lstMultiPartList.Items.Add(strPartNo, True)
        Next

        IsRefreshingMultiPartList = False

        'If lstMultiPartList.Items.Count > 0 Then
        '    For i = 0 To lstMultiPartList.Items.Count - 1
        '        If InStr(1, lstMultiPartList.Items(i), txtPartNo, vbTextCompare) Then lstMultiPartList.SelectedItem(i) = True
        '    Next i
        'End If

        SetActiveInactiveMPListButtons()
        'RefreshData() ' This is the wrong place to do this!
    End Sub

    Private Sub SetActiveInactiveMPListButtons()

        If lstMultiPartList.Items.Count > 0 Then
            If argStartupMode <> "Crew" Then
                If lstMultiPartList.SelectedIndex = 0 Then btnMultiPartList_MoveUp.Enabled = False Else btnMultiPartList_MoveUp.Enabled = True
                If lstMultiPartList.SelectedIndex = lstMultiPartList.Items.Count - 1 Then btnMultiPartList_MoveDown.Enabled = False Else btnMultiPartList_MoveDown.Enabled = True
                btnMultiPartList_Delete.Enabled = True
            End If
        Else
            btnMultiPartList_MoveUp.Enabled = False
            btnMultiPartList_MoveDown.Enabled = False
            btnMultiPartList_Delete.Enabled = False
        End If
    End Sub

    Private Sub lstMultiPartList_ItemCheck(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckEventArgs) Handles lstMultiPartList.ItemCheck
        If IsNothing(_stateTracker) OrElse _stateTracker.is_changing Then Exit Sub
        If IsRefreshingMultiPartList = True Then Exit Sub

        IsRefreshingMultiPartList = True
        lstMultiPartList.SetItemChecked(e.Index, Not lstMultiPartList.GetItemChecked(e.Index))
        IsRefreshingMultiPartList = False

        'NowItemCheckedValueIsActuallyWhatTheUserChecked:

        Dim SQLNewCheckedValue As Integer

        If lstMultiPartList.GetItemChecked(e.Index) = True Then
            SQLNewCheckedValue = 1
        Else
            SQLNewCheckedValue = 0
        End If

        'Application.DoEvents()
        'Dim SelectedPart = GetSelectedMultiPartListPartNo() 'Don't do this because SelectedIndexChanged is also fired when an item is checked or unchecked.

        'Dim SelectedPart = lstMultiPartList.SelectedItem.ToString.Substring(0, InStr(1, lstMultiPartList.SelectedItem.ToString, "  ", vbTextCompare) - 1)
        Dim SelectedPart = _stateTracker.GetProp(txtPartNo.Name, "Text")

        Dim newConn As New SqlConnection(FinesseConnectionString)
        newConn.Open()

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("Update Avail_Multipart_Groups")
        sSQL.AppendLine(" Set partactive = " & SQLNewCheckedValue)
        sSQL.AppendLine(" Where PartGroup = " & SQLQuote(cboMultiPartGroups.Text))
        sSQL.AppendLine(" and partno = " & SQLQuote(SelectedPart))
        newConn.ExecuteNonQuery(sSQL)
        newConn.Close()
        newConn.Dispose()


        Select Case CurrentFlexGrid().Name
            Case fgSchedulingBoard.Name
                'RefreshAvailabilityData()
                RefreshAvailabilityDataWhenIdleFromDirectAction()
            Case fgTimeline.Name
                'FillTimelineFlexGrid(dsSchedulingBoardData) 'Don't do this because SelectedIndexChanged is also fired when an item is checked or unchecked.
            Case fgCalendar.Name
                'RefreshAvailabilityData() 'Don't do this because SelectedIndexChanged is also fired when an item is checked or unchecked.
        End Select


        _stateTracker.Change()
        '       CurrentFlexGrid.Focus()
    End Sub

    Private Sub lstMultiPartList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstMultiPartList.SelectedIndexChanged
        If IsRefreshingMultiPartList = True Then Return
        If lstMultiPartList.SelectedItems.Count = 0 Then Return

        dgvDetail.DataSource = Nothing
        SetActiveInactiveMPListButtons()
        'Me.Focus()

        Application.DoEvents()
        strFirstVisiblePartNo = GetSelectedMultiPartListPartNo()

        Select Case CurrentFlexGrid().Name
            Case fgSchedulingBoard.Name
                If lstMultiPartList.GetItemCheckState(lstMultiPartList.SelectedIndex) Then
                    ResetScrollbarsToFirstVisiblePartnoAndFirstVisibleColumn()
                End If
            Case fgTimeline.Name
                FillTimelineFlexGrid(dsSchedulingBoardData)
            Case fgCalendar.Name
                If Not _suspendRefreshData Then
                    'RefreshAvailabilityData()
                    RefreshAvailabilityDataWhenIdleFromDirectAction()
                End If
        End Select
        'CurrentFlexGrid.Focus()
    End Sub

    Private Sub btnMultiPartList_MoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMultiPartList_MoveUp.Click
        MultiPartList_MoveItem(-1)
    End Sub

    Private Sub btnMultiPartList_MoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMultiPartList_MoveDown.Click
        MultiPartList_MoveItem(1)
    End Sub

    Private Sub MultiPartList_MoveItem(ByVal Direction As Integer)
        Dim strItemText As String = String.Empty
        Dim strPartno As String = String.Empty
        Dim SelectedPartSeq As Integer

        If lstMultiPartList.SelectedIndex = -1 Then Exit Sub

        'SelectedPartSeq = lstMultiPartList.SelectedIndex + 1

        _suspendRefreshData = True

        strItemText = lstMultiPartList.GetItemText(lstMultiPartList.SelectedItem)
        strPartno = GetSelectedMultiPartListPartNo()

        SelectedPartSeq = dtMultiPartList.Select("partno=" & SQLQuote(strPartno))(0)("partseq")

        Dim newConn = GetOpenedFinesseConnection()
        'newConn.Open()

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("Update Avail_MultiPart_Groups Set partseq = '0' where partgroup=" & SQLQuote(cboMultiPartGroups.Text) & " and partseq = " & SQLQuote(SelectedPartSeq + Direction))
        sSQL.AppendLine("Update Avail_MultiPart_Groups Set partseq = " & SQLQuote(SelectedPartSeq + Direction) & " where partgroup=" & SQLQuote(cboMultiPartGroups.Text) & " and partseq = " & SQLQuote(SelectedPartSeq))
        sSQL.AppendLine("Update Avail_MultiPart_Groups Set partseq = " & SQLQuote(SelectedPartSeq) & " where partgroup=" & SQLQuote(cboMultiPartGroups.Text) & " and partseq = '0'")
        newConn.ExecuteNonQuery(sSQL)
        'newConn.Close()
        'newConn.Dispose()

        RefreshMultiPartListByPartGroup(False)

        For Each i In lstMultiPartList.Items
            If InStr(lstMultiPartList.GetItemText(i), strPartno) Then
                lstMultiPartList.SelectedItem = i
                Exit For
            End If
        Next

        _suspendRefreshData = False

    End Sub

    Private Sub btnMultiPartList_Add_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMultiPartList_Add.Click
        'If cboMultiPartGroups.SelectedIndex = 0 Then
        '    MsgBox("A Multi-Part Group must be selected before adding a part to the list.")
        '    Exit Sub
        'End If

        Static myPartPicker As New PartPicker(Me, btnMultiPartList_Add, FinesseConnectionString)
        'If Not String.IsNullOrEmpty(txtPartNo.Text) Then myPartPicker.DefaultSearch = txtPartNo.Text.Trim
        If Not myPartPicker.GetPart() Then Exit Sub
        Dim partno = myPartPicker.PartNumber
        'AddPartToMultipartList_LastSearchString = 

        Dim i As Long
        For i = 0 To lstMultiPartList.Items.Count - 1
            If partno = Split(lstMultiPartList.Items(i), " ")(0) Then
                MsgBox("Part '" & partno & "' already exist in the Multi-Part Group.", vbOKOnly)
                Exit Sub
            End If
        Next i

        If cboMultiPartGroups.SelectedIndex <> 0 Then
            Try
                Dim newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()

                Dim sSQL As New StringBuilder
                sSQL.AppendLine(";with n as (")
                sSQL.AppendLine("select partgroup = " & SQLQuote(cboMultiPartGroups.Text) & ", partno = " & SQLQuote(partno) & ", partseq = 0")
                sSQL.AppendLine(")")
                sSQL.AppendLine("Insert Avail_Multipart_Groups (partgroup, partno, partseq)")
                sSQL.AppendLine("Select")
                sSQL.AppendLine("   n.partgroup,")
                sSQL.AppendLine("   n.partno,")
                sSQL.AppendLine("   partseq = isnull(max(amg.partseq) + 1,n.partseq)")
                sSQL.AppendLine("from n")
                sSQL.AppendLine("left outer join dbo.Avail_Multipart_Groups amg on amg.partgroup = " & SQLQuote(cboMultiPartGroups.Text))
                sSQL.AppendLine("group by n.partgroup, n.partno, n.partseq, amg.partgroup")


                newConn.ExecuteNonQuery(sSQL)
                newConn.Close()
                newConn.Dispose()

                RefreshMultiPartListByPartGroup(True)

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
        Else
            Dim strPartNo = myPartPicker.PartNumber & Space(PartStringSpaces - Len(myPartPicker.PartNumber)) & myPartPicker.PartDesc
            lstMultiPartList.Items.Add(strPartNo, True)
            Dim r = dtMultiPartList.NewRow()
            r.Item("partno") = myPartPicker.PartNumber
            r.Item("partdesc") = myPartPicker.PartDesc
            dtMultiPartList.Rows.Add(r)

            RefreshAvailabilityData()
            RefreshPartData(GetCurrentPartNo)
        End If

    End Sub

    Private Sub btnMultiPartList_Delete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMultiPartList_Delete.Click
        If lstMultiPartList.SelectedIndex = -1 Then Exit Sub

        Dim SelectedPartNo = GetSelectedMultiPartListPartNo()

        Dim newConn As New SqlConnection(FinesseConnectionString)
        newConn.Open()

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("delete from Avail_MultiPart_Groups")
        sSQL.AppendLine("where partgroup = " & SQLQuote(cboMultiPartGroups.Text))
        sSQL.AppendLine("and partno = " & SQLQuote(SelectedPartNo))
        newConn.ExecuteNonQuery(sSQL)

        newConn.Close()
        newConn.Dispose()

        RefreshMultiPartListByPartGroup(True)
    End Sub

#End Region

#End Region

#Region "Refresh Data"

    Private Sub tsbRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Not IsDataLoaded Then Exit Sub
        RefreshAvailabilityData()
    End Sub

    Public Sub RefreshPartData(ByVal partno As String)
        If _suspendRefreshData Then Return
        If Not IsNothing(_stateTracker) AndAlso _stateTracker.is_changing Then Exit Sub

        Application.DoEvents()
        RefreshCurrentPartAttributes(partno)

        Application.DoEvents()
        If argStartupMode <> "Crew" Then RefreshPartWarehouseQtys()

        Application.DoEvents()
        If argStartupMode <> "Crew" Then RefreshPartScheduleDetails(CurrentFlexGrid, Nothing)
    End Sub

    Private Sub RefreshPartWarehouseQtys()

        If dtCurrentPart Is Nothing Then
            RefreshCurrentPartAttributes(txtPartNo.Text.Trim)
        End If

        If dtCurrentPart Is Nothing OrElse dtCurrentPart.Rows.Count = 0 Then Return

        Dim partno As String = dtCurrentPart.Rows(0).Item("partno")

        Dim sql As String = String.Empty
        If argStartupMode = "Crew" Then
            sql = "exec SchedulingBoard_PopulateDivisionQtys " & partno.SQLQuote
        Else
            sql = "EXECUTE AS USER = 'sa_jheitmann' exec SchedulingBoard_PopulateWarehouseQtys " & partno.SQLQuote & " revert"
        End If

        'Using newConn As New SqlConnection(FinesseConnectionString)
        'newConn.Open()
        Dim t = GetOpenedFinesseConnection().GetDataTable(sql)
        PopulateWarehouseQuantities(t)
        'End Using
    End Sub

    Private Sub PopulateWarehouseQuantities(ByVal dt As DataTable)

        Dim divisionValueMember = "WH"
        Dim strQty = "qty"
        Dim strChildrenOrSubs = "children"

        If argStartupMode = "Crew" Then
            divisionValueMember = "groupno"
            strQty = "Employed"
            strChildrenOrSubs = "Subs"
        End If

        Dim inventory = dt.Compute("sum(" & strQty & ")", divisionValueMember & " not in ('WEXLER', 'SUB')")
        Dim childrenOrSubs = dt.Compute("sum(" & strChildrenOrSubs & ")", divisionValueMember & " not in ('WEXLER', 'SUB')")

        If childrenOrSubs Is DBNull.Value Then
            childrenOrSubs = 0
        End If

        If inventory Is DBNull.Value Then
            inventory = 0
        End If

        lblTotal.Text = "Total: " & inventory + childrenOrSubs
        lblInventory.Text = If(argStartupMode = "Crew", "Employed", "Inventory") & ": " & inventory
        lblChildren.Text = If(argStartupMode = "Crew", "Subs", "Children") & ": " & childrenOrSubs

        For Each pnl As Panel In pnlWarehouses.Controls.OfType(Of Panel)
            If pnl.Visible = True Then

                Dim division_code = pnl.Tag

                Dim qtyRows = dt.Select(divisionValueMember & "=" & SQLQuote(division_code))
                Dim rDivision As DataRow

                If argStartupMode = "Crew" Then
                    rDivision = _dtDivisions.Select("division=" & SQLQuote(division_code))(0)
                Else
                    rDivision = _dtWarehouses.Select("warehouse_code=" & SQLQuote(division_code))(0)
                End If

                If Not qtyRows.Any Then
                    For Each c As Control In pnl.Controls
                        If c.Tag = "qty" Then
                            c.Text = If(argStartupMode = "Crew", GetDivisionCheckboxLabel(rDivision, ""), GetWarehouseCheckboxLabel(rDivision, ""))
                        ElseIf c.Tag = "desc" Then
                            ' don't hide the warehouse description
                        Else
                            c.Visible = False
                        End If
                    Next

                    Continue For
                End If

                Dim qtyRow As DataRow = qtyRows(0)

                For Each c As Control In pnl.Controls
                    c.Font = New Font(c.Font, FontStyle.Regular)

                    Select Case c.Tag
                        Case "qty"
                            Dim qtyTxt = ""
                            Dim qty As Long = qtyRow.Item(strQty)
                            Dim qtyChildren As Long = qtyRow.Item(strChildrenOrSubs)
                            Dim qtyTotal = qty + qtyChildren
                            If qtyTotal > 0 Then
                                qtyTxt = qty & IIf(qtyChildren > 0, "+" & qtyChildren, "")
                                c.ForeColor = DefaultForeColor
                            Else
                                c.ForeColor = Color.Gray
                            End If

                            c.Text = If(argStartupMode = "Crew", GetDivisionCheckboxLabel(rDivision, qtyTxt), GetWarehouseCheckboxLabel(rDivision, qtyTxt))
                        Case "repair"
                            If argStartupMode <> "Crew" AndAlso qtyRows.Any AndAlso qtyRow.Item("repair") + qtyRow.Item("childRepair") > 0 Then
                                c.Text = $"{qtyRow.Item("repair")}{If(qtyRow.Item("childRepair") > 0, $"+{qtyRow.Item("childRepair")}", "")}"
                                c.Font = New Font(c.Font, FontStyle.Bold)
                                c.Visible = True
                            Else
                                c.Visible = False
                            End If
                        Case "late"
                            If argStartupMode <> "Crew" AndAlso qtyRow.Item("late") Then
                                c.Text = qtyRow.Item("late")
                                c.Font = New Font(c.Font, FontStyle.Bold)
                                c.Visible = True
                            Else
                                c.Visible = False
                            End If
                        Case "planned"
                            If argStartupMode <> "Crew" AndAlso qtyRow.Item("planned") Then
                                c.Text = qtyRow.Item("planned")
                                c.Font = New Font(c.Font, FontStyle.Bold)
                                c.Visible = True
                            Else
                                c.Visible = False
                            End If
                    End Select
                Next

            End If
        Next
    End Sub

    Private Sub RefreshCurrentPartAttributes(ByVal partno As String)

        Static lastPartno As String

        If argStartupMode = "Crew" Then

            Dim sSQL As New StringBuilder
            sSQL.AppendLine("SELECT partno = p.jobtype, partdesc = p.jobdesc, parttotal = 0 FROM dbo.pejob p where p.jobtype=" & partno.SQLQuote)
            dtCurrentPart = GetOpenedFinesseConnection().GetDataTable(sSQL)

        Else

            If partno = lastPartno Then Return

            Dim sSQL As New StringBuilder
            sSQL.AppendLine("select i.partno, i.partdesc, parttotal=isnull(sum(ips.onhand),0)")
            sSQL.AppendLine(", partlength=i.length_uom * u.conversionFactorFromInches")
            sSQL.AppendLine(", partwidth=i.width_uom * u.conversionFactorFromInches")
            sSQL.AppendLine(", partheight=i.depth_uom * u.conversionFactorFromInches")
            sSQL.AppendLine(", u.unitoflength")
            sSQL.AppendLine("")
            sSQL.AppendLine(", partweight=(i.projpercent + isnull(cp.childrenweight,0)) * u.conversionFactorFromPounds")
            sSQL.AppendLine(", u.unitofweight")
            sSQL.AppendLine("")
            sSQL.AppendLine(", est_unit_amount=c.totmatcost4 + isnull(cp.childrenvalue,0)")
            sSQL.AppendLine(", bid_value=pbv.bidvalue")
            sSQL.AppendLine(", daily_rate = pbv2.bidvalue")
            sSQL.AppendLine(", u.currency")
            sSQL.AppendLine("from inpart i")
            sSQL.AppendLine("join incostpart c on i.partno=c.partno")
            sSQL.AppendLine("cross apply (")
            sSQL.AppendLine("select mui.currency , mui.unitofweight ,mui.conversionFactorFromPounds , mui.WeightDecimalPlaces , mui.unitoflength , mui.conversionFactorFromInches , mui.LengthDecimalPlaces  from dbo.my_user_info mui")
            sSQL.AppendLine(") u")
            sSQL.AppendLine("left outer join inpartsub ips on i.partno = ips.partno and ips.bld not in ('SUB','WEXLER')")
            sSQL.AppendLine("left outer join dbo.PartBidValues pbv on i.partno = pbv.partno and pbv.valuetype = 'Standard' and pbv.currency = u.currency")
            sSQL.AppendLine("left outer join dbo.PartBidValues pbv2 on i.partno = pbv2.partno and pbv2.valuetype = 'Dry Hire' and pbv2.currency = u.currency")
            sSQL.AppendLine("outer apply (")
            sSQL.AppendLine(" select childrenweight = sum(cp.projpercent * ir.Factor), childrenvalue = sum(cpc.totmatcost4 * ir.Factor)")
            sSQL.AppendLine(" from dbo.inpart_Reference ir ")
            sSQL.AppendLine(" join inpart cp on ir.partno = cp.partno")
            sSQL.AppendLine(" join incostpart cpc on cp.partno = cpc.partno")
            sSQL.AppendLine(" where ir.parentpartno = i.partno and ir.id_ReferenceType in (3,4)")
            sSQL.AppendLine(") cp")
            sSQL.AppendLine("where i.partno='" & partno & "'")
            sSQL.AppendLine("group by i.partno, i.partdesc, i.length_uom, i.width_uom, i.depth_uom, u.conversionFactorFromInches, u.unitoflength, i.projpercent, u.conversionFactorFromPounds, u.unitofweight, c.totmatcost4, pbv.bidvalue, pbv2.bidvalue, cp.childrenvalue, cp.childrenweight, u.currency")

            'Using newConn As New SqlConnection(FinesseConnectionString)
            'newConn.Open()
            dtCurrentPart = GetOpenedFinesseConnection().GetDataTable(sSQL)
            'End Using

            With dtCurrentPart.Rows(0)
                sslblLength.Text = "L (" & .Item("unitoflength") & "):"
                sslblLengthValue.Text = FormatNumber(ReplaceNull(.Item("partLength"), 0.0), 1)
                sslblWidth.Text = "W (" & .Item("unitoflength") & "):"
                sslblWidthValue.Text = FormatNumber(ReplaceNull(.Item("partWidth"), 0.0), 1)
                sslblHeight.Text = "H (" & .Item("unitoflength") & "):"
                sslblHeightValue.Text = FormatNumber(ReplaceNull(.Item("partHeight"), 0.0), 1)
                sslblWeight.Text = "Weight (" & .Item("unitofweight") & "):"
                sslblWeightValue.Text = FormatNumber(ReplaceNull(.Item("partWeight"), 0.0), 1)
                sslblRentalValue.Text = "Rental Value (" & .Item("currency") & "):"
                sslblRentalValueValue.Text = FormatNumber(.Item("est_unit_amount"), 2)
                sslblWeeklyRate.Text = "Weekly Rate 1% (" & .Item("currency") & "):"
                sslblWeeklyRateValue.Text = FormatNumber(ReplaceNull(.Item("bid_value"), 0) * 0.01, 0)
                sslbl2DayWeekRate.Text = "2-Day Week (" & .Item("currency") & "):"
                sslbl2DayWeekRateValue.Text = FormatNumber(ReplaceNull(.Item("daily_rate"), 0) * 2.0, 0)
                sslblDailyRate.Text = "Daily Rate (" & .Item("currency") & "):"
                sslblDailyRateValue.Text = FormatNumber(ReplaceNull(.Item("daily_rate"), 0), 0)
            End With

        End If

        txtPartNo.Text = dtCurrentPart.Rows(0).Item("partno")
        txtPartDesc.Text = dtCurrentPart.Rows(0).Item("partdesc")

        HighlightCurrentPartInMultiPartList()

        lastPartno = partno
    End Sub

    Private Sub HighlightCurrentPartInMultiPartList()
        IsRefreshingMultiPartList = True 'Temporarily suspend availability refreshes

        Dim found = lstMultiPartList.FindString(dtCurrentPart.Rows(0).Item("partno") & Space(PartStringSpaces - Len(dtCurrentPart.Rows(0).Item("partno"))))

        If found >= 0 Then
            lstMultiPartList.SelectedItem = lstMultiPartList.Items(found)
        End If

        IsRefreshingMultiPartList = False
    End Sub

    Private _swLastDirectAction As Stopwatch = Stopwatch.StartNew
    Private Const _DefaultIdleExtensionMS As Integer = 2000

    ' the idea behind this Timer & MouseMove crap is to prevent refreshes while a 
    ' user is checking and unchecking boxes. If they're still moving the mouse or tabbing,
    ' keep deferring the refresh (up to another two seconds.) When they actually
    ' check/uncheck a box, they will incur another two seconds of delay during mouse moves.
    Private Sub MaybeExtendDurationOfIdleAvailabilityRefresh(Optional idleExtensionMS As Integer = _DefaultIdleExtensionMS)
        If _swLastDirectAction.ElapsedMilliseconds < idleExtensionMS AndAlso timer_RefreshData.Enabled Then
            RefreshAvailabilityDataWhenIdle()
        End If
    End Sub

    Private Sub RefreshAvailabilityDataWhenIdleFromDirectAction()
        _swLastDirectAction = Stopwatch.StartNew
        RefreshAvailabilityDataWhenIdle()
    End Sub

    Private Sub RefreshAvailabilityDataWhenIdle()
        timer_RefreshData.Stop()
        timer_RefreshData.Start()
    End Sub

    Private Sub availablityControl_NavigationEventHandler(sender As Object, e As EventArgs)
        MaybeExtendDurationOfIdleAvailabilityRefresh()
    End Sub

    Private Sub pnlWarehouses_Scroll(sender As Object, e As ScrollEventArgs) Handles pnlWarehouses.Scroll
        MaybeExtendDurationOfIdleAvailabilityRefresh(idleExtensionMS:=5000)
    End Sub

    Private Sub lvWarehouseGroups_Scroll(sender As Object, e As EventArgs) Handles lvWarehouseGroups.MouseMove
        MaybeExtendDurationOfIdleAvailabilityRefresh(idleExtensionMS:=5000)
    End Sub

    Private Sub timer_RefreshData_Tick(sender As Object, e As EventArgs) Handles timer_RefreshData.Tick
        timer_RefreshData.Stop()
        RefreshAvailabilityData()
    End Sub




    Public Sub RefreshAvailabilityData()
        If IsNothing(_stateTracker) OrElse _stateTracker.is_changing Then Exit Sub
        If Not IsDataLoaded Then Return
        If _suspendRefreshData Then Return

        Dim strIncludedWarehouses As String = GetStrIncludedWarehouses(True, False)
        Dim strIncludedParts As String = GetStrIncludedParts()

        If strIncludedWarehouses = "''" Then Return

        Dim sqlStartDate As DateTime = dtpStartDate.Value
        Dim sqlEndDate As DateTime = dtpEndDate.Value
        Dim include_proposals As Integer = chkIncludeProposals.CheckState

        Dim selected_flexgrid As AxMSFlexGridLib.AxMSFlexGrid = Nothing

        Dim cmdSQL As New SqlCommand()
        cmdSQL.CommandType = CommandType.StoredProcedure

        Select Case tcMain.SelectedTab.Name
            Case tpSchedulingBoard.Name
                selected_flexgrid = fgSchedulingBoard

                If chkSummarize.Checked Then
                    cmdSQL.CommandText = "Scheduling_Board_get_availability_summarized"
                Else
                    cmdSQL.CommandText = "Scheduling_Board_get_availability"
                End If

                cmdSQL.Parameters.Add("@warehouses", SqlDbType.VarChar).Value = Replace(strIncludedWarehouses, "'", "")
                cmdSQL.Parameters.Add("@partnos", SqlDbType.VarChar).Value = Replace(strIncludedParts, "'", "")
                cmdSQL.Parameters.Add("@start_date", SqlDbType.DateTime).Value = DateAdd(DateInterval.Day, -2, sqlStartDate)
                cmdSQL.Parameters.Add("@end_date", SqlDbType.DateTime).Value = DateAdd(DateInterval.Day, 2, sqlEndDate)
                cmdSQL.Parameters.Add("@include_proposals", SqlDbType.Bit).Value = chkIncludeProposals.CheckState

            Case tpTimeline.Name
                selected_flexgrid = fgTimeline

                If argStartupMode = "Crew" Then

                    cmdSQL.CommandText = "Scheduling_Board_get_crew_availability"

                    cmdSQL.Parameters.Add("@divisions", SqlDbType.VarChar).Value = Replace(strIncludedWarehouses, "'", "")
                    cmdSQL.Parameters.Add("@jobtypes", SqlDbType.VarChar).Value = Replace(GetStrIncludedJobTypes, "'", "")
                    cmdSQL.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = DateAdd(DateInterval.Day, -2, sqlStartDate)
                    cmdSQL.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = DateAdd(DateInterval.Day, 2, sqlEndDate)

                Else
                    cmdSQL.CommandText = "pjinvcalproc"
                    cmdSQL.Parameters.Add("@warehouse", SqlDbType.VarChar).Value = Replace(strIncludedWarehouses, "'", "")
                    cmdSQL.Parameters.Add("@partno", SqlDbType.VarChar).Value = Replace(strIncludedParts, "'", "")
                    cmdSQL.Parameters.Add("@start_date", SqlDbType.DateTime).Value = DateAdd(DateInterval.Day, -2, sqlStartDate)
                    cmdSQL.Parameters.Add("@end_date", SqlDbType.DateTime).Value = DateAdd(DateInterval.Day, 2, sqlEndDate)
                End If

            Case tpCalendar.Name
                selected_flexgrid = fgCalendar

                Dim StartDateFirstDayOfWeek = sqlStartDate.AddDays(-Weekday(sqlStartDate, FirstDayOfWeek.System) + 1)
                Dim EndDateLastDayOfWeek = sqlEndDate.AddDays(-Weekday(sqlEndDate, FirstDayOfWeek.System) + 7)

                If argStartupMode = "Crew" Then
                    cmdSQL.CommandText = "Scheduling_Board_get_crew_availability"

                    cmdSQL.Parameters.Add("@divisions", SqlDbType.VarChar).Value = Replace(strIncludedWarehouses, "'", "")
                    cmdSQL.Parameters.Add("@jobtypes", SqlDbType.VarChar).Value = dtCurrentPart.Rows(0).Item("partno")
                    cmdSQL.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = DateAdd(DateInterval.Day, -2, sqlStartDate)
                    cmdSQL.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = DateAdd(DateInterval.Day, 2, sqlEndDate)
                Else
                    cmdSQL.CommandText = "pjinvcalproc"
                    cmdSQL.Parameters.Add("@warehouse", SqlDbType.VarChar).Value = Replace(strIncludedWarehouses, "'", "")
                    cmdSQL.Parameters.Add("@partno", SqlDbType.VarChar).Value = dtCurrentPart.Rows(0).Item("Partno")
                    cmdSQL.Parameters.Add("@start_date", SqlDbType.DateTime).Value = StartDateFirstDayOfWeek
                    cmdSQL.Parameters.Add("@end_date", SqlDbType.DateTime).Value = EndDateLastDayOfWeek
                End If

            Case Else
                Return
        End Select

        If String.IsNullOrEmpty(Replace(strIncludedParts, "'", "")) Then
            'MsgBox("No parts have been selected.")
            selected_flexgrid.Clear()
            selected_flexgrid.Rows = 0
            selected_flexgrid.Cols = 0
            Return
        End If

        Me.Cursor = Cursors.WaitCursor

        Dim newConn = GetOpenedFinesseConnection() ' As New SqlConnection(FinesseConnectionString)
        'newConn.Open()

        cmdSQL.Connection = newConn
        cmdSQL.CommandTimeout = 60

        'Dim ds As New DataSet
        dsSchedulingBoardData.Reset()
        Using a As New SqlDataAdapter(cmdSQL)
            a.Fill(dsSchedulingBoardData)
        End Using

        'newConn.Close()
        'newConn.Dispose()

        Select Case tcMain.SelectedTab.Name
            Case tpSchedulingBoard.Name
                FillSchedulingBoardFlexGrid(dsSchedulingBoardData)
            Case tpTimeline.Name
                FillTimelineFlexGrid(dsSchedulingBoardData)
            Case tpCalendar.Name
                FillCalendarFlexGrid(dsSchedulingBoardData)
        End Select

        IsDataLoaded = True
        Me.Cursor = Cursors.Default
    End Sub

#End Region

#Region "Flexgrid Events"



    Private Shared Function NonEditableCellColor() As Color
        Return Color.Beige
    End Function

    Private Sub FillTimelineFlexGrid(ByVal ds As DataSet)
        If Not IsDataLoaded Then Return

        If dsSchedulingBoardData.Tables.Count = 0 Then
            'MsgBox("No parts have been selected.")
            Return
        End If

        Dim resourceValueMember = "partno"
        Dim resourceDisplayMember = "partdesc"
        Dim divisionValueMember = "bld"
        Dim divisionDisplayMember = "warehouse_description"

        If argStartupMode = "Crew" Then
            resourceValueMember = "jobtype"
            resourceDisplayMember = "jobdesc"
            divisionValueMember = "division"
            divisionDisplayMember = "divdesc"
        End If

        Dim sWarehouses = Replace(GetStrIncludedWarehouses(True, False), "'", "")
        Dim aryWarehouses = sWarehouses.Split(",")

        If argStartupMode = "Crew" Then aryWarehouses = (From r As DataRow In ds.Tables(0).Rows
                                                         Select CStr(r.Item(divisionValueMember))
                                                         Distinct).ToArray

        If String.IsNullOrEmpty(GetStrIncludedWarehouses(True, False)) Then
            fgCalendar.Rows = 0
            Return
        End If

        Try
            Dim dt = ds.Tables(0)

            fgTimeline.Redraw = False
            'fgTimeline.Visible = False
            fgTimeline.ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarBoth

            fgTimeline.Clear()
            fTimelineCellInfoList.Clear()

            Dim aryParts = Replace(GetStrIncludedParts.ToString, "'", "").Split(",")

            If argStartupMode = "Crew" Then aryParts = Replace(GetStrIncludedJobTypes.ToString, "'", "").Split(",")

            'Dim aryParts = From r As DataRow In ds.Tables(0).Rows
            '               Select CStr(r.Item(resourceValueMember))
            '               Distinct

            Dim Dates = From r As DataRow In ds.Tables(0).Rows
                        Select r.Item("date")

            Dim minDate = CDate(Dates.Min)
            Dim maxDate = CDate(Dates.Max)

            fgTimeline.Cols = DateDiff(DateInterval.Day, minDate, maxDate) + 1 + 3
            If chkSummarize.Checked Then
                fgTimeline.Rows = aryParts.Count + 1
            Else
                fgTimeline.Rows = (aryParts.Count * aryWarehouses.Count) + 1
            End If
            fgTimeline.FixedCols = 3
            fgTimeline.FixedRows = 1
            fgTimeline.set_ColWidth(0, 1500)

            fgTimeline.set_TextMatrix(0, 0, If(argStartupMode = "Crew", "DIV", "WH"))
            fgTimeline.set_TextMatrix(0, 1, If(argStartupMode = "Crew", "Job Type", "Part #"))
            fgTimeline.set_TextMatrix(0, 2, If(argStartupMode = "Crew", "Job Description", "Part Description"))

            Dim strParts As String
            strParts = String.Join(",", aryParts)

            Dim curPartCount As Integer = -1
            Dim curWarehouseCount As Integer = -1
            Dim curAvailRecordAbsolutePosition As Integer = -1

            Dim Avail_Array(0 To aryWarehouses.Count, 0 To aryParts.Count, 0 To 10000)


            curWarehouseCount = 0
            For Each w In aryWarehouses
                curWarehouseCount = curWarehouseCount + 1

                curPartCount = 0
                For Each p In aryParts
                    curPartCount = curPartCount + 1

                    curAvailRecordAbsolutePosition = -1

                    Dim CurRow As Long
                    If chkSummarize.Checked Then
                        CurRow = curPartCount
                    Else
                        CurRow = curPartCount + aryParts.Count * (curWarehouseCount - 1)
                    End If

                    'Label the fixed columns
                    If chkSummarize.Checked Then
                        fgTimeline.set_TextMatrix(CurRow, 0, "SUM")
                    Else
                        fgTimeline.set_TextMatrix(CurRow, 0, w)
                    End If
                    fgTimeline.set_TextMatrix(CurRow, 1, p)
                    If Not dtMultiPartList Is Nothing Then
                        fgTimeline.set_TextMatrix(CurRow, 2, dtMultiPartList.Select(resourceValueMember & "=" & p.SQLQuote)(0)(resourceDisplayMember))
                    End If

                    Dim avail As Long
                    Dim CurDate As Date

                    Dim WHAvail = dt.Select(divisionValueMember & "=" & w.SQLQuote & " and " & resourceValueMember & "=" & p.SQLQuote)

                    'If argStartupMode = "Crew" Then WHAvail = dt.Select(resourceValueMember & "=" & p.SQLQuote)

                    For Each d In WHAvail
                        curAvailRecordAbsolutePosition = curAvailRecordAbsolutePosition + 1

                        CurDate = d.Item("Date")

                        fgTimeline.Row = CurRow
                        fgTimeline.Col = curAvailRecordAbsolutePosition + 3

                        Dim monthPart = DatePart(DateInterval.Month, CurDate)
                        Dim dayPart = DatePart(DateInterval.Day, CurDate)

                        fgTimeline.set_TextMatrix(0, curAvailRecordAbsolutePosition + 3, IIf(curAvailRecordAbsolutePosition = 0 Or dayPart = 1, MonthName(monthPart, True) & " ", String.Empty) & dayPart)
                        avail = d.Item("Available")
                        Avail_Array(curWarehouseCount, curPartCount, curAvailRecordAbsolutePosition) = avail

                        If chkSummarize.Checked Then
                            Dim n As Long

                            avail = 0
                            For n = 1 To aryWarehouses.Count
                                avail = avail + Avail_Array(n, curPartCount, curAvailRecordAbsolutePosition)
                            Next

                            If Val(fgTimeline.Row) Mod 2 = 0 Then
                                fgTimeline.CellBackColor = Color.FromArgb(244, 244, 244)
                            Else
                                fgTimeline.CellBackColor = Color.White
                            End If

                        Else
                            If Val(fgTimeline.Row) Mod 2 = 0 Then
                                If curWarehouseCount Mod 2 = 1 Then fgTimeline.CellBackColor = Color.FromArgb(244, 244, 244) Else fgTimeline.CellBackColor = Color.FromArgb(234, 234, 234)
                            Else
                                If curWarehouseCount Mod 2 = 1 Then fgTimeline.CellBackColor = Color.White Else fgTimeline.CellBackColor = Color.FromArgb(224, 224, 224)
                            End If
                        End If

                        fgTimeline.Text = avail

                        With GetTimelineCellInfo(fgTimeline.Col, fgTimeline.Row)
                            .is_date = True
                            .the_date = d.Item("Date")
                            .is_part = True
                            .partno = p
                            .is_qty = True
                            .qty = avail
                            If chkSummarize.Checked Then
                                .is_warehouse = False
                            Else
                                .is_warehouse = True
                                .warehouse = d.Item(divisionValueMember)
                            End If
                        End With

                        If avail < 0 Then
                            fgTimeline.CellForeColor = Color.Red
                        ElseIf avail = 0 Then
                            fgTimeline.CellForeColor = fgTimeline.CellBackColor ' RGB(220, 120, 0)
                        Else
                            fgTimeline.CellForeColor = Color.FromArgb(64, 64, 64)
                        End If

                        If d.Item("Date") = Today Then fgTimeline.CellBackColor = Color.FromArgb(255, 255, 185)

                        If IsWeekend(d.Item("Date")) Then
                            fgTimeline.CellBackColor = Color.FromArgb(234, 234, 234)
                        End If

                        If d.Item(resourceValueMember) = txtPartNo.Text Then fgTimeline.CellFontBold = True

                        fgTimeline.CellAlignment = 1

                    Next d
                Next p
            Next w

            TimelineGridResize()

            fgTimeline.Redraw = True
            'fgTimeline.Visible = True

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub TimelineGridResize()
        Dim c As Long
        Dim r As Long

        For c = 0 To fgTimeline.Cols - 1
            Dim MaxWidth As Single, curWidth As Single
            MaxWidth = 0

            For r = 0 To fgTimeline.Rows - 1
                curWidth = TwipsFromPixelsX(CreateGraphics.MeasureString(fgTimeline.get_TextMatrix(r, c), fgTimeline.Font).Width)

                If curWidth > MaxWidth Then
                    MaxWidth = curWidth
                End If
            Next

            If MaxWidth > 5000 Then MaxWidth = 5500
            fgTimeline.set_ColWidth(c, MaxWidth + TwipsFromPixelsX(6))
            If c > 4 Then
            End If
        Next c
        Exit Sub
    End Sub

    Private Sub FillCalendarFlexGrid(ByVal ds As DataSet)
        If Not IsDataLoaded Then Return

        If dsSchedulingBoardData.Tables.Count = 0 Then
            'MsgBox("No parts have been selected.")
            Return
        End If

        Dim resourceValueMember = "partno"
        Dim resourceDisplayMember = "partdesc"
        Dim divisionValueMember = "bld"
        Dim divisionDisplayMember = "warehouse_description"

        If argStartupMode = "Crew" Then
            resourceValueMember = "jobtype"
            resourceDisplayMember = "jobdesc"
            divisionValueMember = "division"
            divisionDisplayMember = "divdesc"
        End If


        Dim sWarehouses = Replace(GetStrIncludedWarehouses(True, False), "'", "")
        Dim aryWarehouses = sWarehouses.Split(",")

        If argStartupMode = "Crew" Then aryWarehouses = (From r As DataRow In ds.Tables(0).Rows
                                                         Select CStr(r.Item(divisionValueMember))
                                                         Distinct).ToArray

        If String.IsNullOrEmpty(GetStrIncludedWarehouses(True, False)) Then
            fgCalendar.Rows = 0
            Return
        End If

        Try
            Dim dt = ds.Tables(0)

            fgCalendar.Visible = False
            fgCalendar.ScrollBars = MSFlexGridLib.ScrollBarsSettings.flexScrollBarVertical

            Dim calStartDate As Date = dt.Select().Min(Function(x) x.Item("Date"))
            Dim calEndDate As Date = dt.Select().Max(Function(x) x.Item("Date"))

            Dim CURWK As Long = DateDiff(DateInterval.WeekOfYear, calStartDate, calEndDate) + 1

            fgCalendar.Clear()
            fCalendarCellInfoList.Clear()

            If chkSummarize.Checked Then
                fgCalendar.Rows = 1 + CURWK
            Else
                fgCalendar.Rows = 1 + CURWK * aryWarehouses.Count
            End If

            fgCalendar.Cols = 8
            fgCalendar.FixedCols = 1
            fgCalendar.FixedRows = 1
            fgCalendar.set_ColWidth(0, 750)

            Dim day_array = {"", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"}

            fgCalendar.Row = 0
            Dim i As Long
            For i = 0 To 7
                fgCalendar.Col = i
                fgCalendar.Text = day_array(i)
                fgCalendar.CellFontBold = True
                fgCalendar.CellFontSize = 10
                fgCalendar.CellAlignment = 3
            Next

            Dim Avail_Array(aryWarehouses.Count - 1, 10000) As Long

            Dim curWarehouseCount As Integer = -1
            Dim curAvailRecordAbsolutePosition As Integer = -1

            For Each w In aryWarehouses
                curWarehouseCount += 1
                curAvailRecordAbsolutePosition = 0

                Dim WHAvail = dt.Select(divisionValueMember & "=" & w.SQLQuote)

                For Each d In WHAvail
                    curAvailRecordAbsolutePosition = curAvailRecordAbsolutePosition + 1

                    Dim CurDate As Date = d.Item("Date")

                    If chkSummarize.Checked Then
                        fgCalendar.Row = 1 + DateDiff("ww", dtpStartDate.Value, CurDate, vbSunday)
                    Else
                        fgCalendar.Row = 1 + curWarehouseCount + DateDiff("ww", dtpStartDate.Value, CurDate, vbSunday) * aryWarehouses.Count
                    End If

                    'Label the first column
                    fgCalendar.Col = 0
                    If chkSummarize.Checked Then fgCalendar.Text = "SUM" Else fgCalendar.Text = w

                    'Fill data into weekday columns
                    fgCalendar.Col = Weekday(CurDate, vbSunday)

                    Dim calstr As String
                    Dim avail As Long

                    avail = d.Item("Available")
                    Avail_Array(curWarehouseCount, curAvailRecordAbsolutePosition) = avail

                    If chkSummarize.Checked Then
                        Dim n As Long

                        avail = 0
                        For n = 0 To aryWarehouses.Count - 1
                            avail = avail + Avail_Array(n, curAvailRecordAbsolutePosition)
                        Next
                    End If

                    calstr = CurDate
                    calstr = calstr & Space(2 * (12 - Len(calstr))) & avail

                    With GetCalendarCellInfo(fgCalendar.Col, fgCalendar.Row)
                        .is_date = True
                        .the_date = d.Item("Date")
                        .is_part = True
                        .partno = d.Item(resourceValueMember)
                        .is_qty = True
                        .qty = avail
                        If chkSummarize.Checked Then
                            .is_warehouse = False
                        Else
                            .is_warehouse = True
                            .warehouse = d.Item(divisionValueMember)
                        End If
                    End With

                    If (DateDiff("ww", dtpStartDate.Value, CurDate, vbSunday) + 1) Mod 2 = 0 Then
                        fgCalendar.CellBackColor = Color.FromArgb(224, 224, 224)
                    Else
                        fgCalendar.CellBackColor = Color.White
                    End If

                    If avail < 0 Then
                        fgCalendar.CellForeColor = Color.Red
                    ElseIf avail = 0 Then
                        fgCalendar.CellForeColor = Color.FromArgb(185, 185, 185)
                    Else
                        fgCalendar.CellForeColor = Color.FromArgb(64, 64, 64)
                    End If

                    fgCalendar.CellAlignment = 1
                    fgCalendar.Text = calstr

                Next d
            Next w

            CalendarAutoSize()
            fgCalendar.Visible = True

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub CalendarAutoSize()
        If Not IsDataLoaded Then Return
        If fgCalendar.Rows = 0 Or fgCalendar.Cols = 0 Then Return

        Dim c As Integer

        Dim fgWidth = TwipsFromPixelsX(fgCalendar.Width)

        With fgCalendar
            For c = 1 To 7
                .set_ColWidth(c, 0)
                .set_ColWidth(c, (fgWidth - .get_ColWidth(0)) / 7 - 15)
            Next
        End With
    End Sub

    Private Sub FillSchedulingBoardFlexGrid(ByVal ds As DataSet)
        If dsSchedulingBoardData.Tables.Count = 0 Then
            MsgBox("No parts have been selected.")
            Exit Sub
        End If

        ds.Tables(0).TableName = "Parts"
        ds.Tables(1).TableName = "Part_SumAvail"
        ds.Tables(2).TableName = "Warehouses"
        ds.Tables(3).TableName = "WH_BottleneckPerPart"
        ds.Tables(4).TableName = "PartWH_Avail"
        ds.Tables(5).TableName = "Part_Schedule"

        Dim Days = (From r As DataRow In ds.Tables("Part_SumAvail").Rows
                    Select DirectCast(r.Item("date"), Date)).Distinct()

        Dim displayStartDate As Date = Days.Min
        Dim displayEndDate As Date = Days.Max

        With fgSchedulingBoard
            .Redraw = False
            .Clear()
            .MergeCells = MSFlexGridLib.MergeCellsSettings.flexMergeRestrictRows
            .Rows = 0
            .Cols = 0
            .Rows = 2
            .Cols += 3
            .Row = 0

            .set_TextMatrix(.Row, FLEXGRID_COLINDEX_ROWTYPE, "fgcRowType")
            .set_ColWidth(FLEXGRID_COLINDEX_ROWTYPE, 0)

            .set_TextMatrix(.Row, FLEXGRID_COLINDEX_ROWCODE, "fgRowCode")
            .set_ColWidth(FLEXGRID_COLINDEX_ROWCODE, 0)

            .set_TextMatrix(.Row, FLEXGRID_COLINDEX_ROWDESC, "fgRowDesc")
            .set_ColWidth(FLEXGRID_COLINDEX_ROWDESC, 0)

            fSchedulingBoardCellInfoList.Clear()

            'Add columns for each date in given date range
            .Cols += Days.Count
            .Col = 2
            For Each day In Days
                '.Cols += 1
                .Col += 1
                .Text = CDate(day)
                .CellAlignment = 2
                .set_ColWidth(.Col, user_colWidths)
                With GetSchedulingBoardCellInfo(.Col, .Row)
                    .is_date = True
                    .the_date = CDate(day)
                End With
            Next

            .Row = 1
            .Col = FLEXGRID_COLINDEX_ROWDESC

            'Add columns for each date in given date range
            For Each day In Days
                .Col += 1

                Dim monthPart = DatePart(DateInterval.Month, day)
                Dim dayPart = DatePart(DateInterval.Day, day)

                .Text = IIf(.Col = FLEXGRID_COLINDEX_ROWDESC + 1 Or dayPart = 1, MonthName(monthPart, True) & " ", String.Empty) & dayPart

                With GetSchedulingBoardCellInfo(.Col, .Row)
                    .is_date = True
                    .the_date = CDate(day)
                End With
            Next

            Dim rowHeight As Integer = 17 * 15

            'Add rows for each part
            Dim rIndex = .Rows - 1
            Dim currentPartNo As String = Nothing

            For Each p In lstMultiPartList.CheckedItems
                Debug.Print(Mid(p.ToString, 1, InStr(p.ToString, " ") - 1))

                Dim partNo As String = SQLQuote(Mid(p.ToString, 1, InStr(p.ToString, " ") - 1).ToUpper.Split(",")(0))
                Dim currentPartRows = ds.Tables("Parts").Select($"partno={partNo}")

                For Each partRow As DataRow In currentPartRows
                    currentPartNo = partRow.Item("partno")
                    rIndex += 1
                    .AddItem(FLEXGRID_ROWTYPE_PART, rIndex)
                    .set_RowHeight(rIndex, rowHeight * 1.25)
                    .Row = rIndex
                    .Col = FLEXGRID_COLINDEX_ROWCODE
                    .Text = currentPartNo
                    For c = FLEXGRID_COLINDEX_ROWDESC To .Cols - 1
                        .Col = c
                        .Text = partRow.Item("partdesc") & " (" & partRow.Item("partno") & ")     Bottleneck: " & partRow.Item("bottleneck")
                        .CellBackColor = Color.DarkBlue
                        .CellForeColor = Color.White
                        .CellFontSize = 10
                        .CellFontBold = True
                        With GetSchedulingBoardCellInfo(.Col, .Row)
                            .is_part = True
                            .partno = currentPartNo
                            .the_date = GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, 0).the_date
                        End With
                    Next
                    .set_MergeRow(rIndex, True)

                    'Add rows for quantity timeline for each part
                    rIndex += 1
                    .AddItem("qty", rIndex)
                    .set_RowHeight(rIndex, rowHeight)

                    .Col = FLEXGRID_COLINDEX_ROWDESC
                    .Row = rIndex
                    For Each c As DataRow In ds.Tables("Part_SumAvail").Rows
                        If .Col + 1 >= .Cols Then Exit For
                        .Col += 1
                        Dim currentQty = ds.Tables("Part_SumAvail").Select("partno='" & currentPartNo & "' and date='" & .get_TextMatrix(0, .Col) & "'")(0)("available")
                        .Text = currentQty
                        If .Text < 0 Then .CellForeColor = Color.Red
                        .CellAlignment = 1
                        .CellBackColor = SystemColors.Control
                        With GetSchedulingBoardCellInfo(.Col, .Row)
                            .is_qty = True
                            .qty = currentQty
                            .partno = currentPartNo
                            .the_date = GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, 0).the_date
                            If .the_date < Today() Then fgSchedulingBoard.CellForeColor = fgSchedulingBoard.CellBackColor
                        End With
                    Next

                    rIndex += 1
                    flexgrid_AddSpacer(rIndex)

                    'Add rows for each warehouse
                    Dim currentWH As String = Nothing
                    For Each wh In GetStrIncludedWarehouses(Codes:=True, Descriptions:=False).Replace("'", "").Split(",") ' As DataRow In ds.Tables("Warehouses").Rows
                        currentWH = wh '.Item(FLEXGRID_ROWTYPE_WAREHOUSE)
                        rIndex += 1
                        .AddItem(FLEXGRID_ROWTYPE_WAREHOUSE, rIndex)
                        .Row = rIndex
                        .Col = FLEXGRID_COLINDEX_ROWCODE
                        .Text = currentWH
                        For c = FLEXGRID_COLINDEX_ROWDESC To .Cols - 1
                            .Col = c
                            Dim bottleneckPerPart As String = Nothing
                            If Not ds.Tables("WH_BottleneckPerPart").Select("partno='" & currentPartNo & "' and bld='" & currentWH & "'").Count = 0 Then
                                bottleneckPerPart = ds.Tables("WH_BottleneckPerPart").Select("partno='" & currentPartNo & "' and bld='" & currentWH & "'")(0)("bottleneck")
                            Else
                                bottleneckPerPart = 0
                            End If
                            .Text = ds.Tables("Warehouses").Select("bld=" & SQLQuote(wh))(0)("warehouse_description") & "     Bottleneck: " & bottleneckPerPart
                            .CellBackColor = Color.AliceBlue
                            With GetSchedulingBoardCellInfo(.Col, .Row)
                                .is_warehouse = True
                                .warehouse = currentWH
                                .partno = currentPartNo
                                .the_date = GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, 0).the_date
                            End With
                        Next
                        .set_MergeRow(rIndex, True)

                        .set_RowHeight(rIndex, rowHeight)

                        'Add rows for quantity timeline for each warehouse per part
                        rIndex += 1
                        .AddItem("qty", rIndex)
                        .set_RowHeight(rIndex, rowHeight)

                        .Col = FLEXGRID_COLINDEX_ROWDESC
                        .Row = rIndex
                        For Each c As DataRow In ds.Tables("PartWH_Avail").Rows
                            If .Col + 1 >= .Cols Then Exit For
                            .Col += 1
                            Dim currentQty As Integer = 0
                            If Not ds.Tables("PartWH_Avail").Select("partno='" & currentPartNo & "' and bld='" & currentWH & "' and date='" & .get_TextMatrix(0, .Col) & "'").Count = 0 Then
                                currentQty = ds.Tables("PartWH_Avail").Select("partno='" & currentPartNo & "' and bld='" & currentWH & "' and date='" & .get_TextMatrix(0, .Col) & "'")(0)("available")
                            End If
                            .Text = currentQty
                            If .Text < 0 Then .CellForeColor = Color.Red
                            .CellAlignment = 1
                            .CellBackColor = Color.WhiteSmoke
                            With GetSchedulingBoardCellInfo(.Col, .Row)
                                .is_qty = True
                                .qty = currentQty
                                .warehouse = currentWH
                                .partno = currentPartNo
                                .the_date = GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, 0).the_date
                                If .the_date < Today() Then fgSchedulingBoard.CellForeColor = fgSchedulingBoard.CellBackColor
                            End With
                        Next

                        'Add part scheduling details for each warehouse per part
                        Dim ProjectsPerWH = From r As DataRow In ds.Tables("Part_Schedule").Rows
                                            Where r.Item("partno") = currentPartNo And r.Item(FLEXGRID_ROWTYPE_WAREHOUSE) = currentWH
                                            Select r

                        For Each proj In ProjectsPerWH
                            If (chkShowDetail.Checked And proj.Item("startdate") < Today()) _
                                Or (chkShowFuture.Checked And proj.Item("startdate") >= Today()) _
                                Or proj.Item("late") = "Y" Or proj.Item("batchno") = "REPAIR" Then

                                If proj.Item("late") = "Y" Then
                                    Dim foo = proj.Item("enddate")
                                End If

                                If IIf(proj.Item("late") = "Y", displayEndDate, proj.Item("enddate")) >= displayStartDate Then

                                    'Go to the Warehouse Row and see what is the next available set of blank cells in which to place the next part scheduling detail
                                    For f = -rIndex To 0
                                        If fgSchedulingBoard.get_TextMatrix(-f, FLEXGRID_COLINDEX_ROWTYPE) = FLEXGRID_ROWTYPE_WAREHOUSE Then
                                            rIndex = -f + 1 ' The +1 accounts for the availability row right below the Warehouse row.
                                            Exit For
                                        End If
                                    Next

                                    rIndex += 1 'Go to the first row below the Warehouse Availability row

                                    For c = FLEXGRID_COLINDEX_ROWDESC + 1 To fgSchedulingBoard.Cols - 1
                                        If New Date() {proj.Item("startdate"), displayStartDate}.Max <= CDate(.get_TextMatrix(0, c)) _
                                            AndAlso New Date() {displayEndDate, IIf(proj.Item("late") = "Y", displayEndDate, proj.Item("enddate"))}.Min >= CDate(.get_TextMatrix(0, c)) Then
                                            .Col = c
TryNextRow:
                                            'If the next row doesn't exist, add a new row and move on.
                                            If .Rows - 1 < rIndex Then
                                                .AddItem(FLEXGRID_ROWTYPE_PROJECT, rIndex)
                                                Exit For
                                            End If

                                            'If the next row is not already a project row, insert a new row and move on.
                                            If fgSchedulingBoard.get_TextMatrix(rIndex, FLEXGRID_COLINDEX_ROWTYPE) <> FLEXGRID_ROWTYPE_PROJECT Then
                                                If Not fgSchedulingBoard.get_TextMatrix(rIndex, FLEXGRID_COLINDEX_ROWTYPE) = FLEXGRID_ROWTYPE_SPACER Then rIndex += 1

                                                .AddItem(FLEXGRID_ROWTYPE_PROJECT, rIndex)
                                                InsertSchedulingBoardRow(rIndex)
                                                Exit For
                                            End If

                                            'If the next row's cells of the next assignment are already filled with an existing assignmemt, then try the next row.
                                            If GetSchedulingBoardCellInfo(.Col, rIndex).is_project Then
                                                rIndex += 1
                                                GoTo TryNextRow
                                                Exit For
                                            End If

                                            'Otherwise, we are set to add an assignement to the current assignment row because the added assignment dates are blanks on this row.
                                        End If
                                    Next

                                    .Row = rIndex 'error when multipart group cable, mic, aes/ebu selected and cable, neutrik opticalcon fiber and motorola
                                    .Col = FLEXGRID_COLINDEX_ROWCODE
                                    .Text = proj.Item("batchno")
                                    .Col = FLEXGRID_COLINDEX_ROWDESC
                                    .Text = proj.Item("entitydesc") & " (" & proj.Item("startdate") & "-" & proj.Item("enddate") & ")"

                                    Dim qtyRow As Integer = Nothing
                                    For c = FLEXGRID_COLINDEX_ROWDESC + 1 To fgSchedulingBoard.Cols - 1
                                        If (IIf(proj.Item("startdate") < displayStartDate, displayStartDate, proj.Item("startdate")) <= .get_TextMatrix(0, c) _
                                            And IIf(proj.Item("enddate") > displayEndDate, displayEndDate, IIf(proj.Item("late") = "Y", displayEndDate, proj.Item("enddate"))) >= .get_TextMatrix(0, c)) Then
                                            'Look up whether or not there is a shortage over the date range of this project.
                                            For r = -rIndex To 0
                                                If .get_TextMatrix(-r, 0) = "qty" Then
                                                    qtyRow = -r
                                                    Exit For
                                                End If
                                            Next
                                            If qtyRow <> 0 Then
                                                If .get_TextMatrix(qtyRow, c) < 0 And proj.Item("startdate") > Today() And proj.Item("displayorder") <> 3 And proj.Item("displayorder") <> 4 Then
                                                    .CellBackColor = Color.Tomato
                                                End If
                                            End If
                                            .Col = c

                                            .Text = $"{proj.Item(FLEXGRID_ROWTYPE_WAREHOUSE)}"
                                            If proj.Item("batchno") <> "REPAIR" OrElse Not proj.Table.Columns.Contains("childQty") Then
                                                .Text &= $"({proj.Item("qty")})"
                                            Else
                                                If proj.Item("childQty") > 0 Then
                                                    .Text &= $"({proj.Item("qty")}+{proj.Item("childQty")})"
                                                Else
                                                    .Text &= $"({proj.Item("qty")})"
                                                End If
                                            End If

                                            .Text &= $" {proj.Item("entitydesc")} ({proj.Item("startdate")}-{proj.Item("enddate")})"

                                            If proj.Item("late") = "Y" Then .Text = "(LATE) " & .Text
                                            If proj.Item("PhaseIsHeldInWarehouse") = "Y" Or proj.Item("proptype") = "STORG" Then .Text = "(HOLD) " & .Text
                                            .CellAlignment = 1
                                            'Color cell background for project status
                                            Select Case proj.Item("status")
                                                Case "A"
                                                    If proj.Item("startdate") >= Today() Then
                                                        .CellBackColor = Color.LightGreen
                                                    Else
                                                        .CellBackColor = NonEditableCellColor()
                                                    End If
                                                Case "P"
                                                    .CellBackColor = Color.LightGoldenrodYellow
                                                Case "I"
                                                    If proj.Item("enddate") >= Today() Then .CellBackColor = Color.Red Else .CellBackColor = NonEditableCellColor()
                                            End Select
                                            If proj.Item("late") = "Y" Then .CellBackColor = Color.Pink
                                            If (proj.Item("PhaseIsHeldInWarehouse") = "Y" And proj.Item("startdate") <> Today) Or proj.Item("proptype") = "STORG" Then .CellBackColor = Color.Goldenrod
                                            If proj.Item("batchno") = "REPAIR" Then .CellBackColor = Color.Thistle
                                            If proj.Item("displayorder") = 4 Then .CellBackColor = Color.LightGray
                                            If proj.Item("batchno").ToString.Contains("$U3") Then .CellBackColor = Color.LightSkyBlue
                                            If proj.Item("enddate") < Today() And proj.Item("late") <> "Y" Then
                                                .CellForeColor = Color.DarkSlateGray
                                                .CellBackColor = Color.GhostWhite
                                            End If

                                            With GetSchedulingBoardCellInfo(.Col, .Row)
                                                .is_project = True
                                                .entityno = proj.Item("batchno")
                                                .is_late = (proj.Item("late") = "Y")
                                                .entitydesc = proj.Item("entitydesc")
                                                .ProjStartDate = proj.Item("startdate")
                                                .ProjEndDate = proj.Item("enddate")
                                                If proj.Table.Columns.Contains("childQty") Then
                                                    .qty = proj.Item("qty") + proj.Item("childQty")
                                                Else
                                                    .qty = proj.Item("qty")
                                                End If
                                                .warehouse = currentWH
                                                .partno = currentPartNo
                                                .the_date = GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, 0).the_date
                                            End With
                                        End If
                                    Next
                                    'Try
                                    .set_MergeRow(rIndex, True)
                                    .set_RowHeight(rIndex, rowHeight)
                                    'Catch ex As Exception

                                    'End Try
                                End If
                            End If
                        Next 'Project

                        rIndex = .Rows - 1

                        rIndex += 1
                        flexgrid_AddSpacer(rIndex)

                    Next 'Warehouse

                    rIndex += 1
                    flexgrid_AddSpacer(rIndex)
                    Exit For
                Next
            Next p

            .FixedRows = 2

            'Highlight weekends
            For c = FLEXGRID_COLINDEX_ROWDESC + 1 To .Cols - 1
                If IsWeekend(CDate(.get_TextMatrix(0, c))) Then
                    For r = 1 To .Rows - 1
                        .Row = r
                        .Col = c
                        'Debug.Print(.CellBackColor.ToString)
                        If .CellBackColor = Color.Black Then
                            .CellBackColor = Color.WhiteSmoke
                        End If
                    Next
                End If
            Next

            .set_RowHeight(0, 0)

            Try
                'Highlight mouse-click-n-drag selected columns
                If HighlightColumns_Start <> HighlightColumns_End Then
                    For r = 1 To fgSchedulingBoard.Rows - 1
                        For c = HighlightColumns_Start To HighlightColumns_End
                            If c > 0 Then
                                fgSchedulingBoard.Col = c
                                fgSchedulingBoard.Row = r
                                If fgSchedulingBoard.Text = String.Empty Or IsNumeric(fgSchedulingBoard.Text) Then fgSchedulingBoard.CellBackColor = Color.LightYellow
                            End If
                        Next
                    Next
                    PreviousHighlightColumns_Start = HighlightColumns_Start
                    PreviousHighlightColumns_End = HighlightColumns_End
                End If

                .Redraw = True
            Catch ex As Exception

            End Try

            Try
                ResetScrollbarsToFirstVisiblePartnoAndFirstVisibleColumn()
            Catch ex As Exception
                Dim newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()
                Dim empname = newConn.ExecuteScalar("Select p.empname from dbo.pjtfrusr p where p.user_name = suser_sname()")
                newConn.Close()
                newConn.Dispose()
                MsgBox(Mid(empname, 1, InStr(empname, " ") - 1) & ", WTF?")
            End Try
        End With
    End Sub

    Private Sub ResetScrollbarsToFirstVisiblePartnoAndFirstVisibleColumn()
        'Reset scrollbars to first visible partno and first visible column
        For r = 0 To fgSchedulingBoard.Rows - 1
            If strFirstVisiblePartNo = Nothing Then Exit For
            If fgSchedulingBoard.get_TextMatrix(r, FLEXGRID_COLINDEX_ROWCODE) = strFirstVisiblePartNo Then
                fgSchedulingBoard.TopRow = r
                Exit For
            End If
        Next
        If fgSchedulingBoard.Cols > 0 Then
            fgSchedulingBoard.LeftCol = If(intFirstVisibleCol < fgSchedulingBoard.Cols, intFirstVisibleCol, FLEXGRID_COLINDEX_ROWDESC + 1)
        End If
    End Sub

    Private Sub flexgrid_AddSpacer(ByVal rIndex As Integer)
        fgSchedulingBoard.AddItem(FLEXGRID_ROWTYPE_SPACER, rIndex)
        fgSchedulingBoard.set_RowHeight(rIndex, 8 * 15)
    End Sub

    Private Sub flexgrid_ClickEvent(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgSchedulingBoard.ClickEvent
        If fgSchedulingBoard.Rows = 0 Then Exit Sub
        If fgSchedulingBoard.RowSel = fgSchedulingBoard.Rows - 1 Then Exit Sub

        Dim partRow As Integer = Nothing
        If fgSchedulingBoard.get_TextMatrix(fgSchedulingBoard.Row, FLEXGRID_COLINDEX_ROWTYPE) = FLEXGRID_ROWTYPE_PART Then
            partRow = fgSchedulingBoard.Row
        Else
            Dim rIndex = fgSchedulingBoard.Row
            For r = -rIndex To 0
                If fgSchedulingBoard.get_TextMatrix(-r, 0) = FLEXGRID_ROWTYPE_PART Then
                    partRow = -r
                    Exit For
                End If
            Next
        End If

        Dim warehouseRow As Integer = Nothing
        If fgSchedulingBoard.get_TextMatrix(fgSchedulingBoard.Row, FLEXGRID_COLINDEX_ROWTYPE) = FLEXGRID_ROWTYPE_WAREHOUSE Then
            warehouseRow = fgSchedulingBoard.Row
        Else
            Dim rIndex = fgSchedulingBoard.Row
            For r = -rIndex To 0
                If fgSchedulingBoard.get_TextMatrix(-r, 0) = FLEXGRID_ROWTYPE_WAREHOUSE Then
                    warehouseRow = -r
                    Exit For
                End If
            Next
        End If

        Dim cellTxt = fgSchedulingBoard.Clip()
        If cellTxt = "" OrElse IsNumeric(cellTxt.ToCharArray().First()) Then
            LblProjectName.Text = ""
        Else
            LblProjectName.Text = fgSchedulingBoard.Clip()
        End If
    End Sub

    Private Sub fgCalendar_MouseDownEvent(ByVal sender As Object, ByVal e As AxMSFlexGridLib.DMSFlexGridEvents_MouseDownEvent) Handles fgCalendar.MouseDownEvent
        If Not IsDataLoaded Then Exit Sub
        If fgCalendar.Rows = 0 Or fgCalendar.Cols = 0 Then Exit Sub

        SubstitutePartToolStripMenuItem.Visible = False
        ChangeOrderedQtyToolStripMenuItem.Visible = False
        mnuSubstitutePartSeparator.Visible = False
        ClearHighlightingToolStripMenuItem.Visible = False
        ToolStripSeparator2.Visible = False
        ProjectMaintenanceToolStripMenuItem.Visible = False
        TransferToolsToolStripMenuItem.Visible = False
        AvailabilityToolStripMenuItem.Visible = False
        PartMaintenanceToolStripMenuItem.Visible = False
        SubstitutePartToolStripMenuItem.Visible = False
        ChangeOrderedQtyToolStripMenuItem.Visible = False
        ToolStripSeparator3.Visible = False
        CopyPartBarcodeToolStripMenuItem.Visible = False
        CopyPartNumberToolStripMenuItem.Visible = True
        CopyPartDescriptionToolStripMenuItem.Visible = True
        CopyPartNumberAndDescriptionToolStripMenuItem.Visible = True
        CopyPhaseNumberToolStripMenuItem.Visible = False
        CopyPhaseDescriptionToolStripMenuItem.Visible = False
        CopyPhaseNumberAndDescriptionToolStripMenuItem.Visible = False

        With GetCalendarCellInfo(fgCalendar.Col, fgCalendar.Row)
            If Not String.IsNullOrEmpty(.the_date) Then
                Application.DoEvents()
                RefreshCurrentPartAttributes(.partno)

                Application.DoEvents()
                RefreshPartWarehouseQtys()

                timer_DoubleClickInterval.Start()
            End If
        End With

        _fieldsAvailableForCopying.Clear()
        _fieldsAvailableForCopying.Add("partno", GetCalendarCellInfo(fgCalendar.Col, fgCalendar.Row).partno)

        Dim addMultipartGroupPanelWidth As Double = 0
        If pnlMultiPartGroups.Location.X = 0 Then
            addMultipartGroupPanelWidth = pnlMultiPartGroups.Width
        End If

        If e.button = 2 Then
            Dim p = Me.PointToScreen(scParts.Location + tcMain.Location + fgCalendar.Location)
            mnuFlexgrid.Show(addMultipartGroupPanelWidth + tsMainVert.Width + p.X + e.x, pnlControls.Height + pnlWarehouses.Height + p.Y + e.y)
        End If
    End Sub

    Private Sub fgSchedulingBoard_MouseDownEvent(ByVal sender As Object, ByVal e As AxMSFlexGridLib.DMSFlexGridEvents_MouseDownEvent) Handles fgSchedulingBoard.MouseDownEvent
        If Not IsDataLoaded Then Exit Sub
        If fgSchedulingBoard.Rows = 0 Or fgSchedulingBoard.Cols = 0 Then Exit Sub

        SubstitutePartToolStripMenuItem.Visible = True
        ChangeOrderedQtyToolStripMenuItem.Visible = True
        mnuSubstitutePartSeparator.Visible = True
        ClearHighlightingToolStripMenuItem.Visible = True
        ToolStripSeparator2.Visible = True
        ProjectMaintenanceToolStripMenuItem.Visible = True
        TransferToolsToolStripMenuItem.Visible = True
        AvailabilityToolStripMenuItem.Visible = True
        PartMaintenanceToolStripMenuItem.Visible = True
        SubstitutePartToolStripMenuItem.Visible = True
        ChangeOrderedQtyToolStripMenuItem.Visible = True
        ToolStripSeparator3.Visible = True
        CopyPartBarcodeToolStripMenuItem.Visible = False
        CopyPartNumberToolStripMenuItem.Visible = True
        CopyPartDescriptionToolStripMenuItem.Visible = True
        CopyPartNumberAndDescriptionToolStripMenuItem.Visible = True
        CopyPhaseNumberToolStripMenuItem.Visible = True
        CopyPhaseDescriptionToolStripMenuItem.Visible = True
        CopyPhaseNumberAndDescriptionToolStripMenuItem.Visible = True

        HighlightColumns_Start = fgSchedulingBoard.MouseCol
        Dim selectedCellInfo As SchedulingBoardCellInfo = GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)

        With selectedCellInfo
            SubstitutePartToolStripMenuItem.Visible = Not String.IsNullOrEmpty(.entityno) And .is_late = False AndAlso .entityno <> "REPAIR"
            ChangeOrderedQtyToolStripMenuItem.Visible = Not String.IsNullOrEmpty(.entityno) And .is_late = False AndAlso .entityno <> "REPAIR"
            mnuSubstitutePartSeparator.Visible = Not String.IsNullOrEmpty(.entityno) And .is_late = False AndAlso .entityno <> "REPAIR"
            CopyPhaseNumberToolStripMenuItem.Visible = Not String.IsNullOrEmpty(.entityno) And .is_late = False AndAlso .entityno <> "REPAIR"
            CopyPhaseDescriptionToolStripMenuItem.Visible = Not String.IsNullOrEmpty(.entityno) And .is_late = False AndAlso .entityno <> "REPAIR"
            CopyPhaseNumberAndDescriptionToolStripMenuItem.Visible = Not String.IsNullOrEmpty(.entityno) And .is_late = False AndAlso .entityno <> "REPAIR"

            If Not String.IsNullOrEmpty(.partno) Then
                lstMultiPartList.SelectedItems.Clear()

                If e.button = 1 Then 'Left click only
                    Application.DoEvents()
                    RefreshPartData(.partno)
                End If
            End If
        End With

        If fgSchedulingBoard.CellBackColor = NonEditableCellColor() Then
            SubstitutePartToolStripMenuItem.Visible = False
            ChangeOrderedQtyToolStripMenuItem.Visible = False
            CopyPhaseNumberToolStripMenuItem.Visible = False
            CopyPhaseDescriptionToolStripMenuItem.Visible = False
            CopyPhaseNumberAndDescriptionToolStripMenuItem.Visible = False
        End If

        If Not _can_edit_projects Then SubstitutePartToolStripMenuItem.Visible = False
        If Not _can_edit_projects Then ChangeOrderedQtyToolStripMenuItem.Visible = False
        If Not _can_edit_projects Then CopyPhaseNumberToolStripMenuItem.Visible = False
        If Not _can_edit_projects Then CopyPhaseDescriptionToolStripMenuItem.Visible = False
        If Not _can_edit_projects Then CopyPhaseNumberAndDescriptionToolStripMenuItem.Visible = False

        _fieldsAvailableForCopying.Clear()
        _fieldsAvailableForCopying.Add("partno", selectedCellInfo.partno)
        _fieldsAvailableForCopying.Add("entityno", selectedCellInfo.entityno)
        _fieldsAvailableForCopying.Add("entitydesc", selectedCellInfo.entitydesc)



        Dim addMultipartGroupPanelWidth As Double = 0
        If pnlMultiPartGroups.Location.X = 0 Then
            addMultipartGroupPanelWidth = pnlMultiPartGroups.Width
        End If

        If e.button = 2 Then
            Dim p = Me.PointToScreen(scParts.Location + tcMain.Location + fgSchedulingBoard.Location)
            mnuFlexgrid.Show(addMultipartGroupPanelWidth + tsMainVert.Width + p.X + e.x, pnlControls.Height + pnlWarehouses.Height + p.Y + e.y)
        End If
    End Sub

    Declare Function LockWindowUpdate Lib "user32" Alias "LockWindowUpdate" (ByVal hwndLock As Long) As Long

    Private Sub fgTimeline_MouseDownEvent(sender As Object, e As DMSFlexGridEvents_MouseDownEvent) Handles fgTimeline.MouseDownEvent
        If Not IsDataLoaded Then Exit Sub
        If fgTimeline.Rows = 0 Or fgTimeline.Cols = 0 Then Exit Sub

        SubstitutePartToolStripMenuItem.Visible = False
        ChangeOrderedQtyToolStripMenuItem.Visible = False
        mnuSubstitutePartSeparator.Visible = False
        ClearHighlightingToolStripMenuItem.Visible = False
        ToolStripSeparator2.Visible = False
        ProjectMaintenanceToolStripMenuItem.Visible = False
        TransferToolsToolStripMenuItem.Visible = False
        AvailabilityToolStripMenuItem.Visible = False
        PartMaintenanceToolStripMenuItem.Visible = False
        SubstitutePartToolStripMenuItem.Visible = False
        ChangeOrderedQtyToolStripMenuItem.Visible = False
        ToolStripSeparator3.Visible = False
        CopyPartBarcodeToolStripMenuItem.Visible = False
        CopyPartNumberToolStripMenuItem.Visible = True
        CopyPartDescriptionToolStripMenuItem.Visible = True
        CopyPartNumberAndDescriptionToolStripMenuItem.Visible = True
        CopyPhaseNumberToolStripMenuItem.Visible = False
        CopyPhaseDescriptionToolStripMenuItem.Visible = False
        CopyPhaseNumberAndDescriptionToolStripMenuItem.Visible = False

        With GetTimelineCellInfo(fgTimeline.Col, fgTimeline.Row)
            If Not String.IsNullOrEmpty(.the_date) Then
                Application.DoEvents()
                RefreshCurrentPartAttributes(.partno)

                Application.DoEvents()
                RefreshPartWarehouseQtys()

                MultiPartGrid_BoldSelectedPart()

                timer_DoubleClickInterval.Start()
            End If
        End With

        _fieldsAvailableForCopying.Clear()
        _fieldsAvailableForCopying.Add("partno", GetTimelineCellInfo((fgTimeline.Col), (fgTimeline.Row)).partno)

        Dim addMultipartGroupPanelWidth As Double = 0
        If pnlMultiPartGroups.Location.X = 0 Then
            addMultipartGroupPanelWidth = pnlMultiPartGroups.Width
        End If

        If e.button = 2 Then
            Dim p = Me.PointToScreen(scParts.Location + tcMain.Location + fgTimeline.Location)
            mnuFlexgrid.Show(addMultipartGroupPanelWidth + tsMainVert.Width + p.X + e.x, pnlControls.Height + pnlWarehouses.Height + p.Y + e.y)
        End If
    End Sub

    Private Sub MultiPartGrid_BoldSelectedPart()
        Dim r As Integer
        Dim c As Integer

        Dim selectedRow As Integer
        Dim selectedCol As Integer

        With fgTimeline
            selectedRow = .Row
            selectedCol = .Col

            .Redraw = False
            '.Visible = False
            For r = 1 To .Rows - 1
                .Row = r
                For c = 3 To .Cols - 1
                    .Col = c
                    .CellFontBold = False
                    If .get_TextMatrix(r, 1) = dtCurrentPart.Rows(0).Item("partno") Then
                        .CellFontBold = True
                    End If
                    If .Col = selectedCol Then
                        .CellFontBold = True
                    End If
                Next c
            Next r
            .Redraw = True
            '.Visible = True

            .Row = selectedRow
            .Col = selectedCol
        End With

    End Sub

    'Private Sub fgCalendar_MouseDoubleClick(sender As Object, e As EventArgs) Handles fgCalendar.MouseDoubleClick
    'subs never fire
    'End Sub

    'Private Sub fgCalendar_DoubleClick(sender As Object, e As EventArgs) Handles fgCalendar.DoubleClick
    'subs never fire
    'End Sub

    Private Sub fgCalendar_DblClick(sender As Object, e As EventArgs) Handles fgCalendar.DblClick, fgTimeline.DblClick
        If Not IsDataLoaded Then Exit Sub
        If CurrentFlexGrid.Rows = 0 Or CurrentFlexGrid.Cols = 0 Then Exit Sub

        With GetCalendarCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
            If Not String.IsNullOrEmpty(.the_date) Then
                If dgvDetail.Parent Is scPartsAndDetail.Panel2 Then scPartsAndDetail.Panel2Collapsed = False

                timer_DoubleClickInterval.Stop()

                Application.DoEvents()
                RefreshPartScheduleDetails(CurrentFlexGrid, Nothing)
            End If
        End With
    End Sub
    Private Sub flexgrid_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgSchedulingBoard.Resize
        If Not IsDataLoaded Then Exit Sub
        If fgSchedulingBoard.Cols = 0 Or fgSchedulingBoard.Rows = 0 Then Exit Sub
        'Resize all columns
        If fgSchedulingBoard.get_ColWidth(fgSchedulingBoard.MouseCol) = 0 Then Exit Sub
        user_colWidths = fgSchedulingBoard.get_ColWidth(fgSchedulingBoard.MouseCol)
        For c = FLEXGRID_COLINDEX_ROWDESC + 1 To fgSchedulingBoard.Cols - 1
            fgSchedulingBoard.set_ColWidth(c, user_colWidths)
        Next
    End Sub

    Private Sub flexgrid_Scroll(ByVal sender As Object, ByVal e As System.EventArgs) Handles fgSchedulingBoard.Scroll
        For r = 0 To fgSchedulingBoard.Rows - 1
            If fgSchedulingBoard.get_RowIsVisible(r) = True Then
                If fgSchedulingBoard.get_TextMatrix(r, FLEXGRID_COLINDEX_ROWTYPE) = FLEXGRID_ROWTYPE_PART Then
                    strFirstVisiblePartNo = fgSchedulingBoard.get_TextMatrix(r, FLEXGRID_COLINDEX_ROWCODE)
                    Debug.Print("First visible Part No: " & strFirstVisiblePartNo)
                    Exit For
                End If
            End If
        Next
        intFirstVisibleCol = fgSchedulingBoard.LeftCol
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        If Not IsDataLoaded Then Exit Sub
        If txtPartDesc.Text = "" Or txtPartNo.Text = "" Then Exit Sub

        RefreshAvailabilityData()
        RefreshPartData(GetCurrentPartNo)
    End Sub

    Private Function CurrentFlexGrid() As AxMSFlexGridLib.AxMSFlexGrid
        Dim fg = fgSchedulingBoard

        Select Case tcMain.SelectedTab.Name
            Case tpSchedulingBoard.Name
                fg = fgSchedulingBoard
            Case tpTimeline.Name
                fg = fgTimeline
            Case tpCalendar.Name
                fg = fgCalendar
        End Select

        Return fg
    End Function

    Private Sub frmSchedulingBoard_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
        If Not IsDataLoaded Then Exit Sub
        If CurrentFlexGrid.Cols = 0 Or CurrentFlexGrid.Rows = 0 Then Exit Sub

        Dim scrollAttemptRowChangeDelta As Integer = -e.Delta / 28

        Select Case CurrentFlexGrid().Name
            Case fgSchedulingBoard.Name
                Try
                    Select Case Control.ModifierKeys
                        Case Keys.Shift
                            If (fgSchedulingBoard.LeftCol + scrollAttemptRowChangeDelta) > FLEXGRID_COLINDEX_ROWDESC + 1 Then
                                fgSchedulingBoard.LeftCol += scrollAttemptRowChangeDelta
                            Else
                                fgSchedulingBoard.LeftCol = FLEXGRID_COLINDEX_ROWDESC + 1
                            End If
                        Case Keys.Control
                            If (user_colWidths + scrollAttemptRowChangeDelta * 15) > 15 Then
                                user_colWidths += scrollAttemptRowChangeDelta * 15
                                IsDataLoaded = False
                                For c = FLEXGRID_COLINDEX_ROWDESC + 1 To fgSchedulingBoard.Cols - 1
                                    fgSchedulingBoard.set_ColWidth(c, user_colWidths)
                                Next
                                IsDataLoaded = True
                            Else
                                fgSchedulingBoard.LeftCol = 15
                            End If

                        Case Else
                            Try
                                If (fgSchedulingBoard.TopRow + scrollAttemptRowChangeDelta) > fgSchedulingBoard.FixedRows - 1 Then
                                    fgSchedulingBoard.TopRow += scrollAttemptRowChangeDelta
                                Else
                                    fgSchedulingBoard.TopRow = fgSchedulingBoard.FixedRows
                                End If
                            Catch ex As Exception
                                ' if you scroll during a refresh, it can kill the grid, maybe?
                                ' so we'll ignore exceptions here.
                            End Try
                    End Select
                Catch ex As Exception

                End Try

            Case fgCalendar.Name, fgTimeline.Name
                Try
                    If (CurrentFlexGrid.TopRow + scrollAttemptRowChangeDelta) > CurrentFlexGrid.FixedRows - 1 Then
                        CurrentFlexGrid.TopRow += scrollAttemptRowChangeDelta
                    Else
                        CurrentFlexGrid.TopRow = CurrentFlexGrid.FixedRows
                    End If
                Catch ex As Exception

                End Try
        End Select
    End Sub

    Private Sub flexgrid_MouseUpEvent(ByVal sender As Object, ByVal e As AxMSFlexGridLib.DMSFlexGridEvents_MouseUpEvent) Handles fgSchedulingBoard.MouseUpEvent
        'If e.shift <> 1 Then Return

        HighlightColumns_End = fgSchedulingBoard.MouseCol

        If HighlightColumns_Start > HighlightColumns_End Then
            Dim temp = HighlightColumns_End
            HighlightColumns_End = HighlightColumns_Start
            HighlightColumns_Start = temp
        End If

        Dim foomc = fgSchedulingBoard.MouseCol
        Dim foo = fgSchedulingBoard.Cols

        If HighlightColumns_Start <> HighlightColumns_End Then
            FillSchedulingBoardFlexGrid(dsSchedulingBoardData)
        End If
    End Sub

#End Region

#Region "Part Substitutions/Qty Changes"

    Private Sub GetChangeOrderedInfoDetails(ByRef projStartDate As Date, ByRef partdesc As Object, ByRef entitydesc As Object, ByRef qty_current As Object, ByRef shouldReturn As Boolean)
        shouldReturn = False

        With GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)
            Dim seqno = dsSchedulingBoardData.Tables("Part_Schedule").Select("partno = " & SQLQuote(.partno) & " And batchno = " & SQLQuote(.entityno) & " And startdate = " & SQLQuote(CDate(.ProjStartDate)) & " And bld=" & SQLQuote(.warehouse))

            If seqno.Count > 1 Then
                Dim dialogresult As DialogResult = Nothing
                dialogresult = MsgBox("There are multiple instance of this part on the selected order.  Would you Like to go to Project Maintenance to change this order?", vbYesNoCancel)
                If dialogresult = DialogResult.Yes Then StartProjectMaintenanceFromSchedulingBoardProject()
                shouldReturn = True : Exit Sub
            End If

            projStartDate = Nothing
            If Not .is_project Then shouldReturn = True : Exit Sub
            projStartDate = CDate(.ProjStartDate)

            partdesc = dsSchedulingBoardData.Tables("Parts").Select("partno = " & SQLQuote(.partno))(0)("partdesc")
            entitydesc = dsSchedulingBoardData.Tables("Part_Schedule").Select("batchno = " & SQLQuote(.entityno))(0)("entitydesc")
            qty_current = dsSchedulingBoardData.Tables("Part_Schedule").Select("partno = " & SQLQuote(.partno) & " And batchno = " & SQLQuote(.entityno) & " And startdate = " & SQLQuote(projStartDate))(0)("qty")
        End With
    End Sub

    Private Sub csmiPartSchedulingDetailSubstitutePart_Click(sender As Object, e As EventArgs) Handles csmiPartSchedulingDetailSubstitutePart.Click

        Dim selectedRow As DataGridViewRow = dgvDetail.SelectedRows(0)

        Dim entityno As String = Nothing
        Dim entitydesc As String = Nothing
        Dim partno As String = Nothing
        Dim partdesc As String
        Dim qty_current As Integer

        For Each cell As DataGridViewCell In selectedRow.Cells
            Dim foo = cell
            Dim col = dgvDetail.Columns(cell.ColumnIndex)
            Select Case col.DataPropertyName
                Case "entityno"
                    entityno = cell.Value
                Case "entitydesc", "Description"
                    entitydesc = cell.Value
                Case "qty"
                    qty_current = cell.Value
                Case "Qty" 'some of the querys that populate the datagrid have different capitaliztaion
                    qty_current = cell.Value
            End Select
        Next

        partno = dtCurrentPart(0).Item("partno")
        partdesc = dtCurrentPart(0).Item("partdesc")

        fSubstitution = New frmScheduleSubstitution(entityno, entitydesc, partno, partdesc, qty_current)
        fSubstitution.Show()
        ' RefreshAvailabilityData()
    End Sub

    Private Sub csmiPartSchedulingDetailCopyPhaseNumberDesc_Click(sender As Object, e As EventArgs) Handles csmiPartSchedulingDetailCopyPhaseNumberDesc.Click
        Dim strClipboard As String = Nothing
        strClipboard = $"{_fieldsAvailableForCopying("entityno")} ({_fieldsAvailableForCopying("entitydesc")})"
        If strClipboard = Nothing Then Exit Sub
        Clipboard.SetText(strClipboard)

    End Sub

    Private Sub csmiPartSchedulingDetailCopyPhaseDesc_Click(sender As Object, e As EventArgs) Handles csmiPartSchedulingDetailCopyPhaseDesc.Click
        Dim strClipboard As String = Nothing
        strClipboard = _fieldsAvailableForCopying("entitydesc")
        If strClipboard = Nothing Then Exit Sub
        Clipboard.SetText(strClipboard)
    End Sub

    Private Sub csmiPartSchedulingDetailCopyPhaseNumber_Click(sender As Object, e As EventArgs) Handles csmiPartSchedulingDetailCopyPhaseNumber.Click
        Dim strClipboard As String = Nothing
        strClipboard = _fieldsAvailableForCopying("entityno")
        If strClipboard = Nothing Then Exit Sub
        Clipboard.SetText(strClipboard)
    End Sub



    Private Sub SubstitutePartToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SubstitutePartToolStripMenuItem.Click
        If chkSummarize.Checked Then
            MsgBox("Substituting parts Is only available when Scheduling Board results are Not summarized.", vbOKOnly)
            Return
        End If

        Dim partdesc As String = Nothing
        Dim entitydesc As String = Nothing
        Dim qty_current As String = Nothing

        Dim lShouldReturn As Boolean
        Dim projStartDate As Date
        GetChangeOrderedInfoDetails(projStartDate, partdesc, entitydesc, qty_current, lShouldReturn)
        If lShouldReturn Then
            Return
        End If

        With GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)
            fSubstitution = New frmScheduleSubstitution(.entityno, .entitydesc, .partno, partdesc, qty_current)
        End With

        fSubstitution.Show()
    End Sub

    Private Sub ScheduleSubstitutionComplete() Handles fSubstitution.NotifyParentToRefresh
        RefreshAvailabilityData()
    End Sub


    Private Sub ChangeOrderedQtyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangeOrderedQtyToolStripMenuItem.Click
        If chkSummarize.Checked Then
            MsgBox("Changing ordered quantity of parts Is only available when Scheduling Board results are Not summarized.", vbOKOnly)
            Return
        End If

        Dim partdesc As String = Nothing
        Dim entitydesc As String = Nothing
        Dim qty_current As String = Nothing

        Dim lShouldReturn As Boolean
        Dim projStartDate As Date
        GetChangeOrderedInfoDetails(projStartDate, partdesc, entitydesc, qty_current, lShouldReturn)
        If lShouldReturn Then
            Return
        End If

EnterQty:

        Dim qty_new As String
        qty_new = InputBox("Change Ordered Qty of [" & partdesc & "] on [" & entitydesc & "] from (" & qty_current & ") to: ", "Change Ordered Qty", qty_current)

        'Nothing was entered
        If qty_new = String.Empty Then
            MsgBox("Action cancelled...")
            Exit Sub
        End If

        'Checking to see if input is a valid number > 0
        Dim iNumber As Integer
        qty_new = qty_new.ToString.Replace("$"c, "").Replace(","c, "")

        If Not Integer.TryParse(qty_new, iNumber) Then
            MsgBox("Please enter a valid quantity.", vbOKOnly)
            GoTo EnterQty
        Else
            If CInt(qty_new) < 0 Then
                MsgBox("Please enter a valid quantity.", vbOKOnly)
                GoTo EnterQty
            End If
        End If

        'Number with greater than 2 digits entered
        If Len(qty_new) > 2 Then
            MsgBox("Really? Quantities with 3 digits are questionable.  Cancelling...")
            Exit Sub
        End If

        Try

            Dim newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()

            Dim sSQL As New StringBuilder

            With GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)
                sSQL.AppendLine("declare @qty_new as int")
                sSQL.AppendLine("set @qty_new = " & qty_new)
                sSQL.AppendLine("update pjjobbudexp set est_qty = @qty_new,")
                sSQL.AppendLine("   est_amount = (select est_unit_amount * @qty_new from pjjobbudexp where entityno = " & SQLQuote(.entityno) & " and partno = " & SQLQuote(.partno) & ")")
                sSQL.AppendLine("where entityno = " & SQLQuote(.entityno))
                sSQL.AppendLine("and partno = " & SQLQuote(.partno))
                'sSQL.AppendLine("and trandate = " & SQLQuote(projStartDate))
                sSQL.AppendLine("and est_qty = " & qty_current)
                sSQL.AppendLine("and line_no <= " & qty_new)
            End With

            newConn.ExecuteNonQuery(sSQL)
            newConn.Close()
            newConn.Dispose()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            RefreshAvailabilityData()
        End Try
    End Sub

#End Region

#Region "Toolstrip"

    Private Sub tsbProjectMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbProjectMaintenance.Click, ProjectMaintenanceToolStripMenuItem.Click
        StartProjectMaintenanceFromSchedulingBoardProject()
    End Sub

    Private Sub StartProjectMaintenanceFromSchedulingBoardProject()
        Dim commandArgs As String = ""

        If Not CurrentFlexGrid.Rows = 0 Then
            With GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)
                If Not String.IsNullOrEmpty(.entityno) Then commandArgs = .entityno
                If .entityno = "INSTOCK" Then commandArgs = ""
                StartFinesseProcess("Project Maintenance.exe", commandArgs)
            End With
        End If
    End Sub
    Private Sub tsbTransferTools_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbTransferTools.Click, TransferToolsToolStripMenuItem.Click
        Dim commandArgs As String = ""

        If Not CurrentFlexGrid.Rows = 0 Then
            With GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)
                If Not String.IsNullOrEmpty(.entityno) Then commandArgs = "entityno:" + .entityno
                If .entityno = "INSTOCK" Then commandArgs = ""
                StartFinesseProcess("TransferInquiryTools.exe", commandArgs)
            End With
        End If
    End Sub

    Private Sub tsbAvailability_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAvailability.Click, AvailabilityToolStripMenuItem.Click
        Dim commandArgs As String = ""
        If Not String.IsNullOrEmpty(GetCurrentPartNo) Then commandArgs = "partno=" & GetCurrentPartNo()
        StartFinesseProcess("prjpjinvcal.exe", commandArgs)
    End Sub

    Private Sub tsbUtilization_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbUtilization.Click
        Dim commandArgs As String = ""

        Dim selectedMenuItem = TryCast(sender, ToolStripMenuItem)

        Dim mySelected_EntityNo = String.Empty
        Dim mySelected_Partno = GetCurrentPartNo()
        Dim mySelected_Warehouse = String.Empty

        Select Case CurrentFlexGrid().Name
            Case fgSchedulingBoard.Name
                With GetSchedulingBoardCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                    If .is_part Then mySelected_Partno = .partno
                    If .is_warehouse Then mySelected_Warehouse = .warehouse
                End With
            Case fgTimeline.Name, fgCalendar.Name
                With GetCalendarCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                    If .is_part Then mySelected_Partno = .partno
                    If .is_warehouse Then mySelected_Warehouse = .warehouse
                End With
        End Select

        If Not String.IsNullOrEmpty(mySelected_Partno) Then commandArgs += " partno:" & mySelected_Partno
        If Not String.IsNullOrEmpty(mySelected_Warehouse) Then commandArgs += " bld:" & mySelected_Warehouse

        StartFinesseProcess("utilization.exe", commandArgs, ExistingWindowBehavior.CreateNew)
    End Sub

    Private Sub tsbPartMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPartMaintenance.Click, PartMaintenanceToolStripMenuItem.Click
        Dim commandArgs As String = ""
        If Not String.IsNullOrEmpty(GetCurrentPartNo) Then commandArgs = GetCurrentPartNo()
        StartFinesseProcess("Part Maintenance.exe", commandArgs)
    End Sub

    Private Sub tsbPartAttachmentMaintenance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbPartAttachmentMaintenance.Click
        Dim commandArgs As String = "partno:" & GetCurrentPartNo()
        StartFinesseProcess("Part Attachments Maintenance.exe", commandArgs)
    End Sub

    Private Sub tsbCrewTools_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbCrewTools.Click
        Dim commandArgs As String = ""

        If Not fgSchedulingBoard.Rows = 0 Then
            With GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)
                Dim arylstSelectedProjectParts As New ArrayList(.entityno.Split("-").ToArray)
                If arylstSelectedProjectParts.Count > 2 Then
                    arylstSelectedProjectParts.RemoveRange(2, arylstSelectedProjectParts.Count - 2)
                End If
                Dim Selected_Leg As String = Join(arylstSelectedProjectParts.ToArray, "-")
                If Not String.IsNullOrEmpty(.entityno) Then commandArgs = "batchno:" + Selected_Leg + " startdate:" + .ProjStartDate + " enddate:" + .ProjEndDate
                If .entityno = "INSTOCK" Then commandArgs = ""
            End With
        End If

        StartFinesseProcess("Crew Tools.exe", commandArgs)
    End Sub

    Private Sub tsbProjectStoryboard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbProjectStoryboard.Click
        Dim commandArgs As String = ""

        If Not fgSchedulingBoard.Rows = 0 Then
            With GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)
                Dim arylstSelectedProjectParts As New ArrayList(.entityno.Split("-").ToArray)
                If arylstSelectedProjectParts.Count > 1 Then
                    arylstSelectedProjectParts.RemoveRange(1, arylstSelectedProjectParts.Count - 1)
                End If
                Dim Selected_Leg As String = Join(arylstSelectedProjectParts.ToArray, "-")
                If Not String.IsNullOrEmpty(.entityno) Then commandArgs = "batchno:" + Selected_Leg
                If .entityno = "INSTOCK" Then commandArgs = ""
            End With
        End If

        StartFinesseProcess("Project Storyboard.exe", commandArgs)
    End Sub

#End Region

#Region "General"

    Private Sub InsertSchedulingBoardRow(ByVal row As Integer)
        fSchedulingBoardCellInfoList.Insert(row, New List(Of SchedulingBoardCellInfo))
    End Sub

    Private Sub tsbShowSearchCriteria_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbShowSearchCriteria.Click
        scParts.Panel1Collapsed = Not scParts.Panel1Collapsed
        _MultiPartGroupsHidden = scParts.Panel1Collapsed
    End Sub

    Private Sub ClearHighlightingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClearHighlightingToolStripMenuItem.Click
        HighlightColumns_Start = -1
        HighlightColumns_End = -1
        PreviousHighlightColumns_Start = -1
        PreviousHighlightColumns_End = -1
        FillSchedulingBoardFlexGrid(dsSchedulingBoardData)
    End Sub

    Private Sub SubstitutePartToolStripMenuItem_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles SubstitutePartToolStripMenuItem.VisibleChanged
        mnuSubstitutePartSeparator.Visible = SubstitutePartToolStripMenuItem.Visible
    End Sub

#End Region

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If m.Msg = WM_COPYDATA Then
            Select Case m.Msg
                Case WM_COPYDATA
                    Dim cds As COPYDATASTRUCT
                    Dim buf(255) As Byte
                    Dim MessageString As String

                    cds = CType(m.GetLParam(GetType(COPYDATASTRUCT)), COPYDATASTRUCT)
                    'Call CopyMemory(cds, m.LParam, Len(cds))

                    Select Case cds.dwData
                        Case 1
                            Debug.Print("got a 1")
                        Case 2
                            Debug.Print("got a 2")
                        Case 3
                            Debug.Print("got a 3!!!!!!!!!!!!!!!!!!!!!!!!!!!")
                            dtFoundPartGroup.Clear()

                            MessageString = Marshal.PtrToStringAnsi(cds.lpData)
                            Dim aryMessageString() = MessageString.Split(" ")

                            If aryMessageString.Length > 0 Then
                                For Each arg In aryMessageString
                                    If InStr(arg.ToString(), "partno:") Then argPartno = Trim(Replace(arg.ToString(), "partno:", ""))
                                    If InStr(arg.ToString(), "partno=") Then argPartno = Trim(Replace(arg.ToString(), "partno=", ""))
                                    If Not chkLockDates.Checked Then
                                        If InStr(arg.ToString(), "startdate=") Then dtpStartDate.Value = CDate(Trim(Replace(arg.ToString(), "startdate=", "")))
                                        If InStr(arg.ToString(), "enddate=") Then dtpEndDate.Value = CDate(Trim(Replace(arg.ToString(), "enddate=", "")))
                                    End If
                                    If InStr(arg.ToString(), "view=") Then argStartupView = Trim(Replace(arg.ToString(), "view=", ""))
                                Next
                            End If

                            Select Case argStartupView
                                Case "Scheduling Board"
                                    If Not tcMain.SelectedTab Is tpSchedulingBoard Then
                                        fgSchedulingBoard.Rows = 0
                                        tcMain.SelectedTab = tpSchedulingBoard
                                    End If
                                Case "Timeline"
                                    If Not tcMain.SelectedTab Is tpTimeline Then
                                        fgTimeline.Rows = 0
                                        tcMain.SelectedTab = tpTimeline
                                    End If
                                Case "Calendar"
                                    If Not tcMain.SelectedTab Is tpCalendar Then
                                        fgCalendar.Rows = 0
                                        tcMain.SelectedTab = tpCalendar
                                    End If
                            End Select
                            Dim stackSize = _stateTracker.UndoStates.Count
                            Reset_SchedulingBoard_Params()
                            If stackSize = _stateTracker.UndoStates.Count Then
                                _stateTracker.Change()
                            End If


                            If Me.WindowState = FormWindowState.Minimized Then Me.WindowState = FormWindowState.Maximized
                            Me.Show()
                            Me.TopMost = True
                            Me.TopMost = False
                    End Select

                    m.Result = New IntPtr(1)
            End Select
        End If
        MyBase.WndProc(m)
    End Sub

    Private Sub btnFindSelectedPartsGroup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindSelectedPartsGroup.Click
        If dtCurrentPart.Rows.Count = 0 Then
            MsgBox("No part currently selected.", vbOKOnly)
            Return
        End If

        'If lstMultiPartList.SelectedItems.Count = 1 Or lstMultiPartList.Items.Count = 1 Then
        'If lstMultiPartList.SelectedItems.Count <> 1 Then lstMultiPartList.SelectedItem = lstMultiPartList.Items(0)

        'argPartno = GetSelectedMultiPartListPartNo()
        If IsNothing(lstMultiPartList.SelectedItem()) Then
            lstMultiPartList.SelectedIndex = 0
        End If
        Dim partNo
        Try

            partNo = lstMultiPartList.SelectedItem().ToString().Split()(0).Trim

        Catch ex As Exception
            MsgBox("No items in partlist to select.", MsgBoxStyle.OkOnly, "Export to Excel")
            Exit Sub
        End Try


        'LoadMultiPartGroupFromPartNo(dtCurrentPart.Rows(0).Item("partno"))
        _stateTracker.is_changing = True
        LoadMultiPartGroupFromPartNo(partNo)
        _stateTracker.is_changing = False
        'End If

        RefreshAvailabilityData()
        _stateTracker.Change()
    End Sub

    Private Sub LoadMultiPartGroupFromPartNo(ByVal partno As String)
        Dim newconn As New SqlConnection(FinesseConnectionString)
        newconn.Open()

        dtFoundPartGroup = newconn.GetDataTable("select partgroup from Avail_Multipart_Groups where partno = '" & partno & "'")

        newconn.Close()
        newconn.Dispose()

        SetMultiPartGroupComboByPartNo(partno)
    End Sub

    Private Sub SetSelectedPartInMultiPartList(ByVal partno As String)
        If String.IsNullOrEmpty(partno) Then Return
        If lstMultiPartList.Items.Count = 0 Then Return

        Dim found = lstMultiPartList.FindString(partno & Space(PartStringSpaces - Len(partno)))

        If found >= 0 Then
            lstMultiPartList.SelectedItem = lstMultiPartList.Items(found)
        End If
    End Sub

    Private Sub cmdExportToExcel_Click(sender As System.Object, e As System.EventArgs) Handles cmdExportToExcel.Click
        Me.Cursor = Cursors.AppStarting

        Select Case CurrentFlexGrid.Name
            Case fgSchedulingBoard.Name
                Dim result = MsgBox("Warning: This process takes a REALLY long time... you may want to get a cup of coffee while you wait.  Are you sure you want to continue?", vbYesNo)
                If result = MsgBoxResult.No Then
                    Return
                End If

                FlexGridToExcel.ExportFlexGridToExcel(fgSchedulingBoard, {0}, {0, 1, 2}, MergeRequired:=True)
            Case fgTimeline.Name
                FlexGridToExcel.ExportFlexGridToExcel(fgTimeline, MergeRequired:=False)
            Case fgCalendar.Name
                FlexGridToExcel.ExportFlexGridToExcel(fgCalendar, MergeRequired:=False)
        End Select

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub chkSummarize_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles chkSummarize.CheckedChanged
        If IsNothing(_stateTracker) OrElse _stateTracker.is_changing Then Exit Sub
        'RefreshAvailabilityData()
        RefreshAvailabilityDataWhenIdleFromDirectAction()

        _stateTracker.Change()
    End Sub

    Private Sub btnLameLink_Click(sender As System.Object, e As System.EventArgs) Handles btnLameLink.Click
        Process.Start(LAMEURL)
    End Sub

    Private Sub chkToggleWarehouse_CheckedChanged(sender As Object, e As EventArgs) Handles chkToggleWarehouse.CheckedChanged
        If IsNothing(_stateTracker) OrElse _stateTracker.is_changing Then Exit Sub
        'RefreshAvailabilityData()
        RefreshAvailabilityDataWhenIdleFromDirectAction()

        _stateTracker.Change()
    End Sub

    Private Sub lblCloseDetailPane_Click(sender As Object, e As EventArgs) Handles lblCloseDetailPane.Click
        scPartsAndDetail.Panel2Collapsed = True
    End Sub

    Private Sub cmdDetailToExcel_Click(sender As Object, e As EventArgs) Handles cmdDetailToExcel.Click
        ExportDetailToExcel()
    End Sub

    Private Sub ExportDetailToExcel()
        If ((dgvDetail.Columns.Count = 0) Or (dgvDetail.Rows.Count = 0)) Then GoTo NoData

        Dim ex As New ExcelExport()
        Dim ds As New DataSet
        ds.Tables.Add()

        'Add columns
        For i As Integer = 0 To dgvDetail.ColumnCount - 1
            ds.Tables(0).Columns.Add(dgvDetail.Columns(i).HeaderText)
        Next

        'Add rows
        Dim dr1 As DataRow
        For i As Integer = 0 To dgvDetail.RowCount - 1
            dr1 = ds.Tables(0).NewRow
            For j As Integer = 0 To dgvDetail.Columns.Count - 1
                dr1(j) = dgvDetail.Rows(i).Cells(j).Value
            Next
            ds.Tables(0).Rows.Add(dr1)
        Next

        ds.Tables(0).TableName = "Parts"

        Dim invisibleColumns As New HashSet(Of String)
        'invisibleColumns.Add("username")

        Dim workbookname As String = "Scheduling Board Part Detail"
        workbookname = Me.Text & " " & lblPartSchedulingDetail.Text
        workbookname = strClean(workbookname)

        ex.DataSetToExcel(ds, Environment.GetEnvironmentVariable("TEMP") & "\" & workbookname & ".xls", "Sheet1", invisibleColumns)

        Exit Sub

NoData:
        MsgBox("No data is available for export.", MsgBoxStyle.OkOnly, "Export to Excel")
    End Sub

    Private Sub RefreshPartScheduleDetails(sender As Object, e As EventArgs)
        If _suspendRefreshData Then Return

        dgvDetail.DataSource = Nothing

        If Not IsDataLoaded Then Return
        If CurrentFlexGrid.Rows = 0 Or CurrentFlexGrid.Cols = 0 Then Return
        If dtCurrentPart.Rows.Count = 0 Then Return

        Static LastSendingLabel As Label
        'Static LastPartNo As String
        'Static LastWarehouse As String

        Dim sendingLabel As Label = TryCast(sender, Label)
        Dim sendingFlexGrid = TryCast(sender, AxMSFlexGridLib.AxMSFlexGrid)

        If sendingLabel IsNot Nothing And dgvDetail.Parent Is scPartsAndDetail.Panel2 Then scPartsAndDetail.Panel2Collapsed = False
        If dgvDetail.Parent Is scPartsAndDetail.Panel2 And scPartsAndDetail.Panel2Collapsed Then Return

        Dim Current_PartNo = dtCurrentPart.Rows(0).Item("partno")

        AllowRepairCopyOptions = False
        Me.Cursor = Cursors.AppStarting

        If Not sendingLabel Is Nothing Then
            Select Case True
                Case sendingLabel.Name.Contains("Repair")

                    AllowRepairCopyOptions = True
                    Dim WH = sendingLabel.Parent.Tag

                    Dim sql As String = $";with DevicesInRepair as (
    select Qty = case when ps.parentunique_no = '' then ps.onhand else 0 end, ChildQty = case when ps.parentunique_no <> '' then ps.onhand else 0 end
    , ps.partno, ps.unique_no, ps.bld, ps.dept, ps.receipt, t.userid, ps.parentunique_no, ps.parentpartno
    from dbo.inpartsub ps
    left outer join dbo.intrans t on ps.partno = t.partno and ps.unique_no = t.unique_no and ps.serial_no = t.serial_no and ps.bld = t.bld and ps.batchno = t.batchno and ps.dept = t.dept and ps.area = t.area and t.qty >= 0 and dbo.fn_datetime_strip_time(ps.receipt) = t.trandate
    where (ps.onhand > 0 or ps.parentunique_no <> '')      
    and ps.batchno = 'REPAIR'
    and ps.partno = {SQLQuote(Current_PartNo)}
    and ps.bld = {SQLQuote(WH)}
), OpenTickets as (	
    select r.partno, r.unique_no, ps.bld, ps.dept, Ticket = r.id_ticket, ra.entry_date, ra.entry_userid ,
    [Repair Ticket Description] = ra.description, ps.parentunique_no, ps.parentpartno
    from dbo.parts_with_open_repair_ticket r
    join dbo.RepairActivities ra on r.id_activity = ra.id_activity
    join dbo.inpartsub ps on r.unique_no = ps.unique_no and (ps.onhand > 0 or ps.parentunique_no <> '')
    where r.partno = {SQLQuote(Current_PartNo)}
    and ps.bld = {SQLQuote(WH)}
    and r.IsOpen = 1
)
select d.Qty, d.ChildQty, partno = coalesce(d.partno, t.partno) ,
    unique_no = coalesce(d.unique_no, t.unique_no) ,
    bld = coalesce(d.bld,t.bld) , dept = coalesce(d.dept,t.dept),
    [Entry Date] = max(coalesce(t.entry_date, d.receipt)) ,			 
    [Entered By] = coalesce(t.entry_userid, d.userid) ,			     
    t.Ticket ,			                                             
    t.[Repair Ticket Description] ,			                         
    parentunique_no = coalesce(d.parentunique_no,t.parentunique_no)	,
    parentpartdesc = pp.partdesc		
from DevicesInRepair d			                                                                                                                                               
full outer join OpenTickets t on d.unique_no = t.unique_no			                                                                                                           
left outer join dbo.inpart pp on coalesce(d.parentpartno,t.parentpartno) = pp.partno				                                                                           
group by d.Qty, d.ChildQty, coalesce(d.partno, t.partno), coalesce(d.unique_no, t.unique_no), coalesce(d.bld,t.bld), coalesce(d.dept, t.dept), coalesce(t.entry_userid, d.userid), t.Ticket, t.[Repair Ticket Description], coalesce(d.parentunique_no,t.parentunique_no), pp.partdesc			                                                                           
"



                    Dim newConn As New SqlConnection(FinesseConnectionString)
                    newConn.Open()
                    Dim t = newConn.GetDataTable(sql)
                    newConn.Close()
                    newConn.Dispose()




                    dgvDetail.DataSource = t

                    dgvDetail.Columns("partno").HeaderText = "Part #"
                    dgvDetail.Columns("unique_no").HeaderText = "Barcode"
                    dgvDetail.Columns("bld").HeaderText = "Warehouse"
                    dgvDetail.Columns("dept").HeaderText = "Owner"
                    dgvDetail.Columns("parentunique_no").HeaderText = "Parent Barcode"
                    dgvDetail.Columns("parentpartdesc").HeaderText = "Parent Part Description"


                    lblPartSchedulingDetail.Text = "Repair Details - " & _dtWarehouses.Select("warehouse_code=" & SQLQuote(WH))(0)("warehouse_description")
                    lblPartSchedulingDetail.ForeColor = lblRepairQty.ForeColor
                    chkGroupByParentProject.Visible = False

                'LastPartNo = Current_PartNo
                'LastWarehouse = WH

                Case sendingLabel.Name.Contains("Late")

                    Dim WH = sendingLabel.Parent.Tag

                    Dim sSQL As New StringBuilder
                    sSQL.AppendLine("select g.proptype, Qty=(j.checkoutqty), g.entityno, g.entitydesc, [Return Warehouse] = case when g.industry <> '' then g.industry else g.agency end, enddate = p.todate")
                    sSQL.AppendLine("from pjjobbudexp p")
                    sSQL.AppendLine("inner join glentities g on p.entityno=g.entityno")
                    sSQL.AppendLine("inner join job_budget_parts_checked_out j with (noexpand) on g.entityno=j.batchno and p.partno=j.partno")
                    sSQL.AppendLine("where p.partno = " & SQLQuote(Current_PartNo))
                    sSQL.AppendLine("and j.checkoutqty<>0")
                    sSQL.AppendLine("and p.todate < dateadd(d,-1,getdate())")
                    sSQL.AppendLine("and (g.agency = " & SQLQuote(WH) & " and g.subcontract<> 'T' or g.industry=" & SQLQuote(WH) & " and g.subcontract='T')")

                    Dim newConn As New SqlConnection(FinesseConnectionString)
                    newConn.Open()
                    Dim t = newConn.GetDataTable(sSQL)
                    newConn.Close()
                    newConn.Dispose()

                    dgvDetail.DataSource = t

                    dgvDetail.Columns("proptype").HeaderText = "Proj Type"
                    dgvDetail.Columns("entityno").HeaderText = "Project #"
                    dgvDetail.Columns("entitydesc").HeaderText = "Project Description"
                    dgvDetail.Columns("enddate").HeaderText = "Late Return Date"

                    lblPartSchedulingDetail.Text = "Late Returns Details - " & _dtWarehouses.Select("warehouse_code=" & SQLQuote(WH))(0)("warehouse_description")
                    lblPartSchedulingDetail.ForeColor = lblLateQty.ForeColor
                    chkGroupByParentProject.Visible = False

                'LastPartNo = Current_PartNo
                'LastWarehouse = WH

                Case sendingLabel.Name.Contains("Planned")

                    Dim WH = sendingLabel.Parent.Tag

                    Dim sSQL As New StringBuilder
                    sSQL.AppendLine("	select Qty = pj.est_qty, g.entityno, g.entitydesc, [Return Warehouse] = wh.returnWH, pj.trandate, pj.todate		")
                    sSQL.AppendLine("	from dbo.pjjobbudexp pj		")
                    sSQL.AppendLine("	join dbo.glentities g on pj.entityno = g.entityno		")
                    sSQL.AppendLine("	cross apply (		")
                    sSQL.AppendLine("		select returnWH = case when isnull(g.industry,'') <> '' then g.industry else g.agency end	")
                    sSQL.AppendLine("	) wh		")
                    sSQL.AppendLine("	where g.proptype = 'NEW'		")
                    sSQL.AppendLine("	and pj.partno = " & SQLQuote(Current_PartNo))
                    sSQL.AppendLine("	and pj.trandate > dbo.today()		")
                    sSQL.AppendLine("	and pj.est_qty > 0		")
                    sSQL.AppendLine("	and wh.returnWH = " & SQLQuote(WH))

                    Dim newConn As New SqlConnection(FinesseConnectionString)
                    newConn.Open()
                    Dim t = newConn.GetDataTable(sSQL)
                    newConn.Close()
                    newConn.Dispose()

                    dgvDetail.DataSource = t

                    dgvDetail.Columns("Qty").HeaderText = "Planned Order Qty"
                    dgvDetail.Columns("entityno").HeaderText = "Project #"
                    dgvDetail.Columns("entitydesc").HeaderText = "Project Description"
                    dgvDetail.Columns("trandate").HeaderText = "Start Date"
                    dgvDetail.Columns("todate").HeaderText = "Due Date"

                    lblPartSchedulingDetail.Text = "Planned Order Details - " & _dtWarehouses.Select("warehouse_code=" & SQLQuote(WH))(0)("warehouse_description")
                    lblPartSchedulingDetail.ForeColor = lblPlannedOrder.ForeColor
                    chkGroupByParentProject.Visible = False

                    'LastPartNo = Current_PartNo
                    'LastWarehouse = WH
            End Select

            If Not sendingLabel Is Nothing Then LastSendingLabel = sendingLabel
        End If

        If Not sendingFlexGrid Is Nothing Then

            Dim Selected_Warehouses = Replace(GetStrIncludedWarehouses(True, False), "'", String.Empty)
            Dim Selected_Date As Date = Nothing

            Dim RunDecisionTree = False

            Select Case CurrentFlexGrid.Name
                Case fgSchedulingBoard.Name
                    With GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)
                        'Selected_Date = IIf(Not .the_date = Nothing, .the_date, .ProjStartDate)
                        If Not String.IsNullOrEmpty(.warehouse) Then Selected_Warehouses = .warehouse
                        If Not String.IsNullOrEmpty(.entityno) And _can_see_part_prices Then RunDecisionTree = True
                    End With
                Case fgTimeline.Name
                    With GetTimelineCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                        'Selected_Date = IIf(Not .the_date = Nothing, .the_date, Today)
                        If Not String.IsNullOrEmpty(.warehouse) Then Selected_Warehouses = .warehouse
                    End With
                Case fgCalendar.Name
                    With GetCalendarCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                        'Selected_Date = IIf(Not .the_date = Nothing, .the_date, Today)
                        If Not String.IsNullOrEmpty(.warehouse) Then Selected_Warehouses = .warehouse
                    End With
            End Select

            If RunDecisionTree Then
                Dim cmd As New SqlCommand("Unavailability_Decision_Tree", GetOpenedFinesseConnection)
                cmd.CommandType = CommandType.StoredProcedure
                'cmd.Connection.Open()

                Dim foo = GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)

                With GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)
                    cmd.Parameters.Add("@warehouse", SqlDbType.VarChar, 100).Value = .warehouse
                    cmd.Parameters.Add("@partno", SqlDbType.VarChar, 50).Value = .partno
                    cmd.Parameters.Add("@startdate", SqlDbType.Date).Value = .ProjStartDate
                    cmd.Parameters.Add("@enddate", SqlDbType.Date).Value = .ProjEndDate
                    cmd.Parameters.Add("@currency", SqlDbType.VarChar, 10).Value = myCurrency
                End With

                Dim dt As New DataTable
                Dim da As New SqlDataAdapter(cmd)
                da.SelectCommand.CommandTimeout = 120

                Dim t As New System.Threading.Tasks.Task(Sub()
                                                             da.Fill(dt)

                                                             Me.Invoke(Sub()
                                                                           dgvDetail.DataSource = dt

                                                                           With dgvDetail
                                                                               .Columns("partno").HeaderText = "Part #"
                                                                               .Columns("bld").HeaderText = "WH"
                                                                               .Columns("entityno").HeaderText = "Upcoming Transfers"
                                                                               .Columns("entitydesc").HeaderText = "Transfer Description"
                                                                               .Columns("startdate").HeaderText = "Start Date"
                                                                               .Columns("enddate").HeaderText = "Due Date"
                                                                               .Columns("DaysBetween").HeaderText = "Transfer Days"
                                                                           End With

                                                                           lblPartSchedulingDetail.Text = "Unavailability Decision Matrix" & IIf(Not String.IsNullOrEmpty(Selected_Warehouses), " - " & Selected_Warehouses, String.Empty) & IIf(Not String.IsNullOrEmpty(Current_PartNo), ": " & Current_PartNo & " - " & GetPartDescFromPartNo(Current_PartNo), String.Empty)
                                                                           lblPartSchedulingDetail.ForeColor = DefaultForeColor
                                                                           chkGroupByParentProject.Visible = True

                                                                       End Sub)
                                                         End Sub)
                t.Start()

            Else
                Dim cmd As New SqlCommand("WarehousePartSchedulingDetail", GetOpenedFinesseConnection)
                cmd.CommandType = CommandType.StoredProcedure
                'cmd.Connection.Open()
                cmd.Parameters.Add("@day", SqlDbType.Date).Value = IIf(Selected_Date = Nothing, DBNull.Value, Selected_Date)
                cmd.Parameters.Add("@partno", SqlDbType.VarChar, 50).Value = Current_PartNo
                cmd.Parameters.Add("@warehouses", SqlDbType.VarChar, 100).Value = Selected_Warehouses
                cmd.Parameters.Add("@summarize", SqlDbType.Bit).Value = chkGroupByParentProject.Checked

                Dim dt As New DataTable
                Dim da As New SqlDataAdapter(cmd)
                da.SelectCommand.CommandTimeout = 120

                Dim t As New System.Threading.Tasks.Task(Sub()
                                                             da.Fill(dt)

                                                             Me.Invoke(Sub()
                                                                           dgvDetail.DataSource = dt

                                                                           With dgvDetail
                                                                               .Columns("proptype").HeaderText = "Proj Type"
                                                                               .Columns("entityno").HeaderText = "Project #"
                                                                               .Columns("trandate").HeaderText = "Start Date"
                                                                               .Columns("todate").HeaderText = "End Date"
                                                                               .Columns("qty").HeaderText = "Qty"
                                                                               .Columns("AcctMgr").HeaderText = "Account Exec"
                                                                               .Columns("WHAvailable").HeaderText = "Warehouse Available"
                                                                               .Columns("SumAvailable").HeaderText = "Sum Available"
                                                                               .Columns("PhaseIsHeldInWarehouse").HeaderText = "Phase Held In WH"
                                                                           End With

                                                                           lblPartSchedulingDetail.Text = "Part Scheduling Detail" & IIf(Not String.IsNullOrEmpty(Selected_Warehouses), " - " & Selected_Warehouses, String.Empty)
                                                                           lblPartSchedulingDetail.ForeColor = DefaultForeColor
                                                                           chkGroupByParentProject.Visible = True
                                                                       End Sub)
                                                         End Sub)
                t.Start()

            End If

            If RunDecisionTree = True Then
                csmiPartSchedulingDetailSubstitutePart.Visible = False
                CreateJustInTimeTransferToolStripMenuItem.Visible = True
            Else
                csmiPartSchedulingDetailSubstitutePart.Visible = True
                CreateJustInTimeTransferToolStripMenuItem.Visible = False
            End If

        End If

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub RefreshAvailabilityDateChangeData()
        If dgvDetail.Parent Is scPartsAndDetail.Panel2 AndAlso scPartsAndDetail.Panel2Collapsed Then Return

        Static lastPartno As String
        Static lastDate As Date

        Dim sendingFlexGrid = TryCast(CurrentFlexGrid(), AxMSFlexGridLib.AxMSFlexGrid)

        Dim partno = String.Empty
        Dim warehouse = String.Empty
        Dim the_date As DateTime

        Select Case sendingFlexGrid.Name
            Case fgTimeline.Name
                With GetTimelineCellInfo(sendingFlexGrid.Col, sendingFlexGrid.Row)
                    If .is_part Then partno = .partno Else Return
                    If .is_date Then the_date = .the_date Else Return
                    If .is_warehouse Then warehouse = .warehouse
                End With
            Case fgCalendar.Name
                With GetCalendarCellInfo(sendingFlexGrid.Col, sendingFlexGrid.Row)
                    If .is_part Then partno = .partno Else Return
                    If .is_date Then the_date = .the_date Else Return
                    If .is_warehouse Then warehouse = .warehouse
                End With
        End Select

        If partno = lastPartno And the_date = lastDate Then Return

        Me.Cursor = Cursors.AppStarting

        If Not sendingFlexGrid Is Nothing Then

            Dim Selected_Warehouses = Replace(GetStrIncludedWarehouses(True, False), "'", String.Empty)

            Dim sSQL As New StringBuilder

            If argStartupMode = "Crew" Then
                sSQL.AppendLine("Select c.entityno, g.entitydesc, Company = bc.Abbreviation, c.jobtype, c.jobdesc, c.fromdate, c.todate, c.crew_size, e.empno, e.firstname, e.lastname, e.gradecd, e.groupno")
                sSQL.AppendLine("From dbo.pjjobbudget c")
                sSQL.AppendLine("Join dbo.glentities g ON g.entityno = c.entityno And g.engactivecd <> 'I'")
                sSQL.AppendLine("Join dbo.Company bc ON bc.CompanyCode = g.BillingCompany")
                sSQL.AppendLine("Join dbo.Days d ON d.Date BETWEEN c.fromdate And c.todate")
                sSQL.AppendLine("Left OUTER JOIN dbo.pjempassign ea ON ea.entityno = c.entityno And ea.jobtype = c.jobtype And ea.empline_no = c.empline_no And ea.StatusCode = 'A'")
                sSQL.AppendLine("Left OUTER JOIN dbo.peemployee e ON e.empno = ea.empno")
                sSQL.AppendLine("WHERE 1 = 1")
                sSQL.AppendLine("And d.Date = " & SQLDate(the_date))
                sSQL.AppendLine("And c.jobtype = " & partno.SQLQuote)
                sSQL.AppendLine("ORDER BY g.entityno")
            Else
                sSQL.AppendLine("Select g.entityno, Qty = case when j.qty > 0 Then j.qty Else -(j.qty) End, [Out] = isNull(pj.line_no, 0), ")
                sSQL.AppendLine("   Direction = Case When j.qty > 0 Then 'OUT' else 'IN' end,")
                sSQL.AppendLine("   g.entitydesc, g.agency, return_warehouse = case when g.industry <> '' then g.industry else g.agency end, AcctMgr = ae.lastname, Operations = ops.lastname")
                sSQL.AppendLine("from job_budgets_parts_transactions j")
                sSQL.AppendLine("join glentities g on g.entityno = j.entityno")
                sSQL.AppendLine("left outer join pjjobbudexp pj on g.entityno = pj.entityno and j.partno = pj.partno")
                sSQL.AppendLine("left outer join peemployee ae on g.respempno = ae.empno")
                sSQL.AppendLine("left outer join peemployee ops on g.opsmgr = ops.empno")
                sSQL.AppendLine("where j.fromdate = " & SQLDate(the_date))
                sSQL.AppendLine("And j.partno = " & partno.SQLQuote)

                If chkSummarize.Checked Then
                    sSQL.AppendLine("And j.bld In (select string from dbo.fn_split(" & Selected_Warehouses.SQLQuote & ",','))")
                Else
                    sSQL.AppendLine("And j.bld = " & warehouse.SQLQuote)
                End If
                sSQL.AppendLine("order by j.qty desc")
            End If

            'Dim newConn As New SqlConnection(FinesseConnectionString)
            'newConn.Open()
            Dim dt = GetOpenedFinesseConnection().GetDataTable(sSQL)
            'newConn.Close()
            'newConn.Dispose()

            dgvDetail.DataSource = dt

            With dgvDetail
                .Columns("entityno").HeaderText = "Project #"
                .Columns("entitydesc").HeaderText = "Project Description"


                If argStartupMode = "Crew" Then
                    .Columns("jobtype").HeaderText = "Job Type"
                    .Columns("jobdesc").HeaderText = "Job Description"
                    .Columns("fromdate").HeaderText = "Start Date"
                    .Columns("todate").HeaderText = "End Date"
                    .Columns("crew_size").HeaderText = "Crew Demand"
                    .Columns("empno").Visible = False
                    .Columns("firstname").HeaderText = "First Name"
                    .Columns("lastname").HeaderText = "Last Name"
                    .Columns("gradecd").HeaderText = "Default Job Type"
                    .Columns("groupno").HeaderText = "Emp Division"

                    lblPartSchedulingDetail.Text = "Crew Detail on Date " & " - " & Selected_Warehouses
                Else

                    .Columns("agency").HeaderText = "Warehouse"
                    .Columns("return_warehouse").HeaderText = "Return Warehouse"
                    .Columns("AcctMgr").HeaderText = "Account Exec"

                    Dim FoundWarehouse = _dtWarehouses.Select("warehouse_code=" & SQLQuote(warehouse))
                    If FoundWarehouse.Any Then
                        lblPartSchedulingDetail.Text = "Part Availability Date Change Detail" & " - " & _dtWarehouses.Select("warehouse_code=" & SQLQuote(warehouse))(0)("warehouse_description")
                    Else
                        lblPartSchedulingDetail.Text = "Part Availability Date Change Detail" & " - " & Selected_Warehouses
                    End If

                End If

            End With

            lblPartSchedulingDetail.ForeColor = DefaultForeColor
        End If

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub dgvDetail_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvDetail.CellDoubleClick
        HandleCellDoubleClick()
    End Sub

    Private Sub HandleCellDoubleClick()
        Dim myDoubleClickProgram As String

        Dim Selected_Project = String.Empty
        Dim Selected_StartDate As Date = Nothing
        Dim Selected_EndDate As Date = Nothing

        If dgvDetail.Columns.Contains("WeekOf") AndAlso Not IsDBNull(dgvDetail.GetSelectedDataRows(Of DataRow).First()("WeekOf")) AndAlso dgvDetail.GetSelectedDataRows(Of DataRow).First()("WeekOf") > 0 AndAlso IsDBNull(dgvDetail.GetSelectedDataRows(Of DataRow).First()("entityno")) Then
            CreateJustInTimeTransfer()
        End If


        If dgvDetail.Columns.Contains("Late") AndAlso Not IsDBNull(dgvDetail.GetSelectedDataRows(Of DataRow).First()("Late")) AndAlso Not IsDBNull(dgvDetail.GetSelectedDataRows(Of DataRow).First()("Late")) > 0 Then
            OpenOutlookMailHTMLBody("Operations" & dgvDetail.GetSelectedDataRows(Of DataRow).First()("bld") & "@clairglobal.com", "Late Returns: " & dgvDetail.GetSelectedDataRows(Of DataRow).First()("partno") & " - " & dgvDetail.GetSelectedDataRows(Of DataRow).First()("partdesc"), "Please check late returns / cycle count Part # " & dgvDetail.GetSelectedDataRows(Of DataRow).First()("partno") & " - " & dgvDetail.GetSelectedDataRows(Of DataRow).First()("partdesc"), String.Empty, String.Empty)
            Return
        End If

        If dgvDetail.Columns.Contains("Repair") AndAlso Not IsDBNull(dgvDetail.GetSelectedDataRows(Of DataRow).First()("Repair")) AndAlso Not IsDBNull(dgvDetail.GetSelectedDataRows(Of DataRow).First()("Repair")) > 0 Then
            OpenOutlookMailHTMLBody("Operations" & dgvDetail.GetSelectedDataRows(Of DataRow).First()("bld") & "@clairglobal.com", "In Repair: " & dgvDetail.GetSelectedDataRows(Of DataRow).First()("partno") & " - " & dgvDetail.GetSelectedDataRows(Of DataRow).First()("partdesc"), "Is it possible to repair any of the (" & dgvDetail.GetSelectedDataRows(Of DataRow).First()("Repair") & ") " & dgvDetail.GetSelectedDataRows(Of DataRow).First()("partno") & " - " & dgvDetail.GetSelectedDataRows(Of DataRow).First()("partdesc") & " currently in REPAIR stock?", String.Empty, String.Empty)
            Return
        End If

        If dgvDetail.Columns.Contains("entityno") AndAlso Not IsDBNull(dgvDetail.GetSelectedDataRows(Of DataRow).First()("entityno")) Then
            Selected_Project = dgvDetail.GetSelectedDataRows(Of DataRow).First()("entityno")
        End If

        If Not String.IsNullOrEmpty(Selected_Project) Then
            myDoubleClickProgram = myDoubleClickProgramWithProject
        Else
            GoTo NoProject
            'myDoubleClickProgram = myDoubleClickProgramWithoutProject
        End If

        If dgvDetail.Columns.Contains("startdate") Then
            Selected_StartDate = dgvDetail.GetSelectedDataRows(Of DataRow).First()("startdate")
        End If

        If dgvDetail.Columns.Contains("enddate") Then
            Selected_EndDate = dgvDetail.GetSelectedDataRows(Of DataRow).First()("enddate")
        End If

        Select Case myDoubleClickProgram
            Case "Project Maintenance.exe"
                StartFinesseProcess("Project Maintenance.exe", Selected_Project)
            Case "TransferInquiryTools.exe"
                Dim commandArgs As String = ""
                If Not String.IsNullOrEmpty(Selected_Project) Then commandArgs = "entityno:" + Selected_Project
                If Not Selected_StartDate = Nothing Then commandArgs += " startdate:" + Selected_StartDate
                If Not Selected_EndDate = Nothing Then commandArgs += " enddate:" + Selected_EndDate

                StartFinesseProcess("TransferInquiryTools.exe", commandArgs)
            Case "prjpjinvcal.exe"
                AvailabilityToolStripMenuItem.PerformClick()
            Case "Scheduling Board.exe"
                StartSchedulingBoard()
            Case "Part Maintenance.exe"
                PartMaintenanceToolStripMenuItem.PerformClick()
            Case "Project Storyboard.exe"
                StartProjectStoryboard()
        End Select

NoProject:

        If dgvDetail.Columns.Contains("ticket") Then
            If Not String.IsNullOrEmpty(dgvDetail.SelectedCells(0).OwningRow.Cells("ticket").Value.ToString) Then
                StartFinesseProcess("Repair.exe", "TicketID:" & dgvDetail.SelectedCells(0).OwningRow.Cells("ticket").Value)
            End If
        End If


    End Sub

    Private Sub StartSchedulingBoard()
        MsgBox("Not Yet Implemented.")
    End Sub
    Private Sub StartProjectStoryboard()
        MsgBox("Not Yet Implemented.")
    End Sub

    Private Sub chkGroupByParentProject_CheckedChanged(sender As Object, e As EventArgs) Handles chkGroupByParentProject.CheckedChanged
        If IsNothing(_stateTracker) OrElse _stateTracker.is_changing Then Exit Sub
        If Not IsDataLoaded Then Return
        RefreshPartScheduleDetails(CurrentFlexGrid, Nothing)

        _stateTracker.Change()
    End Sub

    Private Sub dgvDetail_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvDetail.CellFormatting
        Select Case dgvDetail.Columns(e.ColumnIndex).DataPropertyName
            Case "startdate"
                If Not IsDBNull(e.Value) AndAlso dgvDetail.Columns.Contains("GroundShipEst") Then
                    If e.Value >= GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row).ProjStartDate Then
                        e.CellStyle.ForeColor = Color.Red
                    End If
                End If
            Case "enddate"
                If Not IsDBNull(e.Value) AndAlso dgvDetail.Columns.Contains("GroundShipEst") Then
                    If e.Value > GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row).ProjStartDate Then
                        e.CellStyle.ForeColor = Color.Red
                    End If

                    If e.Value <= GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row).ProjStartDate And
                            DateDiff(DateInterval.Day, e.Value, GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row).ProjStartDate) <= 7 Then

                        If Not IsDBNull(dgvDetail.Rows(e.RowIndex).Cells("Bottleneck").Value) AndAlso dgvDetail.Rows(e.RowIndex).Cells("Bottleneck").Value > 0 Then
                            If Not IsDBNull(e.Value) Then e.CellStyle.BackColor = Color.LightGreen
                        ElseIf Not IsDBNull(dgvDetail.Rows(e.RowIndex).Cells("WeekOf").Value) AndAlso dgvDetail.Rows(e.RowIndex).Cells("Weekof").Value > 0 Then
                            If Not IsDBNull(e.Value) Then e.CellStyle.BackColor = Color.LightYellow
                        End If

                    End If

                End If
            Case "enddate", "todate"
                If Not IsDBNull(e.Value) AndAlso e.Value < Today Then
                    dgvDetail.Rows(e.RowIndex).DefaultCellStyle.ForeColor = Color.Red
                Else
                    dgvDetail.Rows(e.RowIndex).DefaultCellStyle.ForeColor = DefaultForeColor
                End If
            Case "PhaseIsHeldInWarehouse"
                If e.Value = "Y" Then
                    e.CellStyle.BackColor = Color.Goldenrod
                End If
            Case "WHAvailable"
                If e.Value <> String.Empty Then
                    If e.Value < 0 Then
                        dgvDetail.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.Pink
                    Else
                        dgvDetail.Rows(e.RowIndex).DefaultCellStyle.BackColor = Color.LightGreen
                    End If
                End If
            Case "SumAvailable"
                If e.Value <> String.Empty Then
                    If e.Value < 0 Then
                        dgvDetail.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.Pink
                    Else
                        dgvDetail.Rows(e.RowIndex).Cells(e.ColumnIndex).Style.BackColor = Color.LightGreen
                    End If
                End If
            Case "proptype"
                If e.Value = "STORG" Then
                    e.CellStyle.BackColor = Color.Goldenrod
                End If
            Case "entityno"
                'get no here for timeline and calendar
            Case "entitydesc"
                'get desc here for timeline and calendar
            Case "GroundShipEst", "SeaShipEst", "AirShipEst", "PurchaseEst", "SubrentalEst"
                e.CellStyle.Format = "N0"
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
            Case "Bottleneck"
                If Not IsDBNull(e.Value) AndAlso e.Value > 0 Then e.CellStyle.BackColor = Color.LightGreen
                If Not IsDBNull(e.Value) AndAlso e.Value < 0 Then e.CellStyle.ForeColor = Color.Red
            Case "WeekOf"
                If Not IsDBNull(e.Value) AndAlso e.Value > 0 Then e.CellStyle.BackColor = Color.LightYellow
                If Not IsDBNull(e.Value) AndAlso e.Value < 0 Then e.CellStyle.ForeColor = Color.Red
            Case "Late"
                If Not IsDBNull(e.Value) AndAlso e.Value > 0 Then e.CellStyle.BackColor = Color.LightPink
            Case "Repair"
                If Not IsDBNull(e.Value) AndAlso e.Value > 0 Then e.CellStyle.BackColor = Color.Thistle
        End Select
    End Sub

    Private Sub fgCalendar_Resize(sender As Object, e As EventArgs) Handles fgCalendar.Resize
        CalendarAutoSize()
    End Sub

    Private Sub lblPopOut_Click(sender As Object, e As EventArgs) Handles lblPopOut.Click
        Dim f As New frmPopOut
        f.Text = lblPartSchedulingDetail.Text
        f.Height = pnlDetailControls.Height + dgvDetail.Height
        f.Width = pnlDetailControls.Width
        dgvDetail.Parent = f
        pnlDetailControls.Parent = f

        lblPopOut.Visible = False
        lblCloseDetailPane.Visible = False

        scPartsAndDetail.Panel2Collapsed = True

        dgvDetail.BringToFront()
        f.Show(Me)
    End Sub

    Private Sub timer_DoubleClickInterval_Tick(sender As Object, e As EventArgs) Handles timer_DoubleClickInterval.Tick
        timer_DoubleClickInterval.Stop()
        Application.DoEvents()
        RefreshAvailabilityDateChangeData()
    End Sub

    Private Sub cmdAddPartToOrder_Click(sender As Object, e As EventArgs) Handles cmdAddPartToOrder.Click

        Dim sendingButton As Button = TryCast(sender, Button)
        Dim sendingPanel = sendingButton.Parent

        Dim sendingTxtProjectNo = (From c As HighlightTextBox In sendingPanel.Controls.OfType(Of HighlightTextBox)
                                   Where c.Name.Contains("ProjectNo")
                                   Select c).FirstOrDefault

        Dim sendingTxtProjectDesc = (From c As HighlightTextBox In sendingPanel.Controls.OfType(Of HighlightTextBox)
                                     Where c.Name.Contains("ProjectDesc")
                                     Select c).FirstOrDefault

        Dim sendingChkForToday = (From c As CheckBox In sendingPanel.Controls.OfType(Of CheckBox)
                                  Where c.Name.Contains("chkForToday")
                                  Select c).FirstOrDefault


        Dim sendingTxtNote = (From c As HighlightTextBox In sendingPanel.Controls.OfType(Of HighlightTextBox)
                              Where c.Name.Contains("txtNote")
                              Select c).FirstOrDefault


        If sendingTxtProjectNo Is Nothing Then Return


        Dim partno As String
        Dim partdesc As String

        If dtCurrentPart Is Nothing OrElse dtCurrentPart.Rows.Count = 0 Then
            MsgBox("No part has been selected to add to the order.", vbOKOnly)
            Return
        Else
            partno = dtCurrentPart.Rows(0).Item("partno")
            partdesc = dtCurrentPart.Rows(0).Item("partdesc")
        End If

        If dgvDetail.Columns.Contains("GroundShipEst") AndAlso dgvDetail.SelectedRows.Count = 1 AndAlso Not IsDBNull(dgvDetail.SelectedRows(0).Cells("entityno").Value) Then
            sendingTxtProjectNo.Text = dgvDetail.SelectedRows(0).Cells("entityno").Value
            sendingTxtProjectDesc.Text = dgvDetail.SelectedRows(0).Cells("entitydesc").Value
            sendingTxtNote.Text = myUserName.Substring(0, 2).ToUpper & " - " & GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row).entitydesc
        End If

        If String.IsNullOrEmpty(sendingTxtProjectNo.Text) Then
            MsgBox("No order has been selected.")
            Return
        End If

        Dim result As String = String.Empty
        result = InputBox("How many of part " & partno & " (" & partdesc & ") would you like to add to order " & sendingTxtProjectNo.Text & " (" & sendingTxtProjectDesc.Text & ")", "Add Part to Order", 0)

        Dim iResult As Integer

        If Not IsNumeric(result) OrElse result = 0 Then
            Return
        End If

        If Not Integer.TryParse(result, iResult) Then
            MsgBox(result & " is not a whole number and cannot be added to the transfer order.", vbOKOnly)
            Return
        End If

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("--If item already exists on order, update qty")
        sSQL.AppendLine("declare @add_qty int, @useTodaysDate bit, @partno varchar(50), @entityno varchar(50), @note varchar(250)")
        sSQL.AppendLine("set @add_qty = " & result)
        sSQL.AppendLine("set @useTodaysDate = " & IIf(sendingChkForToday.Checked, 1, 0))
        sSQL.AppendLine("set @partno = " & SQLQuote(dtCurrentPart.Rows(0).Item("partno")))
        sSQL.AppendLine("set @entityno = " & SQLQuote(sendingTxtProjectNo.Text))
        sSQL.AppendLine("set @note = " & SQLQuote(sendingTxtNote.Text))
        sSQL.AppendLine("")
        sSQL.AppendLine("update pj set trandate = case when @useTodaysDate = 0 then trandate else cast(floor(cast(getdate() as float))as datetime) end, est_qty = est_qty + @add_qty, est_amount = (est_qty + @add_qty) * est_unit_amount --*/")
        sSQL.AppendLine("from dbo.pjjobbudexp pj    ")
        sSQL.AppendLine("where entityno = @entityno")
        sSQL.AppendLine("and partno = @partno")
        sSQL.AppendLine("")
        sSQL.AppendLine("--If item doesn't exist on the order, insert it")
        sSQL.AppendLine("insert dbo.pjjobbudexp ( entityno ,seqno ,partno ,partentityno ,line_no ,trandate ,todate ,est_unit_amount ,est_qty ,est_amount ,mfg_desc        ) --*/")
        sSQL.AppendLine("select g.entityno, seqno = isnull(max_seqno.max_seqno,0)+1, p.partno, partentityno = 'RENT', line_no = 0, trandate = case when @useTodaysDate = 0 then g.startdate else dbo.today() end, g.enddate, p.totmatcost1, est_qty = @add_qty, est_amount = @add_qty * p.totmatcost1, mfg_desc = @note")
        sSQL.AppendLine("from glentities g")
        sSQL.AppendLine("left outer join pjjobbudexp pj on g.entityno = pj.entityno and pj.partno = @partno")
        sSQL.AppendLine("cross apply (select max_seqno = max(seqno) from pjjobbudexp pj where pj.entityno = @entityno) max_seqno")
        sSQL.AppendLine("cross apply (select p.partno, p.partdesc, p.totmatcost1 from inpart p where p.partno = @partno) p")
        sSQL.AppendLine("where g.entityno = @entityno")
        sSQL.AppendLine("and pj.partno is null")

        Try
            Dim newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()
            newConn.ExecuteNonQuery(sSQL)
            newConn.Close()
            newConn.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        RefreshAvailabilityData()
    End Sub

    Private Sub cmdProjectSearch_Click(sender As Object, e As EventArgs) Handles cmdProjectSearch.Click

        Dim sendingButton As Button = TryCast(sender, Button)
        Dim sendingPanel = sendingButton.Parent

        Dim sendingTxtProjectNo = (From c As HighlightTextBox In sendingPanel.Controls.OfType(Of HighlightTextBox)
                                   Where c.Name.Contains("ProjectNo")
                                   Select c).FirstOrDefault

        Dim sendingTxtProjectDesc = (From c As HighlightTextBox In sendingPanel.Controls.OfType(Of HighlightTextBox)
                                     Where c.Name.Contains("ProjectDesc")
                                     Select c).FirstOrDefault


        If sendingTxtProjectNo Is Nothing Then Return


        Dim picker As New ProjectPicker(Me, sendingButton)

        If picker.GetProject() = True Then
            sendingTxtProjectNo.Text = picker.ProjectNum
            sendingTxtProjectDesc.Text = picker.ProjectNumDesc
            'cmdOpenProject.Enabled = True
        End If
    End Sub

    Private Sub txtProjectNo_TextChanged(sender As Object, e As EventArgs) Handles txtProjectNo.TextChanged

        Dim sendingTextBox As HighlightTextBox = TryCast(sender, HighlightTextBox)
        Dim sendingPanel = sendingTextBox.Parent

        Dim sendingTxtProjectDesc = (From c As HighlightTextBox In sendingPanel.Controls.OfType(Of HighlightTextBox)
                                     Where c.Name.Contains("ProjectDesc")
                                     Select c).FirstOrDefault


        If sendingTxtProjectDesc Is Nothing Then Return

        sendingTxtProjectDesc.Clear()
    End Sub

    Private Sub txtProjectNo_Validating(sender As Object, e As CancelEventArgs) Handles txtProjectNo.Validating

        Dim sendingTextBox As HighlightTextBox = TryCast(sender, HighlightTextBox)
        Dim sendingPanel = sendingTextBox.Parent

        Dim sendingTxtProjectNo = (From c As HighlightTextBox In sendingPanel.Controls.OfType(Of HighlightTextBox)
                                   Where c.Name.Contains("ProjectNo")
                                   Select c).FirstOrDefault

        Dim sendingTxtProjectDesc = (From c As HighlightTextBox In sendingPanel.Controls.OfType(Of HighlightTextBox)
                                     Where c.Name.Contains("ProjectDesc")
                                     Select c).FirstOrDefault


        If sendingTxtProjectNo Is Nothing Then Return


        sendingTxtProjectDesc.Clear()

        If String.IsNullOrEmpty(sendingTxtProjectNo.Text) Then
            'cmdOpenProject.Enabled = False
            Return
        End If

        Dim newConn As New SqlConnection(FinesseConnectionString)
        newConn.Open()

        Dim dtProject = newConn.GetDataTable("select g.entityno, g.entitydesc from glentities g where g.engactivecd <> 'I' and g.entityno = " & sendingTxtProjectNo.Text.SQLQuote)

        newConn.Close()
        newConn.Dispose()

        If dtProject.Rows.Count <> 1 Then
            'MsgBox("Project does not exist.", vbOKOnly)
            'cmdOpenProject.Enabled = False
            e.Cancel = True
        Else
            sendingTxtProjectDesc.Text = dtProject.Rows(0).Item("entitydesc")
            'cmdOpenProject.Enabled = True
        End If
    End Sub

    Private Sub cmdOpenProject_Click(sender As Object, e As EventArgs) Handles cmdOpenProject.Click

        Dim sendingButton = TryCast(sender, Button)
        Dim sendingPanel = sendingButton.Parent

        Dim sendingTxtProjectNo = (From c As HighlightTextBox In sendingPanel.Controls.OfType(Of HighlightTextBox)
                                   Where c.Name.Contains("ProjectNo")
                                   Select c).FirstOrDefault


        If sendingTxtProjectNo Is Nothing Then Return

        Dim commandArgs As String = ""

        If String.IsNullOrEmpty(sendingTxtProjectNo.Text) Then
            MsgBox("No Project selected.", vbOKOnly)
            Return
        End If

        If Not String.IsNullOrEmpty(sendingTxtProjectNo.Text) Then commandArgs = sendingTxtProjectNo.Text.Trim
        StartFinesseProcess("Project Maintenance.exe", commandArgs)
    End Sub

    Private Sub lblCloseAddPartToOrder_Click(sender As Object, e As EventArgs) Handles lblCloseAddPartToOrder.Click

        Dim sendingLabel = TryCast(sender, Label)

        If sendingLabel Is lblCloseAddPartToOrder Then
            pnlAddPartToOrder.Visible = False
        Else
            sendingLabel.Parent.Visible = False
        End If

    End Sub

    Private Sub cmdPartSearch_Click(sender As Object, e As EventArgs) Handles cmdPartSearch.Click
        Static myPartPicker As New PartPicker(Me, cmdPartSearch, FinesseConnectionString)
        If Not myPartPicker.GetPart() Then Return

        Dim partNo = myPartPicker.PartNumber
        Dim partDesc = myPartPicker.PartDesc

        IsDataLoaded = False
        'If Not tcMain.SelectedTab Is tpCalendar Then tcMain.SelectedTab = tpCalendar 'This gets super annoying
        'LoadMultiPartGroupFromPartNo(txtPartNo.Text.Trim) 'Brad requested if he is just looking for a part, that it only displays a part, not the whole group

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("select partno, partdesc from inpart where partno in (select string from fn_split(" & SQLQuote(partNo) & ",',')) order by partdesc asc")
        dtMultiPartList = GetOpenedFinesseConnection.GetDataTable(sSQL)

        _suspendRefreshData = True
        IsRefreshingMultiPartList = True
        cboMultiPartGroups.SelectedIndex = 0

        lstMultiPartList.Items.Clear()
        lstMultiPartList.Items.Add(partNo & Space(PartStringSpaces - Len(partNo)) & partDesc, True)
        lstMultiPartList.Update()

        txtPartNo.Text = partNo
        txtPartDesc.Text = partDesc

        lstMultiPartList.SelectedIndex = 0


        IsRefreshingMultiPartList = False
        _suspendRefreshData = False

        IsDataLoaded = True

        RefreshPartData(txtPartNo.Text)

        RefreshAvailabilityData()

        'txtPartNo.Select()

        '_stateTracker.Change()
    End Sub

    Private Sub txtPartNo_TextChanged(sender As Object, e As EventArgs) Handles txtPartNo.TextChanged
        txtPartDesc.Clear()
    End Sub

    Private Sub TcMain_Changed(sender As Object, e As EventArgs) Handles tcMain.SelectedIndexChanged
        If IsNothing(_stateTracker) Then Exit Sub
        _stateTracker.Change()
    End Sub

    Private Sub txtPartNo_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPartNo.KeyDown
        If IsNothing(_stateTracker) OrElse _stateTracker.is_changing Then Exit Sub
        If e.KeyCode = Keys.Enter Then
            txtPartNo_Validating(sender, New CancelEventArgs)

            _stateTracker.Change()
        End If
    End Sub

    Private Sub txtPartNo_Validating(sender As Object, e As CancelEventArgs) Handles txtPartNo.Validating
        txtProjectDesc.Clear()

        If String.IsNullOrEmpty(txtPartNo.Text) Or LastPartNo = txtPartNo.Text Then Return

        Dim partno = txtPartNo.Text.Trim

        Dim PartDesc As String = GetPartDescFromPartNo(partno)

        If String.IsNullOrEmpty(PartDesc) Then
            e.Cancel = True
        Else
            'lstMultiPartList.SelectedItems.Clear()

            txtPartDesc.Text = PartDesc

            IsDataLoaded = False
            'If Not tcMain.SelectedTab Is tpCalendar Then tcMain.SelectedTab = tpCalendar 'This gets super annoying
            'LoadMultiPartGroupFromPartNo(txtPartNo.Text.Trim) 'Ops wished for this to be turned off

            Dim sSQL As New StringBuilder
            sSQL.AppendLine("select partno, partdesc from inpart where partno in (select string from fn_split(" & SQLQuote(partno) & ",',')) order by partdesc asc")
            dtMultiPartList = GetOpenedFinesseConnection.GetDataTable(sSQL)

            _suspendRefreshData = True
            IsRefreshingMultiPartList = True
            cboMultiPartGroups.SelectedIndex = 0

            lstMultiPartList.Items.Clear()
            lstMultiPartList.Items.Add(partno & Space(PartStringSpaces - Len(partno)) & PartDesc, True)
            lstMultiPartList.Update()

            txtPartDesc.Text = PartDesc

            lstMultiPartList.SelectedIndex = 0


            IsRefreshingMultiPartList = False
            _suspendRefreshData = False

            IsDataLoaded = True

            'RefreshPartScheduleDetails(CurrentFlexGrid, e)
            RefreshPartData(partno)

            RefreshAvailabilityData()

            If tcMain.SelectedTab Is tpCalendar Then
                txtPartNo.SelectAll()
            End If
        End If
    End Sub

    Private Function GetPartDescFromPartNo(ByVal partno As String) As String
        Dim partdesc = String.Empty

        Dim newConn As New SqlConnection(FinesseConnectionString)
        newConn.Open()

        Dim dtPart = newConn.GetDataTable("select p.partno, p.partdesc from inpart p where p.partno = " & partno.SQLQuote)

        If dtCurrentPart Is Nothing Then dtCurrentPart = dtPart
        If dtMultiPartList Is Nothing Then dtMultiPartList = dtPart

        If dtPart.Rows.Count = 1 Then
            partdesc = dtPart.Rows(0).Item("partdesc")
        End If

        newConn.Close()
        newConn.Dispose()

        Return partdesc
    End Function

    Private Function GetCurrentPartNo() As String
        If dtCurrentPart Is Nothing Then Return String.Empty

        If dtCurrentPart.Rows.Count = 1 Then
            Return dtCurrentPart.Rows(0).Item("partno")
        Else
            Return String.Empty
        End If
    End Function

    Private Sub QueryToolsToolStripDropDownButton_Click(sender As Object, e As EventArgs) Handles EquipmentLocationsToolStripMenuItem1.Click,
            TransactionsToolStripMenuItem1.Click,
            PhaseHistoryToolStripMenuItem1.Click,
            GearListToolStripMenuItem1.Click,
            ManifestSummaryToolStripMenuItem1.Click,
            ManifestToolStripMenuItem1.Click,
            CarnetListToolStripMenuItem1.Click,
            TicSheetToolStripMenuItem1.Click,
            LateReturnsToolStripMenuItem1.Click,
            BatchLocationSummaryToolStripMenuItem1.Click

        Dim commandArgs As String = ""

        Dim selectedMenuItem = TryCast(sender, ToolStripMenuItem)

        Dim mySelected_EntityNo = String.Empty
        Dim mySelected_Partno = GetCurrentPartNo()
        Dim mySelected_Warehouse = String.Empty

        Select Case CurrentFlexGrid().Name
            Case fgSchedulingBoard.Name
                With GetSchedulingBoardCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                    If .is_project Then mySelected_EntityNo = .entityno
                    If .is_part Then mySelected_Partno = .partno
                    If .is_warehouse Then mySelected_Warehouse = .warehouse
                End With
            Case fgTimeline.Name
                With GetTimelineCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                    If .is_part Then mySelected_Partno = .partno
                    If .is_warehouse Then mySelected_Warehouse = .warehouse
                End With
            Case fgCalendar.Name
                With GetCalendarCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                    If .is_part Then mySelected_Partno = .partno
                    If .is_warehouse Then mySelected_Warehouse = .warehouse
                End With
        End Select

        If Not String.IsNullOrEmpty(mySelected_EntityNo) Then commandArgs = "batchno:" & mySelected_EntityNo
        If mySelected_EntityNo = "INSTOCK" Then commandArgs = ""

        If Not String.IsNullOrEmpty(mySelected_Warehouse) Then commandArgs += " bld:" & mySelected_Warehouse

        If selectedMenuItem IsNot Nothing Then
            Select Case selectedMenuItem.Name
                Case GearListToolStripMenuItem1.Name,
                    ManifestToolStripMenuItem1.Name,
                    ManifestSummaryToolStripMenuItem1.Name,
                    CarnetListToolStripMenuItem1.Name,
                    TicSheetToolStripMenuItem1.Name
                    'Nothing
                Case Else
                    If Not String.IsNullOrEmpty(mySelected_Partno) Then commandArgs += " partno:" & mySelected_Partno
            End Select

            Select Case selectedMenuItem.Name
                Case EquipmentLocationsToolStripMenuItem1.Name
                    commandArgs += " target:Locations"
                Case TransactionsToolStripMenuItem1.Name
                    commandArgs += " target:Transactions"
                Case PhaseHistoryToolStripMenuItem1.Name
                    commandArgs += " target:PhaseHistory"
                Case GearListToolStripMenuItem1.Name
                    commandArgs += " target:GearList"
                Case ManifestSummaryToolStripMenuItem1.Name
                    commandArgs += " target:ManifestSummary"
                Case ManifestToolStripMenuItem1.Name
                    commandArgs += " target:Manifest"
                Case CarnetListToolStripMenuItem1.Name
                    commandArgs += " target:CarnetList"
                Case TicSheetToolStripMenuItem1.Name
                    commandArgs += " target:TicSheet"
                Case LateReturnsToolStripMenuItem1.Name
                    commandArgs += " target:LateReturns"
                Case BatchLocationSummaryToolStripMenuItem1.Name
                    commandArgs += " target:BatchLocationSummary"
            End Select
        Else
            If Not String.IsNullOrEmpty(mySelected_Partno) Then commandArgs += " partno:" & mySelected_Partno
        End If

        StartFinesseProcess("Query Tools.exe", commandArgs)
    End Sub

    Private Sub tsbBLInq_Click(sender As Object, e As EventArgs) Handles tsbBLInq.Click
        Dim commandArgs As String = ""

        Dim selectedMenuItem = TryCast(sender, ToolStripMenuItem)

        Dim mySelected_EntityNo = String.Empty
        Dim mySelected_Partno = GetCurrentPartNo()
        Dim mySelected_Warehouse = String.Empty

        Select Case CurrentFlexGrid().Name
            Case fgSchedulingBoard.Name
                With GetSchedulingBoardCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                    If .is_project Then mySelected_EntityNo = .entityno
                    If .is_part Then mySelected_Partno = .partno
                    If .is_warehouse Then mySelected_Warehouse = .warehouse
                End With
            Case fgTimeline.Name
                With GetTimelineCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                    If .is_part Then mySelected_Partno = .partno
                    If .is_warehouse Then mySelected_Warehouse = .warehouse
                End With
            Case fgCalendar.Name
                With GetCalendarCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                    If .is_part Then mySelected_Partno = .partno
                    If .is_warehouse Then mySelected_Warehouse = .warehouse
                End With
        End Select

        If Not String.IsNullOrEmpty(mySelected_EntityNo) Then commandArgs = "batchno:" & mySelected_EntityNo
        If Not String.IsNullOrEmpty(mySelected_Partno) Then commandArgs += " partno:" & mySelected_Partno
        If Not String.IsNullOrEmpty(mySelected_Warehouse) Then commandArgs += " bld:" & mySelected_Warehouse

        StartFinesseProcess("prjinblret.exe", commandArgs)
    End Sub

    Private Sub tsbAddInventory_Click(sender As Object, e As EventArgs) Handles tsbAddInventory.Click
        StartFinesseProcess("Add Inventory.exe", GetCurrentPartNo)
    End Sub

    Private Sub tsbRemoveInventory_Click(sender As Object, e As EventArgs) Handles tsbRemoveInventory.Click
        Dim mySelected_EntityNo = String.Empty
        Dim mySelected_Partno = GetCurrentPartNo()

        Select Case CurrentFlexGrid().Name
            Case fgSchedulingBoard.Name
                With GetSchedulingBoardCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                    If .is_project Then mySelected_EntityNo = .entityno
                    If .is_part Then mySelected_Partno = .partno
                End With
            Case fgTimeline.Name
                With GetTimelineCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                    If .is_part Then mySelected_Partno = .partno
                End With
            Case fgCalendar.Name
                With GetCalendarCellInfo(CurrentFlexGrid.Col, CurrentFlexGrid.Row)
                    If .is_part Then mySelected_Partno = .partno
                End With
        End Select

        StartFinesseProcess("Device Maintenance.exe", mySelected_Partno & " " & mySelected_EntityNo)
    End Sub

    Private Sub tsbDeviceMaintenance_Click(sender As Object, e As EventArgs) Handles tsbDeviceMaintenance.Click
        StartFinesseProcess("Device Maintenance.exe", GetCurrentPartNo)
    End Sub

    Private Sub CycleCountUtilityToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CycleCountUtilityToolStripMenuItem.Click
        Dim commandArgs As String = ""
        Dim mySelected_Partno = GetCurrentPartNo()
        If Not String.IsNullOrEmpty(mySelected_Partno) Then commandArgs += "partno:" & mySelected_Partno
        'If Not String.IsNullOrEmpty(GetCurrentPartNo) Then commandArgs = "partno=" & GetCurrentPartNo()
        StartFinesseProcess("Cycle Count.exe", "partno:" & mySelected_Partno)
    End Sub
    Private Sub tsbRepairLog_Click(sender As Object, e As EventArgs) Handles tsbRepairLog.Click
        StartFinesseProcess("Repair.exe", "partno:" & GetCurrentPartNo())
    End Sub

    Private Sub cmdShare_Click(sender As Object, e As EventArgs) Handles cmdShare.Click
        If lstMultiPartList.Items.Count = 0 Then Return
        If String.IsNullOrEmpty(cboMultiPartGroups.Text) Then Return

        Dim username = InputBox("Type the username of the person with whom you want to share this Multi-Part Group.")
        If String.IsNullOrEmpty(username) Then Return

        Dim uConn As New SqlConnection(FinesseConnectionString)
        uConn.Open()

        Dim dtUser = uConn.GetDataTable("select * from dbo.pjtfrusr p where p.user_name = " & username.SQLQuote)

        uConn.Close()
        uConn.Dispose()

        If dtUser.Rows.Count = 0 Then
            MsgBox("User not found.", vbOKOnly)
            Return
        End If

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("	insert dbo.AvailMultipartGroups		")
        sSQL.AppendLine("	        ( userid ,		")
        sSQL.AppendLine("	          partgroup ,		")
        sSQL.AppendLine("	          partno ,		")
        sSQL.AppendLine("	          partseq ,		")
        sSQL.AppendLine("	          partactive		")
        sSQL.AppendLine("	        )		")
        sSQL.AppendLine("	select " & username.SQLQuote & " ,		")
        sSQL.AppendLine("	       amgd.partgroup ,		")
        sSQL.AppendLine("	       amgd.partno ,		")
        sSQL.AppendLine("	       amgd.partseq ,		")
        sSQL.AppendLine("	       amgd.partactive 		")
        sSQL.AppendLine("	from dbo.Avail_Multipart_Groups_Desc amgd		")
        sSQL.AppendLine("	left outer join (		")
        sSQL.AppendLine("		select *	")
        sSQL.AppendLine("		from dbo.AvailMultipartGroups amg	")
        sSQL.AppendLine("		where amg.userid = " & username.SQLQuote)
        sSQL.AppendLine("	) umpg on amgd.partgroup = umpg.partgroup and amgd.partno = umpg.partno		")
        sSQL.AppendLine("	where amgd.partgroup = " & cboMultiPartGroups.Text.SQLQuote)
        sSQL.AppendLine("	and umpg.partno is null		")

        Try
            Dim newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()

            newConn.ExecuteNonQuery(sSQL)

            newConn.Close()
            newConn.Dispose()

            MsgBox(cboMultiPartGroups.Text & " Multi-Part Group shared with " & username & " successfully.", vbOKOnly)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub cmdSearchByBarcode_Click(sender As Object, e As EventArgs) Handles cmdSearchByBarcode.Click
        Dim result = InputBox("Enter a barcode.", "Search by Barcode")
        If String.IsNullOrEmpty(result) Then
            Return
        End If

        Dim newconn As New SqlConnection(FinesseConnectionString)
        newconn.Open()

        Dim dtPart = newconn.GetDataTable("select ps.partno, p.partdesc from inpartsub ps join inpart p on ps.partno = p.partno where (ps.onhand > 0 or ps.parentunique_no <> '') and ps.unique_no = " & result.SQLQuote)

        newconn.Close()
        newconn.Dispose()

        If dtPart.Rows.Count = 0 Then
            MsgBox("Barcode not found.", vbOKOnly)
            Return
        End If

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("select partno, partdesc from inpart where partno in (select string from fn_split(" & SQLQuote(dtPart.Rows(0).Item("partno")) & ",',')) order by partdesc asc")
        dtMultiPartList = GetOpenedFinesseConnection.GetDataTable(sSQL)

        txtPartNo.Text = dtPart.Rows(0).Item("partno")
        txtPartDesc.Text = dtPart.Rows(0).Item("partdesc")

        Dim partno = txtPartNo.Text.Trim
        Dim partdesc = txtPartDesc.Text.Trim

        'IsDataLoaded = False
        'If Not tcMain.SelectedTab Is tpCalendar Then tcMain.SelectedTab = tpCalendar 'This was annoying people
        'LoadMultiPartGroupFromPartNo(txtPartNo.Text.Trim) 'Ops requested this be turned off
        'IsDataLoaded = True

        _suspendRefreshData = True
        IsRefreshingMultiPartList = True
        cboMultiPartGroups.SelectedIndex = 0

        lstMultiPartList.Items.Clear()
        lstMultiPartList.Items.Add(partno & Space(PartStringSpaces - Len(partno)) & partdesc, True)
        lstMultiPartList.Update()

        txtPartDesc.Text = partdesc

        lstMultiPartList.SelectedIndex = 0


        IsRefreshingMultiPartList = False
        _suspendRefreshData = False

        IsDataLoaded = True

        'RefreshPartScheduleDetails(CurrentFlexGrid, e)
        RefreshPartData(partno)

        RefreshAvailabilityData()
    End Sub

    Private Sub MarkAsCountedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MarkAsCountedToolStripMenuItem.Click
        If String.IsNullOrEmpty(GetCurrentPartNo()) Then
            MsgBox("A part must be selected to Mark as Cycle Counted.", vbOKOnly, "")
            Return
        End If

        Dim result = MsgBox("Are you sure you want to mark this item as cycle counted for YOUR warehouse?", vbYesNoCancel, "Mark as Cycle Counted")
        If Not result = vbYes Then Return

        Try
            Dim sSQL As New StringBuilder
            sSQL.AppendLine("INSERT INTO WarehouseCycleCounts (warehouse, partno, lastcount, username)")
            sSQL.AppendLine("select warehouse=warehouse_entity, partno = " & SQLQuote(GetCurrentPartNo()) & ", GETDATE(), SUSER_SNAME()")
            sSQL.AppendLine("from pjtfrusr p")
            sSQL.AppendLine("join mudbmsinfo i on p.user_name = i.username")

            Dim newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()

            newConn.ExecuteNonQuery(sSQL)

            newConn.Close()
            newConn.Dispose()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub lblCollapseExpandWarehouses_Click(sender As Object, e As EventArgs) Handles lblCollapseExpandWarehouses.Click
        Dim newText = lblCollapseExpandWarehouses.Tag

        lblCollapseExpandWarehouses.Tag = lblCollapseExpandWarehouses.Text
        lblCollapseExpandWarehouses.Text = newText

        Select Case lblCollapseExpandWarehouses.Tag
            Case "-"
                pnlTotalsAndWarehouses.Height = 42
                _WarehousesPaneMinimized = True
            Case Else
                pnlTotalsAndWarehouses.Height = 98
                _WarehousesPaneMinimized = False
        End Select
    End Sub

    Private Sub txtPartNo_Enter(sender As Object, e As EventArgs) Handles txtPartNo.Enter
        txtPartNo.Select()
    End Sub





    Private Sub lblCollapseMultiPartGroups_Click(sender As Object, e As EventArgs) Handles lblCollapseMultiPartGroups.Click
        scParts.Panel1Collapsed = True
        _MultiPartGroupsHidden = True
    End Sub

    Private Sub tcMain_Click(sender As Object, e As EventArgs) Handles tcMain.Click
        If Not IsDataLoaded Then Return
        If String.IsNullOrEmpty(GetCurrentPartNo) Then Return

        RefreshAvailabilityData()
    End Sub



    Private Sub dgvDetail_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles dgvDetail.CellMouseDown
        If Not IsDataLoaded Then Exit Sub
        _fieldsAvailableForCopying.Clear()

        ChangeOrderedQtyToolStripMenuItem.Visible = False
        mnuSubstitutePartSeparator.Visible = False
        ClearHighlightingToolStripMenuItem.Visible = False
        ToolStripSeparator2.Visible = False
        ProjectMaintenanceToolStripMenuItem.Visible = False
        TransferToolsToolStripMenuItem.Visible = False
        AvailabilityToolStripMenuItem.Visible = False
        PartMaintenanceToolStripMenuItem.Visible = False
        SubstitutePartToolStripMenuItem.Visible = False
        ChangeOrderedQtyToolStripMenuItem.Visible = False
        ToolStripSeparator3.Visible = False
        CopyPartBarcodeToolStripMenuItem.Visible = False
        CopyPartNumberToolStripMenuItem.Visible = False
        CopyPartDescriptionToolStripMenuItem.Visible = False
        CopyPartNumberAndDescriptionToolStripMenuItem.Visible = False
        CopyPhaseNumberToolStripMenuItem.Visible = False
        CopyPhaseDescriptionToolStripMenuItem.Visible = False
        CopyPhaseNumberAndDescriptionToolStripMenuItem.Visible = False

        Try
            If Not AllowRepairCopyOptions Then
                CopyPhaseNumberToolStripMenuItem.Visible = True
                CopyPhaseDescriptionToolStripMenuItem.Visible = True
                CopyPhaseNumberAndDescriptionToolStripMenuItem.Visible = True
                If dgvDetail.Columns.Contains("entityno") Then
                    _fieldsAvailableForCopying.Add("entityno", dgvDetail.GetSelectedDataRows(Of DataRow).First()("entityno"))
                End If
                If dgvDetail.Columns.Contains("Description") Then
                    _fieldsAvailableForCopying.Add("entitydesc", dgvDetail.GetSelectedDataRows(Of DataRow).First()("Description"))
                End If
                If dgvDetail.Columns.Contains("entitydesc") Then
                    _fieldsAvailableForCopying.Add("entitydesc", dgvDetail.GetSelectedDataRows(Of DataRow).First()("entitydesc"))
                End If
            Else
                CopyPartBarcodeToolStripMenuItem.Visible = True
                CopyPartNumberToolStripMenuItem.Visible = True
                CopyPartDescriptionToolStripMenuItem.Visible = True
                CopyPartNumberAndDescriptionToolStripMenuItem.Visible = True
                If dgvDetail.Columns.Contains("unique_no") Then
                    If dgvDetail.GetSelectedDataRows(Of DataRow).First()("unique_no") = "" Then
                        _fieldsAvailableForCopying.Add("unique_no", "")
                    Else
                        _fieldsAvailableForCopying.Add("unique_no", dgvDetail.GetSelectedDataRows(Of DataRow).First()("unique_no"))
                    End If
                    If dgvDetail.Columns.Contains("partno") Then
                        _fieldsAvailableForCopying.Add("partno", dgvDetail.GetSelectedDataRows(Of DataRow).First()("partno"))
                    End If
                    If txtPartDesc.Text IsNot Nothing Then _fieldsAvailableForCopying.Add("partdesc", txtPartDesc.Text)
                End If
            End If
        Catch ex As Exception
            'Nothing, only handles null value cells
        End Try

        Dim addMultipartGroupPanelWidth As Double = 0
        If pnlMultiPartGroups.Location.X = 0 Then
            addMultipartGroupPanelWidth = pnlMultiPartGroups.Width
        End If

        If e.Button = 2 Then
            Dim p = Me.PointToScreen(scParts.Location + tcMain.Location + dgvDetail.Location)
            mnuFlexgrid.Show(addMultipartGroupPanelWidth + tsMainVert.Width + p.X + e.X, pnlControls.Height + pnlWarehouses.Height + p.Y + e.Y)
        End If
    End Sub

    Private Sub CopyPartBarcodeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyPartBarcodeToolStripMenuItem.Click
        Dim strClipboard As String = Nothing
        strClipboard = _fieldsAvailableForCopying("unique_no")
        If strClipboard = Nothing Then Exit Sub
        Clipboard.SetText(strClipboard)
    End Sub

    Private Sub CopyPartNumberToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyPartNumberToolStripMenuItem.Click
        Dim strClipboard As String = Nothing
        strClipboard = _fieldsAvailableForCopying("partno")
        If strClipboard = Nothing Then Exit Sub
        Clipboard.SetText(strClipboard)
    End Sub

    Private Sub CopyPartDescriptionToolStripMenuItem_click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyPartDescriptionToolStripMenuItem.Click
        Dim strClipboard As String = Nothing
        strClipboard = GetPartDescFromPartNo(_fieldsAvailableForCopying("partno"))
        If strClipboard = Nothing Then Exit Sub
        Clipboard.SetText(strClipboard)
    End Sub

    Private Sub CopyPartNumberAndDescriptionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyPartNumberAndDescriptionToolStripMenuItem.Click
        Dim strClipboard As String = Nothing
        strClipboard = $"{_fieldsAvailableForCopying("partno")} ({GetPartDescFromPartNo(_fieldsAvailableForCopying("partno"))})"
        If strClipboard = Nothing Then Exit Sub
        Clipboard.SetText(strClipboard)
    End Sub

    Private Sub CopyPhaseNumberToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyPhaseNumberToolStripMenuItem.Click
        Dim strClipboard As String = Nothing
        strClipboard = _fieldsAvailableForCopying("entityno")
        If strClipboard = Nothing Then Exit Sub
        Clipboard.SetText(strClipboard)
    End Sub

    Private Sub CopyPhaseDescriptionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyPhaseDescriptionToolStripMenuItem.Click
        Dim strClipboard As String = Nothing
        strClipboard = _fieldsAvailableForCopying("entitydesc")
        If strClipboard = Nothing Then Exit Sub
        Clipboard.SetText(strClipboard)
    End Sub

    Private Sub CopyPhaseNumberAndDescriptionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CopyPhaseNumberAndDescriptionToolStripMenuItem.Click
        Dim strClipboard As String = Nothing
        strClipboard = $"{_fieldsAvailableForCopying("entityno")} ({_fieldsAvailableForCopying("entitydesc")})"
        If strClipboard = Nothing Then Exit Sub
        Clipboard.SetText(strClipboard)
    End Sub

    Private Sub txtPartDesc_TextChanged(sender As Object, e As EventArgs) Handles txtPartDesc.TextChanged
        If IsNothing(_stateTracker) OrElse _stateTracker.is_changing Then Exit Sub
        If Not String.IsNullOrEmpty(txtPartDesc.Text) Then
            LastPartNo = txtPartNo.Text

            _stateTracker.Change()
        End If
    End Sub

    Private Sub _stateTracker_OnRefresh(sender As Object) Handles _stateTracker.OnRefresh
        RefreshAvailabilityData()
        GetSelectedMultiPartListPartNo()
    End Sub

    Private Sub CmdHelp_Click(sender As Object, e As EventArgs) Handles cmdHelp.Click
        Process.Start("http://pa-finessedocs/applications:scheduling_board")
    End Sub

    Private Sub LblProjectName_Click(sender As Object, e As EventArgs) Handles LblProjectName.Click

    End Sub

    Private Sub LblProjectName_VisibleChanged(sender As Object, e As EventArgs) Handles LblProjectName.VisibleChanged
        pbEmailTeam.Visible = LblProjectName.Visible
    End Sub

    Private Sub PbEmailTeam_Click(sender As Object, e As EventArgs) Handles pbEmailTeam.Click
        Dim cellInfo = GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)

        If cellInfo.entityno Is Nothing OrElse String.IsNullOrEmpty(cellInfo.entityno) Then Return

        Using newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()

            Dim dtTeam = newConn.GetDataTable("SELECT pj.entityno, pj.entitydesc, aeEmail = case when me.email <> ae.email then ae.email else null end, opsEmail = case when me.email <> ops.email then ops.email else null end
FROM dbo.glentities pj
left outer join dbo.peemployee ae on pj.respempno = ae.empno
left outer join dbo.peemployee ops on pj.opsmgr = ops.empno
cross apply (select email from dbo.my_user_info) me
where pj.entityno = " & cellInfo.entityno.SQLQuote)

            If dtTeam.Rows.Count <> 1 Then Return

            OpenOutlookMail(
                    ToAddress:=ReplaceNull(dtTeam.Rows(0).Item("aeEmail"), String.Empty) & ";" & ReplaceNull(dtTeam.Rows(0).Item("opsEmail"), String.Empty),
                    Subject:=cellInfo.entityno & " (" & cellInfo.entitydesc & ")", Body:=String.Empty, AttachmentPath:=Nothing, HtmlBody:=False, CCAddress:=String.Empty)

        End Using


    End Sub

    Private Sub LblProjectName_TextChanged(sender As Object, e As EventArgs) Handles LblProjectName.TextChanged
        pbEmailTeam.Visible = Not String.IsNullOrEmpty(LblProjectName.Text)
    End Sub

    Private Sub RunRefreshToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RunRefreshToolStripMenuItem.Click
        ChosenDisplayWarehouses.Clear()
        ChosenDisplayWarehouses.AddRange(
            From p In pnlWarehouses.Controls.OfType(Of Panel)
            Where p.Controls.OfType(Of CheckBox).First.Checked
            Select CType(p.Tag, String)
        )
        WarehousesDisplayMode = WarehouseListDisplayMode.OnlyChosenWarehouses
    End Sub

    Private Sub ShowAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowAllToolStripMenuItem.Click
        WarehousesDisplayMode = WarehouseListDisplayMode.AllWarehouses

        RefreshPartWarehouseQtys()

    End Sub

    Private Sub ResetMyMultipartGroupsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ResetMyMultipartGroupsToolStripMenuItem.Click
        Dim answer As DialogResult
        answer = MessageBox.Show("Would you like to reset your multipart groups to the default?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If answer = vbYes Then
            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()
                Dim cmd As New SqlCommand($"EXEC dbo.restore_users_multipartgroups_to_default @userName = {GetEnvironmentVariable("ESSUID")}", newConn)
                cmd.ExecuteNonQuery()
            End Using
        End If
    End Sub



#Region "Warehouse Groups"
    Private Sub initWarehouseGroups()

        For Each group As UserWarehouseGroupsRow In _dtUserWarehouseGroups.Rows

            Dim arr As String() = New String(1) {}
            Dim item As ListViewItem = New ListViewItem

            arr(0) = group.WarehouseGroup

            item = New ListViewItem(arr)
            lvWarehouseGroups.Items.Add(item)
        Next

        lvWarehouseGroups.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
    End Sub


    Private Sub tsmiCreateNewWarehouseGroup_Click(sender As Object, e As EventArgs) Handles tsmiCreateNewWarehouseGroup.Click
        Dim newGroupName As Object
        newGroupName = InputBox("What would you like to name the group?", "New Group")

        If newGroupName = "" Then
            Exit Sub
        End If

        Dim newGroup As UserWarehouseGroupsRow = _dtUserWarehouseGroups.NewUserWarehouseGroupsRow

        newGroup.Item("WarehouseGroup") = newGroupName

        _dtUserWarehouseGroups.Rows.Add(newGroup)

        Dim arr As String() = New String(1) {}
        Dim item As ListViewItem = New ListViewItem

        arr(0) = newGroupName

        item = New ListViewItem(arr)
        lvWarehouseGroups.Items.Add(item)

        WarehousesDisplayMode = WarehouseListDisplayMode.AllWarehouses

        RemoveHandler lvWarehouseGroups.SelectedIndexChanged, AddressOf lvWarehouseGroups_AfterSelect
        item.Selected = True
        AddHandler lvWarehouseGroups.SelectedIndexChanged, AddressOf lvWarehouseGroups_AfterSelect

        setCurrentWarehouseGroup()
        RefreshAvailabilityDataWhenIdleFromDirectAction()
    End Sub


    Private Sub lvWarehouseGroups_AfterSelect(sender As Object, e As EventArgs)
        If lvWarehouseGroups.SelectedItems.Count = 0 Then
            Exit Sub
        End If

        'unselect currently selected warehouses
        'pretty hacky there has to be a better way to do this
        For Each iPanel In pnlWarehouses.Controls.OfType(Of Panel)
            iPanel.Controls.OfType(Of CheckBox).First.Checked = False
        Next

        Dim warehouses As userWarehouseGroupsWarehousesRow() = _dtWarehouseGroupWarehouses.Select($"WarehouseGroup = '{lvWarehouseGroups.SelectedItems(0).Text}'")

        For Each warehouse In warehouses
            For Each iPanel In pnlWarehouses.Controls.OfType(Of Panel)
                If iPanel.Tag = warehouse.WarehouseCode Then
                    iPanel.Controls.OfType(Of CheckBox).First.Checked = True
                End If
            Next
        Next

        If Not IsDataLoaded Then Exit Sub
        If txtPartDesc.Text = "" Or txtPartNo.Text = "" Then Exit Sub

        ChosenDisplayWarehouses.Clear()
        ChosenDisplayWarehouses.AddRange(
            From p In pnlWarehouses.Controls.OfType(Of Panel)
            Where p.Controls.OfType(Of CheckBox).First.Checked
            Select CType(p.Tag, String)
        )

        WarehousesDisplayMode = WarehouseListDisplayMode.OnlyChosenWarehouses

        RefreshAvailabilityDataWhenIdleFromDirectAction()
    End Sub

    Private Sub tsmiSaveGroups_Click(sender As Object, e As EventArgs) Handles tsmiSaveGroups.Click
        SaveData()
    End Sub

    Private Sub lvWarehouseGroups_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.LabelEditEventArgs) Handles lvWarehouseGroups.AfterLabelEdit

        Dim oldName As String = lvWarehouseGroups.SelectedItems(0).Text
        Dim newName As String = e.Label


        If newName Is String.Empty Or newName Is Nothing Then
            Exit Sub
        End If

        Dim group As UserWarehouseGroupsRow() = _dtUserWarehouseGroups.Select($"WarehouseGroup = '{oldName}'")

        If group.Any Then
            group(0).WarehouseGroup = newName
        End If


        Dim groupWarehouses As userWarehouseGroupsWarehousesRow() = _dtWarehouseGroupWarehouses.Select($"WarehouseGroup = '{oldName}'")

        For Each warehouse As userWarehouseGroupsWarehousesRow In groupWarehouses
            warehouse.WarehouseGroup = newName
        Next

    End Sub

    Private Sub tsmiRenameWarehouseGroup_Click(sender As Object, e As EventArgs) Handles tsmiRenameWarehouseGroup.Click
        lvWarehouseGroups.SelectedItems(0).BeginEdit()
    End Sub

    Private Sub tsmiDeleteWarehouseGroup_Click(sender As Object, e As EventArgs) Handles tsmiDeleteWarehouseGroup.Click

        If lvWarehouseGroups.SelectedItems.Count = 0 Then
            MessageBox.Show("You do not have a warehouse group select. Please Select One.")
            Exit Sub
        End If

        Select Case MsgBox($"Are you sure you want to delete this warehouse group ({lvWarehouseGroups.SelectedItems(0).Text})?", MsgBoxStyle.YesNo, "Delete warehouse group")
            Case MsgBoxResult.Yes
                DeleteCurrentlySelectedWarehouseGroup()
            Case MsgBoxResult.No
                Exit Sub
        End Select
    End Sub

    Private Sub DeleteCurrentlySelectedWarehouseGroup()
        Dim warehouseGruops As UserWarehouseGroupsRow() = _dtUserWarehouseGroups.Select($"WarehouseGroup = '{lvWarehouseGroups.SelectedItems(0).Text}'")

        Dim warehouses As userWarehouseGroupsWarehousesRow() = _dtWarehouseGroupWarehouses.Select($"WarehouseGroup = '{lvWarehouseGroups.SelectedItems(0).Text}'")


        For Each warehouse In warehouses
            warehouse.Delete()
        Next

        For Each warehouseGroup In warehouseGruops
            warehouseGroup.Delete()
        Next

        lvWarehouseGroups.SelectedItems(0).Remove()
    End Sub



    Private Sub tsmiOverwriteWarehoseGroup_Click(sender As Object, e As EventArgs) Handles tsmiOverwriteWarehoseGroup.Click
        setCurrentWarehouseGroup()
    End Sub


    Private Sub setCurrentWarehouseGroup()

        If lvWarehouseGroups.SelectedItems.Count = 0 Then
            MessageBox.Show("You do not have a warehouse group select. Please Select One.")
            Exit Sub
        End If

        For Each iPanel In pnlWarehouses.Controls.OfType(Of Panel)

            If iPanel.Tag Is Nothing Then
                Continue For
            End If

            Dim isWarehouseAlreadyinGroup As userWarehouseGroupsWarehousesRow() = _dtWarehouseGroupWarehouses.Select($"WarehouseGroup = '{lvWarehouseGroups.SelectedItems(0).Text}' AND WarehouseCode = '{iPanel.Tag}'")

            If iPanel.Controls.OfType(Of CheckBox).First.Checked = True Then


                If Not isWarehouseAlreadyinGroup.Any Then

                    Dim newWarehouseGroupWarehouseRow As userWarehouseGroupsWarehousesRow = _dtWarehouseGroupWarehouses.NewuserWarehouseGroupsWarehousesRow
                    newWarehouseGroupWarehouseRow.WarehouseCode = iPanel.Tag
                    newWarehouseGroupWarehouseRow.WarehouseGroup = lvWarehouseGroups.SelectedItems(0).Text
                    _dtWarehouseGroupWarehouses.AdduserWarehouseGroupsWarehousesRow(newWarehouseGroupWarehouseRow)

                End If
            Else

                If isWarehouseAlreadyinGroup.Any Then
                    '_dtWarehouseGroupWarehouses.RemoveuserWarehouseGroupsWarehousesRow(isWarehouseAlreadyinGroup(0))
                    isWarehouseAlreadyinGroup(0).Delete()
                End If

            End If

        Next

        SaveData()
    End Sub



    Private Sub SaveData()
        If Not Me.Validate Then
            Exit Sub
        End If

        Me.Cursor = Cursors.WaitCursor

        Try
            Dim gconn As SqlConnection = GetOpenedFinesseConnection()

            Dim sSQL As New StringBuilder
            sSQL.AppendLine("delete from dbo.UserWarehouseGroupsBulkSave where session_id = @@spid;")
            sSQL.AppendLine("delete from dbo.userWarehouseGroupsWarehousesBulkSave where session_id = @@spid;")
            gconn.ExecuteNonQuery(sSQL)

            BulkSave(gconn, _dtUserWarehouseGroups, "UserWarehouseGroupsBulkSave", "WarehouseGroup")
            BulkSave(gconn, _dtWarehouseGroupWarehouses, "userWarehouseGroupsWarehousesBulkSave", "WarehouseGroup")

            Dim cmd As New SqlCommand("commit_Scheduling_Board", gconn)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandTimeout = 120
            cmd.ExecuteNonQuery()

            _dsSchedulingBoardDataSet.AcceptChanges()
        Catch ex As Exception
            MessageBox.Show("Error While Saving: " & ex.Message)
        End Try

        Me.Cursor = Cursors.Default
    End Sub

    Private Sub BulkSave(ByVal connection As SqlConnection, ByVal tSource As System.Data.DataTable, ByVal destinationTable As String, ByVal ParamArray editableKeyFields() As String)
        Dim rowsToSave = tSource.Select("", "", DataViewRowState.Added Or DataViewRowState.Deleted Or DataViewRowState.ModifiedOriginal Or DataViewRowState.ModifiedCurrent)

        If rowsToSave.Length = 0 Then
            Return
        End If

        Dim tTemp As New System.Data.DataTable

        tTemp.Columns.Add("is_insert", GetType(Boolean)).DefaultValue = False
        tTemp.Columns.Add("is_update", GetType(Boolean)).DefaultValue = False
        tTemp.Columns.Add("is_delete", GetType(Boolean)).DefaultValue = False

        For Each c As DataColumn In tSource.Columns
            If Not c.ReadOnly Then ' assuming that read-only columns will by calculated automatically by the DB
                tTemp.Columns.Add(c.ColumnName, c.DataType)
            End If
        Next


        For Each r As DataRow In rowsToSave
            Select Case r.RowState
                Case DataRowState.Added, DataRowState.Deleted, DataRowState.Modified
                    tTemp.ImportRow(r)
                Case Else
                    Debug.Assert(r.RowState = DataRowState.Unchanged)
            End Select
        Next


        For Each keyField In editableKeyFields
            tTemp.Columns.Add(keyField & "_old", GetType(String))
            tTemp.Columns.Add(keyField & "_new", GetType(String))
        Next

        For Each r As DataRow In tTemp.Rows
            Select Case r.RowState
                Case DataRowState.Added
                    r("is_insert") = True
                    For Each keyField In editableKeyFields
                        r(keyField & "_new") = r(keyField, DataRowVersion.Current)
                        r(keyField & "_old") = r(keyField & "_new")
                    Next
                Case DataRowState.Modified
                    r("is_update") = True
                    For Each keyField In editableKeyFields
                        r(keyField & "_old") = r(keyField, DataRowVersion.Original)
                        r(keyField & "_new") = r(keyField, DataRowVersion.Current)
                    Next
                Case DataRowState.Deleted
                    r.RejectChanges() ' necessary so that we can look at and modify fields.
                    r("is_delete") = True
                    For Each keyField In editableKeyFields
                        r(keyField & "_old") = r(keyField, DataRowVersion.Original)
                        r(keyField & "_new") = r(keyField & "_old")
                    Next
            End Select
        Next

        For Each keyField In editableKeyFields
            tTemp.Columns.Remove(keyField)
        Next

        Using bulkCopy As New SqlBulkCopy(connection, SqlBulkCopyOptions.CheckConstraints Or SqlBulkCopyOptions.FireTriggers, Nothing)
            bulkCopy.DestinationTableName = destinationTable
            bulkCopy.BatchSize = 0

            bulkCopy.ColumnMappings.Clear()
            For Each c As DataColumn In tTemp.Columns
                Debug.Print(c.ColumnName)
                bulkCopy.ColumnMappings.Add(c.ColumnName, c.ColumnName)
            Next

            bulkCopy.WriteToServer(tTemp)
        End Using
    End Sub

    Private Sub cmdAddTransfer_Click(sender As Object, e As EventArgs) Handles cmdAddTransfer.Click
        Dim new_pnlAddPartToOrder As New Panel
        With new_pnlAddPartToOrder
            .Size = pnlAddPartToOrder.Size
            .Location = pnlAddPartToOrder.Location
            .Dock = pnlAddPartToOrder.Dock
            .Visible = True
            .BorderStyle = pnlAddPartToOrder.BorderStyle
            .Tag = CInt(pnlAddPartToOrder.Tag) + 1
        End With

        Dim new_cmdAddPartToOrder As New Button
        With new_cmdAddPartToOrder
            .Name = cmdAddPartToOrder.Name & new_pnlAddPartToOrder.Tag
            .Text = cmdAddPartToOrder.Text
            .Size = cmdAddPartToOrder.Size
            .Location = cmdAddPartToOrder.Location
            .Anchor = cmdAddPartToOrder.Anchor
            .Visible = True
            .Tag = new_pnlAddPartToOrder.Tag
            AddHandler new_cmdAddPartToOrder.Click, AddressOf cmdAddPartToOrder_Click
        End With
        new_pnlAddPartToOrder.Controls.Add(new_cmdAddPartToOrder)


        Dim new_cmdProjectSearch As New Button
        With new_cmdProjectSearch
            .Name = cmdProjectSearch.Name & new_pnlAddPartToOrder.Tag
            .Text = cmdProjectSearch.Text
            .Size = cmdProjectSearch.Size
            .Location = cmdProjectSearch.Location
            .Anchor = cmdProjectSearch.Anchor
            .Visible = True
            .Tag = new_pnlAddPartToOrder.Tag
            AddHandler new_cmdProjectSearch.Click, AddressOf cmdProjectSearch_Click
        End With
        new_pnlAddPartToOrder.Controls.Add(new_cmdProjectSearch)

        Dim new_txtProjectNo As New HighlightTextBox
        With new_txtProjectNo
            .Name = txtProjectNo.Name & new_pnlAddPartToOrder.Tag
            '.Text = txtProjectNo.Text
            .Size = txtProjectNo.Size
            .Location = txtProjectNo.Location
            .Anchor = txtProjectNo.Anchor
            .Visible = True
            .Tag = new_pnlAddPartToOrder.Tag
            AddHandler new_txtProjectNo.Validating, AddressOf txtProjectNo_Validating
            AddHandler new_txtProjectNo.TextChanged, AddressOf txtProjectNo_TextChanged
        End With
        new_pnlAddPartToOrder.Controls.Add(new_txtProjectNo)

        Dim new_txtProjectDesc As New HighlightTextBox
        With new_txtProjectDesc
            .Name = txtProjectDesc.Name & new_pnlAddPartToOrder.Tag
            '.Text = txtProjectDesc.Text
            .Size = txtProjectDesc.Size
            .Location = txtProjectDesc.Location
            .Anchor = txtProjectDesc.Anchor
            .Visible = True
            .ReadOnly = txtProjectDesc.ReadOnly
            .Tag = new_pnlAddPartToOrder.Tag
        End With
        new_pnlAddPartToOrder.Controls.Add(new_txtProjectDesc)

        Dim new_cmdOpenProject As New Button
        With new_cmdOpenProject
            .Name = cmdOpenProject.Name & new_pnlAddPartToOrder.Tag
            .Text = cmdOpenProject.Text
            .Size = cmdOpenProject.Size
            .Location = cmdOpenProject.Location
            .Anchor = cmdOpenProject.Anchor
            .Visible = True
            .Tag = new_pnlAddPartToOrder.Tag
            .Image = cmdOpenProject.Image
            .TextImageRelation = cmdOpenProject.TextImageRelation
            AddHandler new_cmdOpenProject.Click, AddressOf cmdOpenProject_Click
        End With
        new_pnlAddPartToOrder.Controls.Add(new_cmdOpenProject)

        Dim new_lblNote As New Label
        With new_lblNote
            .Name = lblNote.Name & new_pnlAddPartToOrder.Tag
            .Text = lblNote.Text
            .Size = lblNote.Size
            .Location = lblNote.Location
            .Anchor = lblNote.Anchor
            .Visible = True
            .Tag = new_pnlAddPartToOrder.Tag
        End With
        new_pnlAddPartToOrder.Controls.Add(new_lblNote)

        Dim new_txtNote As New HighlightTextBox
        With new_txtNote
            .Name = txtNote.Name & new_pnlAddPartToOrder.Tag
            .Text = txtNote.Text
            .Size = txtNote.Size
            .Location = txtNote.Location
            .Anchor = txtNote.Anchor
            .Visible = True
            .Tag = new_pnlAddPartToOrder.Tag
        End With
        new_pnlAddPartToOrder.Controls.Add(new_txtNote)

        Dim new_chkForToday As New CheckBox
        With new_chkForToday
            .Name = chkForToday.Name & new_pnlAddPartToOrder.Tag
            .Text = chkForToday.Text
            .Size = chkForToday.Size
            .Location = chkForToday.Location
            .Anchor = chkForToday.Anchor
            .Visible = True
            .Tag = new_pnlAddPartToOrder.Tag
        End With
        new_pnlAddPartToOrder.Controls.Add(new_chkForToday)


        Dim new_lblCloseAddPartToOrder As New Label
        With new_lblCloseAddPartToOrder
            .Text = lblCloseAddPartToOrder.Text
            .Size = lblCloseAddPartToOrder.Size
            .Location = lblCloseAddPartToOrder.Location
            .Anchor = lblCloseAddPartToOrder.Anchor
            .Visible = True
            .BorderStyle = lblCloseAddPartToOrder.BorderStyle
            .TextAlign = lblCloseAddPartToOrder.TextAlign
            .Font = lblCloseAddPartToOrder.Font
            .Tag = new_pnlAddPartToOrder.Tag
            AddHandler new_lblCloseAddPartToOrder.Click, AddressOf lblCloseAddPartToOrder_Click
        End With
        new_pnlAddPartToOrder.Controls.Add(new_lblCloseAddPartToOrder)


        pnlAddPartToOrder.Parent.Controls.Add(new_pnlAddPartToOrder)
        new_pnlAddPartToOrder.SendToBack()

    End Sub

    Private Sub CrewModeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CrewModeToolStripMenuItem.Click

        StartFinesseProcess("Scheduling Board.exe", "mode=Crew")

    End Sub

    Private Sub EquipmentModeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EquipmentModeToolStripMenuItem.Click

        StartFinesseProcess("Scheduling Board.exe")

    End Sub



    Private Sub tsbUnavailability_Click(sender As Object, e As EventArgs) Handles tsbUnavailability.Click
        dtpUnavailabilityCutoff.Value = DateAdd(DateInterval.WeekOfYear, 3, dtpStartDate.Value)

        pnlUnvailability.Parent = gbMultiPartGroups
        pnlUnvailability.Dock = DockStyle.Fill
        pnlUnvailability.BringToFront()

        EnableDoubleBuffering(dgvUnavailability)

        If pnlUnvailability.Visible = True Then
            pnlUnvailability.Visible = False
            scParts.SplitterDistance = Get_AppConfigSetting(AppConfig, "scPartsSplitterDistance")
        Else
            scParts.SplitterDistance = scParts.Width * 0.4
            pnlUnvailability.Visible = True
            'RefreshUnavailability()
        End If
    End Sub

    Private Sub RefreshUnavailability()

        dgvUnavailability.DataSource = Nothing

        Application.DoEvents()

        Dim sWarehouses = GetStrIncludedWarehouses(True, False).Replace("'", "")

        If flpUnavailabilityCompareWarehouses.Controls.Count > 0 Then
            Dim w = (From chk As CheckBox In flpUnavailabilityCompareWarehouses.Controls.OfType(Of CheckBox)
                     Where chk.Checked = True
                     Select chk.Text).ToArray

            sWarehouses = Strings.Join(w, ",")
        End If

        If Not String.IsNullOrEmpty(txtUnavailabilityProjectNo.Text) Then
            Dim homeWarehouse = GetOpenedFinesseConnection.ExecuteScalar("select agency from glentities where entityno = " & txtUnavailabilityProjectNo.Text.SQLQuote)

            If Not String.IsNullOrEmpty(homeWarehouse) Then
                sWarehouses = homeWarehouse & "," & Replace(sWarehouses, "," & homeWarehouse, "")
            End If

        End If

        If chkUnavailabilitySumWarehouses.Checked Then
            Dim cmd As New SqlCommand("pjunavlproc_sumwarehouses", GetOpenedFinesseConnection)
            cmd.CommandType = CommandType.StoredProcedure


            With GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)
                cmd.Parameters.Add("@warehouse", SqlDbType.VarChar, 50).Value = sWarehouses
                cmd.Parameters.Add("@start_date", SqlDbType.Date).Value = dtpStartDate.Value
                cmd.Parameters.Add("@end_date", SqlDbType.Date).Value = If(chkUnavailabilityCutoffDate.Checked, dtpUnavailabilityCutoff.Value, dtpEndDate.Value)
            End With

            Dim dt As New DataTable
            Dim da As New SqlDataAdapter(cmd)
            da.SelectCommand.CommandTimeout = 120

            Me.Cursor = Cursors.AppStarting

            Dim t As New System.Threading.Tasks.Task(Sub()
                                                         da.Fill(dt)

                                                         If Not chkShowRemovedParts.Checked Then
                                                             For Each dr As DataRow In dt.Rows
                                                                 If dr.RowState <> DataRowState.Deleted AndAlso lUnavailabilityDeletedParts.Contains(dr.Item("partno")) Then dr.Delete()
                                                             Next
                                                         End If

                                                         dt.AcceptChanges()

                                                         Me.Invoke(Sub()
                                                                       dgvUnavailability.DataSource = New DataView(dt, Nothing, "Category,BackfillValue desc", DataRowState.Unchanged)

                                                                       With dgvUnavailability
                                                                           .Columns("base_inventory").Visible = False

                                                                           .Columns("partno").HeaderText = "Part #"
                                                                           .Columns("partdesc").HeaderText = "Part Desc"
                                                                           .Columns("partdesc").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                                           .Columns("partdesc").Width = 300

                                                                           .Columns("bottleneck").HeaderText = "Btlnk"

                                                                           .Columns("RentalValue").DefaultCellStyle.Format = "N2"
                                                                           .Columns("BackfillValue").DefaultCellStyle.Format = "N2"
                                                                           .Columns("bottleneckweight").DefaultCellStyle.Format = "N2"

                                                                           .Columns("bottleneckweight").HeaderText = "Weight"
                                                                           .Columns("bottleneckweight").DisplayIndex = .Columns.Count - 1

                                                                       End With

                                                                       dgvUnavailability.ContextMenuStrip = Nothing

                                                                       Me.Cursor = Cursors.Default

                                                                   End Sub)
                                                     End Sub)
            t.Start()

        Else
            Dim cmd As New SqlCommand("pjunavlproc_consolidated", GetOpenedFinesseConnection)
            cmd.CommandType = CommandType.StoredProcedure


            With GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row)
                cmd.Parameters.Add("@entityno", SqlDbType.VarChar, 50).Value = txtUnavailabilityProjectNo.Text & "%"
                cmd.Parameters.Add("@warehouse", SqlDbType.VarChar, 50).Value = sWarehouses.Split(",")(0)
                cmd.Parameters.Add("@category", SqlDbType.VarChar, 30).Value = ""
                cmd.Parameters.Add("@storagelocation", SqlDbType.VarChar, 50).Value = ""
                cmd.Parameters.Add("@start_date", SqlDbType.Date).Value = dtpStartDate.Value
                cmd.Parameters.Add("@end_date", SqlDbType.Date).Value = If(chkUnavailabilityCutoffDate.Checked, dtpUnavailabilityCutoff.Value, dtpEndDate.Value)
                cmd.Parameters.Add("@exclude_timeline", SqlDbType.Bit).Value = 1
                If sWarehouses.Split(",").Count > 1 Then cmd.Parameters.Add("@compare_warehouse", SqlDbType.VarChar, 50).Value = sWarehouses.Split(",")(1)
                If sWarehouses.Split(",").Count > 2 Then cmd.Parameters.Add("@compare_warehouse2", SqlDbType.VarChar, 50).Value = sWarehouses.Split(",")(2)
                If sWarehouses.Split(",").Count > 3 Then cmd.Parameters.Add("@compare_warehouse3", SqlDbType.VarChar, 50).Value = sWarehouses.Split(",")(3)
                If sWarehouses.Split(",").Count > 4 Then cmd.Parameters.Add("@compare_warehouse4", SqlDbType.VarChar, 50).Value = sWarehouses.Split(",")(4)
                If sWarehouses.Split(",").Count > 5 Then cmd.Parameters.Add("@compare_warehouse5", SqlDbType.VarChar, 50).Value = sWarehouses.Split(",")(5)
                If sWarehouses.Split(",").Count > 6 Then cmd.Parameters.Add("@compare_warehouse6", SqlDbType.VarChar, 50).Value = sWarehouses.Split(",")(6)
                If sWarehouses.Split(",").Count > 8 Then cmd.Parameters.Add("@compare_warehouse7", SqlDbType.VarChar, 50).Value = sWarehouses.Split(",")(7)
                If sWarehouses.Split(",").Count > 9 Then cmd.Parameters.Add("@compare_warehouse8", SqlDbType.VarChar, 50).Value = sWarehouses.Split(",")(8)
                If sWarehouses.Split(",").Count > 10 Then cmd.Parameters.Add("@compare_warehouse9", SqlDbType.VarChar, 50).Value = sWarehouses.Split(",")(9)
            End With

            Dim dt As New DataTable
            Dim da As New SqlDataAdapter(cmd)
            da.SelectCommand.CommandTimeout = 120

            Me.Cursor = Cursors.AppStarting

            Dim t As New System.Threading.Tasks.Task(Sub()
                                                         da.Fill(dt)

                                                         Dim sumCol = dt.Columns.Add("Sum", GetType(Integer))
                                                         sumCol.Expression = "bottleneck" & If(sWarehouses.Split(",").Count > 1, " + compare_bottleneck", String.Empty)
                                                         For i = 2 To sWarehouses.Split(",").Count - 2
                                                             sumCol.Expression &= " + compare_bottleneck" & i
                                                         Next

                                                         If chkUnavailabilityHideTurnaroundDays.Checked Then
                                                             For Each dr As DataRow In dt.Select("isTurnaround=1")
                                                                 If dr.RowState <> DataRowState.Deleted Then dr.Delete()
                                                             Next
                                                         End If

                                                         If chkUnvailabilityHideWhenSumBottleneckGreaterThansZero.Checked Then
                                                             For Each dr As DataRow In dt.Select("Sum>=0")
                                                                 If dr.RowState <> DataRowState.Deleted Then dr.Delete()
                                                             Next
                                                         End If

                                                         If Not chkShowRemovedParts.Checked Then
                                                             For Each dr As DataRow In dt.Rows
                                                                 If dr.RowState <> DataRowState.Deleted AndAlso lUnavailabilityDeletedParts.Contains(dr.Item("partno")) Then dr.Delete()
                                                             Next
                                                         End If

                                                         dt.AcceptChanges()


                                                         Me.Invoke(Sub()
                                                                       dgvUnavailability.DataSource = New DataView(dt, Nothing, "Category,partdesc", DataRowState.Unchanged)

                                                                       With dgvUnavailability
                                                                           .Columns("start_new").Visible = False

                                                                           .Columns("Date").Visible = False
                                                                           .Columns("base_inventory").Visible = False
                                                                           .Columns("difference").Visible = False
                                                                           .Columns("bottleneck_date").Visible = False
                                                                           .Columns("min_date").Visible = False
                                                                           .Columns("max_date").Visible = False
                                                                           .Columns("isTurnaround").Visible = False

                                                                           .Columns("partno").Visible = False
                                                                           .Columns("partno").HeaderText = "Part #"
                                                                           .Columns("partdesc").HeaderText = "Part Desc"
                                                                           .Columns("partdesc").AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                                           .Columns("partdesc").Width = 300
                                                                           .Columns("est_qty").HeaderText = "Qty"
                                                                           .Columns("bottleneck").HeaderText = sWarehouses.Split(",")(0)
                                                                           .Columns("first_shortage").HeaderText = "Date"

                                                                           .Columns("bottleneckweight").HeaderText = "Weight"
                                                                           .Columns("bottleneckweight").DisplayIndex = .Columns.Count - 1

                                                                           If sWarehouses.Split(",").Count > 1 Then .Columns("compare_bottleneck").HeaderText = sWarehouses.Split(",")(1)
                                                                           If sWarehouses.Split(",").Count > 2 Then .Columns("compare_bottleneck2").HeaderText = sWarehouses.Split(",")(2)
                                                                           If sWarehouses.Split(",").Count > 3 Then .Columns("compare_bottleneck3").HeaderText = sWarehouses.Split(",")(3)
                                                                           If sWarehouses.Split(",").Count > 4 Then .Columns("compare_bottleneck4").HeaderText = sWarehouses.Split(",")(4)
                                                                           If sWarehouses.Split(",").Count > 5 Then .Columns("compare_bottleneck5").HeaderText = sWarehouses.Split(",")(5)
                                                                           If sWarehouses.Split(",").Count > 6 Then .Columns("compare_bottleneck6").HeaderText = sWarehouses.Split(",")(6)
                                                                           If sWarehouses.Split(",").Count > 8 Then .Columns("compare_bottleneck7").HeaderText = sWarehouses.Split(",")(7)
                                                                           If sWarehouses.Split(",").Count > 9 Then .Columns("compare_bottleneck8").HeaderText = sWarehouses.Split(",")(8)
                                                                           If sWarehouses.Split(",").Count > 10 Then .Columns("compare_bottleneck9").HeaderText = sWarehouses.Split(",")(9)

                                                                       End With

                                                                       Dim cmu As New ContextMenuStrip

                                                                       For Each w In sWarehouses.Split(",")
                                                                           cmu.Items.Add("Copy Available Parts/Qtys from: " & w, Nothing, Sub()


                                                                                                                                              Dim sb As New StringBuilder

                                                                                                                                              Dim foundField = (From c As DataGridViewColumn In dgvUnavailability.Columns
                                                                                                                                                                Where c.HeaderText = w
                                                                                                                                                                Select c.DataPropertyName).FirstOrDefault

                                                                                                                                              For Each dgvr As DataGridViewRow In dgvUnavailability.Rows
                                                                                                                                                  Dim qtyToTransfer = If(-dgvr.DataBoundItem("bottleneck") < dgvr.DataBoundItem(foundField), -dgvr.DataBoundItem("bottleneck"), dgvr.DataBoundItem(foundField))

                                                                                                                                                  If qtyToTransfer > 0 Then
                                                                                                                                                      sb.AppendLine(dgvr.DataBoundItem("partno") & vbTab & qtyToTransfer)
                                                                                                                                                  End If
                                                                                                                                              Next

                                                                                                                                              Clipboard.SetText(sb.ToString)

                                                                                                                                              MsgBox("Parts/Qtys available from " & w & " have been copied to the clipboard, and can be inserted into any Transfer Order via right-click > 'Insert Copied Parts'")

                                                                                                                                          End Sub)
                                                                       Next
                                                                       dgvUnavailability.ContextMenuStrip = cmu

                                                                       Me.Cursor = Cursors.Default

                                                                   End Sub)
                                                     End Sub)
            t.Start()
        End If


    End Sub

    Private Sub dgvUnavailability_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles dgvUnavailability.CellFormatting
        If dgvUnavailability.Columns(e.ColumnIndex).DataPropertyName = "partno" Then Return
        If dgvUnavailability.Columns(e.ColumnIndex).DataPropertyName = "est_qty" Then Return
        If dgvUnavailability.Columns(e.ColumnIndex).DataPropertyName = "RentalValue" Then Return
        If dgvUnavailability.Columns(e.ColumnIndex).DataPropertyName = "BackfillValue" Then Return
        If dgvUnavailability.Columns(e.ColumnIndex).DataPropertyName = "bottleneckweight" Then Return

        If IsNumeric(e.Value) AndAlso e.Value > 0 Then e.CellStyle.BackColor = Color.PaleGoldenrod

        If dgvUnavailability.Columns(e.ColumnIndex).DataPropertyName = "Sum" Then
            If e.Value > 0 Then e.CellStyle.BackColor = Color.LightGreen
        End If

        If IsNumeric(e.Value) AndAlso e.Value < 0 Then e.CellStyle.ForeColor = Color.Red

    End Sub

    Private Sub cmdUnavailabilityProjectNumber_Click(sender As Object, e As EventArgs) Handles cmdUnavailabilityProjectNumber.Click

        Dim picker As New ProjectPicker(Me, cmdUnavailabilityProjectNumber)

        If picker.GetProject() = True Then
            txtUnavailabilityProjectNo.Text = picker.ProjectNum
            txtUnavailabilityProjectDesc.Text = picker.ProjectNumDesc
        End If

    End Sub

    Private Sub txtUnavailabilityProjectNo_Validating(sender As Object, e As CancelEventArgs) Handles txtUnavailabilityProjectNo.Validating

        If txtUnavailabilityProjectNo.Text = String.Empty Then
            txtUnavailabilityProjectDesc.Text = "All Projects"
        Else
            Dim ProjectDesc = GetProjectDescription(txtUnavailabilityProjectNo.Text, GetOpenedFinesseConnection)
            If Not String.IsNullOrEmpty(ProjectDesc) Then
                txtUnavailabilityProjectDesc.Text = ProjectDesc
            Else
                txtUnavailabilityProjectDesc.Text = "Invalid Project"
            End If
        End If

    End Sub

    Private Sub cmdUnavailabilitySetCompareWarehouses_Click(sender As Object, e As EventArgs) Handles cmdUnavailabilitySetCompareWarehouses.Click

        flpUnavailabilityCompareWarehouses.Controls.Clear()

        For Each w In GetStrIncludedWarehouses(True, False).Replace("'", "").Split(",")
            Dim chkWH As New CheckBox
            With chkWH
                .Appearance = Appearance.Button
                .Width = 45
                .Height = 20
                .Text = w
                .Checked = True
                .Parent = flpUnavailabilityCompareWarehouses
                .Visible = True
            End With
            AddHandler chkWH.Click, Sub()
                                        If chkWH.Checked = False Then
                                            flpUnavailabilityCompareWarehouses.Controls.RemoveByKey(chkWH.Name)
                                            flpUnavailabilityCompareWarehouses.Controls.Add(chkWH)
                                        End If
                                    End Sub
        Next

    End Sub

    Private Sub cmdUnavailabilityRefresh_Click(sender As Object, e As EventArgs) Handles cmdUnavailabilityRefresh.Click
        RefreshUnavailability()
    End Sub

    Private Sub dgvUnavailability_MouseUp(sender As Object, e As MouseEventArgs) Handles dgvUnavailability.MouseUp
        If dgvUnavailability.SelectedRows.Count = 0 Then Return

        If e.Button = MouseButtons.Right AndAlso dgvUnavailability.ContextMenuStrip IsNot Nothing Then
            dgvUnavailability.ContextMenuStrip.Show()
        End If

        If e.Button <> MouseButtons.Left Then Return

        Dim s As String() = (From dgvr As DataGridViewRow In dgvUnavailability.SelectedRows
                             Select CStr(dgvr.DataBoundItem("partno"))).ToArray

        Static lastPartno As String()

        Dim same = True

        If lastPartno IsNot Nothing Then

            If s.Length <> lastPartno.Length Then
                same = False
            Else
                Dim a = lastPartno.OrderBy(Function(x) x).ToArray
                Dim b = s.OrderBy(Function(x) x).ToArray

                ' check each element
                For index As Integer = 0 To a.Length - 1
                    If (a.GetValue(index) <> b.GetValue(index)) Then
                        same = False
                    End If
                Next
            End If

            If same Then Return
        End If

        lastPartno = s

        RefreshPartData(s(0))

        RefreshCurrentPartAttributes(s(0))

        RefreshMultiPartListByPartList(String.Join(",", s))

        RefreshAvailabilityDataWhenIdle()
    End Sub

    Private Sub dgvUnavailability_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles dgvUnavailability.UserDeletingRow
        lUnavailabilityDeletedParts.Add(e.Row.Cells("partno").Value)
    End Sub

    Private Sub chkUnavailabilitySumWarehouses_CheckedChanged(sender As Object, e As EventArgs) Handles chkUnavailabilitySumWarehouses.CheckedChanged
        chkUnavailabilityHideTurnaroundDays.Visible = If(chkUnavailabilitySumWarehouses.Checked, False, True)
        chkUnvailabilityHideWhenSumBottleneckGreaterThansZero.Visible = If(chkUnavailabilitySumWarehouses.Checked, False, True)
        cmdUnavailabilityProjectNumber.Visible = If(chkUnavailabilitySumWarehouses.Checked, False, True)
        txtUnavailabilityProjectNo.Visible = If(chkUnavailabilitySumWarehouses.Checked, False, True)
        txtUnavailabilityProjectDesc.Visible = If(chkUnavailabilitySumWarehouses.Checked, False, True)
    End Sub


    Private Sub CreateJustInTimeTransfer()
        Dim result = MsgBox("Create a Just-In-Time transfer from " & dgvDetail.GetSelectedDataRows(Of DataRow).First()("bld") & " to " & GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row).warehouse & "?", vbYesNoCancel)
        If result = MsgBoxResult.Yes Then

            Dim cmd As New SqlCommand("Create_JustInTime_Transfer_Order", GetOpenedFinesseConnection)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@fromWarehouse", SqlDbType.VarChar, 10).Value = dgvDetail.GetSelectedDataRows(Of DataRow).First()("bld")
            cmd.Parameters.Add("@toWarehouse", SqlDbType.VarChar, 10).Value = GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row).warehouse
            cmd.Parameters.Add("@deadlineDate", SqlDbType.Date).Value = GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row).ProjStartDate
            cmd.Parameters.Add("@partno", SqlDbType.VarChar, 50).Value = GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row).partno
            cmd.Parameters.Add("@qty", SqlDbType.Int).Value = 1
            cmd.Parameters.Add("@note", SqlDbType.VarChar, 250).Value = myUserName.Substring(0, 2).ToUpper & " - " & GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row).entitydesc

            Dim ds As New DataSet
            Dim da As New SqlDataAdapter(cmd)
            da.SelectCommand.CommandTimeout = 120
            da.Fill(ds)

            Dim tAvgContainerDays = ds.Tables(0)
            Dim tTransferDate = ds.Tables(1)

            If tAvgContainerDays.Rows.Count > 0 AndAlso Not IsDBNull(tAvgContainerDays.Rows(0).Item("avgContainerDays")) AndAlso DateAdd(DateInterval.Day, -(tAvgContainerDays.Rows(0).Item("avgContainerDays") + 7), GetSchedulingBoardCellInfo(fgSchedulingBoard.Col, fgSchedulingBoard.Row).ProjStartDate) >= Today Then MsgBox("Just-In-Time Transfer Order created.  Note: This far in advance there is time to book a Sea Container...", vbOKOnly)

            'Dim foo = ds

            RefreshPartScheduleDetails(CurrentFlexGrid, Nothing)

            RefreshAvailabilityData()

            Return
        End If
    End Sub

    Private Sub CreateJustInTimeTransferToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CreateJustInTimeTransferToolStripMenuItem.Click
        CreateJustInTimeTransfer()
    End Sub

#End Region


End Class
