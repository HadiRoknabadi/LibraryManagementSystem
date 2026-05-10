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

            if (!string.IsNullOrEmpty(filter.Name))
                query = query.Where(u => EF.Functions.Like(u.Name, $"%{filter.Name}%"));

            if (!string.IsNullOrEmpty(filter.Family))
                query = query.Where(u => EF.Functions.Like(u.Family, $"%{filter.Family}%"));

            #endregion



            #region Paging

            var allEntitiesCount = await query.CountAsync();

            var pager = Pager.Build(filter.PageId, allEntitiesCount, filter.TakeEntity, filter.HowManyShowPageAfterAndBefore);

            var allEntities = _mapper.Map<List<Author>, List<AuthorListItemDTO>>(await query.Paging(pager).ToListAsync());

            #endregion

            return filter.SetPaging(pager).SetData(allEntities);

        }

        public async Task<Author> GetAuthorByIdAsync(int authorId)
        {
            return await _context.Authors.AsQueryable().SingleOrDefaultAsync(a => a.Id == authorId);
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

        public async Task<ResultDTO<EditAuthorResult>> EditAuthorAsync(EditAuthorDTO editAuthorDTO)
        {
            var result = new ResultDTO<EditAuthorResult>
            {
                Status = EditAuthorResult.Success,
                Message = "نویسنده با موفقیت ویرایش شد"
            };

            var author = await GetAuthorByIdAsync(editAuthorDTO.Id);

            if (author == null)
            {
                result.Status = EditAuthorResult.NotFound;
                result.Message = "نویسنده یافت نشد";

                return result;
            }

            var editedBookCategory = _mapper.Map<EditAuthorDTO, Author>(editAuthorDTO, author);

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<ResultDTO<DeleteAuthorResult>> DeleteAuthorAsync(int authorId)
        {
            var result = new ResultDTO<DeleteAuthorResult>
            {
                Status = DeleteAuthorResult.Success,
                Message = "نویسنده با موفقیت حذف شد"
            };

            var author = await GetAuthorByIdAsync(authorId);

            if (author == null)
            {
                result.Status = DeleteAuthorResult.NotFound;
                result.Message = "نویسنده یافت نشد";

                return result;
            }

            author.IsDelete = true;

            await _context.SaveChangesAsync();

            return result;
        }



    }
}
