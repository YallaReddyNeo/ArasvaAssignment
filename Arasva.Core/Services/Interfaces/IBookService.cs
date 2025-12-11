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
    public interface IBookService
    {
        Task<GlobalResponse> GetAllAsync(bool? isAvailable = null, string? author = null);
        //Task<GlobalResponse<IEnumerable<BookFullResponseDTO>>> GetAllAsync();
        Task<GlobalResponse<BookFullResponseDTO?>> GetByIdAsync(int id);
        Task<GlobalResponse<BookCreateResponseDTO>> CreateAsync(BookCreateDTO dto);
        Task<GlobalResponse<BookUpdateResponseDTO?>> UpdateAsync(int id, BookUpdateDTO dto);
    }
}
