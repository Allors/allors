// <copyright file="StaticObjectFactory.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectBase type.</summary>

namespace Allors.Workspace.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Meta;

    public class ReflectionObjectFactory : IObjectFactory
    {
        /// <summary>
        /// <see cref="Type"/> by <see cref="IObjectType"/> cache.
        /// </summary>
        private readonly Dictionary<IObjectType, Type> typeByObjectTypeForObject;

        /// <summary>
        /// <see cref="Type"/> by <see cref="IObjectType"/> id cache.
        /// </summary>
        private readonly Dictionary<Type, IObjectType> objectTypeByTypeForObject;

        private readonly Dictionary<string, IObjectType> objectTypeByNameForObject;

        /// <summary>
        /// <see cref="ConstructorInfo"/> by <see cref="IObjectType"/> cache.
        /// </summary>
        private readonly Dictionary<IObjectType, ConstructorInfo> constructorInfoByObjectTypeForObject;

        private readonly Dictionary<IRoleType, Type> constructedTypeByCompositeRoleType;

        private readonly Dictionary<IRoleType, Type> constructedTypeByCompositesRoleType;

        private readonly Dictionary<IAssociationType, Type> constructedTypeByCompositeAssociationType;

        private readonly Dictionary<IAssociationType, Type> constructedTypeByCompositesAssociationType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReflectionObjectFactory"/> class.
        /// </summary>
        /// <param name="metaPopulation">
        /// The meta databaseOrigin.
        /// </param>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentException"></exception>
        public ReflectionObjectFactory(IMetaPopulation metaPopulation, Type instance)
        {
            var assembly = instance.GetTypeInfo().Assembly;

            // For Object
            {
                var typesForObject = assembly.GetTypes()
                    .Where(type => type.Namespace?.Equals(instance.Namespace) == true &&
                                   type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IObject)))
                    .ToArray();

                metaPopulation.Bind(typesForObject);

                this.typeByObjectTypeForObject = new Dictionary<IObjectType, Type>();
                this.objectTypeByTypeForObject = new Dictionary<Type, IObjectType>();
                this.objectTypeByNameForObject = new Dictionary<string, IObjectType>();
                this.constructorInfoByObjectTypeForObject = new Dictionary<IObjectType, ConstructorInfo>();

                var typeByName = typesForObject.ToDictionary(type => type.Name, type => type);

                foreach (var objectType in metaPopulation.Composites)
                {
                    var type = typeByName[objectType.SingularName];

                    this.typeByObjectTypeForObject[objectType] = type;
                    this.objectTypeByTypeForObject[type] = objectType;
                    this.objectTypeByNameForObject[type.Name] = objectType;

                    if (objectType is IClass)
                    {
                        var parameterTypes = new[] { typeof(IStrategy) };
                        this.constructorInfoByObjectTypeForObject[objectType] = type.GetTypeInfo().GetConstructor(parameterTypes)
                                                                      ?? throw new ArgumentException($"{objectType.SingularName} has no Allors constructor.");
                    }
                }

                this.constructedTypeByCompositeRoleType = new Dictionary<IRoleType, Type>();
                this.constructedTypeByCompositesRoleType = new Dictionary<IRoleType, Type>();
                this.constructedTypeByCompositeAssociationType = new Dictionary<IAssociationType, Type>();
                this.constructedTypeByCompositesAssociationType = new Dictionary<IAssociationType, Type>();
            }
        }

        /// <summary>
        /// Gets the .Net <see cref="Type"/> given the Allors <see cref="IObjectType"/>.
        /// </summary>
        /// <param name="type">The .Net <see cref="Type"/>.</param>
        /// <returns>The Allors <see cref="IObjectType"/>.</returns>
        public IObjectType GetObjectTypeForObject(Type type) => !this.objectTypeByTypeForObject.TryGetValue(type, out var objectType) ? null : objectType;

        public IObjectType GetObjectTypeForObject(string name) => !this.objectTypeByNameForObject.TryGetValue(name, out var objectType) ? null : objectType;

        /// <summary>
        /// Gets the .Net <see cref="Type"/> given the Allors <see cref="IObjectType"/>.
        /// </summary>
        /// <param name="objectType">The Allors <see cref="IObjectType"/>.</param>
        /// <returns>The .Net <see cref="Type"/>.</returns>
        public Type GetTypeForObject(IObjectType objectType)
        {
            this.typeByObjectTypeForObject.TryGetValue(objectType, out var type);
            return type;
        }

        public IObjectType GetObjectTypeForObject<T>()
        {
            var typeName = typeof(T).Name;
            return this.GetObjectTypeForObject(typeName);
        }

        /// <summary>
        /// Creates a new <see cref="Object"/> given the <see cref="Object"/>.
        /// </summary>
        /// <param name="object">
        /// The strategy.
        /// </param>
        /// <returns>
        /// The new <see cref="Object"/>.
        /// </returns>
        public T Object<T>(IStrategy @object) where T : class, IObject
        {
            if (@object == null)
            {
                return null;
            }

            var constructor = this.constructorInfoByObjectTypeForObject[@object.Class];
            object[] parameters = [@object];

            return (T)constructor.Invoke(parameters);
        }

        public IEnumerable<T> Object<T>(IEnumerable<IStrategy> objects) where T : class, IObject
        {
            if (objects == null)
            {
                yield break;
            }

            foreach (var @object in objects)
            {
                yield return this.Object<T>(@object);
            }
        }

        public ICompositeRole CompositeRole(IStrategy strategy, IRoleType roleType)
        {
            if (!this.constructedTypeByCompositeRoleType.TryGetValue(roleType, out var constructedType))
            {
                Type type = typeof(CompositeRole<>);
                Type[] typeArgs = [roleType.ObjectType.ClrType];
                constructedType = type.MakeGenericType(typeArgs);
                this.constructedTypeByCompositeRoleType.Add(roleType, constructedType);
            }

            return (ICompositeRole)Activator.CreateInstance(constructedType, [strategy, roleType]);
        }

        public ICompositesRole CompositesRole(IStrategy strategy, IRoleType roleType)
        {
            if (!this.constructedTypeByCompositesRoleType.TryGetValue(roleType, out var constructedType))
            {
                Type type = typeof(CompositesRole<>);
                Type[] typeArgs = [roleType.ObjectType.ClrType];
                constructedType = type.MakeGenericType(typeArgs);
                this.constructedTypeByCompositesRoleType.Add(roleType, constructedType);
            }

            return (ICompositesRole)Activator.CreateInstance(constructedType, [strategy, roleType]);
        }

        public ICompositeAssociation CompositeAssociation(IStrategy strategy, IAssociationType associationType)
        {
            if (!this.constructedTypeByCompositeAssociationType.TryGetValue(associationType, out var constructedType))
            {
                Type type = typeof(CompositeAssociation<>);
                Type[] typeArgs = [associationType.ObjectType.ClrType];
                constructedType = type.MakeGenericType(typeArgs);
                this.constructedTypeByCompositeAssociationType.Add(associationType, constructedType);
            }

            return (ICompositeAssociation)Activator.CreateInstance(constructedType, [strategy, associationType]);
        }

        public ICompositesAssociation CompositesAssociation(IStrategy strategy, IAssociationType associationType)
        {
            if (!this.constructedTypeByCompositesAssociationType.TryGetValue(associationType, out var constructedType))
            {
                Type type = typeof(CompositesAssociation<>);
                Type[] typeArgs = [associationType.ObjectType.ClrType];
                constructedType = type.MakeGenericType(typeArgs);
                this.constructedTypeByCompositesAssociationType.Add(associationType, constructedType);
            }

            return (ICompositesAssociation)Activator.CreateInstance(constructedType, [strategy, associationType]);
        }
    }
}
