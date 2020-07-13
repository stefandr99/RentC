using RentC.Db;
using RentC.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC.Entities;

namespace RentC.Controllers
{
    public class ReservationController
    {
        public DbConnection db { get; }
        public ReservationModel reservationModel { get; }
        public CarModel carModel { get; }
        public CustomerModel customerModel { get; }
        public CouponModel couponModel { get; }

        public ReservationController()
        {
            db = DbConnection.getInstance;
            reservationModel = new ReservationModel();
            carModel = new CarModel();
            customerModel = new CustomerModel();
            couponModel = new CouponModel();
        }

        public string register(string plate, int customerId, string startDate, string endDate, string city)
        {
            if (!carModel.verifyExistenceCar(plate, db))
                return "This car does not exist! Please choose another one!";

            if (!carModel.verifyAvailableCar(plate, db))
                return "This car is not available at the moment! Please choose another one!";

            int carId = carModel.verifyCarLocationAndGetId(plate, city, db);
            if (carId == 0)
                return "This car is not available in " + city + " . Please choose another one!";

            if (!customerModel.verifyCustomer(customerId, db))
                return "The customer with id: " + customerId.ToString() + " does not exist!";

            string format = "dd/MM/yyyy";
            DateTime sDate, eDate;
            if (!DateTime.TryParseExact(startDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out sDate))
                return "The start date you have entered is not valid! Please enter another one!";

            if (!DateTime.TryParseExact(endDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out eDate))
                return "The end date you have entered is not valid! Please enter another one!";

            if (DateTime.Compare(sDate, eDate) > 0)
                return "Start date is later than end date! Please enter valid dates";

            string coupon = couponModel.getCoupon(db);

            if (!reservationModel.registerCarRent(
                new Reservation(carId, customerId, sDate, eDate, city, coupon), db))
                return "A problem has occured when trying to register! Please try again!";

            return "You have been successfully registered!";
        }

        public List<Reservation> list(int orderBy, string ascendent) {
            return reservationModel.listReservations(orderBy, ascendent, db);
        }

    }
}