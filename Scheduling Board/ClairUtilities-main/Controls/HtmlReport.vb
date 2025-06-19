Imports System.Data.SqlClient
Imports System.Drawing.Printing
Imports System.IO
Imports System.Text
Imports Outlook = Microsoft.Office.Interop.Outlook
Imports Word = Microsoft.Office.Interop.Word

Public Class HtmlReport
    Inherits UserControl

    Private cmndsel As String = "HTMLREPORT"
    Private can_edit_HTMLReport As Boolean = False
    Private _dtAppConfig As New DataTable
    Private _dtMyUserInfo As New DataTable

    Private _dtLanguage As New DataTable
    Private _dtCurrency As New DataTable

    Private _dtReportFieldTranslations As DataTable
    Private _dtGenericPickerParameters As DataTable
    Private _dtGUIColumnNamesFromSQLFields As New DataTable

    Private FINESSE_PUBLICATION_ROOT_DIRECTORY As String

    Private _initialized As Boolean = False
    Private _suspendUpdates As Boolean = False

    Protected Class SQLResultType
        Public id_HtmlBlock As Integer
        Public ResultTypeCode As String
    End Class

    Private _ClairNewExists As Boolean
    Private _id_Template As Integer = -1
    Private _ExcludeHtmlBlocks As Integer() = Nothing
    Private _Recipients As String = String.Empty
    Private _CopyRecipients As String = String.Empty
    Private _EmailBody As String = String.Empty
    Private _ReportOverrideTitleFormat As String = String.Empty
    Private _ReportHeaderIsEmployeeCompany As Boolean = False
    Private _ReportHeaderOverrideCompany As String = String.Empty
    Private _ReportHeaderAccountsLogoOverride As Boolean = False
    Private _sParameters As String = String.Empty
    Private ReplaceParameters As New Dictionary(Of String, String)
    'Private _sCallingAppCmndsel As String = String.Empty
    Private _sAppPrinterName As String = String.Empty
    Private _AllowPrintBitMap As Boolean = False
    Private _BitmapPrintSize As Size = Nothing

    Private _entityno As String = String.Empty
    Private _entitydesc As String = String.Empty
    Private _empno As String = String.Empty
    Private _empname As String = String.Empty
    Private _action As String = String.Empty

    Private _ExportImmediately As Boolean = False
    Private _alreadyExported As Boolean = False
    Private _FileType As String = String.Empty
    Private _FilePath As String = String.Empty

    Private _dtuserinfo As DataTable

    Private _MySignatureImagePath = String.Empty

    Private dtReportHeaderInfo As DataTable
    Private dtProjectInfo As DataTable

    Private dtHtmlBlocks As DataTable
    Private dtHtmlBlocksGroupBy As DataTable
    Private dtHtmlBlocksDataFormat As DataTable
    Private dtHtmlBlocksSummaries As DataTable

    Private sReportTitle As String = String.Empty

    Private BrowserVScroll As Integer = 0

    Private WebBrowserPageSetupKeyName As String = "Software\Microsoft\Internet Explorer\PageSetup"
    Private strWebBrowserRestoreHeader = Nothing
    Private strWebBrowserRestoreFooter = Nothing

    Private ESSVBDir = Environment.GetEnvironmentVariable("ESSVBDir")

    Public Event exportingPDF()
    Public Event exportingDoc()

#Region "Properties"

    Public Property id_template As Integer
        Get
            id_template = _id_Template
        End Get
        Set(ByVal value As Integer)
            _id_Template = value
        End Set
    End Property

    Public Property Parameters As String
        Get
            Parameters = _sParameters
        End Get
        Set(ByVal value As String)
            _sParameters = value
        End Set
    End Property

    Public Property AppPrinterName As String
        Get
            AppPrinterName = _sAppPrinterName
        End Get
        Set(ByVal value As String)
            _sAppPrinterName = value
        End Set
    End Property

    Public Property UseAppPrinter As Boolean
        Get
            UseAppPrinter = tsbUseAppPrinter.Checked
        End Get
        Set(ByVal value As Boolean)
            tsbUseAppPrinter.Checked = value
        End Set
    End Property

    Public Property AllowPrintBitmap As Boolean
        Get
            AllowPrintBitmap = _AllowPrintBitMap
        End Get
        Set(ByVal value As Boolean)
            tsbPrintBitmap.Visible = value
        End Set
    End Property

    Public Property BitmapPrintSize As Size
        Get
            BitmapPrintSize = _BitmapPrintSize
        End Get
        Set(ByVal value As Size)
            _BitmapPrintSize = value
        End Set
    End Property

    Public Property ExcludeHTMLBlocks As Integer()
        Get
            ExcludeHTMLBlocks = _ExcludeHtmlBlocks
        End Get
        Set(ByVal value As Integer())
            _ExcludeHtmlBlocks = value
        End Set
    End Property

    Public Property Recipients As String
        Get
            Recipients = _Recipients
        End Get
        Set(ByVal value As String)
            _Recipients = value
        End Set
    End Property

    Public Property CopyRecipients As String
        Get
            CopyRecipients = _CopyRecipients
        End Get
        Set(ByVal value As String)
            _CopyRecipients = value
        End Set
    End Property

    Public Property EmailBody As String
        Get
            EmailBody = _EmailBody
        End Get
        Set(ByVal value As String)
            _EmailBody = value
        End Set
    End Property

    Public Property ReportOverrideTitleFormat As String
        Get
            ReportOverrideTitleFormat = _ReportOverrideTitleFormat
        End Get
        Set(ByVal value As String)
            _ReportOverrideTitleFormat = value
        End Set
    End Property

    Public Property ReportHeaderIsEmployeeCompany As Boolean
        Get
            ReportHeaderIsEmployeeCompany = _ReportHeaderIsEmployeeCompany
        End Get
        Set(ByVal value As Boolean)
            _ReportHeaderIsEmployeeCompany = value
        End Set
    End Property

    Public Property ReportHeaderOverrideCompany As String
        Get
            ReportHeaderOverrideCompany = _ReportHeaderOverrideCompany
        End Get
        Set(ByVal value As String)
            _ReportHeaderOverrideCompany = value
        End Set
    End Property

    Public Property ReportHeaderAccountsLogoOverride As Boolean
        Get
            ReportHeaderAccountsLogoOverride = _ReportHeaderAccountsLogoOverride
        End Get
        Set(ByVal value As Boolean)
            _ReportHeaderAccountsLogoOverride = value
        End Set
    End Property

    Public Property ExportImmediately As Boolean
        Get
            ExportImmediately = _ExportImmediately
        End Get
        Set(ByVal value As Boolean)
            _ExportImmediately = value
        End Set
    End Property

    Public Property FileType As String
        Get
            FileType = _FileType
        End Get
        Set(ByVal value As String)
            _FileType = value
        End Set
    End Property

    Public Property FilePath As String
        Get
            FilePath = _FilePath
        End Get
        Set(ByVal value As String)
            _FilePath = value
        End Set
    End Property

    Public Property alreadyExported As Boolean
        Get
            alreadyExported = _alreadyExported
        End Get
        Set(ByVal value As Boolean)
            _alreadyExported = value
        End Set
    End Property

    Public Property ClairNewExists As Boolean
        Get
            ClairNewExists = _ClairNewExists
        End Get
        Set(ByVal value As Boolean)
            _ClairNewExists = value
        End Set
    End Property

#End Region

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'InitializeHtmlReport()

    End Sub

    Public Sub New(ByVal id_Template As Integer, ByVal sParameters As String)
        MyBase.New()

        ' This call is required by the designer.
        InitializeComponent()

        _id_Template = id_Template
        _sParameters = sParameters

        ' Add any initialization after the InitializeComponent() call.
        InitializeHtmlReport()

    End Sub

    Public Sub InitializeHtmlReport()
        scHTML.Panel1Collapsed = True
        lblReportError.Dock = DockStyle.Fill

#If DEBUG Then
        scHTML.Panel1Collapsed = False
