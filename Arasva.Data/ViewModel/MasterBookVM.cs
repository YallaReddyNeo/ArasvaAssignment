using System.ComponentModel.DataAnnotations;

namespace Arasva.Data.ViewModel
{
    public class MasterBookVM
    {
        public int? Id { get; set; }

        [StringLength(500)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Author { get; set; }

        public int Pages { get; set; }

        public string? Category { get; set; }

        public bool? IsActive { get; set; } = true;
    }
}
