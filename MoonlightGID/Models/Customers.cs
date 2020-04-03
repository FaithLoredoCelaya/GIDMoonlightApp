using System;
using System.Collections.Generic;

namespace MoonlightGID.Models
{
    public partial class Customers
    {
        public Customers()
        {
            Services = new HashSet<Services>();
        }

        public int CustomerId { get; set; }
        public string UserLogin { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CityAddress { get; set; }
        public string ZipCode { get; set; }
        public string ContactNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Services> Services { get; set; }
    }
}
