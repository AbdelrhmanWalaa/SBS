using System.ComponentModel.DataAnnotations;

namespace CMS.Web.Models
{
    public class AddCompanyViewModel
    {
        [Key]
        public Guid CompID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public required string Name { get; set; }

        public required string Address { get; set; }
    }
}
