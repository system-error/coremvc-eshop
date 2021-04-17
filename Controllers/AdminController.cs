using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using roles.Data;
using roles.Models;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace eshop.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext _db;
        private IWebHostEnvironment _environment;

        public AdminController(ApplicationDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Products()
        {
            List<Product> products = _db.Products.Include(c => c.Category).ToList();
            ViewBag.products = products;
            return View();
        }

        public IActionResult Product(long id)
        {
            Product product = _db.Products.Find(id);
            ViewBag.product = product;
            return View();
        }

        [HttpPost]
        public IActionResult UpdateProduct(long id, string productName, string description, string productPrice, int productStock)
        {

            Product product = _db.Products.Find(id);
            product.Name = productName;
            product.Description = description;
            product.Stock = productStock;
            product.Price = decimal.Parse(productPrice.Replace(".",","));
            
            _db.SaveChanges();
            return RedirectToAction("Products", "Admin");
        }

        [HttpPost]
        public IActionResult AddImage(long id, IFormFile productImage)
        {
            Product product = _db.Products.Find(id);
            string path = Path.Combine(this._environment.WebRootPath, "uploads/images/p/"+id);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if ((productImage.ContentType.ToLower().Equals("image/jpg") || productImage.ContentType.ToLower()
                                                                           .Equals("image/jpeg")
                                                                       || productImage.ContentType.ToLower()
                                                                           .Equals("image/jpeg")
                                                                       || productImage.ContentType.ToLower()
                                                                           .Equals("image/pjpeg")
                                                                       || productImage.ContentType.ToLower()
                                                                           .Equals("image/gif")
                                                                       || productImage.ContentType.ToLower()
                                                                           .Equals("image/x-png")
                                                                       || productImage.ContentType.ToLower()
                                                                           .Equals("image/png")) && productImage != null)
            {
                
               

                    string fileName = Path.GetFileName(productImage.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        productImage.CopyTo(stream);
                        product.Image = fileName;
                    }
            }
            else
            {
                product.Image = "";
            }

            _db.SaveChanges();
            return RedirectToAction("Product", "Admin", new { id });

        }



        public IActionResult AddProduct()
        {
            List<Category> categories = _db.Categories.ToList();
            ViewBag.categories = categories;

            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(string productName, string description, string productPrice, int productStock,int productCategory, IFormFile productImage)
        {
            Product prod = _db.Products.Last();
            string path = Path.Combine(this._environment.WebRootPath, "uploads/images/p/"+prod.Id+1);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Product product = new Product();
            product.Name = productName;
            product.Description = description;
            product.Stock = productStock;
            
            product.Category = _db.Categories.Find(productCategory);
            product.Price = decimal.Parse(productPrice.Replace(".", ","));
            if ((productImage.ContentType.ToLower().Equals("image/jpg") || productImage.ContentType.ToLower()
                                                                           .Equals("image/jpeg")
                                                                       || productImage.ContentType.ToLower()
                                                                           .Equals("image/jpeg")
                                                                       || productImage.ContentType.ToLower()
                                                                           .Equals("image/pjpeg")
                                                                       || productImage.ContentType.ToLower()
                                                                           .Equals("image/gif")
                                                                       || productImage.ContentType.ToLower()
                                                                           .Equals("image/x-png")
                                                                       || productImage.ContentType.ToLower()
                                                                           .Equals("image/png"))&& productImage != null)
            {
                

                    string fileName = Path.GetFileName(productImage.FileName);
                    using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        productImage.CopyTo(stream);
                        product.Image = fileName;
                    }
            }
            else
            {
                product.Image = "";
            }

            _db.Products.Add(product);
            _db.SaveChanges();
            return RedirectToAction("Products", "Admin");
        }

        


    }
}
