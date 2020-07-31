using RentC.Util;
using RentC.DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC.Entities;

namespace RentC.BLL
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
            reservationData = new ReservationData();
            carData = new CarData();
            customerData = new CustomerData();
            couponData = new CouponData();
        }

        public Response register(string plate, string customerId, string startDate, string endDate, string city)
        {
            if (!int.TryParse(customerId, out int id))
                return Response.INCORRECT_ID;
            if (!carData.verifyExistenceCar(plate, db))
                return Response.INEXISTENT_CAR;

            if (!carData.verifyAvailableCar(plate, db))
                return Response.UNAVAILABLE_CAR;

            int carId = carData.verifyCarLocationAndGetId(plate, city, db);
            if (carId == 0)
                return Response.UNAVAILABLE_CAR_IN_CITY;

            if (!customerData.verifyCustomer(id, db))
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

            if (DateTime.Compare(sDate, DateTime.Now) < 0)
                return Response.INCORRECT_SDATE;

            string coupon = couponData.getCoupon(db);

            if (reservationData.register(
                new Reservation(carId, id, sDate, eDate, city, coupon), db) == 0)
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public Response update(string carId, string startDate, string endDate) {
            if (!int.TryParse(carId, out int id))
                return Response.INCORRECT_ID;
            if (!reservationData.verifyReservation(id, db))
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

            if (!reservationData.update(new Reservation(id, sDate, eDate), db))
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public List<Reservation> list(int orderBy, string ascendent) {
            return reservationData.list(orderBy, ascendent, db);
        }

        public Response cancelReservation(string carId, string customerId, string startDate, string endDate) {
            if (!int.TryParse(carId, out int car))
                return Response.INCORRECT_ID;
            if (!int.TryParse(customerId, out int customer))
                return Response.INCORRECT_ID;

            string format = "dd/MM/yyyy";
            DateTime sDate, eDate;
            if (!DateTime.TryParseExact(startDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out sDate))
                return Response.INVALID_DATE;
            if (!DateTime.TryParseExact(endDate, format, new CultureInfo("en-US"),
                DateTimeStyles.None, out eDate))
                return Response.INVALID_DATE;

            if (!reservationData.verifyReservation2(car, customer, sDate, eDate, db))
                return Response.INEXISTENT_RESERVATION;
            if (!reservationData.remove(car, customer, sDate, eDate, db))
                return Response.DATABASE_ERROR;
            return Response.SUCCESS;
        }

        public Response expiredReservations() {
            if (!reservationData.expiredReservations(db))
                return Response.DATABASE_ERROR;
            return Response.SUCCESS;
        }

        public List<Tuple<int, Customer>> goldCustomers()
        {
            return reservationData.goldCustomers(db);
        }

        public List<Tuple<int, Customer>> silverCustomers() {
            return reservationData.silverCustomers(db);
        }
    }
}