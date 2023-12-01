// <copyright file="UniquelyIdentifiableExtension.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using Meta;

    public class XmlSerializer
    {
        public StringComparer ObjectTypeNameComparer { get; } = StringComparer.OrdinalIgnoreCase;

        public StringComparer RoleTypeNameComparer { get; } = StringComparer.OrdinalIgnoreCase;

        public XDocument Serialize(IEnumerable<IObject> objects)
        {
            return new XDocument(new XElement("population", objects
                .GroupBy(v => v.Strategy.Class, v => v.Strategy)
                .OrderBy(v => v.Key.Name, this.ObjectTypeNameComparer)
                .Select(Class)));

            XElement Class(IGrouping<IClass, IStrategy> grouping) =>
               new(grouping.Key.PluralName,
                   grouping.Select(Object(grouping.Key)));

            Func<IStrategy, XElement> Object(IClass @class) => strategy =>
                new XElement(@class.SingularName,
                    @class.RoleTypes
                        .Where(roleType => roleType.ObjectType.IsUnit && !roleType.RelationType.IsDerived && strategy.ExistRole(roleType))
                        .OrderBy(roleType => roleType.Name, this.RoleTypeNameComparer)
                        .Select(Role(strategy)));

            Func<IRoleType, XElement> Role(IStrategy strategy) => roleType =>
                new XElement(roleType.Name, this.WriteString(roleType, strategy.GetUnitRole(roleType)));
        }

        public void Deserialize(ITransaction transaction, XDocument document)
        {
        }

        public string WriteString(IRoleType roleType, object unit) =>
            roleType.ObjectType.Tag switch
            {
                UnitTags.String => (string)unit,
                UnitTags.Integer => XmlConvert.ToString((int)unit),
                UnitTags.Decimal => XmlConvert.ToString((decimal)unit),
                UnitTags.Float => XmlConvert.ToString((double)unit),
                UnitTags.Boolean => XmlConvert.ToString((bool)unit),
                UnitTags.DateTime => XmlConvert.ToString((DateTime)unit, XmlDateTimeSerializationMode.Utc),
                UnitTags.Unique => XmlConvert.ToString((Guid)unit),
                UnitTags.Binary => Convert.ToBase64String((byte[])unit),
                _ => throw new ArgumentException("Unknown Unit Role Type: " + roleType),
            };

        public object ReadString(IRoleType roleType, string value) =>
            roleType.ObjectType.Tag switch
            {
                UnitTags.String => value,
                UnitTags.Integer => XmlConvert.ToInt32(value),
                UnitTags.Decimal => XmlConvert.ToDecimal(value),
                UnitTags.Float => XmlConvert.ToDouble(value),
                UnitTags.Boolean => XmlConvert.ToBoolean(value),
                UnitTags.DateTime => XmlConvert.ToDateTime(value, XmlDateTimeSerializationMode.Utc),
                UnitTags.Unique => Guid.Parse(value),
                UnitTags.Binary => Convert.FromBase64String(value),
                _ => throw new ArgumentException("Unknown Unit Role Type: " + roleType),
            };
    }
}
