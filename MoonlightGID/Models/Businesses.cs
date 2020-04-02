using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoonlightGID.Models
{
    public partial class Businesses
    {
        public Businesses()
        {
            Jobs = new HashSet<Jobs>();
        }

        public int CompanyId { get; set; }
        [Required(ErrorMessage = "Enter your Username")]
        public string CompanyLogin { get; set; }
        [Required(ErrorMessage = "Enter your Password")]
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        [Required(ErrorMessage = "Enter your Buisness Phone number")]
        public string ContactNumber { get; set; }
        [Required]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Please Enter a valid Email address")]
        public string EmailAddress { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Jobs> Jobs { get; set; }
    }
}
