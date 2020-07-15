using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Entities
{
    public class Car
    {
        public int carId { get; set; }
        public string plate { get; set; }
        public string manufacturer { get; set; }
        public string model { get; set; }
        public decimal pricePerDay { get; set; }
        public string city { get; set; }

        public Car(int carId, string plate, string manufacturer, string model, decimal pricePerDay, string city)
        {
            this.carId = carId;
            this.plate = plate;
            this.manufacturer = manufacturer;
            this.model = model;
            this.pricePerDay = pricePerDay;
            this.city = city;
        }

        public Car(string plate, string manufacturer, string model, decimal pricePerDay, string city)
        {
            this.plate = plate;
            this.manufacturer = manufacturer;
            this.model = model;
            this.pricePerDay = pricePerDay;
            this.city = city;
        }

        public override string ToString() {
            return carId + "   " + plate + "   " + manufacturer + "   " + model + "   " + pricePerDay + "   " + city + Environment.NewLine;
        }
    }
}
