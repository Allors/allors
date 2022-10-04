namespace Allors.Database.Meta.Extensions;

public static class ICompositeRoleTypeExtensions
{
    public static bool? RequiredExtension(this ICompositeRoleType @this)
    {
        return @this.Extensions.Required;
    }

    public static void RequiredExtension(this ICompositeRoleType @this, bool? value)
    {
        @this.Extensions.Required = value;
    }

    public static bool RequiredOverridden(this ICompositeRoleType @this) => @this.RequiredExtension() ?? false;

    public static bool Required(this ICompositeRoleType @this) => @this.RoleType.Required() || @this.RequiredOverridden();

    public static bool? UniqueExtension(this ICompositeRoleType @this)
    {
        return @this.Extensions.Unique;
    }

    public static void UniqueExtension(this ICompositeRoleType @this, bool? value)
    {
        @this.Extensions.Unique = value;
    }

    public static bool UniqueOverriden(this ICompositeRoleType @this) => @this.UniqueExtension() ?? false;

    public static bool Unique(this ICompositeRoleType @this) => @this.RoleType.Unique() || @this.UniqueOverriden();
}
