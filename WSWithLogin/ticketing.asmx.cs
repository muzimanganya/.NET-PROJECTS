using System;
//using System.Linq;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Configuration;
using System.IO;
using WSWithLogin.Models;
using System.Security.Cryptography;
using System.Text;

namespace SynovysWebService
{
    /// <summary>
    /// Summary description for ticketing
    /// </summary>
    [WebService(Namespace = "http://www.innovys.co.rw")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ticketing : System.Web.Services.WebService
    {

        static string server = ConfigurationManager.AppSettings["server"].ToString();
        static string pass = ConfigurationManager.AppSettings["sqlpass"].ToString();
        string currentUserFullName = "";

        [WebMethod(CacheDuration = 1)]
        public System.Xml.XmlElement POSLogin(String UserName, String Password, String dbpath, String creator)
        {
            //check if password is expired
            int checkLogin = ValidatePassword(UserName, Password, creator);

            string s = string.Format(" Username: {0} - Password:{1}", UserName, Password);
            //LogString(s);
            DataSet ds = new DataSet("Logins");
            DataTable dt = new DataTable("Login");

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;

            dt.Columns.Add("success");
            dt.Columns.Add("id");


            DataRow dr = dt.NewRow();

            if (checkLogin == 1)
            {
                dr[0] = "1";
                dr[1] = UserName;
                dt.Rows.Add(dr);
            }
            else if (checkLogin == -1)
            {
                dr[0] = "-1";
                dr[1] = UserName;
                dt.Rows.Add(dr);
            }
            else
            {
                dr[0] = "0";
                dr[1] = UserName;
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);

            doc.LoadXml(ds.GetXml());
            return doc.DocumentElement;
        }

        [WebMethod(CacheDuration = 1)]
        public System.Xml.XmlElement POSChgPassword(String UserName, String OldPassword, String NewPassword, String dbpath, String creator)
        {
            String s = String.Format("User-{0} OPass-{1} NPass-{2} POS-{3}", UserName, OldPassword, NewPassword, creator);
            LogString(s);
            bool success = false; //check if changing password succeded
            //check if password is expired
            int checkLogin = ValidatePassword(UserName, OldPassword, creator);
            if (checkLogin == 1 || checkLogin == -1)
            {
                //user exists and Old Password is valid. Change it
                //return types (0) - failed (1)-okay (-1) - change password

                //get SHA256 of the Password
                SHA256Managed crypt = new SHA256Managed();
                StringBuilder hash = new StringBuilder();
                byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(NewPassword), 0, Encoding.UTF8.GetByteCount(NewPassword));
                foreach (byte bit in crypto)
                {
                    hash.Append(bit.ToString("x2"));
                }

                using (var context = new AppDBContext())
                {
                    SystemUser u = context.SystemUsers.Find(UserName.ToLower());
                    if (u != null)
                    {
                        u.LastChangedPass = DateTime.Now;
                        u.Password = hash.ToString();
                        context.SaveChanges();
                        success = true;
                        LogString(String.Format("--- User Found: {0} --", u.ToString()));
                    }
                }
            }

            DataSet ds = new DataSet("Logins");
            DataTable dt = new DataTable("Login");

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;

            dt.Columns.Add("success");
            dt.Columns.Add("id");


            DataRow dr = dt.NewRow();

            if (success)
            {
                dr[0] = "1";
                dr[1] = UserName;
                dt.Rows.Add(dr);
            }
            else
            {
                dr[0] = "0";
                dr[1] = UserName;
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            LogString(doc.ToString());

            doc.LoadXml(ds.GetXml());
            return doc.DocumentElement;
        }


        [WebMethod(CacheDuration = 1)]
        public System.Xml.XmlElement POSOff(String UserName, String dbpath, String creator)
        {
            DataSet ds = new DataSet("Logoffs");
            DataTable dt = new DataTable("Logoff");

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;

            dt.Columns.Add("success");
            dt.Columns.Add("id");

            DataRow dr = dt.NewRow();

            dr[0] = "1";
            dr[1] = "1";

            dt.Rows.Add(dr);

            ds.Tables.Add(dt);

            doc.LoadXml(ds.GetXml());
            return doc.DocumentElement;
        }

        //[WebMethod]
        public System.Xml.XmlElement GetTigoCashTicket(String UserName, String Password, String dbpath, String Creator, String TicketID, String hash, String PosUser)
        {
            SetDBPath(dbpath);

            #region Declarations

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;

            String conString = String.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
            SqlConnection sqlConnection1;
            SqlTransaction transaction;
            SqlCommand cmd = new SqlCommand();

            DataSet dsTT = new DataSet();
            DataTable dt;
            SqlDataReader reader;

            #endregion


            #region Add DataTable Columns

            dt = new DataTable("Ticket");
            dt.Columns.Add("TktID");
            dt.Columns.Add("Cct");
            dt.Columns.Add("Rte");
            dt.Columns.Add("Date");
            dt.Columns.Add("Hr");
            dt.Columns.Add("CIn");
            dt.Columns.Add("COut");
            dt.Columns.Add("TPhone");
            dt.Columns.Add("Traveller");
            dt.Columns.Add("CCode");
            dt.Columns.Add("Price");
            dt.Columns.Add("Disc");
            dt.Columns.Add("Ttl");
            dt.Columns.Add("Pnts");
            dt.Columns.Add("Pos");
            dt.Columns.Add("RNum");
            dt.Columns.Add("RDate");
            dt.Columns.Add("Msg");
            dt.Columns.Add("hash");


            #endregion



            using (sqlConnection1 = new SqlConnection(conString))
            {
                #region Try the Process

                try
                {
                    #region Validate Username and Password

                    if (!Validate(UserName, Password, PosUser))
                    {
                        Exception up = new Exception("Wrong UserName or Password");
                        throw up; //ha ha
                    }

                    #endregion

                    sqlConnection1.Open();

                    #region Catch And Roll

                    using (transaction = sqlConnection1.BeginTransaction())
                    {
                        try
                        {
                            #region Set up Stored Proc Parameters

                            cmd.CommandText = "su.GET_TIGOCASH_TICKET";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = sqlConnection1;
                            cmd.Transaction = transaction;

                            cmd.Parameters.AddWithValue("@TICKET", TicketID);
                            cmd.Parameters.AddWithValue("@CREATOR", Creator);


                            #endregion

                            #region Execute Reader Get Results

                            using (reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    DataRow rw = dt.NewRow();
                                    rw["TktID"] = reader["TicketID"].ToString();
                                    rw["Cct"] = reader["Circuit"].ToString();
                                    rw["Rte"] = reader["Route"].ToString();
                                    rw["Date"] = reader["Date"].ToString();
                                    rw["Hr"] = reader["Hour"].ToString();
                                    rw["CIn"] = reader["CityIN"].ToString();
                                    rw["COut"] = reader["CityOut"].ToString();
                                    rw["TPhone"] = reader["TrajetPhone"].ToString();
                                    rw["Traveller"] = reader["Traveller"].ToString();
                                    rw["CCode"] = reader["ClientCode"].ToString();
                                    rw["Price"] = reader["Price"].ToString();
                                    rw["Disc"] = reader["Discount"].ToString();
                                    rw["Ttl"] = reader["Total"].ToString();
                                    rw["Pnts"] = reader["points"].ToString();
                                    rw["Pos"] = reader["Pos"].ToString();
                                    rw["RNum"] = reader["ResNum"].ToString();
                                    rw["RDate"] = reader["ResDate"].ToString();
                                    rw["Msg"] = reader["Message"].ToString();
                                    rw["hash"] = hash;
                                    dt.Rows.Add(rw);
                                }
                            }

                            #endregion

                            #region Commit Transaction, Add DataTable to DataSet

                            transaction.Commit();
                            dsTT.Tables.Add(dt);
                            doc.LoadXml(dsTT.GetXml());

                            #endregion
                        }
                        catch (Exception e)
                        {
                            #region Set up the Response

                            DataRow rw = dt.NewRow();

                            rw["TktID"] = "0";
                            rw["Cct"] = "-";
                            rw["Rte"] = "-";
                            rw["Date"] = "-";
                            rw["Hr"] = "-";
                            rw["CIn"] = "-";
                            rw["COut"] = "-";
                            rw["TPhone"] = "-";
                            rw["Traveller"] = "-";
                            rw["CCode"] = "-";
                            rw["Price"] = "0";
                            rw["Disc"] = "0";
                            rw["Ttl"] = "0";
                            rw["Pnts"] = "0";
                            rw["Pos"] = Creator;
                            rw["RNum"] = "5";
                            rw["RDate"] = "-";
                            rw["hash"] = hash;
                            rw["Msg"] = "System Error. Try Again";

                            dt.Rows.Add(rw);

                            #endregion

                            #region Rollback Transaction

                            transaction.Rollback();
                            dsTT.Tables.Add(dt);
                            doc.LoadXml(dsTT.GetXml());

                            #endregion

                            #region Log the Error

                            LogException(e, dbpath);

                            #endregion

                            return doc.DocumentElement;
                        }

                    }


                    #endregion
                }

                #endregion

                #region Catch An Error

                catch (Exception ex)
                {
                    #region Set up the Response

                    DataRow rw = dt.NewRow();

                    rw["TktID"] = "0";
                    rw["Cct"] = "-";
                    rw["Rte"] = "-";
                    rw["Date"] = "-";
                    rw["Hr"] = "-";
                    rw["CIn"] = "-";
                    rw["COut"] = "-";
                    rw["TPhone"] = "-";
                    rw["Traveller"] = "-";
                    rw["CCode"] = "-";
                    rw["Price"] = "0";
                    rw["Disc"] = "0";
                    rw["Ttl"] = "0";
                    rw["Pnts"] = "0";
                    rw["Pos"] = Creator;
                    rw["RNum"] = "5";
                    rw["RDate"] = "-";
                    rw["hash"] = hash;
                    rw["Msg"] = "System Error. Try Again";

                    dt.Rows.Add(rw);

                    #endregion

                    #region Set up XMl response

                    dsTT.Tables.Add(dt);
                    doc.LoadXml(dsTT.GetXml());

                    #endregion

                    #region Log the Error

                    LogException(ex, dbpath);

                    #endregion

                    return doc.DocumentElement;
                }

                #endregion

            }


            return doc.DocumentElement;
        }
        [WebMethod]
        public System.Xml.XmlElement ReceiveBatchPOSLogs(String UserName, String Password, String dbpath, String logsxml)
        {
            DataSet ds = new DataSet("logs");
            try
            {
                String randomFilename = "POSLOG." + DateTime.Now.ToString().Replace('/', '.').Replace(' ', '.').Replace(':', '.');



                writeXMLToFile(logsxml, randomFilename, dbpath);

                DataTable dt = new DataTable("logs");

                dt.Columns.Add("received");
                dt.Columns.Add("timestamp");
                dt.Columns.Add("Msg");

                DataRow rw = dt.NewRow();

                rw[0] = "1";
                rw[1] = DateTime.Now.ToString();
                rw[2] = "ok";

                dt.Rows.Add(rw);

                ds.Tables.Add(dt);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(ds.GetXml());

                return doc.DocumentElement;
            }
            catch (Exception ex)
            {

                DataTable dt = new DataTable("logs");

                dt.Columns.Add("received");
                dt.Columns.Add("timestamp");
                dt.Columns.Add("Msg");

                DataRow rw = dt.NewRow();

                rw[0] = "1";
                rw[1] = DateTime.Now.ToString();
                rw[2] = ex.ToString();

                dt.Rows.Add(rw);

                ds.Tables.Add(dt);

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(ds.GetXml());

                return doc.DocumentElement;
            }
        }
        [WebMethod(CacheDuration = 1)]

        public System.Xml.XmlElement GetTicket(String UserName, String Password, String dbpath, String Date, String Hour, String CityIN, String CityOut,
            String Discount, String Creator, String Name = "", String Firstname = "", String ClientCode = "", String TicketID = "0", String currency = "RWF",
            string hash = "", String pref = "0", String BusType = "EXPRESS", String DocumentID = "", String PosUser = "")
        {
            SetDBPath(dbpath);
            DataSet ds = new DataSet("Tickets");
            DataTable dt = new DataTable();
            SqlConnection sqlConnection1;
            SqlTransaction transaction;
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
           try
            {
                if (!Validate(UserName, Password, PosUser))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                //Validate to make sure Client Code Contains Numbers only
                if (!ValidateCode(ClientCode))
                {
                    doc.LoadXml(ValidateClientCode(ClientCode));
                    return doc.DocumentElement;
                }
                String conString = String.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                sqlConnection1 = new SqlConnection(conString);
                using (sqlConnection1 = new SqlConnection(conString))
                {
                    sqlConnection1.Open();
                    //Check the TicketID served if empty then check the most previous ticketid from this pos and remove it
                    String queryLatest = String.Format("SELECT IDRELATION FROM dbo.LASTID WHERE POS='{0}' AND DAY(DAY) = DAY(GETDATE())", Creator);
                    SqlTransaction transactionRoll;
                    SqlConnection con;
                    try
                    {
                        String lastID = ReturnMeSingleString(queryLatest, dbpath);
                        //LogString("Creator is: " + Creator + " : Last ID is " + lastID + "   :TicketID Sent is: " + TicketID, dbpath);
                        //MessageBox.Show("The LastID is " + lastID);
                        if (lastID == String.Empty || lastID == "-1")
                        {
                            //This is the first ticket today so forget checking many things or there was an error retreiving the last ticketID
                            //Forget checking this since we failed to get the Last TicketID
                        }
                        else if (lastID == TicketID)
                        {
                            //The TicketID the POS sent is the same so the ticket was indeed received
                            //Do nothing
                        }
                        else if (TicketID == String.Empty || TicketID == "0" || TicketID == "-1" || TicketID == " " || TicketID.Length < 3)
                        {
                            //Donot bother to check
                        }
                        else if (TicketID != lastID)
                        {
                            //So here the pos sent an id different from the last recorded so 
                            //Either return that specific Ticket or Remove that Lastticket because it did not reach the POS
                            SqlCommand cmdRollback = new SqlCommand();

                            using (con = new SqlConnection(conString))
                            {
                                con.Open();
                                transactionRoll = con.BeginTransaction();
                                cmdRollback.CommandText = "su.TicketRollback";
                                cmdRollback.CommandType = CommandType.StoredProcedure;
                                cmdRollback.Connection = con;
                                cmdRollback.Transaction = transactionRoll;

                                cmdRollback.Parameters.AddWithValue("@TICKETID", lastID);
                                try
                                {
                                    cmdRollback.ExecuteNonQuery();
                                    transactionRoll.Commit();
                                }
                                catch (SqlException exxx)
                                {
                                    transactionRoll.Rollback();
                                    LogException(exxx, dbpath);

                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Logs\YahooWS.txt", true))
                                    {
                                        file.WriteLine(exxx.ToString());
                                    }

                                }
                                catch (Exception exxxxx)
                                {
                                    transactionRoll.Rollback();
                                    LogException(exxxxx, dbpath);

                                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Logs\YahooWS.txt", true))
                                    {
                                        file.WriteLine(exxxxx.ToString());
                                    }

                                }
                            }
                            String rollbackStatement = String.Format("Creator: {0}, Route: {1}-{2}, Hour: {8}, Client Code: {3}, Name: {4} {5}, TicketID Rolledback: {6}, TicketID Sent: {7}", Creator, CityIN, CityOut, ClientCode, Firstname, Name, lastID, TicketID, Hour);
                            //LogString(rollbackStatement, dbpath);
                            //LogString(rollbackStatement, dbpath + "Rolls");
                        }
                    }
                    catch (SqlException ex)
                    {
                        LogException(ex, dbpath);

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Logs\YahooWS.txt", true))
                        {
                            file.WriteLine(ex.ToString());
                        }

                    }
                    catch (Exception exex)
                    {
                        LogException(exex, dbpath);

                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Logs\YahooWS.txt", true))
                        {
                            file.WriteLine(exex.ToString());
                        }

                    }
                    transaction = sqlConnection1.BeginTransaction();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;
                    try
                    {
                        cmd.CommandText = "su.Ticketing";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection1;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@DATE", Date);
                        cmd.Parameters.AddWithValue("@HOUR", Hour);
                        cmd.Parameters.AddWithValue("@CityIN", CityIN);
                        cmd.Parameters.AddWithValue("@CityOut", CityOut);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Firstname", Firstname);
                        cmd.Parameters.AddWithValue("@ClientCode", ClientCode);
                        cmd.Parameters.AddWithValue("@Discount", Discount);
                        cmd.Parameters.AddWithValue("@Creator", Creator);
                        cmd.Parameters.AddWithValue("@Currency", currency);
                        cmd.Parameters.AddWithValue("@PosUser", PosUser);

                        if (pref != "0")
                        {
                            //They want a preferred Seat
                            cmd.Parameters.AddWithValue("@PREFSEAT", pref);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@PREFSEAT", "Yes");
                        }

                        if (DocumentID.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@Passport", DocumentID);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Passport", "");
                        }

                        if (BusType == "OT" || BusType.ToLower().Contains("other"))
                        {
                            //The default
                            cmd.Parameters.AddWithValue("@BusCategory", "OTHER");
                        }
                        else if(BusType.ToLower().Contains("express"))
                        {
                            cmd.Parameters.AddWithValue("@BusCategory", "EXPRESS"); 
                        }
                        else
                        {
                             String type = "EXPRESS";
                             if (BusType != null)
                                 type = BusType.Replace("bus", "").Replace("Bus", "").Replace("BUS", ""); 
                            cmd.Parameters.AddWithValue("@BusCategory", type.Trim()); 
                        }

                        //TEST
                        //String query = "";
                        //foreach (SqlParameter p in cmd.Parameters)
                        //{
                        //    query += "[ " + p.ParameterName + " - " + p.Value.ToString() + " ]";
                        //}
                        //LogMe(query);
                        //ENDTEST

                        //reader = cmd.ExecuteReader();
                        dt = new DataTable("Ticket");
                        dt.Columns.Add("TktID");
                        dt.Columns.Add("Cct");
                        dt.Columns.Add("Rte");
                        dt.Columns.Add("Date");
                        dt.Columns.Add("Hr");
                        dt.Columns.Add("CIn");
                        dt.Columns.Add("COut");
                        dt.Columns.Add("TPhone");
                        dt.Columns.Add("Traveller");
                        dt.Columns.Add("CCode");
                        dt.Columns.Add("Price");
                        dt.Columns.Add("Disc");
                        dt.Columns.Add("Ttl");
                        dt.Columns.Add("Pnts");
                        dt.Columns.Add("Pos");
                        dt.Columns.Add("RNum");
                        dt.Columns.Add("RDate");
                        dt.Columns.Add("Msg");
                        dt.Columns.Add("hash");
                        dt.Columns.Add("SNo");
                        dt.Columns.Add("DTime");
                        dt.Columns.Add("DocID");

                        //String myan = reader.ToString(); 
                        using (reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow rw = dt.NewRow();
                                rw["TktID"] = reader["TicketID"].ToString();
                                rw["Cct"] = reader["Circuit"].ToString();
                                rw["Rte"] = reader["Route"].ToString();
                                rw["Date"] = reader["Date"].ToString();
                                rw["Hr"] = reader["Hour"].ToString();
                                rw["CIn"] = reader["CityIN"].ToString();
                                rw["COut"] = reader["CityOut"].ToString();
                                rw["TPhone"] = reader["TrajetPhone"].ToString();
                                rw["Traveller"] = reader["Traveller"].ToString();
                                rw["CCode"] = reader["ClientCode"].ToString();
                                rw["Price"] = reader["Price"].ToString();
                                rw["Disc"] = reader["Discount"].ToString();
                                rw["Ttl"] = reader["Total"].ToString();
                                rw["Pnts"] = reader["points"].ToString();
                                rw["Pos"] = reader["Pos"].ToString();
                                rw["RNum"] = reader["ResNum"].ToString();
                                rw["RDate"] = reader["ResDate"].ToString();
                                rw["Msg"] = reader["Message"].ToString();
                                rw["hash"] = hash;
                                rw["SNo"] = reader["SeatNo"].ToString();
                                rw["DTime"] = reader["DTime"].ToString();
                                rw["DocID"] = DocumentID;

                                //activate only when PROC returns Platenumber else ignore it!
                                if(HasColumn(reader, "PLATENO"))
                                {
                                    if(!dt.Columns.Contains("PLATENO"))
                                        dt.Columns.Add("PLATENO");

                                    rw["PLATENO"] = reader["PLATENO"].ToString();
                                }
                                dt.Rows.Add(rw);
                                //LogString("Normal Ticketing: Creator is: " + Creator + " TicketID ASsigned is " + rw["TktID"].ToString(), dbpath);
                            }
                      }

                        //reader.Dispose();
                        ds.Tables.Add(dt);
                        transaction.Commit();
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();
                    }
                    catch (SqlException e)
                    {
                        LogMe(e.ToString());

                        transaction.Rollback();
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();

                        //Return A valid Messsage After this to Ensure They Don't Get Confused
                        dt = new DataTable("Ticket");
                        dt.Columns.Add("TktID");
                        dt.Columns.Add("Cct");
                        dt.Columns.Add("Rte");
                        dt.Columns.Add("Date");
                        dt.Columns.Add("Hr");
                        dt.Columns.Add("CIn");
                        dt.Columns.Add("COut");
                        dt.Columns.Add("TPhone");
                        dt.Columns.Add("Traveller");
                        dt.Columns.Add("CCode");
                        dt.Columns.Add("Price");
                        dt.Columns.Add("Disc");
                        dt.Columns.Add("Ttl");
                        dt.Columns.Add("Pnts");
                        dt.Columns.Add("Pos");
                        dt.Columns.Add("RNum");
                        dt.Columns.Add("RDate");
                        dt.Columns.Add("Msg");
                        dt.Columns.Add("hash");
                        dt.Columns.Add("SNo");
                        dt.Columns.Add("DTime");
                        dt.Columns.Add("DocID");

                        DataRow rw = dt.NewRow();
                        rw["TktID"] = "0";
                        rw["Cct"] = "0";
                        rw["Rte"] = "-";
                        rw["Date"] = "-";
                        rw["Hr"] = "-";
                        rw["CIn"] = "-";
                        rw["COut"] = "-";
                        rw["TPhone"] = "-";
                        rw["Traveller"] = "-";
                        rw["CCode"] = "-";
                        rw["Price"] = "-";
                        rw["Disc"] = "-";
                        rw["Ttl"] = "-";
                        rw["Pnts"] = "-";
                        rw["Pos"] = "-";
                        rw["RNum"] = "-";
                        rw["RDate"] = "-";
                        rw["hash"] = hash;
                        rw["SNo"] = "-";
                        rw["DTime"] = "-";
                        rw["DocID"] = "-"; 

                        try
                        {
                            rw["Msg"] = "No Ticket.Error 10.Try Again"; 
                        }
                        catch (ArgumentOutOfRangeException exx)
                        {
                            rw["Msg"] = "System Error. Try Again."; 

                        }
                        LogException(e, dbpath);

                        ds.Tables.Clear();

                        dt.Rows.Add(rw);
                        ds.Tables.Add(dt);
                        doc.LoadXml(ds.GetXml());
                        return doc.DocumentElement;
                    }
                }

           }

