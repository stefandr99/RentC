using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentC_MVC.BLL;
using RentC_MVC.Models;

namespace RentC_MVC.Controllers
{
    public class UserController : Controller
    {
        private Logic logic;

        public UserController()
        {
            logic = new Logic();
        }

        public ActionResult List()
        {
            List<User> users = logic.user.list(1, "ASC");
            return View(users);
        }

        public ActionResult Create()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            else
            {
                logic.user.registerSaleperson(user.password);
                return RedirectToAction("List");
            }
        }


        public ActionResult ChangePass(int id)
        {
            User user = logic.user.findById(id);
            if (user == null)
                return HttpNotFound();
            else
                return View(user);
        }

        [HttpPost]
        public ActionResult ChangePass(User user, int id)
        {
            User userToEdit = logic.user.findById(id);

            if (userToEdit == null)
                return HttpNotFound();
            else
            {
                if (!ModelState.IsValid || logic.user.changePassword(id, userToEdit.password, user.password, user.password) != Util.Response.SUCCESS)
                    return View(userToEdit);
            }

            return RedirectToAction("List");
        }

        public ActionResult Disable(int id)
        {
            User user = logic.user.findById(id);
            if (user == null)
                return HttpNotFound();
            logic.user.disableUser(id);
            return RedirectToAction("List");
        }

    }
}