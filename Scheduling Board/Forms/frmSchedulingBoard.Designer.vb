<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSchedulingBoard
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSchedulingBoard))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.pnlQueryCriteria = New System.Windows.Forms.Panel()
        Me.pnlMain = New System.Windows.Forms.Panel()
        Me.scPartsAndDetail = New System.Windows.Forms.SplitContainer()
        Me.scParts = New System.Windows.Forms.SplitContainer()
        Me.pnlMultiPartGroups = New System.Windows.Forms.Panel()
        Me.gbMultiPartGroups = New System.Windows.Forms.GroupBox()
        Me.tsMultiPartGroups = New System.Windows.Forms.ToolStrip()
        Me.lblCollapseMultiPartGroups = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ResetMyMultipartGroupsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbUnavailability = New System.Windows.Forms.ToolStripButton()
        Me.btnMultiPartGroup_Copy = New System.Windows.Forms.Button()
        Me.lblGroup = New System.Windows.Forms.Label()
        Me.cmdShare = New System.Windows.Forms.Button()
        Me.btnFindSelectedPartsGroup = New System.Windows.Forms.Button()
        Me.cboMultiPartGroups = New System.Windows.Forms.ComboBox()
        Me.btnMultiPartGroup_Add = New System.Windows.Forms.Button()
        Me.btnMultiPartList_MoveUp = New System.Windows.Forms.Button()
        Me.btnMultiPartList_MoveDown = New System.Windows.Forms.Button()
        Me.btnMultiPartList_Delete = New System.Windows.Forms.Button()
        Me.btnMultiPartList_Add = New System.Windows.Forms.Button()
        Me.btnMultiPartGroup_Rename = New System.Windows.Forms.Button()
        Me.btnMultiPartGroup_Delete = New System.Windows.Forms.Button()
        Me.lstMultiPartList = New System.Windows.Forms.CheckedListBox()
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.tpSchedulingBoard = New System.Windows.Forms.TabPage()
        Me.pnlUnvailability = New System.Windows.Forms.Panel()
        Me.dgvUnavailability = New System.Windows.Forms.DataGridView()
        Me.pnlUnavailabilityControls = New System.Windows.Forms.Panel()
        Me.chkUnavailabilitySumWarehouses = New System.Windows.Forms.CheckBox()
        Me.chkShowRemovedParts = New System.Windows.Forms.CheckBox()
        Me.chkUnvailabilityHideWhenSumBottleneckGreaterThansZero = New System.Windows.Forms.CheckBox()
        Me.chkUnavailabilityHideTurnaroundDays = New System.Windows.Forms.CheckBox()
        Me.cmdUnavailabilityRefresh = New System.Windows.Forms.Button()
        Me.cmdUnavailabilitySetCompareWarehouses = New System.Windows.Forms.Button()
        Me.flpUnavailabilityCompareWarehouses = New System.Windows.Forms.FlowLayoutPanel()
        Me.lblUnavailabilityCompareWarehouses = New System.Windows.Forms.Label()
        Me.txtUnavailabilityProjectDesc = New System.Windows.Forms.TextBox()
        Me.cmdUnavailabilityProjectNumber = New System.Windows.Forms.Button()
        Me.fgSchedulingBoard = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.mnuFlexgrid = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.SubstitutePartToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ChangeOrderedQtyToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.mnuSubstitutePartSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.ClearHighlightingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.ProjectMaintenanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TransferToolsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AvailabilityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PartMaintenanceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.CopyPartBarcodeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyPartNumberToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyPartDescriptionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyPartNumberAndDescriptionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyPhaseNumberToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyPhaseDescriptionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyPhaseNumberAndDescriptionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlSchedulingBoardControls = New System.Windows.Forms.Panel()
        Me.pbEmailTeam = New System.Windows.Forms.PictureBox()
        Me.LblProjectName = New System.Windows.Forms.Label()
        Me.chkIncludeProposals = New System.Windows.Forms.CheckBox()
        Me.chkShowDetail = New System.Windows.Forms.CheckBox()
        Me.chkShowFuture = New System.Windows.Forms.CheckBox()
        Me.tpTimeline = New System.Windows.Forms.TabPage()
        Me.fgTimeline = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.tpCalendar = New System.Windows.Forms.TabPage()
        Me.fgCalendar = New AxMSFlexGridLib.AxMSFlexGrid()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.sslblLength = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblLengthValue = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblWidth = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblWidthValue = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblHeight = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblHeightValue = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblWeight = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblWeightValue = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblRentalValue = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblRentalValueValue = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblWeeklyRate = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblWeeklyRateValue = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslbl2DayWeekRate = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslbl2DayWeekRateValue = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblDailyRate = New System.Windows.Forms.ToolStripStatusLabel()
        Me.sslblDailyRateValue = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsMainVert = New System.Windows.Forms.ToolStrip()
        Me.tsbProjectMaintenance = New System.Windows.Forms.ToolStripButton()
        Me.tsbTransferTools = New System.Windows.Forms.ToolStripButton()
        Me.QueryToolsToolStripDropDownButton = New System.Windows.Forms.ToolStripDropDownButton()
        Me.EquipmentLocationsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TransactionsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.PhaseHistoryToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator14 = New System.Windows.Forms.ToolStripSeparator()
        Me.GearListToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ManifestSummaryToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ManifestToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.CarnetListToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.TicSheetToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator15 = New System.Windows.Forms.ToolStripSeparator()
        Me.LateReturnsToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.BatchLocationSummaryToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbBLInq = New System.Windows.Forms.ToolStripButton()
        Me.tsbAvailability = New System.Windows.Forms.ToolStripButton()
        Me.tsbCycleCountUtility = New System.Windows.Forms.ToolStripDropDownButton()
        Me.CycleCountUtilityToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MarkAsCountedToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsbUtilization = New System.Windows.Forms.ToolStripButton()
        Me.tsbPartMaintenance = New System.Windows.Forms.ToolStripButton()
        Me.tsbAddInventory = New System.Windows.Forms.ToolStripButton()
        Me.tsbRemoveInventory = New System.Windows.Forms.ToolStripButton()
        Me.tsbDeviceMaintenance = New System.Windows.Forms.ToolStripButton()
        Me.tsbPartAttachmentMaintenance = New System.Windows.Forms.ToolStripButton()
        Me.tsbCrewTools = New System.Windows.Forms.ToolStripButton()
        Me.tsbProjectStoryboard = New System.Windows.Forms.ToolStripButton()
        Me.tsbShowSearchCriteria = New System.Windows.Forms.ToolStripButton()
        Me.tsbRepairLog = New System.Windows.Forms.ToolStripButton()
        Me.tsbMode = New System.Windows.Forms.ToolStripSplitButton()
        Me.CrewModeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EquipmentModeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.dgvDetail = New System.Windows.Forms.DataGridView()
        Me.cmsPartSchedulingDetails = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.csmiPartSchedulingDetailCopyPhaseNumber = New System.Windows.Forms.ToolStripMenuItem()
        Me.csmiPartSchedulingDetailCopyPhaseDesc = New System.Windows.Forms.ToolStripMenuItem()
        Me.csmiPartSchedulingDetailCopyPhaseNumberDesc = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator7 = New System.Windows.Forms.ToolStripSeparator()
        Me.csmiPartSchedulingDetailSubstitutePart = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlDetailControls = New System.Windows.Forms.Panel()
        Me.lblPopOut = New System.Windows.Forms.Label()
        Me.chkGroupByParentProject = New System.Windows.Forms.CheckBox()
        Me.lblPartSchedulingDetail = New System.Windows.Forms.Label()
        Me.cmdDetailToExcel = New System.Windows.Forms.Button()
        Me.lblCloseDetailPane = New System.Windows.Forms.Label()
        Me.pnlTotalsAndWarehouses = New System.Windows.Forms.Panel()
        Me.scWarehouses = New System.Windows.Forms.SplitContainer()
        Me.pnlWarehouses = New System.Windows.Forms.Panel()
        Me.mnuWarehouse = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.RunRefreshToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ShowAllToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlWarehouse = New System.Windows.Forms.Panel()
        Me.lblRepairQty = New System.Windows.Forms.Label()
        Me.lblPlannedOrder = New System.Windows.Forms.Label()
        Me.lblLateQty = New System.Windows.Forms.Label()
        Me.lblWarehouseDesc = New System.Windows.Forms.Label()
        Me.chkToggleWarehouse = New System.Windows.Forms.CheckBox()
        Me.lvWarehouseGroups = New System.Windows.Forms.ListView()
        Me.lvcWarehouseGroup = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.cmsWarehouseGroups = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmiRenameWarehouseGroup = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiOverwriteWarehoseGroup = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiCreateNewWarehouseGroup = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator4 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiDeleteWarehouseGroup = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator6 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmiSaveGroups = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlTotals = New System.Windows.Forms.Panel()
        Me.lblCollapseExpandWarehouses = New System.Windows.Forms.Label()
        Me.lblInventory = New System.Windows.Forms.Label()
        Me.lblExclSubWexler = New System.Windows.Forms.Label()
        Me.lblChildren = New System.Windows.Forms.Label()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.pnlAddPartToOrder = New System.Windows.Forms.Panel()
        Me.cmdAddTransfer = New System.Windows.Forms.Button()
        Me.chkForToday = New System.Windows.Forms.CheckBox()
        Me.lblCloseAddPartToOrder = New System.Windows.Forms.Label()
        Me.lblNote = New System.Windows.Forms.Label()
        Me.cmdOpenProject = New System.Windows.Forms.Button()
        Me.cmdProjectSearch = New System.Windows.Forms.Button()
        Me.cmdAddPartToOrder = New System.Windows.Forms.Button()
        Me.pnlViewOptions = New System.Windows.Forms.Panel()
        Me.pnlSearchForPart = New System.Windows.Forms.Panel()
        Me.BtnForward = New System.Windows.Forms.Button()
        Me.cmdSearchByBarcode = New System.Windows.Forms.Button()
        Me.BtnBack = New System.Windows.Forms.Button()
        Me.cmdPartSearch = New System.Windows.Forms.Button()
        Me.pnlControls = New System.Windows.Forms.Panel()
        Me.chkLockDates = New System.Windows.Forms.CheckBox()
        Me.chkSummarize = New System.Windows.Forms.CheckBox()
        Me.lblStartDate = New System.Windows.Forms.Label()
        Me.dtpEndDate = New System.Windows.Forms.DateTimePicker()
        Me.dtpStartDate = New System.Windows.Forms.DateTimePicker()
        Me.lblEndDate = New System.Windows.Forms.Label()
        Me.pnlRefresh = New System.Windows.Forms.Panel()
        Me.btnRefresh = New System.Windows.Forms.Button()
        Me.pnlExcelLame = New System.Windows.Forms.Panel()
        Me.cmdHelp = New System.Windows.Forms.Button()
        Me.cmdExportToExcel = New System.Windows.Forms.Button()
        Me.btnLameLink = New System.Windows.Forms.Button()
        Me.timer_DoubleClickInterval = New System.Windows.Forms.Timer(Me.components)
        Me.timer_RefreshData = New System.Windows.Forms.Timer(Me.components)
        Me.bsDetail = New System.Windows.Forms.BindingSource(Me.components)
        Me.chkUnavailabilityCutoffDate = New System.Windows.Forms.CheckBox()
        Me.dtpUnavailabilityCutoff = New System.Windows.Forms.DateTimePicker()
        Me.txtUnavailabilityProjectNo = New SchedulingBoard.HighlightTextBox()
        Me.txtNote = New SchedulingBoard.HighlightTextBox()
        Me.txtProjectDesc = New SchedulingBoard.HighlightTextBox()
        Me.txtProjectNo = New SchedulingBoard.HighlightTextBox()
        Me.txtPartDesc = New SchedulingBoard.HighlightTextBox()
        Me.txtPartNo = New SchedulingBoard.HighlightTextBox()
        Me.CreateJustInTimeTransferToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlMain.SuspendLayout()
        CType(Me.scPartsAndDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scPartsAndDetail.Panel1.SuspendLayout()
        Me.scPartsAndDetail.Panel2.SuspendLayout()
        Me.scPartsAndDetail.SuspendLayout()
        CType(Me.scParts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scParts.Panel1.SuspendLayout()
        Me.scParts.Panel2.SuspendLayout()
        Me.scParts.SuspendLayout()
        Me.pnlMultiPartGroups.SuspendLayout()
        Me.gbMultiPartGroups.SuspendLayout()
        Me.tsMultiPartGroups.SuspendLayout()
        Me.tcMain.SuspendLayout()
        Me.tpSchedulingBoard.SuspendLayout()
        Me.pnlUnvailability.SuspendLayout()
        CType(Me.dgvUnavailability, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlUnavailabilityControls.SuspendLayout()
        CType(Me.fgSchedulingBoard, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.mnuFlexgrid.SuspendLayout()
        Me.pnlSchedulingBoardControls.SuspendLayout()
        CType(Me.pbEmailTeam, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpTimeline.SuspendLayout()
        CType(Me.fgTimeline, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpCalendar.SuspendLayout()
        CType(Me.fgCalendar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip1.SuspendLayout()
        Me.tsMainVert.SuspendLayout()
        CType(Me.dgvDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.cmsPartSchedulingDetails.SuspendLayout()
        Me.pnlDetailControls.SuspendLayout()
        Me.pnlTotalsAndWarehouses.SuspendLayout()
        CType(Me.scWarehouses, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.scWarehouses.Panel1.SuspendLayout()
        Me.scWarehouses.Panel2.SuspendLayout()
        Me.scWarehouses.SuspendLayout()
        Me.pnlWarehouses.SuspendLayout()
        Me.mnuWarehouse.SuspendLayout()
        Me.pnlWarehouse.SuspendLayout()
        Me.cmsWarehouseGroups.SuspendLayout()
        Me.pnlTotals.SuspendLayout()
        Me.pnlAddPartToOrder.SuspendLayout()
        Me.pnlViewOptions.SuspendLayout()
        Me.pnlSearchForPart.SuspendLayout()
        Me.pnlControls.SuspendLayout()
        Me.pnlRefresh.SuspendLayout()
        Me.pnlExcelLame.SuspendLayout()
        CType(Me.bsDetail, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pnlQueryCriteria
        '
        Me.pnlQueryCriteria.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlQueryCriteria.Location = New System.Drawing.Point(0, 0)
        Me.pnlQueryCriteria.Name = "pnlQueryCriteria"
        Me.pnlQueryCriteria.Size = New System.Drawing.Size(1303, 715)
        Me.pnlQueryCriteria.TabIndex = 13
        '
        'pnlMain
        '
        Me.pnlMain.Controls.Add(Me.scPartsAndDetail)
        Me.pnlMain.Controls.Add(Me.pnlTotalsAndWarehouses)
        Me.pnlMain.Controls.Add(Me.pnlAddPartToOrder)
        Me.pnlMain.Controls.Add(Me.pnlViewOptions)
        Me.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMain.Location = New System.Drawing.Point(0, 0)
        Me.pnlMain.Name = "pnlMain"
        Me.pnlMain.Size = New System.Drawing.Size(1303, 715)
        Me.pnlMain.TabIndex = 1
        '
        'scPartsAndDetail
        '
        Me.scPartsAndDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scPartsAndDetail.Location = New System.Drawing.Point(0, 136)
        Me.scPartsAndDetail.Name = "scPartsAndDetail"
        Me.scPartsAndDetail.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'scPartsAndDetail.Panel1
        '
        Me.scPartsAndDetail.Panel1.Controls.Add(Me.scParts)
        '
        'scPartsAndDetail.Panel2
        '
        Me.scPartsAndDetail.Panel2.Controls.Add(Me.dgvDetail)
        Me.scPartsAndDetail.Panel2.Controls.Add(Me.pnlDetailControls)
        Me.scPartsAndDetail.Size = New System.Drawing.Size(1303, 539)
        Me.scPartsAndDetail.SplitterDistance = 363
        Me.scPartsAndDetail.TabIndex = 15
        '
        'scParts
        '
        Me.scParts.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scParts.Location = New System.Drawing.Point(0, 0)
        Me.scParts.Name = "scParts"
        '
        'scParts.Panel1
        '
        Me.scParts.Panel1.Controls.Add(Me.pnlMultiPartGroups)
        '
        'scParts.Panel2
        '
        Me.scParts.Panel2.Controls.Add(Me.tcMain)
        Me.scParts.Panel2.Controls.Add(Me.StatusStrip1)
        Me.scParts.Panel2.Controls.Add(Me.tsMainVert)
        Me.scParts.Size = New System.Drawing.Size(1303, 363)
        Me.scParts.SplitterDistance = 300
        Me.scParts.TabIndex = 14
        '
        'pnlMultiPartGroups
        '
        Me.pnlMultiPartGroups.Controls.Add(Me.gbMultiPartGroups)
        Me.pnlMultiPartGroups.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlMultiPartGroups.Location = New System.Drawing.Point(0, 0)
        Me.pnlMultiPartGroups.MinimumSize = New System.Drawing.Size(230, 300)
        Me.pnlMultiPartGroups.Name = "pnlMultiPartGroups"
        Me.pnlMultiPartGroups.Size = New System.Drawing.Size(300, 363)
        Me.pnlMultiPartGroups.TabIndex = 13
        '
        'gbMultiPartGroups
        '
        Me.gbMultiPartGroups.Controls.Add(Me.tsMultiPartGroups)
        Me.gbMultiPartGroups.Controls.Add(Me.btnMultiPartGroup_Copy)
        Me.gbMultiPartGroups.Controls.Add(Me.lblGroup)
        Me.gbMultiPartGroups.Controls.Add(Me.cmdShare)
        Me.gbMultiPartGroups.Controls.Add(Me.btnFindSelectedPartsGroup)
        Me.gbMultiPartGroups.Controls.Add(Me.cboMultiPartGroups)
        Me.gbMultiPartGroups.Controls.Add(Me.btnMultiPartGroup_Add)
        Me.gbMultiPartGroups.Controls.Add(Me.btnMultiPartList_MoveUp)
        Me.gbMultiPartGroups.Controls.Add(Me.btnMultiPartList_MoveDown)
        Me.gbMultiPartGroups.Controls.Add(Me.btnMultiPartList_Delete)
        Me.gbMultiPartGroups.Controls.Add(Me.btnMultiPartList_Add)
        Me.gbMultiPartGroups.Controls.Add(Me.btnMultiPartGroup_Rename)
        Me.gbMultiPartGroups.Controls.Add(Me.btnMultiPartGroup_Delete)
        Me.gbMultiPartGroups.Controls.Add(Me.lstMultiPartList)
        Me.gbMultiPartGroups.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gbMultiPartGroups.Location = New System.Drawing.Point(0, 0)
        Me.gbMultiPartGroups.Name = "gbMultiPartGroups"
        Me.gbMultiPartGroups.Size = New System.Drawing.Size(300, 363)
        Me.gbMultiPartGroups.TabIndex = 22
        Me.gbMultiPartGroups.TabStop = False
        Me.gbMultiPartGroups.Text = "Multipart Groups"
        '
        'tsMultiPartGroups
        '
        Me.tsMultiPartGroups.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsMultiPartGroups.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.tsMultiPartGroups.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblCollapseMultiPartGroups, Me.ToolStripDropDownButton1, Me.tsbUnavailability})
        Me.tsMultiPartGroups.Location = New System.Drawing.Point(3, 16)
        Me.tsMultiPartGroups.Name = "tsMultiPartGroups"
        Me.tsMultiPartGroups.Size = New System.Drawing.Size(294, 25)
        Me.tsMultiPartGroups.TabIndex = 22
        Me.tsMultiPartGroups.Text = "ToolStrip1"
        '
        'lblCollapseMultiPartGroups
        '
        Me.lblCollapseMultiPartGroups.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.lblCollapseMultiPartGroups.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.lblCollapseMultiPartGroups.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCollapseMultiPartGroups.Image = CType(resources.GetObject("lblCollapseMultiPartGroups.Image"), System.Drawing.Image)
        Me.lblCollapseMultiPartGroups.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.lblCollapseMultiPartGroups.Name = "lblCollapseMultiPartGroups"
        Me.lblCollapseMultiPartGroups.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.lblCollapseMultiPartGroups.Size = New System.Drawing.Size(23, 22)
        Me.lblCollapseMultiPartGroups.Text = "X"
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ResetMyMultipartGroupsToolStripMenuItem})
        Me.ToolStripDropDownButton1.Image = CType(resources.GetObject("ToolStripDropDownButton1.Image"), System.Drawing.Image)
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(62, 22)
        Me.ToolStripDropDownButton1.Text = "Options"
        '
        'ResetMyMultipartGroupsToolStripMenuItem
        '
        Me.ResetMyMultipartGroupsToolStripMenuItem.Name = "ResetMyMultipartGroupsToolStripMenuItem"
        Me.ResetMyMultipartGroupsToolStripMenuItem.Size = New System.Drawing.Size(215, 22)
        Me.ResetMyMultipartGroupsToolStripMenuItem.Text = "Reset My Multipart Groups"
        '
        'tsbUnavailability
        '
        Me.tsbUnavailability.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbUnavailability.Image = CType(resources.GetObject("tsbUnavailability.Image"), System.Drawing.Image)
        Me.tsbUnavailability.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbUnavailability.Name = "tsbUnavailability"
        Me.tsbUnavailability.Size = New System.Drawing.Size(82, 22)
        Me.tsbUnavailability.Text = "Unavailability"
        '
        'btnMultiPartGroup_Copy
        '
        Me.btnMultiPartGroup_Copy.Location = New System.Drawing.Point(114, 62)
        Me.btnMultiPartGroup_Copy.Name = "btnMultiPartGroup_Copy"
        Me.btnMultiPartGroup_Copy.Size = New System.Drawing.Size(78, 23)
        Me.btnMultiPartGroup_Copy.TabIndex = 4
        Me.btnMultiPartGroup_Copy.Text = "Copy/Create"
        Me.btnMultiPartGroup_Copy.UseVisualStyleBackColor = True
        '
        'lblGroup
        '
        Me.lblGroup.AutoSize = True
        Me.lblGroup.Location = New System.Drawing.Point(13, 94)
        Me.lblGroup.Name = "lblGroup"
        Me.lblGroup.Size = New System.Drawing.Size(39, 13)
        Me.lblGroup.TabIndex = 17
        Me.lblGroup.Text = "Group:"
        Me.lblGroup.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'cmdShare
        '
        Me.cmdShare.Location = New System.Drawing.Point(195, 62)
        Me.cmdShare.Name = "cmdShare"
        Me.cmdShare.Size = New System.Drawing.Size(48, 23)
        Me.cmdShare.TabIndex = 19
        Me.cmdShare.Text = "Share"
        Me.cmdShare.UseVisualStyleBackColor = True
        '
        'btnFindSelectedPartsGroup
        '
        Me.btnFindSelectedPartsGroup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnFindSelectedPartsGroup.Location = New System.Drawing.Point(3, 118)
        Me.btnFindSelectedPartsGroup.Name = "btnFindSelectedPartsGroup"
        Me.btnFindSelectedPartsGroup.Size = New System.Drawing.Size(297, 23)
        Me.btnFindSelectedPartsGroup.TabIndex = 16
        Me.btnFindSelectedPartsGroup.Text = "Find Selected Part's Group"
        Me.btnFindSelectedPartsGroup.UseVisualStyleBackColor = True
        '
        'cboMultiPartGroups
        '
        Me.cboMultiPartGroups.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cboMultiPartGroups.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cboMultiPartGroups.FormattingEnabled = True
        Me.cboMultiPartGroups.Location = New System.Drawing.Point(58, 91)
        Me.cboMultiPartGroups.Name = "cboMultiPartGroups"
        Me.cboMultiPartGroups.Size = New System.Drawing.Size(242, 21)
        Me.cboMultiPartGroups.TabIndex = 0
        '
        'btnMultiPartGroup_Add
        '
        Me.btnMultiPartGroup_Add.Image = Global.SchedulingBoard.My.Resources.Resources.Add_New_Row
        Me.btnMultiPartGroup_Add.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMultiPartGroup_Add.Location = New System.Drawing.Point(3, 62)
        Me.btnMultiPartGroup_Add.Name = "btnMultiPartGroup_Add"
        Me.btnMultiPartGroup_Add.Size = New System.Drawing.Size(49, 23)
        Me.btnMultiPartGroup_Add.TabIndex = 1
        Me.btnMultiPartGroup_Add.Text = "Add"
        Me.btnMultiPartGroup_Add.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMultiPartGroup_Add.UseVisualStyleBackColor = True
        '
        'btnMultiPartList_MoveUp
        '
        Me.btnMultiPartList_MoveUp.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnMultiPartList_MoveUp.Image = Global.SchedulingBoard.My.Resources.Resources.arrow_up_blue
        Me.btnMultiPartList_MoveUp.Location = New System.Drawing.Point(125, 263)
        Me.btnMultiPartList_MoveUp.Name = "btnMultiPartList_MoveUp"
        Me.btnMultiPartList_MoveUp.Size = New System.Drawing.Size(37, 36)
        Me.btnMultiPartList_MoveUp.TabIndex = 8
        Me.btnMultiPartList_MoveUp.UseVisualStyleBackColor = True
        '
        'btnMultiPartList_MoveDown
        '
        Me.btnMultiPartList_MoveDown.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnMultiPartList_MoveDown.Image = Global.SchedulingBoard.My.Resources.Resources.arrow_down_blue
        Me.btnMultiPartList_MoveDown.Location = New System.Drawing.Point(125, 302)
        Me.btnMultiPartList_MoveDown.Name = "btnMultiPartList_MoveDown"
        Me.btnMultiPartList_MoveDown.Size = New System.Drawing.Size(38, 33)
        Me.btnMultiPartList_MoveDown.TabIndex = 9
        Me.btnMultiPartList_MoveDown.UseVisualStyleBackColor = True
        '
        'btnMultiPartList_Delete
        '
        Me.btnMultiPartList_Delete.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnMultiPartList_Delete.Image = Global.SchedulingBoard.My.Resources.Resources.Deleting
        Me.btnMultiPartList_Delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMultiPartList_Delete.Location = New System.Drawing.Point(168, 288)
        Me.btnMultiPartList_Delete.Name = "btnMultiPartList_Delete"
        Me.btnMultiPartList_Delete.Size = New System.Drawing.Size(84, 23)
        Me.btnMultiPartList_Delete.TabIndex = 7
        Me.btnMultiPartList_Delete.Text = "Delete Part"
        Me.btnMultiPartList_Delete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMultiPartList_Delete.UseVisualStyleBackColor = True
        '
        'btnMultiPartList_Add
        '
        Me.btnMultiPartList_Add.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.btnMultiPartList_Add.Image = Global.SchedulingBoard.My.Resources.Resources.Add_New_Row
        Me.btnMultiPartList_Add.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMultiPartList_Add.Location = New System.Drawing.Point(49, 288)
        Me.btnMultiPartList_Add.Name = "btnMultiPartList_Add"
        Me.btnMultiPartList_Add.Size = New System.Drawing.Size(70, 23)
        Me.btnMultiPartList_Add.TabIndex = 6
        Me.btnMultiPartList_Add.Text = "Add Part"
        Me.btnMultiPartList_Add.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMultiPartList_Add.UseVisualStyleBackColor = True
        '
        'btnMultiPartGroup_Rename
        '
        Me.btnMultiPartGroup_Rename.Location = New System.Drawing.Point(55, 62)
        Me.btnMultiPartGroup_Rename.Name = "btnMultiPartGroup_Rename"
        Me.btnMultiPartGroup_Rename.Size = New System.Drawing.Size(56, 23)
        Me.btnMultiPartGroup_Rename.TabIndex = 3
        Me.btnMultiPartGroup_Rename.Text = "Rename"
        Me.btnMultiPartGroup_Rename.UseVisualStyleBackColor = True
        '
        'btnMultiPartGroup_Delete
        '
        Me.btnMultiPartGroup_Delete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnMultiPartGroup_Delete.Image = Global.SchedulingBoard.My.Resources.Resources.Deleting
        Me.btnMultiPartGroup_Delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnMultiPartGroup_Delete.Location = New System.Drawing.Point(238, 62)
        Me.btnMultiPartGroup_Delete.Name = "btnMultiPartGroup_Delete"
        Me.btnMultiPartGroup_Delete.Size = New System.Drawing.Size(62, 23)
        Me.btnMultiPartGroup_Delete.TabIndex = 2
        Me.btnMultiPartGroup_Delete.Text = "Delete"
        Me.btnMultiPartGroup_Delete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnMultiPartGroup_Delete.UseVisualStyleBackColor = True
        '
        'lstMultiPartList
        '
        Me.lstMultiPartList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstMultiPartList.FormattingEnabled = True
        Me.lstMultiPartList.Location = New System.Drawing.Point(3, 148)
        Me.lstMultiPartList.Name = "lstMultiPartList"
        Me.lstMultiPartList.Size = New System.Drawing.Size(297, 94)
        Me.lstMultiPartList.TabIndex = 5
        '
        'tcMain
        '
        Me.tcMain.Controls.Add(Me.tpSchedulingBoard)
        Me.tcMain.Controls.Add(Me.tpTimeline)
        Me.tcMain.Controls.Add(Me.tpCalendar)
        Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMain.Location = New System.Drawing.Point(38, 0)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(961, 341)
        Me.tcMain.TabIndex = 11
        '
        'tpSchedulingBoard
        '
        Me.tpSchedulingBoard.Controls.Add(Me.pnlUnvailability)
        Me.tpSchedulingBoard.Controls.Add(Me.fgSchedulingBoard)
        Me.tpSchedulingBoard.Controls.Add(Me.pnlSchedulingBoardControls)
        Me.tpSchedulingBoard.Location = New System.Drawing.Point(4, 22)
        Me.tpSchedulingBoard.Name = "tpSchedulingBoard"
        Me.tpSchedulingBoard.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSchedulingBoard.Size = New System.Drawing.Size(953, 315)
        Me.tpSchedulingBoard.TabIndex = 1
        Me.tpSchedulingBoard.Text = "Scheduling Board"
        Me.tpSchedulingBoard.UseVisualStyleBackColor = True
        '
        'pnlUnvailability
        '
        Me.pnlUnvailability.Controls.Add(Me.dgvUnavailability)
        Me.pnlUnvailability.Controls.Add(Me.pnlUnavailabilityControls)
        Me.pnlUnvailability.Location = New System.Drawing.Point(76, 84)
        Me.pnlUnvailability.Name = "pnlUnvailability"
        Me.pnlUnvailability.Size = New System.Drawing.Size(588, 182)
        Me.pnlUnvailability.TabIndex = 11
        Me.pnlUnvailability.Visible = False
        '
        'dgvUnavailability
        '
        Me.dgvUnavailability.AllowUserToAddRows = False
        Me.dgvUnavailability.AllowUserToResizeRows = False
        Me.dgvUnavailability.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvUnavailability.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvUnavailability.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvUnavailability.Location = New System.Drawing.Point(0, 81)
        Me.dgvUnavailability.Name = "dgvUnavailability"
        Me.dgvUnavailability.ReadOnly = True
        Me.dgvUnavailability.RowHeadersVisible = False
        Me.dgvUnavailability.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvUnavailability.Size = New System.Drawing.Size(588, 101)
        Me.dgvUnavailability.TabIndex = 0
        '
        'pnlUnavailabilityControls
        '
        Me.pnlUnavailabilityControls.Controls.Add(Me.dtpUnavailabilityCutoff)
        Me.pnlUnavailabilityControls.Controls.Add(Me.chkUnavailabilityCutoffDate)
        Me.pnlUnavailabilityControls.Controls.Add(Me.chkUnavailabilitySumWarehouses)
        Me.pnlUnavailabilityControls.Controls.Add(Me.chkShowRemovedParts)
        Me.pnlUnavailabilityControls.Controls.Add(Me.chkUnvailabilityHideWhenSumBottleneckGreaterThansZero)
        Me.pnlUnavailabilityControls.Controls.Add(Me.chkUnavailabilityHideTurnaroundDays)
        Me.pnlUnavailabilityControls.Controls.Add(Me.cmdUnavailabilityRefresh)
        Me.pnlUnavailabilityControls.Controls.Add(Me.cmdUnavailabilitySetCompareWarehouses)
        Me.pnlUnavailabilityControls.Controls.Add(Me.flpUnavailabilityCompareWarehouses)
        Me.pnlUnavailabilityControls.Controls.Add(Me.lblUnavailabilityCompareWarehouses)
        Me.pnlUnavailabilityControls.Controls.Add(Me.txtUnavailabilityProjectDesc)
        Me.pnlUnavailabilityControls.Controls.Add(Me.txtUnavailabilityProjectNo)
        Me.pnlUnavailabilityControls.Controls.Add(Me.cmdUnavailabilityProjectNumber)
        Me.pnlUnavailabilityControls.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlUnavailabilityControls.Location = New System.Drawing.Point(0, 0)
        Me.pnlUnavailabilityControls.Name = "pnlUnavailabilityControls"
        Me.pnlUnavailabilityControls.Size = New System.Drawing.Size(588, 81)
        Me.pnlUnavailabilityControls.TabIndex = 1
        '
        'chkUnavailabilitySumWarehouses
        '
        Me.chkUnavailabilitySumWarehouses.AutoSize = True
        Me.chkUnavailabilitySumWarehouses.Location = New System.Drawing.Point(7, 59)
        Me.chkUnavailabilitySumWarehouses.Name = "chkUnavailabilitySumWarehouses"
        Me.chkUnavailabilitySumWarehouses.Size = New System.Drawing.Size(47, 17)
        Me.chkUnavailabilitySumWarehouses.TabIndex = 10
        Me.chkUnavailabilitySumWarehouses.Text = "Sum"
        Me.chkUnavailabilitySumWarehouses.UseVisualStyleBackColor = True
        '
        'chkShowRemovedParts
        '
        Me.chkShowRemovedParts.AutoSize = True
        Me.chkShowRemovedParts.Location = New System.Drawing.Point(455, 59)
        Me.chkShowRemovedParts.Name = "chkShowRemovedParts"
        Me.chkShowRemovedParts.Size = New System.Drawing.Size(129, 17)
        Me.chkShowRemovedParts.TabIndex = 9
        Me.chkShowRemovedParts.Text = "Show Removed Parts"
        Me.chkShowRemovedParts.UseVisualStyleBackColor = True
        '
        'chkUnvailabilityHideWhenSumBottleneckGreaterThansZero
        '
        Me.chkUnvailabilityHideWhenSumBottleneckGreaterThansZero.AutoSize = True
        Me.chkUnvailabilityHideWhenSumBottleneckGreaterThansZero.Location = New System.Drawing.Point(206, 59)
        Me.chkUnvailabilityHideWhenSumBottleneckGreaterThansZero.Name = "chkUnvailabilityHideWhenSumBottleneckGreaterThansZero"
        Me.chkUnvailabilityHideWhenSumBottleneckGreaterThansZero.Size = New System.Drawing.Size(243, 17)
        Me.chkUnvailabilityHideWhenSumBottleneckGreaterThansZero.TabIndex = 8
        Me.chkUnvailabilityHideWhenSumBottleneckGreaterThansZero.Text = "Hide when Compare WH SUM Bottleneck > 0"
        Me.chkUnvailabilityHideWhenSumBottleneckGreaterThansZero.UseVisualStyleBackColor = True
        '
        'chkUnavailabilityHideTurnaroundDays
        '
        Me.chkUnavailabilityHideTurnaroundDays.AutoSize = True
        Me.chkUnavailabilityHideTurnaroundDays.Checked = True
        Me.chkUnavailabilityHideTurnaroundDays.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUnavailabilityHideTurnaroundDays.Location = New System.Drawing.Point(67, 59)
        Me.chkUnavailabilityHideTurnaroundDays.Name = "chkUnavailabilityHideTurnaroundDays"
        Me.chkUnavailabilityHideTurnaroundDays.Size = New System.Drawing.Size(133, 17)
        Me.chkUnavailabilityHideTurnaroundDays.TabIndex = 7
        Me.chkUnavailabilityHideTurnaroundDays.Text = "Hide Turnaround Days"
        Me.chkUnavailabilityHideTurnaroundDays.UseVisualStyleBackColor = True
        '
        'cmdUnavailabilityRefresh
        '
        Me.cmdUnavailabilityRefresh.Image = Global.SchedulingBoard.My.Resources.Resources.refresh
        Me.cmdUnavailabilityRefresh.Location = New System.Drawing.Point(14, 3)
        Me.cmdUnavailabilityRefresh.Name = "cmdUnavailabilityRefresh"
        Me.cmdUnavailabilityRefresh.Size = New System.Drawing.Size(24, 24)
        Me.cmdUnavailabilityRefresh.TabIndex = 6
        Me.cmdUnavailabilityRefresh.UseVisualStyleBackColor = True
        '
        'cmdUnavailabilitySetCompareWarehouses
        '
        Me.cmdUnavailabilitySetCompareWarehouses.Location = New System.Drawing.Point(172, 30)
        Me.cmdUnavailabilitySetCompareWarehouses.Name = "cmdUnavailabilitySetCompareWarehouses"
        Me.cmdUnavailabilitySetCompareWarehouses.Size = New System.Drawing.Size(40, 23)
        Me.cmdUnavailabilitySetCompareWarehouses.TabIndex = 5
        Me.cmdUnavailabilitySetCompareWarehouses.Text = "Set"
        Me.cmdUnavailabilitySetCompareWarehouses.UseVisualStyleBackColor = True
        '
        'flpUnavailabilityCompareWarehouses
        '
        Me.flpUnavailabilityCompareWarehouses.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.flpUnavailabilityCompareWarehouses.Location = New System.Drawing.Point(291, 30)
        Me.flpUnavailabilityCompareWarehouses.Name = "flpUnavailabilityCompareWarehouses"
        Me.flpUnavailabilityCompareWarehouses.Size = New System.Drawing.Size(294, 23)
        Me.flpUnavailabilityCompareWarehouses.TabIndex = 4
        '
        'lblUnavailabilityCompareWarehouses
        '
        Me.lblUnavailabilityCompareWarehouses.AutoSize = True
        Me.lblUnavailabilityCompareWarehouses.Location = New System.Drawing.Point(214, 35)
        Me.lblUnavailabilityCompareWarehouses.Name = "lblUnavailabilityCompareWarehouses"
        Me.lblUnavailabilityCompareWarehouses.Size = New System.Drawing.Size(70, 13)
        Me.lblUnavailabilityCompareWarehouses.TabIndex = 3
        Me.lblUnavailabilityCompareWarehouses.Text = "Warehouses:"
        '
        'txtUnavailabilityProjectDesc
        '
        Me.txtUnavailabilityProjectDesc.Location = New System.Drawing.Point(257, 5)
        Me.txtUnavailabilityProjectDesc.Name = "txtUnavailabilityProjectDesc"
        Me.txtUnavailabilityProjectDesc.ReadOnly = True
        Me.txtUnavailabilityProjectDesc.Size = New System.Drawing.Size(328, 20)
        Me.txtUnavailabilityProjectDesc.TabIndex = 2
        Me.txtUnavailabilityProjectDesc.Text = "All Projects"
        '
        'cmdUnavailabilityProjectNumber
        '
        Me.cmdUnavailabilityProjectNumber.Location = New System.Drawing.Point(52, 3)
        Me.cmdUnavailabilityProjectNumber.Name = "cmdUnavailabilityProjectNumber"
        Me.cmdUnavailabilityProjectNumber.Size = New System.Drawing.Size(67, 24)
        Me.cmdUnavailabilityProjectNumber.TabIndex = 0
        Me.cmdUnavailabilityProjectNumber.Text = "Project #"
        Me.cmdUnavailabilityProjectNumber.UseVisualStyleBackColor = True
        '
        'fgSchedulingBoard
        '
        Me.fgSchedulingBoard.ContextMenuStrip = Me.mnuFlexgrid
        Me.fgSchedulingBoard.Dock = System.Windows.Forms.DockStyle.Fill
        Me.fgSchedulingBoard.Location = New System.Drawing.Point(3, 22)
        Me.fgSchedulingBoard.Name = "fgSchedulingBoard"
        Me.fgSchedulingBoard.OcxState = CType(resources.GetObject("fgSchedulingBoard.OcxState"), System.Windows.Forms.AxHost.State)
        Me.fgSchedulingBoard.Size = New System.Drawing.Size(947, 290)
        Me.fgSchedulingBoard.TabIndex = 9
        '
        'mnuFlexgrid
        '
        Me.mnuFlexgrid.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.mnuFlexgrid.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SubstitutePartToolStripMenuItem, Me.ChangeOrderedQtyToolStripMenuItem, Me.mnuSubstitutePartSeparator, Me.ClearHighlightingToolStripMenuItem, Me.ToolStripSeparator2, Me.ProjectMaintenanceToolStripMenuItem, Me.TransferToolsToolStripMenuItem, Me.AvailabilityToolStripMenuItem, Me.PartMaintenanceToolStripMenuItem, Me.ToolStripSeparator3, Me.CopyPartBarcodeToolStripMenuItem, Me.CopyPartNumberToolStripMenuItem, Me.CopyPartDescriptionToolStripMenuItem, Me.CopyPartNumberAndDescriptionToolStripMenuItem, Me.CopyPhaseNumberToolStripMenuItem, Me.CopyPhaseDescriptionToolStripMenuItem, Me.CopyPhaseNumberAndDescriptionToolStripMenuItem})
        Me.mnuFlexgrid.Name = "mnuFlexgrid"
        Me.mnuFlexgrid.Size = New System.Drawing.Size(259, 386)
        '
        'SubstitutePartToolStripMenuItem
        '
        Me.SubstitutePartToolStripMenuItem.Name = "SubstitutePartToolStripMenuItem"
        Me.SubstitutePartToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.SubstitutePartToolStripMenuItem.Text = "Substitute Part"
        '
        'ChangeOrderedQtyToolStripMenuItem
        '
        Me.ChangeOrderedQtyToolStripMenuItem.Name = "ChangeOrderedQtyToolStripMenuItem"
        Me.ChangeOrderedQtyToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.ChangeOrderedQtyToolStripMenuItem.Text = "Change Ordered Qty"
        '
        'mnuSubstitutePartSeparator
        '
        Me.mnuSubstitutePartSeparator.Name = "mnuSubstitutePartSeparator"
        Me.mnuSubstitutePartSeparator.Size = New System.Drawing.Size(255, 6)
        '
        'ClearHighlightingToolStripMenuItem
        '
        Me.ClearHighlightingToolStripMenuItem.Name = "ClearHighlightingToolStripMenuItem"
        Me.ClearHighlightingToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.ClearHighlightingToolStripMenuItem.Text = "Clear Highlighting"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(255, 6)
        '
        'ProjectMaintenanceToolStripMenuItem
        '
        Me.ProjectMaintenanceToolStripMenuItem.Image = Global.SchedulingBoard.My.Resources.Resources.settings
        Me.ProjectMaintenanceToolStripMenuItem.Name = "ProjectMaintenanceToolStripMenuItem"
        Me.ProjectMaintenanceToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.ProjectMaintenanceToolStripMenuItem.Text = "Project Maintenance"
        '
        'TransferToolsToolStripMenuItem
        '
        Me.TransferToolsToolStripMenuItem.Image = Global.SchedulingBoard.My.Resources.Resources.MSPAINT2
        Me.TransferToolsToolStripMenuItem.Name = "TransferToolsToolStripMenuItem"
        Me.TransferToolsToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.TransferToolsToolStripMenuItem.Text = "Transfer Tools"
        '
        'AvailabilityToolStripMenuItem
        '
        Me.AvailabilityToolStripMenuItem.Image = Global.SchedulingBoard.My.Resources.Resources.availability
        Me.AvailabilityToolStripMenuItem.Name = "AvailabilityToolStripMenuItem"
        Me.AvailabilityToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.AvailabilityToolStripMenuItem.Text = "Availability"
        '
        'PartMaintenanceToolStripMenuItem
        '
        Me.PartMaintenanceToolStripMenuItem.Image = Global.SchedulingBoard.My.Resources.Resources.Cube
        Me.PartMaintenanceToolStripMenuItem.Name = "PartMaintenanceToolStripMenuItem"
        Me.PartMaintenanceToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.PartMaintenanceToolStripMenuItem.Text = "Part Maintenance"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(255, 6)
        '
        'CopyPartBarcodeToolStripMenuItem
        '
        Me.CopyPartBarcodeToolStripMenuItem.Name = "CopyPartBarcodeToolStripMenuItem"
        Me.CopyPartBarcodeToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.CopyPartBarcodeToolStripMenuItem.Text = "Copy Part Barcode"
        '
        'CopyPartNumberToolStripMenuItem
        '
        Me.CopyPartNumberToolStripMenuItem.Name = "CopyPartNumberToolStripMenuItem"
        Me.CopyPartNumberToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.CopyPartNumberToolStripMenuItem.Text = "Copy Part Number"
        '
        'CopyPartDescriptionToolStripMenuItem
        '
        Me.CopyPartDescriptionToolStripMenuItem.Name = "CopyPartDescriptionToolStripMenuItem"
        Me.CopyPartDescriptionToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.CopyPartDescriptionToolStripMenuItem.Text = "Copy Part Description"
        '
        'CopyPartNumberAndDescriptionToolStripMenuItem
        '
        Me.CopyPartNumberAndDescriptionToolStripMenuItem.Name = "CopyPartNumberAndDescriptionToolStripMenuItem"
        Me.CopyPartNumberAndDescriptionToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.CopyPartNumberAndDescriptionToolStripMenuItem.Text = "Copy Part Number (Description)"
        '
        'CopyPhaseNumberToolStripMenuItem
        '
        Me.CopyPhaseNumberToolStripMenuItem.Name = "CopyPhaseNumberToolStripMenuItem"
        Me.CopyPhaseNumberToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.CopyPhaseNumberToolStripMenuItem.Text = "Copy Phase Number"
        '
        'CopyPhaseDescriptionToolStripMenuItem
        '
        Me.CopyPhaseDescriptionToolStripMenuItem.Name = "CopyPhaseDescriptionToolStripMenuItem"
        Me.CopyPhaseDescriptionToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.CopyPhaseDescriptionToolStripMenuItem.Text = "Copy Phase Description"
        '
        'CopyPhaseNumberAndDescriptionToolStripMenuItem
        '
        Me.CopyPhaseNumberAndDescriptionToolStripMenuItem.Name = "CopyPhaseNumberAndDescriptionToolStripMenuItem"
        Me.CopyPhaseNumberAndDescriptionToolStripMenuItem.Size = New System.Drawing.Size(258, 26)
        Me.CopyPhaseNumberAndDescriptionToolStripMenuItem.Text = "Copy Phase Number (Description)"
        '
        'pnlSchedulingBoardControls
        '
        Me.pnlSchedulingBoardControls.BackColor = System.Drawing.SystemColors.Control
        Me.pnlSchedulingBoardControls.Controls.Add(Me.pbEmailTeam)
        Me.pnlSchedulingBoardControls.Controls.Add(Me.LblProjectName)
        Me.pnlSchedulingBoardControls.Controls.Add(Me.chkIncludeProposals)
        Me.pnlSchedulingBoardControls.Controls.Add(Me.chkShowDetail)
        Me.pnlSchedulingBoardControls.Controls.Add(Me.chkShowFuture)
        Me.pnlSchedulingBoardControls.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlSchedulingBoardControls.Location = New System.Drawing.Point(3, 3)
        Me.pnlSchedulingBoardControls.Name = "pnlSchedulingBoardControls"
        Me.pnlSchedulingBoardControls.Size = New System.Drawing.Size(947, 19)
        Me.pnlSchedulingBoardControls.TabIndex = 10
        '
        'pbEmailTeam
        '
        Me.pbEmailTeam.Image = Global.SchedulingBoard.My.Resources.Resources.OBarMail
        Me.pbEmailTeam.Location = New System.Drawing.Point(532, 2)
        Me.pbEmailTeam.Name = "pbEmailTeam"
        Me.pbEmailTeam.Size = New System.Drawing.Size(20, 17)
        Me.pbEmailTeam.TabIndex = 9
        Me.pbEmailTeam.TabStop = False
        '
        'LblProjectName
        '
        Me.LblProjectName.AutoSize = True
        Me.LblProjectName.Location = New System.Drawing.Point(553, 3)
        Me.LblProjectName.Name = "LblProjectName"
        Me.LblProjectName.Size = New System.Drawing.Size(16, 13)
        Me.LblProjectName.TabIndex = 8
        Me.LblProjectName.Text = "..."
        '
        'chkIncludeProposals
        '
        Me.chkIncludeProposals.Checked = True
        Me.chkIncludeProposals.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkIncludeProposals.Location = New System.Drawing.Point(322, -2)
        Me.chkIncludeProposals.Name = "chkIncludeProposals"
        Me.chkIncludeProposals.Size = New System.Drawing.Size(133, 19)
        Me.chkIncludeProposals.TabIndex = 4
        Me.chkIncludeProposals.Text = "Include Proposals"
        Me.chkIncludeProposals.UseVisualStyleBackColor = False
        '
        'chkShowDetail
        '
        Me.chkShowDetail.Checked = True
        Me.chkShowDetail.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowDetail.Location = New System.Drawing.Point(3, -2)
        Me.chkShowDetail.Name = "chkShowDetail"
        Me.chkShowDetail.Size = New System.Drawing.Size(175, 19)
        Me.chkShowDetail.TabIndex = 6
        Me.chkShowDetail.Text = "Show Current / Past Detail"
        Me.chkShowDetail.UseVisualStyleBackColor = True
        '
        'chkShowFuture
        '
        Me.chkShowFuture.Checked = True
        Me.chkShowFuture.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkShowFuture.Location = New System.Drawing.Point(185, -2)
        Me.chkShowFuture.Name = "chkShowFuture"
        Me.chkShowFuture.Size = New System.Drawing.Size(131, 19)
        Me.chkShowFuture.TabIndex = 7
        Me.chkShowFuture.Text = "Show Future Detail"
        Me.chkShowFuture.UseVisualStyleBackColor = True
        '
        'tpTimeline
        '
        Me.tpTimeline.Controls.Add(Me.fgTimeline)
        Me.tpTimeline.Location = New System.Drawing.Point(4, 22)
        Me.tpTimeline.Name = "tpTimeline"
        Me.tpTimeline.Padding = New System.Windows.Forms.Padding(3)
        Me.tpTimeline.Size = New System.Drawing.Size(953, 315)
        Me.tpTimeline.TabIndex = 2
        Me.tpTimeline.Text = "Timeline (Multi-Part)"
        Me.tpTimeline.UseVisualStyleBackColor = True
        '
        'fgTimeline
        '
        Me.fgTimeline.ContextMenuStrip = Me.mnuFlexgrid
        Me.fgTimeline.Dock = System.Windows.Forms.DockStyle.Fill
        Me.fgTimeline.Location = New System.Drawing.Point(3, 3)
        Me.fgTimeline.Name = "fgTimeline"
        Me.fgTimeline.OcxState = CType(resources.GetObject("fgTimeline.OcxState"), System.Windows.Forms.AxHost.State)
        Me.fgTimeline.Size = New System.Drawing.Size(947, 309)
        Me.fgTimeline.TabIndex = 10
        '
        'tpCalendar
        '
        Me.tpCalendar.Controls.Add(Me.fgCalendar)
        Me.tpCalendar.Location = New System.Drawing.Point(4, 22)
        Me.tpCalendar.Name = "tpCalendar"
        Me.tpCalendar.Padding = New System.Windows.Forms.Padding(3)
        Me.tpCalendar.Size = New System.Drawing.Size(953, 315)
        Me.tpCalendar.TabIndex = 0
        Me.tpCalendar.Text = "Calendar (Single-Part)"
        Me.tpCalendar.UseVisualStyleBackColor = True
        '
        'fgCalendar
        '
        Me.fgCalendar.ContextMenuStrip = Me.mnuFlexgrid
        Me.fgCalendar.Dock = System.Windows.Forms.DockStyle.Fill
        Me.fgCalendar.Location = New System.Drawing.Point(3, 3)
        Me.fgCalendar.Name = "fgCalendar"
        Me.fgCalendar.OcxState = CType(resources.GetObject("fgCalendar.OcxState"), System.Windows.Forms.AxHost.State)
        Me.fgCalendar.Size = New System.Drawing.Size(947, 309)
        Me.fgCalendar.TabIndex = 10
        '
        'StatusStrip1
        '
        Me.StatusStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.sslblLength, Me.sslblLengthValue, Me.sslblWidth, Me.sslblWidthValue, Me.sslblHeight, Me.sslblHeightValue, Me.sslblWeight, Me.sslblWeightValue, Me.sslblRentalValue, Me.sslblRentalValueValue, Me.sslblWeeklyRate, Me.sslblWeeklyRateValue, Me.sslbl2DayWeekRate, Me.sslbl2DayWeekRateValue, Me.sslblDailyRate, Me.sslblDailyRateValue})
        Me.StatusStrip1.Location = New System.Drawing.Point(38, 341)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(961, 22)
        Me.StatusStrip1.SizingGrip = False
        Me.StatusStrip1.TabIndex = 12
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'sslblLength
        '
        Me.sslblLength.Name = "sslblLength"
        Me.sslblLength.Size = New System.Drawing.Size(16, 17)
        Me.sslblLength.Text = "L:"
        '
        'sslblLengthValue
        '
        Me.sslblLengthValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sslblLengthValue.Name = "sslblLengthValue"
        Me.sslblLengthValue.Size = New System.Drawing.Size(55, 17)
        Me.sslblLengthValue.Text = "(...) UOM"
        Me.sslblLengthValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sslblWidth
        '
        Me.sslblWidth.Name = "sslblWidth"
        Me.sslblWidth.Size = New System.Drawing.Size(21, 17)
        Me.sslblWidth.Text = "W:"
        '
        'sslblWidthValue
        '
        Me.sslblWidthValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sslblWidthValue.Name = "sslblWidthValue"
        Me.sslblWidthValue.Size = New System.Drawing.Size(55, 17)
        Me.sslblWidthValue.Text = "(...) UOM"
        Me.sslblWidthValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sslblHeight
        '
        Me.sslblHeight.Name = "sslblHeight"
        Me.sslblHeight.Size = New System.Drawing.Size(19, 17)
        Me.sslblHeight.Text = "H:"
        '
        'sslblHeightValue
        '
        Me.sslblHeightValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sslblHeightValue.Name = "sslblHeightValue"
        Me.sslblHeightValue.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.sslblHeightValue.Size = New System.Drawing.Size(65, 17)
        Me.sslblHeightValue.Text = "(...) UOM"
        Me.sslblHeightValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sslblWeight
        '
        Me.sslblWeight.Name = "sslblWeight"
        Me.sslblWeight.Size = New System.Drawing.Size(48, 17)
        Me.sslblWeight.Text = "Weight:"
        '
        'sslblWeightValue
        '
        Me.sslblWeightValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sslblWeightValue.Name = "sslblWeightValue"
        Me.sslblWeightValue.Padding = New System.Windows.Forms.Padding(0, 0, 15, 0)
        Me.sslblWeightValue.Size = New System.Drawing.Size(70, 17)
        Me.sslblWeightValue.Text = "(...) UOM"
        Me.sslblWeightValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sslblRentalValue
        '
        Me.sslblRentalValue.Name = "sslblRentalValue"
        Me.sslblRentalValue.Size = New System.Drawing.Size(74, 17)
        Me.sslblRentalValue.Text = "Rental Value:"
        '
        'sslblRentalValueValue
        '
        Me.sslblRentalValueValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sslblRentalValueValue.Name = "sslblRentalValueValue"
        Me.sslblRentalValueValue.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.sslblRentalValueValue.Size = New System.Drawing.Size(41, 17)
        Me.sslblRentalValueValue.Text = "$(...)"
        Me.sslblRentalValueValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sslblWeeklyRate
        '
        Me.sslblWeeklyRate.Name = "sslblWeeklyRate"
        Me.sslblWeeklyRate.Size = New System.Drawing.Size(101, 17)
        Me.sslblWeeklyRate.Text = "Weekly Rate (1%):"
        '
        'sslblWeeklyRateValue
        '
        Me.sslblWeeklyRateValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sslblWeeklyRateValue.Name = "sslblWeeklyRateValue"
        Me.sslblWeeklyRateValue.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.sslblWeeklyRateValue.Size = New System.Drawing.Size(41, 17)
        Me.sslblWeeklyRateValue.Text = "$(...)"
        Me.sslblWeeklyRateValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sslbl2DayWeekRate
        '
        Me.sslbl2DayWeekRate.Name = "sslbl2DayWeekRate"
        Me.sslbl2DayWeekRate.Size = New System.Drawing.Size(99, 17)
        Me.sslbl2DayWeekRate.Text = "2-Day Week Rate:"
        '
        'sslbl2DayWeekRateValue
        '
        Me.sslbl2DayWeekRateValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sslbl2DayWeekRateValue.Name = "sslbl2DayWeekRateValue"
        Me.sslbl2DayWeekRateValue.Padding = New System.Windows.Forms.Padding(0, 0, 5, 0)
        Me.sslbl2DayWeekRateValue.Size = New System.Drawing.Size(36, 17)
        Me.sslbl2DayWeekRateValue.Text = "$(...)"
        Me.sslbl2DayWeekRateValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'sslblDailyRate
        '
        Me.sslblDailyRate.Name = "sslblDailyRate"
        Me.sslblDailyRate.Size = New System.Drawing.Size(62, 17)
        Me.sslblDailyRate.Text = "Daily Rate:"
        '
        'sslblDailyRateValue
        '
        Me.sslblDailyRateValue.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.sslblDailyRateValue.Name = "sslblDailyRateValue"
        Me.sslblDailyRateValue.Size = New System.Drawing.Size(31, 17)
        Me.sslblDailyRateValue.Text = "$(...)"
        Me.sslblDailyRateValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'tsMainVert
        '
        Me.tsMainVert.Dock = System.Windows.Forms.DockStyle.Left
        Me.tsMainVert.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tsMainVert.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.tsMainVert.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsbProjectMaintenance, Me.tsbTransferTools, Me.QueryToolsToolStripDropDownButton, Me.tsbBLInq, Me.tsbAvailability, Me.tsbCycleCountUtility, Me.tsbUtilization, Me.tsbPartMaintenance, Me.tsbAddInventory, Me.tsbRemoveInventory, Me.tsbDeviceMaintenance, Me.tsbPartAttachmentMaintenance, Me.tsbCrewTools, Me.tsbProjectStoryboard, Me.tsbShowSearchCriteria, Me.tsbRepairLog, Me.tsbMode})
        Me.tsMainVert.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.tsMainVert.Location = New System.Drawing.Point(0, 0)
        Me.tsMainVert.Name = "tsMainVert"
        Me.tsMainVert.Size = New System.Drawing.Size(38, 363)
        Me.tsMainVert.TabIndex = 10
        Me.tsMainVert.Text = "ToolStrip1"
        '
        'tsbProjectMaintenance
        '
        Me.tsbProjectMaintenance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbProjectMaintenance.Image = Global.SchedulingBoard.My.Resources.Resources.settings
        Me.tsbProjectMaintenance.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbProjectMaintenance.Name = "tsbProjectMaintenance"
        Me.tsbProjectMaintenance.Size = New System.Drawing.Size(35, 28)
        Me.tsbProjectMaintenance.Tag = "Project Maintenance"
        Me.tsbProjectMaintenance.Text = "Project Maintenance"
        '
        'tsbTransferTools
        '
        Me.tsbTransferTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbTransferTools.Image = Global.SchedulingBoard.My.Resources.Resources.MSPAINT2
        Me.tsbTransferTools.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbTransferTools.Name = "tsbTransferTools"
        Me.tsbTransferTools.Size = New System.Drawing.Size(35, 28)
        Me.tsbTransferTools.Tag = "TransferInquiryTools"
        Me.tsbTransferTools.Text = "Transfer Tools"
        '
        'QueryToolsToolStripDropDownButton
        '
        Me.QueryToolsToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.QueryToolsToolStripDropDownButton.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EquipmentLocationsToolStripMenuItem1, Me.TransactionsToolStripMenuItem1, Me.PhaseHistoryToolStripMenuItem1, Me.ToolStripSeparator14, Me.GearListToolStripMenuItem1, Me.ManifestSummaryToolStripMenuItem1, Me.ManifestToolStripMenuItem1, Me.CarnetListToolStripMenuItem1, Me.TicSheetToolStripMenuItem1, Me.ToolStripSeparator15, Me.LateReturnsToolStripMenuItem1, Me.BatchLocationSummaryToolStripMenuItem1})
        Me.QueryToolsToolStripDropDownButton.Image = Global.SchedulingBoard.My.Resources.Resources.OrderDetails48
        Me.QueryToolsToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.QueryToolsToolStripDropDownButton.Name = "QueryToolsToolStripDropDownButton"
        Me.QueryToolsToolStripDropDownButton.Size = New System.Drawing.Size(35, 28)
        Me.QueryToolsToolStripDropDownButton.Tag = "Query Tools"
        Me.QueryToolsToolStripDropDownButton.Text = "Query Tools"
        '
        'EquipmentLocationsToolStripMenuItem1
        '
        Me.EquipmentLocationsToolStripMenuItem1.Name = "EquipmentLocationsToolStripMenuItem1"
        Me.EquipmentLocationsToolStripMenuItem1.Size = New System.Drawing.Size(207, 22)
        Me.EquipmentLocationsToolStripMenuItem1.Tag = "Query Tools"
        Me.EquipmentLocationsToolStripMenuItem1.Text = "Equipment Locations"
        '
        'TransactionsToolStripMenuItem1
        '
        Me.TransactionsToolStripMenuItem1.Name = "TransactionsToolStripMenuItem1"
        Me.TransactionsToolStripMenuItem1.Size = New System.Drawing.Size(207, 22)
        Me.TransactionsToolStripMenuItem1.Tag = "Query Tools"
        Me.TransactionsToolStripMenuItem1.Text = "Transactions"
        '
        'PhaseHistoryToolStripMenuItem1
        '
        Me.PhaseHistoryToolStripMenuItem1.Name = "PhaseHistoryToolStripMenuItem1"
        Me.PhaseHistoryToolStripMenuItem1.Size = New System.Drawing.Size(207, 22)
        Me.PhaseHistoryToolStripMenuItem1.Tag = "Query Tools"
        Me.PhaseHistoryToolStripMenuItem1.Text = "Phase History"
        '
        'ToolStripSeparator14
        '
        Me.ToolStripSeparator14.Name = "ToolStripSeparator14"
        Me.ToolStripSeparator14.Size = New System.Drawing.Size(204, 6)
        '
        'GearListToolStripMenuItem1
        '
        Me.GearListToolStripMenuItem1.Name = "GearListToolStripMenuItem1"
        Me.GearListToolStripMenuItem1.Size = New System.Drawing.Size(207, 22)
        Me.GearListToolStripMenuItem1.Tag = "Query Tools"
        Me.GearListToolStripMenuItem1.Text = "Gear List"
        '
        'ManifestSummaryToolStripMenuItem1
        '
        Me.ManifestSummaryToolStripMenuItem1.Name = "ManifestSummaryToolStripMenuItem1"
        Me.ManifestSummaryToolStripMenuItem1.Size = New System.Drawing.Size(207, 22)
        Me.ManifestSummaryToolStripMenuItem1.Tag = "Query Tools"
        Me.ManifestSummaryToolStripMenuItem1.Text = "Manifest Summary"
        '
        'ManifestToolStripMenuItem1
        '
        Me.ManifestToolStripMenuItem1.Name = "ManifestToolStripMenuItem1"
        Me.ManifestToolStripMenuItem1.Size = New System.Drawing.Size(207, 22)
        Me.ManifestToolStripMenuItem1.Tag = "Query Tools"
        Me.ManifestToolStripMenuItem1.Text = "Manifest"
        '
        'CarnetListToolStripMenuItem1
        '
        Me.CarnetListToolStripMenuItem1.Name = "CarnetListToolStripMenuItem1"
        Me.CarnetListToolStripMenuItem1.Size = New System.Drawing.Size(207, 22)
        Me.CarnetListToolStripMenuItem1.Tag = "Query Tools"
        Me.CarnetListToolStripMenuItem1.Text = "Carnet List"
        '
        'TicSheetToolStripMenuItem1
        '
        Me.TicSheetToolStripMenuItem1.Name = "TicSheetToolStripMenuItem1"
        Me.TicSheetToolStripMenuItem1.Size = New System.Drawing.Size(207, 22)
        Me.TicSheetToolStripMenuItem1.Tag = "Query Tools"
        Me.TicSheetToolStripMenuItem1.Text = "Tic Sheet"
        '
        'ToolStripSeparator15
        '
        Me.ToolStripSeparator15.Name = "ToolStripSeparator15"
        Me.ToolStripSeparator15.Size = New System.Drawing.Size(204, 6)
        '
        'LateReturnsToolStripMenuItem1
        '
        Me.LateReturnsToolStripMenuItem1.Name = "LateReturnsToolStripMenuItem1"
        Me.LateReturnsToolStripMenuItem1.Size = New System.Drawing.Size(207, 22)
        Me.LateReturnsToolStripMenuItem1.Tag = "Query Tools"
        Me.LateReturnsToolStripMenuItem1.Text = "Late Returns"
        '
        'BatchLocationSummaryToolStripMenuItem1
        '
        Me.BatchLocationSummaryToolStripMenuItem1.Name = "BatchLocationSummaryToolStripMenuItem1"
        Me.BatchLocationSummaryToolStripMenuItem1.Size = New System.Drawing.Size(207, 22)
        Me.BatchLocationSummaryToolStripMenuItem1.Tag = "Query Tools"
        Me.BatchLocationSummaryToolStripMenuItem1.Text = "Batch Location Summary"
        '
        'tsbBLInq
        '
        Me.tsbBLInq.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbBLInq.Image = Global.SchedulingBoard.My.Resources.Resources.blinq
        Me.tsbBLInq.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbBLInq.Name = "tsbBLInq"
        Me.tsbBLInq.Size = New System.Drawing.Size(35, 28)
        Me.tsbBLInq.Tag = "prjinblret"
        Me.tsbBLInq.Text = "BLInq (Batch Location Inquiry)"
        '
        'tsbAvailability
        '
        Me.tsbAvailability.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbAvailability.Image = Global.SchedulingBoard.My.Resources.Resources.availability
        Me.tsbAvailability.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAvailability.Name = "tsbAvailability"
        Me.tsbAvailability.Size = New System.Drawing.Size(35, 28)
        Me.tsbAvailability.Tag = "prjpjinvcal"
        Me.tsbAvailability.Text = "Availability"
        '
        'tsbCycleCountUtility
        '
        Me.tsbCycleCountUtility.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbCycleCountUtility.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CycleCountUtilityToolStripMenuItem, Me.MarkAsCountedToolStripMenuItem})
        Me.tsbCycleCountUtility.Image = Global.SchedulingBoard.My.Resources.Resources.CheckmarkBox
        Me.tsbCycleCountUtility.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCycleCountUtility.Name = "tsbCycleCountUtility"
        Me.tsbCycleCountUtility.Size = New System.Drawing.Size(35, 28)
        Me.tsbCycleCountUtility.Tag = "Cycle Count"
        Me.tsbCycleCountUtility.Text = "Cycle Count Utility"
        '
        'CycleCountUtilityToolStripMenuItem
        '
        Me.CycleCountUtilityToolStripMenuItem.Name = "CycleCountUtilityToolStripMenuItem"
        Me.CycleCountUtilityToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.CycleCountUtilityToolStripMenuItem.Text = "Cycle Count Utility"
        '
        'MarkAsCountedToolStripMenuItem
        '
        Me.MarkAsCountedToolStripMenuItem.Name = "MarkAsCountedToolStripMenuItem"
        Me.MarkAsCountedToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.MarkAsCountedToolStripMenuItem.Tag = "Cycle Count"
        Me.MarkAsCountedToolStripMenuItem.Text = "Mark As Counted"
        '
        'tsbUtilization
        '
        Me.tsbUtilization.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbUtilization.Image = Global.SchedulingBoard.My.Resources.Resources.utilization
        Me.tsbUtilization.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbUtilization.Name = "tsbUtilization"
        Me.tsbUtilization.Size = New System.Drawing.Size(35, 28)
        Me.tsbUtilization.Tag = "Utilization"
        Me.tsbUtilization.Text = "Utilization"
        '
        'tsbPartMaintenance
        '
        Me.tsbPartMaintenance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPartMaintenance.Image = Global.SchedulingBoard.My.Resources.Resources.Cube
        Me.tsbPartMaintenance.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPartMaintenance.Name = "tsbPartMaintenance"
        Me.tsbPartMaintenance.Size = New System.Drawing.Size(35, 28)
        Me.tsbPartMaintenance.Tag = "Part Maintenance"
        Me.tsbPartMaintenance.Text = "Part Maintenance"
        '
        'tsbAddInventory
        '
        Me.tsbAddInventory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbAddInventory.Image = Global.SchedulingBoard.My.Resources.Resources.MISC05
        Me.tsbAddInventory.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAddInventory.Name = "tsbAddInventory"
        Me.tsbAddInventory.Size = New System.Drawing.Size(35, 28)
        Me.tsbAddInventory.Tag = "Add Inventory"
        Me.tsbAddInventory.Text = "Add Inventory"
        '
        'tsbRemoveInventory
        '
        Me.tsbRemoveInventory.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbRemoveInventory.Image = Global.SchedulingBoard.My.Resources.Resources.Deleting1
        Me.tsbRemoveInventory.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRemoveInventory.Name = "tsbRemoveInventory"
        Me.tsbRemoveInventory.Size = New System.Drawing.Size(35, 28)
        Me.tsbRemoveInventory.Tag = "Device Maintenance"
        Me.tsbRemoveInventory.Text = "Remove Inventory"
        '
        'tsbDeviceMaintenance
        '
        Me.tsbDeviceMaintenance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDeviceMaintenance.Image = Global.SchedulingBoard.My.Resources.Resources.app_Icon
        Me.tsbDeviceMaintenance.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDeviceMaintenance.Name = "tsbDeviceMaintenance"
        Me.tsbDeviceMaintenance.Size = New System.Drawing.Size(35, 28)
        Me.tsbDeviceMaintenance.Tag = "Device Maintenance"
        Me.tsbDeviceMaintenance.Text = "Device Maintenance"
        '
        'tsbPartAttachmentMaintenance
        '
        Me.tsbPartAttachmentMaintenance.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbPartAttachmentMaintenance.Image = Global.SchedulingBoard.My.Resources.Resources.addin
        Me.tsbPartAttachmentMaintenance.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbPartAttachmentMaintenance.Name = "tsbPartAttachmentMaintenance"
        Me.tsbPartAttachmentMaintenance.Size = New System.Drawing.Size(28, 28)
        Me.tsbPartAttachmentMaintenance.Tag = "Part Attachments Maintenance"
        Me.tsbPartAttachmentMaintenance.Text = "Part Attachment Maintenance"
        '
        'tsbCrewTools
        '
        Me.tsbCrewTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbCrewTools.Image = Global.SchedulingBoard.My.Resources.Resources.emps
        Me.tsbCrewTools.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbCrewTools.Name = "tsbCrewTools"
        Me.tsbCrewTools.Size = New System.Drawing.Size(28, 28)
        Me.tsbCrewTools.Tag = "Crew Tools"
        Me.tsbCrewTools.Text = "Crew Tools"
        '
        'tsbProjectStoryboard
        '
        Me.tsbProjectStoryboard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbProjectStoryboard.Image = Global.SchedulingBoard.My.Resources.Resources.GRAPH01
        Me.tsbProjectStoryboard.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbProjectStoryboard.Name = "tsbProjectStoryboard"
        Me.tsbProjectStoryboard.Size = New System.Drawing.Size(28, 28)
        Me.tsbProjectStoryboard.Tag = "Project Storyboard"
        Me.tsbProjectStoryboard.Text = "Project Storyboard"
        '
        'tsbShowSearchCriteria
        '
        Me.tsbShowSearchCriteria.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbShowSearchCriteria.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbShowSearchCriteria.Image = Global.SchedulingBoard.My.Resources.Resources.mpg24
        Me.tsbShowSearchCriteria.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbShowSearchCriteria.Name = "tsbShowSearchCriteria"
        Me.tsbShowSearchCriteria.Size = New System.Drawing.Size(28, 28)
        Me.tsbShowSearchCriteria.Text = "Show Multi-Part Groups"
        '
        'tsbRepairLog
        '
        Me.tsbRepairLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbRepairLog.Image = Global.SchedulingBoard.My.Resources.Resources.repair321
        Me.tsbRepairLog.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbRepairLog.Name = "tsbRepairLog"
        Me.tsbRepairLog.Size = New System.Drawing.Size(28, 28)
        Me.tsbRepairLog.Tag = "Repair"
        Me.tsbRepairLog.Text = "Repair Log"
        '
        'tsbMode
        '
        Me.tsbMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbMode.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CrewModeToolStripMenuItem, Me.EquipmentModeToolStripMenuItem})
        Me.tsbMode.Image = CType(resources.GetObject("tsbMode.Image"), System.Drawing.Image)
        Me.tsbMode.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbMode.Name = "tsbMode"
        Me.tsbMode.Size = New System.Drawing.Size(31, 42)
        Me.tsbMode.Tag = "Crew Tools"
        Me.tsbMode.Text = "Mode"
        Me.tsbMode.TextDirection = System.Windows.Forms.ToolStripTextDirection.Vertical90
        '
        'CrewModeToolStripMenuItem
        '
        Me.CrewModeToolStripMenuItem.Name = "CrewModeToolStripMenuItem"
        Me.CrewModeToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.CrewModeToolStripMenuItem.Text = "Crew Mode"
        '
        'EquipmentModeToolStripMenuItem
        '
        Me.EquipmentModeToolStripMenuItem.Name = "EquipmentModeToolStripMenuItem"
        Me.EquipmentModeToolStripMenuItem.Size = New System.Drawing.Size(166, 22)
        Me.EquipmentModeToolStripMenuItem.Text = "Equipment Mode"
        Me.EquipmentModeToolStripMenuItem.Visible = False
        '
        'dgvDetail
        '
        Me.dgvDetail.AllowUserToAddRows = False
        Me.dgvDetail.AllowUserToDeleteRows = False
        Me.dgvDetail.AllowUserToResizeColumns = False
        Me.dgvDetail.AllowUserToResizeRows = False
        Me.dgvDetail.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvDetail.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvDetail.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.dgvDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvDetail.ContextMenuStrip = Me.cmsPartSchedulingDetails
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvDetail.DefaultCellStyle = DataGridViewCellStyle2
        Me.dgvDetail.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvDetail.Location = New System.Drawing.Point(0, 32)
        Me.dgvDetail.MultiSelect = False
        Me.dgvDetail.Name = "dgvDetail"
        Me.dgvDetail.ReadOnly = True
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvDetail.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.dgvDetail.RowHeadersVisible = False
        Me.dgvDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvDetail.Size = New System.Drawing.Size(1303, 140)
        Me.dgvDetail.TabIndex = 1
        '
        'cmsPartSchedulingDetails
        '
        Me.cmsPartSchedulingDetails.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.cmsPartSchedulingDetails.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.csmiPartSchedulingDetailCopyPhaseNumber, Me.csmiPartSchedulingDetailCopyPhaseDesc, Me.csmiPartSchedulingDetailCopyPhaseNumberDesc, Me.ToolStripSeparator7, Me.csmiPartSchedulingDetailSubstitutePart, Me.CreateJustInTimeTransferToolStripMenuItem})
        Me.cmsPartSchedulingDetails.Name = "cmsPartSchedulingDetails"
        Me.cmsPartSchedulingDetails.Size = New System.Drawing.Size(255, 142)
        '
        'csmiPartSchedulingDetailCopyPhaseNumber
        '
        Me.csmiPartSchedulingDetailCopyPhaseNumber.Name = "csmiPartSchedulingDetailCopyPhaseNumber"
        Me.csmiPartSchedulingDetailCopyPhaseNumber.Size = New System.Drawing.Size(254, 22)
        Me.csmiPartSchedulingDetailCopyPhaseNumber.Text = "Copy Phase Number"
        '
        'csmiPartSchedulingDetailCopyPhaseDesc
        '
        Me.csmiPartSchedulingDetailCopyPhaseDesc.Name = "csmiPartSchedulingDetailCopyPhaseDesc"
        Me.csmiPartSchedulingDetailCopyPhaseDesc.Size = New System.Drawing.Size(254, 22)
        Me.csmiPartSchedulingDetailCopyPhaseDesc.Text = "Copy Phase Description"
        '
        'csmiPartSchedulingDetailCopyPhaseNumberDesc
        '
        Me.csmiPartSchedulingDetailCopyPhaseNumberDesc.Name = "csmiPartSchedulingDetailCopyPhaseNumberDesc"
        Me.csmiPartSchedulingDetailCopyPhaseNumberDesc.Size = New System.Drawing.Size(254, 22)
        Me.csmiPartSchedulingDetailCopyPhaseNumberDesc.Text = "Copy Phase Number (Description)"
        '
        'ToolStripSeparator7
        '
        Me.ToolStripSeparator7.Name = "ToolStripSeparator7"
        Me.ToolStripSeparator7.Size = New System.Drawing.Size(251, 6)
        '
        'csmiPartSchedulingDetailSubstitutePart
        '
        Me.csmiPartSchedulingDetailSubstitutePart.Name = "csmiPartSchedulingDetailSubstitutePart"
        Me.csmiPartSchedulingDetailSubstitutePart.Size = New System.Drawing.Size(254, 22)
        Me.csmiPartSchedulingDetailSubstitutePart.Text = "Substitute Part"
        '
        'pnlDetailControls
        '
        Me.pnlDetailControls.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlDetailControls.Controls.Add(Me.lblPopOut)
        Me.pnlDetailControls.Controls.Add(Me.chkGroupByParentProject)
        Me.pnlDetailControls.Controls.Add(Me.lblPartSchedulingDetail)
        Me.pnlDetailControls.Controls.Add(Me.cmdDetailToExcel)
        Me.pnlDetailControls.Controls.Add(Me.lblCloseDetailPane)
        Me.pnlDetailControls.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlDetailControls.Location = New System.Drawing.Point(0, 0)
        Me.pnlDetailControls.Name = "pnlDetailControls"
        Me.pnlDetailControls.Size = New System.Drawing.Size(1303, 32)
        Me.pnlDetailControls.TabIndex = 0
        '
        'lblPopOut
        '
        Me.lblPopOut.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblPopOut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblPopOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPopOut.Location = New System.Drawing.Point(1195, 3)
        Me.lblPopOut.Margin = New System.Windows.Forms.Padding(0)
        Me.lblPopOut.Name = "lblPopOut"
        Me.lblPopOut.Size = New System.Drawing.Size(73, 24)
        Me.lblPopOut.TabIndex = 5
        Me.lblPopOut.Text = "□ Pop Out"
        Me.lblPopOut.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'chkGroupByParentProject
        '
        Me.chkGroupByParentProject.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkGroupByParentProject.AutoSize = True
        Me.chkGroupByParentProject.Checked = True
        Me.chkGroupByParentProject.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkGroupByParentProject.Location = New System.Drawing.Point(1035, 7)
        Me.chkGroupByParentProject.Name = "chkGroupByParentProject"
        Me.chkGroupByParentProject.Size = New System.Drawing.Size(139, 17)
        Me.chkGroupByParentProject.TabIndex = 4
        Me.chkGroupByParentProject.Text = "Group by Parent Project"
        Me.chkGroupByParentProject.UseVisualStyleBackColor = True
        '
        'lblPartSchedulingDetail
        '
        Me.lblPartSchedulingDetail.AutoSize = True
        Me.lblPartSchedulingDetail.Font = New System.Drawing.Font("Arial", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPartSchedulingDetail.Location = New System.Drawing.Point(38, 8)
        Me.lblPartSchedulingDetail.Name = "lblPartSchedulingDetail"
        Me.lblPartSchedulingDetail.Size = New System.Drawing.Size(151, 16)
        Me.lblPartSchedulingDetail.TabIndex = 3
        Me.lblPartSchedulingDetail.Text = "Part Scheduling Detail"
        '
        'cmdDetailToExcel
        '
        Me.cmdDetailToExcel.Image = Global.SchedulingBoard.My.Resources.Resources.excel24
        Me.cmdDetailToExcel.Location = New System.Drawing.Point(3, 0)
        Me.cmdDetailToExcel.Name = "cmdDetailToExcel"
        Me.cmdDetailToExcel.Size = New System.Drawing.Size(29, 30)
        Me.cmdDetailToExcel.TabIndex = 1
        Me.cmdDetailToExcel.UseVisualStyleBackColor = True
        '
        'lblCloseDetailPane
        '
        Me.lblCloseDetailPane.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCloseDetailPane.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCloseDetailPane.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCloseDetailPane.Location = New System.Drawing.Point(1270, 3)
        Me.lblCloseDetailPane.Margin = New System.Windows.Forms.Padding(0)
        Me.lblCloseDetailPane.Name = "lblCloseDetailPane"
        Me.lblCloseDetailPane.Size = New System.Drawing.Size(24, 24)
        Me.lblCloseDetailPane.TabIndex = 0
        Me.lblCloseDetailPane.Text = "X"
        Me.lblCloseDetailPane.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'pnlTotalsAndWarehouses
        '
        Me.pnlTotalsAndWarehouses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlTotalsAndWarehouses.Controls.Add(Me.scWarehouses)
        Me.pnlTotalsAndWarehouses.Controls.Add(Me.pnlTotals)
        Me.pnlTotalsAndWarehouses.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlTotalsAndWarehouses.Location = New System.Drawing.Point(0, 38)
        Me.pnlTotalsAndWarehouses.Name = "pnlTotalsAndWarehouses"
        Me.pnlTotalsAndWarehouses.Size = New System.Drawing.Size(1303, 98)
        Me.pnlTotalsAndWarehouses.TabIndex = 16
        '
        'scWarehouses
        '
        Me.scWarehouses.Dock = System.Windows.Forms.DockStyle.Fill
        Me.scWarehouses.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.scWarehouses.Location = New System.Drawing.Point(103, 0)
        Me.scWarehouses.Name = "scWarehouses"
        '
        'scWarehouses.Panel1
        '
        Me.scWarehouses.Panel1.Controls.Add(Me.pnlWarehouses)
        '
        'scWarehouses.Panel2
        '
        Me.scWarehouses.Panel2.Controls.Add(Me.lvWarehouseGroups)
        Me.scWarehouses.Size = New System.Drawing.Size(1198, 96)
        Me.scWarehouses.SplitterDistance = 939
        Me.scWarehouses.TabIndex = 18
        '
        'pnlWarehouses
        '
        Me.pnlWarehouses.AutoScroll = True
        Me.pnlWarehouses.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlWarehouses.ContextMenuStrip = Me.mnuWarehouse
        Me.pnlWarehouses.Controls.Add(Me.pnlWarehouse)
        Me.pnlWarehouses.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlWarehouses.Location = New System.Drawing.Point(0, 0)
        Me.pnlWarehouses.Name = "pnlWarehouses"
        Me.pnlWarehouses.Size = New System.Drawing.Size(939, 96)
        Me.pnlWarehouses.TabIndex = 12
        '
        'mnuWarehouse
        '
        Me.mnuWarehouse.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.mnuWarehouse.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RunRefreshToolStripMenuItem, Me.ShowAllToolStripMenuItem})
        Me.mnuWarehouse.Name = "mnuWarehouse"
        Me.mnuWarehouse.Size = New System.Drawing.Size(161, 48)
        '
        'RunRefreshToolStripMenuItem
        '
        Me.RunRefreshToolStripMenuItem.Name = "RunRefreshToolStripMenuItem"
        Me.RunRefreshToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.RunRefreshToolStripMenuItem.Text = "Hide Unselected"
        '
        'ShowAllToolStripMenuItem
        '
        Me.ShowAllToolStripMenuItem.Name = "ShowAllToolStripMenuItem"
        Me.ShowAllToolStripMenuItem.Size = New System.Drawing.Size(160, 22)
        Me.ShowAllToolStripMenuItem.Text = "Show All"
        '
        'pnlWarehouse
        '
        Me.pnlWarehouse.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlWarehouse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlWarehouse.Controls.Add(Me.lblRepairQty)
        Me.pnlWarehouse.Controls.Add(Me.lblPlannedOrder)
        Me.pnlWarehouse.Controls.Add(Me.lblLateQty)
        Me.pnlWarehouse.Controls.Add(Me.lblWarehouseDesc)
        Me.pnlWarehouse.Controls.Add(Me.chkToggleWarehouse)
        Me.pnlWarehouse.Location = New System.Drawing.Point(694, 0)
        Me.pnlWarehouse.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlWarehouse.Name = "pnlWarehouse"
        Me.pnlWarehouse.Size = New System.Drawing.Size(90, 78)
        Me.pnlWarehouse.TabIndex = 0
        Me.pnlWarehouse.Visible = False
        '
        'lblRepairQty
        '
        Me.lblRepairQty.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblRepairQty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRepairQty.ForeColor = System.Drawing.Color.Purple
        Me.lblRepairQty.Image = Global.SchedulingBoard.My.Resources.Resources.repair16
        Me.lblRepairQty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblRepairQty.Location = New System.Drawing.Point(0, 76)
        Me.lblRepairQty.Name = "lblRepairQty"
        Me.lblRepairQty.Padding = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblRepairQty.Size = New System.Drawing.Size(88, 18)
        Me.lblRepairQty.TabIndex = 1
        Me.lblRepairQty.Text = "Qty"
        Me.lblRepairQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblRepairQty.Visible = False
        '
        'lblPlannedOrder
        '
        Me.lblPlannedOrder.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblPlannedOrder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPlannedOrder.ForeColor = System.Drawing.Color.Green
        Me.lblPlannedOrder.Image = Global.SchedulingBoard.My.Resources.Resources.PlannedOrder16
        Me.lblPlannedOrder.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblPlannedOrder.Location = New System.Drawing.Point(0, 58)
        Me.lblPlannedOrder.Name = "lblPlannedOrder"
        Me.lblPlannedOrder.Padding = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblPlannedOrder.Size = New System.Drawing.Size(88, 18)
        Me.lblPlannedOrder.TabIndex = 5
        Me.lblPlannedOrder.Text = "PO"
        Me.lblPlannedOrder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblPlannedOrder.Visible = False
        '
        'lblLateQty
        '
        Me.lblLateQty.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblLateQty.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblLateQty.ForeColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.lblLateQty.Image = Global.SchedulingBoard.My.Resources.Resources.overdue16
        Me.lblLateQty.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.lblLateQty.Location = New System.Drawing.Point(0, 40)
        Me.lblLateQty.Name = "lblLateQty"
        Me.lblLateQty.Padding = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.lblLateQty.Size = New System.Drawing.Size(88, 18)
        Me.lblLateQty.TabIndex = 2
        Me.lblLateQty.Text = "Qty"
        Me.lblLateQty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.lblLateQty.Visible = False
        '
        'lblWarehouseDesc
        '
        Me.lblWarehouseDesc.Dock = System.Windows.Forms.DockStyle.Top
        Me.lblWarehouseDesc.Font = New System.Drawing.Font("Arial Narrow", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblWarehouseDesc.Location = New System.Drawing.Point(0, 24)
        Me.lblWarehouseDesc.Name = "lblWarehouseDesc"
        Me.lblWarehouseDesc.Size = New System.Drawing.Size(88, 16)
        Me.lblWarehouseDesc.TabIndex = 3
        Me.lblWarehouseDesc.Text = "Warehouse"
        Me.lblWarehouseDesc.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblWarehouseDesc.Visible = False
        '
        'chkToggleWarehouse
        '
        Me.chkToggleWarehouse.Appearance = System.Windows.Forms.Appearance.Button
        Me.chkToggleWarehouse.Dock = System.Windows.Forms.DockStyle.Top
        Me.chkToggleWarehouse.Font = New System.Drawing.Font("Arial Narrow", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkToggleWarehouse.Location = New System.Drawing.Point(0, 0)
        Me.chkToggleWarehouse.Margin = New System.Windows.Forms.Padding(1, 3, 1, 3)
        Me.chkToggleWarehouse.Name = "chkToggleWarehouse"
        Me.chkToggleWarehouse.Size = New System.Drawing.Size(88, 24)
        Me.chkToggleWarehouse.TabIndex = 4
        Me.chkToggleWarehouse.Text = "WH (Total)"
        Me.chkToggleWarehouse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.chkToggleWarehouse.UseVisualStyleBackColor = True
        Me.chkToggleWarehouse.Visible = False
        '
        'lvWarehouseGroups
        '
        Me.lvWarehouseGroups.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.lvcWarehouseGroup})
        Me.lvWarehouseGroups.ContextMenuStrip = Me.cmsWarehouseGroups
        Me.lvWarehouseGroups.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lvWarehouseGroups.HideSelection = False
        Me.lvWarehouseGroups.LabelEdit = True
        Me.lvWarehouseGroups.Location = New System.Drawing.Point(0, 0)
        Me.lvWarehouseGroups.MultiSelect = False
        Me.lvWarehouseGroups.Name = "lvWarehouseGroups"
        Me.lvWarehouseGroups.Size = New System.Drawing.Size(255, 96)
        Me.lvWarehouseGroups.TabIndex = 0
        Me.lvWarehouseGroups.UseCompatibleStateImageBehavior = False
        Me.lvWarehouseGroups.View = System.Windows.Forms.View.Details
        '
        'lvcWarehouseGroup
        '
        Me.lvcWarehouseGroup.Text = "Warehouse Group"
        '
        'cmsWarehouseGroups
        '
        Me.cmsWarehouseGroups.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.cmsWarehouseGroups.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmiRenameWarehouseGroup, Me.ToolStripSeparator5, Me.tsmiOverwriteWarehoseGroup, Me.ToolStripSeparator1, Me.tsmiCreateNewWarehouseGroup, Me.ToolStripSeparator4, Me.tsmiDeleteWarehouseGroup, Me.ToolStripSeparator6, Me.tsmiSaveGroups})
        Me.cmsWarehouseGroups.Name = "cmsWarehouseGroups"
        Me.cmsWarehouseGroups.Size = New System.Drawing.Size(234, 138)
        '
        'tsmiRenameWarehouseGroup
        '
        Me.tsmiRenameWarehouseGroup.Name = "tsmiRenameWarehouseGroup"
        Me.tsmiRenameWarehouseGroup.Size = New System.Drawing.Size(233, 22)
        Me.tsmiRenameWarehouseGroup.Text = "Rename Warehouse Group"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(230, 6)
        '
        'tsmiOverwriteWarehoseGroup
        '
        Me.tsmiOverwriteWarehoseGroup.Name = "tsmiOverwriteWarehoseGroup"
        Me.tsmiOverwriteWarehoseGroup.Size = New System.Drawing.Size(233, 22)
        Me.tsmiOverwriteWarehoseGroup.Text = "Overwrite Warehose Group"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(230, 6)
        '
        'tsmiCreateNewWarehouseGroup
        '
        Me.tsmiCreateNewWarehouseGroup.Name = "tsmiCreateNewWarehouseGroup"
        Me.tsmiCreateNewWarehouseGroup.Size = New System.Drawing.Size(233, 22)
        Me.tsmiCreateNewWarehouseGroup.Text = "Create New Warehouse Group"
        '
        'ToolStripSeparator4
        '
        Me.ToolStripSeparator4.Name = "ToolStripSeparator4"
        Me.ToolStripSeparator4.Size = New System.Drawing.Size(230, 6)
        '
        'tsmiDeleteWarehouseGroup
        '
        Me.tsmiDeleteWarehouseGroup.Name = "tsmiDeleteWarehouseGroup"
        Me.tsmiDeleteWarehouseGroup.Size = New System.Drawing.Size(233, 22)
        Me.tsmiDeleteWarehouseGroup.Text = "Delete Warehouse Group"
        '
        'ToolStripSeparator6
        '
        Me.ToolStripSeparator6.Name = "ToolStripSeparator6"
        Me.ToolStripSeparator6.Size = New System.Drawing.Size(230, 6)
        '
        'tsmiSaveGroups
        '
        Me.tsmiSaveGroups.Name = "tsmiSaveGroups"
        Me.tsmiSaveGroups.Size = New System.Drawing.Size(233, 22)
        Me.tsmiSaveGroups.Text = "Save Groups"
        '
        'pnlTotals
        '
        Me.pnlTotals.Controls.Add(Me.lblCollapseExpandWarehouses)
        Me.pnlTotals.Controls.Add(Me.lblInventory)
        Me.pnlTotals.Controls.Add(Me.lblExclSubWexler)
        Me.pnlTotals.Controls.Add(Me.lblChildren)
        Me.pnlTotals.Controls.Add(Me.lblTotal)
        Me.pnlTotals.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlTotals.Location = New System.Drawing.Point(0, 0)
        Me.pnlTotals.Name = "pnlTotals"
        Me.pnlTotals.Size = New System.Drawing.Size(103, 96)
        Me.pnlTotals.TabIndex = 17
        '
        'lblCollapseExpandWarehouses
        '
        Me.lblCollapseExpandWarehouses.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCollapseExpandWarehouses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCollapseExpandWarehouses.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCollapseExpandWarehouses.Location = New System.Drawing.Point(5, 3)
        Me.lblCollapseExpandWarehouses.Margin = New System.Windows.Forms.Padding(0)
        Me.lblCollapseExpandWarehouses.Name = "lblCollapseExpandWarehouses"
        Me.lblCollapseExpandWarehouses.Size = New System.Drawing.Size(16, 16)
        Me.lblCollapseExpandWarehouses.TabIndex = 4
        Me.lblCollapseExpandWarehouses.Tag = "□"
        Me.lblCollapseExpandWarehouses.Text = "-"
        Me.lblCollapseExpandWarehouses.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblInventory
        '
        Me.lblInventory.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInventory.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInventory.Location = New System.Drawing.Point(12, 36)
        Me.lblInventory.Name = "lblInventory"
        Me.lblInventory.Size = New System.Drawing.Size(89, 21)
        Me.lblInventory.TabIndex = 3
        Me.lblInventory.Text = "Inventory:"
        Me.lblInventory.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblExclSubWexler
        '
        Me.lblExclSubWexler.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblExclSubWexler.Font = New System.Drawing.Font("Arial", 6.0!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblExclSubWexler.ForeColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblExclSubWexler.Location = New System.Drawing.Point(3, 19)
        Me.lblExclSubWexler.Name = "lblExclSubWexler"
        Me.lblExclSubWexler.Size = New System.Drawing.Size(96, 10)
        Me.lblExclSubWexler.TabIndex = 2
        Me.lblExclSubWexler.Text = "(excl. SUB)"
        Me.lblExclSubWexler.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblChildren
        '
        Me.lblChildren.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblChildren.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblChildren.Location = New System.Drawing.Point(10, 58)
        Me.lblChildren.Name = "lblChildren"
        Me.lblChildren.Size = New System.Drawing.Size(89, 21)
        Me.lblChildren.TabIndex = 1
        Me.lblChildren.Text = "Children:"
        Me.lblChildren.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(10, 3)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(89, 21)
        Me.lblTotal.TabIndex = 0
        Me.lblTotal.Text = "Total:"
        Me.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlAddPartToOrder
        '
        Me.pnlAddPartToOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlAddPartToOrder.Controls.Add(Me.cmdAddTransfer)
        Me.pnlAddPartToOrder.Controls.Add(Me.chkForToday)
        Me.pnlAddPartToOrder.Controls.Add(Me.lblCloseAddPartToOrder)
        Me.pnlAddPartToOrder.Controls.Add(Me.lblNote)
        Me.pnlAddPartToOrder.Controls.Add(Me.txtNote)
        Me.pnlAddPartToOrder.Controls.Add(Me.cmdOpenProject)
        Me.pnlAddPartToOrder.Controls.Add(Me.cmdProjectSearch)
        Me.pnlAddPartToOrder.Controls.Add(Me.txtProjectDesc)
        Me.pnlAddPartToOrder.Controls.Add(Me.txtProjectNo)
        Me.pnlAddPartToOrder.Controls.Add(Me.cmdAddPartToOrder)
        Me.pnlAddPartToOrder.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlAddPartToOrder.Location = New System.Drawing.Point(0, 675)
        Me.pnlAddPartToOrder.Name = "pnlAddPartToOrder"
        Me.pnlAddPartToOrder.Size = New System.Drawing.Size(1303, 40)
        Me.pnlAddPartToOrder.TabIndex = 13
        '
        'cmdAddTransfer
        '
        Me.cmdAddTransfer.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cmdAddTransfer.Image = Global.SchedulingBoard.My.Resources.Resources.Add_New_Row
        Me.cmdAddTransfer.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdAddTransfer.Location = New System.Drawing.Point(11, 7)
        Me.cmdAddTransfer.Name = "cmdAddTransfer"
        Me.cmdAddTransfer.Size = New System.Drawing.Size(92, 23)
        Me.cmdAddTransfer.TabIndex = 9
        Me.cmdAddTransfer.Text = "Add Transfer"
        Me.cmdAddTransfer.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdAddTransfer.UseVisualStyleBackColor = True
        '
        'chkForToday
        '
        Me.chkForToday.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.chkForToday.AutoSize = True
        Me.chkForToday.Location = New System.Drawing.Point(1134, 13)
        Me.chkForToday.Name = "chkForToday"
        Me.chkForToday.Size = New System.Drawing.Size(74, 17)
        Me.chkForToday.TabIndex = 8
        Me.chkForToday.Text = "For Today"
        Me.chkForToday.UseVisualStyleBackColor = True
        '
        'lblCloseAddPartToOrder
        '
        Me.lblCloseAddPartToOrder.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCloseAddPartToOrder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblCloseAddPartToOrder.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCloseAddPartToOrder.Location = New System.Drawing.Point(1272, 6)
        Me.lblCloseAddPartToOrder.Margin = New System.Windows.Forms.Padding(0)
        Me.lblCloseAddPartToOrder.Name = "lblCloseAddPartToOrder"
        Me.lblCloseAddPartToOrder.Size = New System.Drawing.Size(24, 24)
        Me.lblCloseAddPartToOrder.TabIndex = 7
        Me.lblCloseAddPartToOrder.Text = "X"
        Me.lblCloseAddPartToOrder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblNote
        '
        Me.lblNote.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.lblNote.AutoSize = True
        Me.lblNote.Location = New System.Drawing.Point(893, 14)
        Me.lblNote.Name = "lblNote"
        Me.lblNote.Size = New System.Drawing.Size(33, 13)
        Me.lblNote.TabIndex = 6
        Me.lblNote.Text = "Note:"
        '
        'cmdOpenProject
        '
        Me.cmdOpenProject.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.cmdOpenProject.Image = Global.SchedulingBoard.My.Resources.Resources.ProjectMaintenance24
        Me.cmdOpenProject.Location = New System.Drawing.Point(809, 7)
        Me.cmdOpenProject.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.cmdOpenProject.Name = "cmdOpenProject"
        Me.cmdOpenProject.Size = New System.Drawing.Size(73, 26)
        Me.cmdOpenProject.TabIndex = 4
        Me.cmdOpenProject.Text = "Open"
        Me.cmdOpenProject.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cmdOpenProject.UseVisualStyleBackColor = True
        '
        'cmdProjectSearch
        '
        Me.cmdProjectSearch.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.cmdProjectSearch.Location = New System.Drawing.Point(361, 7)
        Me.cmdProjectSearch.Name = "cmdProjectSearch"
        Me.cmdProjectSearch.Size = New System.Drawing.Size(66, 26)
        Me.cmdProjectSearch.TabIndex = 3
        Me.cmdProjectSearch.Text = "Project #"
        Me.cmdProjectSearch.UseVisualStyleBackColor = True
        '
        'cmdAddPartToOrder
        '
        Me.cmdAddPartToOrder.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.cmdAddPartToOrder.Location = New System.Drawing.Point(243, 7)
        Me.cmdAddPartToOrder.Name = "cmdAddPartToOrder"
        Me.cmdAddPartToOrder.Size = New System.Drawing.Size(102, 26)
        Me.cmdAddPartToOrder.TabIndex = 0
        Me.cmdAddPartToOrder.Text = "Add Part to Order"
        Me.cmdAddPartToOrder.UseVisualStyleBackColor = True
        '
        'pnlViewOptions
        '
        Me.pnlViewOptions.Controls.Add(Me.pnlSearchForPart)
        Me.pnlViewOptions.Controls.Add(Me.pnlControls)
        Me.pnlViewOptions.Controls.Add(Me.pnlRefresh)
        Me.pnlViewOptions.Controls.Add(Me.pnlExcelLame)
        Me.pnlViewOptions.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlViewOptions.Location = New System.Drawing.Point(0, 0)
        Me.pnlViewOptions.Name = "pnlViewOptions"
        Me.pnlViewOptions.Size = New System.Drawing.Size(1303, 38)
        Me.pnlViewOptions.TabIndex = 6
        '
        'pnlSearchForPart
        '
        Me.pnlSearchForPart.Controls.Add(Me.BtnForward)
        Me.pnlSearchForPart.Controls.Add(Me.cmdSearchByBarcode)
        Me.pnlSearchForPart.Controls.Add(Me.BtnBack)
        Me.pnlSearchForPart.Controls.Add(Me.cmdPartSearch)
        Me.pnlSearchForPart.Controls.Add(Me.txtPartDesc)
        Me.pnlSearchForPart.Controls.Add(Me.txtPartNo)
        Me.pnlSearchForPart.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlSearchForPart.Location = New System.Drawing.Point(103, 0)
        Me.pnlSearchForPart.Name = "pnlSearchForPart"
        Me.pnlSearchForPart.Size = New System.Drawing.Size(604, 38)
        Me.pnlSearchForPart.TabIndex = 19
        '
        'BtnForward
        '
        Me.BtnForward.Location = New System.Drawing.Point(71, 7)
        Me.BtnForward.Name = "BtnForward"
        Me.BtnForward.Size = New System.Drawing.Size(64, 23)
        Me.BtnForward.TabIndex = 22
        Me.BtnForward.Text = "Forward"
        Me.BtnForward.UseVisualStyleBackColor = True
        '
        'cmdSearchByBarcode
        '
        Me.cmdSearchByBarcode.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSearchByBarcode.Image = Global.SchedulingBoard.My.Resources.Resources.barcode241
        Me.cmdSearchByBarcode.ImageAlign = System.Drawing.ContentAlignment.TopLeft
        Me.cmdSearchByBarcode.Location = New System.Drawing.Point(461, 6)
        Me.cmdSearchByBarcode.Name = "cmdSearchByBarcode"
        Me.cmdSearchByBarcode.Size = New System.Drawing.Size(131, 28)
        Me.cmdSearchByBarcode.TabIndex = 7
        Me.cmdSearchByBarcode.Text = "Search by Barcode"
        Me.cmdSearchByBarcode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cmdSearchByBarcode.UseVisualStyleBackColor = True
        '
        'BtnBack
        '
        Me.BtnBack.Location = New System.Drawing.Point(6, 7)
        Me.BtnBack.Name = "BtnBack"
        Me.BtnBack.Size = New System.Drawing.Size(59, 23)
        Me.BtnBack.TabIndex = 21
        Me.BtnBack.Text = "Back"
        Me.BtnBack.UseVisualStyleBackColor = True
        '
        'cmdPartSearch
        '
        Me.cmdPartSearch.Location = New System.Drawing.Point(141, 5)
        Me.cmdPartSearch.Name = "cmdPartSearch"
        Me.cmdPartSearch.Size = New System.Drawing.Size(50, 28)
        Me.cmdPartSearch.TabIndex = 6
        Me.cmdPartSearch.Text = "Part #"
        Me.cmdPartSearch.UseVisualStyleBackColor = True
        '
        'pnlControls
        '
        Me.pnlControls.Controls.Add(Me.chkLockDates)
        Me.pnlControls.Controls.Add(Me.chkSummarize)
        Me.pnlControls.Controls.Add(Me.lblStartDate)
        Me.pnlControls.Controls.Add(Me.dtpEndDate)
        Me.pnlControls.Controls.Add(Me.dtpStartDate)
        Me.pnlControls.Controls.Add(Me.lblEndDate)
        Me.pnlControls.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlControls.Location = New System.Drawing.Point(707, 0)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Size = New System.Drawing.Size(402, 38)
        Me.pnlControls.TabIndex = 12
        '
        'chkLockDates
        '
        Me.chkLockDates.AutoSize = True
        Me.chkLockDates.Location = New System.Drawing.Point(289, 18)
        Me.chkLockDates.Name = "chkLockDates"
        Me.chkLockDates.Size = New System.Drawing.Size(81, 17)
        Me.chkLockDates.TabIndex = 11
        Me.chkLockDates.Text = "Lock Dates"
        Me.chkLockDates.UseVisualStyleBackColor = True
        '
        'chkSummarize
        '
        Me.chkSummarize.AutoSize = True
        Me.chkSummarize.Location = New System.Drawing.Point(289, 3)
        Me.chkSummarize.Name = "chkSummarize"
        Me.chkSummarize.Size = New System.Drawing.Size(115, 17)
        Me.chkSummarize.TabIndex = 10
        Me.chkSummarize.Text = "Summarize Results"
        Me.chkSummarize.UseVisualStyleBackColor = True
        '
        'lblStartDate
        '
        Me.lblStartDate.Location = New System.Drawing.Point(3, 5)
        Me.lblStartDate.Name = "lblStartDate"
        Me.lblStartDate.Size = New System.Drawing.Size(38, 27)
        Me.lblStartDate.TabIndex = 0
        Me.lblStartDate.Text = "Start Date:"
        Me.lblStartDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'dtpEndDate
        '
        Me.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpEndDate.Location = New System.Drawing.Point(185, 8)
        Me.dtpEndDate.Name = "dtpEndDate"
        Me.dtpEndDate.Size = New System.Drawing.Size(97, 20)
        Me.dtpEndDate.TabIndex = 3
        '
        'dtpStartDate
        '
        Me.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpStartDate.Location = New System.Drawing.Point(44, 8)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Size = New System.Drawing.Size(97, 20)
        Me.dtpStartDate.TabIndex = 2
        '
        'lblEndDate
        '
        Me.lblEndDate.Location = New System.Drawing.Point(130, 5)
        Me.lblEndDate.Name = "lblEndDate"
        Me.lblEndDate.Size = New System.Drawing.Size(52, 27)
        Me.lblEndDate.TabIndex = 1
        Me.lblEndDate.Text = "End Date:"
        Me.lblEndDate.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'pnlRefresh
        '
        Me.pnlRefresh.Controls.Add(Me.btnRefresh)
        Me.pnlRefresh.Dock = System.Windows.Forms.DockStyle.Left
        Me.pnlRefresh.Location = New System.Drawing.Point(0, 0)
        Me.pnlRefresh.Name = "pnlRefresh"
        Me.pnlRefresh.Size = New System.Drawing.Size(103, 38)
        Me.pnlRefresh.TabIndex = 20
        '
        'btnRefresh
        '
        Me.btnRefresh.Image = Global.SchedulingBoard.My.Resources.Resources.refresh2
        Me.btnRefresh.Location = New System.Drawing.Point(4, 4)
        Me.btnRefresh.Name = "btnRefresh"
        Me.btnRefresh.Size = New System.Drawing.Size(95, 30)
        Me.btnRefresh.TabIndex = 8
        Me.btnRefresh.Text = "Refresh (F5)"
        Me.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnRefresh.UseVisualStyleBackColor = True
        '
        'pnlExcelLame
        '
        Me.pnlExcelLame.Controls.Add(Me.cmdHelp)
        Me.pnlExcelLame.Controls.Add(Me.cmdExportToExcel)
        Me.pnlExcelLame.Controls.Add(Me.btnLameLink)
        Me.pnlExcelLame.Dock = System.Windows.Forms.DockStyle.Right
        Me.pnlExcelLame.Location = New System.Drawing.Point(1109, 0)
        Me.pnlExcelLame.Name = "pnlExcelLame"
        Me.pnlExcelLame.Size = New System.Drawing.Size(194, 38)
        Me.pnlExcelLame.TabIndex = 21
        '
        'cmdHelp
        '
        Me.cmdHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdHelp.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.cmdHelp.Image = CType(resources.GetObject("cmdHelp.Image"), System.Drawing.Image)
        Me.cmdHelp.Location = New System.Drawing.Point(133, 5)
        Me.cmdHelp.Name = "cmdHelp"
        Me.cmdHelp.Size = New System.Drawing.Size(55, 30)
        Me.cmdHelp.TabIndex = 12
        Me.cmdHelp.Text = "Help"
        Me.cmdHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cmdHelp.UseVisualStyleBackColor = True
        '
        'cmdExportToExcel
        '
        Me.cmdExportToExcel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExportToExcel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.cmdExportToExcel.Image = Global.SchedulingBoard.My.Resources.Resources.excel24
        Me.cmdExportToExcel.Location = New System.Drawing.Point(3, 4)
        Me.cmdExportToExcel.Name = "cmdExportToExcel"
        Me.cmdExportToExcel.Size = New System.Drawing.Size(75, 30)
        Me.cmdExportToExcel.TabIndex = 9
        Me.cmdExportToExcel.Text = "Export"
        Me.cmdExportToExcel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cmdExportToExcel.UseVisualStyleBackColor = True
        '
        'btnLameLink
        '
        Me.btnLameLink.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnLameLink.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.btnLameLink.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control
        Me.btnLameLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.btnLameLink.Location = New System.Drawing.Point(81, 4)
        Me.btnLameLink.Name = "btnLameLink"
        Me.btnLameLink.Size = New System.Drawing.Size(51, 30)
        Me.btnLameLink.TabIndex = 11
        Me.btnLameLink.Text = "LAME!"
        Me.btnLameLink.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.btnLameLink.UseVisualStyleBackColor = True
        '
        'timer_DoubleClickInterval
        '
        '
        'timer_RefreshData
        '
        Me.timer_RefreshData.Interval = 750
        '
        'chkUnavailabilityCutoffDate
        '
        Me.chkUnavailabilityCutoffDate.AutoSize = True
        Me.chkUnavailabilityCutoffDate.Checked = True
        Me.chkUnavailabilityCutoffDate.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkUnavailabilityCutoffDate.Location = New System.Drawing.Point(7, 35)
        Me.chkUnavailabilityCutoffDate.Name = "chkUnavailabilityCutoffDate"
        Me.chkUnavailabilityCutoffDate.Size = New System.Drawing.Size(54, 17)
        Me.chkUnavailabilityCutoffDate.TabIndex = 11
        Me.chkUnavailabilityCutoffDate.Text = "Cutoff"
        Me.chkUnavailabilityCutoffDate.UseVisualStyleBackColor = True
        '
        'dtpUnavailabilityCutoff
        '
        Me.dtpUnavailabilityCutoff.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpUnavailabilityCutoff.Location = New System.Drawing.Point(67, 33)
        Me.dtpUnavailabilityCutoff.Name = "dtpUnavailabilityCutoff"
        Me.dtpUnavailabilityCutoff.Size = New System.Drawing.Size(84, 20)
        Me.dtpUnavailabilityCutoff.TabIndex = 12
        '
        'txtUnavailabilityProjectNo
        '
        Me.txtUnavailabilityProjectNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtUnavailabilityProjectNo.Location = New System.Drawing.Point(125, 5)
        Me.txtUnavailabilityProjectNo.Name = "txtUnavailabilityProjectNo"
        Me.txtUnavailabilityProjectNo.Size = New System.Drawing.Size(126, 20)
        Me.txtUnavailabilityProjectNo.TabIndex = 1
        Me.txtUnavailabilityProjectNo.TextToClear = Nothing
        '
        'txtNote
        '
        Me.txtNote.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.txtNote.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtNote.Location = New System.Drawing.Point(932, 11)
        Me.txtNote.Name = "txtNote"
        Me.txtNote.Size = New System.Drawing.Size(196, 20)
        Me.txtNote.TabIndex = 5
        Me.txtNote.TextToClear = Nothing
        '
        'txtProjectDesc
        '
        Me.txtProjectDesc.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.txtProjectDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProjectDesc.Location = New System.Drawing.Point(539, 10)
        Me.txtProjectDesc.Name = "txtProjectDesc"
        Me.txtProjectDesc.ReadOnly = True
        Me.txtProjectDesc.Size = New System.Drawing.Size(264, 20)
        Me.txtProjectDesc.TabIndex = 2
        Me.txtProjectDesc.TextToClear = Nothing
        '
        'txtProjectNo
        '
        Me.txtProjectNo.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.txtProjectNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtProjectNo.Location = New System.Drawing.Point(434, 10)
        Me.txtProjectNo.Name = "txtProjectNo"
        Me.txtProjectNo.Size = New System.Drawing.Size(100, 20)
        Me.txtProjectNo.TabIndex = 1
        Me.txtProjectNo.TextToClear = Nothing
        '
        'txtPartDesc
        '
        Me.txtPartDesc.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtPartDesc.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPartDesc.Location = New System.Drawing.Point(313, 9)
        Me.txtPartDesc.Name = "txtPartDesc"
        Me.txtPartDesc.ReadOnly = True
        Me.txtPartDesc.Size = New System.Drawing.Size(142, 20)
        Me.txtPartDesc.TabIndex = 5
        Me.txtPartDesc.TextToClear = Nothing
        '
        'txtPartNo
        '
        Me.txtPartNo.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPartNo.Location = New System.Drawing.Point(197, 9)
        Me.txtPartNo.Name = "txtPartNo"
        Me.txtPartNo.Size = New System.Drawing.Size(107, 20)
        Me.txtPartNo.TabIndex = 4
        Me.txtPartNo.TextToClear = Nothing
        '
        'CreateJustInTimeTransferToolStripMenuItem
        '
        Me.CreateJustInTimeTransferToolStripMenuItem.Name = "CreateJustInTimeTransferToolStripMenuItem"
        Me.CreateJustInTimeTransferToolStripMenuItem.Size = New System.Drawing.Size(254, 22)
        Me.CreateJustInTimeTransferToolStripMenuItem.Text = "Create Just-In-Time Transfer"
        Me.CreateJustInTimeTransferToolStripMenuItem.Visible = False
        '
        'frmSchedulingBoard
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1303, 715)
        Me.Controls.Add(Me.pnlMain)
        Me.Controls.Add(Me.pnlQueryCriteria)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.MinimumSize = New System.Drawing.Size(999, 599)
        Me.Name = "frmSchedulingBoard"
        Me.Text = "Scheduling Board"
        Me.pnlMain.ResumeLayout(False)
        Me.scPartsAndDetail.Panel1.ResumeLayout(False)
        Me.scPartsAndDetail.Panel2.ResumeLayout(False)
        CType(Me.scPartsAndDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scPartsAndDetail.ResumeLayout(False)
        Me.scParts.Panel1.ResumeLayout(False)
        Me.scParts.Panel2.ResumeLayout(False)
        Me.scParts.Panel2.PerformLayout()
        CType(Me.scParts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scParts.ResumeLayout(False)
        Me.pnlMultiPartGroups.ResumeLayout(False)
        Me.gbMultiPartGroups.ResumeLayout(False)
        Me.gbMultiPartGroups.PerformLayout()
        Me.tsMultiPartGroups.ResumeLayout(False)
        Me.tsMultiPartGroups.PerformLayout()
        Me.tcMain.ResumeLayout(False)
        Me.tpSchedulingBoard.ResumeLayout(False)
        Me.pnlUnvailability.ResumeLayout(False)
        CType(Me.dgvUnavailability, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlUnavailabilityControls.ResumeLayout(False)
        Me.pnlUnavailabilityControls.PerformLayout()
        CType(Me.fgSchedulingBoard, System.ComponentModel.ISupportInitialize).EndInit()
        Me.mnuFlexgrid.ResumeLayout(False)
        Me.pnlSchedulingBoardControls.ResumeLayout(False)
        Me.pnlSchedulingBoardControls.PerformLayout()
        CType(Me.pbEmailTeam, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpTimeline.ResumeLayout(False)
        CType(Me.fgTimeline, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpCalendar.ResumeLayout(False)
        CType(Me.fgCalendar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.tsMainVert.ResumeLayout(False)
        Me.tsMainVert.PerformLayout()
        CType(Me.dgvDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.cmsPartSchedulingDetails.ResumeLayout(False)
        Me.pnlDetailControls.ResumeLayout(False)
        Me.pnlDetailControls.PerformLayout()
        Me.pnlTotalsAndWarehouses.ResumeLayout(False)
        Me.scWarehouses.Panel1.ResumeLayout(False)
        Me.scWarehouses.Panel2.ResumeLayout(False)
        CType(Me.scWarehouses, System.ComponentModel.ISupportInitialize).EndInit()
        Me.scWarehouses.ResumeLayout(False)
        Me.pnlWarehouses.ResumeLayout(False)
        Me.mnuWarehouse.ResumeLayout(False)
        Me.pnlWarehouse.ResumeLayout(False)
        Me.cmsWarehouseGroups.ResumeLayout(False)
        Me.pnlTotals.ResumeLayout(False)
        Me.pnlAddPartToOrder.ResumeLayout(False)
        Me.pnlAddPartToOrder.PerformLayout()
        Me.pnlViewOptions.ResumeLayout(False)
        Me.pnlSearchForPart.ResumeLayout(False)
        Me.pnlSearchForPart.PerformLayout()
        Me.pnlControls.ResumeLayout(False)
        Me.pnlControls.PerformLayout()
        Me.pnlRefresh.ResumeLayout(False)
        Me.pnlExcelLame.ResumeLayout(False)
        CType(Me.bsDetail, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnMultiPartGroup_Add As System.Windows.Forms.Button
    Friend WithEvents cboMultiPartGroups As System.Windows.Forms.ComboBox
    Friend WithEvents btnMultiPartGroup_Delete As System.Windows.Forms.Button
    Friend WithEvents btnMultiPartList_MoveUp As System.Windows.Forms.Button
    Friend WithEvents btnMultiPartList_Delete As System.Windows.Forms.Button
    Friend WithEvents btnMultiPartList_Add As System.Windows.Forms.Button
    Friend WithEvents lstMultiPartList As System.Windows.Forms.CheckedListBox
    Friend WithEvents btnMultiPartGroup_Copy As System.Windows.Forms.Button
    Friend WithEvents btnMultiPartGroup_Rename As System.Windows.Forms.Button
    Friend WithEvents btnMultiPartList_MoveDown As System.Windows.Forms.Button
    Friend WithEvents pnlMain As System.Windows.Forms.Panel
    Friend WithEvents chkIncludeProposals As System.Windows.Forms.CheckBox
    Friend WithEvents dtpEndDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents dtpStartDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents lblEndDate As System.Windows.Forms.Label
    Friend WithEvents lblStartDate As System.Windows.Forms.Label
    Friend WithEvents pnlQueryCriteria As System.Windows.Forms.Panel
    Friend WithEvents pnlViewOptions As System.Windows.Forms.Panel
    Friend WithEvents fgSchedulingBoard As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents tsMainVert As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbProjectMaintenance As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbTransferTools As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbAvailability As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbUtilization As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbPartMaintenance As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbPartAttachmentMaintenance As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbCrewTools As System.Windows.Forms.ToolStripButton
    Friend WithEvents tsbProjectStoryboard As System.Windows.Forms.ToolStripButton
    Friend WithEvents chkShowDetail As System.Windows.Forms.CheckBox
    Friend WithEvents mnuFlexgrid As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SubstitutePartToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ChangeOrderedQtyToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents chkShowFuture As System.Windows.Forms.CheckBox
    Friend WithEvents btnRefresh As System.Windows.Forms.Button
    Friend WithEvents tsbShowSearchCriteria As System.Windows.Forms.ToolStripButton
    Friend WithEvents ClearHighlightingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mnuSubstitutePartSeparator As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents ProjectMaintenanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TransferToolsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AvailabilityToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents PartMaintenanceToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents btnFindSelectedPartsGroup As System.Windows.Forms.Button
    Friend WithEvents cmdExportToExcel As System.Windows.Forms.Button
    Friend WithEvents chkSummarize As System.Windows.Forms.CheckBox
    Friend WithEvents btnLameLink As System.Windows.Forms.Button
    Friend WithEvents pnlControls As Panel
    Friend WithEvents pnlWarehouses As Panel
    Friend WithEvents pnlWarehouse As Panel
    Friend WithEvents lblRepairQty As Label
    Friend WithEvents tcMain As TabControl
    Friend WithEvents tpCalendar As TabPage
    Friend WithEvents fgCalendar As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents fgTimeline As AxMSFlexGridLib.AxMSFlexGrid
    Friend WithEvents tpSchedulingBoard As TabPage
    Friend WithEvents lblLateQty As Label
    Friend WithEvents lblWarehouseDesc As Label
    Friend WithEvents pnlMultiPartGroups As Panel
    Friend WithEvents lblGroup As Label
    Friend WithEvents scParts As SplitContainer
    Friend WithEvents chkToggleWarehouse As CheckBox
    Friend WithEvents scPartsAndDetail As SplitContainer
    Friend WithEvents dgvDetail As DataGridView
    Friend WithEvents pnlDetailControls As Panel
    Friend WithEvents lblCloseDetailPane As Label
    Friend WithEvents cmdDetailToExcel As Button
    Friend WithEvents lblPartSchedulingDetail As Label
    Friend WithEvents chkGroupByParentProject As CheckBox
    Friend WithEvents tpTimeline As TabPage
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents sslblLength As ToolStripStatusLabel
    Friend WithEvents sslblLengthValue As ToolStripStatusLabel
    Friend WithEvents sslblWidth As ToolStripStatusLabel
    Friend WithEvents sslblWidthValue As ToolStripStatusLabel
    Friend WithEvents sslblHeight As ToolStripStatusLabel
    Friend WithEvents sslblHeightValue As ToolStripStatusLabel
    Friend WithEvents sslblWeight As ToolStripStatusLabel
    Friend WithEvents sslblWeightValue As ToolStripStatusLabel
    Friend WithEvents sslblRentalValue As ToolStripStatusLabel
    Friend WithEvents sslblRentalValueValue As ToolStripStatusLabel
    Friend WithEvents sslblWeeklyRate As ToolStripStatusLabel
    Friend WithEvents sslblWeeklyRateValue As ToolStripStatusLabel
    Friend WithEvents sslbl2DayWeekRate As ToolStripStatusLabel
    Friend WithEvents sslbl2DayWeekRateValue As ToolStripStatusLabel
    Friend WithEvents sslblDailyRate As ToolStripStatusLabel
    Friend WithEvents sslblDailyRateValue As ToolStripStatusLabel
    Friend WithEvents pnlSchedulingBoardControls As Panel
    Friend WithEvents lblPopOut As Label
    Friend WithEvents timer_DoubleClickInterval As Timer
    Friend WithEvents pnlAddPartToOrder As Panel
    Friend WithEvents cmdOpenProject As Button
    Friend WithEvents cmdProjectSearch As Button
    Friend WithEvents txtProjectDesc As HighlightTextBox
    Friend WithEvents txtProjectNo As HighlightTextBox
    Friend WithEvents cmdAddPartToOrder As Button
    Friend WithEvents chkForToday As CheckBox
    Friend WithEvents lblCloseAddPartToOrder As Label
    Friend WithEvents lblNote As Label
    Friend WithEvents txtNote As HighlightTextBox
    Friend WithEvents pnlSearchForPart As Panel
    Friend WithEvents cmdPartSearch As Button
    Friend WithEvents txtPartDesc As HighlightTextBox
    Friend WithEvents txtPartNo As HighlightTextBox
    Friend WithEvents pnlRefresh As Panel
    Friend WithEvents QueryToolsToolStripDropDownButton As ToolStripDropDownButton
    Friend WithEvents EquipmentLocationsToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents TransactionsToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents PhaseHistoryToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator14 As ToolStripSeparator
    Friend WithEvents GearListToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ManifestSummaryToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ManifestToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents CarnetListToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents TicSheetToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator15 As ToolStripSeparator
    Friend WithEvents LateReturnsToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents BatchLocationSummaryToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents tsbBLInq As ToolStripButton
    Friend WithEvents tsbAddInventory As ToolStripButton
    Friend WithEvents tsbRemoveInventory As ToolStripButton
    Friend WithEvents tsbDeviceMaintenance As ToolStripButton
    Friend WithEvents tsbCycleCountUtility As ToolStripDropDownButton
    Friend WithEvents MarkAsCountedToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents tsbRepairLog As ToolStripButton
    Friend WithEvents cmdShare As Button
    Friend WithEvents lblPlannedOrder As Label
    Friend WithEvents pnlTotals As Panel
    Friend WithEvents lblExclSubWexler As Label
    Friend WithEvents lblChildren As Label
    Friend WithEvents lblTotal As Label
    Friend WithEvents pnlTotalsAndWarehouses As Panel
    Friend WithEvents lblInventory As Label
    Friend WithEvents cmdSearchByBarcode As Button
    Friend WithEvents pnlExcelLame As Panel
    Friend WithEvents CycleCountUtilityToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents lblCollapseExpandWarehouses As Label
    Friend WithEvents CopyPartDescriptionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyPartNumberAndDescriptionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyPhaseNumberToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyPhaseDescriptionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyPhaseNumberAndDescriptionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents CopyPartNumberToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CopyPartBarcodeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BtnForward As Button
    Friend WithEvents BtnBack As Button
    Friend WithEvents LblProjectName As Label
    Friend WithEvents cmdHelp As Button
    Friend WithEvents chkLockDates As CheckBox
    Friend WithEvents pbEmailTeam As PictureBox
    Friend WithEvents mnuWarehouse As ContextMenuStrip
    Friend WithEvents RunRefreshToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ShowAllToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents timer_RefreshData As Timer
    Friend WithEvents gbMultiPartGroups As GroupBox
    Friend WithEvents tsMultiPartGroups As ToolStrip
    Private WithEvents lblCollapseMultiPartGroups As ToolStripButton
    Friend WithEvents ToolStripDropDownButton1 As ToolStripDropDownButton
    Friend WithEvents ResetMyMultipartGroupsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents scWarehouses As SplitContainer
    Friend WithEvents lvWarehouseGroups As ListView
    Friend WithEvents lvcWarehouseGroup As ColumnHeader
    Friend WithEvents cmsWarehouseGroups As ContextMenuStrip
    Friend WithEvents tsmiOverwriteWarehoseGroup As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents tsmiCreateNewWarehouseGroup As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator4 As ToolStripSeparator
    Friend WithEvents tsmiDeleteWarehouseGroup As ToolStripMenuItem
    Friend WithEvents tsmiRenameWarehouseGroup As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator6 As ToolStripSeparator
    Friend WithEvents tsmiSaveGroups As ToolStripMenuItem
    Friend WithEvents cmsPartSchedulingDetails As ContextMenuStrip
    Friend WithEvents csmiPartSchedulingDetailCopyPhaseNumber As ToolStripMenuItem
    Friend WithEvents csmiPartSchedulingDetailCopyPhaseDesc As ToolStripMenuItem
    Friend WithEvents csmiPartSchedulingDetailCopyPhaseNumberDesc As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator7 As ToolStripSeparator
    Friend WithEvents csmiPartSchedulingDetailSubstitutePart As ToolStripMenuItem
    Friend WithEvents bsDetail As BindingSource
    Friend WithEvents cmdAddTransfer As Button
    Friend WithEvents tsbMode As ToolStripSplitButton
    Friend WithEvents CrewModeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EquipmentModeToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pnlUnvailability As Panel
    Friend WithEvents dgvUnavailability As DataGridView
    Friend WithEvents tsbUnavailability As ToolStripButton
    Friend WithEvents pnlUnavailabilityControls As Panel
    Friend WithEvents cmdUnavailabilitySetCompareWarehouses As Button
    Friend WithEvents flpUnavailabilityCompareWarehouses As FlowLayoutPanel
    Friend WithEvents lblUnavailabilityCompareWarehouses As Label
    Friend WithEvents txtUnavailabilityProjectDesc As TextBox
    Friend WithEvents txtUnavailabilityProjectNo As HighlightTextBox
    Friend WithEvents cmdUnavailabilityProjectNumber As Button
    Friend WithEvents cmdUnavailabilityRefresh As Button
    Friend WithEvents chkUnvailabilityHideWhenSumBottleneckGreaterThansZero As CheckBox
    Friend WithEvents chkUnavailabilityHideTurnaroundDays As CheckBox
    Friend WithEvents chkShowRemovedParts As CheckBox
    Friend WithEvents chkUnavailabilitySumWarehouses As CheckBox
    Friend WithEvents dtpUnavailabilityCutoff As DateTimePicker
    Friend WithEvents chkUnavailabilityCutoffDate As CheckBox
    Friend WithEvents CreateJustInTimeTransferToolStripMenuItem As ToolStripMenuItem
End Class