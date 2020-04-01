using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoonlightGID.Models
{
    public partial class Customers
    {
        public Customers()
        {
            Services = new HashSet<Services>();
        }

        public int CustomerId { get; set; }
        [Required(ErrorMessage="Enter your Username")]
        public string UserLogin { get; set; }
        [Required(ErrorMessage = "Enter your Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Enter your First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Enter your Last Name")]
        public string LastName { get; set; }
        public string CityAddress { get; set; }
        [Required(ErrorMessage ="Enter your Zip Code")]
        public string ZipCode { get; set; }
        [Required(ErrorMessage="Enter your Phone number")]
        public string ContactNumber { get; set; }
        public DateTime? BirthDate { get; set; }
        [Required]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage= "Please Enter a valid Email address")]
        public string Email { get; set; }

        public virtual ICollection<Services> Services { get; set; }
    }
}
