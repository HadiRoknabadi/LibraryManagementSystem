using Application.DTOs.Borrowing;
using Application.Utils;
using AutoMapper;
using Domain.Entities.Book;

namespace Infrastructure.MappingProfiles
{
    public class BorrowingMappingProfile:Profile
    {
        public BorrowingMappingProfile()
        {
            CreateMap<Borrowing, BorrowingListItemDTO>()
                .ForMember(b => b.BookName, m => m.MapFrom(m => m.BookCopy.Book.Title))
                .ForMember(b => b.UserFullName, m => m.MapFrom(m => m.User.FullName))
                .ForMember(b => b.LibrarianFullName, m => m.MapFrom(m => m.Librarian.FullName))
                .ForMember(b => b.BorrowDate, m => m.MapFrom(m => m.BorrowDate.ToShamsiDate()))
                .ForMember(b => b.DueDate, m => m.MapFrom(m => m.DueDate.ToShamsiDate()))
                .ForMember(d => d.ReturnDate,opt => opt.MapFrom(src => src.ReturnDate.HasValue
                ? src.ReturnDate.Value.ToShamsiDate():null));

            CreateMap<SubmitBorrowDTO, Borrowing>()
                .ForMember(s => s.BorrowDate, m => m.MapFrom(d => DateTime.Now))
                .ForMember(s => s.Status, m => m.MapFrom(d => BorrowingStatus.Borrowed));

            CreateMap<EditBorrowDTO, Borrowing>()
                .ForMember(d=>d.DueDate,d=>d.Ignore());

        }
    }
}
