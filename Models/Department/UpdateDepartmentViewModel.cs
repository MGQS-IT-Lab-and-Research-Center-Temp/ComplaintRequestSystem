using System.ComponentModel.DataAnnotations;

namespace ComplaintRequestSystem.Models.Department
{
    public class UpdateDepartmentViewModel
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Department name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
