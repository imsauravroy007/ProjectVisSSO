Imports System.Web.Security

Partial Public Class Login
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs)
        ' Note: Add code to validate user name, password. This code is for illustrative purpose only.
        ' Do not use it in production enviro


        If Not String.IsNullOrEmpty(txtUserName.Text) Then
            If Request.QueryString("ReturnUrl") IsNot Nothing Then
                FormsAuthentication.RedirectFromLoginPage(txtUserName.Text, False)
            Else
                FormsAuthentication.SetAuthCookie(txtUserName.Text, False)
                Response.Redirect("default.aspx")
            End If

        ElseIf Not IsPostBack Then
        End If
    End Sub
End Class
