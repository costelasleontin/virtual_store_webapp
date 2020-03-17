using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Virtual_Store_WebApp.Models
{
    public class CreateProduct:Product
    {
        public IFormFile ImageForm { get; set; }
    }
}
