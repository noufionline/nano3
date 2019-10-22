using Humanizer;
using Jasmine.Abs.Api.Repositories.Exceptions;
using Jasmine.Abs.Entities;
using Jasmine.Abs.Entities.Models.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Jasmine.Abs.Api.Repositories.Contracts
{
    public interface ILookupItemRepository<out T> where T : class, ILookupItemModel, new()
    {
        Task<bool> DeleteAsync(int id);
        Task<IEnumerable<LookupItem>> GetAllAsync(int id);
        Task<IEnumerable<LookupItem>> GetAllAsync();
        Task<ILookupItemModel> SaveAsync(ILookupItemModel item);
        Task<bool> IsDuplicatedAsync(ILookupItemModel item);
        Task<ILookupItemModel> UpdateAsync(ILookupItemModel item);
        Task<LookupItemModel> GetAsync(int id);
        Task<(bool hasError, ConcurrencyErrorInfo error)> GetConcurrencyErrorInfoAsync(ILookupItemModel item);


    }

     public class LookupItemRepository<T> : ILookupItemRepository<T>
        where T : class, ILookupItemModel,IAuditable, new()
    {
        private readonly AbsContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public string UserName => _httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "Anonymous";
        public LookupItemRepository(AbsContext context,IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> DeleteAsync(int id)
        {

            var item = await _context.Set<T>().SingleOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);

            if (item == null)
            {
                return false;
            }

            _context.Entry(item).State = EntityState.Deleted;

            try
            {
                return (await _context.SaveChangesAsync().ConfigureAwait(false))>0;
            }
            catch (DbUpdateException updateException)
            {
                if (updateException.GetBaseException() is SqlException exception && exception.Number==547)
                {
                    var errorMessage = $"{typeof(T).Name.Humanize()} {item.Name} is in use. Cannot delete!";
                    throw new EntityAlreadyInUseException(errorMessage);
                }
                throw;
            }
        }


        public async Task<IEnumerable<LookupItem>> GetAllAsync(int id)
        {

            var items = await _context.Set<T>()
                .Where(x => x.Id == id)
                .Select(x => new LookupItem
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToListAsync().ConfigureAwait(false);

            return items;
        }

        public async Task<IEnumerable<LookupItem>> GetAllAsync()
        {

            var items = await _context.Set<T>().Select(x => new LookupItem
            {
                Id = x.Id,
                Name = x.Name
            }).ToListAsync().ConfigureAwait(false);

            return items;
        }

        public async Task<ILookupItemModel> SaveAsync(ILookupItemModel item)
        {
            var data = new T
            {
                Name = item.Name,
                CreatedUser = UserName,
                CreatedDate = DateTime.Now
            };
            _context.Set<T>().Add(data);

            await _context.SaveChangesAsync().ConfigureAwait(false);
            return item;
        }


        public async Task<bool> IsDuplicatedAsync(ILookupItemModel item)
        {
            return item.Id==0 ? await _context.Set<T>()
                    .AnyAsync(x => x.Name == item.Name).ConfigureAwait(false) 
                : await _context.Set<T>()
                    .AnyAsync(x=> x.Id!=item.Id && x.Name==item.Name).ConfigureAwait(false);
        }


        public async Task<(bool hasError, ConcurrencyErrorInfo error)> GetConcurrencyErrorInfoAsync(ILookupItemModel item)
        {
            var lookupItem = await _context.Set<T>()
                .Where(x => x.Id == item.Id)
                .Select(x => new
                {
                    x.Name,
                    x.ModifiedUser,
                    x.ModifiedDate,
                    x.RowVersion
                }).SingleOrDefaultAsync();

            if (item.RowVersion.SequenceEqual(lookupItem.RowVersion)) return (false, null);

            var error = new ConcurrencyErrorInfo
            {
                Type = typeof(T).Name,
                Name = lookupItem.Name,
                User = $"{lookupItem.ModifiedUser ?? "Someone"}",
                ModifiedDate = $"{lookupItem.ModifiedDate:dd-MMM-yyyy HH:mm tt}"
            };

            return (true, error);

        }

    

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }


        public async Task<ILookupItemModel> UpdateAsync(ILookupItemModel lookupItem)
        {

            var item = _context.Set<T>()
                .AsTracking()
                .Single(x => x.Id == lookupItem.Id);

            if (!item.RowVersion.SequenceEqual(lookupItem.RowVersion))
            {
                throw new ConcurrencyException(new ConcurrencyErrorInfo()
                {
                    Type = typeof(T).Name,
                    Name = item.Name,
                    User = item.ModifiedUser,
                    ModifiedDate = $"{item.ModifiedDate:dd-MMM-yyyy HH:mm tt}"
                });
            }

            if (item.Name.Equals(lookupItem.Name))
            {
                return lookupItem;
            }

            item.Name = lookupItem.Name;
            item.ModifiedUser = UserName;
            item.ModifiedDate=DateTime.Now;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return new LookupItemModel(){Id = item.Id,Name = item.Name,RowVersion = item.RowVersion};
        }

        public Task<LookupItemModel> GetAsync(int id)
        {
            return _context.Set<T>().Select(x=> new LookupItemModel
            {
                Id = x.Id,
                Name = x.Name,
                RowVersion = x.RowVersion
            }).SingleOrDefaultAsync(x => x.Id == id);
        }

     
    }
}
