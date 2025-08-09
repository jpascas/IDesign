using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IDesign.Access.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    CountryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "USA" },
                    { 2, "Canada" },
                    { 3, "Mexico" },
                    { 4, "Brazil" },
                    { 5, "Argentina" },
                    { 6, "Colombia" },
                    { 7, "Chile" },
                    { 8, "Peru" },
                    { 9, "Venezuela" },
                    { 10, "Ecuador" }
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CountryId", "Name" },
                values: new object[,]
                {
                    { 1, 1, "USA City 1" },
                    { 2, 1, "USA City 2" },
                    { 3, 1, "USA City 3" },
                    { 4, 1, "USA City 4" },
                    { 5, 1, "USA City 5" },
                    { 6, 1, "USA City 6" },
                    { 7, 1, "USA City 7" },
                    { 8, 1, "USA City 8" },
                    { 9, 1, "USA City 9" },
                    { 10, 1, "USA City 10" },
                    { 11, 2, "Canada City 1" },
                    { 12, 2, "Canada City 2" },
                    { 13, 2, "Canada City 3" },
                    { 14, 2, "Canada City 4" },
                    { 15, 2, "Canada City 5" },
                    { 16, 2, "Canada City 6" },
                    { 17, 2, "Canada City 7" },
                    { 18, 2, "Canada City 8" },
                    { 19, 2, "Canada City 9" },
                    { 20, 2, "Canada City 10" },
                    { 21, 3, "Mexico City 1" },
                    { 22, 3, "Mexico City 2" },
                    { 23, 3, "Mexico City 3" },
                    { 24, 3, "Mexico City 4" },
                    { 25, 3, "Mexico City 5" },
                    { 26, 3, "Mexico City 6" },
                    { 27, 3, "Mexico City 7" },
                    { 28, 3, "Mexico City 8" },
                    { 29, 3, "Mexico City 9" },
                    { 30, 3, "Mexico City 10" },
                    { 31, 4, "Brazil City 1" },
                    { 32, 4, "Brazil City 2" },
                    { 33, 4, "Brazil City 3" },
                    { 34, 4, "Brazil City 4" },
                    { 35, 4, "Brazil City 5" },
                    { 36, 4, "Brazil City 6" },
                    { 37, 4, "Brazil City 7" },
                    { 38, 4, "Brazil City 8" },
                    { 39, 4, "Brazil City 9" },
                    { 40, 4, "Brazil City 10" },
                    { 41, 5, "Argentina City 1" },
                    { 42, 5, "Argentina City 2" },
                    { 43, 5, "Argentina City 3" },
                    { 44, 5, "Argentina City 4" },
                    { 45, 5, "Argentina City 5" },
                    { 46, 5, "Argentina City 6" },
                    { 47, 5, "Argentina City 7" },
                    { 48, 5, "Argentina City 8" },
                    { 49, 5, "Argentina City 9" },
                    { 50, 5, "Argentina City 10" },
                    { 51, 6, "Colombia City 1" },
                    { 52, 6, "Colombia City 2" },
                    { 53, 6, "Colombia City 3" },
                    { 54, 6, "Colombia City 4" },
                    { 55, 6, "Colombia City 5" },
                    { 56, 6, "Colombia City 6" },
                    { 57, 6, "Colombia City 7" },
                    { 58, 6, "Colombia City 8" },
                    { 59, 6, "Colombia City 9" },
                    { 60, 6, "Colombia City 10" },
                    { 61, 7, "Chile City 1" },
                    { 62, 7, "Chile City 2" },
                    { 63, 7, "Chile City 3" },
                    { 64, 7, "Chile City 4" },
                    { 65, 7, "Chile City 5" },
                    { 66, 7, "Chile City 6" },
                    { 67, 7, "Chile City 7" },
                    { 68, 7, "Chile City 8" },
                    { 69, 7, "Chile City 9" },
                    { 70, 7, "Chile City 10" },
                    { 71, 8, "Peru City 1" },
                    { 72, 8, "Peru City 2" },
                    { 73, 8, "Peru City 3" },
                    { 74, 8, "Peru City 4" },
                    { 75, 8, "Peru City 5" },
                    { 76, 8, "Peru City 6" },
                    { 77, 8, "Peru City 7" },
                    { 78, 8, "Peru City 8" },
                    { 79, 8, "Peru City 9" },
                    { 80, 8, "Peru City 10" },
                    { 81, 9, "Venezuela City 1" },
                    { 82, 9, "Venezuela City 2" },
                    { 83, 9, "Venezuela City 3" },
                    { 84, 9, "Venezuela City 4" },
                    { 85, 9, "Venezuela City 5" },
                    { 86, 9, "Venezuela City 6" },
                    { 87, 9, "Venezuela City 7" },
                    { 88, 9, "Venezuela City 8" },
                    { 89, 9, "Venezuela City 9" },
                    { 90, 9, "Venezuela City 10" },
                    { 91, 10, "Ecuador City 1" },
                    { 92, 10, "Ecuador City 2" },
                    { 93, 10, "Ecuador City 3" },
                    { 94, 10, "Ecuador City 4" },
                    { 95, 10, "Ecuador City 5" },
                    { 96, 10, "Ecuador City 6" },
                    { 97, 10, "Ecuador City 7" },
                    { 98, 10, "Ecuador City 8" },
                    { 99, 10, "Ecuador City 9" },
                    { 100, 10, "Ecuador City 10" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
