using RentC_MVC.Util;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC_MVC.Repositories
{
    public class CouponRepository
    {
        public string getCoupon(DbConnection db)
        {
            string query = "SELECT TOP 1 c.CouponCode FROM Coupons c LEFT JOIN Reservations r " +
                "ON c.CouponCode = r.CouponCode ORDER BY NEWID()";
            
            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        db.getDbConnection().Close();
                        return reader.GetString(0);
                    }
                    db.getDbConnection().Close();
                    return "";
                }
            }
        }
    }
}
