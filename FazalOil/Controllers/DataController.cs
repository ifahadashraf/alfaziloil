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

            if (obj.ProductID == -1)
            {
                e.Products.Add(obj);
                e.SaveChanges();
            }
            else
            {
                var toUpdate = e.Products.Where(x => x.ProductID == obj.ProductID).ToList<Product>()[0];
                
                toUpdate.PurchasingPrice = (toUpdate.PurchasingPrice + obj.PurchasingPrice) / 2;
                toUpdate.ProductName = obj.ProductName;
                toUpdate.DateOfPurchase = obj.DateOfPurchase;
                toUpdate.DropoffID = obj.DropoffID;
                toUpdate.SupplierID = obj.SupplierID;
                toUpdate.CategoryID = obj.CategoryID;

                if (toUpdate.CategoryID == 3)
                {
                    toUpdate.TotalQuantity = obj.TotalQuantity;
                }
                else if (toUpdate.CategoryID == 2)
                {
                    toUpdate.TotalLiters = obj.TotalLiters;
                }
                else
                {
                    toUpdate.TotalLiters = obj.TotalLiters;
                    toUpdate.CanSize = obj.CanSize;
                    toUpdate.TotalCans = obj.TotalCans;
                }
                
                e.SaveChanges();
                
            }

            
            return JsonConvert.SerializeObject(new {code=200});
        }

        [HttpGet]
        public string GetProducts()
        {
            var list = e.Database.SqlQuery<Product>("SELECT * FROM Products").ToList<Product>();
            return JsonConvert.SerializeObject(list);
        }

        [HttpGet]
        public string GetCategories()
        {
            var list = e.Database.SqlQuery<Category>("SELECT * FROM Categories").ToList<Category>();
            return JsonConvert.SerializeObject(list);
        }

        [HttpGet]
        public string GetBrands()
        {
            var list = e.Database.SqlQuery<Brand>("SELECT * FROM Brands").ToList<Brand>();
            return JsonConvert.SerializeObject(list);
        }

        [HttpGet]
        public string GetSuppliers()
        {
            var list = e.Database.SqlQuery<Supplier>("SELECT * FROM Suppliers").ToList<Supplier>();
            return JsonConvert.SerializeObject(list);
        }

        [HttpGet]
        public string GetDropoffs()
        {
            var list = e.Database.SqlQuery<Dropoff>("SELECT * FROM Dropoffs").ToList<Dropoff>();
            return JsonConvert.SerializeObject(list);
        }
	}
}