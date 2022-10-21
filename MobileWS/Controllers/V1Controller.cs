using MobileWS.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace MobileWS.Controllers
{
    public static class HttpRequestMessageExtensions
    {
        //credits http://www.strathweb.com/2013/05/retrieving-the-clients-ip-address-in-asp-net-web-api/
        private const string HttpContext = "MS_HttpContext";
        private const string RemoteEndpointMessage = "System.ServiceModel.Channels.RemoteEndpointMessageProperty";

        public static string GetClientIpAddress(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey(HttpContext))
            {
                dynamic ctx = request.Properties[HttpContext];
                if (ctx != null)
                {
                    return ctx.Request.UserHostAddress;
                }
            }

            if (request.Properties.ContainsKey(RemoteEndpointMessage))
            {
                dynamic remoteEndpoint = request.Properties[RemoteEndpointMessage];
                if (remoteEndpoint != null)
                {
                    return remoteEndpoint.Address;
                }
            }

            return null;
        }
    }

    public class V1Controller : ApiController
    {
        //private String connStr = "Data Source=INNOVYS-STEFANO\\SQLEXPRESS;User id=sa;Initial Catalog=#company;Password=jesus;";
        //private String connStr = "Data Source=GIFT\\SQLEXPRESS;User id=sa;Initial Catalog=#company;Password=jesus;";
        private String connStr = "Data Source=localhost;User id=sa;Initial Catalog=#company;Password=Sinnovys&750;";

        [Route("api/V1/trips/{company}/{id}")]
        public HttpResponseMessage Get(String company, String id)
        {
            if (!checkSecurity(id))
            {
                var response = Request.CreateResponse(HttpStatusCode.Forbidden);
                response.Content = new StringContent("Request could not be verified", Encoding.UTF8, "application/json");
                return response;
            }

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            String sql = "SELECT NAME, PRICE1 AS RWF, PRICE2 AS FIB FROM su.PRODUCTS WHERE NAME NOT LIKE '%TRIP%' AND NAME NOT LIKE '%courier%' AND NAME NOT LIKE '%Service%' AND NAME NOT LIKE '%Subscription%' AND NAME NOT LIKE '%Transport%' AND NAME NOT LIKE '%(int)%'";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr.Replace("#company", company)))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                            {

                                jsonWriter.WriteStartArray();

                                while (rdr.Read())
                                {
                                    jsonWriter.WriteStartObject();

                                    int fields = rdr.FieldCount;

                                    for (int i = 0; i < fields; i++)
                                    {
                                        jsonWriter.WritePropertyName(rdr.GetName(i));
                                        jsonWriter.WriteValue(rdr[i]);
                                    }

                                    jsonWriter.WriteEndObject();
                                }

                                jsonWriter.WriteEndArray();
                            }
                        }

                    }
                }
                StringBuilder sb2 = new StringBuilder();
                StringWriter sw2 = new StringWriter(sb2);
                using (JsonWriter writer2 = new JsonTextWriter(sw2))
                {
                    writer2.Formatting = Formatting.Indented;
                    writer2.WriteStartObject();
                    writer2.WritePropertyName("CODE");
                    writer2.WriteValue("0");
                    writer2.WritePropertyName("MSG");
                    writer2.WriteValue(sb.ToString());
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(sb2.ToString(), Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception e)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\inetpub\wwwroot\MobileWS\logs.txt");
                file.WriteLine(e.ToString());
                file.Close();

                StringBuilder sb2 = new StringBuilder();
                StringWriter sw2 = new StringWriter(sb2);
                using (JsonWriter writer2 = new JsonTextWriter(sw2))
                {
                    writer2.Formatting = Formatting.Indented;
                    writer2.WriteStartObject();
                    writer2.WritePropertyName("CODE");
                    writer2.WriteValue("111");
                    writer2.WritePropertyName("MSG");
                    writer2.WriteValue("Could not Fetch Routes. Internal Error occured");
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(sb2.ToString(), Encoding.UTF8, "application/json");
                return response;
            }
        }

        [Route("api/V1/schedules/{company}/{id}/{trip}/{date}")]
        public HttpResponseMessage Get(String company, String id, String trip, String date)
        {
            if (!checkSecurity(id))
            {
                var response = Request.CreateResponse(HttpStatusCode.Forbidden);
                response.Content = new StringContent("Request could not be verified", Encoding.UTF8, "application/json");
                return response;
            }

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            String sql = "SELECT FLD163 AS TIME, FLD103 AS SEATS  FROM su.SALES WHERE CONVERT(DATE, TARGET_DATE) = CONVERT(DATE, '" + date + "', 103) AND FLD103>5";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr.Replace("#company", company)))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                            {

                                jsonWriter.WriteStartArray();

                                while (rdr.Read())
                                {
                                    jsonWriter.WriteStartObject();

                                    int fields = rdr.FieldCount;

                                    for (int i = 0; i < fields; i++)
                                    {
                                        jsonWriter.WritePropertyName(rdr.GetName(i));
                                        jsonWriter.WriteValue(rdr[i]);
                                    }

                                    jsonWriter.WriteEndObject();
                                }

                                jsonWriter.WriteEndArray();
                            }
                        }

                    }
                }
                StringBuilder sb2 = new StringBuilder();
                StringWriter sw2 = new StringWriter(sb2);
                using (JsonWriter writer2 = new JsonTextWriter(sw2))
                {
                    writer2.Formatting = Formatting.Indented;
                    writer2.WriteStartObject();
                    writer2.WritePropertyName("CODE");
                    writer2.WriteValue("0");
                    writer2.WritePropertyName("MSG");
                    writer2.WriteValue(sb.ToString());
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(sb2.ToString(), Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb2 = new StringBuilder();
                StringWriter sw2 = new StringWriter(sb2);
                using (JsonWriter writer2 = new JsonTextWriter(sw2))
                {
                    writer2.Formatting = Formatting.Indented;
                    writer2.WriteStartObject();
                    writer2.WritePropertyName("CODE");
                    writer2.WriteValue("111");
                    writer2.WritePropertyName("MSG");
                    writer2.WriteValue("Could not Fetch Hours. Internal Error occured");
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(sb2.ToString(), Encoding.UTF8, "application/json");
                return response;
            }
        }

        [Route("api/V1/balance/{company}/{id}/{walletID}/{dummy}")]
        public HttpResponseMessage Get(String company, String id, int walletId, int dummy)
        {
            if (!checkSecurity(id))
            {
                var resp = Request.CreateResponse(HttpStatusCode.Forbidden);
                resp.Content = new StringContent("Request could not be verified", Encoding.UTF8, "application/json");
                return resp;
            }

            try {
                StringBuilder sb = new StringBuilder();
                StringWriter sw = new StringWriter(sb);
                using (Entities db = new Entities())
                { 
                        Wallet wallet = db.Wallets.FirstOrDefault(w => w.IsActive == 1 && w.Company == company);
                    if (wallet != null)
                    {
                        using (JsonWriter writer = new JsonTextWriter(sw))
                        {
                            writer.Formatting = Formatting.Indented;
                            writer.WriteStartObject();
                            writer.WritePropertyName("CODE");
                            writer.WriteValue("0");
                            writer.WritePropertyName("MSG");
                            writer.WriteValue(wallet.Amount);
                        }
                        var resp = Request.CreateResponse(HttpStatusCode.OK);
                        resp.Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
                        return resp;

                    }
                }
            }
            catch(Exception e)
            {

            }

            StringBuilder sb2 = new StringBuilder();
            StringWriter sw2 = new StringWriter(sb2);
            using (JsonWriter writer2 = new JsonTextWriter(sw2))
            {
                writer2.Formatting = Formatting.Indented;
                writer2.WriteStartObject();
                writer2.WritePropertyName("CODE");
                writer2.WriteValue("10");
                writer2.WritePropertyName("MSG");
                writer2.WriteValue("Invalid or Locked Wallet");
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(sb2.ToString(), Encoding.UTF8, "application/json");
            return response;
        }


        [Route("api/V1/check-code/{company}/{id}/{code}")]
        public HttpResponseMessage Get(String company, String id, String code)
        {
            if (!checkSecurity(id))
            {
                var response = Request.CreateResponse(HttpStatusCode.Forbidden);
                response.Content = new StringContent("Request could not be verified", Encoding.UTF8, "application/json");
                return response;
            }

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            String sql = "SELECT CONVERT(int, IDCONTACT) AS CODE, CONCAT(NAME, ' ', FIRSTNAME) AS FULLNAME FROM su.CONT WHERE IDCONTACT=" + code + "; ";
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr.Replace("#company", company)))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                            {

                                jsonWriter.WriteStartArray();

                                while (rdr.Read())
                                {
                                    jsonWriter.WriteStartObject();

                                    int fields = rdr.FieldCount;

                                    for (int i = 0; i < fields; i++)
                                    {
                                        jsonWriter.WritePropertyName(rdr.GetName(i));
                                        jsonWriter.WriteValue(rdr[i]);
                                    }

                                    jsonWriter.WriteEndObject();
                                }

                                jsonWriter.WriteEndArray();
                            }
                        }

                    }
                }
                StringBuilder sb2 = new StringBuilder();
                StringWriter sw2 = new StringWriter(sb2);
                using (JsonWriter writer2 = new JsonTextWriter(sw2))
                {
                    writer2.Formatting = Formatting.Indented;
                    writer2.WriteStartObject();
                    writer2.WritePropertyName("CODE");
                    writer2.WriteValue("0");
                    writer2.WritePropertyName("MSG");
                    writer2.WriteValue(sb.ToString());
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(sb2.ToString(), Encoding.UTF8, "application/json");
                return response;
            }
            catch (Exception e)
            {
                StringBuilder sb2 = new StringBuilder();
                StringWriter sw2 = new StringWriter(sb2);
                using (JsonWriter writer2 = new JsonTextWriter(sw2))
                {
                    writer2.Formatting = Formatting.Indented;
                    writer2.WriteStartObject();
                    writer2.WritePropertyName("CODE");
                    writer2.WriteValue("111");
                    writer2.WritePropertyName("MSG");
                    writer2.WriteValue("Could not Fetch Code. Internal Error occured");
                }

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent(sb2.ToString(), Encoding.UTF8, "application/json");
                return response;
            }
        }


        [Route("api/V1/reserve")]
        public HttpResponseMessage Post([FromBody]JToken json)
        {
            if(!checkSecurity(json.Value<String>("id")))
            {
                var response = Request.CreateResponse(HttpStatusCode.Forbidden);
                response.Content = new StringContent("Request could not be verified", Encoding.UTF8, "application/json");
                return response;
            }

            //check if have enough money


            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            String errorMsg = ""; 

            StringBuilder sbx = new StringBuilder();
            StringWriter swx = new StringWriter(sbx);

            using (SqlConnection conn = new SqlConnection(connStr.Replace("#company", json.Value<String>("company"))))
            {
                conn.Open(); 

                SqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    int retCode = 0;

                    using (SqlCommand cmdCheck = new SqlCommand())
                    {
                        cmdCheck.CommandText = String.Format("SELECT PRICE1 FROM su.Products WHERE Name='{0}-{1}'", json.Value<String>("depature"), json.Value<String>("destination"));
                        cmdCheck.Connection = conn;
                        cmdCheck.Transaction = transaction;

                        using (SqlDataReader rdr = cmdCheck.ExecuteReader())
                        {
                            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                            {

                                while (rdr.Read())
                                {
                                    int price = int.Parse(rdr["PRICE1"].ToString());
                                    using (Entities db = new Entities())
                                    {
                                        String userCompanyKey = json.Value<String>("company");  
                                            Wallet wallet = db.Wallets.FirstOrDefault(w => w.IsActive == 1 && w.Company == userCompanyKey);
                                        if (wallet != null)
                                        {
                                            if (wallet.Amount < price)
                                            {
                                                using (JsonWriter writer = new JsonTextWriter(sw))
                                                {
                                                    writer.Formatting = Formatting.Indented;
                                                    writer.WriteStartObject();
                                                    writer.WritePropertyName("CODE");
                                                    writer.WriteValue("11");
                                                    writer.WritePropertyName("MSG");
                                                    writer.WriteValue("Insufficient funds in wallet");
                                                }
                                                var resp = Request.CreateResponse(HttpStatusCode.OK);
                                                resp.Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
                                                return resp;
                                            }
                                        }
                                        else
                                        {
                                            using (JsonWriter writer = new JsonTextWriter(sw))
                                            {
                                                writer.Formatting = Formatting.Indented;
                                                writer.WriteStartObject();
                                                writer.WritePropertyName("CODE");
                                                writer.WriteValue("10");
                                                writer.WritePropertyName("MSG");
                                                writer.WriteValue("Invalid or Locked wallet");
                                            }
                                            var resp = Request.CreateResponse(HttpStatusCode.OK);
                                            resp.Content = new StringContent(sb.ToString(), Encoding.UTF8, "application/json");
                                            return resp;
                                        } 
                                    }
                                }
                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "su.Prebooking";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@DATE", json.Value<String>("date").Replace("-", "/"));
                        cmd.Parameters.AddWithValue("@HOUR", json.Value<String>("time"));
                        cmd.Parameters.AddWithValue("@CityIN", json.Value<String>("depature"));
                        cmd.Parameters.AddWithValue("@CityOut", json.Value<String>("destination"));
                        cmd.Parameters.AddWithValue("@Firstname", json.Value<String>("name"));
                        cmd.Parameters.AddWithValue("@NAME", "");
                        cmd.Parameters.AddWithValue("@ClientCode", json.Value<int>("code"));
                        cmd.Parameters.AddWithValue("@Discount", 0);
                        //cmd.Parameters.AddWithValue("@Creator", json.Value<String>("id"));
                        cmd.Parameters.AddWithValue("@Creator", "OLTRANZ");
                        cmd.Parameters.AddWithValue("@Currency", "RWF");  
                        cmd.Parameters.AddWithValue("@IsMobile", 1);

                        if (json.Value<String>("company").ToLower()=="oltranz"|| json.Value<String>("company").ToLower()=="ugusenga")
                        {
                            cmd.Parameters.AddWithValue("@PREFSEAT", "Yes");
                        }

                        //companies without bus type. Note oltranz is a test company and can be removed in production
                        String withNoBusType = "volcano ugusenga oltranz";

                        if (!withNoBusType.Contains(json.Value<String>("company").ToLower()))
                        {
                            //cmd.Parameters.AddWithValue("@PREFSEAT", 0);

                            String BusType = json.Value<String>("bustype");
                            if (String.IsNullOrEmpty(BusType))
                            {
                                BusType = "Express";
                            }

                            if (BusType == "OT" || BusType.ToLower().Contains("other"))
                            {
                                //The default
                                cmd.Parameters.AddWithValue("@BusCategory", "OTHER");
                            }
                            else if (BusType.ToLower().Contains("express"))
                            {
                                cmd.Parameters.AddWithValue("@BusCategory", "EXPRESS");
                            }
                        } 

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            using (JsonWriter jsonWriter2 = new JsonTextWriter(swx))
                            {

                                jsonWriter2.WriteStartArray();


                                while (rdr.Read())
                                {
                                    String msg = rdr["Message"].ToString();
                                    errorMsg = msg;
                                    if (long.Parse(rdr["BookingNo"].ToString()) == 0)
                                        retCode = -1;

                                    String skip = "BusName Circuit Message Pos SeatNo";

                                    jsonWriter2.WriteStartObject();

                                    int fields = rdr.FieldCount;

                                    for (int i = 0; i < fields; i++)
                                    {
                                        if (skip.Contains(rdr.GetName(i))) //skip all fields in skip string
                                            continue;

                                        jsonWriter2.WritePropertyName(rdr.GetName(i));
                                        jsonWriter2.WriteValue(rdr[i]);
                                    }

                                    jsonWriter2.WriteEndObject();
                                }

                                jsonWriter2.WriteEndArray();
                            }
                        }
                    }
                    StringBuilder sb2 = new StringBuilder();
                    StringWriter sw2 = new StringWriter(sb2);
                    using (JsonWriter writer3 = new JsonTextWriter(sw2))
                    {
                        writer3.Formatting = Formatting.Indented;
                        writer3.WriteStartObject();
                        writer3.WritePropertyName("CODE");
                       if(retCode==0)
                       {
                            writer3.WriteValue("0");
                            writer3.WritePropertyName("MSG");
                            writer3.WriteValue(sbx.ToString());
                           transaction.Commit();
                       }
                       else
                       {
                            writer3.WriteValue("-1");
                            writer3.WritePropertyName("MSG");
                            writer3.WriteValue(errorMsg);
                           //writer2.WriteValue("Could Not reserve the Seat. Check Parameters");
                           transaction.Rollback();
                       }
                    }
                     

                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(sb2.ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    StringBuilder sb2 = new StringBuilder();
                    StringWriter sw2 = new StringWriter(sb2);
                    using (JsonWriter writer2 = new JsonTextWriter(sw2))
                    {
                        writer2.Formatting = Formatting.Indented;
                        writer2.WriteStartObject();
                        writer2.WritePropertyName("CODE");
                        writer2.WriteValue("111");
                        writer2.WritePropertyName("MSG");
                        writer2.WriteValue("Could not reserve the seat. Check parameters");
                    }

                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(sb2.ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
            }
        }

        [Route("api/V1/purchase/{company}/{id}/{code}")]
        public HttpResponseMessage Get(String company, String id, int code)
        {
            if (!checkSecurity(id))
            {
                var response = Request.CreateResponse(HttpStatusCode.Forbidden);
                response.Content = new StringContent("Request could not be verified", Encoding.UTF8, "application/json");
                return response;
            }

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            StringBuilder sbx = new StringBuilder();
            StringWriter swx = new StringWriter(sbx);

            using (SqlConnection conn = new SqlConnection(connStr.Replace("#company", company)))
            {
                conn.Open(); 

                SqlTransaction transaction = conn.BeginTransaction();
                Entities db = new Entities();
                Wallet wallet = null;
                User user = null;
                int price = 0;
                try
                {
                    int retCode = 0;

                    using (SqlCommand cmdCheck = new SqlCommand())
                    {
                        cmdCheck.CommandText = String.Format("SELECT PRICE1 FROM su.PRODUCTS WHERE NAME = (SELECT CONCAT(CityIn, '-', CityOut) FROM Prebookings WHERE BookingNo={0})", code);
                        cmdCheck.Connection = conn;
                        cmdCheck.Transaction = transaction;

                        using (SqlDataReader rdr = cmdCheck.ExecuteReader())
                        {
                            using (JsonWriter jsonWriter = new JsonTextWriter(sw))
                            {

                                while (rdr.Read())
                                {
                                    price = int.Parse(rdr["PRICE1"].ToString()); 
                                        wallet = db.Wallets.FirstOrDefault(w => w.IsActive == 1 && w.Company == company);
                                    if (wallet != null)
                                    {
                                        if (wallet.Amount < price || wallet.IsActive==0)
                                        {
                                            using (JsonWriter writerx = new JsonTextWriter(swx))
                                            {
                                                writerx.Formatting = Formatting.Indented;
                                                writerx.WriteStartObject();
                                                writerx.WritePropertyName("CODE");
                                                writerx.WriteValue("11");
                                                writerx.WritePropertyName("MSG");
                                                writerx.WriteValue("Insufficient funds in, or Suspended wallet");
                                            }
                                            var resp = Request.CreateResponse(HttpStatusCode.OK);
                                            resp.Content = new StringContent(sbx.ToString(), Encoding.UTF8, "application/json");
                                            return resp;
                                        }

                                    }
                                    else
                                    {
                                        using (JsonWriter writerx = new JsonTextWriter(swx))
                                        {
                                            writerx.Formatting = Formatting.Indented;
                                            writerx.WriteStartObject();
                                            writerx.WritePropertyName("CODE");
                                            writerx.WriteValue("10");
                                            writerx.WritePropertyName("MSG");
                                            writerx.WriteValue("Invalid or Locked wallet");
                                        }
                                        var resp = Request.CreateResponse(HttpStatusCode.OK);
                                        resp.Content = new StringContent(sbx.ToString(), Encoding.UTF8, "application/json");
                                        return resp;
                                    }
                                }
                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "dbo.PullPreBooking";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = conn;
                        cmd.Transaction = transaction;

                        cmd.Parameters.AddWithValue("@BookingNo", code);
                         
                        Company co = db.Companies.FirstOrDefault(c => c.Name==company);
                        user = db.Users.FirstOrDefault(u => u.UserCompany == company);
                        if (co != null)
                        {
                            cmd.Parameters.AddWithValue("@Creator", co.POS);
                        }
                         
                        if (company.ToLower() != "oltranz" && company.ToLower() != "ugusenga" && company.ToLower() != "volcano")
                        {
                            cmd.Parameters.AddWithValue("@POSUSER", user.Name);
                        }

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            using (JsonWriter jsonWriter = new JsonTextWriter(swx))
                            {
                                String TicketID;
                                String Route;
                                String Price;
                                jsonWriter.WriteStartArray();

                                String skip = "CIRCUIT TrajetPHONE POS USERNAME RESNUM MESSAGE Seatno";
                                while (rdr.Read())
                                {
                                    String message = rdr["MESSAGE"].ToString();
                                    TicketID = rdr["TicketID"].ToString();

                                    if (long.Parse(TicketID) == 0)
                                        retCode = -1;

                                    Route = rdr["ROUTE"].ToString(); 


                                    jsonWriter.WriteStartObject();

                                    int fields = rdr.FieldCount;

                                    for (int i = 0; i < fields; i++)
                                    {
                                        if (skip.Contains(rdr.GetName(i))) //skip all fields in skip string
                                            continue;

                                        jsonWriter.WritePropertyName(rdr.GetName(i).Trim(' '));
                                        jsonWriter.WriteValue(rdr[i]);
                                    }

                                    jsonWriter.WriteEndObject();
                                    //do the payment for that ticket
                                    if(wallet!=null)
                                    {
                                        wallet.Amount = wallet.Amount - price;
                                        db.Entry(wallet).State = EntityState.Modified;
                                        if(db.SaveChanges()>0)
                                        {
                                            //write logs
                                            WalletLog log = new WalletLog();
                                            log.Wallet = wallet.ID;
                                            log.CreatedBy = user.ID;
                                            log.CreatedOn = DateTime.Now;
                                            log.Comment = Route+ " - "+price.ToString();//swx.ToString();
                                            log.Amount = wallet.Amount;
                                            log.OwnerID = wallet.Company;
                                            log.ReferenceNumber = TicketID;
                                            log.ReferenceType = "Ticket";
                                            log.Action = "Purchase Ticket";
                                            db.WalletLogs.Add(log);
                                            db.SaveChanges();
                                        }
                                        db.Dispose();
                                    }
                                }

                                jsonWriter.WriteEndArray();
                            }
                        }
                    }
                    StringBuilder sb2 = new StringBuilder();
                    StringWriter sw2 = new StringWriter(sb2);
                    using (JsonWriter writer2 = new JsonTextWriter(sw2))
                    {
                        writer2.Formatting = Formatting.Indented;
                        writer2.WriteStartObject();
                        writer2.WritePropertyName("CODE");
                       if(retCode==0)
                       {
                           writer2.WriteValue("0");
                           writer2.WritePropertyName("MSG");
                           writer2.WriteValue(sbx.ToString());
                           transaction.Commit();
                       }
                       else
                       {
                           writer2.WriteValue("-1");
                           writer2.WritePropertyName("MSG");
                           writer2.WriteValue("Could Not reserve the Seat. Check Parameters");
                           transaction.Rollback();
                       }
                    }
                     

                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(sb2.ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
                catch (Exception e)
                {
                    transaction.Rollback();

                    StringBuilder sb2 = new StringBuilder();
                    StringWriter sw2 = new StringWriter(sb2);
                    using (JsonWriter writer2 = new JsonTextWriter(sw2))
                    {
                        writer2.Formatting = Formatting.Indented;
                        writer2.WriteStartObject();
                        writer2.WritePropertyName("CODE");
                        writer2.WriteValue("111");
                        writer2.WritePropertyName("MSG");
                        writer2.WriteValue("Could not reserve the seat. Check parameters");
                    }

                    var response = Request.CreateResponse(HttpStatusCode.OK);
                    response.Content = new StringContent(sb2.ToString(), Encoding.UTF8, "application/json");
                    return response;
                }
            }
        }

         private bool checkSecurity(String id)
         {
            using (var db = new Entities())
            {
                User user = db.Users.FirstOrDefault(u => u.UserKey == id);
                if (user == null)
                    return false; //no such key have user
            }

            String ip = Request.GetClientIpAddress();

             if (String.IsNullOrEmpty(ip))
                 return false;

             //if (ip != "41.74.171.131" &&  ip!="41.74.171.132")
                 //return false; 

             return true;
         }
    }
}
