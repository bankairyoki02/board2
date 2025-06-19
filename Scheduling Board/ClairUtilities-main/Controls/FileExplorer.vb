Imports System.Collections.Specialized
Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.AccessControl
Imports System.Security.Principal
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Office.Interop.Outlook
Imports Microsoft.VisualBasic.FileIO

Public Class FileExplorer

    Private _AttachmentCategory As String
    Private _currentGUID As String
    Private currentGlobalOpsFolder As String ''Default Path + Part/Project root folder + Currently Selected folder in the treeview (I.E. \\clair\s\Global Operations\FinesseData\5e965b4b-d6ab-43a5-969e-7f351ee7d256\Manuals)
    Private _attachmentCategorySubFolderPath As String 'Default Path + Part/Project root folder (I.E. \\clair\s\Global Operations\FinesseData\5e965b4b-d6ab-43a5-969e-7f351ee7d256)

    Private _displayArchivedFolders As Boolean = False

    Private GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY As String
    Private DROPBOX_ROOT_DIRECTORY
    Private FINEESSE_FILE_STORAGE_FAVORITES_FOLDER_NAME As String

    Dim newConn As New SqlConnection(FinesseConnectionString)


    Private _isLoadingTree As Boolean = False

    Private WithEvents _fsw As FileSystemWatcher
    Private _Timer1 As New Timers.Timer
    Private _LiveUpdateThread As System.Threading.Thread

    Private _dtUserFolders As DataTable
    Private _dtAttachmentTypes As DataTable
    Private _dtAttachmentTypesCloudTemplates As DataTable
    Private _dtCloudFolderTemplatesIDLevel As DataTable
    Private _dtPermissions As DataTable
    Private _dtContactCategoryToIDLevelandFolderTemplate As DataTable
    Private _dtRootAttachmentTypes As DataTable
    Private _dtUploadedFiles As DataTable
    Private _dtProjectShareRequests As DataTable
    Private _isUploadedToDropboxColumnRemoved As Boolean
    Private _dtFileIndex As DataTable
    Private _dtCurrentAccess As DataTable

    Private _dtAvailableInvitees As DataTable

    Private _projectDesc As String
    Private currentEntityno As String

    Private _partDesc As String
    Private currentPartNo As String

    Private fileNotDisplayed As Integer = 0

    Private _previousNode As TreeNode

    Private _canAddToKnowledgebase As Boolean

    Public Property DropboxEnabled As Boolean = False

    Public Event noGUID() 'cant rename event becuase then it would break parent forms
    Public Event rootFolderBeingCreated()

    Private _EgnyteSharingURI As String

    Public Property GlobalOpsFinesseDataRootDirectory As String

    Public Property currentGUID As String
        Get
            currentGUID = _currentGUID
        End Get
        Set(ByVal value As String)
            _currentGUID = value
        End Set
    End Property

    Public Property attachmentCategorySubFolderPath As String
        Get
            attachmentCategorySubFolderPath = _attachmentCategorySubFolderPath
        End Get
        Set(ByVal value As String)
            _attachmentCategorySubFolderPath = value
        End Set
    End Property

    Public Property attachmentCategory As String
        Get
            attachmentCategory = _AttachmentCategory
        End Get
        Set(ByVal value As String)
            _AttachmentCategory = value
        End Set
    End Property

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        Load_FileExplorer()
    End Sub

    Public Sub New(ByVal newAttachmentCategory As String, Optional ByVal isDropboxEnabled As Boolean = False, Optional ByVal GUID As String = "", Optional ByVal Desc As String = "")
        MyBase.New()

        ' This call is required by the designer.
        InitializeComponent()

        If GUID = "" Then
            RaiseEvent noGUID()
        End If

        _currentGUID = GUID
        _AttachmentCategory = newAttachmentCategory
        _projectDesc = Desc

        DropboxEnabled = isDropboxEnabled

        Load_FileExplorer()
    End Sub

    Public Sub Load_FileExplorer(Optional ByVal GUID As String = "")
        gbSharingNote.Hide()
        Dim jammer As New SQLJammer(GetOpenedFinesseConnection())

        jammer.Add($"SELECT ac.AttachmentCategory, ac.attachmentsCanBeInKnowledgeBase, is_rolemember('KnowledgeBaseEditors') as CanEditKnowledgeBase, ac.defaultRootFolderPath
                        from dbo.AttachmentCategory AS ac
                        WHERE AttachmentCategory = {_AttachmentCategory.SQLQuote}",
                          Sub(t)
                              If t.Rows(0).Item("CanEditKnowledgeBase") Is DBNull.Value Then
                                  _canAddToKnowledgebase = False
                              Else
                                  If t.Rows(0).Item("attachmentsCanBeInKnowledgeBase") And t.Rows(0).Item("CanEditKnowledgeBase") Then
                                      _canAddToKnowledgebase = True
                                  Else
                                      _canAddToKnowledgebase = False
                                  End If
                              End If

                              GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY = t.Rows(0).Item("defaultRootFolderPath")
                              GlobalOpsFinesseDataRootDirectory = t.Rows(0).Item("defaultRootFolderPath")
                          End Sub)
        jammer.Add("SELECT s.Value 
                    FROM dbo.SysConfig s
                    WHERE s.Tag = 'FINEESSE_FILE_STORAGE_FAVORITES_FOLDER_NAME' ",
                          Sub(t)
                              FINEESSE_FILE_STORAGE_FAVORITES_FOLDER_NAME = t.Rows(0).Item("Value")
                          End Sub)
        jammer.Add($"SELECT act.AttachmentCategory, at.AttachmentTypeDescription, Permissionsneeded = is_rolemember(DatabaseRole), act.AttachmentType, atb.DatabaseRole,
						 CASE ISNULL (is_rolemember(DatabaseRole), 1)
						 WHEN 0 THEN 0
						 ELSE 1
						 END AS hasPermissions, at.CanHaveExpirationDate, at.Parent, dbo.fn_get_attachmentTypeFullPath(at.AttachmentType) AS FullAttachmentTypePath
                        ,at.TreeOrder                         
                        FROM dbo.AttachmentCategoryTypes act
                         JOIN dbo.AttachmentTypes at ON act.AttachmentType = at.AttachmentType
						 LEFT OUTER JOIN dbo.AttachmentTypeDatabaseRoles atb ON atb.AttachmentType = at.AttachmentType
                         WHERE act.AttachmentCategory =  {SQLQuote(_AttachmentCategory)}
                            ORDER BY Parent, TreeOrder                         
                            ",
                         Sub(t)
                             _dtAttachmentTypes = t
                         End Sub)

        jammer.Add("SELECT gp.groupName, gp.AttachmentType, gp.allowFullControl, gp.allowModify, gp.[allowRead&Execute], gp.allowListFolderContents, gp.allowRead, gp.allowWrite, gp.allowSpecialPermissions, fpg.GroupPath
                         FROM dbo.GroupPermissions gp
                         JOIN dbo.FilePermissionsGroups fpg ON fpg.GroupName = gp.GroupName
                         JOIN dbo.AttachmentCategoryTypes act ON act.AttachmentType = gp.AttachmentType
                         WHERE act.AttachmentCategory = " & SQLQuote(_AttachmentCategory),
                         Sub(t)
                             _dtPermissions = t
                         End Sub)

        If DropboxEnabled Then

            jammer.Add("SELECT s.Value
                    FROM dbo.SysConfig s
                    WHERE s.Tag = 'DROPBOX_ROOT_DIRECTORY' ",
                  Sub(t)
                      DROPBOX_ROOT_DIRECTORY = t.Rows(0).Item("Value")
                  End Sub)

            jammer.Add("SELECT at.AttachmentTypeDescription, cfsgtat.AttachmentType, cfsgtat.CloudFolderTemplate, cfspft.DropboxFilePathSuffix
                         FROM dbo.CloudFileStorageGroupsToAttachmentTypes AS cfsgtat
                         JOIN dbo.CloudFileStoragePermissionFolderTemplate AS cfspft ON cfspft.CloudFolderTemplate = cfsgtat.CloudFolderTemplate
                         JOIN dbo.AttachmentTypes AS at ON at.AttachmentType = cfsgtat.AttachmentType
                         JOIN dbo.AttachmentCategoryTypes AS act ON act.AttachmentType = cfsgtat.AttachmentType
                         WHERE act.AttachmentCategory = " & SQLQuote(_AttachmentCategory),
                         Sub(t)
                             _dtAttachmentTypesCloudTemplates = t
                         End Sub)

            jammer.Add("SELECT cfsgtpf.CloudFolderTemplate, cfsgtpf.id_Level 
                        FROM dbo.CloudFileStorageGroupsToPermissionFolders AS cfsgtpf",
                             Sub(t)
                                 _dtCloudFolderTemplatesIDLevel = t
                             End Sub)

            jammer.Add("select cc2g.id_ContactCategory ,
                        cc2g.id_Level ,
                        cc.ContactCategory ,
                        g.[Level Description] ,
                        pf.CloudFolderTemplate 
                        from dbo.ContactCategoriesCloudFileStorageGroups cc2g
                        join dbo.ContactCategory cc on cc2g.id_ContactCategory = cc.id_ContactCategory
                        join dbo.CloudFileStorageGroups g on cc2g.id_Level = g.id_Level
                        join dbo.CloudFileStorageGroupsToPermissionFolders pf on g.id_Level = pf.id_Level",
                            Sub(t)
                                _dtContactCategoryToIDLevelandFolderTemplate = t
                            End Sub)

            jammer.Add("SELECT s.ServerName, s.ServerURIPrefix
                        FROM dbo.EgnyteSharingServers s
                        WHERE s.ServerName = @@SERVERNAME",
                             Sub(t)
                                 _EgnyteSharingURI = t.Rows(0).Item("ServerURIPrefix")
                             End Sub)
            jammer.Add("SELECT cfspft.CloudFolderTemplate, cfspft.DropboxFilePathSuffix 
                        FROM dbo.CloudFileStoragePermissionFolderTemplate AS cfspft",
                                        Sub(t)
                                            _dtRootAttachmentTypes = t
                                        End Sub)


            If _AttachmentCategory = "Parts" Then
                scSharingTree.Panel2Collapsed = True
            End If
        Else
            Dim dropboxControls = {btnDropboxFolder, btnUploadToDropbox}

            For Each c In dropboxControls
                c.Visible = False
            Next

            scSharingTree.Panel2Collapsed = True

            If Not _isUploadedToDropboxColumnRemoved Then 'this is not the best 
                globalOpsListView.Columns.RemoveAt(5)
                _isUploadedToDropboxColumnRemoved = True
            End If
        End If

        jammer.Execute()

        EnableDoubleBufferingTreeView(globalOpsTree)


        _Timer1.Interval = 300000 '5 minutes
        _Timer1.Enabled = True
        AddHandler _Timer1.Elapsed, AddressOf refreshFileSystemWatcher

        AddHandler globalOpsTree.AfterSelect, AddressOf globalOpsTree_AfterSelect

        globalOpsListView.ListViewItemSorter = New ListViewColumnSorter With {.SortColumn = 1, .Order = SortOrder.Ascending}

        AllowUserToEditToolStripMenuItem.Visible = False

        'PopulateTreeView() 'maybe refactor this so this only happens in the rootFolderChanged on load. Think this is necessary in the contacts version of the File Explorer. Does not make it load noticeably slower but should not be called 2 times on load

        If Not _canAddToKnowledgebase Then
            tsmiAddSelectedToKB.Visible = False
            tsmiRemoveSelectedToKB.Visible = False
            ToolStripSeparator7.Visible = False



            If chIsInKnowledgeBase.Index >= 0 Then
                globalOpsListView.Columns.RemoveAt(chIsInKnowledgeBase.Index)
            End If
        End If
    End Sub

    Private Sub loadFileIndex()
        Dim conn As SqlConnection = GetOpenedFinesseConnection()
        _dtFileIndex = conn.GetDataTable($"SELECT fai.fileName, fai.fileExtension, fai.CreationTimeUtc, fai.GUID, fai.SubFolderPath, fai.fileNameUnique, fai.entityno, fai.partno, fai.empno, fai.unique_no, fai.CreatedBy, fai.topic, fai.IsInKnowledgeBase, fai.DocumentExpirationDateUTC
                                            from dbo.FileAttachmentIndex AS fai
                                            WHERE fai.GUID = {_currentGUID.ToString.SQLQuote}")

    End Sub


    Private Sub loadLocallyManipulatedDropBoxTables()
        Dim jammer As New SQLJammer(GetOpenedFinesseConnection())
        Select Case _AttachmentCategory
            Case "Projects"
                jammer.Add("SELECT cfsuf.fileName, cfsuf.entityno, cfsuf.isUploaded, cfsuf.dropboxFileID, cfsuf.uploadTime, cfsuf.AttachmentType, cfsuf.AttachmentCategory, cfsuf.UserFolderPath  
                         FROM dbo.CloudFileStorageUploadedFiles AS cfsuf
                         WHERE cfsuf.entityno = " & SQLQuote(currentEntityno),
                        Sub(t)
                            _dtUploadedFiles = t
                        End Sub)
            Case "Parts"
                jammer.Add("SELECT pf.fileName, pf.partno, pf.isUploaded, pf.dropboxFileID, pf.UserFolderPath
                            FROM dbo.CloudFileStorageUploadedPartFiles pf
                            WHERE pf.partno = " & SQLQuote(currentPartNo),
                        Sub(t)
                            _dtUploadedFiles = t
                        End Sub)
        End Select
        jammer.Add("SELECT puftcsf.entityno, puftcsf.UserFolderPath, puftcsf.dropboxFolderID, puftcsf.CloudFolderTemplate, puftcsf.AttachmentType, puftcsf.id_Level, cfspft.CloudFolderTemplate, cfspft.DropboxFilePathSuffix 
                         FROM dbo.ProjectsUsersFoldersToCloudStorageFolders AS puftcsf
                         JOIN dbo.CloudFileStoragePermissionFolderTemplate AS cfspft ON cfspft.CloudFolderTemplate = puftcsf.CloudFolderTemplate
                         WHERE puftcsf.entityno = " & SQLQuote(currentEntityno),
                    Sub(t)
                        _dtUserFolders = t
                    End Sub)
        jammer.Add($";with available as (
                    select c.contactname as Name, c.email, cccfsg.id_Level  
                         FROM dbo.ProjectClientContacts pcc  
                         JOIN dbo.ContactCategory cc on pcc.id_ContactCategory = cc.id_ContactCategory  
                         JOIN dbo.contacts c on pcc.ContactNo = c.contactno
                         JOIN dbo.ContactCategoriesCloudFileStorageGroups AS cccfsg ON cccfsg.id_ContactCategory = cc.id_ContactCategory  
                         WHERE (pcc.entityno =  {SQLQuote(currentEntityno)} or pcc.entityno like {SQLQuote(currentEntityno)} + '-%')
                    union
                         select Name = e.firstname + ' ' + e.lastname, e.email, id_level = 3   --all confirmed crew are an ID level of 3   
                         FROM dbo.pjempassign a  
                         JOIN dbo.pejob j on a.jobtype = j.jobtype and j.is_qualification = 1  
                         JOIN dbo.glentities g on a.entityno = g.entityno and g.engactivecd <> 'I' 
                         JOIN dbo.peemployee e on a.empno = e.empno  
                         WHERE a.StatusCode = 'A'  AND	(a.entityno = {SQLQuote(currentEntityno)} or a.entityno like {SQLQuote(currentEntityno)} + '-%')
                    union
                            select	TeamMemberName as Name, email, id_Level
                            from dbo.fn_GetProjectCoreTeam({SQLQuote(currentEntityno)})
                            join dbo.CloudFileStoragGroupsToCoreTeam on CoreTeamMemberDesc = TeamMemberRole
                    )
                    select a.name as Name, c.email as Email, c.CloudFolderTemplate, c.isRemoveFolderMember, c.isAddFolderMember
                    from available a
                    right outer join dbo.CloudFileStorageShareRequests c on c.email = a.email
                    where c.entityno = {SQLQuote(currentEntityno)}",
                    Sub(t)
                        _dtProjectShareRequests = t
                        bsCurrentMembers.DataSource = _dtProjectShareRequests
                    End Sub)
        jammer.Execute()

    End Sub


    Public Sub loadAvailableInvitees()
        If Not DropboxEnabled Then
            Exit Sub
        End If

        Dim conn As SqlConnection = GetOpenedFinesseConnection()

        If _dtAvailableInvitees Is Nothing Then
            _dtAvailableInvitees = New DataTable
        Else
            _dtAvailableInvitees.Clear()
        End If

        Dim strSql As String = $"dbo.get_fileExplorer_Dropbox_Invitees @entityno = '{currentEntityno}'"
        Using conn
            Using dad As New SqlDataAdapter(strSql, conn)
                dad.Fill(_dtAvailableInvitees)
            End Using
        End Using



        ' _dtAvailableInvitees.Load(query)
        bsFolderInvitees.DataSource = _dtAvailableInvitees

        If dgvAvailableMembers.Columns.Contains("id_Level") Then
            dgvAvailableMembers.Columns.Remove("id_Level")
        End If
    End Sub

    Public Sub rootFolderChanged(ByVal newGUID As String)
        _currentGUID = newGUID
        If DropboxEnabled Then
            loadLocallyManipulatedDropBoxTables()
            loadAvailableInvitees()
        End If

        If _canAddToKnowledgebase Or _dtAttachmentTypes.Select("CanHaveExpirationDate = 1").Any Then
            loadFileIndex()
        End If

        PopulateTreeView()
    End Sub

    'use if you want the root folder path to be something other than the GUID 
    Public Sub rootFolderChanged(newGUID As String, attachmentRootSubFolderPath As String)
        _currentGUID = newGUID
        If DropboxEnabled Then
            loadLocallyManipulatedDropBoxTables()
            loadAvailableInvitees()
        End If

        If _canAddToKnowledgebase Or _dtAttachmentTypes.Select("CanHaveExpirationDate = 1").Any Then
            loadFileIndex()
        End If

        PopulateTreeView(attachmentRootSubFolderPath)
    End Sub

    Public Sub rootFolderChanged(ByVal newGUID As String, ByVal newDescription As String, ByVal newIdentitficationNumber As String)
        _currentGUID = newGUID.ToString

        Select Case _AttachmentCategory
            Case "Projects"
                currentEntityno = newIdentitficationNumber
                _projectDesc = newDescription
            Case "Parts"
                currentPartNo = newIdentitficationNumber
                _partDesc = newDescription
        End Select

        If DropboxEnabled Then
            loadLocallyManipulatedDropBoxTables()
            loadAvailableInvitees()
        End If

        If _canAddToKnowledgebase Or _dtAttachmentTypes.Select("CanHaveExpirationDate = 1").Any Then
            loadFileIndex()
        End If

        PopulateTreeView()
    End Sub

    Public Sub categoryChanged(newCategory As String, Optional ByVal folderGUID As String = "")
        _AttachmentCategory = newCategory
        Load_FileExplorer()
        _currentGUID = folderGUID
        PopulateTreeView()
    End Sub

    Private Function getRootFolderPath() As String
        Dim path As String = String.Empty

        If _currentGUID Is String.Empty Then
            Return path
        End If

        Dim conn As SqlConnection = GetOpenedFinesseConnection()
        path = conn.ExecuteScalar($"select fsp.fileStoragePath
                                    from dbo.fileStoragePaths AS fsp
                                    where fsp.fileStorageGUID = '{_currentGUID}'")
        conn.Close()
        conn.Dispose()

        Return path
    End Function

    Private Function getRootDefaultFolderPath() As String
        Dim path As String = String.Empty

        If _currentGUID Is String.Empty Then
            Return path
        End If

        Dim conn As SqlConnection = GetOpenedFinesseConnection()

        Dim sqlComm As SqlCommand = New SqlCommand("dbo.get_default_file_Storage_path", conn)
        sqlComm.CommandType = CommandType.StoredProcedure
        sqlComm.Parameters.Add("@GUID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(_currentGUID)
        sqlComm.Parameters.Add("@AttachmentType", SqlDbType.VarChar, 50).Value = _AttachmentCategory
        sqlComm.Parameters.Add("@path", SqlDbType.VarChar, 255)
        sqlComm.Parameters("@path").Direction = ParameterDirection.Output
        sqlComm.ExecuteNonQuery()
        path = sqlComm.Parameters.Item("@path").Value

        conn.Close()
        conn.Dispose()

        Return path
    End Function

    Public Sub PopulateTreeView(Optional ByVal attachmentRootSubFolderPath = "")
        globalOpsTree.Hide()

        globalOpsListView.Items.Clear()

        Dim rootPath = getRootFolderPath()
        If rootPath Is String.Empty Or rootPath Is Nothing Then
            If attachmentRootSubFolderPath IsNot String.Empty Then

                _attachmentCategorySubFolderPath = Path.Combine(GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY, attachmentRootSubFolderPath)
            Else
                _attachmentCategorySubFolderPath = getRootDefaultFolderPath()
            End If
        Else
            _attachmentCategorySubFolderPath = rootPath
        End If

        currentGlobalOpsFolder = _attachmentCategorySubFolderPath

        Dim info As DirectoryInfo = Nothing

        If Me.globalOpsTree.Nodes.Count > 0 Then
            globalOpsTree.Nodes.Clear()
        End If

        _isLoadingTree = True
        globalOpsTree.UseWaitCursor = True
        Dim errormessage As String = String.Empty



        If currentGlobalOpsFolder IsNot String.Empty Then

            info = New DirectoryInfo(currentGlobalOpsFolder)

            If info IsNot Nothing AndAlso info.Exists AndAlso currentGlobalOpsFolder <> GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY Then
                Dim subdirs = info.GetDirectories()

                For Each subdir In subdirs
                    Try

                        Dim rootNode As TreeNode = New TreeNode(subdir.Name)
                        info = New DirectoryInfo(Path.Combine(currentGlobalOpsFolder, subdir.Name))
                        'if the folder is an attachment type can only use Folder name since meta data cannot be attached
                        Dim attachmentType As DataRow = getAttachmentTypeByDescription(subdir.Name)

                        If subdir.GetFiles().Count > 0 Then
                            rootNode.ImageIndex = 1
                            rootNode.SelectedImageIndex = 1
                        End If

                        If attachmentType IsNot Nothing Then
                            If doesUserHaveAccessToThisAttachmentType(subdir.Name) Then
                                rootNode.Tag = attachmentType.Item("AttachmentType")
                                globalOpsTree.Nodes.Add(rootNode)
                                rootNode.Name = rootNode.FullPath
                                addSubDirectoriesToTree(info.GetDirectories(), rootNode)
                            End If
                        Else

                            globalOpsTree.Nodes.Add(rootNode)
                            rootNode.Tag = rootNode.FullPath
                            rootNode.Name = rootNode.FullPath
                            addSubDirectoriesToTree(info.GetDirectories(), rootNode)

                        End If
                    Catch ex As System.Exception
                        errormessage = errormessage & subdir.Name & ", "
                    End Try

                Next

            End If

        End If

        'add the rest of the attachments that are not created yet
        'add parent nodes first


        Dim v As DataView = New DataView(_dtAttachmentTypes)
        Dim DistinctAttachmentTypes As DataTable = v.ToTable(True, "attachmentType", "AttachmentTypeDescription", "FullAttachmentTypePath", "Parent", "TreeOrder")

        Dim mainNodes = DistinctAttachmentTypes.Select("[Parent] is null")

        For Each row As DataRow In mainNodes
            'check if node is already in the tree
            'If globalOpsTree.Nodes.Find(row.Item("AttachmentTypeDescription"), True).Length = 0 AndAlso doesUserHaveAccessToThisAttachmentType(row.Item("AttachmentTypeDescription")) Then



            If doesUserHaveAccessToThisAttachmentType(row.Item("AttachmentTypeDescription")) Then

                Try
                    If globalOpsTree.Nodes.Find(row.Item("FullAttachmentTypePath"), True).Length >= 1 Then
                        addAttachmentTypeChildren(globalOpsTree.Nodes.Find(row.Item("AttachmentTypeDescription"), True).First)
                    Else
                        Dim rootNode = New TreeNode(row.Item("AttachmentTypeDescription"))
                        rootNode.Tag = row.Item("AttachmentType")

                        addAttachmentTypeChildren(rootNode)

                        globalOpsTree.Nodes.Add(rootNode)
                        rootNode.Name = rootNode.FullPath
                    End If

                Catch ex As System.Exception
                    errormessage = errormessage & row.Item("AttachmentTypeDescription") & ", "
                End Try
            End If
        Next

        'adding name property to the nodes that do not have it. These need to be added after the parent node is added to the tree. The method Treeview.Nodes.Find seaches on this field.
        For Each node As TreeNode In globalOpsTree.Nodes
            giveTreeNodesChildrensNames(node)
        Next


        If errormessage IsNot String.Empty Then
            MessageBox.Show($"Error Loading one or more folders in the tree {errormessage}")
        End If

        'Avoiding the Alphabetical order and using TreeOrder column
        'globalOpsTree.Sort()
        globalOpsTree.Show()

        _isLoadingTree = False
        SorttreeNode()
        globalOpsTree.UseWaitCursor = False
    End Sub

    Private Sub giveTreeNodesChildrensNames(ByVal parent As TreeNode)
        For Each aNode As TreeNode In parent.Nodes
            aNode.Name = aNode.FullPath

            If aNode.Nodes.Count > 0 Then
                giveTreeNodesChildrensNames(aNode)
            End If

        Next
    End Sub



    Private Sub addAttachmentTypeChildren(parentAttachmentType As TreeNode)




        For Each row As DataRow In _dtAttachmentTypes.Select($"[Parent] = '{parentAttachmentType.Tag}'")

            If parentAttachmentType.Nodes.Find(row.Item("FullAttachmentTypePath"), True).Length = 1 Then
                addAttachmentTypeChildren(parentAttachmentType.Nodes.Find(row.Item("FullAttachmentTypePath"), True)(0))
                Continue For
            End If

            Dim childNode = New TreeNode(row.Item("AttachmentTypeDescription"))
            childNode.Tag = row.Item("AttachmentType")



            Dim isChildNodeAParent As DataRow() = _dtAttachmentTypes.Select($"[Parent] = '{childNode.Tag}'")

            If isChildNodeAParent.Any Then
                addAttachmentTypeChildren(childNode)
            End If

            If (parentAttachmentType.Nodes.Cast(Of TreeNode)().FirstOrDefault(Function(n) n.Tag IsNot Nothing And n.Tag.Equals(childNode.Tag)) Is Nothing) Then
                parentAttachmentType.Nodes.Add(childNode)
            End If


            'childNode.Name = childNode.FullPath
        Next
    End Sub

    Private Sub SorttreeNode()
        globalOpsTree.TreeViewNodeSorter = New NodeSorter(_dtAttachmentTypes)
    End Sub

#Region "Old old Populate Tree view"
    'Need to test no GUID situations
    'Public Sub PopulateTreeView(Optional ByVal attachmentRootSubFolderPath = "")
    '    Dim rootNode As TreeNode
    '    globalOpsListView.Items.Clear()

    '    _isLoadingTree = True
    '    globalOpsTree.UseWaitCursor = True

    '    Dim rootPath = getRootFolderPath()
    '    If rootPath Is String.Empty Or rootPath Is Nothing Then
    '        If attachmentRootSubFolderPath IsNot String.Empty Then
    '            _attachmentCategorySubFolderPath = Path.Combine(GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY, attachmentRootSubFolderPath)
    '        Else
    '            _attachmentCategorySubFolderPath = getRootDefaultFolderPath()
    '        End If
    '    Else
    '        _attachmentCategorySubFolderPath = rootPath
    '    End If

    '    currentGlobalOpsFolder = _attachmentCategorySubFolderPath

    '    Dim info As DirectoryInfo = Nothing

    '    If currentGlobalOpsFolder IsNot String.Empty Then
    '        info = New DirectoryInfo(currentGlobalOpsFolder)
    '    End If

    '    If Me.globalOpsTree.Nodes.Count > 0 Then
    '        globalOpsTree.Nodes.Clear()
    '    End If

    '    _isLoadingTree = True
    '    globalOpsTree.UseWaitCursor = True
    '    Dim errormessage As String = String.Empty
    '    If info IsNot Nothing AndAlso info.Exists AndAlso currentGlobalOpsFolder <> GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY Then
    '        Dim subdirs = info.GetDirectories()
    '        For Each subdir In subdirs
    '            Try
    '                If Not _displayArchivedFolders And subdir.FullName.Contains("- Archive") Then
    '                    Continue For
    '                End If

    '                info = New DirectoryInfo(Path.Combine(currentGlobalOpsFolder, subdir.Name))
    '                rootNode = New TreeNode(subdir.Name)
    '                'rootNode.Name = subdir.Name
    '                'if the folder is an attachment type can only use Folder name since meta data cannot be attached
    '                If isAttachmentType(subdir.Name) Then
    '                    Dim row As DataRow = getAttachmentTypeByDescription(subdir.Name)
    '                    If doesUserHaveAccessToThisAttachmentType(subdir.Name) Then
    '                        rootNode.Tag = row.Item("AttachmentType")

    '                        If subdir.GetFiles().Count > 0 Then
    '                            rootNode.ImageIndex = 1
    '                            rootNode.SelectedImageIndex = 1
    '                        End If
    '                        globalOpsTree.Nodes.Add(rootNode)
    '                        rootNode.Name = rootNode.FullPath

    '                        addSubDirectoriesToTree(info.GetDirectories(), rootNode)
    '                    End If
    '                Else
    '                    If subdir.GetFiles().Count > 0 Then
    '                        rootNode.ImageIndex = 1
    '                        rootNode.SelectedImageIndex = 1
    '                    End If
    '                    globalOpsTree.Nodes.Add(rootNode)
    '                    rootNode.Tag = rootNode.FullPath
    '                    rootNode.Name = rootNode.FullPath

    '                    addSubDirectoriesToTree(info.GetDirectories(), rootNode)
    '                End If
    '            Catch ex As System.Exception
    '                errormessage = errormessage & subdir.Name & ", "
    '            End Try
    '        Next
    '    End If

    '    'add the rest of the attachments that are not created yet
    '    For Each row As DataRow In _dtAttachmentTypes.Rows()
    '        If globalOpsTree.Nodes.Find(row.Item("AttachmentTypeDescription"), True).Length = 0 Then
    '            If doesUserHaveAccessToThisAttachmentType(row.Item("AttachmentTypeDescription")) Then
    '                Try
    '                    rootNode = New TreeNode(row.Item("AttachmentTypeDescription"))
    '                    rootNode.Tag = row.Item("AttachmentType")
    '                    globalOpsTree.Nodes.Add(rootNode)
    '                    rootNode.Name = rootNode.FullPath
    '                Catch ex As System.Exception
    '                    errormessage = errormessage & row.Item("AttachmentTypeDescription") & ", "
    '                End Try
    '            End If
    '        End If
    '    Next

    '    If DropboxEnabled And _dtUserFolders IsNot Nothing Then
    '        Dim test As DataRow() = _dtUserFolders.Select("dropboxFolderId Is not NUll")
    '        For Each folder As DataRow In _dtUserFolders.Select("dropboxFolderId Is not NUll")
    '            Dim search As TreeNode() = globalOpsTree.Nodes.Find(folder.Item("UserFolderPath"), True)
    '            If search.Length > 0 Then
    '                checkSubFoldersForDropboxID(search(0))
    '                search(0).ImageIndex = 2
    '                search(0).SelectedImageIndex = 2
    '            End If
    '        Next
    '    End If

    '    If Not errormessage Is String.Empty And fileNotDisplayed = 0 Then
    '        MessageBox.Show("You do not have permission to view the following folders: " & errormessage & ". If this is incorrect please contact your IT administrator.")
    '        fileNotDisplayed = 1
    '    End If

    '    globalOpsTree.Sort()

    '    globalOpsTree.UseWaitCursor = False
    '    _isLoadingTree = False

    '    'for testing purposes
    '    'For Each node As TreeNode In globalOpsTree.Nodes
    '    '    Debug.WriteLine($"Node: {node.Text} FullPath: {node.FullPath} Name: {node.Name}")
    '    '    Dim children As List(Of TreeNode) = GetChildren(node)
    '    '    Console.WriteLine("")
    '    '    For Each child As TreeNode In children
    '    '        Debug.WriteLine($"Node: {child.Text} FullPath: {child.FullPath} Name: {child.Name}")
    '    '        Console.WriteLine("")
    '    '    Next
    '    'Next
    'End Sub

#End Region

    Private Sub populateListView(Optional ByVal shouldResizeColumns As Boolean = True)


        If doDisplayedFilesMatchActualFolder() Then
            Exit Sub
        End If

        globalOpsListView.Items.Clear()
        If Not (Directory.Exists(currentGlobalOpsFolder)) Then
            Exit Sub
        End If

        Dim fileEntries As String() = Directory.GetFiles(currentGlobalOpsFolder)

        For Each f As String In Directory.GetFiles(currentGlobalOpsFolder)
            Try
                Dim attributes As FileAttributes = File.GetAttributes(f)
                If Not (attributes And FileAttributes.Hidden) = FileAttributes.Hidden Then
                    addFileToListView(f)
                End If
            Catch ex As System.Exception

            End Try
        Next

        For Each Dir As String In Directory.GetDirectories(currentGlobalOpsFolder)
            If Not _displayArchivedFolders And Dir.Contains("- Archive") Then
                Continue For
            End If
            addFolderToListView(Dir)
        Next


        If DropboxEnabled Then
            Try
                Dim i = 0
                For Each item As ListViewItem In globalOpsListView.Items
                    For Each uploadedFile As DataRow In _dtUploadedFiles.Rows
                        Try
                            If uploadedFile.Item("fileName") = item.Text And uploadedFile.Item("UserFolderPath") = globalOpsTree.SelectedNode.FullPath Then
                                item.SubItems(5).Text = "Yes"
                            End If
                        Catch ex As System.Exception

                        End Try
                    Next
                Next
            Catch ex As System.Exception

            End Try
        End If

        globalOpsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)

    End Sub

    Private Sub checkSubFoldersForDropboxID(ByVal parent As TreeNode)
        For Each aNode As TreeNode In parent.Nodes
            aNode.Name = aNode.FullPath
            Dim subdir = New DirectoryInfo(Path.Combine(currentGlobalOpsFolder, aNode.FullPath))
            If subdir.GetFiles().Count > 0 Then
                aNode.ImageIndex = 1
                aNode.SelectedImageIndex = 1
            End If
            Dim dropBoxID = getDropboxFolderID(aNode)
            If dropBoxID IsNot String.Empty Then
                aNode.ImageIndex = 2
                aNode.SelectedImageIndex = 2
            End If
            If aNode.Nodes.Count > 0 Then
                checkSubFoldersForDropboxID(aNode)
            End If
        Next
    End Sub

    Private Sub giveSubfoldersNames(ByVal parent As TreeNode)
        For Each aNode As TreeNode In parent.Nodes
            Dim subdir = New DirectoryInfo(Path.Combine(currentGlobalOpsFolder, aNode.FullPath))
            aNode.Name = aNode.FullPath
            If subdir.GetFiles().Count > 0 Then
                aNode.ImageIndex = 1
                aNode.SelectedImageIndex = 1
            End If
        Next
    End Sub

    Public Sub createSubDirectories(ByVal info As DirectoryInfo, ByRef Row As DataRow)
        Dim newDir As String = info.ToString

        Try
            System.IO.Directory.CreateDirectory(newDir)
        Catch ex As System.Exception
            MessageBox.Show($"Error creating directory {newDir}: {ex.Message}")
        End Try

    End Sub

    'for ensuring that the attachment type subdirectories exsit before adding files/folders
    Private Sub EnsureSubdirectoryExists(ByVal filepath As String, ByVal folder As TreeNode)

        Dim folderPath = Path.GetDirectoryName(filepath)

        If Not (Directory.Exists(folderPath)) Then
            Dim attachmentDataRow As DataRow = getAttachmentTypeByCode(folder.Tag)
            Dim attachmentDirectoryInfo = New DirectoryInfo(folderPath)
            createSubDirectories(attachmentDirectoryInfo, attachmentDataRow)

            If Directory.Exists(currentGlobalOpsFolder) Then
                _fsw = New FileSystemWatcher(currentGlobalOpsFolder)
                '_fsw.IncludeSubdirectories = True
                _fsw.EnableRaisingEvents = True
            End If

            If DropboxEnabled And _AttachmentCategory = "Projects" Then

                Dim attachmentCloudInfo As DataRow() = _dtAttachmentTypesCloudTemplates.Select("AttachmentType = " & SQLQuote(folder.Tag))

                Dim template
                Dim idLevel

                If attachmentCloudInfo.Length = 0 Then
                    template = String.Empty
                    idLevel = 100
                Else
                    template = attachmentCloudInfo(0).Item("CloudFolderTemplate")
                    idLevel = getIDLevelFromTemplateType(template)
                End If


                Try

                    Using newConn As New SqlConnection(FinesseConnectionString)
                        newConn.Open()
                        Dim query As New SqlCommand("INSERT dbo.ProjectsUsersFoldersToCloudStorageFolders
                        (
                            entityno,
                            UserFolderPath,
                            CloudFolderTemplate,
                            AttachmentType,
                            id_Level
                        )
                        VALUES
                        (
                            " & SQLQuote(currentEntityno) & ", -- entityno - uniqueidentifier
                            " & SQLQuote(folder.FullPath) & ",   -- UserFolderPath - varchar(100)
                             " & SQLQuote(template) & ",
                            " & SQLQuote(folder.Tag) & ",
                             " & idLevel & "
                        )", newConn)

                        query.ExecuteNonQuery()
                        newConn.Close()

                        Dim newFolderRow As DataRow = _dtUserFolders.NewRow()
                        newFolderRow("entityno") = currentEntityno
                        newFolderRow("UserFolderPath") = folder.FullPath
                        newFolderRow("dropboxFolderID") = DBNull.Value
                        newFolderRow("CloudFolderTemplate") = template
                        newFolderRow("AttachmentType") = folder.Tag
                        newFolderRow("id_Level") = idLevel
                        _dtUserFolders.Rows.Add(newFolderRow)

                    End Using
                Catch ex As System.Exception
                    If ex.Message.Contains("PK_ProjectsUsersFoldersToCloudStorageFolders") Then

                    Else
                        'MessageBox.Show("Error Creating  DB entry: " & ex.Message)
                    End If

                End Try
            End If
        Else
            If DropboxEnabled And _AttachmentCategory = "Projects" Then
                checkIfFolderHasDBEntry(folder)
            End If
        End If
    End Sub

    Private Sub populateDataGridViews(ByVal folder As TreeNode)
        If Not DropboxEnabled Then Return

        If dgvAvailableMembers.DataSource Is Nothing Then
            dgvAvailableMembers.Columns.Remove("AvailableName")
            dgvAvailableMembers.Columns.Remove("AvailableEmail")
            dgvAvailableMembers.DataSource = bsFolderInvitees
        End If

        If dgvAvailableMembers.Columns.Contains("id_Level") Then
            dgvAvailableMembers.Columns.Remove("id_Level")
        End If

        Dim folderTemplate As String = getCloudFolderTemplate(globalOpsTree.SelectedNode)

        'gets the list of available emails to invite
        Dim availableEmailDictionary As New Dictionary(Of String, String)


        Dim IDLevels = _dtCloudFolderTemplatesIDLevel.Select("CloudFolderTemplate = " & SQLQuote(folderTemplate))
        bsFolderInvitees.RemoveFilter()

        If IDLevels.Count = 0 Then
            bsFolderInvitees.Filter = "email='xxxx'"
            bsCurrentMembers.Filter = "email='xxxx'"
            Exit Sub
        End If

        Dim filter As String = Nothing
        For Each row As DataRow In IDLevels
            filter = filter & "id_Level=" & row.Item("id_Level") & " "

            If Not row Is IDLevels.Last Then
                filter = filter & " OR "
            End If
        Next

        bsFolderInvitees.Filter = filter

        For Each member As KeyValuePair(Of String, String) In availableEmailDictionary
            dgvAvailableMembers.Rows.Add(New String() {member.Value, member.Key})
        Next


        'If isAttachmentType(folder.Text) AndAlso doesAttachmentTypeHavePermissions(folder.Text) Then
        '    'If folder has extra permissions layered on top of it then users need to be explicitly invited to it
        '    bsCurrentMembers.Filter = $"isAddFolderMember=1 AND isRemoveFolderMember=0 AND FileStoragePath = {currentGlobalOpsFolder.SQLQuote}"
        'Else
        '    bsCurrentMembers.Filter = $"isAddFolderMember=1 AND isRemoveFolderMember=0 AND (FileStoragePath = {currentGlobalOpsFolder.SQLQuote} OR FileStoragePath = {attachmentCategorySubFolderPath.SQLQuote})"
        'End If

        Dim dropboxID As String = getDropboxFolderID(folder)

        If dropboxID Is String.Empty Then
            bsCurrentMembers.Filter = "email='xxxx'" ' dont want anything to display
            Exit Sub
        End If

        'is template because if they have access to one folder in the template they have access to all
        bsCurrentMembers.Filter = "isRemoveFolderMember<>1 AND CloudFolderTemplate=" & SQLQuote(folderTemplate)

        'gets who the folder is currently shared with
        If dgvCurrentMembers.DataSource Is Nothing Then
            dgvCurrentMembers.Columns.Remove("currentName")
            dgvCurrentMembers.Columns.Remove("currentEmail")
            dgvCurrentMembers.DataSource = bsCurrentMembers
        End If

        If dgvCurrentMembers.Columns.Contains("isAddFolderMember") Then
            dgvCurrentMembers.Columns.Remove("isAddFolderMember")
        End If

        If dgvCurrentMembers.Columns.Contains("isRemoveFolderMember") Then
            dgvCurrentMembers.Columns.Remove("isRemoveFolderMember")
        End If

        If dgvCurrentMembers.Columns.Contains("CloudFolderTemplate") Then
            dgvCurrentMembers.Columns.Remove("CloudFolderTemplate")
        End If


    End Sub


    Private Sub addSubDirectoriesToTree(ByVal subDirs() As DirectoryInfo, ByVal nodeToAddTo As TreeNode)
        Dim aNode As TreeNode
        Dim subSubDirs() As DirectoryInfo

        Dim directoryTreeOrderMap As New Dictionary(Of String, Integer)()

        For Each row As DataRow In _dtAttachmentTypes.Select("[Parent] = " & SQLQuote(nodeToAddTo.Tag))
            Dim directoryName As String = row("AttachmentTypeDescription").ToString()
            Dim treeOrder As Integer = Convert.ToInt32(If(IsDBNull(row("TreeOrder")), 0, row("TreeOrder")))
            If (Not directoryTreeOrderMap.ContainsKey(directoryName)) Then
                directoryTreeOrderMap.Add(directoryName, treeOrder)
            End If

        Next

        ' Now sort the subdirectories based on their TreeOrder values
        Dim sortedSubdirs = subDirs.OrderBy(Function(d) If(directoryTreeOrderMap.ContainsKey(d.Name),
                                                directoryTreeOrderMap(d.Name), Integer.MaxValue))

        For Each subDir As DirectoryInfo In sortedSubdirs
            If Not _displayArchivedFolders And subDir.FullName.Contains("- Archive") Then
                Continue For
            End If
            aNode = New TreeNode(subDir.Name, 0, 0)
            subSubDirs = subDir.GetDirectories()
            nodeToAddTo.Nodes.Add(aNode)

            If subSubDirs.Length <> 0 Then
                addSubDirectoriesToTree(subSubDirs, aNode)
            End If

            If subDir.GetFiles().Count > 0 Then
                aNode.ImageIndex = 1
                aNode.SelectedImageIndex = 1
            End If

            Dim atRow As DataRow = getAttachmentTypeByDescription(aNode.Text)

            If atRow IsNot Nothing Then
                aNode.Tag = atRow.Item("AttachmentType")
            End If

            aNode.Name = aNode.FullPath
        Next subDir

    End Sub

    Private Sub addFileToListView(ByVal filePath As String)
        Dim arr As String() = New String(6) {}
        Dim item As ListViewItem = New ListViewItem
        Dim DirectoryInfo As New DirectoryInfo(filePath)
        Dim fi As FileInfo = New FileInfo(filePath)

        arr(0) = DirectoryInfo.Name
        arr(1) = Path.GetExtension(filePath)
        arr(2) = DirectoryInfo.LastWriteTime

        Dim myFile As New FileInfo(filePath)
        arr(3) = $"{myFile.Length / 1000: 0} KB"

        Try
            Dim fs As FileSecurity = System.IO.File.GetAccessControl(filePath)
            Dim sid As IdentityReference = fs.GetOwner(GetType(SecurityIdentifier))
            Dim ntaccount As IdentityReference = sid.Translate(GetType(NTAccount))
            Dim owner As String = ntaccount.ToString()
            arr(4) = owner
        Catch ex As System.Exception

        End Try

        'arr(5) = "No"

        'research more 
        item = New ListViewItem(arr)
        If Path.GetExtension(filePath).ToLower = ".docx" Or Path.GetExtension(filePath).ToLower = ".doc" Then
            item.ImageIndex = 0
        ElseIf Path.GetExtension(filePath).ToLower = ".pdf" Then
            item.ImageIndex = 1
        ElseIf Path.GetExtension(filePath).ToLower = ".xlsx" Or Path.GetExtension(filePath).ToLower = ".xls" Then
            item.ImageIndex = 2
        Else
            item.ImageIndex = 3
        End If




        If _canAddToKnowledgebase Then
            Dim hasFileInKnowledgeBase As DataRow() = _dtFileIndex.Select($"SubFolderPath = {globalOpsTree.SelectedNode.FullPath.SQLQuote} and fileName = {item.Text.SQLQuote} and IsInKnowledgeBase = 1")

            If hasFileInKnowledgeBase.Any Then
                item.SubItems(chIsInKnowledgeBase.DisplayIndex).Text = "Yes"
            End If
        End If


        If isAttachmentType(globalOpsTree.SelectedNode.Text) Then
            If canAttachmentTypeHaveExpirationDates(globalOpsTree.SelectedNode.Text) Then
                Dim hasExpirationDate As DataRow() = _dtFileIndex.Select($"SubFolderPath = {globalOpsTree.SelectedNode.FullPath.SQLQuote} and fileName = {item.Text.SQLQuote}")
                If hasExpirationDate.Any Then
                    If hasExpirationDate(0).Item("DocumentExpirationDateUTC") IsNot DBNull.Value Then
                        item.SubItems(chExpirationDate.DisplayIndex).Text = hasExpirationDate(0).Item("DocumentExpirationDateUTC")
                    End If
                End If
            End If
        End If

        globalOpsListView.Items.Add(item)

    End Sub

    Private Sub addFolderToListView(ByVal folderPath As String)
        Dim arr As String() = New String(5) {}
        Dim item As ListViewItem = New ListViewItem
        Dim DirectoryInfo As New DirectoryInfo(folderPath)
        Dim fi As FileInfo = New FileInfo(folderPath)

        arr(0) = DirectoryInfo.Name
        arr(1) = "Folder"
        arr(2) = DirectoryInfo.LastWriteTime

        If Not Directory.Exists(folderPath) Then
            Exit Sub
        End If

        Dim fs As FileSecurity = System.IO.File.GetAccessControl(folderPath)
        Dim sid As IdentityReference = fs.GetOwner(GetType(SecurityIdentifier))
        Dim ntaccount As IdentityReference = sid.Translate(GetType(NTAccount))
        Dim owner As String = ntaccount.ToString()
        arr(4) = owner
        arr(5) = ""

        item = New ListViewItem(arr)
        item.ImageIndex = 8
        globalOpsListView.Items.Add(item)
    End Sub

#Region "Event Handlers"

    Private Sub checkIfPrint(sender As Object, e As EventArgs) Handles cmsListView.Opened

        If globalOpsListView.SelectedItems.Count = 0 Then
            Exit Sub
        End If

        Dim filename = Path.Combine(currentGlobalOpsFolder, globalOpsListView.SelectedItems(0).Text)

        Dim startInfo = New ProcessStartInfo(filename)
        Dim Verbs = startInfo.Verbs
        Dim hasPrintVerb = False

        For Each verb In Verbs
            If verb = "Print" Then
                hasPrintVerb = True
                Exit For
            End If
        Next

        If Not hasPrintVerb Then
            cmsListView.Items.RemoveByKey("PrintToolStripMenuItem")
        Else
            cmsListView.Items.Insert(1, PrintToolStripMenuItem)
        End If

        If isAttachmentType(globalOpsTree.SelectedNode.Text) Then

            If canAttachmentTypeHaveExpirationDates(globalOpsTree.SelectedNode.Text) Then
                cmsListView.Items.Insert(cmsListView.Items.Count - 1, tsmiSetDocumentExpirationDate)
            Else
                cmsListView.Items.RemoveByKey("tsmiSetDocumentExpirationDate")
            End If

        End If
    End Sub

    Private Sub copyProjectsFolder(fromGUID As String, toGUID As String)
        Dim info As New DirectoryInfo(Path.Combine(GlobalOpsFinesseDataRootDirectory, fromGUID))

        Dim folder As DirectoryInfo() = info.GetDirectories()

        Dim listofSubdirs As List(Of DirectoryInfo) = getSubDirs(info)

        For Each copyDir As DirectoryInfo In listofSubdirs

            If copyDir.FullName.Contains("- Archive") Then
                Continue For
            End If

            Dim pattern = "........-....-....-....-............"
            Dim regExpressuib As Match = Regex.Match(copyDir.FullName, pattern)

            Dim archivePath = copyDir.FullName.Substring(0, regExpressuib.Index + regExpressuib.Length)

            Dim subFolderString = copyDir.FullName.Substring(regExpressuib.Index + regExpressuib.Length)
            subFolderString = subFolderString.Substring(1)

            Dim destinationFolder As String = Path.Combine(GlobalOpsFinesseDataRootDirectory, toGUID, subFolderString)

            Try
                FileSystem.CopyDirectory(copyDir.FullName, destinationFolder)
            Catch ex As System.Exception

            End Try
        Next

    End Sub

    Private Function getSubDirs(dir As DirectoryInfo) As List(Of DirectoryInfo)
        Dim subdirs As DirectoryInfo() = dir.GetDirectories()

        Dim listofSubdirs As List(Of DirectoryInfo) = New List(Of DirectoryInfo)

        For Each subdir As DirectoryInfo In subdirs
            listofSubdirs.Add(subdir)
            If subdir.GetDirectories.Length <> 0 Then
                listofSubdirs.AddRange(getSubDirs(subdir))
            End If
        Next
        Return listofSubdirs
    End Function

    'probably need a better name
    Private Sub unrollProject(sourceGUID As String, destinationGUID As String)
        Dim sourceInfo As New DirectoryInfo(Path.Combine(GlobalOpsFinesseDataRootDirectory, sourceGUID))
        Dim destinationInfo As New DirectoryInfo(Path.Combine(GlobalOpsFinesseDataRootDirectory, destinationGUID))

        Dim myFileCompare As New FileCompare

        Dim list1 = sourceInfo.GetFiles("*.*", System.IO.SearchOption.AllDirectories)
        Dim list2 = destinationInfo.GetFiles("*.*", System.IO.SearchOption.AllDirectories)

        Dim queryDirAOnly = list1.Except(list2, myFileCompare)
        Dim queryCommonFiles = list1.Intersect(list2, myFileCompare)

        If queryCommonFiles.Count() > 0 Then

            Console.WriteLine("The following files are in both folders:")
            For Each fi As System.IO.FileInfo In queryCommonFiles
                Try
                    FileSystem.DeleteFile(fi.FullName)
                Catch ex As System.Exception

                End Try

                Console.WriteLine(fi.FullName)
            Next
        Else
            Console.WriteLine("There are no common files in the two folders.")
        End If

        Dim pattern = "........-....-....-....-............"
        For Each sourceSubFolder In sourceInfo.GetDirectories("*.*", System.IO.SearchOption.AllDirectories)
            If sourceSubFolder.FullName.Contains("- Archive") Then
                Continue For
            End If

            Dim SourceRegEx As Match = Regex.Match(sourceSubFolder.FullName, pattern)

            Dim SourceSubFolderString = sourceSubFolder.FullName.Substring(SourceRegEx.Index + SourceRegEx.Length)
            SourceSubFolderString = SourceSubFolderString.Substring(1)

            For Each destinationSubFolder In destinationInfo.GetDirectories("*.*", System.IO.SearchOption.AllDirectories)
                If destinationSubFolder.FullName.Contains("- Archive") Then
                    Continue For
                End If

                Dim regExpressuib As Match = Regex.Match(sourceSubFolder.FullName, pattern)

                Dim DestinationSubFolderString = destinationSubFolder.FullName.Substring(regExpressuib.Index + regExpressuib.Length)
                DestinationSubFolderString = DestinationSubFolderString.Substring(1)

                If DestinationSubFolderString = SourceSubFolderString And Not isAttachmentType(DestinationSubFolderString) Then
                    Debug.Print("Folder Found in Both: " & DestinationSubFolderString)
                    Try
                        System.IO.Directory.Delete(sourceSubFolder.FullName, True)
                    Catch ex As System.Exception

                    End Try

                    Exit For
                End If

            Next
        Next
        'SearchOption.TopDirectoryOnly
    End Sub

    Private Sub cmsTreeView_Opened(sender As Object, e As EventArgs) Handles cmsTreeView.Opened
        'If AttachmentCategory = "Projects" Then

        'End If
        cmsTreeView.Items.RemoveByKey("TreeViewImportProjectFiles")
    End Sub

    'Private Sub ImportProjectsFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TreeViewImportProjectFiles.Click
    '    Dim picker As New ProjectPicker(Me.ParentForm, Me, FinesseConnectionString)

    '    If Not picker.GetProject Then
    '        Exit Sub
    '    End If

    '    Dim importInfo As DataTable

    '    Dim phaseRootNo As String = getProjectRootPhaseNumber(picker.ProjectNum) 'make copy of this function in file explorer so it does not require fileexplorer utilities module

    '    Using newConn As New SqlConnection(FinesseConnectionString)
    '        newConn.Open()
    '        importInfo = newConn.GetDataTable($"SELECT g.GUID, g.entitydesc 
    '                                      FROM dbo.glentities g
    '                                      WHERE entityno = {SQLQuote(phaseRootNo)}")
    '        newConn.Close()
    '    End Using

    '    Dim importFilePath As String = Path.Combine(GlobalOpsFinesseDataRootDirectory, importInfo.Rows(0).Item("GUID").ToString)

    '    Dim importDirectoryInfo As DirectoryInfo = New DirectoryInfo(importFilePath)



    'End Sub

    Private Sub createFolder_Click(sender As Object, e As EventArgs) Handles btnCreateFolder.Click
        Dim newFolderName As Object
        newFolderName = InputBox("What would you like to name the folder?", "New Folder")

        If newFolderName = "" Then
            Exit Sub
        End If

        checkForRootFolderDefinitionAndExistence()

        Dim newFolderPath = Path.Combine(_attachmentCategorySubFolderPath, newFolderName)

        createNewFolder(newFolderPath)
    End Sub

    Private Sub globalOpsTree_beforeSelect(sender As Object, e As TreeViewCancelEventArgs) Handles globalOpsTree.BeforeSelect
        If _isLoadingTree Then
            e.Cancel = True
        End If
    End Sub

    Private Sub globalOpsTree_AfterSelect(sender As Object, e As TreeViewEventArgs) 'Handles globalOpsTree.AfterSelect
        Dim newSelected As TreeNode = e.Node
        If globalOpsTree.SelectedNode.ImageIndex = 2 Then
            globalOpsTree.SelectedNode.SelectedImageIndex = 2
        End If
        globalOpsListView.Items.Clear()

        currentGlobalOpsFolder = Path.Combine(_attachmentCategorySubFolderPath, globalOpsTree.SelectedNode.FullPath)

        If DropboxEnabled And _AttachmentCategory = "Projects" Then
            populateDataGridViews(globalOpsTree.SelectedNode)
        End If

        tsLblCurrentFolder.Text = "Current Folder: " & globalOpsTree.SelectedNode.Text
        _previousNode = globalOpsTree.SelectedNode

        If Directory.Exists(currentGlobalOpsFolder) Then
            _fsw = New FileSystemWatcher(currentGlobalOpsFolder)
            '_fsw.IncludeSubdirectories = True
            _fsw.EnableRaisingEvents = True
        End If

        If isAttachmentType(globalOpsTree.SelectedNode.Text) Then

            If canAttachmentTypeHaveExpirationDates(globalOpsTree.SelectedNode.Text) Then
                If chExpirationDate.Index = -1 Then
                    globalOpsListView.Columns.Add(chExpirationDate)
                End If
            Else
                If chExpirationDate.Index >= 0 Then
                    globalOpsListView.Columns.RemoveAt(chExpirationDate.Index)
                End If
            End If

        End If

        populateListView()
    End Sub

    Private Sub ListViewCopy_Click(sender As Object, e As EventArgs) Handles ListViewCopy.Click
        If globalOpsListView.SelectedItems.Count = 0 Then
            Exit Sub
        End If
        Clipboard.Clear()
        Dim returnList As New System.Collections.Specialized.StringCollection

        For Each item As ListViewItem In globalOpsListView.SelectedItems
            returnList.Add(Path.Combine(currentGlobalOpsFolder, item.Text))
        Next
        Clipboard.SetFileDropList(returnList)
    End Sub

    Private Sub RemoveFromKnowledgeBaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiRemoveSelectedToKB.Click
        Using newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()
            For Each i As ListViewItem In globalOpsListView.SelectedItems
                Dim query As New SqlCommand("dbo.add_or_remove_file_from_Clair_Knowledge_Base", newConn)
                query.CommandType = CommandType.StoredProcedure
                query.Parameters.Add("@filename", SqlDbType.VarChar, 255).Value = i.Text
                query.Parameters.Add("@GUID", SqlDbType.VarChar, 255).Value = _currentGUID
                query.Parameters.Add("@subFolderPath", SqlDbType.VarChar, 255).Value = globalOpsTree.SelectedNode.FullPath
                query.Parameters.Add("@KnowledgeBaseStatus", SqlDbType.Bit).Value = 0
                query.ExecuteNonQuery()

                i.SubItems(chIsInKnowledgeBase.DisplayIndex).Text = "No"

            Next
            newConn.Close()
        End Using
    End Sub


    Private Sub AddToKnowledgeBaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsmiAddSelectedToKB.Click
        Using newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()

            For Each i As ListViewItem In globalOpsListView.SelectedItems
                Dim query As New SqlCommand("dbo.add_or_remove_file_from_Clair_Knowledge_Base", newConn)
                query.CommandType = CommandType.StoredProcedure
                query.Parameters.Add("@filename", SqlDbType.VarChar, 255).Value = i.Text
                query.Parameters.Add("@GUID", SqlDbType.VarChar, 255).Value = _currentGUID
                query.Parameters.Add("@subFolderPath", SqlDbType.VarChar, 255).Value = globalOpsTree.SelectedNode.FullPath
                query.Parameters.Add("@KnowledgeBaseStatus", SqlDbType.Bit).Value = 1
                query.ExecuteNonQuery()

                i.SubItems(chIsInKnowledgeBase.DisplayIndex).Text = "Yes"
            Next

            newConn.Close()
        End Using
    End Sub

    Private Sub ListViewPaste_Click(sender As Object, e As EventArgs) Handles ListViewPaste.Click
        'If Clipboard.GetFileDropList().Count = 0 Then
        '    Exit Sub
        'End If
        If globalOpsTree.SelectedNode Is Nothing Then
            Exit Sub
        End If

        If _isLoadingTree Then
            Exit Sub
        End If

        checkForRootFolderDefinitionAndExistence()
        If Clipboard.ContainsData("FileGroupDescriptor") Then
            Dim theStream As Stream = DirectCast(Clipboard.GetData("FileGroupDescriptor"), Stream)
            Dim fileGroupDescriptor As Byte() = New Byte(511) {}
            theStream.Read(fileGroupDescriptor, 0, 512)
            Dim fileName As New StringBuilder("")
            Dim i As Integer = 76
            While fileGroupDescriptor(i) <> 0
                fileName.Append(Convert.ToChar(fileGroupDescriptor(i)))
                i += 1
            End While
            theStream.Close()

            Dim theFile As String = Path.Combine(currentGlobalOpsFolder, fileName.ToString())

            If theFile.Contains(".msg") Then
                Dim mobjApplication As New Application
                For Each objMi As MailItem In mobjApplication.ActiveExplorer.Selection()
                    Dim filePath As String = IO.Path.Combine(currentGlobalOpsFolder, FixFileName(objMi.Subject + ".msg"))
                    EnsureSubdirectoryExists(filePath, globalOpsTree.SelectedNode)
                    objMi.SaveAs(filePath)
                    addOrUpdateFileIndex(filePath)
                Next
            Else
                EnsureSubdirectoryExists(theFile, globalOpsTree.SelectedNode)

                Dim ms As MemoryStream = DirectCast(Clipboard.GetData("FileContents"), MemoryStream)
                Dim fileBytes As Byte() = New Byte(ms.Length - 1) {}
                ms.Position = 0
                ms.Read(fileBytes, 0, CInt(ms.Length))

                Dim fs As New FileStream(theFile, FileMode.Create)
                fs.Write(fileBytes, 0, CInt(fileBytes.Length))

                fs.Close()
                'populateListView()
            End If
        Else
            For Each f In Clipboard.GetFileDropList()
                If System.IO.File.GetAttributes(f) = FileAttributes.Directory Then
                    Dim dropfolder As TreeNode = globalOpsTree.SelectedNode

                    EnsureSubdirectoryExists(Path.Combine(currentGlobalOpsFolder, Path.GetFileName(f)), dropfolder)
                    createNewFolder(Path.Combine(currentGlobalOpsFolder, Path.GetFileName(f)), dropfolder)
                    FileSystem.CopyDirectory(f, currentGlobalOpsFolder, True)
                    Dim dirInfo As DirectoryInfo = New DirectoryInfo(currentGlobalOpsFolder)
                    For Each filePath In dirInfo.GetFiles()
                        addOrUpdateFileIndex(filePath.FullName)
                    Next
                    addSubDirectoriesToTree(dirInfo.GetDirectories(), globalOpsTree.SelectedNode)
                    globalOpsTree.SelectedNode = dropfolder
                Else
                    Dim filepath = Path.Combine(currentGlobalOpsFolder, Path.GetFileName(f))
                    EnsureSubdirectoryExists(filepath, globalOpsTree.SelectedNode)
                    System.IO.File.Copy(f, filepath)
                    addOrUpdateFileIndex(f)
                    'addFileToListView(f)
                End If
            Next
        End If

        globalOpsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
    End Sub

    Private Sub ListViewCut_Click(sender As Object, e As EventArgs) Handles ListViewCut.Click
        If globalOpsListView.SelectedItems.Count = 0 Then
            Exit Sub
        End If

        Try
            For Each i As ListViewItem In globalOpsListView.SelectedItems
                Dim filepath = Path.Combine(currentGlobalOpsFolder, globalOpsListView.SelectedItems(0).Text)
                Dim attributes As FileAttributes = File.GetAttributes(filepath)

                If attributes = FileAttributes.Directory Then
                    Directory.Delete(filepath, True)
                    Dim subfolderPath = Path.Combine(globalOpsTree.SelectedNode.FullPath, globalOpsListView.SelectedItems(0).Text)
                    Dim nodes As TreeNode() = globalOpsTree.Nodes.Find(subfolderPath, True)
                    globalOpsTree.Nodes.Remove(nodes(0))
                Else
                    File.Delete(filepath)
                    deleteFileFromIndex(filepath)
                End If
                globalOpsListView.Items.Remove(i)
            Next
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub ListView_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs) Handles globalOpsListView.DragEnter
        ' If the data is a file or a bitmap, display the copy cursor.
        If e.Data.GetDataPresent(DataFormats.FileDrop) Or e.Data.GetDataPresent("FileGroupDescriptor") Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub globalOpsTree_DragEnter(ByVal sender As Object, ByVal e As DragEventArgs) Handles globalOpsTree.DragEnter
        ' If the data is a file or a bitmap, display the copy cursor.
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If
    End Sub

    Private Sub globalOpsListView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles globalOpsListView.DragDrop

        If globalOpsTree.SelectedNode Is Nothing Then
            Exit Sub
        End If

        If _isLoadingTree Then
            Exit Sub
        End If

        checkForRootFolderDefinitionAndExistence()
        Dim UserFolderPath = globalOpsTree.SelectedNode.FullPath

        If e.Data.GetDataPresent("FileGroupDescriptor") Then
            Dim theStream As Stream = DirectCast(e.Data.GetData("FileGroupDescriptor"), Stream)
            Dim fileGroupDescriptor As Byte() = New Byte(511) {}
            theStream.Read(fileGroupDescriptor, 0, 512)
            Dim fileName As New StringBuilder("")
            Dim i As Integer = 76
            While fileGroupDescriptor(i) <> 0
                fileName.Append(Convert.ToChar(fileGroupDescriptor(i)))
                i += 1
            End While
            theStream.Close()

            Dim theFile As String = Path.Combine(currentGlobalOpsFolder, fileName.ToString())

            If theFile.Contains(".msg") Then
                Dim mobjApplication As New Application
                For Each objMi As MailItem In mobjApplication.ActiveExplorer.Selection()
                    Dim filePath As String = IO.Path.Combine(currentGlobalOpsFolder, FixFileName(objMi.Subject + ".msg"))
                    EnsureSubdirectoryExists(filePath, globalOpsTree.SelectedNode)
                    Try
                        objMi.SaveAs(filePath)
                    Catch ex As System.Exception
                        MessageBox.Show($"Error in Drag and Drop function for mail item {filePath}: {ex.Message}")
                    End Try
                    addOrUpdateFileIndex(filePath)
                Next
            Else
                EnsureSubdirectoryExists(theFile, globalOpsTree.SelectedNode)

                Dim ms As MemoryStream = DirectCast(e.Data.GetData("FileContents", True), MemoryStream)
                Dim fileBytes As Byte() = New Byte(ms.Length - 1) {}
                ms.Position = 0
                ms.Read(fileBytes, 0, CInt(ms.Length))

                Try
                    Dim fs As New FileStream(theFile, FileMode.Create)
                    fs.Write(fileBytes, 0, CInt(fileBytes.Length))
                    fs.Close()
                Catch ex As System.Exception
                    MessageBox.Show($"Error in Drag and Drop function for mail item stream {theFile}: {ex.Message}")
                End Try
            End If

        Else
            Dim files() As String = e.Data.GetData(DataFormats.FileDrop)
            For Each fileName In files
                If System.IO.File.GetAttributes(fileName).HasFlag(System.IO.FileAttributes.Directory) Then
                    'method createNewFolder selects the newly created folder in the treeview and will create an incorrect folder structure when dropping multiple folders at once.
                    Dim dropfolder As TreeNode = globalOpsTree.SelectedNode

                    EnsureSubdirectoryExists(Path.Combine(currentGlobalOpsFolder, Path.GetFileName(fileName)), dropfolder)
                    createNewFolder(Path.Combine(currentGlobalOpsFolder, Path.GetFileName(fileName)), dropfolder)

                    Try
                        FileSystem.CopyDirectory(fileName, currentGlobalOpsFolder, True)
                    Catch ex As System.Exception
                        MessageBox.Show($"Error in Drag and Drop error copying directory {fileName} to {currentGlobalOpsFolder}: {ex.Message}")
                    End Try

                    Dim dirInfo As DirectoryInfo = New DirectoryInfo(currentGlobalOpsFolder)
                    For Each filePath In dirInfo.GetFiles()
                        addOrUpdateFileIndex(filePath.FullName)
                    Next
                    addSubDirectoriesToTree(dirInfo.GetDirectories(), globalOpsTree.SelectedNode)
                    globalOpsTree.SelectedNode = dropfolder
                Else
                    Dim DestinationFilePath = Path.Combine(currentGlobalOpsFolder, Path.GetFileName(fileName))
                    EnsureSubdirectoryExists(DestinationFilePath, globalOpsTree.SelectedNode)
                    Try
                        System.IO.File.Copy(fileName, DestinationFilePath)
                    Catch ex As System.Exception
                        MessageBox.Show($"Error in Drag and Drop function for file {fileName} to {DestinationFilePath}: {ex.Message}")
                    End Try
                    addOrUpdateFileIndex(fileName)

                End If

            Next

        End If
        ' globalOpsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
    End Sub

    'Private Sub AllowUserToEditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AllowUserToEditToolStripMenuItem.Click

    '    Dim FilesNotGivenPermissionFor As String = String.Empty

    '    For Each file As ListViewItem In globalOpsListView.SelectedItems

    '        Dim filePath = Path.Combine(currentGlobalOpsFolder, file.Text)

    '        Dim fs As FileSecurity = System.IO.File.GetAccessControl(filePath)
    '        Dim sid As IdentityReference = fs.GetOwner(GetType(SecurityIdentifier))
    '        Dim ntaccount As IdentityReference = sid.Translate(GetType(NTAccount))
    '        Dim owner As String = ntaccount.ToString()

    '        Dim currentUser As String = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString()

    '        If currentUser <> owner Then
    '            FilesNotGivenPermissionFor = FilesNotGivenPermissionFor & ", " & file.Text
    '            Continue For
    '        End If

    '        Dim fileInfo As New FileInfo(filePath)
    '        Dim group As String = "cwagner@clairglobal.com"
    '        Dim FileAcl As New FileSecurity

    '        FileAcl.AddAccessRule(New FileSystemAccessRule(group, FileSystemRights.Modify, AccessControlType.Allow))
    '        FileAcl.AddAccessRule(New FileSystemAccessRule(group, FileSystemRights.Write, AccessControlType.Allow))
    '        FileAcl.AddAccessRule(New FileSystemAccessRule(group, FileSystemRights.ReadAndExecute, AccessControlType.Allow))

    '        fileInfo.SetAccessControl(FileAcl)
    '    Next
    'End Sub

    Private Sub globalOpsTree_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles globalOpsTree.DragDrop
        Dim files() As String = e.Data.GetData(DataFormats.FileDrop)

        Dim mousePos As Point = globalOpsTree.PointToClient(Windows.Forms.Cursor.Position)
        Dim nodeOver As TreeNode = globalOpsTree.GetNodeAt(mousePos)

        checkForRootFolderDefinitionAndExistence()

        If nodeOver IsNot Nothing Then
            globalOpsTree.SelectedNode = nodeOver
            Try
                EnsureSubdirectoryExists(Path.Combine(_attachmentCategorySubFolderPath, nodeOver.FullPath, "xxxxxxxx"), nodeOver)
                For Each f In files
                    If System.IO.File.GetAttributes(f) = FileAttributes.Directory Then

                        createNewFolder(Path.Combine(currentGlobalOpsFolder, Path.GetFileName(f)), nodeOver)
                        FileSystem.CopyDirectory(f, currentGlobalOpsFolder, True)

                        'For Each subdir In info.GetDirectories("*", IO.SearchOption.AllDirectories)

                        '    Dim test = Path.Combine(nodeOver.Text, globalOpsTree.SelectedNode.Text)
                        '    Dim TreeSeach As TreeNode() = globalOpsTree.Nodes.Find(Path.Combine(nodeOver.Text, globalOpsTree.SelectedNode.FullPath), True)

                        '    Dim parentNode As TreeNode
                        '    If TreeSeach.Length > 0 Then
                        '        parentNode = TreeSeach(0)
                        '    Else
                        '        parentNode = Nothing
                        '    End If

                        '    createNewFolder(subdir.FullName, parentNode)
                        'Next


                        Dim dirInfo As DirectoryInfo = New DirectoryInfo(currentGlobalOpsFolder)
                        For Each filePath In dirInfo.GetFiles()
                            addOrUpdateFileIndex(filePath.FullName)
                        Next
                    Else
                        File.Copy(f, Path.Combine(_attachmentCategorySubFolderPath, nodeOver.FullPath, Path.GetFileName(f)), True)
                        addOrUpdateFileIndex(f)
                    End If
                Next

            Catch ex As System.Exception

            End Try

            RefreshFileExplorer()
            Exit Sub
        End If

        For Each f In files
            Dim dropDirectoryInfo As New DirectoryInfo(f)
            Dim newDirectoryPath = Path.Combine(_attachmentCategorySubFolderPath, dropDirectoryInfo.Name)
            Dim DestinationDirectoryInfo As New DirectoryInfo(newDirectoryPath)

            If Not dropDirectoryInfo.Exists Then
                Exit Sub
            End If

            If Not DestinationDirectoryInfo.Exists Then
                createNewFolder(newDirectoryPath)
            End If

            FileSystem.CopyDirectory(dropDirectoryInfo.FullName, newDirectoryPath, True)

            For Each file As String In Directory.GetFiles(newDirectoryPath)
                addOrUpdateFileIndex(file)
            Next

        Next
        PopulateTreeView()
    End Sub

    Private Sub fileCreatedOrChanged(sender As Object, e As FileSystemEventArgs) Handles _fsw.Changed, _fsw.Created, _fsw.Deleted
        If _isLoadingTree Then
            Exit Sub
        End If




        _isLoadingTree = True
        _LiveUpdateThread = New System.Threading.Thread(AddressOf populateListViewThread) With {.Name = "populateListView"}
        _LiveUpdateThread.Start()
        _LiveUpdateThread = Nothing
        _isLoadingTree = False
    End Sub

    Private Sub populateListViewThread()
        AccessLvFiles()
    End Sub

    Private Sub AccessLvFiles()
        If Me.InvokeRequired Then
            Try
                Me.Invoke(New MethodInvoker(AddressOf AccessLvFiles))
            Catch ex As System.Exception

            End Try

        Else
            populateListView()
        End If
    End Sub

    Private Sub ListViewSendToEmail_Click(sender As Object, e As EventArgs) Handles ListViewSendToEmail.Click
        If globalOpsListView.SelectedItems.Count = 0 Then
            MessageBox.Show("Please Select a File to attach to the email")
            Exit Sub
        End If
        Dim filePaths As New List(Of String)
        For Each item As ListViewItem In globalOpsListView.SelectedItems
            filePaths.Add(Path.Combine(currentGlobalOpsFolder, item.Text))
        Next
        OpenOutlookMail("", "", "", filePaths, True)
    End Sub

    Private Sub ListViewSendToDesktop_Click(sender As Object, e As EventArgs) Handles ListViewSendToDesktop.Click
        If globalOpsListView.SelectedItems.Count = 0 Then
            MessageBox.Show("Please Select a File to Send to Desktop")
            Exit Sub
        End If
        For Each item As ListViewItem In globalOpsListView.SelectedItems
            System.IO.File.Copy(Path.Combine(currentGlobalOpsFolder, item.Text), Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop), item.Text))
        Next
    End Sub

    Private Sub DocumentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListViewSendToDocuments.Click
        If globalOpsListView.SelectedItems.Count = 0 Then
            MessageBox.Show("Please Select a File to Send to Documents")
            Exit Sub
        End If
        For Each item As ListViewItem In globalOpsListView.SelectedItems
            System.IO.File.Copy(Path.Combine(currentGlobalOpsFolder, item.Text), Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), item.Text))
        Next
    End Sub

    Private Sub Rename_Click(sender As Object, e As EventArgs) Handles ListViewRename.Click
        If globalOpsListView.SelectedItems.Count = 0 Then
            MessageBox.Show("Please Select a File to Rename")
            Exit Sub
        End If

        Try
            globalOpsListView.SelectedItems(0).BeginEdit()
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub MyListView_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.LabelEditEventArgs) Handles globalOpsListView.AfterLabelEdit
        Dim oldPath As String = Path.Combine(currentGlobalOpsFolder, globalOpsListView.SelectedItems(0).Text)
        Dim extension As String = globalOpsListView.SelectedItems(0).SubItems(1).Text

        If Path.GetExtension(oldPath) Is String.Empty Then
            oldPath = oldPath & extension
        End If

        If e.Label Is Nothing Then
            Exit Sub
        End If

        Dim newFileName = e.Label

        If Path.GetExtension(e.Label) Is String.Empty Then
            newFileName = e.Label & extension
        End If

        Try

            If System.IO.File.GetAttributes(Path.Combine(currentGlobalOpsFolder, globalOpsListView.SelectedItems(0).Text)) = FileAttributes.Directory Then
                If Not Directory.Exists(Path.Combine(currentGlobalOpsFolder, globalOpsListView.SelectedItems(0).Text)) Then
                    Exit Sub
                End If
                RenameFolder(e.Label, Path.Combine(globalOpsTree.SelectedNode.FullPath, e.Label), Path.Combine(globalOpsTree.SelectedNode.FullPath, globalOpsListView.SelectedItems(0).Text))
            Else
                If Not System.IO.File.Exists(oldPath) Then
                    Exit Sub
                End If

                My.Computer.FileSystem.RenameFile(oldPath, newFileName)
                'If DropboxEnabled Then
                '    If globalOpsListView.SelectedItems(0).SubItems(5).Text = "Yes" Then
                '        Using newConn As New SqlConnection(FinesseConnectionString)
                '            newConn.Open()
                '            Dim query As New SqlCommand($"UPDATE dbo.CloudFileStorageUploadedFiles
                '                             SET fileName = '{newFileName}'
                '                             WHERE fileName = '{Path.GetFileName(oldPath)}' AND entityno = '{currentEntityno}' AND UserFolderPath = '{globalOpsTree.SelectedNode.Text}'", newConn)
                '            query.ExecuteNonQuery()
                '            newConn.Close()
                '        End Using
                '    End If
                'End If
            End If
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
        'look for shorter version

    End Sub

    Private Sub RenameToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TreeViewRename.Click
        If isAttachmentType(globalOpsTree.SelectedNode.Tag) Then
            Exit Sub
        End If

        If globalOpsTree.SelectedNode.FullPath.EndsWith("- Archive") Then
            Exit Sub
        End If

        globalOpsTree.SelectedNode.BeginEdit()
    End Sub

    Private Sub globalOpsTreeview_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles globalOpsTree.AfterLabelEdit
        If e.Label Is Nothing Then
            Exit Sub
        End If

        Dim oldSubFolderPath = globalOpsTree.SelectedNode.FullPath
        globalOpsTree.SelectedNode.Text = e.Label

        RenameFolder(e.Label, globalOpsTree.SelectedNode.FullPath, oldSubFolderPath)
    End Sub

    Private Sub TreeViewCopy_Click(sender As Object, e As EventArgs) Handles TreeViewCopy.Click
        Clipboard.Clear()
        Dim returnList As New System.Collections.Specialized.StringCollection
        returnList.Add(currentGlobalOpsFolder)
        Clipboard.SetFileDropList(returnList)
    End Sub

    Private Sub buttonRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            RefreshFileExplorer()
        Catch ex As System.Exception
            _isLoadingTree = False
            globalOpsTree.UseWaitCursor = False
        End Try

    End Sub

    Private Sub RefreshFileExplorer()
        Dim selectedNodeName As String = Nothing
        If globalOpsTree.SelectedNode IsNot Nothing Then
            selectedNodeName = globalOpsTree.SelectedNode.FullPath
        End If

        If _isLoadingTree = False Then
            If DropboxEnabled Then
                loadLocallyManipulatedDropBoxTables()
            End If

            If _canAddToKnowledgebase Then
                loadFileIndex()
            End If

            loadAvailableInvitees()
            PopulateTreeView()
        End If

        If selectedNodeName IsNot Nothing Then
            Dim node As TreeNode() = globalOpsTree.Nodes.Find(selectedNodeName, True)
            If node.Length > 0 Then
                globalOpsTree.SelectedNode = node(0)
            End If
        End If

    End Sub


    Private Sub lstvicon_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles globalOpsListView.DoubleClick
        Dim filePath = Path.Combine(_attachmentCategorySubFolderPath, globalOpsTree.SelectedNode.FullPath, globalOpsListView.SelectedItems(0).Text)

        If (System.IO.File.Exists(filePath)) Then
            Process.Start(Path.Combine(_attachmentCategorySubFolderPath, globalOpsTree.SelectedNode.FullPath, globalOpsListView.SelectedItems(0).Text))
        ElseIf (Directory.Exists(filePath)) Then
            Dim nodeToSelect = globalOpsTree.Nodes.Find(Path.Combine(globalOpsTree.SelectedNode.FullPath, globalOpsListView.SelectedItems(0).Text), True)
            If nodeToSelect.Length > 0 Then
                globalOpsTree.SelectedNode = nodeToSelect(0)
            End If
        End If
    End Sub

    Private Sub ListViewCopyFilePath_Click(sender As Object, e As EventArgs) Handles ListViewCopyFilePath.Click
        If globalOpsListView.SelectedItems.Count = 0 Then
            MessageBox.Show("Please Select a File")
            Exit Sub
        End If
        Clipboard.SetText(Path.Combine(currentGlobalOpsFolder, globalOpsListView.SelectedItems(0).Text))
    End Sub

    Private Sub TreeViewCopyFolderPath_Click(sender As Object, e As EventArgs) Handles TreeViewCopyFolderPath.Click
        checkForRootFolderDefinitionAndExistence()
        'Ensure subdirectory exists gets the parent of what ever file/folder has been passed to it so a sub directory is added to create the proper directory
        EnsureSubdirectoryExists(Path.Combine(currentGlobalOpsFolder, "xxxxxxxxxxxxxxxxxxxxxxxxx"), globalOpsTree.SelectedNode)
        Clipboard.SetText(currentGlobalOpsFolder)
    End Sub

    Private Sub TreeViewCreateFolderInSelectedFolder_Click(sender As Object, e As EventArgs) Handles TreeViewCreateFolderInSelectedFolder.Click
        If _isLoadingTree Then
            Exit Sub
        End If

        'If globalOpsTree.SelectedNode Is Nothing Then
        '    MessageBox.Show("Plese s")
        'End If


        Dim newFolderName As Object
        newFolderName = InputBox("What would you like to name the folder?", "New Folder")

        If newFolderName = "" Then
            Exit Sub
        End If

        checkForRootFolderDefinitionAndExistence()

        Dim newFolderPath = Path.Combine(_attachmentCategorySubFolderPath, globalOpsTree.SelectedNode.FullPath, newFolderName)

        createNewFolder(newFolderPath, globalOpsTree.SelectedNode)

        addFolderToListView(newFolderPath)
        globalOpsListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)
    End Sub

    Private Sub deleteKeyPressed(sender As Object, e As KeyEventArgs) Handles globalOpsListView.KeyUp
        If e.KeyCode = Keys.Delete Then
            Try
                For Each i As ListViewItem In globalOpsListView.SelectedItems
                    Dim filepath = Path.Combine(currentGlobalOpsFolder, globalOpsListView.SelectedItems(0).Text)
                    Dim attributes As FileAttributes = File.GetAttributes(filepath)

                    If attributes = FileAttributes.Directory Then
                        System.IO.Directory.Delete(filepath, True)
                        Dim subfolderPath = Path.Combine(globalOpsTree.SelectedNode.FullPath, globalOpsListView.SelectedItems(0).Text)
                        Dim nodes As TreeNode() = globalOpsTree.Nodes.Find(subfolderPath, True)
                        globalOpsTree.Nodes.Remove(nodes(0))
                    Else
                        File.Delete(filepath)
                        deleteFileFromIndex(filepath)
                    End If
                    globalOpsListView.Items.Remove(i)
                Next
            Catch ex As System.Exception
                MessageBox.Show(ex.Message)
            End Try

        End If
    End Sub

    Private Sub ListViewListViewOpenInFileExplorerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListViewOpenInFileExplorerToolStripMenuItem.Click
        Try
            Process.Start(currentGlobalOpsFolder)
        Catch ex As System.Exception

        End Try

    End Sub

    Private Sub TreeViewOpenFolderInFileExplorerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TreeViewOpenFolderInFileExplorerToolStripMenuItem.Click

        checkForRootFolderDefinitionAndExistence()
        Dim filepath = Path.Combine(currentGlobalOpsFolder, "xxxxxx")
        EnsureSubdirectoryExists(filepath, globalOpsTree.SelectedNode)

        Try
            Process.Start(currentGlobalOpsFolder)
        Catch ex As System.Exception

        End Try

    End Sub

    Private Sub btnCreateDeskTopShortCut_Click(sender As Object, e As EventArgs) Handles btnCreateDeskTopShortCut.Click

        If Not Directory.Exists(attachmentCategorySubFolderPath) Then
            System.IO.Directory.CreateDirectory(attachmentCategorySubFolderPath)
        End If

        createDeskTopShortCut()
    End Sub
    Private Sub PrintToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PrintToolStripMenuItem.Click
        Try
            Dim filename = Path.Combine(currentGlobalOpsFolder, globalOpsListView.SelectedItems(0).Text)

            Dim startInfo = New ProcessStartInfo(filename)

            startInfo.UseShellExecute = True
            startInfo.Verb = "PrintTo"
            startInfo.WindowStyle = ProcessWindowStyle.Hidden

            Process.Start(startInfo)
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub ListViewItemDrag(sender As Object, e As EventArgs) Handles globalOpsListView.ItemDrag
        Dim strFullPath As String

        Dim items As ListView.SelectedListViewItemCollection = globalOpsListView.SelectedItems

        Dim DropList As New StringCollection

        For Each item As ListViewItem In items

            strFullPath = Path.Combine(currentGlobalOpsFolder, item.Text)

            Dim FtoDrop As FileInfo = New FileInfo(strFullPath)

            DropList.Add(strFullPath)
        Next

        Dim DragPaths As New DataObject()

        DragPaths.SetFileDropList(DropList)

        DoDragDrop(DragPaths, DragDropEffects.All)

    End Sub

    'this was just moved
    Private Sub globalOpsListView_SelectedIndexChanged(sender As Object, e As ColumnClickEventArgs) Handles globalOpsListView.ColumnClick
        If e.Column = 2 Then
            If globalOpsListView.Sorting = Nothing Or globalOpsListView.Sorting = SortOrder.Ascending Then
                globalOpsListView.Sorting = SortOrder.Descending
                globalOpsListView.ListViewItemSorter = New ListViewItemDateComparer(e.Column, SortOrder.Descending)
            Else
                globalOpsListView.Sorting = SortOrder.Ascending
                globalOpsListView.ListViewItemSorter = New ListViewItemDateComparer(e.Column, SortOrder.Ascending)
            End If
        Else
            Dim test As ListViewItemDateComparer = New ListViewItemDateComparer(e.Column, SortOrder.Ascending)
            If globalOpsListView.ListViewItemSorter.GetType = test.GetType Then
                globalOpsListView.ListViewItemSorter = New ListViewColumnSorter
            End If

            Dim sorter As ListViewColumnSorter = globalOpsListView.ListViewItemSorter

            sorter.SortColumn = e.Column

            If (e.Column = sorter.SortColumn) Then
                ' Reverse the current sort direction for this column.
                If (sorter.Order = System.Windows.Forms.SortOrder.Ascending) Then
                    sorter.Order = System.Windows.Forms.SortOrder.Descending
                Else
                    sorter.Order = System.Windows.Forms.SortOrder.Ascending
                End If
            Else
                ' Set the column number that is to be sorted; default to ascending.
                sorter.SortColumn = e.Column
                sorter.Order = System.Windows.Forms.SortOrder.Ascending
            End If

            globalOpsListView.Sort()
        End If

    End Sub

    Private Sub HideArchivedFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HideArchivedFilesToolStripMenuItem.Click
        HideArchivedFilesToolStripMenuItem.Checked = True
        ShowArchivedFilesToolStripMenuItem.Checked = False
        _displayArchivedFolders = False
        PopulateTreeView()
    End Sub

    Private Sub ShowArchivedFilesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ShowArchivedFilesToolStripMenuItem.Click
        HideArchivedFilesToolStripMenuItem.Checked = False
        ShowArchivedFilesToolStripMenuItem.Checked = True
        _displayArchivedFolders = True
        PopulateTreeView()
    End Sub
    Private Sub tsmiSetDocumentExpirationDate_Click(sender As Object, e As EventArgs) Handles tsmiSetDocumentExpirationDate.Click
        If _isLoadingTree Then
            Exit Sub
        End If

        Dim newDate As Object
        newDate = InputBox("What would you like to make the Expiration Date? Please use YYYY/MM/DD or MM//DD/YYYY format.", "Expiration Date")

        If newDate = "" Then
            Exit Sub
        End If

        Dim isValidDate As Boolean = IsDate(newDate)

        If isValidDate Then
            Try

                'refactor idea find all calls like this and subsititue with a function.
                Dim filePath = Path.Combine(currentGlobalOpsFolder, globalOpsListView.SelectedItems(0).Text)

                Dim fs As FileSecurity = System.IO.File.GetAccessControl(filePath)
                Dim sid As IdentityReference = fs.GetOwner(GetType(SecurityIdentifier))
                Dim ntaccount As IdentityReference = sid.Translate(GetType(NTAccount))
                Dim owner As String = ntaccount.ToString()

                Dim dirInfo As DirectoryInfo = New DirectoryInfo(filePath)

                Using newConn As New SqlConnection(FinesseConnectionString)
                    newConn.Open()
                    Dim cmd As New SqlCommand("dbo.set_file_expiration_date", newConn)
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@fileName", SqlDbType.VarChar, 255).Value = Path.GetFileName(filePath)
                    cmd.Parameters.Add("@CreationTime", SqlDbType.DateTime2).Value = dirInfo.CreationTimeUtc
                    cmd.Parameters.Add("@GUID", SqlDbType.VarChar, 255).Value = currentGUID
                    cmd.Parameters.Add("@subFolderPath", SqlDbType.VarChar, 255).Value = globalOpsTree.SelectedNode.FullPath
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 255).Value = owner
                    cmd.Parameters.Add("@ExpirationDate", SqlDbType.DateTime2).Value = newDate
                    cmd.ExecuteNonQuery()
                    'newConn.Close()
                End Using
                newConn.Close()


                globalOpsListView.SelectedItems(0).SubItems(chExpirationDate.Index).Text = newDate
                Dim hasExpirationDate As DataRow() = _dtFileIndex.Select($"SubFolderPath = {globalOpsTree.SelectedNode.FullPath.SQLQuote} and fileName = {globalOpsListView.SelectedItems(0).Text.SQLQuote}")

                If hasExpirationDate.Any Then
                    hasExpirationDate(0).Item("DocumentExpirationDateUTC") = newDate
                End If


            Catch ex As System.Exception
                MessageBox.Show($"Error when setting expiration date: {ex.Message}")
            End Try
        Else
            MessageBox.Show($"The entered value was unable to be parsed into a date: {newDate}. Please use YYYY/MM/DD or MM//DD/YYYY format.")
        End If

    End Sub



