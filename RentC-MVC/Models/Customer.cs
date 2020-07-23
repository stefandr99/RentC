﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC_MVC.Models
{
    public class Customer
    {
        public int customerId { get; set; }
        public string name { get; set; }
        public System.DateTime birthDate { get; set; }
        public string location { get; set; }
        public string ZIPCode { get; set; }

        public Customer()
        {
        }

        public Customer(int customerId, string name, System.DateTime birthDate, string location, string ZIPCode)
        {
            this.customerId = customerId;
            this.name = name;
            this.birthDate = birthDate;
            this.location = location;
            this.ZIPCode = ZIPCode;
        }

        public Customer(int customerId, string name, System.DateTime birthDate, string ZIPCod) {
            this.ZIPCode = ZIPCod;
            this.customerId = customerId;
            this.name = name;
            this.birthDate = birthDate;
        }

        public Customer(string name, System.DateTime birthDate, string ZIPCode)
        {
            this.name = name;
            this.birthDate = birthDate;
            this.ZIPCode = ZIPCode;
        }

        public override string ToString() {
            return "Customer " + customerId + ": " + name + "   birth date:" + birthDate + 
                   (location.Equals("") ?  "" : "   location: " +location) + (ZIPCode.Equals("") ? "" : "   ZIPCode" + ZIPCode);
        }
    }
}