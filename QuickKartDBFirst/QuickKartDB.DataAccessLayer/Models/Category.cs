using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace QuickKartDB.DataAccessLayer.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public byte CategoryId { get; set; }
        public string CategoryName { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}
