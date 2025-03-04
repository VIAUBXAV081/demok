
using Blog.Server.Database;
using Blog.Server.Exceptions;
using Blog.Server.Repositories;
using Blog.Server.Services;
using Blog.Server.Services.Suggestion;
using Hellang.Middleware.ProblemDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
            builder.Services.AddSwaggerGen((setup) =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Blog API",
                    Version = "v1",
                    Description = "A simple example API to manage blog posts",
                    Contact = new OpenApiContact
                    {
                        Name = "Gabor Knyihar",
                        Email = "gabor.knyihar@aut.bme.hu"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
            });

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

            // Add services to the container
            builder.Services.AddTransient<ISuggestionService, SuggestionService>();
            builder.Services.AddTransient<ITranslationService, TranslationService>();

            // Add ProblemDetails middleware
            builder.Services.AddProblemDetails(options =>
            {
                options.IncludeExceptionDetails = (context, ex) => false;
                options.Map<EntityNotFoundException>( (context, ex) =>
                    {
                        var pd = StatusCodeProblemDetails.Create(StatusCodes.Status404NotFound);
                        pd.Title = ex.Message;
                        return pd;
                    }
                );
            });

            // Build the app
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Add the controllers to the app
            app.MapControllers();
            app.MapFallbackToFile("/index.html");
            
            // Add the ProblemDetails
            app.UseProblemDetails();

            // Migrate the database
            using (var scope = app.Services.CreateScope())
            {
                using var db = scope.ServiceProvider.GetRequiredService<BlogContext>();
                db.Database.Migrate();
            }

            // Run the app
            app.Run();
        }

    }
}
