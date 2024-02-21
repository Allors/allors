namespace Allors.Meta.Generation.Model;

using Allors.Database.Meta;

public class UnitModel : ObjectTypeModel
{
    public UnitModel(Model model, Unit unit)
        : base(model) => this.Unit = unit;

    public Unit Unit { get; }

    public override IMetaIdentifiableObject MetaObject => this.Unit;

    protected override IObjectType ObjectType => this.Unit;

    // IUnit
    public bool IsBinary => this.Unit.IsBinary;

    public bool IsBoolean => this.Unit.IsBoolean;

    public bool IsDateTime => this.Unit.IsDateTime;

    public bool IsDecimal => this.Unit.IsDecimal;

    public bool IsFloat => this.Unit.IsFloat;

    public bool IsInteger => this.Unit.IsInteger;

    public bool IsString => this.Unit.IsString;

    public bool IsUnique => this.Unit.IsUnique;
}
