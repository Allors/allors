namespace Allors.Database.Meta.Extensions;

public static class IRoleTypeExtensions
{
    public static bool? RequiredExtension(this IRoleType @this)
    {
        return @this.Extensions.Required ?? false;
    }

    public static void RequiredExtension(this IRoleType @this, bool? value)
    {
        @this.Extensions.Required = value;
    }

    public static bool Required(this IRoleType @this) => @this.RequiredExtension() ?? false;

    public static bool? UniqueExtension(this IRoleType @this)
    {
        return @this.Extensions.Unique;
    }

    public static void Unique(this IRoleType @this, bool? value)
    {
        @this.Extensions.Unique = value;
    }

    public static bool Unique(this IRoleType @this) => @this.UniqueExtension() ?? false;
}
