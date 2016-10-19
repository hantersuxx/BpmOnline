//using BpmOnline.EntityDataServiceReference;
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
        IQueryable<EntityDataServiceReference.Contact> contacts = Proxy.GetOdataCollectioByLinq();

        public ActionResult Index(int? page)
        {
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(contacts.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View(new Contact());
        }

        [HttpPost]
        public ActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                OdataHttpQuery.CreateBpmEntityByOdataHttp(contact);
                return RedirectToAction("Index", "Home");
            }
            return View(contact);
        }

        [HttpGet]
        public ActionResult Update(Guid? updateId)
        {
            if (updateId == null)
            {
                return HttpNotFound();
            }

            var bpmContact = (from c in contacts
                              where c.Id == updateId
                              select c).FirstOrDefault();
            if (bpmContact == null)
            {
                return HttpNotFound();
            }

            Contact contact = new Contact();
            contact.Init(bpmContact.Id, bpmContact.Name, bpmContact.MobilePhone, bpmContact.Dear, bpmContact.JobTitle, bpmContact.BirthDate);
            return View(contact);
        }

        [HttpPost]
        public ActionResult Update(Contact contact)
        {
            if (ModelState.IsValid)
            {
                OdataHttpQuery.UpdateExistingBpmEntityByOdataHttp(contact);
                return RedirectToAction("Index", "Home");
            }
            return View(contact);
        }

        public ActionResult Delete(Guid deleteId)
        {
            if (deleteId == null)
            {
                return HttpNotFound();
            }
            OdataHttpQuery.DeleteBpmEntityByOdataHttp(deleteId);
            return RedirectToAction("Index", "Home");
        }
    }
}