#End Region

    '#Region "Egnyte"

    '    'Private Sub tsbShareFolderWithSelected_Click(sender As Object, e As EventArgs) Handles tsddbShareFolderWithSelected.Click
    '    '    If globalOpsTree.SelectedNode Is Nothing Then
    '    '        MessageBox.Show("There is no folder selected. Please select a folder to share and try again")
    '    '    End If

    '    '    Dim saveTable As DataTable = createEgnyteSaveDataTable()

    '    '    Dim filePaths As New List(Of String)
    '    '    filePaths.Add(currentGlobalOpsFolder)

    '    '    saveTable = setEgnyteSaveDataTable(saveTable, True, filePaths, Nothing, dgvAvailableMembers.SelectedRows)

    '    '    Dim listOfEmails As List(Of String) = createEmailListForEgnyteSaveList(Nothing, dgvAvailableMembers.SelectedRows)

    '    '    saveEgnytePermissionChanges(saveTable, listOfEmails, True)



    '    'End Sub

    '    'Private Sub tsddbShareFolderWithAll_Click(sender As Object, e As EventArgs) Handles tsddbShareFolderWithAll.Click
    '    '    If globalOpsTree.SelectedNode Is Nothing Then
    '    '        MessageBox.Show("There is no folder selected. Please select a folder to share and try again")
    '    '    End If

    '    '    Dim saveTable As DataTable = createEgnyteSaveDataTable()

    '    '    Dim filePaths As New List(Of String)
    '    '    filePaths.Add(currentGlobalOpsFolder)

    '    '    saveTable = setEgnyteSaveDataTable(saveTable, True, filePaths, dgvAvailableMembers.Rows)

    '    '    Dim listOfEmails As List(Of String) = createEmailListForEgnyteSaveList(dgvAvailableMembers.Rows)

    '    '    saveEgnytePermissionChanges(saveTable, listOfEmails, True)

    '    'End Sub

    '    'Private Sub btnShareSelected_Click(sender As Object, e As EventArgs) Handles tsddbShareProjectWithSelected.Click

    '    '    Dim saveTable As DataTable = createEgnyteSaveDataTable()

    '    '    Dim filePaths As New List(Of String)
    '    '    filePaths.Add(attachmentCategorySubFolderPath)

    '    '    saveTable = setEgnyteSaveDataTable(saveTable, True, filePaths, Nothing, dgvAvailableMembers.SelectedRows)

    '    '    Dim listOfEmails As List(Of String) = createEmailListForEgnyteSaveList(Nothing, dgvAvailableMembers.SelectedRows)

    '    '    saveEgnytePermissionChanges(saveTable, listOfEmails, True)

    '    'End Sub



    '    'Private Sub btnShareAll_Click(sender As Object, e As EventArgs) Handles tsddbShareProjectWithAll.Click

    '    '    Dim saveTable As DataTable = createEgnyteSaveDataTable()

    '    '    Dim filePaths As New List(Of String)
    '    '    filePaths.Add(attachmentCategorySubFolderPath)

    '    '    saveTable = setEgnyteSaveDataTable(saveTable, True, filePaths, dgvAvailableMembers.Rows)

    '    '    Dim listOfEmails As List(Of String) = createEmailListForEgnyteSaveList(dgvAvailableMembers.Rows)

    '    '    saveEgnytePermissionChanges(saveTable, listOfEmails, True)

    '    'End Sub







    '    'Private Sub RevokeProjectFromSelectedToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsddbRevokeProjectFromSelected.Click

    '    '    Dim saveTable As DataTable = createEgnyteSaveDataTable()

    '    '    Dim filePaths As New List(Of String)
    '    '    filePaths.Add(attachmentCategorySubFolderPath)

    '    '    saveTable = setEgnyteSaveDataTable(saveTable, False, filePaths, Nothing, dgvCurrentMembers.SelectedRows)

    '    '    Dim listOfEmails As List(Of String) = createEmailListForEgnyteSaveList(Nothing, dgvCurrentMembers.SelectedRows)

    '    '    saveEgnytePermissionChanges(saveTable, listOfEmails, False)
    '    'End Sub


    '    'Private Sub RevokePToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles tsddbRevokeProjectFromAll.Click

    '    '    Dim saveTable As DataTable = createEgnyteSaveDataTable()

    '    '    Dim filePaths As New List(Of String)
    '    '    filePaths.Add(attachmentCategorySubFolderPath)

    '    '    saveTable = setEgnyteSaveDataTable(saveTable, False, filePaths, dgvCurrentMembers.Rows)

    '    '    Dim listOfEmails As List(Of String) = createEmailListForEgnyteSaveList(dgvCurrentMembers.Rows)

    '    '    saveEgnytePermissionChanges(saveTable, listOfEmails, False)
    '    '    'For Each row As DataGridViewRow In dgvAvailableMembers.Rows
    '    '    '    Dim newRow As DataRow = saveTable.NewRow

    '    '    '    newRow.Item("email") = row.Cells(1).Value.ToString
    '    '    '    newRow.Item("name") = row.Cells(0).Value.ToString
    '    '    '    newRow.Item("FileStorageGUID") = currentGUID
    '    '    '    newRow.Item("FileStoragePath") = attachmentCategorySubFolderPath
    '    '    '    newRow.Item("isExecuted") = 0
    '    '    '    newRow.Item("CreationTimeTimeUTC") = DateTime.UtcNow
    '    '    '    newRow.Item("AttachmentCategory") = attachmentCategory
    '    '    '    newRow.Item("note") = ""
    '    '    '    newRow.Item("isAddFolderMember") = 1
    '    '    '    newRow.Item("isRemoveFolderMember") = 0

    '    '    '    saveTable.Rows.Add(newRow)
    '    '    '    listOfEmails.Add(row.Cells(1).Value.ToString)
    '    '    'Next

    '    'End Sub

    '    'Private Sub tsddbRevokeFolderFromSelected_Click(sender As Object, e As EventArgs) Handles tsddbRevokeFolderFromSelected.Click

    '    '    Dim saveTable As DataTable = createEgnyteSaveDataTable()

    '    '    Dim filePaths As New List(Of String)
    '    '    filePaths.Add(currentGlobalOpsFolder)

    '    '    saveTable = setEgnyteSaveDataTable(saveTable, False, filePaths, Nothing, dgvCurrentMembers.SelectedRows)

    '    '    Dim listOfEmails As List(Of String) = createEmailListForEgnyteSaveList(Nothing, dgvCurrentMembers.SelectedRows)

    '    '    saveEgnytePermissionChanges(saveTable, listOfEmails, False)

    '    'End Sub

    '    'Private Sub tsddbRevokeFolderFromAll_Click(sender As Object, e As EventArgs) Handles tsddbRevokeFolderFromAll.Click

    '    '    Dim saveTable As DataTable = createEgnyteSaveDataTable()

    '    '    Dim filePaths As New List(Of String)
    '    '    filePaths.Add(currentGlobalOpsFolder)

    '    '    saveTable = setEgnyteSaveDataTable(saveTable, False, filePaths, dgvCurrentMembers.Rows)

    '    '    Dim listOfEmails As List(Of String) = createEmailListForEgnyteSaveList(dgvCurrentMembers.Rows)

    '    '    saveEgnytePermissionChanges(saveTable, listOfEmails, False)

    '    'End Sub



    '    Private Sub saveEgnytePermissionChanges(saveTable As DataTable, listOfEmails As List(Of String), openShareEmail As Boolean)
    '        Try
    '            Dim tmpConn As SqlConnection = GetOpenedFinesseConnection()

    '            Dim cmd As New SqlCommand("commit_Egnyte_Share_Requests", tmpConn)
    '            cmd.CommandType = CommandType.StoredProcedure
    '            cmd.Parameters.AddWithValue("@shareTable", saveTable)
    '            cmd.Parameters.Add("@server", SqlDbType.VarChar, 50)
    '            cmd.Parameters("@server").Direction = ParameterDirection.Output
    '            cmd.Parameters.Add("@EgnyteRootURI", SqlDbType.VarChar, 100)
    '            cmd.Parameters("@EgnyteRootURI").Direction = ParameterDirection.Output
    '            cmd.Parameters.Add("@FileStorageEntityDesc", SqlDbType.VarChar, 500)
    '            cmd.Parameters("@FileStorageEntityDesc").Direction = ParameterDirection.Output
    '            cmd.CommandTimeout = 120
    '            cmd.ExecuteNonQuery()

    '            Dim server As String = cmd.Parameters("@server").Value
    '            Dim rootURI As String = cmd.Parameters("@EgnyteRootURI").Value
    '            Dim FileStorageEntityDesc As String = cmd.Parameters("@FileStorageEntityDesc").Value

    '            Dim myReq As HttpWebRequest
    '            Dim myResp As HttpWebResponse
    '            Dim myReader As StreamReader
    '            ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
    '            myReq = HttpWebRequest.Create($"{_EgnyteSharingURI}/{server}/{_currentGUID}")
    '            myResp = myReq.GetResponse
    '            myReader = New System.IO.StreamReader(myResp.GetResponseStream)

    '            'String Path = r.fileStoragePath.Replace("X:\\", "");
    '            '        Path = Path.Replace("\\", "/"); 

    '            Dim SharePath As String = attachmentCategorySubFolderPath.Replace("X:\", "")

    '            Dim url As String = Path.Combine(rootURI, SharePath).Replace("\", "/")

    '            Dim body As String = $"<p> <a href=""{url}"">{url} </a> </p>  <p>Above is a link to the project: {FileStorageEntityDesc} </p>"

    '            If openShareEmail Then
    '                OpenOutlookMail(String.Join(";", listOfEmails), $"Egnyte Share Link for {FileStorageEntityDesc}", body, "", True)
    '            End If
    '        Catch ex As System.Exception
    '            MessageBox.Show($"Error saving shares {ex.Message}")
    '        End Try
    '    End Sub


    '    Private Function createEgnyteSaveDataTable() As DataTable

    '        Dim saveTable As New DataTable

    '        saveTable.Columns.Add("email", GetType(String))
    '        saveTable.Columns.Add("name", GetType(String))
    '        saveTable.Columns.Add("FileStorageGUID", GetType(Guid))
    '        saveTable.Columns.Add("FileStoragePath", GetType(String))
    '        saveTable.Columns.Add("isExecuted", GetType(Boolean))
    '        saveTable.Columns.Add("CreationTimeTimeUTC", GetType(DateTime))
    '        saveTable.Columns.Add("AttachmentCategory", GetType(String))
    '        saveTable.Columns.Add("note", GetType(String))
    '        saveTable.Columns.Add("isAddFolderMember", GetType(Boolean))
    '        saveTable.Columns.Add("isRemoveFolderMember", GetType(Boolean))

    '        Return saveTable

    '    End Function

    '    Private Function setEgnyteSaveDataTable(saveTable As DataTable, isAdd As Boolean, paths As List(Of String), Optional allRows As DataGridViewRowCollection = Nothing, Optional SelectedRows As DataGridViewSelectedRowCollection = Nothing) As DataTable

    '        Dim rows

    '        If allRows IsNot Nothing Then
    '            rows = allRows
    '        Else
    '            rows = SelectedRows
    '        End If

    '        For Each row In rows

    '            For Each path In paths
    '                Dim newRow As DataRow = saveTable.NewRow

    '                newRow.Item("email") = row.Cells(1).Value.ToString
    '                newRow.Item("name") = row.Cells(0).Value.ToString
    '                newRow.Item("FileStorageGUID") = currentGUID
    '                newRow.Item("FileStoragePath") = path
    '                newRow.Item("isExecuted") = 0
    '                newRow.Item("CreationTimeTimeUTC") = DateTime.UtcNow
    '                newRow.Item("AttachmentCategory") = attachmentCategory
    '                newRow.Item("note") = ""
    '                newRow.Item("isAddFolderMember") = isAdd
    '                newRow.Item("isRemoveFolderMember") = Not isAdd

    '                saveTable.Rows.Add(newRow)

    '                setSharedStatus(row.Cells(1).Value.ToString, row.Cells(0).Value.ToString, isAdd, path)
    '            Next

    '        Next


    '        Return saveTable
    '    End Function

    '    Private Sub setSharedStatus(email As String, Name As String, isShared As Boolean, folder As String)

    '        Dim doesEmailHaveShareRow As DataRow() = _dtCurrentAccess.Select($"email = {email.SQLQuote} AND FileStoragePath = {folder.SQLQuote}")

    '        If doesEmailHaveShareRow.Any Then
    '            doesEmailHaveShareRow(0).Item("isAddFolderMember") = isShared
    '            doesEmailHaveShareRow(0).Item("isRemoveFolderMember") = Not isShared
    '        Else
    '            Dim newRow = _dtCurrentAccess.NewRow
    '            newRow.Item("email") = email
    '            newRow.Item("name") = Name
    '            newRow.Item("FileStoragePath") = folder
    '            newRow.Item("isAddFolderMember") = isShared
    '            newRow.Item("isRemoveFolderMember") = Not isShared

    '            _dtCurrentAccess.Rows.Add(newRow)
    '        End If

    '    End Sub



    '    Private Function createEmailListForEgnyteSaveList(Optional allRows As DataGridViewRowCollection = Nothing, Optional SelectedRows As DataGridViewSelectedRowCollection = Nothing) As List(Of String)
    '        Dim listOfEmails As New List(Of String)

    '        Dim rows

    '        If allRows IsNot Nothing Then
    '            rows = allRows
    '        Else
    '            rows = SelectedRows
    '        End If

    '        For Each row In rows
    '            listOfEmails.Add(row.Cells(1).Value.ToString)
    '        Next

    '        Return listOfEmails
    '    End Function

    '    Private Sub btnCreateFolderInSelectedFolder_Click(sender As Object, e As EventArgs) Handles btnCreateFolderInSelectedFolder.Click
    '        If _isLoadingTree Then
    '            Exit Sub
    '        End If

    '        Dim newFolderName As Object
    '        newFolderName = InputBox("What would you like to name the folder?", "New Folder")

    '        If newFolderName = "" Then
    '            Exit Sub
    '        End If

    '        checkForRootFolderDefinitionAndExistence()

    '        Dim newFolderPath = Path.Combine(currentGlobalOpsFolder, newFolderName)

    '        createNewFolder(newFolderPath, globalOpsTree.SelectedNode)
    '        populateListView()
    '    End Sub

    '    Private Sub deleteFileFromIndex(filePath As String)
    '        Try
    '            Using newConn As New SqlConnection(FinesseConnectionString)
    '                newConn.Open()
    '                Dim cmd As New SqlCommand($"DELETE dbo.FileAttachmentIndex
    '                                            WHERE GUID = '{currentGUID}' AND SubFolderPath = '{globalOpsTree.SelectedNode.FullPath}' AND fileName = '{Path.GetFileName(filePath)}'", newConn)

    '                cmd.ExecuteNonQuery()
    '                newConn.Close()
    '            End Using
    '        Catch ex As System.Exception

    '        End Try
    '    End Sub

    '#End Region


