using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniGames.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MiniGames.Pages
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