using Application.DTOs.BookCategory;
using Application.DTOs.Common;
using Domain.Entities.Book;

namespace Application.Services.Interfaces
{
    public interface IBookCategoryService
    {
        Task<ResultDTO<GetBookCategoriesResult, List<BookCategoryListItemDTO>>> GetAllBookCategoriesAsync();
        Task<FilterBookCategoryDTO> FilterBookCategoryAsync(FilterBookCategoryDTO filter);
        Task<BookCategory> GetBookCategoryByIdAsync(int boogCategoryId);
        Task<ResultDTO<AddBookCategoryResult>> AddBookCategoryAsync(AddBookCategoryDTO addBookCategoryDTO);
        Task<ResultDTO<EditBookCategoryResult>> EditBookCategoryAsync(EditBookCategoryDTO editBookCategoryDTO);
        Task<ResultDTO<DeleteBookCategoryResult>> DeleteBookCategoryAsync(int bookCategoryId);
    }
}
