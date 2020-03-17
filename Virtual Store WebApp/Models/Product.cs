using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Virtual_Store_WebApp.Models
{
    public class Product
    {
        public long Id { get; set; }
        
        [Required(ErrorMessage ="Please choose a Product Category")]
        public long? CategoryId { get; set; }
        public Product_Category Category { get; set; }

        public IEnumerable<ProductOrderJoin> Order { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required(ErrorMessage ="Please specify the Stock")]
        public long? Stock { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Specifications { get; set; }

        public string Image { get; set; }

        [Required(ErrorMessage ="Please specify the Price")]
        public decimal? Price { get; set; }
    }
}
