namespace Allors.Workspace.Mvvm.Generator;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

public class Class
{
    public Class(Source source, ClassDeclarationSyntax classDeclarationSyntax)
    {
        this.Source = source;
        this.ClassDeclarationSyntax = classDeclarationSyntax;

        var semanticModel = this.Source.SemanticModel;
        this.TypeModel = (ITypeSymbol)semanticModel.GetDeclaredSymbol(classDeclarationSyntax);
        this.Symbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax);
        this.Name = this.Symbol.Name;
        this.NamespaceSymbol = this.Symbol.ContainingNamespace;
    }

    public Source Source { get; }

    public ClassDeclarationSyntax ClassDeclarationSyntax { get; }

    public ITypeSymbol TypeModel { get; }

    public ISymbol Symbol { get; }

    public INamespaceSymbol NamespaceSymbol { get; }

    public string NamespaceFullyQualifiedName => this.NamespaceSymbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

    public string Name { get; }

    public IEnumerable<Field> Fields { get; private set; }

    public IEnumerable<Field> SignalFields => this.Fields
        .Where(v => v.AttributeInstances.Any(w => w.AttributeData.AttributeClass.Equals(this.Source.Project.AttributeNamedTypeSymbol)));

    public void Build()
    {
        this.Fields = this.ClassDeclarationSyntax.DescendantNodes().OfType<FieldDeclarationSyntax>().Select(fieldDeclarationSyntax => new Field(this, fieldDeclarationSyntax)).ToArray();

        foreach (var field in this.Fields)
        {
            field.Build();
        }
    }

    public void Generate()
    {
        var context = this.Source.Project.Context;

        var fileName = this.Name + ".g.cs";

        var generatedProperties = string.Join("\n", this.SignalFields.Select(v => v.Generate()));

        var code =
            $@"using System;

namespace {this.NamespaceFullyQualifiedName};

public partial class {this.Name}
{{
{generatedProperties}
}}
";

        context.AddSource(fileName, SourceText.From(code, Encoding.UTF8));
    }
}
