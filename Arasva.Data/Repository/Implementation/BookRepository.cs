using Arasva.Core.Interface;
using Arasva.Core.Models;
using Arasva.Data.Data;
using Microsoft.EntityFrameworkCore;

namespace Arasva.Data.Repository.Implementation
{
    public class BookRepository : GenericRepository<Book>, IBookRepository
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Filter by availability and author.
        /// Availability:
        ///  - Available   = no active BorrowingHistory (BorrowToDate == null)
        ///  - Unavailable = at least one active BorrowingHistory (BorrowToDate == null)
        /// </summary>
        public async Task<IEnumerable<Book>> GetFilteredAsync(bool? isAvailable, string? author)
        {
            var query = _context.Books.AsQueryable();

            // Author filter (case-insensitive "contains")
            if (!string.IsNullOrWhiteSpace(author))
            {
                var lowerAuthor = author.ToLower();
                query = query.Where(b => b.Author.ToLower().Contains(lowerAuthor));
            }

            // Availability filter
            if (isAvailable.HasValue)
            {
                if (isAvailable.Value)
                {
                    // available = no active borrowing for that book
                    query = query.Where(b =>
                        !_context.BorrowingHistory.Any(h =>
                            h.BookId == b.Id &&
                            h.BorrowToDate == null));
                }
                else
                {
                    // unavailable = at least one active borrow
                    query = query.Where(b =>
                        _context.BorrowingHistory.Any(h =>
                            h.BookId == b.Id &&
                            h.BorrowToDate == null));
                }
            }

            return await query.ToListAsync();
        }
    }
}
