﻿using RentC.Db;
using RentC.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Models
{
    public class ReservationModel
    {
        public bool registerCarRent(Reservation reservation, DbConnection db) {
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
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public bool verifyReservation(int customerId, DbConnection db) {
            string query = "SELECT CustomerID FROM Reservations where CustomerID = @customerId and ReservStatsID = 1";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                command.Parameters.AddWithValue("@customerId", customerId);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    db.getDbConnection().Close();
                    if (reader.HasRows)
                        return true;
                    return false;
                }
            }
        }

        public List<Reservation> listReservations(int orderBy, string ascendent, DbConnection db) {
            string query = "SELECT * FROM Reservation where ReservStatsID = 1 ORDER BY " + orderBy + " " + ascendent;

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection())) {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader()) {
                    List<Reservation> reservations = new List<Reservation>();
                    if (reader.HasRows) {
                        while (reader.Read()) {
                            Reservation reservation = new Reservation(reader.GetInt32(0), reader.GetInt32(1),
                                reader.GetDateTime(2), reader.GetDateTime(3), reader.GetString(4));
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
        public bool changeReservationStatus(int identifyId, int status, DbConnection db) {
            string query = "UPDATE Reservation SET ReservStatsID = @status where CarID = @id OR CustomerID = @id";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@status", status);
                command.Parameters.AddWithValue("@id", identifyId);

                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public bool expiredReservations(DbConnection db) {
            string query = "UPDATE Reservation SET ReservStatsID = 2 where ReservStatsID = 1 and EndDate < CURRENT_TIMESTAMP";

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