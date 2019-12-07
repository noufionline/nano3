﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Jasmine.Core.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jasmine.Core.Tracking
{
    public static class TrackableExtensions
    {
        /// <summary>
        /// Recursively enable or disable tracking on trackable entities in an object graph.
        /// </summary>
        /// <param name="item">Trackable object</param>
        /// <param name="enableTracking">Enable or disable change-tracking</param>
        /// <param name="visitationHelper">Circular reference checking helper</param>
        /// <param name="oneToManyOnly">True if tracking should be set only for OneToMany relations</param>
        /// <param name="entityChanged">
        /// The parent <see cref="ChangeTrackingCollection{TEntity}"/> EntityChanged event handler
        /// to be added/removed to all entities in the graph.
        /// </param>
        public static void SetTracking(this ITrackable item, bool enableTracking, 
            ObjectVisitationHelper visitationHelper = null, bool oneToManyOnly = false, EventHandler<EntityChangedEventArgs> entityChanged = null)
        {
            // Iterator entity properties
            foreach (var navProp in item.GetNavigationProperties())
            {
                // Skip if 1-M only
                if (!oneToManyOnly)
                {
                    // Set tracking on 1-1 and M-1 properties
                    foreach (var refProp in navProp.AsReferenceProperty())
                    {
                        // Get ref prop change tracker
                        ITrackingCollection refChangeTracker = item.GetRefPropertyChangeTracker(refProp.Property.Name);
                        if (refChangeTracker != null)
                        {
                            // Set tracking on ref prop change tracker
                            refChangeTracker.SetTracking(enableTracking, visitationHelper, oneToManyOnly, entityChanged);
                            refChangeTracker.SetHandler(enableTracking, entityChanged);
                        }
                    } 
                }

                // Set tracking on 1-M and M-M properties (if not 1-M only)
                foreach (var colProp in navProp.AsCollectionProperty<ITrackingCollection>())
                {
                    bool isOneToMany = !IsManyToManyChildCollection(colProp.EntityCollection);
                    if (!oneToManyOnly || isOneToMany)
                    {
                        colProp.EntityCollection.SetTracking(enableTracking, visitationHelper, oneToManyOnly, entityChanged);
                        colProp.EntityCollection.SetHandler(enableTracking, entityChanged);
                    }
                }
            }
        }

        private static void SetHandler(this ITrackingCollection trackableCollection, bool handle,
            EventHandler<EntityChangedEventArgs> entityChanged)
        {
            if (entityChanged == null) return;
            if (handle)
                trackableCollection.EntityChanged += entityChanged;
            else
                trackableCollection.EntityChanged -= entityChanged;
        }

        /// <summary>
        /// Recursively set tracking state on trackable entities in an object graph.
        /// </summary>
        /// <param name="item">Trackable object</param>
        /// <param name="state">Change-tracking state of an entity</param>
        /// <param name="visitationHelper">Circular reference checking helper</param>
        /// <param name="isManyToManyItem">True is an item is treated as part of an M-M collection</param>
        public static void SetState(this ITrackable item, TrackingState state, ObjectVisitationHelper visitationHelper,
            bool isManyToManyItem = false)
        {
            ObjectVisitationHelper.EnsureCreated(ref visitationHelper);

            // Prevent endless recursion
            if (!visitationHelper.TryVisit(item)) return;

            // Recurively set state for unchanged, added or deleted items,
            // but not for M-M child item
            if (!isManyToManyItem && state != TrackingState.Modified)
            {
                // Iterate entity properties
                foreach (var colProp in item.GetNavigationProperties().OfCollectionType<ITrackingCollection>())
                {
                    // Process 1-M and M-M properties
                    // Set state on child entities
                    bool isManyToManyChildCollection = IsManyToManyChildCollection(colProp.EntityCollection);
                    foreach (ITrackable trackableChild in colProp.EntityCollection)
                    {
                        trackableChild.SetState(state, visitationHelper,
                            isManyToManyChildCollection);
                    }
                }
            }

            // Deleted items are treated a bit specially
            if (state == TrackingState.Deleted)
            {
                if (isManyToManyItem)
                {
                    // With M-M properties there is no way to tell if the related entity should be deleted
                    // or simply removed from the relationship, because it is an independent association.
                    // Therefore, deleted children are marked unchanged.
                    if (item.TrackingState != TrackingState.Modified)
                        item.TrackingState = TrackingState.Unchanged;
                    return;
                }
                // When deleting added item, set state to unchanged
                else if (item.TrackingState == TrackingState.Added)
                {
                    item.TrackingState = TrackingState.Unchanged;
                    return;
                }
            }

            item.TrackingState = state;
        }

        /// <summary>
        /// Recursively set tracking state on trackable properties in an object graph.
        /// </summary>
        /// <param name="item">Trackable object</param>
        /// <param name="modified">Properties on an entity that have been modified</param>
        /// <param name="visitationHelper">Circular reference checking helper</param>
        public static void SetModifiedProperties(this ITrackable item,
            ICollection<string> modified, ObjectVisitationHelper visitationHelper = null)
        {
            // Prevent endless recursion
            ObjectVisitationHelper.EnsureCreated(ref visitationHelper);
            if (!visitationHelper.TryVisit(item)) return;

            // Iterate entity properties
            foreach (var colProp in item.GetNavigationProperties().OfCollectionType<ITrackingCollection>())
            {
                // Recursively set modified
                foreach (ITrackable child in colProp.EntityCollection)
                {
                    child.SetModifiedProperties(modified, visitationHelper);
                    child.ModifiedProperties = modified;
                }
            }
        }

        /// <summary>
        /// Recursively remove items marked as deleted.
        /// </summary>
        /// <param name="changeTracker">Change-tracking collection</param>
        /// <param name="visitationHelper">Circular reference checking helper</param>
        public static void RemoveRestoredDeletes(this ITrackingCollection changeTracker, ObjectVisitationHelper visitationHelper = null)
        {
            ObjectVisitationHelper.EnsureCreated(ref visitationHelper);

            // Iterate items in change-tracking collection
            var items = changeTracker as IList;
            if (items == null) return;
            var count = items.Count;

            for (int i = count - 1; i > -1; i--)
            {
                // Get trackable item
                var item = items[i] as ITrackable;
                if (item == null) continue;

                // Prevent endless recursion
                if (visitationHelper.TryVisit(item))
                {
                    // Iterate entity properties
                    foreach (var navProp in item.GetNavigationProperties())
                    {
                        // Process 1-1 and M-1 properties
                        foreach (var refProp in navProp.AsReferenceProperty())
                        {
                            // Get changed ref prop
                            ITrackingCollection refChangeTracker = item.GetRefPropertyChangeTracker(refProp.Property.Name);

                            // Remove deletes on rep prop
                            refChangeTracker?.RemoveRestoredDeletes(visitationHelper);
                        }

                        // Process 1-M and M-M properties
                        foreach (var colProp in navProp.AsCollectionProperty<ITrackingCollection>())
                        {
                            colProp.EntityCollection.RemoveRestoredDeletes(visitationHelper);
                        }
                    }
                }

                // Remove item if marked as deleted
                if (item.TrackingState == TrackingState.Deleted)
                {
                    var isTracking = changeTracker.Tracking;
                    changeTracker.InternalTracking = false;
                    items.RemoveAt(i);
                    changeTracker.InternalTracking = isTracking;
                }
            }
        }

        /// <summary>
        /// Restore items marked as deleted.
        /// </summary>
        /// <param name="changeTracker">Change-tracking collection</param>
        /// <param name="visitationHelper">Circular reference checking helper</param>
        public static void RestoreDeletes(this ITrackingCollection changeTracker, ObjectVisitationHelper visitationHelper = null)
        {
            ObjectVisitationHelper.EnsureCreated(ref visitationHelper);

            // Get cached deletes
            var removedDeletes = changeTracker.CachedDeletes;

            // Restore deleted items
            if (removedDeletes.Count > 0)
            {
                var isTracking = changeTracker.Tracking;
                changeTracker.InternalTracking = false;
                foreach (var delete in removedDeletes)
                {
                    var items = changeTracker as IList;
                    if (items != null && !items.Contains(delete))
                        items.Add(delete);
                }
                changeTracker.InternalTracking = isTracking;
            }

            foreach (var item in changeTracker.Cast<ITrackable>())
            {
                // Prevent endless recursion
                if (!visitationHelper.TryVisit(item)) continue;

                // Iterate entity properties
                foreach (var navProp in item.GetNavigationProperties())
                {
                    // Process 1-1 and M-1 properties
                    foreach (var refProp in navProp.AsReferenceProperty())
                    {
                        // Get changed ref prop
                        ITrackingCollection refChangeTracker = item.GetRefPropertyChangeTracker(refProp.Property.Name);
                        
                        // Restore deletes on rep prop
                        refChangeTracker?.RestoreDeletes(visitationHelper);
                    }

                    // Process 1-M and M-M properties
                    foreach (var colProp in navProp.AsCollectionProperty<ITrackingCollection>())
                    {
                        colProp.EntityCollection.RestoreDeletes(visitationHelper);
                    }
                }
            }
        }

        /// <summary>
        /// Performs a deep copy using Json binary serializer.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="item">Trackable object</param>
        /// <returns>Cloned Trackable object</returns>
        public static T Clone<T>(this T item)
            where T : class, ITrackable
        {
            return CloneObject(item);
        }

        /// <summary>
        /// Performs a deep copy using Json binary serializer.
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <param name="items">Collection of Trackable objects</param>
        /// <returns>Cloned collection of Trackable object</returns>
        public static IEnumerable<T> Clone<T>(this IEnumerable<T> items)
            where T : class, ITrackable
        {
            return CloneObject(new CollectionSerializationHelper<T>() { Result = items }).Result;
        }

        private class CollectionSerializationHelper<T>
        {
            [JsonProperty]
            public IEnumerable<T> Result;
        }

        internal static T CloneObject<T>(T item, IContractResolver contractResolver = null)
            where T : class
        {
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                using (var jsonWriter = new JsonTextWriter(writer))
                {
                    var settings = new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.Objects,
                        ContractResolver = contractResolver ?? new EntityNavigationPropertyResolver(),
                        PreserveReferencesHandling = PreserveReferencesHandling.All
                    };
                    var serWr = JsonSerializer.Create(settings);
                    serWr.Serialize(jsonWriter, item);
                    writer.Flush();

                    stream.Position = 0;
                    var reader = new StreamReader(stream);
                    var jsonReader = new JsonTextReader(reader);
                    settings.ContractResolver = new EntityNavigationPropertyResolver();
                    var serRd = JsonSerializer.Create(settings);
                    var copy = serRd.Deserialize<T>(jsonReader);
                    return copy;
                }
            }
        }

        private class EntityNavigationPropertyResolver : DefaultContractResolver
        {
            protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
            {
                JsonProperty property = base.CreateProperty(member, memberSerialization);
                property.ShouldSerialize =
                    instance =>
                        {
                            var entity = instance as ITrackable;
                            if (entity == null) return true;

                            // The current property is a navigation property and its value is null
                            bool isEmptyNavProp = 
                                (from np in entity.GetNavigationProperties(false)
                                 where np.Property == member
                                 select np.ValueIsNull).Any(isNull => isNull);

                            return !isEmptyNavProp;
                        };
                return property;
            }
        }

        /// <summary>
        /// Get reference property change tracker.
        /// </summary>
        /// <param name="item">ITrackable object</param>
        /// <param name="propertyName">Reference property name</param>
        /// <returns>Reference property change tracker</returns>
        public static ITrackingCollection GetRefPropertyChangeTracker(this ITrackable item, string propertyName)
        {
            var resolver = item as IRefPropertyChangeTrackerResolver;
            if (resolver != null)
                return resolver.GetRefPropertyChangeTracker(propertyName);

            var property = GetChangeTrackingProperty(item.GetType(), propertyName);
            if (property == null) return null;
            return property.GetValue(item, null) as ITrackingCollection;
        }

        /// <summary>
        /// Determine if an entity is a child of a many-to-many change-tracking collection property.
        /// </summary>
        /// <param name="changeTracker">Change-tracking collection</param>
        /// <returns></returns>
        public static bool IsManyToManyChildCollection(ITrackingCollection changeTracker)
        {
            // Entity is a M-M child if change-tracking collection has a non-null Parent property
            bool isManyToManyChild = changeTracker.Parent != null;
            return isManyToManyChild;
        }

        internal static IEnumerable<Type> BaseTypes(this Type type)
        {
            for (Type t = type; t != null; t = PortableReflectionHelper.Instance.GetBaseType(t))
                yield return t;
        }

        private static PropertyInfo GetChangeTrackingProperty(Type entityType, string propertyName)
        {
            var property = entityType.BaseTypes()
                .SelectMany(t => PortableReflectionHelper.Instance.GetPrivateInstanceProperties(t))
                .SingleOrDefault(p => p.Name == propertyName + Constants.ChangeTrackingMembers.ChangeTrackingPropEnd);
            return property;
        }


           public static void AcceptChanges(this ITrackable item)
        {
            // Recursively set tracking state for child collections
            item.AcceptChanges(null);
        }

        /// <summary>
        /// Set tracking state to Unchanged on entities and their child collections.
        /// </summary>
        /// <param name="items">Trackable objects</param>
        public static void AcceptChanges(this IEnumerable<ITrackable> items)
        {
            // Recursively set tracking state for child collections
            foreach (var item in items)
                item.AcceptChanges(null);
        }

        private static void AcceptChanges(this ITrackable item, ObjectVisitationHelper visitationHelper)
        {
            ObjectVisitationHelper.EnsureCreated(ref visitationHelper);

            // Prevent endless recursion
            if (!visitationHelper.TryVisit(item)) return;

            // Set tracking state for child collections
            foreach (var navProp in item.GetNavigationProperties())
            {
                // Apply changes to 1-1 and M-1 properties
                foreach (var refProp in navProp.AsReferenceProperty())
                    refProp.EntityReference.AcceptChanges(visitationHelper);

                // Apply changes to 1-M properties
                foreach (var colProp in navProp.AsCollectionProperty<IList>())
                {
                    var items = colProp.EntityCollection;
                    var count = items.Count;
                    for (int i = count - 1; i > -1; i--)
                    {
                        // Stop recursion if trackable hasn't been visited
                        var trackable = items[i] as ITrackable;
                        if (trackable != null)
                        {
                            if (trackable.TrackingState == TrackingState.Deleted)
                                // Remove items marked as deleted
                                items.RemoveAt(i);
                            else
                                // Recursively accept changes on trackable
                                trackable.AcceptChanges(visitationHelper);
                        }
                    }
                }
            }

            // Set tracking state and clear modified properties
            item.TrackingState = TrackingState.Unchanged;
            item.ModifiedProperties = new HashSet<string>();
        }

        /// <summary>
        /// Get a list of all navigation properties (entity references and entity collections)
        /// of a given entity.
        /// </summary>
        /// <param name="entity">Entity object</param>
        /// <param name="skipNulls">Null properties are skipped</param>
        public static IEnumerable<EntityNavigationProperty>
            GetNavigationProperties(this ITrackable entity, bool skipNulls = true)
        {
            INavigationPropertyInspector inspector = entity as INavigationPropertyInspector;
            if (inspector == null)
                inspector = new DefaultNavigationPropertyInspector(entity);

            foreach (var navProp in inspector.GetNavigationProperties())
            {
                // 1-1 and M-1 properties
                foreach (var refProp in navProp.AsReferenceProperty())
                {
                    if (skipNulls && refProp.EntityReference == null) continue; // skip nulls
                    yield return refProp;
                }

                // 1-M and M-M properties
                foreach (var colProp in navProp.AsCollectionProperty())
                {
                    if (skipNulls && colProp.EntityCollection == null) continue; // skip nulls
                    yield return colProp;
                }
            }
        }

        /// <summary>
        /// Get an entity collection property (1-M or M-M) for the given entity.
        /// </summary>
        /// <typeparam name="TEntityCollection">Type of entity collection</typeparam>
        /// <param name="entity">Entity object</param>
        /// <param name="property">Property information</param>
        public static EntityCollectionProperty<TEntityCollection>
            GetEntityCollectionProperty<TEntityCollection>(this ITrackable entity,
                PropertyInfo property)
            where TEntityCollection : class
        {
            return entity.GetNavigationProperties(false)
                .Where(np => np.Property == property)
                .OfCollectionType<TEntityCollection>()
                .SingleOrDefault();
        }

        /// <summary>
        /// Get an entity collection property (1-M or M-M) for the given entity.
        /// </summary>
        /// <param name="entity">Entity object</param>
        /// <param name="property">Property information</param>
        public static EntityCollectionProperty<IEnumerable<ITrackable>>
            GetEntityCollectionProperty(this ITrackable entity, PropertyInfo property)
        {
            return entity.GetEntityCollectionProperty<IEnumerable<ITrackable>>(property);
        }

        /// <summary>
        /// Get an entity reference property (1-1 or M-1) for the given entity.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="entity">Entity object</param>
        /// <param name="property">Property information</param>
        public static EntityReferenceProperty<TEntity>
            GetEntityReferenceProperty<TEntity>(this ITrackable entity, PropertyInfo property)
            where TEntity : class, ITrackable
        {
            return entity.GetNavigationProperties(false)
                .Where(np => np.Property == property)
                .OfReferenceType<TEntity>()
                .SingleOrDefault();
        }

        /// <summary>
        /// Get an entity reference property (1-1 or M-1) for the given entity.
        /// </summary>
        /// <param name="entity">Entity object</param>
        /// <param name="property">Property information</param>
        public static EntityReferenceProperty<ITrackable>
            GetEntityReferenceProperty(this ITrackable entity, PropertyInfo property)
        {
            return entity.GetEntityReferenceProperty<ITrackable>(property);
        }

        /// <summary>
        /// Pick only properties of type entity reference.
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <param name="navigationProperties">All nagivation properties</param>
        public static IEnumerable<EntityReferenceProperty<TEntity>>
            OfReferenceType<TEntity>(this IEnumerable<EntityNavigationProperty> navigationProperties)
            where TEntity : class, ITrackable
        {
            return navigationProperties.SelectMany(np => np.AsReferenceProperty<TEntity>());
        }

        /// <summary>
        /// Pick only properties of type entity reference.
        /// </summary>
        /// <param name="navigationProperties">All nagivation properties</param>
        public static IEnumerable<EntityReferenceProperty>
            OfReferenceType(this IEnumerable<EntityNavigationProperty> navigationProperties)
        {
            return navigationProperties.OfReferenceType<ITrackable>();
        }

        /// <summary>
        /// Pick only properties of type entity collection.
        /// </summary>
        /// <typeparam name="TEntityCollection">Type of entity collection</typeparam>
        /// <param name="navigationProperties">All nagivation properties</param>
        public static IEnumerable<EntityCollectionProperty<TEntityCollection>>
            OfCollectionType<TEntityCollection>(this IEnumerable<EntityNavigationProperty> navigationProperties)
            where TEntityCollection : class
        {
            return navigationProperties.SelectMany(np => np.AsCollectionProperty<TEntityCollection>());
        }

        /// <summary>
        /// Pick only properties of type entity collection.
        /// </summary>
        /// <param name="navigationProperties">All nagivation properties</param>
        public static IEnumerable<EntityCollectionProperty>
            OfCollectionType(this IEnumerable<EntityNavigationProperty> navigationProperties)
        {
            return navigationProperties.OfCollectionType<IEnumerable<ITrackable>>();
        }

        /// <summary>
        /// Default implementation of INavigationPropertyInspector used if an entity doesn't provide
        /// its own implementation.
        /// DefaultNavigationPropertyInspector simply loops over all entity properties
        /// and yields those, whose values are either ITrackable or IEnumerable&lt;ITrackable&gt;.
        /// </summary>
        private sealed class DefaultNavigationPropertyInspector : INavigationPropertyInspector
        {
            private readonly ITrackable _entity;

            public DefaultNavigationPropertyInspector(ITrackable entity)
            {
                this._entity = entity;
            }

            public IEnumerable<EntityNavigationProperty> GetNavigationProperties()
            {
                foreach (var prop in _entity.GetType().GetProperties())
                {
                    // 1-1 and M-1 properties
                    if (typeof(ITrackable).IsAssignableFrom(prop.PropertyType))
                    {
                        var trackableRef = prop.GetValue(_entity, null) as ITrackable;
                        yield return new EntityReferenceProperty(prop, trackableRef);
                    }

                    // 1-M and M-M properties
                    if (typeof(IEnumerable<ITrackable>).IsAssignableFrom(prop.PropertyType))
                    {
                        var items = prop.GetValue(_entity, null) as IEnumerable<ITrackable>;
                        yield return new EntityCollectionProperty(prop, items);
                    }
                }
            }
        }
    }
}