

Imports System.Data.SqlClient
Imports System.Data

Partial Class Services
    Inherits System.Web.UI.Page
    Dim clsDataLayer As New Datalayer
    Private Shared QueryForCards As String
    Public Property CurrentPage() As Integer
        Get
            ' look for current page in ViewState
            Dim o As Object = Me.ViewState("_CurrentPage")
            If o Is Nothing Then
                Return 0
            Else
                ' default page index of 0
                Return CInt(o)
            End If
        End Get

        Set(value As Integer)
            Me.ViewState("_CurrentPage") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Try
        If Not Page.IsPostBack Then
            'Add the Nodes One By One Starting with the base one of Bus and Subscription
            Dim serviceNode As TreeNode = New TreeNode("Services", "services", "Styles/icon-16-banner-client.png")
            Dim busNode As TreeNode = New TreeNode("BUS SERVICE", "TRANSPORT", "Styles/icon-16-new.png")
            Dim subscription As TreeNode = New TreeNode("SUBSCRIPTION SERVICE", "subscription", "Styles/icon-16-new.png")

            'Now Let us Query the Database and Populate the Tree

            Dim queryTrees As String = "SELECT pp.IDPRODUCT,p.NAME AS 'PARENTNAME',pp.IDPRODUCT2,pTwo.NAME AS 'ChildName' FROM su.PROD_PROD AS pp " &
                                            "INNER JOIN su.PRODUCTS AS p ON pp.IDPRODUCT = p.IDPRODUCT " &
                                                "INNER JOIN su.PRODUCTS AS pTwo ON pp.IDPRODUCT2 = pTwo.IDPRODUCT " &
                                                    "ORDER BY 'PARENTNAME'"
            'Query For All Rows
            Dim serviceDt As DataTable = clsDataLayer.ReturnDataTable(queryTrees)
            'First Get those TO be added directly to the Bus Node
            Dim directBus() As DataRow = serviceDt.Select("PARENTNAME='TRANSPORT'")

            Dim i As Integer
            ' Print column 0 of each returned row. 
            For i = 0 To directBus.GetUpperBound(0)
                Dim busRoute As TreeNode = New TreeNode(directBus(i)("ChildName").ToString(), directBus(i)("ChildName").ToString(), "Styles/icon-16-newarticle.png")
                'Before leaving let us now add all the other child nodes for this dude
                Dim childRows() As DataRow = serviceDt.Select(String.Format("PARENTNAME='{0}'", directBus(i)("ChildName").ToString()))
                Dim j As Integer
                For j = 0 To childRows.GetUpperBound(0)
                    Dim childRoute As TreeNode = New TreeNode(childRows(j)("ChildName"), childRows(j)("ChildName"), "Styles/icon-16-user-note.png")
                    busRoute.ChildNodes.Add(childRoute)
                Next
                busRoute.Expanded = False
                busNode.ChildNodes.Add(busRoute)
            Next

            'Dim directSubs() As DataRow = serviceDt.Select("PARENTNAME='SUBSCRIPTION'")
            'For i = 0 To directSubs.GetUpperBound(0)
            '    Dim subs As TreeNode = New TreeNode(directSubs(i)("ChildName"), directSubs(i)("ChildName"), "Styles/icon-16-inbox.png")
            '    subscription.ChildNodes.Add(subs)
            'Next
            'Time to get the cards as well dear
            Dim queryCards = String.Format("SELECT IDPRODUCT, FLD139+'-'+FLD140 AS 'Route',FLD139+'-'+FLD140+' '+CONVERT(VARCHAR,QUANTITY)+' Trips' AS 'CardName'" &
                                                " FROM su.PRODUCTS WHERE Quantity > 1 And FLD139 Is Not NULL And FLD140 Is Not NULL")

            Dim dtCards = clsDataLayer.ReturnDataTable(queryCards)
            Dim routesTrack As New Dictionary(Of String, Integer)
            For Each rw As DataRow In dtCards.Rows
                Dim routeName As String = rw("Route").ToString.ToUpper()
                Dim routeID As Integer = rw("IDPRODUCT")
                If Not routesTrack.ContainsKey(routeName) Then
                    'Has not being worked on
                    Dim cardsRoute As New TreeNode(routeName, routeID, "Styles/icon-16-new.png")

                    subscription.ChildNodes.Add(cardsRoute)

                    'Now Process the Child Routes
                    Dim dtCardSiblings = dtCards.Select(String.Format(" Route='{0}'", routeName))
                    For Each rwSibling As DataRow In dtCardSiblings
                        'Add to the parent
                        Dim siblingName = rwSibling("CardName")
                        Dim siblingID = rwSibling("IDPRODUCT")
                        Dim sibling As New TreeNode(siblingName, siblingID, "Styles/icon-16-user-note.png")
                        cardsRoute.ChildNodes.Add(sibling)
                    Next

                    routesTrack.Add(routeName, routeID)
                End If
                'Process the Route Add Parent and Children, Make sure it has not being processed yet!

            Next
            subscription.CollapseAll()
            serviceNode.ChildNodes.Add(busNode)
            serviceNode.ChildNodes.Add(subscription)


            trvServices.Nodes.Add(serviceNode)

            pnlCards.Visible = False
            pnlCards.Enabled = False
        End If
        'Catch ex As Exception
        '    clsDataLayer.LogException(ex)
        '    Response.Redirect("/CustomError.aspx")
        'End Try


        Page.Title = "Services | Sinnovys Portal"
    End Sub

    Protected Sub trvServices_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles trvServices.SelectedNodeChanged
        Try
            'Draw the Appropriate Graphs After Selection
            Dim depthClicked = trvServices.SelectedNode.Depth
            Dim currentNodeValue = trvServices.SelectedValue
            'Let's find out at what depth we are operating and set up the stuff
            Dim busProcess As Integer = 0
            If depthClicked = 0 Or depthClicked = 1 Then
                busProcess = 0
            ElseIf depthClicked = 2 Then
                'Check the parent and decide
                If trvServices.SelectedNode.Parent.Value = "bus" Then
                    busProcess = 1 ' We are dealing with anormal route
                ElseIf trvServices.SelectedNode.Parent.Value = "subscription" Then
                    busProcess = 0
                Else
                    busProcess = 0
                End If
            ElseIf depthClicked = 3 Then
                'Check the parent's Parent and decide
                If trvServices.SelectedNode.Parent.Parent.Value = "bus" Then
                    busProcess = 1 ' We are dealing with anormal route
                ElseIf trvServices.SelectedNode.Parent.Parent.Value = "subscription" Then
                    busProcess = 2
                Else
                    busProcess = 0
                End If
            End If
            Page.Title = "Sinnovys | Monthly Trend For " + currentNodeValue
            If busProcess = 1 Then
                'Hide the cards information
                pnlCards.Visible = False
                pnlCards.Enabled = False
                'Show the others
                tbServices.Enabled = True
                tbServices.Visible = True
                ' WE are under transport services lets do the magic
                'Start with Travelers Per Month
                Dim queryTravelersForward = String.Format("SELECT TOP(6) COUNT(IDRELATION) AS 'Total', DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) AS 'MONTH'," &
                "CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) AS 'YearMonth'" &
                "FROM su.SALES_PROD WHERE FLD120+'-'+FLD122='{0}'" &
                "GROUP BY DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) ,CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102)" &
                "ORDER BY CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) DESC", currentNodeValue)

                Dim queryTravelersBack = String.Format("SELECT TOP(6) COUNT(IDRELATION) AS 'Total', DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) AS 'MONTH'," &
                "CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) AS 'YearMonth'" &
                "FROM su.SALES_PROD WHERE FLD122+'-'+FLD120='{0}'" &
                "GROUP BY DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) ,CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102)" &
                "ORDER BY CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) DESC", currentNodeValue)

                Dim json As String = String.Format("[['Month','{0}','{1}'],", currentNodeValue, ReverseString(currentNodeValue))

                Dim dtF As DataTable = clsDataLayer.ReturnDataTable(queryTravelersForward)
                Dim dtB As DataTable = clsDataLayer.ReturnDataTable(queryTravelersBack)

                dtF = ReverseRowsInDataTable(dtF)
                dtB = ReverseRowsInDataTable(dtB)

                Dim i As Integer

                For i = 0 To (dtF.Rows.Count - 1)
                    Dim forwardValue As Integer, backValue As Integer

                    If dtF.Rows.Count > 0 Then
                        forwardValue = If(dtF.Rows(i)("Total").ToString IsNot String.Empty, dtF.Rows(i)("Total").ToString, "0")
                    Else
                        forwardValue = 0
                    End If
                    If dtB.Rows.Count Then
                        backValue = If(dtB.Rows(i)("Total").ToString IsNot String.Empty, dtB.Rows(i)("Total").ToString, "0")
                    Else
                        backValue = 0
                    End If

                    If dtF.Rows.Count > 0 Then
                        json += String.Format("['{0}',{1},{2}],", dtF.Rows(i)("Month").ToString, forwardValue, backValue)
                    Else
                        json += String.Format("['{0}',{1},{2}],", " ", forwardValue, backValue)
                    End If


                Next

                Dim charsToTrim() As Char = {","c}
                Dim jsonF As String = json.TrimEnd(charsToTrim)
                jsonF += "]"

                Dim mys As StringBuilder = New StringBuilder()
                mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawChart);")
                mys.AppendLine("function drawChart() {")
                mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
                mys.AppendLine("var options = {title:  'Last 6 Monthly Travelers Trend','width':800,'height':280,legend: {position:'top'},hAxis: { title: 'Month', titleTextStyle: { color: 'red'} }};")
                mys.AppendLine("var chart = new google.visualization.LineChart(document.getElementById('tchart'));")
                mys.AppendLine("chart.draw(data, options);")
                mys.AppendLine("}")

                Dim script As String = mys.ToString()
                Dim csType As Type = Me.[GetType]()
                Dim cs As ClientScriptManager = Page.ClientScript
                cs.RegisterClientScriptBlock(csType, "pie", script, True)

                ' Now work on the Revenue Chart
                Dim queryRevenueF As String = String.Format("SELECT TOP(6) SUM(TOTAL) AS 'Total', DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) AS 'MONTH'," &
                "CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) AS 'YearMonth'" &
                "FROM su.SALES_PROD WHERE FLD120+'-'+FLD122='{0}'" &
                "GROUP BY DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) ,CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102)" &
                "ORDER BY CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) DESC", currentNodeValue)

                Dim queryRevenueB As String = String.Format("SELECT TOP(6) SUM(TOTAL) AS 'Total', DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) AS 'MONTH'," &
                "CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) AS 'YearMonth'" &
                "FROM su.SALES_PROD WHERE FLD122+'-'+FLD120='{0}'" &
                "GROUP BY DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) ,CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102)" &
                "ORDER BY CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) DESC", currentNodeValue)

                Dim dtRevenueF = clsDataLayer.ReturnDataTable(queryRevenueF)
                Dim dtRevenueB = clsDataLayer.ReturnDataTable(queryRevenueB)

                dtRevenueF = ReverseRowsInDataTable(dtRevenueF)
                dtRevenueB = ReverseRowsInDataTable(dtRevenueB)

                json = String.Format("[['Month','{0}','{1}'],", currentNodeValue, ReverseString(currentNodeValue))
                For i = 0 To (dtRevenueF.Rows.Count - 1)
                    Dim forwardValue As Integer, backValue As Integer

                    If dtRevenueF.Rows.Count > 0 Then
                        forwardValue = If(dtRevenueF.Rows(i)("Total").ToString IsNot String.Empty, dtRevenueF.Rows(i)("Total").ToString, "0")
                    Else
                        forwardValue = 0
                    End If
                    If dtRevenueB.Rows.Count Then
                        backValue = If(dtRevenueB.Rows(i)("Total").ToString IsNot String.Empty, dtRevenueB.Rows(i)("Total").ToString, "0")
                    Else
                        backValue = 0
                    End If

                    If dtRevenueF.Rows.Count > 0 Then
                        json += String.Format("['{0}',{1},{2}],", dtRevenueF.Rows(i)("Month").ToString, forwardValue, backValue)
                    Else
                        json += String.Format("['{0}',{1},{2}],", " ", forwardValue, backValue)
                    End If


                Next


                jsonF = json.TrimEnd(charsToTrim)
                jsonF += "]"

                mys = New StringBuilder()
                mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(StrawChart);")
                mys.AppendLine("function StrawChart() {")
                mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))


                mys.AppendLine("var options = {title:  'Monthly Revenue Trend','width':800,'height':350,hAxis: { title: 'Month', titleTextStyle: { color: 'red'} },seriesType:'bars',series: {5: {type: 'line'}}};")
                mys.AppendLine("var chart = new google.visualization.ComboChart(document.getElementById('rchart'));")
                mys.AppendLine("chart.draw(data, options);")
                mys.AppendLine("}")

                script = mys.ToString()
                csType = Me.[GetType]()
                cs.RegisterClientScriptBlock(csType, "rchart", script, True)

                ' Now Get the Sales Opportunities
                Dim queryPID = String.Format("SELECT IDPRODUCT FROM su.PRODUCTS WHERE NAME='{0}'", currentNodeValue)
                Dim pid = clsDataLayer.ReturnSingleNumeric(queryPID)
                Dim queryOpp As String = String.Format("SELECT IDSALE,FLD164 AS 'Route', NAME,CONVERT(VARCHAR(12),TARGET_DATE,103) AS 'TARGET_DATE'," &
                        " FLD163 AS 'Hour', FLD103 AS 'Capacity',FLD166 AS 'Booked',STATUS,BUDGET,'' AS 'Vehicle',''AS 'Mail','' AS 'Driver'" &
                                "FROM su.SALES WHERE DAY(TARGET_DATE) = DAY(GETDATE()) AND MONTH(TARGET_DATE) = MONTH(GETDATE()) AND YEAR(TARGET_DATE) = YEAR(GETDATE()) AND FLD165 = {0}" &
                                                "ORDER BY FLD164,FLD163", pid)
                Dim dtOpp As DataTable = clsDataLayer.ReturnDataTable(queryOpp)
                rptSalesOpportunities.DataSource = dtOpp
                rptSalesOpportunities.DataBind()
            ElseIf busProcess = 2 Then
                'Hide the top Tabs
                tbServices.Enabled = False
                tbServices.Visible = False
                'Show mine
                pnlCards.Visible = True
                pnlCards.Enabled = True
                Dim routeName As String = trvServices.SelectedNode.Text
                'Filter to get only for that ID, So get the IDPRODUCT which is CurrentNodeValue


                Dim queryCards = String.Format("SELECT TOP(100) cs.IDCUSTOM1 as 'CardNo',FLD243 As 'Route', FLD244 AS 'Trips',FLD248 as 'CreatedBy'," &
                                                   " FLD262 AS 'CreatedOn',ISNULL(FLD247,'NOT SOLD') AS 'Status'" &
                                                        " FROM su.CUSTOM1 as cs INNER JOIN su.PROD_CUSTOM1 as pcs ON cs.IDCUSTOM1=pcs.IDCUSTOM1" &
                                                         " WHERE pcs.IDPRODUCT={0}", currentNodeValue)

                Dim dtCards = clsDataLayer.ReturnDataTable(queryCards)
                rptCards.DataSource = dtCards
                rptCards.DataBind()
            Else
                'Do nothing
            End If

        Catch ex As Exception
            clsDataLayer.LogException(ex)
            Response.Redirect("/CustomError.aspx")
        End Try

        'Lets start with the Travellers per month Graph
    End Sub
    Public Function GetRowStyle(Status As String) As String
        Dim rowStyle As String = "warning"
        If Status = "SOLD" Then
            rowStyle = "info"
        ElseIf Status = "EXPIRED" Then
            rowStyle = "error"
        End If
        Return rowStyle
    End Function
    Private Function ReverseString(ByVal rs As String) As String
        Dim cin As String = rs.Split(New Char() {"-"c}, 2)(0)
        Dim cout As String = rs.Split(New Char() {"-"c}, 2)(1)
        Return cout + "-" + cin
    End Function

    Public Function ReverseRowsInDataTable(ByVal inputTable As DataTable) As DataTable

        Dim outputTable As DataTable = inputTable.Clone()

        For i As Integer = inputTable.Rows.Count - 1 To 0 Step -1
            outputTable.ImportRow(inputTable.Rows(i))
        Next

        Return outputTable
    End Function

   
