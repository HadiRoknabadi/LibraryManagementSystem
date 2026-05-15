using Application.DTOs.BookCopy;
using Application.DTOs.Borrowing;
using Application.DTOs.Common;
using Application.DTOs.Paging;
using Application.Services.Interfaces;
using Application.Services.Interfaces.Context;
using AutoMapper;
using Domain.Entities.Book;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class BorrowingService:IBorrowingService
    {
        #region Constructor

        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;

        public BorrowingService(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        public async Task<FilterBorrowingDTO> FilterBorrowingAsync(FilterBorrowingDTO filter)
        {
            var query = _context.Borrowings
            .Include(b => b.User)
            .Include(b=>b.BookCopy)
            .ThenInclude(b=>b.Book)
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



            #endregion



            #region Paging

            var allEntitiesCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, allEntitiesCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);

            var allEntities = _mapper.Map<List<Borrowing>, List<BorrowingListItemDTO>>(await query.Paging(pager).ToListAsync());

            #endregion

            return filter.SetPaging(pager).SetData(allEntities);
        }





    }
}
