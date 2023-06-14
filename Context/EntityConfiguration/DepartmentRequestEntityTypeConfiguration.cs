using ComplaintRequestSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintRequestSystem.Context.EntityConfiguration
{
    public class DepartmentRequestEntityTypeConfiguration : IEntityTypeConfiguration<DepartmentRequest>
    {
        public void Configure(EntityTypeBuilder<DepartmentRequest> builder)
        {
            builder.ToTable("DepartmentRequests");
            builder.Ignore(dr => dr.Id);
            builder.HasKey(dr => new { dr.DepartmentId, dr.RequestId });

            builder.HasOne(dr => dr.Department)
                .WithMany(r => r.DepartmentRequest)
                .HasForeignKey(cq => cq.DepartmentId)
                .IsRequired();

            builder.HasOne(dr => dr.Request)
                .WithMany(r => r.DepartmentRequest)
                .HasForeignKey(cq => cq.RequestId)
                .IsRequired();
        }
    }
}
