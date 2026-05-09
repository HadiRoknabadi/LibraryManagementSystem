using Application.DTOs.BookCategory;
using Application.DTOs.Common;

namespace Application.Services.Interfaces
{
    public interface IBookCategoryService
    {
        Task<FilterBookCategoryDTO> FilterBookCategoryAsync(FilterBookCategoryDTO filter);
        Task<ResultDTO<AddBookCategoryResult>> AddBookCategoryAsync(AddBookCategoryDTO addBookCategoryDTO);
    }
}
