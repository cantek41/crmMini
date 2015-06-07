using System;
﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using CrmMini.Models;
using CrmMini.Repository;

namespace CrmMini.Controllers
{
	public partial class GridController : Controller
    {
		public ActionResult Orders_Read([DataSourceRequest]DataSourceRequest request)
		{
			var result = Enumerable.Range(0, 50).Select(i => new OrderViewModel
			{
				OrderID = i,
				Freight = i * 10,
				OrderDate = DateTime.Now.AddDays(i),
				ShipName = "ShipName " + i,
				ShipCity = "ShipCity " + i
			});

			return Json(result.ToDataSourceResult(request));
		}

        public ActionResult Activity_Read([DataSourceRequest]DataSourceRequest request)
        {
            VdbSoftEntities db = new VdbSoftEntities();
            return Json(db.ACTIVITies.ToList().ToDataSourceResult(request));
        }
        public ActionResult Company_Read([DataSourceRequest]DataSourceRequest request)
        {
            VdbSoftEntities db = new VdbSoftEntities();           
            return Json(db.COMPANies.ToList().ToDataSourceResult(request));
        }

        public ActionResult Project_Read([DataSourceRequest]DataSourceRequest request)
        {
            VdbSoftEntities db = new VdbSoftEntities();
            return Json(db.PROJECTS.ToList().ToDataSourceResult(request));
        }

        public ActionResult timeGridPartial()
        {
            timeGridModelView model=new timeGridModelView();
            model.ay="Ocak";
            model.gunler=new List<int>();
            for (int i = 0; i < 32; i++)
			{
			 model.gunler.Add(i);
			}

            return PartialView("_timeGridPartial", model);
        }
    }

    public class timeGridModelView
    {
        public string ay { get; set; }
        public List<int> gunler { get; set; }
    }
}