using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Virtual_Store_WebApp.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Virtual_Store_WebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        RoleManager<IdentityRole> roleManager;
        UserManager<AdminUser> userManager;
        IProductRepository prepository;
        IOrderRepository orepository;
        IProduct_CategoryRepository pcrepository;
        IWebHostEnvironment environment;

        public DashboardController(RoleManager<IdentityRole> rolemgr, UserManager<AdminUser> usrmgr, IProductRepository prepo, IOrderRepository orepo, IProduct_CategoryRepository pcrepo, IWebHostEnvironment env) 
        {
            roleManager = rolemgr;
            userManager = usrmgr;
            prepository = prepo;
            orepository = orepo;
            pcrepository = pcrepo;
            environment = env;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Welcome admin";
            ViewBag.DashboardCateg = "Welcome to admin";
            return View();
        }

        //Users action methods
        public async Task<IActionResult> AdminUsers()
        {
            List<AdminUser> adminUsers = new List<AdminUser>();

            foreach(AdminUser user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "Admins"))
                {
                    adminUsers.Add(user);
                }
            }
            ViewBag.Title = "Welcome admin";
            ViewBag.DashboardCateg = "Admin Users Table";
            return View(adminUsers);
        }

        [HttpPost]
        public async Task<IActionResult> AddAdminUser(LoginAdminUser user)
        {
            ViewBag.Title = "Welcome admin";
            ViewBag.DashboardCateg = "Admin Users Table";
            if (ModelState.IsValid)
            {
                IdentityResult result = await userManager.CreateAsync(new AdminUser(user.UserName), user.Password);
                if (result.Succeeded)
                {
                    if(await roleManager.RoleExistsAsync("Admins"))
                    {
                        IdentityResult result1 = await userManager.AddToRoleAsync(await userManager.FindByNameAsync(user.UserName), "Admins");
                        if (result1.Succeeded)
                        {
                            return RedirectToAction(nameof(AdminUsers));
                        }
                        else
                        {
                            AddErrorsFromResult(result1);
                            return View("AdminUsers", userManager.Users);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Role", "The role doesn't exists.");
                        return View("AdminUsers", userManager.Users);
                    }
                }
                else
                {
                    AddErrorsFromResult(result);
                    return View("AdminUsers", userManager.Users);
                }
            }
           else return View("AdminUsers", userManager.Users);//need to implement viewbag to transfer already validated fields
        }

        public async Task<IActionResult> UpdateAdminUser(string id)
        {
            AdminUser adminUser = await userManager.FindByIdAsync(id);
            if (adminUser != null)
            {
                return View(new UpdateAdminUser() { Id = adminUser.Id, UserName = adminUser.UserName});
            }
            return RedirectToAction(nameof(AdminUsers));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdminUser(UpdateAdminUser uadminuser)
        {
            if (ModelState.IsValid)
            {
                AdminUser adminUser = await userManager.FindByIdAsync(uadminuser.Id);
                adminUser.UserName = uadminuser.UserName;//will have to optimize so if the user doesn't changes name no update occurs
                IdentityResult result = await userManager.UpdateAsync(adminUser);
                if (result.Succeeded) 
                {
                    if (uadminuser.Password != null)
                    {
                        IdentityResult result1 = await userManager.RemovePasswordAsync(adminUser);
                        if (result1.Succeeded)
                        {
                            IdentityResult result2 = await userManager.AddPasswordAsync(adminUser, uadminuser.Password);
                            return RedirectToAction(nameof(AdminUsers));
                        }
                        else
                        {
                            AddErrorsFromResult(result1);
                        }
                    }
                    else
                    {
                        return RedirectToAction(nameof(AdminUsers));
                    }
                }
                else
                {
                    AddErrorsFromResult(result);
                }
                return View("UpdateAdminUser",uadminuser);
            }
            else return View("UpdateAdminUser", uadminuser);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdminUser(string id)
        {
            //this can be optimised by placing it in every else statement but it cluters code
            ViewBag.Title = "Welcome admin";
            ViewBag.DashboardCateg = "Admin Users Table";
            List<AdminUser> customers = new List<AdminUser>();
            foreach (AdminUser user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "Admins"))
                {
                    customers.Add(user);
                }
            }
            if (ModelState.IsValid)
            {
                AdminUser user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(AdminUsers));
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                        return View("AdminUsers", customers);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User not found");
                    return View("Users", customers);
                }
            }
            else return View("Users", customers);
        }

        //Users action methods
        public async Task<IActionResult> Users()
        {
            ViewBag.Title = "Welcome admin";
            ViewBag.DashboardCateg = "Customer Users Table";
            List<AdminUser> customers = new List<AdminUser>();
            foreach(AdminUser user in userManager.Users)
            {
                if(await userManager.IsInRoleAsync(user,"Users"))
                {
                    customers.Add(user);
                }
            }
            if (ModelState.IsValid)
            {
                return View(customers);
            }
            else
            {
                return View(customers);
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(UpdateUser user)
        {
            ViewBag.Title = "Welcome admin";
            ViewBag.DashboardCateg = "Customer Users Table";
            AdminUser customer = new AdminUser()
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Birthdate = user.Birthdate??DateTime.MinValue,
                Address = user.Address
            };
            if (ModelState.IsValid)
            {
                IdentityResult result = await userManager.CreateAsync(customer, user.Password);
                if (result.Succeeded)
                {
                    if (await roleManager.RoleExistsAsync("Users")) 
                    {
                        IdentityResult result1 = await userManager.AddToRoleAsync(await userManager.FindByNameAsync(customer.UserName), "Users");
                        if (result1.Succeeded)
                        {
                            return RedirectToAction(nameof(Users));
                        }
                        else
                        {
                            AddErrorsFromResult(result1);
                            return View("Users", userManager.Users);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("Role", "The role doesn't exists");
                        return View("Users", userManager.Users);
                    }
                }
                else
                {
                    return View("Users", userManager.Users);
                }
            }
            else
            {
                return View("Users",userManager.Users);
            }
            
        }

        public async Task<IActionResult> UpdateUser(string id)
        {
            AdminUser user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(new UpdateUser2() 
                {
                    Id = user.Id, 
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Birthdate = user.Birthdate,
                    Address = user.Address
                });
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(UpdateUser2 user)
        {

            if (ModelState.IsValid)
            {
                AdminUser adminUser = await userManager.FindByIdAsync(user.Id);
                adminUser.UserName = user.UserName;//will have to optimize so if the user doesn't changes name no update occurs
                IdentityResult result = await userManager.UpdateAsync(adminUser);
                if (result.Succeeded)
                {
                    if (user.Password != null)
                    {
                        IdentityResult result1 = await userManager.RemovePasswordAsync(adminUser);
                        if (result1.Succeeded)
                        {
                            IdentityResult result2 = await userManager.AddPasswordAsync(adminUser, user.Password);
                            if (result2.Succeeded)
                            {
                                return RedirectToAction(nameof(Users));
                            }
                            else
                            {
                                AddErrorsFromResult(result2);
                            }
                        }
                        else
                        {
                            AddErrorsFromResult(result1);
                        }
                    }
                    else
                    {
                        return RedirectToAction(nameof(Users));
                    }
                }
                else
                {
                    AddErrorsFromResult(result);
                }
                return View("UpdateUser", user);
            }
            else return View("UpdateUser", user);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            ViewBag.Title = "Welcome admin";
            ViewBag.DashboardCateg = "Customer Users Table";
            List<AdminUser> customers = new List<AdminUser>();
            foreach (AdminUser user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, "Users"))
                {
                    customers.Add(user);
                }
            }
            if (ModelState.IsValid)
            {
                AdminUser user = await userManager.FindByIdAsync(id);
                if (user != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Users));
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                        return View("Users", customers);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "User not found");
                    return View("Users", customers);
                }
            }
            else return View("Users",customers);
        }

        //Product_Category action methods
        public async Task<IActionResult> Product_Category()
        {
            ViewBag.Title = "Welcome admin";
            ViewBag.DashboardCateg = "Product Category Table";
            return View(await pcrepository.Product_CategoriesAsync());
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct_Category(CreateProduct_Category pcategory)
        {
            if (ModelState.IsValid)
            {
                Product_Category product_Category;
                if (pcategory.ImageForm != null)
                {
                    string filename = Guid.NewGuid().ToString() + pcategory.ImageForm.FileName;
                    string uploadpath = Path.Combine(environment.WebRootPath, "images");
                    string filepath = Path.Combine(uploadpath, filename);

                    using (FileStream stream = new FileStream(filepath, FileMode.Create))
                    {
                        await pcategory.ImageForm.CopyToAsync(stream);
                    }
                    product_Category = new Product_Category()
                    {
                        Id = pcategory.Id,
                        CategoryName = pcategory.CategoryName,
                        Description = pcategory.Description,
                        Image = filename,
                    };
                }
                else
                {
                    product_Category = new Product_Category()
                    {
                        Id = pcategory.Id,
                        CategoryName = pcategory.CategoryName,
                        Description = pcategory.Description,
                        Image = pcategory.Image
                    };
                }
                await pcrepository.AddProduct_CategoryAsync(product_Category);
                return RedirectToAction(nameof(Product_Category));
            }
            else
            {
                ViewBag.DashboardCateg = "Product Category Table";
                return View("Product_Category", await pcrepository.Product_CategoriesAsync());
            }
        }

        public async Task<IActionResult> UpdateProduct_Category(long id)
        {
            ViewBag.DashboardCateg = "Product Category Edit";
            return View(await pcrepository.GetProduct_CategoryAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct_Category(CreateProduct_Category pcategory)
        {
            if (ModelState.IsValid)
            {
                Product_Category product_Category;
                if (pcategory.ImageForm != null)
                {
                    string filename = Guid.NewGuid().ToString() + pcategory.ImageForm.FileName;
                    string uploadpath = Path.Combine(environment.WebRootPath, "images");
                    string filepath = Path.Combine(uploadpath, filename);

                    using (FileStream stream = new FileStream(filepath, FileMode.Create))
                    {
                        await pcategory.ImageForm.CopyToAsync(stream);
                    }
                    product_Category = new Product_Category()
                    {
                        Id = pcategory.Id,
                        CategoryName = pcategory.CategoryName,
                        Description = pcategory.Description,
                        Image = filename,
                    };
                }
                else
                {
                    product_Category = new Product_Category()
                    {
                        Id = pcategory.Id,
                        CategoryName = pcategory.CategoryName,
                        Description = pcategory.Description,
                        Image = pcategory.Image
                    };
                }
                await pcrepository.UpdateProduct_CategoryAsync(product_Category);
                return RedirectToAction(nameof(Product_Category));
            }
            else
            {
                ViewBag.DashboardCateg = "Product Category Edit";
                return View("UpdateProduct_Category", await pcrepository.GetProduct_CategoryAsync(pcategory.Id));
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct_Category(long id)
        {
            await pcrepository.DeleteProduct_CategoryAsync(id);
            return RedirectToAction(nameof(Product_Category));
        }

        //Product action methods
        public async Task<IActionResult> Products()
        {
            ViewBag.Title = "Welcome admin";
            ViewBag.DashboardCateg = "Products Table";
            ViewBag.Product_Categories = await pcrepository.Product_CategoriesAsync();
            return View(await prepository.ProductsAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(CreateProduct cproduct)
        {
            if (ModelState.IsValid)
            {
                string filename = Guid.NewGuid().ToString() + cproduct.ImageForm.FileName;
                string uploadpath = Path.Combine(environment.WebRootPath, "images");
                string filepath = Path.Combine(uploadpath, filename);

                using (FileStream stream = new FileStream(filepath, FileMode.Create))
                {
                    await cproduct.ImageForm.CopyToAsync(stream);
                }

                Product product = new Product()
                {
                    CategoryId = cproduct.CategoryId,
                    ProductName = cproduct.ProductName,
                    Stock = cproduct.Stock,
                    Description = cproduct.Description,
                    Specifications = cproduct.Specifications,
                    Image = filename,
                    Price = cproduct.Price
                };
                await prepository.AddProductAsync(product);
                return RedirectToAction(nameof(Products));
            }
            else 
            {
                ViewBag.Product_Categories = await pcrepository.Product_CategoriesAsync();
                return View("Products",await prepository.ProductsAsync());
            }
        }

        public async Task<IActionResult> UpdateProduct(long id)
        {
            ViewBag.DashboardCateg = "Edit Product";
            ViewBag.Product_Categories = await pcrepository.Product_CategoriesAsync();
            return View(await prepository.GetProductAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(CreateProduct cproduct)
        {
            if (ModelState.IsValid)
            {
                Product product;
                if (cproduct.ImageForm != null)
                {
                    string filename = Guid.NewGuid().ToString() + cproduct.ImageForm.FileName;
                    string uploadpath = Path.Combine(environment.WebRootPath, "images");
                    string filepath = Path.Combine(uploadpath, filename);

                    using (FileStream stream = new FileStream(filepath, FileMode.Create))
                    {
                        cproduct.ImageForm.CopyTo(stream);//maybe implement async copy in future and also image size limit
                    }

                    product = new Product()
                    {
                        Id = cproduct.Id,
                        CategoryId = cproduct.CategoryId,
                        ProductName = cproduct.ProductName,
                        Stock = cproduct.Stock,
                        Description = cproduct.Description,
                        Specifications = cproduct.Specifications,
                        Image = filename,
                        Price = cproduct.Price
                    };
                }
                else
                {
                    product = new Product()
                    {
                        Id = cproduct.Id,
                        CategoryId = cproduct.CategoryId,
                        ProductName = cproduct.ProductName,
                        Stock = cproduct.Stock,
                        Description = cproduct.Description,
                        Specifications = cproduct.Specifications,
                        Image = cproduct.Image,
                        Price = cproduct.Price
                    };
                }

                await prepository.UpdateProductAsync(product);
                return RedirectToAction(nameof(Products));
            }
            else
            {
                ViewBag.Product_Categories = await pcrepository.Product_CategoriesAsync();
                return View(await prepository.GetProductAsync(cproduct.Id));
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(long id)
        {
            await prepository.DeleteProductAsync(id);
            return RedirectToAction(nameof(Products));
        }

        //Orders action methods
        [Authorize]
        public async Task<IActionResult> Orders()
        {
            ViewBag.Title = "Welcome admin";
            ViewBag.DashboardCateg = "Orders Table";
            return View(await orepository.OrdersAsync());
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                AdminUser user = await userManager.FindByIdAsync(order.UserId);
                if (user != null)
                {
                    await orepository.AddOrderAsync(order);
                    return RedirectToAction(nameof(Orders));
                }
                else
                {
                    ModelState.AddModelError("", "The User doesn't exists");
                    return View("Orders",await orepository.OrdersAsync());
                }
            }
            else return View("Orders", await orepository.OrdersAsync());
        }

        public async Task<IActionResult> UpdateOrder(long id)
        {
            ViewBag.DashboardCateg = "Edit Order";
            return View(await orepository.GetOrderAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(Order order)
        {
            if (ModelState.IsValid)
            {
                AdminUser user = await userManager.FindByIdAsync(order.UserId);
                if (user != null)
                {
                    await orepository.UpdateOrderAsync(order);
                    return RedirectToAction(nameof(Orders));
                }
                else
                {
                    ModelState.AddModelError("", "The User doesn't exists");
                    return View("Orders", await orepository.OrdersAsync());
                }
            }
            else return View(await orepository.GetOrderAsync(order.Id));
            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            await orepository.DeleteOrderAsync(id);
            return RedirectToAction(nameof(Orders));
        }
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
