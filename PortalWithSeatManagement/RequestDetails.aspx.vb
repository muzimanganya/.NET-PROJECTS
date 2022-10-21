Imports System.Data.SqlClient
Imports System.Data

Partial Class RequestDetails
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Public RequestType As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Try

        Dim requestID As String = Request("RequestID").ToString()
        Dim queryRequest As String = String.Format("SELECT bs.*,s.NAME AS 'BusName' FROM dbo.BusShareRequests AS bs INNER JOIN su.SALES as s ON bs.BusID = s.IDSALE WHERE RequestID ={0}", requestID)
        Dim dt As DataTable = clsDataLayer.ReturnDataTable(queryRequest)
        RequestDetails.DataSource = dt
        RequestDetails.DataBind()

        RequestType = dt.Rows(0)("Category")

        'Now Construct the QR Code
        'https://chart.googleapis.com/chart?cht=qr&chs=160x160&chl=840240239+KIGALI-MUHANGA:09H00+PETER
        Dim tinfo As String = ""
        With dt.Rows(0)
            tinfo = String.Format("Bus: {0}. Requested Seat: {1}. Status: {2}", .Item("BusName").ToString, .Item("SeatsNo").ToString, .Item("Status").ToString)
        End With

        Dim qr As String = String.Format("https://chart.googleapis.com/chart?cht=qr&chs=190x190&chl={0}", tinfo)
        imgQr.ImageUrl = qr

        'Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try

        Page.Title = "Ticket Details"
    End Sub

  
End Class
