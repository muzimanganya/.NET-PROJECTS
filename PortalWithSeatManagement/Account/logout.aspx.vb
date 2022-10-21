
Partial Class Account_logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = "Logout | Sinnovys Portal"
        FormsAuthentication.SignOut()
    End Sub
End Class
