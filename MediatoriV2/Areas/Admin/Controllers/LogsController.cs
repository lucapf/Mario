using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;

namespace MyWebApplication.Areas.Admin.Controllers
{
    [MyAuthorize(Roles = "ADMIN")]
    public class LogsController : MyBaseController
    {
        private MyManagerCSharp.Log.LogManager manager = new MyManagerCSharp.Log.LogManager("DefaultConnection");

        public ActionResult Index(MyManagerCSharp.Log.Models.SearchMyLogs model)
        {
            Debug.WriteLine("levelSelected: " + Request.Form["levelSelected"]);

            if (model.Sort == "DateAdded")
            {
                model.Sort = "date_added";
            }

            //if (Request.Form["levelSelected"] != null)
            //{
            //    model.levelSelected = new List<MyManagerCSharp.Log.LogManager.Level>();

            //    foreach (string s in Request.Form["levelSelected"].Split(','))
            //    {
            //        model.levelSelected.Add((MyManagerCSharp.Log.LogManager.Level)System.Enum.Parse(typeof(MyManagerCSharp.Log.LogManager.Level), s));
            //    }
            //}


            //if (Request.Form["myTypeSelected"] != null)
            //{
            //    foreach (string s in Request.Form["myTypeSelected"].Split(','))
            //    {
            //        model.myTypeSelected.Add(s);
            //    }
            //}


            manager.openConnection();

            try
            {
                model.myType = manager.getMyType();

                 manager.getList(model);
             
            }
            finally
            {
                manager.closeConnection();
            }

            return View(model);
        }

        public ActionResult Details(string id, string[] LevelSelected)
        {
            Debug.WriteLine("Reference ID: " + id );

            MyManagerCSharp.Log.Models.MyLogDetail model = new MyManagerCSharp.Log.Models.MyLogDetail();

            model.referenceId = id;

            manager.openConnection();
            try
            {
                if (LevelSelected != null)
                {
                    foreach (string s in LevelSelected)
                    {

                        model.LevelSelected.Add((MyManagerCSharp.Log.LogManager.Level)Enum.Parse(typeof(MyManagerCSharp.Log.LogManager.Level), s));
                    }

                    model.Logs = manager.getReferenceDetail(id, model.LevelSelected );
                }
                else
                {
                    model.Logs = manager.getReferenceDetail(id);
                }
                //model.LevelSelected = 

            }
            finally
            {
                manager.closeConnection();
            }
            return View(model);
        }

        public ActionResult Delete()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(MyManagerCSharp.Log.LogManager.Days days)
        {
            Debug.WriteLine("Delete: " + days);

            manager.openConnection();
            try
            {
                int recordsDeleted;

                recordsDeleted = manager.delete(days);


            }
            finally
            {
                manager.closeConnection();
            }

            return RedirectToAction("Index");
        }

        public ActionResult Home(MyWebApplication.Areas.Admin.Models.LogsSummaryModel model)
        {
            manager.openConnection();

            try
            {
                model.summary = manager.getSummary(model.days);

               
            }
            finally
            {
                manager.closeConnection();
            }
            return View(model);
        }
    
    }
}
