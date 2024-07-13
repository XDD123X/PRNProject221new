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
        public User currUser { get; set; } = default!;
        public long Enrolled { get; set; } = default!;
        public long Duration { get; set; } = default!;

        public bool isEndrolled { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            //lay user hien tai
            currUser = new User()
            {
                Id = 2,
                Role = "Student",
            };
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Explodes)
                .FirstOrDefaultAsync(m => m.Id == id);
            Enrolled = await _context.EnroledCourses.Where(e => e.CourseId == id).CountAsync();
            if (_context.EnroledCourses.FirstOrDefault(ec => ec.CourseId == id && ec.UserId == currUser.Id) == null)
            {
                isEndrolled = false;
            }
            else
            {
                isEndrolled = true;
            }
            Duration = await _context.Explodes.Where(e => e.CourseId != id).CountAsync();

            var lecture = await _context.Users
                            .FirstOrDefaultAsync(l => l.Id == course.UserId);
            if (course == null && lecture == null)
            {
                return NotFound();
            }

            Course = course;
            Lecture = lecture;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            return RedirectToPage("./Index");
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
        public async Task<IActionResult> OnPostEnrollAsync(long? uid, long? cid)
        {
            if (uid == null || cid == null)
            {
                return NotFound();
            }
            currUser = _context.Users.FirstOrDefault(u => u.Id == uid);
            if (currUser == null)
            {
                //return trang dang nhap
                return RedirectToPage("/Authentication/login");
            }
            if (currUser.Role.Equals("Student"))
            {
                var enrollment = new EnroledCourse
                {
                    CourseId = cid,
                    UserId = uid,
                    CreatedAt = DateTime.Now
                };

                _context.EnroledCourses.Add(enrollment);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage(new { id = cid });
        }
    }
}
