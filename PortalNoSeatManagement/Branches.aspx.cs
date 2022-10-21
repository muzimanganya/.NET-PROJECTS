using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using OfficeOpenXml;
using System.IO;

public partial class Branches : System.Web.UI.Page
{
    public string ChartData = " ";

    protected void Page_Load(object sender, EventArgs e)
    {
        String reqDate = Request.Params["requestDate"];
        String endDate = Request.Params["endDate"];

        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AppDBContext"].ConnectionString;
        try
        {
            SqlConnection conn = new SqlConnection(connString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "[dbo].[spBranchReport]";
            cmd.CommandType = CommandType.StoredProcedure;

            if (String.IsNullOrEmpty(reqDate) || String.IsNullOrEmpty(endDate))
            {
                cmd.Parameters.AddWithValue("@STARTDATE", DBNull.Value);
                cmd.Parameters.AddWithValue("@ENDDATE", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@STARTDATE", reqDate);
                cmd.Parameters.AddWithValue("@ENDDATE", endDate);
            }


            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            headerDate.Text = " - " + reqDate + " to " + endDate;
             
            foreach (DataRow dr in dt.Rows)
            {
                string val = dr["TotalRevenue"].ToString();
                string label = dr["BranchName"].ToString(); //remove all white spaces
                label = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(label); //capitalize first word
                ChartData += ("{" + String.Format(" value: {0},  color: \"{1}\",  highlight: \"{2}\", label:\"{3}\"", val, GetRandomColor(int.Parse(val)), "#df65b0", label) + "},");
            }
            ChartData = ChartData.Trim(); //remove any trailing white space at beginning and end 
            //remove any trailing  comma at the end 
            if (ChartData.EndsWith(","))
                ChartData = ChartData.Substring(0, ChartData.Length - 1);
            ChartData = "[" + ChartData + "]";

            AllBranchsGrid.DataSource = dt;
            AllBranchsGrid.DataBind();

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
            cell.Text = "Branch Name";
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

            AllBranchsGrid.Controls[0].Controls.AddAt(0, row);
        }
        else
        {
            //Left them for future reference in case I need to do somethig with rows -- else can be deleted
            //e.Row.Cells[1].Style.Add("color", "red");
            //String uname = DataBinder.Eval(e.Row.DataItem, "Branch").ToString();
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

    protected void exportExcel_Click(object sender, EventArgs e)
    {
        using (ExcelPackage excel = new ExcelPackage())
        {
            //Create the worksheet
            ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Branches Report");

            var totalCols = AllBranchsGrid.Rows[0].Cells.Count;
            var totalRows = AllBranchsGrid.Rows.Count;
            var headerRow = AllBranchsGrid.HeaderRow;
            for (var i = 1; i <= totalCols; i++)
            {
                workSheet.Cells[1, i].Value = headerRow.Cells[i - 1].Text;
            }
            for (var j = 1; j <= totalRows; j++)
            {
                GridViewRow row = (GridViewRow)AllBranchsGrid.Rows[j-1];

                for (var i = 1; i <= totalCols; i++)
                {
                    if (i == 1)
                    {
                        Control ctl = row.Cells[i - 1].Controls[0];

                        if (ctl is HyperLink)
                        {
                            HyperLink h =  ((HyperLink)ctl);
                            String txt = h.Text.Trim();
                            workSheet.Cells[j + 1, i].Value = txt;
                        }
                    }
                    else
                     workSheet.Cells[j + 1, i].Value = row.Cells[i - 1].Text;
                }
            }
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=Branches-Report.xlsx");
                excel.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
                //Response.BinaryWrite(excel.GetAsByteArray());
            }
        }
    }
}
