using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QuickKartDB.DataAccessLayer.Models
{
    public partial class Product
    {
        public Product()
        {
            PurchaseDetails = new HashSet<PurchaseDetail>();
        }

        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public byte? CategoryId { get; set; }
        public decimal Price { get; set; }
        public int QuantityAvailable { get; set; }
        [JsonIgnore]
        public virtual Category Category { get; set; }
        [JsonIgnore]
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
