
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x=>x.ISBN).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.CoverImageUrl).HasMaxLength(500);
            //ert avtors bevri wigniak
            builder.HasOne(x=>x.Author).WithMany(x=>x.Books).HasForeignKey(x=>x.AuthorId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
