//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FazalOil.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SaleItem
    {
        public long SaleID { get; set; }
        public long ProductID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public double SellingPrice { get; set; }
        public Nullable<double> SellingLiters { get; set; }
        public Nullable<int> CansSold { get; set; }
        public Nullable<double> Total { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual SaleInvoice SaleInvoice { get; set; }
    }
}
