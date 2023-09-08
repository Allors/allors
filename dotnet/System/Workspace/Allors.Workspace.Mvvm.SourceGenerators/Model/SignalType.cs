namespace Allors.Workspace.Mvvm.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SignalType
{
    public SignalType(SemanticModel semanticModel, FieldDeclarationSyntax fieldDeclarationSyntax)
    {
        this.TypeSyntax = fieldDeclarationSyntax.Declaration.Type;
        var genericNameSyntax = (GenericNameSyntax)this.TypeSyntax;
        var argument = genericNameSyntax.TypeArgumentList.Arguments.First();
        this.ArgumentType = new ArgumentType(semanticModel, argument);
    }

    public TypeSyntax TypeSyntax { get; }

    public ArgumentType ArgumentType { get; }

    public bool IsValueSignal => this.TypeSyntax.ToString().StartsWith("IValueSignal<");

    public bool IsComputedSignal => this.TypeSyntax.ToString().StartsWith("IComputedSignal<");

    public string Name => this.TypeSyntax.ToFullString();
}
