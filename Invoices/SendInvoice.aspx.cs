 using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Web.Configuration;
using System.Web.UI;
using TuesPechkin;
  
using System.ComponentModel; 
using System.Net;
using System.Net.Mail;
using System.Net.Mime; 
using System.Threading;

public class Emails
{ 
    public DateTime EmailInvoiceDate;
    private string FromEmail;
    private string FromHost;
    private string SMTPPassword;

    public Emails()
    { 
        this.FromEmail = "billing@innovys.co.rw";
        this.FromHost = "mail.innovys.co.rw";
        this.SMTPPassword = "w{xxFLD}EdZB";
        this.EmailInvoiceDate = DateTime.Today;
    }

    public Emails(DateTime InvoiceDate)
    { 
        this.FromEmail = "billing@innovys.co.rw";
        this.FromHost = "mail.innovys.co.rw";
        this.SMTPPassword = "w{xxFLD}EdZB";
        this.EmailInvoiceDate = InvoiceDate;
    }

    public void SendEmail(string ToEmail, String sms, String attachment=null)
    {
        SmtpClient client = new SmtpClient(this.FromHost); 
        client.Credentials = new NetworkCredential(this.FromEmail, this.SMTPPassword);
        MailAddress from = new MailAddress(this.FromEmail, "Innovys Ltd ", Encoding.UTF8);
        MailAddress to = new MailAddress("innovys@innovys.co.rw");
        MailMessage message = new MailMessage(from, to);
        //multiple Emails?
        if (ToEmail.Contains(";"))
        {
            string[] emails = ToEmail.Split(';');
            foreach (string ccEmail in emails)
            {
                MailAddress cc = new MailAddress(ccEmail);
                message.To.Add(cc);
            }
        }
        else
        {
            MailAddress cc = new MailAddress(ToEmail);
            message.To.Add(cc);
        }
        message.IsBodyHtml = true;
        message.Body = sms;
        message.Subject = "Invoices for using Sinnovys Ticketing Platform";

        if(attachment!=null)
        {
            string file = "data.xls";
            // Create  the file attachment for this e-mail message.
            Attachment data = new Attachment(attachment, MediaTypeNames.Application.Octet);
            data.Name = "Invoice File.pdf";
            // Add time stamp information for the file.
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.IO.File.GetCreationTime(file);
            disposition.ModificationDate = System.IO.File.GetLastWriteTime(file);
            disposition.ReadDate = System.IO.File.GetLastAccessTime(file);
            // Add the file attachment to this e-mail message.
            message.Attachments.Add(data);
        }
        client.Send(message);
        message.Dispose();
        client.Dispose();
    }
     
}
  

public class InvoiceCalculator
{ 
    private string CustNo;
    private DateTime IDate; 
    public string InvoiceCompany;

    public InvoiceCalculator(String company)
    {
        this.InvoiceCompany = company;   
        this.IDate = DateTime.Today.AddDays(-1);
    }

    public InvoiceCalculator(DateTime DateOfInvoice, String company)
    {
        this.InvoiceCompany = company;
        this.IDate = DateOfInvoice;
    }
       
     

    public double GetTotalBilledTickets()
    {
        string str = ConfigurationManager.AppSettings["POSExempt"];
        string selectstmt = string.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE DAY(DATE_MOD)={0} AND  MONTH(DATE_MOD)={1} AND YEAR(DATE_MOD)={2} ", new object[] { Convert.ToInt32(this.IDate.Day), Convert.ToInt32(this.IDate.Month), Convert.ToInt32(this.IDate.Year) });
        double num2 = ReturnSingleNumeric(selectstmt);
        return num2;
    }

    public string GetTotalTickets()
    {
        string selectstmt = string.Format("SELECT COUNT(IDRELATION) FROM su.SALES_PROD WHERE DAY(DATE_MOD)={0} AND  MONTH(DATE_MOD)={1} AND YEAR(DATE_MOD)={2}", new object[] { Convert.ToInt32(this.IDate.Day), Convert.ToInt32(this.IDate.Month), Convert.ToInt32(this.IDate.Year) });
        double num = ReturnSingleNumeric(selectstmt);
        return num.ToString();
    }