#End If

        If String.IsNullOrEmpty(id_template) OrElse id_template = Nothing OrElse id_template = -1 Then
            lblReportError.Text = "No report available.  This should not happen.  Please contact your IT Administrator."
            End
        Else
            _id_Template = id_template
        End If
        Initializations()

        InitializeParameters(Parameters)

        _sAppPrinterName = AppPrinterName

        GetHTMLBlocks()

        _initialized = True

        'Application.DoEvents()
        'RunReport()
    End Sub

    Private Sub InitializeParameters(sParameters As String)
        _suspendUpdates = True

        _sParameters = sParameters
        ReplaceParameters.Clear()
        If Not _sParameters Is Nothing AndAlso Not String.IsNullOrEmpty(_sParameters) Then
            For Each p As String In _sParameters.Trim.Split(";")
                Dim splitParam = p.Split({"="c, ":"c}, 2)
                If splitParam.Count = 2 Then
                    Dim key = splitParam(0)
                    Dim value = Replace(splitParam(1), "'", String.Empty)
                    ReplaceParameters.Add("@" & key, value)

                    If key = "id_language" Then
                        Dim rLanguage = _dtLanguage.Select("id_language=" & SQLQuote(value) & " or alt_id_language=" & SQLQuote(value))
                        If rLanguage.Count = 1 Then
                            tscboLanguage.Text = _dtLanguage.Select("id_language=" & SQLQuote(value) & " or alt_id_language=" & SQLQuote(value))(0)("language")
                        Else
                            MsgBox("Invalid language selected for this report: " & key & "=" & value & ".  English will be used by deafult.")
                        End If

                    End If

                    If key = "entityno" Then
                        _entityno = value
                        If String.IsNullOrEmpty(_entitydesc) Then _entitydesc = Get_ProjectDescFromProjectNo(_entityno)

                    End If
                    If key = "empno" Then
                        _empno = value

                    End If
                    If key = "action" Then
                        _action = value

                    End If
                    If key = "empname" Then
                        _empname = value
                        'Make sure empno is provided as a parameter in front of empname if the empname needs to be looked up in the database.
                        If String.IsNullOrWhiteSpace(_empname) Then
                            _empname = Get_EmpnameFromEmpno(_empno)
                        End If
                    End If

                    'added conditional for if export immediately 
                    If key = "ExportImmediately" Then
                        _ExportImmediately = value
                    End If
                    If key = "FileType" Then
                        _FileType = value
                    End If
                    If key = "FilePath" Then
                        _FilePath = value
                    End If
                End If
            Next
        End If

        If Not ReplaceParameters.ContainsKey("@id_language") Then
            ReplaceParameters.Add("@id_langauge", GetLanguageID())
        End If

        _suspendUpdates = False

    End Sub

    Protected Sub Initializations()

        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))

        jammer.Add("select can_edit_HTMLReport = dbo.can_edit_HTMLReport(), u.* from my_user_info u",
                   Sub(t)
                       can_edit_HTMLReport = t.Rows(0).Item("can_edit_HTMLReport")
                       cmdEditReportHTMLSQL.Visible = can_edit_HTMLReport
                       _dtMyUserInfo = t
                   End Sub)

        jammer.Add("select * from dbo.App_Config ac where ac.cmndsel = " & cmndsel.SQLQuote,
                   Sub(t)
                       _dtAppConfig = t
                   End Sub)

        jammer.Add("select gf.SQLField , gf.ColumnName from dbo.GUIColumnNamesFromSQLFields gf",
                   Sub(t)
                       _dtGUIColumnNamesFromSQLFields = t
                   End Sub)

        jammer.Add("select * from dbo.Currency c",
                   Sub(t)
                       _dtCurrency = t
                   End Sub)

        jammer.Add("select * from dbo.xlat_languages xl where xl.language not like ('%sales%') and xl.id_language not in (11,12,13)",
                   Sub(t)
                       If _dtLanguage.Rows.Count = 0 Then
                           _dtLanguage = t

                           For Each r As DataRow In _dtLanguage.Rows
                               tscboLanguage.Items.Add(r.Item("language"))
                           Next
                           tscboLanguage.SelectedItem = tscboLanguage.Items(0)
                       End If
                   End Sub)

        jammer.Add("select rft.field , rft.id_language , rft.translation from dbo.ReportFieldTranslations rft",
                   Sub(t)
                       _dtReportFieldTranslations = t
                   End Sub)

        jammer.Add("select gpp.Title , gpp.SQLQuery , gpp.DisplayMember , gpp.ValueMember from dbo.GenericPickerParameters gpp",
                   Sub(t)
                       _dtGenericPickerParameters = t
                   End Sub)

        jammer.Add("select sc.Value from dbo.SysConfig sc where sc.Tag = 'FINESSE_PUBLICATION_ROOT_DIRECTORY'",
                   Sub(t)
                       FINESSE_PUBLICATION_ROOT_DIRECTORY = t.Rows(0).Item("Value")
                   End Sub)
        jammer.Add("select p.user_name, c.CompanyCode, c.locationcd, o.AutoInvoiceGeneration
                    from ClairTour.dbo.my_user_info p
                    join ClairTour.dbo.Company c on p.DefaultBillingCompany = c.CompanyCode
                    join ClairTour.dbo.oelocation o on c.locationcd = o.locationcd",
                   Sub(t)
                       _dtuserinfo = t
                   End Sub)
        jammer.Add("select ClairNewExists = case when (
                        select db_id(convert(sysname, sc.Value))
                        from dbo.SysConfig sc
                        where sc.Tag = 'PARALLEL_DATABASE'
                      ) is not null then 1 else 0 END",
                   Sub(t)
                       ClairNewExists = t(0).Item("ClairNewExists")
                   End Sub)
        jammer.Execute()

        _initialized = True

    End Sub

#Region "Functions"

    Public Function Get_EmailFromEmpno(ByVal empno As String) As String
        Dim email = String.Empty
        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))
        jammer.Add("select e.email from dbo.peemployee e where e.empno = " & empno.SQLQuote,
                   Sub(t)

                       If t.Rows.Count = 1 Then
                           email = t.Rows(0).Item("email")
                       End If

                   End Sub)
        jammer.Execute()
        Return email
    End Function

    Public Function Get_EmpnoFromProjectNo(ByVal entityno As String) As String
        Dim empno = String.Empty
        Dim crewempnos = String.Empty
        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))
        jammer.Add("SELECT DISTINCT engrEmpno, acctExecEmpno, RFPLEmpno, respempno, opsmgr from dbo.glentities g WHERE g.entityno = " & entityno.SQLQuote,
               Sub(t)

                   If t.Rows.Count = 1 Then
                       For col As Integer = 0 To t.Columns.Count - 1
                           If Not IsDBNull(t.Rows(0).Item(col)) Then
                               empno = empno + ":" + t.Rows(0).Item(col)
                           End If
                       Next
                   End If

               End Sub)

        jammer.Add("SELECT e.empno FROM dbo.pjempassign e WHERE e.entityno LIKE " & entityno.SQLQuote & " + '%'" & "AND e.StatusCode = 'A'",
               Sub(t)

                   If t.Columns.Count = 1 Then
                       For row As Integer = 0 To t.Rows.Count - 1
                           If Not IsDBNull(t.Rows(row).Item(0)) Then
                               crewempnos = crewempnos + ":" + t.Rows(row).Item(0)
                           End If
                       Next
                   End If

               End Sub)
        jammer.Execute()
        Return (empno + crewempnos)
    End Function

    Protected Function Get_myFilename(Optional Clean As Boolean = True)
        Dim filename = sReportTitle

        Dim FromTranslation = _dtReportFieldTranslations.Select("field='quote_report_From' and id_language=" & GetLanguageID())
        Dim strFrom = String.Empty

        If FromTranslation.Length > 0 Then
            strFrom = _dtReportFieldTranslations.Select("field='quote_report_From' and id_language=" & GetLanguageID())(0)("translation")
        End If

        Dim sDateFormat = "yyyy-MM-dd"

        'Select Case _dtMyUserInfo.Rows(0).Item("SalesForecastGroup")
        '    Case "USA"
        '        'Use USA default
        '    Case Else
        '        sDateFormat = "yyyy-dd-MM"
        'End Select

        filename = sReportTitle & " " & IIf(Not strFrom = String.Empty, strFrom & " ", String.Empty) & Today.ToString(sDateFormat)

        If Clean Then
            filename = strClean(filename)
        End If

        Return filename
    End Function

    Protected Function myHTMLFormat(ByVal StringToFormat As String, ByVal id_HtmlBlock As Integer, ByVal FieldName As String)
        Dim formatted As String = StringToFormat

        Dim FormatType = dtHtmlBlocksDataFormat.Select("id_HtmlBlock=" & id_HtmlBlock & " and Field=" & FieldName.SQLQuote)

        Dim cultureToUse As System.Globalization.CultureInfo
        If dtProjectInfo IsNot Nothing Then
            cultureToUse = GetProjectCurrencyCulture(dtProjectInfo)
        Else
            cultureToUse = Globalization.CultureInfo.CurrentCulture
        End If

        If FormatType.Any Then
            If Not String.IsNullOrEmpty(StringToFormat) Then
                Select Case FormatType(0).Item("FormatType")
                    Case "C2"
                        Dim formatCurrency = Function(currencyValue) String.Format(cultureToUse, "{0:N2}", currencyValue)
                        formatted = formatCurrency(CDbl(StringToFormat))
                    Case "C0"
                        Dim formatCurrency = Function(currencyValue) String.Format(cultureToUse, "{0:N0}", currencyValue)
                        formatted = formatCurrency(CDbl(StringToFormat))
                    Case "N2"
                        Dim formatNumber = Function(numberValue) String.Format(cultureToUse, "{0:N2}", numberValue)
                        formatted = formatNumber(CDbl(StringToFormat))
                    Case "N0"
                        Dim formatNumber = Function(numberValue) String.Format(cultureToUse, "{0:N0}", numberValue)
                        formatted = formatNumber(CDbl(StringToFormat))
                End Select
            End If
        End If

        Return formatted
    End Function

    Protected Function GetProjectCurrencyCulture(ByVal dtProjectInfo As DataTable) As System.Globalization.CultureInfo
        Dim cultureBase As Globalization.CultureInfo = GetCurrencyCulture(GetCurrencyRow(dtProjectInfo))
        Dim cultureOverride = New Globalization.CultureInfo(cultureBase.Name, True)

        Dim currencyRow As DataRow = GetCurrencyRow(dtProjectInfo)

        cultureOverride.NumberFormat.CurrencySymbol = currencyRow.Item("symbol")
        cultureOverride.NumberFormat.CurrencyDecimalSeparator = currencyRow.Item("DecimalSeparator")
        cultureOverride.NumberFormat.CurrencyGroupSeparator = currencyRow.Item("ThousandsSeparator")

        Return cultureOverride
    End Function

    Protected Function GetCurrencyCulture(ByVal selectedCurrency As System.Data.DataRow) As System.Globalization.CultureInfo
        Return Globalization.CultureInfo.GetCultureInfo(selectedCurrency("culture"))
    End Function

    Protected Function GetCurrencyRow(ByVal dtProjectInfo As DataTable) As System.Data.DataRow
        If dtProjectInfo Is Nothing Then Return Nothing
        Return _dtCurrency.Select("currency = " & SQLQuote(dtProjectInfo.Rows(0).Item("currency")))(0)
    End Function

    Protected Function GetLanguageID() As Integer
        Return _dtLanguage.Select("language=" & tscboLanguage.Text.SQLQuote)(0)("id_language")
    End Function

