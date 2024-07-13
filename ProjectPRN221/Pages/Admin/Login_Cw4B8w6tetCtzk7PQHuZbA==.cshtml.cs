using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Core;
using ProjectPRN221.Models;
using StackExchange.Redis;
using System.ComponentModel.DataAnnotations;

namespace ProjectPRN221.Pages.Admin
{

    public class AdminAccount
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class Login_Cw4B8w6tetCtzk7PQHuZbA__Model : PageModel
    {
        private readonly IDistributedCache _cache;
        private readonly PROJECT_PRUContext dbcontext = new PROJECT_PRUContext();
        public Login_Cw4B8w6tetCtzk7PQHuZbA__Model(IDistributedCache cache)
        {
            _cache = cache;
        }


        public async Task<IActionResult> OnGet()
        {
            string email = await _cache.GetRecordAsync<string>("adminEmail");
            if (email != null)
            {
                return RedirectToPage("Index");
            }
            else
                return Page();
        }

        public async Task<IActionResult> OnPostLogin(String Email, String Password)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            IConfigurationRoot configuration = builder.Build();

            var adminAccount = new AdminAccount();
            configuration.GetSection("AdminAccount1").Bind(adminAccount);

            User admin = dbcontext.Users.FirstOrDefault(x => x.Email == Email);
            if ((admin != null) || (adminAccount.Email.Equals(Email) && adminAccount.Password.Equals(Password)))
            {
                await _cache.SetRecordAsync<string>("adminEmail", Email);
            } else
            {
                ViewData["ErrorMessage"] = "Wrong mail or password";
                return Page();
            }
            return RedirectToPage("Index");
        }
    }
}
