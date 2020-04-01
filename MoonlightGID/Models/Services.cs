using System;
using System.Collections.Generic;

namespace MoonlightGID.Models
{
    public partial class Services
    {
        public Services()
        {
            Jobs = new HashSet<Jobs>();
        }

        public int ServiceId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public DateTime DateOrder { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual ICollection<Jobs> Jobs { get; set; }
    }
}
