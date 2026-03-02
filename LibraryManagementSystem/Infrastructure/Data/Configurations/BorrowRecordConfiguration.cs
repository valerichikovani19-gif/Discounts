
using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations
{
    public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
    {
        public void Configure(EntityTypeBuilder<BorrowRecord> builder)
        {
            builder.HasKey(x=> x.Id);
            builder.HasOne(x => x.Book).WithMany().HasForeignKey(x=>x.BookId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x=>x.Patron).WithMany(x=>x.BorrowRecords).HasForeignKey(x=>x.PatronId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
