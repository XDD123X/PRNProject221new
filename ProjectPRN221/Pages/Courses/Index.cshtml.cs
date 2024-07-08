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

        public IList<Course> Course { get;set; } = default!;

        public async Task OnGetAsync()
        {
            //check role teacher
            if (_context.Courses != null)
            {
                Course = await _context.Courses
                .Include(c => c.User).ToListAsync();
            }
        }
    }
}
