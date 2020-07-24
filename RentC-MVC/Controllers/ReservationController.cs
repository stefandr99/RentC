using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentC_MVC.BLL;
using RentC_MVC.Models;

namespace RentC_MVC.Controllers
{
    public class ReservationController : Controller
    {
        private Logic logic;

        public ReservationController()
        {
            logic = new Logic();
        }

        public ActionResult List()
        {
            List<Reservation> reservations = logic.reservation.list(1, "ASC");
            return View(reservations);
        }

        public ActionResult Create()
        {
            Reservation reservation = new Reservation();
            return View(reservation);
        }

        [HttpPost]
        public ActionResult Create(Reservation reservation)
        {
            if (!ModelState.IsValid)
            {
                return View(reservation);
            }
            else
            {
                logic.reservation.register(reservation.carId, reservation.customerId, reservation.startDate, reservation.endDate, reservation.location);
                return RedirectToAction("List");
            }
        }


        public ActionResult Edit(int id)
        {
            Reservation reservation = logic.reservation.findById(id);
            if (reservation == null)
                return HttpNotFound();
            else
                return View(reservation);
        }

        [HttpPost]
        public ActionResult Edit(Reservation reservation, int id)
        {
            Reservation reservationToEdit = logic.reservation.findById(id);

            if (reservationToEdit == null)
                return HttpNotFound();
            else
            {
                if (!ModelState.IsValid || logic.reservation.update(id, reservation.startDate, reservation.endDate) != Util.Response.SUCCESS)
                    return View(reservationToEdit);
            }

            return RedirectToAction("List");
        }

        public ActionResult Delete(int id)
        {
            Reservation reservation = logic.reservation.findById(id);
            if (reservation == null)
                return HttpNotFound();
            else
                return View(reservation);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult confirmDelete(int id)
        {
            Reservation reservationToDelete = logic.reservation.findById(id);

            if (reservationToDelete == null)
                return HttpNotFound();
            else
            {
                logic.reservation.cancelReservation(id);
                return RedirectToAction("List");
            }
        }
    }
}