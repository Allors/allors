// <copyright file="LocalPullResult.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters.Direct;

using System;
using System.Collections.Generic;
using System.Linq;
using Allors.Database;
using Allors.Database.Derivations;
using Allors.Database.Meta;
using Allors.Database.Security;
using Allors.Database.Services;
using Allors.Workspace.Request;

public class Invoke : Result
{
    internal Invoke(Workspace workspace) : base(workspace)
    {
        this.Connection = workspace.Connection;
        this.Transaction = this.Connection.Database.CreateTransaction();

        var metaCache = this.Transaction.Database.Services.Get<IMetaCache>();

        this.AccessControl = this.Transaction.Services.Get<IWorkspaceAclsService>().Create(this.Connection.Name);
        this.AllowedClasses = metaCache.GetWorkspaceClasses(this.Connection.Name);
        this.Derive = () => this.Transaction.Database.Services.Get<IDerivationService>().CreateDerivation(this.Transaction).Derive();
    }

    private Connection Connection { get; }

    private ITransaction Transaction { get; }

    private IReadOnlySet<IClass> AllowedClasses { get; }

    private IAccessControl AccessControl { get; }

    private Func<IValidation> Derive { get; }

    internal void Execute(MethodRequest[] methods, BatchOptions options)
    {
        var isolated = options?.Isolated ?? false;
        var continueOnError = options?.ContinueOnError ?? false;

        if (isolated)
        {
            foreach (var method in methods)
            {
                var error = this.Execute(method);

                if (error)
                {
                    this.Transaction.Rollback();
                    if (!continueOnError)
                    {
                        break;
                    }
                }
                else
                {
                    this.Transaction.Commit();
                }
            }
        }
        else
        {
            var error = false;
            foreach (var method in methods)
            {
                error = this.Execute(method);

                if (error)
                {
                    break;
                }
            }

            if (error)
            {
                this.Transaction.Rollback();
            }
            else
            {
                this.Transaction.Commit();
            }
        }

        if (!this.HasErrors)
        {
            this.Transaction.Commit();
        }
    }

    private bool Execute(MethodRequest invocation)
    {
        var obj = this.Transaction.Instantiate(invocation.Object.Id);
        if (obj == null)
        {
            this.AddMissingId(invocation.Object.Id);
            return true;
        }

        var localStrategy = (Object)invocation.Object;

        if (this.AllowedClasses?.Contains(obj.Strategy.Class) != true)
        {
            this.AddAccessError(localStrategy);
            return true;
        }

        var composite = (IComposite)obj.Strategy.Class;

        // TODO: Cache and filter for workspace
        var methodTypes = composite.MethodTypes.Where(v => v.WorkspaceNames.Any());
        var methodType = methodTypes.FirstOrDefault(x => x.Tag.Equals(invocation.MethodType.Tag));

        if (methodType == null)
        {
            throw new Exception("Method " + invocation.MethodType + " not found.");
        }

        if (!localStrategy.Version.Equals(obj.Strategy.ObjectVersion))
        {
            this.AddVersionError(localStrategy.Id);
            return true;
        }

        var acl = this.AccessControl[obj];
        if (!acl.CanExecute(methodType))
        {
            this.AddAccessError(localStrategy);
            return true;
        }

        var method = obj.GetType().GetMethod(methodType.Name);

        try
        {
            var parameterLength = method.GetParameters().Length;
            var args = parameterLength > 0 ? new object[method.GetParameters().Length] : null;
            method.Invoke(obj, args);
        }
        catch (Exception e)
        {
            var innerException = e;
            while (innerException.InnerException != null)
            {
                innerException = innerException.InnerException;
            }

            this.ErrorMessage = innerException.Message;
            return true;
        }

        var derivationResult = this.Derive();
        if (!derivationResult.HasErrors)
        {
            return false;
        }

        this.AddDerivationErrors(derivationResult.Errors);
        return true;
    }
}
