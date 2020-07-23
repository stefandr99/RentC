﻿using RentC_MVC.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC_MVC.Models;

namespace RentC_MVC.Repositories
{
    public class CarRepository : IRepository<Car>
    {
        /**
         * 0 = Database error;
         * 1 = Success;
         * 2 = There is already a car with this plate in this city;
         */
        public int register(Car car, DbConnection db) {
            string query = "SELECT CarID FROM Cars WHERE Plate = @plate and City = @city";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@plate", car.plate);
                command.Parameters.AddWithValue("@city", car.city);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows) {
                        return 2;
                    }
                    db.getDbConnection().Close();
                }
            }


            query = "INSERT INTO Cars VALUES (@plate, @manufacturer, @model, @pricePerDay, @city)";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@plate", car.plate);
                command.Parameters.AddWithValue("@manufacturer", car.manufacturer);
                command.Parameters.AddWithValue("@model", car.model);
                command.Parameters.AddWithValue("@pricePerDay", car.pricePerDay);
                command.Parameters.AddWithValue("@city", car.city);

                db.getDbConnection().Open();
                int result = command.ExecuteNonQuery();
                db.getDbConnection().Close();

                return result;
            }
        }

        /**
         * 1 = Success!
         * 2 = You don't have the permission to do this.
         * 3 = This car doesn't exist.
         */
        public bool remove(int carId, DbConnection db) {

            string query = "DELETE FROM Cars WHERE CarID = @id";
            bool result;
            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@id", carId);

                db.getDbConnection().Open();
                
                try {
                    result = command.ExecuteNonQuery() > 0;
                }
                catch (System.Data.SqlClient.SqlException) {
                    result = false;
                }
            }

            db.getDbConnection().Close();

            return result;
        }

        public bool update(Car car, DbConnection db)
        {
            string query = "UPDATE Cars SET Plate = @plate, Manufacturer = @manufacturer, Model = @model, pricePerDay = @pricePerDay," +
                           " City = @city WHERE CarID = @id";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@plate", car.plate);
                command.Parameters.AddWithValue("@manufacturer", car.manufacturer);
                command.Parameters.AddWithValue("@model", car.model);
                command.Parameters.AddWithValue("@pricePerDay", car.pricePerDay);
                command.Parameters.AddWithValue("@city", car.city);

                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public List<Car> list(int orderBy, string ascendent, DbConnection db) {
            string query = "SELECT DISTINCT c.* FROM Cars c WHERE c.CarID NOT IN (SELECT ca.CarId FROM Cars ca " +
                           "JOIN Reservations r ON ca.CarID = r.CarID WHERE r.ReservStatsID = 1)" +
                           " ORDER BY " + orderBy + " " + ascendent;

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    List<Car> cars = new List<Car>();
                    if (reader.HasRows) {
                        while (reader.Read()) {
                            Car car = new Car(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                                reader.GetString(3), reader.GetDecimal(4), reader.GetString(5));
                            cars.Add(car);
                        }
                    }

                    db.getDbConnection().Close();
                    return cars;
                }
            }
        }
    }
}