    public string GetInvoiceNumber()
    {
        String inumber = String.Format("{0}/{1}/{2}/{3}", InvoiceCompany.Substring(0, 3), IDate.Day, IDate.Month, IDate.Year);
        return inumber.ToUpper();
    }

    public string GetInvoiceDate()
    {
        String date = String.Format("{0}/{1}/{2}", IDate.Day, IDate.Month, IDate.Year);
        return date;
    }


    public string NumberToWords(double p)
    {
        int realP = Convert.ToInt32(p);
        return NumberToWords(realP);
    }

    public string NumberToWords(int number)
    {
        if (number == 0)
        {
            return "zero";
        }
        if (number < 0)
        {
            return ("minus " + NumberToWords(Math.Abs(number)));
        }
        string str2 = "";
        if ((number / 0xf4240) > 0)
        {
            str2 = str2 + NumberToWords(number / 0xf4240) + " million ";
            number = number % 0xf4240;
        }
        if ((number / 0x3e8) > 0)
        {
            str2 = str2 + NumberToWords(number / 0x3e8) + " thousand ";
            number = number % 0x3e8;
        }
        if ((number / 100) > 0)
        {
            str2 = str2 + NumberToWords(number / 100) + " hundred ";
            number = number % 100;
        }
        if (number > 0)
        {
            if (str2 != "")
            {
                str2 = str2 + "and ";
            }
            string[] strArray2 = new string[] { 
                "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", 
                "sixteen", "seventeen", "eighteen", "nineteen"
             };
            string[] strArray = new string[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };
            if (number < 20)
            {
                str2 = str2 + strArray2[number];
            }
            else
            {
                str2 = str2 + strArray[number / 10];
                if ((number % 10) > 0)
                {
                    str2 = str2 + "-" + strArray2[number % 10];
                }
            }
        }
        return str2;
    } 

    private double ReturnSingleNumeric(String sql)
    {

        double num;
        try
        {
            double num2 = 0.0;
            using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["Conn"].ConnectionString.Replace("Invoices", this.InvoiceCompany)))
            {
                connection.Open();
                SqlDataReader reader = new SqlCommand(sql, connection).ExecuteReader();
                if (reader.Read())
                {
                    num2 = Convert.ToDouble(reader[0].ToString());
                }
                else
                {
                    num2 = 0.0;
                }
                connection.Close();
            }
            num = num2;
        }
        catch (Exception exception1)
        { 
            Exception ex = exception1; 
            num = 0.0; 
            return num; 
        }
        return num;
    }

}




public partial class SendInvoice : System.Web.UI.Page
{
    private static readonly IConverter _pdfConverter =
     new ThreadSafeConverter(
        new RemotingToolset<PdfToolset>(
            new Win32EmbeddedDeployment(
                new TempFolderDeployment())));



