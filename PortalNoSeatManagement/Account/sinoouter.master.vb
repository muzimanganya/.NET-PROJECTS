
Partial Class Account_sinoouter
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If System.Web.HttpContext.Current.User.Identity.IsAuthenticated Then
                Dim clsDataLayer As New Datalayer
            End If
        End If
    End Sub
End Class

