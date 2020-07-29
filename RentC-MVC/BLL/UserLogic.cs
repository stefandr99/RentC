using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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

        public int authUser(string username, string password) {
            return userData.authUser(username, password, db);
        }

        public int getRoleId(string username)
        {
            return userData.getRoleId(username, db);
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

        public Response registerSaleperson(string username, string password) {
            if (password.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }

            if (userData.register(new User(username, password), db) == 0)
                return Response.DATABASE_ERROR;
            return Response.SUCCESS;
        }

        public Response enableUser(int userId)
        {
            /*if (userId.Equals("")) {
                return Response.UNFILLED_FIELDS;
            }

            if (!int.TryParse(userId, out int id))
                return Response.INCORRECT_ID;*/

            if (!userData.enableUser(userId, db))
            {
                return Response.DATABASE_ERROR;
            }

            return Response.SUCCESS;
        }

        public Response disableUser(int userId)
        {
            /*if (userId.Equals(""))
            {
                return Response.UNFILLED_FIELDS;
            }

            if (!int.TryParse(userId, out int id))
                return Response.INCORRECT_ID;*/

            if (!userData.remove(userId, db))
            {
                return Response.DATABASE_ERROR;
            }

            return Response.SUCCESS;
        }

        public List<User> list(int orderBy, string asc) {
            return userData.list(orderBy, asc, db);
        }

        public List<User> search(string criteria, string search)
        {
            return userData.search(criteria, search, db);
        }

        public User findById(int id)
        {
            return userData.findById(id, db);
        }

        public Response verifyUsername(string username) {
            return userData.verifyUsername(username, db) ? Response.USED_USERNAME : Response.SUCCESS;
        }
    }
}
