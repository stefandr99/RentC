using RentC_MVC.Util;
using RentC_MVC.Models;
using System; 
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC_MVC.Repositories;

namespace RentC_MVC.DAL
{
    public class CarData
    {
        private IRepository<Car> carRepository { get; set; }

        public CarData(IRepository<Car> car) {
            carRepository = car;
        }

        /**
         * 0 = Database error;
         * 1 = Success;
         * 2 = There is already a car with this plate in this city;
         */
        public int register(Car car, DbConnection db) {
            return this.carRepository.register(car, db);
        }

        /**
         * 1 = Success!
         * 2 = You don't have the permission to do this.
         * 3 = This car doesn't exist.
         */
        public bool remove(int carId, DbConnection db) {
            return carRepository.remove(carId, db);
        }

        public bool update(Car car, DbConnection db) {
            return carRepository.update(car, db);
        }

        public List<Car> list(int orderBy, string ascendent, DbConnection db) {
            return carRepository.list(orderBy, ascendent, db);
        }

        public Car findById(int id, DbConnection db) {
            return carRepository.findById(id, db);
        }

        public List<Car> listMostRecentCars(DbConnection db) {
            string query =
                "SELECT TOP(10) c.* FROM Cars c JOIN Reservations r ON c.CarID = r.CarID ORDER BY r.StartDate DESC";

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
                "SELECT TOP(10) COUNT(r.CarID), c.CarID, c.Plate, c.Manufacturer, c.Model, c.PricePerDay, c.City FROM Cars c " +
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
                "SELECT TOP(10) COUNT(r.CarID), c.CarID, c.Plate, c.Manufacturer, c.Model, c.PricePerDay, c.City FROM Cars c " +
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

        public bool verifyExistenceCar(int id, DbConnection db) {
            string query = "SELECT CarID FROM Cars where CarID = @id";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                command.Parameters.AddWithValue("@id", id);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    if (reader.HasRows) {
                        db.getDbConnection().Close();
                        return true;
                    }
                    db.getDbConnection().Close();
                    return false;
                }
            }
        }

        public bool verifyAvailableCar(int id, DbConnection db) {
            string query =
                "SELECT c.* FROM Cars c LEFT JOIN Reservations r ON c.CarID = r.CarID where c.CarID = @id";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                command.Parameters.AddWithValue("@id", id);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    if (reader.HasRows)
                    {
                        db.getDbConnection().Close();
                        return true;
                    }
                    db.getDbConnection().Close();
                    return false;
                }
            }
        }

        public bool verifyCarLocation(int id, string location, DbConnection db) {
            string query = "SELECT CarID FROM Cars where CarID = @id and City = @location";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@location", location);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    if (reader.HasRows) {
                        db.getDbConnection().Close();
                        return true;
                    }


                    db.getDbConnection().Close();
                    return false;
                }
            }
        }
    }
}