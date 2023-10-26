namespace Allors.Workspace.Mvvm.Generator;

using System;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SignalType
{
    public SignalType(SemanticModel semanticModel, FieldDeclarationSyntax fieldDeclarationSyntax)
    {
        this.TypeSyntax = fieldDeclarationSyntax.Declaration.Type;
        if (this.TypeSyntax is GenericNameSyntax genericNameSyntax)
        {
            var argument = genericNameSyntax.TypeArgumentList.Arguments.First();
            this.ArgumentType = new ArgumentType(semanticModel, argument);
        }
    }

    public TypeSyntax TypeSyntax { get; }

    public ArgumentType ArgumentType { get; }

    public bool IsValueSignal => this.TypeSyntax.ToString().StartsWith("IValueSignal<");

    public bool IsComputedSignal => this.TypeSyntax.ToString().StartsWith("IComputedSignal<");

    public string Name => this.TypeSyntax.ToFullString();
}
