namespace Allors.Database.Meta.Extensions;

public static class IRelationTypeExtensions
{
    public static string MediaType(this RelationType @this) => @this.Attributes.MediaType;

    public static bool IsIndexed(this RelationType @this) => @this.Attributes.IsIndexed ?? false;
}
