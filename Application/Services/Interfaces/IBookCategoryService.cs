using Application.DTOs.BookCategory;

namespace Application.Services.Interfaces
{
    public interface IBookCategoryService
    {
        Task<FilterBookCategoryDTO> FilterBookCategoryAsync(FilterBookCategoryDTO filter);
    }
}
