using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC_MVC.Models
{
    public class User
    {
        [Display(Name = "Id")]
        public int userId { get; set; }
        [Required]
        [Display(Name = "Password")]
        public string password { get; set; }
        [Display(Name = "Enabled?")]
        public bool enabled { get; set; }
        [Display(Name = "Role Id")]
        public int roleId { get; set; }

        public User() { }

        public User(string password) {
            this.userId = userId;
            this.password = password;
        }

        public User(int userId, string password, bool enabled, int roleId)
        {
            this.userId = userId;
            this.password = password;
            this.enabled = enabled;
            this.roleId = roleId;
        }

        public override string ToString() {
            return "User " + userId + ": " + enabled + "   " + roleId;
        }
    }
}
