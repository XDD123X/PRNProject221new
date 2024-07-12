using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly int RecordPerPage = 5;
        private int maxPage;

        private readonly PROJECT_PRUContext dbcontext = new PROJECT_PRUContext();
        private readonly IDistributedCache _cache;
        public UsersModel(IDistributedCache _cache)
        {
            this._cache = _cache;
        }
        public async Task<IActionResult> OnGet()
        {
            String email = await _cache.GetStringAsync("adminEmail");
            if (email == null)
            {
                return RedirectToPage("Login_Cw4B8w6tetCtzk7PQHuZbA==");
            }

            maxPage = dbcontext.Users.Count() / RecordPerPage;
            if (dbcontext.Users.Count() % RecordPerPage != 0) maxPage++;
            ViewData["MaxPage"] = maxPage;
            ViewData["recordPerPage"] = RecordPerPage;

            return Page();
        }

        public IActionResult OnGetPage(int index)
        {
            int skip = (index - 1) * RecordPerPage;

            List<User> users = dbcontext.Users.OrderBy(u => u.Id)
                                                      .Skip(skip)
                                                      .Take(RecordPerPage)
                                                      .ToList();

            return new JsonResult(users);
        }
    }
}
