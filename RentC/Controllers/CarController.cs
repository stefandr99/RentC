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
        public List<Car> list()
        {
            return carModel.listAvailableCars(db);
        }

    }
}
