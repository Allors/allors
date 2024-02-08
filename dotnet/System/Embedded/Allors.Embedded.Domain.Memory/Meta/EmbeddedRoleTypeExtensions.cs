namespace Allors.Embedded.Domain.Memory.Meta
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Embedded.Meta;

    public static class EmbeddedRoleTypeExtenstions
    {
        public static object? Normalize(this EmbeddedRoleType @this, object? value) =>
            @this.IsOne switch
            {
                true => @this.NormalizeToOne(value),
                _ => @this.NormalizeToMany(value)
            };

        private static object? NormalizeToOne(this EmbeddedRoleType @this, object? value)
        {
            if (value != null)
            {
                var type = @this.ObjectType.Type;
                if (!type.IsInstanceOfType(value))
                {
                    throw new ArgumentException($"{@this.Name} should be a {type.Name} but was a {value.GetType()}");
                }
            }

            return value;
        }

        private static object? NormalizeToMany(this EmbeddedRoleType @this, object? value)
        {
            return value switch
            {
                null => null,
                ICollection collection => @this.NormalizeToMany(collection).ToArray(),
                _ => throw new ArgumentException($"{value.GetType()} is not a collection Type")
            };
        }

        private static IEnumerable<IEmbeddedObject> NormalizeToMany(this EmbeddedRoleType @this, ICollection role)
        {
            foreach (IEmbeddedObject @object in role)
            {
                if (@object != null)
                {
                    var type = @this.ObjectType.Type;

                    if (!type.IsInstanceOfType(@object))
                    {
                        throw new ArgumentException($"{@this.Name} should be a {type.Name} but was a {@object.GetType()}");
                    }

                    yield return @object;
                }
            }
        }
    }
}
