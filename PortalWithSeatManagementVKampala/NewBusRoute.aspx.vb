Imports System.Data.SqlClient
Imports System.Data

Partial Class NewBusRoute
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try
        If Page.IsPostBack Then
            Try
                Dim CityIn As String = Request("ctl00$ctl00$body$content$ddlCityIN").ToString().ToUpper()
                Dim CityOut As String = Request("ctl00$ctl00$body$content$ddlCityOut").ToString().ToUpper()
                Dim tdate As String = Request("ctl00$ctl00$body$content$txtDate").ToString()
                Dim Capacity As String = Request("ctl00$ctl00$body$content$txtCapacity").ToString()
                Dim hour As String = Request("ctl00$ctl00$body$content$txtHour").ToString
                Dim prefSeats As String = Request("ctl00$ctl00$body$content$txtPSeats").ToString

                Dim Driver As String = ddlDriver.SelectedValue
                Dim Vehicle As String = ddlVehicle.SelectedValue

                Try
                    'Do initial value checks
                    If (Convert.ToInt16(Capacity)) Then
                        'The Capacity is an integer
                        If Not (Capacity > 0 And Capacity < 100) Then
                            Throw New Exception("Capacity Must Be Between 1 & 100 you entered " + Capacity + ". Please check and try again")
                        End If
                    Else
                        Throw New Exception("Please Enter a Valid Bus Capacity")
                    End If

                    'Check that the PrefSeats are well done

                    If prefSeats.Length > 0 Then
                        'They sent us some prefseats
                        Dim theSeats As String() = prefSeats.Split(New Char() {","c})
                        For Each s As String In theSeats
                            If Not Regex.IsMatch(s, "^[0-9 ]+$") Then
                                Throw New Exception("Error: Adding Failed: Seat Number Must Be A Number And The Seats Separated By A Comma. e.g. 5,8,10,12")
                            Else
                                'Check the number is not greater than the capacity of the bus
                                Dim sno As Integer = Convert.ToInt16(s)
                                If sno > Capacity Or sno < 1 Then
                                    Throw New Exception("Error: Adding Failed: The Seat Numbers Cannot Be Bigger than the Capacity of the Bus. The Bus Capacity is " + Capacity + " so the Maximum Preferred Seat Number is " + Capacity)
                                End If
                            End If
                        Next
                    Else
                        prefSeats = "0"
                    End If

                    'First Check that the City IN / out Product Exists
                    Dim trajet As String = CityIn.Trim() + "-" + CityOut.Trim()
                    Dim idproduct As Double = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT IDPRODUCT FROM su.PRODUCTS WHERE NAME='{0}'", trajet))
                    Dim currentUser As String = HttpContext.Current.User.Identity.Name.ToUpper()
                    If idproduct > 0 Then
                        'Get the Maximum IDSALE
                        Dim idsale As Long = clsDataLayer.ReturnSingleNumeric("SELECT MAX(IDSALE)+1 FROM su.SALES")
                        'Create the Query to Insert
                        Dim memo As String = String.Format("Trajet Added By {0}", currentUser)
                        Dim queryInsert As String = String.Format("INSERT INTO su.SALES(IDSALE,FLD165,FLD164,TARGET_DATE,FLD103,FLD163,Memo,PSEATS,FLD104,FLD167)" &
                                                                  " VALUES({0},{1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')", idsale, idproduct, trajet, tdate, Capacity, hour, memo, prefSeats, Vehicle, Driver)
                        'Execute the Query
                        clsDataLayer.executeCommand(queryInsert)

                        ltrError.Text = "Trajet Has Been Inserted."
                        ddlCityIN.SelectedIndex = -1
                        ddlCityOut.SelectedIndex = -1
                        ddlDriver.SelectedIndex = -1
                        ddlVehicle.SelectedIndex = -1
                        txtPSeats.Text = ""
                        txtCapacity.Text = ""
                        txtDate.Text = ""
                        txtHour.Text = ""
                    Else
                        Throw New Exception("There is no recorded product for " + CityIn + "-" + CityOut + " in the database. Please contact your administrator.")
                    End If
                Catch sqlEx As SqlException
                    ltrError.Text = sqlEx.Message.ToString
                Catch ex As Exception
                    ltrError.Text = ex.Message
                End Try

            Catch ex As Exception
                'Probably a Problem with Request Headers
                ltrError.Text = ex.Message
                clsDataLayer.LogException(ex)
            End Try

        Else
            'Attempt to Load Vehicles & Drivers
            Try
                Dim queryVehicles = "SELECT VehicleNumber,VehicleName FROM dbo.Vehicles"
                Dim queryDrivers = "SELECT DriverName,DriverID FROM dbo.Drivers"

                Dim dtV = clsDataLayer.ReturnDataTable(queryVehicles)
                Dim dtD = clsDataLayer.ReturnDataTable(queryDrivers)

                ddlVehicle.Items.Add(New ListItem("Select Vehicle", "0"))
                ddlDriver.Items.Add(New ListItem("Select Driver", "0"))

                For Each rw As DataRow In dtV.Rows
                    ddlVehicle.Items.Add(New ListItem(rw("VehicleName"), rw("VehicleNumber")))
                Next

                For Each rw As DataRow In dtD.Rows
                    ddlDriver.Items.Add(New ListItem(rw("DriverName"), rw("DriverID")))
                Next
            Catch ex As Exception

            End Try
        End If
        ' Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '   Response.Redirect("/CustomError.aspx")
        'End Try

        Page.Title = "Create A New Bus Route"
    End Sub
End Class
