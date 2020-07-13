using RentC.Db;
using RentC.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Models
{
    public class CarModel
    {
        public bool register(Car car, DbConnection db) {
            string query = "INSERT INTO Cars VALUES (@carId, @plate, @manufacturer, @model, @pricePerDay, @city)";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@carId", car.carId);
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

        /**
         * 1 = Success!
         * 2 = You don't have the permission to do this.
         * 3 = This car doesn't exist.
         */
        public int removeCar(int carId, DbConnection db) {
            if (User.roleId != 1 && User.roleId != 2)
                return 2;

            string query = "DELETE FROM Cars WHERE CarID = @id";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@id", carId);

                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result ? 1 : 3;
            }
        }

        public List<Car> listAvailableCars(int orderBy, string ascendent, DbConnection db) {
            string query = "SELECT c.* FROM Cars c LEFT JOIN Reservations r ON c.CarID = r.CarID ORDER BY " + orderBy +
                           " " + ascendent;

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

        public List<Car> listMostRecentCars(DbConnection db) {
            string query =
                "SELECT TOP(10) c.* FROM Cars c RIGHT JOIN Reservations r ON c.CarID = r.CarID ORDER BY r.StartDate ASC";

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

        public List<Tuple<int, Car>> listMostRentedCarsByMonth(int month, int year, DbConnection db) {
            string query =
                "SELECT TOP(10) COUNT(r.CarID), c.CarID, c.Plate, c.City, c.Manufacturer, c.Model, c.PricePerDay FROM Cars c " +
                "JOIN Reservations r ON c.CarID = r.CarID WHERE MONTH(r.StartDate) = @month AND YEAR(r.StartDate) = @year " +
                "GROUP BY c.CarID, c.Plate, c.City, c.Manufacturer, c.Model, c.PricePerDay ORDER BY COUNT(r.CarID) DESC";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    List<Tuple<int, Car>> cars = new List<Tuple<int, Car>>();
                    if (reader.HasRows) {
                        while (reader.Read()) {
                            Car car = new Car(reader.GetInt32(1), reader.GetString(2), reader.GetString(3),
                                reader.GetString(4), reader.GetDecimal(5), reader.GetString(6));
                            cars.Add(new Tuple<int, Car>(reader.GetInt32(0), car));
                        }
                    }

                    db.getDbConnection().Close();
                    return cars;
                }
            }
        }

        // Nu sigur, trebuie facute niste verificari la interogare. La fel si mai sus.
        public List<Tuple<int, Car>> listLessRentedCarsByMonth(int month, int year, DbConnection db)
        {
            string query =
                "SELECT TOP(10) COUNT(r.CarID), c.CarID, c.Plate, c.City, c.Manufacturer, c.Model, c.PricePerDay FROM Cars c " +
                "JOIN Reservations r ON c.CarID = r.CarID WHERE MONTH(r.StartDate) = @month AND YEAR(r.StartDate) = @year " +
                "GROUP BY c.CarID, c.Plate, c.City, c.Manufacturer, c.Model, c.PricePerDay ORDER BY COUNT(r.CarID) ASC";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@month", month);
                command.Parameters.AddWithValue("@year", year);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Tuple<int, Car>> cars = new List<Tuple<int, Car>>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Car car = new Car(reader.GetInt32(1), reader.GetString(2), reader.GetString(3),
                                reader.GetString(4), reader.GetDecimal(5), reader.GetString(6));
                            cars.Add(new Tuple<int, Car>(reader.GetInt32(0), car));
                        }
                    }

                    db.getDbConnection().Close();
                    return cars;
                }
            }
        }

        public bool verifyExistenceCar(string plate, DbConnection db) {
            string query = "SELECT CarID FROM Cars where Plate = @plate";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                command.Parameters.AddWithValue("@plate", plate);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    db.getDbConnection().Close();
                    if (reader.HasRows)
                        return true;
                    return false;
                }
            }
        }

        public bool verifyAvailableCar(string plate, DbConnection db) {
            string query =
                "SELECT c.* FROM Cars c LEFT JOIN Reservations r ON c.CarID = r.CarID where c.Plate = @plate";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                command.Parameters.AddWithValue("@plate", plate);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    db.getDbConnection().Close();
                    if (reader.HasRows)
                        return true;
                    return false;
                }
            }
        }

        public int verifyCarLocationAndGetId(string plate, string location, DbConnection db) {
            string query = "SELECT CarID FROM Cars where Plate = @plate and City = @location";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                command.Parameters.AddWithValue("@plate", plate);
                command.Parameters.AddWithValue("@location", location);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    db.getDbConnection().Close();
                    if (reader.HasRows)
                        return reader.GetInt32(0);
                    return 0;
                }
            }
        }
    }
}