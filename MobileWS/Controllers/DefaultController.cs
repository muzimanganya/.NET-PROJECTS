using MobileWS.Helpers;
using MobileWS.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MobileWS.Controllers
{
    public class DefaultController : Controller
    {        
        private Entities db = new Entities();

        // GET: Default
        public ActionResult Index()
        {
            int id = SecurityHelper.GetLoginId();
            User user = db.Users.Where(u => u.ID == id).FirstOrDefault();
            if (user == null)
                return RedirectToAction("Forbidden", "Home");

            return View(db.Wallets.Where(w => w.Company == user.UserCompany).ToList());
        }

        [HttpGet]
        public ActionResult Topup()
        {
            int id = SecurityHelper.GetLoginId();
            User user = db.Users.Where(u => u.ID == id).FirstOrDefault();
            if (user == null)
                return RedirectToAction("Forbidden", "Home");

            ViewBag.refTypes = new SelectList(
                    new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Bank Cheque", Value = "CHEQUE"},
                        new SelectListItem { Text = "Bank Slip", Value = "SLIP"}
                    },
                "Value", "Text");

            ViewBag.wallets = new SelectList(db.Wallets.Where(w => w.Company == user.UserCompany), "ID", "Company");
            return View();
        }

        // POST: Wallets/Topup/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Topup([Bind(Include = "ID,Amount,LastReference,LastReferenceType,Company")] Wallet wallet)
        {
            int id = SecurityHelper.GetLoginId();
            User user = db.Users.Where(u => u.ID == id).FirstOrDefault();

            Wallet dbWallet = db.Wallets.FirstOrDefault(u => u.ID == wallet.ID);

            if (user==null || String.IsNullOrEmpty(dbWallet.Company))
                return RedirectToAction("Forbidden", "Home");

            if (dbWallet.Company.ToLower()!=user.UserCompany.ToLower())
                return RedirectToAction("Forbidden", "Home");

            if (ModelState.IsValid)
            {
                if (dbWallet == null)
                {
                    ModelState.AddModelError("ID", "Invalid wallet");
                }
                else
                {
                    dbWallet.LastReference = wallet.LastReference;
                    dbWallet.LastReferenceType = wallet.LastReferenceType;
                    dbWallet.LastBalance = dbWallet.Amount;

                    dbWallet.UpdatedOn = DateTime.Now;
                    dbWallet.Amount = dbWallet.Amount + wallet.Amount;
                    dbWallet.UpdatedBy = SecurityHelper.GetLoginId();

                    db.Entry(dbWallet).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        //write logs
                        WalletLog log = new WalletLog();
                        log.Wallet = dbWallet.ID;
                        log.CreatedBy = SecurityHelper.GetLoginId();
                        log.CreatedOn = DateTime.Now;
                        log.Comment = JsonConvert.SerializeObject(Json(wallet));
                        log.Amount = dbWallet.Amount;
                        log.OwnerID = dbWallet.Company;
                        log.ReferenceType = dbWallet.LastReferenceType;
                        log.ReferenceNumber = dbWallet.LastReference;
                        log.Action = "Topping up Wallet";
                        db.WalletLogs.Add(log);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(wallet);
        }

    }
}