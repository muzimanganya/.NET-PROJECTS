using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class User_Sales_List : System.Web.UI.Page
{
    private String strPreviousRowID;
    //To keep track the Index of Group Total
    private int intSubTotalIndex = 1;

    //Count Promotions And Bookings For the Bus
    private int promotions  = 0;
    private int bookigs = 0;
    private String posNo = "";

    //To temporarily store Sub Total
    private double dblSubTotalQuantity = 0;
    private double dblSubTotalDiscount = 0;
    private double dblSubTotalAmount = 0;

    //To temporarily store Grand Total
    private double dblGrandTotalQuantity = 0;
    private double dblGrandTotalDiscount = 0;
    private double dblGrandTotalAmount = 0;
    private String idsale = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string reqDate = Request.Params.Get("reqDate");
        string username = Request.Params.Get("username");
        //username is mandatory if not defined then redirect to all sales
        if (String.IsNullOrEmpty(username))
            Response.Redirect("/User-Sales.aspx");

        if (String.IsNullOrEmpty(reqDate))
        {
            DateTime dt = DateTime.Now;
            reqDate = String.Format("{0:dd/MM/yyyy}", dt);
        }


        string sql = String.Format("SELECT sl.IDSALE,sl.NAME as 'BusName',sl.DATE_CREAT,sl.FLD103 AS 'Capacity',sl.FLD163 AS 'Hour',sp.IDRELATION,sp.FLD120 as 'CityIN',sp.FLD122 as 'CityOut',sp.PRICE,sp.FLD191 as 'ClientCode', sp.FLD111 as 'ClientName',sp.DISCOUNT,sp.FLD213 AS 'Subscriptiono',sp.DATE_MOD AS 'CreatedOn', sp.FLD123 AS 'CreatedBy',sp.Total FROM su.SALES as sl INNER JOIN su.SALES_PROD as sp  ON sl.IDSALE = sp.IDSALE AND DAY(sp.DATE_MOD) = DAY(CONVERT(datetime, '{0}', 103)) AND MONTH(sp.DATE_MOD) = MONTH(CONVERT(datetime, '{0}', 103)) AND YEAR(sp.DATE_MOD) = YEAR(CONVERT(datetime, '{0}', 103)) WHERE   UserName='{1}' ORDER BY IDSALE", reqDate, username);
        string connString = System.Configuration.ConfigurationManager.ConnectionStrings["AppDBContext"].ConnectionString;
        try
        {
            SqlConnection conn = new SqlConnection(connString);

            
            //get user and set label
            using(SqlConnection connUser = new SqlConnection(connString))
            {
                connUser.Open();
                String sqlUser = String.Format("SELECT Fullname FROM dbo.SystemUsers WHERE UserName='{0}'", username);
                using(SqlCommand cmdUser = new SqlCommand(sqlUser, connUser))
                {
                    using(SqlDataReader reader = cmdUser.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            String fullname = (String)reader.GetValue(0);
                            if(!String.IsNullOrEmpty(fullname))
                            {
                                UserLabel.Text = fullname;
                                break;
                            }
                        }
                    }
                }

            }

            SqlCommand cmd = new SqlCommand(sql, conn);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            gridViewBuses.DataSource = dt;
            gridViewBuses.DataBind();
            conn.Close();

            //Add the POS Details on the header
            String query = "SELECT IDDOCUMENT,NAME, MEMO,FLD220 AS 'MSISDN', FLD221 AS 'IMSI', FLD222 AS 'SimNum',  FLD267 AS 'Place'," +
                                    "FLD268 AS 'TigoSIM',FLD265 AS 'Report',FLD266 AS 'UserMobile' FROM su.DOC"; 

            SqlCommand cmdInfo = new SqlCommand(query, conn);
            DataTable dtInfo = new DataTable();
            SqlDataAdapter daInfo = new SqlDataAdapter(cmdInfo);
            da.Fill(dtInfo);

            DataRow arow =  (DataRow)dtInfo.Rows[0];
            String posInfo = String.Format("POS Name: {0} at {2}, User's Phone: {1}", arow["Memo"].ToString(), arow["UserMobile"].ToString(), arow["Place"].ToString());
            ltrPOS.Text = posInfo; 
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    protected void GridViewRowCreated(Object sender, GridViewRowEventArgs e)
    {
        bool IsSubTotalRowNeedToAdd = false;
        bool IsGrandTotalRowNeedtoAdd = false;

         try{
                int index = e.Row.DataItemIndex;
                GridViewRow currentRow = e.Row;
                var evalIDSALE = System.Web.UI.DataBinder.Eval(currentRow.DataItem, "IDSALE"); 
                if(evalIDSALE!=null)
                    idsale = evalIDSALE.ToString();

             if (strPreviousRowID != null && evalIDSALE != null) 
            {
                if (strPreviousRowID != System.Web.UI.DataBinder.Eval(currentRow.DataItem, "IDSALE").ToString())
                    IsSubTotalRowNeedToAdd = true; 
            }

             if (strPreviousRowID != null && evalIDSALE == null) 
            {
                IsSubTotalRowNeedToAdd = true;
                IsGrandTotalRowNeedtoAdd = true;
                intSubTotalIndex = 0;
            }

            //"Inserting first Row and populating fist Group Header details"
             if (strPreviousRowID == null && evalIDSALE != null) 
            {
                GridView grdViewProducts = (GridView)sender;

                var row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                //Calculate the Header Statistics
                var cell = new TableCell();
                cell.Text = "<div style='width:100%;'>" + "<div style='width:50%;float:left;'>"+" <b style='float:left;'>Bus Route Details :  </b>" +
                    "<div style='float:left;'><a href='Bus-Details.aspx?idsale=" + idsale + "'>" + System.Web.UI.DataBinder.Eval(currentRow.DataItem, "BusName").ToString() +
                    "</a></div></div>" + "<div style='width:50%;float:left;'><div style='width:30%;float:left'><b>Capacity: </b> " + System.Web.UI.DataBinder.Eval(currentRow.DataItem, "Capacity").ToString() + "</div>" +
                    "<div style='width:65%;float:left;'><b>Total Promotions/POS: </b> " + promotions + "<div style='float:right'><b>Total Bookings/POS: </b> " + bookigs + "</div></div></div>";
                cell.ColumnSpan = 8;
                cell.CssClass = "GroupDetailsStyle";
                row.Cells.Add(cell);

                grdViewProducts.Controls[0].Controls.AddAt(index + intSubTotalIndex, row);
                intSubTotalIndex += 1;

                //Region "Add Group Header"
               // Creating a Row
                row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert); 

                cell = new TableCell();
                cell.Text = "City IN";
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "City OUT";
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "TicketID";
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Price";
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Discount";
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Subscription No";
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Created ON";
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = "Creating POS";
                cell.CssClass = "GroupHeaderStyle";
                row.Cells.Add(cell);

                //Adding the Row at the RowIndex position in the Grid
                grdViewProducts.Controls[0].Controls.AddAt(index + intSubTotalIndex, row);
                intSubTotalIndex += 1;
            }

            if(IsSubTotalRowNeedToAdd)
            {  //Region "Adding Sub Total Row"
                GridView grdViewProducts  = (GridView)sender;

                //Creating a Row
                var row= new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                //Adding Total Cell 
                var cell = new TableCell();
                cell.Text = "Total For This Bus: ";
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.ColumnSpan = 7;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                cell = new TableCell();
                cell.Text = String.Format("{0:n2}", dblSubTotalAmount);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "SubTotalRowStyle";
                row.Cells.Add(cell);

                //Adding the Row at the RowIndex position in the Grid
                grdViewProducts.Controls[0].Controls.AddAt(index + intSubTotalIndex, row);
                intSubTotalIndex += 1; 

                //Region "Adding Next Group Header Details"
               if(DataBinder.Eval(currentRow.DataItem, "IDSALE") !=null) 
               {    //Region "Adding Empty Row after each Group Total"
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                    cell = new TableCell();
                    cell.Text = String.Empty;
                    cell.Height = Unit.Parse("10px");
                    cell.ColumnSpan = 7;
                    row.Cells.Add(cell);
                    row.BorderStyle = BorderStyle.None;

                    grdViewProducts.Controls[0].Controls.AddAt(index + intSubTotalIndex, row);
                    intSubTotalIndex += 1;

                    //Region "Adding Next Group Header Details"
                    row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                    //Calculate the Header Statistics

                    cell = new TableCell();
                    cell.Text = "<div style='width:100%;'>"+ "<div style='width:50%;float:left;'>"+ "<b style='float:left;'>Bus Route Details :  </b>"+
                    "<div style='float:left;'><a href='Bus-Details.aspx?idsale=" + idsale + "'>"+ DataBinder.Eval(currentRow.DataItem, "BusName").ToString()+
                    "</a></div></div>"+ "<div style='width:50%;float:left;'><div style='width:30%;float:left'><b>Capacity: </b> "+ DataBinder.Eval(currentRow.DataItem, "Capacity").ToString()+ "</div>"+
                    "<div style='width:65%;float:left;'><b>Total Promotions/POS: </b> "+ promotions+ "<div style='float:right'><b>Total Bookings/POS: </b> "+ bookigs+ "</div></div></div>";
                    cell.ColumnSpan = 8;
                    cell.CssClass = "GroupDetailsStyle";
                    row.Cells.Add(cell);

                    grdViewProducts.Controls[0].Controls.AddAt(index + intSubTotalIndex, row);
                    intSubTotalIndex += 1; 

                    //Region "Add Group Header"
                    //Creating a Row
                    row = new  GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);



                    cell = new TableCell();
                    cell.Text = "City IN";
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = "City OUT";
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = "TicketID";
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = "Price";
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = "Discount";
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = "Subscription No";
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = "Created ON";
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);

                    cell = new TableCell();
                    cell.Text = "Creating POS";
                    cell.CssClass = "GroupHeaderStyle";
                    row.Cells.Add(cell);


                   //Adding the Row at the RowIndex position in the Grid
                    grdViewProducts.Controls[0].Controls.AddAt(index + intSubTotalIndex, row);
                    intSubTotalIndex += 1;
               }

                //Region "Reseting the Sub Total Variables"
                dblSubTotalQuantity = 0;
                dblSubTotalDiscount = 0 ;
                dblSubTotalAmount = 0;
            }

            if (IsGrandTotalRowNeedtoAdd)
            {
                GridView grdViewProducts = (GridView)sender;

                //Region "Adding Empty Row before Grand Total"
                var row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                var cell = new TableCell();
                cell.Text = String.Empty;
                cell.Height = Unit.Parse("10px");
                cell.ColumnSpan = 7;
                row.Cells.Add(cell);
                row.BorderStyle = BorderStyle.None;

                grdViewProducts.Controls[0].Controls.AddAt(index, row);
                intSubTotalIndex += 1;

                //Region "Grand Total Row"
                //Creating a Row
                row = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);

                //Adding Total Cell 
                cell = new TableCell();
                cell.Text = "Grand Total";
                cell.HorizontalAlign = HorizontalAlign.Left;
                cell.ColumnSpan = 7;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);


                //Adding Amount Column
                cell = new TableCell();
                cell.Text = String.Format("{0:n2}", dblGrandTotalAmount);
                cell.HorizontalAlign = HorizontalAlign.Right;
                cell.CssClass = "GrandTotalRowStyle";
                row.Cells.Add(cell);

                //Adding the Row at the RowIndex position in the Grid 
                grdViewProducts.Controls[0].Controls.AddAt(index, row);
            }
         }
        catch(Exception ex)
         {

         }
        
    }

    protected void GridViewDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                strPreviousRowID = DataBinder.Eval(e.Row.DataItem, "IDSALE").ToString();

                double dblQuantity = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Total").ToString());

                double dblAmount = Convert.ToDouble(DataBinder.Eval(e.Row.DataItem, "Total").ToString());

                //Cumulating Sub Total
                dblSubTotalQuantity += dblQuantity;

                dblSubTotalAmount += dblAmount;

                //Cumulating Grand Total
                dblGrandTotalQuantity += dblQuantity;
                dblGrandTotalAmount += dblAmount;
            }
        }
        catch (Exception ex)
        {

        }
        
    }
}