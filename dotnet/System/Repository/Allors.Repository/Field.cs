namespace Allors.Repository.Domain;

using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class Field : RepositoryObject
{
    public Field(ISet<RepositoryObject> objects, SemanticModel semanticModel, Record record, PropertyDeclarationSyntax propertyDeclaration)
    {
        this.Record = record;

        var propertySymbol = (IPropertySymbol)semanticModel.GetDeclaredSymbol(propertyDeclaration);
        this.Name = propertySymbol.Name;

        record.Fields.Add(this);

        objects.Add(this);
    }

    public string Name { get; }

    public Record Record { get; }

    public DataType Type { get; set; }

    public bool IsMany { get; set; }

    public override string ToString() => $"{this.Record.Name}.{this.Name}";
}
