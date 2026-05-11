using Application.DTOs.Common;
using Application.DTOs.Publisher;

namespace Application.Services.Interfaces
{
    public interface IPublisherService
    {
        Task<FilterPublisherDTO> FilterPublisherAsync(FilterPublisherDTO filter);
        Task<ResultDTO<AddPublisherResult>> AddPublisherAsync(AddPublisherDTO addPublisherDTO);
    }
}
