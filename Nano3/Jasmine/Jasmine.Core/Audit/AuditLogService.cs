using Jasmine.Core.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Jasmine.Core.Tracking;

namespace Jasmine.Core.Audit
{


    public class AuditLogService : IAuditLogService
    {
        public virtual Formatting GetFormatting()
        {
            return Formatting.Indented;
        }
        public string WriteLog<T>(T item, params string[] excludedProperties) where T : ITrackable
        {
            var builder = new StringBuilder();
            var stringWriter = new System.IO.StringWriter(builder);

            using (JsonWriter jsonWriter = new JsonTextWriter(stringWriter))
            {
                jsonWriter.Formatting = GetFormatting();

                jsonWriter.WriteStartObject();


                var properties = GetProperties(item.GetType());

                foreach (var property in properties)
                {
                    var i = property.GetValue(item);
                    if (property.PropertyType.IsGenericType)
                    {
                        if (typeof(IEnumerable).IsAssignableFrom(property.PropertyType) && i is IList list)
                        {
                            if (list.Count == 0) continue;
                            jsonWriter.WritePropertyName(property.Name);
                            jsonWriter.WriteStartArray();

                            foreach (var entity in list)
                            {
                                if (entity is ITrackable trackable)
                                {
                                    var ps = GetProperties(entity.GetType());

                                    jsonWriter.WriteStartObject();
                                    foreach (var p in ps)
                                    {
                                        if (trackable.TrackingState == TrackingState.Modified)
                                        {
                                            if (trackable.ModifiedProperties.Contains(p.Name) && !excludedProperties.Contains(p.Name))
                                            {
                                                jsonWriter.WritePropertyName(p.Name);
                                                jsonWriter.WriteValue(p.GetValue(entity));
                                            }
                                        }
                                        else
                                        {
                                            if (p.Name != nameof(trackable.ModifiedProperties) && !excludedProperties.Contains(p.Name))
                                            {
                                                jsonWriter.WritePropertyName(p.Name);
                                                jsonWriter.WriteValue(p.GetValue(entity));
                                            }
                                        }
                                    }

                                    jsonWriter.WritePropertyName("State");
                                    jsonWriter.WriteValue(trackable.TrackingState.ToString());

                                    jsonWriter.WriteEndObject();
                                }
                            }

                            jsonWriter.WriteEndArray();
                        }
                    }
                    else if (typeof(ITrackable).IsAssignableFrom(property.PropertyType))
                    {
                        if (i is ITrackable t && t.ModifiedProperties.Count > 0)
                        {
                            jsonWriter.WritePropertyName(property.Name);
                            jsonWriter.WriteStartObject();
                            foreach (var child in GetProperties(property.PropertyType))
                            {
                                if (child.Name != nameof(t.ModifiedProperties) && !excludedProperties.Contains(child.Name))
                                {
                                    jsonWriter.WritePropertyName(child.Name);
                                    jsonWriter.WriteValue(child.GetValue(t));
                                }
                            }

                            jsonWriter.WriteEndObject();
                        }
                    }
                    else
                    {
                        if (item.ModifiedProperties.Contains(property.Name) && !excludedProperties.Contains(property.Name))
                        {
                            jsonWriter.WritePropertyName(property.Name);
                            jsonWriter.WriteValue(property.GetValue(item));
                        }
                    }
                }
            }

            List<PropertyInfo> GetProperties(Type type) => type.GetProperties().Where(x => !Attribute.IsDefined(x, typeof(IgnoreTrackingAttribute))).ToList();

            return builder.ToString();
        }

        private readonly StringBuilder _entityNameBuilder = new StringBuilder();

        private string ConvertToJson<T>(T item) => ConvertToJson(item,
            new ModifiedPropertiesOnlyContractResolver("RowVersion", "TrackingState", "ModifiedProperties", "EntityIdentifier"));

        private string ConvertToJson<T>(T item, IContractResolver resolver)
        {
            var settings = new JsonSerializerSettings
            {
                Formatting = GetFormatting(),
                ContractResolver = resolver,
                PreserveReferencesHandling = PreserveReferencesHandling.None
            };
            var result = JsonConvert.SerializeObject(item, settings);
            return result;
        }

        public AuditLog CreateAuditLog<T>(T source, T changes, T target) where T : class, IEntity, ITrackable
        {
            if (source == null && target == null)
            {
                throw new InvalidOperationException("Both source and target cannot be null");
            }

            if (source == null)
            {
                var auditLog = new AuditLog
                {
                    PrimaryKey = target.Id,
                    EntityName = target.GetType().Name,
                    After = ConvertToJson(target),
                    TrackingState = TrackingState.Added
                };
                return auditLog;
            }

            if (target == null)
            {
                var auditLog = new AuditLog
                {
                    PrimaryKey = source.Id,
                    EntityName = source.GetType().Name,
                    Before = ConvertToJson(source),
                    TrackingState = TrackingState.Deleted
                };

                return auditLog;
            }

            if (changes == null)
            {
                throw new ArgumentNullException(nameof(changes), @"Changed Entity required");
            }

            return new AuditLog
            {
                PrimaryKey = target.Id,
                EntityName = target.GetType().Name,
                Before = ConvertToJson(source),
                Changes = WriteLog(changes, "RowVersion", "EntityIdentifier", "TrackingState"),
                After = ConvertToJson(target),
                TrackingState = TrackingState.Modified
            };
        }

