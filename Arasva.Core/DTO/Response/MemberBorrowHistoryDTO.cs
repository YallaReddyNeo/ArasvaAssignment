using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core.DTO.Response
{
    public class MemberBorrowHistoryDTO
    {
        public int Id { get; set; }

        public int BookId { get; set; }
        public string BookName { get; set; }

        public int MemberId { get; set; }
        public string MemberName { get; set; }

        public DateTime BorrowFromDate { get; set; }
        public DateTime? BorrowToDate { get; set; }

        public bool IsActiveBorrow => !BorrowToDate.HasValue;
    }
}
