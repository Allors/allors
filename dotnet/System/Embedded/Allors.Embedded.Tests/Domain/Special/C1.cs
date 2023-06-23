namespace Allors.Embedded.Tests.Domain
{
    using Allors.Embedded.Meta;

    public class C1 : EmbeddedObject
    {
        public C1(EmbeddedPopulation population, EmbeddedObjectType objectType)
           : base(population, objectType)
        {
        }

        public string Same
        {
            get { return (string)this.GetRoleValue(nameof(Same)); }
            set { this.SetRoleValue(nameof(Same), value); }
        }
    }
}
