using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Org.BouncyCastle.Utilities.Bzip2;
using ProjectPRN221.Models;
using System.Dynamic;

namespace ProjectPRN221.Pages.Admin
{
    public class EditExplodeModel : PageModel
    {
        private readonly int RecordPerPage = 5;
        private int maxPage;

        private readonly PROJECT_PRUContext dbcontext = new PROJECT_PRUContext();
        private readonly IDistributedCache _cache;

        public EditExplodeModel(IDistributedCache _cache)
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

            maxPage = dbcontext.Explodes.Where(p => p.CourseId == CourseId).Count() / RecordPerPage;
            if (dbcontext.Explodes.Where(p => p.CourseId == CourseId).Count() % RecordPerPage != 0) maxPage++;
            ViewData["CourseName"] = dbcontext.Courses.FirstOrDefault(p => p.Id == CourseId).Title;
            ViewData["MaxPage"] = maxPage;
            ViewData["recordPerPage"] = RecordPerPage;
            ViewData["courseId"] = CourseId;

            return Page();
        }
        public IActionResult OnGetPage(int index, int courseId)
        {
            int skip = (index - 1) * RecordPerPage;

            var explodes = dbcontext.Explodes.Where(p => p.CourseId == courseId)
                                                      .OrderBy(c => c.Id)
                                                      .Skip(skip)
                                                      .Take(RecordPerPage)
                                                      .Include(p => p.Course)
                                                      .Select(c => new
                                                      {
                                                          Id = c.Id,
                                                          CourseName = c.Course.Title,
                                                          Content = c.Content,
                                                          Title = c.Title,
                                                          IsDeleted = c.IsDeleted
                                                      })
                                                      .ToList();

            return new JsonResult(explodes);
        }

        public IActionResult OnPostEdit(Explode explode)
        {
            try
            {
                Explode e = dbcontext.Explodes.FirstOrDefault(p => p.Id == explode.Id);
                if (e != null)
                {
                    e.Title = explode.Title;
                    e.Content = explode.Content;
                    e.IsDeleted = explode.IsDeleted;

                    dbcontext.Update(e);
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
