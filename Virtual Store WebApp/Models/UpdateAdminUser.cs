using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Virtual_Store_WebApp.Models
{
    public class UpdateAdminUser
    {
        [Required]
        public string Id { get; set; }

        public string UserName { get; set; }

        [UIHint("password")]
        public string Password { get; set; }

    }
}
