using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Controllers
{
    public class Controller
    {
        public CarController car { get; }
        public CustomerController customer { get; }
        public ReservationController reservation { get; }
        public UserController user { get; }

        public Controller() {
            car = new CarController();
            customer = new CustomerController();
            reservation = new ReservationController();
            user = new UserController();
        }
    }
}
