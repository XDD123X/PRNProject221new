using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Core;
using ProjectPRN221.Models;
using ProjectPRN221.Utils;

namespace ProjectPRN221.Pages.Authentication
{
    public class registerModel : PageModel
    {
		private IDistributedCache _cache;
		public registerModel(IDistributedCache cache)
		{
			_cache = cache;
		}

		[BindProperty]
		public string? txtUsername { get; set; }
		[BindProperty]
		public string? txtPassword { get; set; }
		[BindProperty]
		public string? txtEmail { get; set; }
		[BindProperty]
		public string? sltRole { get; set; }
		[BindProperty]
		public string? txtRePassword { get; set; }
		public void OnGet()
        {
        }

		public async Task<IActionResult> OnPost()
        {

			if (validateUsername())
            {
                if(ValidateUtils.validatePassword(txtPassword)) 
                { 
                    if(txtPassword != txtRePassword)
                    {
						ViewData["error"] = "Re-password is not match with Password";
						return Page();
					}
					if(!validateEmail())
					{
						ViewData["error"] = "Email already existed";
						return Page();
					}
					ViewData["error"] = await handleRegister();
                } else
				{
					ViewData["error"] = "Password must have digit, letter and at least 8 characters!";
					return Page();
				}
			} else
			{
				ViewData["error"] = "Username already existed";
				return Page();
			}
			return Page();
		}

        private bool validateUsername()
        {
			using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
            {
				var student = _context.Users.Where(t => t.Username == txtUsername && t.IsDeleted == false).ToList();
                if (student == null || !student.Any()) return true;
                return false;
			}
		}

		private bool validateEmail()
		{
			using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
			{
				var student = _context.Users.Where(t => t.Email == txtEmail).ToList();
				if (student == null || !student.Any()) return true;
				return false;
			}
		}

		private async Task<string> handleRegister()
        {
			
			string? tmp = await _cache.GetRecordAsync<string>("Register"+"_"+txtEmail); // Get data from cache
			Console.WriteLine("Get OTP "  + tmp);
			if (tmp != null) return "Your OTP code is already send to your email!";
			string token = Guid.NewGuid().ToString();
			Console.WriteLine("Send otp");
		    EmailService.Send(txtEmail, "Your OTP In Project PRN:", "Your OTP to active your account is: " + token+ ". This code is expired after 10 minutes");
			DateTime expireDate = DateTime.Now.AddMinutes(10);
			Console.WriteLine("Save redis");
			_cache.SetRecordAsync("Register" + "_" + txtEmail, token, TimeSpan.FromMinutes(10));
			using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
			{	
				var student = new User()
				{
					Email = txtEmail,
					Password = BCrypt.Net.BCrypt.HashPassword(txtPassword),
					Role = sltRole,
					Username = txtUsername,
					CreatedAt = DateTime.Now,
					IsDeleted = true,
				};

				_context.Users.Add(student);
				_context.SaveChanges();

			
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
			return "Create account successful!";
		}
    }
}
