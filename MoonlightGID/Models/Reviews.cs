using System;
using System.Collections.Generic;

namespace MoonlightGID.Models
{
    public partial class Reviews
    {
        public int ReviewId { get; set; }
        public int JobId { get; set; }
        public int CompanyId { get; set; }
        public string ReviewContent { get; set; }
        public double Rating { get; set; }

        public virtual Businesses Company { get; set; }
        public virtual Jobs Job { get; set; }
    }
}
