using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Courses
{
    public class DetailsModel : PageModel
    {
        private readonly ProjectPRN221.Models.PROJECT_PRUContext _context = new PROJECT_PRUContext();
        public Course Course { get; set; } = default!;
        public User Lecture { get; set; } = default!;
        public IList<Explode> Explode { get; set; } = default!;
        public int Enrolled { get; set; } = default!;
        public int Duration { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Explodes)
                .FirstOrDefaultAsync(m => m.Id == id);
            Enrolled = await _context.EnroledCourses.Where(e => e.CourseId == id).CountAsync();

            var explode = await _context.Explodes.Where(e => e.CourseId == id).ToListAsync();

            Duration = await _context.Explodes.Where(e => e.CourseId != id).CountAsync();

            var lecture = await _context.Users
                            .FirstOrDefaultAsync(l => l.Id == course.UserId);
            if (course == null)
            {
                return NotFound();
            }

            Course = course;
            Explode = explode;
            Lecture = lecture;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            if (User.Identity.IsAuthenticated && User.IsInRole("student"))
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;
                if (userId != null)
                {
                    var enrollment = new EnroledCourse
                    {
                        CourseId = (long)id,
                        UserId = long.Parse(userId),
                        CreatedAt = DateTime.Now
                    };

                    _context.EnroledCourses.Add(enrollment);
                    await _context.SaveChangesAsync();
                }
            }
            else if (!User.Identity.IsAuthenticated)
            {
                //trang dang nhap la gì day v:
                return RedirectToPage("/Account/Login");
            }

            return RedirectToPage(new { id = id });
        }
    }
}
