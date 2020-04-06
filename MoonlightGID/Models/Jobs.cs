using System;
using System.Collections.Generic;

namespace MoonlightGID.Models
{
    public partial class Jobs
    {
        public Jobs()
        {
            Reviews = new HashSet<Reviews>();
        }

        public int JobId { get; set; }
        public int? ServiceId { get; set; }
        public string JobName { get; set; }
        public string JobType { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        public decimal Price { get; set; }

        public virtual Businesses Company { get; set; }
        public virtual Services Service { get; set; }
        public virtual ICollection<Reviews> Reviews { get; set; }
    }
}
