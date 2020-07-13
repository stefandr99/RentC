using RentC.Db;
using RentC.Entities;
using RentC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Controllers
{
    public class CustomerController
    {
        public DbConnection db { get; }
        public CustomerModel customerModel { get; }

        public CustomerController()
        {
            db = DbConnection.getInstance;
            customerModel = new CustomerModel();
        }

        public string register(string customerId, string customerName, string birthDate, string zip)
        {
            if (!int.TryParse(customerId, out int id))
                return "The ClientID you have entered is not valid! Please enter another one!";

            string format = "dd/MM/yyyy";
            DateTime bdate;
            if (!DateTime.TryParseExact(birthDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out bdate))
                return "The birth date you have entered is not valid! Please enter another one!";

            if (DateTime.Compare(bdate, DateTime.Now) > 0)
                return "Please enter your real birth date! You cannot be born on " + birthDate + "!";

            if (!zip.Equals(""))
                if (!int.TryParse(zip, out _) && zip.Length != 5)
                    return "The ClientID you have entered is not valid! Please enter another one!";

            if (customerModel.verifyCustomer(id, db))
                return "The ClientID you have entered has already been used! Please enter another one!";

            if (!customerModel.registerCustomer(new Entities.Customer(id, customerName, bdate, zip), db))
                return "A problem has occured when trying to register! Please try again!";

            return "You have been successfully registered!";
        }

        public string update(string customerId, string customerName, string birthDate, string zip)
        {
            if (!int.TryParse(customerId, out int id))
                return "The ClientID you have entered is not valid! Please enter another one!";

            if (!customerModel.verifyCustomer(id, db))
                return
                    "You cannot update your customer profile because it doesn't exist anymore! Please create one to be able to update it!";

            string format = "dd/MM/yyyy";
            DateTime bdate;
            if (!DateTime.TryParseExact(birthDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out bdate))
                return "The birth date you have entered is not valid! Please enter another one!";

            if (DateTime.Compare(bdate, DateTime.Now) > 0)
                return "Please enter your real birth date! You cannot be born on " + birthDate + "!";

            if (!zip.Equals(""))
                if (!int.TryParse(zip, out _) && zip.Length != 5)
                    return "The ClientID you have entered is not valid! Please enter another one!";

            if (customerModel.verifyCustomer(id, db))
                return "The ClientID you have entered has already been used! Please enter another one!";


            if (!customerModel.updateCustomer(new Entities.Customer(id, customerName, bdate, zip), db))
                return "A problem has occured when trying to update your profile! Please try again!";

            return "Your profile have been successfully updated!";
        }

        public List<Customer> list(int orderBy, string ascendent)
        {
            return customerModel.listCustomers(orderBy, ascendent, db);
        }
    }
}