using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core.DTO.Create
{
    public class MemberCreateDTO
    {
        [Required, MaxLength(500)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        [EmailAddress]
        public string Email { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }
    }
}
