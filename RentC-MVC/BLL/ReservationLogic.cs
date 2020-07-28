using RentC_MVC.Util;
using RentC_MVC.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RentC_MVC.Models;
using RentC_MVC.Repositories;

namespace RentC_MVC.BLL
{
    public class ReservationLogic
    {
        public DbConnection db { get; }
        public ReservationData reservationData { get; }
        public CarData carData { get; }
        public CustomerData customerData { get; }
        public CouponData couponData { get; }

        public ReservationLogic()
        {
            db = DbConnection.getInstance;
            reservationData = new ReservationData(new ReservationRepository());
            carData = new CarData(new CarRepository());
            customerData = new CustomerData(new CustomerRepository());
            couponData = new CouponData();
        }

        public Response register(int carId, int customerId, DateTime startDate, DateTime endDate, string city)
        {
            /*if (!int.TryParse(customerId, out int id))
                return Response.INCORRECT_ID;*/
            if (!carData.verifyExistenceCar(carId, db))
                return Response.INEXISTENT_CAR;

            if (!carData.verifyAvailableCar(carId, db))
                return Response.UNAVAILABLE_CAR;

            if (!carData.verifyCarLocation(carId, city, db))
                return Response.UNAVAILABLE_CAR_IN_CITY;

            if (!customerData.verifyCustomer(customerId, db))
                return Response.INEXISTENT_CUSTOMER;

            /*string format = "dd/MM/yyyy";
            DateTime sDate, eDate;
            if (!DateTime.TryParseExact(startDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out sDate))
                return Response.INVALID_DATE;

            if (!DateTime.TryParseExact(endDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out eDate))
                return Response.INVALID_DATE;*/

            if (DateTime.Compare(startDate, endDate) > 0)
                return Response.INVERSED_DATES;

            if (DateTime.Compare(startDate, DateTime.Now) < 0)
                return Response.INCORRECT_SDATE;

            string coupon = couponData.getCoupon(db);

            if (reservationData.register(
                new Reservation(carId, customerId, startDate, endDate, city, coupon), db) == 0)
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public Response update(int carId, int customerId, DateTime startDate, DateTime endDate) {
            /*if (!int.TryParse(carId, out int id))
                return Response.INCORRECT_ID;*/
            if (!reservationData.verifyReservation(carId, customerId, startDate, endDate, db))
                return Response.INEXISTENT_RESERVATION;

            /*string format = "dd/MM/yyyy";
            DateTime sDate, eDate;
            if (!DateTime.TryParseExact(startDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out sDate))
                return Response.INVALID_DATE;
            if (!DateTime.TryParseExact(endDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out eDate))
                return Response.INVALID_DATE;*/
            if (DateTime.Compare(startDate, endDate) > 0)
                return Response.INVERSED_DATES;
            if (!(DateTime.Compare(startDate, DateTime.Now) <= 0 && DateTime.Compare(endDate, DateTime.Now) > 0))
                return Response.INCORRECT_SDATE;

            if (!reservationData.update(new Reservation(carId, customerId, startDate, endDate), db))
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public List<Reservation> list(int orderBy, string ascendent) {
            return reservationData.list(orderBy, ascendent, db);
        }

        public Reservation findById(int carId, int customerId, DateTime startDate, DateTime endDate)
        {
            return reservationData.findById(carId, customerId, startDate, endDate, db);
        }

        public Response cancelReservation(int carId, int customerId, DateTime startDate, DateTime endDate) {
            /*if (!int.TryParse(identifyId, out int id))
                return Response.INCORRECT_ID;*/

            if (!reservationData.verifyReservation(carId, customerId, startDate, endDate, db))
                return Response.INEXISTENT_RESERVATION;
            if (!reservationData.remove(carId, customerId, startDate, endDate, db))
                return Response.DATABASE_ERROR;
            return Response.SUCCESS;
        }

        public Response expiredReservations() {
            if (!reservationData.expiredReservations(db))
                return Response.DATABASE_ERROR;
            return Response.SUCCESS;
        }

    }
}