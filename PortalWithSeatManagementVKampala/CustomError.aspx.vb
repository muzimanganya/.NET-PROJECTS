Imports System.Data.SqlClient
Imports System.Data

Partial Class CustomError
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Page.Title = "System Error Encountered"
    End Sub
End Class
