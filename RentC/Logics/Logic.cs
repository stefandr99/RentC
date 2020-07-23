using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Logics
{
    public class Logic
    {
        public CarLogic car { get; }
        public CustomerLogic customer { get; }
        public ReservationLogic reservation { get; }
        public UserLogic user { get; }

        public Logic() {
            car = new CarLogic();
            customer = new CustomerLogic();
            reservation = new ReservationLogic();
            user = new UserLogic();
        }
    }
}
