using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Core;

namespace ProjectPRN221.Pages
{
    public class TestRedisModel : PageModel
    {

        private IDistributedCache _cache;

        public TestRedisModel(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async void OnGet()
        {
            Console.WriteLine("start server");
            await _cache.SetRecordAsync("token", "test", TimeSpan.FromMinutes(10));
            string? tmp = await _cache.GetRecordAsync<string>("test"); // Get data from cache
            Console.WriteLine("value of redis" + tmp);
        }
    }
}
