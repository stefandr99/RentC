using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentC.Entities;

namespace RentC.Presentation
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
