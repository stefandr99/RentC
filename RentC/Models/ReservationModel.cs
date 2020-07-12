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
    public class ReservationModel
    {
        public bool registerCarRent(Reservation reservation, DbConnection db)
        {
            string query = "";
            if(reservation.couponCode.Equals(""))
                query = "INSERT INTO Reservations VALUES " +
                    "(@carID, @customerID, @reservStatsID, @startDate, @endDate, @location)";
            else query = "INSERT INTO Reservations VALUES " +
                    "(@carID, @customerID, @reservStatsID, @startDate, @endDate, @location, @couponCode)";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
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

        public bool verifyReservation(int customerId, DbConnection db)
        {
            string query = "SELECT CustomerID FROM Reservations where CustomerID = @customerId";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@customerId", customerId);
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
    }
}
