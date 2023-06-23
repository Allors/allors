namespace Allors.Embedded
{
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    public class EmbeddedObject : IEmbeddedObject
    {
        protected EmbeddedObject(EmbeddedPopulation population, EmbeddedObjectType objectType)
        {
            this.Population = population;
            this.ObjectType = objectType;
        }

        public EmbeddedPopulation Population { get; }

        public EmbeddedObjectType ObjectType { get; }

        public object GetRoleValue(string name) => this.Population.GetRoleValue(this, this.ObjectType.RoleTypeByName[name]);

        public void SetRoleValue(string name, object value) => this.Population.SetRoleValue(this, this.ObjectType.RoleTypeByName[name], value);

        public void AddRoleValue(string name, EmbeddedObject value) => this.Population.AddRoleValue(this, this.ObjectType.RoleTypeByName[name], value);

        public void RemoveRoleValue(string name, EmbeddedObject value) => this.Population.RemoveRoleValue(this, this.ObjectType.RoleTypeByName[name], value);

        public object GetAssociationValue(string name) => this.Population.GetAssociationValue(this, this.ObjectType.AssociationTypeByName[name]);

        public UnitRole<T> GetUnitRole<T>(string name) => this.Population.GetUnitRole<T>(this, this.ObjectType.RoleTypeByName[name]);

        public CompositeRole<T> GetCompositeRole<T>(string name) where T : IEmbeddedObject => this.Population.GetCompositeRole<T>(this, this.ObjectType.RoleTypeByName[name]);

        public CompositesRole<T> GetCompositesRole<T>(string name) where T : IEmbeddedObject => this.Population.GetCompositesRole<T>(this, this.ObjectType.RoleTypeByName[name]);
    }
}
