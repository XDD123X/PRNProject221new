using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Explodes
{
    public class DetailsModel : PageModel
    {
        private readonly ProjectPRN221.Models.PROJECT_PRUContext _context = new PROJECT_PRUContext();
        public Explode Explode { get; set; } = default!;
        public User currUser { get; set; } = default!;
        public Course Course { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(long? eid, long? cid)
        {
            //lay user hien tai
            currUser = new User()
            {
                Id = 3,
                Role = "Lecture",
            };
            if (eid == null || _context.Courses == null || cid == null || _context.Explodes == null)
            {
                return NotFound();
            }

            var explode = await _context.Explodes
                .FirstOrDefaultAsync(m => m.Id == eid);

            var course = await _context.Courses
                .Include(c => c.Explodes)
                .FirstOrDefaultAsync(m => m.Id == cid);
            if (explode == null)
            {
                return NotFound();
            }
            Explode = explode;
            Course = course;
            return Page();
        }
    }
}