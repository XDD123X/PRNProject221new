using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Quiz
{
    public class QuizAnswer
    {
        public int QuestionNumber { get; set; }
        public int SelectedOption { get; set; }
    }

    public class ListsModel : PageModel
    {
        PROJECT_PRUContext DBContext = new PROJECT_PRUContext();
        public void OnGet()
        {
            ViewData["course"] = DBContext.Courses.Include(p => p.Quizzes).First();
            ViewData["test"] = -1;
            ViewData["isViewing"] = false;
            
        }
  
        public IActionResult OnPostAnswer([FromBody] List<QuizAnswer> Answers)
        {
            ViewData["course"] = DBContext.Courses.Include(p => p.Quizzes).First();
            ViewData["isViewing"] = false;

            // Process the submitted answers
            foreach (var answer in Answers)
            {
                // Your logic to process each answer
                Console.WriteLine($"Question {answer.QuestionNumber}: Selected Option {answer.SelectedOption}");
            }

            ViewData["test"] = Answers.Count;
            return Page();
        }
    }
}
