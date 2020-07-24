using RentC_MVC.Util;
using RentC_MVC.Models;
using RentC_MVC.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC_MVC.Repositories;

namespace RentC_MVC.BLL
{
    public class CustomerLogic
    {
        public DbConnection db { get; }
        public CustomerData customerData { get; }

        public CustomerLogic()
        {
            db = DbConnection.getInstance;
            customerData = new CustomerData(new CustomerRepository());
        }

        public Response register(string customerName, DateTime birthDate, string zip)
        {
            /*string format = "dd/MM/yyyy";
            DateTime bdate;
            if (!DateTime.TryParseExact(birthDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out bdate))
                return Response.INVALID_DATE;

            if (DateTime.Compare(bdate, DateTime.Now) > 0)
                return Response.IREAL_BIRTH;*/

            if (!String.IsNullOrEmpty(zip))
            {
                if (zip.Length != 5 || !int.TryParse(zip, out _))
                    return Response.INVALID_ZIP;
            }
            else zip = "";

            if (customerData.register(new Customer(customerName, birthDate, zip), db) == 0)
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public Response update(int customerId, string customerName, DateTime birthDate, string zip)
        {
            /*if (!int.TryParse(customerId, out int id))
                return Response.INCORRECT_ID;*/

            if (!customerData.verifyCustomer(customerId, db))
                return Response.INEXISTENT_CUSTOMER;

            /*string format = "dd/MM/yyyy";
            DateTime bdate;
            if (!DateTime.TryParseExact(birthDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out bdate))
                return Response.INVALID_DATE;*/

            if (DateTime.Compare(birthDate, DateTime.Now) > 0)
                return Response.IREAL_BIRTH;

            if (!String.IsNullOrEmpty(zip)) {
                if (zip.Length != 5 || !int.TryParse(zip, out _))
                    return Response.INVALID_ZIP;
            }
            else zip = "";


            if (!customerData.update(new Customer(customerId ,customerName, birthDate, zip), db))
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public Response removeCustomer(int customerId) {
            /*if (!int.TryParse(customerId, out int id))
                return Response.INCORRECT_ID;*/
            bool result = customerData.remove(customerId, db);

            return (result ? Response.SUCCESS : Response.INEXISTENT_CUSTOMER);
        }

        public List<Customer> list(int orderBy, string ascendent)
        {
            return customerData.list(orderBy, ascendent, db);
        }

        public Customer findById(int id)
        {
            return customerData.findById(id, db);
        }
    }
}