End Class

'Imports System.Data.SqlClient
'Imports System.Data

'Partial Class traffic
'    Inherits System.Web.UI.Page
'    Dim clsDataLayer As New Datalayer
'    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
'        If Not Page.IsPostBack Then
'            'Add the Nodes One By One Starting with the base one of Bus and Subscription
'            Dim serviceNode As TreeNode = New TreeNode("Services", "services", "Styles/icon-16-banner-client.png")
'            Dim busNode As TreeNode = New TreeNode("BUS SERVICE", "bus", "Styles/icon-16-new.png")
'            Dim subscription As TreeNode = New TreeNode("SUBSCRIPTION SERVICE", "subscription", "Styles/icon-16-new.png")

'            'Now Let us Query the Database and Populate the Tree

'            Dim queryTrees As String = "SELECT pp.IDPRODUCT,p.NAME AS 'PARENTNAME',pp.IDPRODUCT2,pTwo.NAME AS 'ChildName' FROM su.PROD_PROD AS pp " &
'                                            "INNER JOIN su.PRODUCTS AS p ON pp.IDPRODUCT = p.IDPRODUCT " &
'                                                "INNER JOIN su.PRODUCTS AS pTwo ON pp.IDPRODUCT2 = pTwo.IDPRODUCT " &
'                                                    "ORDER BY 'PARENTNAME'"
'            'Query For All Rows
'            Dim serviceDt As DataTable = clsDataLayer.ReturnDataTable(queryTrees)
'            'First Get those TO be added directly to the Bus Node
'            Dim directBus() As DataRow = serviceDt.Select("PARENTNAME='TRANSPORT'")

