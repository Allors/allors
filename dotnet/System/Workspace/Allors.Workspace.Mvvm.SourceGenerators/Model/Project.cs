namespace Allors.Workspace.Mvvm.Generator;

using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

public class Project
{
    private const string AttributeName = "Allors.Workspace.Mvvm.Generator.SignalPropertyAttribute";

    public Project(GeneratorExecutionContext context)
    {
        this.Context = context;
        this.Compilation = context.Compilation;
        this.AttributeNamedTypeSymbol = this.Compilation.GetTypeByMetadataName(AttributeName);
    }

    public GeneratorExecutionContext Context { get; }

    public Compilation Compilation { get; }

    public INamedTypeSymbol AttributeNamedTypeSymbol { get; }

    public Source[] Sources { get; private set; }

    public IEnumerable<Class> Classes => this.Sources
        .SelectMany(v => v.Classes);

    public void Build()
    {
        this.Sources = this.Compilation.SyntaxTrees.Select(v => new Source(this, v)).ToArray();

        foreach (var sourceModel in this.Sources)
        {
            sourceModel.Build();
        }
    }

    public void Generate()
    {
        var classes = this.Classes
            .Where(v => v.SignalFields.Any());

        foreach (var @class in classes)
        {
            @class.Generate();
        }
    }
}
