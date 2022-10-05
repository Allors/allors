namespace Allors.Database.Meta.Extensions;

public static class IRelationTypeExtensions
{
    public static string MediaType(this IRelationType @this)
    {
        return @this.Attributes.MediaType;
    }

    public static void MediaType(this IRelationType @this, string value)
    {
        @this.Attributes.MediaType = value;
    }

    public static bool? IndexedExtension(this IRelationType @this)
    {
        return @this.Attributes.Indexed;
    }

    public static void Indexed(this IRelationType @this, bool? value)
    {
        @this.Attributes.Indexed = value;
    }

    public static bool Indexed(this IRelationType @this) => @this.IndexedExtension() ?? false;
}
