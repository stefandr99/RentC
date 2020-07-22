using RentC.Util;
using RentC.Repositories;
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
        public ReservationRepository reservationRepository { get; }
        public CarRepository carRepository { get; }
        public CustomerRepository customerRepository { get; }
        public CouponRepository couponRepository { get; }

        public ReservationController()
        {
            db = DbConnection.getInstance;
            reservationRepository = new ReservationRepository();
            carRepository = new CarRepository();
            customerRepository = new CustomerRepository();
            couponRepository = new CouponRepository();
        }

        public Response register(string plate, string customerId, string startDate, string endDate, string city)
        {
            if (!int.TryParse(customerId, out int id))
                return Response.INCORRECT_ID;
            if (!carRepository.verifyExistenceCar(plate, db))
                return Response.INEXISTENT_CAR;

            if (!carRepository.verifyAvailableCar(plate, db))
                return Response.UNAVAILABLE_CAR;

            int carId = carRepository.verifyCarLocationAndGetId(plate, city, db);
            if (carId == 0)
                return Response.UNAVAILABLE_CAR_IN_CITY;

            if (!customerRepository.verifyCustomer(id, db))
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

            string coupon = couponRepository.getCoupon(db);

            if (reservationRepository.register(
                new Reservation(carId, id, sDate, eDate, city, coupon), db) == 0)
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public Response update(string carId, string startDate, string endDate) {
            if (!int.TryParse(carId, out int id))
                return Response.INCORRECT_ID;
            if (!reservationRepository.verifyReservation(id, db))
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

            if (!reservationRepository.update(new Reservation(id, sDate, eDate), db))
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public List<Reservation> list(int orderBy, string ascendent) {
            return reservationRepository.list(orderBy, ascendent, db);
        }

        public Response cancelReservation(string identifyId) {
            if (!int.TryParse(identifyId, out int id))
                return Response.INCORRECT_ID;

            if (!reservationRepository.verifyReservation(id, db))
                return Response.INEXISTENT_RESERVATION;
            if (!reservationRepository.remove(id, db))
                return Response.DATABASE_ERROR;
            return Response.SUCCESS;
        }

        public Response expiredReservations() {
            if (!reservationRepository.expiredReservations(db))
                return Response.DATABASE_ERROR;
            return Response.SUCCESS;
        }

        public List<Tuple<int, Customer>> goldCustomers()
        {
            return reservationRepository.goldCustomers(db);
        }

        public List<Tuple<int, Customer>> silverCustomers() {
            return reservationRepository.silverCustomers(db);
        }
    }
}