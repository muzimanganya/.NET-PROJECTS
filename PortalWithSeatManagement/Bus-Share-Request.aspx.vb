Imports System.Data.SqlClient
Imports System.Data

Partial Class BusShareRequest
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Dim bsCapacity As Integer, ksCapacity As Integer, bsSold As Integer, ksSold As Integer, sisterBus As Double
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try
        Dim cfg As New Config
        Dim busname As String, sentIdsale As String
        If Page.IsPostBack Then
            Try
                Dim idsale As String = ""
                Try
                    idsale = Request("IDSALE").ToString()
                Catch ex As Exception
                    idsale = ""
                End Try
                If idsale IsNot String.Empty Then
                    Dim seats As String = Request("ctl00$ctl00$body$content$ddlSeats")
                    Dim comments As String = Request("ctl00$ctl00$body$content$txtComment")

                    'We need to insert this to the other companies database right awayfile
                    If seats IsNot String.Empty Then
                        Dim queryInsert As String = String.Format("INSERT INTO dbo.BusShareRequests(FROMUSERNAME,BUSID,SeatsNo,CreatedON,Status,RequestComment,Category)" &
                                                                  " VALUES('{0}','{1}','{2}',GETDATE(),'Requested','{3}','FBV')", HttpContext.Current.User.Identity.Name, idsale, seats, comments)
                        Dim kDatalayer As New Datalayer


                        With kDatalayer
                            .SQLDatabase = "KigaliSafaris"
                            .SqlPassword = cfg.SQLPassword
                            .SqlServerName = cfg.SQLServer
                            .sqlUserId = cfg.SQLUser
                            .executeCommand(queryInsert)
                        End With

                        'Now insert it into our own records
                        With clsDataLayer
                            queryInsert = String.Format("INSERT INTO dbo.BusShareRequests(FROMUSERNAME,BUSID,SeatsNo,CreatedON,Status,RequestComment,Category)" &
                                                                  " VALUES('{0}','{1}','{2}',GETDATE(),'Requested','{3}','TKS')", HttpContext.Current.User.Identity.Name, idsale, seats, comments)
                            .executeCommand(queryInsert)
                        End With
                        'If we have succeeded then we redirect to the bus sharing requests page
                        Response.Redirect("Bus-Share.aspx?idsale=" + idsale)
                    End If
                Else
                    ltrError.Text = "Bus Was Not Properly Passed"
                End If
                
            Catch ex As Exception
                'Probably a Problem with Request Headers
                ltrError.Text = ex.Message
                clsDataLayer.LogException(ex)
            End Try

        Else
            'Get the bus we are dealing with
            Dim idsale As String = ""
            busname = ""
            Try
                idsale = Request("IDSALE").ToString() 'Of the mother bus
                busname = Request("busname").ToString() 'of the bus route they are sharing
            Catch ex As Exception
                idsale = "9"
            End Try
            If idsale IsNot String.Empty AndAlso busname IsNot String.Empty Then
                'We have a bus, get the current free seats, and populate the dropdown

                Dim querySeats As String = String.Format("SELECT s.FLD103 - (SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0}) AS 'RemainingSeats' FROm su.SALES as s WHERE s.IDSALE={0}", idsale)
                Dim remaining As Integer = clsDataLayer.ReturnSingleNumeric(querySeats)

                If remaining > 10 Then
                    'Add A Maximum of 10 seats
                    For i As Integer = 1 To 10
                        ddlSeats.Items.Add(i.ToString())
                    Next
                Else
                    'Add the Current Free Seats
                    For i As Integer = 1 To remaining
                        ddlSeats.Items.Add(i.ToString())
                    Next
                End If
            End If

            ' Load the each others capacity
            bsCapacity = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT FLD103 FROM su.SALES WHERE IDSALE={0}", idsale), cfg.SecurityDB)
            ksCapacity = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT FLD103 FROM su.SALES WHERE NAME='{0}'", busname), cfg.SharedBus)

            Dim busTwoIDSALE = clsDataLayer.ReturnSingleValue(String.Format("SELECT IDSALE FROM su.SALES WHERE NAME = '{0}'", busname), cfg.SharedBus)

            bsSold = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0}", idsale), cfg.SecurityDB)
            ksSold = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE NAME='{0}'", busname), cfg.SharedBus)

            lblBCapacity.Text = bsCapacity
            lblBFree.Text = bsCapacity - bsSold

            lblKCapacity.Text = ksCapacity
            lblKFree.Text = ksCapacity - ksSold
        End If
        ' Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '   Response.Redirect("/CustomError.aspx")
        'End Try

        Page.Title = "Request For Seats"
    End Sub
End Class
