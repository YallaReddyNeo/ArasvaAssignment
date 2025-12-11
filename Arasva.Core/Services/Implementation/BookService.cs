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
    public class BookService : IBookService
    {
        private readonly IBookRepository _repo;

        public BookService(IBookRepository repo)
        {
            _repo = repo;
        }

        public async Task<GlobalResponse<IEnumerable<BookFullResponseDTO>>> GetAllAsync(bool? isAvailable = null, string? author = null)
        {
            IEnumerable<Book> books;

            // If no filters, fetch all.
            if (!isAvailable.HasValue && string.IsNullOrWhiteSpace(author))
            {
                books = await _repo.GetAllAsync();
            }
            else
            {
                books = await _repo.GetFilteredAsync(isAvailable, author);
            }

            var data = books.Select(b => new BookFullResponseDTO
            {
                Id = b.Id,
                Name = b.Name,
                Author = b.Author,
                Pages = b.Pages,
                Category = b.Category,
                IsActive = b.IsActive,
                CreatedBy = b.CreatedBy,
                CreatedDate = b.CreatedDate,
                ModifiedBy = b.ModifiedBy,
                ModifiedDate = b.ModifiedDate
            });

            return new GlobalResponse<IEnumerable<BookFullResponseDTO>>
            {
                success = true,
                message = string.Format(AppConstants.ActionSuccess),
                error = null,
                data = data,
                totalcount = books.Count()
            };
        }

        //public async Task<GlobalResponse<IEnumerable<BookFullResponseDTO>>> GetAllAsync()
        //{
        //    var books = await _repo.GetAllAsync();

        //    return new GlobalResponse<IEnumerable<BookFullResponseDTO>>
        //    {
        //        Success = true,
        //        Message = string.Format(AppConstants.ActionSuccess),
        //        ErrorMessage = null,
        //        Data = books.Select(b => new BookFullResponseDTO
        //        {
        //            Id = b.Id,
        //            Name = b.Name,
        //            Author = b.Author,
        //            Pages = b.Pages,
        //            Category = b.Category,
        //            IsActive = b.IsActive,
        //            CreatedBy = b.CreatedBy,
        //            CreatedDate = b.CreatedDate,
        //            ModifiedBy = b.ModifiedBy,
        //            ModifiedDate = b.ModifiedDate
        //        }),
        //        TotalCount = books.Count()
        //    };
        //}

        public async Task<GlobalResponse<BookFullResponseDTO?>> GetByIdAsync(int id)
        {
            var b = await _repo.GetByIdAsync(id);
            if (b == null) return null;

            return new GlobalResponse<BookFullResponseDTO?>
            {
                success = true,
                message = string.Format(AppConstants.ActionSuccess),
                error = null,
                totalcount = 1,
                data = new BookFullResponseDTO
                {
                    Id = b.Id,
                    Name = b.Name,
                    Author = b.Author,
                    Pages = b.Pages,
                    Category = b.Category,
                    IsActive = b.IsActive,
                    CreatedBy = b.CreatedBy,
                    CreatedDate = b.CreatedDate,
                    ModifiedBy = b.ModifiedBy,
                    ModifiedDate = b.ModifiedDate
                }
            };
        }

        public async Task<GlobalResponse<BookCreateResponseDTO>> CreateAsync(BookCreateDTO dto)
        {
            try
            {
                var book = new Book
                {
                    Name = dto.Name,
                    Author = dto.Author,
                    Pages = dto.Pages,
                    Category = dto.Category,
                    IsActive = dto.IsActive,
                    CreatedBy = dto.CreatedBy,
                    CreatedDate = DateTime.Now
                };

                await _repo.AddAsync(book);
                await _repo.SaveAsync();

                var createdBook = new BookCreateResponseDTO
                {
                    Id = book.Id,
                    Name = book.Name,
                    Author = book.Author,
                    Pages = book.Pages,
                    Category = book.Category,
                    IsActive = book.IsActive,
                    CreatedBy = book.CreatedBy,
                    CreatedDate = book.CreatedDate
                };

                return new GlobalResponse<BookCreateResponseDTO>
                {
                    success = true,
                    message = string.Format(AppConstants.ActionSuccess),
                    error = null,
                    data = createdBook
                };
            }
            catch (Exception ex)
            {
                return new GlobalResponse<BookCreateResponseDTO>
                {
                    success = false,
                    message = null,
                    error = string.Format(AppConstants.ErrorMessage, ex.Message),
                    data = null
                };
            }
        }

        public async Task<GlobalResponse<BookUpdateResponseDTO?>> UpdateAsync(int id, BookUpdateDTO dto)
        {
            try
            {
                var book = await _repo.GetByIdAsync(id);
                if (book == null) return null;

                book.Name = dto.Name;
                book.Author = dto.Author;
                book.Pages = dto.Pages;
                book.Category = dto.Category;
                book.IsActive = dto.IsActive;
                book.ModifiedBy = dto.ModifiedBy;
                book.ModifiedDate = DateTime.Now;

                _repo.Update(book);
                await _repo.SaveAsync();

                var updatedBook = new BookUpdateResponseDTO
                {
                    Id = book.Id,
                    Name = book.Name,
                    Author = book.Author,
                    Pages = book.Pages,
                    Category = book.Category,
                    IsActive = book.IsActive,
                    ModifiedBy = book.ModifiedBy,
                    ModifiedDate = book.ModifiedDate
                };

                return new GlobalResponse<BookUpdateResponseDTO?>
                {
                    success = true,
                    message = string.Format(AppConstants.ActionSuccess),
                    error = null,
                    data = updatedBook
                };

            }
            catch (Exception ex)
            {
                return new GlobalResponse<BookUpdateResponseDTO?>
                {
                    success = false,
                    message = null,
                    error = string.Format(AppConstants.ErrorMessage, ex.Message),
                    data = null
                };
            }
        }
    }
}
