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
        public Course Course { get; set; }
        public User currUser { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(long? id)
        {
            currUser = new User()
            {
                Id = 1,
                Role = "Lecture",
            };
            if (currUser == null)
            {
                //return trang dang nhap
                return RedirectToPage("/Authentication/login");
            }
            if (id == null)
            {
                return NotFound();
            }

            Course = await _context.Courses
                .Include(c => c.Explodes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Course == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            currUser = new User()
            {
                Id = 1,
                Role = "Lecture",
            };
            if (currUser == null)
            {
                //return trang dang nhap
                return RedirectToPage("/Authentication/login");
            }
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var course = await _context.Courses.FindAsync(id);
            if (course != null)
            {
                if (currUser.Role.Equals("Lecture") && currUser.Id == course.UserId)
                {
                    course.Title = Course.Title;
                    course.Thumbnail = Course.Thumbnail;
                    course.Description = Course.Description;

                    _context.Update(course);
                    await _context.SaveChangesAsync();


                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(long? id)
        {
            currUser = new User()
            {
                Id = 1,
                Role = "Lecture",
            };
            if (currUser == null)
            {
                //return trang dang nhap
                return RedirectToPage("/Authentication/login");
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

        private bool CourseExists(long id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
