using RentC.Util;
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

        public Response register(string plate, string customerId, string startDate, string endDate, string city)
        {
            if (!int.TryParse(customerId, out int id))
                return Response.INCORRECT_ID;
            if (!carModel.verifyExistenceCar(plate, db))
                return Response.INEXISTENT_CAR;

            if (!carModel.verifyAvailableCar(plate, db))
                return Response.UNAVAILABLE_CAR;

            int carId = carModel.verifyCarLocationAndGetId(plate, city, db);
            if (carId == 0)
                return Response.UNAVAILABLE_CAR_IN_CITY;

            if (!customerModel.verifyCustomer(id, db))
                return Response.INEXISTENT_CUSTOMER;

            string format = "dd/MM/yyyy";
            DateTime sDate, eDate;
            if (!DateTime.TryParseExact(startDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out sDate))
                return Response.INVALID_DATE;

            if (!DateTime.TryParseExact(endDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out eDate))
                return Response.INVALID_DATE;

            if (DateTime.Compare(sDate, eDate) > 0)
                return Response.INVERSED_DATES;

            if (DateTime.Compare(sDate, DateTime.Now) > 0)
                return Response.INCORRECT_SDATE;

            string coupon = couponModel.getCoupon(db);

            if (!reservationModel.registerCarRent(
                new Reservation(carId, id, sDate, eDate, city, coupon), db))
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public Response update(string carId, string startDate, string endDate) {
            if (!int.TryParse(carId, out int id))
                return Response.INCORRECT_ID;
            if (!reservationModel.verifyReservation(id, db))
                return Response.INEXISTENT_RESERVATION;

            string format = "dd/MM/yyyy";
            DateTime sDate, eDate;
            if (!DateTime.TryParseExact(startDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out sDate))
                return Response.INVALID_DATE;
            if (!DateTime.TryParseExact(endDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out eDate))
                return Response.INVALID_DATE;
            if (DateTime.Compare(sDate, eDate) > 0)
                return Response.INVERSED_DATES;
            if (DateTime.Compare(sDate, DateTime.Now) > 0)
                return Response.INCORRECT_SDATE;

            if (!reservationModel.update(new Reservation(id, sDate, eDate), db))
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public List<Reservation> list(int orderBy, string ascendent) {
            return reservationModel.listReservations(orderBy, ascendent, db);
        }

        public Response cancelReservation(string identifyId) {
            if (!int.TryParse(identifyId, out int id))
                return Response.INCORRECT_ID;

            if (!reservationModel.verifyReservation(id, db))
                return Response.INEXISTENT_RESERVATION;
            if (!reservationModel.cancelReservation(id, db))
                return Response.DATABASE_ERROR;
            return Response.SUCCESS;
        }

        public Response expiredReservations() {
            if (!reservationModel.expiredReservations(db))
                return Response.DATABASE_ERROR;
            return Response.SUCCESS;
        }
    }
}