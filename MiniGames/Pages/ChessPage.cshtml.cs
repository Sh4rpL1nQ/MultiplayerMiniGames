using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChessWebApp.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChessWebApp
{
    public class ChessPageModel : PageModel
    {
        private GameHub hub;

        public ChessPageModel(GameHub hub)
        {
            this.hub = hub;
        }
    }
}