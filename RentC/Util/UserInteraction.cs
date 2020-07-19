using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RentC.Controllers;
using RentC.Entities;

namespace RentC.Util
{
    public class UserInteraction
    {
        private Controller controller { get; }
        private Response response { get; set; }
        private int previous = 0;
        private int current = 0;
        private string order;
        public UserInteraction() {
            controller = new Controller();
        }

        public void start() {
            Console.WriteLine("Welcome to RentC, your brand new solution to manage and control your company's data " +
                              "without missing anything.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("\t\t\t\tPress ENTER to continue or ESC to quit.");

            ConsoleKeyInfo keyInfo;
            while (true)
            {
                keyInfo = Console.ReadKey();
                if (keyInfo.Key == ConsoleKey.Enter)
                    break;
                if (keyInfo.Key == ConsoleKey.Escape)
                    System.Environment.Exit(0);
            }

            authUser();
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

                if (response != Response.SUCCESS_ADMIN && response != Response.SUCCESS_MANAGER &&
                    response != Response.SUCCESS_SALESPERSON) {
                    getMessage(response);
                }
                else break;

                Thread.Sleep(300);
            }
            if (response == Response.SUCCESS_ADMIN)
                adminSession();
            else if (response == Response.SUCCESS_MANAGER)
                managerSession();
            else if (response == Response.SUCCESS_SALESPERSON)
                salespersonSession();
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
            Console.Write("Password: ");
            string pass = Console.ReadLine();

            response = controller.user.registerSaleperson(pass);
            if (response == Response.SUCCESS)
                Console.WriteLine("Salesperson registered with success.");
            else getMessage(response);

            Thread.Sleep(300);
        }

