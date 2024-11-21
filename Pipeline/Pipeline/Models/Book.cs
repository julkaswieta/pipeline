using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Pipeline
{
    /**Represents a book*/
    public class Book
    {

        /**Primary key for the book object*/
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        /**Name of the book*/
        [Required]
        public string Name { get; set; }

        /**Author of the book*/
        [Required]
        public string Author { get; set; }

        /**Blurb of the book*/
        public string Blurb { get; set; }
    }

    /**Database context for book database*/
    public class BookDb : DbContext
    {
        public DbSet<Book> Books { get; set; } = null!;

        IConfigurationRoot config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SqlServerDbContextOptionsBuilder contextOptionsBuilder = new SqlServerDbContextOptionsBuilder(optionsBuilder);
            contextOptionsBuilder.EnableRetryOnFailure();
            String connectionString = config.GetConnectionString("BooksDb");
            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}