'            Dim i As Integer
'            ' Print column 0 of each returned row. 
'            For i = 0 To directBus.GetUpperBound(0)
'                Dim busRoute As TreeNode = New TreeNode(directBus(i)("ChildName").ToString(), directBus(i)("ChildName").ToString(), "Styles/icon-16-newarticle.png")
'                'Before leaving let us now add all the other child nodes for this dude
'                Dim childRows() As DataRow = serviceDt.Select(String.Format("PARENTNAME='{0}'", directBus(i)("ChildName").ToString()))
'                Dim j As Integer
'                For j = 0 To childRows.GetUpperBound(0)
'                    Dim childRoute As TreeNode = New TreeNode(childRows(j)("ChildName"), childRows(j)("ChildName"), "Styles/icon-16-user-note.png")
'                    busRoute.ChildNodes.Add(childRoute)
'                Next
'                busRoute.Expanded = False
'                busNode.ChildNodes.Add(busRoute)
'            Next

'            Dim directSubs() As DataRow = serviceDt.Select("PARENTNAME='SUBSCRIPTION'")
'            For i = 0 To directSubs.GetUpperBound(0)
'                Dim subs As TreeNode = New TreeNode(directSubs(i)("ChildName"), directSubs(i)("ChildName"), "Styles/icon-16-inbox.png")
'                subscription.ChildNodes.Add(subs)
'            Next
'            subscription.CollapseAll()
'            serviceNode.ChildNodes.Add(busNode)
'            serviceNode.ChildNodes.Add(subscription)


