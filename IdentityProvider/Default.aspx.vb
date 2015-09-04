


Imports System.Globalization
Imports System.Web.UI

Imports Microsoft.IdentityModel.Protocols.WSFederation
Imports Microsoft.IdentityModel.SecurityTokenService
Imports Microsoft.IdentityModel.Web

''' <summary>
''' The Default Page Class
''' </summary>
Partial Public Class _Default
    Inherits Page
    ''' <summary>
    ''' Performs WS-Federation Passive Protocol processing. 
    ''' </summary>
    Protected Sub Page_PreRender(sender As Object, e As EventArgs)
        Dim action As String = Request.QueryString(WSFederationConstants.Parameters.Action)

        Try
            If action = WSFederationConstants.Actions.SignIn Then
                ' Process signin request.
                Dim requestMessage As SignInRequestMessage = DirectCast(WSFederationMessage.CreateFromUri(Request.Url), SignInRequestMessage)
                If User IsNot Nothing AndAlso User.Identity IsNot Nothing AndAlso User.Identity.IsAuthenticated Then
                    Dim sts As SecurityTokenService = New CustomSecurityTokenService(CustomSecurityTokenServiceConfiguration.Current)
                    Dim responseMessage As SignInResponseMessage = FederatedPassiveSecurityTokenServiceOperations.ProcessSignInRequest(requestMessage, User, sts)
                    FederatedPassiveSecurityTokenServiceOperations.ProcessSignInResponse(responseMessage, Response)
                Else
                    Throw New UnauthorizedAccessException()
                End If
            ElseIf action = WSFederationConstants.Actions.SignOut Then
                ' Process signout request.
                Dim requestMessage As SignOutRequestMessage = DirectCast(WSFederationMessage.CreateFromUri(Request.Url), SignOutRequestMessage)
                FederatedPassiveSecurityTokenServiceOperations.ProcessSignOutRequest(requestMessage, User, requestMessage.Reply, Response)
            Else
                Throw New InvalidOperationException([String].Format(CultureInfo.InvariantCulture, "The action '{0}' (Request.QueryString['{1}']) is unexpected. Expected actions are: '{2}' or '{3}'.", If([String].IsNullOrEmpty(action), "<EMPTY>", action), WSFederationConstants.Parameters.Action, WSFederationConstants.Actions.SignIn, WSFederationConstants.Actions.SignOut))
            End If
        Catch exception As Exception
            Throw New Exception("An unexpected error occurred when processing the request. See inner exception for details.", exception)
        End Try
    End Sub
End Class