    protected void Page_Load(object sender, EventArgs e)
    {  
         String id = Request.QueryString["c"];
         String url = null;
         String path = null;
         String email = null;
        bool IsFixed = false;

         if (String.IsNullOrEmpty(id))
             Response.Redirect("Default.aspx"); ;

        using (SqlConnection connection = new SqlConnection(WebConfigurationManager.ConnectionStrings["Conn"].ConnectionString))
        {
            connection.Open(); using (SqlCommand cmd = new SqlCommand("SELECT * FROM Companies WHERE ID='"+id+"'", connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            email = reader["Email"].ToString();
                            IsFixed = Boolean.Parse(reader["IsFixed"].ToString());

                            using (StreamReader streamReader = new StreamReader(Server.MapPath("~/PDF/template.html"), Encoding.UTF8))
                            {
                                String readContents = streamReader.ReadToEnd();

                                using (StreamWriter sw = new StreamWriter(Server.MapPath("~/PDF/" + reader["ID"] + ".html")))
                                {
                                    readContents = readContents.Replace("[Company]", reader["FullName"].ToString());
                                    readContents = readContents.Replace("[BillTo]", reader["BillTo"].ToString().Replace(Environment.NewLine, "<br />"));
                                    readContents = readContents.Replace("[AddressTo]", reader["AddressTo"].ToString().Replace(Environment.NewLine, "<br />"));
                                    readContents = readContents.Replace("[BaseAmount]", reader["Cost"].ToString());

                                    InvoiceCalculator ic = new InvoiceCalculator(reader["DBName"].ToString());
                                    double tickets = ic.GetTotalBilledTickets();
                                    double totalMoney = 0;
                                    if(IsFixed)
                                        totalMoney =  double.Parse(reader["Cost"].ToString());
                                    else
                                        totalMoney = tickets * double.Parse(reader["Cost"].ToString());
                                    double VAT = 0.18; //18%
                                    double vatVal = VAT * totalMoney;
                                    readContents = readContents.Replace("[TicketsBilled]", tickets.ToString());
                                    readContents = readContents.Replace("[TotalTicketsNumber]", tickets.ToString());
                                    readContents = readContents.Replace("[InvoiceAmount]", totalMoney.ToString());
                                    readContents = readContents.Replace("[SubTotalAmount]", totalMoney.ToString());
                                    readContents = readContents.Replace("[VAT]", vatVal.ToString());
                                    readContents = readContents.Replace("[TotalVATed]", (totalMoney + vatVal).ToString());
                                    readContents = readContents.Replace("[TotalWords]", ic.NumberToWords(totalMoney + vatVal));

                                    readContents = readContents.Replace("[InvoiceNumber]", ic.GetInvoiceNumber());
                                    readContents = readContents.Replace("[InvoiceDate]", ic.GetInvoiceDate());
                                    readContents = readContents.Replace("[TimeGenerated]", ic.GetInvoiceDate()); 
                                                                        
                                    sw.Write(readContents); 
                                    // create a new pdf document converting an url
                                    url = new System.Uri(Page.Request.Url, "/PDF/" + reader["ID"] + ".html").AbsoluteUri;
                                    path = Server.MapPath("~/PDF/" + reader["ID"] + ".pdf"); 

                                    break;
                                }
                            }

                        }
                    }
                } // reader closed and disposed up here

            } // command disposed here

        } //connection closed and disposed here 

        GetPDFAttachment(url, path, email);
    }

    public void GetPDFAttachment(string InvoiceURL, string path, string email)
    {
        String PDFURL = InvoiceURL;
        PDFURL = PDFURL.Replace("html", "pdf");
         

        string html = string.Empty;
        using (StreamReader streamReader = new StreamReader(path.Replace("pdf", "html"), Encoding.UTF8))
        {
            html = streamReader.ReadToEnd();
        }


        var document = new HtmlToPdfDocument
       {
           GlobalSettings =
           {
               ProduceOutline = false,
               
               DocumentTitle = "Invoice PDF",
               PaperSize = PaperKind.A4, // Implicit conversion to PechkinPaperSize
               /*Margins =
               {
                   All = 1.375,
                   Unit = Unit.Centimeters
               }*/
               UseCompression = false,
           },
           Objects = { 
                new ObjectSettings{ PageUrl = path.Replace("pdf", "html") }
            }
       };
       
        byte[] result =  _pdfConverter.Convert(document); 
        File.WriteAllBytes(path, result); 
        //send email
        Emails mailer = new Emails();
        string msg = string.Format("<html><body>Hello, <br/><p>Thank you for utilizing the Sinnovys Platform for Ticketing.</p><p>Please find attached a copy of the days Invoice.</p><p>Regards, <br/>Innovys Ltd</p><p>This email was autogenerated at {0:yyyy/MM/dd h.mm.ss tt}</p></body></html>", DateTime.Now);
        mailer.SendEmail(email, msg, path);

        Response.Redirect(PDFURL);
    }  

} 
