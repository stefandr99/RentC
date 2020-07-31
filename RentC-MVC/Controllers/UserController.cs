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
    public class UserController : Controller
    {
        private readonly Logic logic;

        public UserController()
        {
            logic = new Logic();
        }

        [HttpGet]
        public ActionResult List(int orderBy, string ascendent)
        {
            List<User> users = logic.user.list(orderBy, ascendent);
            return View(users);
        }

        [HttpGet]
        public ActionResult Search(string criteria, string search)
        {
            List<User> users = logic.user.search(criteria, search);
            return View("List", users);
        }

        public ActionResult Create()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (logic.user.verifyUsername(user.username) == Util.Response.USED_USERNAME) {
                user.errorMessage = "This username is already used.";
                return View(user);
            }
            else
            {
                logic.user.registerSaleperson(user.username, user.password);
                return RedirectToAction("List", new { orderBy = 1, ascendent = "ASC" });
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
                Response res = logic.user.changePassword(id, user.password, user.newPassword, user.confirmNewPassword);
                if (res == Util.Response.NOT_MATCH_PASS) {
                    userToEdit.errorMessage = "Passwords do not match";
                    return View(userToEdit);
                }
                else if (res == Util.Response.INCORRECT_OLD_PASS) {
                    userToEdit.errorMessage = "Old password is not correct. Is that you?";
                    return View(userToEdit);
                }
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
                user.errorMessage = "Invalid username or password.";
                return View("Login", user);
            }
            else {
                Session["userId"] = id;
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