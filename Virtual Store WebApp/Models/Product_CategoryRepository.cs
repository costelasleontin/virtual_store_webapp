using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Virtual_Store_WebApp.Models
{
    public interface IProduct_CategoryRepository
    {
        Task<IEnumerable<Product_Category>> Product_CategoriesAsync();
        Task<Product_Category> GetProduct_CategoryAsync(long key);
        Task AddProduct_CategoryAsync(Product_Category pcategory);
        Task UpdateProduct_CategoryAsync(Product_Category pcategory);
        Task DeleteProduct_CategoryAsync(long id);
    }
    public class Product_CategoryRepository:IProduct_CategoryRepository
    {
        private AppIdentityDbContext context;
        public Product_CategoryRepository(AppIdentityDbContext ctx) => context = ctx;
        public async Task<IEnumerable<Product_Category>> Product_CategoriesAsync() => await context.Categories.Include(pc=>pc.Products).ToArrayAsync();
        public async Task<Product_Category> GetProduct_CategoryAsync(long key) => await context.Categories.FindAsync(key);
        public async Task AddProduct_CategoryAsync(Product_Category pcategory)
        {
            await context.Categories.AddAsync(pcategory);
            await context.SaveChangesAsync();
        }
        public async Task UpdateProduct_CategoryAsync(Product_Category pcategory)
        {
            Product_Category originalProdCateg = await context.Categories.FindAsync(pcategory.Id);
            originalProdCateg.CategoryName = pcategory.CategoryName;
            originalProdCateg.Description = pcategory.Description;
            originalProdCateg.Image = pcategory.Image;
            await context.SaveChangesAsync(); //using change detection (ef state tracking)  to update database = less trafic but more database processing
        }
        public async Task DeleteProduct_CategoryAsync(long id)
        {
            context.Categories.Remove(new Product_Category() { Id = id });
            await context.SaveChangesAsync();
        }
    }
}
