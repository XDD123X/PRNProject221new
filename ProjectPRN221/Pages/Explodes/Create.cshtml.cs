using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Explodes
{
    public class CreateModel : PageModel
    {
        private readonly PROJECT_PRUContext _context = new PROJECT_PRUContext();

        [BindProperty]
        public Explode? Explode { get; set; }

        [BindProperty]
        public long CourseId { get; set; }

        public IActionResult OnGet(long courseId)
        {
            CourseId = courseId;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Explode.CourseId = CourseId;
            _context.Explodes.Add(Explode);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
