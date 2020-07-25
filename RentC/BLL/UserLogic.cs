using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC.Entities;
using RentC.DAL;
using RentC.Util;

namespace RentC.BLL
{
    public class UserLogic
    {
        public DbConnection db { get; }
        public UserData userData { get; }

        public UserLogic() {
            userData = new UserData();
            db = DbConnection.getInstance;
        }

        public Response authUser(string username, string password) {
            if (username.Equals("") || password.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }

            int res = userData.authUser(username, password, db);
            return res == 0
                ? Response.INCORRECT_CREDENTIALS
                : (res == 1
                    ? Response.SUCCESS_ADMIN
                    : (res == 2 ? Response.SUCCESS_MANAGER : Response.SUCCESS_SALESPERSON));

        }

        public Response changePassword(string username, string oldPass, string newPass1, string newPass2) {
            if (username.Equals("") || oldPass.Equals("") || newPass1.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }


            if (!newPass1.Equals(newPass2))
                return Response.NOT_MATCH_PASS;
            if (!userData.changePassword(username, oldPass, newPass1, db))
                return Response.INCORRECT_OLD_PASS;
            return Response.SUCCESS;
        }

        public Response registerSaleperson(string username, string password) {
            if (password.Equals("") || username.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }

            if (userData.register(new User(username, password), db) == 0)
                return Response.DATABASE_ERROR;
            return Response.SUCCESS;
        }

        public Response enableUser(string username)
        {
            if (username.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }

            
            if (!userData.enableUser(username, db))
            {
                return Response.DATABASE_ERROR;
            }

            return Response.SUCCESS;
        }

        public Response disableUser(string username)
        {
            if (username.Equals(""))
            {
                return Response.UNFILLED_FIELDS;
            }

            if (!userData.remove(username, db))
            {
                return Response.DATABASE_ERROR;
            }

            return Response.SUCCESS;
        }

        public List<User> list(int orderBy, string asc) {
            return userData.list(orderBy, asc, db);
        }
    }
}
