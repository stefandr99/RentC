using RentC.Util;
using RentC.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.DAL
{
    public class ReservationData
    {
        public int register(Reservation reservation, DbConnection db) {
            string query = reservation.couponCode.Equals("")
                ? "INSERT INTO Reservations VALUES " +
                  "(@carID, @customerID, @reservStatsID, @startDate, @endDate, @location)"
                : "INSERT INTO Reservations VALUES " +
                  "(@carID, @customerID, @reservStatsID, @startDate, @endDate, @location, @couponCode)";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                command.Parameters.AddWithValue("@carID", reservation.carId);
                command.Parameters.AddWithValue("@customerID", reservation.customerId);
                command.Parameters.AddWithValue("@reservStatsId", 1);
                command.Parameters.AddWithValue("@startDate", reservation.startDate);
                command.Parameters.AddWithValue("@endDate", reservation.endDate);
                command.Parameters.AddWithValue("@location", reservation.location);
                if (!reservation.couponCode.Equals(""))
                    command.Parameters.AddWithValue("@couponCode", reservation.couponCode);

                db.getDbConnection().Close();
                db.getDbConnection().Open();
                int result = command.ExecuteNonQuery();
                db.getDbConnection().Close();

                return result;
            }
        }

        public bool update(Reservation reservation, DbConnection db) {
            string query = "UPDATE Reservations SET StartDate = @start, EndDate = @end, ReservStatsID = 1 WHERE CarID = @id";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@start", reservation.startDate);
                command.Parameters.AddWithValue("@end", reservation.endDate);
                command.Parameters.AddWithValue("@id", reservation.carId);

                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public bool verifyReservation(int reservationId, DbConnection db) {
            string query = "SELECT CustomerID FROM Reservations r WHERE CarID = @id " +
                           "and ReservStatsID = 1";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                command.Parameters.AddWithValue("@id", reservationId);
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

        public bool verifyReservation2(int carId, int customerId, DateTime startDate, DateTime endDate, DbConnection db)
        {
            string query = "SELECT CustomerID FROM Reservations r WHERE CarID = @carId AND CustomerID = @customerId AND StartDate = @start AND EndDate = @end " +
                           "and ReservStatsID = 1";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@carId", carId);
                command.Parameters.AddWithValue("@customerId", customerId);
                command.Parameters.AddWithValue("@start", startDate);
                command.Parameters.AddWithValue("@end", endDate);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

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

        public List<Reservation> list(int orderBy, string ascendent, DbConnection db) {
            string query = "SELECT * FROM Reservations where ReservStatsID = 1 ORDER BY " + orderBy + " " + ascendent;

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    List<Reservation> reservations = new List<Reservation>();
                    if (reader.HasRows) {
                        while (reader.Read()) {
                            Reservation reservation = new Reservation(reader.GetInt32(0), reader.GetInt32(1),
                                reader.GetDateTime(3), reader.GetDateTime(4), reader.GetString(5));
                            reservations.Add(reservation);
                        }
                    }

                    db.getDbConnection().Close();
                    return reservations;
                }
            }
        }

        /**
         * Could change status depending of CarID or CustomerID
         */
        public bool remove(int carId, int customerId, DateTime startDate, DateTime endDate,  DbConnection db) {
            string query = "UPDATE Reservations SET ReservStatsID = 3 where CarID = @carId AND CustomerID = @customerId AND " +
                           "StartDate = @start AND EndDate = @end";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@carId", carId);
                command.Parameters.AddWithValue("@customerId", customerId);
                command.Parameters.AddWithValue("@start", startDate);
                command.Parameters.AddWithValue("@end", endDate);

                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public bool expiredReservations(DbConnection db) {
            string query = "UPDATE Reservations SET ReservStatsID = 2 where ReservStatsID = 1 and EndDate < CURRENT_TIMESTAMP";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {

                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public List<Tuple<int, Customer>> goldCustomers(DbConnection db) {
            string query = "SELECT COUNT(c.CustomerID), c.CustomerID, c.Name, c.BirthDate, c.Location, c.ZIPCode FROM Reservations r JOIN " +
                           "Customers c ON c.CustomerID = r.CustomerID  WHERE r.StartDate >= (CURRENT_TIMESTAMP - 30) " +
                           "GROUP BY c.CustomerID, c.Name, c.BirthDate, c.Location, c.ZIPCode HAVING COUNT(c.CustomerID) >= 4";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Tuple<int, Customer>> gold = new List<Tuple<int, Customer>>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer(reader.GetInt32(1), reader.GetString(2),
                                reader.GetDateTime(3), reader.IsDBNull(4) ? "" : reader.GetString(4), reader.IsDBNull(5) ? "" : reader.GetString(5));
                            gold.Add(new Tuple<int, Customer>(reader.GetInt32(0), customer));
                        }
                    }

                    db.getDbConnection().Close();
                    return gold;
                }
            }
        }

        public List<Tuple<int, Customer>> silverCustomers(DbConnection db)
        {
            string query = "SELECT COUNT(c.CustomerID), c.CustomerID, c.Name, c.BirthDate, c.Location, c.ZIPCode FROM Reservations r JOIN " +
                           "Customers c ON c.CustomerID = r.CustomerID  WHERE r.StartDate >= (CURRENT_TIMESTAMP - 30) " +
                           "GROUP BY c.CustomerID, c.Name, c.BirthDate, c.Location, c.ZIPCode HAVING COUNT(c.CustomerID) IN (2, 3)";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Tuple<int, Customer>> gold = new List<Tuple<int, Customer>>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer(reader.GetInt32(1), reader.GetString(2),
                                reader.GetDateTime(3), reader.IsDBNull(4) ? "" : reader.GetString(4), reader.IsDBNull(5) ? "" : reader.GetString(5));
                            gold.Add(new Tuple<int, Customer>(reader.GetInt32(0), customer));
                        }
                    }

                    db.getDbConnection().Close();
                    return gold;
                }
            }
        }
    }
}