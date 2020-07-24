using System;
using System.Collections.Generic;
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
        private Logic logic;

        public CustomerController() {
            logic = new Logic();
        }
        // GET: Customer
        public ActionResult List() {
            List<Customer> customers = logic.customer.list(1, "ASC");
            return View(customers);
        }

        public ActionResult Create() {
            Customer customer = new Customer();
            return View(customer);
        }

        [HttpPost]
        public ActionResult Create(Customer customer) {
            if (!ModelState.IsValid) {
                return View(customer);
            }
            else {
                logic.customer.register(customer.name, customer.birthDate, customer.ZIPCode);
                return RedirectToAction("List");
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

            if (customerToEdit == null)
                return HttpNotFound();
            else {
                if (!ModelState.IsValid || logic.customer.update(id, customer.name, customer.birthDate, customer.ZIPCode) != Util.Response.SUCCESS)
                    return View(customerToEdit);
            }

            return RedirectToAction("List");
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
                return RedirectToAction("List");
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