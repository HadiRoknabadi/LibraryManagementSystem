using Application.DTOs.Publisher;
using AutoMapper;
using Domain.Entities.Book;

namespace Infrastructure.MappingProfiles
{
    public class PublisherMappingProfile:Profile
    {
        public PublisherMappingProfile()
        {
            CreateMap<Publisher, PublisherListItemDTO>();
            CreateMap<AddPublisherDTO, Publisher>();
            CreateMap<EditPublisherDTO, Publisher>();
        }
    }
}
