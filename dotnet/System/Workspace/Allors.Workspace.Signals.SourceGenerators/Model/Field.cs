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

        this.SignalType = new SignalType(semanticModel, this.FieldDeclarationSyntax);
    }

    public IFieldSymbol Symbol { get; }

    public string Name { get; }

    public string PropertyName => string.Concat(this.Name[0].ToString().ToUpper(), this.Name.Substring(1));

    public SignalType SignalType { get; set; }

    public AttributeInstance[] AttributeInstances { get; private set; }

    public string OnEffect => this.Class.Source.Project.Configuration.OnEffect;

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

    public string GeneratePropertyWithAttributes()
    {
        var attributes = this.GenerateAttributes();
        var property = this.GenerateProperty();

        return string.IsNullOrWhiteSpace(attributes) ? property : string.Join("\n", attributes, property); ;
    }

    public string GenerateAttributes()
    {
        var attribute = AttributeInstances.FirstOrDefault(v => v.Name.Equals("SignalPropertyAttribute"));

        if (attribute?.AttributeData.ConstructorArguments.Length > 0)
        {
            var constructorArgument = attribute.AttributeData.ConstructorArguments[0];
            if (!constructorArgument.IsNull)
            {
                return $@"    [System.ComponentModel.DisplayName(""{constructorArgument.Value}"")]";
            }
        }

        return string.Empty;
    }

    public string GenerateProperty()
    {
        var argumentType = this.SignalType.ArgumentType;

        if (this.SignalType.IsValueSignal)
        {
            return $@"    public {argumentType.FullName} {this.PropertyName}
    {{
        get => this.{this.Name}.Value;
        set => this.{this.Name}.Value = value;
    }}";

        }

        if (this.SignalType.IsComputedSignal)
        {
            var compositeRoleWrapper = argumentType.ImplementedTypes.FirstOrDefault(v => v.IsCompositeRoleWrapper);
            if (compositeRoleWrapper != null)
            {
                return $@"    public {compositeRoleWrapper.ArgumentTypes[0].FullName} {this.PropertyName}
    {{
        get => this.{this.Name}.Value?.Value;
        set {{ if(this.{this.Name}.Value != null) this.{this.Name}.Value.Value = value; }}
    }}";

            }

            var compositesRoleWrapper = argumentType.ImplementedTypes.FirstOrDefault(v => v.IsCompositesRoleWrapper);
            if (compositesRoleWrapper != null)
            {
                return $@"    public System.Collections.Generic.IEnumerable<{compositesRoleWrapper.ArgumentTypes[0].FullName}> {this.PropertyName}
    {{
        get => this.{this.Name}.Value?.Value;
        set {{ if(this.{this.Name}.Value != null) this.{this.Name}.Value.Value = value; }}
    }}";

            }

            var nestedArgumentType = argumentType.NestedArgumentType;

            if (nestedArgumentType == null)
            {
                return $@"    public {argumentType.FullName} {this.PropertyName}
    {{
        get => this.{this.Name}.Value;
    }}";
            }

            if (argumentType.IsUnitRole)
            {
                if (argumentType.IsNullable)
                {
                    if (nestedArgumentType.IsNullable)
                    {
                        return $@"    public {nestedArgumentType.FullName} {this.PropertyName}
    {{
        get => this.{this.Name}.Value?.Value;
        set {{ if(this.{this.Name}.Value != null) this.{this.Name}.Value.Value = value; }}
    }}";
                    }

                    return $@"    public {nestedArgumentType.FullName}? {this.PropertyName}
    {{
        get => this.{this.Name}.Value?.Value;
        set {{ if(this.{this.Name}.Value != null) this.{this.Name}.Value.Value = value; }}
    }}";
                }

                if (nestedArgumentType.IsNullable)
                {
                    return $@"    public {nestedArgumentType.FullName} {this.PropertyName}
    {{
        get => this.{this.Name}.Value?.Value;
        set {{ if(this.{this.Name}.Value != null) this.{this.Name}.Value.Value = value; }}
    }}";
                }

                return $@"    public {nestedArgumentType.FullName} {this.PropertyName}
    {{
        get => this.{this.Name}.Value?.Value;
        set {{ if(this.{this.Name}.Value != null) this.{this.Name}.Value.Value = value; }}
    }}";
            }

            return $@"    public {nestedArgumentType.FullName} {this.PropertyName}
    {{
        get => this.{this.Name}.Value?.Value;
        set {{ if(this.{this.Name}.Value != null) this.{this.Name}.Value.Value = value; }}
    }}";

        }

        throw new Exception("Unknown signal type");
    }

    public string GenerateEffects()
    {
        return $@"    private IEffect {this.Name}Changed;";
    }

    public string GenerateInitEffects()
    {
        return $@"        this.{this.Name}Changed = new Effect(() => this.{this.OnEffect}(nameof({this.PropertyName})), this.{this.Name});";
    }

    public string GenerateDisposeEffects()
    {
        return $@"        this.{this.Name}Changed?.Dispose();";
    }
}
