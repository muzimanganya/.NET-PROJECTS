using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MobileWS.Models;
using MobileWS.Helpers;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PagedList;

namespace MobileWS.Controllers
{
    [AuthFilter]
    public class WalletsController : Controller
    {
        private Entities db = new Entities();

        // GET: Wallets
        public ActionResult Index()
        {
            return View(db.Wallets.ToList());
        }

        // GET: Wallets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = db.Wallets.Find(id);
            if (wallet == null)
            {
                return HttpNotFound();
            }
            return View(wallet);
        }

        // GET: Wallets/Create
        public ActionResult Create()
        {
            List<SelectListItem> enables = new List<SelectListItem>();
            enables.Add(new SelectListItem { Text = "Activate", Value = "1" });
            enables.Add(new SelectListItem { Text = "Deactivate", Value = "0"});
            ViewBag.IsActive = enables;

            ViewBag.Companies = new SelectList(db.Companies, "Name", "Name");
            return View();
        }

        // POST: Wallets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IsActive,Amount,Company")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
                wallet.CreatedOn = DateTime.Now;
                wallet.UpdatedOn = DateTime.Now;
                wallet.CreatedBy = SecurityHelper.GetLoginId();
                wallet.UpdatedBy = SecurityHelper.GetLoginId();
                wallet.LastReference = "NONE";
                wallet.LastReferenceType = "NONE";

                db.Wallets.Add(wallet);
                if(db.SaveChanges()>0)
                {
                    //write logs
                    WalletLog log = new WalletLog();
                    log.Wallet = wallet.ID;
                    log.CreatedBy = SecurityHelper.GetLoginId();
                    log.CreatedOn = DateTime.Now;
                    log.Comment = JsonConvert.SerializeObject(Json(wallet));
                    log.Amount = wallet.Amount;
                    log.OwnerID = wallet.Company;
                    log.Action = "Creating Wallet";
                    log.ReferenceNumber = wallet.LastReference;
                    log.ReferenceType = wallet.LastReferenceType;
                    db.WalletLogs.Add(log);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            } 

            return View(wallet);
        }

        // GET: Wallets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = db.Wallets.Find(id);
            if (wallet == null)
            {
                return HttpNotFound();
            }
            List<SelectListItem> enables = new List<SelectListItem>();
            enables.Add(new SelectListItem { Text = "Activated", Value = "1", Selected = wallet.IsActive == 1 });
            enables.Add(new SelectListItem { Text = "Deactivated", Value = "0", Selected = wallet.IsActive == 0 });
            ViewBag.enables = enables;

            ViewBag.users = new SelectList(db.Companies, "Name", "Name", wallet.Company);

            return View(wallet);
        }

        // POST: Wallets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult Edit([Bind(Include = "ID,IsActive,Amount,UserName,Company")] Wallet wallet)
        {
            Wallet dbWallet = db.Wallets.FirstOrDefault(u => u.ID == wallet.ID);
            if (ModelState.IsValid)
            {
                if (dbWallet == null)
                {
                    ModelState.AddModelError("ID", "Invalid wallet");
                }
                else
                {
                    if (!String.IsNullOrEmpty(wallet.Company))
                    {
                        dbWallet.Company = wallet.Company;
                    }
                    if (!String.IsNullOrEmpty(wallet.IsActive.ToString()))
                    {
                        dbWallet.IsActive = wallet.IsActive;
                    }
                    if (!String.IsNullOrEmpty(wallet.Amount.ToString()))
                    {
                        dbWallet.Amount = wallet.Amount;
                    }
                    dbWallet.UpdatedOn = DateTime.Now;
                    dbWallet.UpdatedBy = SecurityHelper.GetLoginId();

                    db.Entry(dbWallet).State = EntityState.Modified;
                    if(db.SaveChanges()>0)
                    {
                        //write logs
                        WalletLog log = new WalletLog();
                        log.Wallet = wallet.ID;
                        log.CreatedBy = SecurityHelper.GetLoginId();
                        log.CreatedOn = DateTime.Now;
                        log.Comment = JsonConvert.SerializeObject(Json(wallet));
                        log.Amount = wallet.Amount;
                        log.OwnerID = wallet.Company;
                        log.ReferenceNumber = wallet.LastReference;
                        log.ReferenceType = wallet.LastReferenceType;

                        log.Action = "Updating Wallet";
                        db.WalletLogs.Add(log);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(wallet);
        }

        // GET: Wallets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Wallet wallet = db.Wallets.Find(id);
            if (wallet == null)
            {
                return HttpNotFound();
            }
            return View(wallet);
        }

        // POST: Wallets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Wallet wallet = db.Wallets.Find(id);
            db.Wallets.Remove(wallet);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //  WalletLogs
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Logs(int id, int? page)
        {
            IEnumerable<WalletLog> walletLogs = null;
            if (HttpContext.Request.HttpMethod=="POST" && !String.IsNullOrEmpty(this.HttpContext.Request.Form["RefTypes"]) && this.HttpContext.Request.Form["RefTypes"]!="")
            {
                String type = this.HttpContext.Request.Form["RefTypes"]; 
                walletLogs = db.WalletLogs.Include(w => w.User).Include(w => w.Wallet1).Where(w => w.Wallet == id && w.ReferenceType==type).OrderBy(w => w.ID);
            }
            else
                walletLogs = db.WalletLogs.Include(w => w.User).Include(w => w.Wallet1).Where(w=>w.Wallet==id).OrderBy(w=>w.ID);

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


        // GET: Wallets/Topup/5
        public ActionResult Topup()
        {
            ViewBag.wallets = new SelectList(db.Wallets, "ID", "Company");

            ViewBag.refTypes = new SelectList(
                    new List<SelectListItem>
                    {
                        new SelectListItem { Text = "Bank Cheque", Value = "CHEQUE"},
                        new SelectListItem { Text = "Bank Slip", Value = "SLIP"}
                    }, 
                "Value", "Text");

            return View();
        }

        // POST: Wallets/Topup/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Topup([Bind(Include = "ID,Amount,LastReference,LastReferenceType")] Wallet wallet)
        {
            Wallet dbWallet = db.Wallets.FirstOrDefault(u => u.ID == wallet.ID);
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
                    dbWallet.Amount = dbWallet.Amount+wallet.Amount;
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
                        log.OwnerID = dbWallet.Company;
                        log.Action = "Topping up Wallet";
                        db.WalletLogs.Add(log);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
            }
            return View(wallet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}
