namespace Allors.Workspace.Mvvm.Generator;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

[Generator]
public class ViewModelGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context) { }

    public void Execute(GeneratorExecutionContext context)
    {
        var attributeSymbol = context.Compilation.GetTypeByMetadataName("Allors.Workspace.Mvvm.Generator.AdapterPropertyAttribute");
        var propertyCodeBuilder = new Dictionary<string, (string Namespace, StringBuilder Code)>();

        foreach (var syntaxTree in context.Compilation.SyntaxTrees)
        {
            var model = context.Compilation.GetSemanticModel(syntaxTree);

            var fieldDeclarations = syntaxTree.GetRoot().DescendantNodes()
                .OfType<FieldDeclarationSyntax>()
                .Where(fieldSyntax => fieldSyntax.AttributeLists
                    .SelectMany(attrList => attrList.Attributes)
                    .Any(attr => model.GetTypeInfo(attr).Type?.Equals(attributeSymbol) == true));

            foreach (var fieldDeclaration in fieldDeclarations)
            {
                var className = (fieldDeclaration.Parent as TypeDeclarationSyntax)?.Identifier.ValueText;
                if (className == null)
                {
                    continue;
                }

                var namespaceName = (fieldDeclaration.Parent?.Parent as NamespaceDeclarationSyntax)?.Name?.ToString()
                   ?? (fieldDeclaration.Parent?.Parent as FileScopedNamespaceDeclarationSyntax)?.Name?.ToString();

                var propertyName = fieldDeclaration.Declaration.Variables.First().Identifier.ValueText;

                var fieldTypeSymbol = fieldDeclaration.Declaration.Type as GenericNameSyntax;
                var dataType = fieldTypeSymbol.TypeArgumentList.Arguments.First().ToString();

                var generatedPropertyCode = GeneratePropertyCode(propertyName, dataType);
                if (!propertyCodeBuilder.ContainsKey(className))
                {
                    propertyCodeBuilder[className] = (namespaceName, new StringBuilder());
                }

                propertyCodeBuilder[className].Code.Append(generatedPropertyCode);
            }
        }

        foreach (var kvp in propertyCodeBuilder)
        {
            var className = kvp.Key;
            var namespaceName = kvp.Value.Namespace;
            var generatedProperties = kvp.Value.Code.ToString();

            var generatedCode =
$@"using System;

namespace {namespaceName};

public partial class {className}
{{
{generatedProperties}
}}
";

            var fileName = className + ".g.cs";
            context.AddSource(fileName, SourceText.From(generatedCode, Encoding.UTF8));
        }
    }

    private string GeneratePropertyCode(string propertyName, string dataType)
    {
        var generatedPropertyName = propertyName.TrimStart().Substring(0, 1).ToUpperInvariant() + propertyName.Substring(1);

        return $@"    public {dataType} {generatedPropertyName}
    {{
        get => this.{propertyName}.Value;
        set => this.{propertyName}.Value = value;
    }}";
    }
}
