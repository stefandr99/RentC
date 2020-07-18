using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC.Util;
using RentC.Entities;

namespace RentC.Models
{
    public class UserModel
    {
        public bool authUser(int userId, string password, DbConnection db) {
            string query = "SELECT Password, RoleID FROM Users where UserID = @userId and Enabled = 1";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@userId", userId);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    
                    if (reader.HasRows)
                        if (reader.Read()) {
                            if (password.Equals(reader.GetString(0))) {
                                User.roleId = reader.GetInt32(1);
                                db.getDbConnection().Close();
                                return true;
                            }
                        }
                    db.getDbConnection().Close();
                    return false;
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

        public bool registerSaleperson(User user, DbConnection db) {
            string query = "INSERT INTO Users VALUES (@id, @password, @enabled, @roleId)";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@id", user.userId);
                command.Parameters.AddWithValue("@password", user.password);
                command.Parameters.AddWithValue("@enabled", user.enabled);
                command.Parameters.AddWithValue("@roleId", 3);

                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public bool disableUser(int userId, DbConnection db) {
            string query = "UPDATE Users SET Enabled = 0 where UserID = @userId";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@userId", userId);

                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public List<User> list(int orderBy, string ascendent, DbConnection db)
        {
            string query = "SELECT * FROM Users ORDER BY " + orderBy + " " + ascendent;

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<User> users = new List<User>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            User user = new User(reader.GetInt32(0), reader.GetString(1), 
                                reader.GetInt32(2), reader.GetInt32(3));
                            users.Add(user);
                        }
                    }

                    db.getDbConnection().Close();
                    return users;
                }
            }
        }
    }
}
