using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vega.Core.Entities
{
    [Table("Models")]
    public class Model
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        public int MakeId { get; set; }
        public Make Make { get; set; }
    }
}