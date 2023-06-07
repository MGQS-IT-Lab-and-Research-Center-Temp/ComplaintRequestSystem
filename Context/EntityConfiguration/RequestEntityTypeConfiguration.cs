using ComplaintRequestSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintRequestSystem.Context.EntityConfiguration
{
    public class RequestEntityTypeConfiguration : IEntityTypeConfiguration<Request>
    {
        public void Configure(EntityTypeBuilder<Request> builder)
        {
            builder.ToTable("Requests");
            builder.HasKey(r => r.Id);

            builder.HasOne(u => u.User)
                .WithMany(r => r.Requests)
                .HasForeignKey(r => r.UserId)
                .IsRequired();

            builder.Property(r => r.RequestText)
                .IsRequired()
                .HasMaxLength(150);
        }
    }
}
