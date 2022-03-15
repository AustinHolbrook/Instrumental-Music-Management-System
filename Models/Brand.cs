using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace _2021IMMS.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Instruments = new HashSet<Instrument>();
        }

        public int BrandId { get; set; }

        [Display(Name = "Brand")]
        [Required(ErrorMessage = "Please enter a Brand.")]
        [StringLength(50)]
        public string Brand1 { get; set; }

        public virtual ICollection<Instrument> Instruments { get; set; }
    }
}
