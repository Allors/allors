namespace Allors.Database.Meta.Extensions;

public static class IRoleTypeExtensions
{
    public static bool IsRequired(this RoleType @this) => @this.Attributes.IsRequired;

    public static bool IsUnique(this RoleType @this) => @this.Attributes.IsUnique;

    public static string MediaType(this RoleType @this) => @this.Attributes.MediaType;

    public static bool IsIndexed(this RoleType @this) => @this.Attributes.IsIndexed ?? false;
}
