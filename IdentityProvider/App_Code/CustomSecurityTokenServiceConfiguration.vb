Imports System.Security.Cryptography.X509Certificates
Imports System.Web
Imports System.Web.Configuration

Imports Microsoft.IdentityModel.Configuration
Imports Microsoft.IdentityModel.SecurityTokenService

''' <summary>
''' A custom SecurityTokenServiceConfiguration implementation.
''' </summary>
Public Class CustomSecurityTokenServiceConfiguration
    Inherits SecurityTokenServiceConfiguration
    Shared ReadOnly syncRoot As New Object()
    Const CustomSecurityTokenServiceConfigurationKey As String = "CustomSecurityTokenServiceConfigurationKey"

    ''' <summary>
    ''' Provides a model for creating a single Configuration object for the application. The first call creates a new CustomSecruityTokenServiceConfiguration and 
    ''' places it into the current HttpApplicationState using the key "CustomSecurityTokenServiceConfigurationKey". Subsequent calls will return the same
    ''' Configuration object.  This maintains any state that is set between calls and improves performance.
    ''' </summary>
    Public Shared ReadOnly Property Current() As CustomSecurityTokenServiceConfiguration
        Get
            Dim httpAppState As HttpApplicationState = HttpContext.Current.Application

            Dim customConfiguration As CustomSecurityTokenServiceConfiguration = TryCast(httpAppState.[Get](CustomSecurityTokenServiceConfigurationKey), CustomSecurityTokenServiceConfiguration)

            If customConfiguration Is Nothing Then
                SyncLock syncRoot
                    customConfiguration = TryCast(httpAppState.[Get](CustomSecurityTokenServiceConfigurationKey), CustomSecurityTokenServiceConfiguration)

                    If customConfiguration Is Nothing Then
                        customConfiguration = New CustomSecurityTokenServiceConfiguration()
                        httpAppState.Add(CustomSecurityTokenServiceConfigurationKey, customConfiguration)
                    End If
                End SyncLock
            End If

            Return customConfiguration
        End Get
    End Property

    ''' <summary>
    ''' CustomSecurityTokenServiceConfiguration constructor.
    ''' </summary>
    Public Sub New()
        MyBase.New(WebConfigurationManager.AppSettings(Common.IssuerName), New X509SigningCredentials(CertificateUtil.GetCertificate(StoreName.My, StoreLocation.LocalMachine, WebConfigurationManager.AppSettings(Common.SigningCertificateName))))
        Me.SecurityTokenService = GetType(CustomSecurityTokenService)
    End Sub
End Class