        public void Compare<T>(T source, T target, List<AuditLogLine> logs)

        {
            if (source == null && target != null)
            {
                if (target is IEntity teb)
                {
                    var properties = GetProperties(teb);
                    foreach (var p in properties)
                    {
                        var v = new AuditLogLine
                        {
                            PrimaryKey = teb.Id,
                            EntityName = target.GetType().Name,
                            PropertyName = p.Name,
                            PropertyType = p.PropertyType.ToString(),
                            After = p.GetValue(teb),
                            TrackingState = TrackingState.Added
                        };
                        logs.Add(v);
                    }
                }
                else
                {
                    var properties = GetProperties(target);
                    foreach (var p in properties)
                    {
                        var v = new AuditLogLine
                        {
                            //PrimaryKey = target.Id,
                            EntityName = target.GetType().Name,
                            PropertyName = p.Name,
                            PropertyType = p.PropertyType.ToString(),
                            After = p.GetValue(target),
                            TrackingState = TrackingState.Added
                        };
                        logs.Add(v);
                    }
                }
            }
            else if (source != null && target == null && source is IEntity seb)
            {
                var properties = GetProperties(seb);
                foreach (var p in properties)
                {
                    var v = new AuditLogLine
                    {
                        PrimaryKey = seb.Id,
                        EntityName = source.GetType().Name,
                        PropertyName = p.Name,
                        PropertyType = p.PropertyType.ToString(),
                        Before = p.GetValue(seb),
                        TrackingState = TrackingState.Deleted
                    };
                    logs.Add(v);
                }
            }
            else if (source != null && target != null)
            {
                var properties = GetProperties(target);

                AppendEntityName(target);
                //PrintEntityName();

                foreach (var property in properties)
                {
                    var x = property.GetValue(source);
                    var y = property.GetValue(target);

                    if (x is IList sourceChildren && y is IList targetChildren)
                    {
                        foreach (var targetChild in targetChildren)
                        {
                            if (targetChild is ITrackable trackable)
                            {
                                if (trackable.TrackingState == TrackingState.Added)
                                {
                                    Compare(null, targetChild, logs);
                                }

                                if (trackable.TrackingState == TrackingState.Deleted)
                                {
                                    Compare(targetChild, null, logs);
                                }
                                else if (trackable.TrackingState == TrackingState.Modified)
                                {
                                    foreach (var sourceChild in sourceChildren)
                                    {
                                        if (targetChild is IEntity tc && sourceChild is IEntity sc &&
                                            tc.Id.Equals(sc.Id))
                                        {
                                            Compare(sc, tc, logs);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (x != null && y != null && x is ITrackable tx && y is ITrackable ty)
                    {
                        if (!CompareHelper.CompareObjects(tx, ty, null))
                            Compare(tx, ty, logs);
                    }
                    else
                    {
                        var difference = new AuditLogLine
                        {
                            EntityName = source.GetType().Name,
                            PrimaryKey = target is IEntity ? ((IEntity)target).Id : 0,
                            PropertyName = property.Name,
                            PropertyType = property.PropertyType.ToString(),
                            Before = x is ITrackable ? ConvertToJson(x) : x,
                            After = y is ITrackable ? ConvertToJson(y) : y,
                            TrackingState = TrackingState.Modified
                        };
                        if (!Equals(x, y))
                        {
                            logs.Add(difference);
                        }
                    }
                }

                ClearEntityName();
            }
        }

        private List<PropertyInfo> GetProperties(object item)
        {
            var excludedProperties = new[] { "TrackingState", "ModifiedProperties", "EntityIdentifier" };
            return item.GetType().GetProperties()
                //.GetProperties(BindingFlags.Public | BindingFlags.SetProperty)
                .Where(x => !Attribute.IsDefined(x, typeof(IgnoreTrackingAttribute)) &&
                            !excludedProperties.Contains(x.Name))
                .OrderBy(x => x.MetadataToken).ToList();
        }


        private void AppendEntityName(object entity) => _entityNameBuilder.Append(entity.GetType().Name).Append(".");

        private void ClearEntityName() => _entityNameBuilder.Clear();

        private string GetEntityName(string prefix = null)
        {
            var name = _entityNameBuilder.ToString();
            if (name.IndexOf(".", StringComparison.Ordinal) > 0)
                return name.Substring(0, name.Length - 1);

            return prefix == null ? name : $"{prefix}.{name}";
        }




    }
    public class StringAuditLogService : AuditLogService
    {
        public override Formatting GetFormatting() => Formatting.None;
    }

    public class CompareHelper
    {
        public static bool CompareObjects(object inputObjectA, object inputObjectB, params string[] ignorePropertiesList)
        {
            bool areObjectsEqual = true;
            //check if both objects are not null before starting comparing children
            if (inputObjectA != null && inputObjectB != null)
            {
                //create variables to store object values
                object value1, value2;

                PropertyInfo[] properties = inputObjectA.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                //get all public properties of the object using reflection   
                foreach (PropertyInfo propertyInfo in properties)
                {
                    //if it is not a readable property or if it is a ignorable property
                    //ingore it and move on
                    if (!propertyInfo.CanRead || (ignorePropertiesList?.Contains(propertyInfo.Name) ?? false))
                        continue;

                    //get the property values of both the objects
                    value1 = propertyInfo.GetValue(inputObjectA, null);
                    value2 = propertyInfo.GetValue(inputObjectB, null);

                    // if the objects are primitive types such as (integer, string etc)
                    //that implement IComparable, we can just directly try and compare the value      
                    if (IsAssignableFrom(propertyInfo.PropertyType) || IsPrimitiveType(propertyInfo.PropertyType) || IsValueType(propertyInfo.PropertyType))
                    {
                        //compare the values
                        if (!CompareValues(value1, value2))
                        {
                            Console.WriteLine("Property Name {0}", propertyInfo.Name);
                            areObjectsEqual = false;
                        }
                    }
                    //if the property is a collection (or something that implements IEnumerable)
                    //we have to iterate through all items and compare values
                    else if (IsEnumerableType(propertyInfo.PropertyType))
                    {
                        Console.WriteLine("Property Name {0}", propertyInfo.Name);
                        CompareEnumerations(value1, value2, ignorePropertiesList);
                    }
                    //if it is a class object, call the same function recursively again
                    else if (propertyInfo.PropertyType.IsClass)
                    {
                        if (!CompareObjects(propertyInfo.GetValue(inputObjectA, null), propertyInfo.GetValue(inputObjectB, null), ignorePropertiesList))
                        {
                            areObjectsEqual = false;
                        }
                    }
                    else
                    {
                        areObjectsEqual = false;
                    }
                }
            }
            else
                areObjectsEqual = false;

            return areObjectsEqual;
        }

        //true if c and the current Type represent the same type, or if the current Type is in the inheritance 
        //hierarchy of c, or if the current Type is an interface that c implements, 
        //or if c is a generic type parameter and the current Type represents one of the constraints of c. false if none of these conditions are true, or if c is null.
        private static bool IsAssignableFrom(Type type)
        {
            return typeof(IComparable).IsAssignableFrom(type);
        }

        private static bool IsPrimitiveType(Type type)
        {
            return type.IsPrimitive;
        }

        private static bool IsValueType(Type type)
        {
            return type.IsValueType;
        }

        private static bool IsEnumerableType(Type type)
        {
            return (typeof(IEnumerable).IsAssignableFrom(type));
        }

        /// <summary>
        /// Compares two values and returns if they are the same.
        /// </summary>        
        private static bool CompareValues(object value1, object value2)
        {
            bool areValuesEqual = true;
            IComparable selfValueComparer = value1 as IComparable;

            // one of the values is null             
            if (value1 == null && value2 != null || value1 != null && value2 == null)
                areValuesEqual = false;
            else if (selfValueComparer != null && selfValueComparer.CompareTo(value2) != 0)
                areValuesEqual = false;
            else if (!object.Equals(value1, value2))
                areValuesEqual = false;

            return areValuesEqual;
        }

        private static bool CompareEnumerations(object value1, object value2, string[] ignorePropertiesList)
        {
            // if one of the values is null, no need to proceed return false;
            if (value1 == null && value2 != null || value1 != null && value2 == null)
                return false;
            else if (value1 != null && value2 != null)
            {
                IEnumerable<object> enumValue1, enumValue2;
                enumValue1 = ((IEnumerable)value1).Cast<object>();
                enumValue2 = ((IEnumerable)value2).Cast<object>();

                // if the items count are different return false
                if (enumValue1.Count() != enumValue2.Count())
                    return false;
                // if the count is same, compare individual item 
                else
                {
                    object enumValue1Item, enumValue2Item;
                    Type enumValue1ItemType;
                    for (int itemIndex = 0; itemIndex < enumValue1.Count(); itemIndex++)
                    {
                        enumValue1Item = enumValue1.ElementAt(itemIndex);
                        enumValue2Item = enumValue2.ElementAt(itemIndex);
                        enumValue1ItemType = enumValue1Item.GetType();
                        if (IsAssignableFrom(enumValue1ItemType) || IsPrimitiveType(enumValue1ItemType) || IsValueType(enumValue1ItemType))
                        {
                            if (!CompareValues(enumValue1Item, enumValue2Item))
                                return false;
                        }
                        else if (!CompareObjects(enumValue1Item, enumValue2Item, ignorePropertiesList))
                            return false;
                    }
                }
            }
            return true;
        }
    }
}