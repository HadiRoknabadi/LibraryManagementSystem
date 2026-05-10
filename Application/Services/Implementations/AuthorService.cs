using Application.DTOs.Author;
using Application.DTOs.BookCategory;
using Application.DTOs.Common;
using Application.DTOs.Paging;
using Application.Services.Interfaces;
using Application.Services.Interfaces.Context;
using AutoMapper;
using Domain.Entities.Book;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class AuthorService : IAuthorService
    {
        #region Constructor

        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;

        public AuthorService(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        public async Task<FilterAuthorDTO> FilterAuthorAsync(FilterAuthorDTO filter)
        {
            var query = _context.Authors.AsQueryable().AsNoTracking();


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

            if (!string.IsNullOrEmpty(filter.FullName))
                query = query.Where(u => EF.Functions.Like(u.FullName, $"%{filter.FullName}%"));

            #endregion



            #region Paging

            var allEntitiesCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, allEntitiesCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);

            var allEntities = _mapper.Map<List<Author>, List<AuthorListItemDTO>>(await query.Paging(pager).ToListAsync());

            #endregion

            return filter.SetPaging(pager).SetData(allEntities);

        }

        public async Task<ResultDTO<AddAuthorResult>> AddAuthorAsync(AddAuthorDTO addAuthorDTO)
        {
            var result = new ResultDTO<AddAuthorResult>
            {
                Status = AddAuthorResult.Success,
                Message = "نویسنده با موفقیت اضافه شد"

            };

            var author = _mapper.Map<AddAuthorDTO, Author>(addAuthorDTO);

            await _context.Authors.AddAsync(author);

            await _context.SaveChangesAsync();

            return result;
        }


    }
}
