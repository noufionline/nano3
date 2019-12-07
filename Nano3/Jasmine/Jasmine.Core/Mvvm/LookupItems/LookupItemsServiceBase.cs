using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Jasmine.Core.Common;
using Jasmine.Core.Contracts;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Jasmine.Core.Mvvm.LookupItems
{
    public abstract class LookupItemsServiceBase<TViewModel,TModel>
        where TViewModel:class 
        where TModel:class ,IEntity
    {
        public virtual Task UpdateNavigationParameters(NavigationParameters parameters)
        {
            return Task.CompletedTask;
        }

        public virtual Task<LookupItemDictionary> GetLookupItemsDictionaryAsync(TModel entity) =>
            Task.FromResult(new LookupItemDictionary());


        public virtual void FillLookupItems(TViewModel viewModel, TModel entity){}
        public virtual void FillLookupItems(TViewModel viewModel,params string[] properties){}
        public virtual void FillLookupItems(TViewModel viewModel,IDialogParameters parameters){}
        public TLookup GetLookupItems<TLookup>(LookupItemDictionary dictionary,string key)
        {
            
            if (dictionary.ContainsKey(key))
            {
                object value = dictionary[key];

                if (value.GetType() == typeof(TLookup))
                    return (TLookup)value;
                if (typeof(TLookup).GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
                    return (TLookup)value;
                return (TLookup)Convert.ChangeType(value, typeof(TLookup));
            }

            return default;
        }


        public List<LookupItem> GetLookupItems(LookupItemDictionary dictionary, string key)
        {
            return GetLookupItems<List<LookupItem>>(dictionary,key);
        }

     
    }
}