'            trvServices.Nodes.Add(serviceNode)

'        End If


'    End Sub

'    Protected Sub trvServices_SelectedNodeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles trvServices.SelectedNodeChanged
'        'Draw the Appropriate Graphs After Selection
'        Dim currentNodeValue = trvServices.SelectedValue
'        Page.Title = "Sinnovys | Monthly Trend For " + currentNodeValue
'        If currentNodeValue <> "services" AndAlso currentNodeValue <> "bus" AndAlso currentNodeValue <> "subscription" Then
'            'A good node was selected
'            If trvServices.SelectedNode.Parent.Value <> "subscription" Then
'                ' WE are under transport services lets do the magic
'                'Start with Travelers Per Month
'                Dim queryTravelersForward = String.Format("SELECT TOP(6) COUNT(IDRELATION) AS 'Total', DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) AS 'MONTH'," &
'                "CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) AS 'YearMonth'" &
'                "FROM su.SALES_PROD WHERE FLD120+'-'+FLD122='{0}'" &
'                "GROUP BY DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) ,CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102)" &
'                "ORDER BY CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) DESC", currentNodeValue)

'                Dim queryTravelersBack = String.Format("SELECT TOP(6) COUNT(IDRELATION) AS 'Total', DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) AS 'MONTH'," &
'                "CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) AS 'YearMonth'" &
'                "FROM su.SALES_PROD WHERE FLD122+'-'+FLD120='{0}'" &
'                "GROUP BY DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) ,CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102)" &
'                "ORDER BY CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) DESC", currentNodeValue)

