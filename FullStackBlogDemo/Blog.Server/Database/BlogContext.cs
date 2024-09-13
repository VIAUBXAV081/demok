using Blog.Server.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Server.Database
{
    public class BlogContext : DbContext
    {
        public BlogContext() { }

        public BlogContext(DbContextOptions<BlogContext> options) : base(options) { }

        public DbSet<Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Post>()
                .ToTable("Posts");

            modelBuilder.Entity<Post>().HasData([
                new Post{
                    ID=1,
                    Title="When to use a List vs a LinkedList",
                    Content="When you need to add or remove elements frequently, a LinkedList is the better choice. When you need to access elements by index, a List is the better choice."
                },
                new Post{
                    ID=2,
                    Title="Why CI/CD Pipelines Are Essential for Agile Software Development",
                    Content="Continuous integration and continuous deployment (CI/CD) pipelines are essential for agile software development because they automate the process of building, testing, and deploying software. This allows developers to quickly and easily deliver new features and updates to customers."
                }
            ]);
        }
    }
}
