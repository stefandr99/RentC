using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentC_MVC.BLL;
using RentC_MVC.Models;
using RentC_MVC.Util;

namespace RentC_MVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly Logic logic;

        public CustomerController() {
            logic = new Logic();
        }

        [HttpGet]
        public ActionResult List(int orderBy, string ascendent) {
            
            List<Customer> customers = logic.customer.list(orderBy, ascendent);
            return View(customers);
        }

        [HttpGet]
        public ActionResult Search(string criteria, string search)
        {
            List<Customer> customers = logic.customer.search(criteria, search);
            return View("List", customers);
        }

        public ActionResult Create() {
            Customer customer = new Customer();
            return View(customer);
        }

        [HttpPost]
        public ActionResult Create(Customer customer) {
            Response res = logic.customer.register(customer.name, customer.birthDate, customer.ZIPCode);

            if (!ModelState.IsValid) {
                return View(customer);
            }
            else if (res == Util.Response.IREAL_BIRTH)
            {
                ModelState.AddModelError("birthDate", "You cannot be born on this day.");
                return View(customer);
            }
            else if (res == Util.Response.INVALID_ZIP) {
                ModelState.AddModelError("ZIPCode", "ZIP Code is invalid");
                return View(customer);
            }
            else {
                return RedirectToAction("List", new { orderBy = 1, ascendent = "ASC" });
            }
        }

        
        public ActionResult Edit(int id) {
            Customer customer = logic.customer.findById(id);
            if (customer == null)
                return HttpNotFound();
            else 
                return View(customer);
        }

        [HttpPost]
        public ActionResult Edit(Customer customer, int id) {
            Customer customerToEdit = logic.customer.findById(id);

            Response res = logic.customer.update(id, customer.name, customer.birthDate, customer.ZIPCode);
            if (!ModelState.IsValid)
            {
                return View(customerToEdit);
            }
            else if (res == Util.Response.IREAL_BIRTH) {
                ModelState.AddModelError("birthDate", "You cannot be born on this day.");
                return View(customerToEdit);
            }
            else if (res == Util.Response.INVALID_ZIP)
            {
                ModelState.AddModelError("ZIPCode", "ZIP Code is invalid");
                return View(customerToEdit);
            }
            else
            {
                return RedirectToAction("List", new { orderBy = 1, ascendent = "ASC" });
            }
        }

        public ActionResult Delete(int id)
        {
            Customer customer = logic.customer.findById(id);
            if (customer == null)
                return HttpNotFound();
            else
                return View(customer);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult confirmDelete(int id)
        {
            Customer customerToDelete = logic.customer.findById(id);

            if (customerToDelete == null)
                return HttpNotFound();
            else
            {
                logic.customer.removeCustomer(id);
                return RedirectToAction("List", new { orderBy = 1, ascendent = "ASC" });
            }
        }

        public ActionResult GoldCustomers()
        {
            List<Tuple<int, Customer>> customers = logic.customer.goldCustomers();
            return View(customers);
        }

        public ActionResult SilverCustomers()
        {
            List<Tuple<int, Customer>> customers = logic.customer.silverCustomers();
            return View(customers);
        }
    }
}