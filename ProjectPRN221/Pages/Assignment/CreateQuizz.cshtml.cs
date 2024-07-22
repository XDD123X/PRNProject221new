using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Assignment
{
    public class CreateQuizzModel : PageModel
    {
        PROJECT_PRUContext dbcontext = new PROJECT_PRUContext();
        public IActionResult OnGet(int courseId)
        {
            //Course c = dbcontext.Courses.FirstOrDefault(p => p.Id == courseId);
            //if (c == null) return RedirectToPage("/Courses/Index");

            //string? currUserID = HttpContext.Session.GetString("Session_User");
            //if (currUserID == null) return RedirectToPage("/Authentication/login");
            //User u = dbcontext.Users.FirstOrDefault(p => p.Id == int.Parse(currUserID));
            //if (u.Role != "Lecture" && u.Id != c.Id)
            //{
            //    return RedirectToAction("/Courses/Index");
            //}

            ViewData["courseId"] = courseId;

            ViewData["quizz"] = dbcontext.Quizzes.Where(p => p.CourseId == courseId).ToList();

            return Page();
        }

        public IActionResult OnPostToggle(int quizzId)
        {
            Quiz quiz = dbcontext.Quizzes.FirstOrDefault(p => p.Id == quizzId);
            if (quiz != null)
            {
                quiz.IsDeleted = !quiz.IsDeleted;
                dbcontext.Quizzes.Update(quiz);
                dbcontext.SaveChanges();

                return new JsonResult(new { success = true });
            }
            else
            {

                return new JsonResult(new { success = false });
            }
        }

        public IActionResult OnPostEdit(Quiz quiz)
        {
            try
            { 
                Quiz find = dbcontext.Quizzes.FirstOrDefault(p => p.Id == quiz.Id);
                find.Question = quiz.Question;
                find.Option1 = quiz.Option1;
                find.Option2 = quiz.Option2;
                find.Option3 = quiz.Option3;
                find.Option4 = quiz.Option4;
                find.Answer = quiz.Answer;
                Console.WriteLine("TAO LAM DC");
                dbcontext.Update(find);
                dbcontext.SaveChanges();
                return new JsonResult(new { success = true });
            }
            catch
            {

                return new JsonResult(new { success = false });
            }
        }

        public IActionResult OnPostCreate(Quiz quiz)
        {
            try
            {
                dbcontext.Quizzes.Add(quiz);
                dbcontext.SaveChanges();
                return new JsonResult(new { success = true });
            }
            catch
            {

                return new JsonResult(new { success = false });
            }
        }
    }
}
