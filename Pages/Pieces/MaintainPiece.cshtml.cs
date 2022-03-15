using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using _2021IMMS.Models;
using Microsoft.AspNetCore.Http;

namespace _2021IMMS.Pages.Pieces
{
    public class MaintainPieceModel : PageModel
    {
        private readonly _2021IMMS.Models._2021IMMSContext _2021IMMSContext;

        public MaintainPieceModel(_2021IMMS.Models._2021IMMSContext IMMSC)
        {
            _2021IMMSContext = IMMSC;
        }

        [BindProperty]
        public string Filter { get; set; }

        public IList<Piece> PieceIList;
        private IQueryable<Piece> PieceIQueryable;
        public async Task OnGetAsync()
        {
            await RetrieveRowsForDisplay();
        }

        public async Task OnPostFilterAsync()
        {
            await RetrieveRowsForDisplay();
        }

        public RedirectResult OnPostClearFilter()
        {
            // Refresh the page so that the filter and textbox will clear.
            return Redirect("/Pieces/MaintainPiece");
        }

        private async Task RetrieveRowsForDisplay()
        {
            //Initialize the header data.
            ViewData["Page"] = "Maintain Piece";
            ViewData["Status"] = HttpContext.Session.GetString("Status");
            ViewData["User"] = HttpContext.Session.GetString("User");
            ViewData["MessageColor"] = HttpContext.Session.GetString("MessageColor");
            ViewData["Message"] = HttpContext.Session.GetString("Message");

            // Retrieve the rows for display.
            PieceIQueryable = (
                from p in _2021IMMSContext.Piece
                orderby p.Title
                select p)
                .AsNoTracking();

            // If a filter value was entered, modify the database query.
            if (Filter != null)
            {
                PieceIQueryable = PieceIQueryable
                    .Where(pil => pil.Title.Contains(Filter));
            }

            // Retrieve the rows for display.
            PieceIList = await PieceIQueryable
                .ToListAsync();

            // Expand each abbreviation in the list for user readablility.
            foreach (var item in PieceIList)
            {
                switch (item.Type)
                {
                    case "P":
                        item.Type = "Pep";
                        break;
                    case "C":
                        item.Type = "Concert";
                        break;
                    case "J":
                        item.Type = "Jazz";
                        break;
                    case "O":
                        item.Type = "Orchestra";
                        break;
                    default:
                        break;
                }

                // Here I was trying to make the view say "Never Played" instead of showng the abitrary date.
                //if(item.DateLastPlayed == new DateTime(0001, 01, 01))
                //{
                //    item.DateLastPlayed = "Never Played";
                //}
            }
        }
    }
}
