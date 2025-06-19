Imports System.Data.SqlClient
Imports System.IO
Imports System.Security.AccessControl
Imports System.Security.Principal
Imports System.Text
Imports Microsoft.Office.Interop.Outlook

Module FinesseFileExplorerUtilities

    Public Function getFinesseDataRootDirectory() As String

        Dim GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY As String = String.Empty

        Using newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()
            Dim sqlStr As StringBuilder = New StringBuilder
            sqlStr.Append("select s.value from sysconfig s where s.tag = 'GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY'")
            GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY = newConn.GetDataTable(sqlStr).Rows(0).Item("Value")
            newConn.Close()
        End Using

        Return GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY

    End Function

    'This should not be used anymore
    'Public Function getRolledUpProjectAttachmentFilePath(ByVal entityno As String, ByVal attachmentTypeCode As String) As String

    '    Dim attachmentCategory = "Projects"
    '    Dim folderPath As String = String.Empty
    '    Dim GUID As String = String.Empty

    '    Dim attachmentTypeRow As DataRow = Nothing

    '    Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))

    '    jammer.Add("EXEC dbo.GetProjectRolledUpFileStorageInfo @entityno = " & SQLQuote(entityno),
    '               Sub(t)
    '                   If t.Rows.Count > 0 Then
    '                       GUID = t.Rows(0).Item("GUID").ToString
    '                   End If
    '               End Sub)
    '    jammer.Add("SELECT act.AttachmentCategory, at.AttachmentTypeDescription, Permissionsneeded = is_rolemember(DatabaseRole), act.AttachmentType, cfsgtat.CloudFolderTemplate, 
    '		 CASE ISNULL (is_rolemember(DatabaseRole), 1)
    '		 WHEN 0 THEN 0
    '		 ELSE 1
    '		 END AS hasPermissions
    '                     FROM dbo.AttachmentCategoryTypes act
    '                     JOIN dbo.AttachmentTypes at ON act.AttachmentType = at.AttachmentType
    '		 left outer join dbo.CloudFileStorageGroupsToAttachmentTypes cfsgtat on cfsgtat.AttachmentType = at.AttachmentType
    '		 LEFT OUTER JOIN dbo.AttachmentTypeDatabaseRoles atb ON atb.AttachmentType = at.AttachmentType
    '                     WHERE act.AttachmentCategory = " & SQLQuote(attachmentCategory) & " and act.AttachmentType = " & SQLQuote(attachmentTypeCode),
    '               Sub(t)
    '                   If t.Rows.Count > 0 Then
    '                       attachmentTypeRow = t.Rows(0)
    '                   End If
    '               End Sub)
    '    jammer.Execute()

    '    folderPath = Path.Combine(GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY, GUID)

    '    folderPath = Path.Combine(folderPath, attachmentTypeRow.Item("AttachmentTypeDescription"))

    '    If Not Directory.Exists(folderPath) Then
    '        createFolderDBEntry(attachmentTypeRow, entityno)
    '        Dim directoryInfo As DirectoryInfo = New DirectoryInfo(folderPath)
    '        createSubDirectories(directoryInfo, attachmentTypeRow, attachmentCategory)
    '    End If

    '    Return folderPath
    'End Function

    Public Function getProjectAttachmentFilePath(ByVal entityno As String, ByVal attachmentTypeCode As String) As String

        Dim attachmentCategory = "Projects"
        Dim folderPath As String = String.Empty
        Dim GUID As String = String.Empty

        Dim attachmentTypeRow As DataRow = Nothing

        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))

        jammer.Add("select g.GUID
                    from dbo.glentities g
                    where entityno = " & SQLQuote(entityno),
                   Sub(t)
                       If t.Rows.Count > 0 Then
                           GUID = t.Rows(0).Item("GUID").ToString
                       End If
                   End Sub)
        jammer.Add("SELECT act.AttachmentCategory, at.AttachmentTypeDescription, Permissionsneeded = is_rolemember(DatabaseRole), act.AttachmentType, cfsgtat.CloudFolderTemplate, 
						 CASE ISNULL (is_rolemember(DatabaseRole), 1)
						 WHEN 0 THEN 0
						 ELSE 1
						 END AS hasPermissions, dbo.fn_get_attachmentTypeFullPath(at.AttachmentType) as FullAttachmentPath
                         FROM dbo.AttachmentCategoryTypes act
                         JOIN dbo.AttachmentTypes at ON act.AttachmentType = at.AttachmentType
						 left outer join dbo.CloudFileStorageGroupsToAttachmentTypes cfsgtat on cfsgtat.AttachmentType = at.AttachmentType
						 LEFT OUTER JOIN dbo.AttachmentTypeDatabaseRoles atb ON atb.AttachmentType = at.AttachmentType
                         WHERE act.AttachmentCategory = " & SQLQuote(attachmentCategory) & " and act.AttachmentType = " & SQLQuote(attachmentTypeCode),
                   Sub(t)
                       If t.Rows.Count > 0 Then
                           attachmentTypeRow = t.Rows(0)
                       End If
                   End Sub)
        jammer.Execute()

        folderPath = getAttachmentTypeFolderPath(GUID, attachmentCategory)
        Dim rootFolderPath As String = folderPath
        folderPath = Path.Combine(folderPath, attachmentTypeRow.Item("FullAttachmentPath"))

        If Not Directory.Exists(folderPath) Then
            createFilestoragePathsEntry(rootFolderPath, GUID)
            createFolderDBEntry(attachmentTypeRow, entityno)
            Dim directoryInfo As DirectoryInfo = New DirectoryInfo(folderPath)
            createSubDirectories(directoryInfo, attachmentTypeRow, attachmentCategory)
        End If

        Return folderPath
    End Function


    Private Sub createSubDirectories(ByVal info As DirectoryInfo, ByRef Row As DataRow, ByVal attachmentCategory As String)
        Dim _dtAttachmentPermissions As DataTable = Nothing

        Using newConn As New SqlConnection(FinesseConnectionString)
            newConn.Open()
            Dim sqlStr As StringBuilder = New StringBuilder
            sqlStr.Append("SELECT gp.groupName, gp.AttachmentType, gp.allowFullControl, gp.allowModify, gp.[allowRead&Execute], gp.allowListFolderContents, gp.allowRead, gp.allowWrite, gp.allowSpecialPermissions, fpg.GroupPath
                         FROM dbo.GroupPermissions gp
                         JOIN dbo.FilePermissionsGroups fpg ON fpg.GroupName = gp.GroupName
                         JOIN dbo.AttachmentCategoryTypes act ON act.AttachmentType = gp.AttachmentType
                         WHERE act.AttachmentCategory = " & SQLQuote(attachmentCategory) & " AND act.attachmentType = " & SQLQuote(Row.Item("attachmentTYpe")))
            _dtAttachmentPermissions = newConn.GetDataTable(sqlStr)
            newConn.Close()
        End Using

        Dim newDir As String = info.ToString
        System.IO.Directory.CreateDirectory(newDir)
        'Folder permissions are now handled by the egnyte poller
        'If Not Row.Item("PermissionsNeeded") Is DBNull.Value Then
        '    Dim FolderInfo As IO.DirectoryInfo = New IO.DirectoryInfo(newDir)
        '    Dim FolderAcl As New DirectorySecurity
        '    FolderAcl.SetAccessRuleProtection(True, True)

        '    For Each r As DataRow In _dtAttachmentPermissions.Rows()
        '        If r.Item("AttachmentType") = Row.Item("AttachmentType") Then

        '            Dim group As String = r.Item("GroupPath")

        '            If r.Item("allowFullControl") = True Then
        '                FolderAcl.AddAccessRule(New FileSystemAccessRule(group, FileSystemRights.FullControl, InheritanceFlags.None, PropagationFlags.NoPropagateInherit, AccessControlType.Allow))
        '            End If
        '            If r.Item("allowModify") = True Then
        '                FolderAcl.AddAccessRule(New FileSystemAccessRule(group, FileSystemRights.Modify, InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow))
        '            End If
        '            If r.Item("allowRead&Execute") = True Then
        '                FolderAcl.AddAccessRule(New FileSystemAccessRule(group, FileSystemRights.ReadAndExecute, InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow))
        '            End If
        '            If r.Item("allowListFolderContents") = True Then
        '                FolderAcl.AddAccessRule(New FileSystemAccessRule(group, FileSystemRights.ListDirectory, InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow))
        '            End If
        '            If r.Item("allowRead") = True Then
        '                FolderAcl.AddAccessRule(New FileSystemAccessRule(group, FileSystemRights.Read, InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow))
        '            End If
        '            If r.Item("allowWrite") = True Then
        '                FolderAcl.AddAccessRule(New FileSystemAccessRule(group, FileSystemRights.Write, InheritanceFlags.ContainerInherit Or InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow))
        '            End If
        '            FolderInfo.SetAccessControl(FolderAcl)
        '        End If
        '    Next
        'End If
    End Sub

    Public Function getEmployeeAttachmentFolderPath(ByVal empno As String, ByVal attachmentTypeCode As String) As String
        Dim attachmentCategory = "Employees"
        Dim folderPath As String = String.Empty
        Dim GUID As String = String.Empty

        Dim attachmentTypeRow As DataRow = Nothing

        'Dim GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY = getFinesseDataRootDirectory()

        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))

        jammer.Add("select p.GUID
                    from dbo.peemployee p
                    where p.empno = " & SQLQuote(empno),
                   Sub(t)
                       If t.Rows.Count > 0 Then
                           GUID = t.Rows(0).Item("GUID").ToString
                       End If
                   End Sub)
        jammer.Add("SELECT act.AttachmentCategory, at.AttachmentTypeDescription, Permissionsneeded = is_rolemember(DatabaseRole), act.AttachmentType, atb.DatabaseRole,
						 CASE ISNULL (is_rolemember(DatabaseRole), 1)
						 WHEN 0 THEN 0
						 ELSE 1
						 END AS hasPermissions
                         FROM dbo.AttachmentCategoryTypes act
                         JOIN dbo.AttachmentTypes at ON act.AttachmentType = at.AttachmentType
						 LEFT OUTER JOIN dbo.AttachmentTypeDatabaseRoles atb ON atb.AttachmentType = at.AttachmentType
                         WHERE act.AttachmentCategory = " & SQLQuote(attachmentCategory) & " and act.AttachmentType = " & SQLQuote(attachmentTypeCode),
                   Sub(t)
                       If t.Rows.Count > 0 Then
                           attachmentTypeRow = t.Rows(0)
                       End If
                   End Sub)
        jammer.Execute()

        folderPath = getAttachmentTypeFolderPath(GUID, attachmentCategory)

        Dim rootFolderPath As String = folderPath

        folderPath = Path.Combine(folderPath, attachmentTypeRow.Item("AttachmentTypeDescription"))

        If Not Directory.Exists(folderPath) Then
            createFilestoragePathsEntry(rootFolderPath, GUID)
            Dim directoryInfo As DirectoryInfo = New DirectoryInfo(folderPath)
            createSubDirectories(directoryInfo, attachmentTypeRow, attachmentCategory)
        End If

        Return folderPath
    End Function

    Public Function getCustomerOrderAttachmentFolderPath(ByVal orderno As String, ByVal attachmentTypeCode As String) As Dictionary(Of String, String)
        Dim attachmentCategory = "CustomerOrders"
        Dim folderPath As String = String.Empty
        Dim GUID As String = String.Empty

        Dim attachmentTypeRow As DataRow = Nothing

        'Dim GLOBAL_OPS_FINESSE_DATA_ROOT_DIRECTORY = getFinesseDataRootDirectory()

        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))

        jammer.Add("select o.fileStorageGUID
                    from dbo.oecohead o
                    where o.orderno = " & SQLQuote(orderno),
                   Sub(t)
                       If t.Rows.Count > 0 Then
                           GUID = t.Rows(0).Item("fileStorageGUID").ToString
                       End If
                   End Sub)
        jammer.Add("SELECT act.AttachmentCategory, at.AttachmentTypeDescription, Permissionsneeded = is_rolemember(DatabaseRole), act.AttachmentType, atb.DatabaseRole,
						 CASE ISNULL (is_rolemember(DatabaseRole), 1)
						 WHEN 0 THEN 0
						 ELSE 1
						 END AS hasPermissions
                         FROM dbo.AttachmentCategoryTypes act
                         JOIN dbo.AttachmentTypes at ON act.AttachmentType = at.AttachmentType
						 LEFT OUTER JOIN dbo.AttachmentTypeDatabaseRoles atb ON atb.AttachmentType = at.AttachmentType
                         WHERE act.AttachmentCategory = " & SQLQuote(attachmentCategory) & " and act.AttachmentType = " & SQLQuote(attachmentTypeCode),
                   Sub(t)
                       If t.Rows.Count > 0 Then
                           attachmentTypeRow = t.Rows(0)
                       End If
                   End Sub)
        jammer.Execute()

        folderPath = getAttachmentTypeFolderPath(GUID.ToString, attachmentCategory)

        Dim rootFolderPath As String = folderPath

        folderPath = Path.Combine(folderPath, attachmentTypeRow.Item("AttachmentTypeDescription"))

        If Not Directory.Exists(folderPath) Then
            createFilestoragePathsEntry(rootFolderPath, GUID)
            Dim directoryInfo As DirectoryInfo = New DirectoryInfo(folderPath)
            createSubDirectories(directoryInfo, attachmentTypeRow, attachmentCategory)
        End If

        Dim returnDictionary As New Dictionary(Of String, String)
        returnDictionary.Add("GUID", GUID)
        returnDictionary.Add("folderPath", folderPath)

        Return returnDictionary
    End Function

    Public Function getProjectRootPhaseNumber(ByVal fullentityno As String) As String
        Dim characterstoCutoff = fullentityno.IndexOf("-", fullentityno.IndexOf("-") + 1)
        Dim entityno As String
        If Not (characterstoCutoff = -1) Then
            entityno = fullentityno.Remove(characterstoCutoff)
        Else
            entityno = fullentityno
        End If
        Return entityno
    End Function

    Private Sub createFolderDBEntry(ByVal attachmentType As DataRow, ByVal entityno As String)

        Dim _dtUserFolders As DataTable = Nothing
        Dim AttachmentTypesToCreateDBEntriesFor As DataTable = Nothing

        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))
        jammer.Add("SELECT puftcsf.entityno, puftcsf.UserFolderPath, puftcsf.dropboxFolderID, puftcsf.CloudFolderTemplate, puftcsf.AttachmentType, puftcsf.id_Level, cfspft.CloudFolderTemplate, cfspft.DropboxFilePathSuffix 
                         FROM dbo.ProjectsUsersFoldersToCloudStorageFolders AS puftcsf
                         JOIN dbo.CloudFileStoragePermissionFolderTemplate AS cfspft ON cfspft.CloudFolderTemplate = puftcsf.CloudFolderTemplate
                         WHERE puftcsf.entityno = " & SQLQuote(entityno) & " and puftcsf.AttachmentType = " & SQLQuote(attachmentType.Item("AttachmentType")),
                             Sub(t)
                                 _dtUserFolders = t
                             End Sub)
        jammer.Add("select c.AttachmentType, c.CloudFolderTemplate
                    from dbo.CloudFileStorageGroupsToAttachmentTypes c",
                    Sub(t)
                        AttachmentTypesToCreateDBEntriesFor = t
                    End Sub)
        jammer.Execute()

        Dim folderSearch As DataRow() = _dtUserFolders.Select("entityno = " & SQLQuote(entityno) & " AND UserFolderPath = " & SQLQuote(attachmentType.Item("AttachmentTypeDescription")))

        If folderSearch.Length > 0 Then
            Exit Sub
        End If

        Dim uploadTest As DataRow() = AttachmentTypesToCreateDBEntriesFor.Select("AttachmentType = " & SQLQuote(attachmentType.Item("AttachmentType")))
        'PROJCONTRACT
        If uploadTest.Length = 0 Then
            Exit Sub
        End If


        Dim idLevel = getIDLevelFromTemplateType(attachmentType.Item("CloudFolderTemplate"))

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
                            " & SQLQuote(entityno) & ", -- entityno - uniqueidentifier
                            " & SQLQuote(attachmentType.Item("AttachmentTypeDescription")) & ",   -- UserFolderPath - varchar(100)
                             " & SQLQuote(attachmentType.Item("CloudFolderTemplate")) & ",
                            " & idLevel & ",
                             " & SQLQuote(attachmentType.Item("AttachmentType")) & "
                        )", newConn)

                query.ExecuteNonQuery()
                newConn.Close()
            End Using

        Catch ex As System.Exception

        End Try
    End Sub

    Private Function getAttachmentTypeFolderPath(fileStorageGUID As String, attachmentCategory As String)
        Dim defaultRootPath As String = String.Empty
        Dim alreadyEstablishedFolderPath As String = String.Empty


        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))
        jammer.Add("select fsp.fileStoragePath, fsp.fileStorageGUID
                    from dbo.fileStoragePaths AS fsp
                    WHERE fsp.fileStorageGUID = " & SQLQuote(fileStorageGUID),
                         Sub(t)
                             If t.Rows.Count > 0 Then
                                 alreadyEstablishedFolderPath = t.Rows(0).Item("fileStoragePath")
                             End If
                         End Sub)
        jammer.Add("select ac.AttachmentCategory, ac.attachmentsCanBeInKnowledgeBase, ac.defaultRootFolderPath
                    from dbo.AttachmentCategory AS ac
                    WHERE ac.AttachmentCategory = " & SQLQuote(attachmentCategory),
                    Sub(t)
                        defaultRootPath = t.Rows(0).Item("defaultRootFolderPath")
                    End Sub)
        jammer.Execute()

        If alreadyEstablishedFolderPath IsNot String.Empty Then
            Return alreadyEstablishedFolderPath
        Else
            Return getRootDefaultFolderPath(fileStorageGUID, attachmentCategory)
        End If


    End Function


    Private Function getRootDefaultFolderPath(fileStorageGUID As String, AttachmentCategory As String) As String
        Dim path As String = String.Empty

        Dim conn As SqlConnection = GetOpenedFinesseConnection()

        Dim sqlComm As SqlCommand = New SqlCommand("dbo.get_default_file_Storage_path", conn)
        sqlComm.CommandType = CommandType.StoredProcedure
        sqlComm.Parameters.Add("@GUID", SqlDbType.UniqueIdentifier).Value = New Guid(fileStorageGUID)
        sqlComm.Parameters.Add("@AttachmentType", SqlDbType.VarChar, 50).Value = AttachmentCategory
        sqlComm.Parameters.Add("@path", SqlDbType.VarChar, 255)
        sqlComm.Parameters("@path").Direction = ParameterDirection.Output
        sqlComm.ExecuteNonQuery()
        path = sqlComm.Parameters.Item("@path").Value

        conn.Close()
        conn.Dispose()

        Return path
    End Function

    Public Sub createFilestoragePathsEntry(folderPath As String, fileStorageGUID As String)
        Dim conn As SqlConnection = GetOpenedFinesseConnection()

        Dim query As New SqlCommand("dbo.createFileStoragePathEntry", conn)
        query.CommandType = CommandType.StoredProcedure
        query.Parameters.Add("@folderPath", SqlDbType.VarChar, 255).Value = folderPath
        query.Parameters.Add("@GUID", SqlDbType.UniqueIdentifier).Value = New Guid(fileStorageGUID)
        query.ExecuteNonQuery()

        conn.Close()
        conn.Dispose()
    End Sub

    Private Function getIDLevelFromTemplateType(ByVal Template As String) As Integer

        Dim _dtCloudFolderTemplatesIDLevel As DataTable = Nothing

        Dim jammer As New SQLJammer(New SqlConnection(FinesseConnectionString))
        jammer.Add("SELECT cfsgtpf.CloudFolderTemplate, cfsgtpf.id_Level 
                        FROM dbo.CloudFileStorageGroupsToPermissionFolders AS cfsgtpf",
                             Sub(t)
                                 _dtCloudFolderTemplatesIDLevel = t
                             End Sub)
        jammer.Execute()

        Dim templateIDLevel = _dtCloudFolderTemplatesIDLevel.Select("CloudFolderTemplate = " & SQLQuote(Template))
        Dim idLevel As Integer = 100

        For Each row As DataRow In templateIDLevel
            If idLevel > row.Item("id_Level") Then
                idLevel = row.Item("id_Level")
            End If
        Next
        Return idLevel
    End Function



    Public Function cleanFileOrFolderName(filePath As String) As String

        Dim cleanName As String = filePath

        For Each c As Char In Path.GetInvalidFileNameChars()
            cleanName = cleanName.Replace(c, "")
        Next

        Return cleanName
    End Function


    Public Function getDefaultRootPathForAttachmentCategory(attachmentCategory As String) As String
        Dim conn As SqlConnection = GetOpenedFinesseConnection()

        Dim rootPath As String = conn.ExecuteScalar($"select ac.defaultRootFolderPath
                                                        from dbo.AttachmentCategory AS ac
                                                        WHERE ac.AttachmentCategory = '{attachmentCategory}'")

        conn.Close()
        conn.Dispose()

        Return rootPath
    End Function

End Module
