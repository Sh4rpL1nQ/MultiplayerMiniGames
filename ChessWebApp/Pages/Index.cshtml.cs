using ChessWebApp.Hubs;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
