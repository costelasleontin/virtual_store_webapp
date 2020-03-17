using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Virtual_Store_WebApp.Models
{
    public class ProductOrderJoin
    {
        public long Id { get; set; }

        public long? ProductId { get; set; }
        public Product Product { get; set; }

        public long OrderId { get; set; }
        public Order Order { get; set; }
    }
}
