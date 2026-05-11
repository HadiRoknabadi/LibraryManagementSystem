using Application.DTOs.Common;
using Application.DTOs.Publisher;
using Domain.Entities.Book;

namespace Application.Services.Interfaces
{
    public interface IPublisherService
    {
        Task<FilterPublisherDTO> FilterPublisherAsync(FilterPublisherDTO filter);
        Task<Publisher> GetPublisherByIdAsync(int publisherId);
        Task<ResultDTO<AddPublisherResult>> AddPublisherAsync(AddPublisherDTO addPublisherDTO);
        Task<ResultDTO<EditPublisherResult>> EditPublisherAsync(EditPublisherDTO editPublisherDTO);
    }
}
