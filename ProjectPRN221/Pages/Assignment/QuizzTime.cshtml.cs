using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;
using System.Data;
using System.Threading.Tasks.Dataflow;

namespace ProjectPRN221.Pages.Assignment
{
    public class AssignmentModel : PageModel
    {
        PROJECT_PRUContext DBContext = new PROJECT_PRUContext();
        public static long userId;
        public static long courseID;
        public async Task<IActionResult> OnGet(long CourseID, long UserID, Boolean IsViewing)
        {
            // Check if user remain login
            var sessionValue = HttpContext.Session.GetString("Session_User");
            if (sessionValue == null)
            {
                return RedirectToPage("/Authentication/login");
            }
            // Check if course is exist and user was enroll on that course
            Course course = DBContext.Courses.Include(p => p.Quizzes).FirstOrDefault(p => p.Id == CourseID);
            EnroledCourse ec = DBContext.EnroledCourses.FirstOrDefault(p => p.CourseId == CourseID && p.UserId == UserID);

            if (course == null || ec == null)
            {
                return RedirectToPage("/Courses/Index");
            }

            var list = from quizz in DBContext.HistoryQuizzes
                       select UserID;

            int HistoryQuizz = DBContext.HistoryQuizzes.Include(d => d.Quizz).Where(p => p.UserId == UserID && p.Quizz.CourseId == CourseID).Count();
            if (HistoryQuizz > 0)
            {
                IsViewing = true;
            }

            if (IsViewing)
            {
                float points = await GetPoint(UserID, CourseID);
                await Console.Out.WriteLineAsync(points.ToString());
                ViewData["point"] = points;
                ViewData["history"] = DBContext.HistoryQuizzes.Include(p => p.Quizz).Where(p => p.Quizz.CourseId == CourseID).ToList();
            }
            userId = UserID;
            courseID = CourseID;
            ViewData["course"] = DBContext.Courses.Include(p => p.Quizzes).FirstOrDefault(p => p.Id == CourseID);
            ViewData["isViewing"] = IsViewing;
            return Page();
        }

        public async Task<IActionResult> OnPostAnswer()
        {
            List<Quiz> list = DBContext.Courses.Include(p => p.Quizzes).FirstOrDefault(p => p.Id == courseID).Quizzes.ToList();
            float point = 0f;
            foreach (var quiz in list)
            {
                var selectedOption = Request.Form[$"Answers[{quiz.Id}].SelectedOption"];

                if (!string.IsNullOrEmpty(selectedOption) && int.TryParse(selectedOption, out int selectedAnswer))
                {
                    if (quiz.Answer == selectedAnswer)
                    {
                        point += (10f / list.Count);
                    }
                    HistoryQuiz hq = new HistoryQuiz()
                    {
                        UserId = userId,
                        QuizzId = quiz.Id,
                        Answer = selectedAnswer,
                        IsDeleted = false,
                    };
                    DBContext.Add(hq);
                }
                else
                {
                    HistoryQuiz hq = new HistoryQuiz()
                    {
                        UserId = userId,
                        QuizzId = quiz.Id,
                        Answer = -1,
                        IsDeleted = false,
                    };
                    DBContext.Add(hq);
                }
            }

            DBContext.SaveChanges();

            ViewData["point"] = point; 
            ViewData["history"] = DBContext.HistoryQuizzes.Include(p => p.Quizz).Where(p => p.Quizz.CourseId == courseID).ToList();
            ViewData["course"] = DBContext.Courses.Include(p => p.Quizzes).FirstOrDefault(p => p.Id == courseID);
            
            ViewData["isViewing"] = true;

            return Page();
        }

        public async Task<float> GetPoint(long IdUser, long courseId)
        {
            // Define the output parameter
            var pointParam = new SqlParameter
            {
                ParameterName = "@point",
                SqlDbType = SqlDbType.Float,
                Direction = ParameterDirection.Output
            };


            var sql = "EXEC GetPoint " + IdUser + ", " + courseId + ", @point OUT";

            await DBContext.Database.ExecuteSqlRawAsync(sql, pointParam);

            float points = pointParam.Value != DBNull.Value ? (float)(double)pointParam.Value : 0;

            return points;
        }
    }
}
