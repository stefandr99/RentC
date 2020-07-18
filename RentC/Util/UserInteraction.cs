using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RentC.Controllers;
using RentC.Entities;

namespace RentC.Util
{
    public class UserInteraction<T>
    {
        private Controller controller { get; }
        private Response response { get; set; }
        public UserInteraction() {
            controller = new Controller();
        }

        public void registerCustomer() {
            Console.Write("Client Name: ");
            string name = Console.ReadLine();
            Console.Write("Birth Date: ");
            string birth = Console.ReadLine();
            Console.Write("ZIP Code: ");
            string zip = Console.ReadLine();
            
            response = controller.customer.register(name, birth, zip);
            if (response == Response.SUCCESS)
                Console.WriteLine("Customer registered with success.");
            else getMessage(response);

            Thread.Sleep(300);
        }

        public void updateCustomer()
        {
            Console.Write("Client Id: ");
            string id = Console.ReadLine();
            Console.Write("Client name: ");
            string name = Console.ReadLine();
            Console.Write("Birth Date: ");
            string birth = Console.ReadLine();
            Console.Write("ZIP Code: ");
            string zip = Console.ReadLine();

            response = controller.customer.update(id, name, birth, zip);
            if (response == Response.SUCCESS)
                Console.WriteLine("Customer updated with success.");
            else getMessage(response);

            Thread.Sleep(300);
        }

        public void removeCustomer()
        {
            Console.Write("Client Id: ");
            string id = Console.ReadLine();

            response = controller.customer.removeCustomer(id);
            if (response == Response.SUCCESS)
                Console.WriteLine("Customer removed with success.");
            else getMessage(response);

            Thread.Sleep(300);
        }

        public void registerCar()
        {
            Console.Write("Plate: ");
            string plate = Console.ReadLine();
            Console.Write("Manufacturer: ");
            string manufac = Console.ReadLine();
            Console.Write("Model: ");
            string model = Console.ReadLine();
            Console.Write("Price Per Day: ");
            string price = Console.ReadLine();
            Console.Write("City: ");
            string city = Console.ReadLine();

            response = controller.car.register(plate, manufac, model, price, city);
            if (response == Response.SUCCESS)
                Console.WriteLine("Car registered with success.");
            else getMessage(response);

            Thread.Sleep(300);
        }

        public void removeCar()
        {
            Console.Write("Car Id: ");
            string id = Console.ReadLine();

            response = controller.car.remove(id);
            if (response == Response.SUCCESS)
                Console.WriteLine("Car removed with success.");
            else getMessage(response);

            Thread.Sleep(300);
        }

        public void registerReservation()
        {
            Console.Write("Plate: ");
            string plate = Console.ReadLine();
            Console.Write("Customer Id: ");
            string id = Console.ReadLine();
            Console.Write("Start Date: ");
            string start = Console.ReadLine();
            Console.Write("End Date: ");
            string end = Console.ReadLine();
            Console.Write("City: ");
            string city = Console.ReadLine();

            response = controller.reservation.register(plate, id, start, end, city);
            if (response == Response.SUCCESS)
                Console.WriteLine("Reservation registered with success.");
            else getMessage(response);

            Thread.Sleep(300);
        }

        public void updateReservation()
        {
            Console.Write("Car Id: ");
            string id = Console.ReadLine();
            Console.Write("Start Date: ");
            string start = Console.ReadLine();
            Console.Write("End Date: ");
            string end = Console.ReadLine();

            response = controller.reservation.update(id, start, end);
            if (response == Response.SUCCESS)
                Console.WriteLine("Reservation updated with success.");
            else getMessage(response);

            Thread.Sleep(300);
        }

        public void cancelReservation() {
            Console.Write("Car Id OR Customer Id: ");
            string id = Console.ReadLine();

            response = controller.reservation.cancelReservation(id);
            if (response == Response.SUCCESS)
                Console.WriteLine("Reservation canceled with success.");
            else getMessage(response);

            Thread.Sleep(300);
        }

        public void authUser() {
            while (true) {
                Console.Write("User Id: ");
                string userId = Console.ReadLine();
                Console.Write("Password: ");
                string pass = Console.ReadLine();

                response = controller.user.authUser(userId, pass);
                if (response == Response.SUCCESS_ADMIN)
                    adminSession();
                else if (response == Response.SUCCESS_MANAGER)
                    managerSession();
                else if (response == Response.SUCCESS_SALESPERSON)
                    salespersonSession();
                else getMessage(response);

                Thread.Sleep(300);
            }
        }

