namespace BookstoreDataGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int MaxRowCount = 1_000_000;

            if (args.Length != 2)
            {
                Console.WriteLine(
                    "Usage: BookstoreDataGenerator <row-count> <output-directory>");
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

            var outputDirectory = args[1];

            Directory.CreateDirectory(outputDirectory);

            Console.WriteLine($"Row count: {rowCount}");
            Console.WriteLine($"Output directory: {outputDirectory}");

            const int AuthorIdStart = 10_000_000;
            const int BookIdStart = 20_000_000;
            const int PublisherId = 1;
            const int BatchSize = 1_000;

            var loadFilePath = Path.Combine(
                outputDirectory,
                $"bookstore-load-{rowCount}.sql"
            );

            var cleanupFilePath = Path.Combine(
                outputDirectory,
                "bookstore-cleanup.sql"
            );

            using var writer = new StreamWriter(loadFilePath);

            writer.WriteLine("BEGIN;");

            for (var batchStart = 0; batchStart < rowCount; batchStart += BatchSize)
            {
                var batchEnd = Math.Min(batchStart + BatchSize, rowCount);

                writer.WriteLine(
                    """INSERT INTO "Authors" ("Id", "FullName", "Biography", "Birthday") VALUES"""
                );

                for (var i = batchStart; i < batchEnd; i++)
                {
                    var sequenceNumber = i + 1;
                    var authorId = AuthorIdStart + i;
                    var authorName = $"Perf Author {sequenceNumber:D7}";
                    var biography = $"Performance test author {sequenceNumber:D7}.";

                    var separator = i == batchEnd - 1 ? ";" : ",";

                    writer.WriteLine(
                        $"({authorId}, '{authorName}', '{biography}', '1980-01-01T00:00:00Z'){separator}"
                    );
                }
            }

            for (var batchStart = 0; batchStart < rowCount; batchStart += BatchSize)
            {
                var batchEnd = Math.Min(batchStart + BatchSize, rowCount);

                writer.WriteLine(
                    """
        INSERT INTO "Books" ("Id", "Title", "PageCount", "PublishedDate", "ISBN", "AuthorId", "PublisherId") VALUES
        """
                );

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
                        $"({bookId}, '{title}', {pageCount}, '2000-01-01T00:00:00Z', '{isbn}', {authorId}, {PublisherId}){separator}"
                    );
                }
            }

            writer.WriteLine("COMMIT;");
            writer.WriteLine(@"ANALYZE ""Authors"";");
            writer.WriteLine(@"ANALYZE ""Books"";");

            Console.WriteLine($"Generated file: {loadFilePath}");

            using var cleanupWriter = new StreamWriter(cleanupFilePath);

            cleanupWriter.WriteLine("BEGIN;");

            cleanupWriter.WriteLine(
                $"""
    DELETE FROM "Books"
    WHERE "Id" >= {BookIdStart}
      AND "Id" < {BookIdStart + MaxRowCount}
      AND "Title" LIKE 'Perf Book %';
    """
            );

            cleanupWriter.WriteLine(
                $"""
    DELETE FROM "Authors"
    WHERE "Id" >= {AuthorIdStart}
      AND "Id" < {AuthorIdStart + MaxRowCount}
      AND "FullName" LIKE 'Perf Author %';
    """
            );

            cleanupWriter.WriteLine("COMMIT;");

            Console.WriteLine($"Generated cleanup file: {cleanupFilePath}");
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
