
Partial Class capital_Default
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Response.Status = "301 Moved Permanently"
            Response.AddHeader("Location", "http://capitalportal.innovys.co.rw/Default.aspx?co=capital&id=324y914y92410")
        Catch ex As Exception

        End Try

    End Sub
End Class
