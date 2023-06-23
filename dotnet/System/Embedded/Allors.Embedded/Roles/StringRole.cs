namespace Allors.Embedded
{
    using System;
    using Meta;

    public class StringRole : Role
    {
        public StringRole(EmbeddedObject @object, IEmbeddedRoleType roleType) : base(@object, roleType)
        {
        }

        public String Value
        {
            get
            {
                return (string)this.Object.Population.GetRoleValue(this.Object, this.RoleType);
            }
            set
            {
                this.Object.Population.SetRoleValue(this.Object, this.RoleType, value);
            }
        }
    }
}