            catch (Exception e)
           {
               LogMe(e.ToString());

                if (ds.Tables.Contains("Ticket"))
                {
                    //Here a ticket has been issued successfully but there was a minor exception so don't bother returning that exception
                    LogException(e, dbpath);
                }
                else
                {
                    dt = new DataTable("Ticket");
                    dt.Columns.Add("TktID");
                    dt.Columns.Add("Cct");
                    dt.Columns.Add("Rte");
                    dt.Columns.Add("Date");
                    dt.Columns.Add("Hr");
                    dt.Columns.Add("CIn");
                    dt.Columns.Add("COut");
                    dt.Columns.Add("TPhone");
                    dt.Columns.Add("Traveller");
                    dt.Columns.Add("CCode");
                    dt.Columns.Add("Price");
                    dt.Columns.Add("Disc");
                    dt.Columns.Add("Ttl");
                    dt.Columns.Add("Pnts");
                    dt.Columns.Add("Pos");
                    dt.Columns.Add("RNum");
                    dt.Columns.Add("RDate");
                    dt.Columns.Add("Msg");
                    dt.Columns.Add("hash");
                    dt.Columns.Add("SNo");
                    dt.Columns.Add("DTime");
                    dt.Columns.Add("DocID");

                    DataRow rw = dt.NewRow();
                    rw["TktID"] = "0";
                    rw["Cct"] = "0";
                    rw["Rte"] = "-";
                    rw["Date"] = "-";
                    rw["Hr"] = "-";
                    rw["CIn"] = "-";
                    rw["COut"] = "-";
                    rw["TPhone"] = "-";
                    rw["Traveller"] = "-";
                    rw["CCode"] = "-";
                    rw["Price"] = "-";
                    rw["Disc"] = "-";
                    rw["Ttl"] = "-";
                    rw["Pnts"] = "-";
                    rw["Pos"] = "-";
                    rw["RNum"] = "-";
                    rw["RDate"] = "-";

                    rw["hash"] = hash;
                    rw["SNo"] = "-";
                    rw["DTime"] = "-";
                    rw["DocID"] = "-";
                    try
                    {
                        rw["Msg"] = "No Ticket.Error 20.Try Again";
                        //rw["Msg"] = "Ticket Not Issued Due To " + e.Message.ToString().Substring(0, 15) + ". Please Try Again";
                        //rw["Msg"] = "General Exception. Ticket Not Issued Due To " + e.Message.ToString() + ". Please Try Again. This Error Came From "+e.Source.ToString()+" And the Stack Trace "+e.StackTrace.ToString();
                    }
                    catch (ArgumentOutOfRangeException exx)
                    {
                        rw["Msg"] = "System Error. Try Again."; 

                    }
                    dt.Rows.Add(rw);
                    ds.Tables.Add(dt);
                    doc.LoadXml(ds.GetXml());
                    //transaction.Rollback();
                    //sqlConnection1.Close();
                    //sqlConnection1.Dispose();
                    LogException(e, dbpath);
                    return doc.DocumentElement;
                }

            }

