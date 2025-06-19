Imports System.Collections.Specialized
Imports System.Configuration

Public NotInheritable Class FinesseSettingsProvider
    Inherits SettingsProvider
    Implements IApplicationSettingsProvider

    Private settingValues As New NameValueCollection()

    Public Overrides Sub Initialize(name As String, config As NameValueCollection)
        Debug.WriteLine("in initialize override")
        MyBase.Initialize(Me.ApplicationName, settingValues)
    End Sub

    ''' <summary>
    ''' MSDN states this property should be implemented with this getter and a do nothing setter.
    ''' </summary>
    Public Overrides Property ApplicationName() As String
        Get
            Return (System.Reflection.Assembly.GetExecutingAssembly().GetName().Name)
        End Get
        Set(value As String)
            Debug.WriteLine("set application name called")
        End Set
    End Property


    Public Overrides Function GetPropertyValues(context As SettingsContext, collection As SettingsPropertyCollection) As SettingsPropertyValueCollection
        Dim returnValue As New SettingsPropertyValueCollection()
        For Each item As SettingsProperty In collection
            Dim addMe As New SettingsPropertyValue(item)
            addMe.PropertyValue = [String].Empty
            returnValue.Add(addMe)
        Next

        Return returnValue
    End Function

    Public Overrides Sub SetPropertyValues(context As SettingsContext, collection As SettingsPropertyValueCollection)
        Debug.WriteLine("in setPropertyValues")

        For Each item As SettingsPropertyValue In collection
            Dim isUserScoped As Boolean = (TypeOf item.[Property].Attributes(GetType(UserScopedSettingAttribute)) Is UserScopedSettingAttribute)
            Dim isAppScoped As Boolean = (TypeOf item.[Property].Attributes(GetType(ApplicationScopedSettingAttribute)) Is ApplicationScopedSettingAttribute)
            If isUserScoped AndAlso isAppScoped Then
                Throw New ConfigurationErrorsException("Property can't be userScoped and appScoped according to msdn: http://msdn.microsoft.com/en-us/library/system.configuration.settingsprovider(VS.80).aspx")
            End If
        Next
    End Sub

    Public Function GetPreviousVersion(context As SettingsContext, [property] As SettingsProperty) As SettingsPropertyValue Implements IApplicationSettingsProvider.GetPreviousVersion
        Debug.WriteLine("in getPreviousVersion")
        Throw New NotSupportedException("Get Previous Version is not supported")
    End Function

    Public Sub Reset(context As SettingsContext) Implements IApplicationSettingsProvider.Reset
        Debug.WriteLine("in Reset")
    End Sub

    Public Sub Upgrade(context As SettingsContext, properties As SettingsPropertyCollection) Implements IApplicationSettingsProvider.Upgrade
        Debug.WriteLine("in Upgrade")
    End Sub

End Class
