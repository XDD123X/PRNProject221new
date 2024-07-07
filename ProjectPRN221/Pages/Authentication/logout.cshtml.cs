using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Core;

namespace ProjectPRN221.Pages.Authentication
{
    public class logoutModel : PageModel
    {
		private IDistributedCache _cache;
		public logoutModel(IDistributedCache cache)
		{
			_cache = cache;
		}
		public async Task<IActionResult> OnGet(string userId)
        {
			string? tmp = await _cache.GetRecordAsync<string>("Session_"+ userId); // Get data from cache
			Console.WriteLine(tmp);
			await _cache.RemoveAsync("Session_"+ userId);
			string? tmp2 = await _cache.GetRecordAsync<string>("Session_" + userId);
			Console.WriteLine(tmp2);
			return Page();
		}
	}
}
