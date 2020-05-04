using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniGames.Hubs;

namespace MiniGames.Pages
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
