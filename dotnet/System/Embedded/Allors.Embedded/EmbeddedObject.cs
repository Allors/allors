namespace Allors.Embedded
{
    using Meta;

    public class EmbeddedObject 
    {
        public EmbeddedPopulation Population { get; }

        public EmbeddedObjectType ObjectType { get; }

        protected EmbeddedObject(EmbeddedPopulation population, EmbeddedObjectType objectType)
        {
            this.Population = population;
            this.ObjectType = objectType;
        }
    }
}
