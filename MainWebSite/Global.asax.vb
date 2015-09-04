
Public Class Global_asax
    Inherits HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)


        If String.Compare(Request.Path, Request.ApplicationPath, StringComparison.InvariantCultureIgnoreCase = 0 And Not (Request.Path.EndsWith("/"))) Then
            Response.Redirect(Request.Path + "/")
        End If

    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub
End Class