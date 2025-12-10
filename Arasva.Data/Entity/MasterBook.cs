using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Data.Entity
{
    public class MasterBook
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Author { get; set; }

        public int Pages { get; set; }

        public string? Category { get; set; }

        public bool? IsActive { get; set; } = true;

        public int? CreatedBy { get; set; } = 1;
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;

        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
