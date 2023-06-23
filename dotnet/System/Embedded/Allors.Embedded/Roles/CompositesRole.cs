namespace Allors.Embedded
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Meta;

    public class CompositesRole<TRole> : Role where TRole : IEmbeddedObject
    {
        public CompositesRole(EmbeddedObject @object, IEmbeddedRoleType roleType) : base(@object, roleType)
        {
        }

        public TRole[] Value
        {
            get
            {
                return ((EmbeddedObject[])this.Object.Population.GetRoleValue(this.Object, this.RoleType))?.Cast<TRole>().ToArray() ?? Array.Empty<TRole>();
            }
            set
            {
                this.Object.Population.SetRoleValue(this.Object, this.RoleType, value);
            }
        }

        public void Add(TRole value)
        {
            this.Object.Population.AddRoleValue(this.Object, this.RoleType, value);
        }

        public void Remove(TRole value)
        {
            this.Object.Population.RemoveRoleValue(this.Object, this.RoleType, value);
        }
    }
}
