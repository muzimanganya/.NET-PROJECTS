Imports System.Data.SqlClient
Imports System.Data

Partial Class EditRequests
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Public RequestType As String
    Public busID As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Try

        Dim requestid As String = ""
        Try
            requestid = Request("RequestID")
        Catch ex As Exception

        End Try
        If requestid IsNot String.Empty Then
            Dim queryRequestedSeats As String = String.Format("SELECT SeatsNo from dbo.BusShareRequests where RequestID={0}", requestid)
            Dim seatsRequested = clsDataLayer.ReturnSingleNumeric(queryRequestedSeats)
            If seatsRequested > 0 Then
                txtSeats.Text = seatsRequested
                rvValidate.MaximumValue = seatsRequested
                rvValidate.MinimumValue = 1
            Else
                ltrErrorMessage.Text = "Unable to get the requested Seats"
            End If
            busID = clsDataLayer.ReturnSingleValue(String.Format("SELECT BusID from dbo.BusShareRequests where RequestID={0}", requestid))

            Dim queryTitle = String.Format("SELECT NAME FROM su.SALES WHERE IDSALE={0}", busID)
            ltrTitle.Text = clsDataLayer.ReturnSingleValue(queryTitle)

        End If
        'Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try

        Page.Title = "Edit Seats Request"
    End Sub


    Protected Sub btnAccept_Click(sender As Object, e As EventArgs) Handles btnAccept.Click
        'They have chosen to accept the request so make sure the seats are available then subtract capacity from one and add to the other, Add that process in the memo description
        Dim acceptedSeats As Integer = txtSeats.Text
        Dim queryRemaining As String = String.Format("SELECT s.FLD103 - (SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE IDSALE={0}) AS 'RemainingSeats' FROm su.SALES as s WHERE s.IDSALE={0}", busID)

        Dim remaining = clsDataLayer.ReturnSingleNumeric(queryRemaining, "KigaliSafaris")

        If remaining < acceptedSeats Then
            ltrErrorMessage.Text += "<br/> Cannot Exchange Seats as they are already occupied. Only " + remaining + " Seats Remaining"
            Return
        Else
            Dim swapSeats As SqlTransaction, bSwapSeats As SqlTransaction
            Using aCon As SqlConnection = clsDataLayer.ReturnConnection("KigaliSafaris")
                swapSeats = aCon.BeginTransaction()
                Try
                    Dim currentCapacity = clsDataLayer.ReturnSingleNumeric("SELECT FLD103 FROm su.SALES WHERE IDSALE=" + busID, "KigaliSafaris")
                    Dim newCapacity As Integer = currentCapacity - acceptedSeats
                    Dim querySubtractSeats As String = String.Format("UPDATE su.SALES SET FLD103={0} WHERE IDSALE={1}", newCapacity, busID)
                    Using bCon As SqlConnection = clsDataLayer.ReturnConnection()
                        'Now Paste the Info in Belvedere
                        bSwapSeats = bCon.BeginTransaction()
                        Dim updateQuery As String = String.Format("UPDATE su.SALES SET FLD103=FLD103+{0} WHERE IDSALE={1}", acceptedSeats, busID)
                        Try
                            Dim kCmd As New SqlCommand(querySubtractSeats)
                            kCmd.CommandType = CommandType.Text
                            kCmd.Connection = aCon
                            kCmd.Transaction = swapSeats

                            kCmd.ExecuteNonQuery()

                            Dim bCmd As New SqlCommand(updateQuery)
                            bCmd.CommandType = CommandType.Text
                            bCmd.Connection = bCon
                            bCmd.Transaction = bSwapSeats

                            bCmd.ExecuteNonQuery()

                            swapSeats.Commit()
                            bSwapSeats.Commit()

                        Catch sex As SqlException
                            ltrErrorMessage.Text += "Could Not Swap Seats Due to the Following BSError: " + sex.Message
                            swapSeats.Rollback()
                            bSwapSeats.Rollback()
                        Catch ex As Exception
                            swapSeats.Rollback()
                            bSwapSeats.Rollback()
                            ltrErrorMessage.Text = "Could Not Swap Seats Due to the Following BEError: " + ex.Message + "<br/> Here is the Stack Trace <br/>" + ex.StackTrace
                        End Try
                    End Using
                Catch sex As SqlException
                    swapSeats.Rollback()
                    bSwapSeats.Rollback()
                    ltrErrorMessage.Text += "Could Not Swap Seats Due to the Following KError: " + sex.Message
                Catch ex As Exception
                    swapSeats.Rollback()
                    bSwapSeats.Rollback()
                    ltrErrorMessage.Text += "Could Not Swap Seats Due to the Following KEError: " + ex.Message
                End Try
            End Using
        End If
    End Sub
End Class
