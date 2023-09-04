namespace Allors.Workspace.Mvvm.Generator;

using System;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class Field
{
    public Class Class { get; }

    public FieldDeclarationSyntax FieldDeclarationSyntax { get; }

    public Field(Class @class, FieldDeclarationSyntax fieldDeclarationSyntax)
    {
        this.Class = @class;
        this.FieldDeclarationSyntax = fieldDeclarationSyntax;

        var semanticModel = this.Class.Source.SemanticModel;
        this.Symbol = (IFieldSymbol)semanticModel.GetDeclaredSymbol(fieldDeclarationSyntax.Declaration.Variables.FirstOrDefault()!);
        this.Name = this.Symbol.Name;

        this.FieldType = new FieldType(semanticModel, this.FieldDeclarationSyntax);
    }

    public IFieldSymbol Symbol { get; }

    public string Name { get; }

    public string PropertyName => string.Concat(this.Name[0].ToString().ToUpper(), this.Name.Substring(1));

    public FieldType FieldType { get; set; }

    public AttributeInstance[] AttributeInstances { get; private set; }

    public void Build()
    {
        this.AttributeInstances = this.Symbol.GetAttributes()
            .Select(attributeData => new AttributeInstance(this, attributeData))
            .ToArray();

        foreach (var attributeInstance in this.AttributeInstances)
        {
            attributeInstance.Build();
        }
    }

    public string Generate()
    {
        var typeName = this.FieldType.GenericTypeInfo.Type.Name;

        var fieldType = this.FieldType.NestedFieldType;
        var nestedFieldType = fieldType.NestedFieldType;

        if (nestedFieldType == null)
        {
            if(typeName == "IValueSignal")
            {
                return $@"    public {fieldType.Name} {this.PropertyName}
    {{
        get => this.{this.Name}.Value;
        set => this.{this.Name}.Value = value;
    }}";
            }
            else
            {
                return $@"    public {fieldType.Name} {this.PropertyName}
    {{
        get => this.{this.Name}.Value;
    }}";
            }
        }
        else
        {
            return $@"    public {nestedFieldType.Name} {this.PropertyName}
    {{
        get => this.{this.Name}.Value?.Value;
        set {{ if(this.{this.Name}.Value != null) this.{this.Name}.Value.Value = value; }}
    }}";

        }

    }
}
