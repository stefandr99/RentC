using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC_MVC.Util
{
    public sealed class DbConnection
    {
        public static SqlConnection conn;
        private static DbConnection instance = null;
        private DbConnection() {
            conn = new SqlConnection(@"Server=DESKTOP-GPUSS5T\SQLEXPRESS01;Database=academy_net;Trusted_Connection=True;");
        }
        
        public static DbConnection getInstance
        {
            get
            {
                if(instance == null)
                {
                    instance = new DbConnection();
                }
                return instance;
            }
        }

        public SqlConnection getDbConnection()
        {
            return conn;
        }
    }
}
