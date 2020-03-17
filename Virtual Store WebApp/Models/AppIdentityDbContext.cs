using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Virtual_Store_WebApp.Models
{
    public class AppIdentityDbContext:IdentityDbContext<AdminUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options, IServiceProvider service=null) : base(options) {  }


        //dbsets for creating store data tables
        public DbSet<Product> Products { get; set; }
        public DbSet<Product_Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }


        ////method runed for seeding database with roles and admin user
        //public static async Task CreateRoles(IServiceProvider service)
        //{
        //    if (service != null)
        //    {
        //        RoleManager<IdentityRole> roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();

        //        bool adminroleexists = await roleManager.RoleExistsAsync("Admins");

        //        if (!adminroleexists)
        //        {
        //            IdentityRole identityRole = new IdentityRole() { Name = "Admins" };
        //            IdentityResult result = await roleManager.CreateAsync(identityRole);

        //            if (result.Succeeded) { Console.WriteLine("Succeded adding Admins Role"); }
        //        }
        //        bool userroleexists = await roleManager.RoleExistsAsync("Users");
        //        if (!userroleexists)
        //        {
        //            IdentityRole identityRole = new IdentityRole() { Name = "Users" };
        //            IdentityResult result1 = await roleManager.CreateAsync(identityRole);
        //            if (result1.Succeeded) { Console.WriteLine("Succeded adding Users Role"); }
        //        }
        //        UserManager<AdminUser> userManager = service.GetRequiredService<UserManager<AdminUser>>();
        //        IdentityResult result2 = await userManager.CreateAsync(new AdminUser() { UserName = "costelasleontin" }, "Secret123$"); //this is just for development seeding, in production it gets removed
        //        if (result2.Succeeded)
        //        {
        //            Console.WriteLine("Succeded seeding admin");
        //            IdentityResult result3 = await userManager.AddToRoleAsync(await userManager.FindByNameAsync("costelasleontin"), "Admins");
        //            if (result3.Succeeded) { Console.WriteLine("Succeded adding admin to role"); }
        //        }
        //    }
        //}
    }
}
