using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC_MVC.Models
{
    public class Reservation
    {
        [Required]
        [Display(Name = "Car Id")]
        public int carId { get; set; }
        [Required]
        [Display(Name = "Customer Id")]
        public int customerId { get; set; }
        [Display(Name = "Reservations Stats Id")]
        public byte reservStatsId { get; set; }
        [Required]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime startDate { get; set; }
        [Required]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public System.DateTime endDate { get; set; }
        [Required]
        [Display(Name = "Location")]
        public string location { get; set; }
        [Display(Name = "Coupon Code")]
        public string couponCode { get; set; }

        public Reservation() { }
        public Reservation(int carId, int customerId, System.DateTime startDate, System.DateTime endDate, string location, string couponCode)
        {
            this.carId = carId;
            this.customerId = customerId;
            this.startDate = startDate;
            this.endDate = endDate;
            this.location = location;
            this.couponCode = couponCode;
        }

        public Reservation(int carId, int customerId, System.DateTime startDate, System.DateTime endDate, string location)
        {
            this.carId = carId;
            this.customerId = customerId;
            this.startDate = startDate;
            this.endDate = endDate;
            this.location = location;
        }

        public Reservation(System.DateTime startDate, System.DateTime endDate)
        {
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public Reservation(int carId, int customerId, byte reservationStats, System.DateTime startDate, System.DateTime endDate, string location)
        {
            this.carId = carId;
            this.customerId = customerId;
            this.reservStatsId = reservationStats;
            this.startDate = startDate;
            this.endDate = endDate;
            this.location = location;
        }

        public Reservation(int carId, int customerId, System.DateTime startDate, System.DateTime endDate) {
            this.carId = carId;
            this.customerId = customerId;
            this.startDate = startDate;
            this.endDate = endDate;
        }

        public override string ToString() {
            return "Car " + carId + "   customer " + customerId + "   start date: " + startDate + "   end date:" + endDate + "   city: " + location;
        }
    }
}
