using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace QuickKartDB.DataAccessLayer.Models
{
    public class ProductCategory
    {
        [Key]
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string CategoryName { get; set; } = null!;
        public int QuantityAvailable { get; set; }
    }
}
