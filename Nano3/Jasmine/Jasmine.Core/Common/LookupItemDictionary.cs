using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace Jasmine.Core.Common
{
    public class LookupItemDictionary:Dictionary<string,object>
    {

        public static LookupItemDictionary Create(Dictionary<string, List<LookupItem>> items)
        {
            var dictionary=new LookupItemDictionary();
            foreach (var item in items)
            {
                dictionary.Add(item.Key, item.Value);
            }

            return dictionary;
        }
        public void Add<T>(string key,List<LookupItem> lookupItem)
        {
            var name = lookupItem.SingleOrDefault()?.Name;
            if (name!=null)
            {
                Add(key,new List<T>
                {
                    JsonConvert.DeserializeObject<T>(name)
                });
            }
        }

        public ObservableCollection<TLookup> GetObservableCollection<TLookup>(string key)
        {
            TLookup item= GetLookupItems<TLookup>(key);
            return new ObservableCollection<TLookup>(new []{item});
        }

        public ObservableCollection<LookupItem> GetObservableCollection(string key)
        {
            var item= GetLookupItems(key);
            return new ObservableCollection<LookupItem>(item);
        }
        public TLookup GetLookupItems<TLookup>(string key)
        {
            if (ContainsKey(key))
            {
                var name = GetLookupItems(key)?.SingleOrDefault()?.Name;
                object result =JsonConvert.DeserializeObject<TLookup>(name);
                if (result.GetType() == typeof(TLookup))
                    return (TLookup) result;
              
                if (typeof(TLookup).GetTypeInfo().IsAssignableFrom(result.GetType().GetTypeInfo()))
                    return (TLookup) result;

                if (result is object[] objects && !objects.Any())
                {
                    return default;
                }

                return (TLookup) Convert.ChangeType(result, typeof(TLookup));

            }

            throw new KeyNotFoundException($"key {key} not found in LookupItem Dictionary");
        }

        public List<LookupItem> GetLookupItems(string key)
        {
            if (ContainsKey(key))
            {
                object result = this[key];
                if (result.GetType() == typeof(List<LookupItem>))
                    return (List<LookupItem>) result;
              
                if (typeof(List<LookupItem>).GetTypeInfo().IsAssignableFrom(result.GetType().GetTypeInfo()))
                    return (List<LookupItem>) result;

                if (result is object[] objects && !objects.Any())
                {
                    return default;
                }

                return (List<LookupItem>) Convert.ChangeType(result, typeof(List<LookupItem>));

            }

            throw new KeyNotFoundException($"key {key} not found in LookupItem Dictionary");
        }
        public bool TryGetLookupItems<TLookup>(string key, out TLookup value)
        {
            if (ContainsKey(key))
            {
                object result =JsonConvert.DeserializeObject<TLookup>(GetLookupItems(key)?.SingleOrDefault()?.Name);
                if (result.GetType() == typeof(TLookup))
                    value = (TLookup) result;
                else if (typeof(TLookup).GetTypeInfo().IsAssignableFrom(result.GetType().GetTypeInfo()))
                    value = (TLookup) result;
                else if (result is object[] objects && !objects.Any())
                {
                    value = default;
                    return false;
                }
                else
                {
                    value = (TLookup) Convert.ChangeType(result, typeof(TLookup));
                }

                return true;

            }
            value = default;
            return false;
        }

        public bool HasNoKey(string key) => !ContainsKey(key);
    }
}