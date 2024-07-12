using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Core;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Authentication
{
	public class loginModel : PageModel
	{
		private IDistributedCache _cache;
		public loginModel(IDistributedCache cache)
		{
			_cache = cache;
		}

		[BindProperty]
		public string? txtPassword { get; set; }
		[BindProperty]
		public string? txtEmail { get; set; }
		public async void OnGet()
		{

		}

		public async Task<IActionResult> OnPost()
		{
			using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
			{
				var student = _context.Users.Where(t => t.Email == txtEmail && t.IsDeleted == false).FirstOrDefault();
				if (student == null)
				{
					ViewData["error"] = "Account is not existed!";
					return Page();
				}

				string hashedPassword = student.Password;

				Console.WriteLine("hashPassword: " + hashedPassword);
				Console.WriteLine(BCrypt.Net.BCrypt.HashPassword(txtPassword));
				if (!BCrypt.Net.BCrypt.Verify(txtPassword, hashedPassword))
				{
					ViewData["error"] = "Account is not existed!";
					return Page();
				}

				HttpContext.Session.SetString("Session_"+ student.Id, "session");
				var sessionValue = HttpContext.Session.GetString("Session_" + student.Id);
				Console.WriteLine(sessionValue);
			}


			return Page();
		}

	}
}