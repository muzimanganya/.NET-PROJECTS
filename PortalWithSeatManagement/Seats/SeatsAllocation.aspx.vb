Imports System.Data.SqlClient
Imports System.Data

Partial Class SeatsAllocation
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim busname As String = ""
        Dim idsale As String = ""
        Try
            idsale = Request("idsale").ToString()
        Catch ex As Exception
            idsale = ""
        End Try

        If idsale IsNot String.Empty Then
            busname = clsDataLayer.ReturnSingleValue(String.Format("SELECT NAME from su.SALES WHERE IDSALE={0}", idsale))
        End If

        sdsStopSeats.ConnectionString = clsDataLayer.ReturnConnectionString()
        sdsStopSeats.SelectParameters("NAME").DefaultValue = busname
        sdsStopSeats.UpdateParameters("Username").DefaultValue = HttpContext.Current.User.Identity.Name

        Page.Title = "Bus Seats Allocation"
    End Sub
End Class
