using Microsoft.Extensions.Caching.Distributed;
using ProjectPRN221.Core;

namespace ProjectPRN221
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddRazorPages();
			//Add connection redis
            builder.Services.AddStackExchangeRedisCache(options => {
                options.Configuration = builder.Configuration.GetConnectionString("Redis");
                options.InstanceName = "ProjectPRN221";
            });

			builder.Services.AddDistributedMemoryCache(); // For in-memory caching
			builder.Services.AddSession(options =>
			{
				options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
				options.Cookie.HttpOnly = true; // Make the session cookie HTTP-only
				options.Cookie.IsEssential = true; // Essential for session state to work
			});

			builder.Services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN"); 

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();
			app.UseSession();
			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
}
}