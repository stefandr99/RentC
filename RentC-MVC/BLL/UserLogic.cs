using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC_MVC.Models;
using RentC_MVC.DAL;
using RentC_MVC.Repositories;
using RentC_MVC.Util;

namespace RentC_MVC.BLL
{
    public class UserLogic
    {
        public DbConnection db { get; }
        public UserData userData { get; }

        public UserLogic() {
            userData = new UserData(new UserRepository());
            db = DbConnection.getInstance;
        }

        public Response authUser(string userId, string password) {
            if (userId.Equals("") || password.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }

            if (!int.TryParse(userId, out int id))
                return Response.INCORRECT_ID;
            int res = userData.authUser(id, password, db);
            return res == 0
                ? Response.INCORRECT_CREDENTIALS
                : (res == 1
                    ? Response.SUCCESS_ADMIN
                    : (res == 2 ? Response.SUCCESS_MANAGER : Response.SUCCESS_SALESPERSON));

        }

        public Response changePassword(int userId, string oldPass, string newPass1, string newPass2) {
            /*if (userId.Equals("") || oldPass.Equals("") || newPass1.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }*/

            /*if (!int.TryParse(userId, out int id))
                return Response.INCORRECT_ID;*/


            if (!newPass1.Equals(newPass2))
                return Response.NOT_MATCH_PASS;
            if (!userData.changePassword(userId, oldPass, newPass1, db))
                return Response.INCORRECT_OLD_PASS;
            return Response.SUCCESS;
        }

        public Response registerSaleperson(string password) {
            if (password.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }

            if (userData.register(new User(password), db) == 0)
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

            if (!userData.enableUser(id, db))
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

            if (!userData.remove(id, db))
            {
                return Response.DATABASE_ERROR;
            }

            return Response.SUCCESS;
        }

        public List<User> list(int orderBy, string asc) {
            return userData.list(orderBy, asc, db);
        }

        public User findById(int id)
        {
            return userData.findById(id, db);
        }
    }
}
