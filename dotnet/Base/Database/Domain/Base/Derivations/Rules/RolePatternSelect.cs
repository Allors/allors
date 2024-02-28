namespace Allors.Database.Domain.Derivations.Rules;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Database.Derivations;
using Meta;

public class RolePatternSelect<T, TResult> : IRolePattern
    where T : class, IObject
    where TResult : class, IObject
{
    public RolePatternSelect(RoleType roleType, Expression<Func<T, TResult>> select)
    {
        this.RoleType = roleType;
        this.Select = select;
    }

    public RoleType RoleType { get; }

    public Expression<Func<T, TResult>> Select { get; }

    public Expression<Func<T, IEnumerable<TResult>>> SelectMany { get; }

    public IEnumerable<IObject> Eval(IObject association)
    {
        if (association is not T @object)
        {
            yield break;
        }

        var lambda = this.Select.Compile();

        TResult result = default;
        try
        {
            result = lambda(@object);
        }
        catch (NullReferenceException)
        {
        }

        if (result != null)
        {
            yield return result;
        }
    }
}
