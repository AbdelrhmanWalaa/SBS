using CMS.Web.Models.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CMS.Web.Models
{
    public class AddDepartmentViewModel
    {
        [Key]
        public Guid DepID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public required string Name { get; set; }

        public Guid CompID { get; set; }
        [ForeignKey("CompID")]

        public virtual required Company Company { get; set; }
    }
}
