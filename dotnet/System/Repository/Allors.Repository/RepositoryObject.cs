namespace Allors.Repository
{
    using System.Collections.Generic;
    using System;

    public abstract class RepositoryObject
    {
        protected RepositoryObject()
        {
            this.AttributeByName = new Dictionary<string, Attribute>();
            this.AttributesByName = new Dictionary<string, Attribute[]>();
        }

        public Dictionary<string, Attribute> AttributeByName { get; }

        public Dictionary<string, Attribute[]> AttributesByName { get; }
    }
}
