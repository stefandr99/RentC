using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using RentC_MVC.BLL;
using RentC_MVC.Models;
using RentC_MVC.Util;

namespace RentC_MVC.Controllers
{
    public class CarController : Controller
    {
        private readonly Logic logic;

        public CarController()
        {
            logic = new Logic();
        }

        [HttpGet]
        public ActionResult List(int orderBy, string ascendent)
        {
            List<Car> cars = logic.car.listAvailable(orderBy, ascendent);
            return View(cars);
        }

        [HttpGet]
        public ActionResult Search(string criteria, string search)
        {
            List<Car> cars = logic.car.search(criteria, search);
            return View("List", cars);
        }

        public ActionResult Create()
        {
            Car car = new Car();
            return View(car);
        }

        [HttpPost]
        public ActionResult Create(Car car)
        {
            Response res = logic.car.register(car.plate, car.manufacturer, car.model, car.pricePerDay, car.city);


            if (!ModelState.IsValid)
            {
                return View(car);
            }
            else if (res == Util.Response.INVALID_PLATE) {
                ModelState.AddModelError("plate", "Invalid plate.");
                return View(car);
            }
            else if (res == Util.Response.ALREADY_CAR)
            {
                ModelState.AddModelError("plate", "A car with this plate already exists in this city.");
                return View(car);
            }
            else if (res == Util.Response.INVALID_CITY)
            {
                ModelState.AddModelError("city", "Invalid city.");
                return View(car);
            }
            else
            {
                return RedirectToAction("List", new { orderBy = 1, ascendent = "ASC" });
            }
        }


        public ActionResult Edit(int id)
        {
            Car car = logic.car.findById(id);
            if (car == null)
                return HttpNotFound();
            else
                return View(car);
        }

        [HttpPost]
        public ActionResult Edit(Car car, int id)
        {
            Car carToEdit = logic.car.findById(id);
            Response res = logic.car.update(id, car.plate, car.manufacturer, car.model, car.pricePerDay, car.city);

            if (!ModelState.IsValid)
            {
                return View(carToEdit);
            }
            else if (res == Util.Response.INVALID_PLATE)
            {
                ModelState.AddModelError("plate", "Invalid plate.");
                return View(carToEdit);
            }
            else if (res == Util.Response.ALREADY_CAR)
            {
                ModelState.AddModelError("plate", "A car with this plate already exists in this city.");
                return View(carToEdit);
            }
            else if (res == Util.Response.INVALID_CITY)
            {
                ModelState.AddModelError("city", "Invalid city.");
                return View(carToEdit);
            }

            return RedirectToAction("List", new { orderBy = 1, ascendent = "ASC" });
        }

        public ActionResult Delete(int id)
        {
            Car car = logic.car.findById(id);
            if (car == null)
                return HttpNotFound();
            else
                return View(car);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult confirmDelete(int id)
        {
            Car carToDelete = logic.car.findById(id);

            if (carToDelete == null)
                return HttpNotFound();
            else
            {
                logic.car.remove(id);
                return RedirectToAction("List", new { orderBy = 1, ascendent = "ASC" });
            }
        }

        public ActionResult ListMostRecentCars()
        {
            List<Car> cars = logic.car.listMostRecentCars();
            return View(cars);
        }

        
        public ActionResult ListMostRentedByMonth()
        {
            return View();
        }

        public ActionResult ListMostRentedByMonth2(int month, int year)
        {

            List<Tuple<int, Car>> cars = new List<Tuple<int, Car>>(logic.car.listMostRentedByMonth(month, year));
            return View(cars);
        }

        
        public ActionResult ListLessRentedByMonth()
        {
            return View();
        }

        public ActionResult ListLessRentedByMonth2(int month, int year)
        {
            
            List<Tuple<int, Car>> cars = new List<Tuple<int, Car>>(logic.car.listLessRentedByMonth(month, year));
            return View(cars);
        }
    }
}