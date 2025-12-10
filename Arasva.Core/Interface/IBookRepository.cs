using Arasva.Core.Models;

namespace Arasva.Core.Interface
{
    public interface IBookRepository : IGenericRepository<Book>
    {
        Task<IEnumerable<Book>> GetFilteredAsync(bool? isAvailable, string? author);
    }
}
