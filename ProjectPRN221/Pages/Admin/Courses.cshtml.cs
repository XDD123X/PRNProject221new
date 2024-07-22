using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Models;
using System.Globalization;

namespace ProjectPRN221.Pages.Admin
{
    public class CoursesModel : PageModel
    {
        private readonly int RecordPerPage = 5;

        public static int maxPage;

        private readonly PROJECT_PRUContext dbcontext = new PROJECT_PRUContext();
        private readonly IDistributedCache _cache;

        public CoursesModel(IDistributedCache _cache)
        {
            this._cache = _cache;
        }
        public async Task<IActionResult> OnGet()
        {
            String email = await _cache.GetStringAsync("adminEmail");
            if (email == null)
            {
                return RedirectToPage("Login_Cw4B8w6tetCtzk7PQHuZbA==");
            }

            maxPage = dbcontext.Courses.Count() / RecordPerPage;
            if (dbcontext.Courses.Count() % RecordPerPage != 0) maxPage++;
            ViewData["MaxPage"] = maxPage;
            ViewData["recordPerPage"] = RecordPerPage;

            ViewData["categories"] = dbcontext.Courses.Select(p => p.Categories).Distinct().ToList();

            return Page();
        }

        public IActionResult OnPostEdit(Course course)
        {
            try
            {
                Course c = dbcontext.Courses.FirstOrDefault(p => p.Id == course.Id);

                if (c != null)
                {

                    c.Title = course.Title;
                    c.Thumbnail = course.Thumbnail;
                    c.Categories = course.Categories;
                    c.Description = course.Description;
                    c.Price = course.Price;
                    c.CreatedAt = course.CreatedAt;
                    c.UpdatedAt = DateTime.Now;
                    c.IsDeleted = course.IsDeleted;
                    c.IsActived = course.IsActived;

                    dbcontext.Update(c);
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

        public IActionResult OnGetPage(int index, string category, string sortBy, string title, Boolean isAsc)
        {
            int skip = (index - 1) * RecordPerPage;

            var courses = dbcontext.Courses.OrderBy(c => c.Id)
                                                      .Include(p => p.User)
                                                      .Select(c => new
                                                      {
                                                          Id = c.Id,
                                                          Creator = c.User.Username,
                                                          Title = c.Title,
                                                          Thumbnail = c.Thumbnail,
                                                          Category = c.Categories,
                                                          Description = c.Description,
                                                          Price = c.Price,
                                                          CreatedDate = c.CreatedAt.GetValueOrDefault().ToShortDateString(),
                                                          UpdateDate = c.UpdatedAt.GetValueOrDefault().ToShortDateString(),
                                                          TotalLearner = c.EnrolNums,
                                                          IsActive = c.IsActived,
                                                          IsDeleted = c.IsDeleted
                                                      })
                                                      .ToList();
            
            if (category != null)
            {
                courses = courses.Where(p => p.Category.ToLower().Equals(category.ToLower())).ToList();
            }
            if (title != null)
            {
                courses = courses.Where(p => p.Title.ToLower().Contains(title.ToLower())).ToList();
            }
            if (sortBy != null)
            {
                switch (sortBy)
                {
                    case "IDASC":
                        {
                            courses = courses.OrderBy(p => p.Id).ToList();
                            break;
                        }
                    case "IDDESC":
                        {
                            courses = courses.OrderBy(p => p.Id).Reverse().ToList();
                            break;
                        }
                    case "CreatedDateASC":
                        {
                            courses = courses.OrderBy(p => p.CreatedDate).ToList();
                            break;
                        }
                    case "CreatedDateDESC":
                        {
                            courses = courses.OrderBy(p => p.CreatedDate).Reverse().ToList();
                            break;
                        }
                    case "UpdatedDateASC":
                        {
                            courses = courses.OrderBy(p => p.UpdateDate).ToList();
                            break;
                        }
                    case "UpdatedDateDESC":
                        {
                            courses = courses.OrderBy(p => p.UpdateDate).Reverse().ToList();
                            break;
                        }
                }
            }

            maxPage = courses.Count() / RecordPerPage;
            if (courses.Count() % RecordPerPage != 0) maxPage++;

            courses = courses.Skip(skip).Take(RecordPerPage).ToList(); 

            return new JsonResult(courses);
        }
        public IActionResult OnPostCreate(Course course)
        {
            try
            {
                course.CreatedAt = DateTime.Now;
                course.UpdatedAt = DateTime.Now;
                dbcontext.Courses.Add(course);
                dbcontext.SaveChanges();
            }
            catch
            {
                return new JsonResult(new { success = false });
            }
            return new JsonResult(new { success = true });
        }

        public IActionResult OnGetAuthor()
        {
            var author = dbcontext.Users.ToList();
            return new JsonResult(author);
        }
        public IActionResult OnGetMaxPage()
        {
            return new JsonResult(maxPage);
        }
    }
}
