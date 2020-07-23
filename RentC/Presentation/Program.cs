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
        static void Main(string[] args)
        {
            UserInteraction interaction = new UserInteraction();

            interaction.start();
        }

        
    }
}
