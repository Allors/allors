namespace Generate.Model
{
    using System;
    using Allors;
    using Allors.Repository;
    using Allors.Repository.Domain;

    public class UnitModel : ObjectTypeModel
    {
        public UnitModel(RepositoryModel repositoryModel, Unit unit) : base(repositoryModel) => this.Unit = unit;

        public Unit Unit { get; }

        protected override RepositoryObject RepositoryObject => this.Unit;

        public override FieldObjectType FieldObjectType => this.Unit;

        public override ObjectType ObjectType => this.Unit;

        public override string Id =>
            this.SingularName switch
            {
                UnitNames.Binary => UnitIds.Binary.ToString(),
                UnitNames.Boolean => UnitIds.Boolean.ToString(),
                UnitNames.DateTime => UnitIds.DateTime.ToString(),
                UnitNames.Decimal => UnitIds.Decimal.ToString(),
                UnitNames.Float => UnitIds.Float.ToString(),
                UnitNames.Integer => UnitIds.Integer.ToString(),
                UnitNames.String => UnitIds.String.ToString(),
                UnitNames.Unique => UnitIds.Unique.ToString(),
                _ => throw new Exception($"Unknown unit {this.SingularName}")
            };
    }
}
