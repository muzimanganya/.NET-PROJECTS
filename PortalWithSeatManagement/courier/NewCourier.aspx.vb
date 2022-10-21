Imports System.Data.SqlClient

Partial Class courier_NewCourier
    Inherits System.Web.UI.Page

    Dim clsDatalayer As New Datalayer()

    Protected Sub ddlType_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim selectedType As String = ddlType.SelectedValue

        If (selectedType = "Luggage") Then

            pnlLuggage.Visible = True

        Else

            pnlLuggage.Visible = False

        End If

        Threading.Thread.Sleep(2500)

    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If (Not Page.IsPostBack) Then
            Dim mCities As String = "--Select City--,KIGALI,GATUNA,KAYONZA,KABARORE,KARANGAZI,RYABEGA,RWIMIYAGA,KAGITUMBA,GAFUNZO,KABARE,MBARARA,KAMPALA,KAMPALA2,GOMA,MUSANZE,CYANIKA,GISORO,BUTARE,BUKAVU"

            Dim Cities As String() = mCities.Split(New Char() {","c})

            For Each city As String In Cities
                Dim item As ListItem = New ListItem(city, city)

                ddlCityIn.Items.Add(item)
                ddlCityOut.Items.Add(item)
            Next

            ddlCityIn.SelectedValue = "--Select City--"
            ddlCityOut.SelectedValue = "--Select City--"
        End If



    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        Dim senderID, SenderName, SenderPhone, ReceiverName, ReceiverPhone, CityIn, CityOut, LuggageType, Weight, PackageType, Width, Depth, Length, Creator As String

        senderID = txtSenderID.Text
        SenderName = txtSenderName.Text
        SenderPhone = txtSenderPhone.Text
        ReceiverName = txtReceiverName.Text
        ReceiverPhone = txtReceiverPhone.Text
        CityIn = ddlCityIn.SelectedValue
        CityOut = ddlCityOut.SelectedValue
        LuggageType = ddlLuggageType.SelectedValue
        PackageType = ddlType.SelectedValue
        Weight = txtWeight.Text
        Depth = txtHeight.Text
        Length = txtLength.Text
        Width = txtWidth.Text
        Creator = "TrinityUser"

        Using con As SqlConnection = clsDatalayer.ReturnConnection()

            Using cmd As SqlCommand = New SqlCommand()

                cmd.Connection = con
                cmd.CommandType = Data.CommandType.StoredProcedure
                cmd.CommandText = "sp_AddCourier"
                cmd.Parameters.AddWithValue("@SenderID", senderID)
                cmd.Parameters.AddWithValue("@SenderName", SenderName)
                cmd.Parameters.AddWithValue("@SenderPhone", SenderPhone)
                cmd.Parameters.AddWithValue("@ReceiverName", ReceiverName)
                cmd.Parameters.AddWithValue("@ReceiverPhone", ReceiverPhone)
                cmd.Parameters.AddWithValue("@CityIn", CityIn)
                cmd.Parameters.AddWithValue("@CityOut", CityOut)
                cmd.Parameters.AddWithValue("@LuggageType", LuggageType)
                cmd.Parameters.AddWithValue("@PackageType", PackageType)
                cmd.Parameters.AddWithValue("@Weight", Weight)
                cmd.Parameters.AddWithValue("@Length", Length)
                cmd.Parameters.AddWithValue("@Height", Depth)
                cmd.Parameters.AddWithValue("@Width", Width)
                cmd.Parameters.AddWithValue("@Creator", Creator)

                Using reader As SqlDataReader = cmd.ExecuteReader()
                    While (reader.Read())
                        Dim courierID As String = reader(0).ToString()

                        If (courierID.Length > 0) Then

                            'The courier has been added successfully
                            Dim redirectUrl As String = String.Format("/courier/CourierDetails.aspx?CourierID={0}", courierID)
                            ltrError.Text = String.Format("Courier Successfully Added. Redirecting you to the details page {0}", redirectUrl)

                            Dim com As New Communication()

                            Dim message As String = String.Format("Hello. Courier received at {0} heading to {1}. Tracking Number {2}.", CityIn, CityOut, courierID)

                            com.SendSMS(message, SenderPhone)
                            com.SendSMS(message, ReceiverPhone)

                            Threading.Thread.Sleep(2000)

                            Response.Redirect(redirectUrl)
                        Else
                            ltrError.Text = String.Format("We encountered an error trying to add the courier")

                        End If

                    End While
                End Using

            End Using



        End Using




    End Sub
End Class
