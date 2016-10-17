using BpmOnline.EntityDataServiceReference;
using BpmOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace BpmOnline.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(int? page)
        {
            var contacts = Proxy.GetOdataCollectioByLinq();
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(contacts.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}