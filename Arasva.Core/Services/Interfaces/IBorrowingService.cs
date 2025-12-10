using Arasva.Core.DTO.Create;
using Arasva.Core.DTO.Response;
using Arasva.Core.DTO.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core.Services.Interfaces
{
    public interface IBorrowingService
    {
        Task<GlobalResponse<BorrowResponseDTO>> BorrowBookAsync(BorrowRequestDTO dto);
        Task<GlobalResponse<BorrowResponseDTO>> ReturnBookAsync(ReturnRequestDTO dto);
        Task<GlobalResponse<IEnumerable<MemberBorrowHistoryDTO>>> GetMemberHistoryAsync(int memberId);
    }
}
