using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Virtual_Store_WebApp.Models;
using Microsoft.AspNetCore.Identity;

namespace Virtual_Store_WebApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration config) => Configuration = config;
        
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:IdentityConnection"]));
            services.AddIdentity<AdminUser, IdentityRole>(opts=>
            {
                opts.User.RequireUniqueEmail = false;
                opts.Password.RequiredLength = 6;
            }).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();
            services.AddMvc(options=>options.EnableEndpointRouting=false);
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IProduct_CategoryRepository, Product_CategoryRepository>();
            //string conString = Configuration["ConnectionStrings:DefaultConnection"];
            //services.AddDbContext<DataContext>(options => options.UseSqlServer(conString));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvcWithDefaultRoute();
            //AppIdentityDbContext.CreateRoles(app.ApplicationServices).Wait(); //run method to seed predefined roles because in this apps user roles are predefined
        }
    }
}
