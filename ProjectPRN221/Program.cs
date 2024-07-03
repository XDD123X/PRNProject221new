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

			app.UseAuthorization();

			app.MapRazorPages();

			app.Run();
		}
}
}