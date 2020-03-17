using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Virtual_Store_WebApp.Models;

namespace Virtual_Store_WebApp.Models
{
    public class UpdateUser2:IdentityUser
    {
        public UpdateUser2() : base() { }
        public UpdateUser2(string username) : base(username) { }

        public string Password { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please specify a valid birth date")]
        public DateTime? Birthdate { get; set; }

        [Required]
        public string Address { get; set; }

        public IEnumerable<Order> Orders { get; set; }
    }
}
