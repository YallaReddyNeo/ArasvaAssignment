using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core.DTO.Response
{
    public class MemberCreateResponseDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public bool? IsActive { get; set; }

        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
