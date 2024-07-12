using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using MimeKit.Cryptography;
using ProjectPRN221.Models;
using System.Data;

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
                ViewData["TotalRevenues"] = await GetRevenuesAsync();
                ViewData["TotalUsers"] = dbcontext.Users.Where(p => p.Role.ToUpper() != "ADMIN").Count();
                return Page();
            }
        }

        public async Task<float> GetRevenuesAsync()
        {
            // Define the output parameter
            var revenuesParam = new SqlParameter
            {
                ParameterName = "@revenues",
                SqlDbType = SqlDbType.Float,
                Direction = ParameterDirection.Output
            };

            var sql = "EXEC GetRevenues @revenues OUT";

            await dbcontext.Database.ExecuteSqlRawAsync(sql, revenuesParam);

            float revenues = revenuesParam.Value != DBNull.Value ? (float)(double)revenuesParam.Value : 0;

            return revenues;
        }
    }
}
