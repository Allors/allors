namespace Allors.Workspace.Meta
{
    using System.Collections.Generic;
    using System.Dynamic;

    internal class MetaExtension : DynamicObject
    {
        private Dictionary<string, object> properties;

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if (this.properties != null && this.properties.ContainsKey(binder.Name))
            {
                result = this.properties[binder.Name];
            }
            else
            {
                result = null;
            }

            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            this.properties ??= new Dictionary<string, object>();
            this.properties[binder.Name] = value;
            return true;
        }
    }
}
