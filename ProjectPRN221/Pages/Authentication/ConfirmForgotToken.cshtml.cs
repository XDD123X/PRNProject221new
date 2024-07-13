using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Core;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Authentication
{
    public class ConfirmForgotTokenModel : PageModel
    {
		private IDistributedCache _cache;
		public ConfirmForgotTokenModel(IDistributedCache cache)
		{
			_cache = cache;
		}

		[BindProperty]
		public string? txtEmail { get; set; }
		[BindProperty]
		public string? txtToken { get; set; }

		public void OnGet()
        {
        }


		public async Task<IActionResult> OnPost()
		{
			if (!validateEmail())
			{
				ViewData["notify"] = "Email is not in used";
				return Page();
			}
			String notify = await validateToken();
			ViewData["notify"] = notify;
			return Page();
		}

		private async Task<String> validateToken()
		{
			string activeToken = await _cache.GetRecordAsync<string>("Forgot_Password" + "_" + txtEmail);
			if (activeToken != null)
			{
				Console.WriteLine("Check token in cache");
				if (txtToken != null && txtToken.Equals(activeToken))
				{
					Console.WriteLine("Same token");
					sendPassword();
					return "New Password was send to your email";
				}
				else
				{
					return "Wrong token";
				}
			}
			else
			{
				Console.WriteLine("Check token in database");
				using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
				{
					var token = _context.Tokens
						.Where(t => t.Email == txtEmail && t.IsDeleted == false && t.Type == "FORGOT_PASSWORD")
						.OrderBy(t => t.Id)
						.ToList();
					if (token != null && token.Any())
					{
						Token activeAccount = (Token)token.FirstOrDefault();
						Console.WriteLine("Email has token " + activeAccount.Content);
						if (activeAccount?.Content == txtToken)
						{
							if (activeAccount.ExpiredDate >= DateTime.Now)
							{
								sendPassword();
								return "Active account successful";
							}
							else
							{
								return "Expired Token";
							}
						}
						else
						{
							return "Wrong Token";
						}
					}
					else
					{
						return "This email does not have any active token";
					}
				}
			}
		}

		private void sendPassword()
		{
			using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
			{
				var student = _context.Users.Where(t => t.Email == txtEmail && t.IsDeleted == false).ToList();
				if (student != null && student.Any())
				{
					string password = Guid.NewGuid().ToString();
					User user = (User)student.FirstOrDefault();
					string hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
					Console.WriteLine("Hashpassword from *" + password + "* to *" + hashPassword + "*");
					user.Password = hashPassword;
					EmailService.Send(txtEmail, "Your new password in PROJECT PRN:", "Your password is set to: " + password);
					_context.Update(user);
					_context.SaveChanges();
				}
			}
		}

		private bool validateEmail()
		{
			using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
			{
				var student = _context.Users.Where(t => t.Email.Equals(txtEmail) && t.IsDeleted == false).ToList();
				if (student == null || student.Count() == 0)
				{
					return false;
				}
			}
			return true;
		}
	}
}
