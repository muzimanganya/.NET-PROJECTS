Imports System.Data.SqlClient
Imports System.Data

Partial Class MasterSeatsAllocation
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        sdsMaster.ConnectionString = clsDataLayer.ReturnConnectionString()

        Page.Title = "Master Seats Allocation | Sinnovys Portal"
    End Sub
End Class
