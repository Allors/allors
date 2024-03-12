namespace Allors.Database.Meta.Extensions;

public static class ICompositeRoleTypeExtensions
{
    public static bool IsRequired(this CompositeRoleType @this) => @this.Attributes.IsRequired ?? @this.RoleType.IsRequired();

    public static bool IsUnique(this CompositeRoleType @this) => @this.Attributes.IsUnique ?? @this.RoleType.IsUnique();
}
