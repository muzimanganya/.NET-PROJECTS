Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Partial Class Configurations
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then

        Else
            Using reader = New StreamReader("C:\inetpub\wwwroot\sino\tmpConfig.config")
                txtConfig.Text = reader.ReadToEnd()
            End Using
            Page.Title = "System Configuration"
        End If
    End Sub
End Class
