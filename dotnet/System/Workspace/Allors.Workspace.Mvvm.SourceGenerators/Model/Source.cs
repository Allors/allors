namespace Allors.Workspace.Mvvm.Generator;

using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class Source
{
    public Source(Project project, SyntaxTree syntaxTree)
    {
        this.Project = project;
        this.SyntaxTree = syntaxTree;
        this.SemanticModel = this.Project.Compilation.GetSemanticModel(this.SyntaxTree);
    }

    public Project Project { get; }

    public SyntaxTree SyntaxTree { get; }

    public SemanticModel SemanticModel { get; }

    public Class[] Classes { get; private set; }

    public void Build()
    {
        this.Classes = this.SyntaxTree.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().Select(classDeclaration => new Class(this, classDeclaration)).ToArray();

        foreach (var @class in this.Classes)
        {
            @class.Build();
        }
    }
}
