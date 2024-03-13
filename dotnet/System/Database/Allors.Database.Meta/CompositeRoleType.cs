namespace Allors.Database.Meta;

public sealed class CompositeRoleType : IMetaExtensible
{
    public CompositeRoleType(Composite composite, RoleType roleType)
    {
        this.Composite = composite;
        this.RoleType = roleType;
    }

    public Composite Composite { get; }

    public RoleType RoleType { get; }

    public MetaPopulation MetaPopulation => this.Composite.MetaPopulation;

    public bool? AssignedIsRequired { get; set; }

    public bool? AssignedIsUnique { get; set; }
    
    public bool IsRequired => this.AssignedIsRequired ?? this.RoleType.IsRequired;

    public bool IsUnique => this.AssignedIsUnique ?? this.RoleType.IsUnique;
}
