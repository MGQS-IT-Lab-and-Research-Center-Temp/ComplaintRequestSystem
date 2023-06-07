using ComplaintRequestSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintRequestSystem.Context.EntityConfiguration
{
    public class ComplaintEntityTypeConfiguration : IEntityTypeConfiguration<Complaint>
    {
        public void Configure(EntityTypeBuilder<Complaint> builder) 
        {
            builder.ToTable("Complaints");
            builder.HasKey(c => c.Id);

            builder.HasOne(u => u.User)
                .WithMany(c => c.Complaints)
                .HasForeignKey(q => q.UserId)
                .IsRequired();

            builder.Property(c => c.ComplaintText)
                .IsRequired()
                .HasMaxLength(150);

        }
    }
}
