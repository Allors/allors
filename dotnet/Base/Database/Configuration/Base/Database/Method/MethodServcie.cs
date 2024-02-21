// <copyright file="Time.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.CompilerServices;
using Allors.Database.Meta;
using Allors.Database.Services;

namespace Allors.Database.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
public class MethodService : IMethodService
    {
        private readonly ConcurrentDictionary<IClass, ConcurrentDictionary<MethodType, Action<object, object>[]>> actionsByMethodTypeByClass; 

        public MethodService(MetaPopulation metaPopulation, Assembly assembly)
        {
            var extensionMethodsByInterface = (from type in assembly.ExportedTypes
                    where type.GetTypeInfo().IsSealed && !type.GetTypeInfo().IsGenericType && !type.IsNested
                    from method in type.GetTypeInfo().DeclaredMethods
                    let parameterType = method.GetParameters().FirstOrDefault()?.ParameterType
                    where method.IsStatic &&
                          method.IsDefined(typeof(ExtensionAttribute), false) &&
                          parameterType?.IsInterface == true
                    select new KeyValuePair<Type, MethodInfo>(parameterType, method))
                .GroupBy(kvp => kvp.Key, kvp => kvp.Value)
                .ToDictionary(v => v.Key, v => v.ToArray());
            
            this.MethodCompiler = new MethodCompiler(metaPopulation, extensionMethodsByInterface);
            this.actionsByMethodTypeByClass = new ConcurrentDictionary<IClass, ConcurrentDictionary<MethodType, Action<object, object>[]>>();
        }

        public MethodCompiler MethodCompiler { get; set; }

        public Action<object, object>[] Get(IClass @class, MethodType methodType)
        {
            if (!this.actionsByMethodTypeByClass.TryGetValue(@class, out var actionsByMethodType))
            {
                actionsByMethodType = new ConcurrentDictionary<MethodType, Action<object, object>[]>();
                this.actionsByMethodTypeByClass[@class] = actionsByMethodType;
            }

            if (!actionsByMethodType.TryGetValue(methodType, out var actions))
            {
                actions = this.MethodCompiler.Compile(@class, methodType);
                actionsByMethodType[methodType] = actions;
            }

            return actions;
        }
    }
}