#End Region

#Region "Edit Report Layout"
    Protected Sub cmdEditReportHTMLSQL_Click(sender As Object, e As EventArgs) Handles cmdEditReportHTMLSQL.Click
        scHTML.Panel1Collapsed = Not scHTML.Panel1Collapsed
        Me.ParentForm.AcceptButton = Nothing
    End Sub

    Protected Sub cmdGetHTMLBlocks_Click(sender As Object, e As EventArgs) Handles cmdGetHTMLBlocks.Click
        GetHTMLBlocks()
    End Sub

    Protected Sub GetHTMLBlocks()
        Dim dtHTMLReportBlocks = GetHTMLReportBlocksDataSet()
        If dtHTMLReportBlocks Is Nothing Then Return

        If dtHTMLReportBlocks Is Nothing Then Return

        Dim scrollposition = flpHTML.AutoScrollPosition

        flpHTML.Controls.Clear()

        For Each r As DataRow In dtHTMLReportBlocks.Rows

            Dim pnl As New Panel()
            pnl.BorderStyle = BorderStyle.FixedSingle
            pnl.Padding = New Padding(3, 3, 3, 3)
            pnl.Margin = New Padding(3, 3, 3, 3)
            pnl.Name = pnlHTMLBlock.Name
            pnl.Tag = r.Item("id_HtmlBlock")
            pnl.Height = pnlHTMLBlock.Height

            For Each c As TextBox In pnlHTMLBlock.Controls.OfType(Of TextBox)
                Dim txt As New TextBox

                'txt = Clone_Control(c)

                txt.Size = c.Size
                txt.Multiline = c.Multiline
                txt.WordWrap = c.WordWrap
                txt.Name = c.Name
                txt.Font = c.Font
                txt.Dock = c.Dock
                txt.ScrollBars = c.ScrollBars

                If Not String.IsNullOrEmpty(c.Tag) Then txt.Text = ReplaceNull(r.Item(c.Tag), String.Empty)

                pnl.Controls.Add(txt)

                txt.SendToBack()

                If txt.Multiline Then txt.Height = (pnl.Height - txtHTMLBlockDescription.Height) / 2
            Next

            Dim findSQLTextBox As TextBox = pnl.Controls.Find(txtHTMLBlockSQL.Name, True)(0)

            If String.IsNullOrEmpty(findSQLTextBox.Text.Trim) Then
                findSQLTextBox.Visible = False

                Dim cmd As New Button
                cmd.Size = cmdAddSQLtoHTMLBlock.Size
                cmd.Text = cmdAddSQLtoHTMLBlock.Text
                cmd.Name = cmdAddSQLtoHTMLBlock.Name
                cmd.Visible = True
                cmd.Dock = DockStyle.Bottom
                AddHandler cmd.Click, AddressOf cmdAddSQLtoHTMLBlock_Click

                pnl.Controls.Add(cmd)

                Dim chk As New CheckBox
                chk.Size = chkIsGroupData.Size
                chk.Text = chkIsGroupData.Text
                chk.Name = chkIsGroupData.Name
                chk.Visible = True
                chk.Dock = DockStyle.Bottom
                AddHandler chk.CheckedChanged, AddressOf chkIsGroupData_CheckedChanged

                pnl.Controls.Add(chk)

                _suspendUpdates = True
                chk.Checked = (r.Item("id_SectionType") = "GD")
                _suspendUpdates = False

            End If

            pnl.Width = scHTML.Panel1.Width - SystemInformation.VerticalScrollBarWidth - pnl.Margin.Horizontal - pnl.Padding.Horizontal

            pnl.Visible = True
            flpHTML.Controls.Add(pnl)
        Next

        AutosizeHTMLBlocks()

        flpHTML.AutoScrollPosition = scrollposition

        Me.Refresh()
    End Sub

    Protected Function GetHTMLReportBlocksDataSet() As DataTable

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("	select hrt.id_Template , hrt.TemplateName , hrt.FileNameTemplate, hrb.id_HtmlBlock , hrb.Description , hrb.Html , hrb.SQLQuery ,hrb.ResultTypeCode , hrb.sortOrder , hrb.isVisible , hrb.isDefault , hrb.id_SectionType , hrts.SortOrder 	")
        sSQL.AppendLine("	from dbo.HtmlReportTemplateSections hrts	")
        sSQL.AppendLine("	join dbo.HtmlReportTemplate hrt on hrts.id_Template = hrt.id_Template	")
        sSQL.AppendLine("	join dbo.HtmlReportBlocks hrb on hrts.id_HtmlBlock = hrb.id_HtmlBlock	")
        sSQL.AppendLine("	where hrts.id_Template = " & _id_Template)
        If Not ClairNewExists Then
            sSQL.AppendLine("   AND ISNULL(SQLQuery,'') not like '%ClairNew.%'") ' was "and SQLQuery not like '%ClairNew.%'  but that also excluded null sql query blocks.
        End If
        If _ExcludeHtmlBlocks IsNot Nothing Then
            sSQL.AppendLine("	and hrb.id_HtmlBlock not in (" & String.Join(",", _ExcludeHtmlBlocks) & ")")
        End If

        sSQL.AppendLine("	order by hrts.SortOrder	")
        sSQL.AppendLine("	")
        sSQL.AppendLine("	select hgb.id_HtmlBlock , hgb.seqno , hgb.GroupByField from dbo.HtmlReportBlocksGroupBy hgb	")
        sSQL.AppendLine("	")
        sSQL.AppendLine("	select hrbdf.id_HtmlBlock , hrbdf.Field , hrbdf.FormatType from dbo.HtmlReportBlocksDataFormat hrbdf	")
        sSQL.AppendLine("	")
        sSQL.AppendLine("	select hrbs.id_HtmlBlock , hrbs.seqno , hrbs.ColumnName , hrbs.SummaryType from dbo.HtmlReportBlocksSummaries hrbs	")

        Using newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()
            Dim ds = newConn.GetDataSet(sSQL)
            dtHtmlBlocks = ds.Tables(0)
            dtHtmlBlocksGroupBy = ds.Tables(1)
            dtHtmlBlocksDataFormat = ds.Tables(2)
            dtHtmlBlocksSummaries = ds.Tables(3)
        End Using


        If Not dtHtmlBlocks.Rows.Count > 0 Then
            lblReportError.Text = "No Report layout available.  Contact your IT Administrator."
            lblReportError.Visible = True
            Return Nothing
        End If

        sReportTitle = If(Not String.IsNullOrEmpty(ReportOverrideTitleFormat), ReportOverrideTitleFormat, ReplaceNull(dtHtmlBlocks.Rows(0).Item("FileNameTemplate"), dtHtmlBlocks.Rows(0).Item("TemplateName")))

        Return dtHtmlBlocks

    End Function

    Private Sub AutosizeHTMLBlocks()

        Dim BlockCount = flpHTML.Controls.Count
        If BlockCount > 3 Then BlockCount = 3
        Dim HeightPerBlock = flpHTML.Height / BlockCount

        For Each p As Panel In flpHTML.Controls.OfType(Of Panel)
            p.Height = HeightPerBlock

            For Each c As TextBox In p.Controls.OfType(Of TextBox)
                If c.Multiline And c.Lines.Count > 1 Then c.Height = (p.Height - txtHTMLBlockDescription.Height) / 2
            Next

        Next

    End Sub

    Protected Sub cmdAddHTMLBlock_Click(sender As Object, e As EventArgs) Handles cmdAddHTMLBlock.Click

        Dim resultTitle = InputBox("HTML Block Title:", "Add HTML Block")
        If String.IsNullOrEmpty(resultTitle) Then
            Return
        End If

        Dim resultCreateSQL = MsgBox("Create SQL inquiry with this HTML Block?", vbYesNoCancel)
        If resultCreateSQL = MsgBoxResult.Cancel Then
            Return
        End If

        Dim SQLforHTMLBlock = "select TheDate = dbo.today()"

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("	declare @new_SortOrder as float, @new_id_HtmlBlock int		")
        sSQL.AppendLine("	declare @new_HtmlBlock table(new_id_HtmlBlock int)		")
        sSQL.AppendLine("			")
        sSQL.AppendLine("	select @new_SortOrder = isnull(max(hrts.SortOrder),0) + 1		")
        sSQL.AppendLine("	from dbo.HtmlReportTemplateSections hrts		")
        sSQL.AppendLine("	where hrts.id_Template = " & _id_Template)
        sSQL.AppendLine("			")
        sSQL.AppendLine("	insert dbo.HtmlReportBlocks		")
        sSQL.AppendLine("	        ( Description ,		")
        sSQL.AppendLine("	          Html ,		")
        sSQL.AppendLine("	          SQLQuery ,		")
        sSQL.AppendLine("	          ResultTypeCode ,		")
        sSQL.AppendLine("	          sortOrder ,		")
        sSQL.AppendLine("	          isVisible ,		")
        sSQL.AppendLine("	          isDefault ,		")
        sSQL.AppendLine("	          id_SectionType		")
        sSQL.AppendLine("	        )		")
        sSQL.AppendLine("		output Inserted.id_HtmlBlock into @new_HtmlBlock	")
        sSQL.AppendLine("	values  ( " & resultTitle.SQLQuote & " , -- Description - varchar(100)		")
        sSQL.AppendLine("	          '@TheDate' , -- Html - varchar(max)		")
        sSQL.AppendLine("	          " & IIf(resultCreateSQL = MsgBoxResult.Yes, SQLforHTMLBlock.SQLQuote, "null") & " , -- SQLQuery - varchar(max)		")
        sSQL.AppendLine("	          " & IIf(resultCreateSQL = MsgBoxResult.Yes, "'S'", "null") & " , -- ResultTypeCode - varchar(5)		")
        sSQL.AppendLine("	          @new_SortOrder , -- sortOrder - float		")
        sSQL.AppendLine("	          1 , -- isVisible - int		")
        sSQL.AppendLine("	          1 , -- isDefault - int		")
        sSQL.AppendLine("	          'RH'  -- id_SectionType - varchar(5)		")
        sSQL.AppendLine("	        )		")
        sSQL.AppendLine("			")
        sSQL.AppendLine("	select @new_id_HtmlBlock = nhb.new_id_HtmlBlock from @new_HtmlBlock nhb		")
        sSQL.AppendLine("			")
        sSQL.AppendLine("	insert dbo.HtmlReportTemplateSections		")
        sSQL.AppendLine("	        ( id_Template ,		")
        sSQL.AppendLine("	          id_HtmlBlock ,		")
        sSQL.AppendLine("	          SortOrder		")
        sSQL.AppendLine("	        )		")
        sSQL.AppendLine("	values  ( " & _id_Template & " , -- id_Template - int		")
        sSQL.AppendLine("	          @new_id_HtmlBlock , -- id_HtmlBlock - int		")
        sSQL.AppendLine("	          @new_SortOrder  -- SortOrder - float		")
        sSQL.AppendLine("	        )		")

        Using newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()

            newConn.ExecuteNonQuery(sSQL)
        End Using

        GetHTMLBlocks()

    End Sub

    Protected Sub cmdAddSQLtoHTMLBlock_Click(sender As Object, e As EventArgs) Handles cmdAddSQLtoHTMLBlock.Click

        Dim pnl As Panel = TryCast(sender, Control).Parent
        Dim id_HtmlBlock As Integer = pnl.Tag

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("	update hrb set hrb.SQLQuery = 'select TheDate = dbo.today()', hrb.ResultTypeCode = 'DT'	")
        sSQL.AppendLine("	from dbo.HtmlReportBlocks hrb	")
        sSQL.AppendLine("	where hrb.id_HtmlBlock = " & id_HtmlBlock)

        Using newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()
            newConn.ExecuteNonQuery(sSQL)
        End Using

        GetHTMLBlocks()

    End Sub

    Protected Sub cmdSaveReportLayout_Click(sender As Object, e As EventArgs) Handles cmdSaveReportLayout.Click
        SaveReportLayout()
    End Sub

    Protected Sub SaveReportLayout()
        Dim dtHTMLReportBlocks = GetHTMLReportBlocksDataSet()

        For Each pnl In flpHTML.Controls.OfType(Of Panel)
            Dim Block = dtHTMLReportBlocks.Select("id_HtmlBlock=" & pnl.Tag)(0)

            If pnl.Controls(txtHTMLBlockDescription.Name).Text <> Block.Item("Description") Or pnl.Controls(txtHTMLBlockHTML.Name).Text <> Block.Item("HTML") Or pnl.Controls(txtHTMLBlockSQL.Name).Text <> ReplaceNull(Block.Item("SQLQuery"), String.Empty) Then

                Dim sSQL As New StringBuilder
                sSQL.AppendLine("	update hrb set description = " & SQLQuote(pnl.Controls(txtHTMLBlockDescription.Name).Text) & ", Html = " & SQLQuote(pnl.Controls(txtHTMLBlockHTML.Name).Text) & ", SQLQuery = " & IIf(String.IsNullOrEmpty(pnl.Controls(txtHTMLBlockSQL.Name).Text), "null", SQLQuote(pnl.Controls(txtHTMLBlockSQL.Name).Text)))
                sSQL.AppendLine("	from dbo.HtmlReportBlocks hrb	")
                sSQL.AppendLine("	where hrb.id_HtmlBlock = " & CInt(pnl.Tag))


                Using newConn As New SqlConnection(FinesseConnectionString)
                    newConn.Open()
                    newConn.ExecuteNonQuery(sSQL)

                End Using

            End If
        Next

        RunHTMLReport()
    End Sub

    Private Sub scHTML_SplitterMoved(sender As Object, e As SplitterEventArgs) Handles scHTML.SplitterMoved
        Dim panels = flpHTML.Controls.OfType(Of Panel)
        For Each p As Panel In panels
            p.Width = scHTML.Panel1.Width - SystemInformation.VerticalScrollBarWidth - p.Margin.Horizontal - p.Padding.Horizontal
        Next
    End Sub