        public void changePassword()
        {
            Console.Write("User Id: ");
            string userId = Console.ReadLine();
            Console.Write("Old Password: ");
            string oldPass = Console.ReadLine();
            Console.Write("New Password: ");
            string newPass1 = Console.ReadLine();
            Console.Write("Confirm New Password: ");
            string newPass2 = Console.ReadLine();

            response = controller.user.changePassword(userId, oldPass, newPass1, newPass2);
            if (response == Response.SUCCESS)
                Console.WriteLine("Password changed with success.");
            else getMessage(response);

            Thread.Sleep(300);
        }

        public void registerSaleperson()
        {
            Console.Write("Salesperson Id: ");
            string id = Console.ReadLine();
            Console.Write("Password: ");
            string pass = Console.ReadLine();

            response = controller.user.registerSaleperson(id, pass);
            if (response == Response.SUCCESS)
                Console.WriteLine("Salesperson registered with success.");
            else getMessage(response);

            Thread.Sleep(300);
        }

        public void disableUser()
        {
            Console.Write("User Id: ");
            string id = Console.ReadLine();

            response = controller.user.disableUser(id);
            if (response == Response.SUCCESS)
                Console.WriteLine("User disabled with success.");
            else getMessage(response);

            Thread.Sleep(300);
        }

        public void printList(List<T> list) {
            foreach (var t in list) {
                Console.WriteLine(t.ToString());
            }
        }

        public void printSpecialStats(List<Tuple<int, Car>> cars)
        {
            foreach (var carIterator in cars)
            {
                Console.WriteLine("Rented: " + carIterator.Item1 + " times, car description: " + carIterator.Item2.ToString());
            }
        }

        public void adminSession()
        {
            while (true) {
                Console.WriteLine("1. Register a new customer" + Environment.NewLine + "2. Update customer" +
                                  Environment.NewLine +
                                  "3. Remove customer" + Environment.NewLine + "4. Register a new reservation" +
                                  Environment.NewLine + "5. Edit reservation" + Environment.NewLine +
                                  "6. Cancel reservation" + Environment.NewLine + "7. Register a new car" +
                                  Environment.NewLine +
                                  "8. Remove car" + Environment.NewLine + "9. List customers" + Environment.NewLine +
                                  "10. List reservations" + Environment.NewLine + "11. List cars" + Environment.NewLine +
                                  "12. List users" + Environment.NewLine + "13. Change password" + Environment.NewLine + 
                                  "14. Register salesperson" + Environment.NewLine +
                                  "15. Disable User" + Environment.NewLine + "16. Exit" + Environment.NewLine);
                string respLine = Console.ReadLine();
                switch (respLine) {
                    case "1": {
                        registerCustomer();
                        break;
                    }
                    case "2": {
                        updateCustomer();
                        break;
                    }
                    case "3": {
                        removeCustomer();
                        break;
                    }
                    case "4": {
                        registerReservation();
                        break;
                    }
                    case "5": {
                        updateReservation();
                        break;
                    }
                    case "6": {
                        cancelReservation();
                        break;
                    }
                    case "7": {
                        registerCar();
                        break;
                    }
                    case "8": {
                        removeCar();
                        break;
                    }
                    case "9": {
                        printList(controller.customer.list(1, "ASC"));
                        break;
                    }
                    case "10": {
                        printList(controller.reservation.list(1, "ASC"));
                        break;
                    }
                    case "11": {
                        printList(controller.car.listAvailable(1, "ASC"));
                        break;
                    }
                    case "12":
                    {
                        printList(controller.user.list(1, "ASC"));
                        break;
                    }
                    case "13": {
                        changePassword();
                        break;
                    }
                    case "14": {
                        registerSaleperson();
                        break;
                    }
                    case "15": {
                        disableUser();
                        break;
                    }
                    case "16": {
                        System.Environment.Exit(0);
                        break;
                    }
                    default: {
                        Console.WriteLine("This is not a valid option.");
                        break;
                    }
                }
            }
        }

