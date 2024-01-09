namespace Allors.Database.Meta.Extensions;

public static class ICompositeRoleTypeExtensions
{
    public static bool IsRequired(this ICompositeRoleType @this) => @this.Attributes.IsRequired ?? @this.RoleType.IsRequired();

    public static bool IsRequired(this ICompositeRoleType @this, bool value) => @this.Attributes.IsRequired = value;

    public static bool IsUnique(this ICompositeRoleType @this) => @this.Attributes.IsUnique ?? @this.RoleType.IsUnique();

    public static bool IsUnique(this ICompositeRoleType @this, bool value) => @this.Attributes.IsUnique = value;
}
