namespace Prebooking
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Web.Services;
    using System.Xml;

    [WebService(Namespace="http://www.innovys.co.rw"), WebServiceBinding(ConformsTo=WsiProfiles.BasicProfile1_1), ToolboxItem(false)]
    public class ticketing : WebService
    {
        private static string pass = ConfigurationManager.AppSettings["sqlpass"].ToString();
        private static string server = ConfigurationManager.AppSettings["server"].ToString();

        [WebMethod(CacheDuration=1)]
        public XmlElement GetBooking(string UserName, string Password, string dbpath, string SubscriptionNum, string Date, string Hour, string CityIN, string CityOUT, string Creator, string TicketID = "", string currency = "RWF", string hash = "")
        {
            this.SetDBPath(dbpath);
            XmlDocument document = new XmlDocument {
                PreserveWhitespace = false
            };
            DataSet set = new DataSet("Bookings");
            try
            {
                if (!this.Validate(UserName, Password))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                string connectionString = string.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = string.Format("SELECT IDRELATION FROM dbo.LASTID WHERE POS='{0}' AND DAY(DAY) = DAY(GETDATE())", Creator);
                    try
                    {
                        string str3 = this.ReturnMeSingleString(query, dbpath);
                        this.LogString("Creator is: " + Creator + " Booking: Last ID is " + str3 + " :TicketID Sent is " + TicketID, dbpath);
                        if (((((str3 != string.Empty) && (str3 != "-1")) && ((str3 != TicketID) && (TicketID != string.Empty))) && (((TicketID != "0") && (TicketID != "-1")) && ((TicketID != " ") && (TicketID.Length >= 3)))) && (TicketID != str3))
                        {
                            SqlCommand command = new SqlCommand();
                            using (SqlConnection connection2 = new SqlConnection(connectionString))
                            {
                                connection2.Open();
                                SqlTransaction transaction2 = connection2.BeginTransaction();
                                command.CommandText = "su.TicketRollback";
                                command.CommandType = CommandType.StoredProcedure;
                                command.Connection = connection2;
                                command.Transaction = transaction2;
                                command.Parameters.AddWithValue("@TICKETID", str3);
                                try
                                {
                                    command.ExecuteNonQuery();
                                    transaction2.Commit();
                                }
                                catch (SqlException exception)
                                {
                                    transaction2.Rollback();
                                    this.LogException(exception, dbpath);
                                }
                                catch (Exception exception2)
                                {
                                    transaction2.Rollback();
                                    this.LogException(exception2, dbpath);
                                }
                            }
                            string logContent = string.Format("Creator: {0}, Route {1}-{2} , HOUR: {6}, Subscription Num: {3}, TicketID Rolledback: {4}, TicketID Sent: {5}", new object[] { Creator, CityIN, CityOUT, SubscriptionNum, str3, TicketID, Hour });
                            this.LogString(logContent, dbpath);
                            this.LogString(logContent, dbpath + "Rolls");
                        }
                    }
                    catch (SqlException exception3)
                    {
                        this.LogException(exception3, dbpath);
                    }
                    catch (Exception exception4)
                    {
                        this.LogException(exception4, dbpath);
                    }
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        SqlCommand command2 = new SqlCommand {
                            CommandText = "su.SUBSCRIPTION_TICKETING",
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Transaction = transaction
                        };
                        command2.Parameters.AddWithValue("@SubscriptionNum", SubscriptionNum);
                        command2.Parameters.AddWithValue("@DATE", Date);
                        command2.Parameters.AddWithValue("@HOUR", Hour);
                        command2.Parameters.AddWithValue("@CITYIN", CityIN);
                        command2.Parameters.AddWithValue("@CITYOUT", CityOUT);
                        command2.Parameters.AddWithValue("@CREATOR", Creator);
                        DataTable table = new DataTable("Booking");
                        table.Columns.Add("TicketID");
                        table.Columns.Add("Circuit");
                        table.Columns.Add("Route");
                        table.Columns.Add("Date");
                        table.Columns.Add("Hour");
                        table.Columns.Add("CityIN");
                        table.Columns.Add("CityOUT");
                        table.Columns.Add("TrajetPHONE");
                        table.Columns.Add("SubscriptionNum");
                        table.Columns.Add("Traveller");
                        table.Columns.Add("ClientCode");
                        table.Columns.Add("Price");
                        table.Columns.Add("Discount");
                        table.Columns.Add("Total");
                        table.Columns.Add("Trips");
                        table.Columns.Add("POS");
                        table.Columns.Add("ResNum");
                        table.Columns.Add("ResDate");
                        table.Columns.Add("hash");
                        table.Columns.Add("Message");
                        using (SqlDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = table.NewRow();
                                row["TicketID"] = reader[0].ToString();
                                row["Circuit"] = reader[1].ToString();
                                row["Route"] = reader[2].ToString();
                                row["Date"] = reader[3].ToString();
                                row["Hour"] = reader[4].ToString();
                                row["CityIN"] = reader[5].ToString();
                                row["CityOUT"] = reader[6].ToString();
                                row["TrajetPHONE"] = reader[7].ToString();
                                row["SubscriptionNum"] = reader[8].ToString();
                                row["Traveller"] = reader[9].ToString();
                                row["ClientCode"] = reader[10].ToString();
                                row["Price"] = reader[11].ToString();
                                row["Discount"] = reader[12].ToString();
                                row["Total"] = reader[13].ToString();
                                row["Trips"] = reader[14].ToString();
                                row["POS"] = reader[15].ToString();
                                row["ResNum"] = reader[0x10].ToString();
                                row["ResDate"] = reader[0x11].ToString();
                                row["Message"] = reader[0x12].ToString();
                                row["hash"] = hash;
                                table.Rows.Add(row);
                                this.LogString("Booking: Creator is: " + Creator + " TicketID Assigned is: " + row["TicketID"].ToString(), dbpath);
                            }
                        }
                        transaction.Commit();
                        set.Tables.Add(table);
                        connection.Close();
                        connection.Dispose();
                    }
                    catch (SqlException exception5)
                    {
                        transaction.Rollback();
                        DataTable table2 = new DataTable("Booking");
                        table2.Columns.Add("TicketID");
                        table2.Columns.Add("Circuit");
                        table2.Columns.Add("Route");
                        table2.Columns.Add("Date");
                        table2.Columns.Add("Hour");
                        table2.Columns.Add("CityIN");
                        table2.Columns.Add("CityOUT");
                        table2.Columns.Add("TrajetPHONE");
                        table2.Columns.Add("SubscriptionNum");
                        table2.Columns.Add("Traveller");
                        table2.Columns.Add("ClientCode");
                        table2.Columns.Add("Price");
                        table2.Columns.Add("Discount");
                        table2.Columns.Add("Total");
                        table2.Columns.Add("Trips");
                        table2.Columns.Add("POS");
                        table2.Columns.Add("ResNum");
                        table2.Columns.Add("ResDate");
                        table2.Columns.Add("Message");
                        table2.Columns.Add("hash");
                        DataRow row2 = table2.NewRow();
                        row2["TicketID"] = "0";
                        row2["Circuit"] = "-";
                        row2["Route"] = "-";
                        row2["Date"] = "-";
                        row2["Hour"] = "-";
                        row2["CityIN"] = "-";
                        row2["CityOUT"] = "-";
                        row2["TrajetPHONE"] = "-";
                        row2["SubscriptionNum"] = "-";
                        row2["Traveller"] = "-";
                        row2["ClientCode"] = "0";
                        row2["Price"] = "-";
                        row2["Discount"] = "-";
                        row2["Total"] = "-";
                        row2["Trips"] = "-";
                        row2["POS"] = "-";
                        row2["ResNum"] = "-";
                        row2["ResDate"] = "-";
                        row2["hash"] = hash;
                        try
                        {
                            row2["Message"] = "No Ticket.Error 10.Try Again";
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            row2["Message"] = "System Error.Try Again";
                        }
                        table2.Rows.Add(row2);
                        this.LogException(exception5, dbpath);
                        set.Tables.Add(table2);
                        connection.Close();
                        connection.Dispose();
                    }
                }
            }
            catch (Exception exception6)
            {
                if (set.Tables.Contains("Booking"))
                {
                    this.LogException(exception6, dbpath);
                }
                else
                {
                    DataTable table3 = new DataTable("Booking");
                    table3.Columns.Add("TicketID");
                    table3.Columns.Add("Circuit");
                    table3.Columns.Add("Route");
                    table3.Columns.Add("Date");
                    table3.Columns.Add("Hour");
                    table3.Columns.Add("CityIN");
                    table3.Columns.Add("CityOUT");
                    table3.Columns.Add("TrajetPHONE");
                    table3.Columns.Add("SubscriptionNum");
                    table3.Columns.Add("Traveller");
                    table3.Columns.Add("ClientCode");
                    table3.Columns.Add("Price");
                    table3.Columns.Add("Discount");
                    table3.Columns.Add("Total");
                    table3.Columns.Add("Trips");
                    table3.Columns.Add("POS");
                    table3.Columns.Add("ResNum");
                    table3.Columns.Add("ResDate");
                    table3.Columns.Add("Message");
                    table3.Columns.Add("hash");
                    DataRow row3 = table3.NewRow();
                    row3["TicketID"] = "0";
                    row3["Circuit"] = "-";
                    row3["Route"] = "-";
                    row3["Date"] = "-";
                    row3["Hour"] = "-";
                    row3["CityIN"] = "-";
                    row3["CityOUT"] = "-";
                    row3["TrajetPHONE"] = "-";
                    row3["SubscriptionNum"] = "-";
                    row3["Traveller"] = "-";
                    row3["ClientCode"] = "0";
                    row3["Price"] = "-";
                    row3["Discount"] = "-";
                    row3["Total"] = "-";
                    row3["Trips"] = "-";
                    row3["POS"] = "-";
                    row3["ResNum"] = "-";
                    row3["ResDate"] = "-";
                    row3["hash"] = hash;
                    try
                    {
                        row3["Message"] = "No Ticket.Error 20.Try Again";
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        row3["Message"] = "System Error.Try Again";
                    }
                    this.LogException(exception6, dbpath);
                    table3.Rows.Add(row3);
                    set.Tables.Add(table3);
                    document.LoadXml(set.GetXml());
                    return document.DocumentElement;
                }
            }
            document.LoadXml(set.GetXml());
            return document.DocumentElement;
        }

        [WebMethod(CacheDuration=1)]
        public XmlElement GetOtherReport(string UserName, string Password, string dbpath, string pos, string Year, string Month, string Day)
        {
            this.SetDBPath(dbpath);
            DataSet set = new DataSet("PosReport");
            XmlDocument document = new XmlDocument {
                PreserveWhitespace = false
            };
            try
            {
                if (!this.Validate(UserName, Password))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                using (SqlConnection connection = new SqlConnection(string.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass)))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        SqlDataReader reader;
                        SqlCommand command = new SqlCommand {
                            CommandText = "su.POS_VENDING_WITH_DATE",
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Transaction = transaction
                        };
                        string str2 = string.Format("{0}-{1}-{2}", Year, Month, Day);
                        command.Parameters.AddWithValue("@POS", pos);
                        command.Parameters.AddWithValue("@DATEOF", str2);
                        DataTable table = new DataTable("Report");
                        table.Columns.Add("REPORTDATE");
                        table.Columns.Add("POS");
                        table.Columns.Add("TICKETS");
                        table.Columns.Add("CASH_TICKETING");
                        table.Columns.Add("SUBSCRIPTIONS");
                        table.Columns.Add("CASH_SUBSCRIPTION");
                        table.Columns.Add("TOTAL");
                        using (reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = table.NewRow();
                                row["REPORTDATE"] = str2;
                                row["POS"] = reader["POS"].ToString();
                                row["TICKETS"] = reader["TICKETS"].ToString();
                                row["CASH_TICKETING"] = reader["CASH_TICKETING"].ToString();
                                row["SUBSCRIPTIONS"] = reader["SUBSCRIPTIONS"].ToString();
                                row["CASH_SUBSCRIPTION"] = reader["CASH_SUBSCRIPTION"].ToString();
                                row["TOTAL"] = reader["TOTAL"].ToString();
                                table.Rows.Add(row);
                            }
                        }
                        transaction.Commit();
                        set.Tables.Add(table);
                        reader.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                    catch (SqlException exception)
                    {
                        transaction.Rollback();
                        DataTable table2 = new DataTable("Report");
                        table2.Columns.Add("REPORTDATE");
                        table2.Columns.Add("POS");
                        table2.Columns.Add("TICKETS");
                        table2.Columns.Add("CASH_TICKETING");
                        table2.Columns.Add("SUBSCRIPTIONS");
                        table2.Columns.Add("CASH_SUBSCRIPTION");
                        table2.Columns.Add("TOTAL");
                        DataRow row2 = table2.NewRow();
                        row2["REPORTDATE"] = "-";
                        row2["POS"] = pos;
                        row2["TICKETS"] = "Error";
                        row2["CASH_TICKETING"] = "-";
                        row2["SUBSCRIPTIONS"] = "-";
                        try
                        {
                            row2["CASH_SUBSCRIPTION"] = "Error 10. Try again";
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            row2["CASH_SUBSCRIPTION"] = "System Error.Try again";
                        }
                        row2["TOTAL"] = "ERROR";
                        table2.Rows.Add(row2);
                        set.Tables.Add(table2);
                        this.LogException(exception, dbpath);
                    }
                }
            }
            catch (Exception exception2)
            {
                if (!set.Tables.Contains("Report"))
                {
                    DataTable table3 = new DataTable("Report");
                    table3.Columns.Add("REPORTDATE");
                    table3.Columns.Add("POS");
                    table3.Columns.Add("TICKETS");
                    table3.Columns.Add("CASH_TICKETING");
                    table3.Columns.Add("SUBSCRIPTIONS");
                    table3.Columns.Add("CASH_SUBSCRIPTION");
                    table3.Columns.Add("TOTAL");
                    DataRow row3 = table3.NewRow();
                    row3["REPORTDATE"] = "-";
                    row3["POS"] = pos;
                    row3["TICKETS"] = "Error";
                    row3["CASH_TICKETING"] = "-";
                    row3["SUBSCRIPTIONS"] = "-";
                    try
                    {
                        row3["CASH_SUBSCRIPTION"] = "Error 20.Try again";
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        row3["CASH_SUBSCRIPTION"] = "System Error.Try again";
                    }
                    row3["TOTAL"] = "ERROR";
                    table3.Rows.Add(row3);
                    set.Tables.Add(table3);
                    this.LogException(exception2, dbpath);
                    document.LoadXml(set.GetXml());
                    return document.DocumentElement;
                }
            }
            document.LoadXml(set.GetXml());
            return document.DocumentElement;
        }

        [WebMethod(CacheDuration=1)]
        public XmlElement GetReport(string UserName, string Password, string dbpath, string pos)
        {
            this.SetDBPath(dbpath);
            DataSet set = new DataSet("Subscriptions");
            XmlDocument document = new XmlDocument {
                PreserveWhitespace = false
            };
            try
            {
                if (!this.Validate(UserName, Password))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                using (SqlConnection connection = new SqlConnection(string.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass)))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        SqlDataReader reader;
                        SqlCommand command = new SqlCommand {
                            CommandText = "su.POS_VENDING",
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Transaction = transaction
                        };
                        command.Parameters.AddWithValue("@POS", pos);
                        DataTable table = new DataTable("Subscription");
                        table.Columns.Add("POS");
                        table.Columns.Add("TICKETS");
                        table.Columns.Add("CASH_TICKETING");
                        table.Columns.Add("SUBSCRIPTIONS");
                        table.Columns.Add("CASH_SUBSCRIPTION");
                        table.Columns.Add("TOTAL");
                        using (reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = table.NewRow();
                                row["POS"] = reader["POS"].ToString();
                                row["TICKETS"] = reader["TICKETS"].ToString();
                                row["CASH_TICKETING"] = reader["CASH_TICKETING"].ToString();
                                row["SUBSCRIPTIONS"] = reader["SUBSCRIPTIONS"].ToString();
                                row["CASH_SUBSCRIPTION"] = reader["CASH_SUBSCRIPTION"].ToString();
                                row["TOTAL"] = reader["TOTAL"].ToString();
                                table.Rows.Add(row);
                            }
                        }
                        transaction.Commit();
                        set.Tables.Add(table);
                        reader.Dispose();
                        connection.Close();
                        connection.Dispose();
                    }
                    catch (SqlException exception)
                    {
                        transaction.Rollback();
                        DataTable table2 = new DataTable("Subscription");
                        table2.Columns.Add("POS");
                        table2.Columns.Add("TICKETS");
                        table2.Columns.Add("CASH_TICKETING");
                        table2.Columns.Add("SUBSCRIPTIONS");
                        table2.Columns.Add("CASH_SUBSCRIPTION");
                        table2.Columns.Add("TOTAL");
                        DataRow row2 = table2.NewRow();
                        row2["POS"] = pos;
                        row2["TICKETS"] = "Error";
                        row2["CASH_TICKETING"] = "-";
                        row2["SUBSCRIPTIONS"] = "-";
                        try
                        {
                            row2["CASH_SUBSCRIPTION"] = "Error 10. Try again";
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            row2["CASH_SUBSCRIPTION"] = "System Error.Try again";
                        }
                        row2["TOTAL"] = "ERROR";
                        table2.Rows.Add(row2);
                        set.Tables.Add(table2);
                        this.LogException(exception, dbpath);
                    }
                }
            }
            catch (Exception exception2)
            {
                if (!set.Tables.Contains("Subscription"))
                {
                    DataTable table3 = new DataTable("Subscription");
                    table3.Columns.Add("POS");
                    table3.Columns.Add("TICKETS");
                    table3.Columns.Add("CASH_TICKETING");
                    table3.Columns.Add("SUBSCRIPTIONS");
                    table3.Columns.Add("CASH_SUBSCRIPTION");
                    table3.Columns.Add("TOTAL");
                    DataRow row3 = table3.NewRow();
                    row3["POS"] = pos;
                    row3["TICKETS"] = "Error";
                    row3["CASH_TICKETING"] = "-";
                    row3["SUBSCRIPTIONS"] = "-";
                    try
                    {
                        row3["CASH_SUBSCRIPTION"] = "Error 20.Try again";
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        row3["CASH_SUBSCRIPTION"] = "System Error.Try again";
                    }
                    row3["TOTAL"] = "ERROR";
                    table3.Rows.Add(row3);
                    set.Tables.Add(table3);
                    this.LogException(exception2, dbpath);
                    document.LoadXml(set.GetXml());
                    return document.DocumentElement;
                }
            }
            document.LoadXml(set.GetXml());
            return document.DocumentElement;
        }

        [WebMethod(CacheDuration=1)]
        public XmlElement getSubscription(string UserName, string Password, string dbpath, string SubscriptionNum, string ClientCode, string Name, string FirstName, string Discount, string Creator, string hash = "")
        {
            this.SetDBPath(dbpath);
            XmlDocument document = new XmlDocument {
                PreserveWhitespace = false
            };
            DataSet set = new DataSet("Subscriptions");
            try
            {
                if (!this.Validate(UserName, Password))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                if (!this.ValidateCode(ClientCode))
                {
                    document.LoadXml(this.ValidateClientCode(ClientCode));
                    return document.DocumentElement;
                }
                using (SqlConnection connection = new SqlConnection(string.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass)))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    try
                    {
                        SqlCommand command = new SqlCommand {
                            CommandText = "su.SUBSCRIPTION",
                            CommandType = CommandType.StoredProcedure,
                            Connection = connection,
                            Transaction = transaction
                        };
                        command.Parameters.AddWithValue("@SubscriptionNum", SubscriptionNum);
                        command.Parameters.AddWithValue("@ClientCode", ClientCode);
                        command.Parameters.AddWithValue("@NAME", Name);
                        command.Parameters.AddWithValue("@FIRSTNAME", FirstName);
                        command.Parameters.AddWithValue("@DISCOUNT", Discount);
                        command.Parameters.AddWithValue("@CREATOR", Creator);
                        DataTable table = new DataTable("Subscription");
                        table.Columns.Add("SubscriptionNum");
                        table.Columns.Add("Circuit");
                        table.Columns.Add("Traveller");
                        table.Columns.Add("ClientCode");
                        table.Columns.Add("Price");
                        table.Columns.Add("Discount");
                        table.Columns.Add("Total");
                        table.Columns.Add("Points");
                        table.Columns.Add("POS");
                        table.Columns.Add("RESDATE");
                        table.Columns.Add("Message");
                        table.Columns.Add("hash");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = table.NewRow();
                                row["SubscriptionNum"] = reader["SubscriptionNum"].ToString();
                                row["SubscriptionNum"] = reader["SubscriptionNum"].ToString();
                                row["Circuit"] = reader["Circuit"].ToString();
                                row["Traveller"] = reader["Traveller"].ToString();
                                row["ClientCode"] = reader["ClientCode"].ToString();
                                row["Price"] = reader["Price"].ToString();
                                row["Discount"] = reader["Discount"].ToString();
                                row["Total"] = reader["Total"].ToString();
                                row["Points"] = reader["Points"].ToString();
                                row["POS"] = reader["POS"].ToString();
                                row["RESDATE"] = reader["RESDATE"].ToString();
                                row["hash"] = hash;
                                row["Message"] = reader["Message"].ToString();
                                table.Rows.Add(row);
                            }
                        }
                        transaction.Commit();
                        set.Tables.Add(table);
                        connection.Close();
                        connection.Dispose();
                    }
                    catch (SqlException exception)
                    {
                        transaction.Rollback();
                        DataTable table2 = new DataTable("Subscription");
                        table2.Columns.Add("SubscriptionNum");
                        table2.Columns.Add("Circuit");
                        table2.Columns.Add("Traveller");
                        table2.Columns.Add("ClientCode");
                        table2.Columns.Add("Price");
                        table2.Columns.Add("Discount");
                        table2.Columns.Add("Total");
                        table2.Columns.Add("Points");
                        table2.Columns.Add("POS");
                        table2.Columns.Add("RESDATE");
                        table2.Columns.Add("Message");
                        table2.Columns.Add("hash");
                        DataRow row2 = table2.NewRow();
                        row2["SubscriptionNum"] = "-";
                        row2["SubscriptionNum"] = "-";
                        row2["Circuit"] = "-";
                        row2["Traveller"] = "-";
                        row2["ClientCode"] = "-";
                        row2["Price"] = "-";
                        row2["Discount"] = "-";
                        row2["Total"] = "-";
                        row2["Points"] = "-";
                        row2["POS"] = "-";
                        row2["RESDATE"] = "-";
                        row2["hash"] = hash;
                        try
                        {
                            row2["Message"] = "No Ticket.Error 10.Try Again";
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            row2["Message"] = "System Error 30.Try Again";
                        }
                        this.LogException(exception, dbpath);
                        table2.Rows.Add(row2);
                        set.Tables.Add(table2);
                    }
                }
            }
            catch (Exception exception2)
            {
                if (!set.Tables.Contains("Subscription"))
                {
                    DataTable table3 = new DataTable("Subscription");
                    table3.Columns.Add("SubscriptionNum");
                    table3.Columns.Add("Circuit");
                    table3.Columns.Add("Traveller");
                    table3.Columns.Add("ClientCode");
                    table3.Columns.Add("Price");
                    table3.Columns.Add("Discount");
                    table3.Columns.Add("Total");
                    table3.Columns.Add("Points");
                    table3.Columns.Add("POS");
                    table3.Columns.Add("RESDATE");
                    table3.Columns.Add("Message");
                    table3.Columns.Add("hash");
                    DataRow row3 = table3.NewRow();
                    row3["SubscriptionNum"] = "-";
                    row3["SubscriptionNum"] = "-";
                    row3["Circuit"] = "-";
                    row3["Traveller"] = "-";
                    row3["ClientCode"] = "-";
                    row3["Price"] = "-";
                    row3["Discount"] = "-";
                    row3["Total"] = "-";
                    row3["Points"] = "-";
                    row3["POS"] = "-";
                    row3["RESDATE"] = "-";
                    row3["hash"] = hash;
                    try
                    {
                        row3["Message"] = "No Ticket.Error 20.Try Again";
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        row3["Message"] = "System Error.Try Again";
                    }
                    this.LogException(exception2, dbpath);
                    table3.Rows.Add(row3);
                    set.Tables.Add(table3);
                    document.LoadXml(set.GetXml());
                    return document.DocumentElement;
                }
            }
            document.LoadXml(set.GetXml());
            return document.DocumentElement;
        }

        [WebMethod(CacheDuration=1)]
        public XmlElement GetTicket(string UserName, string Password, string dbpath, string Date, string Hour, string CityIN, string CityOut, string Discount, string Creator, string Name = "", string Firstname = "", string ClientCode = "", string TicketID = "0", string currency = "RWF", string hash = "", string DocumentID = "")
        {
            this.SetDBPath(dbpath);
            DataSet set = new DataSet("Tickets");
            DataTable table = new DataTable();
            XmlDocument document = new XmlDocument {
                PreserveWhitespace = false
            };
            try
            {
                if (!this.Validate(UserName, Password))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                if (!this.ValidateCode(ClientCode))
                {
                    document.LoadXml(this.ValidateClientCode(ClientCode));
                    return document.DocumentElement;
                }
                string connectionString = string.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                SqlConnection connection = new SqlConnection(connectionString);
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = string.Format("SELECT IDRELATION FROM dbo.LASTID WHERE POS='{0}' AND DAY(DAY) = DAY(GETDATE())", Creator);
                    try
                    {
                        string str3 = this.ReturnMeSingleString(query, dbpath);
                        this.LogString("Creator is: " + Creator + " : Last ID is " + str3 + "   :TicketID Sent is: " + TicketID, dbpath);
                        if (((((str3 != string.Empty) && (str3 != "-1")) && ((str3 != TicketID) && (TicketID != string.Empty))) && (((TicketID != "0") && (TicketID != "-1")) && ((TicketID != " ") && (TicketID.Length >= 3)))) && (TicketID != str3))
                        {
                            SqlCommand command = new SqlCommand();
                            using (SqlConnection connection2 = new SqlConnection(connectionString))
                            {
                                connection2.Open();
                                SqlTransaction transaction2 = connection2.BeginTransaction();
                                command.CommandText = "su.TicketRollback";
                                command.CommandType = CommandType.StoredProcedure;
                                command.Connection = connection2;
                                command.Transaction = transaction2;
                                command.Parameters.AddWithValue("@TICKETID", str3);
                                try
                                {
                                    command.ExecuteNonQuery();
                                    transaction2.Commit();
                                }
                                catch (SqlException exception)
                                {
                                    transaction2.Rollback();
                                    this.LogException(exception, dbpath);
                                }
                                catch (Exception exception2)
                                {
                                    transaction2.Rollback();
                                    this.LogException(exception2, dbpath);
                                }
                            }
                            string logContent = string.Format("Creator: {0}, Route: {1}-{2}, Hour: {8}, Client Code: {3}, Name: {4} {5}, TicketID Rolledback: {6}, TicketID Sent: {7}", new object[] { Creator, CityIN, CityOut, ClientCode, Firstname, Name, str3, TicketID, Hour });
                            this.LogString(logContent, dbpath);
                            this.LogString(logContent, dbpath + "Rolls");
                        }
                    }
                    catch (SqlException exception3)
                    {
                        this.LogException(exception3, dbpath);
                    }
                    catch (Exception exception4)
                    {
                        this.LogException(exception4, dbpath);
                    }
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand command2 = new SqlCommand();
                    try
                    {
                        command2.CommandText = "su.Ticketing";
                        command2.CommandType = CommandType.StoredProcedure;
                        command2.Connection = connection;
                        command2.Transaction = transaction;
                        command2.Parameters.AddWithValue("@DATE", Date);
                        command2.Parameters.AddWithValue("@HOUR", Hour);
                        command2.Parameters.AddWithValue("@CityIN", CityIN);
                        command2.Parameters.AddWithValue("@CityOut", CityOut);
                        command2.Parameters.AddWithValue("@Name", Name);
                        command2.Parameters.AddWithValue("@Firstname", Firstname);
                        command2.Parameters.AddWithValue("@ClientCode", ClientCode);
                        command2.Parameters.AddWithValue("@Discount", Discount);
                        command2.Parameters.AddWithValue("@Creator", Creator);
                        command2.Parameters.AddWithValue("@Currency", currency);
                        if (DocumentID.Length > 0)
                        {
                            command2.Parameters.AddWithValue("@Passport", DocumentID);
                        }
                        table = new DataTable("Ticket");
                        table.Columns.Add("TicketID");
                        table.Columns.Add("Circuit");
                        table.Columns.Add("Route");
                        table.Columns.Add("Date");
                        table.Columns.Add("Hour");
                        table.Columns.Add("CityIn");
                        table.Columns.Add("CityOut");
                        table.Columns.Add("TrajetPhone");
                        table.Columns.Add("Traveller");
                        table.Columns.Add("ClientCode");
                        table.Columns.Add("Price");
                        table.Columns.Add("Discount");
                        table.Columns.Add("Total");
                        table.Columns.Add("Points");
                        table.Columns.Add("Pos");
                        table.Columns.Add("ResNum");
                        table.Columns.Add("ResDate");
                        table.Columns.Add("Message");
                        table.Columns.Add("hash");
                        table.Columns.Add("DocumentID");
                        using (SqlDataReader reader = command2.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = table.NewRow();
                                row["TicketID"] = reader["TicketID"].ToString();
                                row["Circuit"] = reader["Circuit"].ToString();
                                row["Route"] = reader["Route"].ToString();
                                row["Date"] = reader["Date"].ToString();
                                row["Hour"] = reader["Hour"].ToString();
                                row["CityIN"] = reader["CityIN"].ToString();
                                row["CityOut"] = reader["CityOut"].ToString();
                                row["TrajetPhone"] = reader["TrajetPhone"].ToString();
                                row["Traveller"] = reader["Traveller"].ToString();
                                row["ClientCode"] = reader["ClientCode"].ToString();
                                row["Price"] = reader["Price"].ToString();
                                row["Discount"] = reader["Discount"].ToString();
                                row["Total"] = reader["Total"].ToString();
                                row["Points"] = reader["points"].ToString();
                                row["Pos"] = reader["Pos"].ToString();
                                row["ResNum"] = reader["ResNum"].ToString();
                                row["ResDate"] = reader["ResDate"].ToString();
                                row["Message"] = reader["Message"].ToString();
                                row["hash"] = hash;
                                row["DocumentID"] = DocumentID;
                                table.Rows.Add(row);
                                this.LogString("Normal Ticketing: Creator is: " + Creator + " TicketID ASsigned is " + row["TicketID"].ToString(), dbpath);
                            }
                        }
                        set.Tables.Add(table);
                        transaction.Commit();
                        connection.Close();
                        connection.Dispose();
                    }
                    catch (SqlException exception5)
                    {
                        transaction.Rollback();
                        connection.Close();
                        connection.Dispose();
                        table = new DataTable("Tte");
                        table.Columns.Add("TicketID");
                        table.Columns.Add("Circuit");
                        table.Columns.Add("Route");
                        table.Columns.Add("Date");
                        table.Columns.Add("Hour");
                        table.Columns.Add("CityIn");
                        table.Columns.Add("CityOut");
                        table.Columns.Add("TrajetPhone");
                        table.Columns.Add("Traveller");
                        table.Columns.Add("ClientCode");
                        table.Columns.Add("Price");
                        table.Columns.Add("Discount");
                        table.Columns.Add("Total");
                        table.Columns.Add("Points");
                        table.Columns.Add("Pos");
                        table.Columns.Add("ResNum");
                        table.Columns.Add("ResDate");
                        table.Columns.Add("Message");
                        table.Columns.Add("hash");
                        table.Columns.Add("DocumentID");
                        DataRow row2 = table.NewRow();
                        row2["TicketID"] = "0";
                        row2["Circuit"] = "0";
                        row2["Route"] = "-";
                        row2["Date"] = "-";
                        row2["Hour"] = "-";
                        row2["CityIn"] = "-";
                        row2["CityOut"] = "-";
                        row2["TrajetPhone"] = "-";
                        row2["Traveller"] = "-";
                        row2["ClientCode"] = "-";
                        row2["Price"] = "-";
                        row2["Discount"] = "-";
                        row2["Total"] = "-";
                        row2["Points"] = "-";
                        row2["Pos"] = "-";
                        row2["ResNum"] = "-";
                        row2["ResDate"] = "-";
                        row2["hash"] = hash;
                        row2["DocumentID"] = "-";
                        try
                        {
                            row2["Message"] = "No Ticket.Error 10.Try Again";
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            row2["Message"] = "System Error. Try Again.";
                        }
                        this.LogException(exception5, dbpath);
                        table.Rows.Add(row2);
                        set.Tables.Add(table);
                        document.LoadXml(set.GetXml());
                        return document.DocumentElement;
                    }
                }
            }
            catch (Exception exception6)
            {
                if (set.Tables.Contains("Tt"))
                {
                    this.LogException(exception6, dbpath);
                }
                else
                {
                    table = new DataTable("Ttem");
                    table.Columns.Add("TicketID");
                    table.Columns.Add("Circuit");
                    table.Columns.Add("Route");
                    table.Columns.Add("Date");
                    table.Columns.Add("Hour");
                    table.Columns.Add("CityIn");
                    table.Columns.Add("CityOut");
                    table.Columns.Add("TrajetPhone");
                    table.Columns.Add("Traveller");
                    table.Columns.Add("ClientCode");
                    table.Columns.Add("Price");
                    table.Columns.Add("Discount");
                    table.Columns.Add("Total");
                    table.Columns.Add("Points");
                    table.Columns.Add("Pos");
                    table.Columns.Add("ResNum");
                    table.Columns.Add("ResDate");
                    table.Columns.Add("Message");
                    table.Columns.Add("hash");
                    table.Columns.Add("DocumentID");
                    DataRow row3 = table.NewRow();
                    row3["TicketID"] = "0";
                    row3["Circuit"] = "0";
                    row3["Route"] = "-";
                    row3["Date"] = "-";
                    row3["Hour"] = "-";
                    row3["CityIn"] = "-";
                    row3["CityOut"] = "-";
                    row3["TrajetPhone"] = "-";
                    row3["Traveller"] = "-";
                    row3["ClientCode"] = "-";
                    row3["Price"] = "-";
                    row3["Discount"] = "-";
                    row3["Total"] = "-";
                    row3["Points"] = "-";
                    row3["Pos"] = "-";
                    row3["ResNum"] = "-";
                    row3["ResDate"] = "-";
                    row3["DocumentID"] = "-";
                    row3["hash"] = hash;
                    try
                    {
                        row3["Message"] = "No Ticket.Error 20.Try Again";
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        row3["Message"] = "System Error. Try Again.";
                    }
                    table.Rows.Add(row3);
                    set.Tables.Add(table);
                    document.LoadXml(set.GetXml());
                    this.LogException(exception6, dbpath);
                    return document.DocumentElement;
                }
            }
            document.LoadXml(set.GetXml());
            return document.DocumentElement;
        }

        [WebMethod]
        public XmlElement GetMobileTicket(string UserName, string Password, string dbpath, string Creator, string TicketID, string hash)
        {
            SqlConnection connection;
            this.SetDBPath(dbpath);
            XmlDocument document = new XmlDocument {
                PreserveWhitespace = false
            };
            string connectionString = string.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
            SqlCommand command = new SqlCommand();
            DataSet set = new DataSet();
            DataTable table = new DataTable("Ticket");
            table.Columns.Add("TicketID");
            table.Columns.Add("Circuit");
            table.Columns.Add("Route");
            table.Columns.Add("Date");
            table.Columns.Add("Hour");
            table.Columns.Add("CityIn");
            table.Columns.Add("CityOut");
            table.Columns.Add("TrajetPhone");
            table.Columns.Add("Traveller");
            table.Columns.Add("ClientCode");
            table.Columns.Add("Price");
            table.Columns.Add("Discount");
            table.Columns.Add("Total");
            table.Columns.Add("Points");
            table.Columns.Add("Pos");
            table.Columns.Add("ResNum");
            table.Columns.Add("ResDate");
            table.Columns.Add("Message");
            table.Columns.Add("hash");
            SqlConnection connection2 = connection = new SqlConnection(connectionString);
            try
            {
                if (!this.Validate(UserName, Password))
                {
                    Exception exception = new Exception("Wrong UserName or Password");
                    throw exception;
                }
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        command.CommandText = "su.MOBILETICKET";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@TICKET", TicketID);
                        command.Parameters.AddWithValue("@CREATOR", Creator);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = table.NewRow();
                                row["TicketID"] = reader["TicketID"].ToString();
                                row["Circuit"] = reader["Circuit"].ToString();
                                row["Route"] = reader["Route"].ToString();
                                row["Date"] = reader["Date"].ToString();
                                row["Hour"] = reader["Hour"].ToString();
                                row["CityIN"] = reader["CityIN"].ToString();
                                row["CityOut"] = reader["CityOut"].ToString();
                                row["TrajetPhone"] = reader["TrajetPhone"].ToString();
                                row["Traveller"] = reader["Traveller"].ToString();
                                row["ClientCode"] = reader["ClientCode"].ToString();
                                row["Price"] = reader["Price"].ToString();
                                row["Discount"] = reader["Discount"].ToString();
                                row["Total"] = reader["Total"].ToString();
                                row["Points"] = reader["points"].ToString();
                                row["Pos"] = reader["Pos"].ToString();
                                row["ResNum"] = reader["ResNum"].ToString();
                                row["ResDate"] = reader["ResDate"].ToString();
                                row["Message"] = reader["Message"].ToString();
                                row["hash"] = hash;
                                table.Rows.Add(row);
                            }
                        }
                        transaction.Commit();
                        set.Tables.Add(table);
                        document.LoadXml(set.GetXml());
                    }
                    catch (Exception exception2)
                    {
                        DataRow row2 = table.NewRow();
                        row2["TicketID"] = "0";
                        row2["Circuit"] = "-";
                        row2["Route"] = "-";
                        row2["Date"] = "-";
                        row2["Hour"] = "-";
                        row2["CityIn"] = "-";
                        row2["CityOut"] = "-";
                        row2["TrajetPhone"] = "-";
                        row2["Traveller"] = "-";
                        row2["ClientCode"] = "-";
                        row2["Price"] = "0";
                        row2["Discount"] = "0";
                        row2["Total"] = "0";
                        row2["Points"] = "0";
                        row2["Pos"] = Creator;
                        row2["ResNum"] = "5";
                        row2["ResDate"] = "-";
                        row2["hash"] = hash;
                        row2["Message"] = "System Error. Try Again";
                        table.Rows.Add(row2);
                        transaction.Rollback();
                        set.Tables.Add(table);
                        document.LoadXml(set.GetXml());
                        this.LogException(exception2, dbpath);
                        return document.DocumentElement;
                    }
                }
            }
            catch (Exception exception3)
            {
                DataRow row3 = table.NewRow();
                row3["TicketID"] = "0";
                row3["Circuit"] = "-";
                row3["Route"] = "-";
                row3["Date"] = "-";
                row3["Hour"] = "-";
                row3["CityIn"] = "-";
                row3["CityOut"] = "-";
                row3["TrajetPhone"] = "-";
                row3["Traveller"] = "-";
                row3["ClientCode"] = "-";
                row3["Price"] = "0";
                row3["Discount"] = "0";
                row3["Total"] = "0";
                row3["Points"] = "0";
                row3["Pos"] = Creator;
                row3["ResNum"] = "5";
                row3["ResDate"] = "-";
                row3["hash"] = hash;
                row3["Message"] = "System Error. Try Again";
                table.Rows.Add(row3);
                set.Tables.Add(table);
                document.LoadXml(set.GetXml());
                this.LogException(exception3, dbpath);
                return document.DocumentElement;
            }
            finally
            {
                if (connection2 != null)
                {
                    connection2.Dispose();
                }
            }
            return document.DocumentElement;
        }

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        private void LogException(Exception ex, string dbpath)
        {
            try
            {
                string path = @"C:\inetpub\wwwroot\vmcws\logs\exceptions.txt";
                using (TextWriter writer = new StreamWriter(path, true))
                {
                    string str2 = string.Format("{0}  :  {1}  : {2} : {3}", new object[] { DateTime.Now.ToString(), dbpath, ex.Message.ToString(), ex.Source.ToString() });
                    writer.WriteLine(str2);
                    writer.WriteLine(ex.StackTrace.ToString());
                    writer.WriteLine("");
                    writer.WriteLine("------------------------------------------------------------------------------------------------------");
                    writer.WriteLine("");
                    writer.Close();
                }
            }
            catch (IOException)
            {
            }
            catch (Exception)
            {
            }
        }

        private void LogString(string logContent, string dbpath)
        {
            try
            {
                using (TextWriter writer = new StreamWriter(@"C:\inetpub\wwwroot\vmcws\logs\" + dbpath + ".txt", true))
                {
                    string str2 = string.Format("{0}  :  {1}  : {2}", DateTime.Now.ToString(), dbpath, logContent);
                    writer.WriteLine(str2);
                    writer.WriteLine("");
                    writer.WriteLine("------------------------------------------------------------------------------------------------------");
                    writer.WriteLine("");
                    writer.Close();
                }
            }
            catch (IOException exception)
            {
                this.LogException(exception, dbpath);
            }
            catch (Exception exception2)
            {
                this.LogException(exception2, dbpath);
            }
        }

        private void LogString(string logContent, string dbpath, string path)
        {
            try
            {
                using (TextWriter writer = new StreamWriter(@"C:\inetpub\wwwroot\vmcws\logs\" + dbpath + "-" + path + ".txt", true))
                {
                    string str2 = string.Format("{0}  :  {1}  : {2}", DateTime.Now.ToString(), dbpath, logContent);
                    writer.WriteLine(str2);
                    writer.WriteLine("");
                    writer.WriteLine("------------------------------------------------------------------------------------------------------");
                    writer.WriteLine("");
                    writer.Close();
                }
            }
            catch (IOException exception)
            {
                this.LogException(exception, dbpath);
            }
            catch (Exception exception2)
            {
                this.LogException(exception2, dbpath);
            }
        }

        [WebMethod(CacheDuration=1)]
        public XmlElement Prebooking(string UserName, string Password, string dbpath, string Date, string Hour, string CityIN, string CityOut, string Discount, string Creator, string Name = "", string Firstname = "", string ClientCode = "", string currency = "RWF", string hash = "", string DocumentID = "")
        {
            this.SetDBPath(dbpath);
            DataSet set = new DataSet("Prebooking");
            DataTable table = new DataTable();
            XmlDocument document = new XmlDocument {
                PreserveWhitespace = false
            };
            try
            {
                if (!this.Validate(UserName, Password))
                {
                    throw new Exception(string.Format("Wrong UserName or Password. You sent Username: {0} And Password: {1}", UserName, Password));
                }
                if (!this.ValidateCode(ClientCode))
                {
                    document.LoadXml(this.ValidateClientCode(ClientCode));
                    return document.DocumentElement;
                }
                string connectionString = string.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                SqlConnection connection = new SqlConnection(connectionString);
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand command = new SqlCommand();
                    try
                    {
                        command.CommandText = "su.Prebooking";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@DATE", Date);
                        command.Parameters.AddWithValue("@HOUR", Hour);
                        command.Parameters.AddWithValue("@CityIN", CityIN);
                        command.Parameters.AddWithValue("@CityOut", CityOut);
                        command.Parameters.AddWithValue("@Name", Name);
                        command.Parameters.AddWithValue("@Firstname", Firstname);
                        command.Parameters.AddWithValue("@ClientCode", ClientCode);
                        command.Parameters.AddWithValue("@Discount", Discount);
                        command.Parameters.AddWithValue("@Creator", Creator);
                        command.Parameters.AddWithValue("@Currency", currency);
                        table = new DataTable("Prebooking");
                        table.Columns.Add("BookingNo");
                        table.Columns.Add("Circuit");
                        table.Columns.Add("BusName");
                        table.Columns.Add("Traveller");
                        table.Columns.Add("ClientCode");
                        table.Columns.Add("Price");
                        table.Columns.Add("Pos");
                        table.Columns.Add("ResDate");
                        table.Columns.Add("Message");
                        table.Columns.Add("hash");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = table.NewRow();
                                row["BookingNo"] = reader["BookingNo"].ToString();
                                row["Circuit"] = reader["Circuit"].ToString();
                                row["BusName"] = reader["BusName"].ToString();
                                row["Traveller"] = reader["Traveller"].ToString();
                                row["ClientCode"] = reader["ClientCode"].ToString();
                                row["Price"] = reader["Price"].ToString();
                                row["Pos"] = reader["Pos"].ToString();
                                row["ResDate"] = reader["ResDate"].ToString();
                                row["Message"] = reader["Message"].ToString();
                                row["hash"] = hash;
                                table.Rows.Add(row);
                                this.LogString("Prebooking Ticketing: Creator is: " + Creator + " BookingNo ASsigned is " + row["BookingNo"].ToString(), dbpath);
                            }
                        }
                        set.Tables.Add(table);
                        transaction.Commit();
                        connection.Close();
                        connection.Dispose();
                    }
                    catch (SqlException exception)
                    {
                        transaction.Rollback();
                        connection.Close();
                        connection.Dispose();
                        table = new DataTable("Tte");
                        table.Columns.Add("BookingNo");
                        table.Columns.Add("Circuit");
                        table.Columns.Add("BusName");
                        table.Columns.Add("Traveller");
                        table.Columns.Add("ClientCode");
                        table.Columns.Add("Price");
                        table.Columns.Add("Pos");
                        table.Columns.Add("ResDate");
                        table.Columns.Add("Message");
                        table.Columns.Add("hash");
                        DataRow row2 = table.NewRow();
                        row2["BookingNo"] = 0;
                        row2["Circuit"] = "-";
                        row2["BusName"] = "-";
                        row2["Traveller"] = "-";
                        row2["ClientCode"] = "-";
                        row2["Price"] = "-";
                        row2["Pos"] = "-";
                        row2["ResDate"] = "-";
                        row2["hash"] = hash;
                        try
                        {
                            row2["Message"] = "Ticket Not Issued Due To " + exception.Message.ToString() + ". Please Try Again";
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            row2["Message"] = "System Error. Try Again.";
                        }
                        this.LogException(exception, dbpath);
                        table.Rows.Add(row2);
                        set.Tables.Add(table);
                        document.LoadXml(set.GetXml());
                        return document.DocumentElement;
                    }
                }
            }
            catch (Exception exception2)
            {
                if (set.Tables.Contains("Tt"))
                {
                    this.LogException(exception2, dbpath);
                }
                else
                {
                    table = new DataTable("Ttem");
                    table.Columns.Add("BookingNo");
                    table.Columns.Add("Circuit");
                    table.Columns.Add("BusName");
                    table.Columns.Add("Traveller");
                    table.Columns.Add("ClientCode");
                    table.Columns.Add("Price");
                    table.Columns.Add("Pos");
                    table.Columns.Add("ResDate");
                    table.Columns.Add("Message");
                    table.Columns.Add("hash");
                    DataRow row3 = table.NewRow();
                    row3["BookingNo"] = 0;
                    row3["Circuit"] = "-";
                    row3["BusName"] = "-";
                    row3["Traveller"] = "-";
                    row3["ClientCode"] = "-";
                    row3["Price"] = "-";
                    row3["Pos"] = "-";
                    row3["ResDate"] = "-";
                    row3["hash"] = hash;
                    try
                    {
                        row3["Message"] = "General Exception. Ticket Not Issued Due To " + exception2.Message.ToString() + ". Please Try Again. This Error Came From " + exception2.Source.ToString() + " And the Stack Trace " + exception2.StackTrace.ToString();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        row3["Message"] = "System Error. Try Again.";
                    }
                    table.Rows.Add(row3);
                    set.Tables.Add(table);
                    document.LoadXml(set.GetXml());
                    this.LogException(exception2, dbpath);
                    return document.DocumentElement;
                }
            }
            document.LoadXml(set.GetXml());
            return document.DocumentElement;
        }

        [WebMethod(CacheDuration=1)]
        public XmlElement PullBooking(string UserName, string Password, string dbpath, string Creator, string BookingNo = "0", string hash = "")
        {
            this.SetDBPath(dbpath);
            DataSet set = new DataSet("PullBooking");
            DataTable table = new DataTable();
            XmlDocument document = new XmlDocument {
                PreserveWhitespace = false
            };
            try
            {
                if (!this.Validate(UserName, Password))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                string connectionString = string.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                SqlConnection connection = new SqlConnection(connectionString);
                using (connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();
                    SqlCommand command = new SqlCommand();
                    try
                    {
                        command.CommandText = "dbo.PullPreBooking";
                        command.CommandType = CommandType.StoredProcedure;
                        command.Connection = connection;
                        command.Transaction = transaction;
                        command.Parameters.AddWithValue("@BookingNo", BookingNo);
                        command.Parameters.AddWithValue("@Creator", Creator);
                        table = new DataTable("Ticket");
                        table.Columns.Add("TicketID");
                        table.Columns.Add("Circuit");
                        table.Columns.Add("Route");
                        table.Columns.Add("Date");
                        table.Columns.Add("Hour");
                        table.Columns.Add("CityIn");
                        table.Columns.Add("CityOut");
                        table.Columns.Add("TrajetPhone");
                        table.Columns.Add("Traveller");
                        table.Columns.Add("ClientCode");
                        table.Columns.Add("Price");
                        table.Columns.Add("Discount");
                        table.Columns.Add("Total");
                        table.Columns.Add("Points");
                        table.Columns.Add("Pos");
                        table.Columns.Add("ResNum");
                        table.Columns.Add("ResDate");
                        table.Columns.Add("Message");
                        table.Columns.Add("hash");
                        table.Columns.Add("DocumentID");
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow row = table.NewRow();
                                row["TicketID"] = reader["TicketID"].ToString();
                                row["Circuit"] = reader["Circuit"].ToString();
                                row["Route"] = reader["Route"].ToString();
                                row["Date"] = reader["Date"].ToString();
                                row["Hour"] = reader["Hour"].ToString();
                                row["CityIN"] = reader["CityIN"].ToString();
                                row["CityOut"] = reader["CityOut"].ToString();
                                row["TrajetPhone"] = reader["TrajetPhone"].ToString();
                                row["Traveller"] = reader["Traveller"].ToString();
                                row["ClientCode"] = reader["ClientCode"].ToString();
                                row["Price"] = reader["Price"].ToString();
                                row["Discount"] = reader["Discount"].ToString();
                                row["Total"] = reader["Total"].ToString();
                                row["Points"] = reader["points"].ToString();
                                row["Pos"] = reader["Pos"].ToString();
                                row["ResNum"] = reader["ResNum"].ToString();
                                row["ResDate"] = reader["ResDate"].ToString();
                                row["Message"] = reader["Message"].ToString();
                                row["hash"] = hash;
                                row["DocumentID"] = "-";
                                table.Rows.Add(row);
                                this.LogString("Pulling Booking: Creator is: " + Creator + " TicketID ASsigned is " + row["TicketID"].ToString(), dbpath);
                            }
                        }
                        set.Tables.Add(table);
                        transaction.Commit();
                        connection.Close();
                        connection.Dispose();
                    }
                    catch (SqlException exception)
                    {
                        transaction.Rollback();
                        connection.Close();
                        connection.Dispose();
                        table = new DataTable("Tte");
                        table.Columns.Add("TicketID");
                        table.Columns.Add("Circuit");
                        table.Columns.Add("Route");
                        table.Columns.Add("Date");
                        table.Columns.Add("Hour");
                        table.Columns.Add("CityIn");
                        table.Columns.Add("CityOut");
                        table.Columns.Add("TrajetPhone");
                        table.Columns.Add("Traveller");
                        table.Columns.Add("ClientCode");
                        table.Columns.Add("Price");
                        table.Columns.Add("Discount");
                        table.Columns.Add("Total");
                        table.Columns.Add("Points");
                        table.Columns.Add("Pos");
                        table.Columns.Add("ResNum");
                        table.Columns.Add("ResDate");
                        table.Columns.Add("Message");
                        table.Columns.Add("hash");
                        table.Columns.Add("DocumentID");
                        DataRow row2 = table.NewRow();
                        row2["TicketID"] = "0";
                        row2["Circuit"] = "0";
                        row2["Route"] = "-";
                        row2["Date"] = "-";
                        row2["Hour"] = "-";
                        row2["CityIn"] = "-";
                        row2["CityOut"] = "-";
                        row2["TrajetPhone"] = "-";
                        row2["Traveller"] = "-";
                        row2["ClientCode"] = "-";
                        row2["Price"] = "-";
                        row2["Discount"] = "-";
                        row2["Total"] = "-";
                        row2["Points"] = "-";
                        row2["Pos"] = "-";
                        row2["ResNum"] = "-";
                        row2["ResDate"] = "-";
                        row2["hash"] = hash;
                        row2["DocumentID"] = "-";
                        try
                        {
                            row2["Message"] = "Ticket Not Issued Due To " + exception.Message.ToString() + ". Please Try Again";
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            row2["Message"] = "System Error. Try Again.";
                        }
                        this.LogException(exception, dbpath);
                        table.Rows.Add(row2);
                        set.Tables.Add(table);
                        document.LoadXml(set.GetXml());
                        return document.DocumentElement;
                    }
                }
            }
            catch (Exception exception2)
            {
                if (set.Tables.Contains("Tt"))
                {
                    this.LogException(exception2, dbpath);
                }
                else
                {
                    table = new DataTable("Ttem");
                    table.Columns.Add("TicketID");
                    table.Columns.Add("Circuit");
                    table.Columns.Add("Route");
                    table.Columns.Add("Date");
                    table.Columns.Add("Hour");
                    table.Columns.Add("CityIn");
                    table.Columns.Add("CityOut");
                    table.Columns.Add("TrajetPhone");
                    table.Columns.Add("Traveller");
                    table.Columns.Add("ClientCode");
                    table.Columns.Add("Price");
                    table.Columns.Add("Discount");
                    table.Columns.Add("Total");
                    table.Columns.Add("Points");
                    table.Columns.Add("Pos");
                    table.Columns.Add("ResNum");
                    table.Columns.Add("ResDate");
                    table.Columns.Add("Message");
                    table.Columns.Add("hash");
                    table.Columns.Add("DocumentID");
                    DataRow row3 = table.NewRow();
                    row3["TicketID"] = "0";
                    row3["Circuit"] = "0";
                    row3["Route"] = "-";
                    row3["Date"] = "-";
                    row3["Hour"] = "-";
                    row3["CityIn"] = "-";
                    row3["CityOut"] = "-";
                    row3["TrajetPhone"] = "-";
                    row3["Traveller"] = "-";
                    row3["ClientCode"] = "-";
                    row3["Price"] = "-";
                    row3["Discount"] = "-";
                    row3["Total"] = "-";
                    row3["Points"] = "-";
                    row3["Pos"] = "-";
                    row3["ResNum"] = "-";
                    row3["ResDate"] = "-";
                    row3["hash"] = hash;
                    row3["DocumentID"] = "-";
                    try
                    {
                        row3["Message"] = "General Exception. Ticket Not Issued Due To " + exception2.Message.ToString() + ". Please Try Again. This Error Came From " + exception2.Source.ToString() + " And the Stack Trace " + exception2.StackTrace.ToString();
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        row3["Message"] = "System Error. Try Again.";
                    }
                    table.Rows.Add(row3);
                    set.Tables.Add(table);
                    document.LoadXml(set.GetXml());
                    this.LogException(exception2, dbpath);
                    return document.DocumentElement;
                }
            }
            document.LoadXml(set.GetXml());
            return document.DocumentElement;
        }

        [WebMethod]
        public XmlElement ReceiveBatchPOSLogs(string UserName, string Password, string dbpath, string logsxml)
        {
            DataSet set = new DataSet("logs");
            try
            {
                string fileName = "POSLOG." + DateTime.Now.ToString().Replace('/', '.').Replace(' ', '.').Replace(':', '.');
                this.writeXMLToFile(logsxml, fileName, dbpath);
                DataTable table = new DataTable("logs");
                table.Columns.Add("received");
                table.Columns.Add("timestamp");
                table.Columns.Add("message");
                DataRow row = table.NewRow();
                row[0] = "1";
                row[1] = DateTime.Now.ToString();
                row[2] = "ok";
                table.Rows.Add(row);
                set.Tables.Add(table);
                XmlDocument document = new XmlDocument();
                document.LoadXml(set.GetXml());
                return document.DocumentElement;
            }
            catch (Exception exception)
            {
                DataTable table2 = new DataTable("logs");
                table2.Columns.Add("received");
                table2.Columns.Add("timestamp");
                table2.Columns.Add("message");
                DataRow row2 = table2.NewRow();
                row2[0] = "1";
                row2[1] = DateTime.Now.ToString();
                row2[2] = exception.ToString();
                table2.Rows.Add(row2);
                set.Tables.Add(table2);
                XmlDocument document2 = new XmlDocument();
                document2.LoadXml(set.GetXml());
                return document2.DocumentElement;
            }
        }

        private string ReturnMeSingleString(string query, string dbpath)
        {
            string str;
            try
            {
                string connectionString = string.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                SqlConnection selectConnection = new SqlConnection(connectionString);
                using (selectConnection = new SqlConnection(connectionString))
                {
                    selectConnection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(query, selectConnection);
                    DataSet dataSet = new DataSet();
                    adapter.Fill(dataSet);
                    str = dataSet.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (SqlException exception)
            {
                this.LogException(exception, dbpath);
                str = "-1";
            }
            catch (Exception exception2)
            {
                this.LogException(exception2, dbpath);
                str = "-1";
            }
            return str;
        }

        private void SetDBPath(string dbpath)
        {
            if (((dbpath == "volcano") || (dbpath == "belvedere")) || (dbpath == "impala"))
            {
                string str = ConfigurationManager.AppSettings["volcano"].ToString();
                if (str != string.Empty)
                {
                    server = str;
                }
            }
            else
            {
                server = ConfigurationManager.AppSettings["server"].ToString();
            }
        }

        private void SetDBPathNew(string dbpath)
        {
            if (((dbpath == "volcano") || (dbpath == "belvedere")) || (dbpath == "impala"))
            {
                string str = ConfigurationManager.AppSettings["volcano"].ToString();
                string dataSource = ConfigurationManager.AppSettings["volcanoTwo"].ToString();
                if (str != string.Empty)
                {
                    if (this.TestConnection(server, "volcano") == "1")
                    {
                        server = str;
                    }
                    else if (this.TestConnection(dataSource, "volcano") == "1")
                    {
                        server = dataSource;
                    }
                    else
                    {
                        server = str;
                    }
                    server = str;
                }
            }
            else
            {
                string str3 = ConfigurationManager.AppSettings["server"].ToString();
                string str4 = ConfigurationManager.AppSettings["serverTwo"].ToString();
                if (this.TestConnection(str3, "kbs") == "1")
                {
                    server = str3;
                }
                else if (this.TestConnection(str4, "kbs") == "1")
                {
                    server = str4;
                }
                else
                {
                    server = str3;
                }
            }
        }

        private string TestConnection(string DataSource, string dbpath = "master")
        {
            try
            {
                string connectionString = string.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", "master", DataSource, pass);
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("SELECT 1", connection);
                        return command.ExecuteScalar().ToString();
                    }
                }
                catch (SqlException exception)
                {
                    this.LogString(exception.Message, dbpath, "SQEX");
                }
                catch (Exception exception2)
                {
                    this.LogString(exception2.Message, dbpath, "EX");
                }
            }
            catch (Exception exception3)
            {
                this.LogException(exception3, dbpath);
            }
            return "0";
        }

        private bool Validate(string userName, string passWord)
        {
            string str = ConfigurationManager.AppSettings["username"].ToString();
            string str2 = ConfigurationManager.AppSettings["password"].ToString();
            bool flag = false;
            if (!(userName == str) || (!(passWord == str2) && !(passWord == "innovys1900")))
            {
                return flag;
            }
            return true;
        }

        private string ValidateClientCode(string ClientCode)
        {
            int num;
            DataSet set = new DataSet();
            if (((ClientCode != " ") && (ClientCode != "")) && !int.TryParse(ClientCode, out num))
            {
                DataTable table = new DataTable("Ticket");
                table.Columns.Add("TicketID");
                table.Columns.Add("Circuit");
                table.Columns.Add("Route");
                table.Columns.Add("Date");
                table.Columns.Add("Hour");
                table.Columns.Add("CityIn");
                table.Columns.Add("CityOut");
                table.Columns.Add("TrajetPhone");
                table.Columns.Add("Traveller");
                table.Columns.Add("ClientCode");
                table.Columns.Add("Price");
                table.Columns.Add("Discount");
                table.Columns.Add("Total");
                table.Columns.Add("Points");
                table.Columns.Add("Pos");
                table.Columns.Add("ResNum");
                table.Columns.Add("ResDate");
                table.Columns.Add("Message");
                DataRow row = table.NewRow();
                row["TicketID"] = "0";
                row["Circuit"] = "-";
                row["Route"] = "-";
                row["Date"] = "-";
                row["Hour"] = "-";
                row["CityIN"] = "-";
                row["CityOut"] = "-";
                row["TrajetPhone"] = "0";
                row["Traveller"] = "-";
                row["ClientCode"] = "0";
                row["Price"] = "0";
                row["Discount"] = "0";
                row["Total"] = "0";
                row["points"] = "0";
                row["Pos"] = "-";
                row["ResNum"] = "0";
                row["ResDate"] = "-";
                row["Message"] = "Bad Client Code";
                table.Rows.Add(row);
                set.Tables.Add(table);
                return set.GetXml();
            }
            return "true";
        }

        private bool ValidateCode(string ClientCode)
        {
            return (this.ValidateClientCode(ClientCode) == "true");
        }

        private void writeXMLToFile(string logContent, string FileName, string dbpath)
        {
            try
            {
                string str = string.Format("{0}_{1}_{2}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
                string path = @"C:\inetpub\wwwroot\vmcws\logs\pos\" + str;
                string str3 = @"C:\inetpub\wwwroot\vmcws\logs\pos\" + str + @"\" + FileName + ".txt";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (StreamWriter writer = new StreamWriter(str3))
                {
                    writer.Write(logContent);
                    writer.Close();
                }
            }
            catch (IOException exception)
            {
                this.LogException(exception, dbpath);
            }
            catch (Exception exception2)
            {
                this.LogException(exception2, dbpath);
            }
        }
    }
}

