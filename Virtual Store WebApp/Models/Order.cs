using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Virtual_Store_WebApp.Models
{
    public class Order
    {
        public long Id { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please select a valid Username")]
        public string UserId { get; set; }

        //[Required] will be implemented after I implement products selection/search menu
        public IEnumerable<ProductOrderJoin> Products { get; set; }

        [UIHint("datetime")]
        public DateTime? Time { get; set; } //buying time and date

        [Required]
        public string DeliveryAddress { get; set; }

        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Select an item form the list.")]
        public OrderStatus Status { get; set; }

        [Required]
        public decimal? TotalPrice { get; set; }//will implement a calculation formula based on selected products

    }

    public enum OrderStatus
    {
        InProcessing = 0,
        InTransit = 1,
        Delivered = 2,
    }
}
