namespace Allors.Database.Meta.Extensions;

public static class IRoleTypeExtensions
{
    public static bool IsRequired(this RoleType @this) => @this.Attributes.IsRequired;

    public static bool IsRequired(this RoleType @this, bool value) => @this.Attributes.IsRequired = value;

    public static bool IsUnique(this RoleType @this) => @this.Attributes.IsUnique;

    public static bool IsUnique(this RoleType @this, bool value) => @this.Attributes.IsUnique = value;

}
