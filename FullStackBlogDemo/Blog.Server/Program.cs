
using Blog.Server.Database;
using Blog.Server.Repositories;
using Blog.Server.Services;
using Blog.Server.Services.Suggestion;
using Microsoft.EntityFrameworkCore;

namespace Blog.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            // Add AutoMapper to the container
            builder.Services.AddAutoMapper(typeof(ApiProfile));
            // Add the DbContext to the container
            builder.Services.AddDbContext<BlogContext>(options =>
            {
                options.UseSqlite(builder.Configuration.GetConnectionString("Default"));

            });
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            // Add the repository to the container
            builder.Services.AddTransient<IPostRepository, PostRepository>();
            builder.Services.AddTransient<ISuggestionService, SuggestionService>();
            builder.Services.AddTransient<ITranslationService, TranslationService>();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            using (var scope = app.Services.CreateScope())
            {
                using var db = scope.ServiceProvider.GetRequiredService<BlogContext>();
                db.Database.Migrate();
            }

            app.Run();
        }

    }
}
