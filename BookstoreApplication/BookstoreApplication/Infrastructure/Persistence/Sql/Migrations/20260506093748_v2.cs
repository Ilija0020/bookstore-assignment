using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookstoreApplication.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorAwards_Authors_AuthorId",
                table: "AuthorAwards");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorAwards_Awards_AwardId",
                table: "AuthorAwards");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorAwards",
                table: "AuthorAwards");

            migrationBuilder.DropIndex(
                name: "IX_AuthorAwards_AuthorId",
                table: "AuthorAwards");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AuthorAwards");

            migrationBuilder.RenameTable(
                name: "AuthorAwards",
                newName: "AuthorAwardBridge");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Authors",
                newName: "Birthday");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorAwards_AwardId",
                table: "AuthorAwardBridge",
                newName: "IX_AuthorAwardBridge_AwardId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorAwardBridge",
                table: "AuthorAwardBridge",
                columns: new[] { "AuthorId", "AwardId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorAwardBridge_Authors_AuthorId",
                table: "AuthorAwardBridge",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorAwardBridge_Awards_AwardId",
                table: "AuthorAwardBridge",
                column: "AwardId",
                principalTable: "Awards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorAwardBridge_Authors_AuthorId",
                table: "AuthorAwardBridge");

            migrationBuilder.DropForeignKey(
                name: "FK_AuthorAwardBridge_Awards_AwardId",
                table: "AuthorAwardBridge");

            migrationBuilder.DropForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorAwardBridge",
                table: "AuthorAwardBridge");

            migrationBuilder.RenameTable(
                name: "AuthorAwardBridge",
                newName: "AuthorAwards");

            migrationBuilder.RenameColumn(
                name: "Birthday",
                table: "Authors",
                newName: "DateOfBirth");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorAwardBridge_AwardId",
                table: "AuthorAwards",
                newName: "IX_AuthorAwards_AwardId");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "AuthorAwards",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorAwards",
                table: "AuthorAwards",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorAwards_AuthorId",
                table: "AuthorAwards",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorAwards_Authors_AuthorId",
                table: "AuthorAwards",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorAwards_Awards_AwardId",
                table: "AuthorAwards",
                column: "AwardId",
                principalTable: "Awards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Publishers_PublisherId",
                table: "Books",
                column: "PublisherId",
                principalTable: "Publishers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
