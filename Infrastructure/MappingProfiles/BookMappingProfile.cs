using Application.DTOs.Book;
using AutoMapper;
using Domain.Entities.Book;

namespace Infrastructure.MappingProfiles
{
    public class BookMappingProfile:Profile
    {
        public BookMappingProfile()
        {
            CreateMap<Book, BookListItemDTO>();
        }
    }
}
