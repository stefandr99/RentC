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

        
    }
}