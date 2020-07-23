﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC_MVC.Util;
using RentC_MVC.Models;

namespace RentC_MVC.Repositories
{
    public class UserRepository : IRepository<User>
    {
        public int register(User user, DbConnection db) {
            string query = "INSERT INTO Users VALUES (@password, @enabled, @roleId)";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@password", user.password);
                command.Parameters.AddWithValue("@enabled", false);
                command.Parameters.AddWithValue("@roleId", 3);

                db.getDbConnection().Open();
                int result = command.ExecuteNonQuery();
                db.getDbConnection().Close();

                return result;
            }
        }

        public bool remove(int userId, DbConnection db) {
            string query = "UPDATE Users SET Enabled = 0 where UserID = @userId";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@userId", userId);
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
                            User user = new User(reader.GetInt32(0), reader.GetString(1), 
                                reader.GetBoolean(2), reader.GetInt32(3));
                            users.Add(user);
                        }
                    }

                    db.getDbConnection().Close();
                    return users;
                }
            }
        }

        public bool update(User obj, DbConnection db) {
            throw new NotImplementedException();
        }
    }
}