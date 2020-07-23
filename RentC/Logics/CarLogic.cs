using RentC.Util;
using RentC.Entities;
using RentC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RentC.Logics
{
    public class CarLogic
    {
        private DbConnection db { get; }
        private CarRepository carRepository { get; }

        public CarLogic()
        {
            db = DbConnection.getInstance;
            carRepository = new CarRepository();
        }

        public Response register(string plate, string manufacturer, string model, string pricePerDay, string city)
        {
            if (plate.Equals("") || manufacturer.Equals("") || model.Equals("") || city.Equals(""))
                return Response.UNFILLED_FIELDS;

            if (!decimal.TryParse(pricePerDay, out decimal price))
                return Response.INCORRECT_PRICE;

            int result = carRepository.register(new Car(plate, manufacturer, model, price, city), db);
            switch (result)
            {
                case 0:
                    return Response.DATABASE_ERROR;
                case 1:
                    return Response.SUCCESS;
                case 2:
                    return Response.ALREADY_CAR;
                default: return Response.DATABASE_ERROR;
            }
        }

        public Response remove(string carId)
        {
            if (!int.TryParse(carId, out int id))
                return Response.INCORRECT_ID;

            bool result = carRepository.remove(id, db);
            return result ? Response.SUCCESS : Response.INEXISTENT_CAR;
        }

        public List<Car> listAvailable(int orderBy, string ascendent)
        {
            return carRepository.list(orderBy, ascendent, db);
        }

        public List<Car> listMostRecentCars() {
            return carRepository.listMostRecentCars(db);
        }

        public List<Tuple<int, Car>> listMostRentedByMonth(string month, string year)
        {
            if (!int.TryParse(month, out int m))
                return null;
            if (!int.TryParse(year, out int y))
                return null;
            return carRepository.listMostRentedCarsByMonth(m, y, db);
        }

        public List<Tuple<int, Car>> listLessRentedByMonth(string month, string year) {
            if (month.Equals("") || year.Equals(""))
                return null;
            if (!int.TryParse(month, out int m))
                return null;
            if (!int.TryParse(year, out int y))
                return null;
            return carRepository.listLessRentedCarsByMonth(m, y, db);
        }

    }
}