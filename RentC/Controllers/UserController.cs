using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC.Db;
using RentC.Entities;
using RentC.Models;

namespace RentC.Controllers
{
    class UserController
    {
        public DbConnection db { get; }
        public UserModel userModel { get; }

        UserController() {
            userModel = new UserModel();
            db = DbConnection.getInstance;
        }

        public string authUser(int userId, string password) {
            if (userId == 0 || password.Equals("")) {
                return "Please fill both fields!";
            }

            if (!userModel.authUser(userId, password, db)) {
                return "UserID or password incorrect!";
            }

            return "Authentication with success";
        }

        public string changePassword(int userId, string oldPass, string newPass1, string newPass2) {
            if (userId == 0 || oldPass.Equals("") || newPass1.Equals("")) {
                return "Please fill all fields!";
            }

            if (!newPass1.Equals(newPass2))
                return "Passwords don't match!";
            if (!userModel.changePassword(userId, oldPass, newPass1, db))
                return "Your old password is incorrect!";
            return "Password changed with success!";
        }

        public string registerSaleperson(int userId, string password) {
            if (userId == 0 || password.Equals(""))
            {
                return "Please fill all fields!";
            }

            if (!userModel.registerSaleperson(new User(userId, password), db))
                return "There was a problem with registration! Please try again!";
            return "Registration with success!";
        }
    }
}