'                Dim json As String = String.Format("[['Month','{0}','{1}'],", currentNodeValue, ReverseString(currentNodeValue))

'                Dim dtF As DataTable = clsDataLayer.ReturnDataTable(queryTravelersForward)
'                Dim dtB As DataTable = clsDataLayer.ReturnDataTable(queryTravelersBack)

'                dtF = ReverseRowsInDataTable(dtF)
'                dtB = ReverseRowsInDataTable(dtB)

'                Dim i As Integer

'                For i = 0 To (dtF.Rows.Count - 1)
'                    json += String.Format("['{0}',{1},{2}],", dtF.Rows(i)("Month").ToString, dtF.Rows(i)("Total").ToString, dtB.Rows(i)("Total"))
'                Next

'                Dim charsToTrim() As Char = {","c}
'                Dim jsonF As String = json.TrimEnd(charsToTrim)
'                jsonF += "]"

'                Dim mys As StringBuilder = New StringBuilder()
'                mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(drawChart);")
'                mys.AppendLine("function drawChart() {")
'                mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))
'                mys.AppendLine("var options = {title:  'Last 6 Monthly Travelers Trend','width':800,'height':280,legend: {position:'top'},hAxis: { title: 'Month', titleTextStyle: { color: 'red'} }};")
'                mys.AppendLine("var chart = new google.visualization.LineChart(document.getElementById('tchart'));")
'                mys.AppendLine("chart.draw(data, options);")
'                mys.AppendLine("}")

