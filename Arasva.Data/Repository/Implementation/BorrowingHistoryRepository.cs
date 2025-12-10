using Arasva.Core.Interface;
using Arasva.Core.Models;
using Arasva.Data.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Data.Repository.Implementation
{
    public class BorrowingHistoryRepository : GenericRepository<BorrowingHistory>, IBorrowingHistoryRepository
    {
        private readonly AppDbContext _context;

        public BorrowingHistoryRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BorrowingHistory?> GetActiveBorrowForBookAsync(int bookId)
        {
            return await _context.BorrowingHistory
                .AsNoTracking()
                .FirstOrDefaultAsync(b => b.BookId == bookId && b.BorrowToDate == null);
        }

        public async Task<BorrowingHistory?> GetActiveBorrowForMemberAndBookAsync(int memberId, int bookId)
        {
            return await _context.BorrowingHistory
                .FirstOrDefaultAsync(b =>
                    b.MemberId == memberId &&
                    b.BookId == bookId &&
                    b.BorrowToDate == null);
        }

        public async Task<IEnumerable<BorrowingHistory>> GetMemberHistoryAsync(int memberId)
        {
            return await _context.BorrowingHistory
                .Include(b => b.Book)
                .Where(b => b.MemberId == memberId)
                .OrderByDescending(b => b.BorrowFromDate)
                .ToListAsync();
        }
    }
}
