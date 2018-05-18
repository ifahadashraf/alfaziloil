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
    
    public partial class SaleInvoice
    {
        public SaleInvoice()
        {
            this.SaleItems = new HashSet<SaleItem>();
        }
    
        public long SaleID { get; set; }
        public System.DateTime SaleDateTime { get; set; }
        public Nullable<long> CustomerID { get; set; }
        public double TotalAmount { get; set; }
        public long SaleTypeID { get; set; }
        public string SaleComments { get; set; }
        public double Balance { get; set; }
    
        public virtual CustomerDetail CustomerDetail { get; set; }
        public virtual SaleType SaleType { get; set; }
        public virtual ICollection<SaleItem> SaleItems { get; set; }
    }
}
