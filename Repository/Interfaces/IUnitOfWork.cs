namespace ComplaintRequestSystem.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRoleRepository Roles { get; }
        IUserRepository Users { get; }
        IComplaintRepository Complaints { get; }
        IRequestRepository Requests { get; }
        IDepartmentRepository Departments { get; }
        Task<int> SaveChangesAsync();
    }
}
