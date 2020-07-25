using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC.Util;
using RentC.Entities;

namespace RentC.DAL
{
    public class UserData
    {
        public int authUser(string username, string password, DbConnection db) {
            string query = "SELECT Password, RoleID FROM Users where Username = @username and Enabled = 1";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@username", username);
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

        public bool changePassword(string username, string oldPass, string newPass, DbConnection db) {
            string query = "SELECT UserID FROM Users where Username = @username and Password = @password and Enabled = 1";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@username", username);
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

            query = "UPDATE Users SET Password = @password where Username = @username";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", newPass);

                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public int register(User user, DbConnection db) {
            string query = "INSERT INTO Users (Username, Password, Enabled, RoleID) VALUES (@username, @password, @enabled, @roleId)";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@username", user.username);
                command.Parameters.AddWithValue("@password", user.password);
                command.Parameters.AddWithValue("@enabled", true);
                command.Parameters.AddWithValue("@roleId", 3);

                db.getDbConnection().Open();
                int result = command.ExecuteNonQuery();
                db.getDbConnection().Close();

                return result;
            }
        }

        public bool enableUser(string username, DbConnection db)
        {
            string query = "UPDATE Users SET Enabled = 1 where Username = @username";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@username", username);
                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public bool remove(string username, DbConnection db) {
            string query = "UPDATE Users SET Enabled = 0 where Username = @username";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@username", username);
                db.getDbConnection().Open();
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
                            User user = new User(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), 
                                reader.GetBoolean(3), reader.GetInt32(4));
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
