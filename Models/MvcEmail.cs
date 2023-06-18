using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ComplaintRequestSystem.Models
{
    public class MvcEmail
    {
        public class EmailFormModel
        {
            [Required, Display(Name = "Your name")]
            public string FromName { get; set; }
            [Required, Display(Name = "Your email"), EmailAddress]
            public string FromEmail { get; set; }
            [Required]
            public string Message { get; set; }
        }
    }
}