#Region "Dropbox"
    Private Sub UploadFileToDropbox_Click(sender As Object, e As EventArgs) Handles btnUploadToDropbox.Click
        If globalOpsListView.SelectedItems.Count = 0 Then
            MessageBox.Show("Please Select a File to upload")
            Exit Sub
        End If

        If globalOpsTree.SelectedNode.Text.Contains("- Archive") Then
            Exit Sub
        End If

        Dim nodeToUpload As TreeNode = globalOpsTree.SelectedNode
        Dim index = nodeToUpload.Index

        If _isLoadingTree Then
            Exit Sub
        End If

        Dim template = getCloudFolderTemplate(nodeToUpload)

        If isAttachmentType(nodeToUpload.Name) And _AttachmentCategory = "Projects" Then
            'folders without a template cannot be uploaded
            If template Is String.Empty Then
                MessageBox.Show("Files from this folder can not be uploaded")
                Exit Sub
            End If
        Else
            template = "PRODUCTION"
        End If

        If _AttachmentCategory = "Projects" Then
            checkIfFolderHasDBEntry(nodeToUpload)
        End If

        checkForRootFolderDefinitionAndExistence()

        Select Case _AttachmentCategory
            Case "Projects"
                For Each row As ListViewItem In globalOpsListView.SelectedItems
                    createProjcetFileUploadRequest(nodeToUpload, template, Path.Combine(currentGlobalOpsFolder, row.Text))
                Next
            Case "Parts"
                For Each row As ListViewItem In globalOpsListView.SelectedItems
                    createPartFileUploadRequest(nodeToUpload, Path.Combine(currentGlobalOpsFolder, row.Text))
                Next
        End Select

    End Sub

    Private Sub dropboxFolderToDropbox_Click(sender As Object, e As EventArgs) Handles btnDropboxFolder.Click
        If globalOpsTree.SelectedNode Is Nothing Then
            MessageBox.Show("Please Select a folder to upload")
            Exit Sub
        End If

        If _isLoadingTree Then
            Exit Sub
        End If

        Dim nodeToUpload As TreeNode = globalOpsTree.SelectedNode
        Dim index = nodeToUpload.Index
        Dim template = getCloudFolderTemplate(nodeToUpload)

        If isAttachmentType(nodeToUpload.Name) And _AttachmentCategory = "Projects" Then
            'folders on projects without a template cannot be uploaded
            If template Is String.Empty Then
                MessageBox.Show("Files from this folder can not be uploaded")
                Exit Sub
            End If
        Else
            template = "PRODUCTION"
        End If
        checkIfFolderHasDBEntry(nodeToUpload)

        checkForRootFolderDefinitionAndExistence()

        UploadAllProjectFilesInFolder(nodeToUpload, template)

        populateListView()
    End Sub

    'TEST THIS METHOD IN PROJECT MAINTENANCE
    Private Sub UploadAllProjectFilesInFolder(selectedFolder As TreeNode, template As String)

        Dim foldersToUpload As New List(Of TreeNode)

        foldersToUpload = GetChildren(selectedFolder)
        foldersToUpload.Add(selectedFolder)


        Try
            If _AttachmentCategory = "Projects" Then
                Dim rootnode() As TreeNode = globalOpsTree.Nodes.Find(template, True)
                If rootnode.Any Then
                    Dim rootFilePath = Path.Combine(currentGlobalOpsFolder, "temp")
                    EnsureSubdirectoryExists(rootFilePath, rootnode(0))
                Else
                    Throw New System.Exception("Could not locate root folder.")
                End If
            End If

            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()

                For Each folder As TreeNode In foldersToUpload

                    If folder.Text.Contains("- Archive") Then
                        Continue For
                    End If

                    Dim folderPath As String = currentGlobalOpsFolder

                    Dim AT As String
                    If Not isAttachmentType(folder.Tag) Then
                        AT = ""
                    Else
                        AT = selectedFolder.Tag
                    End If

                    For Each file In Directory.GetFiles(folderPath)
                        Select Case _AttachmentCategory
                            Case "Projects"
                                createProjcetFileUploadRequest(folder, template, file)
                            Case "Parts"
                                createPartFileUploadRequest(folder, file)
                        End Select
                    Next

                Next
                newConn.Close()
            End Using

            globalOpsTree.SelectedNode = selectedFolder

        Catch ex As System.Exception

        End Try
    End Sub

    'TEST THIS METHOD IN PROJECT MAINTENANCE
    Private Sub createPartFileUploadRequest(nodeToUpload As TreeNode, filePath As String)

        Dim AT As String
        If Not isAttachmentType(nodeToUpload.Tag) Then
            AT = ""
        Else
            AT = nodeToUpload.Tag
        End If

        Dim index = nodeToUpload.Index


        'Dim rootnode() As TreeNode = globalOpsTree.Nodes.Find(template, True)
        'If rootnode.Any Then
        '    Dim rootFilePath = Path.Combine(GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY, _currentGUID, rootnode(0).FullPath, "temp")
        '    EnsureSubdirectoryExists(rootFilePath, rootnode(0))
        'Else
        '    Throw New System.Exception("Could not locate root folder.")
        'End If

        Dim folderPath As String = currentGlobalOpsFolder

        Using newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()
            Dim query As New SqlCommand("dbo.create_dropbox_part_file_upload_request", newConn)
            query.CommandType = CommandType.StoredProcedure
            query.Parameters.Add("@filename", SqlDbType.VarChar, 255).Value = Path.GetFileName(filePath)
            query.Parameters.Add("@partno", SqlDbType.VarChar, 50).Value = currentPartNo
            query.Parameters.Add("@attachmentType", SqlDbType.VarChar, 50).Value = AT
            query.Parameters.Add("@UserFolderPath", SqlDbType.VarChar, 255).Value = nodeToUpload.FullPath
            query.ExecuteNonQuery()
            newConn.Close()
        End Using

        Dim folderRow As DataRow() = _dtUserFolders.Select("UserFolderPath = " & SQLQuote(nodeToUpload.FullPath))
        If folderRow.Any Then
            folderRow(0)("dropboxFolderID") = "Temp"
        End If

        RemoveHandler globalOpsTree.AfterSelect, AddressOf globalOpsTree_AfterSelect

        If nodeToUpload.Parent Is Nothing Then
            globalOpsTree.Nodes.Remove(nodeToUpload)
            nodeToUpload.ImageIndex = 2
            nodeToUpload.SelectedImageIndex = 2
            globalOpsTree.Nodes.Insert(index, nodeToUpload)
            globalOpsTree.SelectedNode = nodeToUpload
        Else
            Dim parent As TreeNode = nodeToUpload.Parent
            nodeToUpload.Parent.Nodes.Remove(nodeToUpload)
            nodeToUpload.ImageIndex = 2
            nodeToUpload.SelectedImageIndex = 2
            parent.Nodes.Insert(index, nodeToUpload)
            globalOpsTree.SelectedNode = nodeToUpload

        End If

        AddHandler globalOpsTree.AfterSelect, AddressOf globalOpsTree_AfterSelect

        'globalOpsTree.SelectedNode = nodeToUpload
    End Sub

    'TEST IN PROJECT MAINTENANCE
    Private Sub createProjcetFileUploadRequest(nodeToUpload As TreeNode, template As String, filePath As String)

        Dim AT As String
        If Not isAttachmentType(nodeToUpload.Tag) Then
            AT = ""
        Else
            AT = nodeToUpload.Tag
        End If

        Dim index = nodeToUpload.Index


        Dim rootnode() As TreeNode = globalOpsTree.Nodes.Find(template, True)
        If rootnode.Any Then
            Dim rootFilePath = Path.Combine(currentGlobalOpsFolder, "temp")
            EnsureSubdirectoryExists(rootFilePath, rootnode(0))
        Else
            Throw New System.Exception("Could not locate root folder.")
        End If

        Dim folderPath As String = Path.Combine(Path.Combine(currentGlobalOpsFolder))

        Using newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()
            Dim query As New SqlCommand("dbo.create_dropbox_file_upload_request", newConn)
            query.CommandType = CommandType.StoredProcedure
            query.Parameters.Add("@filename", SqlDbType.VarChar, 255).Value = Path.GetFileName(filePath)
            query.Parameters.Add("@entityno", SqlDbType.VarChar, 50).Value = currentEntityno
            query.Parameters.Add("@attachmentType", SqlDbType.VarChar, 50).Value = AT
            query.Parameters.Add("@attachmentCategory", SqlDbType.VarChar, 50).Value = _AttachmentCategory
            query.Parameters.Add("@UserFolderPath", SqlDbType.VarChar, 255).Value = nodeToUpload.FullPath
            query.ExecuteNonQuery()
            newConn.Close()
        End Using

        Dim newFileRow As DataRow = _dtUploadedFiles.NewRow()
        newFileRow("fileName") = Path.GetFileName(filePath)
        newFileRow("entityno") = currentEntityno
        newFileRow("isUploaded") = 1
        newFileRow("AttachmentType") = AT
        newFileRow("AttachmentCategory") = _AttachmentCategory
        newFileRow("UserFolderPath") = nodeToUpload.FullPath
        newFileRow("UploadTime") = DateAndTime.Now
        _dtUploadedFiles.Rows.Add(newFileRow)

        Dim folderRow As DataRow() = _dtUserFolders.Select("UserFolderPath = " & SQLQuote(nodeToUpload.FullPath))
        If folderRow.Any Then
            folderRow(0)("dropboxFolderID") = "Temp"
        End If

        If nodeToUpload.Parent Is Nothing Then
            globalOpsTree.Nodes.Remove(nodeToUpload)
            nodeToUpload.ImageIndex = 2
            nodeToUpload.SelectedImageIndex = 2
            globalOpsTree.Nodes.Insert(index, nodeToUpload)
            globalOpsTree.SelectedNode = nodeToUpload
        Else
            Dim parent As TreeNode = nodeToUpload.Parent
            nodeToUpload.Parent.Nodes.Remove(nodeToUpload)
            nodeToUpload.ImageIndex = 2
            nodeToUpload.SelectedImageIndex = 2
            parent.Nodes.Insert(index, nodeToUpload)
            globalOpsTree.SelectedNode = nodeToUpload

        End If

        'globalOpsTree.SelectedNode = nodeToUpload
    End Sub


    Private Sub btnCreateFolderInSelectedFolder_Click(sender As Object, e As EventArgs) Handles btnCreateFolderInSelectedFolder.Click
        If _isLoadingTree Then
            Exit Sub
        End If

        Dim newFolderName As Object
        newFolderName = InputBox("What would you like to name the folder?", "New Folder")

        If newFolderName = "" Then
            Exit Sub
        End If

        checkForRootFolderDefinitionAndExistence()

        Dim newFolderPath = Path.Combine(currentGlobalOpsFolder, newFolderName)

        createNewFolder(newFolderPath, globalOpsTree.SelectedNode)
        populateListView()
    End Sub

    Private Sub deleteFileFromIndex(filePath As String)
        Try
            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()
                Dim cmd As New SqlCommand($"DELETE dbo.FileAttachmentIndex
                                            WHERE GUID = '{currentGUID}' AND SubFolderPath = '{globalOpsTree.SelectedNode.FullPath}' AND fileName = '{Path.GetFileName(filePath)}'", newConn)

                cmd.ExecuteNonQuery()
                newConn.Close()
            End Using
        Catch ex As System.Exception

        End Try
    End Sub

#End Region

#Region "DropBox buttons actions"
    Private Sub buttonShareSelected_Click(sender As Object, e As EventArgs) Handles btnShareSelected.Click
        If globalOpsTree.SelectedNode Is Nothing Then
            MessageBox.Show("Please Select a folder to share")
            Exit Sub
        End If

        If dgvAvailableMembers.CurrentRow Is Nothing Then
            MessageBox.Show("Please Select a member to share with")
            Exit Sub
        End If

        If dgvAvailableMembers.CurrentRow.Cells(1).Value.ToString = "" Then
            MessageBox.Show("Please make sure this contact has a valid email address.")
            Exit Sub
        End If

        If _isLoadingTree Then
            Exit Sub
        End If

        Dim note = rtbSharingNote.Text

        checkForRootFolderDefinitionAndExistence()

        Dim row As DataGridViewRow = dgvAvailableMembers.Rows.Item(0).Clone

        row.Cells(0).Value = dgvAvailableMembers.CurrentRow.Cells(0).Value.ToString
        row.Cells(1).Value = dgvAvailableMembers.CurrentRow.Cells(1).Value.ToString

        Dim rowExists As Boolean = False
        For Each dgvrow As DataGridViewRow In dgvCurrentMembers.Rows
            If dgvrow.Cells(1).Value.ToString = dgvAvailableMembers.CurrentRow.Cells(1).Value.ToString And dgvrow.Cells(0).Value.ToString = dgvAvailableMembers.CurrentRow.Cells(0).Value.ToString Then
                rowExists = True
            End If
        Next

        Try
            checkIfFolderHasDBEntry(globalOpsTree.SelectedNode)
            Dim template = getCloudFolderTemplate(globalOpsTree.SelectedNode)

            Dim rootnode() As TreeNode = globalOpsTree.Nodes.Find(template, True)
            If rootnode.Length > 0 Then
                Dim rootFilePath = Path.Combine(currentGlobalOpsFolder, "temp")
                EnsureSubdirectoryExists(rootFilePath, rootnode(0))
            Else
                Throw New System.Exception("Could not locate root folder.")
            End If

            If template Is String.Empty Then
                MessageBox.Show("Files from this folder can not be shared")
                Exit Sub
            End If

            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()

                Dim query As New SqlCommand("dbo.create_dropbox_share_request", newConn)
                query.CommandType = CommandType.StoredProcedure
                query.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = dgvAvailableMembers.CurrentRow.Cells(1).Value.ToString
                query.Parameters.Add("@entityno", SqlDbType.VarChar, 50).Value = currentEntityno
                query.Parameters.Add("@cloudFolderTemplate", SqlDbType.VarChar, 50).Value = template
                query.Parameters.Add("@attachmentCategory", SqlDbType.VarChar, 50).Value = _AttachmentCategory
                query.Parameters.Add("@UserFolderPath", SqlDbType.VarChar, 255).Value = globalOpsTree.SelectedNode.FullPath
                query.Parameters.Add("@note", SqlDbType.VarChar, -1).Value = note


                '  @email = " & SQLQuote(dgvAvailableMembers.CurrentRow.Cells(1).Value.ToString) & "				-- varchar(50)
                ', @entityno = " & SQLQuote(currentEntityno) & "			-- varchar(50)
                ', @cloudFolderTemplate = " & SQLQuote(template) & " -- varchar(50)
                ', @attachmentCategory = " & SQLQuote(AttachmentCategory) & "	-- varchar(50)
                ', @UserFolderPath = " & SQLQuote(globalOpsTree.SelectedNode.FullPath) & "		-- varchar(255)
                ', @note = " & SQLQuote(note) & "				-- varchar(max)", newConn)
                query.ExecuteNonQuery()

                Dim rows As DataRow() = _dtProjectShareRequests.Select("Email = " & SQLQuote(dgvAvailableMembers.CurrentRow.Cells(1).Value.ToString) & " AND CloudFolderTemplate = " & SQLQuote(template))

                If rows.Length = 0 Then
                    Dim newShareRow As DataRow = _dtProjectShareRequests.NewRow()
                    newShareRow("Name") = row.Cells(0).Value.ToString
                    newShareRow("email") = dgvAvailableMembers.CurrentRow.Cells(1).Value.ToString
                    newShareRow("CloudFolderTemplate") = template
                    newShareRow("isAddFolderMember") = 1
                    newShareRow("isRemoveFolderMember") = 0
                    _dtProjectShareRequests.Rows.Add(newShareRow)
                Else
                    rows(0).Item("isAddFolderMember") = 1
                    rows(0).Item("isRemoveFolderMember") = 0
                End If

                newConn.Close()
            End Using
        Catch ex As System.Exception

        End Try

    End Sub

    'TEST IN PROJECT MAINTENANCE
    Private Sub buttonShareAll_Click(sender As Object, e As EventArgs) Handles btnShareAll.Click
        If globalOpsTree.SelectedNode Is Nothing Then
            MessageBox.Show("Please Select a folder to Share")
            Exit Sub
        End If

        If _isLoadingTree Then
            Exit Sub
        End If


        Dim template = getCloudFolderTemplate(globalOpsTree.SelectedNode)

        If template Is String.Empty Then
            MessageBox.Show("Files from this folder can not be shared")
            Exit Sub
        End If

        Dim note = rtbSharingNote.Text

        checkForRootFolderDefinitionAndExistence()
        checkIfFolderHasDBEntry(globalOpsTree.SelectedNode)

        Dim EmailList As New List(Of String)
        Using newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()
            For Each row As DataGridViewRow In dgvAvailableMembers.Rows
                Try

                    Dim rootnode() As TreeNode = globalOpsTree.Nodes.Find(template, True)
                    If rootnode.Length > 0 Then
                        Dim rootFilePath = Path.Combine(currentGlobalOpsFolder, "temp")
                        EnsureSubdirectoryExists(rootFilePath, rootnode(0))
                    Else
                        Throw New System.Exception("Could not locate root folder.")
                    End If

                    If Not row.Cells(1).Value.ToString = "" Then
                        Dim query As New SqlCommand("dbo.create_dropbox_share_request", newConn)
                        query.CommandType = CommandType.StoredProcedure
                        query.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = row.Cells(1).Value.ToString
                        query.Parameters.Add("@entityno", SqlDbType.VarChar, 50).Value = currentEntityno
                        query.Parameters.Add("@cloudFolderTemplate", SqlDbType.VarChar, 50).Value = template
                        query.Parameters.Add("@attachmentCategory", SqlDbType.VarChar, 50).Value = _AttachmentCategory
                        query.Parameters.Add("@UserFolderPath", SqlDbType.VarChar, 255).Value = globalOpsTree.SelectedNode.FullPath
                        query.Parameters.Add("@note", SqlDbType.VarChar, -1).Value = note
                        query.ExecuteNonQuery()

                        Dim rows As DataRow() = _dtProjectShareRequests.Select("Email = " & SQLQuote(row.Cells(1).Value.ToString) & " AND CloudFolderTemplate = " & SQLQuote(template))

                        If rows.Length = 0 Then
                            Dim newShareRow As DataRow = _dtProjectShareRequests.NewRow()
                            newShareRow("Name") = row.Cells(0).Value.ToString
                            newShareRow("email") = row.Cells(1).Value.ToString
                            newShareRow("CloudFolderTemplate") = template
                            newShareRow("isAddFolderMember") = 1
                            newShareRow("isRemoveFolderMember") = 0
                            _dtProjectShareRequests.Rows.Add(newShareRow)
                        Else
                            rows(0).Item("isAddFolderMember") = 1
                            rows(0).Item("isRemoveFolderMember") = 0
                        End If

                    End If
                Catch ex As System.Exception

                End Try
            Next
            newConn.Close()
        End Using
    End Sub

    Private Sub buttonRemoveSelected_Click(sender As Object, e As EventArgs) Handles btnRemoveSelected.Click
        If globalOpsTree.SelectedNode Is Nothing Then
            MessageBox.Show("Please Select a folder to revoke")
            Exit Sub
        End If

        If dgvCurrentMembers.CurrentRow Is Nothing Then
            MessageBox.Show("Please Select a member to revoke access from")
            Exit Sub
        End If

        If _isLoadingTree Then
            Exit Sub
        End If

        checkIfFolderHasDBEntry(globalOpsTree.SelectedNode)
        Dim template = getCloudFolderTemplate(globalOpsTree.SelectedNode)
        Dim email = dgvCurrentMembers.SelectedRows(0).Cells(1).Value.ToString

        Try
            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()
                Dim query As New SqlCommand("UPDATE dbo.CloudFileStorageShareRequests
                                             SET isExecuted = 0, isAddFolderMember = 0, isRemoveFolderMember = 1
                                             WHERE email = " & SQLQuote(email) & " AND entityno = " & SQLQuote(currentEntityno) & " AND CloudFolderTemplate = " & SQLQuote(template), newConn)
                query.ExecuteNonQuery()
                Dim shareRow As DataRow() = _dtProjectShareRequests.Select("email = " & SQLQuote(dgvCurrentMembers.SelectedRows(0).Cells(1).Value.ToString) & " AND CloudFolderTemplate = " & SQLQuote(template))
                If shareRow.Length > 0 Then
                    shareRow(0)("isAddFolderMember") = 0
                    shareRow(0)("isRemoveFolderMember") = 1
                End If
                newConn.Close()
            End Using
        Catch ex As System.Exception

        End Try
    End Sub

    Private Sub buttonRemoveAll_Click(sender As Object, e As EventArgs) Handles btnRemoveAll.Click
        If globalOpsTree.SelectedNode Is Nothing Then
            MessageBox.Show("Please Select a folder")
            Exit Sub
        End If

        If _isLoadingTree Then
            Exit Sub
        End If

        checkIfFolderHasDBEntry(globalOpsTree.SelectedNode)
        Dim template = getCloudFolderTemplate(globalOpsTree.SelectedNode)
        Try
            For Each row As DataGridViewRow In dgvAvailableMembers.Rows
                Using newConn As New SqlConnection(FinesseConnectionString)
                    newConn.Open()
                    Dim query As New SqlCommand("UPDATE dbo.CloudFileStorageShareRequests
                                             SET isExecuted = 0, isAddFolderMember = 0, isRemoveFolderMember = 1
                                             WHERE email = " & SQLQuote(row.Cells(1).Value.ToString) & " AND entityno = " & SQLQuote(currentEntityno) & " AND CloudFolderTemplate = " & SQLQuote(template), newConn)
                    query.ExecuteNonQuery()
                    newConn.Close()
                End Using
                Dim shareRow As DataRow() = _dtProjectShareRequests.Select("email = " & SQLQuote(row.Cells(1).Value.ToString) & " AND CloudFolderTemplate = " & SQLQuote(template))
                If shareRow.Length > 0 Then
                    shareRow(0)("isAddFolderMember") = 0
                    shareRow(0)("isRemoveFolderMember") = 1
                End If
            Next
        Catch ex As System.Exception

        End Try
    End Sub

    Private Sub addOrUpdateFileIndex(filePath As String)
        Try
            Dim fs As FileSecurity = System.IO.File.GetAccessControl(filePath)
            Dim sid As IdentityReference = fs.GetOwner(GetType(SecurityIdentifier))
            Dim ntaccount As IdentityReference = sid.Translate(GetType(NTAccount))
            Dim owner As String = ntaccount.ToString()

            Dim dirInfo As DirectoryInfo = New DirectoryInfo(filePath)

            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()
                Dim cmd As New SqlCommand("dbo.add_File_to_File_Explorer_Search_Index", newConn)
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@fileName", SqlDbType.VarChar, 255).Value = Path.GetFileName(filePath)
                cmd.Parameters.Add("@fileExtension", SqlDbType.VarChar, 255).Value = ""
                cmd.Parameters.Add("@CreationTime", SqlDbType.DateTime2).Value = dirInfo.CreationTimeUtc
                cmd.Parameters.Add("@GUID", SqlDbType.VarChar, 255).Value = currentGUID
                cmd.Parameters.Add("@subFolderPath", SqlDbType.VarChar, 255).Value = globalOpsTree.SelectedNode.FullPath
                cmd.Parameters.Add("@CreatedBy", SqlDbType.VarChar, 255).Value = owner
                cmd.ExecuteNonQuery()
                'newConn.Close()
            End Using
            newConn.Close()
        Catch ex As System.Exception

        End Try
    End Sub

    Private Sub deleteFolderFromIndex(subFolderPath As String)
        Try
            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()
                Dim cmd As New SqlCommand($"DELETE dbo.FileAttachmentIndex
                                            WHERE GUID = '{currentGUID}' AND SubFolderPath = '{subFolderPath}'", newConn)

                cmd.ExecuteNonQuery()
                newConn.Close()
            End Using
        Catch ex As System.Exception

        End Try
    End Sub

#End Region

#Region "Dropbox/other various Folder functions"

    Private Sub RenameFolder(newFolderName As String, newSubFolderPath As String, OldSubFolderPath As String)

        My.Computer.FileSystem.RenameDirectory(currentGlobalOpsFolder, newFolderName)
        'Dim oldSubFolderPath As String = globalOpsTree.SelectedNode.FullPath

        If DropboxEnabled And _AttachmentCategory = "Projects" Then
            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()
                Dim query As New SqlCommand($"UPDATE dbo.ProjectsUsersFoldersToCloudStorageFolders
                               SET UserFolderPath = '{newSubFolderPath}'
                               WHERE UserFolderPath = '{OldSubFolderPath}' AND entityno = '{currentEntityno}'", newConn)
                query.ExecuteNonQuery()
                newConn.Close()
            End Using
        End If

        checkForRootFolderDefinitionAndExistence()

        currentGlobalOpsFolder = Path.Combine(_attachmentCategorySubFolderPath, globalOpsTree.SelectedNode.FullPath)
    End Sub



    Private Function getCloudFolderTemplate(ByVal Folder As TreeNode) As String
        Dim template As String = String.Empty

        'still need this for when entering template into folder DB entry
        Dim selectedRows As DataRow() = _dtAttachmentTypesCloudTemplates.Select("AttachmentType = " & SQLQuote(Folder.Tag))

        If selectedRows.Length = 0 Then
            selectedRows = _dtAttachmentTypesCloudTemplates.Select("AttachmentType = " & SQLQuote(getRootFolder(globalOpsTree.SelectedNode).Text))
        End If

        If selectedRows.Length > 0 Then
            template = selectedRows(0).Item("CloudFolderTemplate")
        Else
            template = "PRODUCTION"
        End If

        Return template
    End Function

    Private Function getRootFolder(node As TreeNode) As TreeNode
        Dim root As TreeNode = node

        While (root.Parent IsNot Nothing)
            root = root.Parent
        End While

        Return root
    End Function


    Private Function getDropboxFolderID(ByVal folder As TreeNode) As String
        Dim dropboxId As String = String.Empty
        Dim selectedRows As DataRow()

        Dim test = SQLQuote(convertToWindowsPath(folder.FullPath))
        selectedRows = _dtUserFolders.Select("UserFolderPath = " & SQLQuote(convertToWindowsPath(folder.FullPath)) & " OR " & "UserFolderPath = " & SQLQuote(convertToDropboxPath(folder.FullPath)))

        If selectedRows.Length > 0 Then
            If selectedRows(0).Item("dropboxFolderID") IsNot DBNull.Value Then
                dropboxId = selectedRows(0).Item("dropboxFolderID")
            End If
        End If

        Return dropboxId
    End Function

    Private Function isAttachmentType(ByVal Folder As String) As Boolean
        Dim isAttachment As Boolean = False

        Dim ATtest = _dtAttachmentTypes.Select("AttachmentTypeDescription = " & SQLQuote(Folder))

        Dim AT = _dtAttachmentTypes.Select("AttachmentType = " & SQLQuote(Folder))

        If ATtest.Length > 0 Or AT.Length > 0 Then
            isAttachment = True
        End If

        Return isAttachment
    End Function

    Private Function getAttachmentTypeByDescription(ByVal attachmentTypeDescription As String) As DataRow
        Dim AttachmentTypeRow As DataRow = Nothing

        Dim row As DataRow() = _dtAttachmentTypes.Select("AttachmentTypeDescription = " & SQLQuote(attachmentTypeDescription))

        If row.Any Then
            AttachmentTypeRow = row(0)
        End If

        Return AttachmentTypeRow
    End Function

    Private Function doesUserHaveAccessToThisAttachmentType(attachmentTypeDescription As String) As Boolean
        Dim row As DataRow() = _dtAttachmentTypes.Select($"AttachmentTypeDescription = {SQLQuote(attachmentTypeDescription)} And hasPermissions = 1")

        If row.Any Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function doesAttachmentTypeHavePermissions(attachmentTypeDescription As String) As Boolean
        Dim row As DataRow() = _dtAttachmentTypes.Select($"AttachmentTypeDescription = {SQLQuote(attachmentTypeDescription)} And Permissionsneeded IS NOT NULL")

        If row.Any Then
            Return True
        Else
            Return False
        End If
    End Function


    Private Function getAttachmentTypeByCode(ByVal attachmentType As String) As DataRow
        Dim AttachmentTypeRow As DataRow = Nothing

        Dim row As DataRow() = _dtAttachmentTypes.Select("AttachmentType = " & SQLQuote(attachmentType))

        If row.Length > 0 Then
            AttachmentTypeRow = row(0)
        End If

        Return AttachmentTypeRow
    End Function

    Function GetChildren(parentNode As TreeNode) As List(Of TreeNode)
        Dim nodes As List(Of TreeNode) = New List(Of TreeNode)
        GetAllChildren(parentNode, nodes)
        Return nodes
    End Function

    Sub GetAllChildren(parentNode As TreeNode, nodes As List(Of TreeNode))
        For Each childNode As TreeNode In parentNode.Nodes
            nodes.Add(childNode)
            GetAllChildren(childNode, nodes)
        Next
    End Sub

    Private Sub createNewFolder(ByVal newFolderPath As String, Optional ByVal parentFolder As TreeNode = Nothing)

        System.IO.Directory.CreateDirectory(newFolderPath)

        Dim newFolderName = Path.GetFileName(newFolderPath)
        Dim template As String = String.Empty

        Dim newNode = New TreeNode(newFolderName)

        If parentFolder Is Nothing Then
            globalOpsTree.Nodes.Add(newNode)
            template = "PRODUCTION"
        Else
            parentFolder.Nodes.Add(newNode)
            newNode.Name = newNode.FullPath
            If DropboxEnabled And _AttachmentCategory = "Projects" Then
                template = getCloudFolderTemplate(parentFolder)
            End If
        End If

        globalOpsTree.SelectedNode = newNode

        'If Not DropboxEnabled And _AttachmentCategory = "Projects" Then Return
        If Not DropboxEnabled Then Return
        Dim idLevel = getIDLevelFromTemplateType(template)

        Try
            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()
                Dim query As New SqlCommand("INSERT dbo.ProjectsUsersFoldersToCloudStorageFolders
                        (
                            entityno,
                            UserFolderPath,
                            CloudFolderTemplate,
                            id_Level
                        )
                        VALUES
                        (
                            " & SQLQuote(currentEntityno) & ", -- entityno - uniqueidentifier
                            " & SQLQuote(newNode.FullPath) & ",   -- UserFolderPath - varchar(100)
                             " & SQLQuote(template) & ",
                            " & idLevel & "

                        )", newConn)

                query.ExecuteNonQuery()
                newConn.Close()
            End Using

            Dim newFolderRow As DataRow = _dtUserFolders.NewRow()
            newFolderRow("entityno") = currentEntityno
            newFolderRow("UserFolderPath") = newNode.FullPath
            newFolderRow("dropboxFolderID") = DBNull.Value
            newFolderRow("CloudFolderTemplate") = template
            newFolderRow("id_Level") = idLevel
            _dtUserFolders.Rows.Add(newFolderRow)

        Catch ex As System.Exception
            'MessageBox.Show("Error while creating folder entry in the Database: " & ex.Message)
        End Try
    End Sub

    Private Function FixFileName(ByVal pFileName As String) As String
        Dim invalidChars As Char() = Path.GetInvalidFileNameChars()
        If (pFileName.IndexOfAny(invalidChars) >= 0) Then
            pFileName = invalidChars.Aggregate(pFileName, Function(current, invalidChar) current.Replace(invalidChar, Convert.ToChar("_")))
        End If
        Return pFileName
    End Function

    Private Sub GetAttachmentsInfo(ByVal pMailItem As MailItem)
        If Not IsNothing(pMailItem.Attachments) Then
            For i As Integer = 1 To pMailItem.Attachments.Count
                Dim currentAttachment As Attachment = pMailItem.Attachments.Item(i)
                If Not IsNothing(currentAttachment) Then
                    Dim strFile As String = IO.Path.Combine(currentGlobalOpsFolder, FixFileName(currentAttachment.FileName))
                    currentAttachment.SaveAsFile(strFile)
                    addFileToListView(strFile)
                End If
            Next
        End If
    End Sub

    Private Sub EnableDoubleBufferingTreeView(ByVal control As System.Windows.Forms.Control)
        GetType(TreeView).InvokeMember(
                    "DoubleBuffered",
                    Reflection.BindingFlags.NonPublic Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.SetProperty,
                    Nothing,
                    control,
                    New Object() {True}
                    )
    End Sub

    Private Sub DeleteFolderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles treeViewCMSDeleteFolder.Click
        If globalOpsTree.SelectedNode Is Nothing Then
            Exit Sub
        End If

        If _currentGUID = String.Empty Or _currentGUID Is Nothing Or _currentGUID Is DBNull.Value Then
            globalOpsTree.Nodes.Remove(globalOpsTree.SelectedNode)
            Exit Sub
        End If

        deleteFolder()
    End Sub

    Private Sub deleteFolder()

        Dim result = MessageBox.Show("Are you sure you would like to delete this folder and its contents?", "Deleting Folder...", MessageBoxButtons.YesNo)
        If result = DialogResult.No Then
            Exit Sub
        ElseIf result = DialogResult.Yes Then
            Try
                System.IO.Directory.Delete(currentGlobalOpsFolder, True)
                deleteFolderFromIndex(globalOpsTree.SelectedNode.FullPath)
            Catch ex As System.Exception
                MsgBox("There was an unexpected error. " & vbCrLf & ex.Message)
            End Try
        End If

        globalOpsTree.Nodes.Remove(globalOpsTree.SelectedNode)
    End Sub

    Private Function getIDLevelFromTemplateType(ByVal Template As String) As Integer
        Dim templateIDLevel = _dtCloudFolderTemplatesIDLevel.Select("CloudFolderTemplate = " & SQLQuote(Template))
        Dim idLevel As Integer = 100

        For Each row As DataRow In templateIDLevel
            If idLevel > row.Item("id_Level") Then
                idLevel = row.Item("id_Level")
            End If
        Next
        Return idLevel
    End Function

    Private Sub createDeskTopShortCut()
        Try
            Dim objShell, strDesktopPath, objLink
            objShell = CreateObject("WScript.Shell")
            strDesktopPath = objShell.SpecialFolders("Desktop")

            Dim finesseFavoritesFolderPath As String = Path.Combine(strDesktopPath, FINEESSE_FILE_STORAGE_FAVORITES_FOLDER_NAME)

            If Not System.IO.Directory.Exists(finesseFavoritesFolderPath) Then
                System.IO.Directory.CreateDirectory(finesseFavoritesFolderPath)
            End If

            objLink = objShell.CreateShortcut(finesseFavoritesFolderPath & "\" & _projectDesc & ".lnk")
            objLink.TargetPath = Path.Combine(attachmentCategorySubFolderPath)
            objLink.Save
        Catch ex As System.Exception

        End Try

    End Sub

    Private Function doDisplayedFilesMatchActualFolder() As Boolean
        Dim filesInFolder As New List(Of String)
        Dim filesInListView As New List(Of String)

        If Directory.Exists(currentGlobalOpsFolder) Then
            For Each file In Directory.GetFiles(currentGlobalOpsFolder).ToList
                filesInFolder.Add(Path.GetFileName(file))
            Next
        End If

        For Each item As ListViewItem In globalOpsListView.Items
            filesInListView.Add(item.Text)
        Next

        'check how many differences there are.
        Dim filesOnlyInFolder = filesInFolder.Except(filesInListView).ToArray
        Dim filesOnlyInListView = filesInListView.Except(filesInFolder).ToArray


        'if they match the 
        If filesOnlyInFolder.Any Or filesOnlyInListView.Any Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Sub checkIfFolderHasDBEntry(ByVal folder As TreeNode, Optional ByVal parentFolder As TreeNode = Nothing)

        Dim folderSearch As DataRow() = _dtUserFolders.Select("entityno = " & SQLQuote(currentEntityno) & " AND UserFolderPath = " & SQLQuote(folder.FullPath))

        If folderSearch.Length > 0 Then
            Exit Sub
        End If

        Dim FolderName = Path.GetFileName(folder.Text)
        Dim template As String = String.Empty

        template = getCloudFolderTemplate(folder)

        'if there is no template check the root nodes
        If template Is String.Empty Then
            Dim root As TreeNode = getRootFolder(folder)
            template = getCloudFolderTemplate(root)


            'if the root node is not an attachment type and we still don't have a tmeplate make it production. If it is an attachemnt type without a tmeplate we want to enter nothing
            If Not isAttachmentType(root.FullPath) And template Is String.Empty Then
                template = "PRODUCTION"
            End If

        End If

        ' template = _dtAttachmentTypesCloudTemplates.Select("AttachmentType = " & SQLQuote(folder.Tag))(0).Item("CloudFolderTemplate")

        Dim idLevel = getIDLevelFromTemplateType(template)

        Try
            Using newConn As New SqlConnection(FinesseConnectionString)
                newConn.Open()
                Dim query As New SqlCommand("INSERT dbo.ProjectsUsersFoldersToCloudStorageFolders
                        (
                            entityno,
                            UserFolderPath,
                            CloudFolderTemplate,
                            id_Level,
                            AttachmentType
                        )
                        VALUES
                        (
                            " & SQLQuote(currentEntityno) & ", -- entityno - uniqueidentifier
                            " & SQLQuote(folder.FullPath) & ",   -- UserFolderPath - varchar(100)
                             " & SQLQuote(template) & ",
                            " & idLevel & ",
                             " & SQLQuote(folder.Tag) & "
                        )", newConn)

                query.ExecuteNonQuery()
                newConn.Close()
            End Using

            Dim newFolderRow As DataRow = _dtUserFolders.NewRow()
            newFolderRow("entityno") = currentEntityno
            newFolderRow("UserFolderPath") = globalOpsTree.SelectedNode.FullPath
            newFolderRow("dropboxFolderID") = DBNull.Value
            newFolderRow("CloudFolderTemplate") = template
            newFolderRow("AttachmentType") = folder.Tag
            newFolderRow("id_Level") = idLevel
            _dtUserFolders.Rows.Add(newFolderRow)

        Catch ex As System.Exception

        End Try
    End Sub

    Public Sub createAttachmentTypePath(ByVal dirInfo As DirectoryInfo, ByVal AttachmentType As String)
        Dim datarow As DataRow = getAttachmentTypeByCode(AttachmentType)
        createSubDirectories(dirInfo, datarow)
    End Sub



    Private Sub checkForRootFolderDefinitionAndExistence()
        If _currentGUID Is String.Empty Or _currentGUID Is Nothing Or _currentGUID Is DBNull.Value Then
            RaiseEvent noGUID() 'cant rename event becuase then it would break parent forms
            If globalOpsTree.SelectedNode Is Nothing Then
                currentGlobalOpsFolder = getRootDefaultFolderPath()
            Else
                currentGlobalOpsFolder = Path.Combine(getRootDefaultFolderPath(), globalOpsTree.SelectedNode.FullPath)
            End If

            If _attachmentCategorySubFolderPath = GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY Then
                _attachmentCategorySubFolderPath = getRootDefaultFolderPath()
            End If
        End If

        If Not Directory.Exists(_attachmentCategorySubFolderPath) Then
            RaiseEvent rootFolderBeingCreated()
            createFilestoragePathsEntry()
        End If
    End Sub

    Private Sub createFilestoragePathsEntry()
        Dim conn As SqlConnection = GetOpenedFinesseConnection()

        Dim query As New SqlCommand("dbo.createFileStoragePathEntry", conn)
        query.CommandType = CommandType.StoredProcedure
        query.Parameters.Add("@folderPath", SqlDbType.VarChar, 255).Value = _attachmentCategorySubFolderPath
        query.Parameters.Add("@GUID", SqlDbType.UniqueIdentifier).Value = Guid.Parse(_currentGUID)
        query.ExecuteNonQuery()

        conn.Close()
        conn.Dispose()
    End Sub

    Private Function convertToWindowsPath(ByVal dropboxPath) As String
        Dim windowsPath As String = String.Empty
        windowsPath = dropboxPath.Replace("/", "\")
        Return windowsPath
    End Function

    Private Function convertToDropboxPath(ByVal windowsPath) As String
        Dim dropboxPath As String = String.Empty
        dropboxPath = windowsPath.Replace("\", "/")
        Return dropboxPath
    End Function

    Private Sub refreshFileSystemWatcher()
        If _fsw Is Nothing OrElse _fsw.Path Is Nothing Then
            Exit Sub
        End If

        Dim dir As String = _fsw.Path

        If Directory.Exists(dir) Then
            _fsw.Dispose()
            _fsw = Nothing
            _fsw = New FileSystemWatcher(dir)
            _fsw.IncludeSubdirectories = True
            _fsw.EnableRaisingEvents = True
            Debug.WriteLine("Refreshed 1st FileSystemWatcher")
        End If

    End Sub

    Private Function canAttachmentTypeHaveExpirationDates(attachmentType As String) As Boolean

        Dim attachmentTypeRow As DataRow() = _dtAttachmentTypes.Select($"AttachmentTypeDescription = {attachmentType.SQLQuote}")

        If attachmentTypeRow.Any Then

            If attachmentTypeRow(0).Item("CanHaveExpirationDate") Then
                Return True
            End If

        End If

        Return False
    End Function


#End Region


End Class

Public Class FileCompare
    Implements System.Collections.Generic.IEqualityComparer(Of System.IO.FileInfo)

    Public Function Equals1(ByVal x As System.IO.FileInfo, ByVal y As System.IO.FileInfo) _
            As Boolean Implements System.Collections.Generic.IEqualityComparer(Of System.IO.FileInfo).Equals

        If (x.Name = y.Name) And (x.Length = y.Length) Then
            Return True
        Else
            Return False
        End If
    End Function

    ' Return a hash that reflects the comparison criteria. According to the   
    ' rules for IEqualityComparer(Of T), if Equals is true, then the hash codes must  
    ' also be equal. Because equality as defined here is a simple value equality, not  
    ' reference identity, it is possible that two or more objects will produce the same  
    ' hash code.  
    Public Function GetHashCode1(ByVal fi As System.IO.FileInfo) _
            As Integer Implements System.Collections.Generic.IEqualityComparer(Of System.IO.FileInfo).GetHashCode
        Dim s As String = fi.Name & fi.Length
        Return s.GetHashCode()
    End Function
End Class

Friend Class ListViewItemDateComparer
    Implements IComparer
    Private col As Integer
    Public _sort As SortOrder = SortOrder.Ascending

    Public Sub New(column As Integer, sort As Windows.Forms.SortOrder)
        col = column
        _sort = sort
    End Sub

    Public Function Compare(x As Object,
                 y As Object) As Integer Implements System.Collections.IComparer.Compare
        Dim returnVal As Integer = -1

        ' parse LV contents back to DateTime value
        Dim dtX As DateTime = DateTime.Parse(CType(x, ListViewItem).SubItems(col).Text)
        Dim dtY As DateTime = DateTime.Parse(CType(y, ListViewItem).SubItems(col).Text)

        ' compare
        returnVal = DateTime.Compare(dtX, dtY)

        If _sort = SortOrder.Descending Then
            returnVal *= -1
        End If
        Return returnVal

    End Function
End Class

Public Class NodeSorter
    Implements IComparer

    Private ReadOnly attachmenttypes As DataTable

    Public Sub New(ByVal dt As DataTable)
        Me.attachmenttypes = dt
    End Sub

    Public Function Compare(ByVal x As Object, ByVal y As Object) _
        As Integer Implements IComparer.Compare
        Dim tx As TreeNode = CType(x, TreeNode)
        Dim ty As TreeNode = CType(y, TreeNode)

        If tx Is Nothing AndAlso ty Is Nothing Then
            Return 0
        ElseIf tx Is Nothing Then
            Return 1
        ElseIf ty Is Nothing Then
            Return -1
        End If

        If (attachmenttypes IsNot Nothing) Then
            Dim orderX As Integer = GetOrderFromDataTable(tx.Tag)
            Dim orderY As Integer = GetOrderFromDataTable(ty.Tag)

            If orderX <> Integer.MaxValue OrElse orderY <> Integer.MaxValue Then
                If orderX <> orderY Then
                    Return orderX - orderY
                End If
            End If
        End If

        Return String.Compare(tx.Text, ty.Text, StringComparison.CurrentCulture)
    End Function

    Private Function GetOrderFromDataTable(ByVal attachmentType As String) As Integer
        Try
            If attachmentType IsNot Nothing Then
                Dim rows = attachmenttypes.Select($"AttachmentType = '{attachmentType}'")
                If rows.Length > 0 AndAlso rows(0)("TreeOrder") IsNot DBNull.Value Then
                    Return Convert.ToInt32(rows(0)("TreeOrder"))
                End If
            End If
            Return Integer.MaxValue ' If no match is found, move to the end.
        Catch ex As System.Exception
            Return Integer.MaxValue
        End Try
    End Function
End Class