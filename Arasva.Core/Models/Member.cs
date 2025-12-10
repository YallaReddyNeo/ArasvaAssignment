using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core.Models
{
    public class Member
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(500)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string Email { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
