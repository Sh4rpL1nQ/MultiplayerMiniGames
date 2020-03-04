using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChessWebApp.Hubs;
using ChessWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ChessWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private GameHub hub;

        public IndexModel(GameHub hub)
        {
            this.hub = hub;
        }
    }
}
