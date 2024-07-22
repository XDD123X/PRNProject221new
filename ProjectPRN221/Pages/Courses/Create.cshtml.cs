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

        public async Task<IActionResult> OnGetAsync()
        {
            string? currUserID = HttpContext.Session.GetString("Session_User");
            if (currUserID == null || currUserID == "")
            {
                return RedirectToPage("/Authentication/login");
            }
            else
            {
                CurrentUser = await _context.Users.FindAsync(long.Parse(currUserID));
            }

            CurrentUser = _context.Users.FirstOrDefault(u => u.Id == 1);
            Course = new Course();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("./Idenx");
            }
            string? currUserID = HttpContext.Session.GetString("Session_User");
            if (currUserID == null || currUserID == "")
            {
                return RedirectToPage("/Authentication/login");
            }
            else
            {
                CurrentUser = _context.Users.FirstOrDefault(c => c.Id == long.Parse(currUserID));
            }
            var courseToCreate = new Course();
            courseToCreate.Title = Course.Title;
            courseToCreate.Thumbnail = Course.Thumbnail;
            courseToCreate.Description = Course.Description;
            courseToCreate.Categories = Course.Categories;
            courseToCreate.UserId = CurrentUser.Id;
            _context.Courses.Add(courseToCreate);
            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
