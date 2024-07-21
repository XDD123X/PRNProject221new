using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;
using System.Threading.Tasks;

namespace ProjectPRN221.Pages.Courses
{
    public class CreateModel : PageModel
    {
        private readonly PROJECT_PRUContext _context = new PROJECT_PRUContext();

        [BindProperty]
        public Course? Course { get; set; }

        [BindProperty]
        public Explode? NewExplode { get; set; }

        public User? CurrentUser { get; set; }

        public IActionResult OnGet(long cid)
        {
            //string? currUserID = HttpContext.Session.GetString("Session_User");
            //if (currUserID == null || currUserID == "")
            //{
            //    return RedirectToPage("/Authentication/login");
            //}
            //else
            //{
            //    CurrentUser = await _context.Users.FindAsync(Int32.Parse(currUserID));
            //}

            CurrentUser = _context.Users.FirstOrDefault(u => u.Id == 1);
            Course = new Course();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Course.Id == 0)
            {
                _context.Courses.Add(Course);
            }
            else
            {
                _context.Courses.Update(Course);
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
