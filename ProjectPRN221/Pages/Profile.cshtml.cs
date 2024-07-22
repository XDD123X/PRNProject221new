using ClosedXML.Excel;
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
                    if(user.Role == "Student")
                    {
						List<float> coursePoints = new List<float>();

						List<Course> courses = new List<Course>();
						foreach (EnroledCourse e in user.EnroledCourses)
						{
							Course course = _context.Courses.Include(t => t.User).FirstOrDefault(x => x.Id == e.CourseId);
							if (course != null)
							{
								courses.Add(course);
								coursePoints.Add(getPoint(userId, course.Id));
							}

						}

						List<EnroledCourse> enroledCourses = user.EnroledCourses.ToList();
						ViewData["points"] = coursePoints;
						ViewData["courses"] = courses;
						ViewData["enroled_courses"] = enroledCourses;
					} else
                    {
                        List<Course> courses = _context.Courses.Where(t => t.UserId == userId).ToList();
                        List<int> enroleNums = new List<int>();
                        foreach(var course in courses)
                        {
                            var list = _context.EnroledCourses.Where(t => t.CourseId == course.Id).ToList();
                            if (list.Any())
                            {
                                enroleNums.Add(list.Count);
                            } else
                            {
                                enroleNums.Add(0);
                            }
                        }

                        ViewData["enroleNums"] = enroleNums;
						ViewData["courses"] = courses;
                        
					}
                    
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

        public IActionResult OnGetExportCoursesStudent()
        {
            var sessionValue = HttpContext.Session.GetString("Session_User");
            if (sessionValue != null)
            {
                int userId = int.Parse(sessionValue);
                using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
                {
                    var user = _context.Users.Include(t => t.EnroledCourses).FirstOrDefault(t => t.Id == userId);

                    List<Course> courses = new List<Course>();
                    List<float> coursePoints = new List<float>();
                    List<EnroledCourse> enroledCourses = user.EnroledCourses.ToList();
                    if (user.Role == "Student")
                    {
                        foreach (EnroledCourse e in user.EnroledCourses)
                        {
                            Course course = _context.Courses.Include(t => t.User).FirstOrDefault(x => x.Id == e.CourseId);
                            if (course != null)
                            {
                                courses.Add(course);
                                coursePoints.Add(getPoint(userId, course.Id));
                            }
                        }
                    }
                    else
                    {
                        courses = _context.Courses.Where(t => t.UserId == userId).ToList();
                    }

                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Courses");

                        worksheet.Cell(1, 1).Value = "Course Title";
                        worksheet.Cell(1, 2).Value = "Teacher";
                        worksheet.Cell(1, 3).Value = "Enrole Date";
                        worksheet.Cell(1, 4).Value = "Your Score";

                        for (int i = 0; i < courses.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = courses[i].Title;
                            worksheet.Cell(i + 2, 2).Value = courses[i].User.Username;
                            worksheet.Cell(i + 2, 3).Value = enroledCourses[i].CreatedAt;
                            worksheet.Cell(i + 2, 4).Value = coursePoints[i] +  "/ 10";
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Courses.xlsx");
                        }
                    }
                }
            }

            return RedirectToPage("Authentication/login");
        }

        public IActionResult OnGetExportCoursesLecturer()
        {
            var sessionValue = HttpContext.Session.GetString("Session_User");
            if (sessionValue != null)
            {
                int userId = int.Parse(sessionValue);
                using (PROJECT_PRUContext _context = new PROJECT_PRUContext())
                {
                    var user = _context.Users.Include(t => t.EnroledCourses).FirstOrDefault(t => t.Id == userId);
                    List<Course> courses = _context.Courses.Where(t => t.UserId == userId).ToList();
                    List<int> enroleNums = new List<int>();
                    foreach (var course in courses)
                    {
                        var list = _context.EnroledCourses.Where(t => t.CourseId == course.Id).ToList();
                        if (list.Any())
                        {
                            enroleNums.Add(list.Count);
                        }
                        else
                        {
                            enroleNums.Add(0);
                        }
                    }


                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Courses");

                        worksheet.Cell(1, 1).Value = "Course Title";
                        worksheet.Cell(1, 2).Value = "Created Date";
                        worksheet.Cell(1, 3).Value = "Enrole Nums";

                        for (int i = 0; i < courses.Count; i++)
                        {
                            worksheet.Cell(i + 2, 1).Value = courses[i].Title;
                            worksheet.Cell(i + 2, 2).Value = courses[i].CreatedAt;
                            worksheet.Cell(i + 2, 3).Value = enroleNums[i];
                        }

                        using (var stream = new MemoryStream())
                        {
                            workbook.SaveAs(stream);
                            var content = stream.ToArray();

                            return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Courses.xlsx");
                        }
                    }
                }
            }

            return RedirectToPage("Authentication/login");
        }
    }
}
