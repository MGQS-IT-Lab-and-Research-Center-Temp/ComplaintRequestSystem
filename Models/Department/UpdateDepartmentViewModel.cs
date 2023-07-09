using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ComplaintRequestSystem.Models.Department
{
    public class UpdateDepartmentViewModel
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
