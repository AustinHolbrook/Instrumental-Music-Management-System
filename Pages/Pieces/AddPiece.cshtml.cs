using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;

namespace _2021IMMS.Pages.Pieces
{
    [BindProperties]
    public class AddPieceModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public AddPieceModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Piece Piece { get; set; }
        public bool NoDate { get; set; }

        public void OnGet()
        {
            //Initialize the header data.
            ViewData["Page"] = "Add Piece";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = "lightgreen";
            ViewData["Message"] = "Please enter the information below and click Add.";
        }

        public async Task<RedirectResult> OnPostAddAsync()
        {
            if(NoDate == true)
            {
                Piece.DateLastPlayed = new DateTime(0001, 01, 01);
            }
            if (ModelState.IsValid)
            {
                // Autofill these inputs if they are left null.
                if (Piece.TimesPerformed == null)
                {
                    Piece.TimesPerformed = 0;
                }
                if(Piece.Arranger == null)
                {
                    Piece.Arranger = "N/A";
                }
                if(Piece.Composer == null)
                {
                    Piece.Composer = "N/A";
                }
                
                
                try
                {
                    // Try to add the Piece to the database.
                    IMMSContext.Piece.Add(Piece);
                    await IMMSContext.SaveChangesAsync();
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "lightgreen");
                    HttpContext.Session.SetString("Message", Piece.Title + " was successfully added.");
                    return Redirect("MaintainPiece");
                }
                catch (DbUpdateException objDbUpdateException)
                {
                    // An exception has occurred. Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", Piece.Title + " " + " was NOT added. Please report this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
                    return Redirect("MaintainPiece");
                }
            }
            return Redirect("MaintainPiece");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "The operation was cancelled. No data was affected.");
            return Redirect("MaintainPiece");
        }
    }
}
