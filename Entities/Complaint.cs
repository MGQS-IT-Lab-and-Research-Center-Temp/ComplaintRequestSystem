﻿using ComplaintRequestSystem.Helper.Enum;

namespace ComplaintRequestSystem.Entities
{
    public class Complaint : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public Department Department { get; set; }
        public bool IsClosed { get; set; }
        public string? ImageUrl { get; set; }
        public ComplaintStatus status { get; set; }
        public ICollection<DepartmentComplaint> DepartmentComplaint { get; set; } = new HashSet<DepartmentComplaint>();
        public string ComplaintText { get; set; }
    }
}
