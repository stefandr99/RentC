using RentC.Db;
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
    class CarController
    {
        public DbConnection db { get; }
        public CarModel carModel;

        public CarController()
        {
            db = DbConnection.getInstance;
            carModel = new CarModel();
        }

        public string register(string plate, string manufacturer, string model, decimal pricePerDay, string city)
        {
            if (plate.Equals("") || manufacturer.Equals("") || model.Equals("") || city.Equals(""))
                return "Please fill al fields!";
            int result = carModel.register(new Car(plate, manufacturer, model, pricePerDay, city), db);
            switch (result)
            {
                case 0:
                    return "A problem has occured! Please try again!";
                case 1:
                    return "Registration with success!";
                case 2:
                    return "A car with this plate exists already in this city!";
                default: return "";
            }
        }

        public string remove(int carId)
        {
            int result = carModel.removeCar(carId, db);
            switch (result)
            {
                case 1:
                    return "The car was removed with success!";
                case 2:
                    return "You don't have the permission to do this!";
                case 3:
                    return "This car doesn't exist!";
                default:
                    return "";
            }
        }

        public List<Car> listAvailable(int orderBy, string ascendent)
        {
            return carModel.listAvailableCars(orderBy, ascendent, db);
        }

        public List<Car> listMostRecentCars() {
            return carModel.listMostRecentCars(db);
        }

        public List<Tuple<int, Car>> listMostRentedByMonth(int month, int year)
        {
            return carModel.listMostRentedCarsByMonth(month, year, db);
        }

        public List<Tuple<int, Car>> listLessRentedByMonth(int month, int year) {
            return carModel.listLessRentedCarsByMonth(month, year, db);
        }
    }
}