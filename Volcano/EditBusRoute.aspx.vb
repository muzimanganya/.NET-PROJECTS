Imports System.Data.SqlClient
Imports System.Data

Partial Class EditBusRoute
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try
        Dim idsale As String = ""
        Try
            idsale = Request.QueryString("idsale")
        Catch ex As Exception
            'Overall Page Error Handling
        End Try

        If Page.IsPostBack Then
            Try
                Dim tdate As String = Request("ctl00$ctl00$body$content$txtDate").ToString()
                Dim Capacity As String = Request("ctl00$ctl00$body$content$txtCapacity").ToString()
                Dim status As String = Request("ctl00$ctl00$body$content$ddlStatus")
                'Dim memo As String = Request("ctl00$ctl00$body$content$txtMemo")
                Dim comment As String = Request("ctl00$ctl00$body$content$txtComment")
                Dim memo As String = ""
                Dim hour As String = Request("ctl00$ctl00$body$content$txtHour")
                Try
                    'Do initial value checks
                    If (Convert.ToInt16(Capacity)) Then
                        'The Capacity is an integer
                        If Not (Capacity > 0 And Capacity < 70) Then
                            Throw New Exception("Capacity Must Be Between 1 & 70 you entered " + Capacity + ". Please check and try again")
                        End If
                    Else
                        Throw New Exception("Please Enter a Valid Bus Capacity")
                    End If
                    'Get the Previous Capacity From the Database
                    Dim capacityNow As Integer = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT FLD103 FROM su.SALES WHERE IDSALE={0}", idsale))
                    memo = clsDataLayer.ReturnSingleValue(String.Format("SELECT ISNULL(Memo,' ') FROM su.SALES WHERE IDSALE={0}", idsale))
                    Dim currentUser As String = HttpContext.Current.User.Identity.Name.ToUpper()

                    'Check that the idsale is valid
                    Dim busname As String = clsDataLayer.ReturnSingleValue(String.Format("SELECT NAME FROM su.SALES WHERE IDSALE = {0}", idsale))
                    If busname IsNot Nothing Then
                        'The IDSALE is Okay! Construct the update query
                        memo += String.Format("{4} Changed Bus To: Capacity:={1},Status:= {2},TargetDate:= {0},Hour:= {3}. because {5}<br />", tdate, Capacity, status, hour, currentUser, comment)
                        Dim updateQuery As String = String.Format("UPDATE su.SALES SET TARGET_DATE='{0}',FLD163='{1}',STATUS='{2}', FLD103='{3}',MEMO='{4}' WHERE" &
                                                                  " IDSALE={5}", tdate, hour, status, Capacity, memo, idsale)
                        'Update the Database
                        clsDataLayer.executeCommand(updateQuery)
                        ltrError.Text = "Database updated by " '+ updateQuery

                        'Inject a Script to Alert them that the database has been changed the redirect to all days traffic page
                        Dim script As String = String.Format("alert('The Bus Has Been Changed as Requested. \n {0}.\n You are now to be redirected to the previous page.');history.back();history.back();", memo)

                        Dim csType As Type = Me.[GetType]()
                        Dim cs As ClientScriptManager = Page.ClientScript
                        cs.RegisterClientScriptBlock(csType, "update", script, True)
                        'Redirect the user now.
                        Response.Redirect("/All-Days--Planned-Traffic.aspx")
                    Else

                    End If
                Catch sqlEx As SqlException
                    ltrError.Text = "Bus Could Not Be Updated Due to the Following Error: " + sqlEx.Message.ToString
                Catch ex As Exception
                    ltrError.Text = "Bus Could Not Be Updated Due to the Following Error: " + ex.Message
                End Try

            Catch ex As Exception
                'Probably a Problem with Request Headers
                ltrError.Text = ex.Message
            End Try

        Else
            'Page is not on Postback to display the name of this particular sales item
            Try
                Dim busname As String = clsDataLayer.ReturnSingleValue(String.Format("SELECT NAME FROM su.SALES WHERE IDSALE = {0}", idsale))
                If busname IsNot Nothing Then
                    ltrBusName.Text = busname
                Else
                    ltrBusName.Text = "Could Not Retreive Bus Name"
                End If

                'Now Retreive Other Variables and fix them correctly
                Dim queryDetails As String = String.Format("SELECT CONVERT(VARCHAR(14),TARGET_DATE,102) AS 'TARGET_DATE',FLD163,Status,FLD103,Memo FROM su.SALES WHERE IDSALE={0}", idsale)
                Dim dtDetails As DataTable = clsDataLayer.ReturnDataTable(queryDetails)

                With dtDetails.Rows(0)
                    txtDate.Text = .Item("TARGET_DATE").ToString
                    txtHour.Text = .Item("FLD163").ToString
                    txtCapacity.Text = .Item("FLD103").ToString
                    ddlStatus.SelectedValue = .Item("Status").ToString
                    txtMemo.Text = .Item("Memo").ToString
                End With
            Catch sqlEx As SqlException
                'in case we're in trouble
                ltrError.Text += "Error Encountered! " + sqlEx.Message
                clsDataLayer.LogException(sqlEx)
            Catch ex As Exception
                ltrError.Text += "Error Encountered! " + ex.Message
                clsDataLayer.LogException(ex)
            End Try
        End If
        'Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try

        Page.Title = "Edit Bus Information |Sinnovys Portal"
    End Sub
End Class
