using Npgsql;
using NpgsqlTypes;
using System.Diagnostics;

namespace BookstoreDataGenerator
{
    internal class Program
    {
        private const int MaxRowCount = 1_000_000;
        private const int AuthorIdStart = 10_000_000;
        private const int BookIdStart = 20_000_000;
        private const int PublisherId = 1;
        private const int BatchSize = 1_000;

        private static async Task Main(string[] args)
        {
            if (args.Length < 2 || args.Length > 3)
            {
                Console.WriteLine(
                    "Usage: BookstoreDataGenerator <row-count> <output-directory> [insert|copy]");
                return;
            }

            if (!int.TryParse(args[0], out var rowCount)
                || rowCount <= 0
                || rowCount > MaxRowCount)
            {
                Console.WriteLine(
                    $"Row count must be between 1 and {MaxRowCount}.");
                return;
            }

            var mode = args.Length == 3
                ? args[2].ToLowerInvariant()
                : "insert";

            if (mode != "insert" && mode != "copy")
            {
                Console.WriteLine("Mode must be either 'insert' or 'copy'.");
                return;
            }

            var outputDirectory = args[1];

            Directory.CreateDirectory(outputDirectory);

            Console.WriteLine($"Row count: {rowCount}");
            Console.WriteLine($"Output directory: {outputDirectory}");
            Console.WriteLine($"Mode: {mode}");

            GenerateCleanupSql(outputDirectory);

            if (mode == "insert")
            {
                GenerateInsertSql(rowCount, outputDirectory);
                return;
            }

            var connectionString = Environment.GetEnvironmentVariable(
                "BOOKSTORE_CONNECTION_STRING");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Console.WriteLine(
                    "The BOOKSTORE_CONNECTION_STRING environment variable is not set.");
                return;
            }

            try
            {
                await using var connection = new NpgsqlConnection(connectionString);

                await connection.OpenAsync();

                Console.WriteLine("Successfully connected to PostgreSQL.");

                var preflightPassed = await RunPreflightChecksAsync(connection);

                if (!preflightPassed)
                {
                    return;
                }

                await RunBinaryCopyAsync(connection, rowCount);
            }
            catch (NpgsqlException exception)
            {
                Console.WriteLine(
                    $"PostgreSQL operation failed: {exception.Message}");
            }
        }

        private static void GenerateInsertSql(int rowCount, string outputDirectory)
        {
            var loadFilePath = Path.Combine(
                outputDirectory,
                $"bookstore-load-{rowCount}.sql");

            using var writer = new StreamWriter(loadFilePath);

            writer.WriteLine("BEGIN;");

            for (var batchStart = 0; batchStart < rowCount; batchStart += BatchSize)
            {
                var batchEnd = Math.Min(batchStart + BatchSize, rowCount);

                writer.WriteLine(
                    """INSERT INTO "Authors" ("Id", "FullName", "Biography", "Birthday") VALUES""");

                for (var i = batchStart; i < batchEnd; i++)
                {
                    var sequenceNumber = i + 1;
                    var authorId = AuthorIdStart + i;
                    var authorName = $"Perf Author {sequenceNumber:D7}";
                    var biography = $"Performance test author {sequenceNumber:D7}.";

                    var separator = i == batchEnd - 1 ? ";" : ",";

                    writer.WriteLine(
                        $"({authorId}, '{authorName}', '{biography}', '1980-01-01T00:00:00Z'){separator}");
                }
            }

            for (var batchStart = 0; batchStart < rowCount; batchStart += BatchSize)
            {
                var batchEnd = Math.Min(batchStart + BatchSize, rowCount);

                writer.WriteLine(
                    """INSERT INTO "Books" ("Id", "Title", "PageCount", "PublishedDate", "ISBN", "AuthorId", "PublisherId") VALUES""");

                for (var i = batchStart; i < batchEnd; i++)
                {
                    var sequenceNumber = i + 1;
                    var bookId = BookIdStart + i;
                    var authorId = AuthorIdStart + i;

                    var title = $"Perf Book {sequenceNumber:D7}";
                    var isbn = GenerateIsbn13(sequenceNumber);
                    var pageCount = 200 + i % 300;

                    var separator = i == batchEnd - 1 ? ";" : ",";

                    writer.WriteLine(
                        $"({bookId}, '{title}', {pageCount}, '2000-01-01T00:00:00Z', '{isbn}', {authorId}, {PublisherId}){separator}");
                }
            }

            writer.WriteLine("COMMIT;");
            writer.WriteLine(@"ANALYZE ""Authors"";");
            writer.WriteLine(@"ANALYZE ""Books"";");

            Console.WriteLine($"Generated file: {loadFilePath}");
        }

