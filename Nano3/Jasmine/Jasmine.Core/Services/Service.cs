using DevExpress.Export.Xl;
using Jasmine.Core.Contracts;
using Jasmine.Core.Mvvm;
using Jasmine.Core.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Jasmine.Core.Services
{
    //public abstract class Service<TModel> : IService<TModel> where TModel : class, IEntity
    //{
    //    private readonly IRepository<TModel> _repository;

    //    protected Service(IRepository<TModel> repository) => _repository = repository;


    //    public TModel Save(TModel entity) => _repository.Save(entity);
    //    public TModel Update(TModel entity) => _repository.Update(entity);

    //    public bool Delete(TModel entity) => _repository.Delete(entity);

    //    public TModel Get(int id) => _repository.Get(id);

    //    public Task<List<TModel>> GetAllAsync() => _repository.GetAllAsync();
    //}


    public abstract class ServiceAsync<TModel> : IServiceAsync<TModel> where TModel : class, IEntity,ISupportPatchUpdate
    {
        private readonly IRepositoryAsync<TModel> _repository;

        protected ServiceAsync(IRepositoryAsync<TModel> repository) => _repository = repository;


        public Task<TModel> SaveAsync(TModel entity) => _repository.SaveAsync(entity);
        public Task<TModel> UpdateAsync(TModel entity)
        {
            var patchDoc = entity.CreatePatchDocument();
            return _repository.UpdateAsync(entity.Id,patchDoc);
        }

        //public Task<bool> DeleteAsync(TModel entity) => _repository.DeleteAsync(entity);
        public Task<(bool success, string errorMessage)> DeleteAsync(TModel entity) => _repository.DeleteAsync(entity);
        public Task<(bool success, string errorMessage)> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task<TModel> GetAsync(int id) => _repository.GetAsync(id);


        public Task<List<TModel>> GetAllAsync() => _repository.GetAllAsync();


    }



    public abstract class ParentChildServiceAsync<T> : IParentChildServiceAsync<T> where T : class, IEntity
    {
        private readonly IParentChildRepositoryBaseAsync<T> _repository;

        protected ParentChildServiceAsync(IParentChildRepositoryBaseAsync<T> repository)
        {
            _repository = repository;
        }
        public Task<T> Save(int parentId, T entity) => _repository.SaveAsync(parentId, entity);
        public Task<T> Update(T entity, int parentId) => _repository.UpdateAsync(parentId, entity);

        public Task<(bool success, string errorMessage)> Delete(T entity, int parentId) => _repository.DeleteAsync(parentId, entity);

        public Task<T> Get(int id, int parentId) => _repository.GetAsync(parentId, id);


        public string GetBaseAddress()
        {
            return _repository.GetBaseAddress();
        }

        public Task<bool> ExistsAsync(string uri)
        {
            return _repository.ExistsAsync(uri);
        }

        public Task<T> SaveAsync(int parentId, T entity)
        {
            return _repository.SaveAsync(parentId, entity);
        }

        public Task<T> UpdateAsync(int parentId, T entity)
        {
            return _repository.UpdateAsync(parentId, entity);
        }

        public Task<(bool success, string errorMessage)> DeleteAsync(int parentId, T entity)
        {
            return _repository.DeleteAsync(parentId, entity);

        }

        public Task<(bool success, string errorMessage)> DeleteAsync(int parentId, int id)
        {
            return _repository.DeleteAsync(parentId, id);
        }

        public Task<T> GetAsync(int parentId, int id)
        {
            return _repository.GetAsync(parentId, id);
        }

        public Task<List<T>> GetAllAsync() => _repository.GetAllAsync();

    }



    public abstract class ServiceAsync<TModel, TList> : IServiceAsync<TModel, TList> where TModel : class, IEntity,ISupportPatchUpdate where TList : class, IEntity
    {
        private readonly IRepositoryBaseAsync<TModel, TList> _repository;

        protected ServiceAsync(IRepositoryBaseAsync<TModel, TList> repository) => _repository = repository;

        protected string GetExportPath(string fileName)
        {
            string fileToWriteTo = Path.GetRandomFileName();

            string tempDirectory = Path.GetTempPath();

            string directory = Path.Combine(tempDirectory, fileToWriteTo);

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (fileName.IndexOf(".Xlsx", StringComparison.Ordinal) == -1)
            {
                fileName = $"{fileName}.Xlsx";
            }

            string path = Path.Combine(directory, fileName);
            return path;
        }

        protected void CreateHeading(IXlSheet sheet, XlCellFormatting formatting, string[] headings)
        {
            var verticalCenter = new XlCellFormatting { Alignment = new XlCellAlignment { VerticalAlignment = XlVerticalAlignment.Center } };
            using (IXlRow row = sheet.CreateRow())
            {
                row.HeightInPoints = 18;

                foreach (var heading in headings)
                {
                    using (IXlCell cell = row.CreateCell())
                    {
                        cell.Value = heading;
                        cell.ApplyFormatting(formatting);
                        cell.ApplyFormatting(verticalCenter);
                    }
                }

            }
        }

        public Task<TModel> SaveAsync(TModel entity) => _repository.SaveAsync(entity);
        public Task<TModel> UpdateAsync(TModel entity)
        {
            var patchDoc = entity.CreatePatchDocument();
            return _repository.UpdateAsync(entity.Id,patchDoc);
        }

        public Task<(bool success, string errorMessage)> DeleteAsync(TModel entity) => _repository.DeleteAsync(entity);
        public Task<(bool success, string errorMessage)> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public Task<TModel> GetAsync(int id) => _repository.GetAsync(id);
        public Task<Dictionary<string, List<LookupItem>>> GetLookupItems(int id)
        {
            return _repository.GetLookupItemsAsync(id);
        }

        public Task<List<TList>> GetAllAsync() => _repository.GetAllAsync();


    }
}