Imports System.Security.Cryptography.X509Certificates
Imports System.ServiceModel
Imports System.Web.Configuration

Imports Microsoft.IdentityModel.Claims
Imports Microsoft.IdentityModel.Configuration
Imports Microsoft.IdentityModel.Protocols.WSTrust
Imports Microsoft.IdentityModel.SecurityTokenService

''' <summary>
''' A custom SecurityTokenService implementation.
''' </summary>
Public Class CustomSecurityTokenService
    Inherits SecurityTokenService
    ' TODO: Set enableAppliesToValidation to true to enable only the RP Url's specified in the PassiveRedirectBasedClaimsAwareWebApps array to get a token from this STS
    Shared enableAppliesToValidation As Boolean = False

    ' TODO: Add relying party Url's that will be allowed to get token from this STS
    '"https://localhost/PassiveRedirectBasedClaimsAwareWebApp"
    Shared ReadOnly PassiveRedirectBasedClaimsAwareWebApps As String() = {}

    ''' <summary>
    ''' Creates an instance of CustomSecurityTokenService.
    ''' </summary>
    ''' <param name="configuration">The SecurityTokenServiceConfiguration.</param>
    Public Sub New(configuration As SecurityTokenServiceConfiguration)
        MyBase.New(configuration)
    End Sub

    ''' <summary>
    ''' Validates appliesTo and throws an exception if the appliesTo is null or contains an unexpected address.
    ''' </summary>
    ''' <param name="appliesTo">The AppliesTo value that came in the RST.</param>
    ''' <exception cref="ArgumentNullException">If 'appliesTo' parameter is null.</exception>
    ''' <exception cref="InvalidRequestException">If 'appliesTo' is not valid.</exception>
    Private Sub ValidateAppliesTo(appliesTo As EndpointAddress)
        If appliesTo Is Nothing Then
            Throw New ArgumentNullException("appliesTo")
        End If

        ' TODO: Enable AppliesTo validation for allowed relying party Urls by setting enableAppliesToValidation to true. By default it is false.
        If enableAppliesToValidation Then
            Dim validAppliesTo As Boolean = False
            For Each rpUrl As String In PassiveRedirectBasedClaimsAwareWebApps
                If appliesTo.Uri.Equals(New Uri(rpUrl)) Then
                    validAppliesTo = True
                    Exit For
                End If
            Next

            If Not validAppliesTo Then
                Throw New InvalidRequestException([String].Format("The 'appliesTo' address '{0}' is not valid.", appliesTo.Uri.OriginalString))
            End If
        End If
    End Sub

    ''' <summary>
    ''' This method returns the configuration for the token issuance request. The configuration
    ''' is represented by the Scope class. In our case, we are only capable of issuing a token for a
    ''' single RP identity represented by the EncryptingCertificateName.
    ''' </summary>
    ''' <param name="principal">The caller's principal.</param>
    ''' <param name="request">The incoming RST.</param>
    ''' <returns>The scope information to be used for the token issuance.</returns>
    Protected Overrides Function GetScope(principal As IClaimsPrincipal, request As RequestSecurityToken) As Scope
        ValidateAppliesTo(request.AppliesTo)

        '
        ' Note: The signing certificate used by default has a Distinguished name of "CN=STSTestCert",
        ' and is located in the Personal certificate store of the Local Computer. Before going into production,
        ' ensure that you change this certificate to a valid CA-issued certificate as appropriate.
        '
        Dim scope As New Scope(request.AppliesTo.Uri.OriginalString, SecurityTokenServiceConfiguration.SigningCredentials)

        Dim encryptingCertificateName As String = WebConfigurationManager.AppSettings("EncryptingCertificateName")
        If Not String.IsNullOrEmpty(encryptingCertificateName) Then
            ' Important note on setting the encrypting credentials.
            ' In a production deployment, you would need to select a certificate that is specific to the RP that is requesting the token.
            ' You can examine the 'request' to obtain information to determine the certificate to use.
            scope.EncryptingCredentials = New X509EncryptingCredentials(CertificateUtil.GetCertificate(StoreName.My, StoreLocation.LocalMachine, encryptingCertificateName))
        Else
            ' If there is no encryption certificate specified, the STS will not perform encryption.
            ' This will succeed for tokens that are created without keys (BearerTokens) or asymmetric keys.  
            scope.TokenEncryptionRequired = False
        End If

        ' Set the ReplyTo address for the WS-Federation passive protocol (wreply). This is the address to which responses will be directed. 
        ' In this template, we have chosen to set this to the AppliesToAddress.
        scope.ReplyToAddress = scope.AppliesToAddress

        Return scope
    End Function


    ''' <summary>
    ''' This method returns the claims to be issued in the token.
    ''' </summary>
    ''' <param name="principal">The caller's principal.</param>
    ''' <param name="request">The incoming RST, can be used to obtain addtional information.</param>
    ''' <param name="scope">The scope information corresponding to this request.</param> 
    ''' <exception cref="ArgumentNullException">If 'principal' parameter is null.</exception>
    ''' <returns>The outgoing claimsIdentity to be included in the issued token.</returns>
    Protected Overrides Function GetOutputClaimsIdentity(principal As IClaimsPrincipal, request As RequestSecurityToken, scope As Scope) As IClaimsIdentity
        If principal Is Nothing Then
            Throw New ArgumentNullException("principal")
        End If

        Dim outputIdentity As New ClaimsIdentity()

        ' Issue custom claims.
        ' TODO: Change the claims below to issue custom claims required by your application.
        ' Update the application's configuration file too to reflect new claims requirement.

        outputIdentity.Claims.Add(New Claim(System.IdentityModel.Claims.ClaimTypes.Name, principal.Identity.Name))
        outputIdentity.Claims.Add(New Claim(ClaimTypes.Role, "Manager"))

        Return outputIdentity
    End Function
End Class
