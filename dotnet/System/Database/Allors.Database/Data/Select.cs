// <copyright file="Select.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Data;

using System.Text;
using Allors.Database.Meta;

public class Select : IVisitable
{
    public Select()
    {
    }

    public Select(params RelationEndType[] relationEndTypes) : this(relationEndTypes, 0)
    {
    }

    private Select(RelationEndType[] relationEndTypes, int index)
    {
        if (relationEndTypes?.Length > 0)
        {
            this.RelationEndType = relationEndTypes[index];

            var nextIndex = index + 1;
            if (nextIndex < relationEndTypes.Length)
            {
                this.Next = new Select(relationEndTypes, nextIndex);
            }
        }
    }

    public Node[] Include { get; set; }

    public RelationEndType RelationEndType { get; set; }

    public Composite OfType { get; set; }

    public Select Next { get; set; }

    public bool IsOne
    {
        get
        {
            if (this.RelationEndType.IsMany)
            {
                return false;
            }

            return this.ExistNext ? this.Next.IsOne : this.RelationEndType.IsOne;
        }
    }

    public bool ExistNext => this.Next != null;

    public Select End => this.ExistNext ? this.Next.End : this;

    public void Accept(IVisitor visitor) => visitor.VisitSelect(this);

    public ObjectType GetObjectType()
    {
        if (this.ExistNext)
        {
            return this.Next.GetObjectType();
        }

        return this.RelationEndType?.ObjectType;
    }

    public override string ToString()
    {
        var name = new StringBuilder();
        name.Append(this.RelationEndType.Name);
        if (this.ExistNext)
        {
            this.Next.ToStringAppendToName(name);
        }

        return name.ToString();
    }

    private void ToStringAppendToName(StringBuilder name)
    {
        name.Append("." + this.RelationEndType.Name);

        if (this.ExistNext)
        {
            this.Next.ToStringAppendToName(name);
        }
    }

    public static bool TryParse(Composite composite, string selectString, out Select select)
    {
        var relationEndType = Resolve(composite, selectString);
        select = relationEndType == null ? null : new Select(relationEndType);
        return select != null;
    }

    private static RelationEndType Resolve(Composite composite, string propertyName)
    {
        var lowerCasePropertyName = propertyName.ToLowerInvariant();

        foreach (var roleType in composite.RoleTypes)
        {
            if (roleType.SingularName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                roleType.SingularFullName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                roleType.PluralName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                roleType.PluralFullName.ToLowerInvariant().Equals(lowerCasePropertyName))
            {
                return roleType;
            }
        }

        foreach (var associationType in composite.AssociationTypes)
        {
            if (associationType.SingularName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                associationType.SingularFullName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                associationType.PluralName.ToLowerInvariant().Equals(lowerCasePropertyName) ||
                associationType.PluralFullName.ToLowerInvariant().Equals(lowerCasePropertyName))
            {
                return associationType;
            }
        }

        return null;
    }
}
