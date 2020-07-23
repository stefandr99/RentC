using RentC_MVC.Util;
using RentC_MVC.Models;
using RentC_MVC.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RentC_MVC.Repositories;

namespace RentC_MVC.BLL
{
    public class CarLogic
    {
        private DbConnection db { get; }
        private CarData carData { get; }

        public CarLogic()
        {
            db = DbConnection.getInstance;
            carData = new CarData(new CarRepository());
        }

        public Response register(string plate, string manufacturer, string model, string pricePerDay, string city)
        {
            if (plate.Equals("") || manufacturer.Equals("") || model.Equals("") || city.Equals(""))
                return Response.UNFILLED_FIELDS;

            if (!decimal.TryParse(pricePerDay, out decimal price))
                return Response.INCORRECT_PRICE;

            int result = carData.register(new Car(plate, manufacturer, model, price, city), db);
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

            bool result = carData.remove(id, db);
            return result ? Response.SUCCESS : Response.INEXISTENT_CAR;
        }

        public List<Car> listAvailable(int orderBy, string ascendent)
        {
            return carData.list(orderBy, ascendent, db);
        }

        public List<Car> listMostRecentCars() {
            return carData.listMostRecentCars(db);
        }

        public List<Tuple<int, Car>> listMostRentedByMonth(string month, string year)
        {
            if (!int.TryParse(month, out int m))
                return null;
            if (!int.TryParse(year, out int y))
                return null;
            return carData.listMostRentedCarsByMonth(m, y, db);
        }

        public List<Tuple<int, Car>> listLessRentedByMonth(string month, string year) {
            if (month.Equals("") || year.Equals(""))
                return null;
            if (!int.TryParse(month, out int m))
                return null;
            if (!int.TryParse(year, out int y))
                return null;
            return carData.listLessRentedCarsByMonth(m, y, db);
        }

    }
}