using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
using Org.BouncyCastle.Utilities.Bzip2;
using ProjectPRN221.Models;
using System.Dynamic;
using System.Security.Policy;
using System.Text.RegularExpressions;

namespace ProjectPRN221.Pages.Admin
{
    public class EditExplodeModel : PageModel
    {
        private readonly int RecordPerPage = 5;
        public static int maxPage;

        private readonly PROJECT_PRUContext dbcontext = new PROJECT_PRUContext();
        private readonly IDistributedCache _cache;

        public EditExplodeModel(IDistributedCache _cache)
        {
            this._cache = _cache;
        }
        public async Task<IActionResult> OnGet(int CourseId)
        {

            String email = HttpContext.Session.GetString("adminEmail");
            if (email == null)
            {
                return RedirectToPage("Login_Cw4B8w6tetCtzk7PQHuZbA==");
            }

            maxPage = dbcontext.Explodes.Where(p => p.CourseId == CourseId).Count() / RecordPerPage;
            if (dbcontext.Explodes.Where(p => p.CourseId == CourseId).Count() % RecordPerPage != 0) maxPage++;
            if (maxPage == 0) maxPage = 1;
            ViewData["CourseName"] = dbcontext.Courses.FirstOrDefault(p => p.Id == CourseId).Title;
            ViewData["MaxPage"] = maxPage;
            ViewData["recordPerPage"] = RecordPerPage;
            ViewData["courseId"] = CourseId;

            return Page();
        }
        public IActionResult OnGetPage(int index, int courseId, string title, string sortBy)
        {
            int skip = (index - 1) * RecordPerPage;

            var explodes = dbcontext.Explodes.Where(p => p.CourseId == courseId)
                                                      .OrderBy(c => c.Id)
                                                      .Include(p => p.Course)
                                                      .Select(c => new
                                                      {
                                                          Id = c.Id,
                                                          CourseName = c.Course.Title,
                                                          Content = c.Content,
                                                          Title = c.Title,
                                                          IsDeleted = c.IsDeleted,
                                                          Video = c.Video
                                                      })
                                                      .ToList();

            if (title != null)
            {
                explodes = explodes.Where(p => p.Title.ToLower().Contains(title.ToLower())).ToList();
            }

            if (sortBy != null && sortBy.Equals("IDDESC")) {
                explodes.Reverse();
            }

            maxPage = explodes.Count() / RecordPerPage;
            if (explodes.Count() % RecordPerPage != 0) maxPage++;
            if (maxPage == 0) maxPage = 1;

            explodes = explodes.Skip(skip).Take(RecordPerPage).ToList();
            return new JsonResult(explodes);
        }

        public String convertFromLinkToID(string youtubeLink)
        {
            var regex = new Regex(@"(?:https?:\/\/)?(?:www\.)?(?:youtube\.com\/(?:[^\/\n\s]+\/\S+\/|(?:v|e(?:mbed)?)\/|.*[?&]v=)|youtu\.be\/)([a-zA-Z0-9_-]{11})", RegexOptions.IgnoreCase);
            var match = regex.Match(youtubeLink);
            return match.Success ? match.Groups[1].Value : youtubeLink;
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

                        explode.Video = convertFromLinkToID(explode.Video);
                    

                    e.Video = explode.Video;

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
        public IActionResult OnGetMaxpage()
        {
            return new JsonResult(maxPage);
        }
        public IActionResult OnPostCreate(Explode explode)
        {
            try
            {
                explode.Video = convertFromLinkToID(explode.Video);
                dbcontext.Explodes.Add(explode);
                dbcontext.SaveChanges();
            }
            catch
            {
                return new JsonResult(new { success = false });
            }
            return new JsonResult(new { success = true });
        }
    }
}
