namespace Allors.Database.Meta;

using System.Linq;
using Embedded;
using Text;

public class ObjectTypePluralNameDerivation : IEmbeddedDerivation
{
    public void EmbeddedDerive(IEmbeddedChangeSet changeSet)
    {
        var singularNames = changeSet.EmbeddedChangedRoles<ObjectType>(nameof(ObjectType.SingularName));
        var assignedPluralNames = changeSet.EmbeddedChangedRoles<ObjectType>(nameof(ObjectType.AssignedPluralName));

        if (singularNames.Any() || assignedPluralNames.Any())
        {
            var objectTypes = singularNames
                .Union(assignedPluralNames)
                .Select(v => v.Key)
                .OfType<ObjectType>()
                .Distinct(); ;

            foreach (var @this in objectTypes)
            {
                @this.PluralName = @this.AssignedPluralName ?? Pluralizer.Pluralize(@this.SingularName);
            }
        }
    }
}
