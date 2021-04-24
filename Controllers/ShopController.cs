using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using roles.Data;
using roles.Models;

namespace eshop.Controllers
{
    public class ShopController : Controller
    {
        ApplicationDbContext _db;
        

        public ShopController(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }

        public IActionResult Index()
        {
            List<Product> products = _db.Products.Include(c => c.Category).ToList();
            ViewBag.products = products;
            
            return View();
        }

        public IActionResult AddToCart(long id)
        {

            //TO-DO find a way for quantity
            var cartSession = HttpContext.Session.GetString("cart");
            List<long> cartIdList = new List<long>();
            if (cartSession != null)
            {
                cartIdList = JsonConvert.DeserializeObject<List<long>>(cartSession);
            }
            cartIdList.Add(id);
            cartSession = JsonConvert.SerializeObject(cartIdList);
            HttpContext.Session.SetString("cart", cartSession);
            ViewBag.cartSession = cartIdList;
            return View();

        }
        public IActionResult Cart()
        {
            //TO-DO find a way for quantity
            var cartSession = HttpContext.Session.GetString("cart");
            List<Product> products = new List<Product>();
            List<long> cartIdList = new List<long>();
            if (cartSession != null)
            {
                cartIdList = JsonConvert.DeserializeObject<List<long>>(cartSession);
            }

            foreach (var productId in cartIdList)
            {
                products.Add(_db.Products.Include(c =>c.Category).SingleOrDefault(c =>c.Id == productId));
            }

            ViewBag.empty = cartIdList.Count() == 0;
            ViewBag.cartSession = cartIdList;
            ViewBag.products = products;
            return View();
        }
    }
}
