using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace _2021IMMS.Models
{
    public partial class Administrator
    {
        public int AdministratorId { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "Please enter a first name.")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Display(Name = "Middle Initial")]
        [StringLength(1)]
        public string MiddleInitial { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Please enter a last name.")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Display(Name = "Phone Number")]
        [RegularExpression(@"\d{3}\-\d{3}\-\d{4}", ErrorMessage = "Please enter a phone number of the form: 999-999-9999.")]
        [Required(ErrorMessage = "Please enter a phone number.")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Display(Name = "Email Address")]
        [RegularExpression(@"\S+\@\S+\.\S+", 
            ErrorMessage = "Please enter an email address of the form: aaa@bbb.ccc.")]
        [Required(ErrorMessage = "Please enter an email address.")]
        [StringLength(50)]
        public string EmailAddress { get; set; }

        [Display(Name = "Password")]
        //[RegularExpression(@"\S{5,10}",
        //    ErrorMessage = "Please enter a password that contains between 5 and 10 non-blank characters.")]
        [Required(ErrorMessage = "Please enter a password.")]
        [StringLength(10)]
        public string Password { get; set; }

        [Display(Name = "Status")]
        [Required (ErrorMessage = "Please select a status.")]
        public string Status { get; set; }
    }
}