#End Region

#Region "Refresh Report"

    Protected Sub tsbRefresh_Click(sender As Object, e As EventArgs) Handles tsbRefresh.Click, tscboLanguage.SelectedIndexChanged
        If tscboLanguage.Text = String.Empty Then Return

        Dim aryParams = _sParameters.Split(";")
        For i = 0 To aryParams.Length - 1
            Dim param = aryParams(i)
            If param.Split("=")(0) = "id_language" Then
                param = String.Join("=", {"id_lanugage", _dtLanguage.Select("language=" & tscboLanguage.Text.SQLQuote)(0)("id_language").ToString})
            End If
            aryParams(i) = param
        Next
        _sParameters = String.Join(";", aryParams)

        RunReport()
    End Sub

    Public Sub RunReport(Optional ByVal AutoPrint As Boolean = False)
        If Not _initialized Or _suspendUpdates Then Return
        If GetHTMLReportBlocksDataSet() Is Nothing Then Return

        Try
            InitializeParameters(Parameters)
            dtReportHeaderInfo = get_dtReportHeaderInfo()
            RunHTMLReport()
            lblReportError.Visible = False
            If ExportImmediately And Not _alreadyExported And _FilePath IsNot String.Empty Then
                ExportHTMLReportToWord(False, False, True)
                _alreadyExported = True
            End If
        Catch ex As Exception
            'This will fail if the proper paramters are not present.
            lblReportError.Text = "Report Load Error: " & ex.Message
            lblReportError.Visible = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Protected Function get_dtReportHeaderInfo() As DataTable
        Dim newConn As New SqlConnection(FinesseConnectionString)
        newConn.Open()

        Dim SQLcmd As SqlCommand
        If Not String.IsNullOrEmpty(ReportHeaderOverrideCompany) Then
            SQLcmd = New SqlCommand("get_generic_report_header_by_company", newConn)
            SQLcmd.Parameters.Add("@CompanyCode", SqlDbType.VarChar).Value = _ReportHeaderOverrideCompany
            If _ReportHeaderAccountsLogoOverride Then
                SQLcmd.Parameters.Add("@AccountsLogoOverride", SqlDbType.VarChar).Value = _ReportHeaderAccountsLogoOverride
            End If
        ElseIf String.IsNullOrEmpty(_entityno) Or ReportHeaderIsEmployeeCompany Then
            SQLcmd = New SqlCommand("get_generic_report_header_by_user", newConn)
        Else
            SQLcmd = New SqlCommand("get_generic_report_header", newConn)
            SQLcmd.Parameters.Add("@entityno", SqlDbType.VarChar).Value = _entityno
        End If

        SQLcmd.CommandType = CommandType.StoredProcedure

        Dim dt As New DataTable
        Dim sqlAdpt As New SqlDataAdapter
        sqlAdpt.SelectCommand = SQLcmd
        sqlAdpt.Fill(dt)

        newConn.Close()
        newConn.Dispose()

        Return dt

    End Function

    Protected Sub RunHTMLReport()
        If Not _initialized Then Exit Sub

        Me.Cursor = Cursors.WaitCursor

        Dim sHTML As New StringBuilder
        Dim sSQL As New StringBuilder

        Dim filename As String = Get_myFilename(False)

        If Browser.Document IsNot Nothing Then
            Dim eBrowserVScroll = Browser.Document.GetElementsByTagName("HTML")
            If eBrowserVScroll.Count > 0 Then
                BrowserVScroll = Browser.Document.GetElementsByTagName("HTML")(0).ScrollTop
            End If
        End If

        Try

            Dim newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()

            sHTML.AppendLine("<!DOCTYPE html PUBLIC ""-//WC3//DTD XHTML 1.0 Transitional//EN"">")
            sHTML.AppendLine("<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />")
            sHTML.AppendLine("<html ")
            sHTML.AppendLine("xmlns:o='urn:schemas-microsoft-com:office:office'")
            sHTML.AppendLine("xmlns:w='urn:schemas-microsoft-com:office:word'")
            sHTML.AppendLine("xmlns='http://www.w3.org/TR/REC-html40'>")

            sHTML.AppendLine("	<!--[if gte mso 9]><xml>	")
            sHTML.AppendLine("	  <w:WordDocument>	")
            sHTML.AppendLine("	  <w:View>Print</w:View>	")
            sHTML.AppendLine("	  <w:Zoom>90</w:Zoom>	")
            sHTML.AppendLine("	  <w:DoNotOptimizeForBrowser/>	")
            sHTML.AppendLine("	  </w:WordDocument>	")
            sHTML.AppendLine("	</xml><![endif]-->	")

            sHTML.AppendLine("<style type=""text/css"">")
            sHTML.AppendLine(" .pagebreak {page-break-before:always;}")
            'sHTML.AppendLine(" @media print {div.PageHeader {position:fixed;display:block}}")
            ' sHTML.AppendLine(" @media print {div.PageContent {position:relative;display:block;padding-top:3.0cm;overflow:hidden;}}")
            sHTML.AppendLine(" p.bigpadingtop {padding-top:100px;}")
            sHTML.AppendLine(" span.tab {padding: 0 20px;}")
            sHTML.AppendLine(" span.tab_small {padding: 0 20px;}")
            sHTML.AppendLine(" .bb td, .bb th {border-bottom: 1px solid black !important;}")
            sHTML.AppendLine(" .bt td, .bt th {border-top: 1px solid black !important;}")
            sHTML.AppendLine(" .btdbb td, .btdbb th {border-top: 1px solid black !important;border-bottom: 2px solid black !important;}")
            sHTML.AppendLine(" .none td, .none th {border-style:none;}")
            sHTML.AppendLine(" .rb td {font-family: arial;border-radius: 8px;background: #73AD21;padding: 10px;}")
            sHTML.AppendLine("</style>")

            sHTML.AppendLine("<title>" & filename & "</title>")
            sHTML.AppendLine("<body>")

            sHTML.AppendLine("<div class=""PageHeader"">")
            sHTML.AppendLine("</div>")

            sHTML.AppendLine("<div class=""PageContent"">")

            Dim ixSQLResultTypes As New Dictionary(Of Integer, SQLResultType)

            For Each pnl As Panel In flpHTML.Controls.OfType(Of Panel)
                Dim id_HTMLBlock = pnl.Tag

                If ExcludeHTMLBlocks IsNot Nothing Then
                    If Not Array.IndexOf(ExcludeHTMLBlocks, id_HTMLBlock) = -1 Then Continue For
                End If

                Dim HTMLBlock = dtHtmlBlocks.Select("id_HtmlBlock=" & id_HTMLBlock)(0)

                If HTMLBlock("id_SectionType") <> "GD" Then
                    sHTML.AppendLine(pnl.Controls(txtHTMLBlockHTML.Name).Text)
                End If

                If Not String.IsNullOrEmpty(pnl.Controls(txtHTMLBlockSQL.Name).Text.Trim) Then
                    Dim sHTMLBlockSQL As String = pnl.Controls(txtHTMLBlockSQL.Name).Text

                    For Each p In ReplaceParameters
                        sHTMLBlockSQL = Replace(sHTMLBlockSQL, p.Key, p.Value.SQLQuote)
                    Next

                    sSQL.AppendLine(sHTMLBlockSQL)

                    Dim SQLResultType As New SQLResultType
                    SQLResultType.id_HtmlBlock = id_HTMLBlock
                    SQLResultType.ResultTypeCode = HTMLBlock("ResultTypeCode")

                    ixSQLResultTypes.Add(ixSQLResultTypes.Count, SQLResultType)
                End If

            Next

            sHTML.AppendLine("</div>")


            Dim foo = sSQL.ToString

            If Not String.IsNullOrEmpty(sSQL.ToString) Then

                Dim dsSQL = newConn.GetDataSet(sSQL)

                sHTML.AppendLine("</body></html>")

                For Each table As DataTable In dsSQL.Tables

                    Dim TableIndex = dsSQL.Tables.IndexOf(table)
                    Dim SQLResultType = ixSQLResultTypes(TableIndex)

                    Select Case SQLResultType.ResultTypeCode
                        Case "S" 'Scalar
                            For Each column As DataColumn In table.Columns
                                If table.Rows.Count = 1 Then
                                    sReportTitle = Replace(sReportTitle, "@" & column.ColumnName, ReplaceNull(table.Select()(0)(column.ColumnName), String.Empty))
                                    sHTML.Replace("@" & column.ColumnName, myHTMLFormat(ReplaceNull(table.Select()(0)(column.ColumnName), String.Empty), SQLResultType.id_HtmlBlock, column.ColumnName))
                                End If
                                If table.Rows.Count > 1 Then
                                    Dim sReplacement = String.Empty
                                    For Each row As DataRow In table.Rows
                                        sReplacement &= row.Item(column.ColumnName) & "<br>"
                                    Next
                                    sHTML.Replace("@" & column.ColumnName, sReplacement)
                                End If
                            Next
                        Case "DT" ' DataTable
                            Debug.Print("Got a datable!")

                            Dim xHTMLBlock = dtHtmlBlocks.Select("id_HtmlBlock=" & SQLResultType.id_HtmlBlock)(0)
                            Dim SectionDescription = xHTMLBlock.Item("Description")
                            Dim SectionType = xHTMLBlock.Item("id_SectionType")

                            Dim GroupBy = dtHtmlBlocksGroupBy.Select("id_HtmlBlock=" & SQLResultType.id_HtmlBlock)

                            Dim sHTMLReplacement As New StringBuilder

                            Select Case GroupBy.Count
                                Case 0
                                    Debug.Print("No DataTable Grouping here...")
                                    Dim DetailHTMLBlocks = dtHtmlBlocks.Select("id_SectionType='GD' and Description=" & SQLQuote(SectionDescription))

                                    If DetailHTMLBlocks.Length > 0 Then
                                        Debug.Print("Found " & DetailHTMLBlocks.Length & " DetailHTMLBlock Section(s) for " & SectionDescription & "!")

                                        Dim newHTMLLine As String = xHTMLBlock.Item("HTML")

                                        Dim newDetailHTMLLine = String.Empty

                                        For Each DetailHTMLBlock In DetailHTMLBlocks
                                            Dim HTMLRows As New StringBuilder
                                            For Each row As DataRow In table.Rows
                                                Dim HTMLRow As String = DetailHTMLBlock.Item("HTML")
                                                For Each col As DataColumn In table.Columns
                                                    HTMLRow = Replace(HTMLRow, "@" & col.ColumnName, myHTMLFormat(ReplaceNull(row.Item(col), ""), DetailHTMLBlock.Item("id_HtmlBlock"), col.ColumnName))
                                                Next
                                                HTMLRows.AppendLine(HTMLRow)
                                            Next
                                            newDetailHTMLLine &= HTMLRows.ToString

                                            'sHTML.AppendLine(newDetailHTMLLine)
                                        Next

                                        newHTMLLine = Replace(newHTMLLine, "<!--GROUP DATA-->", newDetailHTMLLine) & "<br>"

                                        sHTMLReplacement.AppendLine(newHTMLLine)

                                        sHTML.Replace(xHTMLBlock.Item("HTML"), sHTMLReplacement.ToString)

                                    End If

                                Case Else
                                    For Each group In GroupBy
                                        Dim GroupByField = group.Item("GroupByField")
                                        Dim seqno = group.Item("seqno")
                                        Debug.Print("DataTable Group: " & seqno & " - " & GroupByField)

                                        Dim Summaries = dtHtmlBlocksSummaries.Select("id_HtmlBlock=" & SQLResultType.id_HtmlBlock & " and seqno=" & seqno)

                                        Dim groupItems = From r As DataRow In table.Rows
                                                         Select r.Item(GroupByField) Distinct

                                        For Each groupItem In groupItems
                                            Dim lGroupItem = groupItem
                                            Debug.Print(GroupByField & ": " & groupItem)

                                            Dim newHTMLLine As String = xHTMLBlock.Item("HTML")

                                            'HideHTMLReportElements(newHTMLLine, SectionDescription, False)


                                            For Each col As DataColumn In table.Columns
                                                For Each group2 In GroupBy
                                                    If col.ColumnName = group2.Item("GroupByField") Then
                                                        newHTMLLine = Replace(newHTMLLine, "@" & col.ColumnName, table.Select(GroupByField & "=" & SQLQuote(groupItem))(0)(col.ColumnName))
                                                    End If
                                                Next
                                            Next

                                            For Each Summary In Summaries
                                                Dim lColumnName = Summary.Item("ColumnName")
                                                Dim SummaryResult As Double
                                                Select Case Summary.Item("SummaryType")
                                                    Case "max"
                                                        Dim x = From r As DataRow In table.Rows
                                                                Where r.Item(GroupByField) = lGroupItem
                                                                Select r.Item(lColumnName)

                                                        SummaryResult = x.Max

                                                    Case "sum"

                                                        Dim x = From r As DataRow In table.Rows
                                                                Where r.Item(GroupByField) = lGroupItem
                                                                Select r

                                                        SummaryResult = x.Sum(Function(r) CDec(ReplaceNull(r.Item(lColumnName), 0.0)))

                                                    Case Else
                                                        'Do nothing
                                                End Select

                                                newHTMLLine = Replace(newHTMLLine, "@" & lColumnName, myHTMLFormat(SummaryResult, SQLResultType.id_HtmlBlock, lColumnName))

                                            Next

                                            'Find the matching detail HTML block
                                            Dim DetailHTMLBlocks = dtHtmlBlocks.Select("id_SectionType='GD' and Description=" & SQLQuote(SectionDescription))

                                            If DetailHTMLBlocks.Length > 0 Then
                                                Debug.Print("Found " & DetailHTMLBlocks.Length & " DetailHTMLBlock Section(s) for " & SectionDescription & "!")

                                                Dim newDetailHTMLLine = String.Empty

                                                For Each DetailHTMLBlock In DetailHTMLBlocks
                                                    Dim HTMLRows As New StringBuilder
                                                    For Each row As DataRow In table.Select(GroupByField & "=" & SQLQuote(groupItem))
                                                        Dim HTMLrow As String = DetailHTMLBlock.Item("HTML")


                                                        'HideHTMLReportElements(HTMLrow, SectionDescription, True)


                                                        For Each col As DataColumn In table.Columns
                                                            HTMLrow = Replace(HTMLrow, "@" & col.ColumnName, myHTMLFormat(ReplaceNull(row.Item(col), String.Empty), DetailHTMLBlock.Item("id_HtmlBlock"), col.ColumnName))
                                                        Next

                                                        HTMLRows.AppendLine(HTMLrow)
                                                    Next
                                                    newDetailHTMLLine &= HTMLRows.ToString

                                                    sHTML.Replace(DetailHTMLBlock.Item("HTML"), String.Empty)
                                                Next

                                                newHTMLLine = Replace(newHTMLLine, "<!--GROUP DATA-->", newDetailHTMLLine) & "<br>"

                                            End If

                                            sHTMLReplacement.AppendLine(newHTMLLine)

                                        Next 'GroupItem in GroupItems

                                        sHTML.Replace(xHTMLBlock.Item("HTML"), sHTMLReplacement.ToString)

                                    Next 'Group in GroupBy
                            End Select

                        Case "DS" ' DataSet
                            'Do nothing
                    End Select

                Next 'Section SQL

            End If

            Dim lstTextObjects As New Dictionary(Of String, String)
            lstTextObjects.Add("report_title", dtHtmlBlocks.Rows(0).Item("TemplateName"))
            lstTextObjects.Add("entityno", _entityno)
            lstTextObjects.Add("entitydesc", _entitydesc)
            lstTextObjects.Add("ESSVBDir", ESSVBDir)

            For Each txt In lstTextObjects
                If txt.Value IsNot Nothing Then sHTML.Replace("@" & txt.Key, txt.Value.Replace(vbLf, "<br>")) 'Replaces carriage returns with HTML line breaks
            Next

            For Each p In ReplaceParameters
                sHTML.Replace(p.Key, p.Value)
            Next

            sHTML.Replace("@imgfilepath_Signature", _MySignatureImagePath)

            'Dim FieldTranslations = _dtReportFieldTranslations.Select("id_language=" & cboLanguage.SelectedValue)
            Dim FieldTranslations = _dtReportFieldTranslations.Select("id_language=" & GetLanguageID())
            For Each field In FieldTranslations
                sHTML.Replace("@" & field.Item("field"), field.Item("translation"))
            Next

            Try
                'Below doc.Write required to immediately send sHTML to Browser.DocumentText
                Browser.Navigate("about:blank")
                Do Until Browser.ReadyState = WebBrowserReadyState.Complete
                    Application.DoEvents()
                    System.Threading.Thread.Sleep(250)
                Loop
                Dim doc As HtmlDocument = Browser.Document
                doc.Write(sHTML.ToString)
                Browser.DocumentText = sHTML.ToString
            Catch ex As Exception

            End Try


            newConn.Close()
            newConn.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Me.Cursor = Cursors.Default

    End Sub

