using ComplaintRequestSystem.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ComplaintRequestSystem.Context.EntityConfiguration
{
    public class DepartmentComplaintEntityTypeConfiguration : IEntityTypeConfiguration<DepartmentComplaint>
    {
        public void Configure(EntityTypeBuilder<DepartmentComplaint> builder)
        {
            builder.ToTable("DepartmentComplaints");
            builder.Ignore(dc => dc.Id);
            builder.HasKey(dc => new { dc.DepartmentId, dc.ComplaintId });

            builder.HasOne(dc => dc.Department)
                .WithMany(c => c.DepartmentComplaints)
                .HasForeignKey(cq => cq.DepartmentId)
                .IsRequired();

            builder.HasOne(cq => cq.Complaint)
                .WithMany(q => q.DepartmentComplaint)
                .HasForeignKey(cq => cq.ComplaintId)
                .IsRequired();
        }
    }
}
