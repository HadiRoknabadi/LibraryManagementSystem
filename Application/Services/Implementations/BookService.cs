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



    }
}
