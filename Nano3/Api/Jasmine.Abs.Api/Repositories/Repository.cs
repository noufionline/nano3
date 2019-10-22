using AutoMapper;
using Humanizer;
using Jasmine.Abs.Api.Repositories.Contracts;
using Jasmine.Abs.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;
using Jasmine.Abs.Api.Repositories.Exceptions;
using TrackableEntities.EF.Core;

namespace Jasmine.Abs.Api.Repositories
{
    public abstract class Repository<T, TModel> : IRepository<TModel>
        where TModel : class, IEntity, ITrackable, IMergeable
        where T : class, IEntity, ITrackable, IMergeable
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<Repository<T, TModel>> _logger;

        public Repository(DbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ILogger<Repository<T, TModel>> logger)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }


        public virtual async Task<List<TModel>> GetAllAsync()
        {
            var items = await GetEntitiesAsync().ConfigureAwait(false);
            return _mapper.Map<List<TModel>>(items);
        }

        protected abstract Task<List<TModel>> GetEntitiesAsync();// => _context.Set<T>().ToListAsync();

        public async Task<TModel> GetAsync(int id)
        {
            var item = await GetEntityAsync(id).ConfigureAwait(false);
            return item;
            //return _mapper.Map<TModel>(item);
        }

        protected abstract Task<TModel> GetEntityAsync(int id);// => _context.Set<T>().SingleOrDefaultAsync(x => x.Id == id);


        protected string UserName => _httpContextAccessor.HttpContext.User.Identity.Name ?? "Anonymous User";

        public async Task<TModel> SaveAsync(TModel model)
        {
            var entity = _mapper.Map<T>(model);

            if (entity.Id == 0)
            {
                entity.TrackingState = TrackingState.Added;
                if (entity.TrackingState == TrackingState.Added && entity is IAuditable auditable)
                {
                    auditable.CreatedDate = DateTime.Now;
                    auditable.CreatedUser = UserName;
                }
            }
            else
            {
                throw new InvalidOperationException("Id must be zero");
            }


            // Apply changes to context
            _context.ApplyChanges(entity);


            try
            {
                await OnBeforeSaveAsync(_context, entity);
                // Persist changes
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Set<T>().Any(o => o.Id == model.Id))
                    throw new EntityNotFoundException();
                throw;
            }

           
            _context.AcceptChanges(entity);

            return _mapper.Map<TModel>(entity);

        }

        protected virtual Task OnBeforeSaveAsync(DbContext context, T entity) => Task.CompletedTask;

        public async Task<TModel> UpdateAsync(TModel model)
        {
            var entity = _mapper.Map<T>(model);


            if (entity.Id == 0)
            {
                throw new InvalidOperationException("Id cannot be empty");
            }


            if (entity.TrackingState == TrackingState.Modified && entity is IAuditable auditable)
            {
                auditable.ModifiedDate = DateTime.Now;
                auditable.ModifiedUser = UserName;
            }

            //// Apply changes to context
            _context.ApplyChanges(entity);

            try
            {
                // Persist changes
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                _logger.LogError(exception, exception.Message);
                if (!_context.Set<T>().Any(o => o.Id == model.Id))
                    throw new EntityNotFoundException();
                throw;
            }

            _context.AcceptChanges(entity);

           return _mapper.Map<TModel>(entity);

        }

        public async Task<bool> DeleteAsync(TModel model)
        {

            var entity = _mapper.Map<T>(model);
            _context.Entry(entity).State = EntityState.Deleted;

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (DbUpdateException updateException)
            {
                if (updateException.GetBaseException() is SqlException exception && exception.Number == 547)
                {
                    throw new EntityAlreadyInUseException("This Document is already in use.", updateException);
                }

                throw;
            }
        }

     
    }


    public abstract class Repository<T, TModel, TList> : IRepository<TModel, TList>
        where TModel : class, IEntity, ITrackable, IMergeable
        where T : class, IEntity, ITrackable, IMergeable
        where TList : class, IEntity
    {
        private readonly DbContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<Repository<T, TModel, TList>> _logger;

        protected Repository(DbContext context, IMapper mapper, IHttpContextAccessor httpContextAccessor,
            ILogger<Repository<T, TModel, TList>> logger)
        {
            _context = context;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }


        public virtual Task<List<TList>> GetAllAsync()
        {
            return GetEntitiesAsync();
        }

        protected abstract Task<List<TList>> GetEntitiesAsync();// => _context.Set<T>().ProjectTo<TList>().ToListAsync();

        public Task<TModel> GetAsync(int id)
        {
            return GetEntityAsync(id);

        }

        protected abstract Task<TModel> GetEntityAsync(int id);// => _context.Set<T>().ProjectTo<TModel>().SingleOrDefaultAsync(x => x.Id == id);


        protected string UserName => _httpContextAccessor.HttpContext.User.Identity.Name ?? "Anonymous User";
        protected int DivisionId => Convert.ToInt32(_httpContextAccessor.HttpContext.User.FindFirst("DivisionId")?.Value);

        

        public async Task<TModel> SaveAsync(TModel model)
        {
            var entity = _mapper.Map<T>(model);

            if (entity.Id == 0)
            {
                entity.TrackingState = TrackingState.Added;
                if (entity.TrackingState == TrackingState.Added && entity is IAuditable auditable)
                {
                    auditable.CreatedDate = DateTime.Now;
                    auditable.CreatedUser = UserName;
                }
            }
            else
            {
                throw new InvalidOperationException("Id must be zero");
            }


            // Apply changes to context
            _context.ApplyChanges(entity);


            try
            {
                await OnBeforeSaveAsync(_context, entity);
                // Persist changes
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Set<T>().Any(o => o.Id == model.Id))
                    throw new EntityNotFoundException();
                throw;
            }

            _context.AcceptChanges(entity);

            return _mapper.Map<TModel>(entity);
        }


        protected virtual Task OnBeforeSaveAsync(DbContext context, T entity) => Task.CompletedTask;
        protected virtual Task OnBeforeUpdateAsync(DbContext context, T entity) => Task.CompletedTask;

        public async Task<TModel> UpdateAsync(TModel model)
        {
            var entity = _mapper.Map<T>(model);


            if (entity.Id == 0)
            {
                throw new InvalidOperationException("Id cannot be empty");
            }


            if (entity.TrackingState == TrackingState.Modified && entity is IAuditable auditable)
            {
                auditable.ModifiedDate = DateTime.Now;
                auditable.ModifiedUser = UserName;
            }

            //// Apply changes to context
            _context.ApplyChanges(entity);

            try
            {
                await OnBeforeUpdateAsync(_context, entity);
                // Persist changes
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                _logger.LogError(exception, exception.Message);
                if (!_context.Set<T>().Any(o => o.Id == model.Id))
                    throw new EntityNotFoundException();
                throw;
            }

            _context.AcceptChanges(entity);


            var result = _mapper.Map<TModel>(entity);
            return result;
        }

        public async Task<bool> DeleteAsync(TModel model)
        {
            //T entity = _mapper.Map<T>(model);

            //// Set tracking state to deleted
            //entity.TrackingState = TrackingState.Deleted;

            //// Detach object graph
            //_context.DetachEntities(entity);

            //// Apply changes to context
            //_context.ApplyChanges(entity);

            //// Persist changes
            //return await _context.SaveChangesAsync() > 0;


            var entity = _mapper.Map<T>(model);
            _context.Entry(entity).State = EntityState.Deleted;

            try
            {
                await _context.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (DbUpdateException updateException)
            {
                if (updateException.GetBaseException() is SqlException exception && exception.Number == 547)
                {
                    var errorMessage = $"{typeof(T).Name.Humanize()} is in use. Cannot delete!";
                    throw new EntityAlreadyInUseException(errorMessage);
                }

                throw;
            }
        }


    }



    public class ConcurrencyErrorInfo
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public string User { get; set; }
        public string ModifiedDate { get; set; }
    }


   
}