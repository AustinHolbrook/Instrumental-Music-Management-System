using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace _2021IMMS.Models
{
    public partial class Piece
    {
        public int PieceId { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Please select a type.")]
        public string Type { get; set; }

        [Display(Name = "Title")]
        [Required(ErrorMessage = "Please enter a title.")]
        public string Title { get; set; }

        [Display(Name = "Arranger")]
       // [Required(ErrorMessage = "Please enter an arranger. If there is no arranger, enter 'N/A'.")]
        public string Arranger { get; set; }

        [Display(Name = "Composer")]
        //[Required(ErrorMessage = "Please enter a composer. If there is no composer, enter 'N/A'.")]
        public string Composer { get; set; }

        [Display(Name = "Remark")]
        [Required(ErrorMessage = "Please enter a remark.")]
        public string Remark { get; set; }

        [Display(Name = "Date Last Played")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateLastPlayed { get; set; }

        [Display(Name = "Times Performed")]
        //[Required(ErrorMessage = "Please enter the times performed. If the piece has not been performed, enter '0'.")]
        [RegularExpression(@"\d*", ErrorMessage = "Times performed must be a number.")]
        public int? TimesPerformed { get; set; }
    }
}
