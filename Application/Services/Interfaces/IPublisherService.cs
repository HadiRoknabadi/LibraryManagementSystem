using Application.DTOs.Publisher;

namespace Application.Services.Interfaces
{
    public interface IPublisherService
    {
        Task<FilterPublisherDTO> FilterPublisherAsync(FilterPublisherDTO filter);
    }
}
