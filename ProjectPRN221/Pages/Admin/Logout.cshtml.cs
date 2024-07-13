using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;

namespace ProjectPRN221.Pages.Admin
{
    public class LogoutModel : PageModel
    {
        private readonly IDistributedCache _cache;
        public LogoutModel(IDistributedCache _cache)
        {
            this._cache = _cache;
        }
        public async Task<IActionResult> OnGet()
        {
            await _cache.RemoveAsync("adminEmail");
            return RedirectToPage("Login_Cw4B8w6tetCtzk7PQHuZbA==");
        }
    }
}
