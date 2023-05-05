namespace Allors.Database.Meta.Extensions;

public static class ICompositeRoleTypeExtensions
{
    public static bool IsRequired(this ICompositeRoleType @this) => @this.Attributes.IsRequired;

    public static bool IsUnique(this ICompositeRoleType @this) => @this.Attributes.IsUnique;
}
