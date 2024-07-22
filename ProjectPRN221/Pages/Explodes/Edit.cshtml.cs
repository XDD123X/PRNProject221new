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
        public Explode? Explode { get; set; } = default!;
        public User? currUser { get; set; } = default!;
        public Course? Course { get; set; } = default!;

        public bool isCreateExplode { get; set; }   

        public async Task<IActionResult> OnGetAsync(long? eid, long? cid)
        {
            string? currUserID = HttpContext.Session.GetString("Session_User");
            if (currUserID == null || currUserID == "")
            {
                return RedirectToPage("/Authentication/login");
            }
            else
            {
                currUser = _context.Users.FirstOrDefault(c => c.Id == Int32.Parse(currUserID));
            }
            if (cid == null)
            {
                return NotFound();
            }

            Course = await _context.Courses
                .Include(c => c.Explodes)
                .FirstOrDefaultAsync(m => m.Id == cid);
            isCreateExplode = false;
            if (eid == null)
            {
                isCreateExplode = true;
                Explode = new Explode { CourseId = cid.Value };
            }
            else
            {
                Explode = await _context.Explodes
                    .FirstOrDefaultAsync(e => e.Id == eid);
                if (Explode == null)
                {
                    return NotFound();
                }
            }

            if (Course == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostEditAsync(long? eid, long? cid)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("./Edit", new { edi = eid, cid = cid });
            }
            string? currUserID = HttpContext.Session.GetString("Session_User");
            
            if (currUserID == null || currUserID == "")
            {
                return RedirectToPage("/Authentication/login");
            }
            else
            {
                currUser = _context.Users.FirstOrDefault(c => c.Id == Int32.Parse(currUserID));
            }
            var explodeToUpdate = await _context.Explodes.FindAsync(eid);
            if (explodeToUpdate != null)
            {
                explodeToUpdate.Title = Explode.Title;
                explodeToUpdate.Video = Explode.Video;
                explodeToUpdate.Content = Explode.Content;
                explodeToUpdate.CourseId = cid;
                _context.Update(explodeToUpdate);
                await _context.SaveChangesAsync();
            }
            return RedirectToPage("./Edit", new { edi = eid, cid = cid });
        }

        public async Task<IActionResult> OnPostCreateAsync(long cid)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage("./Edit", new {cid = cid });
            }
            string? currUserID = HttpContext.Session.GetString("Session_User");
            if (currUserID == null || currUserID == "")
            {
                return RedirectToPage("/Authentication/login");
            }
            else
            {
                currUser = _context.Users.FirstOrDefault(c => c.Id == Int32.Parse(currUserID));
            }
            try
            {
                var explodeToCreate = new Explode();
                explodeToCreate.Title = Explode.Title;
                explodeToCreate.Video = Explode.Video;
                explodeToCreate.Content = Explode.Content;
                explodeToCreate.CourseId = cid;
                _context.Explodes.Add(explodeToCreate);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Edit", new { cid = cid });
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(long? eid)
        {
            if (eid == null)
            {
                return NotFound();
            }
            string? currUserID = HttpContext.Session.GetString("Session_User");

            if (currUserID == null || currUserID == "")
            {
                return RedirectToPage("/Authentication/login");
            }
            else
            {
                currUser = _context.Users.FirstOrDefault(c => c.Id == Int32.Parse(currUserID));
            }
            var explode = await _context.Explodes.FindAsync(eid);

            if (explode != null)
            {
                _context.Explodes.Remove(explode);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Edit", new { cid = explode.CourseId });
            }
            return NotFound();
            
        }

        private bool ExplodeExists(long id)
        {
            return _context.Explodes.Any(e => e.Id == id);
        }
    }
}
