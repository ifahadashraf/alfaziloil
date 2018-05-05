using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FazalOil.Models
{
    public class ProductModel
    {
        public long ProductID { get; set; }
        public string ProductName { get; set; }
        public long BrandID { get; set; }
        public double PurchasingPrice { get; set; }
        public Nullable<double> CanSize { get; set; }
        public Nullable<int> TotalCans { get; set; }
        public Nullable<double> TotalLiters { get; set; }
        public Nullable<int> TotalQuantity { get; set; }
        public long DropoffID { get; set; }
        public long SupplierID { get; set; }
        public System.DateTime DateOfPurchase { get; set; }
    }
}