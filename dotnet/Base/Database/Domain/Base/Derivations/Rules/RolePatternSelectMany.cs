namespace Allors.Database.Domain.Derivations.Rules;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Database.Derivations;
using Meta;

public class RolePatternSelectMany<T, TResult> : IRolePattern
    where T : class, IObject
    where TResult : class, IObject
{
    public RolePatternSelectMany(RoleType roleType, Expression<Func<T, IEnumerable<TResult>>> selectMany)
    {
        this.RoleType = roleType;
        this.SelectMany = selectMany;
    }

    public RoleType RoleType { get; }

    public Expression<Func<T, IEnumerable<TResult>>> SelectMany { get; }

    public IEnumerable<IObject> Eval(IObject association)
    {
        if (association is not T @object)
        {
            yield break;
        }

        var lambda = this.SelectMany.Compile();
        IEnumerable<TResult> results = [];

        try
        {
            results = lambda(@object);
        }
        catch (NullReferenceException)
        {
        }

        foreach (var result in results)
        {
            if (result != null)
            {
                yield return result;
            }
        }
    }
}
