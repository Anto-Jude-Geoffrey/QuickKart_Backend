using System;
using System.Collections.Generic;

namespace QuickKartDB.DataAccessLayer.Models
{
    public partial class User
    {
        public User()
        {
            PurchaseDetails = new HashSet<PurchaseDetail>();
        }

        public string EmailId { get; set; }
        public string UserPassword { get; set; }
        public byte? RoleId { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<PurchaseDetail> PurchaseDetails { get; set; }
    }
}
