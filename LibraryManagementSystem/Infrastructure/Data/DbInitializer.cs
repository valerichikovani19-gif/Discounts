using LibraryManagement.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<LibraryDbContext>();

                context.Database.Migrate();
                 //martivi validacia ari tuara ukve bazashi igive monacemi   
                if (context.Books.Any() || context.Authors.Any())
                {
                    return;
                }

                // Add Authors
                var authors = new List<Author>()
                {
                    new Author { FirstName = "George", LastName = "Orwell", Biography = "English novelist.", DateOfBirth = new DateTime(1903, 6, 25) },
                    new Author { FirstName = "J.K.", LastName = "Rowling", Biography = "British author.", DateOfBirth = new DateTime(1965, 7, 31) }
                };
                context.Authors.AddRange(authors);
                context.SaveChanges();

                //Add Books
                var books = new List<Book>()
                {
                    new Book {
                        Title = "1984",
                        ISBN = "9780451524935",
                        PublicationYear = 1949,
                        Description = "Dystopian social science fiction.",
                        CoverImageUrl = "http://img.com/1984.jpg",
                        Quantity = 10,
                        AuthorId = authors[0].Id
                    },
                    new Book {
                        Title = "Harry Potter and the Philosopher's Stone",
                        ISBN = "9780747532743",
                        PublicationYear = 1997,
                        Description = "A wizard boy.",
                        CoverImageUrl = "http://img.com/hp.jpg",
                        Quantity = 20,
                        AuthorId = authors[1].Id
                    }
                };
                context.Books.AddRange(books);

                // Add Patrons
                var patrons = new List<Patron>()
                {
                    new Patron { FirstName = "John", LastName = "Doe", Email = "john@example.com", MembershipDate = DateTime.Now },
                    new Patron { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com", MembershipDate = DateTime.Now }
                };
                context.Patrons.AddRange(patrons);
                //saving g
                context.SaveChanges();
            }
        }
    }
}
