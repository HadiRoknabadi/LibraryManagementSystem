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

            CreateMap<AddBookDTO, Book>();
            CreateMap<Book, EditBookDTO>()
                .ForMember(b => b.AuthorIds,m=>m.MapFrom(f=>f.BookAuthors.Select(b=>b.AuthorId).ToList()));

            CreateMap<EditBookDTO, Book>();
        }
    }
}
