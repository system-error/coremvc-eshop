using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using roles.Data;
using roles.Models;

namespace eshop.Controllers
{
    public class AdminController : Controller
    {
        ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
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
    }
}
