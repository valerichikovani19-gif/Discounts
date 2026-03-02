using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations
{
    public class AuthorConfiguration:IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(x=> x.Id);
            builder.Property(x=>x.FirstName).IsRequired().HasMaxLength(50);
            builder.Property(x=>x.LastName).IsRequired().HasMaxLength(50);
            builder.Property(a => a.Biography).HasMaxLength(1000);
            //builder.HasMany(a=>a.Books).WithOne(b=>b.Author).HasForeignKey(b=>b.AuthorId).OnDelete(DeleteBehavior.Cascade);//tu avtori waishleba wigneebic waishalos m
        }
    }
}
