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
    public class CustomerData
    {
        private IRepository<Customer> customerRepository { get; set; }

        public CustomerData(IRepository<Customer> customer) {
            customerRepository = customer;
        }
        public int register(Customer customer, DbConnection db) {
            return customerRepository.register(customer, db);
        }

        public bool update(Customer customer, DbConnection db) {
            return customerRepository.update(customer, db);
        }

        public List<Customer> list(int orderBy, string ascendent, DbConnection db) {
            return customerRepository.list(orderBy, ascendent, db);
        }

        public Customer findById(int id, DbConnection db) {
            return customerRepository.findById(id, db);
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
        public bool remove(int customerId, DbConnection db) {
            return customerRepository.remove(customerId, db);
        }

        public List<Tuple<int, Customer>> goldCustomers(DbConnection db)
        {
            string query = "SELECT COUNT(c.CustomerID), c.CustomerID, c.Name, c.BirthDate, c.Location, c.ZIPCode FROM Reservations r JOIN " +
                           "Customers c ON c.CustomerID = r.CustomerID  WHERE r.StartDate >= (CURRENT_TIMESTAMP - 30) " +
                           "GROUP BY c.CustomerID, c.Name, c.BirthDate, c.Location, c.ZIPCode HAVING COUNT(c.CustomerID) >= 4";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Tuple<int, Customer>> gold = new List<Tuple<int, Customer>>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer(reader.GetInt32(1), reader.GetString(2), reader.GetDateTime(3),
                                reader.GetString(4), reader.GetString(5));
                            gold.Add(new Tuple<int, Customer>(reader.GetInt32(0), customer));
                        }
                    }

                    db.getDbConnection().Close();
                    return gold;
                }
            }
        }

        public List<Tuple<int, Customer>> silverCustomers(DbConnection db)
        {
            string query = "SELECT COUNT(c.CustomerID), c.CustomerID, c.Name, c.BirthDate, c.Location, c.ZIPCode FROM Reservations r JOIN " +
                           "Customers c ON c.CustomerID = r.CustomerID  WHERE r.StartDate >= (CURRENT_TIMESTAMP - 30) " +
                           "GROUP BY c.CustomerID, c.Name, c.BirthDate, c.Location, c.ZIPCode HAVING COUNT(c.CustomerID) IN (2, 3)";

            using (SqlCommand command = new SqlCommand(query, db.getDbConnection()))
            {
                db.getDbConnection().Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Tuple<int, Customer>> gold = new List<Tuple<int, Customer>>();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Customer customer = new Customer(reader.GetInt32(1), reader.GetString(2), reader.GetDateTime(3),
                                reader.GetString(4), reader.GetString(5));
                            gold.Add(new Tuple<int, Customer>(reader.GetInt32(0), customer));
                        }
                    }

                    db.getDbConnection().Close();
                    return gold;
                }
            }
        }
    }
}