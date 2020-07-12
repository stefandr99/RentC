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
        public List<Car> listAvailableCars(DbConnection db)
        {
            string query = "SELECT c.* FROM Cars c LEFT JOIN Reservations r ON c.CarID = r.CarID ORDER BY CarID";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Car> cars = new List<Car>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Car car = new Car(reader.GetInt32(0), reader.GetString(1),reader.GetString(2), 
                                reader.GetString(3), reader.GetDecimal(4), reader.GetString(5));
                            cars.Add(car);
                        }
                    }

                    db.getDbConnection().Close();
                    return cars;
                }
            }
        }

        public bool verifyExistenceCar(string plate, DbConnection db)
        {
            string query = "SELECT CarID FROM Cars where Plate = @plate";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@plate", plate);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    db.getDbConnection().Close();
                    if (reader.HasRows)
                        return true;
                    return false;
                }
            }
        }

        public bool verifyAvailableCar(string plate, DbConnection db)
        {
            string query = "SELECT c.* FROM Cars c LEFT JOIN Reservations r ON c.CarID = r.CarID where c.Plate = @plate";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@plate", plate);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    db.getDbConnection().Close();
                    if (reader.HasRows)
                        return true;
                    return false;
                }
            }
        }

        public int verifyCarLocationAndGetId(string plate, string location, DbConnection db)
        {
            string query = "SELECT CarID FROM Cars where Plate = @plate and City = @location";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@plate", plate);
                command.Parameters.AddWithValue("@location", location);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    db.getDbConnection().Close();
                    if (reader.HasRows)
                        return reader.GetInt32(0);
                    return 0;
                }
            }
        }
    }
}
