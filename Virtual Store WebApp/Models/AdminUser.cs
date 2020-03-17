using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Virtual_Store_WebApp.Models
{
    public class AdminUser:IdentityUser
    {
        public AdminUser() : base() { }
        public AdminUser(string username) : base(username) { }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthdate { get; set; }

        public string Address { get; set; }

    }
}
