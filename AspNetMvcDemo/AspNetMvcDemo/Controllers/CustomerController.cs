using AspNetMvcDemo.Models;
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
    }
}