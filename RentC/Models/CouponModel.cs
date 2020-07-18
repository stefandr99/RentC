using RentC.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Models
{
    public class CouponModel
    {
        public string getCoupon(DbConnection db)
        {
            string query = "SELECT TOP 1 c.CouponCode FROM Coupons c LEFT JOIN Reservations r " +
                "ON c.CouponCode = r.CouponCode ORDER BY NEWID()";
            db.getDbConnection().Open();
            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader.GetString(0);
                    }
                    return "";
                }
            }
        }
    }
}
