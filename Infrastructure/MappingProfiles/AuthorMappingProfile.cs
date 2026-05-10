using Application.DTOs.Author;
using AutoMapper;
using Domain.Entities.Book;

namespace Infrastructure.MappingProfiles
{
    public class AuthorMappingProfile:Profile
    {
        public AuthorMappingProfile()
        {
            CreateMap<Author, AuthorListItemDTO>();

            CreateMap<AddAuthorDTO, Author>();
        }
    }
}
