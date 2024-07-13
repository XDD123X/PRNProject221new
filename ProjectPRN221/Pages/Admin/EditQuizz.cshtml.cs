using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Admin
{
    public class EditQuizzModel : PageModel
    {
        private readonly int RecordPerPage = 5;
        private int maxPage;

        private readonly PROJECT_PRUContext dbcontext = new PROJECT_PRUContext();
        private readonly IDistributedCache _cache;

        public EditQuizzModel(IDistributedCache _cache)
        {
            this._cache = _cache;
        }
        public async Task<IActionResult> OnGet(int CourseId)
        {

            String email = await _cache.GetStringAsync("adminEmail");
            if (email == null)
            {
                return RedirectToPage("Login_Cw4B8w6tetCtzk7PQHuZbA==");
            }

            maxPage = dbcontext.Quizzes.Where(p => p.CourseId == CourseId).Count() / RecordPerPage;
            if (dbcontext.Quizzes.Where(p => p.CourseId == CourseId).Count() % RecordPerPage != 0) maxPage++;
            ViewData["CourseName"] = dbcontext.Courses.FirstOrDefault(p => p.Id == CourseId).Title;
            ViewData["MaxPage"] = maxPage;
            ViewData["recordPerPage"] = RecordPerPage;
            ViewData["CourseID"] = CourseId;

            return Page();
        }
        public IActionResult OnGetPage(int index, int courseId)
        {
            int skip = (index - 1) * RecordPerPage;

            var quizzes = dbcontext.Quizzes.Where(p => p.CourseId == courseId)
                                                      .OrderBy(c => c.Id)
                                                      .Skip(skip)
                                                      .Take(RecordPerPage)
                                                      .Include(p => p.Course)
                                                      .Select(c => new
                                                      {
                                                          Id = c.Id,
                                                          CourseName = c.Course.Title,
                                                          Question = c.Question,
                                                          Option1 = c.Option1,
                                                          Option2 = c.Option2,
                                                          Option3 = c.Option3,
                                                          Option4 = c.Option4,
                                                          Answer = c.Answer,
                                                          IsDeleted = c.IsDeleted
                                                      })
                                                      .ToList();

            return new JsonResult(quizzes);
        }

        public IActionResult OnPostEdit(Quiz quiz)
        {
            try
            {
                Quiz q = dbcontext.Quizzes.FirstOrDefault(p => p.Id == quiz.Id);
                if (q != null)
                {
                    q.Answer = quiz.Answer;
                    q.Question = quiz.Question;
                    q.Option1 = quiz.Option1;
                    q.Option2 = quiz.Option2;
                    q.Option3 = quiz.Option3;
                    q.Option4 = quiz.Option4;

                    q.IsDeleted = quiz.IsDeleted;

                    dbcontext.Update(q);
                    dbcontext.SaveChanges();
                }
                else
                {
                    return new JsonResult(new { success = false });
                }
            }
            catch
            {
                return new JsonResult(new { success = false });

            }
            return new JsonResult(new { success = true });
        }
    }
}