            finally
            {

            }

            doc.LoadXml(ds.GetXml());
            XmlNode ticket = doc.DocumentElement.FirstChild;
            //append Username
            using(var context = new AppDBContext())
            {
                SystemUser u = context.SystemUsers.Find(PosUser);
                if(u!=null)
                {
                    XmlNode userNode = doc.CreateElement("FullName");
                    userNode.InnerText = u.Fullname;
                    ticket.AppendChild(userNode);
                }
            }

            return doc.DocumentElement;
        }

        
        [WebMethod(CacheDuration = 1)]
        public XmlElement GetSalesReport(String UserName, String Password, String dbpath, String from, String to, String date, String hour, String PosUser = "")
        {
            SetDBPath(dbpath);
            DataSet ds = new DataSet("SalesReport");
            SqlConnection sqlConnection1;
            SqlTransaction transaction;
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            try
            {
                if (!Validate(UserName, Password, PosUser))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                String conString = String.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                using (sqlConnection1 = new SqlConnection(conString))
                {
                    sqlConnection1.Open();
                    transaction = sqlConnection1.BeginTransaction();
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        SqlDataReader reader;

                        cmd.CommandText = "dbo.GetSalesReport";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection1;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@FROM", from);
                        cmd.Parameters.AddWithValue("@TO", to);
                        cmd.Parameters.AddWithValue("@DATE", date);
                        cmd.Parameters.AddWithValue("@HOUR", hour);
                        cmd.Parameters.AddWithValue("@POSUSER", PosUser);

                        //sqlConnection1.Open();

                        //reader = cmd.ExecuteReader();

                        DataTable dt = new DataTable("SalesReport");
                        dt.Columns.Add("USER");
                        dt.Columns.Add("TICKETS");
                        dt.Columns.Add("REVENUE");
                        dt.Columns.Add("BUS");
                        dt.Columns.Add("PlateNo");   
                        //String myan = reader.ToString();
                        using (reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow rw = dt.NewRow();
                                rw["USER"] = PosUser.ToUpper();
                                rw["TICKETS"] = reader["TICKETS"].ToString();
                                rw["REVENUE"] = reader["REVENUE"].ToString().TrimEnd(',');
                                rw["BUS"] = from + " - " + to + "(@" + hour + ")";
                                rw["PlateNo"] = reader["PlateNo"].ToString(); 
                                dt.Rows.Add(rw);
                            }
                        }

                        transaction.Commit();
                        ds.Tables.Add(dt);
                        reader.Dispose();
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();

                    }
                    catch (SqlException ex)
                    {
                        LogMe(ex.ToString());

                        transaction.Rollback();
                        DataTable dt = new DataTable("SalesReport");
                        dt.Columns.Add("USER");
                        dt.Columns.Add("TICKETS");
                        dt.Columns.Add("REVENUE");
                        dt.Columns.Add("BUS");
                        dt.Columns.Add("PlateNo");  

                        DataRow rw = dt.NewRow();
                        rw["USER"] = PosUser;
                        rw["TICKETS"] = "Error";
                        rw["REVENUE"] = "-";
                        rw["BUS"] = "-";
                        rw["PlateNo"] = "-";  
                        dt.Rows.Add(rw);
                        ds.Tables.Add(dt);
                        LogException(ex, dbpath);
                    }

                }

            }

            catch (Exception e)
            {
                if (ds.Tables.Contains("SalesReport"))
                {
                    //The Subscription Process Went through but there's a minor exception so we avoid it
                    //We might never reach here by the way! 
                }
                else
                {
                    //Big time Error and Record was not processed through at all
                    DataTable dt = new DataTable("SalesReport");
                    dt.Columns.Add("USER");
                    dt.Columns.Add("TICKETS");
                    dt.Columns.Add("REVENUE");
                    dt.Columns.Add("BUS");
                    dt.Columns.Add("PlateNo");  
                    DataRow rw = dt.NewRow();
                    rw["USER"] = PosUser;
                    rw["TICKETS"] = "Error";
                    rw["REVENUE"] = "-";
                    rw["BUS"] = "-";
                    rw["PlateNo"] = "-"; 
                    dt.Rows.Add(rw);
                    ds.Tables.Add(dt);
                    LogException(e, dbpath);
                    doc.LoadXml(ds.GetXml());
                    return doc.DocumentElement;
                }

            }

            finally
            {
                //Not Yet Anything Here
            }

            doc.LoadXml(ds.GetXml());
            return doc.DocumentElement;
        }

        [WebMethod(CacheDuration = 1)]
        public XmlElement GetReport(String UserName, String Password, String dbpath, String pos, String PosUser = "")
        {
            SetDBPath(dbpath);
            DataSet ds = new DataSet("Subscriptions");
            SqlConnection sqlConnection1;
            SqlTransaction transaction;
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            try
            {
                if (!Validate(UserName, Password, PosUser))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                String conString = String.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                using (sqlConnection1 = new SqlConnection(conString))
                {
                    sqlConnection1.Open();
                    transaction = sqlConnection1.BeginTransaction();
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        SqlDataReader reader;

                        cmd.CommandText = "su.USER_VENDING";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection1;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@USER", PosUser);

                        //sqlConnection1.Open();

                        //reader = cmd.ExecuteReader();

                        DataTable dt = new DataTable("Subscription");
                        dt.Columns.Add("USER");
                        dt.Columns.Add("TICKETS");
                        dt.Columns.Add("CASH_TICKETING");
                        dt.Columns.Add("SUBSCRIPTIONS");
                        dt.Columns.Add("CASH_SUBSCRIPTION");
                        dt.Columns.Add("Ttl");
                        //String myan = reader.ToString();
                        using (reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow rw = dt.NewRow();
                                rw["USER"] = reader["USERNAME"].ToString();
                                rw["TICKETS"] = reader["TICKETS"].ToString();
                                rw["CASH_TICKETING"] = reader["TOTALBREAK"].ToString().TrimEnd(',');
                                rw["SUBSCRIPTIONS"] = reader["SUBSCRIPTIONS"].ToString();
                                rw["CASH_SUBSCRIPTION"] = reader["CASH_SUBSCRIPTION"].ToString();

                                String rTotal = reader["TOTAL"].ToString().TrimEnd(',');
                                rw["Ttl"] = rTotal;
                                dt.Rows.Add(rw);
                            }
                        }

                        transaction.Commit();
                        ds.Tables.Add(dt);
                        reader.Dispose();
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();

                    }
                    catch (SqlException ex)
                    {
                        LogMe(ex.ToString());

                        transaction.Rollback();
                        DataTable dt = new DataTable("Subscription");
                        dt.Columns.Add("USER");
                        dt.Columns.Add("TICKETS");
                        dt.Columns.Add("CASH_TICKETING");
                        dt.Columns.Add("SUBSCRIPTIONS");
                        dt.Columns.Add("CASH_SUBSCRIPTION");
                        dt.Columns.Add("Ttl");
                        DataRow rw = dt.NewRow();
                        rw["USER"] = PosUser;
                        rw["TICKETS"] = "Error";
                        rw["CASH_TICKETING"] = "-";
                        rw["SUBSCRIPTIONS"] = "-";
                        try
                        {

                            rw["CASH_SUBSCRIPTION"] = "Error 10. Try again";

                        }
                        catch (ArgumentOutOfRangeException exx)
                        {
                            rw["CASH_SUBSCRIPTION"] = "System Error.Try again";
                        }

                        rw["Ttl"] = "ERROR";
                        dt.Rows.Add(rw);
                        ds.Tables.Add(dt);
                        LogException(ex, dbpath);
                    }

                }

            }

            catch (Exception e)
            {
                if (ds.Tables.Contains("Subscription"))
                {
                    //The Subscription Process Went through but there's a minor exception so we avoid it
                    //We might never reach here by the way! 
                }
                else
                {
                    //Big time Error and Record was not processed through at all
                    DataTable dt = new DataTable("Subscription");
                    dt.Columns.Add("USERNAME");
                    dt.Columns.Add("TICKETS");
                    dt.Columns.Add("CASH_TICKETING");
                    dt.Columns.Add("SUBSCRIPTIONS");
                    dt.Columns.Add("CASH_SUBSCRIPTION");
                    dt.Columns.Add("Ttl");
                    DataRow rw = dt.NewRow();
                    rw["USERNAME"] = PosUser;
                    rw["TICKETS"] = "Error";
                    rw["CASH_TICKETING"] = "-";
                    rw["SUBSCRIPTIONS"] = "-";
                    try
                    {
                        rw["CASH_SUBSCRIPTION"] = "Error 20.Try again";

                    }
                    catch (ArgumentOutOfRangeException exx)
                    {
                        rw["CASH_SUBSCRIPTION"] = "System Error.Try again";
                    }

                    rw["Ttl"] = "ERROR";
                    dt.Rows.Add(rw);
                    ds.Tables.Add(dt);
                    LogException(e, dbpath);
                    doc.LoadXml(ds.GetXml());
                    return doc.DocumentElement;
                }

            }

            finally
            {
                //Not Yet Anything Here
            }

            doc.LoadXml(ds.GetXml());
            return doc.DocumentElement;
        }

        [WebMethod(CacheDuration = 1)]
        public XmlElement GetBusReport(String UserName, String Password, String dbpath, String Cityin, String CityOut, String Date, String Hour, String creator, String PosUser = "")
        {
            SetDBPath(dbpath);
            DataSet ds = new DataSet("Reports");
            SqlConnection sqlConnection1;
            SqlTransaction transaction;
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            try
            {
                if (!Validate(UserName, Password, PosUser))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                String conString = String.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                using (sqlConnection1 = new SqlConnection(conString))
                {
                    sqlConnection1.Open();
                    transaction = sqlConnection1.BeginTransaction();
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        SqlDataReader reader;

                        cmd.CommandText = "su.BUS_REPORT";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection1;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@CITYIN", Cityin);
                        cmd.Parameters.AddWithValue("@CITYOUT", CityOut);
                        cmd.Parameters.AddWithValue("@DATE", Date);
                        cmd.Parameters.AddWithValue("@HOUR", Hour);
                        cmd.Parameters.AddWithValue("@CREATOR", creator);

                        DataTable dt = new DataTable("Report");
                        dt.Columns.Add("BusName");
                        dt.Columns.Add("Tickets");
                        dt.Columns.Add("RWF");
                        dt.Columns.Add("FIB");
                        dt.Columns.Add("Creator");
                        dt.Columns.Add("Message");

                        using (reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow rw = dt.NewRow();
                                rw["BusName"] = reader["BUSNAME"].ToString();
                                rw["Tickets"] = reader["TotalTickets"].ToString();
                                rw["RWF"] = reader["TOTALRWF"].ToString();
                                rw["FIB"] = reader["TOTALFIB"].ToString().TrimEnd(',');
                                rw["Creator"] = creator;
                                rw["Message"] = reader["MESSAGE"].ToString();
                                dt.Rows.Add(rw);
                            }
                        }

                        transaction.Commit();
                        ds.Tables.Add(dt);
                        reader.Dispose();
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();

                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        DataTable dt = new DataTable("Report");
                        dt.Columns.Add("BusName");
                        dt.Columns.Add("Tickets");
                        dt.Columns.Add("RWF");
                        dt.Columns.Add("FIB");
                        dt.Columns.Add("Creator");
                        dt.Columns.Add("Message");
                        DataRow rw = dt.NewRow();
                        rw["BUSNAME"] = '-';
                        rw["Tickets"] = 0;
                        rw["RWF"] = "0";
                        rw["FIB"] = "-";
                        rw["MESSAGE"] = "Error 10. Try again";
                        rw["Creator"] = creator;
                        dt.Rows.Add(rw);
                        ds.Tables.Add(dt);
                        LogException(ex, dbpath);
                    }

                }

            }

            catch (Exception e)
            {
                if (ds.Tables.Contains("Report"))
                {
                    //The Subscription Process Went through but there's a minor exception so we avoid it
                    //We might never reach here by the way!
                }
                else
                {
                    LogException(e, dbpath);
                    //Big time Error and Record was not processed through at all
                    DataTable dt = new DataTable("Report");
                    dt.Columns.Add("BusName");
                    dt.Columns.Add("Tickets");
                    dt.Columns.Add("RWF");
                    dt.Columns.Add("FIB");
                    dt.Columns.Add("Creator");
                    dt.Columns.Add("Message");
                    DataRow rw = dt.NewRow();
                    rw["BUSNAME"] = '-';
                    rw["Tickets"] = 0;
                    rw["RWF"] = "0";
                    rw["FIB"] = "-";
                    rw["Creator"] = creator;
                    rw["MESSAGE"] = "Error 10. Try again";

                    dt.Rows.Add(rw);
                    ds.Tables.Add(dt);

                    doc.LoadXml(ds.GetXml());
                    return doc.DocumentElement;
                }

            }

            finally
            {
                //Not Yet Anything Here
            }

            doc.LoadXml(ds.GetXml());
            return doc.DocumentElement;
        }

        [WebMethod(CacheDuration = 1)]
        public XmlElement getSubscription(String UserName, String Password, String dbpath, String SubscriptionNum, String ClientCode, String Name, String FirstName,
            String Discount, String Creator, String hash = "", String DocumentID = "", String PosUser = "")
        {
            SetDBPath(dbpath);
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            DataSet ds = new DataSet("Subscriptions");
            SqlConnection sqlConnection1;
            SqlTransaction transaction;
            try
            {
                if (!Validate(UserName, Password, PosUser))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                if (!ValidateCode(ClientCode))
                {
                    doc.LoadXml(ValidateClientCode(ClientCode));
                    return doc.DocumentElement;
                }
                String conString = String.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                using (sqlConnection1 = new SqlConnection(conString))
                {
                    sqlConnection1.Open();
                    transaction = sqlConnection1.BeginTransaction();
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        SqlDataReader reader;

                        cmd.CommandText = "su.SUBSCRIPTION";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection1;
                        cmd.Transaction = transaction;
                        //Exec su.SUBSCRIPTION @SubscriptionNum=12345, ClientCode=0, @NAME = 'Harelimana', @FIRSTNAME = 'Innocent', 
                        //@DISCOUNT=200,  @CREATOR='12345678'
                        cmd.Parameters.AddWithValue("@SubscriptionNum", SubscriptionNum);
                        cmd.Parameters.AddWithValue("@ClientCode", ClientCode);
                        cmd.Parameters.AddWithValue("@NAME", Name);
                        cmd.Parameters.AddWithValue("@FIRSTNAME", FirstName);
                        cmd.Parameters.AddWithValue("@DISCOUNT", Discount);
                        cmd.Parameters.AddWithValue("@CREATOR", Creator);

                        String contains = "virunga select";
                        if(contains.Contains(dbpath.ToLower()))
                            cmd.Parameters.AddWithValue("@POSUSER", PosUser);

                        //sqlConnection1.Open();



                        DataTable dt = new DataTable("Subscription");
                        dt.Columns.Add("SubscriptionNum");
                        dt.Columns.Add("Cct");
                        dt.Columns.Add("Traveller");
                        dt.Columns.Add("CCode");
                        dt.Columns.Add("Price");
                        dt.Columns.Add("Disc");
                        dt.Columns.Add("Ttl");
                        dt.Columns.Add("Pnts");
                        dt.Columns.Add("POS");
                        dt.Columns.Add("RDate");
                        dt.Columns.Add("Msg");
                        dt.Columns.Add("hash");
                        dt.Columns.Add("DocID");
                        //String myan = reader.ToString();
                        using (reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow rw = dt.NewRow();
                                rw["SubscriptionNum"] = reader["SubscriptionNum"].ToString();
                                rw["SubscriptionNum"] = reader["SubscriptionNum"].ToString();
                                rw["Cct"] = reader["Circuit"].ToString();
                                rw["Traveller"] = reader["Traveller"].ToString();
                                rw["CCode"] = reader["ClientCode"].ToString();
                                rw["Price"] = reader["Price"].ToString();
                                rw["Disc"] = reader["Discount"].ToString();
                                rw["Ttl"] = reader["Total"].ToString();
                                rw["Pnts"] = reader["Points"].ToString();
                                rw["POS"] = reader["POS"].ToString();
                                rw["RDate"] = reader["RESDATE"].ToString();
                                rw["hash"] = hash;
                                rw["DocID"] = DocumentID;
                                rw["Msg"] = reader["Message"].ToString();
                                dt.Rows.Add(rw);
                            }
                        }

                        transaction.Commit();
                        ds.Tables.Add(dt);
                        //reader.Dispose();
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();
                    }
                    catch (SqlException ex)
                    {

                        transaction.Rollback();
                        DataTable dt = new DataTable("Subscription");
                        dt.Columns.Add("SubscriptionNum");
                        dt.Columns.Add("Cct");
                        dt.Columns.Add("Traveller");
                        dt.Columns.Add("CCode");
                        dt.Columns.Add("Price");
                        dt.Columns.Add("Disc");
                        dt.Columns.Add("Ttl");
                        dt.Columns.Add("Pnts");
                        dt.Columns.Add("POS");
                        dt.Columns.Add("RDate");
                        dt.Columns.Add("Msg");
                        dt.Columns.Add("hash");
                        dt.Columns.Add("DocID");

                        DataRow rw = dt.NewRow();
                        rw["SubscriptionNum"] = "-";
                        rw["SubscriptionNum"] = "-";
                        rw["Cct"] = "-";
                        rw["Traveller"] = "-";
                        rw["CCode"] = "-";
                        rw["Price"] = "-";
                        rw["Disc"] = "-";
                        rw["Ttl"] = "-";
                        rw["Pnts"] = "-";
                        rw["POS"] = "-";
                        rw["RDate"] = "-";
                        rw["hash"] = hash;
                        rw["DocID"] = "-";
                        try
                        {
                            rw["Msg"] = "No Ticket.Error 10.Try Again";
                            //rw["Msg"] = "System Error " + ex.Message.ToString().Substring(0,15) + ".Try Again";
                        }
                        catch (ArgumentOutOfRangeException exxx)
                        {
                            rw["Msg"] = "System Error 30.Try Again";
                        }
                        LogException(ex, dbpath);
                        dt.Rows.Add(rw);

                        ds.Tables.Clear();

                        ds.Tables.Add(dt);
                    }
                }


            }

            catch (Exception e)
            {
                if (ds.Tables.Contains("Subscription"))
                {
                    //As usual
                }
                else
                {
                    DataTable dt = new DataTable("Subscription");
                    dt.Columns.Add("SubscriptionNum");
                    dt.Columns.Add("Cct");
                    dt.Columns.Add("Traveller");
                    dt.Columns.Add("CCode");
                    dt.Columns.Add("Price");
                    dt.Columns.Add("Disc");
                    dt.Columns.Add("Ttl");
                    dt.Columns.Add("Pnts");
                    dt.Columns.Add("POS");
                    dt.Columns.Add("RDate");
                    dt.Columns.Add("Msg");
                    dt.Columns.Add("hash");
                    dt.Columns.Add("DocID");
                    DataRow rw = dt.NewRow();
                    rw["SubscriptionNum"] = "-";
                    rw["SubscriptionNum"] = "-";
                    rw["Cct"] = "-";
                    rw["Traveller"] = "-";
                    rw["CCode"] = "-";
                    rw["Price"] = "-";
                    rw["Disc"] = "-";
                    rw["Ttl"] = "-";
                    rw["Pnts"] = "-";
                    rw["POS"] = "-";
                    rw["RDate"] = "-";
                    rw["hash"] = hash;
                    rw["DocID"] = "";
                    try
                    {
                        rw["Msg"] = "No Ticket.Error 20.Try Again";
                        //rw["Msg"] = "System Error " + e.Message.ToString().Substring(0, 15) + ".Try Again";
                    }
                    catch (ArgumentOutOfRangeException exxx)
                    {
                        rw["Msg"] = "System Error.Try Again";
                    }

                    //LogException(e, dbpath);
                    dt.Rows.Add(rw);
                    ds.Tables.Add(dt);
                    doc.LoadXml(ds.GetXml());
                    return doc.DocumentElement;
                }

            }

            finally
            {


            }

            doc.LoadXml(ds.GetXml());
            return doc.DocumentElement;
        }

        [WebMethod(CacheDuration = 1)]
        public XmlElement GetBooking(String UserName, String Password, String dbpath, String SubscriptionNum, String Date, String Hour, String CityIN,
            String CityOUT, String Creator, String TicketID = "", String currency = "RWF", String hash = "", String seatNo = "", String BusType = "OT", String PosUser = "")
        {
            SetDBPath(dbpath);
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            DataSet ds = new DataSet("Bookings");
            SqlConnection sqlConnection1;
            SqlTransaction transaction;
            try
            {
                if (!Validate(UserName, Password, PosUser))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                String conString = String.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                using (sqlConnection1 = new SqlConnection(conString))
                {


                    String queryLatest = String.Format("SELECT IDRELATION FROM dbo.LASTID WHERE POS='{0}' AND DAY(DAY) = DAY(GETDATE())", Creator);
                    SqlTransaction transactionRoll;
                    SqlConnection con;
                    try
                    {
                        String lastID = ReturnMeSingleString(queryLatest, dbpath);
                        //LogString("Creator is: " + Creator + " Booking: Last ID is " + lastID + " :TicketID Sent is " + TicketID, dbpath);
                        //MessageBox.Show("The LastID is " + lastID);
                        if (lastID == String.Empty || lastID == "-1")
                        {
                            //This is the first ticket today so forget checking many things or there was an error retreiving the last ticketID
                            //Forget checking this since we failed to get the Last TicketID
                        }
                        else if (lastID == TicketID)
                        {
                            //The TicketID the POS sent is the same so the ticket was indeed received
                            //Do nothing
                        }
                        else if (TicketID == String.Empty || TicketID == "0" || TicketID == "-1" || TicketID == " " || TicketID.Length < 3)
                        {
                            //Donot bother to check
                        }
                        else if (TicketID != lastID)
                        {
                            //So here the pos sent an id different from the last recorded so 
                            //Either return that specific Ticket or Remove that Lastticket because it did not reach the POS
                            SqlCommand cmdRollback = new SqlCommand();

                            using (con = new SqlConnection(conString))
                            {
                                con.Open();
                                transactionRoll = con.BeginTransaction();
                                cmdRollback.CommandText = "su.TicketRollback";
                                cmdRollback.CommandType = CommandType.StoredProcedure;
                                cmdRollback.Connection = con;
                                cmdRollback.Transaction = transactionRoll;

                                cmdRollback.Parameters.AddWithValue("@TICKETID", lastID);
                                try
                                {
                                    cmdRollback.ExecuteNonQuery();
                                    transactionRoll.Commit();
                                }
                                catch (SqlException exxx)
                                {
                                    transactionRoll.Rollback();
                                    LogException(exxx, dbpath);
                                }
                                catch (Exception exxxxx)
                                {
                                    transactionRoll.Rollback();
                                    LogException(exxxxx, dbpath);
                                }
                            }
                            String rollbackStatement = String.Format("Creator: {0}, Route {1}-{2} , HOUR: {6}, Subscription Num: {3}, TicketID Rolledback: {4}, TicketID Sent: {5}", Creator, CityIN, CityOUT, SubscriptionNum, lastID, TicketID, Hour);
                            //LogString(rollbackStatement, dbpath);
                            //LogString(rollbackStatement, dbpath + "Rolls");
                        }
                    }
                    catch (SqlException ex)
                    {
                        LogException(ex, dbpath);
                    }
                    catch (Exception exex)
                    {
                        LogException(exex, dbpath);
                    }
                    sqlConnection1.Open();
                    transaction = sqlConnection1.BeginTransaction();
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        SqlDataReader reader;

                        cmd.CommandText = "su.SUBSCRIPTION_TICKETING";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection1;
                        cmd.Transaction = transaction;
                        //Exec su.SUBSCRIPTION @SubscriptionNum=12345, ClientCode=0, @NAME = 'Harelimana', @FIRSTNAME = 'Innocent', 
                        //@DISCOUNT=200,  @CREATOR='12345678'
                        cmd.Parameters.AddWithValue("@SubscriptionNum", SubscriptionNum);
                        cmd.Parameters.AddWithValue("@DATE", Date);
                        cmd.Parameters.AddWithValue("@HOUR", Hour);
                        cmd.Parameters.AddWithValue("@CITYIN", CityIN);
                        cmd.Parameters.AddWithValue("@CITYOUT", CityOUT);
                        cmd.Parameters.AddWithValue("@CREATOR", Creator);
                        cmd.Parameters.AddWithValue("@USERNAME", PosUser);
                        cmd.Parameters.AddWithValue("@BUSTYPE", BusType);

                        if (seatNo != "0")
                        {
                            //They want a preferred Seat
                            cmd.Parameters.AddWithValue("@PREFSEAT", seatNo);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@PREFSEAT", "Yes");
                        }

                        //sqlConnection1.Open();

                        //reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable("Booking");
                        dt.Columns.Add("TktID");
                        dt.Columns.Add("Cct");
                        dt.Columns.Add("Rte");
                        dt.Columns.Add("Date");
                        dt.Columns.Add("Hr");
                        dt.Columns.Add("CIn");
                        dt.Columns.Add("COut");
                        dt.Columns.Add("TPhone");
                        dt.Columns.Add("SubscriptionNum");
                        dt.Columns.Add("Traveller");
                        dt.Columns.Add("CCode");
                        dt.Columns.Add("Price");
                        dt.Columns.Add("Disc");
                        dt.Columns.Add("Ttl");
                        dt.Columns.Add("Trips");
                        dt.Columns.Add("POS");
                        dt.Columns.Add("RNum");
                        dt.Columns.Add("RDate");
                        dt.Columns.Add("hash");
                        dt.Columns.Add("Msg");
                        dt.Columns.Add("SNo");
                        dt.Columns.Add("DTime");
                        //String myan = reader.ToString();
                        using (reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow rw = dt.NewRow();
                                rw["TktID"] = reader["TicketID"].ToString();
                                rw["Cct"] = reader["Circuit"].ToString();
                                rw["Rte"] = reader["Route"].ToString();
                                rw["Date"] = reader["Date"].ToString();
                                rw["Hr"] = reader["Hour"].ToString();
                                rw["CIn"] = reader["CityIN"].ToString();
                                rw["COut"] = reader["CityOUT"].ToString();
                                rw["TPhone"] = reader["TrajetPhone"].ToString();
                                rw["SubscriptionNum"] = reader["SubscriptionNum"].ToString();
                                rw["Traveller"] = reader["Traveller"].ToString();
                                rw["CCode"] = reader["ClientCode"].ToString();
                                rw["Price"] = reader["Price"].ToString();
                                rw["Disc"] = reader["Discount"].ToString();
                                rw["Ttl"] = reader["Total"].ToString();
                                rw["Trips"] = reader["Trips"].ToString();
                                rw["POS"] = reader["POS"].ToString();
                                rw["RNum"] = reader["ResNum"].ToString();
                                rw["RDate"] = reader["ResDate"].ToString();
                                rw["Msg"] = reader["Message"].ToString();
                                rw["hash"] = hash;
                                rw["SNo"] = reader["SeatNo"].ToString();
                                rw["DTime"] = reader["Dtime"].ToString();
                                //activate only when PROC returns Platenumber else ignore it!
                                if(HasColumn(reader, "PLATENO"))
                                {
                                    if(!dt.Columns.Contains("PLATENO"))
                                        dt.Columns.Add("PLATENO");

                                    rw["PLATENO"] = reader["PLATENO"].ToString();
                                }
                                //activate only when PROC returns Username else ignore it!
                                if (HasColumn(reader, "UserName"))
                                {
                                    if (!dt.Columns.Contains("UserName"))
                                        dt.Columns.Add("UserName");

                                    rw["UserName"] = reader["UserName"].ToString();
                                }
                                dt.Rows.Add(rw);
                                //LogString("Booking: Creator is: " + Creator + " TicketID Assigned is: " + rw["TktID"].ToString(), dbpath);
                            }
                        }

                        ds.Tables.Add(dt);

                        transaction.Commit();

                        //reader.Dispose();
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        DataTable dt = new DataTable("Booking");
                        dt.Columns.Add("TktID");
                        dt.Columns.Add("Cct");
                        dt.Columns.Add("Rte");
                        dt.Columns.Add("Date");
                        dt.Columns.Add("Hr");
                        dt.Columns.Add("CIn");
                        dt.Columns.Add("COut");
                        dt.Columns.Add("TPhone");
                        dt.Columns.Add("SubscriptionNum");
                        dt.Columns.Add("Traveller");
                        dt.Columns.Add("CCode");
                        dt.Columns.Add("Price");
                        dt.Columns.Add("Disc");
                        dt.Columns.Add("Ttl");
                        dt.Columns.Add("Trips");
                        dt.Columns.Add("POS");
                        dt.Columns.Add("RNum");
                        dt.Columns.Add("RDate");
                        dt.Columns.Add("Msg");
                        dt.Columns.Add("hash");
                        dt.Columns.Add("SNo");
                        dt.Columns.Add("DTime");

                        DataRow rw = dt.NewRow();
                        rw["TktID"] = "0";
                        rw["Cct"] = "-";
                        rw["Rte"] = "-";
                        rw["Date"] = "-";
                        rw["Hr"] = "-";
                        rw["CIn"] = "-";
                        rw["COut"] = "-";
                        rw["TPhone"] = "-";
                        rw["SubscriptionNum"] = "-";
                        rw["Traveller"] = "-";
                        rw["CCode"] = "0";
                        rw["Price"] = "-";
                        rw["Disc"] = "-";
                        rw["Ttl"] = "-";
                        rw["Trips"] = "-";
                        rw["POS"] = "-";
                        rw["RNum"] = "-";
                        rw["RDate"] = "-";
                        rw["hash"] = hash;
                        rw["SNo"] = seatNo;
                        rw["DTime"] = "12H00";
                        try
                        {
                            rw["Msg"] = "No Ticket.Error 10.Try Again";
                            //rw["Msg"] = "System Error " + ex.Message.ToString().Substring(0, 15) + ".Try Again";
                            LogMe("Subscription Ticketing:-"+ex.ToString());
                        }
                        catch (ArgumentOutOfRangeException exxx)
                        {
                            rw["Msg"] = "System Error.Try Again";
                        }
                        dt.Rows.Add(rw);
                        LogException(ex, dbpath);
                        ds.Tables.Add(dt);
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();
                    }
                }


            }

            catch (Exception e)
            {
                if (ds.Tables.Contains("Booking"))
                {
                    //Kama Kawaida
                    //Huenda tusifike hapa 
                    LogException(e, dbpath);
                }
                else
                {
                    DataTable dt = new DataTable("Booking");
                    dt.Columns.Add("TktID");
                    dt.Columns.Add("Cct");
                    dt.Columns.Add("Rte");
                    dt.Columns.Add("Date");
                    dt.Columns.Add("Hr");
                    dt.Columns.Add("CIn");
                    dt.Columns.Add("COut");
                    dt.Columns.Add("TPhone");
                    dt.Columns.Add("SubscriptionNum");
                    dt.Columns.Add("Traveller");
                    dt.Columns.Add("CCode");
                    dt.Columns.Add("Price");
                    dt.Columns.Add("Disc");
                    dt.Columns.Add("Ttl");
                    dt.Columns.Add("Trips");
                    dt.Columns.Add("POS");
                    dt.Columns.Add("RNum");
                    dt.Columns.Add("RDate");
                    dt.Columns.Add("Msg");
                    dt.Columns.Add("hash");
                    dt.Columns.Add("SNo");
                    dt.Columns.Add("DTime");

                    DataRow rw = dt.NewRow();
                    rw["TktID"] = "0";
                    rw["Cct"] = "-";
                    rw["Rte"] = "-";
                    rw["Date"] = "-";
                    rw["Hr"] = "-";
                    rw["CIn"] = "-";
                    rw["COut"] = "-";
                    rw["TPhone"] = "-";
                    rw["SubscriptionNum"] = "-";
                    rw["Traveller"] = "-";
                    rw["CCode"] = "0";
                    rw["Price"] = "-";
                    rw["Disc"] = "-";
                    rw["Ttl"] = "-";
                    rw["Trips"] = "-";
                    rw["POS"] = "-";
                    rw["RNum"] = "-";
                    rw["RDate"] = "-";
                    rw["hash"] = hash;
                    rw["SNo"] = seatNo;
                    rw["DTime"] = "12H00";
                    try
                    {
                        rw["Msg"] = "No Ticket.Error 20.Try Again";
                        //rw["Msg"] = "System Error " + e.Message.ToString().Substring(0, 15) + ".Try Again";
                    }
                    catch (ArgumentOutOfRangeException exxx)
                    {
                        rw["Msg"] = "System Error.Try Again";
                    }
                    LogException(e, dbpath);
                    dt.Rows.Add(rw);

                    ds.Tables.Add(dt);
                    doc.LoadXml(ds.GetXml());
                    return doc.DocumentElement;
                }
            }

            finally
            {


            }

            doc.LoadXml(ds.GetXml());
            return doc.DocumentElement;
        }

        [WebMethod(CacheDuration = 1)]
        public System.Xml.XmlElement Prebooking(String UserName, String Password, String dbpath, String Date, String Hour, String CityIN, String CityOut,
            String Discount, String Creator, String Name = "", String Firstname = "", String ClientCode = "", String currency = "RWF", String hash = "", String seatNo = "", String BusType = "EXPRESS", String DocumentID = "", String PosUser = "")
        {
            SetDBPath(dbpath);
            DataSet ds = new DataSet("Prebookings");
            DataTable dt = new DataTable();
            SqlConnection sqlConnection1;
            SqlTransaction transaction;
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            try
            {
                if (hash.Length == 0)
                {
                    hash = "0";
                }
                if (!Validate(UserName, Password, PosUser))
                {
                    throw new Exception(String.Format("Wrong UserName or Password. You sent Username: {0} And Password: {1}", UserName, Password));
                }
                //Validate to make sure Client Code Contains Numbers only
                if (!ValidateCode(ClientCode))
                {
                    doc.LoadXml(ValidateClientCode(ClientCode));
                    return doc.DocumentElement;
                }
                String conString = String.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                sqlConnection1 = new SqlConnection(conString);
                using (sqlConnection1 = new SqlConnection(conString))
                {
                    sqlConnection1.Open();
                    //SqlConnection con;
                    transaction = sqlConnection1.BeginTransaction();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;
                    try
                    {
                        cmd.CommandText = "su.Prebooking";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection1;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@DATE", Date);
                        cmd.Parameters.AddWithValue("@HOUR", Hour);
                        cmd.Parameters.AddWithValue("@CityIN", CityIN);
                        cmd.Parameters.AddWithValue("@CityOut", CityOut);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Firstname", Firstname);
                        cmd.Parameters.AddWithValue("@ClientCode", ClientCode);
                        cmd.Parameters.AddWithValue("@Discount", Discount);
                        cmd.Parameters.AddWithValue("@Creator", Creator);
                        cmd.Parameters.AddWithValue("@Currency", currency);
                        cmd.Parameters.AddWithValue("@PREFSEAT", seatNo);

                        if (BusType == "OT" || BusType.ToLower().Contains("other"))
                        {
                            //The default
                            cmd.Parameters.AddWithValue("@BusCategory", "OTHER");
                        }
                        else if (BusType.ToLower().Contains("express"))
                        {
                            cmd.Parameters.AddWithValue("@BusCategory", "EXPRESS");
                        }
                        else
                        {
                            String type = "EXPRESS";
                            if (BusType != null)
                                type = BusType.Replace("bus", "").Replace("Bus", "").Replace("BUS", "");
                            cmd.Parameters.AddWithValue("@BusCategory", type.Trim());
                        }

                        if (DocumentID.Length > 0)
                        {
                            cmd.Parameters.AddWithValue("@Passport", DocumentID);
                        }

                        //reader = cmd.ExecuteReader();
                        dt = new DataTable("Prebooking");
                        dt.Columns.Add("BookingNo");
                        dt.Columns.Add("Cct");
                        dt.Columns.Add("BusName");
                        dt.Columns.Add("Traveller");
                        dt.Columns.Add("CCode");
                        dt.Columns.Add("Price");
                        dt.Columns.Add("Pos");
                        dt.Columns.Add("RDate");
                        dt.Columns.Add("Msg");
                        dt.Columns.Add("hash");
                        dt.Columns.Add("SNo");
                        dt.Columns.Add("DocID");
                        //String myan = reader.ToString();
                        using (reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow rw = dt.NewRow();
                                rw["BookingNo"] = reader["BookingNo"].ToString();
                                rw["Cct"] = reader["Circuit"].ToString();
                                rw["BusName"] = reader["BusName"].ToString();
                                rw["Traveller"] = reader["Traveller"].ToString();
                                rw["CCode"] = reader["ClientCode"].ToString();
                                rw["Price"] = reader["Price"].ToString();
                                rw["Pos"] = reader["Pos"].ToString();
                                rw["RDate"] = reader["ResDate"].ToString();
                                rw["Msg"] = reader["Message"].ToString();
                                rw["hash"] = hash;
                                rw["SNo"] = reader["SeatNo"].ToString();
                                rw["DocID"] = DocumentID;
                                dt.Rows.Add(rw);
                                // LogString("Prebooking Ticketing: Creator is: " + Creator + " BookingNo ASsigned is " + rw["BookingNo"].ToString(), dbpath);
                            }
                        }

                        //reader.Dispose();
                        ds.Tables.Add(dt);
                        transaction.Commit();
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();
                    }
                    catch (SqlException e)
                    {
                        transaction.Rollback();
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();
                        //Return A valid Messsage After this to Ensure They Don't Get Confused
                        dt = new DataTable("Prebooking");
                        dt.Columns.Add("BookingNo");
                        dt.Columns.Add("Cct");
                        dt.Columns.Add("BusName");
                        dt.Columns.Add("Traveller");
                        dt.Columns.Add("CCode");
                        dt.Columns.Add("Price");
                        dt.Columns.Add("Pos");
                        dt.Columns.Add("RDate");
                        dt.Columns.Add("Msg");
                        dt.Columns.Add("hash");
                        dt.Columns.Add("SNo");
                        dt.Columns.Add("DocID");

                        DataRow rw = dt.NewRow();

                        rw["BookingNo"] = 0;
                        rw["Cct"] = "-";
                        rw["BusName"] = "-";
                        rw["Traveller"] = "-";
                        rw["CCode"] = "-";
                        rw["Price"] = "-";
                        rw["Pos"] = "-";
                        rw["RDate"] = "-";
                        rw["hash"] = hash;
                        rw["SNo"] = seatNo;
                        rw["DocID"] = DocumentID;
                        try
                        {
                            rw["Msg"] = "No Booking.Error 10.Try Again";
                            //rw["Msg"] = "Ticket Not Issued Due To " + e.Message.ToString() + ". Please Try Again";
                            LogMe(e.ToString());
                        }
                        catch (ArgumentOutOfRangeException exx)
                        {
                            rw["Msg"] = "System Error. Try Again.";
                        }
                        LogException(e, dbpath);
                        dt.Rows.Add(rw);
                        ds.Tables.Add(dt);
                        doc.LoadXml(ds.GetXml());
                        return doc.DocumentElement;
                    }
                }

            }

            catch (Exception e)
            {
                if (ds.Tables.Contains("Prebooking"))
                {
                    //Here a ticket has been issued successfully but there was a minor exception so don't bother returning that exception
                    LogException(e, dbpath);
                }
                else
                {
                    dt = new DataTable("Prebooking");
                    dt.Columns.Add("BookingNo");
                    dt.Columns.Add("Cct");
                    dt.Columns.Add("BusName");
                    dt.Columns.Add("Traveller");
                    dt.Columns.Add("CCode");
                    dt.Columns.Add("Price");
                    dt.Columns.Add("Pos");
                    dt.Columns.Add("RDate");
                    dt.Columns.Add("Msg");
                    dt.Columns.Add("hash");
                    dt.Columns.Add("SNo");
                    dt.Columns.Add("DocID");

                    DataRow rw = dt.NewRow();
                    rw["BookingNo"] = 0;
                    rw["Cct"] = "-";
                    rw["BusName"] = "-";
                    rw["Traveller"] = "-";
                    rw["CCode"] = "-";
                    rw["Price"] = "-";
                    rw["Pos"] = "-";
                    rw["RDate"] = "-";
                    rw["hash"] = hash;
                    rw["SNo"] = seatNo;
                    rw["DocID"] = DocumentID;

                    try
                    {
                        rw["Msg"] = "No Booking.Error 20.Try Again";
                        //rw["Msg"] = "Ticket Not Issued Due To " + e.Message.ToString().Substring(0, 15) + ". Please Try Again";
                        //rw["Msg"] = "General Exception. Ticket Not Issued Due To " + e.Message.ToString() + ". Please Try Again. This Error Came From " + e.Source.ToString() + " And the Stack Trace " + e.StackTrace.ToString();
                    }
                    catch (ArgumentOutOfRangeException exx)
                    {
                        rw["Msg"] = "System Error. Try Again.";
                    }
                    dt.Rows.Add(rw);
                    ds.Tables.Add(dt);
                    doc.LoadXml(ds.GetXml());
                    //transaction.Rollback();
                    //sqlConnection1.Close();
                    //sqlConnection1.Dispose();
                    LogException(e, dbpath);
                    return doc.DocumentElement;
                }

            }

            finally
            {

            }
            doc.LoadXml(ds.GetXml());
            return doc.DocumentElement;
        }

        [WebMethod(CacheDuration = 1)]
        public System.Xml.XmlElement PullBooking(String UserName, String Password, String dbpath, String Creator, String BookingNo = "0", String hash = "", String PosUser = "")
        {
            SetDBPath(dbpath);
            DataSet ds = new DataSet("PullBooking");
            DataTable dt = new DataTable();
            SqlConnection sqlConnection1;
            SqlTransaction transaction;
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            try
            {
                if (hash.Length == 0)
                {
                    hash = "0";
                }
                if (!Validate(UserName, Password, PosUser))
                {
                    throw new Exception("Wrong UserName or Password");
                }

                String conString = String.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);

                sqlConnection1 = new SqlConnection(conString);
                using (sqlConnection1 = new SqlConnection(conString))
                {
                    sqlConnection1.Open();
                    //Check the TicketID served if empty then check the most previous ticketid from this pos and remove it
                    //String queryLatest = String.Format("SELECT IDRELATION FROM dbo.LASTID WHERE POS='{0}' AND DAY(DAY) = DAY(GETDATE())", Creator);
                    //SqlTransaction transactionRoll;
                    //SqlConnection con;

                    //transaction = sqlConnection1.BeginTransaction();
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;
                    try
                    {
                        cmd.CommandText = "dbo.PullPreBooking";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection1;
                        //cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@BookingNo", BookingNo);
                        cmd.Parameters.AddWithValue("@Creator", Creator);
                        cmd.Parameters.AddWithValue("@POSUSER", PosUser);



                        //reader = cmd.ExecuteReader();
                        dt = new DataTable("Ticket");
                        dt.Columns.Add("TktID");
                        dt.Columns.Add("Cct");
                        dt.Columns.Add("Rte");
                        dt.Columns.Add("Date");
                        dt.Columns.Add("Hr");
                        dt.Columns.Add("CIn");
                        dt.Columns.Add("COut");
                        dt.Columns.Add("TPhone");
                        dt.Columns.Add("Traveller");
                        dt.Columns.Add("CCode");
                        dt.Columns.Add("Price");
                        dt.Columns.Add("Disc");
                        dt.Columns.Add("Ttl");
                        dt.Columns.Add("Pnts");
                        dt.Columns.Add("Pos");
                        dt.Columns.Add("RNum");
                        dt.Columns.Add("RDate");
                        dt.Columns.Add("Msg");
                        dt.Columns.Add("hash");
                        dt.Columns.Add("SNo");
                        dt.Columns.Add("DTime");
                        dt.Columns.Add("DocID");
                        dt.Columns.Add("FullName");
                        //String myan = reader.ToString();
                        using (reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow rw = dt.NewRow();
                                rw["TktID"] = reader["TicketID"].ToString();
                                rw["Cct"] = reader["Circuit"].ToString();
                                rw["Rte"] = reader["Route"].ToString();
                                rw["Date"] = reader["Date"].ToString();
                                rw["Hr"] = reader["Hour"].ToString();
                                rw["CIn"] = reader["CityIN"].ToString();
                                rw["COut"] = reader["CityOut"].ToString();
                                rw["TPhone"] = reader["TrajetPhone"].ToString();
                                rw["Traveller"] = reader["Traveller"].ToString();
                                rw["CCode"] = reader["ClientCode"].ToString();
                                rw["Price"] = reader["Price"].ToString();
                                rw["Disc"] = reader["Discount"].ToString();
                                rw["Ttl"] = reader["Total"].ToString();
                                rw["Pnts"] = reader["points"].ToString();
                                rw["Pos"] = reader["Pos"].ToString();
                                rw["RNum"] = reader["ResNum"].ToString();
                                rw["RDate"] = reader["ResDate"].ToString();
                                rw["Msg"] = reader["Message"].ToString();
                                rw["hash"] = hash;
                                rw["SNo"] = reader["SeatNo"].ToString();
                                rw["DTime"] = reader["DTime"].ToString();
                                //rw["DocID"] = reader["DocumentID"].ToString(); 

                                //activate only when PROC returns Platenumber else ignore it!
                                if (HasColumn(reader, "PLATENO"))
                                {
                                    if (!dt.Columns.Contains("PLATENO"))
                                        dt.Columns.Add("PLATENO");

                                    rw["PLATENO"] = reader["PLATENO"].ToString();
                                }
                                dt.Rows.Add(rw);
                                //LogString("Pulling Booking: Creator is: " + Creator + " TicketID ASsigned is " + rw["TktID"].ToString(), dbpath);
                            }
                        }

                        //reader.Dispose();
                        ds.Tables.Add(dt);
                        //transaction.Commit();
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();
                    }
                    catch (SqlException e)
                    {
                        LogMe(e.ToString());
                        throw e;
                        //transaction.Rollback();
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();
                        //Return A valid Messsage After this to Ensure They Don't Get Confused
                        dt = new DataTable("Ticket");
                        dt.Columns.Add("TktID");
                        dt.Columns.Add("Cct");
                        dt.Columns.Add("Rte");
                        dt.Columns.Add("Date");
                        dt.Columns.Add("Hr");
                        dt.Columns.Add("CIn");
                        dt.Columns.Add("COut");
                        dt.Columns.Add("TPhone");
                        dt.Columns.Add("Traveller");
                        dt.Columns.Add("CCode");
                        dt.Columns.Add("Price");
                        dt.Columns.Add("Disc");
                        dt.Columns.Add("Ttl");
                        dt.Columns.Add("Pnts");
                        dt.Columns.Add("Pos");
                        dt.Columns.Add("RNum");
                        dt.Columns.Add("RDate");
                        dt.Columns.Add("Msg");
                        dt.Columns.Add("hash");
                        dt.Columns.Add("SNo");
                        dt.Columns.Add("DTime");
                        dt.Columns.Add("DocID");
                        DataRow rw = dt.NewRow();
                        rw["TktID"] = "0";
                        rw["Cct"] = "0";
                        rw["Rte"] = "-";
                        rw["Date"] = "-";
                        rw["Hr"] = "-";
                        rw["CIn"] = "-";
                        rw["COut"] = "-";
                        rw["TPhone"] = "-";
                        rw["Traveller"] = "-";
                        rw["CCode"] = "-";
                        rw["Price"] = "-";
                        rw["Disc"] = "-";
                        rw["Ttl"] = "-";
                        rw["Pnts"] = "-";
                        rw["Pos"] = "-";
                        rw["RNum"] = "-";
                        rw["RDate"] = "-";
                        rw["hash"] = hash;
                        rw["SNo"] = "-";
                        rw["DTime"] = "-";
                        rw["DocID"] = "-";
                        try
                        {
                            rw["Msg"] = "No Ticket.Error 10.Try Again";
                            //rw["Msg"] = "Ticket Not Issued Due To " + e.Message.ToString() + ". Please Try Again";
                        }
                        catch (ArgumentOutOfRangeException exx)
                        {
                            throw exx;
                            rw["Msg"] = "System Error. Try Again.";
                        }
                        LogException(e, dbpath);
                        dt.Rows.Add(rw);
                        ds.Tables.Add(dt);
                        doc.LoadXml(ds.GetXml());
                        return doc.DocumentElement;
                    }
                }

            }

            catch (Exception e)
            {
                throw e;
                if (ds.Tables.Contains("Ticket"))
                {
                    //Here a ticket has been issued successfully but there was a minor exception so don't bother returning that exception
                    LogException(e, dbpath);
                }
                else
                {
                    dt = new DataTable("Ticket");
                    dt.Columns.Add("TktID");
                    dt.Columns.Add("Cct");
                    dt.Columns.Add("Rte");
                    dt.Columns.Add("Date");
                    dt.Columns.Add("Hr");
                    dt.Columns.Add("CIn");
                    dt.Columns.Add("COut");
                    dt.Columns.Add("TPhone");
                    dt.Columns.Add("Traveller");
                    dt.Columns.Add("CCode");
                    dt.Columns.Add("Price");
                    dt.Columns.Add("Disc");
                    dt.Columns.Add("Ttl");
                    dt.Columns.Add("Pnts");
                    dt.Columns.Add("Pos");
                    dt.Columns.Add("RNum");
                    dt.Columns.Add("RDate");
                    dt.Columns.Add("Msg");
                    dt.Columns.Add("hash");
                    dt.Columns.Add("SNo");
                    dt.Columns.Add("DTime");
                    dt.Columns.Add("DocID");
                    DataRow rw = dt.NewRow();
                    rw["TktID"] = "0";
                    rw["Cct"] = "0";
                    rw["Rte"] = "-";
                    rw["Date"] = "-";
                    rw["Hr"] = "-";
                    rw["CIn"] = "-";
                    rw["COut"] = "-";
                    rw["TPhone"] = "-";
                    rw["Traveller"] = "-";
                    rw["CCode"] = "-";
                    rw["Price"] = "-";
                    rw["Disc"] = "-";
                    rw["Ttl"] = "-";
                    rw["Pnts"] = "-";
                    rw["Pos"] = "-";
                    rw["RNum"] = "-";
                    rw["RDate"] = "-";
                    rw["hash"] = hash;
                    rw["SNo"] = "-";
                    rw["DTime"] = "-";
                    rw["DocID"] = "-";
                    try
                    {
                        rw["Msg"] = "No Ticket.Error 20.Try Again";
                        //rw["Msg"] = "Ticket Not Issued Due To " + e.Message.ToString().Substring(0, 15) + ". Please Try Again";
                        //rw["Msg"] = "General Exception. Ticket Not Issued Due To " + e.Message.ToString() + ". Please Try Again. This Error Came From " + e.Source.ToString() + " And the Stack Trace " + e.StackTrace.ToString();
                    }
                    catch (ArgumentOutOfRangeException exx)
                    {
                        throw exx;
                        rw["Msg"] = "System Error. Try Again.";
                    }
                    dt.Rows.Add(rw);
                    ds.Tables.Add(dt);
                    doc.LoadXml(ds.GetXml());
                    //transaction.Rollback();
                    //sqlConnection1.Close();
                    //sqlConnection1.Dispose();
                    LogException(e, dbpath);
                    return doc.DocumentElement;
                }

            }

            finally
            {

            }

            doc.LoadXml(ds.GetXml());
            XmlNode ticket = doc.DocumentElement.FirstChild;
            //append Username
            using (var context = new AppDBContext())
            {
                SystemUser u = context.SystemUsers.Find(PosUser);
                if (u != null)
                {
                    XmlNode userNode = doc.CreateElement("FullName");
                    userNode.InnerText = u.Fullname;
                    ticket.AppendChild(userNode);
                }
            }

            return doc.DocumentElement;
        }

        [WebMethod(CacheDuration = 1)]
        public XmlElement SeatsReport(String UserName, String Password, String dbpath, String Cityin, String CityOut, String Date, String Hour, String PosUser = "")
        {
            SetDBPath(dbpath);
            DataSet ds = new DataSet("SeatsReport");
            SqlConnection sqlConnection1;
            SqlTransaction transaction;
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            try
            {
                if (!Validate(UserName, Password, PosUser))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                String conString = String.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                using (sqlConnection1 = new SqlConnection(conString))
                {
                    try
                    {
                        using (var cmd = new SqlCommand("su.SEATSREPORT", sqlConnection1))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = sqlConnection1;

                            cmd.Parameters.AddWithValue("@CITYIN", Cityin);
                            cmd.Parameters.AddWithValue("@CITYOUT", CityOut);
                            cmd.Parameters.AddWithValue("@DATE", Date);
                            cmd.Parameters.AddWithValue("@HOUR", Hour);

                            using (var da = new SqlDataAdapter(cmd))
                            {
                                DataTable dt = new DataTable("Seats");
                                da.Fill(dt);
                                ds.Tables.Add(dt);
                            }
                        }

                    }
                    catch (SqlException ex)
                    {
                        DataTable dt = new DataTable("Seats");
                        dt.Columns.Add("BUSNAME");
                        dt.Columns.Add("CAPACITY");
                        dt.Columns.Add("FREESEATS");
                        DataRow rw = dt.NewRow();

                        rw["BUSNAME"] = '-';
                        rw["CAPACITY"] = "-";
                        rw["FREESEATS"] = "- System 10 Error" + ex.Message;
                        dt.Rows.Add(rw);
                        ds.Tables.Add(dt);
                        LogException(ex, dbpath);
                    }

                }

            }

            catch (Exception e)
            {
                if (ds.Tables.Contains("Seats"))
                {
                    //The Subscription Process Went through but there's a minor exception so we avoid it
                    //We might never reach here by the way!
                }
                else
                {
                    //Big time Error and Record was not processed through at all
                    DataTable dt = new DataTable("Seats");
                    dt.Columns.Add("BUSNAME");
                    dt.Columns.Add("CAPACITY");
                    dt.Columns.Add("FREESEATS");
                    DataRow rw = dt.NewRow();

                    rw["BUSNAME"] = '-';
                    rw["CAPACITY"] = "-";
                    rw["FREESEATS"] = "- System 10 Error" + e.Message;
                    dt.Rows.Add(rw);
                    ds.Tables.Add(dt);
                    LogException(e, dbpath);
                    doc.LoadXml(ds.GetXml());
                    return doc.DocumentElement;
                }

            }

            finally
            {
                //Not Yet Anything Here
            }
            doc.LoadXml(ds.GetXml());
            return doc.DocumentElement;
        }

        [WebMethod(CacheDuration = 1)]
        public XmlElement GetOtherReport(String UserName, String Password, String dbpath, String pos, String Year, String Month, String Day, String PosUser = "")
        {
            SetDBPath(dbpath);
            DataSet ds = new DataSet("PosReport");
            SqlConnection sqlConnection1;
            SqlTransaction transaction;
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = false;
            try
            {
                if (!Validate(UserName, Password, PosUser))
                {
                    throw new Exception("Wrong UserName or Password");
                }
                String conString = String.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                using (sqlConnection1 = new SqlConnection(conString))
                {
                    sqlConnection1.Open();
                    transaction = sqlConnection1.BeginTransaction();
                    try
                    {
                        SqlCommand cmd = new SqlCommand();
                        SqlDataReader reader;

                        cmd.CommandText = "su.USER_VENDING_WITH_DATE";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = sqlConnection1;
                        cmd.Transaction = transaction;

                        String reportDate = String.Format("{0}-{1}-{2}", Year, Month, Day);

                        cmd.Parameters.AddWithValue("@USERNAME", PosUser);
                        cmd.Parameters.AddWithValue("@DATEOF", reportDate);


                        //sqlConnection1.Open();

                        //reader = cmd.ExecuteReader();

                        DataTable dt = new DataTable("Report");
                        dt.Columns.Add("REPORTDATE");
                        dt.Columns.Add("USERNAME");
                        dt.Columns.Add("TICKETS");
                        dt.Columns.Add("CASH_TICKETING");
                        dt.Columns.Add("SUBSCRIPTIONS");
                        dt.Columns.Add("CASH_SUBSCRIPTION");
                        dt.Columns.Add("Ttl");
                        //String myan = reader.ToString();
                        using (reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DataRow rw = dt.NewRow();
                                rw["REPORTDATE"] = reportDate;
                                rw["USERNAME"] = reader["USERNAME"].ToString();
                                rw["TICKETS"] = reader["TICKETS"].ToString();
                                rw["CASH_TICKETING"] = reader["CASH_TICKETING"].ToString();
                                rw["SUBSCRIPTIONS"] = reader["SUBSCRIPTIONS"].ToString();
                                rw["CASH_SUBSCRIPTION"] = reader["CASH_SUBSCRIPTION"].ToString();
                                rw["Ttl"] = reader["TOTAL"].ToString();
                                dt.Rows.Add(rw);
                            }
                        }

                        transaction.Commit();
                        ds.Tables.Add(dt);
                        reader.Dispose();
                        sqlConnection1.Close();
                        sqlConnection1.Dispose();

                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        DataTable dt = new DataTable("Report");
                        dt.Columns.Add("REPORTDATE");
                        dt.Columns.Add("POS");
                        dt.Columns.Add("TICKETS");
                        dt.Columns.Add("CASH_TICKETING");
                        dt.Columns.Add("SUBSCRIPTIONS");
                        dt.Columns.Add("CASH_SUBSCRIPTION");
                        dt.Columns.Add("Ttl");
                        DataRow rw = dt.NewRow();
                        rw["REPORTDATE"] = "-";
                        rw["POS"] = pos;
                        rw["TICKETS"] = "Error";
                        rw["CASH_TICKETING"] = "-";
                        rw["SUBSCRIPTIONS"] = "-";
                        try
                        {

                            rw["CASH_SUBSCRIPTION"] = "Error 10. Try again";
                        }
                        catch (ArgumentOutOfRangeException exx)
                        {
                            rw["CASH_SUBSCRIPTION"] = "System Error.Try again";
                        }

                        rw["Ttl"] = "ERROR";
                        dt.Rows.Add(rw);
                        ds.Tables.Add(dt);
                        LogException(ex, dbpath);
                    }

                }

            }

            catch (Exception e)
            {
                if (ds.Tables.Contains("Report"))
                {
                    //The Subscription Process Went through but there's a minor exception so we avoid it
                    //We might never reach here by the way!
                }
                else
                {
                    //Big time Error and Record was not processed through at all
                    DataTable dt = new DataTable("Report");
                    dt.Columns.Add("REPORTDATE");
                    dt.Columns.Add("USERNAME");
                    dt.Columns.Add("TICKETS");
                    dt.Columns.Add("CASH_TICKETING");
                    dt.Columns.Add("SUBSCRIPTIONS");
                    dt.Columns.Add("CASH_SUBSCRIPTION");
                    dt.Columns.Add("Ttl");
                    DataRow rw = dt.NewRow();
                    rw["REPORTDATE"] = "-";
                    rw["USERNAME"] = pos;
                    rw["TICKETS"] = "Error";
                    rw["CASH_TICKETING"] = "-";
                    rw["SUBSCRIPTIONS"] = "-";
                    try
                    {
                        rw["CASH_SUBSCRIPTION"] = "Error 20.Try again";
                    }
                    catch (ArgumentOutOfRangeException exx)
                    {
                        rw["CASH_SUBSCRIPTION"] = "System Error.Try again";
                    }
                    rw["Ttl"] = "ERROR";
                    dt.Rows.Add(rw);
                    ds.Tables.Add(dt);
                    LogException(e, dbpath);
                    doc.LoadXml(ds.GetXml());
                    return doc.DocumentElement;
                }

            }

            finally
            {
                //Not Yet Anything Here
            }

            doc.LoadXml(ds.GetXml());
            return doc.DocumentElement;
        }


        private Boolean Validate(String userName, String passWord, String PosUser)
        {
            string uname = ConfigurationManager.AppSettings["username"].ToString();
            string passW = ConfigurationManager.AppSettings["password"].ToString();
            Boolean flag = false;
            if (userName == uname && passWord == passW) // check POS User Details
            {
                using (var context = new AppDBContext())
                {
                    SystemUser u = context.SystemUsers.Find(PosUser.ToLower().Trim());
                    if (u != null)
                    {
                        currentUserFullName = u.Fullname;
                        flag = true;
                    }
                }

            }
            return flag;
        }


        private int ValidatePassword(String username, String password, String pos)
        {
            //return types (0) - failed (1)-okay (-1) - change password

            //get SHA256 of the Password
            SHA256Managed crypt = new SHA256Managed();
            StringBuilder hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password), 0, Encoding.UTF8.GetByteCount(password));
            foreach (byte bit in crypto)
            {
                hash.Append(bit.ToString("x2"));
            }

            using (var context = new AppDBContext())
            {
                SystemUser u = context.SystemUsers.Find(username.ToLower());
                if (u != null)
                {
                    if (u.Password.Equals(hash.ToString()))
                    {
                        //check if it not yet expired
                        int expire = 30; //days to expire
                        TimeSpan diff = DateTime.Now.Subtract(u.LastChangedPass);
                        if (diff.Days >= expire)
                        {
                            //prompt changing password 
                            return -1;
                        }
                        currentUserFullName = u.Fullname;
                        return 1;
                    }
                }
            }
            return 0;
        }

        private void SetDBPath(String dbpath)
        {
            server = ConfigurationManager.AppSettings["server"].ToString();
        }


        private String ValidateClientCode(string ClientCode)
        {
            int myInt;
            DataSet dsS = new DataSet();
            if (ClientCode == " " || ClientCode == "")
            {
                return "true";
            }
            if (!int.TryParse(ClientCode, out myInt)) //if user input is not a number then or is a very big number bigger than int size
            {
                DataTable dtT = new DataTable("Ticket");
                dtT.Columns.Add("TktID");
                dtT.Columns.Add("Cct");
                dtT.Columns.Add("Rte");
                dtT.Columns.Add("Date");
                dtT.Columns.Add("Hr");
                dtT.Columns.Add("CIn");
                dtT.Columns.Add("COut");
                dtT.Columns.Add("TPhone");
                dtT.Columns.Add("Traveller");
                dtT.Columns.Add("CCode");
                dtT.Columns.Add("Price");
                dtT.Columns.Add("Disc");
                dtT.Columns.Add("Ttl");
                dtT.Columns.Add("Pnts");
                dtT.Columns.Add("Pos");
                dtT.Columns.Add("RNum");
                dtT.Columns.Add("RDate");
                dtT.Columns.Add("Msg");
                dtT.Columns.Add("SNo");
                dtT.Columns.Add("DTime");
                dtT.Columns.Add("DocID");

                DataRow rw = dtT.NewRow();
                rw["TktID"] = "0";
                rw["Cct"] = "-";
                rw["Rte"] = "-";
                rw["Date"] = "-";
                rw["Hr"] = "-";
                rw["CIn"] = "-";
                rw["COut"] = "-";
                rw["TPhone"] = "0";
                rw["Traveller"] = "-";
                rw["CCode"] = "0";
                rw["Price"] = "0";
                rw["Disc"] = "0";
                rw["Ttl"] = "0";
                rw["Pnts"] = "0";
                rw["Pos"] = "-";
                rw["RNum"] = "0";
                rw["RDate"] = "-";
                rw["Msg"] = "Bad Client Code";
                rw["SNo"] = "-";
                rw["DTime"] = "-";
                rw["DocID"] = "";

                dtT.Rows.Add(rw);

                dsS.Tables.Add(dtT);

                return dsS.GetXml();
            }
            else
            {
                return "true";
            }
        }
        private Boolean ValidateCode(String ClientCode)
        {
            if (ValidateClientCode(ClientCode) == "true")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private String ReturnMeSingleString(String query, String dbpath)
        {
            //Debug.Assert(!String.IsNullOrEmpty(query), "query is null or empty.");
            String toReturn;
            try
            {
                SqlConnection SqlConnection1;
                SqlDataAdapter myAdp;
                String conString = String.Format("{1};Initial Catalog={0};User Id=sa;Password={2};", dbpath, server, pass);
                SqlConnection1 = new SqlConnection(conString);
                using (SqlConnection1 = new SqlConnection(conString))
                {
                    SqlConnection1.Open();
                    myAdp = new SqlDataAdapter(query, SqlConnection1);
                    DataSet ds = new DataSet();
                    myAdp.Fill(ds);
                    toReturn = ds.Tables[0].Rows[0][0].ToString();
                }

            }
            catch (SqlException ex)
            {
                LogException(ex, dbpath);
                toReturn = "-1";
            }
            catch (Exception e)
            {
                LogException(e, dbpath);
                toReturn = "-1";
            }
            return toReturn;
        }
        private void LogString(String logContent, String dbpath, String path)
        {
            try
            {
                TextWriter tx;
                String pathToLog = Server.MapPath("~") + "\\logs\\logs.txt";
                using (tx = new StreamWriter(pathToLog, true))
                {
                    String logLine = String.Format("{0}  :  {1}  : {2}", DateTime.Now.ToString(), dbpath, logContent);
                    tx.WriteLine(logLine);
                    tx.WriteLine("");
                    tx.WriteLine("------------------------------------------------------------------------------------------------------");
                    tx.WriteLine("");
                    tx.Close();
                }

            }
            catch (IOException ex)
            {
                LogException(ex, dbpath);
            }
            catch (Exception e)
            {
                LogException(e, dbpath);
            }
        }
        private void LogString(String logContent, String dbpath = "")
        {
            try
            {
                TextWriter tx;
                String pathToLog = Server.MapPath("~") + "\\logs\\logs.txt";
                using (tx = new StreamWriter(pathToLog, true))
                {
                    String logLine = String.Format("{0}  :  {1} ", DateTime.Now.ToString(), logContent);
                    tx.WriteLine(logLine);
                    tx.WriteLine("");
                    tx.WriteLine("------------------------------------------------------------------------------------------------------");
                    tx.WriteLine("");
                    tx.Close();
                }

            }
            catch (IOException ex)
            {
                LogException(ex, logContent);
            }
            catch (Exception e)
            {
                LogException(e, logContent);
            }
        }

        private void writeXMLToFile(String logContent, String FileName, String dbpath)
        {
            try
            {
                String folderName = String.Format("{0}_{1}_{2}", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

                String folderPath = @"C:\inetpub\wwwroot\poslogin\logs\pos\" + folderName;

                String pathToLog = @"C:\inetpub\wwwroot\poslogin\logs\pos\" + folderName + @"\" + FileName + ".txt";


                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                StreamWriter writer;

                using (writer = new StreamWriter(pathToLog))
                {
                    writer.Write(logContent);
                    writer.Close();
                }

                //String content = logContent.OuterXml;


                //logContent.Save(writer);
            }
            catch (IOException ex)
            {
                LogException(ex, dbpath);
            }
            catch (Exception e)
            {
                LogException(e, dbpath);
            }
        }

        private void LogException(Exception ex, String dbpath)
        {
            try
            {
                TextWriter tx;
                String path = Server.MapPath("~") + "\\logs\\exceptions_stella.logs.txt";

                using (tx = new StreamWriter(path, true))
                {
                    String logLine = String.Format("{0}  :  {1}  : {2} : {3}", DateTime.Now.ToString(), dbpath, ex.Message.ToString(), ex.Source.ToString());
                    tx.WriteLine(logLine);
                    tx.WriteLine(ex.StackTrace.ToString());
                    tx.WriteLine("");
                    tx.WriteLine("------------------------------------------------------------------------------------------------------");
                    tx.WriteLine("");
                    tx.Close();
                }

            }
            catch (IOException iex)
            {

            }
            catch (Exception e)
            {

            }
        }

        private void LogMe(String logContent)
        {
            try
            {
                TextWriter tx;
                String pathToLog = "C:\\inetpub\\wwwroot\\ucproduction\\logs.txt";
                using (tx = new StreamWriter(pathToLog, true))
                {
                    String logLine = String.Format("{0}  :  {1} ", DateTime.Now.ToString(), logContent);
                    tx.WriteLine(logLine);
                    tx.WriteLine("");
                    tx.WriteLine("------------------------------------------------------------------------------------------------------");
                    tx.WriteLine("");
                    tx.Close();
                }

            }
            catch (IOException ex)
            {
                LogException(ex, logContent);
            }
            catch (Exception e)
            {
                LogException(e, logContent);
            }
        }

        public bool HasColumn(SqlDataReader Reader, string ColumnName)
        {
            foreach (DataRow row in Reader.GetSchemaTable().Rows)
            {
                if (row["ColumnName"].ToString() == ColumnName)
                    return true;
            }
            //Still here? Column not found. 
            return false;
        }
    }
}
