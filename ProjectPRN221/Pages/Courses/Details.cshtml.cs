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
        public Course? Course { get; set; } = default!;
        public User? Lecture { get; set; } = default!;
        public User? currUser { get; set; } = default!;
        public IList<Quiz> lisQuiz { get; set; } = default!;
        public long Enrolled { get; set; } = default!;
        public long Duration { get; set; } = default!;
        public bool isEndrolled { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {

            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }
            string? currUserID = HttpContext.Session.GetString("Session_User");

            currUserID = "2";

            if (currUserID == null || currUserID == "")
            {
                currUser = null;
                isEndrolled = false;
            }
            else
            {
                currUser = _context.Users.FirstOrDefault(c => c.Id == Int32.Parse(currUserID));
                if (_context.EnroledCourses.FirstOrDefault(ec => ec.CourseId == id && ec.UserId == currUser.Id) == null)
                {
                    isEndrolled = false;
                }
                else
                {
                    isEndrolled = true;
                }
            }
            var course = await _context.Courses
                .Include(c => c.Explodes)
                .FirstOrDefaultAsync(m => m.Id == id);
            Enrolled = await _context.EnroledCourses.Where(e => e.CourseId == id).CountAsync();

            Duration = await _context.Explodes.Where(e => e.CourseId != id).CountAsync();

            lisQuiz = await _context.Quizzes.Where(q => q.CourseId == id).ToListAsync();

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
            string? currUserID = HttpContext.Session.GetString("Session_User");
            if (currUserID == null || currUserID == "")
            {
                return RedirectToPage("/Authentication/login");
            }
            else
            {
                currUser = _context.Users.FirstOrDefault(c => c.Id == Int32.Parse(currUserID));
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
            var currUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == uid);
            if (currUser == null)
            {
                return RedirectToPage("/Authentication/login");
            }
            if (currUser.Role.Equals("Student"))
            {
                var existingEnrollment = await _context.EnroledCourses
                    .FirstOrDefaultAsync(e => e.CourseId == cid && e.UserId == uid);

                if (existingEnrollment == null)
                {
                    var enrollment = new EnroledCourse
                    {
                        CourseId = cid,
                        UserId = uid,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        IsDeleted = false,
                    };

                    _context.EnroledCourses.Add(enrollment);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    TempData["Message"] = "You are already enrolled in this course.";
                }
            }

            return RedirectToPage(new { id = cid });
        }

        public async Task<IActionResult> OnPostUnenrollAsync(long? uid, long? cid)
        {
            if (uid == null || cid == null)
            {
                return NotFound();
            }
            var currUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == uid);
            if (currUser == null)
            {
                return RedirectToPage("/Authentication/login");
            }
            if (currUser.Role.Equals("Student"))
            {
                var enrollment = await _context.EnroledCourses
                    .FirstOrDefaultAsync(e => e.CourseId == cid && e.UserId == uid);

                if (enrollment != null)
                {
                    _context.EnroledCourses.Remove(enrollment);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    TempData["Message"] = "You are not enrolled in this course.";
                }
            }

            return RedirectToPage(new { id = cid });
        }
    }
}