#End Region

#Region "Export"
    Protected Sub ExportToWordToolStripMenuItem_Click(sender As System.Object, e As System.EventArgs) Handles tsbExportHTMLReportToWord.Click, tsbExportHTMLReportToPDF.Click
        Dim tsb = TryCast(sender, ToolStripButton)
        If tsb Is tsbExportHTMLReportToWord Then
            RaiseEvent exportingDoc() 'raises event so you can handle the event and give a unique file name in the same instance of the report
            ExportHTMLReportToWord()
        Else
            RaiseEvent exportingPDF()
            ExportHTMLReportToWord(True)
        End If
    End Sub

    Protected Function ExportHTMLReportToWord(Optional pdf As Boolean = False, Optional isTemp As Boolean = False, Optional ByVal isSaveToFinesseDataFolders As Boolean = False) As String

        Dim filename As String = Get_myFilename()
        Dim saveFileName As String = String.Empty
        Dim myTempFilePath As String = String.Empty
        Dim testTempFilePath As String = String.Empty
        Dim testTempFilePathIterator As Integer = 1

        If Not _FilePath = String.Empty And _dtuserinfo.Rows(0).Item("AutoInvoiceGeneration") = "A" Then
            If pdf Then
                _FilePath = Path.ChangeExtension(_FilePath, ".pdf")
                saveFileName = _FilePath
            Else
                _FilePath = Path.ChangeExtension(_FilePath, ".doc")
                saveFileName = _FilePath
            End If
        Else
            If isTemp = False Then
                Dim sfd As New SaveFileDialog
                sfd.OverwritePrompt = True
                sfd.FileName = filename
                sfd.DefaultExt = IIf(pdf, "pdf", "doc")
                sfd.ValidateNames = True

                sfd.Filter = IIf(pdf, "PDF files (*.pdf)|*.pdf", "Word Document files (*.doc)|*.doc")

                Dim result = sfd.ShowDialog()

                If result = DialogResult.Cancel OrElse sfd.FileName.Length = 0 Then
                    Return Nothing
                    'Canceled
                End If

                saveFileName = sfd.FileName
            End If
        End If

        Dim temppath = Environment.GetEnvironmentVariable("TEMP")

        myTempFilePath = temppath & "\" & filename
        testTempFilePath = myTempFilePath


