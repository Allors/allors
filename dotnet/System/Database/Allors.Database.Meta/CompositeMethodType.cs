namespace Allors.Database.Meta;

public sealed class CompositeMethodType : IMetaObject
{
    public CompositeMethodType(Composite composite, MethodType methodType)
    {
        this.Composite = composite;
        this.MethodType = methodType;
    }

    public Composite Composite { get; }

    public MethodType MethodType { get; }

    public MetaPopulation MetaPopulation => this.Composite.MetaPopulation;
}
