using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectPRN221.Pages.Explodes
{
    public class EditModel : PageModel
    {
        private readonly ProjectPRN221.Models.PROJECT_PRUContext _context = new PROJECT_PRUContext();

        [BindProperty]
        public Explode Explode { get; set; } = default!;
        public User currUser { get; set; } = default!;
        public Course Course { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? eid, long? cid)
        {
            //string currUserID = HttpContext.Session.GetString("Session_User");
            //if (currUserID == null || currUserID == "")
            //{
            //    return RedirectToPage("/Authentication/login");
            //}
            //else
            //{
            //    currUser = _context.Users.FirstOrDefault(c => c.Id == Int32.Parse(currUserID));
            //}
            if (eid == null || cid == null)
            {
                return NotFound();
            }

            var explode = await _context.Explodes.FirstOrDefaultAsync(m => m.Id == eid);
            var course = await _context.Courses.Include(c => c.Explodes).FirstOrDefaultAsync(m => m.Id == cid);

            if (explode == null || course == null)
            {
                return NotFound();
            }

            Explode = explode;
            Course = course;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Explode).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExplodeExists(Explode.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { eid = Explode.Id, cid = Course.Id });
        }

        public async Task<IActionResult> OnPostDeleteAsync(long? eid)
        {
            if (eid == null)
            {
                return NotFound();
            }

            var explode = await _context.Explodes.FindAsync(eid);

            if (explode != null)
            {
                _context.Explodes.Remove(explode);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { cid = Course.Id });
        }

        private bool ExplodeExists(long id)
        {
            return _context.Explodes.Any(e => e.Id == id);
        }
    }
}