        private static void GenerateCleanupSql(string outputDirectory)
        {
            var cleanupFilePath = Path.Combine(
                outputDirectory,
                "bookstore-cleanup.sql");

            using var cleanupWriter = new StreamWriter(cleanupFilePath);

            cleanupWriter.WriteLine("BEGIN;");

            cleanupWriter.WriteLine(
                $"""
                DELETE FROM "Books"
                WHERE "Id" >= {BookIdStart}
                  AND "Id" < {BookIdStart + MaxRowCount}
                  AND "Title" LIKE 'Perf Book %';
                """);

            cleanupWriter.WriteLine(
                $"""
                DELETE FROM "Authors"
                WHERE "Id" >= {AuthorIdStart}
                  AND "Id" < {AuthorIdStart + MaxRowCount}
                  AND "FullName" LIKE 'Perf Author %';
                """);

            cleanupWriter.WriteLine("COMMIT;");

            Console.WriteLine($"Generated cleanup file: {cleanupFilePath}");
        }

        private static async Task<bool> RunPreflightChecksAsync(NpgsqlConnection connection)
        {
            const string sql =
                """
                SELECT
                    EXISTS (
                        SELECT 1
                        FROM "Publishers"
                        WHERE "Id" = @publisherId
                    ),
                    EXISTS (
                        SELECT 1
                        FROM "Authors"
                        WHERE "Id" >= @authorIdStart
                          AND "Id" < @authorIdEnd
                    ),
                    EXISTS (
                        SELECT 1
                        FROM "Books"
                        WHERE "Id" >= @bookIdStart
                          AND "Id" < @bookIdEnd
                    );
                """;

            await using var command = new NpgsqlCommand(sql, connection);

            command.Parameters.AddWithValue("publisherId", PublisherId);
            command.Parameters.AddWithValue("authorIdStart", AuthorIdStart);
            command.Parameters.AddWithValue(
                "authorIdEnd",
                AuthorIdStart + MaxRowCount);
            command.Parameters.AddWithValue("bookIdStart", BookIdStart);
            command.Parameters.AddWithValue(
                "bookIdEnd",
                BookIdStart + MaxRowCount);

            await using var reader = await command.ExecuteReaderAsync();

            await reader.ReadAsync();

            var publisherExists = reader.GetBoolean(0);
            var authorRangeOccupied = reader.GetBoolean(1);
            var bookRangeOccupied = reader.GetBoolean(2);

            if (!publisherExists)
            {
                Console.WriteLine(
                    $"Preflight failed: Publisher with Id {PublisherId} does not exist.");
                return false;
            }

            if (authorRangeOccupied)
            {
                Console.WriteLine(
                    "Preflight failed: The reserved author ID range is occupied.");
                return false;
            }

            if (bookRangeOccupied)
            {
                Console.WriteLine(
                    "Preflight failed: The reserved book ID range is occupied.");
                return false;
            }

            Console.WriteLine("Preflight checks passed.");

            return true;
        }

        private static async Task RunBinaryCopyAsync(
            NpgsqlConnection connection,
            int rowCount)
        {
            var stopwatch = Stopwatch.StartNew();

            await using var transaction = await connection.BeginTransactionAsync();

            try
            {
                await CopyAuthorsAsync(connection, rowCount);
                await CopyBooksAsync(connection, rowCount);

                await transaction.CommitAsync();

                stopwatch.Stop();

                Console.WriteLine(
                    $"Binary COPY committed in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");
            }
            catch
            {
                await transaction.RollbackAsync();

                Console.WriteLine("Binary COPY rolled back.");

                throw;
            }

            await AnalyzeTablesAsync(connection);
        }

