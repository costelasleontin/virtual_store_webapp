using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Virtual_Store_WebApp.Models;

namespace Virtual_Store_WebApp.Controllers
{
    public class HomeController : Controller
    {
        AppIdentityDbContext context;
        public IProduct_CategoryRepository pcrepository;
        public IProductRepository prepository;

        public HomeController(AppIdentityDbContext ctx, IProduct_CategoryRepository pcrepo, IProductRepository prepo) 
        {
            context = ctx;
            pcrepository = pcrepo;
            prepository = prepo;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Welcome to the store";
            return View((await pcrepository.Product_CategoriesAsync(),await prepository.ProductsAsync()));
        }

        public async Task<IActionResult> Products(int? prodcateg, int? pageSize = 10, int? pageNumber = 1)  //defaults for product category, products on page (pageSize) and displayed page (pageNumber)
        {
            ViewBag.Title = "Welcome to the store";
            return View( (await pcrepository.Product_CategoriesAsync(), await prepository.GetProductsAsync(prodcateg,pageSize.Value,pageNumber.Value)));
        }

        public IActionResult Contact()
        {
            ViewBag.Title = "Welcome to the store";
            ViewBag.Heading = "Contact Us";
            return View();
        }

        public IActionResult Shipping()
        {
            ViewBag.Title = "Welcome to the store";
            ViewBag.Heading = "Shipping Terms and Conditions";
            return View();
        }

        public IActionResult TermsOfService()
        {
            ViewBag.Title = "Welcome to the store";
            ViewBag.Heading = "Terms of Service";
            return View();
        }

        public IActionResult PrivacyPolicy()
        {
            ViewBag.Title = "Welcome to the store";
            ViewBag.Heading = "GDPR";
            return View();
        }

        public IActionResult AboutUs()
        {
            ViewBag.Title = "Welcome to the store";
            ViewBag.Heading = "About Us";
            return View();
        }
    }
}
