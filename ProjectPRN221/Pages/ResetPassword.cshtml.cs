using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;
using ProjectPRN221.Utils;

namespace ProjectPRN221.Pages
{
    public class ResetPasswordModel : PageModel
    {
        [BindProperty]
        public string? txtOldPassword { get; set; }
        [BindProperty]
        public string? txtNewPassword { get; set; }
        [BindProperty]
        public string? txtRePassword { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            var sessionValue = HttpContext.Session.GetString("Session_User");
            if (sessionValue != null)
            {
                int userId = int.Parse(sessionValue);
                using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
                {
                    var user = _context.Users.FirstOrDefault(t => t.Id == userId);
                    string hashpassword = user.Password;
                    if (!BCrypt.Net.BCrypt.Verify(txtOldPassword, hashpassword))
                    {
                        ViewData["error"] = "Old password is wrong!";
                        return Page();
                    }

                    if (!ValidateUtils.validatePassword(txtNewPassword))
                    {
                        ViewData["error"] = "Format of new password is wrong!";
                        return Page();
                    }

                    if (txtNewPassword != txtRePassword)
                    {
                        ViewData["error"] = "New password is not match with Re-password";
                        return Page();
                    }

                    user.Password = BCrypt.Net.BCrypt.HashPassword(txtNewPassword);
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    ViewData["error"] = "Change password successful";
                    return Page();
                }
            }
                return RedirectToPage("Authentication/login");
        }
    }
}
