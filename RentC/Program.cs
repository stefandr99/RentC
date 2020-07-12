using RentC.Db;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC
{
    class Program
    {
        static void Main(string[] args)
        {
            /*DbConnection db = DbConnection.getInstance;

            SqlDataAdapter data = new SqlDataAdapter("select * from Cars", db.getDbConnection());
            DataTable dataTable = new DataTable();

            data.Fill(dataTable);

            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine(row["Plate"]);
            }*/
            string format = "dd/MM/yyyy";
            DateTime expectedDate;
            if (!DateTime.TryParseExact("7/12/2012", format, new CultureInfo("en-US"),
                                DateTimeStyles.None, out expectedDate))
                Console.WriteLine("Not ok");
            else Console.WriteLine("Ok");

            /*
             * Verificare daca e in format ZIP code US:
             * if(!int.TryParse("123", out _) && string.Length != 5) not ok;
             */

            /* DateTime.Compare(t1, t2); Greater than zero	-> t1 is later than t2. => not this . Trebuie sa fie >= cu PREZENTUL*/

            Console.ReadKey();
        }
    }
}
