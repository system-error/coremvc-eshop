using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using roles.Data;
using roles.Models;

namespace eshop.dbSeeder
{
    public class DataSeeder
    {

        public static async Task seedData(ApplicationDbContext context)
        {
            if (!context.Products.Any())
            {

                string categoriesJSON = System.IO.File.ReadAllText("dummyData/categoryData.json");
                string productsJSON = System.IO.File.ReadAllText("dummyData/productData.json");

                List<Category> categoriesList = JsonConvert.DeserializeObject<List<Category>>(categoriesJSON);
                List<Product> productsList = JsonConvert.DeserializeObject<List<Product>>(productsJSON);
                await context.Categories.AddRangeAsync(categoriesList);
                await context.SaveChangesAsync();


                foreach (var product in productsList)
                {
                    product.Category = context.Categories.Find(product.Category.Id);
                }
                await context.Products.AddRangeAsync(productsList);
                await context.SaveChangesAsync();
            }
            
        }

    }
}
