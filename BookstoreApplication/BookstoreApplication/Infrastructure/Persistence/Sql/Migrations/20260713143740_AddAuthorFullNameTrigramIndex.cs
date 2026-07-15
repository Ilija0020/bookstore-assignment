using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookstoreApplication.Infrastructure.Persistence.Sql.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorFullNameTrigramIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                    CREATE EXTENSION IF NOT EXISTS pg_trgm;
                """);

            migrationBuilder.Sql(
                """
                    CREATE INDEX "IX_Authors_FullName_Trgm"
                    ON "Authors"
                    USING gin (lower("FullName") gin_trgm_ops);
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                    DROP INDEX IF EXISTS "IX_Authors_FullName_Trgm";
                """);
        }
    }
}
