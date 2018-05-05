using FazalOil.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FazalOil.Controllers
{
    public class DataController : Controller
    {
        FazilOilEntities e = new FazilOilEntities();
        
        [HttpPost]
        public string AddProduct()
        {
            StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);
            string requestFromPost = reader.ReadToEnd();

            Product obj = JsonConvert.DeserializeObject<Product>(requestFromPost);

            e.Products.Add(obj);
            e.SaveChanges();
            return JsonConvert.SerializeObject(new {code=200});
        }

        [HttpGet]
        public string getProducts()
        {
            var list = e.Database.SqlQuery<ProductModel>("SELECT * FROM Products").ToList<ProductModel>();
            return JsonConvert.SerializeObject(list);
        }
	}
}