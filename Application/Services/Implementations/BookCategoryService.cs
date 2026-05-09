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
    public class BookCategoryService:IBookCategoryService
    {
        #region Constructor

        private readonly IDatabaseContext _context;
        private readonly IMapper _mapper;

        public BookCategoryService(IDatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        #endregion

        public async Task<FilterBookCategoryDTO> FilterBookCategoryAsync(FilterBookCategoryDTO filter)
        {
            var query = _context.BookCategories.AsQueryable().AsNoTracking();


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

            #endregion



            #region Paging

            var allEntitiesCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, allEntitiesCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);

            var allEntities = _mapper.Map<List<BookCategory>, List<BookCategoryListItemDTO>>(await query.Paging(pager).ToListAsync());

            #endregion

            return filter.SetPaging(pager).SetData(allEntities);
        }

        public async Task<BookCategory> GetBookCategoryByIdAsync(int boogCategoryId)
        {
            return await _context.BookCategories.AsQueryable().SingleOrDefaultAsync(b => b.Id == boogCategoryId);
        }


        public async Task<ResultDTO<AddBookCategoryResult>> AddBookCategoryAsync(AddBookCategoryDTO addBookCategoryDTO)
        {
            var result = new ResultDTO<AddBookCategoryResult>
            {
                Status=AddBookCategoryResult.Success,
                Message="دسته بندی با موفقیت اضافه شد"

            };

            var bookCategory=_mapper.Map<AddBookCategoryDTO,BookCategory>(addBookCategoryDTO);

            await _context.BookCategories.AddAsync(bookCategory);

            await _context.SaveChangesAsync();

            return result;

        }

        public async Task<ResultDTO<EditBookCategoryResult>> EditBookCategoryAsync(EditBookCategoryDTO editBookCategoryDTO)
        {
            var result = new ResultDTO<EditBookCategoryResult>
            {
                Status=EditBookCategoryResult.Success,
                Message="دسته بندی با موفقیت ویرایش شد"
            };

            var boogCategory = await GetBookCategoryByIdAsync(editBookCategoryDTO.Id);

            if(boogCategory==null)
            {
                result.Status = EditBookCategoryResult.NotFound;
                result.Message = "دسته بندی یافت نشد";

                return result;
            }

            var editedBookCategory = _mapper.Map<EditBookCategoryDTO, BookCategory>(editBookCategoryDTO, boogCategory);

            await _context.SaveChangesAsync();

            return result;

        }


    }
}