TryTempFilePath:

        If System.IO.File.Exists(testTempFilePath) Then
            testTempFilePathIterator += 1
            testTempFilePath = myTempFilePath & " (" & testTempFilePathIterator.ToString & ")"
            GoTo TryTempFilePath
        End If

        myTempFilePath = testTempFilePath

        If isTemp = False Then
            If FileInUse(saveFileName) Then
                MsgBox("The file you're trying to save is already open.  Close all instances of Microsoft Word, then try Exporting again." & vbCrLf & vbCrLf & "If you don't see any instances of Microsoft Word open, check the Task Manager to see if any background Microsoft Word, MSWord, WinWord, or Word processes are currently running.  If so, End those processes, then try again.", vbOKOnly)
                Return Nothing
            End If
        End If

        Dim myfilestream As New System.IO.FileStream(myTempFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None)

        Dim mywriter As New System.IO.StreamWriter(myfilestream)
        mywriter.WriteLine(Browser.DocumentText)
        mywriter.Close()
        myfilestream.Close()

        Me.Cursor = Cursors.WaitCursor

        Dim strProgressMessage As New StringBuilder
        strProgressMessage.AppendLine("Temp file written to " & myTempFilePath & ".")

        Dim oTmpWord As New Word.Application

        Dim Unknown As Object = Type.Missing

        Try
            oTmpWord.Visible = False

            oTmpWord.Documents.Open(myTempFilePath)

            oTmpWord.Application.Visible = False

            ' fix for "you are not allowed to edit this selection because it is protected" error.
            ' Sometimes the document opens in Reading Layout, which screws up permissions
            CType(oTmpWord.ActiveDocument, Object).ActiveWindow.View.ReadingLayout = False

            strProgressMessage.AppendLine("Temp Word opened Temp Doc.")

            With oTmpWord.ActiveDocument
                Select Case IIf(Get_AppConfigSetting(_dtAppConfig, "UseNarrowMargins") IsNot Nothing, Get_AppConfigSetting(_dtAppConfig, "UseNarrowMargins"), False)
                    Case True
                        .PageSetup.LeftMargin = oTmpWord.CentimetersToPoints(3.0)
                        .PageSetup.RightMargin = oTmpWord.CentimetersToPoints(2.0)
                    Case False
                        .PageSetup.LeftMargin = oTmpWord.InchesToPoints(0.25)
                        .PageSetup.RightMargin = oTmpWord.InchesToPoints(0.25)
                End Select

                .PageSetup.TopMargin = oTmpWord.InchesToPoints(0.25)
                .PageSetup.BottomMargin = oTmpWord.InchesToPoints(0.25)
                .PageSetup.HeaderDistance = oTmpWord.InchesToPoints(0.25)
                .PageSetup.FooterDistance = oTmpWord.InchesToPoints(0.25)
            End With

            strProgressMessage.AppendLine("Margins adjusted.")

            Dim rngHeader As Word.Range = oTmpWord.ActiveDocument.Sections(1).Headers(Word.WdHeaderFooterIndex.wdHeaderFooterPrimary).Range
            rngHeader.Font.Name = "Arial"
            rngHeader.Font.Size = 8

            Dim rngFooter As Word.Range = oTmpWord.ActiveDocument.Sections(1).Footers(Word.WdHeaderFooterIndex.wdHeaderFooterPrimary).Range
            rngFooter.Font.Name = "Arial"
            rngFooter.Font.Size = 8

            Dim CompanyInfo As New StringBuilder()

            CompanyInfo.AppendLine(dtReportHeaderInfo.Rows(0).Item("company_address"))
            CompanyInfo.AppendLine(dtReportHeaderInfo.Rows(0).Item("company_contact"))
            CompanyInfo.AppendLine(dtReportHeaderInfo.Rows(0).Item("web_address"))
            CompanyInfo.AppendLine() 'One blank space per request from AR/Alain.

            oTmpWord.ActiveDocument.Tables.Add(rngHeader, 2, IIf(dtReportHeaderInfo.Rows(0).Item("HideHeaderAddress") = True, 1, 2))
            'oTmpDoc.Tables.Add(rngFooter, 1, 2) 'This is for putting the doc title in the footer... looks awful.
            oTmpWord.ActiveDocument.Tables.Add(rngFooter, 1, 1)

            If Not IIf(Get_AppConfigSetting(_dtAppConfig, "HideHeader") IsNot Nothing, Get_AppConfigSetting(_dtAppConfig, "HideHeader"), False) = True Then
                If IIf(Get_AppConfigSetting(_dtAppConfig, "UseNarrowMargins") IsNot Nothing, Get_AppConfigSetting(_dtAppConfig, "UseNarrowMargins"), False) = True And IIf(Get_AppConfigSetting(_dtAppConfig, "FarLeftHeader") IsNot Nothing, Get_AppConfigSetting(_dtAppConfig, "FarLeftHeader"), False) = True Then
                    rngHeader.Tables(1).Rows.SetLeftIndent(-70, Word.WdRulerStyle.wdAdjustFirstColumn)
                End If

                With rngHeader.Tables.Item(1).Cell(1, 1).Range
                    Try
                        Dim logopath = Environ("ESSVBDir") & "\images\" & dtReportHeaderInfo.Rows(0).Item("company_logo_filename")
                        Dim logo = rngHeader.InlineShapes.AddPicture(logopath, False, True, rngHeader)
                    Catch ex As Exception

                    End Try
                    .ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft
                End With

                If Not dtReportHeaderInfo.Rows(0).Item("HideHeaderAddress") Then
                    With rngHeader.Tables.Item(1).Cell(1, 2).Range
                        .Text = vbCrLf & vbCrLf & dtReportHeaderInfo.Rows(0).Item("PublicName") 'Two blank spaces per request from AR/Alain.
                        .ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight
                    End With

                    With rngHeader.Tables.Item(1).Cell(2, 2).Range
                        .Text = CompanyInfo.ToString
                        .ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphRight
                    End With

                    rngHeader.Tables.Item(1).Columns(1).Cells.Merge()
                End If

            Else
                rngHeader.Tables.Item(1).Rows(1).Height = oTmpWord.CentimetersToPoints(3)
            End If

            strProgressMessage.AppendLine("Header created.")

            'If Not String.IsNullOrEmpty(dtReportHeaderInfo.Rows(0).Item("FooterFilename")) Then
            '    With rngFooter.Tables.Item(1).Cell(1, 1).Range
            '        Try
            '            Dim footerpath = Environ("ESSVBDir") & "\images\" & dtReportHeaderInfo.Rows(0).Item("FooterFilename")
            '            Dim footer = rngFooter.InlineShapes.AddPicture(footerpath, False, True, rngFooter)
            '        Catch ex As Exception

            '        End Try
            '    End With
            'End If

            Dim PageFromTranslation = _dtReportFieldTranslations.Select("field='PageFrom' and id_language=" & GetLanguageID())(0)("translation")
            Dim PageToTranslation = _dtReportFieldTranslations.Select("field='PageTo' and id_language=" & GetLanguageID())(0)("translation")

            rngFooter.Fields.Add(rngFooter, , "NumPages", False)
            rngFooter.InsertBefore(" " & PageToTranslation & " ")
            rngFooter.Collapse(Word.WdCollapseDirection.wdCollapseStart)
            rngFooter.Fields.Add(rngFooter, , "Page", False)
            'rngFooter.InsertBefore(vbTab & PageFromTranslation & " ")
            rngFooter.InsertBefore(PageFromTranslation & " ")
            rngFooter.Collapse(Word.WdCollapseDirection.wdCollapseStart)

            rngFooter.InsertBefore(Get_myFilename() & " - ")
            rngFooter.Collapse(Word.WdCollapseDirection.wdCollapseStart)

            rngFooter.InlineShapes.AddHorizontalLineStandard()

            If Not dtReportHeaderInfo.Rows(0).Item("FooterFilename") Is DBNull.Value AndAlso Not String.IsNullOrEmpty(dtReportHeaderInfo.Rows(0).Item("FooterFilename")) Then
                Try
                    Dim footerpath = Environ("ESSVBDir") & "\images\" & dtReportHeaderInfo.Rows(0).Item("FooterFilename")
                    'Dim footer = rngFooter.InlineShapes.AddPicture(footerpath, False, True, rngFooter)
                    rngFooter.InlineShapes.AddPicture(footerpath, False, True, rngFooter)
                Catch ex As Exception

                End Try
            End If

            'rngFooter.InlineShapes.AddPicture(Environ("ESSVBDir") & "\images\" & dtReportHeaderInfo.Rows(0).Item("FooterFilename"), False, True, rngFooter)

            strProgressMessage.AppendLine("Footer created.")

            Dim d As New PrintDocument

            Select Case d.DefaultPageSettings.PaperSize.RawKind
                Case 1
                    oTmpWord.ActiveDocument.PageSetup.PaperSize = Word.WdPaperSize.wdPaperLetter
                Case 9
                    oTmpWord.ActiveDocument.PageSetup.PaperSize = Word.WdPaperSize.wdPaperA4
                Case Else
                    Debug.Assert(False)
            End Select

            strProgressMessage.AppendLine("Paper Size adjusted.")

            'Document is prepared.  Ready to either save/open or export to .pdf

            If isTemp = False Then
                oTmpWord.ActiveDocument.SaveAs(saveFileName, IIf(pdf, Word.WdSaveFormat.wdFormatPDF, Word.WdSaveFormat.wdFormatDocumentDefault), Unknown, Unknown, True, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown)
                strProgressMessage.AppendLine("Save as " & saveFileName & " successful.")
            Else
                If pdf = True Then
                    oTmpWord.ActiveDocument.SaveAs(Replace(myTempFilePath, ".doc", ".pdf"), Word.WdSaveFormat.wdFormatPDF, Unknown, Unknown, True, Unknown, Unknown, Unknown, Unknown, Unknown, Unknown)
                    strProgressMessage.AppendLine("Save as " & myTempFilePath & " successful.")
                End If
            End If

            'oTmpWord.Documents.Close(False, Unknown, Unknown)
            oTmpWord.Quit(False, Unknown, Unknown)

            System.Runtime.InteropServices.Marshal.ReleaseComObject(rngFooter)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(rngHeader)
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oTmpWord)

            oTmpWord = Nothing

            strProgressMessage.AppendLine("Temp Doc closed And Temp Word quitted.")

            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()

            strProgressMessage.AppendLine("GC Collect successful.")

            My.Computer.FileSystem.DeleteFile(myTempFilePath)

            If isTemp = False And isSaveToFinesseDataFolders = False Then
                Process.Start(saveFileName)
                strProgressMessage.AppendLine("Real file opened at " & saveFileName & ".")
            End If

            strProgressMessage.AppendLine("---")

        Catch ex As Exception
            MsgBox(strProgressMessage.ToString & ex.Message)
            Return Nothing
        Finally
            If oTmpWord IsNot Nothing Then oTmpWord.Quit()
            oTmpWord = Nothing

            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()
        End Try

        Me.Cursor = Cursors.Default

        If isTemp = False Then
            Return saveFileName
        Else
            Return myTempFilePath & "." & IIf(pdf, "pdf", "doc")
        End If

    End Function

    Protected Sub tsbEmailHTMLReport_Click(sender As Object, e As EventArgs) Handles tsbEmailHTMLReport.Click, tsbEmailWithPDF.Click
        EmailReport(TryCast(sender, ToolStripButton).Name = tsbEmailWithPDF.Name)
    End Sub

    Private Sub EmailReport(Optional ByVal AttachPDF As Boolean = False)

        Dim myFileName = Get_myFilename(False)
        Dim myFilePath = System.IO.Path.GetTempPath & myFileName & ".htm"
        Dim emailbodyHTML = "<br><br>"

        Try
            If AttachPDF Then
                Dim TempAttachmentPath = ExportHTMLReportToWord(True, True)

                emailbodyHTML &= EmailBody

                OpenOutlookMail(Recipients, myFileName, emailbodyHTML, {TempAttachmentPath}.ToList(), True, CopyRecipients)
                My.Computer.FileSystem.DeleteFile(TempAttachmentPath)

                If _FilePath IsNot String.Empty Then
                    ExportHTMLReportToWord(True, False, True)
                End If
            Else
                Dim myfilestream As New System.IO.FileStream(myFilePath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None)

                Dim mywriter As New System.IO.StreamWriter(myfilestream)
                mywriter.WriteLine(Browser.DocumentText)
                mywriter.Close()
                myfilestream.Close()

                emailbodyHTML &= EmailBody & "<br><br>" & Browser.DocumentText
                emailbodyHTML = Replace(emailbodyHTML, "<br clear=""all"" class=pagebreak>", "")

                OpenOutlookMailHTMLBody(Recipients, myFileName, emailbodyHTML, Nothing, CopyRecipients)

                If _FilePath IsNot String.Empty Then
                    ExportHTMLReportToWord(True, False, True)
                End If
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub tsbPrintPreview_Click(sender As Object, e As EventArgs) Handles tsbPrintPreview.Click
        If tsbUseAppPrinter.Checked Then CheckForAppPrinter()

        If String.IsNullOrEmpty(Browser.DocumentText) Then
            MsgBox("Nothing has been selected to be printed.")
            Return
        End If

        Try
            HideBrowserHeaderAndFooter()
            Browser.ShowPrintPreviewDialog()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub tsbPrint_Click(sender As Object, e As EventArgs) Handles tsbPrint.Click
        If tsbUseAppPrinter.Checked Then CheckForAppPrinter()

        If String.IsNullOrEmpty(Browser.DocumentText) Then
            MsgBox("Nothing has been selected to be printed.")
            Return
        End If

        Try
            HideBrowserHeaderAndFooter()

            Dim prntDoc As New PrintDocument
            Dim sCurrentPrinter As String
            sCurrentPrinter = prntDoc.PrinterSettings.PrinterName

            Dim wsNetwork As Object
            wsNetwork = Microsoft.VisualBasic.CreateObject("WScript.Network")

            If tsbUseAppPrinter.Checked Then
                wsNetwork.SetDefaultPrinter(AppPrinterName)
            End If

            Browser.Print()

            If tsbUseAppPrinter.Checked Then
                wsNetwork.SetDefaultPrinter(sCurrentPrinter)
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub HideBrowserHeaderAndFooter()
        Dim key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(WebBrowserPageSetupKeyName, True)
        If key IsNot Nothing Then
            strWebBrowserRestoreHeader = key.GetValue("header")
            strWebBrowserRestoreFooter = key.GetValue("footer")
            key.SetValue("header", "")
            key.SetValue("footer", "")
            'key.SetValue("footer", old_footer) ' this is moved to FormClosing because when it lives here, it reinserts the footer too quickly
        End If
    End Sub

    Private Sub HtmlReport_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        Try
            Dim key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(WebBrowserPageSetupKeyName, True)
            If strWebBrowserRestoreHeader IsNot Nothing Then key.SetValue("header", strWebBrowserRestoreHeader)
            If strWebBrowserRestoreFooter IsNot Nothing Then key.SetValue("footer", strWebBrowserRestoreFooter)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tsbSaveDefaultPrinter_Click(sender As Object, e As EventArgs) Handles tsbUseAppPrinter.Click
        If tsbUseAppPrinter.Checked Then
            CheckForAppPrinter()
        End If
    End Sub

    Private Sub CheckForAppPrinter()
        If String.IsNullOrEmpty(AppPrinterName) Then
            MsgBox("Please select the printer you will use for printing " & My.Application.Info.ProductName & " reports.  This will Not change your computer's default printer settings.")
            SetAppPrinter()
        End If
    End Sub

    Private Sub SetAppPrinter()
        Dim pd As New PrintDialog
        pd.ShowDialog()
        AppPrinterName = pd.PrinterSettings.PrinterName
    End Sub

    Private Sub tsbPrintBitmap_Click(sender As Object, e As EventArgs) Handles tsbPrintBitmap.Click
        PrintBitmap()
    End Sub

    Public Sub PrintBitmap()
        If tsbUseAppPrinter.Checked Then CheckForAppPrinter()

        Browser.Refresh()

        If String.IsNullOrEmpty(Browser.DocumentText) Then
            MsgBox("Nothing has been selected to be printed.")
            Return
        End If

        If String.IsNullOrEmpty(AppPrinterName) Then
            MsgBox("Please set an App Printer for printing Bitmaps of the report.")
            Return
        End If

        Try
            Dim myPrintDocument = PreparePrintDocument()
            myPrintDocument.Print()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Function PreparePrintDocument() As PrintDocument
        ' Make the PrintDocument object.
        Dim print_document As New PrintDocument

        print_document.PrinterSettings.PrinterName = AppPrinterName
        'print_document.DefaultPageSettings.Landscape = True
        print_document.DocumentName = sReportTitle

        AddHandler print_document.PrintPage, AddressOf myPrintDocument_PrintPage

        ' Return the object.
        Return print_document
    End Function

    Private Sub myPrintDocument_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs)
        Dim Margin As Integer = 15
        Dim Point As New Point(0, 0)

        If _BitmapPrintSize = Nothing Then
            MsgBox("No Bitmap Print Size has been supplied.", vbOKOnly)
            Return
        End If

        Dim bmp As New Bitmap(BitmapPrintSize.Width + Margin * 2, BitmapPrintSize.Height + Margin * 2)
        bmp.SetResolution(e.Graphics.DpiX, e.Graphics.DpiY)
        Browser.DrawToBitmap(bmp, New Rectangle(Point.X, Point.Y, BitmapPrintSize.Width + Margin * 2, BitmapPrintSize.Height + Margin * 2))

        e.Graphics.DrawImage(bmp, Point)
