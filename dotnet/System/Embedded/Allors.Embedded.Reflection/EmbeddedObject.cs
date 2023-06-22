namespace Allors.Embedded
{
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    public class EmbeddedObject
    {
        protected EmbeddedObject(EmbeddedPopulation population, EmbeddedObjectType objectType)
        {
            this.Population = population;
            this.ObjectType = objectType;
        }

        public EmbeddedPopulation Population { get; }

        public EmbeddedObjectType ObjectType { get; }

        public object GetRole(string name) => this.Population.GetRole(this, this.ObjectType.RoleTypeByName[name]);

        public void SetRole(string name, object value) => this.Population.SetRole(this, this.ObjectType.RoleTypeByName[name], value);

        public void AddRole(string name, EmbeddedObject value) => this.Population.AddRole(this, this.ObjectType.RoleTypeByName[name], value);

        public void RemoveRole(string name, EmbeddedObject value) => this.Population.RemoveRole(this, this.ObjectType.RoleTypeByName[name], value);

        public object GetAssociation(string name) => this.Population.GetAssociation(this, this.ObjectType.AssociationTypeByName[name]);

        public bool TryInvokeMember(string name, object[] args, out object result)
        {
            result = null;

            if (name.StartsWith("Add") && this.ObjectType.RoleTypeByName.TryGetValue(name.Substring(3), out var roleType))
            {
                this.Population.AddRole(this, roleType, (EmbeddedObject)args[0]);
                return true;
            }

            if (name.StartsWith("Remove") && this.ObjectType.RoleTypeByName.TryGetValue(name.Substring(6), out roleType))
            {
                // TODO: RemoveAll
                this.Population.RemoveRole(this, roleType, (EmbeddedObject)args[0]);
                return true;
            }

            return false;
        }

        public IEnumerable<string> GetEmbeddedMemberNames()
        {
            var objectType = this.Population.Meta.ObjectTypeByType[this.GetType()];
            foreach (var roleType in objectType.RoleTypeByName.Values.ToArray().Distinct())
            {
                yield return roleType.Name;
            }

            foreach (var associationType in objectType.AssociationTypeByName.Values.ToArray().Distinct())
            {
                yield return associationType.Name;
            }
        }

        private bool TryGet(object nameOrType, out object result)
        {
            switch (nameOrType)
            {
            case string name:
                {
                    if (this.ObjectType.RoleTypeByName.TryGetValue(name, out var roleType))
                    {
                        return this.TryGetRole(roleType, out result);
                    }

                    if (this.ObjectType.AssociationTypeByName.TryGetValue(name, out var associationType))
                    {
                        return this.TryGetAssociation(associationType, out result);
                    }
                }

                break;

            case IEmbeddedRoleType roleType:
                return this.TryGetRole(roleType, out result);

            case IEmbeddedAssociationType associationType:
                return this.TryGetAssociation(associationType, out result);
            }

            result = null;
            return false;
        }

        private bool TryGetRole(IEmbeddedRoleType roleType, out object result)
        {
            result = this.Population.GetRole(this, roleType);
            if (result == null && roleType.IsMany)
            {
                result = roleType.ObjectType.EmptyArray;
            }

            return true;
        }

        private bool TryGetAssociation(IEmbeddedAssociationType associationType, out object result)
        {
            result = this.Population.GetAssociation(this, associationType);
            if (result == null && associationType.IsMany)
            {
                result = associationType.ObjectType.EmptyArray;
            }

            return true;
        }

        private bool TrySet(object nameOrType, object value)
        {
            switch (nameOrType)
            {
            case string name:
                {
                    if (this.ObjectType.RoleTypeByName.TryGetValue(name, out var roleType))
                    {
                        this.Population.SetRole(this, roleType, value);
                        return true;
                    }
                }

                break;

            case IEmbeddedRoleType roleType:
                this.Population.SetRole(this, roleType, value);
                return true;
            }

            return false;
        }
    }
}
