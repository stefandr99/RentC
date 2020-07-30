using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RentC_MVC.BLL;
using RentC_MVC.Util;

namespace RentCTest
{
    [TestClass]
    public class UnitTest1
    {
        Logic logic = new Logic();


        [TestMethod]
        public void carRegister()
        {
            Assert.AreEqual(Response.SUCCESS, logic.car.register("RJ 65 KRP", "Skoda", "Ocatavia", (decimal)228.84, "Bucharest"));
            Assert.AreEqual(Response.SUCCESS, logic.car.register("BV 33 OPP", "VW", "Passat", (decimal)291.02, "Brasov"));
        }
        
        [TestMethod]
        public void removeCar() {
            Assert.AreEqual(Response.SUCCESS, logic.car.remove(34));
            Assert.AreEqual(Response.SUCCESS, logic.car.remove(18));
        }


        [TestMethod]
        public void removeCustomer()
        {
            Assert.AreEqual(Response.SUCCESS, logic.customer.removeCustomer(10));
            Assert.AreEqual(Response.SUCCESS, logic.customer.removeCustomer(152));
            Assert.AreEqual(Response.SUCCESS, logic.customer.removeCustomer(537));
        }

        [TestMethod]
        public void registerCustomer()
        {
            Assert.AreEqual(Response.SUCCESS, logic.customer.register("Walter White", DateTime.Parse("17/03/1960"), "64873"));
        }

        [TestMethod]
        public void registerReservation()
        {
            Assert.AreEqual(Response.SUCCESS, logic.reservation.register(13, 99, DateTime.Parse("01/08/2020"), DateTime.Parse("04/08/2020"), "Porto Alegre"));
            Assert.AreEqual(Response.SUCCESS, logic.reservation.register(42, 60, DateTime.Parse("13/08/2020"), DateTime.Parse("14/08/2020"), "Rio de Janeiro"));
            Assert.AreEqual(Response.SUCCESS, logic.reservation.register(17, 123, DateTime.Parse("30/09/2020"), DateTime.Parse("02/10/2020"), "Chernivtsi"));
            Assert.AreEqual(Response.SUCCESS, logic.reservation.register(18, 512, DateTime.Parse("01/09/2020"), DateTime.Parse("16/12/2020"), "Cluj-Napoja"));

        }

        [TestMethod]
        public void authUser()
        {
            Assert.AreEqual(1, logic.user.authUser("admin", "pass"));
            Assert.AreEqual(3, logic.user.authUser("stefan", "0000"));
        }

        /*[TestMethod]
        public void changePassword()
        {
            Assert.AreEqual(Response.SUCCESS, logic.user.changePassword(3, "0000", "1111", "1111"));
        }*/
        
    }
}
