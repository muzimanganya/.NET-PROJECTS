
Partial Class volcano_Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Response.Status = "301 Moved Permanently"
            Response.AddHeader("Location", "http://s.innovys.co.rw/Default.aspx?co=volcano")
        Catch ex As Exception

        End Try
        
    End Sub
End Class
