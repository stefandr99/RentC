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
    }
}