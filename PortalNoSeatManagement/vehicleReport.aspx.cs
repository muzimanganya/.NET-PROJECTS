using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class vehicleReport : System.Web.UI.Page
{
    double currentAmount = 0;
    double totalRWF = 0;
    double totalFIB = 0; 

    protected void Page_Load(object sender, EventArgs e)
    {
        //Load CSS
        Page.Header.Controls.Add(
            new LiteralControl(
            @"<style type='text/css'>

                table.table tr:hover td { 
                    color:green; 
                } 
            
                table
                {
                    width:100%;
                }
            </style>
                "
        ));

        //get from Query
        String from = Request.QueryString.Get("from");
        String to = Request.QueryString.Get("to");
        String plateno = Request.QueryString.Get("plateno");
        if(IsPostBack)
        {
            //get from Text Boxes and not Query
            from = DateTextBoxFrom.Text;
            to = DateTextBoxTo.Text;
            plateno = TextBoxPlateNo.Text;
        }
        //Query and Bind to ListView
        QueryVehiclesAndBind(from, to, plateno);
    }

    protected void Page_Query(object sender, EventArgs e)
    {
         //Just because of button click. Remove it in future and find a way button can send data without this
    }

    [System.Web.Script.Services.ScriptMethod()]
    [System.Web.Services.WebMethod]
    public static List<string> GetVehicles(string prefixText)
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["AppDBContext"].ConnectionString);
        SqlCommand cmd = new SqlCommand("select * from dbo.Vehicles where PLATENO like @Name+'%'", con);
        cmd.Parameters.AddWithValue("@Name", prefixText);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        List<string> plateNumbers = new List<string>();
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            plateNumbers.Add(dt.Rows[i][0].ToString());
        }
        return plateNumbers;
    }

    private void QueryVehiclesAndBind(String fromDate, String toDate, String PlateNumber)
    {
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AppDBContext"].ConnectionString;
        //get from Query
        String from = fromDate;
        String to = toDate;
        String plateno = PlateNumber;

        if (from == "" || from==null)
            from = DateTime.Now.Date.ToString().Split(' ').First();

        if (to == "" || to == null)
            to = DateTime.Now.Date.ToString().Split(' ').First();

        if (plateno == "" || plateno == null)
            plateno = "NONE";


        //set Text Boxes 
        DateTextBoxFrom.Text = from; 
        DateTextBoxTo.Text = to; 
        TextBoxPlateNo.Text = plateno;

        using (var conn = new SqlConnection(connString))
        {
            using (var cmd = new SqlCommand("dbo.spSelectBusRoutes", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DATEFROM", from);
                cmd.Parameters.AddWithValue("@DATETO", to);
                cmd.Parameters.AddWithValue("@PLATENO", plateno);

                try
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            RoutesListView.DataSource = dt;
                            RoutesListView.DataBind();

                            //set label for plate number
                            PlateNoLabel.Text = plateno.ToUpper();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

        }
    }
    protected void RoutesListView_DataBound(object sender, ListViewItemEventArgs e)
    {
        
        if (e.Item.ItemType == ListViewItemType.DataItem)
        { 
            ListViewDataItem dataItem = (ListViewDataItem)e.Item;

            // Retrieve the underlying data item, a DataRowView object.        
            DataRowView rowView = (DataRowView)dataItem.DataItem;

            // Retrieve the Currencies
            String rwf = rowView["TotalRWF"].ToString();
            //TEST
            //lblSum.Text = lblSum.Text + " -- " + rwf + "[" + rowView["ROUTENAME"].ToString() + " --" + rowView["ROUTEHOUR"].ToString() + "]";
            //=====
            if (String.IsNullOrEmpty(rwf))
                rwf = "0";

            totalRWF += Convert.ToDouble(rwf);

            String fib = rowView["TotalFIB"].ToString();
            if (String.IsNullOrEmpty(fib))
                fib = "0";
            totalFIB += Convert.ToDouble(fib); 
        }
        //bind ro the label
        String s = String.Format("RWF: {0} <br>FIB: {1} ", totalRWF, totalFIB);
        lblSum.Text = s;

    }
}