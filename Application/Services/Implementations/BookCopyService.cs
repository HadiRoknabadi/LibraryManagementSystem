using Application.DTOs.BookCopy;
using Application.DTOs.Common;
using Application.DTOs.Paging;
using Application.Services.Interfaces;
using Application.Services.Interfaces.Context;
using AutoMapper;
using Domain.Entities.Book;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class BookCopyService : IBookCopyService
    {
        #region Constructor

        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;

        public BookCopyService(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        public async Task<FilterBookCopyDTO> FilterBookCopyAsync(FilterBookCopyDTO filter)
        {
            var query = _context.BookCopies
                .Include(b=>b.Book)
                .AsQueryable().AsNoTracking();


            #region Status

            query = query.Where(u => u.Status == filter.Status);

            #endregion

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

            if (!string.IsNullOrEmpty(filter.BookName))
                query = query.Where(u => EF.Functions.Like(u.Book.Title, $"%{filter.BookName}%"));

            if (!string.IsNullOrEmpty(filter.InventoryCode))
                query = query.Where(u => EF.Functions.Like(u.InventoryCode, $"%{filter.InventoryCode}%"));

            #endregion



            #region Paging

            var allEntitiesCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, allEntitiesCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);

            var allEntities = _mapper.Map<List<BookCopy>, List<BookCopyListItemDTO>>(await query.Paging(pager).ToListAsync());

            #endregion

            return filter.SetPaging(pager).SetData(allEntities);
        }
    }
}
