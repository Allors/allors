namespace Allors.Database.Meta.Extensions;

public static class IRelationTypeExtensions
{
    public static string MediaType(this IRelationType @this)
    {
        return @this.Extensions.MediaType;
    }

    public static void MediaType(this IRelationType @this, string value)
    {
        @this.Extensions.MediaType = value;
    }

    public static bool? IndexedExtension(this IRelationType @this)
    {
        return @this.Extensions.Indexed;
    }

    public static void Indexed(this IRelationType @this, bool? value)
    {
        @this.Extensions.Indexed = value;
    }

    public static bool Indexed(this IRelationType @this) => @this.IndexedExtension() ?? false;
}
