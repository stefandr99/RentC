using RentC.Util;
using RentC.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Repositories
{
    public class CustomerRepository : IRepository<Customer>
    {
        public int register(Customer customer, DbConnection db)
        {
            string query = customer.ZIPCode.Equals("")
                ? "INSERT INTO Customers (Name, BirthDate) VALUES (@name, @bdate)"
                : "INSERT INTO Customers (Name, BirthDate, ZIPCode) VALUES (@name, @bdate, @zip)";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@name", customer.name);
                command.Parameters.AddWithValue("@bdate", customer.birthDate);
                if (!customer.ZIPCode.Equals(""))
                    command.Parameters.AddWithValue("@zip", customer.ZIPCode);

                db.getDbConnection().Open();
                int result = command.ExecuteNonQuery() > 0 ? 1 : 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public bool update(Customer customer, DbConnection db)
        {
            string query = "UPDATE Customers SET Name = @name, BirthDate = @bdate, ZIPCode = @zip WHERE CustomerID = @id";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@id", customer.customerId);
                command.Parameters.AddWithValue("@name", customer.name);
                command.Parameters.AddWithValue("@bdate", customer.birthDate);
                command.Parameters.AddWithValue("@zip", customer.ZIPCode);

                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }

        public List<Customer> list(int orderBy, string ascendent, DbConnection db)
        {
            string query = "SELECT * FROM Customers ORDER BY " + orderBy + " " + ascendent;

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Customer> customers = new List<Customer>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer(reader.GetInt32(0), reader.GetString(1),
                                reader.GetDateTime(2), reader.IsDBNull(3) ? "" : reader.GetString(3), reader.IsDBNull(4) ? "" : reader.GetString(4));
                            customers.Add(customer);
                        }
                    }

                    db.getDbConnection().Close();
                    return customers;
                }
            }
        }

        public bool verifyCustomer(int customerId, DbConnection db)
        {
            string query = "SELECT * FROM Customers where CustomerID = @id";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@id", customerId);
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (reader.HasRows) {
                        db.getDbConnection().Close();
                        return true;
                    }
                    db.getDbConnection().Close();
                    return false;
                }
            }
        }

        /**
         * 1 = Success!
         * 2 = You don't have the permission to do this.
         * 3 = This customer doesn't exist.
         */
        public bool remove(int customerId, DbConnection db)
        {
            string query = "DELETE FROM Customers WHERE CustomerID = @id";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                command.Parameters.AddWithValue("@id", customerId);

                db.getDbConnection().Open();
                bool result = command.ExecuteNonQuery() > 0;
                db.getDbConnection().Close();

                return result;
            }
        }
    }
}