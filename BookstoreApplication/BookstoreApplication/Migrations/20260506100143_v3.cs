using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookstoreApplication.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Biography", "Birthday", "FullName" },
                values: new object[,]
                {
                    { 1, "Jugoslovenski književnik i diplomata.", new DateTime(1892, 10, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Ivo Andrić" },
                    { 2, "Istaknuti pisac iz Bosne i Hercegovine.", new DateTime(1910, 4, 26, 0, 0, 0, 0, DateTimeKind.Utc), "Meša Selimović" },
                    { 3, "English novelist and essayist.", new DateTime(1903, 6, 25, 0, 0, 0, 0, DateTimeKind.Utc), "George Orwell" },
                    { 4, "British author of the Harry Potter series.", new DateTime(1965, 7, 31, 0, 0, 0, 0, DateTimeKind.Utc), "J.K. Rowling" },
                    { 5, "Russian writer who is regarded as one of the greatest authors.", new DateTime(1828, 9, 9, 0, 0, 0, 0, DateTimeKind.Utc), "Leo Tolstoy" }
                });

            migrationBuilder.InsertData(
                table: "Awards",
                columns: new[] { "Id", "Description", "Name", "StartYear" },
                values: new object[,]
                {
                    { 1, "Najprestižnija svetska nagrada za književnost.", "Nobelova nagrada", 1901 },
                    { 2, "Prestižna srpska književna nagrada.", "NIN-ova nagrada", 1954 },
                    { 3, "Award for the best novel written in English.", "Booker Prize", 1969 },
                    { 4, "Award for achievements in newspaper journalism and literature.", "Pulitzer Prize", 1917 }
                });

            migrationBuilder.InsertData(
                table: "Publishers",
                columns: new[] { "Id", "Address", "Name", "Website" },
                values: new object[,]
                {
                    { 1, "Resavska 33, Beograd", "Laguna", "https://www.laguna.rs" },
                    { 2, "Gospodara Vučića 245, Beograd", "Vulkan", "https://www.vulkani.rs" },
                    { 3, "80 Strand, London", "Penguin Books", "https://www.penguin.co.uk" }
                });

            migrationBuilder.InsertData(
                table: "AuthorAwardBridge",
                columns: new[] { "AuthorId", "AwardId", "YearReceived" },
                values: new object[,]
                {
                    { 1, 1, 1961 },
                    { 1, 2, 1954 },
                    { 1, 3, 1965 },
                    { 1, 4, 1960 },
                    { 2, 1, 1970 },
                    { 2, 2, 1967 },
                    { 2, 3, 1968 },
                    { 3, 1, 1949 },
                    { 3, 3, 1950 },
                    { 3, 4, 1946 },
                    { 4, 1, 2010 },
                    { 4, 3, 2000 },
                    { 4, 4, 2005 },
                    { 5, 1, 1910 },
                    { 5, 2, 1890 }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "ISBN", "PageCount", "PublishedDate", "PublisherId", "Title" },
                values: new object[,]
                {
                    { 1, 1, "9788652101234", 420, new DateTime(1945, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Na Drini ćuprija" },
                    { 2, 1, "9788652101235", 150, new DateTime(1954, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Prokleta avlija" },
                    { 3, 2, "9788652101236", 380, new DateTime(1966, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Derviš i smrt" },
                    { 4, 2, "9788652101237", 410, new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Tvrđava" },
                    { 5, 3, "9780451524935", 328, new DateTime(1949, 6, 8, 0, 0, 0, 0, DateTimeKind.Utc), 3, "1984" },
                    { 6, 3, "9780451526342", 112, new DateTime(1945, 8, 17, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Animal Farm" },
                    { 7, 4, "9780747532699", 223, new DateTime(1997, 6, 26, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Harry Potter and the Philosopher's Stone" },
                    { 8, 4, "9780747538493", 251, new DateTime(1998, 7, 2, 0, 0, 0, 0, DateTimeKind.Utc), 3, "Harry Potter and the Chamber of Secrets" },
                    { 9, 5, "9780140447934", 1225, new DateTime(1869, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 3, "War and Peace" },
                    { 10, 5, "9780143035008", 864, new DateTime(1877, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Anna Karenina" },
                    { 11, 1, "9788652101238", 310, new DateTime(1977, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Omer-paša Latas" },
                    { 12, 1, "9788652101239", 450, new DateTime(1976, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Znakovi pored puta" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 3, 4 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 4, 1 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 4, 3 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 4, 4 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "AuthorAwardBridge",
                keyColumns: new[] { "AuthorId", "AwardId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Awards",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Awards",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Awards",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Awards",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Publishers",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
