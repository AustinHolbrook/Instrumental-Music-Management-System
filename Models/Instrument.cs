using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace _2021IMMS.Models
{
    public partial class Instrument
    {
        public Instrument()
        {
            RentalAgreements = new HashSet<RentalAgreement>();
        }

        public int InstrumentId { get; set; }

        [Display(Name = "Brand")]
        [Required(ErrorMessage = "Please select a brand.")]
        public int? BrandId { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Please select a type.")]
        public string Type { get; set; }

        [Display(Name = "Instrument")]
        [Required(ErrorMessage = "Please enter an instrument.")]
        [StringLength(50)]
        public string Instrument1 { get; set; }

        [Display(Name = "Serial Number")]
        [Required(ErrorMessage = "Please enter a serial number.")]
        [StringLength(20)]
        public string SerialNumber { get; set; }

        [Display(Name = "Condition")]
        [Required(ErrorMessage = "Please select a condition.")]
        public string Condition { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please enter a description.")]
        public string Description { get; set; }

        [Display(Name = "Additional Parts")]
        public string AdditionalParts { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual ICollection<RentalAgreement> RentalAgreements { get; set; }
    }
}
