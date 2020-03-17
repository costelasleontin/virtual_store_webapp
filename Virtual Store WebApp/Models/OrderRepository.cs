using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Virtual_Store_WebApp.Models
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> OrdersAsync();
        Task<Order> GetOrderAsync(long key);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(long key);
    }
    public class OrderRepository:IOrderRepository
    {
        private AppIdentityDbContext context;
        public OrderRepository(AppIdentityDbContext ctx) => context = ctx;

        public async Task<IEnumerable<Order>> OrdersAsync() => await context.Orders.Include(o=>o.Products).ToListAsync();
        public async Task<Order> GetOrderAsync(long key) =>await context.Orders.FindAsync(key);
        public async Task AddOrderAsync(Order order)
        {
            await context.Orders.AddAsync(order);
            await context.SaveChangesAsync();
        }
        public async Task UpdateOrderAsync(Order order)
        {
            Order originalOrder = await context.Orders.FindAsync(order.Id);
            originalOrder.Products = order.Products;
            originalOrder.Status = order.Status;
            originalOrder.Time = order.Time;
            originalOrder.TotalPrice = order.TotalPrice;
            originalOrder.DeliveryAddress = order.DeliveryAddress;
            originalOrder.UserId = order.UserId;
            await context.SaveChangesAsync();
        }
        public async Task DeleteOrderAsync(long id)
        {
            context.Orders.Remove(new Order() { Id=id });
            await context.SaveChangesAsync();
        }
    }
}
