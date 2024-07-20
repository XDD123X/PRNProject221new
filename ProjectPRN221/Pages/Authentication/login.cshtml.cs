using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Core;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Authentication
{
	public class loginModel : PageModel
	{
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

				HttpContext.Session.SetString("Session_User", student.Id+"");
				var sessionValue = HttpContext.Session.GetString("Session_User");
				Console.WriteLine(sessionValue);
			}


			return RedirectToPage("/Index");
		}

	}
}