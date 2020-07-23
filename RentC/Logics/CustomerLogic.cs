using RentC.Util;
using RentC.Entities;
using RentC.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Logics
{
    public class CustomerLogic
    {
        public DbConnection db { get; }
        public CustomerRepository customerRepository { get; }

        public CustomerLogic()
        {
            db = DbConnection.getInstance;
            customerRepository = new CustomerRepository();
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

            if (customerRepository.register(new Customer(customerName, bdate, zip), db) == 0)
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public Response update(string customerId, string customerName, string birthDate, string zip)
        {
            if (!int.TryParse(customerId, out int id))
                return Response.INCORRECT_ID;

            if (!customerRepository.verifyCustomer(id, db))
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


            if (!customerRepository.update(new Customer(id ,customerName, bdate, zip), db))
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public Response removeCustomer(string customerId) {
            if (!int.TryParse(customerId, out int id))
                return Response.INCORRECT_ID;
            bool result = customerRepository.remove(id, db);

            return (result ? Response.SUCCESS : Response.INEXISTENT_CUSTOMER);
        }

        public List<Customer> list(int orderBy, string ascendent)
        {
            return customerRepository.list(orderBy, ascendent, db);
        }
    }
}