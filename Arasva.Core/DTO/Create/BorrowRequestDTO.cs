using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core.DTO.Create
{
    public class BorrowRequestDTO
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int MemberId { get; set; }

        /// <summary>
        /// Optional. If null, service will use DateTime.Now
        /// </summary>
        public DateTime? BorrowFromDate { get; set; }

        public int? CreatedBy { get; set; }
    }
}