        private static async Task CopyAuthorsAsync(
            NpgsqlConnection connection,
            int rowCount)
        {
            const string copyCommand =
                """
                COPY "Authors" (
                    "Id",
                    "FullName",
                    "Biography",
                    "Birthday"
                )
                FROM STDIN (FORMAT BINARY)
                """;

            await using var importer = await connection.BeginBinaryImportAsync(copyCommand);

            var birthday = new DateTime(
                1980,
                1,
                1,
                0,
                0,
                0,
                DateTimeKind.Utc);

            for (var i = 0; i < rowCount; i++)
            {
                var sequenceNumber = i + 1;
                var authorId = AuthorIdStart + i;
                var authorName = $"Perf Author {sequenceNumber:D7}";
                var biography = $"Performance test author {sequenceNumber:D7}.";

                importer.StartRow();

                importer.Write(authorId, NpgsqlDbType.Integer);
                importer.Write(authorName, NpgsqlDbType.Text);
                importer.Write(biography, NpgsqlDbType.Text);
                importer.Write(birthday, NpgsqlDbType.TimestampTz);
            }

            var copiedRows = await importer.CompleteAsync();

            Console.WriteLine($"Copied {copiedRows:N0} authors.");
        }

        private static async Task CopyBooksAsync(
            NpgsqlConnection connection,
            int rowCount)
        {
            const string copyCommand =
                """
                COPY "Books" (
                    "Id",
                    "Title",
                    "PageCount",
                    "PublishedDate",
                    "ISBN",
                    "AuthorId",
                    "PublisherId"
                )
                FROM STDIN (FORMAT BINARY)
                """;

            await using var importer = await connection.BeginBinaryImportAsync(copyCommand);

            var publishedDate = new DateTime(
                2000,
                1,
                1,
                0,
                0,
                0,
                DateTimeKind.Utc);

            for (var i = 0; i < rowCount; i++)
            {
                var sequenceNumber = i + 1;
                var bookId = BookIdStart + i;
                var authorId = AuthorIdStart + i;

                var title = $"Perf Book {sequenceNumber:D7}";
                var isbn = GenerateIsbn13(sequenceNumber);
                var pageCount = 200 + i % 300;

                importer.StartRow();

                importer.Write(bookId, NpgsqlDbType.Integer);
                importer.Write(title, NpgsqlDbType.Text);
                importer.Write(pageCount, NpgsqlDbType.Integer);
                importer.Write(publishedDate, NpgsqlDbType.TimestampTz);
                importer.Write(isbn, NpgsqlDbType.Text);
                importer.Write(authorId, NpgsqlDbType.Integer);
                importer.Write(PublisherId, NpgsqlDbType.Integer);
            }

            var copiedRows = await importer.CompleteAsync();

            Console.WriteLine($"Copied {copiedRows:N0} books.");
        }

        private static async Task AnalyzeTablesAsync(NpgsqlConnection connection)
        {
            const string sql =
                """
                ANALYZE "Authors";
                ANALYZE "Books";
                """;

            await using var command = new NpgsqlCommand(sql, connection);

            await command.ExecuteNonQueryAsync();

            Console.WriteLine("ANALYZE completed.");
        }

        private static string GenerateIsbn13(int sequenceNumber)
        {
            var firstTwelveDigits = $"978{sequenceNumber:D9}";

            var sum = 0;

            for (var i = 0; i < firstTwelveDigits.Length; i++)
            {
                var digit = firstTwelveDigits[i] - '0';

                sum += i % 2 == 0
                    ? digit
                    : digit * 3;
            }

            var checkDigit = (10 - sum % 10) % 10;

            return $"{firstTwelveDigits}{checkDigit}";
        }
    }
}
