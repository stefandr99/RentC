using RentC.Util;
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

        public Response register(string customerName, string birthDate, string zip)
        {
            string format = "dd/MM/yyyy";
            DateTime bdate;
            if (!DateTime.TryParseExact(birthDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out bdate))
                return Response.INVALID_DATE;

            if (DateTime.Compare(bdate, DateTime.Now) > 0)
                return Response.IREAL_BIRTH;

            if (!zip.Equals(""))
                if (zip.Length != 5 || !int.TryParse(zip, out _))
                    return Response.INVALID_ZIP;

            if (!customerModel.registerCustomer(new Customer(customerName, bdate, zip), db))
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public Response update(string customerId, string customerName, string birthDate, string zip)
        {
            if (!int.TryParse(customerId, out int id))
                return Response.INCORRECT_ID;

            if (!customerModel.verifyCustomer(id, db))
                return Response.INEXISTENT_CUSTOMER;

            string format = "dd/MM/yyyy";
            DateTime bdate;
            if (!DateTime.TryParseExact(birthDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out bdate))
                return Response.INVALID_DATE;

            if (DateTime.Compare(bdate, DateTime.Now) > 0)
                return Response.IREAL_BIRTH;

            if (!zip.Equals(""))
                if (zip.Length != 5 || !int.TryParse(zip, out _))
                    return Response.INVALID_ZIP;


            if (!customerModel.updateCustomer(new Customer(id ,customerName, bdate, zip), db))
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public Response removeCustomer(string customerId) {
            if (!int.TryParse(customerId, out int id))
                return Response.INCORRECT_ID;
            bool result = customerModel.removeCustomer(id, db);

            return (result ? Response.SUCCESS : Response.INEXISTENT_CUSTOMER);
        }

        public List<Customer> list(int orderBy, string ascendent)
        {
            return customerModel.listCustomers(orderBy, ascendent, db);
        }
    }
}