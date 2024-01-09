namespace Allors.Database.Meta.Extensions;

public static class IRoleTypeExtensions
{
    public static bool IsRequired(this IRoleType @this) => @this.Attributes.IsRequired;

    public static bool IsRequired(this IRoleType @this, bool value) => @this.Attributes.IsRequired = value;

    public static bool IsUnique(this IRoleType @this) => @this.Attributes.IsUnique;

    public static bool IsUnique(this IRoleType @this, bool value) => @this.Attributes.IsUnique = value;

}
