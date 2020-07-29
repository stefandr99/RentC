using RentC_MVC.Util;
using RentC_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC_MVC.Repositories
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
                Debug.WriteLine(customer.birthDate);

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
                                reader.GetDateTime(2), reader.IsDBNull(3) ? "" : reader.GetString(3), 
                                reader.IsDBNull(4) ? "" : reader.GetString(4));
                            customers.Add(customer);
                        }
                    }

                    db.getDbConnection().Close();
                    return customers;
                }
            }
        }

        public List<Customer> search(string criteria, string search, DbConnection db)
        {
            string query;
            if (criteria.Equals("CustomerID"))
            {
                query = "SELECT * FROM Customers WHERE " + criteria + " = " + search + "";
            }
            else
            {
                query = "SELECT * FROM Customers WHERE " + criteria + " LIKE '%" + search + "%'";
            }

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
                                reader.GetDateTime(2), reader.IsDBNull(3) ? "" : reader.GetString(3),
                                reader.IsDBNull(4) ? "" : reader.GetString(4));
                            customers.Add(customer);
                        }
                    }

                    db.getDbConnection().Close();
                    return customers;
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

        public Customer findById(int id, DbConnection db)
        {
            string query = "SELECT * FROM Customers c WHERE c.CustomerID = " + id + "";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            Customer customer = new Customer(reader.GetInt32(0), reader.GetString(1),
                                reader.GetDateTime(2), reader.IsDBNull(3) ? "" : reader.GetString(3), 
                                reader.IsDBNull(4) ? "" : reader.GetString(4));
                            db.getDbConnection().Close();
                            return customer;
                        }
                    }
                }

                return null;
            }
        }
    }
}