using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookstoreApplication.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserDateOfBirthToDateOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "07e64854-533d-4a55-970a-9f861742b078");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8c9e792-2588-439e-b561-ca114d020949");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "72a1ca6b-c981-45d9-820b-4aa4971c613a", null, "Librarian", "LIBRARIAN" },
                    { "a2f51a19-539f-4279-b3a1-9a2180800c82", null, "Editor", "EDITOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72a1ca6b-c981-45d9-820b-4aa4971c613a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2f51a19-539f-4279-b3a1-9a2180800c82");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "07e64854-533d-4a55-970a-9f861742b078", null, "Librarian", "LIBRARIAN" },
                    { "a8c9e792-2588-439e-b561-ca114d020949", null, "Editor", "EDITOR" }
                });
        }
    }
}
