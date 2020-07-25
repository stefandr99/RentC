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

        public Response register(string plate, string manufacturer, string model, decimal pricePerDay, string city)
        {
            if (plate.Equals("") || manufacturer.Equals("") || model.Equals("") || city.Equals(""))
                return Response.UNFILLED_FIELDS;

            /*if (!decimal.TryParse(pricePerDay, out decimal price))
                return Response.INCORRECT_PRICE;*/

            int result = carData.register(new Car(plate, manufacturer, model, pricePerDay, city), db);
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

        public Response remove(int carId)
        {
            /*if (!int.TryParse(carId, out int id))
                return Response.INCORRECT_ID;*/

            bool result = carData.remove(carId, db);
            return result ? Response.SUCCESS : Response.INEXISTENT_CAR;
        }

        public List<Car> listAvailable(int orderBy, string ascendent)
        {
            return carData.list(orderBy, ascendent, db);
        }

        public Response update(int id, string plate, string manufacturer, string model, decimal pricePerDay, string city) {
            if(!carData.verifyExistenceCar(id, db))
                return Response.INEXISTENT_CAR;

            if (!carData.update(new Car(id, plate, manufacturer, model, pricePerDay, city), db))
                return Response.DATABASE_ERROR;

            return Response.SUCCESS;
        }

        public Car findById(int id) {
            return carData.findById(id, db);
        }

        public List<Car> listMostRecentCars() {
            return carData.listMostRecentCars(db);
        }

        public List<Tuple<int, Car>> listMostRentedByMonth(int month, int year)
        {
            /*if (!int.TryParse(month, out int m))
                return null;
            if (!int.TryParse(year, out int y))
                return null;*/
            return carData.listMostRentedCarsByMonth(month, year, db);
        }

        public List<Tuple<int, Car>> listLessRentedByMonth(int month, int year) {
            /*if (month.Equals("") || year.Equals(""))
                return null;
            if (!int.TryParse(month, out int m))
                return null;
            if (!int.TryParse(year, out int y))
                return null;*/
            return carData.listLessRentedCarsByMonth(month, year, db);
        }

    }
}