using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace RentC_MVC.Models
{
    public class Customer
    {
        [Display(Name = "Id")]
        public int customerId { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string name { get; set; }
        [Required]
        [Display(Name = "Birth Day")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime birthDate { get; set; }
        [Display(Name = "Location")]
        public string location { get; set; }
        [Display(Name = "ZIP Code")]
        [MaxLength(5)]
        [MinLength(5)]
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