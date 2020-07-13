using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC.Db;
using RentC.Entities;

namespace RentC.Models
{
    class UserModel
    {
        public bool authUser(int userId, string password, DbConnection db) {
            string query = "SELECT Password, RoleID FROM Users where UserID = @userId";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@userId", userId);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    db.getDbConnection().Close();
                    if (reader.HasRows)
                        if (reader.Read()) {
                            if (password.Equals(reader.GetString(0))) {
                                User.roleId = reader.GetInt32(1);
                                return true;
                            }
                        }
                    return false;
                }
            }
        }

        public bool changePassword(int userId, string oldPass, string newPass, DbConnection db) {
            string query = "SELECT UserID FROM Users where UserID = @userId and Password = @password";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@password", oldPass);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    db.getDbConnection().Close();
                    if (!reader.HasRows)
                        return false;
                }
            }

            query = "UPDATE Users SET Password = @password where UserID = @userId";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@userId", userId);
                command.Parameters.AddWithValue("@password", oldPass);

                db.getDbConnection().Open();
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
    }
}
