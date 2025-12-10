using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core.DTO.Create
{
    public class BookCreateDTO
    {
        [Required, MaxLength(500)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string Author { get; set; }

        public int Pages { get; set; }
        public string? Category { get; set; }
        public bool? IsActive { get; set; }
        public int? CreatedBy { get; set; }
    }
}
