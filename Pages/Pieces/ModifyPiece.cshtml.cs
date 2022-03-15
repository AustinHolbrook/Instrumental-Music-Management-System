using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Pieces
{
    [BindProperties]
    public class ModifyPieceModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public ModifyPieceModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Piece Piece { get; set; }

        public async Task<IActionResult> OnGetAsync(int intPieceId)
        {
            //Initialize the header data.
            ViewData["Page"] = "Modify Piece";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = "lightgreen";
            ViewData["Message"] = "Please enter the information below and click Modify.";

            // Attempt to retrieve the row from the table.
            Piece = await IMMSContext.Piece
                .Where(a => a.PieceId == intPieceId)
                .FirstOrDefaultAsync();
            if (Piece != null)
            {
                return Page();
            }
            else
            {
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", "The selected Piece was recently deleted by someone else.");
                return Redirect("MaintainPiece");
            }
        }

        public async Task<IActionResult> OnPostModifyAsync()
        {
            // Autofill these inputs if they are left null.
            if (Piece.TimesPerformed == null)
            {
                Piece.TimesPerformed = 0;
            }
            if (Piece.Arranger == null)
            {
                Piece.Arranger = "N/A";
            }
            if (Piece.Composer == null)
            {
                Piece.Composer = "N/A";
            }

            try
            {
                // Modify the row in the table.
                IMMSContext.Piece.Update(Piece);
                await IMMSContext.SaveChangesAsync();
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "lightgreen");
                HttpContext.Session.SetString("Message", Piece.Title + " " +  " was successfully modified.");
            }
            catch (DbUpdateException objDbUpdateException)
            {
                // A database exception has occured. Set the message.
                HttpContext.Session.SetString("MessageColor", "tomato");
                HttpContext.Session.SetString("Message", Piece.Title + " " + " was NOT modified. Please report this this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
            }
            return Redirect("MaintainPiece");
        }

        public RedirectResult OnPostCancel()
        {
            // Set the message.
            HttpContext.Session.SetString("MessageColor", "tomato");
            HttpContext.Session.SetString("Message", "Operation was canceled. No data was affected.");
            return Redirect("MaintainPiece");
        }
    }
}
