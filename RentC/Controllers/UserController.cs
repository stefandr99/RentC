﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC.Entities;
using RentC.Models;
using RentC.Util;

namespace RentC.Controllers
{
    public class UserController
    {
        public DbConnection db { get; }
        public UserModel userModel { get; }

        public UserController() {
            userModel = new UserModel();
            db = DbConnection.getInstance;
        }

        public Response authUser(string userId, string password) {
            if (userId.Equals("") || password.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }

            if (!int.TryParse(userId, out int id))
                return Response.INCORRECT_ID;
            int res = userModel.authUser(id, password, db);
            return res == 0
                ? Response.INCORRECT_CREDENTIALS
                : (res == 1
                    ? Response.SUCCESS_ADMIN
                    : (res == 2 ? Response.SUCCESS_MANAGER : Response.SUCCESS_SALESPERSON));

        }

        public Response changePassword(string userId, string oldPass, string newPass1, string newPass2) {
            if (userId.Equals("") || oldPass.Equals("") || newPass1.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }

            if (!int.TryParse(userId, out int id))
                return Response.INCORRECT_ID;


            if (!newPass1.Equals(newPass2))
                return Response.NOT_MATCH_PASS;
            if (!userModel.changePassword(id, oldPass, newPass1, db))
                return Response.INCORRECT_OLD_PASS;
            return Response.SUCCESS;
        }

        public Response registerSaleperson(string password) {
            if (password.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }

            if (!userModel.registerSaleperson(new User(password), db))
                return Response.DATABASE_ERROR;
            return Response.SUCCESS;
        }

        public Response enableUser(string userId)
        {
            if (userId.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }

            if (!int.TryParse(userId, out int id))
                return Response.INCORRECT_ID;

            if (!userModel.enableUser(id, db))
            {
                return Response.DATABASE_ERROR;
            }

            return Response.SUCCESS;
        }

        public Response disableUser(string userId)
        {
            if (userId.Equals(""))
            {
                return Response.UNFILLED_FIELDS;
            }

            if (!int.TryParse(userId, out int id))
                return Response.INCORRECT_ID;

            if (!userModel.disableUser(id, db))
            {
                return Response.DATABASE_ERROR;
            }

            return Response.SUCCESS;
        }

        public List<User> list(int orderBy, string asc) {
            return userModel.list(orderBy, asc, db);
        }
    }
}
