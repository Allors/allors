namespace Allors.Database.Meta.Extensions;

public static class IConcreteRoleTypeExtensions
{
    public static bool? RequiredExtension(this IConcreteRoleType @this)
    {
        return @this.Extensions.Required;
    }

    public static void RequiredExtension(this IConcreteRoleType @this, bool? value)
    {
        @this.Extensions.Required = value;
    }

    public static bool RequiredOverridden(this IConcreteRoleType @this) => @this.RequiredExtension() ?? false;

    public static bool Required(this IConcreteRoleType @this) => @this.RoleType.Required() || @this.RequiredOverridden();

    public static bool? UniqueExtension(this IConcreteRoleType @this)
    {
        return @this.Extensions.Unique;
    }

    public static void UniqueExtension(this IConcreteRoleType @this, bool? value)
    {
        @this.Extensions.Unique = value;
    }

    public static bool UniqueOverriden(this IConcreteRoleType @this) => @this.UniqueExtension() ?? false;

    public static bool Unique(this IConcreteRoleType @this) => @this.RoleType.Unique() || @this.UniqueOverriden();
}