'                Dim script As String = mys.ToString()
'                Dim csType As Type = Me.[GetType]()
'                Dim cs As ClientScriptManager = Page.ClientScript
'                cs.RegisterClientScriptBlock(csType, "pie", script, True)

'                ' Now work on the Revenue Chart
'                Dim queryRevenueF As String = String.Format("SELECT TOP(6) SUM(TOTAL) AS 'Total', DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) AS 'MONTH'," &
'                "CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) AS 'YearMonth'" &
'                "FROM su.SALES_PROD WHERE FLD120+'-'+FLD122='{0}'" &
'                "GROUP BY DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) ,CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102)" &
'                "ORDER BY CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) DESC", currentNodeValue)

'                Dim queryRevenueB As String = String.Format("SELECT TOP(6) SUM(TOTAL) AS 'Total', DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) AS 'MONTH'," &
'                "CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) AS 'YearMonth'" &
'                "FROM su.SALES_PROD WHERE FLD122+'-'+FLD120='{0}'" &
'                "GROUP BY DATENAME(MONTH,FLD108)+ CONVERT(VARCHAR,YEAR(FLD108)) ,CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102)" &
'                "ORDER BY CONVERT(DATETIME,CONVERT(VARCHAR,YEAR(FLD108))+'-'+CONVERT(VARCHAR,MONTH(FLD108))+'-01',102) DESC", currentNodeValue)

