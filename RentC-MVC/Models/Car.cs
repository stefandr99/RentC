using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC_MVC.Models
{
    public class Car
    {
        [Required]
        [Display(Name = "Id")]
        public int carId { get; set; }
        [Required]
        [Display(Name = "Plate")]
        public string plate { get; set; }
        [Required]
        [Display(Name = "Manufacturer")]
        public string manufacturer { get; set; }
        [Required]
        [Display(Name = "Model")]
        public string model { get; set; }
        [Required]
        [Display(Name = "Price Per Day")]
        public decimal pricePerDay { get; set; }
        [Required]
        [Display(Name = "City")]
        public string city { get; set; }

        public Car() { }

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
            return "Car " + carId + "   plate: " + plate + "   manufacturer: " + manufacturer + "   model: " + model + "   price per day: " + pricePerDay + "   city: " + city;
        }
    }
}
