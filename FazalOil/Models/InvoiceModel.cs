using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FazalOil.Models
{
    [Serializable]
    public class InvoiceModel
    {
        [JsonProperty("date")]
        public DateTime date { get; set; }

        [JsonProperty("netTotal")]
        public int netTotal { get; set; }

        [JsonProperty("balance")]
        public int balance { get; set; }

        [JsonProperty("typeOfInvoice")]
        public string typeOfInvoice { get; set; }

        [JsonProperty("comments")]
        public string comments { get; set; }

        [JsonProperty("items")]
        public List<Items> items { get; set; }

        [JsonProperty("customer")]
        public CustomerDetail customer;

    }

    public class Items
    {
        [JsonProperty("pid")]
        public int pid { get; set; }

        [JsonProperty("litres")]
        public double? litres { get; set; }

        [JsonProperty("cans")]
        public int? cans { get; set; }

        [JsonProperty("quantity")]
        public int? quantity { get; set; }

        [JsonProperty("rate")]
        public double rate { get; set; }

        [JsonProperty("subTotal")]
        public double subTotal { get; set; } 
    }
}