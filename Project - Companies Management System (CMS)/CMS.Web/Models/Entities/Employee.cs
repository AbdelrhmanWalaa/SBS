using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Web.Models.Entities
{
    public class Employee
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
