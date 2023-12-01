// <copyright file="UniquelyIdentifiableExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public class XmlSerializer
    {
        public StringComparer ObjectTypeNameComparer { get; } = StringComparer.OrdinalIgnoreCase;

        public StringComparer RoleTypeNameComparer { get; } = StringComparer.OrdinalIgnoreCase;

        public XDocument Serialize(IEnumerable<IObject> objects)
        {
            return new XDocument(new XElement("population",
                objects
                    .GroupBy(v => v.Strategy.Class, v => v.Strategy)
                    .OrderBy(v => v.Key.Name, this.ObjectTypeNameComparer)
                    .Select(groupedByClass =>
                {
                    var @class = groupedByClass.Key;
                    return new XElement(@class.PluralName, groupedByClass.Select(strategy =>
                        new XElement(@class.SingularName, @class.RoleTypes
                            .Where(roleType => roleType.ObjectType.IsUnit && !roleType.RelationType.IsDerived && strategy.ExistRole(roleType))
                            .OrderBy(roleType => roleType.Name, this.RoleTypeNameComparer)
                            .Select(roleType => new XElement(roleType.Name, strategy.GetUnitRole(roleType))))
                    ));
                })));
        }

        public void Deserialize(ITransaction transaction, XDocument document)
        {
        }
    }
}
