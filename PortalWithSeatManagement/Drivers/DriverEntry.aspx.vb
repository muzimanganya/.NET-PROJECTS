Imports System.Data.SqlClient
Imports System.Data
Imports System.IO

Partial Class DriverEntry
    Inherits System.Web.UI.Page
    ' To keep track of the previous row Group Identifier
    Private strPreviousRowID As String = String.Empty
    ' To keep track the Index of Group Total
    Private intSubTotalIndex As Integer = 1

    ' To temporarily store Sub Total
    Private dblSubTotalQuantity As Double = 0
    Private dblSubTotalDiscount As Double = 0
    Private dblSubTotalAmount As Double = 0

    ' To temporarily store Grand Total
    Private dblGrandTotalQuantity As Double = 0
    Private dblGrandTotalDiscount As Double = 0
    Private dblGrandTotalAmount As Double = 0
    Dim clsDataLayer As New Datalayer
    Private Shared updating As Boolean
    Private Shared DriverId As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            If Page.IsPostBack Then
                Dim DriverName As String, Phone As String, Address As String, Email As String, Photo As String

                DriverName = txtName.Text
                Phone = txtPhone.Text
                Address = txtAdd.Text
                Email = txtEmail.Text

                If fpProfilePic.HasFile Then
                    Dim newFileName As String = ""
                    Photo = Guid.NewGuid().ToString() + "_" + fpProfilePic.FileName
                    newFileName = RandomFileName() + Photo
                    Dim image As System.Drawing.Image = System.Drawing.Image.FromStream(fpProfilePic.FileContent)
                    Dim FormatType As String = String.Empty
                    If image.RawFormat.Guid = System.Drawing.Imaging.ImageFormat.Tiff.Guid Then
                        FormatType = "TIFF"
                    ElseIf image.RawFormat.Guid = System.Drawing.Imaging.ImageFormat.Gif.Guid Then
                        FormatType = "GIF"
                    ElseIf image.RawFormat.Guid = System.Drawing.Imaging.ImageFormat.Jpeg.Guid Then
                        FormatType = "JPG"
                    ElseIf image.RawFormat.Guid = System.Drawing.Imaging.ImageFormat.Bmp.Guid Then
                        FormatType = "BMP"
                    ElseIf image.RawFormat.Guid = System.Drawing.Imaging.ImageFormat.Png.Guid Then
                        FormatType = "PNG"
                    ElseIf image.RawFormat.Guid = System.Drawing.Imaging.ImageFormat.Icon.Guid Then
                        FormatType = "ICO"
                    Else
                        'The File Format
                        errorMessage.Text = "For the Image, Please Select Only Image Files. Other Filetypes are not allowed"
                        Return
                    End If


                    fpProfilePic.SaveAs(newFileName)
                Else
                    Photo = "default.png"
                End If


                'Insert all this information into the db
                Dim query As String = ""
                If updating Then
                    'We use update command
                    Dim memo As String = ""
                    memo = clsDataLayer.ReturnSingleValue(String.Format("SELECT memo FROM dbo.Drivers WHERE DriverID={0}", DriverId))

                    query = String.Format("UPDATE dbo.Drivers SET DriverName='{0}',Phone='{1}',Address='{2}', Email='{3}',Photo='{4}',Memo='{5}' WHERE" &
                                                                      " DriverID={6}", DriverName, Phone, Address, Email, Photo, memo, DriverId)
                    updating = False
                Else
                    'We use the insert command
                    Dim currentUser As String = HttpContext.Current.User.Identity.Name.ToUpper()
                    Dim memo As String = String.Format("Driver Added By {0}", currentUser)
                    query = String.Format("INSERT INTO dbo.Drivers(DriverName,Phone,Address,Email,Photo,Memo)" &
                                                                      " VALUES('{0}','{1}','{2}','{3}','{4}','{5}')", DriverName, Phone, Address, Email, Photo, memo)
                End If

                'Execute the query
                If query <> String.Empty Then
                    clsDataLayer.executeCommand(query)
                    errorMessage.Text = "Successfully Updated the Driver"
                Else
                    errorMessage.Text = "Encountered An Error. Please Try Again"
                End If
            Else
                'Page is not Posting Back, So we Check if it is a new Insert or an Exsisting Query
                Try
                    DriverId = Request("DriverID")
                Catch ex As Exception
                    DriverId = ""
                End Try

                If Not DriverId > 0 Then
                    'New Query
                    updating = False
                    btnSend.Text = "Insert New Driver!"
                Else
                    'Exsisting & Load Everyone Information
                    updating = True
                    Dim queryInfo = String.Format("SELECT * FROM dbo.Drivers WHERE DriverID={0}", DriverId)
                    Dim dt = clsDataLayer.ReturnDataTable(queryInfo)

                    If dt.Rows.Count > 0 Then
                        With dt
                            txtName.Text = .Rows(0)("DriverName")
                            txtAdd.Text = .Rows(0)("Address")
                            txtEmail.Text = .Rows(0)("Email")
                            txtPhone.Text = .Rows(0)("Phone")

                            Try
                                If .Rows(0)("Photo").Length > 0 Then
                                    imgProfilePic.ImageUrl = "image.aspx?photo=" & .Rows(0)("Photo")
                                Else
                                    imgProfilePic.ImageUrl = "~/Styles/default.png"
                                End If

                            Catch ex As Exception

                            End Try
                            'imgProfilePic.ImageUrl = 
                        End With
                        'Continue to Get the Chart & Plot it
                        'First Get the Bus which he Drove this Week

                        Dim queryChart As String = String.Format("SET DATEFIRST 1;SELECT NAME,DATENAME(WEEKDAY,TARGET_DATE) AS 'Weekday' FROM su.SALES WHERE DATEPART(wk,TARGET_DATE) = DATEPART(wk,GETDATE()) AND YEAR(TARGET_DATE) = YEAR(GETDATE()) AND FLD167='{0}' AND TARGET_DATE <= GETDATE() GROUP BY DATENAME(WEEKDAY,TARGET_DATE),NAME", DriverId)

                        'Get Unique Rows in the Data Table
                        Dim dtRoutes = clsDataLayer.ReturnDataTable(queryChart)
                        Dim dv As DataView = New DataView(dtRoutes)
                        Dim distinctRoutes = dv.ToTable(True, "NAME")

                        'Get the Durations of each route and add to a collection
                        Dim durations = New Dictionary(Of String, Integer)

                        For Each rw As DataRow In distinctRoutes.Rows
                            Dim cityIn As String = rw(0).ToString().Split(New Char() {"-"c}, 2)(0)
                            Dim cityOut As String = rw(0).ToString().Split(New Char() {"-"}, 2)(1).Split(New Char() {" "c}, 2)(0)

                            'Now Get the Stops

                            Dim stopIN As Integer = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT StopNo FRom dbo.Stops WHERE StopName='{0}'", cityIn))
                            Dim stopOut As Integer = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT StopNo FRom dbo.Stops WHERE StopName='{0}'", cityOut))

                            'Now Get the Durations

                            Dim TravelDuration = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT Duration FROM dbo.MaxSeatsPerStopMaster WHERE StopIn={0} AND StopOut={1}", stopIN, stopOut))

                            'Now Associate Distinct Route with The Duration

                            durations.Add(rw(0), TravelDuration)
                            'Now we have distinct durations lets get back to the main data set and iterate through
                        Next

                        Dim charsToTrim() As Char = {","c}
                        Dim json = "[['Day of Week','Total Hours Logged'],"
                        Dim jsonF As String
                        Dim processTracking As New Dictionary(Of String, Integer)
                        For Each rw As DataRow In dtRoutes.Rows
                            Dim currentWeekday = rw("Weekday")
                            If Not processTracking.ContainsKey(currentWeekday) Then
                                'We have computed everything for this day so let us proceed to the next
                                Dim totalDuration As Integer = 0
                                'Get the Routes for this day and add up the totals
                                Dim currentDt = dtRoutes.Select(String.Format("WEEkDAY='{0}'", currentWeekday))

                                For Each newRw As DataRow In currentDt
                                    Dim tmpDuration As Integer = 0
                                    tmpDuration = durations(newRw(0))
                                    totalDuration += tmpDuration
                                Next

                                'I have the total Duration For A Certain Day
                                json += String.Format("['{0}',{1}],", currentWeekday, totalDuration / 60)

                                processTracking.Add(currentWeekday, 1)
                            Else
                            End If

                        Next


                        jsonF = json.TrimEnd(charsToTrim)
                        jsonF += "]"

                        Dim mys As StringBuilder = New StringBuilder()
                        mys.AppendLine("function drawAreaChart() {")
                        mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
                        mys.AppendLine("var options = {title:  'Driver Weekly Report',hAxis: { title: 'Day of the Week', titleTextStyle: { color: 'red'} }};")
                        mys.AppendLine("var chart = new google.visualization.AreaChart(document.getElementById('chart_div'));")
                        mys.AppendLine("chart.draw(data, options);")
                        mys.AppendLine("}")

                        Dim chartScript = mys.ToString

                        Dim initScript As String = "function initCharts () {drawAreaChart();document.getElementById('dateController').style.display = 'block'}" +
                                                "google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(initCharts);"

                        Dim finalScript = initScript + chartScript
                        Dim csType As Type = Me.[GetType]()
                        Dim cs As ClientScriptManager = Page.ClientScript
                        cs.RegisterClientScriptBlock(csType, "charts", finalScript, True)

                        'Success all the way now let us add the grid below to show all the buses he has driven
                        Try
                            Dim query = String.Format("SELECT IDSALE,FLD164 AS 'Route', NAME,TARGET_DATE, FLD163 AS 'Hour', FLD103 AS 'Capacity',FLD166 AS 'Booked'," &
                                        "STATUS,BUDGET,FLD104 AS 'Vehicle',''AS 'Mail',SUBSTRING(MEMO,0,20)+'..' AS 'Memo',FLD167 AS 'Driver' " &
                                        "FROM su.SALES WHERE FLD167='{0}' ORDER BY TARGET_DATE DESC", DriverId)

                            Dim dtHistory = clsDataLayer.ReturnDataTable(query)
                            gridViewBuses.DataSource = dtHistory
                            gridViewBuses.DataBind()


                        Catch ex As Exception
                            clsDataLayer.LogException(ex)
                        End Try
                        'Set up the Week Selection

                        'ceSelectDate.EndDate = DateTime.Now.Add(New TimeSpan(24, 0, 0))
                        'ceSelectDate.EndDate = DateTime.Now
                    Else
                        errorMessage.Text = "Driver With that ID Not Found"
                    End If

                End If
            End If
        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try

        Page.Title = "Driver Information"
    End Sub

    Protected Sub gridViewBuses_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) 'Handles gridViewBuses.RowCreated
        Try
            Dim IsSubTotalRowNeedToAdd As Boolean = False
            Dim IsGrandTotalRowNeedtoAdd As Boolean = False

            If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "Route") IsNot Nothing) Then
                If strPreviousRowID <> DataBinder.Eval(e.Row.DataItem, "Route").ToString() Then
                    IsSubTotalRowNeedToAdd = True
                End If
            End If

            If (strPreviousRowID <> String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "Route") Is Nothing) Then
                IsSubTotalRowNeedToAdd = True
                IsGrandTotalRowNeedtoAdd = True
                intSubTotalIndex = 0
            End If

            '#Region "Inserting first Row and populating fist Group Header details"
            If (strPreviousRowID = String.Empty) AndAlso (DataBinder.Eval(e.Row.DataItem, "Route") IsNot Nothing) Then
                Dim grdViewProducts As GridView = DirectCast(sender, GridView)

                Dim row As New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)

                Dim cell As New TableCell()
                cell.Text = "<div style='width:100%;'>" & "<div style='width:50%;float:left;'>" & "<b style='float:left;'>Bus Route Sales Opportunities :  </b>" & "<div style='float:left;'>" & DataBinder.Eval(e.Row.DataItem, "Route").ToString() & "</br></div>" & "</div>" & "<div style='width:50%;float:left;'></div>" & "</div>"
                cell.ColumnSpan = 10
                cell.CssClass = "GroupDetailsStyle"
                row.Cells.Add(cell)

                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                intSubTotalIndex += 1

                '#Region "Add Group Header"
                ' Creating a Row
                row = New GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert)



                cell = New TableCell()
                cell.Text = "Bus Name"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Date"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Hour"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Vehicle"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Driver"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Max Capacity"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Tickets"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Messages"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)


                cell = New TableCell()
                cell.Text = "Status"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)

                cell = New TableCell()
                cell.Text = "Amount"
                cell.CssClass = "GroupHeaderStyle"
                row.Cells.Add(cell)


                'Adding the Row at the RowIndex position in the Grid
                grdViewProducts.Controls(0).Controls.AddAt(e.Row.RowIndex + intSubTotalIndex, row)
                '#End Region
                intSubTotalIndex += 1
            End If
            '#End Region

            
        Catch ex As Exception
            clsDataLayer.LogException(ex)
        End Try

    End Sub

    Protected Sub gridViewBuses_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) 'Handles gridViewBuses.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "Route").ToString()

            End If
        Catch ex As Exception
            clsDataLayer.LogException(ex)
        End Try

    End Sub

    Private Function RandomFileName() As String
        Dim cfg As New Config
        Return cfg.WritePath
    End Function

    'Protected Sub btnGo_Click(sender As Object, e As EventArgs) Handles btnGo.Click
    '    'Now Load the Driver's Report Again
    '    Dim currentText = txtDate.Text
    '    'Continue to Get the Chart & Plot it
    '    'First Get the Bus which he Drove this Week
    '    'Try
    '    ' Dim tmpDate = New DateTime(currentText)
    '    'No Error So we Can Continue
    '    Dim queryChart As String = String.Format("SET DATEFIRST 1;SELECT NAME,DATENAME(WEEKDAY,TARGET_DATE) AS 'Weekday' FROM su.SALES WHERE DATEPART(wk,TARGET_DATE) = DATEPART(wk,(CONVERT(DATETIME,'{1}'))) AND YEAR(TARGET_DATE) = YEAR(CONVERT(DATETIME,'{1}')) AND FLD167='{0}' GROUP BY DATENAME(WEEKDAY,TARGET_DATE),NAME", DriverId, currentText)

    '    'Get Unique Rows in the Data Table
    '    Dim dtRoutes = clsDataLayer.ReturnDataTable(queryChart)
    '    Dim dv As DataView = New DataView(dtRoutes)
    '    Dim distinctRoutes = dv.ToTable(True, "NAME")

    '    'Get the Durations of each route and add to a collection
    '    Dim durations = New Dictionary(Of String, Integer)

    '    For Each rw As DataRow In distinctRoutes.Rows
    '        Dim cityIn As String = rw(0).ToString().Split(New Char() {"-"c}, 2)(0)
    '        Dim cityOut As String = rw(0).ToString().Split(New Char() {"-"}, 2)(1).Split(New Char() {" "c}, 2)(0)

    '        'Now Get the Stops

    '        Dim stopIN As Integer = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT StopNo FRom dbo.Stops WHERE StopName='{0}'", cityIn))
    '        Dim stopOut As Integer = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT StopNo FRom dbo.Stops WHERE StopName='{0}'", cityOut))

    '        'Now Get the Durations

    '        Dim TravelDuration = clsDataLayer.ReturnSingleNumeric(String.Format("SELECT Duration FROM dbo.MaxSeatsPerStopMaster WHERE StopIn={0} AND StopOut={1}", stopIN, stopOut))

    '        'Now Associate Distinct Route with The Duration

    '        durations.Add(rw(0), TravelDuration)
    '        'Now we have distinct durations lets get back to the main data set and iterate through
    '    Next

    '    Dim charsToTrim() As Char = {","c}
    '    Dim json = "[['Day of Week','Total Hours Logged'],"
    '    Dim jsonF As String
    '    Dim processTracking As New Dictionary(Of String, Integer)
    '    For Each rw As DataRow In dtRoutes.Rows
    '        Dim currentWeekday = rw("Weekday")
    '        If Not processTracking.ContainsKey(currentWeekday) Then
    '            'We have computed everything for this day so let us proceed to the next
    '            Dim totalDuration As Integer = 0
    '            'Get the Routes for this day and add up the totals
    '            Dim currentDt = dtRoutes.Select(String.Format("WEEkDAY='{0}'", currentWeekday))

    '            For Each newRw As DataRow In currentDt
    '                Dim tmpDuration As Integer = 0
    '                tmpDuration = durations(newRw(0))
    '                totalDuration += tmpDuration
    '            Next

    '            'I have the total Duration For A Certain Day
    '            json += String.Format("['{0}',{1}],", currentWeekday, totalDuration / 60)

    '            processTracking.Add(currentWeekday, 1)
    '        Else
    '        End If

    '    Next


    '    jsonF = json.TrimEnd(charsToTrim)
    '    jsonF += "]"

    '    Dim mys As StringBuilder = New StringBuilder()
    '    mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
    '    mys.AppendLine("var options = {title:  'Driver Weekly Report',hAxis: { title: 'Day of the Week', titleTextStyle: { color: 'red'} }};")
    '    mys.AppendLine("var chart = new google.visualization.AreaChart(document.getElementById('chart_div'));")
    '    mys.AppendLine("chart.draw(data, options);")


    '    Dim chartScript = mys.ToString

    '    Dim finalScript As String = "google.load(""visualization"", ""1"", { packages: [""corechart""] });" &
    '        "google.setOnLoadCallback(" + "chartScript" + "document.getElementById('dateController').style.display = 'block';);"

    '    Dim csType As Type = Me.[GetType]()
    '    Dim cs As ClientScriptManager = Page.ClientScript
    '    cs.RegisterClientScriptBlock(csType, "charts", finalScript, True)
    '    'Catch ex As Exception

    '    'End Try

    'End Sub
End Class
