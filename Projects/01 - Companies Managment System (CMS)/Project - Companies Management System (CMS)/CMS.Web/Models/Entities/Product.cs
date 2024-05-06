using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Web.Models.Entities
{
    public class Product
    {
        [Key]
        public Guid ProdID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public required string Name { get; set; }

        public string? Description { get; set; }

        public required int Price { get; set; }

        public Guid CompID { get; set; }
        [ForeignKey("CompID")]

        public virtual required Company Company { get; set; }
    }
}
