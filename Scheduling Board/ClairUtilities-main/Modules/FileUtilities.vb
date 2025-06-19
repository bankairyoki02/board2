Imports System.Runtime.CompilerServices
Imports System.Text
Imports System.Runtime.InteropServices

Public Module FileUtilities

    <Extension>
    Public Function GetRelativePath(file As System.IO.FileInfo, baseFolderPath As String) As String
        Return GetRelativePath(file.FullName, baseFolderPath)
    End Function

    <Extension>
    Public Function GetRelativePath(folder As System.IO.DirectoryInfo, baseFolderPath As String) As String
        Return GetRelativePath(folder.FullName, baseFolderPath)
    End Function

    <DllImport("shlwapi.dll", CharSet:=CharSet.Auto)>
    Public Function PathRelativePathTo(
     ByVal pszPath As StringBuilder,
     ByVal pszFrom As String,
     ByVal dwAttrFrom As Integer,
     ByVal pszTo As String,
     ByVal dwAttrTo As Integer) As Boolean
    End Function

    Private Function GetRelativePath(childPath As String, baseFolderPath As String)
        If String.IsNullOrEmpty(childPath) Then Throw New ArgumentNullException("childPath")
        If String.IsNullOrEmpty(baseFolderPath) Then Throw New ArgumentNullException("baseFolderPath")

        If Not baseFolderPath.EndsWith("\") Then
            baseFolderPath &= "\"
        End If

        Dim sb As New StringBuilder(1000)
        If PathRelativePathTo(sb, baseFolderPath, 0, childPath, 0) Then
            Dim relativePath = sb.ToString
            If relativePath.StartsWith(".\") Then
                relativePath = relativePath.Substring(2)
            End If
            Return relativePath
        Else
            Throw New InvalidOperationException($"{childPath} is not relative to {baseFolderPath}")
        End If
    End Function

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
End Module
