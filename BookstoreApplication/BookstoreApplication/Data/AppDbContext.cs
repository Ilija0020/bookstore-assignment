using BookstoreApplication.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookstoreApplication.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<Award> Awards { get; set; }
        public DbSet<AuthorAward> AuthorAwards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AuthorAward>()
                .ToTable("AuthorAwardBridge");

            modelBuilder.Entity<Author>()
                .Property(a => a.DateOfBirth)
                .HasColumnName("Birthday");

            modelBuilder.Entity<AuthorAward>()
                .HasKey(authorAward => new { authorAward.AuthorId, authorAward.AwardId });

            modelBuilder.Entity<AuthorAward>()
                .HasOne(authorAward => authorAward.Author)
                .WithMany(author => author.AuthorAwards)
                .HasForeignKey(authorAward => authorAward.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AuthorAward>()
                .HasOne(authorAward => authorAward.Award)
                .WithMany(award => award.AuthorAwards)
                .HasForeignKey(authorAward => authorAward.AwardId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Book>()
                .HasOne(book => book.Publisher)
                .WithMany(publisher => publisher.Books)
                .HasForeignKey(book => book.PublisherId)
                .OnDelete(DeleteBehavior.Restrict);

            // 1. Autori (5 komada)
            modelBuilder.Entity<Author>().HasData(
                new Author { Id = 1, FullName = "Ivo Andrić", Biography = "Jugoslovenski književnik i diplomata.", DateOfBirth = new DateTime(1892, 10, 9, 0, 0, 0, DateTimeKind.Utc) },
                new Author { Id = 2, FullName = "Meša Selimović", Biography = "Istaknuti pisac iz Bosne i Hercegovine.", DateOfBirth = new DateTime(1910, 4, 26, 0, 0, 0, DateTimeKind.Utc) },
                new Author { Id = 3, FullName = "George Orwell", Biography = "English novelist and essayist.", DateOfBirth = new DateTime(1903, 6, 25, 0, 0, 0, DateTimeKind.Utc) },
                new Author { Id = 4, FullName = "J.K. Rowling", Biography = "British author of the Harry Potter series.", DateOfBirth = new DateTime(1965, 7, 31, 0, 0, 0, DateTimeKind.Utc) },
                new Author { Id = 5, FullName = "Leo Tolstoy", Biography = "Russian writer who is regarded as one of the greatest authors.", DateOfBirth = new DateTime(1828, 9, 9, 0, 0, 0, DateTimeKind.Utc) }
            );

            // 2. Izdavači (3 komada)
            modelBuilder.Entity<Publisher>().HasData(
                new Publisher { Id = 1, Name = "Laguna", Address = "Resavska 33, Beograd", Website = "https://www.laguna.rs" },
                new Publisher { Id = 2, Name = "Vulkan", Address = "Gospodara Vučića 245, Beograd", Website = "https://www.vulkani.rs" },
                new Publisher { Id = 3, Name = "Penguin Books", Address = "80 Strand, London", Website = "https://www.penguin.co.uk" }
            );

            // 3. Nagrade (4 komada)
            modelBuilder.Entity<Award>().HasData(
                new Award { Id = 1, Name = "Nobelova nagrada", Description = "Najprestižnija svetska nagrada za književnost.", StartYear = 1901 },
                new Award { Id = 2, Name = "NIN-ova nagrada", Description = "Prestižna srpska književna nagrada.", StartYear = 1954 },
                new Award { Id = 3, Name = "Booker Prize", Description = "Award for the best novel written in English.", StartYear = 1969 },
                new Award { Id = 4, Name = "Pulitzer Prize", Description = "Award for achievements in newspaper journalism and literature.", StartYear = 1917 }
            );

            // 4. Knjige (12 komada)
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Na Drini ćuprija", PageCount = 420, ISBN = "9788652101234", PublishedDate = new DateTime(1945, 1, 1, 0, 0, 0, DateTimeKind.Utc), AuthorId = 1, PublisherId = 1 },
                new Book { Id = 2, Title = "Prokleta avlija", PageCount = 150, ISBN = "9788652101235", PublishedDate = new DateTime(1954, 1, 1, 0, 0, 0, DateTimeKind.Utc), AuthorId = 1, PublisherId = 1 },
                new Book { Id = 3, Title = "Derviš i smrt", PageCount = 380, ISBN = "9788652101236", PublishedDate = new DateTime(1966, 1, 1, 0, 0, 0, DateTimeKind.Utc), AuthorId = 2, PublisherId = 1 },
                new Book { Id = 4, Title = "Tvrđava", PageCount = 410, ISBN = "9788652101237", PublishedDate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), AuthorId = 2, PublisherId = 2 },
                new Book { Id = 5, Title = "1984", PageCount = 328, ISBN = "9780451524935", PublishedDate = new DateTime(1949, 6, 8, 0, 0, 0, DateTimeKind.Utc), AuthorId = 3, PublisherId = 3 },
                new Book { Id = 6, Title = "Animal Farm", PageCount = 112, ISBN = "9780451526342", PublishedDate = new DateTime(1945, 8, 17, 0, 0, 0, DateTimeKind.Utc), AuthorId = 3, PublisherId = 3 },
                new Book { Id = 7, Title = "Harry Potter and the Philosopher's Stone", PageCount = 223, ISBN = "9780747532699", PublishedDate = new DateTime(1997, 6, 26, 0, 0, 0, DateTimeKind.Utc), AuthorId = 4, PublisherId = 3 },
                new Book { Id = 8, Title = "Harry Potter and the Chamber of Secrets", PageCount = 251, ISBN = "9780747538493", PublishedDate = new DateTime(1998, 7, 2, 0, 0, 0, DateTimeKind.Utc), AuthorId = 4, PublisherId = 3 },
                new Book { Id = 9, Title = "War and Peace", PageCount = 1225, ISBN = "9780140447934", PublishedDate = new DateTime(1869, 1, 1, 0, 0, 0, DateTimeKind.Utc), AuthorId = 5, PublisherId = 3 },
                new Book { Id = 10, Title = "Anna Karenina", PageCount = 864, ISBN = "9780143035008", PublishedDate = new DateTime(1877, 4, 1, 0, 0, 0, DateTimeKind.Utc), AuthorId = 5, PublisherId = 1 },
                new Book { Id = 11, Title = "Omer-paša Latas", PageCount = 310, ISBN = "9788652101238", PublishedDate = new DateTime(1977, 1, 1, 0, 0, 0, DateTimeKind.Utc), AuthorId = 1, PublisherId = 2 },
                new Book { Id = 12, Title = "Znakovi pored puta", PageCount = 450, ISBN = "9788652101239", PublishedDate = new DateTime(1976, 1, 1, 0, 0, 0, DateTimeKind.Utc), AuthorId = 1, PublisherId = 1 }
            );

            // 5. Poveznica Autori-Nagrade (15 unosa)
            modelBuilder.Entity<AuthorAward>().HasData(
                new AuthorAward { AuthorId = 1, AwardId = 1, YearReceived = 1961 }, // Andrić - Nobel
                new AuthorAward { AuthorId = 1, AwardId = 2, YearReceived = 1954 }, // Andrić - NIN
                new AuthorAward { AuthorId = 2, AwardId = 2, YearReceived = 1967 }, // Meša - NIN
                new AuthorAward { AuthorId = 3, AwardId = 3, YearReceived = 1950 }, // Orwell - Booker (primer)
                new AuthorAward { AuthorId = 4, AwardId = 3, YearReceived = 2000 }, // Rowling - Booker
                new AuthorAward { AuthorId = 5, AwardId = 1, YearReceived = 1910 }, // Tolstoy - Nobel
                new AuthorAward { AuthorId = 1, AwardId = 3, YearReceived = 1965 },
                new AuthorAward { AuthorId = 2, AwardId = 1, YearReceived = 1970 },
                new AuthorAward { AuthorId = 3, AwardId = 4, YearReceived = 1946 },
                new AuthorAward { AuthorId = 4, AwardId = 4, YearReceived = 2005 },
                new AuthorAward { AuthorId = 5, AwardId = 2, YearReceived = 1890 },
                new AuthorAward { AuthorId = 1, AwardId = 4, YearReceived = 1960 },
                new AuthorAward { AuthorId = 2, AwardId = 3, YearReceived = 1968 },
                new AuthorAward { AuthorId = 3, AwardId = 1, YearReceived = 1949 },
                new AuthorAward { AuthorId = 4, AwardId = 1, YearReceived = 2010 }
            );
        }
    }
}
