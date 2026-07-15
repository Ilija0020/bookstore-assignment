using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookstoreApplication.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddIssues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72a1ca6b-c981-45d9-820b-4aa4971c613a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2f51a19-539f-4279-b3a1-9a2180800c82");

            migrationBuilder.CreateTable(
                name: "Issues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ReleaseDate = table.Column<DateOnly>(type: "date", nullable: true),
                    IssueNumber = table.Column<string>(type: "text", nullable: true),
                    ImagePath = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ExternalIssueId = table.Column<int>(type: "integer", nullable: false),
                    ExternalVolumeId = table.Column<int>(type: "integer", nullable: false),
                    PageCount = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    AvailableCopies = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Issues", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2af1f570-ab74-4c16-a373-e7079629f808", null, "Librarian", "LIBRARIAN" },
                    { "4d04a1c6-1377-4430-80a6-60729967604b", null, "Editor", "EDITOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Issues");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2af1f570-ab74-4c16-a373-e7079629f808");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4d04a1c6-1377-4430-80a6-60729967604b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "72a1ca6b-c981-45d9-820b-4aa4971c613a", null, "Librarian", "LIBRARIAN" },
                    { "a2f51a19-539f-4279-b3a1-9a2180800c82", null, "Editor", "EDITOR" }
                });
        }
    }
}
