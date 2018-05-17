using FazalOil.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            if (obj.ProductID == -1) //Add Product
            {
                e.Products.Add(obj);
                e.SaveChanges();
            }
            else //Update Product
            {
                var toUpdate = e.Products.Where(x => x.ProductID == obj.ProductID).ToList<Product>()[0];

                toUpdate.PurchasingPrice = (toUpdate.PurchasingPrice + obj.PurchasingPrice) / 2;
                toUpdate.ProductName = obj.ProductName;
                toUpdate.DateOfPurchase = obj.DateOfPurchase;
                toUpdate.DropoffID = obj.DropoffID;
                toUpdate.SupplierID = obj.SupplierID;
                toUpdate.CategoryID = obj.CategoryID;

                if (toUpdate.CategoryID == 3 || toUpdate.CategoryID == 5) //Filter and others
                {
                    toUpdate.TotalQuantity += obj.TotalQuantity;
                }
                else if (toUpdate.CategoryID == 2) // raw oil
                {
                    toUpdate.TotalLiters += obj.TotalLiters;
                }
                else // engine oil
                {
                    if (obj.TotalLiters == 0)
                    {
                        obj.TotalLiters = toUpdate.CanSize * obj.TotalCans;
                    }
                    toUpdate.TotalLiters += obj.TotalLiters;
                    toUpdate.TotalCans += obj.TotalCans;
                }
                
                e.SaveChanges();
                
            }

            
            return JsonConvert.SerializeObject(new {code=200});
        }

        [HttpPost]
        public string UpdateProduct()
        {
            StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);
            string requestFromPost = reader.ReadToEnd();

            Product obj = JsonConvert.DeserializeObject<Product>(requestFromPost);

            var toUpdate = e.Products.Where(x => x.ProductID == obj.ProductID).ToList<Product>()[0];

            toUpdate.PurchasingPrice = obj.PurchasingPrice;
            toUpdate.ProductName = obj.ProductName;
            toUpdate.DateOfPurchase = obj.DateOfPurchase;
            toUpdate.DropoffID = obj.DropoffID;
            toUpdate.SupplierID = obj.SupplierID;
            toUpdate.CategoryID = obj.CategoryID;
            toUpdate.BrandID = obj.BrandID;

            if (toUpdate.CategoryID == 3 || toUpdate.CategoryID == 5) //Filter and others
            {
                toUpdate.TotalQuantity = obj.TotalQuantity;
            }
            else if (toUpdate.CategoryID == 2) // raw oil
            {
                toUpdate.TotalLiters = obj.TotalLiters;
            }
            else // engine oil
            {
                toUpdate.TotalLiters = obj.TotalLiters;
                toUpdate.CanSize = obj.CanSize;
                toUpdate.TotalCans = obj.TotalCans;
            }

            e.SaveChanges();


            return JsonConvert.SerializeObject(new { code = 200 });
        }

        [HttpGet]
        public string GetProducts()
        {
            var list = e.Database.SqlQuery<Product>("SELECT * FROM Products").ToList<Product>();
            return JsonConvert.SerializeObject(list);
        }

        [HttpGet]
        public string GetAvailableStock()
        {
            var list = e.Database.SqlQuery<Product>("SELECT * FROM Products WHERE (TotalQuantity IS NULL AND TotalLiters > 0) OR (TotalLiters IS NULL AND TotalQuantity > 0)").ToList<Product>();
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

        [HttpPost]
        public string AddInvoice()
        {
            StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);
            string requestFromPost = reader.ReadToEnd();

            InvoiceModel obj = JsonConvert.DeserializeObject<InvoiceModel>(requestFromPost);

            SaleInvoice invoice = new SaleInvoice();
            invoice.SaleDateTime = DateTime.Now;
            invoice.TotalAmount = obj.netTotal;
            invoice.SaleComments = obj.comments;
            invoice.Balance = obj.balance;
            if (obj.typeOfInvoice.ToLower() == "cash")
            {
                invoice.SaleTypeID = 1;
            }
            else
            {
                invoice.SaleTypeID = 1;
            }

            e.SaleInvoices.Add(invoice);
            e.SaveChanges();

            var invoiceID = e.SaleInvoices.SqlQuery("SELECT TOP 1 * FROM SaleInvoice ORDER BY SaleID DESC").ToList<SaleInvoice>()[0].SaleID;

            foreach (var item in obj.items)
            {
                SaleItem newItem = new SaleItem();
                newItem.SaleID = invoiceID;
                newItem.ProductID = item.pid;
                newItem.SellingPrice = item.rate;
                newItem.Quantity = item.quantity;
                newItem.CansSold = item.cans;
                newItem.SellingLiters = item.litres;
                newItem.Total = item.subTotal;

                var prd = e.Products.Where(x => x.ProductID == item.pid).ToList<Product>()[0];
                try
                {
                    prd.TotalCans -= item.cans;
                }
                catch (Exception ex) { }

                try
                {
                    prd.TotalLiters -= item.litres;
                }
                catch (Exception ex) { }

                try
                {
                    prd.TotalQuantity -= item.quantity;
                }
                catch (Exception ex) { }

                e.SaleItems.Add(newItem);
            }

            e.SaveChanges();

            return ""+invoiceID;
        }

        private void SendToPrinter()
        {
            ProcessStartInfo info = new ProcessStartInfo();
            info.Verb = "print";
            info.FileName = @"c:\output.pdf";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            Process p = new Process();
            p.StartInfo = info;
            p.Start();

            p.WaitForInputIdle();
            System.Threading.Thread.Sleep(3000);
            if (false == p.CloseMainWindow())
                p.Kill();
        }
	}
}