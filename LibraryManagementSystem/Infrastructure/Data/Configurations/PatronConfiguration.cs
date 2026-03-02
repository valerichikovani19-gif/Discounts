using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations
{
    public class PatronConfiguration : IEntityTypeConfiguration<Patron>
    {
        public void Configure(EntityTypeBuilder<Patron> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(x=>x.LastName).IsRequired().HasMaxLength(50);
            builder.Property(x=>x.Email).IsRequired().HasMaxLength(100);
            builder.HasIndex(x=>x.Email).IsUnique(); // unikaluri unda ikos 
        }
    }
}