'                Dim dtRevenueF = clsDataLayer.ReturnDataTable(queryRevenueF)
'                Dim dtRevenueB = clsDataLayer.ReturnDataTable(queryRevenueB)

'                dtRevenueF = ReverseRowsInDataTable(dtRevenueF)
'                dtRevenueB = ReverseRowsInDataTable(dtRevenueB)

'                json = String.Format("[['Month','{0}','{1}'],", currentNodeValue, ReverseString(currentNodeValue))
'                For i = 0 To (dtF.Rows.Count - 1)
'                    json += String.Format("['{0}',{1},{2}],", dtRevenueF.Rows(i)("Month").ToString, dtRevenueF.Rows(i)("Total").ToString, dtRevenueB.Rows(i)("Total"))
'                Next

'                jsonF = json.TrimEnd(charsToTrim)
'                jsonF += "]"

'                mys = New StringBuilder()
'                mys.AppendLine("google.load(""visualization"", ""1"", { packages: [""corechart""] });google.setOnLoadCallback(StrawChart);")
'                mys.AppendLine("function StrawChart() {")
'                mys.AppendLine(String.Format("var data = google.visualization.arrayToDataTable({0})", jsonF))


'                mys.AppendLine("var options = {title:  'Monthly Revenue Trend','width':800,'height':350,hAxis: { title: 'Month', titleTextStyle: { color: 'red'} },seriesType:'bars',series: {5: {type: 'line'}}};")
'                mys.AppendLine("var chart = new google.visualization.ComboChart(document.getElementById('rchart'));")
'                mys.AppendLine("chart.draw(data, options);")
'                mys.AppendLine("}")

'                script = mys.ToString()
'                csType = Me.[GetType]()
'                cs.RegisterClientScriptBlock(csType, "rchart", script, True)

'                ' Now Get the Sales Opportunities
'                Dim queryPID = String.Format("SELECT IDPRODUCT FROM su.PRODUCTS WHERE NAME='{0}'", currentNodeValue)
'                Dim pid = clsDataLayer.ReturnSingleNumeric(queryPID)
'                Dim queryOpp As String = String.Format("SELECT IDSALE,FLD164 AS 'Route', NAME,CONVERT(VARCHAR(12),TARGET_DATE,103) AS 'TARGET_DATE'," &
'                        " FLD163 AS 'Hour', FLD103 AS 'Capacity',FLD166 AS 'Booked',STATUS,BUDGET,'' AS 'Vehicle',''AS 'Mail','' AS 'Driver'" &
'                                "FROM su.SALES WHERE DAY(TARGET_DATE) = DAY(GETDATE()) AND MONTH(TARGET_DATE) = MONTH(GETDATE()) AND YEAR(TARGET_DATE) = YEAR(GETDATE()) AND FLD165 = {0}" &
'                                                "ORDER BY FLD164,FLD163", pid)
'                Dim dtOpp As DataTable = clsDataLayer.ReturnDataTable(queryOpp)
'                rptSalesOpportunities.DataSource = dtOpp
'                rptSalesOpportunities.DataBind()
'            ElseIf trvServices.SelectedNode.Parent.Value = "subscription" Then

'            End If
'        End If
'        'Lets start with the Travellers per month Graph
'    End Sub

'    Private Function ReverseString(ByVal rs As String) As String
'        Dim cin As String = rs.Split(New Char() {"-"c}, 2)(0)
'        Dim cout As String = rs.Split(New Char() {"-"c}, 2)(1)
'        Return cout + "-" + cin
'    End Function

'    Public Function ReverseRowsInDataTable(ByVal inputTable As DataTable) As DataTable
'        Dim outputTable As DataTable = inputTable.Clone()

'        For i As Integer = inputTable.Rows.Count - 1 To 0 Step -1
'            outputTable.ImportRow(inputTable.Rows(i))
'        Next

'        Return outputTable
'    End Function
'End Class
