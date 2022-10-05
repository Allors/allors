namespace Allors.Database.Meta.Extensions;

public static class IRelationTypeExtensions
{
    public static string MediaType(this IRelationType @this) => @this.Attributes.MediaType;

    public static bool IsIndexed(this IRelationType @this) => @this.Attributes.IsIndexed ?? false;
}
