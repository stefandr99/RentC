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
        public string password { get; set; }
        public int enabled { get; set; }
        public int roleId { get; set; };

        public User() { }

        public User(int userId, string password) {
            this.userId = userId;
            this.password = password;
        }

        public User(int userId, string password, int enabled, int roleId)
        {
            this.userId = userId;
            this.password = password;
            this.enabled = enabled;
            this.roleId = roleId;
        }
    }
}
