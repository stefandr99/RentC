using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC_MVC.Util;
using RentC_MVC.Models;
using RentC_MVC.Repositories;

namespace RentC_MVC.DAL
{
    public class UserData
    {
        private IRepository<User> userRepository { get; set; }

        public UserData(IRepository<User> user) {
            userRepository = user;
        }
        public int authUser(int userId, string password, DbConnection db) {
            string query = "SELECT Password, RoleID FROM Users where UserID = @userId and Enabled = 1";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@userId", userId);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    
                    if (reader.HasRows)
                        if (reader.Read()) {
                            if (password.Equals(reader.GetString(0))) {
                                int res = reader.GetInt32(1);
                                db.getDbConnection().Close();
                                return res;
                            }
                        }
                    db.getDbConnection().Close();
                    return 0;
                }
            }
        }

        public bool changePassword(int userId, string oldPass, string newPass, DbConnection db) {
            string query = "SELECT UserID FROM Users where UserID = @userId and Password = @password and Enabled = 1";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@password", oldPass);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (!reader.HasRows) {
                        db.getDbConnection().Close();
                        return false;
                    }
                }
            }

            query = "UPDATE Users SET Password = @password where UserID = @userId";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@password", newPass);

                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public int register(User user, DbConnection db) {
            return userRepository.register(user, db);
        }

        public bool enableUser(int userId, DbConnection db)
        {
            string query = "UPDATE Users SET Enabled = 1 where UserID = @userId";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@userId", userId);
                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public bool remove(int userId, DbConnection db) {
            return userRepository.remove(userId, db);
        }

        public List<User> list(int orderBy, string ascendent, DbConnection db) {
            return userRepository.list(orderBy, ascendent, db);
        }
    }
}
