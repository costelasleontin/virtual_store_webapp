using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Virtual_Store_WebApp.Models
{
    public class Product_Category
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "Please specify a Product Category Name")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Please specify a Product Category Description")]
        public string Description { get; set; }

        public string Image { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}
