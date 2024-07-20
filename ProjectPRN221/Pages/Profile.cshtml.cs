using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages
{
    public class ProfileModel : PageModel
    {

        [BindProperty]
        public string? txtUsername { get; set; }
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
                    List<float> coursePoints = new List<float>();

                    List<Course> courses = new List<Course>();
                    foreach(EnroledCourse e in user.EnroledCourses){
                        Course course = _context.Courses.Include(t => t.User).FirstOrDefault(x => x.Id == e.CourseId);
                        if(course != null)
                        {
							courses.Add(course);
							coursePoints.Add(getPoint(userId, course.Id));
						}
                        
                    }

                    List<EnroledCourse> enroledCourses = user.EnroledCourses.ToList();
                    ViewData["points"] = coursePoints;
                    ViewData["courses"] = courses;
                    ViewData["enroled_courses"] = enroledCourses;
                }
				return Page();
            }

            return RedirectToPage("Authentication/login");
		}

        public IActionResult OnPost()
        {
            var sessionValue = HttpContext.Session.GetString("Session_User");
            if (sessionValue != null)
            {
                int userId = int.Parse(sessionValue);
                using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
                {
                    var user = _context.Users.FirstOrDefault(t => t.Id == userId);
                    var user1 = _context.Users.FirstOrDefault(t => t.Username == txtUsername && t.IsDeleted == false);
                    if (user1 !=null)
                    {
                        ViewData["error"] = "Username is already in use";
                        return Page();
                    }

                    user.Username = txtUsername;
                    user.UpdatedAt = DateTime.Now;
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    ViewData["error"] = "Update successful";
                    OnGet();
                }
                return Page();
            }

            return RedirectToPage("Authentication/login");
        }

            private float getPoint(int userId, long courseId)
        {
            float point = 0;
			using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
            {
                var totalQuizs = _context.Quizzes.Include(t => t.Course).
                    Where(t => t.CourseId == courseId).ToList();
                var totalHistoryOfUser = _context.HistoryQuizzes.Include(t => t.User)
                    .Where(t => t.UserId == userId).ToList();

                List<HistoryQuiz> history = new List<HistoryQuiz>();    
                foreach(var quizOfHistory in totalHistoryOfUser)
                {
                    Quiz quiz = null;
                    foreach(Quiz quizz in totalQuizs)
                    {
                        if (quizz.Id == quizOfHistory.QuizzId)
                        {
                            quiz = quizz;
                        }
                    }

                    if (quiz != null && quiz.Answer == quizOfHistory.Answer)
                    {
                        history.Add(quizOfHistory);
                    }
                }

                int totalCount = totalQuizs.Count();
                int rightAnswer = history.Count();

                if(totalQuizs.Count() > 0) {
                    point = (float)rightAnswer / totalCount * 10;
                }

            }

			return point;
        } 
    }
}
