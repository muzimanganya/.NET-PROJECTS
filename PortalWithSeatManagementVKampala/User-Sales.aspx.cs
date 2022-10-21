using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Text.RegularExpressions;
using System.Globalization;

public partial class User_Sales : System.Web.UI.Page
{
    public string ChartData = " ";

    protected void Page_Load(object sender, EventArgs e)
    {
        String reqDate = Request.Params["requestDate"];
        string sql = "";
        if (String.IsNullOrEmpty(reqDate))
            sql = "SELECT '' AS ReqDate, COUNT(IDRELATION) AS 'TotalTickets',  RTRIM(ISNULL(UPPER(UserName),'-')) AS 'User', SUM(TOTAL) as 'TotalRevenue',SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo',SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking',SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) as 'TotalRWF',SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB' FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(GETDATE()) AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())  GROUP BY su.SALES_PROD.UserName ORDER BY 'TotalRevenue' DESC";
        else
            sql = String.Format("SELECT '{0}' AS ReqDate, COUNT(IDRELATION) AS 'TotalTickets', RTRIM(ISNULL(UPPER(UserName),'-')) AS 'User', SUM(TOTAL) as 'TotalRevenue',SUM(CASE WHEN FLD213 = '-' THEN 1 ELSE 0 END) as 'TotalNormal',SUM(CASE WHEN FLD213 = 'PROMO' THEN 1 ELSE 0 END) as 'TotalPromo',SUM(CASE WHEN FLD213 <> '-' AND FLD213 <> 'PROMO' THEN 1 ELSE 0 END) as 'TotalBooking',SUM(CASE WHEN FLD118 = 'RWF' THEN Total ELSE 0 END) as 'TotalRWF',SUM(CASE WHEN FLD118 ='FIB' THEN Total ELSE 0 END) as 'TotalFIB' FROM su.SALES_PROD WHERE DAY(DATE_MOD) = DAY(CONVERT(datetime, '{0}', 103)) AND MONTH(DATE_MOD) = MONTH(CONVERT(datetime, '{0}', 103)) AND YEAR(DATE_MOD) = YEAR(CONVERT(datetime, '{0}', 103))  GROUP BY su.SALES_PROD.UserName ORDER BY 'TotalRevenue' DESC", reqDate.Trim());

        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AppDBContext"].ConnectionString;
        try
        {
            SqlConnection conn = new SqlConnection(connString);

            SqlCommand cmd = new SqlCommand(sql, conn);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);


            foreach (DataRow dr in dt.Rows)
            {
                string val = dr["TotalTickets"].ToString();
                string label = Regex.Replace(dr["User"].ToString(), @"\s+", ""); //remove all white spaces
                label = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(label); //capitalize first word
                ChartData += ("{" + String.Format(" value: {0},  color: \"{1}\",  highlight: \"{2}\", label:\"{3}\"", val, GetRandomColor(int.Parse(val)), "#df65b0", label) + "},");
            }
            ChartData = ChartData.Trim(); //remove any trailing white space at beginning and end 
            //remove any trailing  comma at the end 
            if (ChartData.EndsWith(","))
                ChartData = ChartData.Substring(0, ChartData.Length - 1);
            ChartData = "[" + ChartData + "]";

            AllUsersGrid.DataSource = dt;
            AllUsersGrid.DataBind();

            //Plot the Chart here


            //Call the Javascript function from C#
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "DrawChart()", true);

            conn.Close();
        }
        catch (Exception ex)
        {
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Logs\YahooPortal.txt", true))
            {
                file.WriteLine(ex.ToString());
            }
        }
    }

    protected void GridRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            //e.Row.Cells[0]. = "Date"; Do Something on headers
            var row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            var cell = new TableCell();
            cell.Text = String.Empty;
            cell.Height = Unit.Parse("10px");
            cell.ColumnSpan = 7;
            row.Cells.Add(cell);
            row.BorderStyle = BorderStyle.None;
             
            row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

            cell = new TableCell();
            cell.Text = "User Name"; 
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Sold Tickets"; 
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Promotion Tickets"; 
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Bookings"; 
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Total Tickets"; 
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Revenue (RWF)"; 
            row.Cells.Add(cell);

            cell = new TableCell();
            cell.Text = "Revenue (FIB)";
            row.Cells.Add(cell);

            foreach (TableCell c in row.Cells)
            {
                c.Attributes.CssStyle["text-align"] = "left";
                c.CssClass = "GroupHeaderStyle";
                c.Font.Bold = true;
            }

            AllUsersGrid.Controls[0].Controls.AddAt(0, row); 
        }
        else  
        {
            //Left them for future reference in case I need to do somethig with rows -- else can be deleted
            //e.Row.Cells[1].Style.Add("color", "red");
            //String uname = DataBinder.Eval(e.Row.DataItem, "User").ToString();
            //e.Row.Cells[0].Text = uname.ToUpper();
        }
    }

    protected string GetRandomColor(int seed)
    {
        //string[] colors = { "#ffffcc", "#ffeda0", "#fed976", "#feb24c", "#fd8d3c", "#fc4e2a", "#e31a1c", "#bd0026", "#800026" };
        string[] colors = { "#aee256", "#68e256", "#56e289", "#56e2cf", "#56aee2", "#5668e2", "#8a56e2", "#cf56e2", "#e256ae", "#e25668", "#e28956", "#e2cf56" };
        int idx = new Random(seed).Next(0, colors.Length - 1);
        return colors[idx];
    }
}
