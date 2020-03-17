using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Virtual_Store_WebApp.Models
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> ProductsAsync();
        Task<IEnumerable<Product>> GetProductsAsync(int? categ, int pageSize, int pageNumber);
        Task<Product> GetProductAsync(long key);
        Task AddProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(long key);
    }
    public class ProductRepository:IProductRepository
    {
        private AppIdentityDbContext context;
        public ProductRepository(AppIdentityDbContext ctx) => context = ctx;

        public async Task<IEnumerable<Product>> ProductsAsync() =>await context.Products.Include(p => p.Category).Include(p => p.Order).ToListAsync(); 
        public async Task<IEnumerable<Product>> GetProductsAsync(int? categ, int pageSize, int pageNumber)
        {
            if (categ.HasValue)
            {
                return await context.Products.Include(p => p.Category).Include(p => p.Order).Where(p => p.CategoryId == categ).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            else return await context.Products.Include(p => p.Category).Include(p => p.Order).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<Product> GetProductAsync(long key) => await context.Products.FindAsync(key);
        public async Task AddProductAsync(Product product)
        {
            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();
        }
        public async Task UpdateProductAsync(Product product)
        {
            Product originalProd = await context.Products.FindAsync(product.Id);
            originalProd.CategoryId = product.CategoryId;
            originalProd.Description = product.Description;
            originalProd.Image = product.Image;
            originalProd.Price = product.Price;
            originalProd.ProductName = product.ProductName;
            originalProd.Specifications = product.Specifications;
            originalProd.Stock = product.Stock;
            await context.SaveChangesAsync();
        }
        public async Task DeleteProductAsync(long id)
        {
            context.Products.Remove(new Product() {Id = id });
            await context.SaveChangesAsync();
        }
    }
}
