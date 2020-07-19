using RentC.Util;
using RentC.Entities;
using RentC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RentC.Controllers
{
    public class CarController
    {
        private DbConnection db { get; }
        private CarModel carModel { get; }

        public CarController()
        {
            db = DbConnection.getInstance;
            carModel = new CarModel();
        }

        public Response register(string plate, string manufacturer, string model, string pricePerDay, string city)
        {
            if (plate.Equals("") || manufacturer.Equals("") || model.Equals("") || city.Equals(""))
                return Response.UNFILLED_FIELDS;

            if (!decimal.TryParse(pricePerDay, out decimal price))
                return Response.INCORRECT_PRICE;

            int result = carModel.register(new Car(plate, manufacturer, model, price, city), db);
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

            bool result = carModel.removeCar(id, db);
            return result ? Response.SUCCESS : Response.INEXISTENT_CAR;
        }

        public List<Car> listAvailable(int orderBy, string ascendent)
        {
            return carModel.listAvailableCars(orderBy, ascendent, db);
        }

        public List<Car> listMostRecentCars() {
            return carModel.listMostRecentCars(db);
        }

        public List<Tuple<int, Car>> listMostRentedByMonth(string month, string year)
        {
            if (!int.TryParse(month, out int m))
                return null;
            if (!int.TryParse(year, out int y))
                return null;
            return carModel.listMostRentedCarsByMonth(m, y, db);
        }

        public List<Tuple<int, Car>> listLessRentedByMonth(string month, string year) {
            if (month.Equals("") || year.Equals(""))
                return null;
            if (!int.TryParse(month, out int m))
                return null;
            if (!int.TryParse(year, out int y))
                return null;
            return carModel.listLessRentedCarsByMonth(m, y, db);
        }

    }
}