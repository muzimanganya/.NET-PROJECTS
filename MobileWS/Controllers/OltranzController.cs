using MobileWS.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;

namespace MobileWS.Controllers
{
    public class OltranzController : Controller
    {
        private Entities db = new Entities();

        // GET: Oltranz
        public ActionResult Index(int? id, int? page)
        {
            IEnumerable<WalletLog> walletLogs = null;
            if (HttpContext.Request.HttpMethod == "POST" && !String.IsNullOrEmpty(this.HttpContext.Request.Form["RefTypes"]) && this.HttpContext.Request.Form["RefTypes"] != "")
            {
                String type = this.HttpContext.Request.Form["RefTypes"];
                if (id == null)
                    walletLogs = db.WalletLogs.Include(w => w.User).Include(w => w.Wallet1).Where(w =>  w.ReferenceType == type).OrderBy(w => w.ID);
                else
                    walletLogs = db.WalletLogs.Include(w => w.User).Include(w => w.Wallet1).Where(w => w.Wallet == id && w.ReferenceType == type).OrderBy(w => w.ID);
            }
            else
            {
                if (id == null)
                    walletLogs = db.WalletLogs.Include(w => w.User).Include(w => w.Wallet1).OrderBy(w => w.ID);
                else
                    walletLogs = db.WalletLogs.Include(w => w.User).Include(w => w.Wallet1).Where(w => w.Wallet == id).OrderBy(w => w.ID);
            }

            var pageNumber = page ?? 1;
            var onePageOfProducts = walletLogs.ToPagedList(pageNumber, 10);
            ViewBag.OnePageOfProducts = onePageOfProducts;
            ViewBag.wallet = id;

            ViewBag.RefTypes = new SelectList(
                   new List<SelectListItem>
                   {
                        new SelectListItem { Text = "Bank Cheque", Value = "CHEQUE"},
                        new SelectListItem { Text = "Bank Slip", Value = "SLIP"}
                   },
               "Value", "Text");

            return View(onePageOfProducts);
        }
    }
}