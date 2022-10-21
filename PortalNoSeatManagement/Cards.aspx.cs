using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Cards : System.Web.UI.Page
{
     
    protected void Page_Load(object sender, EventArgs e)
    { 
        if (!IsPostBack)
        {
            searchPanel.Attributes["style"] = "padding: 10px; background: #f0f0f0 none repeat scroll 0 0; border: 1px solid #ccc;";
        }

        if (!String.IsNullOrEmpty(searchText.Text))
        {
            SqlDataSource data = (SqlDataSource)cardsView.DataSourceObject;
            data.SelectCommand = "SELECT Cards.* FROM Cards WHERE CardNo LIKE '%" + searchText.Text + "%'"; 
        }

    }
     
    protected void Page_Init(object sender, EventArgs e)
    {
        Session["UserName"] = HttpContext.Current.User.Identity.Name;
    }

    protected void searchButton_Click(object sender, EventArgs e)
    {
        String s = searchText.Text.Trim();
        if (String.IsNullOrEmpty(s))
            Response.Redirect("Cards.aspx");

        SqlDataSource data = (SqlDataSource)cardsView.DataSourceObject;
        data.SelectCommand = "SELECT Cards.* FROM Cards WHERE CardNo LIKE '%" + s + "%'";
        cardsView.DataBind();
    }

    protected void cardsView_ItemInserted(object sender, ListViewInsertedEventArgs e)
    {
        String CardNo = e.Values["CardNo"].ToString();
        String Amount = e.Values["Amount"].ToString();
        String reason = "New Card";
        addHistory(CardNo, Amount, reason);

    }
    protected void cardsView_ItemEditing(object sender, ListViewEditEventArgs e)
    { 
        if(!String.IsNullOrEmpty(searchText.Text))
        {
            SqlDataSource data = (SqlDataSource)cardsView.DataSourceObject;
            data.SelectCommand = "SELECT Cards.* FROM Cards WHERE CardNo LIKE '%" + searchText.Text.Trim() + "%'"; 
        }
    }

    protected void resetButton_Click(object sender, EventArgs e)
    {
        searchText.Text = null;
        SqlDataSource data = (SqlDataSource)cardsView.DataSourceObject;
        data.SelectCommand = "SELECT Cards.* FROM Cards "; 
    }

    protected void cardsView_ItemUpdated(object sender, ListViewUpdatedEventArgs e)
    { 
        String Amount ="";  
        String reason = "Updating"; 

        String CardNo = cardsView.DataKeys[cardsView.EditIndex].Value.ToString();
        foreach (DictionaryEntry de in e.OldValues)
        {
            String key = de.Key.ToString();
            String val = de.Value.ToString();
            if (key == "Amount")
            { 
                Amount = val;
                Amount = Amount.Replace(",", "");
            }
        }
        addHistory(CardNo, Amount, reason);
    }

    protected void cardsView_ItemDeleting(object sender, ListViewDeleteEventArgs e)
    {
        String CardNo = e.Keys["CardNo"].ToString();
        String Amount = "";
        String reason = "Deleting Card";
         string strConString = ConfigurationManager.ConnectionStrings["AppDBContext"].ConnectionString;
         using (SqlConnection conn = new SqlConnection(strConString))
         {
             using (SqlCommand cmd = new SqlCommand())
             {
                 conn.Open();

                 String sql = "SELECT * FROM [Cards] WHERE CardNo=@CardNo";
                 cmd.Connection = conn;
                 cmd.CommandType = CommandType.Text;
                 cmd.CommandText = sql;
                 cmd.Parameters.AddWithValue("@CardNo", CardNo); 

                using(SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while(rdr.Read())
                    {
                        Amount = rdr["Amount"].ToString();
                        Amount = Amount.Replace(",", "");

                        reason = "Deleting Card:[" + rdr["Owner"].ToString() + "-" + rdr["Phone"].ToString() + "]";
                        addHistory(CardNo, Amount, reason);
                        break;
                    }
                }
             }
         }
       
    }

    /*,[CardNo] ,[CreatedAt] ,[UpdatedAt] ,[Creator] ,[Updater]  ,[Amount] */
    protected void addHistory(String CardNo, String Amount, String reason)
    {
        String sql = "INSERT INTO [CardHistory]([CardNo] ,[CreatedAt] ,[Creator],[Amount],[Reason]) VALUES(@CardNo ,@CreatedAt ,@Creator,@Amount ,@Reason)";
        String user = HttpContext.Current.User.Identity.Name;

        string strConString = ConfigurationManager.ConnectionStrings["AppDBContext"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(strConString))
        {
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = sql;
                //@CardNo ,@UpdatedAt ,@Updater,@Amount, @POS
                cmd.Parameters.AddWithValue("@CardNo", CardNo);
                cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now.ToString()); 
                cmd.Parameters.AddWithValue("@Creator", user);
                cmd.Parameters.AddWithValue("@Reason", reason); 
                cmd.Parameters.AddWithValue("@Amount", Amount); 

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                     
                }
            }
        }
    }
    protected void cardsView_ItemUpdating(object sender, ListViewUpdateEventArgs e)
    {
        SqlDataSource data = (SqlDataSource)cardsView.DataSourceObject;
        bool updatePIN = true;
        bool updateAmount = true;
        foreach (DictionaryEntry de in e.NewValues)
        { 
            if(de.Key==null)
            {
                continue;
            }
            else if (de.Key.ToString() == "PIN" && de.Value==null)
            {
                updatePIN = false;
            }
            else if (de.Key.ToString() == "Amount")
            {
               foreach (DictionaryEntry dold in e.OldValues)
               {
                   if (dold.Key.ToString() == "Amount")
                   {
                       if (dold.Value.ToString() == de.Value.ToString())
                           updateAmount = false;
                   }
               }
            }
            if(!updateAmount && !updatePIN)
            {
                data.UpdateCommand = "UPDATE Cards SET Owner = @Owner, Phone = @Phone, Creator = @Creator, Updater = @Updater, CardNo = @CardNo, IsActive=@IsActive WHERE CardNo = @CardNo"; 
            } 
            else if(updateAmount && !updatePIN)
            {
                data.UpdateCommand = "UPDATE Cards SET Owner = @Owner, Phone = @Phone, Creator = @Creator, Updater = @Updater, CardNo = @CardNo, IsActive=@IsActive, Amount=Amount+@Amount WHERE CardNo = @CardNo";
            }
            else if (!updateAmount && updatePIN)
            {
                data.UpdateCommand = "UPDATE Cards SET Owner = @Owner, Phone = @Phone, Creator = @Creator, Updater = @Updater, CardNo = @CardNo, IsActive=@IsActive, PIN=@PIN WHERE CardNo = @CardNo";
            }
        }
    } 
}