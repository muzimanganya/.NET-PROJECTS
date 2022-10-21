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
                Dim plateno As String = Request("ctl00$ctl00$body$content$txtPlateNo").ToString
                Dim driver As String = Request("ctl00$ctl00$body$content$txtDriver").ToString
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

                    'First Check that the City IN / out Product Exists
                    Dim trajet As String = CityIn.Trim() + "-" + CityOut.Trim()
                    Dim idproduct As Double = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT IDPRODUCT FROM su.PRODUCTS WHERE NAME='{0}'", trajet))
                    Dim currentUser As String = HttpContext.Current.User.Identity.Name.ToUpper()
                    If idproduct > 0 Then
                        'Get the Maximum IDSALE
                        Dim idsale As Long = clsDataLayer.ReturnSingleNumeric("SELECT MAX(IDSALE)+1 FROM su.SALES")
                        'Create the Query to Insert
                        Dim memo As String = String.Format("Trajet Added By {0}", currentUser)
                        Dim queryInsert As String = String.Format("INSERT INTO su.SALES(IDSALE,FLD165,FLD164,TARGET_DATE,FLD103,FLD163,Memo, PLATENO, DriverName)" &
                                                                  " VALUES({0},{1},'{2}','{3}','{4}','{5}','{6}', '{7}', '{8}')", idsale, idproduct, trajet, tdate, Capacity, hour, memo, plateno, driver)
                        'Execute the Query
                        clsDataLayer.executeCommand(queryInsert)

                        ltrError.Text = "Trajet Has Been Inserted."
                        ddlCityIN.SelectedIndex = -1
                        ddlCityOut.SelectedIndex = -1
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
            Call LoadParentStops()

        End If
        ' Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '   Response.Redirect("/CustomError.aspx")
        'End Try

        Page.Title = "Create A New Bus Route"
    End Sub

    Sub LoadParentStops()
        'ddlCityIN
        Dim clsDatalayer As New Datalayer
        Dim dataReader As SqlDataReader = Nothing
        Dim isSelected As Boolean = True

        Try
            Using aCon As SqlConnection = clsDatalayer.ReturnConnection()
                Dim cmd As New SqlCommand("SELECT * FROM ParentStops", aCon)
                dataReader = cmd.ExecuteReader()
                Do While dataReader.Read()
                    Dim name As String = dataReader(0)
                    'add to city in and out
                    Dim newItem As New ListItem(name, name)

                    If isSelected Then
                        isSelected = False ' do it once only
                        newItem.Selected = True
                    End If

                    ddlCityIN.Items.Add(newItem)
                    ddlCityOut.Items.Add(newItem)
                Loop

                dataReader.Close()
                aCon.Close()
            End Using

        Catch ex As Exception

        End Try
       
    End Sub

End Class
