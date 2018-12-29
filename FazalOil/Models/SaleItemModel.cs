using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FazilOil.Models
{
    public class SaleItemModel
    {
        public long SaleID { get; set; }
        public long ProductID { get; set; }
        public long DropoffID { get; set; }
        public long SupplierID { get; set; }
        public long BrandID { get; set; }
        public Nullable<long> CategoryID { get; set; }
        public double PurchasingPrice { get; set; }
        public Nullable<double> CanSize { get; set; }
        public Nullable<int> TotalCans { get; set; }
        public Nullable<double> TotalLiters { get; set; }
        public Nullable<int> TotalQuantity { get; set; }
        public System.DateTime DateOfPurchase { get; set; }
        public Nullable<int> Quantity { get; set; }
        public double SellingPrice { get; set; }
        public Nullable<double> SellingLiters { get; set; }
        public Nullable<int> CansSold { get; set; }
        public Nullable<double> Total { get; set; }
        public string ProductName { get; set; }
    }
}