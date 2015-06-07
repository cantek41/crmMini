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
    public class ActivityController : Controller
    {
        private VdbSoftEntities db = new VdbSoftEntities();

        // GET: /Activity/
        public ActionResult Index()
        {
            
            return View();
        }
        [HttpPost]
        public JsonResult projeGetir(int company)
        {
            DateTime dt = DateTime.Parse("1900-01-01 00:00:00.000");
            var str = (from r in db.PROJECTS
                       where r.COMPANY_CODE == company && r.CLOSED_DATE == dt// && r.ROW_ORDER_NO==1
                       select new { id = r.PROJECT_CODE, state = r.NAME }).ToArray();
            return Json(str);
        }
        [HttpPost]
        public ActionResult filterContact(int CompanyCode)
        {
            //your linq to filter the states by country for example
            var v = (from li in db.CONTACTs
                     where li.COMPANY_CODE == CompanyCode
                     orderby li.NAME
                     select new
                     {
                         id = li.CONTACT_CODE,
                         state = li.NAME + " " + li.SURNAME
                     }).AsQueryable();

            return Json(v);
        }
        private void ViewDataDoldur()
        {
            
            ViewData["company"] = (from r in db.COMPANies
                                   where r.COMPANY_CODE > 1
                                   orderby r.COMPANY_NAME
                                   select new { r.COMPANY_CODE, r.COMPANY_NAME });
            ViewData["user"] = from r in db.USERS
                               select new { USER_CODE = r.USER_CODE, USER_NAME = r.AUSER_NAME + " " + r.SURNAME };
            ViewData["aktiviteType"] = from r in db.GROUPS
                                       where r.GROUP_CODE == 25
                                       select new { code = r.ROW_ORDER_NO, name = r.EXP_TR };
            ViewData["aktiviteStation"] = from r in db.GROUPS
                                          where r.GROUP_CODE == 25
                                          select new { code = r.ROW_ORDER_NO, name = r.EXP_TR };
            ViewData["onem"] = from r in db.GROUPS
                               where r.GROUP_CODE == 28
                               select new { code = r.ROW_ORDER_NO, name = r.EXP_TR };
         
            ViewData["nerde"] = from r in db.GROUPS
                                where r.GROUP_CODE == 27
                                select new { code = r.ROW_ORDER_NO, name = r.EXP_TR };
            ViewData["open"] = from r in db.GROUPS
                                where r.GROUP_CODE == 50
                                select new { code = r.ROW_ORDER_NO, name = r.EXP_TR };
        }

        public ActionResult timePlane()
        {
            return View();
        }

        // GET: /Activity/Create
        public ActionResult Create()
        {
            ViewDataDoldur();
            return View();
        }

        // POST: /Activity/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ACTIVITY aktivite)
        {
            ViewDataDoldur();
            string user = "0";
            try
            {
                int code = (db.ACTIVITies.Count() == 0) ? 0 : db.ACTIVITies.Max(u => u.ACTIVITY_CODE);
                code++;
                //ACTIVITYSUBJECT konuGrup = new ACTIVITYSUBJECT();
                //konuGrup.ACTIVITY_CODE = code;
                //konuGrup.SUBJECT_CODE = aktivite.ACTIVITY_CODE;
                //konuGrup.TYPE = 0;
                aktivite.OPENORCLOSE = 1;             
                aktivite.ACTIVITY_CODE = code;
                aktivite.CREATE_USER = user;
                aktivite.CREATE_DATE = DateTime.Now;
                aktivite.LAST_UPDATE_USER = user;
                aktivite.OWNER = Convert.ToInt32(user);              
                db.ACTIVITies.Add(aktivite);
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
               
                return View(aktivite);

            }          

            return View("Index");
        }

        // GET: /Activity/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ACTIVITY activity = db.ACTIVITies.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: /Activity/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ACTIVITY_CODE,CONTACT_CODE,COMPANY_CODE,OWNER,ACTIVITY_TYPE,SDATE,EDATE,DURATION,TRANSPORTINCLUDE,TRANSFER_TIME,REGARDING,NOTE,SUBJECT,LOCATION,PRIORITY,REMEMBER,COLOR,PROJECT,ACTIVITY_GROUP,APPOINTED_USER_CODE,OPENORCLOSE,COST,COST_UNIT,LAST_UPDATE_USER,LAST_UPDATE,CREATE_DATE,CREATE_USER,R_CODE,SOURCEACTIVITY_CODE,DOCUMENT_NO1,DOCUMENT_NO2,EXT1,EXT2,BASE_TYPE")] ACTIVITY activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // GET: /Activity/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ACTIVITY activity = db.ACTIVITies.Find(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: /Activity/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ACTIVITY activity = db.ACTIVITies.Find(id);
            db.ACTIVITies.Remove(activity);
            db.SaveChanges();
            return RedirectToAction("Index");
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
