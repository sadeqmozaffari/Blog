using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Blog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedDate", "Description", "Title", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Travel", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Technology", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Lifestyle", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "CategoryId", "CreatedDate", "Description", "ImageUrl", "Title", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "A journey through beautiful mountain landscapes.", "https://picsum.photos/id/1018/800/600", "Exploring the Mountains", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 1, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "Experience the energy of modern cities.", "https://picsum.photos/id/1011/800/600", "City Life Vibes", new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 1, new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), "Relaxing views of the endless ocean.", "https://picsum.photos/id/1016/800/600", "Ocean Breeze", new DateTime(2024, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 1, new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), "Peaceful walk through green forests.", "https://picsum.photos/id/1015/800/600", "Forest Walk", new DateTime(2024, 1, 4, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beautiful sunset over the horizon.", "https://picsum.photos/id/1003/800/600", "Sunset Moments", new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 1, new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Stunning building designs from around the world.", "https://picsum.photos/id/1025/800/600", "Modern Architecture", new DateTime(2024, 1, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 1, new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Winter sports and snowy landscapes.", "https://picsum.photos/id/1002/800/600", "Snow Adventure", new DateTime(2024, 1, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 1, new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Golden sands and desert adventures.", "https://picsum.photos/id/1005/800/600", "Desert Journey", new DateTime(2024, 1, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 1, new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Calm and peaceful lake scenery.", "https://picsum.photos/id/1019/800/600", "Lake View", new DateTime(2024, 1, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 1, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Life captured in urban streets.", "https://picsum.photos/id/1027/800/600", "Street Photography", new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 1, new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified), "Animals in their natural habitat.", "https://picsum.photos/id/1024/800/600", "Wild Nature", new DateTime(2024, 1, 11, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 2, new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beautiful city lights at night.", "https://picsum.photos/id/1012/800/600", "Night City Lights", new DateTime(2024, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 1, new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Peaceful rural landscapes.", "https://picsum.photos/id/1020/800/600", "Countryside Calm", new DateTime(2024, 1, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 1, new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "Famous bridges around the world.", "https://picsum.photos/id/1031/800/600", "Bridge View", new DateTime(2024, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 1, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Memories from amazing journeys.", "https://picsum.photos/id/1040/800/600", "Travel Moments", new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoryId",
                table: "Posts",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
