namespace Allors.Meta.Generation.Model
{
    using Database.Meta;

    public abstract class MetaObjectModel
    {
        protected MetaObjectModel(MetaModel metaModel) => this.MetaModel = metaModel;

        public MetaModel MetaModel { get; }

        protected abstract IMetaObject MetaObject { get; }

        public override string ToString() => this.MetaObject.ToString();
    }
}
