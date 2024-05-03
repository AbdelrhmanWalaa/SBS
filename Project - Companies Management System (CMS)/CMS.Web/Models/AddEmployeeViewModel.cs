using CMS.Web.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMS.Web.Models
{
    public class AddEmployeeViewModel
    {
        [Key]
        public Guid EmpID { get; set; }

        [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
        public required string Name { get; set; }

        [StringLength(50, ErrorMessage = "Position cannot be longer than 50 characters.")]
        public required string Position { get; set; }

        public string? Account { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public Guid CompID { get; set; }
        [ForeignKey("CompID")]

        public virtual required Company Company { get; set; }
    }
}
