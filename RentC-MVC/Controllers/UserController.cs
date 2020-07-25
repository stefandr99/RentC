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
                logic.user.registerSaleperson(user.username, user.password);
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

            return RedirectToAction("Index", "Home");
        }

        public void UserActivity(int id, bool decision)
        {
            User user = logic.user.findById(id);
            if (decision)
                logic.user.enableUser(id);
            else logic.user.disableUser(id);
        }

        public ActionResult Login() {
            return View();
        }

        [HttpPost]
        public ActionResult Login(User user) {
            int id;
            if ((id = logic.user.authUser(user.username, user.password)) == 0) {
                user.incorrectCredentials = "Invalid username or password.";
                return View("Login", user);
            }
            else {
                Session["userId"] = id;
                Console.WriteLine(Session["userId"]);
                Console.WriteLine(int.TryParse(Session["userId"].ToString(), out _));
                Session["roleId"] = logic.user.getRoleId(user.username);
                return RedirectToAction("Index", "Home");
            }
        }

        public ActionResult logout() {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}