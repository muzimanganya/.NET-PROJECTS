
Partial Class Account_sinoouter
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If System.Web.HttpContext.Current.User.Identity.IsAuthenticated Then
            Dim clsDataLayer As New Datalayer
            ltrCompany.Text = clsDataLayer.SQLDatabase.ToUpper + " | "
        End If
    End Sub
End Class

