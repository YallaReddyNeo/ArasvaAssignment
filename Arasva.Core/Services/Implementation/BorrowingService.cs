using Arasva.Core.DTO.Create;
using Arasva.Core.DTO.Response;
using Arasva.Core.DTO.Update;
using Arasva.Core.Interface;
using Arasva.Core.Models;
using Arasva.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arasva.Core.Services.Implementation
{
    public class BorrowingService : IBorrowingService
    {
        private readonly IBorrowingHistoryRepository _borrowingRepo;
        private readonly IBookRepository _bookRepo;
        private readonly IMemberRepository _memberRepo;

        public BorrowingService(
            IBorrowingHistoryRepository borrowingRepo,
            IBookRepository bookRepo,
            IMemberRepository memberRepo)
        {
            _borrowingRepo = borrowingRepo;
            _bookRepo = bookRepo;
            _memberRepo = memberRepo;
        }

        /// <summary>
        /// Borrow logic:
        /// 1. Validate book & member existence.
        /// 2. Prevent multiple active borrows of the same book (any member).
        /// 3. Create BorrowingHistory row with BorrowFromDate, CreatedBy, CreatedDate.
        /// </summary>
        public async Task<GlobalResponse<BorrowResponseDTO>> BorrowBookAsync(BorrowRequestDTO dto)
        {
            try
            {
                var book = await _bookRepo.GetByIdAsync(dto.BookId);
                if (book == null)
                    throw new KeyNotFoundException($"Book with ID {dto.BookId} not found.");

                var member = await _memberRepo.GetByIdAsync(dto.MemberId);
                if (member == null)
                    throw new KeyNotFoundException($"Member with ID {dto.MemberId} not found.");

                // Prevent multiple active borrows of the same book
                var existingActiveBorrow = await _borrowingRepo.GetActiveBorrowForBookAsync(dto.BookId);
                if (existingActiveBorrow != null)
                {
                    throw new InvalidOperationException("This book is currently borrowed and is not available.");
                }

                var borrow = new BorrowingHistory
                {
                    BookId = dto.BookId,
                    MemberId = dto.MemberId,
                    BorrowFromDate = dto.BorrowFromDate ?? DateTime.Now,
                    CreatedBy = dto.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                await _borrowingRepo.AddAsync(borrow);
                await _borrowingRepo.SaveAsync();

                var borrowBook = new BorrowResponseDTO
                {
                    Id = borrow.Id,
                    BookId = borrow.BookId,
                    BookName = book.Name,
                    MemberId = borrow.MemberId,
                    MemberName = member.Name,
                    BorrowFromDate = borrow.BorrowFromDate,
                    BorrowToDate = borrow.BorrowToDate,
                    CreatedBy = borrow.CreatedBy,
                    CreatedDate = borrow.CreatedDate,
                    ModifiedBy = borrow.ModifiedBy,
                    ModifiedDate = borrow.ModifiedDate
                };

                return new GlobalResponse<BorrowResponseDTO>
                {
                    Success = true,
                    Message = string.Format(AppConstants.ActionSuccess),
                    ErrorMessage = null,
                    Data = borrowBook
                };
            }
            catch (Exception ex)
            {
                return new GlobalResponse<BorrowResponseDTO>
                {
                    Success = false,
                    Message = null,
                    ErrorMessage = string.Format(AppConstants.ErrorMessage, ex.Message),
                    Data = null
                };
            }
        }

        /// <summary>
        /// Return logic:
        /// 1. Validate book & member existence.
        /// 2. Prevent return if there is no active borrow for that member & book.
        /// 3. Set BorrowToDate, ModifiedBy, ModifiedDate.
        /// </summary>
        public async Task<GlobalResponse<BorrowResponseDTO>> ReturnBookAsync(ReturnRequestDTO dto)
        {
            try
            {
                var book = await _bookRepo.GetByIdAsync(dto.BookId);
                if (book == null)
                    throw new KeyNotFoundException($"Book with ID {dto.BookId} not found.");

                var member = await _memberRepo.GetByIdAsync(dto.MemberId);
                if (member == null)
                    throw new KeyNotFoundException($"Member with ID {dto.MemberId} not found.");

                var activeBorrow = await _borrowingRepo.GetActiveBorrowForMemberAndBookAsync(dto.MemberId, dto.BookId);
                if (activeBorrow == null)
                {
                    // Prevent return of a book that is not currently borrowed
                    throw new InvalidOperationException("This member does not have an active borrow for this book.");
                }

                activeBorrow.BorrowToDate = dto.BorrowToDate ?? DateTime.Now;
                activeBorrow.ModifiedBy = dto.ModifiedBy;
                activeBorrow.ModifiedDate = DateTime.Now;

                _borrowingRepo.Update(activeBorrow);
                await _borrowingRepo.SaveAsync();

                var returnBook = new BorrowResponseDTO
                {
                    Id = activeBorrow.Id,
                    BookId = activeBorrow.BookId,
                    BookName = book.Name,
                    MemberId = activeBorrow.MemberId,
                    MemberName = member.Name,
                    BorrowFromDate = activeBorrow.BorrowFromDate,
                    BorrowToDate = activeBorrow.BorrowToDate,
                    CreatedBy = activeBorrow.CreatedBy,
                    CreatedDate = activeBorrow.CreatedDate,
                    ModifiedBy = activeBorrow.ModifiedBy,
                    ModifiedDate = activeBorrow.ModifiedDate
                };

                return new GlobalResponse<BorrowResponseDTO>
                {
                    Success = true,
                    Message = string.Format(AppConstants.ActionSuccess),
                    ErrorMessage = null,
                    Data = returnBook
                };
            }
            catch (Exception ex)
            {
                return new GlobalResponse<BorrowResponseDTO>
                {
                    Success = false,
                    Message = null,
                    ErrorMessage = string.Format(AppConstants.ErrorMessage, ex.Message),
                    Data = null
                };
            }
        }

        /// <summary>
        /// Full borrowing history for a member.
        /// </summary>
        public async Task<GlobalResponse<IEnumerable<MemberBorrowHistoryDTO>>> GetMemberHistoryAsync(int memberId)
        {
            var member = await _memberRepo.GetByIdAsync(memberId);
            if (member == null)
                throw new KeyNotFoundException($"Member with ID {memberId} not found.");

            var history = await _borrowingRepo.GetMemberHistoryAsync(memberId);

            return new GlobalResponse<IEnumerable<MemberBorrowHistoryDTO>>
            {
                Success = true,
                Message = string.Format(AppConstants.ActionSuccess),
                ErrorMessage = null,
                Data = history.Select(h => new MemberBorrowHistoryDTO
                {
                    Id = h.Id,
                    BookId = h.BookId,
                    BookName = h.Book?.Name ?? string.Empty,
                    MemberId = h.MemberId,
                    MemberName = h.Member?.Name ?? string.Empty,
                    BorrowFromDate = h.BorrowFromDate,
                    BorrowToDate = h.BorrowToDate
                }),
                TotalCount = history.Count()
            };
        }
    }
}
