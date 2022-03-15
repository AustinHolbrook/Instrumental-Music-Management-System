using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Pieces
{
    [BindProperties]
    public class DeletePieceModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext IMMSContext;

        public DeletePieceModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            IMMSContext = IMMSC;
        }

        public Piece Piece { get; set; }

        public async Task<IActionResult> OnGetAsync(int intPieceId)
        {
            // Lookup the row in the table to see if it still exists.
            Piece = await IMMSContext.Piece
                .FindAsync(intPieceId);

            if (Piece != null)
            {
                try
                {
                    // Delete the row from the table.
                    IMMSContext.Piece.Remove(Piece);
                    await IMMSContext.SaveChangesAsync();
                    // Set the message.
                    HttpContext.Session.SetString("MessageColor", "lightgreen");
                    HttpContext.Session.SetString("Message", Piece.Title + " " + " was successfully deleted.");
                }
                catch (DbUpdateException objDbUpdateException)
                {
                    // A database exception has occured. Set the message.
                    HttpContext.Session.SetString("MessageColor", "tomato");
                    HttpContext.Session.SetString("Message", Piece.Title + " " + " was NOT deleted. Please report this this message to: rbeasley@franklincollege.edu." + objDbUpdateException.InnerException.Message);
                }
            }
            else
            {
                // Set the message.
                HttpContext.Session.SetString("MessageColor", "lightgreen");
                HttpContext.Session.SetString("Message", Piece.Title + " " + " was successfully deleted.");
            }
            return Redirect("MaintainPiece");
        }
    }
}
