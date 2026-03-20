    using SwoopMarketplaceProjectFrontend.Infrastructure;
using SwoopMarketplaceProjectFrontend.Services;

namespace SwoopMarketplaceProjectFrontend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            // IHttpContextAccessor + session already registered below; ensure they remain.
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddHttpContextAccessor();

            // Register AuthSession, Jwt handler and API services
            builder.Services.AddScoped<AuthSession>();
            builder.Services.AddScoped<AuthApi>();
            builder.Services.AddTransient<JwtBearerHandler>();

            // Single named HttpClient with JwtBearerHandler attached
            builder.Services.AddHttpClient("SwoopApi", c =>
            {
                c.BaseAddress = new Uri(builder.Configuration["Api:BaseUrl"]!);
            })
            .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseProxy = false })
            .AddHttpMessageHandler<JwtBearerHandler>();

            builder.Services.AddScoped<ListingApi>();
            builder.Services.AddScoped<CategoryApi>();
            builder.Services.AddScoped<ListingViewApi>();
            builder.Services.AddScoped<ListingImageApi>();
            builder.Services.AddScoped<UserApi>();
            builder.Services.AddScoped<ReportApi>();

            builder.Services.AddScoped<AuthPageFilter>();

            builder.Services.AddRazorPages()
                .AddMvcOptions(options =>
                {
                    options.Filters.AddService<AuthPageFilter>();
                });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
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
