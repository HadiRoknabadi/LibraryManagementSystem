using Application.DTOs.BookCategory;
using AutoMapper;
using Domain.Entities.Book;

namespace Infrastructure.MappingProfiles
{
    public class BookCategoryMappingProfile:Profile
    {
        public BookCategoryMappingProfile()
        {
            CreateMap<BookCategory, BookCategoryListItemDTO>();
        }
    }
}
