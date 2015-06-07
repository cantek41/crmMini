using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrmMini.Models;

namespace CrmMini.Controllers
{
    public class CompanyController : Controller
    {
        private VdbSoftEntities db = new VdbSoftEntities();

        // GET: /Company/
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Company/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            COMPANY company = db.COMPANies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // GET: /Company/Create
        public ActionResult Create()
        {
            ViewDataDoldur();
            return View();
        }

        private void ViewDataDoldur()
        {
            ViewData["ulke"] = from r in db.COUNTRies
                               select new { r.COUNTRY_CODE, r.COUNTRY_NAME };
        }
        [HttpPost]
        public ActionResult getil(string ulkeName)
        {
            var str = (from ulke in db.COUNTRies
                       join sehir in db.CITies on ulke.COUNTRY_CODE equals sehir.COUNTRY_CODE
                       where ulke.COUNTRY_NAME == ulkeName
                       select new
                       {
                           id = sehir.CITY_CODE,
                           state = sehir.CITY_NAME
                       }).ToArray();

            return Json(str);
        }
        [HttpPost]
        public ActionResult getilce(string cityname)
        {
            var str = (from li in db.CITY2
                       join il in db.CITies on li.CITY_CODE equals il.CITY_CODE
                       where il.CITY_NAME == cityname && li.UPPER_NO == 0
                       select new
                       {
                           id = li.ORDER_NO,
                           state = li.NAME
                       }).ToArray();

            return Json(str);
        }
        // POST: /Company/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(COMPANY company, string ulke, string iller, string ilce, string adres, string phone)
        {
            int code = (db.COMPANies.Count() == 0) ? 0 : db.COMPANies.Max(u => u.COMPANY_CODE);
            string user = "0";
            code++;
            company.COMPANY_CODE = code;
            company.UPPER_COMPANY_CODE = "-1";
            company.COMPANY_REPRESENT_CODE = 0;// Convert.ToInt32(Session[2]);
            company.CREATE_USER = user;
            company.CREATE_DATE = DateTime.Now;
            company.LAST_UPDATE_USER = user;
            company.LAST_UPDATE = DateTime.Now;


            //tel
            if (!String.IsNullOrEmpty(phone))
            {
                int telefonCode = (db.PHONEs.Count() == 0) ? 0 : db.PHONEs.Max(u => u.PHONE_CODE);
                telefonCode++;
                company.PHONE = telefonCode;
                db.PHONEs.Add(telkaydet(code, telefonCode, phone, 1, user));
                
            }
           
            ///tel
            ///
            //adres
           
            if (!String.IsNullOrEmpty(adres))
            {
                ADDRESS adresTable = new ADDRESS();
                int adresCode = (db.ADDRESSes.Count() == 0) ? 0 : db.ADDRESSes.Max(u => u.ADDRESS_CODE);
                adresCode++;
                adresTable.ADDRESS_CODE = adresCode;
                adresTable.COUNTY = ulke;
                adresTable.CITY = iller;
                adresTable.COUNTY1 = ilce;
                if (adres.Length < 50)
                {
                    adresTable.ADDRESS1 = adres;
                }

                else if (adres.Length < 100)
                {
                    adresTable.ADDRESS1 = adres.Substring(1, 50);
                    adresTable.ADDRESS2 = adres.Substring(51);
                }
                else
                {
                    adresTable.ADDRESS1 = adres.Substring(1, 50);
                    adresTable.ADDRESS2 = adres.Substring(51, 50);
                    adresTable.ADDRESS3 = adres.Substring(100);
                }
                adresTable.LASTUP_DATE = DateTime.Now;
                adresTable.OWNER = Session[2].ToString();
                adresTable.REGION_CODE = 0;
                adresTable.ADDRESS_TYPE_ID = 1;
                adresTable.DISTANCE = -1;
                adresTable.LAST_UPDATE_USER = user;
                adresTable.COMPANY_CODE = code;
                adresTable.CREATE_USER = user;
                adresTable.CREATE_DATE = DateTime.Now;
                company.ADDRESS = adresCode.ToString();
                db.ADDRESSes.Add(adresTable);
            }
            else company.ADDRESS = "-1";
            ///adres
            db.COMPANies.Add(company);
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errs in ex.EntityValidationErrors)
                {
                    foreach (var err in errs.ValidationErrors)
                    {
                        var propName = err.PropertyName;
                        var errMess = err.ErrorMessage;
                    }
                }
                ViewDataDoldur();
                return View(company);

            }

            return RedirectToAction("Index");
        }


        private PHONE telkaydet(int code, int sira, string tel, int tur,string user)
        {
            PHONE telefon = new PHONE();
            telefon.PHONE_CODE = sira;
            telefon.COUNTRY_CODE = tel.Substring(1, 2);
            telefon.AREA_CODE = tel.Substring(4, 3);
            telefon.PHONE_NUMBER = tel.Substring(9, 8);
            telefon.COMPANY_CODE = code;
            telefon.PHONE_TYPE_ID = tur;
            telefon.CREATE_USER = user;
            telefon.CREATE_DATE = DateTime.Now;
            telefon.LASTUP_DATE = DateTime.Now;
            telefon.LAST_UPDATE_USER = user;
            return telefon;
        }
        // GET: /Company/Edit/5
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewDataDoldur();
            COMPANY company = db.COMPANies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        // POST: /Company/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(COMPANY company, string ulke, string iller, string ilce, string adres, string phone)
        {
            //fix mee edit tam değil           
            string user = "0";
            COMPANY cmp = db.COMPANies.Find(company.COMPANY_CODE);
            cmp.COMPANY_NAME = company.COMPANY_NAME;
            cmp.LAST_UPDATE_USER = user;
            cmp.LAST_UPDATE = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewDataDoldur();
            return View(company);
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
