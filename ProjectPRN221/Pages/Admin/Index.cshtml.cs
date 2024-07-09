using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using MimeKit.Cryptography;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Admin
{
    public class IndexModel : PageModel
    {
        private readonly PROJECT_PRUContext dbcontext = new PROJECT_PRUContext();
        private readonly IDistributedCache _cache;
        public IndexModel(IDistributedCache cache)
        {
            _cache = cache; 
        }
        public async Task<IActionResult> OnGet()
        {
            String email = await _cache.GetStringAsync("adminEmail");
            if (email == null)
            {
                return RedirectToPage("Login_Cw4B8w6tetCtzk7PQHuZbA==");
            }
            else
            {

                //ViewData["TotalRevenues"] = dbcontext.EnroledCourses.Where(p => p.UpdatedAt.GetValueOrDefault().Month == DateTime.Now.Month);
                return Page();
            }
        }
    }
}
