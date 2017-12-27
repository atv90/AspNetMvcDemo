using AspNetMvcDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNetMvcDemo.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            ViewBag.OmaTieto = "ABC123";

            NorthwindEntities entities = new NorthwindEntities();
            //muodostetaan asiakastaulun tiedoista model-niminen lista ja pyydetään entities-oliosta Customers-entities-kokoelmasta
            //kokonaisuutena listaksi
            List<Customers> model = entities.Customers.ToList();
            entities.Dispose();

            //palautetaan näkymälle model-olio
            return View(model);
        }

        public ActionResult Index2()
        {
            
            //palautetaan näkymälle model-olio
            return View();
        }

        public JsonResult GetList()
        {
            NorthwindEntities entities = new NorthwindEntities();
            // List<Customers> model = entities.Customers.ToList();

            //muodostetaan näkymäluokka, joka halutaan välittää Ajaxin kautta
            var model = (from c in entities.Customers
                         select new
                         {
                             CustomerID = c.CustomerID,
                             CompanyName = c.CompanyName,
                             Address = c.Address,
                             City = c.City
                         }).ToList();
            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json,JsonRequestBehavior.AllowGet);
        }
    }
}