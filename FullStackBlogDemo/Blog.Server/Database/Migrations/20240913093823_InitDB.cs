using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Blog.Server.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "ID", "Content", "Title" },
                values: new object[,]
                {
                    { 1, "When you need to add or remove elements frequently, a LinkedList is the better choice. When you need to access elements by index, a List is the better choice.", "When to use a List vs a LinkedList" },
                    { 2, "Continuous integration and continuous deployment (CI/CD) pipelines are essential for agile software development because they automate the process of building, testing, and deploying software. This allows developers to quickly and easily deliver new features and updates to customers.", "Why CI/CD Pipelines Are Essential for Agile Software Development" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
