using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Arasva.Core.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(500)]
        public string Name { get; set; }

        [Required, MaxLength(500)]
        public string Author { get; set; }

        public int Pages { get; set; }
        public string? Category { get; set; }
        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public DateTime? ModifiedDate { get; set; }
    }
}
