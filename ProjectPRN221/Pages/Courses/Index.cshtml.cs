using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProjectPRN221.Models;

namespace ProjectPRN221.Pages.Courses
{
    public class IndexModel : PageModel
    {
        private readonly ProjectPRN221.Models.PROJECT_PRUContext _context = new PROJECT_PRUContext();

        public IList<Course> Course { get; set; } = default!;
        public IList<User> Lecture { get; set; } = default!;
        public IList<string> Categories { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string Title { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string Category { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public string Instructor { get; set; } = default!;

        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 3;

        public int TotalPages { get; set; }

        public User? currUser { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            string? currUserID = HttpContext.Session.GetString("Session_User");
            if (currUserID == null)
            {
                currUser = null;
            }
            else
            {
                currUser = _context.Users.FirstOrDefault(c => c.Id == long.Parse(currUserID));
            }

            if (_context.Courses != null)
            {
                var query = _context.Courses.Include(c => c.User).AsQueryable();

                if (!string.IsNullOrEmpty(Title))
                {
                    query = query.Where(c => c.Title.Contains(Title));
                }

                if (!string.IsNullOrEmpty(Category))
                {
                    query = query.Where(c => c.Categories == Category);
                }

                if (!string.IsNullOrEmpty(Instructor))
                {
                    query = query.Where(c => c.UserId.ToString() == Instructor);
                }

                var totalCourses = await query.CountAsync();
                TotalPages = (int)Math.Ceiling(totalCourses / (double)PageSize);

                Course = await query
                    .Skip((PageIndex - 1) * PageSize)
                    .Take(PageSize)
                    .ToListAsync();
            }

            if (_context.Users.Where(u => u.Role == "Lecture").Distinct() != null)
            {
                Lecture = await _context.Users.Where(u => u.Role == "Lecture")
                          .Distinct().ToListAsync();
            }

            if (_context.Courses.Select(c => c.Categories).Distinct() != null)
            {
                Categories = await _context.Courses.Select(c => c.Categories)
                             .Distinct().ToListAsync();
            }
            return Page();
        }
    }
}