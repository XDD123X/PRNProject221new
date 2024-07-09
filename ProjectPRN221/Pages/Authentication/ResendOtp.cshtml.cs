using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Core;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Authentication
{
	public class ResendOtpModel : PageModel
	{
		private IDistributedCache _cache;
		public ResendOtpModel(IDistributedCache cache)
		{
			_cache = cache;
		}

		[BindProperty]
		public string? txtEmail { get; set; }
		public void OnGet()
		{
		}

		public async Task<IActionResult> OnPost()
		{
			if (!validateEmail())
			{
				ViewData["error"] = "Email is not in used";
				return Page();
			}
			bool check = await checkExistToken();
			if (!check)
			{
				ViewData["error"] = "Your OTP is still valid. Please check your email!";
				return Page();
			}

			sendToken();
			ViewData["error"] = "Your OTP is send to your email!";
			return Page();
		}

		private bool validateEmail()
		{
			using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
			{
				var student = _context.Users.Where(t => t.Email == txtEmail && t.IsDeleted == true).ToList();
				if (student == null || !student.Any()) return false;
				return true;
			}
		}

		private async Task<bool> checkExistToken()
		{
			string activeToken = await _cache.GetRecordAsync<string>("Register" + "_" + txtEmail);
			if (activeToken == null)
			{
				using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
				{
					var token = _context.Tokens
						.Where(t => t.Email == txtEmail && t.IsDeleted == false && t.Type == "ACTIVE_ACCOUNT")
						.OrderBy(t => t.Id)
						.ToList();

					if (token != null && token.Count > 0)
					{
						Token activeAccount = (Token)token.FirstOrDefault();
						if (activeAccount.ExpiredDate >= DateTime.Now)
						{
							return false;
						}
					}
				}
			}
			else
			{
				return false;
			}
			return true;
		}

		private void sendToken()
		{
			string token = Guid.NewGuid().ToString();
			EmailService.Send(txtEmail, "Your OTP In Project PRN:", "Your OTP to active your account is: " + token + ". This code is expired after 10 minutes");
			DateTime expireDate = DateTime.Now.AddMinutes(10);
			_cache.SetRecordAsync("Register" + "_" + txtEmail, token, TimeSpan.FromMinutes(10));
			using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
			{
				var student = _context.Users.Where(t => t.Email.Equals(txtEmail) && t.IsDeleted == true).FirstOrDefault();

				var activeToken = new Token()
				{
					UserId = student.Id,
					Content = token,
					ExpiredDate = expireDate,
					Type = "ACTIVE_ACCOUNT",
					Email = txtEmail,
					IsDeleted = false,
				};
				_context.Tokens.Add(activeToken);
				_context.SaveChanges();
			}
		}
	}

}