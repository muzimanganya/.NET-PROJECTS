Imports System.Data.SqlClient
Imports System.Data

Partial Class QueryDB
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Try
        If Page.IsPostBack Then
            Dim pageToQuery As String = Request("ctl00$ctl00$body$content$ddlModules").ToString()
            Dim targetDate As String = Request("ctl00$ctl00$body$content$txtDate").ToString()

            If Not String.IsNullOrEmpty(pageToQuery) AndAlso Not String.IsNullOrEmpty(targetDate) Then
                Dim redString As String = String.Format("{0}?requestDate={1}", pageToQuery, targetDate)
                Response.Redirect(redString)
            Else
                'Bad or Empty Date
            End If
        Else
            'Populate the DropDown List 
            If User.IsInRole("restricted") Then
                ddlModules.Items.Add(New ListItem("Day's Planned Traffic", "All-Days--Planned-Traffic.aspx"))
                ddlModules.Items.Add(New ListItem("Pre-Bookings", "PreBookings.aspx"))
                ddlModules.Items.Add(New ListItem("Mobile Tickets", "MobileTickets.aspx"))
            Else
                ddlModules.Items.Add(New ListItem("Day's Planned Traffic", "All-Days--Planned-Traffic.aspx"))
                ddlModules.Items.Add(New ListItem("Traffic & Business Overview", "traffic.aspx"))
                ddlModules.Items.Add(New ListItem("POS Ticketing", "POSTicketing.aspx"))
                ddlModules.Items.Add(New ListItem("Mobile Tickets", "MobileTickets.aspx"))
                ddlModules.Items.Add(New ListItem("POS Subscription", "POSSubscription.aspx"))
                ddlModules.Items.Add(New ListItem("POS Booking", "POSBooking.aspx"))
                ddlModules.Items.Add(New ListItem("POS Promotions", "POSPromotions.aspx"))
                ddlModules.Items.Add(New ListItem("Client Code Activity", "Activity.aspx"))
                ddlModules.Items.Add(New ListItem("Deleted Tickets", "RemovedTraffic/DeletedTickets.aspx"))
                ddlModules.Items.Add(New ListItem("Pre-Bookings", "PreBookings.aspx"))
                'For Manager and Admin
                If User.IsInRole("Admin") Then
                    ddlModules.Items.Add(New ListItem("Rolled Back Tickets", "RemovedTraffic/RolledBackTickets.aspx"))
                End If
            End If
        End If
        'Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try

        Page.Title = "Query Database Information"
    End Sub
End Class
