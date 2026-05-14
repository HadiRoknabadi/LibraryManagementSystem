using Application.DTOs.BookCopy;
using AutoMapper;
using Domain.Entities.Book;

namespace Infrastructure.MappingProfiles
{
    public class BookCopyMappingProfile:Profile
    {
        public BookCopyMappingProfile()
        {
            CreateMap<BookCopy, BookCopyListItemDTO>()
                .ForMember(b=>b.BookName,m=>m.MapFrom(m=>m.Book.Title));

            CreateMap<AddBookCopyDTO, BookCopy>();

            CreateMap<EditBookCopyDTO, BookCopy>();
        }
    }
}
