using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Courses
{
    public class EditModel : PageModel
    {
        private readonly ProjectPRN221.Models.PROJECT_PRUContext _context = new PROJECT_PRUContext();

        [BindProperty]
        public Course? Course { get; set; } = default!;
        public User? currUser { get; set; } = default!;
        public string? ErrorMessage { get; set; } = default;


        public async Task<IActionResult> OnGetAsync(long? id)
        {
            string? currUserID = HttpContext.Session.GetString("Session_User");

            if (currUserID == null || currUserID == "")
            {
                return RedirectToPage("/Authentication/login");
            }
            else
            {
                currUser = _context.Users.FirstOrDefault(c => c.Id == long.Parse(currUserID));
            }
            var course = await _context.Courses
                .Include(c => c.Explodes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }
            if (currUser.Role.Equals("Lecture") && currUser.Id == course.UserId)
            {
                Course = course;
                return Page();
            }
            else
            {
                ErrorMessage = "You do not have permission to edit this course.";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            string? currUserID = HttpContext.Session.GetString("Session_User");

            if (currUserID == null || currUserID == "")
            {
                return RedirectToPage("/Authentication/login");
            }
            else
            {
                currUser = _context.Users.FirstOrDefault(c => c.Id == long.Parse(currUserID));
            }
            if (!ModelState.IsValid)
            {
                return RedirectToPage("./Edit", new { id = id });
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                if (currUser.Role.Equals("Lecture") && currUser.Id == course.UserId)
                {
                    course.Title = Course.Title;
                    course.Thumbnail = Course.Thumbnail;
                    course.Description = Course.Description;
                    course.UpdatedAt = DateTime.Now;

                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToPage("./Edit", new { id = id });
        }

        public async Task<IActionResult> OnPostDeleteAsync(long? id)
        {
            string? currUserID = HttpContext.Session.GetString("Session_User");

            if (currUserID == null || currUserID == "")
            {
                return RedirectToPage("/Authentication/login");
            }
            else
            {
                currUser = _context.Users.FirstOrDefault(c => c.Id == long.Parse(currUserID));
            }

            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);

            if (course != null)
            {
                if (currUser.Role.Equals("Lecture") && currUser.Id == course.UserId)
                {
                    _context.Courses.Remove(course);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteExplodeAsync(long explodeId)
        {
            string? currUserID = HttpContext.Session.GetString("Session_User");

            if (currUserID == null || currUserID == "")
            {
                return RedirectToPage("/Authentication/login");
            }
            else
            {
                currUser = _context.Users.FirstOrDefault(c => c.Id == long.Parse(currUserID));
            }
            var explode = await _context.Explodes.FindAsync(explodeId);
            if (explode != null)
            {
                var course = await _context.Courses.FindAsync(explode.CourseId);
                if (course != null && currUser.Role.Equals("Lecture") && currUser.Id == course.UserId)
                {
                    _context.Explodes.Remove(explode);
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Edit", new { id = explode.CourseId });
                }
            }
            return NotFound();
        }

        private bool CourseExists(long id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
