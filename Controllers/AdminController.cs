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
        public IActionResult UpdateProduct(long id, string productName, string description, string productPrice, int productStock, IFormFile productImage)
        {

            string path = Path.Combine(this._environment.WebRootPath, "uploads/images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // return Content($"{id} {productName}");
            Product product = _db.Products.Find(id);
            product.Name = productName;
            product.Description = description;
            product.Stock = productStock;
            product.Price = decimal.Parse(productPrice.Replace(".",","));
            if (productImage != null)
            {
                
                string fileName = Path.GetFileName(productImage.FileName);
                using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                {
                    productImage.CopyTo(stream);
                    product.Image = fileName;
                }
                product.Image = product.Image;
            }   
            _db.SaveChanges();
            return RedirectToAction("Products", "Admin");
        }
    }
}
