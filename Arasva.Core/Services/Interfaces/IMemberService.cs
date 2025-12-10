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
    public interface IMemberService
    {
        Task<GlobalResponse<IEnumerable<MemberFullResponseDTO>>> GetAllAsync();
        Task<GlobalResponse<MemberFullResponseDTO?>> GetByIdAsync(int id);
        Task<GlobalResponse<MemberCreateResponseDTO>> CreateAsync(MemberCreateDTO dto);
        Task<GlobalResponse<MemberUpdateResponseDTO?>> UpdateAsync(int id, MemberUpdateDTO dto);
    }
}
