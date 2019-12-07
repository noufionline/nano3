using System;
using System.Collections.Generic;
using System.Reflection;

namespace Jasmine.Core.Odoo
{
    public static class OdooExtentions
    {
        public static string[] GetOdooFields(this Type entity)
        {
            PropertyInfo[] props = entity.GetProperties();
            var list = new List<string>();
            foreach (PropertyInfo prop in props)
            {
                object[] attrs = prop.GetCustomAttributes(true);
                foreach (object attr in attrs)
                {
                    if (attr is OdooFieldNameAttribute authAttr)
                    {
                        string auth = authAttr.Name;
                        list.Add(auth);

                    }
                }
            }

            return list.ToArray();
        }
    }
}