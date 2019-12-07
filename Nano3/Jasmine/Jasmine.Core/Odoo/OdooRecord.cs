using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CookComputing.XmlRpc;

namespace Jasmine.Core.Odoo
{
    public class OdooRecord
    {
        private readonly OdooApi _api;
        private readonly string _model;
        private readonly Dictionary<string, object> _fields = new Dictionary<string, object>();
        private readonly List<string> _modifiedFields = new List<string>();

        public OdooRecord(OdooApi api, string model, int id)
        {
            _model = model;
            _api = api;
            Id = id;
        }

        public bool SetValue(string field, object value)
        {
            if(_fields.ContainsKey(field))
            {
                if(!_modifiedFields.Contains(field))
                {
                    _modifiedFields.Add(field);
                }

                _fields[field] = value;
            }
            else
            {
                _fields.Add(field, value);
            }
            return true;
        }

        public object GetValue(string field)
        {
            if (_fields.ContainsKey(field))
            {
                return _fields[field];
            }
            else
            {
                return null;
            }
        }


        public TEntity GetEntity<TEntity>() where TEntity:class,new()
        {
            var entity=new TEntity();
            PropertyInfo[] props = typeof(TEntity).GetProperties();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    if (attr is OdooFieldNameAttribute authAttr)
                    {
                        string auth = authAttr.Name;
                        if (_fields.ContainsKey(auth))
                        {
                            object value = _fields[auth];
                            SetValue(prop, entity, value,index:authAttr.Index,type:authAttr.Type);
                        }

                    }
                }
            }

            return entity;
        }

        void SetValue(PropertyInfo info, object instance, object value,int index=0,Type type=null)
        {
            if (value is object[] obj)
            {
                if (type == null)
                {
                    if(((object[]) value).Any())
                    {
                        info.SetValue(instance,
                            index == 0
                                ? Convert.ChangeType(obj[index], typeof(int))
                                : Convert.ChangeType(obj[index], typeof(string)));
                    }
                }
                else
                {
                    info.SetValue(instance, Convert.ChangeType(obj[index], type));
                }
            }
            else
            {
                info.SetValue(instance, Convert.ChangeType(value, info.PropertyType));
            }
        }


        public T GetValue<T>(string field)
        {

            if (_fields.ContainsKey(field))
            {
                object value = _fields[field];

                if (value.GetType() == typeof(T))
                    return (T)value;
                if (typeof(T).GetTypeInfo().IsAssignableFrom(value.GetType().GetTypeInfo()))
                    return (T)value;
                return (T)Convert.ChangeType(value, typeof(T));
            }

            return default(T);


        }


        public bool TryGetValue<T>(string field, out T value)
        {
            if (_fields.ContainsKey(field))
            {
                object result = _fields[field];
                if (result.GetType() == typeof(T))
                    value = (T) result;
                else if (typeof(T).GetTypeInfo().IsAssignableFrom(result.GetType().GetTypeInfo()))
                    value = (T) result;
                else if (result is object[] && !((object[]) result).Any())
                {
                    value = default;
                    return false;
                }
                else
                {
                    value = (T) Convert.ChangeType(result, typeof(T));
                }

                return true;

            }
            else
            {
                value = default;
                return false;
            }




        }

        public int Id { get; private set; }

        public void Save()
        {
            XmlRpcStruct values = new XmlRpcStruct();

            if (Id >= 0)
            {
                foreach (string field in _modifiedFields)
                {
                    values[field] = _fields[field];
                }

                _api.Write(_model, new int[1] { Id }, values);
            }
            else
            {
                foreach (string field in _fields.Keys)
                {
                    values[field] = _fields[field];
                }

                Id = _api.Create(_model, values);
            }
        }
    }

    public class OdooFieldNameAttribute:Attribute{
        public OdooFieldNameAttribute(string name,int index=0,Type type=null)
        {
            Name = name;
            Index = index;
            Type = type;
        }
        public string Name { get; set; }
        public int Index { get; }
        public Type Type { get; }
    }


    public class OdooModelNameAttribute : Attribute
    {
        public string Name { get; }

        public OdooModelNameAttribute(string name)
        {
            Name = name;
        }
    }


}
