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
    public class ReservationData
    {
        private IRepository<Reservation> reservationRepository { get; set; }

        public ReservationData(IRepository<Reservation> reservation) {
            reservationRepository = reservation;
        }
        public int register(Reservation reservation, DbConnection db) {
            return reservationRepository.register(reservation, db);
        }

        public bool update(Reservation reservation, DbConnection db) {
            return reservationRepository.update(reservation, db);
        }

        public Reservation findById(int id, DbConnection db)
        {
            return reservationRepository.findById(id, db);
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

        public List<Reservation> list(int orderBy, string ascendent, DbConnection db) {
            return reservationRepository.list(orderBy, ascendent, db);
        }

        /**
         * Could change status depending of CarID or CustomerID
         */
        public bool remove(int identifyId,  DbConnection db) {
            return reservationRepository.remove(identifyId, db);
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
                            Customer customer = new Customer(reader.GetInt32(1), reader.GetString(2), reader.GetDateTime(3),
                                reader.GetString(4), reader.GetString(5));
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
                            Customer customer = new Customer(reader.GetInt32(1), reader.GetString(2), reader.GetDateTime(3),
                                reader.GetString(4), reader.GetString(5));
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