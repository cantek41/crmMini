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
    public class ProjectController : Controller
    {
        private VdbSoftEntities db = new VdbSoftEntities();

        // GET: /Project/
        public ActionResult Index()
        {
            return View();
        }

        private void ViewDataDoldur()
        {

            ViewData["company"] = (from r in db.COMPANies
                                   where r.COMPANY_CODE > 1
                                   orderby r.COMPANY_NAME
                                   select new { r.COMPANY_CODE, r.COMPANY_NAME });
            ViewData["user"] = from r in db.USERS
                               select new { USER_CODE = r.USER_CODE, USER_NAME = r.AUSER_NAME + " " + r.SURNAME };
         
        }
       
        // GET: /Project/Create
        public ActionResult Create()
        {
            ViewDataDoldur();
            return View();
        }

        // POST: /Project/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PROJECT proje)
        {
            string user = "1";
            DateTime dt = DateTime.Now;
            int code = (db.PROJECTS.Count() == 0) ? 0 : db.PROJECTS.Max(u => u.PROJECT_CODE);
            code++;
            proje.OPPORTUNITY_CODE = -1;
            proje.PROJECT_CODE = code;
            proje.DOCUMENT_DATE = dt;
            proje.CREATE_DATE = dt;
            proje.CLOSED_DATE = DateTime.Parse("1900-01-01 00:00:00.000");
            proje.LAST_UPDATE = dt;
            proje.LAST_UPDATE_USER = user;
            proje.CREATE_USER = user;
           

            try
            {
                db.PROJECTS.Add(proje);
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
                return View(proje);
                
            }
           
            return View("Index");
        }

        // GET: /Project/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROJECT project = db.PROJECTS.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Project/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="PROJECT_CODE,NAME,COMPANY_CODE,CONTACT_CODE,OPPORTUNITY_CODE,CONTRACT_CODE,DOCUMENT_DATE,SDATE,EDATE,APPOINTED_USER_CODE,CLOSED_DATE,CREATE_DATE,CREATE_USER,LAST_UPDATE,LAST_UPDATE_USER,ROW_ORDER_NO,STOK_NAME,RS_DATE,CLOSED_USER")] PROJECT project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(project);
        }

        // GET: /Project/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PROJECT project = db.PROJECTS.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: /Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PROJECT project = db.PROJECTS.Find(id);
            db.PROJECTS.Remove(project);
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
