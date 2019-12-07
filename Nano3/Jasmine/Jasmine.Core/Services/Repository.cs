using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Jasmine.Core.Contracts;
using Jasmine.Core.Tracking;

namespace Jasmine.Core.Services
{
    //public abstract class Repository<T,TModel,TContext> : IRepository<TModel>
    //    where T:class,IEntity,ITrackable
    //    where TModel:class,IEntity
    //    where TContext:DbContext
    //{
    //    private readonly Func<TContext> _contextFunc;
    //    private readonly IMapper _mapper;

    //    protected Repository(Func<TContext> contextFunc,IMapper mapper)
    //    {
    //        _contextFunc = contextFunc;
    //        _mapper = mapper;
    //    }

      

    //    public bool Delete(TModel entity)
    //    {
    //        using (TContext uow = _contextFunc())
    //        {
    //            T item = _mapper.Map<T>(entity);
    //            uow.Entry(item).State = EntityState.Deleted;
    //            return uow.SaveChanges()>0;
    //        }
    //    }

    //    public bool Delete(int id)
    //    {
    //        using (TContext uow = _contextFunc())
    //        {
    //            var item = uow.Set<T>().Single(x => x.Id == id);             
    //            uow.Entry(item).State = EntityState.Deleted;
    //            return uow.SaveChanges()>0;
    //        }
    //    }

    //    public virtual TModel Get(int id)
    //    {
    //        using (TContext uow = _contextFunc())
    //        {
    //            return uow.Set<T>()
    //                .ProjectTo<TModel>(_mapper.ConfigurationProvider)
    //                .SingleOrDefault(x => x.Id == id);
    //        }
    //    }

    //    public virtual async Task<List<TModel>> GetAllAsync()
    //    {
    //        using (TContext uow = _contextFunc())
    //        {
    //            return await uow.Set<T>()
    //                .ProjectTo<TModel>(_mapper.ConfigurationProvider)
    //                .ToListAsync();
    //        }
    //    }

    //    public TContext Context => _contextFunc();

    //    public virtual TModel Save(TModel entity)
    //    {
    //        using (TContext uow = _contextFunc())
    //        {
    //            T item = _mapper.Map<T>(entity);

    //            uow.ApplyChanges(item);

    //            uow.SaveChanges();

    //            // Populate reference properties
    //            uow.LoadRelatedEntities(item);

    //            // Reset tracking state to unchanged
    //            item.AcceptChanges();

    //            return _mapper.Map<TModel>(item);
    //        }
    //    }


    //    public TModel Update(TModel entity) => Save(entity);


    //}



}