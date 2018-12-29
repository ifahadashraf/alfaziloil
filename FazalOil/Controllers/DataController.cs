using FazalOil.Models;
using FazilOil.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FazilOil.Controllers
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

                var pr = e.Database.SqlQuery<Product>("SELECT TOP 1 * FROM Products ORDER BY ProductID DESC").ToList<Product>()[0];
                Transaction t = new Transaction();
                t.ProductID = pr.ProductID;
                t.PurchasingPrice = obj.PurchasingPrice;
                t.SupplierID = obj.SupplierID;
                t.TotalCans = obj.TotalCans;
                t.TotalLitres = obj.TotalLiters;
                t.CanSize = obj.CanSize;
                t.TotalQuantity = obj.TotalQuantity;
                t.TransactionDate = obj.DateOfPurchase;
                e.Transactions.Add(t);
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

                Transaction t = new Transaction();
                t.ProductID = obj.ProductID;
                t.PurchasingPrice = obj.PurchasingPrice;
                t.SupplierID = obj.SupplierID;
                t.TotalCans = obj.TotalCans;
                t.TotalLitres = obj.TotalLiters;
                t.CanSize = obj.CanSize;
                t.TotalQuantity = obj.TotalQuantity;
                t.TransactionDate = obj.DateOfPurchase;
                e.Transactions.Add(t);

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

        [HttpGet]
        public string GetZakaat()
        {
            var list = e.Database.SqlQuery<Expens>("SELECT * FROM Expenses WHERE Purpose = 'ZakaatPurpose' ORDER BY ExpenseID DESC").ToList<Expens>();
            return JsonConvert.SerializeObject(list);
        }

        [HttpPost]
        public string AddExpense()
        {
            try
            {
                StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);
                string requestFromPost = reader.ReadToEnd();

                var exp = JsonConvert.DeserializeObject<Expens>(requestFromPost);
                e.Expenses.Add(exp);
                e.SaveChanges();

                return "1";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            return "-1";
            
        }

        //[HttpGet]
        //public void GetSales()
        //{
        //    var list = e.Database.SqlQuery<Expens>("SELECT * FROM Expenses WHERE Purpose != 'ZakaatPurpose' ORDER BY ExpenseID DESC").ToList<Expens>();
        //    return JsonConvert.SerializeObject(list);
        //}

        [HttpGet]
        public string GetExpenses(string date)
        {
            var list = e.Database.SqlQuery<Expens>("SELECT * FROM Expenses WHERE Purpose != 'ZakaatPurpose' AND Date = '"+date+"' ORDER BY ExpenseID DESC").ToList<Expens>();
            return JsonConvert.SerializeObject(list);
        }

        [HttpGet]
        public string GetDailyExpenses(string dateFrom, string dateTo)
        {
            var list = e.Database.SqlQuery<Expens>("SELECT * FROM Expenses WHERE Date >= '" + dateFrom + "' AND Date <= '"+dateTo+"' ORDER BY ExpenseID DESC").ToList<Expens>();
            return JsonConvert.SerializeObject(list);
        }

        [HttpGet]
        public string SearchInvoice(string query, string msg)
        {
            try
            {
                List<SaleInvoice> saleObj = null;
                List<SaleItemModel> listOfItems = null;
                if (msg == "v")
                {
                    saleObj = e.Database.SqlQuery<SaleInvoice>("SELECT SI.* " +
                                                               "FROM SaleInvoice SI " +
                                                               "INNER JOIN CustomerDetails CD ON CD.CustomerID = SI.CustomerID " +
                                                               "WHERE CD.VehicleNumber = '" + query + "' ORDER BY 1 DESC").ToList<SaleInvoice>();
                }
                else
                {
                    saleObj = e.Database.SqlQuery<SaleInvoice>("SELECT * FROM SaleInvoice WHERE SaleID = " + query).ToList<SaleInvoice>();
                }
                if (saleObj.Count > 0)
                {
                    listOfItems = e.Database.SqlQuery<SaleItemModel>("SELECT si.*,p.* " +
                                                                    "FROM SaleItems si , Products p " +
                                                                    "WHERE si.ProductID = p.ProductID AND SaleID = " + saleObj[0].SaleID +
                                                                    " ORDER BY SaleID").ToList<SaleItemModel>();
                    var json = new
                    {
                        invoice = saleObj,
                        items = listOfItems
                    };
                    return JsonConvert.SerializeObject(json);
                }
                else
                {
                    return "-1";
                }
            }
            catch (Exception ex)
            {
                return "-1";
            }
            
        }

        [HttpGet]
        public string GetCustomers()
        {
            var list = e.Database.SqlQuery<CustomerDetail>("SELECT * FROM CustomerDetails").ToList<CustomerDetail>();
            return JsonConvert.SerializeObject(list);
        }

        [HttpGet]
        public string UpdateInvoice(long saleID, long balance)
        {
            var obj = e.SaleInvoices.Where(x => x.SaleID == saleID).ToList<SaleInvoice>()[0];
            obj.Balance = balance;

            if (obj.Balance == 0)
            {
                obj.SaleTypeID = 1;
            }

            e.SaveChanges();

            return "1";
        }

        [HttpPost]
        public string AddInvoice()
        {
            StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);
            string requestFromPost = reader.ReadToEnd();

            try
            {
                InvoiceModel obj = JsonConvert.DeserializeObject<InvoiceModel>(requestFromPost);

                SaleInvoice invoice = new SaleInvoice();
                TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");

                DateTime newDateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
                invoice.SaleDateTime = newDateTime;
                invoice.TotalAmount = obj.netTotal;
                invoice.SaleComments = obj.comments;

                if (obj.customer.VehicleNumber != "") //new Item
                {
                    var exisiting = e.CustomerDetails.Where(x => x.VehicleNumber == obj.customer.VehicleNumber).ToList<CustomerDetail>();
                    if (exisiting.Count > 0)
                    {
                        exisiting[0].CurrentReading = obj.customer.CurrentReading;
                        exisiting[0].ExpectedChange = obj.customer.ExpectedChange;
                        invoice.CustomerID = exisiting[0].CustomerID;
                    }
                    else
                    {
                        e.CustomerDetails.Add(obj.customer);
                        e.SaveChanges();
                        var c_list = e.CustomerDetails.ToList<CustomerDetail>();
                        invoice.CustomerID = c_list[c_list.Count - 1].CustomerID;
                    }
                }

                invoice.Balance = obj.balance;
                if (obj.typeOfInvoice.ToLower() == "cash")
                {
                    invoice.SaleTypeID = 1;
                }
                else
                {
                    invoice.SaleTypeID = 2;
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

                return "" + invoiceID;
            }
            catch (Exception ex)
            {
                return "-1";
            }
        }

        public string UpdateInvoice()
        {
            try
            {
                StreamReader reader = new StreamReader(System.Web.HttpContext.Current.Request.InputStream);
                string requestFromPost = reader.ReadToEnd();

                InvoiceModel obj = JsonConvert.DeserializeObject<InvoiceModel>(requestFromPost);

                SaleInvoice invoice = e.SaleInvoices.Where(x => x.SaleID == obj.SaleID).ToList<SaleInvoice>()[0]; //Get Invoice
                invoice.TotalAmount = obj.netTotal;
                invoice.SaleComments = obj.comments;
                invoice.Balance = obj.balance;
                if (obj.typeOfInvoice.ToLower() == "cash")
                {
                    invoice.SaleTypeID = 1;
                }
                else
                {
                    invoice.SaleTypeID = 2;
                }

                var saleItems = e.SaleItems.Where(pr => pr.SaleID == invoice.SaleID).ToList<SaleItem>(); //Get respective sale items

                foreach (var item in saleItems)
                {
                    var prd = e.Products.Where(x => x.ProductID == item.ProductID).ToList<Product>()[0]; //Update stock
                    try
                    {
                        prd.TotalCans += item.CansSold;
                    }
                    catch (Exception ex) { }

                    try
                    {
                        prd.TotalLiters += item.SellingLiters;
                    }
                    catch (Exception ex) { }

                    try
                    {
                        prd.TotalQuantity += item.Quantity;
                    }
                    catch (Exception ex) { }

                    e.SaleItems.Remove(item);   //Delete saleItem

                }

                foreach (var item in obj.items)
                {
                    SaleItem newItem = new SaleItem();
                    newItem.SaleID = invoice.SaleID;
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

                return "1";
            }
            catch (Exception ex)
            {
                return "-1";
            }
            
            
        }

        [HttpGet]
        public string GetSalesByDate(string dateFrom, string dateTo)
        {
            var list = e.GET_SALES_REPORT_BY_DATE(dateFrom,dateTo).ToList<GET_SALES_REPORT_BY_DATE_Result>();
            foreach(GET_SALES_REPORT_BY_DATE_Result obj in list)
            {
                if(obj.CansSold ==null && obj.SellingLiters == null)
                {
                    obj.Profit = obj.Profit * Convert.ToDouble(obj.Quantity);
                }
                else if(obj.CansSold == null)
                {
                    obj.Profit = obj.Profit * Convert.ToDouble(obj.SellingLiters);
                }
                else
                {
                    obj.Profit = obj.Profit * Convert.ToDouble(obj.CansSold);
                }
            }
            return JsonConvert.SerializeObject(list);
        }

	}
}