using Arasva.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core.Interface
{
    public interface IBorrowingHistoryRepository : IGenericRepository<BorrowingHistory>
    {
        /// <summary>
        /// Returns active borrow of a specific book (BorrowToDate is null), if any.
        /// </summary>
        Task<BorrowingHistory?> GetActiveBorrowForBookAsync(int bookId);

        /// <summary>
        /// Returns active borrow for given member and book, if any.
        /// </summary>
        Task<BorrowingHistory?> GetActiveBorrowForMemberAndBookAsync(int memberId, int bookId);

        /// <summary>
        /// Returns full borrowing history for a member (includes Book navigation).
        /// </summary>
        Task<IEnumerable<BorrowingHistory>> GetMemberHistoryAsync(int memberId);
    }
}
