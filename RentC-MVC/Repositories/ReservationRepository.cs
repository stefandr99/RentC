using RentC_MVC.Util;
using RentC_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC_MVC.Repositories
{
    public class ReservationRepository : IRepository<Reservation>
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

                db.getDbConnection().Open();
                int result = command.ExecuteNonQuery();
                db.getDbConnection().Close();

                return result;
            }
        }

        public bool update(Reservation reservation, DbConnection db) {
            string query = "UPDATE Reservations SET StartDate = @start, EndDate = @end, ReservStatsID = 1 WHERE CarID = @carId AND " +
                           "CustomerID = @customerId";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@start", reservation.startDate);
                command.Parameters.AddWithValue("@end", reservation.endDate);
                command.Parameters.AddWithValue("@carId", reservation.carId);
                command.Parameters.AddWithValue("@customerId", reservation.customerId);

                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
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
        public bool remove(int identifyId,  DbConnection db) {
            string query = "UPDATE Reservation SET ReservStatsID = 3 where CarID = @id OR CustomerID = @id";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@id", identifyId);

                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public Reservation findById(int id, DbConnection db)
        {
            string query = "SELECT * FROM Reservations c WHERE c.CarID = " + id + "";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            Reservation reservation = new Reservation(reader.GetInt32(0), reader.GetInt32(1),
                                reader.GetDateTime(3), reader.GetDateTime(4), reader.GetString(5));
                            db.getDbConnection().Close();
                            return reservation;
                        }
                    }
                }

                return null;
            }
        }

        public List<Reservation> search(string criteria, string search, DbConnection db)
        {
            string query;
            if (criteria.Equals("CustomerID") || criteria.Equals("CarID"))
            {
                query = "SELECT * FROM Customers WHERE " + criteria + " = " + search + "";
            }
            else
            {
                query = "SELECT * FROM Customers WHERE " + criteria + " LIKE '%" + search + "%'";
            }

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Reservation> reservations = new List<Reservation>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
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
    }
}