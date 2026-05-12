using Application.DTOs.Book;
using Application.DTOs.Common;
using Application.DTOs.Paging;
using Application.Services.Interfaces;
using Application.Services.Interfaces.Context;
using AutoMapper;
using Domain.Entities.Book;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class BookService:IBookService
    {
        #region Constructor

        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;

        public BookService(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        public async Task<FilterBookDTO> FilterBookAsync(FilterBookDTO filter)
        {
            var query = _context.Books.AsQueryable().AsNoTracking();


            #region Order

            switch (filter.OrderBy)
            {
                case FilterDataOrder.CreateDate_ASC:
                    query = query.OrderBy(u => u.CreateDate);
                    break;

                case FilterDataOrder.CreateDate_DES:
                    query = query.OrderByDescending(u => u.CreateDate);
                    break;
            }

            #endregion

            #region Filter

            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(u => EF.Functions.Like(u.Title, $"%{filter.Title}%"));

            if (!string.IsNullOrEmpty(filter.ISBN))
                query = query.Where(u => EF.Functions.Like(u.ISBN, $"%{filter.ISBN}%"));

            #endregion



            #region Paging

            var allEntitiesCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, allEntitiesCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);

            var allEntities = _mapper.Map<List<Book>, List<BookListItemDTO>>(await query.Paging(pager).ToListAsync());

            #endregion

            return filter.SetPaging(pager).SetData(allEntities);
        }

        public async Task<Book> GetBookByIdAsync(int bookId)
        {
            return await _context.Books
                .Include(b=>b.BookAuthors)
                .AsQueryable()
                .SingleOrDefaultAsync(b => b.Id == bookId);
        }

        public async Task<ResultDTO<GetBookDetailsResult, EditBookDTO>> GetBookDetailsForEditAsync(int bookId)
        {
            var result = new ResultDTO<GetBookDetailsResult, EditBookDTO>
            {
                Status=GetBookDetailsResult.Success,
                Message="اطلاعات با موفقیت دریافت شد",
                Data=null
            };

            var book = await GetBookByIdAsync(bookId);

            if(book==null)
            {
                result.Status = GetBookDetailsResult.NotFound;
                result.Message = "کتابی یافت نشد";

                return result;
            }

            result.Data=_mapper.Map<Book,EditBookDTO>(book);

            return result;

        }


        public async Task<ResultDTO<AddBookResult>> AddBookAsync(AddBookDTO addBookDTO)
        {
            var result = new ResultDTO<AddBookResult>
            {
                Status=AddBookResult.Success,
                Message="کتاب با موفقیت اضافه شد"
            };

            var book = _mapper.Map<AddBookDTO, Book>(addBookDTO);

            //Add Book Authors
            book.BookAuthors = addBookDTO.AuthorIds.Select(authorId => new BookAuthor
            {
                AuthorId = authorId
            }).ToList();

            await _context.Books.AddAsync(book);

            await _context.SaveChangesAsync();

            return result;


        }

        public async Task<ResultDTO<EditBookResult>> EditBookAsync(EditBookDTO editBookDTO)
        {
            var result = new ResultDTO<EditBookResult>
            {
                Status=EditBookResult.Success,
                Message="کتاب با موفقیت ویرایش شد"
            };

            var book = await GetBookByIdAsync(editBookDTO.Id);

            if(book== null)
            {
                result.Status = EditBookResult.NotFound;
                result.Message = "کتابی یافت نشد";

                return result;
            }

            var editedBook = _mapper.Map<EditBookDTO, Book>(editBookDTO,book);

            //Remove Authors

            foreach (var item in editedBook.BookAuthors)
            {
                item.IsDelete = true;
            }

            //Add New Authors

            foreach (var authorId in editBookDTO.AuthorIds.Distinct())
            {
                book.BookAuthors.Add(new BookAuthor
                {
                    AuthorId = authorId,
                    BookId = book.Id
                });
            }

            await _context.SaveChangesAsync();

            return result;

        }

        public async Task<ResultDTO<DeleteBookResult>> DeleteBookAsync(int bookId)
        {
            var result = new ResultDTO<DeleteBookResult>
            {
                Status=DeleteBookResult.Success,
                Message="کتاب با موفقیت حذف شد"
            };

            var book=await GetBookByIdAsync(bookId);

            if(book==null)
            {
                result.Status=DeleteBookResult.NotFound;
                result.Message = "کتابی یافت نشد";

                return result;
            }

            book.IsDelete= true;

            await _context.SaveChangesAsync();

            return result;


        }




    }
}
