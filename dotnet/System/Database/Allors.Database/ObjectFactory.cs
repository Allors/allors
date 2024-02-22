// <copyright file="ObjectFactory.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectBase type.</summary>

namespace Allors.Database;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Allors.Database.Meta;

/// <summary>
///     A base implementation for a static <see cref="IObjectFactory" />.
/// </summary>
public class ObjectFactory : IObjectFactory
{
    /// <summary>
    ///     <see cref="ConstructorInfo" /> by <see cref="ObjectType" /> cache.
    /// </summary>
    private readonly Dictionary<ObjectType, ConstructorInfo> contructorInfoByObjectType;

    /// <summary>
    ///     <see cref="ObjectType" /> by <see cref="ObjectType" /> id cache.
    /// </summary>
    private readonly Dictionary<Guid, ObjectType> objectTypeByObjectTypeId;

    /// <summary>
    ///     <see cref="Type" /> by <see cref="ObjectType" /> id cache.
    /// </summary>
    private readonly Dictionary<Type, ObjectType> objectTypeByType;

    /// <summary>
    ///     <see cref="Type" /> by <see cref="ObjectType" /> cache.
    /// </summary>
    private readonly Dictionary<ObjectType, Type> typeByObjectType;

    /// <summary>
    ///     Initializes a new state of the <see cref="ObjectFactory" /> class.
    /// </summary>
    /// <param name="metaPopulation">
    ///     The meta population.
    /// </param>
    /// <param name="assembly">
    ///     The assembly.
    /// </param>
    /// <param name="namespace">
    ///     The namespace.
    /// </param>
    public ObjectFactory(MetaPopulation metaPopulation, Type instance)
    {
        this.Assembly = instance.GetTypeInfo().Assembly;

        var types = this.Assembly.GetTypes()
            .Where(type => type.Namespace != null &&
                           type.Namespace.Equals(instance.Namespace) &&
                           type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(IDataObject)))
            .ToArray();

        this.MetaPopulation = metaPopulation;
        this.Namespace = instance.Namespace;

        var validationLog = metaPopulation.Validate();
        if (validationLog.ContainsErrors)
        {
            throw new Exception(validationLog.ToString());
        }

        metaPopulation.Bind(types);

        this.typeByObjectType = new Dictionary<ObjectType, Type>();
        this.objectTypeByType = new Dictionary<Type, ObjectType>();
        this.objectTypeByObjectTypeId = new Dictionary<Guid, ObjectType>();
        this.contructorInfoByObjectType = new Dictionary<ObjectType, ConstructorInfo>();

        var typeByName = types.ToDictionary(type => type.Name, type => type);

        foreach (var objectType in metaPopulation.Composites)
        {
            var type = typeByName[objectType.SingularName];

            this.typeByObjectType[objectType] = type;
            this.objectTypeByType[type] = objectType;
            this.objectTypeByObjectTypeId[objectType.Id] = objectType;

            if (objectType is Class)
            {
                var parameterTypes = new[] { typeof(IStrategy) };
                var constructor = type.GetTypeInfo().GetConstructor(parameterTypes);
                this.contructorInfoByObjectType[objectType] =
                    constructor ?? throw new ArgumentException(objectType.SingularName + " has no Allors constructor.");
            }
        }
    }

    /// <summary>
    ///     Gets the namespace.
    /// </summary>
    public string Namespace { get; }

    /// <summary>
    ///     Gets the assembly.
    /// </summary>
    public Assembly Assembly { get; }

    /// <summary>
    ///     Gets the domain.
    /// </summary>
    public MetaPopulation MetaPopulation { get; }

    /// <summary>
    ///     Creates a new <see cref="IObject" /> given the <see cref="IStrategy" />.
    /// </summary>
    /// <param name="strategy">The <see cref="IStrategy" /> for the new <see cref="IObject" />.</param>
    /// <returns>The new <see cref="IObject" />.</returns>
    public IObject Create(IStrategy strategy)
    {
        var objectType = strategy.Class;
        var constructor = this.contructorInfoByObjectType[objectType];
        object[] parameters = [strategy];

        return (IObject)constructor.Invoke(parameters);
    }

    public ObjectType GetObjectType<T>() => this.GetObjectType(typeof(T));

    /// <summary>
    ///     Gets the .Net <see cref="Type" /> given the Allors <see cref="ObjectType" />.
    /// </summary>
    /// <param name="type">The .Net <see cref="Type" />.</param>
    /// <returns>The Allors <see cref="ObjectType" />.</returns>
    public ObjectType GetObjectType(Type type) => !this.objectTypeByType.TryGetValue(type, out var objectType) ? null : objectType;

    /// <summary>
    ///     Gets the .Net <see cref="Type" /> given the Allors <see cref="ObjectType" />.
    /// </summary>
    /// <param name="objectType">The Allors <see cref="ObjectType" />.</param>
    /// <returns>The .Net <see cref="Type" />.</returns>
    public Type GetType(ObjectType objectType)
    {
        this.typeByObjectType.TryGetValue(objectType, out var type);
        return type;
    }

    /// <summary>
    ///     Gets the .Net <see cref="Type" /> given the Allors <see cref="ObjectType" />.
    /// </summary>
    /// <param name="objectTypeId">The Allors <see cref="ObjectType" /> id.</param>
    /// <returns>The .Net <see cref="Type" />.</returns>
    public ObjectType GetObjectType(Guid objectTypeId)
    {
        this.objectTypeByObjectTypeId.TryGetValue(objectTypeId, out var objectType);
        return objectType;
    }
}
