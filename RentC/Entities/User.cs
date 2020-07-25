using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentC.Entities
{
    public class User
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public bool enabled { get; set; }
        public int roleId { get; set; }

        public User() { }

        public User(string username, string password) {
            this.username = username;
            this.password = password;
        }

        public User(int userId, string username, string password, bool enabled, int roleId)
        {
            this.userId = userId;
            this.username = username;
            this.password = password;
            this.enabled = enabled;
            this.roleId = roleId;
        }

        public override string ToString() {
            return "User " + userId + ": " + username + "   enabled: " + (enabled ? "yes" : "no") + " being " + (roleId == 1 ? "administrator" : (roleId == 2 ? "manager" : "salesperson"));
        }
    }
}
