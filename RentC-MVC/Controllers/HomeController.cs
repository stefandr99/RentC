using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentC_MVC.BLL;

namespace RentC_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly Logic logic;

        public HomeController() {
            logic = new Logic();
        }
        public ActionResult Index() {
            logic.reservation.expiredReservations();
            return View();
        }

    }
}