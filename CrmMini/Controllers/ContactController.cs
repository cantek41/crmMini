using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CrmMini.Models;

namespace CrmMini.Controllers
{
    public class ContactController : Controller
    {
        private VdbSoftEntities db = new VdbSoftEntities();


        // GET: /Contact/Create
        public ActionResult Create()
        {
            ViewDataDoldur();
            return View();
        }

        private void ViewDataDoldur()
        {
            ViewData["ulke"] = from r in db.COUNTRies
                               select new { r.COUNTRY_CODE, r.COUNTRY_NAME };
            ViewData["il"] = from sehir in db.CITies
                             where sehir.COUNTRY_NAME == "TÜRKİYE"
                             select new
                             {
                                 code = sehir.CITY_NAME,
                                 name = sehir.CITY_NAME
                             };

            ViewData["company"] = (from r in db.COMPANies
                                   where r.COMPANY_CODE > 1
                                   orderby r.COMPANY_NAME
                                   select new { r.COMPANY_CODE, r.COMPANY_NAME });

            ViewData["departman"] = from r in db.GROUPS
                                    where r.GROUP_CODE == 11
                                    select new { code = r.ROW_ORDER_NO, name = r.EXP_TR };

        }

        // POST: /Contact/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CONTACT contact,string phone)
        {
            ViewDataDoldur();
            string user = "-1";
            int code = (db.CONTACTs.Count() == 0) ? 0 : db.CONTACTs.Max(u => u.CONTACT_CODE);
            code++;
            contact.CONTACT_CODE = code;
            contact.CONTACT_REPRESENT_CODE = 1;
            contact.LAST_UPDATE_USER = user;
            contact.LAST_UPDATE = DateTime.Now;
            contact.CREATE_USER = user;
            contact.CREATE_DATE = DateTime.Now;
            //tel
            if (!String.IsNullOrEmpty(phone))
            {
                int telefonCode = (db.PHONEs.Count() == 0) ? 0 : db.PHONEs.Max(u => u.PHONE_CODE);
                telefonCode++;
                db.PHONEs.Add(telkaydet(contact.COMPANY_CODE, code, telefonCode, phone, 1, user));

            }

            ///tel
            ///
            db.CONTACTs.Add(contact);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        private PHONE telkaydet(int company,int code, int sira, string tel, int tur, string user)
        {
            PHONE telefon = new PHONE();
            telefon.PHONE_CODE = sira;
            telefon.COUNTRY_CODE = tel.Substring(1, 2);
            telefon.AREA_CODE = tel.Substring(4, 3);
            telefon.PHONE_NUMBER = tel.Substring(9, 8);
            telefon.COMPANY_CODE = company;
            telefon.CONTACT_CODE = code;
            telefon.PHONE_TYPE_ID = tur;
            telefon.CREATE_USER = user;
            telefon.CREATE_DATE = DateTime.Now;
            telefon.LASTUP_DATE = DateTime.Now;
            telefon.LAST_UPDATE_USER = user;
            return telefon;
        }

        // GET: /Contact/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CONTACT contact = db.CONTACTs.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: /Contact/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CONTACT_CODE,COMPANY_CODE,CONTACT_REPRESENT_CODE,NAME,MIDDLE_NAME,SURNAME,BIRTHDATE,NICNAME,SPACIAL_CODE,DEPARTMENT,TITLE,MAIL,MSN,PERSONALWEBADDRESS,CONTACT_REFERANCE,STATUS,ISSPACIAL,LAST_STATE,EDUCATION,LAST_FNISHED_SCHOOL,COLOR,INVOICE_ADDRESS_CODE,SPEC1,SPEC2,SPEC3,FTEEM,ISMARRIED,ANNIVERSARY_DATE,PARTNER_NAME,PARTNER_MIDDLE_NAME,PARTNER_SURNAME,CHILD_COUNT,MALE_CHILD_COUNT,FEMALE_CHILD_COUNT,HAVE_HOME,HOME_RATING,LAST_MEETING_DATE,LAST_OPPORTUNITY_DATE,LAST_OFFER_DATE,LAST_WAYBILL_DATE,LAST_INVOICE_DATE,LAST_UPDATE_USER,LAST_UPDATE,CREATE_DATE,CREATE_USER,BPICTURE,SEXUALITY,PRETITLE,ADDRESS")] CONTACT contact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
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
