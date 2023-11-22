using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerMgmtWebAPI.Models
{
    public class Customer
    {
        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Range(18, 90, ErrorMessage = "Age must be between 18 and 90")]
        public int Age { get; set; }

        [Required(ErrorMessage = "ID is required")]
        public int Id { get; set; }
    }
}