        public void managerSession()
        {
            while (true)
            {
                Console.WriteLine("1. Register a new customer" + Environment.NewLine + "2. Update customer" +
                                  Environment.NewLine +
                                  "3. Remove customer" + Environment.NewLine + "4. Register a new reservation" +
                                  Environment.NewLine + "5. Edit reservation" + Environment.NewLine +
                                  "6. Cancel reservation" + Environment.NewLine + "7. Register a new car" +
                                  Environment.NewLine +
                                  "8. Remove car" + Environment.NewLine + "9. List customers" + Environment.NewLine +
                                  "10. List reservations" + Environment.NewLine + "11. List cars" + Environment.NewLine +
                                  "12. Change password" + Environment.NewLine + "13. Exit" + Environment.NewLine);
                string respLine = Console.ReadLine();
                switch (respLine)
                {
                    case "1": {
                        registerCustomer();
                        break;
                    }
                    case "2": {
                        updateCustomer();
                        break;
                    }
                    case "3": {
                        removeCustomer();
                        break;
                    }
                    case "4": {
                        registerReservation();
                        break;
                    }
                    case "5": {
                        updateReservation();
                        break;
                    }
                    case "6": {
                        cancelReservation();
                        break;
                    }
                    case "7": {
                        registerCar();
                        break;
                    }
                    case "8": {
                        removeCar();
                        break;
                    }
                    case "9": {
                        printList(controller.customer.list(1, "ASC"));
                        break;
                    }
                    case "10": {
                        printList(controller.reservation.list(1, "ASC"));
                        break;
                    }
                    case "11": {
                        printList(controller.car.listAvailable(1, "ASC"));
                        break;
                    }
                    case "12": {
                        changePassword();
                        break;
                    }
                    case "13": {
                        System.Environment.Exit(0);
                        break;
                    }
                    default: {
                        Console.WriteLine("This is not a valid option.");
                        break;
                    }
                }
            }
        }

        public void salespersonSession()
        {
            while (true)
            {
                Console.WriteLine("1. Register a new reservation" + Environment.NewLine + "2. Edit reservation" +
                                  "3. List reservations" + Environment.NewLine + "4. Change password" + Environment.NewLine + 
                                      "5. Exit" + Environment.NewLine);
                string respLine = Console.ReadLine();
                switch (respLine)
                {
                    case "1": {
                        registerReservation();
                        break;
                    }
                    case "2": {
                        updateReservation();
                        break;
                    }
                    case "3": {
                        printList(controller.reservation.list(1, "ASC"));
                        break;
                    }
                    case "4": {
                        changePassword();
                        break;
                    }
                    case "5": {
                        System.Environment.Exit(0);
                        break;
                    }
                    default: {
                        Console.WriteLine("This is not a valid option.");
                        break;
                    }
                }
            }
        }

        public string getMessage(Response response)
        {
            switch (response)
            {
                case Response.DATABASE_ERROR:
                    return "A problem has occured! Please try again!";
                case Response.ALREADY_CAR:
                    return "This car already exists in this city.";
                case Response.UNFILLED_FIELDS:
                    return "Please fill all fields!";
                case Response.INCORRECT_CREDENTIALS:
                    return "Username or password incorrect. Please try again!";
                case Response.INCORRECT_OLD_PASS:
                    return "You have entered a wrong old password. Please try again!";
                case Response.INVALID_DATE:
                    return "The date is in invalid format. Please enter a valid one!";
                case Response.INVALID_ZIP:
                    return "You have entered an invalid zip code.";
                case Response.INVERSED_DATES:
                    return "Start Date cannot be later than End Date";
                case Response.IREAL_BIRTH:
                    return "You cannot be born on this day. Please enter a real one!";
                case Response.INEXISTENT_CAR:
                    return "This car does not exist.";
                case Response.INEXISTENT_CUSTOMER:
                    return "This customer does not exist.";
                case Response.INEXISTENT_RESERVATION:
                    return "This reservation does not exist or it is not open.";
                case Response.UNAVAILABLE_CAR:
                    return "This car is not available at the moment.";
                case Response.UNAVAILABLE_CAR_IN_CITY:
                    return "This car is not available in this city";
                case Response.INCORRECT_ID:
                    return "This id is not correct. Please try again!";
                case Response.NOT_MATCH_PASS:
                    return "Passwords do not match.";
                case Response.INCORRECT_PRICE:
                    return "The price has incorrect format. Please write a correct one!";
                case Response.INCORRECT_SDATE:
                    return "Start date cannot be earlier than present. Please enter a correct date";
                default:
                    return "";
            }
        }
    }
}
