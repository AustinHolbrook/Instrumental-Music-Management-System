using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace _2021IMMS.Models
{
    public partial class RentalAgreement
    {
        public int RentalAgreementId { get; set; }

        [Display(Name = "Student")]
        [Required(ErrorMessage = "Please select a student.")]
        public int? StudentId { get; set; }

        [Display(Name = "Instrument")]
        [Required(ErrorMessage = "Please select an instrument.")]
        public int? InstrumentId { get; set; }

        [Display(Name = "Renter Signature")]
        [StringLength(50)]
        public string RenterSignature { get; set; }

        [Display(Name = "Faculty Signature")]
        [StringLength(50)]
        public string FacultySignature { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Please select todays date.")]
        public DateTime? Date { get; set; }

        public virtual Instrument Instrument { get; set; }
        public virtual Student Student { get; set; }
    }
}
