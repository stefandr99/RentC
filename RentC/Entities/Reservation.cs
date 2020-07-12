using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Entities
{
    public class Reservation
    {
        public int carId { get; set; }
        public int customerId { get; set; }
        public byte reservStatsId { get; set; }
        public System.DateTime startDate { get; set; }
        public System.DateTime endDate { get; set; }
        public string location { get; set; }
        public string couponCode { get; set; }

        public Reservation() { }
        public Reservation(int carId, int customerId, System.DateTime startDate, System.DateTime endDate, string location, string couponCode)
        {
            this.carId = carId;
            this.customerId = customerId;
            this.reservStatsId = reservStatsId;
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
    }
}
