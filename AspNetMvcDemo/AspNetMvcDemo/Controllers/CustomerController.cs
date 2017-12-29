using AspNetMvcDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AspNetMvcDemo.Controllers
{

    //Index malliolio menetelmä
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

        //Index2 osuus ajax-kutsuun liittyvää
        public ActionResult Index2()
        {            
            return View();
        }

        public ActionResult Index3()
        {
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
        public JsonResult GetSingleCustomer(string id)
        {
            NorthwindEntities entities = new NorthwindEntities();
            // List<Customers> model = entities.Customers.ToList();

            //muodostetaan näkymäluokka, joka halutaan välittää Ajaxin kautta
            var model = (from c in entities.Customers
                         where c.CustomerID == id
                         select new
                         {
                             CustomerID = c.CustomerID,
                             CompanyName = c.CompanyName,
                             Address = c.Address,
                             City = c.City
                         }).FirstOrDefault();
            string json = JsonConvert.SerializeObject(model);
            entities.Dispose();

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Update(Customers cust)
        {
            NorthwindEntities entities = new NorthwindEntities();
            string id = cust.CustomerID;

            bool OK = false;
            //onko kyseessä uusi lisäys vai vanhan muokkaus
            if (id == "(uusi)")
            {
                //uuden lisääminen eli kopioidaan kentät
                Customers dbItem = new Customers()
                {
                    //otetaan CompanyNamesta Substring funktiolla 5 ensimmäistä merkkiä, jos loppuu
                    //välilyöntiin tehdään Trim()
                    CustomerID = cust.CompanyName.Substring(0, 5).Trim().ToUpper(),
                    CompanyName = cust.CompanyName,
                    Address = cust.Address,
                    City = cust.City
                };
                //tallennetaan uusi lisäys tietokantaan
                entities.Customers.Add(dbItem);
                entities.SaveChanges();
                OK = true;

            }
            else
            {

                //muokkaus eli haetaan id:n perusteella rivi tietokannasta
                Customers dbItem = (from c in entities.Customers
                                    where c.CustomerID == id
                                    select c).FirstOrDefault();
                //tallennetaan vain ne tiedot joita käyttäjä on voinut muokata
                if (dbItem != null)
                {
                    dbItem.CompanyName = cust.CompanyName;
                    dbItem.Address = cust.Address;
                    dbItem.City = cust.City;

                    //tallennus tietokantaan
                    entities.SaveChanges();
                    OK = true;
                }
            }
            entities.Dispose();
            return Json(OK);
        }

    }
}