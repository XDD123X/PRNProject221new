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
                ViewData["MonthRevenues"] = await GetRevenuesInMonth();
                ViewData["TotalRevenues"] = await GetRevenuesAsync();



                ViewData["UserInMonth"] = dbcontext.Users.Where(p => (p.Role.ToUpper() != "ADMIN")
                && (DateTime.Now.Month == p.CreatedAt.Month)
                && (DateTime.Now.Year == p.CreatedAt.Year)).Count();

                ViewData["TotalUser"] = dbcontext.Users.Where(p => p.Role.ToUpper() != "ADMIN").Count();


                ViewData["Student"] = dbcontext.Users.Where(p => p.Role.Equals("Student")).Count();
                ViewData["Lecture"] = dbcontext.Users.Where(p => p.Role.Equals("Lecture")).Count();

                ViewData["courses"] = dbcontext.Courses.Select(p => p.Title).ToList();
                List<float> eachRevenues = new List<float>();
                List<Course> courses = dbcontext.Courses.ToList();
                foreach (Course c in courses)
                {
                    int count = dbcontext.EnroledCourses.Where(p => p.CourseId == c.Id).Count(); 
                    float revenue = (float)(c.Price.GetValueOrDefault()) * count;
                    eachRevenues.Add(revenue);
                }
                ViewData["eachRevenues"] = eachRevenues;

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

        public async Task<float> GetRevenuesInMonth()
        {
            // Define the output parameter
            var revenuesParam = new SqlParameter
            {
                ParameterName = "@revenues",
                SqlDbType = SqlDbType.Float,
                Direction = ParameterDirection.Output
            };

            var sql = "EXEC GetMonthRevenues @revenues OUT";

            await dbcontext.Database.ExecuteSqlRawAsync(sql, revenuesParam);

            float revenues = revenuesParam.Value != DBNull.Value ? (float)(double)revenuesParam.Value : 0;

            return revenues;
        }
    }
}
