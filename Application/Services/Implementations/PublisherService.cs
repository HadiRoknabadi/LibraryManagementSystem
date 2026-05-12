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

        public async Task<ResultDTO<GetPublishersResult, List<PublisherListItemDTO>>> GetPublishersAsync()
        {
            var result = new ResultDTO<GetPublishersResult, List<PublisherListItemDTO>>
            {
                Status=GetPublishersResult.Success,
                Message="اطلاعات با موفقیت دریافت شد",
                Data=new List<PublisherListItemDTO>()
            };

            var publishers = await _context.Publishers.AsQueryable().ToListAsync();

            if(publishers==null)
            {
                result.Status = GetPublishersResult.PublishersEmpty;
                result.Message = "ناشری یافت نشد";

                return result;
            }

            result.Data = _mapper.Map<List<Publisher>, List<PublisherListItemDTO>>(publishers);

            return result;
        }


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

        public async Task<Publisher> GetPublisherByIdAsync(int publisherId)
        {
            return await _context.Publishers.AsQueryable().SingleOrDefaultAsync(p => p.Id == publisherId);
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

        public async Task<ResultDTO<EditPublisherResult>> EditPublisherAsync(EditPublisherDTO editPublisherDTO)
        {
            var result = new ResultDTO<EditPublisherResult>
            {
                Status = EditPublisherResult.Success,
                Message = "ناشر با موفقیت ویراش شد"
            };

            var publisher = await GetPublisherByIdAsync(editPublisherDTO.Id);

            if(publisher==null)
            {
                result.Status = EditPublisherResult.NotFound;
                result.Message = "ناشر یافت نشد";

                return result;
            }

            var editedPublisher = _mapper.Map<EditPublisherDTO, Publisher>(editPublisherDTO, publisher);

            await _context.SaveChangesAsync();

            return result;

        }

        public async Task<ResultDTO<DeletePublisherResult>> DeletePublisherAsync(int publisherId)
        {
            var result = new ResultDTO<DeletePublisherResult>
            {
                Status=DeletePublisherResult.Success,
                Message="ناشر با موفقیت حذف شد"
            };

            var publisher=await GetPublisherByIdAsync(publisherId);

            if(publisher==null)
            {
                result.Status = DeletePublisherResult.NotFound;
                result.Message = "ناشری یافت نشد";

                return result;
            }

            publisher.IsDelete=true;

            await _context.SaveChangesAsync();

            return result;

        }


    }
}
