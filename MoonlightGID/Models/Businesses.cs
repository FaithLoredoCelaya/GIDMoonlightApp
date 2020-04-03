using System;
using System.Collections.Generic;

namespace MoonlightGID.Models
{
    public partial class Businesses
    {
        public Businesses()
        {
            Jobs = new HashSet<Jobs>();
        }

        public int CompanyId { get; set; }
        public string CompanyLogin { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string ContactNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual Reviews Reviews { get; set; }
        public virtual ICollection<Jobs> Jobs { get; set; }
    }
}
