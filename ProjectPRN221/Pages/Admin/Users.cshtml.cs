using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Org.BouncyCastle.Tls;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Admin
{
    public class UsersModel : PageModel
    {
        private readonly int RecordPerPage = 5;
        public static int maxPage;

        private readonly PROJECT_PRUContext dbcontext = new PROJECT_PRUContext();
        private readonly IDistributedCache _cache;
        public UsersModel(IDistributedCache _cache)
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

            maxPage = dbcontext.Users.Count() / RecordPerPage;
            if (dbcontext.Users.Count() % RecordPerPage != 0) maxPage++;
            ViewData["MaxPage"] = maxPage;
            ViewData["recordPerPage"] = RecordPerPage;
            ViewData["roles"] = dbcontext.Users.Select(p => p.Role).Distinct().ToList();    

            return Page();
        }

        public IActionResult OnGetPage(int index, string search, string orderBy, string role)
        {
            int skip = (index - 1) * RecordPerPage;

            var users = dbcontext.Users.OrderBy(u => u.Id)
                                                      .Select(p => new
                                                      {
                                                          Id = p.Id,
                                                          Email = p.Email,
                                                          Role = p.Role,
                                                          Username = p.Username,
                                                          CreatedDate = p.CreatedAt.ToShortDateString(),
                                                          UpdatedDate = p.UpdatedAt.ToShortDateString(),
                                                          IsDeleted = p.IsDeleted
                                                      })
                                                      .ToList();

            if (search != null)
            {
                users = users.Where(p => p.Username.ToLower().Contains(search.ToLower()) || p.Email.ToLower().Contains(search.ToLower())).ToList();
            }


            if (role != null)
            {
                users = users.Where(p => p.Role.Equals(role)).ToList();
            }

            if (orderBy != null)
            {
                switch (orderBy)
                {
                    case "IDASC":
                        {
                            users = users.OrderBy(p => p.Id).ToList();
                            break;
                        }
                    case "IDDESC":
                        {
                            users = users.OrderBy(p => p.Id).Reverse().ToList();
                            break;
                        }
                    case "CreatedDateASC":
                        {
                            users = users.OrderBy(p => p.CreatedDate).ToList();
                            break;
                        }
                    case "CreatedDateDESC":
                        {
                            users = users.OrderBy(p => p.CreatedDate).Reverse().ToList();
                            break;
                        }
                    case "UpdatedDateASC":
                        {
                            users = users.OrderBy(p => p.UpdatedDate).ToList();
                            break;
                        }
                    case "UpdatedDateDESC":
                        {
                            users = users.OrderBy(p => p.UpdatedDate).Reverse().ToList();
                            break;
                        }
                }
            }


            maxPage = users.Count() / RecordPerPage;
            if (users.Count() % RecordPerPage != 0) maxPage++;

            users = users.Skip(skip).Take(RecordPerPage).ToList();
            return new JsonResult(users);
        }

        public IActionResult OnPostEdit(User user)
        {
            try
            {
                User e = dbcontext.Users.FirstOrDefault(p => p.Id == user.Id);
                if (e != null)
                {
                    e.Email = user.Email;
                    e.IsDeleted = user.IsDeleted;
                    e.Role = user.Role;
                    e.Username = user.Username;
                    e.UpdatedAt = DateTime.Now;

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


        public IActionResult OnGetMaxPage()
        {
            return new JsonResult(maxPage);
        }
    }
}
