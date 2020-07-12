using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Entities
{
    public class Coupon
    {
        public string couponCode { get; set; }
        public string description { get; set; }
        public decimal discount { get; set; }
    }
}
