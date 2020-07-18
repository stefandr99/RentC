using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC.Controllers;
using RentC.Entities;

namespace RentC.Util
{
    public class Program
    {
        public Controller controller = new Controller();
        public Response response { get; set; }

        static void Main(string[] args)
        {
            Controller controller = new Controller();

            Console.WriteLine("Welcome to RentC, your brand new solution to manage and control your company's data" +
                              "without missing anything.");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press ENTER to continue or ESC to quit.");

            ConsoleKeyInfo keyInfo;
            while (true) {
                keyInfo = Console.ReadKey();
                if(keyInfo.Key == ConsoleKey.Enter) 
                    break;
                if(keyInfo.Key == ConsoleKey.Escape)
                    System.Environment.Exit(0);
            }


            Console.ReadKey();
        }

        public void auth() {
            while (true) {
                
                if (response == Response.SUCCESS_ADMIN)
                    adminSession();
                else if (response == Response.SUCCESS_MANAGER)
                    managerSession();
                else if (response == Response.SUCCESS_SALESPERSON)
                    salesPersonSession();
                else getMessage(response);
            }
        }

        

        
    }
}
