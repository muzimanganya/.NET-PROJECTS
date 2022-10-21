using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Data.SqlClient;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["Conn"].ConnectionString))
        {
            connection.Open(); using (SqlCommand cmd = new SqlCommand("SELECT * FROM Companies", connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            String div = "<div class=\"icon\"> <a href=\"SendInvoice.aspx?c=" + reader["ID"].ToString() + "\"> <img src=\"Styles/icon-48-new-privatemessage.png\" /> <span>" + reader["FullName"].ToString() + "</span> </a> </div> ";
                            LiteralControl c = new LiteralControl(div);
                            InvoicesDiv.Controls.Add(c); 
                        }
                    }
                } // reader closed and disposed up here

            } // command disposed here

        } //connection closed and disposed here 
    }
}