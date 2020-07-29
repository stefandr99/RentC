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
    public class ReservationController : Controller
    {
        private readonly Logic logic;

        public ReservationController()
        {
            logic = new Logic();
        }

        [HttpGet]
        public ActionResult List(int orderBy, string ascendent)
        {
            List<Reservation> reservations = logic.reservation.list(orderBy, ascendent);
            return View(reservations);
        }

        [HttpGet]
        public ActionResult Search(string criteria, string search)
        {
            List<Reservation> reservations = logic.reservation.search(criteria, search);
            return View("List", reservations);
        }

        public ActionResult Create()
        {
            Reservation reservation = new Reservation();
            return View(reservation);
        }

        [HttpPost]
        public ActionResult Create(Reservation reservation)
        {
            Util.Response res = logic.reservation.register(reservation.carId, reservation.customerId, reservation.startDate, 
                reservation.endDate, reservation.location);
            if (!ModelState.IsValid)
            {
                return View(reservation);
            }
            else if (res == Util.Response.INEXISTENT_CAR) {
                ModelState.AddModelError("carId", "This car does not exist.");
                return View(reservation);
            }
            else if (res == Util.Response.UNAVAILABLE_CAR)
            {
                ModelState.AddModelError("carId", "This car is not available.");
                return View(reservation);
            }
            else if (res == Util.Response.INEXISTENT_CUSTOMER)
            {
                ModelState.AddModelError("customerId", "This customer does not exist.");
                return View(reservation);
            }
            else if (res == Util.Response.UNAVAILABLE_CAR_IN_CITY)
            {
                ModelState.AddModelError("carId", "This car is not available in this city.");
                return View(reservation);
            }
            else if (res == Util.Response.INCORRECT_SDATE)
            {
                ModelState.AddModelError("startDate", "Reservation cannot start in the past.");
                return View(reservation);
            }
            else
            {
                return RedirectToAction("List");
            }
        }


        public ActionResult Edit(int carId, int customerId, DateTime startDate, DateTime endDate)
        {
            Reservation reservation = logic.reservation.findById(carId, customerId, startDate, endDate);
            if (reservation == null)
                return HttpNotFound();
            else
                return View(reservation);
        }

        [HttpPost]
        public ActionResult Edit(Reservation reservation, int carId, int customerId, DateTime startDate, DateTime endDate)
        {
            Reservation reservationToEdit = logic.reservation.findById(carId, customerId, startDate, endDate);
            Response res = logic.reservation.update(carId, customerId, reservation.startDate, reservation.endDate);

            if (reservationToEdit == null) {
                return HttpNotFound();
            }
            else if (res == Util.Response.INCORRECT_SDATE) {
                ModelState.AddModelError("startDate", "Reservation cannot start in the past.");
                return View(reservationToEdit);
            }

            return RedirectToAction("List");
        }

        public ActionResult Delete(int carId, int customerId, DateTime startDate, DateTime endDate)
        {
            Reservation reservation = logic.reservation.findById(carId, customerId, startDate, endDate);
            if (reservation == null)
                return HttpNotFound();
            else
                return View(reservation);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult confirmDelete(int carId, int customerId, DateTime startDate, DateTime endDate)
        {
            Reservation reservationToDelete = logic.reservation.findById(carId, customerId, startDate, endDate);

            if (reservationToDelete == null)
                return HttpNotFound();
            else
            {
                logic.reservation.cancelReservation(carId, customerId, startDate, endDate);
                return RedirectToAction("List");
            }
        }

        public void ExpiredReservations() {
            logic.reservation.expiredReservations();
        }

    }
}