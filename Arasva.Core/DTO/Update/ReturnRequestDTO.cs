using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core.DTO.Update
{
    public class ReturnRequestDTO
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public int MemberId { get; set; }

        /// <summary>
        /// Optional. If null, service will use DateTime.Now
        /// </summary>
        public DateTime? BorrowToDate { get; set; }

        public int? ModifiedBy { get; set; }
    }
}
