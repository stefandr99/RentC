using RentC.Db;
using RentC.Entities;
using RentC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public List<Car> list(int orderBy, string ascendent)
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