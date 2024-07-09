using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Assignment
{
    public class AssignmentModel : PageModel
    {

        PROJECT_PRUContext DBContext = new PROJECT_PRUContext();
        int UserId;
        public IActionResult OnGet(long CourseID, int UserID, Boolean IsViewing)
        {
            Course course = DBContext.Courses.Include(p => p.Quizzes).FirstOrDefault(p => p.Id == CourseID);
            if (course == null)
            {
                return RedirectToPage("/Courses/Index");
            }

            if (IsViewing)
            {
                ViewData["history"] = DBContext.HistoryQuizzes.Include(p => p.Quizz).Where(p => p.Quizz.CourseId == CourseID).ToList();
            }
            this.UserId = UserId;
            ViewData["course"] = DBContext.Courses.Include(p => p.Quizzes).FirstOrDefault(p => p.Id == CourseID);
            ViewData["isViewing"] = IsViewing;
            return Page();
        }

        public IActionResult OnPostAnswer()
        {
            List<Quiz> list = DBContext.Courses.Include(p => p.Quizzes).First().Quizzes.ToList();
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
                        UserId = UserId,
                        QuizzId = quiz.Id,
                        Answer = selectedAnswer,
                    };
                    DBContext.Add(hq);
                } else
                {
                    HistoryQuiz hq = new HistoryQuiz()
                    {
                        UserId = UserId,
                        QuizzId = quiz.Id,
                        Answer = -1,
                    };
                    DBContext.Add(hq);
                }
            }
            Console.WriteLine(point);

            DBContext.SaveChanges();

            return RedirectToPage("Index");
        }
    }
}