#If DEBUG Then
        bmp.Save("C:\Users\jheitmann\Desktop\" & sReportTitle & ".bmp")
#End If

    End Sub

    Private Sub tsbSetAppPrinter_Click(sender As Object, e As EventArgs) Handles tsbSetAppPrinter.Click
        SetAppPrinter()
    End Sub

    Public Function FileInUse(ByVal sFile As String) As Boolean
        Dim thisFileInUse As Boolean = False
        If System.IO.File.Exists(sFile) Then
            Try
                Using f As New IO.FileStream(sFile, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite, System.IO.FileShare.None)
                    ' thisFileInUse = False
                End Using
            Catch
                thisFileInUse = True
            End Try
        End If
        Return thisFileInUse
    End Function

#End Region

    Private Sub chkIsGroupData_CheckedChanged(sender As Object, e As EventArgs) Handles chkIsGroupData.CheckedChanged
        If _suspendUpdates Then Return

        Dim chk As CheckBox = TryCast(sender, CheckBox)
        Dim pnl As Panel = chk.Parent
        Dim id_HtmlBlock As Integer = pnl.Tag

        Dim sSQL As New StringBuilder
        sSQL.AppendLine("	update hrb set hrb.id_SectionType = " & SQLQuote(IIf(chk.Checked, "GD", "GH")))
        sSQL.AppendLine("	from dbo.HtmlReportBlocks hrb	")
        sSQL.AppendLine("	where hrb.id_HtmlBlock = " & id_HtmlBlock)

        Using newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()
            newConn.ExecuteNonQuery(sSQL)
        End Using

    End Sub

End Class
