using Application.DTOs.Common;
using Application.DTOs.Paging;
using Application.DTOs.Publisher;
using Application.Services.Interfaces;
using Application.Services.Interfaces.Context;
using AutoMapper;
using Domain.Entities.Book;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.Implementations
{
    public class PublisherService : IPublisherService
    {
        #region Constructor

        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;

        public PublisherService(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #endregion

        public async Task<FilterPublisherDTO> FilterPublisherAsync(FilterPublisherDTO filter)
        {
            var query = _context.Publishers.AsQueryable().AsNoTracking();


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

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(u => EF.Functions.Like(u.Name, $"%{filter.Name}%"));

            if (!string.IsNullOrEmpty(filter.PhoneNumber))
                query = query.Where(u => EF.Functions.Like(u.PhoneNumber, $"%{filter.PhoneNumber}%"));

            #endregion



            #region Paging

            var allEntitiesCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, allEntitiesCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);

            var allEntities = _mapper.Map<List<Publisher>, List<PublisherListItemDTO>>(await query.Paging(pager).ToListAsync());

            #endregion

            return filter.SetPaging(pager).SetData(allEntities);
        }

        public async Task<ResultDTO<AddPublisherResult>> AddPublisherAsync(AddPublisherDTO addPublisherDTO)
        {
            var result = new ResultDTO<AddPublisherResult>
            {
                Status=AddPublisherResult.Success,
                Message="ناشر با موفقیت اضافه شد"
            };

            var publisher = _mapper.Map<AddPublisherDTO, Publisher>(addPublisherDTO);

            await _context.Publishers.AddAsync(publisher);

            await _context.SaveChangesAsync();

            return result;
        }


    }
}
