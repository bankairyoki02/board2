Imports System.Runtime.CompilerServices
Imports System.Web.Script.Serialization

Public Module ExtendControlsFunctionalities
    <Extension()>
    Public Sub ModifyBehaviorIfTagIsDictionary(control As Control)
        Try
            If Not String.IsNullOrEmpty(control.Tag) Then
                Dim castResult = control.Tag.ToString.ToDictionaryFromString()
                If (castResult IsNot Nothing) Then
                    Dim tagDictionary As Dictionary(Of String, String) = castResult

                    If tagDictionary.ContainsKey("ReadPermission") Then
                        Dim readPermission As String = tagDictionary("ReadPermission")
                        control.Visible = UserSingleton.Instance.ContainsRole(readPermission)
                    End If

                    If tagDictionary.ContainsKey("WritePermission") Then
                        Dim writePermission As String = tagDictionary("WritePermission")
                        control.Enabled = UserSingleton.Instance.ContainsRole(writePermission)
                    End If

                    If tagDictionary.ContainsKey("Enabled") Then
                        Dim permission As String = tagDictionary("Enabled")
                        control.Enabled = UserSingleton.Instance.ContainsRole(permission)
                    End If

                    If tagDictionary.ContainsKey("Visible") Then
                        Dim permission As String = tagDictionary("Visible")
                        control.Visible = UserSingleton.Instance.ContainsRole(permission)
                    End If
                End If
            End If

            For Each childControl As Control In control.Controls
                childControl.ModifyBehaviorIfTagIsDictionary()
            Next

            Dim isToolsTrip = TryCast(control, ToolStrip)
            If (isToolsTrip IsNot Nothing) Then
                Dim toolsttrip = DirectCast(control, ToolStrip)
                For Each childControl As ToolStripItem In toolsttrip.Items
                    childControl.ModifyBehaviorIfTagIsDictionary()
                Next
            End If

        Catch ex As Exception

        End Try

    End Sub

    <Extension()>
    Public Sub ModifyBehaviorIfTagIsDictionary(control As ToolStripItem)
        Try
            If Not String.IsNullOrEmpty(control.Tag) Then
                Dim castResult = control.Tag.ToString.ToDictionaryFromString()
                If (castResult IsNot Nothing) Then
                    Dim tagDictionary As Dictionary(Of String, String) = castResult

                    If tagDictionary.ContainsKey("ReadPermission") Then
                        Dim readPermission As String = tagDictionary("ReadPermission")
                        control.Visible = UserSingleton.Instance.ContainsRole(readPermission)
                    End If

                    If tagDictionary.ContainsKey("WritePermission") Then
                        Dim writePermission As String = tagDictionary("WritePermission")
                        control.Enabled = UserSingleton.Instance.ContainsRole(writePermission)
                        control.ToolTipText = If(control.Enabled, control.Text, "Disabled because of permissions")
                    End If

                    If tagDictionary.ContainsKey("Enabled") Then
                        Dim permission As String = tagDictionary("Enabled")
                        control.Enabled = UserSingleton.Instance.ContainsRole(permission)
                        control.ToolTipText = If(control.Enabled, control.Text, "Disabled because of permissions")
                    End If

                    If tagDictionary.ContainsKey("Visible") Then
                        Dim permission As String = tagDictionary("Visible")
                        control.Visible = UserSingleton.Instance.ContainsRole(permission)
                    End If
                End If
            End If

        Catch ex As Exception

        End Try

    End Sub

    <Extension()>
    Public Function ToDictionaryFromString(input As String) As Dictionary(Of String, String)
        Try
            Dim serializer As New JavaScriptSerializer()
            Dim dictionary As Dictionary(Of String, String) = serializer.Deserialize(Of Dictionary(Of String, String))(input)
            Return dictionary
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Modify the behavior of all the controls inside the form.
    ''' This will check if any control contain the Tag property and if that json contains Read And Write Permissions details will change automatically
    ''' the Enable and Visible properties to the control
    ''' Copy/Paste the following json inside the Tag Property of any control:
    ''' <code>{"ReadPermission": "MyReadPermission", "WritePermission": "MyWritePermission"}</code>
    ''' </summary>
    ''' <param name="control">Ideally main Form, can be any control</param>
    Public Sub AttachLoadEventHandlers(control As Control)
        For Each childControl As Control In control.Controls
            control.ModifyBehaviorIfTagIsDictionary()
        Next
    End Sub
End Module