        public void enableUser()
        {
            Console.Write("User Id: ");
            string id = Console.ReadLine();

            response = controller.user.enableUser(id);
            if (response == Response.SUCCESS)
                Console.WriteLine("User enabled with success.");
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

        public void printList<T>(List<T> list) where T: class {
            foreach (var t in list) {
                Console.WriteLine(t.ToString());
            }

            try {
                PropertyInfo[] info;
                Type type = list[0].GetType();
                info = type.GetProperties();
                Console.WriteLine("Sort: ");
                int i;
                for (i = 0; i < info.Length; i++) {
                    Console.WriteLine(i + ". " + info[i].Name);
                }

                Console.WriteLine(i + ". Menu");

                string r = Console.ReadLine();
                int.TryParse(r, out int answer);
                if (answer > info.Length || answer < 0) {
                    Console.WriteLine("This hasn't been an option.");
                }
                else if (answer < info.Length) {
                    if(type.FullName.Contains("Customer"))
                        printList(controller.customer.list(answer + 1, order));
                    else if(type.FullName.Contains("Car")) {
                        printList(controller.car.listAvailable(answer + 1, order));
                    }
                    else if (type.FullName.Contains("Reservation")) {
                        printList(controller.reservation.list(answer + 1, order));
                    }
                    else {
                        printList(controller.user.list(answer + 1, order));
                    }
                }

            }
            catch (SecurityException) {
                Console.WriteLine("A problem has occured! Please try again!");
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
                Console.WriteLine("\n");
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
                                  "15. Disable User" + Environment.NewLine + "16. Enable User" + Environment.NewLine + 
                                  "17. Exit" + Environment.NewLine);
                Console.Write("Answer: ");
                string respLine = Console.ReadLine();
                switch (respLine) {
                    case "1": {
                        current = 1;
                        registerCustomer();
                        break;
                    }
                    case "2": {
                        current = 2;
                        updateCustomer();
                        break;
                    }
                    case "3": {
                        current = 3;
                        removeCustomer();
                        break;
                    }
                    case "4": {
                        current = 4;
                        registerReservation();
                        break;
                    }
                    case "5": {
                        current = 5;
                        updateReservation();
                        break;
                    }
                    case "6": {
                        current = 6;
                        cancelReservation();
                        break;
                    }
                    case "7": {
                        current = 7;
                        registerCar();
                        break;
                    }
                    case "8": {
                        current = 8;
                        removeCar();
                        break;
                    }
                    case "9": {
                        if (current != 0)
                            previous = current;
                        current = 9;
                        order = current == previous ? "DESC" : "ASC";
                        printList(controller.customer.list(1, order));
                        break;
                    }
                    case "10": {
                        if (current != 0)
                            previous = current;
                        current = 10;
                        order = current == previous ? "DESC" : "ASC";
                        printList(controller.reservation.list(1, order));
                        break;
                    }
                    case "11": {
                        if (current != 0)
                            previous = current;
                        current = 11;
                        order = current == previous ? "DESC" : "ASC";
                        printList(controller.car.listAvailable(1, order));
                        break;
                    }
                    case "12":
                    {
                        if (current != 0)
                            previous = current;
                        current = 12;
                        order = current == previous ? "DESC" : "ASC";
                        printList(controller.user.list(1, order));
                        break;
                    }
                    case "13": {
                        current = 13;
                        changePassword();
                        break;
                    }
                    case "14": {
                        current = 14;
                        registerSaleperson();
                        break;
                    }
                    case "15": {
                        current = 15;
                        disableUser();
                        break;
                    }
                    case "16": {
                        current = 16;
                        enableUser();
                        break;
                    }
                    case "17": {
                        System.Environment.Exit(0);
                        break;
                    }
                    default: {
                        current = 0;
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
                Console.WriteLine("\n");
                Console.WriteLine("1. Register a new customer" + Environment.NewLine + "2. Update customer" +
                                  Environment.NewLine +
                                  "3. Remove customer" + Environment.NewLine + "4. Register a new reservation" +
                                  Environment.NewLine + "5. Edit reservation" + Environment.NewLine +
                                  "6. Cancel reservation" + Environment.NewLine + "7. Register a new car" +
                                  Environment.NewLine +
                                  "8. Remove car" + Environment.NewLine + "9. List customers" + Environment.NewLine +
                                  "10. List reservations" + Environment.NewLine + "11. List cars" + Environment.NewLine +
                                  "12. Change password" + Environment.NewLine + "13. Exit" + Environment.NewLine);
                Console.Write("Answer: ");
                string respLine = Console.ReadLine();
                switch (respLine)
                {
                    case "1": {
                        current = 1;
                        registerCustomer();
                        break;
                    }
                    case "2": {
                        current = 2;
                        updateCustomer();
                        break;
                    }
                    case "3": {
                        current = 3;
                        removeCustomer();
                        break;
                    }
                    case "4": {
                        current = 4;
                        registerReservation();
                        break;
                    }
                    case "5": {
                        current = 5;
                        updateReservation();
                        break;
                    }
                    case "6": {
                        current = 6;
                        cancelReservation();
                        break;
                    }
                    case "7": {
                        current = 7;
                        registerCar();
                        break;
                    }
                    case "8": {
                        current = 8;
                        removeCar();
                        break;
                    }
                    case "9": {
                        if (current != 0)
                            previous = current;
                        current = 9;
                        order = current == previous ? "DESC" : "ASC";
                        printList(controller.customer.list(1, order));
                        break;
                    }
                    case "10": {
                        if (current != 0)
                            previous = current;
                        current = 10;
                        order = current == previous ? "DESC" : "ASC";
                        printList(controller.reservation.list(1, order));
                        break;
                    }
                    case "11": {
                        if (current != 0)
                            previous = current;
                        current = 11;
                        order = current == previous ? "DESC" : "ASC";
                        printList(controller.car.listAvailable(1, order));
                        break;
                    }
                    case "12": {
                        current = 12;
                        changePassword();
                        break;
                    }
                    case "13": {
                        System.Environment.Exit(0);
                        break;
                    }
                    default: {
                        current = 0;
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
                Console.WriteLine("\n");
                Console.WriteLine("1. Register a new reservation" + Environment.NewLine + "2. Edit reservation" +
                                  "3. List reservations" + Environment.NewLine + "4. Change password" + Environment.NewLine + 
                                      "5. Exit" + Environment.NewLine);
                Console.Write("Answer: ");
                string respLine = Console.ReadLine();
                switch (respLine)
                {
                    case "1": {
                        current = 1;
                        registerReservation();
                        break;
                    }
                    case "2": {
                        current = 2;
                        updateReservation();
                        break;
                    }
                    case "3": {
                        if (current != 0)
                            previous = current;
                        current = 3;
                        order = current == previous ? "DESC" : "ASC";
                        printList(controller.reservation.list(1, order));
                        break;
                    }
                    case "4": {
                        current = 4;
                        changePassword();
                        break;
                    }
                    case "5": {
                        System.Environment.Exit(0);
                        break;
                    }
                    default: {
                        current = 0;
                        Console.WriteLine("This is not a valid option.");
                        break;
                    }
                }
            }
        }

        public void getMessage(Response response)
        {
            switch (response)
            {
                case Response.DATABASE_ERROR:
                    {Console.WriteLine("A problem has occured! Please try again!"); break;}
                case Response.ALREADY_CAR:
                    {Console.WriteLine("This car already exists in this city."); break;}
                case Response.UNFILLED_FIELDS:
                    {Console.WriteLine("Please fill all fields!"); break;}
                case Response.INCORRECT_CREDENTIALS:
                    {Console.WriteLine("Username or password incorrect. Please try again!"); break;}
                case Response.INCORRECT_OLD_PASS:
                    {Console.WriteLine("You have entered a wrong old password. Please try again!"); break;}
                case Response.INVALID_DATE:
                    {Console.WriteLine("The date is in invalid format. Please enter a valid one!"); break;}
                case Response.INVALID_ZIP:
                    {Console.WriteLine("You have entered an invalid zip code."); break;}
                case Response.INVERSED_DATES:
                    {Console.WriteLine("Start Date cannot be later than End Date"); break;}
                case Response.IREAL_BIRTH:
                    {Console.WriteLine("You cannot be born on this day. Please enter a real one!"); break;}
                case Response.INEXISTENT_CAR:
                    {Console.WriteLine("This car does not exist."); break;}
                case Response.INEXISTENT_CUSTOMER:
                    {Console.WriteLine("This customer does not exist."); break;}
                case Response.INEXISTENT_RESERVATION:
                    {Console.WriteLine("This reservation does not exist or it is not open."); break;}
                case Response.UNAVAILABLE_CAR:
                    {Console.WriteLine("This car is not available at the moment."); break;}
                case Response.UNAVAILABLE_CAR_IN_CITY:
                    {Console.WriteLine("This car is not available in this city"); break;}
                case Response.INCORRECT_ID:
                    {Console.WriteLine("This id is not correct. Please try again!"); break;}
                case Response.NOT_MATCH_PASS:
                    {Console.WriteLine("Passwords do not match."); break;}
                case Response.INCORRECT_PRICE:
                    {Console.WriteLine("The price has incorrect format. Please write a correct one!"); break;}
                case Response.INCORRECT_SDATE:
                    {Console.WriteLine("Start date cannot be earlier than present. Please enter a correct date"); break;}
                default:
                    {Console.WriteLine(""); break;}
            }
        }
    }
}
