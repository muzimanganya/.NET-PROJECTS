using System;
using System.Collections.Generic; 
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class ClientCodes : System.Web.UI.Page
{ 

    protected void Page_Load(object sender, EventArgs e)
    {
        String query = "SELECT COUNT(IDRELATION) AS TCount,  FLD111 AS 'Traveler', FLD191 AS 'ClientCode',DATE_MOD FROM su.SALES_PROD  WHERE DAY(DATE_MOD) =DAY(GETDATE())  AND MONTH(DATE_MOD) = MONTH(GETDATE()) AND YEAR(DATE_MOD) = YEAR(GETDATE())  GROUP BY FLD111, FLD191,DATE_MOD HAVING COUNT(IDRELATION) >2";
        String connString = ConfigurationManager.ConnectionStrings["AppDBContext"].ConnectionString; ;
        DataTable dt = new DataTable();
        
        using(SqlConnection conn = new SqlConnection(connString))
        {
	        using(SqlCommand cmd = new SqlCommand(query, conn))
	        {
		        conn.Open();
		        using(SqlDataAdapter da = new SqlDataAdapter(cmd))
		        {
			        da.Fill(dt);
		        }
	        }
        } 
        rptMasterView.DataSource = dt;
        rptMasterView.DataBind();
    }

    protected String GetFormatClass(String TCount)
    {  
        String formatString = "";
        int count = int.Parse(TCount);
        if(count > 2) //Highlight with Yellow
            formatString = "yellowFormat";
        else if( count > 4 ) //Highlight with Red
            formatString = "RedFormat";
        else
            formatString = "Normal";

        return formatString;
    }
}