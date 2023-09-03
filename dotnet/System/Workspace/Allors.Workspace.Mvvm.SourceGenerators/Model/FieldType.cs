namespace Allors.Workspace.Mvvm.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FieldType
{
    public FieldType(SemanticModel semanticModel, FieldDeclarationSyntax fieldDeclarationSyntax)
    {
        this.TypeSyntax = fieldDeclarationSyntax.Declaration.Type;
        this.GenericNameSyntax = this.TypeSyntax as GenericNameSyntax;
        if (this.GenericNameSyntax != null)
        {
            this.GenericTypeInfo = semanticModel.GetTypeInfo(this.GenericNameSyntax);

            var argument = this.GenericNameSyntax.TypeArgumentList.Arguments.First();
            this.NestedFieldType = new FieldType(semanticModel, argument);
        }
    }

    public FieldType(SemanticModel semanticModel, TypeSyntax typeSyntax)
    {
        this.TypeSyntax = typeSyntax;

        this.GenericNameSyntax = this.TypeSyntax as GenericNameSyntax;
        if (this.GenericNameSyntax != null)
        {
            this.GenericTypeInfo = semanticModel.GetTypeInfo(this.GenericNameSyntax);

            var argument = this.GenericNameSyntax.TypeArgumentList.Arguments.First();
            this.NestedFieldType = new FieldType(semanticModel, argument);
        }

        this.NullableTypeSyntax = this.TypeSyntax as NullableTypeSyntax;
        this.NullableTypeElementType = this.NullableTypeSyntax?.ElementType;
    }

    public TypeSyntax TypeSyntax { get; }

    public GenericNameSyntax GenericNameSyntax { get; }

    public TypeInfo GenericTypeInfo { get; }

    public NullableTypeSyntax NullableTypeSyntax { get; }

    public TypeSyntax NullableTypeElementType { get; set; }

    public FieldType NestedFieldType { get; }

    public string Name => this.TypeSyntax.ToFullString();
}
