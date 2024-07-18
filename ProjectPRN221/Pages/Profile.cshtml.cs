using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages
{
    public class ProfileModel : PageModel
    {
        public IActionResult OnGet()
        {
			var sessionValue = HttpContext.Session.GetString("Session_User");
            if(sessionValue != null)
            {
                int userId = int.Parse(sessionValue);
                using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
                {
                    var user = _context.Users.Include(t => t.EnroledCourses).FirstOrDefault(t => t.Id == userId);
                    ViewData["user"] = user;

                    List<Course> courses = new List<Course>();
                    foreach(EnroledCourse e in user.EnroledCourses){
                        Course course = _context.Courses.Include(t => t.User).FirstOrDefault(x => x.Id == e.CourseId);
                        courses.Add(course);
                    }
                    ViewData["courses"] = courses;
                    ViewData["enroled_courses"] = user.EnroledCourses;
                }
				return Page();
            }

            return RedirectToPage("Authentication/login");
		}
    }
}
