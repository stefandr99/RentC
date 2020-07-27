using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using Newtonsoft.Json;
using RentC_MVC.Models;
using RentC_MVC.Util;

namespace RentC_WS
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {

        [WebMethod]
        public string getAvailableCars(int orderBy, string ascendent)
        {
            string query = "SELECT DISTINCT c.* FROM Cars c WHERE c.CarID NOT IN (SELECT ca.CarId FROM Cars ca " +
                           "JOIN Reservations r ON ca.CarID = r.CarID WHERE r.ReservStatsID = 1)" +
                           " ORDER BY " + orderBy + " " + ascendent;

            using (SqlCommand command = new SqlCommand(query, DbConnection.getInstance.getDbConnection()))
            {
                DbConnection.getInstance.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Car> cars = new List<Car>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Car car = new Car(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),
                                reader.GetString(3), reader.GetDecimal(4), reader.GetString(5));
                            cars.Add(car);
                        }
                    }

                    DbConnection.getInstance.getDbConnection().Close();

                    string result = JsonConvert.SerializeObject(cars);
                    return result;
                }
            }
        }
    }
}
