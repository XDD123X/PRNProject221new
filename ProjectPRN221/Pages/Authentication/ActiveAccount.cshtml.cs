using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Core;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Authentication
{
    public class ActiveAccountModel : PageModel
    {
		private IDistributedCache _cache;
		public ActiveAccountModel(IDistributedCache cache)
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
			if (!validateEmail()) {
				ViewData["notify"] = "Email is not in used";
				return Page();
			}
			String notify = await validateToken();
			ViewData["notify"] = notify;
			return Page();
		}

		private async Task<String> validateToken()
		{
			string activeToken = await _cache.GetRecordAsync<string>("Register" + "_" + txtEmail);
			if (activeToken != null)
			{
				Console.WriteLine("Check token in cache");
				if (txtToken != null && txtToken.Equals(activeToken))
				{
					Console.WriteLine("Same token");
					activateAccount();
					return "Active account successful";
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
						.Where(t => t.Email == txtEmail && t.IsDeleted == false && t.Type == "ACTIVE_ACCOUNT")
						.OrderBy(t => t.Id)
						.ToList();
					if (token != null)
					{
						Token activeAccount = (Token)token.FirstOrDefault();
						Console.WriteLine("Email has token " + activeAccount.Content);
						if (activeAccount?.Content == txtToken)
						{
							if(activeAccount.ExpiredDate >= DateTime.Now)
							{
								activateAccount();
								return "Active account successful";
							} else
							{
								return "Expired Token";
							}
						} else
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

		private void activateAccount()
		{
			using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
			{
				var student = _context.Users.Where(t => t.Email == txtEmail && t.IsDeleted == true).ToList();
				if (student != null)
				{
					User user = (User)student.FirstOrDefault();
					user.IsDeleted = true;
					_context.Update(user);
					_context.SaveChanges();
				}
			}
		}

		private bool validateEmail()
		{
			using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
			{
				var student = _context.Users.Where(t => t.Email.Equals(txtEmail) && t.IsDeleted == true).ToList();
				if (student == null || student.Count() == 0)
				{
					return false;
				}
			}
			return true;
		}
    }
}
