// <copyright file="StaticObjectFactory.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectBase type.</summary>

namespace Allors.Workspace.Adapters
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
        
        private readonly Dictionary<IObjectType, ConstructorInfo> constructorInfoByObjectTypeForCompositeRole;

        private readonly Dictionary<IObjectType, ConstructorInfo> constructorInfoByObjectTypeForCompositesRole;

        private readonly Dictionary<IObjectType, ConstructorInfo> constructorInfoByObjectTypeForCompositeAssociation;

        private readonly Dictionary<IObjectType, ConstructorInfo> constructorInfoByObjectTypeForCompositesAssociation;

        private readonly Dictionary<IMethodType, ConstructorInfo> constructorInfoByMethodTypeForMethod;

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
            }

            // For CompositeRole
            {
                var typesForCompositeRole = assembly.GetTypes()
                    .Where(type => type.Namespace?.Equals(instance.Namespace) == true &&
                                   type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ICompositeRole)))
                    .ToArray();

                this.constructorInfoByObjectTypeForCompositeRole = new Dictionary<IObjectType, ConstructorInfo>();

                var typeByName = typesForCompositeRole.ToDictionary(type => type.Name, type => type);

                foreach (var objectType in metaPopulation.Composites)
                {
                    var type = typeByName[objectType.SingularName + "Role"];
                    var parameterTypes = new[] { typeof(IStrategy), typeof(IRoleType) };
                    this.constructorInfoByObjectTypeForCompositeRole[objectType] =
                        type.GetTypeInfo().GetConstructor(parameterTypes);
                }
            }

            // For CompositesRole
            {
                var typesForCompositesRole = assembly.GetTypes()
                    .Where(type => type.Namespace?.Equals(instance.Namespace) == true &&
                                   type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ICompositesRole)))
                    .ToArray();

                this.constructorInfoByObjectTypeForCompositesRole = new Dictionary<IObjectType, ConstructorInfo>();

                var typeByName = typesForCompositesRole.ToDictionary(type => type.Name, type => type);

                foreach (var objectType in metaPopulation.Composites)
                {
                    var type = typeByName[objectType.PluralName + "Role"];
                    var parameterTypes = new[] { typeof(IStrategy), typeof(IRoleType) };
                    this.constructorInfoByObjectTypeForCompositesRole[objectType] =
                        type.GetTypeInfo().GetConstructor(parameterTypes);
                }
            }

            // For CompositeAssociation
            {
                var typesForCompositeAssociation = assembly.GetTypes()
                    .Where(type => type.Namespace?.Equals(instance.Namespace) == true &&
                                   type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ICompositeAssociation)))
                    .ToArray();

                this.constructorInfoByObjectTypeForCompositeAssociation = new Dictionary<IObjectType, ConstructorInfo>();

                var typeByName = typesForCompositeAssociation.ToDictionary(type => type.Name, type => type);

                foreach (var objectType in metaPopulation.Composites)
                {
                    var type = typeByName[objectType.SingularName + "Association"];
                    var parameterTypes = new[] { typeof(IStrategy), typeof(IAssociationType) };
                    this.constructorInfoByObjectTypeForCompositeAssociation[objectType] =
                        type.GetTypeInfo().GetConstructor(parameterTypes);
                }
            }

            // For CompositesAssociation
            {
                var typesForCompositesAssociation = assembly.GetTypes()
                    .Where(type => type.Namespace?.Equals(instance.Namespace) == true &&
                                   type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(ICompositesAssociation)))
                    .ToArray();

                this.constructorInfoByObjectTypeForCompositesAssociation = new Dictionary<IObjectType, ConstructorInfo>();

                var typeByName = typesForCompositesAssociation.ToDictionary(type => type.Name, type => type);

                foreach (var objectType in metaPopulation.Composites)
                {
                    var type = typeByName[objectType.PluralName + "Association"];
                    var parameterTypes = new[] { typeof(IStrategy), typeof(IAssociationType) };
                    this.constructorInfoByObjectTypeForCompositesAssociation[objectType] =
                        type.GetTypeInfo().GetConstructor(parameterTypes);
                }
            }
            
            // For Method
            {
                var typesForMethod = assembly.GetTypes()
                    .Where(type => type.Namespace?.Equals(instance.Namespace) == true &&
                                   type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IMethod)))
                    .ToArray();

                this.constructorInfoByMethodTypeForMethod = new Dictionary<IMethodType, ConstructorInfo>();

                var typeByName = typesForMethod.ToDictionary(type => type.Name, type => type);

                foreach (var methodType in metaPopulation.MethodTypes)
                {
                    var type = typeByName[methodType.ObjectType.SingularName +methodType.Name + "Method"];
                    var parameterTypes = new[] { typeof(IStrategy), typeof(IMethodType) };
                    this.constructorInfoByMethodTypeForMethod[methodType] =
                        type.GetTypeInfo().GetConstructor(parameterTypes);
                }
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
            object[] parameters = { @object };

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

        public T CompositeRole<T>(IStrategy strategy, IRoleType roleType) where T : class, ICompositeRole
        {
            var constructor = this.constructorInfoByObjectTypeForCompositeRole[roleType.ObjectType];
            object[] parameters = { strategy, roleType };

            return (T)constructor.Invoke(parameters);
        }

        public T CompositesRole<T>(IStrategy strategy, IRoleType roleType) where T : class, ICompositesRole
        {
            var constructor = this.constructorInfoByObjectTypeForCompositesRole[roleType.ObjectType];
            object[] parameters = { strategy, roleType };

            return (T)constructor.Invoke(parameters);
        }

        public T CompositeAssociation<T>(IStrategy strategy, IAssociationType associationType) where T : class, ICompositeAssociation
        {
            var constructor = this.constructorInfoByObjectTypeForCompositeAssociation[associationType.ObjectType];
            object[] parameters = { strategy, associationType };

            return (T)constructor.Invoke(parameters);
        }
        
        public T CompositesAssociation<T>(IStrategy strategy, IAssociationType associationType) where T : class, ICompositesAssociation
        {
            var constructor = this.constructorInfoByObjectTypeForCompositesAssociation[associationType.ObjectType];
            object[] parameters = { strategy, associationType };

            return (T)constructor.Invoke(parameters);
        }

        public T Method<T>(IStrategy strategy, IMethodType methodType) where T : class, IMethod
        {
            var constructor = this.constructorInfoByMethodTypeForMethod[methodType];
            object[] parameters = { strategy, methodType };

            return (T)constructor.Invoke(parameters);
        }
    }
}
