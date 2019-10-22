using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Jasmine.Abs.Entities;
using TrackableEntities.Common.Core;

namespace Jasmine.Abs.Api.Helper
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreForeignKeyProperties<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            var properties = typeof(TDest).GetProperties().Where(x => x.CustomAttributes.Any(data => data.AttributeType == typeof(ForeignKeyAttribute))).ToList();

            foreach (var property in properties)
            {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }

            return expression;
        }

        private static  IEnumerable<PropertyInfo> GetCollectionPropertyName<T>()
        {
            foreach (var property in typeof(T).GetProperties())
            {
                if (typeof(IEnumerable<ITrackable>).IsAssignableFrom(property.PropertyType))
                    yield return property;
            }
        }
        public static IMappingExpression<TSource, TDest> IgnoreCollectionProperties<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            var properties = GetCollectionPropertyName<TDest>();
            
            foreach (var property in properties)
            {
                expression.ForMember(property.Name, opt => opt.Ignore());
            }
            return expression;
        }
        public static IMappingExpression<TSource, TDest> IgnoreInverseProperties<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            var properties = typeof(TDest).GetProperties()?.Where(x => x.CustomAttributes.Any(data => data.AttributeType == typeof(InversePropertyAttribute))).ToList();

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }

            return expression;
        }

        public static IMappingExpression<TSource, TDest> IgnoreAuditableProperties<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            var properties = typeof(TDest).GetInterface("IAuditable")?.GetProperties();

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    expression.ForMember(property.Name, opt => opt.Ignore());
                }
            }

            return expression;
        }
    }
}