Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Partial Class DriverList
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Private Shared updating As Boolean
    Private Shared DriverId As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then

                Dim query As String = "SELECT * FROM dbo.Drivers "

                Dim dt = clsDataLayer.ReturnDataTable(query)

                rptDrivers.DataSource = dt
                rptDrivers.DataBind()
            End If
        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try

        Page.Title = "Driver List"
    End Sub
    Private Function RandomFileName() As String
        Dim cfg As New Config
        Return cfg.WritePath
    End Function
End Class
