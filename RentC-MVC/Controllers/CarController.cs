using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentC_MVC.BLL;
using RentC_MVC.Models;

namespace RentC_MVC.Controllers
{
    public class CarController : Controller
    {
        private Logic logic;

        public CarController()
        {
            logic = new Logic();
        }

        public ActionResult List()
        {
            List<Car> cars = logic.car.listAvailable(1, "ASC");
            return View(cars);
        }

        public ActionResult Create()
        {
            Car car = new Car();
            return View(car);
        }

        [HttpPost]
        public ActionResult Create(Car car)
        {
            if (!ModelState.IsValid)
            {
                return View(car);
            }
            else
            {
                logic.car.register(car.plate, car.manufacturer, car.model, car.pricePerDay, car.city);
                return RedirectToAction("List");
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

            if (carToEdit == null)
                return HttpNotFound();
            else
            {
                if (!ModelState.IsValid || logic.car.update(id, car.plate, car.manufacturer, car.model, car.pricePerDay, car.city) != Util.Response.SUCCESS)
                    return View(carToEdit);
            }

            return RedirectToAction("List");
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
                return RedirectToAction("List");
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

        [HttpPost]
        public ActionResult ListMostRentedByMonth2(int month, int year)
        {
            List<Tuple<int, Car>> cars = new List<Tuple<int, Car>>(logic.car.listMostRentedByMonth(month, year));
            return View(cars);
        }

        public ActionResult ListLessRentedByMonth()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ListLessRentedByMonth2(int month, int year)
        {
            List<Tuple<int, Car>> cars = new List<Tuple<int, Car>>(logic.car.listLessRentedByMonth(month, year));
            return View(cars);
        }
    }
}